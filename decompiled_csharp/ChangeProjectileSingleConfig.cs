using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Projectile/Change/ChangeProjectileSingleConfig.cs")]
public class ChangeProjectileSingleConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName projectileData = "projectileData";

		public static readonly StringName projectileConfig = "projectileConfig";

		public static readonly StringName changeAudio = "changeAudio";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData projectileData;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileConfig projectileConfig;

	[Export(PropertyHint.None, "")]
	public string changeAudio;

	public string projectileName
	{
		get
		{
			if (GodotObject.IsInstanceValid(projectileData))
			{
				return projectileData.projectileName.ToString();
			}
			return "";
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.projectileData)
		{
			projectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.projectileConfig)
		{
			projectileConfig = VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName.changeAudio)
		{
			changeAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName.projectileData)
		{
			value = VariantUtils.CreateFrom(in projectileData);
			return true;
		}
		if (name == PropertyName.projectileConfig)
		{
			value = VariantUtils.CreateFrom(in projectileConfig);
			return true;
		}
		if (name == PropertyName.changeAudio)
		{
			value = VariantUtils.CreateFrom(in changeAudio);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileData, PropertyHint.ResourceType, "TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileConfig, PropertyHint.ResourceType, "TowerDefenseProjectileConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.changeAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileData, Variant.From(in projectileData));
		info.AddProperty(PropertyName.projectileConfig, Variant.From(in projectileConfig));
		info.AddProperty(PropertyName.changeAudio, Variant.From(in changeAudio));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileData, out var value))
		{
			projectileData = value.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.projectileConfig, out var value2))
		{
			projectileConfig = value2.As<TowerDefenseProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName.changeAudio, out var value3))
		{
			changeAudio = value3.As<string>();
		}
	}
}
