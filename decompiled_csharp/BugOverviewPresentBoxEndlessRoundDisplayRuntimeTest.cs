using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewPresentBoxEndlessRoundDisplayRuntimeTest.cs")]
public class BugOverviewPresentBoxEndlessRoundDisplayRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetCompletedLevelResult = "SetCompletedLevelResult";

		public static readonly StringName SaveProgress = "SaveProgress";

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

	private const string LevelKey = "Survival_Level_Entertainment1_3";

	private const string ProbeUser = "PresentBoxEndlessRoundDisplayProbe";

	private const string LevelConfigPath = "res://Asset/Config/Level/TowerDefense/Survival/Entertainment/Survival_Level_Entertainment1_3.tres";

	private const string SurvivalConfigPath = "res://Asset/Config/Survival/Config/PresentBox/PresentBoxEndlessNormal.tres";

	private const string PacketConfigPath = "res://Asset/Anime/Character/Plant/Special/PresentBox/Packet/PlantPresentBox.tres";

	private const string LevelItemScenePath = "res://Prefab/GUI/DragMenu/Select/Level/DragMenuSelectItemlevel.tscn";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		DragMenuSelectItemlevel dragMenuSelectItemlevel = null;
		TowerDefenseLevelSaveConfigCSharp towerDefenseLevelSaveConfigCSharp = null;
		TowerDefenseLevelConfig towerDefenseLevelConfig = null;
		TowerDefenseLevelSurvivalConfig towerDefenseLevelSurvivalConfig = null;
		TowerDefensePacketConfig towerDefensePacketConfig = null;
		PackedScene packedScene = null;
		string text = Global.Instance?.currentLevelChoose;
		try
		{
			Check(GodotObject.IsInstanceValid(Global.Instance), "Global autoload must be available.");
			Check(GodotObject.IsInstanceValid(GameSaveManager.Instance), "GameSaveManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(Global.Instance) || !GodotObject.IsInstanceValid(GameSaveManager.Instance))
			{
				return;
			}
			GameSaveManager.Instance.EnsureLoaded();
			GameSaveManager.Instance.SetUserCurrent("PresentBoxEndlessRoundDisplayProbe");
			Global.Instance.currentLevelChoose = "Survival";
			towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/Survival/Entertainment/Survival_Level_Entertainment1_3.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
			towerDefenseLevelSurvivalConfig = ResourceLoader.Load<TowerDefenseLevelSurvivalConfig>("res://Asset/Config/Survival/Config/PresentBox/PresentBoxEndlessNormal.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
			towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Special/PresentBox/Packet/PlantPresentBox.tres", null, ResourceLoader.CacheMode.IgnoreDeep);
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig), "The production Present Box endless level config must load.");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && towerDefenseLevelConfig.name == "Survival_Level_Entertainment1_3", "The production Present Box endless level must use save key 'Survival_Level_Entertainment1_3'.");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && GodotObject.IsInstanceValid(towerDefenseLevelConfig.waveManager), "The production Present Box endless level must own a real Wave manager.");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && GodotObject.IsInstanceValid(towerDefenseLevelConfig.waveManager) && towerDefenseLevelConfig.waveManager.survival.AsString() == "PresentBoxEndlessNormal", "The production level must select PresentBoxEndlessNormal survival flow.");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelSurvivalConfig), "The production PresentBoxEndlessNormal survival config must load.");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelSurvivalConfig) && towerDefenseLevelSurvivalConfig.zombiePoolBase.Count > 0, "The real Present Box endless survival config must contain its zombie pool.");
			Check(GodotObject.IsInstanceValid(towerDefensePacketConfig), "The production PlantPresentBox packet must load.");
			Check(GodotObject.IsInstanceValid(towerDefensePacketConfig) && towerDefensePacketConfig.saveKey == "PlantPresentBox", "The production packet fixture must be the ordinary Present Box card.");
			int condition;
			if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				Array<UnlockConditionBaseConfig> unlockCheckList = towerDefensePacketConfig.unlockCheckList;
				if (unlockCheckList != null && unlockCheckList.Count == 1)
				{
					condition = ((towerDefensePacketConfig.unlockCheckList[0] is UnlockConditionLevelSurvivalRoundConfig) ? 1 : 0);
					goto IL_01f9;
				}
			}
			condition = 0;
			goto IL_01f9;
			IL_0239:
			object obj;
			UnlockConditionLevelSurvivalRoundConfig unlockConditionLevelSurvivalRoundConfig = (UnlockConditionLevelSurvivalRoundConfig)obj;
			Check(unlockConditionLevelSurvivalRoundConfig?.levelSaveKey == "Survival_Level_Entertainment1_3", "The ordinary Present Box card and its endless selection item must share the same level save key.");
			SetCompletedLevelResult();
			towerDefenseLevelSaveConfigCSharp = new TowerDefenseLevelSaveConfigCSharp();
			towerDefenseLevelSaveConfigCSharp.featureSave[new StringName("Wave")] = new Dictionary
			{
				["isSurvival"] = true,
				["survivalRoundNum"] = 14
			};
			towerDefenseLevelSaveConfigCSharp.processSave["main"] = new Dictionary
			{
				["isSurvival"] = true,
				["survivalRoundNum"] = 4
			};
			Check(SaveProgress(towerDefenseLevelSaveConfigCSharp) == Error.Ok, "The isolated real-key Present Box endless progress must save successfully.");
			Check(GameSaveManager.Instance.HasLevelProgress("Survival_Level_Entertainment1_3"), "The production save manager must resolve Present Box progress by its real level key.");
			Check(unlockConditionLevelSurvivalRoundConfig?.CheckProgress(towerDefenseLevelSaveConfigCSharp) ?? false, "The real Present Box unlock condition must accept fourteen completed rounds.");
			packedScene = ResourceLoader.Load<PackedScene>("res://Prefab/GUI/DragMenu/Select/Level/DragMenuSelectItemlevel.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
			Check(GodotObject.IsInstanceValid(packedScene) && packedScene.CanInstantiate(), "The production endless level-selection item scene must instantiate.");
			if (!GodotObject.IsInstanceValid(packedScene) || !packedScene.CanInstantiate())
			{
				return;
			}
			dragMenuSelectItemlevel = packedScene.Instantiate<DragMenuSelectItemlevel>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(dragMenuSelectItemlevel), "The real DragMenuSelectItemlevel script must be attached.");
			if (!GodotObject.IsInstanceValid(dragMenuSelectItemlevel))
			{
				return;
			}
			AddChild(dragMenuSelectItemlevel, forceReadableName: false, InternalMode.Disabled);
			Label nodeOrNull = dragMenuSelectItemlevel.GetNodeOrNull<Label>("%SurvivalLabel");
			Sprite2D nodeOrNull2 = dragMenuSelectItemlevel.GetNodeOrNull<Sprite2D>("%CupSprite");
			Check(GodotObject.IsInstanceValid(nodeOrNull), "The real selection item must expose SurvivalLabel.");
			Check(GodotObject.IsInstanceValid(nodeOrNull2), "The real selection item must expose CupSprite.");
			if (!GodotObject.IsInstanceValid(nodeOrNull) || !GodotObject.IsInstanceValid(nodeOrNull2))
			{
				return;
			}
			dragMenuSelectItemlevel.Init("Survival_Level_Entertainment1_3");
			Check(nodeOrNull.Visible, "The ordinary Present Box endless selection item must show completed rounds.");
			Check(nodeOrNull.Text == "14轮完成", "The selection item must show authoritative round 14, got '" + nodeOrNull.Text + "'.");
			Check(GodotObject.IsInstanceValid(nodeOrNull2.Texture), "The completion trophy and completed-round label must be visible together.");
			dragMenuSelectItemlevel.Init("Survival_Level_Entertainment1_3");
			Check(nodeOrNull.Visible && nodeOrNull.Text == "14轮完成", "Reusing the real selection item must retain the Present Box completed-round count.");
			goto end_IL_0021;
			IL_01f9:
			Check((byte)condition != 0, "PlantPresentBox must keep its real completed-round unlock condition.");
			if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				Array<UnlockConditionBaseConfig> unlockCheckList2 = towerDefensePacketConfig.unlockCheckList;
				if (unlockCheckList2 != null && unlockCheckList2.Count == 1)
				{
					obj = towerDefensePacketConfig.unlockCheckList[0] as UnlockConditionLevelSurvivalRoundConfig;
					goto IL_0239;
				}
			}
			obj = null;
			goto IL_0239;
			end_IL_0021:;
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[PresentBoxEndlessRoundDisplay] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(Global.Instance) && text != null)
			{
				Global.Instance.currentLevelChoose = text;
			}
			if (GodotObject.IsInstanceValid(dragMenuSelectItemlevel))
			{
				dragMenuSelectItemlevel.Free();
			}
			towerDefenseLevelSaveConfigCSharp?.Dispose();
			packedScene?.Dispose();
			towerDefensePacketConfig?.Dispose();
			towerDefenseLevelSurvivalConfig?.Dispose();
			towerDefenseLevelConfig?.Dispose();
		}
		bool flag = _failures == 0 && _checks == 23;
		GD.Print($"PRESENT_BOX_ENDLESS_ROUND_DISPLAY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void SetCompletedLevelResult()
	{
		GameSaveManager.Instance.SetLevelValue("Survival_Level_Entertainment1_3", new Dictionary
		{
			["Normal"] = true,
			["Difficult"] = false,
			["Ultimate"] = false,
			["Mower"] = true,
			["Key"] = new Dictionary { ["Finish"] = 1 }
		});
	}

	private static Error SaveProgress(TowerDefenseLevelSaveConfigCSharp progress)
	{
		string text = "user://Csharp/Progress/PresentBoxEndlessRoundDisplayProbe";
		Error error = DirAccess.MakeDirRecursiveAbsolute(text);
		if (error != Error.Ok && error != Error.AlreadyExists)
		{
			return error;
		}
		return ResourceSaver.Save(progress, text + "/Survival_Level_Entertainment1_3.tres", ResourceSaver.SaverFlags.None);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[PresentBoxEndlessRoundDisplay] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCompletedLevelResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveProgress, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "progress", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.SetCompletedLevelResult && args.Count == 0)
		{
			SetCompletedLevelResult();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveProgress && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Error>(SaveProgress(VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[0])));
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
		if (method == MethodName.SaveProgress && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Error>(SaveProgress(VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[0])));
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
		if (method == MethodName.SetCompletedLevelResult)
		{
			return true;
		}
		if (method == MethodName.SaveProgress)
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
