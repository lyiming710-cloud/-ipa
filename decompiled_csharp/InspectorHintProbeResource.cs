using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
public class InspectorHintProbeResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName SuggestedCard = "SuggestedCard";

		public static readonly StringName Locale = "Locale";

		public static readonly StringName TargetObjectId = "TargetObjectId";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.EnumSuggestion, "pea,sun,wall")]
	public string SuggestedCard { get; set; } = "pea";

	[Export(PropertyHint.LocaleId, "")]
	public string Locale { get; set; } = "zh_CN";

	[Export(PropertyHint.ObjectId, "")]
	public long TargetObjectId { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SuggestedCard)
		{
			SuggestedCard = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Locale)
		{
			Locale = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.TargetObjectId)
		{
			TargetObjectId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.SuggestedCard)
		{
			from = SuggestedCard;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Locale)
		{
			from = Locale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TargetObjectId)
		{
			value = VariantUtils.CreateFrom<long>(TargetObjectId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.SuggestedCard, PropertyHint.EnumSuggestion, "pea,sun,wall", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.Locale, PropertyHint.LocaleId, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.TargetObjectId, PropertyHint.ObjectId, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SuggestedCard, Variant.From<string>(SuggestedCard));
		info.AddProperty(PropertyName.Locale, Variant.From<string>(Locale));
		info.AddProperty(PropertyName.TargetObjectId, Variant.From<long>(TargetObjectId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SuggestedCard, out var value))
		{
			SuggestedCard = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Locale, out var value2))
		{
			Locale = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.TargetObjectId, out var value3))
		{
			TargetObjectId = value3.As<long>();
		}
	}
}
