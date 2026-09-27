using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieCobCannonExplosionRuntimeTest.cs")]
public class ZombieCobCannonExplosionRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnEffectChildEntered = "OnEffectChildEntered";

		public static readonly StringName OnHeadAnimeEvent = "OnHeadAnimeEvent";

		public static readonly StringName CaptureCobCannonParticles = "CaptureCobCannonParticles";

		public static readonly StringName ObservedCompleteExplosion = "ObservedCompleteExplosion";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _sawNaturalFireEvent = "_sawNaturalFireEvent";

		public static readonly StringName _sawExplosionWrapper = "_sawExplosionWrapper";

		public static readonly StringName _sawExplosionParticles = "_sawExplosionParticles";

		public static readonly StringName _particlesVisible = "_particlesVisible";

		public static readonly StringName _particlesEmitting = "_particlesEmitting";

		public static readonly StringName _particlesAmount = "_particlesAmount";

		public static readonly StringName _particlesHaveTexture = "_particlesHaveTexture";

		public static readonly StringName _particlesHaveProcessMaterial = "_particlesHaveProcessMaterial";

		public static readonly StringName _cobCannonParticlesEffect = "_cobCannonParticlesEffect";

		public static readonly StringName _expectedFireEventName = "_expectedFireEventName";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/CobCannon/TowerDefenseZombieNormalCobCannon.tscn";

	private const string CannonHeadPath = "SpriteGroup/TransformPoint/ZombieNormalCobCannon/Head";

	private const string CobCannonExplosionParticleName = "CobCannonExplosion";

	private int _checks;

	private int _failures;

	private bool _sawNaturalFireEvent;

	private bool _sawExplosionWrapper;

	private bool _sawExplosionParticles;

	private bool _particlesVisible;

	private bool _particlesEmitting;

	private int _particlesAmount;

	private bool _particlesHaveTexture;

	private bool _particlesHaveProcessMaterial;

	private TowerDefenseEffectParticlesOnce _cobCannonParticlesEffect;

	private string _expectedFireEventName = "fire";

	public override async void _Ready()
	{
		try
		{
			await VerifyRealZombieCannonExplosion();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[ZombieCobCannonExplosionRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"ZOMBIE_COB_CANNON_EXPLOSION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyRealZombieCannonExplosion()
	{
		TowerDefenseZombieNormalCobCannon zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/CobCannon/TowerDefenseZombieNormalCobCannon.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieNormalCobCannon>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(zombie), "The real zombie cannon scene must instantiate.");
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		zombie.inGame = false;
		zombie.editorPreviewMode = true;
		AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		for (int i = 0; i < 4; i++)
		{
			if (GodotObject.IsInstanceValid(BulletField.Instance))
			{
				break;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		if (!GodotObject.IsInstanceValid(BulletField.Instance))
		{
			TowerDefenseManager.GetCharacterNode().AddChild(new BulletField(), forceReadableName: false, InternalMode.Disabled);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		CannonComponent cannonComponent = zombie.componentManager?.GetRuntime<CannonComponent>("character.cannon");
		Check(cannonComponent != null && !cannonComponent.IsReleased, "The real zombie cannon must expose its CannonComponent runtime.");
		Check(cannonComponent?.projectileData?.projectileName.ToString() == "CobCannonCob", "The real zombie cannon must use CobCannonCob.");
		TowerDefenseProjectileRegistry.Init();
		Check(TowerDefenseProjectileRegistry.GetProjectile("CobCannonCob")?.hitEffect != null, "The projectile registry copy must retain CobCannonCob's hit effect.");
		Check((cannonComponent?.projectileData?.BuildConfig())?.hitEffect != null, "CobCannonCob must retain its configured explosion hit effect.");
		Check(ResourceLoader.Load<TowerDefenseProjectileData>("res://Registry/Projectile/Config/CannonCob/CobCannonCob.tres", null, ResourceLoader.CacheMode.Ignore)?.hitEffect != null, "The canonical CobCannonCob registry resource must load its hit effect.");
		Check(GodotObject.IsInstanceValid(BulletField.Instance), "The production BulletField path must be mounted.");
		AdobeAnimateSpriteBase head = zombie.GetNodeOrNull<AdobeAnimateSpriteBase>("SpriteGroup/TransformPoint/ZombieNormalCobCannon/Head");
		Check(GodotObject.IsInstanceValid(head), "The real nested cannon Head must exist.");
		if (cannonComponent?.projectileData == null || !GodotObject.IsInstanceValid(BulletField.Instance) || !GodotObject.IsInstanceValid(head))
		{
			return;
		}
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		Check(GodotObject.IsInstanceValid(characterNode), "The production character/effect node must exist.");
		characterNode.ChildEnteredTree += OnEffectChildEntered;
		head.OnAnimeEvent += OnHeadAnimeEvent;
		try
		{
			cannonComponent.markerVisualTravelDuration = 0.0;
			cannonComponent.targetPos = new Vector2(300f, 200f);
			_expectedFireEventName = cannonComponent.fireEventName;
			head.timeScale = 1.0;
			head.SetAnimation(cannonComponent.fireAnimeClips, loop: false);
			for (int i = 0; i < 180; i++)
			{
				if (ObservedCompleteExplosion())
				{
					break;
				}
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
				CaptureCobCannonParticles(_cobCannonParticlesEffect);
			}
		}
		finally
		{
			head.OnAnimeEvent -= OnHeadAnimeEvent;
			characterNode.ChildEnteredTree -= OnEffectChildEntered;
		}
		Check(_sawNaturalFireEvent, "The real nested cannon Head Fire animation must naturally emit its fire timeline event.");
		Check(_sawExplosionWrapper, "The natural Fire animation path must create TowerDefenseProjectileEffectCobCannonExplode on landing.");
		Check(_sawExplosionParticles, "The cob-cannon wrapper must create the concrete CobCannonExplosion particle effect.");
		Check(_particlesVisible, "The concrete CobCannonExplosion particles must be visible.");
		Check(_particlesEmitting, "The concrete CobCannonExplosion particles must be emitting.");
		Check(_particlesAmount == 48, "The concrete CobCannonExplosion particles must keep amount 48.");
		Check(_particlesHaveTexture, "The concrete CobCannonExplosion particles must have a texture.");
		Check(_particlesHaveProcessMaterial, "The concrete CobCannonExplosion particles must have a process material.");
		zombie.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private void OnEffectChildEntered(Node child)
	{
		if (child is TowerDefenseProjectileEffectCobCannonExplode)
		{
			_sawExplosionWrapper = true;
		}
		if (child is TowerDefenseEffectParticlesOnce effect)
		{
			CaptureCobCannonParticles(effect);
		}
	}

	private void OnHeadAnimeEvent(string command, Variant argument)
	{
		if (command == _expectedFireEventName)
		{
			_sawNaturalFireEvent = true;
		}
	}

	private void CaptureCobCannonParticles(TowerDefenseEffectParticlesOnce effect)
	{
		if (GodotObject.IsInstanceValid(effect) && GodotObject.IsInstanceValid(effect.particles) && !(effect.particles.Name != (StringName)"CobCannonExplosion"))
		{
			_cobCannonParticlesEffect = effect;
			GPUParticles2DOnece particles = effect.particles;
			_sawExplosionParticles = true;
			_particlesVisible |= particles.Visible;
			_particlesEmitting |= particles.Emitting;
			_particlesAmount = particles.Amount;
			_particlesHaveTexture |= particles.Texture != null;
			_particlesHaveProcessMaterial |= particles.ProcessMaterial != null;
		}
	}

	private bool ObservedCompleteExplosion()
	{
		if (_sawNaturalFireEvent && _sawExplosionWrapper && _sawExplosionParticles && _particlesVisible && _particlesEmitting && _particlesAmount == 48 && _particlesHaveTexture)
		{
			return _particlesHaveProcessMaterial;
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[ZombieCobCannonExplosionRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEffectChildEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHeadAnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureCobCannonParticles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "effect", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ObservedCompleteExplosion, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.OnEffectChildEntered && args.Count == 1)
		{
			OnEffectChildEntered(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHeadAnimeEvent && args.Count == 2)
		{
			OnHeadAnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureCobCannonParticles && args.Count == 1)
		{
			CaptureCobCannonParticles(VariantUtils.ConvertTo<TowerDefenseEffectParticlesOnce>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ObservedCompleteExplosion && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ObservedCompleteExplosion());
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
		if (method == MethodName.OnEffectChildEntered)
		{
			return true;
		}
		if (method == MethodName.OnHeadAnimeEvent)
		{
			return true;
		}
		if (method == MethodName.CaptureCobCannonParticles)
		{
			return true;
		}
		if (method == MethodName.ObservedCompleteExplosion)
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
		if (name == PropertyName._sawNaturalFireEvent)
		{
			_sawNaturalFireEvent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sawExplosionWrapper)
		{
			_sawExplosionWrapper = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sawExplosionParticles)
		{
			_sawExplosionParticles = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._particlesVisible)
		{
			_particlesVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._particlesEmitting)
		{
			_particlesEmitting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._particlesAmount)
		{
			_particlesAmount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._particlesHaveTexture)
		{
			_particlesHaveTexture = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._particlesHaveProcessMaterial)
		{
			_particlesHaveProcessMaterial = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cobCannonParticlesEffect)
		{
			_cobCannonParticlesEffect = VariantUtils.ConvertTo<TowerDefenseEffectParticlesOnce>(in value);
			return true;
		}
		if (name == PropertyName._expectedFireEventName)
		{
			_expectedFireEventName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName._sawNaturalFireEvent)
		{
			value = VariantUtils.CreateFrom(in _sawNaturalFireEvent);
			return true;
		}
		if (name == PropertyName._sawExplosionWrapper)
		{
			value = VariantUtils.CreateFrom(in _sawExplosionWrapper);
			return true;
		}
		if (name == PropertyName._sawExplosionParticles)
		{
			value = VariantUtils.CreateFrom(in _sawExplosionParticles);
			return true;
		}
		if (name == PropertyName._particlesVisible)
		{
			value = VariantUtils.CreateFrom(in _particlesVisible);
			return true;
		}
		if (name == PropertyName._particlesEmitting)
		{
			value = VariantUtils.CreateFrom(in _particlesEmitting);
			return true;
		}
		if (name == PropertyName._particlesAmount)
		{
			value = VariantUtils.CreateFrom(in _particlesAmount);
			return true;
		}
		if (name == PropertyName._particlesHaveTexture)
		{
			value = VariantUtils.CreateFrom(in _particlesHaveTexture);
			return true;
		}
		if (name == PropertyName._particlesHaveProcessMaterial)
		{
			value = VariantUtils.CreateFrom(in _particlesHaveProcessMaterial);
			return true;
		}
		if (name == PropertyName._cobCannonParticlesEffect)
		{
			value = VariantUtils.CreateFrom(in _cobCannonParticlesEffect);
			return true;
		}
		if (name == PropertyName._expectedFireEventName)
		{
			value = VariantUtils.CreateFrom(in _expectedFireEventName);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sawNaturalFireEvent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sawExplosionWrapper, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sawExplosionParticles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesEmitting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._particlesAmount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesHaveTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesHaveProcessMaterial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cobCannonParticlesEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._expectedFireEventName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._sawNaturalFireEvent, Variant.From(in _sawNaturalFireEvent));
		info.AddProperty(PropertyName._sawExplosionWrapper, Variant.From(in _sawExplosionWrapper));
		info.AddProperty(PropertyName._sawExplosionParticles, Variant.From(in _sawExplosionParticles));
		info.AddProperty(PropertyName._particlesVisible, Variant.From(in _particlesVisible));
		info.AddProperty(PropertyName._particlesEmitting, Variant.From(in _particlesEmitting));
		info.AddProperty(PropertyName._particlesAmount, Variant.From(in _particlesAmount));
		info.AddProperty(PropertyName._particlesHaveTexture, Variant.From(in _particlesHaveTexture));
		info.AddProperty(PropertyName._particlesHaveProcessMaterial, Variant.From(in _particlesHaveProcessMaterial));
		info.AddProperty(PropertyName._cobCannonParticlesEffect, Variant.From(in _cobCannonParticlesEffect));
		info.AddProperty(PropertyName._expectedFireEventName, Variant.From(in _expectedFireEventName));
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
		if (info.TryGetProperty(PropertyName._sawNaturalFireEvent, out var value3))
		{
			_sawNaturalFireEvent = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sawExplosionWrapper, out var value4))
		{
			_sawExplosionWrapper = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sawExplosionParticles, out var value5))
		{
			_sawExplosionParticles = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._particlesVisible, out var value6))
		{
			_particlesVisible = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._particlesEmitting, out var value7))
		{
			_particlesEmitting = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._particlesAmount, out var value8))
		{
			_particlesAmount = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._particlesHaveTexture, out var value9))
		{
			_particlesHaveTexture = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._particlesHaveProcessMaterial, out var value10))
		{
			_particlesHaveProcessMaterial = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cobCannonParticlesEffect, out var value11))
		{
			_cobCannonParticlesEffect = value11.As<TowerDefenseEffectParticlesOnce>();
		}
		if (info.TryGetProperty(PropertyName._expectedFireEventName, out var value12))
		{
			_expectedFireEventName = value12.As<string>();
		}
	}
}
