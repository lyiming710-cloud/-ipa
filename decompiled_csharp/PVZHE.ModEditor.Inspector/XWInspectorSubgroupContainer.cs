using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Subgroup/XWInspectorSubgroupContainer.cs")]
public class XWInspectorSubgroupContainer : Container
{
	public new class MethodName : Container.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName LoadIcons = "LoadIcons";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName SetIndentDepth = "SetIndentDepth";

		public static readonly StringName SetLevel = "SetLevel";

		public static readonly StringName AddEditor = "AddEditor";

		public static readonly StringName SetFolded = "SetFolded";

		public static readonly StringName IsFolded = "IsFolded";

		public new static readonly StringName _GuiInput = "_GuiInput";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName ResortChildren = "ResortChildren";

		public static readonly StringName GetHeaderHeight = "GetHeaderHeight";

		public new static readonly StringName _Draw = "_Draw";

		public new static readonly StringName _GetMinimumSize = "_GetMinimumSize";
	}

	public new class PropertyName : Container.PropertyName
	{
		public static readonly StringName _vbox = "_vbox";

		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName Subgroup = "Subgroup";

		public static readonly StringName _isFolded = "_isFolded";

		public static readonly StringName _headerHover = "_headerHover";

		public static readonly StringName _indentDepth = "_indentDepth";

		public static readonly StringName _bgColor = "_bgColor";

		public static readonly StringName _level = "_level";
	}

	public new class SignalName : Container.SignalName
	{
	}

	private static readonly string ScenePath = "res://addons/ModEditor/Inspector/GUI/Subgroup/XWInspectorSubgroupContainer.tscn";

	private const int HeaderMinHeight = 24;

	private const int HeaderVerticalPadding = 6;

	private static Texture2D _iconArrowClose;

	private static Texture2D _iconArrowOpen;

	private VBoxContainer _vbox;

	private VBoxContainer _editorContainer;

	public XWInspectorSubgroup Subgroup;

	private bool _isFolded;

	private bool _headerHover;

	private int _indentDepth = 1;

	private Color _bgColor = new Color(1f, 1f, 1f, 0.06f);

	private int _level = 1;

	public static XWInspectorSubgroupContainer Create()
	{
		return ResourceLoader.Load<PackedScene>(ScenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorSubgroupContainer>(PackedScene.GenEditState.Disabled);
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
		_vbox = GetNode<VBoxContainer>("%VBox");
		_editorContainer = GetNode<VBoxContainer>("%EditorContainer");
	}

	public void Init(XWInspectorSubgroup subgroup)
	{
		Subgroup = subgroup;
		subgroup.SubgroupContainer = this;
	}

	public void SetIndentDepth(int depth)
	{
		_indentDepth = Mathf.Max(depth, 1);
		QueueRedraw();
	}

	public void SetLevel(int level)
	{
		_level = level;
		_bgColor = new Color(1f, 1f, 1f, 0.06f);
		_bgColor.A /= _level;
		QueueRedraw();
	}

	public void AddEditor(XWInspectorPropertyEditorBase editor)
	{
		_editorContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
	}

	public void SetFolded(bool folded)
	{
		_isFolded = folded;
		if (GodotObject.IsInstanceValid(_editorContainer))
		{
			MarginContainer parentOrNull = _editorContainer.GetParentOrNull<MarginContainer>();
			if (GodotObject.IsInstanceValid(parentOrNull))
			{
				parentOrNull.Visible = !folded;
			}
		}
		QueueRedraw();
	}

	public bool IsFolded()
	{
		return _isFolded;
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed)
		{
			int headerHeight = GetHeaderHeight();
			if (inputEventMouseButton.Position.Y < (float)headerHeight)
			{
				_isFolded = !_isFolded;
				SetFolded(_isFolded);
				_headerHover = false;
				QueueRedraw();
			}
		}
	}

	public override void _Notification(int what)
	{
		if ((long)what == 41)
		{
			_headerHover = true;
			QueueRedraw();
		}
		else if ((long)what == 42)
		{
			_headerHover = false;
			QueueRedraw();
		}
		else if ((long)what == 51)
		{
			ResortChildren();
		}
		else if ((long)what == 45)
		{
			QueueRedraw();
		}
	}

	private void ResortChildren()
	{
		if (GodotObject.IsInstanceValid(_vbox))
		{
			int headerHeight = GetHeaderHeight();
			int num = _indentDepth * 12;
			Vector2 size = Size;
			FitChildInRect(_vbox, new Rect2(new Vector2(num, headerHeight), new Vector2(size.X - (float)num, size.Y - (float)headerHeight)));
		}
	}

	private int GetHeaderHeight()
	{
		Font themeFont = GetThemeFont("font", "");
		int themeFontSize = GetThemeFontSize("font_size", "");
		int b = 24;
		if (GodotObject.IsInstanceValid(themeFont))
		{
			b = Mathf.CeilToInt(themeFont.GetHeight(themeFontSize)) + 6;
		}
		return Mathf.Max(24, b);
	}

	public override void _Draw()
	{
		int headerHeight = GetHeaderHeight();
		Vector2 size = Size;
		int num = _indentDepth * 12;
		Color color = _bgColor;
		color.A *= 0.4f;
		if (_headerHover)
		{
			color = color.Lightened(0.15f);
		}
		Rect2 rect = new Rect2(new Vector2(num, 0f), new Vector2(size.X - (float)num, headerHeight));
		DrawRect(rect, color);
		DrawRect(color: new Color(0.3f, 0.3f, 0.3f, 0.4f), rect: new Rect2(Vector2.Zero, new Vector2(num, size.Y)));
		Texture2D texture2D = ((!_isFolded) ? _iconArrowOpen : _iconArrowClose);
		if (GodotObject.IsInstanceValid(texture2D))
		{
			float x = num + 4;
			float y = (float)(headerHeight - texture2D.GetHeight()) / 2f;
			Color value = new Color(1f, 1f, 1f, _headerHover ? 1f : 0.85f);
			DrawTexture(texture2D, new Vector2(x, y), value);
		}
		Font themeFont = GetThemeFont("font", "");
		int themeFontSize = GetThemeFontSize("font_size", "");
		if (GodotObject.IsInstanceValid(themeFont) && GodotObject.IsInstanceValid(Subgroup))
		{
			float x2 = num + 4 + 20;
			float num2 = ((float)headerHeight - themeFont.GetHeight(themeFontSize)) / 2f;
			DrawString(modulate: new Color(0.85f, 0.85f, 0.85f), font: themeFont, pos: new Vector2(x2, num2 + themeFont.GetAscent(themeFontSize)), text: Subgroup.SubgroupName.ToString(), alignment: HorizontalAlignment.Left, width: -1f, fontSize: themeFontSize, justificationFlags: TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, direction: TextServer.Direction.Auto, orientation: TextServer.Orientation.Horizontal, oversampling: 0f);
		}
		if (!_isFolded || !GodotObject.IsInstanceValid(_editorContainer) || !GodotObject.IsInstanceValid(themeFont))
		{
			return;
		}
		int num3 = 0;
		foreach (Node child in _editorContainer.GetChildren())
		{
			if (child is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase && GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase.ResetButton) && xWInspectorPropertyEditorBase.ResetButton.Visible)
			{
				num3++;
			}
		}
		if (num3 > 0)
		{
			string text = $"({num3})";
			int fontSize = themeFontSize - 2;
			float x3 = themeFont.GetStringSize(text, HorizontalAlignment.Left, -1f, fontSize, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal).X;
			float x4 = size.X - x3 - 8f;
			float num4 = ((float)headerHeight - themeFont.GetHeight(fontSize)) / 2f;
			DrawString(themeFont, new Vector2(x4, num4 + themeFont.GetAscent(fontSize)), text, HorizontalAlignment.Left, -1f, fontSize, new Color(0.6f, 0.6f, 0.6f, 0.8f), TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
		}
	}

	public override Vector2 _GetMinimumSize()
	{
		Vector2 zero = Vector2.Zero;
		int headerHeight = GetHeaderHeight();
		zero.Y = headerHeight;
		if (!_isFolded && GodotObject.IsInstanceValid(_vbox))
		{
			Vector2 combinedMinimumSize = _vbox.GetCombinedMinimumSize();
			zero.Y += combinedMinimumSize.Y;
			zero.X = Mathf.Max(zero.X, combinedMinimumSize.X + (float)(_indentDepth * 12));
		}
		return zero;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Container"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.LoadIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "subgroup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetIndentDepth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "depth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "level", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetFolded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "folded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFolded, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResortChildren, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHeaderHeight, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetMinimumSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorSubgroupContainer>(Create());
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<XWInspectorSubgroup>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetIndentDepth && args.Count == 1)
		{
			SetIndentDepth(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLevel && args.Count == 1)
		{
			SetLevel(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddEditor && args.Count == 1)
		{
			AddEditor(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
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
		if (method == MethodName._GuiInput && args.Count == 1)
		{
			_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResortChildren && args.Count == 0)
		{
			ResortChildren();
			ret = default;
			return true;
		}
		if (method == MethodName.GetHeaderHeight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetHeaderHeight());
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName._GetMinimumSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(_GetMinimumSize());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorSubgroupContainer>(Create());
			return true;
		}
		if (method == MethodName.LoadIcons && args.Count == 0)
		{
			LoadIcons();
			ret = default;
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.SetIndentDepth)
		{
			return true;
		}
		if (method == MethodName.SetLevel)
		{
			return true;
		}
		if (method == MethodName.AddEditor)
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
		if (method == MethodName._GuiInput)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.ResortChildren)
		{
			return true;
		}
		if (method == MethodName.GetHeaderHeight)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName._GetMinimumSize)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._vbox)
		{
			_vbox = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			_editorContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.Subgroup)
		{
			Subgroup = VariantUtils.ConvertTo<XWInspectorSubgroup>(in value);
			return true;
		}
		if (name == PropertyName._isFolded)
		{
			_isFolded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._headerHover)
		{
			_headerHover = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._indentDepth)
		{
			_indentDepth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bgColor)
		{
			_bgColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._level)
		{
			_level = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._vbox)
		{
			value = VariantUtils.CreateFrom(in _vbox);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			value = VariantUtils.CreateFrom(in _editorContainer);
			return true;
		}
		if (name == PropertyName.Subgroup)
		{
			value = VariantUtils.CreateFrom(in Subgroup);
			return true;
		}
		if (name == PropertyName._isFolded)
		{
			value = VariantUtils.CreateFrom(in _isFolded);
			return true;
		}
		if (name == PropertyName._headerHover)
		{
			value = VariantUtils.CreateFrom(in _headerHover);
			return true;
		}
		if (name == PropertyName._indentDepth)
		{
			value = VariantUtils.CreateFrom(in _indentDepth);
			return true;
		}
		if (name == PropertyName._bgColor)
		{
			value = VariantUtils.CreateFrom(in _bgColor);
			return true;
		}
		if (name == PropertyName._level)
		{
			value = VariantUtils.CreateFrom(in _level);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._vbox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Subgroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isFolded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._headerHover, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._indentDepth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._bgColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._level, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._vbox, Variant.From(in _vbox));
		info.AddProperty(PropertyName._editorContainer, Variant.From(in _editorContainer));
		info.AddProperty(PropertyName.Subgroup, Variant.From(in Subgroup));
		info.AddProperty(PropertyName._isFolded, Variant.From(in _isFolded));
		info.AddProperty(PropertyName._headerHover, Variant.From(in _headerHover));
		info.AddProperty(PropertyName._indentDepth, Variant.From(in _indentDepth));
		info.AddProperty(PropertyName._bgColor, Variant.From(in _bgColor));
		info.AddProperty(PropertyName._level, Variant.From(in _level));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._vbox, out var value))
		{
			_vbox = value.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._editorContainer, out var value2))
		{
			_editorContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.Subgroup, out var value3))
		{
			Subgroup = value3.As<XWInspectorSubgroup>();
		}
		if (info.TryGetProperty(PropertyName._isFolded, out var value4))
		{
			_isFolded = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._headerHover, out var value5))
		{
			_headerHover = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._indentDepth, out var value6))
		{
			_indentDepth = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bgColor, out var value7))
		{
			_bgColor = value7.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._level, out var value8))
		{
			_level = value8.As<int>();
		}
	}
}
