using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/StateMachine/XWStateMachineUndoAdapter.cs")]
public sealed class XWStateMachineUndoAdapter : RefCounted, IStateMachineUndoAdapter
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Undo = "Undo";

		public static readonly StringName Redo = "Redo";

		public static readonly StringName ReleaseHistory = "ReleaseHistory";

		public static readonly StringName Release = "Release";

		public static readonly StringName ApplyCommand = "ApplyCommand";

		public static readonly StringName RevertCommand = "RevertCommand";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName HistoryScope = "HistoryScope";

		public static readonly StringName IsHistoryReleased = "IsHistoryReleased";

		public static readonly StringName CanUndo = "CanUndo";

		public static readonly StringName CanRedo = "CanRedo";

		public static readonly StringName _undoRedo = "_undoRedo";

		public static readonly StringName _historyScope = "_historyScope";

		public static readonly StringName _nextCommandId = "_nextCommandId";

		public static readonly StringName _historyReleased = "_historyReleased";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private readonly XWUndoRedoManager _undoRedo;

	private readonly Dictionary<long, StateMachineEditCommand> _commands = new Dictionary<long, StateMachineEditCommand>();

	private readonly int _historyScope = -1;

	private long _nextCommandId;

	private bool _historyReleased;

	public int HistoryScope => _historyScope;

	public bool IsHistoryReleased => _historyReleased;

	public bool CanUndo
	{
		get
		{
			if (!_historyReleased && _historyScope >= 0)
			{
				return _undoRedo?.HasUndo(_historyScope) ?? false;
			}
			return false;
		}
	}

	public bool CanRedo
	{
		get
		{
			if (!_historyReleased && _historyScope >= 0)
			{
				return _undoRedo?.HasRedo(_historyScope) ?? false;
			}
			return false;
		}
	}

	public XWStateMachineUndoAdapter()
	{
	}

	public XWStateMachineUndoAdapter(XWUndoRedoManager undoRedo)
	{
		_undoRedo = undoRedo;
		_historyScope = _undoRedo?.CreateHistoryScope() ?? (-1);
	}

	public void Commit(string label, StateMachineEditCommand command)
	{
		if (command != null)
		{
			if (_historyReleased)
			{
				throw new ObjectDisposedException("XWStateMachineUndoAdapter");
			}
			if (_undoRedo == null)
			{
				command.Apply();
				return;
			}
			long num = ++_nextCommandId;
			_commands[num] = command;
			_undoRedo.CreateAction(string.IsNullOrWhiteSpace(label) ? "编辑状态机" : label, mergeMode: false, _historyScope);
			_undoRedo.AddDoMethod(this, "ApplyCommand", num);
			_undoRedo.AddUndoMethod(this, "RevertCommand", num);
			_undoRedo.CommitAction();
		}
	}

	public void Undo()
	{
		if (CanUndo)
		{
			_undoRedo.SetCurrentHistoryType(_historyScope);
			_undoRedo.Undo();
		}
	}

	public void Redo()
	{
		if (CanRedo)
		{
			_undoRedo.SetCurrentHistoryType(_historyScope);
			_undoRedo.Redo();
		}
	}

	public void ReleaseHistory()
	{
		if (!_historyReleased)
		{
			_historyReleased = true;
			_commands.Clear();
			if (_historyScope >= 0)
			{
				_undoRedo?.ReleaseHistory(_historyScope);
			}
		}
	}

	public void Release()
	{
		ReleaseHistory();
	}

	public void ApplyCommand(long id)
	{
		if (_commands.TryGetValue(id, out var value))
		{
			value.Apply();
		}
	}

	public void RevertCommand(long id)
	{
		if (_commands.TryGetValue(id, out var value))
		{
			value.Revert();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Undo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Redo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Release, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RevertCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Undo && args.Count == 0)
		{
			Undo();
			ret = default;
			return true;
		}
		if (method == MethodName.Redo && args.Count == 0)
		{
			Redo();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseHistory && args.Count == 0)
		{
			ReleaseHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.Release && args.Count == 0)
		{
			Release();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCommand && args.Count == 1)
		{
			ApplyCommand(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RevertCommand && args.Count == 1)
		{
			RevertCommand(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Undo)
		{
			return true;
		}
		if (method == MethodName.Redo)
		{
			return true;
		}
		if (method == MethodName.ReleaseHistory)
		{
			return true;
		}
		if (method == MethodName.Release)
		{
			return true;
		}
		if (method == MethodName.ApplyCommand)
		{
			return true;
		}
		if (method == MethodName.RevertCommand)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._nextCommandId)
		{
			_nextCommandId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._historyReleased)
		{
			_historyReleased = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HistoryScope)
		{
			value = VariantUtils.CreateFrom<int>(HistoryScope);
			return true;
		}
		bool from;
		if (name == PropertyName.IsHistoryReleased)
		{
			from = IsHistoryReleased;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CanUndo)
		{
			from = CanUndo;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CanRedo)
		{
			from = CanRedo;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			value = VariantUtils.CreateFrom(in _undoRedo);
			return true;
		}
		if (name == PropertyName._historyScope)
		{
			value = VariantUtils.CreateFrom(in _historyScope);
			return true;
		}
		if (name == PropertyName._nextCommandId)
		{
			value = VariantUtils.CreateFrom(in _nextCommandId);
			return true;
		}
		if (name == PropertyName._historyReleased)
		{
			value = VariantUtils.CreateFrom(in _historyReleased);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._historyScope, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextCommandId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._historyReleased, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.HistoryScope, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHistoryReleased, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanUndo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nextCommandId, Variant.From(in _nextCommandId));
		info.AddProperty(PropertyName._historyReleased, Variant.From(in _historyReleased));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nextCommandId, out var value))
		{
			_nextCommandId = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName._historyReleased, out var value2))
		{
			_historyReleased = value2.As<bool>();
		}
	}
}
