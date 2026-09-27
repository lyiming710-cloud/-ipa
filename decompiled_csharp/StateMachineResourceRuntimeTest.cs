using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/StateMachineResourceRuntimeTest.cs")]
public class StateMachineResourceRuntimeTest : Node
{
	private sealed class PhysicsFastTargetProbe : IStateMachinePhysicsFastCallbackTarget
	{
		internal readonly List<string> Trace = new List<string>();

		public void InvokeStateMachinePhysicsFastCallback(int callbackId, double delta)
		{
			Trace.Add($"fast:{callbackId}:{delta:F3}");
		}
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName BindLegacyTrace = "BindLegacyTrace";

		public static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName RunLegacyScenario = "RunLegacyScenario";

		public static readonly StringName RunLegacyScenarioAsync = "RunLegacyScenarioAsync";

		public static readonly StringName RunValidationScenario = "RunValidationScenario";

		public static readonly StringName CreateValidDefinition = "CreateValidDefinition";

		public static readonly StringName AssertValidationCodes = "AssertValidationCodes";

		public static readonly StringName CreateTransition = "CreateTransition";

		public static readonly StringName AddCompoundWithLeaf = "AddCompoundWithLeaf";

		public static readonly StringName RunCompilerScenario = "RunCompilerScenario";

		public static readonly StringName CreateCompilerDefinition = "CreateCompilerDefinition";

		public static readonly StringName CreateCompositionBaseDefinition = "CreateCompositionBaseDefinition";

		public static readonly StringName CreateCompositionDerivedDefinition = "CreateCompositionDerivedDefinition";

		public static readonly StringName CreateCompilerTransition = "CreateCompilerTransition";

		public static readonly StringName RunRuntimeScenario = "RunRuntimeScenario";

		public static readonly StringName RunAdvancedVisualSemanticsScenario = "RunAdvancedVisualSemanticsScenario";

		public static readonly StringName RunAdvancedDelayScenario = "RunAdvancedDelayScenario";

		public static readonly StringName RunAdvancedGuardScenario = "RunAdvancedGuardScenario";

		public static readonly StringName RunAdvancedParallelScenario = "RunAdvancedParallelScenario";

		public static readonly StringName RunAdvancedHistoryScenario = "RunAdvancedHistoryScenario";

		public static readonly StringName AdvancedState = "AdvancedState";

		public static readonly StringName AdvancedTransition = "AdvancedTransition";

		public static readonly StringName RunControllerScenario = "RunControllerScenario";

		public static readonly StringName ExecuteControllerFlatDirectScenario = "ExecuteControllerFlatDirectScenario";

		public static readonly StringName ExecuteControllerScenario = "ExecuteControllerScenario";

		public static readonly StringName RunControllerDisposeDuringCallbackScenario = "RunControllerDisposeDuringCallbackScenario";

		public static readonly StringName ExecuteControllerDisposeDuringCallbackScenario = "ExecuteControllerDisposeDuringCallbackScenario";

		public static readonly StringName RunControllerCacheScenario = "RunControllerCacheScenario";

		public static readonly StringName ExecuteControllerCacheScenario = "ExecuteControllerCacheScenario";

		public static readonly StringName RunParityScenario = "RunParityScenario";

		public static readonly StringName RunParityScenarioAsync = "RunParityScenarioAsync";

		public static readonly StringName RunSnapshotScenario = "RunSnapshotScenario";

		public static readonly StringName RunCharacterSchedulingScenario = "RunCharacterSchedulingScenario";

		public static readonly StringName RunCharacterSchedulingScenarioAsync = "RunCharacterSchedulingScenarioAsync";

		public static readonly StringName RunComponentOwnerScenario = "RunComponentOwnerScenario";

		public static readonly StringName RunComponentOwnerScenarioAsync = "RunComponentOwnerScenarioAsync";

		public static readonly StringName RunResourceComponentOwnerScenario = "RunResourceComponentOwnerScenario";

		public static readonly StringName RunResourceComponentOwnerScenarioAsync = "RunResourceComponentOwnerScenarioAsync";

		public static readonly StringName RunBaseCharacterScenario = "RunBaseCharacterScenario";

		public static readonly StringName ExecuteBaseCharacterScenario = "ExecuteBaseCharacterScenario";

		public static readonly StringName AddCharacterProbeNode = "AddCharacterProbeNode";

		public static readonly StringName CreateCharacterSchedulingDefinition = "CreateCharacterSchedulingDefinition";

		public static readonly StringName CountNodeBranch = "CountNodeBranch";

		public static readonly StringName RunSnapshotScenarioAsync = "RunSnapshotScenarioAsync";

		public static readonly StringName RunSnapshotAutomaticFallbackScenario = "RunSnapshotAutomaticFallbackScenario";

		public static readonly StringName CreateSnapshotEnvelope = "CreateSnapshotEnvelope";

		public static readonly StringName OnRuntimePhysicsProcessing = "OnRuntimePhysicsProcessing";

		public static readonly StringName RunNestedRuntimeOrderScenario = "RunNestedRuntimeOrderScenario";

		public static readonly StringName RunRuntimeExceptionRecoveryScenario = "RunRuntimeExceptionRecoveryScenario";

		public static readonly StringName RunRuntimeCallbackAtomicScenario = "RunRuntimeCallbackAtomicScenario";

		public static readonly StringName RunRuntimeDisposeCallbackScenario = "RunRuntimeDisposeCallbackScenario";

		public static readonly StringName RunRuntimeReinitializeCallbackScenario = "RunRuntimeReinitializeCallbackScenario";

		public static readonly StringName RunRuntimeQueueBudgetScenario = "RunRuntimeQueueBudgetScenario";

		public static readonly StringName RunRuntimeAutomaticBacklogScenario = "RunRuntimeAutomaticBacklogScenario";

		public static readonly StringName RunRuntimeMultiplePendingScenario = "RunRuntimeMultiplePendingScenario";

		public static readonly StringName RunRuntimeCompoundDelayOnceScenario = "RunRuntimeCompoundDelayOnceScenario";

		public static readonly StringName RunRuntimeReentrantTickScenario = "RunRuntimeReentrantTickScenario";

		public static readonly StringName RunRuntimeSingleActivePhysicsOrderScenario = "RunRuntimeSingleActivePhysicsOrderScenario";

		public static readonly StringName RunRuntimePhysicsFastTargetScenario = "RunRuntimePhysicsFastTargetScenario";

		public static readonly StringName CreateRuntimeDefinition = "CreateRuntimeDefinition";

		public static readonly StringName CreateFlatDirectDefinition = "CreateFlatDirectDefinition";

		public static readonly StringName CreateEventDelaySnapshotDefinition = "CreateEventDelaySnapshotDefinition";

		public static readonly StringName CreateParityDefinition = "CreateParityDefinition";

		public static readonly StringName CreateSnapshotDefinition = "CreateSnapshotDefinition";

		public static readonly StringName CreateSnapshotAutomaticDefinition = "CreateSnapshotAutomaticDefinition";

		public static readonly StringName RunEditingScenario = "RunEditingScenario";

		public static readonly StringName RunMigrationInventoryScenario = "RunMigrationInventoryScenario";

		public static readonly StringName CreateUnsupportedImportChart = "CreateUnsupportedImportChart";

		public static readonly StringName ImportMapsEqual = "ImportMapsEqual";

		public static readonly StringName LayoutsEqual = "LayoutsEqual";

		public static readonly StringName FindImportedState = "FindImportedState";

		public static readonly StringName FindImportedTransition = "FindImportedTransition";

		public static readonly StringName CreateLegacyHost = "CreateLegacyHost";

		public static readonly StringName CreateLegacyRoot = "CreateLegacyRoot";

		public static readonly StringName CreateRuntimeBudgetDefinition = "CreateRuntimeBudgetDefinition";

		public static readonly StringName CreateNestedRuntimeDefinition = "CreateNestedRuntimeDefinition";

		public static readonly StringName CreateMultiplePendingRuntimeDefinition = "CreateMultiplePendingRuntimeDefinition";

		public static readonly StringName CreateCompoundDelayRuntimeDefinition = "CreateCompoundDelayRuntimeDefinition";

		public static readonly StringName CreateCompoundRuntimeDefinition = "CreateCompoundRuntimeDefinition";

		public static readonly StringName CreateRuntimeTransition = "CreateRuntimeTransition";

		public static readonly StringName EmitTrace = "EmitTrace";

		public static readonly StringName EmitSnapshot = "EmitSnapshot";

		public static readonly StringName EmitMetrics = "EmitMetrics";

		public static readonly StringName CountStateChartRelatedNodes = "CountStateChartRelatedNodes";

		public static readonly StringName ReadCommandLineArguments = "ReadCommandLineArguments";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _chart = "_chart";

		public static readonly StringName _root = "_root";

		public static readonly StringName _idle = "_idle";

		public static readonly StringName _attack = "_attack";

		public static readonly StringName _recover = "_recover";

		public static readonly StringName _toRecover = "_toRecover";

		public static readonly StringName _attackRequested = "_attackRequested";

		public static readonly StringName _physicsFrames = "_physicsFrames";

		public static readonly StringName _rootPhysicsCallbacks = "_rootPhysicsCallbacks";

		public static readonly StringName _runtimePhysicsCallbacks = "_runtimePhysicsCallbacks";

		public static readonly StringName _snapshotRoundTripPassed = "_snapshotRoundTripPassed";

		public static readonly StringName _fullRate = "_fullRate";

		public static readonly StringName _requestedSpawnCount = "_requestedSpawnCount";

		public static readonly StringName _mode = "_mode";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ExpectedLegacyTrace = "enter:Root,enter:Idle,event:ToAttack,exit:Idle,enter:Attack,taken:ToRecover,exit:Attack,enter:Recover,event:ToIdle,exit:Recover,enter:Idle";

	private const string LegacySuccessMarker = "STATE_MACHINE_RESULT passed=True";

	private readonly List<string> _trace = new List<string>(16);

	private StateChart _chart;

	private CompoundState _root;

	private AtomicState _idle;

	private AtomicState _attack;

	private AtomicState _recover;

	private Transition _toRecover;

	private bool _attackRequested;

	private int _physicsFrames;

	private int _rootPhysicsCallbacks;

	private int _runtimePhysicsCallbacks;

	private bool _snapshotRoundTripPassed;

	private bool _fullRate;

	private int _requestedSpawnCount = 1;

	private string _mode = "Legacy";

	public override void _Ready()
	{
		ReadCommandLineArguments();
		if (string.Equals(_mode, "Legacy", StringComparison.OrdinalIgnoreCase))
		{
			BindLegacyTrace();
			RunLegacyScenario();
		}
		else if (string.Equals(_mode, "Validation", StringComparison.OrdinalIgnoreCase))
		{
			RunValidationScenario();
		}
		else if (string.Equals(_mode, "Compiler", StringComparison.OrdinalIgnoreCase))
		{
			RunCompilerScenario();
		}
		else if (string.Equals(_mode, "Runtime", StringComparison.OrdinalIgnoreCase))
		{
			RunRuntimeScenario();
		}
		else if (string.Equals(_mode, "AdvancedVisualSemantics", StringComparison.OrdinalIgnoreCase))
		{
			RunAdvancedVisualSemanticsScenario();
		}
		else if (string.Equals(_mode, "Controller", StringComparison.OrdinalIgnoreCase))
		{
			RunControllerScenario();
		}
		else if (string.Equals(_mode, "ControllerDisposeDuringCallback", StringComparison.OrdinalIgnoreCase))
		{
			RunControllerDisposeDuringCallbackScenario();
		}
		else if (string.Equals(_mode, "ControllerCache", StringComparison.OrdinalIgnoreCase))
		{
			RunControllerCacheScenario();
		}
		else if (string.Equals(_mode, "Parity", StringComparison.OrdinalIgnoreCase))
		{
			RunParityScenario();
		}
		else if (string.Equals(_mode, "Snapshot", StringComparison.OrdinalIgnoreCase))
		{
			RunSnapshotScenario();
		}
		else if (string.Equals(_mode, "BaseCharacter", StringComparison.OrdinalIgnoreCase))
		{
			RunBaseCharacterScenario();
		}
		else if (string.Equals(_mode, "CharacterScheduling", StringComparison.OrdinalIgnoreCase))
		{
			RunCharacterSchedulingScenario();
		}
		else if (string.Equals(_mode, "ComponentOwner", StringComparison.OrdinalIgnoreCase))
		{
			RunComponentOwnerScenario();
		}
		else if (string.Equals(_mode, "ResourceComponentOwner", StringComparison.OrdinalIgnoreCase))
		{
			RunResourceComponentOwnerScenario();
		}
		else if (string.Equals(_mode, "Editing", StringComparison.OrdinalIgnoreCase))
		{
			RunEditingScenario();
		}
		else if (string.Equals(_mode, "MigrationInventory", StringComparison.OrdinalIgnoreCase))
		{
			RunMigrationInventoryScenario();
		}
		else
		{
			GD.PushError("Unsupported state machine probe mode '" + _mode + "'.");
			GetTree().Quit(2);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		_physicsFrames++;
	}

	private void BindLegacyTrace()
	{
		_chart = GetNode<StateChart>("StateChart");
		_root = GetNode<CompoundState>("StateChart/Root");
		_idle = GetNode<AtomicState>("StateChart/Root/Idle");
		_attack = GetNode<AtomicState>("StateChart/Root/Attack");
		_recover = GetNode<AtomicState>("StateChart/Root/Recover");
		_toRecover = GetNode<Transition>("StateChart/Root/Attack/ToRecover");
		_chart.OnEventReceived += (StringName eventName) =>
		{
			_trace.Add($"event:{eventName}");
		};
		_root.OnStateEntered += () =>
		{
			_trace.Add("enter:Root");
		};
		_idle.OnStateEntered += IdleEntered;
		_idle.OnStateExited += () =>
		{
			_trace.Add("exit:Idle");
		};
		_attack.OnStateEntered += () =>
		{
			_trace.Add("enter:Attack");
		};
		_attack.OnStateExited += () =>
		{
			_trace.Add("exit:Attack");
		};
		_recover.OnStateEntered += () =>
		{
			_trace.Add("enter:Recover");
		};
		_recover.OnStateExited += () =>
		{
			_trace.Add("exit:Recover");
		};
		_toRecover.OnTaken += () =>
		{
			_trace.Add("taken:ToRecover");
		};
		_root.OnStatePhysicsProcessing += (double _) =>
		{
			_rootPhysicsCallbacks++;
		};
	}

	private void IdleEntered()
	{
		_trace.Add("enter:Idle");
		if (!_attackRequested)
		{
			_attackRequested = true;
			_chart.SendEvent("ToAttack");
		}
	}

	private void RunLegacyScenario()
	{
		RunLegacyScenarioAsync();
	}

	private async void RunLegacyScenarioAsync()
	{
		bool flag = await WaitForActiveState(_recover, 180);
		if (flag)
		{
			_chart.SendEvent("ToIdle");
		}
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = await WaitForActiveState(_idle, 60);
		}
		bool returnedToIdle = flag2;
		SavedState savedState = new SavedState();
		if (returnedToIdle)
		{
			_root.StateSave(savedState);
			_root.StateRestore(savedState);
			_snapshotRoundTripPassed = _root.activeState == _idle && _idle.active;
		}
		int physicsFramesBefore = _physicsFrames;
		int physicsCallbacksBefore = _rootPhysicsCallbacks;
		for (int i = 0; i < 8; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		int num = _physicsFrames - physicsFramesBefore;
		int num2 = _rootPhysicsCallbacks - physicsCallbacksBefore;
		_fullRate = num > 0 && num2 == num;
		EmitTrace(_mode);
		EmitSnapshot(_mode);
		EmitMetrics(_mode);
		bool flag3 = returnedToIdle && string.Join(',', _trace) == "enter:Root,enter:Idle,event:ToAttack,exit:Idle,enter:Attack,taken:ToRecover,exit:Attack,enter:Recover,event:ToIdle,exit:Recover,enter:Idle" && _snapshotRoundTripPassed && _fullRate;
		GD.Print(flag3 ? "STATE_MACHINE_RESULT passed=True" : ("STATE_MACHINE_RESULT passed=False mode=" + _mode));
		GetTree().Quit((!flag3) ? 2 : 0);
	}

	private void RunValidationScenario()
	{
		bool flag = true;
		flag &= AssertValidationCodes("MissingDefinition", null, "SM001");
		StateMachineDefinition definition = CreateValidDefinition();
		flag &= AssertValidationCodes("Valid", definition);
		StateMachineDefinition stateMachineDefinition = CreateValidDefinition();
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Idle",
			DisplayName = "Duplicate Idle",
			ParentId = "Root"
		});
		flag &= AssertValidationCodes("DuplicateStableId", stateMachineDefinition, "SM002");
		StateMachineDefinition stateMachineDefinition2 = CreateValidDefinition();
		stateMachineDefinition2.RootStateId = "MissingRoot";
		flag &= AssertValidationCodes("InvalidRoot", stateMachineDefinition2, "SM003");
		StateMachineDefinition stateMachineDefinition3 = CreateValidDefinition();
		stateMachineDefinition3.States.Add(new StateMachineStateDefinition
		{
			StableId = "Orphan",
			DisplayName = "Orphan",
			ParentId = "MissingParent"
		});
		flag &= AssertValidationCodes("InvalidParent", stateMachineDefinition3, "SM004");
		StateMachineDefinition stateMachineDefinition4 = CreateValidDefinition();
		stateMachineDefinition4.States[0].InitialChildId = "MissingChild";
		flag &= AssertValidationCodes("InvalidInitialChild", stateMachineDefinition4, "SM005");
		StateMachineDefinition stateMachineDefinition5 = CreateValidDefinition();
		stateMachineDefinition5.Transitions.Add(CreateTransition("BadSource", "MissingSource", "Idle"));
		flag &= AssertValidationCodes("InvalidTransitionSource", stateMachineDefinition5, "SM006");
		StateMachineDefinition stateMachineDefinition6 = CreateValidDefinition();
		stateMachineDefinition6.Transitions.Add(CreateTransition("BadTarget", "Idle", "MissingTarget"));
		flag &= AssertValidationCodes("InvalidTransitionTarget", stateMachineDefinition6, "SM007");
		StateMachineDefinition stateMachineDefinition7 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition = CreateTransition("NegativeDelay", "Idle", "Idle");
		stateMachineTransitionDefinition.DelaySeconds = -0.01;
		stateMachineDefinition7.Transitions.Add(stateMachineTransitionDefinition);
		flag &= AssertValidationCodes("NegativeDelay", stateMachineDefinition7, "SM008");
		StateMachineDefinition stateMachineDefinition8 = CreateValidDefinition();
		stateMachineDefinition8.States.Add(new StateMachineStateDefinition
		{
			StableId = "Attack",
			DisplayName = "Attack",
			ParentId = "Root"
		});
		stateMachineDefinition8.Transitions.Add(CreateTransition("IdleToAttack", "Idle", "Attack", StateMachineTriggerKind.Automatic));
		stateMachineDefinition8.Transitions.Add(CreateTransition("AttackToIdle", "Attack", "Idle", StateMachineTriggerKind.Automatic));
		flag &= AssertValidationCodes("AutomaticCycle", stateMachineDefinition8, "SM009");
		StateMachineDefinition stateMachineDefinition9 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition2 = CreateTransition("Guarded", "Idle", "Idle");
		stateMachineTransitionDefinition2.GuardDefinition = new StateMachineDefinition();
		stateMachineDefinition9.Transitions.Add(stateMachineTransitionDefinition2);
		flag &= AssertValidationCodes("UnsupportedGuard", stateMachineDefinition9, "SM010");
		StateMachineDefinition stateMachineDefinition10 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition3 = CreateTransition("SupportedGuard", "Idle", "Idle");
		stateMachineTransitionDefinition3.GuardDefinition = new StateMachineGuardDefinition
		{
			ComparedProperty = "ready",
			Operator = StateMachineComparisonOperator.Equal,
			ExpectedValue = true
		};
		stateMachineDefinition10.Transitions.Add(stateMachineTransitionDefinition3);
		flag &= AssertValidationCodes("SupportedGuard", stateMachineDefinition10);
		StateMachineDefinition stateMachineDefinition11 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition4 = CreateTransition("SupportedDelay", "Idle", "Idle", StateMachineTriggerKind.Delay);
		stateMachineTransitionDefinition4.DelaySeconds = 0.25;
		stateMachineDefinition11.Transitions.Add(stateMachineTransitionDefinition4);
		flag &= AssertValidationCodes("SupportedDelay", stateMachineDefinition11);
		StateMachineDefinition stateMachineDefinition12 = CreateValidDefinition();
		stateMachineDefinition12.States[1].Kind = StateMachineStateKind.Parallel;
		flag &= AssertValidationCodes("IncompleteParallel", stateMachineDefinition12, "SM005");
		StateMachineDefinition stateMachineDefinition13 = CreateValidDefinition();
		stateMachineDefinition13.States[1].Kind = StateMachineStateKind.History;
		flag &= AssertValidationCodes("SupportedHistory", stateMachineDefinition13);
		StateMachineDefinition stateMachineDefinition14 = CreateValidDefinition();
		stateMachineDefinition14.RootStateId = "MissingRoot";
		stateMachineDefinition14.States.Add(new StateMachineStateDefinition
		{
			StableId = "Idle",
			DisplayName = "Duplicate Idle",
			ParentId = "Root"
		});
		stateMachineDefinition14.States.Add(new StateMachineStateDefinition
		{
			StableId = "Orphan",
			DisplayName = "Orphan",
			ParentId = "MissingParent"
		});
		StateMachineTransitionDefinition stateMachineTransitionDefinition5 = CreateTransition("InvalidAll", "MissingSource", "MissingTarget");
		stateMachineTransitionDefinition5.DelaySeconds = -1.0;
		stateMachineTransitionDefinition5.GuardDefinition = new StateMachineDefinition();
		stateMachineDefinition14.Transitions.Add(stateMachineTransitionDefinition5);
		flag &= AssertValidationCodes("DeterministicOrder", stateMachineDefinition14, "SM003", "SM002", "SM004", "SM006", "SM007", "SM008", "SM010");
		StateMachineDefinition stateMachineDefinition15 = CreateValidDefinition();
		stateMachineDefinition15.States.Add(new StateMachineStateDefinition());
		stateMachineDefinition15.States.Add(new StateMachineStateDefinition
		{
			StableId = "Attack",
			DisplayName = "Attack",
			ParentId = "Root"
		});
		stateMachineDefinition15.Transitions.Add(CreateTransition("SparseIdleToAttack", "Idle", "Attack", StateMachineTriggerKind.Automatic));
		stateMachineDefinition15.Transitions.Add(CreateTransition("SparseAttackToIdle", "Attack", "Idle", StateMachineTriggerKind.Automatic));
		flag &= AssertValidationCodes("SparseAutomaticCycle", stateMachineDefinition15, "SM002", "SM009");
		StateMachineDefinition stateMachineDefinition16 = CreateValidDefinition();
		stateMachineDefinition16.States.Add(new StateMachineStateDefinition
		{
			StableId = "AtomicChild",
			DisplayName = "Atomic Child",
			ParentId = "Idle"
		});
		flag &= AssertValidationCodes("AtomicParent", stateMachineDefinition16, "SM004");
		StateMachineDefinition definition2 = CreateValidDefinition();
		AddCompoundWithLeaf(definition2, "CycleA", "CycleB", "CycleALeaf");
		AddCompoundWithLeaf(definition2, "CycleB", "CycleA", "CycleBLeaf");
		flag &= AssertValidationCodes("ParentCycle", definition2, "SM004", "SM004", "SM004", "SM004");
		StateMachineDefinition definition3 = CreateValidDefinition();
		AddCompoundWithLeaf(definition3, "Detached", string.Empty, "DetachedLeaf");
		flag &= AssertValidationCodes("DisconnectedParent", definition3, "SM004", "SM004");
		StateMachineDefinition stateMachineDefinition17 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition6 = CreateTransition("NegativeAutomatic", "Idle", "Idle", StateMachineTriggerKind.Automatic);
		stateMachineTransitionDefinition6.DelaySeconds = -1.0;
		stateMachineDefinition17.Transitions.Add(stateMachineTransitionDefinition6);
		flag &= AssertValidationCodes("NegativeAutomatic", stateMachineDefinition17, "SM008");
		StateMachineDefinition stateMachineDefinition18 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition7 = CreateTransition("NaNDelay", "Idle", "Idle");
		stateMachineTransitionDefinition7.DelaySeconds = 0.0 / 0.0;
		stateMachineDefinition18.Transitions.Add(stateMachineTransitionDefinition7);
		flag &= AssertValidationCodes("NaNDelay", stateMachineDefinition18, "SM008");
		StateMachineDefinition stateMachineDefinition19 = CreateValidDefinition();
		StateMachineTransitionDefinition stateMachineTransitionDefinition8 = CreateTransition("InfiniteDelay", "Idle", "Idle");
		stateMachineTransitionDefinition8.DelaySeconds = 1.0 / 0.0;
		stateMachineDefinition19.Transitions.Add(stateMachineTransitionDefinition8);
		flag &= AssertValidationCodes("InfiniteDelay", stateMachineDefinition19, "SM008");
		GD.Print(flag ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=Validation");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private StateMachineDefinition CreateValidDefinition()
	{
		return new StateMachineDefinition
		{
			DefinitionId = "probe.validation",
			RootStateId = "Root",
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = "Root",
					DisplayName = "Root",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = "Idle"
				},
				new StateMachineStateDefinition
				{
					StableId = "Idle",
					DisplayName = "Idle",
					Kind = StateMachineStateKind.Atomic,
					ParentId = "Root"
				}
			}
		};
	}

	private bool AssertValidationCodes(string caseName, StateMachineDefinition definition, params string[] expectedCodes)
	{
		StateMachineValidationResult stateMachineValidationResult = StateMachineValidator.Validate(definition);
		string[] array = new string[stateMachineValidationResult.Diagnostics.Count];
		for (int i = 0; i < stateMachineValidationResult.Diagnostics.Count; i++)
		{
			array[i] = stateMachineValidationResult.Diagnostics[i].Code;
		}
		bool flag = array.Length == expectedCodes.Length;
		if (flag)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (!string.Equals(array[j], expectedCodes[j], StringComparison.Ordinal))
				{
					flag = false;
					break;
				}
			}
		}
		flag &= stateMachineValidationResult.IsValid == (expectedCodes.Length == 0);
		GD.Print($"STATE_MACHINE_VALIDATION case={caseName} codes={string.Join(',', array)} expected={string.Join(',', expectedCodes)} passed={flag}");
		return flag;
	}

	private static StateMachineTransitionDefinition CreateTransition(string stableId, string sourceStateId, string targetStateId, StateMachineTriggerKind triggerKind = StateMachineTriggerKind.Event)
	{
		return new StateMachineTransitionDefinition
		{
			StableId = stableId,
			SourceStateId = sourceStateId,
			TargetStateId = targetStateId,
			TriggerKind = triggerKind,
			EventName = ((triggerKind == StateMachineTriggerKind.Event) ? new StringName(stableId) : new StringName())
		};
	}

	private static void AddCompoundWithLeaf(StateMachineDefinition definition, string compoundId, string parentId, string leafId)
	{
		definition.States.Add(new StateMachineStateDefinition
		{
			StableId = compoundId,
			DisplayName = compoundId,
			Kind = StateMachineStateKind.Compound,
			ParentId = parentId,
			InitialChildId = leafId
		});
		definition.States.Add(new StateMachineStateDefinition
		{
			StableId = leafId,
			DisplayName = leafId,
			ParentId = compoundId
		});
	}

	private void RunCompilerScenario()
	{
		StateMachineDefinition definition = CreateCompilerDefinition("probe.compiler.first");
		StateMachineDefinition definition2 = CreateCompilerDefinition("probe.compiler.first");
		bool flag = StateMachineCompiler.TryCompile(definition, out var program, out var validation);
		bool flag2 = StateMachineCompiler.TryCompile(definition2, out var program2, out var validation2);
		bool flag3 = (flag & flag2) && validation.IsValid && validation2.IsValid && ProgramsEquivalent(program, program2);
		bool flag4 = flag3 && program.States.Length == 4 && program.States[0].StableId == "Root" && program.States[1].StableId == "Idle" && program.States[2].StableId == "Attack" && program.States[3].StableId == "Recover";
		string[] array = new string[5] { "IdleAlpha", "IdleBeta", "AttackRecover", "RecoverIdle", "RootReset" };
		bool flag5 = flag3 && program.Transitions.Length == array.Length && program2.Transitions.Length == array.Length;
		if (flag5)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!(program.Transitions[i].StableId == array[i]) || !(program2.Transitions[i].StableId == array[i]))
				{
					flag5 = false;
					break;
				}
			}
		}
		bool flag6 = flag3 && EventSliceMatches(program, "Reset", 4) && EventSliceMatches(program, "ToAttack", 0, 1) && EventSliceMatches(program, "ToIdle", 3) && EventSliceMatches(program2, "Reset", 4) && EventSliceMatches(program2, "ToAttack", 0, 1) && EventSliceMatches(program2, "ToIdle", 3) && program.AutomaticTransitionIndices.SequenceEqual(new int[1] { 2 }) && program2.AutomaticTransitionIndices.SequenceEqual(new int[1] { 2 });
		StateMachineDefinition stateMachineDefinition = CreateCompilerDefinition("probe.compiler.first");
		Array<StateMachineTransitionDefinition> array2 = new Array<StateMachineTransitionDefinition>();
		for (int num = stateMachineDefinition.Transitions.Count - 1; num >= 0; num--)
		{
			array2.Add(stateMachineDefinition.Transitions[num]);
		}
		stateMachineDefinition.Transitions = array2;
		bool flag7 = StateMachineCompiler.TryCompile(stateMachineDefinition, out var program3, out var validation3) && ProgramsEquivalent(program, program3);
		StateMachineDefinition stateMachineDefinition2 = CreateCompilerDefinition("probe.compiler.null-collections");
		stateMachineDefinition2.Transitions = null;
		stateMachineDefinition2.Aliases = null;
		bool flag8 = StateMachineCompiler.TryCompile(stateMachineDefinition2, out var program4, out var validation4) && validation4.IsValid && program4.TransitionCount == 0 && program4.EventTransitionIndices.Length == 0 && program4.AutomaticTransitionIndices.Length == 0;
		StateMachineDefinition stateMachineDefinition3 = CreateCompilerDefinition("probe.compiler.null-entries");
		stateMachineDefinition3.States.Add(null);
		stateMachineDefinition3.Transitions.Add(null);
		bool flag9 = !StateMachineCompiler.TryCompile(stateMachineDefinition3, out var program5, out var validation5) && program5 == null && validation5.Diagnostics.Count == 2 && validation5.Diagnostics[0].Code == "SM002" && validation5.Diagnostics[1].Code == "SM002";
		StateMachineDefinition stateMachineDefinition4 = CreateCompilerDefinition("probe.compiler.first");
		stateMachineDefinition4.SetMeta("editor_positions", new Vector2(320f, 180f));
		bool flag10 = StateMachineCompiler.TryCompile(stateMachineDefinition4, out var program6, out validation3) && string.Equals(program.ContentHash, program6.ContentHash, StringComparison.Ordinal);
		StateMachineDefinition stateMachineDefinition5 = CreateCompilerDefinition("probe.compiler.first");
		int stateIndex = stateMachineDefinition5.Transitions[0].Priority++;
		bool flag11 = StateMachineCompiler.TryCompile(stateMachineDefinition5, out var program7, out validation3) && !string.Equals(program.ContentHash, program7.ContentHash, StringComparison.Ordinal);
		StateMachineDefinition stateMachineDefinition6 = CreateCompositionBaseDefinition("probe.composition.base");
		StateMachineDefinition definition3 = CreateCompositionDerivedDefinition("probe.composition.derived", stateMachineDefinition6, overrideInitial: false);
		bool flag12 = StateMachineCompiler.TryCompile(definition3, out var program8, out var validation6) && validation6.IsValid && program8.StateCount == 3 && program8.TryGetStateIndex("Root", out stateIndex) && program8.TryGetStateIndex("Idle", out stateIndex) && program8.TryGetStateIndex("Attack", out stateIndex);
		bool flag13 = StateMachineCompiler.TryCompile(CreateCompositionDerivedDefinition("probe.composition.override", stateMachineDefinition6, overrideInitial: true), out var program9, out validation3) && program9.TryGetStateIndex("Root", out var stateIndex2) && program9.TryGetStateIndex("Attack", out var stateIndex3) && program9.States[stateIndex2].InitialChildIndex == stateIndex3;
		StateMachineDefinition stateMachineDefinition7 = new StateMachineDefinition
		{
			DefinitionId = "probe.composition.cycle.a"
		};
		StateMachineDefinition baseDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.composition.cycle.b",
			BaseDefinition = stateMachineDefinition7
		};
		stateMachineDefinition7.BaseDefinition = baseDefinition;
		bool flag14 = !StateMachineCompiler.TryCompile(stateMachineDefinition7, out var _, out var validation7) && validation7.Diagnostics.Count == 1 && validation7.Diagnostics[0].Code == "SM011";
		bool flag15 = StateMachineCompiler.TryCompile(definition3, out var program11, out validation3);
		stateMachineDefinition6.States[1].DisplayName = "Idle Changed";
		bool flag16 = StateMachineCompiler.TryCompile(definition3, out var program12, out validation3);
		bool flag17 = (flag15 & flag16) && !string.Equals(program11.ContentHash, program12.ContentHash, StringComparison.Ordinal);
		StateMachineDefinition stateMachineDefinition8 = CreateCompilerDefinition("probe.compiler.extension");
		StateMachineStateDefinition stateMachineStateDefinition = stateMachineDefinition8.States[1];
		StateMachineResourceRuntimeExtensionState stateMachineResourceRuntimeExtensionState = new StateMachineResourceRuntimeExtensionState
		{
			StableId = stateMachineStateDefinition.StableId,
			DisplayName = stateMachineStateDefinition.DisplayName,
			Kind = stateMachineStateDefinition.Kind,
			ParentId = stateMachineStateDefinition.ParentId,
			InitialChildId = stateMachineStateDefinition.InitialChildId,
			ProcessFlags = stateMachineStateDefinition.ProcessFlags,
			CallbackKey = stateMachineStateDefinition.CallbackKey,
			ProbeStateValue = 7
		};
		stateMachineDefinition8.States[1] = stateMachineResourceRuntimeExtensionState;
		StateMachineTransitionDefinition stateMachineTransitionDefinition = stateMachineDefinition8.Transitions[0];
		StateMachineResourceRuntimeExtensionTransition stateMachineResourceRuntimeExtensionTransition = new StateMachineResourceRuntimeExtensionTransition
		{
			StableId = stateMachineTransitionDefinition.StableId,
			SourceStateId = stateMachineTransitionDefinition.SourceStateId,
			TargetStateId = stateMachineTransitionDefinition.TargetStateId,
			TriggerKind = stateMachineTransitionDefinition.TriggerKind,
			EventName = stateMachineTransitionDefinition.EventName,
			DelaySeconds = stateMachineTransitionDefinition.DelaySeconds,
			Priority = stateMachineTransitionDefinition.Priority,
			DeclarationOrder = stateMachineTransitionDefinition.DeclarationOrder,
			GuardDefinition = stateMachineTransitionDefinition.GuardDefinition,
			ProbeTransitionValue = "alpha"
		};
		stateMachineDefinition8.Transitions[0] = stateMachineResourceRuntimeExtensionTransition;
		bool flag18 = StateMachineCompiler.TryCompile(stateMachineDefinition8, out var program13, out var validation8) && validation8.IsValid && program13.TryGetStateExtensionProperty(stateMachineResourceRuntimeExtensionState.StableId, "ProbeStateValue", out var value) && value.AsInt64() == 7 && program13.TryGetTransitionExtensionProperty(stateMachineResourceRuntimeExtensionTransition.StableId, "ProbeTransitionValue", out var value2) && value2.AsString() == "alpha";
		stateMachineResourceRuntimeExtensionState.ProbeStateValue = 11;
		stateMachineResourceRuntimeExtensionTransition.ProbeTransitionValue = "beta";
		bool flag19 = StateMachineCompiler.TryCompile(stateMachineDefinition8, out var program14, out validation3) && !string.Equals(program13.ContentHash, program14.ContentHash, StringComparison.Ordinal) && program14.TryGetStateExtensionProperty(stateMachineResourceRuntimeExtensionState.StableId, "ProbeStateValue", out var value3) && value3.AsInt64() == 11 && program14.TryGetTransitionExtensionProperty(stateMachineResourceRuntimeExtensionTransition.StableId, "ProbeTransitionValue", out var value4) && value4.AsString() == "beta";
		GD.Print($"STATE_MACHINE_COMPILER deterministic={flag3} stableStateOrder={flag4} transitionOrder={flag5} eventSlicesEqual={flag6} transitionReorderStable={flag7} nullCollections={flag8} invalidNullEntriesHandled={flag9} layoutHashStable={flag10} logicalHashChanged={flag11} compositionInherited={flag12} compositionOverride={flag13} compositionCycleBlocked={flag14} baseHashInvalidates={flag17} extensionValuesPreserved={flag18} extensionMutationInvalidates={flag19}");
		StateMachineDefinition definition4 = CreateCompilerDefinition("probe.cache.shared");
		StateMachineProgram stateMachineProgram = StateMachineProgramCache.Acquire(definition4);
		StateMachineProgram stateMachineProgram2 = StateMachineProgramCache.Acquire(definition4);
		bool flag20 = stateMachineProgram == stateMachineProgram2;
		bool flag21 = StateMachineProgramCache.Release(stateMachineProgram);
		StateMachineDefinition definition5 = CreateCompilerDefinition("probe.cache.pinned");
		StateMachineProgram stateMachineProgram3 = StateMachineProgramCache.Acquire(definition5, pinned: true);
		bool flag22 = StateMachineProgramCache.Release(stateMachineProgram3);
		bool flag23 = StateMachineProgramCache.Release(StateMachineProgramCache.Acquire(CreateCompilerDefinition("probe.cache.unused")));
		int num2 = StateMachineProgramCache.ClearUnused();
		StateMachineProgramCacheStats stats = StateMachineProgramCache.GetStats();
		bool flag24 = flag21 && num2 == 1 && stats.ActiveEntryCount == 1 && stats.PinnedEntryCount == 1 && stats.TotalReferences == 1;
		bool flag25 = flag22 && stats.PinnedEntryCount == 1;
		StateMachineProgram stateMachineProgram4 = StateMachineProgramCache.Acquire(definition5);
		flag25 &= stateMachineProgram3 == stateMachineProgram4;
		StateMachineProgramCache.SetPinned(stateMachineProgram4, pinned: false);
		StateMachineProgramCache.Release(stateMachineProgram4);
		bool flag26 = StateMachineProgramCache.Release(stateMachineProgram2);
		bool flag27 = (flag23 & flag26) && StateMachineProgramCache.ClearUnused() == 2;
		StateMachineDefinition stateMachineDefinition9 = CreateCompilerDefinition("probe.cache.path");
		stateMachineDefinition9.ResourcePath = "res://Test/.state_machine_cache_shared_probe.tres";
		StateMachineProgram stateMachineProgram5 = StateMachineProgramCache.Acquire(stateMachineDefinition9);
		stateMachineDefinition9.ResourcePath = string.Empty;
		StateMachineDefinition stateMachineDefinition10 = CreateCompilerDefinition("probe.cache.path");
		stateMachineDefinition10.ResourcePath = "res://Test/.state_machine_cache_shared_probe.tres";
		StateMachineProgram stateMachineProgram6 = StateMachineProgramCache.Acquire(stateMachineDefinition10);
		stateMachineDefinition10.ResourcePath = string.Empty;
		bool flag28 = stateMachineProgram5 == stateMachineProgram6;
		StateMachineDefinition stateMachineDefinition11 = CreateCompilerDefinition("probe.cache.path");
		stateMachineDefinition11.Transitions[0].Priority++;
		stateMachineDefinition11.ResourcePath = "res://Test/.state_machine_cache_shared_probe.tres";
		StateMachineProgram stateMachineProgram7 = StateMachineProgramCache.Acquire(stateMachineDefinition11);
		stateMachineDefinition11.ResourcePath = string.Empty;
		bool flag29 = stateMachineProgram5 != stateMachineProgram7;
		StateMachineProgramCache.Release(stateMachineProgram5);
		StateMachineProgramCache.Release(stateMachineProgram6);
		StateMachineProgramCache.Release(stateMachineProgram7);
		StateMachineProgramCache.ClearUnused();
		StateMachineDefinition definition6 = CreateCompilerDefinition("probe.cache.embedded");
		StateMachineDefinition definition7 = CreateCompilerDefinition("probe.cache.embedded");
		StateMachineProgram stateMachineProgram8 = StateMachineProgramCache.Acquire(definition6);
		StateMachineProgram stateMachineProgram9 = StateMachineProgramCache.Acquire(definition7);
		bool flag30 = stateMachineProgram8 != stateMachineProgram9;
		StateMachineProgramCache.Release(stateMachineProgram8);
		StateMachineProgramCache.Release(stateMachineProgram9);
		StateMachineProgramCache.ClearUnused();
		StateMachineDefinition stateMachineDefinition12 = CreateCompilerDefinition("probe.cache.fast.root");
		StateMachineProgram stateMachineProgram10 = StateMachineProgramCache.Acquire(stateMachineDefinition12);
		StateMachineProgram stateMachineProgram11 = StateMachineProgramCache.Acquire(stateMachineDefinition12);
		bool flag31 = stateMachineProgram10 == stateMachineProgram11;
		stateMachineDefinition12.Transitions[0].Priority++;
		stateMachineDefinition12.EmitChanged();
		StateMachineProgram stateMachineProgram12 = StateMachineProgramCache.Acquire(stateMachineDefinition12);
		bool flag32 = stateMachineProgram10 != stateMachineProgram12 && !string.Equals(stateMachineProgram10.ContentHash, stateMachineProgram12.ContentHash, StringComparison.Ordinal);
		StateMachineProgramCache.Release(stateMachineProgram10);
		StateMachineProgramCache.Release(stateMachineProgram11);
		StateMachineProgramCache.Release(stateMachineProgram12);
		StateMachineProgramCache.ClearUnused();
		StateMachineProgram stateMachineProgram13 = StateMachineProgramCache.Acquire(stateMachineDefinition12);
		bool flag33 = stateMachineProgram12 != stateMachineProgram13 && string.Equals(stateMachineProgram12.ContentHash, stateMachineProgram13.ContentHash, StringComparison.Ordinal);
		StateMachineProgramCache.Release(stateMachineProgram13);
		StateMachineDefinition stateMachineDefinition13 = CreateCompilerDefinition("probe.cache.fast.base");
		StateMachineDefinition definition8 = new StateMachineDefinition
		{
			DefinitionId = "probe.cache.fast.derived",
			BaseDefinition = stateMachineDefinition13
		};
		StateMachineProgram stateMachineProgram14 = StateMachineProgramCache.Acquire(definition8);
		stateMachineDefinition13.States[1].DisplayName = "Idle Changed";
		stateMachineDefinition13.EmitChanged();
		StateMachineProgram stateMachineProgram15 = StateMachineProgramCache.Acquire(definition8);
		bool flag34 = stateMachineProgram14 != stateMachineProgram15 && !string.Equals(stateMachineProgram14.ContentHash, stateMachineProgram15.ContentHash, StringComparison.Ordinal);
		StateMachineProgramCache.Release(stateMachineProgram14);
		StateMachineProgramCache.Release(stateMachineProgram15);
		StateMachineProgramCache.ClearUnused();
		GD.Print($"STATE_MACHINE_CACHE sharedReference={flag20} activeSurvivedCleanup={flag24} unusedReleased={flag27} pinnedSurvivedCleanup={flag25} pathBackedShared={flag28} pathHashIsolated={flag29} embeddedIsolated={flag30} fastRootShared={flag31} fastRootInvalidates={flag32} fastRootReacquiresAfterCleanup={flag33} fastBaseInvalidates={flag34}");
		bool flag35 = flag3 & flag4 & flag5 & flag6 & flag7 & flag8 & flag9 & flag10 & flag11 & flag12 & flag13 & flag14 & flag17 & flag18 & flag19 & flag20 & flag24 & flag27 & flag25 & flag28 & flag29 & flag30 & flag31 & flag32 & flag33 & flag34;
		GD.Print(flag35 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=Compiler");
		GetTree().Quit((!flag35) ? 2 : 0);
	}

	private static StateMachineDefinition CreateCompilerDefinition(string definitionId)
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = definitionId,
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "Idle"
		});
		string[] array = new string[3] { "Idle", "Attack", "Recover" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
		}
		stateMachineDefinition.Transitions.Add(CreateCompilerTransition("RootReset", "Root", "Idle", "Reset", 100, 0));
		stateMachineDefinition.Transitions.Add(CreateCompilerTransition("IdleBeta", "Idle", "Recover", "ToAttack", 10, 2));
		stateMachineDefinition.Transitions.Add(CreateCompilerTransition("RecoverIdle", "Recover", "Idle", "ToIdle", 5, 3));
		stateMachineDefinition.Transitions.Add(CreateCompilerTransition("IdleAlpha", "Idle", "Attack", "ToAttack", 10, 2));
		StateMachineTransitionDefinition stateMachineTransitionDefinition = CreateCompilerTransition("AttackRecover", "Attack", "Recover", string.Empty, 5, 1);
		stateMachineTransitionDefinition.TriggerKind = StateMachineTriggerKind.Automatic;
		stateMachineTransitionDefinition.DelaySeconds = 0.5;
		stateMachineDefinition.Transitions.Add(stateMachineTransitionDefinition);
		stateMachineDefinition.Aliases["OldIdle"] = "Idle";
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateCompositionBaseDefinition(string definitionId)
	{
		return new StateMachineDefinition
		{
			DefinitionId = definitionId,
			RootStateId = "Root",
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = "Root",
					DisplayName = "Root",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = "Idle"
				},
				new StateMachineStateDefinition
				{
					StableId = "Idle",
					DisplayName = "Idle",
					ParentId = "Root"
				}
			}
		};
	}

	private static StateMachineDefinition CreateCompositionDerivedDefinition(string definitionId, StateMachineDefinition baseDefinition, bool overrideInitial)
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = definitionId,
			BaseDefinition = baseDefinition
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Attack",
			DisplayName = "Attack",
			ParentId = "Root"
		});
		if (overrideInitial)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = "Root",
				DisplayName = "Root",
				Kind = StateMachineStateKind.Compound,
				InitialChildId = "Attack"
			});
		}
		return stateMachineDefinition;
	}

	private static StateMachineTransitionDefinition CreateCompilerTransition(string stableId, string sourceStateId, string targetStateId, string eventName, int priority, int declarationOrder)
	{
		return new StateMachineTransitionDefinition
		{
			StableId = stableId,
			SourceStateId = sourceStateId,
			TargetStateId = targetStateId,
			TriggerKind = StateMachineTriggerKind.Event,
			EventName = new StringName(eventName),
			Priority = priority,
			DeclarationOrder = declarationOrder
		};
	}

	private static bool ProgramsEquivalent(StateMachineProgram left, StateMachineProgram right)
	{
		if (left == null || right == null || left.DefinitionId != right.DefinitionId || left.SchemaVersion != right.SchemaVersion || !string.Equals(left.ContentHash, right.ContentHash, StringComparison.Ordinal) || left.States.Length != right.States.Length || left.Transitions.Length != right.Transitions.Length || !left.EventTransitionIndices.SequenceEqual(right.EventTransitionIndices) || !left.AutomaticTransitionIndices.SequenceEqual(right.AutomaticTransitionIndices))
		{
			return false;
		}
		for (int i = 0; i < left.States.Length; i++)
		{
			CompiledStateMachineState compiledStateMachineState = left.States[i];
			CompiledStateMachineState compiledStateMachineState2 = right.States[i];
			if (compiledStateMachineState.StableId != compiledStateMachineState2.StableId || compiledStateMachineState.DisplayName != compiledStateMachineState2.DisplayName || compiledStateMachineState.Kind != compiledStateMachineState2.Kind || compiledStateMachineState.ParentIndex != compiledStateMachineState2.ParentIndex || compiledStateMachineState.InitialChildIndex != compiledStateMachineState2.InitialChildIndex || compiledStateMachineState.Depth != compiledStateMachineState2.Depth || compiledStateMachineState.ProcessFlags != compiledStateMachineState2.ProcessFlags || compiledStateMachineState.CallbackKey != compiledStateMachineState2.CallbackKey)
			{
				return false;
			}
		}
		for (int j = 0; j < left.Transitions.Length; j++)
		{
			CompiledStateMachineTransition compiledStateMachineTransition = left.Transitions[j];
			CompiledStateMachineTransition compiledStateMachineTransition2 = right.Transitions[j];
			if (compiledStateMachineTransition.SourceIndex != compiledStateMachineTransition2.SourceIndex || compiledStateMachineTransition.TargetIndex != compiledStateMachineTransition2.TargetIndex || compiledStateMachineTransition.TriggerKind != compiledStateMachineTransition2.TriggerKind || compiledStateMachineTransition.EventName != compiledStateMachineTransition2.EventName || BitConverter.DoubleToInt64Bits(compiledStateMachineTransition.DelaySeconds) != BitConverter.DoubleToInt64Bits(compiledStateMachineTransition2.DelaySeconds) || compiledStateMachineTransition.Priority != compiledStateMachineTransition2.Priority || compiledStateMachineTransition.DeclarationOrder != compiledStateMachineTransition2.DeclarationOrder || compiledStateMachineTransition.StableId != compiledStateMachineTransition2.StableId)
			{
				return false;
			}
		}
		return true;
	}

	private static bool EventSliceMatches(StateMachineProgram program, string eventName, params int[] expectedTransitionIndices)
	{
		StringName stringName = new StringName(eventName);
		if (!program.TryGetEventTransitionSlice(stringName, out var slice) || slice.Count != expectedTransitionIndices.Length)
		{
			return false;
		}
		for (int i = 0; i < expectedTransitionIndices.Length; i++)
		{
			int num = program.EventTransitionIndices[slice.Offset + i];
			if (num != expectedTransitionIndices[i] || program.Transitions[num].EventName != stringName)
			{
				return false;
			}
		}
		return true;
	}

	private void RunRuntimeScenario()
	{
		bool flag = StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var validation) && validation.IsValid;
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("Root");
		StateHandle idle = stateMachineRuntime.GetStateHandle("Idle");
		StateHandle stateHandle2 = stateMachineRuntime.GetStateHandle("Attack");
		StateHandle stateHandle3 = stateMachineRuntime.GetStateHandle("Recover");
		List<string> trace = new List<string>(24);
		bool reentrantSent = false;
		int delayedTakenCount = 0;
		stateHandle.Entered += () =>
		{
			trace.Add("enter:Root");
		};
		idle.Entered += () =>
		{
			trace.Add("enter:Idle");
			if (!reentrantSent)
			{
				reentrantSent = true;
				idle.SendEvent("ToAttack");
			}
		};
		idle.Exited += () =>
		{
			trace.Add("exit:Idle");
		};
		stateHandle2.Entered += () =>
		{
			trace.Add("enter:Attack");
		};
		stateHandle2.Exited += () =>
		{
			trace.Add("exit:Attack");
		};
		stateHandle3.Entered += () =>
		{
			trace.Add("enter:Recover");
		};
		stateHandle3.Exited += () =>
		{
			trace.Add("exit:Recover");
		};
		stateMachineRuntime.EventReceived += (StringName eventName) =>
		{
			trace.Add($"event:{eventName}");
		};
		stateMachineRuntime.TransitionTaken += (CompiledStateMachineTransition transition) =>
		{
			trace.Add("taken:" + transition.StableId);
			if (transition.StableId == "AttackToRecover")
			{
				delayedTakenCount++;
			}
		};
		bool flag2 = (stateMachineRuntime.EnterInitialState() & flag) && string.Join(',', trace) == "enter:Root,enter:Idle,event:ToAttack,taken:IdleToAttack,exit:Idle,enter:Attack" && stateMachineRuntime.IsActive("Attack") && stateMachineRuntime.PendingDelayRemaining > 0.0;
		stateMachineRuntime.TickProcess(0.02);
		stateMachineRuntime.TickProcess(0.02);
		bool num = stateMachineRuntime.IsActive("Attack") && delayedTakenCount == 0 && stateMachineRuntime.PendingDelayRemaining > 0.0;
		stateMachineRuntime.TickProcess(0.02);
		bool flag3 = num && stateMachineRuntime.IsActive("Recover") && delayedTakenCount == 1 && stateMachineRuntime.PendingDelayRemaining == 0.0;
		long revision = stateMachineRuntime.Revision;
		stateMachineRuntime.SendEvent("Unknown");
		bool flag4 = stateMachineRuntime.Revision == revision && stateMachineRuntime.IsActive("Recover");
		stateMachineRuntime.SendEvent("ToIdle");
		bool flag5 = stateMachineRuntime.IsActive("Idle") && !stateMachineRuntime.IsActive("Recover");
		idle.PhysicsProcessing += OnRuntimePhysicsProcessing;
		for (int num2 = 0; num2 < 1000; num2++)
		{
			stateMachineRuntime.TickPhysics(1.0 / 60.0);
		}
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		for (int num3 = 0; num3 < 10000; num3++)
		{
			stateMachineRuntime.TickPhysics(1.0 / 60.0);
		}
		long num4 = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		bool flag6 = num4 == 0L && _runtimePhysicsCallbacks >= 11000;
		bool flag7 = StateMachineCompiler.TryCompile(CreateRuntimeBudgetDefinition(), out var program2, out var _);
		StateMachineRuntime stateMachineRuntime2 = new StateMachineRuntime();
		stateMachineRuntime2.Initialize(program2);
		bool budgetEventRaised = false;
		stateMachineRuntime2.Diagnostic += (string code) =>
		{
			budgetEventRaised |= code == "SMR001";
		};
		stateMachineRuntime2.EnterInitialState();
		bool flag8 = (flag7 & budgetEventRaised) && stateMachineRuntime2.LastDiagnosticCode == "SMR001";
		bool flag9 = RunNestedRuntimeOrderScenario();
		bool flag10 = RunRuntimeExceptionRecoveryScenario();
		bool flag11 = RunRuntimeCallbackAtomicScenario();
		bool flag12 = RunRuntimeDisposeCallbackScenario();
		bool flag13 = RunRuntimeReinitializeCallbackScenario();
		bool flag14 = RunRuntimeQueueBudgetScenario();
		bool flag15 = RunRuntimeAutomaticBacklogScenario();
		bool flag16 = RunRuntimeMultiplePendingScenario();
		bool flag17 = RunRuntimeCompoundDelayOnceScenario();
		bool flag18 = RunRuntimeReentrantTickScenario();
		bool flag19 = RunRuntimeSingleActivePhysicsOrderScenario();
		bool flag20 = RunRuntimePhysicsFastTargetScenario();
		GD.Print($"STATE_MACHINE_RUNTIME reentrantOrder={flag2} delayedOnce={flag3} unknownIgnored={flag4} returnedToIdle={flag5} nestedOrder={flag9} budgetDiagnostic={flag8} exceptionRecovered={flag10} callbackAtomic={flag11} disposeSafe={flag12} reinitializeRejected={flag13} queueBudget={flag14} automaticBacklogCleared={flag15} multiplePending={flag16} compoundDelayOnce={flag17} reentrantTickRejected={flag18} singleActivePhysicsOrder={flag19} physicsFastTarget={flag20} physicsAllocatedBytes={num4} allocationFree={flag6}");
		stateMachineRuntime2.Dispose();
		stateMachineRuntime.Dispose();
		bool flag21 = !idle.IsValid && !stateHandle.IsValid;
		bool flag22 = flag & flag2 & flag3 & flag4 & flag5 & flag9 & flag8 & flag10 & flag11 & flag12 & flag13 & flag14 & flag15 & flag16 & flag17 & flag18 & flag19 & flag20 & flag6 & flag21;
		GD.Print(flag22 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=Runtime");
		GetTree().Quit((!flag22) ? 2 : 0);
	}

	private void RunAdvancedVisualSemanticsScenario()
	{
		bool flag = RunAdvancedDelayScenario();
		bool flag2 = RunAdvancedGuardScenario();
		bool flag3 = RunAdvancedParallelScenario();
		bool flag4 = RunAdvancedHistoryScenario();
		bool flag5 = flag & flag2 & flag3 & flag4;
		GD.Print($"STATE_MACHINE_ADVANCED delay={flag} guard={flag2} parallel={flag3} history={flag4}");
		GD.Print(flag5 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=AdvancedVisualSemantics");
		GetTree().Quit((!flag5) ? 2 : 0);
	}

	private static bool RunAdvancedDelayScenario()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.advanced.delay",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(AdvancedState("Root", StateMachineStateKind.Compound, "", "Idle"));
		stateMachineDefinition.States.Add(AdvancedState("Idle", StateMachineStateKind.Atomic, "Root"));
		stateMachineDefinition.States.Add(AdvancedState("Done", StateMachineStateKind.Atomic, "Root"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("DelayToDone", "Idle", "Done", StateMachineTriggerKind.Delay, "", 0.05));
		using StateMachineController stateMachineController = new StateMachineController();
		if (!stateMachineController.Initialize(stateMachineDefinition) || !stateMachineController.EnterInitialState())
		{
			return false;
		}
		StateHandle stateById = stateMachineController.GetStateById("Idle");
		int num;
		if (stateById != null && stateById.IsActive)
		{
			StateHandle stateById2 = stateMachineController.GetStateById("Done");
			if (stateById2 != null && !stateById2.IsActive)
			{
				num = (stateMachineController.HasPendingTransition ? 1 : 0);
				goto IL_010a;
			}
		}
		num = 0;
		goto IL_010a;
		IL_010a:
		stateMachineController.TickProcess(0.04);
		StateHandle stateById3 = stateMachineController.GetStateById("Idle");
		int num2;
		if (stateById3 != null && stateById3.IsActive)
		{
			StateHandle stateById4 = stateMachineController.GetStateById("Done");
			num2 = ((stateById4 != null && !stateById4.IsActive) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag = (byte)num2 != 0;
		stateMachineController.TickProcess(0.02);
		int result;
		if (((uint)num & (flag ? 1u : 0u)) != 0)
		{
			StateHandle stateById5 = stateMachineController.GetStateById("Idle");
			if (stateById5 != null && !stateById5.IsActive)
			{
				StateHandle stateById6 = stateMachineController.GetStateById("Done");
				if (stateById6 != null && stateById6.IsActive)
				{
					result = ((!stateMachineController.HasPendingTransition) ? 1 : 0);
					goto IL_01a4;
				}
			}
		}
		result = 0;
		goto IL_01a4;
		IL_01a4:
		return (byte)result != 0;
	}

	private static bool RunAdvancedGuardScenario()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.advanced.guard",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(AdvancedState("Root", StateMachineStateKind.Compound, "", "Locked"));
		stateMachineDefinition.States.Add(AdvancedState("Locked", StateMachineStateKind.Atomic, "Root"));
		stateMachineDefinition.States.Add(AdvancedState("Open", StateMachineStateKind.Atomic, "Root"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("GuardedOpen", "Locked", "Open", StateMachineTriggerKind.Automatic, "", 0.0, 0, new StateMachineGuardDefinition
		{
			ComparedProperty = "score",
			Operator = StateMachineComparisonOperator.GreaterOrEqual,
			ExpectedValue = 2
		}));
		using StateMachineController stateMachineController = new StateMachineController();
		if (!stateMachineController.Initialize(stateMachineDefinition) || !stateMachineController.EnterInitialState())
		{
			return false;
		}
		stateMachineController.SetExpressionProperty("score", 1);
		StateHandle stateById = stateMachineController.GetStateById("Locked");
		int num;
		if (stateById != null && stateById.IsActive)
		{
			StateHandle stateById2 = stateMachineController.GetStateById("Open");
			num = ((stateById2 != null && !stateById2.IsActive) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		stateMachineController.SetExpressionProperty("score", 2);
		int result;
		if (num != 0)
		{
			StateHandle stateById3 = stateMachineController.GetStateById("Locked");
			if (stateById3 != null && !stateById3.IsActive)
			{
				result = ((stateMachineController.GetStateById("Open")?.IsActive ?? false) ? 1 : 0);
				goto IL_018d;
			}
		}
		result = 0;
		goto IL_018d;
		IL_018d:
		return (byte)result != 0;
	}

	private static bool RunAdvancedParallelScenario()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.advanced.parallel",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(AdvancedState("Root", StateMachineStateKind.Parallel));
		stateMachineDefinition.States.Add(AdvancedState("RegionA", StateMachineStateKind.Compound, "Root", "A1"));
		stateMachineDefinition.States.Add(AdvancedState("A1", StateMachineStateKind.Atomic, "RegionA"));
		stateMachineDefinition.States.Add(AdvancedState("A2", StateMachineStateKind.Atomic, "RegionA"));
		stateMachineDefinition.States.Add(AdvancedState("RegionB", StateMachineStateKind.Compound, "Root", "B1"));
		stateMachineDefinition.States.Add(AdvancedState("B1", StateMachineStateKind.Atomic, "RegionB"));
		stateMachineDefinition.States.Add(AdvancedState("B2", StateMachineStateKind.Atomic, "RegionB"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("AdvanceA", "A1", "A2", StateMachineTriggerKind.Event, "Advance"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("AdvanceB", "B1", "B2", StateMachineTriggerKind.Event, "Advance", 0.0, 1));
		using StateMachineController stateMachineController = new StateMachineController();
		if (!stateMachineController.Initialize(stateMachineDefinition) || !stateMachineController.EnterInitialState())
		{
			return false;
		}
		StateHandle stateById = stateMachineController.GetStateById("A1");
		bool num = stateById != null && stateById.IsActive && (stateMachineController.GetStateById("B1")?.IsActive ?? false);
		stateMachineController.SendEvent("Advance");
		int result;
		if (num)
		{
			StateHandle stateById2 = stateMachineController.GetStateById("A1");
			if (stateById2 != null && !stateById2.IsActive)
			{
				StateHandle stateById3 = stateMachineController.GetStateById("B1");
				if (stateById3 != null && !stateById3.IsActive)
				{
					StateHandle stateById4 = stateMachineController.GetStateById("A2");
					if (stateById4 != null && stateById4.IsActive)
					{
						result = ((stateMachineController.GetStateById("B2")?.IsActive ?? false) ? 1 : 0);
						goto IL_022d;
					}
				}
			}
		}
		result = 0;
		goto IL_022d;
		IL_022d:
		return (byte)result != 0;
	}

	private static bool RunAdvancedHistoryScenario()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.advanced.history",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(AdvancedState("Root", StateMachineStateKind.Compound, "", "Container"));
		stateMachineDefinition.States.Add(AdvancedState("Container", StateMachineStateKind.Compound, "Root", "First"));
		stateMachineDefinition.States.Add(AdvancedState("First", StateMachineStateKind.Atomic, "Container"));
		stateMachineDefinition.States.Add(AdvancedState("Second", StateMachineStateKind.Atomic, "Container"));
		stateMachineDefinition.States.Add(AdvancedState("History", StateMachineStateKind.History, "Container"));
		stateMachineDefinition.States.Add(AdvancedState("Outside", StateMachineStateKind.Atomic, "Root"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("Next", "First", "Second", StateMachineTriggerKind.Event, "Next"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("Leave", "Second", "Outside", StateMachineTriggerKind.Event, "Leave"));
		stateMachineDefinition.Transitions.Add(AdvancedTransition("Return", "Outside", "History", StateMachineTriggerKind.Event, "Return"));
		using StateMachineController stateMachineController = new StateMachineController();
		if (!stateMachineController.Initialize(stateMachineDefinition) || !stateMachineController.EnterInitialState())
		{
			return false;
		}
		stateMachineController.SendEvent("Next");
		bool num = stateMachineController.GetStateById("Second")?.IsActive ?? false;
		stateMachineController.SendEvent("Leave");
		StateHandle stateById = stateMachineController.GetStateById("Outside");
		int num2;
		if (stateById != null && stateById.IsActive)
		{
			StateHandle stateById2 = stateMachineController.GetStateById("Second");
			num2 = ((stateById2 != null && !stateById2.IsActive) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag = (byte)num2 != 0;
		stateMachineController.SendEvent("Return");
		int result;
		if (num & flag)
		{
			StateHandle stateById3 = stateMachineController.GetStateById("Container");
			if (stateById3 != null && stateById3.IsActive)
			{
				StateHandle stateById4 = stateMachineController.GetStateById("Second");
				if (stateById4 != null && stateById4.IsActive)
				{
					StateHandle stateById5 = stateMachineController.GetStateById("First");
					result = ((stateById5 != null && !stateById5.IsActive) ? 1 : 0);
					goto IL_0260;
				}
			}
		}
		result = 0;
		goto IL_0260;
		IL_0260:
		return (byte)result != 0;
	}

	private static StateMachineStateDefinition AdvancedState(string stableId, StateMachineStateKind kind = StateMachineStateKind.Atomic, string parentId = "", string initialChildId = "")
	{
		return new StateMachineStateDefinition
		{
			StableId = stableId,
			DisplayName = stableId,
			Kind = kind,
			ParentId = parentId,
			InitialChildId = initialChildId
		};
	}

	private static StateMachineTransitionDefinition AdvancedTransition(string stableId, string sourceStateId, string targetStateId, StateMachineTriggerKind triggerKind = StateMachineTriggerKind.Event, string eventName = "", double delaySeconds = 0.0, int declarationOrder = 0, StateMachineGuardDefinition guard = null)
	{
		return new StateMachineTransitionDefinition
		{
			StableId = stableId,
			SourceStateId = sourceStateId,
			TargetStateId = targetStateId,
			TriggerKind = triggerKind,
			EventName = eventName,
			DelaySeconds = delaySeconds,
			DeclarationOrder = declarationOrder,
			GuardDefinition = guard
		};
	}

	private void RunControllerScenario()
	{
		bool flag = ExecuteControllerScenario();
		bool flag2 = ExecuteControllerFlatDirectScenario();
		bool flag3 = ExecuteControllerDisposeDuringCallbackScenario();
		bool flag4 = ExecuteControllerCacheScenario();
		bool flag5 = flag & flag2 & flag3 & flag4;
		GD.Print(flag5 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=Controller");
		GetTree().Quit((!flag5) ? 2 : 0);
	}

	private static bool ExecuteControllerFlatDirectScenario()
	{
		StateMachineDefinition definition = CreateFlatDirectDefinition();
		StateMachineController controller = new StateMachineController();
		try
		{
			using StateMachineController stateMachineController = new StateMachineController();
			if (!controller.Initialize(definition) || !stateMachineController.Initialize(definition))
			{
				return false;
			}
			StateHandle stateById = controller.GetStateById("FlatIdle");
			StateHandle stateById2 = controller.GetStateById("FlatAttack");
			StateHandle stateById3 = stateMachineController.GetStateById("FlatAttack");
			List<string> trace = new List<string>();
			stateById.Entered += () =>
			{
				trace.Add("enter:idle");
			};
			stateById.Exited += () =>
			{
				trace.Add("exit:idle");
			};
			stateById2.Entered += () =>
			{
				trace.Add("enter:attack");
			};
			stateById2.Exited += () =>
			{
				trace.Add("exit:attack");
			};
			controller.EventReceived += (StringName eventName) =>
			{
				trace.Add($"event:{eventName}");
			};
			controller.TransitionTaken += (CompiledStateMachineTransition transition) =>
			{
				trace.Add("taken:" + transition.StableId + ":" + controller.CurrentStateHandle?.StableId);
			};
			controller.TransitionCompleted += (CompiledStateMachineTransition transition) =>
			{
				trace.Add("completed:" + transition.StableId + ":" + controller.CurrentStateHandle?.StableId);
			};
			bool flag = controller.EnterInitialState() && stateById.IsActive;
			trace.Clear();
			long revision = controller.Revision;
			bool flag2 = controller.TrySetFlatState(stateById2, "ToAttack");
			bool flag3 = string.Join(',', trace) == "event:ToAttack,taken:FlatToAttack:FlatIdle,exit:idle,enter:attack,completed:FlatToAttack:FlatAttack";
			StateMachineSnapshot stateMachineSnapshot = controller.CaptureSnapshot();
			bool flag4 = stateMachineSnapshot != null && stateMachineSnapshot.ActiveStateIds.Contains("FlatAttack") && !stateMachineSnapshot.ActiveStateIds.Contains("FlatIdle") && controller.Revision == revision + 1;
			trace.Clear();
			bool flag5 = controller.TrySetFlatState(stateById2, "ToAttack") && string.Join(',', trace) == "event:ToAttack,taken:FlatToAttack:FlatAttack,exit:attack,enter:attack,completed:FlatToAttack:FlatAttack";
			long revision2 = controller.Revision;
			bool flag6 = !controller.TrySetFlatState(stateById, "Unknown") && controller.Revision == revision2 && stateById2.IsActive;
			bool flag7 = !controller.TrySetFlatState(stateById3, "ToAttack") && stateById2.IsActive;
			bool flag8 = controller.TrySetFlatState(stateById, "ToIdle") && stateById.IsActive;
			bool result = flag & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag8;
			GD.Print($"STATE_MACHINE_FLAT_DIRECT initial={flag} direct={flag2} ordered={flag3} snapshot={flag4} reenter={flag5} mismatch={flag6} foreign={flag7} idle={flag8}");
			return result;
		}
		finally
		{
			if (controller != null)
			{
				((IDisposable)controller).Dispose();
			}
		}
	}

	private bool ExecuteControllerScenario()
	{
		StateMachineController stateMachineController = new StateMachineController();
		int throwingDiagnosticCount = 0;
		stateMachineController.Diagnostic += (string _) =>
		{
			throwingDiagnosticCount++;
			throw new InvalidOperationException("controller diagnostic probe");
		};
		bool flag = false;
		try
		{
			flag = !stateMachineController.Initialize(null);
		}
		catch (InvalidOperationException)
		{
		}
		bool flag2 = flag && throwingDiagnosticCount == 1 && !stateMachineController.IsInitialized && !string.IsNullOrWhiteSpace(stateMachineController.InitializationError);
		stateMachineController.Dispose();
		StateMachineController controller = new StateMachineController();
		int initializationDiagnostics = 0;
		bool diagnosticContainsError = false;
		controller.Diagnostic += (string error) =>
		{
			initializationDiagnostics++;
			diagnosticContainsError |= !string.IsNullOrWhiteSpace(error);
		};
		bool flag3 = (!controller.Initialize(null) && !controller.IsInitialized && !string.IsNullOrWhiteSpace(controller.InitializationError) && !controller.HasProcessWork && !controller.HasPhysicsWork && initializationDiagnostics == 1) & diagnosticContainsError;
		StateMachineDefinition stateMachineDefinition = CreateRuntimeDefinition();
		stateMachineDefinition.Aliases["LegacyIdle"] = "Idle";
		stateMachineDefinition.States[1].DisplayName = "Idle Display";
		bool flag4 = controller.Initialize(stateMachineDefinition) && controller.IsInitialized && string.IsNullOrEmpty(controller.InitializationError);
		StateHandle stateById = controller.GetStateById("Idle");
		StateHandle stateById2 = controller.GetStateById("Attack");
		StateHandle stateById3 = controller.GetStateById("Recover");
		bool flag5 = controller.GetStateById("LegacyIdle") == null && stateById == controller.ResolveState("LegacyIdle") && controller.ResolveState("Idle Display") == null && stateById == controller.ResolveState("Idle Display", allowDisplayNameFallback: true);
		bool eventReceived = false;
		int physicsCallbacks = 0;
		List<string> transitionCallbackOrder = new List<string>();
		bool completedSnapshotExact = false;
		controller.EventReceived += (StringName eventName) =>
		{
			eventReceived |= eventName == (StringName)"ToAttack";
		};
		controller.TransitionTaken += (CompiledStateMachineTransition transition) =>
		{
			transitionCallbackOrder.Add("taken:" + transition.StableId + ":" + controller.CurrentStateHandle?.StableId);
		};
		controller.TransitionCompleted += (CompiledStateMachineTransition transition) =>
		{
			StateMachineSnapshot stateMachineSnapshot3 = controller.CaptureSnapshot();
			transitionCallbackOrder.Add("completed:" + transition.StableId + ":" + controller.CurrentStateHandle?.StableId);
			if (transition.StableId == "IdleToAttack")
			{
				completedSnapshotExact = stateMachineSnapshot3 != null && stateMachineSnapshot3.ActiveStateIds.Contains("Attack") && !stateMachineSnapshot3.ActiveStateIds.Contains("Idle") && stateMachineSnapshot3.Revision > 0;
			}
		};
		stateById.PhysicsProcessing += (double _) =>
		{
			physicsCallbacks++;
		};
		bool flag6 = controller.GetType().BaseType == typeof(object);
		bool flag7 = controller.CurrentStateHandle == null && controller.EnterInitialState() && stateById.IsActive && stateById == controller.CurrentStateHandle;
		controller.TickPhysics(1.0 / 60.0);
		bool flag8 = controller.HasPhysicsWork && physicsCallbacks == 1;
		controller.SetExpressionProperty("score", 42);
		controller.SetExpressionProperty("transient_resource", new Resource());
		controller.SendEvent("ToAttack");
		bool flag9 = (transitionCallbackOrder.Count >= 2 && transitionCallbackOrder[0] == "taken:IdleToAttack:Idle" && transitionCallbackOrder[1] == "completed:IdleToAttack:Attack") & completedSnapshotExact;
		bool flag10 = (eventReceived && stateById2.IsActive) & flag9;
		StateMachineSnapshot stateMachineSnapshot = controller.CaptureSnapshot();
		Dictionary dictionary = StateMachineSnapshotCodec.Encode(stateMachineSnapshot);
		StateMachineSnapshot stateMachineSnapshot2 = StateMachineSnapshotCodec.Decode(dictionary);
		Dictionary data = new Dictionary
		{
			["definition_id"] = stateMachineSnapshot.DefinitionId,
			["schema_version"] = stateMachineSnapshot.SchemaVersion,
			["content_hash"] = stateMachineSnapshot.ContentHash,
			["pending_delay_remaining"] = new Dictionary { ["AttackToRecover"] = 0.0 / 0.0 }
		};
		Dictionary data2 = new Dictionary { ["definition_id"] = stateMachineSnapshot.DefinitionId };
		string[] array = new string[6] { "revision", "active_state_ids", "history_state_ids", "pending_transition_ids", "pending_delay_remaining", "expression_properties" };
		bool flag11 = true;
		string[] array2 = array;
		foreach (string b in array2)
		{
			Dictionary dictionary2 = new Dictionary();
			foreach (Variant key6 in dictionary.Keys)
			{
				if (!string.Equals(key6.AsString(), b, StringComparison.Ordinal))
				{
					dictionary2[key6] = dictionary[key6];
				}
			}
			flag11 &= StateMachineSnapshotCodec.Decode(dictionary2) == null;
		}
		Dictionary dictionary3 = dictionary.Duplicate(deep: true);
		dictionary3["future_optional_field"] = 1;
		bool flag12 = ((dictionary.Count == 9 && stateMachineSnapshot2 != null && stateMachineSnapshot2.DefinitionId == stateMachineSnapshot.DefinitionId && stateMachineSnapshot2.ContentHash == stateMachineSnapshot.ContentHash && stateMachineSnapshot2.ActiveStateIds.Contains("Attack") && stateMachineSnapshot2.PendingTransitionIds.Contains("AttackToRecover") && stateMachineSnapshot2.ExpressionProperties.ContainsKey("score") && !stateMachineSnapshot2.ExpressionProperties.ContainsKey("transient_resource") && StateMachineSnapshotCodec.Decode(data) == null && StateMachineSnapshotCodec.Decode(data2) == null && StateMachineSnapshotCodec.Decode(dictionary3) != null) & flag11) && controller.RestoreSnapshot(stateMachineSnapshot2, suppressEntryEffects: true) && stateById2.IsActive;
		controller.TickProcess(0.06);
		bool flag13 = controller.HasProcessWork && stateById3.IsActive;
		GD.Print($"STATE_MACHINE_OWNER controller_pure={flag6} initial={flag7} event={flag10} process={flag13} physics={flag8} snapshot={flag12} transition_completed={flag9}");
		GD.Print($"STATE_MACHINE_OWNER initialization_diagnostic_safe={flag2}");
		controller.Dispose();
		return flag6 & flag2 & flag3 & flag4 & flag5 & flag7 & flag10 & flag13 & flag8 & flag12 & flag9;
	}

	private void RunControllerDisposeDuringCallbackScenario()
	{
		bool flag = ExecuteControllerDisposeDuringCallbackScenario();
		GD.Print(flag ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=ControllerDisposeDuringCallback");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static bool ExecuteControllerDisposeDuringCallbackScenario()
	{
		int totalReferences = StateMachineProgramCache.GetStats().TotalReferences;
		StateMachineController controller = new StateMachineController();
		bool flag = controller.Initialize(CreateRuntimeDefinition());
		StateHandle stateById = controller.GetStateById("Idle");
		bool callbackRan = false;
		controller.EventReceived += (StringName eventName) =>
		{
			if (!(eventName != (StringName)"ToAttack"))
			{
				callbackRan = true;
				controller.Dispose();
			}
		};
		controller.EnterInitialState();
		controller.SendEvent("ToAttack");
		bool flag2 = (flag & callbackRan) && !controller.IsInitialized && controller.Runtime == null && !stateById.IsValid;
		int totalReferences2 = StateMachineProgramCache.GetStats().TotalReferences;
		controller.Dispose();
		int totalReferences3 = StateMachineProgramCache.GetStats().TotalReferences;
		bool flag3 = totalReferences2 == totalReferences && totalReferences3 == totalReferences;
		GD.Print($"STATE_MACHINE_OWNER dispose_during_callback={flag2} release_once={flag3}");
		return flag2 & flag3;
	}

	private void RunControllerCacheScenario()
	{
		bool flag = ExecuteControllerCacheScenario();
		GD.Print(flag ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=ControllerCache");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private bool ExecuteControllerCacheScenario()
	{
		int totalReferences = StateMachineProgramCache.GetStats().TotalReferences;
		int num = Math.Max(1, _requestedSpawnCount);
		StateMachineController[] array = new StateMachineController[num];
		StateMachineDefinition definition = CreateRuntimeDefinition();
		StateMachineProgram stateMachineProgram = null;
		bool flag = true;
		try
		{
			for (int i = 0; i < array.Length; i++)
			{
				StateMachineController stateMachineController = new StateMachineController();
				flag &= stateMachineController.Initialize(definition);
				array[i] = stateMachineController;
				if (stateMachineProgram == null)
				{
					stateMachineProgram = stateMachineController.Runtime?.Program;
				}
				flag &= stateMachineProgram == stateMachineController.Runtime?.Program;
			}
			flag &= StateMachineProgramCache.GetStats().TotalReferences - totalReferences == num;
		}
		finally
		{
			for (int j = 0; j < array.Length; j++)
			{
				array[j]?.Dispose();
			}
		}
		int num2 = StateMachineProgramCache.GetStats().TotalReferences - totalReferences;
		flag &= num2 == 0;
		GD.Print($"STATE_MACHINE_OWNER cache_shared={flag} active_refs={num2}");
		return flag;
	}

	private void RunParityScenario()
	{
		RunParityScenarioAsync();
	}

	private async void RunParityScenarioAsync()
	{
		int baselineRegistrations = StateMachineRuntimeBatch.RegistrationCount;
		int baselineProcessRegistrations = StateMachineRuntimeBatch.ProcessRegistrationCount;
		int baselinePhysicsRegistrations = StateMachineRuntimeBatch.PhysicsRegistrationCount;
		int baselineCacheReferences = StateMachineProgramCache.GetStats().TotalReferences;
		StateChart node = GetNode<StateChart>("StateChart");
		bool autoLegacy = node.RuntimeMode == StateMachineRuntimeMode.Auto && node.Definition == null && !node.IsResourceMode && node.CurrentState != null;
		StateChart explicitLegacyChart = CreateLegacyHost("ExplicitLegacyChart");
		AddChild(explicitLegacyChart, forceReadableName: false, InternalMode.Disabled);
		bool explicitLegacy = explicitLegacyChart.RuntimeMode == StateMachineRuntimeMode.Legacy && !explicitLegacyChart.IsResourceMode && explicitLegacyChart.CurrentState != null;
		StateMachineDefinition definition = CreateParityDefinition();
		StateChart autoResourceChart = new StateChart
		{
			Name = "AutoResourceChart",
			RuntimeMode = StateMachineRuntimeMode.Auto,
			Definition = definition
		};
		CompoundState compoundState = CreateLegacyRoot("IgnoredRoot");
		autoResourceChart.AddChild(compoundState, forceReadableName: false, InternalMode.Disabled);
		AddChild(autoResourceChart, forceReadableName: false, InternalMode.Disabled);
		bool autoResource = autoResourceChart.IsResourceMode && autoResourceChart.ResourceInitializationSucceeded && autoResourceChart.CurrentState == null;
		bool childNodesIgnored = compoundState.ProcessMode == ProcessModeEnum.Disabled && !compoundState.active;
		List<string> trace = new List<string>(16);
		StateHandle stateById = autoResourceChart.GetStateById("Root");
		StateHandle idle = autoResourceChart.GetState("Idle");
		StateHandle stateById2 = autoResourceChart.GetStateById("Attack");
		StateHandle recover = autoResourceChart.GetStateById("Recover");
		bool attackRequested = false;
		int resourcePhysicsCallbacks = 0;
		autoResourceChart.OnEventReceived += (StringName eventName) =>
		{
			trace.Add($"event:{eventName}");
		};
		stateById.Entered += () =>
		{
			trace.Add("enter:Root");
		};
		idle.Entered += () =>
		{
			trace.Add("enter:Idle");
			if (!attackRequested)
			{
				attackRequested = true;
				autoResourceChart.SendEvent("ToAttack");
			}
		};
		idle.Exited += () =>
		{
			trace.Add("exit:Idle");
		};
		stateById2.Entered += () =>
		{
			trace.Add("enter:Attack");
		};
		stateById2.Exited += () =>
		{
			trace.Add("exit:Attack");
		};
		recover.Entered += () =>
		{
			trace.Add("enter:Recover");
		};
		recover.Exited += () =>
		{
			trace.Add("exit:Recover");
		};
		stateById.PhysicsProcessing += (double _) =>
		{
			resourcePhysicsCallbacks++;
		};
		autoResourceChart.ResourceRuntime.TransitionTaken += (CompiledStateMachineTransition transition) =>
		{
			if (transition.StableId == "ToRecover")
			{
				trace.Add("taken:ToRecover");
			}
		};
		StateChart explicitResourceChart = new StateChart
		{
			Name = "ExplicitResourceChart",
			RuntimeMode = StateMachineRuntimeMode.Resource,
			Definition = definition
		};
		AddChild(explicitResourceChart, forceReadableName: false, InternalMode.Disabled);
		bool explicitResource = explicitResourceChart.IsResourceMode && explicitResourceChart.ResourceInitializationSucceeded && explicitResourceChart.CurrentState == null;
		Node reparentSource = new Node
		{
			Name = "ReparentSource"
		};
		Node reparentTarget = new Node
		{
			Name = "ReparentTarget"
		};
		Node reparentCharacter = new Node
		{
			Name = "ReparentCharacter"
		};
		AddChild(reparentSource, forceReadableName: false, InternalMode.Disabled);
		AddChild(reparentTarget, forceReadableName: false, InternalMode.Disabled);
		reparentSource.AddChild(reparentCharacter, forceReadableName: false, InternalMode.Disabled);
		StateChart reparentChart = new StateChart
		{
			Name = "ReparentChart",
			RuntimeMode = StateMachineRuntimeMode.Resource,
			Definition = definition
		};
		reparentCharacter.AddChild(reparentChart, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		StateHandle stateById3 = reparentChart.GetStateById("Idle");
		reparentCharacter.Reparent(reparentTarget);
		bool reparentRuntimePreserved = reparentChart.ResourceInitializationSucceeded && stateById3 != null && stateById3.IsValid && stateById3 == reparentChart.GetStateById("Idle");
		bool reparentEventDelivered = false;
		if (reparentRuntimePreserved)
		{
			reparentChart.SendEvent("ToAttack");
			reparentEventDelivered = reparentChart.GetStateById("Attack")?.IsActive ?? false;
		}
		bool flag = await WaitForActiveState(recover, 180);
		if (flag)
		{
			autoResourceChart.SendEvent("ToIdle");
		}
		bool flag2 = flag;
		if (flag2)
		{
			flag2 = await WaitForActiveState(idle, 60);
		}
		bool handleLookup = flag2 && idle == autoResourceChart.CurrentStateHandle && idle == autoResourceChart.GetStateById("Idle");
		int physicsFramesBefore = _physicsFrames;
		int physicsCallbacksBefore = resourcePhysicsCallbacks;
		for (int i = 0; i < 8; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		int num = _physicsFrames - physicsFramesBefore;
		int num2 = resourcePhysicsCallbacks - physicsCallbacksBefore;
		bool resourceFullRate = num > 0 && num2 == num;
		bool traceMatches = string.Join(',', trace) == "enter:Root,enter:Idle,event:ToAttack,exit:Idle,enter:Attack,taken:ToRecover,exit:Attack,enter:Recover,event:ToIdle,exit:Recover,enter:Idle";
		bool batchRegistered = StateMachineRuntimeBatch.RegistrationCount >= baselineRegistrations + 2 && StateMachineRuntimeBatch.ProcessRegistrationCount >= baselineProcessRegistrations + 2 && StateMachineRuntimeBatch.PhysicsRegistrationCount >= baselinePhysicsRegistrations + 2;
		bool batchRootMounted = GodotObject.IsInstanceValid(StateMachineRuntimeBatch.Instance) && StateMachineRuntimeBatch.Instance.GetParent() == GetTree().Root;
		autoResourceChart.QueueFree();
		explicitResourceChart.QueueFree();
		explicitLegacyChart.QueueFree();
		reparentCharacter.QueueFree();
		reparentSource.QueueFree();
		reparentTarget.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool flag3 = StateMachineRuntimeBatch.RegistrationCount == baselineRegistrations && StateMachineRuntimeBatch.ProcessRegistrationCount == baselineProcessRegistrations && StateMachineRuntimeBatch.PhysicsRegistrationCount == baselinePhysicsRegistrations;
		bool flag4 = StateMachineProgramCache.GetStats().TotalReferences == baselineCacheReferences;
		GD.Print($"STATE_MACHINE_PARITY traceMatches={traceMatches} autoLegacy={autoLegacy} autoResource={autoResource} explicitLegacy={explicitLegacy} explicitResource={explicitResource} childNodesIgnored={childNodesIgnored} reparentRuntimePreserved={reparentRuntimePreserved} reparentEventDelivered={reparentEventDelivered} handleLookup={handleLookup} fullRate={resourceFullRate} batchRegistered={batchRegistered} batchRootMounted={batchRootMounted} batchReleased={flag3} cacheReleased={flag4}");
		bool flag5 = traceMatches & autoLegacy & autoResource & explicitLegacy & explicitResource & childNodesIgnored & reparentRuntimePreserved & reparentEventDelivered & handleLookup & resourceFullRate & batchRegistered & batchRootMounted & flag3 & flag4;
		GD.Print(flag5 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=Parity");
		GetTree().Quit((!flag5) ? 2 : 0);
	}

	private void RunSnapshotScenario()
	{
		RunSnapshotScenarioAsync();
	}

	private void RunCharacterSchedulingScenario()
	{
		RunCharacterSchedulingScenarioAsync();
	}

	private async void RunCharacterSchedulingScenarioAsync()
	{
		bool previousCharacterBatch = TowerDefenseCharacter.UseCharacterBatch;
		bool previousZombieBatch = TowerDefenseZombie.UseBatch;
		BattleEventBus eventBus = new BattleEventBus
		{
			Name = "CharacterSchedulingEventBus"
		};
		AddChild(eventBus, forceReadableName: false, InternalMode.Disabled);
		CharacterSchedulingProbeCharacter character = CreateCharacterSchedulingOwner<CharacterSchedulingProbeCharacter>("CharacterSchedulingOwner", CreateCharacterSchedulingDefinition("probe.character.scheduling", 60.0));
		TowerDefenseCharacter.UseCharacterBatch = false;
		AddChild(character, forceReadableName: false, InternalMode.Disabled);
		StateHandle characterIdle = character.GetStateById("probe.idle");
		int characterProcessCallbacks = 0;
		int characterPhysicsCallbacks = 0;
		characterIdle.Processing += (double _) =>
		{
			characterProcessCallbacks++;
		};
		characterIdle.PhysicsProcessing += (double _) =>
		{
			characterPhysicsCallbacks++;
			character.SchedulingTrace.Add("statePhysics");
		};
		character.editorPreviewMode = false;
		character.ActivateGameplayProcessing();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool ownerReady = (character.StateMachine?.IsInitialized ?? false) && characterIdle.IsActive;
		TowerDefenseCharacter.UseCharacterBatch = true;
		character.ActivateGameplayProcessing();
		while (!GodotObject.IsInstanceValid(TowerDefenseCharacterBatch.Instance) || !TowerDefenseCharacterBatch.Instance.IsInsideTree())
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		character.SetMainStateMachineDispatchEnabled(enabled: false);
		TowerDefenseCharacter.UseCharacterBatch = false;
		character.ActivateGameplayProcessing();
		character.SetMainStateMachineDispatchEnabled(enabled: true);
		int num = characterProcessCallbacks;
		character._Process(1.0 / 60.0);
		TowerDefenseCharacter.UseCharacterBatch = true;
		character.ActivateGameplayProcessing();
		TowerDefenseCharacterBatch.Instance?._Process(1.0 / 60.0);
		int processSwitchCallbacks = characterProcessCallbacks - num;
		bool processSingleTick = processSwitchCallbacks == 1;
		TowerDefenseCharacter.UseCharacterBatch = false;
		character.ActivateGameplayProcessing();
		character.SetMainStateMachineDispatchEnabled(enabled: false);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		character.SetMainStateMachineDispatchEnabled(enabled: true);
		TowerDefenseProcessModeDispatch.BeginFrame();
		character.ArmExistingPhysicsProbe(TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
		character.SchedulingTrace.Clear();
		int num2 = characterPhysicsCallbacks;
		int existingPhysicsUpdateCount = character.ExistingPhysicsUpdateCount;
		character._PhysicsProcess(1.0 / 60.0);
		TowerDefenseCharacter.UseCharacterBatch = true;
		character.ActivateGameplayProcessing();
		TowerDefenseCharacterBatch.Instance?._PhysicsProcess(1.0 / 60.0);
		int physicsSwitchCallbacks = characterPhysicsCallbacks - num2;
		int existingPhysicsSwitchUpdates = character.ExistingPhysicsUpdateCount - existingPhysicsUpdateCount;
		bool physicsSingleTick = physicsSwitchCallbacks == 1;
		bool existingPhysicsSingleUpdate = existingPhysicsSwitchUpdates == 1;
		int physicsSwitchTraceCount = character.SchedulingTrace.Count;
		bool physicsBeforeExistingBody = character.SchedulingTrace.Count == 2 && character.SchedulingTrace[0] == "statePhysics" && character.SchedulingTrace[1] == "existingPhysics";
		ulong physicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame + 1;
		character.ArmExistingPhysicsProbe(physicsFrame);
		character.SchedulingTrace.Clear();
		int num3 = characterPhysicsCallbacks;
		int existingPhysicsUpdateCount2 = character.ExistingPhysicsUpdateCount;
		character.ProbePhysicsFrame(1.0 / 60.0, physicsFrame);
		bool nextPhysicsFrameWholeUpdate = characterPhysicsCallbacks - num3 == 1 && character.ExistingPhysicsUpdateCount - existingPhysicsUpdateCount2 == 1 && character.SchedulingTrace.Count == 2 && character.SchedulingTrace[0] == "statePhysics" && character.SchedulingTrace[1] == "existingPhysics";
		character.SetMainStateMachineDispatchEnabled(enabled: false);
		int num4 = characterProcessCallbacks;
		int num5 = characterPhysicsCallbacks;
		character.BatchProcessUpdate(1.0 / 60.0);
		character.BatchUpdate(1.0 / 60.0);
		bool dispatchDisabledStopsTicks = characterProcessCallbacks == num4 && characterPhysicsCallbacks == num5;
		character.SetMainStateMachineDispatchEnabled(enabled: true);
		TowerDefenseCharacterBatch characterBatch = TowerDefenseCharacterBatch.Instance;
		while (GodotObject.IsInstanceValid(characterBatch) && !characterBatch.IsInsideTree())
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		int physicsBeforeBatchExit = characterPhysicsCallbacks;
		Node node = characterBatch?.GetParent();
		if (GodotObject.IsInstanceValid(node))
		{
			node.RemoveChild(characterBatch);
		}
		characterBatch?.Free();
		bool directPhysicsRestored = character.IsPhysicsProcessing();
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		character._PhysicsProcess(1.0 / 60.0);
		bool batchExitFallback = directPhysicsRestored && characterPhysicsCallbacks > physicsBeforeBatchExit;
		IStateMachineController controllerBeforeReparent = character.StateMachine;
		StateHandle handleBeforeReparent = characterIdle;
		Node parent = character.GetParent();
		parent.RemoveChild(character);
		parent.AddChild(character, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool reparentPreserved = controllerBeforeReparent == character.StateMachine && handleBeforeReparent == character.GetStateById("probe.idle") && handleBeforeReparent.IsValid && handleBeforeReparent.IsActive;
		TowerDefenseZombie zombie = CreateCharacterSchedulingOwner<TowerDefenseZombie>("ZombieSchedulingOwner", CreateCharacterSchedulingDefinition("probe.zombie.scheduling", 1.0));
		TowerDefenseZombie.UseBatch = true;
		AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		StateHandle zombieIdle = zombie.GetStateById("probe.idle");
		StateHandle zombieDone = zombie.GetStateById("probe.done");
		int zombieProcessCallbacks = 0;
		int zombiePhysicsCallbacks = 0;
		zombieIdle.Processing += (double _) =>
		{
			zombieProcessCallbacks++;
		};
		zombieIdle.PhysicsProcessing += (double _) =>
		{
			zombiePhysicsCallbacks++;
		};
		zombie.editorPreviewMode = false;
		zombie.ActivateGameplayProcessing();
		IStateMachineController zombieControllerBeforeReparent = zombie.StateMachine;
		Node parent2 = zombie.GetParent();
		parent2.RemoveChild(zombie);
		parent2.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool zombieReparentRegistered = zombieControllerBeforeReparent == zombie.StateMachine && zombieIdle.IsValid && zombieIdle.IsActive && zombie.IsOwnerBatchRegistered && zombie.IsBatchDispatchActive && !zombie.IsPhysicsProcessing();
		zombieProcessCallbacks = 0;
		zombiePhysicsCallbacks = 0;
		zombie.isPause = true;
		for (int i = 0; i < 5; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		bool flag = (zombie.StateMachine?.IsInitialized ?? false) && zombieProcessCallbacks == 0 && zombiePhysicsCallbacks == 0 && zombieIdle.IsActive && !zombieDone.IsActive;
		bool flag2 = ownerReady & processSingleTick & physicsSingleTick & existingPhysicsSingleUpdate & physicsBeforeExistingBody & nextPhysicsFrameWholeUpdate & dispatchDisabledStopsTicks & batchExitFallback & reparentPreserved & zombieReparentRegistered & flag;
		GD.Print($"STATE_MACHINE_CHARACTER_SCHEDULING ownerReady={ownerReady} processSingleTick={processSingleTick} processSwitchCallbacks={processSwitchCallbacks} physicsSingleTick={physicsSingleTick} physicsSwitchCallbacks={physicsSwitchCallbacks} existingPhysicsSingleUpdate={existingPhysicsSingleUpdate} existingPhysicsSwitchUpdates={existingPhysicsSwitchUpdates} physicsBeforeExistingBody={physicsBeforeExistingBody} physicsSwitchTraceCount={physicsSwitchTraceCount} nextPhysicsFrameWholeUpdate={nextPhysicsFrameWholeUpdate} dispatchDisabledStopsTicks={dispatchDisabledStopsTicks} batchExitFallback={batchExitFallback} directPhysicsRestored={directPhysicsRestored} reparentPreserved={reparentPreserved} zombieReparentRegistered={zombieReparentRegistered} pausedZombieStopsTicks={flag} zombieProcessCallbacks={zombieProcessCallbacks} zombiePhysicsCallbacks={zombiePhysicsCallbacks}");
		TowerDefenseCharacter.UseCharacterBatch = previousCharacterBatch;
		TowerDefenseZombie.UseBatch = previousZombieBatch;
		character.QueueFree();
		zombie.QueueFree();
		eventBus.QueueFree();
		GD.Print(flag2 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=CharacterScheduling");
		GetTree().Quit((!flag2) ? 2 : 0);
	}

	private void RunComponentOwnerScenario()
	{
		RunComponentOwnerScenarioAsync();
	}

	private async void RunComponentOwnerScenarioAsync()
	{
		bool passed = false;
		try
		{
			passed = await ExecuteComponentOwnerScenarioAsync();
		}
		catch (Exception value)
		{
			GD.PushError($"Component-owned state machine probe failed: {value}");
		}
		finally
		{
			GD.Print(passed ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=ComponentOwner");
			GetTree().Quit((!passed) ? 2 : 0);
		}
	}

	private async Task<bool> ExecuteComponentOwnerScenarioAsync()
	{
		int baselineCacheReferences = StateMachineProgramCache.GetStats().TotalReferences;
		ComponentStateMachineBareCharacter owner = new ComponentStateMachineBareCharacter
		{
			Name = "ComponentOwnerProbe",
			inGame = false,
			ProcessMode = ProcessModeEnum.Disabled
		};
		ComponentManager manager = new ComponentManager
		{
			Name = "ComponentManager"
		};
		ComponentBase first = new ComponentBase
		{
			Name = "FirstStatefulComponent"
		};
		ComponentBase second = new ComponentBase
		{
			Name = "SecondStatefulComponent"
		};
		ComponentBase lateBound = new ComponentBase
		{
			Name = "LateBoundStatefulComponent"
		};
		StateMachineDefinition definition = (second.StateMachineDefinition = (first.StateMachineDefinition = CreateRuntimeDefinition()));
		IStateMachineController firstController = null;
		IStateMachineController secondController = null;
		IStateMachineController lateBoundController = null;
		IStateMachineController queuedBeforeAttachController = null;
		bool initialized = false;
		bool queuedInitialEffectsSuppressed = false;
		bool programShared = false;
		bool lateDefinitionAttached = false;
		bool stateIndependent = false;
		bool alivePaused = false;
		bool detachedPreserved = false;
		bool wireSlotsStable = false;
		bool wireRegistrationIsolated = false;
		bool resumedRemainingDelay = false;
		bool authoritativeSnapshotApplied = false;
		bool equalRevisionDelayUpdated = false;
		bool staleRemoteRejected = false;
		bool detachedSnapshotQueued = false;
		try
		{
			owner.componentManager = manager;
			manager.AttachOwner(owner);
			AddChild(owner, forceReadableName: false, InternalMode.Disabled);
			manager.AddChild(first);
			manager.AddChild(second);
			manager.AddChild(lateBound);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			firstController = first.StateMachine;
			secondController = second.StateMachine;
			bool flag = lateBound.StateMachine == null;
			lateBound.StateMachineDefinition = definition;
			lateBoundController = lateBound.StateMachine;
			StateMachineController stateMachineController = firstController as StateMachineController;
			StateMachineController stateMachineController2 = secondController as StateMachineController;
			StateMachineController stateMachineController3 = lateBoundController as StateMachineController;
			initialized = (firstController?.IsInitialized ?? false) && (secondController?.IsInitialized ?? false) && firstController.CurrentStateHandle?.StableId == "Idle" && secondController.CurrentStateHandle?.StableId == "Idle";
			lateDefinitionAttached = flag && (lateBoundController?.IsInitialized ?? false) && lateBoundController.CurrentStateHandle?.StableId == "Idle";
			programShared = stateMachineController?.Runtime?.Program == stateMachineController2?.Runtime?.Program && stateMachineController?.Runtime?.Program == stateMachineController3?.Runtime?.Program && StateMachineProgramCache.GetStats().TotalReferences - baselineCacheReferences == 3;
			ComponentBase wireCollisionNode = new ComponentBase
			{
				Name = "WireCollisionNode",
				StateMachineDefinition = definition
			};
			manager.AddChild(wireCollisionNode);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			bool nodeRememberedSlot = manager.TryGetWireKey(wireCollisionNode, out var wireKey) && wireKey == "ComponentBase#3";
			manager.RemoveChild(wireCollisionNode);
			EffectCreateComponentDefinition definition2 = new EffectCreateComponentDefinition
			{
				ComponentTypeId = "ComponentBase",
				DefinitionId = "probe.component-owner.wire-collision",
				InstanceId = "probe.component-owner.wire-collision",
				WireIndex = 3,
				StateMachineDefinition = definition
			};
			CharacterComponentRuntime wireCollisionRuntime = manager.AddRuntimeComponent(definition2);
			string runtimeCollisionKey = string.Empty;
			bool runtimeOwnsRememberedSlot = wireCollisionRuntime != null && manager.TryGetWireKey(wireCollisionRuntime, out runtimeCollisionKey) && runtimeCollisionKey == wireKey;
			manager.AddChild(wireCollisionNode);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			string wireKey2 = string.Empty;
			bool flag2 = manager.TryGetWireKey(wireCollisionNode, out wireKey2) && wireKey2 == "ComponentBase#4";
			wireRegistrationIsolated = (nodeRememberedSlot & runtimeOwnsRememberedSlot & flag2) && manager.TryGetRuntimeByWireKey(runtimeCollisionKey, out var runtime) && runtime == wireCollisionRuntime && !manager.TryGetComponentByWireKey(runtimeCollisionKey, out var _) && manager.TryGetComponentByWireKey(wireKey2, out var component2) && component2 == wireCollisionNode && !manager.TryGetRuntimeByWireKey(wireKey2, out var _);
			bool firstKeyAssigned = manager.TryGetWireKey(first, out var wireKey3) && wireKey3 == "ComponentBase";
			bool secondKeyAssigned = manager.TryGetWireKey(second, out var wireKey4) && wireKey4 == "ComponentBase#1";
			bool lateBoundKeyAssigned = manager.TryGetWireKey(lateBound, out var wireKey5) && wireKey5 == "ComponentBase#2";
			StateMachineSnapshot snapshot = firstController?.CaptureSnapshot();
			bool flag3 = first.SendStateEvent("ToAttack");
			manager.TickStateMachineProcess(0.02);
			StateMachineSnapshot stateMachineSnapshot = firstController?.CaptureSnapshot();
			double remainingBeforePause = ((stateMachineSnapshot != null && stateMachineSnapshot.PendingDelayRemaining.ContainsKey("AttackToRecover")) ? stateMachineSnapshot.PendingDelayRemaining["AttackToRecover"] : (-1.0));
			stateIndependent = flag3 && firstController?.CurrentStateHandle?.StableId == "Attack" && secondController?.CurrentStateHandle?.StableId == "Idle" && firstController.Revision > secondController.Revision && remainingBeforePause > 0.0;
			Dictionary data = first.CaptureStateMachineSnapshotData();
			authoritativeSnapshotApplied = ComponentBase.TryDecodeStateMachineSnapshot(data, out var authoritativeSnapshot) && lateBound.RestoreStateMachineSnapshot(authoritativeSnapshot, remote: true, suppressEntryEffects: true) && lateBoundController.CurrentStateHandle?.StableId == "Attack";
			StateMachineSnapshot stateMachineSnapshot2 = StateMachineSnapshotCodec.Decode(StateMachineSnapshotCodec.Encode(authoritativeSnapshot));
			double num = remainingBeforePause * 0.5;
			stateMachineSnapshot2.PendingDelayRemaining["AttackToRecover"] = num;
			equalRevisionDelayUpdated = lateBound.RestoreStateMachineSnapshot(stateMachineSnapshot2, remote: true, suppressEntryEffects: true) && Math.Abs(lateBoundController.CaptureSnapshot().PendingDelayRemaining["AttackToRecover"] - num) < 1E-06;
			staleRemoteRejected = lateBound.RestoreStateMachineSnapshot(snapshot, remote: true, suppressEntryEffects: true) && lateBoundController.CurrentStateHandle?.StableId == "Attack";
			manager.RemoveChild(lateBound);
			StateMachineSnapshot stateMachineSnapshot3 = StateMachineSnapshotCodec.Decode(StateMachineSnapshotCodec.Encode(stateMachineSnapshot2));
			double detachedDelay = remainingBeforePause * 0.25;
			stateMachineSnapshot3.PendingDelayRemaining["AttackToRecover"] = detachedDelay;
			bool queuedWhileDetached = lateBound.RestoreStateMachineSnapshot(stateMachineSnapshot3, remote: true, suppressEntryEffects: true);
			manager.AddChild(lateBound);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			detachedSnapshotQueued = queuedWhileDetached && lateBoundController.CurrentStateHandle?.StableId == "Attack" && Math.Abs(lateBoundController.CaptureSnapshot().PendingDelayRemaining["AttackToRecover"] - detachedDelay) < 1E-06;
			ComponentBase componentBase = new ComponentBase
			{
				Name = "QueuedBeforeAttachComponent",
				StateMachineDefinition = definition
			};
			StateMachineSnapshot snapshot2 = StateMachineSnapshotCodec.Decode(StateMachineSnapshotCodec.Encode(authoritativeSnapshot));
			bool queuedBeforeTree = componentBase.RestoreStateMachineSnapshot(snapshot2, remote: true, suppressEntryEffects: true);
			queuedBeforeAttachController = componentBase.StateMachine;
			int queuedEntryCallbacks = 0;
			queuedBeforeAttachController.GetStateById("Idle").Entered += () =>
			{
				queuedEntryCallbacks++;
			};
			queuedBeforeAttachController.GetStateById("Attack").Entered += () =>
			{
				queuedEntryCallbacks++;
			};
			manager.AddChild(componentBase);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			queuedInitialEffectsSuppressed = queuedBeforeTree && queuedBeforeAttachController.CurrentStateHandle?.StableId == "Attack" && queuedEntryCallbacks == 0;
			first.SetAlive(_alive: false);
			manager.TickStateMachineProcess(0.2);
			StateMachineSnapshot stateMachineSnapshot4 = firstController?.CaptureSnapshot();
			double num2 = ((stateMachineSnapshot4 != null && stateMachineSnapshot4.PendingDelayRemaining.ContainsKey("AttackToRecover")) ? stateMachineSnapshot4.PendingDelayRemaining["AttackToRecover"] : (-1.0));
			alivePaused = firstController?.CurrentStateHandle?.StableId == "Attack" && Math.Abs(num2 - remainingBeforePause) < 1E-06;
			manager.RemoveChild(first);
			bool flag4 = !first.SendStateEvent("ToIdle");
			bool firstSlotInactive = !manager.TryGetWireKey(first, out var _);
			bool secondSlotUnshifted = manager.TryGetWireKey(second, out var wireKey7) && wireKey7 == "ComponentBase#1" && manager.TryGetComponentByWireKey(wireKey7, out var component3) && component3 == second;
			detachedPreserved = flag4 && firstController == first.StateMachine && (firstController?.IsInitialized ?? false) && firstController.CurrentStateHandle?.StableId == "Attack";
			manager.AddChild(first);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			StateMachineSnapshot stateMachineSnapshot5 = firstController?.CaptureSnapshot();
			double num3 = ((stateMachineSnapshot5 != null && stateMachineSnapshot5.PendingDelayRemaining.ContainsKey("AttackToRecover")) ? stateMachineSnapshot5.PendingDelayRemaining["AttackToRecover"] : (-1.0));
			wireSlotsStable = (firstKeyAssigned & secondKeyAssigned & lateBoundKeyAssigned & firstSlotInactive & secondSlotUnshifted) && manager.TryGetWireKey(first, out var wireKey8) && wireKey8 == "ComponentBase";
			detachedPreserved &= firstController == first.StateMachine && Math.Abs(num3 - remainingBeforePause) < 1E-06;
			first.SetAlive(_alive: true);
			manager.TickStateMachineProcess(Math.Max(0.0, num3 - 0.005));
			bool flag5 = firstController?.CurrentStateHandle?.StableId == "Attack";
			manager.TickStateMachineProcess(0.01);
			resumedRemainingDelay = flag5 && firstController?.CurrentStateHandle?.StableId == "Recover";
		}
		finally
		{
			if (GodotObject.IsInstanceValid(owner))
			{
				owner.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			StateMachineProgramCache.ClearUnused();
		}
		IStateMachineController stateMachineController4 = firstController;
		int num4;
		if (stateMachineController4 != null && !stateMachineController4.IsInitialized)
		{
			IStateMachineController stateMachineController5 = secondController;
			if (stateMachineController5 != null && !stateMachineController5.IsInitialized)
			{
				IStateMachineController stateMachineController6 = lateBoundController;
				if (stateMachineController6 != null && !stateMachineController6.IsInitialized)
				{
					IStateMachineController stateMachineController7 = queuedBeforeAttachController;
					if (stateMachineController7 != null && !stateMachineController7.IsInitialized)
					{
						num4 = ((StateMachineProgramCache.GetStats().TotalReferences == baselineCacheReferences) ? 1 : 0);
						goto IL_1424;
					}
				}
			}
		}
		num4 = 0;
		goto IL_1424;
		IL_1424:
		bool flag6 = (byte)num4 != 0;
		GD.Print($"STATE_MACHINE_COMPONENT_OWNER initialized={initialized} programShared={programShared} lateDefinitionAttached={lateDefinitionAttached} stateIndependent={stateIndependent} alivePaused={alivePaused} detachedPreserved={detachedPreserved} wireSlotsStable={wireSlotsStable} wireRegistrationIsolated={wireRegistrationIsolated} resumedRemainingDelay={resumedRemainingDelay} authoritativeSnapshotApplied={authoritativeSnapshotApplied} equalRevisionDelayUpdated={equalRevisionDelayUpdated} staleRemoteRejected={staleRemoteRejected} detachedSnapshotQueued={detachedSnapshotQueued} queuedInitialEffectsSuppressed={queuedInitialEffectsSuppressed} lifecycleReleased={flag6}");
		return initialized & programShared & lateDefinitionAttached & stateIndependent & alivePaused & detachedPreserved & wireSlotsStable & wireRegistrationIsolated & resumedRemainingDelay & authoritativeSnapshotApplied & equalRevisionDelayUpdated & staleRemoteRejected & detachedSnapshotQueued & queuedInitialEffectsSuppressed & flag6;
	}

	private void RunResourceComponentOwnerScenario()
	{
		RunResourceComponentOwnerScenarioAsync();
	}

	private async void RunResourceComponentOwnerScenarioAsync()
	{
		bool passed = false;
		try
		{
			passed = await ExecuteResourceComponentOwnerScenarioAsync();
		}
		catch (Exception value)
		{
			GD.PushError($"Resource-component state machine probe failed: {value}");
		}
		finally
		{
			GD.Print(passed ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=ResourceComponentOwner");
			GetTree().Quit((!passed) ? 2 : 0);
		}
	}

	private async Task<bool> ExecuteResourceComponentOwnerScenarioAsync()
	{
		int baselineCacheReferences = StateMachineProgramCache.GetStats().TotalReferences;
		StateMachineDefinition stateMachineDefinition = CreateRuntimeDefinition();
		EffectCreateComponentDefinition item = new EffectCreateComponentDefinition
		{
			ComponentTypeId = "EffectCreateComponent",
			DefinitionId = "probe.resource.effect.first",
			InstanceId = "probe.resource.effect.first",
			WireIndex = 0,
			StateMachineDefinition = stateMachineDefinition
		};
		EffectCreateComponentDefinition item2 = new EffectCreateComponentDefinition
		{
			ComponentTypeId = "EffectCreateComponent",
			DefinitionId = "probe.resource.effect.second",
			InstanceId = "probe.resource.effect.second",
			WireIndex = 1,
			StateMachineDefinition = stateMachineDefinition
		};
		CharacterComponentSet characterComponentSet = new CharacterComponentSet();
		characterComponentSet.Components.Add(item);
		characterComponentSet.Components.Add(item2);
		TowerDefenseCharacter owner = CreateCharacterSchedulingOwner<TowerDefenseCharacter>("ResourceComponentOwnerProbe", CreateCharacterSchedulingDefinition("probe.resource.owner", 0.2));
		owner.EnableComponentGameplayUntilBattlefieldEntry();
		ComponentManager manager = owner.componentManager;
		owner.ComponentSet = characterComponentSet;
		manager.ComponentSet = characterComponentSet;
		EffectCreateComponent first = null;
		EffectCreateComponent second = null;
		EffectCreateComponent replacement = null;
		bool initialized = false;
		bool queuedInitialEffectsSuppressed = false;
		bool stateIndependent = false;
		bool processMembershipUpdated = false;
		bool authoritativeSnapshotApplied = false;
		bool alivePaused = false;
		bool reentryPreserved = false;
		bool facadesRebound = false;
		bool flag;
		try
		{
			AddChild(owner, forceReadableName: false, InternalMode.Disabled);
			first = ((manager.ResourceComponents.Count > 0) ? (manager.ResourceComponents[0] as EffectCreateComponent) : null);
			second = ((manager.ResourceComponents.Count > 1) ? (manager.ResourceComponents[1] as EffectCreateComponent) : null);
			int queuedEntryCallbacks = 0;
			StateHandle stateHandle = second?.StateMachine?.GetStateById("Idle");
			StateHandle stateHandle2 = second?.StateMachine?.GetStateById("Attack");
			if (stateHandle != null)
			{
				stateHandle.Entered += () =>
				{
					queuedEntryCallbacks++;
				};
			}
			if (stateHandle2 != null)
			{
				stateHandle2.Entered += () =>
				{
					queuedEntryCallbacks++;
				};
			}
			using (StateMachineController stateMachineController = new StateMachineController())
			{
				stateMachineController.Initialize(stateMachineDefinition);
				stateMachineController.EnterInitialState();
				stateMachineController.SendEvent("ToAttack");
				second?.ApplyAuthoritativeSync(new Dictionary { ["sm"] = StateMachineSnapshotCodec.Encode(stateMachineController.CaptureSnapshot()) });
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			IStateMachineController firstController = first?.StateMachine;
			initialized = firstController?.CurrentStateHandle?.StableId == "Idle" && second?.StateMachine?.CurrentStateHandle?.StableId == "Attack" && manager.HasStateMachinePhysicsWork && manager.HasStateMachineProcessWork;
			queuedInitialEffectsSuppressed = queuedEntryCallbacks == 0;
			long num = second?.StateMachine?.Revision ?? (-1);
			processMembershipUpdated = (first?.SendStateEvent("ToAttack") ?? false) && manager.HasStateMachineProcessWork && firstController?.CurrentStateHandle?.StableId == "Attack";
			stateIndependent = second?.StateMachine?.CurrentStateHandle?.StableId == "Attack" && second.StateMachine.Revision == num;
			Dictionary data = new Dictionary { ["sm"] = first.CaptureStateMachineSnapshotData() };
			second.ApplyAuthoritativeSync(data);
			authoritativeSnapshotApplied = second.StateMachine?.CurrentStateHandle?.StableId == "Attack";
			second.SetAlive(alive: false);
			StateMachineSnapshot stateMachineSnapshot = firstController?.CaptureSnapshot();
			first.SetAlive(alive: false);
			manager.TickStateMachineProcess(0.1);
			StateMachineSnapshot stateMachineSnapshot2 = firstController?.CaptureSnapshot();
			alivePaused = !manager.HasStateMachineProcessWork && !manager.HasStateMachinePhysicsWork && stateMachineSnapshot != null && stateMachineSnapshot.PendingDelayRemaining.ContainsKey("AttackToRecover") && stateMachineSnapshot2 != null && stateMachineSnapshot2.PendingDelayRemaining.ContainsKey("AttackToRecover") && Math.Abs(stateMachineSnapshot.PendingDelayRemaining["AttackToRecover"] - stateMachineSnapshot2.PendingDelayRemaining["AttackToRecover"]) < 1E-06;
			first.SetAlive(alive: true);
			second.SetAlive(alive: true);
			manager.DetachOwner();
			bool detachedEventRejected = !first.SendStateEvent("ToIdle");
			manager.AttachOwner(owner);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			reentryPreserved = detachedEventRejected && first.Lifecycle == ComponentRuntimeLifecycle.Active && firstController == first.StateMachine && first.StateMachine.CurrentStateHandle?.StableId == "Attack";
			EffectCreateComponentDefinition item3 = new EffectCreateComponentDefinition
			{
				ComponentTypeId = "EffectCreateComponent",
				DefinitionId = "probe.resource.effect.replacement",
				InstanceId = "probe.resource.effect.first",
				WireIndex = 0
			};
			CharacterComponentSet characterComponentSet2 = new CharacterComponentSet();
			characterComponentSet2.Components.Add(item3);
			manager.ComponentSet = characterComponentSet2;
			manager.InitializeResourceComponents();
			manager.ActivateResourceComponents();
			replacement = manager.GetRuntime<EffectCreateComponent>();
			int num2;
			if (first.IsReleased && second.IsReleased)
			{
				EffectCreateComponent effectCreateComponent = replacement;
				if (effectCreateComponent != null && effectCreateComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
				{
					num2 = ((owner.effectCreateComponent == replacement) ? 1 : 0);
					goto IL_0925;
				}
			}
			num2 = 0;
			goto IL_0925;
			IL_0925:
			facadesRebound = (byte)num2 != 0;
		}
		finally
		{
			owner.QueueFree();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			flag = (first?.IsReleased ?? false) && (second?.IsReleased ?? false) && (replacement?.IsReleased ?? false) && StateMachineProgramCache.GetStats().TotalReferences == baselineCacheReferences;
		}
		GD.Print($"STATE_MACHINE_RESOURCE_COMPONENT_OWNER initialized={initialized} queuedInitialEffectsSuppressed={queuedInitialEffectsSuppressed} stateIndependent={stateIndependent} processMembershipUpdated={processMembershipUpdated} authoritativeSnapshotApplied={authoritativeSnapshotApplied} alivePaused={alivePaused} reentryPreserved={reentryPreserved} facadesRebound={facadesRebound} lifecycleReleased={flag}");
		return initialized & queuedInitialEffectsSuppressed & stateIndependent & processMembershipUpdated & authoritativeSnapshotApplied & alivePaused & reentryPreserved & facadesRebound & flag;
	}

	private void RunBaseCharacterScenario()
	{
		bool flag = false;
		try
		{
			flag = ExecuteBaseCharacterScenario();
		}
		catch (Exception value)
		{
			GD.PushError($"BaseCharacter state machine probe failed: {value}");
		}
		finally
		{
			GD.Print(flag ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=BaseCharacter");
			GetTree().Quit((!flag) ? 2 : 0);
		}
	}

	private bool ExecuteBaseCharacterScenario()
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/Character/TowerDefenseCharacter.tscn", null, ResourceLoader.CacheMode.Reuse);
		PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://Prefab/TowerDefense/Character/TowerDefenseZombie.tscn", null, ResourceLoader.CacheMode.Reuse);
		AttackComponentDefinition attackComponentDefinition = ResourceLoader.Load<AttackComponentDefinition>("res://Script/Component/TowerDefense/Character/AttackComponent/AttackComponentDefinition.tres", null, ResourceLoader.CacheMode.Reuse);
		if (packedScene == null || packedScene2 == null || attackComponentDefinition == null)
		{
			GD.Print("STATE_MACHINE_BASE_CHARACTER resourcesBound=False");
			return false;
		}
		Node node = null;
		Node node2 = null;
		StateMachineDefinition stateMachineDefinition = null;
		StateMachineDefinition stateMachineDefinition2 = null;
		StateMachineDefinition stateMachineDefinition3 = null;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag;
		try
		{
			node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			node2 = packedScene2.Instantiate(PackedScene.GenEditState.Disabled);
			TowerDefenseCharacter obj = node as TowerDefenseCharacter;
			TowerDefenseZombie towerDefenseZombie = node2 as TowerDefenseZombie;
			StateChart root = obj?.GetNodeOrNull<StateChart>("StateChart");
			StateChart root2 = towerDefenseZombie?.GetNodeOrNull<StateChart>("StateChart");
			num = CountNodeBranch(root);
			num2 = CountNodeBranch(root2);
			stateMachineDefinition = obj?.MainStateMachineDefinition;
			stateMachineDefinition2 = towerDefenseZombie?.MainStateMachineDefinition;
			stateMachineDefinition3 = attackComponentDefinition.StateMachineDefinition;
			flag = obj != null && towerDefenseZombie != null && num == 0 && num2 == 0 && stateMachineDefinition?.DefinitionId == "builtin.character.base" && stateMachineDefinition2?.DefinitionId == "builtin.character.zombie" && stateMachineDefinition3?.DefinitionId == "builtin.component.attack";
		}
		finally
		{
			node?.Free();
			node2?.Free();
		}
		int num4 = num + num2 + num3;
		double num5 = 100.0 * (double)(30 - num4) / 30.0;
		int totalReferences = StateMachineProgramCache.GetStats().TotalReferences;
		int registrationCount = StateMachineRuntimeBatch.RegistrationCount;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		StateMachineController stateMachineController = new StateMachineController();
		StateMachineController stateMachineController2 = new StateMachineController();
		StateMachineController stateMachineController3 = new StateMachineController();
		try
		{
			if (stateMachineController.Initialize(stateMachineDefinition) && stateMachineController2.Initialize(stateMachineDefinition2) && stateMachineController3.Initialize(stateMachineDefinition3) && stateMachineController.EnterInitialState() && stateMachineController2.EnterInitialState() && stateMachineController3.EnterInitialState())
			{
				StateHandle stateById = stateMachineController.GetStateById("character.idle");
				StateHandle stateById2 = stateMachineController.GetStateById("character.sleep");
				StateHandle stateById3 = stateMachineController.GetStateById("character.component");
				int characterTransitions = 0;
				int characterEvents = 0;
				stateMachineController.EventReceived += (StringName _) =>
				{
					characterEvents++;
				};
				stateById2.Entered += () =>
				{
					characterTransitions++;
				};
				stateById3.Entered += () =>
				{
					characterTransitions++;
				};
				stateById.Entered += () =>
				{
					characterTransitions++;
				};
				bool flag6 = stateMachineController.SendEvent("ToSleep") && stateMachineController.SendEvent("ToComponent") && stateMachineController.SendEvent("ToIdle");
				StateHandle stateById4 = stateMachineController2.GetStateById("zombie.walk");
				StateHandle stateById5 = stateMachineController2.GetStateById("zombie.attack");
				StateHandle stateById6 = stateMachineController2.GetStateById("zombie.garlic");
				StateHandle stateById7 = stateMachineController2.GetStateById("zombie.die");
				int zombieTransitions = 0;
				int zombieEvents = 0;
				stateMachineController2.EventReceived += (StringName _) =>
				{
					zombieEvents++;
				};
				stateById4.Entered += () =>
				{
					zombieTransitions++;
				};
				stateById5.Entered += () =>
				{
					zombieTransitions++;
				};
				stateById6.Entered += () =>
				{
					zombieTransitions++;
				};
				stateById7.Entered += () =>
				{
					zombieTransitions++;
				};
				bool flag7 = stateMachineController2.SendEvent("ToWalk") && stateMachineController2.SendEvent("ToAttack") && stateMachineController2.SendEvent("ToGarlic") && stateMachineController2.SendEvent("ToDie");
				StateHandle stateById8 = stateMachineController3.GetStateById("attack.idle");
				StateHandle stateById9 = stateMachineController3.GetStateById("attack.attack");
				int attackTransitions = 0;
				int attackEvents = 0;
				stateMachineController3.EventReceived += (StringName _) =>
				{
					attackEvents++;
				};
				stateById9.Entered += () =>
				{
					attackTransitions++;
				};
				stateById8.Entered += () =>
				{
					attackTransitions++;
				};
				bool flag8 = stateMachineController3.SendEvent("ToAttack") && stateMachineController3.SendEvent("ToIdle");
				flag2 = characterTransitions == 3 && stateById.IsActive && zombieTransitions == 4 && stateById7.IsActive && attackTransitions == 2 && stateById8.IsActive;
				flag3 = (flag6 & flag7 & flag8) && characterEvents == 3 && zombieEvents == 4 && attackEvents == 2;
				int physicsCallbacks = 0;
				stateById.PhysicsProcessing += (double _) =>
				{
					physicsCallbacks++;
				};
				for (int num6 = 0; num6 < 3; num6++)
				{
					stateMachineController.TickPhysics(1.0 / 60.0);
				}
				flag4 = physicsCallbacks == 3;
			}
			flag5 = StateMachineRuntimeBatch.RegistrationCount == registrationCount && StateMachineProgramCache.GetStats().TotalReferences - totalReferences == 3;
		}
		finally
		{
			stateMachineController.Dispose();
			stateMachineController2.Dispose();
			stateMachineController3.Dispose();
		}
		bool flag9 = StateMachineRuntimeBatch.RegistrationCount == registrationCount && StateMachineProgramCache.GetStats().TotalReferences == totalReferences;
		List<StateMachineController> list = new List<StateMachineController>(_requestedSpawnCount);
		StateMachineProgram stateMachineProgram = null;
		bool flag10 = true;
		int num7 = 0;
		try
		{
			for (int num8 = 0; num8 < _requestedSpawnCount; num8++)
			{
				StateMachineController stateMachineController4 = new StateMachineController();
				list.Add(stateMachineController4);
				flag10 &= stateMachineController4.Initialize(stateMachineDefinition) && stateMachineController4.EnterInitialState() && (stateMachineController4.GetStateById("character.idle")?.IsActive ?? false);
				if (stateMachineProgram == null)
				{
					stateMachineProgram = stateMachineController4.Runtime?.Program;
				}
				flag10 &= stateMachineProgram == stateMachineController4.Runtime?.Program;
			}
			num7 = StateMachineProgramCache.GetStats().TotalReferences - totalReferences;
			flag10 &= num7 == _requestedSpawnCount && StateMachineRuntimeBatch.RegistrationCount == registrationCount;
		}
		finally
		{
			for (int num9 = 0; num9 < list.Count; num9++)
			{
				list[num9].Dispose();
			}
		}
		StateMachineProgramCache.ClearUnused();
		int num10 = StateMachineProgramCache.GetStats().TotalReferences - totalReferences;
		int num11 = StateMachineRuntimeBatch.RegistrationCount - registrationCount;
		bool flag11 = flag9 && num10 == 0 && num11 == 0;
		bool flag12 = num == 0 && num2 == 0 && num3 == 0 && num5 >= 100.0;
		GD.Print($"STATE_MACHINE_BASE_CHARACTER resourcesBound={flag} gameplayTrace={flag2} eventConversion={flag3} physicsTick={flag4} ownerSchedulingProbe=CharacterScheduling gameplayControllersPure={flag5} characterStateChartNodeCount={num} zombieStateChartNodeCount={num2} attackStateChartNodeCount={num3} stateChartNodeCount={num4} legacyStateChartNodeCount={30} nodeReductionPercent={num5:F2} requestedSpawnCount={_requestedSpawnCount} stressControllersPure={flag10} stressCacheReferences={num7} cacheActiveReferences={num10} runtimeRegistrations={num11}");
		return flag & flag2 & flag3 & flag4 & flag5 & flag12 & flag10 & flag11;
	}

	private static T CreateCharacterSchedulingOwner<T>(string name, StateMachineDefinition definition) where T : TowerDefenseCharacter, new()
	{
		T val = new T
		{
			Name = name,
			config = new TowerDefenseCharacterConfig
			{
				name = name
			},
			MainStateMachineDefinition = definition,
			HitBoxDefinition = new CharacterHitBoxDefinition
			{
				Size = new Vector2(40f, 80f)
			},
			inGame = true,
			editorPreviewMode = true
		};
		AddCharacterProbeNode(val, new Node2D(), "BackEffectNode", unique: true);
		AddCharacterProbeNode(val, new Node2D(), "FrontEffectNode", unique: true);
		AddCharacterProbeNode(val, new Sprite2D(), "ShadowSprite", unique: true);
		Node2D node2D = new Node2D();
		AddCharacterProbeNode(val, node2D, "SpriteGroup", unique: true);
		AddCharacterProbeNode(val, new Marker2D(), "TransformPoint", unique: true, node2D);
		AdobeAnimateSprite adobeAnimateSprite = new AdobeAnimateSprite
		{
			Name = "ProbeSprite"
		};
		node2D.AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
		adobeAnimateSprite.Owner = val;
		val.sprite = adobeAnimateSprite;
		val.componentManager = new ComponentManager();
		val.componentManager.AttachOwner(val);
		return val;
	}

	private static void AddCharacterProbeNode(TowerDefenseCharacter owner, Node node, string name, bool unique, Node parent = null)
	{
		node.Name = name;
		node.UniqueNameInOwner = unique;
		(parent ?? owner).AddChild(node, forceReadableName: false, InternalMode.Disabled);
		node.Owner = owner;
	}

	private static StateMachineDefinition CreateCharacterSchedulingDefinition(string definitionId, double delaySeconds)
	{
		return new StateMachineDefinition
		{
			DefinitionId = definitionId,
			RootStateId = "probe.root",
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = "probe.root",
					DisplayName = "Root",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = "probe.idle"
				},
				new StateMachineStateDefinition
				{
					StableId = "probe.idle",
					DisplayName = "Idle",
					ParentId = "probe.root",
					ProcessFlags = (StateMachineProcessFlags.Process | StateMachineProcessFlags.PhysicsProcess)
				},
				new StateMachineStateDefinition
				{
					StableId = "probe.done",
					DisplayName = "Done",
					ParentId = "probe.root"
				}
			},
			Transitions = { CreateRuntimeTransition("probe.idle_to_done", "probe.idle", "probe.done", StateMachineTriggerKind.Automatic, string.Empty, delaySeconds, 0) }
		};
	}

	private static int CountNodeBranch(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return 0;
		}
		int num = 1;
		foreach (Node child in root.GetChildren())
		{
			num += CountNodeBranch(child);
		}
		return num;
	}

	private async void RunSnapshotScenarioAsync()
	{
		StateMachineDefinition definition = CreateSnapshotDefinition();
		bool compiled = StateMachineCompiler.TryCompile(definition, out var program, out var _);
		StateMachineRuntime runtime = new StateMachineRuntime();
		runtime.Initialize(program);
		StateHandle stateHandle = runtime.GetStateHandle("Attack");
		int attackEnteredCount = 0;
		stateHandle.Entered += () =>
		{
			attackEnteredCount++;
		};
		runtime.EnterInitialState();
		runtime.SetExpressionProperty("score", Variant.From<long>(42L));
		runtime.SetExpressionProperty("transient_resource", Variant.From<Resource>(new Resource()));
		runtime.SendEvent("ToAttack");
		runtime.TickProcess(0.04);
		StateMachineSnapshot stateMachineSnapshot = runtime.CaptureSnapshot();
		double num = stateMachineSnapshot.PendingDelayRemaining["AttackToRecover"];
		bool stableIdsCaptured = stateMachineSnapshot.ActiveStateIds.Contains("Root") && stateMachineSnapshot.ActiveStateIds.Contains("Attack") && stateMachineSnapshot.PendingTransitionIds.Contains("AttackToRecover");
		bool expressionFiltered = stateMachineSnapshot.ExpressionProperties.ContainsKey("score") && stateMachineSnapshot.ExpressionProperties["score"].AsInt64() == 42 && !stateMachineSnapshot.ExpressionProperties.ContainsKey("transient_resource");
		runtime.TickProcess(0.07);
		bool recoveredBeforeRestore = runtime.IsActive("Recover");
		bool pendingExact = runtime.RestoreSnapshot(stateMachineSnapshot, suppressEntryEffects: true) && runtime.IsActive("Attack") && Math.Abs(runtime.PendingDelayRemaining - num) < 1E-06;
		runtime.TickProcess(0.05);
		bool flag = runtime.IsActive("Attack");
		runtime.TickProcess(0.02);
		pendingExact &= flag && runtime.IsActive("Recover");
		bool suppressedEffects = attackEnteredCount == 1;
		StateMachineSnapshot stateMachineSnapshot2 = CreateSnapshotEnvelope(runtime.CaptureSnapshot());
		stateMachineSnapshot2.ActiveStateIds.Add("Root");
		stateMachineSnapshot2.ActiveStateIds.Add("IdleOld");
		bool aliasRestored = runtime.RestoreSnapshot(stateMachineSnapshot2, suppressEntryEffects: true) && runtime.IsActive("IdleRenamed");
		StateMachineSnapshot stateMachineSnapshot3 = CreateSnapshotEnvelope(runtime.CaptureSnapshot());
		stateMachineSnapshot3.ActiveStateIds.Add("Root");
		stateMachineSnapshot3.ActiveStateIds.Add("RemovedLeaf");
		aliasRestored &= runtime.RestoreSnapshot(stateMachineSnapshot3, suppressEntryEffects: true) && runtime.IsActive("IdleRenamed");
		StateMachineSnapshot stateMachineSnapshot4 = CreateSnapshotEnvelope(runtime.CaptureSnapshot());
		stateMachineSnapshot4.ContentHash = "mismatched-content-hash";
		stateMachineSnapshot4.ActiveStateIds.Add("Attack");
		long revision = runtime.Revision;
		bool hashRejected = !runtime.RestoreSnapshot(stateMachineSnapshot4, suppressEntryEffects: true) && runtime.IsActive("IdleRenamed") && runtime.Revision == revision && runtime.LastDiagnosticCode == "SMS001";
		StateMachineSnapshot stateMachineSnapshot5 = CreateSnapshotEnvelope(runtime.CaptureSnapshot());
		stateMachineSnapshot5.ActiveStateIds.Add("Root");
		stateMachineSnapshot5.ActiveStateIds.Add("Attack");
		stateMachineSnapshot5.Revision = runtime.Revision + 5;
		bool flag2 = runtime.ApplyRemoteSnapshot(stateMachineSnapshot5);
		long revision2 = runtime.Revision;
		bool flag3 = !runtime.ApplyRemoteSnapshot(stateMachineSnapshot5);
		bool newerRevisionOnly = (flag2 & flag3) && revision2 == stateMachineSnapshot5.Revision && runtime.IsActive("Attack");
		bool automaticSuppressed = RunSnapshotAutomaticFallbackScenario();
		bool invalidPendingRejected = RunSnapshotInvalidPendingScenario(program);
		bool restoreExceptionAtomic = RunSnapshotRestoreExceptionScenario(program);
		bool restoreDisposeSafe = RunSnapshotRestoreDisposeScenario(program);
		bool eventPendingRestored = RunSnapshotEventDelayScenario(out var snapshotRevisionDirty);
		int baselineCacheReferences = StateMachineProgramCache.GetStats().TotalReferences;
		int baselineRuntimeRegistrations = StateMachineRuntimeBatch.RegistrationCount;
		StateChart adapterChart = new StateChart
		{
			Name = "SnapshotAdapterChart",
			RuntimeMode = StateMachineRuntimeMode.Resource,
			Definition = definition
		};
		AddChild(adapterChart, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		adapterChart.SetExpressionProperty("score", Variant.From<long>(7L));
		StateMachineSnapshot stateMachineSnapshot6 = adapterChart.CaptureResourceSnapshot();
		bool hostAdapter = stateMachineSnapshot6 != null && adapterChart.StateRevision == stateMachineSnapshot6.Revision && stateMachineSnapshot6.ExpressionProperties["score"].AsInt64() == 7;
		StateMachineDefinition definition2 = CreateValidDefinition();
		List<StateChart> stressHosts = new List<StateChart>(_requestedSpawnCount);
		for (int num2 = 0; num2 < _requestedSpawnCount; num2++)
		{
			StateChart stateChart = new StateChart
			{
				Name = $"SnapshotStress{num2}",
				RuntimeMode = StateMachineRuntimeMode.Resource,
				Definition = definition2
			};
			stressHosts.Add(stateChart);
			AddChild(stateChart, forceReadableName: false, InternalMode.Disabled);
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		for (int num3 = 0; num3 < stressHosts.Count; num3++)
		{
			stressHosts[num3].QueueFree();
		}
		adapterChart.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		StateMachineProgramCache.ClearUnused();
		int num4 = StateMachineProgramCache.GetStats().TotalReferences - baselineCacheReferences;
		int num5 = StateMachineRuntimeBatch.RegistrationCount - baselineRuntimeRegistrations;
		bool flag4 = num4 == 0 && num5 == 0;
		bool flag5 = compiled & stableIdsCaptured & recoveredBeforeRestore & hostAdapter;
		GD.Print($"STATE_MACHINE_SNAPSHOT_RESOURCE snapshotPassed={flag5} aliasRestored={aliasRestored} pendingExact={pendingExact} suppressedEffects={suppressedEffects} hashRejected={hashRejected} newerRevisionOnly={newerRevisionOnly} expressionFiltered={expressionFiltered} automaticSuppressed={automaticSuppressed} invalidPendingRejected={invalidPendingRejected} restoreExceptionAtomic={restoreExceptionAtomic} restoreDisposeSafe={restoreDisposeSafe} eventPendingRestored={eventPendingRestored} snapshotRevisionDirty={snapshotRevisionDirty} cacheActiveReferences={num4} runtimeRegistrations={num5}");
		runtime.Dispose();
		bool flag6 = flag5 & aliasRestored & pendingExact & suppressedEffects & hashRejected & newerRevisionOnly & expressionFiltered & automaticSuppressed & invalidPendingRejected & restoreExceptionAtomic & restoreDisposeSafe & eventPendingRestored & snapshotRevisionDirty & flag4;
		GD.Print(flag6 ? "STATE_MACHINE_RESULT passed=True" : "STATE_MACHINE_RESULT passed=False mode=Snapshot");
		GetTree().Quit((!flag6) ? 2 : 0);
	}

	private static bool RunSnapshotAutomaticFallbackScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateSnapshotAutomaticDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		stateMachineRuntime.EnterInitialState();
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("Start");
		StateHandle stateHandle2 = stateMachineRuntime.GetStateHandle("Done");
		int stateCallbacks = 0;
		int transitionCallbacks = 0;
		stateHandle.Entered += () =>
		{
			stateCallbacks++;
		};
		stateHandle.Exited += () =>
		{
			stateCallbacks++;
		};
		stateHandle2.Exited += () =>
		{
			stateCallbacks++;
		};
		stateHandle2.Entered += () =>
		{
			stateCallbacks++;
		};
		stateMachineRuntime.TransitionTaken += (CompiledStateMachineTransition _) =>
		{
			transitionCallbacks++;
		};
		StateMachineSnapshot stateMachineSnapshot = CreateSnapshotEnvelope(stateMachineRuntime.CaptureSnapshot());
		stateMachineSnapshot.ActiveStateIds.Add("Root");
		stateMachineSnapshot.ActiveStateIds.Add("RemovedLeaf");
		stateMachineSnapshot.Revision = stateMachineRuntime.Revision + 5;
		bool result = stateMachineRuntime.ApplyRemoteSnapshot(stateMachineSnapshot) && stateMachineRuntime.IsActive("Done") && !stateMachineRuntime.IsActive("Start") && stateCallbacks == 0 && transitionCallbacks == 0 && stateMachineRuntime.Revision == stateMachineSnapshot.Revision;
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunSnapshotEventDelayScenario(out bool snapshotRevisionDirty)
	{
		snapshotRevisionDirty = false;
		if (!StateMachineCompiler.TryCompile(CreateEventDelaySnapshotDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		stateMachineRuntime.EnterInitialState();
		long snapshotRevision = stateMachineRuntime.SnapshotRevision;
		stateMachineRuntime.SendEvent("ToAttack");
		long snapshotRevision2 = stateMachineRuntime.SnapshotRevision;
		stateMachineRuntime.SetExpressionProperty("phase", Variant.From<long>(1L));
		long snapshotRevision3 = stateMachineRuntime.SnapshotRevision;
		stateMachineRuntime.SetExpressionProperty("phase", Variant.From<long>(1L));
		snapshotRevisionDirty = stateMachineRuntime.HasPendingTransition && snapshotRevision2 > snapshotRevision && snapshotRevision3 > snapshotRevision2 && stateMachineRuntime.SnapshotRevision == snapshotRevision3;
		stateMachineRuntime.TickProcess(0.04);
		StateMachineSnapshot stateMachineSnapshot = stateMachineRuntime.CaptureSnapshot();
		double num = stateMachineSnapshot.PendingDelayRemaining["IdleToAttackDelayed"];
		StateMachineRuntime stateMachineRuntime2 = new StateMachineRuntime();
		stateMachineRuntime2.Initialize(program);
		stateMachineRuntime2.EnterInitialState();
		bool num2 = stateMachineRuntime2.RestoreSnapshot(stateMachineSnapshot, suppressEntryEffects: true) && stateMachineRuntime2.IsActive("Idle") && stateMachineRuntime2.HasPendingTransition && Math.Abs(stateMachineRuntime2.PendingDelayRemaining - num) < 1E-06;
		stateMachineRuntime2.TickProcess(Math.Max(0.0, num - 0.005));
		bool flag = stateMachineRuntime2.IsActive("Idle");
		stateMachineRuntime2.TickProcess(0.01);
		bool flag2 = stateMachineRuntime2.IsActive("Attack") && !stateMachineRuntime2.HasPendingTransition;
		stateMachineRuntime.Dispose();
		stateMachineRuntime2.Dispose();
		return num2 & flag & flag2;
	}

	private static bool RunSnapshotInvalidPendingScenario(StateMachineProgram program)
	{
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		stateMachineRuntime.EnterInitialState();
		StateMachineSnapshot stateMachineSnapshot = CreateSnapshotEnvelope(stateMachineRuntime.CaptureSnapshot());
		stateMachineSnapshot.ActiveStateIds.Add("Root");
		stateMachineSnapshot.ActiveStateIds.Add("IdleRenamed");
		stateMachineSnapshot.PendingTransitionIds.Add("IdleToAttack");
		stateMachineSnapshot.PendingDelayRemaining["IdleToAttack"] = 0.5;
		bool num = stateMachineRuntime.RestoreSnapshot(stateMachineSnapshot, suppressEntryEffects: true) && stateMachineRuntime.IsActive("IdleRenamed") && stateMachineRuntime.PendingDelayRemaining == 0.0;
		StateMachineSnapshot stateMachineSnapshot2 = CreateSnapshotEnvelope(stateMachineRuntime.CaptureSnapshot());
		stateMachineSnapshot2.ActiveStateIds.Add("Root");
		stateMachineSnapshot2.ActiveStateIds.Add("Attack");
		stateMachineSnapshot2.PendingTransitionIds.Add("AttackToRecover");
		stateMachineSnapshot2.PendingDelayRemaining["AttackToRecover"] = 0.5;
		bool flag = stateMachineRuntime.RestoreSnapshot(stateMachineSnapshot2, suppressEntryEffects: true) && stateMachineRuntime.IsActive("Attack") && stateMachineRuntime.PendingDelayRemaining > 0.0 && stateMachineRuntime.PendingDelayRemaining <= 0.1000001;
		stateMachineRuntime.Dispose();
		return num & flag;
	}

	private static bool RunSnapshotRestoreExceptionScenario(StateMachineProgram program)
	{
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("IdleRenamed");
		StateHandle stateHandle2 = stateMachineRuntime.GetStateHandle("Attack");
		stateMachineRuntime.EnterInitialState();
		int throwingCallbacks = 0;
		stateHandle.Exited += () =>
		{
			throwingCallbacks++;
			throw new InvalidOperationException("snapshot exit callback failure");
		};
		stateHandle2.Entered += () =>
		{
			throwingCallbacks++;
			throw new InvalidOperationException("snapshot entry callback failure");
		};
		StateMachineSnapshot stateMachineSnapshot = CreateSnapshotEnvelope(stateMachineRuntime.CaptureSnapshot());
		stateMachineSnapshot.ActiveStateIds.Add("Root");
		stateMachineSnapshot.ActiveStateIds.Add("Attack");
		stateMachineSnapshot.Revision = stateMachineRuntime.Revision + 5;
		bool flag = false;
		try
		{
			stateMachineRuntime.RestoreSnapshot(stateMachineSnapshot);
		}
		catch (InvalidOperationException)
		{
			flag = true;
		}
		bool flag2 = stateMachineRuntime.SendEvent("Unknown");
		bool result = ((flag && throwingCallbacks == 2) & flag2) && stateMachineRuntime.IsActive("Attack") && stateMachineRuntime.Revision == stateMachineSnapshot.Revision + 1;
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunSnapshotRestoreDisposeScenario(StateMachineProgram program)
	{
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("IdleRenamed");
		StateHandle stateHandle2 = stateMachineRuntime.GetStateHandle("Attack");
		stateMachineRuntime.EnterInitialState();
		int attackEntered = 0;
		stateHandle.Exited += stateMachineRuntime.Dispose;
		stateHandle2.Entered += () =>
		{
			attackEntered++;
		};
		StateMachineSnapshot stateMachineSnapshot = CreateSnapshotEnvelope(stateMachineRuntime.CaptureSnapshot());
		stateMachineSnapshot.ActiveStateIds.Add("Root");
		stateMachineSnapshot.ActiveStateIds.Add("Attack");
		if (!stateMachineRuntime.RestoreSnapshot(stateMachineSnapshot) && attackEntered == 0 && !stateHandle.IsValid && !stateHandle2.IsValid)
		{
			return !stateMachineRuntime.SendEvent("Ignored");
		}
		return false;
	}

	private static StateMachineSnapshot CreateSnapshotEnvelope(StateMachineSnapshot source)
	{
		return new StateMachineSnapshot
		{
			DefinitionId = source.DefinitionId,
			SchemaVersion = source.SchemaVersion,
			ContentHash = source.ContentHash,
			Revision = source.Revision
		};
	}

	private void OnRuntimePhysicsProcessing(double delta)
	{
		_runtimePhysicsCallbacks++;
	}

	private static bool RunNestedRuntimeOrderScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateNestedRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("BranchA");
		StateHandle stateHandle2 = stateMachineRuntime.GetStateHandle("LeafA");
		StateHandle stateHandle3 = stateMachineRuntime.GetStateHandle("BranchB");
		StateHandle stateHandle4 = stateMachineRuntime.GetStateHandle("LeafB");
		List<string> trace = new List<string>(8);
		stateHandle2.Exited += () =>
		{
			trace.Add("exit:LeafA");
		};
		stateHandle.Exited += () =>
		{
			trace.Add("exit:BranchA");
		};
		stateHandle3.Entered += () =>
		{
			trace.Add("enter:BranchB");
		};
		stateHandle4.Entered += () =>
		{
			trace.Add("enter:LeafB");
		};
		stateMachineRuntime.EnterInitialState();
		trace.Clear();
		stateMachineRuntime.SendEvent("SwitchBranch");
		bool result = string.Join(',', trace) == "exit:LeafA,exit:BranchA,enter:BranchB,enter:LeafB" && stateMachineRuntime.IsActive("LeafB") && !stateMachineRuntime.IsActive("LeafA");
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunRuntimeExceptionRecoveryScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		stateMachineRuntime.EnterInitialState();
		bool shouldThrow = true;
		bool flag = false;
		stateMachineRuntime.EventReceived += (StringName eventName) =>
		{
			if (eventName != new StringName("Throw") || !shouldThrow)
			{
				return;
			}
			shouldThrow = false;
			throw new InvalidOperationException("runtime probe callback failure");
		};
		try
		{
			stateMachineRuntime.SendEvent("Throw");
		}
		catch (InvalidOperationException)
		{
			flag = true;
		}
		bool flag2 = stateMachineRuntime.SendEvent("ToAttack");
		bool result = (flag & flag2) && stateMachineRuntime.IsActive("Attack");
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunRuntimeCallbackAtomicScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("Idle");
		StateHandle stateHandle2 = stateMachineRuntime.GetStateHandle("Attack");
		stateMachineRuntime.EnterInitialState();
		int throwingCallbacks = 0;
		stateMachineRuntime.TransitionTaken += (CompiledStateMachineTransition transition) =>
		{
			if (transition.StableId != "IdleToAttack")
			{
				return;
			}
			throwingCallbacks++;
			throw new InvalidOperationException("transition callback failure");
		};
		stateHandle.Exited += () =>
		{
			throwingCallbacks++;
			throw new InvalidOperationException("exit callback failure");
		};
		stateHandle2.Entered += () =>
		{
			throwingCallbacks++;
			throw new InvalidOperationException("entry callback failure");
		};
		bool flag = false;
		try
		{
			stateMachineRuntime.SendEvent("ToAttack");
		}
		catch (InvalidOperationException)
		{
			flag = true;
		}
		bool flag2 = stateMachineRuntime.SendEvent("Unknown");
		bool result = ((flag && throwingCallbacks == 3) & flag2) && stateMachineRuntime.IsActive("Attack") && stateMachineRuntime.Revision == 1;
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunRuntimeDisposeCallbackScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime rootRuntime = new StateMachineRuntime();
		rootRuntime.Initialize(program);
		StateHandle root = rootRuntime.GetStateHandle("Root");
		StateHandle stateHandle = rootRuntime.GetStateHandle("Idle");
		int initialIdleEntered = 0;
		bool invalidImmediately = false;
		root.Entered += () =>
		{
			rootRuntime.Dispose();
			invalidImmediately = !root.IsValid && !rootRuntime.SendEvent("Ignored");
		};
		stateHandle.Entered += () =>
		{
			initialIdleEntered++;
		};
		rootRuntime.EnterInitialState();
		bool num = invalidImmediately && initialIdleEntered == 0 && !root.IsValid;
		StateMachineRuntime eventRuntime = new StateMachineRuntime();
		eventRuntime.Initialize(program);
		StateHandle stateHandle2 = eventRuntime.GetStateHandle("Idle");
		StateHandle stateHandle3 = eventRuntime.GetStateHandle("Attack");
		eventRuntime.EnterInitialState();
		int eventTransitionCallbacks = 0;
		int eventExitCallbacks = 0;
		int eventEntryCallbacks = 0;
		eventRuntime.EventReceived += (StringName eventName) =>
		{
			if (eventName == new StringName("ToAttack"))
			{
				eventRuntime.Dispose();
			}
		};
		eventRuntime.TransitionTaken += (CompiledStateMachineTransition _) =>
		{
			eventTransitionCallbacks++;
		};
		stateHandle2.Exited += () =>
		{
			eventExitCallbacks++;
		};
		stateHandle3.Entered += () =>
		{
			eventEntryCallbacks++;
		};
		eventRuntime.SendEvent("ToAttack");
		bool flag = eventTransitionCallbacks == 0 && eventExitCallbacks == 0 && eventEntryCallbacks == 0 && !stateHandle2.IsValid;
		StateMachineRuntime transitionRuntime = new StateMachineRuntime();
		transitionRuntime.Initialize(program);
		StateHandle stateHandle4 = transitionRuntime.GetStateHandle("Idle");
		StateHandle stateHandle5 = transitionRuntime.GetStateHandle("Attack");
		transitionRuntime.EnterInitialState();
		int transitionExitCallbacks = 0;
		int transitionEntryCallbacks = 0;
		transitionRuntime.TransitionTaken += (CompiledStateMachineTransition transition) =>
		{
			if (transition.StableId == "IdleToAttack")
			{
				transitionRuntime.Dispose();
			}
		};
		stateHandle4.Exited += () =>
		{
			transitionExitCallbacks++;
		};
		stateHandle5.Entered += () =>
		{
			transitionEntryCallbacks++;
		};
		transitionRuntime.SendEvent("ToAttack");
		bool flag2 = transitionExitCallbacks == 0 && transitionEntryCallbacks == 0 && !stateHandle4.IsValid;
		StateMachineRuntime exitRuntime = new StateMachineRuntime();
		exitRuntime.Initialize(program);
		StateHandle stateHandle6 = exitRuntime.GetStateHandle("Idle");
		StateHandle stateHandle7 = exitRuntime.GetStateHandle("Attack");
		exitRuntime.EnterInitialState();
		int exitCallbacks = 0;
		int entryAfterExit = 0;
		stateHandle6.Exited += () =>
		{
			exitCallbacks++;
			exitRuntime.Dispose();
		};
		stateHandle7.Entered += () =>
		{
			entryAfterExit++;
		};
		exitRuntime.SendEvent("ToAttack");
		bool flag3 = exitCallbacks == 1 && entryAfterExit == 0 && !stateHandle6.IsValid;
		rootRuntime.Dispose();
		eventRuntime.Dispose();
		transitionRuntime.Dispose();
		exitRuntime.Dispose();
		return num & flag & flag2 & flag3;
	}

	private static bool RunRuntimeReinitializeCallbackScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime runtime = new StateMachineRuntime();
		runtime.Initialize(program);
		StateHandle stateHandle = runtime.GetStateHandle("Idle");
		bool rejected = false;
		stateHandle.Entered += () =>
		{
			try
			{
				runtime.Initialize(program);
			}
			catch (InvalidOperationException)
			{
				rejected = true;
			}
		};
		runtime.EnterInitialState();
		bool result = rejected && stateHandle.IsValid && runtime.IsActive("Idle");
		runtime.Dispose();
		return result;
	}

	private static bool RunRuntimeQueueBudgetScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime runtime = new StateMachineRuntime();
		runtime.Initialize(program);
		StateHandle stateHandle = runtime.GetStateHandle("Idle");
		int rejectedCount = 0;
		int receivedCount = 0;
		bool diagnosed = false;
		runtime.EventReceived += (StringName _) =>
		{
			receivedCount++;
		};
		runtime.Diagnostic += (string code) =>
		{
			diagnosed |= code == "SMR001";
		};
		stateHandle.Entered += () =>
		{
			for (int i = 0; i < 1025; i++)
			{
				if (!runtime.SendEvent("Queued"))
				{
					rejectedCount++;
				}
			}
		};
		runtime.EnterInitialState();
		bool result = ((rejectedCount == 1 && receivedCount == 1024) & diagnosed) && runtime.LastDiagnosticCode == "SMR001";
		runtime.Dispose();
		return result;
	}

	private static bool RunRuntimeAutomaticBacklogScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeBudgetDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime runtime = new StateMachineRuntime();
		runtime.Initialize(program);
		StateHandle stateHandle = runtime.GetStateHandle("S0");
		bool allQueued = true;
		bool diagnosed = false;
		int afterReceived = 0;
		stateHandle.Entered += () =>
		{
			for (int i = 0; i < 1024; i++)
			{
				allQueued &= runtime.SendEvent("Queued");
			}
		};
		runtime.Diagnostic += (string code) =>
		{
			diagnosed |= code == "SMR001";
		};
		runtime.EventReceived += (StringName eventName) =>
		{
			if (eventName == new StringName("After"))
			{
				afterReceived++;
			}
		};
		runtime.EnterInitialState();
		bool flag = runtime.SendEvent("After");
		bool result = (allQueued & diagnosed & flag) && afterReceived == 1 && runtime.IsActive("S1025");
		runtime.Dispose();
		return result;
	}

	private static bool RunRuntimeMultiplePendingScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateMultiplePendingRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		int takenCount = 0;
		stateMachineRuntime.TransitionTaken += (CompiledStateMachineTransition _) =>
		{
			takenCount++;
		};
		stateMachineRuntime.EnterInitialState();
		stateMachineRuntime.TickProcess(0.06);
		bool result = takenCount == 2 && stateMachineRuntime.IsActive("Recover") && stateMachineRuntime.PendingDelayRemaining == 0.0;
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunRuntimeCompoundDelayOnceScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateCompoundDelayRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		int takenCount = 0;
		stateMachineRuntime.TransitionTaken += (CompiledStateMachineTransition _) =>
		{
			takenCount++;
		};
		stateMachineRuntime.EnterInitialState();
		stateMachineRuntime.TickProcess(0.06);
		stateMachineRuntime.TickProcess(0.1);
		bool result = takenCount == 1 && stateMachineRuntime.IsActive("Attack") && stateMachineRuntime.PendingDelayRemaining == 0.0;
		stateMachineRuntime.Dispose();
		return result;
	}

	private static bool RunRuntimeReentrantTickScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime runtime = new StateMachineRuntime();
		runtime.Initialize(program);
		StateHandle stateHandle = runtime.GetStateHandle("Idle");
		runtime.EnterInitialState();
		bool attempted = false;
		bool flag = false;
		stateHandle.PhysicsProcessing += (double _) =>
		{
			if (!attempted)
			{
				attempted = true;
				runtime.TickPhysics(0.0);
			}
		};
		try
		{
			runtime.TickPhysics(1.0 / 60.0);
		}
		catch (InvalidOperationException)
		{
			flag = true;
		}
		bool result = (attempted & flag) && runtime.IsActive("Idle");
		runtime.Dispose();
		return result;
	}

	private static bool RunRuntimeSingleActivePhysicsOrderScenario()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.runtime.single_active_physics_order",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "Idle"
		});
		string[] array = new string[2] { "Idle", "Attack" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root",
				ProcessFlags = StateMachineProcessFlags.PhysicsProcess
			});
		}
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("IdleToAttack", "Idle", "Attack", StateMachineTriggerKind.Event, "ToAttack", 0.0, 0));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("AttackToIdle", "Attack", "Idle", StateMachineTriggerKind.Event, "ToIdle", 0.0, 1));
		if (!StateMachineCompiler.TryCompile(stateMachineDefinition, out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle idle = stateMachineRuntime.GetStateHandle("Idle");
		StateHandle attack = stateMachineRuntime.GetStateHandle("Attack");
		List<string> trace = new List<string>(3);
		bool transitionRequested = false;
		idle.PhysicsProcessing += (double _) =>
		{
			trace.Add("idle");
			if (!transitionRequested)
			{
				transitionRequested = true;
				idle.SendEvent("ToAttack");
			}
		};
		attack.PhysicsProcessing += (double _) =>
		{
			trace.Add("attack");
			attack.SendEvent("ToIdle");
		};
		stateMachineRuntime.EnterInitialState();
		stateMachineRuntime.TickPhysics(1.0 / 60.0);
		bool num = string.Join(',', trace) == "idle,attack" && stateMachineRuntime.IsActive("Idle") && !stateMachineRuntime.IsActive("Attack");
		stateMachineRuntime.TickPhysics(1.0 / 60.0);
		bool flag = string.Join(',', trace) == "idle,attack,idle";
		stateMachineRuntime.Dispose();
		return num & flag;
	}

	private static bool RunRuntimePhysicsFastTargetScenario()
	{
		if (!StateMachineCompiler.TryCompile(CreateRuntimeDefinition(), out var program, out var _))
		{
			return false;
		}
		StateMachineRuntime stateMachineRuntime = new StateMachineRuntime();
		stateMachineRuntime.Initialize(program);
		StateHandle stateHandle = stateMachineRuntime.GetStateHandle("Idle");
		PhysicsFastTargetProbe target = new PhysicsFastTargetProbe();
		PhysicsFastTargetProbe target2 = new PhysicsFastTargetProbe();
		stateHandle.PhysicsProcessing += (double delta) =>
		{
			target.Trace.Add($"event:{delta:F3}");
		};
		bool flag = stateHandle.TrySetPhysicsFastCallback(target, 7);
		bool flag2 = !stateHandle.TrySetPhysicsFastCallback(target2, 9);
		stateMachineRuntime.EnterInitialState();
		stateMachineRuntime.TickPhysics(0.125);
		bool flag3 = string.Join(',', target.Trace) == "fast:7:0.125,event:0.125";
		bool flag4 = stateHandle.ClearPhysicsFastCallback(target);
		stateMachineRuntime.TickPhysics(0.25);
		bool flag5 = string.Join(',', target.Trace) == "fast:7:0.125,event:0.125,event:0.250";
		stateMachineRuntime.Dispose();
		if (flag & flag2 & flag3 & flag4 & flag5)
		{
			return !stateHandle.IsValid;
		}
		return false;
	}

	private static StateMachineDefinition CreateRuntimeDefinition()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.runtime",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "Idle"
		});
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Idle",
			DisplayName = "Idle",
			ParentId = "Root",
			ProcessFlags = StateMachineProcessFlags.PhysicsProcess
		});
		string[] array = new string[2] { "Attack", "Recover" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
		}
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("IdleToAttack", "Idle", "Attack", StateMachineTriggerKind.Event, "ToAttack", 0.0, 0));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("AttackToRecover", "Attack", "Recover", StateMachineTriggerKind.Automatic, string.Empty, 0.05, 1));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("RecoverToIdle", "Recover", "Idle", StateMachineTriggerKind.Event, "ToIdle", 0.0, 2));
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateFlatDirectDefinition()
	{
		return new StateMachineDefinition
		{
			DefinitionId = "probe.flat-direct",
			RootStateId = "FlatRoot",
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = "FlatRoot",
					DisplayName = "FlatRoot",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = "FlatIdle"
				},
				new StateMachineStateDefinition
				{
					StableId = "FlatIdle",
					DisplayName = "FlatIdle",
					ParentId = "FlatRoot"
				},
				new StateMachineStateDefinition
				{
					StableId = "FlatAttack",
					DisplayName = "FlatAttack",
					ParentId = "FlatRoot"
				}
			},
			Transitions = 
			{
				CreateRuntimeTransition("FlatToIdle", "FlatRoot", "FlatIdle", StateMachineTriggerKind.Event, "ToIdle", 0.0, 0),
				CreateRuntimeTransition("FlatToAttack", "FlatRoot", "FlatAttack", StateMachineTriggerKind.Event, "ToAttack", 0.0, 1)
			}
		};
	}

	private static StateMachineDefinition CreateEventDelaySnapshotDefinition()
	{
		return new StateMachineDefinition
		{
			DefinitionId = "probe.snapshot.event-delay",
			RootStateId = "Root",
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = "Root",
					DisplayName = "Root",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = "Idle"
				},
				new StateMachineStateDefinition
				{
					StableId = "Idle",
					DisplayName = "Idle",
					ParentId = "Root"
				},
				new StateMachineStateDefinition
				{
					StableId = "Attack",
					DisplayName = "Attack",
					ParentId = "Root"
				}
			},
			Transitions = { CreateRuntimeTransition("IdleToAttackDelayed", "Idle", "Attack", StateMachineTriggerKind.Event, "ToAttack", 0.1, 0) }
		};
	}

	private static StateMachineDefinition CreateParityDefinition()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.parity",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "Idle",
			ProcessFlags = StateMachineProcessFlags.PhysicsProcess,
			CallbackKey = "probe.root"
		});
		string[] array = new string[3] { "Idle", "Attack", "Recover" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
		}
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("IdleToAttack", "Idle", "Attack", StateMachineTriggerKind.Event, "ToAttack", 0.0, 0));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("ToRecover", "Attack", "Recover", StateMachineTriggerKind.Automatic, string.Empty, 0.05, 1));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("RecoverToIdle", "Recover", "Idle", StateMachineTriggerKind.Event, "ToIdle", 0.0, 2));
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateSnapshotDefinition()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.snapshot",
			RootStateId = "Root"
		};
		stateMachineDefinition.Aliases["IdleOld"] = "IdleRenamed";
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "IdleRenamed"
		});
		string[] array = new string[3] { "IdleRenamed", "Attack", "Recover" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
		}
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("IdleToAttack", "IdleRenamed", "Attack", StateMachineTriggerKind.Event, "ToAttack", 0.0, 0));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("AttackToRecover", "Attack", "Recover", StateMachineTriggerKind.Automatic, string.Empty, 0.1, 1));
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateSnapshotAutomaticDefinition()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.snapshot.automatic",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "Start"
		});
		string[] array = new string[2] { "Start", "Done" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
		}
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("StartToDone", "Start", "Done", StateMachineTriggerKind.Automatic, string.Empty, 0.0, 0));
		return stateMachineDefinition;
	}

	private void RunEditingScenario()
	{
		BindLegacyTrace();
		StateMachineSceneImporter stateMachineSceneImporter = new StateMachineSceneImporter();
		StateMachineSceneImportResult stateMachineSceneImportResult = stateMachineSceneImporter.Import(_chart, "res://Test/StateMachineResourceRuntimeTest.tscn");
		StateMachineSceneImportResult stateMachineSceneImportResult2 = stateMachineSceneImporter.Import(_chart, "RES:\\Test\\StateMachineResourceRuntimeTest.tscn", stateMachineSceneImportResult.LegacyPathToStableId);
		bool flag = stateMachineSceneImportResult.Succeeded && stateMachineSceneImportResult2.Succeeded && TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash) && TryGetDefinitionHash(stateMachineSceneImportResult2.Definition, out var contentHash2) && string.Equals(contentHash, contentHash2, StringComparison.Ordinal) && ImportMapsEqual(stateMachineSceneImportResult.LegacyPathToStableId, stateMachineSceneImportResult2.LegacyPathToStableId) && LayoutsEqual(stateMachineSceneImportResult.Layout, stateMachineSceneImportResult2.Layout);
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		if (stateMachineSceneImportResult.Succeeded && TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash3))
		{
			StateMachineEditService stateMachineEditService = new StateMachineEditService();
			string text = stateMachineSceneImportResult.LegacyPathToStableId["Root/Idle"];
			string parentId = stateMachineSceneImportResult.LegacyPathToStableId["Root"];
			stateMachineEditService.RenameState(stateMachineSceneImportResult.Definition, text, "Idle Renamed");
			StateMachineStateDefinition stateMachineStateDefinition = FindImportedState(stateMachineSceneImportResult.Definition, text);
			flag2 = stateMachineStateDefinition != null && string.Equals(stateMachineStateDefinition.StableId, text, StringComparison.Ordinal) && stateMachineStateDefinition.DisplayName.ToString() == "Idle Renamed";
			bool num = TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash4) && !string.Equals(contentHash3, contentHash4, StringComparison.Ordinal);
			bool flag6 = stateMachineEditService.Undo() && TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash5) && string.Equals(contentHash3, contentHash5, StringComparison.Ordinal);
			bool flag7 = stateMachineEditService.Redo() && TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash6) && string.Equals(contentHash4, contentHash6, StringComparison.Ordinal);
			flag3 = (num & flag6 & flag7) && stateMachineEditService.Undo();
			string text2 = stateMachineEditService.AddState(stateMachineSceneImportResult.Definition, "Temporary", StateMachineStateKind.Atomic, parentId, "probe").AffectedStableIds[0];
			string stableId = stateMachineEditService.AddTransition(stateMachineSceneImportResult.Definition, text, text2, StateMachineTriggerKind.Event, "ToTemporary").AffectedStableIds[0];
			stateMachineEditService.RemoveState(stateMachineSceneImportResult.Definition, text2, text);
			StateMachineTransitionDefinition stateMachineTransitionDefinition = FindImportedTransition(stateMachineSceneImportResult.Definition, stableId);
			bool num2 = FindImportedState(stateMachineSceneImportResult.Definition, text2) == null && stateMachineTransitionDefinition != null && string.Equals(stateMachineTransitionDefinition.TargetStateId, text, StringComparison.Ordinal);
			bool flag8 = stateMachineEditService.Undo() && FindImportedState(stateMachineSceneImportResult.Definition, text2) != null && string.Equals(FindImportedTransition(stateMachineSceneImportResult.Definition, stableId)?.TargetStateId, text2, StringComparison.Ordinal);
			flag4 = num2 & flag8;
			while (stateMachineEditService.Undo())
			{
			}
			bool flag9 = TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash7);
			stateMachineEditService.MoveGraphNode(stateMachineSceneImportResult.Layout, text, new Vector2(777f, 333f));
			flag5 = flag9 && TryGetDefinitionHash(stateMachineSceneImportResult.Definition, out var contentHash8) && string.Equals(contentHash7, contentHash8, StringComparison.Ordinal);
		}
		StateChart stateChart = CreateUnsupportedImportChart();
		StateMachineSceneImportResult stateMachineSceneImportResult3 = stateMachineSceneImporter.Import(stateChart, "res://Test/UnsupportedStateMachine.tscn");
		bool flag10 = stateMachineSceneImportResult3.Definition == null && HasDiagnostic(stateMachineSceneImportResult3.Diagnostics, "SMI002") && HasDiagnostic(stateMachineSceneImportResult3.Diagnostics, "SMI003") && HasDiagnostic(stateMachineSceneImportResult3.Diagnostics, "SMI004");
		stateChart.Free();
		bool flag11 = flag & flag2 & flag3 & flag4 & flag5 & flag10;
		GD.Print($"STATE_MACHINE_EDITING importIdempotent={flag} undoRoundTrip={flag3} layoutHashIsolated={flag5} unsupportedBlocked={flag10} stableIdPreserved={flag2} removeRetargetRoundTrip={flag4}");
		GD.Print($"STATE_MACHINE_RESULT passed={flag11}");
		GetTree().Quit((!flag11) ? 2 : 0);
	}

	private void RunMigrationInventoryScenario()
	{
		MatchCollection matchCollection = Regex.Matches(FileAccess.GetFileAsString("res://Tests/Fixtures/StateMachineMigrationManifest.json"), "\\\"path\\\":\\\"([^\\\"]+\\.tscn)\\\",\\\"classification\\\":\\\"(?:LegacyAdvanced|Migrated)\\\"");
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<string> list = new List<string>();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			string text = matchCollection[i].Groups[1].Value.Replace('\\', '/');
			if (!hashSet.Add(text))
			{
				continue;
			}
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://" + text, null, ResourceLoader.CacheMode.Ignore);
			if (packedScene == null)
			{
				list.Add("load:" + text);
				continue;
			}
			num++;
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node == null)
			{
				list.Add("instantiate:" + text);
				continue;
			}
			num2++;
			if (!(node.FindChild("StateChart", recursive: true, owned: false) is StateChart))
			{
				list.Add("chart:" + text);
			}
			else
			{
				num3++;
			}
			node.Free();
		}
		bool flag = matchCollection.Count > 0 && list.Count == 0 && num == hashSet.Count && num2 == hashSet.Count && num3 == hashSet.Count;
		GD.Print($"STATE_MACHINE_MIGRATION_INVENTORY sceneCount={hashSet.Count} loaded={num} instantiated={num2} chartsFound={num3} failures={list.Count}");
		for (int j = 0; j < list.Count; j++)
		{
			GD.PrintErr("STATE_MACHINE_MIGRATION_FAILURE " + list[j]);
		}
		GD.Print($"STATE_MACHINE_RESULT passed={flag}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static StateChart CreateUnsupportedImportChart()
	{
		StateChart stateChart = new StateChart();
		stateChart.Name = "UnsupportedChart";
		CompoundState compoundState = new CompoundState
		{
			Name = "Root",
			initialState = new NodePath("Idle")
		};
		stateChart.AddChild(compoundState, forceReadableName: false, InternalMode.Disabled);
		compoundState.AddChild(new AtomicState
		{
			Name = "Idle"
		}, forceReadableName: false, InternalMode.Disabled);
		ParallelState parallelState = new ParallelState
		{
			Name = "Parallel"
		};
		parallelState.AddChild(new AtomicState
		{
			Name = "Left"
		}, forceReadableName: false, InternalMode.Disabled);
		parallelState.AddChild(new AtomicState
		{
			Name = "Right"
		}, forceReadableName: false, InternalMode.Disabled);
		compoundState.AddChild(parallelState, forceReadableName: false, InternalMode.Disabled);
		compoundState.AddChild(new HistoryState
		{
			Name = "History"
		}, forceReadableName: false, InternalMode.Disabled);
		compoundState.AddChild(new Transition
		{
			Name = "UnsupportedTransition",
			to = new NodePath("../Idle"),
			guard = new Guard(),
			delayInSeconds = "speed * 2"
		}, forceReadableName: false, InternalMode.Disabled);
		return stateChart;
	}

	private static bool TryGetDefinitionHash(StateMachineDefinition definition, out string contentHash)
	{
		contentHash = string.Empty;
		if (!StateMachineCompiler.TryCompile(definition, out var program, out var _))
		{
			return false;
		}
		contentHash = program.ContentHash;
		return !string.IsNullOrWhiteSpace(contentHash);
	}

	private static bool ImportMapsEqual(Godot.Collections.Dictionary<string, string> left, Godot.Collections.Dictionary<string, string> right)
	{
		if (left == null || right == null || left.Count != right.Count)
		{
			return false;
		}
		foreach (string key in left.Keys)
		{
			if (!right.TryGetValue(key, out var value) || !string.Equals(left[key], value, StringComparison.Ordinal))
			{
				return false;
			}
		}
		return true;
	}

	private static bool LayoutsEqual(StateMachineLayout left, StateMachineLayout right)
	{
		if (left == null || right == null || left.Positions.Count != right.Positions.Count)
		{
			return false;
		}
		foreach (string key in left.Positions.Keys)
		{
			if (!right.Positions.TryGetValue(key, out var value) || left.Positions[key] != value)
			{
				return false;
			}
		}
		if (left.Collapsed.Count == right.Collapsed.Count && left.Comments.Count == right.Comments.Count && left.ScrollOffset == right.ScrollOffset)
		{
			return Mathf.IsEqualApprox(left.Zoom, right.Zoom);
		}
		return false;
	}

	private static StateMachineStateDefinition FindImportedState(StateMachineDefinition definition, string stableId)
	{
		for (int i = 0; i < definition.States.Count; i++)
		{
			StateMachineStateDefinition stateMachineStateDefinition = definition.States[i];
			if (stateMachineStateDefinition != null && string.Equals(stateMachineStateDefinition.StableId, stableId, StringComparison.Ordinal))
			{
				return stateMachineStateDefinition;
			}
		}
		return null;
	}

	private static StateMachineTransitionDefinition FindImportedTransition(StateMachineDefinition definition, string stableId)
	{
		for (int i = 0; i < definition.Transitions.Count; i++)
		{
			StateMachineTransitionDefinition stateMachineTransitionDefinition = definition.Transitions[i];
			if (stateMachineTransitionDefinition != null && string.Equals(stateMachineTransitionDefinition.StableId, stableId, StringComparison.Ordinal))
			{
				return stateMachineTransitionDefinition;
			}
		}
		return null;
	}

	private static bool HasDiagnostic(StateMachineValidationResult result, string code)
	{
		if (result?.Diagnostics == null)
		{
			return false;
		}
		for (int i = 0; i < result.Diagnostics.Count; i++)
		{
			if (string.Equals(result.Diagnostics[i].Code, code, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private static StateChart CreateLegacyHost(string name)
	{
		StateChart stateChart = new StateChart();
		stateChart.Name = name;
		stateChart.RuntimeMode = StateMachineRuntimeMode.Legacy;
		stateChart.AddChild(CreateLegacyRoot("Root"), forceReadableName: false, InternalMode.Disabled);
		return stateChart;
	}

	private static CompoundState CreateLegacyRoot(string name)
	{
		CompoundState compoundState = new CompoundState();
		compoundState.Name = name;
		compoundState.initialState = new NodePath("Idle");
		compoundState.AddChild(new AtomicState
		{
			Name = "Idle"
		}, forceReadableName: false, InternalMode.Disabled);
		return compoundState;
	}

	private static StateMachineDefinition CreateRuntimeBudgetDefinition()
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "probe.runtime.budget",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "S0"
		});
		for (int i = 0; i <= 1025; i++)
		{
			string text = $"S{i}";
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
			if (i != 0)
			{
				stateMachineDefinition.Transitions.Add(CreateRuntimeTransition($"Auto{i - 1}To{i}", $"S{i - 1}", text, StateMachineTriggerKind.Automatic, string.Empty, 0.0, i));
			}
		}
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateNestedRuntimeDefinition()
	{
		return new StateMachineDefinition
		{
			DefinitionId = "probe.runtime.nested",
			RootStateId = "Root",
			States = 
			{
				new StateMachineStateDefinition
				{
					StableId = "Root",
					DisplayName = "Root",
					Kind = StateMachineStateKind.Compound,
					InitialChildId = "BranchA"
				},
				new StateMachineStateDefinition
				{
					StableId = "BranchA",
					DisplayName = "Branch A",
					Kind = StateMachineStateKind.Compound,
					ParentId = "Root",
					InitialChildId = "LeafA"
				},
				new StateMachineStateDefinition
				{
					StableId = "LeafA",
					DisplayName = "Leaf A",
					ParentId = "BranchA"
				},
				new StateMachineStateDefinition
				{
					StableId = "BranchB",
					DisplayName = "Branch B",
					Kind = StateMachineStateKind.Compound,
					ParentId = "Root",
					InitialChildId = "LeafB"
				},
				new StateMachineStateDefinition
				{
					StableId = "LeafB",
					DisplayName = "Leaf B",
					ParentId = "BranchB"
				}
			},
			Transitions = { CreateRuntimeTransition("LeafAToLeafB", "LeafA", "LeafB", StateMachineTriggerKind.Event, "SwitchBranch", 0.0, 0) }
		};
	}

	private static StateMachineDefinition CreateMultiplePendingRuntimeDefinition()
	{
		StateMachineDefinition stateMachineDefinition = CreateCompoundRuntimeDefinition("probe.runtime.multiple_pending");
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("IdleToAttack", "Idle", "Attack", StateMachineTriggerKind.Automatic, string.Empty, 0.05, 0));
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("RootToRecover", "Root", "Recover", StateMachineTriggerKind.Automatic, string.Empty, 0.05, 1));
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateCompoundDelayRuntimeDefinition()
	{
		StateMachineDefinition stateMachineDefinition = CreateCompoundRuntimeDefinition("probe.runtime.compound_delay");
		stateMachineDefinition.Transitions.Add(CreateRuntimeTransition("RootToAttack", "Root", "Attack", StateMachineTriggerKind.Automatic, string.Empty, 0.05, 0));
		return stateMachineDefinition;
	}

	private static StateMachineDefinition CreateCompoundRuntimeDefinition(string definitionId)
	{
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = definitionId,
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "Idle"
		});
		string[] array = new string[3] { "Idle", "Attack", "Recover" };
		foreach (string text in array)
		{
			stateMachineDefinition.States.Add(new StateMachineStateDefinition
			{
				StableId = text,
				DisplayName = text,
				ParentId = "Root"
			});
		}
		return stateMachineDefinition;
	}

	private static StateMachineTransitionDefinition CreateRuntimeTransition(string stableId, string sourceStateId, string targetStateId, StateMachineTriggerKind triggerKind, string eventName, double delaySeconds, int declarationOrder)
	{
		return new StateMachineTransitionDefinition
		{
			StableId = stableId,
			SourceStateId = sourceStateId,
			TargetStateId = targetStateId,
			TriggerKind = triggerKind,
			EventName = new StringName(eventName),
			DelaySeconds = delaySeconds,
			DeclarationOrder = declarationOrder
		};
	}

	private async Task<bool> WaitForActiveState(StateChartState state, int maximumFrames)
	{
		for (int i = 0; i < maximumFrames; i++)
		{
			if (state.active)
			{
				return true;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		return state.active;
	}

	private async Task<bool> WaitForActiveState(StateHandle state, int maximumFrames)
	{
		for (int i = 0; i < maximumFrames; i++)
		{
			if (state != null && state.IsActive)
			{
				return true;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		return state?.IsActive ?? false;
	}

	private void EmitTrace(string mode)
	{
		GD.Print("STATE_MACHINE_TRACE mode=" + mode + " trace=" + string.Join(',', _trace));
	}

	private void EmitSnapshot(string mode)
	{
		string value = _root.activeState?.Name.ToString() ?? "<none>";
		GD.Print($"STATE_MACHINE_SNAPSHOT mode={mode} active={value} roundTrip={_snapshotRoundTripPassed}");
	}

	private void EmitMetrics(string mode)
	{
		int value = CountStateChartRelatedNodes(_chart);
		GD.Print($"STATE_MACHINE_METRIC mode={mode} requestedSpawnCount={_requestedSpawnCount} stateChartNodeCount={value} physicsFrames={_physicsFrames} physicsCallbacks={_rootPhysicsCallbacks} fullRate={_fullRate}");
	}

	private static int CountStateChartRelatedNodes(Node node)
	{
		bool flag = ((node is StateChart || node is StateChartState || node is Transition) ? true : false);
		int num = (flag ? 1 : 0);
		foreach (Node child in node.GetChildren())
		{
			num += CountStateChartRelatedNodes(child);
		}
		return num;
	}

	private void ReadCommandLineArguments()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string text in cmdlineUserArgs)
		{
			if (text.StartsWith("--state-machine-mode=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--state-machine-mode=".Length;
				_mode = text2.Substring(length, text2.Length - length);
			}
			else if (text.StartsWith("--state-machine-spawn-count=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--state-machine-spawn-count=".Length;
				if (int.TryParse(text2.Substring(length, text2.Length - length), out var result))
				{
					_requestedSpawnCount = Math.Max(1, result);
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(87)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindLegacyTrace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunLegacyScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunLegacyScenarioAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunValidationScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateValidDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AssertValidationCodes, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "caseName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "expectedCodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateTransition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sourceStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "triggerKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddCompoundWithLeaf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "compoundId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "parentId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "leafId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunCompilerScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCompilerDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "definitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCompositionBaseDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "definitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCompositionDerivedDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "definitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "baseDefinition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "overrideInitial", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCompilerTransition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sourceStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "priority", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "declarationOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunRuntimeScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunAdvancedVisualSemanticsScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunAdvancedDelayScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunAdvancedGuardScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunAdvancedParallelScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunAdvancedHistoryScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.AdvancedState, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "parentId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "initialChildId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvancedTransition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sourceStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "triggerKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "declarationOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RunControllerScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteControllerFlatDirectScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ExecuteControllerScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunControllerDisposeDuringCallbackScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteControllerDisposeDuringCallbackScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunControllerCacheScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteControllerCacheScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunParityScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunParityScenarioAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunSnapshotScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunCharacterSchedulingScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunCharacterSchedulingScenarioAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunComponentOwnerScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunComponentOwnerScenarioAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunResourceComponentOwnerScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunResourceComponentOwnerScenarioAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunBaseCharacterScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteBaseCharacterScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCharacterProbeNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "unique", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateCharacterSchedulingDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "definitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountNodeBranch, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RunSnapshotScenarioAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunSnapshotAutomaticFallbackScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSnapshotEnvelope, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnRuntimePhysicsProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunNestedRuntimeOrderScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeExceptionRecoveryScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeCallbackAtomicScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeDisposeCallbackScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeReinitializeCallbackScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeQueueBudgetScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeAutomaticBacklogScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeMultiplePendingScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeCompoundDelayOnceScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeReentrantTickScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimeSingleActivePhysicsOrderScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunRuntimePhysicsFastTargetScenario, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateRuntimeDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateFlatDirectDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateEventDelaySnapshotDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateParityDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSnapshotDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSnapshotAutomaticDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RunEditingScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunMigrationInventoryScenario, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateUnsupportedImportChart, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ImportMapsEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LayoutsEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindImportedState, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindImportedTransition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateLegacyHost, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateLegacyRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateRuntimeBudgetDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateNestedRuntimeDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateMultiplePendingRuntimeDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateCompoundDelayRuntimeDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateCompoundRuntimeDefinition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "definitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateRuntimeTransition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sourceStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "triggerKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "declarationOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitTrace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitMetrics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountStateChartRelatedNodes, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadCommandLineArguments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindLegacyTrace && args.Count == 0)
		{
			BindLegacyTrace();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RunLegacyScenario && args.Count == 0)
		{
			RunLegacyScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunLegacyScenarioAsync && args.Count == 0)
		{
			RunLegacyScenarioAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunValidationScenario && args.Count == 0)
		{
			RunValidationScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateValidDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateValidDefinition());
			return true;
		}
		if (method == MethodName.AssertValidationCodes && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(AssertValidationCodes(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineDefinition>(in args[1]), VariantUtils.ConvertTo<string[]>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateTransition && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(CreateTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[3])));
			return true;
		}
		if (method == MethodName.AddCompoundWithLeaf && args.Count == 4)
		{
			AddCompoundWithLeaf(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunCompilerScenario && args.Count == 0)
		{
			RunCompilerScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCompilerDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompilerDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateCompositionBaseDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompositionBaseDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateCompositionDerivedDefinition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompositionDerivedDefinition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineDefinition>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateCompilerTransition && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(CreateCompilerTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<int>(in args[5])));
			return true;
		}
		if (method == MethodName.RunRuntimeScenario && args.Count == 0)
		{
			RunRuntimeScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunAdvancedVisualSemanticsScenario && args.Count == 0)
		{
			RunAdvancedVisualSemanticsScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunAdvancedDelayScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedDelayScenario());
			return true;
		}
		if (method == MethodName.RunAdvancedGuardScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedGuardScenario());
			return true;
		}
		if (method == MethodName.RunAdvancedParallelScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedParallelScenario());
			return true;
		}
		if (method == MethodName.RunAdvancedHistoryScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedHistoryScenario());
			return true;
		}
		if (method == MethodName.AdvancedState && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StateMachineStateDefinition>(AdvancedState(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineStateKind>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.AdvancedTransition && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(AdvancedTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[7])));
			return true;
		}
		if (method == MethodName.RunControllerScenario && args.Count == 0)
		{
			RunControllerScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteControllerFlatDirectScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteControllerFlatDirectScenario());
			return true;
		}
		if (method == MethodName.ExecuteControllerScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteControllerScenario());
			return true;
		}
		if (method == MethodName.RunControllerDisposeDuringCallbackScenario && args.Count == 0)
		{
			RunControllerDisposeDuringCallbackScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteControllerDisposeDuringCallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteControllerDisposeDuringCallbackScenario());
			return true;
		}
		if (method == MethodName.RunControllerCacheScenario && args.Count == 0)
		{
			RunControllerCacheScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteControllerCacheScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteControllerCacheScenario());
			return true;
		}
		if (method == MethodName.RunParityScenario && args.Count == 0)
		{
			RunParityScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunParityScenarioAsync && args.Count == 0)
		{
			RunParityScenarioAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunSnapshotScenario && args.Count == 0)
		{
			RunSnapshotScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunCharacterSchedulingScenario && args.Count == 0)
		{
			RunCharacterSchedulingScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunCharacterSchedulingScenarioAsync && args.Count == 0)
		{
			RunCharacterSchedulingScenarioAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunComponentOwnerScenario && args.Count == 0)
		{
			RunComponentOwnerScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunComponentOwnerScenarioAsync && args.Count == 0)
		{
			RunComponentOwnerScenarioAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunResourceComponentOwnerScenario && args.Count == 0)
		{
			RunResourceComponentOwnerScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunResourceComponentOwnerScenarioAsync && args.Count == 0)
		{
			RunResourceComponentOwnerScenarioAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunBaseCharacterScenario && args.Count == 0)
		{
			RunBaseCharacterScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteBaseCharacterScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteBaseCharacterScenario());
			return true;
		}
		if (method == MethodName.AddCharacterProbeNode && args.Count == 5)
		{
			AddCharacterProbeNode(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Node>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCharacterSchedulingDefinition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCharacterSchedulingDefinition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.CountNodeBranch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountNodeBranch(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.RunSnapshotScenarioAsync && args.Count == 0)
		{
			RunSnapshotScenarioAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunSnapshotAutomaticFallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunSnapshotAutomaticFallbackScenario());
			return true;
		}
		if (method == MethodName.CreateSnapshotEnvelope && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineSnapshot>(CreateSnapshotEnvelope(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.OnRuntimePhysicsProcessing && args.Count == 1)
		{
			OnRuntimePhysicsProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunNestedRuntimeOrderScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunNestedRuntimeOrderScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeExceptionRecoveryScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeExceptionRecoveryScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeCallbackAtomicScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeCallbackAtomicScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeDisposeCallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeDisposeCallbackScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeReinitializeCallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeReinitializeCallbackScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeQueueBudgetScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeQueueBudgetScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeAutomaticBacklogScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeAutomaticBacklogScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeMultiplePendingScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeMultiplePendingScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeCompoundDelayOnceScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeCompoundDelayOnceScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeReentrantTickScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeReentrantTickScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeSingleActivePhysicsOrderScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeSingleActivePhysicsOrderScenario());
			return true;
		}
		if (method == MethodName.RunRuntimePhysicsFastTargetScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimePhysicsFastTargetScenario());
			return true;
		}
		if (method == MethodName.CreateRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateFlatDirectDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateFlatDirectDefinition());
			return true;
		}
		if (method == MethodName.CreateEventDelaySnapshotDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateEventDelaySnapshotDefinition());
			return true;
		}
		if (method == MethodName.CreateParityDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateParityDefinition());
			return true;
		}
		if (method == MethodName.CreateSnapshotDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateSnapshotDefinition());
			return true;
		}
		if (method == MethodName.CreateSnapshotAutomaticDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateSnapshotAutomaticDefinition());
			return true;
		}
		if (method == MethodName.RunEditingScenario && args.Count == 0)
		{
			RunEditingScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.RunMigrationInventoryScenario && args.Count == 0)
		{
			RunMigrationInventoryScenario();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateUnsupportedImportChart && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateChart>(CreateUnsupportedImportChart());
			return true;
		}
		if (method == MethodName.ImportMapsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportMapsEqual(VariantUtils.ConvertToDictionary<string, string>(in args[0]), VariantUtils.ConvertToDictionary<string, string>(in args[1])));
			return true;
		}
		if (method == MethodName.LayoutsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(LayoutsEqual(VariantUtils.ConvertTo<StateMachineLayout>(in args[0]), VariantUtils.ConvertTo<StateMachineLayout>(in args[1])));
			return true;
		}
		if (method == MethodName.FindImportedState && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StateMachineStateDefinition>(FindImportedState(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindImportedTransition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(FindImportedTransition(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateLegacyHost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateChart>(CreateLegacyHost(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateLegacyRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CompoundState>(CreateLegacyRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateRuntimeBudgetDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateRuntimeBudgetDefinition());
			return true;
		}
		if (method == MethodName.CreateNestedRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateNestedRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateMultiplePendingRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateMultiplePendingRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateCompoundDelayRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompoundDelayRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateCompoundRuntimeDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompoundRuntimeDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateRuntimeTransition && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(CreateRuntimeTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6])));
			return true;
		}
		if (method == MethodName.EmitTrace && args.Count == 1)
		{
			EmitTrace(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitSnapshot && args.Count == 1)
		{
			EmitSnapshot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitMetrics && args.Count == 1)
		{
			EmitMetrics(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountStateChartRelatedNodes && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountStateChartRelatedNodes(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadCommandLineArguments && args.Count == 0)
		{
			ReadCommandLineArguments();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateTransition && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(CreateTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[3])));
			return true;
		}
		if (method == MethodName.AddCompoundWithLeaf && args.Count == 4)
		{
			AddCompoundWithLeaf(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCompilerDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompilerDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateCompositionBaseDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompositionBaseDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateCompositionDerivedDefinition && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompositionDerivedDefinition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineDefinition>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateCompilerTransition && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(CreateCompilerTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<int>(in args[5])));
			return true;
		}
		if (method == MethodName.RunAdvancedDelayScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedDelayScenario());
			return true;
		}
		if (method == MethodName.RunAdvancedGuardScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedGuardScenario());
			return true;
		}
		if (method == MethodName.RunAdvancedParallelScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedParallelScenario());
			return true;
		}
		if (method == MethodName.RunAdvancedHistoryScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunAdvancedHistoryScenario());
			return true;
		}
		if (method == MethodName.AdvancedState && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StateMachineStateDefinition>(AdvancedState(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineStateKind>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.AdvancedTransition && args.Count == 8)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(AdvancedTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<StateMachineGuardDefinition>(in args[7])));
			return true;
		}
		if (method == MethodName.ExecuteControllerFlatDirectScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteControllerFlatDirectScenario());
			return true;
		}
		if (method == MethodName.ExecuteControllerDisposeDuringCallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteControllerDisposeDuringCallbackScenario());
			return true;
		}
		if (method == MethodName.AddCharacterProbeNode && args.Count == 5)
		{
			AddCharacterProbeNode(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Node>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCharacterSchedulingDefinition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCharacterSchedulingDefinition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.CountNodeBranch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountNodeBranch(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.RunSnapshotAutomaticFallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunSnapshotAutomaticFallbackScenario());
			return true;
		}
		if (method == MethodName.CreateSnapshotEnvelope && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineSnapshot>(CreateSnapshotEnvelope(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.RunNestedRuntimeOrderScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunNestedRuntimeOrderScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeExceptionRecoveryScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeExceptionRecoveryScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeCallbackAtomicScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeCallbackAtomicScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeDisposeCallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeDisposeCallbackScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeReinitializeCallbackScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeReinitializeCallbackScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeQueueBudgetScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeQueueBudgetScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeAutomaticBacklogScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeAutomaticBacklogScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeMultiplePendingScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeMultiplePendingScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeCompoundDelayOnceScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeCompoundDelayOnceScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeReentrantTickScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeReentrantTickScenario());
			return true;
		}
		if (method == MethodName.RunRuntimeSingleActivePhysicsOrderScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimeSingleActivePhysicsOrderScenario());
			return true;
		}
		if (method == MethodName.RunRuntimePhysicsFastTargetScenario && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunRuntimePhysicsFastTargetScenario());
			return true;
		}
		if (method == MethodName.CreateRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateFlatDirectDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateFlatDirectDefinition());
			return true;
		}
		if (method == MethodName.CreateEventDelaySnapshotDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateEventDelaySnapshotDefinition());
			return true;
		}
		if (method == MethodName.CreateParityDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateParityDefinition());
			return true;
		}
		if (method == MethodName.CreateSnapshotDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateSnapshotDefinition());
			return true;
		}
		if (method == MethodName.CreateSnapshotAutomaticDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateSnapshotAutomaticDefinition());
			return true;
		}
		if (method == MethodName.CreateUnsupportedImportChart && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateChart>(CreateUnsupportedImportChart());
			return true;
		}
		if (method == MethodName.ImportMapsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportMapsEqual(VariantUtils.ConvertToDictionary<string, string>(in args[0]), VariantUtils.ConvertToDictionary<string, string>(in args[1])));
			return true;
		}
		if (method == MethodName.LayoutsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(LayoutsEqual(VariantUtils.ConvertTo<StateMachineLayout>(in args[0]), VariantUtils.ConvertTo<StateMachineLayout>(in args[1])));
			return true;
		}
		if (method == MethodName.FindImportedState && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StateMachineStateDefinition>(FindImportedState(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindImportedTransition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(FindImportedTransition(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateLegacyHost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateChart>(CreateLegacyHost(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateLegacyRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CompoundState>(CreateLegacyRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateRuntimeBudgetDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateRuntimeBudgetDefinition());
			return true;
		}
		if (method == MethodName.CreateNestedRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateNestedRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateMultiplePendingRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateMultiplePendingRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateCompoundDelayRuntimeDefinition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompoundDelayRuntimeDefinition());
			return true;
		}
		if (method == MethodName.CreateCompoundRuntimeDefinition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineDefinition>(CreateCompoundRuntimeDefinition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateRuntimeTransition && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionDefinition>(CreateRuntimeTransition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6])));
			return true;
		}
		if (method == MethodName.CountStateChartRelatedNodes && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountStateChartRelatedNodes(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.BindLegacyTrace)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.RunLegacyScenario)
		{
			return true;
		}
		if (method == MethodName.RunLegacyScenarioAsync)
		{
			return true;
		}
		if (method == MethodName.RunValidationScenario)
		{
			return true;
		}
		if (method == MethodName.CreateValidDefinition)
		{
			return true;
		}
		if (method == MethodName.AssertValidationCodes)
		{
			return true;
		}
		if (method == MethodName.CreateTransition)
		{
			return true;
		}
		if (method == MethodName.AddCompoundWithLeaf)
		{
			return true;
		}
		if (method == MethodName.RunCompilerScenario)
		{
			return true;
		}
		if (method == MethodName.CreateCompilerDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateCompositionBaseDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateCompositionDerivedDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateCompilerTransition)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeScenario)
		{
			return true;
		}
		if (method == MethodName.RunAdvancedVisualSemanticsScenario)
		{
			return true;
		}
		if (method == MethodName.RunAdvancedDelayScenario)
		{
			return true;
		}
		if (method == MethodName.RunAdvancedGuardScenario)
		{
			return true;
		}
		if (method == MethodName.RunAdvancedParallelScenario)
		{
			return true;
		}
		if (method == MethodName.RunAdvancedHistoryScenario)
		{
			return true;
		}
		if (method == MethodName.AdvancedState)
		{
			return true;
		}
		if (method == MethodName.AdvancedTransition)
		{
			return true;
		}
		if (method == MethodName.RunControllerScenario)
		{
			return true;
		}
		if (method == MethodName.ExecuteControllerFlatDirectScenario)
		{
			return true;
		}
		if (method == MethodName.ExecuteControllerScenario)
		{
			return true;
		}
		if (method == MethodName.RunControllerDisposeDuringCallbackScenario)
		{
			return true;
		}
		if (method == MethodName.ExecuteControllerDisposeDuringCallbackScenario)
		{
			return true;
		}
		if (method == MethodName.RunControllerCacheScenario)
		{
			return true;
		}
		if (method == MethodName.ExecuteControllerCacheScenario)
		{
			return true;
		}
		if (method == MethodName.RunParityScenario)
		{
			return true;
		}
		if (method == MethodName.RunParityScenarioAsync)
		{
			return true;
		}
		if (method == MethodName.RunSnapshotScenario)
		{
			return true;
		}
		if (method == MethodName.RunCharacterSchedulingScenario)
		{
			return true;
		}
		if (method == MethodName.RunCharacterSchedulingScenarioAsync)
		{
			return true;
		}
		if (method == MethodName.RunComponentOwnerScenario)
		{
			return true;
		}
		if (method == MethodName.RunComponentOwnerScenarioAsync)
		{
			return true;
		}
		if (method == MethodName.RunResourceComponentOwnerScenario)
		{
			return true;
		}
		if (method == MethodName.RunResourceComponentOwnerScenarioAsync)
		{
			return true;
		}
		if (method == MethodName.RunBaseCharacterScenario)
		{
			return true;
		}
		if (method == MethodName.ExecuteBaseCharacterScenario)
		{
			return true;
		}
		if (method == MethodName.AddCharacterProbeNode)
		{
			return true;
		}
		if (method == MethodName.CreateCharacterSchedulingDefinition)
		{
			return true;
		}
		if (method == MethodName.CountNodeBranch)
		{
			return true;
		}
		if (method == MethodName.RunSnapshotScenarioAsync)
		{
			return true;
		}
		if (method == MethodName.RunSnapshotAutomaticFallbackScenario)
		{
			return true;
		}
		if (method == MethodName.CreateSnapshotEnvelope)
		{
			return true;
		}
		if (method == MethodName.OnRuntimePhysicsProcessing)
		{
			return true;
		}
		if (method == MethodName.RunNestedRuntimeOrderScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeExceptionRecoveryScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeCallbackAtomicScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeDisposeCallbackScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeReinitializeCallbackScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeQueueBudgetScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeAutomaticBacklogScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeMultiplePendingScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeCompoundDelayOnceScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeReentrantTickScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeSingleActivePhysicsOrderScenario)
		{
			return true;
		}
		if (method == MethodName.RunRuntimePhysicsFastTargetScenario)
		{
			return true;
		}
		if (method == MethodName.CreateRuntimeDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateFlatDirectDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateEventDelaySnapshotDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateParityDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateSnapshotDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateSnapshotAutomaticDefinition)
		{
			return true;
		}
		if (method == MethodName.RunEditingScenario)
		{
			return true;
		}
		if (method == MethodName.RunMigrationInventoryScenario)
		{
			return true;
		}
		if (method == MethodName.CreateUnsupportedImportChart)
		{
			return true;
		}
		if (method == MethodName.ImportMapsEqual)
		{
			return true;
		}
		if (method == MethodName.LayoutsEqual)
		{
			return true;
		}
		if (method == MethodName.FindImportedState)
		{
			return true;
		}
		if (method == MethodName.FindImportedTransition)
		{
			return true;
		}
		if (method == MethodName.CreateLegacyHost)
		{
			return true;
		}
		if (method == MethodName.CreateLegacyRoot)
		{
			return true;
		}
		if (method == MethodName.CreateRuntimeBudgetDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateNestedRuntimeDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateMultiplePendingRuntimeDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateCompoundDelayRuntimeDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateCompoundRuntimeDefinition)
		{
			return true;
		}
		if (method == MethodName.CreateRuntimeTransition)
		{
			return true;
		}
		if (method == MethodName.EmitTrace)
		{
			return true;
		}
		if (method == MethodName.EmitSnapshot)
		{
			return true;
		}
		if (method == MethodName.EmitMetrics)
		{
			return true;
		}
		if (method == MethodName.CountStateChartRelatedNodes)
		{
			return true;
		}
		if (method == MethodName.ReadCommandLineArguments)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._chart)
		{
			_chart = VariantUtils.ConvertTo<StateChart>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<CompoundState>(in value);
			return true;
		}
		if (name == PropertyName._idle)
		{
			_idle = VariantUtils.ConvertTo<AtomicState>(in value);
			return true;
		}
		if (name == PropertyName._attack)
		{
			_attack = VariantUtils.ConvertTo<AtomicState>(in value);
			return true;
		}
		if (name == PropertyName._recover)
		{
			_recover = VariantUtils.ConvertTo<AtomicState>(in value);
			return true;
		}
		if (name == PropertyName._toRecover)
		{
			_toRecover = VariantUtils.ConvertTo<Transition>(in value);
			return true;
		}
		if (name == PropertyName._attackRequested)
		{
			_attackRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._physicsFrames)
		{
			_physicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._rootPhysicsCallbacks)
		{
			_rootPhysicsCallbacks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimePhysicsCallbacks)
		{
			_runtimePhysicsCallbacks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._snapshotRoundTripPassed)
		{
			_snapshotRoundTripPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fullRate)
		{
			_fullRate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._requestedSpawnCount)
		{
			_requestedSpawnCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._mode)
		{
			_mode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._chart)
		{
			value = VariantUtils.CreateFrom(in _chart);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._idle)
		{
			value = VariantUtils.CreateFrom(in _idle);
			return true;
		}
		if (name == PropertyName._attack)
		{
			value = VariantUtils.CreateFrom(in _attack);
			return true;
		}
		if (name == PropertyName._recover)
		{
			value = VariantUtils.CreateFrom(in _recover);
			return true;
		}
		if (name == PropertyName._toRecover)
		{
			value = VariantUtils.CreateFrom(in _toRecover);
			return true;
		}
		if (name == PropertyName._attackRequested)
		{
			value = VariantUtils.CreateFrom(in _attackRequested);
			return true;
		}
		if (name == PropertyName._physicsFrames)
		{
			value = VariantUtils.CreateFrom(in _physicsFrames);
			return true;
		}
		if (name == PropertyName._rootPhysicsCallbacks)
		{
			value = VariantUtils.CreateFrom(in _rootPhysicsCallbacks);
			return true;
		}
		if (name == PropertyName._runtimePhysicsCallbacks)
		{
			value = VariantUtils.CreateFrom(in _runtimePhysicsCallbacks);
			return true;
		}
		if (name == PropertyName._snapshotRoundTripPassed)
		{
			value = VariantUtils.CreateFrom(in _snapshotRoundTripPassed);
			return true;
		}
		if (name == PropertyName._fullRate)
		{
			value = VariantUtils.CreateFrom(in _fullRate);
			return true;
		}
		if (name == PropertyName._requestedSpawnCount)
		{
			value = VariantUtils.CreateFrom(in _requestedSpawnCount);
			return true;
		}
		if (name == PropertyName._mode)
		{
			value = VariantUtils.CreateFrom(in _mode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._chart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._idle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._attack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._recover, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toRecover, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._attackRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._physicsFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._rootPhysicsCallbacks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimePhysicsCallbacks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._snapshotRoundTripPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._fullRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._requestedSpawnCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._mode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._chart, Variant.From(in _chart));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._idle, Variant.From(in _idle));
		info.AddProperty(PropertyName._attack, Variant.From(in _attack));
		info.AddProperty(PropertyName._recover, Variant.From(in _recover));
		info.AddProperty(PropertyName._toRecover, Variant.From(in _toRecover));
		info.AddProperty(PropertyName._attackRequested, Variant.From(in _attackRequested));
		info.AddProperty(PropertyName._physicsFrames, Variant.From(in _physicsFrames));
		info.AddProperty(PropertyName._rootPhysicsCallbacks, Variant.From(in _rootPhysicsCallbacks));
		info.AddProperty(PropertyName._runtimePhysicsCallbacks, Variant.From(in _runtimePhysicsCallbacks));
		info.AddProperty(PropertyName._snapshotRoundTripPassed, Variant.From(in _snapshotRoundTripPassed));
		info.AddProperty(PropertyName._fullRate, Variant.From(in _fullRate));
		info.AddProperty(PropertyName._requestedSpawnCount, Variant.From(in _requestedSpawnCount));
		info.AddProperty(PropertyName._mode, Variant.From(in _mode));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._chart, out var value))
		{
			_chart = value.As<StateChart>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value2))
		{
			_root = value2.As<CompoundState>();
		}
		if (info.TryGetProperty(PropertyName._idle, out var value3))
		{
			_idle = value3.As<AtomicState>();
		}
		if (info.TryGetProperty(PropertyName._attack, out var value4))
		{
			_attack = value4.As<AtomicState>();
		}
		if (info.TryGetProperty(PropertyName._recover, out var value5))
		{
			_recover = value5.As<AtomicState>();
		}
		if (info.TryGetProperty(PropertyName._toRecover, out var value6))
		{
			_toRecover = value6.As<Transition>();
		}
		if (info.TryGetProperty(PropertyName._attackRequested, out var value7))
		{
			_attackRequested = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._physicsFrames, out var value8))
		{
			_physicsFrames = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._rootPhysicsCallbacks, out var value9))
		{
			_rootPhysicsCallbacks = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimePhysicsCallbacks, out var value10))
		{
			_runtimePhysicsCallbacks = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._snapshotRoundTripPassed, out var value11))
		{
			_snapshotRoundTripPassed = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fullRate, out var value12))
		{
			_fullRate = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._requestedSpawnCount, out var value13))
		{
			_requestedSpawnCount = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._mode, out var value14))
		{
			_mode = value14.As<string>();
		}
	}
}
