using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/Event/ShovelEventExplodeShovelConfig.cs")]
public class ShovelEventExplodeShovelConfig : ShovelEventConfig
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

	private static PackedScene _explosionScene;

	private static PackedScene ExplosionScene => _explosionScene ?? (_explosionScene = GD.Load<PackedScene>("uid://cxnt2jbnk48fp"));

	public override void Execute(TowerDefenseCharacter character)
	{
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = null;
		double num = -1.0;
		List<TowerDefenseCharacter> cleanCharactersList = TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList();
		for (int i = 0; i < cleanCharactersList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = cleanCharactersList[i];
			if (towerDefenseCharacter2.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && !towerDefenseCharacter2.die && !towerDefenseCharacter2.nearDie && GodotObject.IsInstanceValid(towerDefenseCharacter2.instance) && !towerDefenseCharacter2.instance.hypnoses && towerDefenseCharacter2.instance.hitpoints > num)
			{
				num = towerDefenseCharacter2.instance.hitpoints;
				towerDefenseCharacter = towerDefenseCharacter2;
			}
		}
		if (towerDefenseCharacter != null)
		{
			double num2 = character.cost * 5.0;
			ViewManager.Instance.CameraShake(new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)), 5.0, 0.05, 4);
			AudioManager.Instance.AudioPlay("ExplodeCherrybomb");
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(ExplosionScene, towerDefenseCharacter.gridPos);
			Vector2 pos = (towerDefenseEffectParticlesOnce.GlobalPosition = towerDefenseCharacter.GetLogicalGlobalPosition());
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, Node.InternalMode.Disabled);
			TowerDefenseCharacterEventHurt item = new TowerDefenseCharacterEventHurt
			{
				num = num2
			};
			Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase> { item };
			TowerDefenseExplode.CreateExplode(pos, new Vector2(0.5f, 0.5f), eventList, new Array<TowerDefenseCharacter>(), TowerDefenseEnum.CHARACTER_CAMP.PLANT, -1);
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
