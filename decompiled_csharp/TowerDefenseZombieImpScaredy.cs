using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/ImpScaredy/Scene/TowerDefenseZombieImpScaredy.cs")]
public class TowerDefenseZombieImpScaredy : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public static readonly StringName ScareEntered = "ScareEntered";

		public static readonly StringName ScareProcessing = "ScareProcessing";

		public static readonly StringName ScareExited = "ScareExited";

		public static readonly StringName UpEntered = "UpEntered";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName Timeout = "Timeout";

		public static readonly StringName RecoverHiddenNearDieState = "RecoverHiddenNearDieState";

		public static readonly StringName CanRecoverHiddenNearDie = "CanRecoverHiddenNearDie";

		public static readonly StringName TryRecoverHiddenHealth = "TryRecoverHiddenHealth";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName scare = "scare";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const string RecoveryTimerName = "Spawn";

	private const double HiddenRecoveryHealth = 25.0;

	private StateHandle _scareStateHandle;

	private StateHandle _upStateHandle;

	private bool _stateSignalsConnected;

	private CharacterTimerComponent _timerComponent;

	public bool scare;

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return;
		}
		_scareStateHandle = StateMachine?.GetStateById("zombie.imp_scaredy.scare");
		_upStateHandle = StateMachine?.GetStateById("zombie.imp_scaredy.up");
		StateHandle scareStateHandle = _scareStateHandle;
		if (scareStateHandle != null && scareStateHandle.IsValid)
		{
			StateHandle upStateHandle = _upStateHandle;
			if (upStateHandle != null && upStateHandle.IsValid)
			{
				_scareStateHandle.Entered += ScareEntered;
				_scareStateHandle.Exited += ScareExited;
				_scareStateHandle.PhysicsProcessing += ScareProcessing;
				_upStateHandle.Entered += UpEntered;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_scareStateHandle != null)
			{
				_scareStateHandle.Entered -= ScareEntered;
				_scareStateHandle.Exited -= ScareExited;
				_scareStateHandle.PhysicsProcessing -= ScareProcessing;
			}
			_scareStateHandle = null;
			if (_upStateHandle != null)
			{
				_upStateHandle.Entered -= UpEntered;
			}
			_upStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
		_timerComponent.OnTimeout += Timeout;
		OnBodyHurt += (int _num) =>
		{
			if (!sprite.pause && !scare && sprite.timeScale > 0.0)
			{
				ChangeLine();
			}
		};
		ConnectStateSignals();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= Timeout;
		}
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		if (IsInsideComponentBattlefield && !sprite.pause && sprite.timeScale > 0.0 && instance.hitpoints <= instance.hitpointsSave / 2.0)
		{
			SendStateEvent("ToScare");
		}
	}

	public void ScareEntered()
	{
		sprite.timeScale = timeScale * 2.0;
		if (inWater)
		{
			instance.maskFlags = 32;
			sprite.SetAnimation("Scaredwater", loop: false);
			sprite.AddAnimation("IdleWater", 0.0);
		}
		else
		{
			instance.maskFlags = 16;
			sprite.SetAnimation("Scared", loop: false);
			sprite.AddAnimation("IdleScare", 0.0);
		}
	}

	public void ScareProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		scare = true;
		RecoverHiddenNearDieState();
		_timerComponent.SetAlive(alive: true);
		if (!_timerComponent.IsRunning("Spawn"))
		{
			_timerComponent.Run("Spawn", 1.0);
		}
		if (instance.hitpoints >= instance.hitpointsSave)
		{
			instance.hitpoints = instance.hitpointsSave;
			if (!sprite.pause && sprite.timeScale > 0.0)
			{
				SendStateEvent("ToUp");
			}
		}
	}

	public void ScareExited()
	{
		scare = false;
		_timerComponent.SetAlive(alive: false);
		instance.maskFlags = 9;
	}

	public void UpEntered()
	{
		sprite.timeScale = timeScale * 2.0;
		if (inWater)
		{
			sprite.SetAnimation("UpWater", loop: false);
		}
		else
		{
			sprite.SetAnimation("Up", loop: false);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Up" || clip == "UpWater")
		{
			Walk();
		}
	}

	public void Timeout(string timerName)
	{
		if (timerName == "Spawn")
		{
			TryRecoverHiddenHealth();
			if (scare)
			{
				_timerComponent.Run("Spawn", 1.0);
			}
		}
	}

	private void RecoverHiddenNearDieState()
	{
		if (CanRecoverHiddenNearDie())
		{
			nearDie = false;
			instance.nearDie = false;
			destroyComponent?.EndDeathSettlement();
			targetRegistrationComponent?.NotifyTargetStateChanged();
		}
	}

	private bool CanRecoverHiddenNearDie()
	{
		if (scare && !die && !instance.die && instance.hitpoints > 0.0)
		{
			if (!nearDie)
			{
				return instance.nearDie;
			}
			return true;
		}
		return false;
	}

	private void TryRecoverHiddenHealth()
	{
		if (!scare || die || instance.die || instance.hitpoints <= 0.0)
		{
			return;
		}
		RecoverHiddenNearDieState();
		if (!sprite.pause && !(sprite.timeScale <= 0.0))
		{
			Health(25.0);
			if (instance.hitpoints > instance.hitpointsSave)
			{
				instance.hitpoints = instance.hitpointsSave;
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["scare"] = scare };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		scare = data.GetValueOrDefault("scare", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScareEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScareProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScareExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RecoverHiddenNearDieState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanRecoverHiddenNearDie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRecoverHiddenHealth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScareEntered && args.Count == 0)
		{
			ScareEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ScareProcessing && args.Count == 1)
		{
			ScareProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScareExited && args.Count == 0)
		{
			ScareExited();
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
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RecoverHiddenNearDieState && args.Count == 0)
		{
			RecoverHiddenNearDieState();
			ret = default;
			return true;
		}
		if (method == MethodName.CanRecoverHiddenNearDie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanRecoverHiddenNearDie());
			return true;
		}
		if (method == MethodName.TryRecoverHiddenHealth && args.Count == 0)
		{
			TryRecoverHiddenHealth();
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
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
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.ScareEntered)
		{
			return true;
		}
		if (method == MethodName.ScareProcessing)
		{
			return true;
		}
		if (method == MethodName.ScareExited)
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
		if (method == MethodName.Timeout)
		{
			return true;
		}
		if (method == MethodName.RecoverHiddenNearDieState)
		{
			return true;
		}
		if (method == MethodName.CanRecoverHiddenNearDie)
		{
			return true;
		}
		if (method == MethodName.TryRecoverHiddenHealth)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.scare)
		{
			scare = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName.scare)
		{
			value = VariantUtils.CreateFrom(in scare);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.scare, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.scare, Variant.From(in scare));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.scare, out var value2))
		{
			scare = value2.As<bool>();
		}
	}
}
