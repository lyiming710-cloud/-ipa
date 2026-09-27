using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/DamagePartComponent/DamagePartComponentDefinition.cs")]
public class DamagePartComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName randomVelocityXRange = "randomVelocityXRange";

		public static readonly StringName defaultVelocityY = "defaultVelocityY";

		public static readonly StringName armorHeightOffset = "armorHeightOffset";

		public static readonly StringName normalHeightOffset = "normalHeightOffset";

		public static readonly StringName inheritCharacterScale = "inheritCharacterScale";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	public static readonly Vector2 DefaultRandomVelocityXRange = new Vector2(-100f, 100f);

	public const float DefaultVelocityY = -300f;

	public const float DefaultArmorHeightOffset = 24f;

	public const float DefaultNormalHeightOffset = 0f;

	public const bool DefaultInheritCharacterScale = true;

	[Export(PropertyHint.None, "")]
	public Vector2 randomVelocityXRange = DefaultRandomVelocityXRange;

	[Export(PropertyHint.None, "")]
	public float defaultVelocityY = -300f;

	[Export(PropertyHint.None, "")]
	public float armorHeightOffset = 24f;

	[Export(PropertyHint.None, "")]
	public float normalHeightOffset;

	[Export(PropertyHint.None, "")]
	public bool inheritCharacterScale = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new DamagePartComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.randomVelocityXRange)
		{
			randomVelocityXRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.defaultVelocityY)
		{
			defaultVelocityY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.armorHeightOffset)
		{
			armorHeightOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.normalHeightOffset)
		{
			normalHeightOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.inheritCharacterScale)
		{
			inheritCharacterScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.randomVelocityXRange)
		{
			value = VariantUtils.CreateFrom(in randomVelocityXRange);
			return true;
		}
		if (name == PropertyName.defaultVelocityY)
		{
			value = VariantUtils.CreateFrom(in defaultVelocityY);
			return true;
		}
		if (name == PropertyName.armorHeightOffset)
		{
			value = VariantUtils.CreateFrom(in armorHeightOffset);
			return true;
		}
		if (name == PropertyName.normalHeightOffset)
		{
			value = VariantUtils.CreateFrom(in normalHeightOffset);
			return true;
		}
		if (name == PropertyName.inheritCharacterScale)
		{
			value = VariantUtils.CreateFrom(in inheritCharacterScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName.randomVelocityXRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultVelocityY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.armorHeightOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.normalHeightOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.inheritCharacterScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.randomVelocityXRange, Variant.From(in randomVelocityXRange));
		info.AddProperty(PropertyName.defaultVelocityY, Variant.From(in defaultVelocityY));
		info.AddProperty(PropertyName.armorHeightOffset, Variant.From(in armorHeightOffset));
		info.AddProperty(PropertyName.normalHeightOffset, Variant.From(in normalHeightOffset));
		info.AddProperty(PropertyName.inheritCharacterScale, Variant.From(in inheritCharacterScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.randomVelocityXRange, out var value))
		{
			randomVelocityXRange = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.defaultVelocityY, out var value2))
		{
			defaultVelocityY = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.armorHeightOffset, out var value3))
		{
			armorHeightOffset = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.normalHeightOffset, out var value4))
		{
			normalHeightOffset = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.inheritCharacterScale, out var value5))
		{
			inheritCharacterScale = value5.As<bool>();
		}
	}
}
