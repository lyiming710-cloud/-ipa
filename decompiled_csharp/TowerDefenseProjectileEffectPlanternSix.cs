using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/PlanternSix/TowerDefenseProjectileEffectPlanternSix.cs")]
public class TowerDefenseProjectileEffectPlanternSix : TowerDefenseProjectileEffectBase
{
	private struct StructBullet
	{
		public int index;

		public double timer;

		public bool recallStarted;

		public float recallDirRad;

		public Vector2 recallPos;

		public bool origRotateFollowVelocity;
	}

	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName UpdateOrbitingBullets = "UpdateOrbitingBullets";

		public static readonly StringName GetOrbitPosition = "GetOrbitPosition";

		public static readonly StringName PlaceOrbitingBulletImmediately = "PlaceOrbitingBulletImmediately";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
		public new static readonly StringName NetworkEffectId = "NetworkEffectId";

		public static readonly StringName _projectileDataByDirection = "_projectileDataByDirection";

		public static readonly StringName timer = "timer";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private readonly List<StructBullet> structBullets = new List<StructBullet>();

	private TowerDefenseProjectileCreateData[] _projectileDataByDirection;

	public double timer;

	public bool over;

	public override string NetworkEffectId => "plantern_six_burst";

	public override void _Ready()
	{
		if (PrepareNetworkEffect())
		{
			_projectileDataByDirection = new TowerDefenseProjectileCreateData[6]
			{
				new TowerDefenseProjectileCreateData(new StringName("Pea")),
				new TowerDefenseProjectileCreateData(new StringName("FirePea")),
				new TowerDefenseProjectileCreateData(new StringName("SnowPea")),
				new TowerDefenseProjectileCreateData(new StringName("Pea")),
				new TowerDefenseProjectileCreateData(new StringName("FirePea")),
				new TowerDefenseProjectileCreateData(new StringName("SnowPea"))
			};
			AttackCreateAsync();
		}
	}

	public override void _ExitTree()
	{
		BulletField instance = BulletField.Instance;
		for (int i = 0; i < structBullets.Count; i++)
		{
			StructBullet structBullet = structBullets[i];
			if (instance != null && instance.IsBulletActive(structBullet.index))
			{
				ref BulletData bulletDataRef = ref instance.GetBulletDataRef(structBullet.index);
				bulletDataRef.externalControlled = false;
				bulletDataRef.rotateFollowVelocity = structBullet.origRotateFollowVelocity;
			}
		}
		structBullets.Clear();
		if (_projectileDataByDirection != null)
		{
			TowerDefenseProjectileCreateData[] projectileDataByDirection = _projectileDataByDirection;
			for (int j = 0; j < projectileDataByDirection.Length; j++)
			{
				projectileDataByDirection[j]?.Dispose();
			}
			_projectileDataByDirection = null;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		timer += delta;
		if (structBullets.Count > 0)
		{
			BulletField instance = BulletField.Instance;
			for (int num = structBullets.Count - 1; num >= 0; num--)
			{
				StructBullet value = structBullets[num];
				if (instance == null || !instance.IsBulletActive(value.index))
				{
					structBullets.RemoveAt(num);
				}
				else
				{
					ref BulletData bulletDataRef = ref instance.GetBulletDataRef(value.index);
					if (value.recallStarted)
					{
						if (!instance.IsBulletTweening(value.index))
						{
							bulletDataRef.vel = Vector2.FromAngle(value.recallDirRad) * 300f;
							bulletDataRef.speed = 300f;
							bulletDataRef.rotation = value.recallDirRad;
							bulletDataRef.rotateFollowVelocity = value.origRotateFollowVelocity;
							bulletDataRef.externalControlled = false;
							structBullets.RemoveAt(num);
						}
					}
					else
					{
						value.timer += delta;
						if (value.timer >= 2.0)
						{
							value.recallStarted = true;
							instance.StartRuntimeTween(value.index, value.recallPos, 0.1f, Tween.EaseType.Out, Tween.TransitionType.Quart);
						}
						structBullets[num] = value;
					}
				}
			}
			UpdateOrbitingBullets(instance, delta);
		}
		if (over && structBullets.Count == 0)
		{
			QueueFree();
		}
	}

	private void UpdateOrbitingBullets(BulletField bulletField, double delta)
	{
		int count = structBullets.Count;
		for (int i = 0; i < count; i++)
		{
			StructBullet structBullet = structBullets[i];
			if (bulletField != null && bulletField.IsBulletActive(structBullet.index))
			{
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(structBullet.index);
				if (structBullet.recallStarted)
				{
					bulletDataRef.rotation = (float)Mathf.LerpAngle(bulletDataRef.rotation, structBullet.recallDirRad, 10.0 * delta);
					continue;
				}
				Vector2 orbitPosition = GetOrbitPosition(i, count);
				bulletDataRef.rotation = (float)Mathf.LerpAngle(bulletDataRef.rotation, (orbitPosition - bulletDataRef.pos).Angle(), 5.0 * delta);
				bulletDataRef.pos = bulletDataRef.pos.Lerp(orbitPosition, (float)(5.0 * delta));
			}
		}
	}

	private Vector2 GetOrbitPosition(int index, int total)
	{
		int num = total / 2;
		bool flag = index < num;
		int num2 = (flag ? num : (total - num));
		int num3 = (flag ? index : (index - num));
		double s = (double)((float)Math.PI * 2f / (float)num2 * (float)num3) + (flag ? (timer * 5.0) : timer);
		double num4 = (flag ? 30.0 : 60.0);
		return GlobalPosition + new Vector2((float)(Mathf.Cos(s) * num4), (float)(Mathf.Sin(s) * num4));
	}

	private void PlaceOrbitingBulletImmediately(BulletField bulletField, int trackedIndex)
	{
		int count = structBullets.Count;
		if (trackedIndex >= 0 && trackedIndex < count && bulletField != null)
		{
			StructBullet structBullet = structBullets[trackedIndex];
			if (!structBullet.recallStarted && bulletField.IsBulletActive(structBullet.index))
			{
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(structBullet.index);
				bulletDataRef.rotation = ((bulletDataRef.pos = GetOrbitPosition(trackedIndex, count)) - GlobalPosition).Angle();
			}
		}
	}

	private async Task AttackCreateAsync()
	{
		Vector2 startPos = GlobalPosition;
		for (int wave = 0; wave < 10; wave++)
		{
			for (int i = 0; i < 6; i++)
			{
				double num = i switch
				{
					0 => 60.0, 
					1 => 0.0, 
					2 => -60.0, 
					3 => 120.0, 
					4 => 180.0, 
					_ => 240.0, 
				};
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					checkAllOverride = true,
					gridYOverride = gridPos.Y
				};
				int num2 = BulletField.Instance?.ActiveCount ?? 0;
				FireComponent.CreateProjectilePositionByData(null, null, 30.0, GlobalPosition, Vector2.Zero, _projectileDataByDirection[i], collisionFlag, camp, default, overrides);
				BulletField instance = BulletField.Instance;
				if (instance == null || instance.ActiveCount <= num2)
				{
					continue;
				}
				int lastSpawnedIndex = instance.LastSpawnedIndex;
				if (lastSpawnedIndex >= 0 && instance.IsBulletActive(lastSpawnedIndex))
				{
					ref BulletData bulletDataRef = ref instance.GetBulletDataRef(lastSpawnedIndex);
					if (bulletDataRef.trackOpen)
					{
						bulletDataRef.speed = 300f;
						continue;
					}
					bool rotateFollowVelocity = bulletDataRef.rotateFollowVelocity;
					bulletDataRef.externalControlled = true;
					bulletDataRef.rotateFollowVelocity = false;
					structBullets.Add(new StructBullet
					{
						index = lastSpawnedIndex,
						timer = 0.0,
						recallStarted = false,
						recallDirRad = Mathf.DegToRad((float)num),
						recallPos = startPos,
						origRotateFollowVelocity = rotateFollowVelocity
					});
				}
			}
			BulletField instance2 = BulletField.Instance;
			for (int j = 0; j < structBullets.Count; j++)
			{
				PlaceOrbitingBulletImmediately(instance2, j);
			}
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		over = true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateOrbitingBullets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOrbitPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "total", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlaceOrbitingBulletImmediately, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "trackedIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateOrbitingBullets && args.Count == 2)
		{
			UpdateOrbitingBullets(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOrbitPosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetOrbitPosition(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.PlaceOrbitingBulletImmediately && args.Count == 2)
		{
			PlaceOrbitingBulletImmediately(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.UpdateOrbitingBullets)
		{
			return true;
		}
		if (method == MethodName.GetOrbitPosition)
		{
			return true;
		}
		if (method == MethodName.PlaceOrbitingBulletImmediately)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._projectileDataByDirection)
		{
			_projectileDataByDirection = VariantUtils.ConvertToSystemArrayOfGodotObject<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._projectileDataByDirection)
		{
			GodotObject[] projectileDataByDirection = _projectileDataByDirection;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(projectileDataByDirection);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._projectileDataByDirection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		StringName projectileDataByDirection = PropertyName._projectileDataByDirection;
		GodotObject[] projectileDataByDirection2 = _projectileDataByDirection;
		info.AddProperty(projectileDataByDirection, Variant.CreateFrom(projectileDataByDirection2));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._projectileDataByDirection, out var value))
		{
			_projectileDataByDirection = value.AsGodotObjectArray<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value2))
		{
			timer = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
	}
}
