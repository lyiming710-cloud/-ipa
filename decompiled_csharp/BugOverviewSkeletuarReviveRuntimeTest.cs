using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewSkeletuarReviveRuntimeTest.cs")]
public class BugOverviewSkeletuarReviveRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadSkeletuar = "LoadSkeletuar";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName FirstDeathClip = "FirstDeathClip";

		public static readonly StringName CountCharactersNamed = "CountCharactersNamed";

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

	private const string SkeletuarScenePath = "res://Asset/Anime/Character/Zombie/Challenge/Skeletuar/Scene/TowerDefenseZombieSkeletuar.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewSkeletuarReviveControlStub control = null;
		TowerDefenseZombieSkeletuar original = null;
		TowerDefenseZombieSkeletuar revived = null;
		TowerDefenseZombieSkeletuar restored = null;
		TowerDefenseZombieSkeletuar remoteSpawn = null;
		TowerDefenseZombieSkeletuar lateJoin = null;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_00ef;
				}
				control = new BugOverviewSkeletuarReviveControlStub
				{
					Name = "SkeletuarReviveControl",
					isGameRunning = false,
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
				original = LoadSkeletuar();
				PrepareCharacter(original, new Vector2I(6, 2), new Vector2(600f, 200f));
				control.characterNode.AddChild(original, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				original.ProcessMode = ProcessModeEnum.Disabled;
				Check(original.config?.name == "ZombieSkeletuar" && original.impName == "ZombieImpSkeleton", "The fixture must use the real Skeleton Gargantuar scene and Imp payload.");
				Check(!original.over && original.impThrowDamagePointName == "ThrowImp", "A first-life Skeleton Gargantuar must retain its authored one-time Imp throw.");
				revived = LoadSkeletuar();
				PrepareCharacter(revived, new Vector2I(5, 2), new Vector2(500f, 200f));
				revived.over = true;
				revived.impThrowFlag = true;
				control.characterNode.AddChild(revived, forceReadableName: false, InternalMode.Disabled);
				Check(revived.over && revived.impThrowDamagePointName == "" && !revived.impThrowFlag, "A revived Skeleton Gargantuar must disable the Imp throw synchronously in Ready, without a one-frame race.");
				await WaitFrames(2);
				revived.ProcessMode = ProcessModeEnum.Disabled;
				Check(revived.sprite.clip == "Relife", "A revived Skeleton Gargantuar must enter its real revive clip; clip=" + revived.sprite?.clip + ".");
				int skeletonsBeforeSecondDeath = CountCharactersNamed(control.characterNode, "Skeleton");
				revived.AnimeCompleted(FirstDeathClip(revived));
				await WaitFrames(2);
				Check(CountCharactersNamed(control.characterNode, "Skeleton") == skeletonsBeforeSecondDeath, "Completing the revived Gargantuar's second death must not create another Skeleton revive pile.");
				original.over = true;
				original.impThrowFlag = true;
				Dictionary save = original.ExportVariantSave();
				Check(save.GetValueOrDefault("over", false).AsBool() && save.GetValueOrDefault("impThrowFlag", false).AsBool(), "Progress save data must retain both the revived marker and inherited pending-throw field.");
				restored = LoadSkeletuar();
				PrepareCharacter(restored, new Vector2I(4, 2), new Vector2(400f, 200f));
				control.characterNode.AddChild(restored, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(1);
				restored.ImportVariantSave(save);
				Check(restored.over && restored.impThrowDamagePointName == "" && !restored.impThrowFlag, "Importing a revived progress save must immediately suppress the stale inherited Imp throw.");
				restored.ProcessMode = ProcessModeEnum.Disabled;
				remoteSpawn = LoadSkeletuar();
				PrepareCharacter(remoteSpawn, new Vector2I(3, 2), new Vector2(300f, 200f));
				Dictionary dictionary = original.ExportNetworkSpawnState();
				Check(remoteSpawn != null && dictionary.GetValueOrDefault("skeletuar_revived", false).AsBool(), "Skeleton Gargantuar must export and accept deterministic pre-Ready network spawn state.");
				((INetworkSpawnStateReceiver)remoteSpawn)?.ImportNetworkSpawnState(dictionary);
				Check(remoteSpawn.over && remoteSpawn.impThrowDamagePointName == "", "Replicated revive spawn state must disable the throw before the node enters the tree.");
				control.characterNode.AddChild(remoteSpawn, forceReadableName: false, InternalMode.Disabled);
				Check(remoteSpawn.over && remoteSpawn.impThrowDamagePointName == "" && !remoteSpawn.impThrowFlag, "The remote revived instance's first Ready must observe the authoritative revived state.");
				await WaitFrames(1);
				remoteSpawn.ProcessMode = ProcessModeEnum.Disabled;
				Dictionary special = original.ExportNetworkSpecialState();
				Check(special.GetValueOrDefault("over", false).AsBool() && original.GetNetworkSpecialStateRevision() != 0, "Late-join special state and its priority revision must expose the revived marker.");
				lateJoin = LoadSkeletuar();
				PrepareCharacter(lateJoin, new Vector2I(2, 2), new Vector2(200f, 200f));
				control.characterNode.AddChild(lateJoin, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(1);
				lateJoin.impThrowFlag = true;
				lateJoin.ImportNetworkSpecialState(special);
				Check(lateJoin.over && lateJoin.impThrowDamagePointName == "" && !lateJoin.impThrowFlag, "A late-join/full-snapshot import must immediately apply the revived no-throw contract.");
				lateJoin.ProcessMode = ProcessModeEnum.Disabled;
				goto end_IL_00cc;
				end_IL_00ef:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewSkeletuarReviveRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00cc;
			}
			return;
			end_IL_00cc:;
		}
		finally
		{
			TowerDefenseZombieSkeletuar[] array = new TowerDefenseZombieSkeletuar[5] { original, revived, restored, remoteSpawn, lateJoin };
			foreach (TowerDefenseZombieSkeletuar towerDefenseZombieSkeletuar in array)
			{
				if (GodotObject.IsInstanceValid(towerDefenseZombieSkeletuar) && !towerDefenseZombieSkeletuar.IsQueuedForDeletion())
				{
					towerDefenseZombieSkeletuar.QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 13;
		GD.Print($"SKELETUAR_REVIVE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseZombieSkeletuar LoadSkeletuar()
	{
		return ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Challenge/Skeletuar/Scene/TowerDefenseZombieSkeletuar.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieSkeletuar>(PackedScene.GenEditState.Disabled);
	}

	private static void PrepareCharacter(TowerDefenseZombieSkeletuar character, Vector2I gridPos, Vector2 position)
	{
		character.editorPreviewMode = false;
		character.inGame = true;
		character.gridPos = gridPos;
		character.GlobalPosition = position;
	}

	private static string FirstDeathClip(TowerDefenseZombieSkeletuar character)
	{
		return character.dieAnimeClip.Split('&', StringSplitOptions.RemoveEmptyEntries)[0];
	}

	private static int CountCharactersNamed(Node parent, string configName)
	{
		int num = 0;
		foreach (Node child in parent.GetChildren())
		{
			if (child is TowerDefenseCharacter { config: var config } && config?.name == configName)
			{
				num++;
			}
		}
		return num;
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
			GD.PushError("[BugOverviewSkeletuarReviveRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadSkeletuar, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.PrepareCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FirstDeathClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountCharactersNamed, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "configName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadSkeletuar && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieSkeletuar>(LoadSkeletuar());
			return true;
		}
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseZombieSkeletuar>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstDeathClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstDeathClip(VariantUtils.ConvertTo<TowerDefenseZombieSkeletuar>(in args[0])));
			return true;
		}
		if (method == MethodName.CountCharactersNamed && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountCharactersNamed(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.LoadSkeletuar && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieSkeletuar>(LoadSkeletuar());
			return true;
		}
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseZombieSkeletuar>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstDeathClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstDeathClip(VariantUtils.ConvertTo<TowerDefenseZombieSkeletuar>(in args[0])));
			return true;
		}
		if (method == MethodName.CountCharactersNamed && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountCharactersNamed(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.LoadSkeletuar)
		{
			return true;
		}
		if (method == MethodName.PrepareCharacter)
		{
			return true;
		}
		if (method == MethodName.FirstDeathClip)
		{
			return true;
		}
		if (method == MethodName.CountCharactersNamed)
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
