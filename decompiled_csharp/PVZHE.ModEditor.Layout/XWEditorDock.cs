using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWEditorDock.cs")]
public class XWEditorDock : MarginContainer
{
	public new class MethodName : MarginContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetTitle = "SetTitle";

		public static readonly StringName MakeFloating = "MakeFloating";

		public static readonly StringName MakeDocked = "MakeDocked";

		public static readonly StringName SetDockVisible = "SetDockVisible";

		public static readonly StringName IsFloating = "IsFloating";

		public static readonly StringName Close = "Close";
	}

	public new class PropertyName : MarginContainer.PropertyName
	{
		public static readonly StringName LayoutKey = "LayoutKey";

		public static readonly StringName Title = "Title";

		public static readonly StringName DefaultSlot = "DefaultSlot";

		public static readonly StringName Closable = "Closable";

		public static readonly StringName IsGlobal = "IsGlobal";

		public static readonly StringName IsTransient = "IsTransient";

		public static readonly StringName IsMainScreen = "IsMainScreen";

		public static readonly StringName DockIcon = "DockIcon";

		public static readonly StringName Shortcut = "Shortcut";

		public static readonly StringName _layoutManager = "_layoutManager";

		public static readonly StringName _floatingWindow = "_floatingWindow";

		public static readonly StringName _isFloating = "_isFloating";
	}

	public new class SignalName : MarginContainer.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string LayoutKey = "";

	[Export(PropertyHint.None, "")]
	public string Title = "";

	[Export(PropertyHint.None, "")]
	public int DefaultSlot;

	[Export(PropertyHint.None, "")]
	public bool Closable = true;

	[Export(PropertyHint.None, "")]
	public bool IsGlobal;

	[Export(PropertyHint.None, "")]
	public bool IsTransient;

	[Export(PropertyHint.None, "")]
	public bool IsMainScreen;

	public Texture2D DockIcon;

	public Shortcut Shortcut;

	protected XWLayoutManager _layoutManager;

	protected XWWindowWrapper _floatingWindow;

	protected bool _isFloating;

	public override void _Ready()
	{
		_layoutManager = XWLayoutManager.Instance;
		AddThemeConstantOverride("margin_top", 0);
		AddThemeConstantOverride("margin_bottom", 0);
		AddThemeConstantOverride("margin_left", 0);
		AddThemeConstantOverride("margin_right", 0);
		if (!string.IsNullOrEmpty(Title))
		{
			Name = Title;
		}
	}

	public void SetTitle(string title)
	{
		Title = title;
		Name = title;
		if (GetParent() is TabContainer tabContainer)
		{
			int num = tabContainer.GetChildren().IndexOf(this);
			if (num >= 0)
			{
				tabContainer.SetTabTitle(num, title);
			}
		}
	}

	public void MakeFloating(Rect2I rect = default(Rect2I))
	{
		if (!_isFloating)
		{
			_floatingWindow = new XWWindowWrapper();
			_floatingWindow.Title = Title;
			if (!rect.Equals(default))
			{
				_floatingWindow.Position = rect.Position;
				_floatingWindow.Size = rect.Size;
			}
			GetParent()?.RemoveChild(this);
			_layoutManager?.RefreshDockVisibility();
			_floatingWindow.AddChild(this, forceReadableName: false, InternalMode.Disabled);
			_layoutManager?.GetRoot()?.AddChild(_floatingWindow, forceReadableName: false, InternalMode.Disabled);
			_floatingWindow.CloseRequested += MakeDocked;
			_isFloating = true;
		}
	}

	public void MakeDocked()
	{
		if (_isFloating && _floatingWindow != null)
		{
			_floatingWindow.RemoveChild(this);
			_floatingWindow.QueueFree();
			_floatingWindow = null;
			_isFloating = false;
			_layoutManager?.RestoreDock(this);
		}
	}

	public void SetDockVisible(bool visible)
	{
		Visible = visible;
	}

	public bool IsFloating()
	{
		return _isFloating;
	}

	public void Close()
	{
		if (Closable)
		{
			if (_isFloating)
			{
				_floatingWindow?.QueueFree();
				return;
			}
			_layoutManager?.RemoveControlFromDock(this);
			QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeFloating, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2I, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeDocked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDockVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFloating, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Close, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetTitle && args.Count == 1)
		{
			SetTitle(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeFloating && args.Count == 1)
		{
			MakeFloating(VariantUtils.ConvertTo<Rect2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeDocked && args.Count == 0)
		{
			MakeDocked();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDockVisible && args.Count == 1)
		{
			SetDockVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsFloating && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFloating());
			return true;
		}
		if (method == MethodName.Close && args.Count == 0)
		{
			Close();
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
		if (method == MethodName.SetTitle)
		{
			return true;
		}
		if (method == MethodName.MakeFloating)
		{
			return true;
		}
		if (method == MethodName.MakeDocked)
		{
			return true;
		}
		if (method == MethodName.SetDockVisible)
		{
			return true;
		}
		if (method == MethodName.IsFloating)
		{
			return true;
		}
		if (method == MethodName.Close)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LayoutKey)
		{
			LayoutKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Title)
		{
			Title = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DefaultSlot)
		{
			DefaultSlot = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Closable)
		{
			Closable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsGlobal)
		{
			IsGlobal = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsTransient)
		{
			IsTransient = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsMainScreen)
		{
			IsMainScreen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.DockIcon)
		{
			DockIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.Shortcut)
		{
			Shortcut = VariantUtils.ConvertTo<Shortcut>(in value);
			return true;
		}
		if (name == PropertyName._layoutManager)
		{
			_layoutManager = VariantUtils.ConvertTo<XWLayoutManager>(in value);
			return true;
		}
		if (name == PropertyName._floatingWindow)
		{
			_floatingWindow = VariantUtils.ConvertTo<XWWindowWrapper>(in value);
			return true;
		}
		if (name == PropertyName._isFloating)
		{
			_isFloating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.LayoutKey)
		{
			value = VariantUtils.CreateFrom(in LayoutKey);
			return true;
		}
		if (name == PropertyName.Title)
		{
			value = VariantUtils.CreateFrom(in Title);
			return true;
		}
		if (name == PropertyName.DefaultSlot)
		{
			value = VariantUtils.CreateFrom(in DefaultSlot);
			return true;
		}
		if (name == PropertyName.Closable)
		{
			value = VariantUtils.CreateFrom(in Closable);
			return true;
		}
		if (name == PropertyName.IsGlobal)
		{
			value = VariantUtils.CreateFrom(in IsGlobal);
			return true;
		}
		if (name == PropertyName.IsTransient)
		{
			value = VariantUtils.CreateFrom(in IsTransient);
			return true;
		}
		if (name == PropertyName.IsMainScreen)
		{
			value = VariantUtils.CreateFrom(in IsMainScreen);
			return true;
		}
		if (name == PropertyName.DockIcon)
		{
			value = VariantUtils.CreateFrom(in DockIcon);
			return true;
		}
		if (name == PropertyName.Shortcut)
		{
			value = VariantUtils.CreateFrom(in Shortcut);
			return true;
		}
		if (name == PropertyName._layoutManager)
		{
			value = VariantUtils.CreateFrom(in _layoutManager);
			return true;
		}
		if (name == PropertyName._floatingWindow)
		{
			value = VariantUtils.CreateFrom(in _floatingWindow);
			return true;
		}
		if (name == PropertyName._isFloating)
		{
			value = VariantUtils.CreateFrom(in _isFloating);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.LayoutKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.Title, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.DefaultSlot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Closable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsGlobal, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsTransient, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsMainScreen, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.DockIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Shortcut, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._floatingWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isFloating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LayoutKey, Variant.From(in LayoutKey));
		info.AddProperty(PropertyName.Title, Variant.From(in Title));
		info.AddProperty(PropertyName.DefaultSlot, Variant.From(in DefaultSlot));
		info.AddProperty(PropertyName.Closable, Variant.From(in Closable));
		info.AddProperty(PropertyName.IsGlobal, Variant.From(in IsGlobal));
		info.AddProperty(PropertyName.IsTransient, Variant.From(in IsTransient));
		info.AddProperty(PropertyName.IsMainScreen, Variant.From(in IsMainScreen));
		info.AddProperty(PropertyName.DockIcon, Variant.From(in DockIcon));
		info.AddProperty(PropertyName.Shortcut, Variant.From(in Shortcut));
		info.AddProperty(PropertyName._layoutManager, Variant.From(in _layoutManager));
		info.AddProperty(PropertyName._floatingWindow, Variant.From(in _floatingWindow));
		info.AddProperty(PropertyName._isFloating, Variant.From(in _isFloating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LayoutKey, out var value))
		{
			LayoutKey = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Title, out var value2))
		{
			Title = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DefaultSlot, out var value3))
		{
			DefaultSlot = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Closable, out var value4))
		{
			Closable = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsGlobal, out var value5))
		{
			IsGlobal = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsTransient, out var value6))
		{
			IsTransient = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsMainScreen, out var value7))
		{
			IsMainScreen = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DockIcon, out var value8))
		{
			DockIcon = value8.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.Shortcut, out var value9))
		{
			Shortcut = value9.As<Shortcut>();
		}
		if (info.TryGetProperty(PropertyName._layoutManager, out var value10))
		{
			_layoutManager = value10.As<XWLayoutManager>();
		}
		if (info.TryGetProperty(PropertyName._floatingWindow, out var value11))
		{
			_floatingWindow = value11.As<XWWindowWrapper>();
		}
		if (info.TryGetProperty(PropertyName._isFloating, out var value12))
		{
			_isFloating = value12.As<bool>();
		}
	}
}
