using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Tools;

[ScriptPath("res://Test/ZombieBossEdgarIIResourceRuntimeTest.cs")]
public class ZombieBossEdgarIIResourceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CheckUid = "CheckUid";

		public static readonly StringName HasEvent = "HasEvent";

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

	private const string Root = "res://Asset/Anime/Character/Zombie/Boss/EdgarII";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Boss/EdgarII/Scene/TowerDefenseZombieBossEdgarII.tscn", null, ResourceLoader.CacheMode.Reuse);
		Check(packedScene != null, "Character scene must load.");
		TowerDefenseZombieBossEdgarII towerDefenseZombieBossEdgarII = packedScene?.Instantiate<TowerDefenseZombieBossEdgarII>(PackedScene.GenEditState.Disabled);
		Check(towerDefenseZombieBossEdgarII != null, "Character scene must instantiate the Edgar II class.");
		if (towerDefenseZombieBossEdgarII != null)
		{
			Check(towerDefenseZombieBossEdgarII.config is TowerDefenseZombieConfig, "Character config must be a zombie config.");
			Check(Mathf.IsEqualApprox((float)towerDefenseZombieBossEdgarII.config.hitpoints, 250000f), "HP must be 250000.");
			Check(towerDefenseZombieBossEdgarII.config.unUseBuffFlags == 15, "Slow, frozen, butter and hypnoses immunity flags must equal 15.");
			Check(towerDefenseZombieBossEdgarII.config.physiqueTypeFlags == 1024, "Projectile penetration blocking flag must equal 1024.");
			AdobeAnimateSpriteBase nodeOrNull = towerDefenseZombieBossEdgarII.GetNodeOrNull<AdobeAnimateSpriteBase>("SpriteGroup/TransformPoint/ZombieBossEdgarII");
			Check(nodeOrNull != null, "Character scene must wire the Edgar II sprite.");
			Check(nodeOrNull != null && nodeOrNull.Scale.X < 0.95f, "Edgar II model scale must stay below the original head threshold.");
			Check(typeof(TowerDefenseZombieBossEdgarII).GetMethod("Walk")?.DeclaringType == typeof(TowerDefenseZombieBossEdgarII), "Boss must override Walk so zero movement speed cannot freeze Adobe Animate playback.");
			Check(Mathf.IsEqualApprox((float)towerDefenseZombieBossEdgarII.walkSpeedScale, 1f) && towerDefenseZombieBossEdgarII.walkAnimeClip == "Walk", "Boss must use the ordinary zombie Walk/GroundMove entry path.");
			Check(typeof(TowerDefenseZombieBossEdgarII).GetField("IdleDuration", BindingFlags.Static | BindingFlags.NonPublic)?.GetRawConstantValue() is double num && Mathf.IsEqualApprox((float)num, 10f), "Boss must wait 10 seconds between active skills.");
			Dictionary dictionary = towerDefenseZombieBossEdgarII.ExportNetworkSpecialState();
			Check(dictionary.GetValueOrDefault("schemaVersion", 0).AsInt32() >= 2 && dictionary.ContainsKey("pulseActionSequence") && dictionary.ContainsKey("pulseAffectedCharacterIds") && dictionary.ContainsKey("idleRowMoveActive") && dictionary.ContainsKey("idleMoveWaitTimer") && dictionary.ContainsKey("animationFrame"), "Network state must carry the idempotent pulse presentation event.");
			Dictionary dictionary2 = towerDefenseZombieBossEdgarII.ExportVariantSave();
			Check(!dictionary2.ContainsKey("pulseActionSequence"), "Progress saves must not replay transient pulse presentation events.");
			Check(!dictionary2.ContainsKey("rev"), "Progress saves must not overwrite network revision ordering state.");
			towerDefenseZombieBossEdgarII.Free();
		}
		ParameterInfo[] array = typeof(XWAdobeAnimateXflDatExporter).GetMethod("TryExport", BindingFlags.Static | BindingFlags.Public)?.GetParameters();
		Check(array != null && array.Length != 0 && array[^1].HasDefaultValue && array[^1].DefaultValue is XWAdobeAnimateClipEndMode xWAdobeAnimateClipEndMode && xWAdobeAnimateClipEndMode == XWAdobeAnimateClipEndMode.LegacyLastFrame, "The exporter must preserve legacy clip endpoints unless a caller explicitly opts in.");
		AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>("res://Asset/Anime/Character/Zombie/Boss/EdgarII/Animation/ZombieBossEdgarII.tres", null, ResourceLoader.CacheMode.Reuse);
		Check(adobeAnimateData?.HasPackedRuntimeData() ?? false, "Main animation must contain packed runtime data.");
		Check(adobeAnimateData?.GetClip("Idle") == new Vector2I(0, 22), "Idle clip must use the Edgar II end-exclusive endpoint.");
		Check(adobeAnimateData?.GetClip("Death") == new Vector2I(328, 387), "Death clip must expose authored frame 386.");
		Check(HasEvent(adobeAnimateData, 136, "smash"), "Smash gameplay event must be present.");
		Check(HasEvent(adobeAnimateData, 205, "fire"), "Fire gameplay event must be present.");
		Check(HasEvent(adobeAnimateData, 253, "pulse"), "Pulse gameplay event must be present.");
		Check(HasEvent(adobeAnimateData, 295, "summon"), "Summon gameplay event must be present.");
		Check(adobeAnimateData != null && adobeAnimateData.mediaDictionary?.Count == 62, "Main animation must retain all 62 bitmap media items from the authoritative FLA.");
		Check(adobeAnimateData != null && adobeAnimateData.mediaDictionary?.ContainsKey("Zombie_BossEdgar_head.png") == true && adobeAnimateData.mediaDictionary.ContainsKey("Zombie_boss_innerjaw.png"), "Main animation must retain the authored head and inner-jaw media.");
		AdobeAnimateSpriteBase adobeAnimateSpriteBase = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Boss/EdgarII/ZombieBossEdgarII.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<AdobeAnimateSpriteBase>(PackedScene.GenEditState.Disabled);
		Check(adobeAnimateSpriteBase != null, "Packet sprite scene must instantiate.");
		Check(adobeAnimateSpriteBase?.offset == new Vector2(-40f, -80f), "Packet sprite must offset the 80x139 Animate canvas into the preview crop.");
		Check(adobeAnimateSpriteBase != null && !adobeAnimateSpriteBase.GetFliter("_ground"), "The authored Ground movement layer must remain available but never render.");
		if (adobeAnimateSpriteBase != null)
		{
			adobeAnimateSpriteBase.timeScale = 1.0;
			adobeAnimateSpriteBase.SetAnimation("Idle");
			int frameIndex = adobeAnimateSpriteBase.frameIndex;
			adobeAnimateSpriteBase.RunBatchedProcessUpdate(0.25);
			Check(adobeAnimateSpriteBase.frameIndex != frameIndex, "Imported Idle animation must advance through the production batch playback path.");
			adobeAnimateSpriteBase.Free();
		}
		System.Reflection.MethodInfo method = typeof(TowerDefenseZombieBossEdgarII).GetMethod("ResolveHomeGridPosition", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		Check(method != null, "Boss must expose a deterministic home-grid resolver for spawn placement.");
		if (method != null)
		{
			Vector2I vector2I = (Vector2I)method.Invoke(null, new object[2]
			{
				new Vector2I(9, 5),
				3
			});
			Vector2I vector2I2 = (Vector2I)method.Invoke(null, new object[2]
			{
				new Vector2I(7, 5),
				9
			});
			Check(vector2I == new Vector2I(9, 3), "Boss must spawn at column nine on a standard map.");
			Check(vector2I2 == new Vector2I(7, 5), "Boss home position must clamp to narrow map bounds.");
		}
		Check(ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Boss/EdgarII/Packet/ZombieBossEdgarII.tres", null, ResourceLoader.CacheMode.Reuse)?.saveKey == "ZombieBossEdgarII", "Packet save key must be stable.");
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = ResourceLoader.Load<TowerDefenseProjectileConfig>("res://Asset/Config/Projectile/EdgarII/ZombieBossEdgarIIFireball.tres", null, ResourceLoader.CacheMode.Reuse);
		Check(towerDefenseProjectileConfig != null && Mathf.IsEqualApprox((float)towerDefenseProjectileConfig.baseDamage, 2000f), "Fireball damage must be 2000.");
		Check(towerDefenseProjectileConfig != null && towerDefenseProjectileConfig.useDurabilityBlockingSweep && Mathf.IsEqualApprox((float)towerDefenseProjectileConfig.durabilityBlockingThreshold, 2000f), "Fireball durability blocking sweep must be enabled at 2000.");
		Check(towerDefenseProjectileConfig != null && towerDefenseProjectileConfig.penetrateNum == -1, "Fireball must retain unlimited penetration until blocked.");
		Check(towerDefenseProjectileConfig != null && towerDefenseProjectileConfig.scale == new Vector2(2.25f, 2.25f), "Fireball must enlarge the reused FirePea animation by 2.25x.");
		Check(towerDefenseProjectileConfig != null && Mathf.IsZeroApprox((float)towerDefenseProjectileConfig.rotateScale), "Animated FirePea must not retain the static-art tumble workaround.");
		Check(towerDefenseProjectileConfig?.hitEffect == null && string.IsNullOrEmpty(towerDefenseProjectileConfig?.splatAudio), "Fireball must deal straight-line contact damage without an explosion effect or sound.");
		Check(towerDefenseProjectileConfig?.splatScene?.ResourcePath == "res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn" && towerDefenseProjectileConfig.splatSceneType == "Sprite" && towerDefenseProjectileConfig.hitBody && !towerDefenseProjectileConfig.useRange, "Fireball must show a body-anchored FireSplats hit effect without range damage.");
		AdobeAnimateSpriteBase adobeAnimateSpriteBase2 = towerDefenseProjectileConfig?.projectileScene?.Instantiate<AdobeAnimateSpriteBase>(PackedScene.GenEditState.Disabled);
		Check(adobeAnimateSpriteBase2?.flashAnimeData?.GetClip("Idle") == new Vector2I(0, 24) && Mathf.IsEqualApprox((float)adobeAnimateSpriteBase2.flashAnimeData.frameRate, 12f), "Fireball must reuse the complete 25-frame FirePea animation at 12 FPS.");
		adobeAnimateSpriteBase2?.Free();
		AdobeAnimateSpriteBase adobeAnimateSpriteBase3 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Boss/EdgarII/Effect/EdgarSPDown.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<AdobeAnimateSpriteBase>(PackedScene.GenEditState.Disabled);
		Check(adobeAnimateSpriteBase3?.flashAnimeData?.GetClip("Fire") == new Vector2I(0, 13), "Attack-speed-down effect must expose its complete Fire clip.");
		Check(adobeAnimateSpriteBase3?.offset == new Vector2(-50f, -100f), "Attack-speed-down effect must be centered above each affected plant.");
		adobeAnimateSpriteBase3?.Free();
		CheckUid("uid://moidupmva0vo", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Scene/TowerDefenseZombieBossEdgarII.tscn");
		CheckUid("uid://dk1epfrva36rp", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Packet/ZombieBossEdgarII.tres");
		CheckUid("uid://bh5hotdp0gpng", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/ZombieBossEdgarII.tscn");
		CheckUid("uid://e57i8n4afsci", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Config/TowerDefenseZombieBossEdgarII.tres");
		CheckUid("uid://ccy78cxlmoxe7", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Effect/EdgarSPDown.tscn");
		CheckUid("uid://k4cpmbeug7bh", "res://Asset/Config/Projectile/EdgarII/ZombieBossEdgarIIFireball.tres");
		bool flag = _failures == 0;
		GD.Print($"ZOMBIE_BOSS_EDGAR_II_RESOURCE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void CheckUid(string uidText, string expectedPath)
	{
		long num = ResourceUid.TextToId(uidText);
		Check(num != -1 && ResourceUid.HasId(num) && ResourceUid.GetIdPath(num) == expectedPath, $"Resource UID {uidText} must resolve to {expectedPath}.");
	}

	private static bool HasEvent(AdobeAnimateData data, int frame, string command)
	{
		if (data?.events == null || frame < 0 || frame >= data.events.Count)
		{
			return false;
		}
		Array array = data.events[frame];
		if (array == null)
		{
			return false;
		}
		foreach (Variant item in array)
		{
			if (item.AsGodotDictionary().GetValueOrDefault("Command", "").AsString() == command)
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
			GD.PushError("ZOMBIE_BOSS_EDGAR_II_RESOURCE_FAILURE " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(4)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CheckUid, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "uidText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "expectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HasEvent, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CheckUid && args.Count == 2)
		{
			CheckUid(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasEvent && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEvent(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
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
		if (method == MethodName.HasEvent && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEvent(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
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
		if (method == MethodName.CheckUid)
		{
			return true;
		}
		if (method == MethodName.HasEvent)
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
