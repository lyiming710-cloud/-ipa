using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BgmCanonicalLevelFieldRuntimeTest.cs")]
public class BgmCanonicalLevelFieldRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyGemMatchLevel = "VerifyGemMatchLevel";

		public static readonly StringName VerifyHistoricalFeatureField = "VerifyHistoricalFeatureField";

		public static readonly StringName VerifyHistoricalOnlineLevel = "VerifyHistoricalOnlineLevel";

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

	private static readonly string[] GemMatchLevelPaths = new string[4] { "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_1.tres", "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_1_D.tres", "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_2.tres", "res://Asset/Config/Level/TowerDefense/MiniGames/MiniGames_Level5_2_D.tres" };

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		Check(GodotObject.IsInstanceValid(TowerDefenseManager.Instance), "TowerDefenseManager autoload must be available.");
		Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
		if (!GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			GetTree().Quit(2);
			return;
		}
		TowerDefenseBackgroundMusicConfig towerDefenseBackgroundMusicConfig = ResourceLoader.Load<TowerDefenseBackgroundMusicConfig>("res://Asset/Config/BGM/Config/TowerDefenseFrontlawnNight.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
		Check(GodotObject.IsInstanceValid(towerDefenseBackgroundMusicConfig), "FrontlawnNight BGM configuration must load.");
		ResourceManager.Instance.BGMS["FrontlawnNight"] = towerDefenseBackgroundMusicConfig;
		string[] gemMatchLevelPaths = GemMatchLevelPaths;
		foreach (string levelPath in gemMatchLevelPaths)
		{
			VerifyGemMatchLevel(levelPath);
		}
		VerifyHistoricalFeatureField();
		VerifyHistoricalOnlineLevel();
		bool flag = _failures == 0 && _checks == 32;
		GD.Print($"BGM_CANONICAL_LEVEL_FIELD_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyGemMatchLevel(string levelPath)
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>(levelPath, null, ResourceLoader.CacheMode.IgnoreDeep);
		Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig), levelPath + " must load as TowerDefenseLevelConfig.");
		if (GodotObject.IsInstanceValid(towerDefenseLevelConfig))
		{
			towerDefenseLevelConfig.Init();
			bool flag = towerDefenseLevelConfig.featureData.TryGetValue(new StringName("BGM"), out var value);
			Check(flag, levelPath + " must contain a BGM feature.");
			if (flag)
			{
				Check(value.GetValueOrDefault("BackgroundMusic", "").AsString() == "FrontlawnNight", levelPath + " must use FrontlawnNight through BackgroundMusic.");
				Check(!value.ContainsKey("BGMName"), levelPath + " must not retain the historical BGM field.");
				TowerDefenseBattleFeatureBGM towerDefenseBattleFeatureBGM = new TowerDefenseBattleFeatureBGM();
				towerDefenseBattleFeatureBGM.Init(value);
				Check(GodotObject.IsInstanceValid(towerDefenseBattleFeatureBGM.backgroundMusicConfig), levelPath + " must resolve its BGM resource.");
				Check(towerDefenseBattleFeatureBGM.backgroundMusicConfig?.flag1 == "Moongrains", levelPath + " must resolve FrontlawnNight to Moongrains.");
				towerDefenseBattleFeatureBGM.Destroy();
			}
		}
	}

	private void VerifyHistoricalFeatureField()
	{
		Dictionary data = new Dictionary { ["BGMName"] = "FrontlawnNight" };
		TowerDefenseBattleFeatureBGM towerDefenseBattleFeatureBGM = new TowerDefenseBattleFeatureBGM();
		towerDefenseBattleFeatureBGM.Init(data);
		Check(GodotObject.IsInstanceValid(towerDefenseBattleFeatureBGM.backgroundMusicConfig), "Historical online BGMName must remain readable.");
		Check(towerDefenseBattleFeatureBGM.backgroundMusicConfig?.flag1 == "Moongrains", "Historical online BGMName must resolve to Moongrains.");
		towerDefenseBattleFeatureBGM.Destroy();
	}

	private void VerifyHistoricalOnlineLevel()
	{
		Json json = ResourceLoader.Load<Json>("res://Core/Save/OnlineLevel/OnlineLevel-4.json", null, ResourceLoader.CacheMode.IgnoreDeep);
		Check(GodotObject.IsInstanceValid(json), "Historical online level JSON must load.");
		bool flag = new TowerDefenseLevelConfig
		{
			data = json
		}.featureData.TryGetValue(new StringName("BGM"), out var value);
		Check(flag, "Historical top-level online BGM must create a BGM feature.");
		Check(flag && value.GetValueOrDefault("BackgroundMusic", "").AsString() == "Frontlawn", "Historical top-level online BGM must normalize to BackgroundMusic.");
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BgmCanonicalLevelFieldRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyGemMatchLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VerifyHistoricalFeatureField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyHistoricalOnlineLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.VerifyGemMatchLevel && args.Count == 1)
		{
			VerifyGemMatchLevel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyHistoricalFeatureField && args.Count == 0)
		{
			VerifyHistoricalFeatureField();
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyHistoricalOnlineLevel && args.Count == 0)
		{
			VerifyHistoricalOnlineLevel();
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
		if (method == MethodName.VerifyGemMatchLevel)
		{
			return true;
		}
		if (method == MethodName.VerifyHistoricalFeatureField)
		{
			return true;
		}
		if (method == MethodName.VerifyHistoricalOnlineLevel)
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
