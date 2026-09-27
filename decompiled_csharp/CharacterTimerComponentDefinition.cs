using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/CharacterTimerComponent/CharacterTimerComponentDefinition.cs")]
public class CharacterTimerComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName timerDictionary = "timerDictionary";

		public static readonly StringName timeScale = "timeScale";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Dictionary timerDictionary { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public double timeScale { get; set; } = 1.0;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new CharacterTimerComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.timerDictionary)
		{
			timerDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.timerDictionary)
		{
			value = VariantUtils.CreateFrom<Dictionary>(timerDictionary);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			value = VariantUtils.CreateFrom<double>(timeScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.timerDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.timerDictionary, Variant.From<Dictionary>(timerDictionary));
		info.AddProperty(PropertyName.timeScale, Variant.From<double>(timeScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.timerDictionary, out var value))
		{
			timerDictionary = value.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.timeScale, out var value2))
		{
			timeScale = value2.As<double>();
		}
	}
}
