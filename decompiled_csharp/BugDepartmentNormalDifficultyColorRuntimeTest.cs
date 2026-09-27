using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentNormalDifficultyColorRuntimeTest.cs")]
public class BugDepartmentNormalDifficultyColorRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsColor = "IsColor";

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

	private const string ProgressManagerPath = "res://Registry/Battle/Feature/Progress/Manager/TowerDefenseProgressManager.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseProgressManager progress = null;
		try
		{
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/Progress/Manager/TowerDefenseProgressManager.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The production progress-manager scene must load.");
				if (!GodotObject.IsInstanceValid(packedScene))
				{
					goto end_IL_0037;
				}
				progress = packedScene.Instantiate<TowerDefenseProgressManager>(PackedScene.GenEditState.Disabled);
				AddChild(progress, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				Check(GodotObject.IsInstanceValid(progress.difficultLabel), "The real progress manager must expose DifficultLabel.");
				progress.SetupUI("Normal", "Runtime difficulty color test");
				Check(IsColor(progress.difficultLabel.Modulate, 1f, 1f, 0f, 1f), $"Normal difficulty must render as yellow; got {progress.difficultLabel.Modulate}.");
				Check(!string.IsNullOrWhiteSpace(progress.difficultLabel.Text), "The real Normal setup path must also populate the difficulty label text.");
				progress.difficultLabel.Modulate = new Color(1f, 0f, 0f, 0.35f);
				TowerDefenseBattleFeatureProgress towerDefenseBattleFeatureProgress = new TowerDefenseBattleFeatureProgress();
				towerDefenseBattleFeatureProgress.progressManager = progress;
				towerDefenseBattleFeatureProgress.SetDifficultModulate("Normal");
				Check(IsColor(progress.difficultLabel.Modulate, 1f, 1f, 0f, 0.35f), $"Feature-driven Normal difficulty must be yellow and preserve alpha; got {progress.difficultLabel.Modulate}.");
				towerDefenseBattleFeatureProgress.SetDifficultModulate("Difficult");
				Check(IsColor(progress.difficultLabel.Modulate, 1f, 0f, 0f, 0.35f), "Difficult must remain red while preserving label alpha.");
				towerDefenseBattleFeatureProgress.SetDifficultModulate("Ultimate");
				Check(IsColor(progress.difficultLabel.Modulate, 1f, 0f, 0f, 0.35f), "Ultimate must remain red while preserving label alpha.");
				goto end_IL_002e;
				end_IL_0037:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentNormalDifficultyColorRuntimeTest] Unexpected exception: {value}");
				goto end_IL_002e;
			}
			return;
			end_IL_002e:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(progress))
			{
				progress.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0 && _checks == 7;
		GD.Print($"NORMAL_DIFFICULTY_COLOR_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static bool IsColor(Color color, float red, float green, float blue, float alpha)
	{
		if (Mathf.IsEqualApprox(color.R, red) && Mathf.IsEqualApprox(color.G, green) && Mathf.IsEqualApprox(color.B, blue))
		{
			return Mathf.IsEqualApprox(color.A, alpha);
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugDepartmentNormalDifficultyColorRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsColor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "red", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "green", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "blue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "alpha", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsColor && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(IsColor(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4])));
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
		if (method == MethodName.IsColor && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(IsColor(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4])));
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
		if (method == MethodName.IsColor)
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
