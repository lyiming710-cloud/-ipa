using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWSurvivalPreviewWindow.cs")]
public class XWSurvivalPreviewWindow : Window
{
	public new class MethodName : Window.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Preview = "Preview";

		public static readonly StringName RefreshConfig = "RefreshConfig";

		public static readonly StringName BindInterface = "BindInterface";

		public static readonly StringName ResetSimulation = "ResetSimulation";

		public static readonly StringName AdvanceWave = "AdvanceWave";

		public static readonly StringName AdvanceRound = "AdvanceRound";

		public static readonly StringName RecalculateFromControls = "RecalculateFromControls";

		public static readonly StringName UpdateControls = "UpdateControls";

		public static readonly StringName RenderSimulation = "RenderSimulation";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName OnPreviewVisibilityChanged = "OnPreviewVisibilityChanged";

		public static readonly StringName RefreshPreviewProcessing = "RefreshPreviewProcessing";

		public static readonly StringName SetPreviewTreeProcessing = "SetPreviewTreeProcessing";

		public static readonly StringName ForgetPreviewTreeModes = "ForgetPreviewTreeModes";

		public static readonly StringName CreateChip = "CreateChip";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName _config = "_config";

		public static readonly StringName _round = "_round";

		public static readonly StringName _wave = "_wave";

		public static readonly StringName _points = "_points";

		public static readonly StringName _roundLabel = "_roundLabel";

		public static readonly StringName _waveLabel = "_waveLabel";

		public static readonly StringName _timeLabel = "_timeLabel";

		public static readonly StringName _pointLabel = "_pointLabel";

		public static readonly StringName _pointProgress = "_pointProgress";

		public static readonly StringName _zombiePool = "_zombiePool";

		public static readonly StringName _roundAdds = "_roundAdds";

		public static readonly StringName _zombiePreviewRoot = "_zombiePreviewRoot";

		public static readonly StringName _gameBackground = "_gameBackground";

		public static readonly StringName _roundSpin = "_roundSpin";

		public static readonly StringName _waveSpin = "_waveSpin";

		public static readonly StringName _bigWaveCheck = "_bigWaveCheck";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : Window.SignalName
	{
	}

	private TowerDefenseLevelSurvivalConfig _config;

	private int _round;

	private int _wave;

	private long _points;

	private Label _roundLabel;

	private Label _waveLabel;

	private Label _timeLabel;

	private Label _pointLabel;

	private ProgressBar _pointProgress;

	private FlowContainer _zombiePool;

	private FlowContainer _roundAdds;

	private Node2D _zombiePreviewRoot;

	private TextureRect _gameBackground;

	private readonly List<Node2D> _runtimeZombies = new List<Node2D>();

	private readonly Dictionary<ulong, ProcessModeEnum> _runtimeProcessModes = new Dictionary<ulong, ProcessModeEnum>();

	private SpinBox _roundSpin;

	private SpinBox _waveSpin;

	private CheckBox _bigWaveCheck;

	private bool _updatingControls;

	public override void _Ready()
	{
		SetProcess(enable: false);
		CloseRequested += Hide;
		VisibilityChanged += OnPreviewVisibilityChanged;
		BindInterface();
		ResetSimulation();
		RefreshPreviewProcessing();
	}

	public override void _ExitTree()
	{
		VisibilityChanged -= OnPreviewVisibilityChanged;
		SetProcess(enable: false);
		_runtimeZombies.Clear();
		_runtimeProcessModes.Clear();
		base._ExitTree();
	}

	public void Preview(TowerDefenseLevelSurvivalConfig config)
	{
		_config = config;
		if (IsNodeReady())
		{
			ResetSimulation();
			PopupCenteredClamped(new Vector2I(900, 620), 0.9f);
		}
	}

	public void RefreshConfig(TowerDefenseLevelSurvivalConfig config)
	{
		_config = config;
		if (IsNodeReady())
		{
			RecalculateFromControls();
		}
	}

	private void BindInterface()
	{
		_roundLabel = GetNode<Label>("PreviewScroll/Layout/Stage/Content/Counters/Round");
		_waveLabel = GetNode<Label>("PreviewScroll/Layout/Stage/Content/Counters/Wave");
		_timeLabel = GetNode<Label>("PreviewScroll/Layout/Stage/Content/Counters/Time");
		_pointLabel = GetNode<Label>("PreviewScroll/Layout/Stage/Content/PointLabel");
		_pointProgress = GetNode<ProgressBar>("PreviewScroll/Layout/Stage/Content/PointProgress");
		_roundSpin = GetNode<SpinBox>("PreviewScroll/Layout/Selector/RoundSpin");
		_waveSpin = GetNode<SpinBox>("PreviewScroll/Layout/Selector/WaveSpin");
		_bigWaveCheck = GetNode<CheckBox>("PreviewScroll/Layout/Selector/BigWaveCheck");
		_zombiePool = GetNode<FlowContainer>("PreviewScroll/Layout/ZombiePoolScroll/ZombiePool");
		_roundAdds = GetNode<FlowContainer>("PreviewScroll/Layout/RoundAddsScroll/RoundAdds");
		_zombiePreviewRoot = GetNode<Node2D>("%ZombieRoot");
		_gameBackground = GetNode<TextureRect>("%GameBackground");
		_roundSpin.ValueChanged += (double _) =>
		{
			RecalculateFromControls();
		};
		_waveSpin.ValueChanged += (double _) =>
		{
			RecalculateFromControls();
		};
		Button node = GetNode<Button>("PreviewScroll/Layout/Controls/ResetButton");
		Button node2 = GetNode<Button>("PreviewScroll/Layout/Controls/NextWaveButton");
		Button node3 = GetNode<Button>("PreviewScroll/Layout/Controls/NextRoundButton");
		node.Pressed += ResetSimulation;
		node2.Pressed += AdvanceWave;
		node3.Pressed += AdvanceRound;
	}

	private void ResetSimulation()
	{
		_round = 0;
		_wave = 0;
		_points = _config?.pointBegin ?? 0;
		UpdateControls();
		RenderSimulation();
	}

	private void AdvanceWave()
	{
		if (GodotObject.IsInstanceValid(_config))
		{
			_wave++;
			_points += (_bigWaveCheck.ButtonPressed ? _config.pointIncrementPerBigWave : _config.pointIncrementPerWave);
			_points = Math.Min(_points, _config.pointMax);
			_bigWaveCheck.ButtonPressed = false;
			UpdateControls();
			RenderSimulation();
		}
	}

	private void AdvanceRound()
	{
		if (GodotObject.IsInstanceValid(_config) && (_config.roundLimit < 0 || _round < _config.roundLimit))
		{
			_round++;
			_points += _config.pointIncrementPerRound;
			_points = Math.Min(_points, _config.pointMax);
			UpdateControls();
			RenderSimulation();
		}
	}

	private void RecalculateFromControls()
	{
		if (!_updatingControls && GodotObject.IsInstanceValid(_config))
		{
			_round = (int)_roundSpin.Value;
			_wave = (int)_waveSpin.Value;
			_points = _config.pointBegin + (long)_round * (long)_config.pointIncrementPerRound + (long)_wave * (long)_config.pointIncrementPerWave;
			_points = Math.Min(_points, _config.pointMax);
			RenderSimulation();
		}
	}

	private void UpdateControls()
	{
		_updatingControls = true;
		_roundSpin.Value = _round;
		_waveSpin.Value = _wave;
		SpinBox roundSpin = _roundSpin;
		TowerDefenseLevelSurvivalConfig config = _config;
		roundSpin.MaxValue = ((config != null && config.roundLimit >= 0) ? _config.roundLimit : 999);
		_updatingControls = false;
	}

	private void RenderSimulation()
	{
		foreach (Node child in _zombiePool.GetChildren())
		{
			child.QueueFree();
		}
		foreach (Node child2 in _roundAdds.GetChildren())
		{
			child2.QueueFree();
		}
		if (!GodotObject.IsInstanceValid(_config))
		{
			return;
		}
		_roundLabel.Text = ((_config.roundLimit < 0) ? $"轮次 {_round} / 无尽" : $"轮次 {_round} / {_config.roundLimit}");
		_waveLabel.Text = $"波次 {_wave}";
		_timeLabel.Text = ((!_config.roundDayNightChange || _round % 2 == 0) ? "白天" : "黑夜");
		_timeLabel.AddThemeColorOverride("font_color", (_timeLabel.Text == "白天") ? new Color(1f, 0.88f, 0.38f) : new Color(0.55f, 0.68f, 1f));
		_gameBackground.Modulate = ((_timeLabel.Text == "白天") ? Colors.White : new Color(0.36f, 0.48f, 0.72f));
		_pointLabel.Text = $"生成点数 {_points:N0} / {_config.pointMax:N0}    大波加成 ×{_config.pointBigWaveScale:0.##}";
		_pointProgress.MinValue = 0.0;
		_pointProgress.MaxValue = Math.Max(1, _config.pointMax);
		_pointProgress.Value = _points;
		List<string> list = new List<string>();
		foreach (Variant item in _config.zombiePoolBase)
		{
			list.Add(item.AsString());
		}
		foreach (TowerDefenseLevelSurvivalZombiePoolRoundAddConfig item2 in _config.zombiePoolRoundAdd)
		{
			if (!GodotObject.IsInstanceValid(item2))
			{
				continue;
			}
			bool flag = item2.round <= _round;
			_roundAdds.AddChild(CreateChip($"第 {item2.round} 轮: {item2.zombieList.Count} 种", flag), forceReadableName: false, InternalMode.Disabled);
			if (!flag)
			{
				continue;
			}
			foreach (Variant zombie in item2.zombieList)
			{
				list.Add(zombie.AsString());
			}
		}
		foreach (string item3 in list)
		{
			_zombiePool.AddChild(CreateChip(string.IsNullOrWhiteSpace(item3) ? "未设置" : item3, active: true), forceReadableName: false, InternalMode.Disabled);
		}
		if (list.Count == 0)
		{
			_zombiePool.AddChild(new Label
			{
				Text = "僵尸池为空"
			}, forceReadableName: false, InternalMode.Disabled);
		}
		RebuildRuntimeZombieWave(list);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		float num = 34f + (float)Mathf.Min(_round, 20) * 2f;
		foreach (Node2D runtimeZombie in _runtimeZombies)
		{
			if (GodotObject.IsInstanceValid(runtimeZombie))
			{
				runtimeZombie.Position += Vector2.Left * num * (float)delta;
				if (runtimeZombie.Position.X < -80f)
				{
					runtimeZombie.Position = new Vector2(960f, runtimeZombie.Position.Y);
				}
			}
		}
	}

	private void RebuildRuntimeZombieWave(List<string> pool)
	{
		if (!GodotObject.IsInstanceValid(_zombiePreviewRoot))
		{
			RefreshPreviewProcessing();
			return;
		}
		foreach (Node child in _zombiePreviewRoot.GetChildren())
		{
			ForgetPreviewTreeModes(child);
			child.QueueFree();
		}
		_runtimeZombies.Clear();
		int num = Math.Min(pool?.Count ?? 0, 7);
		for (int i = 0; i < num; i++)
		{
			string characterName = pool[i];
			PackedScene packedScene = ResourceManager.Instance?.GetCharacterScene(characterName);
			if (packedScene == null)
			{
				packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Puzzle/Target/Scene/TowerDefenseZombieTarget.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			if (GodotObject.IsInstanceValid(packedScene))
			{
				Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
				if (!(node is TowerDefenseCharacter towerDefenseCharacter))
				{
					node.Free();
					continue;
				}
				towerDefenseCharacter.inGame = false;
				towerDefenseCharacter.editorPreviewMode = true;
				towerDefenseCharacter.Position = new Vector2(760f + (float)i * 115f, 145f + (float)(i % 3) * 45f);
				towerDefenseCharacter.Scale = Vector2.One * 0.78f;
				_zombiePreviewRoot.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
				_runtimeZombies.Add(towerDefenseCharacter);
			}
		}
		RefreshPreviewProcessing();
	}

	private void OnPreviewVisibilityChanged()
	{
		RefreshPreviewProcessing();
	}

	private void RefreshPreviewProcessing()
	{
		for (int num = _runtimeZombies.Count - 1; num >= 0; num--)
		{
			if (!GodotObject.IsInstanceValid(_runtimeZombies[num]))
			{
				_runtimeZombies.RemoveAt(num);
			}
		}
		bool flag = Visible && _runtimeZombies.Count > 0;
		SetProcess(flag);
		SetPreviewTreeProcessing(_zombiePreviewRoot, flag);
	}

	private void SetPreviewTreeProcessing(Node node, bool active)
	{
		if (!GodotObject.IsInstanceValid(node) || node.IsQueuedForDeletion())
		{
			return;
		}
		ulong instanceId = node.GetInstanceId();
		if (active)
		{
			if (_runtimeProcessModes.TryGetValue(instanceId, out var value))
			{
				node.ProcessMode = value;
			}
		}
		else
		{
			_runtimeProcessModes.TryAdd(instanceId, node.ProcessMode);
			node.ProcessMode = ProcessModeEnum.Disabled;
		}
		foreach (Node child in node.GetChildren())
		{
			SetPreviewTreeProcessing(child, active);
		}
	}

	private void ForgetPreviewTreeModes(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		foreach (Node child in node.GetChildren())
		{
			ForgetPreviewTreeModes(child);
		}
		_runtimeProcessModes.Remove(node.GetInstanceId());
	}

	private static Control CreateChip(string text, bool active)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.CustomMinimumSize = new Vector2(135f, 34f);
		panelContainer.Modulate = (active ? Colors.White : new Color(1f, 1f, 1f, 0.4f));
		panelContainer.AddChild(new Label
		{
			Text = text,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		}, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Preview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindInterface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceRound, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RecalculateFromControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPreviewVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPreviewProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPreviewTreeProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ForgetPreviewTreeModes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateChip, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Preview && args.Count == 1)
		{
			Preview(VariantUtils.ConvertTo<TowerDefenseLevelSurvivalConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConfig && args.Count == 1)
		{
			RefreshConfig(VariantUtils.ConvertTo<TowerDefenseLevelSurvivalConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindInterface && args.Count == 0)
		{
			BindInterface();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetSimulation && args.Count == 0)
		{
			ResetSimulation();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceWave && args.Count == 0)
		{
			AdvanceWave();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceRound && args.Count == 0)
		{
			AdvanceRound();
			ret = default;
			return true;
		}
		if (method == MethodName.RecalculateFromControls && args.Count == 0)
		{
			RecalculateFromControls();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateControls && args.Count == 0)
		{
			UpdateControls();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderSimulation && args.Count == 0)
		{
			RenderSimulation();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewVisibilityChanged && args.Count == 0)
		{
			OnPreviewVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPreviewProcessing && args.Count == 0)
		{
			RefreshPreviewProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPreviewTreeProcessing && args.Count == 2)
		{
			SetPreviewTreeProcessing(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ForgetPreviewTreeModes && args.Count == 1)
		{
			ForgetPreviewTreeModes(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateChip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateChip(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateChip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateChip(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
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
		if (method == MethodName.Preview)
		{
			return true;
		}
		if (method == MethodName.RefreshConfig)
		{
			return true;
		}
		if (method == MethodName.BindInterface)
		{
			return true;
		}
		if (method == MethodName.ResetSimulation)
		{
			return true;
		}
		if (method == MethodName.AdvanceWave)
		{
			return true;
		}
		if (method == MethodName.AdvanceRound)
		{
			return true;
		}
		if (method == MethodName.RecalculateFromControls)
		{
			return true;
		}
		if (method == MethodName.UpdateControls)
		{
			return true;
		}
		if (method == MethodName.RenderSimulation)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.OnPreviewVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshPreviewProcessing)
		{
			return true;
		}
		if (method == MethodName.SetPreviewTreeProcessing)
		{
			return true;
		}
		if (method == MethodName.ForgetPreviewTreeModes)
		{
			return true;
		}
		if (method == MethodName.CreateChip)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<TowerDefenseLevelSurvivalConfig>(in value);
			return true;
		}
		if (name == PropertyName._round)
		{
			_round = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._wave)
		{
			_wave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._points)
		{
			_points = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._roundLabel)
		{
			_roundLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._waveLabel)
		{
			_waveLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._timeLabel)
		{
			_timeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pointLabel)
		{
			_pointLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pointProgress)
		{
			_pointProgress = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._zombiePool)
		{
			_zombiePool = VariantUtils.ConvertTo<FlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._roundAdds)
		{
			_roundAdds = VariantUtils.ConvertTo<FlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._zombiePreviewRoot)
		{
			_zombiePreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._gameBackground)
		{
			_gameBackground = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._roundSpin)
		{
			_roundSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._waveSpin)
		{
			_waveSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._bigWaveCheck)
		{
			_bigWaveCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._round)
		{
			value = VariantUtils.CreateFrom(in _round);
			return true;
		}
		if (name == PropertyName._wave)
		{
			value = VariantUtils.CreateFrom(in _wave);
			return true;
		}
		if (name == PropertyName._points)
		{
			value = VariantUtils.CreateFrom(in _points);
			return true;
		}
		if (name == PropertyName._roundLabel)
		{
			value = VariantUtils.CreateFrom(in _roundLabel);
			return true;
		}
		if (name == PropertyName._waveLabel)
		{
			value = VariantUtils.CreateFrom(in _waveLabel);
			return true;
		}
		if (name == PropertyName._timeLabel)
		{
			value = VariantUtils.CreateFrom(in _timeLabel);
			return true;
		}
		if (name == PropertyName._pointLabel)
		{
			value = VariantUtils.CreateFrom(in _pointLabel);
			return true;
		}
		if (name == PropertyName._pointProgress)
		{
			value = VariantUtils.CreateFrom(in _pointProgress);
			return true;
		}
		if (name == PropertyName._zombiePool)
		{
			value = VariantUtils.CreateFrom(in _zombiePool);
			return true;
		}
		if (name == PropertyName._roundAdds)
		{
			value = VariantUtils.CreateFrom(in _roundAdds);
			return true;
		}
		if (name == PropertyName._zombiePreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _zombiePreviewRoot);
			return true;
		}
		if (name == PropertyName._gameBackground)
		{
			value = VariantUtils.CreateFrom(in _gameBackground);
			return true;
		}
		if (name == PropertyName._roundSpin)
		{
			value = VariantUtils.CreateFrom(in _roundSpin);
			return true;
		}
		if (name == PropertyName._waveSpin)
		{
			value = VariantUtils.CreateFrom(in _waveSpin);
			return true;
		}
		if (name == PropertyName._bigWaveCheck)
		{
			value = VariantUtils.CreateFrom(in _bigWaveCheck);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._round, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._wave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._points, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._roundLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pointLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pointProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombiePool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._roundAdds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombiePreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gameBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._roundSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._waveSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bigWaveCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._round, Variant.From(in _round));
		info.AddProperty(PropertyName._wave, Variant.From(in _wave));
		info.AddProperty(PropertyName._points, Variant.From(in _points));
		info.AddProperty(PropertyName._roundLabel, Variant.From(in _roundLabel));
		info.AddProperty(PropertyName._waveLabel, Variant.From(in _waveLabel));
		info.AddProperty(PropertyName._timeLabel, Variant.From(in _timeLabel));
		info.AddProperty(PropertyName._pointLabel, Variant.From(in _pointLabel));
		info.AddProperty(PropertyName._pointProgress, Variant.From(in _pointProgress));
		info.AddProperty(PropertyName._zombiePool, Variant.From(in _zombiePool));
		info.AddProperty(PropertyName._roundAdds, Variant.From(in _roundAdds));
		info.AddProperty(PropertyName._zombiePreviewRoot, Variant.From(in _zombiePreviewRoot));
		info.AddProperty(PropertyName._gameBackground, Variant.From(in _gameBackground));
		info.AddProperty(PropertyName._roundSpin, Variant.From(in _roundSpin));
		info.AddProperty(PropertyName._waveSpin, Variant.From(in _waveSpin));
		info.AddProperty(PropertyName._bigWaveCheck, Variant.From(in _bigWaveCheck));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._config, out var value))
		{
			_config = value.As<TowerDefenseLevelSurvivalConfig>();
		}
		if (info.TryGetProperty(PropertyName._round, out var value2))
		{
			_round = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._wave, out var value3))
		{
			_wave = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._points, out var value4))
		{
			_points = value4.As<long>();
		}
		if (info.TryGetProperty(PropertyName._roundLabel, out var value5))
		{
			_roundLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._waveLabel, out var value6))
		{
			_waveLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._timeLabel, out var value7))
		{
			_timeLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pointLabel, out var value8))
		{
			_pointLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pointProgress, out var value9))
		{
			_pointProgress = value9.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._zombiePool, out var value10))
		{
			_zombiePool = value10.As<FlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._roundAdds, out var value11))
		{
			_roundAdds = value11.As<FlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._zombiePreviewRoot, out var value12))
		{
			_zombiePreviewRoot = value12.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._gameBackground, out var value13))
		{
			_gameBackground = value13.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._roundSpin, out var value14))
		{
			_roundSpin = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._waveSpin, out var value15))
		{
			_waveSpin = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._bigWaveCheck, out var value16))
		{
			_bigWaveCheck = value16.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value17))
		{
			_updatingControls = value17.As<bool>();
		}
	}
}
