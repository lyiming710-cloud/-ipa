using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ExplodeComponentAwakeDeathRuntimeTest.cs")]
public class ExplodeComponentAwakeDeathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PreparePlant = "PreparePlant";

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

	private const string CherryBombScenePath = "res://Asset/Anime/Character/Plant/Chapter0/CherryBomb/Scene/TowerDefensePlantCherryBomb.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefensePlant awakePlant = null;
		TowerDefensePlant sleepingPlant = null;
		TowerDefensePlant shoveledPlant = null;
		try
		{
			_ = 1;
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/CherryBomb/Scene/TowerDefensePlantCherryBomb.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The real Cherry Bomb scene must load.");
				awakePlant = packedScene?.Instantiate<TowerDefensePlant>(PackedScene.GenEditState.Disabled);
				sleepingPlant = packedScene?.Instantiate<TowerDefensePlant>(PackedScene.GenEditState.Disabled);
				shoveledPlant = packedScene?.Instantiate<TowerDefensePlant>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(awakePlant) && GodotObject.IsInstanceValid(sleepingPlant) && GodotObject.IsInstanceValid(shoveledPlant), "All three real Cherry Bomb fixtures must instantiate.");
				if (!GodotObject.IsInstanceValid(awakePlant) || !GodotObject.IsInstanceValid(sleepingPlant) || !GodotObject.IsInstanceValid(shoveledPlant))
				{
					goto end_IL_004f;
				}
				PreparePlant(awakePlant, "AwakeDeathCherryBomb");
				PreparePlant(sleepingPlant, "SleepingDeathCherryBomb");
				PreparePlant(shoveledPlant, "ShoveledCherryBomb");
				AddChild(awakePlant, forceReadableName: false, InternalMode.Disabled);
				AddChild(sleepingPlant, forceReadableName: false, InternalMode.Disabled);
				AddChild(shoveledPlant, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				ExplodeComponent explode = GetExplode(awakePlant);
				ExplodeComponent explode2 = GetExplode(sleepingPlant);
				ExplodeComponent explode3 = GetExplode(shoveledPlant);
				Check(explode != null && !explode.IsReleased && explode2 != null && !explode2.IsReleased && explode3 != null && !explode3.IsReleased, "Every fixture must bind the production ExplodeComponent runtime.");
				if (explode == null || explode.IsReleased || explode2 == null || explode2.IsReleased || explode3 == null || explode3.IsReleased)
				{
					goto end_IL_004f;
				}
				PrepareExplode(explode);
				PrepareExplode(explode2);
				PrepareExplode(explode3);
				Check(explode.reload && explode2.reload && explode3.reload, "All fixtures must begin with one pending explosion.");
				int awakeStarts = 0;
				int awakeCallbacks = 0;
				int sleepingStarts = 0;
				int sleepingCallbacks = 0;
				int shoveledStarts = 0;
				int shoveledCallbacks = 0;
				explode.OnExplodeStart += () =>
				{
					awakeStarts++;
				};
				explode.OnExplode += () =>
				{
					awakeCallbacks++;
				};
				explode2.OnExplodeStart += () =>
				{
					sleepingStarts++;
				};
				explode2.OnExplode += () =>
				{
					sleepingCallbacks++;
				};
				explode3.OnExplodeStart += () =>
				{
					shoveledStarts++;
				};
				explode3.OnExplode += () =>
				{
					shoveledCallbacks++;
				};
				awakePlant.instance.sleep = false;
				sleepingPlant.instance.sleep = true;
				sleepingPlant.componentAlive = false;
				shoveledPlant.instance.sleep = false;
				Check(!awakePlant.instance.sleep && sleepingPlant.instance.sleep && !shoveledPlant.instance.sleep, "The regression must distinguish awake death, sleeping death, and shovel removal.");
				double num = awakePlant.SkipInvincibleHurt(awakePlant.instance.hitpoints + 1.0, playSplatAudio: false);
				Check(num > 0.0 && awakePlant.instance.hitpoints <= 0.0 && awakePlant.die && awakePlant.instance.die, "The awake fixture must enter the real zero-health death path.");
				Check(awakeStarts == 1 && awakeCallbacks == 1, $"Awake death must commit exactly one explosion; starts={awakeStarts}, callbacks={awakeCallbacks}.");
				Check(!explode.reload, "Awake death must consume the pending explosion.");
				awakePlant.Destroy();
				Check(awakeStarts == 1 && awakeCallbacks == 1, "Repeated destroy entry must not duplicate the death explosion.");
				double num2 = sleepingPlant.SkipInvincibleHurt(sleepingPlant.instance.hitpoints + 1.0, playSplatAudio: false);
				Check(num2 > 0.0 && sleepingPlant.instance.hitpoints <= 0.0 && sleepingPlant.die && sleepingPlant.instance.die, "The sleeping fixture must enter the same real zero-health death path.");
				Check(sleepingStarts == 0 && sleepingCallbacks == 0, "Sleeping death must not commit an explosion.");
				Check(explode2.reload, "Sleeping death must leave its pending explosion unconsumed.");
				shoveledPlant.ShovelDestroy();
				Check(shoveledPlant.isShovel && shoveledPlant.isDestroy, "The control fixture must use the real shovel-removal path.");
				Check(shoveledStarts == 0 && shoveledCallbacks == 0, "Shovel removal must not be mistaken for a zero-health death.");
				Check(explode3.reload, "Shovel removal must leave its pending explosion unconsumed.");
				Check(awakePlant.isDestroy && sleepingPlant.isDestroy, "Both zero-health fixtures must continue through normal destruction.");
				await WaitFrames(4);
				Check(!GodotObject.IsInstanceValid(awakePlant) && !GodotObject.IsInstanceValid(sleepingPlant) && !GodotObject.IsInstanceValid(shoveledPlant), "All fixtures must finish their normal destruction lifecycle.");
				goto end_IL_003d;
				end_IL_004f:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[ExplodeComponentAwakeDeath] Unexpected exception: {value}");
				goto end_IL_003d;
			}
			return;
			end_IL_003d:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(awakePlant))
			{
				awakePlant.QueueFree();
			}
			if (GodotObject.IsInstanceValid(sleepingPlant))
			{
				sleepingPlant.QueueFree();
			}
			if (GodotObject.IsInstanceValid(shoveledPlant))
			{
				shoveledPlant.QueueFree();
			}
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 17;
		GD.Print($"EXPLODE_COMPONENT_AWAKE_DEATH_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static void PreparePlant(TowerDefensePlant plant, string name)
	{
		plant.Name = name;
		plant.inGame = false;
		plant.ProcessMode = ProcessModeEnum.Disabled;
	}

	private static ExplodeComponent GetExplode(TowerDefensePlant plant)
	{
		return plant.componentManager?.GetRuntime<ExplodeComponent>("character.explode");
	}

	private static void PrepareExplode(ExplodeComponent explode)
	{
		explode.cameraShakeUse = false;
		explode.screenColorBlinkUse = false;
		explode.explodeEffect = null;
		explode.explodeAudio = "";
		explode.reverseAudio = "";
		explode.explodeMethod = "";
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
			GD.PushError("[ExplodeComponentAwakeDeath] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreparePlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
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
		if (method == MethodName.PreparePlant && args.Count == 2)
		{
			PreparePlant(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.PreparePlant && args.Count == 2)
		{
			PreparePlant(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.PreparePlant)
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
