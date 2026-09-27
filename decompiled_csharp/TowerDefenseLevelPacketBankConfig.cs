using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/Resource/TowerDefenseLevelPacketBankConfig.cs")]
public class TowerDefenseLevelPacketBankConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName packetBankType = "packetBankType";

		public static readonly StringName categoryBatchSize = "categoryBatchSize";

		public static readonly StringName maxPoolSize = "maxPoolSize";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetBankType = "GeneralPlant";

	[Export(PropertyHint.None, "")]
	public int categoryBatchSize = 256;

	[Export(PropertyHint.None, "")]
	public int maxPoolSize = 96;

	public void Init(Dictionary data)
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = TowerDefenseManager.Instance?.currentLevelConfig as TowerDefenseLevelConfig;
		string text = (GodotObject.IsInstanceValid(towerDefenseLevelConfig) ? towerDefenseLevelConfig.packetBank : packetBankType);
		packetBankType = data.GetValueOrDefault("PacketBankName", text).AsString();
		packetBankType = (string.IsNullOrEmpty(packetBankType) ? "GeneralPlant" : packetBankType);
		categoryBatchSize = Math.Max(1, data.GetValueOrDefault("CategoryBatchSize", 256).AsInt32());
		maxPoolSize = Math.Max(0, data.GetValueOrDefault("MaxPoolSize", 96).AsInt32());
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetBankType)
		{
			packetBankType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.categoryBatchSize)
		{
			categoryBatchSize = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maxPoolSize)
		{
			maxPoolSize = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetBankType)
		{
			value = VariantUtils.CreateFrom(in packetBankType);
			return true;
		}
		if (name == PropertyName.categoryBatchSize)
		{
			value = VariantUtils.CreateFrom(in categoryBatchSize);
			return true;
		}
		if (name == PropertyName.maxPoolSize)
		{
			value = VariantUtils.CreateFrom(in maxPoolSize);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.packetBankType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.categoryBatchSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxPoolSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetBankType, Variant.From(in packetBankType));
		info.AddProperty(PropertyName.categoryBatchSize, Variant.From(in categoryBatchSize));
		info.AddProperty(PropertyName.maxPoolSize, Variant.From(in maxPoolSize));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetBankType, out var value))
		{
			packetBankType = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.categoryBatchSize, out var value2))
		{
			categoryBatchSize = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maxPoolSize, out var value3))
		{
			maxPoolSize = value3.As<int>();
		}
	}
}
