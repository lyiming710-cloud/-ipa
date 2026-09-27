using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/Resource/Projectile/FireComponentProjectileResource.cs")]
public class FireComponentProjectileResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetProjectile = "GetProjectile";

		public static readonly StringName GetProjetile = "GetProjetile";
	}

	public new class PropertyName : Resource.PropertyName
	{
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public void CollectProjectileData(List<TowerDefenseProjectileCreateData> output, HashSet<FireComponentProjectileResource> visited)
	{
		if (output != null && visited != null && visited.Add(this))
		{
			CollectProjectileDataCore(output, visited);
		}
	}

	protected virtual void CollectProjectileDataCore(List<TowerDefenseProjectileCreateData> output, HashSet<FireComponentProjectileResource> visited)
	{
	}

	public virtual bool CanFire(FireComponent fireComponent, int collisionFlag)
	{
		return false;
	}

	public virtual TowerDefenseProjectileCreateData GetProjectile()
	{
		return null;
	}

	public virtual TowerDefenseProjectileCreateData GetProjectile(RandomNumberGenerator random)
	{
		return GetProjectile();
	}

	public virtual TowerDefenseProjectileCreateData GetProjetile()
	{
		return GetProjectile();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "random", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RandomNumberGenerator"), exported: false)
			}, null),
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
