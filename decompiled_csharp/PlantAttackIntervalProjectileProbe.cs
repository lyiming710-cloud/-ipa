using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class PlantAttackIntervalProjectileProbe : FireComponentProjectileSingle
{
	public new class MethodName : FireComponentProjectileSingle.MethodName
	{
	}

	public new class PropertyName : FireComponentProjectileSingle.PropertyName
	{
		public static readonly StringName Checks = "Checks";

		public static readonly StringName Available = "Available";
	}

	public new class SignalName : FireComponentProjectileSingle.SignalName
	{
	}

	public int Checks { get; private set; }

	public bool Available { get; set; }

	public override bool CanFire(FireComponent fireComponent, int collisionFlag)
	{
		Checks++;
		return Available;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Checks)
		{
			Checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Available)
		{
			Available = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Checks)
		{
			value = VariantUtils.CreateFrom<int>(Checks);
			return true;
		}
		if (name == PropertyName.Available)
		{
			value = VariantUtils.CreateFrom<bool>(Available);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.Checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Available, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Checks, Variant.From<int>(Checks));
		info.AddProperty(PropertyName.Available, Variant.From<bool>(Available));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Checks, out var value))
		{
			Checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Available, out var value2))
		{
			Available = value2.As<bool>();
		}
	}
}
