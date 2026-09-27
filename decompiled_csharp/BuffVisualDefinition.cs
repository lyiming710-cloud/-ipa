using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BuffComponent/BuffVisualDefinition.cs")]
public class BuffVisualDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName buffKey = "buffKey";

		public static readonly StringName enabled = "enabled";

		public static readonly StringName texture = "texture";

		public static readonly StringName scene = "scene";

		public static readonly StringName nodeName = "nodeName";

		public static readonly StringName position = "position";

		public static readonly StringName scale = "scale";

		public static readonly StringName rotation = "rotation";

		public static readonly StringName zIndex = "zIndex";

		public static readonly StringName zAsRelative = "zAsRelative";

		public static readonly StringName centered = "centered";

		public static readonly StringName offset = "offset";

		public static readonly StringName drawBand = "drawBand";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName buffKey;

	[Export(PropertyHint.None, "")]
	public bool enabled = true;

	[Export(PropertyHint.None, "")]
	public Texture2D texture;

	[Export(PropertyHint.None, "")]
	public PackedScene scene;

	[Export(PropertyHint.None, "")]
	public StringName nodeName;

	[Export(PropertyHint.None, "")]
	public Vector2 position;

	[Export(PropertyHint.None, "")]
	public Vector2 scale = Vector2.One;

	[Export(PropertyHint.None, "")]
	public float rotation;

	[Export(PropertyHint.Range, "-4096,4096,1")]
	public int zIndex;

	[Export(PropertyHint.None, "")]
	public bool zAsRelative = true;

	[Export(PropertyHint.None, "")]
	public bool centered = true;

	[Export(PropertyHint.None, "")]
	public Vector2 offset;

	[Export(PropertyHint.None, "")]
	public AdobeAnimateExternalVisualDrawBand drawBand = AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.buffKey)
		{
			buffKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.enabled)
		{
			enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.texture)
		{
			texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.scene)
		{
			scene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.nodeName)
		{
			nodeName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.position)
		{
			position = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.scale)
		{
			scale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.rotation)
		{
			rotation = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.zIndex)
		{
			zIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.zAsRelative)
		{
			zAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.centered)
		{
			centered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.offset)
		{
			offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.drawBand)
		{
			drawBand = VariantUtils.ConvertTo<AdobeAnimateExternalVisualDrawBand>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.buffKey)
		{
			value = VariantUtils.CreateFrom(in buffKey);
			return true;
		}
		if (name == PropertyName.enabled)
		{
			value = VariantUtils.CreateFrom(in enabled);
			return true;
		}
		if (name == PropertyName.texture)
		{
			value = VariantUtils.CreateFrom(in texture);
			return true;
		}
		if (name == PropertyName.scene)
		{
			value = VariantUtils.CreateFrom(in scene);
			return true;
		}
		if (name == PropertyName.nodeName)
		{
			value = VariantUtils.CreateFrom(in nodeName);
			return true;
		}
		if (name == PropertyName.position)
		{
			value = VariantUtils.CreateFrom(in position);
			return true;
		}
		if (name == PropertyName.scale)
		{
			value = VariantUtils.CreateFrom(in scale);
			return true;
		}
		if (name == PropertyName.rotation)
		{
			value = VariantUtils.CreateFrom(in rotation);
			return true;
		}
		if (name == PropertyName.zIndex)
		{
			value = VariantUtils.CreateFrom(in zIndex);
			return true;
		}
		if (name == PropertyName.zAsRelative)
		{
			value = VariantUtils.CreateFrom(in zAsRelative);
			return true;
		}
		if (name == PropertyName.centered)
		{
			value = VariantUtils.CreateFrom(in centered);
			return true;
		}
		if (name == PropertyName.offset)
		{
			value = VariantUtils.CreateFrom(in offset);
			return true;
		}
		if (name == PropertyName.drawBand)
		{
			value = VariantUtils.CreateFrom(in drawBand);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.buffKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.texture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.scene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.nodeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.position, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rotation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.zIndex, PropertyHint.Range, "-4096,4096,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.zAsRelative, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.centered, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.drawBand, PropertyHint.Enum, "BehindAnimation,InFrontOfAnimation", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.buffKey, Variant.From(in buffKey));
		info.AddProperty(PropertyName.enabled, Variant.From(in enabled));
		info.AddProperty(PropertyName.texture, Variant.From(in texture));
		info.AddProperty(PropertyName.scene, Variant.From(in scene));
		info.AddProperty(PropertyName.nodeName, Variant.From(in nodeName));
		info.AddProperty(PropertyName.position, Variant.From(in position));
		info.AddProperty(PropertyName.scale, Variant.From(in scale));
		info.AddProperty(PropertyName.rotation, Variant.From(in rotation));
		info.AddProperty(PropertyName.zIndex, Variant.From(in zIndex));
		info.AddProperty(PropertyName.zAsRelative, Variant.From(in zAsRelative));
		info.AddProperty(PropertyName.centered, Variant.From(in centered));
		info.AddProperty(PropertyName.offset, Variant.From(in offset));
		info.AddProperty(PropertyName.drawBand, Variant.From(in drawBand));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.buffKey, out var value))
		{
			buffKey = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.enabled, out var value2))
		{
			enabled = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.texture, out var value3))
		{
			texture = value3.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.scene, out var value4))
		{
			scene = value4.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.nodeName, out var value5))
		{
			nodeName = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.position, out var value6))
		{
			position = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.scale, out var value7))
		{
			scale = value7.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.rotation, out var value8))
		{
			rotation = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.zIndex, out var value9))
		{
			zIndex = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.zAsRelative, out var value10))
		{
			zAsRelative = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.centered, out var value11))
		{
			centered = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.offset, out var value12))
		{
			offset = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.drawBand, out var value13))
		{
			drawBand = value13.As<AdobeAnimateExternalVisualDrawBand>();
		}
	}
}
