using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewAngelReviveRuntimeTest.cs")]
public class BugOverviewAngelReviveRuntimeTest : Node
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

	private const string AngelScenePath = "res://Asset/Anime/Character/Zombie/Chapter4/Angel/Scene/TowerDefenseZombieAngel.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		AngelReviveRuntimeControlStub control = null;
		TowerDefenseZombieAngel angel = null;
		TowerDefenseZombieAngel spawnedReviveAngel = null;
		try
		{
			_ = 3;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_007b;
				}
				control = new AngelReviveRuntimeControlStub
				{
					Name = "AngelReviveRuntimeControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				Node2D characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(characterNode, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = characterNode;
				manager.currentControl = control;
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter4/Angel/Scene/TowerDefenseZombieAngel.tscn", null, ResourceLoader.CacheMode.Ignore);
				if (GodotObject.IsInstanceValid(packedScene))
				{
					ResourceManager.Instance.TOWERDEFENSE_CHARCATERS["ZombieAngel"] = packedScene;
				}
				angel = packedScene?.Instantiate<TowerDefenseZombieAngel>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(angel), "The real Angel Zombie scene must instantiate.");
				if (!GodotObject.IsInstanceValid(angel))
				{
					goto end_IL_007b;
				}
				angel.inGame = true;
				angel.editorPreviewMode = false;
				angel.isRevive = true;
				angel.GlobalPosition = new Vector2(500f, 300f);
				characterNode.AddChild(angel, forceReadableName: false, InternalMode.Disabled);
				Check(angel.StateMachine?.IsInitialized ?? false, "The real Angel Zombie state machine must initialize in _Ready.");
				Check(angel.CurrentStateHandle == null, "The regression fixture must call Walk before deferred initial-state entry.");
				angel.Walk();
				await WaitFrames(4);
				Check(angel.CurrentStateHandle?.StableId == "zombie.angel.revive", "A revived Angel must enter its revive state; got " + (angel.CurrentStateHandle?.StableId ?? "<null>") + ".");
				Check(angel.isReviveOver, "The revive-complete gate must only latch after the revive transition succeeds.");
				Check(angel.invincible, "The revived Angel role must enter its timed invincible phase.");
				Check(angel.instance?.invincible ?? false, "The revived Angel damage instance must reject hits during revival.");
				BugOverviewAngelReviveRuntimeTest bugOverviewAngelReviveRuntimeTest = this;
				TowerDefenseCharacterInstance instance = angel.instance;
				bugOverviewAngelReviveRuntimeTest.Check(instance != null && instance.unUseBuffFlags == -1, "The revived Angel must ignore all buffs during the revive phase.");
				Check(angel.sprite?.clip == "Revive", "The revived Angel must visibly play its authored Revive animation.");
				float reviveEndStartX = angel.GlobalPosition.X;
				await WaitFrames(90);
				Check(angel.CurrentStateHandle?.StableId == "zombie.walk", "A revived Angel must return to Walk after Revive completes; got " + (angel.CurrentStateHandle?.StableId ?? "<null>") + ".");
				Check(angel.sprite?.clip == "Idle", "A revived Angel must restore its walk animation; got " + (angel.sprite?.clip ?? "<null>") + ".");
				Check(angel.GlobalPosition.X < reviveEndStartX - 0.1f, $"A revived Angel must move left after Revive completes; start={reviveEndStartX}, end={angel.GlobalPosition.X}.");
				TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Chapter4/Angel/Packet/ZombieAngel.tres", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(towerDefensePacketConfig), "The real Angel packet must load for the self-revival path.");
				if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
				{
					goto end_IL_007b;
				}
				angel.packet = towerDefensePacketConfig;
				angel.CreateSelf(new Vector2(620f, 300f));
				await WaitFrames(8);
				foreach (Node child in characterNode.GetChildren())
				{
					if (child is TowerDefenseZombieAngel towerDefenseZombieAngel && towerDefenseZombieAngel != angel)
					{
						spawnedReviveAngel = towerDefenseZombieAngel;
						break;
					}
				}
				Check(GodotObject.IsInstanceValid(spawnedReviveAngel), "CreateSelf must add the replacement Angel to the battlefield.");
				if (!GodotObject.IsInstanceValid(spawnedReviveAngel))
				{
					goto end_IL_007b;
				}
				Check(spawnedReviveAngel.CurrentStateHandle?.StableId == "zombie.angel.revive", "The replacement Angel must enter Revive; got " + (spawnedReviveAngel.CurrentStateHandle?.StableId ?? "<null>") + ".");
				float spawnedReviveStartX = spawnedReviveAngel.GlobalPosition.X;
				await WaitFrames(90);
				Check(spawnedReviveAngel.CurrentStateHandle?.StableId == "zombie.walk", "The replacement Angel must return to Walk; got " + (spawnedReviveAngel.CurrentStateHandle?.StableId ?? "<null>") + ".");
				Check(spawnedReviveAngel.GlobalPosition.X < spawnedReviveStartX - 0.1f, $"The replacement Angel must move left after Revive; start={spawnedReviveStartX}, end={spawnedReviveAngel.GlobalPosition.X}.");
				goto end_IL_0060;
				end_IL_007b:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewAngelReviveRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0060;
			}
			return;
			end_IL_0060:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(angel))
			{
				angel.QueueFree();
			}
			if (GodotObject.IsInstanceValid(spawnedReviveAngel))
			{
				spawnedReviveAngel.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 18;
		GD.Print($"ANGEL_REVIVE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
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
			GD.PushError("[BugOverviewAngelReviveRuntimeTest] " + message);
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
