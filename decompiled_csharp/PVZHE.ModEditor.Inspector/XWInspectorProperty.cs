using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/RefCounted/XWInspectorProperty.cs")]
public class XWInspectorProperty : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SetCall = "SetCall";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Object = "Object";

		public static readonly StringName PropName = "PropName";

		public static readonly StringName RestValue = "RestValue";

		public static readonly StringName GroupName = "GroupName";

		public static readonly StringName SubgroupName = "SubgroupName";

		public static readonly StringName CategoryName = "CategoryName";

		public static readonly StringName GroupBase = "GroupBase";

		public static readonly StringName SubgroupBase = "SubgroupBase";

		public static readonly StringName SectionDepth = "SectionDepth";

		public static readonly StringName Hint = "Hint";

		public static readonly StringName HintString = "HintString";

		public static readonly StringName ReadOnly = "ReadOnly";

		public static readonly StringName Description = "Description";

		public static readonly StringName PropertyUsage = "PropertyUsage";

		public static readonly StringName LabelOverride = "LabelOverride";

		public static readonly StringName SetCallable = "SetCallable";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public Callable SetCallable;

	public GodotObject Object { get; set; }

	public StringName PropName { get; set; } = "";

	public Variant RestValue { get; set; }

	public StringName GroupName { get; set; } = "";

	public StringName SubgroupName { get; set; } = "";

	public StringName CategoryName { get; set; } = "";

	public StringName GroupBase { get; set; } = "";

	public StringName SubgroupBase { get; set; } = "";

	public int SectionDepth { get; set; }

	public PropertyHint Hint { get; set; }

	public string HintString { get; set; } = "";

	public bool ReadOnly { get; set; }

	public string Description { get; set; } = "";

	public long PropertyUsage { get; set; }

	public string LabelOverride { get; set; } = "";

	public XWInspectorProperty()
	{
	}

	public XWInspectorProperty(GodotObject obj, StringName propName, Variant restValue = default(Variant), StringName groupName = null, StringName subgroupName = null, PropertyHint hint = PropertyHint.None, string hintString = "", Callable setCallable = default(Callable))
	{
		Object = obj;
		PropName = propName;
		RestValue = restValue;
		GroupName = groupName ?? ((StringName)"");
		SubgroupName = subgroupName ?? ((StringName)"");
		Hint = hint;
		HintString = hintString;
		SetCallable = setCallable;
	}

	public void SetCall()
	{
		if (!SetCallable.Equals(default(Callable)))
		{
			SetCallable.Call();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.SetCall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetCall && args.Count == 0)
		{
			SetCall();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetCall)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Object)
		{
			Object = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName.PropName)
		{
			PropName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.RestValue)
		{
			RestValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.GroupName)
		{
			GroupName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.SubgroupName)
		{
			SubgroupName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.CategoryName)
		{
			CategoryName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.GroupBase)
		{
			GroupBase = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.SubgroupBase)
		{
			SubgroupBase = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.SectionDepth)
		{
			SectionDepth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Hint)
		{
			Hint = VariantUtils.ConvertTo<PropertyHint>(in value);
			return true;
		}
		if (name == PropertyName.HintString)
		{
			HintString = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ReadOnly)
		{
			ReadOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Description)
		{
			Description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.PropertyUsage)
		{
			PropertyUsage = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.LabelOverride)
		{
			LabelOverride = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SetCallable)
		{
			SetCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Object)
		{
			value = VariantUtils.CreateFrom<GodotObject>(Object);
			return true;
		}
		StringName from;
		if (name == PropertyName.PropName)
		{
			from = PropName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RestValue)
		{
			value = VariantUtils.CreateFrom<Variant>(RestValue);
			return true;
		}
		if (name == PropertyName.GroupName)
		{
			from = GroupName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SubgroupName)
		{
			from = SubgroupName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CategoryName)
		{
			from = CategoryName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.GroupBase)
		{
			from = GroupBase;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SubgroupBase)
		{
			from = SubgroupBase;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SectionDepth)
		{
			value = VariantUtils.CreateFrom<int>(SectionDepth);
			return true;
		}
		if (name == PropertyName.Hint)
		{
			value = VariantUtils.CreateFrom<PropertyHint>(Hint);
			return true;
		}
		string from2;
		if (name == PropertyName.HintString)
		{
			from2 = HintString;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ReadOnly)
		{
			value = VariantUtils.CreateFrom<bool>(ReadOnly);
			return true;
		}
		if (name == PropertyName.Description)
		{
			from2 = Description;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PropertyUsage)
		{
			value = VariantUtils.CreateFrom<long>(PropertyUsage);
			return true;
		}
		if (name == PropertyName.LabelOverride)
		{
			from2 = LabelOverride;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SetCallable)
		{
			value = VariantUtils.CreateFrom(in SetCallable);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Object, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.PropName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.RestValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.GroupName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.SubgroupName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.CategoryName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.GroupBase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.SubgroupBase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SectionDepth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName.SetCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Hint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.HintString, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ReadOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PropertyUsage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LabelOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Object, Variant.From<GodotObject>(Object));
		info.AddProperty(PropertyName.PropName, Variant.From<StringName>(PropName));
		info.AddProperty(PropertyName.RestValue, Variant.From<Variant>(RestValue));
		info.AddProperty(PropertyName.GroupName, Variant.From<StringName>(GroupName));
		info.AddProperty(PropertyName.SubgroupName, Variant.From<StringName>(SubgroupName));
		info.AddProperty(PropertyName.CategoryName, Variant.From<StringName>(CategoryName));
		info.AddProperty(PropertyName.GroupBase, Variant.From<StringName>(GroupBase));
		info.AddProperty(PropertyName.SubgroupBase, Variant.From<StringName>(SubgroupBase));
		info.AddProperty(PropertyName.SectionDepth, Variant.From<int>(SectionDepth));
		info.AddProperty(PropertyName.Hint, Variant.From<PropertyHint>(Hint));
		info.AddProperty(PropertyName.HintString, Variant.From<string>(HintString));
		info.AddProperty(PropertyName.ReadOnly, Variant.From<bool>(ReadOnly));
		info.AddProperty(PropertyName.Description, Variant.From<string>(Description));
		info.AddProperty(PropertyName.PropertyUsage, Variant.From<long>(PropertyUsage));
		info.AddProperty(PropertyName.LabelOverride, Variant.From<string>(LabelOverride));
		info.AddProperty(PropertyName.SetCallable, Variant.From(in SetCallable));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Object, out var value))
		{
			Object = value.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName.PropName, out var value2))
		{
			PropName = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.RestValue, out var value3))
		{
			RestValue = value3.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.GroupName, out var value4))
		{
			GroupName = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.SubgroupName, out var value5))
		{
			SubgroupName = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.CategoryName, out var value6))
		{
			CategoryName = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.GroupBase, out var value7))
		{
			GroupBase = value7.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.SubgroupBase, out var value8))
		{
			SubgroupBase = value8.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.SectionDepth, out var value9))
		{
			SectionDepth = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Hint, out var value10))
		{
			Hint = value10.As<PropertyHint>();
		}
		if (info.TryGetProperty(PropertyName.HintString, out var value11))
		{
			HintString = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ReadOnly, out var value12))
		{
			ReadOnly = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Description, out var value13))
		{
			Description = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.PropertyUsage, out var value14))
		{
			PropertyUsage = value14.As<long>();
		}
		if (info.TryGetProperty(PropertyName.LabelOverride, out var value15))
		{
			LabelOverride = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SetCallable, out var value16))
		{
			SetCallable = value16.As<Callable>();
		}
	}
}
