using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/RefCounted/XWInspectorGroup.cs")]
public class XWInspectorGroup : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SetContainer = "SetContainer";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName GroupContainer = "GroupContainer";

		public static readonly StringName GroupName = "GroupName";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public XWInspectorGroupContainer GroupContainer;

	public StringName GroupName = "";

	public XWInspectorGroup()
	{
	}

	public XWInspectorGroup(StringName groupName)
	{
		GroupName = groupName;
	}

	public void SetContainer(XWInspectorGroupContainer container)
	{
		GroupContainer = container;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.SetContainer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetContainer && args.Count == 1)
		{
			SetContainer(VariantUtils.ConvertTo<XWInspectorGroupContainer>(in args[0]));
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
		if (name == PropertyName.GroupContainer)
		{
			GroupContainer = VariantUtils.ConvertTo<XWInspectorGroupContainer>(in value);
			return true;
		}
		if (name == PropertyName.GroupName)
		{
			GroupName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GroupContainer)
		{
			value = VariantUtils.CreateFrom(in GroupContainer);
			return true;
		}
		if (name == PropertyName.GroupName)
		{
			value = VariantUtils.CreateFrom(in GroupName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.GroupContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.GroupName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GroupContainer, Variant.From(in GroupContainer));
		info.AddProperty(PropertyName.GroupName, Variant.From(in GroupName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GroupContainer, out var value))
		{
			GroupContainer = value.As<XWInspectorGroupContainer>();
		}
		if (info.TryGetProperty(PropertyName.GroupName, out var value2))
		{
			GroupName = value2.As<StringName>();
		}
	}
}
