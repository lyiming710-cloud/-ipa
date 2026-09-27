using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/LevelEditorDiyResourcePathMigrationRuntimeTest.cs")]
public class LevelEditorDiyResourcePathMigrationRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildAllMappingsFixture = "BuildAllMappingsFixture";

		public static readonly StringName WriteText = "WriteText";

		public static readonly StringName RemoveProbeFile = "RemoveProbeFile";

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

	private const string SourceLevelPath = "res://Asset/Config/Level/TowerDefense/Test/LevelTest.tres";

	private const string DiyDirectoryPath = "user://Csharp/Diy";

	private const string AllMappingsPath = "user://Csharp/Diy/LevelEditorPathAllMappingsProbe.tres";

	private const string LoadableLevelPath = "user://Csharp/Diy/LevelEditorPathLoadProbe.tres";

	private const string GdOnlyMigrationPath = "user://Csharp/Diy/LevelEditorPathGdOnlyProbe.tres";

	private static readonly KeyValuePair<string, string>[] PathMoves = new KeyValuePair<string, string>[13]
	{
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/ConveyorBelt/Resource/TowerDefenseConveyorConfig.cs", "res://Resource/TowerDefense/Conveyor/TowerDefenseConveyorConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Event/Resource/Map/TowerDefenseLevelEventCurrentMapUseStripe.cs", "res://Resource/TowerDefense/Level/Event/Map/TowerDefenseLevelEventCurrentMapUseStripe.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Fog/Resource/TowerDefenseLevelFogManagerConfig.cs", "res://Resource/TowerDefense/Level/Fog/TowerDefenseLevelFogManagerConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Process/IZM/Resource/TowerDefenseLevelIZMManagerConfig.cs", "res://Resource/TowerDefense/Level/IZM/TowerDefenseLevelIZMManagerConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/LookStar/Resource/TowerDefenseLevelLookStarManagerConfig.cs", "res://Resource/TowerDefense/Level/LookStar/TowerDefenseLevelLookStarManagerConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/SeedBank/Resource/TowerDefenseLevelPacketConfig.cs", "res://Resource/TowerDefense/Level/Packet/TowerDefenseLevelPacketConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/PreSpawn/Resource/TowerDefenseLevelPreSpawnConfig.cs", "res://Resource/TowerDefense/Level/PreSpawn/TowerDefenseLevelPreSpawnConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/RainMode/Resource/TowerDefenseRainModeConfig.cs", "res://Resource/TowerDefense/Level/RainMode/TowerDefenseRainModeConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelSpawnConfig.cs", "res://Resource/TowerDefense/Level/Spawn/TowerDefenseLevelSpawnConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelSpawnDynamicConfig.cs", "res://Resource/TowerDefense/Level/Spawn/TowerDefenseLevelSpawnDynamicConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Sun/Resource/TowerDefenseLevelSunManagerConfig.cs", "res://Resource/TowerDefense/Level/Sun/TowerDefenseLevelSunManagerConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelWaveConfig.cs", "res://Resource/TowerDefense/Level/Wave/TowerDefenseLevelWaveConfig.cs"),
		new KeyValuePair<string, string>("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelWaveManagerConfig.cs", "res://Resource/TowerDefense/Level/Wave/TowerDefenseLevelWaveManagerConfig.cs")
	};

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		try
		{
			Error error = DirAccess.MakeDirRecursiveAbsolute("user://Csharp/Diy");
			Check(error == Error.Ok || DirAccess.DirExistsAbsolute("user://Csharp/Diy"), "本地自制关卡目录必须可用。");
			string text = BuildAllMappingsFixture();
			Check(WriteText("user://Csharp/Diy/LevelEditorPathGdOnlyProbe.tres", text), "旧 GDScript 专用迁移探针必须能够写入。");
			bool flag = ResourceTextScriptMigration.MigrateGdScriptResourceToCSharpIfNeeded("user://Csharp/Diy/LevelEditorPathGdOnlyProbe.tres");
			Check(!flag && FileAccess.GetFileAsString("user://Csharp/Diy/LevelEditorPathGdOnlyProbe.tres").Contains(PathMoves[0].Key), "非关卡编辑器迁移入口不得改写 C# 历史路径。");
			Check(WriteText("user://Csharp/Diy/LevelEditorPathAllMappingsProbe.tres", text), "十三条路径迁移探针必须能够写入。");
			bool condition = LevelEditorStage.MigrateDiyLevelTresIfNeeded("user://Csharp/Diy/LevelEditorPathAllMappingsProbe.tres");
			Check(condition, "本地关卡首次加载必须报告路径已修改。");
			string fileAsString = FileAccess.GetFileAsString("user://Csharp/Diy/LevelEditorPathAllMappingsProbe.tres");
			KeyValuePair<string, string>[] pathMoves = PathMoves;
			for (int i = 0; i < pathMoves.Length; i++)
			{
				KeyValuePair<string, string> keyValuePair = pathMoves[i];
				Check(!fileAsString.Contains(keyValuePair.Key), "迁移后路径必须被删除：" + keyValuePair.Key);
				Check(fileAsString.Contains("path=\"" + keyValuePair.Value + "\""), "稳定路径必须写回文件并移除旧 UID：" + keyValuePair.Value);
			}
			Check(!fileAsString.Contains("uid=\"uid://history"), "十三条历史脚本引用的旧 UID 必须全部移除。");
			Check(!LevelEditorStage.MigrateDiyLevelTresIfNeeded("user://Csharp/Diy/LevelEditorPathAllMappingsProbe.tres"), "已经规范化的本地关卡不得重复写盘。");
			string text2 = FileAccess.GetFileAsString("res://Asset/Config/Level/TowerDefense/Test/LevelTest.tres");
			Check(!string.IsNullOrEmpty(text2), "真实 LevelTest 资源文本必须存在。");
			int num = 0;
			pathMoves = PathMoves;
			for (int i = 0; i < pathMoves.Length; i++)
			{
				KeyValuePair<string, string> keyValuePair2 = pathMoves[i];
				if (text2.Contains(keyValuePair2.Value))
				{
					text2 = text2.Replace(keyValuePair2.Value, keyValuePair2.Key);
					num++;
				}
			}
			Check(num == 8, $"真实 LevelTest 应覆盖八条历史路径，实际为 {num} 条。");
			Check(WriteText("user://Csharp/Diy/LevelEditorPathLoadProbe.tres", text2), "带历史路径的真实关卡副本必须能够写入。");
			Check(LevelEditorStage.MigrateDiyLevelTresIfNeeded("user://Csharp/Diy/LevelEditorPathLoadProbe.tres"), "真实本地关卡副本必须在加载前完成路径迁移。");
			TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>("user://Csharp/Diy/LevelEditorPathLoadProbe.tres", "", ResourceLoader.CacheMode.Ignore);
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig), "迁移后的本地自制关卡必须反序列化为 TowerDefenseLevelConfig。");
			Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig?.fogManager) && GodotObject.IsInstanceValid(towerDefenseLevelConfig?.lookStarManager) && GodotObject.IsInstanceValid(towerDefenseLevelConfig?.sunManager) && GodotObject.IsInstanceValid(towerDefenseLevelConfig?.waveManager), "迁移后的关卡必须保留迷雾、观星、阳光和波次强类型资源。");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[LevelEditorDiyResourcePathMigrationRuntimeTest] 未处理异常：{value}");
		}
		finally
		{
			RemoveProbeFile("user://Csharp/Diy/LevelEditorPathAllMappingsProbe.tres");
			RemoveProbeFile("user://Csharp/Diy/LevelEditorPathLoadProbe.tres");
			RemoveProbeFile("user://Csharp/Diy/LevelEditorPathGdOnlyProbe.tres");
			bool flag2 = _failures == 0 && _checks == 39;
			GD.Print($"LEVEL_EDITOR_DIY_PATH_MIGRATION_RESULT passed={flag2} checks={_checks} failures={_failures}");
			GetTree().Quit((!flag2) ? 2 : 0);
		}
	}

	private static string BuildAllMappingsFixture()
	{
		StringBuilder stringBuilder = new StringBuilder("[gd_resource type=\"Resource\" format=3]\n\n");
		for (int i = 0; i < PathMoves.Length; i++)
		{
			stringBuilder.Append("[ext_resource type=\"Script\" uid=\"uid://history").Append(i).Append("\" path=\"")
				.Append(PathMoves[i].Key)
				.Append("\" id=\"")
				.Append(i + 1)
				.Append("\"]\n");
		}
		return stringBuilder.ToString();
	}

	private static bool WriteText(string path, string text)
	{
		using FileAccess fileAccess = FileAccess.Open(path, FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			return false;
		}
		fileAccess.StoreString(text);
		return true;
	}

	private static void RemoveProbeFile(string path)
	{
		if (FileAccess.FileExists(path))
		{
			DirAccess.RemoveAbsolute(path);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[LevelEditorDiyResourcePathMigrationRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildAllMappingsFixture, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.WriteText, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProbeFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.BuildAllMappingsFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildAllMappingsFixture());
			return true;
		}
		if (method == MethodName.WriteText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(WriteText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RemoveProbeFile && args.Count == 1)
		{
			RemoveProbeFile(VariantUtils.ConvertTo<string>(in args[0]));
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildAllMappingsFixture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildAllMappingsFixture());
			return true;
		}
		if (method == MethodName.WriteText && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(WriteText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RemoveProbeFile && args.Count == 1)
		{
			RemoveProbeFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
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
		if (method == MethodName.BuildAllMappingsFixture)
		{
			return true;
		}
		if (method == MethodName.WriteText)
		{
			return true;
		}
		if (method == MethodName.RemoveProbeFile)
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
