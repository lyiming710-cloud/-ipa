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
[ScriptPath("res://Prefab/GUI/DialogBox/Almanac/Almanac.cs")]
public class Almanac : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public static readonly StringName MobilePreset = "MobilePreset";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectButtons = "ConnectButtons";

		public static readonly StringName InitPlant = "InitPlant";

		public static readonly StringName PlantLoveChange = "PlantLoveChange";

		public static readonly StringName PlantPacketChoose = "PlantPacketChoose";

		public static readonly StringName PlantInformationSet = "PlantInformationSet";

		public static readonly StringName PlantNextButtonPressed = "PlantNextButtonPressed";

		public static readonly StringName PlantPreButtonPressed = "PlantPreButtonPressed";

		public static readonly StringName PlantButtonPressed = "PlantButtonPressed";

		public static readonly StringName ShowButtonPressed = "ShowButtonPressed";

		public static readonly StringName InitZombie = "InitZombie";

		public static readonly StringName ZombiePacketChoose = "ZombiePacketChoose";

		public static readonly StringName ZombieInformationSet = "ZombieInformationSet";

		public static readonly StringName ZombieButtonPressed = "ZombieButtonPressed";

		public static readonly StringName OnPlantScrollChanged = "OnPlantScrollChanged";

		public static readonly StringName OnZombieScrollChanged = "OnZombieScrollChanged";

		public static readonly StringName OnViewportSizeChanged = "OnViewportSizeChanged";

		public static readonly StringName QueuePlantVirtualRefresh = "QueuePlantVirtualRefresh";

		public static readonly StringName QueueZombieVirtualRefresh = "QueueZombieVirtualRefresh";

		public static readonly StringName RefreshPlantVirtualBindings = "RefreshPlantVirtualBindings";

		public static readonly StringName RefreshZombieVirtualBindings = "RefreshZombieVirtualBindings";

		public static readonly StringName CanQueueVirtualRefresh = "CanQueueVirtualRefresh";

		public static readonly StringName GetVisibleRowCount = "GetVisibleRowCount";

		public static readonly StringName UpdatePlantContentSize = "UpdatePlantContentSize";

		public static readonly StringName UpdateZombieContentSize = "UpdateZombieContentSize";

		public static readonly StringName GetPlantColumnStride = "GetPlantColumnStride";

		public static readonly StringName GetPlantRowStride = "GetPlantRowStride";

		public static readonly StringName GetPlantColumnCount = "GetPlantColumnCount";

		public static readonly StringName GetZombieColumnCount = "GetZombieColumnCount";

		public static readonly StringName GetVirtualContentWidth = "GetVirtualContentWidth";

		public static readonly StringName GetColumnCount = "GetColumnCount";

		public static readonly StringName PositionVirtualPreview = "PositionVirtualPreview";

		public static readonly StringName TakePlantPreview = "TakePlantPreview";

		public static readonly StringName BindPlantPreview = "BindPlantPreview";

		public static readonly StringName TakeZombiePreview = "TakeZombiePreview";

		public static readonly StringName BindZombiePreview = "BindZombiePreview";

		public static readonly StringName RecyclePlantPreviewsOutside = "RecyclePlantPreviewsOutside";

		public static readonly StringName RecycleZombiePreviewsOutside = "RecycleZombiePreviewsOutside";

		public static readonly StringName TrimPlantPreviewPoolToLimit = "TrimPlantPreviewPoolToLimit";

		public static readonly StringName TrimZombiePreviewPoolToLimit = "TrimZombiePreviewPoolToLimit";

		public static readonly StringName ReturnPlantPreviewToPool = "ReturnPlantPreviewToPool";

		public static readonly StringName ReturnZombiePreviewToPool = "ReturnZombiePreviewToPool";

		public static readonly StringName ReturnAllPlantPreviewsToPool = "ReturnAllPlantPreviewsToPool";

		public static readonly StringName ReturnAllZombiePreviewsToPool = "ReturnAllZombiePreviewsToPool";

		public static readonly StringName InitProp = "InitProp";

		public static readonly StringName PropShovelChoose = "PropShovelChoose";

		public static readonly StringName PropMowerChoose = "PropMowerChoose";

		public static readonly StringName SetPropUseButtonState = "SetPropUseButtonState";

		public static readonly StringName PropShovelInformationSet = "PropShovelInformationSet";

		public static readonly StringName PropMowerInformationSet = "PropMowerInformationSet";

		public static readonly StringName PropUseButtonPressed = "PropUseButtonPressed";

		public static readonly StringName PropButtonPressed = "PropButtonPressed";

		public static readonly StringName ButtonPressed = "ButtonPressed";

		public static readonly StringName IndexButtonPressed = "IndexButtonPressed";

		public static readonly StringName CloseButtonPressed = "CloseButtonPressed";

		public new static readonly StringName SetLightMask = "SetLightMask";

		public static readonly StringName SetIndexPreviewRender = "SetIndexPreviewRender";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName PlantLogicalEntryCount = "PlantLogicalEntryCount";

		public static readonly StringName ZombieLogicalEntryCount = "ZombieLogicalEntryCount";

		public static readonly StringName PlantPreviewNodeCount = "PlantPreviewNodeCount";

		public static readonly StringName ZombiePreviewNodeCount = "ZombiePreviewNodeCount";

		public static readonly StringName ArePlantVirtualBindingsStable = "ArePlantVirtualBindingsStable";

		public static readonly StringName AreZombieVirtualBindingsStable = "AreZombieVirtualBindingsStable";

		public static readonly StringName IsPropCategoryInitialized = "IsPropCategoryInitialized";

		public static readonly StringName PlantPreviewNodeLimit = "PlantPreviewNodeLimit";

		public static readonly StringName ZombiePreviewNodeLimit = "ZombiePreviewNodeLimit";

		public static readonly StringName PlantColumnCount = "PlantColumnCount";

		public static readonly StringName ZombieColumnCount = "ZombieColumnCount";

		public static readonly StringName _plantInitialized = "_plantInitialized";

		public static readonly StringName _zombieInitialized = "_zombieInitialized";

		public static readonly StringName _propInitialized = "_propInitialized";

		public static readonly StringName _plantRefreshQueued = "_plantRefreshQueued";

		public static readonly StringName _zombieRefreshQueued = "_zombieRefreshQueued";

		public static readonly StringName _mobilePreset = "_mobilePreset";

		public static readonly StringName _plantBindingGeneration = "_plantBindingGeneration";

		public static readonly StringName _zombieBindingGeneration = "_zombieBindingGeneration";

		public static readonly StringName _plantBindingOperationCount = "_plantBindingOperationCount";

		public static readonly StringName _zombieBindingOperationCount = "_zombieBindingOperationCount";

		public static readonly StringName _plantRefreshRequestedWhileBinding = "_plantRefreshRequestedWhileBinding";

		public static readonly StringName _zombieRefreshRequestedWhileBinding = "_zombieRefreshRequestedWhileBinding";

		public static readonly StringName _layoutViewport = "_layoutViewport";

		public static readonly StringName _plantLayoutRoot = "_plantLayoutRoot";

		public static readonly StringName _zombieLayoutRoot = "_zombieLayoutRoot";

		public static readonly StringName _zombiePacketMargin = "_zombiePacketMargin";

		public static readonly StringName plantPacketScroll = "plantPacketScroll";

		public static readonly StringName packetMargin = "packetMargin";

		public static readonly StringName plantLayer = "plantLayer";

		public static readonly StringName plantPacketContainer = "plantPacketContainer";

		public static readonly StringName plantInformationPanel = "plantInformationPanel";

		public static readonly StringName plantShowLabel = "plantShowLabel";

		public static readonly StringName zombieLayer = "zombieLayer";

		public static readonly StringName zombiePacketScroll = "zombiePacketScroll";

		public static readonly StringName zombiePacketContainer = "zombiePacketContainer";

		public static readonly StringName zombieInformationPanel = "zombieInformationPanel";

		public static readonly StringName propLayer = "propLayer";

		public static readonly StringName propShovelContainer = "propShovelContainer";

		public static readonly StringName propMowerContainer = "propMowerContainer";

		public static readonly StringName propInformationPanel = "propInformationPanel";

		public static readonly StringName propUseButton = "propUseButton";

		public static readonly StringName plantPacketBank = "plantPacketBank";

		public static readonly StringName zombiePacketBank = "zombiePacketBank";

		public static readonly StringName plantCategoryId = "plantCategoryId";

		public static readonly StringName plantCurrentSelect = "plantCurrentSelect";

		public static readonly StringName plantShowAll = "plantShowAll";

		public static readonly StringName propType = "propType";

		public static readonly StringName propKey = "propKey";

		public static readonly StringName audio = "audio";

		public static readonly StringName isPlaying = "isPlaying";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private const float PlantDesktopColumnStride = 52f;

	private const float PlantMobileColumnStride = 98f;

	private const float PlantDesktopRowStride = 75f;

	private const float PlantMobileRowStride = 62f;

	private const float ZombieColumnStride = 75f;

	private const float ZombieRowStride = 75f;

	private static PackedScene _almanacZombieWidow;

	private static PackedScene _almanacPropWidow;

	private readonly List<TowerDefensePacketConfig> _plantLogicalConfigs = new List<TowerDefensePacketConfig>();

	private readonly List<TowerDefensePacketConfig> _zombieLogicalConfigs = new List<TowerDefensePacketConfig>();

	private readonly System.Collections.Generic.Dictionary<int, TowerDefenseInGamePacketShow> _visiblePlantPreviews = new System.Collections.Generic.Dictionary<int, TowerDefenseInGamePacketShow>();

	private readonly System.Collections.Generic.Dictionary<int, AlmanacZombieWidow> _visibleZombiePreviews = new System.Collections.Generic.Dictionary<int, AlmanacZombieWidow>();

	private readonly List<TowerDefenseInGamePacketShow> _plantPreviewPool = new List<TowerDefenseInGamePacketShow>();

	private readonly List<AlmanacZombieWidow> _zombiePreviewPool = new List<AlmanacZombieWidow>();

	private bool _plantInitialized;

	private bool _zombieInitialized;

	private bool _propInitialized;

	private bool _plantRefreshQueued;

	private bool _zombieRefreshQueued;

	private bool _mobilePreset;

	private int _plantBindingGeneration;

	private int _zombieBindingGeneration;

	private int _plantBindingOperationCount;

	private int _zombieBindingOperationCount;

	private bool _plantRefreshRequestedWhileBinding;

	private bool _zombieRefreshRequestedWhileBinding;

	private Viewport _layoutViewport;

	private Control _plantLayoutRoot;

	private Control _zombieLayoutRoot;

	private MarginContainer _zombiePacketMargin;

	public ScrollContainer plantPacketScroll;

	public MarginContainer packetMargin;

	public CanvasLayer plantLayer;

	public Control plantPacketContainer;

	public InformationPanel plantInformationPanel;

	public Label plantShowLabel;

	public CanvasLayer zombieLayer;

	public ScrollContainer zombiePacketScroll;

	public Control zombiePacketContainer;

	public InformationPanel zombieInformationPanel;

	public CanvasLayer propLayer;

	public GridContainer propShovelContainer;

	public GridContainer propMowerContainer;

	public InformationPanel propInformationPanel;

	public NinePatchButtonBase propUseButton;

	public TowerDefensePacketBankData plantPacketBank;

	public TowerDefensePacketBankData zombiePacketBank;

	public int plantCategoryId;

	public TowerDefenseInGamePacketShow plantCurrentSelect;

	public bool plantShowAll;

	public string propType = "Shovel";

	public string propKey = string.Empty;

	public AudioStreamPlayerMember audio;

	public bool isPlaying;

	private static PackedScene ALMANAC_ZOMBIE_WIDOW => _almanacZombieWidow ?? (_almanacZombieWidow = GD.Load<PackedScene>("uid://r4o1fp1gc7bg"));

	private static PackedScene ALMANAC_PROP_WIDOW => _almanacPropWidow ?? (_almanacPropWidow = GD.Load<PackedScene>("uid://uhke44e0fue6"));

	public int PlantLogicalEntryCount => _plantLogicalConfigs.Count;

	public int ZombieLogicalEntryCount => _zombieLogicalConfigs.Count;

	public int PlantPreviewNodeCount => _visiblePlantPreviews.Count + _plantPreviewPool.Count;

	public int ZombiePreviewNodeCount => _visibleZombiePreviews.Count + _zombiePreviewPool.Count;

	public bool ArePlantVirtualBindingsStable
	{
		get
		{
			if (_plantBindingOperationCount == 0 && !_plantRefreshQueued)
			{
				return !_plantRefreshRequestedWhileBinding;
			}
			return false;
		}
	}

	public bool AreZombieVirtualBindingsStable
	{
		get
		{
			if (_zombieBindingOperationCount == 0 && !_zombieRefreshQueued)
			{
				return !_zombieRefreshRequestedWhileBinding;
			}
			return false;
		}
	}

	public bool IsPropCategoryInitialized => _propInitialized;

	public int PlantPreviewNodeLimit => GetPlantColumnCount() * (GetVisibleRowCount(plantPacketScroll, GetPlantRowStride()) + 2);

	public int ZombiePreviewNodeLimit => GetZombieColumnCount() * (GetVisibleRowCount(zombiePacketScroll, 75f) + 2);

	public int PlantColumnCount => GetPlantColumnCount();

	public int ZombieColumnCount => GetZombieColumnCount();

	public void MobilePreset()
	{
		_mobilePreset = true;
		packetMargin.AddThemeConstantOverride("margin_left", 48);
		packetMargin.AddThemeConstantOverride("margin_top", 30);
		packetMargin.AddThemeConstantOverride("margin_right", 48);
		packetMargin.AddThemeConstantOverride("margin_down", 30);
		QueuePlantVirtualRefresh();
		QueueZombieVirtualRefresh();
	}

	public override void _EnterTree()
	{
		audio = AudioManager.Instance.MemberFind("ChooseYourSeeds", AudioManagerEnum.TYPE.MUSIC);
		audio.ProcessMode = ProcessModeEnum.Always;
		isPlaying = audio.Playing;
	}

	public override void _Ready()
	{
		base._Ready();
		ResourceManager.Instance.RequireFullGameplayResourcesReady("Almanac");
		_ = ALMANAC_ZOMBIE_WIDOW;
		_ = ALMANAC_PROP_WIDOW;
		plantPacketScroll = GetNode<ScrollContainer>("PlantLayer/Plant/PacketScroll");
		_plantLayoutRoot = GetNode<Control>("PlantLayer/Plant");
		packetMargin = GetNode<MarginContainer>("%PacketMargin");
		plantLayer = GetNode<CanvasLayer>("%PlantLayer");
		plantPacketContainer = GetNode<Control>("%PacketContainer");
		plantInformationPanel = GetNode<InformationPanel>("%PlantInformationPanel");
		plantShowLabel = GetNode<Label>("%PlantShowLabel");
		zombieLayer = GetNode<CanvasLayer>("%ZombieLayer");
		zombiePacketScroll = GetNode<ScrollContainer>("ZombieLayer/Zombie/ScrollContainer");
		_zombieLayoutRoot = GetNode<Control>("ZombieLayer/Zombie");
		_zombiePacketMargin = GetNode<MarginContainer>("ZombieLayer/Zombie/ScrollContainer/MarginContainer");
		zombiePacketContainer = GetNode<Control>("%ZombiePacketContainer");
		zombieInformationPanel = GetNode<InformationPanel>("%ZombieInformationPanel");
		propLayer = GetNode<CanvasLayer>("%PropLayer");
		propShovelContainer = GetNode<GridContainer>("%PropShovelContainer");
		propMowerContainer = GetNode<GridContainer>("%PropMowerContainer");
		propInformationPanel = GetNode<InformationPanel>("%PropInformationPanel");
		propUseButton = GetNode<NinePatchButtonBase>("%PropUseButton");
		ConnectButtons();
		plantPacketScroll.GetVScrollBar().ValueChanged += OnPlantScrollChanged;
		zombiePacketScroll.GetVScrollBar().ValueChanged += OnZombieScrollChanged;
		_layoutViewport = GetViewport();
		_layoutViewport.SizeChanged += OnViewportSizeChanged;
		Resized += OnViewportSizeChanged;
		_plantLayoutRoot.Resized += OnViewportSizeChanged;
		_zombieLayoutRoot.Resized += OnViewportSizeChanged;
		if (GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool())
		{
			MobilePreset();
		}
		plantPacketBank = XWModContentCatalog.WithPlants(TowerDefenseManager.GetPacketBankData("GeneralPlant"));
		zombiePacketBank = TowerDefenseManager.GetPacketBankData("GeneralZombie");
		if (!isPlaying)
		{
			audio.Play();
		}
		Node node = GetNode("ChooseLayer/Choose/PlantNode");
		Node node2 = GetNode("ChooseLayer/Choose/ZombieNode");
		SetLightMask(node);
		SetLightMask(node2);
		SetIndexPreviewRender(node);
		SetIndexPreviewRender(node2);
	}

	public override void _ExitTree()
	{
		_plantBindingGeneration++;
		_zombieBindingGeneration++;
		_plantRefreshQueued = false;
		_zombieRefreshQueued = false;
		_plantRefreshRequestedWhileBinding = false;
		_zombieRefreshRequestedWhileBinding = false;
		if (GodotObject.IsInstanceValid(plantPacketScroll))
		{
			plantPacketScroll.GetVScrollBar().ValueChanged -= OnPlantScrollChanged;
		}
		if (GodotObject.IsInstanceValid(zombiePacketScroll))
		{
			zombiePacketScroll.GetVScrollBar().ValueChanged -= OnZombieScrollChanged;
		}
		if (GodotObject.IsInstanceValid(_layoutViewport))
		{
			_layoutViewport.SizeChanged -= OnViewportSizeChanged;
		}
		Resized -= OnViewportSizeChanged;
		if (GodotObject.IsInstanceValid(_plantLayoutRoot))
		{
			_plantLayoutRoot.Resized -= OnViewportSizeChanged;
		}
		if (GodotObject.IsInstanceValid(_zombieLayoutRoot))
		{
			_zombieLayoutRoot.Resized -= OnViewportSizeChanged;
		}
		_layoutViewport = null;
		_plantLayoutRoot = null;
		_zombieLayoutRoot = null;
		_zombiePacketMargin = null;
		foreach (TowerDefenseInGamePacketShow value in _visiblePlantPreviews.Values)
		{
			value?.ClearEventHandlers();
		}
		foreach (AlmanacZombieWidow value2 in _visibleZombiePreviews.Values)
		{
			value2?.ClearEventHandlers();
		}
		base._ExitTree();
	}

	private void ConnectButtons()
	{
		NinePatchButtonBase node = GetNode<NinePatchButtonBase>("ChooseLayer/Choose/PlantButton");
		node.OnPressed += PlantButtonPressed;
		node.OnPressed += ButtonPressed;
		NinePatchButtonBase node2 = GetNode<NinePatchButtonBase>("ChooseLayer/Choose/ZombieButton");
		node2.OnPressed += ZombieButtonPressed;
		node2.OnPressed += ButtonPressed;
		NinePatchButtonBase node3 = GetNode<NinePatchButtonBase>("ChooseLayer/Choose/PropButton");
		node3.OnPressed += PropButtonPressed;
		node3.OnPressed += ButtonPressed;
		TextureButton node4 = GetNode<TextureButton>("PlantLayer/Plant/IndexButton");
		node4.Pressed += IndexButtonPressed;
		node4.Pressed += ButtonPressed;
		TextureButton node5 = GetNode<TextureButton>("PlantLayer/Plant/PlantPreButton");
		node5.Pressed += PlantPreButtonPressed;
		node5.Pressed += ButtonPressed;
		TextureButton node6 = GetNode<TextureButton>("PlantLayer/Plant/PlantNextButton");
		node6.Pressed += PlantNextButtonPressed;
		node6.Pressed += ButtonPressed;
		GetNode<TextureButton>("PlantLayer/Plant/ShowButton").Pressed += ShowButtonPressed;
		TextureButton node7 = GetNode<TextureButton>("ZombieLayer/Zombie/IndexButton");
		node7.Pressed += IndexButtonPressed;
		node7.Pressed += ButtonPressed;
		TextureButton node8 = GetNode<TextureButton>("PropLayer/IndexButton");
		node8.Pressed += IndexButtonPressed;
		node8.Pressed += ButtonPressed;
		propUseButton.OnPressed += PropUseButtonPressed;
		propUseButton.OnPressed += ButtonPressed;
		TextureButton node9 = GetNode<TextureButton>("ButtonLayer/Button/CloseButton");
		node9.Pressed += CloseButtonPressed;
		node9.Pressed += ButtonPressed;
	}

	public async void InitPlant()
	{
		int generation = ++_plantBindingGeneration;
		ReturnAllPlantPreviewsToPool();
		_plantLogicalConfigs.Clear();
		plantCurrentSelect = null;
		if (!GodotObject.IsInstanceValid(plantPacketBank) || plantCategoryId >= plantPacketBank.category.Count)
		{
			UpdatePlantContentSize();
			TrimPlantPreviewPoolToLimit();
			return;
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Variant key in plantPacketBank.category.Keys)
		{
			array.Add(key);
		}
		Godot.Collections.Array array2 = plantPacketBank.category[array[plantCategoryId]].AsGodotArray();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (Variant item in array2)
		{
			string text = item.AsString();
			if (XWModPlayerProgressService.GetPacketState(text).GetValueOrDefault("Love", false).AsBool())
			{
				list.Add(text);
			}
			else
			{
				list2.Add(text);
			}
		}
		list.AddRange(list2);
		foreach (string item2 in list)
		{
			TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(item2);
			if (GodotObject.IsInstanceValid(packetConfigReadOnly) && (plantShowAll || packetConfigReadOnly.Unlock()))
			{
				_plantLogicalConfigs.Add(packetConfigReadOnly);
			}
		}
		_plantInitialized = true;
		plantPacketScroll.ScrollVertical = 0;
		UpdatePlantContentSize();
		await PopulateInitialPlantBindingsAsync(generation);
	}

	public void PlantLoveChange(TowerDefenseInGamePacketShow packet)
	{
		InitPlant();
	}

	public void PlantPacketChoose(TowerDefenseInGamePacketShow packet)
	{
		if (plantCurrentSelect != packet)
		{
			plantCurrentSelect?.Reset();
			PlantInformationSet(packet);
			plantCurrentSelect = packet;
		}
	}

	public void PlantInformationSet(TowerDefenseInGamePacketShow packet)
	{
		plantInformationPanel.InitPacket(packet.config);
	}

	public void PlantNextButtonPressed()
	{
		plantCategoryId = (plantCategoryId + 1) % plantPacketBank.category.Count;
		InitPlant();
	}

	public void PlantPreButtonPressed()
	{
		int count = plantPacketBank.category.Count;
		plantCategoryId = ((plantCategoryId - 1) % count + count) % count;
		InitPlant();
	}

	public void PlantButtonPressed()
	{
		plantLayer.Visible = true;
		if (!_plantInitialized)
		{
			InitPlant();
		}
		else
		{
			QueuePlantVirtualRefresh();
		}
	}

	public void ShowButtonPressed()
	{
		plantShowAll = !plantShowAll;
		plantShowLabel.Text = (plantShowAll ? "关闭显示全部" : "显示全部");
		InitPlant();
	}

	public async void InitZombie()
	{
		int generation = ++_zombieBindingGeneration;
		ReturnAllZombiePreviewsToPool();
		_zombieLogicalConfigs.Clear();
		if (GodotObject.IsInstanceValid(zombiePacketBank))
		{
			foreach (Variant item in zombiePacketBank.GetCategory("Zombie"))
			{
				TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(item.AsString());
				if (GodotObject.IsInstanceValid(packetConfigReadOnly))
				{
					_zombieLogicalConfigs.Add(packetConfigReadOnly);
				}
			}
		}
		foreach (XWModContentCatalog.Packet packet in XWModContentCatalog.GetPackets(plants: false))
		{
			_zombieLogicalConfigs.Add(packet.Config);
		}
		_zombieInitialized = true;
		zombiePacketScroll.ScrollVertical = 0;
		UpdateZombieContentSize();
		await PopulateInitialZombieBindingsAsync(generation);
	}

	public void ZombiePacketChoose(TowerDefensePacketConfig config)
	{
		ZombieInformationSet(config);
	}

	public void ZombieInformationSet(TowerDefensePacketConfig config)
	{
		zombieInformationPanel.InitPacket(config);
	}

	public void ZombieButtonPressed()
	{
		zombieLayer.Visible = true;
		if (!_zombieInitialized)
		{
			InitZombie();
		}
		else
		{
			QueueZombieVirtualRefresh();
		}
	}

	private void OnPlantScrollChanged(double value)
	{
		QueuePlantVirtualRefresh();
	}

	private void OnZombieScrollChanged(double value)
	{
		QueueZombieVirtualRefresh();
	}

	private void OnViewportSizeChanged()
	{
		QueuePlantVirtualRefresh();
		QueueZombieVirtualRefresh();
	}

	public void QueuePlantVirtualRefresh()
	{
		if (!_plantRefreshQueued && IsInsideTree() && _plantInitialized)
		{
			_plantRefreshQueued = true;
			CallDeferred("RefreshPlantVirtualBindings");
		}
	}

	public void QueueZombieVirtualRefresh()
	{
		if (!_zombieRefreshQueued && IsInsideTree() && _zombieInitialized)
		{
			_zombieRefreshQueued = true;
			CallDeferred("RefreshZombieVirtualBindings");
		}
	}

	public async void RefreshPlantVirtualBindings()
	{
		_plantRefreshQueued = false;
		if (!_plantInitialized || !GodotObject.IsInstanceValid(plantPacketScroll))
		{
			return;
		}
		if (_plantBindingOperationCount > 0)
		{
			_plantRefreshRequestedWhileBinding = true;
			return;
		}
		int generation = ++_plantBindingGeneration;
		_plantBindingOperationCount++;
		try
		{
			UpdatePlantContentSize();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (generation != _plantBindingGeneration || !IsInsideTree())
			{
				return;
			}
			UpdatePlantContentSize();
			int columns = GetPlantColumnCount();
			float columnStride = GetPlantColumnStride();
			float rowStride = GetPlantRowStride();
			var (firstIndex, lastIndex) = GetVisibleIndexRange(_plantLogicalConfigs.Count, columns, rowStride, plantPacketScroll);
			RecyclePlantPreviewsOutside(firstIndex, lastIndex);
			TrimPlantPreviewPoolToLimit();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (generation != _plantBindingGeneration || !IsInsideTree())
			{
				return;
			}
			int batchCount = 0;
			for (int index = firstIndex; index <= lastIndex; index++)
			{
				if (_visiblePlantPreviews.TryGetValue(index, out var value) && GodotObject.IsInstanceValid(value))
				{
					PositionVirtualPreview(value, index, columns, columnStride, rowStride);
					continue;
				}
				TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TakePlantPreview();
				BindPlantPreview(towerDefenseInGamePacketShow, _plantLogicalConfigs[index]);
				PositionVirtualPreview(towerDefenseInGamePacketShow, index, columns, columnStride, rowStride);
				_visiblePlantPreviews[index] = towerDefenseInGamePacketShow;
				batchCount++;
				if (batchCount >= 12)
				{
					batchCount = 0;
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
					if (generation != _plantBindingGeneration || !IsInsideTree())
					{
						return;
					}
				}
			}
			if (plantCurrentSelect == null && _visiblePlantPreviews.TryGetValue(0, out var value2))
			{
				PlantPacketChoose(value2);
			}
		}
		finally
		{
			_plantBindingOperationCount--;
			if (_plantRefreshRequestedWhileBinding && _plantBindingOperationCount == 0 && CanQueueVirtualRefresh())
			{
				_plantRefreshRequestedWhileBinding = false;
				QueuePlantVirtualRefresh();
			}
		}
	}

	public async void RefreshZombieVirtualBindings()
	{
		_zombieRefreshQueued = false;
		if (!_zombieInitialized || !GodotObject.IsInstanceValid(zombiePacketScroll))
		{
			return;
		}
		if (_zombieBindingOperationCount > 0)
		{
			_zombieRefreshRequestedWhileBinding = true;
			return;
		}
		int generation = ++_zombieBindingGeneration;
		_zombieBindingOperationCount++;
		try
		{
			UpdateZombieContentSize();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (generation != _zombieBindingGeneration || !IsInsideTree())
			{
				return;
			}
			UpdateZombieContentSize();
			int columns = GetZombieColumnCount();
			var (firstIndex, lastIndex) = GetVisibleIndexRange(_zombieLogicalConfigs.Count, columns, 75f, zombiePacketScroll);
			RecycleZombiePreviewsOutside(firstIndex, lastIndex);
			TrimZombiePreviewPoolToLimit();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (generation != _zombieBindingGeneration || !IsInsideTree())
			{
				return;
			}
			int batchCount = 0;
			for (int index = firstIndex; index <= lastIndex; index++)
			{
				if (_visibleZombiePreviews.TryGetValue(index, out var value) && GodotObject.IsInstanceValid(value))
				{
					PositionVirtualPreview(value, index, columns, 75f, 75f);
					continue;
				}
				AlmanacZombieWidow almanacZombieWidow = TakeZombiePreview();
				BindZombiePreview(almanacZombieWidow, _zombieLogicalConfigs[index]);
				PositionVirtualPreview(almanacZombieWidow, index, columns, 75f, 75f);
				_visibleZombiePreviews[index] = almanacZombieWidow;
				batchCount++;
				if (batchCount >= 12)
				{
					batchCount = 0;
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
					if (generation != _zombieBindingGeneration || !IsInsideTree())
					{
						return;
					}
				}
			}
			if (_visibleZombiePreviews.TryGetValue(0, out var value2))
			{
				ZombiePacketChoose(value2.config);
			}
		}
		finally
		{
			_zombieBindingOperationCount--;
			if (_zombieRefreshRequestedWhileBinding && _zombieBindingOperationCount == 0 && CanQueueVirtualRefresh())
			{
				_zombieRefreshRequestedWhileBinding = false;
				QueueZombieVirtualRefresh();
			}
		}
	}

	private async Task PopulateInitialPlantBindingsAsync(int generation)
	{
		_plantBindingOperationCount++;
		try
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (generation != _plantBindingGeneration || !IsInsideTree())
			{
				return;
			}
			UpdatePlantContentSize();
			TrimPlantPreviewPoolToLimit();
			int columns = GetPlantColumnCount();
			float columnStride = GetPlantColumnStride();
			float rowStride = GetPlantRowStride();
			(int, int) visibleIndexRange = GetVisibleIndexRange(_plantLogicalConfigs.Count, columns, rowStride, plantPacketScroll);
			int item = visibleIndexRange.Item1;
			int lastIndex = visibleIndexRange.Item2;
			int batchCount = 0;
			for (int index = item; index <= lastIndex; index++)
			{
				if (generation != _plantBindingGeneration || !IsInsideTree())
				{
					return;
				}
				TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TakePlantPreview();
				BindPlantPreview(towerDefenseInGamePacketShow, _plantLogicalConfigs[index]);
				PositionVirtualPreview(towerDefenseInGamePacketShow, index, columns, columnStride, rowStride);
				_visiblePlantPreviews[index] = towerDefenseInGamePacketShow;
				batchCount++;
				if (batchCount >= 12)
				{
					batchCount = 0;
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
			}
			if (generation == _plantBindingGeneration && plantCurrentSelect == null && _visiblePlantPreviews.TryGetValue(0, out var value))
			{
				PlantPacketChoose(value);
			}
		}
		finally
		{
			_plantBindingOperationCount--;
			if (_plantRefreshRequestedWhileBinding && _plantBindingOperationCount == 0 && CanQueueVirtualRefresh())
			{
				_plantRefreshRequestedWhileBinding = false;
				QueuePlantVirtualRefresh();
			}
		}
	}

	private async Task PopulateInitialZombieBindingsAsync(int generation)
	{
		_zombieBindingOperationCount++;
		try
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (generation != _zombieBindingGeneration || !IsInsideTree())
			{
				return;
			}
			UpdateZombieContentSize();
			TrimZombiePreviewPoolToLimit();
			int columns = GetZombieColumnCount();
			(int, int) visibleIndexRange = GetVisibleIndexRange(_zombieLogicalConfigs.Count, columns, 75f, zombiePacketScroll);
			int item = visibleIndexRange.Item1;
			int lastIndex = visibleIndexRange.Item2;
			int batchCount = 0;
			for (int index = item; index <= lastIndex; index++)
			{
				if (generation != _zombieBindingGeneration || !IsInsideTree())
				{
					return;
				}
				AlmanacZombieWidow almanacZombieWidow = TakeZombiePreview();
				BindZombiePreview(almanacZombieWidow, _zombieLogicalConfigs[index]);
				PositionVirtualPreview(almanacZombieWidow, index, columns, 75f, 75f);
				_visibleZombiePreviews[index] = almanacZombieWidow;
				batchCount++;
				if (batchCount >= 12)
				{
					batchCount = 0;
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				}
			}
			if (generation == _zombieBindingGeneration && _visibleZombiePreviews.TryGetValue(0, out var value))
			{
				ZombiePacketChoose(value.config);
			}
		}
		finally
		{
			_zombieBindingOperationCount--;
			if (_zombieRefreshRequestedWhileBinding && _zombieBindingOperationCount == 0 && CanQueueVirtualRefresh())
			{
				_zombieRefreshRequestedWhileBinding = false;
				QueueZombieVirtualRefresh();
			}
		}
	}

	private bool CanQueueVirtualRefresh()
	{
		if (GodotObject.IsInstanceValid(this))
		{
			return IsInsideTree();
		}
		return false;
	}

	private static (int firstIndex, int lastIndex) GetVisibleIndexRange(int itemCount, int columns, float rowStride, ScrollContainer scroll)
	{
		if (itemCount <= 0)
		{
			return (firstIndex: 0, lastIndex: -1);
		}
		int num = Mathf.CeilToInt((float)itemCount / (float)columns);
		int num2 = Mathf.FloorToInt(Math.Max(0.0, scroll.ScrollVertical) / (double)rowStride);
		int visibleRowCount = GetVisibleRowCount(scroll, rowStride);
		int num3 = Math.Max(0, num2 - 1);
		int num4 = Math.Min(num - 1, num2 + visibleRowCount);
		return (firstIndex: num3 * columns, lastIndex: Math.Min(itemCount - 1, (num4 + 1) * columns - 1));
	}

	private static int GetVisibleRowCount(ScrollContainer scroll, float rowStride)
	{
		if (!GodotObject.IsInstanceValid(scroll))
		{
			return 1;
		}
		return Math.Max(1, Mathf.CeilToInt(scroll.Size.Y / rowStride));
	}

	private void UpdatePlantContentSize()
	{
		int columnCount = GetColumnCount(GetVirtualContentWidth(plantPacketScroll, packetMargin), GetPlantColumnStride());
		int num = ((_plantLogicalConfigs.Count != 0) ? Mathf.CeilToInt((float)_plantLogicalConfigs.Count / (float)columnCount) : 0);
		plantPacketContainer.CustomMinimumSize = new Vector2(0f, (float)num * GetPlantRowStride());
	}

	private void UpdateZombieContentSize()
	{
		int columnCount = GetColumnCount(GetVirtualContentWidth(zombiePacketScroll, _zombiePacketMargin), 75f);
		int num = ((_zombieLogicalConfigs.Count != 0) ? Mathf.CeilToInt((float)_zombieLogicalConfigs.Count / (float)columnCount) : 0);
		zombiePacketContainer.CustomMinimumSize = new Vector2(0f, (float)num * 75f);
	}

	private float GetPlantColumnStride()
	{
		if (!_mobilePreset)
		{
			return 52f;
		}
		return 98f;
	}

	private float GetPlantRowStride()
	{
		if (!_mobilePreset)
		{
			return 75f;
		}
		return 62f;
	}

	private int GetPlantColumnCount()
	{
		return GetColumnCount(GetVirtualContentWidth(plantPacketScroll, packetMargin), GetPlantColumnStride());
	}

	private int GetZombieColumnCount()
	{
		return GetColumnCount(GetVirtualContentWidth(zombiePacketScroll, _zombiePacketMargin), 75f);
	}

	private static float GetVirtualContentWidth(ScrollContainer scroll, MarginContainer margin)
	{
		if (!GodotObject.IsInstanceValid(scroll) || scroll.Size.X <= 0f)
		{
			return 0f;
		}
		float num = scroll.Size.X;
		ScrollBar vScrollBar = scroll.GetVScrollBar();
		if (GodotObject.IsInstanceValid(vScrollBar) && vScrollBar.Visible)
		{
			num -= vScrollBar.Size.X;
		}
		if (GodotObject.IsInstanceValid(margin))
		{
			num -= (float)(margin.GetThemeConstant("margin_left") + margin.GetThemeConstant("margin_right"));
		}
		return Math.Max(0f, num);
	}

	private static int GetColumnCount(float width, float columnStride)
	{
		if (width <= 0f)
		{
			return 1;
		}
		return Math.Max(1, Mathf.FloorToInt(width / columnStride) + 1);
	}

	private static void PositionVirtualPreview(Control preview, int itemIndex, int columns, float columnStride, float rowStride)
	{
		preview.Position = new Vector2((float)(itemIndex % columns) * columnStride, (float)(itemIndex / columns) * rowStride);
	}

	private TowerDefenseInGamePacketShow TakePlantPreview()
	{
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow;
		if (_plantPreviewPool.Count > 0)
		{
			int index = _plantPreviewPool.Count - 1;
			towerDefenseInGamePacketShow = _plantPreviewPool[index];
			_plantPreviewPool.RemoveAt(index);
		}
		else
		{
			towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			plantPacketContainer.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			SetLightMask(towerDefenseInGamePacketShow);
		}
		towerDefenseInGamePacketShow.Visible = true;
		return towerDefenseInGamePacketShow;
	}

	private void BindPlantPreview(TowerDefenseInGamePacketShow preview, TowerDefensePacketConfig config)
	{
		preview.ResetForPool();
		preview.setMobileLayout = _mobilePreset;
		preview.showLove = true;
		preview.enforceRuntimeAvailabilityOnPress = false;
		preview.Init(config);
		preview.openShadow = plantShowAll && !config.Unlock();
		preview.OnLoveChange += PlantLoveChange;
		preview.OnPressed += PlantPacketChoose;
	}

	private AlmanacZombieWidow TakeZombiePreview()
	{
		AlmanacZombieWidow almanacZombieWidow;
		if (_zombiePreviewPool.Count > 0)
		{
			int index = _zombiePreviewPool.Count - 1;
			almanacZombieWidow = _zombiePreviewPool[index];
			_zombiePreviewPool.RemoveAt(index);
		}
		else
		{
			almanacZombieWidow = ALMANAC_ZOMBIE_WIDOW.Instantiate<AlmanacZombieWidow>(PackedScene.GenEditState.Disabled);
			zombiePacketContainer.AddChild(almanacZombieWidow, forceReadableName: false, InternalMode.Disabled);
			SetLightMask(almanacZombieWidow);
		}
		almanacZombieWidow.Visible = true;
		return almanacZombieWidow;
	}

	private void BindZombiePreview(AlmanacZombieWidow preview, TowerDefensePacketConfig config)
	{
		preview.ClearEventHandlers();
		preview.Reset();
		preview.Init(config);
		preview.OnPressed += ZombiePacketChoose;
	}

	private void RecyclePlantPreviewsOutside(int firstIndex, int lastIndex)
	{
		List<int> list = new List<int>();
		foreach (var (num2, preview) in _visiblePlantPreviews)
		{
			if (num2 < firstIndex || num2 > lastIndex)
			{
				list.Add(num2);
				ReturnPlantPreviewToPool(preview);
			}
		}
		foreach (int item in list)
		{
			_visiblePlantPreviews.Remove(item);
		}
	}

	private void RecycleZombiePreviewsOutside(int firstIndex, int lastIndex)
	{
		List<int> list = new List<int>();
		foreach (var (num2, preview) in _visibleZombiePreviews)
		{
			if (num2 < firstIndex || num2 > lastIndex)
			{
				list.Add(num2);
				ReturnZombiePreviewToPool(preview);
			}
		}
		foreach (int item in list)
		{
			_visibleZombiePreviews.Remove(item);
		}
	}

	private void TrimPlantPreviewPoolToLimit()
	{
		int num = Math.Max(0, PlantPreviewNodeLimit - _visiblePlantPreviews.Count);
		while (_plantPreviewPool.Count > num)
		{
			int index = _plantPreviewPool.Count - 1;
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = _plantPreviewPool[index];
			_plantPreviewPool.RemoveAt(index);
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.ResetForPool();
				towerDefenseInGamePacketShow.QueueFree();
			}
		}
	}

	private void TrimZombiePreviewPoolToLimit()
	{
		int num = Math.Max(0, ZombiePreviewNodeLimit - _visibleZombiePreviews.Count);
		while (_zombiePreviewPool.Count > num)
		{
			int index = _zombiePreviewPool.Count - 1;
			AlmanacZombieWidow almanacZombieWidow = _zombiePreviewPool[index];
			_zombiePreviewPool.RemoveAt(index);
			if (GodotObject.IsInstanceValid(almanacZombieWidow))
			{
				almanacZombieWidow.ReleaseForPoolDisposal();
				almanacZombieWidow.QueueFree();
			}
		}
	}

	private void ReturnPlantPreviewToPool(TowerDefenseInGamePacketShow preview)
	{
		if (GodotObject.IsInstanceValid(preview))
		{
			if (plantCurrentSelect == preview)
			{
				plantCurrentSelect = null;
			}
			preview.ResetForPool();
			preview.Visible = false;
			_plantPreviewPool.Add(preview);
		}
	}

	private void ReturnZombiePreviewToPool(AlmanacZombieWidow preview)
	{
		if (GodotObject.IsInstanceValid(preview))
		{
			preview.ClearEventHandlers();
			preview.Reset();
			preview.Visible = false;
			_zombiePreviewPool.Add(preview);
		}
	}

	private void ReturnAllPlantPreviewsToPool()
	{
		foreach (TowerDefenseInGamePacketShow value in _visiblePlantPreviews.Values)
		{
			ReturnPlantPreviewToPool(value);
		}
		_visiblePlantPreviews.Clear();
	}

	private void ReturnAllZombiePreviewsToPool()
	{
		foreach (AlmanacZombieWidow value in _visibleZombiePreviews.Values)
		{
			ReturnZombiePreviewToPool(value);
		}
		_visibleZombiePreviews.Clear();
	}

	public void InitProp()
	{
		if (_propInitialized)
		{
			return;
		}
		_propInitialized = true;
		foreach (Variant shovel2 in TowerDefenseManager.GetShovelList())
		{
			ShovelConfig shovel = TowerDefenseManager.GetShovel((string)shovel2);
			if (shovel != null && shovel.Unlock())
			{
				AlmanacPropWidow almanacPropWidow = ALMANAC_PROP_WIDOW.Instantiate<AlmanacPropWidow>(PackedScene.GenEditState.Disabled);
				propShovelContainer.AddChild(almanacPropWidow, forceReadableName: false, InternalMode.Disabled);
				almanacPropWidow.InitShovel(shovel);
				SetLightMask(almanacPropWidow);
				almanacPropWidow.OnPressedShovel += PropShovelChoose;
			}
		}
		foreach (Variant mower in TowerDefenseManager.GetMowerList())
		{
			MowerConfig mowerConfig = TowerDefenseManager.GetMowerConfig((string)mower);
			if (mowerConfig != null && mowerConfig.Unlock())
			{
				AlmanacPropWidow almanacPropWidow2 = ALMANAC_PROP_WIDOW.Instantiate<AlmanacPropWidow>(PackedScene.GenEditState.Disabled);
				propMowerContainer.AddChild(almanacPropWidow2, forceReadableName: false, InternalMode.Disabled);
				almanacPropWidow2.InitMower(mowerConfig);
				SetLightMask(almanacPropWidow2);
				almanacPropWidow2.OnPressedMower += PropMowerChoose;
			}
		}
		if (propShovelContainer.GetChildCount() > 0)
		{
			PropShovelChoose(((AlmanacPropWidow)propShovelContainer.GetChild(0)).shovelConfig);
		}
	}

	public void PropShovelChoose(ShovelConfig config)
	{
		PropShovelInformationSet(config);
		propType = "Shovel";
		propKey = config.saveKey;
		SetPropUseButtonState(GameSaveManager.Instance.GetKeyValue("CurrentShovel").AsString() == config.saveKey);
	}

	public void PropMowerChoose(MowerConfig config)
	{
		PropMowerInformationSet(config);
		propType = "Mower";
		propKey = config.saveKey;
		SetPropUseButtonState(GameSaveManager.Instance.GetKeyValue("CurrentMower").AsString() == config.saveKey);
	}

	private void SetPropUseButtonState(bool equipped)
	{
		propUseButton.text = (equipped ? "已装备" : "装备道具");
		propUseButton.disable = equipped;
	}

	public void PropShovelInformationSet(ShovelConfig config)
	{
		propInformationPanel.InitShovel(config);
	}

	public void PropMowerInformationSet(MowerConfig config)
	{
		propInformationPanel.InitMower(config);
	}

	public void PropUseButtonPressed()
	{
		string key = ((propType == "Mower") ? "CurrentMower" : "CurrentShovel");
		GameSaveManager.Instance.SetKeyValue(key, propKey);
		SetPropUseButtonState(equipped: true);
		GameSaveManager.Instance.Save();
	}

	public void PropButtonPressed()
	{
		propLayer.Visible = true;
		InitProp();
	}

	public void ButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
	}

	public void IndexButtonPressed()
	{
		plantLayer.Visible = false;
		zombieLayer.Visible = false;
		propLayer.Visible = false;
	}

	public void CloseButtonPressed()
	{
		audio.ProcessMode = ProcessModeEnum.Pausable;
		if (!isPlaying && GodotObject.IsInstanceValid(audio))
		{
			audio.Stop();
		}
		CloseDialog();
	}

	private void SetLightMask(Node node)
	{
		if (node is CanvasItem canvasItem)
		{
			canvasItem.LightMask = 0;
		}
		if (node is Light2D light2D)
		{
			light2D.Visible = false;
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			SetLightMask(child);
		}
	}

	private void SetIndexPreviewRender(Node node)
	{
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.forceLocalRender = true;
			adobeAnimateSprite.ProcessMode = ProcessModeEnum.Always;
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			SetIndexPreviewRender(child);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(60)
		{
			new MethodInfo(MethodName.MobilePreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitPlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantLoveChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlantPacketChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlantInformationSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlantNextButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantPreButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ZombiePacketChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieInformationSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPlantScrollChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnZombieScrollChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnViewportSizeChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueuePlantVirtualRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueZombieVirtualRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPlantVirtualBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshZombieVirtualBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanQueueVirtualRefresh, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetVisibleRowCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scroll", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ScrollContainer"), exported: false),
				new PropertyInfo(Variant.Type.Float, "rowStride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePlantContentSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateZombieContentSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlantColumnStride, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlantRowStride, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlantColumnCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetZombieColumnCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetVirtualContentWidth, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scroll", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ScrollContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "margin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetColumnCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "columnStride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PositionVirtualPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "itemIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "columns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "columnStride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "rowStride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TakePlantPreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindPlantPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TakeZombiePreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindZombiePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RecyclePlantPreviewsOutside, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "firstIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "lastIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RecycleZombiePreviewsOutside, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "firstIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "lastIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrimPlantPreviewPoolToLimit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrimZombiePreviewPoolToLimit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReturnPlantPreviewToPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReturnZombiePreviewToPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReturnAllPlantPreviewsToPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReturnAllZombiePreviewsToPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitProp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PropShovelChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PropMowerChoose, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPropUseButtonState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "equipped", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PropShovelInformationSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PropMowerInformationSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PropUseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PropButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IndexButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetLightMask, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetIndexPreviewRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MobilePreset && args.Count == 0)
		{
			MobilePreset();
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
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
		if (method == MethodName.ConnectButtons && args.Count == 0)
		{
			ConnectButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.InitPlant && args.Count == 0)
		{
			InitPlant();
			ret = default;
			return true;
		}
		if (method == MethodName.PlantLoveChange && args.Count == 1)
		{
			PlantLoveChange(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlantPacketChoose && args.Count == 1)
		{
			PlantPacketChoose(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlantInformationSet && args.Count == 1)
		{
			PlantInformationSet(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlantNextButtonPressed && args.Count == 0)
		{
			PlantNextButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PlantPreButtonPressed && args.Count == 0)
		{
			PlantPreButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PlantButtonPressed && args.Count == 0)
		{
			PlantButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowButtonPressed && args.Count == 0)
		{
			ShowButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.InitZombie && args.Count == 0)
		{
			InitZombie();
			ret = default;
			return true;
		}
		if (method == MethodName.ZombiePacketChoose && args.Count == 1)
		{
			ZombiePacketChoose(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieInformationSet && args.Count == 1)
		{
			ZombieInformationSet(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieButtonPressed && args.Count == 0)
		{
			ZombieButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPlantScrollChanged && args.Count == 1)
		{
			OnPlantScrollChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnZombieScrollChanged && args.Count == 1)
		{
			OnZombieScrollChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnViewportSizeChanged && args.Count == 0)
		{
			OnViewportSizeChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.QueuePlantVirtualRefresh && args.Count == 0)
		{
			QueuePlantVirtualRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueZombieVirtualRefresh && args.Count == 0)
		{
			QueueZombieVirtualRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPlantVirtualBindings && args.Count == 0)
		{
			RefreshPlantVirtualBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshZombieVirtualBindings && args.Count == 0)
		{
			RefreshZombieVirtualBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.CanQueueVirtualRefresh && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanQueueVirtualRefresh());
			return true;
		}
		if (method == MethodName.GetVisibleRowCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetVisibleRowCount(VariantUtils.ConvertTo<ScrollContainer>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.UpdatePlantContentSize && args.Count == 0)
		{
			UpdatePlantContentSize();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateZombieContentSize && args.Count == 0)
		{
			UpdateZombieContentSize();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPlantColumnStride && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(GetPlantColumnStride());
			return true;
		}
		if (method == MethodName.GetPlantRowStride && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(GetPlantRowStride());
			return true;
		}
		if (method == MethodName.GetPlantColumnCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPlantColumnCount());
			return true;
		}
		if (method == MethodName.GetZombieColumnCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetZombieColumnCount());
			return true;
		}
		if (method == MethodName.GetVirtualContentWidth && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetVirtualContentWidth(VariantUtils.ConvertTo<ScrollContainer>(in args[0]), VariantUtils.ConvertTo<MarginContainer>(in args[1])));
			return true;
		}
		if (method == MethodName.GetColumnCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetColumnCount(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.PositionVirtualPreview && args.Count == 5)
		{
			PositionVirtualPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.TakePlantPreview && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(TakePlantPreview());
			return true;
		}
		if (method == MethodName.BindPlantPreview && args.Count == 2)
		{
			BindPlantPreview(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TakeZombiePreview && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AlmanacZombieWidow>(TakeZombiePreview());
			return true;
		}
		if (method == MethodName.BindZombiePreview && args.Count == 2)
		{
			BindZombiePreview(VariantUtils.ConvertTo<AlmanacZombieWidow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RecyclePlantPreviewsOutside && args.Count == 2)
		{
			RecyclePlantPreviewsOutside(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RecycleZombiePreviewsOutside && args.Count == 2)
		{
			RecycleZombiePreviewsOutside(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrimPlantPreviewPoolToLimit && args.Count == 0)
		{
			TrimPlantPreviewPoolToLimit();
			ret = default;
			return true;
		}
		if (method == MethodName.TrimZombiePreviewPoolToLimit && args.Count == 0)
		{
			TrimZombiePreviewPoolToLimit();
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnPlantPreviewToPool && args.Count == 1)
		{
			ReturnPlantPreviewToPool(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnZombiePreviewToPool && args.Count == 1)
		{
			ReturnZombiePreviewToPool(VariantUtils.ConvertTo<AlmanacZombieWidow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnAllPlantPreviewsToPool && args.Count == 0)
		{
			ReturnAllPlantPreviewsToPool();
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnAllZombiePreviewsToPool && args.Count == 0)
		{
			ReturnAllZombiePreviewsToPool();
			ret = default;
			return true;
		}
		if (method == MethodName.InitProp && args.Count == 0)
		{
			InitProp();
			ret = default;
			return true;
		}
		if (method == MethodName.PropShovelChoose && args.Count == 1)
		{
			PropShovelChoose(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PropMowerChoose && args.Count == 1)
		{
			PropMowerChoose(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPropUseButtonState && args.Count == 1)
		{
			SetPropUseButtonState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PropShovelInformationSet && args.Count == 1)
		{
			PropShovelInformationSet(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PropMowerInformationSet && args.Count == 1)
		{
			PropMowerInformationSet(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PropUseButtonPressed && args.Count == 0)
		{
			PropUseButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PropButtonPressed && args.Count == 0)
		{
			PropButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ButtonPressed && args.Count == 0)
		{
			ButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.IndexButtonPressed && args.Count == 0)
		{
			IndexButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseButtonPressed && args.Count == 0)
		{
			CloseButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SetLightMask && args.Count == 1)
		{
			SetLightMask(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetIndexPreviewRender && args.Count == 1)
		{
			SetIndexPreviewRender(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetVisibleRowCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetVisibleRowCount(VariantUtils.ConvertTo<ScrollContainer>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.GetVirtualContentWidth && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetVirtualContentWidth(VariantUtils.ConvertTo<ScrollContainer>(in args[0]), VariantUtils.ConvertTo<MarginContainer>(in args[1])));
			return true;
		}
		if (method == MethodName.GetColumnCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetColumnCount(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.PositionVirtualPreview && args.Count == 5)
		{
			PositionVirtualPreview(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.MobilePreset)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
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
		if (method == MethodName.ConnectButtons)
		{
			return true;
		}
		if (method == MethodName.InitPlant)
		{
			return true;
		}
		if (method == MethodName.PlantLoveChange)
		{
			return true;
		}
		if (method == MethodName.PlantPacketChoose)
		{
			return true;
		}
		if (method == MethodName.PlantInformationSet)
		{
			return true;
		}
		if (method == MethodName.PlantNextButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PlantPreButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PlantButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ShowButtonPressed)
		{
			return true;
		}
		if (method == MethodName.InitZombie)
		{
			return true;
		}
		if (method == MethodName.ZombiePacketChoose)
		{
			return true;
		}
		if (method == MethodName.ZombieInformationSet)
		{
			return true;
		}
		if (method == MethodName.ZombieButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnPlantScrollChanged)
		{
			return true;
		}
		if (method == MethodName.OnZombieScrollChanged)
		{
			return true;
		}
		if (method == MethodName.OnViewportSizeChanged)
		{
			return true;
		}
		if (method == MethodName.QueuePlantVirtualRefresh)
		{
			return true;
		}
		if (method == MethodName.QueueZombieVirtualRefresh)
		{
			return true;
		}
		if (method == MethodName.RefreshPlantVirtualBindings)
		{
			return true;
		}
		if (method == MethodName.RefreshZombieVirtualBindings)
		{
			return true;
		}
		if (method == MethodName.CanQueueVirtualRefresh)
		{
			return true;
		}
		if (method == MethodName.GetVisibleRowCount)
		{
			return true;
		}
		if (method == MethodName.UpdatePlantContentSize)
		{
			return true;
		}
		if (method == MethodName.UpdateZombieContentSize)
		{
			return true;
		}
		if (method == MethodName.GetPlantColumnStride)
		{
			return true;
		}
		if (method == MethodName.GetPlantRowStride)
		{
			return true;
		}
		if (method == MethodName.GetPlantColumnCount)
		{
			return true;
		}
		if (method == MethodName.GetZombieColumnCount)
		{
			return true;
		}
		if (method == MethodName.GetVirtualContentWidth)
		{
			return true;
		}
		if (method == MethodName.GetColumnCount)
		{
			return true;
		}
		if (method == MethodName.PositionVirtualPreview)
		{
			return true;
		}
		if (method == MethodName.TakePlantPreview)
		{
			return true;
		}
		if (method == MethodName.BindPlantPreview)
		{
			return true;
		}
		if (method == MethodName.TakeZombiePreview)
		{
			return true;
		}
		if (method == MethodName.BindZombiePreview)
		{
			return true;
		}
		if (method == MethodName.RecyclePlantPreviewsOutside)
		{
			return true;
		}
		if (method == MethodName.RecycleZombiePreviewsOutside)
		{
			return true;
		}
		if (method == MethodName.TrimPlantPreviewPoolToLimit)
		{
			return true;
		}
		if (method == MethodName.TrimZombiePreviewPoolToLimit)
		{
			return true;
		}
		if (method == MethodName.ReturnPlantPreviewToPool)
		{
			return true;
		}
		if (method == MethodName.ReturnZombiePreviewToPool)
		{
			return true;
		}
		if (method == MethodName.ReturnAllPlantPreviewsToPool)
		{
			return true;
		}
		if (method == MethodName.ReturnAllZombiePreviewsToPool)
		{
			return true;
		}
		if (method == MethodName.InitProp)
		{
			return true;
		}
		if (method == MethodName.PropShovelChoose)
		{
			return true;
		}
		if (method == MethodName.PropMowerChoose)
		{
			return true;
		}
		if (method == MethodName.SetPropUseButtonState)
		{
			return true;
		}
		if (method == MethodName.PropShovelInformationSet)
		{
			return true;
		}
		if (method == MethodName.PropMowerInformationSet)
		{
			return true;
		}
		if (method == MethodName.PropUseButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PropButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ButtonPressed)
		{
			return true;
		}
		if (method == MethodName.IndexButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CloseButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SetLightMask)
		{
			return true;
		}
		if (method == MethodName.SetIndexPreviewRender)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._plantInitialized)
		{
			_plantInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._zombieInitialized)
		{
			_zombieInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._propInitialized)
		{
			_propInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._plantRefreshQueued)
		{
			_plantRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._zombieRefreshQueued)
		{
			_zombieRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mobilePreset)
		{
			_mobilePreset = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._plantBindingGeneration)
		{
			_plantBindingGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._zombieBindingGeneration)
		{
			_zombieBindingGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._plantBindingOperationCount)
		{
			_plantBindingOperationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._zombieBindingOperationCount)
		{
			_zombieBindingOperationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._plantRefreshRequestedWhileBinding)
		{
			_plantRefreshRequestedWhileBinding = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._zombieRefreshRequestedWhileBinding)
		{
			_zombieRefreshRequestedWhileBinding = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._layoutViewport)
		{
			_layoutViewport = VariantUtils.ConvertTo<Viewport>(in value);
			return true;
		}
		if (name == PropertyName._plantLayoutRoot)
		{
			_plantLayoutRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._zombieLayoutRoot)
		{
			_zombieLayoutRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._zombiePacketMargin)
		{
			_zombiePacketMargin = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.plantPacketScroll)
		{
			plantPacketScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName.packetMargin)
		{
			packetMargin = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.plantLayer)
		{
			plantLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.plantPacketContainer)
		{
			plantPacketContainer = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.plantInformationPanel)
		{
			plantInformationPanel = VariantUtils.ConvertTo<InformationPanel>(in value);
			return true;
		}
		if (name == PropertyName.plantShowLabel)
		{
			plantShowLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.zombieLayer)
		{
			zombieLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.zombiePacketScroll)
		{
			zombiePacketScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName.zombiePacketContainer)
		{
			zombiePacketContainer = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.zombieInformationPanel)
		{
			zombieInformationPanel = VariantUtils.ConvertTo<InformationPanel>(in value);
			return true;
		}
		if (name == PropertyName.propLayer)
		{
			propLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.propShovelContainer)
		{
			propShovelContainer = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName.propMowerContainer)
		{
			propMowerContainer = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName.propInformationPanel)
		{
			propInformationPanel = VariantUtils.ConvertTo<InformationPanel>(in value);
			return true;
		}
		if (name == PropertyName.propUseButton)
		{
			propUseButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName.plantPacketBank)
		{
			plantPacketBank = VariantUtils.ConvertTo<TowerDefensePacketBankData>(in value);
			return true;
		}
		if (name == PropertyName.zombiePacketBank)
		{
			zombiePacketBank = VariantUtils.ConvertTo<TowerDefensePacketBankData>(in value);
			return true;
		}
		if (name == PropertyName.plantCategoryId)
		{
			plantCategoryId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.plantCurrentSelect)
		{
			plantCurrentSelect = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.plantShowAll)
		{
			plantShowAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.propType)
		{
			propType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.propKey)
		{
			propKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.audio)
		{
			audio = VariantUtils.ConvertTo<AudioStreamPlayerMember>(in value);
			return true;
		}
		if (name == PropertyName.isPlaying)
		{
			isPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.PlantLogicalEntryCount)
		{
			from = PlantLogicalEntryCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZombieLogicalEntryCount)
		{
			from = ZombieLogicalEntryCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PlantPreviewNodeCount)
		{
			from = PlantPreviewNodeCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZombiePreviewNodeCount)
		{
			from = ZombiePreviewNodeCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.ArePlantVirtualBindingsStable)
		{
			from2 = ArePlantVirtualBindingsStable;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.AreZombieVirtualBindingsStable)
		{
			from2 = AreZombieVirtualBindingsStable;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsPropCategoryInitialized)
		{
			from2 = IsPropCategoryInitialized;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PlantPreviewNodeLimit)
		{
			from = PlantPreviewNodeLimit;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZombiePreviewNodeLimit)
		{
			from = ZombiePreviewNodeLimit;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PlantColumnCount)
		{
			from = PlantColumnCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZombieColumnCount)
		{
			from = ZombieColumnCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._plantInitialized)
		{
			value = VariantUtils.CreateFrom(in _plantInitialized);
			return true;
		}
		if (name == PropertyName._zombieInitialized)
		{
			value = VariantUtils.CreateFrom(in _zombieInitialized);
			return true;
		}
		if (name == PropertyName._propInitialized)
		{
			value = VariantUtils.CreateFrom(in _propInitialized);
			return true;
		}
		if (name == PropertyName._plantRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _plantRefreshQueued);
			return true;
		}
		if (name == PropertyName._zombieRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _zombieRefreshQueued);
			return true;
		}
		if (name == PropertyName._mobilePreset)
		{
			value = VariantUtils.CreateFrom(in _mobilePreset);
			return true;
		}
		if (name == PropertyName._plantBindingGeneration)
		{
			value = VariantUtils.CreateFrom(in _plantBindingGeneration);
			return true;
		}
		if (name == PropertyName._zombieBindingGeneration)
		{
			value = VariantUtils.CreateFrom(in _zombieBindingGeneration);
			return true;
		}
		if (name == PropertyName._plantBindingOperationCount)
		{
			value = VariantUtils.CreateFrom(in _plantBindingOperationCount);
			return true;
		}
		if (name == PropertyName._zombieBindingOperationCount)
		{
			value = VariantUtils.CreateFrom(in _zombieBindingOperationCount);
			return true;
		}
		if (name == PropertyName._plantRefreshRequestedWhileBinding)
		{
			value = VariantUtils.CreateFrom(in _plantRefreshRequestedWhileBinding);
			return true;
		}
		if (name == PropertyName._zombieRefreshRequestedWhileBinding)
		{
			value = VariantUtils.CreateFrom(in _zombieRefreshRequestedWhileBinding);
			return true;
		}
		if (name == PropertyName._layoutViewport)
		{
			value = VariantUtils.CreateFrom(in _layoutViewport);
			return true;
		}
		if (name == PropertyName._plantLayoutRoot)
		{
			value = VariantUtils.CreateFrom(in _plantLayoutRoot);
			return true;
		}
		if (name == PropertyName._zombieLayoutRoot)
		{
			value = VariantUtils.CreateFrom(in _zombieLayoutRoot);
			return true;
		}
		if (name == PropertyName._zombiePacketMargin)
		{
			value = VariantUtils.CreateFrom(in _zombiePacketMargin);
			return true;
		}
		if (name == PropertyName.plantPacketScroll)
		{
			value = VariantUtils.CreateFrom(in plantPacketScroll);
			return true;
		}
		if (name == PropertyName.packetMargin)
		{
			value = VariantUtils.CreateFrom(in packetMargin);
			return true;
		}
		if (name == PropertyName.plantLayer)
		{
			value = VariantUtils.CreateFrom(in plantLayer);
			return true;
		}
		if (name == PropertyName.plantPacketContainer)
		{
			value = VariantUtils.CreateFrom(in plantPacketContainer);
			return true;
		}
		if (name == PropertyName.plantInformationPanel)
		{
			value = VariantUtils.CreateFrom(in plantInformationPanel);
			return true;
		}
		if (name == PropertyName.plantShowLabel)
		{
			value = VariantUtils.CreateFrom(in plantShowLabel);
			return true;
		}
		if (name == PropertyName.zombieLayer)
		{
			value = VariantUtils.CreateFrom(in zombieLayer);
			return true;
		}
		if (name == PropertyName.zombiePacketScroll)
		{
			value = VariantUtils.CreateFrom(in zombiePacketScroll);
			return true;
		}
		if (name == PropertyName.zombiePacketContainer)
		{
			value = VariantUtils.CreateFrom(in zombiePacketContainer);
			return true;
		}
		if (name == PropertyName.zombieInformationPanel)
		{
			value = VariantUtils.CreateFrom(in zombieInformationPanel);
			return true;
		}
		if (name == PropertyName.propLayer)
		{
			value = VariantUtils.CreateFrom(in propLayer);
			return true;
		}
		if (name == PropertyName.propShovelContainer)
		{
			value = VariantUtils.CreateFrom(in propShovelContainer);
			return true;
		}
		if (name == PropertyName.propMowerContainer)
		{
			value = VariantUtils.CreateFrom(in propMowerContainer);
			return true;
		}
		if (name == PropertyName.propInformationPanel)
		{
			value = VariantUtils.CreateFrom(in propInformationPanel);
			return true;
		}
		if (name == PropertyName.propUseButton)
		{
			value = VariantUtils.CreateFrom(in propUseButton);
			return true;
		}
		if (name == PropertyName.plantPacketBank)
		{
			value = VariantUtils.CreateFrom(in plantPacketBank);
			return true;
		}
		if (name == PropertyName.zombiePacketBank)
		{
			value = VariantUtils.CreateFrom(in zombiePacketBank);
			return true;
		}
		if (name == PropertyName.plantCategoryId)
		{
			value = VariantUtils.CreateFrom(in plantCategoryId);
			return true;
		}
		if (name == PropertyName.plantCurrentSelect)
		{
			value = VariantUtils.CreateFrom(in plantCurrentSelect);
			return true;
		}
		if (name == PropertyName.plantShowAll)
		{
			value = VariantUtils.CreateFrom(in plantShowAll);
			return true;
		}
		if (name == PropertyName.propType)
		{
			value = VariantUtils.CreateFrom(in propType);
			return true;
		}
		if (name == PropertyName.propKey)
		{
			value = VariantUtils.CreateFrom(in propKey);
			return true;
		}
		if (name == PropertyName.audio)
		{
			value = VariantUtils.CreateFrom(in audio);
			return true;
		}
		if (name == PropertyName.isPlaying)
		{
			value = VariantUtils.CreateFrom(in isPlaying);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zombieInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._propInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zombieRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mobilePreset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._plantBindingGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._zombieBindingGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._plantBindingOperationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._zombieBindingOperationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantRefreshRequestedWhileBinding, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zombieRefreshRequestedWhileBinding, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plantLayoutRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombieLayoutRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombiePacketMargin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantPacketScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetMargin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantPacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantInformationPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantShowLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombieLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombiePacketScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombiePacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombieInformationPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propShovelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propMowerContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propInformationPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propUseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantPacketBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombiePacketBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.plantCategoryId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.plantCurrentSelect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantShowAll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.propType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.propKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.audio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PlantLogicalEntryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZombieLogicalEntryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PlantPreviewNodeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZombiePreviewNodeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ArePlantVirtualBindingsStable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AreZombieVirtualBindingsStable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPropCategoryInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PlantPreviewNodeLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZombiePreviewNodeLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PlantColumnCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZombieColumnCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._plantInitialized, Variant.From(in _plantInitialized));
		info.AddProperty(PropertyName._zombieInitialized, Variant.From(in _zombieInitialized));
		info.AddProperty(PropertyName._propInitialized, Variant.From(in _propInitialized));
		info.AddProperty(PropertyName._plantRefreshQueued, Variant.From(in _plantRefreshQueued));
		info.AddProperty(PropertyName._zombieRefreshQueued, Variant.From(in _zombieRefreshQueued));
		info.AddProperty(PropertyName._mobilePreset, Variant.From(in _mobilePreset));
		info.AddProperty(PropertyName._plantBindingGeneration, Variant.From(in _plantBindingGeneration));
		info.AddProperty(PropertyName._zombieBindingGeneration, Variant.From(in _zombieBindingGeneration));
		info.AddProperty(PropertyName._plantBindingOperationCount, Variant.From(in _plantBindingOperationCount));
		info.AddProperty(PropertyName._zombieBindingOperationCount, Variant.From(in _zombieBindingOperationCount));
		info.AddProperty(PropertyName._plantRefreshRequestedWhileBinding, Variant.From(in _plantRefreshRequestedWhileBinding));
		info.AddProperty(PropertyName._zombieRefreshRequestedWhileBinding, Variant.From(in _zombieRefreshRequestedWhileBinding));
		info.AddProperty(PropertyName._layoutViewport, Variant.From(in _layoutViewport));
		info.AddProperty(PropertyName._plantLayoutRoot, Variant.From(in _plantLayoutRoot));
		info.AddProperty(PropertyName._zombieLayoutRoot, Variant.From(in _zombieLayoutRoot));
		info.AddProperty(PropertyName._zombiePacketMargin, Variant.From(in _zombiePacketMargin));
		info.AddProperty(PropertyName.plantPacketScroll, Variant.From(in plantPacketScroll));
		info.AddProperty(PropertyName.packetMargin, Variant.From(in packetMargin));
		info.AddProperty(PropertyName.plantLayer, Variant.From(in plantLayer));
		info.AddProperty(PropertyName.plantPacketContainer, Variant.From(in plantPacketContainer));
		info.AddProperty(PropertyName.plantInformationPanel, Variant.From(in plantInformationPanel));
		info.AddProperty(PropertyName.plantShowLabel, Variant.From(in plantShowLabel));
		info.AddProperty(PropertyName.zombieLayer, Variant.From(in zombieLayer));
		info.AddProperty(PropertyName.zombiePacketScroll, Variant.From(in zombiePacketScroll));
		info.AddProperty(PropertyName.zombiePacketContainer, Variant.From(in zombiePacketContainer));
		info.AddProperty(PropertyName.zombieInformationPanel, Variant.From(in zombieInformationPanel));
		info.AddProperty(PropertyName.propLayer, Variant.From(in propLayer));
		info.AddProperty(PropertyName.propShovelContainer, Variant.From(in propShovelContainer));
		info.AddProperty(PropertyName.propMowerContainer, Variant.From(in propMowerContainer));
		info.AddProperty(PropertyName.propInformationPanel, Variant.From(in propInformationPanel));
		info.AddProperty(PropertyName.propUseButton, Variant.From(in propUseButton));
		info.AddProperty(PropertyName.plantPacketBank, Variant.From(in plantPacketBank));
		info.AddProperty(PropertyName.zombiePacketBank, Variant.From(in zombiePacketBank));
		info.AddProperty(PropertyName.plantCategoryId, Variant.From(in plantCategoryId));
		info.AddProperty(PropertyName.plantCurrentSelect, Variant.From(in plantCurrentSelect));
		info.AddProperty(PropertyName.plantShowAll, Variant.From(in plantShowAll));
		info.AddProperty(PropertyName.propType, Variant.From(in propType));
		info.AddProperty(PropertyName.propKey, Variant.From(in propKey));
		info.AddProperty(PropertyName.audio, Variant.From(in audio));
		info.AddProperty(PropertyName.isPlaying, Variant.From(in isPlaying));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._plantInitialized, out var value))
		{
			_plantInitialized = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._zombieInitialized, out var value2))
		{
			_zombieInitialized = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._propInitialized, out var value3))
		{
			_propInitialized = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._plantRefreshQueued, out var value4))
		{
			_plantRefreshQueued = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._zombieRefreshQueued, out var value5))
		{
			_zombieRefreshQueued = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mobilePreset, out var value6))
		{
			_mobilePreset = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._plantBindingGeneration, out var value7))
		{
			_plantBindingGeneration = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._zombieBindingGeneration, out var value8))
		{
			_zombieBindingGeneration = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._plantBindingOperationCount, out var value9))
		{
			_plantBindingOperationCount = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._zombieBindingOperationCount, out var value10))
		{
			_zombieBindingOperationCount = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._plantRefreshRequestedWhileBinding, out var value11))
		{
			_plantRefreshRequestedWhileBinding = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._zombieRefreshRequestedWhileBinding, out var value12))
		{
			_zombieRefreshRequestedWhileBinding = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._layoutViewport, out var value13))
		{
			_layoutViewport = value13.As<Viewport>();
		}
		if (info.TryGetProperty(PropertyName._plantLayoutRoot, out var value14))
		{
			_plantLayoutRoot = value14.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._zombieLayoutRoot, out var value15))
		{
			_zombieLayoutRoot = value15.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._zombiePacketMargin, out var value16))
		{
			_zombiePacketMargin = value16.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.plantPacketScroll, out var value17))
		{
			plantPacketScroll = value17.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName.packetMargin, out var value18))
		{
			packetMargin = value18.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.plantLayer, out var value19))
		{
			plantLayer = value19.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.plantPacketContainer, out var value20))
		{
			plantPacketContainer = value20.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.plantInformationPanel, out var value21))
		{
			plantInformationPanel = value21.As<InformationPanel>();
		}
		if (info.TryGetProperty(PropertyName.plantShowLabel, out var value22))
		{
			plantShowLabel = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.zombieLayer, out var value23))
		{
			zombieLayer = value23.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.zombiePacketScroll, out var value24))
		{
			zombiePacketScroll = value24.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName.zombiePacketContainer, out var value25))
		{
			zombiePacketContainer = value25.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.zombieInformationPanel, out var value26))
		{
			zombieInformationPanel = value26.As<InformationPanel>();
		}
		if (info.TryGetProperty(PropertyName.propLayer, out var value27))
		{
			propLayer = value27.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.propShovelContainer, out var value28))
		{
			propShovelContainer = value28.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName.propMowerContainer, out var value29))
		{
			propMowerContainer = value29.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName.propInformationPanel, out var value30))
		{
			propInformationPanel = value30.As<InformationPanel>();
		}
		if (info.TryGetProperty(PropertyName.propUseButton, out var value31))
		{
			propUseButton = value31.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName.plantPacketBank, out var value32))
		{
			plantPacketBank = value32.As<TowerDefensePacketBankData>();
		}
		if (info.TryGetProperty(PropertyName.zombiePacketBank, out var value33))
		{
			zombiePacketBank = value33.As<TowerDefensePacketBankData>();
		}
		if (info.TryGetProperty(PropertyName.plantCategoryId, out var value34))
		{
			plantCategoryId = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName.plantCurrentSelect, out var value35))
		{
			plantCurrentSelect = value35.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.plantShowAll, out var value36))
		{
			plantShowAll = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.propType, out var value37))
		{
			propType = value37.As<string>();
		}
		if (info.TryGetProperty(PropertyName.propKey, out var value38))
		{
			propKey = value38.As<string>();
		}
		if (info.TryGetProperty(PropertyName.audio, out var value39))
		{
			audio = value39.As<AudioStreamPlayerMember>();
		}
		if (info.TryGetProperty(PropertyName.isPlaying, out var value40))
		{
			isPlaying = value40.As<bool>();
		}
	}
}
