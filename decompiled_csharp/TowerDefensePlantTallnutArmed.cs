using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/TallnutArmed/Scene/TowerDefensePlantTallnutArmed.cs")]
public class TowerDefensePlantTallnutArmed : TowerDefensePlant, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName ApplyStageHitpointsOnce = "ApplyStageHitpointsOnce";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public static readonly StringName ApplyNetworkStage = "ApplyNetworkStage";

		public static readonly StringName GetNetworkStage = "GetNetworkStage";

		public static readonly StringName ApplySyncedStageVisual = "ApplySyncedStageVisual";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName eventList = "eventList";

		public static readonly StringName dieNum = "dieNum";

		public static readonly StringName _stageHitpointsApplied = "_stageHitpointsApplied";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _TALLNUT_ARMED;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	public int dieNum;

	private bool _stageHitpointsApplied;

	private static PackedScene TALLNUT_ARMED => _TALLNUT_ARMED ?? (_TALLNUT_ARMED = GD.Load<PackedScene>("uid://dw1omhgw0ymw6"));

	public override void IdleEntered()
	{
		base.IdleEntered();
		ApplyStageHitpointsOnce();
		switch (dieNum)
		{
		case 0:
			sprite.SetAnimation("IdleA");
			break;
		case 1:
			sprite.SetAnimation("CrakedA", loop: false);
			sprite.AddAnimation("IdleB", 0.0);
			break;
		case 2:
			sprite.SetAnimation("CrakedB", loop: false);
			sprite.AddAnimation("IdleC", 0.0);
			break;
		case 3:
			sprite.SetAnimation("CrakedC", loop: false);
			sprite.AddAnimation("IdleD", 0.0);
			break;
		}
	}

	private void ApplyStageHitpointsOnce()
	{
		if (!_stageHitpointsApplied)
		{
			_stageHitpointsApplied = true;
			switch (dieNum)
			{
			case 1:
				instance.hitpointsSave -= 2000.0;
				instance.hitpoints -= 2000.0;
				break;
			case 2:
				instance.hitpointsSave -= 4000.0;
				instance.hitpoints -= 4000.0;
				break;
			case 3:
				instance.hitpointsSave -= 6000.0;
				instance.hitpoints -= 6000.0;
				break;
			}
		}
	}

	public override void DestroySet()
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		base.DestroySet();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			TowerDefenseExplode.CreateExplode(logicalGlobalPosition, new Vector2(1.25f, 0.25f), eventList, new Array<TowerDefenseCharacter>(), camp, -1);
		}
		if (dieNum == 3)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(TALLNUT_ARMED, gridPos, "CrakedD");
			towerDefenseEffectSpriteOnce.GlobalPosition = logicalGlobalPosition + new Vector2(0f, (float)groundHeight);
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			if (!TowerDefenseManager.HasGameplayAuthority)
			{
				return;
			}
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantTallnutArmed");
			if (!GodotObject.IsInstanceValid(packetConfig))
			{
				return;
			}
			TowerDefensePlant towerDefensePlant = (TowerDefensePlant)(HasEconomyOwner ? packetConfig.Plant(EconomyOwnerAccountId, gridPos, playAudio: true, noLimit: true, skipPlacementCheck: true) : packetConfig.Plant(gridPos, playAudio: true, noLimit: true, default, skipPlacementCheck: true));
			if (GodotObject.IsInstanceValid(towerDefensePlant))
			{
				((TowerDefensePlantTallnutArmed)towerDefensePlant).dieNum = dieNum + 1;
				if (instance.hypnoses)
				{
					towerDefensePlant.Hypnoses();
				}
				Dictionary spawnState = new Dictionary { ["dieNum"] = dieNum + 1 };
				TowerDefenseManager.PublishSpawnedCharacter("PlantTallnutArmed", towerDefensePlant, useCreate: false, 0.0, walkAfterSpawn: false, "", spawnState);
			}
		}
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		int num = dieNum;
		ApplyNetworkStage(data);
		if (num != dieNum)
		{
			_stageHitpointsApplied = false;
		}
		if (IsNodeReady())
		{
			ApplyStageHitpointsOnce();
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary { ["dieNum"] = dieNum };
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ApplyNetworkStage(data);
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return dieNum;
	}

	private void ApplyNetworkStage(Dictionary data)
	{
		if (data != null)
		{
			int networkStage = GetNetworkStage(data);
			bool flag = networkStage != dieNum;
			dieNum = networkStage;
			if (flag && IsNodeReady() && GodotObject.IsInstanceValid(sprite))
			{
				ApplySyncedStageVisual();
			}
		}
	}

	private int GetNetworkStage(Dictionary data)
	{
		if (data != null)
		{
			return Math.Clamp(data.GetValueOrDefault("dieNum", dieNum).AsInt32(), 0, 3);
		}
		return dieNum;
	}

	private void ApplySyncedStageVisual()
	{
		switch (dieNum)
		{
		case 0:
			sprite.SetAnimation("IdleA");
			break;
		case 1:
			sprite.SetAnimation("IdleB");
			break;
		case 2:
			sprite.SetAnimation("IdleC");
			break;
		case 3:
			sprite.SetAnimation("IdleD");
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "dieNum", dieNum } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		dieNum = Math.Clamp(data.GetValueOrDefault("dieNum", Variant.From<int>(0)).AsInt32(), 0, 3);
		_stageHitpointsApplied = true;
		if (IsNodeReady() && GodotObject.IsInstanceValid(sprite))
		{
			ApplySyncedStageVisual();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyStageHitpointsOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyNetworkStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkStage, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySyncedStageVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyStageHitpointsOnce && args.Count == 0)
		{
			ApplyStageHitpointsOnce();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
			return true;
		}
		if (method == MethodName.ApplyNetworkStage && args.Count == 1)
		{
			ApplyNetworkStage(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkStage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkStage(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplySyncedStageVisual && args.Count == 0)
		{
			ApplySyncedStageVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.ApplyStageHitpointsOnce)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkStage)
		{
			return true;
		}
		if (method == MethodName.GetNetworkStage)
		{
			return true;
		}
		if (method == MethodName.ApplySyncedStageVisual)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.dieNum)
		{
			dieNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._stageHitpointsApplied)
		{
			_stageHitpointsApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.dieNum)
		{
			value = VariantUtils.CreateFrom(in dieNum);
			return true;
		}
		if (name == PropertyName._stageHitpointsApplied)
		{
			value = VariantUtils.CreateFrom(in _stageHitpointsApplied);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.dieNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stageHitpointsApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.dieNum, Variant.From(in dieNum));
		info.AddProperty(PropertyName._stageHitpointsApplied, Variant.From(in _stageHitpointsApplied));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventList, out var value))
		{
			eventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.dieNum, out var value2))
		{
			dieNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._stageHitpointsApplied, out var value3))
		{
			_stageHitpointsApplied = value3.As<bool>();
		}
	}
}
