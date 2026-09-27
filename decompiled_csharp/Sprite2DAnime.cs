using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Extends/Sprite2D/Sprite2DAnime.cs")]
public class Sprite2DAnime : Sprite2D
{
	public new class MethodName : Sprite2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";
	}

	public new class PropertyName : Sprite2D.PropertyName
	{
		public static readonly StringName isPlaying = "isPlaying";

		public static readonly StringName fps = "fps";

		public static readonly StringName _frameNum = "_frameNum";

		public static readonly StringName _timer = "_timer";
	}

	public new class SignalName : Sprite2D.SignalName
	{
	}

	private int _frameNum;

	private double _timer;

	[Export(PropertyHint.None, "")]
	public bool isPlaying { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public double fps { get; set; } = 24.0;

	public override void _Ready()
	{
		_frameNum = Hframes * Vframes;
	}

	public override void _Process(double delta)
	{
		if (isPlaying)
		{
			if (Engine.IsEditorHint())
			{
				_frameNum = Hframes * Vframes;
			}
			if (_timer < 1.0 / fps)
			{
				_timer += delta;
				return;
			}
			_timer = 0.0;
			Frame = (Frame + 1) % _frameNum;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.isPlaying)
		{
			isPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fps)
		{
			fps = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._frameNum)
		{
			_frameNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._timer)
		{
			_timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.isPlaying)
		{
			value = VariantUtils.CreateFrom<bool>(isPlaying);
			return true;
		}
		if (name == PropertyName.fps)
		{
			value = VariantUtils.CreateFrom<double>(fps);
			return true;
		}
		if (name == PropertyName._frameNum)
		{
			value = VariantUtils.CreateFrom(in _frameNum);
			return true;
		}
		if (name == PropertyName._timer)
		{
			value = VariantUtils.CreateFrom(in _timer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPlaying, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.isPlaying, Variant.From<bool>(isPlaying));
		info.AddProperty(PropertyName.fps, Variant.From<double>(fps));
		info.AddProperty(PropertyName._frameNum, Variant.From(in _frameNum));
		info.AddProperty(PropertyName._timer, Variant.From(in _timer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.isPlaying, out var value))
		{
			isPlaying = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fps, out var value2))
		{
			fps = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._frameNum, out var value3))
		{
			_frameNum = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._timer, out var value4))
		{
			_timer = value4.As<double>();
		}
	}
}
