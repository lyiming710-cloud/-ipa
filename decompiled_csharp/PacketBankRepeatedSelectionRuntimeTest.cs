using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PacketBankRepeatedSelectionRuntimeTest.cs")]
public class PacketBankRepeatedSelectionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreatePacket = "CreatePacket";

		public static readonly StringName CreateSelectedPacket = "CreateSelectedPacket";

		public static readonly StringName IsPreviewTreeFrozen = "IsPreviewTreeFrozen";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PacketName = "PacketBankRepeatedSelectionProbe";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		TowerDefensePacketConfig towerDefensePacketConfig = new TowerDefensePacketConfig
		{
			saveKey = "PacketBankRepeatedSelectionProbe",
			packetAnimeClip = "BodyIdle"
		};
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = CreatePacket(towerDefensePacketConfig);
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow2 = CreatePacket(towerDefensePacketConfig);
		TowerDefenseInGameSeedBank towerDefenseInGameSeedBank = new TowerDefenseInGameSeedBank();
		TowerDefenseInGamePacketBank towerDefenseInGamePacketBank = new TowerDefenseInGamePacketBank
		{
			seedBank = towerDefenseInGameSeedBank
		};
		TowerDefenseBattleFeaturePacketBank towerDefenseBattleFeaturePacketBank = new TowerDefenseBattleFeaturePacketBank
		{
			packetBank = towerDefenseInGamePacketBank
		};
		Node node = (towerDefenseInGameSeedBank.packetContainer = new Node
		{
			Name = "SelectedPacketContainer"
		});
		towerDefenseBattleFeaturePacketBank.packetList.Add(towerDefenseInGamePacketShow);
		towerDefenseInGamePacketShow.sprite = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/PeaShooterSingle/PeaShooterSingle.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		AddChild(towerDefenseInGamePacketShow.sprite, forceReadableName: false, InternalMode.Disabled);
		towerDefenseInGamePacketShow.OnMouseExited();
		int blockedPressCount = 0;
		towerDefenseInGamePacketShow2.OnPressed += (TowerDefenseInGamePacketShow _) =>
		{
			blockedPressCount++;
		};
		towerDefenseInGamePacketShow2.alive = false;
		towerDefenseInGamePacketShow2.Pressed();
		Check(blockedPressCount == 0, "普通不可用卡牌不应绕过原有点击拦截。");
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow3 = CreateSelectedPacket(towerDefensePacketConfig, node, towerDefenseInGameSeedBank, locked: false);
		towerDefenseInGamePacketShow.enforceRuntimeAvailabilityOnPress = false;
		towerDefenseInGamePacketShow.allowPressWhenUnavailable = true;
		towerDefenseInGamePacketShow.alive = false;
		towerDefenseInGamePacketShow.select = true;
		towerDefenseInGamePacketShow.OnPressed += towerDefenseBattleFeaturePacketBank.PacketChoose;
		Check(towerDefenseInGameSeedBank.HasPacket("PacketBankRepeatedSelectionProbe") && towerDefenseInGameSeedBank.packetNum == 1, "取消前种子栏必须包含目标卡牌。");
		towerDefenseInGamePacketShow.OnMouseEntered();
		Check(IsPreviewTreeFrozen(towerDefenseInGamePacketShow.sprite), "已选变暗卡牌再次收到鼠标进入时，角色及子动画必须保持冻结。");
		towerDefenseInGamePacketShow.Pressed();
		Check(IsPreviewTreeFrozen(towerDefenseInGamePacketShow.sprite), "再次点击取消选择后，角色预览不应重新播放。");
		Check(!towerDefenseInGameSeedBank.HasPacket("PacketBankRepeatedSelectionProbe"), "再次点击已选卡牌后种子栏标识未移除。");
		Check(towerDefenseInGameSeedBank.packetList.Count == 0 && towerDefenseInGameSeedBank.packetNum == 0, "再次点击已选卡牌后种子栏实例或容量未释放。");
		Check(towerDefenseInGamePacketShow.alive && !towerDefenseInGamePacketShow.select && towerDefenseInGamePacketShow.pressDelayTimer <= 0.0, "取消选择后卡库卡牌未恢复为可立即选择状态。");
		Check(!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow3.config), "取消选择后种子栏卡牌未进入既有回收流程。");
		towerDefenseInGamePacketShow.OnMouseEntered();
		Check(!towerDefenseInGamePacketShow.sprite.IsFrozenPreview, "取消选择后重新移入可用卡牌，预览仍应正常播放。");
		towerDefenseInGamePacketShow.OnMouseExited();
		Check(IsPreviewTreeFrozen(towerDefenseInGamePacketShow.sprite), "离开可用卡牌后，预览应恢复冻结。");
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow4 = CreateSelectedPacket(towerDefensePacketConfig, node, towerDefenseInGameSeedBank, locked: true);
		towerDefenseInGamePacketShow.alive = false;
		towerDefenseInGamePacketShow.select = false;
		towerDefenseInGamePacketShow.pressDelayTimer = 0.0;
		towerDefenseInGamePacketShow.Pressed();
		Check(towerDefenseInGameSeedBank.HasPacket("PacketBankRepeatedSelectionProbe") && towerDefenseInGameSeedBank.packetList.Count == 1 && towerDefenseInGameSeedBank.packetNum == 1, "锁定的预选卡牌不应被再次点击取消。");
		Check(!towerDefenseInGamePacketShow.alive && !towerDefenseInGamePacketShow.select, "锁定卡牌点击后应保持不可用且不显示临时选择框。");
		towerDefenseInGamePacketShow4.@lock = false;
		towerDefenseInGameSeedBank.DeletePacket(towerDefenseInGamePacketShow4);
		towerDefenseInGameSeedBank.DisposeOwnedPackets();
		node.Free();
		towerDefenseInGamePacketShow.sprite.Free();
		towerDefenseInGamePacketShow.sprite = null;
		towerDefenseInGamePacketShow.Free();
		towerDefenseInGamePacketShow2.Free();
		towerDefenseInGamePacketBank.Free();
		towerDefenseInGameSeedBank.Free();
		towerDefenseBattleFeaturePacketBank.Dispose();
		towerDefensePacketConfig.Dispose();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Finish();
	}

	private static TowerDefenseInGamePacketShow CreatePacket(TowerDefensePacketConfig packetConfig)
	{
		return new TowerDefenseInGamePacketShow
		{
			config = packetConfig,
			originalSaveKey = "PacketBankRepeatedSelectionProbe"
		};
	}

	private static TowerDefenseInGamePacketShow CreateSelectedPacket(TowerDefensePacketConfig packetConfig, Node packetContainer, TowerDefenseInGameSeedBank seedBank, bool locked)
	{
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = CreatePacket(packetConfig);
		towerDefenseInGamePacketShow.@lock = locked;
		packetContainer.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
		seedBank.packetList.Add(towerDefenseInGamePacketShow);
		seedBank.packetNameSet["PacketBankRepeatedSelectionProbe"] = true;
		seedBank.packetNum = seedBank.packetList.Count;
		return towerDefenseInGamePacketShow;
	}

	private static bool IsPreviewTreeFrozen(Node node)
	{
		if (node is AdobeAnimateSprite { IsFrozenPreview: false })
		{
			return false;
		}
		foreach (Node child in node.GetChildren())
		{
			if (!IsPreviewTreeFrozen(child))
			{
				return false;
			}
		}
		return true;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[PacketBankRepeatedSelectionRuntimeTest] " + message);
		}
	}

	private void Finish()
	{
		bool flag = _failures == 0 && _checks == 12;
		GD.Print($"PACKET_BANK_REPEAT_SELECTION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSelectedPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetContainer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "seedBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "locked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPreviewTreeFrozen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.CreatePacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSelectedPacket && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreateSelectedPacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<TowerDefenseInGameSeedBank>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.IsPreviewTreeFrozen && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPreviewTreeFrozen(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreatePacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreatePacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSelectedPacket && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreateSelectedPacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<TowerDefenseInGameSeedBank>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.IsPreviewTreeFrozen && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPreviewTreeFrozen(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.CreatePacket)
		{
			return true;
		}
		if (method == MethodName.CreateSelectedPacket)
		{
			return true;
		}
		if (method == MethodName.IsPreviewTreeFrozen)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
