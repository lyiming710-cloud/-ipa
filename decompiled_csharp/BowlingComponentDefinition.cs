using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BowlingComponent/BowlingComponentDefinition.cs")]
public class BowlingComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName rollXVelocityMax = "rollXVelocityMax";

		public static readonly StringName rollXVelocityMin = "rollXVelocityMin";

		public static readonly StringName rollYVelocity = "rollYVelocity";

		public static readonly StringName selfRotateUse = "selfRotateUse";

		public static readonly StringName selfRotationDivisor = "selfRotationDivisor";

		public static readonly StringName useParentHitBox = "useParentHitBox";

		public static readonly StringName hitEvent = "hitEvent";

		public static readonly StringName maxHitNum = "maxHitNum";

		public static readonly StringName hitLineUse = "hitLineUse";

		public static readonly StringName hitLineBackUse = "hitLineBackUse";

		public static readonly StringName edgeReboundUse = "edgeReboundUse";

		public static readonly StringName offscreenDestroyMargin = "offscreenDestroyMargin";

		public static readonly StringName topBoundaryOffset = "topBoundaryOffset";

		public static readonly StringName bottomBoundaryOffset = "bottomBoundaryOffset";

		public static readonly StringName collisionCheckIntervalPhysicsFrames = "collisionCheckIntervalPhysicsFrames";

		public static readonly StringName fallCoinUse = "fallCoinUse";

		public static readonly StringName hitCoinRewards = "hitCoinRewards";

		public static readonly StringName rollAnimeClips = "rollAnimeClips";

		public static readonly StringName rollAnimeTimeScale = "rollAnimeTimeScale";

		public static readonly StringName dieAnimeClip = "dieAnimeClip";

		public static readonly StringName rollAnimationStartPosition = "rollAnimationStartPosition";

		public static readonly StringName rollAudioName = "rollAudioName";

		public static readonly StringName impactAudioName = "impactAudioName";

		public static readonly StringName rollStateEvent = "rollStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName cameraShakeRange = "cameraShakeRange";

		public static readonly StringName cameraShakeStrength = "cameraShakeStrength";

		public static readonly StringName cameraShakeDuration = "cameraShakeDuration";

		public static readonly StringName cameraShakeFrequency = "cameraShakeFrequency";

		public static readonly StringName checkAreaPath = "checkAreaPath";

		public static readonly StringName spritePath = "spritePath";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Movement", "")]
	[Export(PropertyHint.None, "")]
	public double rollXVelocityMax = 250.0;

	[Export(PropertyHint.None, "")]
	public double rollXVelocityMin = 200.0;

	[Export(PropertyHint.None, "")]
	public double rollYVelocity = 250.0;

	[Export(PropertyHint.None, "")]
	public bool selfRotateUse;

	[Export(PropertyHint.None, "")]
	public double selfRotationDivisor = 1000.0;

	[ExportGroup("Collision", "")]
	[Export(PropertyHint.None, "")]
	public bool useParentHitBox;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitEvent = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public int maxHitNum = -1;

	[Export(PropertyHint.None, "")]
	public bool hitLineUse;

	[Export(PropertyHint.None, "")]
	public bool hitLineBackUse;

	[Export(PropertyHint.None, "")]
	public bool edgeReboundUse;

	[ExportGroup("Map Boundaries", "")]
	[Export(PropertyHint.None, "")]
	public double offscreenDestroyMargin = 300.0;

	[Export(PropertyHint.None, "")]
	public double topBoundaryOffset = 50.0;

	[Export(PropertyHint.None, "")]
	public double bottomBoundaryOffset = 20.0;

	[Export(PropertyHint.Range, "1,10,1")]
	public int collisionCheckIntervalPhysicsFrames = 1;

	[ExportGroup("Rewards", "")]
	[Export(PropertyHint.None, "")]
	public bool fallCoinUse = true;

	[Export(PropertyHint.None, "")]
	public Array<int> hitCoinRewards = new Array<int> { 0, 0, 0, 10, 10, 10, 20 };

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string rollAnimeClips = "Roll";

	[Export(PropertyHint.None, "")]
	public double rollAnimeTimeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public string dieAnimeClip = "Death";

	[Export(PropertyHint.None, "")]
	public double rollAnimationStartPosition = 0.2;

	[ExportGroup("Audio", "")]
	[Export(PropertyHint.None, "")]
	public string rollAudioName = "Bowling";

	[Export(PropertyHint.None, "")]
	public string impactAudioName = "BowlingImpact";

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName rollStateEvent = "ToRoll";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent = "ToIdle";

	[ExportGroup("Camera Shake", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 cameraShakeRange = Vector2.One;

	[Export(PropertyHint.None, "")]
	public double cameraShakeStrength = 2.0;

	[Export(PropertyHint.None, "")]
	public double cameraShakeDuration = 0.05;

	[Export(PropertyHint.None, "")]
	public int cameraShakeFrequency = 4;

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public NodePath checkAreaPath = new NodePath();

	[Export(PropertyHint.None, "")]
	public NodePath spritePath = new NodePath();

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new BowlingComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.rollXVelocityMax)
		{
			rollXVelocityMax = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.rollXVelocityMin)
		{
			rollXVelocityMin = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.rollYVelocity)
		{
			rollYVelocity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.selfRotateUse)
		{
			selfRotateUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.selfRotationDivisor)
		{
			selfRotationDivisor = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.useParentHitBox)
		{
			useParentHitBox = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hitEvent)
		{
			hitEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.maxHitNum)
		{
			maxHitNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hitLineUse)
		{
			hitLineUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hitLineBackUse)
		{
			hitLineBackUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.edgeReboundUse)
		{
			edgeReboundUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.offscreenDestroyMargin)
		{
			offscreenDestroyMargin = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.topBoundaryOffset)
		{
			topBoundaryOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.bottomBoundaryOffset)
		{
			bottomBoundaryOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.collisionCheckIntervalPhysicsFrames)
		{
			collisionCheckIntervalPhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fallCoinUse)
		{
			fallCoinUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hitCoinRewards)
		{
			hitCoinRewards = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.rollAnimeClips)
		{
			rollAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.rollAnimeTimeScale)
		{
			rollAnimeTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dieAnimeClip)
		{
			dieAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.rollAnimationStartPosition)
		{
			rollAnimationStartPosition = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.rollAudioName)
		{
			rollAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.impactAudioName)
		{
			impactAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.rollStateEvent)
		{
			rollStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
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
		if (name == PropertyName.checkAreaPath)
		{
			checkAreaPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.rollXVelocityMax)
		{
			value = VariantUtils.CreateFrom(in rollXVelocityMax);
			return true;
		}
		if (name == PropertyName.rollXVelocityMin)
		{
			value = VariantUtils.CreateFrom(in rollXVelocityMin);
			return true;
		}
		if (name == PropertyName.rollYVelocity)
		{
			value = VariantUtils.CreateFrom(in rollYVelocity);
			return true;
		}
		if (name == PropertyName.selfRotateUse)
		{
			value = VariantUtils.CreateFrom(in selfRotateUse);
			return true;
		}
		if (name == PropertyName.selfRotationDivisor)
		{
			value = VariantUtils.CreateFrom(in selfRotationDivisor);
			return true;
		}
		if (name == PropertyName.useParentHitBox)
		{
			value = VariantUtils.CreateFrom(in useParentHitBox);
			return true;
		}
		if (name == PropertyName.hitEvent)
		{
			value = VariantUtils.CreateFromArray(hitEvent);
			return true;
		}
		if (name == PropertyName.maxHitNum)
		{
			value = VariantUtils.CreateFrom(in maxHitNum);
			return true;
		}
		if (name == PropertyName.hitLineUse)
		{
			value = VariantUtils.CreateFrom(in hitLineUse);
			return true;
		}
		if (name == PropertyName.hitLineBackUse)
		{
			value = VariantUtils.CreateFrom(in hitLineBackUse);
			return true;
		}
		if (name == PropertyName.edgeReboundUse)
		{
			value = VariantUtils.CreateFrom(in edgeReboundUse);
			return true;
		}
		if (name == PropertyName.offscreenDestroyMargin)
		{
			value = VariantUtils.CreateFrom(in offscreenDestroyMargin);
			return true;
		}
		if (name == PropertyName.topBoundaryOffset)
		{
			value = VariantUtils.CreateFrom(in topBoundaryOffset);
			return true;
		}
		if (name == PropertyName.bottomBoundaryOffset)
		{
			value = VariantUtils.CreateFrom(in bottomBoundaryOffset);
			return true;
		}
		if (name == PropertyName.collisionCheckIntervalPhysicsFrames)
		{
			value = VariantUtils.CreateFrom(in collisionCheckIntervalPhysicsFrames);
			return true;
		}
		if (name == PropertyName.fallCoinUse)
		{
			value = VariantUtils.CreateFrom(in fallCoinUse);
			return true;
		}
		if (name == PropertyName.hitCoinRewards)
		{
			value = VariantUtils.CreateFromArray(hitCoinRewards);
			return true;
		}
		if (name == PropertyName.rollAnimeClips)
		{
			value = VariantUtils.CreateFrom(in rollAnimeClips);
			return true;
		}
		if (name == PropertyName.rollAnimeTimeScale)
		{
			value = VariantUtils.CreateFrom(in rollAnimeTimeScale);
			return true;
		}
		if (name == PropertyName.dieAnimeClip)
		{
			value = VariantUtils.CreateFrom(in dieAnimeClip);
			return true;
		}
		if (name == PropertyName.rollAnimationStartPosition)
		{
			value = VariantUtils.CreateFrom(in rollAnimationStartPosition);
			return true;
		}
		if (name == PropertyName.rollAudioName)
		{
			value = VariantUtils.CreateFrom(in rollAudioName);
			return true;
		}
		if (name == PropertyName.impactAudioName)
		{
			value = VariantUtils.CreateFrom(in impactAudioName);
			return true;
		}
		if (name == PropertyName.rollStateEvent)
		{
			value = VariantUtils.CreateFrom(in rollStateEvent);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			value = VariantUtils.CreateFrom(in idleStateEvent);
			return true;
		}
		if (name == PropertyName.cameraShakeRange)
		{
			value = VariantUtils.CreateFrom(in cameraShakeRange);
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
		if (name == PropertyName.checkAreaPath)
		{
			value = VariantUtils.CreateFrom(in checkAreaPath);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom(in spritePath);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.rollXVelocityMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rollXVelocityMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rollYVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.selfRotateUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.selfRotationDivisor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Collision", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useParentHitBox, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxHitNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitLineUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitLineBackUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.edgeReboundUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Map Boundaries", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.offscreenDestroyMargin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.topBoundaryOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.bottomBoundaryOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionCheckIntervalPhysicsFrames, PropertyHint.Range, "1,10,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Rewards", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.fallCoinUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitCoinRewards, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.rollAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rollAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dieAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rollAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Audio", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.rollAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.impactAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.rollStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Camera Shake", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.cameraShakeRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeStrength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cameraShakeFrequency, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.checkAreaPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.rollXVelocityMax, Variant.From(in rollXVelocityMax));
		info.AddProperty(PropertyName.rollXVelocityMin, Variant.From(in rollXVelocityMin));
		info.AddProperty(PropertyName.rollYVelocity, Variant.From(in rollYVelocity));
		info.AddProperty(PropertyName.selfRotateUse, Variant.From(in selfRotateUse));
		info.AddProperty(PropertyName.selfRotationDivisor, Variant.From(in selfRotationDivisor));
		info.AddProperty(PropertyName.useParentHitBox, Variant.From(in useParentHitBox));
		info.AddProperty(PropertyName.hitEvent, Variant.CreateFrom(hitEvent));
		info.AddProperty(PropertyName.maxHitNum, Variant.From(in maxHitNum));
		info.AddProperty(PropertyName.hitLineUse, Variant.From(in hitLineUse));
		info.AddProperty(PropertyName.hitLineBackUse, Variant.From(in hitLineBackUse));
		info.AddProperty(PropertyName.edgeReboundUse, Variant.From(in edgeReboundUse));
		info.AddProperty(PropertyName.offscreenDestroyMargin, Variant.From(in offscreenDestroyMargin));
		info.AddProperty(PropertyName.topBoundaryOffset, Variant.From(in topBoundaryOffset));
		info.AddProperty(PropertyName.bottomBoundaryOffset, Variant.From(in bottomBoundaryOffset));
		info.AddProperty(PropertyName.collisionCheckIntervalPhysicsFrames, Variant.From(in collisionCheckIntervalPhysicsFrames));
		info.AddProperty(PropertyName.fallCoinUse, Variant.From(in fallCoinUse));
		info.AddProperty(PropertyName.hitCoinRewards, Variant.CreateFrom(hitCoinRewards));
		info.AddProperty(PropertyName.rollAnimeClips, Variant.From(in rollAnimeClips));
		info.AddProperty(PropertyName.rollAnimeTimeScale, Variant.From(in rollAnimeTimeScale));
		info.AddProperty(PropertyName.dieAnimeClip, Variant.From(in dieAnimeClip));
		info.AddProperty(PropertyName.rollAnimationStartPosition, Variant.From(in rollAnimationStartPosition));
		info.AddProperty(PropertyName.rollAudioName, Variant.From(in rollAudioName));
		info.AddProperty(PropertyName.impactAudioName, Variant.From(in impactAudioName));
		info.AddProperty(PropertyName.rollStateEvent, Variant.From(in rollStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From(in idleStateEvent));
		info.AddProperty(PropertyName.cameraShakeRange, Variant.From(in cameraShakeRange));
		info.AddProperty(PropertyName.cameraShakeStrength, Variant.From(in cameraShakeStrength));
		info.AddProperty(PropertyName.cameraShakeDuration, Variant.From(in cameraShakeDuration));
		info.AddProperty(PropertyName.cameraShakeFrequency, Variant.From(in cameraShakeFrequency));
		info.AddProperty(PropertyName.checkAreaPath, Variant.From(in checkAreaPath));
		info.AddProperty(PropertyName.spritePath, Variant.From(in spritePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.rollXVelocityMax, out var value))
		{
			rollXVelocityMax = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rollXVelocityMin, out var value2))
		{
			rollXVelocityMin = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rollYVelocity, out var value3))
		{
			rollYVelocity = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.selfRotateUse, out var value4))
		{
			selfRotateUse = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.selfRotationDivisor, out var value5))
		{
			selfRotationDivisor = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.useParentHitBox, out var value6))
		{
			useParentHitBox = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hitEvent, out var value7))
		{
			hitEvent = value7.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.maxHitNum, out var value8))
		{
			maxHitNum = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hitLineUse, out var value9))
		{
			hitLineUse = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hitLineBackUse, out var value10))
		{
			hitLineBackUse = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.edgeReboundUse, out var value11))
		{
			edgeReboundUse = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.offscreenDestroyMargin, out var value12))
		{
			offscreenDestroyMargin = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.topBoundaryOffset, out var value13))
		{
			topBoundaryOffset = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.bottomBoundaryOffset, out var value14))
		{
			bottomBoundaryOffset = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.collisionCheckIntervalPhysicsFrames, out var value15))
		{
			collisionCheckIntervalPhysicsFrames = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fallCoinUse, out var value16))
		{
			fallCoinUse = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hitCoinRewards, out var value17))
		{
			hitCoinRewards = value17.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.rollAnimeClips, out var value18))
		{
			rollAnimeClips = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.rollAnimeTimeScale, out var value19))
		{
			rollAnimeTimeScale = value19.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dieAnimeClip, out var value20))
		{
			dieAnimeClip = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.rollAnimationStartPosition, out var value21))
		{
			rollAnimationStartPosition = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rollAudioName, out var value22))
		{
			rollAudioName = value22.As<string>();
		}
		if (info.TryGetProperty(PropertyName.impactAudioName, out var value23))
		{
			impactAudioName = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.rollStateEvent, out var value24))
		{
			rollStateEvent = value24.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value25))
		{
			idleStateEvent = value25.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeRange, out var value26))
		{
			cameraShakeRange = value26.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeStrength, out var value27))
		{
			cameraShakeStrength = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeDuration, out var value28))
		{
			cameraShakeDuration = value28.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeFrequency, out var value29))
		{
			cameraShakeFrequency = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName.checkAreaPath, out var value30))
		{
			checkAreaPath = value30.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value31))
		{
			spritePath = value31.As<NodePath>();
		}
	}
}
