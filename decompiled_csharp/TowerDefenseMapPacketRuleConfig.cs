using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Resource/Rule/TowerDefenseMapPacketRuleConfig.cs")]
public class TowerDefenseMapPacketRuleConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Matches = "Matches";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName packetType = "packetType";

		public static readonly StringName cooldownMultiplier = "cooldownMultiplier";

		public static readonly StringName ignoreDynamicCostGrowth = "ignoreDynamicCostGrowth";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.PACKET_TYPE packetType = TowerDefenseEnum.PACKET_TYPE.NOONE;

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public double cooldownMultiplier = 1.0;

	[Export(PropertyHint.None, "")]
	public bool ignoreDynamicCostGrowth;

	public bool Matches(TowerDefenseEnum.PACKET_TYPE candidateType)
	{
		if (packetType != TowerDefenseEnum.PACKET_TYPE.NOONE)
		{
			return packetType == candidateType;
		}
		return true;
	}

	public bool TryValidateRuntime(out string reason)
	{
		if (!double.IsFinite(cooldownMultiplier) || cooldownMultiplier < 0.0)
		{
			reason = $"packet cooldown multiplier must be finite and non-negative, got {cooldownMultiplier}";
			return false;
		}
		reason = "";
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Matches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "candidateType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Matches && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Matches(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Matches)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetType)
		{
			packetType = VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in value);
			return true;
		}
		if (name == PropertyName.cooldownMultiplier)
		{
			cooldownMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ignoreDynamicCostGrowth)
		{
			ignoreDynamicCostGrowth = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetType)
		{
			value = VariantUtils.CreateFrom(in packetType);
			return true;
		}
		if (name == PropertyName.cooldownMultiplier)
		{
			value = VariantUtils.CreateFrom(in cooldownMultiplier);
			return true;
		}
		if (name == PropertyName.ignoreDynamicCostGrowth)
		{
			value = VariantUtils.CreateFrom(in ignoreDynamicCostGrowth);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.packetType, PropertyHint.Enum, "NOONE:-1,WHITE:0,GOLD:1,DIAMOND:2,COLOUR:3,STAR:4,ORIGINAL:5,ZOMBIE:6,COVER:7,GRAY:8", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cooldownMultiplier, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreDynamicCostGrowth, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetType, Variant.From(in packetType));
		info.AddProperty(PropertyName.cooldownMultiplier, Variant.From(in cooldownMultiplier));
		info.AddProperty(PropertyName.ignoreDynamicCostGrowth, Variant.From(in ignoreDynamicCostGrowth));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetType, out var value))
		{
			packetType = value.As<TowerDefenseEnum.PACKET_TYPE>();
		}
		if (info.TryGetProperty(PropertyName.cooldownMultiplier, out var value2))
		{
			cooldownMultiplier = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ignoreDynamicCostGrowth, out var value3))
		{
			ignoreDynamicCostGrowth = value3.As<bool>();
		}
	}
}
