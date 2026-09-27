using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefenseItemConfig.cs")]
public class TowerDefenseItemConfig : TowerDefenseCharacterConfig
{
	public new class MethodName : TowerDefenseCharacterConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCharacterConfig.PropertyName
	{
		public static readonly StringName isLadder = "isLadder";

		public static readonly StringName isShield = "isShield";
	}

	public new class SignalName : TowerDefenseCharacterConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool isLadder;

	[Export(PropertyHint.None, "")]
	public bool isShield;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.isLadder)
		{
			isLadder = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isShield)
		{
			isShield = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.isLadder)
		{
			value = VariantUtils.CreateFrom(in isLadder);
			return true;
		}
		if (name == PropertyName.isShield)
		{
			value = VariantUtils.CreateFrom(in isShield);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.isLadder, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isShield, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.isLadder, Variant.From(in isLadder));
		info.AddProperty(PropertyName.isShield, Variant.From(in isShield));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.isLadder, out var value))
		{
			isLadder = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isShield, out var value2))
		{
			isShield = value2.As<bool>();
		}
	}
}
