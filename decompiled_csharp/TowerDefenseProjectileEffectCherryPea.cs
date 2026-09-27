using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/CherryPea/TowerDefenseProjectileEffectCherryPea.cs")]
public class TowerDefenseProjectileEffectCherryPea : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateBurstDirections = "CreateBurstDirections";

		public static readonly StringName CreateBurstAngles = "CreateBurstAngles";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
		public new static readonly StringName NetworkEffectId = "NetworkEffectId";

		public static readonly StringName num = "num";
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private static readonly StringName PeaName = new StringName("Pea");

	private static readonly StringName DefaultSkinName = new StringName("Default");

	private static readonly Vector2[] BurstDirections = CreateBurstDirections();

	private static readonly float[] BurstAngles = CreateBurstAngles();

	public int num = 10;

	public override string NetworkEffectId => "cherry_pea_burst";

	public override void _Ready()
	{
		if (PrepareNetworkEffect())
		{
			AttackCreateAsync();
		}
	}

	protected async Task AttackCreateAsync()
	{
		RandomNumberGenerator random = new RandomNumberGenerator
		{
			Seed = NetworkRandomSeed
		};
		try
		{
			TowerDefenseProjectileConfig projectileConfig = BulletField.BuildTemplateConfig(PeaName, DefaultSkinName);
			if (projectileConfig == null)
			{
				GD.PushError("[CherryPea:E_PROJECTILE_CONFIG] Pea/Default template config is unavailable");
				return;
			}
			for (int wave = 0; wave < this.num; wave++)
			{
				if (!FireComponent.TryPrepareProjectileBurstPositionByConfig(null, null, 10.0, GlobalPosition, projectileConfig, collisionFlag, camp, out var context))
				{
					break;
				}
				for (int i = 0; i < BurstDirections.Length; i++)
				{
					BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
					{
						checkAllOverride = true,
						initialRotationOverride = BurstAngles[i]
					};
					float num = random.RandfRange(200f, 800f);
					FireComponent.SpawnPreparedProjectileBurstItem(in context, BurstDirections[i] * num, num, overrides);
				}
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
		}
		finally
		{
			random.Dispose();
			if (GodotObject.IsInstanceValid(this))
			{
				QueueFree();
			}
		}
	}

	private static Vector2[] CreateBurstDirections()
	{
		Vector2[] array = new Vector2[36];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Vector2.FromAngle(Mathf.DegToRad((float)i * 10f));
		}
		return array;
	}

	private static float[] CreateBurstAngles()
	{
		float[] array = new float[36];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Mathf.DegToRad((float)i * 10f);
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBurstDirections, new PropertyInfo(Variant.Type.PackedVector2Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateBurstAngles, new PropertyInfo(Variant.Type.PackedFloat32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBurstDirections && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2[]>(CreateBurstDirections());
			return true;
		}
		if (method == MethodName.CreateBurstAngles && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float[]>(CreateBurstAngles());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateBurstDirections && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2[]>(CreateBurstDirections());
			return true;
		}
		if (method == MethodName.CreateBurstAngles && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float[]>(CreateBurstAngles());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.CreateBurstDirections)
		{
			return true;
		}
		if (method == MethodName.CreateBurstAngles)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.NetworkEffectId)
		{
			value = VariantUtils.CreateFrom<string>(NetworkEffectId);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From(in num));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<int>();
		}
	}
}
