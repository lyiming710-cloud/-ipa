using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWEditorSelectionHistory.cs")]
public class XWEditorSelectionHistory : RefCounted
{
	public class HistoryObject
	{
		public ulong ObjectId;

		public string Property = "";

		public bool InspectorOnly;

		public HistoryObject(ulong objectId = 0uL, string property = "", bool inspectorOnly = false)
		{
			ObjectId = objectId;
			Property = property;
			InspectorOnly = inspectorOnly;
		}

		public bool IsValid()
		{
			if (ObjectId == 0L)
			{
				return false;
			}
			return GodotObject.IsInstanceValid(GodotObject.InstanceFromId(ObjectId));
		}
	}

	public class HistoryElement
	{
		public List<HistoryObject> Path = new List<HistoryObject>();

		public int Level;
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName AddObject = "AddObject";

		public static readonly StringName ReplaceObject = "ReplaceObject";

		public static readonly StringName CleanupHistory = "CleanupHistory";

		public static readonly StringName IsAtBeginning = "IsAtBeginning";

		public static readonly StringName IsAtEnd = "IsAtEnd";

		public static readonly StringName Next = "Next";

		public static readonly StringName Previous = "Previous";

		public static readonly StringName GetCurrent = "GetCurrent";

		public static readonly StringName GetCurrentObject = "GetCurrentObject";

		public static readonly StringName IsCurrentInspectorOnly = "IsCurrentInspectorOnly";

		public static readonly StringName GetHistoryLen = "GetHistoryLen";

		public static readonly StringName GetHistoryPos = "GetHistoryPos";

		public static readonly StringName GetPathSize = "GetPathSize";

		public static readonly StringName GetPathObject = "GetPathObject";

		public static readonly StringName GetPathProperty = "GetPathProperty";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName SaveState = "SaveState";

		public static readonly StringName RestoreState = "RestoreState";

		public static readonly StringName GetHistory = "GetHistory";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _currentElemIdx = "_currentElemIdx";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private readonly List<HistoryElement> _history = new List<HistoryElement>();

	private int _currentElemIdx = -1;

	public void AddObject(ulong objectId, string property = "", bool inspectorOnly = false)
	{
		if (_currentElemIdx >= 0 && _currentElemIdx < _history.Count)
		{
			HistoryElement historyElement = _history[_currentElemIdx];
			if (historyElement.Path.Count > 0)
			{
				HistoryObject historyObject = historyElement.Path[historyElement.Path.Count - 1];
				if (historyObject.ObjectId == objectId && historyObject.Property == property)
				{
					return;
				}
			}
		}
		if (_currentElemIdx + 1 < _history.Count)
		{
			_history.RemoveRange(_currentElemIdx + 1, _history.Count - _currentElemIdx - 1);
		}
		HistoryElement historyElement2 = new HistoryElement();
		if (property != "" && _currentElemIdx >= 0 && _currentElemIdx < _history.Count)
		{
			HistoryElement historyElement3 = _history[_currentElemIdx];
			foreach (HistoryObject item in historyElement3.Path)
			{
				historyElement2.Path.Add(new HistoryObject(item.ObjectId, item.Property, item.InspectorOnly));
			}
			historyElement2.Level = historyElement3.Level + 1;
		}
		else
		{
			historyElement2.Level = 0;
		}
		historyElement2.Path.Add(new HistoryObject(objectId, property, inspectorOnly));
		_history.Add(historyElement2);
		_currentElemIdx = _history.Count - 1;
	}

	public void ReplaceObject(ulong oldObjectId, ulong newObjectId)
	{
		foreach (HistoryElement item in _history)
		{
			foreach (HistoryObject item2 in item.Path)
			{
				if (item2.ObjectId == oldObjectId)
				{
					item2.ObjectId = newObjectId;
				}
			}
		}
	}

	public void CleanupHistory()
	{
		for (int num = _history.Count - 1; num >= 0; num--)
		{
			HistoryElement historyElement = _history[num];
			bool flag = true;
			foreach (HistoryObject item in historyElement.Path)
			{
				if (!item.IsValid())
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				_history.RemoveAt(num);
				if (_currentElemIdx >= _history.Count)
				{
					_currentElemIdx = _history.Count - 1;
				}
				else if (_currentElemIdx > num)
				{
					_currentElemIdx--;
				}
			}
		}
	}

	public bool IsAtBeginning()
	{
		return _currentElemIdx <= 0;
	}

	public bool IsAtEnd()
	{
		return _currentElemIdx >= _history.Count - 1;
	}

	public bool Next()
	{
		CleanupHistory();
		if (_currentElemIdx < _history.Count - 1)
		{
			_currentElemIdx++;
			return true;
		}
		return false;
	}

	public bool Previous()
	{
		CleanupHistory();
		if (_currentElemIdx > 0)
		{
			_currentElemIdx--;
			return true;
		}
		return false;
	}

	public ulong GetCurrent()
	{
		if (_currentElemIdx < 0 || _currentElemIdx >= _history.Count)
		{
			return 0uL;
		}
		HistoryElement historyElement = _history[_currentElemIdx];
		if (historyElement.Path.Count == 0)
		{
			return 0uL;
		}
		return historyElement.Path[historyElement.Path.Count - 1].ObjectId;
	}

	public GodotObject GetCurrentObject()
	{
		ulong current = GetCurrent();
		if (current == 0L)
		{
			return null;
		}
		return GodotObject.InstanceFromId(current);
	}

	public bool IsCurrentInspectorOnly()
	{
		if (_currentElemIdx < 0 || _currentElemIdx >= _history.Count)
		{
			return false;
		}
		HistoryElement historyElement = _history[_currentElemIdx];
		if (historyElement.Path.Count == 0)
		{
			return false;
		}
		return historyElement.Path[historyElement.Path.Count - 1].InspectorOnly;
	}

	public int GetHistoryLen()
	{
		return _history.Count;
	}

	public int GetHistoryPos()
	{
		return _currentElemIdx;
	}

	public int GetPathSize()
	{
		if (_currentElemIdx < 0 || _currentElemIdx >= _history.Count)
		{
			return 0;
		}
		return _history[_currentElemIdx].Path.Count;
	}

	public ulong GetPathObject(int index)
	{
		if (_currentElemIdx < 0 || _currentElemIdx >= _history.Count)
		{
			return 0uL;
		}
		HistoryElement historyElement = _history[_currentElemIdx];
		if (index < 0 || index >= historyElement.Path.Count)
		{
			return 0uL;
		}
		return historyElement.Path[index].ObjectId;
	}

	public string GetPathProperty(int index)
	{
		if (_currentElemIdx < 0 || _currentElemIdx >= _history.Count)
		{
			return "";
		}
		HistoryElement historyElement = _history[_currentElemIdx];
		if (index < 0 || index >= historyElement.Path.Count)
		{
			return "";
		}
		return historyElement.Path[index].Property;
	}

	public void Clear()
	{
		_history.Clear();
		_currentElemIdx = -1;
	}

	public Dictionary SaveState()
	{
		Dictionary dictionary = new Dictionary();
		Array array = new Array();
		foreach (HistoryElement item in _history)
		{
			Array array2 = new Array();
			foreach (HistoryObject item2 in item.Path)
			{
				array2.Add(new Dictionary
				{
					{
						"object_id",
						(long)item2.ObjectId
					},
					{ "property", item2.Property },
					{ "inspector_only", item2.InspectorOnly }
				});
			}
			array.Add(new Dictionary
			{
				{ "path", array2 },
				{ "level", item.Level }
			});
		}
		dictionary["history"] = array;
		dictionary["current"] = _currentElemIdx;
		return dictionary;
	}

	public void RestoreState(Dictionary state)
	{
		Clear();
		if (!state.ContainsKey("history"))
		{
			return;
		}
		foreach (Variant item in (Array)state["history"])
		{
			Dictionary dictionary = (Dictionary)item;
			HistoryElement historyElement = new HistoryElement
			{
				Level = (int)dictionary.GetValueOrDefault("level", 0)
			};
			foreach (Variant item2 in (Array)dictionary.GetValueOrDefault("path", new Array()))
			{
				Dictionary dictionary2 = (Dictionary)item2;
				historyElement.Path.Add(new HistoryObject((ulong)(long)dictionary2.GetValueOrDefault("object_id", 0L), (string)dictionary2.GetValueOrDefault("property", ""), (bool)dictionary2.GetValueOrDefault("inspector_only", false)));
			}
			_history.Add(historyElement);
		}
		_currentElemIdx = (int)state.GetValueOrDefault("current", -1);
	}

	public Array GetHistory()
	{
		Array array = new Array();
		foreach (HistoryElement item in _history)
		{
			if (item.Path.Count == 0)
			{
				array.Add(0L);
			}
			else
			{
				array.Add((long)item.Path[item.Path.Count - 1].ObjectId);
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.AddObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "objectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "inspectorOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "oldObjectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "newObjectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CleanupHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsAtBeginning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsAtEnd, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Next, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Previous, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrent, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentObject, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCurrentInspectorOnly, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHistoryLen, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHistoryPos, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPathSize, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPathObject, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPathProperty, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetHistory, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddObject && args.Count == 3)
		{
			AddObject(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceObject && args.Count == 2)
		{
			ReplaceObject(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupHistory && args.Count == 0)
		{
			CleanupHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.IsAtBeginning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAtBeginning());
			return true;
		}
		if (method == MethodName.IsAtEnd && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAtEnd());
			return true;
		}
		if (method == MethodName.Next && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Next());
			return true;
		}
		if (method == MethodName.Previous && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Previous());
			return true;
		}
		if (method == MethodName.GetCurrent && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetCurrent());
			return true;
		}
		if (method == MethodName.GetCurrentObject && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<GodotObject>(GetCurrentObject());
			return true;
		}
		if (method == MethodName.IsCurrentInspectorOnly && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentInspectorOnly());
			return true;
		}
		if (method == MethodName.GetHistoryLen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetHistoryLen());
			return true;
		}
		if (method == MethodName.GetHistoryPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetHistoryPos());
			return true;
		}
		if (method == MethodName.GetPathSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPathSize());
			return true;
		}
		if (method == MethodName.GetPathObject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetPathObject(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPathProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPathProperty(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveState());
			return true;
		}
		if (method == MethodName.RestoreState && args.Count == 1)
		{
			RestoreState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetHistory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetHistory());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddObject)
		{
			return true;
		}
		if (method == MethodName.ReplaceObject)
		{
			return true;
		}
		if (method == MethodName.CleanupHistory)
		{
			return true;
		}
		if (method == MethodName.IsAtBeginning)
		{
			return true;
		}
		if (method == MethodName.IsAtEnd)
		{
			return true;
		}
		if (method == MethodName.Next)
		{
			return true;
		}
		if (method == MethodName.Previous)
		{
			return true;
		}
		if (method == MethodName.GetCurrent)
		{
			return true;
		}
		if (method == MethodName.GetCurrentObject)
		{
			return true;
		}
		if (method == MethodName.IsCurrentInspectorOnly)
		{
			return true;
		}
		if (method == MethodName.GetHistoryLen)
		{
			return true;
		}
		if (method == MethodName.GetHistoryPos)
		{
			return true;
		}
		if (method == MethodName.GetPathSize)
		{
			return true;
		}
		if (method == MethodName.GetPathObject)
		{
			return true;
		}
		if (method == MethodName.GetPathProperty)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.SaveState)
		{
			return true;
		}
		if (method == MethodName.RestoreState)
		{
			return true;
		}
		if (method == MethodName.GetHistory)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._currentElemIdx)
		{
			_currentElemIdx = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._currentElemIdx)
		{
			value = VariantUtils.CreateFrom(in _currentElemIdx);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._currentElemIdx, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._currentElemIdx, Variant.From(in _currentElemIdx));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._currentElemIdx, out var value))
		{
			_currentElemIdx = value.As<int>();
		}
	}
}
