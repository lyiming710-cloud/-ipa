using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/BroadCastManager/Resource/BroadCastConfig.cs")]
public class BroadCastConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName broadCastString = "broadCastString";

		public static readonly StringName broadCastTime = "broadCastTime";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.MultilineText, "")]
	public string broadCastString = "";

	[Export(PropertyHint.None, "")]
	public double broadCastTime = -1.0;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.broadCastString)
		{
			broadCastString = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.broadCastTime)
		{
			broadCastTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.broadCastString)
		{
			value = VariantUtils.CreateFrom(in broadCastString);
			return true;
		}
		if (name == PropertyName.broadCastTime)
		{
			value = VariantUtils.CreateFrom(in broadCastTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.broadCastString, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.broadCastTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.broadCastString, Variant.From(in broadCastString));
		info.AddProperty(PropertyName.broadCastTime, Variant.From(in broadCastTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.broadCastString, out var value))
		{
			broadCastString = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.broadCastTime, out var value2))
		{
			broadCastTime = value2.As<double>();
		}
	}
}
