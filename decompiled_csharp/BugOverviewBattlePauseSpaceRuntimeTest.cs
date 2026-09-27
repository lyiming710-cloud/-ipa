using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewBattlePauseSpaceRuntimeTest.cs")]
public class BugOverviewBattlePauseSpaceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Press = "Press";

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

	public override async void _Ready()
	{
		bool multiplayer = Global.Instance.isMultiplayerMode;
		bool editor = Global.Instance.isEditor;
		bool enterBattle = Global.Instance.enterLevelIsBattle;
		Variant background = GameSaveManager.Instance.GetConfigValue("Backgrounder");
		TowerDefenseControlNew control = null;
		FieldInfo stateField = typeof(ResourceManager).GetField("_gameplayResourceLoadState", BindingFlags.Instance | BindingFlags.NonPublic);
		FieldInfo spritePathsField = typeof(ResourceManager).GetField("_characterSpritePaths", BindingFlags.Instance | BindingFlags.NonPublic);
		object resourceState = stateField.GetValue(ResourceManager.Instance);
		object spritePaths = spritePathsField.GetValue(ResourceManager.Instance);
		bool accumulatedInput = Input.UseAccumulatedInput;
		try
		{
			Input.UseAccumulatedInput = false;
			stateField.SetValue(ResourceManager.Instance, GameplayResourceLoadState.Ready);
			spritePathsField.SetValue(ResourceManager.Instance, new Dictionary<string, string> { ["PlantSunFlower"] = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/SunFlower.tscn" });
			ResourceManager.Instance.CHARCTAER_SPRITE["PlantSunFlower"] = GD.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/SunFlower/SunFlower.tscn");
			Global.Instance.isMultiplayerMode = false;
			Global.Instance.isEditor = false;
			Global.Instance.enterLevelIsBattle = false;
			GameSaveManager.Instance.SetConfigValue("Backgrounder", true);
			control = new BugOverviewBattlePauseControlStub
			{
				isGameRunning = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
			AddChild(control, forceReadableName: false, InternalMode.Disabled);
			Check(InputMap.HasAction("Pause"), "The real Pause action must exist.");
			control.ButtonPauseToggled(toggled: true);
			await Frames(1);
			Check(GetTree().Paused && FindDialog<DialogBattlePause>() != null, "The menu pause button must open BattlePause.");
			Press(echo: true);
			Check(GetTree().Paused, "Held-key repeats must not dismiss BattlePause.");
			Press();
			Check(!GetTree().Paused, "A fresh space press must close BattlePause immediately.");
			await Frames(2);
			Check(FindDialog<DialogBoxGamePause>() == null, "The resume press must not reopen pause on the next physics frame.");
			for (int cycle = 0; cycle < 6; cycle++)
			{
				Press();
				Check(GetTree().Paused && FindDialog<DialogBoxGamePause>() != null, $"Rapid cycle {cycle}: space must pause without a cooldown.");
				await Frames(1);
				Press(echo: true);
				Check(GetTree().Paused, $"Rapid cycle {cycle}: echoes must not toggle pause.");
				Press();
				Check(!GetTree().Paused && !control.waitPause, $"Rapid cycle {cycle}: the next distinct press must resume.");
				await Frames(1);
				Check(FindDialog<DialogBoxGamePause>() == null, $"Rapid cycle {cycle}: a consumed resume press must remain consumed.");
			}
			for (int i = 0; i < 4; i++)
			{
				Press();
				Check(GetTree().Paused == (i % 2 == 0), $"Same-frame press {i} must toggle once even with queued dialogs.");
			}
			await Frames(2);
			Check(!GetTree().Paused && FindDialog<DialogBoxGamePause>() == null, "No stale physics polling may re-pause after rapid presses.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewBattlePauseSpaceRuntimeTest] {value}");
		}
		finally
		{
			GetTree().Paused = false;
			Input.UseAccumulatedInput = accumulatedInput;
			stateField.SetValue(ResourceManager.Instance, resourceState);
			spritePathsField.SetValue(ResourceManager.Instance, spritePaths);
			Global.Instance.isMultiplayerMode = multiplayer;
			Global.Instance.isEditor = editor;
			Global.Instance.enterLevelIsBattle = enterBattle;
			GameSaveManager.Instance.SetConfigValue("Backgrounder", background);
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
		}
		bool flag = _failures == 0 && _checks == 34;
		GD.Print($"BATTLE_PAUSE_SPACE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void Press(bool echo = false)
	{
		Input.ParseInputEvent(new InputEventKey
		{
			PhysicalKeycode = Key.Space,
			Unicode = 32L,
			Pressed = true,
			Echo = echo
		});
		if (!echo)
		{
			Input.ParseInputEvent(new InputEventKey
			{
				PhysicalKeycode = Key.Space,
				Unicode = 32L,
				Pressed = false
			});
		}
		Input.FlushBufferedEvents();
	}

	private async Task Frames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private T FindDialog<T>() where T : Node
	{
		foreach (Node child in DialogManager.Instance.GetNode<CanvasLayer>("%DialogLayer").GetChildren())
		{
			if (child is T val && !val.IsQueuedForDeletion())
			{
				return val;
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
			GD.PushError("[BugOverviewBattlePauseSpaceRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(3)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Press, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "echo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Press && args.Count == 1)
		{
			Press(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.Press)
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
