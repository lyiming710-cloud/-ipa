using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/Action/CardActionBehaviorChangePacket.cs")]
public class CardActionBehaviorChangePacket : CardActionBehaviorDefinition
{
	public new class MethodName : CardActionBehaviorDefinition.MethodName
	{
		public new static readonly StringName ImportConfiguration = "ImportConfiguration";

		public new static readonly StringName ExecuteAction = "ExecuteAction";

		public new static readonly StringName ExportConfiguration = "ExportConfiguration";
	}

	public new class PropertyName : CardActionBehaviorDefinition.PropertyName
	{
		public static readonly StringName levelPacketConfig = "levelPacketConfig";

		public static readonly StringName packetConfig = "packetConfig";

		public static readonly StringName count = "count";
	}

	public new class SignalName : CardActionBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelPacketConfig levelPacketConfig;

	public TowerDefensePacketConfig packetConfig;

	[Export(PropertyHint.None, "")]
	public int count = 1;

	public override void ImportConfiguration(Dictionary data)
	{
		levelPacketConfig = new TowerDefenseLevelPacketConfig();
		levelPacketConfig.Init(data);
		count = data.GetValueOrDefault("Count", 1).AsInt32();
	}

	public override void ExecuteAction(TowerDefenseInGamePacketShow packet)
	{
		if (count > 1)
		{
			count--;
			return;
		}
		if (!GodotObject.IsInstanceValid(packetConfig) && GodotObject.IsInstanceValid(levelPacketConfig))
		{
			packetConfig = levelPacketConfig.GetPacket();
		}
		if (GodotObject.IsInstanceValid(packetConfig))
		{
			packet.Init(packetConfig);
		}
	}

	public override Dictionary ExportConfiguration()
	{
		Dictionary dictionary = (GodotObject.IsInstanceValid(levelPacketConfig) ? levelPacketConfig.Export() : new Dictionary());
		if (count > 1)
		{
			dictionary["Count"] = count;
		}
		return new Dictionary
		{
			["ActionId"] = "ChangePacket",
			["Configuration"] = dictionary
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
		if (name == PropertyName.levelPacketConfig)
		{
			levelPacketConfig = VariantUtils.ConvertTo<TowerDefenseLevelPacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.packetConfig)
		{
			packetConfig = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.count)
		{
			count = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.levelPacketConfig)
		{
			value = VariantUtils.CreateFrom(in levelPacketConfig);
			return true;
		}
		if (name == PropertyName.packetConfig)
		{
			value = VariantUtils.CreateFrom(in packetConfig);
			return true;
		}
		if (name == PropertyName.count)
		{
			value = VariantUtils.CreateFrom(in count);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.levelPacketConfig, PropertyHint.ResourceType, "TowerDefenseLevelPacketConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.count, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.levelPacketConfig, Variant.From(in levelPacketConfig));
		info.AddProperty(PropertyName.packetConfig, Variant.From(in packetConfig));
		info.AddProperty(PropertyName.count, Variant.From(in count));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.levelPacketConfig, out var value))
		{
			levelPacketConfig = value.As<TowerDefenseLevelPacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.packetConfig, out var value2))
		{
			packetConfig = value2.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.count, out var value3))
		{
			count = value3.As<int>();
		}
	}
}
