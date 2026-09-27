using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWBottomPanel.cs")]
public class XWBottomPanel : VBoxContainer
{
	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnPinToggled = "OnPinToggled";

		public static readonly StringName OnExpandToggled = "OnExpandToggled";

		public static readonly StringName ApplyExpandedState = "ApplyExpandedState";

		public static readonly StringName SyncCurrentHeight = "SyncCurrentHeight";

		public static readonly StringName SyncParentSplitOffset = "SyncParentSplitOffset";

		public static readonly StringName AddTab = "AddTab";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName ContentContainer = "ContentContainer";

		public static readonly StringName ToasterContainer = "ToasterContainer";

		public static readonly StringName _pinButton = "_pinButton";

		public static readonly StringName _expandButton = "_expandButton";

		public static readonly StringName _toasterContainer = "_toasterContainer";

		public static readonly StringName _layoutButton = "_layoutButton";

		public static readonly StringName _contentContainer = "_contentContainer";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	private const int CollapsedHeight = 32;

	private const int ExpandedHeight = 220;

	private Button _pinButton;

	private Button _expandButton;

	private HBoxContainer _toasterContainer;

	private MenuButton _layoutButton;

	private TabContainer _contentContainer;

	public TabContainer ContentContainer => _contentContainer;

	public HBoxContainer ToasterContainer => _toasterContainer;

	public override void _Ready()
	{
		_pinButton = GetNode<Button>("%PinButton");
		_expandButton = GetNode<Button>("%ExpandButton");
		_toasterContainer = GetNode<HBoxContainer>("%ToasterContainer");
		_layoutButton = GetNode<MenuButton>("%LayoutButton");
		_contentContainer = GetNode<TabContainer>("%ContentContainer");
		SizeFlagsVertical = SizeFlags.ExpandFill;
		_pinButton.Toggled += OnPinToggled;
		_expandButton.Toggled += OnExpandToggled;
		ApplyExpandedState(_expandButton.ButtonPressed);
	}

	private void OnPinToggled(bool pinned)
	{
	}

	private void OnExpandToggled(bool expanded)
	{
		ApplyExpandedState(expanded);
	}

	private void ApplyExpandedState(bool expanded)
	{
		int num = (expanded ? 220 : 32);
		_contentContainer.Visible = expanded;
		SizeFlagsVertical = (SizeFlags)(expanded ? 3 : 0);
		CustomMinimumSize = new Vector2(0f, num);
		SyncParentSplitOffset(num);
		CallDeferred("SyncCurrentHeight");
	}

	private void SyncCurrentHeight()
	{
		SyncParentSplitOffset((int)CustomMinimumSize.Y);
	}

	private void SyncParentSplitOffset(int targetHeight)
	{
		if (GetParent() is VSplitContainer vSplitContainer)
		{
			vSplitContainer.SplitOffsets = new int[1] { -Mathf.Max(targetHeight, 32) };
		}
	}

	public void AddTab(Control content, string title)
	{
		_contentContainer.AddChild(content, forceReadableName: false, InternalMode.Disabled);
		_contentContainer.SetTabTitle(_contentContainer.GetTabCount() - 1, title);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPinToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pinned", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnExpandToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "expanded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyExpandedState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "expanded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncCurrentHeight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncParentSplitOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "targetHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnPinToggled && args.Count == 1)
		{
			OnPinToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnExpandToggled && args.Count == 1)
		{
			OnExpandToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyExpandedState && args.Count == 1)
		{
			ApplyExpandedState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCurrentHeight && args.Count == 0)
		{
			SyncCurrentHeight();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncParentSplitOffset && args.Count == 1)
		{
			SyncParentSplitOffset(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddTab && args.Count == 2)
		{
			AddTab(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.OnPinToggled)
		{
			return true;
		}
		if (method == MethodName.OnExpandToggled)
		{
			return true;
		}
		if (method == MethodName.ApplyExpandedState)
		{
			return true;
		}
		if (method == MethodName.SyncCurrentHeight)
		{
			return true;
		}
		if (method == MethodName.SyncParentSplitOffset)
		{
			return true;
		}
		if (method == MethodName.AddTab)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._pinButton)
		{
			_pinButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._expandButton)
		{
			_expandButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._toasterContainer)
		{
			_toasterContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._layoutButton)
		{
			_layoutButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._contentContainer)
		{
			_contentContainer = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ContentContainer)
		{
			value = VariantUtils.CreateFrom<TabContainer>(ContentContainer);
			return true;
		}
		if (name == PropertyName.ToasterContainer)
		{
			value = VariantUtils.CreateFrom<HBoxContainer>(ToasterContainer);
			return true;
		}
		if (name == PropertyName._pinButton)
		{
			value = VariantUtils.CreateFrom(in _pinButton);
			return true;
		}
		if (name == PropertyName._expandButton)
		{
			value = VariantUtils.CreateFrom(in _expandButton);
			return true;
		}
		if (name == PropertyName._toasterContainer)
		{
			value = VariantUtils.CreateFrom(in _toasterContainer);
			return true;
		}
		if (name == PropertyName._layoutButton)
		{
			value = VariantUtils.CreateFrom(in _layoutButton);
			return true;
		}
		if (name == PropertyName._contentContainer)
		{
			value = VariantUtils.CreateFrom(in _contentContainer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._pinButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expandButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toasterContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._contentContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ContentContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ToasterContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._pinButton, Variant.From(in _pinButton));
		info.AddProperty(PropertyName._expandButton, Variant.From(in _expandButton));
		info.AddProperty(PropertyName._toasterContainer, Variant.From(in _toasterContainer));
		info.AddProperty(PropertyName._layoutButton, Variant.From(in _layoutButton));
		info.AddProperty(PropertyName._contentContainer, Variant.From(in _contentContainer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._pinButton, out var value))
		{
			_pinButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._expandButton, out var value2))
		{
			_expandButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._toasterContainer, out var value3))
		{
			_toasterContainer = value3.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._layoutButton, out var value4))
		{
			_layoutButton = value4.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._contentContainer, out var value5))
		{
			_contentContainer = value5.As<TabContainer>();
		}
	}
}
