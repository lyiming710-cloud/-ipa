using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Armor/Data/TowerDefenseArmorTypeData.cs")]
public class TowerDefenseArmorTypeData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName stagePersontage = "stagePersontage";

		public static readonly StringName armorName = "armorName";

		public static readonly StringName damagePoint = "damagePoint";

		public static readonly StringName _stagePersontage = "_stagePersontage";

		public static readonly StringName height = "height";

		public static readonly StringName stageAnimeTexturePaths = "stageAnimeTexturePaths";

		public static readonly StringName impactAudio = "impactAudio";

		public static readonly StringName damageAudio = "damageAudio";

		public static readonly StringName behaviorIds = "behaviorIds";

		public static readonly StringName behaviors = "behaviors";

		public static readonly StringName limitMaxHit = "limitMaxHit";

		public static readonly StringName explodePersontage = "explodePersontage";

		public static readonly StringName armorMethodFlags = "armorMethodFlags";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string armorName = "";

	[Export(PropertyHint.None, "")]
	public double damagePoint = 370.0;

	private Array<double> _stagePersontage = new Array<double>();

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.CHARACTER_HEIGHT height = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL;

	[Export(PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga")]
	public Array<string> stageAnimeTexturePaths = new Array<string>();

	[Export(PropertyHint.None, "")]
	public string impactAudio = "";

	[Export(PropertyHint.None, "")]
	public string damageAudio = "";

	[ExportCategory("Behavior")]
	[Export(PropertyHint.None, "")]
	public Array<StringName> behaviorIds = new Array<StringName>();

	[Export(PropertyHint.None, "")]
	public Array<ArmorBehaviorDefinition> behaviors = new Array<ArmorBehaviorDefinition>();

	[ExportGroup("Method", "")]
	[Export(PropertyHint.None, "")]
	public double limitMaxHit = -1.0;

	[Export(PropertyHint.None, "")]
	public double explodePersontage = 1.0;

	[ExportGroup("", "")]
	[Export(PropertyHint.None, "")]
	public int armorMethodFlags = 200;

	[Export(PropertyHint.None, "")]
	public Array<double> stagePersontage
	{
		get
		{
			return _stagePersontage;
		}
		set
		{
			_stagePersontage = value;
			List<double> list = _stagePersontage.ToList();
			list.Sort((double a, double b) => b.CompareTo(a));
			_stagePersontage = new Array<double>(list);
		}
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		return new Array<Dictionary>
		{
			new Dictionary
			{
				["name"] = "Flag/Method",
				["type"] = 2,
				["hint"] = 6,
				["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.ARMOR_METHOD_FLAGS>()),
				["usage"] = num
			}
		};
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property == (StringName)"Flag/Method")
		{
			armorMethodFlags = value.AsInt32();
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if (property == (StringName)"Flag/Method")
		{
			return armorMethodFlags;
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if (property == (StringName)"Flag/Method")
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (property == (StringName)"Flag/Method")
		{
			return 200;
		}
		return default;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyCanRevert, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyGetRevert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.stagePersontage)
		{
			stagePersontage = VariantUtils.ConvertToArray<double>(in value);
			return true;
		}
		if (name == PropertyName.armorName)
		{
			armorName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damagePoint)
		{
			damagePoint = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._stagePersontage)
		{
			_stagePersontage = VariantUtils.ConvertToArray<double>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_HEIGHT>(in value);
			return true;
		}
		if (name == PropertyName.stageAnimeTexturePaths)
		{
			stageAnimeTexturePaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.impactAudio)
		{
			impactAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damageAudio)
		{
			damageAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			behaviorIds = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			behaviors = VariantUtils.ConvertToArray<ArmorBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName.limitMaxHit)
		{
			limitMaxHit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.explodePersontage)
		{
			explodePersontage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.armorMethodFlags)
		{
			armorMethodFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.stagePersontage)
		{
			value = VariantUtils.CreateFromArray(stagePersontage);
			return true;
		}
		if (name == PropertyName.armorName)
		{
			value = VariantUtils.CreateFrom(in armorName);
			return true;
		}
		if (name == PropertyName.damagePoint)
		{
			value = VariantUtils.CreateFrom(in damagePoint);
			return true;
		}
		if (name == PropertyName._stagePersontage)
		{
			value = VariantUtils.CreateFromArray(_stagePersontage);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.stageAnimeTexturePaths)
		{
			value = VariantUtils.CreateFromArray(stageAnimeTexturePaths);
			return true;
		}
		if (name == PropertyName.impactAudio)
		{
			value = VariantUtils.CreateFrom(in impactAudio);
			return true;
		}
		if (name == PropertyName.damageAudio)
		{
			value = VariantUtils.CreateFrom(in damageAudio);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			value = VariantUtils.CreateFromArray(behaviorIds);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			value = VariantUtils.CreateFromArray(behaviors);
			return true;
		}
		if (name == PropertyName.limitMaxHit)
		{
			value = VariantUtils.CreateFrom(in limitMaxHit);
			return true;
		}
		if (name == PropertyName.explodePersontage)
		{
			value = VariantUtils.CreateFrom(in explodePersontage);
			return true;
		}
		if (name == PropertyName.armorMethodFlags)
		{
			value = VariantUtils.CreateFrom(in armorMethodFlags);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.armorName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damagePoint, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._stagePersontage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.stagePersontage, PropertyHint.TypeString, "3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.height, PropertyHint.Enum, "GROUND,LOW,NORMAL,TALL", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.stageAnimeTexturePaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.impactAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.damageAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Behavior", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviorIds, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviors, PropertyHint.TypeString, "24/17:ArmorBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Method", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.limitMaxHit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explodePersontage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.armorMethodFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.stagePersontage, Variant.CreateFrom(stagePersontage));
		info.AddProperty(PropertyName.armorName, Variant.From(in armorName));
		info.AddProperty(PropertyName.damagePoint, Variant.From(in damagePoint));
		info.AddProperty(PropertyName._stagePersontage, Variant.CreateFrom(_stagePersontage));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.stageAnimeTexturePaths, Variant.CreateFrom(stageAnimeTexturePaths));
		info.AddProperty(PropertyName.impactAudio, Variant.From(in impactAudio));
		info.AddProperty(PropertyName.damageAudio, Variant.From(in damageAudio));
		info.AddProperty(PropertyName.behaviorIds, Variant.CreateFrom(behaviorIds));
		info.AddProperty(PropertyName.behaviors, Variant.CreateFrom(behaviors));
		info.AddProperty(PropertyName.limitMaxHit, Variant.From(in limitMaxHit));
		info.AddProperty(PropertyName.explodePersontage, Variant.From(in explodePersontage));
		info.AddProperty(PropertyName.armorMethodFlags, Variant.From(in armorMethodFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.stagePersontage, out var value))
		{
			stagePersontage = value.AsGodotArray<double>();
		}
		if (info.TryGetProperty(PropertyName.armorName, out var value2))
		{
			armorName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damagePoint, out var value3))
		{
			damagePoint = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._stagePersontage, out var value4))
		{
			_stagePersontage = value4.AsGodotArray<double>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value5))
		{
			height = value5.As<TowerDefenseEnum.CHARACTER_HEIGHT>();
		}
		if (info.TryGetProperty(PropertyName.stageAnimeTexturePaths, out var value6))
		{
			stageAnimeTexturePaths = value6.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.impactAudio, out var value7))
		{
			impactAudio = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damageAudio, out var value8))
		{
			damageAudio = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.behaviorIds, out var value9))
		{
			behaviorIds = value9.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.behaviors, out var value10))
		{
			behaviors = value10.AsGodotArray<ArmorBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName.limitMaxHit, out var value11))
		{
			limitMaxHit = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.explodePersontage, out var value12))
		{
			explodePersontage = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.armorMethodFlags, out var value13))
		{
			armorMethodFlags = value13.As<int>();
		}
	}
}
