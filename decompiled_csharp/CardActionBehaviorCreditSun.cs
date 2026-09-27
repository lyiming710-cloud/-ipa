using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/Action/CardActionBehaviorCreditSun.cs")]
public class CardActionBehaviorCreditSun : CardActionBehaviorDefinition
{
	public new class MethodName : CardActionBehaviorDefinition.MethodName
	{
		public new static readonly StringName ImportConfiguration = "ImportConfiguration";

		public new static readonly StringName ExecuteAction = "ExecuteAction";

		public new static readonly StringName ExportConfiguration = "ExportConfiguration";
	}

	public new class PropertyName : CardActionBehaviorDefinition.PropertyName
	{
		public static readonly StringName amount = "amount";
	}

	public new class SignalName : CardActionBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public long amount;

	public override void ImportConfiguration(Dictionary data)
	{
		amount = data.GetValueOrDefault("Amount", 0L).AsInt64();
	}

	public override void ExecuteAction(TowerDefenseInGamePacketShow packet)
	{
		if (amount > 0 && GodotObject.IsInstanceValid(packet) && packet.HasSunAccount)
		{
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.CreditSun(packet.SunAccountId, amount);
			}
		}
	}

	public override Dictionary ExportConfiguration()
	{
		return new Dictionary
		{
			["ActionId"] = "CreditSun",
			["Configuration"] = new Dictionary { ["Amount"] = amount }
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.ImportConfiguration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportConfiguration, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ImportConfiguration && args.Count == 1)
		{
			ImportConfiguration(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteAction && args.Count == 1)
		{
			ExecuteAction(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportConfiguration && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportConfiguration());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ImportConfiguration)
		{
			return true;
		}
		if (method == MethodName.ExecuteAction)
		{
			return true;
		}
		if (method == MethodName.ExportConfiguration)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.amount)
		{
			amount = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.amount)
		{
			value = VariantUtils.CreateFrom(in amount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.amount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.amount, Variant.From(in amount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.amount, out var value))
		{
			amount = value.As<long>();
		}
	}
}
