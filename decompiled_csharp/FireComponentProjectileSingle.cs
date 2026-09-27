using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/Resource/Projectile/FireComponentProjectileSingle.cs")]
public class FireComponentProjectileSingle : FireComponentProjectileResource
{
	public new class MethodName : FireComponentProjectileResource.MethodName
	{
		public new static readonly StringName GetProjectile = "GetProjectile";

		public new static readonly StringName GetProjetile = "GetProjetile";
	}

	public new class PropertyName : FireComponentProjectileResource.PropertyName
	{
		public static readonly StringName projectileData = "projectileData";

		public static readonly StringName projectileName = "projectileName";
	}

	public new class SignalName : FireComponentProjectileResource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData projectileData { get; set; }

	public string projectileName
	{
		get
		{
			if (projectileData != null)
			{
				return projectileData.projectileName;
			}
			return "";
		}
		set
		{
			if (projectileData == null)
			{
				projectileData = new TowerDefenseProjectileCreateData(value);
			}
			else if (value != (string?)projectileData.projectileName)
			{
				projectileData.projectileName = value;
				projectileData.baseDamage = -1.0;
				projectileData.InvalidateConfigCache();
			}
		}
	}

	protected override void CollectProjectileDataCore(List<TowerDefenseProjectileCreateData> output, HashSet<FireComponentProjectileResource> visited)
	{
		if (projectileData != null)
		{
			output.Add(projectileData);
		}
	}

	public override bool CanFire(FireComponent fireComponent, int collisionFlag)
	{
		if (projectileData == null)
		{
			return false;
		}
		return fireComponent.CanFire(projectileData, collisionFlag);
	}

	public override TowerDefenseProjectileCreateData GetProjectile()
	{
		return projectileData;
	}

	public override TowerDefenseProjectileCreateData GetProjetile()
	{
		return GetProjectile();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjetile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetProjectile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(GetProjectile());
			return true;
		}
		if (method == MethodName.GetProjetile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(GetProjetile());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetProjectile)
		{
			return true;
		}
		if (method == MethodName.GetProjetile)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.projectileData)
		{
			projectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.projectileData)
		{
			value = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(projectileData);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
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
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileData, Variant.From<TowerDefenseProjectileCreateData>(projectileData));
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileData, out var value))
		{
			projectileData = value.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value2))
		{
			projectileName = value2.As<string>();
		}
	}
}
