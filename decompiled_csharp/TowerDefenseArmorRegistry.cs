using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Armor/TowerDefenseArmorRegistry.cs")]
public class TowerDefenseArmorRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName RegisterArmor = "RegisterArmor";

		public static readonly StringName GetArmorType = "GetArmorType";

		public static readonly StringName HasArmor = "HasArmor";

		public static readonly StringName GetArmorNames = "GetArmorNames";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private static Json _registryJson;

	public static bool IsInit = false;

	public static System.Collections.Generic.Dictionary<StringName, TowerDefenseArmorTypeData> ArmorDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseArmorTypeData>();

	private static Json RegistryJson => _registryJson ?? (_registryJson = GD.Load<Json>("res://Registry/Armor/ArmorRegistry.json"));

	public static void Init()
	{
		if (!IsInit)
		{
			IsInit = true;
			RegisterInit();
		}
	}

	public static void RegisterInit()
	{
		Dictionary dictionary = (Dictionary)RegistryJson.Data;
		Dictionary dictionary2 = (dictionary.ContainsKey("Armors") ? ((Dictionary)dictionary["Armors"]) : new Dictionary());
		foreach (Variant key in dictionary2.Keys)
		{
			string name = (string)key;
			TowerDefenseArmorTypeData towerDefenseArmorTypeData = GD.Load<TowerDefenseArmorTypeData>((string)dictionary2[key]);
			if (towerDefenseArmorTypeData != null)
			{
				RegisterArmor(new StringName(name), towerDefenseArmorTypeData);
			}
		}
	}

	public static void RegisterArmor(StringName armorName, TowerDefenseArmorTypeData armorTypeData)
	{
		ArmorDictionary[armorName] = armorTypeData;
	}

	public static TowerDefenseArmorTypeData GetArmorType(StringName armorName)
	{
		if (!ArmorDictionary.ContainsKey(armorName))
		{
			return null;
		}
		return ArmorDictionary[armorName].Duplicate() as TowerDefenseArmorTypeData;
	}

	public static bool HasArmor(StringName armorName)
	{
		return ArmorDictionary.ContainsKey(armorName);
	}

	public static Array<StringName> GetArmorNames()
	{
		Array<StringName> array = new Array<StringName>();
		foreach (StringName key in ArmorDictionary.Keys)
		{
			array.Add(key);
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "armorTypeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetArmorType, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasArmor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArmorNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
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
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterArmor && args.Count == 2)
		{
			RegisterArmor(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetArmorType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorTypeData>(GetArmorType(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasArmor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasArmor(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetArmorNames && args.Count == 0)
		{
			Array<StringName> armorNames = GetArmorNames();
			ret = VariantUtils.CreateFromArray(armorNames);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterArmor && args.Count == 2)
		{
			RegisterArmor(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetArmorType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorTypeData>(GetArmorType(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasArmor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasArmor(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetArmorNames && args.Count == 0)
		{
			Array<StringName> armorNames = GetArmorNames();
			ret = VariantUtils.CreateFromArray(armorNames);
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.RegisterArmor)
		{
			return true;
		}
		if (method == MethodName.GetArmorType)
		{
			return true;
		}
		if (method == MethodName.HasArmor)
		{
			return true;
		}
		if (method == MethodName.GetArmorNames)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
