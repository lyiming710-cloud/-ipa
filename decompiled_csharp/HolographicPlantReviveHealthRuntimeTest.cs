using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/HolographicPlantReviveHealthRuntimeTest.cs")]
public class HolographicPlantReviveHealthRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VerifySharedPlantRecovery = "VerifySharedPlantRecovery";

		public static readonly StringName VerifyWallnutDamageStageRecovery = "VerifyWallnutDamageStageRecovery";

		public static readonly StringName VerifyGravestoneDamageStageRecovery = "VerifyGravestoneDamageStageRecovery";

		public static readonly StringName MarkDead = "MarkDead";

		public static readonly StringName GroundProjectileFlags = "GroundProjectileFlags";

		public static readonly StringName ApplyDamageStage = "ApplyDamageStage";

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

	private const string WallnutConfigPath = "res://Asset/Anime/Character/Plant/Chapter9/WallnutQX/Config/TowerDefensePlantWallnutQX.tres";

	private const string WallnutSpritePath = "res://Asset/Anime/Character/Plant/Chapter9/WallnutQX/WallnutQX.tscn";

	private const string WallnutMediaName = "WallnutQX_body.png";

	private const string GravestoneConfigPath = "res://Asset/Anime/Character/GraveStone/TargetQX/Config/TowerDefenseGraveStoneTargetQX.tres";

	private const string GravestoneSpritePath = "res://Asset/Anime/Character/GraveStone/TargetQX/GraveStoneTargetQX.tscn";

	private const string GravestoneMediaName = "TargetStonesQX_1.png";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		try
		{
			VerifySharedPlantRecovery();
			VerifyWallnutDamageStageRecovery();
			VerifyGravestoneDamageStageRecovery();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[HolographicPlantReviveHealthRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0 && _checks == 23;
		GD.Print($"HOLOGRAPHIC_PLANT_REVIVE_HEALTH_RUNTIME_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void VerifySharedPlantRecovery()
	{
		TowerDefensePlant towerDefensePlant = new TowerDefensePlant();
		TowerDefensePlantConfig towerDefensePlantConfig = (TowerDefensePlantConfig)(towerDefensePlant.config = new TowerDefensePlantConfig
		{
			hitpoints = 475.0,
			collisionFlags = 25,
			maskFlags = 25
		});
		towerDefensePlant.instance = new TowerDefenseCharacterInstance();
		towerDefensePlant.instance._Init(towerDefensePlant, towerDefensePlantConfig);
		MarkDead(towerDefensePlant);
		System.Reflection.MethodInfo method = typeof(TowerDefensePlant).GetMethod("RestoreFullHealthAfterRevive", BindingFlags.Instance | BindingFlags.NonPublic);
		Check(method != null, "TowerDefensePlant must expose the protected revive-health operation.");
		method?.Invoke(towerDefensePlant, null);
		Check(Mathf.IsEqualApprox(towerDefensePlant.instance.hitpoints, towerDefensePlant.instance.hitpointsSave), "Revived plants must restore their runtime maximum hitpoints.");
		Check(!towerDefensePlant.instance.die && !towerDefensePlant.die, "Revived plants must clear both instance and character death flags.");
		Check(!towerDefensePlant.instance.nearDie && !towerDefensePlant.nearDie, "Revived plants must clear both instance and character near-death flags.");
		Check(towerDefensePlant.instance.collisionFlags == towerDefensePlantConfig.collisionFlags, "Revived plants must restore their configured collision identity.");
		Check(towerDefensePlant.instance.maskFlags == towerDefensePlantConfig.maskFlags, "Revived plants must restore their configured projectile target mask.");
		towerDefensePlant.Free();
	}

	private void VerifyWallnutDamageStageRecovery()
	{
		TowerDefensePlantConfig towerDefensePlantConfig = ResourceLoader.Load<TowerDefensePlantConfig>("res://Asset/Anime/Character/Plant/Chapter9/WallnutQX/Config/TowerDefensePlantWallnutQX.tres", null, ResourceLoader.CacheMode.Ignore);
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter9/WallnutQX/WallnutQX.tscn", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(towerDefensePlantConfig), "The production holographic Wallnut config must load.");
		Check(GodotObject.IsInstanceValid(packedScene), "The production holographic Wallnut sprite must load.");
		if (GodotObject.IsInstanceValid(towerDefensePlantConfig) && GodotObject.IsInstanceValid(packedScene))
		{
			TowerDefensePlantWallnutQX towerDefensePlantWallnutQX = new TowerDefensePlantWallnutQX();
			AdobeAnimateSprite adobeAnimateSprite = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			towerDefensePlantWallnutQX.config = towerDefensePlantConfig;
			towerDefensePlantWallnutQX.sprite = adobeAnimateSprite;
			towerDefensePlantWallnutQX.AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
			towerDefensePlantWallnutQX.instance = new TowerDefenseCharacterInstance();
			towerDefensePlantWallnutQX.instance._Init(towerDefensePlantWallnutQX, towerDefensePlantConfig);
			MarkDead(towerDefensePlantWallnutQX);
			ApplyDamageStage(towerDefensePlantWallnutQX, "Damage2", 3, "WallnutQX_body.png");
			typeof(TowerDefensePlantWallnutQX).GetMethod("RestoreFullHealthAfterRevive", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(towerDefensePlantWallnutQX, null);
			Check(Mathf.IsEqualApprox(towerDefensePlantWallnutQX.instance.hitpoints, towerDefensePlantWallnutQX.instance.hitpointsSave), "Holographic Wallnut must restore full hitpoints.");
			Check(towerDefensePlantWallnutQX.instance.damagePointIndex == 0, "Holographic Wallnut must reset damagePointIndex to zero at full hitpoints.");
			Check(string.IsNullOrEmpty(adobeAnimateSprite.GetAtlasReplacePath("WallnutQX_body.png")), "Holographic Wallnut must clear its damaged atlas replacement at full hitpoints.");
			towerDefensePlantWallnutQX.Free();
		}
	}

	private void VerifyGravestoneDamageStageRecovery()
	{
		TowerDefenseGravestoneConfig towerDefenseGravestoneConfig = ResourceLoader.Load<TowerDefenseGravestoneConfig>("res://Asset/Anime/Character/GraveStone/TargetQX/Config/TowerDefenseGraveStoneTargetQX.tres", null, ResourceLoader.CacheMode.Ignore);
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/GraveStone/TargetQX/GraveStoneTargetQX.tscn", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(towerDefenseGravestoneConfig), "The production holographic Gravestone config must load.");
		Check(GodotObject.IsInstanceValid(packedScene), "The production holographic Gravestone sprite must load.");
		if (GodotObject.IsInstanceValid(towerDefenseGravestoneConfig) && GodotObject.IsInstanceValid(packedScene))
		{
			TowerDefenseGraveStoneTargetQX towerDefenseGraveStoneTargetQX = new TowerDefenseGraveStoneTargetQX();
			AdobeAnimateSprite adobeAnimateSprite = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			towerDefenseGraveStoneTargetQX.config = towerDefenseGravestoneConfig;
			towerDefenseGraveStoneTargetQX.sprite = adobeAnimateSprite;
			towerDefenseGraveStoneTargetQX.AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
			towerDefenseGraveStoneTargetQX.instance = new TowerDefenseCharacterInstance();
			towerDefenseGraveStoneTargetQX.instance._Init(towerDefenseGraveStoneTargetQX, towerDefenseGravestoneConfig);
			MarkDead(towerDefenseGraveStoneTargetQX);
			towerDefenseGraveStoneTargetQX.instance.canBeCollection = false;
			ApplyDamageStage(towerDefenseGraveStoneTargetQX, "Damage4", 5, "TargetStonesQX_1.png");
			System.Reflection.MethodInfo method = typeof(TowerDefenseGraveStoneTargetQX).GetMethod("RestoreNormalState", BindingFlags.Instance | BindingFlags.NonPublic);
			Check(method != null, "Holographic Gravestone must expose its private normal-state restore operation.");
			method?.Invoke(towerDefenseGraveStoneTargetQX, null);
			Check(Mathf.IsEqualApprox(towerDefenseGraveStoneTargetQX.instance.hitpoints, towerDefenseGraveStoneTargetQX.instance.hitpointsSave), "Holographic Gravestone must restore full hitpoints.");
			Check(!towerDefenseGraveStoneTargetQX.instance.die && !towerDefenseGraveStoneTargetQX.die && !towerDefenseGraveStoneTargetQX.instance.nearDie && !towerDefenseGraveStoneTargetQX.nearDie, "Holographic Gravestone must clear its death and near-death flags.");
			Check(towerDefenseGraveStoneTargetQX.instance.damagePointIndex == 0, "Holographic Gravestone must reset damagePointIndex to zero at full hitpoints.");
			Check(string.IsNullOrEmpty(adobeAnimateSprite.GetAtlasReplacePath("TargetStonesQX_1.png")), "Holographic Gravestone must clear its damaged atlas replacement at full hitpoints.");
			Check(towerDefenseGraveStoneTargetQX.instance.canBeCollection, "Holographic Gravestone must restore its targetable state.");
			Check(towerDefenseGraveStoneTargetQX.instance.maskFlags == towerDefenseGravestoneConfig.maskFlags, "Holographic Gravestone must restore its projectile-blocking mask.");
			Check((GroundProjectileFlags() & towerDefenseGraveStoneTargetQX.instance.maskFlags) != 0, "Holographic Gravestone must block ordinary ground projectiles after revive.");
			towerDefenseGraveStoneTargetQX.Free();
		}
	}

	private static void MarkDead(TowerDefenseCharacter character)
	{
		character.instance.hitpoints = 0.0;
		character.instance.die = true;
		character.instance.nearDie = true;
		character.instance.collisionFlags = 0;
		character.instance.maskFlags = 4;
		character.die = true;
		character.nearDie = true;
	}

	private static int GroundProjectileFlags()
	{
		return 9;
	}

	private void ApplyDamageStage(TowerDefenseCharacter character, string damagePointName, int damagePointIndex, StringName mediaName)
	{
		character.instance.damagePointData.SetDamagePointFliters(character.sprite, damagePointName);
		character.instance.damagePointIndex = damagePointIndex;
		Check(!string.IsNullOrEmpty(character.sprite.GetAtlasReplacePath(mediaName)), "The damaged fixture for " + character.GetType().Name + " must install an atlas replacement.");
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[HolographicPlantReviveHealthRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(8)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifySharedPlantRecovery, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyWallnutDamageStageRecovery, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyGravestoneDamageStageRecovery, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MarkDead, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GroundProjectileFlags, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ApplyDamageStage, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "damagePointIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.StringName, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VerifySharedPlantRecovery && args.Count == 0)
		{
			VerifySharedPlantRecovery();
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyWallnutDamageStageRecovery && args.Count == 0)
		{
			VerifyWallnutDamageStageRecovery();
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyGravestoneDamageStageRecovery && args.Count == 0)
		{
			VerifyGravestoneDamageStageRecovery();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkDead && args.Count == 1)
		{
			MarkDead(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GroundProjectileFlags && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GroundProjectileFlags());
			return true;
		}
		if (method == MethodName.ApplyDamageStage && args.Count == 4)
		{
			ApplyDamageStage(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<StringName>(in args[3]));
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
		if (method == MethodName.MarkDead && args.Count == 1)
		{
			MarkDead(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GroundProjectileFlags && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GroundProjectileFlags());
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
		if (method == MethodName.VerifySharedPlantRecovery)
		{
			return true;
		}
		if (method == MethodName.VerifyWallnutDamageStageRecovery)
		{
			return true;
		}
		if (method == MethodName.VerifyGravestoneDamageStageRecovery)
		{
			return true;
		}
		if (method == MethodName.MarkDead)
		{
			return true;
		}
		if (method == MethodName.GroundProjectileFlags)
		{
			return true;
		}
		if (method == MethodName.ApplyDamageStage)
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
