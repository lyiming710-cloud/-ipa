using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseZombie.cs")]
public class TowerDefenseZombie : TowerDefenseCharacter, IStateMachinePhysicsFastCallbackTarget
{
	public new class MethodName : TowerDefenseCharacter.MethodName
	{
		public new static readonly StringName OnRiseStart = "OnRiseStart";

		public new static readonly StringName OnRiseEnd = "OnRiseEnd";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConnectZombieStateHandles = "ConnectZombieStateHandles";

		public static readonly StringName DisconnectZombieStateHandles = "DisconnectZombieStateHandles";

		public static readonly StringName AutoRegisterSync = "AutoRegisterSync";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EnsureShowZombieHealthHandler = "EnsureShowZombieHealthHandler";

		public static readonly StringName SubscribeShowZombieHealth = "SubscribeShowZombieHealth";

		public static readonly StringName UnsubscribeShowZombieHealth = "UnsubscribeShowZombieHealth";

		public static readonly StringName OnShowZombieHealth = "OnShowZombieHealth";

		public static readonly StringName OnShowBossHealthBar = "OnShowBossHealthBar";

		public new static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public static readonly StringName ConfigureWaterLineVisualLayers = "ConfigureWaterLineVisualLayers";

		public static readonly StringName RefreshWaterLineVisualLayers = "RefreshWaterLineVisualLayers";

		public static readonly StringName RefreshWaterLineState = "RefreshWaterLineState";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName BatchUpdateValidated = "BatchUpdateValidated";

		public static readonly StringName BatchUpdateCore = "BatchUpdateCore";

		public static readonly StringName ConfigureBatchProcessing = "ConfigureBatchProcessing";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName IsZombieRewindActive = "IsZombieRewindActive";

		public static readonly StringName CanStartAttackFromCurrentPosition = "CanStartAttackFromCurrentPosition";

		public static readonly StringName GarlicEntered = "GarlicEntered";

		public static readonly StringName GarlicProcessing = "GarlicProcessing";

		public static readonly StringName GarlicExited = "GarlicExited";

		public static readonly StringName WalkEntered = "WalkEntered";

		public static readonly StringName ActivateWalkMovement = "ActivateWalkMovement";

		public static readonly StringName WalkWithoutTransitionDelay = "WalkWithoutTransitionDelay";

		public static readonly StringName WalkProcessing = "WalkProcessing";

		public static readonly StringName WalkExited = "WalkExited";

		public static readonly StringName AttackEntered = "AttackEntered";

		public static readonly StringName AttackProcessing = "AttackProcessing";

		public static readonly StringName AttackExited = "AttackExited";

		public static readonly StringName DieEntered = "DieEntered";

		public static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName DieExited = "DieExited";

		public static readonly StringName WalkReady = "WalkReady";

		public static readonly StringName Walk = "Walk";

		public static readonly StringName EnsureZeroHealthDeath = "EnsureZeroHealthDeath";

		public static readonly StringName ConvergeZeroHealthDeath = "ConvergeZeroHealthDeath";

		public new static readonly StringName NormalizeMainStateEvent = "NormalizeMainStateEvent";

		public static readonly StringName Attack = "Attack";

		public static readonly StringName Die = "Die";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName Garlic = "Garlic";

		public static readonly StringName ChangeLine = "ChangeLine";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public static readonly StringName InWaterDiscardSet = "InWaterDiscardSet";

		public static readonly StringName OutWaterDiscardSet = "OutWaterDiscardSet";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";
	}

	public new class PropertyName : TowerDefenseCharacter.PropertyName
	{
		public static readonly StringName groan = "groan";

		public static readonly StringName walkSpeedScale = "walkSpeedScale";

		public static readonly StringName inSwimAnimeClipScale = "inSwimAnimeClipScale";

		public static readonly StringName useAttackDps = "useAttackDps";

		public static readonly StringName walkAnimeClip = "walkAnimeClip";

		public static readonly StringName inSwimAnimeClip = "inSwimAnimeClip";

		public static readonly StringName swimAnimeClip = "swimAnimeClip";

		public static readonly StringName outSwimAnimeClip = "outSwimAnimeClip";

		public static readonly StringName attackAnimeClip = "attackAnimeClip";

		public static readonly StringName attackWaterAnimeClip = "attackWaterAnimeClip";

		public static readonly StringName dieAnimeClip = "dieAnimeClip";

		public static readonly StringName dieWaterAnimeClip = "dieWaterAnimeClip";

		public static readonly StringName duckytobeSprite = "duckytobeSprite";

		public static readonly StringName waterLineSprite = "waterLineSprite";

		public static readonly StringName waterAnimeFliter = "waterAnimeFliter";

		public static readonly StringName garlicFliters = "garlicFliters";

		public static readonly StringName garlicReplace = "garlicReplace";

		public static readonly StringName waterHeight = "waterHeight";

		public static readonly StringName IsBatchDispatchActive = "IsBatchDispatchActive";

		public static readonly StringName isPause = "isPause";

		public static readonly StringName _garlicFliters = "_garlicFliters";

		public static readonly StringName _zombieStateHandlesConnected = "_zombieStateHandlesConnected";

		public static readonly StringName _showZombieHealthEventBus = "_showZombieHealthEventBus";

		public static readonly StringName _showZombieHealthSubscribed = "_showZombieHealthSubscribed";

		public static readonly StringName _walkMovementActivationVersion = "_walkMovementActivationVersion";

		public static readonly StringName _walkWithoutTransitionDelay = "_walkWithoutTransitionDelay";

		public static readonly StringName _walkReadyGeneration = "_walkReadyGeneration";

		public static readonly StringName _groanPlaybackGeneration = "_groanPlaybackGeneration";

		public static readonly StringName _isPause = "_isPause";

		public static readonly StringName isGarlic = "isGarlic";

		public static readonly StringName isGarlicBird = "isGarlicBird";

		public static readonly StringName isChangeLine = "isChangeLine";

		public static readonly StringName inSwimPlay = "inSwimPlay";

		public static readonly StringName inGround = "inGround";

		public static readonly StringName startAttack = "startAttack";

		public static readonly StringName sizeUpNum = "sizeUpNum";

		public static readonly StringName hasGhost = "hasGhost";

		public static readonly StringName ghostCharacter = "ghostCharacter";

		public static readonly StringName hasSpikeball = "hasSpikeball";

		public static readonly StringName riderCarryOwner = "riderCarryOwner";

		public static readonly StringName spritePause = "spritePause";

		public static readonly StringName _gridSize = "_gridSize";

		public static readonly StringName _validatedBatchDispatchActive = "_validatedBatchDispatchActive";

		public static readonly StringName _validatedBatchPhysicsFrame = "_validatedBatchPhysicsFrame";

		public static readonly StringName _ensuringZeroHealthDeath = "_ensuringZeroHealthDeath";
	}

	public new class SignalName : TowerDefenseCharacter.SignalName
	{
	}

	private static readonly StringName DieStateEvent = "ToDie";

	private const string DieStateId = "zombie.die";

	private const int GarlicPhysicsCallbackId = 1;

	private const int WalkPhysicsCallbackId = 2;

	private const int AttackPhysicsCallbackId = 3;

	private const int DiePhysicsCallbackId = 4;

	public WaterInteractionComponent waterInteractionComponent;

	public GroundMoveComponent groundMoveComponent;

	public AttackComponent attackComponent;

	public GarlicComponent garlicComponent;

	public SwimComponent swimComponent;

	public ZombieDeathComponent zombieDeathComponent;

	private Array<string> _garlicFliters;

	public static bool UseBatch = true;

	private StateHandle _garlicStateHandle;

	private StateHandle _walkStateHandle;

	private StateHandle _attackStateHandle;

	private StateHandle _dieStateHandle;

	private List<StringName> _waterLineVisualLayers;

	private bool _zombieStateHandlesConnected;

	private BattleEventBus.ShowZombieHealthEventHandler _showZombieHealthHandler;

	private BattleEventBus.ShowBossHealthBarEventHandler _showBossHealthBarHandler;

	private BattleEventBus _showZombieHealthEventBus;

	private bool _showZombieHealthSubscribed;

	private ulong _walkMovementActivationVersion;

	private bool _walkWithoutTransitionDelay;

	private ulong _walkReadyGeneration;

	private ulong _groanPlaybackGeneration;

	private bool _isPause;

	public bool isGarlic;

	public bool isGarlicBird;

	public bool isChangeLine;

	public bool inSwimPlay;

	public bool inGround;

	public bool startAttack;

	public int sizeUpNum = 2;

	public bool hasGhost;

	public TowerDefenseZombie ghostCharacter;

	public bool hasSpikeball;

	public TowerDefenseZombie riderCarryOwner;

	public BobsledTeamComponent bobsledTeamOwner;

	public bool spritePause;

	public Vector2 _gridSize;

	private bool _validatedBatchDispatchActive;

	private ulong _validatedBatchPhysicsFrame;

	private bool _ensuringZeroHealthDeath;

	[Export(PropertyHint.None, "")]
	public string groan { get; set; }

	[Export(PropertyHint.None, "")]
	public double walkSpeedScale { get; set; } = 1.0;

	[Export(PropertyHint.None, "")]
	public double inSwimAnimeClipScale { get; set; } = 1.0;

	[Export(PropertyHint.None, "")]
	public bool useAttackDps { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public string walkAnimeClip { get; set; } = "Walk";

	[Export(PropertyHint.None, "")]
	public string inSwimAnimeClip { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public string swimAnimeClip { get; set; } = "Walk";

	[Export(PropertyHint.None, "")]
	public string outSwimAnimeClip { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public string attackAnimeClip { get; set; } = "Eat";

	[Export(PropertyHint.None, "")]
	public string attackWaterAnimeClip { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public string dieAnimeClip { get; set; } = "Death";

	[Export(PropertyHint.None, "")]
	public string dieWaterAnimeClip { get; set; } = "Death";

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite duckytobeSprite { get; set; }

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite waterLineSprite { get; set; }

	[Export(PropertyHint.MultilineText, "")]
	public string waterAnimeFliter { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Array<string> garlicFliters
	{
		get
		{
			Array<string> array = _garlicFliters;
			if (array == null)
			{
				Array<string> array2 = new Array<string> { "anim_head2", "anim_tongue" };
				Array<string> array3 = array2;
				_garlicFliters = array2;
				array = array3;
			}
			return array;
		}
		set
		{
			_garlicFliters = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public string garlicReplace { get; set; } = "Zombie_head.png";

	[Export(PropertyHint.None, "")]
	public double waterHeight { get; set; } = 25.0;

	internal bool IsBatchDispatchActive
	{
		get
		{
			if (IsOwnerBatchRegistered)
			{
				return !_isPause;
			}
			return false;
		}
	}

	protected StateHandle WalkStateHandle => _walkStateHandle;

	protected StateHandle AttackStateHandle => _attackStateHandle;

	public bool isPause
	{
		get
		{
			return _isPause;
		}
		set
		{
			_isPause = value;
			if (!IsNodeReady())
			{
				return;
			}
			SetMainStateMachineDispatchEnabled(!_isPause);
			if (!GodotObject.IsInstanceValid(sprite))
			{
				return;
			}
			IStateMachineController stateMachine = StateMachine;
			if (stateMachine == null || !stateMachine.IsInitialized || !HasHitBox)
			{
				return;
			}
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent == null || groundHeightComponent.IsReleased)
			{
				return;
			}
			if (_isPause)
			{
				sprite.pause = true;
				SetHitBoxSuppressed(HitBoxSuppressionReason.Paused, suppressed: true);
				if (IsOwnerBatchRegistered)
				{
					TowerDefenseZombieBatch.Unregister(this);
				}
				SetPhysicsProcess(enable: false);
				base.groundHeightComponent.SetAlive(alive: false);
			}
			else
			{
				sprite.pause = false;
				SetHitBoxSuppressed(HitBoxSuppressionReason.Paused, suppressed: false);
				ConfigureBatchProcessing();
				base.groundHeightComponent.SetAlive(alive: true);
			}
		}
	}

	public override void OnRiseStart()
	{
		Idle();
		AttackComponent attackComponent = this.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			this.attackComponent.alive = false;
		}
	}

	public override void OnRiseEnd()
	{
		Walk();
		AttackComponent attackComponent = this.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			this.attackComponent.alive = true;
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		if (!TowerDefenseCharacter.CachedEditorHint && IsNodeReady() && inGame)
		{
			SubscribeShowZombieHealth();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (editorPreviewMode)
		{
			SetPhysicsProcess(enable: false);
		}
		else
		{
			if (Engine.IsEditorHint() || !HasValidRuntimeConfiguration)
			{
				return;
			}
			EnsureShowZombieHealthHandler();
			if (GodotObject.IsInstanceValid(componentManager))
			{
				this.waterInteractionComponent = componentManager.GetRuntime<WaterInteractionComponent>();
				groundMoveComponent = componentManager.GetRuntime<GroundMoveComponent>();
				zombieDeathComponent = componentManager.GetRuntime<ZombieDeathComponent>();
				garlicComponent = componentManager.GetRuntime<GarlicComponent>();
				swimComponent = componentManager.GetRuntime<SwimComponent>();
				this.attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
				WaterInteractionComponent waterInteractionComponent = this.waterInteractionComponent;
				if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
				{
					this.waterInteractionComponent.shadowComponent = shadowComponent;
					if (GodotObject.IsInstanceValid(waterLineSprite))
					{
						this.waterInteractionComponent.waterLineSprite = waterLineSprite as AdobeAnimateSpriteBase;
					}
					if (GodotObject.IsInstanceValid(duckytobeSprite))
					{
						this.waterInteractionComponent.duckytobeSprite = duckytobeSprite as AdobeAnimateSpriteBase;
					}
				}
				GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
				if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
				{
					WaterInteractionComponent waterInteractionComponent2 = this.waterInteractionComponent;
					if (waterInteractionComponent2 != null && !waterInteractionComponent2.IsReleased)
					{
						base.groundHeightComponent.waterInteractionComponent = this.waterInteractionComponent;
					}
				}
			}
			if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
			{
				timeScale += GD.RandRange(-0.1, 0.1);
			}
			AddToGroup("Zombie", persistent: true);
			if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
			{
				walkSpeedScale += walkSpeedScale * GD.RandRange(-0.1, 0.1);
			}
			instance.hitpointsEmpty += Die;
			RefreshWaterLineState();
			if (GodotObject.IsInstanceValid(cell) && cell.isWater && (instance.maskFlags & 2) == 0)
			{
				inWater = true;
			}
			_gridSize = TowerDefenseManager.Instance.GetMapGridSize();
			ShowHealthComponent showHealthComponent = base.showHealthComponent;
			if (showHealthComponent != null && !showHealthComponent.IsReleased)
			{
				RefreshShowHealthVisibility();
			}
			if (inGame)
			{
				SubscribeShowZombieHealth();
			}
			if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
			{
				AttackComponent attackComponent = this.attackComponent;
				if (attackComponent != null && !attackComponent.IsReleased)
				{
					this.attackComponent.alive = false;
					this.attackComponent.SetAlive(alive: false);
				}
			}
			if (inGame && Global.IsMultiplayerMode && MultiPlayerManager.IsHost && syncId < 0)
			{
				CallDeferred(MethodName.AutoRegisterSync);
			}
			ConnectZombieStateHandles();
			ConfigureBatchProcessing();
		}
	}

	private void ConnectZombieStateHandles()
	{
		if (_zombieStateHandlesConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return;
		}
		_garlicStateHandle = StateMachine?.GetStateById("zombie.garlic");
		_walkStateHandle = StateMachine?.GetStateById("zombie.walk");
		_attackStateHandle = StateMachine?.GetStateById("zombie.attack");
		_dieStateHandle = StateMachine?.GetStateById("zombie.die");
		if (_garlicStateHandle != null)
		{
			_garlicStateHandle.Entered += GarlicEntered;
			_garlicStateHandle.Exited += GarlicExited;
			if (!_garlicStateHandle.TrySetPhysicsFastCallback(this, 1))
			{
				_garlicStateHandle.PhysicsProcessing += GarlicProcessing;
			}
		}
		if (_walkStateHandle != null)
		{
			_walkStateHandle.Entered += WalkEntered;
			_walkStateHandle.Exited += WalkExited;
			if (!_walkStateHandle.TrySetPhysicsFastCallback(this, 2))
			{
				_walkStateHandle.PhysicsProcessing += WalkProcessing;
			}
		}
		if (_attackStateHandle != null)
		{
			_attackStateHandle.Entered += AttackEntered;
			_attackStateHandle.Exited += AttackExited;
			if (!_attackStateHandle.TrySetPhysicsFastCallback(this, 3))
			{
				_attackStateHandle.PhysicsProcessing += AttackProcessing;
			}
		}
		if (_dieStateHandle != null)
		{
			_dieStateHandle.Entered += DieEntered;
			if (!_dieStateHandle.TrySetPhysicsFastCallback(this, 4))
			{
				_dieStateHandle.PhysicsProcessing += DieProcessing;
			}
		}
		_zombieStateHandlesConnected = true;
	}

	private void DisconnectZombieStateHandles()
	{
		if (!_zombieStateHandlesConnected)
		{
			return;
		}
		if (_garlicStateHandle != null)
		{
			_garlicStateHandle.Entered -= GarlicEntered;
			_garlicStateHandle.Exited -= GarlicExited;
			if (!_garlicStateHandle.ClearPhysicsFastCallback(this))
			{
				_garlicStateHandle.PhysicsProcessing -= GarlicProcessing;
			}
		}
		if (_walkStateHandle != null)
		{
			_walkStateHandle.Entered -= WalkEntered;
			_walkStateHandle.Exited -= WalkExited;
			if (!_walkStateHandle.ClearPhysicsFastCallback(this))
			{
				_walkStateHandle.PhysicsProcessing -= WalkProcessing;
			}
		}
		if (_attackStateHandle != null)
		{
			_attackStateHandle.Entered -= AttackEntered;
			_attackStateHandle.Exited -= AttackExited;
			if (!_attackStateHandle.ClearPhysicsFastCallback(this))
			{
				_attackStateHandle.PhysicsProcessing -= AttackProcessing;
			}
		}
		if (_dieStateHandle != null)
		{
			_dieStateHandle.Entered -= DieEntered;
			if (!_dieStateHandle.ClearPhysicsFastCallback(this))
			{
				_dieStateHandle.PhysicsProcessing -= DieProcessing;
			}
		}
		_garlicStateHandle = null;
		_walkStateHandle = null;
		_attackStateHandle = null;
		_dieStateHandle = null;
		_zombieStateHandlesConnected = false;
	}

	void IStateMachinePhysicsFastCallbackTarget.InvokeStateMachinePhysicsFastCallback(int callbackId, double delta)
	{
		switch (callbackId)
		{
		case 1:
			GarlicProcessing(delta);
			break;
		case 2:
			WalkProcessing(delta);
			break;
		case 3:
			AttackProcessing(delta);
			break;
		case 4:
			DieProcessing(delta);
			break;
		}
	}

	private void AutoRegisterSync()
	{
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.IsHost || syncId >= 0 || !GodotObject.IsInstanceValid(this) || isDestroy || isShow)
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (GodotObject.IsInstanceValid(currentControl))
		{
			int nextSyncId = currentControl.GetNextSyncId();
			currentControl.RegisterSyncCharacter(nextSyncId, this);
			if (GodotObject.IsInstanceValid(config))
			{
				MultiPlayerManager.Instance.SendSpawnCharacterAt(config.name, gridPos.X, gridPos.Y, nextSyncId, GodotObject.IsInstanceValid(instance) ? instance.hitpointScale : 1.0, transformPoint.Scale.X, GodotObject.IsInstanceValid(instance) && instance.hypnoses, 0.0, useCreate: true, GetLogicalGlobalPosition().X, GetLogicalGlobalPosition().Y);
			}
		}
	}

	public override void _ExitTree()
	{
		_walkReadyGeneration++;
		_groanPlaybackGeneration++;
		startAttack = false;
		base._ExitTree();
		if (!Engine.IsEditorHint())
		{
			if (IsOwnerBatchRegistered)
			{
				TowerDefenseZombieBatch.Unregister(this);
			}
			UnsubscribeShowZombieHealth();
		}
	}

	private void EnsureShowZombieHealthHandler()
	{
		if (_showZombieHealthHandler == null)
		{
			_showZombieHealthHandler = OnShowZombieHealth;
		}
		if (_showBossHealthBarHandler == null)
		{
			_showBossHealthBarHandler = OnShowBossHealthBar;
		}
	}

	private void SubscribeShowZombieHealth()
	{
		if (!_showZombieHealthSubscribed || !GodotObject.IsInstanceValid(_showZombieHealthEventBus))
		{
			_showZombieHealthSubscribed = false;
			_showZombieHealthEventBus = null;
			BattleEventBus battleEventBus = BattleEventBus.Instance;
			if (GodotObject.IsInstanceValid(battleEventBus))
			{
				EnsureShowZombieHealthHandler();
				battleEventBus.OnShowZombieHealth += _showZombieHealthHandler;
				battleEventBus.OnShowBossHealthBar += _showBossHealthBarHandler;
				_showZombieHealthEventBus = battleEventBus;
				_showZombieHealthSubscribed = true;
			}
		}
	}

	private void UnsubscribeShowZombieHealth()
	{
		if (_showZombieHealthSubscribed && GodotObject.IsInstanceValid(_showZombieHealthEventBus) && _showZombieHealthHandler != null)
		{
			_showZombieHealthEventBus.OnShowZombieHealth -= _showZombieHealthHandler;
			_showZombieHealthEventBus.OnShowBossHealthBar -= _showBossHealthBarHandler;
		}
		_showZombieHealthSubscribed = false;
		_showZombieHealthEventBus = null;
	}

	private void OnShowZombieHealth(bool show)
	{
		RefreshShowHealthVisibility(show);
	}

	private void OnShowBossHealthBar(bool show)
	{
		bool? showBossHealthBar = show;
		RefreshShowHealthVisibility(null, showBossHealthBar);
	}

	private void RefreshShowHealthVisibility(bool? showZombieHealth = null, bool? showBossHealthBar = null)
	{
		ShowHealthComponent showHealthComponent = base.showHealthComponent;
		if (showHealthComponent != null && !showHealthComponent.IsReleased)
		{
			bool flag = showZombieHealth ?? GameSaveManager.Instance.GetConfigValue("ShowZombieHealth").AsBool();
			bool flag2 = showBossHealthBar ?? GameSaveManager.Instance.GetConfigValue("ShowBossHealthBar").AsBool();
			bool flag3 = (GodotObject.IsInstanceValid(instance) && instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS) & flag2;
			base.showHealthComponent.SetAlive(flag && !flag3);
		}
	}

	public override void ActivateGameplayProcessing()
	{
		base.ActivateGameplayProcessing();
		ConfigureBatchProcessing();
		groundHeightComponent?.SetAlive(alive: true);
		ReactivateAttackComponent(attackComponent);
	}

	protected void ConfigureWaterLineVisualLayers(params string[] layerNames)
	{
		if (layerNames == null)
		{
			return;
		}
		foreach (string text in layerNames)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				StringName item = new StringName(text);
				List<StringName> list = _waterLineVisualLayers ?? (_waterLineVisualLayers = new List<StringName>());
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		RefreshWaterLineVisualLayers();
	}

	private void RefreshWaterLineVisualLayers()
	{
		bool waterLine = TowerDefenseManager.MapLineHasType(gridPos.Y, TowerDefenseEnum.PLANTGRIDTYPE.WATER);
		RefreshWaterLineVisualLayers(waterLine);
	}

	private void RefreshWaterLineState()
	{
		bool flag = TowerDefenseManager.MapLineHasType(gridPos.Y, TowerDefenseEnum.PLANTGRIDTYPE.WATER);
		WaterInteractionComponent waterInteractionComponent = this.waterInteractionComponent;
		if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
		{
			this.waterInteractionComponent.inWaterLine = flag;
		}
		if (GodotObject.IsInstanceValid(duckytobeSprite))
		{
			duckytobeSprite.Visible = flag;
		}
		RefreshWaterLineVisualLayers(flag);
	}

	private void RefreshWaterLineVisualLayers(bool waterLine)
	{
		if (!GodotObject.IsInstanceValid(sprite) || _waterLineVisualLayers == null)
		{
			return;
		}
		foreach (StringName waterLineVisualLayer in _waterLineVisualLayers)
		{
			sprite.SetFliter(waterLineVisualLayer, waterLine);
		}
	}

	private static void ReactivateAttackComponent(AttackComponent component)
	{
		if (component != null && !component.IsReleased && component.alive)
		{
			component.alive = true;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsOwnerBatchRegistered)
		{
			BatchUpdate(delta);
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!TowerDefenseCharacter.CachedEditorHint && !_isPause)
		{
			if (_validatedBatchDispatchActive)
			{
				BatchUpdateCore(delta, _validatedBatchPhysicsFrame);
				return;
			}
			TowerDefenseProcessModeDispatch.BeginFrame();
			BatchUpdateCore(delta, TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
		}
	}

	internal void BatchUpdateValidated(double delta, ulong physicsFrame)
	{
		if (_validatedBatchDispatchActive)
		{
			BatchUpdateCore(delta, physicsFrame);
			return;
		}
		_validatedBatchDispatchActive = true;
		_validatedBatchPhysicsFrame = physicsFrame;
		try
		{
			BatchUpdate(delta);
		}
		finally
		{
			_validatedBatchDispatchActive = false;
		}
	}

	private void BatchUpdateCore(double delta, ulong physicsFrame)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		if (EnsureZeroHealthDeath() && CurrentStateHandle?.StableId != "zombie.die")
		{
			SendStateEvent(DieStateEvent);
		}
		TowerDefensePerfProfiler.End("batch.zombie.preBase", startTicks);
		long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
		PhysicsProcessWithFrame(delta, physicsFrame);
		TowerDefensePerfProfiler.End("batch.zombie.characterBase", startTicks2);
		long startTicks3 = TowerDefensePerfProfiler.BeginHotPath();
		if (!GodotObject.IsInstanceValid(sprite))
		{
			TowerDefensePerfProfiler.End("batch.zombie.postBase", startTicks3);
			return;
		}
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (towerDefenseManager == null)
		{
			TowerDefensePerfProfiler.End("batch.zombie.postBase", startTicks3);
			return;
		}
		sprite.pause = towerDefenseManager.pauseZombie || spritePause;
		sprite.playBack = towerDefenseManager.backZombie || isGarlicBird;
		Vector2 vector = default;
		bool flag = false;
		if (TowerDefenseProcessModeDispatch.HasMapFeatureForCurrentPhysicsFrame)
		{
			if (GodotObject.IsInstanceValid(cell))
			{
				Vector2 mapCellPos = towerDefenseManager.GetMapCellPos(gridPos);
				vector = GetGlobalPositionForPhysicsFrame(physicsFrame);
				flag = true;
				cellPercentage = (vector - mapCellPos).X / _gridSize.X;
			}
			else
			{
				inWater = false;
			}
		}
		if ((ulong)((long)physicsFrame + (long)randFreshIndex) % 30uL == 0L)
		{
			if (!flag)
			{
				vector = GetGlobalPositionForPhysicsFrame(physicsFrame);
			}
			if (!inGround && (double)vector.X < groundRight + 150.0)
			{
				inGround = true;
				if (inGame)
				{
					PlayGroanDelayed(++_groanPlaybackGeneration);
				}
			}
			float cachedLocalScaleX = CachedLocalScaleX;
			if (cachedLocalScaleX < 0f && (double)vector.X > groundRight + 150.0 && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
			{
				Destroy();
			}
			if (cachedLocalScaleX > 0f)
			{
				TowerDefenseProcessModeDispatch.TryGetComponentBattlefieldBoundsForCurrentPhysicsFrame(out var left, out var _);
				if (vector.X < left - 150f && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
				{
					Destroy();
				}
			}
			if (inWater)
			{
				InWaterDiscardSet();
			}
			else
			{
				OutWaterDiscardSet();
			}
		}
		TowerDefensePerfProfiler.End("batch.zombie.postBase", startTicks3);
	}

	private void ConfigureBatchProcessing()
	{
		if (!IsNodeReady() || Engine.IsEditorHint())
		{
			return;
		}
		if (!inGame)
		{
			if (IsOwnerBatchRegistered)
			{
				TowerDefenseZombieBatch.Unregister(this);
			}
			SetPhysicsProcess(enable: false);
		}
		else if (_isPause)
		{
			if (StateMachine != null)
			{
				SetMainStateMachineDispatchEnabled(enabled: false);
			}
			if (IsOwnerBatchRegistered)
			{
				TowerDefenseZombieBatch.Unregister(this);
			}
			SetPhysicsProcess(enable: false);
		}
		else if (UseBatch)
		{
			if (!TowerDefenseZombieBatch.Register(this))
			{
				SetOwnerBatchRegistration(registered: false);
			}
		}
		else if (IsOwnerBatchRegistered)
		{
			TowerDefenseZombieBatch.Unregister(this);
		}
		else
		{
			SetOwnerBatchRegistration(registered: false);
		}
	}

	private async Task PlayGroanDelayed(ulong playbackGeneration)
	{
		await ToSignal(GetTree().CreateTimer(GD.RandRange(0.5, 3.0), processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (playbackGeneration == _groanPlaybackGeneration && GodotObject.IsInstanceValid(this) && IsInsideTree() && inGame && !die && !nearDie && !isDestroy)
		{
			AudioManager.Instance.AudioPlay(groan);
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (isRise || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost))
		{
			return;
		}
		sprite.timeScale = timeScale;
		if (!IsZombieRewindActive() && CanStartAttackFromCurrentPosition())
		{
			AttackComponent attackComponent = this.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased && this.attackComponent.CanAttack())
			{
				Attack();
			}
		}
	}

	protected static bool IsZombieRewindActive()
	{
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(towerDefenseManager))
		{
			return towerDefenseManager.backZombie;
		}
		return false;
	}

	private bool CanStartAttackFromCurrentPosition()
	{
		if (inGame && !HasComponentGameplayUntilBattlefieldEntry && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && IsInsideComponentBattlefieldForRuntime)
		{
			return true;
		}
		if ((double)GetGlobalPositionForPhysicsFrame(TowerDefenseProcessModeDispatch.CurrentPhysicsFrame).X > groundRight)
		{
			return false;
		}
		return true;
	}

	public void GarlicEntered()
	{
	}

	public void GarlicProcessing(double delta)
	{
		sprite.timeScale = 0.0;
	}

	public void GarlicExited()
	{
	}

	public virtual void WalkEntered()
	{
		bool walkWithoutTransitionDelay = _walkWithoutTransitionDelay;
		SwimComponent swimComponent = this.swimComponent;
		if (swimComponent != null && !swimComponent.IsReleased)
		{
			try
			{
				this.swimComponent.WalkEntered(walkWithoutTransitionDelay ? new double?(0.0) : ((double?)null));
			}
			catch (Exception ex)
			{
				GD.PrintErr("WalkEntered: SwimComponent.WalkEntered() threw: " + ex.Message);
				sprite.SetAnimation(walkAnimeClip, loop: true, walkWithoutTransitionDelay ? 0.0 : 0.2);
			}
		}
		else
		{
			sprite.SetAnimation(walkAnimeClip, loop: true, walkWithoutTransitionDelay ? 0.0 : 0.2);
		}
		_walkMovementActivationVersion++;
		if (walkWithoutTransitionDelay)
		{
			ActivateWalkMovement(_walkMovementActivationVersion);
		}
		else
		{
			WalkEnteredDelay();
		}
	}

	private async Task WalkEnteredDelay()
	{
		ulong activationVersion = _walkMovementActivationVersion;
		if (GodotObject.IsInstanceValid(GetTree()))
		{
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		ActivateWalkMovement(activationVersion);
	}

	private void ActivateWalkMovement(ulong activationVersion)
	{
		if (activationVersion == _walkMovementActivationVersion && !die && !isDestroy && inGame)
		{
			GroundMoveComponent groundMoveComponent = this.groundMoveComponent;
			if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
			{
				this.groundMoveComponent.SetAlive(this.groundMoveComponent.HasMovementSource);
			}
		}
	}

	protected void WalkWithoutTransitionDelay()
	{
		_walkWithoutTransitionDelay = true;
		try
		{
			Walk();
		}
		finally
		{
			_walkWithoutTransitionDelay = false;
		}
	}

	public virtual void WalkProcessing(double delta)
	{
		SwimComponent swimComponent = this.swimComponent;
		if (swimComponent != null && !swimComponent.IsReleased)
		{
			AdobeAnimateSprite adobeAnimateSprite = sprite;
			if (_validatedBatchDispatchActive && adobeAnimateSprite != null)
			{
				swimComponent.WalkProcessingValidated(this, adobeAnimateSprite, _validatedBatchPhysicsFrame);
			}
			else
			{
				swimComponent.WalkProcessing((float)delta);
			}
		}
		if ((!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && !sprite.pause && !IsZombieRewindActive() && CanStartAttackFromCurrentPosition())
		{
			AttackComponent attackComponent = this.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased && this.attackComponent.CanAttackFromZombieWalk())
			{
				Attack();
			}
		}
	}

	public virtual void WalkExited()
	{
		_walkMovementActivationVersion++;
		GroundMoveComponent groundMoveComponent = this.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			this.groundMoveComponent.SetAlive(false);
		}
	}

	public virtual void AttackEntered()
	{
		if (inWater && attackWaterAnimeClip != "")
		{
			sprite.SetAnimation(attackWaterAnimeClip, loop: true, 0.2);
		}
		else
		{
			sprite.SetAnimation(attackAnimeClip, loop: true, 0.2);
		}
		ActivateAttackAfterStateDelay(AttackStateHandle);
	}

	protected void ActivateAttackAfterStateDelay(StateHandle stateHandle)
	{
		startAttack = false;
		ActivateAttackAfterStateDelayAsync(stateHandle);
	}

	private async Task ActivateAttackAfterStateDelayAsync(StateHandle stateHandle)
	{
		if (await WaitForStateDelayAsync(stateHandle, 0.1))
		{
			startAttack = true;
		}
	}

	public virtual void AttackProcessing(double delta)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		if (IsZombieRewindActive())
		{
			Walk();
			return;
		}
		if (!attackComponent.CanAttack())
		{
			Walk();
		}
		else if (startAttack && !nearDie && !sprite.pause && sprite.timeScale > 0.0 && useAttackDps)
		{
			attackComponent.AttackDpsExecute(delta, ((TowerDefenseZombieConfig)config).attack);
		}
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void AttackExited()
	{
		startAttack = false;
	}

	public virtual void DieEntered()
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		_walkReadyGeneration++;
		_groanPlaybackGeneration++;
		startAttack = false;
		_walkMovementActivationVersion++;
		long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
		GroundMoveComponent groundMoveComponent = this.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			this.groundMoveComponent.SetAlive(false);
		}
		TowerDefensePerfProfiler.End("zombie.death.stopMovement", startTicks2, 1);
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		ZombieDeathComponent zombieDeathComponent = this.zombieDeathComponent;
		if (zombieDeathComponent != null && !zombieDeathComponent.IsReleased)
		{
			this.zombieDeathComponent.DieEntered();
		}
		TowerDefensePerfProfiler.End("zombie.death.visual", probe.StartTicks, 1);
		TowerDefensePerfProfiler.EndSpikeProbe("zombie.death.visual", in probe, 1, 0.5);
		TowerDefensePerfProfiler.End("zombie.death.enter", startTicks, 1);
	}

	public virtual void DieProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void DieExited()
	{
	}

	public virtual void WalkReady()
	{
		WalkReadyAsync(++_walkReadyGeneration);
	}

	private async Task WalkReadyAsync(ulong readyGeneration)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (readyGeneration == _walkReadyGeneration && GodotObject.IsInstanceValid(this) && IsInsideTree() && inGame && !die && !nearDie && !isDestroy)
		{
			ActivateGameplayProcessing();
			Walk();
		}
	}

	protected void ActivateGroundMovementAfterStateDelay(StateHandle stateHandle)
	{
		ActivateGroundMovementAfterStateDelayAsync(stateHandle);
	}

	private async Task ActivateGroundMovementAfterStateDelayAsync(StateHandle stateHandle)
	{
		if (await WaitForStateDelayAsync(stateHandle, 0.1))
		{
			GroundMoveComponent groundMoveComponent = this.groundMoveComponent;
			if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
			{
				this.groundMoveComponent.SetAlive(true);
			}
		}
	}

	public virtual void Walk()
	{
		ActivateGameplayProcessing();
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else
		{
			SendStateEvent("ToWalk");
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool EnsureZeroHealthDeath()
	{
		TowerDefenseCharacterInstance towerDefenseCharacterInstance = instance;
		if (towerDefenseCharacterInstance == null || towerDefenseCharacterInstance.keepAlive || towerDefenseCharacterInstance.hitpoints > 0.0)
		{
			return false;
		}
		return ConvergeZeroHealthDeath(towerDefenseCharacterInstance);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ConvergeZeroHealthDeath(TowerDefenseCharacterInstance currentInstance)
	{
		if (_ensuringZeroHealthDeath)
		{
			return true;
		}
		_ensuringZeroHealthDeath = true;
		try
		{
			if (!currentInstance.die)
			{
				currentInstance.SkipInvincibleDealHurt(0.0, playSplatAudio: false, Vector2.Zero, createDamagePart: false);
			}
			if (!currentInstance.nearDie)
			{
				currentInstance.nearDie = true;
			}
			if (!nearDie)
			{
				HitpointsNearDie();
			}
			if (!currentInstance.die)
			{
				currentInstance.DieMethod();
			}
			if (!die)
			{
				HitpointsEmpty();
			}
		}
		finally
		{
			_ensuringZeroHealthDeath = false;
		}
		return true;
	}

	protected override StringName NormalizeMainStateEvent(StringName eventName)
	{
		if (eventName != DieStateEvent && (EnsureZeroHealthDeath() || die || (GodotObject.IsInstanceValid(instance) && instance.die)))
		{
			return DieStateEvent;
		}
		return eventName;
	}

	public virtual void Attack()
	{
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else
		{
			SendStateEvent("ToAttack");
		}
	}

	public virtual void Die()
	{
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
			SendStateEvent("ToDie");
			TowerDefensePerfProfiler.End("zombie.death.stateTransition", probe.StartTicks, 1);
			TowerDefensePerfProfiler.EndSpikeProbe("zombie.death.stateTransition", in probe, 1, 0.5);
		}
	}

	public override async void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		SwimComponent swimComponent = this.swimComponent;
		if (swimComponent != null && !swimComponent.IsReleased)
		{
			this.swimComponent.AnimeCompleted(clip);
		}
		ZombieDeathComponent zombieDeathComponent = this.zombieDeathComponent;
		bool flag = zombieDeathComponent != null && !zombieDeathComponent.IsReleased;
		if (flag)
		{
			flag = await this.zombieDeathComponent.AnimeCompleted(clip);
		}
		if (!flag && die)
		{
			ZombieDeathComponent zombieDeathComponent2 = this.zombieDeathComponent;
			if (zombieDeathComponent2 == null || zombieDeathComponent2.IsReleased || !GodotObject.IsInstanceValid(sprite) || !this.zombieDeathComponent.IsDeathAnimationClip(sprite.clip) || this.zombieDeathComponent.IsDeathAnimationClip(clip))
			{
				HitBoxDestroy();
				Die();
			}
		}
	}

	public override void Garlic()
	{
		GarlicComponent garlicComponent = this.garlicComponent;
		if (garlicComponent != null && !garlicComponent.IsReleased)
		{
			this.garlicComponent.Garlic();
		}
	}

	public void ChangeLine()
	{
		GarlicComponent garlicComponent = this.garlicComponent;
		if (garlicComponent != null && !garlicComponent.IsReleased)
		{
			this.garlicComponent.ChangeLine();
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
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
			return;
		}
		base.Hypnoses(time, canFliter, hypnosesConfig);
		if ((instance.unUseBuffFlags & 8) == 0)
		{
			AttackComponent attackComponent = this.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased)
			{
				this.attackComponent.target = null;
			}
			if (time == -1.0)
			{
				EmitBodyHurt((int)GetCurrentHitPoint());
			}
		}
	}

	public virtual void InWaterDiscardSet()
	{
		WaterInteractionComponent waterInteractionComponent = this.waterInteractionComponent;
		if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
		{
			this.waterInteractionComponent.InWaterDiscardSet();
		}
	}

	public virtual void OutWaterDiscardSet()
	{
		WaterInteractionComponent waterInteractionComponent = this.waterInteractionComponent;
		if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
		{
			this.waterInteractionComponent.OutWaterDiscardSet();
		}
	}

	public override void InWater()
	{
		base.InWater();
		SwimComponent swimComponent = this.swimComponent;
		if (swimComponent != null && !swimComponent.IsReleased)
		{
			this.swimComponent.InWater();
		}
		WaterInteractionComponent waterInteractionComponent = this.waterInteractionComponent;
		if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
		{
			this.waterInteractionComponent.InWater();
		}
	}

	public override void OutWater()
	{
		base.OutWater();
		WaterInteractionComponent waterInteractionComponent = this.waterInteractionComponent;
		if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
		{
			this.waterInteractionComponent.OutWater();
		}
		SwimComponent swimComponent = this.swimComponent;
		if (swimComponent != null && !swimComponent.IsReleased)
		{
			this.swimComponent.OutWater();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(55)
		{
			new MethodInfo(MethodName.OnRiseStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRiseEnd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectZombieStateHandles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectZombieStateHandles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AutoRegisterSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureShowZombieHealthHandler, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SubscribeShowZombieHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeShowZombieHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnShowZombieHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnShowBossHealthBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureWaterLineVisualLayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "layerNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshWaterLineVisualLayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWaterLineState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWaterLineVisualLayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "waterLine", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdateValidated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdateCore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureBatchProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsZombieRewindActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CanStartAttackFromCurrentPosition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GarlicEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GarlicProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GarlicExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateWalkMovement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "activationVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkWithoutTransitionDelay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureZeroHealthDeath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConvergeZeroHealthDeath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "currentInstance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeMainStateEvent, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Attack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Die, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Garlic, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChangeLine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InWaterDiscardSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWaterDiscardSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.ConnectZombieStateHandles && args.Count == 0)
		{
			ConnectZombieStateHandles();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectZombieStateHandles && args.Count == 0)
		{
			DisconnectZombieStateHandles();
			ret = default;
			return true;
		}
		if (method == MethodName.AutoRegisterSync && args.Count == 0)
		{
			AutoRegisterSync();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureShowZombieHealthHandler && args.Count == 0)
		{
			EnsureShowZombieHealthHandler();
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeShowZombieHealth && args.Count == 0)
		{
			SubscribeShowZombieHealth();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeShowZombieHealth && args.Count == 0)
		{
			UnsubscribeShowZombieHealth();
			ret = default;
			return true;
		}
		if (method == MethodName.OnShowZombieHealth && args.Count == 1)
		{
			OnShowZombieHealth(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnShowBossHealthBar && args.Count == 1)
		{
			OnShowBossHealthBar(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureWaterLineVisualLayers && args.Count == 1)
		{
			ConfigureWaterLineVisualLayers(VariantUtils.ConvertTo<string[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWaterLineVisualLayers && args.Count == 0)
		{
			RefreshWaterLineVisualLayers();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWaterLineState && args.Count == 0)
		{
			RefreshWaterLineState();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWaterLineVisualLayers && args.Count == 1)
		{
			RefreshWaterLineVisualLayers(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.BatchUpdateValidated && args.Count == 2)
		{
			BatchUpdateValidated(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdateCore && args.Count == 2)
		{
			BatchUpdateCore(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureBatchProcessing && args.Count == 0)
		{
			ConfigureBatchProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsZombieRewindActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZombieRewindActive());
			return true;
		}
		if (method == MethodName.CanStartAttackFromCurrentPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanStartAttackFromCurrentPosition());
			return true;
		}
		if (method == MethodName.GarlicEntered && args.Count == 0)
		{
			GarlicEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GarlicProcessing && args.Count == 1)
		{
			GarlicProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GarlicExited && args.Count == 0)
		{
			GarlicExited();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateWalkMovement && args.Count == 1)
		{
			ActivateWalkMovement(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkWithoutTransitionDelay && args.Count == 0)
		{
			WalkWithoutTransitionDelay();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkExited && args.Count == 0)
		{
			WalkExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackExited && args.Count == 0)
		{
			AttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieExited && args.Count == 0)
		{
			DieExited();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkReady && args.Count == 0)
		{
			WalkReady();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureZeroHealthDeath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureZeroHealthDeath());
			return true;
		}
		if (method == MethodName.ConvergeZeroHealthDeath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ConvergeZeroHealthDeath(VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeMainStateEvent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(NormalizeMainStateEvent(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.Attack && args.Count == 0)
		{
			Attack();
			ret = default;
			return true;
		}
		if (method == MethodName.Die && args.Count == 0)
		{
			Die();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Garlic && args.Count == 0)
		{
			Garlic();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeLine && args.Count == 0)
		{
			ChangeLine();
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.InWaterDiscardSet && args.Count == 0)
		{
			InWaterDiscardSet();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWaterDiscardSet && args.Count == 0)
		{
			OutWaterDiscardSet();
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsZombieRewindActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZombieRewindActive());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.OnRiseStart)
		{
			return true;
		}
		if (method == MethodName.OnRiseEnd)
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
		if (method == MethodName.ConnectZombieStateHandles)
		{
			return true;
		}
		if (method == MethodName.DisconnectZombieStateHandles)
		{
			return true;
		}
		if (method == MethodName.AutoRegisterSync)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.EnsureShowZombieHealthHandler)
		{
			return true;
		}
		if (method == MethodName.SubscribeShowZombieHealth)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeShowZombieHealth)
		{
			return true;
		}
		if (method == MethodName.OnShowZombieHealth)
		{
			return true;
		}
		if (method == MethodName.OnShowBossHealthBar)
		{
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.ConfigureWaterLineVisualLayers)
		{
			return true;
		}
		if (method == MethodName.RefreshWaterLineVisualLayers)
		{
			return true;
		}
		if (method == MethodName.RefreshWaterLineState)
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
		if (method == MethodName.BatchUpdateValidated)
		{
			return true;
		}
		if (method == MethodName.BatchUpdateCore)
		{
			return true;
		}
		if (method == MethodName.ConfigureBatchProcessing)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.IsZombieRewindActive)
		{
			return true;
		}
		if (method == MethodName.CanStartAttackFromCurrentPosition)
		{
			return true;
		}
		if (method == MethodName.GarlicEntered)
		{
			return true;
		}
		if (method == MethodName.GarlicProcessing)
		{
			return true;
		}
		if (method == MethodName.GarlicExited)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.ActivateWalkMovement)
		{
			return true;
		}
		if (method == MethodName.WalkWithoutTransitionDelay)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.WalkExited)
		{
			return true;
		}
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackExited)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.DieExited)
		{
			return true;
		}
		if (method == MethodName.WalkReady)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.EnsureZeroHealthDeath)
		{
			return true;
		}
		if (method == MethodName.ConvergeZeroHealthDeath)
		{
			return true;
		}
		if (method == MethodName.NormalizeMainStateEvent)
		{
			return true;
		}
		if (method == MethodName.Attack)
		{
			return true;
		}
		if (method == MethodName.Die)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Garlic)
		{
			return true;
		}
		if (method == MethodName.ChangeLine)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.InWaterDiscardSet)
		{
			return true;
		}
		if (method == MethodName.OutWaterDiscardSet)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.groan)
		{
			groan = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.walkSpeedScale)
		{
			walkSpeedScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.inSwimAnimeClipScale)
		{
			inSwimAnimeClipScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.useAttackDps)
		{
			useAttackDps = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.walkAnimeClip)
		{
			walkAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.inSwimAnimeClip)
		{
			inSwimAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.swimAnimeClip)
		{
			swimAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.outSwimAnimeClip)
		{
			outSwimAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackAnimeClip)
		{
			attackAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackWaterAnimeClip)
		{
			attackWaterAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dieAnimeClip)
		{
			dieAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dieWaterAnimeClip)
		{
			dieWaterAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.duckytobeSprite)
		{
			duckytobeSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.waterLineSprite)
		{
			waterLineSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.waterAnimeFliter)
		{
			waterAnimeFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.garlicFliters)
		{
			garlicFliters = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.garlicReplace)
		{
			garlicReplace = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.waterHeight)
		{
			waterHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isPause)
		{
			isPause = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._garlicFliters)
		{
			_garlicFliters = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._zombieStateHandlesConnected)
		{
			_zombieStateHandlesConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._showZombieHealthEventBus)
		{
			_showZombieHealthEventBus = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		if (name == PropertyName._showZombieHealthSubscribed)
		{
			_showZombieHealthSubscribed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._walkMovementActivationVersion)
		{
			_walkMovementActivationVersion = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._walkWithoutTransitionDelay)
		{
			_walkWithoutTransitionDelay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._walkReadyGeneration)
		{
			_walkReadyGeneration = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._groanPlaybackGeneration)
		{
			_groanPlaybackGeneration = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._isPause)
		{
			_isPause = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isGarlic)
		{
			isGarlic = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isGarlicBird)
		{
			isGarlicBird = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isChangeLine)
		{
			isChangeLine = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.inSwimPlay)
		{
			inSwimPlay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.inGround)
		{
			inGround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.startAttack)
		{
			startAttack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sizeUpNum)
		{
			sizeUpNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hasGhost)
		{
			hasGhost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ghostCharacter)
		{
			ghostCharacter = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		if (name == PropertyName.hasSpikeball)
		{
			hasSpikeball = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.riderCarryOwner)
		{
			riderCarryOwner = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		if (name == PropertyName.spritePause)
		{
			spritePause = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gridSize)
		{
			_gridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._validatedBatchDispatchActive)
		{
			_validatedBatchDispatchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._validatedBatchPhysicsFrame)
		{
			_validatedBatchPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._ensuringZeroHealthDeath)
		{
			_ensuringZeroHealthDeath = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.groan)
		{
			from = groan;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		double from2;
		if (name == PropertyName.walkSpeedScale)
		{
			from2 = walkSpeedScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.inSwimAnimeClipScale)
		{
			from2 = inSwimAnimeClipScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.useAttackDps)
		{
			from3 = useAttackDps;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.walkAnimeClip)
		{
			from = walkAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.inSwimAnimeClip)
		{
			from = inSwimAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.swimAnimeClip)
		{
			from = swimAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.outSwimAnimeClip)
		{
			from = outSwimAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.attackAnimeClip)
		{
			from = attackAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.attackWaterAnimeClip)
		{
			from = attackWaterAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dieAnimeClip)
		{
			from = dieAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dieWaterAnimeClip)
		{
			from = dieWaterAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		AdobeAnimateSprite from4;
		if (name == PropertyName.duckytobeSprite)
		{
			from4 = duckytobeSprite;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.waterLineSprite)
		{
			from4 = waterLineSprite;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.waterAnimeFliter)
		{
			from = waterAnimeFliter;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.garlicFliters)
		{
			value = VariantUtils.CreateFromArray(garlicFliters);
			return true;
		}
		if (name == PropertyName.garlicReplace)
		{
			from = garlicReplace;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.waterHeight)
		{
			from2 = waterHeight;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsBatchDispatchActive)
		{
			from3 = IsBatchDispatchActive;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.isPause)
		{
			from3 = isPause;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._garlicFliters)
		{
			value = VariantUtils.CreateFromArray(_garlicFliters);
			return true;
		}
		if (name == PropertyName._zombieStateHandlesConnected)
		{
			value = VariantUtils.CreateFrom(in _zombieStateHandlesConnected);
			return true;
		}
		if (name == PropertyName._showZombieHealthEventBus)
		{
			value = VariantUtils.CreateFrom(in _showZombieHealthEventBus);
			return true;
		}
		if (name == PropertyName._showZombieHealthSubscribed)
		{
			value = VariantUtils.CreateFrom(in _showZombieHealthSubscribed);
			return true;
		}
		if (name == PropertyName._walkMovementActivationVersion)
		{
			value = VariantUtils.CreateFrom(in _walkMovementActivationVersion);
			return true;
		}
		if (name == PropertyName._walkWithoutTransitionDelay)
		{
			value = VariantUtils.CreateFrom(in _walkWithoutTransitionDelay);
			return true;
		}
		if (name == PropertyName._walkReadyGeneration)
		{
			value = VariantUtils.CreateFrom(in _walkReadyGeneration);
			return true;
		}
		if (name == PropertyName._groanPlaybackGeneration)
		{
			value = VariantUtils.CreateFrom(in _groanPlaybackGeneration);
			return true;
		}
		if (name == PropertyName._isPause)
		{
			value = VariantUtils.CreateFrom(in _isPause);
			return true;
		}
		if (name == PropertyName.isGarlic)
		{
			value = VariantUtils.CreateFrom(in isGarlic);
			return true;
		}
		if (name == PropertyName.isGarlicBird)
		{
			value = VariantUtils.CreateFrom(in isGarlicBird);
			return true;
		}
		if (name == PropertyName.isChangeLine)
		{
			value = VariantUtils.CreateFrom(in isChangeLine);
			return true;
		}
		if (name == PropertyName.inSwimPlay)
		{
			value = VariantUtils.CreateFrom(in inSwimPlay);
			return true;
		}
		if (name == PropertyName.inGround)
		{
			value = VariantUtils.CreateFrom(in inGround);
			return true;
		}
		if (name == PropertyName.startAttack)
		{
			value = VariantUtils.CreateFrom(in startAttack);
			return true;
		}
		if (name == PropertyName.sizeUpNum)
		{
			value = VariantUtils.CreateFrom(in sizeUpNum);
			return true;
		}
		if (name == PropertyName.hasGhost)
		{
			value = VariantUtils.CreateFrom(in hasGhost);
			return true;
		}
		if (name == PropertyName.ghostCharacter)
		{
			value = VariantUtils.CreateFrom(in ghostCharacter);
			return true;
		}
		if (name == PropertyName.hasSpikeball)
		{
			value = VariantUtils.CreateFrom(in hasSpikeball);
			return true;
		}
		if (name == PropertyName.riderCarryOwner)
		{
			value = VariantUtils.CreateFrom(in riderCarryOwner);
			return true;
		}
		if (name == PropertyName.spritePause)
		{
			value = VariantUtils.CreateFrom(in spritePause);
			return true;
		}
		if (name == PropertyName._gridSize)
		{
			value = VariantUtils.CreateFrom(in _gridSize);
			return true;
		}
		if (name == PropertyName._validatedBatchDispatchActive)
		{
			value = VariantUtils.CreateFrom(in _validatedBatchDispatchActive);
			return true;
		}
		if (name == PropertyName._validatedBatchPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _validatedBatchPhysicsFrame);
			return true;
		}
		if (name == PropertyName._ensuringZeroHealthDeath)
		{
			value = VariantUtils.CreateFrom(in _ensuringZeroHealthDeath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.groan, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.walkSpeedScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.inSwimAnimeClipScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useAttackDps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.walkAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.inSwimAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.swimAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.outSwimAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackWaterAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dieAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dieWaterAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.duckytobeSprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.waterLineSprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.waterAnimeFliter, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._garlicFliters, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.garlicFliters, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.garlicReplace, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waterHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zombieStateHandlesConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._showZombieHealthEventBus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showZombieHealthSubscribed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._walkMovementActivationVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._walkWithoutTransitionDelay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._walkReadyGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._groanPlaybackGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsBatchDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isPause, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPause, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGarlic, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGarlicBird, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isChangeLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.inSwimPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.inGround, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.startAttack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sizeUpNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasGhost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ghostCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasSpikeball, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.riderCarryOwner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spritePause, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._gridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._validatedBatchDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._validatedBatchPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ensuringZeroHealthDeath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.groan, Variant.From<string>(groan));
		info.AddProperty(PropertyName.walkSpeedScale, Variant.From<double>(walkSpeedScale));
		info.AddProperty(PropertyName.inSwimAnimeClipScale, Variant.From<double>(inSwimAnimeClipScale));
		info.AddProperty(PropertyName.useAttackDps, Variant.From<bool>(useAttackDps));
		info.AddProperty(PropertyName.walkAnimeClip, Variant.From<string>(walkAnimeClip));
		info.AddProperty(PropertyName.inSwimAnimeClip, Variant.From<string>(inSwimAnimeClip));
		info.AddProperty(PropertyName.swimAnimeClip, Variant.From<string>(swimAnimeClip));
		info.AddProperty(PropertyName.outSwimAnimeClip, Variant.From<string>(outSwimAnimeClip));
		info.AddProperty(PropertyName.attackAnimeClip, Variant.From<string>(attackAnimeClip));
		info.AddProperty(PropertyName.attackWaterAnimeClip, Variant.From<string>(attackWaterAnimeClip));
		info.AddProperty(PropertyName.dieAnimeClip, Variant.From<string>(dieAnimeClip));
		info.AddProperty(PropertyName.dieWaterAnimeClip, Variant.From<string>(dieWaterAnimeClip));
		info.AddProperty(PropertyName.duckytobeSprite, Variant.From<AdobeAnimateSprite>(duckytobeSprite));
		info.AddProperty(PropertyName.waterLineSprite, Variant.From<AdobeAnimateSprite>(waterLineSprite));
		info.AddProperty(PropertyName.waterAnimeFliter, Variant.From<string>(waterAnimeFliter));
		info.AddProperty(PropertyName.garlicFliters, Variant.CreateFrom(garlicFliters));
		info.AddProperty(PropertyName.garlicReplace, Variant.From<string>(garlicReplace));
		info.AddProperty(PropertyName.waterHeight, Variant.From<double>(waterHeight));
		info.AddProperty(PropertyName.isPause, Variant.From<bool>(isPause));
		info.AddProperty(PropertyName._garlicFliters, Variant.CreateFrom(_garlicFliters));
		info.AddProperty(PropertyName._zombieStateHandlesConnected, Variant.From(in _zombieStateHandlesConnected));
		info.AddProperty(PropertyName._showZombieHealthEventBus, Variant.From(in _showZombieHealthEventBus));
		info.AddProperty(PropertyName._showZombieHealthSubscribed, Variant.From(in _showZombieHealthSubscribed));
		info.AddProperty(PropertyName._walkMovementActivationVersion, Variant.From(in _walkMovementActivationVersion));
		info.AddProperty(PropertyName._walkWithoutTransitionDelay, Variant.From(in _walkWithoutTransitionDelay));
		info.AddProperty(PropertyName._walkReadyGeneration, Variant.From(in _walkReadyGeneration));
		info.AddProperty(PropertyName._groanPlaybackGeneration, Variant.From(in _groanPlaybackGeneration));
		info.AddProperty(PropertyName._isPause, Variant.From(in _isPause));
		info.AddProperty(PropertyName.isGarlic, Variant.From(in isGarlic));
		info.AddProperty(PropertyName.isGarlicBird, Variant.From(in isGarlicBird));
		info.AddProperty(PropertyName.isChangeLine, Variant.From(in isChangeLine));
		info.AddProperty(PropertyName.inSwimPlay, Variant.From(in inSwimPlay));
		info.AddProperty(PropertyName.inGround, Variant.From(in inGround));
		info.AddProperty(PropertyName.startAttack, Variant.From(in startAttack));
		info.AddProperty(PropertyName.sizeUpNum, Variant.From(in sizeUpNum));
		info.AddProperty(PropertyName.hasGhost, Variant.From(in hasGhost));
		info.AddProperty(PropertyName.ghostCharacter, Variant.From(in ghostCharacter));
		info.AddProperty(PropertyName.hasSpikeball, Variant.From(in hasSpikeball));
		info.AddProperty(PropertyName.riderCarryOwner, Variant.From(in riderCarryOwner));
		info.AddProperty(PropertyName.spritePause, Variant.From(in spritePause));
		info.AddProperty(PropertyName._gridSize, Variant.From(in _gridSize));
		info.AddProperty(PropertyName._validatedBatchDispatchActive, Variant.From(in _validatedBatchDispatchActive));
		info.AddProperty(PropertyName._validatedBatchPhysicsFrame, Variant.From(in _validatedBatchPhysicsFrame));
		info.AddProperty(PropertyName._ensuringZeroHealthDeath, Variant.From(in _ensuringZeroHealthDeath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.groan, out var value))
		{
			groan = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.walkSpeedScale, out var value2))
		{
			walkSpeedScale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.inSwimAnimeClipScale, out var value3))
		{
			inSwimAnimeClipScale = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.useAttackDps, out var value4))
		{
			useAttackDps = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.walkAnimeClip, out var value5))
		{
			walkAnimeClip = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.inSwimAnimeClip, out var value6))
		{
			inSwimAnimeClip = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.swimAnimeClip, out var value7))
		{
			swimAnimeClip = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.outSwimAnimeClip, out var value8))
		{
			outSwimAnimeClip = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackAnimeClip, out var value9))
		{
			attackAnimeClip = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackWaterAnimeClip, out var value10))
		{
			attackWaterAnimeClip = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dieAnimeClip, out var value11))
		{
			dieAnimeClip = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dieWaterAnimeClip, out var value12))
		{
			dieWaterAnimeClip = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.duckytobeSprite, out var value13))
		{
			duckytobeSprite = value13.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.waterLineSprite, out var value14))
		{
			waterLineSprite = value14.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.waterAnimeFliter, out var value15))
		{
			waterAnimeFliter = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.garlicFliters, out var value16))
		{
			garlicFliters = value16.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.garlicReplace, out var value17))
		{
			garlicReplace = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.waterHeight, out var value18))
		{
			waterHeight = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isPause, out var value19))
		{
			isPause = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._garlicFliters, out var value20))
		{
			_garlicFliters = value20.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._zombieStateHandlesConnected, out var value21))
		{
			_zombieStateHandlesConnected = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showZombieHealthEventBus, out var value22))
		{
			_showZombieHealthEventBus = value22.As<BattleEventBus>();
		}
		if (info.TryGetProperty(PropertyName._showZombieHealthSubscribed, out var value23))
		{
			_showZombieHealthSubscribed = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._walkMovementActivationVersion, out var value24))
		{
			_walkMovementActivationVersion = value24.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._walkWithoutTransitionDelay, out var value25))
		{
			_walkWithoutTransitionDelay = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._walkReadyGeneration, out var value26))
		{
			_walkReadyGeneration = value26.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._groanPlaybackGeneration, out var value27))
		{
			_groanPlaybackGeneration = value27.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._isPause, out var value28))
		{
			_isPause = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isGarlic, out var value29))
		{
			isGarlic = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isGarlicBird, out var value30))
		{
			isGarlicBird = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isChangeLine, out var value31))
		{
			isChangeLine = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.inSwimPlay, out var value32))
		{
			inSwimPlay = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.inGround, out var value33))
		{
			inGround = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.startAttack, out var value34))
		{
			startAttack = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sizeUpNum, out var value35))
		{
			sizeUpNum = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hasGhost, out var value36))
		{
			hasGhost = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ghostCharacter, out var value37))
		{
			ghostCharacter = value37.As<TowerDefenseZombie>();
		}
		if (info.TryGetProperty(PropertyName.hasSpikeball, out var value38))
		{
			hasSpikeball = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.riderCarryOwner, out var value39))
		{
			riderCarryOwner = value39.As<TowerDefenseZombie>();
		}
		if (info.TryGetProperty(PropertyName.spritePause, out var value40))
		{
			spritePause = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gridSize, out var value41))
		{
			_gridSize = value41.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._validatedBatchDispatchActive, out var value42))
		{
			_validatedBatchDispatchActive = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._validatedBatchPhysicsFrame, out var value43))
		{
			_validatedBatchPhysicsFrame = value43.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._ensuringZeroHealthDeath, out var value44))
		{
			_ensuringZeroHealthDeath = value44.As<bool>();
		}
	}
}
