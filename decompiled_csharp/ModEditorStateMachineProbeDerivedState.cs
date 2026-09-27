using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Test/ModEditorStateMachineProbeDerivedState.cs")]
public class ModEditorStateMachineProbeDerivedState : StateMachineStateDefinition
{
	public new class MethodName : StateMachineStateDefinition.MethodName
	{
	}

	public new class PropertyName : StateMachineStateDefinition.PropertyName
	{
		public static readonly StringName ModStateNote = "ModStateNote";

		public static readonly StringName RuntimeWeight = "RuntimeWeight";

		public static readonly StringName RuntimeEnabled = "RuntimeEnabled";

		public static readonly StringName AccentColor = "AccentColor";

		public static readonly StringName PreviewOffset = "PreviewOffset";

		public static readonly StringName RuntimeTags = "RuntimeTags";
	}

	public new class SignalName : StateMachineStateDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ModStateNote { get; set; } = "state-base";

	[Export(PropertyHint.Range, "0,100,1")]
	public int RuntimeWeight { get; set; } = 3;

	[Export(PropertyHint.None, "")]
	public bool RuntimeEnabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color AccentColor { get; set; } = new Color("62d6ff");

	[Export(PropertyHint.None, "")]
	public Vector2 PreviewOffset { get; set; } = new Vector2(12f, -4f);

	[Export(PropertyHint.None, "")]
	public Array<string> RuntimeTags { get; set; } = new Array<string> { "probe", "state" };

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ModStateNote)
		{
			ModStateNote = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeWeight)
		{
			RuntimeWeight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeEnabled)
		{
			RuntimeEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.AccentColor)
		{
			AccentColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.PreviewOffset)
		{
			PreviewOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeTags)
		{
			RuntimeTags = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ModStateNote)
		{
			value = VariantUtils.CreateFrom<string>(ModStateNote);
			return true;
		}
		if (name == PropertyName.RuntimeWeight)
		{
			value = VariantUtils.CreateFrom<int>(RuntimeWeight);
			return true;
		}
		if (name == PropertyName.RuntimeEnabled)
		{
			value = VariantUtils.CreateFrom<bool>(RuntimeEnabled);
			return true;
		}
		if (name == PropertyName.AccentColor)
		{
			value = VariantUtils.CreateFrom<Color>(AccentColor);
			return true;
		}
		if (name == PropertyName.PreviewOffset)
		{
			value = VariantUtils.CreateFrom<Vector2>(PreviewOffset);
			return true;
		}
		if (name == PropertyName.RuntimeTags)
		{
			value = VariantUtils.CreateFromArray(RuntimeTags);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ModStateNote, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimeWeight, PropertyHint.Range, "0,100,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RuntimeEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.AccentColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.PreviewOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.RuntimeTags, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ModStateNote, Variant.From<string>(ModStateNote));
		info.AddProperty(PropertyName.RuntimeWeight, Variant.From<int>(RuntimeWeight));
		info.AddProperty(PropertyName.RuntimeEnabled, Variant.From<bool>(RuntimeEnabled));
		info.AddProperty(PropertyName.AccentColor, Variant.From<Color>(AccentColor));
		info.AddProperty(PropertyName.PreviewOffset, Variant.From<Vector2>(PreviewOffset));
		info.AddProperty(PropertyName.RuntimeTags, Variant.CreateFrom(RuntimeTags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ModStateNote, out var value))
		{
			ModStateNote = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeWeight, out var value2))
		{
			RuntimeWeight = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeEnabled, out var value3))
		{
			RuntimeEnabled = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.AccentColor, out var value4))
		{
			AccentColor = value4.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.PreviewOffset, out var value5))
		{
			PreviewOffset = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeTags, out var value6))
		{
			RuntimeTags = value6.AsGodotArray<string>();
		}
	}
}
