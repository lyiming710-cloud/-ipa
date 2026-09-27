using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Node/XWBPNodeConnectionData.cs")]
public class XWBPNodeConnectionData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public new static readonly StringName Equals = "Equals";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName FromNodeId = "FromNodeId";

		public static readonly StringName FromPortIndex = "FromPortIndex";

		public static readonly StringName ToNodeId = "ToNodeId";

		public static readonly StringName ToPortIndex = "ToPortIndex";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public int FromNodeId { get; set; }

	public int FromPortIndex { get; set; }

	public int ToNodeId { get; set; }

	public int ToPortIndex { get; set; }

	public XWBPNodeConnectionData()
	{
	}

	public XWBPNodeConnectionData(int fromNodeId, int fromPortIndex, int toNodeId, int toPortIndex)
	{
		FromNodeId = fromNodeId;
		FromPortIndex = fromPortIndex;
		ToNodeId = toNodeId;
		ToPortIndex = toPortIndex;
	}

	public bool Equals(XWBPNodeConnectionData other)
	{
		if (other == null)
		{
			return false;
		}
		if (FromNodeId == other.FromNodeId && FromPortIndex == other.FromPortIndex && ToNodeId == other.ToNodeId)
		{
			return ToPortIndex == other.ToPortIndex;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Equals, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "other", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Equals && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Equals(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Equals)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FromNodeId)
		{
			FromNodeId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.FromPortIndex)
		{
			FromPortIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ToNodeId)
		{
			ToNodeId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ToPortIndex)
		{
			ToPortIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.FromNodeId)
		{
			from = FromNodeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.FromPortIndex)
		{
			from = FromPortIndex;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ToNodeId)
		{
			from = ToNodeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ToPortIndex)
		{
			from = ToPortIndex;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.FromNodeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FromPortIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ToNodeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ToPortIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FromNodeId, Variant.From<int>(FromNodeId));
		info.AddProperty(PropertyName.FromPortIndex, Variant.From<int>(FromPortIndex));
		info.AddProperty(PropertyName.ToNodeId, Variant.From<int>(ToNodeId));
		info.AddProperty(PropertyName.ToPortIndex, Variant.From<int>(ToPortIndex));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FromNodeId, out var value))
		{
			FromNodeId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.FromPortIndex, out var value2))
		{
			FromPortIndex = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ToNodeId, out var value3))
		{
			ToNodeId = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ToPortIndex, out var value4))
		{
			ToPortIndex = value4.As<int>();
		}
	}
}
