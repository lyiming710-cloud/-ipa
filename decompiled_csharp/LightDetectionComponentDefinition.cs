using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/LightDetectionComponent/LightDetectionComponentDefinition.cs")]
public class LightDetectionComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName detectionRadius = "detectionRadius";

		public static readonly StringName includeCenter = "includeCenter";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,8,1")]
	public int detectionRadius = 1;

	[Export(PropertyHint.None, "")]
	public bool includeCenter = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new LightDetectionComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.detectionRadius)
		{
			detectionRadius = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.includeCenter)
		{
			includeCenter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.detectionRadius)
		{
			value = VariantUtils.CreateFrom(in detectionRadius);
			return true;
		}
		if (name == PropertyName.includeCenter)
		{
			value = VariantUtils.CreateFrom(in includeCenter);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.detectionRadius, PropertyHint.Range, "0,8,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.includeCenter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.detectionRadius, Variant.From(in detectionRadius));
		info.AddProperty(PropertyName.includeCenter, Variant.From(in includeCenter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.detectionRadius, out var value))
		{
			detectionRadius = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.includeCenter, out var value2))
		{
			includeCenter = value2.As<bool>();
		}
	}
}
