using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewLevelEditorFinishMethodAfterTestRuntimeTest.cs")]
public class BugOverviewLevelEditorFinishMethodAfterTestRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadConfig = "LoadConfig";

		public static readonly StringName FindFeature = "FindFeature";

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
				["FinishMethod"] = "WAVE",
				["Feature"] = new Godot.Collections.Array
				{
					new Dictionary
					{
						["Name"] = "Wave",
						["Data"] = new Dictionary { ["CustomWaveMarker"] = 31 }
					},
					new Dictionary
					{
						["Name"] = "CustomEditorFeature",
						["Data"] = new Dictionary { ["KeepMarker"] = 47 }
					}
				},
				["Process"] = new Dictionary
				{
					["Name"] = "Wave",
					["Data"] = new Dictionary { ["CustomMarker"] = 77 }
				},
				["PacketBank"] = new Dictionary
				{
					["Method"] = "CHOOSE",
					["Value"] = new Godot.Collections.Array()
				}
			});
			Check(towerDefenseLevelConfig.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE && towerDefenseLevelConfig.processName == (StringName)"Wave", "A tested/reloaded Wave level must expose its explicit process.");
			towerDefenseLevelConfig.SetFinishMethodFromEditor(TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE);
			Dictionary dictionary = towerDefenseLevelConfig.Export();
			Dictionary dictionary2 = dictionary["Process"].AsGodotDictionary();
			Check(dictionary2.GetValueOrDefault("Name", "").AsString() == "Wave", "Selecting the existing editor mode must keep the Wave process.");
			Check(dictionary2.GetValueOrDefault("Data", new Dictionary()).AsGodotDictionary().GetValueOrDefault("CustomMarker", 0)
				.AsInt32() == 77, "Selecting the existing mode must preserve explicit process data.");
			Dictionary dictionary3 = FindFeature(dictionary, "Wave");
			Check(dictionary3 != null && dictionary3.GetValueOrDefault("CustomWaveMarker", 0).AsInt32() == 31, "Selecting the existing mode must preserve the explicit Wave feature snapshot.");
			towerDefenseLevelConfig.vaseManager = new TowerDefenseLevelVaseManagerConfig();
			towerDefenseLevelConfig.SetFinishMethodFromEditor(TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE);
			Dictionary dictionary4 = towerDefenseLevelConfig.Export();
			Dictionary dictionary5 = dictionary4["Process"].AsGodotDictionary();
			Check(dictionary4.GetValueOrDefault("FinishMethod", "").AsString() == "VASE", "Editor mode change must export the Vase finish method.");
			Check(dictionary5.GetValueOrDefault("Name", "").AsString() == "Vase", "Old explicit Wave process must not overwrite the selected Vase process.");
			Check(!dictionary5.GetValueOrDefault("Data", new Dictionary()).AsGodotDictionary().ContainsKey("CustomMarker"), "Mode change must not carry old Wave-only process data into Vase.");
			Check(FindFeature(dictionary4, "Wave") == null, "Mode change to Vase must remove the tested level's stale Wave feature snapshot.");
			Dictionary dictionary6 = FindFeature(dictionary4, "CustomEditorFeature");
			Check(dictionary6 != null && dictionary6.GetValueOrDefault("KeepMarker", 0).AsInt32() == 47, "Mode change must preserve unrelated explicit/custom features.");
			TowerDefenseLevelConfig towerDefenseLevelConfig2 = LoadConfig(dictionary4);
			Check(towerDefenseLevelConfig2.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE && towerDefenseLevelConfig2.processName == (StringName)"Vase", "A saved Vase edit must survive the same reload that follows level testing.");
			towerDefenseLevelConfig2.izmManager = new TowerDefenseLevelIZMManagerConfig();
			towerDefenseLevelConfig2.SetFinishMethodFromEditor(TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM);
			Dictionary dictionary7 = towerDefenseLevelConfig2.Export();
			Dictionary dictionary8 = dictionary7["Process"].AsGodotDictionary();
			Check(dictionary7.GetValueOrDefault("FinishMethod", "").AsString() == "IZM", "A second editor mode change must export IZM.");
			Check(dictionary8.GetValueOrDefault("Name", "").AsString() == "IZM", "Reloaded explicit Vase process must not pin the editor to Vase.");
			Check(FindFeature(dictionary7, "Wave") == null, "A reloaded mode edit must not resurrect the old Wave feature snapshot.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewLevelEditorFinishMethodAfterTestRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0 && _checks == 13;
		GD.Print($"LEVEL_EDITOR_FINISH_METHOD_AFTER_TEST_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewLevelEditorFinishMethodAfterTestRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "exportedLevel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
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
		if (method == MethodName.LoadConfig)
		{
			return true;
		}
		if (method == MethodName.FindFeature)
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
