using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/TowerDefenseLevelBaseConfig.cs")]
public class TowerDefenseLevelBaseConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName name = "name";

		public static readonly StringName description = "description";

		public static readonly StringName levelName = "levelName";

		public static readonly StringName levelNumber = "levelNumber";

		public static readonly StringName nextLevel = "nextLevel";

		public static readonly StringName homeWorld = "homeWorld";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string name = "";

	[Export(PropertyHint.None, "")]
	public string description = "关卡描述";

	[Export(PropertyHint.None, "")]
	public string levelName = "关卡名";

	[Export(PropertyHint.None, "")]
	public int levelNumber = 1;

	[Export(PropertyHint.None, "")]
	public string nextLevel = "";

	[Export(PropertyHint.None, "")]
	public GeneralEnum.HOMEWORLD homeWorld;

	public virtual void Init()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.description)
		{
			description = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.levelName)
		{
			levelName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.levelNumber)
		{
			levelNumber = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.nextLevel)
		{
			nextLevel = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.homeWorld)
		{
			homeWorld = VariantUtils.ConvertTo<GeneralEnum.HOMEWORLD>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.description)
		{
			value = VariantUtils.CreateFrom(in description);
			return true;
		}
		if (name == PropertyName.levelName)
		{
			value = VariantUtils.CreateFrom(in levelName);
			return true;
		}
		if (name == PropertyName.levelNumber)
		{
			value = VariantUtils.CreateFrom(in levelNumber);
			return true;
		}
		if (name == PropertyName.nextLevel)
		{
			value = VariantUtils.CreateFrom(in nextLevel);
			return true;
		}
		if (name == PropertyName.homeWorld)
		{
			value = VariantUtils.CreateFrom(in homeWorld);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.description, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.levelName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.levelNumber, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.nextLevel, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.homeWorld, PropertyHint.Enum, "NOONE,MORDEN", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.description, Variant.From(in description));
		info.AddProperty(PropertyName.levelName, Variant.From(in levelName));
		info.AddProperty(PropertyName.levelNumber, Variant.From(in levelNumber));
		info.AddProperty(PropertyName.nextLevel, Variant.From(in nextLevel));
		info.AddProperty(PropertyName.homeWorld, Variant.From(in homeWorld));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.name, out var value))
		{
			name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.description, out var value2))
		{
			description = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelName, out var value3))
		{
			levelName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelNumber, out var value4))
		{
			levelNumber = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.nextLevel, out var value5))
		{
			nextLevel = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.homeWorld, out var value6))
		{
			homeWorld = value6.As<GeneralEnum.HOMEWORLD>();
		}
	}
}
