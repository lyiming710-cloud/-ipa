using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/HitFlashComponent/HitFlashComponentDefinition.cs")]
public class HitFlashComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName brightShaderParameter = "brightShaderParameter";

		public static readonly StringName whiteShaderParameter = "whiteShaderParameter";

		public static readonly StringName restartBrightOnHit = "restartBrightOnHit";

		public static readonly StringName restartWhiteOnHit = "restartWhiteOnHit";

		public static readonly StringName minimumTweenDuration = "minimumTweenDuration";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName brightShaderParameter = "brightStrength";

	[Export(PropertyHint.None, "")]
	public StringName whiteShaderParameter = "whiteStrength";

	[Export(PropertyHint.None, "")]
	public bool restartBrightOnHit;

	[Export(PropertyHint.None, "")]
	public bool restartWhiteOnHit = true;

	[Export(PropertyHint.None, "")]
	public float minimumTweenDuration = 0.001f;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new HitFlashComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.brightShaderParameter)
		{
			brightShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.whiteShaderParameter)
		{
			whiteShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.restartBrightOnHit)
		{
			restartBrightOnHit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.restartWhiteOnHit)
		{
			restartWhiteOnHit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.minimumTweenDuration)
		{
			minimumTweenDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.brightShaderParameter)
		{
			value = VariantUtils.CreateFrom(in brightShaderParameter);
			return true;
		}
		if (name == PropertyName.whiteShaderParameter)
		{
			value = VariantUtils.CreateFrom(in whiteShaderParameter);
			return true;
		}
		if (name == PropertyName.restartBrightOnHit)
		{
			value = VariantUtils.CreateFrom(in restartBrightOnHit);
			return true;
		}
		if (name == PropertyName.restartWhiteOnHit)
		{
			value = VariantUtils.CreateFrom(in restartWhiteOnHit);
			return true;
		}
		if (name == PropertyName.minimumTweenDuration)
		{
			value = VariantUtils.CreateFrom(in minimumTweenDuration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.brightShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.whiteShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.restartBrightOnHit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.restartWhiteOnHit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.minimumTweenDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.brightShaderParameter, Variant.From(in brightShaderParameter));
		info.AddProperty(PropertyName.whiteShaderParameter, Variant.From(in whiteShaderParameter));
		info.AddProperty(PropertyName.restartBrightOnHit, Variant.From(in restartBrightOnHit));
		info.AddProperty(PropertyName.restartWhiteOnHit, Variant.From(in restartWhiteOnHit));
		info.AddProperty(PropertyName.minimumTweenDuration, Variant.From(in minimumTweenDuration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.brightShaderParameter, out var value))
		{
			brightShaderParameter = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.whiteShaderParameter, out var value2))
		{
			whiteShaderParameter = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.restartBrightOnHit, out var value3))
		{
			restartBrightOnHit = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.restartWhiteOnHit, out var value4))
		{
			restartWhiteOnHit = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.minimumTweenDuration, out var value5))
		{
			minimumTweenDuration = value5.As<float>();
		}
	}
}
