using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewIceCobCannonLaunchAudioRuntimeTest.cs")]
public class BugOverviewIceCobCannonLaunchAudioRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnEffectChildEntered = "OnEffectChildEntered";

		public static readonly StringName OnAnimeEvent = "OnAnimeEvent";

		public static readonly StringName CaptureIceCobCannonParticles = "CaptureIceCobCannonParticles";

		public static readonly StringName ObservedCompleteExplosion = "ObservedCompleteExplosion";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _sawFire = "_sawFire";

		public static readonly StringName _sawExplosionWrapper = "_sawExplosionWrapper";

		public static readonly StringName _sawExplosionParticles = "_sawExplosionParticles";

		public static readonly StringName _particlesVisible = "_particlesVisible";

		public static readonly StringName _particlesEmitting = "_particlesEmitting";

		public static readonly StringName _particlesAmount = "_particlesAmount";

		public static readonly StringName _particlesHaveTexture = "_particlesHaveTexture";

		public static readonly StringName _particlesHaveProcessMaterial = "_particlesHaveProcessMaterial";

		public static readonly StringName _iceCobCannonParticlesEffect = "_iceCobCannonParticlesEffect";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ScenePath = "res://Asset/Anime/Character/Plant/Gold/IceCobCannon/Scene/TowerDefensePlantIceCobCannon.tscn";

	private int _checks;

	private int _failures;

	private bool _sawFire;

	private bool _sawExplosionWrapper;

	private bool _sawExplosionParticles;

	private bool _particlesVisible;

	private bool _particlesEmitting;

	private int _particlesAmount;

	private bool _particlesHaveTexture;

	private bool _particlesHaveProcessMaterial;

	private TowerDefenseEffectParticlesOnce _iceCobCannonParticlesEffect;

	public override async void _Ready()
	{
		TowerDefensePlantIceCobCannon plant = null;
		ProcessModeEnum previousAudioProcessMode = ProcessModeEnum.Inherit;
		Resource previousCobLaunch = null;
		bool hadCobLaunch = false;
		try
		{
			_ = 4;
			try
			{
				plant = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Gold/IceCobCannon/Scene/TowerDefensePlantIceCobCannon.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantIceCobCannon>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(plant), "The real Ice Cob Cannon scene must instantiate.");
				if (!GodotObject.IsInstanceValid(plant))
				{
					goto end_IL_0064;
				}
				plant.editorPreviewMode = true;
				plant.inGame = false;
				AddChild(plant, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(3);
				CannonComponent cannon = plant.componentManager?.GetRuntime<CannonComponent>("character.cannon");
				Check(cannon != null && !cannon.IsReleased, "The real Ice Cob Cannon must expose its CannonComponent runtime.");
				Check(GodotObject.IsInstanceValid(cannon?.sprite), "The CannonComponent must bind the real Ice Cob Cannon animation sprite.");
				Check(cannon?.fireEventName == "fire" && cannon.fireReadyAudioName == "CobLaunch", "Ice Cob Cannon must retain the authored fire-event/audio contract.");
				plant.OnCustomSwitched("Custom0");
				Check(cannon?.projectileData?.skinName == new StringName("IceCream"), "The real Custom0 path must select the IceCream projectile skin.");
				TowerDefenseProjectileRegistry.Init();
				Check(TowerDefenseProjectileRegistry.GetProjectile("IceCobCannonCob")?.hitEffect != null, "The projectile registry copy must retain IceCobCannonCob's hit effect.");
				Check((cannon?.projectileData?.BuildConfig())?.hitEffect != null, "IceCobCannonCob must retain its configured landing effect.");
				BulletField bulletField = BulletField.EnsureMountedOnCharacterNode();
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					TowerDefenseManager.GetCharacterNode().AddChild(new BulletField(), forceReadableName: false, InternalMode.Disabled);
					await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
					bulletField = BulletField.Instance;
				}
				Check(GodotObject.IsInstanceValid(bulletField) && BulletField.Instance == bulletField, "The production BulletField path must be mounted.");
				hadCobLaunch = ResourceManager.Instance?.AUDIOS.TryGetValue("CobLaunch", out previousCobLaunch) ?? false;
				if (!hadCobLaunch && GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					ResourceManager.Instance.AUDIOS["CobLaunch"] = ResourceLoader.Load<AudioStream>("uid://dg4whq0aisres", null, ResourceLoader.CacheMode.Ignore);
				}
				Check(ResourceManager.Instance?.AUDIOS.ContainsKey("CobLaunch") ?? false, "The production CobLaunch audio resource must be registered.");
				Check(GodotObject.IsInstanceValid(AudioManager.Instance), "The production AudioManager autoload must be available.");
				if (cannon == null || cannon.IsReleased || !GodotObject.IsInstanceValid(cannon.sprite) || !GodotObject.IsInstanceValid(AudioManager.Instance))
				{
					goto end_IL_0064;
				}
				previousAudioProcessMode = AudioManager.Instance.ProcessMode;
				AudioManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
				AudioManager.Instance.audioStack.Clear();
				Node2D characterNode = TowerDefenseManager.GetCharacterNode();
				Check(GodotObject.IsInstanceValid(characterNode), "The production character/effect node must exist.");
				cannon.sprite.OnAnimeEvent += OnAnimeEvent;
				characterNode.ChildEnteredTree += OnEffectChildEntered;
				try
				{
					cannon.markerVisualTravelDuration = 0.0;
					cannon.targetPos = new Vector2(300f, 200f);
					cannon.sprite.timeScale = 4.0;
					cannon.sprite.SetAnimation(cannon.fireAnimeClips, loop: false);
					for (int frame = 0; frame < 180; frame++)
					{
						if (ObservedCompleteExplosion())
						{
							break;
						}
						await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
						CaptureIceCobCannonParticles(_iceCobCannonParticlesEffect);
					}
				}
				finally
				{
					cannon.sprite.OnAnimeEvent -= OnAnimeEvent;
					characterNode.ChildEnteredTree -= OnEffectChildEntered;
				}
				Check(_sawFire, "The real Fire timeline must naturally emit fire.");
				Check(_sawExplosionWrapper, "The natural Fire animation path must create the Ice Cob Cannon landing effect.");
				Check(_sawExplosionParticles, "The landing effect must create the concrete IceCobCannonExplosion particles.");
				Check(_particlesVisible, "The concrete IceCobCannonExplosion particles must be visible.");
				Check(_particlesEmitting, "The concrete IceCobCannonExplosion particles must be emitting.");
				Check(_particlesAmount == 48, "The concrete IceCobCannonExplosion particles must keep amount 48.");
				Check(_particlesHaveTexture, "The concrete IceCobCannonExplosion particles must have a texture.");
				Check(_particlesHaveProcessMaterial, "The concrete IceCobCannonExplosion particles must have a process material.");
				int num = AudioManager.Instance.audioStack.Count((AudioManager.AudioRequest request) => request.Stream == "CobLaunch");
				GD.Print($"ICE_COB_CANNON_AUDIO_DIAGNOSTIC count={num} sameSprite={cannon.sprite == plant.sprite}");
				Check(num == 1, $"The natural fire event must enqueue CobLaunch exactly once; got {num}.");
				await WaitForPhysicsBatchRemainder(1uL);
				AudioManager.Instance._PhysicsProcess(0.0);
				Check(AudioManager.Instance.audioStack.Count((AudioManager.AudioRequest request) => request.Stream == "CobLaunch") == 1, "A non-boundary physics frame must retain the pending SFX batch.");
				await WaitForPhysicsBatchRemainder(0uL);
				AudioManager.Instance._PhysicsProcess(0.0);
				Check(AudioManager.Instance.audioStack.Count == 0, "The fourth physics frame must submit and clear the pending SFX batch.");
				goto end_IL_0045;
				end_IL_0064:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewIceCobCannonLaunchAudioRuntimeTest] Unexpected exception: {value}");
				goto end_IL_0045;
			}
			return;
			end_IL_0045:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(AudioManager.Instance))
			{
				AudioManager.Instance.ProcessMode = previousAudioProcessMode;
			}
			if (GodotObject.IsInstanceValid(ResourceManager.Instance))
			{
				if (hadCobLaunch)
				{
					ResourceManager.Instance.AUDIOS["CobLaunch"] = previousCobLaunch;
				}
				else
				{
					ResourceManager.Instance.AUDIOS.Remove("CobLaunch");
				}
			}
			if (GodotObject.IsInstanceValid(plant))
			{
				plant.QueueFree();
			}
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 22;
		GD.Print($"ICE_COB_CANNON_LAUNCH_AUDIO_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void OnEffectChildEntered(Node child)
	{
		if (child is TowerDefenseProjectileEffectIceCobCannonExplode)
		{
			_sawExplosionWrapper = true;
		}
		if (child is TowerDefenseEffectParticlesOnce effect)
		{
			CaptureIceCobCannonParticles(effect);
		}
	}

	private void OnAnimeEvent(string command, Variant argument)
	{
		if (command == "fire")
		{
			_sawFire = true;
		}
	}

	private void CaptureIceCobCannonParticles(TowerDefenseEffectParticlesOnce effect)
	{
		if (GodotObject.IsInstanceValid(effect) && GodotObject.IsInstanceValid(effect.particles) && !(effect.particles.Name != (StringName)"IceCobCannonExplosion"))
		{
			_iceCobCannonParticlesEffect = effect;
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
		if (_sawFire && _sawExplosionWrapper && _sawExplosionParticles && _particlesVisible && _particlesEmitting && _particlesAmount == 48 && _particlesHaveTexture)
		{
			return _particlesHaveProcessMaterial;
		}
		return false;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task WaitForPhysicsBatchRemainder(ulong remainder)
	{
		while (Engine.GetPhysicsFrames() % 4 != remainder)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewIceCobCannonLaunchAudioRuntimeTest] " + message);
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
			new MethodInfo(MethodName.OnAnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureIceCobCannonParticles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.OnAnimeEvent && args.Count == 2)
		{
			OnAnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureIceCobCannonParticles && args.Count == 1)
		{
			CaptureIceCobCannonParticles(VariantUtils.ConvertTo<TowerDefenseEffectParticlesOnce>(in args[0]));
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
		if (method == MethodName.OnAnimeEvent)
		{
			return true;
		}
		if (method == MethodName.CaptureIceCobCannonParticles)
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
		if (name == PropertyName._sawFire)
		{
			_sawFire = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._iceCobCannonParticlesEffect)
		{
			_iceCobCannonParticlesEffect = VariantUtils.ConvertTo<TowerDefenseEffectParticlesOnce>(in value);
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
		if (name == PropertyName._sawFire)
		{
			value = VariantUtils.CreateFrom(in _sawFire);
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
		if (name == PropertyName._iceCobCannonParticlesEffect)
		{
			value = VariantUtils.CreateFrom(in _iceCobCannonParticlesEffect);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName._sawFire, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sawExplosionWrapper, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sawExplosionParticles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesEmitting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._particlesAmount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesHaveTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._particlesHaveProcessMaterial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iceCobCannonParticlesEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._sawFire, Variant.From(in _sawFire));
		info.AddProperty(PropertyName._sawExplosionWrapper, Variant.From(in _sawExplosionWrapper));
		info.AddProperty(PropertyName._sawExplosionParticles, Variant.From(in _sawExplosionParticles));
		info.AddProperty(PropertyName._particlesVisible, Variant.From(in _particlesVisible));
		info.AddProperty(PropertyName._particlesEmitting, Variant.From(in _particlesEmitting));
		info.AddProperty(PropertyName._particlesAmount, Variant.From(in _particlesAmount));
		info.AddProperty(PropertyName._particlesHaveTexture, Variant.From(in _particlesHaveTexture));
		info.AddProperty(PropertyName._particlesHaveProcessMaterial, Variant.From(in _particlesHaveProcessMaterial));
		info.AddProperty(PropertyName._iceCobCannonParticlesEffect, Variant.From(in _iceCobCannonParticlesEffect));
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
		if (info.TryGetProperty(PropertyName._sawFire, out var value3))
		{
			_sawFire = value3.As<bool>();
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
		if (info.TryGetProperty(PropertyName._iceCobCannonParticlesEffect, out var value11))
		{
			_iceCobCannonParticlesEffect = value11.As<TowerDefenseEffectParticlesOnce>();
		}
	}
}
