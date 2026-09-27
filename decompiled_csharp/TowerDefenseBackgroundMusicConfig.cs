using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/BGM/Resource/TowerDefenseBackgroundMusicConfig.cs")]
public class TowerDefenseBackgroundMusicConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName translate = "translate";

		public static readonly StringName entry = "entry";

		public static readonly StringName flag1 = "flag1";

		public static readonly StringName drums = "drums";

		public static readonly StringName win = "win";

		public static readonly StringName drumsZombieThreshold = "drumsZombieThreshold";

		public static readonly StringName drumsFadeSpeed = "drumsFadeSpeed";

		public static readonly StringName drumsCheckInterval = "drumsCheckInterval";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string translate = "";

	[Export(PropertyHint.None, "")]
	public string entry;

	[Export(PropertyHint.None, "")]
	public string flag1;

	[Export(PropertyHint.None, "")]
	public string drums;

	[Export(PropertyHint.None, "")]
	public string win;

	[Export(PropertyHint.Range, "0,100,1")]
	public int drumsZombieThreshold = 10;

	[Export(PropertyHint.Range, "0.01,20.0,0.01")]
	public float drumsFadeSpeed = 1f;

	[Export(PropertyHint.Range, "0.05,5.0,0.05")]
	public float drumsCheckInterval = 0.25f;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.translate)
		{
			translate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.entry)
		{
			entry = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.flag1)
		{
			flag1 = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.drums)
		{
			drums = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.win)
		{
			win = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.drumsZombieThreshold)
		{
			drumsZombieThreshold = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.drumsFadeSpeed)
		{
			drumsFadeSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.drumsCheckInterval)
		{
			drumsCheckInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.translate)
		{
			value = VariantUtils.CreateFrom(in translate);
			return true;
		}
		if (name == PropertyName.entry)
		{
			value = VariantUtils.CreateFrom(in entry);
			return true;
		}
		if (name == PropertyName.flag1)
		{
			value = VariantUtils.CreateFrom(in flag1);
			return true;
		}
		if (name == PropertyName.drums)
		{
			value = VariantUtils.CreateFrom(in drums);
			return true;
		}
		if (name == PropertyName.win)
		{
			value = VariantUtils.CreateFrom(in win);
			return true;
		}
		if (name == PropertyName.drumsZombieThreshold)
		{
			value = VariantUtils.CreateFrom(in drumsZombieThreshold);
			return true;
		}
		if (name == PropertyName.drumsFadeSpeed)
		{
			value = VariantUtils.CreateFrom(in drumsFadeSpeed);
			return true;
		}
		if (name == PropertyName.drumsCheckInterval)
		{
			value = VariantUtils.CreateFrom(in drumsCheckInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.translate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.entry, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.flag1, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.drums, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.win, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.drumsZombieThreshold, PropertyHint.Range, "0,100,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.drumsFadeSpeed, PropertyHint.Range, "0.01,20.0,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.drumsCheckInterval, PropertyHint.Range, "0.05,5.0,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.translate, Variant.From(in translate));
		info.AddProperty(PropertyName.entry, Variant.From(in entry));
		info.AddProperty(PropertyName.flag1, Variant.From(in flag1));
		info.AddProperty(PropertyName.drums, Variant.From(in drums));
		info.AddProperty(PropertyName.win, Variant.From(in win));
		info.AddProperty(PropertyName.drumsZombieThreshold, Variant.From(in drumsZombieThreshold));
		info.AddProperty(PropertyName.drumsFadeSpeed, Variant.From(in drumsFadeSpeed));
		info.AddProperty(PropertyName.drumsCheckInterval, Variant.From(in drumsCheckInterval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.translate, out var value))
		{
			translate = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.entry, out var value2))
		{
			entry = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.flag1, out var value3))
		{
			flag1 = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.drums, out var value4))
		{
			drums = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.win, out var value5))
		{
			win = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.drumsZombieThreshold, out var value6))
		{
			drumsZombieThreshold = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.drumsFadeSpeed, out var value7))
		{
			drumsFadeSpeed = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.drumsCheckInterval, out var value8))
		{
			drumsCheckInterval = value8.As<float>();
		}
	}
}
