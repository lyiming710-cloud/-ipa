using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/ConveyorBelt/Resource/TowerDefenseConveyorPacketConfig.cs")]
public class TowerDefenseConveyorPacketConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public new static readonly StringName _Set = "_Set";

		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName GetPacket = "GetPacket";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName @override = "override";

		public static readonly StringName name = "name";

		public static readonly StringName weight = "weight";

		public static readonly StringName maxNum = "maxNum";

		public static readonly StringName maxMagnification = "maxMagnification";

		public static readonly StringName minNum = "minNum";

		public static readonly StringName minMagnification = "minMagnification";

		public static readonly StringName _override = "_override";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string name = "";

	[Export(PropertyHint.None, "")]
	public int weight = 10;

	[Export(PropertyHint.None, "")]
	public int maxNum = -1;

	[Export(PropertyHint.None, "")]
	public double maxMagnification = 0.1;

	[Export(PropertyHint.None, "")]
	public int minNum = -1;

	[Export(PropertyHint.None, "")]
	public double minMagnification = 2.0;

	public TowerDefensePacketOverride _override;

	[Export(PropertyHint.None, "")]
	public TowerDefensePacketOverride @override
	{
		get
		{
			return _override;
		}
		set
		{
			_override = value;
		}
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property.ToString() == "_override")
		{
			_override = value.As<TowerDefensePacketOverride>();
			return true;
		}
		return false;
	}

	public void Init(Dictionary data)
	{
		name = data.GetValueOrDefault("Name", "").AsString();
		weight = data.GetValueOrDefault("Weight", 0).AsInt32();
		maxNum = data.GetValueOrDefault("MaxNum", -1).AsInt32();
		maxMagnification = data.GetValueOrDefault("MaxMagnification", 0).AsDouble();
		minNum = data.GetValueOrDefault("MinNum", -1).AsInt32();
		minMagnification = data.GetValueOrDefault("MinMagnification", 0).AsDouble();
		if (data.ContainsKey("Override"))
		{
			_override = new TowerDefensePacketOverride();
			_override.Init(data.GetValueOrDefault("Override", new Dictionary()).AsGodotDictionary());
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Name"] = name,
			["Weight"] = weight,
			["MaxNum"] = maxNum,
			["MaxMagnification"] = maxMagnification,
			["MinNum"] = minNum,
			["MinMagnification"] = minMagnification
		};
		if (GodotObject.IsInstanceValid(_override))
		{
			dictionary["Override"] = _override.Export();
		}
		return dictionary;
	}

	public TowerDefensePacketConfig GetPacket()
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(name);
		if (GodotObject.IsInstanceValid(_override))
		{
			packetConfig._override = _override.Duplicate(deep: true) as TowerDefensePacketOverride;
		}
		return packetConfig;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
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
		if (method == MethodName.GetPacket && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(GetPacket());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Set)
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
		if (method == MethodName.GetPacket)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			@override = VariantUtils.ConvertTo<TowerDefensePacketOverride>(in value);
			return true;
		}
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.weight)
		{
			weight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maxNum)
		{
			maxNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maxMagnification)
		{
			maxMagnification = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.minNum)
		{
			minNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.minMagnification)
		{
			minMagnification = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._override)
		{
			_override = VariantUtils.ConvertTo<TowerDefensePacketOverride>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			value = VariantUtils.CreateFrom<TowerDefensePacketOverride>(@override);
			return true;
		}
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.weight)
		{
			value = VariantUtils.CreateFrom(in weight);
			return true;
		}
		if (name == PropertyName.maxNum)
		{
			value = VariantUtils.CreateFrom(in maxNum);
			return true;
		}
		if (name == PropertyName.maxMagnification)
		{
			value = VariantUtils.CreateFrom(in maxMagnification);
			return true;
		}
		if (name == PropertyName.minNum)
		{
			value = VariantUtils.CreateFrom(in minNum);
			return true;
		}
		if (name == PropertyName.minMagnification)
		{
			value = VariantUtils.CreateFrom(in minMagnification);
			return true;
		}
		if (name == PropertyName._override)
		{
			value = VariantUtils.CreateFrom(in _override);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.weight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.maxMagnification, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.minNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.minMagnification, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._override, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.@override, PropertyHint.ResourceType, "TowerDefensePacketOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.@override, Variant.From<TowerDefensePacketOverride>(@override));
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.weight, Variant.From(in weight));
		info.AddProperty(PropertyName.maxNum, Variant.From(in maxNum));
		info.AddProperty(PropertyName.maxMagnification, Variant.From(in maxMagnification));
		info.AddProperty(PropertyName.minNum, Variant.From(in minNum));
		info.AddProperty(PropertyName.minMagnification, Variant.From(in minMagnification));
		info.AddProperty(PropertyName._override, Variant.From(in _override));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.@override, out var value))
		{
			@override = value.As<TowerDefensePacketOverride>();
		}
		if (info.TryGetProperty(PropertyName.name, out var value2))
		{
			name = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.weight, out var value3))
		{
			weight = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maxNum, out var value4))
		{
			maxNum = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maxMagnification, out var value5))
		{
			maxMagnification = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.minNum, out var value6))
		{
			minNum = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.minMagnification, out var value7))
		{
			minMagnification = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._override, out var value8))
		{
			_override = value8.As<TowerDefensePacketOverride>();
		}
	}
}
