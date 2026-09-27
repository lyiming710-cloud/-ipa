using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/SquashComponent/SquashComponentDefinition.cs")]
public class SquashComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName attackComponentInstanceId = "attackComponentInstanceId";

		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName hurtRange = "hurtRange";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName checkAliveCharacter = "checkAliveCharacter";

		public static readonly StringName aliveCheckPhysicsFrames = "aliveCheckPhysicsFrames";

		public static readonly StringName cleanupDelay = "cleanupDelay";

		public static readonly StringName jumpHeight = "jumpHeight";

		public static readonly StringName targetPositionDelay = "targetPositionDelay";

		public static readonly StringName noLookFallbackDelay = "noLookFallbackDelay";

		public static readonly StringName lookCompletionDelay = "lookCompletionDelay";

		public static readonly StringName jumpUpLeadDelay = "jumpUpLeadDelay";

		public static readonly StringName jumpTravelDuration = "jumpTravelDuration";

		public static readonly StringName noJumpUpLandingDelay = "noJumpUpLandingDelay";

		public static readonly StringName descentStartDelay = "descentStartDelay";

		public static readonly StringName descentDuration = "descentDuration";

		public static readonly StringName trackMovingTargetUntilImpact = "trackMovingTargetUntilImpact";

		public static readonly StringName completeStartedSequenceOutsideComponentBattlefield = "completeStartedSequenceOutsideComponentBattlefield";

		public static readonly StringName lookLeftAnimeClip = "lookLeftAnimeClip";

		public static readonly StringName lookRightAnimeClip = "lookRightAnimeClip";

		public static readonly StringName lookAnimeTimeScale = "lookAnimeTimeScale";

		public static readonly StringName jumpUpAnimeClip = "jumpUpAnimeClip";

		public static readonly StringName jumpDownAnimeClip = "jumpDownAnimeClip";

		public static readonly StringName jumpAnimeTimeScale = "jumpAnimeTimeScale";

		public static readonly StringName lookAnimationStartPosition = "lookAnimationStartPosition";

		public static readonly StringName jumpUpAnimationStartPosition = "jumpUpAnimationStartPosition";

		public static readonly StringName jumpDownAnimationStartPosition = "jumpDownAnimationStartPosition";

		public static readonly StringName readyAudioName = "readyAudioName";

		public static readonly StringName smashAudioName = "smashAudioName";

		public static readonly StringName waterAudioName = "waterAudioName";

		public static readonly StringName cameraShakeRange = "cameraShakeRange";

		public static readonly StringName cameraShakeStrength = "cameraShakeStrength";

		public static readonly StringName cameraShakeDuration = "cameraShakeDuration";

		public static readonly StringName cameraShakeFrequency = "cameraShakeFrequency";

		public static readonly StringName readyStateEvent = "readyStateEvent";

		public static readonly StringName jumpStateEvent = "jumpStateEvent";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public string attackComponentInstanceId { get; set; } = "character.attack.0";

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[ExportGroup("Impact", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 hurtRange { get; set; } = new Vector2(0.625f, 0.2f);

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList { get; set; } = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public bool checkAliveCharacter { get; set; }

	[Export(PropertyHint.None, "")]
	public int aliveCheckPhysicsFrames { get; set; } = 2;

	[Export(PropertyHint.None, "")]
	public double cleanupDelay { get; set; } = 0.5;

	[ExportGroup("Movement", "")]
	[Export(PropertyHint.None, "")]
	public double jumpHeight { get; set; } = 120.0;

	[Export(PropertyHint.None, "")]
	public double targetPositionDelay { get; set; }

	[Export(PropertyHint.None, "")]
	public double noLookFallbackDelay { get; set; } = 0.1;

	[Export(PropertyHint.None, "")]
	public double lookCompletionDelay { get; set; } = 0.5;

	[Export(PropertyHint.None, "")]
	public double jumpUpLeadDelay { get; set; } = 0.3;

	[Export(PropertyHint.None, "")]
	public double jumpTravelDuration { get; set; } = 0.4;

	[Export(PropertyHint.None, "")]
	public double noJumpUpLandingDelay { get; set; } = 0.1;

	[Export(PropertyHint.None, "")]
	public double descentStartDelay { get; set; } = 0.4;

	[Export(PropertyHint.None, "")]
	public double descentDuration { get; set; } = 0.1;

	[Export(PropertyHint.None, "")]
	public bool trackMovingTargetUntilImpact { get; set; }

	[Export(PropertyHint.None, "")]
	public bool completeStartedSequenceOutsideComponentBattlefield { get; set; }

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string lookLeftAnimeClip { get; set; } = "LookLeft";

	[Export(PropertyHint.None, "")]
	public string lookRightAnimeClip { get; set; } = "LookRight";

	[Export(PropertyHint.None, "")]
	public float lookAnimeTimeScale { get; set; } = 2f;

	[Export(PropertyHint.None, "")]
	public string jumpUpAnimeClip { get; set; } = "JumpUp";

	[Export(PropertyHint.None, "")]
	public string jumpDownAnimeClip { get; set; } = "JumpDown";

	[Export(PropertyHint.None, "")]
	public float jumpAnimeTimeScale { get; set; } = 3f;

	[Export(PropertyHint.None, "")]
	public float lookAnimationStartPosition { get; set; } = 0.1f;

	[Export(PropertyHint.None, "")]
	public float jumpUpAnimationStartPosition { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public float jumpDownAnimationStartPosition { get; set; } = 0.1f;

	[ExportGroup("Audio and Camera", "")]
	[Export(PropertyHint.None, "")]
	public string readyAudioName { get; set; } = "SquasHmm";

	[Export(PropertyHint.None, "")]
	public string smashAudioName { get; set; } = "GargantuarThump";

	[Export(PropertyHint.None, "")]
	public string waterAudioName { get; set; } = "PlantWater";

	[Export(PropertyHint.None, "")]
	public Vector2 cameraShakeRange { get; set; } = Vector2.One;

	[Export(PropertyHint.None, "")]
	public double cameraShakeStrength { get; set; } = 5.0;

	[Export(PropertyHint.None, "")]
	public double cameraShakeDuration { get; set; } = 0.05;

	[Export(PropertyHint.None, "")]
	public int cameraShakeFrequency { get; set; } = 4;

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName readyStateEvent { get; set; } = "ToReady";

	[Export(PropertyHint.None, "")]
	public StringName jumpStateEvent { get; set; } = "ToJump";

	public int CollisionPreviewShapeCount => GodotObject.IsInstanceValid(checkShape) ? 1 : 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0)
		{
			return null;
		}
		return checkShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new SquashComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetCollisionPreviewShape, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCollisionPreviewRay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetCollisionPreviewShape && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbShape2DResource>(GetCollisionPreviewShape(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbRay2DResource>(GetCollisionPreviewRay(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetCollisionPreviewShape)
		{
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.attackComponentInstanceId)
		{
			attackComponentInstanceId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.hurtRange)
		{
			hurtRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.checkAliveCharacter)
		{
			checkAliveCharacter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.aliveCheckPhysicsFrames)
		{
			aliveCheckPhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.cleanupDelay)
		{
			cleanupDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpHeight)
		{
			jumpHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.targetPositionDelay)
		{
			targetPositionDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.noLookFallbackDelay)
		{
			noLookFallbackDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.lookCompletionDelay)
		{
			lookCompletionDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpUpLeadDelay)
		{
			jumpUpLeadDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpTravelDuration)
		{
			jumpTravelDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.noJumpUpLandingDelay)
		{
			noJumpUpLandingDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.descentStartDelay)
		{
			descentStartDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.descentDuration)
		{
			descentDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.trackMovingTargetUntilImpact)
		{
			trackMovingTargetUntilImpact = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.completeStartedSequenceOutsideComponentBattlefield)
		{
			completeStartedSequenceOutsideComponentBattlefield = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.lookLeftAnimeClip)
		{
			lookLeftAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.lookRightAnimeClip)
		{
			lookRightAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.lookAnimeTimeScale)
		{
			lookAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.jumpUpAnimeClip)
		{
			jumpUpAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.jumpDownAnimeClip)
		{
			jumpDownAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.jumpAnimeTimeScale)
		{
			jumpAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.lookAnimationStartPosition)
		{
			lookAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.jumpUpAnimationStartPosition)
		{
			jumpUpAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.jumpDownAnimationStartPosition)
		{
			jumpDownAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.readyAudioName)
		{
			readyAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.smashAudioName)
		{
			smashAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.waterAudioName)
		{
			waterAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeRange)
		{
			cameraShakeRange = VariantUtils.ConvertTo<Vector2>(in value);
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
		if (name == PropertyName.readyStateEvent)
		{
			readyStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.jumpStateEvent)
		{
			jumpStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom<NodePath>(spritePath);
			return true;
		}
		string from;
		if (name == PropertyName.attackComponentInstanceId)
		{
			from = attackComponentInstanceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(checkShape);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.hurtRange)
		{
			from2 = hurtRange;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		bool from3;
		if (name == PropertyName.checkAliveCharacter)
		{
			from3 = checkAliveCharacter;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		int from4;
		if (name == PropertyName.aliveCheckPhysicsFrames)
		{
			from4 = aliveCheckPhysicsFrames;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		double from5;
		if (name == PropertyName.cleanupDelay)
		{
			from5 = cleanupDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.jumpHeight)
		{
			from5 = jumpHeight;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.targetPositionDelay)
		{
			from5 = targetPositionDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.noLookFallbackDelay)
		{
			from5 = noLookFallbackDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.lookCompletionDelay)
		{
			from5 = lookCompletionDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.jumpUpLeadDelay)
		{
			from5 = jumpUpLeadDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.jumpTravelDuration)
		{
			from5 = jumpTravelDuration;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.noJumpUpLandingDelay)
		{
			from5 = noJumpUpLandingDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.descentStartDelay)
		{
			from5 = descentStartDelay;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.descentDuration)
		{
			from5 = descentDuration;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.trackMovingTargetUntilImpact)
		{
			from3 = trackMovingTargetUntilImpact;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.completeStartedSequenceOutsideComponentBattlefield)
		{
			from3 = completeStartedSequenceOutsideComponentBattlefield;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.lookLeftAnimeClip)
		{
			from = lookLeftAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.lookRightAnimeClip)
		{
			from = lookRightAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from6;
		if (name == PropertyName.lookAnimeTimeScale)
		{
			from6 = lookAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.jumpUpAnimeClip)
		{
			from = jumpUpAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.jumpDownAnimeClip)
		{
			from = jumpDownAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.jumpAnimeTimeScale)
		{
			from6 = jumpAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.lookAnimationStartPosition)
		{
			from6 = lookAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.jumpUpAnimationStartPosition)
		{
			from6 = jumpUpAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.jumpDownAnimationStartPosition)
		{
			from6 = jumpDownAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.readyAudioName)
		{
			from = readyAudioName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.smashAudioName)
		{
			from = smashAudioName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.waterAudioName)
		{
			from = waterAudioName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.cameraShakeRange)
		{
			from2 = cameraShakeRange;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.cameraShakeStrength)
		{
			from5 = cameraShakeStrength;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.cameraShakeDuration)
		{
			from5 = cameraShakeDuration;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.cameraShakeFrequency)
		{
			from4 = cameraShakeFrequency;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		StringName from7;
		if (name == PropertyName.readyStateEvent)
		{
			from7 = readyStateEvent;
			value = VariantUtils.CreateFrom(in from7);
			return true;
		}
		if (name == PropertyName.jumpStateEvent)
		{
			from7 = jumpStateEvent;
			value = VariantUtils.CreateFrom(in from7);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from4 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from4 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackComponentInstanceId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Impact", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.hurtRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAliveCharacter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.aliveCheckPhysicsFrames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cleanupDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Movement", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.targetPositionDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.noLookFallbackDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.lookCompletionDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpUpLeadDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpTravelDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.noJumpUpLandingDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.descentStartDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.descentDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackMovingTargetUntilImpact, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.completeStartedSequenceOutsideComponentBattlefield, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.lookLeftAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.lookRightAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.lookAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.jumpUpAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.jumpDownAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.lookAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpUpAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpDownAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Audio and Camera", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.readyAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.smashAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.waterAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.cameraShakeRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeStrength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cameraShakeFrequency, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.readyStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.jumpStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.attackComponentInstanceId, Variant.From<string>(attackComponentInstanceId));
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.hurtRange, Variant.From<Vector2>(hurtRange));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.checkAliveCharacter, Variant.From<bool>(checkAliveCharacter));
		info.AddProperty(PropertyName.aliveCheckPhysicsFrames, Variant.From<int>(aliveCheckPhysicsFrames));
		info.AddProperty(PropertyName.cleanupDelay, Variant.From<double>(cleanupDelay));
		info.AddProperty(PropertyName.jumpHeight, Variant.From<double>(jumpHeight));
		info.AddProperty(PropertyName.targetPositionDelay, Variant.From<double>(targetPositionDelay));
		info.AddProperty(PropertyName.noLookFallbackDelay, Variant.From<double>(noLookFallbackDelay));
		info.AddProperty(PropertyName.lookCompletionDelay, Variant.From<double>(lookCompletionDelay));
		info.AddProperty(PropertyName.jumpUpLeadDelay, Variant.From<double>(jumpUpLeadDelay));
		info.AddProperty(PropertyName.jumpTravelDuration, Variant.From<double>(jumpTravelDuration));
		info.AddProperty(PropertyName.noJumpUpLandingDelay, Variant.From<double>(noJumpUpLandingDelay));
		info.AddProperty(PropertyName.descentStartDelay, Variant.From<double>(descentStartDelay));
		info.AddProperty(PropertyName.descentDuration, Variant.From<double>(descentDuration));
		info.AddProperty(PropertyName.trackMovingTargetUntilImpact, Variant.From<bool>(trackMovingTargetUntilImpact));
		info.AddProperty(PropertyName.completeStartedSequenceOutsideComponentBattlefield, Variant.From<bool>(completeStartedSequenceOutsideComponentBattlefield));
		info.AddProperty(PropertyName.lookLeftAnimeClip, Variant.From<string>(lookLeftAnimeClip));
		info.AddProperty(PropertyName.lookRightAnimeClip, Variant.From<string>(lookRightAnimeClip));
		info.AddProperty(PropertyName.lookAnimeTimeScale, Variant.From<float>(lookAnimeTimeScale));
		info.AddProperty(PropertyName.jumpUpAnimeClip, Variant.From<string>(jumpUpAnimeClip));
		info.AddProperty(PropertyName.jumpDownAnimeClip, Variant.From<string>(jumpDownAnimeClip));
		info.AddProperty(PropertyName.jumpAnimeTimeScale, Variant.From<float>(jumpAnimeTimeScale));
		info.AddProperty(PropertyName.lookAnimationStartPosition, Variant.From<float>(lookAnimationStartPosition));
		info.AddProperty(PropertyName.jumpUpAnimationStartPosition, Variant.From<float>(jumpUpAnimationStartPosition));
		info.AddProperty(PropertyName.jumpDownAnimationStartPosition, Variant.From<float>(jumpDownAnimationStartPosition));
		info.AddProperty(PropertyName.readyAudioName, Variant.From<string>(readyAudioName));
		info.AddProperty(PropertyName.smashAudioName, Variant.From<string>(smashAudioName));
		info.AddProperty(PropertyName.waterAudioName, Variant.From<string>(waterAudioName));
		info.AddProperty(PropertyName.cameraShakeRange, Variant.From<Vector2>(cameraShakeRange));
		info.AddProperty(PropertyName.cameraShakeStrength, Variant.From<double>(cameraShakeStrength));
		info.AddProperty(PropertyName.cameraShakeDuration, Variant.From<double>(cameraShakeDuration));
		info.AddProperty(PropertyName.cameraShakeFrequency, Variant.From<int>(cameraShakeFrequency));
		info.AddProperty(PropertyName.readyStateEvent, Variant.From<StringName>(readyStateEvent));
		info.AddProperty(PropertyName.jumpStateEvent, Variant.From<StringName>(jumpStateEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spritePath, out var value))
		{
			spritePath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.attackComponentInstanceId, out var value2))
		{
			attackComponentInstanceId = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.checkShape, out var value3))
		{
			checkShape = value3.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.hurtRange, out var value4))
		{
			hurtRange = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value5))
		{
			eventList = value5.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.checkAliveCharacter, out var value6))
		{
			checkAliveCharacter = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.aliveCheckPhysicsFrames, out var value7))
		{
			aliveCheckPhysicsFrames = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.cleanupDelay, out var value8))
		{
			cleanupDelay = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpHeight, out var value9))
		{
			jumpHeight = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.targetPositionDelay, out var value10))
		{
			targetPositionDelay = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.noLookFallbackDelay, out var value11))
		{
			noLookFallbackDelay = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.lookCompletionDelay, out var value12))
		{
			lookCompletionDelay = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpUpLeadDelay, out var value13))
		{
			jumpUpLeadDelay = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpTravelDuration, out var value14))
		{
			jumpTravelDuration = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.noJumpUpLandingDelay, out var value15))
		{
			noJumpUpLandingDelay = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.descentStartDelay, out var value16))
		{
			descentStartDelay = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName.descentDuration, out var value17))
		{
			descentDuration = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName.trackMovingTargetUntilImpact, out var value18))
		{
			trackMovingTargetUntilImpact = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.completeStartedSequenceOutsideComponentBattlefield, out var value19))
		{
			completeStartedSequenceOutsideComponentBattlefield = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.lookLeftAnimeClip, out var value20))
		{
			lookLeftAnimeClip = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.lookRightAnimeClip, out var value21))
		{
			lookRightAnimeClip = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName.lookAnimeTimeScale, out var value22))
		{
			lookAnimeTimeScale = value22.As<float>();
		}
		if (info.TryGetProperty(PropertyName.jumpUpAnimeClip, out var value23))
		{
			jumpUpAnimeClip = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.jumpDownAnimeClip, out var value24))
		{
			jumpDownAnimeClip = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName.jumpAnimeTimeScale, out var value25))
		{
			jumpAnimeTimeScale = value25.As<float>();
		}
		if (info.TryGetProperty(PropertyName.lookAnimationStartPosition, out var value26))
		{
			lookAnimationStartPosition = value26.As<float>();
		}
		if (info.TryGetProperty(PropertyName.jumpUpAnimationStartPosition, out var value27))
		{
			jumpUpAnimationStartPosition = value27.As<float>();
		}
		if (info.TryGetProperty(PropertyName.jumpDownAnimationStartPosition, out var value28))
		{
			jumpDownAnimationStartPosition = value28.As<float>();
		}
		if (info.TryGetProperty(PropertyName.readyAudioName, out var value29))
		{
			readyAudioName = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName.smashAudioName, out var value30))
		{
			smashAudioName = value30.As<string>();
		}
		if (info.TryGetProperty(PropertyName.waterAudioName, out var value31))
		{
			waterAudioName = value31.As<string>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeRange, out var value32))
		{
			cameraShakeRange = value32.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeStrength, out var value33))
		{
			cameraShakeStrength = value33.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeDuration, out var value34))
		{
			cameraShakeDuration = value34.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeFrequency, out var value35))
		{
			cameraShakeFrequency = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName.readyStateEvent, out var value36))
		{
			readyStateEvent = value36.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.jumpStateEvent, out var value37))
		{
			jumpStateEvent = value37.As<StringName>();
		}
	}
}
