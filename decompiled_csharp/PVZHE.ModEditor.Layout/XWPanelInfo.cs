using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWPanelInfo.cs")]
public class XWPanelInfo : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Key = "Key";

		public static readonly StringName DisplayName = "DisplayName";

		public static readonly StringName Scene = "Scene";

		public static readonly StringName DefaultDock = "DefaultDock";

		public static readonly StringName CanClose = "CanClose";

		public static readonly StringName CanAddMultiple = "CanAddMultiple";

		public static readonly StringName IsMainPanel = "IsMainPanel";

		public static readonly StringName Icon = "Icon";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public string Key = "";

	public string DisplayName = "";

	public PackedScene Scene;

	public int DefaultDock;

	public bool CanClose = true;

	public bool CanAddMultiple;

	public bool IsMainPanel;

	public Texture2D Icon;

	public XWPanelInfo()
	{
	}

	public XWPanelInfo(string key, string name, PackedScene scene, int defaultDock = 0, bool canClose = true, bool canAddMultiple = false, bool isMainPanel = false, Texture2D icon = null)
	{
		Key = key;
		DisplayName = name;
		Scene = scene;
		DefaultDock = defaultDock;
		CanClose = canClose;
		CanAddMultiple = canAddMultiple;
		IsMainPanel = isMainPanel;
		Icon = icon;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Key)
		{
			Key = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			DisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Scene)
		{
			Scene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.DefaultDock)
		{
			DefaultDock = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CanClose)
		{
			CanClose = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CanAddMultiple)
		{
			CanAddMultiple = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsMainPanel)
		{
			IsMainPanel = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			Icon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Key)
		{
			value = VariantUtils.CreateFrom(in Key);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			value = VariantUtils.CreateFrom(in DisplayName);
			return true;
		}
		if (name == PropertyName.Scene)
		{
			value = VariantUtils.CreateFrom(in Scene);
			return true;
		}
		if (name == PropertyName.DefaultDock)
		{
			value = VariantUtils.CreateFrom(in DefaultDock);
			return true;
		}
		if (name == PropertyName.CanClose)
		{
			value = VariantUtils.CreateFrom(in CanClose);
			return true;
		}
		if (name == PropertyName.CanAddMultiple)
		{
			value = VariantUtils.CreateFrom(in CanAddMultiple);
			return true;
		}
		if (name == PropertyName.IsMainPanel)
		{
			value = VariantUtils.CreateFrom(in IsMainPanel);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			value = VariantUtils.CreateFrom(in Icon);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.Key, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.DisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Scene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DefaultDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanClose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanAddMultiple, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsMainPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Icon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Key, Variant.From(in Key));
		info.AddProperty(PropertyName.DisplayName, Variant.From(in DisplayName));
		info.AddProperty(PropertyName.Scene, Variant.From(in Scene));
		info.AddProperty(PropertyName.DefaultDock, Variant.From(in DefaultDock));
		info.AddProperty(PropertyName.CanClose, Variant.From(in CanClose));
		info.AddProperty(PropertyName.CanAddMultiple, Variant.From(in CanAddMultiple));
		info.AddProperty(PropertyName.IsMainPanel, Variant.From(in IsMainPanel));
		info.AddProperty(PropertyName.Icon, Variant.From(in Icon));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Key, out var value))
		{
			Key = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DisplayName, out var value2))
		{
			DisplayName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Scene, out var value3))
		{
			Scene = value3.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.DefaultDock, out var value4))
		{
			DefaultDock = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CanClose, out var value5))
		{
			CanClose = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CanAddMultiple, out var value6))
		{
			CanAddMultiple = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsMainPanel, out var value7))
		{
			IsMainPanel = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Icon, out var value8))
		{
			Icon = value8.As<Texture2D>();
		}
	}
}
