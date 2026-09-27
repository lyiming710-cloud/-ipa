using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/MowerHitComponent/MowerHitComponentDefinition.cs")]
public class MowerHitComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName mowerHitScene = "mowerHitScene";

		public static readonly StringName mowerPuffScene = "mowerPuffScene";

		public static readonly StringName startupVelocity = "startupVelocity";

		public static readonly StringName runVelocity = "runVelocity";

		public static readonly StringName accelerationDelay = "accelerationDelay";

		public static readonly StringName attackAnimationBlend = "attackAnimationBlend";

		public static readonly StringName hugeTargetDamage = "hugeTargetDamage";

		public static readonly StringName terminalTargetDamage = "terminalTargetDamage";

		public static readonly StringName maxEffectCount = "maxEffectCount";

		public static readonly StringName createPuffEffect = "createPuffEffect";

		public static readonly StringName ignoreGravestones = "ignoreGravestones";

		public static readonly StringName ignoreCraters = "ignoreCraters";

		public static readonly StringName ignoreBowlingPlants = "ignoreBowlingPlants";

		public static readonly StringName ignoreSoccerBalls = "ignoreSoccerBalls";

		public static readonly StringName ignoreHypnotizedZombies = "ignoreHypnotizedZombies";

		public static readonly StringName ignoreBossZombies = "ignoreBossZombies";

		public static readonly StringName allowJumpingPolestriker = "allowJumpingPolestriker";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene mowerHitScene;

	[Export(PropertyHint.None, "")]
	public PackedScene mowerPuffScene;

	[Export(PropertyHint.Range, "0,1000,1")]
	public float startupVelocity = 100f;

	[Export(PropertyHint.Range, "0,1000,1")]
	public float runVelocity = 200f;

	[Export(PropertyHint.Range, "0,5,0.01")]
	public float accelerationDelay = 0.25f;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double attackAnimationBlend = 0.1;

	[Export(PropertyHint.None, "")]
	public double hugeTargetDamage = 100000.0;

	[Export(PropertyHint.None, "")]
	public double terminalTargetDamage = 1000000000.0;

	[Export(PropertyHint.Range, "-1,1000,1")]
	public int maxEffectCount = 100;

	[Export(PropertyHint.None, "")]
	public bool createPuffEffect = true;

	[Export(PropertyHint.None, "")]
	public bool ignoreGravestones = true;

	[Export(PropertyHint.None, "")]
	public bool ignoreCraters = true;

	[Export(PropertyHint.None, "")]
	public bool ignoreBowlingPlants = true;

	[Export(PropertyHint.None, "")]
	public bool ignoreSoccerBalls = true;

	[Export(PropertyHint.None, "")]
	public bool ignoreHypnotizedZombies = true;

	[Export(PropertyHint.None, "")]
	public bool ignoreBossZombies = true;

	[Export(PropertyHint.None, "")]
	public bool allowJumpingPolestriker = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new MowerHitComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mowerHitScene)
		{
			mowerHitScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.mowerPuffScene)
		{
			mowerPuffScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.startupVelocity)
		{
			startupVelocity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.runVelocity)
		{
			runVelocity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.accelerationDelay)
		{
			accelerationDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.attackAnimationBlend)
		{
			attackAnimationBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hugeTargetDamage)
		{
			hugeTargetDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.terminalTargetDamage)
		{
			terminalTargetDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.maxEffectCount)
		{
			maxEffectCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.createPuffEffect)
		{
			createPuffEffect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreGravestones)
		{
			ignoreGravestones = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreCraters)
		{
			ignoreCraters = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreBowlingPlants)
		{
			ignoreBowlingPlants = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreSoccerBalls)
		{
			ignoreSoccerBalls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreHypnotizedZombies)
		{
			ignoreHypnotizedZombies = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreBossZombies)
		{
			ignoreBossZombies = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.allowJumpingPolestriker)
		{
			allowJumpingPolestriker = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mowerHitScene)
		{
			value = VariantUtils.CreateFrom(in mowerHitScene);
			return true;
		}
		if (name == PropertyName.mowerPuffScene)
		{
			value = VariantUtils.CreateFrom(in mowerPuffScene);
			return true;
		}
		if (name == PropertyName.startupVelocity)
		{
			value = VariantUtils.CreateFrom(in startupVelocity);
			return true;
		}
		if (name == PropertyName.runVelocity)
		{
			value = VariantUtils.CreateFrom(in runVelocity);
			return true;
		}
		if (name == PropertyName.accelerationDelay)
		{
			value = VariantUtils.CreateFrom(in accelerationDelay);
			return true;
		}
		if (name == PropertyName.attackAnimationBlend)
		{
			value = VariantUtils.CreateFrom(in attackAnimationBlend);
			return true;
		}
		if (name == PropertyName.hugeTargetDamage)
		{
			value = VariantUtils.CreateFrom(in hugeTargetDamage);
			return true;
		}
		if (name == PropertyName.terminalTargetDamage)
		{
			value = VariantUtils.CreateFrom(in terminalTargetDamage);
			return true;
		}
		if (name == PropertyName.maxEffectCount)
		{
			value = VariantUtils.CreateFrom(in maxEffectCount);
			return true;
		}
		if (name == PropertyName.createPuffEffect)
		{
			value = VariantUtils.CreateFrom(in createPuffEffect);
			return true;
		}
		if (name == PropertyName.ignoreGravestones)
		{
			value = VariantUtils.CreateFrom(in ignoreGravestones);
			return true;
		}
		if (name == PropertyName.ignoreCraters)
		{
			value = VariantUtils.CreateFrom(in ignoreCraters);
			return true;
		}
		if (name == PropertyName.ignoreBowlingPlants)
		{
			value = VariantUtils.CreateFrom(in ignoreBowlingPlants);
			return true;
		}
		if (name == PropertyName.ignoreSoccerBalls)
		{
			value = VariantUtils.CreateFrom(in ignoreSoccerBalls);
			return true;
		}
		if (name == PropertyName.ignoreHypnotizedZombies)
		{
			value = VariantUtils.CreateFrom(in ignoreHypnotizedZombies);
			return true;
		}
		if (name == PropertyName.ignoreBossZombies)
		{
			value = VariantUtils.CreateFrom(in ignoreBossZombies);
			return true;
		}
		if (name == PropertyName.allowJumpingPolestriker)
		{
			value = VariantUtils.CreateFrom(in allowJumpingPolestriker);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerHitScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerPuffScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.startupVelocity, PropertyHint.Range, "0,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.runVelocity, PropertyHint.Range, "0,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.accelerationDelay, PropertyHint.Range, "0,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackAnimationBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hugeTargetDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.terminalTargetDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxEffectCount, PropertyHint.Range, "-1,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createPuffEffect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreGravestones, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreCraters, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreBowlingPlants, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreSoccerBalls, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreHypnotizedZombies, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreBossZombies, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.allowJumpingPolestriker, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mowerHitScene, Variant.From(in mowerHitScene));
		info.AddProperty(PropertyName.mowerPuffScene, Variant.From(in mowerPuffScene));
		info.AddProperty(PropertyName.startupVelocity, Variant.From(in startupVelocity));
		info.AddProperty(PropertyName.runVelocity, Variant.From(in runVelocity));
		info.AddProperty(PropertyName.accelerationDelay, Variant.From(in accelerationDelay));
		info.AddProperty(PropertyName.attackAnimationBlend, Variant.From(in attackAnimationBlend));
		info.AddProperty(PropertyName.hugeTargetDamage, Variant.From(in hugeTargetDamage));
		info.AddProperty(PropertyName.terminalTargetDamage, Variant.From(in terminalTargetDamage));
		info.AddProperty(PropertyName.maxEffectCount, Variant.From(in maxEffectCount));
		info.AddProperty(PropertyName.createPuffEffect, Variant.From(in createPuffEffect));
		info.AddProperty(PropertyName.ignoreGravestones, Variant.From(in ignoreGravestones));
		info.AddProperty(PropertyName.ignoreCraters, Variant.From(in ignoreCraters));
		info.AddProperty(PropertyName.ignoreBowlingPlants, Variant.From(in ignoreBowlingPlants));
		info.AddProperty(PropertyName.ignoreSoccerBalls, Variant.From(in ignoreSoccerBalls));
		info.AddProperty(PropertyName.ignoreHypnotizedZombies, Variant.From(in ignoreHypnotizedZombies));
		info.AddProperty(PropertyName.ignoreBossZombies, Variant.From(in ignoreBossZombies));
		info.AddProperty(PropertyName.allowJumpingPolestriker, Variant.From(in allowJumpingPolestriker));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mowerHitScene, out var value))
		{
			mowerHitScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.mowerPuffScene, out var value2))
		{
			mowerPuffScene = value2.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.startupVelocity, out var value3))
		{
			startupVelocity = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.runVelocity, out var value4))
		{
			runVelocity = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.accelerationDelay, out var value5))
		{
			accelerationDelay = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.attackAnimationBlend, out var value6))
		{
			attackAnimationBlend = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hugeTargetDamage, out var value7))
		{
			hugeTargetDamage = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.terminalTargetDamage, out var value8))
		{
			terminalTargetDamage = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.maxEffectCount, out var value9))
		{
			maxEffectCount = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.createPuffEffect, out var value10))
		{
			createPuffEffect = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreGravestones, out var value11))
		{
			ignoreGravestones = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreCraters, out var value12))
		{
			ignoreCraters = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreBowlingPlants, out var value13))
		{
			ignoreBowlingPlants = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreSoccerBalls, out var value14))
		{
			ignoreSoccerBalls = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreHypnotizedZombies, out var value15))
		{
			ignoreHypnotizedZombies = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreBossZombies, out var value16))
		{
			ignoreBossZombies = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.allowJumpingPolestriker, out var value17))
		{
			allowJumpingPolestriker = value17.As<bool>();
		}
	}
}
