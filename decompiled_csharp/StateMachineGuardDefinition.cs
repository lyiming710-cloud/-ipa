using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/ResourceRuntime/StateMachineGuardDefinition.cs")]
public class StateMachineGuardDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Kind = "Kind";

		public static readonly StringName ComparedProperty = "ComparedProperty";

		public static readonly StringName Operator = "Operator";

		public static readonly StringName ExpectedValue = "ExpectedValue";

		public static readonly StringName Negate = "Negate";

		public static readonly StringName CallbackKey = "CallbackKey";

		public static readonly StringName Children = "Children";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StateMachineGuardKind Kind { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName ComparedProperty { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public StateMachineComparisonOperator Operator { get; set; }

	[Export(PropertyHint.None, "")]
	public Variant ExpectedValue { get; set; }

	[Export(PropertyHint.None, "")]
	public bool Negate { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName CallbackKey { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public Array<Resource> Children { get; set; } = new Array<Resource>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Kind)
		{
			Kind = VariantUtils.ConvertTo<StateMachineGuardKind>(in value);
			return true;
		}
		if (name == PropertyName.ComparedProperty)
		{
			ComparedProperty = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.Operator)
		{
			Operator = VariantUtils.ConvertTo<StateMachineComparisonOperator>(in value);
			return true;
		}
		if (name == PropertyName.ExpectedValue)
		{
			ExpectedValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.Negate)
		{
			Negate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CallbackKey)
		{
			CallbackKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.Children)
		{
			Children = VariantUtils.ConvertToArray<Resource>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Kind)
		{
			value = VariantUtils.CreateFrom<StateMachineGuardKind>(Kind);
			return true;
		}
		StringName from;
		if (name == PropertyName.ComparedProperty)
		{
			from = ComparedProperty;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Operator)
		{
			value = VariantUtils.CreateFrom<StateMachineComparisonOperator>(Operator);
			return true;
		}
		if (name == PropertyName.ExpectedValue)
		{
			value = VariantUtils.CreateFrom<Variant>(ExpectedValue);
			return true;
		}
		if (name == PropertyName.Negate)
		{
			value = VariantUtils.CreateFrom<bool>(Negate);
			return true;
		}
		if (name == PropertyName.CallbackKey)
		{
			from = CallbackKey;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Children)
		{
			value = VariantUtils.CreateFromArray(Children);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.Kind, PropertyHint.Enum, "ExpressionProperty,All,Any,Not,Callback", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ComparedProperty, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Operator, PropertyHint.Enum, "Equal,NotEqual,Less,LessOrEqual,Greater,GreaterOrEqual", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.ExpectedValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Negate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.CallbackKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Children, PropertyHint.TypeString, "24/17:Resource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Kind, Variant.From<StateMachineGuardKind>(Kind));
		info.AddProperty(PropertyName.ComparedProperty, Variant.From<StringName>(ComparedProperty));
		info.AddProperty(PropertyName.Operator, Variant.From<StateMachineComparisonOperator>(Operator));
		info.AddProperty(PropertyName.ExpectedValue, Variant.From<Variant>(ExpectedValue));
		info.AddProperty(PropertyName.Negate, Variant.From<bool>(Negate));
		info.AddProperty(PropertyName.CallbackKey, Variant.From<StringName>(CallbackKey));
		info.AddProperty(PropertyName.Children, Variant.CreateFrom(Children));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Kind, out var value))
		{
			Kind = value.As<StateMachineGuardKind>();
		}
		if (info.TryGetProperty(PropertyName.ComparedProperty, out var value2))
		{
			ComparedProperty = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Operator, out var value3))
		{
			Operator = value3.As<StateMachineComparisonOperator>();
		}
		if (info.TryGetProperty(PropertyName.ExpectedValue, out var value4))
		{
			ExpectedValue = value4.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.Negate, out var value5))
		{
			Negate = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CallbackKey, out var value6))
		{
			CallbackKey = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Children, out var value7))
		{
			Children = value7.AsGodotArray<Resource>();
		}
	}
}
