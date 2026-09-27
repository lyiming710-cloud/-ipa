using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BattleFeatureFullTimingRuntimeTest.cs")]
public class BattleFeatureFullTimingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildPhaseExpectation = "BuildPhaseExpectation";

		public static readonly StringName BuildProgressPhaseExpectation = "BuildProgressPhaseExpectation";

		public static readonly StringName SetupControl = "SetupControl";

		public static readonly StringName CreateProbeLevel = "CreateProbeLevel";

		public static readonly StringName CreateTutorialFirstLevel = "CreateTutorialFirstLevel";

		public static readonly StringName CreateProbeNewLevel = "CreateProbeNewLevel";

		public static readonly StringName ProbeData = "ProbeData";

		public static readonly StringName RegisterProbeGraph = "RegisterProbeGraph";

		public static readonly StringName UnregisterProbeGraph = "UnregisterProbeGraph";

		public static readonly StringName FreeDetachedUi = "FreeDetachedUi";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _pauseButton = "_pauseButton";

		public static readonly StringName _optionButton = "_optionButton";

		public static readonly StringName _speedCheckBox = "_speedCheckBox";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static readonly StringName RequiredName = "TimingProbeRequired";

	private static readonly StringName OptionalName = "TimingProbeOptional";

	private static readonly StringName OwnerName = "TimingProbeOwner";

	private static readonly StringName SiblingName = "TimingProbeSibling";

	private static readonly StringName TutorialName = "TimingProbeTutorial";

	private static readonly StringName ProcessName = "TimingProbeProcess";

	private static readonly string[] FeatureOrder = new string[4] { "Required", "Optional", "Owner", "Sibling" };

	private int _checks;

	private int _failures;

	private BattleFeatureFullTimingRuntimeControlStub _control;

	private MainButton _pauseButton;

	private SpriteBrightButton _optionButton;

	private CheckBox _speedCheckBox;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		TowerDefenseLevelBaseConfig previousLevel = manager?.currentLevelConfig;
		TowerDefenseLevelConfig level = null;
		TowerDefenseZombie probeZombie = null;
		TowerDefenseCharacterInstance probeInstance = null;
		try
		{
			_ = 7;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00a3;
				}
				RegisterProbeGraph();
				await VerifyTutorialStartsLast(manager);
				await VerifyNewLevelInitialization(manager);
				BattleFeatureFullTimingProbeLog.Reset();
				level = CreateProbeLevel();
				SetupControl(manager, level);
				bool condition = _control.OldLevelInit(level);
				Check(condition, "The production OldLevelInit path must build the probe graph.");
				Check(_control.featureDictionary.Keys.Select((StringName key) => key.ToString()).SequenceEqual(new string[4]
				{
					RequiredName.ToString(),
					OptionalName.ToString(),
					OwnerName.ToString(),
					SiblingName.ToString()
				}), "Required and configured-optional dependencies must initialize before their owner.");
				Check(_control.process is BattleFeatureFullTimingProbeProcess, "The production SetProcess path must install the probe Process after its Feature dependencies.");
				ExpectExact("Init/OnReady", 0, new string[10] { "Init:Required", "Init:Optional", "Init:Owner", "Init:Sibling", "Init:Process", "OnReady:Required", "OnReady:Optional", "OnReady:Owner", "OnReady:Sibling", "OnReady:Process" });
				await VerifyAsyncPhase("GameInit", _control.GameInitEntered);
				await VerifyProgressAwarePhase("GameInit", "GameInitFromProgress", (TowerDefenseBattleComponentBase component) => component.GameInit(), (TowerDefenseBattleComponentBase component) => component.GameInitFromProgress());
				await VerifyAsyncPhase("GameEntry", _control.GameEntryEntered);
				await VerifyAsyncPhase("GameReady", _control.GameReadyEntered);
				await VerifyAsyncPhase("GameStart", _control.GameRunningEntered);
				await VerifyProgressAwarePhase("GameStart", "GameStartFromProgress", (TowerDefenseBattleComponentBase component) => component.GameStart(), (TowerDefenseBattleComponentBase component) => component.GameStartFromProgress());
				Check(_control.isGameRunning && !_control.isInit, "GameRunning must publish its state before Feature GameStart dispatch.");
				int count = BattleFeatureFullTimingProbeLog.Entries.Count;
				_control.GameRunningProcessing(1.0 / 60.0);
				ExpectExact("per-frame", count, new string[5] { "Process:Required", "Process:Optional", "Process:Owner", "Process:Sibling", "PhysicsProcess:Process" });
				int count2 = BattleFeatureFullTimingProbeLog.Entries.Count;
				_control.GameFailEntered();
				ExpectExact("GameFail", count2, new string[5] { "GameFail:Required", "GameFail:Optional", "GameFail:Owner", "GameFail:Sibling", "GameFail:Process" });
				probeInstance = new TowerDefenseCharacterInstance();
				probeZombie = new TowerDefenseZombie
				{
					instance = probeInstance
				};
				int count3 = BattleFeatureFullTimingProbeLog.Entries.Count;
				System.Reflection.MethodInfo method = typeof(TowerDefenseControlNew).GetMethod("HandleZombieEnterHouse", BindingFlags.Instance | BindingFlags.NonPublic);
				Check(method != null, "The runtime fixture must reach the production ZombieEnterHouse dispatcher.");
				method?.Invoke(_control, new object[1] { probeZombie });
				ExpectExact("ZombieEnterHouse", count3, new string[5] { "ZombieEnterHouse:Required", "ZombieEnterHouse:Optional", "ZombieEnterHouse:Owner", "ZombieEnterHouse:Sibling", "ZombieEnterHouse:Process" });
				Check(_control.featureDictionary.Values.All((TowerDefenseBattleFeature feature) => feature.SaveFeature().Count == 0 && feature.SyncSerialize().Count == 0) && _control.process.SaveProcess().Count == 0 && _control.process.SyncSerialize().Count == 0, "Stateless timing probes must not invent progress or network state.");
				int count4 = BattleFeatureFullTimingProbeLog.Entries.Count;
				_control.Free();
				_control = null;
				ExpectExact("Destroy", count4, new string[5] { "Destroy:Process", "Destroy:Sibling", "Destroy:Owner", "Destroy:Optional", "Destroy:Required" });
				goto end_IL_0078;
				end_IL_00a3:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BattleFeatureFullTimingRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0078;
			}
			return;
			end_IL_0078:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.Free();
			}
			_control = null;
			if (GodotObject.IsInstanceValid(probeZombie))
			{
				probeZombie.Free();
			}
			probeInstance?.Dispose();
			FreeDetachedUi();
			UnregisterProbeGraph();
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.currentLevelConfig = previousLevel;
			}
			level?.Dispose();
			await WaitFrames(3);
		}
		bool flag = _failures == 0;
		GD.Print($"BATTLE_FEATURE_FULL_TIMING_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyAsyncPhase(string phase, Action dispatch)
	{
		int start = BattleFeatureFullTimingProbeLog.Entries.Count;
		dispatch();
		Check(await WaitUntil(() => BattleFeatureFullTimingProbeLog.Entries.Skip(start).Contains(phase + ".end:Process"), 120), phase + " must reach the Process after all Feature callbacks.");
		ExpectExact(phase, start, BuildPhaseExpectation(phase));
	}

	private async Task VerifyProgressAwarePhase(string freshPhase, string progressPhase, Func<TowerDefenseBattleComponentBase, Task> fresh, Func<TowerDefenseBattleComponentBase, Task> fromProgress)
	{
		int start = BattleFeatureFullTimingProbeLog.Entries.Count;
		System.Reflection.MethodInfo method = typeof(TowerDefenseControlNew).GetMethod("InvokeLifecycleWithProgress", BindingFlags.Instance | BindingFlags.NonPublic);
		Check(method != null, "The runtime fixture must reach the production progress-aware lifecycle dispatcher.");
		if (!(method == null))
		{
			Task task = method.Invoke(_control, new object[2] { fresh, fromProgress }) as Task;
			Check(task != null, "The production progress-aware lifecycle dispatcher must return an awaitable Task.");
			if (task != null)
			{
				await task;
				ExpectExact(progressPhase, start, BuildProgressPhaseExpectation(freshPhase, progressPhase));
			}
		}
	}

	private static string[] BuildPhaseExpectation(string phase)
	{
		List<string> list = new List<string>();
		foreach (string item in FeatureOrder.Append("Process"))
		{
			list.Add(phase + ".begin:" + item);
			list.Add(phase + ".end:" + item);
		}
		return list.ToArray();
	}

	private static string[] BuildProgressPhaseExpectation(string freshPhase, string progressPhase)
	{
		List<string> list = new List<string>();
		foreach (string item in FeatureOrder.Append("Process"))
		{
			string text = ((item == "Optional") ? freshPhase : progressPhase);
			list.Add(text + ".begin:" + item);
			list.Add(text + ".end:" + item);
		}
		return list.ToArray();
	}

	private void ExpectExact(string label, int start, IReadOnlyList<string> expected)
	{
		string[] array = BattleFeatureFullTimingProbeLog.Entries.Skip(start).Take(expected.Count).ToArray();
		Check(array.SequenceEqual(expected), $"{label} order mismatch. expected=[{string.Join(", ", expected)}] actual=[{string.Join(", ", array)}]");
	}

	private async Task VerifyNewLevelInitialization(TowerDefenseManager manager)
	{
		TowerDefenseLevelNewConfig towerDefenseLevelNewConfig = CreateProbeNewLevel();
		BattleFeatureFullTimingProbeLog.Reset();
		try
		{
			SetupControl(manager, towerDefenseLevelNewConfig);
			bool condition = _control.NewLevelInit(towerDefenseLevelNewConfig);
			Check(condition, "The production NewLevelInit path must build the probe graph.");
			Check(_control.featureDictionary.Keys.Select((StringName key) => key.ToString()).SequenceEqual(new string[4]
			{
				RequiredName.ToString(),
				OptionalName.ToString(),
				OwnerName.ToString(),
				SiblingName.ToString()
			}), "NewLevelInit must use the same dependency-first Feature order as OldLevelInit.");
			ExpectExact("NewLevel Init/OnReady", 0, new string[10] { "Init:Required", "Init:Optional", "Init:Owner", "Init:Sibling", "Init:Process", "OnReady:Required", "OnReady:Optional", "OnReady:Owner", "OnReady:Sibling", "OnReady:Process" });
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.Free();
			}
			_control = null;
			FreeDetachedUi();
			towerDefenseLevelNewConfig.Dispose();
			await WaitFrames(1);
		}
	}

	private async Task VerifyTutorialStartsLast(TowerDefenseManager manager)
	{
		TowerDefenseLevelConfig level = CreateTutorialFirstLevel();
		BattleFeatureFullTimingProbeLog.Reset();
		try
		{
			SetupControl(manager, level);
			bool condition = _control.OldLevelInit(level);
			Check(condition, "The tutorial-order probe graph must initialize.");
			Check(_control.featureDictionary.Keys.FirstOrDefault() == TutorialName, "The fixture must place Tutorial before the other configured Features.");
			BattleFeatureFullTimingProbeLog.Reset();
			_control.GameRunningEntered();
			Check(await WaitUntil(() => BattleFeatureFullTimingProbeLog.Entries.Contains("GameStart.end:Tutorial"), 120), "Tutorial content must start last without blocking the shared GameStart lifecycle.");
			ExpectExact("Tutorial-last GameStart", 0, new string[12]
			{
				"GameStart.begin:Sibling", "GameStart.end:Sibling", "GameStart.begin:Required", "GameStart.end:Required", "GameStart.begin:Optional", "GameStart.end:Optional", "GameStart.begin:Owner", "GameStart.end:Owner", "GameStart.begin:Process", "GameStart.end:Process",
				"GameStart.begin:Tutorial", "GameStart.end:Tutorial"
			});
			Check(TutorialManager.Instance.currentTutoroal == (_control.GetFeature(TutorialName) as BattleFeatureFullTimingProbeTutorialFeature)?.config, "The pending tutorial fixture must still be active after GameStart returns.");
			int count = BattleFeatureFullTimingProbeLog.Entries.Count;
			_control.GameRunningProcessing(1.0 / 60.0);
			ExpectExact("Tutorial-active per-frame", count, new string[5] { "Process:Sibling", "Process:Required", "Process:Optional", "Process:Owner", "PhysicsProcess:Process" });
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.Free();
			}
			_control = null;
			FreeDetachedUi();
			level.Dispose();
			await WaitFrames(1);
		}
	}

	private void SetupControl(TowerDefenseManager manager, TowerDefenseLevelBaseConfig level)
	{
		_pauseButton = new MainButton();
		_optionButton = new SpriteBrightButton();
		_speedCheckBox = new CheckBox();
		_control = new BattleFeatureFullTimingRuntimeControlStub
		{
			Name = "BattleFeatureFullTimingRuntimeControl",
			levelConfig = level,
			isInit = true,
			isGameRunning = false,
			buttonPause = _pauseButton,
			optionButton = _optionButton,
			checkBox2X = _speedCheckBox
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		manager.currentControl = _control;
		manager.currentLevelConfig = level;
	}

	private static TowerDefenseLevelConfig CreateProbeLevel()
	{
		return new TowerDefenseLevelConfig
		{
			data = null,
			name = "BattleFeatureFullTimingProbe",
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>
			{
				[OwnerName] = ProbeData("Owner"),
				[RequiredName] = ProbeData("Required"),
				[OptionalName] = ProbeData("Optional", canLoadProgress: false),
				[SiblingName] = ProbeData("Sibling")
			},
			processName = ProcessName,
			processData = ProbeData("Process")
		};
	}

	private static TowerDefenseLevelConfig CreateTutorialFirstLevel()
	{
		return new TowerDefenseLevelConfig
		{
			data = null,
			name = "BattleFeatureTutorialLastProbe",
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>
			{
				[TutorialName] = ProbeData("Tutorial"),
				[SiblingName] = ProbeData("Sibling"),
				[OwnerName] = ProbeData("Owner"),
				[RequiredName] = ProbeData("Required"),
				[OptionalName] = ProbeData("Optional", canLoadProgress: false)
			},
			processName = ProcessName,
			processData = ProbeData("Process")
		};
	}

	private static TowerDefenseLevelNewConfig CreateProbeNewLevel()
	{
		return new TowerDefenseLevelNewConfig
		{
			data = null,
			name = "BattleFeatureFullTimingNewProbe",
			featureData = new Godot.Collections.Dictionary<StringName, Dictionary>
			{
				[OwnerName] = ProbeData("Owner"),
				[RequiredName] = ProbeData("Required"),
				[OptionalName] = ProbeData("Optional", canLoadProgress: false),
				[SiblingName] = ProbeData("Sibling")
			},
			processName = ProcessName,
			processData = ProbeData("Process")
		};
	}

	private static Dictionary ProbeData(string id, bool canLoadProgress = true)
	{
		return new Dictionary
		{
			["Id"] = id,
			["CanLoadProgress"] = canLoadProgress
		};
	}

	private static void RegisterProbeGraph()
	{
		TowerDefenseBattleRegistry.RegisterFeature(RequiredName, new BattleFeatureFullTimingProbeFeature());
		TowerDefenseBattleRegistry.RegisterFeature(OptionalName, new BattleFeatureFullTimingProbeFeature());
		TowerDefenseBattleRegistry.RegisterFeature(OwnerName, new BattleFeatureFullTimingProbeFeature());
		TowerDefenseBattleRegistry.RegisterFeature(SiblingName, new BattleFeatureFullTimingProbeFeature());
		TowerDefenseBattleRegistry.RegisterFeature(TutorialName, new BattleFeatureFullTimingProbeTutorialFeature(), 200);
		TowerDefenseBattleRegistry.SetFeatureDependence(RequiredName, new Array<StringName>());
		TowerDefenseBattleRegistry.SetFeatureDependence(OptionalName, new Array<StringName>());
		TowerDefenseBattleRegistry.SetFeatureDependence(OwnerName, new Array<StringName> { RequiredName }, new Array<StringName> { OptionalName });
		TowerDefenseBattleRegistry.SetFeatureDependence(SiblingName, new Array<StringName>());
		TowerDefenseBattleRegistry.SetFeatureDependence(TutorialName, new Array<StringName>());
		TowerDefenseBattleRegistry.RegisterProcess(ProcessName, new BattleFeatureFullTimingProbeProcess());
		TowerDefenseBattleRegistry.SetProcessDependence(ProcessName, new Array<StringName> { OwnerName });
	}

	private static void UnregisterProbeGraph()
	{
		StringName[] array = new StringName[5] { RequiredName, OptionalName, OwnerName, SiblingName, TutorialName };
		foreach (StringName key in array)
		{
			if (TowerDefenseBattleRegistry.BattleFeatureDictionary.Remove(key, out var value))
			{
				TowerDefenseBattleDependenceData dependenceData = value.dependenceData;
				value.Dispose();
				dependenceData?.Dispose();
			}
		}
		if (TowerDefenseBattleRegistry.BattleProcessDictionary.Remove(ProcessName, out var value2))
		{
			TowerDefenseBattleDependenceData dependenceData2 = value2.dependenceData;
			value2.Dispose();
			dependenceData2?.Dispose();
		}
	}

	private void FreeDetachedUi()
	{
		if (GodotObject.IsInstanceValid(_pauseButton))
		{
			_pauseButton.Free();
		}
		if (GodotObject.IsInstanceValid(_optionButton))
		{
			_optionButton.Free();
		}
		if (GodotObject.IsInstanceValid(_speedCheckBox))
		{
			_speedCheckBox.Free();
		}
		_pauseButton = null;
		_optionButton = null;
		_speedCheckBox = null;
	}

	private async Task<bool> WaitUntil(Func<bool> predicate, int maximumFrames)
	{
		for (int frame = 0; frame < maximumFrames; frame++)
		{
			if (predicate())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return predicate();
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BattleFeatureFullTimingRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(12)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.BuildPhaseExpectation, new Godot.Bridge.PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.BuildProgressPhaseExpectation, new Godot.Bridge.PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "freshPhase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "progressPhase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupControl, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateProbeLevel, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateTutorialFirstLevel, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateProbeNewLevel, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ProbeData, new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "canLoadProgress", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterProbeGraph, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.UnregisterProbeGraph, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FreeDetachedUi, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BuildPhaseExpectation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildPhaseExpectation(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildProgressPhaseExpectation && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildProgressPhaseExpectation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SetupControl && args.Count == 2)
		{
			SetupControl(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateProbeLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(CreateProbeLevel());
			return true;
		}
		if (method == MethodName.CreateTutorialFirstLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(CreateTutorialFirstLevel());
			return true;
		}
		if (method == MethodName.CreateProbeNewLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelNewConfig>(CreateProbeNewLevel());
			return true;
		}
		if (method == MethodName.ProbeData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ProbeData(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterProbeGraph && args.Count == 0)
		{
			RegisterProbeGraph();
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterProbeGraph && args.Count == 0)
		{
			UnregisterProbeGraph();
			ret = default;
			return true;
		}
		if (method == MethodName.FreeDetachedUi && args.Count == 0)
		{
			FreeDetachedUi();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildPhaseExpectation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildPhaseExpectation(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildProgressPhaseExpectation && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildProgressPhaseExpectation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateProbeLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(CreateProbeLevel());
			return true;
		}
		if (method == MethodName.CreateTutorialFirstLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(CreateTutorialFirstLevel());
			return true;
		}
		if (method == MethodName.CreateProbeNewLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelNewConfig>(CreateProbeNewLevel());
			return true;
		}
		if (method == MethodName.ProbeData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ProbeData(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterProbeGraph && args.Count == 0)
		{
			RegisterProbeGraph();
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterProbeGraph && args.Count == 0)
		{
			UnregisterProbeGraph();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.BuildPhaseExpectation)
		{
			return true;
		}
		if (method == MethodName.BuildProgressPhaseExpectation)
		{
			return true;
		}
		if (method == MethodName.SetupControl)
		{
			return true;
		}
		if (method == MethodName.CreateProbeLevel)
		{
			return true;
		}
		if (method == MethodName.CreateTutorialFirstLevel)
		{
			return true;
		}
		if (method == MethodName.CreateProbeNewLevel)
		{
			return true;
		}
		if (method == MethodName.ProbeData)
		{
			return true;
		}
		if (method == MethodName.RegisterProbeGraph)
		{
			return true;
		}
		if (method == MethodName.UnregisterProbeGraph)
		{
			return true;
		}
		if (method == MethodName.FreeDetachedUi)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<BattleFeatureFullTimingRuntimeControlStub>(in value);
			return true;
		}
		if (name == PropertyName._pauseButton)
		{
			_pauseButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._optionButton)
		{
			_optionButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		if (name == PropertyName._speedCheckBox)
		{
			_speedCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._pauseButton)
		{
			value = VariantUtils.CreateFrom(in _pauseButton);
			return true;
		}
		if (name == PropertyName._optionButton)
		{
			value = VariantUtils.CreateFrom(in _optionButton);
			return true;
		}
		if (name == PropertyName._speedCheckBox)
		{
			value = VariantUtils.CreateFrom(in _speedCheckBox);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._pauseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._optionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._speedCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._pauseButton, Variant.From(in _pauseButton));
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
		info.AddProperty(PropertyName._speedCheckBox, Variant.From(in _speedCheckBox));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value3))
		{
			_control = value3.As<BattleFeatureFullTimingRuntimeControlStub>();
		}
		if (info.TryGetProperty(PropertyName._pauseButton, out var value4))
		{
			_pauseButton = value4.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._optionButton, out var value5))
		{
			_optionButton = value5.As<SpriteBrightButton>();
		}
		if (info.TryGetProperty(PropertyName._speedCheckBox, out var value6))
		{
			_speedCheckBox = value6.As<CheckBox>();
		}
	}
}
