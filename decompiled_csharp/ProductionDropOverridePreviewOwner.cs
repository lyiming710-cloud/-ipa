using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ProductionDropOverridePreviewOwner : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName produceType = "produceType";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName produceInterval = "produceInterval";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string produceType { get; set; } = "QXSun";

	[Export(PropertyHint.None, "")]
	public int sunNum { get; set; } = 75;

	[Export(PropertyHint.None, "")]
	public double produceInterval { get; set; } = 18.0;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.produceType)
		{
			produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.produceInterval)
		{
			produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.produceType)
		{
			value = VariantUtils.CreateFrom<string>(produceType);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom<int>(sunNum);
			return true;
		}
		if (name == PropertyName.produceInterval)
		{
			value = VariantUtils.CreateFrom<double>(produceInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceType, Variant.From<string>(produceType));
		info.AddProperty(PropertyName.sunNum, Variant.From<int>(sunNum));
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.produceType, out var value))
		{
			produceType = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.sunNum, out var value2))
		{
			sunNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.produceInterval, out var value3))
		{
			produceInterval = value3.As<double>();
		}
	}
}
