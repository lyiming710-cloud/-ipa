using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Registry.Class;

[ScriptPath("res://addons/ModEditor/Registry/Class/XWClassRegistry.cs")]
public sealed class XWClassRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName LoadIconSafe = "LoadIconSafe";

		public static readonly StringName LoadClassIconSafe = "LoadClassIconSafe";

		public static readonly StringName ResolveClassIcon = "ResolveClassIcon";

		public static readonly StringName Initialize = "Initialize";

		public static readonly StringName GetScanStatistics = "GetScanStatistics";

		public static readonly StringName ClearCache = "ClearCache";

		public static readonly StringName GetIconRegex = "GetIconRegex";

		public static readonly StringName RebuildParentLinks = "RebuildParentLinks";

		public static readonly StringName RegisterGodotClassInit = "RegisterGodotClassInit";

		public static readonly StringName RegisterGlobalClass = "RegisterGlobalClass";

		public static readonly StringName Register = "Register";

		public static readonly StringName Unregister = "Unregister";

		public static readonly StringName HasClass = "HasClass";

		public static readonly StringName GetClassData = "GetClassData";

		public static readonly StringName IsClassInstanceOf = "IsClassInstanceOf";

		public static readonly StringName GetParentClass = "GetParentClass";

		public static readonly StringName IsParentClass = "IsParentClass";

		public static readonly StringName CanInstantiate = "CanInstantiate";

		public static readonly StringName Instantiate = "Instantiate";

		public static readonly StringName GetClassIcon = "GetClassIcon";

		public static readonly StringName GetClassColor = "GetClassColor";

		public static readonly StringName GetClassIntegerConstantList = "GetClassIntegerConstantList";

		public static readonly StringName GetClassEnumList = "GetClassEnumList";

		public static readonly StringName GetUIIcon = "GetUIIcon";

		public static readonly StringName InitUIIcons = "InitUIIcons";

		public static readonly StringName DoesClassImplementMethod = "DoesClassImplementMethod";

		public static readonly StringName EnsureMethodNamesCached = "EnsureMethodNamesCached";

		public static readonly StringName ResetInstance = "ResetInstance";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _scanStatistics = "_scanStatistics";

		public static readonly StringName _iconRegex = "_iconRegex";

		public static readonly StringName _uiIcons = "_uiIcons";

		public static readonly StringName _isInit = "_isInit";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private static XWClassRegistry _instance;

	private static Texture2D _objectIcon;

	private static Texture2D _iconNode2D;

	private static Texture2D _iconControl;

	private static Texture2D _iconNode3D;

	private static readonly Color ColorNode2D = new Color(0.553f, 0.647f, 0.953f);

	private static readonly Color ColorControl = new Color(0.557f, 0.937f, 0.592f);

	private static readonly Color ColorNode3D = new Color(0.988f, 0.498f, 0.498f);

	private readonly System.Collections.Generic.Dictionary<string, XWClassData> _classDictionary = new System.Collections.Generic.Dictionary<string, XWClassData>();

	private readonly System.Collections.Generic.Dictionary<string, XWClassData> _globalClassDictionary = new System.Collections.Generic.Dictionary<string, XWClassData>();

	private readonly System.Collections.Generic.Dictionary<string, Dictionary> _scannedScriptsCache = new System.Collections.Generic.Dictionary<string, Dictionary>();

	private readonly System.Collections.Generic.Dictionary<string, long> _lastScanTime = new System.Collections.Generic.Dictionary<string, long>();

	private Dictionary _scanStatistics = new Dictionary();

	private RegEx _iconRegex;

	private readonly Dictionary _uiIcons = new Dictionary();

	private bool _isInit;

	public static XWClassRegistry Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new XWClassRegistry();
				_instance.Initialize();
			}
			return _instance;
		}
		private set
		{
			_instance = value;
		}
	}

	private static Texture2D ObjectIcon => _objectIcon ?? (_objectIcon = LoadIconSafe("res://addons/ModEditor/Icons/Object.svg"));

	private static Texture2D IconNode2D => _iconNode2D ?? (_iconNode2D = LoadClassIconSafe("Node2D"));

	private static Texture2D IconControl => _iconControl ?? (_iconControl = LoadClassIconSafe("Control"));

	private static Texture2D IconNode3D => _iconNode3D ?? (_iconNode3D = LoadClassIconSafe("Node3D"));

	private static Texture2D LoadIconSafe(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	private static Texture2D LoadClassIconSafe(string className)
	{
		return ResolveClassIcon(className);
	}

	private static Texture2D ResolveClassIcon(string className)
	{
		if (string.IsNullOrEmpty(className))
		{
			return null;
		}
		string text = ((className == "AudioStreamWav") ? "AudioStreamWAV" : className);
		string[] array = new string[2]
		{
			"res://addons/ModEditor/Icons/ClassIcon/" + text + ".svg",
			"res://addons/ModEditor/Icons/" + text + ".svg"
		};
		foreach (string path in array)
		{
			if (ResourceLoader.Exists(path))
			{
				Texture2D texture2D = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
				if (GodotObject.IsInstanceValid(texture2D))
				{
					return texture2D;
				}
			}
		}
		return null;
	}

	public void Initialize()
	{
		if (!_isInit)
		{
			_isInit = true;
			RegisterGodotClassInit();
			RegisterGlobalClass();
			RebuildParentLinks();
		}
	}

	public Dictionary GetScanStatistics()
	{
		return _scanStatistics;
	}

	public void ClearCache()
	{
		_scannedScriptsCache.Clear();
		_lastScanTime.Clear();
		_scanStatistics = new Dictionary();
	}

	private RegEx GetIconRegex()
	{
		if (_iconRegex == null)
		{
			_iconRegex = new RegEx();
			_iconRegex.Compile("@icon\\s*\\(\\s*\"(.*?)\"\\s*\\)");
		}
		return _iconRegex;
	}

	private (Color color, Texture2D icon) GetClassColorAndIconByBase(string baseClassName)
	{
		if (ClassDB.IsParentClass(baseClassName, "Node2D"))
		{
			return (color: ColorNode2D, icon: IconNode2D);
		}
		if (ClassDB.IsParentClass(baseClassName, "Control"))
		{
			return (color: ColorControl, icon: IconControl);
		}
		if (ClassDB.IsParentClass(baseClassName, "Node3D"))
		{
			return (color: ColorNode3D, icon: IconNode3D);
		}
		return (color: Colors.Gray, icon: ObjectIcon);
	}

	private void RebuildParentLinks()
	{
		foreach (KeyValuePair<string, XWClassData> item in _classDictionary)
		{
			XWClassData value = item.Value;
			if (!string.IsNullOrEmpty(value.BaseClass) && value.ParentClassData == null && _classDictionary.TryGetValue(value.BaseClass, out var value2))
			{
				value.ParentClassData = value2;
			}
		}
	}

	private void RegisterGodotClassInit()
	{
		string[] classList = ClassDB.GetClassList();
		foreach (string text in classList)
		{
			Texture2D texture2D = ResolveClassIcon(text);
			(Color color, Texture2D icon) classColorAndIconByBase = GetClassColorAndIconByBase(text);
			Color item = classColorAndIconByBase.color;
			Texture2D item2 = classColorAndIconByBase.icon;
			XWClassData xWClassData = new XWClassData(text, item, null, texture2D ?? item2 ?? ObjectIcon);
			xWClassData.IsGodotClass = true;
			xWClassData.BaseClass = ClassDB.GetParentClass(text);
			Register(xWClassData);
		}
	}

	private void RegisterGlobalClass()
	{
		foreach (Dictionary globalClass in ProjectSettings.GetGlobalClassList())
		{
			string className = (string)globalClass["class"];
			string text = (string)globalClass["path"];
			if (string.IsNullOrEmpty(text) || text == "res://" || !ResourceLoader.Exists(text))
			{
				continue;
			}
			Script script = ResourceLoader.Load<Script>(text, null, ResourceLoader.CacheMode.Reuse);
			if (!GodotObject.IsInstanceValid(script))
			{
				continue;
			}
			string text2 = (string)globalClass["base"];
			(Color color, Texture2D icon) classColorAndIconByBase = GetClassColorAndIconByBase(text2);
			Color item = classColorAndIconByBase.color;
			Texture2D item2 = classColorAndIconByBase.icon;
			item2 = ResolveClassIcon(className) ?? item2;
			if (globalClass.ContainsKey("icon"))
			{
				string text3 = (string)globalClass["icon"];
				if (!string.IsNullOrEmpty(text3) && text3 != "res://" && ResourceLoader.Exists(text3))
				{
					item2 = XWTextureSafety.SafeIcon(ResourceLoader.Load(text3, "", ResourceLoader.CacheMode.Reuse) as Texture2D, item2);
				}
				if (item2 == null && GodotObject.IsInstanceValid(script))
				{
					item2 = GetClassIcon(text2);
				}
			}
			XWClassData xWClassData = new XWClassData(className, item, script, item2);
			xWClassData.IsGodotClass = false;
			xWClassData.IsGlobalClass = true;
			xWClassData.BaseClass = text2;
			Register(xWClassData);
		}
	}

	public void Register(XWClassData classData)
	{
		_classDictionary[classData.ClassName] = classData;
		if (classData.IsGlobalClass)
		{
			_globalClassDictionary[classData.ClassName] = classData;
		}
		if (!string.IsNullOrEmpty(classData.BaseClass) && _classDictionary.TryGetValue(classData.BaseClass, out var value))
		{
			classData.ParentClassData = value;
		}
	}

	public void Unregister(string className)
	{
		if (_classDictionary.TryGetValue(className, out var value))
		{
			if (value.IsGlobalClass)
			{
				_globalClassDictionary.Remove(className);
			}
			_classDictionary.Remove(className);
		}
	}

	public IReadOnlyCollection<string> GetAllClass()
	{
		return _classDictionary.Keys;
	}

	public IReadOnlyCollection<XWClassData> GetAllClassData()
	{
		return _classDictionary.Values;
	}

	public bool HasClass(string className)
	{
		return _classDictionary.ContainsKey(className);
	}

	public XWClassData GetClassData(string className)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return null;
		}
		return value;
	}

	public bool IsClassInstanceOf(string className, string baseClassName)
	{
		if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(baseClassName))
		{
			return false;
		}
		if (ClassDB.IsParentClass(className, baseClassName))
		{
			return true;
		}
		return IsParentClass(className, baseClassName);
	}

	public List<string> GetClassInherits(string className)
	{
		List<string> list = new List<string>();
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			string text = className;
			while (!string.IsNullOrEmpty(text))
			{
				list.Add(text);
				text = ClassDB.GetParentClass(text);
			}
			return list;
		}
		while (value != null)
		{
			list.Add(value.ClassName);
			value = value.ParentClassData;
		}
		return list;
	}

	public string GetParentClass(string className)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return "";
		}
		if (value.ParentClassData != null)
		{
			return value.ParentClassData.ClassName;
		}
		if (value.IsGodotClass)
		{
			return ClassDB.GetParentClass(className);
		}
		return value.BaseClass;
	}

	public bool IsParentClass(string className, string inherits)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return false;
		}
		while (value != null)
		{
			if (value.ClassName == inherits)
			{
				return true;
			}
			value = value.ParentClassData;
		}
		return false;
	}

	public List<string> GetInheritersFromClass(string className)
	{
		List<string> list = new List<string>(ClassDB.GetInheritersFromClass(className));
		foreach (KeyValuePair<string, XWClassData> item in _classDictionary)
		{
			XWClassData value = item.Value;
			if (value.IsGodotClass)
			{
				continue;
			}
			for (XWClassData parentClassData = value.ParentClassData; parentClassData != null; parentClassData = parentClassData.ParentClassData)
			{
				if (parentClassData.ClassName == className)
				{
					list.Add(item.Key);
					break;
				}
			}
		}
		return list;
	}

	public bool CanInstantiate(string className)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return false;
		}
		if (value.IsGodotClass)
		{
			return ClassDB.CanInstantiate(className);
		}
		return true;
	}

	public Variant Instantiate(string className)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return default;
		}
		if (value.IsGodotClass)
		{
			return ClassDB.Instantiate(className);
		}
		if (value.IsGlobalClass && GodotObject.IsInstanceValid(value.ScriptFile))
		{
			return value.ScriptFile.Call("new");
		}
		return default;
	}

	public Texture2D GetClassIcon(string className)
	{
		if (_classDictionary.TryGetValue(className, out var value) && value != null)
		{
			return XWTextureSafety.SafeIcon(value.Icon, ObjectIcon);
		}
		return XWTextureSafety.SafeIcon(ResolveClassIcon(className), ObjectIcon);
	}

	public Color GetClassColor(string className)
	{
		if (_classDictionary.TryGetValue(className, out var value) && value != null)
		{
			return value.Color;
		}
		return Colors.White;
	}

	public List<Dictionary> GetClassPropertyList(string className)
	{
		List<Dictionary> list = new List<Dictionary>();
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return list;
		}
		while (value != null)
		{
			Array<Dictionary> array = null;
			if (value.IsGodotClass)
			{
				array = ClassDB.ClassGetPropertyList(value.ClassName, noInheritance: true);
			}
			else if (value.IsGlobalClass && GodotObject.IsInstanceValid(value.ScriptFile))
			{
				array = value.ScriptFile.GetScriptPropertyList();
			}
			if (array != null)
			{
				foreach (Dictionary item in array)
				{
					item["base_class_name"] = value.ClassName;
					list.Add(item);
				}
			}
			value = value.ParentClassData;
		}
		return list;
	}

	public List<Dictionary> GetClassMethodList(string className)
	{
		List<Dictionary> list = new List<Dictionary>();
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return list;
		}
		while (value != null)
		{
			Array<Dictionary> array = null;
			if (value.IsGodotClass)
			{
				array = ClassDB.ClassGetMethodList(value.ClassName, noInheritance: true);
			}
			else if (value.IsGlobalClass && GodotObject.IsInstanceValid(value.ScriptFile))
			{
				array = value.ScriptFile.GetScriptMethodList();
			}
			if (array != null)
			{
				foreach (Dictionary item in array)
				{
					item["base_class_name"] = value.ClassName;
					list.Add(item);
				}
			}
			value = value.ParentClassData;
		}
		return list;
	}

	public List<Dictionary> GetClassSignalList(string className)
	{
		List<Dictionary> list = new List<Dictionary>();
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return list;
		}
		while (value != null)
		{
			Array<Dictionary> array = null;
			if (value.IsGodotClass)
			{
				array = ClassDB.ClassGetSignalList(value.ClassName, noInheritance: true);
			}
			else if (value.IsGlobalClass && GodotObject.IsInstanceValid(value.ScriptFile))
			{
				array = value.ScriptFile.GetScriptSignalList();
			}
			if (array != null)
			{
				foreach (Dictionary item in array)
				{
					item["base_class_name"] = value.ClassName;
					list.Add(item);
				}
			}
			value = value.ParentClassData;
		}
		return list;
	}

	public string[] GetClassIntegerConstantList(string className)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return System.Array.Empty<string>();
		}
		if (!value.IsGodotClass)
		{
			return System.Array.Empty<string>();
		}
		return ClassDB.ClassGetIntegerConstantList(className, noInheritance: true);
	}

	public string[] GetClassEnumList(string className)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return System.Array.Empty<string>();
		}
		if (!value.IsGodotClass)
		{
			return System.Array.Empty<string>();
		}
		return ClassDB.ClassGetEnumList(className, noInheritance: true);
	}

	public Texture2D GetUIIcon(StringName iconName)
	{
		if (_uiIcons.Count == 0)
		{
			InitUIIcons();
		}
		if (!_uiIcons.ContainsKey(iconName))
		{
			return ObjectIcon;
		}
		return XWTextureSafety.SafeIcon((Texture2D)(GodotObject)_uiIcons[iconName], ObjectIcon);
	}

	private void InitUIIcons()
	{
		string text = "res://addons/ModEditor/Icons/";
		string[] array = new string[93]
		{
			"Add", "Instance", "Script", "ScriptCreate", "ScriptRemove", "ScriptExtend", "Search", "Rename", "Remove", "MoveUp",
			"MoveDown", "Duplicate", "ActionCopy", "ActionPaste", "ActionCut", "GuiVisibilityVisible", "GuiVisibilityHidden", "Lock", "ExpandTree", "CollapseTree",
			"GuiTabMenuHl", "Node", "Groups", "Favorites", "Reload", "Folder", "FolderCreate", "GDScript", "BuildCSharp", "Clear",
			"Sort", "File", "Filesystem", "PackedScene", "SceneUniqueName", "Keyword", "MemberSignal", "MemberMethod", "MemberProperty", "Close",
			"IconReload", "NodeWarning", "NodeWarnings2", "NodeWarnings3", "NodeWarnings4Plus", "Pin", "SignalsAndGroups", "EditGroup", "InstanceOptions", "Back",
			"Forward", "FileList", "FileThumbnail", "Panels1", "Panels2", "Panels2Alt", "Tools", "Object", "Terminal", "TextFile",
			"New", "Load", "Save", "Info", "VisualShader", "ShowInFileSystem", "CreateNewSceneFrom", "NonFavorite", "StatusWarning", "StatusError",
			"Progress1", "TransitionSyncAuto", "TransitionSyncAutoBig", "MainPlay", "Pause", "Stop", "PlayScene", "PlayCustom", "MainMovieWrite", "2D",
			"3D", "Game", "AssetLib", "GraphEdit", "GuiTreeArrowDown", "GuiTreeArrowRight", "CombineLines", "DistractionFree", "ExternalLink", "Paint",
			"Popup", "History", "Unlock"
		};
		foreach (string text2 in array)
		{
			string path = text + text2 + ".svg";
			if (ResourceLoader.Exists(path))
			{
				Texture2D texture2D = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
				if (GodotObject.IsInstanceValid(texture2D))
				{
					_uiIcons[text2] = texture2D;
				}
			}
		}
	}

	public bool DoesClassImplementMethod(string className, string methodName)
	{
		if (!_classDictionary.TryGetValue(className, out var value))
		{
			return false;
		}
		while (value != null)
		{
			EnsureMethodNamesCached(value);
			if (value.MethodNamesCache.ContainsKey(methodName))
			{
				return true;
			}
			value = value.ParentClassData;
		}
		return false;
	}

	private void EnsureMethodNamesCached(XWClassData classData)
	{
		if (classData.MethodNamesCached)
		{
			return;
		}
		classData.MethodNamesCached = true;
		Array<Dictionary> array = null;
		if (classData.IsGodotClass)
		{
			array = ClassDB.ClassGetMethodList(classData.ClassName);
		}
		else if (classData.IsGlobalClass && GodotObject.IsInstanceValid(classData.ScriptFile))
		{
			array = classData.ScriptFile.GetScriptMethodList();
		}
		if (array == null)
		{
			return;
		}
		foreach (Dictionary item in array)
		{
			if (((long)(item.ContainsKey("flags") ? item["flags"] : ((Variant)0L)) & 8) == 0L)
			{
				string text = (string)(item.ContainsKey("name") ? item["name"] : ((Variant)""));
				if (!string.IsNullOrEmpty(text))
				{
					classData.MethodNamesCache[text] = true;
				}
			}
		}
	}

	internal static void ResetInstance()
	{
		_instance = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(28)
		{
			new MethodInfo(MethodName.LoadIconSafe, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadClassIconSafe, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveClassIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetScanStatistics, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetIconRegex, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RegEx"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildParentLinks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterGodotClassInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterGlobalClass, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "classData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasClass, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetClassData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsClassInstanceOf, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "baseClassName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetParentClass, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsParentClass, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "inherits", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanInstantiate, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Instantiate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetClassIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetClassColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetClassIntegerConstantList, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetClassEnumList, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUIIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "iconName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitUIIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoesClassImplementMethod, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "methodName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureMethodNamesCached, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "classData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResetInstance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadIconSafe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIconSafe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadClassIconSafe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadClassIconSafe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveClassIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(ResolveClassIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Initialize && args.Count == 0)
		{
			Initialize();
			ret = default;
			return true;
		}
		if (method == MethodName.GetScanStatistics && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetScanStatistics());
			return true;
		}
		if (method == MethodName.ClearCache && args.Count == 0)
		{
			ClearCache();
			ret = default;
			return true;
		}
		if (method == MethodName.GetIconRegex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<RegEx>(GetIconRegex());
			return true;
		}
		if (method == MethodName.RebuildParentLinks && args.Count == 0)
		{
			RebuildParentLinks();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterGodotClassInit && args.Count == 0)
		{
			RegisterGodotClassInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterGlobalClass && args.Count == 0)
		{
			RegisterGlobalClass();
			ret = default;
			return true;
		}
		if (method == MethodName.Register && args.Count == 1)
		{
			Register(VariantUtils.ConvertTo<XWClassData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetClassData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWClassData>(GetClassData(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsClassInstanceOf && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsClassInstanceOf(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetParentClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetParentClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsParentClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsParentClass(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CanInstantiate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanInstantiate(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Instantiate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(Instantiate(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetClassIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetClassIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetClassColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetClassColor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetClassIntegerConstantList && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetClassIntegerConstantList(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetClassEnumList && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetClassEnumList(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUIIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetUIIcon(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.InitUIIcons && args.Count == 0)
		{
			InitUIIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.DoesClassImplementMethod && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(DoesClassImplementMethod(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.EnsureMethodNamesCached && args.Count == 1)
		{
			EnsureMethodNamesCached(VariantUtils.ConvertTo<XWClassData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetInstance && args.Count == 0)
		{
			ResetInstance();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadIconSafe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIconSafe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadClassIconSafe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadClassIconSafe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveClassIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(ResolveClassIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetInstance && args.Count == 0)
		{
			ResetInstance();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.LoadIconSafe)
		{
			return true;
		}
		if (method == MethodName.LoadClassIconSafe)
		{
			return true;
		}
		if (method == MethodName.ResolveClassIcon)
		{
			return true;
		}
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName.GetScanStatistics)
		{
			return true;
		}
		if (method == MethodName.ClearCache)
		{
			return true;
		}
		if (method == MethodName.GetIconRegex)
		{
			return true;
		}
		if (method == MethodName.RebuildParentLinks)
		{
			return true;
		}
		if (method == MethodName.RegisterGodotClassInit)
		{
			return true;
		}
		if (method == MethodName.RegisterGlobalClass)
		{
			return true;
		}
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.Unregister)
		{
			return true;
		}
		if (method == MethodName.HasClass)
		{
			return true;
		}
		if (method == MethodName.GetClassData)
		{
			return true;
		}
		if (method == MethodName.IsClassInstanceOf)
		{
			return true;
		}
		if (method == MethodName.GetParentClass)
		{
			return true;
		}
		if (method == MethodName.IsParentClass)
		{
			return true;
		}
		if (method == MethodName.CanInstantiate)
		{
			return true;
		}
		if (method == MethodName.Instantiate)
		{
			return true;
		}
		if (method == MethodName.GetClassIcon)
		{
			return true;
		}
		if (method == MethodName.GetClassColor)
		{
			return true;
		}
		if (method == MethodName.GetClassIntegerConstantList)
		{
			return true;
		}
		if (method == MethodName.GetClassEnumList)
		{
			return true;
		}
		if (method == MethodName.GetUIIcon)
		{
			return true;
		}
		if (method == MethodName.InitUIIcons)
		{
			return true;
		}
		if (method == MethodName.DoesClassImplementMethod)
		{
			return true;
		}
		if (method == MethodName.EnsureMethodNamesCached)
		{
			return true;
		}
		if (method == MethodName.ResetInstance)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._scanStatistics)
		{
			_scanStatistics = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._iconRegex)
		{
			_iconRegex = VariantUtils.ConvertTo<RegEx>(in value);
			return true;
		}
		if (name == PropertyName._isInit)
		{
			_isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._scanStatistics)
		{
			value = VariantUtils.CreateFrom(in _scanStatistics);
			return true;
		}
		if (name == PropertyName._iconRegex)
		{
			value = VariantUtils.CreateFrom(in _iconRegex);
			return true;
		}
		if (name == PropertyName._uiIcons)
		{
			value = VariantUtils.CreateFrom(in _uiIcons);
			return true;
		}
		if (name == PropertyName._isInit)
		{
			value = VariantUtils.CreateFrom(in _isInit);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._scanStatistics, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconRegex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._uiIcons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._scanStatistics, Variant.From(in _scanStatistics));
		info.AddProperty(PropertyName._iconRegex, Variant.From(in _iconRegex));
		info.AddProperty(PropertyName._isInit, Variant.From(in _isInit));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._scanStatistics, out var value))
		{
			_scanStatistics = value.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._iconRegex, out var value2))
		{
			_iconRegex = value2.As<RegEx>();
		}
		if (info.TryGetProperty(PropertyName._isInit, out var value3))
		{
			_isInit = value3.As<bool>();
		}
	}
}
