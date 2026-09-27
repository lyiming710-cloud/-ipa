using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/EntryAnimationComponent/EntryAnimationComponentDefinition.cs")]
public class EntryAnimationComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName defaultFallHeight = "defaultFallHeight";

		public static readonly StringName defaultFallDelay = "defaultFallDelay";

		public static readonly StringName defaultFallDuration = "defaultFallDuration";

		public static readonly StringName preImpactScale = "preImpactScale";

		public static readonly StringName impactScale = "impactScale";

		public static readonly StringName settleScale = "settleScale";

		public static readonly StringName impactDuration = "impactDuration";

		public static readonly StringName settleDuration = "settleDuration";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Fall", "")]
	[Export(PropertyHint.None, "")]
	public float defaultFallHeight = 900f;

	[Export(PropertyHint.Range, "0,10,0.01,or_greater")]
	public float defaultFallDelay;

	[Export(PropertyHint.Range, "0.001,10,0.001,or_greater")]
	public float defaultFallDuration = 0.25f;

	[ExportGroup("Bounce", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 preImpactScale = new Vector2(0.75f, 1.25f);

	[Export(PropertyHint.None, "")]
	public Vector2 impactScale = new Vector2(1.5f, 0.5f);

	[Export(PropertyHint.None, "")]
	public Vector2 settleScale = Vector2.One;

	[Export(PropertyHint.Range, "0.001,2,0.001,or_greater")]
	public float impactDuration = 0.1f;

	[Export(PropertyHint.Range, "0.001,2,0.001,or_greater")]
	public float settleDuration = 0.2f;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new EntryAnimationComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.defaultFallHeight)
		{
			defaultFallHeight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.defaultFallDelay)
		{
			defaultFallDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.defaultFallDuration)
		{
			defaultFallDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.preImpactScale)
		{
			preImpactScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.impactScale)
		{
			impactScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.settleScale)
		{
			settleScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.impactDuration)
		{
			impactDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.settleDuration)
		{
			settleDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.defaultFallHeight)
		{
			value = VariantUtils.CreateFrom(in defaultFallHeight);
			return true;
		}
		if (name == PropertyName.defaultFallDelay)
		{
			value = VariantUtils.CreateFrom(in defaultFallDelay);
			return true;
		}
		if (name == PropertyName.defaultFallDuration)
		{
			value = VariantUtils.CreateFrom(in defaultFallDuration);
			return true;
		}
		if (name == PropertyName.preImpactScale)
		{
			value = VariantUtils.CreateFrom(in preImpactScale);
			return true;
		}
		if (name == PropertyName.impactScale)
		{
			value = VariantUtils.CreateFrom(in impactScale);
			return true;
		}
		if (name == PropertyName.settleScale)
		{
			value = VariantUtils.CreateFrom(in settleScale);
			return true;
		}
		if (name == PropertyName.impactDuration)
		{
			value = VariantUtils.CreateFrom(in impactDuration);
			return true;
		}
		if (name == PropertyName.settleDuration)
		{
			value = VariantUtils.CreateFrom(in settleDuration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Fall", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultFallHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultFallDelay, PropertyHint.Range, "0,10,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultFallDuration, PropertyHint.Range, "0.001,10,0.001,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Bounce", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.preImpactScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.impactScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.settleScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.impactDuration, PropertyHint.Range, "0.001,2,0.001,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.settleDuration, PropertyHint.Range, "0.001,2,0.001,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.defaultFallHeight, Variant.From(in defaultFallHeight));
		info.AddProperty(PropertyName.defaultFallDelay, Variant.From(in defaultFallDelay));
		info.AddProperty(PropertyName.defaultFallDuration, Variant.From(in defaultFallDuration));
		info.AddProperty(PropertyName.preImpactScale, Variant.From(in preImpactScale));
		info.AddProperty(PropertyName.impactScale, Variant.From(in impactScale));
		info.AddProperty(PropertyName.settleScale, Variant.From(in settleScale));
		info.AddProperty(PropertyName.impactDuration, Variant.From(in impactDuration));
		info.AddProperty(PropertyName.settleDuration, Variant.From(in settleDuration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.defaultFallHeight, out var value))
		{
			defaultFallHeight = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.defaultFallDelay, out var value2))
		{
			defaultFallDelay = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.defaultFallDuration, out var value3))
		{
			defaultFallDuration = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.preImpactScale, out var value4))
		{
			preImpactScale = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.impactScale, out var value5))
		{
			impactScale = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.settleScale, out var value6))
		{
			settleScale = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.impactDuration, out var value7))
		{
			impactDuration = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.settleDuration, out var value8))
		{
			settleDuration = value8.As<float>();
		}
	}
}
