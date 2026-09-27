using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/GloomSquid/TowerDefenseProjectileEffectGloomSquid.cs")]
public class TowerDefenseProjectileEffectGloomSquid : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AttackCreate = "AttackCreate";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
		public static readonly StringName num = "num";
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private static PackedScene gloomSquidSplatsScene;

	public int num = 4;

	private static PackedScene GLOOM_SQUID_SPLATS_SCENE => gloomSquidSplatsScene ?? (gloomSquidSplatsScene = GD.Load<PackedScene>("uid://vc2i84jbs3fd"));

	public override void _Ready()
	{
		GetNode<Timer>("Timer").Timeout += AttackCreate;
		AttackCreate();
	}

	public void AttackCreate()
	{
		num--;
		AudioManager.Instance.AudioPlay("SplatNormal");
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = GLOOM_SQUID_SPLATS_SCENE.Instantiate<TowerDefenseEffectParticlesOnce>(PackedScene.GenEditState.Disabled);
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
		if (num <= 0)
		{
			QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
