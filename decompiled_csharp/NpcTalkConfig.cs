using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/Npc/Talk/Base/NpcTalkConfig.cs")]
public class NpcTalkConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Load = "Load";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName data = "data";

		public static readonly StringName _data = "_data";

		public static readonly StringName saveKey = "saveKey";

		public static readonly StringName talkList = "talkList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private Json _data;

	[Export(PropertyHint.None, "")]
	public string saveKey = "";

	[Export(PropertyHint.None, "")]
	public Array<NpcTalkBaseConfig> talkList = new Array<NpcTalkBaseConfig>();

	[Export(PropertyHint.None, "")]
	public Json data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			Init();
			NotifyPropertyListChanged();
		}
	}

	public void Init()
	{
		talkList.Clear();
		if (GodotObject.IsInstanceValid(data))
		{
			Load(data.Data.AsGodotDictionary());
		}
	}

	public void Load(Dictionary _data)
	{
		talkList.Clear();
		saveKey = _data.GetValueOrDefault("SaveKey", "").AsString();
		Array array = _data.GetValueOrDefault("Talk", new Array()).AsGodotArray();
		for (int i = 0; i < array.Count; i++)
		{
			Dictionary dictionary = array[i].AsGodotDictionary();
			switch (dictionary.GetValueOrDefault("Mode", "Default").AsString())
			{
			case "Default":
			{
				NpcTalkBaseConfig npcTalkBaseConfig = new NpcTalkBaseConfig();
				npcTalkBaseConfig.Init(dictionary);
				talkList.Add(npcTalkBaseConfig);
				break;
			}
			case "Hand":
			{
				NpcTalkHandConfig npcTalkHandConfig = new NpcTalkHandConfig();
				npcTalkHandConfig.Init(dictionary);
				talkList.Add(npcTalkHandConfig);
				break;
			}
			case "Tutorial":
			{
				NpcTalkTutorialConfig npcTalkTutorialConfig = new NpcTalkTutorialConfig();
				npcTalkTutorialConfig.Init(dictionary);
				talkList.Add(npcTalkTutorialConfig);
				break;
			}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Load, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Load && args.Count == 1)
		{
			Load(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Load)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName._data)
		{
			_data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			saveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.talkList)
		{
			talkList = VariantUtils.ConvertToArray<NpcTalkBaseConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom<Json>(data);
			return true;
		}
		if (name == PropertyName._data)
		{
			value = VariantUtils.CreateFrom(in _data);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			value = VariantUtils.CreateFrom(in saveKey);
			return true;
		}
		if (name == PropertyName.talkList)
		{
			value = VariantUtils.CreateFromArray(talkList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.data, PropertyHint.ResourceType, "JSON", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.saveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.talkList, PropertyHint.TypeString, "24/17:NpcTalkBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.data, Variant.From<Json>(data));
		info.AddProperty(PropertyName._data, Variant.From(in _data));
		info.AddProperty(PropertyName.saveKey, Variant.From(in saveKey));
		info.AddProperty(PropertyName.talkList, Variant.CreateFrom(talkList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.data, out var value))
		{
			data = value.As<Json>();
		}
		if (info.TryGetProperty(PropertyName._data, out var value2))
		{
			_data = value2.As<Json>();
		}
		if (info.TryGetProperty(PropertyName.saveKey, out var value3))
		{
			saveKey = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.talkList, out var value4))
		{
			talkList = value4.AsGodotArray<NpcTalkBaseConfig>();
		}
	}
}
