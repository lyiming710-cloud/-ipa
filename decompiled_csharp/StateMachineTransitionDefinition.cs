using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/ResourceRuntime/StateMachineTransitionDefinition.cs")]
public class StateMachineTransitionDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName StableId = "StableId";

		public static readonly StringName SourceStateId = "SourceStateId";

		public static readonly StringName TargetStateId = "TargetStateId";

		public static readonly StringName TriggerKind = "TriggerKind";

		public static readonly StringName EventName = "EventName";

		public static readonly StringName DelaySeconds = "DelaySeconds";

		public static readonly StringName Priority = "Priority";

		public static readonly StringName DeclarationOrder = "DeclarationOrder";

		public static readonly StringName GuardDefinition = "GuardDefinition";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string StableId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string SourceStateId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string TargetStateId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public StateMachineTriggerKind TriggerKind { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName EventName { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public double DelaySeconds { get; set; }

	[Export(PropertyHint.None, "")]
	public int Priority { get; set; }

	[Export(PropertyHint.None, "")]
	public int DeclarationOrder { get; set; }

	[Export(PropertyHint.None, "")]
	public Resource GuardDefinition { get; set; }

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.StableId)
		{
			StableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SourceStateId)
		{
			SourceStateId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.TargetStateId)
		{
			TargetStateId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.TriggerKind)
		{
			TriggerKind = VariantUtils.ConvertTo<StateMachineTriggerKind>(in value);
			return true;
		}
		if (name == PropertyName.EventName)
		{
			EventName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.DelaySeconds)
		{
			DelaySeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.Priority)
		{
			Priority = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DeclarationOrder)
		{
			DeclarationOrder = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.GuardDefinition)
		{
			GuardDefinition = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.StableId)
		{
			from = StableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SourceStateId)
		{
			from = SourceStateId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TargetStateId)
		{
			from = TargetStateId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TriggerKind)
		{
			value = VariantUtils.CreateFrom<StateMachineTriggerKind>(TriggerKind);
			return true;
		}
		if (name == PropertyName.EventName)
		{
			value = VariantUtils.CreateFrom<StringName>(EventName);
			return true;
		}
		if (name == PropertyName.DelaySeconds)
		{
			value = VariantUtils.CreateFrom<double>(DelaySeconds);
			return true;
		}
		int from2;
		if (name == PropertyName.Priority)
		{
			from2 = Priority;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DeclarationOrder)
		{
			from2 = DeclarationOrder;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.GuardDefinition)
		{
			value = VariantUtils.CreateFrom<Resource>(GuardDefinition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.StableId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.SourceStateId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.TargetStateId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.TriggerKind, PropertyHint.Enum, "Event,Automatic,Delay", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.EventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.DelaySeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Priority, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.DeclarationOrder, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.GuardDefinition, PropertyHint.ResourceType, "Resource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.StableId, Variant.From<string>(StableId));
		info.AddProperty(PropertyName.SourceStateId, Variant.From<string>(SourceStateId));
		info.AddProperty(PropertyName.TargetStateId, Variant.From<string>(TargetStateId));
		info.AddProperty(PropertyName.TriggerKind, Variant.From<StateMachineTriggerKind>(TriggerKind));
		info.AddProperty(PropertyName.EventName, Variant.From<StringName>(EventName));
		info.AddProperty(PropertyName.DelaySeconds, Variant.From<double>(DelaySeconds));
		info.AddProperty(PropertyName.Priority, Variant.From<int>(Priority));
		info.AddProperty(PropertyName.DeclarationOrder, Variant.From<int>(DeclarationOrder));
		info.AddProperty(PropertyName.GuardDefinition, Variant.From<Resource>(GuardDefinition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.StableId, out var value))
		{
			StableId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SourceStateId, out var value2))
		{
			SourceStateId = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.TargetStateId, out var value3))
		{
			TargetStateId = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.TriggerKind, out var value4))
		{
			TriggerKind = value4.As<StateMachineTriggerKind>();
		}
		if (info.TryGetProperty(PropertyName.EventName, out var value5))
		{
			EventName = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.DelaySeconds, out var value6))
		{
			DelaySeconds = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.Priority, out var value7))
		{
			Priority = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DeclarationOrder, out var value8))
		{
			DeclarationOrder = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.GuardDefinition, out var value9))
		{
			GuardDefinition = value9.As<Resource>();
		}
	}
}
