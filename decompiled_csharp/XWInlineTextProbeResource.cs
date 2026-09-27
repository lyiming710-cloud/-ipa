using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Tests/XWInlineTextProbeResource.cs")]
public class XWInlineTextProbeResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Title = "Title";

		public static readonly StringName Description = "Description";

		public static readonly StringName DialogueText = "DialogueText";

		public static readonly StringName HintText = "HintText";

		public static readonly StringName ButtonText = "ButtonText";

		public static readonly StringName SaveKey = "SaveKey";

		public static readonly StringName SleepMode = "SleepMode";

		public static readonly StringName TargetPath = "TargetPath";

		public static readonly StringName ResourceFile = "ResourceFile";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string Title { get; set; } = "Original title";

	[Export(PropertyHint.MultilineText, "")]
	public string Description { get; set; } = "Original description";

	[Export(PropertyHint.None, "")]
	public string DialogueText { get; set; } = "Original dialogue";

	[Export(PropertyHint.None, "")]
	public string HintText { get; set; } = "Original hint";

	[Export(PropertyHint.None, "")]
	public string ButtonText { get; set; } = "Original button";

	[Export(PropertyHint.None, "")]
	public StringName SaveKey { get; set; } = new StringName("probe_key");

	[Export(PropertyHint.Enum, "Never,Night,Day")]
	public string SleepMode { get; set; } = "Never";

	[Export(PropertyHint.None, "")]
	public NodePath TargetPath { get; set; } = new NodePath("PreviewRoot/Target");

	[Export(PropertyHint.File, "*.tres,*.res,*.tscn")]
	public string ResourceFile { get; set; } = "res://";

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Title)
		{
			Title = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Description)
		{
			Description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DialogueText)
		{
			DialogueText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.HintText)
		{
			HintText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ButtonText)
		{
			ButtonText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SaveKey)
		{
			SaveKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.SleepMode)
		{
			SleepMode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.TargetPath)
		{
			TargetPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.ResourceFile)
		{
			ResourceFile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.Title)
		{
			from = Title;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Description)
		{
			from = Description;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DialogueText)
		{
			from = DialogueText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HintText)
		{
			from = HintText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ButtonText)
		{
			from = ButtonText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SaveKey)
		{
			value = VariantUtils.CreateFrom<StringName>(SaveKey);
			return true;
		}
		if (name == PropertyName.SleepMode)
		{
			from = SleepMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TargetPath)
		{
			value = VariantUtils.CreateFrom<NodePath>(TargetPath);
			return true;
		}
		if (name == PropertyName.ResourceFile)
		{
			from = ResourceFile;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.Title, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.Description, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.DialogueText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.HintText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ButtonText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.SaveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.SleepMode, PropertyHint.Enum, "Never,Night,Day", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.TargetPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ResourceFile, PropertyHint.File, "*.tres,*.res,*.tscn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Title, Variant.From<string>(Title));
		info.AddProperty(PropertyName.Description, Variant.From<string>(Description));
		info.AddProperty(PropertyName.DialogueText, Variant.From<string>(DialogueText));
		info.AddProperty(PropertyName.HintText, Variant.From<string>(HintText));
		info.AddProperty(PropertyName.ButtonText, Variant.From<string>(ButtonText));
		info.AddProperty(PropertyName.SaveKey, Variant.From<StringName>(SaveKey));
		info.AddProperty(PropertyName.SleepMode, Variant.From<string>(SleepMode));
		info.AddProperty(PropertyName.TargetPath, Variant.From<NodePath>(TargetPath));
		info.AddProperty(PropertyName.ResourceFile, Variant.From<string>(ResourceFile));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Title, out var value))
		{
			Title = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Description, out var value2))
		{
			Description = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DialogueText, out var value3))
		{
			DialogueText = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.HintText, out var value4))
		{
			HintText = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ButtonText, out var value5))
		{
			ButtonText = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SaveKey, out var value6))
		{
			SaveKey = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.SleepMode, out var value7))
		{
			SleepMode = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.TargetPath, out var value8))
		{
			TargetPath = value8.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.ResourceFile, out var value9))
		{
			ResourceFile = value9.As<string>();
		}
	}
}
