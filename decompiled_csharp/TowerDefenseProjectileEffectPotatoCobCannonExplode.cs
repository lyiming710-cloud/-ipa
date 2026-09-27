using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/PotatoCobCannonExplode/TowerDefenseProjectileEffectPotatoCobCannonExplode.cs")]
public class TowerDefenseProjectileEffectPotatoCobCannonExplode : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName EffectCreate = "EffectCreate";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private static PackedScene mineExplosion;

	private static PackedScene MINE_EXPLOSION => mineExplosion ?? (mineExplosion = GD.Load<PackedScene>("uid://dqcu2ycvf4u72"));

	public override async void _Ready()
	{
		if (GodotObject.IsInstanceValid(target))
		{
			EffectCreate();
			TowerDefenseExplode.CreateExplode(GlobalPosition, new Vector2(1f, 0.2f), Eventlist, new Array<TowerDefenseCharacter>(), camp, -1);
			if (camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
			{
				TowerDefenseManager.Instance.BrainSunCreate(GlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
			}
			else
			{
				TowerDefenseManager.Instance.SunCreate(GlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
			}
		}
		else
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantSunMine");
			if (GodotObject.IsInstanceValid(mapCell))
			{
				if (mapCell.CanPacketPlant(packetConfig))
				{
					TowerDefenseCharacter sunMine = packetConfig.Plant(gridPos);
					if (camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
					{
						sunMine.Hypnoses();
					}
					await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
					if (sunMine is TowerDefensePlantSunMine towerDefensePlantSunMine)
					{
						towerDefensePlantSunMine.ReadyRise();
					}
					else if (sunMine is TowerDefensePlantMagnetMine towerDefensePlantMagnetMine)
					{
						towerDefensePlantMagnetMine.ReadyRise();
					}
				}
				else
				{
					if (camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
					{
						TowerDefenseManager.Instance.BrainSunCreate(GlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
					}
					else
					{
						TowerDefenseManager.Instance.SunCreate(GlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
					}
					EffectCreate();
				}
			}
			else
			{
				if (camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
				{
					TowerDefenseManager.Instance.BrainSunCreate(GlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
				}
				else
				{
					TowerDefenseManager.Instance.SunCreate(GlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 0.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
				}
				EffectCreate();
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		QueueFree();
	}

	public void EffectCreate()
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		ViewManager.Instance.CameraShake(new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)), 3.0, 0.05, 4);
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(MINE_EXPLOSION, gridPos);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		towerDefenseEffectParticlesOnce.GlobalPosition = GlobalPosition;
		if (GodotObject.IsInstanceValid(mapCell))
		{
			towerDefenseEffectParticlesOnce.GlobalPosition = new Vector2(towerDefenseEffectParticlesOnce.GlobalPosition.X, towerDefenseEffectParticlesOnce.GlobalPosition.Y - (float)(mapCell.GetGroundHeight() - 30.0));
		}
		characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		AudioManager.Instance.AudioPlay("MineExplosion");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EffectCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.EffectCreate && args.Count == 0)
		{
			EffectCreate();
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
		if (method == MethodName.EffectCreate)
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
