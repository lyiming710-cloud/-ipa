using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefensePlantConfig.cs")]
public class TowerDefensePlantConfig : TowerDefenseCharacterConfig
{
	public new class MethodName : TowerDefenseCharacterConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCharacterConfig.PropertyName
	{
		public static readonly StringName canUsePlantfood = "canUsePlantfood";

		public static readonly StringName extendGrid = "extendGrid";

		public static readonly StringName extendCoverDictionary = "extendCoverDictionary";

		public static readonly StringName izm2Fliter = "izm2Fliter";

		public static readonly StringName coverEvents = "coverEvents";
	}

	public new class SignalName : TowerDefenseCharacterConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool canUsePlantfood = true;

	[Export(PropertyHint.None, "")]
	public Array<Vector2I> extendGrid = new Array<Vector2I>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<Vector2I, string> extendCoverDictionary = new Godot.Collections.Dictionary<Vector2I, string>();

	[Export(PropertyHint.None, "")]
	public bool izm2Fliter;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> coverEvents = new Array<TowerDefenseCharacterEventBase>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.canUsePlantfood)
		{
			canUsePlantfood = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.extendGrid)
		{
			extendGrid = VariantUtils.ConvertToArray<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.extendCoverDictionary)
		{
			extendCoverDictionary = VariantUtils.ConvertToDictionary<Vector2I, string>(in value);
			return true;
		}
		if (name == PropertyName.izm2Fliter)
		{
			izm2Fliter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.coverEvents)
		{
			coverEvents = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.canUsePlantfood)
		{
			value = VariantUtils.CreateFrom(in canUsePlantfood);
			return true;
		}
		if (name == PropertyName.extendGrid)
		{
			value = VariantUtils.CreateFromArray(extendGrid);
			return true;
		}
		if (name == PropertyName.extendCoverDictionary)
		{
			value = VariantUtils.CreateFromDictionary(extendCoverDictionary);
			return true;
		}
		if (name == PropertyName.izm2Fliter)
		{
			value = VariantUtils.CreateFrom(in izm2Fliter);
			return true;
		}
		if (name == PropertyName.coverEvents)
		{
			value = VariantUtils.CreateFromArray(coverEvents);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.canUsePlantfood, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.extendGrid, PropertyHint.TypeString, "6/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.extendCoverDictionary, PropertyHint.TypeString, "6/0:;4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.izm2Fliter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.coverEvents, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.canUsePlantfood, Variant.From(in canUsePlantfood));
		info.AddProperty(PropertyName.extendGrid, Variant.CreateFrom(extendGrid));
		info.AddProperty(PropertyName.extendCoverDictionary, Variant.CreateFrom(extendCoverDictionary));
		info.AddProperty(PropertyName.izm2Fliter, Variant.From(in izm2Fliter));
		info.AddProperty(PropertyName.coverEvents, Variant.CreateFrom(coverEvents));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.canUsePlantfood, out var value))
		{
			canUsePlantfood = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.extendGrid, out var value2))
		{
			extendGrid = value2.AsGodotArray<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.extendCoverDictionary, out var value3))
		{
			extendCoverDictionary = value3.AsGodotDictionary<Vector2I, string>();
		}
		if (info.TryGetProperty(PropertyName.izm2Fliter, out var value4))
		{
			izm2Fliter = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.coverEvents, out var value5))
		{
			coverEvents = value5.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
