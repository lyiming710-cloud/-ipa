using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Grid/XW2DGridSettingsPanel.cs")]
public class XW2DGridSettingsPanel : PanelContainer
{
	private readonly struct GridPreset(string name, string description, Vector2 gridStep, Vector2I primaryStep, Vector2 moveStep, Color accent)
	{
		public readonly string Name = name;

		public readonly string Description = description;

		public readonly Vector2 GridStep = gridStep;

		public readonly Vector2I PrimaryStep = primaryStep;

		public readonly Vector2 MoveStep = moveStep;

		public readonly Color Accent = accent;
	}

	private sealed class GridPresetTile : Button
	{
		public new class MethodName : Button.MethodName
		{
			public new static readonly StringName _Ready = "_Ready";

			public new static readonly StringName _Draw = "_Draw";
		}

		public new class PropertyName : Button.PropertyName
		{
			public static readonly StringName PresetName = "PresetName";

			public static readonly StringName Description = "Description";

			public static readonly StringName Accent = "Accent";

			public static readonly StringName GridDivisions = "GridDivisions";
		}

		public new class SignalName : Button.SignalName
		{
		}

		public string PresetName = "";

		public string Description = "";

		public Color Accent = Colors.White;

		public int GridDivisions = 4;

		public override void _Ready()
		{
			Text = "";
			Flat = true;
			MouseDefaultCursorShape = CursorShape.PointingHand;
		}

		public override void _Draw()
		{
			Color color = (ButtonPressed ? new Color("274838") : new Color("162936"));
			Color color2 = (ButtonPressed ? Accent : new Color("395669"));
			DrawRect(new Rect2(Vector2.Zero, Size), color);
			DrawRect(new Rect2(Vector2.One, Size - Vector2.One * 2f), color2, filled: false, ButtonPressed ? 2.5f : 1f);
			Rect2 rect = new Rect2(9f, 12f, 48f, 48f);
			DrawRect(rect, new Color("0d1820"));
			int num = Math.Clamp(GridDivisions, 2, 8);
			for (int i = 0; i <= num; i++)
			{
				float weight = (float)i / (float)num;
				float x = Mathf.Lerp(rect.Position.X, rect.End.X, weight);
				float y = Mathf.Lerp(rect.Position.Y, rect.End.Y, weight);
				Color color3;
				if (i != 0 && i != num)
				{
					Color accent = Accent;
					accent.A = 0.35f;
					color3 = accent;
				}
				else
				{
					color3 = Accent;
				}
				Color color4 = color3;
				DrawLine(new Vector2(x, rect.Position.Y), new Vector2(x, rect.End.Y), color4, (i == 0 || i == num) ? 1.4f : 1f);
				DrawLine(new Vector2(rect.Position.X, y), new Vector2(rect.End.X, y), color4, (i == 0 || i == num) ? 1.4f : 1f);
			}
			DrawCircle(rect.GetCenter(), 3.5f, Accent);
			Font themeDefaultFont = GetThemeDefaultFont();
			DrawString(themeDefaultFont, new Vector2(66f, 33f), PresetName, HorizontalAlignment.Left, Math.Max(1f, Size.X - 72f), 13, ButtonPressed ? Accent.Lightened(0.2f) : new Color("e5edf0"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawString(themeDefaultFont, new Vector2(66f, 54f), Description, HorizontalAlignment.Left, Math.Max(1f, Size.X - 72f), 11, new Color("91abba"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
			DrawString(themeDefaultFont, new Vector2(10f, 78f), ButtonPressed ? "✓ 已选" : "点击套用", HorizontalAlignment.Left, Math.Max(1f, Size.X - 20f), 10, ButtonPressed ? Accent : new Color("718c9b"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(2)
			{
				new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
				new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
			if (method == MethodName._Draw && args.Count == 0)
			{
				_Draw();
				ret = default;
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName._Ready)
			{
				return true;
			}
			if (method == MethodName._Draw)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName.PresetName)
			{
				PresetName = VariantUtils.ConvertTo<string>(in value);
				return true;
			}
			if (name == PropertyName.Description)
			{
				Description = VariantUtils.ConvertTo<string>(in value);
				return true;
			}
			if (name == PropertyName.Accent)
			{
				Accent = VariantUtils.ConvertTo<Color>(in value);
				return true;
			}
			if (name == PropertyName.GridDivisions)
			{
				GridDivisions = VariantUtils.ConvertTo<int>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if (name == PropertyName.PresetName)
			{
				value = VariantUtils.CreateFrom(in PresetName);
				return true;
			}
			if (name == PropertyName.Description)
			{
				value = VariantUtils.CreateFrom(in Description);
				return true;
			}
			if (name == PropertyName.Accent)
			{
				value = VariantUtils.CreateFrom(in Accent);
				return true;
			}
			if (name == PropertyName.GridDivisions)
			{
				value = VariantUtils.CreateFrom(in GridDivisions);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, PropertyName.PresetName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.String, PropertyName.Description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Color, PropertyName.Accent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Int, PropertyName.GridDivisions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName.PresetName, Variant.From(in PresetName));
			info.AddProperty(PropertyName.Description, Variant.From(in Description));
			info.AddProperty(PropertyName.Accent, Variant.From(in Accent));
			info.AddProperty(PropertyName.GridDivisions, Variant.From(in GridDivisions));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName.PresetName, out var value))
			{
				PresetName = value.As<string>();
			}
			if (info.TryGetProperty(PropertyName.Description, out var value2))
			{
				Description = value2.As<string>();
			}
			if (info.TryGetProperty(PropertyName.Accent, out var value3))
			{
				Accent = value3.As<Color>();
			}
			if (info.TryGetProperty(PropertyName.GridDivisions, out var value4))
			{
				GridDivisions = value4.As<int>();
			}
		}
	}

	private sealed class GridPreviewCanvas : Control
	{
		public new class MethodName : Control.MethodName
		{
			public new static readonly StringName _Draw = "_Draw";
		}

		public new class PropertyName : Control.PropertyName
		{
			public static readonly StringName OwnerPanel = "OwnerPanel";
		}

		public new class SignalName : Control.SignalName
		{
		}

		public XW2DGridSettingsPanel OwnerPanel;

		public override void _Draw()
		{
			DrawRect(new Rect2(Vector2.Zero, Size), new Color("09131a"));
			if (GodotObject.IsInstanceValid(OwnerPanel))
			{
				float num = Math.Clamp(OwnerPanel._gridStep.X * 1.7f, 12f, 38f);
				float num2 = Math.Clamp(OwnerPanel._gridStep.Y * 1.7f, 12f, 38f);
				float num3 = Mathf.PosMod(OwnerPanel._gridOffset.X * 0.35f, num);
				float num4 = Mathf.PosMod(OwnerPanel._gridOffset.Y * 0.35f, num2);
				int num5 = 0;
				float num6 = num3;
				while (num6 <= Size.X)
				{
					bool flag = num5 % Math.Max(1, OwnerPanel._primaryGridStep.X) == 0;
					DrawLine(new Vector2(num6, 0f), new Vector2(num6, Size.Y), flag ? new Color("4f7666aa") : new Color("28443c88"), flag ? 1.5f : 1f);
					num6 += num;
					num5++;
				}
				int num7 = 0;
				float num8 = num4;
				while (num8 <= Size.Y)
				{
					bool flag2 = num7 % Math.Max(1, OwnerPanel._primaryGridStep.Y) == 0;
					DrawLine(new Vector2(0f, num8), new Vector2(Size.X, num8), flag2 ? new Color("4f7666aa") : new Color("28443c88"), flag2 ? 1.5f : 1f);
					num8 += num2;
					num7++;
				}
				Vector2 vector = Size * 0.5f;
				float radius = Math.Clamp(OwnerPanel._smartSnapThreshold * 1.35f, 8f, 64f);
				DrawCircle(vector, radius, new Color("ffd46a18"));
				DrawArc(vector, radius, 0f, (float)Math.PI * 2f, 64, new Color("ffd46acc"), 2f, antialiased: true);
				DrawLine(new Vector2(vector.X, 0f), new Vector2(vector.X, Size.Y), new Color("ff7373aa"), 1.5f);
				DrawLine(new Vector2(0f, vector.Y), new Vector2(Size.X, vector.Y), new Color("76d88baa"), 1.5f);
				DrawCircle(vector, 5f, new Color("ffe088"));
				Font themeDefaultFont = GetThemeDefaultFont();
				DrawString(themeDefaultFont, new Vector2(12f, 24f), "实时网格预览", HorizontalAlignment.Left, 150f, 13, new Color("d8ebdd"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				DrawString(themeDefaultFont, new Vector2(12f, Size.Y - 12f), $"◎ 智能吸附感应半径 {OwnerPanel._smartSnapThreshold:0.#} px", HorizontalAlignment.Left, Math.Max(1f, Size.X - 24f), 11, new Color("f4d47d"), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
				DrawRect(new Rect2(Vector2.Zero, Size), new Color("436a78"), filled: false, 1f);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(1)
			{
				new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if (method == MethodName._Draw && args.Count == 0)
			{
				_Draw();
				ret = default;
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName._Draw)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName.OwnerPanel)
			{
				OwnerPanel = VariantUtils.ConvertTo<XW2DGridSettingsPanel>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			if (name == PropertyName.OwnerPanel)
			{
				value = VariantUtils.CreateFrom(in OwnerPanel);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, PropertyName.OwnerPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName.OwnerPanel, Variant.From(in OwnerPanel));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName.OwnerPanel, out var value))
			{
				OwnerPanel = value.As<XW2DGridSettingsPanel>();
			}
		}
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BindState = "BindState";

		public static readonly StringName CaptureState = "CaptureState";

		public static readonly StringName ResetSettings = "ResetSettings";

		public static readonly StringName BuildInterface = "BuildInterface";

		public static readonly StringName ApplyPreset = "ApplyPreset";

		public static readonly StringName SetGridOffset = "SetGridOffset";

		public static readonly StringName SetGridStep = "SetGridStep";

		public static readonly StringName SetPrimaryGridStep = "SetPrimaryGridStep";

		public static readonly StringName SetSmartSnapThreshold = "SetSmartSnapThreshold";

		public static readonly StringName SetMoveSnapStep = "SetMoveSnapStep";

		public static readonly StringName SetRotateSnapStep = "SetRotateSnapStep";

		public static readonly StringName SetScaleSnapStep = "SetScaleSnapStep";

		public static readonly StringName SyncControls = "SyncControls";

		public static readonly StringName UpdatePresetSelection = "UpdatePresetSelection";

		public static readonly StringName NotifySettingsChanged = "NotifySettingsChanged";

		public static readonly StringName ClampOffset = "ClampOffset";

		public static readonly StringName ClampPositiveVector = "ClampPositiveVector";

		public static readonly StringName ClampPrimaryStep = "ClampPrimaryStep";

		public static readonly StringName ClampSmartThreshold = "ClampSmartThreshold";

		public static readonly StringName SetSpinValue = "SetSpinValue";

		public static readonly StringName MakeSection = "MakeSection";

		public static readonly StringName MakeStandaloneSection = "MakeStandaloneSection";

		public static readonly StringName MakeVectorRow = "MakeVectorRow";

		public static readonly StringName MakeScalarRow = "MakeScalarRow";

		public static readonly StringName MakeAxisCell = "MakeAxisCell";

		public static readonly StringName MakeSpin = "MakeSpin";

		public static readonly StringName MakeButton = "MakeButton";

		public static readonly StringName MakeBadge = "MakeBadge";

		public static readonly StringName MakeLabel = "MakeLabel";

		public static readonly StringName MakePanelStyle = "MakePanelStyle";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName GridOffset = "GridOffset";

		public static readonly StringName GridStep = "GridStep";

		public static readonly StringName PrimaryGridStep = "PrimaryGridStep";

		public static readonly StringName SmartSnapThreshold = "SmartSnapThreshold";

		public static readonly StringName MoveSnapStep = "MoveSnapStep";

		public static readonly StringName RotateSnapStep = "RotateSnapStep";

		public static readonly StringName ScaleSnapStep = "ScaleSnapStep";

		public static readonly StringName _gridOffset = "_gridOffset";

		public static readonly StringName _gridStep = "_gridStep";

		public static readonly StringName _primaryGridStep = "_primaryGridStep";

		public static readonly StringName _smartSnapThreshold = "_smartSnapThreshold";

		public static readonly StringName _moveSnapStep = "_moveSnapStep";

		public static readonly StringName _rotateSnapStep = "_rotateSnapStep";

		public static readonly StringName _scaleSnapStep = "_scaleSnapStep";

		public static readonly StringName _preview = "_preview";

		public static readonly StringName _offsetX = "_offsetX";

		public static readonly StringName _offsetY = "_offsetY";

		public static readonly StringName _gridX = "_gridX";

		public static readonly StringName _gridY = "_gridY";

		public static readonly StringName _primaryX = "_primaryX";

		public static readonly StringName _primaryY = "_primaryY";

		public static readonly StringName _moveX = "_moveX";

		public static readonly StringName _moveY = "_moveY";

		public static readonly StringName _rotateStep = "_rotateStep";

		public static readonly StringName _scaleStep = "_scaleStep";

		public static readonly StringName _thresholdSlider = "_thresholdSlider";

		public static readonly StringName _thresholdSpin = "_thresholdSpin";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private const float MinimumGridStep = 0.25f;

	private const float MaximumGridStep = 1024f;

	private const float MinimumSmartThreshold = 1f;

	private const float MaximumSmartThreshold = 64f;

	private static readonly GridPreset[] Presets = new GridPreset[4]
	{
		new GridPreset("精细像素", "4 × 4", new Vector2(4f, 4f), new Vector2I(8, 8), new Vector2(4f, 4f), new Color("79d7ff")),
		new GridPreset("经典像素", "8 × 8", new Vector2(8f, 8f), new Vector2I(8, 8), new Vector2(8f, 8f), new Color("8be58d")),
		new GridPreset("标准场景", "16 × 16", new Vector2(16f, 16f), new Vector2I(4, 4), new Vector2(16f, 16f), new Color("ffd36f")),
		new GridPreset("大地图", "32 × 32", new Vector2(32f, 32f), new Vector2I(4, 4), new Vector2(32f, 32f), new Color("ff9d73"))
	};

	private Vector2 _gridOffset = Vector2.Zero;

	private Vector2 _gridStep = new Vector2(8f, 8f);

	private Vector2I _primaryGridStep = new Vector2I(8, 8);

	private float _smartSnapThreshold = 8f;

	private Vector2 _moveSnapStep = new Vector2(8f, 8f);

	private float _rotateSnapStep = 15f;

	private float _scaleSnapStep = 0.1f;

	private GridPreviewCanvas _preview;

	private SpinBox _offsetX;

	private SpinBox _offsetY;

	private SpinBox _gridX;

	private SpinBox _gridY;

	private SpinBox _primaryX;

	private SpinBox _primaryY;

	private SpinBox _moveX;

	private SpinBox _moveY;

	private SpinBox _rotateStep;

	private SpinBox _scaleStep;

	private HSlider _thresholdSlider;

	private SpinBox _thresholdSpin;

	private Label _summaryLabel;

	private readonly List<GridPresetTile> _presetTiles = new List<GridPresetTile>();

	private bool _updatingControls;

	public Vector2 GridOffset
	{
		get
		{
			return _gridOffset;
		}
		set
		{
			SetGridOffset(value, notify: true);
		}
	}

	public Vector2 GridStep
	{
		get
		{
			return _gridStep;
		}
		set
		{
			SetGridStep(value, notify: true);
		}
	}

	public Vector2I PrimaryGridStep
	{
		get
		{
			return _primaryGridStep;
		}
		set
		{
			SetPrimaryGridStep(value, notify: true);
		}
	}

	public float SmartSnapThreshold
	{
		get
		{
			return _smartSnapThreshold;
		}
		set
		{
			SetSmartSnapThreshold(value, notify: true);
		}
	}

	public Vector2 MoveSnapStep
	{
		get
		{
			return _moveSnapStep;
		}
		set
		{
			SetMoveSnapStep(value, notify: true);
		}
	}

	public float RotateSnapStep
	{
		get
		{
			return _rotateSnapStep;
		}
		set
		{
			SetRotateSnapStep(value, notify: true);
		}
	}

	public float ScaleSnapStep
	{
		get
		{
			return _scaleSnapStep;
		}
		set
		{
			SetScaleSnapStep(value, notify: true);
		}
	}

	public event Action SettingsChanged;

	public override void _Ready()
	{
		BuildInterface();
		SyncControls();
	}

	public void BindState(Dictionary state)
	{
		if (state == null)
		{
			SyncControls();
			return;
		}
		if (state.ContainsKey("grid_offset"))
		{
			_gridOffset = ClampOffset(state["grid_offset"].AsVector2());
		}
		if (state.ContainsKey("grid_step"))
		{
			_gridStep = ClampPositiveVector(state["grid_step"].AsVector2(), 0.25f, 1024f);
		}
		if (state.ContainsKey("primary_grid_step"))
		{
			_primaryGridStep = ClampPrimaryStep(state["primary_grid_step"].AsVector2I());
		}
		if (state.ContainsKey("smart_snap_threshold"))
		{
			_smartSnapThreshold = ClampSmartThreshold(state["smart_snap_threshold"].AsSingle());
		}
		if (state.ContainsKey("move_snap_step"))
		{
			_moveSnapStep = ClampPositiveVector(state["move_snap_step"].AsVector2(), 0.25f, 1024f);
		}
		if (state.ContainsKey("rotate_snap_step"))
		{
			_rotateSnapStep = Math.Clamp(state["rotate_snap_step"].AsSingle(), 0.1f, 180f);
		}
		if (state.ContainsKey("scale_snap_step"))
		{
			_scaleSnapStep = Math.Clamp(state["scale_snap_step"].AsSingle(), 0.001f, 10f);
		}
		SyncControls();
	}

	public Dictionary CaptureState()
	{
		return new Dictionary
		{
			["grid_offset"] = _gridOffset,
			["grid_step"] = _gridStep,
			["primary_grid_step"] = _primaryGridStep,
			["smart_snap_threshold"] = _smartSnapThreshold,
			["move_snap_step"] = _moveSnapStep,
			["rotate_snap_step"] = _rotateSnapStep,
			["scale_snap_step"] = _scaleSnapStep
		};
	}

	public void ResetSettings()
	{
		_gridOffset = Vector2.Zero;
		_gridStep = new Vector2(8f, 8f);
		_primaryGridStep = new Vector2I(8, 8);
		_smartSnapThreshold = 8f;
		_moveSnapStep = new Vector2(8f, 8f);
		_rotateSnapStep = 15f;
		_scaleSnapStep = 0.1f;
		SyncControls();
		NotifySettingsChanged();
	}

	private void BuildInterface()
	{
		if (GodotObject.IsInstanceValid(_preview))
		{
			return;
		}
		CustomMinimumSize = new Vector2(520f, 610f);
		AddThemeStyleboxOverride("panel", MakePanelStyle(new Color("101b24"), new Color("41657c"), 12, 1));
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "GridSettingsSurface",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 9);
		AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer();
		Label node = MakeBadge("▦", new Color("7ee29d"));
		hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer2 = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		Label node2 = MakeLabel("网格与吸附", 20, new Color("f5dd91"));
		Label node3 = MakeLabel("用图块选择常用网格，再直接微调坐标和手感", 12, new Color("91afbf"));
		vBoxContainer2.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(node3, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		Button button = MakeButton("↺ 恢复默认", "恢复 8 像素网格、15° 旋转与 10% 缩放步长");
		button.Pressed += ResetSettings;
		hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		_preview = new GridPreviewCanvas
		{
			Name = "GridPreview",
			OwnerPanel = this,
			CustomMinimumSize = new Vector2(0f, 150f),
			MouseFilter = MouseFilterEnum.Ignore
		};
		vBoxContainer.AddChild(_preview, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer3 = MakeSection(vBoxContainer, "网格图块", "点击图块立即配置次网格、主网格与移动吸附");
		HBoxContainer hBoxContainer2 = new HBoxContainer();
		hBoxContainer2.AddThemeConstantOverride("separation", 7);
		for (int i = 0; i < Presets.Length; i++)
		{
			int captured = i;
			GridPreset gridPreset = Presets[i];
			GridPresetTile gridPresetTile = new GridPresetTile
			{
				Name = $"Preset{i}",
				PresetName = gridPreset.Name,
				Description = gridPreset.Description,
				Accent = gridPreset.Accent,
				GridDivisions = Math.Clamp(36 / Math.Max(1, Mathf.RoundToInt(gridPreset.GridStep.X)), 2, 7),
				ToggleMode = true,
				CustomMinimumSize = new Vector2(128f, 88f),
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				TooltipText = gridPreset.Name + "：" + gridPreset.Description + " 像素"
			};
			gridPresetTile.Pressed += () =>
			{
				ApplyPreset(captured);
			};
			_presetTiles.Add(gridPresetTile);
			hBoxContainer2.AddChild(gridPresetTile, forceReadableName: false, InternalMode.Disabled);
		}
		vBoxContainer3.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		HSplitContainer hSplitContainer = new HSplitContainer();
		hSplitContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hSplitContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
		hSplitContainer.SplitOffsets = new int[1] { 260 };
		HSplitContainer hSplitContainer2 = hSplitContainer;
		vBoxContainer.AddChild(hSplitContainer2, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer4 = MakeStandaloneSection("网格坐标", "图中十字代表原点偏移；粗线间隔由主网格控制");
		hSplitContainer2.AddChild(vBoxContainer4, forceReadableName: false, InternalMode.Disabled);
		_offsetX = MakeSpin(-100000.0, 100000.0, 1.0, " px");
		_offsetY = MakeSpin(-100000.0, 100000.0, 1.0, " px");
		_gridX = MakeSpin(0.25, 1024.0, 0.25, " px");
		_gridY = MakeSpin(0.25, 1024.0, 0.25, " px");
		_primaryX = MakeSpin(1.0, 64.0, 1.0, " 格");
		_primaryY = MakeSpin(1.0, 64.0, 1.0, " 格");
		vBoxContainer4.AddChild(MakeVectorRow("⌖", "原点偏移", _offsetX, _offsetY, new Color("79c9ff")), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer4.AddChild(MakeVectorRow("▦", "次网格间距", _gridX, _gridY, new Color("87e0a2")), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer4.AddChild(MakeVectorRow("▣", "主网格倍率", _primaryX, _primaryY, new Color("f5cf71")), forceReadableName: false, InternalMode.Disabled);
		_offsetX.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				GridOffset = new Vector2((float)value, _gridOffset.Y);
			}
		};
		_offsetY.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				GridOffset = new Vector2(_gridOffset.X, (float)value);
			}
		};
		_gridX.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				GridStep = new Vector2((float)value, _gridStep.Y);
			}
		};
		_gridY.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				GridStep = new Vector2(_gridStep.X, (float)value);
			}
		};
		_primaryX.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				PrimaryGridStep = new Vector2I((int)Math.Round(value), _primaryGridStep.Y);
			}
		};
		_primaryY.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				PrimaryGridStep = new Vector2I(_primaryGridStep.X, (int)Math.Round(value));
			}
		};
		VBoxContainer vBoxContainer5 = MakeStandaloneSection("吸附手感", "拖动滑杆或数字；单位图标会同步显示实际效果");
		hSplitContainer2.AddChild(vBoxContainer5, forceReadableName: false, InternalMode.Disabled);
		_moveX = MakeSpin(0.25, 1024.0, 0.25, " px");
		_moveY = MakeSpin(0.25, 1024.0, 0.25, " px");
		_rotateStep = MakeSpin(0.1, 180.0, 0.1, "°");
		_scaleStep = MakeSpin(0.001, 10.0, 0.01, " ×");
		vBoxContainer5.AddChild(MakeVectorRow("↔", "移动步长", _moveX, _moveY, new Color("76d5ff")), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer5.AddChild(MakeScalarRow("◔", "旋转步长", _rotateStep, new Color("ffb66f")), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer5.AddChild(MakeScalarRow("◇", "缩放步长", _scaleStep, new Color("c99cff")), forceReadableName: false, InternalMode.Disabled);
		_moveX.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				MoveSnapStep = new Vector2((float)value, _moveSnapStep.Y);
			}
		};
		_moveY.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				MoveSnapStep = new Vector2(_moveSnapStep.X, (float)value);
			}
		};
		_rotateStep.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				RotateSnapStep = (float)value;
			}
		};
		_scaleStep.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				ScaleSnapStep = (float)value;
			}
		};
		VBoxContainer vBoxContainer6 = MakeSection(vBoxContainer, "智能吸附感应圈", "黄色感应圈越大，节点边缘与中心线越容易被捕获");
		HBoxContainer hBoxContainer3 = new HBoxContainer();
		hBoxContainer3.AddChild(MakeBadge("◎", new Color("ffd86f")), forceReadableName: false, InternalMode.Disabled);
		_thresholdSlider = new HSlider
		{
			MinValue = 1.0,
			MaxValue = 64.0,
			Step = 1.0,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			CustomMinimumSize = new Vector2(220f, 34f),
			TooltipText = "屏幕像素感应半径"
		};
		_thresholdSpin = MakeSpin(1.0, 64.0, 1.0, " px");
		_thresholdSpin.CustomMinimumSize = new Vector2(108f, 34f);
		hBoxContainer3.AddChild(_thresholdSlider, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer3.AddChild(_thresholdSpin, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer6.AddChild(hBoxContainer3, forceReadableName: false, InternalMode.Disabled);
		_thresholdSlider.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				SmartSnapThreshold = (float)value;
			}
		};
		_thresholdSpin.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				SmartSnapThreshold = (float)value;
			}
		};
		_summaryLabel = MakeLabel("", 12, new Color("a6d5e8"));
		_summaryLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
		vBoxContainer.AddChild(_summaryLabel, forceReadableName: false, InternalMode.Disabled);
	}

	private void ApplyPreset(int index)
	{
		if (index >= 0 && index < Presets.Length)
		{
			GridPreset gridPreset = Presets[index];
			_gridStep = gridPreset.GridStep;
			_primaryGridStep = gridPreset.PrimaryStep;
			_moveSnapStep = gridPreset.MoveStep;
			SyncControls();
			NotifySettingsChanged();
		}
	}

	private void SetGridOffset(Vector2 value, bool notify)
	{
		Vector2 vector = ClampOffset(value);
		if (!_gridOffset.IsEqualApprox(vector))
		{
			_gridOffset = vector;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SetGridStep(Vector2 value, bool notify)
	{
		Vector2 vector = ClampPositiveVector(value, 0.25f, 1024f);
		if (!_gridStep.IsEqualApprox(vector))
		{
			_gridStep = vector;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SetPrimaryGridStep(Vector2I value, bool notify)
	{
		Vector2I vector2I = ClampPrimaryStep(value);
		if (!(_primaryGridStep == vector2I))
		{
			_primaryGridStep = vector2I;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SetSmartSnapThreshold(float value, bool notify)
	{
		float num = ClampSmartThreshold(value);
		if (!Mathf.IsEqualApprox(_smartSnapThreshold, num))
		{
			_smartSnapThreshold = num;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SetMoveSnapStep(Vector2 value, bool notify)
	{
		Vector2 vector = ClampPositiveVector(value, 0.25f, 1024f);
		if (!_moveSnapStep.IsEqualApprox(vector))
		{
			_moveSnapStep = vector;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SetRotateSnapStep(float value, bool notify)
	{
		float num = Math.Clamp(value, 0.1f, 180f);
		if (!Mathf.IsEqualApprox(_rotateSnapStep, num))
		{
			_rotateSnapStep = num;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SetScaleSnapStep(float value, bool notify)
	{
		float num = Math.Clamp(value, 0.001f, 10f);
		if (!Mathf.IsEqualApprox(_scaleSnapStep, num))
		{
			_scaleSnapStep = num;
			SyncControls();
			if (notify)
			{
				NotifySettingsChanged();
			}
		}
	}

	private void SyncControls()
	{
		_updatingControls = true;
		SetSpinValue(_offsetX, _gridOffset.X);
		SetSpinValue(_offsetY, _gridOffset.Y);
		SetSpinValue(_gridX, _gridStep.X);
		SetSpinValue(_gridY, _gridStep.Y);
		SetSpinValue(_primaryX, _primaryGridStep.X);
		SetSpinValue(_primaryY, _primaryGridStep.Y);
		SetSpinValue(_moveX, _moveSnapStep.X);
		SetSpinValue(_moveY, _moveSnapStep.Y);
		SetSpinValue(_rotateStep, _rotateSnapStep);
		SetSpinValue(_scaleStep, _scaleSnapStep);
		if (GodotObject.IsInstanceValid(_thresholdSlider))
		{
			_thresholdSlider.Value = _smartSnapThreshold;
		}
		SetSpinValue(_thresholdSpin, _smartSnapThreshold);
		_updatingControls = false;
		UpdatePresetSelection();
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = $"▦ {_gridStep.X:0.##} × {_gridStep.Y:0.##} px  ·  ↔ {_moveSnapStep.X:0.##} × {_moveSnapStep.Y:0.##} px  ·  ◔ {_rotateSnapStep:0.##}°  ·  ◇ {_scaleSnapStep * 100f:0.##}%  ·  ◎ {_smartSnapThreshold:0.#} px";
		}
		_preview?.QueueRedraw();
	}

	private void UpdatePresetSelection()
	{
		for (int i = 0; i < _presetTiles.Count; i++)
		{
			GridPreset gridPreset = Presets[i];
			bool pressedNoSignal = _gridStep.IsEqualApprox(gridPreset.GridStep) && _primaryGridStep == gridPreset.PrimaryStep && _moveSnapStep.IsEqualApprox(gridPreset.MoveStep);
			_presetTiles[i].SetPressedNoSignal(pressedNoSignal);
			_presetTiles[i].QueueRedraw();
		}
	}

	private void NotifySettingsChanged()
	{
		_preview?.QueueRedraw();
		SettingsChanged?.Invoke();
	}

	private static Vector2 ClampOffset(Vector2 value)
	{
		return new Vector2(Math.Clamp(value.X, -100000f, 100000f), Math.Clamp(value.Y, -100000f, 100000f));
	}

	private static Vector2 ClampPositiveVector(Vector2 value, float minimum, float maximum)
	{
		return new Vector2(Math.Clamp(value.X, minimum, maximum), Math.Clamp(value.Y, minimum, maximum));
	}

	private static Vector2I ClampPrimaryStep(Vector2I value)
	{
		return new Vector2I(Math.Clamp(value.X, 1, 64), Math.Clamp(value.Y, 1, 64));
	}

	private static float ClampSmartThreshold(float value)
	{
		return Math.Clamp(value, 1f, 64f);
	}

	private static void SetSpinValue(SpinBox spin, double value)
	{
		if (GodotObject.IsInstanceValid(spin))
		{
			spin.Value = value;
		}
	}

	private static VBoxContainer MakeSection(VBoxContainer parent, string title, string subtitle)
	{
		VBoxContainer vBoxContainer = MakeStandaloneSection(title, subtitle);
		parent.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static VBoxContainer MakeStandaloneSection(string title, string subtitle)
	{
		PanelContainer panelContainer = new PanelContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		panelContainer.AddThemeStyleboxOverride("panel", MakePanelStyle(new Color("172735"), new Color("33536a"), 8, 1));
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddThemeConstantOverride("separation", 6);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(MakeLabel(title, 15, new Color("f1d487")), forceReadableName: false, InternalMode.Disabled);
		if (!string.IsNullOrWhiteSpace(subtitle))
		{
			Label label = MakeLabel(subtitle, 11, new Color("8daaba"));
			label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
			vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		}
		VBoxContainer vBoxContainer2 = new VBoxContainer();
		vBoxContainer2.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer2;
	}

	private static HBoxContainer MakeVectorRow(string icon, string title, SpinBox x, SpinBox y, Color accent)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 6);
		hBoxContainer.AddChild(MakeBadge(icon, accent), forceReadableName: false, InternalMode.Disabled);
		Label label = MakeLabel(title, 12, new Color("dce8eb"));
		label.CustomMinimumSize = new Vector2(82f, 0f);
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(MakeAxisCell("X", x, new Color("ff8276")), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(MakeAxisCell("Y", y, new Color("7fda91")), forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static HBoxContainer MakeScalarRow(string icon, string title, SpinBox value, Color accent)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 6);
		hBoxContainer.AddChild(MakeBadge(icon, accent), forceReadableName: false, InternalMode.Disabled);
		Label label = MakeLabel(title, 12, new Color("dce8eb"));
		label.CustomMinimumSize = new Vector2(82f, 0f);
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		value.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(value, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static VBoxContainer MakeAxisCell(string axis, SpinBox spin, Color color)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		Label label = MakeLabel(axis, 10, color);
		label.HorizontalAlignment = HorizontalAlignment.Center;
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		spin.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer.AddChild(spin, forceReadableName: false, InternalMode.Disabled);
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
			CustomMinimumSize = new Vector2(86f, 32f)
		};
	}

	private static Button MakeButton(string text, string tooltip)
	{
		Button button = new Button();
		button.Text = text;
		button.TooltipText = tooltip;
		button.CustomMinimumSize = new Vector2(108f, 36f);
		button.AddThemeStyleboxOverride("normal", MakePanelStyle(new Color("203b4d"), new Color("3e6a80"), 7, 1));
		button.AddThemeStyleboxOverride("hover", MakePanelStyle(new Color("2b5265"), new Color("6bc0d6"), 7, 1));
		button.AddThemeStyleboxOverride("pressed", MakePanelStyle(new Color("142c38"), new Color("f1cb72"), 7, 1));
		return button;
	}

	private static Label MakeBadge(string text, Color color)
	{
		Label label = MakeLabel(text, 20, color);
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.CustomMinimumSize = new Vector2(34f, 34f);
		label.AddThemeStyleboxOverride("normal", MakePanelStyle(new Color("142b38"), color.Darkened(0.38f), 17, 1));
		return label;
	}

	private static Label MakeLabel(string text, int fontSize, Color color)
	{
		Label label = new Label();
		label.Text = text;
		label.AddThemeFontSizeOverride("font_size", fontSize);
		label.AddThemeColorOverride("font_color", color);
		return label;
	}

	private static StyleBoxFlat MakePanelStyle(Color background, Color border, int radius, int width)
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
		return new List<MethodInfo>(31)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildInterface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGridOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGridStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPrimaryGridStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSmartSnapThreshold, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMoveSnapStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRotateSnapStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetScaleSnapStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "notify", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePresetSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifySettingsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampOffset, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampPositiveVector, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampPrimaryStep, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampSmartThreshold, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpinValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSection, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeStandaloneSection, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeVectorRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "x", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "y", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Color, "accent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeScalarRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Color, "accent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeAxisCell, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "axis", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "spin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeBadge, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fontSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakePanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.BindState && args.Count == 1)
		{
			BindState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureState());
			return true;
		}
		if (method == MethodName.ResetSettings && args.Count == 0)
		{
			ResetSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildInterface && args.Count == 0)
		{
			BuildInterface();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPreset && args.Count == 1)
		{
			ApplyPreset(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGridOffset && args.Count == 2)
		{
			SetGridOffset(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGridStep && args.Count == 2)
		{
			SetGridStep(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPrimaryGridStep && args.Count == 2)
		{
			SetPrimaryGridStep(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSmartSnapThreshold && args.Count == 2)
		{
			SetSmartSnapThreshold(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMoveSnapStep && args.Count == 2)
		{
			SetMoveSnapStep(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRotateSnapStep && args.Count == 2)
		{
			SetRotateSnapStep(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetScaleSnapStep && args.Count == 2)
		{
			SetScaleSnapStep(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncControls && args.Count == 0)
		{
			SyncControls();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePresetSelection && args.Count == 0)
		{
			UpdatePresetSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifySettingsChanged && args.Count == 0)
		{
			NotifySettingsChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ClampOffset && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ClampOffset(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.ClampPositiveVector && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ClampPositiveVector(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.ClampPrimaryStep && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ClampPrimaryStep(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.ClampSmartThreshold && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(ClampSmartThreshold(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSpinValue && args.Count == 2)
		{
			SetSpinValue(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeSection && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeSection(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeStandaloneSection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeStandaloneSection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeVectorRow && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(MakeVectorRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<SpinBox>(in args[3]), VariantUtils.ConvertTo<Color>(in args[4])));
			return true;
		}
		if (method == MethodName.MakeScalarRow && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(MakeScalarRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3])));
			return true;
		}
		if (method == MethodName.MakeAxisCell && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeAxisCell(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeSpin && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(MakeSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.MakeButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(MakeButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeBadge && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeLabel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeLabel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.MakePanelStyle && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(MakePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClampOffset && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ClampOffset(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.ClampPositiveVector && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ClampPositiveVector(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.ClampPrimaryStep && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ClampPrimaryStep(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.ClampSmartThreshold && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(ClampSmartThreshold(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSpinValue && args.Count == 2)
		{
			SetSpinValue(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeSection && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeSection(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeStandaloneSection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeStandaloneSection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeVectorRow && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(MakeVectorRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<SpinBox>(in args[3]), VariantUtils.ConvertTo<Color>(in args[4])));
			return true;
		}
		if (method == MethodName.MakeScalarRow && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(MakeScalarRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3])));
			return true;
		}
		if (method == MethodName.MakeAxisCell && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<VBoxContainer>(MakeAxisCell(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.MakeSpin && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(MakeSpin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.MakeButton && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Button>(MakeButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeBadge && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeLabel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Label>(MakeLabel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.MakePanelStyle && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(MakePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
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
		if (method == MethodName.BindState)
		{
			return true;
		}
		if (method == MethodName.CaptureState)
		{
			return true;
		}
		if (method == MethodName.ResetSettings)
		{
			return true;
		}
		if (method == MethodName.BuildInterface)
		{
			return true;
		}
		if (method == MethodName.ApplyPreset)
		{
			return true;
		}
		if (method == MethodName.SetGridOffset)
		{
			return true;
		}
		if (method == MethodName.SetGridStep)
		{
			return true;
		}
		if (method == MethodName.SetPrimaryGridStep)
		{
			return true;
		}
		if (method == MethodName.SetSmartSnapThreshold)
		{
			return true;
		}
		if (method == MethodName.SetMoveSnapStep)
		{
			return true;
		}
		if (method == MethodName.SetRotateSnapStep)
		{
			return true;
		}
		if (method == MethodName.SetScaleSnapStep)
		{
			return true;
		}
		if (method == MethodName.SyncControls)
		{
			return true;
		}
		if (method == MethodName.UpdatePresetSelection)
		{
			return true;
		}
		if (method == MethodName.NotifySettingsChanged)
		{
			return true;
		}
		if (method == MethodName.ClampOffset)
		{
			return true;
		}
		if (method == MethodName.ClampPositiveVector)
		{
			return true;
		}
		if (method == MethodName.ClampPrimaryStep)
		{
			return true;
		}
		if (method == MethodName.ClampSmartThreshold)
		{
			return true;
		}
		if (method == MethodName.SetSpinValue)
		{
			return true;
		}
		if (method == MethodName.MakeSection)
		{
			return true;
		}
		if (method == MethodName.MakeStandaloneSection)
		{
			return true;
		}
		if (method == MethodName.MakeVectorRow)
		{
			return true;
		}
		if (method == MethodName.MakeScalarRow)
		{
			return true;
		}
		if (method == MethodName.MakeAxisCell)
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
		if (method == MethodName.MakeBadge)
		{
			return true;
		}
		if (method == MethodName.MakeLabel)
		{
			return true;
		}
		if (method == MethodName.MakePanelStyle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.GridOffset)
		{
			GridOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.GridStep)
		{
			GridStep = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.PrimaryGridStep)
		{
			PrimaryGridStep = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.SmartSnapThreshold)
		{
			SmartSnapThreshold = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.MoveSnapStep)
		{
			MoveSnapStep = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.RotateSnapStep)
		{
			RotateSnapStep = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.ScaleSnapStep)
		{
			ScaleSnapStep = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._gridOffset)
		{
			_gridOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._gridStep)
		{
			_gridStep = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._primaryGridStep)
		{
			_primaryGridStep = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._smartSnapThreshold)
		{
			_smartSnapThreshold = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._moveSnapStep)
		{
			_moveSnapStep = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._rotateSnapStep)
		{
			_rotateSnapStep = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._scaleSnapStep)
		{
			_scaleSnapStep = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._preview)
		{
			_preview = VariantUtils.ConvertTo<GridPreviewCanvas>(in value);
			return true;
		}
		if (name == PropertyName._offsetX)
		{
			_offsetX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetY)
		{
			_offsetY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._gridX)
		{
			_gridX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._gridY)
		{
			_gridY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._primaryX)
		{
			_primaryX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._primaryY)
		{
			_primaryY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._moveX)
		{
			_moveX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._moveY)
		{
			_moveY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rotateStep)
		{
			_rotateStep = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._scaleStep)
		{
			_scaleStep = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._thresholdSlider)
		{
			_thresholdSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._thresholdSpin)
		{
			_thresholdSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
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
		Vector2 from;
		if (name == PropertyName.GridOffset)
		{
			from = GridOffset;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.GridStep)
		{
			from = GridStep;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PrimaryGridStep)
		{
			value = VariantUtils.CreateFrom<Vector2I>(PrimaryGridStep);
			return true;
		}
		float from2;
		if (name == PropertyName.SmartSnapThreshold)
		{
			from2 = SmartSnapThreshold;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MoveSnapStep)
		{
			from = MoveSnapStep;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RotateSnapStep)
		{
			from2 = RotateSnapStep;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ScaleSnapStep)
		{
			from2 = ScaleSnapStep;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._gridOffset)
		{
			value = VariantUtils.CreateFrom(in _gridOffset);
			return true;
		}
		if (name == PropertyName._gridStep)
		{
			value = VariantUtils.CreateFrom(in _gridStep);
			return true;
		}
		if (name == PropertyName._primaryGridStep)
		{
			value = VariantUtils.CreateFrom(in _primaryGridStep);
			return true;
		}
		if (name == PropertyName._smartSnapThreshold)
		{
			value = VariantUtils.CreateFrom(in _smartSnapThreshold);
			return true;
		}
		if (name == PropertyName._moveSnapStep)
		{
			value = VariantUtils.CreateFrom(in _moveSnapStep);
			return true;
		}
		if (name == PropertyName._rotateSnapStep)
		{
			value = VariantUtils.CreateFrom(in _rotateSnapStep);
			return true;
		}
		if (name == PropertyName._scaleSnapStep)
		{
			value = VariantUtils.CreateFrom(in _scaleSnapStep);
			return true;
		}
		if (name == PropertyName._preview)
		{
			value = VariantUtils.CreateFrom(in _preview);
			return true;
		}
		if (name == PropertyName._offsetX)
		{
			value = VariantUtils.CreateFrom(in _offsetX);
			return true;
		}
		if (name == PropertyName._offsetY)
		{
			value = VariantUtils.CreateFrom(in _offsetY);
			return true;
		}
		if (name == PropertyName._gridX)
		{
			value = VariantUtils.CreateFrom(in _gridX);
			return true;
		}
		if (name == PropertyName._gridY)
		{
			value = VariantUtils.CreateFrom(in _gridY);
			return true;
		}
		if (name == PropertyName._primaryX)
		{
			value = VariantUtils.CreateFrom(in _primaryX);
			return true;
		}
		if (name == PropertyName._primaryY)
		{
			value = VariantUtils.CreateFrom(in _primaryY);
			return true;
		}
		if (name == PropertyName._moveX)
		{
			value = VariantUtils.CreateFrom(in _moveX);
			return true;
		}
		if (name == PropertyName._moveY)
		{
			value = VariantUtils.CreateFrom(in _moveY);
			return true;
		}
		if (name == PropertyName._rotateStep)
		{
			value = VariantUtils.CreateFrom(in _rotateStep);
			return true;
		}
		if (name == PropertyName._scaleStep)
		{
			value = VariantUtils.CreateFrom(in _scaleStep);
			return true;
		}
		if (name == PropertyName._thresholdSlider)
		{
			value = VariantUtils.CreateFrom(in _thresholdSlider);
			return true;
		}
		if (name == PropertyName._thresholdSpin)
		{
			value = VariantUtils.CreateFrom(in _thresholdSpin);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
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
			new PropertyInfo(Variant.Type.Vector2, PropertyName._gridOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._gridStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._primaryGridStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._smartSnapThreshold, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._moveSnapStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._rotateSnapStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._scaleSnapStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._preview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._primaryX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._primaryY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rotateStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._thresholdSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._thresholdSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.GridOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.GridStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.PrimaryGridStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.SmartSnapThreshold, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.MoveSnapStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.RotateSnapStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.ScaleSnapStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GridOffset, Variant.From<Vector2>(GridOffset));
		info.AddProperty(PropertyName.GridStep, Variant.From<Vector2>(GridStep));
		info.AddProperty(PropertyName.PrimaryGridStep, Variant.From<Vector2I>(PrimaryGridStep));
		info.AddProperty(PropertyName.SmartSnapThreshold, Variant.From<float>(SmartSnapThreshold));
		info.AddProperty(PropertyName.MoveSnapStep, Variant.From<Vector2>(MoveSnapStep));
		info.AddProperty(PropertyName.RotateSnapStep, Variant.From<float>(RotateSnapStep));
		info.AddProperty(PropertyName.ScaleSnapStep, Variant.From<float>(ScaleSnapStep));
		info.AddProperty(PropertyName._gridOffset, Variant.From(in _gridOffset));
		info.AddProperty(PropertyName._gridStep, Variant.From(in _gridStep));
		info.AddProperty(PropertyName._primaryGridStep, Variant.From(in _primaryGridStep));
		info.AddProperty(PropertyName._smartSnapThreshold, Variant.From(in _smartSnapThreshold));
		info.AddProperty(PropertyName._moveSnapStep, Variant.From(in _moveSnapStep));
		info.AddProperty(PropertyName._rotateSnapStep, Variant.From(in _rotateSnapStep));
		info.AddProperty(PropertyName._scaleSnapStep, Variant.From(in _scaleSnapStep));
		info.AddProperty(PropertyName._preview, Variant.From(in _preview));
		info.AddProperty(PropertyName._offsetX, Variant.From(in _offsetX));
		info.AddProperty(PropertyName._offsetY, Variant.From(in _offsetY));
		info.AddProperty(PropertyName._gridX, Variant.From(in _gridX));
		info.AddProperty(PropertyName._gridY, Variant.From(in _gridY));
		info.AddProperty(PropertyName._primaryX, Variant.From(in _primaryX));
		info.AddProperty(PropertyName._primaryY, Variant.From(in _primaryY));
		info.AddProperty(PropertyName._moveX, Variant.From(in _moveX));
		info.AddProperty(PropertyName._moveY, Variant.From(in _moveY));
		info.AddProperty(PropertyName._rotateStep, Variant.From(in _rotateStep));
		info.AddProperty(PropertyName._scaleStep, Variant.From(in _scaleStep));
		info.AddProperty(PropertyName._thresholdSlider, Variant.From(in _thresholdSlider));
		info.AddProperty(PropertyName._thresholdSpin, Variant.From(in _thresholdSpin));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GridOffset, out var value))
		{
			GridOffset = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.GridStep, out var value2))
		{
			GridStep = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.PrimaryGridStep, out var value3))
		{
			PrimaryGridStep = value3.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.SmartSnapThreshold, out var value4))
		{
			SmartSnapThreshold = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.MoveSnapStep, out var value5))
		{
			MoveSnapStep = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.RotateSnapStep, out var value6))
		{
			RotateSnapStep = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.ScaleSnapStep, out var value7))
		{
			ScaleSnapStep = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName._gridOffset, out var value8))
		{
			_gridOffset = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._gridStep, out var value9))
		{
			_gridStep = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._primaryGridStep, out var value10))
		{
			_primaryGridStep = value10.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._smartSnapThreshold, out var value11))
		{
			_smartSnapThreshold = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName._moveSnapStep, out var value12))
		{
			_moveSnapStep = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._rotateSnapStep, out var value13))
		{
			_rotateSnapStep = value13.As<float>();
		}
		if (info.TryGetProperty(PropertyName._scaleSnapStep, out var value14))
		{
			_scaleSnapStep = value14.As<float>();
		}
		if (info.TryGetProperty(PropertyName._preview, out var value15))
		{
			_preview = value15.As<GridPreviewCanvas>();
		}
		if (info.TryGetProperty(PropertyName._offsetX, out var value16))
		{
			_offsetX = value16.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetY, out var value17))
		{
			_offsetY = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._gridX, out var value18))
		{
			_gridX = value18.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._gridY, out var value19))
		{
			_gridY = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._primaryX, out var value20))
		{
			_primaryX = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._primaryY, out var value21))
		{
			_primaryY = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._moveX, out var value22))
		{
			_moveX = value22.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._moveY, out var value23))
		{
			_moveY = value23.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rotateStep, out var value24))
		{
			_rotateStep = value24.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleStep, out var value25))
		{
			_scaleStep = value25.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._thresholdSlider, out var value26))
		{
			_thresholdSlider = value26.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._thresholdSpin, out var value27))
		{
			_thresholdSpin = value27.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value28))
		{
			_summaryLabel = value28.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value29))
		{
			_updatingControls = value29.As<bool>();
		}
	}
}
