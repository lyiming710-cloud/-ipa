using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/HypnoShroomGhostCostAndTimingRuntimeTest.cs")]
public class HypnoShroomGhostCostAndTimingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName InstantiateZombie = "InstantiateZombie";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName QueueIfValid = "QueueIfValid";

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

	private const string GhostPacketPath = "res://Asset/Anime/Character/Plant/Diamond/HypnoShroomGhost/Packet/PlantHypnoShroomGhost.tres";

	private const string GhostScenePath = "res://Asset/Anime/Character/Plant/Diamond/HypnoShroomGhost/Scene/TowerDefensePlantHypnoShroomGhost.tscn";

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew control = null;
		TowerDefenseInGameSeedBank seedBank = null;
		TowerDefenseInGamePacketShow ghostSlot = null;
		TowerDefensePlantHypnoShroomGhost ghost = null;
		TowerDefenseZombie firstZombie = null;
		TowerDefenseZombie secondZombie = null;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_009b;
				}
				TowerDefensePlantHypnoShroomGhost.ResetBattleState();
				control = new TowerDefenseControlNew
				{
					isGameRunning = true
				};
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				seedBank = new TowerDefenseInGameSeedBank();
				TowerDefenseBattleFeatureSeedBank value = new TowerDefenseBattleFeatureSeedBank
				{
					seedBank = seedBank,
					config = new TowerDefenseLevelSeedBankConfig
					{
						method = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE
					}
				};
				control.featureDictionary[new StringName("SeedBank")] = value;
				TowerDefensePacketConfig ghostConfig = LoadPacket("res://Asset/Anime/Character/Plant/Diamond/HypnoShroomGhost/Packet/PlantHypnoShroomGhost.tres");
				Check(GodotObject.IsInstanceValid(ghostConfig), "Real HypnoShroomGhost packet resource must load.");
				if (!GodotObject.IsInstanceValid(ghostConfig))
				{
					goto end_IL_009b;
				}
				Check(ghostConfig.GetCost() == 800, $"Ghost packet must start at its documented 800 cost; got {ghostConfig.GetCost()}.");
				ghostSlot = TowerDefenseManager.CreatePacketShow();
				control.characterNode.AddChild(ghostSlot, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				ghostSlot.Init(ghostConfig);
				ghostSlot.originalSaveKey = "PlantHypnoShroomGhost";
				ghostSlot.start = true;
				seedBank.packetList.Add(ghostSlot);
				firstZombie = InstantiateZombie();
				secondZombie = InstantiateZombie();
				Check(GodotObject.IsInstanceValid(firstZombie) && GodotObject.IsInstanceValid(secondZombie), "Two real normal-zombie scenes must instantiate.");
				if (!GodotObject.IsInstanceValid(firstZombie) || !GodotObject.IsInstanceValid(secondZombie))
				{
					goto end_IL_009b;
				}
				PrepareCharacter(firstZombie, control.characterNode);
				PrepareCharacter(secondZombie, control.characterNode);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				firstZombie.Hypnoses(10.0);
				Check(firstZombie.instance.hypnoses, "The real zombie hypnosis component and buff must enter hypnosis state.");
				Check(ghostConfig.GetCost() == 775, $"One externally hypnotized zombie must reduce Ghost cost by 25; got {ghostConfig.GetCost()}.");
				secondZombie.Hypnoses(10.0);
				Check(secondZombie.instance.hypnoses, "A second real zombie must also enter hypnosis state.");
				Check(ghostConfig.GetCost() == 750, $"Two externally hypnotized zombies must reduce Ghost cost to 750; got {ghostConfig.GetCost()}.");
				for (int frame = 0; frame < 20; frame++)
				{
					if (ghostSlot.itemCost == 750)
					{
						break;
					}
					await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				}
				Check(ghostSlot.itemCost == 750, $"The live card display must refresh to the reduced configuration cost; got {ghostSlot.itemCost}.");
				ghost = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Diamond/HypnoShroomGhost/Scene/TowerDefensePlantHypnoShroomGhost.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantHypnoShroomGhost>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(ghost), "Real HypnoShroomGhost scene must instantiate.");
				if (!GodotObject.IsInstanceValid(ghost))
				{
					goto end_IL_009b;
				}
				ghost.inGame = false;
				ghost.editorPreviewMode = true;
				control.characterNode.AddChild(ghost, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				ExplodeComponent runtime = ghost.componentManager.GetRuntime<ExplodeComponent>("character.explode");
				Check(runtime != null && !runtime.IsReleased, "Ghost must bind its real ExplodeComponent runtime.");
				Check(GodotObject.IsInstanceValid(ghost.sprite) && ghost.sprite.HasClip("Explode"), "Ghost's real Adobe animation must contain the Explode clip.");
				if (runtime != null && !runtime.IsReleased && GodotObject.IsInstanceValid(ghost.sprite))
				{
					runtime.ExplodeEntered();
					Check(ghost.sprite.clip == "Explode", "Entering the explode state must switch to Explode immediately, not wait in Idle; got " + ghost.sprite.clip + ".");
					Check(ghost.sprite.clipRange == new Vector2I(20, 54), $"The authored Explode clip must remain frames 20..54; got {ghost.sprite.clipRange}.");
					double num = ghost.sprite.flashAnimeData?.frameRate ?? 0.0;
					double num2 = ((num > 0.0) ? ((double)(ghost.sprite.clipRange.Y - ghost.sprite.clipRange.X) / num) : 0.0);
					Check(Math.Abs(num - 12.0) < 0.0001, $"The authored Ghost animation must run at 12 fps; got {num}.");
					Check(Math.Abs(num2 - 2.8333333333333335) < 0.0001, $"The visible summon animation must account for the full timing (~2.83s); got {num2:0.000}s.");
				}
				goto end_IL_0078;
				end_IL_009b:;
			}
			catch (Exception value2)
			{
				_failures++;
				GD.PushError($"[HypnoShroomGhostCostAndTimingRuntimeTest] Unexpected exception: {value2}");
				goto end_IL_0078;
			}
			return;
			end_IL_0078:;
		}
		finally
		{
			TowerDefensePlantHypnoShroomGhost.ResetBattleState();
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = null;
			}
			QueueIfValid(ghost);
			QueueIfValid(firstZombie);
			QueueIfValid(secondZombie);
			QueueIfValid(ghostSlot);
			if (GodotObject.IsInstanceValid(seedBank))
			{
				seedBank.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.Free();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0;
		GD.Print($"HYPNO_SHROOM_GHOST_COST_TIMING_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private static TowerDefenseZombie InstantiateZombie()
	{
		return ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
	}

	private static void PrepareCharacter(TowerDefenseCharacter character, Node parent)
	{
		character.inGame = false;
		character.editorPreviewMode = false;
		parent.AddChild(character, forceReadableName: false, InternalMode.Disabled);
	}

	private static void QueueIfValid(Node node)
	{
		if (GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion())
		{
			node.QueueFree();
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[HypnoShroomGhostCostAndTimingRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InstantiateZombie, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.PrepareCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.QueueIfValid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InstantiateZombie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(InstantiateZombie());
			return true;
		}
		if (method == MethodName.PrepareCharacter && args.Count == 2)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueIfValid && args.Count == 1)
		{
			QueueIfValid(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InstantiateZombie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(InstantiateZombie());
			return true;
		}
		if (method == MethodName.PrepareCharacter && args.Count == 2)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueIfValid && args.Count == 1)
		{
			QueueIfValid(VariantUtils.ConvertTo<Node>(in args[0]));
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
		if (method == MethodName.LoadPacket)
		{
			return true;
		}
		if (method == MethodName.InstantiateZombie)
		{
			return true;
		}
		if (method == MethodName.PrepareCharacter)
		{
			return true;
		}
		if (method == MethodName.QueueIfValid)
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
