using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/GrowUpComponent/GrowUpComponentDefinition.cs")]
public class GrowUpComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName growUpTime = "growUpTime";

		public static readonly StringName growUpSize = "growUpSize";

		public static readonly StringName growAudio = "growAudio";

		public static readonly StringName growTweenDuration = "growTweenDuration";

		public static readonly StringName instantGrowInIZM = "instantGrowInIZM";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<float> growUpTime { get; set; } = new Array<float>();

	[Export(PropertyHint.None, "")]
	public Array<float> growUpSize { get; set; } = new Array<float>();

	[Export(PropertyHint.None, "")]
	public string growAudio { get; set; } = "Grow";

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double growTweenDuration { get; set; } = 1.0;

	[Export(PropertyHint.None, "")]
	public bool instantGrowInIZM { get; set; } = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new GrowUpComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.growUpTime)
		{
			growUpTime = VariantUtils.ConvertToArray<float>(in value);
			return true;
		}
		if (name == PropertyName.growUpSize)
		{
			growUpSize = VariantUtils.ConvertToArray<float>(in value);
			return true;
		}
		if (name == PropertyName.growAudio)
		{
			growAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.growTweenDuration)
		{
			growTweenDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.instantGrowInIZM)
		{
			instantGrowInIZM = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.growUpTime)
		{
			value = VariantUtils.CreateFromArray(growUpTime);
			return true;
		}
		if (name == PropertyName.growUpSize)
		{
			value = VariantUtils.CreateFromArray(growUpSize);
			return true;
		}
		if (name == PropertyName.growAudio)
		{
			value = VariantUtils.CreateFrom<string>(growAudio);
			return true;
		}
		if (name == PropertyName.growTweenDuration)
		{
			value = VariantUtils.CreateFrom<double>(growTweenDuration);
			return true;
		}
		if (name == PropertyName.instantGrowInIZM)
		{
			value = VariantUtils.CreateFrom<bool>(instantGrowInIZM);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.growUpTime, PropertyHint.TypeString, "3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.growUpSize, PropertyHint.TypeString, "3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.growAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.growTweenDuration, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.instantGrowInIZM, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.growUpTime, Variant.CreateFrom(growUpTime));
		info.AddProperty(PropertyName.growUpSize, Variant.CreateFrom(growUpSize));
		info.AddProperty(PropertyName.growAudio, Variant.From<string>(growAudio));
		info.AddProperty(PropertyName.growTweenDuration, Variant.From<double>(growTweenDuration));
		info.AddProperty(PropertyName.instantGrowInIZM, Variant.From<bool>(instantGrowInIZM));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.growUpTime, out var value))
		{
			growUpTime = value.AsGodotArray<float>();
		}
		if (info.TryGetProperty(PropertyName.growUpSize, out var value2))
		{
			growUpSize = value2.AsGodotArray<float>();
		}
		if (info.TryGetProperty(PropertyName.growAudio, out var value3))
		{
			growAudio = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.growTweenDuration, out var value4))
		{
			growTweenDuration = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.instantGrowInIZM, out var value5))
		{
			instantGrowInIZM = value5.As<bool>();
		}
	}
}
