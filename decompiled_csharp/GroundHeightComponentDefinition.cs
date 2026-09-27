using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/GroundHeightComponent/GroundHeightComponentDefinition.cs")]
public class GroundHeightComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName interpolationSpeed = "interpolationSpeed";

		public static readonly StringName threshold = "threshold";

		public static readonly StringName waterHeight = "waterHeight";

		public static readonly StringName ladderHeight = "ladderHeight";

		public static readonly StringName handleWaterHeight = "handleWaterHeight";

		public static readonly StringName handleLadder = "handleLadder";

		public static readonly StringName detectWater = "detectWater";

		public static readonly StringName detectLadder = "detectLadder";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public float interpolationSpeed = 3f;

	[Export(PropertyHint.None, "")]
	public float threshold = 0.1f;

	[Export(PropertyHint.None, "")]
	public float waterHeight = 25f;

	[Export(PropertyHint.None, "")]
	public float ladderHeight = 60f;

	[Export(PropertyHint.None, "")]
	public bool handleWaterHeight;

	[Export(PropertyHint.None, "")]
	public bool handleLadder;

	[Export(PropertyHint.None, "")]
	public bool detectWater;

	[Export(PropertyHint.None, "")]
	public bool detectLadder;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new GroundHeightComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.interpolationSpeed)
		{
			interpolationSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.threshold)
		{
			threshold = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.waterHeight)
		{
			waterHeight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.ladderHeight)
		{
			ladderHeight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.handleWaterHeight)
		{
			handleWaterHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.handleLadder)
		{
			handleLadder = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.detectWater)
		{
			detectWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.detectLadder)
		{
			detectLadder = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.interpolationSpeed)
		{
			value = VariantUtils.CreateFrom(in interpolationSpeed);
			return true;
		}
		if (name == PropertyName.threshold)
		{
			value = VariantUtils.CreateFrom(in threshold);
			return true;
		}
		if (name == PropertyName.waterHeight)
		{
			value = VariantUtils.CreateFrom(in waterHeight);
			return true;
		}
		if (name == PropertyName.ladderHeight)
		{
			value = VariantUtils.CreateFrom(in ladderHeight);
			return true;
		}
		if (name == PropertyName.handleWaterHeight)
		{
			value = VariantUtils.CreateFrom(in handleWaterHeight);
			return true;
		}
		if (name == PropertyName.handleLadder)
		{
			value = VariantUtils.CreateFrom(in handleLadder);
			return true;
		}
		if (name == PropertyName.detectWater)
		{
			value = VariantUtils.CreateFrom(in detectWater);
			return true;
		}
		if (name == PropertyName.detectLadder)
		{
			value = VariantUtils.CreateFrom(in detectLadder);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.interpolationSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.threshold, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waterHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ladderHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.handleWaterHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.handleLadder, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.detectWater, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.detectLadder, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.interpolationSpeed, Variant.From(in interpolationSpeed));
		info.AddProperty(PropertyName.threshold, Variant.From(in threshold));
		info.AddProperty(PropertyName.waterHeight, Variant.From(in waterHeight));
		info.AddProperty(PropertyName.ladderHeight, Variant.From(in ladderHeight));
		info.AddProperty(PropertyName.handleWaterHeight, Variant.From(in handleWaterHeight));
		info.AddProperty(PropertyName.handleLadder, Variant.From(in handleLadder));
		info.AddProperty(PropertyName.detectWater, Variant.From(in detectWater));
		info.AddProperty(PropertyName.detectLadder, Variant.From(in detectLadder));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.interpolationSpeed, out var value))
		{
			interpolationSpeed = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.threshold, out var value2))
		{
			threshold = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.waterHeight, out var value3))
		{
			waterHeight = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.ladderHeight, out var value4))
		{
			ladderHeight = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.handleWaterHeight, out var value5))
		{
			handleWaterHeight = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.handleLadder, out var value6))
		{
			handleLadder = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.detectWater, out var value7))
		{
			detectWater = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.detectLadder, out var value8))
		{
			detectLadder = value8.As<bool>();
		}
	}
}
