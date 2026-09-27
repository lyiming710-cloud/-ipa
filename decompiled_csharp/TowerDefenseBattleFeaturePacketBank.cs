using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/TowerDefenseBattleFeaturePacketBank.cs")]
public class TowerDefenseBattleFeaturePacketBank : TowerDefenseBattleFeature
{
	public delegate void ChooseOverEventHandler();

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public static readonly StringName EmitChooseOver = "EmitChooseOver";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName PacketBankInit = "PacketBankInit";

		public static readonly StringName SetPacketBankData = "SetPacketBankData";

		public static readonly StringName PacketChoose = "PacketChoose";

		public static readonly StringName FindSelectedPacket = "FindSelectedPacket";

		public static readonly StringName BindVirtualizedPacket = "BindVirtualizedPacket";

		public static readonly StringName UnbindVirtualizedPacket = "UnbindVirtualizedPacket";

		public static readonly StringName IsPacketAvailableInCurrentPacketBank = "IsPacketAvailableInCurrentPacketBank";

		public static readonly StringName RebuildPacketAvailabilityIndex = "RebuildPacketAvailabilityIndex";

		public static readonly StringName PacketListChoose = "PacketListChoose";

		public static readonly StringName PacketChooseFromName = "PacketChooseFromName";

		public static readonly StringName PacketAlive = "PacketAlive";

		public static readonly StringName RockButtonPressed = "RockButtonPressed";

		public static readonly StringName _EmitChooseOver = "_EmitChooseOver";

		public static readonly StringName PacketClear = "PacketClear";

		public static readonly StringName CategoryChoose = "CategoryChoose";

		public static readonly StringName LoveChange = "LoveChange";

		public static readonly StringName ReSelectButtonPressed = "ReSelectButtonPressed";

		public static readonly StringName ViewButtonPressed = "ViewButtonPressed";

		public static readonly StringName LoadPacketGroup = "LoadPacketGroup";

		public static readonly StringName SavePacketGroup = "SavePacketGroup";

		public static readonly StringName ShopButtonPressed = "ShopButtonPressed";

		public static readonly StringName AlmanacButtonPressed = "AlmanacButtonPressed";

		public static readonly StringName _ConnectPacketBankSignals = "_ConnectPacketBankSignals";

		public static readonly StringName _OnCategoryChoose = "_OnCategoryChoose";

		public static readonly StringName _InitGuiTopButtons = "_InitGuiTopButtons";

		public static readonly StringName _DisconnectPacketBankSignals = "_DisconnectPacketBankSignals";

		public static readonly StringName _OnGlobalFeatureChanged = "_OnGlobalFeatureChanged";

		public static readonly StringName _OnPacketBankAnimationFinished = "_OnPacketBankAnimationFinished";

		public static readonly StringName _OnUISwitched = "_OnUISwitched";

		public static readonly StringName _UpdateGuiTopButtonVisibility = "_UpdateGuiTopButtonVisibility";

		public static readonly StringName _HidePacketBankLegacyButtons = "_HidePacketBankLegacyButtons";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName packetBank = "packetBank";

		public static readonly StringName config = "config";

		public static readonly StringName skipPacketChoose = "skipPacketChoose";

		public static readonly StringName packetBankData = "packetBankData";

		public static readonly StringName packetList = "packetList";

		public static readonly StringName currentCategory = "currentCategory";

		public static readonly StringName currentIndex = "currentIndex";

		public static readonly StringName _currentPacket = "_currentPacket";

		public static readonly StringName _categoryGeneration = "_categoryGeneration";

		public static readonly StringName _guiTopShopButton = "_guiTopShopButton";

		public static readonly StringName _guiTopAlmanacButton = "_guiTopAlmanacButton";

		public static readonly StringName _packetBankEntered = "_packetBankEntered";

		public static readonly StringName _signalsConnected = "_signalsConnected";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _towerDefenseIngamePacketBank;

	public TowerDefenseInGamePacketBank packetBank;

	public TowerDefenseLevelPacketBankConfig config;

	public bool skipPacketChoose;

	public TowerDefensePacketBankData packetBankData;

	public Array<TowerDefenseInGamePacketShow> packetList = new Array<TowerDefenseInGamePacketShow>();

	public string currentCategory = "";

	public int currentIndex = -1;

	private TowerDefenseInGamePacketShow _currentPacket;

	private int _categoryGeneration;

	private readonly HashSet<string> _availablePacketNames = new HashSet<string>(StringComparer.Ordinal);

	private NinePatchButtonBase _guiTopShopButton;

	private NinePatchButtonBase _guiTopAlmanacButton;

	private bool _packetBankEntered;

	private bool _signalsConnected;

	private static PackedScene TOWER_DEFENSE_INGAME_PACKET_BANK => _towerDefenseIngamePacketBank ?? (_towerDefenseIngamePacketBank = GD.Load<PackedScene>("uid://iydc3rp06xgs"));

	public event ChooseOverEventHandler OnChooseOver;

	public void EmitChooseOver()
	{
		OnChooseOver?.Invoke();
	}

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelPacketBankConfig();
		config.Init(data);
		packetBank = TOWER_DEFENSE_INGAME_PACKET_BANK.Instantiate<TowerDefenseInGamePacketBank>(PackedScene.GenEditState.Disabled);
		packetBank.packetBankFeature = this;
		control.AddUI(packetBank, 3);
	}

	public override void Destroy()
	{
		_categoryGeneration++;
		_DisconnectPacketBankSignals();
		OnChooseOver = null;
		if (GodotObject.IsInstanceValid(packetBank))
		{
			packetBank.DisposeOwnedPackets();
			packetBank.QueueFree();
		}
		packetBank = null;
		packetList.Clear();
		_currentPacket = null;
		_availablePacketNames.Clear();
		packetBankData = null;
		config = null;
		_guiTopShopButton = null;
		_guiTopAlmanacButton = null;
		base.Destroy();
	}

	public override Task GameInit()
	{
		TowerDefenseBattleFeatureSeedBank towerDefenseBattleFeatureSeedBank = GetFeature("SeedBank") as TowerDefenseBattleFeatureSeedBank;
		ResourceManager.Instance.RequireFullGameplayResourcesReady("TowerDefenseBattleFeaturePacketBank");
		packetBank.seedBank = towerDefenseBattleFeatureSeedBank.seedBank;
		_ConnectPacketBankSignals();
		PacketBankInit();
		if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
		{
			CategoryChoose("Zombie");
		}
		else
		{
			CategoryChoose("White", reFresh: true);
		}
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		TowerDefenseBattleFeatureSeedBank towerDefenseBattleFeatureSeedBank = GetFeature("SeedBank") as TowerDefenseBattleFeatureSeedBank;
		ResourceManager.Instance.RequireFullGameplayResourcesReady("TowerDefenseBattleFeaturePacketBank");
		packetBank.seedBank = towerDefenseBattleFeatureSeedBank.seedBank;
		_ConnectPacketBankSignals();
		skipPacketChoose = true;
		if (towerDefenseBattleFeatureSeedBank.config.method == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE)
		{
			TowerDefensePacketBankData towerDefensePacketBankData = ((!CommandManager.Instance.debugPacketOpenAll) ? TowerDefenseManager.GetPacketBankData(config.packetBankType) : TowerDefenseManager.GetPacketBankData("Total"));
			SetPacketBankData(towerDefensePacketBankData);
		}
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		if (GetFeature("SeedBank") is TowerDefenseBattleFeatureSeedBank towerDefenseBattleFeatureSeedBank)
		{
			switch (towerDefenseBattleFeatureSeedBank.config.method)
			{
			case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE:
				skipPacketChoose = true;
				break;
			case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
				if (GodotObject.IsInstanceValid(packetBankData))
				{
					Array<string> unlockPacket = packetBankData.GetUnlockPacket();
					skipPacketChoose = unlockPacket.Count <= TowerDefenseManager.Instance.seedbankPacketMax;
				}
				else
				{
					skipPacketChoose = false;
				}
				break;
			case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
				skipPacketChoose = true;
				break;
			}
		}
		return Task.CompletedTask;
	}

	public override Task GameEntry()
	{
		if (!skipPacketChoose && currentCategory != "")
		{
			CategoryChoose(currentCategory, reFresh: true);
		}
		return Task.CompletedTask;
	}

	public void PacketBankInit()
	{
		TowerDefenseBattleFeatureSeedBank towerDefenseBattleFeatureSeedBank = GetFeature("SeedBank") as TowerDefenseBattleFeatureSeedBank;
		TowerDefenseInGameSeedBank seedBank = towerDefenseBattleFeatureSeedBank.seedBank;
		packetBank.seedBank = seedBank;
		bool flag = false;
		switch (towerDefenseBattleFeatureSeedBank.config.method)
		{
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE:
			skipPacketChoose = true;
			if (Global.IsMultiplayerMode)
			{
				flag = true;
			}
			break;
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE:
		{
			TowerDefensePacketBankData towerDefensePacketBankData = ((!CommandManager.Instance.debugPacketOpenAll) ? TowerDefenseManager.GetPacketBankData(config.packetBankType) : TowerDefenseManager.GetPacketBankData("Total"));
			if (!GodotObject.IsInstanceValid(towerDefensePacketBankData))
			{
				control.RejectLevelConfiguration("选卡库不可用：" + config.packetBankType);
				return;
			}
			SetPacketBankData(towerDefensePacketBankData);
			Array<string> unlockPacket = towerDefensePacketBankData.GetUnlockPacket();
			skipPacketChoose = unlockPacket.Count <= TowerDefenseManager.Instance.seedbankPacketMax;
			foreach (TowerDefenseLevelPacketConfig packet3 in towerDefenseBattleFeatureSeedBank.config.packetList)
			{
				if (!GodotObject.IsInstanceValid(packet3.overrideVal))
				{
					PacketChooseFromName(packet3.packetName, @lock: true);
					continue;
				}
				TowerDefensePacketConfig packet2 = packet3.GetPacket();
				TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = seedBank.AddPacket(packet2);
				if (towerDefenseInGamePacketShow != null)
				{
					towerDefenseInGamePacketShow.@lock = true;
				}
			}
			if (!CommandManager.Instance.debugPacketSelect && skipPacketChoose)
			{
				foreach (string item in unlockPacket)
				{
					TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(item);
					seedBank.AddPacket(packetConfigReadOnly);
				}
				seedBank.ReadyPackets();
				if (Global.IsMultiplayerMode)
				{
					flag = true;
				}
			}
			if (CommandManager.Instance.debugPacketSelect)
			{
				skipPacketChoose = false;
			}
			break;
		}
		case TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET:
			skipPacketChoose = true;
			if (control.isInit && !control.hasProgress)
			{
				seedBank.DeleteAllPacket();
				foreach (TowerDefenseLevelPacketConfig packet4 in towerDefenseBattleFeatureSeedBank.config.packetList)
				{
					TowerDefensePacketConfig packet = packet4.GetPacket();
					seedBank.AddPacket(packet);
				}
				seedBank.ReadyPackets();
			}
			if (Global.IsMultiplayerMode)
			{
				flag = true;
			}
			if (CommandManager.Instance.debugPacketSelect)
			{
				skipPacketChoose = false;
			}
			break;
		}
		if (flag)
		{
			_EmitChooseOver();
		}
	}

	public void SetPacketBankData(TowerDefensePacketBankData _data)
	{
		if (!GodotObject.IsInstanceValid(_data))
		{
			control.RejectLevelConfiguration("选卡库不可用：" + config.packetBankType);
			return;
		}
		TowerDefenseBattleFeatureSeedBank towerDefenseBattleFeatureSeedBank = GetFeature("SeedBank") as TowerDefenseBattleFeatureSeedBank;
		packetBankData = ((config.packetBankType == "GeneralPlant" && towerDefenseBattleFeatureSeedBank != null && towerDefenseBattleFeatureSeedBank.config.method == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE) ? XWModContentCatalog.WithPlants(_data) : _data);
		Control nodeOrNull = packetBank.cardSort.GetNodeOrNull<Control>("CardMod");
		if (nodeOrNull != null)
		{
			nodeOrNull.Visible = packetBankData.category.ContainsKey("ModPlants");
		}
		RebuildPacketAvailabilityIndex();
		PacketClear();
		if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
		{
			CategoryChoose("Zombie");
		}
		else
		{
			CategoryChoose("White", reFresh: true);
		}
	}

	public void PacketChoose(TowerDefenseInGamePacketShow packet)
	{
		if (!GodotObject.IsInstanceValid(packet) || !GodotObject.IsInstanceValid(packet.config) || !GodotObject.IsInstanceValid(packetBank?.seedBank))
		{
			return;
		}
		string saveKey = packet.config.saveKey;
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = FindSelectedPacket(saveKey);
		if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
		{
			if (towerDefenseInGamePacketShow.@lock)
			{
				packet.select = false;
				return;
			}
			packetBank.CancelPacketAnimation(towerDefenseInGamePacketShow);
			packetBank.seedBank.DeletePacket(towerDefenseInGamePacketShow);
			if (!packet.alive)
			{
				PacketAlive(saveKey);
			}
			if (packet == _currentPacket)
			{
				_currentPacket = null;
				currentIndex = -1;
			}
			return;
		}
		if (!packet.alive || !packetBank.seedBank.CanAddPacket())
		{
			packet.Reset();
			return;
		}
		Vector2 cameraPos = packetBank.GetCameraPos();
		packetBank.CreateAnime(packet.config, cameraPos + packet.GlobalPosition);
		packet.alive = false;
		if (packet != _currentPacket)
		{
			if (GodotObject.IsInstanceValid(_currentPacket))
			{
				_currentPacket.Reset();
			}
			_currentPacket = packet;
			currentIndex = packetList.IndexOf(packet);
		}
	}

	private TowerDefenseInGamePacketShow FindSelectedPacket(string packetName)
	{
		foreach (TowerDefenseInGamePacketShow packet in packetBank.seedBank.packetList)
		{
			if (GodotObject.IsInstanceValid(packet) && GodotObject.IsInstanceValid(packet.config) && ((packet.originalSaveKey != "") ? packet.originalSaveKey : packet.config.saveKey) == packetName)
			{
				return packet;
			}
		}
		return null;
	}

	public void BindVirtualizedPacket(TowerDefenseInGamePacketShow packet, TowerDefensePacketConfig packetConfig)
	{
		if (GodotObject.IsInstanceValid(packet) && GodotObject.IsInstanceValid(packetConfig))
		{
			packet.enforceRuntimeAvailabilityOnPress = false;
			packet.allowPressWhenUnavailable = true;
			packet.Init(packetConfig);
			packet.showLove = true;
			packet.OnLoveChange += LoveChange;
			packet.OnPressed += PacketChoose;
			packet.alive = !GodotObject.IsInstanceValid(packetBank?.seedBank) || !packetBank.seedBank.HasPacket(packetConfig.saveKey);
			packetList.Add(packet);
		}
	}

	public void UnbindVirtualizedPacket(TowerDefenseInGamePacketShow packet)
	{
		if (GodotObject.IsInstanceValid(packet))
		{
			packet.OnPressed -= PacketChoose;
			packet.OnLoveChange -= LoveChange;
			packetList.Remove(packet);
			if (packet == _currentPacket)
			{
				_currentPacket = null;
				currentIndex = -1;
			}
		}
	}

	private bool IsPacketAvailableInCurrentPacketBank(string packetName)
	{
		if (!string.IsNullOrEmpty(packetName))
		{
			return _availablePacketNames.Contains(packetName);
		}
		return false;
	}

	private void RebuildPacketAvailabilityIndex()
	{
		_availablePacketNames.Clear();
		if (!GodotObject.IsInstanceValid(packetBankData))
		{
			return;
		}
		foreach (Variant value in packetBankData.category.Values)
		{
			foreach (Variant item in (Godot.Collections.Array)value)
			{
				string text = item.AsString();
				if (text != "")
				{
					_availablePacketNames.Add(text);
				}
			}
		}
	}

	public void PacketListChoose(Godot.Collections.Array _packetList)
	{
		AudioManager.Instance.AudioPlay("PacketPick");
		packetBank.ClearAnimeNode();
		foreach (Variant _packet in _packetList)
		{
			string text = _packet.AsString();
			if (!IsPacketAvailableInCurrentPacketBank(text))
			{
				continue;
			}
			TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(text);
			if (!GodotObject.IsInstanceValid(packetConfigReadOnly) || !packetConfigReadOnly.Unlock() || packetBank.seedBank.HasPacket(text) || packetBank.seedBank == null || !packetBank.seedBank.CanAddPacket())
			{
				continue;
			}
			bool flag = false;
			foreach (TowerDefenseInGamePacketShow packet in packetList)
			{
				if (packet.config.saveKey == text && packet.alive)
				{
					flag = true;
					PacketChoose(packet);
					break;
				}
			}
			if (!flag)
			{
				Vector2 cameraPos = packetBank.GetCameraPos();
				packetBank.CreateAnime(packetConfigReadOnly, cameraPos + new Vector2(300f, 260f));
			}
		}
	}

	public void PacketChooseFromName(string packetName, bool @lock = false)
	{
		TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(packetName);
		if (!GodotObject.IsInstanceValid(packetConfigReadOnly) || packetBank.seedBank.HasPacket(packetName))
		{
			return;
		}
		if (packetBank.seedBank.CanAddPacket())
		{
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = packetBank.seedBank.AddPacket(packetConfigReadOnly);
			if (towerDefenseInGamePacketShow != null)
			{
				towerDefenseInGamePacketShow.@lock = @lock;
			}
		}
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			if (packet.config.saveKey == packetName && packet.alive)
			{
				packet.alive = false;
			}
		}
	}

	public void PacketAlive(string packetName)
	{
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			if (packet.config.saveKey == packetName)
			{
				packet.alive = true;
				packet.Reset();
				break;
			}
		}
	}

	public void RockButtonPressed()
	{
		if (GodotObject.IsInstanceValid(packetBank.seedBank) && (packetBank.seedBank.packetNum > 0 || !(Global.Instance.enterLevelMode != "LevelTest")))
		{
			_EmitChooseOver();
		}
	}

	public void _EmitChooseOver()
	{
		RunLifetimeTask(EmitChooseOverAsync, "_EmitChooseOver");
	}

	private async Task EmitChooseOverAsync()
	{
		EmitChooseOver();
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseInGamePacketShow packet in packetBank.seedBank.packetList)
		{
			string text = ((packet.originalSaveKey != "") ? packet.originalSaveKey : packet.config.saveKey);
			array.Add(text);
		}
		if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
		{
			GameSaveManager.Instance.SetKeyValue("ZombiePacketReSlect", array);
		}
		else
		{
			GameSaveManager.Instance.SetKeyValue("PacketReSlect", array);
		}
		GameSaveManager.Instance.Save();
		await ToSignal(control.GetTree().CreateTimer(1.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (IsLifetimeActive && GodotObject.IsInstanceValid(packetBank))
		{
			PacketClear();
		}
	}

	public void PacketClear()
	{
		packetBank.ClearPackets();
		packetList.Clear();
		_currentPacket = null;
		currentIndex = -1;
	}

	public void CategoryChoose(string _category, bool reFresh = false)
	{
		RunLifetimeTask(() => CategoryChooseAsync(_category, reFresh), "CategoryChoose");
	}

	private Task CategoryChooseAsync(string _category, bool reFresh)
	{
		if (currentCategory == _category && !reFresh)
		{
			return Task.CompletedTask;
		}
		if (!GodotObject.IsInstanceValid(packetBankData))
		{
			return Task.CompletedTask;
		}
		_categoryGeneration++;
		int categoryGeneration = _categoryGeneration;
		currentCategory = _category;
		if (!packetBankData.category.ContainsKey(_category))
		{
			PacketClear();
			return Task.CompletedTask;
		}
		Godot.Collections.Array array = (Godot.Collections.Array)packetBankData.category[_category];
		Godot.Collections.Array array2 = new Godot.Collections.Array();
		Godot.Collections.Array array3 = new Godot.Collections.Array();
		foreach (Variant item in array)
		{
			string text = item.AsString();
			if (XWModPlayerProgressService.GetPacketState(text).GetValueOrDefault("Love", false).AsBool())
			{
				array2.Add(text);
			}
			else
			{
				array3.Add(text);
			}
		}
		foreach (Variant item2 in array3)
		{
			array2.Add(item2);
		}
		ResourceManager.Instance.RequireFullGameplayResourcesReady("CategoryChooseAsync");
		if (!IsLifetimeActive || _categoryGeneration != categoryGeneration || !GodotObject.IsInstanceValid(packetBank))
		{
			return Task.CompletedTask;
		}
		PacketClear();
		List<TowerDefensePacketConfig> list = new List<TowerDefensePacketConfig>(array2.Count);
		foreach (Variant item3 in array2)
		{
			if (_categoryGeneration != categoryGeneration)
			{
				return Task.CompletedTask;
			}
			TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(item3.AsString());
			if (GodotObject.IsInstanceValid(packetConfigReadOnly) && packetConfigReadOnly.Unlock())
			{
				list.Add(packetConfigReadOnly);
			}
		}
		if (!IsLifetimeActive || _categoryGeneration != categoryGeneration || !GodotObject.IsInstanceValid(packetBank))
		{
			return Task.CompletedTask;
		}
		packetBank.SetVirtualizedPackets(list);
		return Task.CompletedTask;
	}

	public void LoveChange(TowerDefenseInGamePacketShow packet)
	{
		CategoryChoose(currentCategory, reFresh: true);
	}

	public void ReSelectButtonPressed()
	{
		Godot.Collections.Array array = ((!TowerDefenseManager.Instance.IsIZMMode() && !TowerDefenseManager.Instance.IsIZM2Mode()) ? GameSaveManager.Instance.GetKeyValue("PacketReSlect").AsGodotArray() : GameSaveManager.Instance.GetKeyValue("ZombiePacketReSlect").AsGodotArray());
		packetBank.seedBank.DeleteAllPacket();
		PacketListChoose(array);
	}

	public void ViewButtonPressed()
	{
		control.ViewMap();
	}

	public void LoadPacketGroup(int id)
	{
		Godot.Collections.Array array = ((!TowerDefenseManager.Instance.IsIZMMode() && !TowerDefenseManager.Instance.IsIZM2Mode()) ? GameSaveManager.Instance.GetKeyValue($"PacketGroup{id}").AsGodotArray() : GameSaveManager.Instance.GetKeyValue($"ZombiePacketGroup{id}").AsGodotArray());
		packetBank.seedBank.DeleteAllPacket();
		PacketListChoose(array);
	}

	public void SavePacketGroup(int id)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseInGamePacketShow packet in packetBank.seedBank.packetList)
		{
			string text = ((packet.originalSaveKey != "") ? packet.originalSaveKey : packet.config.saveKey);
			array.Add(text);
		}
		if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
		{
			GameSaveManager.Instance.SetKeyValue($"ZombiePacketGroup{id}", array);
		}
		else
		{
			GameSaveManager.Instance.SetKeyValue($"PacketGroup{id}", array);
		}
		GameSaveManager.Instance.Save();
	}

	public void ShopButtonPressed()
	{
		GlobalFeatureManager instance = GlobalFeatureManager.Instance;
		if (instance != null && instance.IsUnlocked("Shop"))
		{
			DialogManager.Instance.DialogCreate("Shop");
		}
	}

	public void AlmanacButtonPressed()
	{
		DialogManager.Instance.DialogCreate("Almanac");
	}

	private void _ConnectPacketBankSignals()
	{
		if (_signalsConnected || !GodotObject.IsInstanceValid(packetBank))
		{
			return;
		}
		foreach (Node child in packetBank.cardSort.GetNode("VBoxContainer").GetChildren())
		{
			if (child is PacketCategoryButton packetCategoryButton)
			{
				packetCategoryButton.OnChoose += _OnCategoryChoose;
			}
		}
		(packetBank.cardItem as PacketCategoryButton).OnChoose += _OnCategoryChoose;
		(packetBank.cardGraveStone as PacketCategoryButton).OnChoose += _OnCategoryChoose;
		(packetBank.cardZombie as PacketCategoryButton).OnChoose += _OnCategoryChoose;
		packetBank.cardSort.GetNode<PacketCategoryButton>("CardMod").OnChoose += _OnCategoryChoose;
		(packetBank.translate.GetNode("RockButton") as NinePatchButtonBase).OnPressed += RockButtonPressed;
		(packetBank.translate.GetNode("ReSelectButton") as NinePatchButtonBase).OnPressed += ReSelectButtonPressed;
		(packetBank.translate.GetNode("ViewButton") as NinePatchButtonBase).OnPressed += ViewButtonPressed;
		Node node = packetBank.translate.GetNode("PacketGroup");
		for (int i = 1; i < 7; i++)
		{
			string text = ((i == 1) ? "PacketGroupButton" : $"PacketGroupButton{i}");
			PacketGroupButton node2 = node.GetNode<PacketGroupButton>(text);
			node2.OnLoadGroup += LoadPacketGroup;
			node2.OnSaveGroup += SavePacketGroup;
		}
		(packetBank.GetNode("ShopButton") as NinePatchButtonBase).OnPressed += ShopButtonPressed;
		(packetBank.GetNode("AlmanacButton") as NinePatchButtonBase).OnPressed += AlmanacButtonPressed;
		_signalsConnected = true;
		if (GlobalFeatureManager.Instance != null)
		{
			GlobalFeatureManager.Instance.FeatureChanged += _OnGlobalFeatureChanged;
		}
		_InitGuiTopButtons();
	}

	private void _OnCategoryChoose(string category)
	{
		CategoryChoose(category);
	}

	private void _InitGuiTopButtons()
	{
		packetBank.packetBankAnimationPlayer.AnimationFinished += _OnPacketBankAnimationFinished;
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched += _OnUISwitched;
		}
		Node nodeOrNull = control.GetNodeOrNull("GUITop");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			_guiTopShopButton = nodeOrNull.GetNodeOrNull<NinePatchButtonBase>("ShopButton");
			_guiTopAlmanacButton = nodeOrNull.GetNodeOrNull<NinePatchButtonBase>("AlmanacButton");
			if (GodotObject.IsInstanceValid(_guiTopShopButton))
			{
				_guiTopShopButton.Visible = false;
				_guiTopShopButton.OnPressed += ShopButtonPressed;
			}
			if (GodotObject.IsInstanceValid(_guiTopAlmanacButton))
			{
				_guiTopAlmanacButton.Visible = false;
				_guiTopAlmanacButton.OnPressed += AlmanacButtonPressed;
			}
		}
	}

	private void _DisconnectPacketBankSignals()
	{
		if (!_signalsConnected)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(packetBank))
		{
			foreach (Node child in packetBank.cardSort.GetNode("VBoxContainer").GetChildren())
			{
				if (child is PacketCategoryButton packetCategoryButton)
				{
					packetCategoryButton.OnChoose -= _OnCategoryChoose;
				}
			}
			(packetBank.cardItem as PacketCategoryButton).OnChoose -= _OnCategoryChoose;
			(packetBank.cardGraveStone as PacketCategoryButton).OnChoose -= _OnCategoryChoose;
			(packetBank.cardZombie as PacketCategoryButton).OnChoose -= _OnCategoryChoose;
			packetBank.cardSort.GetNode<PacketCategoryButton>("CardMod").OnChoose -= _OnCategoryChoose;
			(packetBank.translate.GetNode("RockButton") as NinePatchButtonBase).OnPressed -= RockButtonPressed;
			(packetBank.translate.GetNode("ReSelectButton") as NinePatchButtonBase).OnPressed -= ReSelectButtonPressed;
			(packetBank.translate.GetNode("ViewButton") as NinePatchButtonBase).OnPressed -= ViewButtonPressed;
			Node node = packetBank.translate.GetNode("PacketGroup");
			for (int i = 1; i < 7; i++)
			{
				string text = ((i == 1) ? "PacketGroupButton" : $"PacketGroupButton{i}");
				PacketGroupButton node2 = node.GetNode<PacketGroupButton>(text);
				node2.OnLoadGroup -= LoadPacketGroup;
				node2.OnSaveGroup -= SavePacketGroup;
			}
			(packetBank.GetNode("ShopButton") as NinePatchButtonBase).OnPressed -= ShopButtonPressed;
			(packetBank.GetNode("AlmanacButton") as NinePatchButtonBase).OnPressed -= AlmanacButtonPressed;
			if (GodotObject.IsInstanceValid(packetBank.packetBankAnimationPlayer))
			{
				packetBank.packetBankAnimationPlayer.AnimationFinished -= _OnPacketBankAnimationFinished;
			}
		}
		if (GodotObject.IsInstanceValid(_guiTopShopButton))
		{
			_guiTopShopButton.OnPressed -= ShopButtonPressed;
			_guiTopShopButton.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_guiTopAlmanacButton))
		{
			_guiTopAlmanacButton.OnPressed -= AlmanacButtonPressed;
			_guiTopAlmanacButton.Visible = false;
		}
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched -= _OnUISwitched;
		}
		if (GlobalFeatureManager.Instance != null)
		{
			GlobalFeatureManager.Instance.FeatureChanged -= _OnGlobalFeatureChanged;
		}
		_signalsConnected = false;
	}

	private void _OnGlobalFeatureChanged(string featureId, int _oldValue, int _newValue)
	{
		if (!(featureId != "Shop"))
		{
			bool mobileMode = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
			_UpdateGuiTopButtonVisibility(mobileMode);
			_HidePacketBankLegacyButtons();
		}
	}

	private void _OnPacketBankAnimationFinished(StringName animName)
	{
		switch (animName.ToString())
		{
		case "Enter":
		case "MobileEnter":
			_packetBankEntered = true;
			break;
		case "Exit":
		case "MobileExit":
			_packetBankEntered = false;
			break;
		}
		_UpdateGuiTopButtonVisibility(GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool());
		_HidePacketBankLegacyButtons();
	}

	private void _OnUISwitched(bool mobileMode)
	{
		if (GodotObject.IsInstanceValid(packetBank) && GodotObject.IsInstanceValid(packetBank.translate))
		{
			_packetBankEntered = packetBank.translate.Position.Y < 500f;
			_UpdateGuiTopButtonVisibility(mobileMode);
			_HidePacketBankLegacyButtons();
		}
	}

	private void _UpdateGuiTopButtonVisibility(bool mobileMode)
	{
		if (GodotObject.IsInstanceValid(_guiTopShopButton) && GodotObject.IsInstanceValid(_guiTopAlmanacButton))
		{
			if (mobileMode || !_packetBankEntered)
			{
				_guiTopShopButton.Visible = false;
				_guiTopAlmanacButton.Visible = false;
			}
			else
			{
				_guiTopShopButton.Visible = GlobalFeatureManager.Instance?.IsUnlocked("Shop") ?? false;
				_guiTopAlmanacButton.Visible = true;
			}
		}
	}

	private void _HidePacketBankLegacyButtons()
	{
		CanvasItem nodeOrNull = packetBank.GetNodeOrNull<CanvasItem>("ShopButton");
		CanvasItem nodeOrNull2 = packetBank.GetNodeOrNull<CanvasItem>("AlmanacButton");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.Visible = false;
		}
		if (GodotObject.IsInstanceValid(nodeOrNull2))
		{
			nodeOrNull2.Visible = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(34)
		{
			new MethodInfo(MethodName.EmitChooseOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PacketBankInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPacketBankData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PacketChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindSelectedPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindVirtualizedPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnbindVirtualizedPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsPacketAvailableInCurrentPacketBank, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildPacketAvailabilityIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PacketListChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "_packetList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketChooseFromName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "lock", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketAlive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RockButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EmitChooseOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PacketClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CategoryChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reFresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoveChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReSelectButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ViewButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPacketGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SavePacketGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShopButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AlmanacButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ConnectPacketBankSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnCategoryChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._InitGuiTopButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._DisconnectPacketBankSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnGlobalFeatureChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_oldValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnPacketBankAnimationFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "animName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnUISwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobileMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UpdateGuiTopButtonVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobileMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HidePacketBankLegacyButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitChooseOver && args.Count == 0)
		{
			EmitChooseOver();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.PacketBankInit && args.Count == 0)
		{
			PacketBankInit();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketBankData && args.Count == 1)
		{
			SetPacketBankData(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketChoose && args.Count == 1)
		{
			PacketChoose(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindSelectedPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(FindSelectedPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BindVirtualizedPacket && args.Count == 2)
		{
			BindVirtualizedPacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindVirtualizedPacket && args.Count == 1)
		{
			UnbindVirtualizedPacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPacketAvailableInCurrentPacketBank && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPacketAvailableInCurrentPacketBank(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RebuildPacketAvailabilityIndex && args.Count == 0)
		{
			RebuildPacketAvailabilityIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.PacketListChoose && args.Count == 1)
		{
			PacketListChoose(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketChooseFromName && args.Count == 2)
		{
			PacketChooseFromName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketAlive && args.Count == 1)
		{
			PacketAlive(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RockButtonPressed && args.Count == 0)
		{
			RockButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._EmitChooseOver && args.Count == 0)
		{
			_EmitChooseOver();
			ret = default;
			return true;
		}
		if (method == MethodName.PacketClear && args.Count == 0)
		{
			PacketClear();
			ret = default;
			return true;
		}
		if (method == MethodName.CategoryChoose && args.Count == 2)
		{
			CategoryChoose(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoveChange && args.Count == 1)
		{
			LoveChange(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReSelectButtonPressed && args.Count == 0)
		{
			ReSelectButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ViewButtonPressed && args.Count == 0)
		{
			ViewButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacketGroup && args.Count == 1)
		{
			LoadPacketGroup(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SavePacketGroup && args.Count == 1)
		{
			SavePacketGroup(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShopButtonPressed && args.Count == 0)
		{
			ShopButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed && args.Count == 0)
		{
			AlmanacButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._ConnectPacketBankSignals && args.Count == 0)
		{
			_ConnectPacketBankSignals();
			ret = default;
			return true;
		}
		if (method == MethodName._OnCategoryChoose && args.Count == 1)
		{
			_OnCategoryChoose(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._InitGuiTopButtons && args.Count == 0)
		{
			_InitGuiTopButtons();
			ret = default;
			return true;
		}
		if (method == MethodName._DisconnectPacketBankSignals && args.Count == 0)
		{
			_DisconnectPacketBankSignals();
			ret = default;
			return true;
		}
		if (method == MethodName._OnGlobalFeatureChanged && args.Count == 3)
		{
			_OnGlobalFeatureChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnPacketBankAnimationFinished && args.Count == 1)
		{
			_OnPacketBankAnimationFinished(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnUISwitched && args.Count == 1)
		{
			_OnUISwitched(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UpdateGuiTopButtonVisibility && args.Count == 1)
		{
			_UpdateGuiTopButtonVisibility(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._HidePacketBankLegacyButtons && args.Count == 0)
		{
			_HidePacketBankLegacyButtons();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitChooseOver)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.PacketBankInit)
		{
			return true;
		}
		if (method == MethodName.SetPacketBankData)
		{
			return true;
		}
		if (method == MethodName.PacketChoose)
		{
			return true;
		}
		if (method == MethodName.FindSelectedPacket)
		{
			return true;
		}
		if (method == MethodName.BindVirtualizedPacket)
		{
			return true;
		}
		if (method == MethodName.UnbindVirtualizedPacket)
		{
			return true;
		}
		if (method == MethodName.IsPacketAvailableInCurrentPacketBank)
		{
			return true;
		}
		if (method == MethodName.RebuildPacketAvailabilityIndex)
		{
			return true;
		}
		if (method == MethodName.PacketListChoose)
		{
			return true;
		}
		if (method == MethodName.PacketChooseFromName)
		{
			return true;
		}
		if (method == MethodName.PacketAlive)
		{
			return true;
		}
		if (method == MethodName.RockButtonPressed)
		{
			return true;
		}
		if (method == MethodName._EmitChooseOver)
		{
			return true;
		}
		if (method == MethodName.PacketClear)
		{
			return true;
		}
		if (method == MethodName.CategoryChoose)
		{
			return true;
		}
		if (method == MethodName.LoveChange)
		{
			return true;
		}
		if (method == MethodName.ReSelectButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ViewButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LoadPacketGroup)
		{
			return true;
		}
		if (method == MethodName.SavePacketGroup)
		{
			return true;
		}
		if (method == MethodName.ShopButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed)
		{
			return true;
		}
		if (method == MethodName._ConnectPacketBankSignals)
		{
			return true;
		}
		if (method == MethodName._OnCategoryChoose)
		{
			return true;
		}
		if (method == MethodName._InitGuiTopButtons)
		{
			return true;
		}
		if (method == MethodName._DisconnectPacketBankSignals)
		{
			return true;
		}
		if (method == MethodName._OnGlobalFeatureChanged)
		{
			return true;
		}
		if (method == MethodName._OnPacketBankAnimationFinished)
		{
			return true;
		}
		if (method == MethodName._OnUISwitched)
		{
			return true;
		}
		if (method == MethodName._UpdateGuiTopButtonVisibility)
		{
			return true;
		}
		if (method == MethodName._HidePacketBankLegacyButtons)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetBank)
		{
			packetBank = VariantUtils.ConvertTo<TowerDefenseInGamePacketBank>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelPacketBankConfig>(in value);
			return true;
		}
		if (name == PropertyName.skipPacketChoose)
		{
			skipPacketChoose = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetBankData)
		{
			packetBankData = VariantUtils.ConvertTo<TowerDefensePacketBankData>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.currentCategory)
		{
			currentCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			currentIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._currentPacket)
		{
			_currentPacket = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._categoryGeneration)
		{
			_categoryGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._guiTopShopButton)
		{
			_guiTopShopButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName._guiTopAlmanacButton)
		{
			_guiTopAlmanacButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName._packetBankEntered)
		{
			_packetBankEntered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._signalsConnected)
		{
			_signalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetBank)
		{
			value = VariantUtils.CreateFrom(in packetBank);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.skipPacketChoose)
		{
			value = VariantUtils.CreateFrom(in skipPacketChoose);
			return true;
		}
		if (name == PropertyName.packetBankData)
		{
			value = VariantUtils.CreateFrom(in packetBankData);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		if (name == PropertyName.currentCategory)
		{
			value = VariantUtils.CreateFrom(in currentCategory);
			return true;
		}
		if (name == PropertyName.currentIndex)
		{
			value = VariantUtils.CreateFrom(in currentIndex);
			return true;
		}
		if (name == PropertyName._currentPacket)
		{
			value = VariantUtils.CreateFrom(in _currentPacket);
			return true;
		}
		if (name == PropertyName._categoryGeneration)
		{
			value = VariantUtils.CreateFrom(in _categoryGeneration);
			return true;
		}
		if (name == PropertyName._guiTopShopButton)
		{
			value = VariantUtils.CreateFrom(in _guiTopShopButton);
			return true;
		}
		if (name == PropertyName._guiTopAlmanacButton)
		{
			value = VariantUtils.CreateFrom(in _guiTopAlmanacButton);
			return true;
		}
		if (name == PropertyName._packetBankEntered)
		{
			value = VariantUtils.CreateFrom(in _packetBankEntered);
			return true;
		}
		if (name == PropertyName._signalsConnected)
		{
			value = VariantUtils.CreateFrom(in _signalsConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skipPacketChoose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._currentPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._categoryGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._guiTopShopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._guiTopAlmanacButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._packetBankEntered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._signalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetBank, Variant.From(in packetBank));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.skipPacketChoose, Variant.From(in skipPacketChoose));
		info.AddProperty(PropertyName.packetBankData, Variant.From(in packetBankData));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
		info.AddProperty(PropertyName.currentCategory, Variant.From(in currentCategory));
		info.AddProperty(PropertyName.currentIndex, Variant.From(in currentIndex));
		info.AddProperty(PropertyName._currentPacket, Variant.From(in _currentPacket));
		info.AddProperty(PropertyName._categoryGeneration, Variant.From(in _categoryGeneration));
		info.AddProperty(PropertyName._guiTopShopButton, Variant.From(in _guiTopShopButton));
		info.AddProperty(PropertyName._guiTopAlmanacButton, Variant.From(in _guiTopAlmanacButton));
		info.AddProperty(PropertyName._packetBankEntered, Variant.From(in _packetBankEntered));
		info.AddProperty(PropertyName._signalsConnected, Variant.From(in _signalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetBank, out var value))
		{
			packetBank = value.As<TowerDefenseInGamePacketBank>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseLevelPacketBankConfig>();
		}
		if (info.TryGetProperty(PropertyName.skipPacketChoose, out var value3))
		{
			skipPacketChoose = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetBankData, out var value4))
		{
			packetBankData = value4.As<TowerDefensePacketBankData>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value5))
		{
			packetList = value5.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.currentCategory, out var value6))
		{
			currentCategory = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentIndex, out var value7))
		{
			currentIndex = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._currentPacket, out var value8))
		{
			_currentPacket = value8.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._categoryGeneration, out var value9))
		{
			_categoryGeneration = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._guiTopShopButton, out var value10))
		{
			_guiTopShopButton = value10.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName._guiTopAlmanacButton, out var value11))
		{
			_guiTopAlmanacButton = value11.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName._packetBankEntered, out var value12))
		{
			_packetBankEntered = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._signalsConnected, out var value13))
		{
			_signalsConnected = value13.As<bool>();
		}
	}
}
