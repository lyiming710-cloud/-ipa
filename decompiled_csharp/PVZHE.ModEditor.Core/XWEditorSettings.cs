using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWEditorSettings.cs")]
public class XWEditorSettings : RefCounted
{
	[Signal]
	public delegate void SettingsChangedEventHandler();

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Load = "Load";

		public static readonly StringName Save = "Save";

		public static readonly StringName RegisterSetting = "RegisterSetting";

		public static readonly StringName GetSetting = "GetSetting";

		public static readonly StringName SetSetting = "SetSetting";

		public static readonly StringName HasSetting = "HasSetting";

		public static readonly StringName InitializeDefaultSettings = "InitializeDefaultSettings";

		public static readonly StringName EnsureDirectory = "EnsureDirectory";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _config = "_config";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName SettingsChanged = "SettingsChanged";
	}

	private const string SavePath = "user://ModEditor/editor_settings.cfg";

	private readonly ConfigFile _config = new ConfigFile();

	private readonly Dictionary<string, Variant> _cache = new Dictionary<string, Variant>();

	private SettingsChangedEventHandler backing_SettingsChanged;

	public event SettingsChangedEventHandler SettingsChanged
	{
		add
		{
			backing_SettingsChanged = (SettingsChangedEventHandler)Delegate.Combine(backing_SettingsChanged, value);
		}
		remove
		{
			backing_SettingsChanged = (SettingsChangedEventHandler)Delegate.Remove(backing_SettingsChanged, value);
		}
	}

	public void Load()
	{
		if (_config.Load("user://ModEditor/editor_settings.cfg") != Error.Ok)
		{
			return;
		}
		_cache.Clear();
		string[] sections = _config.GetSections();
		foreach (string text in sections)
		{
			string[] sectionKeys = _config.GetSectionKeys(text);
			foreach (string text2 in sectionKeys)
			{
				string key = text + "/" + text2;
				_cache[key] = _config.GetValue(text, text2);
			}
		}
	}

	public void Save()
	{
		EnsureDirectory();
		_config.Save("user://ModEditor/editor_settings.cfg");
	}

	public void RegisterSetting(string name, Variant defaultValue)
	{
		if (!_cache.ContainsKey(name))
		{
			_cache[name] = defaultValue;
			string[] array = name.Split('/', 2);
			if (array.Length == 2)
			{
				_config.SetValue(array[0], array[1], defaultValue);
			}
			else
			{
				_config.SetValue("editor", name, defaultValue);
			}
		}
	}

	public Variant GetSetting(string name)
	{
		if (!_cache.TryGetValue(name, out var value))
		{
			return default;
		}
		return value;
	}

	public T GetSetting<[MustBeVariant] T>(string name, T defaultValue = default(T))
	{
		if (!_cache.TryGetValue(name, out var value))
		{
			return defaultValue;
		}
		try
		{
			return value.As<T>();
		}
		catch
		{
			return defaultValue;
		}
	}

	public void SetSetting(string name, Variant value)
	{
		_cache[name] = value;
		string[] array = name.Split('/', 2);
		if (array.Length == 2)
		{
			_config.SetValue(array[0], array[1], value);
		}
		else
		{
			_config.SetValue("editor", name, value);
		}
		EmitSignal(SignalName.SettingsChanged);
	}

	public bool HasSetting(string name)
	{
		return _cache.ContainsKey(name);
	}

	public void InitializeDefaultSettings()
	{
		RegisterSetting("interface/theme", "Default");
		RegisterSetting("interface/language", "zh_CN");
		RegisterSetting("interface/reduced_motion", false);
		RegisterSetting("interface/low_performance_mode", false);
		RegisterSetting("editor/auto_save_interval", 300);
		RegisterSetting("editor/show_toasts", true);
	}

	private void EnsureDirectory()
	{
		if (!DirAccess.DirExistsAbsolute("user://ModEditor"))
		{
			DirAccess.MakeDirAbsolute("user://ModEditor");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Load, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterSetting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSetting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSetting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSetting, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeDefaultSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureDirectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Load && args.Count == 0)
		{
			Load();
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterSetting && args.Count == 2)
		{
			RegisterSetting(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSetting && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetSetting(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSetting && args.Count == 2)
		{
			SetSetting(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasSetting && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSetting(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InitializeDefaultSettings && args.Count == 0)
		{
			InitializeDefaultSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureDirectory && args.Count == 0)
		{
			EnsureDirectory();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Load)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.RegisterSetting)
		{
			return true;
		}
		if (method == MethodName.GetSetting)
		{
			return true;
		}
		if (method == MethodName.SetSetting)
		{
			return true;
		}
		if (method == MethodName.HasSetting)
		{
			return true;
		}
		if (method == MethodName.InitializeDefaultSettings)
		{
			return true;
		}
		if (method == MethodName.EnsureDirectory)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddSignalEventDelegate(SignalName.SettingsChanged, backing_SettingsChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetSignalEventDelegate<SettingsChangedEventHandler>(SignalName.SettingsChanged, out var value))
		{
			backing_SettingsChanged = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.SettingsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalSettingsChanged()
	{
		EmitSignal(SignalName.SettingsChanged, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.SettingsChanged && args.Count == 0)
		{
			backing_SettingsChanged?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.SettingsChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
