using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/WaterInteractionComponent/WaterInteractionComponentDefinition.cs")]
public class WaterInteractionComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName inWaterLine = "inWaterLine";

		public static readonly StringName handleDuckytobe = "handleDuckytobe";

		public static readonly StringName discardOffsetIn = "discardOffsetIn";

		public static readonly StringName discardOffsetOut = "discardOffsetOut";

		public static readonly StringName discardOffsetOutTarget = "discardOffsetOutTarget";

		public static readonly StringName exitDuration = "exitDuration";

		public static readonly StringName discardResetPosition = "discardResetPosition";

		public static readonly StringName discardShaderParameter = "discardShaderParameter";

		public static readonly StringName createSplashOnEntry = "createSplashOnEntry";

		public static readonly StringName monitorScaleChanges = "monitorScaleChanges";

		public static readonly StringName scaleChangeEpsilon = "scaleChangeEpsilon";

		public static readonly StringName exitEase = "exitEase";

		public static readonly StringName exitTransition = "exitTransition";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool inWaterLine;

	[Export(PropertyHint.None, "")]
	public bool handleDuckytobe;

	[Export(PropertyHint.None, "")]
	public float discardOffsetIn = 36f;

	[Export(PropertyHint.None, "")]
	public float discardOffsetOut = 56f;

	[Export(PropertyHint.None, "")]
	public float discardOffsetOutTarget = 86f;

	[Export(PropertyHint.Range, "0,10,0.01")]
	public float exitDuration = 1f;

	[Export(PropertyHint.None, "")]
	public float discardResetPosition = 10000f;

	[Export(PropertyHint.None, "")]
	public StringName discardShaderParameter = "discardDownPos";

	[Export(PropertyHint.None, "")]
	public bool createSplashOnEntry = true;

	[Export(PropertyHint.None, "")]
	public bool monitorScaleChanges = true;

	[Export(PropertyHint.Range, "0.0001,0.1,0.0001")]
	public float scaleChangeEpsilon = 0.001f;

	[Export(PropertyHint.None, "")]
	public Tween.EaseType exitEase = Tween.EaseType.Out;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType exitTransition = Tween.TransitionType.Cubic;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new WaterInteractionComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.inWaterLine)
		{
			inWaterLine = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.handleDuckytobe)
		{
			handleDuckytobe = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.discardOffsetIn)
		{
			discardOffsetIn = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardOffsetOut)
		{
			discardOffsetOut = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardOffsetOutTarget)
		{
			discardOffsetOutTarget = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.exitDuration)
		{
			exitDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			discardResetPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			discardShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.createSplashOnEntry)
		{
			createSplashOnEntry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.monitorScaleChanges)
		{
			monitorScaleChanges = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.scaleChangeEpsilon)
		{
			scaleChangeEpsilon = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.exitEase)
		{
			exitEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.exitTransition)
		{
			exitTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.inWaterLine)
		{
			value = VariantUtils.CreateFrom(in inWaterLine);
			return true;
		}
		if (name == PropertyName.handleDuckytobe)
		{
			value = VariantUtils.CreateFrom(in handleDuckytobe);
			return true;
		}
		if (name == PropertyName.discardOffsetIn)
		{
			value = VariantUtils.CreateFrom(in discardOffsetIn);
			return true;
		}
		if (name == PropertyName.discardOffsetOut)
		{
			value = VariantUtils.CreateFrom(in discardOffsetOut);
			return true;
		}
		if (name == PropertyName.discardOffsetOutTarget)
		{
			value = VariantUtils.CreateFrom(in discardOffsetOutTarget);
			return true;
		}
		if (name == PropertyName.exitDuration)
		{
			value = VariantUtils.CreateFrom(in exitDuration);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			value = VariantUtils.CreateFrom(in discardResetPosition);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			value = VariantUtils.CreateFrom(in discardShaderParameter);
			return true;
		}
		if (name == PropertyName.createSplashOnEntry)
		{
			value = VariantUtils.CreateFrom(in createSplashOnEntry);
			return true;
		}
		if (name == PropertyName.monitorScaleChanges)
		{
			value = VariantUtils.CreateFrom(in monitorScaleChanges);
			return true;
		}
		if (name == PropertyName.scaleChangeEpsilon)
		{
			value = VariantUtils.CreateFrom(in scaleChangeEpsilon);
			return true;
		}
		if (name == PropertyName.exitEase)
		{
			value = VariantUtils.CreateFrom(in exitEase);
			return true;
		}
		if (name == PropertyName.exitTransition)
		{
			value = VariantUtils.CreateFrom(in exitTransition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.inWaterLine, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.handleDuckytobe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardOffsetIn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardOffsetOut, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardOffsetOutTarget, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.exitDuration, PropertyHint.Range, "0,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardResetPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.discardShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createSplashOnEntry, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.monitorScaleChanges, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaleChangeEpsilon, PropertyHint.Range, "0.0001,0.1,0.0001", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.exitEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.exitTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.inWaterLine, Variant.From(in inWaterLine));
		info.AddProperty(PropertyName.handleDuckytobe, Variant.From(in handleDuckytobe));
		info.AddProperty(PropertyName.discardOffsetIn, Variant.From(in discardOffsetIn));
		info.AddProperty(PropertyName.discardOffsetOut, Variant.From(in discardOffsetOut));
		info.AddProperty(PropertyName.discardOffsetOutTarget, Variant.From(in discardOffsetOutTarget));
		info.AddProperty(PropertyName.exitDuration, Variant.From(in exitDuration));
		info.AddProperty(PropertyName.discardResetPosition, Variant.From(in discardResetPosition));
		info.AddProperty(PropertyName.discardShaderParameter, Variant.From(in discardShaderParameter));
		info.AddProperty(PropertyName.createSplashOnEntry, Variant.From(in createSplashOnEntry));
		info.AddProperty(PropertyName.monitorScaleChanges, Variant.From(in monitorScaleChanges));
		info.AddProperty(PropertyName.scaleChangeEpsilon, Variant.From(in scaleChangeEpsilon));
		info.AddProperty(PropertyName.exitEase, Variant.From(in exitEase));
		info.AddProperty(PropertyName.exitTransition, Variant.From(in exitTransition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.inWaterLine, out var value))
		{
			inWaterLine = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.handleDuckytobe, out var value2))
		{
			handleDuckytobe = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.discardOffsetIn, out var value3))
		{
			discardOffsetIn = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardOffsetOut, out var value4))
		{
			discardOffsetOut = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardOffsetOutTarget, out var value5))
		{
			discardOffsetOutTarget = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.exitDuration, out var value6))
		{
			exitDuration = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardResetPosition, out var value7))
		{
			discardResetPosition = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardShaderParameter, out var value8))
		{
			discardShaderParameter = value8.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.createSplashOnEntry, out var value9))
		{
			createSplashOnEntry = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.monitorScaleChanges, out var value10))
		{
			monitorScaleChanges = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.scaleChangeEpsilon, out var value11))
		{
			scaleChangeEpsilon = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.exitEase, out var value12))
		{
			exitEase = value12.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.exitTransition, out var value13))
		{
			exitTransition = value13.As<Tween.TransitionType>();
		}
	}
}
