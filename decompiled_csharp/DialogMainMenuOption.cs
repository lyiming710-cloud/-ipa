using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.Tools.GUI;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/MenuDialog/DialogMainMenuOption/DialogMainMenuOption.cs")]
public class DialogMainMenuOption : MenuDialogBase
{
	public new class MethodName : MenuDialogBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OpenManagement = "OpenManagement";

		public static readonly StringName ReturnFromManagement = "ReturnFromManagement";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : MenuDialogBase.PropertyName
	{
		public static readonly StringName AllowModNavigation = "AllowModNavigation";

		public static readonly StringName _options = "_options";

		public static readonly StringName _managementButton = "_managementButton";

		public static readonly StringName _backButton = "_backButton";

		public static readonly StringName _backShortcut = "_backShortcut";

		public static readonly StringName _managementOverlay = "_managementOverlay";

		public static readonly StringName _managementPanel = "_managementPanel";
	}

	public new class SignalName : MenuDialogBase.SignalName
	{
	}

	private Control _options;

	private MainButton _managementButton;

	private BaseButton _backButton;

	private Shortcut _backShortcut;

	private Control _managementOverlay;

	private XWModToolsPanel _managementPanel;

	public bool AllowModNavigation { get; set; } = true;

	public override void _Ready()
	{
		base._Ready();
		_options = GetNode<Control>("Layer/Control/DialogMenu");
		_backButton = GetNode<BaseButton>("%BackButton");
		_managementButton = _options.GetNode<MainButton>("ModManagementButton");
		_managementButton.Pressed += OpenManagement;
	}

	public void OpenManagement()
	{
		if (GodotObject.IsInstanceValid(_managementOverlay))
		{
			return;
		}
		dragStart = false;
		isInDrag = false;
		_backShortcut = _backButton.Shortcut;
		_backButton.Shortcut = null;
		_backButton.Disabled = true;
		_options.Hide();
		_managementOverlay = new Control
		{
			Name = "ModManagementOverlay",
			MouseFilter = MouseFilterEnum.Stop,
			Theme = XWModToolsPanel.CreatePlayerManagementTheme()
		};
		GetNode<Control>("Layer/Control").AddChild(_managementOverlay, forceReadableName: false, InternalMode.Disabled);
		_managementOverlay.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		TextureRect textureRect = new TextureRect
		{
			Name = "HandbookBackground",
			MouseFilter = MouseFilterEnum.Ignore,
			Texture = GD.Load<Texture2D>("res://Asset/Texture/GUI/Almanac/AlmanacPlantBackground.jpg"),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.Scale
		};
		_managementOverlay.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
		textureRect.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		Label label = new Label
		{
			Name = "ManagementTitle",
			Text = "Mod 管理",
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			MouseFilter = MouseFilterEnum.Ignore
		};
		label.AddThemeFontOverride("font", GD.Load<Font>("res://Asset/Font/fzjz.ttf"));
		label.AddThemeFontSizeOverride("font_size", 32);
		label.AddThemeColorOverride("font_color", new Color("f8df9a"));
		label.AddThemeColorOverride("font_outline_color", new Color("482508"));
		label.AddThemeConstantOverride("outline_size", 2);
		_managementOverlay.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		label.SetAnchorsAndOffsetsPreset(LayoutPreset.TopWide, LayoutPresetMode.Minsize);
		label.OffsetTop = 16f;
		label.OffsetBottom = 66f;
		MarginContainer marginContainer = new MarginContainer
		{
			Name = "HandbookContent"
		};
		marginContainer.AddThemeConstantOverride("margin_left", 36);
		marginContainer.AddThemeConstantOverride("margin_right", 36);
		marginContainer.AddThemeConstantOverride("margin_top", 90);
		marginContainer.AddThemeConstantOverride("margin_bottom", 36);
		_managementOverlay.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		marginContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		_managementPanel = GD.Load<PackedScene>("res://addons/ModEditor/Tools/GUI/XWModToolsPanel.tscn").Instantiate<XWModToolsPanel>(PackedScene.GenEditState.Disabled);
		_managementPanel.Name = "PlayerModManager";
		_managementPanel.ManagementOnly = true;
		_managementPanel.ManagementConfirmationHost = _managementOverlay;
		_managementPanel.ManagementReturnRequested = ReturnFromManagement;
		if (AllowModNavigation)
		{
			_managementPanel.ManagementPlayRequested = () =>
			{
				_managementPanel.CloseManagementSession();
				CloseDialog();
				XWModLevelSession.Clear();
				Global.Instance.isEditor = false;
				Global.Instance.currentLevelChoose = "__ModLevels";
				Global.Instance.currentChapterId = -1;
				Global.Instance.currentLevelId = -1;
				SceneManager.Instance.ChangeScene("LevelChoose");
			};
			_managementPanel.ManagementCreateRequested = () =>
			{
				ReturnFromManagement();
				CloseDialog();
				ModEditorManager.Instance?.OpenEditor();
			};
		}
		marginContainer.AddChild(_managementPanel, forceReadableName: false, InternalMode.Disabled);
	}

	private void ReturnFromManagement()
	{
		if (GodotObject.IsInstanceValid(_managementPanel) && !_managementPanel.TryCancelManagementConfirmation() && !_managementPanel.TryClosePlayerUtility())
		{
			_managementPanel.CloseManagementSession();
			_managementOverlay.QueueFree();
			_managementPanel = null;
			_managementOverlay = null;
			_backButton.Shortcut = _backShortcut;
			_backButton.Disabled = false;
			_options.Show();
			_managementButton.GrabFocus();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (GodotObject.IsInstanceValid(_managementOverlay) && @event.IsPressed() && !@event.IsEcho())
		{
			Shortcut backShortcut = _backShortcut;
			if (backShortcut != null && backShortcut.MatchesEvent(@event))
			{
				GetViewport().SetInputAsHandled();
				ReturnFromManagement();
			}
		}
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_managementPanel))
		{
			_managementPanel.CloseManagementSession();
		}
		base._ExitTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenManagement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReturnFromManagement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OpenManagement && args.Count == 0)
		{
			OpenManagement();
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnFromManagement && args.Count == 0)
		{
			ReturnFromManagement();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
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
		if (method == MethodName.OpenManagement)
		{
			return true;
		}
		if (method == MethodName.ReturnFromManagement)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.AllowModNavigation)
		{
			AllowModNavigation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._options)
		{
			_options = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._managementButton)
		{
			_managementButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._backButton)
		{
			_backButton = VariantUtils.ConvertTo<BaseButton>(in value);
			return true;
		}
		if (name == PropertyName._backShortcut)
		{
			_backShortcut = VariantUtils.ConvertTo<Shortcut>(in value);
			return true;
		}
		if (name == PropertyName._managementOverlay)
		{
			_managementOverlay = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._managementPanel)
		{
			_managementPanel = VariantUtils.ConvertTo<XWModToolsPanel>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.AllowModNavigation)
		{
			value = VariantUtils.CreateFrom<bool>(AllowModNavigation);
			return true;
		}
		if (name == PropertyName._options)
		{
			value = VariantUtils.CreateFrom(in _options);
			return true;
		}
		if (name == PropertyName._managementButton)
		{
			value = VariantUtils.CreateFrom(in _managementButton);
			return true;
		}
		if (name == PropertyName._backButton)
		{
			value = VariantUtils.CreateFrom(in _backButton);
			return true;
		}
		if (name == PropertyName._backShortcut)
		{
			value = VariantUtils.CreateFrom(in _backShortcut);
			return true;
		}
		if (name == PropertyName._managementOverlay)
		{
			value = VariantUtils.CreateFrom(in _managementOverlay);
			return true;
		}
		if (name == PropertyName._managementPanel)
		{
			value = VariantUtils.CreateFrom(in _managementPanel);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._options, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._managementButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._backButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._backShortcut, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._managementOverlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._managementPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AllowModNavigation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.AllowModNavigation, Variant.From<bool>(AllowModNavigation));
		info.AddProperty(PropertyName._options, Variant.From(in _options));
		info.AddProperty(PropertyName._managementButton, Variant.From(in _managementButton));
		info.AddProperty(PropertyName._backButton, Variant.From(in _backButton));
		info.AddProperty(PropertyName._backShortcut, Variant.From(in _backShortcut));
		info.AddProperty(PropertyName._managementOverlay, Variant.From(in _managementOverlay));
		info.AddProperty(PropertyName._managementPanel, Variant.From(in _managementPanel));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.AllowModNavigation, out var value))
		{
			AllowModNavigation = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._options, out var value2))
		{
			_options = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._managementButton, out var value3))
		{
			_managementButton = value3.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._backButton, out var value4))
		{
			_backButton = value4.As<BaseButton>();
		}
		if (info.TryGetProperty(PropertyName._backShortcut, out var value5))
		{
			_backShortcut = value5.As<Shortcut>();
		}
		if (info.TryGetProperty(PropertyName._managementOverlay, out var value6))
		{
			_managementOverlay = value6.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._managementPanel, out var value7))
		{
			_managementPanel = value7.As<XWModToolsPanel>();
		}
	}
}
