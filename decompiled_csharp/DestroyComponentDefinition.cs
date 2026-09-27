using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/DestroyComponent/DestroyComponentDefinition.cs")]
public class DestroyComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName destroyDelay = "destroyDelay";

		public static readonly StringName smashScaleY = "smashScaleY";

		public static readonly StringName ashShaderParameter = "ashShaderParameter";

		public static readonly StringName waitForPhysicsFrame = "waitForPhysicsFrame";

		public static readonly StringName pauseSpriteOnSpecialDestroy = "pauseSpriteOnSpecialDestroy";

		public static readonly StringName hideShadowOnSmash = "hideShadowOnSmash";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,10,0.01")]
	public float destroyDelay = 1f;

	[Export(PropertyHint.Range, "0.01,1,0.01")]
	public float smashScaleY = 0.25f;

	[Export(PropertyHint.None, "")]
	public StringName ashShaderParameter = "ash";

	[Export(PropertyHint.None, "")]
	public bool waitForPhysicsFrame = true;

	[Export(PropertyHint.None, "")]
	public bool pauseSpriteOnSpecialDestroy = true;

	[Export(PropertyHint.None, "")]
	public bool hideShadowOnSmash = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new DestroyComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.destroyDelay)
		{
			destroyDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.smashScaleY)
		{
			smashScaleY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.ashShaderParameter)
		{
			ashShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.waitForPhysicsFrame)
		{
			waitForPhysicsFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pauseSpriteOnSpecialDestroy)
		{
			pauseSpriteOnSpecialDestroy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hideShadowOnSmash)
		{
			hideShadowOnSmash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.destroyDelay)
		{
			value = VariantUtils.CreateFrom(in destroyDelay);
			return true;
		}
		if (name == PropertyName.smashScaleY)
		{
			value = VariantUtils.CreateFrom(in smashScaleY);
			return true;
		}
		if (name == PropertyName.ashShaderParameter)
		{
			value = VariantUtils.CreateFrom(in ashShaderParameter);
			return true;
		}
		if (name == PropertyName.waitForPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in waitForPhysicsFrame);
			return true;
		}
		if (name == PropertyName.pauseSpriteOnSpecialDestroy)
		{
			value = VariantUtils.CreateFrom(in pauseSpriteOnSpecialDestroy);
			return true;
		}
		if (name == PropertyName.hideShadowOnSmash)
		{
			value = VariantUtils.CreateFrom(in hideShadowOnSmash);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.destroyDelay, PropertyHint.Range, "0,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.smashScaleY, PropertyHint.Range, "0.01,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ashShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.waitForPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pauseSpriteOnSpecialDestroy, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideShadowOnSmash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.destroyDelay, Variant.From(in destroyDelay));
		info.AddProperty(PropertyName.smashScaleY, Variant.From(in smashScaleY));
		info.AddProperty(PropertyName.ashShaderParameter, Variant.From(in ashShaderParameter));
		info.AddProperty(PropertyName.waitForPhysicsFrame, Variant.From(in waitForPhysicsFrame));
		info.AddProperty(PropertyName.pauseSpriteOnSpecialDestroy, Variant.From(in pauseSpriteOnSpecialDestroy));
		info.AddProperty(PropertyName.hideShadowOnSmash, Variant.From(in hideShadowOnSmash));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.destroyDelay, out var value))
		{
			destroyDelay = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.smashScaleY, out var value2))
		{
			smashScaleY = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.ashShaderParameter, out var value3))
		{
			ashShaderParameter = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.waitForPhysicsFrame, out var value4))
		{
			waitForPhysicsFrame = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pauseSpriteOnSpecialDestroy, out var value5))
		{
			pauseSpriteOnSpecialDestroy = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hideShadowOnSmash, out var value6))
		{
			hideShadowOnSmash = value6.As<bool>();
		}
	}
}
