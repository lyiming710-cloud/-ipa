using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Core/Display/DeviceFrameRateService.cs")]
public class DeviceFrameRateService : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Initialize = "Initialize";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName GetFrameRateOptions = "GetFrameRateOptions";

		public static readonly StringName SetPreferredFrameRate = "SetPreferredFrameRate";

		public static readonly StringName RefreshDesktopCapabilities = "RefreshDesktopCapabilities";

		public static readonly StringName RefreshAndroidCapabilities = "RefreshAndroidCapabilities";

		public static readonly StringName GetAndroidDisplay = "GetAndroidDisplay";

		public static readonly StringName SetDeviceLimit = "SetDeviceLimit";

		public static readonly StringName ApplyPreferredFrameRate = "ApplyPreferredFrameRate";

		public static readonly StringName RequestAndroidRefreshRate = "RequestAndroidRefreshRate";

		public static readonly StringName ThrowIfJavaException = "ThrowIfJavaException";

		public static readonly StringName ApplyAndroidFallback = "ApplyAndroidFallback";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName PreferredFrameRate = "PreferredFrameRate";

		public static readonly StringName DeviceFrameRateLimit = "DeviceFrameRateLimit";

		public static readonly StringName EffectiveFrameRate = "EffectiveFrameRate";

		public static readonly StringName FrameRateOptions = "FrameRateOptions";

		public static readonly StringName IsAndroid = "IsAndroid";

		public static readonly StringName _desktopPollElapsed = "_desktopPollElapsed";

		public static readonly StringName _lastScreen = "_lastScreen";

		public static readonly StringName _hasApplied = "_hasApplied";

		public static readonly StringName _androidFailureReported = "_androidFailureReported";

		public static readonly StringName _androidWidth = "_androidWidth";

		public static readonly StringName _androidHeight = "_androidHeight";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const double DesktopRefreshPollSeconds = 1.0;

	private readonly List<DisplayRefreshMode> _androidModes = new List<DisplayRefreshMode>();

	private double _desktopPollElapsed;

	private int _lastScreen = -2147483648;

	private bool _hasApplied;

	private bool _androidFailureReported;

	private int _androidWidth;

	private int _androidHeight;

	public int PreferredFrameRate { get; private set; } = 60;

	public int DeviceFrameRateLimit { get; private set; } = 60;

	public int EffectiveFrameRate { get; private set; } = 60;

	public int[] FrameRateOptions { get; private set; } = FrameRatePolicy.BuildOptions(60);

	private bool IsAndroid => OS.GetName() == "Android";

	public event Action FrameRateOptionsChanged;

	public event Action<int> EffectiveFrameRateChanged;

	public void Initialize(double preferredFrameRate)
	{
		PreferredFrameRate = FrameRatePolicy.NormalizeRefreshRate(preferredFrameRate);
	}

	public override void _Ready()
	{
		if (IsAndroid)
		{
			RefreshAndroidCapabilities();
			SetProcess(enable: false);
		}
		else
		{
			RefreshDesktopCapabilities(force: true);
			SetProcess(enable: true);
		}
	}

	public override void _Process(double delta)
	{
		_desktopPollElapsed += delta;
		if (!(_desktopPollElapsed < 1.0))
		{
			_desktopPollElapsed = 0.0;
			RefreshDesktopCapabilities(force: false);
		}
	}

	public override void _Notification(int what)
	{
		if ((long)what != 2014)
		{
			return;
		}
		Callable.From(() =>
		{
			if (IsAndroid)
			{
				RefreshAndroidCapabilities();
			}
			else
			{
				RefreshDesktopCapabilities(force: true);
			}
		}).CallDeferred();
	}

	public int[] GetFrameRateOptions()
	{
		return (int[])FrameRateOptions.Clone();
	}

	public void SetPreferredFrameRate(double value)
	{
		PreferredFrameRate = FrameRatePolicy.NormalizeRefreshRate(value);
		ApplyPreferredFrameRate();
	}

	private void RefreshDesktopCapabilities(bool force)
	{
		int num = DisplayServer.WindowGetCurrentScreen();
		double num2 = DisplayServer.ScreenGetRefreshRate(num);
		int num3 = ((num2 >= 1.0) ? FrameRatePolicy.NormalizeRefreshRate(num2) : 60);
		if (force || num != _lastScreen || num3 != DeviceFrameRateLimit)
		{
			_lastScreen = num;
			SetDeviceLimit(num3);
		}
	}

	private void RefreshAndroidCapabilities()
	{
		try
		{
			if (!TryGetAndroidContext(out var _, out var activity))
			{
				ApplyAndroidFallback("AndroidRuntime or Activity is unavailable.");
				return;
			}
			JavaObject androidDisplay = GetAndroidDisplay(activity);
			JavaObject javaObject = ((androidDisplay == null) ? null : (androidDisplay.Call("getMode").AsGodotObject() as JavaObject));
			JavaObject[] array = ((androidDisplay == null) ? Array.Empty<JavaObject>() : androidDisplay.Call("getSupportedModes").AsGodotObjectArray<JavaObject>());
			if (javaObject == null || array.Length == 0)
			{
				ApplyAndroidFallback("Android returned no supported display modes.");
				return;
			}
			_androidWidth = javaObject.Call("getPhysicalWidth").AsInt32();
			_androidHeight = javaObject.Call("getPhysicalHeight").AsInt32();
			_androidModes.Clear();
			JavaObject[] array2 = array;
			foreach (JavaObject javaObject2 in array2)
			{
				if (javaObject2 != null)
				{
					_androidModes.Add(new DisplayRefreshMode(javaObject2.Call("getModeId").AsInt32(), javaObject2.Call("getPhysicalWidth").AsInt32(), javaObject2.Call("getPhysicalHeight").AsInt32(), FrameRatePolicy.NormalizeRefreshRate(javaObject2.Call("getRefreshRate").AsDouble())));
				}
			}
			ThrowIfJavaException("query supported display modes");
			if (_androidModes.Count == 0)
			{
				ApplyAndroidFallback("Android returned only invalid display modes.");
				return;
			}
			_androidFailureReported = false;
			DisplayRefreshMode[] array3 = _androidModes.Where((DisplayRefreshMode mode) => mode.Width == _androidWidth && mode.Height == _androidHeight).ToArray();
			DisplayRefreshMode[] source = ((array3.Length != 0) ? array3 : _androidModes.ToArray());
			SetDeviceLimit(source.Max((DisplayRefreshMode mode) => mode.RefreshRate));
		}
		catch (Exception ex)
		{
			ApplyAndroidFallback("Android refresh-rate discovery failed: " + ex.Message);
		}
	}

	private static bool TryGetAndroidContext(out GodotObject runtime, out JavaObject activity)
	{
		runtime = null;
		activity = null;
		if (!Engine.HasSingleton("AndroidRuntime"))
		{
			return false;
		}
		runtime = Engine.GetSingleton("AndroidRuntime");
		activity = runtime.Call("getActivity").AsGodotObject() as JavaObject;
		return activity != null;
	}

	private static JavaObject GetAndroidDisplay(JavaObject activity)
	{
		if (activity.Call("getWindowManager").AsGodotObject() is JavaObject javaObject)
		{
			return javaObject.Call("getDefaultDisplay").AsGodotObject() as JavaObject;
		}
		return null;
	}

	private void SetDeviceLimit(int limit)
	{
		DeviceFrameRateLimit = Math.Max(1, limit);
		int[] array = FrameRatePolicy.BuildOptions(DeviceFrameRateLimit);
		bool flag = !Enumerable.SequenceEqual(FrameRateOptions, array);
		if (flag)
		{
			FrameRateOptions = array;
		}
		ApplyPreferredFrameRate();
		if (flag)
		{
			FrameRateOptionsChanged?.Invoke();
		}
	}

	private void ApplyPreferredFrameRate()
	{
		int num = FrameRatePolicy.ResolveEffective(PreferredFrameRate, DeviceFrameRateLimit);
		if (!_hasApplied || num != EffectiveFrameRate)
		{
			if (IsAndroid && _androidModes.Count > 0)
			{
				RequestAndroidRefreshRate(num);
			}
			_hasApplied = true;
			EffectiveFrameRate = num;
			EffectiveFrameRateChanged?.Invoke(num);
		}
	}

	private void RequestAndroidRefreshRate(int targetFrameRate)
	{
		DisplayRefreshMode selected = FrameRatePolicy.SelectAndroidMode(_androidModes.ToArray(), targetFrameRate, _androidWidth, _androidHeight);
		if (selected == null || !TryGetAndroidContext(out var runtime, out var activity))
		{
			return;
		}
		Callable callable = Callable.From(() =>
		{
			JavaObject javaObject2 = activity.Call("getWindow").AsGodotObject() as JavaObject;
			JavaObject javaObject3 = ((javaObject2 == null) ? null : (javaObject2.Call("getAttributes").AsGodotObject() as JavaObject));
			if (javaObject2 != null && javaObject3 != null)
			{
				javaObject3.Set("preferredDisplayModeId", 0);
				javaObject3.Set("preferredRefreshRate", (float)selected.RefreshRate);
				javaObject2.Call("setAttributes", javaObject3);
				JavaObject exception = JavaClassWrapper.Singleton.GetException();
				if (exception != null && !_androidFailureReported)
				{
					_androidFailureReported = true;
					GD.PushWarning($"Android display-mode request failed: {exception}");
				}
			}
		});
		if (runtime.Call("createRunnableFromGodotCallable", callable).AsGodotObject() is JavaObject javaObject)
		{
			activity.Call("runOnUiThread", javaObject);
		}
	}

	private static void ThrowIfJavaException(string operation)
	{
		JavaObject exception = JavaClassWrapper.Singleton.GetException();
		if (exception != null)
		{
			throw new InvalidOperationException($"Java exception during {operation}: {exception}");
		}
	}

	private void ApplyAndroidFallback(string message)
	{
		_androidModes.Clear();
		if (!_androidFailureReported)
		{
			_androidFailureReported = true;
			GD.PushWarning(message);
		}
		double num = DisplayServer.ScreenGetRefreshRate(DisplayServer.WindowGetCurrentScreen());
		SetDeviceLimit((num >= 1.0) ? FrameRatePolicy.NormalizeRefreshRate(num) : 60);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "preferredFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFrameRateOptions, new PropertyInfo(Variant.Type.PackedInt32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPreferredFrameRate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDesktopCapabilities, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAndroidCapabilities, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAndroidDisplay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JavaObject"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "activity", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JavaObject"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetDeviceLimit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "limit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPreferredFrameRate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestAndroidRefreshRate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "targetFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ThrowIfJavaException, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyAndroidFallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Initialize && args.Count == 1)
		{
			Initialize(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFrameRateOptions && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int[]>(GetFrameRateOptions());
			return true;
		}
		if (method == MethodName.SetPreferredFrameRate && args.Count == 1)
		{
			SetPreferredFrameRate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDesktopCapabilities && args.Count == 1)
		{
			RefreshDesktopCapabilities(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAndroidCapabilities && args.Count == 0)
		{
			RefreshAndroidCapabilities();
			ret = default;
			return true;
		}
		if (method == MethodName.GetAndroidDisplay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<JavaObject>(GetAndroidDisplay(VariantUtils.ConvertTo<JavaObject>(in args[0])));
			return true;
		}
		if (method == MethodName.SetDeviceLimit && args.Count == 1)
		{
			SetDeviceLimit(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPreferredFrameRate && args.Count == 0)
		{
			ApplyPreferredFrameRate();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestAndroidRefreshRate && args.Count == 1)
		{
			RequestAndroidRefreshRate(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowIfJavaException && args.Count == 1)
		{
			ThrowIfJavaException(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAndroidFallback && args.Count == 1)
		{
			ApplyAndroidFallback(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetAndroidDisplay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<JavaObject>(GetAndroidDisplay(VariantUtils.ConvertTo<JavaObject>(in args[0])));
			return true;
		}
		if (method == MethodName.ThrowIfJavaException && args.Count == 1)
		{
			ThrowIfJavaException(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.GetFrameRateOptions)
		{
			return true;
		}
		if (method == MethodName.SetPreferredFrameRate)
		{
			return true;
		}
		if (method == MethodName.RefreshDesktopCapabilities)
		{
			return true;
		}
		if (method == MethodName.RefreshAndroidCapabilities)
		{
			return true;
		}
		if (method == MethodName.GetAndroidDisplay)
		{
			return true;
		}
		if (method == MethodName.SetDeviceLimit)
		{
			return true;
		}
		if (method == MethodName.ApplyPreferredFrameRate)
		{
			return true;
		}
		if (method == MethodName.RequestAndroidRefreshRate)
		{
			return true;
		}
		if (method == MethodName.ThrowIfJavaException)
		{
			return true;
		}
		if (method == MethodName.ApplyAndroidFallback)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.PreferredFrameRate)
		{
			PreferredFrameRate = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DeviceFrameRateLimit)
		{
			DeviceFrameRateLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.EffectiveFrameRate)
		{
			EffectiveFrameRate = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.FrameRateOptions)
		{
			FrameRateOptions = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName._desktopPollElapsed)
		{
			_desktopPollElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._lastScreen)
		{
			_lastScreen = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hasApplied)
		{
			_hasApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._androidFailureReported)
		{
			_androidFailureReported = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._androidWidth)
		{
			_androidWidth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._androidHeight)
		{
			_androidHeight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.PreferredFrameRate)
		{
			from = PreferredFrameRate;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DeviceFrameRateLimit)
		{
			from = DeviceFrameRateLimit;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.EffectiveFrameRate)
		{
			from = EffectiveFrameRate;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.FrameRateOptions)
		{
			value = VariantUtils.CreateFrom<int[]>(FrameRateOptions);
			return true;
		}
		if (name == PropertyName.IsAndroid)
		{
			value = VariantUtils.CreateFrom<bool>(IsAndroid);
			return true;
		}
		if (name == PropertyName._desktopPollElapsed)
		{
			value = VariantUtils.CreateFrom(in _desktopPollElapsed);
			return true;
		}
		if (name == PropertyName._lastScreen)
		{
			value = VariantUtils.CreateFrom(in _lastScreen);
			return true;
		}
		if (name == PropertyName._hasApplied)
		{
			value = VariantUtils.CreateFrom(in _hasApplied);
			return true;
		}
		if (name == PropertyName._androidFailureReported)
		{
			value = VariantUtils.CreateFrom(in _androidFailureReported);
			return true;
		}
		if (name == PropertyName._androidWidth)
		{
			value = VariantUtils.CreateFrom(in _androidWidth);
			return true;
		}
		if (name == PropertyName._androidHeight)
		{
			value = VariantUtils.CreateFrom(in _androidHeight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._desktopPollElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastScreen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._androidFailureReported, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._androidWidth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._androidHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PreferredFrameRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DeviceFrameRateLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EffectiveFrameRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.FrameRateOptions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsAndroid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.PreferredFrameRate, Variant.From<int>(PreferredFrameRate));
		info.AddProperty(PropertyName.DeviceFrameRateLimit, Variant.From<int>(DeviceFrameRateLimit));
		info.AddProperty(PropertyName.EffectiveFrameRate, Variant.From<int>(EffectiveFrameRate));
		info.AddProperty(PropertyName.FrameRateOptions, Variant.From<int[]>(FrameRateOptions));
		info.AddProperty(PropertyName._desktopPollElapsed, Variant.From(in _desktopPollElapsed));
		info.AddProperty(PropertyName._lastScreen, Variant.From(in _lastScreen));
		info.AddProperty(PropertyName._hasApplied, Variant.From(in _hasApplied));
		info.AddProperty(PropertyName._androidFailureReported, Variant.From(in _androidFailureReported));
		info.AddProperty(PropertyName._androidWidth, Variant.From(in _androidWidth));
		info.AddProperty(PropertyName._androidHeight, Variant.From(in _androidHeight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.PreferredFrameRate, out var value))
		{
			PreferredFrameRate = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DeviceFrameRateLimit, out var value2))
		{
			DeviceFrameRateLimit = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.EffectiveFrameRate, out var value3))
		{
			EffectiveFrameRate = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.FrameRateOptions, out var value4))
		{
			FrameRateOptions = value4.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName._desktopPollElapsed, out var value5))
		{
			_desktopPollElapsed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._lastScreen, out var value6))
		{
			_lastScreen = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hasApplied, out var value7))
		{
			_hasApplied = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._androidFailureReported, out var value8))
		{
			_androidFailureReported = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._androidWidth, out var value9))
		{
			_androidWidth = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._androidHeight, out var value10))
		{
			_androidHeight = value10.As<int>();
		}
	}
}
