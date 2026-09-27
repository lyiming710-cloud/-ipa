using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/QuizSuperBigMapAvailabilityRuntimeTest.cs")]
public class QuizSuperBigMapAvailabilityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string QuizScenePath = "res://Prefab/GUI/LevelEditor/Quiz/LevelEditorQuiz.tscn";

	private const string DisabledMapName = "FrontlawnSuperBig";

	private const string RegularMapName = "Frontlawn";

	private const int ExpectedChecks = 7;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		LevelEditorQuiz quiz = null;
		Dictionary<string, Resource> originalMaps = null;
		TowerDefenseMapConfig regularMapConfig = null;
		TowerDefenseMapConfig disabledMapConfig = null;
		try
		{
			try
			{
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "地图资源管理器必须有效。");
				if (!GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_004c;
				}
				originalMaps = ResourceManager.Instance.MAPS;
				regularMapConfig = new TowerDefenseMapConfig
				{
					translate = "MAP_FRONTLAWN"
				};
				disabledMapConfig = new TowerDefenseMapConfig
				{
					translate = "MAP_FRONTLAWN_SUPER_BIG"
				};
				ResourceManager.Instance.MAPS = new Dictionary<string, Resource>
				{
					["Frontlawn"] = regularMapConfig,
					["FrontlawnSuperBig"] = disabledMapConfig
				};
				Check(ResourceManager.Instance.MAPS.ContainsKey("FrontlawnSuperBig"), "超级大地图必须继续保留在全局地图注册表中，禁用范围只能是 Quiz。");
				quiz = ResourceLoader.Load<PackedScene>("res://Prefab/GUI/LevelEditor/Quiz/LevelEditorQuiz.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<LevelEditorQuiz>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(quiz), "真实 Quiz 选图界面必须能够实例化。");
				if (!GodotObject.IsInstanceValid(quiz))
				{
					goto end_IL_004c;
				}
				AddChild(quiz, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(1);
				DragMenu node = quiz.GetNode<DragMenu>("%QuizDragMenu");
				HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
				bool flag = true;
				foreach (Node child in node.GetChildren())
				{
					if (!(child is LevelEditorQuizLevelItem levelEditorQuizLevelItem) || !ResourceManager.Instance.MAPS.ContainsKey(levelEditorQuizLevelItem.map) || !hashSet.Add(levelEditorQuizLevelItem.map))
					{
						flag = false;
					}
				}
				Check(node.GetChildCount() == ResourceManager.Instance.MAPS.Count - 1, $"Quiz 候选地图数量必须只比全局地图少一张，候选={node.GetChildCount()}，全局={ResourceManager.Instance.MAPS.Count}。");
				Check(!hashSet.Contains("FrontlawnSuperBig"), "Quiz 随机地图候选集中不能出现超级大地图。");
				Check(hashSet.Contains("Frontlawn"), "Quiz 随机地图候选集必须继续包含普通白天地图。");
				Check(flag && hashSet.Count == node.GetChildCount(), "Quiz 候选项必须全部来自地图注册表且不能重复。");
				goto end_IL_0043;
				end_IL_004c:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[QuizSuperBigMapAvailabilityRuntimeTest] 未处理异常：{value}");
				goto end_IL_0043;
			}
			return;
			end_IL_0043:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(quiz))
			{
				quiz.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (GodotObject.IsInstanceValid(ResourceManager.Instance) && originalMaps != null)
			{
				ResourceManager.Instance.MAPS = originalMaps;
			}
			regularMapConfig?.Dispose();
			disabledMapConfig?.Dispose();
		}
		bool flag2 = _failures == 0 && _checks == 7;
		GD.Print($"QUIZ_SUPER_BIG_MAP_AVAILABILITY_RESULT passed={flag2} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag2) ? 2 : 0);
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[QuizSuperBigMapAvailabilityRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
