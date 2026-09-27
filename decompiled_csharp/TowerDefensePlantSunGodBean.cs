using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Diamond/SunGodBean/Scene/TowerDefensePlantSunGodBean.cs")]
public class TowerDefensePlantSunGodBean : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Explode = "Explode";

		public static readonly StringName _GetChangePacketEvent = "_GetChangePacketEvent";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ExplodeComponent _explodeComponent;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= Explode;
		}
	}

	public void Explode()
	{
		bool hypnoses = instance.hypnoses;
		if (hypnoses)
		{
			foreach (Variant item3 in TowerDefenseManager.Instance.GetCampFriendly(camp))
			{
				TowerDefenseCharacter towerDefenseCharacter = item3.As<TowerDefenseCharacter>();
				ProduceComponent runtime = towerDefenseCharacter.componentManager.GetRuntime<ProduceComponent>();
				if (runtime == null || runtime.IsReleased || !(runtime.produceType == "BrainSun"))
				{
					continue;
				}
				if (runtime.marker.Count > 0)
				{
					foreach (Marker2D item4 in runtime.marker)
					{
						if (GodotObject.IsInstanceValid(item4))
						{
							runtime.Create(towerDefenseCharacter.GetLogicalGlobalPosition(item4), runtime.num);
						}
					}
				}
				else
				{
					runtime.Create(towerDefenseCharacter.GetLogicalGlobalPosition(), runtime.num);
				}
			}
		}
		else
		{
			foreach (Variant item5 in TowerDefenseManager.Instance.GetCampFriendly(camp))
			{
				TowerDefenseCharacter towerDefenseCharacter2 = item5.As<TowerDefenseCharacter>();
				towerDefenseCharacter2.WakeUp();
				ProduceComponent runtime2 = towerDefenseCharacter2.componentManager.GetRuntime<ProduceComponent>();
				bool flag = runtime2 != null && !runtime2.IsReleased;
				if (flag)
				{
					bool flag2;
					switch (runtime2.produceType)
					{
					case "Sun":
					case "JalaSun":
					case "QXSun":
						flag2 = true;
						break;
					default:
						flag2 = false;
						break;
					}
					flag = flag2;
				}
				if (flag)
				{
					runtime2.ImmediateProduct();
				}
			}
		}
		Array<TowerDefenseInGamePacketShow> array = new Array<TowerDefenseInGamePacketShow>();
		array.AddRange(TowerDefenseManager.Instance.GetSeedBankList());
		TowerDefenseBattleFeatureConveyorBelt conveyorBeltFeature = TowerDefenseManager.Instance.GetConveyorBeltFeature();
		if (GodotObject.IsInstanceValid(conveyorBeltFeature))
		{
			foreach (Node packetChild in conveyorBeltFeature.GetPacketChildren())
			{
				if (packetChild is TowerDefenseInGamePacketShow item)
				{
					array.Add(item);
				}
			}
		}
		foreach (TowerDefenseInGamePacketShow item6 in array)
		{
			if (((item6.originalSaveKey != "") ? item6.originalSaveKey : item6.config.saveKey) == packet.saveKey || item6.config.saveKey == "PlantCardless")
			{
				continue;
			}
			if (GodotObject.IsInstanceValid(item6.config._override) && item6.config._override.type == TowerDefenseEnum.PACKET_TYPE.GOLD)
			{
				CardActionBehaviorChangePacket cardActionBehaviorChangePacket = _GetChangePacketEvent(item6);
				if (cardActionBehaviorChangePacket != null)
				{
					cardActionBehaviorChangePacket.count++;
				}
				continue;
			}
			TowerDefensePacketConfig towerDefensePacketConfig = (TowerDefensePacketConfig)item6.config.Duplicate(deep: true);
			towerDefensePacketConfig.characterConfig = item6.config.characterConfig;
			towerDefensePacketConfig.coldDownDecreaseDictionary = item6.config.coldDownDecreaseDictionary;
			TowerDefensePacketOverride towerDefensePacketOverride;
			if (GodotObject.IsInstanceValid(item6.config._override))
			{
				towerDefensePacketOverride = (TowerDefensePacketOverride)item6.config._override.Duplicate(deep: true);
				towerDefensePacketOverride.useSucceededActions.Clear();
				if (!GodotObject.IsInstanceValid(towerDefensePacketOverride.characterOverride))
				{
					towerDefensePacketOverride.characterOverride = new TowerDefenseCharacterOverride();
				}
			}
			else
			{
				towerDefensePacketOverride = new TowerDefensePacketOverride();
				towerDefensePacketOverride.characterOverride = new TowerDefenseCharacterOverride();
			}
			TowerDefenseCharacterEventSunCreate towerDefenseCharacterEventSunCreate = new TowerDefenseCharacterEventSunCreate();
			towerDefenseCharacterEventSunCreate.num = 50.0;
			if (hypnoses)
			{
				towerDefenseCharacterEventSunCreate.forceBrainSun = true;
			}
			towerDefensePacketOverride.characterOverride.spawnEvent.Add(towerDefenseCharacterEventSunCreate);
			if (!hypnoses)
			{
				TowerDefenseCharacterEventWakeUp item2 = new TowerDefenseCharacterEventWakeUp();
				towerDefensePacketOverride.characterOverride.spawnEvent.Add(item2);
			}
			towerDefensePacketOverride.type = TowerDefenseEnum.PACKET_TYPE.GOLD;
			item6.Cover(towerDefensePacketConfig, towerDefensePacketOverride, keepColddown: false);
		}
	}

	public CardActionBehaviorChangePacket _GetChangePacketEvent(TowerDefenseInGamePacketShow packetShow)
	{
		if (!GodotObject.IsInstanceValid(packetShow.config._override))
		{
			return null;
		}
		foreach (CardActionBehaviorDefinition useSucceededAction in packetShow.config._override.useSucceededActions)
		{
			if (useSucceededAction is CardActionBehaviorChangePacket result)
			{
				return result;
			}
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetChangePacketEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
			ret = default;
			return true;
		}
		if (method == MethodName._GetChangePacketEvent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CardActionBehaviorChangePacket>(_GetChangePacketEvent(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0])));
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
		if (method == MethodName.Explode)
		{
			return true;
		}
		if (method == MethodName._GetChangePacketEvent)
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
