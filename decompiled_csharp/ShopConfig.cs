using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/Shop/ShopConfig.cs")]
public class ShopConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName GetPageTypeList = "GetPageTypeList";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName data = "data";

		public static readonly StringName _data = "_data";

		public static readonly StringName pageList = "pageList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private Json _data;

	[Export(PropertyHint.None, "")]
	public Array<ShopPageConfig> pageList = new Array<ShopPageConfig>();

	[Export(PropertyHint.None, "")]
	public Json data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			Init();
			NotifyPropertyListChanged();
		}
	}

	public void Init()
	{
		pageList.Clear();
		if (data == null)
		{
			return;
		}
		foreach (Variant item in data.Data.AsGodotDictionary()["Page"].AsGodotArray())
		{
			ShopPageConfig shopPageConfig = new ShopPageConfig();
			shopPageConfig.Init(item.AsGodotDictionary());
			pageList.Add(shopPageConfig);
		}
	}

	public Array<ShopPageConfig> GetPageTypeList(string type = "Total")
	{
		if (type == "Total")
		{
			return pageList;
		}
		List<ShopItemConfig> list = new List<ShopItemConfig>();
		foreach (ShopPageConfig page in pageList)
		{
			foreach (ShopItemConfig item in page.itemList)
			{
				if (item.type == type)
				{
					list.Add(item);
				}
			}
		}
		Array<ShopPageConfig> array = new Array<ShopPageConfig>();
		ShopPageConfig shopPageConfig = null;
		for (int i = 0; i < list.Count; i++)
		{
			if (i % 8 == 0)
			{
				shopPageConfig = new ShopPageConfig();
				array.Add(shopPageConfig);
			}
			shopPageConfig.itemList.Add(list[i]);
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPageTypeList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPageTypeList && args.Count == 1)
		{
			Array<ShopPageConfig> pageTypeList = GetPageTypeList(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(pageTypeList);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.GetPageTypeList)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName._data)
		{
			_data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName.pageList)
		{
			pageList = VariantUtils.ConvertToArray<ShopPageConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom<Json>(data);
			return true;
		}
		if (name == PropertyName._data)
		{
			value = VariantUtils.CreateFrom(in _data);
			return true;
		}
		if (name == PropertyName.pageList)
		{
			value = VariantUtils.CreateFromArray(pageList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.data, PropertyHint.ResourceType, "JSON", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.pageList, PropertyHint.TypeString, "24/17:ShopPageConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.data, Variant.From<Json>(data));
		info.AddProperty(PropertyName._data, Variant.From(in _data));
		info.AddProperty(PropertyName.pageList, Variant.CreateFrom(pageList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.data, out var value))
		{
			data = value.As<Json>();
		}
		if (info.TryGetProperty(PropertyName._data, out var value2))
		{
			_data = value2.As<Json>();
		}
		if (info.TryGetProperty(PropertyName.pageList, out var value3))
		{
			pageList = value3.AsGodotArray<ShopPageConfig>();
		}
	}
}
