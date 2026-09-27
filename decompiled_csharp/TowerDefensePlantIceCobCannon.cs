using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/IceCobCannon/Scene/TowerDefensePlantIceCobCannon.cs")]
public class TowerDefensePlantIceCobCannon : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName restTime = "restTime";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName _restTime = "_restTime";

		public static readonly StringName _skinName = "_skinName";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private CannonComponent _cannonComponent;

	private double _restTime = 30.0;

	private string _skinName = "Default";

	[Export(PropertyHint.None, "")]
	public double restTime
	{
		get
		{
			return _restTime;
		}
		set
		{
			_restTime = value;
			if (IsNodeReady() && _cannonComponent != null)
			{
				CannonComponent cannonComponent = _cannonComponent;
				if (cannonComponent != null && !cannonComponent.IsReleased)
				{
					_cannonComponent.restTime = (float)value;
				}
			}
		}
	}

	public string skinName
	{
		get
		{
			return _skinName;
		}
		set
		{
			_skinName = value;
			CannonComponent cannonComponent = _cannonComponent;
			if (cannonComponent != null && !cannonComponent.IsReleased && _cannonComponent.projectileData != null)
			{
				_cannonComponent.projectileData.skinName = value;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_cannonComponent = componentManager.GetRuntime<CannonComponent>();
			if (currentCustom.Contains("Custom0"))
			{
				skinName = "IceCream";
			}
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			skinName = "IceCream";
		}
		else
		{
			skinName = "Default";
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "fire")
		{
			AudioManager.Instance?.AudioPlay("CobLaunch");
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["restTime"] = restTime,
			["skinName"] = skinName
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		restTime = (data.ContainsKey("restTime") ? data["restTime"].AsDouble() : 30.0);
		skinName = (data.ContainsKey("skinName") ? data["skinName"].AsString() : "Default");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.OnCustomSwitched)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.restTime)
		{
			restTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._restTime)
		{
			_restTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			_skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.restTime)
		{
			value = VariantUtils.CreateFrom<double>(restTime);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			value = VariantUtils.CreateFrom<string>(skinName);
			return true;
		}
		if (name == PropertyName._restTime)
		{
			value = VariantUtils.CreateFrom(in _restTime);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			value = VariantUtils.CreateFrom(in _skinName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.restTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._restTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.restTime, Variant.From<double>(restTime));
		info.AddProperty(PropertyName.skinName, Variant.From<string>(skinName));
		info.AddProperty(PropertyName._restTime, Variant.From(in _restTime));
		info.AddProperty(PropertyName._skinName, Variant.From(in _skinName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.restTime, out var value))
		{
			restTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.skinName, out var value2))
		{
			skinName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._restTime, out var value3))
		{
			_restTime = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._skinName, out var value4))
		{
			_skinName = value4.As<string>();
		}
	}
}
