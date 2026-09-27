using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/Event/ShovelEventCaptainShovelConfig.cs")]
public class ShovelEventCaptainShovelConfig : ShovelEventConfig
{
	public new class MethodName : ShovelEventConfig.MethodName
	{
		public new static readonly StringName Execute = "Execute";
	}

	public new class PropertyName : ShovelEventConfig.PropertyName
	{
	}

	public new class SignalName : ShovelEventConfig.SignalName
	{
	}

	public override void Execute(TowerDefenseCharacter character)
	{
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return;
		}
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		double groundHeight = character.GetGroundHeight(logicalGlobalPosition.Y);
		if (character.cost < 500.0)
		{
			for (int i = 0; i < (int)Mathf.Floor(character.cost / 100.0); i++)
			{
				double num = GD.RandRange(-10.0, 40.0);
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData(new StringName("CoinSilver"));
				towerDefenseProjectileCreateData.baseDamage = 100.0;
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					gridYOverride = character.gridPos.Y
				};
				FireComponent.CreateProjectilePositionByData(character, null, groundHeight + num - 20.0, logicalGlobalPosition + new Vector2(GD.RandRange(-10, 10), -20f), new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, character.camp, default, overrides);
			}
			Array<int> array = new Array<int>();
			for (int j = character.gridPos.Y - 1; j <= character.gridPos.Y + 1; j++)
			{
				if (j >= 1 && j <= TowerDefenseManager.Instance.GetMapGridNum().Y)
				{
					array.Add(j);
				}
				else
				{
					array.Add(character.gridPos.Y);
				}
			}
			for (int k = 0; k < (int)Mathf.Floor(character.cost / 100.0); k++)
			{
				double num2 = (double)logicalGlobalPosition.X + GD.RandRange(-1.5, 1.5) * (double)TowerDefenseManager.Instance.GetMapGridSize().X;
				TowerDefenseManager.Instance.BungiSpawn("ZombieCrew", new Vector2I(TowerDefenseManager.Instance.GetMapGridPos(new Vector2((float)num2, 0f)).X, array.PickRandom()), null, hypnoses: true);
			}
			return;
		}
		for (int l = 0; l < (int)Mathf.Floor(character.cost / 300.0); l++)
		{
			double num3 = GD.RandRange(-10.0, 40.0);
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData2 = new TowerDefenseProjectileCreateData(new StringName("CoinGold"));
			towerDefenseProjectileCreateData2.baseDamage = 500.0;
			BulletFieldSpawnOverrides overrides2 = new BulletFieldSpawnOverrides
			{
				gridYOverride = character.gridPos.Y
			};
			FireComponent.CreateProjectilePositionByData(character, null, groundHeight + num3 - 20.0, logicalGlobalPosition + new Vector2(GD.RandRange(-10, 10), -20f), new Vector2(300f, 0f), towerDefenseProjectileCreateData2, -1, character.camp, default, overrides2);
		}
		Array<int> array2 = new Array<int>();
		for (int m = character.gridPos.Y - 1; m <= character.gridPos.Y + 1; m++)
		{
			if (m >= 1 && m <= TowerDefenseManager.Instance.GetMapGridNum().Y)
			{
				array2.Add(m);
			}
			else
			{
				array2.Add(character.gridPos.Y);
			}
		}
		for (int n = 0; n < (int)Mathf.Floor(character.cost / 500.0); n++)
		{
			double num4 = (double)logicalGlobalPosition.X + GD.RandRange(-1.5, 1.5) * (double)TowerDefenseManager.Instance.GetMapGridSize().X;
			TowerDefenseManager.Instance.BungiSpawn("ZombieCaptain", new Vector2I(TowerDefenseManager.Instance.GetMapGridPos(new Vector2((float)num4, 0f)).X, array2.PickRandom()), null, hypnoses: true);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
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
