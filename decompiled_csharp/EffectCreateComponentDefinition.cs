using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/EffectCreateComponent/EffectCreateComponentDefinition.cs")]
public class EffectCreateComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName maxEffectCount = "maxEffectCount";

		public static readonly StringName applyLimitToIceTrap = "applyLimitToIceTrap";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	public const int DefaultMaxEffectCount = 100;

	[Export(PropertyHint.Range, "-1,1000,1")]
	public int maxEffectCount = 100;

	[Export(PropertyHint.None, "")]
	public bool applyLimitToIceTrap;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new EffectCreateComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.maxEffectCount)
		{
			maxEffectCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.applyLimitToIceTrap)
		{
			applyLimitToIceTrap = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.maxEffectCount)
		{
			value = VariantUtils.CreateFrom(in maxEffectCount);
			return true;
		}
		if (name == PropertyName.applyLimitToIceTrap)
		{
			value = VariantUtils.CreateFrom(in applyLimitToIceTrap);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.maxEffectCount, PropertyHint.Range, "-1,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.applyLimitToIceTrap, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.maxEffectCount, Variant.From(in maxEffectCount));
		info.AddProperty(PropertyName.applyLimitToIceTrap, Variant.From(in applyLimitToIceTrap));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.maxEffectCount, out var value))
		{
			maxEffectCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.applyLimitToIceTrap, out var value2))
		{
			applyLimitToIceTrap = value2.As<bool>();
		}
	}
}
