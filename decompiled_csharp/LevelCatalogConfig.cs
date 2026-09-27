using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Level/LevelCatalogConfig.cs")]
public class LevelCatalogConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName ToRuntimeDictionary = "ToRuntimeDictionary";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName catalogKey = "catalogKey";

		public static readonly StringName chapterList = "chapterList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string catalogKey = "Adventure";

	[Export(PropertyHint.None, "")]
	public Array<LevelChapterConfig> chapterList = new Array<LevelChapterConfig>();

	public Dictionary ToRuntimeDictionary()
	{
		Array array = new Array();
		foreach (LevelChapterConfig chapter in chapterList)
		{
			if (GodotObject.IsInstanceValid(chapter))
			{
				array.Add(chapter.ToRuntimeDictionary());
			}
		}
		return new Dictionary { ["Chapter"] = array };
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
		if (name == PropertyName.catalogKey)
		{
			catalogKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.chapterList)
		{
			chapterList = VariantUtils.ConvertToArray<LevelChapterConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.catalogKey)
		{
			value = VariantUtils.CreateFrom(in catalogKey);
			return true;
		}
		if (name == PropertyName.chapterList)
		{
			value = VariantUtils.CreateFromArray(chapterList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.catalogKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.chapterList, PropertyHint.TypeString, "24/17:LevelChapterConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.catalogKey, Variant.From(in catalogKey));
		info.AddProperty(PropertyName.chapterList, Variant.CreateFrom(chapterList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.catalogKey, out var value))
		{
			catalogKey = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.chapterList, out var value2))
		{
			chapterList = value2.AsGodotArray<LevelChapterConfig>();
		}
	}
}
