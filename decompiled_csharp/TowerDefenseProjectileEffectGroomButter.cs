using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/GroomButter/TowerDefenseProjectileEffectGroomButter.cs")]
public class TowerDefenseProjectileEffectGroomButter : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName AttackCreate = "AttackCreate";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
		public new static readonly StringName NetworkEffectId = "NetworkEffectId";

		public static readonly StringName num = "num";

		public static readonly StringName _timer = "_timer";

		public static readonly StringName _random = "_random";

		public static readonly StringName _kernelData = "_kernelData";

		public static readonly StringName _butterData = "_butterData";
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private static PackedScene gloomSplatsScene;

	public int num = 4;

	private Timer _timer;

	private RandomNumberGenerator _random;

	private TowerDefenseProjectileCreateData _kernelData;

	private TowerDefenseProjectileCreateData _butterData;

	private static PackedScene GLOOM_SPLATS_SCENE => gloomSplatsScene ?? (gloomSplatsScene = GD.Load<PackedScene>("uid://djchpnx8iwbpe"));

	public override string NetworkEffectId => "butter_gloom_burst";

	public override void _Ready()
	{
		if (PrepareNetworkEffect())
		{
			_random = new RandomNumberGenerator
			{
				Seed = NetworkRandomSeed
			};
			_kernelData = new TowerDefenseProjectileCreateData(new StringName("Kernal"))
			{
				baseDamage = 20.0
			};
			_butterData = new TowerDefenseProjectileCreateData(new StringName("Butter"))
			{
				baseDamage = 40.0,
				damageFlags = 2
			};
			_timer = GetNode<Timer>("Timer");
			_timer.Timeout += AttackCreate;
			AttackCreate();
		}
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_timer))
		{
			_timer.Timeout -= AttackCreate;
		}
		_random?.Dispose();
		_kernelData?.Dispose();
		_butterData?.Dispose();
	}

	public void AttackCreate()
	{
		this.num--;
		for (int i = 0; i < 8; i++)
		{
			float num = Mathf.DegToRad(45f * (float)i);
			Vector2 vector = Vector2.FromAngle(num);
			TowerDefenseProjectileCreateData projectileData = ((_random.Randf() < 0.08f) ? _butterData : _kernelData);
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				checkAllOverride = true,
				initialRotationOverride = num + (float)Math.PI / 2f
			};
			FireComponent.CreateProjectilePositionByData(null, target, 0.0 - height, GlobalPosition + vector * 20f, vector * 300f, projectileData, collisionFlag, camp, default, overrides);
		}
		AudioManager.Instance.AudioPlay("SplatNormal");
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = GLOOM_SPLATS_SCENE.Instantiate<TowerDefenseEffectParticlesOnce>(PackedScene.GenEditState.Disabled);
		towerDefenseEffectParticlesOnce.objectId = ObjectManagerConfig.OBJECT.NOONE;
		characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		towerDefenseEffectParticlesOnce.Refresh();
		towerDefenseEffectParticlesOnce.gridPos = gridPos;
		if (!GodotObject.IsInstanceValid(target))
		{
			towerDefenseEffectParticlesOnce.GlobalPosition = GlobalPosition - new Vector2(0f, (float)height);
			TowerDefenseExplode.CreateExplode(GlobalPosition - new Vector2(0f, (float)height), new Vector2(1.25f, 1.25f), Eventlist, new Array<TowerDefenseCharacter>(), camp, collisionFlag);
		}
		else
		{
			Vector2 pos = (towerDefenseEffectParticlesOnce.GlobalPosition = target.GetLogicalGlobalPosition(target.transformPoint) - new Vector2(0f, 50f));
			TowerDefenseExplode.CreateExplode(pos, new Vector2(1.25f, 1.25f), Eventlist, new Array<TowerDefenseCharacter>(), camp, collisionFlag);
		}
		if (this.num <= 0)
		{
			QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.AttackCreate && args.Count == 0)
		{
			AttackCreate();
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
		if (method == MethodName.AttackCreate)
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
		if (name == PropertyName._timer)
		{
			_timer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._random)
		{
			_random = VariantUtils.ConvertTo<RandomNumberGenerator>(in value);
			return true;
		}
		if (name == PropertyName._kernelData)
		{
			_kernelData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName._butterData)
		{
			_butterData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
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
		if (name == PropertyName._timer)
		{
			value = VariantUtils.CreateFrom(in _timer);
			return true;
		}
		if (name == PropertyName._random)
		{
			value = VariantUtils.CreateFrom(in _random);
			return true;
		}
		if (name == PropertyName._kernelData)
		{
			value = VariantUtils.CreateFrom(in _kernelData);
			return true;
		}
		if (name == PropertyName._butterData)
		{
			value = VariantUtils.CreateFrom(in _butterData);
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
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._random, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._kernelData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._butterData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName._timer, Variant.From(in _timer));
		info.AddProperty(PropertyName._random, Variant.From(in _random));
		info.AddProperty(PropertyName._kernelData, Variant.From(in _kernelData));
		info.AddProperty(PropertyName._butterData, Variant.From(in _butterData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._timer, out var value2))
		{
			_timer = value2.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._random, out var value3))
		{
			_random = value3.As<RandomNumberGenerator>();
		}
		if (info.TryGetProperty(PropertyName._kernelData, out var value4))
		{
			_kernelData = value4.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName._butterData, out var value5))
		{
			_butterData = value5.As<TowerDefenseProjectileCreateData>();
		}
	}
}
