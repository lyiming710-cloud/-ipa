using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/SlotMachine/TowerDefenseBattleFeatureSlotMachine.cs")]
public class TowerDefenseBattleFeatureSlotMachine : TowerDefenseBattleFeature
{
	private sealed class SpinResult
	{
		public string[] Items = new string[3] { "PlantPeaShooterSingle", "PlantWallnut", "PlantSnowPea" };

		public string MatchedItem = "";

		public int MatchedCount;

		public int RewardCount;
	}

	private enum SlotRewardType
	{
		Packet,
		Sun,
		BrainSun,
		Diamond,
		Coin
	}

	private sealed class SlotItem
	{
		public string Key = "";

		public string DisplayKey = "";

		public SlotRewardType Type;

		public int Amount = 1;

		public int Weight = 1;
	}

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName GetSpinItemKeys = "GetSpinItemKeys";

		public static readonly StringName CanSpin = "CanSpin";

		public static readonly StringName RequestSpin = "RequestSpin";

		public static readonly StringName GetStatusText = "GetStatusText";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName ReadConfig = "ReadConfig";

		public static readonly StringName ReadPacketList = "ReadPacketList";

		public static readonly StringName FinishSpin = "FinishSpin";

		public static readonly StringName CreateLevelClearReward = "CreateLevelClearReward";

		public static readonly StringName SpawnPacketReward = "SpawnPacketReward";

		public static readonly StringName CreateRewardVelocity = "CreateRewardVelocity";

		public static readonly StringName GetRewardSpawnPosition = "GetRewardSpawnPosition";

		public static readonly StringName GetShortPacketName = "GetShortPacketName";

		public static readonly StringName FormatNumber = "FormatNumber";

		public static readonly StringName FormatText = "FormatText";

		public static readonly StringName SetMessage = "SetMessage";

		public static readonly StringName SubscribeSunAccount = "SubscribeSunAccount";

		public static readonly StringName UnsubscribeSunAccount = "UnsubscribeSunAccount";

		public static readonly StringName UpdateControl = "UpdateControl";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName slotMachineControl = "slotMachineControl";

		public static readonly StringName open = "open";

		public static readonly StringName running = "running";

		public static readonly StringName autoStart = "autoStart";

		public static readonly StringName cost = "cost";

		public static readonly StringName cooldown = "cooldown";

		public static readonly StringName cooldownTimer = "cooldownTimer";

		public static readonly StringName spinDuration = "spinDuration";

		public static readonly StringName packetAliveTime = "packetAliveTime";

		public static readonly StringName spinNum = "spinNum";

		public static readonly StringName clearSunReward = "clearSunReward";

		public static readonly StringName closedText = "closedText";

		public static readonly StringName waitText = "waitText";

		public static readonly StringName hostOnlyText = "hostOnlyText";

		public static readonly StringName spinningText = "spinningText";

		public static readonly StringName missText = "missText";

		public static readonly StringName spinText = "spinText";

		public static readonly StringName spinCostTextTemplate = "spinCostTextTemplate";

		public static readonly StringName insufficientSunTextTemplate = "insufficientSunTextTemplate";

		public static readonly StringName cooldownTextTemplate = "cooldownTextTemplate";

		public static readonly StringName rewardCountTextTemplate = "rewardCountTextTemplate";

		public static readonly StringName sunRewardTextTemplate = "sunRewardTextTemplate";

		public static readonly StringName brainRewardTextTemplate = "brainRewardTextTemplate";

		public static readonly StringName diamondRewardTextTemplate = "diamondRewardTextTemplate";

		public static readonly StringName coinRewardTextTemplate = "coinRewardTextTemplate";

		public static readonly StringName _spinInProgress = "_spinInProgress";

		public static readonly StringName _clearRewardCreated = "_clearRewardCreated";

		public static readonly StringName _message = "_message";

		public static readonly StringName _messageTimer = "_messageTimer";

		public static readonly StringName _sunFeature = "_sunFeature";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _slotMachineControlScene;

	public SlotMachineControl slotMachineControl;

	public bool open = true;

	public bool running;

	public bool autoStart = true;

	public int cost = 25;

	public double cooldown = 0.5;

	public double cooldownTimer;

	public double spinDuration = 1.25;

	public double packetAliveTime = 15.0;

	public int spinNum;

	public int clearSunReward;

	public string closedText = "CLOSED";

	public string waitText = "WAIT";

	public string hostOnlyText = "HOST ONLY";

	public string spinningText = "SPINNING";

	public string missText = "MISS";

	public string spinText = "SPIN";

	public string spinCostTextTemplate = "SPIN {0}";

	public string insufficientSunTextTemplate = "{0} SUN";

	public string cooldownTextTemplate = "{0}s";

	public string rewardCountTextTemplate = "{0}x {1}";

	public string sunRewardTextTemplate = "SUN {0}";

	public string brainRewardTextTemplate = "BRAIN {0}";

	public string diamondRewardTextTemplate = "DIAMOND {0}";

	public string coinRewardTextTemplate = "COIN {0}";

	private readonly List<SlotItem> _slotItems = new List<SlotItem>();

	private readonly System.Collections.Generic.Dictionary<string, SlotItem> _slotItemLookup = new System.Collections.Generic.Dictionary<string, SlotItem>(StringComparer.OrdinalIgnoreCase);

	private bool _spinInProgress;

	private bool _clearRewardCreated;

	private string _message = "";

	private double _messageTimer;

	private readonly EconomyAccountId _sunAccountId = EconomyAccountId.Local;

	private TowerDefenseBattleFeatureSun _sunFeature;

	private static PackedScene SlotMachineControlScene => _slotMachineControlScene ?? (_slotMachineControlScene = GD.Load<PackedScene>("res://Registry/Battle/Feature/SlotMachine/SlotMachineControl.tscn"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		ReadConfig();
		slotMachineControl = SlotMachineControlScene.Instantiate<SlotMachineControl>(PackedScene.GenEditState.Disabled);
		slotMachineControl.Init(this);
		slotMachineControl.Visible = open;
		control.AddUIToTopBankContainer(slotMachineControl);
	}

	public override Task GameInit()
	{
		SubscribeSunAccount();
		running = false;
		_clearRewardCreated = false;
		UpdateControl();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		SubscribeSunAccount();
		UpdateControl();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		running = open && autoStart;
		UpdateControl();
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		running = open && autoStart;
		UpdateControl();
		return Task.CompletedTask;
	}

	public override void Process(double delta)
	{
		bool flag = false;
		if (cooldownTimer > 0.0)
		{
			int num = (int)Math.Ceiling(cooldownTimer);
			cooldownTimer = Math.Max(0.0, cooldownTimer - delta);
			flag = num != (int)Math.Ceiling(cooldownTimer);
		}
		if (_messageTimer > 0.0)
		{
			_messageTimer = Math.Max(0.0, _messageTimer - delta);
			flag |= _messageTimer <= 0.0;
		}
		if (flag)
		{
			UpdateControl();
		}
	}

	public override void GameFail()
	{
		running = false;
		UpdateControl();
	}

	public override void Destroy()
	{
		UnsubscribeSunAccount();
		running = false;
		_spinInProgress = false;
		if (GodotObject.IsInstanceValid(slotMachineControl))
		{
			slotMachineControl.CancelPendingWork();
			slotMachineControl.QueueFree();
		}
		slotMachineControl = null;
		_slotItems.Clear();
		_slotItemLookup.Clear();
		base.Destroy();
	}

	public string[] GetSpinItemKeys()
	{
		if (_slotItems.Count == 0)
		{
			return new string[4] { "PlantPeaShooterSingle", "PlantWallnut", "PlantSnowPea", "PlantSunFlower" };
		}
		string[] array = new string[_slotItems.Count];
		for (int i = 0; i < _slotItems.Count; i++)
		{
			array[i] = _slotItems[i].Key;
		}
		return array;
	}

	public bool CanSpin()
	{
		if (!open || !running || _spinInProgress || cooldownTimer > 0.0)
		{
			return false;
		}
		if (cost <= 0)
		{
			return true;
		}
		return TowerDefenseManager.Instance.CanAffordSun(_sunAccountId, cost);
	}

	public void RequestSpin()
	{
		if (_spinInProgress)
		{
			return;
		}
		if (!open || !running)
		{
			SetMessage(waitText, 0.8);
		}
		else if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			SetMessage(hostOnlyText, 1.0);
		}
		else if (cooldownTimer > 0.0)
		{
			SetMessage(FormatText(cooldownTextTemplate, FormatNumber(Math.Ceiling(cooldownTimer))), 0.4);
		}
		else if (GodotObject.IsInstanceValid(slotMachineControl))
		{
			SunSpendReceipt receipt = null;
			if (cost > 0 && !TowerDefenseManager.Instance.TryBeginSunSpend(_sunAccountId, cost, out receipt))
			{
				SetMessage(FormatText(insufficientSunTextTemplate, FormatNumber(cost)), 1.0);
				AudioManager.Instance.AudioPlay("ShovelDeny");
				return;
			}
			SpinResult spinResult = RollSpinResult();
			receipt?.TryCommit();
			spinNum++;
			cooldownTimer = cooldown;
			_spinInProgress = true;
			SetMessage(spinningText, spinDuration);
			UpdateControl();
			slotMachineControl.PlaySpin(spinResult.Items, spinDuration, FinishSpin);
		}
	}

	public string GetStatusText()
	{
		if (!open)
		{
			return closedText;
		}
		if (_spinInProgress)
		{
			return spinningText;
		}
		if (_messageTimer > 0.0 && _message != "")
		{
			return _message;
		}
		if (!running)
		{
			return waitText;
		}
		if (cooldownTimer > 0.0)
		{
			return FormatText(cooldownTextTemplate, FormatNumber(Math.Ceiling(cooldownTimer)));
		}
		if (cost > 0)
		{
			if (!TowerDefenseManager.Instance.CanAffordSun(_sunAccountId, cost))
			{
				return FormatText(insufficientSunTextTemplate, FormatNumber(cost));
			}
			return FormatText(spinCostTextTemplate, FormatNumber(cost));
		}
		return spinText;
	}

	public override Dictionary SaveFeature()
	{
		return new Dictionary
		{
			["open"] = open,
			["running"] = running,
			["cooldownTimer"] = cooldownTimer,
			["spinNum"] = spinNum,
			["clearRewardCreated"] = _clearRewardCreated
		};
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		open = _data.GetValueOrDefault("open", open).AsBool();
		running = _data.GetValueOrDefault("running", running).AsBool();
		cooldownTimer = _data.GetValueOrDefault("cooldownTimer", 0.0).AsDouble();
		spinNum = _data.GetValueOrDefault("spinNum", 0).AsInt32();
		_clearRewardCreated = _data.GetValueOrDefault("clearRewardCreated", false).AsBool();
		if (GodotObject.IsInstanceValid(slotMachineControl))
		{
			slotMachineControl.Visible = open;
		}
		UpdateControl();
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary
		{
			["open"] = open,
			["running"] = running,
			["cooldownTimer"] = cooldownTimer,
			["spinNum"] = spinNum,
			["clearRewardCreated"] = _clearRewardCreated
		};
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		open = _data.GetValueOrDefault("open", open).AsBool();
		running = _data.GetValueOrDefault("running", running).AsBool();
		cooldownTimer = _data.GetValueOrDefault("cooldownTimer", cooldownTimer).AsDouble();
		spinNum = _data.GetValueOrDefault("spinNum", spinNum).AsInt32();
		_clearRewardCreated = _data.GetValueOrDefault("clearRewardCreated", _clearRewardCreated).AsBool();
		if (GodotObject.IsInstanceValid(slotMachineControl))
		{
			slotMachineControl.Visible = open;
		}
		UpdateControl();
	}

	private void ReadConfig()
	{
		open = data.GetValueOrDefault("Open", true).AsBool();
		autoStart = data.GetValueOrDefault("AutoStart", true).AsBool();
		cost = Math.Max(0, data.GetValueOrDefault("Cost", data.GetValueOrDefault("SunCost", 25)).AsInt32());
		cooldown = Math.Max(0.0, data.GetValueOrDefault("Cooldown", 0.5).AsDouble());
		spinDuration = Math.Max(0.35, data.GetValueOrDefault("SpinDuration", 1.25).AsDouble());
		packetAliveTime = Math.Max(0.1, data.GetValueOrDefault("PacketAliveTime", 15.0).AsDouble());
		clearSunReward = Math.Max(0, data.GetValueOrDefault("ClearSunReward", data.GetValueOrDefault("PassSunReward", 0)).AsInt32());
		closedText = data.GetValueOrDefault("ClosedText", "CLOSED").AsString();
		waitText = data.GetValueOrDefault("WaitText", "WAIT").AsString();
		hostOnlyText = data.GetValueOrDefault("HostOnlyText", "HOST ONLY").AsString();
		spinningText = data.GetValueOrDefault("SpinningText", "SPINNING").AsString();
		missText = data.GetValueOrDefault("MissText", "MISS").AsString();
		spinText = data.GetValueOrDefault("SpinText", "SPIN").AsString();
		spinCostTextTemplate = data.GetValueOrDefault("SpinCostTextTemplate", "SPIN {0}").AsString();
		insufficientSunTextTemplate = data.GetValueOrDefault("InsufficientSunTextTemplate", "{0} SUN").AsString();
		cooldownTextTemplate = data.GetValueOrDefault("CooldownTextTemplate", "{0}s").AsString();
		rewardCountTextTemplate = data.GetValueOrDefault("RewardCountTextTemplate", "{0}x {1}").AsString();
		sunRewardTextTemplate = data.GetValueOrDefault("SunRewardTextTemplate", "SUN {0}").AsString();
		brainRewardTextTemplate = data.GetValueOrDefault("BrainRewardTextTemplate", "BRAIN {0}").AsString();
		diamondRewardTextTemplate = data.GetValueOrDefault("DiamondRewardTextTemplate", "DIAMOND {0}").AsString();
		coinRewardTextTemplate = data.GetValueOrDefault("CoinRewardTextTemplate", "COIN {0}").AsString();
		ReadPacketList(data.GetValueOrDefault("PacketList", new Godot.Collections.Array()).AsGodotArray());
	}

	private void ReadPacketList(Godot.Collections.Array packetListData)
	{
		_slotItems.Clear();
		_slotItemLookup.Clear();
		foreach (Variant packetListDatum in packetListData)
		{
			AddSlotItem(ParseSlotItem(packetListDatum.AsString()));
		}
		if (_slotItems.Count == 0)
		{
			AddSlotItem(ParseSlotItem("PlantPeaShooterSingle"));
			AddSlotItem(ParseSlotItem("PlantWallnut"));
			AddSlotItem(ParseSlotItem("PlantSnowPea"));
			AddSlotItem(ParseSlotItem("PlantSunFlower"));
		}
	}

	private void AddSlotItem(SlotItem item)
	{
		if (item != null && !(item.Key == "") && !_slotItemLookup.ContainsKey(item.Key))
		{
			_slotItems.Add(item);
			_slotItemLookup[item.Key] = item;
		}
	}

	private SpinResult RollSpinResult()
	{
		string[] array = new string[3];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PickSlotItem().Key;
		}
		return BuildSpinResult(array);
	}

	private SpinResult BuildSpinResult(string[] items)
	{
		System.Collections.Generic.Dictionary<string, int> dictionary = new System.Collections.Generic.Dictionary<string, int>();
		foreach (string text in items)
		{
			if (!(text == ""))
			{
				dictionary.TryGetValue(text, out var value);
				dictionary[text] = value + 1;
			}
		}
		string matchedItem = "";
		int num = 0;
		foreach (KeyValuePair<string, int> item in dictionary)
		{
			if (item.Value > num)
			{
				matchedItem = item.Key;
				num = item.Value;
			}
		}
		int rewardCount = 0;
		if (num >= 3)
		{
			rewardCount = 3;
		}
		else if (num >= 2)
		{
			rewardCount = 1;
		}
		return new SpinResult
		{
			Items = items,
			MatchedItem = matchedItem,
			MatchedCount = num,
			RewardCount = rewardCount
		};
	}

	private SlotItem PickSlotItem()
	{
		if (_slotItems.Count == 0)
		{
			return ParseSlotItem("PlantPeaShooterSingle");
		}
		double num = 0.0;
		foreach (SlotItem slotItem in _slotItems)
		{
			num += (double)Math.Max(1, slotItem.Weight);
		}
		double num2 = ((num > 0.0) ? ((double)GD.Randf() * num) : 0.0);
		foreach (SlotItem slotItem2 in _slotItems)
		{
			num2 -= (double)Math.Max(1, slotItem2.Weight);
			if (num2 < 0.0)
			{
				return slotItem2;
			}
		}
		List<SlotItem> slotItems = _slotItems;
		return slotItems[slotItems.Count - 1];
	}

	private void FinishSpin(string[] finalItems)
	{
		_spinInProgress = false;
		SpinResult result = BuildSpinResult(finalItems);
		string message = ApplyReward(result);
		SetMessage(message, 1.6);
		UpdateControl();
	}

	private string ApplyReward(SpinResult result)
	{
		Vector2 rewardSpawnPosition = GetRewardSpawnPosition();
		if (result.MatchedCount < 2 || result.RewardCount <= 0)
		{
			return missText;
		}
		if (result.RewardCount > 0)
		{
			SlotItem slotItem = GetSlotItem(result.MatchedItem);
			for (int i = 0; i < result.RewardCount; i++)
			{
				float x = (float)(((double)i - (double)(result.RewardCount - 1) * 0.5) * 46.0);
				Vector2 pos = rewardSpawnPosition + new Vector2(x, 0f);
				SpawnSlotReward(slotItem, pos);
			}
			return FormatText(rewardCountTextTemplate, FormatNumber(result.RewardCount), GetRewardLabel(slotItem));
		}
		return missText;
	}

	public void CreateLevelClearReward(Vector2 pos)
	{
		if (!_clearRewardCreated && clearSunReward > 0)
		{
			_clearRewardCreated = true;
			TowerDefenseManager.Instance.SunCreate(_sunAccountId, pos + new Vector2(0f, -36f), clearSunReward, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2(0f, -320f));
		}
	}

	private SlotItem GetSlotItem(string itemKey)
	{
		if (itemKey != "" && _slotItemLookup.TryGetValue(itemKey, out var value))
		{
			return value;
		}
		return ParseSlotItem(itemKey);
	}

	private void SpawnSlotReward(SlotItem reward, Vector2 pos)
	{
		switch (reward.Type)
		{
		case SlotRewardType.Sun:
			TowerDefenseManager.Instance.SunCreate(_sunAccountId, pos, reward.Amount, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, CreateRewardVelocity());
			break;
		case SlotRewardType.BrainSun:
			TowerDefenseManager.Instance.BrainSunCreate(_sunAccountId, pos, reward.Amount, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, CreateRewardVelocity());
			break;
		case SlotRewardType.Diamond:
			TowerDefenseManager.Instance.CoinCreate(pos, Math.Max(1, reward.Amount) * 1000, 0.0, CreateRewardVelocity(), 980.0);
			break;
		case SlotRewardType.Coin:
			TowerDefenseManager.Instance.CoinCreate(pos, Math.Max(10, reward.Amount), 0.0, CreateRewardVelocity(), 980.0);
			break;
		default:
			SpawnPacketReward(reward.Key, pos);
			break;
		}
	}

	private void SpawnPacketReward(string packetName, Vector2 pos)
	{
		if (!(packetName == ""))
		{
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
			if (!GodotObject.IsInstanceValid(packetConfig))
			{
				GD.PushWarning("[SlotMachine] Invalid packet reward: " + packetName);
			}
			else
			{
				TowerDefenseManager.Instance.SpawnPacket(_sunAccountId, packetConfig, pos, packetAliveTime, isFall: false);
			}
		}
	}

	private static Vector2 CreateRewardVelocity()
	{
		return new Vector2((float)GD.RandRange(-50.0, 50.0), -400f);
	}

	private Vector2 GetRewardSpawnPosition()
	{
		if (TowerDefenseManager.GetMapFeature() == null)
		{
			return new Vector2(400f, 120f);
		}
		Vector2 mapGridBeginPos = TowerDefenseManager.Instance.GetMapGridBeginPos();
		return new Vector2((float)(((double)mapGridBeginPos.X + TowerDefenseManager.Instance.GetMapGroundRight()) * 0.5), mapGridBeginPos.Y - 60f);
	}

	private static string GetShortPacketName(string packetName)
	{
		switch (packetName)
		{
		case "PlantPeaShooterSingle":
			return "PEA";
		case "PlantWallnut":
			return "NUT";
		case "PlantSnowPea":
			return "ICE";
		case "PlantSunFlower":
			return "SUN";
		case "PlantPresentBox":
			return "BOX";
		default:
		{
			string result;
			if (!packetName.StartsWith("Plant", StringComparison.OrdinalIgnoreCase))
			{
				result = packetName;
			}
			else
			{
				result = packetName.Substring(5, packetName.Length - 5);
			}
			return result;
		}
		}
	}

	private string GetRewardLabel(SlotItem reward)
	{
		return reward.Type switch
		{
			SlotRewardType.Sun => FormatText(sunRewardTextTemplate, FormatNumber(reward.Amount)), 
			SlotRewardType.BrainSun => FormatText(brainRewardTextTemplate, FormatNumber(reward.Amount)), 
			SlotRewardType.Diamond => FormatText(diamondRewardTextTemplate, FormatNumber(reward.Amount)), 
			SlotRewardType.Coin => FormatText(coinRewardTextTemplate, FormatNumber(reward.Amount)), 
			_ => GetShortPacketName(reward.Key), 
		};
	}

	private static string FormatNumber(double value)
	{
		return value.ToString("0.##", CultureInfo.InvariantCulture);
	}

	private static string FormatText(string template, params string[] values)
	{
		string text = template ?? "";
		for (int i = 0; i < values.Length; i++)
		{
			text = text.Replace($"{{{i}}}", values[i] ?? "", StringComparison.Ordinal);
		}
		return text;
	}

	private static SlotItem ParseSlotItem(string rawItem)
	{
		string text = (rawItem ?? "").Trim();
		if (text == "")
		{
			return new SlotItem();
		}
		int weight = 1;
		bool flag = false;
		int num = text.LastIndexOf('@');
		if (num >= 0)
		{
			string text2 = text;
			int num2 = num + 1;
			if (int.TryParse(text2.Substring(num2, text2.Length - num2).Trim(), out var result) && result > 0)
			{
				weight = result;
				flag = true;
			}
			text = text.Substring(0, num).Trim();
		}
		string[] array = text.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		string text3 = ((array.Length != 0) ? array[0] : text).ToUpperInvariant();
		if (!flag && array.Length >= 3 && int.TryParse(array[2], out var result2) && result2 > 0)
		{
			weight = result2;
		}
		switch (text3)
		{
		case "SUN":
			return CreateValueReward(SlotRewardType.Sun, "SUN", array, 25, weight, "PlantSunFlower");
		case "BRAIN":
		case "BRAINSUN":
		case "SUNBRAIN":
			return CreateValueReward(SlotRewardType.BrainSun, "BRAIN", array, 25, weight, "ItemBrain");
		case "DIAMOND":
			return CreateValueReward(SlotRewardType.Diamond, "DIAMOND", array, 1, weight, "PlantDiamondseed");
		case "COIN":
			return CreateValueReward(SlotRewardType.Coin, "COIN", array, 1000, weight, "PlantDiamondseed");
		default:
			return CreatePacketReward(array, text, weight);
		}
	}

	private static SlotItem CreateValueReward(SlotRewardType type, string prefix, string[] parts, int defaultAmount, int weight, string displayKey)
	{
		int num = defaultAmount;
		if (parts.Length >= 2 && int.TryParse(parts[1], out var result) && result > 0)
		{
			num = result;
		}
		return new SlotItem
		{
			Key = $"{prefix}:{num}",
			DisplayKey = displayKey,
			Type = type,
			Amount = num,
			Weight = Math.Max(1, weight)
		};
	}

	private static SlotItem CreatePacketReward(string[] parts, string token, int weight)
	{
		string text = token;
		if (parts.Length == 2 && int.TryParse(parts[1], out var result) && result > 0)
		{
			text = parts[0];
			weight = result;
		}
		return new SlotItem
		{
			Key = text,
			DisplayKey = text,
			Type = SlotRewardType.Packet,
			Amount = 1,
			Weight = Math.Max(1, weight)
		};
	}

	private void SetMessage(string message, double duration)
	{
		_message = message;
		_messageTimer = duration;
		UpdateControl();
	}

	private void SubscribeSunAccount()
	{
		UnsubscribeSunAccount();
		_sunFeature = GetFeature<TowerDefenseBattleFeatureSun>("Sun");
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange += OnAccountSunChanged;
		}
	}

	private void UnsubscribeSunAccount()
	{
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange -= OnAccountSunChanged;
		}
		_sunFeature = null;
	}

	private void OnAccountSunChanged(EconomyAccountId accountId, long _balance, SunTransactionReason _reason)
	{
		if (accountId == _sunAccountId)
		{
			UpdateControl();
		}
	}

	private void UpdateControl()
	{
		if (GodotObject.IsInstanceValid(slotMachineControl))
		{
			slotMachineControl.SetStatusText(GetStatusText());
			slotMachineControl.SetInteractable(CanSpin());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSpinItemKeys, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSpin, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestSpin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetStatusText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadPacketList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "packetListData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishSpin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "finalItems", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateLevelClearReward, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnPacketReward, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateRewardVelocity, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetRewardSpawnPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetShortPacketName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatNumber, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "template", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SubscribeSunAccount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeSunAccount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSpinItemKeys && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetSpinItemKeys());
			return true;
		}
		if (method == MethodName.CanSpin && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSpin());
			return true;
		}
		if (method == MethodName.RequestSpin && args.Count == 0)
		{
			RequestSpin();
			ret = default;
			return true;
		}
		if (method == MethodName.GetStatusText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetStatusText());
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadConfig && args.Count == 0)
		{
			ReadConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadPacketList && args.Count == 1)
		{
			ReadPacketList(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishSpin && args.Count == 1)
		{
			FinishSpin(VariantUtils.ConvertTo<string[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateLevelClearReward && args.Count == 1)
		{
			CreateLevelClearReward(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnPacketReward && args.Count == 2)
		{
			SpawnPacketReward(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateRewardVelocity && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(CreateRewardVelocity());
			return true;
		}
		if (method == MethodName.GetRewardSpawnPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetRewardSpawnPosition());
			return true;
		}
		if (method == MethodName.GetShortPacketName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetShortPacketName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatNumber && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatNumber(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.SetMessage && args.Count == 2)
		{
			SetMessage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeSunAccount && args.Count == 0)
		{
			SubscribeSunAccount();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeSunAccount && args.Count == 0)
		{
			UnsubscribeSunAccount();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateControl && args.Count == 0)
		{
			UpdateControl();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateRewardVelocity && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(CreateRewardVelocity());
			return true;
		}
		if (method == MethodName.GetShortPacketName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetShortPacketName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatNumber && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatNumber(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.GetSpinItemKeys)
		{
			return true;
		}
		if (method == MethodName.CanSpin)
		{
			return true;
		}
		if (method == MethodName.RequestSpin)
		{
			return true;
		}
		if (method == MethodName.GetStatusText)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.ReadConfig)
		{
			return true;
		}
		if (method == MethodName.ReadPacketList)
		{
			return true;
		}
		if (method == MethodName.FinishSpin)
		{
			return true;
		}
		if (method == MethodName.CreateLevelClearReward)
		{
			return true;
		}
		if (method == MethodName.SpawnPacketReward)
		{
			return true;
		}
		if (method == MethodName.CreateRewardVelocity)
		{
			return true;
		}
		if (method == MethodName.GetRewardSpawnPosition)
		{
			return true;
		}
		if (method == MethodName.GetShortPacketName)
		{
			return true;
		}
		if (method == MethodName.FormatNumber)
		{
			return true;
		}
		if (method == MethodName.FormatText)
		{
			return true;
		}
		if (method == MethodName.SetMessage)
		{
			return true;
		}
		if (method == MethodName.SubscribeSunAccount)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeSunAccount)
		{
			return true;
		}
		if (method == MethodName.UpdateControl)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.slotMachineControl)
		{
			slotMachineControl = VariantUtils.ConvertTo<SlotMachineControl>(in value);
			return true;
		}
		if (name == PropertyName.open)
		{
			open = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.running)
		{
			running = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.autoStart)
		{
			autoStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.cooldown)
		{
			cooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cooldownTimer)
		{
			cooldownTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spinDuration)
		{
			spinDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetAliveTime)
		{
			packetAliveTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spinNum)
		{
			spinNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.clearSunReward)
		{
			clearSunReward = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.closedText)
		{
			closedText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.waitText)
		{
			waitText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.hostOnlyText)
		{
			hostOnlyText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spinningText)
		{
			spinningText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.missText)
		{
			missText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spinText)
		{
			spinText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spinCostTextTemplate)
		{
			spinCostTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.insufficientSunTextTemplate)
		{
			insufficientSunTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.cooldownTextTemplate)
		{
			cooldownTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.rewardCountTextTemplate)
		{
			rewardCountTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.sunRewardTextTemplate)
		{
			sunRewardTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.brainRewardTextTemplate)
		{
			brainRewardTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.diamondRewardTextTemplate)
		{
			diamondRewardTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.coinRewardTextTemplate)
		{
			coinRewardTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._spinInProgress)
		{
			_spinInProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._clearRewardCreated)
		{
			_clearRewardCreated = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._message)
		{
			_message = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._messageTimer)
		{
			_messageTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			_sunFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSun>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.slotMachineControl)
		{
			value = VariantUtils.CreateFrom(in slotMachineControl);
			return true;
		}
		if (name == PropertyName.open)
		{
			value = VariantUtils.CreateFrom(in open);
			return true;
		}
		if (name == PropertyName.running)
		{
			value = VariantUtils.CreateFrom(in running);
			return true;
		}
		if (name == PropertyName.autoStart)
		{
			value = VariantUtils.CreateFrom(in autoStart);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.cooldown)
		{
			value = VariantUtils.CreateFrom(in cooldown);
			return true;
		}
		if (name == PropertyName.cooldownTimer)
		{
			value = VariantUtils.CreateFrom(in cooldownTimer);
			return true;
		}
		if (name == PropertyName.spinDuration)
		{
			value = VariantUtils.CreateFrom(in spinDuration);
			return true;
		}
		if (name == PropertyName.packetAliveTime)
		{
			value = VariantUtils.CreateFrom(in packetAliveTime);
			return true;
		}
		if (name == PropertyName.spinNum)
		{
			value = VariantUtils.CreateFrom(in spinNum);
			return true;
		}
		if (name == PropertyName.clearSunReward)
		{
			value = VariantUtils.CreateFrom(in clearSunReward);
			return true;
		}
		if (name == PropertyName.closedText)
		{
			value = VariantUtils.CreateFrom(in closedText);
			return true;
		}
		if (name == PropertyName.waitText)
		{
			value = VariantUtils.CreateFrom(in waitText);
			return true;
		}
		if (name == PropertyName.hostOnlyText)
		{
			value = VariantUtils.CreateFrom(in hostOnlyText);
			return true;
		}
		if (name == PropertyName.spinningText)
		{
			value = VariantUtils.CreateFrom(in spinningText);
			return true;
		}
		if (name == PropertyName.missText)
		{
			value = VariantUtils.CreateFrom(in missText);
			return true;
		}
		if (name == PropertyName.spinText)
		{
			value = VariantUtils.CreateFrom(in spinText);
			return true;
		}
		if (name == PropertyName.spinCostTextTemplate)
		{
			value = VariantUtils.CreateFrom(in spinCostTextTemplate);
			return true;
		}
		if (name == PropertyName.insufficientSunTextTemplate)
		{
			value = VariantUtils.CreateFrom(in insufficientSunTextTemplate);
			return true;
		}
		if (name == PropertyName.cooldownTextTemplate)
		{
			value = VariantUtils.CreateFrom(in cooldownTextTemplate);
			return true;
		}
		if (name == PropertyName.rewardCountTextTemplate)
		{
			value = VariantUtils.CreateFrom(in rewardCountTextTemplate);
			return true;
		}
		if (name == PropertyName.sunRewardTextTemplate)
		{
			value = VariantUtils.CreateFrom(in sunRewardTextTemplate);
			return true;
		}
		if (name == PropertyName.brainRewardTextTemplate)
		{
			value = VariantUtils.CreateFrom(in brainRewardTextTemplate);
			return true;
		}
		if (name == PropertyName.diamondRewardTextTemplate)
		{
			value = VariantUtils.CreateFrom(in diamondRewardTextTemplate);
			return true;
		}
		if (name == PropertyName.coinRewardTextTemplate)
		{
			value = VariantUtils.CreateFrom(in coinRewardTextTemplate);
			return true;
		}
		if (name == PropertyName._spinInProgress)
		{
			value = VariantUtils.CreateFrom(in _spinInProgress);
			return true;
		}
		if (name == PropertyName._clearRewardCreated)
		{
			value = VariantUtils.CreateFrom(in _clearRewardCreated);
			return true;
		}
		if (name == PropertyName._message)
		{
			value = VariantUtils.CreateFrom(in _message);
			return true;
		}
		if (name == PropertyName._messageTimer)
		{
			value = VariantUtils.CreateFrom(in _messageTimer);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			value = VariantUtils.CreateFrom(in _sunFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.slotMachineControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.open, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.running, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.cooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.cooldownTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spinDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetAliveTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.spinNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.clearSunReward, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.closedText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.waitText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.hostOnlyText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.spinningText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.missText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.spinText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.spinCostTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.insufficientSunTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.cooldownTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.rewardCountTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.sunRewardTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.brainRewardTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.diamondRewardTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.coinRewardTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spinInProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._clearRewardCreated, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._message, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._messageTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.slotMachineControl, Variant.From(in slotMachineControl));
		info.AddProperty(PropertyName.open, Variant.From(in open));
		info.AddProperty(PropertyName.running, Variant.From(in running));
		info.AddProperty(PropertyName.autoStart, Variant.From(in autoStart));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.cooldown, Variant.From(in cooldown));
		info.AddProperty(PropertyName.cooldownTimer, Variant.From(in cooldownTimer));
		info.AddProperty(PropertyName.spinDuration, Variant.From(in spinDuration));
		info.AddProperty(PropertyName.packetAliveTime, Variant.From(in packetAliveTime));
		info.AddProperty(PropertyName.spinNum, Variant.From(in spinNum));
		info.AddProperty(PropertyName.clearSunReward, Variant.From(in clearSunReward));
		info.AddProperty(PropertyName.closedText, Variant.From(in closedText));
		info.AddProperty(PropertyName.waitText, Variant.From(in waitText));
		info.AddProperty(PropertyName.hostOnlyText, Variant.From(in hostOnlyText));
		info.AddProperty(PropertyName.spinningText, Variant.From(in spinningText));
		info.AddProperty(PropertyName.missText, Variant.From(in missText));
		info.AddProperty(PropertyName.spinText, Variant.From(in spinText));
		info.AddProperty(PropertyName.spinCostTextTemplate, Variant.From(in spinCostTextTemplate));
		info.AddProperty(PropertyName.insufficientSunTextTemplate, Variant.From(in insufficientSunTextTemplate));
		info.AddProperty(PropertyName.cooldownTextTemplate, Variant.From(in cooldownTextTemplate));
		info.AddProperty(PropertyName.rewardCountTextTemplate, Variant.From(in rewardCountTextTemplate));
		info.AddProperty(PropertyName.sunRewardTextTemplate, Variant.From(in sunRewardTextTemplate));
		info.AddProperty(PropertyName.brainRewardTextTemplate, Variant.From(in brainRewardTextTemplate));
		info.AddProperty(PropertyName.diamondRewardTextTemplate, Variant.From(in diamondRewardTextTemplate));
		info.AddProperty(PropertyName.coinRewardTextTemplate, Variant.From(in coinRewardTextTemplate));
		info.AddProperty(PropertyName._spinInProgress, Variant.From(in _spinInProgress));
		info.AddProperty(PropertyName._clearRewardCreated, Variant.From(in _clearRewardCreated));
		info.AddProperty(PropertyName._message, Variant.From(in _message));
		info.AddProperty(PropertyName._messageTimer, Variant.From(in _messageTimer));
		info.AddProperty(PropertyName._sunFeature, Variant.From(in _sunFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.slotMachineControl, out var value))
		{
			slotMachineControl = value.As<SlotMachineControl>();
		}
		if (info.TryGetProperty(PropertyName.open, out var value2))
		{
			open = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.running, out var value3))
		{
			running = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.autoStart, out var value4))
		{
			autoStart = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value5))
		{
			cost = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.cooldown, out var value6))
		{
			cooldown = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cooldownTimer, out var value7))
		{
			cooldownTimer = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spinDuration, out var value8))
		{
			spinDuration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetAliveTime, out var value9))
		{
			packetAliveTime = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spinNum, out var value10))
		{
			spinNum = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.clearSunReward, out var value11))
		{
			clearSunReward = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.closedText, out var value12))
		{
			closedText = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.waitText, out var value13))
		{
			waitText = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.hostOnlyText, out var value14))
		{
			hostOnlyText = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spinningText, out var value15))
		{
			spinningText = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.missText, out var value16))
		{
			missText = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spinText, out var value17))
		{
			spinText = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spinCostTextTemplate, out var value18))
		{
			spinCostTextTemplate = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.insufficientSunTextTemplate, out var value19))
		{
			insufficientSunTextTemplate = value19.As<string>();
		}
		if (info.TryGetProperty(PropertyName.cooldownTextTemplate, out var value20))
		{
			cooldownTextTemplate = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.rewardCountTextTemplate, out var value21))
		{
			rewardCountTextTemplate = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName.sunRewardTextTemplate, out var value22))
		{
			sunRewardTextTemplate = value22.As<string>();
		}
		if (info.TryGetProperty(PropertyName.brainRewardTextTemplate, out var value23))
		{
			brainRewardTextTemplate = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.diamondRewardTextTemplate, out var value24))
		{
			diamondRewardTextTemplate = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName.coinRewardTextTemplate, out var value25))
		{
			coinRewardTextTemplate = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName._spinInProgress, out var value26))
		{
			_spinInProgress = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._clearRewardCreated, out var value27))
		{
			_clearRewardCreated = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._message, out var value28))
		{
			_message = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName._messageTimer, out var value29))
		{
			_messageTimer = value29.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunFeature, out var value30))
		{
			_sunFeature = value30.As<TowerDefenseBattleFeatureSun>();
		}
	}
}
