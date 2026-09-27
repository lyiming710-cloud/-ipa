using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Protal/TowerDefenseLevelEventCreateProtal.cs")]
public class TowerDefenseLevelEventCreateProtal : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";

		public static readonly StringName GetPortalFeature = "GetPortalFeature";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName protalShape = "protalShape";

		public static readonly StringName posRange = "posRange";

		public static readonly StringName changeTime = "changeTime";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string protalShape = "Circle";

	[Export(PropertyHint.None, "")]
	public Vector4I posRange = new Vector4I(3, 1, 9, 5);

	[Export(PropertyHint.None, "")]
	public double changeTime;

	public override string _GetName()
	{
		return "LEVLE_EVENT_CREATE_PROTAL";
	}

	public override void Execute()
	{
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return;
		}
		TowerDefenseBattleFeaturePortal portalFeature = GetPortalFeature();
		if (portalFeature == null)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				currentControl.AddFeature("Portal", new Dictionary());
				portalFeature = GetPortalFeature();
			}
		}
		if (GodotObject.IsInstanceValid(portalFeature))
		{
			portalFeature.PortalCreate(protalShape, posRange, changeTime);
		}
	}

	public override void Init(Dictionary valueDictionary)
	{
		protalShape = valueDictionary.GetValueOrDefault("Shape", "Circle").AsString();
		Dictionary dictionary = (valueDictionary.ContainsKey("PosRange") ? valueDictionary["PosRange"].AsGodotDictionary() : new Dictionary());
		posRange = new Vector4I(dictionary.GetValueOrDefault("x", -1).AsInt32(), dictionary.GetValueOrDefault("y", -1).AsInt32(), dictionary.GetValueOrDefault("z", -1).AsInt32(), dictionary.GetValueOrDefault("w", -1).AsInt32());
		changeTime = valueDictionary.GetValueOrDefault("ChangeTime", 0.0).AsDouble();
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "CreateProtal",
			["Value"] = new Dictionary
			{
				["Shape"] = protalShape,
				["PosRange"] = new Dictionary
				{
					["x"] = posRange.X,
					["y"] = posRange.Y,
					["z"] = posRange.Z,
					["w"] = posRange.W
				},
				["ChangeTime"] = changeTime
			}
		};
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["改变地图"] = new Dictionary
		{
			["地图"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Enum",
				["Property"] = "protalShape",
				["Hint"] = new Dictionary
				{
					["Circle"] = "Circle",
					["Square"] = "Square",
					["Rhombus"] = "Rhombus"
				},
				["Rest"] = "Circle"
			},
			["范围"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Vector4i",
				["Property"] = "posRange",
				["Rest"] = new Vector4I(3, 1, 9, 5)
			},
			["位置改变时间"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Float",
				["Property"] = "changeTime",
				["Rest"] = 0.0
			}
		};
		return property;
	}

	public TowerDefenseBattleFeaturePortal GetPortalFeature()
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (GodotObject.IsInstanceValid(currentControl))
		{
			return currentControl.GetFeature("Portal") as TowerDefenseBattleFeaturePortal;
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPortalFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName.Execute && args.Count == 0)
		{
			Execute();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.GetProperty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetProperty());
			return true;
		}
		if (method == MethodName.GetPortalFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeaturePortal>(GetPortalFeature());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.GetProperty)
		{
			return true;
		}
		if (method == MethodName.GetPortalFeature)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.protalShape)
		{
			protalShape = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.posRange)
		{
			posRange = VariantUtils.ConvertTo<Vector4I>(in value);
			return true;
		}
		if (name == PropertyName.changeTime)
		{
			changeTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.protalShape)
		{
			value = VariantUtils.CreateFrom(in protalShape);
			return true;
		}
		if (name == PropertyName.posRange)
		{
			value = VariantUtils.CreateFrom(in posRange);
			return true;
		}
		if (name == PropertyName.changeTime)
		{
			value = VariantUtils.CreateFrom(in changeTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.protalShape, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector4I, PropertyName.posRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.changeTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.protalShape, Variant.From(in protalShape));
		info.AddProperty(PropertyName.posRange, Variant.From(in posRange));
		info.AddProperty(PropertyName.changeTime, Variant.From(in changeTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.protalShape, out var value))
		{
			protalShape = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.posRange, out var value2))
		{
			posRange = value2.As<Vector4I>();
		}
		if (info.TryGetProperty(PropertyName.changeTime, out var value3))
		{
			changeTime = value3.As<double>();
		}
	}
}
