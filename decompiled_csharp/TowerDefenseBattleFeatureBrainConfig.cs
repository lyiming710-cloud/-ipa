using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Brain/Resource/TowerDefenseBattleFeatureBrainConfig.cs")]
public class TowerDefenseBattleFeatureBrainConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName packetName = "packetName";

		public static readonly StringName horizontalOffset = "horizontalOffset";

		public static readonly StringName characterFilter = "characterFilter";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetName = "ItemBrain";

	[Export(PropertyHint.Range, "-200,200,1")]
	public double horizontalOffset = 10.0;

	[Export(PropertyHint.None, "")]
	public bool characterFilter = true;

	public void Init(Dictionary data)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		packetName = data.GetValueOrDefault("PacketName", "ItemBrain").AsString();
		if (string.IsNullOrWhiteSpace(packetName))
		{
			packetName = "ItemBrain";
		}
		double num = data.GetValueOrDefault("HorizontalOffset", 10.0).AsDouble();
		horizontalOffset = (double.IsFinite(num) ? Math.Clamp(num, -200.0, 200.0) : 10.0);
		characterFilter = data.GetValueOrDefault("CharacterFilter", true).AsBool();
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["PacketName"] = packetName,
			["HorizontalOffset"] = horizontalOffset,
			["CharacterFilter"] = characterFilter
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.packetName)
		{
			packetName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.horizontalOffset)
		{
			horizontalOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.characterFilter)
		{
			characterFilter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetName)
		{
			value = VariantUtils.CreateFrom(in packetName);
			return true;
		}
		if (name == PropertyName.horizontalOffset)
		{
			value = VariantUtils.CreateFrom(in horizontalOffset);
			return true;
		}
		if (name == PropertyName.characterFilter)
		{
			value = VariantUtils.CreateFrom(in characterFilter);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.packetName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.horizontalOffset, PropertyHint.Range, "-200,200,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.characterFilter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.horizontalOffset, Variant.From(in horizontalOffset));
		info.AddProperty(PropertyName.characterFilter, Variant.From(in characterFilter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetName, out var value))
		{
			packetName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.horizontalOffset, out var value2))
		{
			horizontalOffset = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.characterFilter, out var value3))
		{
			characterFilter = value3.As<bool>();
		}
	}
}
