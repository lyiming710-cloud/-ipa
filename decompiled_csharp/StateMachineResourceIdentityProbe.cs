using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Test/StateMachineResourceIdentityProbe.cs")]
public class StateMachineResourceIdentityProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ProbeDerivedResourceIdentity = "ProbeDerivedResourceIdentity";

		public static readonly StringName ProbeUndoHistoryIsolation = "ProbeUndoHistoryIsolation";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	public override void _Ready()
	{
		bool flag = ProbeDerivedResourceIdentity();
		bool flag2 = ProbeUndoHistoryIsolation();
		GD.Print($"[STATE_MACHINE_RESOURCE_IDENTITY_PROBE] identity={flag} isolatedUndo={flag2}");
		GetTree().Quit((!(flag & flag2)) ? 1 : 0);
	}

	private static bool ProbeDerivedResourceIdentity()
	{
		StateMachineStateDefinition stateMachineStateDefinition = new StateMachineStateDefinition
		{
			StableId = "Root",
			DisplayName = "Root",
			Kind = StateMachineStateKind.Compound,
			InitialChildId = "ModState"
		};
		StateMachineProbeDerivedState stateMachineProbeDerivedState = new StateMachineProbeDerivedState
		{
			StableId = "ModState",
			DisplayName = "Before",
			ParentId = "Root",
			ModPayload = "keep-me",
			ResourceName = "DerivedStateResource"
		};
		StateMachineProbeDerivedTransition stateMachineProbeDerivedTransition = new StateMachineProbeDerivedTransition
		{
			StableId = "ModTransition",
			SourceStateId = "ModState",
			TargetStateId = "Root",
			TriggerKind = StateMachineTriggerKind.Event,
			EventName = "go",
			ModWeight = 73,
			ResourceName = "DerivedTransitionResource"
		};
		StateMachineDefinition stateMachineDefinition = new StateMachineDefinition
		{
			DefinitionId = "identity-probe",
			RootStateId = "Root"
		};
		stateMachineDefinition.States.Add(stateMachineStateDefinition);
		stateMachineDefinition.States.Add(stateMachineProbeDerivedState);
		stateMachineDefinition.Transitions.Add(stateMachineProbeDerivedTransition);
		StateMachineEditService stateMachineEditService = new StateMachineEditService();
		stateMachineEditService.RenameState(stateMachineDefinition, "ModState", "After");
		bool flag = stateMachineDefinition.States[0] == stateMachineStateDefinition && stateMachineDefinition.States[1] == stateMachineProbeDerivedState && stateMachineDefinition.States[1] is StateMachineProbeDerivedState stateMachineProbeDerivedState2 && stateMachineProbeDerivedState2.DisplayName.ToString() == "After" && stateMachineProbeDerivedState2.ModPayload == "keep-me" && stateMachineProbeDerivedState2.ResourceName == "DerivedStateResource" && stateMachineDefinition.Transitions[0] == stateMachineProbeDerivedTransition && stateMachineProbeDerivedTransition.ModWeight == 73 && stateMachineProbeDerivedTransition.ResourceName == "DerivedTransitionResource";
		bool flag2 = stateMachineEditService.Undo() && stateMachineDefinition.States[1] == stateMachineProbeDerivedState && stateMachineProbeDerivedState.DisplayName.ToString() == "Before" && stateMachineProbeDerivedState.ModPayload == "keep-me";
		bool flag3 = stateMachineEditService.Redo() && stateMachineDefinition.States[1] == stateMachineProbeDerivedState && stateMachineProbeDerivedState.DisplayName.ToString() == "After" && stateMachineProbeDerivedState.ModPayload == "keep-me";
		stateMachineEditService.RemoveState(stateMachineDefinition, "ModState");
		bool flag4 = !stateMachineDefinition.States.Contains(stateMachineProbeDerivedState) && !stateMachineDefinition.Transitions.Contains(stateMachineProbeDerivedTransition);
		bool flag5 = stateMachineEditService.Undo() && stateMachineDefinition.States.Contains(stateMachineProbeDerivedState) && stateMachineDefinition.Transitions.Contains(stateMachineProbeDerivedTransition) && stateMachineProbeDerivedState.ModPayload == "keep-me" && stateMachineProbeDerivedTransition.ModWeight == 73;
		return flag & flag2 & flag3 & flag4 & flag5;
	}

	private static bool ProbeUndoHistoryIsolation()
	{
		XWUndoRedoManager xWUndoRedoManager = new XWUndoRedoManager();
		XWStateMachineUndoAdapter xWStateMachineUndoAdapter = new XWStateMachineUndoAdapter(xWUndoRedoManager);
		int firstValue = 0;
		xWStateMachineUndoAdapter.Commit("first", new StateMachineEditCommand(() =>
		{
			firstValue = 1;
		}, () =>
		{
			firstValue = 0;
		}, Array.Empty<string>()));
		XWStateMachineUndoAdapter xWStateMachineUndoAdapter2 = new XWStateMachineUndoAdapter(xWUndoRedoManager);
		bool num = xWStateMachineUndoAdapter.HistoryScope != xWStateMachineUndoAdapter2.HistoryScope && xWStateMachineUndoAdapter.CanUndo && !xWStateMachineUndoAdapter2.CanUndo;
		int secondValue = 0;
		xWStateMachineUndoAdapter2.Commit("second", new StateMachineEditCommand(() =>
		{
			secondValue = 1;
		}, () =>
		{
			secondValue = 0;
		}, Array.Empty<string>()));
		xWStateMachineUndoAdapter2.Undo();
		bool flag = secondValue == 0 && firstValue == 1;
		xWStateMachineUndoAdapter.Undo();
		bool flag2 = firstValue == 0 && secondValue == 0;
		int historyScope = xWStateMachineUndoAdapter.HistoryScope;
		int historyScope2 = xWStateMachineUndoAdapter2.HistoryScope;
		xWStateMachineUndoAdapter.ReleaseHistory();
		xWStateMachineUndoAdapter2.ReleaseHistory();
		bool flag3 = xWStateMachineUndoAdapter.IsHistoryReleased && xWStateMachineUndoAdapter2.IsHistoryReleased && !xWUndoRedoManager.HasHistory(historyScope) && !xWUndoRedoManager.HasHistory(historyScope2) && !xWStateMachineUndoAdapter.CanUndo && !xWStateMachineUndoAdapter2.CanUndo;
		return num & flag & flag2 & flag3;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProbeDerivedResourceIdentity, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ProbeUndoHistoryIsolation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
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
		if (method == MethodName.ProbeDerivedResourceIdentity && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProbeDerivedResourceIdentity());
			return true;
		}
		if (method == MethodName.ProbeUndoHistoryIsolation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProbeUndoHistoryIsolation());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ProbeDerivedResourceIdentity && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProbeDerivedResourceIdentity());
			return true;
		}
		if (method == MethodName.ProbeUndoHistoryIsolation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProbeUndoHistoryIsolation());
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
		if (method == MethodName.ProbeDerivedResourceIdentity)
		{
			return true;
		}
		if (method == MethodName.ProbeUndoHistoryIsolation)
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
