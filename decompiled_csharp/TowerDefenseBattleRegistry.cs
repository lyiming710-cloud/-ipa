using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/TowerDefenseBattleRegistry.cs")]
public class TowerDefenseBattleRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName RegisterDependence = "RegisterDependence";

		public static readonly StringName RegisterFeature = "RegisterFeature";

		public static readonly StringName RegisterProcess = "RegisterProcess";

		public static readonly StringName SetFeaturePrototype = "SetFeaturePrototype";

		public static readonly StringName SetProcessPrototype = "SetProcessPrototype";

		public static readonly StringName RemoveFeatureRegistration = "RemoveFeatureRegistration";

		public static readonly StringName RemoveProcessRegistration = "RemoveProcessRegistration";

		public static readonly StringName SetFeatureDependence = "SetFeatureDependence";

		public static readonly StringName SetProcessDependence = "SetProcessDependence";

		public static readonly StringName GetFeature = "GetFeature";

		public static readonly StringName GetProcess = "GetProcess";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static bool IsInit = false;

	public static System.Collections.Generic.Dictionary<StringName, TowerDefenseBattleFeature> BattleFeatureDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseBattleFeature>();

	public static System.Collections.Generic.Dictionary<StringName, TowerDefenseBattleProcess> BattleProcessDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseBattleProcess>();

	private static readonly System.Collections.Generic.Dictionary<StringName, Func<TowerDefenseBattleFeature>> BattleFeatureFactoryDictionary = new System.Collections.Generic.Dictionary<StringName, Func<TowerDefenseBattleFeature>>();

	private static readonly System.Collections.Generic.Dictionary<StringName, Func<TowerDefenseBattleProcess>> BattleProcessFactoryDictionary = new System.Collections.Generic.Dictionary<StringName, Func<TowerDefenseBattleProcess>>();

	public static void Init()
	{
		if (!IsInit)
		{
			IsInit = true;
			RegisterInit();
		}
	}

	public static void RegisterInit()
	{
		RegisterFeature<TowerDefenseBattleFeatureCamera>("Camera");
		RegisterFeature<TowerDefenseBattleFeatureMap>("Map");
		RegisterFeature<TowerDefenseBattleFeatureShovel>("Shovel");
		RegisterFeature<TowerDefenseBattleFeatureGlove>("Glove");
		RegisterFeature<TowerDefenseBattleFeaturePacketPick>("PacketPick");
		RegisterFeature<TowerDefenseBattleFeatureMower>("Mower");
		RegisterFeature<TowerDefenseBattleFeatureBrain>("Brain");
		RegisterFeature<TowerDefenseBattleFeatureNpcTalk>("NpcTalk");
		RegisterFeature<TowerDefenseBattleFeatureSun>("Sun");
		RegisterFeature<TowerDefenseBattleFeatureProgress>("Progress");
		RegisterFeature<TowerDefenseBattleFeatureRainMode>("RainMode");
		RegisterFeature<TowerDefenseBattleFeatureConveyorBelt>("ConveyorBelt");
		RegisterFeature<TowerDefenseBattleFeaturePacketBank>("PacketBank");
		RegisterFeature<TowerDefenseBattleFeatureSeedBank>("SeedBank");
		RegisterFeature<TowerDefenseBattleFeaturePreSpawn>("PreSpawn");
		RegisterFeature<TowerDefenseBattleFeatureBGM>("BGM");
		RegisterFeature<TowerDefenseBattleFeatureFog>("Fog");
		RegisterFeature<TowerDefenseBattleFeatureLookStar>("LookStar");
		RegisterFeature<TowerDefenseBattleFeatureScreenEffect>("ScreenEffect");
		RegisterFeature<TowerDefenseBattleFeatureWarningLine>("WarningLine");
		RegisterFeature<TowerDefenseBattleFeaturePortal>("Portal");
		RegisterFeature<TowerDefenseBattleFeatureTutorial>("Tutorial", 200);
		RegisterFeature<TowerDefenseBattleFeatureEvent>("Event");
		RegisterFeature<TowerDefenseBattleFeatureHammer>("Hammer");
		RegisterFeature<TowerDefenseBattleFeatureGemMatch>("GemMatch");
		RegisterFeature<TowerDefenseBattleFeatureSlotMachine>("SlotMachine");
		RegisterFeature<TowerDefenseBattleFeatureWave>("Wave");
		RegisterProcess<TowerDefenseBattleProcessWave>("Wave");
		RegisterProcess<TowerDefenseBattleProcessVase>("Vase");
		RegisterProcess<TowerDefenseBattleProcessIZM>("IZM");
		RegisterProcess<TowerDefenseBattleProcessIZM2>("IZM2");
		RegisterProcess<TowerDefenseBattleProcessQuiz>("Quiz");
		RegisterProcess<TowerDefenseBattleProcessEmpty>("Empty");
		RegisterDependence();
	}

	public static void RegisterDependence()
	{
		SetFeatureDependence("Camera", new Array<StringName> { "Map" });
		SetFeatureDependence("Map", new Array<StringName>());
		SetFeatureDependence("Shovel", new Array<StringName> { "Map", "PacketPick" });
		SetFeatureDependence("Glove", new Array<StringName> { "Map", "PacketPick" });
		SetFeatureDependence("PacketPick", new Array<StringName> { "Map" });
		SetFeatureDependence("Mower", new Array<StringName> { "Map" });
		SetFeatureDependence("Brain", new Array<StringName> { "Map" }, new Array<StringName> { "Progress" });
		SetFeatureDependence("NpcTalk", new Array<StringName>(), new Array<StringName> { "BGM" });
		SetFeatureDependence("Sun", new Array<StringName> { "Map" });
		SetFeatureDependence("Progress", new Array<StringName>());
		SetFeatureDependence("RainMode", new Array<StringName> { "Map", "PacketPick" }, new Array<StringName> { "Sun" });
		SetFeatureDependence("ConveyorBelt", new Array<StringName> { "PacketPick" }, new Array<StringName> { "Wave" });
		SetFeatureDependence("PacketBank", new Array<StringName> { "SeedBank" });
		SetFeatureDependence("SeedBank", new Array<StringName> { "PacketPick" });
		SetFeatureDependence("PreSpawn", new Array<StringName> { "Map" });
		SetFeatureDependence("BGM", new Array<StringName>());
		SetFeatureDependence("Fog", new Array<StringName> { "Map" });
		SetFeatureDependence("LookStar", new Array<StringName> { "Map" });
		SetFeatureDependence("ScreenEffect", new Array<StringName>());
		SetFeatureDependence("WarningLine", new Array<StringName> { "Map" });
		SetFeatureDependence("Portal", new Array<StringName> { "Map" });
		SetFeatureDependence("Tutorial", new Array<StringName>());
		SetFeatureDependence("Event", new Array<StringName> { "Map" });
		SetFeatureDependence("Hammer", new Array<StringName>());
		SetFeatureDependence("GemMatch", new Array<StringName> { "Map", "Sun", "SeedBank" });
		SetFeatureDependence("SlotMachine", new Array<StringName> { "Sun" });
		SetFeatureDependence("Wave", new Array<StringName> { "Map", "Progress" }, new Array<StringName> { "LookStar", "GemMatch", "PacketPick" });
		SetProcessDependence("Wave", new Array<StringName> { "Map", "Progress", "Camera", "Wave" }, new Array<StringName> { "Mower", "Sun", "SeedBank", "PacketBank", "SlotMachine" });
		SetProcessDependence("Vase", new Array<StringName> { "Map" }, new Array<StringName> { "Mower", "Progress", "SeedBank", "PacketBank" });
		SetProcessDependence("IZM", new Array<StringName> { "Map", "Brain", "Progress", "Sun" }, new Array<StringName> { "SeedBank", "PacketBank", "ConveyorBelt" });
		SetProcessDependence("IZM2", new Array<StringName> { "Mower", "Progress", "Camera", "Wave" }, new Array<StringName> { "SeedBank", "PacketBank" });
		SetProcessDependence("Quiz", new Array<StringName> { "Map", "Brain", "Progress" });
		SetProcessDependence("Empty", new Array<StringName>(), new Array<StringName> { "SeedBank" });
	}

	public static void RegisterFeature<T>(StringName featureName, int gameStartPriority = 0) where T : TowerDefenseBattleFeature, new()
	{
		BattleFeatureDictionary[featureName] = new T
		{
			gameStartPriority = gameStartPriority
		};
		BattleFeatureFactoryDictionary[featureName] = () => new T();
	}

	public static void RegisterProcess<T>(StringName processName, int gameStartPriority = 100) where T : TowerDefenseBattleProcess, new()
	{
		BattleProcessDictionary[processName] = new T
		{
			gameStartPriority = gameStartPriority
		};
		BattleProcessFactoryDictionary[processName] = () => new T();
	}

	public static void RegisterFeature(StringName featureName, TowerDefenseBattleFeature feature, int gameStartPriority = 0)
	{
		feature.gameStartPriority = gameStartPriority;
		SetFeaturePrototype(featureName, feature);
	}

	public static void RegisterProcess(StringName processName, TowerDefenseBattleProcess process, int gameStartPriority = 100)
	{
		process.gameStartPriority = gameStartPriority;
		SetProcessPrototype(processName, process);
	}

	internal static void SetFeaturePrototype(StringName key, TowerDefenseBattleFeature prototype)
	{
		BattleFeatureDictionary[key] = prototype;
		BattleFeatureFactoryDictionary.Remove(key);
	}

	internal static void SetProcessPrototype(StringName key, TowerDefenseBattleProcess prototype)
	{
		BattleProcessDictionary[key] = prototype;
		BattleProcessFactoryDictionary.Remove(key);
	}

	internal static bool RemoveFeatureRegistration(StringName key)
	{
		BattleFeatureFactoryDictionary.Remove(key);
		return BattleFeatureDictionary.Remove(key);
	}

	internal static bool RemoveProcessRegistration(StringName key)
	{
		BattleProcessFactoryDictionary.Remove(key);
		return BattleProcessDictionary.Remove(key);
	}

	internal static Action CaptureFeatureRegistration(StringName key)
	{
		return CaptureRegistration(key, BattleFeatureDictionary, BattleFeatureFactoryDictionary);
	}

	internal static Action CaptureProcessRegistration(StringName key)
	{
		return CaptureRegistration(key, BattleProcessDictionary, BattleProcessFactoryDictionary);
	}

	private static Action CaptureRegistration<T>(StringName key, System.Collections.Generic.Dictionary<StringName, T> prototypes, System.Collections.Generic.Dictionary<StringName, Func<T>> factories)
	{
		bool hadPrototype = prototypes.TryGetValue(key, out var prototype);
		bool hadFactory = factories.TryGetValue(key, out var factory);
		return () =>
		{
			if (hadPrototype)
			{
				prototypes[key] = prototype;
			}
			else
			{
				prototypes.Remove(key);
			}
			if (hadFactory)
			{
				factories[key] = factory;
			}
			else
			{
				factories.Remove(key);
			}
		};
	}

	public static void SetFeatureDependence(StringName featureName, Array<StringName> _featureNames, Array<StringName> _optionalFeatureNames = null)
	{
		if (BattleFeatureDictionary.ContainsKey(featureName))
		{
			TowerDefenseBattleDependenceData towerDefenseBattleDependenceData = new TowerDefenseBattleDependenceData();
			towerDefenseBattleDependenceData.FeatureNames = _featureNames ?? new Array<StringName>();
			towerDefenseBattleDependenceData.ConfiguredOptionalFeatureNames = _optionalFeatureNames ?? new Array<StringName>();
			BattleFeatureDictionary[featureName].dependenceData = towerDefenseBattleDependenceData;
		}
	}

	public static void SetProcessDependence(StringName processName, Array<StringName> _featureNames, Array<StringName> _optionalFeatureNames = null)
	{
		if (BattleProcessDictionary.ContainsKey(processName))
		{
			TowerDefenseBattleDependenceData towerDefenseBattleDependenceData = new TowerDefenseBattleDependenceData();
			towerDefenseBattleDependenceData.FeatureNames = _featureNames ?? new Array<StringName>();
			towerDefenseBattleDependenceData.ConfiguredOptionalFeatureNames = _optionalFeatureNames ?? new Array<StringName>();
			BattleProcessDictionary[processName].dependenceData = towerDefenseBattleDependenceData;
		}
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "Mod 动态注册的类型走 Activator 回退路径，内置类型使用工厂字典无反射。")]
	public static TowerDefenseBattleFeature GetFeature(StringName featureName)
	{
		if (!BattleFeatureDictionary.ContainsKey(featureName))
		{
			GD.PushError($"[GetFeature] 未注册的 feature: {featureName}");
			return null;
		}
		TowerDefenseBattleFeature towerDefenseBattleFeature = BattleFeatureDictionary[featureName];
		try
		{
			TowerDefenseBattleFeature towerDefenseBattleFeature2 = ((!BattleFeatureFactoryDictionary.TryGetValue(featureName, out var value)) ? ((TowerDefenseBattleFeature)CreateInstanceDynamically(towerDefenseBattleFeature.GetType())) : value());
			towerDefenseBattleFeature2.dependenceData = towerDefenseBattleFeature.dependenceData;
			towerDefenseBattleFeature2.gameStartPriority = towerDefenseBattleFeature.gameStartPriority;
			return towerDefenseBattleFeature2;
		}
		catch (Exception value2)
		{
			GD.PushError($"[GetFeature] 创建实例失败: {featureName}, 类型: {towerDefenseBattleFeature.GetType().Name}, 错误: {value2}");
			return null;
		}
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "Mod 动态注册的类型走 Activator 回退路径，内置类型使用工厂字典无反射。")]
	public static TowerDefenseBattleProcess GetProcess(StringName processName)
	{
		if (processName == null || processName.IsEmpty)
		{
			GD.PushError("[GetProcess] Process name is empty.");
			return null;
		}
		if (!BattleProcessDictionary.ContainsKey(processName))
		{
			GD.PushError($"[GetProcess] 未注册的 process: {processName}");
			return null;
		}
		TowerDefenseBattleProcess towerDefenseBattleProcess = BattleProcessDictionary[processName];
		try
		{
			TowerDefenseBattleProcess towerDefenseBattleProcess2 = ((!BattleProcessFactoryDictionary.TryGetValue(processName, out var value)) ? ((TowerDefenseBattleProcess)CreateInstanceDynamically(towerDefenseBattleProcess.GetType())) : value());
			towerDefenseBattleProcess2.dependenceData = towerDefenseBattleProcess.dependenceData;
			towerDefenseBattleProcess2.gameStartPriority = towerDefenseBattleProcess.gameStartPriority;
			return towerDefenseBattleProcess2;
		}
		catch (Exception value2)
		{
			GD.PushError($"[GetProcess] 创建实例失败: {processName}, 类型: {towerDefenseBattleProcess.GetType().Name}, 错误: {value2}");
			return null;
		}
	}

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "Mod 动态注册的类型走 Activator 回退路径，内置类型使用工厂字典无反射。")]
	private static object CreateInstanceDynamically([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type)
	{
		return Activator.CreateInstance(type);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterDependence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "feature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "gameStartPriority", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "processName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "process", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "gameStartPriority", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFeaturePrototype, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "prototype", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetProcessPrototype, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "prototype", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFeatureRegistration, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcessRegistration, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFeatureDependence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_featureNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_optionalFeatureNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProcessDependence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "processName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_featureNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_optionalFeatureNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProcess, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "processName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterDependence && args.Count == 0)
		{
			RegisterDependence();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterFeature && args.Count == 3)
		{
			RegisterFeature(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProcess && args.Count == 3)
		{
			RegisterProcess(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFeaturePrototype && args.Count == 2)
		{
			SetFeaturePrototype(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProcessPrototype && args.Count == 2)
		{
			SetProcessPrototype(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFeatureRegistration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveFeatureRegistration(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveProcessRegistration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveProcessRegistration(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFeatureDependence && args.Count == 3)
		{
			SetFeatureDependence(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertToArray<StringName>(in args[1]), VariantUtils.ConvertToArray<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProcessDependence && args.Count == 3)
		{
			SetProcessDependence(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertToArray<StringName>(in args[1]), VariantUtils.ConvertToArray<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeature>(GetFeature(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProcess && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleProcess>(GetProcess(VariantUtils.ConvertTo<StringName>(in args[0])));
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
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterDependence && args.Count == 0)
		{
			RegisterDependence();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterFeature && args.Count == 3)
		{
			RegisterFeature(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterProcess && args.Count == 3)
		{
			RegisterProcess(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFeaturePrototype && args.Count == 2)
		{
			SetFeaturePrototype(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProcessPrototype && args.Count == 2)
		{
			SetProcessPrototype(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFeatureRegistration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveFeatureRegistration(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveProcessRegistration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveProcessRegistration(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFeatureDependence && args.Count == 3)
		{
			SetFeatureDependence(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertToArray<StringName>(in args[1]), VariantUtils.ConvertToArray<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProcessDependence && args.Count == 3)
		{
			SetProcessDependence(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertToArray<StringName>(in args[1]), VariantUtils.ConvertToArray<StringName>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeature>(GetFeature(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProcess && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleProcess>(GetProcess(VariantUtils.ConvertTo<StringName>(in args[0])));
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
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.RegisterDependence)
		{
			return true;
		}
		if (method == MethodName.RegisterFeature)
		{
			return true;
		}
		if (method == MethodName.RegisterProcess)
		{
			return true;
		}
		if (method == MethodName.SetFeaturePrototype)
		{
			return true;
		}
		if (method == MethodName.SetProcessPrototype)
		{
			return true;
		}
		if (method == MethodName.RemoveFeatureRegistration)
		{
			return true;
		}
		if (method == MethodName.RemoveProcessRegistration)
		{
			return true;
		}
		if (method == MethodName.SetFeatureDependence)
		{
			return true;
		}
		if (method == MethodName.SetProcessDependence)
		{
			return true;
		}
		if (method == MethodName.GetFeature)
		{
			return true;
		}
		if (method == MethodName.GetProcess)
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
