using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.RunBar;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWEditorTitleBar.cs")]
public class XWEditorTitleBar : VBoxContainer
{
	[Signal]
	public delegate void MainScreenChangedEventHandler(string screenKey);

	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName AddMainScreenButton = "AddMainScreenButton";

		public static readonly StringName ConfigureMotion = "ConfigureMotion";

		public static readonly StringName AddResourceEditorOption = "AddResourceEditorOption";

		public static readonly StringName SetProjectTitle = "SetProjectTitle";

		public static readonly StringName SetupResourceWorkspacePalette = "SetupResourceWorkspacePalette";

		public static readonly StringName OpenResourceWorkspacePalette = "OpenResourceWorkspacePalette";

		public static readonly StringName SelectResourceWorkspace = "SelectResourceWorkspace";

		public static readonly StringName ResetResourceEditorOption = "ResetResourceEditorOption";

		public static readonly StringName RestoreResourceHubState = "RestoreResourceHubState";

		public static readonly StringName PrepareMainScreenButton = "PrepareMainScreenButton";

		public static readonly StringName BindMainScreenButton = "BindMainScreenButton";

		public static readonly StringName OnScreenSelected = "OnScreenSelected";

		public static readonly StringName SetupMotionControls = "SetupMotionControls";

		public static readonly StringName OnMotionModeToggled = "OnMotionModeToggled";

		public static readonly StringName UpdateMotionTooltip = "UpdateMotionTooltip";

		public static readonly StringName PlayInitialHudReveal = "PlayInitialHudReveal";

		public static readonly StringName UpdateResponsiveLayout = "UpdateResponsiveLayout";

		public static readonly StringName UpdateMainScreenButtonPresentation = "UpdateMainScreenButtonPresentation";

		public static readonly StringName UpdateResourceOptionPresentation = "UpdateResourceOptionPresentation";

		public static readonly StringName OnRunBarChildEnteredTree = "OnRunBarChildEnteredTree";

		public static readonly StringName BindRunBarIfAvailable = "BindRunBarIfAvailable";

		public static readonly StringName UnbindRunBar = "UnbindRunBar";

		public static readonly StringName UpdateRunStatus = "UpdateRunStatus";

		public static readonly StringName FindAncestorPanel = "FindAncestorPanel";

		public static readonly StringName ShortenStatusText = "ShortenStatusText";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName MenuBar = "MenuBar";

		public static readonly StringName RunBarContainer = "RunBarContainer";

		public static readonly StringName IsCompactMode = "IsCompactMode";

		public static readonly StringName MotionModeButton = "MotionModeButton";

		public static readonly StringName IsReducedMotion = "IsReducedMotion";

		public static readonly StringName IsLowPerformanceMode = "IsLowPerformanceMode";

		public static readonly StringName _menuBar = "_menuBar";

		public static readonly StringName _brandLabel = "_brandLabel";

		public static readonly StringName _brandPanel = "_brandPanel";

		public static readonly StringName _projectStatusPanel = "_projectStatusPanel";

		public static readonly StringName _projectStatusIcon = "_projectStatusIcon";

		public static readonly StringName _projectTitle = "_projectTitle";

		public static readonly StringName _workspaceLabel = "_workspaceLabel";

		public static readonly StringName _mainScreenButtons = "_mainScreenButtons";

		public static readonly StringName _blueprintButton = "_blueprintButton";

		public static readonly StringName _twoDButton = "_twoDButton";

		public static readonly StringName _scriptButton = "_scriptButton";

		public static readonly StringName _resourceHubButton = "_resourceHubButton";

		public static readonly StringName _motionModeButton = "_motionModeButton";

		public static readonly StringName _resourcePalette = "_resourcePalette";

		public static readonly StringName _runBarContainer = "_runBarContainer";

		public static readonly StringName _runStatusPanel = "_runStatusPanel";

		public static readonly StringName _runStatusIcon = "_runStatusIcon";

		public static readonly StringName _runStatusLabel = "_runStatusLabel";

		public static readonly StringName _boundRunBar = "_boundRunBar";

		public static readonly StringName _topHudMotion = "_topHudMotion";

		public static readonly StringName _workspaceHudMotion = "_workspaceHudMotion";

		public static readonly StringName _activeResourceKey = "_activeResourceKey";

		public static readonly StringName _compactMode = "_compactMode";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
		public static readonly StringName MainScreenChanged = "MainScreenChanged";
	}

	private const float CompactWidth = 1020f;

	private const string ReducedMotionSetting = "interface/reduced_motion";

	private const string LowPerformanceSetting = "interface/low_performance_mode";

	private const string ResourceHubIconPath = "res://addons/ModEditor/Icons/ResourceMap.svg";

	private const string ResourcePaletteScenePath = "res://addons/ModEditor/GUI/XWResourceWorkspacePalette.tscn";

	private static PackedScene _resourcePaletteScene;

	private MenuBar _menuBar;

	private Label _brandLabel;

	private PanelContainer _brandPanel;

	private PanelContainer _projectStatusPanel;

	private TextureRect _projectStatusIcon;

	private Label _projectTitle;

	private Label _workspaceLabel;

	private HBoxContainer _mainScreenButtons;

	private Button _blueprintButton;

	private Button _twoDButton;

	private Button _scriptButton;

	private Button _resourceHubButton;

	private Button _motionModeButton;

	private XWResourceWorkspacePalette _resourcePalette;

	private HBoxContainer _runBarContainer;

	private PanelContainer _runStatusPanel;

	private TextureRect _runStatusIcon;

	private Label _runStatusLabel;

	private XWEditorRunBar _boundRunBar;

	private readonly Dictionary<Button, string> _mainScreenLabels = new Dictionary<Button, string>();

	private readonly Dictionary<Button, XWUiMotion> _navigationMotions = new Dictionary<Button, XWUiMotion>();

	private XWUiMotion _topHudMotion;

	private XWUiMotion _workspaceHudMotion;

	private string _activeResourceKey = "";

	private bool _compactMode;

	private MainScreenChangedEventHandler backing_MainScreenChanged;

	public MenuBar MenuBar => _menuBar;

	public HBoxContainer RunBarContainer => _runBarContainer;

	public bool IsCompactMode => _compactMode;

	public Button MotionModeButton => _motionModeButton;

	public bool IsReducedMotion => XWUiMotion.ReducedMotion;

	public bool IsLowPerformanceMode => XWUiMotion.LowPerformanceMode;

	public event MainScreenChangedEventHandler MainScreenChanged
	{
		add
		{
			backing_MainScreenChanged = (MainScreenChangedEventHandler)Delegate.Combine(backing_MainScreenChanged, value);
		}
		remove
		{
			backing_MainScreenChanged = (MainScreenChangedEventHandler)Delegate.Remove(backing_MainScreenChanged, value);
		}
	}

	public override void _Ready()
	{
		_menuBar = GetNode<MenuBar>("%MenuBar");
		_brandLabel = GetNodeOrNull<Label>("%BrandLabel");
		_brandPanel = GetNodeOrNull<PanelContainer>("%BrandPanel");
		_projectStatusPanel = GetNodeOrNull<PanelContainer>("%ProjectStatusPanel");
		_projectStatusIcon = GetNodeOrNull<TextureRect>("%ProjectStatusIcon");
		_projectTitle = GetNodeOrNull<Label>("%ProjectTitle");
		_workspaceLabel = GetNodeOrNull<Label>("%WorkspaceLabel");
		_mainScreenButtons = GetNode<HBoxContainer>("%MainScreenButtons");
		_blueprintButton = GetNode<Button>("%BlueprintButton");
		_twoDButton = GetNodeOrNull<Button>("%2DButton");
		_scriptButton = GetNode<Button>("%ScriptButton");
		_resourceHubButton = GetNodeOrNull<Button>("%ResourceHubButton");
		_motionModeButton = GetNodeOrNull<Button>("%MotionModeButton");
		_runBarContainer = GetNode<HBoxContainer>("%RunBarContainer");
		_runStatusPanel = GetNodeOrNull<PanelContainer>("%RunStatusPanel");
		_runStatusIcon = GetNodeOrNull<TextureRect>("%RunStatusIcon");
		_runStatusLabel = GetNodeOrNull<Label>("%RunStatusLabel");
		SetupResourceWorkspacePalette();
		PrepareMainScreenButton(_blueprintButton, "blueprint", _blueprintButton.Text);
		PrepareMainScreenButton(_twoDButton, "2d", _twoDButton?.Text ?? "2D");
		PrepareMainScreenButton(_scriptButton, "script", _scriptButton.Text);
		SetupMotionControls();
		_runBarContainer.ChildEnteredTree += OnRunBarChildEnteredTree;
		Resized += UpdateResponsiveLayout;
		SetProjectTitle("");
		UpdateRunStatus(running: false, paused: false, 0);
		UpdateResponsiveLayout();
		CallDeferred("BindRunBarIfAvailable");
		CallDeferred("PlayInitialHudReveal");
	}

	public override void _ExitTree()
	{
		Resized -= UpdateResponsiveLayout;
		if (GodotObject.IsInstanceValid(_runBarContainer))
		{
			_runBarContainer.ChildEnteredTree -= OnRunBarChildEnteredTree;
		}
		UnbindRunBar();
		base._ExitTree();
	}

	public Button AddMainScreenButton(string key, string text, Texture2D icon = null)
	{
		if (_mainScreenButtons == null || string.IsNullOrWhiteSpace(key))
		{
			return null;
		}
		foreach (Node child in _mainScreenButtons.GetChildren())
		{
			if (child is Button button && button.HasMeta("_main_screen_key") && button.GetMeta("_main_screen_key").AsString() == key)
			{
				return button;
			}
		}
		Button button2 = new Button
		{
			Icon = icon,
			ToggleMode = true,
			ThemeTypeVariation = "GameNavCard"
		};
		_mainScreenButtons.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
		PrepareMainScreenButton(button2, key, text);
		UpdateMainScreenButtonPresentation(button2);
		return button2;
	}

	public void ConfigureMotion(bool reducedMotion, bool lowPerformanceMode)
	{
		XWUiMotion.ConfigurePolicy(reducedMotion, lowPerformanceMode);
		if (GodotObject.IsInstanceValid(_motionModeButton))
		{
			_motionModeButton.SetPressedNoSignal(!reducedMotion && !lowPerformanceMode);
			_motionModeButton.Disabled = lowPerformanceMode;
			UpdateMotionTooltip();
		}
	}

	public void AddResourceEditorOption(string key, string text, Texture2D icon = null)
	{
		if (GodotObject.IsInstanceValid(_resourcePalette) && !string.IsNullOrWhiteSpace(key))
		{
			string label = (string.IsNullOrWhiteSpace(text) ? "资源" : text.Trim());
			_resourcePalette.AddWorkspace(key, label, icon);
			if (GodotObject.IsInstanceValid(_resourceHubButton))
			{
				_resourceHubButton.Disabled = false;
			}
		}
	}

	public void SetProjectTitle(string title)
	{
		string text = title?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(text))
		{
			text = FindAncestorPanel()?.GetCurrentProject()?.Name?.Trim() ?? "";
		}
		bool flag = !string.IsNullOrWhiteSpace(text);
		string text2 = (flag ? ShortenStatusText(text, 20) : "项目待加载");
		if (GodotObject.IsInstanceValid(_projectTitle))
		{
			_projectTitle.Text = text2;
		}
		if (GodotObject.IsInstanceValid(_projectStatusPanel))
		{
			_projectStatusPanel.ThemeTypeVariation = (flag ? "HudStatusReady" : "HudStatusIdle");
			_projectStatusPanel.TooltipText = (flag ? ("当前 Mod 项目：" + text) : "尚未打开 Mod 项目");
			_projectStatusPanel.Visible = !_compactMode;
		}
		if (GodotObject.IsInstanceValid(_projectStatusIcon))
		{
			_projectStatusIcon.Modulate = (flag ? ModEditorTheme.LeafColor.Lightened(0.25f) : ModEditorTheme.DimTextColor);
		}
	}

	private void SetupResourceWorkspacePalette()
	{
		if (GodotObject.IsInstanceValid(_resourceHubButton))
		{
			Texture2D icon = (ResourceLoader.Exists("res://addons/ModEditor/Icons/ResourceMap.svg") ? XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceMap.svg", null, ResourceLoader.CacheMode.Reuse)) : null);
			_resourceHubButton.Icon = icon;
			_resourceHubButton.Disabled = true;
			_resourceHubButton.Pressed += OpenResourceWorkspacePalette;
			_navigationMotions[_resourceHubButton] = XWUiMotion.BindButton(_resourceHubButton);
			if (_resourcePaletteScene == null)
			{
				_resourcePaletteScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWResourceWorkspacePalette.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_resourcePalette = _resourcePaletteScene?.Instantiate<XWResourceWorkspacePalette>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(_resourcePalette))
			{
				AddChild(_resourcePalette, forceReadableName: false, InternalMode.Disabled);
				_resourcePalette.WorkspaceSelected += SelectResourceWorkspace;
				_resourcePalette.PaletteDismissed += RestoreResourceHubState;
			}
		}
	}

	private void OpenResourceWorkspacePalette()
	{
		if (GodotObject.IsInstanceValid(_resourcePalette))
		{
			_resourcePalette.OpenPalette();
		}
	}

	private void SelectResourceWorkspace(string key)
	{
		if (!string.IsNullOrWhiteSpace(key))
		{
			_activeResourceKey = key;
			if (GodotObject.IsInstanceValid(_resourceHubButton))
			{
				_resourceHubButton.SetPressedNoSignal(pressed: true);
				_resourceHubButton.TooltipText = "当前资源工作台：" + key + "\n点击选择其他可视资源面板";
				XWUiMotion.FindBinding(_resourceHubButton)?.SyncButtonState();
			}
			OnScreenSelected(key, _resourceHubButton);
		}
	}

	private void ResetResourceEditorOption()
	{
		_activeResourceKey = "";
		RestoreResourceHubState();
	}

	private void RestoreResourceHubState()
	{
		if (GodotObject.IsInstanceValid(_resourceHubButton))
		{
			_resourceHubButton.SetPressedNoSignal(!string.IsNullOrWhiteSpace(_activeResourceKey));
			XWUiMotion.FindBinding(_resourceHubButton)?.SyncButtonState();
			if (string.IsNullOrWhiteSpace(_activeResourceKey))
			{
				_resourceHubButton.TooltipText = "用搜索和图块选择可视资源工作台";
			}
		}
	}

	private void PrepareMainScreenButton(Button button, string key, string label)
	{
		if (button != null)
		{
			label = (string.IsNullOrWhiteSpace(label) ? key : label.Trim());
			button.SetMeta("_main_screen_key", key);
			button.SetMeta("_hud_label", label);
			button.TooltipText = "切换到" + label + "工作区";
			_mainScreenLabels[button] = label;
			_navigationMotions[button] = XWUiMotion.BindButton(button);
			BindMainScreenButton(button, key);
			UpdateMainScreenButtonPresentation(button);
		}
	}

	private void BindMainScreenButton(Button button, string key)
	{
		if (button == null)
		{
			return;
		}
		button.Toggled += (bool pressed) =>
		{
			if (pressed)
			{
				ResetResourceEditorOption();
				OnScreenSelected(key, button);
			}
		};
	}

	private void OnScreenSelected(string key, Button active)
	{
		foreach (Node child in _mainScreenButtons.GetChildren())
		{
			if (child is Button button && button != active && button.ToggleMode)
			{
				button.SetPressedNoSignal(pressed: false);
				XWUiMotion.FindBinding(button)?.SyncButtonState();
			}
		}
		XWUiMotion.FindBinding(active)?.SyncButtonState();
		_workspaceHudMotion?.PlayPanelReveal(4f);
		EmitSignal(SignalName.MainScreenChanged, key);
	}

	private void SetupMotionControls()
	{
		_topHudMotion = XWUiMotion.BindPanel(GetNodeOrNull<PanelContainer>("%TopHudSurface"));
		_workspaceHudMotion = XWUiMotion.BindPanel(GetNodeOrNull<PanelContainer>("%WorkspaceHudSurface"));
		if (GodotObject.IsInstanceValid(_motionModeButton))
		{
			_motionModeButton.SetPressedNoSignal(XWUiMotion.MotionAllowed);
			_motionModeButton.Toggled += OnMotionModeToggled;
			_navigationMotions[_motionModeButton] = XWUiMotion.BindButton(_motionModeButton);
			UpdateMotionTooltip();
		}
	}

	private void OnMotionModeToggled(bool enabled)
	{
		if (XWUiMotion.LowPerformanceMode)
		{
			_motionModeButton?.SetPressedNoSignal(pressed: false);
			UpdateMotionTooltip();
			return;
		}
		bool flag = !enabled;
		XWUiMotion.SetReducedMotion(flag);
		XWEditorSettings xWEditorSettings = XWEditorInterface.Instance?.GetEditorSettings();
		if (xWEditorSettings != null)
		{
			xWEditorSettings.SetSetting("interface/reduced_motion", flag);
			xWEditorSettings.Save();
		}
		UpdateMotionTooltip();
	}

	private void UpdateMotionTooltip()
	{
		if (GodotObject.IsInstanceValid(_motionModeButton))
		{
			Button motionModeButton = _motionModeButton;
			string tooltipText;
			if (XWUiMotion.LowPerformanceMode)
			{
				tooltipText = "低性能模式已启用：界面动效自动关闭";
			}
			else
			{
				tooltipText = (XWUiMotion.ReducedMotion ? "低动效已启用；点击恢复卡片与面板过渡" : "界面动效已启用；点击切换为低动效");
			}
			motionModeButton.TooltipText = tooltipText;
		}
	}

	private void PlayInitialHudReveal()
	{
		_topHudMotion?.PlayPanelReveal(6f);
		_workspaceHudMotion?.PlayPanelReveal(8f);
	}

	private void UpdateResponsiveLayout()
	{
		bool flag = Size.X > 1f && Size.X < 1020f;
		if (_compactMode == flag && Size.X > 1f)
		{
			return;
		}
		_compactMode = flag;
		if (GodotObject.IsInstanceValid(_brandLabel))
		{
			_brandLabel.Visible = !flag;
		}
		if (GodotObject.IsInstanceValid(_brandPanel))
		{
			_brandPanel.CustomMinimumSize = new Vector2(flag ? 44f : 138f, 38f);
		}
		if (GodotObject.IsInstanceValid(_projectStatusPanel))
		{
			_projectStatusPanel.Visible = !flag;
		}
		if (GodotObject.IsInstanceValid(_workspaceLabel))
		{
			_workspaceLabel.Visible = !flag;
		}
		if (GodotObject.IsInstanceValid(_runStatusPanel))
		{
			_runStatusPanel.Visible = !flag;
		}
		foreach (Button key in _mainScreenLabels.Keys)
		{
			UpdateMainScreenButtonPresentation(key);
		}
		UpdateResourceOptionPresentation();
	}

	private void UpdateMainScreenButtonPresentation(Button button)
	{
		if (GodotObject.IsInstanceValid(button) && _mainScreenLabels.TryGetValue(button, out var value))
		{
			button.Text = (_compactMode ? "" : value);
			button.CustomMinimumSize = new Vector2(_compactMode ? 42f : 92f, 38f);
		}
	}

	private void UpdateResourceOptionPresentation()
	{
		if (GodotObject.IsInstanceValid(_resourceHubButton))
		{
			_resourceHubButton.Text = (_compactMode ? "" : "资源");
			_resourceHubButton.CustomMinimumSize = new Vector2(_compactMode ? 48f : 132f, 38f);
		}
	}

	private void OnRunBarChildEnteredTree(Node child)
	{
		if (child is XWEditorRunBar)
		{
			CallDeferred("BindRunBarIfAvailable");
		}
	}

	private void BindRunBarIfAvailable()
	{
		XWEditorRunBar xWEditorRunBar = null;
		foreach (Node child in _runBarContainer.GetChildren())
		{
			if (child is XWEditorRunBar xWEditorRunBar2)
			{
				xWEditorRunBar = xWEditorRunBar2;
				break;
			}
		}
		if (GodotObject.IsInstanceValid(xWEditorRunBar) && xWEditorRunBar != _boundRunBar)
		{
			UnbindRunBar();
			_boundRunBar = xWEditorRunBar;
			_boundRunBar.RunStatusChanged += UpdateRunStatus;
			UpdateRunStatus(_boundRunBar.IsRunning(), _boundRunBar.IsPaused(), (int)_boundRunBar.GetRunMode());
		}
	}

	private void UnbindRunBar()
	{
		if (GodotObject.IsInstanceValid(_boundRunBar))
		{
			_boundRunBar.RunStatusChanged -= UpdateRunStatus;
		}
		_boundRunBar = null;
	}

	private void UpdateRunStatus(bool running, bool paused, int mode)
	{
		string text;
		if (paused)
		{
			text = "已暂停";
		}
		else
		{
			text = (running ? "运行中" : "待机");
		}
		string text2;
		if (paused)
		{
			text2 = "HudStatusPaused";
		}
		else
		{
			text2 = (running ? "HudStatusRunning" : "HudStatusIdle");
		}
		if (GodotObject.IsInstanceValid(_runStatusLabel))
		{
			_runStatusLabel.Text = text;
		}
		if (GodotObject.IsInstanceValid(_runStatusPanel))
		{
			_runStatusPanel.ThemeTypeVariation = text2;
			_runStatusPanel.TooltipText = (running ? $"游戏预览状态：{text}（模式 {(XWEditorRunBar.RunMode)mode}）" : "游戏预览尚未运行");
		}
		if (GodotObject.IsInstanceValid(_runStatusIcon))
		{
			TextureRect runStatusIcon = _runStatusIcon;
			Color modulate;
			if (paused)
			{
				modulate = new Color(1f, 0.63f, 0.25f);
			}
			else
			{
				modulate = (running ? ModEditorTheme.SunGoldColor : ModEditorTheme.DimTextColor);
			}
			runStatusIcon.Modulate = modulate;
		}
	}

	private ModEditorPanel FindAncestorPanel()
	{
		Node parent = GetParent();
		while (GodotObject.IsInstanceValid(parent))
		{
			if (parent is ModEditorPanel result)
			{
				return result;
			}
			parent = parent.GetParent();
		}
		return null;
	}

	private static string ShortenStatusText(string text, int maxLength)
	{
		if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
		{
			return text;
		}
		return text.Substring(0, Math.Max(1, maxLength - 1)) + "…";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(27)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddMainScreenButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureMotion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "reducedMotion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "lowPerformanceMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddResourceEditorOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetProjectTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupResourceWorkspacePalette, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenResourceWorkspacePalette, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectResourceWorkspace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetResourceEditorOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreResourceHubState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareMainScreenButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindMainScreenButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnScreenSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "active", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetupMotionControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMotionModeToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMotionTooltip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayInitialHudReveal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateResponsiveLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMainScreenButtonPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateResourceOptionPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunBarChildEnteredTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindRunBarIfAvailable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnbindRunBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateRunStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShortenStatusText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maxLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.AddMainScreenButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(AddMainScreenButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Texture2D>(in args[2])));
			return true;
		}
		if (method == MethodName.ConfigureMotion && args.Count == 2)
		{
			ConfigureMotion(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddResourceEditorOption && args.Count == 3)
		{
			AddResourceEditorOption(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Texture2D>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProjectTitle && args.Count == 1)
		{
			SetProjectTitle(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupResourceWorkspacePalette && args.Count == 0)
		{
			SetupResourceWorkspacePalette();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenResourceWorkspacePalette && args.Count == 0)
		{
			OpenResourceWorkspacePalette();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectResourceWorkspace && args.Count == 1)
		{
			SelectResourceWorkspace(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetResourceEditorOption && args.Count == 0)
		{
			ResetResourceEditorOption();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreResourceHubState && args.Count == 0)
		{
			RestoreResourceHubState();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareMainScreenButton && args.Count == 3)
		{
			PrepareMainScreenButton(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindMainScreenButton && args.Count == 2)
		{
			BindMainScreenButton(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnScreenSelected && args.Count == 2)
		{
			OnScreenSelected(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Button>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupMotionControls && args.Count == 0)
		{
			SetupMotionControls();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMotionModeToggled && args.Count == 1)
		{
			OnMotionModeToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMotionTooltip && args.Count == 0)
		{
			UpdateMotionTooltip();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayInitialHudReveal && args.Count == 0)
		{
			PlayInitialHudReveal();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateResponsiveLayout && args.Count == 0)
		{
			UpdateResponsiveLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMainScreenButtonPresentation && args.Count == 1)
		{
			UpdateMainScreenButtonPresentation(VariantUtils.ConvertTo<Button>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateResourceOptionPresentation && args.Count == 0)
		{
			UpdateResourceOptionPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunBarChildEnteredTree && args.Count == 1)
		{
			OnRunBarChildEnteredTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindRunBarIfAvailable && args.Count == 0)
		{
			BindRunBarIfAvailable();
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindRunBar && args.Count == 0)
		{
			UnbindRunBar();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRunStatus && args.Count == 3)
		{
			UpdateRunStatus(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindAncestorPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ModEditorPanel>(FindAncestorPanel());
			return true;
		}
		if (method == MethodName.ShortenStatusText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ShortenStatusText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ShortenStatusText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ShortenStatusText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
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
		if (method == MethodName.AddMainScreenButton)
		{
			return true;
		}
		if (method == MethodName.ConfigureMotion)
		{
			return true;
		}
		if (method == MethodName.AddResourceEditorOption)
		{
			return true;
		}
		if (method == MethodName.SetProjectTitle)
		{
			return true;
		}
		if (method == MethodName.SetupResourceWorkspacePalette)
		{
			return true;
		}
		if (method == MethodName.OpenResourceWorkspacePalette)
		{
			return true;
		}
		if (method == MethodName.SelectResourceWorkspace)
		{
			return true;
		}
		if (method == MethodName.ResetResourceEditorOption)
		{
			return true;
		}
		if (method == MethodName.RestoreResourceHubState)
		{
			return true;
		}
		if (method == MethodName.PrepareMainScreenButton)
		{
			return true;
		}
		if (method == MethodName.BindMainScreenButton)
		{
			return true;
		}
		if (method == MethodName.OnScreenSelected)
		{
			return true;
		}
		if (method == MethodName.SetupMotionControls)
		{
			return true;
		}
		if (method == MethodName.OnMotionModeToggled)
		{
			return true;
		}
		if (method == MethodName.UpdateMotionTooltip)
		{
			return true;
		}
		if (method == MethodName.PlayInitialHudReveal)
		{
			return true;
		}
		if (method == MethodName.UpdateResponsiveLayout)
		{
			return true;
		}
		if (method == MethodName.UpdateMainScreenButtonPresentation)
		{
			return true;
		}
		if (method == MethodName.UpdateResourceOptionPresentation)
		{
			return true;
		}
		if (method == MethodName.OnRunBarChildEnteredTree)
		{
			return true;
		}
		if (method == MethodName.BindRunBarIfAvailable)
		{
			return true;
		}
		if (method == MethodName.UnbindRunBar)
		{
			return true;
		}
		if (method == MethodName.UpdateRunStatus)
		{
			return true;
		}
		if (method == MethodName.FindAncestorPanel)
		{
			return true;
		}
		if (method == MethodName.ShortenStatusText)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._menuBar)
		{
			_menuBar = VariantUtils.ConvertTo<MenuBar>(in value);
			return true;
		}
		if (name == PropertyName._brandLabel)
		{
			_brandLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._brandPanel)
		{
			_brandPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._projectStatusPanel)
		{
			_projectStatusPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._projectStatusIcon)
		{
			_projectStatusIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._projectTitle)
		{
			_projectTitle = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._workspaceLabel)
		{
			_workspaceLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mainScreenButtons)
		{
			_mainScreenButtons = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._blueprintButton)
		{
			_blueprintButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._twoDButton)
		{
			_twoDButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._scriptButton)
		{
			_scriptButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resourceHubButton)
		{
			_resourceHubButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._motionModeButton)
		{
			_motionModeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resourcePalette)
		{
			_resourcePalette = VariantUtils.ConvertTo<XWResourceWorkspacePalette>(in value);
			return true;
		}
		if (name == PropertyName._runBarContainer)
		{
			_runBarContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._runStatusPanel)
		{
			_runStatusPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._runStatusIcon)
		{
			_runStatusIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._runStatusLabel)
		{
			_runStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._boundRunBar)
		{
			_boundRunBar = VariantUtils.ConvertTo<XWEditorRunBar>(in value);
			return true;
		}
		if (name == PropertyName._topHudMotion)
		{
			_topHudMotion = VariantUtils.ConvertTo<XWUiMotion>(in value);
			return true;
		}
		if (name == PropertyName._workspaceHudMotion)
		{
			_workspaceHudMotion = VariantUtils.ConvertTo<XWUiMotion>(in value);
			return true;
		}
		if (name == PropertyName._activeResourceKey)
		{
			_activeResourceKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._compactMode)
		{
			_compactMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.MenuBar)
		{
			value = VariantUtils.CreateFrom<MenuBar>(MenuBar);
			return true;
		}
		if (name == PropertyName.RunBarContainer)
		{
			value = VariantUtils.CreateFrom<HBoxContainer>(RunBarContainer);
			return true;
		}
		bool from;
		if (name == PropertyName.IsCompactMode)
		{
			from = IsCompactMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MotionModeButton)
		{
			value = VariantUtils.CreateFrom<Button>(MotionModeButton);
			return true;
		}
		if (name == PropertyName.IsReducedMotion)
		{
			from = IsReducedMotion;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsLowPerformanceMode)
		{
			from = IsLowPerformanceMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._menuBar)
		{
			value = VariantUtils.CreateFrom(in _menuBar);
			return true;
		}
		if (name == PropertyName._brandLabel)
		{
			value = VariantUtils.CreateFrom(in _brandLabel);
			return true;
		}
		if (name == PropertyName._brandPanel)
		{
			value = VariantUtils.CreateFrom(in _brandPanel);
			return true;
		}
		if (name == PropertyName._projectStatusPanel)
		{
			value = VariantUtils.CreateFrom(in _projectStatusPanel);
			return true;
		}
		if (name == PropertyName._projectStatusIcon)
		{
			value = VariantUtils.CreateFrom(in _projectStatusIcon);
			return true;
		}
		if (name == PropertyName._projectTitle)
		{
			value = VariantUtils.CreateFrom(in _projectTitle);
			return true;
		}
		if (name == PropertyName._workspaceLabel)
		{
			value = VariantUtils.CreateFrom(in _workspaceLabel);
			return true;
		}
		if (name == PropertyName._mainScreenButtons)
		{
			value = VariantUtils.CreateFrom(in _mainScreenButtons);
			return true;
		}
		if (name == PropertyName._blueprintButton)
		{
			value = VariantUtils.CreateFrom(in _blueprintButton);
			return true;
		}
		if (name == PropertyName._twoDButton)
		{
			value = VariantUtils.CreateFrom(in _twoDButton);
			return true;
		}
		if (name == PropertyName._scriptButton)
		{
			value = VariantUtils.CreateFrom(in _scriptButton);
			return true;
		}
		if (name == PropertyName._resourceHubButton)
		{
			value = VariantUtils.CreateFrom(in _resourceHubButton);
			return true;
		}
		if (name == PropertyName._motionModeButton)
		{
			value = VariantUtils.CreateFrom(in _motionModeButton);
			return true;
		}
		if (name == PropertyName._resourcePalette)
		{
			value = VariantUtils.CreateFrom(in _resourcePalette);
			return true;
		}
		if (name == PropertyName._runBarContainer)
		{
			value = VariantUtils.CreateFrom(in _runBarContainer);
			return true;
		}
		if (name == PropertyName._runStatusPanel)
		{
			value = VariantUtils.CreateFrom(in _runStatusPanel);
			return true;
		}
		if (name == PropertyName._runStatusIcon)
		{
			value = VariantUtils.CreateFrom(in _runStatusIcon);
			return true;
		}
		if (name == PropertyName._runStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _runStatusLabel);
			return true;
		}
		if (name == PropertyName._boundRunBar)
		{
			value = VariantUtils.CreateFrom(in _boundRunBar);
			return true;
		}
		if (name == PropertyName._topHudMotion)
		{
			value = VariantUtils.CreateFrom(in _topHudMotion);
			return true;
		}
		if (name == PropertyName._workspaceHudMotion)
		{
			value = VariantUtils.CreateFrom(in _workspaceHudMotion);
			return true;
		}
		if (name == PropertyName._activeResourceKey)
		{
			value = VariantUtils.CreateFrom(in _activeResourceKey);
			return true;
		}
		if (name == PropertyName._compactMode)
		{
			value = VariantUtils.CreateFrom(in _compactMode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._menuBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brandLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brandPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectStatusPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectStatusIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectTitle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workspaceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mainScreenButtons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._blueprintButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._twoDButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceHubButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._motionModeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePalette, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runBarContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runStatusPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runStatusIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._boundRunBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._topHudMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workspaceHudMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._activeResourceKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._compactMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MenuBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.RunBarContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCompactMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MotionModeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsReducedMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsLowPerformanceMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._menuBar, Variant.From(in _menuBar));
		info.AddProperty(PropertyName._brandLabel, Variant.From(in _brandLabel));
		info.AddProperty(PropertyName._brandPanel, Variant.From(in _brandPanel));
		info.AddProperty(PropertyName._projectStatusPanel, Variant.From(in _projectStatusPanel));
		info.AddProperty(PropertyName._projectStatusIcon, Variant.From(in _projectStatusIcon));
		info.AddProperty(PropertyName._projectTitle, Variant.From(in _projectTitle));
		info.AddProperty(PropertyName._workspaceLabel, Variant.From(in _workspaceLabel));
		info.AddProperty(PropertyName._mainScreenButtons, Variant.From(in _mainScreenButtons));
		info.AddProperty(PropertyName._blueprintButton, Variant.From(in _blueprintButton));
		info.AddProperty(PropertyName._twoDButton, Variant.From(in _twoDButton));
		info.AddProperty(PropertyName._scriptButton, Variant.From(in _scriptButton));
		info.AddProperty(PropertyName._resourceHubButton, Variant.From(in _resourceHubButton));
		info.AddProperty(PropertyName._motionModeButton, Variant.From(in _motionModeButton));
		info.AddProperty(PropertyName._resourcePalette, Variant.From(in _resourcePalette));
		info.AddProperty(PropertyName._runBarContainer, Variant.From(in _runBarContainer));
		info.AddProperty(PropertyName._runStatusPanel, Variant.From(in _runStatusPanel));
		info.AddProperty(PropertyName._runStatusIcon, Variant.From(in _runStatusIcon));
		info.AddProperty(PropertyName._runStatusLabel, Variant.From(in _runStatusLabel));
		info.AddProperty(PropertyName._boundRunBar, Variant.From(in _boundRunBar));
		info.AddProperty(PropertyName._topHudMotion, Variant.From(in _topHudMotion));
		info.AddProperty(PropertyName._workspaceHudMotion, Variant.From(in _workspaceHudMotion));
		info.AddProperty(PropertyName._activeResourceKey, Variant.From(in _activeResourceKey));
		info.AddProperty(PropertyName._compactMode, Variant.From(in _compactMode));
		info.AddSignalEventDelegate(SignalName.MainScreenChanged, backing_MainScreenChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._menuBar, out var value))
		{
			_menuBar = value.As<MenuBar>();
		}
		if (info.TryGetProperty(PropertyName._brandLabel, out var value2))
		{
			_brandLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._brandPanel, out var value3))
		{
			_brandPanel = value3.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._projectStatusPanel, out var value4))
		{
			_projectStatusPanel = value4.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._projectStatusIcon, out var value5))
		{
			_projectStatusIcon = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._projectTitle, out var value6))
		{
			_projectTitle = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._workspaceLabel, out var value7))
		{
			_workspaceLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mainScreenButtons, out var value8))
		{
			_mainScreenButtons = value8.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._blueprintButton, out var value9))
		{
			_blueprintButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._twoDButton, out var value10))
		{
			_twoDButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._scriptButton, out var value11))
		{
			_scriptButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resourceHubButton, out var value12))
		{
			_resourceHubButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._motionModeButton, out var value13))
		{
			_motionModeButton = value13.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resourcePalette, out var value14))
		{
			_resourcePalette = value14.As<XWResourceWorkspacePalette>();
		}
		if (info.TryGetProperty(PropertyName._runBarContainer, out var value15))
		{
			_runBarContainer = value15.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._runStatusPanel, out var value16))
		{
			_runStatusPanel = value16.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._runStatusIcon, out var value17))
		{
			_runStatusIcon = value17.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._runStatusLabel, out var value18))
		{
			_runStatusLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._boundRunBar, out var value19))
		{
			_boundRunBar = value19.As<XWEditorRunBar>();
		}
		if (info.TryGetProperty(PropertyName._topHudMotion, out var value20))
		{
			_topHudMotion = value20.As<XWUiMotion>();
		}
		if (info.TryGetProperty(PropertyName._workspaceHudMotion, out var value21))
		{
			_workspaceHudMotion = value21.As<XWUiMotion>();
		}
		if (info.TryGetProperty(PropertyName._activeResourceKey, out var value22))
		{
			_activeResourceKey = value22.As<string>();
		}
		if (info.TryGetProperty(PropertyName._compactMode, out var value23))
		{
			_compactMode = value23.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<MainScreenChangedEventHandler>(SignalName.MainScreenChanged, out var value24))
		{
			backing_MainScreenChanged = value24;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.MainScreenChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "screenKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalMainScreenChanged(string screenKey)
	{
		EmitSignal(SignalName.MainScreenChanged, new ReadOnlySpan<Variant>((Variant)screenKey));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.MainScreenChanged && args.Count == 1)
		{
			backing_MainScreenChanged?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.MainScreenChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
