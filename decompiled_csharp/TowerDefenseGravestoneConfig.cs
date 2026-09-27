using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefenseGravestoneConfig.cs")]
public class TowerDefenseGravestoneConfig : TowerDefenseCharacterConfig
{
	public new class MethodName : TowerDefenseCharacterConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCharacterConfig.PropertyName
	{
		public static readonly StringName isChests = "isChests";

		public static readonly StringName blocksPlanting = "blocksPlanting";
	}

	public new class SignalName : TowerDefenseCharacterConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool isChests;

	[Export(PropertyHint.None, "")]
	public bool blocksPlanting;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.isChests)
		{
			isChests = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.blocksPlanting)
		{
			blocksPlanting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.isChests)
		{
			value = VariantUtils.CreateFrom(in isChests);
			return true;
		}
		if (name == PropertyName.blocksPlanting)
		{
			value = VariantUtils.CreateFrom(in blocksPlanting);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.isChests, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.blocksPlanting, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.isChests, Variant.From(in isChests));
		info.AddProperty(PropertyName.blocksPlanting, Variant.From(in blocksPlanting));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.isChests, out var value))
		{
			isChests = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.blocksPlanting, out var value2))
		{
			blocksPlanting = value2.As<bool>();
		}
	}
}
