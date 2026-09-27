using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/Runtime/CharacterComponentDefinition.cs")]
public abstract class CharacterComponentDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ComponentTypeId = "ComponentTypeId";

		public static readonly StringName DefinitionId = "DefinitionId";

		public static readonly StringName InstanceId = "InstanceId";

		public static readonly StringName WireIndex = "WireIndex";

		public static readonly StringName SchemaVersion = "SchemaVersion";

		public static readonly StringName InitiallyAlive = "InitiallyAlive";

		public static readonly StringName StateMachineDefinition = "StateMachineDefinition";

		public static readonly StringName LegacyNodeNames = "LegacyNodeNames";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string ComponentTypeId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string DefinitionId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string InstanceId { get; set; } = string.Empty;

	[Export(PropertyHint.Range, "-1,128,1")]
	public int WireIndex { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public int SchemaVersion { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public bool InitiallyAlive { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public StateMachineDefinition StateMachineDefinition { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<StringName> LegacyNodeNames { get; set; } = new Array<StringName>();

	public abstract CharacterComponentRuntime CreateRuntime();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ComponentTypeId)
		{
			ComponentTypeId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DefinitionId)
		{
			DefinitionId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.InstanceId)
		{
			InstanceId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.WireIndex)
		{
			WireIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SchemaVersion)
		{
			SchemaVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.InitiallyAlive)
		{
			InitiallyAlive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.StateMachineDefinition)
		{
			StateMachineDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName.LegacyNodeNames)
		{
			LegacyNodeNames = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ComponentTypeId)
		{
			from = ComponentTypeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DefinitionId)
		{
			from = DefinitionId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InstanceId)
		{
			from = InstanceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.WireIndex)
		{
			from2 = WireIndex;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SchemaVersion)
		{
			from2 = SchemaVersion;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.InitiallyAlive)
		{
			value = VariantUtils.CreateFrom<bool>(InitiallyAlive);
			return true;
		}
		if (name == PropertyName.StateMachineDefinition)
		{
			value = VariantUtils.CreateFrom<StateMachineDefinition>(StateMachineDefinition);
			return true;
		}
		if (name == PropertyName.LegacyNodeNames)
		{
			value = VariantUtils.CreateFromArray(LegacyNodeNames);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ComponentTypeId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.DefinitionId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.InstanceId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.WireIndex, PropertyHint.Range, "-1,128,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SchemaVersion, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.InitiallyAlive, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.StateMachineDefinition, PropertyHint.ResourceType, "StateMachineDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.LegacyNodeNames, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ComponentTypeId, Variant.From<string>(ComponentTypeId));
		info.AddProperty(PropertyName.DefinitionId, Variant.From<string>(DefinitionId));
		info.AddProperty(PropertyName.InstanceId, Variant.From<string>(InstanceId));
		info.AddProperty(PropertyName.WireIndex, Variant.From<int>(WireIndex));
		info.AddProperty(PropertyName.SchemaVersion, Variant.From<int>(SchemaVersion));
		info.AddProperty(PropertyName.InitiallyAlive, Variant.From<bool>(InitiallyAlive));
		info.AddProperty(PropertyName.StateMachineDefinition, Variant.From<StateMachineDefinition>(StateMachineDefinition));
		info.AddProperty(PropertyName.LegacyNodeNames, Variant.CreateFrom(LegacyNodeNames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ComponentTypeId, out var value))
		{
			ComponentTypeId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DefinitionId, out var value2))
		{
			DefinitionId = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.InstanceId, out var value3))
		{
			InstanceId = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.WireIndex, out var value4))
		{
			WireIndex = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SchemaVersion, out var value5))
		{
			SchemaVersion = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.InitiallyAlive, out var value6))
		{
			InitiallyAlive = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineDefinition, out var value7))
		{
			StateMachineDefinition = value7.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName.LegacyNodeNames, out var value8))
		{
			LegacyNodeNames = value8.AsGodotArray<StringName>();
		}
	}
}
