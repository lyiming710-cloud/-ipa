using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/RainMode/TowerDefenseBattleFeatureRainMode.cs")]
public class TowerDefenseBattleFeatureRainMode : TowerDefenseBattleFeature
{
	private readonly struct WeightedRainPacket(TowerDefenseRainModePacketConfig config, int weight)
	{
		public readonly TowerDefenseRainModePacketConfig Config = config;

		public readonly int Weight = weight;
	}

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName InitializeRuntime = "InitializeRuntime";

		public new static readonly StringName Process = "Process";

		public static readonly StringName Spawn = "Spawn";

		public static readonly StringName SpawnPacket = "SpawnPacket";

		public static readonly StringName IsSunType = "IsSunType";

		public static readonly StringName UpdateRainVisibility = "UpdateRainVisibility";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName SerializeRuntimeState = "SerializeRuntimeState";

		public static readonly StringName SerializeActivePackets = "SerializeActivePackets";

		public static readonly StringName RestoreActivePackets = "RestoreActivePackets";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName UnsubscribeSunDisplay = "UnsubscribeSunDisplay";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName rainManager = "rainManager";

		public static readonly StringName config = "config";

		public static readonly StringName _sunFeature = "_sunFeature";

		public static readonly StringName running = "running";

		public static readonly StringName timer = "timer";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private const double MinimumSpawnInterval = 0.05;

	private const double MinimumPacketAliveTime = 0.1;

	private static PackedScene _rainManagerScene;

	public RainManager rainManager;

	public TowerDefenseRainModeConfig config;

	private readonly List<TowerDefenseRainModePacketConfig> _packetList = new List<TowerDefenseRainModePacketConfig>();

	private readonly List<WeightedRainPacket> _weightedPackets = new List<WeightedRainPacket>();

	private readonly List<TowerDefenseInGamePacketShow> _activePackets = new List<TowerDefenseInGamePacketShow>();

	private TowerDefenseBattleFeatureSun _sunFeature;

	private readonly EconomyAccountId _sunAccountId = EconomyAccountId.Local;

	public bool running;

	public double timer;

	private static PackedScene RainManagerScene => _rainManagerScene ?? (_rainManagerScene = GD.Load<PackedScene>("res://Registry/Battle/Feature/RainMode/RainManager/RainManager.tscn"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseRainModeConfig();
		config.Init(data);
		rainManager = RainManagerScene.Instantiate<RainManager>(PackedScene.GenEditState.Disabled);
		control.AddUIToTopBankContainer(rainManager);
	}

	public override Task GameInit()
	{
		InitializeRuntime();
		if (data.Count == 0)
		{
			rainManager.Visible = false;
		}
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		InitializeRuntime();
		return Task.CompletedTask;
	}

	private void InitializeRuntime()
	{
		_packetList.Clear();
		foreach (Variant packet in config.packetList)
		{
			TowerDefenseRainModePacketConfig item = (TowerDefenseRainModePacketConfig)(GodotObject)packet;
			_packetList.Add(item);
		}
		UnsubscribeSunDisplay();
		_sunFeature = GetFeature<TowerDefenseBattleFeatureSun>("Sun");
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange += OnAccountSunChanged;
		}
		long sun = TowerDefenseManager.Instance.GetSun(_sunAccountId);
		rainManager.Init(config.type, sun);
	}

	public override Task GameReady()
	{
		running = false;
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (GodotObject.IsInstanceValid(rainManager) && rainManager.Visible)
		{
			running = true;
		}
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		return GameStart();
	}

	public override void Process(double delta)
	{
		if (running && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && _packetList.Count > 0)
		{
			double num = Math.Max(0.05, config.interval);
			timer += Math.Max(0.0, delta);
			if (!(timer < num))
			{
				timer %= num;
				Spawn();
			}
		}
	}

	public TowerDefenseInGamePacketShow Spawn()
	{
		if (_packetList.Count <= 0)
		{
			return null;
		}
		_weightedPackets.Clear();
		int num = 0;
		foreach (TowerDefenseRainModePacketConfig packet in _packetList)
		{
			double num2 = packet.weight;
			int characterNum = TowerDefenseManager.Instance.GetCharacterNum(packet.name);
			if (packet.maxNum != -1 && characterNum >= packet.maxNum)
			{
				num2 *= packet.maxMagnification;
			}
			if (packet.minNum != -1 && characterNum < packet.minNum)
			{
				num2 *= packet.minMagnification;
			}
			int num3 = Mathf.Max(0, (int)num2);
			if (num3 != 0)
			{
				_weightedPackets.Add(new WeightedRainPacket(packet, num3));
				num += num3;
			}
		}
		if (num <= 0)
		{
			return null;
		}
		int num4 = (int)(GD.Randi() % (uint)num);
		foreach (WeightedRainPacket weightedPacket in _weightedPackets)
		{
			num4 -= weightedPacket.Weight;
			if (num4 < 0)
			{
				return SpawnPacket(weightedPacket.Config.GetPacket());
			}
		}
		return null;
	}

	public TowerDefenseInGamePacketShow SpawnPacket(TowerDefensePacketConfig packetConfig)
	{
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return null;
		}
		double num = GD.RandRange(TowerDefenseManager.Instance.GetMapGridBeginPos().X, TowerDefenseManager.Instance.GetMapGroundRight());
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.Instance.SpawnPacket(_sunAccountId, packetConfig, new Vector2((float)num, TowerDefenseManager.Instance.GetMapGridBeginPos().Y - 100f), Math.Max(0.1, config.aliveTime), isFall: true, config.type == "Sun");
		if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
		{
			_activePackets.Add(towerDefenseInGamePacketShow);
		}
		return towerDefenseInGamePacketShow;
	}

	public bool IsSunType()
	{
		if (GodotObject.IsInstanceValid(config))
		{
			return config.type == "Sun";
		}
		return false;
	}

	public Dictionary UpdateRainVisibility(bool mobilePreset)
	{
		bool flag = IsSunType();
		return new Dictionary
		{
			["sun_visible"] = flag,
			["sun_bar_visible"] = flag,
			["mobile_sun_bar_visible"] = flag,
			["use_mobile_sun_label"] = flag & mobilePreset
		};
	}

	public override Dictionary SaveFeature()
	{
		return SerializeRuntimeState(includePackets: true);
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		timer = Math.Max(0.0, _data.GetValueOrDefault("timer", 0.0).AsDouble());
		running = _data.GetValueOrDefault("running", false).AsBool();
		if (GodotObject.IsInstanceValid(rainManager))
		{
			rainManager.Visible = _data.GetValueOrDefault("rain_visible", false).AsBool();
		}
		RestoreActivePackets(_data.GetValueOrDefault("packets", new Godot.Collections.Array()).AsGodotArray());
	}

	public override Dictionary SyncSerialize()
	{
		return SerializeRuntimeState(includePackets: false);
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		timer = Math.Max(0.0, _data.GetValueOrDefault("timer", timer).AsDouble());
		running = _data.GetValueOrDefault("running", running).AsBool();
		if (GodotObject.IsInstanceValid(rainManager))
		{
			rainManager.Visible = _data.GetValueOrDefault("rain_visible", rainManager.Visible).AsBool();
		}
	}

	private Dictionary SerializeRuntimeState(bool includePackets)
	{
		Dictionary dictionary = new Dictionary
		{
			["timer"] = timer,
			["running"] = running,
			["rain_visible"] = GodotObject.IsInstanceValid(rainManager) && rainManager.Visible
		};
		if (includePackets)
		{
			dictionary["packets"] = SerializeActivePackets();
		}
		return dictionary;
	}

	private Godot.Collections.Array SerializeActivePackets()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		for (int num = _activePackets.Count - 1; num >= 0; num--)
		{
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = _activePackets[num];
			if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow) || towerDefenseInGamePacketShow.IsQueuedForDeletion() || !GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config))
			{
				_activePackets.RemoveAt(num);
			}
			else
			{
				Dictionary dictionary = new Dictionary
				{
					["saveKey"] = towerDefenseInGamePacketShow.config.saveKey,
					["position"] = towerDefenseInGamePacketShow.GlobalPosition,
					["aliveTime"] = towerDefenseInGamePacketShow.aliveTime,
					["aliveTimer"] = towerDefenseInGamePacketShow.aliveTimer,
					["useCost"] = towerDefenseInGamePacketShow.useCost
				};
				TowerDefensePacketRuntimeState.Write(towerDefenseInGamePacketShow.config, dictionary, "packetOverride", "canChangeCost", "changeCostList");
				array.Add(dictionary);
			}
		}
		return array;
	}

	private void RestoreActivePackets(Godot.Collections.Array packetStates)
	{
		_activePackets.Clear();
		foreach (Variant packetState in packetStates)
		{
			Dictionary dictionary = packetState.AsGodotDictionary();
			if (!TowerDefensePacketRuntimeState.TryCreate(dictionary.GetValueOrDefault("saveKey", "").AsString(), dictionary, "packetOverride", "canChangeCost", "changeCostList", out var packetConfig) || !GodotObject.IsInstanceValid(packetConfig))
			{
				continue;
			}
			Vector2 vector = dictionary.GetValueOrDefault("position", Vector2.Zero).AsVector2();
			double num = Math.Max(0.1, dictionary.GetValueOrDefault("aliveTime", config.aliveTime).AsDouble());
			bool useCost = dictionary.GetValueOrDefault("useCost", config.type == "Sun").AsBool();
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.Instance.SpawnPacket(_sunAccountId, packetConfig, vector, num, isFall: false, useCost, useRandf: false, Vector2.Zero);
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.GlobalPosition = vector;
				towerDefenseInGamePacketShow.savePos = vector;
				towerDefenseInGamePacketShow.height = -1.0;
				towerDefenseInGamePacketShow.aliveTimer = Mathf.Clamp(dictionary.GetValueOrDefault("aliveTimer", 0.0).AsDouble(), 0.0, num);
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.moveComponent))
				{
					towerDefenseInGamePacketShow.moveComponent.QueueFree();
				}
				_activePackets.Add(towerDefenseInGamePacketShow);
			}
		}
	}

	private void OnAccountSunChanged(EconomyAccountId accountId, long balance, SunTransactionReason _reason)
	{
		if (accountId == _sunAccountId && GodotObject.IsInstanceValid(rainManager))
		{
			rainManager.SetSunDisplay(balance);
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		UnsubscribeSunDisplay();
		_packetList.Clear();
		_weightedPackets.Clear();
		_activePackets.Clear();
		running = false;
		if (GodotObject.IsInstanceValid(rainManager))
		{
			rainManager.QueueFree();
		}
		rainManager = null;
	}

	private void UnsubscribeSunDisplay()
	{
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange -= OnAccountSunChanged;
		}
		_sunFeature = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Spawn, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSunType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateRainVisibility, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			new MethodInfo(MethodName.SerializeRuntimeState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includePackets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SerializeActivePackets, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreActivePackets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "packetStates", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitializeRuntime && args.Count == 0)
		{
			InitializeRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.IsSunType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSunType());
			return true;
		}
		if (method == MethodName.UpdateRainVisibility && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(UpdateRainVisibility(VariantUtils.ConvertTo<bool>(in args[0])));
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
		if (method == MethodName.SerializeRuntimeState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SerializeRuntimeState(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.SerializeActivePackets && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(SerializeActivePackets());
			return true;
		}
		if (method == MethodName.RestoreActivePackets && args.Count == 1)
		{
			RestoreActivePackets(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeSunDisplay && args.Count == 0)
		{
			UnsubscribeSunDisplay();
			ret = default;
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
		if (method == MethodName.InitializeRuntime)
		{
			return true;
		}
		if (method == MethodName.Process)
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
		if (method == MethodName.IsSunType)
		{
			return true;
		}
		if (method == MethodName.UpdateRainVisibility)
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
		if (method == MethodName.SerializeRuntimeState)
		{
			return true;
		}
		if (method == MethodName.SerializeActivePackets)
		{
			return true;
		}
		if (method == MethodName.RestoreActivePackets)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeSunDisplay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.rainManager)
		{
			rainManager = VariantUtils.ConvertTo<RainManager>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseRainModeConfig>(in value);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			_sunFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSun>(in value);
			return true;
		}
		if (name == PropertyName.running)
		{
			running = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.rainManager)
		{
			value = VariantUtils.CreateFrom(in rainManager);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			value = VariantUtils.CreateFrom(in _sunFeature);
			return true;
		}
		if (name == PropertyName.running)
		{
			value = VariantUtils.CreateFrom(in running);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.rainManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.running, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.rainManager, Variant.From(in rainManager));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._sunFeature, Variant.From(in _sunFeature));
		info.AddProperty(PropertyName.running, Variant.From(in running));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.rainManager, out var value))
		{
			rainManager = value.As<RainManager>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseRainModeConfig>();
		}
		if (info.TryGetProperty(PropertyName._sunFeature, out var value3))
		{
			_sunFeature = value3.As<TowerDefenseBattleFeatureSun>();
		}
		if (info.TryGetProperty(PropertyName.running, out var value4))
		{
			running = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value5))
		{
			timer = value5.As<double>();
		}
	}
}
