using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/LookStar/TowerDefenseLevelLookStarManagerConfig.cs")]
public class TowerDefenseLevelLookStarManagerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName open = "open";

		public static readonly StringName checkInterval = "checkInterval";

		public static readonly StringName previewOpacity = "previewOpacity";

		public static readonly StringName previewLayer = "previewLayer";

		public static readonly StringName checkList = "checkList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool open;

	[Export(PropertyHint.Range, "0.05,10.0,0.05")]
	public double checkInterval = 0.5;

	[Export(PropertyHint.Range, "0.0,1.0,0.01")]
	public float previewOpacity = 0.5f;

	[Export(PropertyHint.None, "")]
	public int previewLayer = 2;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelLookStarCheckConfig> checkList = new Array<TowerDefenseLevelLookStarCheckConfig>();

	public void Init(Dictionary lookStarManagerData)
	{
		open = lookStarManagerData.GetValueOrDefault("Open", false).AsBool();
		checkInterval = Math.Max(0.05, lookStarManagerData.GetValueOrDefault("CheckInterval", 0.5).AsDouble());
		previewOpacity = Mathf.Clamp((float)lookStarManagerData.GetValueOrDefault("PreviewOpacity", 0.5).AsDouble(), 0f, 1f);
		previewLayer = lookStarManagerData.GetValueOrDefault("PreviewLayer", 2).AsInt32();
		checkList.Clear();
		foreach (Variant item in lookStarManagerData.ContainsKey("Check") ? ((Godot.Collections.Array)lookStarManagerData["Check"]) : new Godot.Collections.Array())
		{
			Dictionary data = item.AsGodotDictionary();
			TowerDefenseLevelLookStarCheckConfig towerDefenseLevelLookStarCheckConfig = new TowerDefenseLevelLookStarCheckConfig();
			towerDefenseLevelLookStarCheckConfig.Init(data);
			checkList.Add(towerDefenseLevelLookStarCheckConfig);
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Open"] = open,
			["CheckInterval"] = checkInterval,
			["PreviewOpacity"] = previewOpacity,
			["PreviewLayer"] = previewLayer,
			["Check"] = new Godot.Collections.Array()
		};
		foreach (TowerDefenseLevelLookStarCheckConfig check in checkList)
		{
			((Godot.Collections.Array)dictionary["Check"]).Add(check.Export());
		}
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "lookStarManagerData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
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
		if (method == MethodName.Export)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.open)
		{
			open = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkInterval)
		{
			checkInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.previewOpacity)
		{
			previewOpacity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.previewLayer)
		{
			previewLayer = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.checkList)
		{
			checkList = VariantUtils.ConvertToArray<TowerDefenseLevelLookStarCheckConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.open)
		{
			value = VariantUtils.CreateFrom(in open);
			return true;
		}
		if (name == PropertyName.checkInterval)
		{
			value = VariantUtils.CreateFrom(in checkInterval);
			return true;
		}
		if (name == PropertyName.previewOpacity)
		{
			value = VariantUtils.CreateFrom(in previewOpacity);
			return true;
		}
		if (name == PropertyName.previewLayer)
		{
			value = VariantUtils.CreateFrom(in previewLayer);
			return true;
		}
		if (name == PropertyName.checkList)
		{
			value = VariantUtils.CreateFromArray(checkList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.open, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.checkInterval, PropertyHint.Range, "0.05,10.0,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.previewOpacity, PropertyHint.Range, "0.0,1.0,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.previewLayer, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.checkList, PropertyHint.TypeString, "24/17:TowerDefenseLevelLookStarCheckConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.open, Variant.From(in open));
		info.AddProperty(PropertyName.checkInterval, Variant.From(in checkInterval));
		info.AddProperty(PropertyName.previewOpacity, Variant.From(in previewOpacity));
		info.AddProperty(PropertyName.previewLayer, Variant.From(in previewLayer));
		info.AddProperty(PropertyName.checkList, Variant.CreateFrom(checkList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.open, out var value))
		{
			open = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkInterval, out var value2))
		{
			checkInterval = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.previewOpacity, out var value3))
		{
			previewOpacity = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.previewLayer, out var value4))
		{
			previewLayer = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.checkList, out var value5))
		{
			checkList = value5.AsGodotArray<TowerDefenseLevelLookStarCheckConfig>();
		}
	}
}
