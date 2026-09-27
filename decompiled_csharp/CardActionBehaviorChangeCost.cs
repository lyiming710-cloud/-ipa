using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/Action/CardActionBehaviorChangeCost.cs")]
public class CardActionBehaviorChangeCost : CardActionBehaviorDefinition
{
	public enum METHOD
	{
		ADD,
		MULTIPLY
	}

	public new class MethodName : CardActionBehaviorDefinition.MethodName
	{
		public new static readonly StringName ImportConfiguration = "ImportConfiguration";

		public new static readonly StringName ExecuteAction = "ExecuteAction";

		public new static readonly StringName ExportConfiguration = "ExportConfiguration";
	}

	public new class PropertyName : CardActionBehaviorDefinition.PropertyName
	{
		public static readonly StringName method = "method";

		public static readonly StringName value = "value";

		public static readonly StringName _min = "_min";

		public static readonly StringName _max = "_max";
	}

	public new class SignalName : CardActionBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public METHOD method;

	[Export(PropertyHint.None, "")]
	public double value;

	[Export(PropertyHint.None, "")]
	public int _min = -1;

	[Export(PropertyHint.None, "")]
	public int _max = -1;

	public override void ImportConfiguration(Dictionary data)
	{
		if (Enum.TryParse<METHOD>(data.GetValueOrDefault("Method", "ADD").AsString().ToUpper(), out var result))
		{
			method = result;
		}
		value = data.GetValueOrDefault("Value", 0.0).AsDouble();
		_min = data.GetValueOrDefault("Min", -1).AsInt32();
		_max = data.GetValueOrDefault("Max", -1).AsInt32();
	}

	public override void ExecuteAction(TowerDefenseInGamePacketShow packet)
	{
		switch (method)
		{
		case METHOD.ADD:
			packet.baseItemCost += (long)value;
			break;
		case METHOD.MULTIPLY:
			packet.baseItemCost = (long)Mathf.Floor((double)packet.baseItemCost * value);
			break;
		}
		if (_min != -1 && packet.baseItemCost < _min)
		{
			packet.baseItemCost = _min;
		}
		if (_max != -1 && packet.baseItemCost > _max)
		{
			packet.baseItemCost = _max;
		}
	}

	public override Dictionary ExportConfiguration()
	{
		return new Dictionary
		{
			["ActionId"] = "ChangeCost",
			["Configuration"] = new Dictionary
			{
				["Method"] = Enum.GetName(typeof(METHOD), method),
				["Value"] = value,
				["Min"] = _min,
				["Max"] = _max
			}
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
		if (name == PropertyName.method)
		{
			method = VariantUtils.ConvertTo<METHOD>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._min)
		{
			_min = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._max)
		{
			_max = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.method)
		{
			value = VariantUtils.CreateFrom(in method);
			return true;
		}
		if (name == PropertyName.value)
		{
			value = VariantUtils.CreateFrom(in this.value);
			return true;
		}
		if (name == PropertyName._min)
		{
			value = VariantUtils.CreateFrom(in _min);
			return true;
		}
		if (name == PropertyName._max)
		{
			value = VariantUtils.CreateFrom(in _max);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.method, PropertyHint.Enum, "ADD,MULTIPLY", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._min, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._max, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.method, Variant.From(in method));
		info.AddProperty(PropertyName.value, Variant.From(in value));
		info.AddProperty(PropertyName._min, Variant.From(in _min));
		info.AddProperty(PropertyName._max, Variant.From(in _max));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.method, out var variant))
		{
			method = variant.As<METHOD>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant2))
		{
			value = variant2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._min, out var variant3))
		{
			_min = variant3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._max, out var variant4))
		{
			_max = variant4.As<int>();
		}
	}
}
