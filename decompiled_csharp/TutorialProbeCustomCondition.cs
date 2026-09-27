using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Tests/TutorialProbeCustomCondition.cs")]
public class TutorialProbeCustomCondition : TutorialConditionConfig
{
	public new class MethodName : TutorialConditionConfig.MethodName
	{
	}

	public new class PropertyName : TutorialConditionConfig.PropertyName
	{
		public static readonly StringName customValue = "customValue";

		public static readonly StringName playerText = "playerText";

		public static readonly StringName accentColor = "accentColor";
	}

	public new class SignalName : TutorialConditionConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int customValue = 7;

	[Export(PropertyHint.None, "")]
	public string playerText = "自定义条件提示";

	[Export(PropertyHint.None, "")]
	public Color accentColor = new Color(0.2f, 0.8f, 0.35f);

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.customValue)
		{
			customValue = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.playerText)
		{
			playerText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.accentColor)
		{
			accentColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.customValue)
		{
			value = VariantUtils.CreateFrom(in customValue);
			return true;
		}
		if (name == PropertyName.playerText)
		{
			value = VariantUtils.CreateFrom(in playerText);
			return true;
		}
		if (name == PropertyName.accentColor)
		{
			value = VariantUtils.CreateFrom(in accentColor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.customValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.playerText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.accentColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.customValue, Variant.From(in customValue));
		info.AddProperty(PropertyName.playerText, Variant.From(in playerText));
		info.AddProperty(PropertyName.accentColor, Variant.From(in accentColor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.customValue, out var value))
		{
			customValue = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.playerText, out var value2))
		{
			playerText = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.accentColor, out var value3))
		{
			accentColor = value3.As<Color>();
		}
	}
}
