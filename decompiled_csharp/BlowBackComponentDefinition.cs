using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BlowBackComponent/BlowBackComponentDefinition.cs")]
public class BlowBackComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName defaultDuration = "defaultDuration";

		public static readonly StringName allowStacking = "allowStacking";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,30,0.05")]
	public double defaultDuration = 1.0;

	[Export(PropertyHint.None, "")]
	public bool allowStacking = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new BlowBackComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.defaultDuration)
		{
			defaultDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.allowStacking)
		{
			allowStacking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.defaultDuration)
		{
			value = VariantUtils.CreateFrom(in defaultDuration);
			return true;
		}
		if (name == PropertyName.allowStacking)
		{
			value = VariantUtils.CreateFrom(in allowStacking);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultDuration, PropertyHint.Range, "0,30,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.allowStacking, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.defaultDuration, Variant.From(in defaultDuration));
		info.AddProperty(PropertyName.allowStacking, Variant.From(in allowStacking));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.defaultDuration, out var value))
		{
			defaultDuration = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.allowStacking, out var value2))
		{
			allowStacking = value2.As<bool>();
		}
	}
}
