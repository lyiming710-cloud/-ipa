using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewZombieArmorStackingRuntimeTest.cs")]
public class BugOverviewZombieArmorStackingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName HasFlag = "HasFlag";

		public static readonly StringName IsOrdinaryDropableHelmet = "IsOrdinaryDropableHelmet";

		public static readonly StringName CountArmor = "CountArmor";

		public static readonly StringName CountOrdinaryDropableHelmets = "CountOrdinaryDropableHelmets";

		public static readonly StringName FindArmor = "FindArmor";

		public static readonly StringName ClearArmors = "ClearArmors";

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

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const double ProbeDamage = 200.0;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BugOverviewZombieArmorStackingControlStub control = null;
		TowerDefenseZombieNormal zombie = null;
		try
		{
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_0061;
				}
				control = new BugOverviewZombieArmorStackingControlStub
				{
					Name = "ZombieArmorStackingControl",
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
				zombie = LoadCharacter<TowerDefenseZombieNormal>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
				Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormal", "The fixture must instantiate the real ordinary Zombie scene.");
				if (!GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_0061;
				}
				zombie.editorPreviewMode = true;
				zombie.inGame = false;
				control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				Check(GodotObject.IsInstanceValid(zombie.instance) && GodotObject.IsInstanceValid(zombie.sprite), "The real Zombie runtime and authored armor sprite must initialize.");
				if (!GodotObject.IsInstanceValid(zombie.instance))
				{
					goto end_IL_0061;
				}
				TowerDefenseArmorRegistry.Init();
				TowerDefenseArmorTypeData armorType = TowerDefenseArmorRegistry.GetArmorType("Cone");
				TowerDefenseArmorTypeData armorType2 = TowerDefenseArmorRegistry.GetArmorType("Bucket");
				TowerDefenseArmorTypeData armorType3 = TowerDefenseArmorRegistry.GetArmorType("Shield");
				TowerDefenseArmorTypeData armorType4 = TowerDefenseArmorRegistry.GetArmorType("Balloon");
				Check(armorType != null && armorType2 != null && armorType3 != null && armorType4 != null, "The production armor registry must provide Cone, Bucket, Shield, and Balloon.");
				Check(IsOrdinaryDropableHelmet(armorType?.armorMethodFlags ?? 0) && IsOrdinaryDropableHelmet(armorType2?.armorMethodFlags ?? 0), "Cone and Bucket must remain ordinary mutually-exclusive dropable helmets.");
				Check(armorType3 != null && HasFlag(armorType3.armorMethodFlags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.SHIELD) && !IsOrdinaryDropableHelmet(armorType3.armorMethodFlags), "Shield must remain a separate shield layer rather than an ordinary helmet.");
				Check(armorType4 != null && HasFlag(armorType4.armorMethodFlags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.HELM) && HasFlag(armorType4.armorMethodFlags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.BODY) && !HasFlag(armorType4.armorMethodFlags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.DROPABLE) && !IsOrdinaryDropableHelmet(armorType4.armorMethodFlags), "Balloon must retain its HELM+BODY non-dropable special-layer contract.");
				zombie.instance.ArmorAdd("Bucket");
				zombie.instance.ArmorAdd("Bucket");
				Check(CountArmor(zombie, "Bucket") == 1, $"Adding Bucket twice must leave one Bucket; count={CountArmor(zombie, "Bucket")}.");
				Check(CountOrdinaryDropableHelmets(zombie) == 1, "A duplicate add must not create a second ordinary helmet layer.");
				ClearArmors(zombie);
				zombie.instance.ArmorAdd("Cone");
				zombie.instance.ArmorAdd("Bucket");
				Check(!zombie.instance.ArmorHas("Cone") && zombie.instance.ArmorHas("Bucket"), "Adding Bucket after Cone must replace Cone with Bucket.");
				Check(CountOrdinaryDropableHelmets(zombie) == 1, $"Cone then Bucket must leave one ordinary helmet; count={CountOrdinaryDropableHelmets(zombie)}.");
				TowerDefenseArmorInstance towerDefenseArmorInstance = FindArmor(zombie, "Bucket");
				double num = towerDefenseArmorInstance?.hitPoints ?? (-1.0);
				double hitpoints = zombie.instance.hitpoints;
				zombie.instance.Hurt(200.0, playSplatAudio: false, Vector2.Zero, hitShield: true, createDamagePart: false);
				Check(towerDefenseArmorInstance != null && Math.Abs(towerDefenseArmorInstance.hitPoints - (num - 200.0)) < 0.001, $"Damage must hit the selected Bucket directly, not an illegal older helmet; before={num}, after={towerDefenseArmorInstance?.hitPoints}.");
				Check(Math.Abs(zombie.instance.hitpoints - hitpoints) < 0.001, "Damage absorbed by the selected Bucket must not leak into Zombie body health.");
				Check(!zombie.instance.ArmorHas("Cone") && CountArmor(zombie, "Bucket") == 1, "The damage chain must not reveal or retain an illegal hidden Cone layer.");
				ClearArmors(zombie);
				zombie.instance.ArmorAdd("Shield");
				zombie.instance.ArmorAdd("Bucket");
				Check(zombie.instance.ArmorHas("Shield") && zombie.instance.ArmorHas("Bucket"), "Shield and Bucket must legally coexist.");
				Check(zombie.instance.armorShield.Count == 1 && CountOrdinaryDropableHelmets(zombie) == 1 && zombie.instance.armorList.Count == 2, $"Shield+Bucket must create exactly one shield and one ordinary helmet; shield={zombie.instance.armorShield.Count}, total={zombie.instance.armorList.Count}.");
				ClearArmors(zombie);
				zombie.instance.ArmorAdd("Balloon");
				zombie.instance.ArmorAdd("Bucket");
				TowerDefenseArmorInstance towerDefenseArmorInstance2 = FindArmor(zombie, "Balloon");
				Check(zombie.instance.ArmorHas("Balloon") && zombie.instance.ArmorHas("Bucket"), "Balloon and Bucket must legally coexist.");
				Check(towerDefenseArmorInstance2 != null && zombie.instance.armorHelm.Contains(towerDefenseArmorInstance2) && zombie.instance.armorBody.Contains(towerDefenseArmorInstance2), "The real Balloon instance must remain in both HELM and BODY routing lists.");
				Check(CountOrdinaryDropableHelmets(zombie) == 1 && zombie.instance.armorList.Count == 2, $"Balloon+Bucket must retain two legal layers while counting only Bucket as ordinary; ordinary={CountOrdinaryDropableHelmets(zombie)}, total={zombie.instance.armorList.Count}.");
				goto end_IL_0058;
				end_IL_0061:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewZombieArmorStackingRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0058;
			}
			return;
			end_IL_0058:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				ClearArmors(zombie);
				if (!zombie.IsQueuedForDeletion())
				{
					zombie.QueueFree();
				}
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 19;
		GD.Print($"ZOMBIE_ARMOR_STACKING_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T LoadCharacter<T>(string path) where T : TowerDefenseCharacter
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static bool HasFlag(int flags, TowerDefenseEnum.ARMOR_METHOD_FLAGS flag)
	{
		return ((uint)flags & (uint)flag) != 0;
	}

	private static bool IsOrdinaryDropableHelmet(int flags)
	{
		if (HasFlag(flags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.HELM) && HasFlag(flags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.DROPABLE))
		{
			return !HasFlag(flags, TowerDefenseEnum.ARMOR_METHOD_FLAGS.BODY);
		}
		return false;
	}

	private static int CountArmor(TowerDefenseCharacter character, string armorName)
	{
		int num = 0;
		foreach (TowerDefenseArmorInstance armor in character.instance.armorList)
		{
			if (armor?.slotConfig?.armorName == armorName && !armor.isRemove)
			{
				num++;
			}
		}
		return num;
	}

	private static int CountOrdinaryDropableHelmets(TowerDefenseCharacter character)
	{
		int num = 0;
		foreach (TowerDefenseArmorInstance armor in character.instance.armorList)
		{
			if (armor != null && !armor.isRemove && IsOrdinaryDropableHelmet(armor.armorMethodFlags))
			{
				num++;
			}
		}
		return num;
	}

	private static TowerDefenseArmorInstance FindArmor(TowerDefenseCharacter character, string armorName)
	{
		foreach (TowerDefenseArmorInstance armor in character.instance.armorList)
		{
			if (armor?.slotConfig?.armorName == armorName && !armor.isRemove)
			{
				return armor;
			}
		}
		return null;
	}

	private static void ClearArmors(TowerDefenseCharacter character)
	{
		while (character != null && character.instance?.armorList?.Count > 0)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = character.instance.armorList[0];
			if (!GodotObject.IsInstanceValid(towerDefenseArmorInstance))
			{
				character.instance.armorList.RemoveAt(0);
				character.instance.RefreshArmorRuntimeIndex();
				continue;
			}
			towerDefenseArmorInstance.Hurt(10000000.0, playSplatAudio: false, Vector2.Zero, createDamagePart: false, ignoreLimit: true);
			if (character.instance.armorList.Count > 0 && character.instance.armorList[0] == towerDefenseArmorInstance)
			{
				character.instance.ArmorDestroy(towerDefenseArmorInstance);
			}
		}
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
			GD.PushError("[BugOverviewZombieArmorStackingRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasFlag, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "flag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsOrdinaryDropableHelmet, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountArmor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountOrdinaryDropableHelmets, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindArmor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearArmors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.HasFlag && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlag(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.ARMOR_METHOD_FLAGS>(in args[1])));
			return true;
		}
		if (method == MethodName.IsOrdinaryDropableHelmet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsOrdinaryDropableHelmet(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CountArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CountOrdinaryDropableHelmets && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountOrdinaryDropableHelmets(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.FindArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorInstance>(FindArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearArmors && args.Count == 1)
		{
			ClearArmors(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.HasFlag && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFlag(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.ARMOR_METHOD_FLAGS>(in args[1])));
			return true;
		}
		if (method == MethodName.IsOrdinaryDropableHelmet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsOrdinaryDropableHelmet(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CountArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CountOrdinaryDropableHelmets && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountOrdinaryDropableHelmets(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.FindArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorInstance>(FindArmor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearArmors && args.Count == 1)
		{
			ClearArmors(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.HasFlag)
		{
			return true;
		}
		if (method == MethodName.IsOrdinaryDropableHelmet)
		{
			return true;
		}
		if (method == MethodName.CountArmor)
		{
			return true;
		}
		if (method == MethodName.CountOrdinaryDropableHelmets)
		{
			return true;
		}
		if (method == MethodName.FindArmor)
		{
			return true;
		}
		if (method == MethodName.ClearArmors)
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
