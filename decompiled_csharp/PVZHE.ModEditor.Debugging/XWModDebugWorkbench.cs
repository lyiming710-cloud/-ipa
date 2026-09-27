using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Debugging;

[ScriptPath("res://addons/ModEditor/Debugging/GUI/XWModDebugWorkbench.cs")]
public class XWModDebugWorkbench : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName UnbindController = "UnbindController";

		public static readonly StringName ApplyPendingSnapshotOnMainThread = "ApplyPendingSnapshotOnMainThread";

		public static readonly StringName OnPauseContinuePressed = "OnPauseContinuePressed";

		public static readonly StringName OnStepIntoPressed = "OnStepIntoPressed";

		public static readonly StringName OnStepOverPressed = "OnStepOverPressed";

		public static readonly StringName OnStepOutPressed = "OnStepOutPressed";

		public static readonly StringName OnStopPressed = "OnStopPressed";

		public static readonly StringName ShowCommandResult = "ShowCommandResult";

		public static readonly StringName UpdateProcessingState = "UpdateProcessingState";

		public static readonly StringName UpdateResponsiveLayout = "UpdateResponsiveLayout";

		public static readonly StringName ShowDisconnectedState = "ShowDisconnectedState";

		public static readonly StringName GetStateBadge = "GetStateBadge";

		public static readonly StringName GetStateColor = "GetStateColor";

		public static readonly StringName GetPauseReasonText = "GetPauseReasonText";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName IsControllerBound = "IsControllerBound";

		public static readonly StringName IsHiddenProcessSuspended = "IsHiddenProcessSuspended";

		public static readonly StringName CallStackItemCount = "CallStackItemCount";

		public static readonly StringName VariableItemCount = "VariableItemCount";

		public static readonly StringName StatusText = "StatusText";

		public static readonly StringName CurrentLocationText = "CurrentLocationText";

		public static readonly StringName PauseReasonText = "PauseReasonText";

		public static readonly StringName LastAppliedSequence = "LastAppliedSequence";

		public static readonly StringName VisibleRefreshCount = "VisibleRefreshCount";

		public static readonly StringName _pendingSnapshotGeneration = "_pendingSnapshotGeneration";

		public static readonly StringName _bindingGeneration = "_bindingGeneration";

		public static readonly StringName _deferredRefreshQueued = "_deferredRefreshQueued";

		public static readonly StringName _snapshotDirty = "_snapshotDirty";

		public static readonly StringName _leavingTree = "_leavingTree";

		public static readonly StringName _reconcileElapsed = "_reconcileElapsed";

		public static readonly StringName _stateBadgeLabel = "_stateBadgeLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _locationLabel = "_locationLabel";

		public static readonly StringName _pauseReasonLabel = "_pauseReasonLabel";

		public static readonly StringName _callStackCountLabel = "_callStackCountLabel";

		public static readonly StringName _variableCountLabel = "_variableCountLabel";

		public static readonly StringName _footerLabel = "_footerLabel";

		public static readonly StringName _pauseContinueButton = "_pauseContinueButton";

		public static readonly StringName _stepIntoButton = "_stepIntoButton";

		public static readonly StringName _stepOverButton = "_stepOverButton";

		public static readonly StringName _stepOutButton = "_stepOutButton";

		public static readonly StringName _stopButton = "_stopButton";

		public static readonly StringName _callStackList = "_callStackList";

		public static readonly StringName _variableList = "_variableList";

		public static readonly StringName _dataSplit = "_dataSplit";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private readonly object _pendingSnapshotLock = new object();

	private XWModDebugController _controller;

	private XWModDebugSnapshot _snapshot;

	private XWModDebugSnapshot _pendingSnapshot;

	private int _pendingSnapshotGeneration;

	private int _bindingGeneration;

	private int _deferredRefreshQueued;

	private bool _snapshotDirty;

	private bool _leavingTree;

	private double _reconcileElapsed;

	private Label _stateBadgeLabel;

	private Label _statusLabel;

	private Label _locationLabel;

	private Label _pauseReasonLabel;

	private Label _callStackCountLabel;

	private Label _variableCountLabel;

	private Label _footerLabel;

	private Button _pauseContinueButton;

	private Button _stepIntoButton;

	private Button _stepOverButton;

	private Button _stepOutButton;

	private Button _stopButton;

	private ItemList _callStackList;

	private ItemList _variableList;

	private GridContainer _dataSplit;

	public XWModDebugController Controller => _controller;

	public XWModDebugSnapshot Snapshot => _snapshot;

	public bool IsControllerBound => _controller != null;

	public bool IsHiddenProcessSuspended => !IsProcessing();

	public int CallStackItemCount => _callStackList?.ItemCount ?? 0;

	public int VariableItemCount => _variableList?.ItemCount ?? 0;

	public string StatusText => _statusLabel?.Text ?? "";

	public string CurrentLocationText => _locationLabel?.Text ?? "";

	public string PauseReasonText => _pauseReasonLabel?.Text ?? "";

	public long LastAppliedSequence => _snapshot?.Sequence ?? (-1);

	public int VisibleRefreshCount { get; private set; }

	public event Action StopRequested;

	public override void _Ready()
	{
		_stateBadgeLabel = GetNode<Label>("%StateBadgeLabel");
		_statusLabel = GetNode<Label>("%StatusLabel");
		_locationLabel = GetNode<Label>("%LocationLabel");
		_pauseReasonLabel = GetNode<Label>("%PauseReasonLabel");
		_callStackCountLabel = GetNode<Label>("%CallStackCountLabel");
		_variableCountLabel = GetNode<Label>("%VariableCountLabel");
		_footerLabel = GetNode<Label>("%FooterLabel");
		_pauseContinueButton = GetNode<Button>("%PauseContinueButton");
		_stepIntoButton = GetNode<Button>("%StepIntoButton");
		_stepOverButton = GetNode<Button>("%StepOverButton");
		_stepOutButton = GetNode<Button>("%StepOutButton");
		_stopButton = GetNode<Button>("%StopButton");
		_callStackList = GetNode<ItemList>("%CallStackList");
		_variableList = GetNode<ItemList>("%VariableList");
		_dataSplit = GetNode<GridContainer>("%DataSplit");
		_pauseContinueButton.Pressed += OnPauseContinuePressed;
		_stepIntoButton.Pressed += OnStepIntoPressed;
		_stepOverButton.Pressed += OnStepOverPressed;
		_stepOutButton.Pressed += OnStepOutPressed;
		_stopButton.Pressed += OnStopPressed;
		VisibilityChanged += UpdateProcessingState;
		Resized += UpdateResponsiveLayout;
		if (_snapshot != null)
		{
			RefreshSnapshot(_snapshot);
		}
		else
		{
			ShowDisconnectedState();
		}
		UpdateProcessingState();
		UpdateResponsiveLayout();
	}

	public override void _ExitTree()
	{
		_leavingTree = true;
		Resized -= UpdateResponsiveLayout;
		UnbindController();
		SetProcess(enable: false);
	}

	public override void _Process(double delta)
	{
		if (!IsVisibleInTree() || _controller == null)
		{
			SetProcess(enable: false);
			return;
		}
		if (_snapshotDirty)
		{
			_snapshotDirty = false;
			RefreshSnapshot(_snapshot);
		}
		_reconcileElapsed += delta;
		if (!(_reconcileElapsed < 0.5))
		{
			_reconcileElapsed = 0.0;
			XWModDebugSnapshot snapshot = _controller.Snapshot;
			if (snapshot != null && snapshot.Sequence != (_snapshot?.Sequence ?? (-1)))
			{
				QueueSnapshotFromAnyThread(snapshot, _bindingGeneration);
			}
		}
	}

	public void BindController(XWModDebugController controller)
	{
		if (_controller == controller)
		{
			if (controller != null)
			{
				QueueSnapshotFromAnyThread(controller.Snapshot, _bindingGeneration);
			}
			return;
		}
		UnbindController();
		_leavingTree = false;
		_controller = controller;
		_bindingGeneration++;
		if (_controller != null)
		{
			_controller.SnapshotChanged += OnControllerSnapshotChanged;
			QueueSnapshotFromAnyThread(_controller.Snapshot, _bindingGeneration);
		}
		else if (IsNodeReady())
		{
			ShowDisconnectedState();
		}
		UpdateProcessingState();
	}

	public void UnbindController()
	{
		XWModDebugController controller = _controller;
		_controller = null;
		_bindingGeneration++;
		if (controller != null)
		{
			controller.SnapshotChanged -= OnControllerSnapshotChanged;
		}
		lock (_pendingSnapshotLock)
		{
			_pendingSnapshot = null;
			_pendingSnapshotGeneration = _bindingGeneration;
		}
		Interlocked.Exchange(ref _deferredRefreshQueued, 0);
		_snapshot = null;
		_snapshotDirty = false;
		if (IsNodeReady())
		{
			ShowDisconnectedState();
		}
		SetProcess(enable: false);
	}

	private void OnControllerSnapshotChanged(object sender, XWModDebugSnapshot snapshot)
	{
		XWModDebugController controller = _controller;
		if (controller != null && sender == controller && snapshot != null)
		{
			QueueSnapshotFromAnyThread(snapshot, _bindingGeneration);
		}
	}

	private void QueueSnapshotFromAnyThread(XWModDebugSnapshot snapshot, int generation)
	{
		if (snapshot == null || _leavingTree || generation != _bindingGeneration)
		{
			return;
		}
		lock (_pendingSnapshotLock)
		{
			if (generation != _bindingGeneration)
			{
				return;
			}
			_pendingSnapshot = snapshot;
			_pendingSnapshotGeneration = generation;
		}
		if (Interlocked.Exchange(ref _deferredRefreshQueued, 1) == 0)
		{
			CallDeferred("ApplyPendingSnapshotOnMainThread");
		}
	}

	private void ApplyPendingSnapshotOnMainThread()
	{
		Interlocked.Exchange(ref _deferredRefreshQueued, 0);
		XWModDebugSnapshot pendingSnapshot;
		int pendingSnapshotGeneration;
		lock (_pendingSnapshotLock)
		{
			pendingSnapshot = _pendingSnapshot;
			pendingSnapshotGeneration = _pendingSnapshotGeneration;
			_pendingSnapshot = null;
		}
		if (!_leavingTree && pendingSnapshot != null && pendingSnapshotGeneration == _bindingGeneration && _controller != null)
		{
			_snapshot = pendingSnapshot;
			if (!IsNodeReady() || !IsVisibleInTree())
			{
				_snapshotDirty = true;
			}
			else
			{
				RefreshSnapshot(pendingSnapshot);
			}
		}
	}

	private void RefreshSnapshot(XWModDebugSnapshot snapshot)
	{
		if (IsNodeReady() && snapshot != null)
		{
			VisibleRefreshCount++;
			_stateBadgeLabel.Text = GetStateBadge(snapshot.State);
			_stateBadgeLabel.Modulate = GetStateColor(snapshot.State);
			_statusLabel.Text = (string.IsNullOrWhiteSpace(snapshot.StatusText) ? "等待调试状态" : snapshot.StatusText);
			_locationLabel.Text = FormatCheckpoint(snapshot.CurrentCheckpoint);
			_pauseReasonLabel.Text = "暂停原因：" + GetPauseReasonText(snapshot.PauseReason);
			RefreshCallStack(snapshot);
			RefreshVariables(snapshot);
			UpdateCommandButtons(snapshot);
			_footerLabel.Text = ((snapshot.State == XWModDebugState.Paused) ? "已停在安全检查点，可单步检查调用路径与变量。" : "调试工作在后台异步运行，不会阻塞编辑器主线程。");
			_footerLabel.Modulate = new Color("81b0c4");
		}
	}

	private void RefreshCallStack(XWModDebugSnapshot snapshot)
	{
		_callStackList.Clear();
		for (int i = 0; i < snapshot.CallStack.Count; i++)
		{
			XWModDebugStackFrame xWModDebugStackFrame = snapshot.CallStack[i];
			string text = FormatFrameLocation(xWModDebugStackFrame);
			_callStackList.AddItem("◆ " + xWModDebugStackFrame.DisplayName + "\n   " + text);
			_callStackList.SetItemTooltip(i, $"{xWModDebugStackFrame.DisplayName}\n{text}\n帧编号：{xWModDebugStackFrame.Id}");
		}
		if (_callStackList.ItemCount == 0)
		{
			_callStackList.AddItem("◇ 暂无调用栈；暂停后将在这里显示执行路径");
			_callStackList.SetItemDisabled(0, disabled: true);
		}
		_callStackCountLabel.Text = ((snapshot.CallStack.Count > 0) ? $"{snapshot.CallStack.Count} 帧" : "未暂停");
	}

	private void RefreshVariables(XWModDebugSnapshot snapshot)
	{
		_variableList.Clear();
		int rendered = 0;
		foreach (var (fallbackName, variable) in snapshot.Variables)
		{
			AppendVariable(fallbackName, variable, 0, ref rendered);
		}
		if (rendered == 0)
		{
			_variableList.AddItem("◇ 暂无变量；命中断点后可检查当前上下文");
			_variableList.SetItemDisabled(0, disabled: true);
		}
		_variableCountLabel.Text = ((rendered > 0) ? $"{rendered} 项" : "无数据");
	}

	private void AppendVariable(string fallbackName, XWModDebugVariable variable, int depth, ref int rendered)
	{
		if (variable == null || rendered >= 512)
		{
			return;
		}
		string value = (string.IsNullOrWhiteSpace(variable.Name) ? fallbackName : variable.Name);
		string value2 = (string.IsNullOrWhiteSpace(variable.TypeName) ? "自动" : variable.TypeName);
		string value3 = new string(' ', Math.Min(depth, 12) * 3);
		_variableList.AddItem($"{value3}◆ {value}  :  {value2}\n{value3}   {variable.DisplayValue}");
		int idx = _variableList.ItemCount - 1;
		_variableList.SetItemTooltip(idx, $"{value} ({value2}) = {variable.DisplayValue}");
		rendered++;
		foreach (XWModDebugVariable child in variable.Children)
		{
			AppendVariable(child?.Name ?? "", child, depth + 1, ref rendered);
		}
	}

	private void UpdateCommandButtons(XWModDebugSnapshot snapshot)
	{
		bool flag = snapshot.State == XWModDebugState.Paused;
		_pauseContinueButton.Text = (flag ? "▶ 继续" : "Ⅱ 暂停");
		_pauseContinueButton.TooltipText = (flag ? "继续运行到下一个断点" : "请求在下一个安全检查点暂停");
		_pauseContinueButton.Disabled = (flag ? (!snapshot.CanContinue) : (snapshot.State != XWModDebugState.Running));
		_stepIntoButton.Disabled = !snapshot.CanStep;
		_stepOverButton.Disabled = !snapshot.CanStep;
		_stepOutButton.Disabled = !snapshot.CanStep;
		_stopButton.Disabled = !snapshot.CanStop;
	}

	private void OnPauseContinuePressed()
	{
		if (_controller != null && _snapshot != null)
		{
			bool accepted = ((_snapshot.State == XWModDebugState.Paused) ? _controller.Continue() : _controller.RequestPause());
			ShowCommandResult(accepted, (_snapshot.State == XWModDebugState.Paused) ? "继续运行" : "请求暂停");
		}
	}

	private void OnStepIntoPressed()
	{
		ShowCommandResult(_controller?.StepInto() ?? false, "单步进入");
	}

	private void OnStepOverPressed()
	{
		ShowCommandResult(_controller?.StepOver() ?? false, "单步跳过");
	}

	private void OnStepOutPressed()
	{
		ShowCommandResult(_controller?.StepOut() ?? false, "单步跳出");
	}

	private void OnStopPressed()
	{
		if (_controller != null)
		{
			Action action = StopRequested;
			if (action != null)
			{
				action();
				_footerLabel.Text = "已向宿主请求停止调试。";
			}
			else
			{
				ShowCommandResult(_controller.Stop(), "停止调试");
			}
		}
	}

	private void ShowCommandResult(bool accepted, string action)
	{
		_footerLabel.Text = (accepted ? ("已发送“" + action + "”命令。") : ("当前调试状态不允许“" + action + "”。"));
		_footerLabel.Modulate = (accepted ? new Color("9ddfb0") : new Color("ff9a80"));
	}

	private void UpdateProcessingState()
	{
		bool flag = IsInsideTree() && IsVisibleInTree() && _controller != null;
		SetProcess(flag);
		if (flag)
		{
			UpdateResponsiveLayout();
		}
		if (flag && _snapshotDirty)
		{
			_snapshotDirty = false;
			RefreshSnapshot(_snapshot);
		}
	}

	private void UpdateResponsiveLayout()
	{
		if (GodotObject.IsInstanceValid(_dataSplit))
		{
			_dataSplit.Columns = ((Size.X < 700f) ? 1 : 2);
		}
	}

	private void ShowDisconnectedState()
	{
		if (IsNodeReady())
		{
			_stateBadgeLabel.Text = "◇ 未连接";
			_stateBadgeLabel.Modulate = new Color("8ba0aa");
			_statusLabel.Text = "等待 C# 或蓝图调试会话";
			_locationLabel.Text = "当前位置：尚未进入调试检查点";
			_pauseReasonLabel.Text = "暂停原因：无";
			_callStackList.Clear();
			_callStackList.AddItem("◇ 绑定调试控制器后显示调用栈");
			_callStackList.SetItemDisabled(0, disabled: true);
			_variableList.Clear();
			_variableList.AddItem("◇ 暂无变量监视");
			_variableList.SetItemDisabled(0, disabled: true);
			_callStackCountLabel.Text = "未连接";
			_variableCountLabel.Text = "未连接";
			_footerLabel.Text = "共享工作台可由 C# 脚本编辑器与蓝图编辑器复用。";
			Button[] array = new Button[5] { _pauseContinueButton, _stepIntoButton, _stepOverButton, _stepOutButton, _stopButton };
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Disabled = true;
			}
		}
	}

	private static string FormatCheckpoint(XWModDebugCheckpoint checkpoint)
	{
		if (checkpoint == null)
		{
			return "当前位置：等待下一个安全检查点";
		}
		if (checkpoint.Kind != XWModDebugCheckpointKind.CSharpLine)
		{
			return $"当前位置：{checkpoint.DisplayName} · 蓝图节点 {checkpoint.BlueprintNodeId}\n{checkpoint.SourcePath}";
		}
		return $"当前位置：{checkpoint.DisplayName} · {checkpoint.SourcePath} 第 {checkpoint.Line} 行";
	}

	private static string FormatFrameLocation(XWModDebugStackFrame frame)
	{
		if (frame == null)
		{
			return "未知调用位置";
		}
		if (frame.Line > 0)
		{
			return $"{frame.SourcePath} 第 {frame.Line} 行";
		}
		if (!string.IsNullOrWhiteSpace(frame.BlueprintNodeId))
		{
			return frame.SourcePath + " · 节点 " + frame.BlueprintNodeId;
		}
		if (!string.IsNullOrWhiteSpace(frame.SourcePath))
		{
			return frame.SourcePath;
		}
		return "内部调用帧";
	}

	private static string GetStateBadge(XWModDebugState state)
	{
		return state switch
		{
			XWModDebugState.Running => "▶ 运行中", 
			XWModDebugState.PauseRequested => "◌ 等待暂停", 
			XWModDebugState.Paused => "Ⅱ 已暂停", 
			XWModDebugState.Stopped => "■ 已停止", 
			_ => "◇ 未知状态", 
		};
	}

	private static Color GetStateColor(XWModDebugState state)
	{
		return state switch
		{
			XWModDebugState.Running => new Color("78e196"), 
			XWModDebugState.PauseRequested => new Color("ffd36d"), 
			XWModDebugState.Paused => new Color("79c8ff"), 
			XWModDebugState.Stopped => new Color("ff8b77"), 
			_ => new Color("9babb3"), 
		};
	}

	private static string GetPauseReasonText(XWModDebugPauseReason reason)
	{
		return reason switch
		{
			XWModDebugPauseReason.None => "无", 
			XWModDebugPauseReason.PauseRequest => "手动暂停请求", 
			XWModDebugPauseReason.Breakpoint => "命中断点", 
			XWModDebugPauseReason.ForcedBreakpoint => "强制断点", 
			XWModDebugPauseReason.Step => "单步执行完成", 
			_ => "未知原因", 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UnbindController, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingSnapshotOnMainThread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPauseContinuePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStepIntoPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStepOverPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStepOutPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStopPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCommandResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "accepted", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessingState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateResponsiveLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowDisconnectedState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetStateBadge, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetStateColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPauseReasonText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindController && args.Count == 0)
		{
			UnbindController();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingSnapshotOnMainThread && args.Count == 0)
		{
			ApplyPendingSnapshotOnMainThread();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPauseContinuePressed && args.Count == 0)
		{
			OnPauseContinuePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStepIntoPressed && args.Count == 0)
		{
			OnStepIntoPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStepOverPressed && args.Count == 0)
		{
			OnStepOverPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStepOutPressed && args.Count == 0)
		{
			OnStepOutPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStopPressed && args.Count == 0)
		{
			OnStopPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCommandResult && args.Count == 2)
		{
			ShowCommandResult(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessingState && args.Count == 0)
		{
			UpdateProcessingState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateResponsiveLayout && args.Count == 0)
		{
			UpdateResponsiveLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowDisconnectedState && args.Count == 0)
		{
			ShowDisconnectedState();
			ret = default;
			return true;
		}
		if (method == MethodName.GetStateBadge && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetStateBadge(VariantUtils.ConvertTo<XWModDebugState>(in args[0])));
			return true;
		}
		if (method == MethodName.GetStateColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetStateColor(VariantUtils.ConvertTo<XWModDebugState>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPauseReasonText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPauseReasonText(VariantUtils.ConvertTo<XWModDebugPauseReason>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetStateBadge && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetStateBadge(VariantUtils.ConvertTo<XWModDebugState>(in args[0])));
			return true;
		}
		if (method == MethodName.GetStateColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetStateColor(VariantUtils.ConvertTo<XWModDebugState>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPauseReasonText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPauseReasonText(VariantUtils.ConvertTo<XWModDebugPauseReason>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.UnbindController)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingSnapshotOnMainThread)
		{
			return true;
		}
		if (method == MethodName.OnPauseContinuePressed)
		{
			return true;
		}
		if (method == MethodName.OnStepIntoPressed)
		{
			return true;
		}
		if (method == MethodName.OnStepOverPressed)
		{
			return true;
		}
		if (method == MethodName.OnStepOutPressed)
		{
			return true;
		}
		if (method == MethodName.OnStopPressed)
		{
			return true;
		}
		if (method == MethodName.ShowCommandResult)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessingState)
		{
			return true;
		}
		if (method == MethodName.UpdateResponsiveLayout)
		{
			return true;
		}
		if (method == MethodName.ShowDisconnectedState)
		{
			return true;
		}
		if (method == MethodName.GetStateBadge)
		{
			return true;
		}
		if (method == MethodName.GetStateColor)
		{
			return true;
		}
		if (method == MethodName.GetPauseReasonText)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.VisibleRefreshCount)
		{
			VisibleRefreshCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pendingSnapshotGeneration)
		{
			_pendingSnapshotGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bindingGeneration)
		{
			_bindingGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._deferredRefreshQueued)
		{
			_deferredRefreshQueued = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._snapshotDirty)
		{
			_snapshotDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._leavingTree)
		{
			_leavingTree = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._reconcileElapsed)
		{
			_reconcileElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._stateBadgeLabel)
		{
			_stateBadgeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._locationLabel)
		{
			_locationLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pauseReasonLabel)
		{
			_pauseReasonLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._callStackCountLabel)
		{
			_callStackCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._variableCountLabel)
		{
			_variableCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._footerLabel)
		{
			_footerLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pauseContinueButton)
		{
			_pauseContinueButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stepIntoButton)
		{
			_stepIntoButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stepOverButton)
		{
			_stepOverButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stepOutButton)
		{
			_stepOutButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stopButton)
		{
			_stopButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._callStackList)
		{
			_callStackList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._variableList)
		{
			_variableList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._dataSplit)
		{
			_dataSplit = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsControllerBound)
		{
			from = IsControllerBound;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsHiddenProcessSuspended)
		{
			from = IsHiddenProcessSuspended;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.CallStackItemCount)
		{
			from2 = CallStackItemCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.VariableItemCount)
		{
			from2 = VariableItemCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		string from3;
		if (name == PropertyName.StatusText)
		{
			from3 = StatusText;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CurrentLocationText)
		{
			from3 = CurrentLocationText;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.PauseReasonText)
		{
			from3 = PauseReasonText;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastAppliedSequence)
		{
			value = VariantUtils.CreateFrom<long>(LastAppliedSequence);
			return true;
		}
		if (name == PropertyName.VisibleRefreshCount)
		{
			from2 = VisibleRefreshCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._pendingSnapshotGeneration)
		{
			value = VariantUtils.CreateFrom(in _pendingSnapshotGeneration);
			return true;
		}
		if (name == PropertyName._bindingGeneration)
		{
			value = VariantUtils.CreateFrom(in _bindingGeneration);
			return true;
		}
		if (name == PropertyName._deferredRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _deferredRefreshQueued);
			return true;
		}
		if (name == PropertyName._snapshotDirty)
		{
			value = VariantUtils.CreateFrom(in _snapshotDirty);
			return true;
		}
		if (name == PropertyName._leavingTree)
		{
			value = VariantUtils.CreateFrom(in _leavingTree);
			return true;
		}
		if (name == PropertyName._reconcileElapsed)
		{
			value = VariantUtils.CreateFrom(in _reconcileElapsed);
			return true;
		}
		if (name == PropertyName._stateBadgeLabel)
		{
			value = VariantUtils.CreateFrom(in _stateBadgeLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._locationLabel)
		{
			value = VariantUtils.CreateFrom(in _locationLabel);
			return true;
		}
		if (name == PropertyName._pauseReasonLabel)
		{
			value = VariantUtils.CreateFrom(in _pauseReasonLabel);
			return true;
		}
		if (name == PropertyName._callStackCountLabel)
		{
			value = VariantUtils.CreateFrom(in _callStackCountLabel);
			return true;
		}
		if (name == PropertyName._variableCountLabel)
		{
			value = VariantUtils.CreateFrom(in _variableCountLabel);
			return true;
		}
		if (name == PropertyName._footerLabel)
		{
			value = VariantUtils.CreateFrom(in _footerLabel);
			return true;
		}
		if (name == PropertyName._pauseContinueButton)
		{
			value = VariantUtils.CreateFrom(in _pauseContinueButton);
			return true;
		}
		if (name == PropertyName._stepIntoButton)
		{
			value = VariantUtils.CreateFrom(in _stepIntoButton);
			return true;
		}
		if (name == PropertyName._stepOverButton)
		{
			value = VariantUtils.CreateFrom(in _stepOverButton);
			return true;
		}
		if (name == PropertyName._stepOutButton)
		{
			value = VariantUtils.CreateFrom(in _stepOutButton);
			return true;
		}
		if (name == PropertyName._stopButton)
		{
			value = VariantUtils.CreateFrom(in _stopButton);
			return true;
		}
		if (name == PropertyName._callStackList)
		{
			value = VariantUtils.CreateFrom(in _callStackList);
			return true;
		}
		if (name == PropertyName._variableList)
		{
			value = VariantUtils.CreateFrom(in _variableList);
			return true;
		}
		if (name == PropertyName._dataSplit)
		{
			value = VariantUtils.CreateFrom(in _dataSplit);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingSnapshotGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bindingGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._deferredRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._snapshotDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._leavingTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._reconcileElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stateBadgeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._locationLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pauseReasonLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._callStackCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._variableCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._footerLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pauseContinueButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepIntoButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepOverButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepOutButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._callStackList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._variableList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dataSplit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsControllerBound, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHiddenProcessSuspended, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CallStackItemCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VariableItemCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.StatusText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentLocationText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.PauseReasonText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastAppliedSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.VisibleRefreshCount, Variant.From<int>(VisibleRefreshCount));
		info.AddProperty(PropertyName._pendingSnapshotGeneration, Variant.From(in _pendingSnapshotGeneration));
		info.AddProperty(PropertyName._bindingGeneration, Variant.From(in _bindingGeneration));
		info.AddProperty(PropertyName._deferredRefreshQueued, Variant.From(in _deferredRefreshQueued));
		info.AddProperty(PropertyName._snapshotDirty, Variant.From(in _snapshotDirty));
		info.AddProperty(PropertyName._leavingTree, Variant.From(in _leavingTree));
		info.AddProperty(PropertyName._reconcileElapsed, Variant.From(in _reconcileElapsed));
		info.AddProperty(PropertyName._stateBadgeLabel, Variant.From(in _stateBadgeLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._locationLabel, Variant.From(in _locationLabel));
		info.AddProperty(PropertyName._pauseReasonLabel, Variant.From(in _pauseReasonLabel));
		info.AddProperty(PropertyName._callStackCountLabel, Variant.From(in _callStackCountLabel));
		info.AddProperty(PropertyName._variableCountLabel, Variant.From(in _variableCountLabel));
		info.AddProperty(PropertyName._footerLabel, Variant.From(in _footerLabel));
		info.AddProperty(PropertyName._pauseContinueButton, Variant.From(in _pauseContinueButton));
		info.AddProperty(PropertyName._stepIntoButton, Variant.From(in _stepIntoButton));
		info.AddProperty(PropertyName._stepOverButton, Variant.From(in _stepOverButton));
		info.AddProperty(PropertyName._stepOutButton, Variant.From(in _stepOutButton));
		info.AddProperty(PropertyName._stopButton, Variant.From(in _stopButton));
		info.AddProperty(PropertyName._callStackList, Variant.From(in _callStackList));
		info.AddProperty(PropertyName._variableList, Variant.From(in _variableList));
		info.AddProperty(PropertyName._dataSplit, Variant.From(in _dataSplit));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.VisibleRefreshCount, out var value))
		{
			VisibleRefreshCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pendingSnapshotGeneration, out var value2))
		{
			_pendingSnapshotGeneration = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bindingGeneration, out var value3))
		{
			_bindingGeneration = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._deferredRefreshQueued, out var value4))
		{
			_deferredRefreshQueued = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._snapshotDirty, out var value5))
		{
			_snapshotDirty = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._leavingTree, out var value6))
		{
			_leavingTree = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._reconcileElapsed, out var value7))
		{
			_reconcileElapsed = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._stateBadgeLabel, out var value8))
		{
			_stateBadgeLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value9))
		{
			_statusLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._locationLabel, out var value10))
		{
			_locationLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pauseReasonLabel, out var value11))
		{
			_pauseReasonLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._callStackCountLabel, out var value12))
		{
			_callStackCountLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._variableCountLabel, out var value13))
		{
			_variableCountLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._footerLabel, out var value14))
		{
			_footerLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pauseContinueButton, out var value15))
		{
			_pauseContinueButton = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stepIntoButton, out var value16))
		{
			_stepIntoButton = value16.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stepOverButton, out var value17))
		{
			_stepOverButton = value17.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stepOutButton, out var value18))
		{
			_stepOutButton = value18.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stopButton, out var value19))
		{
			_stopButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._callStackList, out var value20))
		{
			_callStackList = value20.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._variableList, out var value21))
		{
			_variableList = value21.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._dataSplit, out var value22))
		{
			_dataSplit = value22.As<GridContainer>();
		}
	}
}
