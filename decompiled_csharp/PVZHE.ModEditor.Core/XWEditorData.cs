using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWEditorData.cs")]
public class XWEditorData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Load = "Load";

		public static readonly StringName Save = "Save";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName GetValue = "GetValue";

		public static readonly StringName HasSection = "HasSection";

		public static readonly StringName HasSectionKey = "HasSectionKey";

		public static readonly StringName EnsureDirectory = "EnsureDirectory";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName EditedScenePath = "EditedScenePath";

		public static readonly StringName _config = "_config";

		public static readonly StringName _editedScenePath = "_editedScenePath";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private const string SavePath = "user://ModEditor/editor_data.cfg";

	private ConfigFile _config = new ConfigFile();

	private string _editedScenePath = "";

	public string EditedScenePath
	{
		get
		{
			return _editedScenePath;
		}
		set
		{
			_editedScenePath = value;
			_config.SetValue("editor", "edited_scene_path", value);
		}
	}

	public void Load()
	{
		if (_config.Load("user://ModEditor/editor_data.cfg") == Error.Ok)
		{
			_editedScenePath = (string)_config.GetValue("editor", "edited_scene_path", "");
		}
	}

	public void Save()
	{
		EnsureDirectory();
		_config.Save("user://ModEditor/editor_data.cfg");
	}

	public void SetValue(string section, string key, Variant value)
	{
		_config.SetValue(section, key, value);
	}

	public Variant GetValue(string section, string key, Variant defaultValue = default(Variant))
	{
		return _config.GetValue(section, key, defaultValue);
	}

	public bool HasSection(string section)
	{
		return _config.HasSection(section);
	}

	public bool HasSectionKey(string section, string key)
	{
		return _config.HasSectionKey(section, key);
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
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Load, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "section", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "section", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "section", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSectionKey, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "section", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.SetValue && args.Count == 3)
		{
			SetValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2])));
			return true;
		}
		if (method == MethodName.HasSection && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSection(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasSectionKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSectionKey(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.SetValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.HasSection)
		{
			return true;
		}
		if (method == MethodName.HasSectionKey)
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
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.EditedScenePath)
		{
			EditedScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<ConfigFile>(in value);
			return true;
		}
		if (name == PropertyName._editedScenePath)
		{
			_editedScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.EditedScenePath)
		{
			value = VariantUtils.CreateFrom<string>(EditedScenePath);
			return true;
		}
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._editedScenePath)
		{
			value = VariantUtils.CreateFrom(in _editedScenePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._editedScenePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.EditedScenePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.EditedScenePath, Variant.From<string>(EditedScenePath));
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._editedScenePath, Variant.From(in _editedScenePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.EditedScenePath, out var value))
		{
			EditedScenePath = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._config, out var value2))
		{
			_config = value2.As<ConfigFile>();
		}
		if (info.TryGetProperty(PropertyName._editedScenePath, out var value3))
		{
			_editedScenePath = value3.As<string>();
		}
	}
}
