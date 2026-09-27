using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWDragData.cs")]
public class XWDragData : RefCounted
{
	public enum Type
	{
		Unknown,
		BpFunction,
		BpVariable,
		BpSignal,
		FileSystem,
		DockPanel
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName IsValidDragData = "IsValidDragData";

		public static readonly StringName IsType = "IsType";

		public static readonly StringName GetPanelKey = "GetPanelKey";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Data = "Data";

		public static readonly StringName DragType = "DragType";

		public static readonly StringName Paths = "Paths";

		public static readonly StringName HasDirs = "HasDirs";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public Variant Data;

	public Type DragType;

	public string[] Paths = Array.Empty<string>();

	public bool HasDirs;

	public XWDragData()
	{
	}

	public XWDragData(Variant data, Type type)
	{
		Data = data;
		DragType = type;
	}

	public static bool IsValidDragData(Variant dragData)
	{
		return dragData.Obj is XWDragData;
	}

	public bool IsType(Type expectedType)
	{
		return DragType == expectedType;
	}

	public string GetPanelKey()
	{
		if (DragType != Type.DockPanel)
		{
			return "";
		}
		return Data.AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.IsValidDragData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "dragData", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.IsType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "expectedType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPanelKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsValidDragData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidDragData(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.IsType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsType(VariantUtils.ConvertTo<Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPanelKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetPanelKey());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsValidDragData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidDragData(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsValidDragData)
		{
			return true;
		}
		if (method == MethodName.IsType)
		{
			return true;
		}
		if (method == MethodName.GetPanelKey)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Data)
		{
			Data = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.DragType)
		{
			DragType = VariantUtils.ConvertTo<Type>(in value);
			return true;
		}
		if (name == PropertyName.Paths)
		{
			Paths = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName.HasDirs)
		{
			HasDirs = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Data)
		{
			value = VariantUtils.CreateFrom(in Data);
			return true;
		}
		if (name == PropertyName.DragType)
		{
			value = VariantUtils.CreateFrom(in DragType);
			return true;
		}
		if (name == PropertyName.Paths)
		{
			value = VariantUtils.CreateFrom(in Paths);
			return true;
		}
		if (name == PropertyName.HasDirs)
		{
			value = VariantUtils.CreateFrom(in HasDirs);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, PropertyName.Data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DragType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName.Paths, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDirs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Data, Variant.From(in Data));
		info.AddProperty(PropertyName.DragType, Variant.From(in DragType));
		info.AddProperty(PropertyName.Paths, Variant.From(in Paths));
		info.AddProperty(PropertyName.HasDirs, Variant.From(in HasDirs));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Data, out var value))
		{
			Data = value.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.DragType, out var value2))
		{
			DragType = value2.As<Type>();
		}
		if (info.TryGetProperty(PropertyName.Paths, out var value3))
		{
			Paths = value3.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName.HasDirs, out var value4))
		{
			HasDirs = value4.As<bool>();
		}
	}
}
