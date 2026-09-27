using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/General/Character/Costom/CharacterCustomConfig.cs")]
public class CharacterCustomConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName animeFliterOpen = "animeFliterOpen";

		public static readonly StringName animeFliterClose = "animeFliterClose";

		public static readonly StringName openKey = "openKey";

		public static readonly StringName customName = "customName";

		public static readonly StringName type = "type";

		public static readonly StringName customHandbookName = "customHandbookName";

		public static readonly StringName customHandbookAccess = "customHandbookAccess";

		public static readonly StringName customHandbookStory = "customHandbookStory";

		public static readonly StringName _animeFliterOpen = "_animeFliterOpen";

		public static readonly StringName _animeFliterClose = "_animeFliterClose";

		public static readonly StringName damagePointChangeMediaName = "damagePointChangeMediaName";

		public static readonly StringName damagePointChangeMediaTexturePaths = "damagePointChangeMediaTexturePaths";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string openKey = "";

	[Export(PropertyHint.None, "")]
	public string customName = "";

	[Export(PropertyHint.Enum, "White,Gold")]
	public string type = "White";

	[Export(PropertyHint.None, "")]
	public string customHandbookName = "";

	[Export(PropertyHint.None, "")]
	public string customHandbookAccess = "";

	[Export(PropertyHint.None, "")]
	public string customHandbookStory = "";

	private string _animeFliterOpen = "";

	private string _animeFliterClose = "";

	[Export(PropertyHint.None, "")]
	public string damagePointChangeMediaName = "";

	[Export(PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga")]
	public Array<string> damagePointChangeMediaTexturePaths = new Array<string>();

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
		if (name == PropertyName.openKey)
		{
			openKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.customName)
		{
			customName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.customHandbookName)
		{
			customHandbookName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.customHandbookAccess)
		{
			customHandbookAccess = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.customHandbookStory)
		{
			customHandbookStory = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.damagePointChangeMediaName)
		{
			damagePointChangeMediaName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damagePointChangeMediaTexturePaths)
		{
			damagePointChangeMediaTexturePaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
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
		if (name == PropertyName.openKey)
		{
			value = VariantUtils.CreateFrom(in openKey);
			return true;
		}
		if (name == PropertyName.customName)
		{
			value = VariantUtils.CreateFrom(in customName);
			return true;
		}
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.customHandbookName)
		{
			value = VariantUtils.CreateFrom(in customHandbookName);
			return true;
		}
		if (name == PropertyName.customHandbookAccess)
		{
			value = VariantUtils.CreateFrom(in customHandbookAccess);
			return true;
		}
		if (name == PropertyName.customHandbookStory)
		{
			value = VariantUtils.CreateFrom(in customHandbookStory);
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
		if (name == PropertyName.damagePointChangeMediaName)
		{
			value = VariantUtils.CreateFrom(in damagePointChangeMediaName);
			return true;
		}
		if (name == PropertyName.damagePointChangeMediaTexturePaths)
		{
			value = VariantUtils.CreateFromArray(damagePointChangeMediaTexturePaths);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.openKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.customName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.Enum, "White,Gold", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.customHandbookName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.customHandbookAccess, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.customHandbookStory, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._animeFliterOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._animeFliterClose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.animeFliterOpen, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.animeFliterClose, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.damagePointChangeMediaName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.damagePointChangeMediaTexturePaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.animeFliterOpen, Variant.From<string>(animeFliterOpen));
		info.AddProperty(PropertyName.animeFliterClose, Variant.From<string>(animeFliterClose));
		info.AddProperty(PropertyName.openKey, Variant.From(in openKey));
		info.AddProperty(PropertyName.customName, Variant.From(in customName));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.customHandbookName, Variant.From(in customHandbookName));
		info.AddProperty(PropertyName.customHandbookAccess, Variant.From(in customHandbookAccess));
		info.AddProperty(PropertyName.customHandbookStory, Variant.From(in customHandbookStory));
		info.AddProperty(PropertyName._animeFliterOpen, Variant.From(in _animeFliterOpen));
		info.AddProperty(PropertyName._animeFliterClose, Variant.From(in _animeFliterClose));
		info.AddProperty(PropertyName.damagePointChangeMediaName, Variant.From(in damagePointChangeMediaName));
		info.AddProperty(PropertyName.damagePointChangeMediaTexturePaths, Variant.CreateFrom(damagePointChangeMediaTexturePaths));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.animeFliterOpen, out var value))
		{
			animeFliterOpen = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.animeFliterClose, out var value2))
		{
			animeFliterClose = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.openKey, out var value3))
		{
			openKey = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.customName, out var value4))
		{
			customName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value5))
		{
			type = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.customHandbookName, out var value6))
		{
			customHandbookName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.customHandbookAccess, out var value7))
		{
			customHandbookAccess = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.customHandbookStory, out var value8))
		{
			customHandbookStory = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animeFliterOpen, out var value9))
		{
			_animeFliterOpen = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animeFliterClose, out var value10))
		{
			_animeFliterClose = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damagePointChangeMediaName, out var value11))
		{
			damagePointChangeMediaName = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damagePointChangeMediaTexturePaths, out var value12))
		{
			damagePointChangeMediaTexturePaths = value12.AsGodotArray<string>();
		}
	}
}
