using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/AudioManager/AudioStreamPlayerMember.cs")]
public class AudioStreamPlayerMember : AudioStreamPlayer
{
	public new class MethodName : AudioStreamPlayer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName VolumRefresh = "VolumRefresh";
	}

	public new class PropertyName : AudioStreamPlayer.PropertyName
	{
		public static readonly StringName volumeScale = "volumeScale";

		public static readonly StringName type = "type";

		public static readonly StringName _volumeScale = "_volumeScale";
	}

	public new class SignalName : AudioStreamPlayer.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public AudioManagerEnum.TYPE type = AudioManagerEnum.TYPE.SFX;

	private double _volumeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double volumeScale
	{
		get
		{
			return _volumeScale;
		}
		set
		{
			_volumeScale = value;
			VolumRefresh();
		}
	}

	public override void _Ready()
	{
		VolumRefresh();
	}

	public void VolumRefresh()
	{
		VolumeDb = (float)Mathf.LinearToDb(volumeScale);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VolumRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.VolumRefresh && args.Count == 0)
		{
			VolumRefresh();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.VolumRefresh)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.volumeScale)
		{
			volumeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<AudioManagerEnum.TYPE>(in value);
			return true;
		}
		if (name == PropertyName._volumeScale)
		{
			_volumeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.volumeScale)
		{
			value = VariantUtils.CreateFrom<double>(volumeScale);
			return true;
		}
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName._volumeScale)
		{
			value = VariantUtils.CreateFrom(in _volumeScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.type, PropertyHint.Enum, "MUSIC,SFX", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._volumeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.volumeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.volumeScale, Variant.From<double>(volumeScale));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName._volumeScale, Variant.From(in _volumeScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.volumeScale, out var value))
		{
			volumeScale = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value2))
		{
			type = value2.As<AudioManagerEnum.TYPE>();
		}
		if (info.TryGetProperty(PropertyName._volumeScale, out var value3))
		{
			_volumeScale = value3.As<double>();
		}
	}
}
