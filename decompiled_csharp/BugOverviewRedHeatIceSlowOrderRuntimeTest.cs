using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewRedHeatIceSlowOrderRuntimeTest.cs")]
public class BugOverviewRedHeatIceSlowOrderRuntimeTest : Node
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
		TowerDefenseControlNew control = null;
		TowerDefenseCharacter zombie = null;
		try
		{
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			Check(GodotObject.IsInstanceValid(instance), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(instance))
			{
				return;
			}
			control = (instance.currentControl = new TowerDefenseControlNew
			{
				isGameRunning = true
			});
			instance.gridBeginPos = new Vector2(0f, 100f);
			instance.gridSize = new Vector2(100f, 76f);
			instance.gridNum = new Vector2I(9, 5);
			zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
			Check(GodotObject.IsInstanceValid(zombie), "Normal-zombie scene must instantiate.");
			if (!GodotObject.IsInstanceValid(zombie))
			{
				return;
			}
			zombie.Position = new Vector2(300f, 176f);
			zombie.gridPos = new Vector2I(3, 1);
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
			Check(zombie.buff.BuffHas("Frozen"), "The fixture must accept Frozen before RedHeat is applied.");
			Check(zombie.buff.BuffHas("IceSpeedDown"), "The fixture must accept IceSpeedDown before RedHeat is applied.");
			zombie.buff.AddBuff(new TowerDefenseCharacterBuffRedHeat
			{
				time = 15.0
			});
			Check(zombie.buff.BuffHas("RedHeat"), "RedHeat must remain active after entering.");
			Check(!zombie.buff.BuffHas("Frozen"), "Entering RedHeat must immediately clear an existing Frozen buff.");
			Check(!zombie.buff.BuffHas("IceSpeedDown"), "Entering RedHeat must immediately clear an existing IceSpeedDown buff.");
			zombie.timeScale = 1.0;
			zombie.buff.BuffUpdate(0.25);
			Check(!zombie.buff.BuffHas("FireHit"), "The one-shot FireHit cleanup helper must expire after the buff update.");
			Check(Math.Abs(zombie.timeScale - 1.0) < 0.0001, $"RedHeat alone must not slow the character; got timeScale={zombie.timeScale}.");
			zombie.buff.AddBuff(new TowerDefenseCharacterBuffIceSpeedDown
			{
				time = 15.0
			});
			Check(zombie.buff.BuffHas("RedHeat") && zombie.buff.BuffHas("IceSpeedDown"), "IceSpeedDown applied after RedHeat must be allowed to coexist by design.");
			zombie.timeScale = 1.0;
			zombie.buff.BuffUpdate(0.25);
			Check(zombie.buff.BuffHas("RedHeat") && zombie.buff.BuffHas("IceSpeedDown"), "RedHeat updates must not repeatedly erase a later IceSpeedDown buff.");
			Check(Math.Abs(zombie.timeScale - 0.5) < 0.0001, $"The later IceSpeedDown must still halve timeScale; got {zombie.timeScale}.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewRedHeatIceSlowOrderRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				zombie.QueueFree();
			}
			TowerDefenseManager instance2 = TowerDefenseManager.Instance;
			if (GodotObject.IsInstanceValid(instance2))
			{
				instance2.currentControl = null;
			}
			control?.Free();
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_REDHEAT_ICE_SLOW_ORDER_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewRedHeatIceSlowOrderRuntimeTest] " + message);
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
