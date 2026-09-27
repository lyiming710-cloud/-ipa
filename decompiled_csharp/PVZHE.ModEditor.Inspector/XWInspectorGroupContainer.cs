using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Group/XWInspectorGroupContainer.cs")]
public class XWInspectorGroupContainer : VBoxContainer
{
	public new class MethodName : VBoxContainer.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName LoadIcons = "LoadIcons";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupFoldButton = "SetupFoldButton";

		public static readonly StringName Init = "Init";

		public static readonly StringName SetGroupName = "SetGroupName";

		public static readonly StringName AddEditor = "AddEditor";

		public static readonly StringName OnFoldButtonPressed = "OnFoldButtonPressed";

		public static readonly StringName SetFolded = "SetFolded";

		public static readonly StringName IsFolded = "IsFolded";

		public static readonly StringName UpdateChangeCount = "UpdateChangeCount";

		public static readonly StringName CountRevertableInSubgroup = "CountRevertableInSubgroup";

		public static readonly StringName OnHeaderGuiInput = "OnHeaderGuiInput";

		public static readonly StringName OnHeaderMouseEntered = "OnHeaderMouseEntered";

		public static readonly StringName OnHeaderMouseExited = "OnHeaderMouseExited";

		public static readonly StringName ApplyHeaderPanelStyle = "ApplyHeaderPanelStyle";

		public static readonly StringName CreateHeaderPanelStyle = "CreateHeaderPanelStyle";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName _groupNameLabel = "_groupNameLabel";

		public static readonly StringName EditorContainer = "EditorContainer";

		public static readonly StringName Group = "Group";

		public static readonly StringName _isFolded = "_isFolded";

		public static readonly StringName _headerButton = "_headerButton";

		public static readonly StringName _changeCountLabel = "_changeCountLabel";

		public static readonly StringName _headerHover = "_headerHover";

		public static readonly StringName _panelContainer = "_panelContainer";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	private static readonly string ScenePath = "res://addons/ModEditor/Inspector/GUI/Group/XWInspectorGroupContainer.tscn";

	private const int HeaderMinHeight = 18;

	private const int HeaderHorizontalMargin = 4;

	private static Texture2D _iconArrowClose;

	private static Texture2D _iconArrowOpen;

	private Label _groupNameLabel;

	public VBoxContainer EditorContainer;

	public XWInspectorGroup Group;

	private bool _isFolded;

	private Button _headerButton;

	private Label _changeCountLabel;

	private bool _headerHover;

	private PanelContainer _panelContainer;

	public static XWInspectorGroupContainer Create()
	{
		return ResourceLoader.Load<PackedScene>(ScenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorGroupContainer>(PackedScene.GenEditState.Disabled);
	}

	private static void LoadIcons()
	{
		_iconArrowClose = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GuiTreeArrowRight.svg", null, ResourceLoader.CacheMode.Reuse));
		_iconArrowOpen = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GuiTreeArrowDown.svg", null, ResourceLoader.CacheMode.Reuse));
	}

	public override void _Ready()
	{
		if (_iconArrowOpen == null)
		{
			LoadIcons();
		}
		_groupNameLabel = GetNode<Label>("%GroupNameLabel");
		EditorContainer = GetNode<VBoxContainer>("%EditorContainer");
		SetupFoldButton();
		_panelContainer = GetNodeOrNull<PanelContainer>("PanelContainer");
		if (GodotObject.IsInstanceValid(_panelContainer))
		{
			_panelContainer.CustomMinimumSize = new Vector2(0f, 18f);
			ApplyHeaderPanelStyle(hover: false);
			_panelContainer.GuiInput += OnHeaderGuiInput;
			_panelContainer.MouseEntered += OnHeaderMouseEntered;
			_panelContainer.MouseExited += OnHeaderMouseExited;
		}
	}

	private void SetupFoldButton()
	{
		PanelContainer nodeOrNull = GetNodeOrNull<PanelContainer>("PanelContainer");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			_headerButton = new Button
			{
				Name = "FoldButton",
				Flat = true,
				Icon = _iconArrowOpen,
				CustomMinimumSize = new Vector2(14f, 14f)
			};
			_headerButton.Pressed += OnFoldButtonPressed;
			_changeCountLabel = new Label
			{
				Name = "ChangeCount"
			};
			_changeCountLabel.AddThemeColorOverride("font_color", new Color(0.6f, 0.8f, 1f, 0.8f));
			_changeCountLabel.AddThemeFontSizeOverride("font_size", 11);
			_changeCountLabel.Visible = false;
			HBoxContainer nodeOrNull2 = nodeOrNull.GetNodeOrNull<HBoxContainer>("HBoxContainer");
			if (GodotObject.IsInstanceValid(nodeOrNull2))
			{
				nodeOrNull2.AddChild(_headerButton, forceReadableName: false, InternalMode.Disabled);
				nodeOrNull2.MoveChild(_headerButton, 0);
				nodeOrNull2.AddChild(_changeCountLabel, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	public void Init(XWInspectorGroup group)
	{
		Group = group;
		SetGroupName(group.GroupName.ToString());
		group.GroupContainer = this;
	}

	public void SetGroupName(string groupName)
	{
		if (GodotObject.IsInstanceValid(_groupNameLabel))
		{
			_groupNameLabel.Text = groupName;
		}
	}

	public void AddEditor(XWInspectorPropertyEditorBase editor)
	{
		EditorContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
	}

	private void OnFoldButtonPressed()
	{
		_isFolded = !_isFolded;
		EditorContainer.Visible = !_isFolded;
		if (GodotObject.IsInstanceValid(_headerButton))
		{
			_headerButton.Icon = ((!_isFolded) ? _iconArrowOpen : _iconArrowClose);
		}
		UpdateChangeCount();
	}

	public void SetFolded(bool folded)
	{
		_isFolded = folded;
		if (GodotObject.IsInstanceValid(EditorContainer))
		{
			EditorContainer.Visible = !folded;
		}
		if (GodotObject.IsInstanceValid(_headerButton))
		{
			_headerButton.Icon = ((!folded) ? _iconArrowOpen : _iconArrowClose);
		}
		UpdateChangeCount();
	}

	public bool IsFolded()
	{
		return _isFolded;
	}

	private void UpdateChangeCount()
	{
		if (!GodotObject.IsInstanceValid(_changeCountLabel))
		{
			return;
		}
		if (!_isFolded)
		{
			_changeCountLabel.Visible = false;
			return;
		}
		int num = 0;
		foreach (Node child in EditorContainer.GetChildren())
		{
			if (child is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase)
			{
				if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase.ResetButton) && xWInspectorPropertyEditorBase.ResetButton.Visible)
				{
					num++;
				}
			}
			else if (child is XWInspectorSubgroupContainer subgroup)
			{
				num += CountRevertableInSubgroup(subgroup);
			}
		}
		if (num > 0)
		{
			_changeCountLabel.Text = $"({num} 项变更)";
			_changeCountLabel.Visible = true;
		}
		else
		{
			_changeCountLabel.Visible = false;
		}
	}

	private static int CountRevertableInSubgroup(XWInspectorSubgroupContainer subgroup)
	{
		int num = 0;
		if (!GodotObject.IsInstanceValid(subgroup))
		{
			return 0;
		}
		VBoxContainer nodeOrNull = subgroup.GetNodeOrNull<VBoxContainer>("%EditorContainer");
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			return 0;
		}
		foreach (Node child in nodeOrNull.GetChildren())
		{
			if (child is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase && GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase.ResetButton) && xWInspectorPropertyEditorBase.ResetButton.Visible)
			{
				num++;
			}
		}
		return num;
	}

	private void OnHeaderGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed)
		{
			OnFoldButtonPressed();
		}
	}

	private void OnHeaderMouseEntered()
	{
		_headerHover = true;
		ApplyHeaderPanelStyle(hover: true);
	}

	private void OnHeaderMouseExited()
	{
		_headerHover = false;
		if (GodotObject.IsInstanceValid(_panelContainer))
		{
			ApplyHeaderPanelStyle(hover: false);
		}
	}

	private void ApplyHeaderPanelStyle(bool hover)
	{
		if (GodotObject.IsInstanceValid(_panelContainer))
		{
			_panelContainer.AddThemeStyleboxOverride("panel", CreateHeaderPanelStyle(hover));
		}
	}

	private static StyleBoxFlat CreateHeaderPanelStyle(bool hover)
	{
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat();
		styleBoxFlat.BgColor = (hover ? new Color(0.392f, 0.392f, 0.392f, 0.6f) : new Color(0.363f, 0.363f, 0.363f, 0.6f));
		styleBoxFlat.CornerRadiusTopLeft = 3;
		styleBoxFlat.CornerRadiusTopRight = 3;
		styleBoxFlat.CornerRadiusBottomRight = 3;
		styleBoxFlat.CornerRadiusBottomLeft = 3;
		styleBoxFlat.CornerDetail = 5;
		styleBoxFlat.SetContentMargin(Side.Left, 4f);
		styleBoxFlat.SetContentMargin(Side.Top, 0f);
		styleBoxFlat.SetContentMargin(Side.Right, 4f);
		styleBoxFlat.SetContentMargin(Side.Bottom, 0f);
		return styleBoxFlat;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.LoadIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupFoldButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "group", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetGroupName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "groupName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnFoldButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFolded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "folded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFolded, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateChangeCount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountRevertableInSubgroup, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "subgroup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Container"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHeaderGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHeaderMouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHeaderMouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyHeaderPanelStyle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hover", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateHeaderPanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hover", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorGroupContainer>(Create());
			return true;
		}
		if (method == MethodName.LoadIcons && args.Count == 0)
		{
			LoadIcons();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupFoldButton && args.Count == 0)
		{
			SetupFoldButton();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<XWInspectorGroup>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGroupName && args.Count == 1)
		{
			SetGroupName(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddEditor && args.Count == 1)
		{
			AddEditor(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFoldButtonPressed && args.Count == 0)
		{
			OnFoldButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SetFolded && args.Count == 1)
		{
			SetFolded(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsFolded && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFolded());
			return true;
		}
		if (method == MethodName.UpdateChangeCount && args.Count == 0)
		{
			UpdateChangeCount();
			ret = default;
			return true;
		}
		if (method == MethodName.CountRevertableInSubgroup && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountRevertableInSubgroup(VariantUtils.ConvertTo<XWInspectorSubgroupContainer>(in args[0])));
			return true;
		}
		if (method == MethodName.OnHeaderGuiInput && args.Count == 1)
		{
			OnHeaderGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHeaderMouseEntered && args.Count == 0)
		{
			OnHeaderMouseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHeaderMouseExited && args.Count == 0)
		{
			OnHeaderMouseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyHeaderPanelStyle && args.Count == 1)
		{
			ApplyHeaderPanelStyle(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateHeaderPanelStyle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreateHeaderPanelStyle(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorGroupContainer>(Create());
			return true;
		}
		if (method == MethodName.LoadIcons && args.Count == 0)
		{
			LoadIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.CountRevertableInSubgroup && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountRevertableInSubgroup(VariantUtils.ConvertTo<XWInspectorSubgroupContainer>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateHeaderPanelStyle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreateHeaderPanelStyle(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.LoadIcons)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetupFoldButton)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.SetGroupName)
		{
			return true;
		}
		if (method == MethodName.AddEditor)
		{
			return true;
		}
		if (method == MethodName.OnFoldButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SetFolded)
		{
			return true;
		}
		if (method == MethodName.IsFolded)
		{
			return true;
		}
		if (method == MethodName.UpdateChangeCount)
		{
			return true;
		}
		if (method == MethodName.CountRevertableInSubgroup)
		{
			return true;
		}
		if (method == MethodName.OnHeaderGuiInput)
		{
			return true;
		}
		if (method == MethodName.OnHeaderMouseEntered)
		{
			return true;
		}
		if (method == MethodName.OnHeaderMouseExited)
		{
			return true;
		}
		if (method == MethodName.ApplyHeaderPanelStyle)
		{
			return true;
		}
		if (method == MethodName.CreateHeaderPanelStyle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._groupNameLabel)
		{
			_groupNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.EditorContainer)
		{
			EditorContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.Group)
		{
			Group = VariantUtils.ConvertTo<XWInspectorGroup>(in value);
			return true;
		}
		if (name == PropertyName._isFolded)
		{
			_isFolded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._headerButton)
		{
			_headerButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._changeCountLabel)
		{
			_changeCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._headerHover)
		{
			_headerHover = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._panelContainer)
		{
			_panelContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._groupNameLabel)
		{
			value = VariantUtils.CreateFrom(in _groupNameLabel);
			return true;
		}
		if (name == PropertyName.EditorContainer)
		{
			value = VariantUtils.CreateFrom(in EditorContainer);
			return true;
		}
		if (name == PropertyName.Group)
		{
			value = VariantUtils.CreateFrom(in Group);
			return true;
		}
		if (name == PropertyName._isFolded)
		{
			value = VariantUtils.CreateFrom(in _isFolded);
			return true;
		}
		if (name == PropertyName._headerButton)
		{
			value = VariantUtils.CreateFrom(in _headerButton);
			return true;
		}
		if (name == PropertyName._changeCountLabel)
		{
			value = VariantUtils.CreateFrom(in _changeCountLabel);
			return true;
		}
		if (name == PropertyName._headerHover)
		{
			value = VariantUtils.CreateFrom(in _headerHover);
			return true;
		}
		if (name == PropertyName._panelContainer)
		{
			value = VariantUtils.CreateFrom(in _panelContainer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._groupNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.EditorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Group, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isFolded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headerButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._changeCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._headerHover, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._panelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._groupNameLabel, Variant.From(in _groupNameLabel));
		info.AddProperty(PropertyName.EditorContainer, Variant.From(in EditorContainer));
		info.AddProperty(PropertyName.Group, Variant.From(in Group));
		info.AddProperty(PropertyName._isFolded, Variant.From(in _isFolded));
		info.AddProperty(PropertyName._headerButton, Variant.From(in _headerButton));
		info.AddProperty(PropertyName._changeCountLabel, Variant.From(in _changeCountLabel));
		info.AddProperty(PropertyName._headerHover, Variant.From(in _headerHover));
		info.AddProperty(PropertyName._panelContainer, Variant.From(in _panelContainer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._groupNameLabel, out var value))
		{
			_groupNameLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.EditorContainer, out var value2))
		{
			EditorContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.Group, out var value3))
		{
			Group = value3.As<XWInspectorGroup>();
		}
		if (info.TryGetProperty(PropertyName._isFolded, out var value4))
		{
			_isFolded = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._headerButton, out var value5))
		{
			_headerButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._changeCountLabel, out var value6))
		{
			_changeCountLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._headerHover, out var value7))
		{
			_headerHover = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._panelContainer, out var value8))
		{
			_panelContainer = value8.As<PanelContainer>();
		}
	}
}
