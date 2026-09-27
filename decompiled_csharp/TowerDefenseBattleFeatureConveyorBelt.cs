using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/ConveyorBelt/TowerDefenseBattleFeatureConveyorBelt.cs")]
public class TowerDefenseBattleFeatureConveyorBelt : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName SerializePacketList = "SerializePacketList";

		public static readonly StringName RestorePacketList = "RestorePacketList";

		public static readonly StringName PacketListMatches = "PacketListMatches";

		public static readonly StringName ClearPacketList = "ClearPacketList";

		public static readonly StringName SerializePriorityPacketList = "SerializePriorityPacketList";

		public static readonly StringName RestorePriorityPacketList = "RestorePriorityPacketList";

		public static readonly StringName Spawn = "Spawn";

		public static readonly StringName SpawnPacket = "SpawnPacket";

		public static readonly StringName SpawnPacketFromSync = "SpawnPacketFromSync";

		public static readonly StringName CreatePacketShow = "CreatePacketShow";

		public static readonly StringName GetSpawnInterval = "GetSpawnInterval";

		public static readonly StringName SubscribeSunDisplay = "SubscribeSunDisplay";

		public static readonly StringName SubscribeWaveEvents = "SubscribeWaveEvents";

		public static readonly StringName UnsubscribeWaveEvents = "UnsubscribeWaveEvents";

		public static readonly StringName UnsubscribeSunDisplay = "UnsubscribeSunDisplay";

		public static readonly StringName WaveEventExecute = "WaveEventExecute";

		public static readonly StringName IsSunType = "IsSunType";

		public static readonly StringName UpdateConveyorVisibility = "UpdateConveyorVisibility";

		public static readonly StringName GetPacketChildren = "GetPacketChildren";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName conveyorBeltManager = "conveyorBeltManager";

		public static readonly StringName config = "config";

		public static readonly StringName packetPrioritySpawnList = "packetPrioritySpawnList";

		public static readonly StringName packetList = "packetList";

		public static readonly StringName timer = "timer";

		public static readonly StringName running = "running";

		public static readonly StringName _sunFeature = "_sunFeature";

		public static readonly StringName _waveFeature = "_waveFeature";

		public static readonly StringName _weightPickItems = "_weightPickItems";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private const double MinimumSpawnInterval = 0.05;

	private static PackedScene _conveyorBeltManagerScene;

	public ConveyorBeltManager conveyorBeltManager;

	public TowerDefenseConveyorConfig config;

	public Array<TowerDefenseLevelPacketConfig> packetPrioritySpawnList = new Array<TowerDefenseLevelPacketConfig>();

	public Array<TowerDefenseConveyorPacketConfig> packetList = new Array<TowerDefenseConveyorPacketConfig>();

	public double timer;

	public bool running;

	private TowerDefenseBattleFeatureSun _sunFeature;

	private TowerDefenseBattleFeatureWave _waveFeature;

	private readonly EconomyAccountId _sunAccountId = EconomyAccountId.Local;

	private readonly Array<WeightPickItemBase> _weightPickItems = new Array<WeightPickItemBase>();

	private static PackedScene ConveyorBeltManagerScene => _conveyorBeltManagerScene ?? (_conveyorBeltManagerScene = GD.Load<PackedScene>("uid://u3bg1snrdknl"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseConveyorConfig();
		config.Init(data);
		conveyorBeltManager = ConveyorBeltManagerScene.Instantiate<ConveyorBeltManager>(PackedScene.GenEditState.Disabled);
		if (!conveyorBeltManager.TryBindSunAccount(_sunAccountId))
		{
			GD.PushError("[ConveyorBelt] Failed to bind the local Sun account.");
		}
		control.AddUIToTopBankContainer(conveyorBeltManager);
		packetPrioritySpawnList = config.packetPrioritySpawnList.Duplicate(deep: true);
		packetList = config.packetList.Duplicate(deep: true);
		conveyorBeltManager.Init(config.type);
	}

	public override void Destroy()
	{
		UnsubscribeWaveEvents();
		UnsubscribeSunDisplay();
		_weightPickItems.Clear();
		base.Destroy();
	}

	public override Task GameInit()
	{
		SubscribeWaveEvents();
		SubscribeSunDisplay();
		if (data.Count == 0)
		{
			conveyorBeltManager.Visible = false;
		}
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		SubscribeWaveEvents();
		SubscribeSunDisplay();
		if (data.Count > 0 && GodotObject.IsInstanceValid(conveyorBeltManager))
		{
			conveyorBeltManager.Visible = true;
		}
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (GodotObject.IsInstanceValid(conveyorBeltManager) && conveyorBeltManager.Visible)
		{
			running = true;
		}
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		if (GodotObject.IsInstanceValid(conveyorBeltManager) && conveyorBeltManager.Visible)
		{
			running = true;
		}
		return Task.CompletedTask;
	}

	public override void Process(double _delta)
	{
		if (!running)
		{
			return;
		}
		conveyorBeltManager.UpdateBeltAnimation(_delta);
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		int packetCount = conveyorBeltManager.GetPacketCount();
		if (packetCount < config.maxPacketCount)
		{
			timer += Math.Max(0.0, _delta);
			double spawnInterval = GetSpawnInterval(packetCount);
			if (!(timer < spawnInterval))
			{
				timer %= spawnInterval;
				Spawn();
			}
		}
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary
		{
			["timer"] = timer,
			["running"] = running,
			["packetList"] = SerializePacketList()
		};
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (_data.ContainsKey("timer"))
		{
			timer = _data["timer"].AsDouble();
		}
		if (_data.ContainsKey("running"))
		{
			running = _data["running"].AsBool();
		}
		if (_data.ContainsKey("packetList"))
		{
			RestorePacketList(_data["packetList"].AsGodotArray(), force: false);
		}
	}

	public override Dictionary SaveFeature()
	{
		return new Dictionary
		{
			["timer"] = timer,
			["running"] = running,
			["packetList"] = SerializePacketList(),
			["priorityPacketList"] = SerializePriorityPacketList()
		};
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		timer = _data.GetValueOrDefault("timer", 0.0).AsDouble();
		running = _data.GetValueOrDefault("running", false).AsBool();
		if (_data.ContainsKey("priorityPacketList"))
		{
			RestorePriorityPacketList(_data["priorityPacketList"].AsGodotArray());
		}
		RestorePacketList(_data.GetValueOrDefault("packetList", new Godot.Collections.Array()).AsGodotArray(), force: true);
	}

	private Godot.Collections.Array SerializePacketList()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		if (!GodotObject.IsInstanceValid(conveyorBeltManager))
		{
			return array;
		}
		foreach (Node packetChild in conveyorBeltManager.GetPacketChildren(conveyorBeltManager.isMobileUI))
		{
			if (packetChild is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow) && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config))
			{
				Dictionary dictionary = new Dictionary { ["saveKey"] = towerDefenseInGamePacketShow.config.saveKey };
				TowerDefensePacketRuntimeState.Write(towerDefenseInGamePacketShow.config, dictionary, "packetOverride", "canChangeCost", "changeCostList");
				array.Add(dictionary);
			}
		}
		return array;
	}

	private void RestorePacketList(Godot.Collections.Array packetListData, bool force)
	{
		if (!GodotObject.IsInstanceValid(conveyorBeltManager) || (!force && PacketListMatches(packetListData)))
		{
			return;
		}
		ClearPacketList();
		foreach (Variant packetListDatum in packetListData)
		{
			Dictionary dictionary = packetListDatum.AsGodotDictionary();
			if (TowerDefensePacketRuntimeState.TryCreate(dictionary.GetValueOrDefault("saveKey", "").AsString(), dictionary, "packetOverride", "canChangeCost", "changeCostList", out var packetConfig) && GodotObject.IsInstanceValid(packetConfig))
			{
				CreatePacketShow(packetConfig, config.type);
			}
		}
		conveyorBeltManager.ResetPacketPositions();
	}

	private bool PacketListMatches(Godot.Collections.Array packetListData)
	{
		Array<Node> packetChildren = conveyorBeltManager.GetPacketChildren(conveyorBeltManager.isMobileUI);
		if (packetChildren.Count != packetListData.Count)
		{
			return false;
		}
		for (int i = 0; i < packetChildren.Count; i++)
		{
			if (!(packetChildren[i] is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow) || !GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config))
			{
				return false;
			}
			Dictionary dictionary = packetListData[i].AsGodotDictionary();
			if (towerDefenseInGamePacketShow.config.saveKey != dictionary.GetValueOrDefault("saveKey", "").AsString())
			{
				return false;
			}
		}
		return true;
	}

	private void ClearPacketList()
	{
		foreach (Node packetChild in conveyorBeltManager.GetPacketChildren(conveyorBeltManager.isMobileUI))
		{
			if (GodotObject.IsInstanceValid(packetChild))
			{
				Node parent = packetChild.GetParent();
				if (GodotObject.IsInstanceValid(parent))
				{
					parent.RemoveChild(packetChild);
				}
				packetChild.QueueFree();
			}
		}
	}

	private Godot.Collections.Array SerializePriorityPacketList()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseLevelPacketConfig packetPrioritySpawn in packetPrioritySpawnList)
		{
			if (GodotObject.IsInstanceValid(packetPrioritySpawn))
			{
				array.Add(packetPrioritySpawn.Export());
			}
		}
		return array;
	}

	private void RestorePriorityPacketList(Godot.Collections.Array priorityPacketData)
	{
		packetPrioritySpawnList.Clear();
		foreach (Variant priorityPacketDatum in priorityPacketData)
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = new TowerDefenseLevelPacketConfig();
			towerDefenseLevelPacketConfig.Init(priorityPacketDatum);
			if (towerDefenseLevelPacketConfig.packetName != "")
			{
				packetPrioritySpawnList.Add(towerDefenseLevelPacketConfig);
			}
		}
	}

	public TowerDefenseInGamePacketShow Spawn()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		TowerDefensePacketConfig towerDefensePacketConfig = null;
		if (packetPrioritySpawnList.Count <= 0)
		{
			_weightPickItems.Clear();
			foreach (TowerDefenseConveyorPacketConfig packet in packetList)
			{
				double num = packet.weight;
				int characterNum = TowerDefenseManager.Instance.GetCharacterNum(packet.name, containConveyor: true);
				if (packet.maxNum != -1 && characterNum >= packet.maxNum)
				{
					num *= packet.maxMagnification;
				}
				if (packet.minNum != -1 && characterNum < packet.minNum)
				{
					num *= packet.minMagnification;
				}
				_weightPickItems.Add(new WeightPickItemBase(packet, Math.Max(0, (int)num)));
			}
			WeightPickItemBase weightPickItemBase = WeightPickMathine.Pick(_weightPickItems);
			if (GodotObject.IsInstanceValid(weightPickItemBase))
			{
				towerDefensePacketConfig = (weightPickItemBase.item.AsGodotObject() as TowerDefenseConveyorPacketConfig).GetPacket();
			}
		}
		else
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = packetPrioritySpawnList[0];
			packetPrioritySpawnList.RemoveAt(0);
			towerDefensePacketConfig = towerDefenseLevelPacketConfig.GetPacket();
		}
		TowerDefenseInGamePacketShow result = SpawnPacket(towerDefensePacketConfig);
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			MultiPlayerManager.Instance.SendConveyorSpawn(towerDefensePacketConfig.saveKey, config.type);
		}
		return result;
	}

	public TowerDefenseInGamePacketShow SpawnPacket(TowerDefensePacketConfig packetConfig)
	{
		return CreatePacketShow(packetConfig, config.type);
	}

	public TowerDefenseInGamePacketShow SpawnPacketFromSync(TowerDefensePacketConfig packetConfig, string packetType)
	{
		return CreatePacketShow(packetConfig, packetType);
	}

	private TowerDefenseInGamePacketShow CreatePacketShow(TowerDefensePacketConfig packetConfig, string packetType)
	{
		if (!GodotObject.IsInstanceValid(conveyorBeltManager))
		{
			return null;
		}
		if (conveyorBeltManager.GetPacketCount() >= config.maxPacketCount)
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(packetConfig))
		{
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			conveyorBeltManager.AddPacketToUI(towerDefenseInGamePacketShow);
			towerDefenseInGamePacketShow.Init(packetConfig);
			if (!towerDefenseInGamePacketShow.TryBindSunAccount(_sunAccountId))
			{
				GD.PushError("[ConveyorBelt] Packet already belongs to a different Sun account.");
				towerDefenseInGamePacketShow.QueueFree();
				return null;
			}
			towerDefenseInGamePacketShow.coldDownOpen = false;
			towerDefenseInGamePacketShow.onlyDraw = false;
			towerDefenseInGamePacketShow.plantOnce = true;
			towerDefenseInGamePacketShow.StartInit();
			towerDefenseInGamePacketShow.alive = true;
			bool start = (towerDefenseInGamePacketShow.showCost = (towerDefenseInGamePacketShow.useCost = packetType == "Sun"));
			towerDefenseInGamePacketShow.start = start;
			towerDefenseInGamePacketShow.Reset();
			towerDefenseInGamePacketShow.OnPressed += TowerDefenseManager.Instance.GetPacketPickControl().PickPacket;
			return towerDefenseInGamePacketShow;
		}
		return null;
	}

	private double GetSpawnInterval(int packetCount)
	{
		int num = Math.Max(1, config.intervalIncreaseEvery);
		double num2 = Math.Floor((double)Math.Max(0, packetCount) / (double)num);
		double num3 = Math.Max(0.0, 1.0 + num2 * config.intervalMagnification);
		return Math.Max(0.05, config.interval * num3);
	}

	private void SubscribeSunDisplay()
	{
		UnsubscribeSunDisplay();
		_sunFeature = GetFeature<TowerDefenseBattleFeatureSun>("Sun");
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange += OnAccountSunChanged;
		}
		if (GodotObject.IsInstanceValid(conveyorBeltManager))
		{
			conveyorBeltManager.UpdateSunDisplay();
		}
	}

	private void SubscribeWaveEvents()
	{
		UnsubscribeWaveEvents();
		_waveFeature = GetFeature<TowerDefenseBattleFeatureWave>("Wave");
		if (_waveFeature != null)
		{
			_waveFeature.OnBigWaveBegin += WaveEventExecute;
		}
	}

	private void UnsubscribeWaveEvents()
	{
		if (_waveFeature != null)
		{
			_waveFeature.OnBigWaveBegin -= WaveEventExecute;
		}
		_waveFeature = null;
	}

	private void UnsubscribeSunDisplay()
	{
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange -= OnAccountSunChanged;
		}
		_sunFeature = null;
	}

	private void OnAccountSunChanged(EconomyAccountId accountId, long _balance, SunTransactionReason _reason)
	{
		if (accountId == _sunAccountId && GodotObject.IsInstanceValid(conveyorBeltManager))
		{
			conveyorBeltManager.UpdateSunDisplay();
		}
	}

	public void WaveEventExecute(int bigWaveId)
	{
		if (!conveyorBeltManager.Visible || !running || config.waveEvent.Count <= bigWaveId)
		{
			return;
		}
		foreach (Variant item in config.waveEvent[bigWaveId].AsGodotArray())
		{
			item.As<TowerDefenseConveyorEventBase>()?.Execute();
		}
	}

	public bool IsSunType()
	{
		if (GodotObject.IsInstanceValid(config))
		{
			return config.type == "Sun";
		}
		return false;
	}

	public Dictionary UpdateConveyorVisibility(bool mobilePreset)
	{
		Dictionary dictionary = new Dictionary
		{
			["sun_visible"] = false,
			["sun_bar_visible"] = false,
			["mobile_sun_bar_visible"] = false,
			["use_mobile_sun_label"] = false
		};
		bool flag = IsSunType();
		dictionary["sun_visible"] = flag;
		dictionary["sun_bar_visible"] = flag;
		dictionary["mobile_sun_bar_visible"] = flag;
		dictionary["use_mobile_sun_label"] = flag & mobilePreset;
		return dictionary;
	}

	public Array<Node> GetPacketChildren()
	{
		return conveyorBeltManager.GetPacketChildren(conveyorBeltManager.isMobileUI);
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
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SerializePacketList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestorePacketList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "packetListData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketListMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "packetListData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPacketList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SerializePriorityPacketList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestorePriorityPacketList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "priorityPacketData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Spawn, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnPacketFromSync, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePacketShow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSpawnInterval, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SubscribeSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SubscribeWaveEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeWaveEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WaveEventExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "bigWaveId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSunType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateConveyorVisibility, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketChildren, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.SerializePacketList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(SerializePacketList());
			return true;
		}
		if (method == MethodName.RestorePacketList && args.Count == 2)
		{
			RestorePacketList(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketListMatches && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PacketListMatches(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearPacketList && args.Count == 0)
		{
			ClearPacketList();
			ret = default;
			return true;
		}
		if (method == MethodName.SerializePriorityPacketList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(SerializePriorityPacketList());
			return true;
		}
		if (method == MethodName.RestorePriorityPacketList && args.Count == 1)
		{
			RestorePriorityPacketList(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Spawn && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(Spawn());
			return true;
		}
		if (method == MethodName.SpawnPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(SpawnPacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.SpawnPacketFromSync && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(SpawnPacketFromSync(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreatePacketShow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacketShow(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSpawnInterval && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetSpawnInterval(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SubscribeSunDisplay && args.Count == 0)
		{
			SubscribeSunDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeWaveEvents && args.Count == 0)
		{
			SubscribeWaveEvents();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeWaveEvents && args.Count == 0)
		{
			UnsubscribeWaveEvents();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeSunDisplay && args.Count == 0)
		{
			UnsubscribeSunDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.WaveEventExecute && args.Count == 1)
		{
			WaveEventExecute(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSunType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSunType());
			return true;
		}
		if (method == MethodName.UpdateConveyorVisibility && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(UpdateConveyorVisibility(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketChildren && args.Count == 0)
		{
			Array<Node> packetChildren = GetPacketChildren();
			ret = VariantUtils.CreateFromArray(packetChildren);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.Process)
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
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.SerializePacketList)
		{
			return true;
		}
		if (method == MethodName.RestorePacketList)
		{
			return true;
		}
		if (method == MethodName.PacketListMatches)
		{
			return true;
		}
		if (method == MethodName.ClearPacketList)
		{
			return true;
		}
		if (method == MethodName.SerializePriorityPacketList)
		{
			return true;
		}
		if (method == MethodName.RestorePriorityPacketList)
		{
			return true;
		}
		if (method == MethodName.Spawn)
		{
			return true;
		}
		if (method == MethodName.SpawnPacket)
		{
			return true;
		}
		if (method == MethodName.SpawnPacketFromSync)
		{
			return true;
		}
		if (method == MethodName.CreatePacketShow)
		{
			return true;
		}
		if (method == MethodName.GetSpawnInterval)
		{
			return true;
		}
		if (method == MethodName.SubscribeSunDisplay)
		{
			return true;
		}
		if (method == MethodName.SubscribeWaveEvents)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeWaveEvents)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeSunDisplay)
		{
			return true;
		}
		if (method == MethodName.WaveEventExecute)
		{
			return true;
		}
		if (method == MethodName.IsSunType)
		{
			return true;
		}
		if (method == MethodName.UpdateConveyorVisibility)
		{
			return true;
		}
		if (method == MethodName.GetPacketChildren)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.conveyorBeltManager)
		{
			conveyorBeltManager = VariantUtils.ConvertTo<ConveyorBeltManager>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseConveyorConfig>(in value);
			return true;
		}
		if (name == PropertyName.packetPrioritySpawnList)
		{
			packetPrioritySpawnList = VariantUtils.ConvertToArray<TowerDefenseLevelPacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseConveyorPacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.running)
		{
			running = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			_sunFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSun>(in value);
			return true;
		}
		if (name == PropertyName._waveFeature)
		{
			_waveFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.conveyorBeltManager)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltManager);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.packetPrioritySpawnList)
		{
			value = VariantUtils.CreateFromArray(packetPrioritySpawnList);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.running)
		{
			value = VariantUtils.CreateFrom(in running);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			value = VariantUtils.CreateFrom(in _sunFeature);
			return true;
		}
		if (name == PropertyName._waveFeature)
		{
			value = VariantUtils.CreateFrom(in _waveFeature);
			return true;
		}
		if (name == PropertyName._weightPickItems)
		{
			value = VariantUtils.CreateFromArray(_weightPickItems);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetPrioritySpawnList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.running, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._weightPickItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.conveyorBeltManager, Variant.From(in conveyorBeltManager));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.packetPrioritySpawnList, Variant.CreateFrom(packetPrioritySpawnList));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.running, Variant.From(in running));
		info.AddProperty(PropertyName._sunFeature, Variant.From(in _sunFeature));
		info.AddProperty(PropertyName._waveFeature, Variant.From(in _waveFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.conveyorBeltManager, out var value))
		{
			conveyorBeltManager = value.As<ConveyorBeltManager>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseConveyorConfig>();
		}
		if (info.TryGetProperty(PropertyName.packetPrioritySpawnList, out var value3))
		{
			packetPrioritySpawnList = value3.AsGodotArray<TowerDefenseLevelPacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value4))
		{
			packetList = value4.AsGodotArray<TowerDefenseConveyorPacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value5))
		{
			timer = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.running, out var value6))
		{
			running = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sunFeature, out var value7))
		{
			_sunFeature = value7.As<TowerDefenseBattleFeatureSun>();
		}
		if (info.TryGetProperty(PropertyName._waveFeature, out var value8))
		{
			_waveFeature = value8.As<TowerDefenseBattleFeatureWave>();
		}
	}
}
