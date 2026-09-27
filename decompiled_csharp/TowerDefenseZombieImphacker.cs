using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Imphacker/Scene/TowerDefenseZombieImphacker.cs")]
public class TowerDefenseZombieImphacker : TowerDefenseZombieImpBase
{
	public new class MethodName : TowerDefenseZombieImpBase.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName HackEntered = "HackEntered";

		public static readonly StringName HackProcessing = "HackProcessing";

		public static readonly StringName HackExited = "HackExited";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName Hack = "Hack";

		public static readonly StringName SetPacket = "SetPacket";

		public static readonly StringName HackTimerTimeout = "HackTimerTimeout";
	}

	public new class PropertyName : TowerDefenseZombieImpBase.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _hackTimer = "_hackTimer";

		public static readonly StringName isHack = "isHack";
	}

	public new class SignalName : TowerDefenseZombieImpBase.SignalName
	{
	}

	private StateHandle _hackStateHandle;

	private bool _stateSignalsConnected;

	private Timer _hackTimer;

	public bool isHack;

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_hackStateHandle = StateMachine?.GetStateById("zombie.imphacker.hack");
			StateHandle hackStateHandle = _hackStateHandle;
			if (hackStateHandle != null && hackStateHandle.IsValid)
			{
				_hackStateHandle.Entered += HackEntered;
				_hackStateHandle.Exited += HackExited;
				_hackStateHandle.Processing += HackProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_hackStateHandle != null)
			{
				_hackStateHandle.Entered -= HackEntered;
				_hackStateHandle.Exited -= HackExited;
				_hackStateHandle.Processing -= HackProcessing;
			}
			_hackStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_hackTimer = GetNode<Timer>("HackTimer");
			_hackTimer.Timeout += HackTimerTimeout;
			ConnectStateSignals();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && TowerDefenseManager.Instance.IsGameRunning() && inGame && IsInsideComponentBattlefield)
		{
			if (!isHack && _hackTimer.IsStopped())
			{
				_hackTimer.Start(10.0);
			}
			if (!instance.canBeCollection)
			{
				Color meshColor = sprite.meshColor;
				sprite.meshColor = new Color(meshColor)
				{
					A = (float)((double)meshColor.A * 0.5)
				};
			}
		}
	}

	public void HackEntered()
	{
		sprite.SetAnimation("Hack", loop: false, 0.2);
		isHack = true;
	}

	public void HackProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void HackExited()
	{
		isHack = false;
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void InWater()
	{
		base.InWater();
		sprite.SetFliter("Zombie_whitewater", open: true);
	}

	public override void OutWater()
	{
		base.OutWater();
		sprite.SetFliter("Zombie_whitewater", open: false);
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Hack")
		{
			Walk();
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "hack")
		{
			Hack();
		}
	}

	public async void Hack()
	{
		instance.canBeCollection = false;
		instance.collisionFlags = 0;
		SetPacket();
		await ToSignal(GetTree().CreateTimer(4.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		instance.canBeCollection = true;
		instance.collisionFlags = 1;
	}

	public void SetPacket()
	{
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(towerDefenseManager))
		{
			return;
		}
		TowerDefenseInGameSeedBank seedBank = towerDefenseManager.GetSeedBank();
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieImp");
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return;
		}
		TowerDefensePacketOverride towerDefensePacketOverride = null;
		if (instance.hypnoses || towerDefenseManager.IsIZMMode())
		{
			packetConfig.overrideCost = 0;
			towerDefensePacketOverride = new TowerDefensePacketOverride();
			towerDefensePacketOverride.useSucceededActions.Add(new CardActionBehaviorCreditSun
			{
				amount = 50L
			});
			packetConfig._override = towerDefensePacketOverride;
		}
		switch (towerDefenseManager.GetCurrentPacketBankMethod())
		{
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
			if (GodotObject.IsInstanceValid(seedBank) && seedBank.packetList.Count > 1)
			{
				seedBank.packetList[GD.RandRange(0, seedBank.packetList.Count - 1)].Cover(packetConfig, towerDefensePacketOverride);
			}
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR:
		{
			TowerDefenseBattleFeatureConveyorBelt conveyorBeltFeature = towerDefenseManager.GetConveyorBeltFeature();
			if (GodotObject.IsInstanceValid(conveyorBeltFeature))
			{
				conveyorBeltFeature.SpawnPacket(packetConfig);
			}
			break;
		}
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN:
		{
			TowerDefenseBattleFeatureRainMode rainModeFeature = towerDefenseManager.GetRainModeFeature();
			if (GodotObject.IsInstanceValid(rainModeFeature))
			{
				rainModeFeature.SpawnPacket(packetConfig);
			}
			break;
		}
		}
	}

	public void HackTimerTimeout()
	{
		if (!die && !nearDie && IsInsideComponentBattlefield)
		{
			SendStateEvent("ToHack");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Hack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HackTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HackEntered && args.Count == 0)
		{
			HackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.HackProcessing && args.Count == 1)
		{
			HackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HackExited && args.Count == 0)
		{
			HackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.Hack && args.Count == 0)
		{
			Hack();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacket && args.Count == 0)
		{
			SetPacket();
			ret = default;
			return true;
		}
		if (method == MethodName.HackTimerTimeout && args.Count == 0)
		{
			HackTimerTimeout();
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.HackEntered)
		{
			return true;
		}
		if (method == MethodName.HackProcessing)
		{
			return true;
		}
		if (method == MethodName.HackExited)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
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
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.Hack)
		{
			return true;
		}
		if (method == MethodName.SetPacket)
		{
			return true;
		}
		if (method == MethodName.HackTimerTimeout)
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
		if (name == PropertyName._hackTimer)
		{
			_hackTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName.isHack)
		{
			isHack = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._hackTimer)
		{
			value = VariantUtils.CreateFrom(in _hackTimer);
			return true;
		}
		if (name == PropertyName.isHack)
		{
			value = VariantUtils.CreateFrom(in isHack);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._hackTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isHack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._hackTimer, Variant.From(in _hackTimer));
		info.AddProperty(PropertyName.isHack, Variant.From(in isHack));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hackTimer, out var value2))
		{
			_hackTimer = value2.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName.isHack, out var value3))
		{
			isHack = value3.As<bool>();
		}
	}
}
