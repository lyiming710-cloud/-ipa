using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Atlas/XW2DSpriteAtlasPanel.cs")]
public class XW2DSpriteAtlasPanel : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BindEditor = "BindEditor";

		public static readonly StringName BindSprite = "BindSprite";

		public static readonly StringName RefreshFromSprite = "RefreshFromSprite";

		public static readonly StringName SetGridWithHistory = "SetGridWithHistory";

		public static readonly StringName SetFrameWithHistory = "SetFrameWithHistory";

		public static readonly StringName SetRegionWithHistory = "SetRegionWithHistory";

		public static readonly StringName BuildInterface = "BuildInterface";

		public static readonly StringName OnGridChanged = "OnGridChanged";

		public static readonly StringName OnFramePicked = "OnFramePicked";

		public static readonly StringName OnRegionToggled = "OnRegionToggled";

		public static readonly StringName BeginRegionEdit = "BeginRegionEdit";

		public static readonly StringName OnRegionValueChanged = "OnRegionValueChanged";

		public static readonly StringName CommitRegionEdit = "CommitRegionEdit";

		public static readonly StringName ReadRegionControls = "ReadRegionControls";

		public static readonly StringName SanitizeRegionRect = "SanitizeRegionRect";

		public static readonly StringName UpdateRegionFieldState = "UpdateRegionFieldState";

		public static readonly StringName RefreshCanvasAndLabels = "RefreshCanvasAndLabels";

		public static readonly StringName MakeSectionTitle = "MakeSectionTitle";

		public static readonly StringName Labeled = "Labeled";

		public static readonly StringName MakeSpin = "MakeSpin";

		public static readonly StringName MakeButton = "MakeButton";

		public static readonly StringName MakeStyle = "MakeStyle";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName CurrentSprite = "CurrentSprite";

		public static readonly StringName AtlasCanvas = "AtlasCanvas";

		public static readonly StringName StatusText = "StatusText";

		public static readonly StringName _editor = "_editor";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName _canvas = "_canvas";

		public static readonly StringName _title = "_title";

		public static readonly StringName _status = "_status";

		public static readonly StringName _frameLabel = "_frameLabel";

		public static readonly StringName _columns = "_columns";

		public static readonly StringName _rows = "_rows";

		public static readonly StringName _regionX = "_regionX";

		public static readonly StringName _regionY = "_regionY";

		public static readonly StringName _regionWidth = "_regionWidth";

		public static readonly StringName _regionHeight = "_regionHeight";

		public static readonly StringName _regionEnabled = "_regionEnabled";

		public static readonly StringName _previous = "_previous";

		public static readonly StringName _next = "_next";

		public static readonly StringName _updating = "_updating";

		public static readonly StringName _regionEditActive = "_regionEditActive";

		public static readonly StringName _regionEditStart = "_regionEditStart";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private XW2DSceneEditor _editor;

	private Sprite2D _sprite;

	private XW2DSpriteAtlasCanvas _canvas;

	private Label _title;

	private Label _status;

	private Label _frameLabel;

	private SpinBox _columns;

	private SpinBox _rows;

	private SpinBox _regionX;

	private SpinBox _regionY;

	private SpinBox _regionWidth;

	private SpinBox _regionHeight;

	private CheckButton _regionEnabled;

	private Button _previous;

	private Button _next;

	private bool _updating;

	private bool _regionEditActive;

	private Rect2 _regionEditStart;

	public Sprite2D CurrentSprite => _sprite;

	public XW2DSpriteAtlasCanvas AtlasCanvas => _canvas;

	public string StatusText => _status?.Text ?? "";

	public override void _Ready()
	{
		BuildInterface();
		SetProcess(enable: false);
		BindSprite(null);
	}

	public void BindEditor(XW2DSceneEditor editor)
	{
		_editor = editor;
	}

	public void BindSprite(Sprite2D sprite)
	{
		_sprite = (GodotObject.IsInstanceValid(sprite) ? sprite : null);
		RefreshFromSprite();
	}

	public void RefreshFromSprite()
	{
		bool flag = GodotObject.IsInstanceValid(_sprite);
		_updating = true;
		_title.Text = (flag ? $"图集切片 · {_sprite.Name}" : "图集切片");
		if (flag)
		{
			_columns.SetValueNoSignal(Math.Max(1, _sprite.Hframes));
			_rows.SetValueNoSignal(Math.Max(1, _sprite.Vframes));
			_regionEnabled.SetPressedNoSignal(_sprite.RegionEnabled);
			Rect2 regionRect = _sprite.RegionRect;
			_regionX.SetValueNoSignal(regionRect.Position.X);
			_regionY.SetValueNoSignal(regionRect.Position.Y);
			_regionWidth.SetValueNoSignal(regionRect.Size.X);
			_regionHeight.SetValueNoSignal(regionRect.Size.Y);
		}
		_updating = false;
		Control[] array = new Control[10] { _columns, _rows, _regionEnabled, _regionX, _regionY, _regionWidth, _regionHeight, _previous, _next, _canvas };
		for (int i = 0; i < array.Length; i++)
		{
			array[i].MouseFilter = (MouseFilterEnum)(flag ? 0 : 2);
		}
		_columns.Editable = flag;
		_rows.Editable = flag;
		_regionEnabled.Disabled = !flag;
		_previous.Disabled = !flag;
		_next.Disabled = !flag;
		UpdateRegionFieldState();
		RefreshCanvasAndLabels();
	}

	public bool SetGridWithHistory(int columns, int rows)
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return false;
		}
		int num = Math.Clamp(columns, 1, 256);
		int num2 = Math.Clamp(rows, 1, 256);
		int frame = Math.Clamp(_sprite.Frame, 0, num * num2 - 1);
		return _editor?.CommitSpriteAtlasStateWithHistory(_sprite, num, num2, frame, _sprite.RegionEnabled, _sprite.RegionRect, "设置精灵图集网格") ?? false;
	}

	public bool SetFrameWithHistory(int frame)
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return false;
		}
		int max = Math.Max(0, _sprite.Hframes * _sprite.Vframes - 1);
		int frame2 = Math.Clamp(frame, 0, max);
		return _editor?.CommitSpriteAtlasStateWithHistory(_sprite, _sprite.Hframes, _sprite.Vframes, frame2, _sprite.RegionEnabled, _sprite.RegionRect, "选择精灵图集帧") ?? false;
	}

	public bool SetRegionWithHistory(bool enabled, Rect2 rect)
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			return false;
		}
		Rect2 regionRect = SanitizeRegionRect(rect);
		return _editor?.CommitSpriteAtlasStateWithHistory(_sprite, _sprite.Hframes, _sprite.Vframes, _sprite.Frame, enabled, regionRect, "设置精灵图集区域") ?? false;
	}

	private void BuildInterface()
	{
		AddThemeStyleboxOverride("panel", MakeStyle(new Color("#0d1721"), new Color("#3b6078"), 10, 1));
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer();
		Label label = new Label
		{
			Text = "▦",
			CustomMinimumSize = new Vector2(38f, 38f),
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
		label.AddThemeFontSizeOverride("font_size", 23);
		label.AddThemeColorOverride("font_color", new Color("#63dcff"));
		label.AddThemeStyleboxOverride("normal", MakeStyle(new Color("#172d3d"), new Color("#3a7997"), 19, 1));
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		_title = new Label
		{
			Text = "图集切片",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_title.AddThemeFontSizeOverride("font_size", 19);
		_title.AddThemeColorOverride("font_color", new Color("#f3d889"));
		hBoxContainer.AddChild(_title, forceReadableName: false, InternalMode.Disabled);
		_status = new Label
		{
			Text = "选择 Sprite2D 后可直接点击贴图选帧",
			HorizontalAlignment = HorizontalAlignment.Right
		};
		_status.AddThemeColorOverride("font_color", new Color("#8eafc4"));
		hBoxContainer.AddChild(_status, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HSplitContainer hSplitContainer = new HSplitContainer();
		hSplitContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hSplitContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
		hSplitContainer.SplitOffsets = new int[1] { 390 };
		HSplitContainer hSplitContainer2 = hSplitContainer;
		vBoxContainer.AddChild(hSplitContainer2, forceReadableName: false, InternalMode.Disabled);
		PanelContainer panelContainer = new PanelContainer
		{
			CustomMinimumSize = new Vector2(360f, 300f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		panelContainer.AddThemeStyleboxOverride("panel", MakeStyle(new Color("#101d29"), new Color("#2c4d62"), 8, 1));
		_canvas = new XW2DSpriteAtlasCanvas
		{
			Name = "SpriteAtlasCanvas"
		};
		_canvas.FramePicked += OnFramePicked;
		panelContainer.AddChild(_canvas, forceReadableName: false, InternalMode.Disabled);
		hSplitContainer2.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		ScrollContainer scrollContainer = new ScrollContainer
		{
			CustomMinimumSize = new Vector2(330f, 0f),
			HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
		};
		hSplitContainer2.AddChild(scrollContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer2 = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer2.AddThemeConstantOverride("separation", 8);
		scrollContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(MakeSectionTitle("网格切片", "设置横纵帧数，然后直接点击左侧格子。"), forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer2 = new HBoxContainer();
		_columns = MakeSpin(1.0, 256.0, 1.0, " 列");
		_rows = MakeSpin(1.0, 256.0, 1.0, " 行");
		hBoxContainer2.AddChild(Labeled("横向", _columns), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer2.AddChild(Labeled("纵向", _rows), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer3 = new HBoxContainer();
		(string, int, int)[] array = new (string, int, int)[4]
		{
			("单图", 1, 1),
			("横 4 帧", 4, 1),
			("4 × 4", 4, 4),
			("8 × 8", 8, 8)
		};
		for (int i = 0; i < array.Length; i++)
		{
			(string, int, int) tuple = array[i];
			string item = tuple.Item1;
			int columns = tuple.Item2;
			int rows = tuple.Item3;
			Button button = MakeButton(item);
			button.Pressed += () =>
			{
				SetGridWithHistory(columns, rows);
			};
			hBoxContainer3.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
		vBoxContainer2.AddChild(hBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		_columns.ValueChanged += (double _) =>
		{
			OnGridChanged();
		};
		_rows.ValueChanged += (double _) =>
		{
			OnGridChanged();
		};
		vBoxContainer2.AddChild(MakeSectionTitle("当前帧", "黄色框就是运行时 Sprite2D 显示的画面。"), forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer4 = new HBoxContainer();
		_previous = MakeButton("◀ 上一帧");
		_next = MakeButton("下一帧 ▶");
		_frameLabel = new Label
		{
			Text = "帧 0 · 第 1 列 / 第 1 行",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
		hBoxContainer4.AddChild(_previous, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(_frameLabel, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer4.AddChild(_next, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(hBoxContainer4, forceReadableName: false, InternalMode.Disabled);
		_previous.Pressed += () =>
		{
			SetFrameWithHistory((_sprite?.Frame ?? 0) - 1);
		};
		_next.Pressed += () =>
		{
			SetFrameWithHistory((_sprite?.Frame ?? 0) + 1);
		};
		vBoxContainer2.AddChild(MakeSectionTitle("局部图集区域", "只在贴图的一块矩形内继续分帧，适合合并图集。"), forceReadableName: false, InternalMode.Disabled);
		_regionEnabled = new CheckButton
		{
			Text = "启用局部区域"
		};
		_regionEnabled.Toggled += OnRegionToggled;
		vBoxContainer2.AddChild(_regionEnabled, forceReadableName: false, InternalMode.Disabled);
		GridContainer gridContainer = new GridContainer
		{
			Columns = 2
		};
		_regionX = MakeSpin(-100000.0, 100000.0, 1.0, " X");
		_regionY = MakeSpin(-100000.0, 100000.0, 1.0, " Y");
		_regionWidth = MakeSpin(0.0, 100000.0, 1.0, " 宽");
		_regionHeight = MakeSpin(0.0, 100000.0, 1.0, " 高");
		gridContainer.AddChild(Labeled("起点", _regionX), forceReadableName: false, InternalMode.Disabled);
		gridContainer.AddChild(Labeled("起点", _regionY), forceReadableName: false, InternalMode.Disabled);
		gridContainer.AddChild(Labeled("尺寸", _regionWidth), forceReadableName: false, InternalMode.Disabled);
		gridContainer.AddChild(Labeled("尺寸", _regionHeight), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(gridContainer, forceReadableName: false, InternalMode.Disabled);
		SpinBox[] array2 = new SpinBox[4] { _regionX, _regionY, _regionWidth, _regionHeight };
		foreach (SpinBox obj in array2)
		{
			obj.ValueChanged += (double _) =>
			{
				OnRegionValueChanged();
			};
			obj.GetLineEdit().FocusEntered += BeginRegionEdit;
			obj.GetLineEdit().FocusExited += CommitRegionEdit;
		}
	}

	private void OnGridChanged()
	{
		if (!_updating)
		{
			SetGridWithHistory(Mathf.RoundToInt(_columns.Value), Mathf.RoundToInt(_rows.Value));
		}
	}

	private void OnFramePicked(int frame)
	{
		SetFrameWithHistory(frame);
	}

	private void OnRegionToggled(bool enabled)
	{
		if (!_updating && GodotObject.IsInstanceValid(_sprite))
		{
			Rect2 rect = ReadRegionControls();
			if (enabled && (rect.Size.X <= 0f || rect.Size.Y <= 0f) && GodotObject.IsInstanceValid(_sprite.Texture))
			{
				rect = new Rect2(Vector2.Zero, _sprite.Texture.GetSize());
			}
			SetRegionWithHistory(enabled, rect);
		}
	}

	private void BeginRegionEdit()
	{
		if (!_updating && !_regionEditActive && GodotObject.IsInstanceValid(_sprite))
		{
			_regionEditActive = true;
			_regionEditStart = _sprite.RegionRect;
		}
	}

	private void OnRegionValueChanged()
	{
		if (!_updating && GodotObject.IsInstanceValid(_sprite))
		{
			if (!_regionEditActive)
			{
				BeginRegionEdit();
			}
			Rect2 regionRect = SanitizeRegionRect(ReadRegionControls());
			_sprite.RegionRect = regionRect;
			RefreshCanvasAndLabels();
			_editor?.QueueViewportRedraw();
		}
	}

	private void CommitRegionEdit()
	{
		if (_regionEditActive && GodotObject.IsInstanceValid(_sprite))
		{
			_regionEditActive = false;
			Rect2 regionRect = _sprite.RegionRect;
			_sprite.RegionRect = _regionEditStart;
			SetRegionWithHistory(_sprite.RegionEnabled, regionRect);
		}
	}

	private Rect2 ReadRegionControls()
	{
		return new Rect2(new Vector2((float)_regionX.Value, (float)_regionY.Value), new Vector2((float)_regionWidth.Value, (float)_regionHeight.Value));
	}

	private Rect2 SanitizeRegionRect(Rect2 rect)
	{
		Vector2 position = rect.Position;
		Vector2 size = new Vector2(Mathf.Max(0f, rect.Size.X), Mathf.Max(0f, rect.Size.Y));
		if (GodotObject.IsInstanceValid(_sprite?.Texture))
		{
			Vector2 size2 = _sprite.Texture.GetSize();
			position.X = Mathf.Clamp(position.X, 0f, size2.X);
			position.Y = Mathf.Clamp(position.Y, 0f, size2.Y);
			size.X = Mathf.Clamp(size.X, 0f, size2.X - position.X);
			size.Y = Mathf.Clamp(size.Y, 0f, size2.Y - position.Y);
		}
		return new Rect2(position, size);
	}

	private void UpdateRegionFieldState()
	{
		bool editable = GodotObject.IsInstanceValid(_sprite) && _regionEnabled.ButtonPressed;
		_regionX.Editable = editable;
		_regionY.Editable = editable;
		_regionWidth.Editable = editable;
		_regionHeight.Editable = editable;
	}

	private void RefreshCanvasAndLabels()
	{
		if (!GodotObject.IsInstanceValid(_sprite))
		{
			_canvas.Bind(null, 1, 1, 0, regionEnabled: false, default);
			_frameLabel.Text = "未选择精灵";
			_status.Text = "选择 Sprite2D 后可直接点击贴图选帧";
			return;
		}
		int num = Math.Max(1, _sprite.Hframes);
		int num2 = Math.Max(1, _sprite.Vframes);
		int num3 = Math.Clamp(_sprite.Frame, 0, num * num2 - 1);
		_canvas.Bind(_sprite.Texture, num, num2, num3, _sprite.RegionEnabled, _sprite.RegionRect);
		_frameLabel.Text = $"帧 {num3} · 第 {num3 % num + 1} 列 / 第 {num3 / num + 1} 行";
		Label status = _status;
		string text;
		if (GodotObject.IsInstanceValid(_sprite.Texture))
		{
			text = (_canvas.IsGuideDensityLimited ? "网格过密：辅助线已抽样，点选精度不受影响" : $"{num * num2} 个可视帧 · 点击即写入场景");
		}
		else
		{
			text = "当前精灵还没有贴图";
		}
		status.Text = text;
		_previous.Disabled = num3 <= 0;
		_next.Disabled = num3 >= num * num2 - 1;
		UpdateRegionFieldState();
	}

	private static VBoxContainer MakeSectionTitle(string title, string hint)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		Label label = new Label
		{
			Text = title
		};
		label.AddThemeFontSizeOverride("font_size", 15);
		label.AddThemeColorOverride("font_color", new Color("#f0cf7a"));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Label label2 = new Label
		{
			Text = hint,
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		label2.AddThemeColorOverride("font_color", new Color("#829fb2"));
		vBoxContainer.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static Control Labeled(string text, Control control)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		Label label = new Label
		{
			Text = text
		};
		label.AddThemeFontSizeOverride("font_size", 11);
		label.AddThemeColorOverride("font_color", new Color("#8ca8bb"));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static SpinBox MakeSpin(double minimum, double maximum, double step, string suffix)
	{
		return new SpinBox
		{
			MinValue = minimum,
			MaxValue = maximum,
			Step = step,
			Suffix = suffix,
			AllowGreater = false,
			AllowLesser = false,
			CustomMinimumSize = new Vector2(110f, 34f)
		};
	}

	private static Button MakeButton(string text)
	{
		Button button = new Button();
		button.Text = text;
		button.CustomMinimumSize = new Vector2(54f, 34f);
		button.AddThemeStyleboxOverride("normal", MakeStyle(new Color("#203548"), new Color("#3e6680"), 6, 1));
		button.AddThemeStyleboxOverride("hover", MakeStyle(new Color("#2b4a60"), new Color("#65b7d5"), 6, 1));
		button.AddThemeStyleboxOverride("pressed", MakeStyle(new Color("#152633"), new Color("#f2c961"), 6, 1));
		return button;
	}

	private static StyleBoxFlat MakeStyle(Color background, Color border, int radius, int width)
	{
		return new StyleBoxFlat
		{
			BgColor = background,
			BorderColor = border,
			BorderWidthLeft = width,
			BorderWidthTop = width,
			BorderWidthRight = width,
			BorderWidthBottom = width,
			CornerRadiusTopLeft = radius,
			CornerRadiusTopRight = radius,
			CornerRadiusBottomLeft = radius,
			CornerRadiusBottomRight = radius,
			ContentMarginLeft = 8f,
			ContentMarginTop = 7f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 7f
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshFromSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetGridWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "columns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rows", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFrameWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRegionWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildInterface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGridChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFramePicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRegionToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginRegionEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRegionValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitRegionEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadRegionControls, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SanitizeRegionRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRegionFieldState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCanvasAndLabels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MakeSectionTitle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Labeled, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSpin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "suffix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "border", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BindEditor && args.Count == 1)
		{
			BindEditor(VariantUtils.ConvertTo<XW2DSceneEditor>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSprite && args.Count == 1)
		{
			BindSprite(VariantUtils.ConvertTo<Sprite2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFromSprite && args.Count == 0)
		{
			RefreshFromSprite();
			ret = default;
			return true;
		}
		if (method == MethodName.SetGridWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetGridWithHistory(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.SetFrameWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetFrameWithHistory(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetRegionWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetRegionWithHistory(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildInterface && args.Count == 0)
		{
			BuildInterface();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGridChanged && args.Count == 0)
		{
			OnGridChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFramePicked && args.Count == 1)
		{
			OnFramePicked(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRegionToggled && args.Count == 1)
		{
			OnRegionToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginRegionEdit && args.Count == 0)
		{
			BeginRegionEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRegionValueChanged && args.Count == 0)
		{
			OnRegionValueChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitRegionEdit && args.Count == 0)
		{
			CommitRegionEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadRegionControls && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(ReadRegionControls());
			return true;
		}
		if (method == MethodName.SanitizeRegionRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(SanitizeRegionRect(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateRegionFieldState && args.Count == 0)
		{
			UpdateRegionFieldState();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCanvasAndLabels && args.Count == 0)
		{
			RefreshCanvasAndLabels();
			ret = default;
			return true;
		}
		if (method == MethodName.MakeSectionTitle && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeSectionTitle(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Labeled && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(Labeled(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeSpin && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(MakeSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.MakeButton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Button>(MakeButton(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeStyle && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(MakeStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MakeSectionTitle && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeSectionTitle(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Labeled && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(Labeled(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeSpin && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(MakeSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.MakeButton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Button>(MakeButton(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeStyle && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(MakeStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
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
		if (method == MethodName.BindEditor)
		{
			return true;
		}
		if (method == MethodName.BindSprite)
		{
			return true;
		}
		if (method == MethodName.RefreshFromSprite)
		{
			return true;
		}
		if (method == MethodName.SetGridWithHistory)
		{
			return true;
		}
		if (method == MethodName.SetFrameWithHistory)
		{
			return true;
		}
		if (method == MethodName.SetRegionWithHistory)
		{
			return true;
		}
		if (method == MethodName.BuildInterface)
		{
			return true;
		}
		if (method == MethodName.OnGridChanged)
		{
			return true;
		}
		if (method == MethodName.OnFramePicked)
		{
			return true;
		}
		if (method == MethodName.OnRegionToggled)
		{
			return true;
		}
		if (method == MethodName.BeginRegionEdit)
		{
			return true;
		}
		if (method == MethodName.OnRegionValueChanged)
		{
			return true;
		}
		if (method == MethodName.CommitRegionEdit)
		{
			return true;
		}
		if (method == MethodName.ReadRegionControls)
		{
			return true;
		}
		if (method == MethodName.SanitizeRegionRect)
		{
			return true;
		}
		if (method == MethodName.UpdateRegionFieldState)
		{
			return true;
		}
		if (method == MethodName.RefreshCanvasAndLabels)
		{
			return true;
		}
		if (method == MethodName.MakeSectionTitle)
		{
			return true;
		}
		if (method == MethodName.Labeled)
		{
			return true;
		}
		if (method == MethodName.MakeSpin)
		{
			return true;
		}
		if (method == MethodName.MakeButton)
		{
			return true;
		}
		if (method == MethodName.MakeStyle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XW2DSceneEditor>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._canvas)
		{
			_canvas = VariantUtils.ConvertTo<XW2DSpriteAtlasCanvas>(in value);
			return true;
		}
		if (name == PropertyName._title)
		{
			_title = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._status)
		{
			_status = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._frameLabel)
		{
			_frameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._columns)
		{
			_columns = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rows)
		{
			_rows = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._regionX)
		{
			_regionX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._regionY)
		{
			_regionY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._regionWidth)
		{
			_regionWidth = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._regionHeight)
		{
			_regionHeight = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._regionEnabled)
		{
			_regionEnabled = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._previous)
		{
			_previous = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._next)
		{
			_next = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._updating)
		{
			_updating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._regionEditActive)
		{
			_regionEditActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._regionEditStart)
		{
			_regionEditStart = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CurrentSprite)
		{
			value = VariantUtils.CreateFrom<Sprite2D>(CurrentSprite);
			return true;
		}
		if (name == PropertyName.AtlasCanvas)
		{
			value = VariantUtils.CreateFrom<XW2DSpriteAtlasCanvas>(AtlasCanvas);
			return true;
		}
		if (name == PropertyName.StatusText)
		{
			value = VariantUtils.CreateFrom<string>(StatusText);
			return true;
		}
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName._canvas)
		{
			value = VariantUtils.CreateFrom(in _canvas);
			return true;
		}
		if (name == PropertyName._title)
		{
			value = VariantUtils.CreateFrom(in _title);
			return true;
		}
		if (name == PropertyName._status)
		{
			value = VariantUtils.CreateFrom(in _status);
			return true;
		}
		if (name == PropertyName._frameLabel)
		{
			value = VariantUtils.CreateFrom(in _frameLabel);
			return true;
		}
		if (name == PropertyName._columns)
		{
			value = VariantUtils.CreateFrom(in _columns);
			return true;
		}
		if (name == PropertyName._rows)
		{
			value = VariantUtils.CreateFrom(in _rows);
			return true;
		}
		if (name == PropertyName._regionX)
		{
			value = VariantUtils.CreateFrom(in _regionX);
			return true;
		}
		if (name == PropertyName._regionY)
		{
			value = VariantUtils.CreateFrom(in _regionY);
			return true;
		}
		if (name == PropertyName._regionWidth)
		{
			value = VariantUtils.CreateFrom(in _regionWidth);
			return true;
		}
		if (name == PropertyName._regionHeight)
		{
			value = VariantUtils.CreateFrom(in _regionHeight);
			return true;
		}
		if (name == PropertyName._regionEnabled)
		{
			value = VariantUtils.CreateFrom(in _regionEnabled);
			return true;
		}
		if (name == PropertyName._previous)
		{
			value = VariantUtils.CreateFrom(in _previous);
			return true;
		}
		if (name == PropertyName._next)
		{
			value = VariantUtils.CreateFrom(in _next);
			return true;
		}
		if (name == PropertyName._updating)
		{
			value = VariantUtils.CreateFrom(in _updating);
			return true;
		}
		if (name == PropertyName._regionEditActive)
		{
			value = VariantUtils.CreateFrom(in _regionEditActive);
			return true;
		}
		if (name == PropertyName._regionEditStart)
		{
			value = VariantUtils.CreateFrom(in _regionEditStart);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._title, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._status, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._frameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._columns, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rows, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._regionX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._regionY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._regionWidth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._regionHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._regionEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previous, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._next, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._regionEditActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._regionEditStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.AtlasCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.StatusText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName._canvas, Variant.From(in _canvas));
		info.AddProperty(PropertyName._title, Variant.From(in _title));
		info.AddProperty(PropertyName._status, Variant.From(in _status));
		info.AddProperty(PropertyName._frameLabel, Variant.From(in _frameLabel));
		info.AddProperty(PropertyName._columns, Variant.From(in _columns));
		info.AddProperty(PropertyName._rows, Variant.From(in _rows));
		info.AddProperty(PropertyName._regionX, Variant.From(in _regionX));
		info.AddProperty(PropertyName._regionY, Variant.From(in _regionY));
		info.AddProperty(PropertyName._regionWidth, Variant.From(in _regionWidth));
		info.AddProperty(PropertyName._regionHeight, Variant.From(in _regionHeight));
		info.AddProperty(PropertyName._regionEnabled, Variant.From(in _regionEnabled));
		info.AddProperty(PropertyName._previous, Variant.From(in _previous));
		info.AddProperty(PropertyName._next, Variant.From(in _next));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
		info.AddProperty(PropertyName._regionEditActive, Variant.From(in _regionEditActive));
		info.AddProperty(PropertyName._regionEditStart, Variant.From(in _regionEditStart));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XW2DSceneEditor>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value2))
		{
			_sprite = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._canvas, out var value3))
		{
			_canvas = value3.As<XW2DSpriteAtlasCanvas>();
		}
		if (info.TryGetProperty(PropertyName._title, out var value4))
		{
			_title = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._status, out var value5))
		{
			_status = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._frameLabel, out var value6))
		{
			_frameLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._columns, out var value7))
		{
			_columns = value7.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rows, out var value8))
		{
			_rows = value8.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._regionX, out var value9))
		{
			_regionX = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._regionY, out var value10))
		{
			_regionY = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._regionWidth, out var value11))
		{
			_regionWidth = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._regionHeight, out var value12))
		{
			_regionHeight = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._regionEnabled, out var value13))
		{
			_regionEnabled = value13.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._previous, out var value14))
		{
			_previous = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._next, out var value15))
		{
			_next = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value16))
		{
			_updating = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._regionEditActive, out var value17))
		{
			_regionEditActive = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._regionEditStart, out var value18))
		{
			_regionEditStart = value18.As<Rect2>();
		}
	}
}
