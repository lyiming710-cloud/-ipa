using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewBossDeathHeadOverlayRuntimeTest.cs")]
public class BugOverviewBossDeathHeadOverlayRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ApplyDamagePoint = "ApplyDamagePoint";

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

	private const string BossScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/ZombieBoss.tscn";

	private const string BossDaveScenePath = "res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.tscn";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		VerifyBossDeathOverlay<ZombieBoss>("res://Asset/Anime/Character/Zombie/Boss/Boss/ZombieBoss.tscn", "Boss", "Zombie_boss_head_damage2.png");
		VerifyBossDeathOverlay<ZombieBossDave>("res://Asset/Anime/Character/Zombie/Boss/BossDave/ZombieBossDave.tscn", "DaveBoss", "Zombie_bossDave_head_damage2.png");
		bool flag = _failures == 0 && _checks == 18;
		GD.Print($"BOSS_DEATH_HEAD_OVERLAY_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifyBossDeathOverlay<TSprite>(string scenePath, string label, string damagedHeadFile) where TSprite : AdobeAnimateSprite
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(packedScene), label + " production sprite scene must load.");
		TSprite val = ((packedScene != null) ? packedScene.Instantiate<TSprite>(PackedScene.GenEditState.Disabled) : null);
		Check(GodotObject.IsInstanceValid(val), label + " production sprite must instantiate with its exact runtime type.");
		if (!GodotObject.IsInstanceValid(val))
		{
			return;
		}
		AddChild(val, forceReadableName: false, InternalMode.Disabled);
		AdobeAnimateSlot nodeOrNull = val.GetNodeOrNull<AdobeAnimateSlot>("HeadSlot");
		AdobeAnimatePart nodeOrNull2 = val.GetNodeOrNull<AdobeAnimatePart>("%HeadSprite");
		Check(GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull.useFollowVisible, label + " must expose its production follow-visible head slot.");
		Check(GodotObject.IsInstanceValid(nodeOrNull2), label + " must expose its production external-atlas head overlay.");
		if (!GodotObject.IsInstanceValid(nodeOrNull) || !GodotObject.IsInstanceValid(nodeOrNull2))
		{
			val.QueueFree();
			return;
		}
		ApplyDamagePoint(val, "Stage2");
		Check(nodeOrNull2.externalAtlasTexturePath.GetFile() == damagedHeadFile, label + " Stage2 must activate the authored damaged head overlay; path=" + nodeOrNull2.externalAtlasTexturePath + ".");
		Check(AdobeAnimateManagedSprite2D.IsManagedVisible(nodeOrNull2), label + " damaged head overlay must be submitted before death.");
		val.SetAnimation("Death", loop: false);
		Check(val.clip == "Death" && !val.loop, label + " must enter the real non-looping Death clip.");
		bool flag = false;
		int layerId = val.flashAnimeData.layerDictionary.GetValueOrDefault("Boss_head", -1).AsInt32();
		for (int i = val.clipRange.X; i < val.clipRange.Y; i++)
		{
			val.frameIndex = i;
			val.elapsedTimer = 0.0;
			flag |= val.TryGetInterpolatedLayerPoseForRender(layerId, out var mediaId, out var _) && mediaId != 65535;
		}
		Check(flag, label + " Death timeline must retain its own authored main-head frames.");
		bool flag2 = AdobeAnimateManagedSprite2D.IsManagedVisible(nodeOrNull2) && AdobeAnimateManagedSprite2D.ShouldRender(nodeOrNull2);
		bool value = val.TryGetManagedSlotTransformForRender(nodeOrNull, out var transform2);
		Check(!flag2, $"{label} Death clip must not submit the detached damaged-head overlay; managedVisible={flag2}, slotPose={value}, slotOrigin={transform2.Origin}, path={nodeOrNull2.externalAtlasTexturePath}.");
		val.QueueFree();
	}

	private static void ApplyDamagePoint(AdobeAnimateSprite sprite, string damagePoint)
	{
		if (!(sprite is ZombieBoss zombieBoss))
		{
			if (sprite is ZombieBossDave zombieBossDave)
			{
				zombieBossDave.DamagePointSet(damagePoint);
			}
		}
		else
		{
			zombieBoss.DamagePointSet(damagePoint);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BossDeathHeadOverlay] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "damagePoint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ApplyDamagePoint && args.Count == 2)
		{
			ApplyDamagePoint(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.ApplyDamagePoint && args.Count == 2)
		{
			ApplyDamagePoint(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.ApplyDamagePoint)
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
