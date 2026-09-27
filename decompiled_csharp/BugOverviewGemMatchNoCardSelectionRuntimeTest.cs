using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewGemMatchNoCardSelectionRuntimeTest.cs")]
public class BugOverviewGemMatchNoCardSelectionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyLevel = "VerifyLevel";

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

	private static readonly string[] LevelPaths = new string[4] { "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_1.tres", "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_1_D.tres", "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_2.tres", "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_2_D.tres" };

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		TowerDefenseLevelBaseConfig currentLevelConfig = instance?.currentLevelConfig;
		try
		{
			Check(GodotObject.IsInstanceValid(instance), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(instance))
			{
				return;
			}
			string[] levelPaths = LevelPaths;
			foreach (string levelPath in levelPaths)
			{
				VerifyLevel(instance, levelPath);
			}
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewGemMatchNoCardSelectionRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.currentLevelConfig = currentLevelConfig;
			}
		}
		bool flag = _failures == 0 && _checks == 29;
		GD.Print($"GEM_MATCH_NO_CARD_SELECTION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyLevel(TowerDefenseManager manager, string levelPath)
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>(levelPath, null, ResourceLoader.CacheMode.IgnoreDeep);
		Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig), levelPath + " must load as a TowerDefenseLevelConfig.");
		if (GodotObject.IsInstanceValid(towerDefenseLevelConfig))
		{
			Check(towerDefenseLevelConfig.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE, levelPath + " must persist the cardless SeedBank enum.");
			towerDefenseLevelConfig.Init();
			Check(towerDefenseLevelConfig.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE, levelPath + " must parse its authored SeedBank method as NOONE.");
			Check(towerDefenseLevelConfig.featureData.ContainsKey(new StringName("GemMatch")), levelPath + " must remain a GemMatch level.");
			Check(!towerDefenseLevelConfig.featureData.ContainsKey(new StringName("PacketBank")), levelPath + " must not configure the interactive card picker.");
			Check(towerDefenseLevelConfig.featureData.TryGetValue(new StringName("SeedBank"), out var value) && value.GetValueOrDefault("Method", "").AsString() == "NOONE", levelPath + " must expose NOONE to the runtime SeedBank consumer.");
			manager.currentLevelConfig = towerDefenseLevelConfig;
			TowerDefenseLevelSeedBankConfig towerDefenseLevelSeedBankConfig = new TowerDefenseLevelSeedBankConfig();
			towerDefenseLevelSeedBankConfig.Init(value);
			Check(towerDefenseLevelSeedBankConfig.method == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE && towerDefenseLevelSeedBankConfig.packetList.Count == 0, levelPath + " must resolve to a cardless runtime SeedBank.");
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewGemMatchNoCardSelectionRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "levelPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifyLevel && args.Count == 2)
		{
			VerifyLevel(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.VerifyLevel)
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
