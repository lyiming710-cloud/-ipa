using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Battle/Resource/TowerDefenseBattleDependenceData.cs")]
public class TowerDefenseBattleDependenceData : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName RequiredFeatureNames = "RequiredFeatureNames";

		public static readonly StringName FeatureNames = "FeatureNames";

		public static readonly StringName ConfiguredOptionalFeatureNames = "ConfiguredOptionalFeatureNames";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public Array<StringName> FeatureNames = new Array<StringName>();

	public Array<StringName> ConfiguredOptionalFeatureNames = new Array<StringName>();

	public Array<StringName> RequiredFeatureNames => FeatureNames ?? (FeatureNames = new Array<StringName>());

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FeatureNames)
		{
			FeatureNames = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.ConfiguredOptionalFeatureNames)
		{
			ConfiguredOptionalFeatureNames = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.RequiredFeatureNames)
		{
			value = VariantUtils.CreateFromArray(RequiredFeatureNames);
			return true;
		}
		if (name == PropertyName.FeatureNames)
		{
			value = VariantUtils.CreateFromArray(FeatureNames);
			return true;
		}
		if (name == PropertyName.ConfiguredOptionalFeatureNames)
		{
			value = VariantUtils.CreateFromArray(ConfiguredOptionalFeatureNames);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.FeatureNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.ConfiguredOptionalFeatureNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.RequiredFeatureNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FeatureNames, Variant.CreateFrom(FeatureNames));
		info.AddProperty(PropertyName.ConfiguredOptionalFeatureNames, Variant.CreateFrom(ConfiguredOptionalFeatureNames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FeatureNames, out var value))
		{
			FeatureNames = value.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.ConfiguredOptionalFeatureNames, out var value2))
		{
			ConfiguredOptionalFeatureNames = value2.AsGodotArray<StringName>();
		}
	}
}
