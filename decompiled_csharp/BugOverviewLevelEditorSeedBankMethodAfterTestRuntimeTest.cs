using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewLevelEditorSeedBankMethodAfterTestRuntimeTest.cs")]
public class BugOverviewLevelEditorSeedBankMethodAfterTestRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifySwitch = "VerifySwitch";

		public static readonly StringName LoadConfig = "LoadConfig";

		public static readonly StringName FindFeature = "FindFeature";

		public static readonly StringName HasFeature = "HasFeature";

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

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		try
		{
			TowerDefenseLevelConfig towerDefenseLevelConfig = LoadConfig(new Dictionary
			{
				["FinishMethod"] = "VASE",
				["Feature"] = new Godot.Collections.Array
				{
					new Dictionary
					{
						["Name"] = "RainMode",
						["Data"] = new Dictionary { ["OldRainMarker"] = 31 }
					},
					new Dictionary
					{
						["Name"] = "ScreenEffect",
						["Data"] = new Dictionary
						{
							["StormOpen"] = false,
							["PacketBankMethod"] = 4
						}
					},
					new Dictionary
					{
						["Name"] = "CustomEditorFeature",
						["Data"] = new Dictionary { ["KeepMarker"] = 47 }
					}
				},
				["Process"] = new Dictionary
				{
					["Name"] = "Vase",
					["Data"] = new Dictionary { ["PacketBankMethod"] = 4 }
				},
				["PacketBank"] = new Dictionary
				{
					["Method"] = "RAIN",
					["Value"] = new Godot.Collections.Array(),
					["RainPreset"] = new Dictionary()
				}
			});
			TowerDefenseLevelConfig towerDefenseLevelConfig2 = towerDefenseLevelConfig;
			if (towerDefenseLevelConfig2.conveyorData == null)
			{
				towerDefenseLevelConfig2.conveyorData = new TowerDefenseConveyorConfig();
			}
			towerDefenseLevelConfig2 = towerDefenseLevelConfig;
			if (towerDefenseLevelConfig2.rainData == null)
			{
				towerDefenseLevelConfig2.rainData = new TowerDefenseRainModeConfig();
			}
			Check(towerDefenseLevelConfig.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN, "A tested/reloaded Rain level must expose its explicit SeedBank method.");
			Check(!towerDefenseLevelConfig.SetPacketBankMethodFromEditor(TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN), "Selecting the existing SeedBank method must not discard explicit data.");
			Dictionary exportedLevel = towerDefenseLevelConfig.Export();
			Dictionary dictionary = FindFeature(exportedLevel, "RainMode");
			Check(dictionary != null && dictionary.GetValueOrDefault("OldRainMarker", 0).AsInt32() == 31, "Selecting the existing Rain method must preserve its explicit snapshot.");
			Dictionary dictionary2 = FindFeature(exportedLevel, "CustomEditorFeature");
			Check(dictionary2 != null && dictionary2.GetValueOrDefault("KeepMarker", 0).AsInt32() == 47, "Selecting the existing method must preserve unrelated custom features.");
			VerifySwitch(towerDefenseLevelConfig, TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE, expectedRain: false, expectedConveyor: false, expectedSeedBank: true, expectedPacketBank: true);
			VerifySwitch(towerDefenseLevelConfig, TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CONVEYOR, expectedRain: false, expectedConveyor: true, expectedSeedBank: false, expectedPacketBank: false);
			VerifySwitch(towerDefenseLevelConfig, TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.RAIN, expectedRain: true, expectedConveyor: false, expectedSeedBank: false, expectedPacketBank: false);
			VerifySwitch(towerDefenseLevelConfig, TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET, expectedRain: false, expectedConveyor: false, expectedSeedBank: true, expectedPacketBank: true);
			TowerDefenseLevelConfig towerDefenseLevelConfig3 = LoadConfig(VerifySwitch(towerDefenseLevelConfig, TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE, expectedRain: false, expectedConveyor: false, expectedSeedBank: false, expectedPacketBank: false));
			Check(towerDefenseLevelConfig3.packetBankMethod == TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE, "The final no-card mode must survive the save/reload cycle.");
			Check(!HasFeature(towerDefenseLevelConfig3.featureData, "RainMode") && !HasFeature(towerDefenseLevelConfig3.featureData, "ConveyorBelt") && !HasFeature(towerDefenseLevelConfig3.featureData, "SeedBank") && !HasFeature(towerDefenseLevelConfig3.featureData, "PacketBank"), "Reloading must not resurrect any stale SeedBank-mode feature.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewLevelEditorSeedBankMethodAfterTestRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0 && _checks == 36;
		GD.Print($"LEVEL_EDITOR_SEEDBANK_METHOD_AFTER_TEST_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private Dictionary VerifySwitch(TowerDefenseLevelConfig config, TowerDefenseEnum.LEVEL_SEEDBANK_METHOD method, bool expectedRain, bool expectedConveyor, bool expectedSeedBank, bool expectedPacketBank)
	{
		Check(config.SetPacketBankMethodFromEditor(method), $"Switching to {method} must be recognized as an editor mode change.");
		Dictionary dictionary = config.Export();
		Check(dictionary.GetValueOrDefault("PacketBank", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Method", "")
			.AsString() == method.ToString(), $"The legacy PacketBank section must export {method}.");
		Check(FindFeature(dictionary, "RainMode") != null == expectedRain && FindFeature(dictionary, "ConveyorBelt") != null == expectedConveyor && FindFeature(dictionary, "SeedBank") != null == expectedSeedBank && FindFeature(dictionary, "PacketBank") != null == expectedPacketBank, $"{method} must export only the Feature set owned by that SeedBank mode.");
		Check(FindFeature(dictionary, "ScreenEffect")?.GetValueOrDefault("PacketBankMethod", -1).AsInt32() == (int?)method, $"ScreenEffect must receive the new {method} value.");
		Dictionary dictionary2 = dictionary.GetValueOrDefault("Process", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Data", new Dictionary())
			.AsGodotDictionary();
		Check(dictionary2.GetValueOrDefault("PacketBankMethod", -1).AsInt32() == (int)method, $"The preserved Vase process must receive the new {method} value.");
		Dictionary dictionary3 = FindFeature(dictionary, "CustomEditorFeature");
		Check(dictionary3 != null && dictionary3.GetValueOrDefault("KeepMarker", 0).AsInt32() == 47, $"Switching to {method} must preserve unrelated custom features.");
		return dictionary;
	}

	private static TowerDefenseLevelConfig LoadConfig(Dictionary data)
	{
		Json json = new Json();
		Error error = json.Parse(Json.Stringify(data));
		if (error != Error.Ok)
		{
			throw new InvalidOperationException($"Unable to parse focused level JSON: {error}");
		}
		return new TowerDefenseLevelConfig
		{
			data = json
		};
	}

	private static Dictionary FindFeature(Dictionary exportedLevel, string name)
	{
		foreach (Variant item in exportedLevel.GetValueOrDefault("Feature", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			if (dictionary.GetValueOrDefault("Name", "").AsString() == name)
			{
				return dictionary.GetValueOrDefault("Data", new Dictionary()).AsGodotDictionary();
			}
		}
		return null;
	}

	private static bool HasFeature(Godot.Collections.Dictionary<StringName, Dictionary> featureData, string name)
	{
		return featureData?.ContainsKey(new StringName(name)) ?? false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewLevelEditorSeedBankMethodAfterTestRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifySwitch, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "method", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "expectedRain", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "expectedConveyor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "expectedSeedBank", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "expectedPacketBank", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "exportedLevel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFeature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "featureData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifySwitch && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(VerifySwitch(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.LoadConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(LoadConfig(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.FindFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(FindFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFeature(VariantUtils.ConvertToDictionary<StringName, Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.LoadConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(LoadConfig(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.FindFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(FindFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFeature(VariantUtils.ConvertToDictionary<StringName, Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.VerifySwitch)
		{
			return true;
		}
		if (method == MethodName.LoadConfig)
		{
			return true;
		}
		if (method == MethodName.FindFeature)
		{
			return true;
		}
		if (method == MethodName.HasFeature)
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
