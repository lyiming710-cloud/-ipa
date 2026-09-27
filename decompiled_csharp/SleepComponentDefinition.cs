using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/SleepComponent/SleepComponentDefinition.cs")]
public class SleepComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName sleepIndicatorScene = "sleepIndicatorScene";

		public static readonly StringName sleepIndicatorPosition = "sleepIndicatorPosition";

		public static readonly StringName sleepIndicatorHeadPosition = "sleepIndicatorHeadPosition";

		public static readonly StringName sleepAnimationBlend = "sleepAnimationBlend";

		public static readonly StringName wakeSquashAmount = "wakeSquashAmount";

		public static readonly StringName wakeStretchAmount = "wakeStretchAmount";

		public static readonly StringName wakeStepDuration = "wakeStepDuration";

		public static readonly StringName wakeEase = "wakeEase";

		public static readonly StringName wakeTransition = "wakeTransition";

		public static readonly StringName freezeTimeInIZM = "freezeTimeInIZM";

		public static readonly StringName sleepBuffName = "sleepBuffName";

		public static readonly StringName neverSleepValue = "neverSleepValue";

		public static readonly StringName daySleepValue = "daySleepValue";

		public static readonly StringName nightSleepValue = "nightSleepValue";

		public static readonly StringName mapRulesPreventSleep = "mapRulesPreventSleep";

		public static readonly StringName coffeePreventsSleep = "coffeePreventsSleep";

		public static readonly StringName matchingCellElementPreventsSleep = "matchingCellElementPreventsSleep";

		public static readonly StringName hostAuthoritativeState = "hostAuthoritativeState";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene sleepIndicatorScene;

	[Export(PropertyHint.None, "")]
	public Vector2 sleepIndicatorPosition = new Vector2(20f, 25f);

	[Export(PropertyHint.None, "")]
	public Vector2 sleepIndicatorHeadPosition = new Vector2(32f, -28f);

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double sleepAnimationBlend = 0.2;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float wakeSquashAmount = 0.25f;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float wakeStretchAmount = 0.1f;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float wakeStepDuration = 0.25f;

	[Export(PropertyHint.None, "")]
	public Tween.EaseType wakeEase = Tween.EaseType.Out;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType wakeTransition = Tween.TransitionType.Quart;

	[Export(PropertyHint.None, "")]
	public bool freezeTimeInIZM = true;

	[Export(PropertyHint.None, "")]
	public string sleepBuffName = "Sleep";

	[Export(PropertyHint.None, "")]
	public string neverSleepValue = "Never";

	[Export(PropertyHint.None, "")]
	public string daySleepValue = "Day";

	[Export(PropertyHint.None, "")]
	public string nightSleepValue = "Night";

	[Export(PropertyHint.None, "")]
	public bool mapRulesPreventSleep = true;

	[Export(PropertyHint.None, "")]
	public bool coffeePreventsSleep = true;

	[Export(PropertyHint.None, "")]
	public bool matchingCellElementPreventsSleep = true;

	[Export(PropertyHint.None, "")]
	public bool hostAuthoritativeState = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new SleepComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sleepIndicatorScene)
		{
			sleepIndicatorScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.sleepIndicatorPosition)
		{
			sleepIndicatorPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.sleepIndicatorHeadPosition)
		{
			sleepIndicatorHeadPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.sleepAnimationBlend)
		{
			sleepAnimationBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.wakeSquashAmount)
		{
			wakeSquashAmount = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.wakeStretchAmount)
		{
			wakeStretchAmount = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.wakeStepDuration)
		{
			wakeStepDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.wakeEase)
		{
			wakeEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.wakeTransition)
		{
			wakeTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		if (name == PropertyName.freezeTimeInIZM)
		{
			freezeTimeInIZM = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sleepBuffName)
		{
			sleepBuffName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.neverSleepValue)
		{
			neverSleepValue = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.daySleepValue)
		{
			daySleepValue = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.nightSleepValue)
		{
			nightSleepValue = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.mapRulesPreventSleep)
		{
			mapRulesPreventSleep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.coffeePreventsSleep)
		{
			coffeePreventsSleep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.matchingCellElementPreventsSleep)
		{
			matchingCellElementPreventsSleep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hostAuthoritativeState)
		{
			hostAuthoritativeState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.sleepIndicatorScene)
		{
			value = VariantUtils.CreateFrom(in sleepIndicatorScene);
			return true;
		}
		if (name == PropertyName.sleepIndicatorPosition)
		{
			value = VariantUtils.CreateFrom(in sleepIndicatorPosition);
			return true;
		}
		if (name == PropertyName.sleepIndicatorHeadPosition)
		{
			value = VariantUtils.CreateFrom(in sleepIndicatorHeadPosition);
			return true;
		}
		if (name == PropertyName.sleepAnimationBlend)
		{
			value = VariantUtils.CreateFrom(in sleepAnimationBlend);
			return true;
		}
		if (name == PropertyName.wakeSquashAmount)
		{
			value = VariantUtils.CreateFrom(in wakeSquashAmount);
			return true;
		}
		if (name == PropertyName.wakeStretchAmount)
		{
			value = VariantUtils.CreateFrom(in wakeStretchAmount);
			return true;
		}
		if (name == PropertyName.wakeStepDuration)
		{
			value = VariantUtils.CreateFrom(in wakeStepDuration);
			return true;
		}
		if (name == PropertyName.wakeEase)
		{
			value = VariantUtils.CreateFrom(in wakeEase);
			return true;
		}
		if (name == PropertyName.wakeTransition)
		{
			value = VariantUtils.CreateFrom(in wakeTransition);
			return true;
		}
		if (name == PropertyName.freezeTimeInIZM)
		{
			value = VariantUtils.CreateFrom(in freezeTimeInIZM);
			return true;
		}
		if (name == PropertyName.sleepBuffName)
		{
			value = VariantUtils.CreateFrom(in sleepBuffName);
			return true;
		}
		if (name == PropertyName.neverSleepValue)
		{
			value = VariantUtils.CreateFrom(in neverSleepValue);
			return true;
		}
		if (name == PropertyName.daySleepValue)
		{
			value = VariantUtils.CreateFrom(in daySleepValue);
			return true;
		}
		if (name == PropertyName.nightSleepValue)
		{
			value = VariantUtils.CreateFrom(in nightSleepValue);
			return true;
		}
		if (name == PropertyName.mapRulesPreventSleep)
		{
			value = VariantUtils.CreateFrom(in mapRulesPreventSleep);
			return true;
		}
		if (name == PropertyName.coffeePreventsSleep)
		{
			value = VariantUtils.CreateFrom(in coffeePreventsSleep);
			return true;
		}
		if (name == PropertyName.matchingCellElementPreventsSleep)
		{
			value = VariantUtils.CreateFrom(in matchingCellElementPreventsSleep);
			return true;
		}
		if (name == PropertyName.hostAuthoritativeState)
		{
			value = VariantUtils.CreateFrom(in hostAuthoritativeState);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.sleepIndicatorScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.sleepIndicatorPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.sleepIndicatorHeadPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.sleepAnimationBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.wakeSquashAmount, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.wakeStretchAmount, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.wakeStepDuration, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.wakeEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.wakeTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.freezeTimeInIZM, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.sleepBuffName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.neverSleepValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.daySleepValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.nightSleepValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mapRulesPreventSleep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.coffeePreventsSleep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.matchingCellElementPreventsSleep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hostAuthoritativeState, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sleepIndicatorScene, Variant.From(in sleepIndicatorScene));
		info.AddProperty(PropertyName.sleepIndicatorPosition, Variant.From(in sleepIndicatorPosition));
		info.AddProperty(PropertyName.sleepIndicatorHeadPosition, Variant.From(in sleepIndicatorHeadPosition));
		info.AddProperty(PropertyName.sleepAnimationBlend, Variant.From(in sleepAnimationBlend));
		info.AddProperty(PropertyName.wakeSquashAmount, Variant.From(in wakeSquashAmount));
		info.AddProperty(PropertyName.wakeStretchAmount, Variant.From(in wakeStretchAmount));
		info.AddProperty(PropertyName.wakeStepDuration, Variant.From(in wakeStepDuration));
		info.AddProperty(PropertyName.wakeEase, Variant.From(in wakeEase));
		info.AddProperty(PropertyName.wakeTransition, Variant.From(in wakeTransition));
		info.AddProperty(PropertyName.freezeTimeInIZM, Variant.From(in freezeTimeInIZM));
		info.AddProperty(PropertyName.sleepBuffName, Variant.From(in sleepBuffName));
		info.AddProperty(PropertyName.neverSleepValue, Variant.From(in neverSleepValue));
		info.AddProperty(PropertyName.daySleepValue, Variant.From(in daySleepValue));
		info.AddProperty(PropertyName.nightSleepValue, Variant.From(in nightSleepValue));
		info.AddProperty(PropertyName.mapRulesPreventSleep, Variant.From(in mapRulesPreventSleep));
		info.AddProperty(PropertyName.coffeePreventsSleep, Variant.From(in coffeePreventsSleep));
		info.AddProperty(PropertyName.matchingCellElementPreventsSleep, Variant.From(in matchingCellElementPreventsSleep));
		info.AddProperty(PropertyName.hostAuthoritativeState, Variant.From(in hostAuthoritativeState));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sleepIndicatorScene, out var value))
		{
			sleepIndicatorScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.sleepIndicatorPosition, out var value2))
		{
			sleepIndicatorPosition = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.sleepIndicatorHeadPosition, out var value3))
		{
			sleepIndicatorHeadPosition = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.sleepAnimationBlend, out var value4))
		{
			sleepAnimationBlend = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.wakeSquashAmount, out var value5))
		{
			wakeSquashAmount = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.wakeStretchAmount, out var value6))
		{
			wakeStretchAmount = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.wakeStepDuration, out var value7))
		{
			wakeStepDuration = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.wakeEase, out var value8))
		{
			wakeEase = value8.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.wakeTransition, out var value9))
		{
			wakeTransition = value9.As<Tween.TransitionType>();
		}
		if (info.TryGetProperty(PropertyName.freezeTimeInIZM, out var value10))
		{
			freezeTimeInIZM = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sleepBuffName, out var value11))
		{
			sleepBuffName = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.neverSleepValue, out var value12))
		{
			neverSleepValue = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.daySleepValue, out var value13))
		{
			daySleepValue = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.nightSleepValue, out var value14))
		{
			nightSleepValue = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.mapRulesPreventSleep, out var value15))
		{
			mapRulesPreventSleep = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.coffeePreventsSleep, out var value16))
		{
			coffeePreventsSleep = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.matchingCellElementPreventsSleep, out var value17))
		{
			matchingCellElementPreventsSleep = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hostAuthoritativeState, out var value18))
		{
			hostAuthoritativeState = value18.As<bool>();
		}
	}
}
