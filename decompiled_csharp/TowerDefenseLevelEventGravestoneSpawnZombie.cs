using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Gravestone/TowerDefenseLevelEventGravestoneSpawnZombie.cs")]
public class TowerDefenseLevelEventGravestoneSpawnZombie : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName @override = "override";

		public static readonly StringName zombieNames = "zombieNames";

		public static readonly StringName zombieNum = "zombieNum";

		public static readonly StringName delay = "delay";

		public static readonly StringName overrideVal = "overrideVal";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array zombieNames = new Array { "ZombieNormal", "ZombieNormalCone", "ZombieNormalBucket" };

	[Export(PropertyHint.None, "")]
	public int zombieNum = 5;

	[Export(PropertyHint.None, "")]
	public Vector2 delay = new Vector2(-1f, -1f);

	public TowerDefenseCharacterOverride overrideVal;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride @override
	{
		get
		{
			return overrideVal;
		}
		set
		{
			overrideVal = value;
		}
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property.ToString() == "overrideVal")
		{
			overrideVal = value.As<TowerDefenseCharacterOverride>();
			return true;
		}
		return false;
	}

	public override string _GetName()
	{
		return "LEVLE_EVENT_GRAVESTONE_SPAWN_ZOMBIE";
	}

	public override void Execute()
	{
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			TowerDefenseBattleFeatureWave.Instance.GravestoneSpawn(zombieNames, zombieNum, delay, overrideVal);
		}
	}

	public override void Init(Dictionary valueDictionary)
	{
		zombieNames = valueDictionary.GetValueOrDefault("ZombieNames", new Array()).AsGodotArray();
		zombieNum = valueDictionary.GetValueOrDefault("ZombieNum", 1).AsInt32();
		Variant valueOrDefault = valueDictionary.GetValueOrDefault("Delay", new Array { -1, -1 });
		if (valueOrDefault.VariantType == Variant.Type.Array)
		{
			Array array = valueOrDefault.AsGodotArray();
			if (array.Count == 2)
			{
				delay = new Vector2((float)array[0].AsDouble(), (float)array[1].AsDouble());
			}
			else if (array.Count == 1)
			{
				delay = new Vector2((float)array[0].AsDouble(), (float)array[0].AsDouble());
			}
		}
		if (valueOrDefault.VariantType == Variant.Type.Float)
		{
			delay = Vector2.One * (float)valueOrDefault.AsDouble();
		}
		Dictionary dictionary = valueDictionary.GetValueOrDefault("Override", new Dictionary()).AsGodotDictionary();
		if (dictionary.Count > 0)
		{
			overrideVal = new TowerDefenseCharacterOverride();
			overrideVal.Init(dictionary);
		}
	}

	public override Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["EventName"] = "GravestoneSpawnZombie",
			["Value"] = new Dictionary
			{
				["ZombieNames"] = zombieNames,
				["ZombieNum"] = zombieNum,
				["Delay"] = new Array { delay.X, delay.Y }
			}
		};
		if (GodotObject.IsInstanceValid(overrideVal))
		{
			((Dictionary)dictionary["Value"])["Override"] = overrideVal.Export();
		}
		return dictionary;
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["随机创建墓碑僵尸"] = new Dictionary
		{
			["数量"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Int",
				["Property"] = "zombieNum",
				["Rest"] = 5
			},
			["延时范围"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Vector2",
				["Property"] = "delay",
				["Rest"] = new Vector2(-1f, -1f)
			}
		};
		return property;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Set)
		{
			return true;
		}
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			@override = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName.zombieNames)
		{
			zombieNames = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.zombieNum)
		{
			zombieNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.delay)
		{
			delay = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.overrideVal)
		{
			overrideVal = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			value = VariantUtils.CreateFrom<TowerDefenseCharacterOverride>(@override);
			return true;
		}
		if (name == PropertyName.zombieNames)
		{
			value = VariantUtils.CreateFrom(in zombieNames);
			return true;
		}
		if (name == PropertyName.zombieNum)
		{
			value = VariantUtils.CreateFrom(in zombieNum);
			return true;
		}
		if (name == PropertyName.delay)
		{
			value = VariantUtils.CreateFrom(in delay);
			return true;
		}
		if (name == PropertyName.overrideVal)
		{
			value = VariantUtils.CreateFrom(in overrideVal);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.zombieNames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.zombieNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.delay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.overrideVal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.@override, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.@override, Variant.From<TowerDefenseCharacterOverride>(@override));
		info.AddProperty(PropertyName.zombieNames, Variant.From(in zombieNames));
		info.AddProperty(PropertyName.zombieNum, Variant.From(in zombieNum));
		info.AddProperty(PropertyName.delay, Variant.From(in delay));
		info.AddProperty(PropertyName.overrideVal, Variant.From(in overrideVal));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.@override, out var value))
		{
			@override = value.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.zombieNames, out var value2))
		{
			zombieNames = value2.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.zombieNum, out var value3))
		{
			zombieNum = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.delay, out var value4))
		{
			delay = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.overrideVal, out var value5))
		{
			overrideVal = value5.As<TowerDefenseCharacterOverride>();
		}
	}
}
