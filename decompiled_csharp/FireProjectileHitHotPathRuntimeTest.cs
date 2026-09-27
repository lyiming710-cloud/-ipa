using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/FireProjectileHitHotPathRuntimeTest.cs")]
public class FireProjectileHitHotPathRuntimeTest : Node
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

	private const string NormalZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		TowerDefenseControlNew control = null;
		TowerDefenseCharacter zombie = null;
		try
		{
			Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(manager))
			{
				return;
			}
			control = (manager.currentControl = new TowerDefenseControlNew
			{
				isGameRunning = true
			});
			zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(zombie), "Normal-zombie scene must instantiate.");
			if (!GodotObject.IsInstanceValid(zombie))
			{
				return;
			}
			zombie.inGame = true;
			AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			zombie.buff.AddBuff(new TowerDefenseCharacterBuffFrozen
			{
				time = 8.0,
				iceSpeedDownTime = 15.0
			});
			zombie.buff.AddBuff(new TowerDefenseCharacterBuffIceSpeedDown
			{
				time = 15.0
			});
			zombie.buff.ApplyFireHit();
			TowerDefenseCharacterBuffConfig towerDefenseCharacterBuffConfig = zombie.buff.BuffGet("FireHit");
			Check(towerDefenseCharacterBuffConfig is TowerDefenseCharacterBuffFireHit, "First built-in FireHit application must create one runtime state.");
			Check(!zombie.buff.BuffHas("Frozen") && !zombie.buff.BuffHas("IceSpeedDown"), "FireHit must still clear Frozen and IceSpeedDown immediately.");
			zombie.buff.ApplyFireHit();
			Check(towerDefenseCharacterBuffConfig == zombie.buff.BuffGet("FireHit"), "Repeated built-in FireHit in one frame must refresh the existing runtime.");
			zombie.buff.BuffUpdate(0.01);
			Check(!zombie.buff.BuffHas("FireHit"), "Built-in FireHit must retain its one-update lifetime.");
			FireHitModRuntimeProbe.RuntimeInstanceCreates = 0;
			FireHitModRuntimeProbe fireHitModRuntimeProbe = new FireHitModRuntimeProbe();
			fireHitModRuntimeProbe._Init();
			zombie.buff.ApplyFireHit(fireHitModRuntimeProbe);
			FireHitModRuntimeProbe fireHitModRuntimeProbe2 = zombie.buff.BuffGet("FireHit") as FireHitModRuntimeProbe;
			Check(fireHitModRuntimeProbe2 != null, "Mod-derived FireHit must use its explicit runtime factory.");
			Check(FireHitModRuntimeProbe.RuntimeInstanceCreates == 1, "First Mod-derived FireHit must create exactly one runtime instance.");
			zombie.buff.ApplyFireHit(fireHitModRuntimeProbe);
			Check(fireHitModRuntimeProbe2 == zombie.buff.BuffGet("FireHit"), "Repeated Mod-derived FireHit must refresh the existing runtime.");
			Check(FireHitModRuntimeProbe.RuntimeInstanceCreates == 1, "Refreshing a Mod-derived FireHit must not create another runtime Resource.");
			Check(fireHitModRuntimeProbe2.RefreshCount == 1, "Mod-derived FireHit refresh override must still execute.");
			Array<TowerDefenseCharacterBuffConfig> buffList = new Array<TowerDefenseCharacterBuffConfig> { fireHitModRuntimeProbe };
			TowerDefenseCharacterEventAddBuff.Run(zombie, buffList);
			Check(fireHitModRuntimeProbe2 == zombie.buff.BuffGet("FireHit"), "AddBuff events must share the Mod-derived FireHit runtime.");
			Check(FireHitModRuntimeProbe.RuntimeInstanceCreates == 1, "AddBuff events must not allocate another Mod-derived FireHit runtime.");
			Check(fireHitModRuntimeProbe2.RefreshCount == 2, "AddBuff events must preserve Mod-derived FireHit refresh behavior.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[FireProjectileHitHotPathRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			control?.Free();
		}
		bool flag = _failures == 0 && _checks == 14;
		GD.Print($"FIRE_PROJECTILE_HIT_HOT_PATH_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[FireProjectileHitHotPathRuntimeTest] " + message);
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
