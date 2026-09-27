using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventPacketCreate.cs")]
public class TowerDefenseCharacterEventPacketCreate : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public static readonly StringName Run = "Run";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName @override = "override";

		public static readonly StringName usePacketBank = "usePacketBank";

		public static readonly StringName packetName = "packetName";

		public static readonly StringName packetBankName = "packetBankName";

		public static readonly StringName categoryName = "categoryName";

		public static readonly StringName aliveTime = "aliveTime";

		public static readonly StringName useCost = "useCost";

		public static readonly StringName _override = "_override";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool usePacketBank;

	[Export(PropertyHint.None, "")]
	public string packetName = "";

	[Export(PropertyHint.None, "")]
	public string packetBankName = "GeneralPlant";

	[Export(PropertyHint.None, "")]
	public string categoryName = "";

	[Export(PropertyHint.None, "")]
	public double aliveTime = 15.0;

	[Export(PropertyHint.None, "")]
	public bool useCost;

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

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target, usePacketBank, packetName, packetBankName, categoryName, aliveTime, useCost, _override);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target, usePacketBank, packetName, packetBankName, categoryName, aliveTime, useCost, _override);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target, usePacketBank, packetName, packetBankName, categoryName, aliveTime, useCost, _override);
	}

	public override void Init(Dictionary valueDictionary)
	{
		usePacketBank = valueDictionary.GetValueOrDefault("UsePacketBank", false).AsBool();
		packetName = valueDictionary.GetValueOrDefault("PacketName", "").AsString();
		packetBankName = valueDictionary.GetValueOrDefault("PacketBankName", "GeneralPlant").AsString();
		categoryName = valueDictionary.GetValueOrDefault("CategoryName", "").AsString();
		aliveTime = valueDictionary.GetValueOrDefault("AliveTime", 15.0).AsDouble();
		useCost = valueDictionary.GetValueOrDefault("UseCost", false).AsBool();
		Dictionary dictionary = valueDictionary.GetValueOrDefault("Override", new Dictionary()).AsGodotDictionary();
		if (dictionary.Count > 0)
		{
			_override = new TowerDefensePacketOverride();
			_override.Init(dictionary);
		}
	}

	public override Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["EventName"] = "PacketCreate",
			["Value"] = new Dictionary
			{
				["PacketName"] = packetName,
				["PacketBankName"] = packetBankName,
				["CategoryName"] = categoryName,
				["UsePacketBank"] = usePacketBank,
				["AliveTime"] = aliveTime,
				["UseCost"] = useCost
			}
		};
		if (GodotObject.IsInstanceValid(_override))
		{
			((Dictionary)dictionary["Value"])["Override"] = _override.Export();
		}
		return dictionary;
	}

	public static void Run(TowerDefenseCharacter target, bool _usePacketBank, string _packetName, string _packetBankName, string _categoryName, double _aliveTime = 8.0, bool _useCost = false, TowerDefensePacketOverride _override = null)
	{
		string text;
		if (_usePacketBank)
		{
			TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData(_packetBankName);
			Array array = ((!(_categoryName != "")) ? packetBankData.GetPacketList() : packetBankData.GetCategory(_categoryName));
			text = (string)array[(int)((ulong)GD.Randi() % (ulong)array.Count)];
		}
		else
		{
			text = _packetName;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
		if (GodotObject.IsInstanceValid(_override))
		{
			packetConfig._override = _override.Duplicate(deep: true) as TowerDefensePacketOverride;
		}
		TowerDefenseManager.Instance.SpawnPacket(packetConfig, target.GetLogicalGlobalPosition(), _aliveTime, isFall: false, _useCost);
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
		towerDefenseInGamePacketShow.showCost = _useCost;
		towerDefenseInGamePacketShow.aliveTime = _aliveTime;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteDps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "_usePacketBank", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "_packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "_packetBankName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "_categoryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_aliveTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_useCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_override", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
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
		if (method == MethodName.Execute && args.Count == 2)
		{
			Execute(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteDps && args.Count == 3)
		{
			ExecuteDps(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteProject && args.Count == 2)
		{
			ExecuteProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
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
		if (method == MethodName.Run && args.Count == 8)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[7]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 8)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[7]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.ExecuteDps)
		{
			return true;
		}
		if (method == MethodName.ExecuteProject)
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
		if (method == MethodName.Run)
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
		if (name == PropertyName.usePacketBank)
		{
			usePacketBank = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			packetName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.packetBankName)
		{
			packetBankName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.categoryName)
		{
			categoryName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.aliveTime)
		{
			aliveTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.useCost)
		{
			useCost = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.usePacketBank)
		{
			value = VariantUtils.CreateFrom(in usePacketBank);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			value = VariantUtils.CreateFrom(in packetName);
			return true;
		}
		if (name == PropertyName.packetBankName)
		{
			value = VariantUtils.CreateFrom(in packetBankName);
			return true;
		}
		if (name == PropertyName.categoryName)
		{
			value = VariantUtils.CreateFrom(in categoryName);
			return true;
		}
		if (name == PropertyName.aliveTime)
		{
			value = VariantUtils.CreateFrom(in aliveTime);
			return true;
		}
		if (name == PropertyName.useCost)
		{
			value = VariantUtils.CreateFrom(in useCost);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.usePacketBank, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.packetName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.packetBankName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.categoryName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.aliveTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._override, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.@override, PropertyHint.ResourceType, "TowerDefensePacketOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.@override, Variant.From<TowerDefensePacketOverride>(@override));
		info.AddProperty(PropertyName.usePacketBank, Variant.From(in usePacketBank));
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.packetBankName, Variant.From(in packetBankName));
		info.AddProperty(PropertyName.categoryName, Variant.From(in categoryName));
		info.AddProperty(PropertyName.aliveTime, Variant.From(in aliveTime));
		info.AddProperty(PropertyName.useCost, Variant.From(in useCost));
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
		if (info.TryGetProperty(PropertyName.usePacketBank, out var value2))
		{
			usePacketBank = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetName, out var value3))
		{
			packetName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.packetBankName, out var value4))
		{
			packetBankName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.categoryName, out var value5))
		{
			categoryName = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.aliveTime, out var value6))
		{
			aliveTime = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.useCost, out var value7))
		{
			useCost = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._override, out var value8))
		{
			_override = value8.As<TowerDefensePacketOverride>();
		}
	}
}
