using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventSunCreate.cs")]
public class TowerDefenseCharacterEventSunCreate : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public static readonly StringName Run = "Run";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName num = "num";

		public static readonly StringName dieCreate = "dieCreate";

		public static readonly StringName mustForzen = "mustForzen";

		public static readonly StringName fromPacket = "fromPacket";

		public static readonly StringName fromPacketPercentage = "fromPacketPercentage";

		public static readonly StringName byCamp = "byCamp";

		public static readonly StringName forceBrainSun = "forceBrainSun";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double num = 25.0;

	[Export(PropertyHint.None, "")]
	public bool dieCreate;

	[Export(PropertyHint.None, "")]
	public bool mustForzen;

	[Export(PropertyHint.None, "")]
	public bool fromPacket;

	[Export(PropertyHint.None, "")]
	public double fromPacketPercentage = 1.0;

	[Export(PropertyHint.None, "")]
	public bool byCamp;

	[Export(PropertyHint.None, "")]
	public bool forceBrainSun;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target, num, dieCreate, mustForzen, fromPacket, fromPacketPercentage, byCamp, forceBrainSun);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target, num * delta, dieCreate, mustForzen, fromPacket, fromPacketPercentage, byCamp, forceBrainSun);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target, num, dieCreate, mustForzen, fromPacket, fromPacketPercentage, byCamp, forceBrainSun);
	}

	public override void Init(Dictionary valueDictionary)
	{
		num = valueDictionary.GetValueOrDefault("Num", 50.0).AsDouble();
		dieCreate = valueDictionary.GetValueOrDefault("DieCreate", false).AsBool();
		mustForzen = valueDictionary.GetValueOrDefault("MustForzen", false).AsBool();
		fromPacket = valueDictionary.GetValueOrDefault("FromPacket", false).AsBool();
		fromPacketPercentage = valueDictionary.GetValueOrDefault("FromPacketPercentage", 1.0).AsDouble();
		byCamp = valueDictionary.GetValueOrDefault("ByCamp", false).AsBool();
		forceBrainSun = valueDictionary.GetValueOrDefault("ForceBrainSun", false).AsBool();
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "SunCreate",
			["Value"] = new Dictionary
			{
				["Num"] = num,
				["DieCreate"] = dieCreate,
				["MustForzen"] = mustForzen,
				["FromPacket"] = fromPacket,
				["FromPacketPercentage"] = fromPacketPercentage,
				["ByCamp"] = byCamp,
				["ForceBrainSun"] = forceBrainSun
			}
		};
	}

	public static void Run(TowerDefenseCharacter target, double _num = 25.0, bool _dieCreate = false, bool _mustForzen = false, bool _fromPacket = false, double _fromPacketPercentage = 1.0, bool _byCamp = false, bool _forceBrainSun = false)
	{
		if (!GodotObject.IsInstanceValid(target))
		{
			return;
		}
		bool flag = target.IsDie() || (target.nearDie && GodotObject.IsInstanceValid(target.instance) && target.instance.die);
		if ((_dieCreate && !flag) || (_mustForzen && !target.buff.BuffHas("Frozen")))
		{
			return;
		}
		long sunNum;
		if (_fromPacket)
		{
			if (!GodotObject.IsInstanceValid(target.packet))
			{
				return;
			}
			sunNum = (long)Math.Floor((double)target.packet.GetCost() * _fromPacketPercentage);
		}
		else
		{
			sunNum = (long)_num;
		}
		bool createBrainSun = _forceBrainSun || (_byCamp && target.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		target.ReplicatedEventSunCreate(target.GetLogicalGlobalPosition(), sunNum, createBrainSun, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2.Zero);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
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
				new PropertyInfo(Variant.Type.Float, "_num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_dieCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_mustForzen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_fromPacket", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_fromPacketPercentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_byCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_forceBrainSun", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]));
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
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
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
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dieCreate)
		{
			dieCreate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mustForzen)
		{
			mustForzen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fromPacket)
		{
			fromPacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fromPacketPercentage)
		{
			fromPacketPercentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.byCamp)
		{
			byCamp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.forceBrainSun)
		{
			forceBrainSun = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		if (name == PropertyName.dieCreate)
		{
			value = VariantUtils.CreateFrom(in dieCreate);
			return true;
		}
		if (name == PropertyName.mustForzen)
		{
			value = VariantUtils.CreateFrom(in mustForzen);
			return true;
		}
		if (name == PropertyName.fromPacket)
		{
			value = VariantUtils.CreateFrom(in fromPacket);
			return true;
		}
		if (name == PropertyName.fromPacketPercentage)
		{
			value = VariantUtils.CreateFrom(in fromPacketPercentage);
			return true;
		}
		if (name == PropertyName.byCamp)
		{
			value = VariantUtils.CreateFrom(in byCamp);
			return true;
		}
		if (name == PropertyName.forceBrainSun)
		{
			value = VariantUtils.CreateFrom(in forceBrainSun);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dieCreate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mustForzen, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.fromPacket, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fromPacketPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.byCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.forceBrainSun, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName.dieCreate, Variant.From(in dieCreate));
		info.AddProperty(PropertyName.mustForzen, Variant.From(in mustForzen));
		info.AddProperty(PropertyName.fromPacket, Variant.From(in fromPacket));
		info.AddProperty(PropertyName.fromPacketPercentage, Variant.From(in fromPacketPercentage));
		info.AddProperty(PropertyName.byCamp, Variant.From(in byCamp));
		info.AddProperty(PropertyName.forceBrainSun, Variant.From(in forceBrainSun));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dieCreate, out var value2))
		{
			dieCreate = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mustForzen, out var value3))
		{
			mustForzen = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fromPacket, out var value4))
		{
			fromPacket = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fromPacketPercentage, out var value5))
		{
			fromPacketPercentage = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.byCamp, out var value6))
		{
			byCamp = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.forceBrainSun, out var value7))
		{
			forceBrainSun = value7.As<bool>();
		}
	}
}
