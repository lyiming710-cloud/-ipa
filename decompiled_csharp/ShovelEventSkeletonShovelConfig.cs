using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/Event/ShovelEventSkeletonShovelConfig.cs")]
public class ShovelEventSkeletonShovelConfig : ShovelEventConfig
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

	private static PackedScene _rivive;

	private static PackedScene RIVIVE => _rivive ?? (_rivive = GD.Load<PackedScene>("uid://dbgw1lmiiyypp"));

	public override void Execute(TowerDefenseCharacter character)
	{
		if ((Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost) || !(character.cost >= 50.0) || !TowerDefenseManager.TryTakeLatestDeathRecord(TowerDefenseEnum.CHARACTER_CAMP.PLANT, requireAngelEligible: false, out var record))
		{
			return;
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(RIVIVE, record.GridPosition);
		towerDefenseEffectSpriteOnce.GlobalPosition = record.Position;
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, Node.InternalMode.Disabled);
		TowerDefensePacketConfig packet = record.Packet;
		TowerDefenseCharacter towerDefenseCharacter = packet.Create(record.Position, record.GridPosition);
		towerDefenseCharacter.invisible = record.Invisible;
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter.transformPoint))
		{
			towerDefenseCharacter.transformPoint.Scale = (float)record.Scale * Vector2.One;
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacter.instance))
		{
			towerDefenseCharacter.instance.hitpointScale = record.HitpointScale;
		}
		towerDefenseCharacter.Hypnoses();
		if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, character);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packet.saveKey, record.GridPosition.X, record.GridPosition.Y, nextSyncId, record.HitpointScale, record.Scale, hypnoses: true, 0.0, useCreate: true, record.Position.X, record.Position.Y, walkAfterSpawn: true);
			}
		}
		towerDefenseCharacter.CallDeferred("Walk");
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
