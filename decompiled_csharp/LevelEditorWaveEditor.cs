using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/WaveEditor/LevelEditorWaveEditor.cs")]
public class LevelEditorWaveEditor : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnChildPacketBankVisibilityChanged = "OnChildPacketBankVisibilityChanged";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName ClearPanel = "ClearPanel";

		public static readonly StringName Init = "Init";

		public static readonly StringName Save = "Save";

		public static readonly StringName SaveFlag = "SaveFlag";

		public static readonly StringName MapChange = "MapChange";

		public static readonly StringName FlagChanged = "FlagChanged";

		public static readonly StringName BigWaveSpinBoxValueChanged = "BigWaveSpinBoxValueChanged";

		public static readonly StringName WaveIntervalSpinBoxValueChanged = "WaveIntervalSpinBoxValueChanged";

		public static readonly StringName WaveNumChange = "WaveNumChange";

		public static readonly StringName Selected = "Selected";

		public static readonly StringName SpawnButtonPressed = "SpawnButtonPressed";

		public static readonly StringName EventButtonPressed = "EventButtonPressed";

		public static readonly StringName BeginColSpinBoxValueChanged = "BeginColSpinBoxValueChanged";

		public static readonly StringName SpawnColEndSpinBoxValueChanged = "SpawnColEndSpinBoxValueChanged";

		public static readonly StringName SpawnColStartSpinBoxValueChanged = "SpawnColStartSpinBoxValueChanged";

		public static readonly StringName MinNextWaveHealthPercentageSpinBoxValueChanged = "MinNextWaveHealthPercentageSpinBoxValueChanged";

		public static readonly StringName MaxNextWaveHealthPercentageSpinBoxValueChanged = "MaxNextWaveHealthPercentageSpinBoxValueChanged";

		public static readonly StringName EventListContainerChange = "EventListContainerChange";

		public static readonly StringName ZombieInvisibleCheckBoxToggled = "ZombieInvisibleCheckBoxToggled";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName currentFlag = "currentFlag";

		public static readonly StringName _spawnNode = "_spawnNode";

		public static readonly StringName _eventNode = "_eventNode";

		public static readonly StringName _zombieInvisibleCheckBox = "_zombieInvisibleCheckBox";

		public static readonly StringName _poolPacketContainer = "_poolPacketContainer";

		public static readonly StringName _randomPacketContainer = "_randomPacketContainer";

		public static readonly StringName _packetContainerListContainer = "_packetContainerListContainer";

		public static readonly StringName _eventListContainer = "_eventListContainer";

		public static readonly StringName _bigWaveSpinBox = "_bigWaveSpinBox";

		public static readonly StringName _waveIntervalSpinBox = "_waveIntervalSpinBox";

		public static readonly StringName _beginColSpinBox = "_beginColSpinBox";

		public static readonly StringName _spawnColEndSpinBox = "_spawnColEndSpinBox";

		public static readonly StringName _spawnColStartSpinBox = "_spawnColStartSpinBox";

		public static readonly StringName _minNextWaveHealthPercentageSpinBox = "_minNextWaveHealthPercentageSpinBox";

		public static readonly StringName _maxNextWaveHealthPercentageSpinBox = "_maxNextWaveHealthPercentageSpinBox";

		public static readonly StringName _tipsLabel = "_tipsLabel";

		public static readonly StringName _flagSlider = "_flagSlider";

		public static readonly StringName _waveLabel = "_waveLabel";

		public static readonly StringName _levelEditorInspector = "_levelEditorInspector";

		public static readonly StringName levelConfig = "levelConfig";

		public static readonly StringName waveManagerConfig = "waveManagerConfig";

		public static readonly StringName isInit = "isInit";

		public static readonly StringName isLoad = "isLoad";

		public static readonly StringName isLoading = "isLoading";

		public static readonly StringName _loadGeneration = "_loadGeneration";

		public static readonly StringName _currentFlag = "_currentFlag";

		public static readonly StringName currentPacketContainer = "currentPacketContainer";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static PackedScene _levelEditorWaveEditorPacketContainer;

	private Control _spawnNode;

	private Control _eventNode;

	private CheckBox _zombieInvisibleCheckBox;

	private LevelEditorWaveEditorPacketContainer _poolPacketContainer;

	private LevelEditorWaveEditorPacketContainer _randomPacketContainer;

	private VBoxContainer _packetContainerListContainer;

	private LevelEventListContainer _eventListContainer;

	private SpinBox _bigWaveSpinBox;

	private SpinBox _waveIntervalSpinBox;

	private SpinBox _beginColSpinBox;

	private SpinBox _spawnColEndSpinBox;

	private SpinBox _spawnColStartSpinBox;

	private SpinBox _minNextWaveHealthPercentageSpinBox;

	private SpinBox _maxNextWaveHealthPercentageSpinBox;

	private Label _tipsLabel;

	private HSlider _flagSlider;

	private Label _waveLabel;

	private LevelEditorInspector _levelEditorInspector;

	public static LevelEditorWaveEditor Instance;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelConfig levelConfig;

	public TowerDefenseLevelWaveManagerConfig waveManagerConfig;

	public bool isInit;

	public bool isLoad;

	public bool isLoading;

	public int _loadGeneration;

	private int _currentFlag = -1;

	public LevelEditorWaveEditorPacketContainer currentPacketContainer;

	private static PackedScene LEVEL_EDITOR_WAVE_EDITOR_PACKET_CONTAINER => _levelEditorWaveEditorPacketContainer ?? (_levelEditorWaveEditorPacketContainer = GD.Load<PackedScene>("uid://bua2w8mprv7s"));

	public int currentFlag
	{
		get
		{
			return _currentFlag;
		}
		set
		{
			if (_currentFlag != -1 && value != -1 && !isLoading)
			{
				SaveFlag(_currentFlag);
			}
			_currentFlag = value;
		}
	}

	public override void _Ready()
	{
		_spawnNode = GetNode<Control>("%SpawnNode");
		_eventNode = GetNode<Control>("%EventNode");
		_zombieInvisibleCheckBox = GetNode<CheckBox>("%ZombieInvisibleCheckBox");
		_poolPacketContainer = GetNode<LevelEditorWaveEditorPacketContainer>("%PoolPacketContainer");
		_randomPacketContainer = GetNode<LevelEditorWaveEditorPacketContainer>("%RandomPacketContainer");
		_packetContainerListContainer = GetNode<VBoxContainer>("%PacketContainerListContainer");
		_eventListContainer = GetNode<LevelEventListContainer>("%EventListContainer");
		_eventListContainer.OnChange += EventListContainerChange;
		_bigWaveSpinBox = GetNode<SpinBox>("%BigWaveSpinBox");
		_waveIntervalSpinBox = GetNode<SpinBox>("%WaveIntervalSpinBox");
		_beginColSpinBox = GetNode<SpinBox>("%BeginColSpinBox");
		_spawnColEndSpinBox = GetNode<SpinBox>("%SpawnColEndSpinBox");
		_spawnColStartSpinBox = GetNode<SpinBox>("%SpawnColStartSpinBox");
		_minNextWaveHealthPercentageSpinBox = GetNode<SpinBox>("%MinNextWaveHealthPercentageSpinBox");
		_maxNextWaveHealthPercentageSpinBox = GetNode<SpinBox>("%MaxNextWaveHealthPercentageSpinBox");
		_tipsLabel = GetNode<Label>("%TipsLabel");
		_flagSlider = GetNode<HSlider>("%FlagSlider");
		_waveLabel = GetNode<Label>("%WaveLabel");
		_levelEditorInspector = GetNode<LevelEditorInspector>("%LevelEditorInspector");
		_poolPacketContainer.OnSelected += Selected;
		_randomPacketContainer.mainButton.text = "编辑随机出怪";
		_randomPacketContainer.OnSelected += Selected;
		for (int i = 0; i < 25; i++)
		{
			LevelEditorWaveEditorPacketContainer obj = _packetContainerListContainer.GetChild(i) as LevelEditorWaveEditorPacketContainer;
			obj.mainButton.text = $"编辑第{i + 1}行";
			obj.OnSelected += Selected;
		}
		currentPacketContainer = null;
		Instance = this;
		_zombieInvisibleCheckBox.Toggled += ZombieInvisibleCheckBoxToggled;
		_bigWaveSpinBox.ValueChanged += BigWaveSpinBoxValueChanged;
		_waveIntervalSpinBox.ValueChanged += WaveIntervalSpinBoxValueChanged;
		_beginColSpinBox.ValueChanged += BeginColSpinBoxValueChanged;
		_spawnColEndSpinBox.ValueChanged += SpawnColEndSpinBoxValueChanged;
		_spawnColStartSpinBox.ValueChanged += SpawnColStartSpinBoxValueChanged;
		_minNextWaveHealthPercentageSpinBox.ValueChanged += MinNextWaveHealthPercentageSpinBoxValueChanged;
		_maxNextWaveHealthPercentageSpinBox.ValueChanged += MaxNextWaveHealthPercentageSpinBoxValueChanged;
		_flagSlider.DragEnded += FlagChanged;
		GetNode<NinePatchButtonBase>("Transform/SpawnButton").OnPressed += SpawnButtonPressed;
		GetNode<NinePatchButtonBase>("Transform/EventButton").OnPressed += EventButtonPressed;
		VisibilityChanged += OnChildPacketBankVisibilityChanged;
	}

	public void OnChildPacketBankVisibilityChanged()
	{
		if (GodotObject.IsInstanceValid(LevelEditorWaveEditorPacketChoose.Instance))
		{
			LevelEditorWaveEditorPacketChoose.Instance.RefreshWhenVisible();
		}
	}

	public void Clear()
	{
		levelConfig = null;
		waveManagerConfig = null;
		ClearPanel();
	}

	public void ClearPanel()
	{
		isLoading = false;
		_poolPacketContainer.Clear();
		_randomPacketContainer.Clear();
		_eventListContainer.Clear();
		for (int i = 0; i < 25; i++)
		{
			(_packetContainerListContainer.GetChild(i) as LevelEditorWaveEditorPacketContainer).Clear();
		}
	}

	public void Init(TowerDefenseLevelConfig _levelConfig)
	{
		isInit = true;
		Clear();
		_currentFlag = -1;
		levelConfig = _levelConfig;
		if (!GodotObject.IsInstanceValid(levelConfig.waveManager))
		{
			waveManagerConfig = new TowerDefenseLevelWaveManagerConfig();
			for (int i = 0; i < waveManagerConfig.flagWaveInterval; i++)
			{
				TowerDefenseLevelWaveConfig item = new TowerDefenseLevelWaveConfig();
				waveManagerConfig.wave.Add(item);
			}
		}
		else
		{
			waveManagerConfig = levelConfig.waveManager.Duplicate(deep: true) as TowerDefenseLevelWaveManagerConfig;
		}
		_zombieInvisibleCheckBox.ButtonPressed = waveManagerConfig.zombieInvisible;
		_bigWaveSpinBox.Value = (double)waveManagerConfig.wave.Count / (double)waveManagerConfig.flagWaveInterval;
		_waveIntervalSpinBox.Value = waveManagerConfig.flagWaveInterval;
		_beginColSpinBox.Value = waveManagerConfig.beginCol;
		_spawnColEndSpinBox.Value = waveManagerConfig.spawnColEnd;
		_spawnColStartSpinBox.Value = waveManagerConfig.spawnColStart;
		_minNextWaveHealthPercentageSpinBox.Value = waveManagerConfig.minNextWaveHealthPercentage;
		_maxNextWaveHealthPercentageSpinBox.Value = waveManagerConfig.maxNextWaveHealthPercentage;
		WaveNumChange();
		isInit = false;
		FlagChanged(isChange: true);
	}

	public void Save()
	{
		if (GodotObject.IsInstanceValid(waveManagerConfig) && !isLoading)
		{
			SaveFlag(_currentFlag);
			waveManagerConfig.zombieInvisible = _zombieInvisibleCheckBox.ButtonPressed;
			waveManagerConfig.flagWaveInterval = (int)_waveIntervalSpinBox.Value;
			waveManagerConfig.beginCol = _beginColSpinBox.Value;
			waveManagerConfig.spawnColEnd = _spawnColEndSpinBox.Value;
			waveManagerConfig.spawnColStart = _spawnColStartSpinBox.Value;
			waveManagerConfig.minNextWaveHealthPercentage = _minNextWaveHealthPercentageSpinBox.Value;
			waveManagerConfig.maxNextWaveHealthPercentage = _maxNextWaveHealthPercentageSpinBox.Value;
			Array<TowerDefenseLevelWaveConfig> array = new Array<TowerDefenseLevelWaveConfig>();
			for (int i = 0; (double)i < _waveIntervalSpinBox.Value * _bigWaveSpinBox.Value; i++)
			{
				array.Add(waveManagerConfig.wave[i]);
			}
			waveManagerConfig.wave = array;
			levelConfig.waveManager = waveManagerConfig;
		}
	}

	public void SaveFlag(int flagId)
	{
		if (_currentFlag == -1 || !GodotObject.IsInstanceValid(waveManagerConfig))
		{
			return;
		}
		TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = waveManagerConfig.wave[flagId];
		towerDefenseLevelWaveConfig.spawn.Clear();
		towerDefenseLevelWaveConfig.dynamic = new TowerDefenseLevelSpawnDynamicConfig();
		foreach (TowerDefenseInGamePacketShow packet in _poolPacketContainer.packetList)
		{
			string saveKey = packet.config.saveKey;
			int wavePointCost = packet.config.GetWavePointCost();
			towerDefenseLevelWaveConfig.dynamic.points += wavePointCost;
			towerDefenseLevelWaveConfig.dynamic.zombiePool.Add(saveKey);
		}
		foreach (TowerDefenseInGamePacketShow packet2 in _randomPacketContainer.packetList)
		{
			string saveKey2 = packet2.config.saveKey;
			TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig = new TowerDefenseLevelSpawnConfig();
			towerDefenseLevelSpawnConfig.zombie = saveKey2;
			towerDefenseLevelSpawnConfig.line = -1;
			towerDefenseLevelSpawnConfig.num = 1;
			towerDefenseLevelWaveConfig.spawn.Add(towerDefenseLevelSpawnConfig);
		}
		for (int i = 0; i < 25; i++)
		{
			LevelEditorWaveEditorPacketContainer levelEditorWaveEditorPacketContainer = _packetContainerListContainer.GetChild(i) as LevelEditorWaveEditorPacketContainer;
			if (!levelEditorWaveEditorPacketContainer.Visible)
			{
				continue;
			}
			foreach (TowerDefenseInGamePacketShow packet3 in levelEditorWaveEditorPacketContainer.packetList)
			{
				string saveKey3 = packet3.config.saveKey;
				TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig2 = new TowerDefenseLevelSpawnConfig();
				towerDefenseLevelSpawnConfig2.zombie = saveKey3;
				towerDefenseLevelSpawnConfig2.line = i + 1;
				towerDefenseLevelSpawnConfig2.num = 1;
				towerDefenseLevelWaveConfig.spawn.Add(towerDefenseLevelSpawnConfig2);
			}
		}
		towerDefenseLevelWaveConfig.eventList = _eventListContainer.GetEventList();
		_levelEditorInspector.Clear();
	}

	public void MapChange(TowerDefenseMapConfig _config)
	{
		for (int i = 0; i < 25; i++)
		{
			((CanvasItem)_packetContainerListContainer.GetChild(i)).Visible = i < _config.gridNum.Y;
		}
	}

	public async void FlagChanged(bool isChange)
	{
		if (!GodotObject.IsInstanceValid(waveManagerConfig))
		{
			isLoading = false;
			return;
		}
		if (!isChange)
		{
			isLoading = false;
			return;
		}
		if (_currentFlag != -1)
		{
			SaveFlag(_currentFlag);
		}
		_loadGeneration++;
		int currentGeneration = _loadGeneration;
		isLoading = true;
		int num = (int)_flagSlider.Value;
		_currentFlag = num;
		_waveLabel.Text = $"当前波:{_currentFlag + 1}";
		ClearPanel();
		isLoading = true;
		TowerDefenseLevelWaveConfig waveConfig = waveManagerConfig.wave[_currentFlag];
		int batchCount = 0;
		foreach (string item in waveConfig.dynamic.zombiePool)
		{
			if (_loadGeneration != currentGeneration)
			{
				return;
			}
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(item);
			_poolPacketContainer.AddPacket(packetConfig);
			batchCount++;
			if (batchCount >= 8)
			{
				batchCount = 0;
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				if (_loadGeneration != currentGeneration)
				{
					return;
				}
			}
		}
		foreach (TowerDefenseLevelSpawnConfig item2 in waveConfig.spawn)
		{
			if (_loadGeneration != currentGeneration)
			{
				return;
			}
			string zombie = item2.zombie;
			LevelEditorWaveEditorPacketContainer levelEditorWaveEditorPacketContainer = ((item2.line != -1) ? (_packetContainerListContainer.GetChild(item2.line - 1) as LevelEditorWaveEditorPacketContainer) : _randomPacketContainer);
			TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(zombie);
			for (int i = 0; i < item2.num; i++)
			{
				levelEditorWaveEditorPacketContainer.AddPacket(packetConfig2);
			}
			batchCount++;
			if (batchCount >= 8)
			{
				batchCount = 0;
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				if (_loadGeneration != currentGeneration)
				{
					return;
				}
			}
		}
		_eventListContainer.Init(waveConfig.eventList);
		isLoading = false;
	}

	public void BigWaveSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		WaveNumChange();
	}

	public void WaveIntervalSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		WaveNumChange();
	}

	public void WaveNumChange()
	{
		for (int i = 0; (double)i < _waveIntervalSpinBox.Value * _bigWaveSpinBox.Value; i++)
		{
			if (waveManagerConfig.wave.Count <= i)
			{
				TowerDefenseLevelWaveConfig item = new TowerDefenseLevelWaveConfig();
				waveManagerConfig.wave.Add(item);
			}
		}
		_flagSlider.MaxValue = _waveIntervalSpinBox.Value * _bigWaveSpinBox.Value - 1.0;
		_flagSlider.TickCount = (int)_bigWaveSpinBox.Value + 1;
		_flagSlider.Value = 0.0;
		if (!isInit)
		{
			FlagChanged(isChange: true);
		}
	}

	public void Selected(LevelEditorWaveEditorPacketContainer packetContainer)
	{
		currentPacketContainer = packetContainer;
		_tipsLabel.Modulate = Colors.White;
		_tipsLabel.Text = $"当前{currentPacketContainer.mainButton.text}";
	}

	public void SpawnButtonPressed()
	{
		_spawnNode.Visible = true;
		_eventNode.Visible = false;
	}

	public void EventButtonPressed()
	{
		_spawnNode.Visible = false;
		_eventNode.Visible = true;
	}

	public void BeginColSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void SpawnColEndSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void SpawnColStartSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void MinNextWaveHealthPercentageSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void MaxNextWaveHealthPercentageSpinBoxValueChanged(double value)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void EventListContainerChange()
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
	}

	public void ZombieInvisibleCheckBoxToggled(bool toggledOn)
	{
		if (!isInit)
		{
			levelConfig.canExport = false;
		}
		waveManagerConfig.zombieInvisible = toggledOn;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnChildPacketBankVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFlag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flagId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FlagChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isChange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BigWaveSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WaveIntervalSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WaveNumChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Selected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetContainer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EventButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginColSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnColEndSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnColStartSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MinNextWaveHealthPercentageSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MaxNextWaveHealthPercentageSpinBoxValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EventListContainerChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ZombieInvisibleCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnChildPacketBankVisibilityChanged && args.Count == 0)
		{
			OnChildPacketBankVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPanel && args.Count == 0)
		{
			ClearPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFlag && args.Count == 1)
		{
			SaveFlag(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MapChange && args.Count == 1)
		{
			MapChange(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlagChanged && args.Count == 1)
		{
			FlagChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BigWaveSpinBoxValueChanged && args.Count == 1)
		{
			BigWaveSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WaveIntervalSpinBoxValueChanged && args.Count == 1)
		{
			WaveIntervalSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WaveNumChange && args.Count == 0)
		{
			WaveNumChange();
			ret = default;
			return true;
		}
		if (method == MethodName.Selected && args.Count == 1)
		{
			Selected(VariantUtils.ConvertTo<LevelEditorWaveEditorPacketContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnButtonPressed && args.Count == 0)
		{
			SpawnButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.EventButtonPressed && args.Count == 0)
		{
			EventButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginColSpinBoxValueChanged && args.Count == 1)
		{
			BeginColSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnColEndSpinBoxValueChanged && args.Count == 1)
		{
			SpawnColEndSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnColStartSpinBoxValueChanged && args.Count == 1)
		{
			SpawnColStartSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MinNextWaveHealthPercentageSpinBoxValueChanged && args.Count == 1)
		{
			MinNextWaveHealthPercentageSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MaxNextWaveHealthPercentageSpinBoxValueChanged && args.Count == 1)
		{
			MaxNextWaveHealthPercentageSpinBoxValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EventListContainerChange && args.Count == 0)
		{
			EventListContainerChange();
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieInvisibleCheckBoxToggled && args.Count == 1)
		{
			ZombieInvisibleCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.OnChildPacketBankVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.ClearPanel)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.SaveFlag)
		{
			return true;
		}
		if (method == MethodName.MapChange)
		{
			return true;
		}
		if (method == MethodName.FlagChanged)
		{
			return true;
		}
		if (method == MethodName.BigWaveSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.WaveIntervalSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.WaveNumChange)
		{
			return true;
		}
		if (method == MethodName.Selected)
		{
			return true;
		}
		if (method == MethodName.SpawnButtonPressed)
		{
			return true;
		}
		if (method == MethodName.EventButtonPressed)
		{
			return true;
		}
		if (method == MethodName.BeginColSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.SpawnColEndSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.SpawnColStartSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.MinNextWaveHealthPercentageSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.MaxNextWaveHealthPercentageSpinBoxValueChanged)
		{
			return true;
		}
		if (method == MethodName.EventListContainerChange)
		{
			return true;
		}
		if (method == MethodName.ZombieInvisibleCheckBoxToggled)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.currentFlag)
		{
			currentFlag = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._spawnNode)
		{
			_spawnNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._eventNode)
		{
			_eventNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._zombieInvisibleCheckBox)
		{
			_zombieInvisibleCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._poolPacketContainer)
		{
			_poolPacketContainer = VariantUtils.ConvertTo<LevelEditorWaveEditorPacketContainer>(in value);
			return true;
		}
		if (name == PropertyName._randomPacketContainer)
		{
			_randomPacketContainer = VariantUtils.ConvertTo<LevelEditorWaveEditorPacketContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetContainerListContainer)
		{
			_packetContainerListContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._eventListContainer)
		{
			_eventListContainer = VariantUtils.ConvertTo<LevelEventListContainer>(in value);
			return true;
		}
		if (name == PropertyName._bigWaveSpinBox)
		{
			_bigWaveSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._waveIntervalSpinBox)
		{
			_waveIntervalSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._beginColSpinBox)
		{
			_beginColSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._spawnColEndSpinBox)
		{
			_spawnColEndSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._spawnColStartSpinBox)
		{
			_spawnColStartSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._minNextWaveHealthPercentageSpinBox)
		{
			_minNextWaveHealthPercentageSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._maxNextWaveHealthPercentageSpinBox)
		{
			_maxNextWaveHealthPercentageSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._tipsLabel)
		{
			_tipsLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._flagSlider)
		{
			_flagSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._waveLabel)
		{
			_waveLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._levelEditorInspector)
		{
			_levelEditorInspector = VariantUtils.ConvertTo<LevelEditorInspector>(in value);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.waveManagerConfig)
		{
			waveManagerConfig = VariantUtils.ConvertTo<TowerDefenseLevelWaveManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isLoad)
		{
			isLoad = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isLoading)
		{
			isLoading = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._loadGeneration)
		{
			_loadGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._currentFlag)
		{
			_currentFlag = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentPacketContainer)
		{
			currentPacketContainer = VariantUtils.ConvertTo<LevelEditorWaveEditorPacketContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.currentFlag)
		{
			value = VariantUtils.CreateFrom<int>(currentFlag);
			return true;
		}
		if (name == PropertyName._spawnNode)
		{
			value = VariantUtils.CreateFrom(in _spawnNode);
			return true;
		}
		if (name == PropertyName._eventNode)
		{
			value = VariantUtils.CreateFrom(in _eventNode);
			return true;
		}
		if (name == PropertyName._zombieInvisibleCheckBox)
		{
			value = VariantUtils.CreateFrom(in _zombieInvisibleCheckBox);
			return true;
		}
		if (name == PropertyName._poolPacketContainer)
		{
			value = VariantUtils.CreateFrom(in _poolPacketContainer);
			return true;
		}
		if (name == PropertyName._randomPacketContainer)
		{
			value = VariantUtils.CreateFrom(in _randomPacketContainer);
			return true;
		}
		if (name == PropertyName._packetContainerListContainer)
		{
			value = VariantUtils.CreateFrom(in _packetContainerListContainer);
			return true;
		}
		if (name == PropertyName._eventListContainer)
		{
			value = VariantUtils.CreateFrom(in _eventListContainer);
			return true;
		}
		if (name == PropertyName._bigWaveSpinBox)
		{
			value = VariantUtils.CreateFrom(in _bigWaveSpinBox);
			return true;
		}
		if (name == PropertyName._waveIntervalSpinBox)
		{
			value = VariantUtils.CreateFrom(in _waveIntervalSpinBox);
			return true;
		}
		if (name == PropertyName._beginColSpinBox)
		{
			value = VariantUtils.CreateFrom(in _beginColSpinBox);
			return true;
		}
		if (name == PropertyName._spawnColEndSpinBox)
		{
			value = VariantUtils.CreateFrom(in _spawnColEndSpinBox);
			return true;
		}
		if (name == PropertyName._spawnColStartSpinBox)
		{
			value = VariantUtils.CreateFrom(in _spawnColStartSpinBox);
			return true;
		}
		if (name == PropertyName._minNextWaveHealthPercentageSpinBox)
		{
			value = VariantUtils.CreateFrom(in _minNextWaveHealthPercentageSpinBox);
			return true;
		}
		if (name == PropertyName._maxNextWaveHealthPercentageSpinBox)
		{
			value = VariantUtils.CreateFrom(in _maxNextWaveHealthPercentageSpinBox);
			return true;
		}
		if (name == PropertyName._tipsLabel)
		{
			value = VariantUtils.CreateFrom(in _tipsLabel);
			return true;
		}
		if (name == PropertyName._flagSlider)
		{
			value = VariantUtils.CreateFrom(in _flagSlider);
			return true;
		}
		if (name == PropertyName._waveLabel)
		{
			value = VariantUtils.CreateFrom(in _waveLabel);
			return true;
		}
		if (name == PropertyName._levelEditorInspector)
		{
			value = VariantUtils.CreateFrom(in _levelEditorInspector);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom(in levelConfig);
			return true;
		}
		if (name == PropertyName.waveManagerConfig)
		{
			value = VariantUtils.CreateFrom(in waveManagerConfig);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			value = VariantUtils.CreateFrom(in isInit);
			return true;
		}
		if (name == PropertyName.isLoad)
		{
			value = VariantUtils.CreateFrom(in isLoad);
			return true;
		}
		if (name == PropertyName.isLoading)
		{
			value = VariantUtils.CreateFrom(in isLoading);
			return true;
		}
		if (name == PropertyName._loadGeneration)
		{
			value = VariantUtils.CreateFrom(in _loadGeneration);
			return true;
		}
		if (name == PropertyName._currentFlag)
		{
			value = VariantUtils.CreateFrom(in _currentFlag);
			return true;
		}
		if (name == PropertyName.currentPacketContainer)
		{
			value = VariantUtils.CreateFrom(in currentPacketContainer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._spawnNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombieInvisibleCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._poolPacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._randomPacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetContainerListContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventListContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bigWaveSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveIntervalSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._beginColSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spawnColEndSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spawnColStartSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._minNextWaveHealthPercentageSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._maxNextWaveHealthPercentageSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tipsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._flagSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelEditorInspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.ResourceType, "TowerDefenseLevelConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.waveManagerConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isLoad, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isLoading, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._loadGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentFlag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentFlag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentPacketContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.currentFlag, Variant.From<int>(currentFlag));
		info.AddProperty(PropertyName._spawnNode, Variant.From(in _spawnNode));
		info.AddProperty(PropertyName._eventNode, Variant.From(in _eventNode));
		info.AddProperty(PropertyName._zombieInvisibleCheckBox, Variant.From(in _zombieInvisibleCheckBox));
		info.AddProperty(PropertyName._poolPacketContainer, Variant.From(in _poolPacketContainer));
		info.AddProperty(PropertyName._randomPacketContainer, Variant.From(in _randomPacketContainer));
		info.AddProperty(PropertyName._packetContainerListContainer, Variant.From(in _packetContainerListContainer));
		info.AddProperty(PropertyName._eventListContainer, Variant.From(in _eventListContainer));
		info.AddProperty(PropertyName._bigWaveSpinBox, Variant.From(in _bigWaveSpinBox));
		info.AddProperty(PropertyName._waveIntervalSpinBox, Variant.From(in _waveIntervalSpinBox));
		info.AddProperty(PropertyName._beginColSpinBox, Variant.From(in _beginColSpinBox));
		info.AddProperty(PropertyName._spawnColEndSpinBox, Variant.From(in _spawnColEndSpinBox));
		info.AddProperty(PropertyName._spawnColStartSpinBox, Variant.From(in _spawnColStartSpinBox));
		info.AddProperty(PropertyName._minNextWaveHealthPercentageSpinBox, Variant.From(in _minNextWaveHealthPercentageSpinBox));
		info.AddProperty(PropertyName._maxNextWaveHealthPercentageSpinBox, Variant.From(in _maxNextWaveHealthPercentageSpinBox));
		info.AddProperty(PropertyName._tipsLabel, Variant.From(in _tipsLabel));
		info.AddProperty(PropertyName._flagSlider, Variant.From(in _flagSlider));
		info.AddProperty(PropertyName._waveLabel, Variant.From(in _waveLabel));
		info.AddProperty(PropertyName._levelEditorInspector, Variant.From(in _levelEditorInspector));
		info.AddProperty(PropertyName.levelConfig, Variant.From(in levelConfig));
		info.AddProperty(PropertyName.waveManagerConfig, Variant.From(in waveManagerConfig));
		info.AddProperty(PropertyName.isInit, Variant.From(in isInit));
		info.AddProperty(PropertyName.isLoad, Variant.From(in isLoad));
		info.AddProperty(PropertyName.isLoading, Variant.From(in isLoading));
		info.AddProperty(PropertyName._loadGeneration, Variant.From(in _loadGeneration));
		info.AddProperty(PropertyName._currentFlag, Variant.From(in _currentFlag));
		info.AddProperty(PropertyName.currentPacketContainer, Variant.From(in currentPacketContainer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.currentFlag, out var value))
		{
			currentFlag = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._spawnNode, out var value2))
		{
			_spawnNode = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._eventNode, out var value3))
		{
			_eventNode = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._zombieInvisibleCheckBox, out var value4))
		{
			_zombieInvisibleCheckBox = value4.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._poolPacketContainer, out var value5))
		{
			_poolPacketContainer = value5.As<LevelEditorWaveEditorPacketContainer>();
		}
		if (info.TryGetProperty(PropertyName._randomPacketContainer, out var value6))
		{
			_randomPacketContainer = value6.As<LevelEditorWaveEditorPacketContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetContainerListContainer, out var value7))
		{
			_packetContainerListContainer = value7.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._eventListContainer, out var value8))
		{
			_eventListContainer = value8.As<LevelEventListContainer>();
		}
		if (info.TryGetProperty(PropertyName._bigWaveSpinBox, out var value9))
		{
			_bigWaveSpinBox = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._waveIntervalSpinBox, out var value10))
		{
			_waveIntervalSpinBox = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._beginColSpinBox, out var value11))
		{
			_beginColSpinBox = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._spawnColEndSpinBox, out var value12))
		{
			_spawnColEndSpinBox = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._spawnColStartSpinBox, out var value13))
		{
			_spawnColStartSpinBox = value13.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._minNextWaveHealthPercentageSpinBox, out var value14))
		{
			_minNextWaveHealthPercentageSpinBox = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._maxNextWaveHealthPercentageSpinBox, out var value15))
		{
			_maxNextWaveHealthPercentageSpinBox = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._tipsLabel, out var value16))
		{
			_tipsLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._flagSlider, out var value17))
		{
			_flagSlider = value17.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._waveLabel, out var value18))
		{
			_waveLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._levelEditorInspector, out var value19))
		{
			_levelEditorInspector = value19.As<LevelEditorInspector>();
		}
		if (info.TryGetProperty(PropertyName.levelConfig, out var value20))
		{
			levelConfig = value20.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.waveManagerConfig, out var value21))
		{
			waveManagerConfig = value21.As<TowerDefenseLevelWaveManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.isInit, out var value22))
		{
			isInit = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isLoad, out var value23))
		{
			isLoad = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isLoading, out var value24))
		{
			isLoading = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._loadGeneration, out var value25))
		{
			_loadGeneration = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._currentFlag, out var value26))
		{
			_currentFlag = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentPacketContainer, out var value27))
		{
			currentPacketContainer = value27.As<LevelEditorWaveEditorPacketContainer>();
		}
	}
}
