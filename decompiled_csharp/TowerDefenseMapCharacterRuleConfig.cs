using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Resource/Rule/TowerDefenseMapCharacterRuleConfig.cs")]
public class TowerDefenseMapCharacterRuleConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Matches = "Matches";

		public static readonly StringName Apply = "Apply";

		public static readonly StringName IsBossCharacter = "IsBossCharacter";

		public static readonly StringName IsFiniteNonNegative = "IsFiniteNonNegative";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName characterName = "characterName";

		public static readonly StringName timeScaleMultiplier = "timeScaleMultiplier";

		public static readonly StringName hitpointScaleMultiplier = "hitpointScaleMultiplier";

		public static readonly StringName visualScaleMultiplier = "visualScaleMultiplier";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string characterName = "";

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public double timeScaleMultiplier = 1.0;

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public double hitpointScaleMultiplier = 1.0;

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public double visualScaleMultiplier = 1.0;

	public bool Matches(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character?.config) && !string.IsNullOrWhiteSpace(characterName))
		{
			return string.Equals(character.config.name, characterName, StringComparison.Ordinal);
		}
		return false;
	}

	public void Apply(TowerDefenseCharacter character)
	{
		if (Matches(character))
		{
			if (!IsBossCharacter(character))
			{
				character.timeScaleInit *= timeScaleMultiplier;
			}
			if (GodotObject.IsInstanceValid(character.instance))
			{
				character.instance.hitpointScale *= hitpointScaleMultiplier;
			}
			if (GodotObject.IsInstanceValid(character.transformPoint))
			{
				character.transformPoint.Scale *= (float)visualScaleMultiplier;
			}
		}
	}

	private static bool IsBossCharacter(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character?.instance))
		{
			return character.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		}
		if (character?.config is TowerDefenseZombieConfig towerDefenseZombieConfig)
		{
			return towerDefenseZombieConfig.physique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		}
		return false;
	}

	public bool TryValidateRuntime(out string reason)
	{
		if (string.IsNullOrWhiteSpace(characterName))
		{
			reason = "character modifier has no character name";
			return false;
		}
		if (!IsFiniteNonNegative(timeScaleMultiplier) || !IsFiniteNonNegative(hitpointScaleMultiplier) || !IsFiniteNonNegative(visualScaleMultiplier))
		{
			reason = "character modifier '" + characterName + "' contains a non-finite or negative multiplier";
			return false;
		}
		reason = "";
		return true;
	}

	private static bool IsFiniteNonNegative(double value)
	{
		if (double.IsFinite(value))
		{
			return value >= 0.0;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Matches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Apply, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsBossCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsFiniteNonNegative, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Matches && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Matches(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Apply && args.Count == 1)
		{
			Apply(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsBossCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFiniteNonNegative && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFiniteNonNegative(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsBossCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFiniteNonNegative && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFiniteNonNegative(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Matches)
		{
			return true;
		}
		if (method == MethodName.Apply)
		{
			return true;
		}
		if (method == MethodName.IsBossCharacter)
		{
			return true;
		}
		if (method == MethodName.IsFiniteNonNegative)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.characterName)
		{
			characterName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.timeScaleMultiplier)
		{
			timeScaleMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpointScaleMultiplier)
		{
			hitpointScaleMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.visualScaleMultiplier)
		{
			visualScaleMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.characterName)
		{
			value = VariantUtils.CreateFrom(in characterName);
			return true;
		}
		if (name == PropertyName.timeScaleMultiplier)
		{
			value = VariantUtils.CreateFrom(in timeScaleMultiplier);
			return true;
		}
		if (name == PropertyName.hitpointScaleMultiplier)
		{
			value = VariantUtils.CreateFrom(in hitpointScaleMultiplier);
			return true;
		}
		if (name == PropertyName.visualScaleMultiplier)
		{
			value = VariantUtils.CreateFrom(in visualScaleMultiplier);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.characterName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScaleMultiplier, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointScaleMultiplier, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.visualScaleMultiplier, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.characterName, Variant.From(in characterName));
		info.AddProperty(PropertyName.timeScaleMultiplier, Variant.From(in timeScaleMultiplier));
		info.AddProperty(PropertyName.hitpointScaleMultiplier, Variant.From(in hitpointScaleMultiplier));
		info.AddProperty(PropertyName.visualScaleMultiplier, Variant.From(in visualScaleMultiplier));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.characterName, out var value))
		{
			characterName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.timeScaleMultiplier, out var value2))
		{
			timeScaleMultiplier = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpointScaleMultiplier, out var value3))
		{
			hitpointScaleMultiplier = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.visualScaleMultiplier, out var value4))
		{
			visualScaleMultiplier = value4.As<double>();
		}
	}
}
