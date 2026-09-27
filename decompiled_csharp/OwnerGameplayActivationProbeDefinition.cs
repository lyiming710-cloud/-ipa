using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class OwnerGameplayActivationProbeDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName OrderId = "OrderId";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	public int OrderId { get; set; }

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new OwnerGameplayActivationProbeRuntime
		{
			OrderId = OrderId
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.OrderId)
		{
			OrderId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.OrderId)
		{
			value = VariantUtils.CreateFrom<int>(OrderId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.OrderId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.OrderId, Variant.From<int>(OrderId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.OrderId, out var value))
		{
			OrderId = value.As<int>();
		}
	}
}
