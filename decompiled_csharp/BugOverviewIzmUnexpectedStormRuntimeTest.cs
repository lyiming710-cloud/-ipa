using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewIzmUnexpectedStormRuntimeTest.cs")]
public class BugOverviewIzmUnexpectedStormRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName JsonScreenEffectIsClear = "JsonScreenEffectIsClear";

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

	private static readonly int[] AffectedLevels = new int[5] { 31, 32, 35, 36, 38 };

	private const string LevelDirectory = "res://Asset/Config/Level/TowerDefense/IZM";

	private const string ScreenEffectControlPath = "res://Registry/Battle/Feature/ScreenEffect/ScreenEffectControl/ScreenEffectControl.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		IzmUnexpectedStormRuntimeControlStub control = null;
		try
		{
			control = new IzmUnexpectedStormRuntimeControlStub
			{
				Name = "IzmUnexpectedStormRuntimeControl",
				isGameRunning = false,
				isInit = true
			};
			AddChild(control, forceReadableName: false, InternalMode.Disabled);
			Check(GodotObject.IsInstanceValid(control) && control.IsInsideTree(), "The focused scene must mount a production TowerDefenseControlNew host.");
			int[] affectedLevels = AffectedLevels;
			foreach (int levelNumber in affectedLevels)
			{
				await VerifyLevel(control, levelNumber);
			}
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewIzmUnexpectedStormRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 41;
		GD.Print($"IZM_UNEXPECTED_STORM_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyLevel(IzmUnexpectedStormRuntimeControlStub control, int levelNumber)
	{
		string text = $"{"res://Asset/Config/Level/TowerDefense/IZM"}/IZMLevel{levelNumber}.tres";
		string jsonPath = $"{"res://Asset/Config/Level/TowerDefense/IZM"}/IZMLevel{levelNumber}.json";
		TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>(text, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && towerDefenseLevelConfig.ResourcePath == text && towerDefenseLevelConfig.name == $"IZM_Level{levelNumber}", $"IZM {levelNumber} must load its real production level resource.");
		if (!GodotObject.IsInstanceValid(towerDefenseLevelConfig))
		{
			throw new InvalidOperationException($"IZM {levelNumber} resource did not load.");
		}
		Check(towerDefenseLevelConfig.processName == new StringName("IZM") && !towerDefenseLevelConfig.featureData.ContainsKey(new StringName("RainMode")), $"IZM {levelNumber} must retain the IZM process without a RainMode feature.");
		bool flag = towerDefenseLevelConfig.featureData.TryGetValue(new StringName("ScreenEffect"), out var value);
		Check(flag && value != null, $"IZM {levelNumber} must expose its authored ScreenEffect data.");
		if (!flag || value == null)
		{
			throw new InvalidOperationException($"IZM {levelNumber} has no ScreenEffect data.");
		}
		Check(!value.GetValueOrDefault("StormOpen", false).AsBool(), $"IZM {levelNumber} must not request the storm weather pair.");
		string a = value.GetValueOrDefault("PacketBankMethod", "NOONE").AsString();
		Check(!string.Equals(a, "RAIN", StringComparison.OrdinalIgnoreCase), $"IZM {levelNumber} must not request rain through PacketBankMethod.");
		Check(JsonScreenEffectIsClear(jsonPath), $"IZM {levelNumber} JSON source must agree with the runtime resource and keep StormOpen false.");
		TowerDefenseBattleFeatureScreenEffect feature = new TowerDefenseBattleFeatureScreenEffect
		{
			control = control
		};
		feature.Init(value.Duplicate(deep: true));
		await WaitFrames(1);
		Check(GodotObject.IsInstanceValid(feature.screenEffectControl) && feature.screenEffectControl.SceneFilePath == "res://Registry/Battle/Feature/ScreenEffect/ScreenEffectControl/ScreenEffectControl.tscn", $"IZM {levelNumber} must exercise the real ScreenEffect production container.");
		Check(!feature.HasScreenEffect("Rain") && !feature.HasScreenEffect("Storm") && feature.screenEffectControl.GetChildCount() == 0, $"IZM {levelNumber} must mount neither the real Rain nor Storm effect node.");
		feature.Destroy();
		await WaitFrames(2);
	}

	private static bool JsonScreenEffectIsClear(string jsonPath)
	{
		Variant variant = Json.ParseString(FileAccess.GetFileAsString(jsonPath));
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		foreach (Variant item in variant.AsGodotDictionary().GetValueOrDefault("Feature", new Godot.Collections.Array()).AsGodotArray())
		{
			if (item.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary = item.AsGodotDictionary();
				if (string.Equals(dictionary.GetValueOrDefault("Name", "").AsString(), "ScreenEffect", StringComparison.Ordinal))
				{
					return !dictionary.GetValueOrDefault("Data", new Dictionary()).AsGodotDictionary().GetValueOrDefault("StormOpen", false)
						.AsBool();
				}
			}
		}
		return false;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
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
			GD.PushError("[BugOverviewIzmUnexpectedStormRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JsonScreenEffectIsClear, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "jsonPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.JsonScreenEffectIsClear && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(JsonScreenEffectIsClear(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.JsonScreenEffectIsClear && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(JsonScreenEffectIsClear(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.JsonScreenEffectIsClear)
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
