using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/SwimComponent/SwimComponentDefinition.cs")]
public class SwimComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName animationBlend = "animationBlend";

		public static readonly StringName waterLoopBlend = "waterLoopBlend";

		public static readonly StringName outWaterWalkBlend = "outWaterWalkBlend";

		public static readonly StringName offscreenSpeedMultiplier = "offscreenSpeedMultiplier";

		public static readonly StringName outWaterHorizontalOffset = "outWaterHorizontalOffset";

		public static readonly StringName underwaterEntryAudio = "underwaterEntryAudio";

		public static readonly StringName surfaceEntryAudio = "surfaceEntryAudio";

		public static readonly StringName createSplashOnEntryAnimation = "createSplashOnEntryAnimation";

		public static readonly StringName updateGroundHeightOnEntry = "updateGroundHeightOnEntry";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double animationBlend { get; set; } = 0.2;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double waterLoopBlend { get; set; }

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double outWaterWalkBlend { get; set; }

	[Export(PropertyHint.Range, "0,10,0.1")]
	public float offscreenSpeedMultiplier { get; set; } = 2f;

	[Export(PropertyHint.Range, "-100,100,0.5")]
	public float outWaterHorizontalOffset { get; set; } = 5f;

	[Export(PropertyHint.None, "")]
	public string underwaterEntryAudio { get; set; } = "ZombieEnteringWater";

	[Export(PropertyHint.None, "")]
	public string surfaceEntryAudio { get; set; } = "PlantWater";

	[Export(PropertyHint.None, "")]
	public bool createSplashOnEntryAnimation { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool updateGroundHeightOnEntry { get; set; } = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new SwimComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.animationBlend)
		{
			animationBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.waterLoopBlend)
		{
			waterLoopBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.outWaterWalkBlend)
		{
			outWaterWalkBlend = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.offscreenSpeedMultiplier)
		{
			offscreenSpeedMultiplier = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.outWaterHorizontalOffset)
		{
			outWaterHorizontalOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.underwaterEntryAudio)
		{
			underwaterEntryAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.surfaceEntryAudio)
		{
			surfaceEntryAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.createSplashOnEntryAnimation)
		{
			createSplashOnEntryAnimation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.updateGroundHeightOnEntry)
		{
			updateGroundHeightOnEntry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		double from;
		if (name == PropertyName.animationBlend)
		{
			from = animationBlend;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.waterLoopBlend)
		{
			from = waterLoopBlend;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.outWaterWalkBlend)
		{
			from = outWaterWalkBlend;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from2;
		if (name == PropertyName.offscreenSpeedMultiplier)
		{
			from2 = offscreenSpeedMultiplier;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.outWaterHorizontalOffset)
		{
			from2 = outWaterHorizontalOffset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		string from3;
		if (name == PropertyName.underwaterEntryAudio)
		{
			from3 = underwaterEntryAudio;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.surfaceEntryAudio)
		{
			from3 = surfaceEntryAudio;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		bool from4;
		if (name == PropertyName.createSplashOnEntryAnimation)
		{
			from4 = createSplashOnEntryAnimation;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.updateGroundHeightOnEntry)
		{
			from4 = updateGroundHeightOnEntry;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.animationBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waterLoopBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.outWaterWalkBlend, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.offscreenSpeedMultiplier, PropertyHint.Range, "0,10,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.outWaterHorizontalOffset, PropertyHint.Range, "-100,100,0.5", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.underwaterEntryAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.surfaceEntryAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createSplashOnEntryAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.updateGroundHeightOnEntry, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.animationBlend, Variant.From<double>(animationBlend));
		info.AddProperty(PropertyName.waterLoopBlend, Variant.From<double>(waterLoopBlend));
		info.AddProperty(PropertyName.outWaterWalkBlend, Variant.From<double>(outWaterWalkBlend));
		info.AddProperty(PropertyName.offscreenSpeedMultiplier, Variant.From<float>(offscreenSpeedMultiplier));
		info.AddProperty(PropertyName.outWaterHorizontalOffset, Variant.From<float>(outWaterHorizontalOffset));
		info.AddProperty(PropertyName.underwaterEntryAudio, Variant.From<string>(underwaterEntryAudio));
		info.AddProperty(PropertyName.surfaceEntryAudio, Variant.From<string>(surfaceEntryAudio));
		info.AddProperty(PropertyName.createSplashOnEntryAnimation, Variant.From<bool>(createSplashOnEntryAnimation));
		info.AddProperty(PropertyName.updateGroundHeightOnEntry, Variant.From<bool>(updateGroundHeightOnEntry));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.animationBlend, out var value))
		{
			animationBlend = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.waterLoopBlend, out var value2))
		{
			waterLoopBlend = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.outWaterWalkBlend, out var value3))
		{
			outWaterWalkBlend = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.offscreenSpeedMultiplier, out var value4))
		{
			offscreenSpeedMultiplier = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.outWaterHorizontalOffset, out var value5))
		{
			outWaterHorizontalOffset = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.underwaterEntryAudio, out var value6))
		{
			underwaterEntryAudio = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.surfaceEntryAudio, out var value7))
		{
			surfaceEntryAudio = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.createSplashOnEntryAnimation, out var value8))
		{
			createSplashOnEntryAnimation = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.updateGroundHeightOnEntry, out var value9))
		{
			updateGroundHeightOnEntry = value9.As<bool>();
		}
	}
}
