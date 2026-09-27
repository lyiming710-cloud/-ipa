using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffHypnoses.cs")]
public class TowerDefenseCharacterBuffHypnoses : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public static readonly StringName ApplyCharacterFacingSign = "ApplyCharacterFacingSign";

		public static readonly StringName ApplyTwoCellPlantMirrorAnchor = "ApplyTwoCellPlantMirrorAnchor";

		public static readonly StringName ApplyMirrorAnchorRecursive = "ApplyMirrorAnchorRecursive";

		public static readonly StringName ApplySpriteMirrorAnchor = "ApplySpriteMirrorAnchor";

		public static readonly StringName HasHorizontalGridExtension = "HasHorizontalGridExtension";

		public static readonly StringName ResolveSpritePlaybackArtCenterX = "ResolveSpritePlaybackArtCenterX";

		public new static readonly StringName SyncPresentationReadOnlyClient = "SyncPresentationReadOnlyClient";

		public static readonly StringName CleanupBattleState = "CleanupBattleState";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public new static readonly StringName Cancel = "Cancel";

		public new static readonly StringName Refresh = "Refresh";

		public static readonly StringName RefreshGroundMoveDirectionCache = "RefreshGroundMoveDirectionCache";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName SetupSunProduce = "SetupSunProduce";

		public static readonly StringName CleanupSunProduce = "CleanupSunProduce";

		public static readonly StringName SetupTorchwood = "SetupTorchwood";

		public static readonly StringName SyncTorchwoodVisualReadOnlyClient = "SyncTorchwoodVisualReadOnlyClient";

		public static readonly StringName SetupTorchwoodVisual = "SetupTorchwoodVisual";

		public static readonly StringName CleanupTorchwood = "CleanupTorchwood";

		public static readonly StringName CleanupTorchwoodVisual = "CleanupTorchwoodVisual";

		public static readonly StringName SetupDeathFreeze = "SetupDeathFreeze";

		public static readonly StringName RefreshDeathFreezeRuntime = "RefreshDeathFreezeRuntime";

		public static readonly StringName CleanupDeathFreeze = "CleanupDeathFreeze";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public new static readonly StringName FrameMeshColorMultiplier = "FrameMeshColorMultiplier";

		public static readonly StringName deathFreezeOver = "deathFreezeOver";

		public static readonly StringName time = "time";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName saveCamp = "saveCamp";

		public static readonly StringName enableTorchwood = "enableTorchwood";

		public static readonly StringName torchwoodChangeName = "torchwoodChangeName";

		public static readonly StringName torchwoodAudio = "torchwoodAudio";

		public static readonly StringName torchwoodAreaSize = "torchwoodAreaSize";

		public static readonly StringName enableDeathFreeze = "enableDeathFreeze";

		public static readonly StringName deathFreezeTime = "deathFreezeTime";

		public static readonly StringName deathSlowTime = "deathSlowTime";

		public static readonly StringName deathDamage = "deathDamage";

		public static readonly StringName enableSunProduce = "enableSunProduce";

		public static readonly StringName sunProduceInterval = "sunProduceInterval";

		public static readonly StringName sunProduceNum = "sunProduceNum";

		public static readonly StringName torchwoodSprite = "torchwoodSprite";

		public static readonly StringName _deathFreezeOver = "_deathFreezeOver";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	private static readonly Color CHARM_COLOR = new Color(0.83f, 0.34f, 0.81f);

	private static readonly Vector2 IceFireTorchwoodHeadOffset = new Vector2(0f, 34f);

	private static readonly Vector2 IceFireTorchwoodFallbackOffset = new Vector2(-20f, 28f);

	private const float IceFireTorchwoodScale = 0.35f;

	[Export(PropertyHint.None, "")]
	public double time = -1.0;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.CHARACTER_CAMP saveCamp = TowerDefenseEnum.CHARACTER_CAMP.NOONE;

	[Export(PropertyHint.None, "")]
	public bool enableTorchwood;

	[Export(PropertyHint.None, "")]
	public string torchwoodChangeName = "IceFire";

	[Export(PropertyHint.None, "")]
	public string torchwoodAudio = "";

	[Export(PropertyHint.None, "")]
	public Vector2 torchwoodAreaSize = new Vector2(80f, 80f);

	[Export(PropertyHint.None, "")]
	public bool enableDeathFreeze;

	[Export(PropertyHint.None, "")]
	public double deathFreezeTime = 3.0;

	[Export(PropertyHint.None, "")]
	public double deathSlowTime = 15.0;

	[Export(PropertyHint.None, "")]
	public double deathDamage = 1800.0;

	[Export(PropertyHint.None, "")]
	public bool enableSunProduce;

	[Export(PropertyHint.None, "")]
	public double sunProduceInterval = 5.0;

	[Export(PropertyHint.None, "")]
	public int sunProduceNum = 25;

	private ProduceComponent _produceComponent;

	private ChangeProjectileComponent _changeProjectileComponent;

	private ExplodeComponent _explodeComponent;

	public Node2D torchwoodSprite;

	private bool _deathFreezeOver;

	private static StateMachineDefinition _explodeStateMachine;

	private static PackedScene _iceFireTorchwoodScene;

	private const string MirrorAnchorOriginalOffsetMeta = "__hypnosesTwoCellOriginalOffset";

	public override Color FrameMeshColorMultiplier => CHARM_COLOR;

	[Export(PropertyHint.None, "")]
	public bool deathFreezeOver
	{
		get
		{
			return _deathFreezeOver;
		}
		set
		{
			_deathFreezeOver = value;
		}
	}

	public static event Action<TowerDefenseCharacter> OnHypnotized;

	public override void _Init()
	{
		key = "Hypnoses";
	}

	public override void Enter()
	{
		if (canFliter && (character.instance.unUseBuffFlags & 8) != 0)
		{
			if (saveCamp == TowerDefenseEnum.CHARACTER_CAMP.NOONE)
			{
				saveCamp = character.camp;
			}
			character.buff.DeleteBuff("Hypnoses");
			return;
		}
		character.buff.DeleteBuff("Butter");
		character.buff.DeleteBuff("Dizziness");
		character.buff.DeleteBuff("Frozen");
		character.instance.hypnoses = true;
		if (saveCamp == TowerDefenseEnum.CHARACTER_CAMP.NOONE)
		{
			saveCamp = character.camp;
		}
		if (character.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			character.camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
		}
		else if (character.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT)
		{
			character.camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
		}
		ApplyCharacterFacingSign(-1f);
		if (saveCamp == character.camp)
		{
			character.buff.DeleteBuff("Hypnoses");
			return;
		}
		RefreshGroundMoveDirectionCache();
		character.OnHypnosisStateChanged();
		if (enableSunProduce)
		{
			SetupSunProduce();
		}
		if (enableTorchwood)
		{
			SetupTorchwood();
		}
		if (enableDeathFreeze)
		{
			SetupDeathFreeze();
		}
		OnHypnotized?.Invoke(character);
		TowerDefensePlantHypnoShroomGhost.OnCharacterHypnotized(character);
	}

	public override void EnterReadOnlyClient()
	{
		if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance))
		{
			if (saveCamp == TowerDefenseEnum.CHARACTER_CAMP.NOONE)
			{
				saveCamp = character.camp;
			}
			character.instance.hypnoses = true;
			if (saveCamp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
			{
				character.camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
			}
			else if (saveCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT)
			{
				character.camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
			}
			ApplyCharacterFacingSign(-1f);
			RefreshGroundMoveDirectionCache();
			character.OnHypnosisStateChanged();
			SyncTorchwoodVisualReadOnlyClient();
		}
	}

	private void ApplyCharacterFacingSign(float sign)
	{
		bool flag = character.Scale.X < 0f;
		bool flag2 = sign < 0f;
		float num = Mathf.Abs(character.Scale.X);
		character.Scale = new Vector2((sign < 0f) ? (0f - num) : num, character.Scale.Y);
		if (flag != flag2)
		{
			ApplyTwoCellPlantMirrorAnchor(flag2);
		}
		if (GodotObject.IsInstanceValid(character.sprite))
		{
			character.sprite.NotifyAncestorTransformChangedForRender();
		}
	}

	private void ApplyTwoCellPlantMirrorAnchor(bool mirrored)
	{
		if (GodotObject.IsInstanceValid(character) && character.config is TowerDefensePlantConfig plantConfig && HasHorizontalGridExtension(plantConfig) && GodotObject.IsInstanceValid(character.sprite))
		{
			ApplyMirrorAnchorRecursive(character.sprite, mirrored);
		}
	}

	private static void ApplyMirrorAnchorRecursive(Node node, bool mirrored)
	{
		if (node is AdobeAnimateSprite sprite)
		{
			ApplySpriteMirrorAnchor(sprite, mirrored);
		}
		int childCount = node.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			ApplyMirrorAnchorRecursive(node.GetChild(i), mirrored);
		}
	}

	private static void ApplySpriteMirrorAnchor(AdobeAnimateSprite sprite, bool mirrored)
	{
		if (mirrored)
		{
			if (!sprite.HasMeta("__hypnosesTwoCellOriginalOffset"))
			{
				sprite.SetMeta("__hypnosesTwoCellOriginalOffset", sprite.offset);
			}
			Vector2 vector = (Vector2)sprite.GetMeta("__hypnosesTwoCellOriginalOffset");
			sprite.offset = new Vector2(0f - vector.X - 2f * ResolveSpritePlaybackArtCenterX(sprite), vector.Y);
		}
		else if (sprite.HasMeta("__hypnosesTwoCellOriginalOffset"))
		{
			sprite.offset = (Vector2)sprite.GetMeta("__hypnosesTwoCellOriginalOffset");
		}
	}

	private static bool HasHorizontalGridExtension(TowerDefensePlantConfig plantConfig)
	{
		foreach (Vector2I item in plantConfig.extendGrid)
		{
			if (item.X != 0)
			{
				return true;
			}
		}
		return false;
	}

	private static float ResolveSpritePlaybackArtCenterX(AdobeAnimateSprite sprite)
	{
		Rect2[] array = AdobeAnimateDefinitionCache.GetOrBuild(sprite.flashAnimeData)?.FrameLocalBounds;
		if (array == null || array.Length == 0)
		{
			return 0f;
		}
		Vector2I clipRange = sprite.clipRange;
		int num = clipRange.X;
		int num2 = clipRange.Y;
		if (num > num2 || num < 0 || num2 >= array.Length)
		{
			num = 0;
			num2 = array.Length - 1;
		}
		float[] array2 = new float[num2 - num + 1];
		int num3 = 0;
		for (int i = num; i <= num2; i++)
		{
			Rect2 rect = array[i];
			if (!(rect.Size.X <= 0f) || !(rect.Size.Y <= 0f))
			{
				array2[num3] = rect.GetCenter().X;
				num3++;
			}
		}
		if (num3 == 0)
		{
			return array[0].GetCenter().X;
		}
		System.Array.Sort(array2, 0, num3);
		return array2[num3 / 2];
	}

	public override void SyncPresentationReadOnlyClient()
	{
		SyncTorchwoodVisualReadOnlyClient();
	}

	public static void CleanupBattleState()
	{
		TowerDefensePlantHypnoShroomGhost.ResetBattleState();
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		if (time != -1.0)
		{
			return currentTime >= time;
		}
		return false;
	}

	public override void StepReadOnlyClient(double delta)
	{
		Step(delta);
	}

	public override void Exit()
	{
		CleanupSunProduce();
		CleanupTorchwood();
		CleanupDeathFreeze();
		character.instance.hypnoses = false;
		character.camp = saveCamp;
		ApplyCharacterFacingSign(1f);
		RefreshGroundMoveDirectionCache();
		character.OnHypnosisStateChanged();
		if (character is TowerDefenseZombie towerDefenseZombie && !character.die && GodotObject.IsInstanceValid(towerDefenseZombie))
		{
			AttackComponent attackComponent = towerDefenseZombie.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased)
			{
				attackComponent.target = null;
			}
			towerDefenseZombie.Walk();
		}
	}

	public override void ExitReadOnlyClient()
	{
		CleanupTorchwoodVisual();
		if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance))
		{
			character.instance.hypnoses = false;
			if (saveCamp != TowerDefenseEnum.CHARACTER_CAMP.NOONE)
			{
				character.camp = saveCamp;
			}
			ApplyCharacterFacingSign(1f);
			if (character is TowerDefenseZombie { attackComponent: { IsReleased: false } } towerDefenseZombie)
			{
				towerDefenseZombie.attackComponent.target = null;
			}
			RefreshGroundMoveDirectionCache();
			character.OnHypnosisStateChanged();
		}
	}

	public override void Cancel()
	{
		CleanupSunProduce();
		CleanupTorchwood();
		CleanupDeathFreeze();
		ExitReadOnlyClient();
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		if (!(config is TowerDefenseCharacterBuffHypnoses towerDefenseCharacterBuffHypnoses))
		{
			return;
		}
		if (time != -1.0)
		{
			time = ((towerDefenseCharacterBuffHypnoses.time == -1.0) ? (-1.0) : Mathf.Max(time, towerDefenseCharacterBuffHypnoses.time));
		}
		currentTime = 0.0;
		if (enableSunProduce != towerDefenseCharacterBuffHypnoses.enableSunProduce)
		{
			CleanupSunProduce();
		}
		enableSunProduce = towerDefenseCharacterBuffHypnoses.enableSunProduce;
		sunProduceInterval = towerDefenseCharacterBuffHypnoses.sunProduceInterval;
		sunProduceNum = towerDefenseCharacterBuffHypnoses.sunProduceNum;
		if (enableSunProduce)
		{
			ProduceComponent produceComponent = _produceComponent;
			if (produceComponent == null || produceComponent.IsReleased)
			{
				SetupSunProduce();
			}
			else
			{
				_produceComponent.produceInterval = (float)sunProduceInterval;
				_produceComponent.num = sunProduceNum;
			}
		}
		if (enableTorchwood != towerDefenseCharacterBuffHypnoses.enableTorchwood)
		{
			CleanupTorchwood();
		}
		enableTorchwood = towerDefenseCharacterBuffHypnoses.enableTorchwood;
		torchwoodChangeName = towerDefenseCharacterBuffHypnoses.torchwoodChangeName;
		torchwoodAudio = towerDefenseCharacterBuffHypnoses.torchwoodAudio;
		torchwoodAreaSize = towerDefenseCharacterBuffHypnoses.torchwoodAreaSize;
		if (enableTorchwood)
		{
			ChangeProjectileComponent changeProjectileComponent = _changeProjectileComponent;
			if (changeProjectileComponent == null || changeProjectileComponent.IsReleased)
			{
				SetupTorchwood();
			}
			else
			{
				_changeProjectileComponent.changeName = new StringName(torchwoodChangeName);
				_changeProjectileComponent.changeAudio = torchwoodAudio;
				if (_changeProjectileComponent.checkShape?.Geometry is RectangleShape2D rectangleShape2D)
				{
					rectangleShape2D.Size = torchwoodAreaSize;
				}
			}
		}
		SyncTorchwoodVisualReadOnlyClient();
		if (enableDeathFreeze != towerDefenseCharacterBuffHypnoses.enableDeathFreeze)
		{
			CleanupDeathFreeze();
		}
		enableDeathFreeze = towerDefenseCharacterBuffHypnoses.enableDeathFreeze;
		deathFreezeTime = towerDefenseCharacterBuffHypnoses.deathFreezeTime;
		deathSlowTime = towerDefenseCharacterBuffHypnoses.deathSlowTime;
		deathDamage = towerDefenseCharacterBuffHypnoses.deathDamage;
		if (enableDeathFreeze)
		{
			ExplodeComponent explodeComponent = _explodeComponent;
			if (explodeComponent == null || explodeComponent.IsReleased)
			{
				SetupDeathFreeze();
			}
			else
			{
				RefreshDeathFreezeRuntime();
			}
		}
	}

	private void RefreshGroundMoveDirectionCache()
	{
		if (character is TowerDefenseZombie { groundMoveComponent: { IsReleased: false } } towerDefenseZombie)
		{
			towerDefenseZombie.groundMoveComponent.RefreshDirectionCache();
		}
	}

	public override void Destroy()
	{
		if (enableDeathFreeze && !_deathFreezeOver && GodotObject.IsInstanceValid(character) && (character.die || character.nearDie))
		{
			_deathFreezeOver = true;
			ExplodeComponent explodeComponent = _explodeComponent;
			if (explodeComponent != null && !explodeComponent.IsReleased)
			{
				_explodeComponent.Explode();
			}
		}
		CleanupSunProduce();
		CleanupTorchwood();
		CleanupDeathFreeze();
	}

	private void SetupSunProduce()
	{
		if (GodotObject.IsInstanceValid(character))
		{
			ComponentManager componentManager = character.componentManager;
			if (GodotObject.IsInstanceValid(componentManager))
			{
				ProduceComponentDefinition definition = new ProduceComponentDefinition
				{
					ComponentTypeId = "ProduceComponent",
					DefinitionId = "runtime.buff.hypnoses.produce",
					InstanceId = "buff.hypnoses.produce",
					WireIndex = -1,
					produceType = ((character.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE) ? "BrainSun" : "Sun"),
					produceInterval = (float)sunProduceInterval,
					num = sunProduceNum
				};
				_produceComponent = componentManager.AddRuntimeComponent(definition) as ProduceComponent;
			}
		}
	}

	private void CleanupSunProduce()
	{
		ProduceComponent produceComponent = _produceComponent;
		if (produceComponent != null && !produceComponent.IsReleased)
		{
			ComponentManager componentManager = _produceComponent.Manager;
			if (!GodotObject.IsInstanceValid(componentManager) && GodotObject.IsInstanceValid(character))
			{
				componentManager = character.componentManager;
			}
			if (GodotObject.IsInstanceValid(componentManager))
			{
				componentManager.RemoveRuntimeComponent(_produceComponent);
			}
		}
		_produceComponent = null;
	}

	private void SetupTorchwood()
	{
		if (GodotObject.IsInstanceValid(character))
		{
			ComponentManager componentManager = character.componentManager;
			if (GodotObject.IsInstanceValid(componentManager))
			{
				RectangleShape2D rectangleShape2D = new RectangleShape2D();
				rectangleShape2D.Size = torchwoodAreaSize;
				ChangeProjectileComponentDefinition definition = new ChangeProjectileComponentDefinition
				{
					ComponentTypeId = "ChangeProjectileComponent",
					DefinitionId = "runtime.buff.hypnoses.change_projectile",
					InstanceId = "buff.hypnoses.change_projectile",
					WireIndex = -1,
					changeName = new StringName(torchwoodChangeName),
					changeAudio = torchwoodAudio,
					checkShape = new AabbShape2DResource
					{
						Geometry = rectangleShape2D,
						LocalTransform = Transform2D.Identity
					}
				};
				_changeProjectileComponent = componentManager.AddRuntimeComponent(definition) as ChangeProjectileComponent;
				SetupTorchwoodVisual();
			}
		}
	}

	private void SyncTorchwoodVisualReadOnlyClient()
	{
		if (enableTorchwood && torchwoodChangeName == "IceFire")
		{
			SetupTorchwoodVisual();
		}
		else
		{
			CleanupTorchwoodVisual();
		}
	}

	private void SetupTorchwoodVisual()
	{
		if (!enableTorchwood || torchwoodChangeName != "IceFire" || !(character is TowerDefenseZombie) || GodotObject.IsInstanceValid(torchwoodSprite))
		{
			return;
		}
		if (_iceFireTorchwoodScene == null)
		{
			_iceFireTorchwoodScene = GD.Load<PackedScene>("res://Asset/Anime/Effect/Fire/Ice/IceFire.tscn");
		}
		if (!GodotObject.IsInstanceValid(_iceFireTorchwoodScene))
		{
			return;
		}
		torchwoodSprite = _iceFireTorchwoodScene.Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
		torchwoodSprite.Name = "HypnosesIceFireTorchwood";
		torchwoodSprite.Scale = new Vector2(character.Scale.X * character.sprite.Scale.X * 0.35f, 0.35f);
		if (torchwoodSprite is AdobeAnimateSprite adobeAnimateSprite)
		{
			AdobeAnimateSlot adobeAnimateSlot = (GodotObject.IsInstanceValid(character.headSlot) ? character.headSlot : null);
			Transform2D targetGlobalTransform;
			if (GodotObject.IsInstanceValid(adobeAnimateSlot))
			{
				adobeAnimateSlot.Update();
				adobeAnimateSprite.Position = IceFireTorchwoodHeadOffset;
				targetGlobalTransform = character.GetLogicalGlobalTransform(adobeAnimateSlot) * adobeAnimateSprite.Transform;
			}
			else
			{
				targetGlobalTransform = character.GetLogicalGlobalTransform(character.spriteGroup) * adobeAnimateSprite.Transform;
				targetGlobalTransform.Origin = character.GetLogicalGlobalPosition(character.sprite) + IceFireTorchwoodFallbackOffset;
			}
			character.AttachAnimatedStatusVisual(adobeAnimateSprite, adobeAnimateSlot, targetGlobalTransform);
		}
		else if (GodotObject.IsInstanceValid(character.headSlot))
		{
			character.headSlot.Update();
			character.headSlot.AddChild(torchwoodSprite, forceReadableName: false, Node.InternalMode.Disabled);
			torchwoodSprite.Position = IceFireTorchwoodHeadOffset;
		}
		else
		{
			character.spriteGroup.AddChild(torchwoodSprite, forceReadableName: false, Node.InternalMode.Disabled);
			torchwoodSprite.Position = character.GetLogicalGlobalTransform(character.spriteGroup).AffineInverse() * (character.GetLogicalGlobalPosition(character.sprite) + IceFireTorchwoodFallbackOffset);
		}
	}

	private void CleanupTorchwood()
	{
		ChangeProjectileComponent changeProjectileComponent = _changeProjectileComponent;
		if (changeProjectileComponent != null && !changeProjectileComponent.IsReleased)
		{
			ComponentManager componentManager = _changeProjectileComponent.Manager;
			if (!GodotObject.IsInstanceValid(componentManager) && GodotObject.IsInstanceValid(character))
			{
				componentManager = character.componentManager;
			}
			if (GodotObject.IsInstanceValid(componentManager))
			{
				componentManager.RemoveRuntimeComponent(_changeProjectileComponent);
			}
		}
		_changeProjectileComponent = null;
		CleanupTorchwoodVisual();
	}

	private void CleanupTorchwoodVisual()
	{
		if (GodotObject.IsInstanceValid(torchwoodSprite))
		{
			if (torchwoodSprite is AdobeAnimateSprite child && GodotObject.IsInstanceValid(character))
			{
				character.DetachAnimatedStatusVisual(child);
			}
			torchwoodSprite.QueueFree();
		}
		torchwoodSprite = null;
	}

	private void SetupDeathFreeze()
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		ComponentManager componentManager = character.componentManager;
		if (GodotObject.IsInstanceValid(componentManager))
		{
			if (_explodeStateMachine == null)
			{
				_explodeStateMachine = GD.Load<StateMachineDefinition>("res://Script/Component/TowerDefense/Character/ExplodeComponent/ExplodeComponentStateMachine.tres");
			}
			TowerDefenseCharacterEventForzen towerDefenseCharacterEventForzen = new TowerDefenseCharacterEventForzen();
			towerDefenseCharacterEventForzen.time = deathFreezeTime;
			towerDefenseCharacterEventForzen.iceSpeedDownTime = deathSlowTime;
			ExplodeComponentDefinition definition = new ExplodeComponentDefinition
			{
				ComponentTypeId = "ExplodeComponent",
				DefinitionId = "buff.hypnoses.death_freeze.explode",
				InstanceId = "buff.hypnoses.death_freeze.explode",
				WireIndex = -1,
				StateMachineDefinition = _explodeStateMachine,
				reload = false,
				explodeMethod = "Line",
				explodeJalaFireType = "IceFire",
				explodeJalaOffset = new Array<int> { 0 },
				explodeJalaNum = (float)deathDamage,
				explodeAudio = "",
				cameraShakeUse = false,
				screenColorBlinkUse = false,
				craterCreateUse = false,
				explodeEvent = new Array<TowerDefenseCharacterEventBase> { towerDefenseCharacterEventForzen }
			};
			_explodeComponent = componentManager.AddRuntimeComponent(definition) as ExplodeComponent;
		}
	}

	private void RefreshDeathFreezeRuntime()
	{
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent == null || explodeComponent.IsReleased)
		{
			return;
		}
		_explodeComponent.explodeJalaNum = (float)deathDamage;
		TowerDefenseCharacterEventForzen towerDefenseCharacterEventForzen = null;
		for (int i = 0; i < _explodeComponent.explodeEvent.Count; i++)
		{
			if (_explodeComponent.explodeEvent[i] is TowerDefenseCharacterEventForzen towerDefenseCharacterEventForzen2)
			{
				towerDefenseCharacterEventForzen = towerDefenseCharacterEventForzen2;
				break;
			}
		}
		if (towerDefenseCharacterEventForzen == null)
		{
			towerDefenseCharacterEventForzen = new TowerDefenseCharacterEventForzen();
			_explodeComponent.explodeEvent.Add(towerDefenseCharacterEventForzen);
		}
		towerDefenseCharacterEventForzen.time = deathFreezeTime;
		towerDefenseCharacterEventForzen.iceSpeedDownTime = deathSlowTime;
	}

	private void CleanupDeathFreeze()
	{
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			ComponentManager componentManager = _explodeComponent.Manager;
			if (!GodotObject.IsInstanceValid(componentManager) && GodotObject.IsInstanceValid(character))
			{
				componentManager = character.componentManager;
			}
			if (GodotObject.IsInstanceValid(componentManager))
			{
				componentManager.RemoveRuntimeComponent(_explodeComponent);
			}
		}
		_explodeComponent = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(29)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCharacterFacingSign, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sign", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTwoCellPlantMirrorAnchor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mirrored", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMirrorAnchorRecursive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "mirrored", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySpriteMirrorAnchor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "mirrored", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasHorizontalGridExtension, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plantConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSpritePlaybackArtCenterX, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncPresentationReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupBattleState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Cancel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshGroundMoveDirectionCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupSunProduce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupSunProduce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupTorchwood, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncTorchwoodVisualReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupTorchwoodVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupTorchwood, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupTorchwoodVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupDeathFreeze, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshDeathFreezeRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupDeathFreeze, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Init && args.Count == 0)
		{
			_Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Enter && args.Count == 0)
		{
			Enter();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient && args.Count == 0)
		{
			EnterReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCharacterFacingSign && args.Count == 1)
		{
			ApplyCharacterFacingSign(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTwoCellPlantMirrorAnchor && args.Count == 1)
		{
			ApplyTwoCellPlantMirrorAnchor(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMirrorAnchorRecursive && args.Count == 2)
		{
			ApplyMirrorAnchorRecursive(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpriteMirrorAnchor && args.Count == 2)
		{
			ApplySpriteMirrorAnchor(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasHorizontalGridExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasHorizontalGridExtension(VariantUtils.ConvertTo<TowerDefensePlantConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveSpritePlaybackArtCenterX && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(ResolveSpritePlaybackArtCenterX(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.SyncPresentationReadOnlyClient && args.Count == 0)
		{
			SyncPresentationReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupBattleState && args.Count == 0)
		{
			CleanupBattleState();
			ret = default;
			return true;
		}
		if (method == MethodName.Step && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Step(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.StepReadOnlyClient && args.Count == 1)
		{
			StepReadOnlyClient(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient && args.Count == 0)
		{
			ExitReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.Cancel && args.Count == 0)
		{
			Cancel();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshGroundMoveDirectionCache && args.Count == 0)
		{
			RefreshGroundMoveDirectionCache();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupSunProduce && args.Count == 0)
		{
			SetupSunProduce();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupSunProduce && args.Count == 0)
		{
			CleanupSunProduce();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupTorchwood && args.Count == 0)
		{
			SetupTorchwood();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncTorchwoodVisualReadOnlyClient && args.Count == 0)
		{
			SyncTorchwoodVisualReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupTorchwoodVisual && args.Count == 0)
		{
			SetupTorchwoodVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupTorchwood && args.Count == 0)
		{
			CleanupTorchwood();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupTorchwoodVisual && args.Count == 0)
		{
			CleanupTorchwoodVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupDeathFreeze && args.Count == 0)
		{
			SetupDeathFreeze();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDeathFreezeRuntime && args.Count == 0)
		{
			RefreshDeathFreezeRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupDeathFreeze && args.Count == 0)
		{
			CleanupDeathFreeze();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyMirrorAnchorRecursive && args.Count == 2)
		{
			ApplyMirrorAnchorRecursive(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpriteMirrorAnchor && args.Count == 2)
		{
			ApplySpriteMirrorAnchor(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasHorizontalGridExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasHorizontalGridExtension(VariantUtils.ConvertTo<TowerDefensePlantConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveSpritePlaybackArtCenterX && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(ResolveSpritePlaybackArtCenterX(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanupBattleState && args.Count == 0)
		{
			CleanupBattleState();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.ApplyCharacterFacingSign)
		{
			return true;
		}
		if (method == MethodName.ApplyTwoCellPlantMirrorAnchor)
		{
			return true;
		}
		if (method == MethodName.ApplyMirrorAnchorRecursive)
		{
			return true;
		}
		if (method == MethodName.ApplySpriteMirrorAnchor)
		{
			return true;
		}
		if (method == MethodName.HasHorizontalGridExtension)
		{
			return true;
		}
		if (method == MethodName.ResolveSpritePlaybackArtCenterX)
		{
			return true;
		}
		if (method == MethodName.SyncPresentationReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.CleanupBattleState)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		if (method == MethodName.StepReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Cancel)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.RefreshGroundMoveDirectionCache)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.SetupSunProduce)
		{
			return true;
		}
		if (method == MethodName.CleanupSunProduce)
		{
			return true;
		}
		if (method == MethodName.SetupTorchwood)
		{
			return true;
		}
		if (method == MethodName.SyncTorchwoodVisualReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.SetupTorchwoodVisual)
		{
			return true;
		}
		if (method == MethodName.CleanupTorchwood)
		{
			return true;
		}
		if (method == MethodName.CleanupTorchwoodVisual)
		{
			return true;
		}
		if (method == MethodName.SetupDeathFreeze)
		{
			return true;
		}
		if (method == MethodName.RefreshDeathFreezeRuntime)
		{
			return true;
		}
		if (method == MethodName.CleanupDeathFreeze)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.deathFreezeOver)
		{
			deathFreezeOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.saveCamp)
		{
			saveCamp = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in value);
			return true;
		}
		if (name == PropertyName.enableTorchwood)
		{
			enableTorchwood = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.torchwoodChangeName)
		{
			torchwoodChangeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.torchwoodAudio)
		{
			torchwoodAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.torchwoodAreaSize)
		{
			torchwoodAreaSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.enableDeathFreeze)
		{
			enableDeathFreeze = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.deathFreezeTime)
		{
			deathFreezeTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.deathSlowTime)
		{
			deathSlowTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.deathDamage)
		{
			deathDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enableSunProduce)
		{
			enableSunProduce = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunProduceInterval)
		{
			sunProduceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.sunProduceNum)
		{
			sunProduceNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.torchwoodSprite)
		{
			torchwoodSprite = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._deathFreezeOver)
		{
			_deathFreezeOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.FrameMeshColorMultiplier)
		{
			value = VariantUtils.CreateFrom<Color>(FrameMeshColorMultiplier);
			return true;
		}
		if (name == PropertyName.deathFreezeOver)
		{
			value = VariantUtils.CreateFrom<bool>(deathFreezeOver);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName.saveCamp)
		{
			value = VariantUtils.CreateFrom(in saveCamp);
			return true;
		}
		if (name == PropertyName.enableTorchwood)
		{
			value = VariantUtils.CreateFrom(in enableTorchwood);
			return true;
		}
		if (name == PropertyName.torchwoodChangeName)
		{
			value = VariantUtils.CreateFrom(in torchwoodChangeName);
			return true;
		}
		if (name == PropertyName.torchwoodAudio)
		{
			value = VariantUtils.CreateFrom(in torchwoodAudio);
			return true;
		}
		if (name == PropertyName.torchwoodAreaSize)
		{
			value = VariantUtils.CreateFrom(in torchwoodAreaSize);
			return true;
		}
		if (name == PropertyName.enableDeathFreeze)
		{
			value = VariantUtils.CreateFrom(in enableDeathFreeze);
			return true;
		}
		if (name == PropertyName.deathFreezeTime)
		{
			value = VariantUtils.CreateFrom(in deathFreezeTime);
			return true;
		}
		if (name == PropertyName.deathSlowTime)
		{
			value = VariantUtils.CreateFrom(in deathSlowTime);
			return true;
		}
		if (name == PropertyName.deathDamage)
		{
			value = VariantUtils.CreateFrom(in deathDamage);
			return true;
		}
		if (name == PropertyName.enableSunProduce)
		{
			value = VariantUtils.CreateFrom(in enableSunProduce);
			return true;
		}
		if (name == PropertyName.sunProduceInterval)
		{
			value = VariantUtils.CreateFrom(in sunProduceInterval);
			return true;
		}
		if (name == PropertyName.sunProduceNum)
		{
			value = VariantUtils.CreateFrom(in sunProduceNum);
			return true;
		}
		if (name == PropertyName.torchwoodSprite)
		{
			value = VariantUtils.CreateFrom(in torchwoodSprite);
			return true;
		}
		if (name == PropertyName._deathFreezeOver)
		{
			value = VariantUtils.CreateFrom(in _deathFreezeOver);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Color, PropertyName.FrameMeshColorMultiplier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.saveCamp, PropertyHint.Enum, "NOONE:-1,PLANT:0,ZOMBIE:1,ALL:2", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enableTorchwood, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.torchwoodChangeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.torchwoodAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.torchwoodAreaSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enableDeathFreeze, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.deathFreezeTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.deathSlowTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.deathDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enableSunProduce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.sunProduceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunProduceNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.torchwoodSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._deathFreezeOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.deathFreezeOver, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.deathFreezeOver, Variant.From<bool>(deathFreezeOver));
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName.saveCamp, Variant.From(in saveCamp));
		info.AddProperty(PropertyName.enableTorchwood, Variant.From(in enableTorchwood));
		info.AddProperty(PropertyName.torchwoodChangeName, Variant.From(in torchwoodChangeName));
		info.AddProperty(PropertyName.torchwoodAudio, Variant.From(in torchwoodAudio));
		info.AddProperty(PropertyName.torchwoodAreaSize, Variant.From(in torchwoodAreaSize));
		info.AddProperty(PropertyName.enableDeathFreeze, Variant.From(in enableDeathFreeze));
		info.AddProperty(PropertyName.deathFreezeTime, Variant.From(in deathFreezeTime));
		info.AddProperty(PropertyName.deathSlowTime, Variant.From(in deathSlowTime));
		info.AddProperty(PropertyName.deathDamage, Variant.From(in deathDamage));
		info.AddProperty(PropertyName.enableSunProduce, Variant.From(in enableSunProduce));
		info.AddProperty(PropertyName.sunProduceInterval, Variant.From(in sunProduceInterval));
		info.AddProperty(PropertyName.sunProduceNum, Variant.From(in sunProduceNum));
		info.AddProperty(PropertyName.torchwoodSprite, Variant.From(in torchwoodSprite));
		info.AddProperty(PropertyName._deathFreezeOver, Variant.From(in _deathFreezeOver));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.deathFreezeOver, out var value))
		{
			deathFreezeOver = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value2))
		{
			time = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value3))
		{
			currentTime = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.saveCamp, out var value4))
		{
			saveCamp = value4.As<TowerDefenseEnum.CHARACTER_CAMP>();
		}
		if (info.TryGetProperty(PropertyName.enableTorchwood, out var value5))
		{
			enableTorchwood = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.torchwoodChangeName, out var value6))
		{
			torchwoodChangeName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.torchwoodAudio, out var value7))
		{
			torchwoodAudio = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.torchwoodAreaSize, out var value8))
		{
			torchwoodAreaSize = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.enableDeathFreeze, out var value9))
		{
			enableDeathFreeze = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.deathFreezeTime, out var value10))
		{
			deathFreezeTime = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.deathSlowTime, out var value11))
		{
			deathSlowTime = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.deathDamage, out var value12))
		{
			deathDamage = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enableSunProduce, out var value13))
		{
			enableSunProduce = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunProduceInterval, out var value14))
		{
			sunProduceInterval = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.sunProduceNum, out var value15))
		{
			sunProduceNum = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName.torchwoodSprite, out var value16))
		{
			torchwoodSprite = value16.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._deathFreezeOver, out var value17))
		{
			_deathFreezeOver = value17.As<bool>();
		}
	}
}
