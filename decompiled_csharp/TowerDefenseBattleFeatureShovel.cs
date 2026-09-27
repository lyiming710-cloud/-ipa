using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/TowerDefenseBattleFeatureShovel.cs")]
public class TowerDefenseBattleFeatureShovel : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName BindMapFeature = "BindMapFeature";

		public static readonly StringName RegisterShovelTool = "RegisterShovelTool";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName shovelManager = "shovelManager";

		public static readonly StringName shovelPickTool = "shovelPickTool";

		public static readonly StringName _mapFeature = "_mapFeature";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _shovelManager;

	public ShovelManager shovelManager;

	public ShovelPickTool shovelPickTool;

	private TowerDefenseBattleFeatureMap _mapFeature;

	private static PackedScene SHOVEL_MANAGER => _shovelManager ?? (_shovelManager = GD.Load<PackedScene>("res://Registry/Battle/Feature/Shovel/ShovelManager/ShovelManager.tscn"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		shovelManager = SHOVEL_MANAGER.Instantiate<ShovelManager>(PackedScene.GenEditState.Disabled);
		control.AddUIToTopPropContainer(shovelManager);
	}

	public override Task GameInit()
	{
		BindMapFeature();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		BindMapFeature();
		return Task.CompletedTask;
	}

	public override Task GameEntry()
	{
		RegisterShovelTool();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			shovelManager.ShovelReset();
		}
		if (GodotObject.IsInstanceValid(shovelPickTool))
		{
			return Task.CompletedTask;
		}
		RegisterShovelTool();
		return Task.CompletedTask;
	}

	private void BindMapFeature()
	{
		_mapFeature = GetFeature("Map") as TowerDefenseBattleFeatureMap;
		if (GodotObject.IsInstanceValid(_mapFeature) && GodotObject.IsInstanceValid(_mapFeature.mapControl) && GodotObject.IsInstanceValid(shovelManager))
		{
			_mapFeature.shovelManager = shovelManager;
			shovelManager.Init(_mapFeature.mapControl, _mapFeature);
		}
	}

	private void RegisterShovelTool()
	{
		if (_mapFeature == null)
		{
			_mapFeature = GetFeature("Map") as TowerDefenseBattleFeatureMap;
		}
		if (!GodotObject.IsInstanceValid(shovelPickTool) && GodotObject.IsInstanceValid(_mapFeature) && GodotObject.IsInstanceValid(_mapFeature.mapControl) && GodotObject.IsInstanceValid(_mapFeature.packetPickControl))
		{
			shovelPickTool = new ShovelPickTool();
			shovelPickTool.Init(_mapFeature.mapControl);
			shovelPickTool.SetShovelManager(shovelManager);
			_mapFeature.packetPickControl.RegisterTool(shovelPickTool);
		}
	}

	public override void Destroy()
	{
		if (GodotObject.IsInstanceValid(_mapFeature) && GodotObject.IsInstanceValid(_mapFeature.packetPickControl) && GodotObject.IsInstanceValid(shovelPickTool))
		{
			_mapFeature.packetPickControl.UnregisterTool(shovelPickTool);
		}
		if (GodotObject.IsInstanceValid(shovelPickTool))
		{
			shovelPickTool.SetShovelManager(null);
			shovelPickTool.Free();
		}
		shovelPickTool = null;
		if (GodotObject.IsInstanceValid(_mapFeature) && _mapFeature.shovelManager == shovelManager)
		{
			_mapFeature.shovelManager = null;
		}
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			shovelManager.QueueFree();
		}
		shovelManager = null;
		_mapFeature = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindMapFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterShovelTool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.BindMapFeature && args.Count == 0)
		{
			BindMapFeature();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterShovelTool && args.Count == 0)
		{
			RegisterShovelTool();
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
		if (method == MethodName.BindMapFeature)
		{
			return true;
		}
		if (method == MethodName.RegisterShovelTool)
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
		if (name == PropertyName.shovelManager)
		{
			shovelManager = VariantUtils.ConvertTo<ShovelManager>(in value);
			return true;
		}
		if (name == PropertyName.shovelPickTool)
		{
			shovelPickTool = VariantUtils.ConvertTo<ShovelPickTool>(in value);
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
		if (name == PropertyName.shovelManager)
		{
			value = VariantUtils.CreateFrom(in shovelManager);
			return true;
		}
		if (name == PropertyName.shovelPickTool)
		{
			value = VariantUtils.CreateFrom(in shovelPickTool);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelPickTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shovelManager, Variant.From(in shovelManager));
		info.AddProperty(PropertyName.shovelPickTool, Variant.From(in shovelPickTool));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shovelManager, out var value))
		{
			shovelManager = value.As<ShovelManager>();
		}
		if (info.TryGetProperty(PropertyName.shovelPickTool, out var value2))
		{
			shovelPickTool = value2.As<ShovelPickTool>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value3))
		{
			_mapFeature = value3.As<TowerDefenseBattleFeatureMap>();
		}
	}
}
