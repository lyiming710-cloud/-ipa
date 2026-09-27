using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/UnlockCondition/UnlockConditionPacketBankCategoryPacketUnlockNumConfig.cs")]
public class UnlockConditionPacketBankCategoryPacketUnlockNumConfig : UnlockConditionBaseConfig
{
	public new class MethodName : UnlockConditionBaseConfig.MethodName
	{
		public new static readonly StringName Check = "Check";
	}

	public new class PropertyName : UnlockConditionBaseConfig.PropertyName
	{
		public static readonly StringName packetBankName = "packetBankName";

		public static readonly StringName category = "category";

		public static readonly StringName num = "num";
	}

	public new class SignalName : UnlockConditionBaseConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetBankName = "";

	[Export(PropertyHint.None, "")]
	public string category = "";

	[Export(PropertyHint.None, "")]
	public int num = 1;

	public override bool Check()
	{
		TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData(packetBankName);
		int num = 0;
		if (GodotObject.IsInstanceValid(packetBankData))
		{
			foreach (Variant item in packetBankData.GetCategory(category))
			{
				string key = (string)item;
				if (GameSaveManager.Instance.GetTowerDefensePacketValue(key).GetValueOrDefault("Unlock", false).AsBool())
				{
					num++;
				}
			}
		}
		return num >= this.num;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Check && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Check());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Check)
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
		if (name == PropertyName.category)
		{
			category = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetBankName)
		{
			value = VariantUtils.CreateFrom(in packetBankName);
			return true;
		}
		if (name == PropertyName.category)
		{
			value = VariantUtils.CreateFrom(in category);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
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
			new PropertyInfo(Variant.Type.String, PropertyName.category, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetBankName, Variant.From(in packetBankName));
		info.AddProperty(PropertyName.category, Variant.From(in category));
		info.AddProperty(PropertyName.num, Variant.From(in num));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetBankName, out var value))
		{
			packetBankName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.category, out var value2))
		{
			category = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value3))
		{
			num = value3.As<int>();
		}
	}
}
