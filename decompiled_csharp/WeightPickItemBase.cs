using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Core/WeightPick/WeightPickItemBase.cs")]
public class WeightPickItemBase : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName empty = "empty";

		public static readonly StringName item = "item";

		public static readonly StringName weight = "weight";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public bool empty;

	public Variant item;

	public double weight = 100.0;

	public WeightPickItemBase()
	{
	}

	public WeightPickItemBase(Variant _item, double _weight = 100.0, bool _empty = false)
	{
		item = _item;
		weight = _weight;
		empty = _empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.empty)
		{
			empty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.item)
		{
			item = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.weight)
		{
			weight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.empty)
		{
			value = VariantUtils.CreateFrom(in empty);
			return true;
		}
		if (name == PropertyName.item)
		{
			value = VariantUtils.CreateFrom(in item);
			return true;
		}
		if (name == PropertyName.weight)
		{
			value = VariantUtils.CreateFrom(in weight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.empty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.item, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.weight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.empty, Variant.From(in empty));
		info.AddProperty(PropertyName.item, Variant.From(in item));
		info.AddProperty(PropertyName.weight, Variant.From(in weight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.empty, out var value))
		{
			empty = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.item, out var value2))
		{
			item = value2.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.weight, out var value3))
		{
			weight = value3.As<double>();
		}
	}
}
