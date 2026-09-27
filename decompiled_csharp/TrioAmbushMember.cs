using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Registry/Battle/Feature/Wave/TrioAmbush/TrioAmbushMember.cs")]
public class TrioAmbushMember : Node
{
	private enum EntryPhase
	{
		Emerging,
		Descending,
		Leaving,
		Repelled,
		Complete
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Attach = "Attach";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Initialize = "Initialize";

		public static readonly StringName StopMovement = "StopMovement";

		public static readonly StringName CreateCarrier = "CreateCarrier";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName Present = "Present";

		public static readonly StringName Land = "Land";

		public static readonly StringName Repel = "Repel";

		public static readonly StringName FreezeCarriedPayload = "FreezeCarriedPayload";

		public static readonly StringName CompleteOperation = "CompleteOperation";

		public static readonly StringName ReleaseCarrier = "ReleaseCarrier";

		public static readonly StringName CancelEntry = "CancelEntry";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ExportSpawnState = "ExportSpawnState";

		public static readonly StringName PublishPhase = "PublishPhase";

		public static readonly StringName ApplyNetworkState = "ApplyNetworkState";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName CanRepel = "CanRepel";

		public static readonly StringName EntryPending = "EntryPending";

		public static readonly StringName IsCoral = "IsCoral";

		public static readonly StringName Height = "Height";

		public static readonly StringName _zombie = "_zombie";

		public static readonly StringName _carrier = "_carrier";

		public static readonly StringName _wave = "_wave";

		public static readonly StringName _bus = "_bus";

		public static readonly StringName _operation = "_operation";

		public static readonly StringName _coral = "_coral";

		public static readonly StringName _initialized = "_initialized";

		public static readonly StringName _savedGravity = "_savedGravity";

		public static readonly StringName _savedInvincible = "_savedInvincible";

		public static readonly StringName _savedGroundHeightAlive = "_savedGroundHeightAlive";

		public static readonly StringName _waterLayers = "_waterLayers";

		public static readonly StringName _savedMask = "_savedMask";

		public static readonly StringName _savedCollision = "_savedCollision";

		public static readonly StringName _ground = "_ground";

		public static readonly StringName _height = "_height";

		public static readonly StringName _elapsed = "_elapsed";

		public static readonly StringName _unitScale = "_unitScale";

		public static readonly StringName _phase = "_phase";

		public static readonly StringName SyncId = "SyncId";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public const string StateKey = "trio_ambush";

	public const string CoralSaveKey = "trio_coral";

	private const string CarrierScene = "res://Registry/Battle/Feature/Wave/TrioAmbush/TowerDefenseTrioBungi.tscn";

	private TowerDefenseZombie _zombie;

	private TowerDefenseTrioBungi _carrier;

	private TowerDefenseBattleFeatureWave _wave;

	private BattleEventBus _bus;

	private int _operation = -1;

	private bool _coral;

	private bool _initialized;

	private bool _savedGravity;

	private bool _savedInvincible;

	private bool _savedGroundHeightAlive;

	private readonly string[] _waterLayers = new string[3] { "Zombie_duckytube", "Zombie_whitewater", "Zombie_whitewater2" };

	private readonly bool[] _savedWaterFilters = new bool[3];

	private int _savedMask;

	private int _savedCollision;

	private double _ground;

	private double _height;

	private double _elapsed;

	private float _unitScale = 1f;

	private EntryPhase _phase;

	public int SyncId = -1;

	public bool CanRepel
	{
		get
		{
			if (_phase == EntryPhase.Descending)
			{
				return _height <= 40.0;
			}
			return false;
		}
	}

	public bool EntryPending => _phase != EntryPhase.Complete;

	public bool IsCoral => _coral;

	public double Height => _height;

	public static TrioAmbushMember Attach(TowerDefenseZombie zombie, bool coral, double height, TowerDefenseBattleFeatureWave wave = null, int operation = -1, bool restored = false)
	{
		TrioAmbushMember nodeOrNull = zombie.GetNodeOrNull<TrioAmbushMember>("TrioAmbushMember");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			return nodeOrNull;
		}
		TrioAmbushMember trioAmbushMember = new TrioAmbushMember
		{
			Name = "TrioAmbushMember",
			_zombie = zombie,
			_coral = coral,
			_height = height,
			_wave = wave,
			_operation = operation,
			_phase = (restored ? EntryPhase.Complete : ((!coral) ? EntryPhase.Descending : EntryPhase.Emerging))
		};
		if (coral)
		{
			zombie.SetMeta("trio_coral", true);
		}
		zombie.AddChild(trioAmbushMember, forceReadableName: false, InternalMode.Disabled);
		return trioAmbushMember;
	}

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Pausable;
		ProcessPhysicsPriority = 10001;
		_bus = BattleEventBus.Instance;
		if (!_coral && GodotObject.IsInstanceValid(_bus))
		{
			_bus.OnColdEffectEmit += FreezeCarriedPayload;
		}
		if (_zombie.IsNodeReady())
		{
			Initialize();
		}
	}

	private void Initialize()
	{
		if (_initialized)
		{
			return;
		}
		_initialized = true;
		_savedGravity = _zombie.gravityUse;
		_savedInvincible = _zombie.instance.invincible;
		_savedGroundHeightAlive = _zombie.groundHeightComponent?.Alive ?? false;
		_savedMask = _zombie.instance.maskFlags;
		_savedCollision = _zombie.instance.collisionFlags;
		_ground = (_zombie.inWater ? (0.0 - _zombie.waterHeight) : (_zombie.cell?.GetGroundHeight() ?? 0.0));
		_unitScale = Math.Max(0.01f, TowerDefenseManager.Instance.gridSize.X / 80f);
		if (_coral)
		{
			TrioSeaweed.Attach(_zombie);
			if (_phase == EntryPhase.Complete)
			{
				SetPhysicsProcess(enable: false);
				return;
			}
			_zombie.isRise = true;
			_zombie.groundHeightComponent?.SetAlive(alive: false);
			_zombie.OnRiseStart();
			_zombie.CreateSplash();
		}
		else
		{
			CreateCarrier();
			_zombie.Idle();
			_zombie.instance.canBeCollection = false;
			_zombie.instance.maskFlags = 1;
			_zombie.instance.invincible = true;
			_zombie.instance.collisionFlags = 0;
			for (int i = 0; i < _waterLayers.Length; i++)
			{
				_savedWaterFilters[i] = _zombie.sprite.GetFliter(_waterLayers[i]);
				_zombie.sprite.SetFliter(_waterLayers[i], open: false);
			}
			if (GodotObject.IsInstanceValid(_zombie.waterLineSprite))
			{
				_zombie.waterLineSprite.Visible = false;
			}
			if (GodotObject.IsInstanceValid(_zombie.duckytobeSprite))
			{
				_zombie.duckytobeSprite.Visible = false;
			}
		}
		_zombie.forceLocalRenderDuringZMotion = true;
		_zombie.gravityUse = false;
		_zombie.isGround = false;
		_zombie.ySpeed = 0.0;
		_zombie.shadowSprite.Visible = false;
		StopMovement();
		Present();
	}

	private void StopMovement()
	{
		AttackComponent attackComponent = _zombie.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			_zombie.attackComponent.alive = false;
		}
		GroundMoveComponent groundMoveComponent = _zombie.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			_zombie.groundMoveComponent.SetAlive(false);
		}
	}

	private void CreateCarrier()
	{
		if (!GodotObject.IsInstanceValid(_carrier))
		{
			_carrier = GD.Load<PackedScene>("res://Registry/Battle/Feature/Wave/TrioAmbush/TowerDefenseTrioBungi.tscn").Instantiate<TowerDefenseTrioBungi>(PackedScene.GenEditState.Disabled);
			_carrier.Member = this;
			_carrier.gridPos = _zombie.gridPos;
			_carrier.SetLogicalGlobalPosition(_zombie.GetLogicalGlobalPosition() + new Vector2(15f * _unitScale, 0f));
			_carrier.z = _height * (double)_unitScale;
			_zombie.GetParent().AddChild(_carrier, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!GodotObject.IsInstanceValid(_zombie) || _zombie.IsQueuedForDeletion())
		{
			CompleteOperation();
		}
		else
		{
			if (!_zombie.IsNodeReady())
			{
				return;
			}
			if (!_initialized)
			{
				Initialize();
			}
			if (!TowerDefenseManager.Instance.IsGameRunning() || GetTree().Paused)
			{
				return;
			}
			if (_zombie.die || _zombie.nearDie)
			{
				CancelEntry();
				return;
			}
			if (_wave != null && (!_wave.IsLifetimeActive || (_operation >= 0 && !_wave.IsPendingSpawnOperationCurrent(_operation))))
			{
				CancelEntry();
				return;
			}
			switch (_phase)
			{
			case EntryPhase.Emerging:
				StopMovement();
				if (!_zombie.buff.buffDictionary.ContainsKey("Frozen"))
				{
					_elapsed += delta;
				}
				Present();
				if (_elapsed + 1E-09 >= 0.5)
				{
					Land();
				}
				break;
			case EntryPhase.Descending:
			{
				StopMovement();
				double height = _height;
				_height = Math.Max(0.0, _height - 800.0 * delta);
				if (height >= 1500.0 && _height < 1500.0)
				{
					AudioManager.Instance.AudioPlay("BungeeScream");
				}
				Present();
				if (CanRepel && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
				{
					foreach (Variant item in TowerDefenseManager.Instance.GetCampTarget(_carrier.camp))
					{
						((TowerDefenseCharacter)(GodotObject)item).componentManager?.GetRuntime<BlockComponent>()?.BlockCheckCharacter(_carrier);
					}
				}
				if (_phase == EntryPhase.Descending && _height <= 0.0)
				{
					Land();
				}
				break;
			}
			case EntryPhase.Leaving:
			case EntryPhase.Repelled:
				_height += 800.0 * delta;
				Present();
				if (_height >= 600.0)
				{
					bool flag = _phase == EntryPhase.Repelled;
					_phase = EntryPhase.Complete;
					CompleteOperation();
					ReleaseCarrier();
					if (flag && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
					{
						_zombie.Destroy();
					}
					SetPhysicsProcess(enable: false);
				}
				break;
			}
		}
	}

	private void Present()
	{
		if (_phase == EntryPhase.Emerging)
		{
			_zombie.isGround = true;
			_zombie.groundHeight = Mathf.Lerp(-150f * _unitScale, _ground, Math.Clamp(_elapsed / 0.5, 0.0, 1.0));
			_zombie.z = _zombie.groundHeight;
			_zombie.spriteGroup.Position = new Vector2(_zombie.spriteGroup.Position.X, 0f - (float)_zombie.z);
			Transform2D screenTransform = _zombie.GetViewport().GetScreenTransform();
			screenTransform.Origin = Vector2.Zero;
			float y = (screenTransform * (_zombie.GetLogicalGlobalPosition(_zombie.transformPoint) + new Vector2(0f, (float)_zombie.z * _zombie.spriteGroup.GlobalScale.Y))).Y;
			_zombie.SetSpriteGroupShaderParameter("discardDownPos", y);
		}
		else
		{
			if (!GodotObject.IsInstanceValid(_carrier))
			{
				return;
			}
			_carrier.z = _ground + _height * (double)_unitScale;
			_carrier.spriteGroup.Position = new Vector2(_carrier.spriteGroup.Position.X, 0f - (float)_carrier.z);
			_carrier.sprite.timeScale = 3.0;
			if (_phase == EntryPhase.Leaving)
			{
				return;
			}
			string[] waterLayers = _waterLayers;
			foreach (string text in waterLayers)
			{
				if (_zombie.sprite.GetFliter(text))
				{
					_zombie.sprite.SetFliter(text, open: false);
				}
			}
			if (GodotObject.IsInstanceValid(_zombie.waterLineSprite))
			{
				_zombie.waterLineSprite.Visible = false;
			}
			if (GodotObject.IsInstanceValid(_zombie.duckytobeSprite))
			{
				_zombie.duckytobeSprite.Visible = false;
			}
			_zombie.z = _carrier.z;
			_zombie.spriteGroup.Position = new Vector2(_zombie.spriteGroup.Position.X, 0f - (float)_zombie.z);
			_zombie.SetSpriteGroupShaderParameter("discardDownPos", 10000.0);
		}
	}

	private void Land()
	{
		_zombie.gravityUse = _savedGravity;
		_zombie.instance.invincible = _savedInvincible;
		_zombie.isGround = true;
		_zombie.isRise = false;
		_zombie.groundHeight = _ground;
		if (_coral)
		{
			_zombie.groundHeightComponent?.SetAlive(_savedGroundHeightAlive);
		}
		_zombie.z = _ground;
		_zombie.ySpeed = 0.0;
		_zombie.spriteGroup.Position = new Vector2(_zombie.spriteGroup.Position.X, 0f - (float)_ground);
		_zombie.instance.maskFlags = _savedMask;
		_zombie.instance.collisionFlags = _savedCollision;
		_zombie.instance.canBeCollection = true;
		_zombie.shadowSprite.Visible = !_zombie.inWater && !_zombie.invisible;
		if (!_coral)
		{
			for (int i = 0; i < _waterLayers.Length; i++)
			{
				_zombie.sprite.SetFliter(_waterLayers[i], _savedWaterFilters[i]);
			}
		}
		if (_zombie.inWater)
		{
			_zombie.waterInteractionComponent?.InWaterDiscardSet();
			if (GodotObject.IsInstanceValid(_zombie.waterLineSprite))
			{
				_zombie.waterLineSprite.Visible = true;
			}
			if (GodotObject.IsInstanceValid(_zombie.duckytobeSprite))
			{
				_zombie.duckytobeSprite.Visible = true;
			}
		}
		else
		{
			_zombie.SetSpriteGroupShaderParameter("discardDownPos", 10000.0);
		}
		_zombie.groundMoveComponent?.SetAlive(true);
		_zombie.OnRiseEnd();
		_phase = (_coral ? EntryPhase.Complete : EntryPhase.Leaving);
		if (_coral)
		{
			CompleteOperation();
			SetPhysicsProcess(enable: false);
		}
		else if (GodotObject.IsInstanceValid(_carrier))
		{
			_carrier.sprite.SetAnimation("Raise", loop: false, 0.2);
		}
		PublishPhase();
	}

	public void Repel()
	{
		if (CanRepel)
		{
			_phase = EntryPhase.Repelled;
			StopMovement();
			PublishPhase();
		}
	}

	public void FreezeCarriedPayload()
	{
		if (_coral || _phase != EntryPhase.Descending || !_initialized || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost))
		{
			return;
		}
		BuffComponent buff = _zombie.buff;
		if (buff == null || buff.IsReleased)
		{
			return;
		}
		bool flag = _zombie.buff.buffDictionary.ContainsKey("Frozen") || _zombie.iceSpeedDown;
		int maskFlags = _zombie.instance.maskFlags;
		try
		{
			_zombie.instance.maskFlags = 1;
			_zombie.buff.AddBuff(new TowerDefenseCharacterBuffFrozen
			{
				time = (flag ? GD.RandRange(3.0, 4.0) : GD.RandRange(4.0, 6.0)),
				iceSpeedDownTime = 20.0
			});
		}
		finally
		{
			_zombie.instance.maskFlags = maskFlags;
		}
	}

	private void CompleteOperation()
	{
		if (_operation >= 0 && GodotObject.IsInstanceValid(_wave))
		{
			_wave.CompletePendingSpawnOperation(_operation);
		}
		_operation = -1;
	}

	private void ReleaseCarrier()
	{
		if (GodotObject.IsInstanceValid(_carrier) && !_carrier.IsQueuedForDeletion())
		{
			_carrier.Destroy();
		}
		_carrier = null;
	}

	public void CancelEntry()
	{
		bool flag = _phase != EntryPhase.Complete;
		_phase = EntryPhase.Complete;
		CompleteOperation();
		ReleaseCarrier();
		if (flag && GodotObject.IsInstanceValid(_zombie))
		{
			_zombie.gravityUse = _savedGravity;
			_zombie.isRise = false;
			if (!_zombie.die && !_zombie.nearDie && !_zombie.isDestroy && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
			{
				_zombie.Destroy();
			}
		}
		_phase = EntryPhase.Complete;
		SetPhysicsProcess(enable: false);
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_bus))
		{
			_bus.OnColdEffectEmit -= FreezeCarriedPayload;
		}
		CancelEntry();
	}

	public static Dictionary ExportSpawnState(TowerDefenseCharacter character)
	{
		TrioAmbushMember nodeOrNull = character.GetNodeOrNull<TrioAmbushMember>("TrioAmbushMember");
		if (!GodotObject.IsInstanceValid(nodeOrNull) || (!nodeOrNull._coral && nodeOrNull._phase == EntryPhase.Complete))
		{
			return new Dictionary();
		}
		return new Dictionary { ["trio_ambush"] = new Dictionary
		{
			["coral"] = nodeOrNull._coral,
			["height"] = nodeOrNull._height,
			["elapsed"] = nodeOrNull._elapsed,
			["phase"] = (int)nodeOrNull._phase
		} };
	}

	private void PublishPhase()
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && SyncId >= 0)
		{
			MultiPlayerManager.Instance.SendSpawnCharacterAt(_zombie.packet.saveKey, _zombie.gridPos.X, _zombie.gridPos.Y, SyncId, 1.0, 1.0, hypnoses: false, 0.0, useCreate: false, 0.0, 0.0, walkAfterSpawn: false, 0.0, "", ExportSpawnState(_zombie));
		}
	}

	public static void ApplyNetworkState(TowerDefenseCharacter character, Dictionary state)
	{
		if (!(character is TowerDefenseZombie towerDefenseZombie) || state == null || !state.ContainsKey("trio_ambush"))
		{
			return;
		}
		Dictionary dictionary = state["trio_ambush"].AsGodotDictionary();
		bool flag = dictionary.GetValueOrDefault("coral", false).AsBool();
		int num = dictionary.GetValueOrDefault("phase", 0).AsInt32();
		if (!flag && num == 4)
		{
			return;
		}
		TrioAmbushMember trioAmbushMember = Attach(towerDefenseZombie, flag, dictionary.GetValueOrDefault("height", 0.0).AsDouble(), null, -1, flag && num == 4);
		trioAmbushMember._elapsed = Math.Max(trioAmbushMember._elapsed, dictionary.GetValueOrDefault("elapsed", 0.0).AsDouble());
		switch (num)
		{
		case 3:
			trioAmbushMember._phase = EntryPhase.Repelled;
			trioAmbushMember._height = dictionary.GetValueOrDefault("height", 0.0).AsDouble();
			if (trioAmbushMember._initialized)
			{
				trioAmbushMember.CreateCarrier();
				towerDefenseZombie.gravityUse = false;
				towerDefenseZombie.isGround = false;
				towerDefenseZombie.instance.invincible = true;
				towerDefenseZombie.instance.canBeCollection = false;
				towerDefenseZombie.instance.collisionFlags = 0;
				trioAmbushMember.StopMovement();
				trioAmbushMember.SetPhysicsProcess(enable: true);
			}
			break;
		case 2:
			if (trioAmbushMember._phase == EntryPhase.Descending && trioAmbushMember._initialized)
			{
				trioAmbushMember.Land();
			}
			break;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.Attach, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "coral", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "wave", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "restored", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopMovement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Present, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Land, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Repel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreezeCarriedPayload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.PublishPhase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyNetworkState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Attach && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TrioAmbushMember>(Attach(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.Initialize && args.Count == 0)
		{
			Initialize();
			ret = default;
			return true;
		}
		if (method == MethodName.StopMovement && args.Count == 0)
		{
			StopMovement();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCarrier && args.Count == 0)
		{
			CreateCarrier();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Present && args.Count == 0)
		{
			Present();
			ret = default;
			return true;
		}
		if (method == MethodName.Land && args.Count == 0)
		{
			Land();
			ret = default;
			return true;
		}
		if (method == MethodName.Repel && args.Count == 0)
		{
			Repel();
			ret = default;
			return true;
		}
		if (method == MethodName.FreezeCarriedPayload && args.Count == 0)
		{
			FreezeCarriedPayload();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteOperation && args.Count == 0)
		{
			CompleteOperation();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCarrier && args.Count == 0)
		{
			ReleaseCarrier();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelEntry && args.Count == 0)
		{
			CancelEntry();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportSpawnState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportSpawnState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.PublishPhase && args.Count == 0)
		{
			PublishPhase();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNetworkState && args.Count == 2)
		{
			ApplyNetworkState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Attach && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TrioAmbushMember>(Attach(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.ExportSpawnState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportSpawnState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyNetworkState && args.Count == 2)
		{
			ApplyNetworkState(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Attach)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName.StopMovement)
		{
			return true;
		}
		if (method == MethodName.CreateCarrier)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.Present)
		{
			return true;
		}
		if (method == MethodName.Land)
		{
			return true;
		}
		if (method == MethodName.Repel)
		{
			return true;
		}
		if (method == MethodName.FreezeCarriedPayload)
		{
			return true;
		}
		if (method == MethodName.CompleteOperation)
		{
			return true;
		}
		if (method == MethodName.ReleaseCarrier)
		{
			return true;
		}
		if (method == MethodName.CancelEntry)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ExportSpawnState)
		{
			return true;
		}
		if (method == MethodName.PublishPhase)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkState)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._zombie)
		{
			_zombie = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		if (name == PropertyName._carrier)
		{
			_carrier = VariantUtils.ConvertTo<TowerDefenseTrioBungi>(in value);
			return true;
		}
		if (name == PropertyName._wave)
		{
			_wave = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		if (name == PropertyName._bus)
		{
			_bus = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		if (name == PropertyName._operation)
		{
			_operation = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._coral)
		{
			_coral = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			_initialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._savedGravity)
		{
			_savedGravity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._savedInvincible)
		{
			_savedInvincible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._savedGroundHeightAlive)
		{
			_savedGroundHeightAlive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._savedMask)
		{
			_savedMask = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._savedCollision)
		{
			_savedCollision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._ground)
		{
			_ground = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._height)
		{
			_height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			_elapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._unitScale)
		{
			_unitScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._phase)
		{
			_phase = VariantUtils.ConvertTo<EntryPhase>(in value);
			return true;
		}
		if (name == PropertyName.SyncId)
		{
			SyncId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.CanRepel)
		{
			from = CanRepel;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.EntryPending)
		{
			from = EntryPending;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsCoral)
		{
			from = IsCoral;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Height)
		{
			value = VariantUtils.CreateFrom<double>(Height);
			return true;
		}
		if (name == PropertyName._zombie)
		{
			value = VariantUtils.CreateFrom(in _zombie);
			return true;
		}
		if (name == PropertyName._carrier)
		{
			value = VariantUtils.CreateFrom(in _carrier);
			return true;
		}
		if (name == PropertyName._wave)
		{
			value = VariantUtils.CreateFrom(in _wave);
			return true;
		}
		if (name == PropertyName._bus)
		{
			value = VariantUtils.CreateFrom(in _bus);
			return true;
		}
		if (name == PropertyName._operation)
		{
			value = VariantUtils.CreateFrom(in _operation);
			return true;
		}
		if (name == PropertyName._coral)
		{
			value = VariantUtils.CreateFrom(in _coral);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			value = VariantUtils.CreateFrom(in _initialized);
			return true;
		}
		if (name == PropertyName._savedGravity)
		{
			value = VariantUtils.CreateFrom(in _savedGravity);
			return true;
		}
		if (name == PropertyName._savedInvincible)
		{
			value = VariantUtils.CreateFrom(in _savedInvincible);
			return true;
		}
		if (name == PropertyName._savedGroundHeightAlive)
		{
			value = VariantUtils.CreateFrom(in _savedGroundHeightAlive);
			return true;
		}
		if (name == PropertyName._waterLayers)
		{
			value = VariantUtils.CreateFrom(in _waterLayers);
			return true;
		}
		if (name == PropertyName._savedMask)
		{
			value = VariantUtils.CreateFrom(in _savedMask);
			return true;
		}
		if (name == PropertyName._savedCollision)
		{
			value = VariantUtils.CreateFrom(in _savedCollision);
			return true;
		}
		if (name == PropertyName._ground)
		{
			value = VariantUtils.CreateFrom(in _ground);
			return true;
		}
		if (name == PropertyName._height)
		{
			value = VariantUtils.CreateFrom(in _height);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			value = VariantUtils.CreateFrom(in _elapsed);
			return true;
		}
		if (name == PropertyName._unitScale)
		{
			value = VariantUtils.CreateFrom(in _unitScale);
			return true;
		}
		if (name == PropertyName._phase)
		{
			value = VariantUtils.CreateFrom(in _phase);
			return true;
		}
		if (name == PropertyName.SyncId)
		{
			value = VariantUtils.CreateFrom(in SyncId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._carrier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._wave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._operation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._coral, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._initialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._savedGravity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._savedInvincible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._savedGroundHeightAlive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._waterLayers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._savedMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._savedCollision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._ground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._elapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._unitScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._phase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SyncId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanRepel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EntryPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCoral, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.Height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zombie, Variant.From(in _zombie));
		info.AddProperty(PropertyName._carrier, Variant.From(in _carrier));
		info.AddProperty(PropertyName._wave, Variant.From(in _wave));
		info.AddProperty(PropertyName._bus, Variant.From(in _bus));
		info.AddProperty(PropertyName._operation, Variant.From(in _operation));
		info.AddProperty(PropertyName._coral, Variant.From(in _coral));
		info.AddProperty(PropertyName._initialized, Variant.From(in _initialized));
		info.AddProperty(PropertyName._savedGravity, Variant.From(in _savedGravity));
		info.AddProperty(PropertyName._savedInvincible, Variant.From(in _savedInvincible));
		info.AddProperty(PropertyName._savedGroundHeightAlive, Variant.From(in _savedGroundHeightAlive));
		info.AddProperty(PropertyName._savedMask, Variant.From(in _savedMask));
		info.AddProperty(PropertyName._savedCollision, Variant.From(in _savedCollision));
		info.AddProperty(PropertyName._ground, Variant.From(in _ground));
		info.AddProperty(PropertyName._height, Variant.From(in _height));
		info.AddProperty(PropertyName._elapsed, Variant.From(in _elapsed));
		info.AddProperty(PropertyName._unitScale, Variant.From(in _unitScale));
		info.AddProperty(PropertyName._phase, Variant.From(in _phase));
		info.AddProperty(PropertyName.SyncId, Variant.From(in SyncId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zombie, out var value))
		{
			_zombie = value.As<TowerDefenseZombie>();
		}
		if (info.TryGetProperty(PropertyName._carrier, out var value2))
		{
			_carrier = value2.As<TowerDefenseTrioBungi>();
		}
		if (info.TryGetProperty(PropertyName._wave, out var value3))
		{
			_wave = value3.As<TowerDefenseBattleFeatureWave>();
		}
		if (info.TryGetProperty(PropertyName._bus, out var value4))
		{
			_bus = value4.As<BattleEventBus>();
		}
		if (info.TryGetProperty(PropertyName._operation, out var value5))
		{
			_operation = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._coral, out var value6))
		{
			_coral = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._initialized, out var value7))
		{
			_initialized = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._savedGravity, out var value8))
		{
			_savedGravity = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._savedInvincible, out var value9))
		{
			_savedInvincible = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._savedGroundHeightAlive, out var value10))
		{
			_savedGroundHeightAlive = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._savedMask, out var value11))
		{
			_savedMask = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._savedCollision, out var value12))
		{
			_savedCollision = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._ground, out var value13))
		{
			_ground = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName._height, out var value14))
		{
			_height = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName._elapsed, out var value15))
		{
			_elapsed = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName._unitScale, out var value16))
		{
			_unitScale = value16.As<float>();
		}
		if (info.TryGetProperty(PropertyName._phase, out var value17))
		{
			_phase = value17.As<EntryPhase>();
		}
		if (info.TryGetProperty(PropertyName.SyncId, out var value18))
		{
			SyncId = value18.As<int>();
		}
	}
}
