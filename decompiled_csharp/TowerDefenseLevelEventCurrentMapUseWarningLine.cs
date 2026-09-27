using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Map/TowerDefenseLevelEventCurrentMapUseWarningLine.cs")]
public class TowerDefenseLevelEventCurrentMapUseWarningLine : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName ExecuteWithWarningLineFeature = "ExecuteWithWarningLineFeature";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";

		public static readonly StringName GetWarningLineFeature = "GetWarningLineFeature";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName column = "column";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int column = 5;

	public override string _GetName()
	{
		return "LEVLE_EVENT_CURRENTMAP_USE_WARNINGLINE";
	}

	public override void Execute()
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance.GetCurrentMap()))
		{
			TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
			if (mapFeature != null)
			{
				int capturedColumn = column;
				mapFeature.TryEnqueuePendingMapAction(() =>
				{
					ExecuteWithWarningLineFeature(capturedColumn);
				});
			}
		}
		else
		{
			ExecuteWithWarningLineFeature(column);
		}
	}

	private void ExecuteWithWarningLineFeature(int columnValue)
	{
		TowerDefenseBattleFeatureWarningLine warningLineFeature = GetWarningLineFeature();
		if (warningLineFeature == null)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				currentControl.AddFeature("WarningLine", new Dictionary());
				warningLineFeature = GetWarningLineFeature();
			}
		}
		if (GodotObject.IsInstanceValid(warningLineFeature))
		{
			warningLineFeature.AddWarningColumn(columnValue);
		}
	}

	public override void Init(Dictionary valueDictionary)
	{
		column = valueDictionary.GetValueOrDefault("Column", valueDictionary.GetValueOrDefault("Row", 5)).AsInt32();
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "CurrentMapUseWarningLine",
			["Value"] = new Dictionary
			{
				["Column"] = column,
				["Row"] = column
			}
		};
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["使用警戒线"] = new Dictionary { ["列"] = new Dictionary
		{
			["Object"] = this,
			["Type"] = "Int",
			["Property"] = "column",
			["Rest"] = 5
		} };
		return property;
	}

	public TowerDefenseBattleFeatureWarningLine GetWarningLineFeature()
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (GodotObject.IsInstanceValid(currentControl))
		{
			return currentControl.GetFeature("WarningLine") as TowerDefenseBattleFeatureWarningLine;
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteWithWarningLineFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "columnValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetWarningLineFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ExecuteWithWarningLineFeature && args.Count == 1)
		{
			ExecuteWithWarningLineFeature(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.GetWarningLineFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureWarningLine>(GetWarningLineFeature());
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
		if (method == MethodName.ExecuteWithWarningLineFeature)
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
		if (method == MethodName.GetWarningLineFeature)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.column)
		{
			column = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.column)
		{
			value = VariantUtils.CreateFrom(in column);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.column, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.column, Variant.From(in column));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.column, out var value))
		{
			column = value.As<int>();
		}
	}
}
