using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/XWBPClipboardData.cs")]
public class XWBPClipboardData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName IsEmpty = "IsEmpty";

		public static readonly StringName HasNodes = "HasNodes";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName CenterPosition = "CenterPosition";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public List<XWBPNodeData> Nodes { get; set; } = new List<XWBPNodeData>();

	public List<XWBPNodeConnectionData> Connections { get; set; } = new List<XWBPNodeConnectionData>();

	public Vector2 CenterPosition { get; set; } = Vector2.Zero;

	public bool IsEmpty()
	{
		return Nodes.Count == 0;
	}

	public bool HasNodes()
	{
		return Nodes.Count > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.IsEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasNodes, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEmpty());
			return true;
		}
		if (method == MethodName.HasNodes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasNodes());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsEmpty)
		{
			return true;
		}
		if (method == MethodName.HasNodes)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.CenterPosition)
		{
			CenterPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CenterPosition)
		{
			value = VariantUtils.CreateFrom<Vector2>(CenterPosition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName.CenterPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.CenterPosition, Variant.From<Vector2>(CenterPosition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.CenterPosition, out var value))
		{
			CenterPosition = value.As<Vector2>();
		}
	}
}
