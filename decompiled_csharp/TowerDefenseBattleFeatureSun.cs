using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Sun/TowerDefenseBattleFeatureSun.cs")]
public class TowerDefenseBattleFeatureSun : TowerDefenseBattleFeature
{
	public delegate void SunCollectEventHandler(long num);

	public delegate void SunChangeEventHandler(long num);

	public delegate void AccountSunCollectEventHandler(EconomyAccountId accountId, long num);

	public delegate void AccountSunChangeEventHandler(EconomyAccountId accountId, long balance, SunTransactionReason reason);

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public static readonly StringName EmitSunCollect = "EmitSunCollect";

		public static readonly StringName EmitSunChange = "EmitSunChange";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName AddSun = "AddSun";

		public static readonly StringName UseSun = "UseSun";

		public static readonly StringName SetSun = "SetSun";

		public static readonly StringName SunInit = "SunInit";

		public static readonly StringName SunCreate = "SunCreate";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName NextSpendTransactionId = "NextSpendTransactionId";

		public static readonly StringName InvalidateSpendReceipts = "InvalidateSpendReceipts";

		public static readonly StringName SaturatingAdd = "SaturatingAdd";

		public static readonly StringName SaturatingSubtract = "SaturatingSubtract";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName config = "config";

		public static readonly StringName _mapFeature = "_mapFeature";

		public static readonly StringName type = "type";

		public static readonly StringName spawnInterval = "spawnInterval";

		public static readonly StringName spawnNum = "spawnNum";

		public static readonly StringName movingMethod = "movingMethod";

		public static readonly StringName isRunning = "isRunning";

		public static readonly StringName timer = "timer";

		public static readonly StringName _ledgerGeneration = "_ledgerGeneration";

		public static readonly StringName _nextSpendTransactionId = "_nextSpendTransactionId";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private const int AccountSchemaVersion = 1;

	private const long LegacyDefaultSun = 50L;

	public TowerDefenseLevelSunManagerConfig config;

	private TowerDefenseBattleFeatureMap _mapFeature;

	public string type = "Normal";

	public double spawnInterval = 25.0;

	public long spawnNum = 25L;

	public TowerDefenseEnum.SUN_MOVING_METHOD movingMethod;

	public bool isRunning;

	public double timer;

	private readonly System.Collections.Generic.Dictionary<EconomyAccountId, long> _accountBalances = new System.Collections.Generic.Dictionary<EconomyAccountId, long>();

	private readonly System.Collections.Generic.Dictionary<long, SunSpendReceipt> _activeSpendReceipts = new System.Collections.Generic.Dictionary<long, SunSpendReceipt>();

	private long _ledgerGeneration;

	private long _nextSpendTransactionId;

	public EconomyAccountId LocalAccountId => EconomyAccountId.Local;

	public long sunNum
	{
		get
		{
			if (!TryGetSun(LocalAccountId, out var balance))
			{
				return 50L;
			}
			return balance;
		}
		set
		{
			if (!SetSun(LocalAccountId, value, SunTransactionReason.Legacy))
			{
				RegisterAccount(LocalAccountId, value);
			}
		}
	}

	public event SunCollectEventHandler OnSunCollect;

	public event SunChangeEventHandler OnSunChange;

	public event AccountSunCollectEventHandler OnAccountSunCollect;

	public event AccountSunChangeEventHandler OnAccountSunChange;

	public void EmitSunCollect(long num)
	{
		EmitSunCollect(LocalAccountId, num);
	}

	public void EmitSunChange(long num)
	{
		EmitSunChange(LocalAccountId, num, SunTransactionReason.Legacy);
	}

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelSunManagerConfig();
		config.Init(data);
		InvalidateSpendReceipts();
		_accountBalances.Clear();
		RegisterAccount(LocalAccountId, config.begin);
	}

	public override Task GameInit()
	{
		_mapFeature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
		SetSun(LocalAccountId, config.begin, SunTransactionReason.Initialization);
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		_mapFeature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (GetFeature("Wave") is TowerDefenseBattleFeatureWave { isSurvival: not false } towerDefenseBattleFeatureWave && GodotObject.IsInstanceValid(towerDefenseBattleFeatureWave.survivalRunner) && towerDefenseBattleFeatureWave.survivalRunner.roundNum > 0)
		{
			type = config.type;
			spawnInterval = config.spawnInterval;
			spawnNum = config.spawnNum;
			movingMethod = config.movingMethod;
			timer = spawnInterval - 6.0;
		}
		else
		{
			SunInit(config);
		}
		isRunning = true;
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		isRunning = true;
		return Task.CompletedTask;
	}

	public override void GameFail()
	{
		isRunning = false;
	}

	public override void Process(double delta)
	{
		if (isRunning && config.open && GodotObject.IsInstanceValid(_mapFeature) && (!_mapFeature.config.isNight || _mapFeature.config.useSunFall))
		{
			if (timer < spawnInterval)
			{
				timer += delta;
				return;
			}
			SunCreate();
			timer = 0.0;
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
		Dictionary dictionary = new Dictionary { ["local"] = sunNum };
		return new Dictionary
		{
			["timer"] = timer,
			["isRunning"] = isRunning,
			["spawnInterval"] = spawnInterval,
			["spawnNum"] = spawnNum,
			["type"] = type,
			["movingMethod"] = (int)movingMethod,
			["sunNum"] = sunNum,
			["accountSchemaVersion"] = 1,
			["accounts"] = dictionary
		};
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		timer = _data.GetValueOrDefault("timer", 0.0).AsDouble();
		isRunning = _data.GetValueOrDefault("isRunning", false).AsBool();
		spawnInterval = _data.GetValueOrDefault("spawnInterval", spawnInterval).AsDouble();
		spawnNum = _data.GetValueOrDefault("spawnNum", spawnNum).AsInt64();
		type = _data.GetValueOrDefault("type", type).AsString();
		movingMethod = (TowerDefenseEnum.SUN_MOVING_METHOD)_data.GetValueOrDefault("movingMethod", (int)movingMethod).AsInt32();
		long num = _data.GetValueOrDefault("sunNum", sunNum).AsInt64();
		if (_data.TryGetValue("accounts", out var value) && value.VariantType == Variant.Type.Dictionary && value.AsGodotDictionary().TryGetValue("local", out var value2))
		{
			num = value2.AsInt64();
		}
		InvalidateSpendReceipts();
		_accountBalances.Clear();
		RegisterAccount(LocalAccountId, num);
		EmitSunChange(LocalAccountId, num, SunTransactionReason.Load);
	}

	public bool RegisterAccount(EconomyAccountId accountId, long initialSun)
	{
		if (!accountId.IsValid)
		{
			return false;
		}
		if (_accountBalances.ContainsKey(accountId))
		{
			return true;
		}
		_accountBalances.Add(accountId, initialSun);
		return true;
	}

	public bool HasAccount(EconomyAccountId accountId)
	{
		if (accountId.IsValid)
		{
			return _accountBalances.ContainsKey(accountId);
		}
		return false;
	}

	public bool TryGetSun(EconomyAccountId accountId, out long balance)
	{
		balance = 0L;
		if (accountId.IsValid)
		{
			return _accountBalances.TryGetValue(accountId, out balance);
		}
		return false;
	}

	public long GetSun(EconomyAccountId accountId)
	{
		if (!TryGetSun(accountId, out var balance))
		{
			return -1L;
		}
		return balance;
	}

	public bool CreditSun(EconomyAccountId accountId, long amount, SunTransactionReason reason = SunTransactionReason.Reward)
	{
		if (amount < 0)
		{
			return false;
		}
		return ApplyBalanceDelta(accountId, amount, reason);
	}

	internal bool ApplyCollectedSunValue(EconomyAccountId accountId, long value)
	{
		return ApplyBalanceDelta(accountId, value, (value >= 0) ? SunTransactionReason.Collection : SunTransactionReason.Penalty);
	}

	private bool ApplyBalanceDelta(EconomyAccountId accountId, long delta, SunTransactionReason reason)
	{
		if (!TryGetSun(accountId, out var balance))
		{
			return false;
		}
		if (delta == 0L)
		{
			return true;
		}
		long num = SaturatingAdd(balance, delta);
		long num2 = num - balance;
		_accountBalances[accountId] = num;
		if (num2 > 0 && reason == SunTransactionReason.Collection)
		{
			EmitSunCollect(accountId, num2);
		}
		EmitSunChange(accountId, num, reason);
		return true;
	}

	public bool TrySpendSun(EconomyAccountId accountId, long amount, SunTransactionReason reason = SunTransactionReason.Cost)
	{
		if (amount < 0 || !TryGetSun(accountId, out var balance) || balance < amount)
		{
			return false;
		}
		if (amount == 0L)
		{
			return true;
		}
		long num = balance - amount;
		_accountBalances[accountId] = num;
		EmitSunChange(accountId, num, reason);
		return true;
	}

	public bool TryBeginSunSpend(EconomyAccountId accountId, long amount, out SunSpendReceipt receipt, SunTransactionReason reason = SunTransactionReason.Cost)
	{
		receipt = null;
		if (amount >= 0)
		{
			if (!TrySpendSun(accountId, amount, reason))
			{
				return false;
			}
		}
		else if (amount == -9223372036854775808L || !CreditSun(accountId, -amount, reason))
		{
			return false;
		}
		long num = NextSpendTransactionId();
		receipt = new SunSpendReceipt(this, _ledgerGeneration, num, accountId, amount);
		_activeSpendReceipts.Add(num, receipt);
		return true;
	}

	internal bool TryCommitSunSpend(SunSpendReceipt receipt)
	{
		if (!TryGetActiveSpendReceipt(receipt, out var transactionId))
		{
			return false;
		}
		_activeSpendReceipts.Remove(transactionId);
		receipt.Complete(SunSpendReceiptState.Committed);
		return true;
	}

	internal bool TryRollbackSunSpend(SunSpendReceipt receipt)
	{
		if (!TryGetActiveSpendReceipt(receipt, out var transactionId))
		{
			return false;
		}
		if (!((receipt.Amount >= 0) ? CreditSun(receipt.AccountId, receipt.Amount, SunTransactionReason.Refund) : ApplyBalanceDelta(receipt.AccountId, receipt.Amount, SunTransactionReason.Refund)))
		{
			return false;
		}
		_activeSpendReceipts.Remove(transactionId);
		receipt.Complete(SunSpendReceiptState.RolledBack);
		return true;
	}

	public bool SetSun(EconomyAccountId accountId, long value, SunTransactionReason reason = SunTransactionReason.Debug)
	{
		if (!HasAccount(accountId))
		{
			return false;
		}
		_accountBalances[accountId] = value;
		EmitSunChange(accountId, value, reason);
		return true;
	}

	public void AddSun(long num)
	{
		long balance;
		if (num >= 0)
		{
			CreditSun(LocalAccountId, num, SunTransactionReason.Collection);
		}
		else if (TryGetSun(LocalAccountId, out balance))
		{
			long num2 = SaturatingAdd(balance, num);
			_accountBalances[LocalAccountId] = num2;
			EmitSunCollect(LocalAccountId, num);
			EmitSunChange(LocalAccountId, num2, SunTransactionReason.Legacy);
		}
	}

	public void UseSun(long num)
	{
		if (TryGetSun(LocalAccountId, out var balance))
		{
			SetSun(LocalAccountId, SaturatingSubtract(balance, num), SunTransactionReason.Legacy);
		}
	}

	public void SetSun(long num)
	{
		SetSun(LocalAccountId, num, SunTransactionReason.Legacy);
	}

	public void SunInit(TowerDefenseLevelSunManagerConfig _config)
	{
		SetSun(LocalAccountId, _config.begin, SunTransactionReason.Initialization);
		type = _config.type;
		spawnInterval = _config.spawnInterval;
		spawnNum = _config.spawnNum;
		movingMethod = _config.movingMethod;
		timer = spawnInterval - 6.0;
	}

	public Node2D SunCreate()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return null;
		}
		Vector2 mapGridBeginPos = instance.GetMapGridBeginPos();
		double num = GD.RandRange(mapGridBeginPos.X, instance.GetMapGroundRight());
		double height = GD.RandRange(mapGridBeginPos.Y + 200f, instance.GetMapGroundDown() - (double)mapGridBeginPos.Y);
		Vector2 pos = new Vector2((float)num, mapGridBeginPos.Y - 100f);
		Node2D result = null;
		switch (type)
		{
		case "Normal":
			result = instance.SunCreate(LocalAccountId, pos, spawnNum, movingMethod, height, new Vector2(0f, 100f));
			break;
		case "Brain":
			result = instance.BrainSunCreate(LocalAccountId, pos, spawnNum, movingMethod, height, new Vector2(0f, 100f));
			break;
		case "Jala":
			result = instance.JalapenoSunCreate(LocalAccountId, pos, spawnNum, movingMethod, height, new Vector2(0f, 100f));
			break;
		case "QX":
			result = instance.QXSunCreate(LocalAccountId, pos, spawnNum, movingMethod, height, new Vector2(0f, 100f));
			break;
		case "MagicSun":
			result = instance.MagicSunCreate(LocalAccountId, pos, spawnNum, movingMethod, height, new Vector2(0f, 100f));
			break;
		}
		return result;
	}

	public override void Destroy()
	{
		isRunning = false;
		OnSunCollect = null;
		OnSunChange = null;
		OnAccountSunCollect = null;
		OnAccountSunChange = null;
		InvalidateSpendReceipts();
		_accountBalances.Clear();
		_mapFeature = null;
		config = null;
		base.Destroy();
	}

	private bool TryGetActiveSpendReceipt(SunSpendReceipt receipt, out long transactionId)
	{
		transactionId = 0L;
		if (receipt == null || !receipt.IsActive || receipt.LedgerGeneration != _ledgerGeneration || !_activeSpendReceipts.TryGetValue(receipt.TransactionId, out var value) || value != receipt)
		{
			return false;
		}
		transactionId = receipt.TransactionId;
		return true;
	}

	private long NextSpendTransactionId()
	{
		do
		{
			_nextSpendTransactionId = ((_nextSpendTransactionId == 9223372036854775807L) ? 1 : (_nextSpendTransactionId + 1));
		}
		while (_activeSpendReceipts.ContainsKey(_nextSpendTransactionId));
		return _nextSpendTransactionId;
	}

	private void InvalidateSpendReceipts()
	{
		foreach (SunSpendReceipt value in _activeSpendReceipts.Values)
		{
			value.Complete(SunSpendReceiptState.Invalidated);
		}
		_activeSpendReceipts.Clear();
		_nextSpendTransactionId = 0L;
		_ledgerGeneration = ((_ledgerGeneration == 9223372036854775807L) ? 1 : (_ledgerGeneration + 1));
	}

	private void EmitSunCollect(EconomyAccountId accountId, long num)
	{
		OnAccountSunCollect?.Invoke(accountId, num);
		if (accountId == LocalAccountId)
		{
			OnSunCollect?.Invoke(num);
		}
	}

	private void EmitSunChange(EconomyAccountId accountId, long balance, SunTransactionReason reason)
	{
		OnAccountSunChange?.Invoke(accountId, balance, reason);
		if (accountId == LocalAccountId)
		{
			OnSunChange?.Invoke(balance);
		}
	}

	private static long SaturatingAdd(long value, long delta)
	{
		if (delta > 0 && value > 9223372036854775807L - delta)
		{
			return 9223372036854775807L;
		}
		if (delta < 0 && value < -9223372036854775808L - delta)
		{
			return -9223372036854775808L;
		}
		return value + delta;
	}

	private static long SaturatingSubtract(long value, long amount)
	{
		if (amount > 0 && value < -9223372036854775808L + amount)
		{
			return -9223372036854775808L;
		}
		if (amount < 0 && value > 9223372036854775807L + amount)
		{
			return 9223372036854775807L;
		}
		return value - amount;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.EmitSunCollect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitSunChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.AddSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UseSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SunInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SunCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextSpendTransactionId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateSpendReceipts, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaturatingAdd, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaturatingSubtract, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "amount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitSunCollect && args.Count == 1)
		{
			EmitSunCollect(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitSunChange && args.Count == 1)
		{
			EmitSunChange(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
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
		if (method == MethodName.AddSun && args.Count == 1)
		{
			AddSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UseSun && args.Count == 1)
		{
			UseSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSun && args.Count == 1)
		{
			SetSun(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SunInit && args.Count == 1)
		{
			SunInit(VariantUtils.ConvertTo<TowerDefenseLevelSunManagerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SunCreate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(SunCreate());
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.NextSpendTransactionId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(NextSpendTransactionId());
			return true;
		}
		if (method == MethodName.InvalidateSpendReceipts && args.Count == 0)
		{
			InvalidateSpendReceipts();
			ret = default;
			return true;
		}
		if (method == MethodName.SaturatingAdd && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long>(SaturatingAdd(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1])));
			return true;
		}
		if (method == MethodName.SaturatingSubtract && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long>(SaturatingSubtract(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SaturatingAdd && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long>(SaturatingAdd(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1])));
			return true;
		}
		if (method == MethodName.SaturatingSubtract && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<long>(SaturatingSubtract(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<long>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitSunCollect)
		{
			return true;
		}
		if (method == MethodName.EmitSunChange)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.GameFail)
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
		if (method == MethodName.AddSun)
		{
			return true;
		}
		if (method == MethodName.UseSun)
		{
			return true;
		}
		if (method == MethodName.SetSun)
		{
			return true;
		}
		if (method == MethodName.SunInit)
		{
			return true;
		}
		if (method == MethodName.SunCreate)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.NextSpendTransactionId)
		{
			return true;
		}
		if (method == MethodName.InvalidateSpendReceipts)
		{
			return true;
		}
		if (method == MethodName.SaturatingAdd)
		{
			return true;
		}
		if (method == MethodName.SaturatingSubtract)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelSunManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spawnInterval)
		{
			spawnInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnNum)
		{
			spawnNum = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			movingMethod = VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in value);
			return true;
		}
		if (name == PropertyName.isRunning)
		{
			isRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._ledgerGeneration)
		{
			_ledgerGeneration = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._nextSpendTransactionId)
		{
			_nextSpendTransactionId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom<long>(sunNum);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
			return true;
		}
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.spawnInterval)
		{
			value = VariantUtils.CreateFrom(in spawnInterval);
			return true;
		}
		if (name == PropertyName.spawnNum)
		{
			value = VariantUtils.CreateFrom(in spawnNum);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			value = VariantUtils.CreateFrom(in movingMethod);
			return true;
		}
		if (name == PropertyName.isRunning)
		{
			value = VariantUtils.CreateFrom(in isRunning);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName._ledgerGeneration)
		{
			value = VariantUtils.CreateFrom(in _ledgerGeneration);
			return true;
		}
		if (name == PropertyName._nextSpendTransactionId)
		{
			value = VariantUtils.CreateFrom(in _nextSpendTransactionId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.movingMethod, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._ledgerGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextSpendTransactionId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sunNum, Variant.From<long>(sunNum));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.spawnInterval, Variant.From(in spawnInterval));
		info.AddProperty(PropertyName.spawnNum, Variant.From(in spawnNum));
		info.AddProperty(PropertyName.movingMethod, Variant.From(in movingMethod));
		info.AddProperty(PropertyName.isRunning, Variant.From(in isRunning));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName._ledgerGeneration, Variant.From(in _ledgerGeneration));
		info.AddProperty(PropertyName._nextSpendTransactionId, Variant.From(in _nextSpendTransactionId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sunNum, out var value))
		{
			sunNum = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseLevelSunManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value3))
		{
			_mapFeature = value3.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value4))
		{
			type = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spawnInterval, out var value5))
		{
			spawnInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnNum, out var value6))
		{
			spawnNum = value6.As<long>();
		}
		if (info.TryGetProperty(PropertyName.movingMethod, out var value7))
		{
			movingMethod = value7.As<TowerDefenseEnum.SUN_MOVING_METHOD>();
		}
		if (info.TryGetProperty(PropertyName.isRunning, out var value8))
		{
			isRunning = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value9))
		{
			timer = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._ledgerGeneration, out var value10))
		{
			_ledgerGeneration = value10.As<long>();
		}
		if (info.TryGetProperty(PropertyName._nextSpendTransactionId, out var value11))
		{
			_nextSpendTransactionId = value11.As<long>();
		}
	}
}
