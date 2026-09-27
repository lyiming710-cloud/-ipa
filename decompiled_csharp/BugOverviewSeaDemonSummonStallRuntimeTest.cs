using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSeaDemonSummonStallRuntimeTest.cs")]
public class BugOverviewSeaDemonSummonStallRuntimeTest : Node
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

	private const string ScenePath = "res://Asset/Anime/Character/Zombie/Chapter3/SeaDemon/Scene/Base/TowerDefenseZombieSeaDemon.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombieSeaDemon zombie = null;
		try
		{
			_ = 4;
			try
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter3/SeaDemon/Scene/Base/TowerDefenseZombieSeaDemon.tscn", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(packedScene), "The real Sea Demon scene must load.");
				zombie = packedScene?.Instantiate<TowerDefenseZombieSeaDemon>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(zombie), "The real Sea Demon must instantiate.");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_004e;
				}
				zombie.editorPreviewMode = true;
				zombie.inGame = false;
				AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				Check(zombie.StateMachine?.IsInitialized ?? false, "The real Sea Demon state machine must initialize.");
				Check(GodotObject.IsInstanceValid(zombie.sprite) && zombie.sprite.HasClip("Point"), "The real Sea Demon Point clip must be available.");
				zombie.Walk();
				await WaitFrames(1);
				Check(zombie.CurrentStateHandle?.StableId == "zombie.walk", "Fixture must start from walk; current=" + zombie.CurrentStateHandle?.StableId + ".");
				Check(zombie.SendStateEvent("ToPoint"), "The authored ToPoint transition must be accepted.");
				Check(zombie.CurrentStateHandle?.StableId == "zombie.sea_demon.point" && zombie.isPointing && zombie.sprite.clip == "Point", $"Fixture must enter the real Point state; current={zombie.CurrentStateHandle?.StableId}, isPointing={zombie.isPointing}, clip={zombie.sprite.clip}.");
				zombie.spawnNext = true;
				zombie.AnimeCompleted("Point");
				await WaitFrames(1);
				Check(zombie.CurrentStateHandle?.StableId == "zombie.walk" && !zombie.isPointing && zombie.spawnNext, $"Point completion must return to walk without consuming overlapping summon readiness; current={zombie.CurrentStateHandle?.StableId}, isPointing={zombie.isPointing}, spawnNext={zombie.spawnNext}.");
				zombie.AnimeCompleted("Walk");
				await WaitFrames(1);
				Check(zombie.CurrentStateHandle?.StableId == "zombie.sea_demon.point" && zombie.isPointing && !zombie.spawnNext, $"The retained summon readiness must retry from locomotion; current={zombie.CurrentStateHandle?.StableId}, isPointing={zombie.isPointing}, spawnNext={zombie.spawnNext}.");
				zombie.AnimeCompleted("Point");
				await WaitFrames(1);
				Check(zombie.CurrentStateHandle?.StableId == "zombie.walk" && !zombie.isPointing, $"A completed summon must recover locomotion; current={zombie.CurrentStateHandle?.StableId}, isPointing={zombie.isPointing}.");
				goto end_IL_002f;
				end_IL_004e:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewSeaDemonSummonStallRuntimeTest] Unexpected exception: {value}");
				goto end_IL_002f;
			}
			return;
			end_IL_002f:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 10;
		GD.Print($"SEA_DEMON_SUMMON_STALL_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[BugOverviewSeaDemonSummonStallRuntimeTest] " + message);
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
