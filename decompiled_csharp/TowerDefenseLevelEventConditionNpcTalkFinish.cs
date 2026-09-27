using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Condition/TowerDefenseLevelEventConditionNpcTalkFinish.cs")]
public class TowerDefenseLevelEventConditionNpcTalkFinish : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";

		public static readonly StringName LoadNestedEvents = "LoadNestedEvents";

		public static readonly StringName ExportNestedEvents = "ExportNestedEvents";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName npcTalkKey = "npcTalkKey";

		public static readonly StringName finishEventList = "finishEventList";

		public static readonly StringName unfinishEventList = "unfinishEventList";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string npcTalkKey = "";

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> finishEventList = new Array<TowerDefenseLevelEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> unfinishEventList = new Array<TowerDefenseLevelEventBase>();

	public override string _GetName()
	{
		return "LEVLE_EVENT_CONDITION_NPC_TALK_FINISH";
	}

	public override void Execute()
	{
		if (GodotObject.IsInstanceValid(GameSaveManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseManager.Instance.ExecuteLevelEvent(GameSaveManager.Instance.GetTutorialValue(npcTalkKey) ? finishEventList : unfinishEventList);
		}
	}

	public override void Init(Dictionary valueDictionary)
	{
		if (valueDictionary == null)
		{
			valueDictionary = new Dictionary();
		}
		npcTalkKey = valueDictionary.GetValueOrDefault("NpcTalkKey", "").AsString();
		if (finishEventList == null)
		{
			finishEventList = new Array<TowerDefenseLevelEventBase>();
		}
		if (unfinishEventList == null)
		{
			unfinishEventList = new Array<TowerDefenseLevelEventBase>();
		}
		finishEventList.Clear();
		unfinishEventList.Clear();
		LoadNestedEvents(valueDictionary, "FinishEvent", finishEventList);
		string key = (valueDictionary.ContainsKey("UnfinishEvent") ? "UnfinishEvent" : "UnfinishedEvent");
		LoadNestedEvents(valueDictionary, key, unfinishEventList);
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "ConditionNpcTalkFinish",
			["Value"] = new Dictionary
			{
				["NpcTalkKey"] = npcTalkKey,
				["FinishEvent"] = ExportNestedEvents(finishEventList, "FinishEvent"),
				["UnfinishEvent"] = ExportNestedEvents(unfinishEventList, "UnfinishEvent")
			}
		};
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["NPC对话完成条件"] = new Dictionary { ["对话存档键"] = new Dictionary
		{
			["Object"] = this,
			["Type"] = "String",
			["Property"] = "npcTalkKey",
			["Rest"] = ""
		} };
		return property;
	}

	private static void LoadNestedEvents(Dictionary source, string key, Array<TowerDefenseLevelEventBase> target)
	{
		Godot.Collections.Array array = source.GetValueOrDefault(key, new Godot.Collections.Array()).AsGodotArray();
		for (int i = 0; i < array.Count; i++)
		{
			try
			{
				if (array[i].VariantType == Variant.Type.Dictionary)
				{
					Dictionary dictionary = array[i].AsGodotDictionary();
					string text = dictionary.GetValueOrDefault("EventName", "").AsString();
					TowerDefenseLevelEventBase towerDefenseLevelEventBase = TowerDefenseLevelEventRegistry.Create(text);
					if (!GodotObject.IsInstanceValid(towerDefenseLevelEventBase))
					{
						GD.PushError($"[ConditionNpcTalkFinish] Unknown {key}[{i}] event '{text}'.");
					}
					else
					{
						towerDefenseLevelEventBase.Init(dictionary.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary());
						target.Add(towerDefenseLevelEventBase);
					}
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[ConditionNpcTalkFinish] Failed to load {key}[{i}]: {value}");
			}
		}
	}

	private static Godot.Collections.Array ExportNestedEvents(Array<TowerDefenseLevelEventBase> events, string key)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		if (events == null)
		{
			return array;
		}
		for (int i = 0; i < events.Count; i++)
		{
			try
			{
				if (GodotObject.IsInstanceValid(events[i]))
				{
					array.Add(events[i].Export());
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[ConditionNpcTalkFinish] Failed to export {key}[{i}]: {value}");
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadNestedEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNestedEvents, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "events", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName.Execute && args.Count == 0)
		{
			Execute();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.GetProperty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetProperty());
			return true;
		}
		if (method == MethodName.LoadNestedEvents && args.Count == 3)
		{
			LoadNestedEvents(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNestedEvents && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ExportNestedEvents(VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadNestedEvents && args.Count == 3)
		{
			LoadNestedEvents(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNestedEvents && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ExportNestedEvents(VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.GetProperty)
		{
			return true;
		}
		if (method == MethodName.LoadNestedEvents)
		{
			return true;
		}
		if (method == MethodName.ExportNestedEvents)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.npcTalkKey)
		{
			npcTalkKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.finishEventList)
		{
			finishEventList = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		if (name == PropertyName.unfinishEventList)
		{
			unfinishEventList = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.npcTalkKey)
		{
			value = VariantUtils.CreateFrom(in npcTalkKey);
			return true;
		}
		if (name == PropertyName.finishEventList)
		{
			value = VariantUtils.CreateFromArray(finishEventList);
			return true;
		}
		if (name == PropertyName.unfinishEventList)
		{
			value = VariantUtils.CreateFromArray(unfinishEventList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.npcTalkKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.finishEventList, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.unfinishEventList, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.npcTalkKey, Variant.From(in npcTalkKey));
		info.AddProperty(PropertyName.finishEventList, Variant.CreateFrom(finishEventList));
		info.AddProperty(PropertyName.unfinishEventList, Variant.CreateFrom(unfinishEventList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.npcTalkKey, out var value))
		{
			npcTalkKey = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.finishEventList, out var value2))
		{
			finishEventList = value2.AsGodotArray<TowerDefenseLevelEventBase>();
		}
		if (info.TryGetProperty(PropertyName.unfinishEventList, out var value3))
		{
			unfinishEventList = value3.AsGodotArray<TowerDefenseLevelEventBase>();
		}
	}
}
