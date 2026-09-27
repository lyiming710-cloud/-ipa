using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/ResourceRuntime/StateMachineDefinition.cs")]
public class StateMachineDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName SchemaVersion = "SchemaVersion";

		public static readonly StringName DefinitionId = "DefinitionId";

		public static readonly StringName BaseDefinition = "BaseDefinition";

		public static readonly StringName RootStateId = "RootStateId";

		public static readonly StringName States = "States";

		public static readonly StringName Transitions = "Transitions";

		public static readonly StringName Aliases = "Aliases";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int SchemaVersion { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public string DefinitionId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public StateMachineDefinition BaseDefinition { get; set; }

	[Export(PropertyHint.None, "")]
	public string RootStateId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public Array<StateMachineStateDefinition> States { get; set; } = new Array<StateMachineStateDefinition>();

	[Export(PropertyHint.None, "")]
	public Array<StateMachineTransitionDefinition> Transitions { get; set; } = new Array<StateMachineTransitionDefinition>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, string> Aliases { get; set; } = new Godot.Collections.Dictionary<string, string>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SchemaVersion)
		{
			SchemaVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DefinitionId)
		{
			DefinitionId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.BaseDefinition)
		{
			BaseDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName.RootStateId)
		{
			RootStateId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.States)
		{
			States = VariantUtils.ConvertToArray<StateMachineStateDefinition>(in value);
			return true;
		}
		if (name == PropertyName.Transitions)
		{
			Transitions = VariantUtils.ConvertToArray<StateMachineTransitionDefinition>(in value);
			return true;
		}
		if (name == PropertyName.Aliases)
		{
			Aliases = VariantUtils.ConvertToDictionary<string, string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.SchemaVersion)
		{
			value = VariantUtils.CreateFrom<int>(SchemaVersion);
			return true;
		}
		string from;
		if (name == PropertyName.DefinitionId)
		{
			from = DefinitionId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BaseDefinition)
		{
			value = VariantUtils.CreateFrom<StateMachineDefinition>(BaseDefinition);
			return true;
		}
		if (name == PropertyName.RootStateId)
		{
			from = RootStateId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.States)
		{
			value = VariantUtils.CreateFromArray(States);
			return true;
		}
		if (name == PropertyName.Transitions)
		{
			value = VariantUtils.CreateFromArray(Transitions);
			return true;
		}
		if (name == PropertyName.Aliases)
		{
			value = VariantUtils.CreateFromDictionary(Aliases);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.SchemaVersion, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.DefinitionId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.BaseDefinition, PropertyHint.ResourceType, "StateMachineDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.RootStateId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.States, PropertyHint.TypeString, "24/17:StateMachineStateDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Transitions, PropertyHint.TypeString, "24/17:StateMachineTransitionDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.Aliases, PropertyHint.TypeString, "4/0:;4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SchemaVersion, Variant.From<int>(SchemaVersion));
		info.AddProperty(PropertyName.DefinitionId, Variant.From<string>(DefinitionId));
		info.AddProperty(PropertyName.BaseDefinition, Variant.From<StateMachineDefinition>(BaseDefinition));
		info.AddProperty(PropertyName.RootStateId, Variant.From<string>(RootStateId));
		info.AddProperty(PropertyName.States, Variant.CreateFrom(States));
		info.AddProperty(PropertyName.Transitions, Variant.CreateFrom(Transitions));
		info.AddProperty(PropertyName.Aliases, Variant.CreateFrom(Aliases));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SchemaVersion, out var value))
		{
			SchemaVersion = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DefinitionId, out var value2))
		{
			DefinitionId = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.BaseDefinition, out var value3))
		{
			BaseDefinition = value3.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName.RootStateId, out var value4))
		{
			RootStateId = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.States, out var value5))
		{
			States = value5.AsGodotArray<StateMachineStateDefinition>();
		}
		if (info.TryGetProperty(PropertyName.Transitions, out var value6))
		{
			Transitions = value6.AsGodotArray<StateMachineTransitionDefinition>();
		}
		if (info.TryGetProperty(PropertyName.Aliases, out var value7))
		{
			Aliases = value7.AsGodotDictionary<string, string>();
		}
	}
}
