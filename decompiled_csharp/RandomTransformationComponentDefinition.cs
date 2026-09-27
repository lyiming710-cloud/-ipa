using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/RandomTransformationComponent/RandomTransformationComponentDefinition.cs")]
public class RandomTransformationComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName includePacketBankList = "includePacketBankList";

		public static readonly StringName excludePacketBankList = "excludePacketBankList";

		public static readonly StringName includePacketNameList = "includePacketNameList";

		public static readonly StringName excludePacketNameList = "excludePacketNameList";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<RandomTransformationComponentPacketBankConfig> includePacketBankList { get; set; } = new Array<RandomTransformationComponentPacketBankConfig>();

	[Export(PropertyHint.None, "")]
	public Array<RandomTransformationComponentPacketBankConfig> excludePacketBankList { get; set; } = new Array<RandomTransformationComponentPacketBankConfig>();

	[Export(PropertyHint.None, "")]
	public Array<string> includePacketNameList { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> excludePacketNameList { get; set; } = new Array<string>();

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new RandomTransformationComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.includePacketBankList)
		{
			includePacketBankList = VariantUtils.ConvertToArray<RandomTransformationComponentPacketBankConfig>(in value);
			return true;
		}
		if (name == PropertyName.excludePacketBankList)
		{
			excludePacketBankList = VariantUtils.ConvertToArray<RandomTransformationComponentPacketBankConfig>(in value);
			return true;
		}
		if (name == PropertyName.includePacketNameList)
		{
			includePacketNameList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.excludePacketNameList)
		{
			excludePacketNameList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.includePacketBankList)
		{
			value = VariantUtils.CreateFromArray(includePacketBankList);
			return true;
		}
		if (name == PropertyName.excludePacketBankList)
		{
			value = VariantUtils.CreateFromArray(excludePacketBankList);
			return true;
		}
		if (name == PropertyName.includePacketNameList)
		{
			value = VariantUtils.CreateFromArray(includePacketNameList);
			return true;
		}
		if (name == PropertyName.excludePacketNameList)
		{
			value = VariantUtils.CreateFromArray(excludePacketNameList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.includePacketBankList, PropertyHint.TypeString, "24/17:RandomTransformationComponentPacketBankConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.excludePacketBankList, PropertyHint.TypeString, "24/17:RandomTransformationComponentPacketBankConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.includePacketNameList, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.excludePacketNameList, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.includePacketBankList, Variant.CreateFrom(includePacketBankList));
		info.AddProperty(PropertyName.excludePacketBankList, Variant.CreateFrom(excludePacketBankList));
		info.AddProperty(PropertyName.includePacketNameList, Variant.CreateFrom(includePacketNameList));
		info.AddProperty(PropertyName.excludePacketNameList, Variant.CreateFrom(excludePacketNameList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.includePacketBankList, out var value))
		{
			includePacketBankList = value.AsGodotArray<RandomTransformationComponentPacketBankConfig>();
		}
		if (info.TryGetProperty(PropertyName.excludePacketBankList, out var value2))
		{
			excludePacketBankList = value2.AsGodotArray<RandomTransformationComponentPacketBankConfig>();
		}
		if (info.TryGetProperty(PropertyName.includePacketNameList, out var value3))
		{
			includePacketNameList = value3.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.excludePacketNameList, out var value4))
		{
			excludePacketNameList = value4.AsGodotArray<string>();
		}
	}
}
