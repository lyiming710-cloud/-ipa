using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Command/CommandConfig.cs")]
public class CommandConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Name = "Name";

		public static readonly StringName Description = "Description";

		public static readonly StringName Usage = "Usage";

		public static readonly StringName Callback = "Callback";

		public static readonly StringName ArgsInfo = "ArgsInfo";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public string Name;

	public string Description;

	public string Usage;

	public Callable Callback;

	public Array<CommandArg> ArgsInfo = new Array<CommandArg>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Description)
		{
			Description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Usage)
		{
			Usage = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Callback)
		{
			Callback = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName.ArgsInfo)
		{
			ArgsInfo = VariantUtils.ConvertToArray<CommandArg>(in value);
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
		if (name == PropertyName.Description)
		{
			value = VariantUtils.CreateFrom(in Description);
			return true;
		}
		if (name == PropertyName.Usage)
		{
			value = VariantUtils.CreateFrom(in Usage);
			return true;
		}
		if (name == PropertyName.Callback)
		{
			value = VariantUtils.CreateFrom(in Callback);
			return true;
		}
		if (name == PropertyName.ArgsInfo)
		{
			value = VariantUtils.CreateFromArray(ArgsInfo);
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
			new PropertyInfo(Variant.Type.String, PropertyName.Description, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Usage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName.Callback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.ArgsInfo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Name, Variant.From(in Name));
		info.AddProperty(PropertyName.Description, Variant.From(in Description));
		info.AddProperty(PropertyName.Usage, Variant.From(in Usage));
		info.AddProperty(PropertyName.Callback, Variant.From(in Callback));
		info.AddProperty(PropertyName.ArgsInfo, Variant.CreateFrom(ArgsInfo));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Name, out var value))
		{
			Name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Description, out var value2))
		{
			Description = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Usage, out var value3))
		{
			Usage = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Callback, out var value4))
		{
			Callback = value4.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName.ArgsInfo, out var value5))
		{
			ArgsInfo = value5.AsGodotArray<CommandArg>();
		}
	}
}
