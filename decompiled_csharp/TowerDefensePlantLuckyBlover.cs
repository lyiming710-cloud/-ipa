using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Diamond/LuckyBlover/Scene/TowerDefensePlantLuckyBlover.cs")]
public class TowerDefensePlantLuckyBlover : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Explode = "Explode";
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
		AudioManager.Instance.AudioPlay("Diamond");
		if (instance.hypnoses)
		{
			return;
		}
		TowerDefenseEnum.LEVEL_SEEDBANK_METHOD currentPacketBankMethod = TowerDefenseManager.Instance.GetCurrentPacketBankMethod();
		if ((uint)(currentPacketBankMethod - 1) > 1u)
		{
			return;
		}
		Array plantList = TowerDefenseManager.GetPacketBankData("GeneralPlant").GetPlantList();
		Array<TowerDefenseInGamePacketShow> seedBankList = TowerDefenseManager.Instance.GetSeedBankList();
		foreach (TowerDefenseInGamePacketShow item in seedBankList)
		{
			if (!(((item.originalSaveKey != "") ? item.originalSaveKey : item.config.saveKey) == packet.saveKey))
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig((string)plantList.PickRandom());
				TowerDefensePacketOverride towerDefensePacketOverride = new TowerDefensePacketOverride();
				if (packetConfig._GetType() != TowerDefenseEnum.PACKET_TYPE.DIAMOND)
				{
					towerDefensePacketOverride.cost = Mathf.Max(packetConfig.characterConfig.cost - 50, 0);
				}
				item.Cover(packetConfig, towerDefensePacketOverride, keepColddown: false, changePacket: false);
				item.coldDownTimer = packetConfig.GetStartingCooldown();
			}
		}
		int num = ((currentPacketBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET) ? 16 : TowerDefenseManager.Instance.seedbankPacketMax);
		if (seedBankList.Count >= num)
		{
			return;
		}
		int num2 = num - seedBankList.Count;
		for (int i = 0; i < num2; i++)
		{
			string packetName = (string)plantList.PickRandom();
			TowerDefensePacketOverride towerDefensePacketOverride2 = new TowerDefensePacketOverride();
			TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(packetName);
			if (packetConfig2._GetType() != TowerDefenseEnum.PACKET_TYPE.DIAMOND)
			{
				towerDefensePacketOverride2.cost = Mathf.Max(packetConfig2.characterConfig.cost - 50, 0);
			}
			TowerDefenseManager.Instance.AddPacket(packetName, towerDefensePacketOverride2);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
