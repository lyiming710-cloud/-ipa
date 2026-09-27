using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
public class InspectorNestedContinuousProbeResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName CreateTypedWeights = "CreateTypedWeights";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ProbeNames = "ProbeNames";

		public static readonly StringName ProbeWeights = "ProbeWeights";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<string> ProbeNames { get; set; } = new Array<string> { "alpha", "beta" };

	[Export(PropertyHint.None, "")]
	public Dictionary ProbeWeights { get; set; } = CreateTypedWeights();

	private static Dictionary CreateTypedWeights()
	{
		return (Dictionary?)new Godot.Collections.Dictionary<string, int>
		{
			{ "pea", 3 },
			{ "sun", 5 }
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.CreateTypedWeights, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateTypedWeights && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateTypedWeights());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateTypedWeights && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateTypedWeights());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateTypedWeights)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProbeNames)
		{
			ProbeNames = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.ProbeWeights)
		{
			ProbeWeights = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ProbeNames)
		{
			value = VariantUtils.CreateFromArray(ProbeNames);
			return true;
		}
		if (name == PropertyName.ProbeWeights)
		{
			value = VariantUtils.CreateFrom<Dictionary>(ProbeWeights);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.ProbeNames, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.ProbeWeights, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProbeNames, Variant.CreateFrom(ProbeNames));
		info.AddProperty(PropertyName.ProbeWeights, Variant.From<Dictionary>(ProbeWeights));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProbeNames, out var value))
		{
			ProbeNames = value.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.ProbeWeights, out var value2))
		{
			ProbeWeights = value2.As<Dictionary>();
		}
	}
}
