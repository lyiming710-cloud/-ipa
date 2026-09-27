using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/SeedBank/TowerDefenseBattleFeatureSeedBank.cs")]
public class TowerDefenseBattleFeatureSeedBank : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName SubscribeSunDisplay = "SubscribeSunDisplay";

		public static readonly StringName UnsubscribeSunDisplay = "UnsubscribeSunDisplay";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName RestoreLegacySunIfNeeded = "RestoreLegacySunIfNeeded";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName seedBank = "seedBank";

		public static readonly StringName config = "config";

		public static readonly StringName _sunFeature = "_sunFeature";

		public static readonly StringName _lastCharacterQueryRevision = "_lastCharacterQueryRevision";

		public static readonly StringName _lastCostRefreshBucket = "_lastCostRefreshBucket";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _towerDefenseIngameSeedBank;

	public TowerDefenseInGameSeedBank seedBank;

	public TowerDefenseLevelSeedBankConfig config;

	private TowerDefenseBattleFeatureSun _sunFeature;

	private readonly EconomyAccountId _sunAccountId = EconomyAccountId.Local;

	private ulong _lastCharacterQueryRevision = 18446744073709551615uL;

	private ulong _lastCostRefreshBucket = 18446744073709551615uL;

	private static PackedScene TOWER_DEFENSE_INGAME_SEED_BANK => _towerDefenseIngameSeedBank ?? (_towerDefenseIngameSeedBank = GD.Load<PackedScene>("uid://2vh1csqbvbqx"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelSeedBankConfig();
		config.Init(data);
		seedBank = TOWER_DEFENSE_INGAME_SEED_BANK.Instantiate<TowerDefenseInGameSeedBank>(PackedScene.GenEditState.Disabled);
		if (!seedBank.TryBindSunAccount(EconomyAccountId.Local))
		{
			GD.PushError("[SeedBank] Failed to bind the local Sun account.");
		}
		control.AddUIToTopBankContainer(seedBank);
	}

	public override void OnReady()
	{
		if (config != null && config.method == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN && GodotObject.IsInstanceValid(seedBank))
		{
			seedBank.Visible = false;
		}
	}

	public override Task GameInit()
	{
		SubscribeSunDisplay();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		SubscribeSunDisplay();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (GodotObject.IsInstanceValid(seedBank))
		{
			seedBank.Start();
		}
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		if (GodotObject.IsInstanceValid(seedBank))
		{
			seedBank.StartFromProgress();
		}
		return Task.CompletedTask;
	}

	public override void Process(double _delta)
	{
		if (GodotObject.IsInstanceValid(seedBank) && seedBank.Visible && CommandManager.Instance != null && CommandManager.Instance.debugSunMax)
		{
			seedBank.ApplyDebugSunMax();
		}
		if (!GodotObject.IsInstanceValid(seedBank) || !seedBank.hasGameStarted)
		{
			return;
		}
		bool flag = false;
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			ulong queryRevision = instance.characterRegistry.QueryRevision;
			if (_lastCharacterQueryRevision != queryRevision)
			{
				_lastCharacterQueryRevision = queryRevision;
				flag = true;
			}
		}
		ulong num = Engine.GetPhysicsFrames() / 10;
		if (_lastCostRefreshBucket != num)
		{
			_lastCostRefreshBucket = num;
			flag = true;
		}
		if (flag)
		{
			seedBank.RefreshPacketRuntimeStates(includeCost: true);
		}
	}

	public override void Destroy()
	{
		UnsubscribeSunDisplay();
		if (GodotObject.IsInstanceValid(seedBank))
		{
			seedBank.DisposeOwnedPackets();
			seedBank.QueueFree();
		}
		seedBank = null;
		config = null;
		_lastCharacterQueryRevision = 18446744073709551615uL;
		_lastCostRefreshBucket = 18446744073709551615uL;
		base.Destroy();
	}

	private void SubscribeSunDisplay()
	{
		UnsubscribeSunDisplay();
		_sunFeature = GetFeature<TowerDefenseBattleFeatureSun>("Sun");
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange += OnAccountSunChanged;
		}
		if (GodotObject.IsInstanceValid(seedBank))
		{
			seedBank.RefreshSunDisplay();
			seedBank.RefreshPacketRuntimeStates(includeCost: true);
		}
	}

	private void UnsubscribeSunDisplay()
	{
		if (_sunFeature != null)
		{
			_sunFeature.OnAccountSunChange -= OnAccountSunChanged;
		}
		_sunFeature = null;
	}

	private void OnAccountSunChanged(EconomyAccountId accountId, long balance, SunTransactionReason _reason)
	{
		if (accountId == _sunAccountId && GodotObject.IsInstanceValid(seedBank))
		{
			seedBank.SetSunDisplay(balance);
			seedBank.RefreshPacketRuntimeStates(includeCost: false);
		}
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary();
	}

	public override void SyncDeserialize(Dictionary _data)
	{
	}

	public override Dictionary SaveFeature()
	{
		Array array = new Array();
		foreach (TowerDefenseInGamePacketShow item in GetPacketsForProgressSave())
		{
			Array array2 = new Array();
			if (GodotObject.IsInstanceValid(item.config))
			{
				foreach (TowerDefensePacketChangeCost changeCost in item.config.changeCostList)
				{
					if (changeCost != null)
					{
						array2.Add(changeCost.ExportSave());
					}
				}
			}
			array.Add(new Dictionary
			{
				["saveKey"] = item.config.saveKey,
				["showLove"] = item.showLove,
				["showCost"] = item.showCost,
				["onlyDraw"] = item.onlyDraw,
				["alive"] = item.alive,
				["lock"] = item.@lock,
				["plantOnce"] = item.plantOnce,
				["useCost"] = item.useCost,
				["openShadow"] = item.Get("openShadow").AsBool(),
				["start"] = item.start,
				["select"] = item.Get("select").AsBool(),
				["coldDown"] = item.Get("coldDown").AsDouble(),
				["coldDownOpen"] = item.coldDownOpen,
				["coldDownTimer"] = item.coldDownTimer,
				["aliveTime"] = item.aliveTime,
				["aliveTimer"] = item.aliveTimer,
				["blinkTimer"] = item.blinkTimer,
				["blink"] = item.blink,
				["height"] = item.height,
				["savePosX"] = item.savePos.X,
				["savePosY"] = item.savePos.Y,
				["overrideSave"] = (GodotObject.IsInstanceValid(item.config._override) ? item.config._override.Export() : new Dictionary()),
				["canChangeCost"] = item.config.canChangeCost,
				["changeCostList"] = array2
			});
		}
		return new Dictionary
		{
			["packetNum"] = array.Count,
			["packetList"] = array
		};
	}

	private List<TowerDefenseInGamePacketShow> GetPacketsForProgressSave()
	{
		List<TowerDefenseInGamePacketShow> packets = new List<TowerDefenseInGamePacketShow>();
		HashSet<TowerDefenseInGamePacketShow> seen = new HashSet<TowerDefenseInGamePacketShow>();
		if (!GodotObject.IsInstanceValid(seedBank))
		{
			return packets;
		}
		foreach (TowerDefenseInGamePacketShow packet2 in seedBank.packetList)
		{
			AddIfValid(packet2);
		}
		if (GodotObject.IsInstanceValid(seedBank.packetContainer))
		{
			foreach (Node child in seedBank.packetContainer.GetChildren())
			{
				if (child is TowerDefenseInGamePacketShow packet)
				{
					AddIfValid(packet);
				}
			}
		}
		return packets;
		void AddIfValid(TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
		{
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow) && GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config) && seen.Add(towerDefenseInGamePacketShow))
			{
				packets.Add(towerDefenseInGamePacketShow);
			}
		}
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		if (!GodotObject.IsInstanceValid(seedBank))
		{
			return;
		}
		RestoreLegacySunIfNeeded(_data, _owner);
		seedBank.ClearPacketsForProgressRestore();
		foreach (Variant item in _data.GetValueOrDefault("packetList", new Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			if (!TryCreateProgressPacketConfig(dictionary, out var runtimeConfig))
			{
				continue;
			}
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = seedBank.RestorePacketFromProgress(runtimeConfig);
			if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				continue;
			}
			towerDefenseInGamePacketShow.showLove = dictionary.GetValueOrDefault("showLove", false).AsBool();
			towerDefenseInGamePacketShow.showCost = dictionary.GetValueOrDefault("showCost", true).AsBool();
			towerDefenseInGamePacketShow.onlyDraw = dictionary.GetValueOrDefault("onlyDraw", false).AsBool();
			towerDefenseInGamePacketShow.alive = dictionary.GetValueOrDefault("alive", true).AsBool();
			towerDefenseInGamePacketShow.@lock = dictionary.GetValueOrDefault("lock", false).AsBool();
			towerDefenseInGamePacketShow.plantOnce = dictionary.GetValueOrDefault("plantOnce", false).AsBool();
			towerDefenseInGamePacketShow.useCost = dictionary.GetValueOrDefault("useCost", true).AsBool();
			towerDefenseInGamePacketShow.Set("openShadow", dictionary.GetValueOrDefault("openShadow", false).AsBool());
			towerDefenseInGamePacketShow.start = dictionary.GetValueOrDefault("start", false).AsBool();
			towerDefenseInGamePacketShow.Set("select", dictionary.GetValueOrDefault("select", false).AsBool());
			towerDefenseInGamePacketShow.Set("coldDown", dictionary.GetValueOrDefault("coldDown", 0.0).AsDouble());
			towerDefenseInGamePacketShow.coldDownOpen = dictionary.GetValueOrDefault("coldDownOpen", false).AsBool();
			towerDefenseInGamePacketShow.coldDownTimer = dictionary.GetValueOrDefault("coldDownTimer", 0.0).AsDouble();
			towerDefenseInGamePacketShow.aliveTime = dictionary.GetValueOrDefault("aliveTime", -1.0).AsDouble();
			towerDefenseInGamePacketShow.aliveTimer = dictionary.GetValueOrDefault("aliveTimer", 0.0).AsDouble();
			towerDefenseInGamePacketShow.blinkTimer = dictionary.GetValueOrDefault("blinkTimer", 0.0).AsDouble();
			towerDefenseInGamePacketShow.blink = dictionary.GetValueOrDefault("blink", false).AsBool();
			towerDefenseInGamePacketShow.height = dictionary.GetValueOrDefault("height", -1.0).AsDouble();
			towerDefenseInGamePacketShow.savePos = new Vector2((float)dictionary.GetValueOrDefault("savePosX", 0.0).AsDouble(), (float)dictionary.GetValueOrDefault("savePosY", 0.0).AsDouble());
			if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.config))
			{
				continue;
			}
			towerDefenseInGamePacketShow.config.canChangeCost = dictionary.GetValueOrDefault("canChangeCost", true).AsBool();
			towerDefenseInGamePacketShow.config.changeCostList = new List<TowerDefensePacketChangeCost>();
			foreach (Variant item2 in dictionary.GetValueOrDefault("changeCostList", new Array()).AsGodotArray())
			{
				Dictionary dictionary2 = item2.AsGodotDictionary();
				towerDefenseInGamePacketShow.config.changeCostList.Add(TowerDefensePacketChangeCost.ImportSave(dictionary2));
			}
		}
	}

	private void RestoreLegacySunIfNeeded(Dictionary savedSeedBank, TowerDefenseLevelSaveConfigCSharp owner)
	{
		if (savedSeedBank.ContainsKey("sunNum") && owner != null && !owner.featureSave.ContainsKey("Sun") && !owner.featureSave.ContainsKey(new StringName("Sun")))
		{
			GetFeature<TowerDefenseBattleFeatureSun>("Sun")?.SetSun(savedSeedBank["sunNum"].AsInt64());
		}
	}

	private bool TryCreateProgressPacketConfig(Dictionary packetData, out TowerDefensePacketConfig runtimeConfig)
	{
		runtimeConfig = null;
		string text = packetData.GetValueOrDefault("saveKey", "").AsString();
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return false;
		}
		runtimeConfig = packetConfig.Duplicate(deep: true) as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(runtimeConfig))
		{
			runtimeConfig = null;
			return false;
		}
		Dictionary dictionary = packetData.GetValueOrDefault("overrideSave", new Dictionary()).AsGodotDictionary();
		if (dictionary.Count > 0)
		{
			TowerDefensePacketOverride towerDefensePacketOverride = new TowerDefensePacketOverride();
			towerDefensePacketOverride.Init(dictionary);
			runtimeConfig._override = towerDefensePacketOverride;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SubscribeSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.RestoreLegacySunIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "savedSeedBank", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
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
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeSunDisplay && args.Count == 0)
		{
			SubscribeSunDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeSunDisplay && args.Count == 0)
		{
			UnsubscribeSunDisplay();
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
		if (method == MethodName.RestoreLegacySunIfNeeded && args.Count == 2)
		{
			RestoreLegacySunIfNeeded(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
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
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.SubscribeSunDisplay)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeSunDisplay)
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
		if (method == MethodName.RestoreLegacySunIfNeeded)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.seedBank)
		{
			seedBank = VariantUtils.ConvertTo<TowerDefenseInGameSeedBank>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelSeedBankConfig>(in value);
			return true;
		}
		if (name == PropertyName._sunFeature)
		{
			_sunFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSun>(in value);
			return true;
		}
		if (name == PropertyName._lastCharacterQueryRevision)
		{
			_lastCharacterQueryRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._lastCostRefreshBucket)
		{
			_lastCostRefreshBucket = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.seedBank)
		{
			value = VariantUtils.CreateFrom(in seedBank);
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
		if (name == PropertyName._lastCharacterQueryRevision)
		{
			value = VariantUtils.CreateFrom(in _lastCharacterQueryRevision);
			return true;
		}
		if (name == PropertyName._lastCostRefreshBucket)
		{
			value = VariantUtils.CreateFrom(in _lastCostRefreshBucket);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sunFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastCharacterQueryRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastCostRefreshBucket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.seedBank, Variant.From(in seedBank));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._sunFeature, Variant.From(in _sunFeature));
		info.AddProperty(PropertyName._lastCharacterQueryRevision, Variant.From(in _lastCharacterQueryRevision));
		info.AddProperty(PropertyName._lastCostRefreshBucket, Variant.From(in _lastCostRefreshBucket));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.seedBank, out var value))
		{
			seedBank = value.As<TowerDefenseInGameSeedBank>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseLevelSeedBankConfig>();
		}
		if (info.TryGetProperty(PropertyName._sunFeature, out var value3))
		{
			_sunFeature = value3.As<TowerDefenseBattleFeatureSun>();
		}
		if (info.TryGetProperty(PropertyName._lastCharacterQueryRevision, out var value4))
		{
			_lastCharacterQueryRevision = value4.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._lastCostRefreshBucket, out var value5))
		{
			_lastCostRefreshBucket = value5.As<ulong>();
		}
	}
}
