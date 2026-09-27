using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/CactusQX/Scene/TowerDefensePlantCactusQX.cs")]
public class TowerDefensePlantCactusQX : TowerDefensePlant, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ApplyFireComponentSettings = "ApplyFireComponentSettings";

		public static readonly StringName FireVolley = "FireVolley";

		public static readonly StringName ResolveVolleyBulletCount = "ResolveVolleyBulletCount";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public static readonly StringName SetupBaseState = "SetupBaseState";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName RestoreNormalState = "RestoreNormalState";

		public static readonly StringName DownEntered = "DownEntered";

		public static readonly StringName DownIdleEntered = "DownIdleEntered";

		public static readonly StringName DownIdleProcessing = "DownIdleProcessing";

		public static readonly StringName UpEntered = "UpEntered";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";

		public static readonly StringName _projectileName = "_projectileName";

		public static readonly StringName isDowned = "isDowned";

		public static readonly StringName _reviveTimer = "_reviveTimer";

		public static readonly StringName _originalPhysiqueTypeFlags = "_originalPhysiqueTypeFlags";

		public static readonly StringName _originalMaskFlags = "_originalMaskFlags";

		public static readonly StringName _originalCollisionFlags = "_originalCollisionFlags";

		public static readonly StringName _baseSetupDone = "_baseSetupDone";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const double ReviveTime = 30.0;

	private const int SpikeFlag = 16;

	private const int BaseMaskFlags = 8;

	private FireComponentExtendCactus fireComponentExtendCactus;

	private FireComponent fireComponent;

	private StateHandle _downState;

	private StateHandle _downIdleState;

	private StateHandle _upState;

	private bool _stateSignalsConnected;

	private double _fireInterval = 2.0;

	private int _fireNum = 1;

	private string _projectileName = "SpikeQX";

	public bool isDowned;

	private double _reviveTimer;

	private int _originalPhysiqueTypeFlags;

	private int _originalMaskFlags;

	private int _originalCollisionFlags;

	private bool _baseSetupDone;

	[Export(PropertyHint.None, "")]
	public double fireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			ApplyFireComponentSettings();
		}
	}

	[Export(PropertyHint.None, "")]
	public int fireNum
	{
		get
		{
			return _fireNum;
		}
		set
		{
			_fireNum = value;
			ApplyFireComponentSettings();
		}
	}

	[Export(PropertyHint.None, "")]
	public string projectileName
	{
		get
		{
			return _projectileName;
		}
		set
		{
			_projectileName = value;
			ApplyFireComponentSettings();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode && inGame)
		{
			_originalPhysiqueTypeFlags = instance.physiqueTypeFlags;
			_originalMaskFlags = instance.maskFlags;
			_originalCollisionFlags = instance.collisionFlags;
			this.fireComponentExtendCactus = componentManager.GetRuntime<FireComponentExtendCactus>("character.fire.cactus");
			this.fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			ApplyFireComponentSettings();
			FireComponent fireComponent = this.fireComponent;
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				this.fireComponent.onlyEmitSignal = true;
				this.fireComponent.OnFireVolley += FireVolley;
			}
			FireComponent fireComponent2 = this.fireComponent;
			if (fireComponent2 != null && !fireComponent2.IsReleased)
			{
				this.fireComponent.alive = true;
			}
			FireComponentExtendCactus fireComponentExtendCactus = this.fireComponentExtendCactus;
			if (fireComponentExtendCactus != null && !fireComponentExtendCactus.IsReleased)
			{
				this.fireComponentExtendCactus.alive = true;
			}
			_downState = StateMachine?.GetStateById("plant.cactusqx.down");
			_downIdleState = StateMachine?.GetStateById("plant.cactusqx.down_idle");
			_upState = StateMachine?.GetStateById("plant.cactusqx.up");
			ConnectStateSignals();
		}
	}

	public override void _ExitTree()
	{
		FireComponent fireComponent = this.fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			this.fireComponent.OnFireVolley -= FireVolley;
		}
		DisconnectStateSignals();
		base._ExitTree();
	}

	private void ApplyFireComponentSettings()
	{
		FireComponent fireComponent = this.fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			this.fireComponent.fireInterval = (float)_fireInterval;
			this.fireComponent.fireNum = _fireNum;
			if (this.fireComponent.fireCheckList.Count > 0 && this.fireComponent.fireCheckList[0].projectile is FireComponentProjectileSingle fireComponentProjectileSingle)
			{
				fireComponentProjectileSingle.projectileName = _projectileName;
			}
			if (this.fireComponent.fireCheckList.Count > 1 && this.fireComponent.fireCheckList[1].projectile is FireComponentProjectileSingle fireComponentProjectileSingle2)
			{
				fireComponentProjectileSingle2.projectileName = _projectileName;
			}
		}
	}

	private void FireVolley(ulong randomSeed)
	{
		FireComponentCheckConfig runningCheck = fireComponent.runningCheck;
		if (!GodotObject.IsInstanceValid(runningCheck) || !GodotObject.IsInstanceValid(runningCheck.projectile))
		{
			return;
		}
		TowerDefenseProjectileCreateData projectile = runningCheck.projectile.GetProjectile();
		if (projectile == null)
		{
			return;
		}
		int collisionFlags = runningCheck.GetCollisionFlags();
		int num = ResolveVolleyBulletCount(runningCheck, _fireNum);
		if (num <= 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				flipXOverride = (Scale.X < 0f)
			};
			if (i != 0)
			{
				overrides.spawnTweenOffset = new Vector2(i * 60, 0f);
				overrides.spawnTweenDuration = 0.03f;
				overrides.spawnTweenEase = Tween.EaseType.Out;
				overrides.spawnTweenTrans = Tween.TransitionType.Quad;
			}
			fireComponent.CreateProjectile(0, new Vector2(300f, 0f), projectile, collisionFlags, camp, Vector2.Zero, 0, filterByLine: false, overrides);
		}
	}

	internal static int ResolveVolleyBulletCount(FireComponentCheckConfig activeCheck, int fireNum)
	{
		if (!GodotObject.IsInstanceValid(activeCheck))
		{
			return 0;
		}
		bool flag = !activeCheck.useParentCollision && (activeCheck.collisionFlags & 2) != 0;
		return fireNum + (flag ? 1 : 0);
	}

	private void ConnectStateSignals()
	{
		if (!_stateSignalsConnected)
		{
			StateHandle downState = _downState;
			if (downState != null && downState.IsValid)
			{
				_downState.Entered += DownEntered;
			}
			StateHandle downIdleState = _downIdleState;
			if (downIdleState != null && downIdleState.IsValid)
			{
				_downIdleState.Entered += DownIdleEntered;
				_downIdleState.PhysicsProcessing += DownIdleProcessing;
			}
			StateHandle upState = _upState;
			if (upState != null && upState.IsValid)
			{
				_upState.Entered += UpEntered;
			}
			_stateSignalsConnected = true;
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			StateHandle downState = _downState;
			if (downState != null && downState.IsValid)
			{
				_downState.Entered -= DownEntered;
			}
			StateHandle downIdleState = _downIdleState;
			if (downIdleState != null && downIdleState.IsValid)
			{
				_downIdleState.Entered -= DownIdleEntered;
				_downIdleState.PhysicsProcessing -= DownIdleProcessing;
			}
			StateHandle upState = _upState;
			if (upState != null && upState.IsValid)
			{
				_upState.Entered -= UpEntered;
			}
			_stateSignalsConnected = false;
		}
	}

	public void SetupBaseState()
	{
		if (!_baseSetupDone)
		{
			_baseSetupDone = true;
			isDowned = true;
			die = false;
			nearDie = false;
			instance.die = false;
			instance.nearDie = false;
			destroyComponent?.EndDeathSettlement();
			instance.invincible = false;
			instance.invincibleHurt = false;
			instance.invincibleSmash = false;
			instance.hitpoints = instance.hitpointsSave;
			instance.collisionFlags = _originalCollisionFlags;
			instance.maskFlags = 8;
			instance.physiqueTypeFlags = _originalPhysiqueTypeFlags | 0x10;
			instance.spikeHurt = instance.hitpoints;
			FireComponent fireComponent = this.fireComponent;
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				this.fireComponent.alive = false;
			}
			FireComponentExtendCactus fireComponentExtendCactus = this.fireComponentExtendCactus;
			if (fireComponentExtendCactus != null && !fireComponentExtendCactus.IsReleased)
			{
				this.fireComponentExtendCactus.alive = false;
			}
			SendStateEvent("ToDown");
		}
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
		if (isDowned)
		{
			SetupBaseState();
		}
		else if (fireComponentExtendCactus != null && fireComponentExtendCactus.IsUp())
		{
			sprite.SetAnimation("IdleHigh", loop: true, 0.1);
		}
		else
		{
			sprite.SetAnimation("Idle", loop: true, 0.1);
		}
	}

	public override void Destroy(bool freeInstance = true)
	{
		if (!isDowned && !isShovel && (!die || !(instance.hitpoints > 0.0)))
		{
			SetupBaseState();
		}
		else
		{
			base.Destroy(freeInstance);
		}
	}

	public void RestoreNormalState()
	{
		isDowned = false;
		_baseSetupDone = false;
		instance.maskFlags = _originalMaskFlags;
		instance.physiqueTypeFlags = _originalPhysiqueTypeFlags;
		instance.spikeHurt = 0.0;
		RestoreFullHealthAfterRevive();
		FireComponent fireComponent = this.fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			this.fireComponent.alive = true;
		}
		FireComponentExtendCactus fireComponentExtendCactus = this.fireComponentExtendCactus;
		if (fireComponentExtendCactus != null && !fireComponentExtendCactus.IsReleased)
		{
			this.fireComponentExtendCactus.alive = true;
		}
	}

	private void DownEntered()
	{
		if (sprite != null && sprite.HasClip("Down"))
		{
			sprite.SetAnimation("Down", loop: false);
		}
	}

	private void DownIdleEntered()
	{
		if (sprite != null && sprite.HasClip("DownIdle"))
		{
			sprite.SetAnimation("DownIdle");
		}
		if (_reviveTimer <= 0.0 || _reviveTimer > 30.0)
		{
			_reviveTimer = 30.0;
		}
		if (isDowned)
		{
			FireComponent fireComponent = this.fireComponent;
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				this.fireComponent.alive = false;
			}
		}
		if (isDowned)
		{
			FireComponentExtendCactus fireComponentExtendCactus = this.fireComponentExtendCactus;
			if (fireComponentExtendCactus != null && !fireComponentExtendCactus.IsReleased)
			{
				this.fireComponentExtendCactus.alive = false;
			}
		}
	}

	private void DownIdleProcessing(double delta)
	{
		if (sprite != null)
		{
			sprite.timeScale = timeScale;
		}
		_reviveTimer -= delta;
		if (_reviveTimer <= 0.0)
		{
			SendStateEvent("ToUp");
		}
	}

	private void UpEntered()
	{
		if (sprite != null && sprite.HasClip("Up"))
		{
			sprite.SetAnimation("Up", loop: false);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Down")
		{
			SendStateEvent("ToDownIdle");
		}
		else if (clip == "Up")
		{
			RestoreNormalState();
			SendStateEvent("ToRevived");
		}
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		bool flag = isDowned;
		isDowned = data?.GetValueOrDefault("isDowned", false).AsBool() ?? false;
		if (isDowned && !flag && IsNodeReady())
		{
			SetupBaseState();
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary { ["isDowned"] = isDowned };
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		isDowned = data?.GetValueOrDefault("isDowned", false).AsBool() ?? false;
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return isDowned ? 1 : 0;
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["fireNum"] = fireNum,
			["projectileName"] = projectileName,
			["fireInterval"] = fireInterval,
			["isDowned"] = isDowned,
			["reviveTimer"] = _reviveTimer,
			["originalPhysiqueTypeFlags"] = _originalPhysiqueTypeFlags,
			["originalMaskFlags"] = _originalMaskFlags
		};
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		fireNum = data.GetValueOrDefault("fireNum", 1).AsInt32();
		projectileName = data.GetValueOrDefault("projectileName", "SpikeQX").AsString();
		fireInterval = data.GetValueOrDefault("fireInterval", 2.0).AsDouble();
		isDowned = data.GetValueOrDefault("isDowned", false).AsBool();
		_reviveTimer = data.GetValueOrDefault("reviveTimer", 30.0).AsDouble();
		_originalPhysiqueTypeFlags = data.GetValueOrDefault("originalPhysiqueTypeFlags", config.physiqueTypeFlags).AsInt32();
		_originalMaskFlags = data.GetValueOrDefault("originalMaskFlags", config.maskFlags).AsInt32();
		if (isDowned && IsNodeReady())
		{
			SetupBaseState();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFireComponentSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireVolley, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "randomSeed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveVolleyBulletCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "activeCheck", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "fireNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupBaseState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "freeInstance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreNormalState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownIdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownIdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.ApplyFireComponentSettings && args.Count == 0)
		{
			ApplyFireComponentSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.FireVolley && args.Count == 1)
		{
			FireVolley(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveVolleyBulletCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveVolleyBulletCount(VariantUtils.ConvertTo<FireComponentCheckConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ConnectStateSignals && args.Count == 0)
		{
			ConnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectStateSignals && args.Count == 0)
		{
			DisconnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupBaseState && args.Count == 0)
		{
			SetupBaseState();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 1)
		{
			Destroy(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreNormalState && args.Count == 0)
		{
			RestoreNormalState();
			ret = default;
			return true;
		}
		if (method == MethodName.DownEntered && args.Count == 0)
		{
			DownEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DownIdleEntered && args.Count == 0)
		{
			DownIdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DownIdleProcessing && args.Count == 1)
		{
			DownIdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpEntered && args.Count == 0)
		{
			UpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
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
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveVolleyBulletCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveVolleyBulletCount(VariantUtils.ConvertTo<FireComponentCheckConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ApplyFireComponentSettings)
		{
			return true;
		}
		if (method == MethodName.FireVolley)
		{
			return true;
		}
		if (method == MethodName.ResolveVolleyBulletCount)
		{
			return true;
		}
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.SetupBaseState)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.RestoreNormalState)
		{
			return true;
		}
		if (method == MethodName.DownEntered)
		{
			return true;
		}
		if (method == MethodName.DownIdleEntered)
		{
			return true;
		}
		if (method == MethodName.DownIdleProcessing)
		{
			return true;
		}
		if (method == MethodName.UpEntered)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
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
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
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
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			_fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.isDowned)
		{
			isDowned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._reviveTimer)
		{
			_reviveTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._originalPhysiqueTypeFlags)
		{
			_originalPhysiqueTypeFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalMaskFlags)
		{
			_originalMaskFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalCollisionFlags)
		{
			_originalCollisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._baseSetupDone)
		{
			_baseSetupDone = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom<double>(fireInterval);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			value = VariantUtils.CreateFrom<int>(fireNum);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			value = VariantUtils.CreateFrom(in _fireNum);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		if (name == PropertyName.isDowned)
		{
			value = VariantUtils.CreateFrom(in isDowned);
			return true;
		}
		if (name == PropertyName._reviveTimer)
		{
			value = VariantUtils.CreateFrom(in _reviveTimer);
			return true;
		}
		if (name == PropertyName._originalPhysiqueTypeFlags)
		{
			value = VariantUtils.CreateFrom(in _originalPhysiqueTypeFlags);
			return true;
		}
		if (name == PropertyName._originalMaskFlags)
		{
			value = VariantUtils.CreateFrom(in _originalMaskFlags);
			return true;
		}
		if (name == PropertyName._originalCollisionFlags)
		{
			value = VariantUtils.CreateFrom(in _originalCollisionFlags);
			return true;
		}
		if (name == PropertyName._baseSetupDone)
		{
			value = VariantUtils.CreateFrom(in _baseSetupDone);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isDowned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._reviveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalPhysiqueTypeFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaskFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalCollisionFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._baseSetupDone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
		info.AddProperty(PropertyName.isDowned, Variant.From(in isDowned));
		info.AddProperty(PropertyName._reviveTimer, Variant.From(in _reviveTimer));
		info.AddProperty(PropertyName._originalPhysiqueTypeFlags, Variant.From(in _originalPhysiqueTypeFlags));
		info.AddProperty(PropertyName._originalMaskFlags, Variant.From(in _originalMaskFlags));
		info.AddProperty(PropertyName._originalCollisionFlags, Variant.From(in _originalCollisionFlags));
		info.AddProperty(PropertyName._baseSetupDone, Variant.From(in _baseSetupDone));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value2))
		{
			fireNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value3))
		{
			projectileName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value4))
		{
			_stateSignalsConnected = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value5))
		{
			_fireInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value6))
		{
			_fireNum = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value7))
		{
			_projectileName = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.isDowned, out var value8))
		{
			isDowned = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._reviveTimer, out var value9))
		{
			_reviveTimer = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._originalPhysiqueTypeFlags, out var value10))
		{
			_originalPhysiqueTypeFlags = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalMaskFlags, out var value11))
		{
			_originalMaskFlags = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalCollisionFlags, out var value12))
		{
			_originalCollisionFlags = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._baseSetupDone, out var value13))
		{
			_baseSetupDone = value13.As<bool>();
		}
	}
}
