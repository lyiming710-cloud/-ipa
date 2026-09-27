using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseCharacter.cs")]
public class TowerDefenseCharacter : TowerDefenseGroundItemBase, IAabbCollisionPreview2D
{
	private enum MainRuntimeState : byte
	{
		Idle,
		Sleep,
		Component
	}

	private sealed class MainStatePhysicsFastCallbackTarget : IStateMachinePhysicsFastCallbackTarget
	{
		private readonly TowerDefenseCharacter _owner;

		internal MainStatePhysicsFastCallbackTarget(TowerDefenseCharacter owner)
		{
			_owner = owner;
		}

		void IStateMachinePhysicsFastCallbackTarget.InvokeStateMachinePhysicsFastCallback(int callbackId, double delta)
		{
			switch (callbackId)
			{
			case 1:
				_owner.IdleProcessing(delta);
				break;
			case 2:
				_owner.SleepProcessing(delta);
				break;
			}
		}
	}

	[Flags]
	public enum HitBoxSuppressionReason : uint
	{
		None = 0u,
		Manual = 1u,
		Paused = 2u,
		Destroyed = 4u,
		Carried = 8u,
		Tanglekelp = 0x10u,
		Scripted = 0x20u,
		PlanternTanglekelp = 0x40u
	}

	public delegate void DestroyEventHandler(TowerDefenseCharacter character);

	public delegate void CharacterNearDieEventHandler(TowerDefenseCharacter character);

	public delegate void BodyHurtEventHandler(int num);

	public delegate void ArmorHurtEventHandler(int num);

	public delegate void RiseOverEventHandler();

	public delegate void ComponentChangeEventHandler();

	public delegate void DamageBlockedEventHandler();

	private sealed class TransformWatcher : Node
	{
		public new class MethodName : Node.MethodName
		{
			public new static readonly StringName _Process = "_Process";
		}

		public new class PropertyName : Node.PropertyName
		{
			public static readonly StringName _source = "_source";

			public static readonly StringName _target = "_target";

			public static readonly StringName _bodyHp = "_bodyHp";

			public static readonly StringName _syncBodyHp = "_syncBodyHp";

			public static readonly StringName _removeSource = "_removeSource";

			public static readonly StringName _readyInvoked = "_readyInvoked";

			public static readonly StringName _waitFrames = "_waitFrames";

			public static readonly StringName _elapsedFrames = "_elapsedFrames";
		}

		public new class SignalName : Node.SignalName
		{
		}

		private readonly TowerDefenseCharacter _source;

		private readonly TowerDefenseCharacter _target;

		private readonly double _bodyHp;

		private readonly bool _syncBodyHp;

		private readonly bool _removeSource;

		private readonly Action<TowerDefenseCharacter> _onTargetReady;

		private bool _readyInvoked;

		private int _waitFrames;

		private int _elapsedFrames;

		public TransformWatcher(TowerDefenseCharacter source, TowerDefenseCharacter target, double bodyHp, bool syncBodyHp, bool removeSource, Action<TowerDefenseCharacter> onTargetReady)
		{
			_source = source;
			_target = target;
			_bodyHp = bodyHp;
			_syncBodyHp = syncBodyHp;
			_removeSource = removeSource;
			_onTargetReady = onTargetReady;
		}

		public override void _Process(double delta)
		{
			_elapsedFrames++;
			if (_elapsedFrames > 120)
			{
				QueueFree();
				return;
			}
			if (!_readyInvoked)
			{
				if (!GodotObject.IsInstanceValid(_target))
				{
					QueueFree();
					return;
				}
				if (!_target.IsInsideTree())
				{
					return;
				}
				_readyInvoked = true;
				if (_syncBodyHp && GodotObject.IsInstanceValid(_target.instance))
				{
					_target.instance.hitpointsSave = _bodyHp;
					_target.instance.hitpoints = _bodyHp;
				}
				_onTargetReady?.Invoke(_target);
			}
			if (_waitFrames < 2)
			{
				_waitFrames++;
				return;
			}
			QueueFree();
			if (_removeSource)
			{
				_source.SilentlyRemove();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if (method == MethodName._Process && args.Count == 1)
			{
				_Process(VariantUtils.ConvertTo<double>(in args[0]));
				ret = default;
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName._Process)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName._readyInvoked)
			{
				_readyInvoked = VariantUtils.ConvertTo<bool>(in value);
				return true;
			}
			if (name == PropertyName._waitFrames)
			{
				_waitFrames = VariantUtils.ConvertTo<int>(in value);
				return true;
			}
			if (name == PropertyName._elapsedFrames)
			{
				_elapsedFrames = VariantUtils.ConvertTo<int>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if (name == PropertyName._source)
			{
				value = VariantUtils.CreateFrom(in _source);
				return true;
			}
			if (name == PropertyName._target)
			{
				value = VariantUtils.CreateFrom(in _target);
				return true;
			}
			if (name == PropertyName._bodyHp)
			{
				value = VariantUtils.CreateFrom(in _bodyHp);
				return true;
			}
			if (name == PropertyName._syncBodyHp)
			{
				value = VariantUtils.CreateFrom(in _syncBodyHp);
				return true;
			}
			if (name == PropertyName._removeSource)
			{
				value = VariantUtils.CreateFrom(in _removeSource);
				return true;
			}
			if (name == PropertyName._readyInvoked)
			{
				value = VariantUtils.CreateFrom(in _readyInvoked);
				return true;
			}
			if (name == PropertyName._waitFrames)
			{
				value = VariantUtils.CreateFrom(in _waitFrames);
				return true;
			}
			if (name == PropertyName._elapsedFrames)
			{
				value = VariantUtils.CreateFrom(in _elapsedFrames);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, PropertyName._source, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Object, PropertyName._target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Float, PropertyName._bodyHp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Bool, PropertyName._syncBodyHp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Bool, PropertyName._removeSource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Bool, PropertyName._readyInvoked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Int, PropertyName._waitFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Int, PropertyName._elapsedFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName._readyInvoked, Variant.From(in _readyInvoked));
			info.AddProperty(PropertyName._waitFrames, Variant.From(in _waitFrames));
			info.AddProperty(PropertyName._elapsedFrames, Variant.From(in _elapsedFrames));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName._readyInvoked, out var value))
			{
				_readyInvoked = value.As<bool>();
			}
			if (info.TryGetProperty(PropertyName._waitFrames, out var value2))
			{
				_waitFrames = value2.As<int>();
			}
			if (info.TryGetProperty(PropertyName._elapsedFrames, out var value3))
			{
				_elapsedFrames = value3.As<int>();
			}
		}
	}

	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public static readonly StringName EmitDestroy = "EmitDestroy";

		public static readonly StringName EmitBodyHurt = "EmitBodyHurt";

		public static readonly StringName EmitArmorHurt = "EmitArmorHurt";

		public static readonly StringName EmitRiseOver = "EmitRiseOver";

		public static readonly StringName EmitComponentChange = "EmitComponentChange";

		public static readonly StringName EmitDamageBlocked = "EmitDamageBlocked";

		public static readonly StringName CanReceiveExplosionHit = "CanReceiveExplosionHit";

		public static readonly StringName SetHitBoxEnabled = "SetHitBoxEnabled";

		public static readonly StringName SetHitBoxMonitorable = "SetHitBoxMonitorable";

		public static readonly StringName SetHitBoxSuppressed = "SetHitBoxSuppressed";

		public static readonly StringName SetHitBoxMonitorSuppressed = "SetHitBoxMonitorSuppressed";

		public static readonly StringName DestroyHitBoxRuntime = "DestroyHitBoxRuntime";

		public static readonly StringName InvalidateHitBoxBounds = "InvalidateHitBoxBounds";

		public new static readonly StringName OnGridPositionChanged = "OnGridPositionChanged";

		public new static readonly StringName OnGroundHeightChanged = "OnGroundHeightChanged";

		public static readonly StringName GetCachedWorldPositionForProjectile = "GetCachedWorldPositionForProjectile";

		public static readonly StringName RefreshLocalScaleXSnapshot = "RefreshLocalScaleXSnapshot";

		public static readonly StringName NotifyRenderAncestorScaleChangedIfNeeded = "NotifyRenderAncestorScaleChangedIfNeeded";

		public static readonly StringName EnsureHitBoxRuntimeInitialized = "EnsureHitBoxRuntimeInitialized";

		public static readonly StringName ResetHitBoxRuntimeFromDefinition = "ResetHitBoxRuntimeFromDefinition";

		public static readonly StringName RefreshExplosionHitEligibility = "RefreshExplosionHitEligibility";

		public static readonly StringName SetCollisionPreviewDraw = "SetCollisionPreviewDraw";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawPrimaryHitBoxPreview = "DrawPrimaryHitBoxPreview";

		public static readonly StringName DrawResourceComponentCollisionPreviews = "DrawResourceComponentCollisionPreviews";

		public static readonly StringName SendStateEvent = "SendStateEvent";

		public static readonly StringName SetMainRuntimeState = "SetMainRuntimeState";

		public static readonly StringName NormalizeMainStateEvent = "NormalizeMainStateEvent";

		public static readonly StringName PrepareBatchRegistration = "PrepareBatchRegistration";

		public static readonly StringName CancelCellMoveTween = "CancelCellMoveTween";

		public static readonly StringName TrackCellMoveTween = "TrackCellMoveTween";

		public static readonly StringName EnableComponentGameplayUntilBattlefieldEntry = "EnableComponentGameplayUntilBattlefieldEntry";

		public static readonly StringName IsWithinComponentBattlefieldBoundsForPhysicsFrame = "IsWithinComponentBattlefieldBoundsForPhysicsFrame";

		public static readonly StringName GetGlobalPositionForPhysicsFrame = "GetGlobalPositionForPhysicsFrame";

		public static readonly StringName SetGlobalPositionForPhysicsFrame = "SetGlobalPositionForPhysicsFrame";

		public static readonly StringName SetGlobalPositionForPhysicsFrameNode = "SetGlobalPositionForPhysicsFrameNode";

		public static readonly StringName TranslateForPhysicsFrame = "TranslateForPhysicsFrame";

		public static readonly StringName PublishCharacterTranslationForRender = "PublishCharacterTranslationForRender";

		public static readonly StringName GetGlobalTransformForShadow = "GetGlobalTransformForShadow";

		public static readonly StringName GetGlobalTransformForPhysicsFrame = "GetGlobalTransformForPhysicsFrame";

		public static readonly StringName GetLogicalGlobalPosition = "GetLogicalGlobalPosition";

		public static readonly StringName GetLogicalGlobalTransform = "GetLogicalGlobalTransform";

		public static readonly StringName SetLogicalGlobalPosition = "SetLogicalGlobalPosition";

		public static readonly StringName SetCellMoveLogicalGlobalPosition = "SetCellMoveLogicalGlobalPosition";

		public static readonly StringName RefreshCellMoveRenderOrder = "RefreshCellMoveRenderOrder";

		public static readonly StringName DisableGameplayForPermanentEmbeddedVisual = "DisableGameplayForPermanentEmbeddedVisual";

		public static readonly StringName InvalidateGlobalPositionForPhysicsFrame = "InvalidateGlobalPositionForPhysicsFrame";

		public static readonly StringName InvalidateComponentGameplayForPhysicsFrame = "InvalidateComponentGameplayForPhysicsFrame";

		public static readonly StringName GetComponentGameplayForCurrentPhysicsFrame = "GetComponentGameplayForCurrentPhysicsFrame";

		public static readonly StringName RefreshComponentGameplayEntryExceptionForPhysicsFrame = "RefreshComponentGameplayEntryExceptionForPhysicsFrame";

		public static readonly StringName ResolveComponentGameplayForPhysicsFrame = "ResolveComponentGameplayForPhysicsFrame";

		public static readonly StringName CacheComponentGameplayForPhysicsFrame = "CacheComponentGameplayForPhysicsFrame";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName EnsureComponentManagerResource = "EnsureComponentManagerResource";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshPacketCustomFromSave = "RefreshPacketCustomFromSave";

		public static readonly StringName BindMainStateHandles = "BindMainStateHandles";

		public static readonly StringName UnbindMainStateHandles = "UnbindMainStateHandles";

		public static readonly StringName DisableInvalidRuntimeCharacter = "DisableInvalidRuntimeCharacter";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EnsureCharacterSkinSwitchedHandler = "EnsureCharacterSkinSwitchedHandler";

		public static readonly StringName SubscribeCharacterSkinSwitched = "SubscribeCharacterSkinSwitched";

		public static readonly StringName UnsubscribeCharacterSkinSwitched = "UnsubscribeCharacterSkinSwitched";

		public new static readonly StringName _Notification = "_Notification";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public static readonly StringName ReleaseMainStateMachine = "ReleaseMainStateMachine";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName BatchProcessUpdate = "BatchProcessUpdate";

		public static readonly StringName TickMainStateMachineProcess = "TickMainStateMachineProcess";

		public static readonly StringName TickComponentStateMachineProcess = "TickComponentStateMachineProcess";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName PhysicsProcessWithFrame = "PhysicsProcessWithFrame";

		public static readonly StringName RunComponentPhysicsForTrace = "RunComponentPhysicsForTrace";

		public static readonly StringName RunCharacterPhysicsPostForTrace = "RunCharacterPhysicsPostForTrace";

		public static readonly StringName PrepareCharacterTimeScaleForPhysics = "PrepareCharacterTimeScaleForPhysics";

		public static readonly StringName SynchronizeAnimationPlaybackBlock = "SynchronizeAnimationPlaybackBlock";

		public static readonly StringName DamagePartInit = "DamagePartInit";

		public static readonly StringName PreviewDamagePoint = "PreviewDamagePoint";

		public static readonly StringName ClearArmor = "ClearArmor";

		public static readonly StringName ClearArmorAll = "ClearArmorAll";

		public static readonly StringName SetArmor = "SetArmor";

		public static readonly StringName SetArmors = "SetArmors";

		public static readonly StringName ClearCustom = "ClearCustom";

		public static readonly StringName SetCustom = "SetCustom";

		public static readonly StringName SetCustoms = "SetCustoms";

		public static readonly StringName OnCharacterSkinSwitched = "OnCharacterSkinSwitched";

		public static readonly StringName SwitchCustom = "SwitchCustom";

		public static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName IdleExited = "IdleExited";

		public static readonly StringName SleepEntered = "SleepEntered";

		public static readonly StringName SleepProcessing = "SleepProcessing";

		public static readonly StringName SleepExited = "SleepExited";

		public static readonly StringName ComponentEntered = "ComponentEntered";

		public static readonly StringName ComponentExited = "ComponentExited";

		public static readonly StringName Idle = "Idle";

		public static readonly StringName Sleep = "Sleep";

		public static readonly StringName Component = "Component";

		public static readonly StringName IsDie = "IsDie";

		public static readonly StringName CanSleep = "CanSleep";

		public static readonly StringName IsSleep = "IsSleep";

		public static readonly StringName IsIncapacitatedTarget = "IsIncapacitatedTarget";

		public static readonly StringName GetTotalHitPoint = "GetTotalHitPoint";

		public static readonly StringName GetCurrentHitPoint = "GetCurrentHitPoint";

		public static readonly StringName SetHitpointAndScale = "SetHitpointAndScale";

		public static readonly StringName MagnetCreate = "MagnetCreate";

		public static readonly StringName ArmorDraw = "ArmorDraw";

		public static readonly StringName HasShield = "HasShield";

		public static readonly StringName HasHelm = "HasHelm";

		public static readonly StringName ProjectileEffectsBlocked = "ProjectileEffectsBlocked";

		public static readonly StringName GetHasArmor = "GetHasArmor";

		public static readonly StringName GetArmorFromName = "GetArmorFromName";

		public static readonly StringName GetArmor = "GetArmor";

		public static readonly StringName GetArmorShield = "GetArmorShield";

		public static readonly StringName GetArmorHelment = "GetArmorHelment";

		public static readonly StringName GetArmorHeadCover = "GetArmorHeadCover";

		public static readonly StringName CanCollision = "CanCollision";

		public static readonly StringName CanTarget = "CanTarget";

		public static readonly StringName CheckDifferentCamp = "CheckDifferentCamp";

		public static readonly StringName CheckSameLine = "CheckSameLine";

		public static readonly StringName IsTargetableFromLine = "IsTargetableFromLine";

		public static readonly StringName IsTargetableFromLineRange = "IsTargetableFromLineRange";

		public static readonly StringName GetGroundHeight = "GetGroundHeight";

		public static readonly StringName SetSpriteGroupShaderParameter = "SetSpriteGroupShaderParameter";

		public new static readonly StringName SetZ = "SetZ";

		public static readonly StringName ApplySpriteGroupZPosition = "ApplySpriteGroupZPosition";

		public static readonly StringName ReleaseZMotionLocalRenderWhenStable = "ReleaseZMotionLocalRenderWhenStable";

		public static readonly StringName ShovelDestroy = "ShovelDestroy";

		public static readonly StringName Destroy = "Destroy";

		public static readonly StringName ClearFromMap = "ClearFromMap";

		public static readonly StringName DestroyWithVisualDelay = "DestroyWithVisualDelay";

		public static readonly StringName AshDestroy = "AshDestroy";

		public static readonly StringName SmashDestroy = "SmashDestroy";

		public static readonly StringName DestroyReplace = "DestroyReplace";

		public static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName HitBoxDestroy = "HitBoxDestroy";

		public static readonly StringName HurtWithAttackConfig = "HurtWithAttackConfig";

		public static readonly StringName Hurt = "Hurt";

		public static readonly StringName SkipInvincibleHurt = "SkipInvincibleHurt";

		public static readonly StringName Health = "Health";

		public static readonly StringName RestoreFullHealthAfterRevive = "RestoreFullHealthAfterRevive";

		public static readonly StringName BowlingHurt = "BowlingHurt";

		public static readonly StringName SmashHurt = "SmashHurt";

		public static readonly StringName ExplodeHurt = "ExplodeHurt";

		public static readonly StringName FlagHurt = "FlagHurt";

		public static readonly StringName ProjectileHurt = "ProjectileHurt";

		public static readonly StringName Bright = "Bright";

		public static readonly StringName White = "White";

		public static readonly StringName YBCreate = "YBCreate";

		public static readonly StringName CoinCreate = "CoinCreate";

		public static readonly StringName LuckyBagCreate = "LuckyBagCreate";

		public static readonly StringName SunCreate = "SunCreate";

		public static readonly StringName BrainSunCreate = "BrainSunCreate";

		public static readonly StringName ReplicatedEventSunCreate = "ReplicatedEventSunCreate";

		public static readonly StringName JalapenoSunCreate = "JalapenoSunCreate";

		public static readonly StringName QXSunCreate = "QXSunCreate";

		public static readonly StringName MagicSunCreate = "MagicSunCreate";

		public static readonly StringName ExplodeSunCreate = "ExplodeSunCreate";

		public static readonly StringName GoldShardCreate = "GoldShardCreate";

		public static readonly StringName CraterCreate = "CraterCreate";

		public static readonly StringName BlowBack = "BlowBack";

		public static readonly StringName ApplyBowlingImpactDisplacement = "ApplyBowlingImpactDisplacement";

		public static readonly StringName UpdateBowlingImpactDisplacement = "UpdateBowlingImpactDisplacement";

		public static readonly StringName CancelBowlingImpactDisplacement = "CancelBowlingImpactDisplacement";

		public static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public static readonly StringName SyncHologramHypnoses = "SyncHologramHypnoses";

		public static readonly StringName Hypnoses = "Hypnoses";

		public static readonly StringName Rise = "Rise";

		public static readonly StringName Recycle = "Recycle";

		public static readonly StringName RefreshResourceComponentFacades = "RefreshResourceComponentFacades";

		public static readonly StringName CreateDirt = "CreateDirt";

		public static readonly StringName CreateSplash = "CreateSplash";

		public static readonly StringName CreateIceTrap = "CreateIceTrap";

		public static readonly StringName WakeUp = "WakeUp";

		public static readonly StringName OnWakeUp = "OnWakeUp";

		public static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName DamagePointReach = "DamagePointReach";

		public static readonly StringName ArmorDamagePointReach = "ArmorDamagePointReach";

		public static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public static readonly StringName AttackDeal = "AttackDeal";

		public static readonly StringName UnlimitedFireInit = "UnlimitedFireInit";

		public static readonly StringName InheritCoverSleepState = "InheritCoverSleepState";

		public static readonly StringName SupportsCoverSleepInheritance = "SupportsCoverSleepInheritance";

		public static readonly StringName Cover = "Cover";

		public static readonly StringName Spawn = "Spawn";

		public static readonly StringName PreSpawn = "PreSpawn";

		public static readonly StringName ShouldEnableGameplayDispatch = "ShouldEnableGameplayDispatch";

		public static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public static readonly StringName ActivateLevelEntryPreview = "ActivateLevelEntryPreview";

		public static readonly StringName SetMainStateMachineDispatchEnabled = "SetMainStateMachineDispatchEnabled";

		public static readonly StringName RefreshComponentStateMachineDispatch = "RefreshComponentStateMachineDispatch";

		public static readonly StringName SetOwnerBatchRegistration = "SetOwnerBatchRegistration";

		public static readonly StringName UpdateDirectMainStateMachineProcess = "UpdateDirectMainStateMachineProcess";

		public static readonly StringName ResumeOwnerBatchDispatch = "ResumeOwnerBatchDispatch";

		public static readonly StringName SuspendOwnerBatchDispatch = "SuspendOwnerBatchDispatch";

		public static readonly StringName ConfigureCharacterBatchProcessing = "ConfigureCharacterBatchProcessing";

		public static readonly StringName EnterInitialMainStateMachine = "EnterInitialMainStateMachine";

		public static readonly StringName CaptureMainStateMachineSnapshotData = "CaptureMainStateMachineSnapshotData";

		public static readonly StringName RestoreMainStateMachineSnapshotData = "RestoreMainStateMachineSnapshotData";

		public static readonly StringName RestoreLegacyMainState = "RestoreLegacyMainState";

		public static readonly StringName BeginAuthoritativeMainStateRestore = "BeginAuthoritativeMainStateRestore";

		public static readonly StringName EndAuthoritativeMainStateRestore = "EndAuthoritativeMainStateRestore";

		public static readonly StringName ApplyPendingMainStateMachineSnapshot = "ApplyPendingMainStateMachineSnapshot";

		public static readonly StringName PrepareMainStateMachineSnapshot = "PrepareMainStateMachineSnapshot";

		public static readonly StringName IsProgressStateMachineDefinitionIdCompatible = "IsProgressStateMachineDefinitionIdCompatible";

		public static readonly StringName ApplyMainStateMachineSnapshot = "ApplyMainStateMachineSnapshot";

		public static readonly StringName OnAuthoritativeMainStateRestored = "OnAuthoritativeMainStateRestored";

		public static readonly StringName ActivateAdobeAnimateSpriteTree = "ActivateAdobeAnimateSpriteTree";

		public static readonly StringName ShouldKeepAnimationPaused = "ShouldKeepAnimationPaused";

		public static readonly StringName Blow = "Blow";

		public static readonly StringName Garlic = "Garlic";

		public static readonly StringName CanBlock = "CanBlock";

		public static readonly StringName ShouldUpdateGridPos = "ShouldUpdateGridPos";

		public static readonly StringName OnRiseStart = "OnRiseStart";

		public static readonly StringName OnRiseEnd = "OnRiseEnd";

		public static readonly StringName BlockType = "BlockType";

		public static readonly StringName Block = "Block";

		public static readonly StringName CanDiggerBlock = "CanDiggerBlock";

		public static readonly StringName BlockDigger = "BlockDigger";

		public static readonly StringName Purify = "Purify";

		public static readonly StringName SpawnZombie = "SpawnZombie";

		public static readonly StringName OnAnimeStarted = "OnAnimeStarted";

		public static readonly StringName SyncAnimation = "SyncAnimation";

		public static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public static readonly StringName InWater = "InWater";

		public static readonly StringName OutWater = "OutWater";

		public static readonly StringName GetFireAnime = "GetFireAnime";

		public static readonly StringName CreateFireEventLists = "CreateFireEventLists";

		public static readonly StringName CreateFireEffectAtCell = "CreateFireEffectAtCell";

		public static readonly StringName IsJalapenoBattleContextCurrent = "IsJalapenoBattleContextCurrent";

		public static readonly StringName PlayFireExplodeEffects = "PlayFireExplodeEffects";

		public static readonly StringName CreateSnowEventList = "CreateSnowEventList";

		public static readonly StringName CreateColdVisualEffect = "CreateColdVisualEffect";

		public static readonly StringName CreateColdEffect = "CreateColdEffect";

		public static readonly StringName CreateColdEffectRange = "CreateColdEffectRange";

		public static readonly StringName CreateCharacter = "CreateCharacter";

		public static readonly StringName TryConsumeIncomingBuff = "TryConsumeIncomingBuff";

		public static readonly StringName BuffAdd = "BuffAdd";

		public static readonly StringName BuffDelete = "BuffDelete";

		public static readonly StringName BuffGet = "BuffGet";

		public static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public static readonly StringName OnRemoteNetworkAnimationStart = "OnRemoteNetworkAnimationStart";

		public static readonly StringName RestoreFromSave = "RestoreFromSave";

		public static readonly StringName ExportVariantSave = "ExportVariantSave";

		public static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public static readonly StringName PrepareForProgressRestore = "PrepareForProgressRestore";

		public static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public static readonly StringName SilentlyRemove = "SilentlyRemove";

		public static readonly StringName SilentlyRemoveForTransformation = "SilentlyRemoveForTransformation";

		public static readonly StringName SpawnTransformEffect = "SpawnTransformEffect";

		public static readonly StringName InitializeCharacterExternalVisuals = "InitializeCharacterExternalVisuals";

		public static readonly StringName DisposeCharacterExternalVisuals = "DisposeCharacterExternalVisuals";

		public static readonly StringName AttachAnimatedStatusVisual = "AttachAnimatedStatusVisual";

		public static readonly StringName DetachAnimatedStatusVisual = "DetachAnimatedStatusVisual";

		public static readonly StringName RegisterLegacySpriteGroupVisualsOnce = "RegisterLegacySpriteGroupVisualsOnce";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName IsRemoteNetworkReplica = "IsRemoteNetworkReplica";

		public static readonly StringName HasValidRuntimeConfiguration = "HasValidRuntimeConfiguration";

		public static readonly StringName HasArmorHurtSubscribers = "HasArmorHurtSubscribers";

		public static readonly StringName icetrapSprite = "icetrapSprite";

		public static readonly StringName HitBoxDefinition = "HitBoxDefinition";

		public static readonly StringName HitBoxBoundsRevision = "HitBoxBoundsRevision";

		public static readonly StringName HasHitBox = "HasHitBox";

		public static readonly StringName IsHitBoxEnabled = "IsHitBoxEnabled";

		public static readonly StringName IsHitBoxMonitorable = "IsHitBoxMonitorable";

		public static readonly StringName WorldBroadphaseRect = "WorldBroadphaseRect";

		public static readonly StringName CachedLocalScaleX = "CachedLocalScaleX";

		public static readonly StringName WorldHitRect = "WorldHitRect";

		public static readonly StringName MainStateMachineDefinition = "MainStateMachineDefinition";

		public static readonly StringName WantsMainStateMachineProcessDispatch = "WantsMainStateMachineProcessDispatch";

		public static readonly StringName IsBowlingImpactDisplacementActive = "IsBowlingImpactDisplacementActive";

		public static readonly StringName IsOwnerBatchRegistered = "IsOwnerBatchRegistered";

		public static readonly StringName BatchUsesInheritedProcessMode = "BatchUsesInheritedProcessMode";

		public static readonly StringName BatchProcessModeParent = "BatchProcessModeParent";

		public static readonly StringName IsOwnerBatchDispatchActive = "IsOwnerBatchDispatchActive";

		public static readonly StringName CanDispatchMainStateMachine = "CanDispatchMainStateMachine";

		public static readonly StringName CanDispatchOwnedStateMachines = "CanDispatchOwnedStateMachines";

		public static readonly StringName ComponentSet = "ComponentSet";

		public static readonly StringName invisible = "invisible";

		public static readonly StringName config = "config";

		public static readonly StringName sprite = "sprite";

		public static readonly StringName headSlot = "headSlot";

		public static readonly StringName HasEconomyOwner = "HasEconomyOwner";

		public static readonly StringName camp = "camp";

		public static readonly StringName damagePartClip = "damagePartClip";

		public static readonly StringName damagePart = "damagePart";

		public static readonly StringName damagePartSlot = "damagePartSlot";

		public static readonly StringName previewDamagePointPersontage = "previewDamagePointPersontage";

		public static readonly StringName currentArmor = "currentArmor";

		public static readonly StringName currentCustom = "currentCustom";

		public static readonly StringName componentAlive = "componentAlive";

		public static readonly StringName inWater = "inWater";

		public static readonly StringName HasComponentGameplayUntilBattlefieldEntry = "HasComponentGameplayUntilBattlefieldEntry";

		public static readonly StringName IsInsideComponentBattlefield = "IsInsideComponentBattlefield";

		public static readonly StringName IsInsideComponentBattlefieldForRuntime = "IsInsideComponentBattlefieldForRuntime";

		public static readonly StringName PreserveDeathTransformation = "PreserveDeathTransformation";

		public static readonly StringName IsHardControlImmune = "IsHardControlImmune";

		public static readonly StringName IsProgressRestoreInFlight = "IsProgressRestoreInFlight";

		public static readonly StringName CharacterExternalVisualCompatibilityScanCount = "CharacterExternalVisualCompatibilityScanCount";

		public static readonly StringName CharacterAnimatedStatusVisualCount = "CharacterAnimatedStatusVisualCount";

		public static readonly StringName CharacterAnimatedStatusTopologyChangeCount = "CharacterAnimatedStatusTopologyChangeCount";

		public static readonly StringName syncId = "syncId";

		public static readonly StringName backEffectNode = "backEffectNode";

		public static readonly StringName frontEffectNode = "frontEffectNode";

		public static readonly StringName spriteGroup = "spriteGroup";

		public static readonly StringName transformPoint = "transformPoint";

		public static readonly StringName shadowSprite = "shadowSprite";

		public static readonly StringName _hitBoxDefinition = "_hitBoxDefinition";

		public static readonly StringName _hitBoxRuntimeInitialized = "_hitBoxRuntimeInitialized";

		public static readonly StringName _hitBoxAvailable = "_hitBoxAvailable";

		public static readonly StringName _hitBoxDefaultEnabled = "_hitBoxDefaultEnabled";

		public static readonly StringName _hitBoxDefaultMonitorable = "_hitBoxDefaultMonitorable";

		public static readonly StringName _hitBoxSuppression = "_hitBoxSuppression";

		public static readonly StringName _hitBoxMonitorSuppression = "_hitBoxMonitorSuppression";

		public static readonly StringName _canReceiveExplosionHit = "_canReceiveExplosionHit";

		public static readonly StringName _hitBoxPreviewDraw = "_hitBoxPreviewDraw";

		public static readonly StringName _worldHitRectCache = "_worldHitRectCache";

		public static readonly StringName _worldHitRectCacheFrame = "_worldHitRectCacheFrame";

		public static readonly StringName _worldHitRectCacheTransform = "_worldHitRectCacheTransform";

		public static readonly StringName _worldHitRectCacheScaleX = "_worldHitRectCacheScaleX";

		public static readonly StringName _localScaleXSnapshot = "_localScaleXSnapshot";

		public static readonly StringName _localScaleXSnapshotValid = "_localScaleXSnapshotValid";

		public static readonly StringName _worldHitRectCacheDefinition = "_worldHitRectCacheDefinition";

		public static readonly StringName _worldHitRectCacheValid = "_worldHitRectCacheValid";

		public static readonly StringName _hitBoxBoundsRevision = "_hitBoxBoundsRevision";

		public static readonly StringName _renderAncestorScaleSnapshot = "_renderAncestorScaleSnapshot";

		public static readonly StringName _renderAncestorScaleSnapshotValid = "_renderAncestorScaleSnapshotValid";

		public static readonly StringName _mainStateMachineDispatchEnabled = "_mainStateMachineDispatchEnabled";

		public static readonly StringName _ownerBatchRegistered = "_ownerBatchRegistered";

		public static readonly StringName _batchUsesInheritedProcessMode = "_batchUsesInheritedProcessMode";

		public static readonly StringName _batchProcessModeParent = "_batchProcessModeParent";

		public static readonly StringName _mainStateMachineReleased = "_mainStateMachineReleased";

		public static readonly StringName _authoritativeMainStateRestoreDepth = "_authoritativeMainStateRestoreDepth";

		public static readonly StringName _characterSkinSwitchedEventBus = "_characterSkinSwitchedEventBus";

		public static readonly StringName _characterSkinSwitchedSubscribed = "_characterSkinSwitchedSubscribed";

		public static readonly StringName _pendingMainStateMachineSnapshot = "_pendingMainStateMachineSnapshot";

		public static readonly StringName _pendingMainStateMachineSnapshotIsRemote = "_pendingMainStateMachineSnapshotIsRemote";

		public static readonly StringName _pendingMainStateMachineSnapshotSuppressEffects = "_pendingMainStateMachineSnapshotSuppressEffects";

		public static readonly StringName _pendingMainStateMachineSnapshotProgressCompatible = "_pendingMainStateMachineSnapshotProgressCompatible";

		public static readonly StringName _lastRemoteMainStateMachineRevision = "_lastRemoteMainStateMachineRevision";

		public static readonly StringName _lastMainStateMachineProcessFrame = "_lastMainStateMachineProcessFrame";

		public static readonly StringName _lastComponentStateMachineProcessFrame = "_lastComponentStateMachineProcessFrame";

		public static readonly StringName _lastMainStateMachinePhysicsFrame = "_lastMainStateMachinePhysicsFrame";

		public static readonly StringName _bowlingImpactDisplacementActive = "_bowlingImpactDisplacementActive";

		public static readonly StringName _bowlingImpactStartGlobalX = "_bowlingImpactStartGlobalX";

		public static readonly StringName _bowlingImpactTargetGlobalX = "_bowlingImpactTargetGlobalX";

		public static readonly StringName _bowlingImpactElapsed = "_bowlingImpactElapsed";

		public static readonly StringName _bowlingImpactDuration = "_bowlingImpactDuration";

		public static readonly StringName _characterStateHandlesConnected = "_characterStateHandlesConnected";

		public static readonly StringName showHealthOffset = "showHealthOffset";

		public static readonly StringName componentManager = "componentManager";

		public static readonly StringName _resourceComponentFacadesCurrent = "_resourceComponentFacadesCurrent";

		public static readonly StringName _invisible = "_invisible";

		public static readonly StringName idleAnimeClip = "idleAnimeClip";

		public static readonly StringName sleepAnimeClip = "sleepAnimeClip";

		public static readonly StringName _config = "_config";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName _headSlot = "_headSlot";

		public static readonly StringName _camp = "_camp";

		public static readonly StringName _damagePartClip = "_damagePartClip";

		public static readonly StringName _damagePart = "_damagePart";

		public static readonly StringName _damagePartSlot = "_damagePartSlot";

		public static readonly StringName _previewDamagePointPersontage = "_previewDamagePointPersontage";

		public static readonly StringName _currentArmor = "_currentArmor";

		public static readonly StringName _currentCustom = "_currentCustom";

		public static readonly StringName instance = "instance";

		public static readonly StringName characterDisabled = "characterDisabled";

		public static readonly StringName _componentAlive = "_componentAlive";

		public static readonly StringName componentRunning = "componentRunning";

		public static readonly StringName characterFilter = "characterFilter";

		public static readonly StringName timeScaleInit = "timeScaleInit";

		public static readonly StringName timeScale = "timeScale";

		public static readonly StringName timeScaleSave = "timeScaleSave";

		public static readonly StringName inGame = "inGame";

		public static readonly StringName editorPreviewMode = "editorPreviewMode";

		public static readonly StringName editorMapPreviewMode = "editorMapPreviewMode";

		public static readonly StringName forceLocalRenderDuringZMotion = "forceLocalRenderDuringZMotion";

		public static readonly StringName isShow = "isShow";

		public static readonly StringName packet = "packet";

		public static readonly StringName cost = "cost";

		public static readonly StringName nearDie = "nearDie";

		public static readonly StringName die = "die";

		public static readonly StringName canMowerMove = "canMowerMove";

		public static readonly StringName baseSpriteScale = "baseSpriteScale";

		public static readonly StringName isRise = "isRise";

		public static readonly StringName isShovel = "isShovel";

		public static readonly StringName isSmash = "isSmash";

		public static readonly StringName isExplode = "isExplode";

		public static readonly StringName isChomp = "isChomp";

		public static readonly StringName skipDestroySet = "skipDestroySet";

		public static readonly StringName suppressDeathrattles = "suppressDeathrattles";

		public static readonly StringName mowerDeathVisualOwned = "mowerDeathVisualOwned";

		public static readonly StringName _inWater = "_inWater";

		public static readonly StringName iceSpeedDown = "iceSpeedDown";

		public static readonly StringName emSpeedDown = "emSpeedDown";

		public static readonly StringName useIdleAnimeReset = "useIdleAnimeReset";

		public static readonly StringName _syncApplyingAnimation = "_syncApplyingAnimation";

		public static readonly StringName isUnlimitedFire = "isUnlimitedFire";

		public static readonly StringName randFreshIndex = "randFreshIndex";

		public static readonly StringName groundRight = "groundRight";

		public static readonly StringName _componentGameplayUntilBattlefieldEntry = "_componentGameplayUntilBattlefieldEntry";

		public static readonly StringName _componentGameplayPhysicsDispatchActive = "_componentGameplayPhysicsDispatchActive";

		public static readonly StringName _componentGameplayPhysicsSnapshot = "_componentGameplayPhysicsSnapshot";

		public static readonly StringName _componentGameplayPhysicsSnapshotFrame = "_componentGameplayPhysicsSnapshotFrame";

		public static readonly StringName _physicsGlobalPositionFrame = "_physicsGlobalPositionFrame";

		public static readonly StringName _physicsGlobalPosition = "_physicsGlobalPosition";

		public static readonly StringName _shadowGlobalTransform = "_shadowGlobalTransform";

		public static readonly StringName _shadowGlobalTransformValid = "_shadowGlobalTransformValid";

		public static readonly StringName _physicsLocalPosition = "_physicsLocalPosition";

		public static readonly StringName _physicsLocalPositionValid = "_physicsLocalPositionValid";

		public static readonly StringName _writingCachedGlobalPosition = "_writingCachedGlobalPosition";

		public static readonly StringName _pendingCachedGlobalTransform = "_pendingCachedGlobalTransform";

		public static readonly StringName _pendingCachedGlobalTransformNotification = "_pendingCachedGlobalTransformNotification";

		public static readonly StringName _cellMoveTween = "_cellMoveTween";

		public static readonly StringName _zMotionOwnsLocalRender = "_zMotionOwnsLocalRender";

		public static readonly StringName _zMotionStablePhysicsFrames = "_zMotionStablePhysicsFrames";

		public static readonly StringName _spriteGroupLocalX = "_spriteGroupLocalX";

		public static readonly StringName _spriteGroupLocalXCaptured = "_spriteGroupLocalXCaptured";

		public static readonly StringName isDestroy = "isDestroy";

		public static readonly StringName _progressRestorePresentationRefreshPending = "_progressRestorePresentationRefreshPending";

		public static readonly StringName _characterExternalVisualsInitialized = "_characterExternalVisualsInitialized";

		public static readonly StringName _legacySpriteGroupVisualsScanned = "_legacySpriteGroupVisualsScanned";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private const int IdlePhysicsCallbackId = 1;

	private const int SleepPhysicsCallbackId = 2;

	private static readonly StringName ToIdleStateEvent = new StringName("ToIdle");

	private static readonly StringName ToSleepStateEvent = new StringName("ToSleep");

	private static readonly StringName ToComponentStateEvent = new StringName("ToComponent");

	protected static readonly bool CachedEditorHint = Engine.IsEditorHint();

	public static bool UseCharacterBatch = true;

	public int syncId = -1;

	private static PackedScene _FIRE;

	private static PackedScene _ICE_FIRE;

	private static PackedScene _MEGA_FIRE;

	private static PackedScene _PURIFY_FIRE;

	private static PackedScene _WHITE_FIRE;

	private static PackedScene _GARLIC_FIRE;

	private static PackedScene _SNOW_FLAKES;

	protected const int FRAME_CHECK_INTERVAL = 5;

	private const double NEAR_DEATH_DAMAGE_DIVISOR = 3.0;

	public Node2D backEffectNode;

	public Node2D frontEffectNode;

	public Node2D spriteGroup;

	public Marker2D transformPoint;

	public TowerDefenseShadowVisual shadowSprite;

	private CharacterHitBoxDefinition _hitBoxDefinition;

	private bool _hitBoxRuntimeInitialized;

	private bool _hitBoxAvailable;

	private bool _hitBoxDefaultEnabled;

	private bool _hitBoxDefaultMonitorable;

	private HitBoxSuppressionReason _hitBoxSuppression;

	private HitBoxSuppressionReason _hitBoxMonitorSuppression;

	private bool _canReceiveExplosionHit;

	private bool _hitBoxPreviewDraw;

	private Rect2 _worldHitRectCache;

	private long _worldHitRectCacheFrame = -1L;

	private Transform2D _worldHitRectCacheTransform;

	private float _worldHitRectCacheScaleX;

	private float _localScaleXSnapshot = 1f;

	private bool _localScaleXSnapshotValid;

	private CharacterHitBoxDefinition _worldHitRectCacheDefinition;

	private bool _worldHitRectCacheValid;

	private ulong _hitBoxBoundsRevision = 1uL;

	private Vector2 _renderAncestorScaleSnapshot = Vector2.One;

	private bool _renderAncestorScaleSnapshotValid;

	private StateMachineController _mainStateMachineController;

	private bool _mainStateMachineDispatchEnabled;

	private bool _ownerBatchRegistered;

	private bool _batchUsesInheritedProcessMode = true;

	private Node _batchProcessModeParent;

	private bool _mainStateMachineReleased;

	private int _authoritativeMainStateRestoreDepth;

	private BattleEventBus.CharacterSkinSwitchedEventHandler _characterSkinSwitchedHandler;

	private BattleEventBus _characterSkinSwitchedEventBus;

	private bool _characterSkinSwitchedSubscribed;

	private StateMachineSnapshot _pendingMainStateMachineSnapshot;

	private bool _pendingMainStateMachineSnapshotIsRemote;

	private bool _pendingMainStateMachineSnapshotSuppressEffects;

	private bool _pendingMainStateMachineSnapshotProgressCompatible;

	private long _lastRemoteMainStateMachineRevision = -1L;

	private ulong _lastMainStateMachineProcessFrame = 18446744073709551615uL;

	private ulong _lastComponentStateMachineProcessFrame = 18446744073709551615uL;

	private ulong _lastMainStateMachinePhysicsFrame = 18446744073709551615uL;

	private bool _bowlingImpactDisplacementActive;

	private float _bowlingImpactStartGlobalX;

	private float _bowlingImpactTargetGlobalX;

	private double _bowlingImpactElapsed;

	private double _bowlingImpactDuration;

	private StateHandle _idleStateHandle;

	private StateHandle _sleepStateHandle;

	private StateHandle _componentStateHandle;

	private MainStatePhysicsFastCallbackTarget _mainStatePhysicsFastCallbackTarget;

	private bool _characterStateHandlesConnected;

	public ShowHealthComponent showHealthComponent;

	[Export(PropertyHint.None, "")]
	public Vector2 showHealthOffset = Vector2.Zero;

	public ComponentManager componentManager;

	public BuffComponent buff;

	public ShadowComponent shadowComponent;

	public TargetRegistrationComponent targetRegistrationComponent;

	public RiseComponent riseComponent;

	public DestroyComponent destroyComponent;

	public HitFlashComponent hitFlashComponent;

	public ShaderEffectComponent shaderEffectComponent;

	public BlowBackComponent blowBackComponent;

	public SleepComponent sleepComponent;

	public ResourceSpawnComponent resourceSpawnComponent;

	public GroundHeightComponent groundHeightComponent;

	public DamagePartComponent damagePartComponent;

	public HurtComponent hurtComponent;

	public HypnosesComponent hypnosesComponent;

	public ArmorVisualComponent armorVisualComponent;

	public CustomVisualComponent customVisualComponent;

	public RecycleComponent recycleComponent;

	public EffectCreateComponent effectCreateComponent;

	public PhonkComponent phonkComponent;

	public DismemberComponent dismemberComponent;

	private bool _resourceComponentFacadesCurrent;

	private bool _invisible;

	[Export(PropertyHint.None, "")]
	public string idleAnimeClip = "Idle";

	[Export(PropertyHint.None, "")]
	public string sleepAnimeClip = "Idle";

	private TowerDefenseCharacterConfig _config;

	private AdobeAnimateSprite _sprite;

	private AdobeAnimateSlot _headSlot;

	private TowerDefenseEnum.CHARACTER_CAMP _camp;

	private EconomyAccountId _economyOwnerAccountId;

	private string _damagePartClip = "particles";

	private Dictionary _damagePart;

	private List<string> _damagePartList;

	private Dictionary _damagePartSlot;

	private double _previewDamagePointPersontage = 1.0;

	private Array<string> _currentArmor;

	private Array<string> _currentCustom;

	public TowerDefenseCharacterInstance instance;

	public bool characterDisabled;

	private bool _componentAlive = true;

	public bool componentRunning;

	public bool characterFilter;

	public double timeScaleInit = 1.0;

	public double timeScale = 1.0;

	public double timeScaleSave = 1.0;

	private List<TowerDefenseCharacterEventBase> _dieEvent;

	public bool inGame = true;

	public bool editorPreviewMode;

	public bool editorMapPreviewMode;

	[Export(PropertyHint.None, "")]
	public bool forceLocalRenderDuringZMotion;

	public bool isShow;

	public TowerDefensePacketConfig packet;

	public double cost;

	public bool nearDie;

	public bool die;

	public bool canMowerMove;

	public Vector2 baseSpriteScale;

	public bool isRise;

	public bool isShovel;

	public bool isSmash;

	public bool isExplode;

	public bool isChomp;

	public bool skipDestroySet;

	internal bool suppressDeathrattles;

	public bool mowerDeathVisualOwned;

	private bool _inWater;

	public bool iceSpeedDown;

	public bool emSpeedDown;

	public bool useIdleAnimeReset = true;

	private bool _syncApplyingAnimation;

	public bool isUnlimitedFire;

	public int randFreshIndex;

	public double groundRight;

	private bool _componentGameplayUntilBattlefieldEntry;

	private bool _componentGameplayPhysicsDispatchActive;

	private bool _componentGameplayPhysicsSnapshot;

	private ulong _componentGameplayPhysicsSnapshotFrame = 18446744073709551615uL;

	private ulong _physicsGlobalPositionFrame = 18446744073709551615uL;

	private Vector2 _physicsGlobalPosition;

	private Transform2D _shadowGlobalTransform;

	private bool _shadowGlobalTransformValid;

	private Vector2 _physicsLocalPosition;

	private bool _physicsLocalPositionValid;

	private bool _writingCachedGlobalPosition;

	private Transform2D _pendingCachedGlobalTransform;

	private bool _pendingCachedGlobalTransformNotification;

	private Tween _cellMoveTween;

	private static readonly StringName PropPreviewDamagePoint = "PreviewDamagePointPersontage";

	private static readonly StringName PropArmor = "Armor";

	private static readonly StringName PropCustom = "Custom";

	private const string DamagePartSlotPrefix = "DamagePartSlot/";

	private const int ZMotionLocalRenderReleasePhysicsFrames = 2;

	private bool _zMotionOwnsLocalRender;

	private int _zMotionStablePhysicsFrames;

	private float _spriteGroupLocalX;

	private bool _spriteGroupLocalXCaptured;

	public bool isDestroy;

	private (bool Sleep, bool WakeUp)? _pendingCoverSleepState;

	private bool _progressRestorePresentationRefreshPending;

	private static PackedScene _defaultTransformEffect;

	private const int TransformTimeoutFrames = 120;

	private const int TransformSettleFrames = 2;

	private readonly List<AdobeAnimateExternalVisualHandle> _legacySpriteGroupVisualHandles = new List<AdobeAnimateExternalVisualHandle>();

	private bool _characterExternalVisualsInitialized;

	private bool _legacySpriteGroupVisualsScanned;

	public bool IsRemoteNetworkReplica
	{
		get
		{
			if (syncId >= 0 && Global.IsMultiplayerMode)
			{
				return !MultiPlayerManager.IsHost;
			}
			return false;
		}
	}

	public bool HasValidRuntimeConfiguration { get; private set; }

	private static PackedScene FIRE => _FIRE ?? (_FIRE = GD.Load<PackedScene>("uid://c4fjg42kgeupv"));

	private static PackedScene ICE_FIRE => _ICE_FIRE ?? (_ICE_FIRE = GD.Load<PackedScene>("uid://bvuvd1ircxhxn"));

	private static PackedScene MEGA_FIRE => _MEGA_FIRE ?? (_MEGA_FIRE = GD.Load<PackedScene>("uid://c61k0gdy05vo4"));

	private static PackedScene PURIFY_FIRE => _PURIFY_FIRE ?? (_PURIFY_FIRE = GD.Load<PackedScene>("uid://cxmk8jiq0nkjg"));

	private static PackedScene WHITE_FIRE => _WHITE_FIRE ?? (_WHITE_FIRE = GD.Load<PackedScene>("uid://dsxnymbe4mghd"));

	private static PackedScene GARLIC_FIRE => _GARLIC_FIRE ?? (_GARLIC_FIRE = GD.Load<PackedScene>("uid://dsxnymbe4mghd"));

	private static PackedScene SNOW_FLAKES => _SNOW_FLAKES ?? (_SNOW_FLAKES = GD.Load<PackedScene>("uid://b1ba7ajcvcgj8"));

	public bool HasArmorHurtSubscribers => OnArmorHurt != null;

	public Sprite2D icetrapSprite => buff?.GetCachedBuffVisual("Frozen");

	[Export(PropertyHint.None, "")]
	public CharacterHitBoxDefinition HitBoxDefinition
	{
		get
		{
			return _hitBoxDefinition;
		}
		set
		{
			if (_hitBoxDefinition != value)
			{
				_hitBoxDefinition = value;
				if (!_hitBoxRuntimeInitialized)
				{
					ResetHitBoxRuntimeFromDefinition();
				}
				InvalidateHitBoxBounds();
				QueueRedraw();
			}
		}
	}

	internal ulong HitBoxBoundsRevision => _hitBoxBoundsRevision;

	public bool HasHitBox
	{
		get
		{
			EnsureHitBoxRuntimeInitialized();
			if (_hitBoxAvailable && GodotObject.IsInstanceValid(_hitBoxDefinition))
			{
				return _hitBoxDefinition.HasValidGeometry;
			}
			return false;
		}
	}

	public bool IsHitBoxEnabled
	{
		get
		{
			EnsureHitBoxRuntimeInitialized();
			if (HasHitBox && _hitBoxDefaultEnabled)
			{
				return _hitBoxSuppression == HitBoxSuppressionReason.None;
			}
			return false;
		}
	}

	public bool IsHitBoxMonitorable
	{
		get
		{
			EnsureHitBoxRuntimeInitialized();
			if (HasHitBox && _hitBoxDefaultMonitorable)
			{
				return _hitBoxMonitorSuppression == HitBoxSuppressionReason.None;
			}
			return false;
		}
	}

	public Rect2 WorldBroadphaseRect => WorldHitRect;

	internal float CachedLocalScaleX
	{
		get
		{
			if (!_localScaleXSnapshotValid)
			{
				RefreshLocalScaleXSnapshot();
			}
			return _localScaleXSnapshot;
		}
	}

	public Rect2 WorldHitRect
	{
		get
		{
			ulong num = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (num == 18446744073709551615uL)
			{
				num = Engine.GetPhysicsFrames();
			}
			if (_worldHitRectCacheValid && _worldHitRectCacheFrame == (long)num && _worldHitRectCacheDefinition == _hitBoxDefinition)
			{
				return _worldHitRectCache;
			}
			Transform2D globalTransformForShadow = GetGlobalTransformForShadow(num);
			if (!_localScaleXSnapshotValid)
			{
				RefreshLocalScaleXSnapshot();
			}
			_worldHitRectCache = ((GodotObject.IsInstanceValid(_hitBoxDefinition) && _hitBoxDefinition.TryGetWorldRect(globalTransformForShadow, out var rect)) ? rect : AabbShapeUtil.RectFromCenter(globalTransformForShadow.Origin, AabbShapeUtil.DefaultAreaSize));
			_worldHitRectCacheFrame = (long)num;
			_worldHitRectCacheTransform = globalTransformForShadow;
			_worldHitRectCacheScaleX = _localScaleXSnapshot;
			_worldHitRectCacheDefinition = _hitBoxDefinition;
			_worldHitRectCacheValid = true;
			return _worldHitRectCache;
		}
	}

	[Export(PropertyHint.None, "")]
	public StateMachineDefinition MainStateMachineDefinition { get; set; }

	public IStateMachineController StateMachine { get; private set; }

	public StateHandle CurrentStateHandle => StateMachine?.CurrentStateHandle;

	internal bool WantsMainStateMachineProcessDispatch
	{
		get
		{
			if (_mainStateMachineDispatchEnabled)
			{
				StateMachineController mainStateMachineController = _mainStateMachineController;
				if (mainStateMachineController == null || !mainStateMachineController.HasProcessWork)
				{
					return componentManager?.HasStateMachineProcessWork ?? false;
				}
				return true;
			}
			return false;
		}
	}

	internal bool IsBowlingImpactDisplacementActive => _bowlingImpactDisplacementActive;

	internal bool IsOwnerBatchRegistered => _ownerBatchRegistered;

	internal bool BatchUsesInheritedProcessMode => _batchUsesInheritedProcessMode;

	internal Node BatchProcessModeParent => _batchProcessModeParent;

	internal bool IsOwnerBatchDispatchActive
	{
		get
		{
			if (_ownerBatchRegistered && inGame)
			{
				return !isDestroy;
			}
			return false;
		}
	}

	private bool CanDispatchMainStateMachine
	{
		get
		{
			if (_mainStateMachineDispatchEnabled)
			{
				StateMachineController mainStateMachineController = _mainStateMachineController;
				if (mainStateMachineController != null && mainStateMachineController.IsInitialized && inGame)
				{
					return !isDestroy;
				}
			}
			return false;
		}
	}

	private bool CanDispatchOwnedStateMachines
	{
		get
		{
			if (_mainStateMachineDispatchEnabled && inGame)
			{
				return !isDestroy;
			}
			return false;
		}
	}

	[Export(PropertyHint.None, "")]
	public CharacterComponentSet ComponentSet { get; set; }

	[Export(PropertyHint.None, "")]
	public bool invisible
	{
		get
		{
			return _invisible;
		}
		set
		{
			if (_invisible == value)
			{
				return;
			}
			_invisible = value;
			if (!IsNodeReady())
			{
				return;
			}
			if (_invisible)
			{
				sprite.invisible = true;
				sprite.Visible = false;
				shadowSprite.Visible = false;
				ShadowComponent shadowComponent = this.shadowComponent;
				if (shadowComponent != null && !shadowComponent.IsReleased)
				{
					this.shadowComponent.SetShadowVisible(visible: false);
				}
				ShowHealthComponent showHealthComponent = this.showHealthComponent;
				if (showHealthComponent != null && !showHealthComponent.IsReleased)
				{
					this.showHealthComponent.Visible = false;
				}
			}
			else
			{
				sprite.invisible = false;
				sprite.Visible = true;
				shadowSprite.Visible = !inWater;
				ShadowComponent shadowComponent2 = this.shadowComponent;
				if (shadowComponent2 != null && !shadowComponent2.IsReleased)
				{
					this.shadowComponent.SetShadowVisible(!inWater);
				}
				ShowHealthComponent showHealthComponent2 = this.showHealthComponent;
				if (showHealthComponent2 != null && !showHealthComponent2.IsReleased)
				{
					this.showHealthComponent.Visible = true;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterConfig config
	{
		get
		{
			return _config;
		}
		set
		{
			_config = value;
			if (_currentCustom != null)
			{
				_currentCustom = new Array<string>();
			}
			if (IsNodeReady())
			{
				if (!CachedEditorHint && GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.characterRegistry))
				{
					TowerDefenseManager.Instance.characterRegistry.NotifyCharacterConfigChanged(this);
				}
				DamagePartInit();
				NotifyPropertyListChanged();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite sprite
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_sprite))
			{
				return null;
			}
			return _sprite;
		}
		set
		{
			_sprite = value;
			if (IsNodeReady())
			{
				if (config != null && config.customData != null && sprite != null)
				{
					SetCustoms(currentCustom);
				}
				DamagePartInit();
				NotifyPropertyListChanged();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSlot headSlot
	{
		get
		{
			return _headSlot;
		}
		set
		{
			_headSlot = value;
			if (IsNodeReady())
			{
				DamagePartInit();
				NotifyPropertyListChanged();
			}
		}
	}

	public EconomyAccountId EconomyOwnerAccountId => _economyOwnerAccountId;

	public bool HasEconomyOwner => _economyOwnerAccountId.IsValid;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.CHARACTER_CAMP camp
	{
		get
		{
			return _camp;
		}
		set
		{
			if (_camp != value)
			{
				_camp = value;
				TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
				if (GodotObject.IsInstanceValid(towerDefenseManager) && GodotObject.IsInstanceValid(towerDefenseManager.characterRegistry))
				{
					towerDefenseManager.characterRegistry.NotifyCharacterCampChanged(this);
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public string damagePartClip
	{
		get
		{
			return _damagePartClip;
		}
		set
		{
			_damagePartClip = value;
			if (IsNodeReady())
			{
				DamagePartInit();
				NotifyPropertyListChanged();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Dictionary damagePart
	{
		get
		{
			return _damagePart ?? (_damagePart = new Dictionary());
		}
		set
		{
			_damagePart = value;
		}
	}

	public List<string> damagePartList => _damagePartList ?? (_damagePartList = new List<string>());

	[Export(PropertyHint.None, "")]
	public Dictionary damagePartSlot
	{
		get
		{
			return _damagePartSlot ?? (_damagePartSlot = new Dictionary());
		}
		set
		{
			_damagePartSlot = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public double previewDamagePointPersontage
	{
		get
		{
			return _previewDamagePointPersontage;
		}
		set
		{
			_previewDamagePointPersontage = value;
			if (IsNodeReady() && config != null && config.damagePointData != null && sprite != null)
			{
				PreviewDamagePoint(value);
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<string> currentArmor
	{
		get
		{
			return _currentArmor ?? (_currentArmor = new Array<string>());
		}
		set
		{
			_currentArmor = value;
			if (IsNodeReady() && config != null && config.armorData != null && sprite != null)
			{
				SetArmors(_currentArmor);
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<string> currentCustom
	{
		get
		{
			return _currentCustom ?? (_currentCustom = new Array<string>());
		}
		set
		{
			_currentCustom = value;
			if (IsNodeReady() && config != null && config.customData != null && sprite != null)
			{
				SetCustoms(_currentCustom);
			}
		}
	}

	public bool componentAlive
	{
		get
		{
			if (_componentAlive)
			{
				return !characterDisabled;
			}
			return false;
		}
		set
		{
			_componentAlive = value;
		}
	}

	public List<TowerDefenseCharacterEventBase> dieEvent => _dieEvent ?? (_dieEvent = new List<TowerDefenseCharacterEventBase>());

	public bool inWater
	{
		get
		{
			return _inWater;
		}
		set
		{
			if (_inWater == value)
			{
				return;
			}
			_inWater = value;
			if (inGame)
			{
				if (value)
				{
					InWater();
				}
				else
				{
					OutWater();
				}
			}
			groundHeightComponent?.NotifyEnvironmentChanged();
		}
	}

	public bool HasComponentGameplayUntilBattlefieldEntry => _componentGameplayUntilBattlefieldEntry;

	public bool IsInsideComponentBattlefield
	{
		get
		{
			if (!inGame || _componentGameplayUntilBattlefieldEntry || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				return true;
			}
			float x = GetLogicalGlobalPosition().X;
			if ((double)x >= TowerDefenseManager.Instance.GetMapGroundLeft())
			{
				return (double)x <= TowerDefenseManager.Instance.GetMapGroundRight();
			}
			return false;
		}
	}

	internal bool IsInsideComponentBattlefieldForRuntime
	{
		get
		{
			if (!_componentGameplayPhysicsDispatchActive)
			{
				return GetComponentGameplayForCurrentPhysicsFrame();
			}
			return _componentGameplayPhysicsSnapshot;
		}
	}

	public virtual bool PreserveDeathTransformation => false;

	public virtual bool IsHardControlImmune => false;

	public bool IsProgressRestoreInFlight { get; private set; }

	private static PackedScene DefaultTransformEffect => _defaultTransformEffect ?? (_defaultTransformEffect = GD.Load<PackedScene>("uid://k8ms8gteeqp6"));

	public int CharacterExternalVisualCompatibilityScanCount { get; private set; }

	public int CharacterAnimatedStatusVisualCount { get; private set; }

	public int CharacterAnimatedStatusTopologyChangeCount { get; private set; }

	public event DestroyEventHandler OnDestroy;

	public event CharacterNearDieEventHandler OnCharacterNearDie;

	public event BodyHurtEventHandler OnBodyHurt;

	public event ArmorHurtEventHandler OnArmorHurt;

	public event RiseOverEventHandler OnRiseOver;

	public event ComponentChangeEventHandler OnComponentChange;

	public event DamageBlockedEventHandler OnDamageBlocked;

	public void EmitDestroy()
	{
		OnDestroy?.Invoke(this);
	}

	public void EmitBodyHurt(int num)
	{
		OnBodyHurt?.Invoke(num);
	}

	public void EmitArmorHurt(int num)
	{
		OnArmorHurt?.Invoke(num);
	}

	public void EmitRiseOver()
	{
		OnRiseOver?.Invoke();
	}

	public void EmitComponentChange()
	{
		OnComponentChange?.Invoke();
	}

	public void EmitDamageBlocked()
	{
		OnDamageBlocked?.Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal bool CanReceiveExplosionHit()
	{
		if (ProcessMode == ProcessModeEnum.Disabled)
		{
			return false;
		}
		EnsureHitBoxRuntimeInitialized();
		return _canReceiveExplosionHit;
	}

	public void SetHitBoxEnabled(bool enabled)
	{
		EnsureHitBoxRuntimeInitialized();
		if (_hitBoxDefaultEnabled != enabled)
		{
			_hitBoxDefaultEnabled = enabled;
			InvalidateHitBoxBounds();
		}
	}

	public void SetHitBoxMonitorable(bool monitorable)
	{
		EnsureHitBoxRuntimeInitialized();
		if (_hitBoxDefaultMonitorable != monitorable)
		{
			_hitBoxDefaultMonitorable = monitorable;
			RefreshExplosionHitEligibility();
			InvalidateHitBoxBounds();
		}
	}

	public void SetHitBoxSuppressed(HitBoxSuppressionReason reason, bool suppressed)
	{
		if (reason != HitBoxSuppressionReason.None)
		{
			EnsureHitBoxRuntimeInitialized();
			HitBoxSuppressionReason hitBoxSuppression = _hitBoxSuppression;
			if (suppressed)
			{
				_hitBoxSuppression |= reason;
			}
			else
			{
				_hitBoxSuppression &= ~reason;
			}
			if (_hitBoxSuppression != hitBoxSuppression)
			{
				InvalidateHitBoxBounds();
			}
		}
	}

	public void SetHitBoxMonitorSuppressed(HitBoxSuppressionReason reason, bool suppressed)
	{
		if (reason != HitBoxSuppressionReason.None)
		{
			EnsureHitBoxRuntimeInitialized();
			HitBoxSuppressionReason hitBoxMonitorSuppression = _hitBoxMonitorSuppression;
			if (suppressed)
			{
				_hitBoxMonitorSuppression |= reason;
			}
			else
			{
				_hitBoxMonitorSuppression &= ~reason;
			}
			if (_hitBoxMonitorSuppression != hitBoxMonitorSuppression)
			{
				RefreshExplosionHitEligibility();
				InvalidateHitBoxBounds();
			}
		}
	}

	public void DestroyHitBoxRuntime()
	{
		EnsureHitBoxRuntimeInitialized();
		_hitBoxAvailable = false;
		_hitBoxSuppression |= HitBoxSuppressionReason.Destroyed;
		_hitBoxMonitorSuppression |= HitBoxSuppressionReason.Destroyed;
		RefreshExplosionHitEligibility();
		InvalidateHitBoxBounds();
	}

	public void InvalidateHitBoxBounds()
	{
		_hitBoxBoundsRevision++;
		if (_hitBoxBoundsRevision == 0L)
		{
			_hitBoxBoundsRevision = 1uL;
		}
		_worldHitRectCacheValid = false;
		_worldHitRectCacheFrame = -1L;
		if (!CachedEditorHint && GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.characterRegistry))
		{
			TowerDefenseManager.Instance.characterRegistry.NotifyCharacterGeometryChanged(this);
		}
	}

	protected override void OnGridPositionChanged(Vector2I oldGridPos, Vector2I newGridPos)
	{
		base.OnGridPositionChanged(oldGridPos, newGridPos);
		groundHeightComponent?.NotifyCellChanged();
		if (!CachedEditorHint && GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.characterRegistry))
		{
			TowerDefenseManager.Instance.characterRegistry.NotifyCharacterGridChanged(this);
		}
	}

	protected override void OnGroundHeightChanged(double oldGroundHeight, double newGroundHeight)
	{
		base.OnGroundHeightChanged(oldGroundHeight, newGroundHeight);
		groundHeightComponent?.NotifyOwnerGroundHeightChanged();
		shadowComponent?.MarkDirty();
	}

	public bool TryGetActiveWorldHitRect(out Rect2 rect, bool requireMonitorable = false)
	{
		if (!IsHitBoxEnabled || (requireMonitorable && !IsHitBoxMonitorable))
		{
			rect = default;
			return false;
		}
		rect = WorldHitRect;
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal bool TryGetProjectileWorldHitRect(out Rect2 rect)
	{
		if (_hitBoxRuntimeInitialized && _hitBoxAvailable && _hitBoxDefaultEnabled && _hitBoxSuppression == HitBoxSuppressionReason.None && _worldHitRectCacheValid)
		{
			rect = _worldHitRectCache;
			return true;
		}
		return TryGetActiveWorldHitRect(out rect);
	}

	internal bool TryGetCachedHitTransform(out float originX, out float scaleX)
	{
		if (!_worldHitRectCacheValid)
		{
			originX = 0f;
			scaleX = 0f;
			return false;
		}
		originX = _worldHitRectCacheTransform.Origin.X;
		scaleX = _worldHitRectCacheScaleX;
		return true;
	}

	internal Vector2 GetCachedWorldPositionForProjectile()
	{
		if (!_worldHitRectCacheValid)
		{
			_ = WorldHitRect;
		}
		return _worldHitRectCacheTransform.Origin;
	}

	private void RefreshLocalScaleXSnapshot()
	{
		_localScaleXSnapshot = Scale.X;
		_localScaleXSnapshotValid = true;
	}

	private void NotifyRenderAncestorScaleChangedIfNeeded()
	{
		Vector2 scale = Scale;
		if (!_renderAncestorScaleSnapshotValid || !Mathf.IsEqualApprox(scale.X, _renderAncestorScaleSnapshot.X) || !Mathf.IsEqualApprox(scale.Y, _renderAncestorScaleSnapshot.Y))
		{
			_renderAncestorScaleSnapshot = scale;
			_renderAncestorScaleSnapshotValid = true;
			if (GodotObject.IsInstanceValid(_sprite))
			{
				_sprite.NotifyAncestorTransformChangedForRender();
			}
		}
	}

	private void EnsureHitBoxRuntimeInitialized()
	{
		if (!_hitBoxRuntimeInitialized)
		{
			ResetHitBoxRuntimeFromDefinition();
			_hitBoxRuntimeInitialized = true;
		}
	}

	private void ResetHitBoxRuntimeFromDefinition()
	{
		_hitBoxAvailable = GodotObject.IsInstanceValid(_hitBoxDefinition) && _hitBoxDefinition.HasValidGeometry;
		_hitBoxDefaultEnabled = _hitBoxDefinition?.DefaultEnabled ?? false;
		_hitBoxDefaultMonitorable = _hitBoxDefinition?.DefaultMonitorable ?? false;
		_hitBoxSuppression = HitBoxSuppressionReason.None;
		_hitBoxMonitorSuppression = HitBoxSuppressionReason.None;
		RefreshExplosionHitEligibility();
	}

	private void RefreshExplosionHitEligibility()
	{
		_canReceiveExplosionHit = _hitBoxAvailable && GodotObject.IsInstanceValid(_hitBoxDefinition) && _hitBoxDefinition.HasValidGeometry && _hitBoxDefaultMonitorable && _hitBoxMonitorSuppression == HitBoxSuppressionReason.None;
	}

	public void SetCollisionPreviewDraw(bool enabled)
	{
		_hitBoxPreviewDraw = enabled;
		QueueRedraw();
	}

	public override void _Draw()
	{
		if (CachedEditorHint || _hitBoxPreviewDraw)
		{
			DrawPrimaryHitBoxPreview();
			DrawResourceComponentCollisionPreviews();
		}
	}

	private void DrawPrimaryHitBoxPreview()
	{
		if (GodotObject.IsInstanceValid(_hitBoxDefinition) && _hitBoxDefinition.DebugDraw && _hitBoxDefinition.HasValidGeometry)
		{
			Vector2 vector = _hitBoxDefinition.Size * 0.5f;
			Transform2D localTransform = _hitBoxDefinition.LocalTransform;
			Vector2[] array = new Vector2[4]
			{
				localTransform * new Vector2(0f - vector.X, 0f - vector.Y),
				localTransform * new Vector2(vector.X, 0f - vector.Y),
				localTransform * new Vector2(vector.X, vector.Y),
				localTransform * new Vector2(0f - vector.X, vector.Y)
			};
			if (_hitBoxDefinition.DebugFillColor.A > 0f)
			{
				DrawColoredPolygon(array, _hitBoxDefinition.DebugFillColor);
			}
			Vector2[] points = new Vector2[5]
			{
				array[0],
				array[1],
				array[2],
				array[3],
				array[0]
			};
			DrawPolyline(points, _hitBoxDefinition.DebugOutlineColor, Mathf.Max(0.5f, _hitBoxDefinition.DebugLineWidth), antialiased: true);
		}
	}

	private void DrawResourceComponentCollisionPreviews()
	{
		CharacterComponentSet characterComponentSet = ComponentSet;
		if (!GodotObject.IsInstanceValid(characterComponentSet))
		{
			characterComponentSet = GetNodeOrNull<LegacyComponentManagerNode>("%ComponentManager")?.ComponentSet;
		}
		if (!GodotObject.IsInstanceValid(characterComponentSet))
		{
			return;
		}
		IReadOnlyList<CharacterComponentDefinition> flattenedDefinitions = characterComponentSet.GetFlattenedDefinitions();
		for (int i = 0; i < flattenedDefinitions.Count; i++)
		{
			CharacterComponentDefinition characterComponentDefinition = flattenedDefinitions[i];
			if (characterComponentDefinition is AttackComponentDefinition attackComponentDefinition)
			{
				AttackComponent attackComponent = componentManager?.GetRuntime<AttackComponent>(attackComponentDefinition.InstanceId);
				if (attackComponent != null && !attackComponent.IsReleased && attackComponent.DrawCheckAreaCollisionPreview(this))
				{
					continue;
				}
			}
			if (characterComponentDefinition is ICharacterComponentCollisionPreviewProvider characterComponentCollisionPreviewProvider)
			{
				for (int j = 0; j < characterComponentCollisionPreviewProvider.CollisionPreviewShapeCount; j++)
				{
					AabbCollisionResourceDebugDraw.DrawShape(this, characterComponentCollisionPreviewProvider.GetCollisionPreviewShape(j));
				}
				for (int k = 0; k < characterComponentCollisionPreviewProvider.CollisionPreviewRayCount; k++)
				{
					AabbCollisionResourceDebugDraw.DrawRay(this, characterComponentCollisionPreviewProvider.GetCollisionPreviewRay(k));
				}
			}
		}
	}

	public bool SendStateEvent(StringName eventName)
	{
		eventName = NormalizeMainStateEvent(eventName);
		if (_authoritativeMainStateRestoreDepth == 0)
		{
			return StateMachine?.SendEvent(eventName) ?? false;
		}
		return false;
	}

	private bool SetMainRuntimeState(MainRuntimeState state)
	{
		StateHandle targetState;
		StringName eventName;
		switch (state)
		{
		case MainRuntimeState.Sleep:
			targetState = _sleepStateHandle;
			eventName = ToSleepStateEvent;
			break;
		case MainRuntimeState.Component:
			targetState = _componentStateHandle;
			eventName = ToComponentStateEvent;
			break;
		default:
			targetState = _idleStateHandle;
			eventName = ToIdleStateEvent;
			break;
		}
		eventName = NormalizeMainStateEvent(eventName);
		if (_authoritativeMainStateRestoreDepth == 0)
		{
			IStateMachineController stateMachine = StateMachine;
			if (stateMachine != null && stateMachine.IsInitialized)
			{
				if (!StateMachine.TrySetFlatState(targetState, eventName))
				{
					return StateMachine.SendEvent(eventName);
				}
				return true;
			}
		}
		return false;
	}

	protected virtual StringName NormalizeMainStateEvent(StringName eventName)
	{
		return eventName;
	}

	public StateHandle GetStateById(string stableId)
	{
		return StateMachine?.GetStateById(stableId);
	}

	internal void PrepareBatchRegistration()
	{
		_batchUsesInheritedProcessMode = ProcessMode == ProcessModeEnum.Inherit;
		_batchProcessModeParent = (_batchUsesInheritedProcessMode ? GetParent() : null);
	}

	public bool TryAssignEconomyOwner(EconomyAccountId accountId)
	{
		if (!accountId.IsValid)
		{
			return false;
		}
		if (_economyOwnerAccountId.IsValid)
		{
			return _economyOwnerAccountId == accountId;
		}
		_economyOwnerAccountId = accountId;
		return true;
	}

	internal void CancelCellMoveTween()
	{
		if (_cellMoveTween != null && _cellMoveTween.IsValid())
		{
			_cellMoveTween.Kill();
		}
		_cellMoveTween = null;
	}

	internal void TrackCellMoveTween(Tween tween)
	{
		_cellMoveTween = tween;
	}

	public void EnableComponentGameplayUntilBattlefieldEntry()
	{
		_componentGameplayUntilBattlefieldEntry = true;
	}

	private bool IsWithinComponentBattlefieldBoundsForPhysicsFrame()
	{
		if (!TowerDefenseProcessModeDispatch.TryGetComponentBattlefieldBoundsForCurrentPhysicsFrame(out var left, out var right))
		{
			return true;
		}
		float x = GetGlobalPositionForPhysicsFrame(TowerDefenseProcessModeDispatch.CurrentPhysicsFrame).X;
		if (x >= left)
		{
			return x <= right;
		}
		return false;
	}

	protected internal Vector2 GetGlobalPositionForPhysicsFrame(ulong physicsFrame)
	{
		if (physicsFrame != 18446744073709551615uL && _physicsGlobalPositionFrame == physicsFrame)
		{
			return _physicsGlobalPosition;
		}
		if (_physicsGlobalPositionFrame != physicsFrame)
		{
			_physicsGlobalPosition = (_shadowGlobalTransformValid ? _shadowGlobalTransform.Origin : GlobalPosition);
			_physicsGlobalPositionFrame = physicsFrame;
			if (_shadowGlobalTransformValid)
			{
				_shadowGlobalTransform.Origin = _physicsGlobalPosition;
			}
		}
		return _physicsGlobalPosition;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected internal void SetGlobalPositionForPhysicsFrame(Vector2 value, ulong physicsFrame)
	{
		SetGlobalPositionForPhysicsFrameNode(value, physicsFrame);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetGlobalPositionForPhysicsFrameNode(Vector2 value, ulong physicsFrame)
	{
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(physicsFrame);
		_writingCachedGlobalPosition = true;
		SetNotifyLocalTransform(enable: false);
		try
		{
			GlobalPosition = value;
		}
		finally
		{
			SetNotifyLocalTransform(enable: true);
			_writingCachedGlobalPosition = false;
		}
		bool flag = globalPositionForPhysicsFrame != value;
		_physicsLocalPositionValid = false;
		_physicsGlobalPosition = value;
		_physicsGlobalPositionFrame = physicsFrame;
		if (_shadowGlobalTransformValid)
		{
			_shadowGlobalTransform.Origin = value;
			if (flag)
			{
				_pendingCachedGlobalTransform = _shadowGlobalTransform;
				_pendingCachedGlobalTransformNotification = true;
			}
		}
		if (flag)
		{
			InvalidateHitBoxBounds();
			PublishCharacterTranslationForRender(value - globalPositionForPhysicsFrame, physicsFrame);
		}
		InvalidateComponentGameplayForPhysicsFrame();
	}

	internal void TranslateForPhysicsFrame(Vector2 localDelta, Vector2 globalDelta, ulong physicsFrame)
	{
		Vector2 vector = (_physicsLocalPositionValid ? _physicsLocalPosition : Position) + localDelta;
		_writingCachedGlobalPosition = true;
		SetNotifyLocalTransform(enable: false);
		try
		{
			Position = vector;
		}
		finally
		{
			SetNotifyLocalTransform(enable: true);
			_writingCachedGlobalPosition = false;
		}
		_physicsLocalPosition = vector;
		_physicsLocalPositionValid = true;
		if (_shadowGlobalTransformValid)
		{
			_shadowGlobalTransform.Origin += globalDelta;
			if (localDelta != Vector2.Zero)
			{
				_pendingCachedGlobalTransform = _shadowGlobalTransform;
				_pendingCachedGlobalTransformNotification = true;
			}
		}
		if (_physicsGlobalPositionFrame == physicsFrame)
		{
			_physicsGlobalPosition += globalDelta;
		}
		else if (_shadowGlobalTransformValid)
		{
			_physicsGlobalPosition = _shadowGlobalTransform.Origin;
			_physicsGlobalPositionFrame = physicsFrame;
		}
		if (localDelta != Vector2.Zero)
		{
			InvalidateHitBoxBounds();
			PublishCharacterTranslationForRender(globalDelta, physicsFrame);
		}
		InvalidateComponentGameplayForPhysicsFrame();
	}

	private void PublishCharacterTranslationForRender(Vector2 globalDelta, ulong physicsFrame)
	{
		if (!(globalDelta == Vector2.Zero))
		{
			AdobeAnimateSprite adobeAnimateSprite = _sprite;
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.NotifyAncestorTranslatedForRender(globalDelta, physicsFrame);
			}
		}
	}

	internal Transform2D GetGlobalTransformForShadow(ulong physicsFrame)
	{
		if (physicsFrame != 18446744073709551615uL && _shadowGlobalTransformValid && _physicsGlobalPositionFrame == physicsFrame)
		{
			_shadowGlobalTransform.Origin = _physicsGlobalPosition;
			return _shadowGlobalTransform;
		}
		if (!_shadowGlobalTransformValid)
		{
			_shadowGlobalTransform = GlobalTransform;
			_shadowGlobalTransformValid = true;
			_physicsGlobalPosition = _shadowGlobalTransform.Origin;
			_physicsGlobalPositionFrame = physicsFrame;
		}
		else if (_physicsGlobalPositionFrame == physicsFrame)
		{
			_shadowGlobalTransform.Origin = _physicsGlobalPosition;
		}
		return _shadowGlobalTransform;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal Transform2D GetGlobalTransformForPhysicsFrame(ulong physicsFrame)
	{
		return GetGlobalTransformForShadow(physicsFrame);
	}

	public Vector2 GetLogicalGlobalPosition()
	{
		return GlobalPosition;
	}

	public Vector2 GetLogicalGlobalPosition(Node2D descendant)
	{
		if (!GodotObject.IsInstanceValid(descendant))
		{
			return GetLogicalGlobalPosition();
		}
		return descendant.GlobalPosition;
	}

	public Vector2 GetLogicalGlobalPosition(TowerDefenseShadowVisual visual)
	{
		if (!GodotObject.IsInstanceValid(visual))
		{
			return GetLogicalGlobalPosition();
		}
		return visual.GlobalPosition;
	}

	public Transform2D GetLogicalGlobalTransform(Node2D descendant)
	{
		if (!GodotObject.IsInstanceValid(descendant))
		{
			return GetGlobalTransformForShadow(Engine.GetPhysicsFrames());
		}
		return descendant.GlobalTransform;
	}

	public void SetLogicalGlobalPosition(Vector2 value)
	{
		ulong num = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (num == 18446744073709551615uL)
		{
			num = Engine.GetPhysicsFrames();
		}
		SetGlobalPositionForPhysicsFrame(value, num);
	}

	internal void SetCellMoveLogicalGlobalPosition(Vector2 value)
	{
		SetLogicalGlobalPosition(value);
		RefreshCellMoveRenderOrder(value);
	}

	internal void RefreshCellMoveRenderOrder(Vector2 value)
	{
		FreshZIndexForGlobalPosition(value, "cell-move-visual-position");
	}

	internal void DisableGameplayForPermanentEmbeddedVisual()
	{
		inGame = false;
	}

	private void InvalidateGlobalPositionForPhysicsFrame()
	{
		_physicsGlobalPositionFrame = 18446744073709551615uL;
	}

	private void InvalidateComponentGameplayForPhysicsFrame()
	{
		_componentGameplayPhysicsSnapshotFrame = 18446744073709551615uL;
	}

	protected bool IsStateActivationCurrent(StateHandle stateHandle, ulong activationGeneration)
	{
		if (GodotObject.IsInstanceValid(this) && IsInsideTree() && inGame && !die && !nearDie && !isDestroy && stateHandle != null && stateHandle.IsActive)
		{
			return stateHandle.ActivationGeneration == activationGeneration;
		}
		return false;
	}

	protected async Task<bool> WaitForStateDelayAsync(StateHandle stateHandle, double seconds)
	{
		if (stateHandle == null || !stateHandle.IsActive || !GodotObject.IsInstanceValid(GetTree()))
		{
			return false;
		}
		ulong activationGeneration = stateHandle.ActivationGeneration;
		await ToSignal(GetTree().CreateTimer(seconds, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		return IsStateActivationCurrent(stateHandle, activationGeneration);
	}

	protected async Task<bool> WaitForStatePhysicsFramesAsync(StateHandle stateHandle, int frameCount)
	{
		if (stateHandle == null || !stateHandle.IsActive || !GodotObject.IsInstanceValid(GetTree()))
		{
			return false;
		}
		ulong activationGeneration = stateHandle.ActivationGeneration;
		for (int frame = 0; frame < frameCount; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			if (!IsStateActivationCurrent(stateHandle, activationGeneration))
			{
				return false;
			}
		}
		return true;
	}

	private bool GetComponentGameplayForCurrentPhysicsFrame()
	{
		if (!inGame || _componentGameplayUntilBattlefieldEntry)
		{
			return true;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (currentPhysicsFrame == 18446744073709551615uL)
		{
			return IsInsideComponentBattlefield;
		}
		if (_componentGameplayPhysicsSnapshotFrame == currentPhysicsFrame)
		{
			return _componentGameplayPhysicsSnapshot;
		}
		CacheComponentGameplayForPhysicsFrame(currentPhysicsFrame);
		return _componentGameplayPhysicsSnapshot;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshComponentGameplayEntryExceptionForPhysicsFrame()
	{
		if (_componentGameplayUntilBattlefieldEntry && IsWithinComponentBattlefieldBoundsForPhysicsFrame())
		{
			_componentGameplayUntilBattlefieldEntry = false;
		}
	}

	private bool ResolveComponentGameplayForPhysicsFrame()
	{
		if (inGame && !_componentGameplayUntilBattlefieldEntry)
		{
			return IsWithinComponentBattlefieldBoundsForPhysicsFrame();
		}
		return true;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CacheComponentGameplayForPhysicsFrame(ulong physicsFrame)
	{
		_componentGameplayPhysicsSnapshot = ResolveComponentGameplayForPhysicsFrame();
		_componentGameplayPhysicsSnapshotFrame = physicsFrame;
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		if (config != null)
		{
			if (config.damagePointData != null)
			{
				array.Add(new Dictionary
				{
					["name"] = "PreviewDamagePointPersontage",
					["type"] = 3,
					["hint"] = 1,
					["hint_string"] = "0.0,1.0,0.01",
					["usage"] = num
				});
			}
			if (config.armorData != null)
			{
				array.Add(new Dictionary
				{
					["name"] = "Armor",
					["type"] = 28,
					["hint"] = 2,
					["hint_string"] = $"{4}/{2}:{string.Join(",", config.armorData.armorDictionary.Keys)}",
					["usage"] = num
				});
			}
			if (config.customData != null)
			{
				array.Add(new Dictionary
				{
					["name"] = "Custom",
					["type"] = 28,
					["hint"] = 2,
					["hint_string"] = $"{4}/{2}:{string.Join(",", config.customData.customDictionary.Keys)}",
					["usage"] = num
				});
			}
		}
		List<string> list = _damagePartList;
		if (list != null && list.Count > 0)
		{
			foreach (string damagePart in _damagePartList)
			{
				array.Add(new Dictionary
				{
					["name"] = "DamagePartSlot/" + damagePart,
					["type"] = 22,
					["hint"] = 26,
					["hint_string"] = "AdobeAnimateSlot",
					["usage"] = num
				});
			}
		}
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property == PropPreviewDamagePoint)
		{
			previewDamagePointPersontage = value.AsDouble();
			return true;
		}
		if (property == PropArmor)
		{
			Array<string> array = new Array<string>();
			foreach (Variant item in value.AsGodotArray())
			{
				array.Add(item.AsString());
			}
			currentArmor = array;
			return true;
		}
		if (property == PropCustom)
		{
			Array<string> array2 = new Array<string>();
			foreach (Variant item2 in value.AsGodotArray())
			{
				array2.Add(item2.AsString());
			}
			currentCustom = array2;
			return true;
		}
		string text = property.ToString();
		if (text.StartsWith("DamagePartSlot/"))
		{
			string text2 = text.Substring("DamagePartSlot/".Length);
			damagePartSlot[text2] = value;
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if (property == PropPreviewDamagePoint)
		{
			return Variant.From<double>(previewDamagePointPersontage);
		}
		if (property == PropArmor)
		{
			return Variant.From<Array<string>>(currentArmor);
		}
		if (property == PropCustom)
		{
			return Variant.From<Array<string>>(currentCustom);
		}
		string text = property.ToString();
		if (text.StartsWith("DamagePartSlot/"))
		{
			string text2 = text.Substring("DamagePartSlot/".Length);
			Dictionary dictionary = _damagePartSlot;
			if (dictionary != null && dictionary.ContainsKey(text2))
			{
				return _damagePartSlot[text2];
			}
			return default;
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if (property == PropPreviewDamagePoint || property == PropArmor || property == PropCustom)
		{
			return true;
		}
		if (property.ToString().StartsWith("DamagePartSlot/"))
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (property == PropPreviewDamagePoint)
		{
			return Variant.From<double>(1.0);
		}
		if (property == PropCustom)
		{
			return Variant.From<Array<string>>(new Array<string>());
		}
		if (property == PropArmor)
		{
			return Variant.From<Array<string>>(new Array<string>());
		}
		property.ToString().StartsWith("DamagePartSlot/");
		return default;
	}

	private void EnsureComponentManagerResource(bool migrateLegacyHost)
	{
		LegacyComponentManagerNode nodeOrNull = GetNodeOrNull<LegacyComponentManagerNode>("%ComponentManager");
		if (!GodotObject.IsInstanceValid(ComponentSet) && GodotObject.IsInstanceValid(nodeOrNull))
		{
			ComponentSet = nodeOrNull.ComponentSet;
		}
		if (!GodotObject.IsInstanceValid(componentManager))
		{
			componentManager = new ComponentManager();
		}
		if (GodotObject.IsInstanceValid(ComponentSet) || !GodotObject.IsInstanceValid(componentManager.ComponentSet))
		{
			componentManager.ComponentSet = ComponentSet;
		}
		else
		{
			ComponentSet = componentManager.ComponentSet;
		}
		componentManager.AttachOwner(this);
		if (migrateLegacyHost && GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.MigrateComponentsTo(this);
			componentManager.AttachOwner(this);
			nodeOrNull.QueueFree();
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		if (!CachedEditorHint)
		{
			EnsureComponentManagerResource(migrateLegacyHost: false);
		}
		RefreshLocalScaleXSnapshot();
		SetNotifyTransform(enable: true);
		SetNotifyLocalTransform(enable: true);
		int num;
		if (!CachedEditorHint && IsNodeReady())
		{
			num = (inGame ? 1 : 0);
			if (num != 0)
			{
				InitializeCharacterExternalVisuals();
				if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.characterRegistry))
				{
					TowerDefenseManager.Instance.CharacterRegister(this);
				}
				AddToGroup("Character", persistent: true);
			}
		}
		else
		{
			num = 0;
		}
		if (!CachedEditorHint && _mainStateMachineDispatchEnabled)
		{
			ResumeOwnerBatchDispatch();
		}
		if (num != 0)
		{
			SubscribeCharacterSkinSwitched();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		DamagePartInit();
		if (config != null && config.damagePointData != null && sprite != null && previewDamagePointPersontage < 1.0)
		{
			PreviewDamagePoint(previewDamagePointPersontage);
		}
		if (config != null && config.customData != null && sprite != null)
		{
			SetCustoms(currentCustom);
		}
		if (CachedEditorHint)
		{
			return;
		}
		EnsureCharacterSkinSwitchedHandler();
		if (!GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(sprite))
		{
			DisableInvalidRuntimeCharacter();
			return;
		}
		HasValidRuntimeConfiguration = true;
		EnsureHitBoxRuntimeInitialized();
		randFreshIndex = (int)GD.Randi();
		if (TowerDefenseManager.Instance != null)
		{
			groundRight = TowerDefenseManager.Instance.GetMapGroundRight();
		}
		backEffectNode = GetNode<Node2D>("%BackEffectNode");
		frontEffectNode = GetNode<Node2D>("%FrontEffectNode");
		spriteGroup = GetNode<Node2D>("%SpriteGroup");
		transformPoint = GetNode<Marker2D>("SpriteGroup/TransformPoint");
		Sprite2D nodeOrNull = GetNodeOrNull<Sprite2D>("%ShadowSprite");
		bool flag = Global.Instance != null && Global.Instance.isEditor && SceneManager.CurrentScene == "LevelEditorStage";
		shadowSprite = TowerDefenseShadowVisual.Capture(this, nodeOrNull, !flag);
		if (GodotObject.IsInstanceValid(shadowSprite) && shadowSprite.RequiresLegacyRendering)
		{
			GD.PushWarning("[ShadowData:E_LEGACY_NODE] '" + SceneFilePath + "' keeps ShadowSprite because it requires native compatibility rendering.");
		}
		InitializeCharacterExternalVisuals();
		if (MainStateMachineDefinition == null)
		{
			GD.PushError($"Character '{Name}' is missing its main StateMachine definition.");
			SetMainStateMachineDispatchEnabled(enabled: false);
			return;
		}
		StateMachineController stateMachineController = (StateMachineController)(StateMachine = (_mainStateMachineController = new StateMachineController()));
		if (!stateMachineController.Initialize(MainStateMachineDefinition, this))
		{
			GD.PushError("Character state machine initialization failed: " + stateMachineController.InitializationError);
			SetMainStateMachineDispatchEnabled(enabled: false);
			return;
		}
		BindMainStateHandles();
		Callable.From(EnterInitialMainStateMachine).CallDeferred();
		EnsureComponentManagerResource(migrateLegacyHost: true);
		if (GodotObject.IsInstanceValid(componentManager))
		{
			componentManager.InitializeResourceComponents();
			if (PhonkComponent.phonkEnabled && phonkComponent == null)
			{
				PhonkComponent.InjectToCharacter(this);
			}
			if (DismemberComponent.dismemberEnabled && dismemberComponent == null && this is TowerDefenseZombie)
			{
				DismemberComponent.InjectToCharacter(this);
			}
		}
		SetMainStateMachineDispatchEnabled(ShouldEnableGameplayDispatch());
		instance = new TowerDefenseCharacterInstance();
		instance._Init(this, config);
		(bool, bool)? pendingCoverSleepState = _pendingCoverSleepState;
		if (pendingCoverSleepState.HasValue)
		{
			(bool, bool) valueOrDefault = pendingCoverSleepState.GetValueOrDefault();
			instance.sleep = valueOrDefault.Item1;
			instance.wakeUp = valueOrDefault.Item2;
			_pendingCoverSleepState = null;
		}
		instance.damagePointReach += DamagePointReach;
		instance.hitpointsNearDie += HitpointsNearDie;
		instance.hitpointsEmpty += HitpointsEmpty;
		instance.armorDamagePointReach += ArmorDamagePointReach;
		instance.armorHitpointsEmpty += ArmorHitpointsEmpty;
		sprite.OnAnimeCompleted += AnimeCompleted;
		sprite.OnAnimeEvent += AnimeEvent;
		sprite.OnAnimeStarted += OnAnimeStarted;
		componentManager.ActivateResourceComponents();
		ApplySpriteGroupZPosition();
		shadowComponent?.UpdateShadow();
		AddToGroup("Character", persistent: true);
		if (inGame && TowerDefenseManager.Instance != null)
		{
			TowerDefenseManager.Instance.CharacterRegister(this);
		}
		if (config != null)
		{
			AddToGroup(config.name, persistent: true);
		}
		RefreshPacketCustomFromSave();
		if (inGame && CanSleep())
		{
			Sleep();
		}
		isUnlimitedFire = inGame && !editorPreviewMode && TowerDefenseManager.IsUnlimitedFire();
		if (isUnlimitedFire)
		{
			UnlimitedFireInit();
		}
		if (inGame)
		{
			SubscribeCharacterSkinSwitched();
		}
		BindMainStateHandles();
		ConfigureCharacterBatchProcessing();
		if (inGame)
		{
			TowerDefenseManager.ApplyMapCharacterRules(this);
		}
	}

	private void RefreshPacketCustomFromSave()
	{
		if (!GodotObject.IsInstanceValid(config?.customData) || !GodotObject.IsInstanceValid(packet))
		{
			return;
		}
		Dictionary packetState = XWModPlayerProgressService.GetPacketState(packet.saveKey);
		Dictionary dictionary = (packetState.ContainsKey("Key") ? ((Dictionary)packetState["Key"]) : new Dictionary());
		if (!dictionary.ContainsKey("Custom"))
		{
			dictionary["Custom"] = "";
		}
		packetState["Key"] = dictionary;
		if ((string)dictionary["Custom"] != "")
		{
			string text = (string)dictionary["Custom"];
			if (config.customData.customDictionary.ContainsKey(text))
			{
				currentCustom = new Array<string> { text };
			}
			else
			{
				dictionary["Custom"] = "";
				packetState["Key"] = dictionary;
				if (XWModContentCatalog.GetContentOwner("Packet", packet.saveKey).Length > 0)
				{
					XWModPlayerProgressService.SetPacketState(packet.saveKey, packetState);
				}
				else
				{
					GameSaveManager.Instance.SetTowerDefensePacketValue(packet.saveKey, packetState);
				}
				currentCustom = new Array<string>();
			}
		}
		else
		{
			currentCustom = new Array<string>();
		}
		SetCustoms(currentCustom);
	}

	private void BindMainStateHandles()
	{
		if (_characterStateHandlesConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return;
		}
		_idleStateHandle = StateMachine?.GetStateById("character.idle");
		_sleepStateHandle = StateMachine?.GetStateById("character.sleep");
		_componentStateHandle = StateMachine?.GetStateById("character.component");
		if (_mainStatePhysicsFastCallbackTarget == null)
		{
			_mainStatePhysicsFastCallbackTarget = new MainStatePhysicsFastCallbackTarget(this);
		}
		if (_idleStateHandle != null)
		{
			_idleStateHandle.Entered += IdleEntered;
			_idleStateHandle.Exited += IdleExited;
			if (!_idleStateHandle.TrySetPhysicsFastCallback(_mainStatePhysicsFastCallbackTarget, 1))
			{
				_idleStateHandle.PhysicsProcessing += IdleProcessing;
			}
		}
		if (_sleepStateHandle != null)
		{
			_sleepStateHandle.Entered += SleepEntered;
			_sleepStateHandle.Exited += SleepExited;
			if (!_sleepStateHandle.TrySetPhysicsFastCallback(_mainStatePhysicsFastCallbackTarget, 2))
			{
				_sleepStateHandle.PhysicsProcessing += SleepProcessing;
			}
		}
		if (_componentStateHandle != null)
		{
			_componentStateHandle.Entered += ComponentEntered;
			_componentStateHandle.Exited += ComponentExited;
		}
		_characterStateHandlesConnected = true;
	}

	private void UnbindMainStateHandles()
	{
		if (!_characterStateHandlesConnected)
		{
			return;
		}
		if (_idleStateHandle != null)
		{
			_idleStateHandle.Entered -= IdleEntered;
			_idleStateHandle.Exited -= IdleExited;
			if (!_idleStateHandle.ClearPhysicsFastCallback(_mainStatePhysicsFastCallbackTarget))
			{
				_idleStateHandle.PhysicsProcessing -= IdleProcessing;
			}
		}
		if (_sleepStateHandle != null)
		{
			_sleepStateHandle.Entered -= SleepEntered;
			_sleepStateHandle.Exited -= SleepExited;
			if (!_sleepStateHandle.ClearPhysicsFastCallback(_mainStatePhysicsFastCallbackTarget))
			{
				_sleepStateHandle.PhysicsProcessing -= SleepProcessing;
			}
		}
		if (_componentStateHandle != null)
		{
			_componentStateHandle.Entered -= ComponentEntered;
			_componentStateHandle.Exited -= ComponentExited;
		}
		_idleStateHandle = null;
		_sleepStateHandle = null;
		_componentStateHandle = null;
		_characterStateHandlesConnected = false;
	}

	private void DisableInvalidRuntimeCharacter()
	{
		HasValidRuntimeConfiguration = false;
		inGame = false;
		SetPhysicsProcess(enable: false);
		SetProcess(enable: false);
		ProcessMode = ProcessModeEnum.Disabled;
		GD.PushError($"[TowerDefenseCharacter] Disabled invalid runtime character '{Name}': config or sprite is null/invalid.");
	}

	public override void _ExitTree()
	{
		SetNotifyTransform(enable: false);
		SetNotifyLocalTransform(enable: false);
		CancelBowlingImpactDisplacement();
		SuspendOwnerBatchDispatch();
		DisposeCharacterExternalVisuals();
		componentManager?.DetachOwner();
		base._ExitTree();
		if (!CachedEditorHint)
		{
			if (inGame && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.characterRegistry))
			{
				TowerDefenseManager.Instance.CharacterUnregister(this);
			}
			RemoveFromGroup("Character");
			UnsubscribeCharacterSkinSwitched();
		}
	}

	private void EnsureCharacterSkinSwitchedHandler()
	{
		if (_characterSkinSwitchedHandler == null)
		{
			_characterSkinSwitchedHandler = OnCharacterSkinSwitched;
		}
	}

	private void SubscribeCharacterSkinSwitched()
	{
		if (!_characterSkinSwitchedSubscribed || !GodotObject.IsInstanceValid(_characterSkinSwitchedEventBus))
		{
			_characterSkinSwitchedSubscribed = false;
			_characterSkinSwitchedEventBus = null;
			BattleEventBus battleEventBus = BattleEventBus.Instance;
			if (GodotObject.IsInstanceValid(battleEventBus))
			{
				EnsureCharacterSkinSwitchedHandler();
				battleEventBus.OnCharacterSkinSwitched += _characterSkinSwitchedHandler;
				_characterSkinSwitchedEventBus = battleEventBus;
				_characterSkinSwitchedSubscribed = true;
			}
		}
	}

	private void UnsubscribeCharacterSkinSwitched()
	{
		if (_characterSkinSwitchedSubscribed && GodotObject.IsInstanceValid(_characterSkinSwitchedEventBus) && _characterSkinSwitchedHandler != null)
		{
			_characterSkinSwitchedEventBus.OnCharacterSkinSwitched -= _characterSkinSwitchedHandler;
		}
		_characterSkinSwitchedSubscribed = false;
		_characterSkinSwitchedEventBus = null;
	}

	public override void _Notification(int what)
	{
		if ((long)what == 28 || (long)what == 29 || (long)what == 14 || (long)what == 15 || (long)what == 18 || (long)what == 19)
		{
			PrepareBatchRegistration();
		}
		if ((long)what == 20)
		{
			AdobeAnimateSprite.SealPackedSceneStaticArraysInSubtree(this);
		}
		if ((long)what == 18 || (long)what == 19)
		{
			AdobeAnimateSprite adobeAnimateSprite = _sprite;
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.InvalidateRuntimeRenderMountForAncestorChange();
			}
		}
		bool flag = (long)what == 35;
		bool flag2 = (long)what == 2000;
		bool flag3 = false;
		if (flag2 && !_writingCachedGlobalPosition && _pendingCachedGlobalTransformNotification)
		{
			_pendingCachedGlobalTransformNotification = false;
			flag3 = GlobalTransform.IsEqualApprox(_pendingCachedGlobalTransform);
		}
		if ((flag2 | flag) && !flag3)
		{
			InvalidateHitBoxBounds();
			if (!_writingCachedGlobalPosition)
			{
				_shadowGlobalTransformValid = false;
				_physicsLocalPositionValid = false;
				InvalidateGlobalPositionForPhysicsFrame();
				InvalidateComponentGameplayForPhysicsFrame();
				if (flag)
				{
					RefreshLocalScaleXSnapshot();
					NotifyRenderAncestorScaleChangedIfNeeded();
				}
			}
		}
		if ((long)what == 1)
		{
			componentManager?.Release();
			componentManager = null;
			ReleaseMainStateMachine();
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		componentManager?.DispatchRuntimeInput(inputEvent);
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		componentManager?.DispatchRuntimeUnhandledInput(inputEvent);
	}

	private void ReleaseMainStateMachine()
	{
		if (!_mainStateMachineReleased)
		{
			_mainStateMachineReleased = true;
			_mainStateMachineDispatchEnabled = false;
			UnbindMainStateHandles();
			StateMachineController mainStateMachineController = _mainStateMachineController;
			_mainStateMachineController = null;
			StateMachine = null;
			mainStateMachineController?.Dispose();
		}
	}

	public override void _Process(double delta)
	{
		if (!_ownerBatchRegistered)
		{
			BatchProcessUpdate(delta);
		}
	}

	public void BatchProcessUpdate(double delta)
	{
		ulong processFrames = Engine.GetProcessFrames();
		TickMainStateMachineProcess(delta, processFrames);
		TickComponentStateMachineProcess(delta, processFrames);
	}

	private void TickMainStateMachineProcess(double delta, ulong processFrame)
	{
		StateMachineController mainStateMachineController = _mainStateMachineController;
		if (CanDispatchMainStateMachine && mainStateMachineController != null && mainStateMachineController.HasProcessWork && _lastMainStateMachineProcessFrame != processFrame)
		{
			_lastMainStateMachineProcessFrame = processFrame;
			mainStateMachineController.TickProcess(delta);
		}
	}

	private void TickComponentStateMachineProcess(double delta, ulong processFrame)
	{
		ComponentManager componentManager = this.componentManager;
		if (CanDispatchOwnedStateMachines && componentManager != null && componentManager.HasStateMachineProcessWork && _lastComponentStateMachineProcessFrame != processFrame)
		{
			_lastComponentStateMachineProcessFrame = processFrame;
			componentManager.TickStateMachineProcess(delta);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_ownerBatchRegistered)
		{
			BatchUpdate(delta);
		}
	}

	public virtual void BatchUpdate(double delta)
	{
		if (!CachedEditorHint)
		{
			if (!TowerDefenseProcessModeDispatch.IsCharacterPhysicsBatchDispatchActive)
			{
				TowerDefenseProcessModeDispatch.BeginFrame();
			}
			PhysicsProcessWithFrame(delta, TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
		}
	}

	protected void PhysicsProcessWithFrame(double delta, ulong physicsFrame)
	{
		if (inGame && _lastMainStateMachinePhysicsFrame != physicsFrame)
		{
			PrepareCharacterPhysicsForTrace(physicsFrame, out var mainStateMachine, out var canDispatchMainStateMachine, out var manager, out var canDispatchOwnedStateMachines);
			PrepareCharacterTimeScaleForPhysics(delta);
			_componentGameplayPhysicsDispatchActive = true;
			try
			{
				TickMainStateMachinePhysics(delta, mainStateMachine, canDispatchMainStateMachine);
				RunComponentPhysicsForTrace(delta, physicsFrame, manager, canDispatchOwnedStateMachines);
			}
			finally
			{
				_componentGameplayPhysicsDispatchActive = false;
			}
			RunCharacterPhysicsPostForTrace(delta, physicsFrame);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrepareCharacterPhysicsForTrace(ulong physicsFrame, out StateMachineController mainStateMachine, out bool canDispatchMainStateMachine, out ComponentManager manager, out bool canDispatchOwnedStateMachines)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		_lastMainStateMachinePhysicsFrame = physicsFrame;
		RefreshComponentGameplayEntryExceptionForPhysicsFrame();
		mainStateMachine = _mainStateMachineController;
		int num;
		if (_mainStateMachineDispatchEnabled)
		{
			StateMachineController obj = mainStateMachine;
			if (obj != null && obj.IsInitialized && inGame)
			{
				num = ((!isDestroy) ? 1 : 0);
				goto IL_0048;
			}
		}
		num = 0;
		goto IL_0048;
		IL_0048:
		canDispatchMainStateMachine = (byte)num != 0;
		manager = componentManager;
		canDispatchOwnedStateMachines = CanDispatchOwnedStateMachines && manager != null;
		CacheComponentGameplayForPhysicsFrame(physicsFrame);
		TowerDefensePerfProfiler.End("batch.character.prepare", startTicks);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RunComponentPhysicsForTrace(double delta, ulong physicsFrame, ComponentManager manager, bool canDispatchOwnedStateMachines)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		if (canDispatchOwnedStateMachines)
		{
			bool hasStateMachinePhysicsWork = manager.HasStateMachinePhysicsWork;
			bool hasRuntimePhysicsWork = manager.HasRuntimePhysicsWork;
			bool hasComponentTimerWork = manager.HasComponentTimerWork;
			if (hasStateMachinePhysicsWork | hasRuntimePhysicsWork | hasComponentTimerWork)
			{
				if (_componentGameplayPhysicsSnapshotFrame != physicsFrame)
				{
					CacheComponentGameplayForPhysicsFrame(physicsFrame);
				}
				if (hasStateMachinePhysicsWork)
				{
					manager.TickStateMachinePhysics(delta, _componentGameplayPhysicsSnapshot);
					if (_componentGameplayPhysicsSnapshotFrame != physicsFrame)
					{
						CacheComponentGameplayForPhysicsFrame(physicsFrame);
					}
				}
				if (hasRuntimePhysicsWork)
				{
					manager.TickRuntimePhysics(delta, physicsFrame, _componentGameplayPhysicsSnapshot);
				}
				if (hasComponentTimerWork)
				{
					manager.TickComponentTimers(delta, _componentGameplayPhysicsSnapshot);
				}
			}
		}
		TowerDefensePerfProfiler.End("batch.character.componentWork", startTicks);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RunCharacterPhysicsPostForTrace(double delta, ulong physicsFrame)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		InvalidateGlobalPositionForPhysicsFrame();
		base._PhysicsProcess(delta);
		ReleaseZMotionLocalRenderWhenStable();
		BlowBackComponent blowBackComponent = this.blowBackComponent;
		if (blowBackComponent != null && !blowBackComponent.IsReleased && this.blowBackComponent.blowBack)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			if (logicalGlobalPosition.X <= (float)groundRight)
			{
				logicalGlobalPosition.X += (float)(this.blowBackComponent.blowBackNum * delta);
				SetGlobalPositionForPhysicsFrame(logicalGlobalPosition, physicsFrame);
			}
		}
		if ((ulong)((long)physicsFrame + (long)randFreshIndex) % 5uL == 0L && ShouldUpdateGridPos() && inGame)
		{
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition());
		}
		if (nearDie && !die && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost || !(this is TowerDefenseZombie)))
		{
			instance.DealHurt(config.hitpointsNearDeath * delta / 3.0, playSplatAudio: false);
		}
		TowerDefensePerfProfiler.End("batch.character.post", startTicks);
	}

	private void PrepareCharacterTimeScaleForPhysics(double delta)
	{
		timeScale = timeScaleInit;
		if (inGame && this is TowerDefenseZombie && (!GodotObject.IsInstanceValid(instance) || instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS))
		{
			double mapZombieColumnSpeedMultiplier = TowerDefenseManager.GetMapZombieColumnSpeedMultiplier(gridPos.X);
			if (mapZombieColumnSpeedMultiplier != 1.0)
			{
				timeScale *= mapZombieColumnSpeedMultiplier;
			}
		}
		BuffComponent buffComponent = buff;
		if (buffComponent != null && buffComponent.HasFrameUpdateWork)
		{
			buffComponent.BuffUpdate(delta);
		}
		else if (sprite.meshColor != Colors.White)
		{
			sprite.meshColor = Colors.White;
		}
		SynchronizeAnimationPlaybackBlock();
	}

	private void SynchronizeAnimationPlaybackBlock()
	{
		AdobeAnimateSprite adobeAnimateSprite = sprite;
		if (GodotObject.IsInstanceValid(adobeAnimateSprite))
		{
			adobeAnimateSprite.SetPlaybackBlocked(Math.Abs(timeScale) <= 1E-06);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TickMainStateMachinePhysics(double delta, StateMachineController controller, bool dispatchEnabled)
	{
		if (!dispatchEnabled || controller == null || !controller.HasPhysicsWork)
		{
			return;
		}
		if (TowerDefensePerfProfiler.Enabled && TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			long startTicks = TowerDefensePerfProfiler.BeginHotPath();
			if (!TryTickFlatMainStatePhysics(delta, controller))
			{
				controller.TickPhysicsDetailed(delta);
			}
			TowerDefensePerfProfiler.End("batch.character.mainState", startTicks);
		}
		else if (!TryTickFlatMainStatePhysics(delta, controller))
		{
			controller.TickPhysics(delta);
		}
	}

	private bool TryTickFlatMainStatePhysics(double delta, StateMachineController controller)
	{
		if (!controller.SupportsFlatDirectStateTransitions || !TryResolveMainRuntimeState(controller.CurrentStateHandle, out var state))
		{
			return false;
		}
		for (int i = 0; i < 3; i++)
		{
			int num = (int)state;
			switch (state)
			{
			case MainRuntimeState.Idle:
				IdleProcessing(delta);
				break;
			case MainRuntimeState.Sleep:
				SleepProcessing(delta);
				break;
			default:
				return true;
			}
			if (!TryResolveMainRuntimeState(controller.CurrentStateHandle, out var state2) || (int)state2 <= num)
			{
				return true;
			}
			state = state2;
		}
		return true;
	}

	private bool TryResolveMainRuntimeState(StateHandle activeState, out MainRuntimeState state)
	{
		if (activeState == _idleStateHandle)
		{
			state = MainRuntimeState.Idle;
			return true;
		}
		if (activeState == _sleepStateHandle)
		{
			state = MainRuntimeState.Sleep;
			return true;
		}
		if (activeState == _componentStateHandle)
		{
			state = MainRuntimeState.Component;
			return true;
		}
		state = MainRuntimeState.Idle;
		return false;
	}

	public void DamagePartInit()
	{
		List<string> list = _damagePartList ?? (_damagePartList = new List<string>());
		list.Clear();
		Dictionary dictionary = _damagePart ?? (_damagePart = new Dictionary());
		dictionary.Clear();
		if (sprite == null || config == null)
		{
			return;
		}
		if (config.damagePointData != null)
		{
			foreach (CharacterDamagePointConfig damagePoint in config.damagePointData.damagePointList)
			{
				if (damagePoint.isDrop)
				{
					string damagePointName = damagePoint.damagePointName;
					dictionary[damagePointName] = damagePoint;
					list.Add(damagePointName);
				}
			}
		}
		if (config.armorData == null)
		{
			return;
		}
		foreach (ArmorSlotConfig armor in config.armorData.armorList)
		{
			TowerDefenseArmorTypeData towerDefenseArmorTypeData = CharacterArmorData._LoadTypeDataFromJSON(armor.armorName);
			if (towerDefenseArmorTypeData == null || (towerDefenseArmorTypeData.armorMethodFlags & 0x80) != 0)
			{
				string armorName = armor.armorName;
				dictionary[armorName] = armor;
				list.Add(armorName);
			}
		}
	}

	public void PreviewDamagePoint(double persontage)
	{
		config.damagePointData.ClearDamagePointFliters(sprite);
		int num = 0;
		foreach (Variant key in config.damagePointData.damagePointDictionary.Keys)
		{
			CharacterDamagePointConfig characterDamagePointConfig = (CharacterDamagePointConfig)(GodotObject)((Dictionary)config.damagePointData.damagePointDictionary[key])["Config"];
			if (persontage <= characterDamagePointConfig.damagePersontage)
			{
				config.damagePointData.SetDamagePointFliters(sprite, characterDamagePointConfig.damagePointName);
				if (config.customData != null)
				{
					foreach (string item in currentCustom)
					{
						config.customData.SetDamagePoint(sprite, item, num);
					}
				}
			}
			num++;
		}
	}

	public void ClearArmor(string armor)
	{
		ArmorVisualComponent armorVisualComponent = this.armorVisualComponent;
		if (armorVisualComponent != null && !armorVisualComponent.IsReleased)
		{
			this.armorVisualComponent.ClearArmor(armor);
		}
		else
		{
			config.armorData.ClearArmorFliters(sprite, armor);
		}
	}

	public void ClearArmorAll()
	{
		ArmorVisualComponent armorVisualComponent = this.armorVisualComponent;
		if (armorVisualComponent != null && !armorVisualComponent.IsReleased)
		{
			this.armorVisualComponent.ClearArmorAll();
		}
		else
		{
			config.armorData.ClearArmorFlitersAll(sprite);
		}
	}

	public void SetArmor(string armor, int stage)
	{
		ArmorVisualComponent armorVisualComponent = this.armorVisualComponent;
		if (armorVisualComponent != null && !armorVisualComponent.IsReleased)
		{
			this.armorVisualComponent.SetArmor(armor, stage);
			return;
		}
		ClearArmor(armor);
		ArmorSlotConfig slotConfig = config.armorData.GetSlotConfig(armor);
		if (slotConfig == null || !(slotConfig.replaceMethod == "Sprite"))
		{
			config.armorData.OpenArmorFliters(sprite, armor);
			config.armorData.SetArmorReplace(sprite, armor, stage);
		}
	}

	public void SetArmors(Array<string> armorList)
	{
		ArmorVisualComponent armorVisualComponent = this.armorVisualComponent;
		if (armorVisualComponent != null && !armorVisualComponent.IsReleased)
		{
			this.armorVisualComponent.SetArmors(armorList);
			return;
		}
		ClearArmorAll();
		foreach (string armor in armorList)
		{
			if (armor != "")
			{
				SetArmor(armor, 0);
			}
		}
	}

	public void ClearCustom()
	{
		CustomVisualComponent customVisualComponent = this.customVisualComponent;
		if (customVisualComponent != null && customVisualComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.customVisualComponent.ClearCustom();
		}
		else
		{
			config.customData.ClearCustomFliters(sprite);
		}
	}

	public void SetCustom(string custom)
	{
		CustomVisualComponent customVisualComponent = this.customVisualComponent;
		if (customVisualComponent != null && customVisualComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.customVisualComponent.SetCustom(custom);
		}
		else
		{
			config.customData.SetCustomFliters(sprite, custom);
		}
	}

	public void SetCustoms(Array<string> customList)
	{
		CustomVisualComponent customVisualComponent = this.customVisualComponent;
		if (customVisualComponent != null && customVisualComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.customVisualComponent.SetCustoms(customList);
			return;
		}
		ClearCustom();
		foreach (string custom in customList)
		{
			if (custom != "")
			{
				SetCustom(custom);
			}
		}
	}

	public void OnCharacterSkinSwitched(string packetSaveKey, string customKey)
	{
		if (GodotObject.IsInstanceValid(packet) && !(packet.saveKey != packetSaveKey))
		{
			SwitchCustom(customKey);
		}
	}

	public void SwitchCustom(string customKey)
	{
		if (!GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(config.customData))
		{
			return;
		}
		if (customKey == "")
		{
			currentCustom = new Array<string>();
			OnCustomSwitched("");
		}
		else
		{
			if (!config.customData.customDictionary.ContainsKey(customKey))
			{
				return;
			}
			currentCustom = new Array<string> { customKey };
			if (GodotObject.IsInstanceValid(packet))
			{
				Dictionary packetState = XWModPlayerProgressService.GetPacketState(packet.saveKey);
				if (!packetState.ContainsKey("Key"))
				{
					packetState["Key"] = new Dictionary();
				}
				((Dictionary)packetState["Key"])["Custom"] = customKey;
				if (XWModContentCatalog.GetContentOwner("Packet", packet.saveKey).Length > 0)
				{
					XWModPlayerProgressService.SetPacketState(packet.saveKey, packetState);
				}
				else
				{
					GameSaveManager.Instance.SetTowerDefensePacketValue(packet.saveKey, packetState);
				}
			}
			OnCustomSwitched(customKey);
		}
	}

	public virtual void OnCustomSwitched(string customKey)
	{
	}

	public virtual void IdleEntered()
	{
		sprite.timeScale = timeScale;
		if (useIdleAnimeReset && idleAnimeClip != "")
		{
			sprite.SetAnimation(idleAnimeClip, loop: true, 0.2);
		}
	}

	public virtual void IdleProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (CanSleep())
		{
			Sleep();
		}
	}

	public virtual void IdleExited()
	{
	}

	public virtual void SleepEntered()
	{
		GetSleepRuntime()?.SleepEntered();
	}

	public virtual void SleepProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		GetSleepRuntime()?.SleepProcessing((float)delta);
	}

	public virtual void SleepExited()
	{
		GetSleepRuntime()?.SleepExited();
	}

	public virtual void ComponentEntered()
	{
		componentRunning = true;
	}

	public virtual void ComponentExited()
	{
		componentRunning = false;
	}

	public virtual void Idle()
	{
		SetMainRuntimeState(MainRuntimeState.Idle);
	}

	public virtual void Sleep()
	{
		if (!GodotObject.IsInstanceValid(instance) || !instance.hologram)
		{
			SetMainRuntimeState(MainRuntimeState.Sleep);
			if (!IsSleep())
			{
				GetSleepRuntime()?.SleepEnteredWithoutStateTransition();
			}
		}
	}

	public virtual void Component()
	{
		SetMainRuntimeState(MainRuntimeState.Component);
	}

	public bool IsDie()
	{
		return die;
	}

	public virtual bool CanSleep()
	{
		return GetSleepRuntime()?.CanSleep() ?? false;
	}

	public bool IsSleep()
	{
		return instance.sleep;
	}

	public bool IsIncapacitatedTarget()
	{
		if (!instance.sleep)
		{
			return buff?.BuffHas("MagicImmobilize") ?? false;
		}
		return true;
	}

	public double GetTotalHitPoint()
	{
		double num = config.hitpoints + config.hitpointsNearDeath;
		Array<TowerDefenseArmorInstance> armorList = instance.armorList;
		for (int i = 0; i < armorList.Count; i++)
		{
			num += armorList[i].damagePointBase;
		}
		return num * instance.hitpointScale;
	}

	public double GetCurrentHitPoint()
	{
		double num = instance.hitpoints;
		Array<TowerDefenseArmorInstance> armorList = instance.armorList;
		for (int i = 0; i < armorList.Count; i++)
		{
			num += armorList[i].hitPoints;
		}
		return num;
	}

	public void SetHitpointAndScale(double hitpointScale, Vector2 scale)
	{
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.hitpointScale = hitpointScale;
		}
		if (GodotObject.IsInstanceValid(transformPoint))
		{
			transformPoint.Scale = scale;
			componentManager?.GetRuntime<GroundMoveComponent>()?.RefreshDirectionCache();
		}
	}

	public void DamagePartCreate(StringName damagePointName, Node2D node, Vector2 velocity = default(Vector2), bool keepSlotScale = true, Vector2 offset = default(Vector2), bool fromSync = false, Vector2? synchronizedPosition = null, long synchronizedSequence = 0L)
	{
		GetDamagePartRuntime()?.DamagePartCreate(damagePointName, node, velocity, keepSlotScale, offset, fromSync, synchronizedPosition, synchronizedSequence);
	}

	public TowerDefenseMagnet MagnetCreate(TowerDefenseArmorInstance armorInstance, Node2D node)
	{
		return GetDamagePartRuntime()?.MagnetCreate(armorInstance, node);
	}

	public TowerDefenseMagnet ArmorDraw(TowerDefenseArmorInstance armor)
	{
		return GetDamagePartRuntime()?.ArmorDraw(armor);
	}

	public bool HasShield()
	{
		return instance.HasShieldArmor;
	}

	public bool HasHelm()
	{
		return instance.HasHelmetArmor;
	}

	public bool ProjectileEffectsBlocked(int damageFlags, int fireMethodFlags)
	{
		if (instance != null)
		{
			return instance.ProjectileEffectsBlockedByArmor(damageFlags, fireMethodFlags);
		}
		return false;
	}

	public bool GetHasArmor(string armorName)
	{
		foreach (TowerDefenseArmorInstance item in GetArmor())
		{
			if (item.slotConfig.armorName == armorName)
			{
				return true;
			}
		}
		return false;
	}

	public TowerDefenseArmorInstance GetArmorFromName(string armorName)
	{
		foreach (TowerDefenseArmorInstance item in GetArmor())
		{
			if (item.slotConfig.armorName == armorName)
			{
				return item;
			}
		}
		return null;
	}

	public Array<TowerDefenseArmorInstance> GetArmor()
	{
		return instance.armorList;
	}

	public Array<TowerDefenseArmorInstance> GetArmorShield()
	{
		return instance.armorShield;
	}

	public Array<TowerDefenseArmorInstance> GetArmorHelment()
	{
		return instance.armorHelm;
	}

	public Array<TowerDefenseArmorInstance> GetArmorHeadCover()
	{
		return instance.armorHeadCover;
	}

	public bool CanCollision(int maskFlags)
	{
		return (maskFlags & instance.collisionFlags) != 0;
	}

	public bool CanTarget(TowerDefenseCharacter character)
	{
		return CheckDifferentCamp(character.camp);
	}

	public bool CheckDifferentCamp(TowerDefenseEnum.CHARACTER_CAMP _camp)
	{
		return camp != _camp;
	}

	public bool CheckSameLine(int line)
	{
		return line == gridPos.Y;
	}

	public bool IsTargetableFromLine(int line, bool includeAllLineCheck = true)
	{
		if (line == gridPos.Y)
		{
			return true;
		}
		TargetRegistrationComponent targetRegistrationComponent = this.targetRegistrationComponent;
		if (targetRegistrationComponent == null || targetRegistrationComponent.IsReleased)
		{
			return false;
		}
		if (includeAllLineCheck && this.targetRegistrationComponent.allLineCheck)
		{
			return true;
		}
		if (TryGetValidTargetLineAlias(out var aliasLine))
		{
			return line == aliasLine;
		}
		return false;
	}

	public bool IsTargetableFromLineRange(int minLine, int maxLine, bool includeAllLineCheck = true)
	{
		if (gridPos.Y >= minLine && gridPos.Y <= maxLine)
		{
			return true;
		}
		TargetRegistrationComponent targetRegistrationComponent = this.targetRegistrationComponent;
		if (targetRegistrationComponent == null || targetRegistrationComponent.IsReleased)
		{
			return false;
		}
		if (includeAllLineCheck && this.targetRegistrationComponent.allLineCheck)
		{
			return true;
		}
		if (TryGetValidTargetLineAlias(out var aliasLine) && aliasLine >= minLine)
		{
			return aliasLine <= maxLine;
		}
		return false;
	}

	private bool TryGetValidTargetLineAlias(out int aliasLine)
	{
		int attackGridLineAliasOffset = targetRegistrationComponent.attackGridLineAliasOffset;
		aliasLine = gridPos.Y + attackGridLineAliasOffset;
		if (attackGridLineAliasOffset == 0 || aliasLine < 1)
		{
			return false;
		}
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(towerDefenseManager) && towerDefenseManager.gridNum.Y > 0 && aliasLine > towerDefenseManager.gridNum.Y)
		{
			return false;
		}
		return true;
	}

	public double GetGroundHeight(double posHeight)
	{
		return (double)GetLogicalGlobalPosition().Y - posHeight + groundHeight;
	}

	public void SetSpriteGroupShaderParameter(string property, Variant value)
	{
		ShaderEffectComponent shaderEffectComponent = this.shaderEffectComponent;
		if (shaderEffectComponent != null && !shaderEffectComponent.IsReleased)
		{
			this.shaderEffectComponent.SetSpriteGroupShaderParameter(property, value);
		}
	}

	public override void SetZ()
	{
		base.SetZ();
		shadowComponent?.MarkDirty();
		if (forceLocalRenderDuringZMotion && GodotObject.IsInstanceValid(sprite))
		{
			_zMotionStablePhysicsFrames = 0;
			if (!sprite.forceLocalRender)
			{
				sprite.forceLocalRender = true;
				_zMotionOwnsLocalRender = true;
			}
		}
		if (GodotObject.IsInstanceValid(spriteGroup))
		{
			ApplySpriteGroupZPosition();
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.NotifyAncestorTransformChangedForRender(ZChangedInsidePhysicsUpdate);
			}
		}
	}

	private void ApplySpriteGroupZPosition()
	{
		if (GodotObject.IsInstanceValid(spriteGroup))
		{
			if (!_spriteGroupLocalXCaptured)
			{
				_spriteGroupLocalX = spriteGroup.Position.X;
				_spriteGroupLocalXCaptured = true;
			}
			spriteGroup.Position = new Vector2(_spriteGroupLocalX, 0f - (float)z);
		}
	}

	private void ReleaseZMotionLocalRenderWhenStable()
	{
		if (!_zMotionOwnsLocalRender || !GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		if (!isGround || !Mathf.IsEqualApprox((float)z, (float)groundHeight))
		{
			_zMotionStablePhysicsFrames = 0;
			return;
		}
		_zMotionStablePhysicsFrames++;
		if (_zMotionStablePhysicsFrames >= 2)
		{
			sprite.forceLocalRender = false;
			_zMotionOwnsLocalRender = false;
			_zMotionStablePhysicsFrames = 0;
		}
	}

	public virtual void ShovelDestroy()
	{
		GetDestroyRuntime()?.ShovelDestroy();
	}

	public virtual void Destroy(bool freeInstance = true)
	{
		GetDestroyRuntime()?.Destroy(freeInstance);
	}

	public virtual void ClearFromMap()
	{
		Destroy();
	}

	public virtual void DestroyWithVisualDelay(double delaySeconds)
	{
		GetDestroyRuntime()?.DestroyWithVisualDelay(delaySeconds);
	}

	public virtual void AshDestroy()
	{
		GetDestroyRuntime()?.AshDestroy();
	}

	public virtual void SmashDestroy()
	{
		GetDestroyRuntime()?.SmashDestroy();
	}

	public virtual void DestroyReplace()
	{
	}

	public virtual async void DestroySet()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	public virtual void HitBoxDestroy()
	{
		GetDestroyRuntime()?.HitBoxDestroy();
	}

	public virtual double HurtWithAttackConfig(AttackConfig attackConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		return GetHurtRuntime()?.HurtWithAttackConfig(attackConfig, playSplatAudio, velocity, createDamagePart) ?? 0.0;
	}

	public virtual double Hurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, double damageLimit = -1.0)
	{
		return GetHurtRuntime()?.Hurt((float)num, playSplatAudio, velocity, createDamagePart, damageLimit) ?? 0.0;
	}

	public virtual double SkipInvincibleHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		return GetHurtRuntime()?.SkipInvincibleHurt((float)num, playSplatAudio, velocity, createDamagePart) ?? 0.0;
	}

	public virtual void Health(double num)
	{
		GetHurtRuntime()?.Health((float)num);
	}

	protected void RestoreFullHealthAfterRevive()
	{
		instance.hitpoints = instance.hitpointsSave;
		instance.die = false;
		instance.nearDie = false;
		instance.collisionFlags = config.collisionFlags;
		instance.maskFlags = config.maskFlags;
		die = false;
		nearDie = false;
		destroyComponent?.EndDeathSettlement();
		instance.RefreshDamagePoint();
		targetRegistrationComponent?.NotifyTargetStateChanged();
		ShowHealthComponent showHealthComponent = this.showHealthComponent;
		if (showHealthComponent != null && !showHealthComponent.IsReleased)
		{
			this.showHealthComponent.MarkDirty();
		}
	}

	public virtual double BowlingHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true)
	{
		return GetHurtRuntime()?.BowlingHurt((float)num, playSplatAudio, velocity, hitShield, createDamagePart) ?? 0.0;
	}

	public virtual double SmashHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return GetHurtRuntime()?.SmashHurt((float)num, playSplatAudio, velocity) ?? 0.0;
	}

	public virtual double ExplodeHurt(double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return GetHurtRuntime()?.ExplodeHurt((float)num, damageKind, playSplatAudio, velocity) ?? 0.0;
	}

	public virtual double FlagHurt(double num, int damageFlags, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return GetHurtRuntime()?.FlagHurt((float)num, damageFlags, playSplatAudio, velocity) ?? 0.0;
	}

	public virtual double ProjectileHurt(TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false)
	{
		return GetHurtRuntime()?.ProjectileHurt(projectile, projectileConfig, playSplatAudio, velocity, isRange) ?? 0.0;
	}

	public virtual double ProjectileHurt(in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false)
	{
		return GetHurtRuntime()?.ProjectileHurt(in info, projectileConfig, playSplatAudio, velocity, isRange) ?? 0.0;
	}

	public void Bright(double init = 0.5, double delay = 0.0, double rise = 0.5, double riseDuration = 0.0, double duration = 0.2)
	{
		HitFlashComponent hitFlashComponent = this.hitFlashComponent;
		if (hitFlashComponent != null && !hitFlashComponent.IsReleased)
		{
			this.hitFlashComponent.Bright((float)init, (float)delay, (float)rise, (float)riseDuration, (float)duration);
		}
	}

	public void White(double init = 1.0, double delay = 0.0, double duration = 0.5)
	{
		HitFlashComponent hitFlashComponent = this.hitFlashComponent;
		if (hitFlashComponent != null && !hitFlashComponent.IsReleased)
		{
			this.hitFlashComponent.White((float)init, (float)delay, (float)duration);
		}
	}

	public TowerDefenseInGamePacketShow SpawnPacket(TowerDefensePacketConfig packetConfig, Vector2 pos, double aliveTime, bool isFall, bool useCost = false, bool useRandf = true, Vector2? velocityOverride = null)
	{
		return GetResourceSpawnRuntime()?.SpawnPacket(packetConfig, pos, (float)aliveTime, isFall, useCost, useRandf, velocityOverride);
	}

	public void YBCreate(Vector2 pos, int num, Vector2 _velocity = default(Vector2), double _gravity = 980.0, bool _collect = false)
	{
		GetResourceSpawnRuntime()?.YBCreate(pos, num, _velocity, (float)_gravity, _collect);
	}

	public void CoinCreate(Vector2 pos, int num, Vector2 _velocity = default(Vector2), double _gravity = 980.0, bool _collect = false)
	{
		GetResourceSpawnRuntime()?.CoinCreate(pos, num, _velocity, (float)_gravity, _collect);
	}

	public void LuckyBagCreate(Vector2 pos, Vector2 _velocity = default(Vector2), double _gravity = 980.0)
	{
		GetResourceSpawnRuntime()?.LuckyBagCreate(pos, _velocity, (float)_gravity);
	}

	public TowerDefenseSunBase SunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2 _velocity = default(Vector2), double _gravity = 980.0, double _moveStopTime = -1.0)
	{
		return GetResourceSpawnRuntime()?.SunCreate(pos, sunNum, movingMethod, _velocity, (float)_gravity, (float)_moveStopTime);
	}

	public TowerDefenseSunBase BrainSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2 _velocity = default(Vector2), double _gravity = 980.0, double _moveStopTime = -1.0)
	{
		return GetResourceSpawnRuntime()?.BrainSunCreate(pos, sunNum, movingMethod, _velocity, (float)_gravity, (float)_moveStopTime);
	}

	public TowerDefenseSunBase ReplicatedEventSunCreate(Vector2 pos, long sunNum, bool createBrainSun, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2 velocity = default(Vector2), double gravity = 980.0, double moveStopTime = -1.0)
	{
		return GetResourceSpawnRuntime()?.ReplicatedEventSunCreate(pos, sunNum, createBrainSun, movingMethod, velocity, (float)gravity, (float)moveStopTime);
	}

	public TowerDefenseSunBase JalapenoSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2 _velocity = default(Vector2), double _gravity = 980.0, double _moveStopTime = -1.0)
	{
		return GetResourceSpawnRuntime()?.JalapenoSunCreate(pos, sunNum, movingMethod, _velocity, (float)_gravity, (float)_moveStopTime);
	}

	public TowerDefenseSunBase QXSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2 _velocity = default(Vector2), double _gravity = 980.0, double _moveStopTime = -1.0)
	{
		return GetResourceSpawnRuntime()?.QXSunCreate(pos, sunNum, movingMethod, _velocity, (float)_gravity, (float)_moveStopTime);
	}

	public TowerDefenseSunBase MagicSunCreate(Vector2 pos, long sunNum, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, Vector2 _velocity = default(Vector2), double _gravity = 980.0, double _moveStopTime = -1.0)
	{
		return GetResourceSpawnRuntime()?.MagicSunCreate(pos, sunNum, movingMethod, _velocity, (float)_gravity, (float)_moveStopTime);
	}

	public void ExplodeSunCreate(Vector2 pos, long sunNum, long sunOnce, TowerDefenseEnum.SUN_MOVING_METHOD movingMethod = TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, double _speed = 0.0, double _gravity = 0.0, double _moveStopTime = -1.0)
	{
		GetResourceSpawnRuntime()?.ExplodeSunCreate(pos, sunNum, sunOnce, movingMethod, (float)_speed, (float)_gravity, (float)_moveStopTime);
	}

	public void GoldShardCreate(Vector2 pos, Vector2 _velocity = default(Vector2), double _gravity = 980.0)
	{
		GetResourceSpawnRuntime()?.GoldShardCreate(pos, _velocity, (float)_gravity);
	}

	public void CraterCreate(bool nolimit = false, string craterName = "CraterDayGround", bool halfCrater = false)
	{
		if (this is TowerDefensePlant && nolimit)
		{
			Destroy();
		}
		if (!GodotObject.IsInstanceValid(cell) || (!nolimit && !cell.CanCraterCreate()))
		{
			return;
		}
		if (TowerDefenseManager.GetPacketConfig(craterName).Plant(gridPos, playAudio: false, noLimit: true) is TowerDefenseCrater towerDefenseCrater)
		{
			towerDefenseCrater.spawnHypnoses = GodotObject.IsInstanceValid(instance) && instance.hypnoses;
			if (halfCrater)
			{
				towerDefenseCrater.halfCrater = true;
			}
		}
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			MultiPlayerManager.Instance.SendCraterCreate(gridPos.X, gridPos.Y, craterName);
		}
	}

	public virtual void BlowBack(double num, double time = -1.0)
	{
		GetBlowBackRuntime()?.BlowBack(num, time);
	}

	public void ApplyBowlingImpactDisplacement(double distance, double duration = 0.4)
	{
		if (Math.Abs(distance) <= 1E-06)
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		float num = logicalGlobalPosition.X + (float)distance;
		if (duration <= 0.0 || !IsInsideTree())
		{
			logicalGlobalPosition.X = num;
			SetGlobalPositionForPhysicsFrame(logicalGlobalPosition, Engine.GetPhysicsFrames());
			CancelBowlingImpactDisplacement();
			return;
		}
		_bowlingImpactStartGlobalX = logicalGlobalPosition.X;
		_bowlingImpactTargetGlobalX = num;
		_bowlingImpactElapsed = 0.0;
		_bowlingImpactDuration = duration;
		_bowlingImpactDisplacementActive = true;
		if (!TowerDefenseCharacterMotionBatch.Register(this))
		{
			logicalGlobalPosition.X = num;
			SetGlobalPositionForPhysicsFrame(logicalGlobalPosition, Engine.GetPhysicsFrames());
			_bowlingImpactDisplacementActive = false;
			_bowlingImpactElapsed = 0.0;
			_bowlingImpactDuration = 0.0;
		}
	}

	internal bool UpdateBowlingImpactDisplacement(double delta, ulong physicsFrame)
	{
		if (!_bowlingImpactDisplacementActive)
		{
			return false;
		}
		if (!IsInsideTree() || _bowlingImpactDuration <= 0.0)
		{
			_bowlingImpactDisplacementActive = false;
			return false;
		}
		_bowlingImpactElapsed = Math.Min(_bowlingImpactDuration, _bowlingImpactElapsed + Math.Max(0.0, delta));
		double num = _bowlingImpactElapsed / _bowlingImpactDuration;
		double num2 = 1.0 - num;
		double num3 = 1.0 - num2 * num2 * num2 * num2 * num2;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		logicalGlobalPosition.X = Mathf.Lerp(_bowlingImpactStartGlobalX, _bowlingImpactTargetGlobalX, (float)num3);
		SetGlobalPositionForPhysicsFrame(logicalGlobalPosition, physicsFrame);
		if (num < 1.0)
		{
			return true;
		}
		_bowlingImpactDisplacementActive = false;
		_bowlingImpactElapsed = 0.0;
		_bowlingImpactDuration = 0.0;
		return false;
	}

	internal void CancelBowlingImpactDisplacement()
	{
		_bowlingImpactDisplacementActive = false;
		_bowlingImpactElapsed = 0.0;
		_bowlingImpactDuration = 0.0;
		TowerDefenseCharacterMotionBatch.Unregister(this);
	}

	protected internal virtual void OnHypnosisStateChanged()
	{
	}

	internal void SyncHologramHypnoses(bool hypnoses)
	{
		if (GodotObject.IsInstanceValid(instance) && instance.hologram && instance.hypnoses != hypnoses && hypnosesComponent != null && !hypnosesComponent.IsReleased)
		{
			hypnosesComponent.HypnosesInternal(-1f, canFliter: true, null);
		}
	}

	public virtual void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		if (!IsNodeReady())
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(this))
				{
					Hypnoses(time, canFliter, hypnosesConfig);
				}
			}).CallDeferred();
		}
		else
		{
			if (hypnosesComponent == null || hypnosesComponent.IsReleased)
			{
				return;
			}
			if (this is TowerDefensePlant && GodotObject.IsInstanceValid(cell) && GodotObject.IsInstanceValid(cell.itemShield))
			{
				TowerDefenseItemSheild itemShield = cell.itemShield;
				if (itemShield != this && itemShield.instance.hypnoses == instance.hypnoses && itemShield.ShieldBlockCharm())
				{
					return;
				}
			}
			hypnosesComponent.Hypnoses((float)time, canFliter, hypnosesConfig);
		}
	}

	public virtual void Rise(double duration = -1.0, double delay = 0.0, bool createDirt = true, bool changeState = true, double from = 150.0, bool emitRiseOver = true)
	{
		if (duration < 0.0)
		{
			duration = GD.RandRange(0.4, 0.6);
		}
		if (!IsNodeReady())
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(this))
				{
					Rise(duration, delay, createDirt, changeState, from, emitRiseOver);
				}
			}).CallDeferred();
			return;
		}
		RiseComponent riseRuntime = GetRiseRuntime();
		if (riseRuntime == null)
		{
			GD.PushError($"[TowerDefenseCharacter.Rise] Missing RiseComponent: {Name}");
		}
		else
		{
			riseRuntime.Rise((float)duration, (float)delay, createDirt, changeState, (float)from, emitRiseOver);
		}
	}

	public virtual void Recycle(double percentage = -1.0, bool destroyAfterRecycle = true)
	{
		GetRecycleRuntime()?.Recycle((float)percentage, destroyAfterRecycle);
	}

	internal void RefreshResourceComponentFacades()
	{
		_resourceComponentFacadesCurrent = false;
		ComponentManager componentManager = this.componentManager;
		if (!GodotObject.IsInstanceValid(componentManager))
		{
			return;
		}
		buff = null;
		this.showHealthComponent = null;
		this.shadowComponent = null;
		this.resourceSpawnComponent = null;
		this.customVisualComponent = null;
		this.targetRegistrationComponent = null;
		this.riseComponent = null;
		this.destroyComponent = null;
		this.sleepComponent = null;
		this.groundHeightComponent = null;
		this.damagePartComponent = null;
		this.hurtComponent = null;
		this.hypnosesComponent = null;
		this.armorVisualComponent = null;
		this.hitFlashComponent = null;
		this.shaderEffectComponent = null;
		this.blowBackComponent = null;
		this.recycleComponent = null;
		this.effectCreateComponent = null;
		this.phonkComponent = null;
		this.dismemberComponent = null;
		IReadOnlyList<CharacterComponentRuntime> resourceComponents = componentManager.ResourceComponents;
		for (int i = 0; i < resourceComponents.Count; i++)
		{
			CharacterComponentRuntime characterComponentRuntime = resourceComponents[i];
			if (characterComponentRuntime != null && !characterComponentRuntime.IsReleased)
			{
				if (buff == null && characterComponentRuntime is BuffComponent buffComponent)
				{
					buff = buffComponent;
				}
				if (this.showHealthComponent == null && characterComponentRuntime is ShowHealthComponent showHealthComponent)
				{
					this.showHealthComponent = showHealthComponent;
				}
				if (this.shadowComponent == null && characterComponentRuntime is ShadowComponent shadowComponent)
				{
					this.shadowComponent = shadowComponent;
				}
				if (this.resourceSpawnComponent == null && characterComponentRuntime is ResourceSpawnComponent resourceSpawnComponent)
				{
					this.resourceSpawnComponent = resourceSpawnComponent;
				}
				if (this.customVisualComponent == null && characterComponentRuntime is CustomVisualComponent customVisualComponent)
				{
					this.customVisualComponent = customVisualComponent;
				}
				if (this.targetRegistrationComponent == null && characterComponentRuntime is TargetRegistrationComponent targetRegistrationComponent)
				{
					this.targetRegistrationComponent = targetRegistrationComponent;
				}
				if (this.riseComponent == null && characterComponentRuntime is RiseComponent riseComponent)
				{
					this.riseComponent = riseComponent;
				}
				if (this.destroyComponent == null && characterComponentRuntime is DestroyComponent destroyComponent)
				{
					this.destroyComponent = destroyComponent;
				}
				if (this.sleepComponent == null && characterComponentRuntime is SleepComponent sleepComponent)
				{
					this.sleepComponent = sleepComponent;
				}
				if (this.groundHeightComponent == null && characterComponentRuntime is GroundHeightComponent groundHeightComponent)
				{
					this.groundHeightComponent = groundHeightComponent;
				}
				if (this.damagePartComponent == null && characterComponentRuntime is DamagePartComponent damagePartComponent)
				{
					this.damagePartComponent = damagePartComponent;
				}
				if (this.hurtComponent == null && characterComponentRuntime is HurtComponent hurtComponent)
				{
					this.hurtComponent = hurtComponent;
				}
				if (this.hypnosesComponent == null && characterComponentRuntime is HypnosesComponent hypnosesComponent)
				{
					this.hypnosesComponent = hypnosesComponent;
				}
				if (this.armorVisualComponent == null && characterComponentRuntime is ArmorVisualComponent armorVisualComponent)
				{
					this.armorVisualComponent = armorVisualComponent;
				}
				if (this.hitFlashComponent == null && characterComponentRuntime is HitFlashComponent hitFlashComponent)
				{
					this.hitFlashComponent = hitFlashComponent;
				}
				if (this.shaderEffectComponent == null && characterComponentRuntime is ShaderEffectComponent shaderEffectComponent)
				{
					this.shaderEffectComponent = shaderEffectComponent;
				}
				if (this.blowBackComponent == null && characterComponentRuntime is BlowBackComponent blowBackComponent)
				{
					this.blowBackComponent = blowBackComponent;
				}
				if (this.recycleComponent == null && characterComponentRuntime is RecycleComponent recycleComponent)
				{
					this.recycleComponent = recycleComponent;
				}
				if (this.effectCreateComponent == null && characterComponentRuntime is EffectCreateComponent effectCreateComponent)
				{
					this.effectCreateComponent = effectCreateComponent;
				}
				if (this.phonkComponent == null && characterComponentRuntime is PhonkComponent phonkComponent)
				{
					this.phonkComponent = phonkComponent;
				}
				if (this.dismemberComponent == null && characterComponentRuntime is DismemberComponent dismemberComponent)
				{
					this.dismemberComponent = dismemberComponent;
				}
			}
		}
		_resourceComponentFacadesCurrent = true;
	}

	private ResourceSpawnComponent GetResourceSpawnRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.resourceSpawnComponent != null && this.resourceSpawnComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		ResourceSpawnComponent resourceSpawnComponent = this.resourceSpawnComponent;
		if (resourceSpawnComponent == null || resourceSpawnComponent.IsReleased)
		{
			return null;
		}
		return this.resourceSpawnComponent;
	}

	protected SleepComponent GetSleepRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.sleepComponent != null && this.sleepComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		SleepComponent sleepComponent = this.sleepComponent;
		if (sleepComponent == null || sleepComponent.IsReleased)
		{
			return null;
		}
		return this.sleepComponent;
	}

	private DamagePartComponent GetDamagePartRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.damagePartComponent != null && this.damagePartComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		DamagePartComponent damagePartComponent = this.damagePartComponent;
		if (damagePartComponent == null || damagePartComponent.IsReleased)
		{
			return null;
		}
		return this.damagePartComponent;
	}

	private HurtComponent GetHurtRuntime()
	{
		HurtComponent hurtComponent = this.hurtComponent;
		if (hurtComponent != null && !hurtComponent.IsReleased)
		{
			return hurtComponent;
		}
		if (!_resourceComponentFacadesCurrent || hurtComponent != null)
		{
			RefreshResourceComponentFacades();
		}
		hurtComponent = this.hurtComponent;
		if (hurtComponent == null || hurtComponent.IsReleased)
		{
			return null;
		}
		return hurtComponent;
	}

	private RiseComponent GetRiseRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.riseComponent != null && this.riseComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		RiseComponent riseComponent = this.riseComponent;
		if (riseComponent == null || riseComponent.IsReleased)
		{
			return null;
		}
		return this.riseComponent;
	}

	private DestroyComponent GetDestroyRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.destroyComponent != null && this.destroyComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		DestroyComponent destroyComponent = this.destroyComponent;
		if (destroyComponent == null || destroyComponent.IsReleased)
		{
			return null;
		}
		return this.destroyComponent;
	}

	private BlowBackComponent GetBlowBackRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.blowBackComponent != null && this.blowBackComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		BlowBackComponent blowBackComponent = this.blowBackComponent;
		if (blowBackComponent == null || blowBackComponent.IsReleased)
		{
			return null;
		}
		return this.blowBackComponent;
	}

	private RecycleComponent GetRecycleRuntime()
	{
		if (!_resourceComponentFacadesCurrent || (this.recycleComponent != null && this.recycleComponent.IsReleased))
		{
			RefreshResourceComponentFacades();
		}
		RecycleComponent recycleComponent = this.recycleComponent;
		if (recycleComponent == null || recycleComponent.IsReleased)
		{
			return null;
		}
		return this.recycleComponent;
	}

	public TowerDefenseEffectParticlesOnce CreateDirt()
	{
		return effectCreateComponent?.CreateDirt();
	}

	public TowerDefenseEffectSpriteOnce CreateSplash()
	{
		return effectCreateComponent?.CreateSplash();
	}

	public TowerDefenseEffectParticlesOnce CreateIceTrap()
	{
		return effectCreateComponent?.CreateIceTrap();
	}

	public void WakeUp()
	{
		if (!IsNodeReady())
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(this))
				{
					WakeUp();
				}
			}).CallDeferred();
		}
		else
		{
			buff.DeleteBuff("Sleep");
			instance.wakeUp = true;
			GetSleepRuntime()?.ReevaluateEnvironmentState();
			OnWakeUp();
		}
	}

	protected virtual void OnWakeUp()
	{
	}

	public virtual void AnimeCompleted(string clip)
	{
	}

	public virtual void AnimeEvent(string command, Variant argument)
	{
	}

	public virtual void DamagePointReach(string damagePointName)
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && syncId >= 0)
		{
			MultiPlayerManager.Instance.SendDamagePointReach(syncId, damagePointName);
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && instance.damagePointData != null)
		{
			instance.damagePointData.SetDamagePointFliters(sprite, damagePointName);
		}
	}

	public virtual void ArmorDamagePointReach(string armorName, int stage)
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && syncId >= 0)
		{
			MultiPlayerManager.Instance.SendArmorDamagePointReach(syncId, armorName, stage);
		}
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefenseArmorInstance armorFromName = GetArmorFromName(armorName);
		if (armorFromName == null)
		{
			return;
		}
		armorFromName.stageIndex = stage;
		string replaceMethod = armorFromName.slotConfig.replaceMethod;
		if (!(replaceMethod == "Media"))
		{
			if (replaceMethod == "Sprite" && armorFromName.sprite != null && stage < armorFromName.typeData.stageAnimeTexturePaths.Count)
			{
				armorFromName.sprite.externalAtlasTexturePath = armorFromName.typeData.stageAnimeTexturePaths[stage];
			}
		}
		else
		{
			SetArmor(armorName, stage);
		}
	}

	public virtual void ArmorHitpointsEmpty(string armorName)
	{
		ShowHealthComponent showHealthComponent = this.showHealthComponent;
		if (showHealthComponent != null && !showHealthComponent.IsReleased)
		{
			this.showHealthComponent.MarkDirty();
		}
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && syncId >= 0)
		{
			MultiPlayerManager.Instance.SendArmorHitpointsEmpty(syncId, armorName);
		}
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefenseArmorInstance armorFromName = GetArmorFromName(armorName);
		if (armorFromName != null && !armorFromName.isRemove)
		{
			if ((armorFromName.armorMethodFlags & 0x80) != 0 && !armorFromName.damagePartDropped)
			{
				DamagePartCreate(new StringName(armorName), null, new Vector2(GD.RandRange(-100, 100), -300f), keepSlotScale: true, Vector2.Zero, fromSync: true, null, 0L);
			}
			armorFromName.RemoveArmor();
			armorFromName.isRemove = true;
		}
	}

	public virtual void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
	}

	public virtual void UnlimitedFireInit()
	{
	}

	internal void InheritCoverSleepState(TowerDefenseCharacter source)
	{
		if (GodotObject.IsInstanceValid(source) && GodotObject.IsInstanceValid(source.instance) && SupportsCoverSleepInheritance(source) && SupportsCoverSleepInheritance(this))
		{
			if (!GodotObject.IsInstanceValid(instance))
			{
				_pendingCoverSleepState = (source.instance.sleep, source.instance.wakeUp);
				return;
			}
			instance.wakeUp = source.instance.wakeUp;
			GetSleepRuntime()?.ReevaluateEnvironmentState();
		}
	}

	private static bool SupportsCoverSleepInheritance(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return false;
		}
		if (character.GetSleepRuntime() != null)
		{
			return true;
		}
		if (GodotObject.IsInstanceValid(character.config))
		{
			return !string.Equals(character.config.sleepTime, "Never", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public virtual void Cover(TowerDefenseCharacter character)
	{
	}

	public virtual void Spawn()
	{
	}

	public virtual void PreSpawn()
	{
	}

	private bool ShouldEnableGameplayDispatch()
	{
		if (inGame && !editorPreviewMode)
		{
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				return TowerDefenseManager.Instance.IsGameRunning();
			}
			return true;
		}
		return false;
	}

	public virtual void ActivateGameplayProcessing()
	{
		ProcessMode = ProcessModeEnum.Inherit;
		if (GodotObject.IsInstanceValid(sprite))
		{
			ActivateAdobeAnimateSpriteTree(sprite);
			if (_progressRestorePresentationRefreshPending)
			{
				sprite.InvalidateProgressRestorePresentation();
				_progressRestorePresentationRefreshPending = false;
			}
		}
		SetMainStateMachineDispatchEnabled(enabled: true);
		SetHitBoxEnabled(enabled: true);
		ConfigureCharacterBatchProcessing();
	}

	public virtual void ActivateLevelEntryPreview(string animationClip)
	{
		isShow = true;
		SetMainStateMachineDispatchEnabled(enabled: false);
		SuspendOwnerBatchDispatch();
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
		ProcessMode = ProcessModeEnum.Disabled;
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.RestoreRuntimeNativeCanvasLayer();
			sprite.ProcessMode = ProcessModeEnum.Always;
			sprite.pause = false;
			if (!string.IsNullOrWhiteSpace(animationClip))
			{
				sprite.SetAnimation(animationClip);
			}
			sprite.RefreshProcessScheduling();
		}
	}

	public void SetMainStateMachineDispatchEnabled(bool enabled)
	{
		_mainStateMachineDispatchEnabled = enabled && !_mainStateMachineReleased;
		RefreshComponentStateMachineDispatch();
	}

	internal void RefreshComponentStateMachineDispatch()
	{
		if (this is TowerDefenseZombie zombie)
		{
			TowerDefenseZombieBatch.UpdateProcessDispatch(zombie);
		}
		else
		{
			TowerDefenseCharacterBatch.UpdateProcessDispatch(this);
		}
		UpdateDirectMainStateMachineProcess();
	}

	internal void SetOwnerBatchRegistration(bool registered)
	{
		_ownerBatchRegistered = registered;
		UpdateDirectMainStateMachineProcess();
		SetPhysicsProcess(!registered && IsInsideTree() && inGame && !isDestroy);
	}

	private void UpdateDirectMainStateMachineProcess()
	{
		SetProcess(IsInsideTree() && !_ownerBatchRegistered && WantsMainStateMachineProcessDispatch);
	}

	private void ResumeOwnerBatchDispatch()
	{
		if (IsInsideTree() && inGame && !((!(this is TowerDefenseZombie zombie)) ? (UseCharacterBatch && TowerDefenseCharacterBatch.Register(this)) : (TowerDefenseZombie.UseBatch && TowerDefenseZombieBatch.Register(zombie))))
		{
			SetOwnerBatchRegistration(registered: false);
		}
	}

	private void SuspendOwnerBatchDispatch()
	{
		if (_ownerBatchRegistered)
		{
			if (this is TowerDefenseZombie zombie)
			{
				TowerDefenseZombieBatch.Unregister(zombie);
			}
			else
			{
				TowerDefenseCharacterBatch.Unregister(this);
			}
		}
		_ownerBatchRegistered = false;
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
	}

	private void ConfigureCharacterBatchProcessing()
	{
		if (!IsNodeReady() || CachedEditorHint || this is TowerDefenseZombie)
		{
			return;
		}
		if (!inGame)
		{
			SetPhysicsProcess(enable: false);
		}
		else if (UseCharacterBatch)
		{
			if (!TowerDefenseCharacterBatch.Register(this))
			{
				SetOwnerBatchRegistration(registered: false);
			}
		}
		else if (_ownerBatchRegistered)
		{
			TowerDefenseCharacterBatch.Unregister(this);
		}
		else
		{
			SetOwnerBatchRegistration(registered: false);
		}
	}

	private void EnterInitialMainStateMachine()
	{
		if (_pendingMainStateMachineSnapshot != null)
		{
			ApplyPendingMainStateMachineSnapshot();
		}
		else
		{
			StateMachine?.EnterInitialState();
		}
	}

	internal Dictionary CaptureMainStateMachineSnapshotData()
	{
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return new Dictionary();
		}
		return StateMachineSnapshotCodec.Encode(StateMachine.CaptureSnapshot());
	}

	internal bool RestoreMainStateMachineSnapshotData(Dictionary data, bool remote, bool suppressEntryEffects, bool progressCompatible = false)
	{
		StateMachineSnapshot stateMachineSnapshot = StateMachineSnapshotCodec.Decode(data);
		if (stateMachineSnapshot == null)
		{
			return false;
		}
		if (remote && stateMachineSnapshot.Revision < _lastRemoteMainStateMachineRevision)
		{
			return true;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			if (_pendingMainStateMachineSnapshot == null || !remote || !_pendingMainStateMachineSnapshotIsRemote || stateMachineSnapshot.Revision >= _pendingMainStateMachineSnapshot.Revision)
			{
				_pendingMainStateMachineSnapshot = stateMachineSnapshot;
				_pendingMainStateMachineSnapshotIsRemote = remote;
				_pendingMainStateMachineSnapshotSuppressEffects = suppressEntryEffects;
				_pendingMainStateMachineSnapshotProgressCompatible = progressCompatible;
			}
			return true;
		}
		if (!PrepareMainStateMachineSnapshot(stateMachineSnapshot, progressCompatible))
		{
			return false;
		}
		return ApplyMainStateMachineSnapshot(stateMachineSnapshot, remote, suppressEntryEffects);
	}

	internal bool RestoreLegacyMainState(string stateIdentity)
	{
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized || string.IsNullOrWhiteSpace(stateIdentity))
		{
			return false;
		}
		StateHandle stateHandle = StateMachine.ResolveState(stateIdentity, allowDisplayNameFallback: true);
		if (stateHandle == null || !stateHandle.IsValid)
		{
			return false;
		}
		StateMachineSnapshot stateMachineSnapshot = StateMachine.CaptureSnapshot();
		if (stateMachineSnapshot == null)
		{
			return false;
		}
		stateMachineSnapshot.ActiveStateIds.Clear();
		stateMachineSnapshot.ActiveStateIds.Add(stateHandle.StableId);
		stateMachineSnapshot.PendingTransitionIds.Clear();
		stateMachineSnapshot.PendingDelayRemaining.Clear();
		return StateMachine.RestoreSnapshot(stateMachineSnapshot);
	}

	internal void BeginAuthoritativeMainStateRestore()
	{
		_authoritativeMainStateRestoreDepth++;
	}

	internal void EndAuthoritativeMainStateRestore()
	{
		if (_authoritativeMainStateRestoreDepth > 0)
		{
			_authoritativeMainStateRestoreDepth--;
		}
	}

	private void ApplyPendingMainStateMachineSnapshot()
	{
		if (_pendingMainStateMachineSnapshot == null)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			StateMachineSnapshot pendingMainStateMachineSnapshot = _pendingMainStateMachineSnapshot;
			bool pendingMainStateMachineSnapshotIsRemote = _pendingMainStateMachineSnapshotIsRemote;
			bool pendingMainStateMachineSnapshotSuppressEffects = _pendingMainStateMachineSnapshotSuppressEffects;
			bool pendingMainStateMachineSnapshotProgressCompatible = _pendingMainStateMachineSnapshotProgressCompatible;
			_pendingMainStateMachineSnapshot = null;
			_pendingMainStateMachineSnapshotIsRemote = false;
			_pendingMainStateMachineSnapshotSuppressEffects = false;
			_pendingMainStateMachineSnapshotProgressCompatible = false;
			if (PrepareMainStateMachineSnapshot(pendingMainStateMachineSnapshot, pendingMainStateMachineSnapshotProgressCompatible))
			{
				ApplyMainStateMachineSnapshot(pendingMainStateMachineSnapshot, pendingMainStateMachineSnapshotIsRemote, pendingMainStateMachineSnapshotSuppressEffects);
			}
		}
	}

	private bool PrepareMainStateMachineSnapshot(StateMachineSnapshot snapshot, bool progressCompatible)
	{
		if (snapshot != null)
		{
			IStateMachineController stateMachine = StateMachine;
			if (stateMachine != null && stateMachine.IsInitialized)
			{
				if (progressCompatible)
				{
					StateMachineSnapshot stateMachineSnapshot = StateMachine.CaptureSnapshot();
					if (stateMachineSnapshot == null || !IsProgressStateMachineDefinitionIdCompatible(snapshot.DefinitionId, stateMachineSnapshot.DefinitionId) || snapshot.SchemaVersion != stateMachineSnapshot.SchemaVersion)
					{
						return false;
					}
					snapshot.DefinitionId = stateMachineSnapshot.DefinitionId;
					snapshot.ContentHash = stateMachineSnapshot.ContentHash;
				}
				return StateMachine.CanRestoreSnapshot(snapshot);
			}
		}
		return false;
	}

	protected virtual bool IsProgressStateMachineDefinitionIdCompatible(string savedDefinitionId, string currentDefinitionId)
	{
		return string.Equals(savedDefinitionId, currentDefinitionId, StringComparison.Ordinal);
	}

	private bool ApplyMainStateMachineSnapshot(StateMachineSnapshot snapshot, bool remote, bool suppressEntryEffects)
	{
		if (remote && snapshot.Revision < _lastRemoteMainStateMachineRevision)
		{
			return true;
		}
		if (remote)
		{
			BeginAuthoritativeMainStateRestore();
		}
		bool flag;
		try
		{
			flag = StateMachine.RestoreSnapshot(snapshot, suppressEntryEffects);
		}
		finally
		{
			if (remote)
			{
				EndAuthoritativeMainStateRestore();
			}
		}
		if (flag & remote)
		{
			_lastRemoteMainStateMachineRevision = Math.Max(_lastRemoteMainStateMachineRevision, snapshot.Revision);
		}
		if (flag)
		{
			OnAuthoritativeMainStateRestored(snapshot, remote);
		}
		return flag;
	}

	protected virtual void OnAuthoritativeMainStateRestored(StateMachineSnapshot snapshot, bool remote)
	{
	}

	private void ActivateAdobeAnimateSpriteTree(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.RestoreRuntimeNativeCanvasLayer();
			adobeAnimateSprite.ProcessMode = ProcessModeEnum.Inherit;
			if (!ShouldKeepAnimationPaused())
			{
				adobeAnimateSprite.pause = false;
			}
			adobeAnimateSprite.RefreshProcessScheduling();
		}
		int childCount = node.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			ActivateAdobeAnimateSpriteTree(node.GetChild(i));
		}
	}

	private bool ShouldKeepAnimationPaused()
	{
		if (this is TowerDefenseZombie towerDefenseZombie)
		{
			TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
			if (!towerDefenseZombie.isPause && !towerDefenseZombie.spritePause)
			{
				return towerDefenseManager?.pauseZombie ?? false;
			}
			return true;
		}
		return false;
	}

	public virtual void Blow()
	{
	}

	public virtual void Garlic()
	{
	}

	public virtual bool CanBlock()
	{
		return false;
	}

	public virtual bool ShouldUpdateGridPos()
	{
		return true;
	}

	public virtual void OnRiseStart()
	{
	}

	public virtual void OnRiseEnd()
	{
	}

	public virtual string BlockType()
	{
		return "General";
	}

	public virtual void Block(TowerDefenseCharacter target)
	{
	}

	public virtual bool CanDiggerBlock()
	{
		return false;
	}

	public virtual void BlockDigger(TowerDefenseCharacter target)
	{
	}

	public virtual void Purify()
	{
	}

	public virtual void SpawnZombie()
	{
	}

	public virtual void OnAnimeStarted(string _clipName)
	{
	}

	public virtual void SyncAnimation(string clipName, bool loopAnim, double blendTimeVal)
	{
		_syncApplyingAnimation = true;
		sprite.SetAnimation(clipName, loopAnim, (float)blendTimeVal);
		_syncApplyingAnimation = false;
	}

	public virtual void HitpointsNearDie()
	{
		destroyComponent?.BeginDeathSettlement();
		nearDie = true;
		OnCharacterNearDie?.Invoke(this);
	}

	public virtual void HitpointsEmpty()
	{
		die = true;
		destroyComponent?.BeginDeathSettlement();
	}

	public virtual void InWater()
	{
		ShadowComponent shadowComponent = this.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			this.shadowComponent.SetShadowVisible(visible: false);
		}
	}

	public virtual void OutWater()
	{
		ShadowComponent shadowComponent = this.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			this.shadowComponent.SetShadowVisible(!invisible);
		}
	}

	public static Variant GetFireAnime(string type)
	{
		return type switch
		{
			"Fire" => (GodotObject)FIRE, 
			"IceFire" => ICE_FIRE, 
			"MegaFire" => MEGA_FIRE, 
			"PurifyFire" => PURIFY_FIRE, 
			"WhiteFire" => WHITE_FIRE, 
			"GarlicFire" => GARLIC_FIRE, 
			_ => FIRE, 
		};
	}

	public static Dictionary CreateFireEventLists(double num, Array<TowerDefenseCharacterEventBase> _eventTarget, Array<TowerDefenseCharacterEventBase> _allEventList)
	{
		Array<TowerDefenseCharacterEventBase> array = new Array<TowerDefenseCharacterEventBase>(_eventTarget);
		TowerDefenseCharacterEventExplodeHurt towerDefenseCharacterEventExplodeHurt = new TowerDefenseCharacterEventExplodeHurt();
		towerDefenseCharacterEventExplodeHurt.num = num;
		towerDefenseCharacterEventExplodeHurt.type = "Jala";
		array.Insert(0, towerDefenseCharacterEventExplodeHurt);
		Array<TowerDefenseCharacterEventBase> array2 = new Array<TowerDefenseCharacterEventBase>(_allEventList);
		TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff = new TowerDefenseCharacterEventAddBuff();
		towerDefenseCharacterEventAddBuff.buffList.Insert(0, new TowerDefenseCharacterBuffFireHit());
		towerDefenseCharacterEventAddBuff.buffList.Insert(1, new TowerDefenseCharacterBuffJalaHit());
		array2.Add(towerDefenseCharacterEventAddBuff);
		return new Dictionary
		{
			["event_list"] = array,
			["all_event_list"] = array2
		};
	}

	private static void CreateFireEffectAtCell(Variant fireAnime, Vector2I cellPos, TowerDefenseControlNew battleControl, Node2D effectParent)
	{
		if (IsJalapenoBattleContextCurrent(battleControl, effectParent))
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(cellPos);
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce((PackedScene)(GodotObject)fireAnime, cellPos, "Flame|Done");
			towerDefenseEffectSpriteOnce.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(cellPos) + new Vector2(0f, 30f);
			if (GodotObject.IsInstanceValid(mapCell))
			{
				towerDefenseEffectSpriteOnce.GlobalPosition = new Vector2(towerDefenseEffectSpriteOnce.GlobalPosition.X, (float)((double)towerDefenseEffectSpriteOnce.GlobalPosition.Y - mapCell.GetGroundHeight()));
			}
			effectParent.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private static bool IsJalapenoBattleContextCurrent(TowerDefenseControlNew battleControl, Node2D effectParent)
	{
		if (GodotObject.IsInstanceValid(battleControl) && battleControl.IsInsideTree() && battleControl.isGameRunning && TowerDefenseManager.CurrentControl == battleControl && GodotObject.IsInstanceValid(effectParent) && effectParent.IsInsideTree())
		{
			return TowerDefenseGroundItemBase.characterNode == effectParent;
		}
		return false;
	}

	public static void PlayFireExplodeEffects()
	{
		ViewManager.Instance.CameraShake(new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)), 5.0, 0.05, 4);
		AudioManager.Instance.AudioPlay("ExplodeJalapeno");
	}

	public static Array<TowerDefenseCharacterEventBase> CreateSnowEventList(Array<TowerDefenseCharacterEventBase> _addEventList)
	{
		Array<TowerDefenseCharacterEventBase> array = new Array<TowerDefenseCharacterEventBase>(_addEventList);
		TowerDefenseCharacterEventForzen item = new TowerDefenseCharacterEventForzen
		{
			time = 3.0
		};
		array.Insert(0, new TowerDefenseCharacterEventHurt
		{
			collisionFlags = -1,
			num = 20.0
		});
		array.Insert(0, item);
		return array;
	}

	public static async Task CreateJalapenoFire(TowerDefenseEnum.CHARACTER_CAMP _camp, Vector2I _gridPos, double num = -1.0, Array<TowerDefenseCharacterEventBase> _eventTarget = null, Array<TowerDefenseCharacterEventBase> _allEventList = null, string type = "Fire", bool suppressDeathrattles = false)
	{
		TowerDefenseControlNew battleControl = TowerDefenseManager.CurrentControl;
		Node2D effectParent = TowerDefenseGroundItemBase.characterNode;
		if (!IsJalapenoBattleContextCurrent(battleControl, effectParent))
		{
			return;
		}
		if (_eventTarget == null)
		{
			_eventTarget = new Array<TowerDefenseCharacterEventBase>();
		}
		if (_allEventList == null)
		{
			_allEventList = new Array<TowerDefenseCharacterEventBase>();
		}
		Variant fireAnime = GetFireAnime(type);
		Dictionary dictionary = CreateFireEventLists(num, _eventTarget, _allEventList);
		Array<TowerDefenseCharacterEventBase> eventList = (Array<TowerDefenseCharacterEventBase>)dictionary["event_list"];
		Array<TowerDefenseCharacterEventBase> eventList2 = (Array<TowerDefenseCharacterEventBase>)dictionary["all_event_list"];
		BattleEventBus.Instance.EmitJalaLineEffectEmit(_gridPos.Y);
		TowerDefenseExplode.CreateExplodeLine(_gridPos.Y, eventList, new Array<TowerDefenseCharacter>(), _camp, -1, suppressDeathrattles);
		TowerDefenseExplode.CreateExplodeLine(_gridPos.Y, eventList2, new Array<TowerDefenseCharacter>(), TowerDefenseEnum.CHARACTER_CAMP.ALL, -1, suppressDeathrattles);
		Godot.Collections.Array mapIceCapList = TowerDefenseManager.Instance.GetMapIceCapList();
		if (_gridPos.Y >= 0 && _gridPos.Y < mapIceCapList.Count)
		{
			Node node = mapIceCapList[_gridPos.Y].AsGodotObject() as Node;
			if (GodotObject.IsInstanceValid(node))
			{
				node.QueueFree();
			}
		}
		PlayFireExplodeEffects();
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		for (int i = 0; i < mapGridNum.X; i++)
		{
			bool flag = true;
			if (i == 0)
			{
				CreateFireEffectAtCell(fireAnime, _gridPos, battleControl, effectParent);
				flag = false;
			}
			else
			{
				if (_gridPos.X - i > 0)
				{
					CreateFireEffectAtCell(fireAnime, _gridPos - new Vector2I(i, 0), battleControl, effectParent);
					flag = false;
				}
				if (_gridPos.X + i <= mapGridNum.X)
				{
					CreateFireEffectAtCell(fireAnime, _gridPos + new Vector2I(i, 0), battleControl, effectParent);
					flag = false;
				}
			}
			if (!flag)
			{
				SceneTreeTimer source = battleControl.GetTree().CreateTimer(0.025, processAlways: false);
				await battleControl.ToSignal(source, SceneTreeTimer.SignalName.Timeout);
				if (!IsJalapenoBattleContextCurrent(battleControl, effectParent))
				{
					break;
				}
				continue;
			}
			break;
		}
	}

	public static async Task CreateJalapenoFireColumn(TowerDefenseEnum.CHARACTER_CAMP _camp, Vector2I _gridPos, double num = -1.0, Array<TowerDefenseCharacterEventBase> _eventTarget = null, Array<TowerDefenseCharacterEventBase> _allEventList = null, string type = "Fire", bool suppressDeathrattles = false)
	{
		TowerDefenseControlNew battleControl = TowerDefenseManager.CurrentControl;
		Node2D effectParent = TowerDefenseGroundItemBase.characterNode;
		if (!IsJalapenoBattleContextCurrent(battleControl, effectParent))
		{
			return;
		}
		if (_eventTarget == null)
		{
			_eventTarget = new Array<TowerDefenseCharacterEventBase>();
		}
		if (_allEventList == null)
		{
			_allEventList = new Array<TowerDefenseCharacterEventBase>();
		}
		Variant fireAnime = GetFireAnime(type);
		Dictionary dictionary = CreateFireEventLists(num, _eventTarget, _allEventList);
		Array<TowerDefenseCharacterEventBase> eventList = (Array<TowerDefenseCharacterEventBase>)dictionary["event_list"];
		Array<TowerDefenseCharacterEventBase> eventList2 = (Array<TowerDefenseCharacterEventBase>)dictionary["all_event_list"];
		BattleEventBus.Instance.EmitJalaRowEffectEmit(_gridPos.X);
		TowerDefenseExplode.CreateExplodeColumn(_gridPos.X, eventList, new Array<TowerDefenseCharacter>(), _camp, -1, suppressDeathrattles);
		TowerDefenseExplode.CreateExplodeColumn(_gridPos.X, eventList2, new Array<TowerDefenseCharacter>(), TowerDefenseEnum.CHARACTER_CAMP.ALL, -1, suppressDeathrattles);
		PlayFireExplodeEffects();
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		for (int i = 0; i < mapGridNum.Y; i++)
		{
			bool flag = true;
			if (i == 0)
			{
				CreateFireEffectAtCell(fireAnime, _gridPos, battleControl, effectParent);
				flag = false;
			}
			else
			{
				if (_gridPos.Y - i > 0)
				{
					CreateFireEffectAtCell(fireAnime, _gridPos - new Vector2I(0, i), battleControl, effectParent);
					flag = false;
				}
				if (_gridPos.Y + i <= mapGridNum.Y)
				{
					CreateFireEffectAtCell(fireAnime, _gridPos + new Vector2I(0, i), battleControl, effectParent);
					flag = false;
				}
			}
			if (!flag)
			{
				SceneTreeTimer source = battleControl.GetTree().CreateTimer(0.025, processAlways: false);
				await battleControl.ToSignal(source, SceneTreeTimer.SignalName.Timeout);
				if (!IsJalapenoBattleContextCurrent(battleControl, effectParent))
				{
					break;
				}
				continue;
			}
			break;
		}
	}

	public static async Task CreateJalapenoFireSlash(TowerDefenseEnum.CHARACTER_CAMP _camp, Vector2I _gridPos, double num = -1.0, Array<TowerDefenseCharacterEventBase> _eventTarget = null, Array<TowerDefenseCharacterEventBase> _allEventList = null, string type = "Fire", bool suppressDeathrattles = false)
	{
		TowerDefenseControlNew battleControl = TowerDefenseManager.CurrentControl;
		Node2D effectParent = TowerDefenseGroundItemBase.characterNode;
		BattleEventBus battleEventBus = BattleEventBus.Instance;
		if (!IsJalapenoBattleContextCurrent(battleControl, effectParent) || !GodotObject.IsInstanceValid(battleEventBus))
		{
			return;
		}
		if (_eventTarget == null)
		{
			_eventTarget = new Array<TowerDefenseCharacterEventBase>();
		}
		if (_allEventList == null)
		{
			_allEventList = new Array<TowerDefenseCharacterEventBase>();
		}
		Variant fireAnime = GetFireAnime(type);
		Dictionary dictionary = CreateFireEventLists(num, _eventTarget, _allEventList);
		Array<TowerDefenseCharacterEventBase> eventList = (Array<TowerDefenseCharacterEventBase>)dictionary["event_list"];
		Array<TowerDefenseCharacterEventBase> allEventList = (Array<TowerDefenseCharacterEventBase>)dictionary["all_event_list"];
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		List<Vector2I> list = new List<Vector2I>();
		int num2 = Math.Max(mapGridNum.X, mapGridNum.Y);
		for (int i = 0; i <= num2; i++)
		{
			foreach (Vector2I item in new List<Vector2I>
			{
				_gridPos + new Vector2I(i, i),
				_gridPos + new Vector2I(i, -i),
				_gridPos + new Vector2I(-i, i),
				_gridPos + new Vector2I(-i, -i)
			})
			{
				if (item.X >= 1 && item.X <= mapGridNum.X && item.Y >= 1 && item.Y <= mapGridNum.Y)
				{
					if (i == 0)
					{
						list.Add(item);
						break;
					}
					list.Add(item);
				}
			}
		}
		PlayFireExplodeEffects();
		foreach (Vector2I item2 in list)
		{
			battleEventBus.EmitJalaGridEffectEmit(item2);
			TowerDefenseExplode.CreateExplode(TowerDefenseManager.GetMapCellPlantPos(item2), new Vector2(0.5f, 0.5f), eventList, new Array<TowerDefenseCharacter>(), _camp, -1, suppressDeathrattles);
			TowerDefenseExplode.CreateExplode(TowerDefenseManager.GetMapCellPlantPos(item2), new Vector2(0.5f, 0.5f), allEventList, new Array<TowerDefenseCharacter>(), TowerDefenseEnum.CHARACTER_CAMP.ALL, -1, suppressDeathrattles);
			CreateFireEffectAtCell(fireAnime, item2, battleControl, effectParent);
			SceneTreeTimer source = battleControl.GetTree().CreateTimer(0.025, processAlways: false);
			await battleControl.ToSignal(source, SceneTreeTimer.SignalName.Timeout);
			if (!IsJalapenoBattleContextCurrent(battleControl, effectParent) || !GodotObject.IsInstanceValid(battleEventBus))
			{
				return;
			}
		}
	}

	public static void CreateColdVisualEffect(Vector2I gridPos)
	{
		ViewManager.Instance.FullScreenColorBlink(new Color(0.117647f, 0.564706f, 1f, 0.5f));
		AudioManager.Instance.AudioPlay("Frozen");
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(SNOW_FLAKES, gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos);
		TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
	}

	public static void CreateColdEffect(TowerDefenseEnum.CHARACTER_CAMP _camp, Vector2I _gridPos, Array<TowerDefenseCharacterEventBase> _addEventList = null)
	{
		if (_addEventList == null)
		{
			_addEventList = new Array<TowerDefenseCharacterEventBase>();
		}
		BattleEventBus.Instance.EmitColdEffectEmit();
		CreateColdVisualEffect(_gridPos);
		Array<TowerDefenseCharacterEventBase> array = CreateSnowEventList(_addEventList);
		foreach (Variant item in TowerDefenseManager.Instance.GetCampTarget(_camp))
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			foreach (TowerDefenseCharacterEventBase item2 in array)
			{
				item2.Execute(towerDefenseCharacter.GetLogicalGlobalPosition(), towerDefenseCharacter);
			}
		}
	}

	public static void CreateColdEffectRange(TowerDefenseEnum.CHARACTER_CAMP _camp, Vector2I _gridPos, Vector2 _range, Array<TowerDefenseCharacterEventBase> _addEventList = null)
	{
		if (_addEventList == null)
		{
			_addEventList = new Array<TowerDefenseCharacterEventBase>();
		}
		ViewManager.Instance.FullScreenColorBlink(new Color(0.117647f, 0.564706f, 1f, 0.5f));
		AudioManager.Instance.AudioPlay("Frozen");
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(SNOW_FLAKES, _gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(_gridPos);
		TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		Array<TowerDefenseCharacterEventBase> eventList = CreateSnowEventList(_addEventList);
		TowerDefenseExplode.CreateExplode(towerDefenseEffectParticlesOnce.GlobalPosition, _range, eventList, new Array<TowerDefenseCharacter>(), _camp, -1);
	}

	public static TowerDefenseCharacter CreateCharacter(string packetName, Vector2 pos, Vector2I _gridPos, double _groundHeight)
	{
		return CreateCharacterCore(default, packetName, pos, _gridPos, _groundHeight);
	}

	public static TowerDefenseCharacter CreateCharacter(EconomyAccountId economyOwnerAccountId, string packetName, Vector2 pos, Vector2I gridPos, double groundHeight)
	{
		if (!economyOwnerAccountId.IsValid)
		{
			return null;
		}
		return CreateCharacterCore(economyOwnerAccountId, packetName, pos, gridPos, groundHeight);
	}

	private static TowerDefenseCharacter CreateCharacterCore(EconomyAccountId economyOwnerAccountId, string packetName, Vector2 pos, Vector2I gridPos, double groundHeight)
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(TowerDefenseGroundItemBase.characterNode))
		{
			return null;
		}
		TowerDefenseCharacter towerDefenseCharacter = (economyOwnerAccountId.IsValid ? packetConfig.Create(economyOwnerAccountId, pos, gridPos, groundHeight) : packetConfig.Create(pos, gridPos, groundHeight));
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return null;
		}
		towerDefenseCharacter.groundHeight = groundHeight;
		TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
		return towerDefenseCharacter;
	}

	public virtual bool TryConsumeIncomingBuff(TowerDefenseCharacterBuffConfig buffConfig)
	{
		return false;
	}

	public void BuffAdd(TowerDefenseCharacterBuffConfig buffConfig)
	{
		buff.AddBuff(buffConfig);
	}

	public void BuffDelete(string key)
	{
		buff.DeleteBuff(key);
	}

	public TowerDefenseCharacterBuffConfig BuffGet(string key)
	{
		return buff.BuffGet(key);
	}

	public virtual Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary();
	}

	public virtual void ImportNetworkSpecialState(Dictionary _data)
	{
	}

	public virtual Dictionary ExportNetworkSpawnState()
	{
		return new Dictionary();
	}

	public virtual int GetNetworkSpecialStateRevision()
	{
		return 0;
	}

	public virtual bool IsNetworkSpecialMovementActive()
	{
		return false;
	}

	public virtual void OnRemoteNetworkAnimationStart(string _clipName)
	{
	}

	public void RestoreFromSave(TowerDefenseCharacterSaveConfigCSharp saveConfig)
	{
		instance.ImportSave(saveConfig.instanceSave);
		nearDie = instance.nearDie;
		die = instance.die;
		timeScaleInit = saveConfig.timeScaleInit;
		timeScale = saveConfig.timeScale;
		timeScaleSave = saveConfig.timeScaleSave;
		if (instance.nearDie)
		{
			ShowHealthComponent showHealthComponent = this.showHealthComponent;
			if (showHealthComponent != null && !showHealthComponent.IsReleased)
			{
				this.showHealthComponent.SetAlive(alive: true);
			}
		}
		buff?.ImportSave(saveConfig.buffSave);
		if (instance.damagePointIndex <= 0 || instance.damagePointData == null)
		{
			return;
		}
		for (int i = 0; i < instance.damagePointIndex; i++)
		{
			if (i >= instance.damagePoints.Count)
			{
				continue;
			}
			string damagePointName = (string)instance.damagePoints[i]["Name"];
			instance.damagePointData.SetDamagePointFliters(sprite, damagePointName);
			if (config.customData == null)
			{
				continue;
			}
			foreach (string item in currentCustom)
			{
				config.customData.SetDamagePoint(sprite, item, i);
			}
		}
	}

	public virtual Dictionary ExportVariantSave()
	{
		return new Dictionary();
	}

	public virtual void ImportVariantSave(Dictionary _data)
	{
	}

	public virtual bool ImportVariantSaveWhenEmpty()
	{
		return false;
	}

	public virtual void PrepareForProgressRestore()
	{
		IsProgressRestoreInFlight = true;
		ProcessMode = ProcessModeEnum.Disabled;
	}

	public virtual void FinalizeProgressRestore()
	{
		if (die || nearDie)
		{
			destroyComponent?.BeginDeathSettlement();
		}
		else
		{
			destroyComponent?.EndDeathSettlement();
		}
		IsProgressRestoreInFlight = false;
		_progressRestorePresentationRefreshPending = true;
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.InvalidateProgressRestorePresentation();
		}
		GetSleepRuntime()?.ReevaluateEnvironmentState();
	}

	public TowerDefenseCharacter TransformTo(string targetName, Action<TowerDefenseCharacter> onTargetReady = null, PackedScene effectScene = null, bool syncCamp = true, bool syncBodyHp = true, bool syncHypnoses = true, bool removeSource = true)
	{
		if (!GodotObject.IsInstanceValid(this))
		{
			return null;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(targetName);
		if (packetConfig == null)
		{
			return null;
		}
		Vector2I vector2I = gridPos;
		Vector2 vector = ((this is TowerDefenseZombie) ? GetLogicalGlobalPosition() : TowerDefenseManager.GetMapCellPlantPos(vector2I));
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		double num = ((instance != null) ? instance.hitpoints : 0.0);
		bool flag = instance != null && instance.hypnoses;
		TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = camp;
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(vector2I, playAudio: false, noLimit: true, default, skipPlacementCheck: true);
		if (towerDefenseCharacter == null || !GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return null;
		}
		if (this is TowerDefenseZombie && towerDefenseCharacter is TowerDefenseZombie)
		{
			towerDefenseCharacter.SetLogicalGlobalPosition(node2D.ToLocal(vector));
			towerDefenseCharacter.groundHeight = groundHeight;
			towerDefenseCharacter.z = groundHeight;
			towerDefenseCharacter.Scale = new Vector2((Scale.X < 0f) ? (0f - Mathf.Abs(towerDefenseCharacter.Scale.X)) : Mathf.Abs(towerDefenseCharacter.Scale.X), towerDefenseCharacter.Scale.Y);
		}
		if (syncCamp)
		{
			towerDefenseCharacter.camp = cHARACTER_CAMP;
		}
		if (syncBodyHp && GodotObject.IsInstanceValid(towerDefenseCharacter.instance))
		{
			towerDefenseCharacter.instance.hitpointsSave = num;
			towerDefenseCharacter.instance.hitpoints = num;
		}
		if (syncHypnoses & flag)
		{
			TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP2 = cHARACTER_CAMP;
			if (buff?.BuffGet("Hypnoses") is TowerDefenseCharacterBuffHypnoses { saveCamp: not TowerDefenseEnum.CHARACTER_CAMP.NOONE } towerDefenseCharacterBuffHypnoses)
			{
				cHARACTER_CAMP2 = towerDefenseCharacterBuffHypnoses.saveCamp;
			}
			else
			{
				switch (cHARACTER_CAMP)
				{
				case TowerDefenseEnum.CHARACTER_CAMP.PLANT:
					cHARACTER_CAMP2 = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
					break;
				case TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE:
					cHARACTER_CAMP2 = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
					break;
				}
			}
			if (syncCamp)
			{
				towerDefenseCharacter.camp = cHARACTER_CAMP2;
			}
			towerDefenseCharacter.Hypnoses();
		}
		if (effectScene == null)
		{
			effectScene = DefaultTransformEffect;
		}
		if (effectScene != null)
		{
			SpawnTransformEffect(effectScene, vector2I, vector);
		}
		TransformWatcher transformWatcher = new TransformWatcher(this, towerDefenseCharacter, num, syncBodyHp, removeSource, onTargetReady);
		if (GodotObject.IsInstanceValid(node2D))
		{
			node2D.AddChild(transformWatcher, forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			GetTree().Root.CallDeferred("add_child", transformWatcher);
		}
		return towerDefenseCharacter;
	}

	private void SilentlyRemove()
	{
		if (GodotObject.IsInstanceValid(this))
		{
			skipDestroySet = true;
			if (GodotObject.IsInstanceValid(cell))
			{
				cell.RemoveCharacter(this);
			}
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				TowerDefenseManager.Instance.CharacterUnregister(this);
			}
			RemoveFromGroup("Character");
			if (!IsQueuedForDeletion())
			{
				QueueFree();
			}
		}
	}

	internal void SilentlyRemoveForTransformation()
	{
		SilentlyRemove();
	}

	private static void SpawnTransformEffect(PackedScene effectScene, Vector2I gridPos, Vector2 globalPosition)
	{
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(node2D) && effectScene != null)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(effectScene, gridPos, "Idle");
			if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
			{
				node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
				towerDefenseEffectSpriteOnce.GlobalPosition = globalPosition;
			}
		}
	}

	private void InitializeCharacterExternalVisuals()
	{
		if (!_characterExternalVisualsInitialized && GodotObject.IsInstanceValid(sprite))
		{
			_characterExternalVisualsInitialized = true;
			RegisterLegacySpriteGroupVisualsOnce();
		}
	}

	private void DisposeCharacterExternalVisuals()
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			for (int i = 0; i < _legacySpriteGroupVisualHandles.Count; i++)
			{
				sprite.UnregisterExternalVisual(_legacySpriteGroupVisualHandles[i]);
			}
		}
		_legacySpriteGroupVisualHandles.Clear();
		_characterExternalVisualsInitialized = false;
		_legacySpriteGroupVisualsScanned = false;
	}

	public void AttachAnimatedStatusVisual(AdobeAnimateSprite child, AdobeAnimateSlot preferredSlot, Transform2D targetGlobalTransform)
	{
		if (GodotObject.IsInstanceValid(child) && GodotObject.IsInstanceValid(sprite))
		{
			bool flag = GodotObject.IsInstanceValid(preferredSlot);
			Node node = (flag ? ((Node2D)preferredSlot) : ((Node2D)sprite));
			if (!flag)
			{
				child.usePos = false;
				child.useRotate = false;
			}
			Node parent = child.GetParent();
			bool flag2 = parent != node;
			if (GodotObject.IsInstanceValid(parent) && parent != node)
			{
				parent.RemoveChild(child);
			}
			if (child.GetParent() != node)
			{
				node.AddChild(child, forceReadableName: false, InternalMode.Disabled);
			}
			child.GlobalTransform = targetGlobalTransform;
			if (flag2)
			{
				CharacterAnimatedStatusVisualCount++;
				CharacterAnimatedStatusTopologyChangeCount++;
				sprite.RefreshManagedSlotSpriteCacheForRender();
			}
		}
	}

	public void DetachAnimatedStatusVisual(AdobeAnimateSprite child)
	{
		if (!GodotObject.IsInstanceValid(child))
		{
			return;
		}
		Node parent = child.GetParent();
		if (GodotObject.IsInstanceValid(parent))
		{
			parent.RemoveChild(child);
			CharacterAnimatedStatusVisualCount = Mathf.Max(0, CharacterAnimatedStatusVisualCount - 1);
			CharacterAnimatedStatusTopologyChangeCount++;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.RefreshManagedSlotSpriteCacheForRender();
			}
		}
	}

	public void RegisterLegacySpriteGroupVisualsOnce()
	{
		if (_legacySpriteGroupVisualsScanned || !GodotObject.IsInstanceValid(spriteGroup))
		{
			return;
		}
		_legacySpriteGroupVisualsScanned = true;
		CharacterExternalVisualCompatibilityScanCount++;
		foreach (Node child in spriteGroup.GetChildren())
		{
			if (child is Sprite2D sprite2D && sprite2D != icetrapSprite && sprite2D.Texture != null && sprite2D.Material == null && sprite2D.GetScript().VariantType == Variant.Type.Nil)
			{
				AdobeAnimateExternalVisualHandle item = RegisterCharacterExternalVisual(sprite2D, new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Root, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation));
				if (item.IsValid)
				{
					_legacySpriteGroupVisualHandles.Add(item);
				}
			}
		}
	}

	public AdobeAnimateExternalVisualHandle RegisterCharacterExternalVisual(Sprite2D visual, AdobeAnimateExternalVisualDescriptor descriptor)
	{
		if (!GodotObject.IsInstanceValid(sprite))
		{
			return AdobeAnimateExternalVisualHandle.Invalid;
		}
		return sprite.RegisterExternalVisual(visual, descriptor);
	}

	public bool UnregisterCharacterExternalVisual(AdobeAnimateExternalVisualHandle handle)
	{
		if (GodotObject.IsInstanceValid(sprite) && handle.IsValid)
		{
			return sprite.UnregisterExternalVisual(handle);
		}
		return false;
	}

	internal bool TryGetIceTrapExternalVisualForDiagnostics(out AdobeAnimateExternalVisualSnapshot visual)
	{
		BuffComponent buffComponent = buff;
		if (buffComponent != null && !buffComponent.IsReleased)
		{
			return buff.TryGetBuffExternalVisual("Frozen", out visual);
		}
		visual = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(271)
		{
			new MethodInfo(MethodName.EmitDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitBodyHurt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitArmorHurt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitRiseOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitComponentChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitDamageBlocked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanReceiveExplosionHit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetHitBoxEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitBoxMonitorable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "monitorable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitBoxSuppressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitBoxMonitorSuppressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyHitBoxRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateHitBoxBounds, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGridPositionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "oldGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "newGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGroundHeightChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "oldGroundHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "newGroundHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCachedWorldPositionForProjectile, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshLocalScaleXSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyRenderAncestorScaleChangedIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureHitBoxRuntimeInitialized, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetHitBoxRuntimeFromDefinition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshExplosionHitEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCollisionPreviewDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawPrimaryHitBoxPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawResourceComponentCollisionPreviews, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendStateEvent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMainRuntimeState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeMainStateEvent, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareBatchRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelCellMoveTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrackCellMoveTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tween", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnableComponentGameplayUntilBattlefieldEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsWithinComponentBattlefieldBoundsForPhysicsFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGlobalPositionForPhysicsFrame, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGlobalPositionForPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGlobalPositionForPhysicsFrameNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TranslateForPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "globalDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishCharacterTranslationForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGlobalTransformForShadow, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGlobalTransformForPhysicsFrame, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLogicalGlobalPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLogicalGlobalPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "descendant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetLogicalGlobalTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "descendant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetLogicalGlobalPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCellMoveLogicalGlobalPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCellMoveRenderOrder, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisableGameplayForPermanentEmbeddedVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateGlobalPositionForPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateComponentGameplayForPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetComponentGameplayForCurrentPhysicsFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshComponentGameplayEntryExceptionForPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveComponentGameplayForPhysicsFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CacheComponentGameplayForPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			}, null),
			new MethodInfo(MethodName.EnsureComponentManagerResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "migrateLegacyHost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketCustomFromSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindMainStateHandles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnbindMainStateHandles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableInvalidRuntimeCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCharacterSkinSwitchedHandler, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SubscribeCharacterSkinSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeCharacterSkinSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseMainStateMachine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchProcessUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TickMainStateMachineProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "processFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TickComponentStateMachineProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "processFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PhysicsProcessWithFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunComponentPhysicsForTrace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "canDispatchOwnedStateMachines", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunCharacterPhysicsPostForTrace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareCharacterTimeScaleForPhysics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SynchronizeAnimationPlaybackBlock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePartInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "persontage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearArmorAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetArmors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "armorList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCustom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCustom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "custom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCustoms, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "customList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCharacterSkinSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetSaveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SwitchCustom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SleepEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SleepProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SleepExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComponentEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComponentExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Idle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Sleep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Component, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsDie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsIncapacitatedTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTotalHitPoint, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentHitPoint, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetHitpointAndScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "hitpointScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MagnetCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "armorInstance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDraw, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasShield, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasHelm, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProjectileEffectsBlocked, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetHasArmor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArmorFromName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArmor, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetArmorShield, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetArmorHelment, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetArmorHeadCover, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCollision, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maskFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CheckDifferentCamp, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckSameLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsTargetableFromLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "includeAllLineCheck", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsTargetableFromLineRange, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "minLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maxLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "includeAllLineCheck", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGroundHeight, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "posHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpriteGroupShaderParameter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SetZ, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySpriteGroupZPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseZMotionLocalRenderWhenStable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShovelDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "freeInstance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearFromMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroyWithVisualDelay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AshDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SmashDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroyReplace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitBoxDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HurtWithAttackConfig, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "attackConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "damageLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SkipInvincibleHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Health, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreFullHealthAfterRevive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BowlingHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SmashHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlagHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProjectileHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Bright, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "init", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "rise", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "riseDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.White, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "init", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.YBCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_collect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CoinCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_collect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LuckyBagCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BrainSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplicatedEventSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createBrainSun", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.JalapenoSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QXSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MagicSunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExplodeSunCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sunOnce", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_speed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_moveStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GoldShardCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CraterCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "nolimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "craterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "halfCrater", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BlowBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyBowlingImpactDisplacement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "distance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateBowlingImpactDisplacement, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelBowlingImpactDisplacement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncHologramHypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Rise, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDirt", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "changeState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "emitRiseOver", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "destroyAfterRecycle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshResourceComponentFacades, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateDirt, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSplash, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateIceTrap, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WakeUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnWakeUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UnlimitedFireInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InheritCoverSleepState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SupportsCoverSleepInheritance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Cover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Spawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldEnableGameplayDispatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateLevelEntryPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "animationClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMainStateMachineDispatchEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshComponentStateMachineDispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetOwnerBatchRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "registered", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateDirectMainStateMachineProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeOwnerBatchDispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SuspendOwnerBatchDispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureCharacterBatchProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterInitialMainStateMachine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureMainStateMachineSnapshotData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreMainStateMachineSnapshotData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressEntryEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "progressCompatible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreLegacyMainState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stateIdentity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginAuthoritativeMainStateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndAuthoritativeMainStateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingMainStateMachineSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareMainStateMachineSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "progressCompatible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsProgressStateMachineDefinitionIdCompatible, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "savedDefinitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "currentDefinitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMainStateMachineSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressEntryEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAuthoritativeMainStateRestored, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ActivateAdobeAnimateSpriteTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldKeepAnimationPaused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Blow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Garlic, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldUpdateGridPos, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRiseStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRiseEnd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanDiggerBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockDigger, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAnimeStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "loopAnim", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "blendTimeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HitpointsNearDie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFireAnime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateFireEventLists, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_eventTarget", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_allEventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateFireEffectAtCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "fireAnime", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "cellPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "battleControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "effectParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsJalapenoBattleContextCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "battleControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "effectParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlayFireExplodeEffects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSnowEventList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "_addEventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateColdVisualEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateColdEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_addEventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateColdEffectRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_range", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_addEventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_groundHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryConsumeIncomingBuff, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "buffConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuffAdd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "buffConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuffDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuffGet, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNetworkSpecialMovementActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRemoteNetworkAnimationStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreFromSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "saveConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SilentlyRemove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SilentlyRemoveForTransformation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnTransformEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "effectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeCharacterExternalVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeCharacterExternalVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttachAnimatedStatusVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "preferredSlot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "targetGlobalTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DetachAnimatedStatusVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterLegacySpriteGroupVisualsOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitDestroy && args.Count == 0)
		{
			EmitDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitBodyHurt && args.Count == 1)
		{
			EmitBodyHurt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitArmorHurt && args.Count == 1)
		{
			EmitArmorHurt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitRiseOver && args.Count == 0)
		{
			EmitRiseOver();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitComponentChange && args.Count == 0)
		{
			EmitComponentChange();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitDamageBlocked && args.Count == 0)
		{
			EmitDamageBlocked();
			ret = default;
			return true;
		}
		if (method == MethodName.CanReceiveExplosionHit && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReceiveExplosionHit());
			return true;
		}
		if (method == MethodName.SetHitBoxEnabled && args.Count == 1)
		{
			SetHitBoxEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitBoxMonitorable && args.Count == 1)
		{
			SetHitBoxMonitorable(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitBoxSuppressed && args.Count == 2)
		{
			SetHitBoxSuppressed(VariantUtils.ConvertTo<HitBoxSuppressionReason>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitBoxMonitorSuppressed && args.Count == 2)
		{
			SetHitBoxMonitorSuppressed(VariantUtils.ConvertTo<HitBoxSuppressionReason>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyHitBoxRuntime && args.Count == 0)
		{
			DestroyHitBoxRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateHitBoxBounds && args.Count == 0)
		{
			InvalidateHitBoxBounds();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGridPositionChanged && args.Count == 2)
		{
			OnGridPositionChanged(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGroundHeightChanged && args.Count == 2)
		{
			OnGroundHeightChanged(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedWorldPositionForProjectile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCachedWorldPositionForProjectile());
			return true;
		}
		if (method == MethodName.RefreshLocalScaleXSnapshot && args.Count == 0)
		{
			RefreshLocalScaleXSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderAncestorScaleChangedIfNeeded && args.Count == 0)
		{
			NotifyRenderAncestorScaleChangedIfNeeded();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureHitBoxRuntimeInitialized && args.Count == 0)
		{
			EnsureHitBoxRuntimeInitialized();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetHitBoxRuntimeFromDefinition && args.Count == 0)
		{
			ResetHitBoxRuntimeFromDefinition();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshExplosionHitEligibility && args.Count == 0)
		{
			RefreshExplosionHitEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw && args.Count == 1)
		{
			SetCollisionPreviewDraw(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawPrimaryHitBoxPreview && args.Count == 0)
		{
			DrawPrimaryHitBoxPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawResourceComponentCollisionPreviews && args.Count == 0)
		{
			DrawResourceComponentCollisionPreviews();
			ret = default;
			return true;
		}
		if (method == MethodName.SendStateEvent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SendStateEvent(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetMainRuntimeState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetMainRuntimeState(VariantUtils.ConvertTo<MainRuntimeState>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeMainStateEvent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(NormalizeMainStateEvent(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.PrepareBatchRegistration && args.Count == 0)
		{
			PrepareBatchRegistration();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelCellMoveTween && args.Count == 0)
		{
			CancelCellMoveTween();
			ret = default;
			return true;
		}
		if (method == MethodName.TrackCellMoveTween && args.Count == 1)
		{
			TrackCellMoveTween(VariantUtils.ConvertTo<Tween>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnableComponentGameplayUntilBattlefieldEntry && args.Count == 0)
		{
			EnableComponentGameplayUntilBattlefieldEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.IsWithinComponentBattlefieldBoundsForPhysicsFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWithinComponentBattlefieldBoundsForPhysicsFrame());
			return true;
		}
		if (method == MethodName.GetGlobalPositionForPhysicsFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetGlobalPositionForPhysicsFrame(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.SetGlobalPositionForPhysicsFrame && args.Count == 2)
		{
			SetGlobalPositionForPhysicsFrame(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGlobalPositionForPhysicsFrameNode && args.Count == 2)
		{
			SetGlobalPositionForPhysicsFrameNode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TranslateForPhysicsFrame && args.Count == 3)
		{
			TranslateForPhysicsFrame(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<ulong>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PublishCharacterTranslationForRender && args.Count == 2)
		{
			PublishCharacterTranslationForRender(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetGlobalTransformForShadow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetGlobalTransformForShadow(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGlobalTransformForPhysicsFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetGlobalTransformForPhysicsFrame(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLogicalGlobalPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetLogicalGlobalPosition());
			return true;
		}
		if (method == MethodName.GetLogicalGlobalPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetLogicalGlobalPosition(VariantUtils.ConvertTo<Node2D>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLogicalGlobalTransform && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetLogicalGlobalTransform(VariantUtils.ConvertTo<Node2D>(in args[0])));
			return true;
		}
		if (method == MethodName.SetLogicalGlobalPosition && args.Count == 1)
		{
			SetLogicalGlobalPosition(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCellMoveLogicalGlobalPosition && args.Count == 1)
		{
			SetCellMoveLogicalGlobalPosition(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCellMoveRenderOrder && args.Count == 1)
		{
			RefreshCellMoveRenderOrder(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisableGameplayForPermanentEmbeddedVisual && args.Count == 0)
		{
			DisableGameplayForPermanentEmbeddedVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateGlobalPositionForPhysicsFrame && args.Count == 0)
		{
			InvalidateGlobalPositionForPhysicsFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateComponentGameplayForPhysicsFrame && args.Count == 0)
		{
			InvalidateComponentGameplayForPhysicsFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.GetComponentGameplayForCurrentPhysicsFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(GetComponentGameplayForCurrentPhysicsFrame());
			return true;
		}
		if (method == MethodName.RefreshComponentGameplayEntryExceptionForPhysicsFrame && args.Count == 0)
		{
			RefreshComponentGameplayEntryExceptionForPhysicsFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveComponentGameplayForPhysicsFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ResolveComponentGameplayForPhysicsFrame());
			return true;
		}
		if (method == MethodName.CacheComponentGameplayForPhysicsFrame && args.Count == 1)
		{
			CacheComponentGameplayForPhysicsFrame(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.EnsureComponentManagerResource && args.Count == 1)
		{
			EnsureComponentManagerResource(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketCustomFromSave && args.Count == 0)
		{
			RefreshPacketCustomFromSave();
			ret = default;
			return true;
		}
		if (method == MethodName.BindMainStateHandles && args.Count == 0)
		{
			BindMainStateHandles();
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindMainStateHandles && args.Count == 0)
		{
			UnbindMainStateHandles();
			ret = default;
			return true;
		}
		if (method == MethodName.DisableInvalidRuntimeCharacter && args.Count == 0)
		{
			DisableInvalidRuntimeCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCharacterSkinSwitchedHandler && args.Count == 0)
		{
			EnsureCharacterSkinSwitchedHandler();
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeCharacterSkinSwitched && args.Count == 0)
		{
			SubscribeCharacterSkinSwitched();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeCharacterSkinSwitched && args.Count == 0)
		{
			UnsubscribeCharacterSkinSwitched();
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseMainStateMachine && args.Count == 0)
		{
			ReleaseMainStateMachine();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchProcessUpdate && args.Count == 1)
		{
			BatchProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TickMainStateMachineProcess && args.Count == 2)
		{
			TickMainStateMachineProcess(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TickComponentStateMachineProcess && args.Count == 2)
		{
			TickComponentStateMachineProcess(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PhysicsProcessWithFrame && args.Count == 2)
		{
			PhysicsProcessWithFrame(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunComponentPhysicsForTrace && args.Count == 4)
		{
			RunComponentPhysicsForTrace(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertTo<ComponentManager>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunCharacterPhysicsPostForTrace && args.Count == 2)
		{
			RunCharacterPhysicsPostForTrace(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareCharacterTimeScaleForPhysics && args.Count == 1)
		{
			PrepareCharacterTimeScaleForPhysics(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SynchronizeAnimationPlaybackBlock && args.Count == 0)
		{
			SynchronizeAnimationPlaybackBlock();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePartInit && args.Count == 0)
		{
			DamagePartInit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewDamagePoint && args.Count == 1)
		{
			PreviewDamagePoint(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearArmor && args.Count == 1)
		{
			ClearArmor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearArmorAll && args.Count == 0)
		{
			ClearArmorAll();
			ret = default;
			return true;
		}
		if (method == MethodName.SetArmor && args.Count == 2)
		{
			SetArmor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetArmors && args.Count == 1)
		{
			SetArmors(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCustom && args.Count == 0)
		{
			ClearCustom();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCustom && args.Count == 1)
		{
			SetCustom(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCustoms && args.Count == 1)
		{
			SetCustoms(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterSkinSwitched && args.Count == 2)
		{
			OnCharacterSkinSwitched(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SwitchCustom && args.Count == 1)
		{
			SwitchCustom(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleExited && args.Count == 0)
		{
			IdleExited();
			ret = default;
			return true;
		}
		if (method == MethodName.SleepEntered && args.Count == 0)
		{
			SleepEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.SleepProcessing && args.Count == 1)
		{
			SleepProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SleepExited && args.Count == 0)
		{
			SleepExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ComponentEntered && args.Count == 0)
		{
			ComponentEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ComponentExited && args.Count == 0)
		{
			ComponentExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Idle && args.Count == 0)
		{
			Idle();
			ret = default;
			return true;
		}
		if (method == MethodName.Sleep && args.Count == 0)
		{
			Sleep();
			ret = default;
			return true;
		}
		if (method == MethodName.Component && args.Count == 0)
		{
			Component();
			ret = default;
			return true;
		}
		if (method == MethodName.IsDie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDie());
			return true;
		}
		if (method == MethodName.CanSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSleep());
			return true;
		}
		if (method == MethodName.IsSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSleep());
			return true;
		}
		if (method == MethodName.IsIncapacitatedTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIncapacitatedTarget());
			return true;
		}
		if (method == MethodName.GetTotalHitPoint && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetTotalHitPoint());
			return true;
		}
		if (method == MethodName.GetCurrentHitPoint && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetCurrentHitPoint());
			return true;
		}
		if (method == MethodName.SetHitpointAndScale && args.Count == 2)
		{
			SetHitpointAndScale(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MagnetCreate && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMagnet>(MagnetCreate(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1])));
			return true;
		}
		if (method == MethodName.ArmorDraw && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMagnet>(ArmorDraw(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.HasShield && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasShield());
			return true;
		}
		if (method == MethodName.HasHelm && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasHelm());
			return true;
		}
		if (method == MethodName.ProjectileEffectsBlocked && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ProjectileEffectsBlocked(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetHasArmor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetHasArmor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetArmorFromName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorInstance>(GetArmorFromName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetArmor && args.Count == 0)
		{
			Array<TowerDefenseArmorInstance> armor = GetArmor();
			ret = VariantUtils.CreateFromArray(armor);
			return true;
		}
		if (method == MethodName.GetArmorShield && args.Count == 0)
		{
			Array<TowerDefenseArmorInstance> armorShield = GetArmorShield();
			ret = VariantUtils.CreateFromArray(armorShield);
			return true;
		}
		if (method == MethodName.GetArmorHelment && args.Count == 0)
		{
			Array<TowerDefenseArmorInstance> armorHelment = GetArmorHelment();
			ret = VariantUtils.CreateFromArray(armorHelment);
			return true;
		}
		if (method == MethodName.GetArmorHeadCover && args.Count == 0)
		{
			Array<TowerDefenseArmorInstance> armorHeadCover = GetArmorHeadCover();
			ret = VariantUtils.CreateFromArray(armorHeadCover);
			return true;
		}
		if (method == MethodName.CanCollision && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCollision(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CanTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CheckDifferentCamp && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckDifferentCamp(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0])));
			return true;
		}
		if (method == MethodName.CheckSameLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckSameLine(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsTargetableFromLine && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTargetableFromLine(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.IsTargetableFromLineRange && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTargetableFromLineRange(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetGroundHeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetGroundHeight(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSpriteGroupShaderParameter && args.Count == 2)
		{
			SetSpriteGroupShaderParameter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetZ && args.Count == 0)
		{
			SetZ();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpriteGroupZPosition && args.Count == 0)
		{
			ApplySpriteGroupZPosition();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseZMotionLocalRenderWhenStable && args.Count == 0)
		{
			ReleaseZMotionLocalRenderWhenStable();
			ret = default;
			return true;
		}
		if (method == MethodName.ShovelDestroy && args.Count == 0)
		{
			ShovelDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 1)
		{
			Destroy(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearFromMap && args.Count == 0)
		{
			ClearFromMap();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyWithVisualDelay && args.Count == 1)
		{
			DestroyWithVisualDelay(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AshDestroy && args.Count == 0)
		{
			AshDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SmashDestroy && args.Count == 0)
		{
			SmashDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyReplace && args.Count == 0)
		{
			DestroyReplace();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.HitBoxDestroy && args.Count == 0)
		{
			HitBoxDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(HurtWithAttackConfig(VariantUtils.ConvertTo<AttackConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.Hurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(Hurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		if (method == MethodName.SkipInvincibleHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(SkipInvincibleHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.Health && args.Count == 1)
		{
			Health(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreFullHealthAfterRevive && args.Count == 0)
		{
			RestoreFullHealthAfterRevive();
			ret = default;
			return true;
		}
		if (method == MethodName.BowlingHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(BowlingHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.SmashHurt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(SmashHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.ExplodeHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ExplodeHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.FlagHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(FlagHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.ProjectileHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ProjectileHurt(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.Bright && args.Count == 5)
		{
			Bright(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.White && args.Count == 3)
		{
			White(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.YBCreate && args.Count == 5)
		{
			YBCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.CoinCreate && args.Count == 5)
		{
			CoinCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.LuckyBagCreate && args.Count == 3)
		{
			LuckyBagCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SunCreate && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(SunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.BrainSunCreate && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(BrainSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.ReplicatedEventSunCreate && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(ReplicatedEventSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6])));
			return true;
		}
		if (method == MethodName.JalapenoSunCreate && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(JalapenoSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.QXSunCreate && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(QXSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.MagicSunCreate && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseSunBase>(MagicSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.ExplodeSunCreate && args.Count == 7)
		{
			ExplodeSunCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<long>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.GoldShardCreate && args.Count == 3)
		{
			GoldShardCreate(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CraterCreate && args.Count == 3)
		{
			CraterCreate(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BlowBack && args.Count == 2)
		{
			BlowBack(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyBowlingImpactDisplacement && args.Count == 2)
		{
			ApplyBowlingImpactDisplacement(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateBowlingImpactDisplacement && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(UpdateBowlingImpactDisplacement(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.CancelBowlingImpactDisplacement && args.Count == 0)
		{
			CancelBowlingImpactDisplacement();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncHologramHypnoses && args.Count == 1)
		{
			SyncHologramHypnoses(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Rise && args.Count == 6)
		{
			Rise(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.Recycle && args.Count == 2)
		{
			Recycle(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshResourceComponentFacades && args.Count == 0)
		{
			RefreshResourceComponentFacades();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDirt && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(CreateDirt());
			return true;
		}
		if (method == MethodName.CreateSplash && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(CreateSplash());
			return true;
		}
		if (method == MethodName.CreateIceTrap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(CreateIceTrap());
			return true;
		}
		if (method == MethodName.WakeUp && args.Count == 0)
		{
			WakeUp();
			ret = default;
			return true;
		}
		if (method == MethodName.OnWakeUp && args.Count == 0)
		{
			OnWakeUp();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach && args.Count == 2)
		{
			ArmorDamagePointReach(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnlimitedFireInit && args.Count == 0)
		{
			UnlimitedFireInit();
			ret = default;
			return true;
		}
		if (method == MethodName.InheritCoverSleepState && args.Count == 1)
		{
			InheritCoverSleepState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SupportsCoverSleepInheritance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SupportsCoverSleepInheritance(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Cover && args.Count == 1)
		{
			Cover(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Spawn && args.Count == 0)
		{
			Spawn();
			ret = default;
			return true;
		}
		if (method == MethodName.PreSpawn && args.Count == 0)
		{
			PreSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldEnableGameplayDispatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldEnableGameplayDispatch());
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateLevelEntryPreview && args.Count == 1)
		{
			ActivateLevelEntryPreview(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMainStateMachineDispatchEnabled && args.Count == 1)
		{
			SetMainStateMachineDispatchEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshComponentStateMachineDispatch && args.Count == 0)
		{
			RefreshComponentStateMachineDispatch();
			ret = default;
			return true;
		}
		if (method == MethodName.SetOwnerBatchRegistration && args.Count == 1)
		{
			SetOwnerBatchRegistration(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDirectMainStateMachineProcess && args.Count == 0)
		{
			UpdateDirectMainStateMachineProcess();
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeOwnerBatchDispatch && args.Count == 0)
		{
			ResumeOwnerBatchDispatch();
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendOwnerBatchDispatch && args.Count == 0)
		{
			SuspendOwnerBatchDispatch();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureCharacterBatchProcessing && args.Count == 0)
		{
			ConfigureCharacterBatchProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterInitialMainStateMachine && args.Count == 0)
		{
			EnterInitialMainStateMachine();
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureMainStateMachineSnapshotData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureMainStateMachineSnapshotData());
			return true;
		}
		if (method == MethodName.RestoreMainStateMachineSnapshotData && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreMainStateMachineSnapshotData(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.RestoreLegacyMainState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreLegacyMainState(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BeginAuthoritativeMainStateRestore && args.Count == 0)
		{
			BeginAuthoritativeMainStateRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.EndAuthoritativeMainStateRestore && args.Count == 0)
		{
			EndAuthoritativeMainStateRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingMainStateMachineSnapshot && args.Count == 0)
		{
			ApplyPendingMainStateMachineSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareMainStateMachineSnapshot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(PrepareMainStateMachineSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.IsProgressStateMachineDefinitionIdCompatible && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProgressStateMachineDefinitionIdCompatible(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyMainStateMachineSnapshot && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplyMainStateMachineSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.OnAuthoritativeMainStateRestored && args.Count == 2)
		{
			OnAuthoritativeMainStateRestored(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateAdobeAnimateSpriteTree && args.Count == 1)
		{
			ActivateAdobeAnimateSpriteTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldKeepAnimationPaused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldKeepAnimationPaused());
			return true;
		}
		if (method == MethodName.Blow && args.Count == 0)
		{
			Blow();
			ret = default;
			return true;
		}
		if (method == MethodName.Garlic && args.Count == 0)
		{
			Garlic();
			ret = default;
			return true;
		}
		if (method == MethodName.CanBlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanBlock());
			return true;
		}
		if (method == MethodName.ShouldUpdateGridPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUpdateGridPos());
			return true;
		}
		if (method == MethodName.OnRiseStart && args.Count == 0)
		{
			OnRiseStart();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRiseEnd && args.Count == 0)
		{
			OnRiseEnd();
			ret = default;
			return true;
		}
		if (method == MethodName.BlockType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BlockType());
			return true;
		}
		if (method == MethodName.Block && args.Count == 1)
		{
			Block(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanDiggerBlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDiggerBlock());
			return true;
		}
		if (method == MethodName.BlockDigger && args.Count == 1)
		{
			BlockDigger(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnZombie && args.Count == 0)
		{
			SpawnZombie();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimeStarted && args.Count == 1)
		{
			OnAnimeStarted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncAnimation && args.Count == 3)
		{
			SyncAnimation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsNearDie && args.Count == 0)
		{
			HitpointsNearDie();
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsEmpty && args.Count == 0)
		{
			HitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.GetFireAnime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetFireAnime(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateFireEventLists && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateFireEventLists(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateFireEffectAtCell && args.Count == 4)
		{
			CreateFireEffectAtCell(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<Node2D>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsJalapenoBattleContextCurrent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsJalapenoBattleContextCurrent(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1])));
			return true;
		}
		if (method == MethodName.PlayFireExplodeEffects && args.Count == 0)
		{
			PlayFireExplodeEffects();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSnowEventList && args.Count == 1)
		{
			Array<TowerDefenseCharacterEventBase> array2 = CreateSnowEventList(VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CreateColdVisualEffect && args.Count == 1)
		{
			CreateColdVisualEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateColdEffect && args.Count == 3)
		{
			CreateColdEffect(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateColdEffectRange && args.Count == 4)
		{
			CreateColdEffectRange(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCharacter && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.TryConsumeIncomingBuff && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryConsumeIncomingBuff(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuffAdd && args.Count == 1)
		{
			BuffAdd(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuffDelete && args.Count == 1)
		{
			BuffDelete(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuffGet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterBuffConfig>(BuffGet(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpawnState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpawnState());
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
			return true;
		}
		if (method == MethodName.IsNetworkSpecialMovementActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNetworkSpecialMovementActive());
			return true;
		}
		if (method == MethodName.OnRemoteNetworkAnimationStart && args.Count == 1)
		{
			OnRemoteNetworkAnimationStart(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreFromSave && args.Count == 1)
		{
			RestoreFromSave(VariantUtils.ConvertTo<TowerDefenseCharacterSaveConfigCSharp>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.PrepareForProgressRestore && args.Count == 0)
		{
			PrepareForProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.SilentlyRemove && args.Count == 0)
		{
			SilentlyRemove();
			ret = default;
			return true;
		}
		if (method == MethodName.SilentlyRemoveForTransformation && args.Count == 0)
		{
			SilentlyRemoveForTransformation();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnTransformEffect && args.Count == 3)
		{
			SpawnTransformEffect(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeCharacterExternalVisuals && args.Count == 0)
		{
			InitializeCharacterExternalVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCharacterExternalVisuals && args.Count == 0)
		{
			DisposeCharacterExternalVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.AttachAnimatedStatusVisual && args.Count == 3)
		{
			AttachAnimatedStatusVisual(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachAnimatedStatusVisual && args.Count == 1)
		{
			DetachAnimatedStatusVisual(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterLegacySpriteGroupVisualsOnce && args.Count == 0)
		{
			RegisterLegacySpriteGroupVisualsOnce();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SupportsCoverSleepInheritance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SupportsCoverSleepInheritance(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFireAnime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetFireAnime(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateFireEventLists && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateFireEventLists(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateFireEffectAtCell && args.Count == 4)
		{
			CreateFireEffectAtCell(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[2]), VariantUtils.ConvertTo<Node2D>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsJalapenoBattleContextCurrent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsJalapenoBattleContextCurrent(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1])));
			return true;
		}
		if (method == MethodName.PlayFireExplodeEffects && args.Count == 0)
		{
			PlayFireExplodeEffects();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSnowEventList && args.Count == 1)
		{
			Array<TowerDefenseCharacterEventBase> array = CreateSnowEventList(VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CreateColdVisualEffect && args.Count == 1)
		{
			CreateColdVisualEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateColdEffect && args.Count == 3)
		{
			CreateColdEffect(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateColdEffectRange && args.Count == 4)
		{
			CreateColdEffectRange(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCharacter && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		if (method == MethodName.SpawnTransformEffect && args.Count == 3)
		{
			SpawnTransformEffect(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitDestroy)
		{
			return true;
		}
		if (method == MethodName.EmitBodyHurt)
		{
			return true;
		}
		if (method == MethodName.EmitArmorHurt)
		{
			return true;
		}
		if (method == MethodName.EmitRiseOver)
		{
			return true;
		}
		if (method == MethodName.EmitComponentChange)
		{
			return true;
		}
		if (method == MethodName.EmitDamageBlocked)
		{
			return true;
		}
		if (method == MethodName.CanReceiveExplosionHit)
		{
			return true;
		}
		if (method == MethodName.SetHitBoxEnabled)
		{
			return true;
		}
		if (method == MethodName.SetHitBoxMonitorable)
		{
			return true;
		}
		if (method == MethodName.SetHitBoxSuppressed)
		{
			return true;
		}
		if (method == MethodName.SetHitBoxMonitorSuppressed)
		{
			return true;
		}
		if (method == MethodName.DestroyHitBoxRuntime)
		{
			return true;
		}
		if (method == MethodName.InvalidateHitBoxBounds)
		{
			return true;
		}
		if (method == MethodName.OnGridPositionChanged)
		{
			return true;
		}
		if (method == MethodName.OnGroundHeightChanged)
		{
			return true;
		}
		if (method == MethodName.GetCachedWorldPositionForProjectile)
		{
			return true;
		}
		if (method == MethodName.RefreshLocalScaleXSnapshot)
		{
			return true;
		}
		if (method == MethodName.NotifyRenderAncestorScaleChangedIfNeeded)
		{
			return true;
		}
		if (method == MethodName.EnsureHitBoxRuntimeInitialized)
		{
			return true;
		}
		if (method == MethodName.ResetHitBoxRuntimeFromDefinition)
		{
			return true;
		}
		if (method == MethodName.RefreshExplosionHitEligibility)
		{
			return true;
		}
		if (method == MethodName.SetCollisionPreviewDraw)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawPrimaryHitBoxPreview)
		{
			return true;
		}
		if (method == MethodName.DrawResourceComponentCollisionPreviews)
		{
			return true;
		}
		if (method == MethodName.SendStateEvent)
		{
			return true;
		}
		if (method == MethodName.SetMainRuntimeState)
		{
			return true;
		}
		if (method == MethodName.NormalizeMainStateEvent)
		{
			return true;
		}
		if (method == MethodName.PrepareBatchRegistration)
		{
			return true;
		}
		if (method == MethodName.CancelCellMoveTween)
		{
			return true;
		}
		if (method == MethodName.TrackCellMoveTween)
		{
			return true;
		}
		if (method == MethodName.EnableComponentGameplayUntilBattlefieldEntry)
		{
			return true;
		}
		if (method == MethodName.IsWithinComponentBattlefieldBoundsForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.GetGlobalPositionForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.SetGlobalPositionForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.SetGlobalPositionForPhysicsFrameNode)
		{
			return true;
		}
		if (method == MethodName.TranslateForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.PublishCharacterTranslationForRender)
		{
			return true;
		}
		if (method == MethodName.GetGlobalTransformForShadow)
		{
			return true;
		}
		if (method == MethodName.GetGlobalTransformForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.GetLogicalGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.GetLogicalGlobalTransform)
		{
			return true;
		}
		if (method == MethodName.SetLogicalGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.SetCellMoveLogicalGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.RefreshCellMoveRenderOrder)
		{
			return true;
		}
		if (method == MethodName.DisableGameplayForPermanentEmbeddedVisual)
		{
			return true;
		}
		if (method == MethodName.InvalidateGlobalPositionForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.InvalidateComponentGameplayForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.GetComponentGameplayForCurrentPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.RefreshComponentGameplayEntryExceptionForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.ResolveComponentGameplayForPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.CacheComponentGameplayForPhysicsFrame)
		{
			return true;
		}
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
		if (method == MethodName.EnsureComponentManagerResource)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketCustomFromSave)
		{
			return true;
		}
		if (method == MethodName.BindMainStateHandles)
		{
			return true;
		}
		if (method == MethodName.UnbindMainStateHandles)
		{
			return true;
		}
		if (method == MethodName.DisableInvalidRuntimeCharacter)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.EnsureCharacterSkinSwitchedHandler)
		{
			return true;
		}
		if (method == MethodName.SubscribeCharacterSkinSwitched)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeCharacterSkinSwitched)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName.ReleaseMainStateMachine)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.BatchProcessUpdate)
		{
			return true;
		}
		if (method == MethodName.TickMainStateMachineProcess)
		{
			return true;
		}
		if (method == MethodName.TickComponentStateMachineProcess)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.PhysicsProcessWithFrame)
		{
			return true;
		}
		if (method == MethodName.RunComponentPhysicsForTrace)
		{
			return true;
		}
		if (method == MethodName.RunCharacterPhysicsPostForTrace)
		{
			return true;
		}
		if (method == MethodName.PrepareCharacterTimeScaleForPhysics)
		{
			return true;
		}
		if (method == MethodName.SynchronizeAnimationPlaybackBlock)
		{
			return true;
		}
		if (method == MethodName.DamagePartInit)
		{
			return true;
		}
		if (method == MethodName.PreviewDamagePoint)
		{
			return true;
		}
		if (method == MethodName.ClearArmor)
		{
			return true;
		}
		if (method == MethodName.ClearArmorAll)
		{
			return true;
		}
		if (method == MethodName.SetArmor)
		{
			return true;
		}
		if (method == MethodName.SetArmors)
		{
			return true;
		}
		if (method == MethodName.ClearCustom)
		{
			return true;
		}
		if (method == MethodName.SetCustom)
		{
			return true;
		}
		if (method == MethodName.SetCustoms)
		{
			return true;
		}
		if (method == MethodName.OnCharacterSkinSwitched)
		{
			return true;
		}
		if (method == MethodName.SwitchCustom)
		{
			return true;
		}
		if (method == MethodName.OnCustomSwitched)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.IdleExited)
		{
			return true;
		}
		if (method == MethodName.SleepEntered)
		{
			return true;
		}
		if (method == MethodName.SleepProcessing)
		{
			return true;
		}
		if (method == MethodName.SleepExited)
		{
			return true;
		}
		if (method == MethodName.ComponentEntered)
		{
			return true;
		}
		if (method == MethodName.ComponentExited)
		{
			return true;
		}
		if (method == MethodName.Idle)
		{
			return true;
		}
		if (method == MethodName.Sleep)
		{
			return true;
		}
		if (method == MethodName.Component)
		{
			return true;
		}
		if (method == MethodName.IsDie)
		{
			return true;
		}
		if (method == MethodName.CanSleep)
		{
			return true;
		}
		if (method == MethodName.IsSleep)
		{
			return true;
		}
		if (method == MethodName.IsIncapacitatedTarget)
		{
			return true;
		}
		if (method == MethodName.GetTotalHitPoint)
		{
			return true;
		}
		if (method == MethodName.GetCurrentHitPoint)
		{
			return true;
		}
		if (method == MethodName.SetHitpointAndScale)
		{
			return true;
		}
		if (method == MethodName.MagnetCreate)
		{
			return true;
		}
		if (method == MethodName.ArmorDraw)
		{
			return true;
		}
		if (method == MethodName.HasShield)
		{
			return true;
		}
		if (method == MethodName.HasHelm)
		{
			return true;
		}
		if (method == MethodName.ProjectileEffectsBlocked)
		{
			return true;
		}
		if (method == MethodName.GetHasArmor)
		{
			return true;
		}
		if (method == MethodName.GetArmorFromName)
		{
			return true;
		}
		if (method == MethodName.GetArmor)
		{
			return true;
		}
		if (method == MethodName.GetArmorShield)
		{
			return true;
		}
		if (method == MethodName.GetArmorHelment)
		{
			return true;
		}
		if (method == MethodName.GetArmorHeadCover)
		{
			return true;
		}
		if (method == MethodName.CanCollision)
		{
			return true;
		}
		if (method == MethodName.CanTarget)
		{
			return true;
		}
		if (method == MethodName.CheckDifferentCamp)
		{
			return true;
		}
		if (method == MethodName.CheckSameLine)
		{
			return true;
		}
		if (method == MethodName.IsTargetableFromLine)
		{
			return true;
		}
		if (method == MethodName.IsTargetableFromLineRange)
		{
			return true;
		}
		if (method == MethodName.GetGroundHeight)
		{
			return true;
		}
		if (method == MethodName.SetSpriteGroupShaderParameter)
		{
			return true;
		}
		if (method == MethodName.SetZ)
		{
			return true;
		}
		if (method == MethodName.ApplySpriteGroupZPosition)
		{
			return true;
		}
		if (method == MethodName.ReleaseZMotionLocalRenderWhenStable)
		{
			return true;
		}
		if (method == MethodName.ShovelDestroy)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.ClearFromMap)
		{
			return true;
		}
		if (method == MethodName.DestroyWithVisualDelay)
		{
			return true;
		}
		if (method == MethodName.AshDestroy)
		{
			return true;
		}
		if (method == MethodName.SmashDestroy)
		{
			return true;
		}
		if (method == MethodName.DestroyReplace)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.HitBoxDestroy)
		{
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig)
		{
			return true;
		}
		if (method == MethodName.Hurt)
		{
			return true;
		}
		if (method == MethodName.SkipInvincibleHurt)
		{
			return true;
		}
		if (method == MethodName.Health)
		{
			return true;
		}
		if (method == MethodName.RestoreFullHealthAfterRevive)
		{
			return true;
		}
		if (method == MethodName.BowlingHurt)
		{
			return true;
		}
		if (method == MethodName.SmashHurt)
		{
			return true;
		}
		if (method == MethodName.ExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.FlagHurt)
		{
			return true;
		}
		if (method == MethodName.ProjectileHurt)
		{
			return true;
		}
		if (method == MethodName.Bright)
		{
			return true;
		}
		if (method == MethodName.White)
		{
			return true;
		}
		if (method == MethodName.YBCreate)
		{
			return true;
		}
		if (method == MethodName.CoinCreate)
		{
			return true;
		}
		if (method == MethodName.LuckyBagCreate)
		{
			return true;
		}
		if (method == MethodName.SunCreate)
		{
			return true;
		}
		if (method == MethodName.BrainSunCreate)
		{
			return true;
		}
		if (method == MethodName.ReplicatedEventSunCreate)
		{
			return true;
		}
		if (method == MethodName.JalapenoSunCreate)
		{
			return true;
		}
		if (method == MethodName.QXSunCreate)
		{
			return true;
		}
		if (method == MethodName.MagicSunCreate)
		{
			return true;
		}
		if (method == MethodName.ExplodeSunCreate)
		{
			return true;
		}
		if (method == MethodName.GoldShardCreate)
		{
			return true;
		}
		if (method == MethodName.CraterCreate)
		{
			return true;
		}
		if (method == MethodName.BlowBack)
		{
			return true;
		}
		if (method == MethodName.ApplyBowlingImpactDisplacement)
		{
			return true;
		}
		if (method == MethodName.UpdateBowlingImpactDisplacement)
		{
			return true;
		}
		if (method == MethodName.CancelBowlingImpactDisplacement)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.SyncHologramHypnoses)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.Rise)
		{
			return true;
		}
		if (method == MethodName.Recycle)
		{
			return true;
		}
		if (method == MethodName.RefreshResourceComponentFacades)
		{
			return true;
		}
		if (method == MethodName.CreateDirt)
		{
			return true;
		}
		if (method == MethodName.CreateSplash)
		{
			return true;
		}
		if (method == MethodName.CreateIceTrap)
		{
			return true;
		}
		if (method == MethodName.WakeUp)
		{
			return true;
		}
		if (method == MethodName.OnWakeUp)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		if (method == MethodName.UnlimitedFireInit)
		{
			return true;
		}
		if (method == MethodName.InheritCoverSleepState)
		{
			return true;
		}
		if (method == MethodName.SupportsCoverSleepInheritance)
		{
			return true;
		}
		if (method == MethodName.Cover)
		{
			return true;
		}
		if (method == MethodName.Spawn)
		{
			return true;
		}
		if (method == MethodName.PreSpawn)
		{
			return true;
		}
		if (method == MethodName.ShouldEnableGameplayDispatch)
		{
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.ActivateLevelEntryPreview)
		{
			return true;
		}
		if (method == MethodName.SetMainStateMachineDispatchEnabled)
		{
			return true;
		}
		if (method == MethodName.RefreshComponentStateMachineDispatch)
		{
			return true;
		}
		if (method == MethodName.SetOwnerBatchRegistration)
		{
			return true;
		}
		if (method == MethodName.UpdateDirectMainStateMachineProcess)
		{
			return true;
		}
		if (method == MethodName.ResumeOwnerBatchDispatch)
		{
			return true;
		}
		if (method == MethodName.SuspendOwnerBatchDispatch)
		{
			return true;
		}
		if (method == MethodName.ConfigureCharacterBatchProcessing)
		{
			return true;
		}
		if (method == MethodName.EnterInitialMainStateMachine)
		{
			return true;
		}
		if (method == MethodName.CaptureMainStateMachineSnapshotData)
		{
			return true;
		}
		if (method == MethodName.RestoreMainStateMachineSnapshotData)
		{
			return true;
		}
		if (method == MethodName.RestoreLegacyMainState)
		{
			return true;
		}
		if (method == MethodName.BeginAuthoritativeMainStateRestore)
		{
			return true;
		}
		if (method == MethodName.EndAuthoritativeMainStateRestore)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingMainStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.PrepareMainStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.IsProgressStateMachineDefinitionIdCompatible)
		{
			return true;
		}
		if (method == MethodName.ApplyMainStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.OnAuthoritativeMainStateRestored)
		{
			return true;
		}
		if (method == MethodName.ActivateAdobeAnimateSpriteTree)
		{
			return true;
		}
		if (method == MethodName.ShouldKeepAnimationPaused)
		{
			return true;
		}
		if (method == MethodName.Blow)
		{
			return true;
		}
		if (method == MethodName.Garlic)
		{
			return true;
		}
		if (method == MethodName.CanBlock)
		{
			return true;
		}
		if (method == MethodName.ShouldUpdateGridPos)
		{
			return true;
		}
		if (method == MethodName.OnRiseStart)
		{
			return true;
		}
		if (method == MethodName.OnRiseEnd)
		{
			return true;
		}
		if (method == MethodName.BlockType)
		{
			return true;
		}
		if (method == MethodName.Block)
		{
			return true;
		}
		if (method == MethodName.CanDiggerBlock)
		{
			return true;
		}
		if (method == MethodName.BlockDigger)
		{
			return true;
		}
		if (method == MethodName.Purify)
		{
			return true;
		}
		if (method == MethodName.SpawnZombie)
		{
			return true;
		}
		if (method == MethodName.OnAnimeStarted)
		{
			return true;
		}
		if (method == MethodName.SyncAnimation)
		{
			return true;
		}
		if (method == MethodName.HitpointsNearDie)
		{
			return true;
		}
		if (method == MethodName.HitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.GetFireAnime)
		{
			return true;
		}
		if (method == MethodName.CreateFireEventLists)
		{
			return true;
		}
		if (method == MethodName.CreateFireEffectAtCell)
		{
			return true;
		}
		if (method == MethodName.IsJalapenoBattleContextCurrent)
		{
			return true;
		}
		if (method == MethodName.PlayFireExplodeEffects)
		{
			return true;
		}
		if (method == MethodName.CreateSnowEventList)
		{
			return true;
		}
		if (method == MethodName.CreateColdVisualEffect)
		{
			return true;
		}
		if (method == MethodName.CreateColdEffect)
		{
			return true;
		}
		if (method == MethodName.CreateColdEffectRange)
		{
			return true;
		}
		if (method == MethodName.CreateCharacter)
		{
			return true;
		}
		if (method == MethodName.TryConsumeIncomingBuff)
		{
			return true;
		}
		if (method == MethodName.BuffAdd)
		{
			return true;
		}
		if (method == MethodName.BuffDelete)
		{
			return true;
		}
		if (method == MethodName.BuffGet)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		if (method == MethodName.IsNetworkSpecialMovementActive)
		{
			return true;
		}
		if (method == MethodName.OnRemoteNetworkAnimationStart)
		{
			return true;
		}
		if (method == MethodName.RestoreFromSave)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.PrepareForProgressRestore)
		{
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.SilentlyRemove)
		{
			return true;
		}
		if (method == MethodName.SilentlyRemoveForTransformation)
		{
			return true;
		}
		if (method == MethodName.SpawnTransformEffect)
		{
			return true;
		}
		if (method == MethodName.InitializeCharacterExternalVisuals)
		{
			return true;
		}
		if (method == MethodName.DisposeCharacterExternalVisuals)
		{
			return true;
		}
		if (method == MethodName.AttachAnimatedStatusVisual)
		{
			return true;
		}
		if (method == MethodName.DetachAnimatedStatusVisual)
		{
			return true;
		}
		if (method == MethodName.RegisterLegacySpriteGroupVisualsOnce)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.HasValidRuntimeConfiguration)
		{
			HasValidRuntimeConfiguration = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.HitBoxDefinition)
		{
			HitBoxDefinition = VariantUtils.ConvertTo<CharacterHitBoxDefinition>(in value);
			return true;
		}
		if (name == PropertyName.MainStateMachineDefinition)
		{
			MainStateMachineDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName.ComponentSet)
		{
			ComponentSet = VariantUtils.ConvertTo<CharacterComponentSet>(in value);
			return true;
		}
		if (name == PropertyName.invisible)
		{
			invisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.headSlot)
		{
			headSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName.camp)
		{
			camp = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in value);
			return true;
		}
		if (name == PropertyName.damagePartClip)
		{
			damagePartClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damagePart)
		{
			damagePart = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.damagePartSlot)
		{
			damagePartSlot = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.previewDamagePointPersontage)
		{
			previewDamagePointPersontage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentArmor)
		{
			currentArmor = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.currentCustom)
		{
			currentCustom = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.componentAlive)
		{
			componentAlive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.inWater)
		{
			inWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsProgressRestoreInFlight)
		{
			IsProgressRestoreInFlight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CharacterExternalVisualCompatibilityScanCount)
		{
			CharacterExternalVisualCompatibilityScanCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CharacterAnimatedStatusVisualCount)
		{
			CharacterAnimatedStatusVisualCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CharacterAnimatedStatusTopologyChangeCount)
		{
			CharacterAnimatedStatusTopologyChangeCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.syncId)
		{
			syncId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.backEffectNode)
		{
			backEffectNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.frontEffectNode)
		{
			frontEffectNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.spriteGroup)
		{
			spriteGroup = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.transformPoint)
		{
			transformPoint = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.shadowSprite)
		{
			shadowSprite = VariantUtils.ConvertTo<TowerDefenseShadowVisual>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxDefinition)
		{
			_hitBoxDefinition = VariantUtils.ConvertTo<CharacterHitBoxDefinition>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxRuntimeInitialized)
		{
			_hitBoxRuntimeInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxAvailable)
		{
			_hitBoxAvailable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxDefaultEnabled)
		{
			_hitBoxDefaultEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxDefaultMonitorable)
		{
			_hitBoxDefaultMonitorable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxSuppression)
		{
			_hitBoxSuppression = VariantUtils.ConvertTo<HitBoxSuppressionReason>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxMonitorSuppression)
		{
			_hitBoxMonitorSuppression = VariantUtils.ConvertTo<HitBoxSuppressionReason>(in value);
			return true;
		}
		if (name == PropertyName._canReceiveExplosionHit)
		{
			_canReceiveExplosionHit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxPreviewDraw)
		{
			_hitBoxPreviewDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._worldHitRectCache)
		{
			_worldHitRectCache = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheFrame)
		{
			_worldHitRectCacheFrame = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheTransform)
		{
			_worldHitRectCacheTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheScaleX)
		{
			_worldHitRectCacheScaleX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._localScaleXSnapshot)
		{
			_localScaleXSnapshot = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._localScaleXSnapshotValid)
		{
			_localScaleXSnapshotValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheDefinition)
		{
			_worldHitRectCacheDefinition = VariantUtils.ConvertTo<CharacterHitBoxDefinition>(in value);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheValid)
		{
			_worldHitRectCacheValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitBoxBoundsRevision)
		{
			_hitBoxBoundsRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._renderAncestorScaleSnapshot)
		{
			_renderAncestorScaleSnapshot = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._renderAncestorScaleSnapshotValid)
		{
			_renderAncestorScaleSnapshotValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mainStateMachineDispatchEnabled)
		{
			_mainStateMachineDispatchEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ownerBatchRegistered)
		{
			_ownerBatchRegistered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._batchUsesInheritedProcessMode)
		{
			_batchUsesInheritedProcessMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._batchProcessModeParent)
		{
			_batchProcessModeParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._mainStateMachineReleased)
		{
			_mainStateMachineReleased = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._authoritativeMainStateRestoreDepth)
		{
			_authoritativeMainStateRestoreDepth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._characterSkinSwitchedEventBus)
		{
			_characterSkinSwitchedEventBus = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		if (name == PropertyName._characterSkinSwitchedSubscribed)
		{
			_characterSkinSwitchedSubscribed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshot)
		{
			_pendingMainStateMachineSnapshot = VariantUtils.ConvertTo<StateMachineSnapshot>(in value);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshotIsRemote)
		{
			_pendingMainStateMachineSnapshotIsRemote = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshotSuppressEffects)
		{
			_pendingMainStateMachineSnapshotSuppressEffects = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshotProgressCompatible)
		{
			_pendingMainStateMachineSnapshotProgressCompatible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastRemoteMainStateMachineRevision)
		{
			_lastRemoteMainStateMachineRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastMainStateMachineProcessFrame)
		{
			_lastMainStateMachineProcessFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._lastComponentStateMachineProcessFrame)
		{
			_lastComponentStateMachineProcessFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._lastMainStateMachinePhysicsFrame)
		{
			_lastMainStateMachinePhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactDisplacementActive)
		{
			_bowlingImpactDisplacementActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactStartGlobalX)
		{
			_bowlingImpactStartGlobalX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactTargetGlobalX)
		{
			_bowlingImpactTargetGlobalX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactElapsed)
		{
			_bowlingImpactElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactDuration)
		{
			_bowlingImpactDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._characterStateHandlesConnected)
		{
			_characterStateHandlesConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showHealthOffset)
		{
			showHealthOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.componentManager)
		{
			componentManager = VariantUtils.ConvertTo<ComponentManager>(in value);
			return true;
		}
		if (name == PropertyName._resourceComponentFacadesCurrent)
		{
			_resourceComponentFacadesCurrent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._invisible)
		{
			_invisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.idleAnimeClip)
		{
			idleAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.sleepAnimeClip)
		{
			sleepAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._headSlot)
		{
			_headSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._camp)
		{
			_camp = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in value);
			return true;
		}
		if (name == PropertyName._damagePartClip)
		{
			_damagePartClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._damagePart)
		{
			_damagePart = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._damagePartSlot)
		{
			_damagePartSlot = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._previewDamagePointPersontage)
		{
			_previewDamagePointPersontage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._currentArmor)
		{
			_currentArmor = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._currentCustom)
		{
			_currentCustom = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.instance)
		{
			instance = VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in value);
			return true;
		}
		if (name == PropertyName.characterDisabled)
		{
			characterDisabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._componentAlive)
		{
			_componentAlive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.componentRunning)
		{
			componentRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.characterFilter)
		{
			characterFilter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timeScaleInit)
		{
			timeScaleInit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.timeScaleSave)
		{
			timeScaleSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.inGame)
		{
			inGame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			editorPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.editorMapPreviewMode)
		{
			editorMapPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.forceLocalRenderDuringZMotion)
		{
			forceLocalRenderDuringZMotion = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isShow)
		{
			isShow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packet)
		{
			packet = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.nearDie)
		{
			nearDie = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.die)
		{
			die = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canMowerMove)
		{
			canMowerMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.baseSpriteScale)
		{
			baseSpriteScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.isRise)
		{
			isRise = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isShovel)
		{
			isShovel = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isSmash)
		{
			isSmash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isExplode)
		{
			isExplode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isChomp)
		{
			isChomp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.skipDestroySet)
		{
			skipDestroySet = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.suppressDeathrattles)
		{
			suppressDeathrattles = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mowerDeathVisualOwned)
		{
			mowerDeathVisualOwned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._inWater)
		{
			_inWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.iceSpeedDown)
		{
			iceSpeedDown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.emSpeedDown)
		{
			emSpeedDown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useIdleAnimeReset)
		{
			useIdleAnimeReset = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._syncApplyingAnimation)
		{
			_syncApplyingAnimation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isUnlimitedFire)
		{
			isUnlimitedFire = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.randFreshIndex)
		{
			randFreshIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.groundRight)
		{
			groundRight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._componentGameplayUntilBattlefieldEntry)
		{
			_componentGameplayUntilBattlefieldEntry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._componentGameplayPhysicsDispatchActive)
		{
			_componentGameplayPhysicsDispatchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._componentGameplayPhysicsSnapshot)
		{
			_componentGameplayPhysicsSnapshot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._componentGameplayPhysicsSnapshotFrame)
		{
			_componentGameplayPhysicsSnapshotFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._physicsGlobalPositionFrame)
		{
			_physicsGlobalPositionFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._physicsGlobalPosition)
		{
			_physicsGlobalPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._shadowGlobalTransform)
		{
			_shadowGlobalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._shadowGlobalTransformValid)
		{
			_shadowGlobalTransformValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._physicsLocalPosition)
		{
			_physicsLocalPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._physicsLocalPositionValid)
		{
			_physicsLocalPositionValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._writingCachedGlobalPosition)
		{
			_writingCachedGlobalPosition = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingCachedGlobalTransform)
		{
			_pendingCachedGlobalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._pendingCachedGlobalTransformNotification)
		{
			_pendingCachedGlobalTransformNotification = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cellMoveTween)
		{
			_cellMoveTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._zMotionOwnsLocalRender)
		{
			_zMotionOwnsLocalRender = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._zMotionStablePhysicsFrames)
		{
			_zMotionStablePhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._spriteGroupLocalX)
		{
			_spriteGroupLocalX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._spriteGroupLocalXCaptured)
		{
			_spriteGroupLocalXCaptured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isDestroy)
		{
			isDestroy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._progressRestorePresentationRefreshPending)
		{
			_progressRestorePresentationRefreshPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._characterExternalVisualsInitialized)
		{
			_characterExternalVisualsInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._legacySpriteGroupVisualsScanned)
		{
			_legacySpriteGroupVisualsScanned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsRemoteNetworkReplica)
		{
			from = IsRemoteNetworkReplica;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasValidRuntimeConfiguration)
		{
			from = HasValidRuntimeConfiguration;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasArmorHurtSubscribers)
		{
			from = HasArmorHurtSubscribers;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.icetrapSprite)
		{
			value = VariantUtils.CreateFrom<Sprite2D>(icetrapSprite);
			return true;
		}
		if (name == PropertyName.HitBoxDefinition)
		{
			value = VariantUtils.CreateFrom<CharacterHitBoxDefinition>(HitBoxDefinition);
			return true;
		}
		if (name == PropertyName.HitBoxBoundsRevision)
		{
			value = VariantUtils.CreateFrom<ulong>(HitBoxBoundsRevision);
			return true;
		}
		if (name == PropertyName.HasHitBox)
		{
			from = HasHitBox;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsHitBoxEnabled)
		{
			from = IsHitBoxEnabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsHitBoxMonitorable)
		{
			from = IsHitBoxMonitorable;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Rect2 from2;
		if (name == PropertyName.WorldBroadphaseRect)
		{
			from2 = WorldBroadphaseRect;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CachedLocalScaleX)
		{
			value = VariantUtils.CreateFrom<float>(CachedLocalScaleX);
			return true;
		}
		if (name == PropertyName.WorldHitRect)
		{
			from2 = WorldHitRect;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MainStateMachineDefinition)
		{
			value = VariantUtils.CreateFrom<StateMachineDefinition>(MainStateMachineDefinition);
			return true;
		}
		if (name == PropertyName.WantsMainStateMachineProcessDispatch)
		{
			from = WantsMainStateMachineProcessDispatch;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsBowlingImpactDisplacementActive)
		{
			from = IsBowlingImpactDisplacementActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsOwnerBatchRegistered)
		{
			from = IsOwnerBatchRegistered;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BatchUsesInheritedProcessMode)
		{
			from = BatchUsesInheritedProcessMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BatchProcessModeParent)
		{
			value = VariantUtils.CreateFrom<Node>(BatchProcessModeParent);
			return true;
		}
		if (name == PropertyName.IsOwnerBatchDispatchActive)
		{
			from = IsOwnerBatchDispatchActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CanDispatchMainStateMachine)
		{
			from = CanDispatchMainStateMachine;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CanDispatchOwnedStateMachines)
		{
			from = CanDispatchOwnedStateMachines;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ComponentSet)
		{
			value = VariantUtils.CreateFrom<CharacterComponentSet>(ComponentSet);
			return true;
		}
		if (name == PropertyName.invisible)
		{
			from = invisible;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom<TowerDefenseCharacterConfig>(config);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSprite>(sprite);
			return true;
		}
		if (name == PropertyName.headSlot)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSlot>(headSlot);
			return true;
		}
		if (name == PropertyName.HasEconomyOwner)
		{
			from = HasEconomyOwner;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.camp)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.CHARACTER_CAMP>(camp);
			return true;
		}
		if (name == PropertyName.damagePartClip)
		{
			value = VariantUtils.CreateFrom<string>(damagePartClip);
			return true;
		}
		Dictionary from3;
		if (name == PropertyName.damagePart)
		{
			from3 = damagePart;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.damagePartSlot)
		{
			from3 = damagePartSlot;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.previewDamagePointPersontage)
		{
			value = VariantUtils.CreateFrom<double>(previewDamagePointPersontage);
			return true;
		}
		if (name == PropertyName.currentArmor)
		{
			value = VariantUtils.CreateFromArray(currentArmor);
			return true;
		}
		if (name == PropertyName.currentCustom)
		{
			value = VariantUtils.CreateFromArray(currentCustom);
			return true;
		}
		if (name == PropertyName.componentAlive)
		{
			from = componentAlive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.inWater)
		{
			from = inWater;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasComponentGameplayUntilBattlefieldEntry)
		{
			from = HasComponentGameplayUntilBattlefieldEntry;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsInsideComponentBattlefield)
		{
			from = IsInsideComponentBattlefield;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsInsideComponentBattlefieldForRuntime)
		{
			from = IsInsideComponentBattlefieldForRuntime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PreserveDeathTransformation)
		{
			from = PreserveDeathTransformation;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsHardControlImmune)
		{
			from = IsHardControlImmune;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsProgressRestoreInFlight)
		{
			from = IsProgressRestoreInFlight;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from4;
		if (name == PropertyName.CharacterExternalVisualCompatibilityScanCount)
		{
			from4 = CharacterExternalVisualCompatibilityScanCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.CharacterAnimatedStatusVisualCount)
		{
			from4 = CharacterAnimatedStatusVisualCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.CharacterAnimatedStatusTopologyChangeCount)
		{
			from4 = CharacterAnimatedStatusTopologyChangeCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.syncId)
		{
			value = VariantUtils.CreateFrom(in syncId);
			return true;
		}
		if (name == PropertyName.backEffectNode)
		{
			value = VariantUtils.CreateFrom(in backEffectNode);
			return true;
		}
		if (name == PropertyName.frontEffectNode)
		{
			value = VariantUtils.CreateFrom(in frontEffectNode);
			return true;
		}
		if (name == PropertyName.spriteGroup)
		{
			value = VariantUtils.CreateFrom(in spriteGroup);
			return true;
		}
		if (name == PropertyName.transformPoint)
		{
			value = VariantUtils.CreateFrom(in transformPoint);
			return true;
		}
		if (name == PropertyName.shadowSprite)
		{
			value = VariantUtils.CreateFrom(in shadowSprite);
			return true;
		}
		if (name == PropertyName._hitBoxDefinition)
		{
			value = VariantUtils.CreateFrom(in _hitBoxDefinition);
			return true;
		}
		if (name == PropertyName._hitBoxRuntimeInitialized)
		{
			value = VariantUtils.CreateFrom(in _hitBoxRuntimeInitialized);
			return true;
		}
		if (name == PropertyName._hitBoxAvailable)
		{
			value = VariantUtils.CreateFrom(in _hitBoxAvailable);
			return true;
		}
		if (name == PropertyName._hitBoxDefaultEnabled)
		{
			value = VariantUtils.CreateFrom(in _hitBoxDefaultEnabled);
			return true;
		}
		if (name == PropertyName._hitBoxDefaultMonitorable)
		{
			value = VariantUtils.CreateFrom(in _hitBoxDefaultMonitorable);
			return true;
		}
		if (name == PropertyName._hitBoxSuppression)
		{
			value = VariantUtils.CreateFrom(in _hitBoxSuppression);
			return true;
		}
		if (name == PropertyName._hitBoxMonitorSuppression)
		{
			value = VariantUtils.CreateFrom(in _hitBoxMonitorSuppression);
			return true;
		}
		if (name == PropertyName._canReceiveExplosionHit)
		{
			value = VariantUtils.CreateFrom(in _canReceiveExplosionHit);
			return true;
		}
		if (name == PropertyName._hitBoxPreviewDraw)
		{
			value = VariantUtils.CreateFrom(in _hitBoxPreviewDraw);
			return true;
		}
		if (name == PropertyName._worldHitRectCache)
		{
			value = VariantUtils.CreateFrom(in _worldHitRectCache);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheFrame)
		{
			value = VariantUtils.CreateFrom(in _worldHitRectCacheFrame);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheTransform)
		{
			value = VariantUtils.CreateFrom(in _worldHitRectCacheTransform);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheScaleX)
		{
			value = VariantUtils.CreateFrom(in _worldHitRectCacheScaleX);
			return true;
		}
		if (name == PropertyName._localScaleXSnapshot)
		{
			value = VariantUtils.CreateFrom(in _localScaleXSnapshot);
			return true;
		}
		if (name == PropertyName._localScaleXSnapshotValid)
		{
			value = VariantUtils.CreateFrom(in _localScaleXSnapshotValid);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheDefinition)
		{
			value = VariantUtils.CreateFrom(in _worldHitRectCacheDefinition);
			return true;
		}
		if (name == PropertyName._worldHitRectCacheValid)
		{
			value = VariantUtils.CreateFrom(in _worldHitRectCacheValid);
			return true;
		}
		if (name == PropertyName._hitBoxBoundsRevision)
		{
			value = VariantUtils.CreateFrom(in _hitBoxBoundsRevision);
			return true;
		}
		if (name == PropertyName._renderAncestorScaleSnapshot)
		{
			value = VariantUtils.CreateFrom(in _renderAncestorScaleSnapshot);
			return true;
		}
		if (name == PropertyName._renderAncestorScaleSnapshotValid)
		{
			value = VariantUtils.CreateFrom(in _renderAncestorScaleSnapshotValid);
			return true;
		}
		if (name == PropertyName._mainStateMachineDispatchEnabled)
		{
			value = VariantUtils.CreateFrom(in _mainStateMachineDispatchEnabled);
			return true;
		}
		if (name == PropertyName._ownerBatchRegistered)
		{
			value = VariantUtils.CreateFrom(in _ownerBatchRegistered);
			return true;
		}
		if (name == PropertyName._batchUsesInheritedProcessMode)
		{
			value = VariantUtils.CreateFrom(in _batchUsesInheritedProcessMode);
			return true;
		}
		if (name == PropertyName._batchProcessModeParent)
		{
			value = VariantUtils.CreateFrom(in _batchProcessModeParent);
			return true;
		}
		if (name == PropertyName._mainStateMachineReleased)
		{
			value = VariantUtils.CreateFrom(in _mainStateMachineReleased);
			return true;
		}
		if (name == PropertyName._authoritativeMainStateRestoreDepth)
		{
			value = VariantUtils.CreateFrom(in _authoritativeMainStateRestoreDepth);
			return true;
		}
		if (name == PropertyName._characterSkinSwitchedEventBus)
		{
			value = VariantUtils.CreateFrom(in _characterSkinSwitchedEventBus);
			return true;
		}
		if (name == PropertyName._characterSkinSwitchedSubscribed)
		{
			value = VariantUtils.CreateFrom(in _characterSkinSwitchedSubscribed);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshot)
		{
			value = VariantUtils.CreateFrom(in _pendingMainStateMachineSnapshot);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshotIsRemote)
		{
			value = VariantUtils.CreateFrom(in _pendingMainStateMachineSnapshotIsRemote);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshotSuppressEffects)
		{
			value = VariantUtils.CreateFrom(in _pendingMainStateMachineSnapshotSuppressEffects);
			return true;
		}
		if (name == PropertyName._pendingMainStateMachineSnapshotProgressCompatible)
		{
			value = VariantUtils.CreateFrom(in _pendingMainStateMachineSnapshotProgressCompatible);
			return true;
		}
		if (name == PropertyName._lastRemoteMainStateMachineRevision)
		{
			value = VariantUtils.CreateFrom(in _lastRemoteMainStateMachineRevision);
			return true;
		}
		if (name == PropertyName._lastMainStateMachineProcessFrame)
		{
			value = VariantUtils.CreateFrom(in _lastMainStateMachineProcessFrame);
			return true;
		}
		if (name == PropertyName._lastComponentStateMachineProcessFrame)
		{
			value = VariantUtils.CreateFrom(in _lastComponentStateMachineProcessFrame);
			return true;
		}
		if (name == PropertyName._lastMainStateMachinePhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _lastMainStateMachinePhysicsFrame);
			return true;
		}
		if (name == PropertyName._bowlingImpactDisplacementActive)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactDisplacementActive);
			return true;
		}
		if (name == PropertyName._bowlingImpactStartGlobalX)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactStartGlobalX);
			return true;
		}
		if (name == PropertyName._bowlingImpactTargetGlobalX)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactTargetGlobalX);
			return true;
		}
		if (name == PropertyName._bowlingImpactElapsed)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactElapsed);
			return true;
		}
		if (name == PropertyName._bowlingImpactDuration)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactDuration);
			return true;
		}
		if (name == PropertyName._characterStateHandlesConnected)
		{
			value = VariantUtils.CreateFrom(in _characterStateHandlesConnected);
			return true;
		}
		if (name == PropertyName.showHealthOffset)
		{
			value = VariantUtils.CreateFrom(in showHealthOffset);
			return true;
		}
		if (name == PropertyName.componentManager)
		{
			value = VariantUtils.CreateFrom(in componentManager);
			return true;
		}
		if (name == PropertyName._resourceComponentFacadesCurrent)
		{
			value = VariantUtils.CreateFrom(in _resourceComponentFacadesCurrent);
			return true;
		}
		if (name == PropertyName._invisible)
		{
			value = VariantUtils.CreateFrom(in _invisible);
			return true;
		}
		if (name == PropertyName.idleAnimeClip)
		{
			value = VariantUtils.CreateFrom(in idleAnimeClip);
			return true;
		}
		if (name == PropertyName.sleepAnimeClip)
		{
			value = VariantUtils.CreateFrom(in sleepAnimeClip);
			return true;
		}
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName._headSlot)
		{
			value = VariantUtils.CreateFrom(in _headSlot);
			return true;
		}
		if (name == PropertyName._camp)
		{
			value = VariantUtils.CreateFrom(in _camp);
			return true;
		}
		if (name == PropertyName._damagePartClip)
		{
			value = VariantUtils.CreateFrom(in _damagePartClip);
			return true;
		}
		if (name == PropertyName._damagePart)
		{
			value = VariantUtils.CreateFrom(in _damagePart);
			return true;
		}
		if (name == PropertyName._damagePartSlot)
		{
			value = VariantUtils.CreateFrom(in _damagePartSlot);
			return true;
		}
		if (name == PropertyName._previewDamagePointPersontage)
		{
			value = VariantUtils.CreateFrom(in _previewDamagePointPersontage);
			return true;
		}
		if (name == PropertyName._currentArmor)
		{
			value = VariantUtils.CreateFromArray(_currentArmor);
			return true;
		}
		if (name == PropertyName._currentCustom)
		{
			value = VariantUtils.CreateFromArray(_currentCustom);
			return true;
		}
		if (name == PropertyName.instance)
		{
			value = VariantUtils.CreateFrom(in instance);
			return true;
		}
		if (name == PropertyName.characterDisabled)
		{
			value = VariantUtils.CreateFrom(in characterDisabled);
			return true;
		}
		if (name == PropertyName._componentAlive)
		{
			value = VariantUtils.CreateFrom(in _componentAlive);
			return true;
		}
		if (name == PropertyName.componentRunning)
		{
			value = VariantUtils.CreateFrom(in componentRunning);
			return true;
		}
		if (name == PropertyName.characterFilter)
		{
			value = VariantUtils.CreateFrom(in characterFilter);
			return true;
		}
		if (name == PropertyName.timeScaleInit)
		{
			value = VariantUtils.CreateFrom(in timeScaleInit);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			value = VariantUtils.CreateFrom(in timeScale);
			return true;
		}
		if (name == PropertyName.timeScaleSave)
		{
			value = VariantUtils.CreateFrom(in timeScaleSave);
			return true;
		}
		if (name == PropertyName.inGame)
		{
			value = VariantUtils.CreateFrom(in inGame);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			value = VariantUtils.CreateFrom(in editorPreviewMode);
			return true;
		}
		if (name == PropertyName.editorMapPreviewMode)
		{
			value = VariantUtils.CreateFrom(in editorMapPreviewMode);
			return true;
		}
		if (name == PropertyName.forceLocalRenderDuringZMotion)
		{
			value = VariantUtils.CreateFrom(in forceLocalRenderDuringZMotion);
			return true;
		}
		if (name == PropertyName.isShow)
		{
			value = VariantUtils.CreateFrom(in isShow);
			return true;
		}
		if (name == PropertyName.packet)
		{
			value = VariantUtils.CreateFrom(in packet);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.nearDie)
		{
			value = VariantUtils.CreateFrom(in nearDie);
			return true;
		}
		if (name == PropertyName.die)
		{
			value = VariantUtils.CreateFrom(in die);
			return true;
		}
		if (name == PropertyName.canMowerMove)
		{
			value = VariantUtils.CreateFrom(in canMowerMove);
			return true;
		}
		if (name == PropertyName.baseSpriteScale)
		{
			value = VariantUtils.CreateFrom(in baseSpriteScale);
			return true;
		}
		if (name == PropertyName.isRise)
		{
			value = VariantUtils.CreateFrom(in isRise);
			return true;
		}
		if (name == PropertyName.isShovel)
		{
			value = VariantUtils.CreateFrom(in isShovel);
			return true;
		}
		if (name == PropertyName.isSmash)
		{
			value = VariantUtils.CreateFrom(in isSmash);
			return true;
		}
		if (name == PropertyName.isExplode)
		{
			value = VariantUtils.CreateFrom(in isExplode);
			return true;
		}
		if (name == PropertyName.isChomp)
		{
			value = VariantUtils.CreateFrom(in isChomp);
			return true;
		}
		if (name == PropertyName.skipDestroySet)
		{
			value = VariantUtils.CreateFrom(in skipDestroySet);
			return true;
		}
		if (name == PropertyName.suppressDeathrattles)
		{
			value = VariantUtils.CreateFrom(in suppressDeathrattles);
			return true;
		}
		if (name == PropertyName.mowerDeathVisualOwned)
		{
			value = VariantUtils.CreateFrom(in mowerDeathVisualOwned);
			return true;
		}
		if (name == PropertyName._inWater)
		{
			value = VariantUtils.CreateFrom(in _inWater);
			return true;
		}
		if (name == PropertyName.iceSpeedDown)
		{
			value = VariantUtils.CreateFrom(in iceSpeedDown);
			return true;
		}
		if (name == PropertyName.emSpeedDown)
		{
			value = VariantUtils.CreateFrom(in emSpeedDown);
			return true;
		}
		if (name == PropertyName.useIdleAnimeReset)
		{
			value = VariantUtils.CreateFrom(in useIdleAnimeReset);
			return true;
		}
		if (name == PropertyName._syncApplyingAnimation)
		{
			value = VariantUtils.CreateFrom(in _syncApplyingAnimation);
			return true;
		}
		if (name == PropertyName.isUnlimitedFire)
		{
			value = VariantUtils.CreateFrom(in isUnlimitedFire);
			return true;
		}
		if (name == PropertyName.randFreshIndex)
		{
			value = VariantUtils.CreateFrom(in randFreshIndex);
			return true;
		}
		if (name == PropertyName.groundRight)
		{
			value = VariantUtils.CreateFrom(in groundRight);
			return true;
		}
		if (name == PropertyName._componentGameplayUntilBattlefieldEntry)
		{
			value = VariantUtils.CreateFrom(in _componentGameplayUntilBattlefieldEntry);
			return true;
		}
		if (name == PropertyName._componentGameplayPhysicsDispatchActive)
		{
			value = VariantUtils.CreateFrom(in _componentGameplayPhysicsDispatchActive);
			return true;
		}
		if (name == PropertyName._componentGameplayPhysicsSnapshot)
		{
			value = VariantUtils.CreateFrom(in _componentGameplayPhysicsSnapshot);
			return true;
		}
		if (name == PropertyName._componentGameplayPhysicsSnapshotFrame)
		{
			value = VariantUtils.CreateFrom(in _componentGameplayPhysicsSnapshotFrame);
			return true;
		}
		if (name == PropertyName._physicsGlobalPositionFrame)
		{
			value = VariantUtils.CreateFrom(in _physicsGlobalPositionFrame);
			return true;
		}
		if (name == PropertyName._physicsGlobalPosition)
		{
			value = VariantUtils.CreateFrom(in _physicsGlobalPosition);
			return true;
		}
		if (name == PropertyName._shadowGlobalTransform)
		{
			value = VariantUtils.CreateFrom(in _shadowGlobalTransform);
			return true;
		}
		if (name == PropertyName._shadowGlobalTransformValid)
		{
			value = VariantUtils.CreateFrom(in _shadowGlobalTransformValid);
			return true;
		}
		if (name == PropertyName._physicsLocalPosition)
		{
			value = VariantUtils.CreateFrom(in _physicsLocalPosition);
			return true;
		}
		if (name == PropertyName._physicsLocalPositionValid)
		{
			value = VariantUtils.CreateFrom(in _physicsLocalPositionValid);
			return true;
		}
		if (name == PropertyName._writingCachedGlobalPosition)
		{
			value = VariantUtils.CreateFrom(in _writingCachedGlobalPosition);
			return true;
		}
		if (name == PropertyName._pendingCachedGlobalTransform)
		{
			value = VariantUtils.CreateFrom(in _pendingCachedGlobalTransform);
			return true;
		}
		if (name == PropertyName._pendingCachedGlobalTransformNotification)
		{
			value = VariantUtils.CreateFrom(in _pendingCachedGlobalTransformNotification);
			return true;
		}
		if (name == PropertyName._cellMoveTween)
		{
			value = VariantUtils.CreateFrom(in _cellMoveTween);
			return true;
		}
		if (name == PropertyName._zMotionOwnsLocalRender)
		{
			value = VariantUtils.CreateFrom(in _zMotionOwnsLocalRender);
			return true;
		}
		if (name == PropertyName._zMotionStablePhysicsFrames)
		{
			value = VariantUtils.CreateFrom(in _zMotionStablePhysicsFrames);
			return true;
		}
		if (name == PropertyName._spriteGroupLocalX)
		{
			value = VariantUtils.CreateFrom(in _spriteGroupLocalX);
			return true;
		}
		if (name == PropertyName._spriteGroupLocalXCaptured)
		{
			value = VariantUtils.CreateFrom(in _spriteGroupLocalXCaptured);
			return true;
		}
		if (name == PropertyName.isDestroy)
		{
			value = VariantUtils.CreateFrom(in isDestroy);
			return true;
		}
		if (name == PropertyName._progressRestorePresentationRefreshPending)
		{
			value = VariantUtils.CreateFrom(in _progressRestorePresentationRefreshPending);
			return true;
		}
		if (name == PropertyName._characterExternalVisualsInitialized)
		{
			value = VariantUtils.CreateFrom(in _characterExternalVisualsInitialized);
			return true;
		}
		if (name == PropertyName._legacySpriteGroupVisualsScanned)
		{
			value = VariantUtils.CreateFrom(in _legacySpriteGroupVisualsScanned);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.syncId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRemoteNetworkReplica, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasValidRuntimeConfiguration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasArmorHurtSubscribers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backEffectNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.frontEffectNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteGroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.transformPoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shadowSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.icetrapSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hitBoxDefinition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.HitBoxDefinition, PropertyHint.ResourceType, "CharacterHitBoxDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitBoxRuntimeInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitBoxAvailable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitBoxDefaultEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitBoxDefaultMonitorable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hitBoxSuppression, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hitBoxMonitorSuppression, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._canReceiveExplosionHit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitBoxPreviewDraw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._worldHitRectCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._worldHitRectCacheFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._worldHitRectCacheTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._worldHitRectCacheScaleX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._localScaleXSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._localScaleXSnapshotValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._worldHitRectCacheDefinition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._worldHitRectCacheValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hitBoxBoundsRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.HitBoxBoundsRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasHitBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHitBoxEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHitBoxMonitorable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldBroadphaseRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.CachedLocalScaleX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldHitRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._renderAncestorScaleSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderAncestorScaleSnapshotValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MainStateMachineDefinition, PropertyHint.ResourceType, "StateMachineDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mainStateMachineDispatchEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ownerBatchRegistered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._batchUsesInheritedProcessMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._batchProcessModeParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mainStateMachineReleased, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._authoritativeMainStateRestoreDepth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterSkinSwitchedEventBus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._characterSkinSwitchedSubscribed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingMainStateMachineSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingMainStateMachineSnapshotIsRemote, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingMainStateMachineSnapshotSuppressEffects, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingMainStateMachineSnapshotProgressCompatible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastRemoteMainStateMachineRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastMainStateMachineProcessFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastComponentStateMachineProcessFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastMainStateMachinePhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bowlingImpactDisplacementActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactStartGlobalX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactTargetGlobalX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._characterStateHandlesConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.WantsMainStateMachineProcessDispatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsBowlingImpactDisplacementActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsOwnerBatchRegistered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.BatchUsesInheritedProcessMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.BatchProcessModeParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsOwnerBatchDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanDispatchMainStateMachine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanDispatchOwnedStateMachines, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.showHealthOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.ComponentSet, PropertyHint.ResourceType, "CharacterComponentSet", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.componentManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resourceComponentFacadesCurrent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._invisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.invisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.idleAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.sleepAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.ResourceType, "TowerDefenseCharacterConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._headSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.headSlot, PropertyHint.NodeType, "AdobeAnimateSlot", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._camp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasEconomyOwner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.camp, PropertyHint.Enum, "NOONE:-1,PLANT:0,ZOMBIE:1,ALL:2", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._damagePartClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.damagePartClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._damagePart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.damagePart, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._damagePartSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.damagePartSlot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewDamagePointPersontage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.previewDamagePointPersontage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._currentArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentArmor, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._currentCustom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentCustom, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.instance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.characterDisabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._componentAlive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.componentAlive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.componentRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.characterFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScaleInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScaleSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.inGame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorPreviewMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorMapPreviewMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.forceLocalRenderDuringZMotion, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.nearDie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.die, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canMowerMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.baseSpriteScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRise, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isShovel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSmash, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isExplode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isChomp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skipDestroySet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suppressDeathrattles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerDeathVisualOwned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._inWater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.inWater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.iceSpeedDown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.emSpeedDown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useIdleAnimeReset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._syncApplyingAnimation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isUnlimitedFire, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.randFreshIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.groundRight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._componentGameplayUntilBattlefieldEntry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._componentGameplayPhysicsDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._componentGameplayPhysicsSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._componentGameplayPhysicsSnapshotFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._physicsGlobalPositionFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._physicsGlobalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._shadowGlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._shadowGlobalTransformValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._physicsLocalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._physicsLocalPositionValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._writingCachedGlobalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._pendingCachedGlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingCachedGlobalTransformNotification, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cellMoveTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasComponentGameplayUntilBattlefieldEntry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInsideComponentBattlefield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInsideComponentBattlefieldForRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zMotionOwnsLocalRender, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._zMotionStablePhysicsFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spriteGroupLocalX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spriteGroupLocalXCaptured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PreserveDeathTransformation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isDestroy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHardControlImmune, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsProgressRestoreInFlight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._progressRestorePresentationRefreshPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._characterExternalVisualsInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._legacySpriteGroupVisualsScanned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CharacterExternalVisualCompatibilityScanCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CharacterAnimatedStatusVisualCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CharacterAnimatedStatusTopologyChangeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.HasValidRuntimeConfiguration, Variant.From<bool>(HasValidRuntimeConfiguration));
		info.AddProperty(PropertyName.HitBoxDefinition, Variant.From<CharacterHitBoxDefinition>(HitBoxDefinition));
		info.AddProperty(PropertyName.MainStateMachineDefinition, Variant.From<StateMachineDefinition>(MainStateMachineDefinition));
		info.AddProperty(PropertyName.ComponentSet, Variant.From<CharacterComponentSet>(ComponentSet));
		info.AddProperty(PropertyName.invisible, Variant.From<bool>(invisible));
		info.AddProperty(PropertyName.config, Variant.From<TowerDefenseCharacterConfig>(config));
		info.AddProperty(PropertyName.sprite, Variant.From<AdobeAnimateSprite>(sprite));
		info.AddProperty(PropertyName.headSlot, Variant.From<AdobeAnimateSlot>(headSlot));
		info.AddProperty(PropertyName.camp, Variant.From<TowerDefenseEnum.CHARACTER_CAMP>(camp));
		info.AddProperty(PropertyName.damagePartClip, Variant.From<string>(damagePartClip));
		info.AddProperty(PropertyName.damagePart, Variant.From<Dictionary>(damagePart));
		info.AddProperty(PropertyName.damagePartSlot, Variant.From<Dictionary>(damagePartSlot));
		info.AddProperty(PropertyName.previewDamagePointPersontage, Variant.From<double>(previewDamagePointPersontage));
		info.AddProperty(PropertyName.currentArmor, Variant.CreateFrom(currentArmor));
		info.AddProperty(PropertyName.currentCustom, Variant.CreateFrom(currentCustom));
		info.AddProperty(PropertyName.componentAlive, Variant.From<bool>(componentAlive));
		info.AddProperty(PropertyName.inWater, Variant.From<bool>(inWater));
		info.AddProperty(PropertyName.IsProgressRestoreInFlight, Variant.From<bool>(IsProgressRestoreInFlight));
		info.AddProperty(PropertyName.CharacterExternalVisualCompatibilityScanCount, Variant.From<int>(CharacterExternalVisualCompatibilityScanCount));
		info.AddProperty(PropertyName.CharacterAnimatedStatusVisualCount, Variant.From<int>(CharacterAnimatedStatusVisualCount));
		info.AddProperty(PropertyName.CharacterAnimatedStatusTopologyChangeCount, Variant.From<int>(CharacterAnimatedStatusTopologyChangeCount));
		info.AddProperty(PropertyName.syncId, Variant.From(in syncId));
		info.AddProperty(PropertyName.backEffectNode, Variant.From(in backEffectNode));
		info.AddProperty(PropertyName.frontEffectNode, Variant.From(in frontEffectNode));
		info.AddProperty(PropertyName.spriteGroup, Variant.From(in spriteGroup));
		info.AddProperty(PropertyName.transformPoint, Variant.From(in transformPoint));
		info.AddProperty(PropertyName.shadowSprite, Variant.From(in shadowSprite));
		info.AddProperty(PropertyName._hitBoxDefinition, Variant.From(in _hitBoxDefinition));
		info.AddProperty(PropertyName._hitBoxRuntimeInitialized, Variant.From(in _hitBoxRuntimeInitialized));
		info.AddProperty(PropertyName._hitBoxAvailable, Variant.From(in _hitBoxAvailable));
		info.AddProperty(PropertyName._hitBoxDefaultEnabled, Variant.From(in _hitBoxDefaultEnabled));
		info.AddProperty(PropertyName._hitBoxDefaultMonitorable, Variant.From(in _hitBoxDefaultMonitorable));
		info.AddProperty(PropertyName._hitBoxSuppression, Variant.From(in _hitBoxSuppression));
		info.AddProperty(PropertyName._hitBoxMonitorSuppression, Variant.From(in _hitBoxMonitorSuppression));
		info.AddProperty(PropertyName._canReceiveExplosionHit, Variant.From(in _canReceiveExplosionHit));
		info.AddProperty(PropertyName._hitBoxPreviewDraw, Variant.From(in _hitBoxPreviewDraw));
		info.AddProperty(PropertyName._worldHitRectCache, Variant.From(in _worldHitRectCache));
		info.AddProperty(PropertyName._worldHitRectCacheFrame, Variant.From(in _worldHitRectCacheFrame));
		info.AddProperty(PropertyName._worldHitRectCacheTransform, Variant.From(in _worldHitRectCacheTransform));
		info.AddProperty(PropertyName._worldHitRectCacheScaleX, Variant.From(in _worldHitRectCacheScaleX));
		info.AddProperty(PropertyName._localScaleXSnapshot, Variant.From(in _localScaleXSnapshot));
		info.AddProperty(PropertyName._localScaleXSnapshotValid, Variant.From(in _localScaleXSnapshotValid));
		info.AddProperty(PropertyName._worldHitRectCacheDefinition, Variant.From(in _worldHitRectCacheDefinition));
		info.AddProperty(PropertyName._worldHitRectCacheValid, Variant.From(in _worldHitRectCacheValid));
		info.AddProperty(PropertyName._hitBoxBoundsRevision, Variant.From(in _hitBoxBoundsRevision));
		info.AddProperty(PropertyName._renderAncestorScaleSnapshot, Variant.From(in _renderAncestorScaleSnapshot));
		info.AddProperty(PropertyName._renderAncestorScaleSnapshotValid, Variant.From(in _renderAncestorScaleSnapshotValid));
		info.AddProperty(PropertyName._mainStateMachineDispatchEnabled, Variant.From(in _mainStateMachineDispatchEnabled));
		info.AddProperty(PropertyName._ownerBatchRegistered, Variant.From(in _ownerBatchRegistered));
		info.AddProperty(PropertyName._batchUsesInheritedProcessMode, Variant.From(in _batchUsesInheritedProcessMode));
		info.AddProperty(PropertyName._batchProcessModeParent, Variant.From(in _batchProcessModeParent));
		info.AddProperty(PropertyName._mainStateMachineReleased, Variant.From(in _mainStateMachineReleased));
		info.AddProperty(PropertyName._authoritativeMainStateRestoreDepth, Variant.From(in _authoritativeMainStateRestoreDepth));
		info.AddProperty(PropertyName._characterSkinSwitchedEventBus, Variant.From(in _characterSkinSwitchedEventBus));
		info.AddProperty(PropertyName._characterSkinSwitchedSubscribed, Variant.From(in _characterSkinSwitchedSubscribed));
		info.AddProperty(PropertyName._pendingMainStateMachineSnapshot, Variant.From(in _pendingMainStateMachineSnapshot));
		info.AddProperty(PropertyName._pendingMainStateMachineSnapshotIsRemote, Variant.From(in _pendingMainStateMachineSnapshotIsRemote));
		info.AddProperty(PropertyName._pendingMainStateMachineSnapshotSuppressEffects, Variant.From(in _pendingMainStateMachineSnapshotSuppressEffects));
		info.AddProperty(PropertyName._pendingMainStateMachineSnapshotProgressCompatible, Variant.From(in _pendingMainStateMachineSnapshotProgressCompatible));
		info.AddProperty(PropertyName._lastRemoteMainStateMachineRevision, Variant.From(in _lastRemoteMainStateMachineRevision));
		info.AddProperty(PropertyName._lastMainStateMachineProcessFrame, Variant.From(in _lastMainStateMachineProcessFrame));
		info.AddProperty(PropertyName._lastComponentStateMachineProcessFrame, Variant.From(in _lastComponentStateMachineProcessFrame));
		info.AddProperty(PropertyName._lastMainStateMachinePhysicsFrame, Variant.From(in _lastMainStateMachinePhysicsFrame));
		info.AddProperty(PropertyName._bowlingImpactDisplacementActive, Variant.From(in _bowlingImpactDisplacementActive));
		info.AddProperty(PropertyName._bowlingImpactStartGlobalX, Variant.From(in _bowlingImpactStartGlobalX));
		info.AddProperty(PropertyName._bowlingImpactTargetGlobalX, Variant.From(in _bowlingImpactTargetGlobalX));
		info.AddProperty(PropertyName._bowlingImpactElapsed, Variant.From(in _bowlingImpactElapsed));
		info.AddProperty(PropertyName._bowlingImpactDuration, Variant.From(in _bowlingImpactDuration));
		info.AddProperty(PropertyName._characterStateHandlesConnected, Variant.From(in _characterStateHandlesConnected));
		info.AddProperty(PropertyName.showHealthOffset, Variant.From(in showHealthOffset));
		info.AddProperty(PropertyName.componentManager, Variant.From(in componentManager));
		info.AddProperty(PropertyName._resourceComponentFacadesCurrent, Variant.From(in _resourceComponentFacadesCurrent));
		info.AddProperty(PropertyName._invisible, Variant.From(in _invisible));
		info.AddProperty(PropertyName.idleAnimeClip, Variant.From(in idleAnimeClip));
		info.AddProperty(PropertyName.sleepAnimeClip, Variant.From(in sleepAnimeClip));
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName._headSlot, Variant.From(in _headSlot));
		info.AddProperty(PropertyName._camp, Variant.From(in _camp));
		info.AddProperty(PropertyName._damagePartClip, Variant.From(in _damagePartClip));
		info.AddProperty(PropertyName._damagePart, Variant.From(in _damagePart));
		info.AddProperty(PropertyName._damagePartSlot, Variant.From(in _damagePartSlot));
		info.AddProperty(PropertyName._previewDamagePointPersontage, Variant.From(in _previewDamagePointPersontage));
		info.AddProperty(PropertyName._currentArmor, Variant.CreateFrom(_currentArmor));
		info.AddProperty(PropertyName._currentCustom, Variant.CreateFrom(_currentCustom));
		info.AddProperty(PropertyName.instance, Variant.From(in instance));
		info.AddProperty(PropertyName.characterDisabled, Variant.From(in characterDisabled));
		info.AddProperty(PropertyName._componentAlive, Variant.From(in _componentAlive));
		info.AddProperty(PropertyName.componentRunning, Variant.From(in componentRunning));
		info.AddProperty(PropertyName.characterFilter, Variant.From(in characterFilter));
		info.AddProperty(PropertyName.timeScaleInit, Variant.From(in timeScaleInit));
		info.AddProperty(PropertyName.timeScale, Variant.From(in timeScale));
		info.AddProperty(PropertyName.timeScaleSave, Variant.From(in timeScaleSave));
		info.AddProperty(PropertyName.inGame, Variant.From(in inGame));
		info.AddProperty(PropertyName.editorPreviewMode, Variant.From(in editorPreviewMode));
		info.AddProperty(PropertyName.editorMapPreviewMode, Variant.From(in editorMapPreviewMode));
		info.AddProperty(PropertyName.forceLocalRenderDuringZMotion, Variant.From(in forceLocalRenderDuringZMotion));
		info.AddProperty(PropertyName.isShow, Variant.From(in isShow));
		info.AddProperty(PropertyName.packet, Variant.From(in packet));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.nearDie, Variant.From(in nearDie));
		info.AddProperty(PropertyName.die, Variant.From(in die));
		info.AddProperty(PropertyName.canMowerMove, Variant.From(in canMowerMove));
		info.AddProperty(PropertyName.baseSpriteScale, Variant.From(in baseSpriteScale));
		info.AddProperty(PropertyName.isRise, Variant.From(in isRise));
		info.AddProperty(PropertyName.isShovel, Variant.From(in isShovel));
		info.AddProperty(PropertyName.isSmash, Variant.From(in isSmash));
		info.AddProperty(PropertyName.isExplode, Variant.From(in isExplode));
		info.AddProperty(PropertyName.isChomp, Variant.From(in isChomp));
		info.AddProperty(PropertyName.skipDestroySet, Variant.From(in skipDestroySet));
		info.AddProperty(PropertyName.suppressDeathrattles, Variant.From(in suppressDeathrattles));
		info.AddProperty(PropertyName.mowerDeathVisualOwned, Variant.From(in mowerDeathVisualOwned));
		info.AddProperty(PropertyName._inWater, Variant.From(in _inWater));
		info.AddProperty(PropertyName.iceSpeedDown, Variant.From(in iceSpeedDown));
		info.AddProperty(PropertyName.emSpeedDown, Variant.From(in emSpeedDown));
		info.AddProperty(PropertyName.useIdleAnimeReset, Variant.From(in useIdleAnimeReset));
		info.AddProperty(PropertyName._syncApplyingAnimation, Variant.From(in _syncApplyingAnimation));
		info.AddProperty(PropertyName.isUnlimitedFire, Variant.From(in isUnlimitedFire));
		info.AddProperty(PropertyName.randFreshIndex, Variant.From(in randFreshIndex));
		info.AddProperty(PropertyName.groundRight, Variant.From(in groundRight));
		info.AddProperty(PropertyName._componentGameplayUntilBattlefieldEntry, Variant.From(in _componentGameplayUntilBattlefieldEntry));
		info.AddProperty(PropertyName._componentGameplayPhysicsDispatchActive, Variant.From(in _componentGameplayPhysicsDispatchActive));
		info.AddProperty(PropertyName._componentGameplayPhysicsSnapshot, Variant.From(in _componentGameplayPhysicsSnapshot));
		info.AddProperty(PropertyName._componentGameplayPhysicsSnapshotFrame, Variant.From(in _componentGameplayPhysicsSnapshotFrame));
		info.AddProperty(PropertyName._physicsGlobalPositionFrame, Variant.From(in _physicsGlobalPositionFrame));
		info.AddProperty(PropertyName._physicsGlobalPosition, Variant.From(in _physicsGlobalPosition));
		info.AddProperty(PropertyName._shadowGlobalTransform, Variant.From(in _shadowGlobalTransform));
		info.AddProperty(PropertyName._shadowGlobalTransformValid, Variant.From(in _shadowGlobalTransformValid));
		info.AddProperty(PropertyName._physicsLocalPosition, Variant.From(in _physicsLocalPosition));
		info.AddProperty(PropertyName._physicsLocalPositionValid, Variant.From(in _physicsLocalPositionValid));
		info.AddProperty(PropertyName._writingCachedGlobalPosition, Variant.From(in _writingCachedGlobalPosition));
		info.AddProperty(PropertyName._pendingCachedGlobalTransform, Variant.From(in _pendingCachedGlobalTransform));
		info.AddProperty(PropertyName._pendingCachedGlobalTransformNotification, Variant.From(in _pendingCachedGlobalTransformNotification));
		info.AddProperty(PropertyName._cellMoveTween, Variant.From(in _cellMoveTween));
		info.AddProperty(PropertyName._zMotionOwnsLocalRender, Variant.From(in _zMotionOwnsLocalRender));
		info.AddProperty(PropertyName._zMotionStablePhysicsFrames, Variant.From(in _zMotionStablePhysicsFrames));
		info.AddProperty(PropertyName._spriteGroupLocalX, Variant.From(in _spriteGroupLocalX));
		info.AddProperty(PropertyName._spriteGroupLocalXCaptured, Variant.From(in _spriteGroupLocalXCaptured));
		info.AddProperty(PropertyName.isDestroy, Variant.From(in isDestroy));
		info.AddProperty(PropertyName._progressRestorePresentationRefreshPending, Variant.From(in _progressRestorePresentationRefreshPending));
		info.AddProperty(PropertyName._characterExternalVisualsInitialized, Variant.From(in _characterExternalVisualsInitialized));
		info.AddProperty(PropertyName._legacySpriteGroupVisualsScanned, Variant.From(in _legacySpriteGroupVisualsScanned));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.HasValidRuntimeConfiguration, out var value))
		{
			HasValidRuntimeConfiguration = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.HitBoxDefinition, out var value2))
		{
			HitBoxDefinition = value2.As<CharacterHitBoxDefinition>();
		}
		if (info.TryGetProperty(PropertyName.MainStateMachineDefinition, out var value3))
		{
			MainStateMachineDefinition = value3.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName.ComponentSet, out var value4))
		{
			ComponentSet = value4.As<CharacterComponentSet>();
		}
		if (info.TryGetProperty(PropertyName.invisible, out var value5))
		{
			invisible = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value6))
		{
			config = value6.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value7))
		{
			sprite = value7.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.headSlot, out var value8))
		{
			headSlot = value8.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName.camp, out var value9))
		{
			camp = value9.As<TowerDefenseEnum.CHARACTER_CAMP>();
		}
		if (info.TryGetProperty(PropertyName.damagePartClip, out var value10))
		{
			damagePartClip = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damagePart, out var value11))
		{
			damagePart = value11.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.damagePartSlot, out var value12))
		{
			damagePartSlot = value12.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.previewDamagePointPersontage, out var value13))
		{
			previewDamagePointPersontage = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentArmor, out var value14))
		{
			currentArmor = value14.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.currentCustom, out var value15))
		{
			currentCustom = value15.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.componentAlive, out var value16))
		{
			componentAlive = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.inWater, out var value17))
		{
			inWater = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsProgressRestoreInFlight, out var value18))
		{
			IsProgressRestoreInFlight = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CharacterExternalVisualCompatibilityScanCount, out var value19))
		{
			CharacterExternalVisualCompatibilityScanCount = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CharacterAnimatedStatusVisualCount, out var value20))
		{
			CharacterAnimatedStatusVisualCount = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CharacterAnimatedStatusTopologyChangeCount, out var value21))
		{
			CharacterAnimatedStatusTopologyChangeCount = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName.syncId, out var value22))
		{
			syncId = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName.backEffectNode, out var value23))
		{
			backEffectNode = value23.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.frontEffectNode, out var value24))
		{
			frontEffectNode = value24.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.spriteGroup, out var value25))
		{
			spriteGroup = value25.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.transformPoint, out var value26))
		{
			transformPoint = value26.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.shadowSprite, out var value27))
		{
			shadowSprite = value27.As<TowerDefenseShadowVisual>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxDefinition, out var value28))
		{
			_hitBoxDefinition = value28.As<CharacterHitBoxDefinition>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxRuntimeInitialized, out var value29))
		{
			_hitBoxRuntimeInitialized = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxAvailable, out var value30))
		{
			_hitBoxAvailable = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxDefaultEnabled, out var value31))
		{
			_hitBoxDefaultEnabled = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxDefaultMonitorable, out var value32))
		{
			_hitBoxDefaultMonitorable = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxSuppression, out var value33))
		{
			_hitBoxSuppression = value33.As<HitBoxSuppressionReason>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxMonitorSuppression, out var value34))
		{
			_hitBoxMonitorSuppression = value34.As<HitBoxSuppressionReason>();
		}
		if (info.TryGetProperty(PropertyName._canReceiveExplosionHit, out var value35))
		{
			_canReceiveExplosionHit = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxPreviewDraw, out var value36))
		{
			_hitBoxPreviewDraw = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._worldHitRectCache, out var value37))
		{
			_worldHitRectCache = value37.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._worldHitRectCacheFrame, out var value38))
		{
			_worldHitRectCacheFrame = value38.As<long>();
		}
		if (info.TryGetProperty(PropertyName._worldHitRectCacheTransform, out var value39))
		{
			_worldHitRectCacheTransform = value39.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._worldHitRectCacheScaleX, out var value40))
		{
			_worldHitRectCacheScaleX = value40.As<float>();
		}
		if (info.TryGetProperty(PropertyName._localScaleXSnapshot, out var value41))
		{
			_localScaleXSnapshot = value41.As<float>();
		}
		if (info.TryGetProperty(PropertyName._localScaleXSnapshotValid, out var value42))
		{
			_localScaleXSnapshotValid = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._worldHitRectCacheDefinition, out var value43))
		{
			_worldHitRectCacheDefinition = value43.As<CharacterHitBoxDefinition>();
		}
		if (info.TryGetProperty(PropertyName._worldHitRectCacheValid, out var value44))
		{
			_worldHitRectCacheValid = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitBoxBoundsRevision, out var value45))
		{
			_hitBoxBoundsRevision = value45.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._renderAncestorScaleSnapshot, out var value46))
		{
			_renderAncestorScaleSnapshot = value46.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._renderAncestorScaleSnapshotValid, out var value47))
		{
			_renderAncestorScaleSnapshotValid = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mainStateMachineDispatchEnabled, out var value48))
		{
			_mainStateMachineDispatchEnabled = value48.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ownerBatchRegistered, out var value49))
		{
			_ownerBatchRegistered = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._batchUsesInheritedProcessMode, out var value50))
		{
			_batchUsesInheritedProcessMode = value50.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._batchProcessModeParent, out var value51))
		{
			_batchProcessModeParent = value51.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._mainStateMachineReleased, out var value52))
		{
			_mainStateMachineReleased = value52.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._authoritativeMainStateRestoreDepth, out var value53))
		{
			_authoritativeMainStateRestoreDepth = value53.As<int>();
		}
		if (info.TryGetProperty(PropertyName._characterSkinSwitchedEventBus, out var value54))
		{
			_characterSkinSwitchedEventBus = value54.As<BattleEventBus>();
		}
		if (info.TryGetProperty(PropertyName._characterSkinSwitchedSubscribed, out var value55))
		{
			_characterSkinSwitchedSubscribed = value55.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingMainStateMachineSnapshot, out var value56))
		{
			_pendingMainStateMachineSnapshot = value56.As<StateMachineSnapshot>();
		}
		if (info.TryGetProperty(PropertyName._pendingMainStateMachineSnapshotIsRemote, out var value57))
		{
			_pendingMainStateMachineSnapshotIsRemote = value57.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingMainStateMachineSnapshotSuppressEffects, out var value58))
		{
			_pendingMainStateMachineSnapshotSuppressEffects = value58.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingMainStateMachineSnapshotProgressCompatible, out var value59))
		{
			_pendingMainStateMachineSnapshotProgressCompatible = value59.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastRemoteMainStateMachineRevision, out var value60))
		{
			_lastRemoteMainStateMachineRevision = value60.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastMainStateMachineProcessFrame, out var value61))
		{
			_lastMainStateMachineProcessFrame = value61.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._lastComponentStateMachineProcessFrame, out var value62))
		{
			_lastComponentStateMachineProcessFrame = value62.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._lastMainStateMachinePhysicsFrame, out var value63))
		{
			_lastMainStateMachinePhysicsFrame = value63.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactDisplacementActive, out var value64))
		{
			_bowlingImpactDisplacementActive = value64.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactStartGlobalX, out var value65))
		{
			_bowlingImpactStartGlobalX = value65.As<float>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactTargetGlobalX, out var value66))
		{
			_bowlingImpactTargetGlobalX = value66.As<float>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactElapsed, out var value67))
		{
			_bowlingImpactElapsed = value67.As<double>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactDuration, out var value68))
		{
			_bowlingImpactDuration = value68.As<double>();
		}
		if (info.TryGetProperty(PropertyName._characterStateHandlesConnected, out var value69))
		{
			_characterStateHandlesConnected = value69.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showHealthOffset, out var value70))
		{
			showHealthOffset = value70.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.componentManager, out var value71))
		{
			componentManager = value71.As<ComponentManager>();
		}
		if (info.TryGetProperty(PropertyName._resourceComponentFacadesCurrent, out var value72))
		{
			_resourceComponentFacadesCurrent = value72.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._invisible, out var value73))
		{
			_invisible = value73.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.idleAnimeClip, out var value74))
		{
			idleAnimeClip = value74.As<string>();
		}
		if (info.TryGetProperty(PropertyName.sleepAnimeClip, out var value75))
		{
			sleepAnimeClip = value75.As<string>();
		}
		if (info.TryGetProperty(PropertyName._config, out var value76))
		{
			_config = value76.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value77))
		{
			_sprite = value77.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._headSlot, out var value78))
		{
			_headSlot = value78.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._camp, out var value79))
		{
			_camp = value79.As<TowerDefenseEnum.CHARACTER_CAMP>();
		}
		if (info.TryGetProperty(PropertyName._damagePartClip, out var value80))
		{
			_damagePartClip = value80.As<string>();
		}
		if (info.TryGetProperty(PropertyName._damagePart, out var value81))
		{
			_damagePart = value81.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._damagePartSlot, out var value82))
		{
			_damagePartSlot = value82.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._previewDamagePointPersontage, out var value83))
		{
			_previewDamagePointPersontage = value83.As<double>();
		}
		if (info.TryGetProperty(PropertyName._currentArmor, out var value84))
		{
			_currentArmor = value84.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._currentCustom, out var value85))
		{
			_currentCustom = value85.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.instance, out var value86))
		{
			instance = value86.As<TowerDefenseCharacterInstance>();
		}
		if (info.TryGetProperty(PropertyName.characterDisabled, out var value87))
		{
			characterDisabled = value87.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._componentAlive, out var value88))
		{
			_componentAlive = value88.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.componentRunning, out var value89))
		{
			componentRunning = value89.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.characterFilter, out var value90))
		{
			characterFilter = value90.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timeScaleInit, out var value91))
		{
			timeScaleInit = value91.As<double>();
		}
		if (info.TryGetProperty(PropertyName.timeScale, out var value92))
		{
			timeScale = value92.As<double>();
		}
		if (info.TryGetProperty(PropertyName.timeScaleSave, out var value93))
		{
			timeScaleSave = value93.As<double>();
		}
		if (info.TryGetProperty(PropertyName.inGame, out var value94))
		{
			inGame = value94.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.editorPreviewMode, out var value95))
		{
			editorPreviewMode = value95.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.editorMapPreviewMode, out var value96))
		{
			editorMapPreviewMode = value96.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.forceLocalRenderDuringZMotion, out var value97))
		{
			forceLocalRenderDuringZMotion = value97.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isShow, out var value98))
		{
			isShow = value98.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packet, out var value99))
		{
			packet = value99.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value100))
		{
			cost = value100.As<double>();
		}
		if (info.TryGetProperty(PropertyName.nearDie, out var value101))
		{
			nearDie = value101.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.die, out var value102))
		{
			die = value102.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canMowerMove, out var value103))
		{
			canMowerMove = value103.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.baseSpriteScale, out var value104))
		{
			baseSpriteScale = value104.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.isRise, out var value105))
		{
			isRise = value105.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isShovel, out var value106))
		{
			isShovel = value106.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isSmash, out var value107))
		{
			isSmash = value107.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isExplode, out var value108))
		{
			isExplode = value108.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isChomp, out var value109))
		{
			isChomp = value109.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.skipDestroySet, out var value110))
		{
			skipDestroySet = value110.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.suppressDeathrattles, out var value111))
		{
			suppressDeathrattles = value111.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mowerDeathVisualOwned, out var value112))
		{
			mowerDeathVisualOwned = value112.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._inWater, out var value113))
		{
			_inWater = value113.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.iceSpeedDown, out var value114))
		{
			iceSpeedDown = value114.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.emSpeedDown, out var value115))
		{
			emSpeedDown = value115.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useIdleAnimeReset, out var value116))
		{
			useIdleAnimeReset = value116.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._syncApplyingAnimation, out var value117))
		{
			_syncApplyingAnimation = value117.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isUnlimitedFire, out var value118))
		{
			isUnlimitedFire = value118.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.randFreshIndex, out var value119))
		{
			randFreshIndex = value119.As<int>();
		}
		if (info.TryGetProperty(PropertyName.groundRight, out var value120))
		{
			groundRight = value120.As<double>();
		}
		if (info.TryGetProperty(PropertyName._componentGameplayUntilBattlefieldEntry, out var value121))
		{
			_componentGameplayUntilBattlefieldEntry = value121.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._componentGameplayPhysicsDispatchActive, out var value122))
		{
			_componentGameplayPhysicsDispatchActive = value122.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._componentGameplayPhysicsSnapshot, out var value123))
		{
			_componentGameplayPhysicsSnapshot = value123.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._componentGameplayPhysicsSnapshotFrame, out var value124))
		{
			_componentGameplayPhysicsSnapshotFrame = value124.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._physicsGlobalPositionFrame, out var value125))
		{
			_physicsGlobalPositionFrame = value125.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._physicsGlobalPosition, out var value126))
		{
			_physicsGlobalPosition = value126.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._shadowGlobalTransform, out var value127))
		{
			_shadowGlobalTransform = value127.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._shadowGlobalTransformValid, out var value128))
		{
			_shadowGlobalTransformValid = value128.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._physicsLocalPosition, out var value129))
		{
			_physicsLocalPosition = value129.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._physicsLocalPositionValid, out var value130))
		{
			_physicsLocalPositionValid = value130.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._writingCachedGlobalPosition, out var value131))
		{
			_writingCachedGlobalPosition = value131.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingCachedGlobalTransform, out var value132))
		{
			_pendingCachedGlobalTransform = value132.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._pendingCachedGlobalTransformNotification, out var value133))
		{
			_pendingCachedGlobalTransformNotification = value133.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cellMoveTween, out var value134))
		{
			_cellMoveTween = value134.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._zMotionOwnsLocalRender, out var value135))
		{
			_zMotionOwnsLocalRender = value135.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._zMotionStablePhysicsFrames, out var value136))
		{
			_zMotionStablePhysicsFrames = value136.As<int>();
		}
		if (info.TryGetProperty(PropertyName._spriteGroupLocalX, out var value137))
		{
			_spriteGroupLocalX = value137.As<float>();
		}
		if (info.TryGetProperty(PropertyName._spriteGroupLocalXCaptured, out var value138))
		{
			_spriteGroupLocalXCaptured = value138.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isDestroy, out var value139))
		{
			isDestroy = value139.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._progressRestorePresentationRefreshPending, out var value140))
		{
			_progressRestorePresentationRefreshPending = value140.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._characterExternalVisualsInitialized, out var value141))
		{
			_characterExternalVisualsInitialized = value141.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._legacySpriteGroupVisualsScanned, out var value142))
		{
			_legacySpriteGroupVisualsScanned = value142.As<bool>();
		}
	}
}
