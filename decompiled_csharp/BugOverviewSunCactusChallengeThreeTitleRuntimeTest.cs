using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSunCactusChallengeThreeTitleRuntimeTest.cs")]
public class BugOverviewSunCactusChallengeThreeTitleRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildRuntimeTitle = "BuildRuntimeTitle";

		public static readonly StringName ReadSourceLevelNumber = "ReadSourceLevelNumber";

		public static readonly StringName ResolveUidPath = "ResolveUidPath";

		public static readonly StringName FindLevelEntry = "FindLevelEntry";

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

	private const string LevelRegistryPath = "res://Asset/Config/Level/LevelResource.json";

	private const string TargetSaveKey = "Challenge_Level17_3";

	private const string ExpectedNormalPath = "res://Asset/Config/Level/TowerDefense/Challenge/Gold/Challenge_Level17_3.tres";

	private const string ExpectedDifficultPath = "res://Asset/Config/Level/TowerDefense/Challenge/Gold/Challenge_Level17_3_D.tres";

	private const string ProgressManagerPath = "res://Registry/Battle/Feature/Progress/Manager/TowerDefenseProgressManager.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		string previousLocale = TranslationServer.GetLocale();
		TowerDefenseLevelConfig normalConfig = null;
		TowerDefenseLevelConfig difficultConfig = null;
		TowerDefenseProgressManager normalProgress = null;
		TowerDefenseProgressManager difficultProgress = null;
		try
		{
			try
			{
				Json json = ResourceLoader.Load<Json>("res://Asset/Config/Level/LevelResource.json", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(json), "The production level registry must load.");
				if (!GodotObject.IsInstanceValid(json))
				{
					goto end_IL_0057;
				}
				Dictionary dictionary = FindLevelEntry(json.Data.AsGodotDictionary(), "Challenge_Level17_3");
				Check(dictionary.Count > 0, "The production challenge registry must contain Sun Cactus challenge 3.");
				if (dictionary.Count == 0)
				{
					goto end_IL_0057;
				}
				Check(dictionary.GetValueOrDefault("OpenKey", "").AsString() == "Challenge_Level17_2", "Challenge 3 must remain unlocked by completing challenge 2.");
				Dictionary dictionary2 = dictionary.GetValueOrDefault("Level", new Dictionary()).AsGodotDictionary();
				string text = dictionary2.GetValueOrDefault("Normal", "").AsString();
				string text2 = dictionary2.GetValueOrDefault("Difficult", "").AsString();
				Check(ResolveUidPath(text) == "res://Asset/Config/Level/TowerDefense/Challenge/Gold/Challenge_Level17_3.tres", "The registry Normal UID must resolve to the challenge-3 resource.");
				Check(ResolveUidPath(text2) == "res://Asset/Config/Level/TowerDefense/Challenge/Gold/Challenge_Level17_3_D.tres", "The registry Difficult UID must resolve to the challenge-3 resource.");
				normalConfig = ResourceLoader.Load<TowerDefenseLevelConfig>(text, null, ResourceLoader.CacheMode.Ignore);
				difficultConfig = ResourceLoader.Load<TowerDefenseLevelConfig>(text2, null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(normalConfig) && GodotObject.IsInstanceValid(difficultConfig), "Both production challenge-3 difficulties must load through their registry UIDs.");
				if (!GodotObject.IsInstanceValid(normalConfig) || !GodotObject.IsInstanceValid(difficultConfig))
				{
					goto end_IL_0057;
				}
				Check(normalConfig.name == "Challenge_Level17_3" && difficultConfig.name == "Challenge_Level17_3", "Both resources must identify themselves as challenge 3.");
				Check(normalConfig.levelNumber == 3 && difficultConfig.levelNumber == 3, "Both runtime resources must retain LevelNumber 3 instead of the default 1.");
				Check(ReadSourceLevelNumber(normalConfig) == 3 && ReadSourceLevelNumber(difficultConfig) == 3, "Both backing JSON resources must also retain LevelNumber 3.");
				Check(normalConfig.levelName == "CHALLENGE_LEVEL_NAME_17" && difficultConfig.levelName == normalConfig.levelName, "Both difficulties must use the shared Sun Cactus title template.");
				TranslationServer.SetLocale("zh");
				string normalTitle = BuildRuntimeTitle(normalConfig);
				string difficultTitle = BuildRuntimeTitle(difficultConfig);
				Check(normalTitle.Contains("3", StringComparison.Ordinal) && !normalTitle.Contains("{LevelNumber}", StringComparison.Ordinal), "The production title formatter must resolve Normal to challenge 3; got '" + normalTitle + "'.");
				Check(difficultTitle.Contains("3", StringComparison.Ordinal) && !difficultTitle.Contains("{LevelNumber}", StringComparison.Ordinal), "The production title formatter must resolve Difficult to challenge 3; got '" + difficultTitle + "'.");
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/Progress/Manager/TowerDefenseProgressManager.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The production in-game progress UI must load.");
				if (!GodotObject.IsInstanceValid(packedScene))
				{
					goto end_IL_0057;
				}
				normalProgress = packedScene.Instantiate<TowerDefenseProgressManager>(PackedScene.GenEditState.Disabled);
				difficultProgress = packedScene.Instantiate<TowerDefenseProgressManager>(PackedScene.GenEditState.Disabled);
				AddChild(normalProgress, forceReadableName: false, InternalMode.Disabled);
				AddChild(difficultProgress, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				normalProgress.SetupUI("Normal", normalTitle);
				difficultProgress.SetupUI("Difficult", difficultTitle);
				Check(normalProgress.levelNameLabel.Text == normalTitle && normalProgress.levelNameLabel.Text.Contains("3", StringComparison.Ordinal), "The real Normal progress label must display Sun Cactus challenge 3.");
				Check(difficultProgress.levelNameLabel.Text == difficultTitle && difficultProgress.levelNameLabel.Text.Contains("3", StringComparison.Ordinal), "The real Difficult progress label must display Sun Cactus challenge 3.");
				goto end_IL_004e;
				end_IL_0057:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewSunCactusChallengeThreeTitleRuntimeTest] Unexpected exception: {value}");
				goto end_IL_004e;
			}
			return;
			end_IL_004e:;
		}
		finally
		{
			TranslationServer.SetLocale(previousLocale);
			if (GodotObject.IsInstanceValid(normalProgress))
			{
				normalProgress.QueueFree();
			}
			if (GodotObject.IsInstanceValid(difficultProgress))
			{
				difficultProgress.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			normalConfig?.Dispose();
			difficultConfig?.Dispose();
		}
		bool flag = _failures == 0 && _checks == 15;
		GD.Print($"SUN_CACTUS_CHALLENGE_THREE_TITLE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private string BuildRuntimeTitle(TowerDefenseLevelConfig config)
	{
		return Tr(config.levelName).Replace("{LevelNumber}", config.levelNumber.ToString(), StringComparison.Ordinal);
	}

	private static int ReadSourceLevelNumber(TowerDefenseLevelConfig config)
	{
		if (!GodotObject.IsInstanceValid(config?.data))
		{
			return -1;
		}
		return config.data.Data.AsGodotDictionary().GetValueOrDefault("LevelNumber", -1).AsInt32();
	}

	private static string ResolveUidPath(string uid)
	{
		long num = ResourceUid.TextToId(uid);
		if (num == -1 || !ResourceUid.HasId(num))
		{
			return "";
		}
		return ResourceUid.GetIdPath(num);
	}

	private static Dictionary FindLevelEntry(Dictionary registry, string saveKey)
	{
		foreach (Variant item in registry.GetValueOrDefault("Challenge", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Chapter", new Godot.Collections.Array())
			.AsGodotArray())
		{
			foreach (Variant item2 in item.AsGodotDictionary().GetValueOrDefault("Level", new Godot.Collections.Array()).AsGodotArray())
			{
				Dictionary dictionary = item2.AsGodotDictionary();
				if (dictionary.GetValueOrDefault("SaveKey", "").AsString() == saveKey)
				{
					return dictionary;
				}
			}
		}
		return new Dictionary();
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewSunCactusChallengeThreeTitleRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRuntimeTitle, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadSourceLevelNumber, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveUidPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "uid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindLevelEntry, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "registry", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "saveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BuildRuntimeTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildRuntimeTitle(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadSourceLevelNumber && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ReadSourceLevelNumber(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveUidPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveUidPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindLevelEntry && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(FindLevelEntry(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.ReadSourceLevelNumber && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ReadSourceLevelNumber(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveUidPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveUidPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindLevelEntry && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(FindLevelEntry(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.BuildRuntimeTitle)
		{
			return true;
		}
		if (method == MethodName.ReadSourceLevelNumber)
		{
			return true;
		}
		if (method == MethodName.ResolveUidPath)
		{
			return true;
		}
		if (method == MethodName.FindLevelEntry)
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
