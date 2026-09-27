using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewPogoCorpseCleanupRuntimeTest.cs")]
public class BugOverviewPogoCorpseCleanupRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PreparePogo = "PreparePogo";

		public static readonly StringName EnterRealDeath = "EnterRealDeath";

		public static readonly StringName DescribeCorpse = "DescribeCorpse";

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

	private const string PogoScenePath = "res://Asset/Anime/Character/Zombie/Chapter4/Pogo/Scene/TowerDefenseZombiePogo.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewPogoCorpseCleanupControlStub control = null;
		TowerDefenseZombiePogo armed = null;
		TowerDefenseZombiePogo disarmed = null;
		try
		{
			_ = 4;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					return;
				}
				control = new BugOverviewPogoCorpseCleanupControlStub
				{
					Name = "PogoCorpseCleanupControl",
					isGameRunning = true,
					isInit = true
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter4/Pogo/Scene/TowerDefenseZombiePogo.tscn", null, ResourceLoader.CacheMode.Reuse);
				armed = packedScene?.Instantiate<TowerDefenseZombiePogo>(PackedScene.GenEditState.Disabled);
				disarmed = packedScene?.Instantiate<TowerDefenseZombiePogo>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(armed) && GodotObject.IsInstanceValid(disarmed), "The real Pogo zombie scene must instantiate both death variants.");
				if (!GodotObject.IsInstanceValid(armed) || !GodotObject.IsInstanceValid(disarmed))
				{
					return;
				}
				PreparePogo(armed, new Vector2I(5, 2), new Vector2(500f, 252f));
				PreparePogo(disarmed, new Vector2I(4, 2), new Vector2(400f, 252f));
				control.characterNode.AddChild(armed, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(disarmed, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				Check(armed.config?.name == "ZombiePogo" && (armed.sprite?.HasClip("Pogo") ?? false) && armed.sprite.HasClip("Death"), "The fixture must use the authored Pogo and Death clips from the production zombie.");
				BugOverviewPogoCorpseCleanupRuntimeTest bugOverviewPogoCorpseCleanupRuntimeTest = this;
				ZombieDeathComponent zombieDeathComponent = armed.zombieDeathComponent;
				int condition;
				if (zombieDeathComponent != null && !zombieDeathComponent.IsReleased)
				{
					ZombieDeathComponent zombieDeathComponent2 = disarmed.zombieDeathComponent;
					condition = ((zombieDeathComponent2 != null && !zombieDeathComponent2.IsReleased) ? 1 : 0);
				}
				else
				{
					condition = 0;
				}
				bugOverviewPogoCorpseCleanupRuntimeTest.Check((byte)condition != 0, "Both real Pogo variants must bind the shared ZombieDeath runtime.");
				armed.zombieDeathComponent.dropFeatureName = "";
				disarmed.zombieDeathComponent.dropFeatureName = "";
				Check(armed.hasPogo && (armed.instance?.ArmorHas("Pogo") ?? false), "The armed variant must begin with its real pogo armor.");
				disarmed.instance.ArmorDelete("Pogo");
				disarmed.hasPogo = false;
				disarmed.isJump = false;
				disarmed.Walk();
				await WaitFrames(2);
				BugOverviewPogoCorpseCleanupRuntimeTest bugOverviewPogoCorpseCleanupRuntimeTest2 = this;
				int condition2;
				if (!disarmed.hasPogo)
				{
					TowerDefenseCharacterInstance instance = disarmed.instance;
					if (instance != null && !instance.ArmorHas("Pogo"))
					{
						condition2 = ((disarmed.CurrentStateHandle?.StableId == "zombie.walk") ? 1 : 0);
						goto IL_0527;
					}
				}
				condition2 = 0;
				goto IL_0527;
				IL_0527:
				bugOverviewPogoCorpseCleanupRuntimeTest2.Check((byte)condition2 != 0, "The disarmed variant must exercise the real post-pogo walking state.");
				EnterRealDeath(armed);
				EnterRealDeath(disarmed);
				await WaitFrames(2);
				Check(armed.die && armed.nearDie && armed.CurrentStateHandle?.StableId == "zombie.die" && armed.zombieDeathComponent.IsDeathAnimationClip(armed.sprite.clip), $"Armed Pogo must enter one real death animation; state={armed.CurrentStateHandle?.StableId}, clip={armed.sprite?.clip}.");
				Check(disarmed.die && disarmed.nearDie && disarmed.CurrentStateHandle?.StableId == "zombie.die" && disarmed.zombieDeathComponent.IsDeathAnimationClip(disarmed.sprite.clip), $"Disarmed Pogo must enter one real death animation; state={disarmed.CurrentStateHandle?.StableId}, clip={disarmed.sprite?.clip}.");
				string clip = armed.sprite.clip;
				string clip2 = disarmed.sprite.clip;
				armed.AnimeCompleted("Pogo");
				disarmed.AnimeCompleted("Idle");
				Check(armed.CurrentStateHandle?.StableId == "zombie.die" && armed.sprite.clip == clip, "A queued Pogo completion must not restart an armed corpse's death animation.");
				Check(disarmed.CurrentStateHandle?.StableId == "zombie.die" && disarmed.sprite.clip == clip2, "A queued post-pogo Idle completion must not restart a disarmed corpse's death animation.");
				await WaitSeconds(4.5);
				await WaitFrames(3);
				Check(!GodotObject.IsInstanceValid(armed), "An armed Pogo corpse must be removed after its authored Death clip and shared fade complete; " + DescribeCorpse(armed));
				Check(!GodotObject.IsInstanceValid(disarmed), "A disarmed Pogo corpse must be removed after its authored Death clip and shared fade complete; " + DescribeCorpse(disarmed));
				goto end_IL_00d5;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewPogoCorpseCleanupRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00d5;
			}
			end_IL_00d5:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(armed))
			{
				armed.QueueFree();
			}
			if (GodotObject.IsInstanceValid(disarmed))
			{
				disarmed.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitSeconds(1.0);
			await WaitFrames(3);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			await WaitFrames(3);
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 12;
		GD.Print($"POGO_CORPSE_CLEANUP_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static void PreparePogo(TowerDefenseZombiePogo pogo, Vector2I gridPos, Vector2 position)
	{
		pogo.editorPreviewMode = false;
		pogo.inGame = true;
		pogo.gridPos = gridPos;
		pogo.GlobalPosition = position;
	}

	private static void EnterRealDeath(TowerDefenseZombiePogo pogo)
	{
		pogo.instance.SkipInvincibleDealHurt(pogo.instance.hitpoints + 1.0, playSplatAudio: false, default, createDamagePart: false);
	}

	private static string DescribeCorpse(TowerDefenseZombiePogo pogo)
	{
		if (!GodotObject.IsInstanceValid(pogo))
		{
			return "freed";
		}
		return $"state={pogo.CurrentStateHandle?.StableId}, clip={pogo.sprite?.clip}, frame={pogo.sprite?.frameIndex}, loop={pogo.sprite?.loop}, spriteTimeScale={pogo.sprite?.timeScale}, characterTimeScale={pogo.timeScale}, pause={pogo.sprite?.pause}, alpha={pogo.Modulate.A}, destroy={pogo.isDestroy}";
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task WaitSeconds(double seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewPogoCorpseCleanupRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreparePogo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pogo", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnterRealDeath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pogo", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DescribeCorpse, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pogo", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.PreparePogo && args.Count == 3)
		{
			PreparePogo(VariantUtils.ConvertTo<TowerDefenseZombiePogo>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterRealDeath && args.Count == 1)
		{
			EnterRealDeath(VariantUtils.ConvertTo<TowerDefenseZombiePogo>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DescribeCorpse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeCorpse(VariantUtils.ConvertTo<TowerDefenseZombiePogo>(in args[0])));
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
		if (method == MethodName.PreparePogo && args.Count == 3)
		{
			PreparePogo(VariantUtils.ConvertTo<TowerDefenseZombiePogo>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterRealDeath && args.Count == 1)
		{
			EnterRealDeath(VariantUtils.ConvertTo<TowerDefenseZombiePogo>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DescribeCorpse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeCorpse(VariantUtils.ConvertTo<TowerDefenseZombiePogo>(in args[0])));
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
		if (method == MethodName.PreparePogo)
		{
			return true;
		}
		if (method == MethodName.EnterRealDeath)
		{
			return true;
		}
		if (method == MethodName.DescribeCorpse)
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
