using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Test/ModEditorStateMachineProbeDerivedTransition.cs")]
public class ModEditorStateMachineProbeDerivedTransition : StateMachineTransitionDefinition
{
	public new class MethodName : StateMachineTransitionDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineTransitionDefinition.PropertyName
	{
		public static readonly StringName ModTransitionNote = "ModTransitionNote";

		public static readonly StringName RuntimeScale = "RuntimeScale";

		public static readonly StringName WireColor = "WireColor";

		public static readonly StringName RuntimeImpulse = "RuntimeImpulse";

		public static readonly StringName RuntimeCosts = "RuntimeCosts";
	}

	public new class SignalName : StateMachineTransitionDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ModTransitionNote { get; set; } = "transition-base";

	[Export(PropertyHint.Range, "0,8,0.05")]
	public double RuntimeScale { get; set; } = 1.25;

	[Export(PropertyHint.None, "")]
	public Color WireColor { get; set; } = new Color("ffb84d");

	[Export(PropertyHint.None, "")]
	public Vector3 RuntimeImpulse { get; set; } = new Vector3(1f, 2f, 3f);

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, int> RuntimeCosts { get; set; } = new Godot.Collections.Dictionary<string, int> { { "energy", 2 } };

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ModTransitionNote)
		{
			ModTransitionNote = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeScale)
		{
			RuntimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.WireColor)
		{
			WireColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeImpulse)
		{
			RuntimeImpulse = VariantUtils.ConvertTo<Vector3>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeCosts)
		{
			RuntimeCosts = VariantUtils.ConvertToDictionary<string, int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ModTransitionNote)
		{
			value = VariantUtils.CreateFrom<string>(ModTransitionNote);
			return true;
		}
		if (name == PropertyName.RuntimeScale)
		{
			value = VariantUtils.CreateFrom<double>(RuntimeScale);
			return true;
		}
		if (name == PropertyName.WireColor)
		{
			value = VariantUtils.CreateFrom<Color>(WireColor);
			return true;
		}
		if (name == PropertyName.RuntimeImpulse)
		{
			value = VariantUtils.CreateFrom<Vector3>(RuntimeImpulse);
			return true;
		}
		if (name == PropertyName.RuntimeCosts)
		{
			value = VariantUtils.CreateFromDictionary(RuntimeCosts);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ModTransitionNote, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.RuntimeScale, PropertyHint.Range, "0,8,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.WireColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector3, PropertyName.RuntimeImpulse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.RuntimeCosts, PropertyHint.TypeString, "4/0:;2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ModTransitionNote, Variant.From<string>(ModTransitionNote));
		info.AddProperty(PropertyName.RuntimeScale, Variant.From<double>(RuntimeScale));
		info.AddProperty(PropertyName.WireColor, Variant.From<Color>(WireColor));
		info.AddProperty(PropertyName.RuntimeImpulse, Variant.From<Vector3>(RuntimeImpulse));
		info.AddProperty(PropertyName.RuntimeCosts, Variant.CreateFrom(RuntimeCosts));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ModTransitionNote, out var value))
		{
			ModTransitionNote = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeScale, out var value2))
		{
			RuntimeScale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.WireColor, out var value3))
		{
			WireColor = value3.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeImpulse, out var value4))
		{
			RuntimeImpulse = value4.As<Vector3>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeCosts, out var value5))
		{
			RuntimeCosts = value5.AsGodotDictionary<string, int>();
		}
	}
}
