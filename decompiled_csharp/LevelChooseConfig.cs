using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Level/LevelChooseConfig.cs")]
public class LevelChooseConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName ToRuntimeDictionary = "ToRuntimeDictionary";

		public static readonly StringName ResourceReference = "ResourceReference";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName openKey = "openKey";

		public static readonly StringName saveKey = "saveKey";

		public static readonly StringName unlockImage = "unlockImage";

		public static readonly StringName normalLevel = "normalLevel";

		public static readonly StringName difficultLevel = "difficultLevel";

		public static readonly StringName ultimateLevel = "ultimateLevel";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string openKey = "";

	[Export(PropertyHint.None, "")]
	public string saveKey = "";

	[Export(PropertyHint.None, "")]
	public Texture2D unlockImage;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelBaseConfig normalLevel;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelBaseConfig difficultLevel;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelBaseConfig ultimateLevel;

	public Dictionary ToRuntimeDictionary()
	{
		return new Dictionary
		{
			["OpenKey"] = openKey ?? "",
			["SaveKey"] = saveKey ?? "",
			["UnlockImage"] = ResourceReference(unlockImage),
			["Level"] = new Dictionary
			{
				["Normal"] = ResourceReference(normalLevel),
				["Difficult"] = ResourceReference(difficultLevel),
				["Ultimate"] = ResourceReference(ultimateLevel)
			}
		};
	}

	internal static string ResourceReference(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "";
		}
		return resource.ResourcePath ?? "";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.ToRuntimeDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResourceReference, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
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
		if (method == MethodName.ResourceReference && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResourceReference(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResourceReference && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResourceReference(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ToRuntimeDictionary)
		{
			return true;
		}
		if (method == MethodName.ResourceReference)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.openKey)
		{
			openKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			saveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.unlockImage)
		{
			unlockImage = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.normalLevel)
		{
			normalLevel = VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.difficultLevel)
		{
			difficultLevel = VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.ultimateLevel)
		{
			ultimateLevel = VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.openKey)
		{
			value = VariantUtils.CreateFrom(in openKey);
			return true;
		}
		if (name == PropertyName.saveKey)
		{
			value = VariantUtils.CreateFrom(in saveKey);
			return true;
		}
		if (name == PropertyName.unlockImage)
		{
			value = VariantUtils.CreateFrom(in unlockImage);
			return true;
		}
		if (name == PropertyName.normalLevel)
		{
			value = VariantUtils.CreateFrom(in normalLevel);
			return true;
		}
		if (name == PropertyName.difficultLevel)
		{
			value = VariantUtils.CreateFrom(in difficultLevel);
			return true;
		}
		if (name == PropertyName.ultimateLevel)
		{
			value = VariantUtils.CreateFrom(in ultimateLevel);
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
			new PropertyInfo(Variant.Type.String, PropertyName.saveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.unlockImage, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.normalLevel, PropertyHint.ResourceType, "TowerDefenseLevelBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.difficultLevel, PropertyHint.ResourceType, "TowerDefenseLevelBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.ultimateLevel, PropertyHint.ResourceType, "TowerDefenseLevelBaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.openKey, Variant.From(in openKey));
		info.AddProperty(PropertyName.saveKey, Variant.From(in saveKey));
		info.AddProperty(PropertyName.unlockImage, Variant.From(in unlockImage));
		info.AddProperty(PropertyName.normalLevel, Variant.From(in normalLevel));
		info.AddProperty(PropertyName.difficultLevel, Variant.From(in difficultLevel));
		info.AddProperty(PropertyName.ultimateLevel, Variant.From(in ultimateLevel));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.openKey, out var value))
		{
			openKey = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.saveKey, out var value2))
		{
			saveKey = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.unlockImage, out var value3))
		{
			unlockImage = value3.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.normalLevel, out var value4))
		{
			normalLevel = value4.As<TowerDefenseLevelBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.difficultLevel, out var value5))
		{
			difficultLevel = value5.As<TowerDefenseLevelBaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.ultimateLevel, out var value6))
		{
			ultimateLevel = value6.As<TowerDefenseLevelBaseConfig>();
		}
	}
}
