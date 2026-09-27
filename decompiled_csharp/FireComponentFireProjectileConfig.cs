using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/Resource/FireComponentFireProjectileConfig.cs")]
public class FireComponentFireProjectileConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName checkProjectileId = "checkProjectileId";

		public static readonly StringName firePosId = "firePosId";

		public static readonly StringName speed = "speed";

		public static readonly StringName dir = "dir";

		public static readonly StringName offsetLine = "offsetLine";

		public static readonly StringName fireNumSkip = "fireNumSkip";

		public static readonly StringName fireEventNeed = "fireEventNeed";

		public static readonly StringName projectileFlip = "projectileFlip";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int checkProjectileId;

	[Export(PropertyHint.None, "")]
	public int firePosId;

	[Export(PropertyHint.None, "")]
	public float speed = 300f;

	[Export(PropertyHint.None, "")]
	public float dir;

	[Export(PropertyHint.None, "")]
	public int offsetLine;

	[Export(PropertyHint.None, "")]
	public int fireNumSkip = -1;

	[Export(PropertyHint.None, "")]
	public string fireEventNeed = "";

	[Export(PropertyHint.None, "")]
	public bool projectileFlip;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.checkProjectileId)
		{
			checkProjectileId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.firePosId)
		{
			firePosId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dir)
		{
			dir = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.offsetLine)
		{
			offsetLine = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireNumSkip)
		{
			fireNumSkip = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireEventNeed)
		{
			fireEventNeed = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.projectileFlip)
		{
			projectileFlip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.checkProjectileId)
		{
			value = VariantUtils.CreateFrom(in checkProjectileId);
			return true;
		}
		if (name == PropertyName.firePosId)
		{
			value = VariantUtils.CreateFrom(in firePosId);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.dir)
		{
			value = VariantUtils.CreateFrom(in dir);
			return true;
		}
		if (name == PropertyName.offsetLine)
		{
			value = VariantUtils.CreateFrom(in offsetLine);
			return true;
		}
		if (name == PropertyName.fireNumSkip)
		{
			value = VariantUtils.CreateFrom(in fireNumSkip);
			return true;
		}
		if (name == PropertyName.fireEventNeed)
		{
			value = VariantUtils.CreateFrom(in fireEventNeed);
			return true;
		}
		if (name == PropertyName.projectileFlip)
		{
			value = VariantUtils.CreateFrom(in projectileFlip);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.checkProjectileId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.firePosId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dir, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.offsetLine, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNumSkip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireEventNeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.projectileFlip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.checkProjectileId, Variant.From(in checkProjectileId));
		info.AddProperty(PropertyName.firePosId, Variant.From(in firePosId));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.dir, Variant.From(in dir));
		info.AddProperty(PropertyName.offsetLine, Variant.From(in offsetLine));
		info.AddProperty(PropertyName.fireNumSkip, Variant.From(in fireNumSkip));
		info.AddProperty(PropertyName.fireEventNeed, Variant.From(in fireEventNeed));
		info.AddProperty(PropertyName.projectileFlip, Variant.From(in projectileFlip));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.checkProjectileId, out var value))
		{
			checkProjectileId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.firePosId, out var value2))
		{
			firePosId = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value3))
		{
			speed = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dir, out var value4))
		{
			dir = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.offsetLine, out var value5))
		{
			offsetLine = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireNumSkip, out var value6))
		{
			fireNumSkip = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireEventNeed, out var value7))
		{
			fireEventNeed = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.projectileFlip, out var value8))
		{
			projectileFlip = value8.As<bool>();
		}
	}
}
