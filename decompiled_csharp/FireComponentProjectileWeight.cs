using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/Resource/Projectile/FireComponentProjectileWeight.cs")]
public class FireComponentProjectileWeight : FireComponentProjectileResource
{
	public new class MethodName : FireComponentProjectileResource.MethodName
	{
		public new static readonly StringName GetProjectile = "GetProjectile";

		public new static readonly StringName GetProjetile = "GetProjetile";

		public static readonly StringName PickUniformProjectile = "PickUniformProjectile";

		public static readonly StringName PickWeightedProjectile = "PickWeightedProjectile";

		public static readonly StringName RefreshProjectile = "RefreshProjectile";
	}

	public new class PropertyName : FireComponentProjectileResource.PropertyName
	{
		public static readonly StringName projectileData = "projectileData";

		public static readonly StringName projectileWeight = "projectileWeight";

		public static readonly StringName projectileSimilarName = "projectileSimilarName";

		public static readonly StringName averageWeight = "averageWeight";

		public static readonly StringName readyProjectile = "readyProjectile";
	}

	public new class SignalName : FireComponentProjectileResource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool averageWeight;

	public TowerDefenseProjectileCreateData readyProjectile;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData projectileData { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<FireComponentProjectileWeightItem> projectileWeight { get; set; } = new Array<FireComponentProjectileWeightItem>();

	public string projectileSimilarName
	{
		get
		{
			if (projectileData == null)
			{
				return "";
			}
			return projectileData.projectileName;
		}
		set
		{
			if (projectileData == null)
			{
				projectileData = new TowerDefenseProjectileCreateData(value);
			}
			else if (!(value == (string?)projectileData.projectileName))
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
		if (projectileWeight == null)
		{
			return;
		}
		for (int i = 0; i < projectileWeight.Count; i++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = projectileWeight[i];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem.projectileResource))
			{
				fireComponentProjectileWeightItem.projectileResource.CollectProjectileData(output, visited);
			}
		}
	}

	public override bool CanFire(FireComponent fireComponent, int collisionFlag)
	{
		if (projectileData != null && fireComponent != null && !fireComponent.IsReleased)
		{
			return fireComponent.CanFire(projectileData, collisionFlag);
		}
		return false;
	}

	public override TowerDefenseProjectileCreateData GetProjectile()
	{
		return GetProjectile(null);
	}

	public override TowerDefenseProjectileCreateData GetProjectile(RandomNumberGenerator random)
	{
		if (projectileWeight == null || projectileWeight.Count == 0)
		{
			return projectileData;
		}
		FireComponentProjectileResource fireComponentProjectileResource = (averageWeight ? PickUniformProjectile(random) : PickWeightedProjectile(random));
		if (!GodotObject.IsInstanceValid(fireComponentProjectileResource))
		{
			return projectileData;
		}
		return fireComponentProjectileResource.GetProjectile();
	}

	public override TowerDefenseProjectileCreateData GetProjetile()
	{
		return GetProjectile();
	}

	private FireComponentProjectileResource PickUniformProjectile(RandomNumberGenerator random)
	{
		int num = 0;
		for (int i = 0; i < projectileWeight.Count; i++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = projectileWeight[i];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem.projectileResource))
			{
				num++;
			}
		}
		if (num == 0)
		{
			return null;
		}
		int num2 = (int)((random?.Randi() ?? GD.Randi()) % (uint)num);
		for (int j = 0; j < projectileWeight.Count; j++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem2 = projectileWeight[j];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2.projectileResource) && num2-- == 0)
			{
				return fireComponentProjectileWeightItem2.projectileResource;
			}
		}
		return null;
	}

	private FireComponentProjectileResource PickWeightedProjectile(RandomNumberGenerator random)
	{
		float num = 0f;
		for (int i = 0; i < projectileWeight.Count; i++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem = projectileWeight[i];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem.projectileResource))
			{
				num += Mathf.Max(0f, fireComponentProjectileWeightItem.weight);
			}
		}
		if (num <= 0f)
		{
			return PickUniformProjectile(random);
		}
		float num2 = (random?.Randf() ?? GD.Randf()) * num;
		FireComponentProjectileResource result = null;
		for (int j = 0; j < projectileWeight.Count; j++)
		{
			FireComponentProjectileWeightItem fireComponentProjectileWeightItem2 = projectileWeight[j];
			if (GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2) && GodotObject.IsInstanceValid(fireComponentProjectileWeightItem2.projectileResource))
			{
				result = fireComponentProjectileWeightItem2.projectileResource;
				num2 -= Mathf.Max(0f, fireComponentProjectileWeightItem2.weight);
				if (num2 <= 0f)
				{
					return fireComponentProjectileWeightItem2.projectileResource;
				}
			}
		}
		return result;
	}

	public void RefreshProjectile()
	{
		readyProjectile = GetProjectile();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "random", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RandomNumberGenerator"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjetile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PickUniformProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "random", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RandomNumberGenerator"), exported: false)
			}, null),
			new MethodInfo(MethodName.PickWeightedProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "random", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RandomNumberGenerator"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.GetProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(GetProjectile(VariantUtils.ConvertTo<RandomNumberGenerator>(in args[0])));
			return true;
		}
		if (method == MethodName.GetProjetile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(GetProjetile());
			return true;
		}
		if (method == MethodName.PickUniformProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<FireComponentProjectileResource>(PickUniformProjectile(VariantUtils.ConvertTo<RandomNumberGenerator>(in args[0])));
			return true;
		}
		if (method == MethodName.PickWeightedProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<FireComponentProjectileResource>(PickWeightedProjectile(VariantUtils.ConvertTo<RandomNumberGenerator>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshProjectile && args.Count == 0)
		{
			RefreshProjectile();
			ret = default;
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
		if (method == MethodName.PickUniformProjectile)
		{
			return true;
		}
		if (method == MethodName.PickWeightedProjectile)
		{
			return true;
		}
		if (method == MethodName.RefreshProjectile)
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
		if (name == PropertyName.projectileWeight)
		{
			projectileWeight = VariantUtils.ConvertToArray<FireComponentProjectileWeightItem>(in value);
			return true;
		}
		if (name == PropertyName.projectileSimilarName)
		{
			projectileSimilarName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.averageWeight)
		{
			averageWeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.readyProjectile)
		{
			readyProjectile = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
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
		if (name == PropertyName.projectileWeight)
		{
			value = VariantUtils.CreateFromArray(projectileWeight);
			return true;
		}
		if (name == PropertyName.projectileSimilarName)
		{
			value = VariantUtils.CreateFrom<string>(projectileSimilarName);
			return true;
		}
		if (name == PropertyName.averageWeight)
		{
			value = VariantUtils.CreateFrom(in averageWeight);
			return true;
		}
		if (name == PropertyName.readyProjectile)
		{
			value = VariantUtils.CreateFrom(in readyProjectile);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.averageWeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.projectileWeight, PropertyHint.TypeString, "24/17:FireComponentProjectileWeightItem", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.readyProjectile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileSimilarName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileData, Variant.From<TowerDefenseProjectileCreateData>(projectileData));
		info.AddProperty(PropertyName.projectileWeight, Variant.CreateFrom(projectileWeight));
		info.AddProperty(PropertyName.projectileSimilarName, Variant.From<string>(projectileSimilarName));
		info.AddProperty(PropertyName.averageWeight, Variant.From(in averageWeight));
		info.AddProperty(PropertyName.readyProjectile, Variant.From(in readyProjectile));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileData, out var value))
		{
			projectileData = value.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.projectileWeight, out var value2))
		{
			projectileWeight = value2.AsGodotArray<FireComponentProjectileWeightItem>();
		}
		if (info.TryGetProperty(PropertyName.projectileSimilarName, out var value3))
		{
			projectileSimilarName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.averageWeight, out var value4))
		{
			averageWeight = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.readyProjectile, out var value5))
		{
			readyProjectile = value5.As<TowerDefenseProjectileCreateData>();
		}
	}
}
