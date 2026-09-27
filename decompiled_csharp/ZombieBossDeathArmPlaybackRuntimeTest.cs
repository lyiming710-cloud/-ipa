using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieBossDeathArmPlaybackRuntimeTest.cs")]
public class ZombieBossDeathArmPlaybackRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifyDeathArmPlayback = "VerifyDeathArmPlayback";

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

	private const string BossSpriteScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/ZombieBoss.tscn";

	private const string BossDaveSpriteScenePath = "res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.tscn";

	private const string ResultMarker = "ZOMBIE_BOSS_DEATH_ARM_PLAYBACK_RESULT";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		VerifyDeathArmPlayback("res://Asset/Anime/Character/Zombie/Boss/Boss/ZombieBoss.tscn", "Boss");
		VerifyDeathArmPlayback("res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.tscn", "DaveBoss");
		bool flag = _failures == 0 && _checks == 12;
		GD.Print($"{"ZOMBIE_BOSS_DEATH_ARM_PLAYBACK_RESULT"} passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyDeathArmPlayback(string scenePath, string bossName)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(packedScene), bossName + " production sprite scene must load.");
		AdobeAnimateSpriteBase adobeAnimateSpriteBase = packedScene?.Instantiate<AdobeAnimateSpriteBase>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(adobeAnimateSpriteBase), bossName + " production sprite must instantiate.");
		if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase))
		{
			AddChild(adobeAnimateSpriteBase, forceReadableName: false, InternalMode.Disabled);
			AdobeAnimateSpriteBase nodeOrNull = adobeAnimateSpriteBase.GetNodeOrNull<AdobeAnimateSpriteBase>("%Arm");
			Check(GodotObject.IsInstanceValid(nodeOrNull), bossName + " production sprite must expose its separated arm.");
			if (!GodotObject.IsInstanceValid(nodeOrNull))
			{
				adobeAnimateSpriteBase.QueueFree();
				return;
			}
			adobeAnimateSpriteBase.SetAnimation("Death", loop: false);
			Check(adobeAnimateSpriteBase.clip == "Death" && nodeOrNull.clip == "Death", bossName + " body and arm must both select the Death clip.");
			Check(!adobeAnimateSpriteBase.loop && !nodeOrNull.loop, bossName + " arm must inherit the body's non-looping Death playback.");
			nodeOrNull.frameIndex = Math.Max(nodeOrNull.clipRange.X, nodeOrNull.clipRange.Y - 1);
			nodeOrNull.elapsedTimer = 0.0;
			nodeOrNull.RunBatchedProcessUpdate(2.0 / Math.Max(1.0, nodeOrNull.frameRate));
			Check(nodeOrNull.clipOver && nodeOrNull.frameIndex == Math.Max(nodeOrNull.clipRange.X, nodeOrNull.clipRange.Y - 1), bossName + " arm must stop on the Death end pose instead of falling again.");
			adobeAnimateSpriteBase.QueueFree();
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[ZombieBossDeathArmPlaybackRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyDeathArmPlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scenePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "bossName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifyDeathArmPlayback && args.Count == 2)
		{
			VerifyDeathArmPlayback(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.VerifyDeathArmPlayback)
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
