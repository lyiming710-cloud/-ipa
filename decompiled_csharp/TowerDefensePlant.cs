using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefensePlant.cs")]
public class TowerDefensePlant : TowerDefenseCharacter
{
	private sealed class PlantStatePhysicsFastCallbackTarget : IStateMachinePhysicsFastCallbackTarget
	{
		private readonly TowerDefensePlant _owner;

		internal PlantStatePhysicsFastCallbackTarget(TowerDefensePlant owner)
		{
			_owner = owner;
		}

		void IStateMachinePhysicsFastCallbackTarget.InvokeStateMachinePhysicsFastCallback(int callbackId, double delta)
		{
			if (callbackId == 1)
			{
				_owner.PlantProcessing(delta);
			}
		}
	}

	public new class MethodName : TowerDefenseCharacter.MethodName
	{
		public new static readonly StringName ShouldUpdateGridPos = "ShouldUpdateGridPos";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectPlantStateHandle = "ConnectPlantStateHandle";

		public static readonly StringName DisconnectPlantStateHandle = "DisconnectPlantStateHandle";

		public static readonly StringName OnShowPlantHealth = "OnShowPlantHealth";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName OnTargetZombieDestroyed = "OnTargetZombieDestroyed";

		public static readonly StringName OnTargetPlantDestroyed = "OnTargetPlantDestroyed";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName IsIzmLevelMode = "IsIzmLevelMode";

		public static readonly StringName IsIzmOneShotGateOpen = "IsIzmOneShotGateOpen";

		public static readonly StringName BindIzmOneShotTouchHook = "BindIzmOneShotTouchHook";

		public static readonly StringName MarkIzmOneShotTouched = "MarkIzmOneShotTouched";

		public new static readonly StringName IdleExited = "IdleExited";

		public static readonly StringName PlantEntered = "PlantEntered";

		public static readonly StringName PlantProcessing = "PlantProcessing";

		public static readonly StringName PlantExited = "PlantExited";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName Cover = "Cover";
	}

	public new class PropertyName : TowerDefenseCharacter.PropertyName
	{
		public static readonly StringName _plantStateHandleConnected = "_plantStateHandleConnected";

		public static readonly StringName targetZombie = "targetZombie";

		public static readonly StringName targetPlant = "targetPlant";

		public static readonly StringName zombiePlaceDamage = "zombiePlaceDamage";

		public static readonly StringName plantAnimeClip = "plantAnimeClip";

		public static readonly StringName _izmOneShotTouched = "_izmOneShotTouched";

		public static readonly StringName _izmOneShotTouchHookBound = "_izmOneShotTouchHookBound";
	}

	public new class SignalName : TowerDefenseCharacter.SignalName
	{
	}

	private const int PlantPhysicsCallbackId = 1;

	public WaterInteractionComponent waterInteractionComponent;

	public PlantAnimeComponent plantAnimeComponent;

	public PuzzleShaderComponent puzzleShaderComponent;

	private StateHandle _plantStateHandle;

	private PlantStatePhysicsFastCallbackTarget _plantStatePhysicsFastCallbackTarget;

	private bool _plantStateHandleConnected;

	public TowerDefenseCharacter targetZombie;

	public TowerDefenseCharacter targetPlant;

	public double zombiePlaceDamage;

	[Export(PropertyHint.None, "")]
	public string plantAnimeClip = "";

	private bool _izmOneShotTouched;

	private bool _izmOneShotTouchHookBound;

	public override bool ShouldUpdateGridPos()
	{
		return false;
	}

	public override void _Ready()
	{
		base._Ready();
		if (!editorPreviewMode && !Engine.IsEditorHint())
		{
			if (GodotObject.IsInstanceValid(componentManager))
			{
				plantAnimeComponent = componentManager.GetRuntime<PlantAnimeComponent>();
				waterInteractionComponent = componentManager.GetRuntime<WaterInteractionComponent>();
				this.puzzleShaderComponent = componentManager.GetRuntime<PuzzleShaderComponent>();
			}
			AddToGroup("Plant", persistent: true);
			instance.hitpointsEmpty += () =>
			{
				Destroy();
			};
			PuzzleShaderComponent puzzleShaderComponent = this.puzzleShaderComponent;
			if (puzzleShaderComponent != null && !puzzleShaderComponent.IsReleased)
			{
				this.puzzleShaderComponent.Init();
			}
			ShowHealthComponent showHealthComponent = base.showHealthComponent;
			if (showHealthComponent != null && !showHealthComponent.IsReleased)
			{
				base.showHealthComponent.SetAlive(GameSaveManager.Instance.GetConfigValue("ShowPlantHealth").AsBool());
			}
			if (inGame)
			{
				BattleEventBus.Instance.OnShowPlantHealth += OnShowPlantHealth;
			}
			if (config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
			{
				shadowComponent.shadowDisabled = true;
				shadowSprite.Visible = false;
			}
			ConnectPlantStateHandle();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnShowPlantHealth -= OnShowPlantHealth;
		}
	}

	private void ConnectPlantStateHandle()
	{
		if (_plantStateHandleConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return;
		}
		_plantStateHandle = StateMachine?.GetStateById("plant.plant");
		StateHandle plantStateHandle = _plantStateHandle;
		if (plantStateHandle != null && plantStateHandle.IsValid)
		{
			if (_plantStatePhysicsFastCallbackTarget == null)
			{
				_plantStatePhysicsFastCallbackTarget = new PlantStatePhysicsFastCallbackTarget(this);
			}
			_plantStateHandle.Entered += PlantEntered;
			_plantStateHandle.Exited += PlantExited;
			if (!_plantStateHandle.TrySetPhysicsFastCallback(_plantStatePhysicsFastCallbackTarget, 1))
			{
				_plantStateHandle.PhysicsProcessing += PlantProcessing;
			}
			_plantStateHandleConnected = true;
		}
	}

	private void DisconnectPlantStateHandle()
	{
		if (_plantStateHandleConnected && _plantStateHandle != null)
		{
			_plantStateHandle.Entered -= PlantEntered;
			_plantStateHandle.Exited -= PlantExited;
			if (!_plantStateHandle.ClearPhysicsFastCallback(_plantStatePhysicsFastCallbackTarget))
			{
				_plantStateHandle.PhysicsProcessing -= PlantProcessing;
			}
		}
		_plantStateHandle = null;
		_plantStateHandleConnected = false;
	}

	private void OnShowPlantHealth(bool show)
	{
		ShowHealthComponent showHealthComponent = base.showHealthComponent;
		if (showHealthComponent != null && !showHealthComponent.IsReleased)
		{
			base.showHealthComponent.SetAlive(show);
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!TowerDefenseCharacter.CachedEditorHint)
		{
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (GodotObject.IsInstanceValid(targetZombie))
			{
				SetGlobalPositionForPhysicsFrame(targetZombie.GetGlobalPositionForPhysicsFrame(currentPhysicsFrame), currentPhysicsFrame);
			}
			if (GodotObject.IsInstanceValid(targetPlant))
			{
				SetGlobalPositionForPhysicsFrame(targetPlant.GetGlobalPositionForPhysicsFrame(currentPhysicsFrame), currentPhysicsFrame);
			}
			if ((ulong)((long)currentPhysicsFrame + (long)randFreshIndex) % 5uL == 0L && instance != null)
			{
				instance.RefreshDamagePoint();
			}
		}
	}

	public void OnTargetZombieDestroyed()
	{
		targetZombie = null;
		Destroy();
	}

	public void OnTargetPlantDestroyed()
	{
		targetPlant = null;
		Destroy();
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
		PuzzleShaderComponent puzzleShaderComponent = this.puzzleShaderComponent;
		if (puzzleShaderComponent != null && !puzzleShaderComponent.IsReleased)
		{
			this.puzzleShaderComponent.IdleEntered();
		}
	}

	protected static bool IsIzmLevelMode()
	{
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(towerDefenseManager))
		{
			return towerDefenseManager.IsIZMMode();
		}
		return false;
	}

	protected bool IsIzmOneShotGateOpen()
	{
		if (!IsIzmLevelMode() || _izmOneShotTouched)
		{
			return true;
		}
		BindIzmOneShotTouchHook();
		return _izmOneShotTouched;
	}

	private void BindIzmOneShotTouchHook()
	{
		if (!_izmOneShotTouchHookBound)
		{
			_izmOneShotTouchHookBound = true;
			OnBodyHurt += MarkIzmOneShotTouched;
			OnArmorHurt += MarkIzmOneShotTouched;
		}
	}

	private void MarkIzmOneShotTouched(int num)
	{
		_izmOneShotTouched = true;
	}

	public override void IdleExited()
	{
		base.IdleExited();
		PuzzleShaderComponent puzzleShaderComponent = this.puzzleShaderComponent;
		if (puzzleShaderComponent != null && !puzzleShaderComponent.IsReleased)
		{
			this.puzzleShaderComponent.IdleExited();
		}
	}

	public void PlantEntered()
	{
		PlantAnimeComponent plantAnimeComponent = this.plantAnimeComponent;
		if (plantAnimeComponent != null && plantAnimeComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.plantAnimeComponent.PlantEntered();
		}
		else
		{
			Idle();
		}
	}

	public void PlantProcessing(double delta)
	{
		PlantAnimeComponent plantAnimeComponent = this.plantAnimeComponent;
		if (plantAnimeComponent != null && plantAnimeComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.plantAnimeComponent.PlantProcessing((float)delta);
		}
	}

	public void PlantExited()
	{
		PlantAnimeComponent plantAnimeComponent = this.plantAnimeComponent;
		if (plantAnimeComponent != null && plantAnimeComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.plantAnimeComponent.PlantExited();
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		PlantAnimeComponent plantAnimeComponent = this.plantAnimeComponent;
		if (plantAnimeComponent != null && plantAnimeComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
		{
			this.plantAnimeComponent.AnimeCompleted(clip);
		}
	}

	public override void InWater()
	{
		base.InWater();
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
	}

	public override void Cover(TowerDefenseCharacter character)
	{
		base.Cover(character);
		if (GodotObject.IsInstanceValid(character) && character.config is TowerDefensePlantConfig { coverEvents: { } coverEvents })
		{
			for (int i = 0; i < coverEvents.Count; i++)
			{
				coverEvents[i]?.Execute(GetLogicalGlobalPosition(), this);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName.ShouldUpdateGridPos, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectPlantStateHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectPlantStateHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnShowPlantHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTargetZombieDestroyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTargetPlantDestroyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsIzmLevelMode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsIzmOneShotGateOpen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindIzmOneShotTouchHook, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkIzmOneShotTouched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlantExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Cover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ShouldUpdateGridPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUpdateGridPos());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectPlantStateHandle && args.Count == 0)
		{
			ConnectPlantStateHandle();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectPlantStateHandle && args.Count == 0)
		{
			DisconnectPlantStateHandle();
			ret = default;
			return true;
		}
		if (method == MethodName.OnShowPlantHealth && args.Count == 1)
		{
			OnShowPlantHealth(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTargetZombieDestroyed && args.Count == 0)
		{
			OnTargetZombieDestroyed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTargetPlantDestroyed && args.Count == 0)
		{
			OnTargetPlantDestroyed();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IsIzmLevelMode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIzmLevelMode());
			return true;
		}
		if (method == MethodName.IsIzmOneShotGateOpen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIzmOneShotGateOpen());
			return true;
		}
		if (method == MethodName.BindIzmOneShotTouchHook && args.Count == 0)
		{
			BindIzmOneShotTouchHook();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkIzmOneShotTouched && args.Count == 1)
		{
			MarkIzmOneShotTouched(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleExited && args.Count == 0)
		{
			IdleExited();
			ret = default;
			return true;
		}
		if (method == MethodName.PlantEntered && args.Count == 0)
		{
			PlantEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PlantProcessing && args.Count == 1)
		{
			PlantProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlantExited && args.Count == 0)
		{
			PlantExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.Cover && args.Count == 1)
		{
			Cover(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsIzmLevelMode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIzmLevelMode());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ShouldUpdateGridPos)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ConnectPlantStateHandle)
		{
			return true;
		}
		if (method == MethodName.DisconnectPlantStateHandle)
		{
			return true;
		}
		if (method == MethodName.OnShowPlantHealth)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.OnTargetZombieDestroyed)
		{
			return true;
		}
		if (method == MethodName.OnTargetPlantDestroyed)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IsIzmLevelMode)
		{
			return true;
		}
		if (method == MethodName.IsIzmOneShotGateOpen)
		{
			return true;
		}
		if (method == MethodName.BindIzmOneShotTouchHook)
		{
			return true;
		}
		if (method == MethodName.MarkIzmOneShotTouched)
		{
			return true;
		}
		if (method == MethodName.IdleExited)
		{
			return true;
		}
		if (method == MethodName.PlantEntered)
		{
			return true;
		}
		if (method == MethodName.PlantProcessing)
		{
			return true;
		}
		if (method == MethodName.PlantExited)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
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
		if (method == MethodName.Cover)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._plantStateHandleConnected)
		{
			_plantStateHandleConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.targetZombie)
		{
			targetZombie = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.targetPlant)
		{
			targetPlant = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.zombiePlaceDamage)
		{
			zombiePlaceDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.plantAnimeClip)
		{
			plantAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._izmOneShotTouched)
		{
			_izmOneShotTouched = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._izmOneShotTouchHookBound)
		{
			_izmOneShotTouchHookBound = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._plantStateHandleConnected)
		{
			value = VariantUtils.CreateFrom(in _plantStateHandleConnected);
			return true;
		}
		if (name == PropertyName.targetZombie)
		{
			value = VariantUtils.CreateFrom(in targetZombie);
			return true;
		}
		if (name == PropertyName.targetPlant)
		{
			value = VariantUtils.CreateFrom(in targetPlant);
			return true;
		}
		if (name == PropertyName.zombiePlaceDamage)
		{
			value = VariantUtils.CreateFrom(in zombiePlaceDamage);
			return true;
		}
		if (name == PropertyName.plantAnimeClip)
		{
			value = VariantUtils.CreateFrom(in plantAnimeClip);
			return true;
		}
		if (name == PropertyName._izmOneShotTouched)
		{
			value = VariantUtils.CreateFrom(in _izmOneShotTouched);
			return true;
		}
		if (name == PropertyName._izmOneShotTouchHookBound)
		{
			value = VariantUtils.CreateFrom(in _izmOneShotTouchHookBound);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantStateHandleConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.targetZombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.targetPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.zombiePlaceDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.plantAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._izmOneShotTouched, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._izmOneShotTouchHookBound, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._plantStateHandleConnected, Variant.From(in _plantStateHandleConnected));
		info.AddProperty(PropertyName.targetZombie, Variant.From(in targetZombie));
		info.AddProperty(PropertyName.targetPlant, Variant.From(in targetPlant));
		info.AddProperty(PropertyName.zombiePlaceDamage, Variant.From(in zombiePlaceDamage));
		info.AddProperty(PropertyName.plantAnimeClip, Variant.From(in plantAnimeClip));
		info.AddProperty(PropertyName._izmOneShotTouched, Variant.From(in _izmOneShotTouched));
		info.AddProperty(PropertyName._izmOneShotTouchHookBound, Variant.From(in _izmOneShotTouchHookBound));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._plantStateHandleConnected, out var value))
		{
			_plantStateHandleConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.targetZombie, out var value2))
		{
			targetZombie = value2.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.targetPlant, out var value3))
		{
			targetPlant = value3.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.zombiePlaceDamage, out var value4))
		{
			zombiePlaceDamage = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.plantAnimeClip, out var value5))
		{
			plantAnimeClip = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._izmOneShotTouched, out var value6))
		{
			_izmOneShotTouched = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._izmOneShotTouchHookBound, out var value7))
		{
			_izmOneShotTouchHookBound = value7.As<bool>();
		}
	}
}
