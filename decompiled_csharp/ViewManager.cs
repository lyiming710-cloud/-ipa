using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/ViewManager/ViewManager.cs")]
public class ViewManager : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FullScreenColorBlink = "FullScreenColorBlink";

		public static readonly StringName FinishFullScreenColorBlink = "FinishFullScreenColorBlink";

		public static readonly StringName CameraShake = "CameraShake";

		public static readonly StringName RunCameraShake = "RunCameraShake";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _fullScreenColorRect = "_fullScreenColorRect";

		public static readonly StringName _fullScreenBlinkTween = "_fullScreenBlinkTween";

		public static readonly StringName _cameraShakeRunning = "_cameraShakeRunning";

		public static readonly StringName _cameraShakeDirection = "_cameraShakeDirection";

		public static readonly StringName _cameraShakeForce = "_cameraShakeForce";

		public static readonly StringName _cameraShakeInterval = "_cameraShakeInterval";

		public static readonly StringName _cameraShakeStepsRemaining = "_cameraShakeStepsRemaining";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public static ViewManager Instance;

	private ColorRect _fullScreenColorRect;

	private Tween _fullScreenBlinkTween;

	private bool _cameraShakeRunning;

	private Vector2 _cameraShakeDirection;

	private double _cameraShakeForce;

	private double _cameraShakeInterval;

	private int _cameraShakeStepsRemaining;

	public override void _Ready()
	{
		Instance = this;
		_fullScreenColorRect = GetNode<ColorRect>("%FullScreenColorRect");
	}

	public void FullScreenColorBlink(Color color, double duration = 0.2, bool rise = true)
	{
		if (GodotObject.IsInstanceValid(_fullScreenColorRect))
		{
			duration = Math.Max(0.0, duration);
			if (GodotObject.IsInstanceValid(_fullScreenBlinkTween))
			{
				_fullScreenBlinkTween.Kill();
			}
			_fullScreenColorRect.Visible = true;
			_fullScreenColorRect.Color = color;
			_fullScreenBlinkTween = CreateTween();
			if (rise)
			{
				_fullScreenBlinkTween.TweenProperty(_fullScreenColorRect, "modulate:a", color.A, duration / 2.0).From(0f);
				_fullScreenBlinkTween.TweenProperty(_fullScreenColorRect, "modulate:a", 0f, duration / 2.0).From(color.A);
			}
			else
			{
				_fullScreenBlinkTween.TweenProperty(_fullScreenColorRect, "modulate:a", 0f, duration).From(color.A);
			}
			_fullScreenBlinkTween.TweenCallback(Callable.From(FinishFullScreenColorBlink));
		}
	}

	private void FinishFullScreenColorBlink()
	{
		if (GodotObject.IsInstanceValid(_fullScreenColorRect))
		{
			_fullScreenColorRect.Visible = false;
		}
		_fullScreenBlinkTween = null;
	}

	public void CameraShake(Vector2 dir, double force, double interval = 0.05, int time = 1)
	{
		force = Math.Max(0.0, force);
		interval = Math.Max(0.0, interval);
		time = Math.Max(0, time);
		if (!(force <= 0.0) && time != 0)
		{
			if (!dir.IsZeroApprox())
			{
				_cameraShakeDirection = dir.Normalized();
			}
			_cameraShakeForce = Math.Max(_cameraShakeForce, force);
			_cameraShakeInterval = (_cameraShakeRunning ? Math.Min(_cameraShakeInterval, interval) : interval);
			_cameraShakeStepsRemaining = Math.Max(_cameraShakeStepsRemaining, time);
			if (!_cameraShakeRunning)
			{
				_cameraShakeRunning = true;
				Callable.From(RunCameraShake).CallDeferred();
			}
		}
	}

	private async void RunCameraShake()
	{
		try
		{
			bool firstStep = true;
			while (_cameraShakeStepsRemaining > 0 && IsInsideTree())
			{
				Camera2D camera2D = GetViewport().GetCamera2D();
				if (GodotObject.IsInstanceValid(camera2D))
				{
					camera2D.Offset = (firstStep ? (_cameraShakeDirection * (float)_cameraShakeForce) : (-camera2D.Offset / 2f));
					firstStep = false;
					_cameraShakeStepsRemaining--;
					double cameraShakeInterval = _cameraShakeInterval;
					await ToSignal(GetTree().CreateTimer(cameraShakeInterval, processAlways: false), SceneTreeTimer.SignalName.Timeout);
					continue;
				}
				break;
			}
		}
		finally
		{
			Camera2D camera2D2 = (IsInsideTree() ? GetViewport().GetCamera2D() : null);
			if (GodotObject.IsInstanceValid(camera2D2))
			{
				camera2D2.Offset = Vector2.Zero;
			}
			_cameraShakeRunning = false;
			_cameraShakeForce = 0.0;
			_cameraShakeInterval = 0.0;
			_cameraShakeStepsRemaining = 0;
		}
	}

	public ViewManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/ViewManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FullScreenColorBlink, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rise", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishFullScreenColorBlink, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CameraShake, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "interval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunCameraShake, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.FullScreenColorBlink && args.Count == 3)
		{
			FullScreenColorBlink(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishFullScreenColorBlink && args.Count == 0)
		{
			FinishFullScreenColorBlink();
			ret = default;
			return true;
		}
		if (method == MethodName.CameraShake && args.Count == 4)
		{
			CameraShake(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunCameraShake && args.Count == 0)
		{
			RunCameraShake();
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
		if (method == MethodName.FullScreenColorBlink)
		{
			return true;
		}
		if (method == MethodName.FinishFullScreenColorBlink)
		{
			return true;
		}
		if (method == MethodName.CameraShake)
		{
			return true;
		}
		if (method == MethodName.RunCameraShake)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._fullScreenColorRect)
		{
			_fullScreenColorRect = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName._fullScreenBlinkTween)
		{
			_fullScreenBlinkTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._cameraShakeRunning)
		{
			_cameraShakeRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cameraShakeDirection)
		{
			_cameraShakeDirection = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._cameraShakeForce)
		{
			_cameraShakeForce = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cameraShakeInterval)
		{
			_cameraShakeInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cameraShakeStepsRemaining)
		{
			_cameraShakeStepsRemaining = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._fullScreenColorRect)
		{
			value = VariantUtils.CreateFrom(in _fullScreenColorRect);
			return true;
		}
		if (name == PropertyName._fullScreenBlinkTween)
		{
			value = VariantUtils.CreateFrom(in _fullScreenBlinkTween);
			return true;
		}
		if (name == PropertyName._cameraShakeRunning)
		{
			value = VariantUtils.CreateFrom(in _cameraShakeRunning);
			return true;
		}
		if (name == PropertyName._cameraShakeDirection)
		{
			value = VariantUtils.CreateFrom(in _cameraShakeDirection);
			return true;
		}
		if (name == PropertyName._cameraShakeForce)
		{
			value = VariantUtils.CreateFrom(in _cameraShakeForce);
			return true;
		}
		if (name == PropertyName._cameraShakeInterval)
		{
			value = VariantUtils.CreateFrom(in _cameraShakeInterval);
			return true;
		}
		if (name == PropertyName._cameraShakeStepsRemaining)
		{
			value = VariantUtils.CreateFrom(in _cameraShakeStepsRemaining);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._fullScreenColorRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fullScreenBlinkTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cameraShakeRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cameraShakeDirection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cameraShakeForce, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cameraShakeInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cameraShakeStepsRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._fullScreenColorRect, Variant.From(in _fullScreenColorRect));
		info.AddProperty(PropertyName._fullScreenBlinkTween, Variant.From(in _fullScreenBlinkTween));
		info.AddProperty(PropertyName._cameraShakeRunning, Variant.From(in _cameraShakeRunning));
		info.AddProperty(PropertyName._cameraShakeDirection, Variant.From(in _cameraShakeDirection));
		info.AddProperty(PropertyName._cameraShakeForce, Variant.From(in _cameraShakeForce));
		info.AddProperty(PropertyName._cameraShakeInterval, Variant.From(in _cameraShakeInterval));
		info.AddProperty(PropertyName._cameraShakeStepsRemaining, Variant.From(in _cameraShakeStepsRemaining));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._fullScreenColorRect, out var value))
		{
			_fullScreenColorRect = value.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName._fullScreenBlinkTween, out var value2))
		{
			_fullScreenBlinkTween = value2.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._cameraShakeRunning, out var value3))
		{
			_cameraShakeRunning = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cameraShakeDirection, out var value4))
		{
			_cameraShakeDirection = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._cameraShakeForce, out var value5))
		{
			_cameraShakeForce = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cameraShakeInterval, out var value6))
		{
			_cameraShakeInterval = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cameraShakeStepsRemaining, out var value7))
		{
			_cameraShakeStepsRemaining = value7.As<int>();
		}
	}
}
