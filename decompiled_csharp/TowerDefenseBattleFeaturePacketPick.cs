using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketPick/TowerDefenseBattleFeaturePacketPick.cs")]
public class TowerDefenseBattleFeaturePacketPick : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName AttachPacketPickControl = "AttachPacketPickControl";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName packetPickControl = "packetPickControl";

		public static readonly StringName config = "config";

		public static readonly StringName _mapFeature = "_mapFeature";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public PacketPickControl packetPickControl;

	public TowerDefenseBattleFeaturePacketPickConfig config;

	private TowerDefenseBattleFeatureMap _mapFeature;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseBattleFeaturePacketPickConfig();
		config.Init(data);
		packetPickControl = new PacketPickControl();
	}

	public override Task GameInit()
	{
		AttachPacketPickControl("GameInit");
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		AttachPacketPickControl("GameInitFromProgress");
		return Task.CompletedTask;
	}

	private void AttachPacketPickControl(string operation)
	{
		_mapFeature = GetFeature("Map") as TowerDefenseBattleFeatureMap;
		if (!GodotObject.IsInstanceValid(_mapFeature) || !GodotObject.IsInstanceValid(_mapFeature.mapControl) || !GodotObject.IsInstanceValid(packetPickControl))
		{
			GD.PushError("[PacketPick." + operation + "] Map, mapControl, or packetPickControl is invalid.");
			return;
		}
		_mapFeature.packetPickControl = packetPickControl;
		packetPickControl.Init(_mapFeature.mapControl, _mapFeature, config);
		if (packetPickControl.GetParent() == null)
		{
			_mapFeature.mapControl.AddChild(packetPickControl, forceReadableName: false, Node.InternalMode.Disabled);
		}
	}

	public override void Destroy()
	{
		if (GodotObject.IsInstanceValid(_mapFeature) && _mapFeature.packetPickControl == packetPickControl)
		{
			_mapFeature.packetPickControl = null;
		}
		if (GodotObject.IsInstanceValid(packetPickControl))
		{
			packetPickControl.DisposeBattleState();
			packetPickControl.QueueFree();
		}
		packetPickControl = null;
		config = null;
		_mapFeature = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttachPacketPickControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.AttachPacketPickControl && args.Count == 1)
		{
			AttachPacketPickControl(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
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
		if (method == MethodName.AttachPacketPickControl)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetPickControl)
		{
			packetPickControl = VariantUtils.ConvertTo<PacketPickControl>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleFeaturePacketPickConfig>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetPickControl)
		{
			value = VariantUtils.CreateFrom(in packetPickControl);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.packetPickControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetPickControl, Variant.From(in packetPickControl));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetPickControl, out var value))
		{
			packetPickControl = value.As<PacketPickControl>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseBattleFeaturePacketPickConfig>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value3))
		{
			_mapFeature = value3.As<TowerDefenseBattleFeatureMap>();
		}
	}
}
