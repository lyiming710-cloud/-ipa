using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/DropItem/DropItemRegistry.cs")]
public class DropItemRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName CreateHandlers = "CreateHandlers";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName CreateConfig = "CreateConfig";

		public static readonly StringName Register = "Register";

		public static readonly StringName GetDropItem = "GetDropItem";

		public static readonly StringName GetById = "GetById";

		public static readonly StringName GetByCoinObjectId = "GetByCoinObjectId";

		public static readonly StringName GetByCategory = "GetByCategory";

		public static readonly StringName GetPoolKeyByCoinObjectId = "GetPoolKeyByCoinObjectId";

		public static readonly StringName GetHandler = "GetHandler";

		public static readonly StringName GetHandlerById = "GetHandlerById";

		public static readonly StringName GetLuckyBagHandler = "GetLuckyBagHandler";

		public static readonly StringName GetJalapenoSunHandler = "GetJalapenoSunHandler";

		public static readonly StringName GetQXSunHandler = "GetQXSunHandler";

		public static readonly StringName GetMagicSunHandler = "GetMagicSunHandler";

		public static readonly StringName Reset = "Reset";

		public static readonly StringName GetNames = "GetNames";

		public static readonly StringName GetIds = "GetIds";

		public static readonly StringName GetDropItemLegacyForTests = "GetDropItemLegacyForTests";

		public static readonly StringName GetByIdLegacyForTests = "GetByIdLegacyForTests";

		public static readonly StringName GetByCoinObjectIdLegacyForTests = "GetByCoinObjectIdLegacyForTests";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static bool IsInit = false;

	public static System.Collections.Generic.Dictionary<StringName, DropItemConfig> DropItemDictionary = new System.Collections.Generic.Dictionary<StringName, DropItemConfig>();

	public static System.Collections.Generic.Dictionary<int, DropItemConfig> DropItemByIdDictionary = new System.Collections.Generic.Dictionary<int, DropItemConfig>();

	public static System.Collections.Generic.Dictionary<int, DropItemConfig> DropItemByCoinObjectIdDictionary = new System.Collections.Generic.Dictionary<int, DropItemConfig>();

	public static SunDropItemHandler _SunHandler;

	public static CoinDropItemHandler _CoinHandler;

	public static LuckyBagDropItemHandler _LuckyBagHandler;

	public static JalapenoSunDropItemHandler _JalapenoSunHandler;

	public static BrainSunDropItemHandler _BrainSunHandler;

	public static QXSunDropItemHandler _QXSunHandler;

	public static MagicSunDropItemHandler _MagicSunHandler;

	public static GoldShardDropItemHandler _GoldShardHandler;

	public static void Init()
	{
		if (!IsInit)
		{
			IsInit = true;
			CreateHandlers();
			RegisterInit();
		}
	}

	public static void CreateHandlers()
	{
		_SunHandler = new SunDropItemHandler();
		_CoinHandler = new CoinDropItemHandler();
		_LuckyBagHandler = new LuckyBagDropItemHandler();
		_JalapenoSunHandler = new JalapenoSunDropItemHandler();
		_QXSunHandler = new QXSunDropItemHandler();
		_MagicSunHandler = new MagicSunDropItemHandler();
		_BrainSunHandler = new BrainSunDropItemHandler();
		_GoldShardHandler = new GoldShardDropItemHandler();
	}

	public static void RegisterInit()
	{
		Register(CreateConfig("Sun", ObjectManagerConfig.OBJECT.SUN, GD.Load<PackedScene>("uid://dk3bkihnh1i0l"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN, 25, "", "Sun", -1, _SunHandler));
		Register(CreateConfig("SunBrain", ObjectManagerConfig.OBJECT.SUN_BRAIN, GD.Load<PackedScene>("uid://d161xee5m0kkw"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN, 25, "", "Sun", -1, _BrainSunHandler));
		Register(CreateConfig("SunJalapeno", ObjectManagerConfig.OBJECT.SUN_JALAPENO, GD.Load<PackedScene>("uid://da7lvlco511ds"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN, 25, "", "Sun", -1, _JalapenoSunHandler));
		Register(CreateConfig("SunQX", ObjectManagerConfig.OBJECT.SUN_QX, GD.Load<PackedScene>("uid://boebeodp5s2g2"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN, 25, "", "Sun", -1, _QXSunHandler));
		Register(CreateConfig("SunMagic", ObjectManagerConfig.OBJECT.SUN_MAGIC, GD.Load<PackedScene>("uid://c4r7bn2wqxnat"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN, 25, "", "Sun", -1, _MagicSunHandler));
		Register(CreateConfig("CoinSilver", ObjectManagerConfig.OBJECT.COIN_SILVER, GD.Load<PackedScene>("uid://csynbfevdbiju"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN, 10, "CoinFall", "CoinPick", 0, _CoinHandler));
		Register(CreateConfig("CoinGold", ObjectManagerConfig.OBJECT.COIN_GOLD, GD.Load<PackedScene>("uid://kbif4idtgolo"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN, 50, "CoinFall", "CoinPick", 1, _CoinHandler));
		Register(CreateConfig("CoinDiamond", ObjectManagerConfig.OBJECT.COIN_DIAMOND, GD.Load<PackedScene>("uid://6b78y08u52f5"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN, 1000, "CoinFall", "Diamond", 2, _CoinHandler));
		Register(CreateConfig("CoinTQ", ObjectManagerConfig.OBJECT.COIN_TQ, GD.Load<PackedScene>("uid://733w81lrellb"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN, 10, "CoinFall", "CoinPick", 4, _CoinHandler));
		Register(CreateConfig("CoinYr1", ObjectManagerConfig.OBJECT.COIN_YB1, GD.Load<PackedScene>("uid://c25ngfvpp0uo3"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN, 50, "CoinFall", "CoinPick", 5, _CoinHandler));
		Register(CreateConfig("CoinYr2", ObjectManagerConfig.OBJECT.COIN_YB2, GD.Load<PackedScene>("uid://1twddwaolt4r"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN, 1000, "CoinFall", "CoinPick", 6, _CoinHandler));
		Register(CreateConfig("LuckyBag", ObjectManagerConfig.OBJECT.COIN_LUCKY_BAG, GD.Load<PackedScene>("uid://bnrh1k2cgopsn"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SPECIAL, 0, "CoinFall", "CoinPick", 3, _LuckyBagHandler));
		Register(CreateConfig("GoldShard", ObjectManagerConfig.OBJECT.COIN_GOLD_SHARD, GD.Load<PackedScene>("uid://dsfn18qda4271"), 100, TowerDefenseEnum.DROP_ITEM_CATEGORY.SPECIAL, 0, "CoinFall", "CoinPick", 7, _GoldShardHandler));
	}

	public static DropItemConfig CreateConfig(StringName _name, ObjectManagerConfig.OBJECT _id, PackedScene _scene, int _poolMaxNum, TowerDefenseEnum.DROP_ITEM_CATEGORY _category, int _value, string _fallAudio = "CoinFall", string _pickAudio = "CoinPick", int _coinObjectId = -1, DropItemHandler _handler = null)
	{
		return new DropItemConfig
		{
			Name = _name,
			Id = _id,
			Scene = _scene,
			PoolMaxNum = _poolMaxNum,
			Category = _category,
			Value = _value,
			FallAudio = _fallAudio,
			PickAudio = _pickAudio,
			CoinObjectId = _coinObjectId,
			Handler = _handler
		};
	}

	public static void Register(DropItemConfig config)
	{
		DropItemDictionary[config.Name] = config;
		DropItemByIdDictionary[(int)config.Id] = config;
		if (config.CoinObjectId >= 0)
		{
			DropItemByCoinObjectIdDictionary[config.CoinObjectId] = config;
		}
	}

	public static DropItemConfig GetDropItem(StringName _name)
	{
		Init();
		if (!DropItemDictionary.TryGetValue(_name, out var value))
		{
			return null;
		}
		return value;
	}

	public static DropItemConfig GetById(ObjectManagerConfig.OBJECT id)
	{
		Init();
		if (!DropItemByIdDictionary.TryGetValue((int)id, out var value))
		{
			return null;
		}
		return value;
	}

	public static DropItemConfig GetByCoinObjectId(int coinObjectId)
	{
		Init();
		if (!DropItemByCoinObjectIdDictionary.TryGetValue(coinObjectId, out var value))
		{
			return null;
		}
		return value;
	}

	public static Array<DropItemConfig> GetByCategory(TowerDefenseEnum.DROP_ITEM_CATEGORY category)
	{
		Init();
		Array<DropItemConfig> array = new Array<DropItemConfig>();
		foreach (DropItemConfig value in DropItemDictionary.Values)
		{
			if (value.Category == category)
			{
				array.Add(value);
			}
		}
		return array;
	}

	public static ObjectManagerConfig.OBJECT GetPoolKeyByCoinObjectId(int coinObjectId)
	{
		return GetByCoinObjectId(coinObjectId)?.Id ?? ObjectManagerConfig.OBJECT.NOONE;
	}

	public static DropItemHandler GetHandler(StringName _name)
	{
		DropItemConfig dropItem = GetDropItem(_name);
		if (dropItem != null && dropItem.Handler != null)
		{
			return dropItem.Handler;
		}
		return null;
	}

	public static DropItemHandler GetHandlerById(ObjectManagerConfig.OBJECT id)
	{
		DropItemConfig byId = GetById(id);
		if (byId != null && byId.Handler != null)
		{
			return byId.Handler;
		}
		return null;
	}

	public static LuckyBagDropItemHandler GetLuckyBagHandler()
	{
		Init();
		return _LuckyBagHandler;
	}

	public static JalapenoSunDropItemHandler GetJalapenoSunHandler()
	{
		Init();
		return _JalapenoSunHandler;
	}

	public static QXSunDropItemHandler GetQXSunHandler()
	{
		Init();
		return _QXSunHandler;
	}

	public static MagicSunDropItemHandler GetMagicSunHandler()
	{
		Init();
		return _MagicSunHandler;
	}

	public static void Reset()
	{
		Init();
		foreach (DropItemConfig value in DropItemDictionary.Values)
		{
			if (value.Handler != null)
			{
				value.Handler.Reset();
			}
		}
	}

	public static Array<StringName> GetNames()
	{
		Init();
		Array<StringName> array = new Array<StringName>();
		foreach (StringName key in DropItemDictionary.Keys)
		{
			array.Add(key);
		}
		return array;
	}

	public static Array<int> GetIds()
	{
		Init();
		Array<int> array = new Array<int>();
		foreach (int key in DropItemByIdDictionary.Keys)
		{
			array.Add(key);
		}
		return array;
	}

	internal static DropItemConfig GetDropItemLegacyForTests(StringName name)
	{
		Init();
		if (DropItemDictionary.ContainsKey(name))
		{
			return DropItemDictionary[name];
		}
		return null;
	}

	internal static DropItemConfig GetByIdLegacyForTests(ObjectManagerConfig.OBJECT id)
	{
		Init();
		if (DropItemByIdDictionary.ContainsKey((int)id))
		{
			return DropItemByIdDictionary[(int)id];
		}
		return null;
	}

	internal static DropItemConfig GetByCoinObjectIdLegacyForTests(int coinObjectId)
	{
		Init();
		if (DropItemByCoinObjectIdDictionary.ContainsKey(coinObjectId))
		{
			return DropItemByCoinObjectIdDictionary[coinObjectId];
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateHandlers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "_name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Int, "_poolMaxNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "_fallAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "_pickAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_coinObjectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_handler", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetDropItem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "_name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetById, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetByCoinObjectId, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "coinObjectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetByCategory, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPoolKeyByCoinObjectId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "coinObjectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetHandler, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "_name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetHandlerById, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLuckyBagHandler, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetJalapenoSunHandler, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetQXSunHandler, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetMagicSunHandler, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Reset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetIds, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetDropItemLegacyForTests, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetByIdLegacyForTests, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetByCoinObjectIdLegacyForTests, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "coinObjectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateHandlers && args.Count == 0)
		{
			CreateHandlers();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateConfig && args.Count == 10)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(CreateConfig(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<string>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<DropItemHandler>(in args[9])));
			return true;
		}
		if (method == MethodName.Register && args.Count == 1)
		{
			Register(VariantUtils.ConvertTo<DropItemConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDropItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetDropItem(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetById && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetById(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByCoinObjectId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetByCoinObjectId(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByCategory && args.Count == 1)
		{
			Array<DropItemConfig> byCategory = GetByCategory(VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in args[0]));
			ret = VariantUtils.CreateFromArray(byCategory);
			return true;
		}
		if (method == MethodName.GetPoolKeyByCoinObjectId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(GetPoolKeyByCoinObjectId(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandler && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemHandler>(GetHandler(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandlerById && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemHandler>(GetHandlerById(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLuckyBagHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<LuckyBagDropItemHandler>(GetLuckyBagHandler());
			return true;
		}
		if (method == MethodName.GetJalapenoSunHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<JalapenoSunDropItemHandler>(GetJalapenoSunHandler());
			return true;
		}
		if (method == MethodName.GetQXSunHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<QXSunDropItemHandler>(GetQXSunHandler());
			return true;
		}
		if (method == MethodName.GetMagicSunHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<MagicSunDropItemHandler>(GetMagicSunHandler());
			return true;
		}
		if (method == MethodName.Reset && args.Count == 0)
		{
			Reset();
			ret = default;
			return true;
		}
		if (method == MethodName.GetNames && args.Count == 0)
		{
			Array<StringName> names = GetNames();
			ret = VariantUtils.CreateFromArray(names);
			return true;
		}
		if (method == MethodName.GetIds && args.Count == 0)
		{
			Array<int> ids = GetIds();
			ret = VariantUtils.CreateFromArray(ids);
			return true;
		}
		if (method == MethodName.GetDropItemLegacyForTests && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetDropItemLegacyForTests(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByIdLegacyForTests && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetByIdLegacyForTests(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByCoinObjectIdLegacyForTests && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetByCoinObjectIdLegacyForTests(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateHandlers && args.Count == 0)
		{
			CreateHandlers();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateConfig && args.Count == 10)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(CreateConfig(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<string>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<DropItemHandler>(in args[9])));
			return true;
		}
		if (method == MethodName.Register && args.Count == 1)
		{
			Register(VariantUtils.ConvertTo<DropItemConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDropItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetDropItem(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetById && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetById(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByCoinObjectId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetByCoinObjectId(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByCategory && args.Count == 1)
		{
			Array<DropItemConfig> byCategory = GetByCategory(VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in args[0]));
			ret = VariantUtils.CreateFromArray(byCategory);
			return true;
		}
		if (method == MethodName.GetPoolKeyByCoinObjectId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(GetPoolKeyByCoinObjectId(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandler && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemHandler>(GetHandler(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandlerById && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemHandler>(GetHandlerById(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLuckyBagHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<LuckyBagDropItemHandler>(GetLuckyBagHandler());
			return true;
		}
		if (method == MethodName.GetJalapenoSunHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<JalapenoSunDropItemHandler>(GetJalapenoSunHandler());
			return true;
		}
		if (method == MethodName.GetQXSunHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<QXSunDropItemHandler>(GetQXSunHandler());
			return true;
		}
		if (method == MethodName.GetMagicSunHandler && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<MagicSunDropItemHandler>(GetMagicSunHandler());
			return true;
		}
		if (method == MethodName.Reset && args.Count == 0)
		{
			Reset();
			ret = default;
			return true;
		}
		if (method == MethodName.GetNames && args.Count == 0)
		{
			Array<StringName> names = GetNames();
			ret = VariantUtils.CreateFromArray(names);
			return true;
		}
		if (method == MethodName.GetIds && args.Count == 0)
		{
			Array<int> ids = GetIds();
			ret = VariantUtils.CreateFromArray(ids);
			return true;
		}
		if (method == MethodName.GetDropItemLegacyForTests && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetDropItemLegacyForTests(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByIdLegacyForTests && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetByIdLegacyForTests(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetByCoinObjectIdLegacyForTests && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetByCoinObjectIdLegacyForTests(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.CreateHandlers)
		{
			return true;
		}
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.CreateConfig)
		{
			return true;
		}
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.GetDropItem)
		{
			return true;
		}
		if (method == MethodName.GetById)
		{
			return true;
		}
		if (method == MethodName.GetByCoinObjectId)
		{
			return true;
		}
		if (method == MethodName.GetByCategory)
		{
			return true;
		}
		if (method == MethodName.GetPoolKeyByCoinObjectId)
		{
			return true;
		}
		if (method == MethodName.GetHandler)
		{
			return true;
		}
		if (method == MethodName.GetHandlerById)
		{
			return true;
		}
		if (method == MethodName.GetLuckyBagHandler)
		{
			return true;
		}
		if (method == MethodName.GetJalapenoSunHandler)
		{
			return true;
		}
		if (method == MethodName.GetQXSunHandler)
		{
			return true;
		}
		if (method == MethodName.GetMagicSunHandler)
		{
			return true;
		}
		if (method == MethodName.Reset)
		{
			return true;
		}
		if (method == MethodName.GetNames)
		{
			return true;
		}
		if (method == MethodName.GetIds)
		{
			return true;
		}
		if (method == MethodName.GetDropItemLegacyForTests)
		{
			return true;
		}
		if (method == MethodName.GetByIdLegacyForTests)
		{
			return true;
		}
		if (method == MethodName.GetByCoinObjectIdLegacyForTests)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
