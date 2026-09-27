using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefenseCraterConfig.cs")]
public class TowerDefenseCraterConfig : TowerDefenseCharacterConfig
{
	public new class MethodName : TowerDefenseCharacterConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCharacterConfig.PropertyName
	{
		public static readonly StringName dieDownTime = "dieDownTime";

		public static readonly StringName dieDownFliters = "dieDownFliters";
	}

	public new class SignalName : TowerDefenseCharacterConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double dieDownTime = 60.0;

	[Export(PropertyHint.None, "")]
	public Array<string> dieDownFliters = new Array<string>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dieDownTime)
		{
			dieDownTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dieDownFliters)
		{
			dieDownFliters = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dieDownTime)
		{
			value = VariantUtils.CreateFrom(in dieDownTime);
			return true;
		}
		if (name == PropertyName.dieDownFliters)
		{
			value = VariantUtils.CreateFromArray(dieDownFliters);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.dieDownTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.dieDownFliters, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dieDownTime, Variant.From(in dieDownTime));
		info.AddProperty(PropertyName.dieDownFliters, Variant.CreateFrom(dieDownFliters));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dieDownTime, out var value))
		{
			dieDownTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dieDownFliters, out var value2))
		{
			dieDownFliters = value2.AsGodotArray<string>();
		}
	}
}
