using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/CatapultComponent/CatapultComponentDefinition.cs")]
public class CatapultComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName speed = "speed";

		public static readonly StringName damageSpeedReduction = "damageSpeedReduction";

		public static readonly StringName lowHealthThreshold = "lowHealthThreshold";

		public static readonly StringName lowHealthDeceleration = "lowHealthDeceleration";

		public static readonly StringName lowHealthMinSpeed = "lowHealthMinSpeed";

		public static readonly StringName lowHealthShake = "lowHealthShake";

		public static readonly StringName outsideMapSpeedMultiplier = "outsideMapSpeedMultiplier";

		public static readonly StringName idleBoundaryOffsetRatio = "idleBoundaryOffsetRatio";

		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName projectileNum = "projectileNum";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName fireSpeedMultiplier = "fireSpeedMultiplier";

		public static readonly StringName fireAudioName = "fireAudioName";

		public static readonly StringName useCanFireCheck = "useCanFireCheck";

		public static readonly StringName refreshInFireEntered = "refreshInFireEntered";

		public static readonly StringName hostAuthoritativeRuntime = "hostAuthoritativeRuntime";

		public static readonly StringName fireStateEvent = "fireStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName walkAnimeClip = "walkAnimeClip";

		public static readonly StringName fireAnimeClip = "fireAnimeClip";

		public static readonly StringName walkAnimationSpeedMultiplier = "walkAnimationSpeedMultiplier";

		public static readonly StringName fireAnimationBaseSpeed = "fireAnimationBaseSpeed";

		public static readonly StringName fireAnimationIntervalOffset = "fireAnimationIntervalOffset";

		public static readonly StringName fireAnimationStartScale = "fireAnimationStartScale";

		public static readonly StringName fireAnimationStartIntervalOffset = "fireAnimationStartIntervalOffset";

		public static readonly StringName repeatFireAnimationStart = "repeatFireAnimationStart";

		public static readonly StringName speedDamagePointName = "speedDamagePointName";

		public static readonly StringName showSmokeOnSpeedDamage = "showSmokeOnSpeedDamage";

		public static readonly StringName explosionEffect = "explosionEffect";

		public static readonly StringName explosionOffset = "explosionOffset";

		public static readonly StringName explosionAudioName = "explosionAudioName";

		public static readonly StringName cameraShakeEnabled = "cameraShakeEnabled";

		public static readonly StringName cameraShakeStrength = "cameraShakeStrength";

		public static readonly StringName cameraShakeDuration = "cameraShakeDuration";

		public static readonly StringName cameraShakeFrequency = "cameraShakeFrequency";

		public static readonly StringName cameraShakeRange = "cameraShakeRange";

		public static readonly StringName smokeParticlePath = "smokeParticlePath";

		public static readonly StringName fireSlotPath = "fireSlotPath";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Movement", "")]
	[Export(PropertyHint.None, "")]
	public double speed = 50.0;

	[Export(PropertyHint.None, "")]
	public double damageSpeedReduction = 40.0;

	[Export(PropertyHint.None, "")]
	public double lowHealthThreshold = 0.2;

	[Export(PropertyHint.None, "")]
	public double lowHealthDeceleration = 1.0;

	[Export(PropertyHint.None, "")]
	public double lowHealthMinSpeed = 5.0;

	[Export(PropertyHint.None, "")]
	public bool lowHealthShake = true;

	[Export(PropertyHint.None, "")]
	public double outsideMapSpeedMultiplier = 2.0;

	[Export(PropertyHint.None, "")]
	public double idleBoundaryOffsetRatio = 0.5;

	[ExportGroup("Fire", "")]
	[Export(PropertyHint.None, "")]
	public double fireInterval = 3.0;

	[Export(PropertyHint.None, "")]
	public int fireNum = 1;

	[Export(PropertyHint.None, "")]
	public int projectileNum = 20;

	[Export(PropertyHint.None, "")]
	public string projectileName = "Basketball";

	[Export(PropertyHint.None, "")]
	public double fireSpeedMultiplier = 3.0;

	[Export(PropertyHint.None, "")]
	public string fireAudioName = "Basketball";

	[Export(PropertyHint.None, "")]
	public bool useCanFireCheck = true;

	[Export(PropertyHint.None, "")]
	public bool refreshInFireEntered = true;

	[Export(PropertyHint.None, "")]
	public bool hostAuthoritativeRuntime = true;

	[Export(PropertyHint.None, "")]
	public StringName fireStateEvent = "ToFire";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent = "ToIdle";

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string walkAnimeClip = "Walk";

	[Export(PropertyHint.None, "")]
	public string fireAnimeClip = "Fire";

	[Export(PropertyHint.None, "")]
	public double walkAnimationSpeedMultiplier = 0.5;

	[Export(PropertyHint.None, "")]
	public double fireAnimationBaseSpeed = 1.75;

	[Export(PropertyHint.None, "")]
	public double fireAnimationIntervalOffset = 0.25;

	[Export(PropertyHint.None, "")]
	public double fireAnimationStartScale = 1.0 / 30.0;

	[Export(PropertyHint.None, "")]
	public double fireAnimationStartIntervalOffset = 4.5;

	[Export(PropertyHint.None, "")]
	public double repeatFireAnimationStart = 0.1;

	[ExportGroup("Damage", "")]
	[Export(PropertyHint.None, "")]
	public string speedDamagePointName = "DamagePoint2";

	[Export(PropertyHint.None, "")]
	public bool showSmokeOnSpeedDamage = true;

	[ExportGroup("Death Effect", "")]
	[Export(PropertyHint.None, "")]
	public PackedScene explosionEffect;

	[Export(PropertyHint.None, "")]
	public Vector2 explosionOffset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public string explosionAudioName = "ZamboniExplosion";

	[Export(PropertyHint.None, "")]
	public bool cameraShakeEnabled = true;

	[Export(PropertyHint.None, "")]
	public double cameraShakeStrength = 5.0;

	[Export(PropertyHint.None, "")]
	public double cameraShakeDuration = 0.05;

	[Export(PropertyHint.None, "")]
	public int cameraShakeFrequency = 4;

	[Export(PropertyHint.None, "")]
	public Vector2 cameraShakeRange = Vector2.One;

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public NodePath smokeParticlePath;

	[Export(PropertyHint.None, "")]
	public NodePath fireSlotPath;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new CatapultComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.damageSpeedReduction)
		{
			damageSpeedReduction = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.lowHealthThreshold)
		{
			lowHealthThreshold = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.lowHealthDeceleration)
		{
			lowHealthDeceleration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.lowHealthMinSpeed)
		{
			lowHealthMinSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.lowHealthShake)
		{
			lowHealthShake = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.outsideMapSpeedMultiplier)
		{
			outsideMapSpeedMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.idleBoundaryOffsetRatio)
		{
			idleBoundaryOffsetRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileNum)
		{
			projectileNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireSpeedMultiplier)
		{
			fireSpeedMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireAudioName)
		{
			fireAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.useCanFireCheck)
		{
			useCanFireCheck = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.refreshInFireEntered)
		{
			refreshInFireEntered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hostAuthoritativeRuntime)
		{
			hostAuthoritativeRuntime = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireStateEvent)
		{
			fireStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.walkAnimeClip)
		{
			walkAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeClip)
		{
			fireAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.walkAnimationSpeedMultiplier)
		{
			walkAnimationSpeedMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimationBaseSpeed)
		{
			fireAnimationBaseSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimationIntervalOffset)
		{
			fireAnimationIntervalOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimationStartScale)
		{
			fireAnimationStartScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimationStartIntervalOffset)
		{
			fireAnimationStartIntervalOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.repeatFireAnimationStart)
		{
			repeatFireAnimationStart = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.speedDamagePointName)
		{
			speedDamagePointName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.showSmokeOnSpeedDamage)
		{
			showSmokeOnSpeedDamage = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.explosionEffect)
		{
			explosionEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.explosionOffset)
		{
			explosionOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.explosionAudioName)
		{
			explosionAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeEnabled)
		{
			cameraShakeEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeStrength)
		{
			cameraShakeStrength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeDuration)
		{
			cameraShakeDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeFrequency)
		{
			cameraShakeFrequency = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeRange)
		{
			cameraShakeRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.smokeParticlePath)
		{
			smokeParticlePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.fireSlotPath)
		{
			fireSlotPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.damageSpeedReduction)
		{
			value = VariantUtils.CreateFrom(in damageSpeedReduction);
			return true;
		}
		if (name == PropertyName.lowHealthThreshold)
		{
			value = VariantUtils.CreateFrom(in lowHealthThreshold);
			return true;
		}
		if (name == PropertyName.lowHealthDeceleration)
		{
			value = VariantUtils.CreateFrom(in lowHealthDeceleration);
			return true;
		}
		if (name == PropertyName.lowHealthMinSpeed)
		{
			value = VariantUtils.CreateFrom(in lowHealthMinSpeed);
			return true;
		}
		if (name == PropertyName.lowHealthShake)
		{
			value = VariantUtils.CreateFrom(in lowHealthShake);
			return true;
		}
		if (name == PropertyName.outsideMapSpeedMultiplier)
		{
			value = VariantUtils.CreateFrom(in outsideMapSpeedMultiplier);
			return true;
		}
		if (name == PropertyName.idleBoundaryOffsetRatio)
		{
			value = VariantUtils.CreateFrom(in idleBoundaryOffsetRatio);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom(in fireInterval);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			value = VariantUtils.CreateFrom(in fireNum);
			return true;
		}
		if (name == PropertyName.projectileNum)
		{
			value = VariantUtils.CreateFrom(in projectileNum);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom(in projectileName);
			return true;
		}
		if (name == PropertyName.fireSpeedMultiplier)
		{
			value = VariantUtils.CreateFrom(in fireSpeedMultiplier);
			return true;
		}
		if (name == PropertyName.fireAudioName)
		{
			value = VariantUtils.CreateFrom(in fireAudioName);
			return true;
		}
		if (name == PropertyName.useCanFireCheck)
		{
			value = VariantUtils.CreateFrom(in useCanFireCheck);
			return true;
		}
		if (name == PropertyName.refreshInFireEntered)
		{
			value = VariantUtils.CreateFrom(in refreshInFireEntered);
			return true;
		}
		if (name == PropertyName.hostAuthoritativeRuntime)
		{
			value = VariantUtils.CreateFrom(in hostAuthoritativeRuntime);
			return true;
		}
		if (name == PropertyName.fireStateEvent)
		{
			value = VariantUtils.CreateFrom(in fireStateEvent);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			value = VariantUtils.CreateFrom(in idleStateEvent);
			return true;
		}
		if (name == PropertyName.walkAnimeClip)
		{
			value = VariantUtils.CreateFrom(in walkAnimeClip);
			return true;
		}
		if (name == PropertyName.fireAnimeClip)
		{
			value = VariantUtils.CreateFrom(in fireAnimeClip);
			return true;
		}
		if (name == PropertyName.walkAnimationSpeedMultiplier)
		{
			value = VariantUtils.CreateFrom(in walkAnimationSpeedMultiplier);
			return true;
		}
		if (name == PropertyName.fireAnimationBaseSpeed)
		{
			value = VariantUtils.CreateFrom(in fireAnimationBaseSpeed);
			return true;
		}
		if (name == PropertyName.fireAnimationIntervalOffset)
		{
			value = VariantUtils.CreateFrom(in fireAnimationIntervalOffset);
			return true;
		}
		if (name == PropertyName.fireAnimationStartScale)
		{
			value = VariantUtils.CreateFrom(in fireAnimationStartScale);
			return true;
		}
		if (name == PropertyName.fireAnimationStartIntervalOffset)
		{
			value = VariantUtils.CreateFrom(in fireAnimationStartIntervalOffset);
			return true;
		}
		if (name == PropertyName.repeatFireAnimationStart)
		{
			value = VariantUtils.CreateFrom(in repeatFireAnimationStart);
			return true;
		}
		if (name == PropertyName.speedDamagePointName)
		{
			value = VariantUtils.CreateFrom(in speedDamagePointName);
			return true;
		}
		if (name == PropertyName.showSmokeOnSpeedDamage)
		{
			value = VariantUtils.CreateFrom(in showSmokeOnSpeedDamage);
			return true;
		}
		if (name == PropertyName.explosionEffect)
		{
			value = VariantUtils.CreateFrom(in explosionEffect);
			return true;
		}
		if (name == PropertyName.explosionOffset)
		{
			value = VariantUtils.CreateFrom(in explosionOffset);
			return true;
		}
		if (name == PropertyName.explosionAudioName)
		{
			value = VariantUtils.CreateFrom(in explosionAudioName);
			return true;
		}
		if (name == PropertyName.cameraShakeEnabled)
		{
			value = VariantUtils.CreateFrom(in cameraShakeEnabled);
			return true;
		}
		if (name == PropertyName.cameraShakeStrength)
		{
			value = VariantUtils.CreateFrom(in cameraShakeStrength);
			return true;
		}
		if (name == PropertyName.cameraShakeDuration)
		{
			value = VariantUtils.CreateFrom(in cameraShakeDuration);
			return true;
		}
		if (name == PropertyName.cameraShakeFrequency)
		{
			value = VariantUtils.CreateFrom(in cameraShakeFrequency);
			return true;
		}
		if (name == PropertyName.cameraShakeRange)
		{
			value = VariantUtils.CreateFrom(in cameraShakeRange);
			return true;
		}
		if (name == PropertyName.smokeParticlePath)
		{
			value = VariantUtils.CreateFrom(in smokeParticlePath);
			return true;
		}
		if (name == PropertyName.fireSlotPath)
		{
			value = VariantUtils.CreateFrom(in fireSlotPath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Movement", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damageSpeedReduction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.lowHealthThreshold, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.lowHealthDeceleration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.lowHealthMinSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.lowHealthShake, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.outsideMapSpeedMultiplier, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.idleBoundaryOffsetRatio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Fire", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.projectileNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireSpeedMultiplier, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useCanFireCheck, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.refreshInFireEntered, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hostAuthoritativeRuntime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.fireStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.walkAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.walkAnimationSpeedMultiplier, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimationBaseSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimationIntervalOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimationStartScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimationStartIntervalOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.repeatFireAnimationStart, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Damage", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.speedDamagePointName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showSmokeOnSpeedDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Death Effect", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.explosionEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.explosionOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.explosionAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.cameraShakeEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeStrength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cameraShakeFrequency, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.cameraShakeRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.smokeParticlePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.fireSlotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.damageSpeedReduction, Variant.From(in damageSpeedReduction));
		info.AddProperty(PropertyName.lowHealthThreshold, Variant.From(in lowHealthThreshold));
		info.AddProperty(PropertyName.lowHealthDeceleration, Variant.From(in lowHealthDeceleration));
		info.AddProperty(PropertyName.lowHealthMinSpeed, Variant.From(in lowHealthMinSpeed));
		info.AddProperty(PropertyName.lowHealthShake, Variant.From(in lowHealthShake));
		info.AddProperty(PropertyName.outsideMapSpeedMultiplier, Variant.From(in outsideMapSpeedMultiplier));
		info.AddProperty(PropertyName.idleBoundaryOffsetRatio, Variant.From(in idleBoundaryOffsetRatio));
		info.AddProperty(PropertyName.fireInterval, Variant.From(in fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From(in fireNum));
		info.AddProperty(PropertyName.projectileNum, Variant.From(in projectileNum));
		info.AddProperty(PropertyName.projectileName, Variant.From(in projectileName));
		info.AddProperty(PropertyName.fireSpeedMultiplier, Variant.From(in fireSpeedMultiplier));
		info.AddProperty(PropertyName.fireAudioName, Variant.From(in fireAudioName));
		info.AddProperty(PropertyName.useCanFireCheck, Variant.From(in useCanFireCheck));
		info.AddProperty(PropertyName.refreshInFireEntered, Variant.From(in refreshInFireEntered));
		info.AddProperty(PropertyName.hostAuthoritativeRuntime, Variant.From(in hostAuthoritativeRuntime));
		info.AddProperty(PropertyName.fireStateEvent, Variant.From(in fireStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From(in idleStateEvent));
		info.AddProperty(PropertyName.walkAnimeClip, Variant.From(in walkAnimeClip));
		info.AddProperty(PropertyName.fireAnimeClip, Variant.From(in fireAnimeClip));
		info.AddProperty(PropertyName.walkAnimationSpeedMultiplier, Variant.From(in walkAnimationSpeedMultiplier));
		info.AddProperty(PropertyName.fireAnimationBaseSpeed, Variant.From(in fireAnimationBaseSpeed));
		info.AddProperty(PropertyName.fireAnimationIntervalOffset, Variant.From(in fireAnimationIntervalOffset));
		info.AddProperty(PropertyName.fireAnimationStartScale, Variant.From(in fireAnimationStartScale));
		info.AddProperty(PropertyName.fireAnimationStartIntervalOffset, Variant.From(in fireAnimationStartIntervalOffset));
		info.AddProperty(PropertyName.repeatFireAnimationStart, Variant.From(in repeatFireAnimationStart));
		info.AddProperty(PropertyName.speedDamagePointName, Variant.From(in speedDamagePointName));
		info.AddProperty(PropertyName.showSmokeOnSpeedDamage, Variant.From(in showSmokeOnSpeedDamage));
		info.AddProperty(PropertyName.explosionEffect, Variant.From(in explosionEffect));
		info.AddProperty(PropertyName.explosionOffset, Variant.From(in explosionOffset));
		info.AddProperty(PropertyName.explosionAudioName, Variant.From(in explosionAudioName));
		info.AddProperty(PropertyName.cameraShakeEnabled, Variant.From(in cameraShakeEnabled));
		info.AddProperty(PropertyName.cameraShakeStrength, Variant.From(in cameraShakeStrength));
		info.AddProperty(PropertyName.cameraShakeDuration, Variant.From(in cameraShakeDuration));
		info.AddProperty(PropertyName.cameraShakeFrequency, Variant.From(in cameraShakeFrequency));
		info.AddProperty(PropertyName.cameraShakeRange, Variant.From(in cameraShakeRange));
		info.AddProperty(PropertyName.smokeParticlePath, Variant.From(in smokeParticlePath));
		info.AddProperty(PropertyName.fireSlotPath, Variant.From(in fireSlotPath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.speed, out var value))
		{
			speed = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.damageSpeedReduction, out var value2))
		{
			damageSpeedReduction = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.lowHealthThreshold, out var value3))
		{
			lowHealthThreshold = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.lowHealthDeceleration, out var value4))
		{
			lowHealthDeceleration = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.lowHealthMinSpeed, out var value5))
		{
			lowHealthMinSpeed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.lowHealthShake, out var value6))
		{
			lowHealthShake = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.outsideMapSpeedMultiplier, out var value7))
		{
			outsideMapSpeedMultiplier = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.idleBoundaryOffsetRatio, out var value8))
		{
			idleBoundaryOffsetRatio = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireInterval, out var value9))
		{
			fireInterval = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value10))
		{
			fireNum = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileNum, out var value11))
		{
			projectileNum = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value12))
		{
			projectileName = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireSpeedMultiplier, out var value13))
		{
			fireSpeedMultiplier = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireAudioName, out var value14))
		{
			fireAudioName = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.useCanFireCheck, out var value15))
		{
			useCanFireCheck = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.refreshInFireEntered, out var value16))
		{
			refreshInFireEntered = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hostAuthoritativeRuntime, out var value17))
		{
			hostAuthoritativeRuntime = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireStateEvent, out var value18))
		{
			fireStateEvent = value18.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value19))
		{
			idleStateEvent = value19.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.walkAnimeClip, out var value20))
		{
			walkAnimeClip = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeClip, out var value21))
		{
			fireAnimeClip = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName.walkAnimationSpeedMultiplier, out var value22))
		{
			walkAnimationSpeedMultiplier = value22.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimationBaseSpeed, out var value23))
		{
			fireAnimationBaseSpeed = value23.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimationIntervalOffset, out var value24))
		{
			fireAnimationIntervalOffset = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimationStartScale, out var value25))
		{
			fireAnimationStartScale = value25.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimationStartIntervalOffset, out var value26))
		{
			fireAnimationStartIntervalOffset = value26.As<double>();
		}
		if (info.TryGetProperty(PropertyName.repeatFireAnimationStart, out var value27))
		{
			repeatFireAnimationStart = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.speedDamagePointName, out var value28))
		{
			speedDamagePointName = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName.showSmokeOnSpeedDamage, out var value29))
		{
			showSmokeOnSpeedDamage = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.explosionEffect, out var value30))
		{
			explosionEffect = value30.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.explosionOffset, out var value31))
		{
			explosionOffset = value31.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.explosionAudioName, out var value32))
		{
			explosionAudioName = value32.As<string>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeEnabled, out var value33))
		{
			cameraShakeEnabled = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeStrength, out var value34))
		{
			cameraShakeStrength = value34.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeDuration, out var value35))
		{
			cameraShakeDuration = value35.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeFrequency, out var value36))
		{
			cameraShakeFrequency = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeRange, out var value37))
		{
			cameraShakeRange = value37.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.smokeParticlePath, out var value38))
		{
			smokeParticlePath = value38.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.fireSlotPath, out var value39))
		{
			fireSlotPath = value39.As<NodePath>();
		}
	}
}
