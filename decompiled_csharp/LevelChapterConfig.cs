using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Level/LevelChapterConfig.cs")]
public class LevelChapterConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName ToRuntimeDictionary = "ToRuntimeDictionary";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName chapterName = "chapterName";

		public static readonly StringName openKey = "openKey";

		public static readonly StringName unlockImage = "unlockImage";

		public static readonly StringName lockImage = "lockImage";

		public static readonly StringName background = "background";

		public static readonly StringName building = "building";

		public static readonly StringName preOpen = "preOpen";

		public static readonly StringName locked = "locked";

		public static readonly StringName levelList = "levelList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string chapterName = "新章节";

	[Export(PropertyHint.None, "")]
	public string openKey = "";

	[Export(PropertyHint.None, "")]
	public Texture2D unlockImage;

	[Export(PropertyHint.None, "")]
	public Texture2D lockImage;

	[Export(PropertyHint.None, "")]
	public Texture2D background;

	[Export(PropertyHint.None, "")]
	public Texture2D building;

	[Export(PropertyHint.Range, "0,20,1")]
	public int preOpen;

	[Export(PropertyHint.None, "")]
	public bool locked;

	[Export(PropertyHint.None, "")]
	public Array<LevelChooseConfig> levelList = new Array<LevelChooseConfig>();

	public Dictionary ToRuntimeDictionary()
	{
		Array array = new Array();
		foreach (LevelChooseConfig level in levelList)
		{
			if (GodotObject.IsInstanceValid(level))
			{
				array.Add(level.ToRuntimeDictionary());
			}
		}
		return new Dictionary
		{
			["Name"] = chapterName ?? "",
			["OpenKey"] = openKey ?? "",
			["UnlockImage"] = LevelChooseConfig.ResourceReference(unlockImage),
			["LockImage"] = LevelChooseConfig.ResourceReference(lockImage),
			["Background"] = LevelChooseConfig.ResourceReference(background),
			["Building"] = LevelChooseConfig.ResourceReference(building),
			["PreOpen"] = preOpen,
			["Lock"] = locked,
			["Level"] = array
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.ToRuntimeDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ToRuntimeDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ToRuntimeDictionary());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ToRuntimeDictionary)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.chapterName)
		{
			chapterName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.openKey)
		{
			openKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.unlockImage)
		{
			unlockImage = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.lockImage)
		{
			lockImage = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.background)
		{
			background = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.building)
		{
			building = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.preOpen)
		{
			preOpen = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.locked)
		{
			locked = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.levelList)
		{
			levelList = VariantUtils.ConvertToArray<LevelChooseConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.chapterName)
		{
			value = VariantUtils.CreateFrom(in chapterName);
			return true;
		}
		if (name == PropertyName.openKey)
		{
			value = VariantUtils.CreateFrom(in openKey);
			return true;
		}
		if (name == PropertyName.unlockImage)
		{
			value = VariantUtils.CreateFrom(in unlockImage);
			return true;
		}
		if (name == PropertyName.lockImage)
		{
			value = VariantUtils.CreateFrom(in lockImage);
			return true;
		}
		if (name == PropertyName.background)
		{
			value = VariantUtils.CreateFrom(in background);
			return true;
		}
		if (name == PropertyName.building)
		{
			value = VariantUtils.CreateFrom(in building);
			return true;
		}
		if (name == PropertyName.preOpen)
		{
			value = VariantUtils.CreateFrom(in preOpen);
			return true;
		}
		if (name == PropertyName.locked)
		{
			value = VariantUtils.CreateFrom(in locked);
			return true;
		}
		if (name == PropertyName.levelList)
		{
			value = VariantUtils.CreateFromArray(levelList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.chapterName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.openKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.unlockImage, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.lockImage, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.background, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.building, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.preOpen, PropertyHint.Range, "0,20,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.locked, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.levelList, PropertyHint.TypeString, "24/17:LevelChooseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.chapterName, Variant.From(in chapterName));
		info.AddProperty(PropertyName.openKey, Variant.From(in openKey));
		info.AddProperty(PropertyName.unlockImage, Variant.From(in unlockImage));
		info.AddProperty(PropertyName.lockImage, Variant.From(in lockImage));
		info.AddProperty(PropertyName.background, Variant.From(in background));
		info.AddProperty(PropertyName.building, Variant.From(in building));
		info.AddProperty(PropertyName.preOpen, Variant.From(in preOpen));
		info.AddProperty(PropertyName.locked, Variant.From(in locked));
		info.AddProperty(PropertyName.levelList, Variant.CreateFrom(levelList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.chapterName, out var value))
		{
			chapterName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.openKey, out var value2))
		{
			openKey = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.unlockImage, out var value3))
		{
			unlockImage = value3.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.lockImage, out var value4))
		{
			lockImage = value4.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.background, out var value5))
		{
			background = value5.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.building, out var value6))
		{
			building = value6.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.preOpen, out var value7))
		{
			preOpen = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.locked, out var value8))
		{
			locked = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.levelList, out var value9))
		{
			levelList = value9.AsGodotArray<LevelChooseConfig>();
		}
	}
}
