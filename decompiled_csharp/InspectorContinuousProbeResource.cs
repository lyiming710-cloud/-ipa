using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
public class InspectorContinuousProbeResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ProbeTitle = "ProbeTitle";

		public static readonly StringName ProbeSpeed = "ProbeSpeed";

		public static readonly StringName ProbeOffset = "ProbeOffset";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ProbeTitle { get; set; } = "before-title";

	[Export(PropertyHint.None, "")]
	public double ProbeSpeed { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public Vector2 ProbeOffset { get; set; } = new Vector2(3f, 4f);

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProbeTitle)
		{
			ProbeTitle = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ProbeSpeed)
		{
			ProbeSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ProbeOffset)
		{
			ProbeOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ProbeTitle)
		{
			value = VariantUtils.CreateFrom<string>(ProbeTitle);
			return true;
		}
		if (name == PropertyName.ProbeSpeed)
		{
			value = VariantUtils.CreateFrom<double>(ProbeSpeed);
			return true;
		}
		if (name == PropertyName.ProbeOffset)
		{
			value = VariantUtils.CreateFrom<Vector2>(ProbeOffset);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ProbeTitle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ProbeSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.ProbeOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProbeTitle, Variant.From<string>(ProbeTitle));
		info.AddProperty(PropertyName.ProbeSpeed, Variant.From<double>(ProbeSpeed));
		info.AddProperty(PropertyName.ProbeOffset, Variant.From<Vector2>(ProbeOffset));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProbeTitle, out var value))
		{
			ProbeTitle = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ProbeSpeed, out var value2))
		{
			ProbeSpeed = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ProbeOffset, out var value3))
		{
			ProbeOffset = value3.As<Vector2>();
		}
	}
}
