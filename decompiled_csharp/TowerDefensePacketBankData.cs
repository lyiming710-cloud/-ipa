using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/Resource/TowerDefensePacketBankData.cs")]
public class TowerDefensePacketBankData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetUnlockPacket = "GetUnlockPacket";

		public static readonly StringName GetCategory = "GetCategory";

		public static readonly StringName GetPacketList = "GetPacketList";

		public static readonly StringName GetPlantList = "GetPlantList";

		public static readonly StringName GetZombieList = "GetZombieList";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName category = "category";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Dictionary category = new Dictionary();

	public Array<string> GetUnlockPacket()
	{
		Array<string> array = new Array<string>();
		foreach (Variant value in category.Values)
		{
			foreach (Variant item in (Array)value)
			{
				string text = (string)item;
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
				if (packetConfig != null && packetConfig.Unlock())
				{
					array.Add(text);
				}
			}
		}
		return array;
	}

	public Array GetCategory(string _category)
	{
		if (category.ContainsKey(_category))
		{
			return ((Array)category[_category]).Duplicate();
		}
		return new Array();
	}

	public Array GetPacketList()
	{
		Array array = new Array();
		foreach (Variant key in category.Keys)
		{
			string text = (string)key;
			array.AddRange((Array)category[text]);
		}
		return array;
	}

	public Array GetPlantList()
	{
		Array array = new Array();
		foreach (Variant key in category.Keys)
		{
			string text = (string)key;
			switch (text)
			{
			case "White":
			case "Gold":
			case "Diamond":
			case "Colour":
			case "Star":
			case "Original":
				array.AddRange((Array)category[text]);
				break;
			}
		}
		array.Remove("PlantLuckyBlover");
		return array;
	}

	public Array GetZombieList()
	{
		Array array = new Array();
		foreach (Variant key in category.Keys)
		{
			string text = (string)key;
			if (text == "Zombie")
			{
				array.AddRange((Array)category[text]);
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.GetUnlockPacket, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCategory, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlantList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetZombieList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetUnlockPacket && args.Count == 0)
		{
			Array<string> unlockPacket = GetUnlockPacket();
			ret = VariantUtils.CreateFromArray(unlockPacket);
			return true;
		}
		if (method == MethodName.GetCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Array>(GetCategory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetPacketList());
			return true;
		}
		if (method == MethodName.GetPlantList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetPlantList());
			return true;
		}
		if (method == MethodName.GetZombieList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetZombieList());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetUnlockPacket)
		{
			return true;
		}
		if (method == MethodName.GetCategory)
		{
			return true;
		}
		if (method == MethodName.GetPacketList)
		{
			return true;
		}
		if (method == MethodName.GetPlantList)
		{
			return true;
		}
		if (method == MethodName.GetZombieList)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.category)
		{
			category = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.category)
		{
			value = VariantUtils.CreateFrom(in category);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.category, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.category, Variant.From(in category));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.category, out var value))
		{
			category = value.As<Dictionary>();
		}
	}
}
