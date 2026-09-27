using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefenseCharacterConfig.cs")]
public class TowerDefenseCharacterConfig : Resource
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
		public static readonly StringName name = "name";

		public static readonly StringName hitpointsNearDeath = "hitpointsNearDeath";

		public static readonly StringName hitpoints = "hitpoints";

		public static readonly StringName explosionHurt = "explosionHurt";

		public static readonly StringName smashHurt = "smashHurt";

		public static readonly StringName dragHurt = "dragHurt";

		public static readonly StringName spikeHurt = "spikeHurt";

		public static readonly StringName biteHurt = "biteHurt";

		public static readonly StringName canDragIntoWater = "canDragIntoWater";

		public static readonly StringName canImitate = "canImitate";

		public static readonly StringName canCopy = "canCopy";

		public static readonly StringName warnningLineFliter = "warnningLineFliter";

		public static readonly StringName sleepTime = "sleepTime";

		public static readonly StringName height = "height";

		public static readonly StringName damagePointData = "damagePointData";

		public static readonly StringName armorData = "armorData";

		public static readonly StringName customData = "customData";

		public static readonly StringName ashScene = "ashScene";

		public static readonly StringName homeWorld = "homeWorld";

		public static readonly StringName costRise = "costRise";

		public static readonly StringName cost = "cost";

		public static readonly StringName costNight = "costNight";

		public static readonly StringName costMultiple = "costMultiple";

		public static readonly StringName packetCooldown = "packetCooldown";

		public static readonly StringName startingCooldown = "startingCooldown";

		public static readonly StringName plantCoverAll = "plantCoverAll";

		public static readonly StringName plantCoverSelf = "plantCoverSelf";

		public static readonly StringName plantCover = "plantCover";

		public static readonly StringName plantCoverRecycle = "plantCoverRecycle";

		public static readonly StringName plantCanHasSurround = "plantCanHasSurround";

		public static readonly StringName plantSurroundCanPlantWater = "plantSurroundCanPlantWater";

		public static readonly StringName plantSurroundCanHasSlot = "plantSurroundCanHasSlot";

		public static readonly StringName plantGridType = "plantGridType";

		public static readonly StringName plantGridOverrideType = "plantGridOverrideType";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName maskFlags = "maskFlags";

		public static readonly StringName unUseBuffFlags = "unUseBuffFlags";

		public static readonly StringName physiqueTypeFlags = "physiqueTypeFlags";

		public static readonly StringName elementFlags = "elementFlags";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string name = "";

	[Export(PropertyHint.None, "")]
	public double hitpointsNearDeath;

	[Export(PropertyHint.None, "")]
	public double hitpoints = 300.0;

	[Export(PropertyHint.None, "")]
	public double explosionHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double smashHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double dragHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double spikeHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double biteHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public bool canDragIntoWater = true;

	[Export(PropertyHint.None, "")]
	public bool canImitate = true;

	[Export(PropertyHint.None, "")]
	public bool canCopy = true;

	[Export(PropertyHint.None, "")]
	public bool warnningLineFliter;

	[Export(PropertyHint.Enum, "Never,Night,Day")]
	public string sleepTime = "Never";

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.CHARACTER_HEIGHT height = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL;

	[Export(PropertyHint.None, "")]
	public CharacterDamagePointData damagePointData;

	[Export(PropertyHint.None, "")]
	public CharacterArmorData armorData;

	[Export(PropertyHint.None, "")]
	public CharacterCustomData customData;

	[Export(PropertyHint.None, "")]
	public PackedScene ashScene;

	[ExportCategory("packet")]
	[Export(PropertyHint.None, "")]
	public GeneralEnum.HOMEWORLD homeWorld;

	[Export(PropertyHint.None, "")]
	public int costRise = -1;

	[Export(PropertyHint.None, "")]
	public int cost = 100;

	[Export(PropertyHint.None, "")]
	public int costNight = -1;

	[Export(PropertyHint.None, "")]
	public double costMultiple = -1.0;

	[Export(PropertyHint.None, "")]
	public double packetCooldown = 5.0;

	[Export(PropertyHint.None, "")]
	public double startingCooldown;

	[Export(PropertyHint.None, "")]
	public bool plantCoverAll;

	[Export(PropertyHint.None, "")]
	public bool plantCoverSelf;

	[Export(PropertyHint.None, "")]
	public Array<string> plantCover = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<int> plantCoverRecycle = new Array<int>();

	[Export(PropertyHint.None, "")]
	public bool plantCanHasSurround = true;

	[Export(PropertyHint.None, "")]
	public bool plantSurroundCanPlantWater;

	[Export(PropertyHint.None, "")]
	public bool plantSurroundCanHasSlot = true;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseEnum.PLANTGRIDTYPE> plantGridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
	{
		TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
		TowerDefenseEnum.PLANTGRIDTYPE.POT,
		TowerDefenseEnum.PLANTGRIDTYPE.LILYPAD
	};

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.PLANTGRIDTYPE plantGridOverrideType;

	[Export(PropertyHint.None, "")]
	public int collisionFlags = 1;

	[Export(PropertyHint.None, "")]
	public int maskFlags = 1;

	[Export(PropertyHint.None, "")]
	public int unUseBuffFlags;

	[Export(PropertyHint.None, "")]
	public int physiqueTypeFlags;

	[Export(PropertyHint.None, "")]
	public int elementFlags;

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		array.Add(new Dictionary
		{
			["name"] = "Flag/Collision",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>()),
			["usage"] = num
		});
		array.Add(new Dictionary
		{
			["name"] = "Flag/Mask",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>()),
			["usage"] = num
		});
		string text = "";
		string[] names = Enum.GetNames<TowerDefenseEnum.CHARACTER_BUFF_FLAGS>();
		TowerDefenseEnum.CHARACTER_BUFF_FLAGS[] values = Enum.GetValues<TowerDefenseEnum.CHARACTER_BUFF_FLAGS>();
		for (int i = 0; i < names.Length; i++)
		{
			string value = names[i];
			int value2 = (int)values.GetValue(i);
			text += $"{value}:{value2}";
			if (i != names.Length - 1)
			{
				text += ",";
			}
		}
		array.Add(new Dictionary
		{
			["name"] = "Flag/UnUseBuff",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = text,
			["usage"] = num
		});
		array.Add(new Dictionary
		{
			["name"] = "Flag/PhysiqueType",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.CHARACTER_PHYSIQUE_TYPE>()),
			["usage"] = num
		});
		array.Add(new Dictionary
		{
			["name"] = "Flag/Elemet",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.ELEMENT_SYSTEM>()),
			["usage"] = num
		});
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		switch (property.ToString())
		{
		case "Flag/Collision":
			collisionFlags = value.AsInt32();
			return true;
		case "Flag/Mask":
			maskFlags = value.AsInt32();
			return true;
		case "Flag/UnUseBuff":
			unUseBuffFlags = value.AsInt32();
			return true;
		case "Flag/PhysiqueType":
			physiqueTypeFlags = value.AsInt32();
			return true;
		case "Flag/Elemet":
			elementFlags = value.AsInt32();
			return true;
		default:
			return false;
		}
	}

	public override Variant _Get(StringName property)
	{
		return property.ToString() switch
		{
			"Flag/Collision" => Variant.From(in collisionFlags), 
			"Flag/Mask" => Variant.From(in maskFlags), 
			"Flag/UnUseBuff" => Variant.From(in unUseBuffFlags), 
			"Flag/PhysiqueType" => Variant.From(in physiqueTypeFlags), 
			"Flag/Elemet" => Variant.From(in elementFlags), 
			_ => default, 
		};
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		switch (property.ToString())
		{
		case "Flag/Collision":
		case "Flag/Mask":
		case "Flag/UnUseBuff":
		case "Flag/PhysiqueType":
		case "Flag/Elemet":
			return true;
		default:
			return false;
		}
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		return property.ToString() switch
		{
			"Flag/Collision" => Variant.From<int>(1), 
			"Flag/Mask" => Variant.From<int>(1), 
			"Flag/UnUseBuff" => Variant.From<int>(0), 
			"Flag/PhysiqueType" => Variant.From<int>(0), 
			"Flag/Elemet" => Variant.From<int>(0), 
			_ => default, 
		};
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
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.hitpointsNearDeath)
		{
			hitpointsNearDeath = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpoints)
		{
			hitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.explosionHurt)
		{
			explosionHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.smashHurt)
		{
			smashHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dragHurt)
		{
			dragHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spikeHurt)
		{
			spikeHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.biteHurt)
		{
			biteHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.canDragIntoWater)
		{
			canDragIntoWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canImitate)
		{
			canImitate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canCopy)
		{
			canCopy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.warnningLineFliter)
		{
			warnningLineFliter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sleepTime)
		{
			sleepTime = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_HEIGHT>(in value);
			return true;
		}
		if (name == PropertyName.damagePointData)
		{
			damagePointData = VariantUtils.ConvertTo<CharacterDamagePointData>(in value);
			return true;
		}
		if (name == PropertyName.armorData)
		{
			armorData = VariantUtils.ConvertTo<CharacterArmorData>(in value);
			return true;
		}
		if (name == PropertyName.customData)
		{
			customData = VariantUtils.ConvertTo<CharacterCustomData>(in value);
			return true;
		}
		if (name == PropertyName.ashScene)
		{
			ashScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.homeWorld)
		{
			homeWorld = VariantUtils.ConvertTo<GeneralEnum.HOMEWORLD>(in value);
			return true;
		}
		if (name == PropertyName.costRise)
		{
			costRise = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.costNight)
		{
			costNight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.costMultiple)
		{
			costMultiple = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetCooldown)
		{
			packetCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.startingCooldown)
		{
			startingCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.plantCoverAll)
		{
			plantCoverAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.plantCoverSelf)
		{
			plantCoverSelf = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.plantCover)
		{
			plantCover = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.plantCoverRecycle)
		{
			plantCoverRecycle = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.plantCanHasSurround)
		{
			plantCanHasSurround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.plantSurroundCanPlantWater)
		{
			plantSurroundCanPlantWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.plantSurroundCanHasSlot)
		{
			plantSurroundCanHasSlot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.plantGridType)
		{
			plantGridType = VariantUtils.ConvertToArray<TowerDefenseEnum.PLANTGRIDTYPE>(in value);
			return true;
		}
		if (name == PropertyName.plantGridOverrideType)
		{
			plantGridOverrideType = VariantUtils.ConvertTo<TowerDefenseEnum.PLANTGRIDTYPE>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maskFlags)
		{
			maskFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.unUseBuffFlags)
		{
			unUseBuffFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.physiqueTypeFlags)
		{
			physiqueTypeFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.elementFlags)
		{
			elementFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.hitpointsNearDeath)
		{
			value = VariantUtils.CreateFrom(in hitpointsNearDeath);
			return true;
		}
		if (name == PropertyName.hitpoints)
		{
			value = VariantUtils.CreateFrom(in hitpoints);
			return true;
		}
		if (name == PropertyName.explosionHurt)
		{
			value = VariantUtils.CreateFrom(in explosionHurt);
			return true;
		}
		if (name == PropertyName.smashHurt)
		{
			value = VariantUtils.CreateFrom(in smashHurt);
			return true;
		}
		if (name == PropertyName.dragHurt)
		{
			value = VariantUtils.CreateFrom(in dragHurt);
			return true;
		}
		if (name == PropertyName.spikeHurt)
		{
			value = VariantUtils.CreateFrom(in spikeHurt);
			return true;
		}
		if (name == PropertyName.biteHurt)
		{
			value = VariantUtils.CreateFrom(in biteHurt);
			return true;
		}
		if (name == PropertyName.canDragIntoWater)
		{
			value = VariantUtils.CreateFrom(in canDragIntoWater);
			return true;
		}
		if (name == PropertyName.canImitate)
		{
			value = VariantUtils.CreateFrom(in canImitate);
			return true;
		}
		if (name == PropertyName.canCopy)
		{
			value = VariantUtils.CreateFrom(in canCopy);
			return true;
		}
		if (name == PropertyName.warnningLineFliter)
		{
			value = VariantUtils.CreateFrom(in warnningLineFliter);
			return true;
		}
		if (name == PropertyName.sleepTime)
		{
			value = VariantUtils.CreateFrom(in sleepTime);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.damagePointData)
		{
			value = VariantUtils.CreateFrom(in damagePointData);
			return true;
		}
		if (name == PropertyName.armorData)
		{
			value = VariantUtils.CreateFrom(in armorData);
			return true;
		}
		if (name == PropertyName.customData)
		{
			value = VariantUtils.CreateFrom(in customData);
			return true;
		}
		if (name == PropertyName.ashScene)
		{
			value = VariantUtils.CreateFrom(in ashScene);
			return true;
		}
		if (name == PropertyName.homeWorld)
		{
			value = VariantUtils.CreateFrom(in homeWorld);
			return true;
		}
		if (name == PropertyName.costRise)
		{
			value = VariantUtils.CreateFrom(in costRise);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.costNight)
		{
			value = VariantUtils.CreateFrom(in costNight);
			return true;
		}
		if (name == PropertyName.costMultiple)
		{
			value = VariantUtils.CreateFrom(in costMultiple);
			return true;
		}
		if (name == PropertyName.packetCooldown)
		{
			value = VariantUtils.CreateFrom(in packetCooldown);
			return true;
		}
		if (name == PropertyName.startingCooldown)
		{
			value = VariantUtils.CreateFrom(in startingCooldown);
			return true;
		}
		if (name == PropertyName.plantCoverAll)
		{
			value = VariantUtils.CreateFrom(in plantCoverAll);
			return true;
		}
		if (name == PropertyName.plantCoverSelf)
		{
			value = VariantUtils.CreateFrom(in plantCoverSelf);
			return true;
		}
		if (name == PropertyName.plantCover)
		{
			value = VariantUtils.CreateFromArray(plantCover);
			return true;
		}
		if (name == PropertyName.plantCoverRecycle)
		{
			value = VariantUtils.CreateFromArray(plantCoverRecycle);
			return true;
		}
		if (name == PropertyName.plantCanHasSurround)
		{
			value = VariantUtils.CreateFrom(in plantCanHasSurround);
			return true;
		}
		if (name == PropertyName.plantSurroundCanPlantWater)
		{
			value = VariantUtils.CreateFrom(in plantSurroundCanPlantWater);
			return true;
		}
		if (name == PropertyName.plantSurroundCanHasSlot)
		{
			value = VariantUtils.CreateFrom(in plantSurroundCanHasSlot);
			return true;
		}
		if (name == PropertyName.plantGridType)
		{
			value = VariantUtils.CreateFromArray(plantGridType);
			return true;
		}
		if (name == PropertyName.plantGridOverrideType)
		{
			value = VariantUtils.CreateFrom(in plantGridOverrideType);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
			return true;
		}
		if (name == PropertyName.maskFlags)
		{
			value = VariantUtils.CreateFrom(in maskFlags);
			return true;
		}
		if (name == PropertyName.unUseBuffFlags)
		{
			value = VariantUtils.CreateFrom(in unUseBuffFlags);
			return true;
		}
		if (name == PropertyName.physiqueTypeFlags)
		{
			value = VariantUtils.CreateFrom(in physiqueTypeFlags);
			return true;
		}
		if (name == PropertyName.elementFlags)
		{
			value = VariantUtils.CreateFrom(in elementFlags);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointsNearDeath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpoints, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explosionHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.smashHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dragHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spikeHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.biteHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canDragIntoWater, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canImitate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canCopy, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.warnningLineFliter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.sleepTime, PropertyHint.Enum, "Never,Night,Day", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.height, PropertyHint.Enum, "GROUND,LOW,NORMAL,TALL", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.damagePointData, PropertyHint.ResourceType, "CharacterDamagePointData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.armorData, PropertyHint.ResourceType, "CharacterArmorData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.customData, PropertyHint.ResourceType, "CharacterCustomData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.ashScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "packet", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.homeWorld, PropertyHint.Enum, "NOONE,MORDEN", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.costRise, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.costNight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.costMultiple, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetCooldown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.startingCooldown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantCoverAll, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantCoverSelf, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantCover, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantCoverRecycle, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantCanHasSurround, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantSurroundCanPlantWater, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantSurroundCanHasSlot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantGridType, PropertyHint.TypeString, "2/2:ALL:-1,NOONE:0,SOIL:1,GROUND:2,WATER:3,AIR:4,LILYPAD:5,POT:6,SURROUND:7,GRAVESTONE:8,CRATER:9,BRICK:10,ICECAP:11,PLANT:12", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.plantGridOverrideType, PropertyHint.Enum, "ALL:-1,NOONE:0,SOIL:1,GROUND:2,WATER:3,AIR:4,LILYPAD:5,POT:6,SURROUND:7,GRAVESTONE:8,CRATER:9,BRICK:10,ICECAP:11,PLANT:12", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maskFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.unUseBuffFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.physiqueTypeFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.elementFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.hitpointsNearDeath, Variant.From(in hitpointsNearDeath));
		info.AddProperty(PropertyName.hitpoints, Variant.From(in hitpoints));
		info.AddProperty(PropertyName.explosionHurt, Variant.From(in explosionHurt));
		info.AddProperty(PropertyName.smashHurt, Variant.From(in smashHurt));
		info.AddProperty(PropertyName.dragHurt, Variant.From(in dragHurt));
		info.AddProperty(PropertyName.spikeHurt, Variant.From(in spikeHurt));
		info.AddProperty(PropertyName.biteHurt, Variant.From(in biteHurt));
		info.AddProperty(PropertyName.canDragIntoWater, Variant.From(in canDragIntoWater));
		info.AddProperty(PropertyName.canImitate, Variant.From(in canImitate));
		info.AddProperty(PropertyName.canCopy, Variant.From(in canCopy));
		info.AddProperty(PropertyName.warnningLineFliter, Variant.From(in warnningLineFliter));
		info.AddProperty(PropertyName.sleepTime, Variant.From(in sleepTime));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.damagePointData, Variant.From(in damagePointData));
		info.AddProperty(PropertyName.armorData, Variant.From(in armorData));
		info.AddProperty(PropertyName.customData, Variant.From(in customData));
		info.AddProperty(PropertyName.ashScene, Variant.From(in ashScene));
		info.AddProperty(PropertyName.homeWorld, Variant.From(in homeWorld));
		info.AddProperty(PropertyName.costRise, Variant.From(in costRise));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.costNight, Variant.From(in costNight));
		info.AddProperty(PropertyName.costMultiple, Variant.From(in costMultiple));
		info.AddProperty(PropertyName.packetCooldown, Variant.From(in packetCooldown));
		info.AddProperty(PropertyName.startingCooldown, Variant.From(in startingCooldown));
		info.AddProperty(PropertyName.plantCoverAll, Variant.From(in plantCoverAll));
		info.AddProperty(PropertyName.plantCoverSelf, Variant.From(in plantCoverSelf));
		info.AddProperty(PropertyName.plantCover, Variant.CreateFrom(plantCover));
		info.AddProperty(PropertyName.plantCoverRecycle, Variant.CreateFrom(plantCoverRecycle));
		info.AddProperty(PropertyName.plantCanHasSurround, Variant.From(in plantCanHasSurround));
		info.AddProperty(PropertyName.plantSurroundCanPlantWater, Variant.From(in plantSurroundCanPlantWater));
		info.AddProperty(PropertyName.plantSurroundCanHasSlot, Variant.From(in plantSurroundCanHasSlot));
		info.AddProperty(PropertyName.plantGridType, Variant.CreateFrom(plantGridType));
		info.AddProperty(PropertyName.plantGridOverrideType, Variant.From(in plantGridOverrideType));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
		info.AddProperty(PropertyName.maskFlags, Variant.From(in maskFlags));
		info.AddProperty(PropertyName.unUseBuffFlags, Variant.From(in unUseBuffFlags));
		info.AddProperty(PropertyName.physiqueTypeFlags, Variant.From(in physiqueTypeFlags));
		info.AddProperty(PropertyName.elementFlags, Variant.From(in elementFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.name, out var value))
		{
			name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.hitpointsNearDeath, out var value2))
		{
			hitpointsNearDeath = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpoints, out var value3))
		{
			hitpoints = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.explosionHurt, out var value4))
		{
			explosionHurt = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.smashHurt, out var value5))
		{
			smashHurt = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dragHurt, out var value6))
		{
			dragHurt = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spikeHurt, out var value7))
		{
			spikeHurt = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.biteHurt, out var value8))
		{
			biteHurt = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.canDragIntoWater, out var value9))
		{
			canDragIntoWater = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canImitate, out var value10))
		{
			canImitate = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canCopy, out var value11))
		{
			canCopy = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.warnningLineFliter, out var value12))
		{
			warnningLineFliter = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sleepTime, out var value13))
		{
			sleepTime = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value14))
		{
			height = value14.As<TowerDefenseEnum.CHARACTER_HEIGHT>();
		}
		if (info.TryGetProperty(PropertyName.damagePointData, out var value15))
		{
			damagePointData = value15.As<CharacterDamagePointData>();
		}
		if (info.TryGetProperty(PropertyName.armorData, out var value16))
		{
			armorData = value16.As<CharacterArmorData>();
		}
		if (info.TryGetProperty(PropertyName.customData, out var value17))
		{
			customData = value17.As<CharacterCustomData>();
		}
		if (info.TryGetProperty(PropertyName.ashScene, out var value18))
		{
			ashScene = value18.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.homeWorld, out var value19))
		{
			homeWorld = value19.As<GeneralEnum.HOMEWORLD>();
		}
		if (info.TryGetProperty(PropertyName.costRise, out var value20))
		{
			costRise = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value21))
		{
			cost = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName.costNight, out var value22))
		{
			costNight = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName.costMultiple, out var value23))
		{
			costMultiple = value23.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetCooldown, out var value24))
		{
			packetCooldown = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName.startingCooldown, out var value25))
		{
			startingCooldown = value25.As<double>();
		}
		if (info.TryGetProperty(PropertyName.plantCoverAll, out var value26))
		{
			plantCoverAll = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.plantCoverSelf, out var value27))
		{
			plantCoverSelf = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.plantCover, out var value28))
		{
			plantCover = value28.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.plantCoverRecycle, out var value29))
		{
			plantCoverRecycle = value29.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.plantCanHasSurround, out var value30))
		{
			plantCanHasSurround = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.plantSurroundCanPlantWater, out var value31))
		{
			plantSurroundCanPlantWater = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.plantSurroundCanHasSlot, out var value32))
		{
			plantSurroundCanHasSlot = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.plantGridType, out var value33))
		{
			plantGridType = value33.AsGodotArray<TowerDefenseEnum.PLANTGRIDTYPE>();
		}
		if (info.TryGetProperty(PropertyName.plantGridOverrideType, out var value34))
		{
			plantGridOverrideType = value34.As<TowerDefenseEnum.PLANTGRIDTYPE>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value35))
		{
			collisionFlags = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maskFlags, out var value36))
		{
			maskFlags = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName.unUseBuffFlags, out var value37))
		{
			unUseBuffFlags = value37.As<int>();
		}
		if (info.TryGetProperty(PropertyName.physiqueTypeFlags, out var value38))
		{
			physiqueTypeFlags = value38.As<int>();
		}
		if (info.TryGetProperty(PropertyName.elementFlags, out var value39))
		{
			elementFlags = value39.As<int>();
		}
	}
}
