using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/GarCobCannonExplode/TowerDefenseProjectileEffectGarCobCannonExplode.cs")]
public class TowerDefenseProjectileEffectGarCobCannonExplode : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SelectGiantZombie = "SelectGiantZombie";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	private static readonly (string packetName, int weight)[] GiantZombies = new (string, int)[5]
	{
		("ZombieGargantuar", 60),
		("ZombieGargantuarRedEyes", 20),
		("ZombieFootballGargantuar", 13),
		("ZombieFootballGargantuarBlack", 5),
		("ZombieDiscoGargantuar", 2)
	};

	public override async void _Ready()
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("VaseZombie");
		if (GodotObject.IsInstanceValid(mapCell) && GodotObject.IsInstanceValid(packetConfig))
		{
			TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(SelectGiantZombie());
			if (GodotObject.IsInstanceValid(packetConfig2))
			{
				TowerDefensePacketConfig hypnotizedConfig = packetConfig2.Duplicate(deep: true) as TowerDefensePacketConfig;
				if (GodotObject.IsInstanceValid(hypnotizedConfig))
				{
					hypnotizedConfig.coldDownDecreaseDictionary = new Dictionary();
					hypnotizedConfig.overrideHypnoses = true;
					TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos, playAudio: true, noLimit: true, default, skipPlacementCheck: true);
					if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter is TowerDefenseVase zombieVase)
					{
						await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
						if (GodotObject.IsInstanceValid(zombieVase))
						{
							zombieVase.SetContentConfig(hypnotizedConfig);
						}
					}
				}
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		QueueFree();
	}

	private string SelectGiantZombie()
	{
		int num = 0;
		(string, int)[] giantZombies = GiantZombies;
		for (int i = 0; i < giantZombies.Length; i++)
		{
			int item = giantZombies[i].Item2;
			num += item;
		}
		int num2 = GD.RandRange(0, num - 1);
		int num3 = 0;
		giantZombies = GiantZombies;
		for (int i = 0; i < giantZombies.Length; i++)
		{
			(string, int) tuple = giantZombies[i];
			string item2 = tuple.Item1;
			int item3 = tuple.Item2;
			num3 += item3;
			if (num2 < num3)
			{
				return item2;
			}
		}
		return GiantZombies[0].packetName;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectGiantZombie, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SelectGiantZombie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(SelectGiantZombie());
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
		if (method == MethodName.SelectGiantZombie)
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
