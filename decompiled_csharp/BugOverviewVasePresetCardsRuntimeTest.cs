using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewVasePresetCardsRuntimeTest.cs")]
public class BugOverviewVasePresetCardsRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadLevel = "LoadLevel";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PresetLevelPath = "res://Asset/Config/Level/TowerDefense/Vase/VaseLevel6.tres";

	private const string NoCardLevelPath = "res://Asset/Config/Level/TowerDefense/Vase/VaseLevel2.tres";

	private static readonly string[] ExpectedPresetPackets = new string[3] { "PlantSquash", "PlantHypnoShroom", "PlantCherryBomb" };

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		TowerDefenseLevelBaseConfig currentLevelConfig = instance?.currentLevelConfig;
		TowerDefenseControlNew currentControl = instance?.currentControl;
		try
		{
			Check(GodotObject.IsInstanceValid(instance), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(instance))
			{
				return;
			}
			TowerDefenseLevelConfig towerDefenseLevelConfig = LoadLevel("res://Asset/Config/Level/TowerDefense/Vase/VaseLevel6.tres");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig), "The real VaseLevel6 preset-card resource must load.");
			if (!GodotObject.IsInstanceValid(towerDefenseLevelConfig))
			{
				return;
			}
			towerDefenseLevelConfig.Init();
			Check(towerDefenseLevelConfig.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE, "VaseLevel6 must remain a Vase process level.");
			Check(towerDefenseLevelConfig.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET, "VaseLevel6 must retain its configured PRESET card mode.");
			Check(towerDefenseLevelConfig.packetBankList.Count == ExpectedPresetPackets.Length, "VaseLevel6 must retain all three configured preset cards.");
			Check(PacketNamesMatch(towerDefenseLevelConfig.packetBankList, ExpectedPresetPackets), "VaseLevel6 preset cards must remain Squash, Hypno-shroom, and Cherry Bomb.");
			Check(towerDefenseLevelConfig.featureData.TryGetValue(new StringName("SeedBank"), out var value), "Legacy level conversion must produce SeedBank feature data for VaseLevel6.");
			Check(value.GetValueOrDefault("Method", "").AsString() == "PRESET", "SeedBank feature data must expose PRESET instead of forcing NOONE.");
			Check(value.GetValueOrDefault("Packet", new Godot.Collections.Array()).AsGodotArray().Count == ExpectedPresetPackets.Length, "SeedBank feature data must carry all configured preset cards.");
			instance.currentControl = null;
			instance.currentLevelConfig = towerDefenseLevelConfig;
			TowerDefenseLevelSeedBankConfig towerDefenseLevelSeedBankConfig = new TowerDefenseLevelSeedBankConfig();
			towerDefenseLevelSeedBankConfig.Init(value);
			Check(towerDefenseLevelSeedBankConfig.method == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET, "The real SeedBank config consumer must resolve PRESET for VaseLevel6.");
			Check(towerDefenseLevelSeedBankConfig.packetList.Count == ExpectedPresetPackets.Length, "The real SeedBank config consumer must materialize all preset cards.");
			Check(PacketNamesMatch(towerDefenseLevelSeedBankConfig.packetList, ExpectedPresetPackets), "The real SeedBank config consumer must preserve the configured card identities.");
			Check(instance.GetCurrentPacketBankMethod() == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET, "The runtime manager must report PRESET while VaseLevel6 is active.");
			Check(towerDefenseLevelConfig.processName == new StringName("Vase"), "VaseLevel6 must export the Vase process name.");
			Check(towerDefenseLevelConfig.processData.GetValueOrDefault("PacketBankMethod", -1).AsInt32() == 2, "Vase process data must carry PRESET instead of silently defaulting to NOONE.");
			TowerDefenseBattleProcessVase towerDefenseBattleProcessVase = new TowerDefenseBattleProcessVase();
			towerDefenseBattleProcessVase.Init(towerDefenseLevelConfig.processData.Duplicate(deep: true));
			Check(GodotObject.IsInstanceValid(towerDefenseBattleProcessVase.config), "The real Vase process config must initialize from the level process data.");
			Check(towerDefenseBattleProcessVase.config.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET, "The real Vase process consumer must resolve PRESET for VaseLevel6.");
			Check(towerDefenseBattleProcessVase.config.vaseList.Count == towerDefenseLevelConfig.vaseManager.vaseList.Count && towerDefenseBattleProcessVase.config.vaseList.Count > 0, "Vase process initialization must preserve the real level's vase layout.");
			towerDefenseBattleProcessVase.Destroy();
			TowerDefenseLevelConfig towerDefenseLevelConfig2 = LoadLevel("res://Asset/Config/Level/TowerDefense/Vase/VaseLevel2.tres");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig2), "The real VaseLevel2 no-card control resource must load.");
			if (!GodotObject.IsInstanceValid(towerDefenseLevelConfig2))
			{
				return;
			}
			towerDefenseLevelConfig2.Init();
			Check(towerDefenseLevelConfig2.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE, "Control: VaseLevel2 must remain intentionally configured without cards.");
			Check(towerDefenseLevelConfig2.processData.GetValueOrDefault("PacketBankMethod", -1).AsInt32() == 0, "Control: the Vase process must preserve an explicitly cardless level.");
			instance.currentLevelConfig = towerDefenseLevelConfig2;
			Check(instance.GetCurrentPacketBankMethod() == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE, "Control: the runtime manager must report NOONE for the cardless Vase level.");
			Check(!towerDefenseLevelConfig2.featureData.ContainsKey(new StringName("SeedBank")), "Control: an intentionally cardless Vase level must not create a SeedBank feature.");
		}
		catch (Exception value2)
		{
			_failures++;
			GD.PushError($"[BugOverviewVasePresetCardsRuntimeTest] Unexpected exception: {value2}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.currentLevelConfig = currentLevelConfig;
				instance.currentControl = currentControl;
			}
		}
		bool flag = _failures == 0 && _checks == 23;
		GD.Print($"VASE_PRESET_CARDS_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseLevelConfig LoadLevel(string path)
	{
		return ResourceLoader.Load<TowerDefenseLevelConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefenseLevelConfig;
	}

	private static bool PacketNamesMatch(Godot.Collections.Array packetList, IReadOnlyList<string> expected)
	{
		if (packetList == null || packetList.Count != expected.Count)
		{
			return false;
		}
		for (int i = 0; i < expected.Count; i++)
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = packetList[i].AsGodotObject() as TowerDefenseLevelPacketConfig;
			if (!GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig) || towerDefenseLevelPacketConfig.packetName != expected[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool PacketNamesMatch(Array<TowerDefenseLevelPacketConfig> packetList, IReadOnlyList<string> expected)
	{
		if (packetList == null || packetList.Count != expected.Count)
		{
			return false;
		}
		for (int i = 0; i < expected.Count; i++)
		{
			if (!GodotObject.IsInstanceValid(packetList[i]) || packetList[i].packetName != expected[i])
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
			GD.PushError("[BugOverviewVasePresetCardsRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadLevel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(LoadLevel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(LoadLevel(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadLevel)
		{
			return true;
		}
		if (method == MethodName.Check)
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
