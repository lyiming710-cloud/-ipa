using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/SeedBank/Control/TowerDefenseInGameSeedBank.cs")]
public class TowerDefenseInGameSeedBank : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName GetBoundSun = "GetBoundSun";

		public static readonly StringName SetBoundSun = "SetBoundSun";

		public static readonly StringName SetMobileMode = "SetMobileMode";

		public static readonly StringName SetModeReferences = "SetModeReferences";

		public static readonly StringName ApplyMode = "ApplyMode";

		public static readonly StringName TransferPackets = "TransferPackets";

		public static readonly StringName GetRequiredSlotNum = "GetRequiredSlotNum";

		public static readonly StringName SyncPacketSlots = "SyncPacketSlots";

		public static readonly StringName UpdateCustomMinimumSize = "UpdateCustomMinimumSize";

		public static readonly StringName EnsurePacketSlots = "EnsurePacketSlots";

		public static readonly StringName EnsurePacketSlotCount = "EnsurePacketSlotCount";

		public static readonly StringName InitPacketSlots = "InitPacketSlots";

		public static readonly StringName ScheduleLayoutRefresh = "ScheduleLayoutRefresh";

		public static readonly StringName OnLayoutRefreshFrame = "OnLayoutRefreshFrame";

		public static readonly StringName CancelLayoutRefresh = "CancelLayoutRefresh";

		public static readonly StringName InitializeReferences = "InitializeReferences";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FinalizeReady = "FinalizeReady";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName UpdateDisplay = "UpdateDisplay";

		public static readonly StringName RefreshSunDisplay = "RefreshSunDisplay";

		public static readonly StringName SetSunDisplay = "SetSunDisplay";

		public static readonly StringName ApplyDebugSunMax = "ApplyDebugSunMax";

		public static readonly StringName RefreshPacketRuntimeStates = "RefreshPacketRuntimeStates";

		public static readonly StringName HasPacket = "HasPacket";

		public static readonly StringName CanAddPacket = "CanAddPacket";

		public static readonly StringName GetPacketPos = "GetPacketPos";

		public static readonly StringName GetPacketFromPool = "GetPacketFromPool";

		public static readonly StringName AttachBattlePacket = "AttachBattlePacket";

		public static readonly StringName CreateRoundPacketConfig = "CreateRoundPacketConfig";

		public static readonly StringName ReturnPacketToPool = "ReturnPacketToPool";

		public static readonly StringName AddPacket = "AddPacket";

		public static readonly StringName ClearPacketsForProgressRestore = "ClearPacketsForProgressRestore";

		public static readonly StringName RestorePacketFromProgress = "RestorePacketFromProgress";

		public static readonly StringName BindPacketSunAccount = "BindPacketSunAccount";

		public static readonly StringName DeletePacket = "DeletePacket";

		public static readonly StringName DeleteAllPacket = "DeleteAllPacket";

		public static readonly StringName Prepare = "Prepare";

		public static readonly StringName ReadyPackets = "ReadyPackets";

		public static readonly StringName Start = "Start";

		public static readonly StringName StartFromProgress = "StartFromProgress";

		public static readonly StringName DisposeOwnedPackets = "DisposeOwnedPackets";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName HasSunAccount = "HasSunAccount";

		public static readonly StringName sunNumShow = "sunNumShow";

		public static readonly StringName pcControl = "pcControl";

		public static readonly StringName pcSunLabel = "pcSunLabel";

		public static readonly StringName pcSeedContainer = "pcSeedContainer";

		public static readonly StringName pcItemContainer = "pcItemContainer";

		public static readonly StringName pcPacketSlotContainer = "pcPacketSlotContainer";

		public static readonly StringName pcPacketContainer = "pcPacketContainer";

		public static readonly StringName pcSeedBankTexture = "pcSeedBankTexture";

		public static readonly StringName pcSunBankTexture = "pcSunBankTexture";

		public static readonly StringName mobileControl = "mobileControl";

		public static readonly StringName mobileSunLabel = "mobileSunLabel";

		public static readonly StringName mobileSeedContainer = "mobileSeedContainer";

		public static readonly StringName mobileItemContainer = "mobileItemContainer";

		public static readonly StringName mobilePacketSlotContainer = "mobilePacketSlotContainer";

		public static readonly StringName mobilePacketContainer = "mobilePacketContainer";

		public static readonly StringName mobileSunBarTexture = "mobileSunBarTexture";

		public static readonly StringName mobileISeedContain = "mobileISeedContain";

		public static readonly StringName _sunNumShow = "_sunNumShow";

		public static readonly StringName _sunDisplayLabel = "_sunDisplayLabel";

		public static readonly StringName _sunDisplayValue = "_sunDisplayValue";

		public static readonly StringName _sunDisplayInitialized = "_sunDisplayInitialized";

		public static readonly StringName packetNum = "packetNum";

		public static readonly StringName packetList = "packetList";

		public static readonly StringName _packetPool = "_packetPool";

		public static readonly StringName packetNameSet = "packetNameSet";

		public static readonly StringName packetColdDownState = "packetColdDownState";

		public static readonly StringName hasGameStarted = "hasGameStarted";

		public static readonly StringName mobilePreset = "mobilePreset";

		public static readonly StringName _referencesInitialized = "_referencesInitialized";

		public static readonly StringName _layoutRefreshTree = "_layoutRefreshTree";

		public static readonly StringName _layoutRefreshPending = "_layoutRefreshPending";

		public static readonly StringName sunLabel = "sunLabel";

		public static readonly StringName itemContainer = "itemContainer";

		public static readonly StringName packetSlotContainer = "packetSlotContainer";

		public static readonly StringName packetContainer = "packetContainer";

		public static readonly StringName seedContainer = "seedContainer";

		public static readonly StringName sunBankTexture = "sunBankTexture";

		public static readonly StringName animationPlayer = "animationPlayer";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static PackedScene _towerDefenseInGamePacketSlot;

	private static Texture2D _seedBankZombie;

	private static Texture2D _sunBankZombieMobile;

	public Control pcControl;

	public Label pcSunLabel;

	public MarginContainer pcSeedContainer;

	public HBoxContainer pcItemContainer;

	public CanvasItem pcPacketSlotContainer;

	public Node pcPacketContainer;

	public NinePatchRect pcSeedBankTexture;

	public TextureRect pcSunBankTexture;

	public Control mobileControl;

	public Label mobileSunLabel;

	public MarginContainer mobileSeedContainer;

	public HBoxContainer mobileItemContainer;

	public CanvasItem mobilePacketSlotContainer;

	public Node mobilePacketContainer;

	public TextureRect mobileSunBarTexture;

	public MarginContainer mobileISeedContain;

	private double _sunNumShow = 50.0;

	private Label _sunDisplayLabel;

	private long _sunDisplayValue;

	private bool _sunDisplayInitialized;

	private EconomyAccountId _sunAccountId;

	public int packetNum;

	public Array<TowerDefenseInGamePacketShow> packetList = new Array<TowerDefenseInGamePacketShow>();

	private Array<TowerDefenseInGamePacketShow> _packetPool = new Array<TowerDefenseInGamePacketShow>();

	private const int MAX_POOL_SIZE = 24;

	public Dictionary packetNameSet = new Dictionary();

	public Dictionary packetColdDownState = new Dictionary();

	public bool hasGameStarted;

	public bool mobilePreset;

	private bool _referencesInitialized;

	private SceneTree _layoutRefreshTree;

	private Action _layoutRefreshHandler;

	private bool _layoutRefreshPending;

	public Label sunLabel;

	public HBoxContainer itemContainer;

	public CanvasItem packetSlotContainer;

	public Node packetContainer;

	public MarginContainer seedContainer;

	public TextureRect sunBankTexture;

	public AnimationPlayer animationPlayer;

	private static PackedScene TOWER_DEFENSE_IN_GAME_PACKET_SLOT => _towerDefenseInGamePacketSlot ?? (_towerDefenseInGamePacketSlot = GD.Load<PackedScene>("res://Prefab/TowerDefense/GUI/InGame/Packet/TowerDefenseInGamePacketSlot.tscn"));

	private static Texture2D SEED_BANK_ZOMBIE => _seedBankZombie ?? (_seedBankZombie = GD.Load<Texture2D>("uid://1styv80tn18a"));

	private static Texture2D SUN_BANK_ZOMBIE_MOBILE => _sunBankZombieMobile ?? (_sunBankZombieMobile = GD.Load<Texture2D>("uid://bpdw8ty3ivc67"));

	public EconomyAccountId SunAccountId => _sunAccountId;

	public bool HasSunAccount => _sunAccountId.IsValid;

	public double sunNumShow
	{
		get
		{
			return _sunNumShow;
		}
		set
		{
			long num = (long)Math.Round(value);
			_sunNumShow = value;
			if (sunLabel != null && (!_sunDisplayInitialized || _sunDisplayLabel != sunLabel || _sunDisplayValue != num))
			{
				sunLabel.Text = num.ToString();
				_sunDisplayLabel = sunLabel;
				_sunDisplayValue = num;
				_sunDisplayInitialized = true;
			}
		}
	}

	public bool TryBindSunAccount(EconomyAccountId accountId)
	{
		if (!accountId.IsValid)
		{
			return false;
		}
		if (_sunAccountId.IsValid)
		{
			return _sunAccountId == accountId;
		}
		_sunAccountId = accountId;
		return true;
	}

	private long GetBoundSun()
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return (long)Math.Round(_sunNumShow);
		}
		if (!HasSunAccount)
		{
			return TowerDefenseManager.Instance.GetSun();
		}
		return TowerDefenseManager.Instance.GetSun(_sunAccountId);
	}

	private void SetBoundSun(long value)
	{
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			if (HasSunAccount)
			{
				TowerDefenseManager.Instance.SetSun(_sunAccountId, value);
			}
			else
			{
				TowerDefenseManager.Instance.SetSun(value);
			}
		}
	}

	public void SetMobileMode(bool enabled)
	{
		if (mobilePreset != enabled)
		{
			Node targetContainer = (enabled ? mobilePacketContainer : pcPacketContainer);
			TransferPackets(targetContainer);
			mobilePreset = enabled;
			ApplyMode();
			GameSaveManager.Instance.SetConfigValue("MobilePreset", enabled);
		}
	}

	private void SetModeReferences()
	{
		if (mobilePreset)
		{
			sunLabel = mobileSunLabel;
			itemContainer = mobileItemContainer;
			packetSlotContainer = mobilePacketSlotContainer;
			packetContainer = mobilePacketContainer;
			seedContainer = mobileSeedContainer;
			sunBankTexture = mobileSunBarTexture;
		}
		else
		{
			sunLabel = pcSunLabel;
			itemContainer = pcItemContainer;
			packetSlotContainer = pcPacketSlotContainer;
			packetContainer = pcPacketContainer;
			seedContainer = pcSeedContainer;
			sunBankTexture = pcSunBankTexture;
		}
	}

	private void ApplyMode()
	{
		SetModeReferences();
		if (mobilePreset)
		{
			pcControl.Visible = false;
			mobileControl.Visible = true;
			pcSeedContainer.Visible = false;
			mobilePacketSlotContainer.Visible = true;
			pcPacketSlotContainer.Visible = false;
			CustomMinimumSize = new Vector2(0f, CustomMinimumSize.Y);
		}
		else
		{
			mobileControl.Visible = false;
			pcControl.Visible = true;
			mobileSeedContainer.Visible = false;
			mobilePacketSlotContainer.Visible = false;
			pcPacketSlotContainer.Visible = true;
			CustomMinimumSize = new Vector2(itemContainer.Size.X, CustomMinimumSize.Y);
		}
		SyncPacketSlots();
		seedContainer.Visible = true;
		sunNumShow = GetBoundSun();
		itemContainer.Visible = true;
	}

	private void TransferPackets(Node targetContainer)
	{
		if (packetContainer == targetContainer)
		{
			return;
		}
		foreach (Node child in packetContainer.GetChildren())
		{
			if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
			{
				towerDefenseInGamePacketShow.Reparent(targetContainer);
			}
		}
	}

	private int GetRequiredSlotNum()
	{
		return Math.Max(GodotObject.IsInstanceValid(TowerDefenseManager.Instance) ? TowerDefenseManager.Instance.seedbankPacketMax : packetList.Count, packetList.Count);
	}

	private void SyncPacketSlots()
	{
		int requiredSlotNum = GetRequiredSlotNum();
		EnsurePacketSlotCount(pcPacketSlotContainer, requiredSlotNum, isMobile: false);
		EnsurePacketSlotCount(mobilePacketSlotContainer, requiredSlotNum, isMobile: true);
		ScheduleLayoutRefresh();
	}

	private void UpdateCustomMinimumSize()
	{
		if (mobilePreset)
		{
			CustomMinimumSize = new Vector2(0f, CustomMinimumSize.Y);
		}
		else
		{
			CustomMinimumSize = new Vector2(seedContainer.Size.X, CustomMinimumSize.Y);
		}
	}

	private void EnsurePacketSlots()
	{
		int requiredSlotNum = GetRequiredSlotNum();
		int childCount = pcPacketSlotContainer.GetChildCount();
		if (requiredSlotNum > childCount)
		{
			EnsurePacketSlotCount(pcPacketSlotContainer, requiredSlotNum, isMobile: false);
			EnsurePacketSlotCount(mobilePacketSlotContainer, requiredSlotNum, isMobile: true);
			ScheduleLayoutRefresh();
		}
	}

	private void EnsurePacketSlotCount(Node container, int slotNum, bool isMobile)
	{
		while (container.GetChildCount() > slotNum)
		{
			Node child = container.GetChild(container.GetChildCount() - 1);
			container.RemoveChild(child);
			child.QueueFree();
		}
		while (container.GetChildCount() < slotNum)
		{
			TowerDefenseInGamePacketSlot towerDefenseInGamePacketSlot = TOWER_DEFENSE_IN_GAME_PACKET_SLOT.Instantiate<TowerDefenseInGamePacketSlot>(PackedScene.GenEditState.Disabled);
			container.AddChild(towerDefenseInGamePacketSlot, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGamePacketSlot.SetContainerFootprint(enabled: true);
			towerDefenseInGamePacketSlot.SetMobileMode(isMobile);
		}
	}

	private void InitPacketSlots(CanvasItem container, int slotNum, bool isMobile)
	{
		EnsurePacketSlotCount(container, slotNum, isMobile);
		container.Visible = false;
	}

	private void ScheduleLayoutRefresh()
	{
		if (!_layoutRefreshPending && IsInsideTree())
		{
			_layoutRefreshTree = GetTree();
			if (GodotObject.IsInstanceValid(_layoutRefreshTree))
			{
				_layoutRefreshPending = true;
				_layoutRefreshHandler = OnLayoutRefreshFrame;
				_layoutRefreshTree.ProcessFrame += _layoutRefreshHandler;
			}
		}
	}

	private void OnLayoutRefreshFrame()
	{
		CancelLayoutRefresh();
		if (IsInsideTree())
		{
			UpdateCustomMinimumSize();
		}
	}

	private void CancelLayoutRefresh()
	{
		if (GodotObject.IsInstanceValid(_layoutRefreshTree) && _layoutRefreshHandler != null)
		{
			_layoutRefreshTree.ProcessFrame -= _layoutRefreshHandler;
		}
		_layoutRefreshTree = null;
		_layoutRefreshHandler = null;
		_layoutRefreshPending = false;
	}

	private void InitializeReferences()
	{
		if (!_referencesInitialized)
		{
			pcControl = GetNode<Control>("%PCControl");
			pcSunLabel = GetNode<Label>("%SunLabel");
			pcSeedContainer = GetNode<MarginContainer>("%SeedContainer");
			pcItemContainer = GetNode<HBoxContainer>("%ItemContainer");
			pcPacketSlotContainer = GetNode<CanvasItem>("%PacketSlotContainer");
			pcPacketContainer = GetNode("%PacketContainer");
			pcSeedBankTexture = GetNode<NinePatchRect>("%SeedBankTexture");
			pcSunBankTexture = GetNode<TextureRect>("%SunBankTexture");
			mobileControl = GetNode<Control>("%MobileControl");
			mobileSunLabel = GetNode<Label>("%MobileSunLabel");
			mobileSeedContainer = GetNode<MarginContainer>("%MobileSeedContainer");
			mobileItemContainer = GetNode<HBoxContainer>("%MobileItemContainer");
			mobilePacketSlotContainer = GetNode<CanvasItem>("%MobilePacketSlotContainer");
			mobilePacketContainer = GetNode("%MobilePacketContainer");
			mobileSunBarTexture = GetNode<TextureRect>("%MobileSunBarTexture");
			mobileISeedContain = GetNode<MarginContainer>("%MobileISeedContanin");
			if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
			{
				BattleEventBus.Instance.OnUiSwitched -= SetMobileMode;
				BattleEventBus.Instance.OnUiSwitched += SetMobileMode;
			}
			int packetSlotNum = TowerDefenseManager.Instance.GetPacketSlotNum();
			InitPacketSlots(pcPacketSlotContainer, packetSlotNum, isMobile: false);
			InitPacketSlots(mobilePacketSlotContainer, packetSlotNum, isMobile: true);
			mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
			SetModeReferences();
			packetList.Clear();
			packetNameSet.Clear();
			_referencesInitialized = true;
		}
	}

	public override void _Ready()
	{
		InitializeReferences();
		Callable.From(FinalizeReady).CallDeferred();
	}

	private void FinalizeReady()
	{
		if (IsInsideTree() && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode())
			{
				pcSeedBankTexture.Texture = SEED_BANK_ZOMBIE;
				mobileSunBarTexture.Texture = SUN_BANK_ZOMBIE_MOBILE;
			}
			ApplyMode();
		}
	}

	public override void _ExitTree()
	{
		CancelLayoutRefresh();
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched -= SetMobileMode;
		}
		DisposeOwnedPackets();
		base._ExitTree();
	}

	public void UpdateDisplay()
	{
		RefreshSunDisplay();
	}

	public void RefreshSunDisplay()
	{
		if (GodotObject.IsInstanceValid(seedContainer) && GodotObject.IsInstanceValid(sunLabel) && !sunLabel.Visible)
		{
			sunLabel.Visible = seedContainer.Visible;
		}
		if (GodotObject.IsInstanceValid(sunBankTexture) && GodotObject.IsInstanceValid(sunLabel) && !sunLabel.Visible)
		{
			sunLabel.Visible = sunBankTexture.Visible;
		}
		sunNumShow = GetBoundSun();
	}

	public void SetSunDisplay(long balance)
	{
		sunNumShow = balance;
	}

	public void ApplyDebugSunMax()
	{
		SetBoundSun(100000L);
	}

	public void RefreshPacketRuntimeStates(bool includeCost)
	{
		for (int i = 0; i < packetList.Count; i++)
		{
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = packetList[i];
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.RefreshRuntimeState(includeCost);
			}
		}
	}

	public bool HasPacket(string packetName)
	{
		return packetNameSet.ContainsKey(packetName);
	}

	public bool CanAddPacket()
	{
		return packetNum < TowerDefenseManager.Instance.seedbankPacketMax;
	}

	public Vector2 GetPacketPos(int id)
	{
		return packetSlotContainer.GetChild<TowerDefenseInGamePacketSlot>(id).GetVisualCenterGlobalPosition();
	}

	private TowerDefenseInGamePacketShow GetPacketFromPool()
	{
		if (_packetPool.Count > 0)
		{
			TowerDefenseInGamePacketShow result = _packetPool[_packetPool.Count - 1];
			_packetPool.RemoveAt(_packetPool.Count - 1);
			return result;
		}
		return TowerDefenseManager.CreatePacketShow();
	}

	private void AttachBattlePacket(TowerDefenseInGamePacketShow packet)
	{
		packet.SetCentralRuntimeStateRefresh(enabled: true);
		packetContainer.AddChild(packet, forceReadableName: false, InternalMode.Disabled);
		packet.SetMobileMode(mobilePreset);
		packet.SetContainerFootprint(enabled: true);
	}

	private static TowerDefensePacketConfig CreateRoundPacketConfig(TowerDefensePacketConfig source)
	{
		if (!GodotObject.IsInstanceValid(source))
		{
			return null;
		}
		TowerDefensePacketConfig towerDefensePacketConfig = source.Duplicate(deep: true) as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(source._override))
		{
			towerDefensePacketConfig._override = source._override.Duplicate(deep: true) as TowerDefensePacketOverride;
		}
		towerDefensePacketConfig.changeCostList = new List<TowerDefensePacketChangeCost>();
		towerDefensePacketConfig.coldDownDecreaseDictionary = new Dictionary();
		return towerDefensePacketConfig;
	}

	private void ReturnPacketToPool(TowerDefenseInGamePacketShow packet)
	{
		packet.ResetForPool();
		packetContainer.RemoveChild(packet);
		if (_packetPool.Count < 24)
		{
			_packetPool.Add(packet);
		}
		else
		{
			packet.QueueFree();
		}
	}

	public TowerDefenseInGamePacketShow AddPacket(TowerDefensePacketConfig _packetConfig, bool isStart = false)
	{
		if (_packetConfig == null)
		{
			GD.PushError("[SeedBank] AddPacket: _packetConfig is null, skipping");
			return null;
		}
		TowerDefensePacketConfig towerDefensePacketConfig = CreateRoundPacketConfig(_packetConfig);
		if (towerDefensePacketConfig == null)
		{
			GD.PushError("[SeedBank] AddPacket: failed to create round-local packet config, skipping");
			return null;
		}
		InitializeReferences();
		if (packetContainer == null)
		{
			GD.PushError("[SeedBank] AddPacket: packetContainer is null (_Ready not complete), skipping");
			return null;
		}
		packetNum++;
		TowerDefenseInGamePacketShow packetFromPool = GetPacketFromPool();
		AttachBattlePacket(packetFromPool);
		packetFromPool.Init(towerDefensePacketConfig);
		BindPacketSunAccount(packetFromPool);
		packetFromPool.onlyDraw = false;
		packetFromPool.enforceRuntimeAvailabilityOnPress = isStart;
		packetNameSet[towerDefensePacketConfig.saveKey] = true;
		if (!isStart)
		{
			packetFromPool.OnPressed += DeletePacket;
		}
		else
		{
			packetFromPool.alive = true;
			packetFromPool.start = true;
			packetFromPool.OnPressed += TowerDefenseManager.Instance.GetPacketPickControl().PickPacket;
			packetFromPool.StartInit();
		}
		packetList.Add(packetFromPool);
		if (packetFromPool.start)
		{
			packetFromPool.RefreshRuntimeState(includeCost: true);
		}
		EnsurePacketSlots();
		return packetFromPool;
	}

	public void ClearPacketsForProgressRestore()
	{
		InitializeReferences();
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			if (GodotObject.IsInstanceValid(packet))
			{
				packetNameSet.Remove(packet.config?.saveKey ?? "");
				if (packet.GetParent() == packetContainer)
				{
					ReturnPacketToPool(packet);
				}
				else
				{
					packet.QueueFree();
				}
			}
		}
		packetList.Clear();
		packetNameSet.Clear();
		packetColdDownState.Clear();
		packetNum = 0;
		EnsurePacketSlots();
	}

	public TowerDefenseInGamePacketShow RestorePacketFromProgress(TowerDefensePacketConfig packetConfig)
	{
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			GD.PushError("[SeedBank] RestorePacketFromProgress: packetConfig is invalid, skipping");
			return null;
		}
		InitializeReferences();
		if (packetContainer == null)
		{
			GD.PushError("[SeedBank] RestorePacketFromProgress: packetContainer is null, skipping");
			return null;
		}
		TowerDefenseInGamePacketShow packetFromPool = GetPacketFromPool();
		AttachBattlePacket(packetFromPool);
		packetFromPool.Init(packetConfig);
		BindPacketSunAccount(packetFromPool);
		packetFromPool.onlyDraw = false;
		packetNameSet[packetConfig.saveKey] = true;
		packetList.Add(packetFromPool);
		packetNum = packetList.Count;
		EnsurePacketSlots();
		return packetFromPool;
	}

	private void BindPacketSunAccount(TowerDefenseInGamePacketShow packet)
	{
		if (HasSunAccount && !packet.TryBindSunAccount(_sunAccountId))
		{
			GD.PushError("[SeedBank] Packet already belongs to a different Sun account.");
		}
	}

	public void DeletePacket(TowerDefenseInGamePacketShow _packet)
	{
		int num = packetList.IndexOf(_packet);
		if (num < 0)
		{
			if (GodotObject.IsInstanceValid(_packet))
			{
				if (GodotObject.IsInstanceValid(_packet.config))
				{
					packetNameSet.Remove(_packet.config.saveKey);
					string packetName = ((_packet.originalSaveKey != "") ? _packet.originalSaveKey : _packet.config.saveKey);
					TowerDefenseManager.Instance.GetPacketBankFeature()?.PacketAlive(packetName);
				}
				if (_packet.GetParent() == packetContainer)
				{
					ReturnPacketToPool(_packet);
				}
				else
				{
					_packet.QueueFree();
				}
			}
			packetNum = packetList.Count;
		}
		else
		{
			packetNameSet.Remove(_packet.config.saveKey);
			string packetName2 = ((_packet.originalSaveKey != "") ? _packet.originalSaveKey : _packet.config.saveKey);
			TowerDefenseManager.Instance.GetPacketBankFeature()?.PacketAlive(packetName2);
			packetList.RemoveAt(num);
			ReturnPacketToPool(_packet);
			packetNum = packetList.Count;
		}
	}

	public void DeleteAllPacket()
	{
		Array<TowerDefenseInGamePacketShow> array = new Array<TowerDefenseInGamePacketShow>();
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			if (packet.@lock)
			{
				array.Add(packet);
				continue;
			}
			packetNameSet.Remove(packet.config.saveKey);
			string packetName = ((packet.originalSaveKey != "") ? packet.originalSaveKey : packet.config.saveKey);
			TowerDefenseManager.Instance.GetPacketBankFeature()?.PacketAlive(packetName);
			ReturnPacketToPool(packet);
		}
		packetList = array;
		packetNum = packetList.Count;
	}

	public void Prepare()
	{
		packetNameSet.Clear();
		if (hasGameStarted)
		{
			packetColdDownState.Clear();
			foreach (Node child in packetContainer.GetChildren())
			{
				if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config))
				{
					packetColdDownState[towerDefenseInGamePacketShow.config.saveKey] = new Dictionary
					{
						["coldDownOpen"] = towerDefenseInGamePacketShow.coldDownOpen,
						["coldDownTimer"] = towerDefenseInGamePacketShow.coldDownTimer
					};
				}
			}
		}
		foreach (Node child2 in packetContainer.GetChildren())
		{
			if (child2 is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow2)
			{
				BindPacketSunAccount(towerDefenseInGamePacketShow2);
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow2.config))
				{
					packetNameSet[towerDefenseInGamePacketShow2.config.saveKey] = true;
				}
				towerDefenseInGamePacketShow2.alive = true;
				towerDefenseInGamePacketShow2.@lock = false;
				towerDefenseInGamePacketShow2.onlyDraw = false;
				towerDefenseInGamePacketShow2.enforceRuntimeAvailabilityOnPress = false;
				towerDefenseInGamePacketShow2.start = false;
				towerDefenseInGamePacketShow2.coldDownProgressBar.Visible = false;
				towerDefenseInGamePacketShow2.OnPressed += DeletePacket;
				towerDefenseInGamePacketShow2.OnPressed -= TowerDefenseManager.Instance.GetPacketPickControl().PickPacket;
			}
		}
	}

	public void ReadyPackets()
	{
		PacketPickControl packetPickControl = TowerDefenseManager.Instance?.GetPacketPickControl();
		foreach (Node child in packetContainer.GetChildren())
		{
			if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
			{
				towerDefenseInGamePacketShow.alive = false;
				towerDefenseInGamePacketShow.@lock = true;
				towerDefenseInGamePacketShow.onlyDraw = true;
				towerDefenseInGamePacketShow.OnPressed -= DeletePacket;
				if (GodotObject.IsInstanceValid(packetPickControl))
				{
					towerDefenseInGamePacketShow.OnPressed -= packetPickControl.PickPacket;
				}
			}
		}
	}

	public void Start()
	{
		if (!GodotObject.IsInstanceValid(packetContainer))
		{
			return;
		}
		foreach (Node child in packetContainer.GetChildren())
		{
			if (!(child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow))
			{
				continue;
			}
			towerDefenseInGamePacketShow.alive = true;
			towerDefenseInGamePacketShow.@lock = false;
			towerDefenseInGamePacketShow.onlyDraw = false;
			towerDefenseInGamePacketShow.enforceRuntimeAvailabilityOnPress = true;
			towerDefenseInGamePacketShow.start = true;
			towerDefenseInGamePacketShow.StartInit();
			if (packetColdDownState.ContainsKey(towerDefenseInGamePacketShow.config.saveKey))
			{
				Dictionary dictionary = packetColdDownState[towerDefenseInGamePacketShow.config.saveKey].AsGodotDictionary();
				towerDefenseInGamePacketShow.coldDownOpen = dictionary.GetValueOrDefault("coldDownOpen", false).AsBool();
				towerDefenseInGamePacketShow.coldDownTimer = dictionary.GetValueOrDefault("coldDownTimer", 0.0).AsDouble();
				if (towerDefenseInGamePacketShow.coldDownOpen)
				{
					TextureProgressBar coldDownProgressBar = towerDefenseInGamePacketShow.coldDownProgressBar;
					coldDownProgressBar.Visible = true;
					coldDownProgressBar.MaxValue = towerDefenseInGamePacketShow.coldDown;
					coldDownProgressBar.Value = towerDefenseInGamePacketShow.coldDownTimer;
				}
			}
			towerDefenseInGamePacketShow.OnPressed += TowerDefenseManager.Instance.GetPacketPickControl().PickPacket;
			towerDefenseInGamePacketShow.OnPressed -= DeletePacket;
		}
		hasGameStarted = true;
		packetColdDownState.Clear();
		RefreshPacketRuntimeStates(includeCost: true);
	}

	public void StartFromProgress()
	{
		hasGameStarted = true;
		if (!GodotObject.IsInstanceValid(packetContainer))
		{
			return;
		}
		foreach (Node child in packetContainer.GetChildren())
		{
			if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
			{
				towerDefenseInGamePacketShow.enforceRuntimeAvailabilityOnPress = true;
				towerDefenseInGamePacketShow.start = true;
				towerDefenseInGamePacketShow.onlyDraw = false;
				towerDefenseInGamePacketShow.OnPressed -= DeletePacket;
				towerDefenseInGamePacketShow.OnPressed -= TowerDefenseManager.Instance.GetPacketPickControl().PickPacket;
				towerDefenseInGamePacketShow.OnPressed += TowerDefenseManager.Instance.GetPacketPickControl().PickPacket;
				towerDefenseInGamePacketShow.OnVisibilityChanged();
			}
		}
		RefreshPacketRuntimeStates(includeCost: true);
	}

	public void DisposeOwnedPackets()
	{
		HashSet<TowerDefenseInGamePacketShow> released = new HashSet<TowerDefenseInGamePacketShow>();
		foreach (TowerDefenseInGamePacketShow packet in packetList)
		{
			ReleasePacket(packet);
		}
		if (GodotObject.IsInstanceValid(pcPacketContainer))
		{
			foreach (Node child in pcPacketContainer.GetChildren())
			{
				ReleasePacket(child as TowerDefenseInGamePacketShow);
			}
		}
		if (GodotObject.IsInstanceValid(mobilePacketContainer))
		{
			foreach (Node child2 in mobilePacketContainer.GetChildren())
			{
				ReleasePacket(child2 as TowerDefenseInGamePacketShow);
			}
		}
		foreach (TowerDefenseInGamePacketShow item in _packetPool)
		{
			ReleasePacket(item);
		}
		packetList.Clear();
		_packetPool.Clear();
		packetNameSet.Clear();
		packetColdDownState.Clear();
		packetNum = 0;
		hasGameStarted = false;
		void ReleasePacket(TowerDefenseInGamePacketShow packet)
		{
			if (GodotObject.IsInstanceValid(packet) && released.Add(packet))
			{
				packet.ClearEventHandlers();
				packet.QueueFree();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(42)
		{
			new MethodInfo(MethodName.GetBoundSun, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetBoundSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMobileMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetModeReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TransferPackets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetContainer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetRequiredSlotNum, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncPacketSlots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCustomMinimumSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePacketSlots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePacketSlotCount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "slotNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isMobile", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitPacketSlots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasItem"), exported: false),
				new PropertyInfo(Variant.Type.Int, "slotNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isMobile", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleLayoutRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnLayoutRefreshFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelLayoutRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "balance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyDebugSunMax, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketRuntimeStates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includeCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasPacket, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanAddPacket, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacketPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketFromPool, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttachBattlePacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateRoundPacketConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReturnPacketToPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPacketsForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestorePacketFromProgress, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindPacketSunAccount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.DeletePacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteAllPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Prepare, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadyPackets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Start, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartFromProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeOwnedPackets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetBoundSun && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetBoundSun());
			return true;
		}
		if (method == MethodName.SetBoundSun && args.Count == 1)
		{
			SetBoundSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMobileMode && args.Count == 1)
		{
			SetMobileMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetModeReferences && args.Count == 0)
		{
			SetModeReferences();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMode && args.Count == 0)
		{
			ApplyMode();
			ret = default;
			return true;
		}
		if (method == MethodName.TransferPackets && args.Count == 1)
		{
			TransferPackets(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetRequiredSlotNum && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetRequiredSlotNum());
			return true;
		}
		if (method == MethodName.SyncPacketSlots && args.Count == 0)
		{
			SyncPacketSlots();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCustomMinimumSize && args.Count == 0)
		{
			UpdateCustomMinimumSize();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePacketSlots && args.Count == 0)
		{
			EnsurePacketSlots();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePacketSlotCount && args.Count == 3)
		{
			EnsurePacketSlotCount(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitPacketSlots && args.Count == 3)
		{
			InitPacketSlots(VariantUtils.ConvertTo<CanvasItem>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleLayoutRefresh && args.Count == 0)
		{
			ScheduleLayoutRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.OnLayoutRefreshFrame && args.Count == 0)
		{
			OnLayoutRefreshFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelLayoutRefresh && args.Count == 0)
		{
			CancelLayoutRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeReferences && args.Count == 0)
		{
			InitializeReferences();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeReady && args.Count == 0)
		{
			FinalizeReady();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDisplay && args.Count == 0)
		{
			UpdateDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSunDisplay && args.Count == 0)
		{
			RefreshSunDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.SetSunDisplay && args.Count == 1)
		{
			SetSunDisplay(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDebugSunMax && args.Count == 0)
		{
			ApplyDebugSunMax();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketRuntimeStates && args.Count == 1)
		{
			RefreshPacketRuntimeStates(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanAddPacket && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAddPacket());
			return true;
		}
		if (method == MethodName.GetPacketPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPacketPos(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketFromPool && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(GetPacketFromPool());
			return true;
		}
		if (method == MethodName.AttachBattlePacket && args.Count == 1)
		{
			AttachBattlePacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateRoundPacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(CreateRoundPacketConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReturnPacketToPool && args.Count == 1)
		{
			ReturnPacketToPool(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPacket && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(AddPacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearPacketsForProgressRestore && args.Count == 0)
		{
			ClearPacketsForProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.RestorePacketFromProgress && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(RestorePacketFromProgress(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BindPacketSunAccount && args.Count == 1)
		{
			BindPacketSunAccount(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeletePacket && args.Count == 1)
		{
			DeletePacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteAllPacket && args.Count == 0)
		{
			DeleteAllPacket();
			ret = default;
			return true;
		}
		if (method == MethodName.Prepare && args.Count == 0)
		{
			Prepare();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyPackets && args.Count == 0)
		{
			ReadyPackets();
			ret = default;
			return true;
		}
		if (method == MethodName.Start && args.Count == 0)
		{
			Start();
			ret = default;
			return true;
		}
		if (method == MethodName.StartFromProgress && args.Count == 0)
		{
			StartFromProgress();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeOwnedPackets && args.Count == 0)
		{
			DisposeOwnedPackets();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateRoundPacketConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(CreateRoundPacketConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetBoundSun)
		{
			return true;
		}
		if (method == MethodName.SetBoundSun)
		{
			return true;
		}
		if (method == MethodName.SetMobileMode)
		{
			return true;
		}
		if (method == MethodName.SetModeReferences)
		{
			return true;
		}
		if (method == MethodName.ApplyMode)
		{
			return true;
		}
		if (method == MethodName.TransferPackets)
		{
			return true;
		}
		if (method == MethodName.GetRequiredSlotNum)
		{
			return true;
		}
		if (method == MethodName.SyncPacketSlots)
		{
			return true;
		}
		if (method == MethodName.UpdateCustomMinimumSize)
		{
			return true;
		}
		if (method == MethodName.EnsurePacketSlots)
		{
			return true;
		}
		if (method == MethodName.EnsurePacketSlotCount)
		{
			return true;
		}
		if (method == MethodName.InitPacketSlots)
		{
			return true;
		}
		if (method == MethodName.ScheduleLayoutRefresh)
		{
			return true;
		}
		if (method == MethodName.OnLayoutRefreshFrame)
		{
			return true;
		}
		if (method == MethodName.CancelLayoutRefresh)
		{
			return true;
		}
		if (method == MethodName.InitializeReferences)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.FinalizeReady)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.UpdateDisplay)
		{
			return true;
		}
		if (method == MethodName.RefreshSunDisplay)
		{
			return true;
		}
		if (method == MethodName.SetSunDisplay)
		{
			return true;
		}
		if (method == MethodName.ApplyDebugSunMax)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketRuntimeStates)
		{
			return true;
		}
		if (method == MethodName.HasPacket)
		{
			return true;
		}
		if (method == MethodName.CanAddPacket)
		{
			return true;
		}
		if (method == MethodName.GetPacketPos)
		{
			return true;
		}
		if (method == MethodName.GetPacketFromPool)
		{
			return true;
		}
		if (method == MethodName.AttachBattlePacket)
		{
			return true;
		}
		if (method == MethodName.CreateRoundPacketConfig)
		{
			return true;
		}
		if (method == MethodName.ReturnPacketToPool)
		{
			return true;
		}
		if (method == MethodName.AddPacket)
		{
			return true;
		}
		if (method == MethodName.ClearPacketsForProgressRestore)
		{
			return true;
		}
		if (method == MethodName.RestorePacketFromProgress)
		{
			return true;
		}
		if (method == MethodName.BindPacketSunAccount)
		{
			return true;
		}
		if (method == MethodName.DeletePacket)
		{
			return true;
		}
		if (method == MethodName.DeleteAllPacket)
		{
			return true;
		}
		if (method == MethodName.Prepare)
		{
			return true;
		}
		if (method == MethodName.ReadyPackets)
		{
			return true;
		}
		if (method == MethodName.Start)
		{
			return true;
		}
		if (method == MethodName.StartFromProgress)
		{
			return true;
		}
		if (method == MethodName.DisposeOwnedPackets)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sunNumShow)
		{
			sunNumShow = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.pcControl)
		{
			pcControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.pcSunLabel)
		{
			pcSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.pcSeedContainer)
		{
			pcSeedContainer = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.pcItemContainer)
		{
			pcItemContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.pcPacketSlotContainer)
		{
			pcPacketSlotContainer = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		if (name == PropertyName.pcPacketContainer)
		{
			pcPacketContainer = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.pcSeedBankTexture)
		{
			pcSeedBankTexture = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.pcSunBankTexture)
		{
			pcSunBankTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.mobileControl)
		{
			mobileControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.mobileSunLabel)
		{
			mobileSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.mobileSeedContainer)
		{
			mobileSeedContainer = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.mobileItemContainer)
		{
			mobileItemContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.mobilePacketSlotContainer)
		{
			mobilePacketSlotContainer = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		if (name == PropertyName.mobilePacketContainer)
		{
			mobilePacketContainer = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.mobileSunBarTexture)
		{
			mobileSunBarTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.mobileISeedContain)
		{
			mobileISeedContain = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName._sunNumShow)
		{
			_sunNumShow = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sunDisplayLabel)
		{
			_sunDisplayLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._sunDisplayValue)
		{
			_sunDisplayValue = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._sunDisplayInitialized)
		{
			_sunDisplayInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetNum)
		{
			packetNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._packetPool)
		{
			_packetPool = VariantUtils.ConvertToArray<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.packetNameSet)
		{
			packetNameSet = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.packetColdDownState)
		{
			packetColdDownState = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.hasGameStarted)
		{
			hasGameStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mobilePreset)
		{
			mobilePreset = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._referencesInitialized)
		{
			_referencesInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._layoutRefreshTree)
		{
			_layoutRefreshTree = VariantUtils.ConvertTo<SceneTree>(in value);
			return true;
		}
		if (name == PropertyName._layoutRefreshPending)
		{
			_layoutRefreshPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunLabel)
		{
			sunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.itemContainer)
		{
			itemContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.packetSlotContainer)
		{
			packetSlotContainer = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			packetContainer = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.seedContainer)
		{
			seedContainer = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.sunBankTexture)
		{
			sunBankTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.animationPlayer)
		{
			animationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HasSunAccount)
		{
			value = VariantUtils.CreateFrom<bool>(HasSunAccount);
			return true;
		}
		if (name == PropertyName.sunNumShow)
		{
			value = VariantUtils.CreateFrom<double>(sunNumShow);
			return true;
		}
		if (name == PropertyName.pcControl)
		{
			value = VariantUtils.CreateFrom(in pcControl);
			return true;
		}
		if (name == PropertyName.pcSunLabel)
		{
			value = VariantUtils.CreateFrom(in pcSunLabel);
			return true;
		}
		if (name == PropertyName.pcSeedContainer)
		{
			value = VariantUtils.CreateFrom(in pcSeedContainer);
			return true;
		}
		if (name == PropertyName.pcItemContainer)
		{
			value = VariantUtils.CreateFrom(in pcItemContainer);
			return true;
		}
		if (name == PropertyName.pcPacketSlotContainer)
		{
			value = VariantUtils.CreateFrom(in pcPacketSlotContainer);
			return true;
		}
		if (name == PropertyName.pcPacketContainer)
		{
			value = VariantUtils.CreateFrom(in pcPacketContainer);
			return true;
		}
		if (name == PropertyName.pcSeedBankTexture)
		{
			value = VariantUtils.CreateFrom(in pcSeedBankTexture);
			return true;
		}
		if (name == PropertyName.pcSunBankTexture)
		{
			value = VariantUtils.CreateFrom(in pcSunBankTexture);
			return true;
		}
		if (name == PropertyName.mobileControl)
		{
			value = VariantUtils.CreateFrom(in mobileControl);
			return true;
		}
		if (name == PropertyName.mobileSunLabel)
		{
			value = VariantUtils.CreateFrom(in mobileSunLabel);
			return true;
		}
		if (name == PropertyName.mobileSeedContainer)
		{
			value = VariantUtils.CreateFrom(in mobileSeedContainer);
			return true;
		}
		if (name == PropertyName.mobileItemContainer)
		{
			value = VariantUtils.CreateFrom(in mobileItemContainer);
			return true;
		}
		if (name == PropertyName.mobilePacketSlotContainer)
		{
			value = VariantUtils.CreateFrom(in mobilePacketSlotContainer);
			return true;
		}
		if (name == PropertyName.mobilePacketContainer)
		{
			value = VariantUtils.CreateFrom(in mobilePacketContainer);
			return true;
		}
		if (name == PropertyName.mobileSunBarTexture)
		{
			value = VariantUtils.CreateFrom(in mobileSunBarTexture);
			return true;
		}
		if (name == PropertyName.mobileISeedContain)
		{
			value = VariantUtils.CreateFrom(in mobileISeedContain);
			return true;
		}
		if (name == PropertyName._sunNumShow)
		{
			value = VariantUtils.CreateFrom(in _sunNumShow);
			return true;
		}
		if (name == PropertyName._sunDisplayLabel)
		{
			value = VariantUtils.CreateFrom(in _sunDisplayLabel);
			return true;
		}
		if (name == PropertyName._sunDisplayValue)
		{
			value = VariantUtils.CreateFrom(in _sunDisplayValue);
			return true;
		}
		if (name == PropertyName._sunDisplayInitialized)
		{
			value = VariantUtils.CreateFrom(in _sunDisplayInitialized);
			return true;
		}
		if (name == PropertyName.packetNum)
		{
			value = VariantUtils.CreateFrom(in packetNum);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		if (name == PropertyName._packetPool)
		{
			value = VariantUtils.CreateFromArray(_packetPool);
			return true;
		}
		if (name == PropertyName.packetNameSet)
		{
			value = VariantUtils.CreateFrom(in packetNameSet);
			return true;
		}
		if (name == PropertyName.packetColdDownState)
		{
			value = VariantUtils.CreateFrom(in packetColdDownState);
			return true;
		}
		if (name == PropertyName.hasGameStarted)
		{
			value = VariantUtils.CreateFrom(in hasGameStarted);
			return true;
		}
		if (name == PropertyName.mobilePreset)
		{
			value = VariantUtils.CreateFrom(in mobilePreset);
			return true;
		}
		if (name == PropertyName._referencesInitialized)
		{
			value = VariantUtils.CreateFrom(in _referencesInitialized);
			return true;
		}
		if (name == PropertyName._layoutRefreshTree)
		{
			value = VariantUtils.CreateFrom(in _layoutRefreshTree);
			return true;
		}
		if (name == PropertyName._layoutRefreshPending)
		{
			value = VariantUtils.CreateFrom(in _layoutRefreshPending);
			return true;
		}
		if (name == PropertyName.sunLabel)
		{
			value = VariantUtils.CreateFrom(in sunLabel);
			return true;
		}
		if (name == PropertyName.itemContainer)
		{
			value = VariantUtils.CreateFrom(in itemContainer);
			return true;
		}
		if (name == PropertyName.packetSlotContainer)
		{
			value = VariantUtils.CreateFrom(in packetSlotContainer);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			value = VariantUtils.CreateFrom(in packetContainer);
			return true;
		}
		if (name == PropertyName.seedContainer)
		{
			value = VariantUtils.CreateFrom(in seedContainer);
			return true;
		}
		if (name == PropertyName.sunBankTexture)
		{
			value = VariantUtils.CreateFrom(in sunBankTexture);
			return true;
		}
		if (name == PropertyName.animationPlayer)
		{
			value = VariantUtils.CreateFrom(in animationPlayer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.pcControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcSeedContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcItemContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcPacketSlotContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcPacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcSeedBankTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcSunBankTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileSeedContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileItemContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobilePacketSlotContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobilePacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileSunBarTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileISeedContain, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._sunNumShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunDisplayLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sunDisplayValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sunDisplayInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasSunAccount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.sunNumShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.packetNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._packetPool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.packetNameSet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.packetColdDownState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasGameStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mobilePreset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._referencesInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutRefreshTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._layoutRefreshPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.itemContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetSlotContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sunBankTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.animationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sunNumShow, Variant.From<double>(sunNumShow));
		info.AddProperty(PropertyName.pcControl, Variant.From(in pcControl));
		info.AddProperty(PropertyName.pcSunLabel, Variant.From(in pcSunLabel));
		info.AddProperty(PropertyName.pcSeedContainer, Variant.From(in pcSeedContainer));
		info.AddProperty(PropertyName.pcItemContainer, Variant.From(in pcItemContainer));
		info.AddProperty(PropertyName.pcPacketSlotContainer, Variant.From(in pcPacketSlotContainer));
		info.AddProperty(PropertyName.pcPacketContainer, Variant.From(in pcPacketContainer));
		info.AddProperty(PropertyName.pcSeedBankTexture, Variant.From(in pcSeedBankTexture));
		info.AddProperty(PropertyName.pcSunBankTexture, Variant.From(in pcSunBankTexture));
		info.AddProperty(PropertyName.mobileControl, Variant.From(in mobileControl));
		info.AddProperty(PropertyName.mobileSunLabel, Variant.From(in mobileSunLabel));
		info.AddProperty(PropertyName.mobileSeedContainer, Variant.From(in mobileSeedContainer));
		info.AddProperty(PropertyName.mobileItemContainer, Variant.From(in mobileItemContainer));
		info.AddProperty(PropertyName.mobilePacketSlotContainer, Variant.From(in mobilePacketSlotContainer));
		info.AddProperty(PropertyName.mobilePacketContainer, Variant.From(in mobilePacketContainer));
		info.AddProperty(PropertyName.mobileSunBarTexture, Variant.From(in mobileSunBarTexture));
		info.AddProperty(PropertyName.mobileISeedContain, Variant.From(in mobileISeedContain));
		info.AddProperty(PropertyName._sunNumShow, Variant.From(in _sunNumShow));
		info.AddProperty(PropertyName._sunDisplayLabel, Variant.From(in _sunDisplayLabel));
		info.AddProperty(PropertyName._sunDisplayValue, Variant.From(in _sunDisplayValue));
		info.AddProperty(PropertyName._sunDisplayInitialized, Variant.From(in _sunDisplayInitialized));
		info.AddProperty(PropertyName.packetNum, Variant.From(in packetNum));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
		info.AddProperty(PropertyName._packetPool, Variant.CreateFrom(_packetPool));
		info.AddProperty(PropertyName.packetNameSet, Variant.From(in packetNameSet));
		info.AddProperty(PropertyName.packetColdDownState, Variant.From(in packetColdDownState));
		info.AddProperty(PropertyName.hasGameStarted, Variant.From(in hasGameStarted));
		info.AddProperty(PropertyName.mobilePreset, Variant.From(in mobilePreset));
		info.AddProperty(PropertyName._referencesInitialized, Variant.From(in _referencesInitialized));
		info.AddProperty(PropertyName._layoutRefreshTree, Variant.From(in _layoutRefreshTree));
		info.AddProperty(PropertyName._layoutRefreshPending, Variant.From(in _layoutRefreshPending));
		info.AddProperty(PropertyName.sunLabel, Variant.From(in sunLabel));
		info.AddProperty(PropertyName.itemContainer, Variant.From(in itemContainer));
		info.AddProperty(PropertyName.packetSlotContainer, Variant.From(in packetSlotContainer));
		info.AddProperty(PropertyName.packetContainer, Variant.From(in packetContainer));
		info.AddProperty(PropertyName.seedContainer, Variant.From(in seedContainer));
		info.AddProperty(PropertyName.sunBankTexture, Variant.From(in sunBankTexture));
		info.AddProperty(PropertyName.animationPlayer, Variant.From(in animationPlayer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sunNumShow, out var value))
		{
			sunNumShow = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.pcControl, out var value2))
		{
			pcControl = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.pcSunLabel, out var value3))
		{
			pcSunLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.pcSeedContainer, out var value4))
		{
			pcSeedContainer = value4.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.pcItemContainer, out var value5))
		{
			pcItemContainer = value5.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.pcPacketSlotContainer, out var value6))
		{
			pcPacketSlotContainer = value6.As<CanvasItem>();
		}
		if (info.TryGetProperty(PropertyName.pcPacketContainer, out var value7))
		{
			pcPacketContainer = value7.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.pcSeedBankTexture, out var value8))
		{
			pcSeedBankTexture = value8.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.pcSunBankTexture, out var value9))
		{
			pcSunBankTexture = value9.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.mobileControl, out var value10))
		{
			mobileControl = value10.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.mobileSunLabel, out var value11))
		{
			mobileSunLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.mobileSeedContainer, out var value12))
		{
			mobileSeedContainer = value12.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.mobileItemContainer, out var value13))
		{
			mobileItemContainer = value13.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.mobilePacketSlotContainer, out var value14))
		{
			mobilePacketSlotContainer = value14.As<CanvasItem>();
		}
		if (info.TryGetProperty(PropertyName.mobilePacketContainer, out var value15))
		{
			mobilePacketContainer = value15.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.mobileSunBarTexture, out var value16))
		{
			mobileSunBarTexture = value16.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.mobileISeedContain, out var value17))
		{
			mobileISeedContain = value17.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName._sunNumShow, out var value18))
		{
			_sunNumShow = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunDisplayLabel, out var value19))
		{
			_sunDisplayLabel = value19.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._sunDisplayValue, out var value20))
		{
			_sunDisplayValue = value20.As<long>();
		}
		if (info.TryGetProperty(PropertyName._sunDisplayInitialized, out var value21))
		{
			_sunDisplayInitialized = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetNum, out var value22))
		{
			packetNum = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value23))
		{
			packetList = value23.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._packetPool, out var value24))
		{
			_packetPool = value24.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.packetNameSet, out var value25))
		{
			packetNameSet = value25.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.packetColdDownState, out var value26))
		{
			packetColdDownState = value26.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.hasGameStarted, out var value27))
		{
			hasGameStarted = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mobilePreset, out var value28))
		{
			mobilePreset = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._referencesInitialized, out var value29))
		{
			_referencesInitialized = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._layoutRefreshTree, out var value30))
		{
			_layoutRefreshTree = value30.As<SceneTree>();
		}
		if (info.TryGetProperty(PropertyName._layoutRefreshPending, out var value31))
		{
			_layoutRefreshPending = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunLabel, out var value32))
		{
			sunLabel = value32.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.itemContainer, out var value33))
		{
			itemContainer = value33.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.packetSlotContainer, out var value34))
		{
			packetSlotContainer = value34.As<CanvasItem>();
		}
		if (info.TryGetProperty(PropertyName.packetContainer, out var value35))
		{
			packetContainer = value35.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.seedContainer, out var value36))
		{
			seedContainer = value36.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.sunBankTexture, out var value37))
		{
			sunBankTexture = value37.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.animationPlayer, out var value38))
		{
			animationPlayer = value38.As<AnimationPlayer>();
		}
	}
}
