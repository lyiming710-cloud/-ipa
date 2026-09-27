using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/Resource/Projectile/FireComponentProjectileWeightItem.cs")]
public class FireComponentProjectileWeightItem : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName projectileResource = "projectileResource";

		public static readonly StringName weight = "weight";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public FireComponentProjectileResource projectileResource;

	[Export(PropertyHint.None, "")]
	public float weight = 1f;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.projectileResource)
		{
			projectileResource = VariantUtils.ConvertTo<FireComponentProjectileResource>(in value);
			return true;
		}
		if (name == PropertyName.weight)
		{
			weight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.projectileResource)
		{
			value = VariantUtils.CreateFrom(in projectileResource);
			return true;
		}
		if (name == PropertyName.weight)
		{
			value = VariantUtils.CreateFrom(in weight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileResource, PropertyHint.ResourceType, "FireComponentProjectileResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.weight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileResource, Variant.From(in projectileResource));
		info.AddProperty(PropertyName.weight, Variant.From(in weight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileResource, out var value))
		{
			projectileResource = value.As<FireComponentProjectileResource>();
		}
		if (info.TryGetProperty(PropertyName.weight, out var value2))
		{
			weight = value2.As<float>();
		}
	}
}
