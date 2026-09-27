using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/FallingObjects/FallingObjectConfig.cs")]
public class FallingObjectConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Pick = "Pick";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName weightItem = "weightItem";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<FallingObjectWeightItemConfig> weightItem = new Array<FallingObjectWeightItemConfig>();

	public ObjectManagerConfig.OBJECT Pick()
	{
		Array<WeightPickItemBase> array = new Array<WeightPickItemBase>();
		foreach (FallingObjectWeightItemConfig item in weightItem)
		{
			array.Add(new WeightPickItemBase((int)item.item, item.weight, item.empty));
		}
		return (ObjectManagerConfig.OBJECT)WeightPickMathine.Pick(array).item.AsInt32();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Pick, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Pick && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(Pick());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Pick)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.weightItem)
		{
			weightItem = VariantUtils.ConvertToArray<FallingObjectWeightItemConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.weightItem)
		{
			value = VariantUtils.CreateFromArray(weightItem);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.weightItem, PropertyHint.TypeString, "24/17:FallingObjectWeightItemConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.weightItem, Variant.CreateFrom(weightItem));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.weightItem, out var value))
		{
			weightItem = value.AsGodotArray<FallingObjectWeightItemConfig>();
		}
	}
}
