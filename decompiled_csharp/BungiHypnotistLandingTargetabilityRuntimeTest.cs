using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BungiHypnotistLandingTargetabilityRuntimeTest.cs")]
public class BungiHypnotistLandingTargetabilityRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName InvokeTargetabilityTransition = "InvokeTargetabilityTransition";

		public static readonly StringName VerifyUntargetable = "VerifyUntargetable";

		public static readonly StringName VerifyGroundTargetable = "VerifyGroundTargetable";

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

	private const string BungiScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungi.tscn";

	private const string HypnotistScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BungiHypnotistLandingTargetabilityRuntimeControlStub control = null;
		TowerDefenseZombieBungi bungi = null;
		TowerDefenseZombieHypnotist hypnotist = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_0087;
				}
				control = new BungiHypnotistLandingTargetabilityRuntimeControlStub
				{
					Name = "BungiHypnotistLandingTargetabilityRuntimeControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				bungi = Instantiate<TowerDefenseZombieBungi>("res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungi.tscn");
				hypnotist = Instantiate<TowerDefenseZombieHypnotist>("res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn");
				Check(GodotObject.IsInstanceValid(bungi) && bungi.config?.name == "ZombieBungi" && GodotObject.IsInstanceValid(hypnotist) && hypnotist.config?.name == "ZombieHypnotist", "The fixture must instantiate the real Bungee and Hypnotist scenes.");
				if (!GodotObject.IsInstanceValid(bungi) || !GodotObject.IsInstanceValid(hypnotist))
				{
					goto end_IL_0087;
				}
				PrepareCharacter(bungi, new Vector2I(3, 2));
				PrepareCharacter(hypnotist, new Vector2I(5, 2));
				control.characterNode.AddChild(bungi, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(hypnotist, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				bungi.ProcessMode = ProcessModeEnum.Disabled;
				hypnotist.ProcessMode = ProcessModeEnum.Disabled;
				VerifyUntargetable(bungi, "An ordinary Bungee must begin outside targeting before landing.");
				VerifyUntargetable(hypnotist, "A Hypnotist must begin outside targeting before landing.");
				bungi.isGround = true;
				InvokeTargetabilityTransition(bungi, "ApplyLandedTargetability");
				VerifyGroundTargetable(bungi, "An ordinary Bungee must enter ground targeting after landing.");
				bungi.skipBungeeTarget = true;
				bungi.instance.hypnoses = true;
				InvokeTargetabilityTransition(bungi, "ApplyLandedTargetability");
				VerifyUntargetable(bungi, "The protected hypnotized Bungee must remain untargetable after landing.");
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = hypnotist.gridPos
				};
				towerDefenseCellInstance.Init(new TowerDefenseCellConfig());
				towerDefenseCellInstance.characterList.Add(hypnotist);
				hypnotist.cell = towerDefenseCellInstance;
				hypnotist.ProcessMode = ProcessModeEnum.Always;
				hypnotist.Walk();
				await WaitFrames(2);
				VerifyUntargetable(hypnotist, "A descending Hypnotist must remain outside every target channel.");
				Check(await WaitUntil(() => GodotObject.IsInstanceValid(hypnotist) && hypnotist.waitGrab, 300), "The real Hypnotist drop tween must reach its landed wait state.");
				VerifyGroundTargetable(hypnotist, "A Hypnotist must enter ground targeting only after its real drop tween lands.");
				goto end_IL_0070;
				end_IL_0087:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BungiHypnotistLandingTargetabilityRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0070;
			}
			return;
			end_IL_0070:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(hypnotist))
			{
				hypnotist.QueueFree();
			}
			if (GodotObject.IsInstanceValid(bungi))
			{
				bungi.QueueFree();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(4);
		}
		bool flag = _failures == 0 && _checks >= 8;
		GD.Print($"BUNGI_HYPNOTIST_TARGETABILITY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static void PrepareCharacter(TowerDefenseCharacter character, Vector2I grid)
	{
		character.editorPreviewMode = false;
		character.inGame = true;
		character.gridPos = grid;
		character.SetLogicalGlobalPosition(new Vector2((float)grid.X * 100f, (float)grid.Y * 76f));
	}

	private static void InvokeTargetabilityTransition(TowerDefenseCharacter character, string methodName)
	{
		System.Reflection.MethodInfo? method = character.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(character.GetType().Name, methodName);
		}
		method.Invoke(character, null);
	}

	private void VerifyUntargetable(TowerDefenseCharacter character, string message)
	{
		Check(character.HasHitBox && !character.IsHitBoxEnabled && !character.IsHitBoxMonitorable && character.instance.invincible && !character.instance.canBeCollection && !character.targetRegistrationComponent.canProjectileCheck && character.instance.maskFlags == 0, $"{message} enabled={character.IsHitBoxEnabled}, monitorable={character.IsHitBoxMonitorable}, invincible={character.instance.invincible}, collection={character.instance.canBeCollection}, projectile={character.targetRegistrationComponent.canProjectileCheck}, mask={character.instance.maskFlags}.");
	}

	private void VerifyGroundTargetable(TowerDefenseCharacter character, string message)
	{
		int num = 1;
		Check(character.HasHitBox && character.IsHitBoxEnabled && character.IsHitBoxMonitorable && !character.instance.invincible && character.instance.canBeCollection && character.targetRegistrationComponent.canProjectileCheck && character.instance.maskFlags == num, $"{message} enabled={character.IsHitBoxEnabled}, monitorable={character.IsHitBoxMonitorable}, invincible={character.instance.invincible}, collection={character.instance.canBeCollection}, projectile={character.targetRegistrationComponent.canProjectileCheck}, mask={character.instance.maskFlags}, expected={num}.");
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task<bool> WaitUntil(Func<bool> predicate, int maximumFrames)
	{
		for (int frame = 0; frame < maximumFrames; frame++)
		{
			if (predicate())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return predicate();
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BungiHypnotistLandingTargetabilityRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(6)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2I, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvokeTargetabilityTransition, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "methodName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyUntargetable, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyGroundTargetable, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.PrepareCharacter && args.Count == 2)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokeTargetabilityTransition && args.Count == 2)
		{
			InvokeTargetabilityTransition(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyUntargetable && args.Count == 2)
		{
			VerifyUntargetable(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyGroundTargetable && args.Count == 2)
		{
			VerifyGroundTargetable(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.PrepareCharacter && args.Count == 2)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokeTargetabilityTransition && args.Count == 2)
		{
			InvokeTargetabilityTransition(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.PrepareCharacter)
		{
			return true;
		}
		if (method == MethodName.InvokeTargetabilityTransition)
		{
			return true;
		}
		if (method == MethodName.VerifyUntargetable)
		{
			return true;
		}
		if (method == MethodName.VerifyGroundTargetable)
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
