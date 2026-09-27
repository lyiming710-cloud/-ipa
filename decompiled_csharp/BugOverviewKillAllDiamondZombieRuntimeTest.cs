using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewKillAllDiamondZombieRuntimeTest.cs")]
public class BugOverviewKillAllDiamondZombieRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InstantiateZombie = "InstantiateZombie";

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

	private const string DiamondZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter7/Diamond/Scene/TowerDefenseZombieDiamond.tscn";

	private const string DiamondHelmetSlotPath = "res://Asset/Anime/Character/Zombie/Chapter7/Diamond/Armor/Config/ZombieDiamondArmorHelmetDiamond.tres";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseZombie normal = null;
		TowerDefenseZombie diamond = null;
		try
		{
			normal = InstantiateZombie("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
			diamond = InstantiateZombie("res://Asset/Anime/Character/Zombie/Chapter7/Diamond/Scene/TowerDefenseZombieDiamond.tscn");
			Check(GodotObject.IsInstanceValid(normal) && GodotObject.IsInstanceValid(diamond), "The real normal and Diamond zombie scenes must instantiate.");
			if (!GodotObject.IsInstanceValid(normal) || !GodotObject.IsInstanceValid(diamond))
			{
				return;
			}
			diamond.currentArmor = new Array<string> { "HelmetDiamond" };
			normal.inGame = false;
			normal.editorPreviewMode = false;
			diamond.inGame = false;
			diamond.editorPreviewMode = false;
			AddChild(normal, forceReadableName: false, InternalMode.Disabled);
			AddChild(diamond, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(3);
			ArmorSlotConfig slotConfig = ResourceLoader.Load<ArmorSlotConfig>("res://Asset/Anime/Character/Zombie/Chapter7/Diamond/Armor/Config/ZombieDiamondArmorHelmetDiamond.tres", null, ResourceLoader.CacheMode.Ignore);
			TowerDefenseArmorInstance item = new TowerDefenseArmorInstance(diamond, slotConfig);
			diamond.instance.armorList.Add(item);
			diamond.instance.armorHelm.Add(item);
			diamond.instance.RefreshArmorRuntimeIndex();
			Check(normal.IsInGroup("Zombie") && diamond.IsInGroup("Zombie"), "Both real characters must be discoverable by the killall command.");
			Check(diamond.instance.armorHelm.Count == 1, $"Diamond zombie must start with its packet-authored helmet; count={diamond.instance.armorHelm.Count}.");
			TowerDefenseArmorInstance diamondHelmet = ((diamond.instance.armorHelm.Count > 0) ? diamond.instance.armorHelm[0] : null);
			Check(GodotObject.IsInstanceValid(diamondHelmet) && diamondHelmet.typeData.limitMaxHit == 20.0, "The real Diamond helmet must retain its 20-damage per-hit cap that reproduced the report.");
			Check(normal.instance.hitpoints > 0.0 && diamond.instance.hitpoints > 0.0, "Both fixtures must be alive before the console command runs.");
			CommandRegistry.Init();
			CommandConfig command = CommandRegistry.GetCommand("killall");
			Check(command != null, "The built-in killall command must be registered.");
			command?.Callback.Call();
			await WaitFrames(3);
			Check(normal.instance.die && normal.instance.hitpoints <= 0.0, $"killall must still kill a normal zombie; hp={normal.instance.hitpoints}, die={normal.instance.die}.");
			Check(diamond.instance.die && diamond.instance.hitpoints <= 0.0, $"killall must bypass Diamond helmet hit caps and kill the body; hp={diamond.instance.hitpoints}, die={diamond.instance.die}.");
			Check(Math.Abs(diamondHelmet.hitPoints) < 0.001, $"Direct body settlement must still run normal death armor cleanup; helmet hp={diamondHelmet.hitPoints}.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewKillAllDiamondZombieRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(normal))
			{
				normal.QueueFree();
			}
			if (GodotObject.IsInstanceValid(diamond))
			{
				diamond.QueueFree();
			}
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_KILLALL_DIAMOND_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseZombie InstantiateZombie(string scenePath)
	{
		return ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombie>(PackedScene.GenEditState.Disabled);
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
			GD.PushError("[BugOverviewKillAllDiamondZombieRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InstantiateZombie, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.InstantiateZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(InstantiateZombie(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.InstantiateZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombie>(InstantiateZombie(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.InstantiateZombie)
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
