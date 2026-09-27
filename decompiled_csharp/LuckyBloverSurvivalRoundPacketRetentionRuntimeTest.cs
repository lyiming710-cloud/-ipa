using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/LuckyBloverSurvivalRoundPacketRetentionRuntimeTest.cs")]
public class LuckyBloverSurvivalRoundPacketRetentionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateInteractivePacket = "CreateInteractivePacket";

		public static readonly StringName IsFrozen = "IsFrozen";

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

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		TowerDefenseInGameSeedBank towerDefenseInGameSeedBank = null;
		Node node = null;
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = null;
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow2 = null;
		try
		{
			towerDefenseInGameSeedBank = new TowerDefenseInGameSeedBank();
			node = (towerDefenseInGameSeedBank.packetContainer = new Node());
			towerDefenseInGamePacketShow = CreateInteractivePacket("LuckyBloverPacket");
			towerDefenseInGamePacketShow2 = CreateInteractivePacket("RandomPacket");
			node.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			node.AddChild(towerDefenseInGamePacketShow2, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGameSeedBank.packetList.Add(towerDefenseInGamePacketShow);
			towerDefenseInGameSeedBank.packetList.Add(towerDefenseInGamePacketShow2);
			towerDefenseInGamePacketShow.OnPressed += towerDefenseInGameSeedBank.DeletePacket;
			towerDefenseInGamePacketShow2.OnPressed += towerDefenseInGameSeedBank.DeletePacket;
			Check(HasPressedHandler(towerDefenseInGamePacketShow, towerDefenseInGameSeedBank, "DeletePacket") && HasPressedHandler(towerDefenseInGamePacketShow2, towerDefenseInGameSeedBank, "DeletePacket"), "The probe must begin with both round packets in the old removable state.");
			towerDefenseInGameSeedBank.ReadyPackets();
			Check(towerDefenseInGameSeedBank.packetList.Count == 2 && node.GetChildCount() == 2, "Freezing round entry must retain both the mandatory Lucky Blover and the random packet.");
			Check(IsFrozen(towerDefenseInGamePacketShow) && IsFrozen(towerDefenseInGamePacketShow2), "Every packet must become non-interactive immediately at round entry.");
			Check(!HasPressedHandler(towerDefenseInGamePacketShow, towerDefenseInGameSeedBank, "DeletePacket") && !HasPressedHandler(towerDefenseInGamePacketShow2, towerDefenseInGameSeedBank, "DeletePacket"), "Round-entry freezing must detach the deletion handler from every packet.");
			Check(!towerDefenseInGamePacketShow.IsQueuedForDeletion() && !towerDefenseInGamePacketShow2.IsQueuedForDeletion(), "Neither packet may be queued for deletion during the freeze.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[LuckyBloverSurvivalRoundPacketRetentionRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(node))
			{
				node.Free();
			}
			if (GodotObject.IsInstanceValid(towerDefenseInGameSeedBank))
			{
				towerDefenseInGameSeedBank.Free();
			}
		}
		bool flag = _failures == 0;
		GD.Print($"LUCKY_BLOVER_SURVIVAL_PACKET_RETENTION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseInGamePacketShow CreateInteractivePacket(string name)
	{
		return new TowerDefenseInGamePacketShow
		{
			Name = name,
			alive = true,
			@lock = false,
			onlyDraw = false,
			start = true
		};
	}

	private static bool IsFrozen(TowerDefenseInGamePacketShow packet)
	{
		if (GodotObject.IsInstanceValid(packet) && !packet.alive && packet.@lock)
		{
			return packet.onlyDraw;
		}
		return false;
	}

	private static bool HasPressedHandler(TowerDefenseInGamePacketShow packet, object target, string methodName)
	{
		if (!(typeof(TowerDefenseInGamePacketShow).GetField("OnPressed", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(packet) is Delegate obj))
		{
			return false;
		}
		Delegate[] invocationList = obj.GetInvocationList();
		foreach (Delegate obj2 in invocationList)
		{
			if (obj2.Target == target && obj2.Method.Name == methodName)
			{
				return true;
			}
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[LuckyBloverSurvivalRoundPacketRetentionRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(4)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateInteractivePacket, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsFrozen, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.CreateInteractivePacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreateInteractivePacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFrozen && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFrozen(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0])));
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
		if (method == MethodName.CreateInteractivePacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(CreateInteractivePacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsFrozen && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFrozen(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0])));
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
		if (method == MethodName.CreateInteractivePacket)
		{
			return true;
		}
		if (method == MethodName.IsFrozen)
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
