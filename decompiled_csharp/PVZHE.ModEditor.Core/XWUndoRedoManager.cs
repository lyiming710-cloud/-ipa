using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWUndoRedoManager.cs")]
public class XWUndoRedoManager : RefCounted
{
	public enum HistoryType
	{
		Global,
		Scene,
		Remote
	}

	public enum MergeMode
	{
		Disabled,
		Merge,
		End
	}

	public class Action
	{
		public string Name;

		public long BeforeStateToken;

		public long AfterStateToken;

		public List<MethodCall> DoMethods = new List<MethodCall>();

		public List<MethodCall> UndoMethods = new List<MethodCall>();

		public List<PropertyChange> DoProperties = new List<PropertyChange>();

		public List<PropertyChange> UndoProperties = new List<PropertyChange>();
	}

	public class MethodCall
	{
		public GodotObject Object;

		public StringName Method;

		public Variant[] Args;
	}

	public class PropertyChange
	{
		public GodotObject Object;

		public StringName Property;

		public Variant Value;
	}

	public class History
	{
		public List<Action> Actions = new List<Action>();

		public int CurrentAction = -1;

		public long CurrentStateToken;

		public long SavedStateToken;

		public bool HasSavedState;

		public bool IsCommitting;
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName CreateHistoryScope = "CreateHistoryScope";

		public static readonly StringName HasHistory = "HasHistory";

		public static readonly StringName ReleaseHistory = "ReleaseHistory";

		public static readonly StringName CreateAction = "CreateAction";

		public static readonly StringName CommitAction = "CommitAction";

		public static readonly StringName AddDoProperty = "AddDoProperty";

		public static readonly StringName AddUndoProperty = "AddUndoProperty";

		public static readonly StringName Undo = "Undo";

		public static readonly StringName Redo = "Redo";

		public static readonly StringName IsUndoing = "IsUndoing";

		public static readonly StringName IsRedoing = "IsRedoing";

		public static readonly StringName HasUndo = "HasUndo";

		public static readonly StringName HasRedo = "HasRedo";

		public static readonly StringName GetCurrentActionName = "GetCurrentActionName";

		public static readonly StringName GetCurrentActionStateToken = "GetCurrentActionStateToken";

		public static readonly StringName GetNextRedoActionStateToken = "GetNextRedoActionStateToken";

		public static readonly StringName ClearHistory = "ClearHistory";

		public static readonly StringName SetHistoryAsSaved = "SetHistoryAsSaved";

		public static readonly StringName IsHistoryUnsaved = "IsHistoryUnsaved";

		public static readonly StringName SetCurrentHistoryType = "SetCurrentHistoryType";

		public static readonly StringName GetCurrentHistoryType = "GetCurrentHistoryType";

		public static readonly StringName GetVersion = "GetVersion";

		public static readonly StringName GetHistoryStateToken = "GetHistoryStateToken";

		public static readonly StringName ResolveHistoryType = "ResolveHistoryType";

		public static readonly StringName NextStateToken = "NextStateToken";

		public static readonly StringName NotifyHistoryChanged = "NotifyHistoryChanged";

		public static readonly StringName CommitPending = "CommitPending";

		public static readonly StringName DiscardPendingAction = "DiscardPendingAction";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName OnVersionChanged = "OnVersionChanged";

		public static readonly StringName ActionCommitted = "ActionCommitted";

		public static readonly StringName ActionUndone = "ActionUndone";

		public static readonly StringName ActionRedone = "ActionRedone";

		public static readonly StringName _currentHistoryType = "_currentHistoryType";

		public static readonly StringName _mergeMode = "_mergeMode";

		public static readonly StringName _mergeActionName = "_mergeActionName";

		public static readonly StringName _version = "_version";

		public static readonly StringName _nextDynamicHistoryId = "_nextDynamicHistoryId";

		public static readonly StringName _nextStateToken = "_nextStateToken";

		public static readonly StringName _pendingHistoryType = "_pendingHistoryType";

		public static readonly StringName _isUndoing = "_isUndoing";

		public static readonly StringName _isRedoing = "_isRedoing";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private const int FirstDynamicHistoryId = 1000;

	private readonly System.Collections.Generic.Dictionary<int, History> _histories = new System.Collections.Generic.Dictionary<int, History>();

	private int _currentHistoryType;

	private bool _mergeMode;

	private string _mergeActionName = "";

	private int _version;

	private int _nextDynamicHistoryId = 1000;

	private long _nextStateToken = 1L;

	private Action _pendingAction;

	private int _pendingHistoryType;

	private bool _isUndoing;

	private bool _isRedoing;

	public Callable OnVersionChanged { get; set; }

	public Callable ActionCommitted { get; set; }

	public Callable ActionUndone { get; set; }

	public Callable ActionRedone { get; set; }

	public event Action<int> HistoryChanged;

	public int CreateHistoryScope()
	{
		while (_histories.ContainsKey(_nextDynamicHistoryId))
		{
			_nextDynamicHistoryId++;
		}
		int num = _nextDynamicHistoryId++;
		GetOrCreateHistory(num);
		return num;
	}

	public bool HasHistory(int historyType)
	{
		return _histories.ContainsKey(historyType);
	}

	public void ReleaseHistory(int historyType)
	{
		int num = ResolveHistoryType(historyType);
		if (_pendingAction != null && _pendingHistoryType == num)
		{
			DiscardPendingAction();
		}
		if (_histories.Remove(num))
		{
			if (_currentHistoryType == num)
			{
				_currentHistoryType = 0;
			}
			_version++;
			NotifyHistoryChanged(num);
		}
	}

	public void CreateAction(string name, bool mergeMode = false, int historyType = 0)
	{
		int num = ResolveHistoryType(historyType);
		if (!mergeMode || !(_mergeActionName == name) || _pendingAction == null || _pendingHistoryType != num)
		{
			CommitPending();
			_currentHistoryType = num;
			_pendingHistoryType = num;
			_pendingAction = new Action
			{
				Name = name
			};
			_mergeMode = mergeMode;
			_mergeActionName = name;
		}
	}

	public void CommitAction()
	{
		if (_pendingAction != null)
		{
			int pendingHistoryType = _pendingHistoryType;
			History orCreateHistory = GetOrCreateHistory(pendingHistoryType);
			string name = _pendingAction.Name;
			orCreateHistory.IsCommitting = true;
			if (_mergeMode && orCreateHistory.CurrentAction >= 0 && orCreateHistory.Actions[orCreateHistory.CurrentAction].Name == _pendingAction.Name)
			{
				Action action = orCreateHistory.Actions[orCreateHistory.CurrentAction];
				action.DoMethods.AddRange(_pendingAction.DoMethods);
				action.UndoMethods.AddRange(_pendingAction.UndoMethods);
				action.DoProperties.AddRange(_pendingAction.DoProperties);
				action.UndoProperties.AddRange(_pendingAction.UndoProperties);
				action.AfterStateToken = NextStateToken();
				orCreateHistory.CurrentStateToken = action.AfterStateToken;
			}
			else
			{
				orCreateHistory.Actions.RemoveRange(orCreateHistory.CurrentAction + 1, orCreateHistory.Actions.Count - orCreateHistory.CurrentAction - 1);
				_pendingAction.BeforeStateToken = orCreateHistory.CurrentStateToken;
				_pendingAction.AfterStateToken = NextStateToken();
				orCreateHistory.Actions.Add(_pendingAction);
				orCreateHistory.CurrentAction = orCreateHistory.Actions.Count - 1;
				orCreateHistory.CurrentStateToken = _pendingAction.AfterStateToken;
			}
			ExecuteDo(_pendingAction);
			_version++;
			orCreateHistory.IsCommitting = false;
			_pendingAction = null;
			_pendingHistoryType = 0;
			_mergeMode = false;
			_mergeActionName = "";
			NotifyHistoryChanged(pendingHistoryType);
			if (!ActionCommitted.Equals(default(Callable)))
			{
				ActionCommitted.Call(name);
			}
		}
	}

	public void AddDoMethod(GodotObject obj, string method, params Variant[] args)
	{
		if (_pendingAction != null)
		{
			_pendingAction.DoMethods.Add(new MethodCall
			{
				Object = obj,
				Method = new StringName(method),
				Args = args
			});
		}
	}

	public void AddUndoMethod(GodotObject obj, string method, params Variant[] args)
	{
		if (_pendingAction != null)
		{
			_pendingAction.UndoMethods.Add(new MethodCall
			{
				Object = obj,
				Method = new StringName(method),
				Args = args
			});
		}
	}

	public void AddDoProperty(GodotObject obj, StringName property, Variant value)
	{
		if (_pendingAction != null)
		{
			_pendingAction.DoProperties.Add(new PropertyChange
			{
				Object = obj,
				Property = property,
				Value = value
			});
		}
	}

	public void AddUndoProperty(GodotObject obj, StringName property, Variant value)
	{
		if (_pendingAction != null)
		{
			_pendingAction.UndoProperties.Add(new PropertyChange
			{
				Object = obj,
				Property = property,
				Value = value
			});
		}
	}

	public bool Undo()
	{
		CommitPending();
		History history = GetHistory(_currentHistoryType);
		if (history == null || history.CurrentAction < 0)
		{
			return false;
		}
		Action action = history.Actions[history.CurrentAction];
		_isUndoing = true;
		ExecuteUndo(action);
		_isUndoing = false;
		history.CurrentAction--;
		history.CurrentStateToken = action.BeforeStateToken;
		_version++;
		NotifyHistoryChanged(_currentHistoryType);
		if (!ActionUndone.Equals(default(Callable)))
		{
			ActionUndone.Call(action.Name);
		}
		return true;
	}

	public bool Redo()
	{
		CommitPending();
		History history = GetHistory(_currentHistoryType);
		if (history == null || history.CurrentAction >= history.Actions.Count - 1)
		{
			return false;
		}
		history.CurrentAction++;
		Action action = history.Actions[history.CurrentAction];
		_isRedoing = true;
		ExecuteDo(action);
		_isRedoing = false;
		history.CurrentStateToken = action.AfterStateToken;
		_version++;
		NotifyHistoryChanged(_currentHistoryType);
		if (!ActionRedone.Equals(default(Callable)))
		{
			ActionRedone.Call(action.Name);
		}
		return true;
	}

	public bool IsUndoing()
	{
		return _isUndoing;
	}

	public bool IsRedoing()
	{
		return _isRedoing;
	}

	public bool HasUndo(int historyType = -1)
	{
		History history = GetHistory((historyType == -1) ? _currentHistoryType : ResolveHistoryType(historyType));
		if (history != null)
		{
			return history.CurrentAction >= 0;
		}
		return false;
	}

	public bool HasRedo(int historyType = -1)
	{
		History history = GetHistory((historyType == -1) ? _currentHistoryType : ResolveHistoryType(historyType));
		if (history != null)
		{
			return history.CurrentAction < history.Actions.Count - 1;
		}
		return false;
	}

	public string GetCurrentActionName()
	{
		History history = GetHistory(_currentHistoryType);
		if (history == null || history.CurrentAction < 0 || history.CurrentAction >= history.Actions.Count)
		{
			return "";
		}
		return history.Actions[history.CurrentAction].Name;
	}

	public long GetCurrentActionStateToken()
	{
		History history = GetHistory(_currentHistoryType);
		if (history == null || history.CurrentAction < 0 || history.CurrentAction >= history.Actions.Count)
		{
			return 0L;
		}
		return history.Actions[history.CurrentAction].AfterStateToken;
	}

	public long GetNextRedoActionStateToken()
	{
		History history = GetHistory(_currentHistoryType);
		int num = ((history == null) ? (-1) : (history.CurrentAction + 1));
		if (history == null || num < 0 || num >= history.Actions.Count)
		{
			return 0L;
		}
		return history.Actions[num].AfterStateToken;
	}

	public void ClearHistory(int historyType = -1)
	{
		if (historyType == -1)
		{
			DiscardPendingAction();
			List<int> list = new List<int>(_histories.Keys);
			_histories.Clear();
			_currentHistoryType = 0;
			_version++;
			{
				foreach (int item in list)
				{
					NotifyHistoryChanged(item);
				}
				return;
			}
		}
		int num = ResolveHistoryType(historyType);
		if (_pendingAction != null && _pendingHistoryType == num)
		{
			DiscardPendingAction();
		}
		if (_histories.Remove(num))
		{
			_version++;
			NotifyHistoryChanged(num);
		}
	}

	public void SetHistoryAsSaved(int historyType = -1)
	{
		if (historyType == -1)
		{
			foreach (KeyValuePair<int, History> history in _histories)
			{
				history.Value.SavedStateToken = history.Value.CurrentStateToken;
				history.Value.HasSavedState = true;
				NotifyHistoryChanged(history.Key);
			}
			return;
		}
		int num = ResolveHistoryType(historyType);
		History orCreateHistory = GetOrCreateHistory(num);
		orCreateHistory.SavedStateToken = orCreateHistory.CurrentStateToken;
		orCreateHistory.HasSavedState = true;
		NotifyHistoryChanged(num);
	}

	public bool IsHistoryUnsaved(int historyType = -1)
	{
		if (historyType == -1)
		{
			foreach (KeyValuePair<int, History> history2 in _histories)
			{
				if (IsHistoryUnsaved(history2.Value))
				{
					return true;
				}
			}
			return false;
		}
		History history = GetHistory(ResolveHistoryType(historyType));
		if (history == null)
		{
			return false;
		}
		return IsHistoryUnsaved(history);
	}

	public void SetCurrentHistoryType(int type)
	{
		CommitPending();
		_currentHistoryType = ResolveHistoryType(type);
	}

	public int GetCurrentHistoryType()
	{
		return _currentHistoryType;
	}

	public int GetVersion()
	{
		return _version;
	}

	public long GetHistoryStateToken(int historyType)
	{
		return GetHistory(ResolveHistoryType(historyType))?.CurrentStateToken ?? 0;
	}

	private History GetOrCreateHistory(int type)
	{
		if (!_histories.TryGetValue(type, out var value))
		{
			value = new History();
			_histories[type] = value;
		}
		return value;
	}

	private History GetHistory(int type)
	{
		if (!_histories.TryGetValue(type, out var value))
		{
			return null;
		}
		return value;
	}

	private int ResolveHistoryType(int requestedHistory)
	{
		if (requestedHistory == 1 && _currentHistoryType >= 1000)
		{
			return _currentHistoryType;
		}
		return requestedHistory;
	}

	private static bool IsHistoryUnsaved(History history)
	{
		if (!history.HasSavedState)
		{
			return history.CurrentAction >= 0;
		}
		return history.CurrentStateToken != history.SavedStateToken;
	}

	private long NextStateToken()
	{
		return _nextStateToken++;
	}

	private void NotifyHistoryChanged(int historyType)
	{
		if (!OnVersionChanged.Equals(default(Callable)))
		{
			OnVersionChanged.Call();
		}
		HistoryChanged?.Invoke(historyType);
	}

	private void CommitPending()
	{
		if (_pendingAction != null)
		{
			CommitAction();
		}
	}

	private void DiscardPendingAction()
	{
		_pendingAction = null;
		_pendingHistoryType = 0;
		_mergeMode = false;
		_mergeActionName = "";
	}

	private void ExecuteDo(Action action)
	{
		foreach (PropertyChange doProperty in action.DoProperties)
		{
			if (GodotObject.IsInstanceValid(doProperty.Object))
			{
				doProperty.Object.Set(doProperty.Property, doProperty.Value);
			}
		}
		foreach (MethodCall doMethod in action.DoMethods)
		{
			if (GodotObject.IsInstanceValid(doMethod.Object))
			{
				doMethod.Object.Callv(doMethod.Method, new Godot.Collections.Array(doMethod.Args));
			}
		}
	}

	private void ExecuteUndo(Action action)
	{
		foreach (PropertyChange undoProperty in action.UndoProperties)
		{
			if (GodotObject.IsInstanceValid(undoProperty.Object))
			{
				undoProperty.Object.Set(undoProperty.Property, undoProperty.Value);
			}
		}
		for (int num = action.UndoMethods.Count - 1; num >= 0; num--)
		{
			MethodCall methodCall = action.UndoMethods[num];
			if (GodotObject.IsInstanceValid(methodCall.Object))
			{
				methodCall.Object.Callv(methodCall.Method, new Godot.Collections.Array(methodCall.Args));
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(28)
		{
			new MethodInfo(MethodName.CreateHistoryScope, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "mergeMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddDoProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddUndoProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Undo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Redo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsUndoing, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRedoing, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasUndo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasRedo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentActionName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentActionStateToken, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNextRedoActionStateToken, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHistoryAsSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsHistoryUnsaved, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCurrentHistoryType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentHistoryType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetVersion, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHistoryStateToken, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveHistoryType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requestedHistory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NextStateToken, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyHistoryChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitPending, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DiscardPendingAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateHistoryScope && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CreateHistoryScope());
			return true;
		}
		if (method == MethodName.HasHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasHistory(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ReleaseHistory && args.Count == 1)
		{
			ReleaseHistory(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAction && args.Count == 3)
		{
			CreateAction(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitAction && args.Count == 0)
		{
			CommitAction();
			ret = default;
			return true;
		}
		if (method == MethodName.AddDoProperty && args.Count == 3)
		{
			AddDoProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddUndoProperty && args.Count == 3)
		{
			AddUndoProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Undo && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Undo());
			return true;
		}
		if (method == MethodName.Redo && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Redo());
			return true;
		}
		if (method == MethodName.IsUndoing && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUndoing());
			return true;
		}
		if (method == MethodName.IsRedoing && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRedoing());
			return true;
		}
		if (method == MethodName.HasUndo && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUndo(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.HasRedo && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRedo(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentActionName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentActionName());
			return true;
		}
		if (method == MethodName.GetCurrentActionStateToken && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetCurrentActionStateToken());
			return true;
		}
		if (method == MethodName.GetNextRedoActionStateToken && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetNextRedoActionStateToken());
			return true;
		}
		if (method == MethodName.ClearHistory && args.Count == 1)
		{
			ClearHistory(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHistoryAsSaved && args.Count == 1)
		{
			SetHistoryAsSaved(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsHistoryUnsaved && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsHistoryUnsaved(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetCurrentHistoryType && args.Count == 1)
		{
			SetCurrentHistoryType(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentHistoryType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCurrentHistoryType());
			return true;
		}
		if (method == MethodName.GetVersion && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetVersion());
			return true;
		}
		if (method == MethodName.GetHistoryStateToken && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(GetHistoryStateToken(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveHistoryType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveHistoryType(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.NextStateToken && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(NextStateToken());
			return true;
		}
		if (method == MethodName.NotifyHistoryChanged && args.Count == 1)
		{
			NotifyHistoryChanged(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitPending && args.Count == 0)
		{
			CommitPending();
			ret = default;
			return true;
		}
		if (method == MethodName.DiscardPendingAction && args.Count == 0)
		{
			DiscardPendingAction();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateHistoryScope)
		{
			return true;
		}
		if (method == MethodName.HasHistory)
		{
			return true;
		}
		if (method == MethodName.ReleaseHistory)
		{
			return true;
		}
		if (method == MethodName.CreateAction)
		{
			return true;
		}
		if (method == MethodName.CommitAction)
		{
			return true;
		}
		if (method == MethodName.AddDoProperty)
		{
			return true;
		}
		if (method == MethodName.AddUndoProperty)
		{
			return true;
		}
		if (method == MethodName.Undo)
		{
			return true;
		}
		if (method == MethodName.Redo)
		{
			return true;
		}
		if (method == MethodName.IsUndoing)
		{
			return true;
		}
		if (method == MethodName.IsRedoing)
		{
			return true;
		}
		if (method == MethodName.HasUndo)
		{
			return true;
		}
		if (method == MethodName.HasRedo)
		{
			return true;
		}
		if (method == MethodName.GetCurrentActionName)
		{
			return true;
		}
		if (method == MethodName.GetCurrentActionStateToken)
		{
			return true;
		}
		if (method == MethodName.GetNextRedoActionStateToken)
		{
			return true;
		}
		if (method == MethodName.ClearHistory)
		{
			return true;
		}
		if (method == MethodName.SetHistoryAsSaved)
		{
			return true;
		}
		if (method == MethodName.IsHistoryUnsaved)
		{
			return true;
		}
		if (method == MethodName.SetCurrentHistoryType)
		{
			return true;
		}
		if (method == MethodName.GetCurrentHistoryType)
		{
			return true;
		}
		if (method == MethodName.GetVersion)
		{
			return true;
		}
		if (method == MethodName.GetHistoryStateToken)
		{
			return true;
		}
		if (method == MethodName.ResolveHistoryType)
		{
			return true;
		}
		if (method == MethodName.NextStateToken)
		{
			return true;
		}
		if (method == MethodName.NotifyHistoryChanged)
		{
			return true;
		}
		if (method == MethodName.CommitPending)
		{
			return true;
		}
		if (method == MethodName.DiscardPendingAction)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.OnVersionChanged)
		{
			OnVersionChanged = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName.ActionCommitted)
		{
			ActionCommitted = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName.ActionUndone)
		{
			ActionUndone = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName.ActionRedone)
		{
			ActionRedone = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._currentHistoryType)
		{
			_currentHistoryType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._mergeMode)
		{
			_mergeMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mergeActionName)
		{
			_mergeActionName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._version)
		{
			_version = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._nextDynamicHistoryId)
		{
			_nextDynamicHistoryId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._nextStateToken)
		{
			_nextStateToken = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._pendingHistoryType)
		{
			_pendingHistoryType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isUndoing)
		{
			_isUndoing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isRedoing)
		{
			_isRedoing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		Callable from;
		if (name == PropertyName.OnVersionChanged)
		{
			from = OnVersionChanged;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ActionCommitted)
		{
			from = ActionCommitted;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ActionUndone)
		{
			from = ActionUndone;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ActionRedone)
		{
			from = ActionRedone;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._currentHistoryType)
		{
			value = VariantUtils.CreateFrom(in _currentHistoryType);
			return true;
		}
		if (name == PropertyName._mergeMode)
		{
			value = VariantUtils.CreateFrom(in _mergeMode);
			return true;
		}
		if (name == PropertyName._mergeActionName)
		{
			value = VariantUtils.CreateFrom(in _mergeActionName);
			return true;
		}
		if (name == PropertyName._version)
		{
			value = VariantUtils.CreateFrom(in _version);
			return true;
		}
		if (name == PropertyName._nextDynamicHistoryId)
		{
			value = VariantUtils.CreateFrom(in _nextDynamicHistoryId);
			return true;
		}
		if (name == PropertyName._nextStateToken)
		{
			value = VariantUtils.CreateFrom(in _nextStateToken);
			return true;
		}
		if (name == PropertyName._pendingHistoryType)
		{
			value = VariantUtils.CreateFrom(in _pendingHistoryType);
			return true;
		}
		if (name == PropertyName._isUndoing)
		{
			value = VariantUtils.CreateFrom(in _isUndoing);
			return true;
		}
		if (name == PropertyName._isRedoing)
		{
			value = VariantUtils.CreateFrom(in _isRedoing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Callable, PropertyName.OnVersionChanged, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName.ActionCommitted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName.ActionUndone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName.ActionRedone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentHistoryType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mergeMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._mergeActionName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._version, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextDynamicHistoryId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextStateToken, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingHistoryType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isUndoing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isRedoing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.OnVersionChanged, Variant.From<Callable>(OnVersionChanged));
		info.AddProperty(PropertyName.ActionCommitted, Variant.From<Callable>(ActionCommitted));
		info.AddProperty(PropertyName.ActionUndone, Variant.From<Callable>(ActionUndone));
		info.AddProperty(PropertyName.ActionRedone, Variant.From<Callable>(ActionRedone));
		info.AddProperty(PropertyName._currentHistoryType, Variant.From(in _currentHistoryType));
		info.AddProperty(PropertyName._mergeMode, Variant.From(in _mergeMode));
		info.AddProperty(PropertyName._mergeActionName, Variant.From(in _mergeActionName));
		info.AddProperty(PropertyName._version, Variant.From(in _version));
		info.AddProperty(PropertyName._nextDynamicHistoryId, Variant.From(in _nextDynamicHistoryId));
		info.AddProperty(PropertyName._nextStateToken, Variant.From(in _nextStateToken));
		info.AddProperty(PropertyName._pendingHistoryType, Variant.From(in _pendingHistoryType));
		info.AddProperty(PropertyName._isUndoing, Variant.From(in _isUndoing));
		info.AddProperty(PropertyName._isRedoing, Variant.From(in _isRedoing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.OnVersionChanged, out var value))
		{
			OnVersionChanged = value.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName.ActionCommitted, out var value2))
		{
			ActionCommitted = value2.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName.ActionUndone, out var value3))
		{
			ActionUndone = value3.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName.ActionRedone, out var value4))
		{
			ActionRedone = value4.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._currentHistoryType, out var value5))
		{
			_currentHistoryType = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._mergeMode, out var value6))
		{
			_mergeMode = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mergeActionName, out var value7))
		{
			_mergeActionName = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._version, out var value8))
		{
			_version = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._nextDynamicHistoryId, out var value9))
		{
			_nextDynamicHistoryId = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._nextStateToken, out var value10))
		{
			_nextStateToken = value10.As<long>();
		}
		if (info.TryGetProperty(PropertyName._pendingHistoryType, out var value11))
		{
			_pendingHistoryType = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isUndoing, out var value12))
		{
			_isUndoing = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isRedoing, out var value13))
		{
			_isRedoing = value13.As<bool>();
		}
	}
}
