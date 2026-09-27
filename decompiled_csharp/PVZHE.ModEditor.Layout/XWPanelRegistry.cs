using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWPanelRegistry.cs")]
public class XWPanelRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName RegisterDock = "RegisterDock";

		public new static readonly StringName Get = "Get";

		public static readonly StringName GetDockInstance = "GetDockInstance";

		public static readonly StringName GetDisplayName = "GetDisplayName";

		public static readonly StringName GetIcon = "GetIcon";

		public static readonly StringName GetDefaultDock = "GetDefaultDock";

		public static readonly StringName CanClose = "CanClose";

		public static readonly StringName CanAddMultiple = "CanAddMultiple";

		public static readonly StringName IsMainScreen = "IsMainScreen";

		public static readonly StringName IsMainPanel = "IsMainPanel";

		public static readonly StringName IsGlobal = "IsGlobal";

		public static readonly StringName IsTransient = "IsTransient";

		public static readonly StringName GenerateInstanceKey = "GenerateInstanceKey";

		public static readonly StringName CreatePanelInstance = "CreatePanelInstance";

		public static readonly StringName RegisterBuiltInPanels = "RegisterBuiltInPanels";

		public static readonly StringName LoadTabIcon = "LoadTabIcon";

		public static readonly StringName SetPanelScene = "SetPanelScene";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _instanceCounter = "_instanceCounter";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private readonly Dictionary<string, XWPanelInfo> _panels = new Dictionary<string, XWPanelInfo>();

	private readonly Dictionary<string, PackedScene> _dockClasses = new Dictionary<string, PackedScene>();

	private readonly Dictionary<string, XWEditorDock> _dockInstances = new Dictionary<string, XWEditorDock>();

	private int _instanceCounter;

	public static XWPanelRegistry Instance { get; private set; }

	public XWPanelRegistry()
	{
		Instance = this;
	}

	public void Register(XWPanelInfo info)
	{
		_panels[info.Key] = info;
	}

	public void RegisterDock(string key, PackedScene dockScene)
	{
		_dockClasses[key] = dockScene;
	}

	public XWPanelInfo Get(string key)
	{
		if (!_panels.TryGetValue(key, out var value))
		{
			return null;
		}
		return value;
	}

	public XWEditorDock GetDockInstance(string key)
	{
		if (_dockInstances.TryGetValue(key, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		return null;
	}

	public List<string> GetAllKeys()
	{
		List<string> list = new List<string>();
		foreach (string key in _panels.Keys)
		{
			list.Add(key);
		}
		foreach (string key2 in _dockClasses.Keys)
		{
			if (!list.Contains(key2))
			{
				list.Add(key2);
			}
		}
		return list;
	}

	public string GetDisplayName(string key)
	{
		XWEditorDock dockInstance = GetDockInstance(key);
		if (dockInstance == null || !(dockInstance.Title != ""))
		{
			return Get(key)?.DisplayName ?? key;
		}
		return dockInstance.Title;
	}

	public Texture2D GetIcon(string key)
	{
		XWEditorDock dockInstance = GetDockInstance(key);
		if (dockInstance != null && dockInstance.DockIcon != null)
		{
			return dockInstance.DockIcon;
		}
		return Get(key)?.Icon;
	}

	public int GetDefaultDock(string key)
	{
		return GetDockInstance(key)?.DefaultSlot ?? Get(key)?.DefaultDock ?? 0;
	}

	public bool CanClose(string key)
	{
		return GetDockInstance(key)?.Closable ?? Get(key)?.CanClose ?? true;
	}

	public bool CanAddMultiple(string key)
	{
		return Get(key)?.CanAddMultiple ?? false;
	}

	public bool IsMainScreen(string key)
	{
		return GetDockInstance(key)?.IsMainScreen ?? Get(key)?.IsMainPanel ?? false;
	}

	public bool IsMainPanel(string key)
	{
		return IsMainScreen(key);
	}

	public bool IsGlobal(string key)
	{
		return GetDockInstance(key)?.IsGlobal ?? false;
	}

	public bool IsTransient(string key)
	{
		return GetDockInstance(key)?.IsTransient ?? false;
	}

	public string GenerateInstanceKey(string key)
	{
		_instanceCounter++;
		return $"{key}_{_instanceCounter}";
	}

	public Control CreatePanelInstance(string key)
	{
		Control control = null;
		if (_dockClasses.TryGetValue(key, out var value) && value != null)
		{
			control = value.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		}
		else
		{
			XWPanelInfo xWPanelInfo = Get(key);
			if (xWPanelInfo?.Scene != null)
			{
				control = xWPanelInfo.Scene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			}
		}
		if (control == null)
		{
			return null;
		}
		if (control is XWEditorDock xWEditorDock)
		{
			if (string.IsNullOrEmpty(xWEditorDock.LayoutKey))
			{
				xWEditorDock.LayoutKey = key;
			}
			control.SetMeta("_dock_panel_key", key);
			_dockInstances[key] = xWEditorDock;
		}
		return control;
	}

	public void RegisterBuiltInPanels()
	{
		Register(new XWPanelInfo("scene_tree", "场景", null, 6, canClose: false, canAddMultiple: false, isMainPanel: false, LoadTabIcon("res://addons/ModEditor/Icons/PackedScene.svg")));
		Register(new XWPanelInfo("file_system", "文件系统", null, 7, canClose: false, canAddMultiple: false, isMainPanel: false, LoadTabIcon("res://addons/ModEditor/Icons/Filesystem.svg")));
		Register(new XWPanelInfo("inspector", "检查器", null, 1, canClose: false, canAddMultiple: false, isMainPanel: false, LoadTabIcon("res://addons/ModEditor/Icons/EditorInspector.svg")));
		Register(new XWPanelInfo("bp_editor", "蓝图", null, 2, canClose: false, canAddMultiple: false, isMainPanel: true, LoadTabIcon("res://addons/ModEditor/Icons/GraphEdit.svg")));
		Register(new XWPanelInfo("2d_editor", "二维场景", null, 2, canClose: false, canAddMultiple: false, isMainPanel: true, LoadTabIcon("res://addons/ModEditor/Icons/ClassIcon/2D.svg")));
		Register(new XWPanelInfo("script_editor", "脚本", null, 2, canClose: false, canAddMultiple: false, isMainPanel: true, LoadTabIcon("res://addons/ModEditor/Icons/Script.svg")));
		foreach (XWVisualEditorDescriptor allEditor in XWResourceEditorRegistry.GetAllEditors())
		{
			Register(new XWPanelInfo(allEditor.DockKey, allEditor.DisplayName, null, 2, canClose: false, canAddMultiple: false, isMainPanel: true, LoadTabIcon(allEditor.IconPath) ?? LoadTabIcon("res://addons/ModEditor/Icons/ClassIcon/ResourcePreloader.svg")));
		}
		Register(new XWPanelInfo("output", "输出", null, 3, canClose: false, canAddMultiple: false, isMainPanel: false, LoadTabIcon("res://addons/ModEditor/Icons/Output.svg")));
		Register(new XWPanelInfo("history_dock", "历史", null, 1, canClose: false));
	}

	private static Texture2D LoadTabIcon(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	public void SetPanelScene(string key, PackedScene scene)
	{
		if (_panels.TryGetValue(key, out var value))
		{
			value.Scene = scene;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "info", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dockScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.Get, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDockInstance, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDefaultDock, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanClose, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanAddMultiple, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMainScreen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMainPanel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsGlobal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsTransient, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateInstanceKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePanelInstance, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterBuiltInPanels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadTabIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPanelScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 1)
		{
			Register(VariantUtils.ConvertTo<XWPanelInfo>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterDock && args.Count == 2)
		{
			RegisterDock(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWPanelInfo>(Get(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDockInstance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWEditorDock>(GetDockInstance(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDisplayName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDefaultDock && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetDefaultDock(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanClose && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanClose(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanAddMultiple && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAddMultiple(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMainScreen && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMainScreen(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMainPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMainPanel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGlobal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGlobal(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsTransient && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTransient(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GenerateInstanceKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GenerateInstanceKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePanelInstance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreatePanelInstance(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterBuiltInPanels && args.Count == 0)
		{
			RegisterBuiltInPanels();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadTabIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTabIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPanelScene && args.Count == 2)
		{
			SetPanelScene(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadTabIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTabIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.RegisterDock)
		{
			return true;
		}
		if (method == MethodName.Get)
		{
			return true;
		}
		if (method == MethodName.GetDockInstance)
		{
			return true;
		}
		if (method == MethodName.GetDisplayName)
		{
			return true;
		}
		if (method == MethodName.GetIcon)
		{
			return true;
		}
		if (method == MethodName.GetDefaultDock)
		{
			return true;
		}
		if (method == MethodName.CanClose)
		{
			return true;
		}
		if (method == MethodName.CanAddMultiple)
		{
			return true;
		}
		if (method == MethodName.IsMainScreen)
		{
			return true;
		}
		if (method == MethodName.IsMainPanel)
		{
			return true;
		}
		if (method == MethodName.IsGlobal)
		{
			return true;
		}
		if (method == MethodName.IsTransient)
		{
			return true;
		}
		if (method == MethodName.GenerateInstanceKey)
		{
			return true;
		}
		if (method == MethodName.CreatePanelInstance)
		{
			return true;
		}
		if (method == MethodName.RegisterBuiltInPanels)
		{
			return true;
		}
		if (method == MethodName.LoadTabIcon)
		{
			return true;
		}
		if (method == MethodName.SetPanelScene)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._instanceCounter)
		{
			_instanceCounter = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._instanceCounter)
		{
			value = VariantUtils.CreateFrom(in _instanceCounter);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._instanceCounter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._instanceCounter, Variant.From(in _instanceCounter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._instanceCounter, out var value))
		{
			_instanceCounter = value.As<int>();
		}
	}
}
