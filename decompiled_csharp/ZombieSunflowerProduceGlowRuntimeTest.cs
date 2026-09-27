using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ZombieSunflowerProduceGlowRuntimeTest.cs")]
public class ZombieSunflowerProduceGlowRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VariantName = "VariantName";

		public static readonly StringName ColorsEqual = "ColorsEqual";

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

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Sunflower/TowerDefenseZombieNormalSunflower.tscn";

	private const float ColorTolerance = 0.002f;

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 1;
		try
		{
			await VerifyGlow(hypnotized: false);
			await VerifyGlow(hypnotized: true);
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[ZombieSunflowerProduceGlowRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"ZOMBIE_SUNFLOWER_PRODUCE_GLOW_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyGlow(bool hypnotized)
	{
		TowerDefenseZombieNormalSunflower zombie = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Sunflower/TowerDefenseZombieNormalSunflower.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieNormalSunflower>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(zombie), "The real Zombie Sunflower scene must instantiate.");
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		zombie.inGame = false;
		zombie.editorPreviewMode = true;
		AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		ProduceComponent produceComponent = zombie.componentManager?.GetRuntime<ProduceComponent>("character.produce");
		AdobeAnimateSpriteBase rootSprite = zombie.sprite as AdobeAnimateSpriteBase;
		AdobeAnimateSpriteBase headSprite = zombie.GetNodeOrNull<AdobeAnimateSpriteBase>("SpriteGroup/TransformPoint/ZombieNormalSunflower/Head");
		Check(produceComponent != null && !produceComponent.IsReleased, "Zombie Sunflower must expose its real ProduceComponent runtime.");
		Check(GodotObject.IsInstanceValid(rootSprite) && GodotObject.IsInstanceValid(headSprite), "Zombie Sunflower must expose both body and nested sunflower-head animations.");
		if (produceComponent == null || produceComponent.IsReleased || !GodotObject.IsInstanceValid(rootSprite) || !GodotObject.IsInstanceValid(headSprite))
		{
			zombie.QueueFree();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			return;
		}
		if (hypnotized)
		{
			TowerDefenseZombieNormalSunflower towerDefenseZombieNormalSunflower = zombie;
			if (towerDefenseZombieNormalSunflower.attackComponent == null)
			{
				towerDefenseZombieNormalSunflower.attackComponent = zombie.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
			}
			ZombieSunflowerProduceGlowRuntimeTest zombieSunflowerProduceGlowRuntimeTest = this;
			AttackComponent attackComponent = zombie.attackComponent;
			zombieSunflowerProduceGlowRuntimeTest.Check(attackComponent != null && !attackComponent.IsReleased, "The real Zombie Sunflower attack runtime must be available for its hypnosis transition.");
			zombie.Hypnoses(60.0, canFliter: false);
			Check(zombie.instance.hypnoses && produceComponent.produceType == "Sun", "The reported hypnotized Zombie Sunflower must use the same producer while emitting Sun.");
		}
		else
		{
			Check(!zombie.instance.hypnoses && produceComponent.produceType == "BrainSun", "The normal Zombie Sunflower must use its real BrainSun producer.");
		}
		Check(produceComponent.produceGlowTarget == headSprite, "Production glow must resolve to the nested sunflower head, not the whole zombie.");
		Color rootModulateBefore = rootSprite.Modulate;
		Color rootSelfBefore = rootSprite.SelfModulate;
		Color headModulateBefore = headSprite.Modulate;
		Color headSelfBefore = headSprite.SelfModulate;
		string rootClip = rootSprite.clip;
		string headClip = headSprite.clip;
		float maximumHeadBrightness = headSelfBefore.R;
		float previousBrightness = headSelfBefore.R;
		int direction = 0;
		int directionChanges = 0;
		bool wholeZombieChanged = false;
		bool glowObserved = false;
		double elapsedSeconds = 0.0;
		double glowTimeoutSeconds = Math.Max(0.001, produceComponent.glowFadeInTime) + Math.Max(0.001, produceComponent.glowFadeOutTime) + 1.0;
		produceComponent._StartProduceGlow();
		while (elapsedSeconds < glowTimeoutSeconds)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			elapsedSeconds += GetProcessDeltaTime();
			maximumHeadBrightness = Mathf.Max(maximumHeadBrightness, headSprite.SelfModulate.R);
			wholeZombieChanged |= !ColorsEqual(rootSprite.Modulate, rootModulateBefore) || !ColorsEqual(rootSprite.SelfModulate, rootSelfBefore) || !ColorsEqual(headSprite.Modulate, headModulateBefore);
			glowObserved |= !ColorsEqual(headSprite.SelfModulate, headSelfBefore);
			float num = headSprite.SelfModulate.R - previousBrightness;
			int num2 = ((num > 0.002f) ? 1 : ((num < -0.002f) ? (-1) : 0));
			if (num2 != 0)
			{
				if (direction != 0 && num2 != direction)
				{
					directionChanges++;
				}
				direction = num2;
			}
			previousBrightness = headSprite.SelfModulate.R;
			if (glowObserved && ColorsEqual(headSprite.SelfModulate, headSelfBefore))
			{
				break;
			}
		}
		Check(glowObserved && ColorsEqual(headSprite.SelfModulate, headSelfBefore), $"{VariantName(hypnotized)} production glow must finish and restore within {glowTimeoutSeconds:0.###} seconds of accumulated process time.");
		Check(!wholeZombieChanged, VariantName(hypnotized) + " production must not flash the zombie body or compound the nested head's inherited Modulate.");
		Check(maximumHeadBrightness >= headSelfBefore.R + 0.25f, VariantName(hypnotized) + " production must visibly brighten the sunflower head.");
		Check(directionChanges <= 1, $"{VariantName(hypnotized)} sunflower-head brightness must rise and fall smoothly without high-frequency reversals; reversals={directionChanges}.");
		Check(ColorsEqual(headSprite.SelfModulate, headSelfBefore), VariantName(hypnotized) + " sunflower-head glow must restore its original SelfModulate.");
		Check(rootSprite.clip == rootClip && headSprite.clip == headClip, VariantName(hypnotized) + " production glow must not restart body or head animation clips.");
		zombie.QueueFree();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private static string VariantName(bool hypnotized)
	{
		if (!hypnotized)
		{
			return "Zombie Sunflower";
		}
		return "Hypnotized Zombie Sunflower";
	}

	private static bool ColorsEqual(Color left, Color right)
	{
		if (Mathf.Abs(left.R - right.R) <= 0.002f && Mathf.Abs(left.G - right.G) <= 0.002f && Mathf.Abs(left.B - right.B) <= 0.002f)
		{
			return Mathf.Abs(left.A - right.A) <= 0.002f;
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[ZombieSunflowerProduceGlowRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VariantName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hypnotized", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ColorsEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.VariantName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(VariantName(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ColorsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ColorsEqual(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.VariantName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(VariantName(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ColorsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ColorsEqual(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
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
		if (method == MethodName.VariantName)
		{
			return true;
		}
		if (method == MethodName.ColorsEqual)
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
