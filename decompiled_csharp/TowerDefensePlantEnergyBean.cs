using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/EnergyBean/Scene/TowerDefensePlantEnergyBean.cs")]
public class TowerDefensePlantEnergyBean : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SendToChargingDeferred = "SendToChargingDeferred";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public static readonly StringName ChargingEntered = "ChargingEntered";

		public static readonly StringName ChargingProcessing = "ChargingProcessing";

		public static readonly StringName ReleasingEntered = "ReleasingEntered";

		public static readonly StringName ReleasingProcessing = "ReleasingProcessing";

		public static readonly StringName WakeUpCellPlants = "WakeUpCellPlants";

		public static readonly StringName OnPressed = "OnPressed";

		public static readonly StringName ProduceBrainsun = "ProduceBrainsun";

		public static readonly StringName ApplySpeedUp = "ApplySpeedUp";

		public static readonly StringName SetLevelVisual = "SetLevelVisual";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _chargeLevel = "_chargeLevel";

		public static readonly StringName _chargeTimer = "_chargeTimer";

		public static readonly StringName _releaseTimer = "_releaseTimer";

		public static readonly StringName _wakeTimer = "_wakeTimer";

		public static readonly StringName produceType = "produceType";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const int MaxChargeLevel = 5;

	private const double ChargeInterval = 30.0;

	private const double ReleaseDuration = 15.0;

	private const double WakeInterval = 1.0;

	private const string LevelTexturePath = "res://Asset/AtlasSource/Anime/Character/Plant/Chapter9/EnergyBean/EnergyBean_level";

	private const string LevelMediaName = "EnergyBean_level0.png";

	private MousePressComponent _mousePress;

	private ProduceComponent _produce;

	private StateHandle _chargingState;

	private StateHandle _releasingState;

	private bool _stateSignalsConnected;

	private int _chargeLevel;

	private double _chargeTimer;

	private double _releaseTimer;

	private double _wakeTimer;

	[Export(PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,QXSun")]
	public string produceType = "BrainSun";

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_mousePress = componentManager.GetRuntime<MousePressComponent>();
			if (_mousePress != null)
			{
				_mousePress.OnPressed += OnPressed;
			}
			_produce = componentManager.GetRuntime<ProduceComponent>();
			_chargingState = StateMachine?.GetStateById("plant.energy_bean.charging");
			_releasingState = StateMachine?.GetStateById("plant.energy_bean.releasing");
			ConnectStateSignals();
			SetLevelVisual(_chargeLevel);
			Callable.From(SendToChargingDeferred).CallDeferred();
		}
	}

	private void SendToChargingDeferred()
	{
		SendStateEvent("ToCharging");
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		if (_mousePress != null)
		{
			_mousePress.OnPressed -= OnPressed;
			_mousePress = null;
		}
		_produce = null;
		base._ExitTree();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (!Engine.IsEditorHint() && !die)
		{
			_wakeTimer += delta;
			if (_wakeTimer >= 1.0)
			{
				_wakeTimer = 0.0;
				WakeUpCellPlants();
			}
		}
	}

	private void ConnectStateSignals()
	{
		if (!_stateSignalsConnected)
		{
			StateHandle chargingState = _chargingState;
			if (chargingState != null && chargingState.IsValid)
			{
				_chargingState.Entered += ChargingEntered;
				_chargingState.PhysicsProcessing += ChargingProcessing;
			}
			StateHandle releasingState = _releasingState;
			if (releasingState != null && releasingState.IsValid)
			{
				_releasingState.Entered += ReleasingEntered;
				_releasingState.PhysicsProcessing += ReleasingProcessing;
			}
			_stateSignalsConnected = true;
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			StateHandle chargingState = _chargingState;
			if (chargingState != null && chargingState.IsValid)
			{
				_chargingState.Entered -= ChargingEntered;
				_chargingState.PhysicsProcessing -= ChargingProcessing;
			}
			StateHandle releasingState = _releasingState;
			if (releasingState != null && releasingState.IsValid)
			{
				_releasingState.Entered -= ReleasingEntered;
				_releasingState.PhysicsProcessing -= ReleasingProcessing;
			}
			_stateSignalsConnected = false;
		}
	}

	private void ChargingEntered()
	{
		_chargeTimer = 0.0;
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.SetAnimation("Idle");
		}
	}

	private void ChargingProcessing(double delta)
	{
		_chargeTimer += delta;
		if (_chargeTimer >= 30.0 && _chargeLevel < 5)
		{
			_chargeTimer = 0.0;
			_chargeLevel++;
			SetLevelVisual(_chargeLevel);
		}
	}

	private void ReleasingEntered()
	{
		_releaseTimer = 0.0;
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.SetAnimation("Shooting", loop: false, 0.1);
			sprite.AddAnimation("IdleB", 0.0);
		}
	}

	private void ReleasingProcessing(double delta)
	{
		_releaseTimer += delta;
		if (_releaseTimer >= 15.0)
		{
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAnimation("Idle");
			}
			SetLevelVisual(0);
			_chargeLevel = 0;
			SendStateEvent("ToCharging");
		}
	}

	private void WakeUpCellPlants()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (character != null && character.camp == camp)
			{
				character.WakeUp();
			}
		}
	}

	private void OnPressed(Vector2 pos)
	{
		StateHandle chargingState = _chargingState;
		if (chargingState != null && chargingState.IsActive && _chargeLevel > 0)
		{
			int chargeLevel = _chargeLevel;
			ProduceBrainsun(25 * chargeLevel);
			ApplySpeedUp(chargeLevel);
			SendStateEvent("ToReleasing");
		}
	}

	private void ProduceBrainsun(int amount)
	{
		if (_produce == null)
		{
			_produce = componentManager.GetRuntime<ProduceComponent>();
		}
		if (_produce != null)
		{
			bool flag = GodotObject.IsInstanceValid(instance) && instance.hypnoses;
			_produce.produceType = (flag ? "Sun" : "BrainSun");
			_produce.Create(GlobalPosition, amount);
		}
	}

	private void ApplySpeedUp(int level)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		TowerDefenseCharacterBuffCoffee towerDefenseCharacterBuffCoffee = new TowerDefenseCharacterBuffCoffee
		{
			timeScaleValue = level,
			time = 15.0
		};
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (character != null && GodotObject.IsInstanceValid(character) && character.camp == camp && character != this)
			{
				character.buff.AddBuff(towerDefenseCharacterBuffCoffee.Duplicate(deep: true) as TowerDefenseCharacterBuffConfig);
			}
		}
	}

	private void SetLevelVisual(int level)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			int num = Mathf.Clamp(level, 0, 5);
			sprite.SetAtlasReplace("EnergyBean_level0.png", "res://Asset/AtlasSource/Anime/Character/Plant/Chapter9/EnergyBean/EnergyBean_level" + num + ".png");
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["EnergyBeanChargeLevel"] = _chargeLevel;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		if (data != null && data.ContainsKey("EnergyBeanChargeLevel"))
		{
			_chargeLevel = Mathf.Clamp(data["EnergyBeanChargeLevel"].AsInt32(), 0, 5);
		}
		SetLevelVisual(_chargeLevel);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendToChargingDeferred, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChargingEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChargingProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleasingEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleasingProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WakeUpCellPlants, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProduceBrainsun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "amount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySpeedUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "level", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLevelVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "level", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SendToChargingDeferred && args.Count == 0)
		{
			SendToChargingDeferred();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.ChargingEntered && args.Count == 0)
		{
			ChargingEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ChargingProcessing && args.Count == 1)
		{
			ChargingProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasingEntered && args.Count == 0)
		{
			ReleasingEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasingProcessing && args.Count == 1)
		{
			ReleasingProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WakeUpCellPlants && args.Count == 0)
		{
			WakeUpCellPlants();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPressed && args.Count == 1)
		{
			OnPressed(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProduceBrainsun && args.Count == 1)
		{
			ProduceBrainsun(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpeedUp && args.Count == 1)
		{
			ApplySpeedUp(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLevelVisual && args.Count == 1)
		{
			SetLevelVisual(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SendToChargingDeferred)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
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
		if (method == MethodName.ChargingEntered)
		{
			return true;
		}
		if (method == MethodName.ChargingProcessing)
		{
			return true;
		}
		if (method == MethodName.ReleasingEntered)
		{
			return true;
		}
		if (method == MethodName.ReleasingProcessing)
		{
			return true;
		}
		if (method == MethodName.WakeUpCellPlants)
		{
			return true;
		}
		if (method == MethodName.OnPressed)
		{
			return true;
		}
		if (method == MethodName.ProduceBrainsun)
		{
			return true;
		}
		if (method == MethodName.ApplySpeedUp)
		{
			return true;
		}
		if (method == MethodName.SetLevelVisual)
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
		if (name == PropertyName._chargeLevel)
		{
			_chargeLevel = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._chargeTimer)
		{
			_chargeTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._releaseTimer)
		{
			_releaseTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._wakeTimer)
		{
			_wakeTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			produceType = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName._chargeLevel)
		{
			value = VariantUtils.CreateFrom(in _chargeLevel);
			return true;
		}
		if (name == PropertyName._chargeTimer)
		{
			value = VariantUtils.CreateFrom(in _chargeTimer);
			return true;
		}
		if (name == PropertyName._releaseTimer)
		{
			value = VariantUtils.CreateFrom(in _releaseTimer);
			return true;
		}
		if (name == PropertyName._wakeTimer)
		{
			value = VariantUtils.CreateFrom(in _wakeTimer);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			value = VariantUtils.CreateFrom(in produceType);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._chargeLevel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._chargeTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._releaseTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._wakeTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,QXSun", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._chargeLevel, Variant.From(in _chargeLevel));
		info.AddProperty(PropertyName._chargeTimer, Variant.From(in _chargeTimer));
		info.AddProperty(PropertyName._releaseTimer, Variant.From(in _releaseTimer));
		info.AddProperty(PropertyName._wakeTimer, Variant.From(in _wakeTimer));
		info.AddProperty(PropertyName.produceType, Variant.From(in produceType));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._chargeLevel, out var value2))
		{
			_chargeLevel = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._chargeTimer, out var value3))
		{
			_chargeTimer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._releaseTimer, out var value4))
		{
			_releaseTimer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._wakeTimer, out var value5))
		{
			_wakeTimer = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.produceType, out var value6))
		{
			produceType = value6.As<string>();
		}
	}
}
