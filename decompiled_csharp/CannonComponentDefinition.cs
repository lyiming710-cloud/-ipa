using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/CannonComponent/CannonComponentDefinition.cs")]
public class CannonComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName mode = "mode";

		public static readonly StringName autoAttack = "autoAttack";

		public static readonly StringName autoAttackMaxXOffset = "autoAttackMaxXOffset";

		public static readonly StringName projectileSlotPath = "projectileSlotPath";

		public static readonly StringName projectileNodePath = "projectileNodePath";

		public static readonly StringName projectileData = "projectileData";

		public static readonly StringName projectileLineFireComponentInstanceId = "projectileLineFireComponentInstanceId";

		public static readonly StringName projectileLineMarkerPath = "projectileLineMarkerPath";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName restTime = "restTime";

		public static readonly StringName firstRestTime = "firstRestTime";

		public static readonly StringName disableRestTimeout = "disableRestTimeout";

		public static readonly StringName markerProjectileHeight = "markerProjectileHeight";

		public static readonly StringName markerProjectileFallSpeed = "markerProjectileFallSpeed";

		public static readonly StringName markerProjectileHitBoxScale = "markerProjectileHitBoxScale";

		public static readonly StringName markerVisualTravelHeight = "markerVisualTravelHeight";

		public static readonly StringName markerVisualTravelDuration = "markerVisualTravelDuration";

		public static readonly StringName markerProjectileStraightMove = "markerProjectileStraightMove";

		public static readonly StringName markerProjectileStraightSpeed = "markerProjectileStraightSpeed";

		public static readonly StringName lineProjectileVelocity = "lineProjectileVelocity";

		public static readonly StringName lineFirePositionIndex = "lineFirePositionIndex";

		public static readonly StringName lineProjectileFlipWithParent = "lineProjectileFlipWithParent";

		public static readonly StringName fireReadyEventName = "fireReadyEventName";

		public static readonly StringName fireEventName = "fireEventName";

		public static readonly StringName restAnimeClips = "restAnimeClips";

		public static readonly StringName restAnimeTimeScale = "restAnimeTimeScale";

		public static readonly StringName chargeAnimeClips = "chargeAnimeClips";

		public static readonly StringName chargeAnimeTimeScale = "chargeAnimeTimeScale";

		public static readonly StringName fireAnimeClips = "fireAnimeClips";

		public static readonly StringName fireAnimeTimeScale = "fireAnimeTimeScale";

		public static readonly StringName animationStartPosition = "animationStartPosition";

		public static readonly StringName chargeAudioName = "chargeAudioName";

		public static readonly StringName fireReadyAudioName = "fireReadyAudioName";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName restStateEvent = "restStateEvent";

		public static readonly StringName chargeStateEvent = "chargeStateEvent";

		public static readonly StringName fireStateEvent = "fireStateEvent";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Mode", "")]
	[Export(PropertyHint.Enum, "Marker,Line")]
	public string mode = "Marker";

	[Export(PropertyHint.None, "")]
	public bool autoAttack;

	[Export(PropertyHint.None, "")]
	public double autoAttackMaxXOffset;

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public NodePath projectileSlotPath = new NodePath();

	[Export(PropertyHint.None, "")]
	public NodePath projectileNodePath = new NodePath();

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData projectileData;

	[Export(PropertyHint.None, "")]
	public string projectileLineFireComponentInstanceId = "character.fire";

	[Export(PropertyHint.None, "")]
	public NodePath projectileLineMarkerPath = new NodePath();

	[Export(PropertyHint.None, "")]
	public NodePath spritePath = new NodePath();

	[ExportGroup("Timing", "")]
	[Export(PropertyHint.None, "")]
	public double restTime = 30.0;

	[Export(PropertyHint.None, "")]
	public double firstRestTime = 3.0;

	[Export(PropertyHint.None, "")]
	public bool disableRestTimeout;

	[ExportGroup("Marker Projectile", "")]
	[Export(PropertyHint.None, "")]
	public double markerProjectileHeight = 600.0;

	[Export(PropertyHint.None, "")]
	public double markerProjectileFallSpeed = 800.0;

	[Export(PropertyHint.None, "")]
	public Vector2 markerProjectileHitBoxScale = new Vector2(2f, 1f);

	[Export(PropertyHint.None, "")]
	public double markerVisualTravelHeight = 600.0;

	[Export(PropertyHint.None, "")]
	public double markerVisualTravelDuration = 0.75;

	[Export(PropertyHint.None, "")]
	public bool markerProjectileStraightMove;

	[Export(PropertyHint.None, "")]
	public double markerProjectileStraightSpeed = 600.0;

	[ExportGroup("Line Projectile", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 lineProjectileVelocity = new Vector2(800f, 0f);

	[Export(PropertyHint.None, "")]
	public int lineFirePositionIndex;

	[Export(PropertyHint.None, "")]
	public bool lineProjectileFlipWithParent = true;

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string fireReadyEventName = "fire_ready";

	[Export(PropertyHint.None, "")]
	public string fireEventName = "fire";

	[Export(PropertyHint.None, "")]
	public string restAnimeClips = "Rest";

	[Export(PropertyHint.None, "")]
	public double restAnimeTimeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public string chargeAnimeClips = "Charge";

	[Export(PropertyHint.None, "")]
	public double chargeAnimeTimeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public string fireAnimeClips = "Fire";

	[Export(PropertyHint.None, "")]
	public double fireAnimeTimeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double animationStartPosition = 0.2;

	[ExportGroup("Audio", "")]
	[Export(PropertyHint.None, "")]
	public string chargeAudioName = "Shoop";

	[Export(PropertyHint.None, "")]
	public string fireReadyAudioName = "CobLaunch";

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent = "ToIdle";

	[Export(PropertyHint.None, "")]
	public StringName restStateEvent = "ToRest";

	[Export(PropertyHint.None, "")]
	public StringName chargeStateEvent = "ToCharge";

	[Export(PropertyHint.None, "")]
	public StringName fireStateEvent = "ToFire";

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new CannonComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mode)
		{
			mode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.autoAttack)
		{
			autoAttack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.autoAttackMaxXOffset)
		{
			autoAttackMaxXOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.projectileSlotPath)
		{
			projectileSlotPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.projectileNodePath)
		{
			projectileNodePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.projectileData)
		{
			projectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.projectileLineFireComponentInstanceId)
		{
			projectileLineFireComponentInstanceId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.projectileLineMarkerPath)
		{
			projectileLineMarkerPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.restTime)
		{
			restTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.firstRestTime)
		{
			firstRestTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.disableRestTimeout)
		{
			disableRestTimeout = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.markerProjectileHeight)
		{
			markerProjectileHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.markerProjectileFallSpeed)
		{
			markerProjectileFallSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.markerProjectileHitBoxScale)
		{
			markerProjectileHitBoxScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.markerVisualTravelHeight)
		{
			markerVisualTravelHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.markerVisualTravelDuration)
		{
			markerVisualTravelDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.markerProjectileStraightMove)
		{
			markerProjectileStraightMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.markerProjectileStraightSpeed)
		{
			markerProjectileStraightSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.lineProjectileVelocity)
		{
			lineProjectileVelocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.lineFirePositionIndex)
		{
			lineFirePositionIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.lineProjectileFlipWithParent)
		{
			lineProjectileFlipWithParent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireReadyEventName)
		{
			fireReadyEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireEventName)
		{
			fireEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.restAnimeClips)
		{
			restAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.restAnimeTimeScale)
		{
			restAnimeTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.chargeAnimeClips)
		{
			chargeAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.chargeAnimeTimeScale)
		{
			chargeAnimeTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeClips)
		{
			fireAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeTimeScale)
		{
			fireAnimeTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.animationStartPosition)
		{
			animationStartPosition = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.chargeAudioName)
		{
			chargeAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireReadyAudioName)
		{
			fireReadyAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.restStateEvent)
		{
			restStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.chargeStateEvent)
		{
			chargeStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.fireStateEvent)
		{
			fireStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mode)
		{
			value = VariantUtils.CreateFrom(in mode);
			return true;
		}
		if (name == PropertyName.autoAttack)
		{
			value = VariantUtils.CreateFrom(in autoAttack);
			return true;
		}
		if (name == PropertyName.autoAttackMaxXOffset)
		{
			value = VariantUtils.CreateFrom(in autoAttackMaxXOffset);
			return true;
		}
		if (name == PropertyName.projectileSlotPath)
		{
			value = VariantUtils.CreateFrom(in projectileSlotPath);
			return true;
		}
		if (name == PropertyName.projectileNodePath)
		{
			value = VariantUtils.CreateFrom(in projectileNodePath);
			return true;
		}
		if (name == PropertyName.projectileData)
		{
			value = VariantUtils.CreateFrom(in projectileData);
			return true;
		}
		if (name == PropertyName.projectileLineFireComponentInstanceId)
		{
			value = VariantUtils.CreateFrom(in projectileLineFireComponentInstanceId);
			return true;
		}
		if (name == PropertyName.projectileLineMarkerPath)
		{
			value = VariantUtils.CreateFrom(in projectileLineMarkerPath);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom(in spritePath);
			return true;
		}
		if (name == PropertyName.restTime)
		{
			value = VariantUtils.CreateFrom(in restTime);
			return true;
		}
		if (name == PropertyName.firstRestTime)
		{
			value = VariantUtils.CreateFrom(in firstRestTime);
			return true;
		}
		if (name == PropertyName.disableRestTimeout)
		{
			value = VariantUtils.CreateFrom(in disableRestTimeout);
			return true;
		}
		if (name == PropertyName.markerProjectileHeight)
		{
			value = VariantUtils.CreateFrom(in markerProjectileHeight);
			return true;
		}
		if (name == PropertyName.markerProjectileFallSpeed)
		{
			value = VariantUtils.CreateFrom(in markerProjectileFallSpeed);
			return true;
		}
		if (name == PropertyName.markerProjectileHitBoxScale)
		{
			value = VariantUtils.CreateFrom(in markerProjectileHitBoxScale);
			return true;
		}
		if (name == PropertyName.markerVisualTravelHeight)
		{
			value = VariantUtils.CreateFrom(in markerVisualTravelHeight);
			return true;
		}
		if (name == PropertyName.markerVisualTravelDuration)
		{
			value = VariantUtils.CreateFrom(in markerVisualTravelDuration);
			return true;
		}
		if (name == PropertyName.markerProjectileStraightMove)
		{
			value = VariantUtils.CreateFrom(in markerProjectileStraightMove);
			return true;
		}
		if (name == PropertyName.markerProjectileStraightSpeed)
		{
			value = VariantUtils.CreateFrom(in markerProjectileStraightSpeed);
			return true;
		}
		if (name == PropertyName.lineProjectileVelocity)
		{
			value = VariantUtils.CreateFrom(in lineProjectileVelocity);
			return true;
		}
		if (name == PropertyName.lineFirePositionIndex)
		{
			value = VariantUtils.CreateFrom(in lineFirePositionIndex);
			return true;
		}
		if (name == PropertyName.lineProjectileFlipWithParent)
		{
			value = VariantUtils.CreateFrom(in lineProjectileFlipWithParent);
			return true;
		}
		if (name == PropertyName.fireReadyEventName)
		{
			value = VariantUtils.CreateFrom(in fireReadyEventName);
			return true;
		}
		if (name == PropertyName.fireEventName)
		{
			value = VariantUtils.CreateFrom(in fireEventName);
			return true;
		}
		if (name == PropertyName.restAnimeClips)
		{
			value = VariantUtils.CreateFrom(in restAnimeClips);
			return true;
		}
		if (name == PropertyName.restAnimeTimeScale)
		{
			value = VariantUtils.CreateFrom(in restAnimeTimeScale);
			return true;
		}
		if (name == PropertyName.chargeAnimeClips)
		{
			value = VariantUtils.CreateFrom(in chargeAnimeClips);
			return true;
		}
		if (name == PropertyName.chargeAnimeTimeScale)
		{
			value = VariantUtils.CreateFrom(in chargeAnimeTimeScale);
			return true;
		}
		if (name == PropertyName.fireAnimeClips)
		{
			value = VariantUtils.CreateFrom(in fireAnimeClips);
			return true;
		}
		if (name == PropertyName.fireAnimeTimeScale)
		{
			value = VariantUtils.CreateFrom(in fireAnimeTimeScale);
			return true;
		}
		if (name == PropertyName.animationStartPosition)
		{
			value = VariantUtils.CreateFrom(in animationStartPosition);
			return true;
		}
		if (name == PropertyName.chargeAudioName)
		{
			value = VariantUtils.CreateFrom(in chargeAudioName);
			return true;
		}
		if (name == PropertyName.fireReadyAudioName)
		{
			value = VariantUtils.CreateFrom(in fireReadyAudioName);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			value = VariantUtils.CreateFrom(in idleStateEvent);
			return true;
		}
		if (name == PropertyName.restStateEvent)
		{
			value = VariantUtils.CreateFrom(in restStateEvent);
			return true;
		}
		if (name == PropertyName.chargeStateEvent)
		{
			value = VariantUtils.CreateFrom(in chargeStateEvent);
			return true;
		}
		if (name == PropertyName.fireStateEvent)
		{
			value = VariantUtils.CreateFrom(in fireStateEvent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Mode", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.mode, PropertyHint.Enum, "Marker,Line", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoAttack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.autoAttackMaxXOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.projectileSlotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.projectileNodePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileData, PropertyHint.ResourceType, "TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileLineFireComponentInstanceId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.projectileLineMarkerPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Timing", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.restTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.firstRestTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.disableRestTimeout, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Marker Projectile", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.markerProjectileHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.markerProjectileFallSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.markerProjectileHitBoxScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.markerVisualTravelHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.markerVisualTravelDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.markerProjectileStraightMove, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.markerProjectileStraightSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Line Projectile", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.lineProjectileVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.lineFirePositionIndex, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.lineProjectileFlipWithParent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireReadyEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.restAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.restAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.chargeAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.chargeAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.animationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Audio", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.chargeAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireReadyAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.restStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.chargeStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.fireStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mode, Variant.From(in mode));
		info.AddProperty(PropertyName.autoAttack, Variant.From(in autoAttack));
		info.AddProperty(PropertyName.autoAttackMaxXOffset, Variant.From(in autoAttackMaxXOffset));
		info.AddProperty(PropertyName.projectileSlotPath, Variant.From(in projectileSlotPath));
		info.AddProperty(PropertyName.projectileNodePath, Variant.From(in projectileNodePath));
		info.AddProperty(PropertyName.projectileData, Variant.From(in projectileData));
		info.AddProperty(PropertyName.projectileLineFireComponentInstanceId, Variant.From(in projectileLineFireComponentInstanceId));
		info.AddProperty(PropertyName.projectileLineMarkerPath, Variant.From(in projectileLineMarkerPath));
		info.AddProperty(PropertyName.spritePath, Variant.From(in spritePath));
		info.AddProperty(PropertyName.restTime, Variant.From(in restTime));
		info.AddProperty(PropertyName.firstRestTime, Variant.From(in firstRestTime));
		info.AddProperty(PropertyName.disableRestTimeout, Variant.From(in disableRestTimeout));
		info.AddProperty(PropertyName.markerProjectileHeight, Variant.From(in markerProjectileHeight));
		info.AddProperty(PropertyName.markerProjectileFallSpeed, Variant.From(in markerProjectileFallSpeed));
		info.AddProperty(PropertyName.markerProjectileHitBoxScale, Variant.From(in markerProjectileHitBoxScale));
		info.AddProperty(PropertyName.markerVisualTravelHeight, Variant.From(in markerVisualTravelHeight));
		info.AddProperty(PropertyName.markerVisualTravelDuration, Variant.From(in markerVisualTravelDuration));
		info.AddProperty(PropertyName.markerProjectileStraightMove, Variant.From(in markerProjectileStraightMove));
		info.AddProperty(PropertyName.markerProjectileStraightSpeed, Variant.From(in markerProjectileStraightSpeed));
		info.AddProperty(PropertyName.lineProjectileVelocity, Variant.From(in lineProjectileVelocity));
		info.AddProperty(PropertyName.lineFirePositionIndex, Variant.From(in lineFirePositionIndex));
		info.AddProperty(PropertyName.lineProjectileFlipWithParent, Variant.From(in lineProjectileFlipWithParent));
		info.AddProperty(PropertyName.fireReadyEventName, Variant.From(in fireReadyEventName));
		info.AddProperty(PropertyName.fireEventName, Variant.From(in fireEventName));
		info.AddProperty(PropertyName.restAnimeClips, Variant.From(in restAnimeClips));
		info.AddProperty(PropertyName.restAnimeTimeScale, Variant.From(in restAnimeTimeScale));
		info.AddProperty(PropertyName.chargeAnimeClips, Variant.From(in chargeAnimeClips));
		info.AddProperty(PropertyName.chargeAnimeTimeScale, Variant.From(in chargeAnimeTimeScale));
		info.AddProperty(PropertyName.fireAnimeClips, Variant.From(in fireAnimeClips));
		info.AddProperty(PropertyName.fireAnimeTimeScale, Variant.From(in fireAnimeTimeScale));
		info.AddProperty(PropertyName.animationStartPosition, Variant.From(in animationStartPosition));
		info.AddProperty(PropertyName.chargeAudioName, Variant.From(in chargeAudioName));
		info.AddProperty(PropertyName.fireReadyAudioName, Variant.From(in fireReadyAudioName));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From(in idleStateEvent));
		info.AddProperty(PropertyName.restStateEvent, Variant.From(in restStateEvent));
		info.AddProperty(PropertyName.chargeStateEvent, Variant.From(in chargeStateEvent));
		info.AddProperty(PropertyName.fireStateEvent, Variant.From(in fireStateEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mode, out var value))
		{
			mode = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.autoAttack, out var value2))
		{
			autoAttack = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.autoAttackMaxXOffset, out var value3))
		{
			autoAttackMaxXOffset = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.projectileSlotPath, out var value4))
		{
			projectileSlotPath = value4.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.projectileNodePath, out var value5))
		{
			projectileNodePath = value5.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.projectileData, out var value6))
		{
			projectileData = value6.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.projectileLineFireComponentInstanceId, out var value7))
		{
			projectileLineFireComponentInstanceId = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.projectileLineMarkerPath, out var value8))
		{
			projectileLineMarkerPath = value8.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value9))
		{
			spritePath = value9.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.restTime, out var value10))
		{
			restTime = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.firstRestTime, out var value11))
		{
			firstRestTime = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.disableRestTimeout, out var value12))
		{
			disableRestTimeout = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.markerProjectileHeight, out var value13))
		{
			markerProjectileHeight = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.markerProjectileFallSpeed, out var value14))
		{
			markerProjectileFallSpeed = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.markerProjectileHitBoxScale, out var value15))
		{
			markerProjectileHitBoxScale = value15.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.markerVisualTravelHeight, out var value16))
		{
			markerVisualTravelHeight = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName.markerVisualTravelDuration, out var value17))
		{
			markerVisualTravelDuration = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName.markerProjectileStraightMove, out var value18))
		{
			markerProjectileStraightMove = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.markerProjectileStraightSpeed, out var value19))
		{
			markerProjectileStraightSpeed = value19.As<double>();
		}
		if (info.TryGetProperty(PropertyName.lineProjectileVelocity, out var value20))
		{
			lineProjectileVelocity = value20.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.lineFirePositionIndex, out var value21))
		{
			lineFirePositionIndex = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName.lineProjectileFlipWithParent, out var value22))
		{
			lineProjectileFlipWithParent = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireReadyEventName, out var value23))
		{
			fireReadyEventName = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireEventName, out var value24))
		{
			fireEventName = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName.restAnimeClips, out var value25))
		{
			restAnimeClips = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName.restAnimeTimeScale, out var value26))
		{
			restAnimeTimeScale = value26.As<double>();
		}
		if (info.TryGetProperty(PropertyName.chargeAnimeClips, out var value27))
		{
			chargeAnimeClips = value27.As<string>();
		}
		if (info.TryGetProperty(PropertyName.chargeAnimeTimeScale, out var value28))
		{
			chargeAnimeTimeScale = value28.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeClips, out var value29))
		{
			fireAnimeClips = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeTimeScale, out var value30))
		{
			fireAnimeTimeScale = value30.As<double>();
		}
		if (info.TryGetProperty(PropertyName.animationStartPosition, out var value31))
		{
			animationStartPosition = value31.As<double>();
		}
		if (info.TryGetProperty(PropertyName.chargeAudioName, out var value32))
		{
			chargeAudioName = value32.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireReadyAudioName, out var value33))
		{
			fireReadyAudioName = value33.As<string>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value34))
		{
			idleStateEvent = value34.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.restStateEvent, out var value35))
		{
			restStateEvent = value35.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.chargeStateEvent, out var value36))
		{
			chargeStateEvent = value36.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.fireStateEvent, out var value37))
		{
			fireStateEvent = value37.As<StringName>();
		}
	}
}
