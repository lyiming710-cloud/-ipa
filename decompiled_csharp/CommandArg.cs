using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Command/CommandArg.cs")]
public class CommandArg : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Name = "Name";

		public static readonly StringName Type = "Type";

		public static readonly StringName Required = "Required";

		public static readonly StringName DefaultValue = "DefaultValue";

		public static readonly StringName Description = "Description";

		public static readonly StringName Suggestions = "Suggestions";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public string Name;

	public int Type = 4;

	public bool Required = true;

	public Variant DefaultValue;

	public string Description = "";

	public Callable Suggestions;

	public CommandArg()
	{
	}

	public CommandArg(string _name, int _type = 4, bool _required = true, Variant _defaultValue = default(Variant), string _description = "", Callable _suggestions = default(Callable))
	{
		Name = _name;
		Type = _type;
		Required = _required;
		DefaultValue = _defaultValue;
		Description = _description;
		Suggestions = _suggestions;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Type)
		{
			Type = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Required)
		{
			Required = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			DefaultValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.Description)
		{
			Description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Suggestions)
		{
			Suggestions = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom(in Name);
			return true;
		}
		if (name == PropertyName.Type)
		{
			value = VariantUtils.CreateFrom(in Type);
			return true;
		}
		if (name == PropertyName.Required)
		{
			value = VariantUtils.CreateFrom(in Required);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			value = VariantUtils.CreateFrom(in DefaultValue);
			return true;
		}
		if (name == PropertyName.Description)
		{
			value = VariantUtils.CreateFrom(in Description);
			return true;
		}
		if (name == PropertyName.Suggestions)
		{
			value = VariantUtils.CreateFrom(in Suggestions);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Required, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.DefaultValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName.Suggestions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Name, Variant.From(in Name));
		info.AddProperty(PropertyName.Type, Variant.From(in Type));
		info.AddProperty(PropertyName.Required, Variant.From(in Required));
		info.AddProperty(PropertyName.DefaultValue, Variant.From(in DefaultValue));
		info.AddProperty(PropertyName.Description, Variant.From(in Description));
		info.AddProperty(PropertyName.Suggestions, Variant.From(in Suggestions));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Name, out var value))
		{
			Name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Type, out var value2))
		{
			Type = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Required, out var value3))
		{
			Required = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValue, out var value4))
		{
			DefaultValue = value4.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.Description, out var value5))
		{
			Description = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Suggestions, out var value6))
		{
			Suggestions = value6.As<Callable>();
		}
	}
}
