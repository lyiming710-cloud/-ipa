using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/RefCounted/XWInspectorSubgroup.cs")]
public class XWInspectorSubgroup : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SetContainer = "SetContainer";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName SubgroupContainer = "SubgroupContainer";

		public static readonly StringName SubgroupName = "SubgroupName";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public XWInspectorSubgroupContainer SubgroupContainer;

	public StringName SubgroupName = "";

	public XWInspectorSubgroup()
	{
	}

	public XWInspectorSubgroup(StringName subgroupName)
	{
		SubgroupName = subgroupName;
	}

	public void SetContainer(XWInspectorSubgroupContainer container)
	{
		SubgroupContainer = container;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.SetContainer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Container"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetContainer && args.Count == 1)
		{
			SetContainer(VariantUtils.ConvertTo<XWInspectorSubgroupContainer>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetContainer)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SubgroupContainer)
		{
			SubgroupContainer = VariantUtils.ConvertTo<XWInspectorSubgroupContainer>(in value);
			return true;
		}
		if (name == PropertyName.SubgroupName)
		{
			SubgroupName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.SubgroupContainer)
		{
			value = VariantUtils.CreateFrom(in SubgroupContainer);
			return true;
		}
		if (name == PropertyName.SubgroupName)
		{
			value = VariantUtils.CreateFrom(in SubgroupName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.SubgroupContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.SubgroupName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SubgroupContainer, Variant.From(in SubgroupContainer));
		info.AddProperty(PropertyName.SubgroupName, Variant.From(in SubgroupName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SubgroupContainer, out var value))
		{
			SubgroupContainer = value.As<XWInspectorSubgroupContainer>();
		}
		if (info.TryGetProperty(PropertyName.SubgroupName, out var value2))
		{
			SubgroupName = value2.As<StringName>();
		}
	}
}
