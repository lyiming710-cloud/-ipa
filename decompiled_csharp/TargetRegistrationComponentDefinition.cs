using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/TargetRegistrationComponent/TargetRegistrationComponentDefinition.cs")]
public class TargetRegistrationComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName allLineCheck = "allLineCheck";

		public static readonly StringName canProjectileCheck = "canProjectileCheck";

		public static readonly StringName canCarry = "canCarry";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool allLineCheck;

	[Export(PropertyHint.None, "")]
	public bool canProjectileCheck = true;

	[Export(PropertyHint.None, "")]
	public bool canCarry = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new TargetRegistrationComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.allLineCheck)
		{
			allLineCheck = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canProjectileCheck)
		{
			canProjectileCheck = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canCarry)
		{
			canCarry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.allLineCheck)
		{
			value = VariantUtils.CreateFrom(in allLineCheck);
			return true;
		}
		if (name == PropertyName.canProjectileCheck)
		{
			value = VariantUtils.CreateFrom(in canProjectileCheck);
			return true;
		}
		if (name == PropertyName.canCarry)
		{
			value = VariantUtils.CreateFrom(in canCarry);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.allLineCheck, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canProjectileCheck, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canCarry, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.allLineCheck, Variant.From(in allLineCheck));
		info.AddProperty(PropertyName.canProjectileCheck, Variant.From(in canProjectileCheck));
		info.AddProperty(PropertyName.canCarry, Variant.From(in canCarry));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.allLineCheck, out var value))
		{
			allLineCheck = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canProjectileCheck, out var value2))
		{
			canProjectileCheck = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canCarry, out var value3))
		{
			canCarry = value3.As<bool>();
		}
	}
}
