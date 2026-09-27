using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/DismemberComponent/DismemberComponentDefinition.cs")]
public class DismemberComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName velocityXRange = "velocityXRange";

		public static readonly StringName velocityYRange = "velocityYRange";

		public static readonly StringName heightOffset = "heightOffset";

		public static readonly StringName fadeDuration = "fadeDuration";

		public static readonly StringName minimumPartSize = "minimumPartSize";

		public static readonly StringName maxParts = "maxParts";

		public static readonly StringName partColor = "partColor";

		public static readonly StringName includeHiddenLayers = "includeHiddenLayers";

		public static readonly StringName hideOriginalSprite = "hideOriginalSprite";

		public static readonly StringName hideShadow = "hideShadow";

		public static readonly StringName useShadowComponentVisibility = "useShadowComponentVisibility";

		public static readonly StringName clearSpecialDeathFlagsOnDestroy = "clearSpecialDeathFlagsOnDestroy";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Parts", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 velocityXRange { get; set; } = new Vector2(-150f, 150f);

	[Export(PropertyHint.None, "")]
	public Vector2 velocityYRange { get; set; } = new Vector2(-450f, -200f);

	[Export(PropertyHint.None, "")]
	public float heightOffset { get; set; }

	[Export(PropertyHint.Range, "0,5,0.01")]
	public float fadeDuration { get; set; } = 0.5f;

	[Export(PropertyHint.Range, "0,256,0.5")]
	public float minimumPartSize { get; set; }

	[Export(PropertyHint.Range, "0,256,1")]
	public int maxParts { get; set; }

	[Export(PropertyHint.None, "")]
	public Color partColor { get; set; } = Colors.White;

	[Export(PropertyHint.None, "")]
	public bool includeHiddenLayers { get; set; }

	[ExportGroup("Original Visual", "")]
	[Export(PropertyHint.None, "")]
	public bool hideOriginalSprite { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool hideShadow { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool useShadowComponentVisibility { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool clearSpecialDeathFlagsOnDestroy { get; set; } = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new DismemberComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.velocityXRange)
		{
			velocityXRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.velocityYRange)
		{
			velocityYRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.heightOffset)
		{
			heightOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fadeDuration)
		{
			fadeDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.minimumPartSize)
		{
			minimumPartSize = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxParts)
		{
			maxParts = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.partColor)
		{
			partColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.includeHiddenLayers)
		{
			includeHiddenLayers = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hideOriginalSprite)
		{
			hideOriginalSprite = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hideShadow)
		{
			hideShadow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useShadowComponentVisibility)
		{
			useShadowComponentVisibility = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.clearSpecialDeathFlagsOnDestroy)
		{
			clearSpecialDeathFlagsOnDestroy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		Vector2 from;
		if (name == PropertyName.velocityXRange)
		{
			from = velocityXRange;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.velocityYRange)
		{
			from = velocityYRange;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from2;
		if (name == PropertyName.heightOffset)
		{
			from2 = heightOffset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fadeDuration)
		{
			from2 = fadeDuration;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.minimumPartSize)
		{
			from2 = minimumPartSize;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.maxParts)
		{
			value = VariantUtils.CreateFrom<int>(maxParts);
			return true;
		}
		if (name == PropertyName.partColor)
		{
			value = VariantUtils.CreateFrom<Color>(partColor);
			return true;
		}
		bool from3;
		if (name == PropertyName.includeHiddenLayers)
		{
			from3 = includeHiddenLayers;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.hideOriginalSprite)
		{
			from3 = hideOriginalSprite;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.hideShadow)
		{
			from3 = hideShadow;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.useShadowComponentVisibility)
		{
			from3 = useShadowComponentVisibility;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.clearSpecialDeathFlagsOnDestroy)
		{
			from3 = clearSpecialDeathFlagsOnDestroy;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Parts", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocityXRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocityYRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.heightOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fadeDuration, PropertyHint.Range, "0,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.minimumPartSize, PropertyHint.Range, "0,256,0.5", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxParts, PropertyHint.Range, "0,256,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.partColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.includeHiddenLayers, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Original Visual", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideOriginalSprite, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideShadow, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useShadowComponentVisibility, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.clearSpecialDeathFlagsOnDestroy, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.velocityXRange, Variant.From<Vector2>(velocityXRange));
		info.AddProperty(PropertyName.velocityYRange, Variant.From<Vector2>(velocityYRange));
		info.AddProperty(PropertyName.heightOffset, Variant.From<float>(heightOffset));
		info.AddProperty(PropertyName.fadeDuration, Variant.From<float>(fadeDuration));
		info.AddProperty(PropertyName.minimumPartSize, Variant.From<float>(minimumPartSize));
		info.AddProperty(PropertyName.maxParts, Variant.From<int>(maxParts));
		info.AddProperty(PropertyName.partColor, Variant.From<Color>(partColor));
		info.AddProperty(PropertyName.includeHiddenLayers, Variant.From<bool>(includeHiddenLayers));
		info.AddProperty(PropertyName.hideOriginalSprite, Variant.From<bool>(hideOriginalSprite));
		info.AddProperty(PropertyName.hideShadow, Variant.From<bool>(hideShadow));
		info.AddProperty(PropertyName.useShadowComponentVisibility, Variant.From<bool>(useShadowComponentVisibility));
		info.AddProperty(PropertyName.clearSpecialDeathFlagsOnDestroy, Variant.From<bool>(clearSpecialDeathFlagsOnDestroy));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.velocityXRange, out var value))
		{
			velocityXRange = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.velocityYRange, out var value2))
		{
			velocityYRange = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.heightOffset, out var value3))
		{
			heightOffset = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fadeDuration, out var value4))
		{
			fadeDuration = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.minimumPartSize, out var value5))
		{
			minimumPartSize = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxParts, out var value6))
		{
			maxParts = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.partColor, out var value7))
		{
			partColor = value7.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.includeHiddenLayers, out var value8))
		{
			includeHiddenLayers = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hideOriginalSprite, out var value9))
		{
			hideOriginalSprite = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hideShadow, out var value10))
		{
			hideShadow = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useShadowComponentVisibility, out var value11))
		{
			useShadowComponentVisibility = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.clearSpecialDeathFlagsOnDestroy, out var value12))
		{
			clearSpecialDeathFlagsOnDestroy = value12.As<bool>();
		}
	}
}
