using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/General/Character/DamagePoint/CharacterDamagePointConfig.cs")]
public class CharacterDamagePointConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName damagePointName = "damagePointName";

		public static readonly StringName animeFliterOpen = "animeFliterOpen";

		public static readonly StringName animeFliterClose = "animeFliterClose";

		public static readonly StringName _damagePointName = "_damagePointName";

		public static readonly StringName _animeFliterOpen = "_animeFliterOpen";

		public static readonly StringName _animeFliterClose = "_animeFliterClose";

		public static readonly StringName damagePersontage = "damagePersontage";

		public static readonly StringName animeEffect = "animeEffect";

		public static readonly StringName animeEffectOffset = "animeEffectOffset";

		public static readonly StringName isDrop = "isDrop";

		public static readonly StringName replaceMediaName = "replaceMediaName";

		public static readonly StringName replaceMediaTexturePath = "replaceMediaTexturePath";

		public static readonly StringName damageAudio = "damageAudio";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private string _damagePointName = "";

	private string _animeFliterOpen = "";

	private string _animeFliterClose = "";

	[Export(PropertyHint.None, "")]
	public double damagePersontage = 0.5;

	[Export(PropertyHint.None, "")]
	public PackedScene animeEffect;

	[Export(PropertyHint.None, "")]
	public Vector2 animeEffectOffset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public bool isDrop = true;

	[Export(PropertyHint.None, "")]
	public StringName replaceMediaName = "";

	[Export(PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga")]
	public string replaceMediaTexturePath = "";

	[Export(PropertyHint.None, "")]
	public string damageAudio = "LimbsPop";

	[Export(PropertyHint.None, "")]
	public string damagePointName
	{
		get
		{
			return _damagePointName;
		}
		set
		{
			_damagePointName = value;
			EmitChanged();
		}
	}

	[Export(PropertyHint.MultilineText, "")]
	public string animeFliterOpen
	{
		get
		{
			return _animeFliterOpen;
		}
		set
		{
			_animeFliterOpen = value;
			EmitChanged();
		}
	}

	[Export(PropertyHint.MultilineText, "")]
	public string animeFliterClose
	{
		get
		{
			return _animeFliterClose;
		}
		set
		{
			_animeFliterClose = value;
			EmitChanged();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.damagePointName)
		{
			damagePointName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.animeFliterOpen)
		{
			animeFliterOpen = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.animeFliterClose)
		{
			animeFliterClose = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._damagePointName)
		{
			_damagePointName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animeFliterOpen)
		{
			_animeFliterOpen = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animeFliterClose)
		{
			_animeFliterClose = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damagePersontage)
		{
			damagePersontage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.animeEffect)
		{
			animeEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.animeEffectOffset)
		{
			animeEffectOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.isDrop)
		{
			isDrop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.replaceMediaName)
		{
			replaceMediaName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.replaceMediaTexturePath)
		{
			replaceMediaTexturePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damageAudio)
		{
			damageAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.damagePointName)
		{
			from = damagePointName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.animeFliterOpen)
		{
			from = animeFliterOpen;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.animeFliterClose)
		{
			from = animeFliterClose;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._damagePointName)
		{
			value = VariantUtils.CreateFrom(in _damagePointName);
			return true;
		}
		if (name == PropertyName._animeFliterOpen)
		{
			value = VariantUtils.CreateFrom(in _animeFliterOpen);
			return true;
		}
		if (name == PropertyName._animeFliterClose)
		{
			value = VariantUtils.CreateFrom(in _animeFliterClose);
			return true;
		}
		if (name == PropertyName.damagePersontage)
		{
			value = VariantUtils.CreateFrom(in damagePersontage);
			return true;
		}
		if (name == PropertyName.animeEffect)
		{
			value = VariantUtils.CreateFrom(in animeEffect);
			return true;
		}
		if (name == PropertyName.animeEffectOffset)
		{
			value = VariantUtils.CreateFrom(in animeEffectOffset);
			return true;
		}
		if (name == PropertyName.isDrop)
		{
			value = VariantUtils.CreateFrom(in isDrop);
			return true;
		}
		if (name == PropertyName.replaceMediaName)
		{
			value = VariantUtils.CreateFrom(in replaceMediaName);
			return true;
		}
		if (name == PropertyName.replaceMediaTexturePath)
		{
			value = VariantUtils.CreateFrom(in replaceMediaTexturePath);
			return true;
		}
		if (name == PropertyName.damageAudio)
		{
			value = VariantUtils.CreateFrom(in damageAudio);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._damagePointName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animeFliterOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animeFliterClose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.damagePointName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damagePersontage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.animeEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.animeEffectOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isDrop, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.animeFliterOpen, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.animeFliterClose, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.replaceMediaName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.replaceMediaTexturePath, PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.damageAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.damagePointName, Variant.From<string>(damagePointName));
		info.AddProperty(PropertyName.animeFliterOpen, Variant.From<string>(animeFliterOpen));
		info.AddProperty(PropertyName.animeFliterClose, Variant.From<string>(animeFliterClose));
		info.AddProperty(PropertyName._damagePointName, Variant.From(in _damagePointName));
		info.AddProperty(PropertyName._animeFliterOpen, Variant.From(in _animeFliterOpen));
		info.AddProperty(PropertyName._animeFliterClose, Variant.From(in _animeFliterClose));
		info.AddProperty(PropertyName.damagePersontage, Variant.From(in damagePersontage));
		info.AddProperty(PropertyName.animeEffect, Variant.From(in animeEffect));
		info.AddProperty(PropertyName.animeEffectOffset, Variant.From(in animeEffectOffset));
		info.AddProperty(PropertyName.isDrop, Variant.From(in isDrop));
		info.AddProperty(PropertyName.replaceMediaName, Variant.From(in replaceMediaName));
		info.AddProperty(PropertyName.replaceMediaTexturePath, Variant.From(in replaceMediaTexturePath));
		info.AddProperty(PropertyName.damageAudio, Variant.From(in damageAudio));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.damagePointName, out var value))
		{
			damagePointName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.animeFliterOpen, out var value2))
		{
			animeFliterOpen = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.animeFliterClose, out var value3))
		{
			animeFliterClose = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._damagePointName, out var value4))
		{
			_damagePointName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animeFliterOpen, out var value5))
		{
			_animeFliterOpen = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animeFliterClose, out var value6))
		{
			_animeFliterClose = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damagePersontage, out var value7))
		{
			damagePersontage = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.animeEffect, out var value8))
		{
			animeEffect = value8.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.animeEffectOffset, out var value9))
		{
			animeEffectOffset = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.isDrop, out var value10))
		{
			isDrop = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.replaceMediaName, out var value11))
		{
			replaceMediaName = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.replaceMediaTexturePath, out var value12))
		{
			replaceMediaTexturePath = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damageAudio, out var value13))
		{
			damageAudio = value13.As<string>();
		}
	}
}
