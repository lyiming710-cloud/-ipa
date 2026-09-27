using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/SleepComponentHypnotistFastPathRuntimeTest.cs")]
public class SleepComponentHypnotistFastPathRuntimeTest : Node
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

	private const string CactusScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Cactus/Scene/TowerDefensePlantCactus.tscn";

	private const string HypnotistScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefensePlantCactus cactus = null;
		TowerDefenseZombieHypnotist hypnotist = null;
		try
		{
			_ = 1;
			try
			{
				cactus = Instantiate<TowerDefensePlantCactus>("res://Asset/Anime/Character/Plant/Chapter0/Cactus/Scene/TowerDefensePlantCactus.tscn");
				hypnotist = Instantiate<TowerDefenseZombieHypnotist>("res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn");
				Check(GodotObject.IsInstanceValid(cactus) && GodotObject.IsInstanceValid(hypnotist), "The real Cactus and Hypnotist scenes must instantiate.");
				if (!GodotObject.IsInstanceValid(cactus) || !GodotObject.IsInstanceValid(hypnotist))
				{
					goto end_IL_0048;
				}
				cactus.inGame = false;
				cactus.ProcessMode = ProcessModeEnum.Disabled;
				AddChild(cactus, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				SleepComponent sleep = cactus.componentManager?.GetRuntime<SleepComponent>("character.sleep");
				Check(sleep != null && !sleep.IsReleased, "The real Cactus must bind its production SleepComponent.");
				SleepComponentHypnotistFastPathRuntimeTest sleepComponentHypnotistFastPathRuntimeTest = this;
				int condition;
				if (string.Equals(cactus.config?.sleepTime, "Never", StringComparison.OrdinalIgnoreCase))
				{
					condition = ((sleep != null && !sleep.CanSleep()) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				sleepComponentHypnotistFastPathRuntimeTest.Check((byte)condition != 0, "A Never-configured Cactus without buffs must remain awake.");
				TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff = null;
				foreach (TowerDefenseCharacterEventBase item in hypnotist.rangeEvent)
				{
					if (item is TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff2)
					{
						towerDefenseCharacterEventAddBuff = towerDefenseCharacterEventAddBuff2;
						break;
					}
				}
				Check(towerDefenseCharacterEventAddBuff != null && towerDefenseCharacterEventAddBuff.buffList.Count > 0 && towerDefenseCharacterEventAddBuff.buffList[0] is TowerDefenseCharacterBuffSleep, "The real Hypnotist range event must carry the production Sleep buff.");
				if (sleep == null || sleep.IsReleased || towerDefenseCharacterEventAddBuff == null)
				{
					goto end_IL_0048;
				}
				towerDefenseCharacterEventAddBuff.Execute(Vector2.Zero, cactus);
				await WaitFrames(1);
				Check(cactus.buff.HasActiveBuffs && cactus.buff.BuffHas("Sleep"), "The Hypnotist event must publish the Sleep buff on the Cactus.");
				Check(sleep.CanSleep(), "The Never fast path must yield to the active Hypnotist Sleep buff.");
				Check(cactus.IsSleep() && !cactus.componentAlive, "The real Never-configured Cactus must sleep after Hypnotist applies the buff.");
				cactus.buff.DeleteBuff("Sleep");
				sleep.SleepProcessing(0f);
				Check(!cactus.IsSleep() && cactus.componentAlive, "Removing the Hypnotist Sleep buff must restore the awake state.");
				goto end_IL_0036;
				end_IL_0048:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[SleepComponentHypnotistFastPathRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0036;
			}
			return;
			end_IL_0036:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(cactus))
			{
				cactus.QueueFree();
			}
			if (GodotObject.IsInstanceValid(hypnotist))
			{
				hypnotist.QueueFree();
			}
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 8;
		GD.Print($"SLEEP_COMPONENT_HYPNOTIST_FAST_PATH_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T Instantiate<T>(string scenePath) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
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
			GD.PushError("[SleepComponentHypnotistFastPathRuntimeTest] " + message);
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
