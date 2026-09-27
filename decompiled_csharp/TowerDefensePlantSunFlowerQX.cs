using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/SunFlowerQX/Scene/TowerDefensePlantSunFlowerQX.cs")]
public class TowerDefensePlantSunFlowerQX : TowerDefensePlant, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

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
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName produceType = "produceType";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName isDowned = "isDowned";

		public static readonly StringName _reviveTimer = "_reviveTimer";

		public static readonly StringName _originalPhysiqueTypeFlags = "_originalPhysiqueTypeFlags";

		public static readonly StringName _originalMaskFlags = "_originalMaskFlags";

		public static readonly StringName _originalCollisionFlags = "_originalCollisionFlags";

		public static readonly StringName _baseSetupDone = "_baseSetupDone";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName _produceType = "_produceType";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const double ReviveTime = 30.0;

	private const int SpikeFlag = 16;

	private const int BaseMaskFlags = 8;

	private ProduceComponent _produceComponent;

	private StateHandle _downState;

	private StateHandle _downIdleState;

	private StateHandle _upState;

	private bool _stateSignalsConnected;

	public bool isDowned;

	private double _reviveTimer;

	private int _originalPhysiqueTypeFlags;

	private int _originalMaskFlags;

	private int _originalCollisionFlags;

	private bool _baseSetupDone;

	private double _produceInterval = 25.0;

	private int _sunNum = 25;

	private string _produceType = "QXSun";

	[Export(PropertyHint.None, "")]
	public double produceInterval
	{
		get
		{
			return _produceInterval;
		}
		set
		{
			_produceInterval = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.produceInterval = (float)value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int sunNum
	{
		get
		{
			return _sunNum;
		}
		set
		{
			_sunNum = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.num = value;
				}
			}
		}
	}

	[Export(PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,QXSun")]
	public string produceType
	{
		get
		{
			return _produceType;
		}
		set
		{
			_produceType = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.produceType = value;
				}
			}
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
			_produceComponent = componentManager.GetRuntime<ProduceComponent>();
			ProduceComponent produceComponent = _produceComponent;
			if (produceComponent != null && !produceComponent.IsReleased)
			{
				_produceComponent.produceInterval = (float)produceInterval;
				_produceComponent.num = sunNum;
				_produceComponent.produceType = produceType;
			}
			_downState = StateMachine?.GetStateById("plant.sunflower_qx.down");
			_downIdleState = StateMachine?.GetStateById("plant.sunflower_qx.down_idle");
			_upState = StateMachine?.GetStateById("plant.sunflower_qx.up");
			ConnectStateSignals();
		}
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		base._ExitTree();
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
			_produceComponent?.SetAlive(alive: false);
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
		_produceComponent?.SetAlive(alive: true);
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
			ProduceComponent produceComponent = _produceComponent;
			if (produceComponent != null && !produceComponent.IsReleased)
			{
				_produceComponent.SetAlive(alive: false);
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
			["isDowned"] = isDowned,
			["reviveTimer"] = _reviveTimer,
			["originalPhysiqueTypeFlags"] = _originalPhysiqueTypeFlags,
			["originalMaskFlags"] = _originalMaskFlags,
			["produceInterval"] = produceInterval,
			["sunNum"] = sunNum
		};
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isDowned = data.GetValueOrDefault("isDowned", false).AsBool();
		_reviveTimer = data.GetValueOrDefault("reviveTimer", 30.0).AsDouble();
		_originalPhysiqueTypeFlags = data.GetValueOrDefault("originalPhysiqueTypeFlags", config.physiqueTypeFlags).AsInt32();
		_originalMaskFlags = data.GetValueOrDefault("originalMaskFlags", config.maskFlags).AsInt32();
		produceInterval = (data.ContainsKey("produceInterval") ? data["produceInterval"].AsDouble() : 25.0);
		sunNum = (data.ContainsKey("sunNum") ? data["sunNum"].AsInt32() : 25);
		if (isDowned && IsNodeReady())
		{
			SetupBaseState();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(20)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (name == PropertyName.produceInterval)
		{
			produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._produceInterval)
		{
			_produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			_sunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			_produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.produceInterval)
		{
			value = VariantUtils.CreateFrom<double>(produceInterval);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom<int>(sunNum);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			value = VariantUtils.CreateFrom<string>(produceType);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
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
		if (name == PropertyName._produceInterval)
		{
			value = VariantUtils.CreateFrom(in _produceInterval);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			value = VariantUtils.CreateFrom(in _sunNum);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			value = VariantUtils.CreateFrom(in _produceType);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.isDowned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._reviveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalPhysiqueTypeFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaskFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalCollisionFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._baseSetupDone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._produceInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,QXSun", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._produceType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
		info.AddProperty(PropertyName.sunNum, Variant.From<int>(sunNum));
		info.AddProperty(PropertyName.produceType, Variant.From<string>(produceType));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.isDowned, Variant.From(in isDowned));
		info.AddProperty(PropertyName._reviveTimer, Variant.From(in _reviveTimer));
		info.AddProperty(PropertyName._originalPhysiqueTypeFlags, Variant.From(in _originalPhysiqueTypeFlags));
		info.AddProperty(PropertyName._originalMaskFlags, Variant.From(in _originalMaskFlags));
		info.AddProperty(PropertyName._originalCollisionFlags, Variant.From(in _originalCollisionFlags));
		info.AddProperty(PropertyName._baseSetupDone, Variant.From(in _baseSetupDone));
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName._produceType, Variant.From(in _produceType));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.produceInterval, out var value))
		{
			produceInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.sunNum, out var value2))
		{
			sunNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.produceType, out var value3))
		{
			produceType = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value4))
		{
			_stateSignalsConnected = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isDowned, out var value5))
		{
			isDowned = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._reviveTimer, out var value6))
		{
			_reviveTimer = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._originalPhysiqueTypeFlags, out var value7))
		{
			_originalPhysiqueTypeFlags = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalMaskFlags, out var value8))
		{
			_originalMaskFlags = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalCollisionFlags, out var value9))
		{
			_originalCollisionFlags = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._baseSetupDone, out var value10))
		{
			_baseSetupDone = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._produceInterval, out var value11))
		{
			_produceInterval = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunNum, out var value12))
		{
			_sunNum = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._produceType, out var value13))
		{
			_produceType = value13.As<string>();
		}
	}
}
