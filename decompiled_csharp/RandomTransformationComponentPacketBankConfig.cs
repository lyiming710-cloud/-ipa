using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/RandomTransformationComponent/Resource/RandomTransformationComponentPacketBankConfig.cs")]
public class RandomTransformationComponentPacketBankConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetPacketList = "GetPacketList";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName packetBankName = "packetBankName";

		public static readonly StringName categoryUseAll = "categoryUseAll";

		public static readonly StringName categoryUseList = "categoryUseList";

		public static readonly StringName rand = "rand";

		public static readonly StringName catagoryUseAll = "catagoryUseAll";

		public static readonly StringName catagoryUseList = "catagoryUseList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetBankName { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public bool categoryUseAll { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<string> categoryUseList { get; set; } = new Array<string>();

	[Export(PropertyHint.Range, "0,1,0.01")]
	public float rand { get; set; } = 1f;

	public bool catagoryUseAll
	{
		get
		{
			return categoryUseAll;
		}
		set
		{
			categoryUseAll = value;
		}
	}

	public Array<string> catagoryUseList
	{
		get
		{
			return categoryUseList;
		}
		set
		{
			categoryUseList = value ?? new Array<string>();
		}
	}

	public void FillPacketList(ICollection<string> output)
	{
		if (output == null || string.IsNullOrEmpty(packetBankName) || GD.Randf() > Mathf.Clamp(rand, 0f, 1f))
		{
			return;
		}
		TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData(packetBankName);
		if (!GodotObject.IsInstanceValid(packetBankData))
		{
			return;
		}
		if (categoryUseAll)
		{
			AppendValues(packetBankData.GetPacketList(), output);
		}
		else if (categoryUseList != null)
		{
			for (int i = 0; i < categoryUseList.Count; i++)
			{
				AppendValues(packetBankData.GetCategory(categoryUseList[i]), output);
			}
		}
	}

	private static void AppendValues(Array values, ICollection<string> output)
	{
		if (values == null)
		{
			return;
		}
		foreach (Variant value in values)
		{
			string text = value.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				output.Add(text);
			}
		}
	}

	public Array GetPacketList()
	{
		Array array = new Array();
		List<string> list = new List<string>();
		FillPacketList(list);
		for (int i = 0; i < list.Count; i++)
		{
			array.Add(list[i]);
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.GetPacketList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetPacketList && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetPacketList());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetPacketList)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetBankName)
		{
			packetBankName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.categoryUseAll)
		{
			categoryUseAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.categoryUseList)
		{
			categoryUseList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.rand)
		{
			rand = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.catagoryUseAll)
		{
			catagoryUseAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.catagoryUseList)
		{
			catagoryUseList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetBankName)
		{
			value = VariantUtils.CreateFrom<string>(packetBankName);
			return true;
		}
		bool from;
		if (name == PropertyName.categoryUseAll)
		{
			from = categoryUseAll;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.categoryUseList)
		{
			value = VariantUtils.CreateFromArray(categoryUseList);
			return true;
		}
		if (name == PropertyName.rand)
		{
			value = VariantUtils.CreateFrom<float>(rand);
			return true;
		}
		if (name == PropertyName.catagoryUseAll)
		{
			from = catagoryUseAll;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.catagoryUseList)
		{
			value = VariantUtils.CreateFromArray(catagoryUseList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.packetBankName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.categoryUseAll, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.categoryUseList, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rand, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.catagoryUseAll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.catagoryUseList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetBankName, Variant.From<string>(packetBankName));
		info.AddProperty(PropertyName.categoryUseAll, Variant.From<bool>(categoryUseAll));
		info.AddProperty(PropertyName.categoryUseList, Variant.CreateFrom(categoryUseList));
		info.AddProperty(PropertyName.rand, Variant.From<float>(rand));
		info.AddProperty(PropertyName.catagoryUseAll, Variant.From<bool>(catagoryUseAll));
		info.AddProperty(PropertyName.catagoryUseList, Variant.CreateFrom(catagoryUseList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetBankName, out var value))
		{
			packetBankName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.categoryUseAll, out var value2))
		{
			categoryUseAll = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.categoryUseList, out var value3))
		{
			categoryUseList = value3.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.rand, out var value4))
		{
			rand = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.catagoryUseAll, out var value5))
		{
			catagoryUseAll = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.catagoryUseList, out var value6))
		{
			catagoryUseList = value6.AsGodotArray<string>();
		}
	}
}
