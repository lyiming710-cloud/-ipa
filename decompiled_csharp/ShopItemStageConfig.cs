using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Shop/ShopItemStageConfig.cs")]
public class ShopItemStageConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName saveType = "saveType";

		public static readonly StringName saveKey = "saveKey";

		public static readonly StringName texture = "texture";

		public static readonly StringName describe = "describe";

		public static readonly StringName npcTalk = "npcTalk";

		public static readonly StringName type = "type";

		public static readonly StringName cost = "cost";

		public static readonly StringName openMinNum = "openMinNum";

		public static readonly StringName openMaxNum = "openMaxNum";

		public static readonly StringName addNum = "addNum";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[ExportGroup("Base", "")]
	[Export(PropertyHint.Enum, "Feature,TowerDefensePacket")]
	public string saveType = "Feature";

	[Export(PropertyHint.None, "")]
	public string saveKey = "";

	[Export(PropertyHint.None, "")]
	public Texture2D texture;

	[Export(PropertyHint.None, "")]
	public string describe;

	[Export(PropertyHint.None, "")]
	public string npcTalk;

	[ExportGroup("Init", "")]
	[Export(PropertyHint.Enum, "Coin")]
	public string type = "Coin";

	[Export(PropertyHint.None, "")]
	public int cost;

	[Export(PropertyHint.None, "")]
	public int openMinNum;

	[Export(PropertyHint.None, "")]
	public int openMaxNum = 1;

	[Export(PropertyHint.None, "")]
	public int addNum = 1;

	public void Init(Dictionary data)
	{
		saveType = data.GetValueOrDefault("SaveType", "Feature").AsString();
		saveKey = data.GetValueOrDefault("SaveKey", "").AsString();
		string text = data.GetValueOrDefault("Texture", "").AsString();
		if (text != "")
		{
			texture = GD.Load<Texture2D>(text);
		}
		describe = data.GetValueOrDefault("Describe", "").AsString();
		npcTalk = data.GetValueOrDefault("NpcTalk", "").AsString();
		cost = data.GetValueOrDefault("Cost", 0).AsInt32();
		openMinNum = data.GetValueOrDefault("OpenMinNum", 0).AsInt32();
		openMaxNum = data.GetValueOrDefault("OpenMaxNum", 1).AsInt32();
		addNum = data.GetValueOrDefault("AddNum", 1).AsInt32();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.saveType)
		{
			saveType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			saveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.texture)
		{
			texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.describe)
		{
			describe = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.npcTalk)
		{
			npcTalk = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.openMinNum)
		{
			openMinNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.openMaxNum)
		{
			openMaxNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.addNum)
		{
			addNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.saveType)
		{
			value = VariantUtils.CreateFrom(in saveType);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			value = VariantUtils.CreateFrom(in saveKey);
			return true;
		}
		if (name == PropertyName.texture)
		{
			value = VariantUtils.CreateFrom(in texture);
			return true;
		}
		if (name == PropertyName.describe)
		{
			value = VariantUtils.CreateFrom(in describe);
			return true;
		}
		if (name == PropertyName.npcTalk)
		{
			value = VariantUtils.CreateFrom(in npcTalk);
			return true;
		}
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.openMinNum)
		{
			value = VariantUtils.CreateFrom(in openMinNum);
			return true;
		}
		if (name == PropertyName.openMaxNum)
		{
			value = VariantUtils.CreateFrom(in openMaxNum);
			return true;
		}
		if (name == PropertyName.addNum)
		{
			value = VariantUtils.CreateFrom(in addNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Base", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.saveType, PropertyHint.Enum, "Feature,TowerDefensePacket", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.saveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.texture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.describe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.npcTalk, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Init", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.Enum, "Coin", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.openMinNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.openMaxNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.addNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.saveType, Variant.From(in saveType));
		info.AddProperty(PropertyName.saveKey, Variant.From(in saveKey));
		info.AddProperty(PropertyName.texture, Variant.From(in texture));
		info.AddProperty(PropertyName.describe, Variant.From(in describe));
		info.AddProperty(PropertyName.npcTalk, Variant.From(in npcTalk));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.openMinNum, Variant.From(in openMinNum));
		info.AddProperty(PropertyName.openMaxNum, Variant.From(in openMaxNum));
		info.AddProperty(PropertyName.addNum, Variant.From(in addNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.saveType, out var value))
		{
			saveType = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.saveKey, out var value2))
		{
			saveKey = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.texture, out var value3))
		{
			texture = value3.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.describe, out var value4))
		{
			describe = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.npcTalk, out var value5))
		{
			npcTalk = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value6))
		{
			type = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value7))
		{
			cost = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.openMinNum, out var value8))
		{
			openMinNum = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.openMaxNum, out var value9))
		{
			openMaxNum = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.addNum, out var value10))
		{
			addNum = value10.As<int>();
		}
	}
}
