using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/AwardNote/AwardSettlementConfig.cs")]
public class AwardSettlementConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName texture = "texture";

		public static readonly StringName background = "background";

		public static readonly StringName image = "image";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Texture2D texture;

	[Export(PropertyHint.None, "")]
	public Texture2D background;

	[Export(PropertyHint.None, "")]
	public Texture2D image;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.texture)
		{
			texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.background)
		{
			background = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.image)
		{
			image = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.texture)
		{
			value = VariantUtils.CreateFrom(in texture);
			return true;
		}
		if (name == PropertyName.background)
		{
			value = VariantUtils.CreateFrom(in background);
			return true;
		}
		if (name == PropertyName.image)
		{
			value = VariantUtils.CreateFrom(in image);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.texture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.background, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.image, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.texture, Variant.From(in texture));
		info.AddProperty(PropertyName.background, Variant.From(in background));
		info.AddProperty(PropertyName.image, Variant.From(in image));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.texture, out var value))
		{
			texture = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.background, out var value2))
		{
			background = value2.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.image, out var value3))
		{
			image = value3.As<Texture2D>();
		}
	}
}
