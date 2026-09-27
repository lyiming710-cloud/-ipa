using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.Registry;

[ScriptPath("res://addons/ModEditor/Registry/Type/XWTypeRegistry.cs")]
public sealed class XWTypeRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Initialize = "Initialize";

		public static readonly StringName Init = "Init";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName LoadTypeData = "LoadTypeData";

		public static readonly StringName LoadScene = "LoadScene";

		public static readonly StringName LoadTypeIcon = "LoadTypeIcon";

		public static readonly StringName RegisterEditor = "RegisterEditor";

		public static readonly StringName RegisterHintEditor = "RegisterHintEditor";

		public static readonly StringName GetHintEditor = "GetHintEditor";

		public static readonly StringName GetEditorFromObjectProperty = "GetEditorFromObjectProperty";

		public static readonly StringName ReadPropertyValue = "ReadPropertyValue";

		public static readonly StringName IsContainerValueType = "IsContainerValueType";

		public static readonly StringName GetResourcePropertyEditor = "GetResourcePropertyEditor";

		public static readonly StringName GetResourceValueEditor = "GetResourceValueEditor";

		public static readonly StringName GetTypeEditor = "GetTypeEditor";

		public static readonly StringName HasType = "HasType";

		public static readonly StringName GetTypeName = "GetTypeName";

		public static readonly StringName GetTypeDefaultValue = "GetTypeDefaultValue";

		public static readonly StringName GetTypeDefaultValueString = "GetTypeDefaultValueString";

		public static readonly StringName GetTypeColor = "GetTypeColor";

		public static readonly StringName GetTypeIcon = "GetTypeIcon";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _isInit = "_isInit";

		public static readonly StringName _resourcePropertyEditorScene = "_resourcePropertyEditorScene";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private static XWTypeRegistry _instance;

	private bool _isInit;

	private readonly Dictionary<Variant.Type, XWTypeData> _typeDictionary = new Dictionary<Variant.Type, XWTypeData>();

	private readonly Dictionary<int, PackedScene> _hintEditorDictionary = new Dictionary<int, PackedScene>();

	private PackedScene _resourcePropertyEditorScene;

	public static XWTypeRegistry Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new XWTypeRegistry();
				_instance.Initialize();
			}
			return _instance;
		}
		private set
		{
			_instance = value;
		}
	}

	public void Initialize()
	{
		if (!_isInit)
		{
			_isInit = true;
			RegisterInit();
		}
	}

	public void Init()
	{
		Initialize();
	}

	private void RegisterInit()
	{
		RegisterEditor(LoadTypeData(Variant.Type.Bool, "bool", false, "false", new Color(0.435f, 0.569f, 0.941f), "Bool/XWInspectorPropertyEditorBool.tscn", "bool.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Int, "int", 0, "0", new Color(0.353f, 0.733f, 0.937f), "Integer/XWInspectorPropertyEditorInteger.tscn", "int.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Float, "float", 0.0, "0.0", new Color(0.208f, 0.831f, 0.957f), "Float/XWInspectorPropertyEditorFloat.tscn", "float.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.String, "String", "", "\"\"", new Color(0.271f, 0.576f, 0.925f), "String/XWInspectorPropertyEditorString.tscn", "String.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Color, "Color", default(Color), "Color.WHITE", new Color(0.373f, 1f, 0.592f), "Color/XWInspectorPropertyEditorColor.tscn", "Color.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.StringName, "StringName", default, "&\"\"", new Color(0.561f, 0.745f, 0.953f), "StringName/XWInspectorPropertyEditorStringName.tscn", "StringName.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.NodePath, "NodePath", default, "^\"\"", new Color(0.255f, 0.478f, 0.925f), "NodePath/XWInspectorPropertyEditorNodePath.tscn", "NodePath.svg"));
		RegisterHintEditor(2, LoadScene("Enum/XWInspectorPropertyEditorEnum.tscn"));
		RegisterHintEditor(18, LoadScene("Multiline/XWInspectorPropertyEditorMultiline.tscn"));
		RegisterHintEditor(6, LoadScene("Flags/XWInspectorPropertyEditorFlags.tscn"));
		RegisterHintEditor(7, LoadScene("Layers/XWInspectorPropertyEditorLayers.tscn"));
		RegisterHintEditor(8, LoadScene("Layers/XWInspectorPropertyEditorLayers.tscn"));
		RegisterHintEditor(9, LoadScene("Layers/XWInspectorPropertyEditorLayers.tscn"));
		RegisterHintEditor(10, LoadScene("Layers/XWInspectorPropertyEditorLayers.tscn"));
		RegisterHintEditor(11, LoadScene("Layers/XWInspectorPropertyEditorLayers.tscn"));
		RegisterHintEditor(12, LoadScene("Layers/XWInspectorPropertyEditorLayers.tscn"));
		RegisterHintEditor(4, LoadScene("Easing/XWInspectorPropertyEditorEasing.tscn"));
		RegisterHintEditor(23, LoadScene("ClassName/XWInspectorPropertyEditorClassName.tscn"));
		RegisterHintEditor(3, LoadScene("TextEnum/XWInspectorPropertyEditorTextEnum.tscn"));
		RegisterHintEditor(32, LoadScene("Locale/XWInspectorPropertyEditorLocale.tscn"));
		RegisterHintEditor(22, LoadScene("ObjectID/XWInspectorPropertyEditorObjectID.tscn"));
		RegisterHintEditor(13, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterHintEditor(44, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterHintEditor(14, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterHintEditor(15, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterHintEditor(16, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterHintEditor(27, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterHintEditor(28, LoadScene("Path/XWInspectorPropertyEditorPath.tscn"));
		RegisterEditor(LoadTypeData(Variant.Type.Vector2, "Vector2", Vector2.Zero, "Vector2.ZERO", new Color(0.675f, 0.451f, 0.945f), "Vector2/XWInspectorPropertyEditorVector2.tscn", "Vector2.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Vector2I, "Vector2i", Vector2I.Zero, "Vector2i.ZERO", new Color(0.675f, 0.451f, 0.945f), "Vector2i/XWInspectorPropertyEditorVector2i.tscn", "Vector2i.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Vector3, "Vector3", Vector3.Zero, "Vector3.ZERO", new Color(0.922f, 0.639f, 0.965f), "Vector3/XWInspectorPropertyEditorVector3.tscn", "Vector3.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Vector3I, "Vector3i", Vector3I.Zero, "Vector3i.ZERO", new Color(0.922f, 0.639f, 0.965f), "Vector3i/XWInspectorPropertyEditorVector3i.tscn", "Vector3i.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Vector4, "Vector4", default, "Vector4.ZERO", new Color(0.847f, 0.369f, 0.671f), "Vector4/XWInspectorPropertyEditorVector4.tscn", "Vector4.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Vector4I, "Vector4i", default, "Vector4i.ZERO", new Color(0.847f, 0.369f, 0.671f), "Vector4i/XWInspectorPropertyEditorVector4i.tscn", "Vector4i.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Rect2, "Rect2", default, "Rect2()", new Color(0.945f, 0.451f, 0.561f), "Rect2/XWInspectorPropertyEditorRect2.tscn", "Rect2.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Rect2I, "Rect2i", default, "Rect2i()", new Color(0.945f, 0.451f, 0.561f), "Rect2i/XWInspectorPropertyEditorRect2i.tscn", "Rect2i.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Plane, "Plane", default, "Plane()", new Color(0.969f, 0.286f, 0.286f), "Plane/XWInspectorPropertyEditorPlane.tscn", "Plane.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Quaternion, "Quaternion", default, "Quaternion.IDENTITY", new Color(0.953f, 0.553f, 0.733f), "Quaternion/XWInspectorPropertyEditorQuaternion.tscn", "Quaternion.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Aabb, "AABB", default, "AABB()", new Color(0.949f, 0.49f, 0.592f), "AABB/XWInspectorPropertyEditorAABB.tscn", "AABB.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Basis, "Basis", default, "Basis.IDENTITY", new Color(0.882f, 0.925f, 0.255f), "Basis/XWInspectorPropertyEditorBasis.tscn", "Basis.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Transform2D, "Transform2D", default, "Transform2D.IDENTITY", new Color(0.835f, 0.953f, 0.553f), "Transform2D/XWInspectorPropertyEditorTransform2D.tscn", "Transform2D.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Transform3D, "Transform3D", default, "Transform3D.IDENTITY", new Color(0.976f, 0.737f, 0.561f), "Transform3D/XWInspectorPropertyEditorTransform3D.tscn", "Transform3D.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Projection, "Projection", default, "Projection()", new Color(0.8f, 0.667f, 0.965f), "Projection/XWInspectorPropertyEditorProjection.tscn", "Projection.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Nil, "Variant", default, "null", new Color(0.282f, 0.929f, 0.69f), "Variant/XWInspectorPropertyEditorVariant.tscn", "Nil.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Object, "Object", default, "null", new Color(0.878f, 0.878f, 0.878f), "Object/XWInspectorPropertyEditorObject.tscn", "Object.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Array, "Array", default, "[]", new Color(0.878f, 0.878f, 0.878f), "Array/XWInspectorPropertyEditorArray.tscn", "Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Dictionary, "Dictionary", default, "{}", new Color(0.302f, 0.824f, 0.557f), "Dictionary/XWInspectorPropertyEditorDictionary.tscn", "Dictionary.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedByteArray, "PackedByteArray", default, "PackedByteArray()", new Color(0.624f, 1f, 0.753f), "PackedByteArray/XWInspectorPropertyEditorPackedByteArray.tscn", "PackedByteArray.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedInt32Array, "PackedInt32Array", default, "PackedInt32Array()", new Color(0.612f, 0.839f, 0.961f), "PackedInt32Array/XWInspectorPropertyEditorPackedInt32Array.tscn", "PackedInt32Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedInt64Array, "PackedInt64Array", default, "PackedInt64Array()", new Color(0.612f, 0.839f, 0.961f), "PackedInt64Array/XWInspectorPropertyEditorPackedInt64Array.tscn", "PackedInt64Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedFloat32Array, "PackedFloat32Array", default, "PackedFloat32Array()", new Color(0.522f, 0.898f, 0.973f), "PackedFloat32Array/XWInspectorPropertyEditorPackedFloat32Array.tscn", "PackedFloat32Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedFloat64Array, "PackedFloat64Array", default, "PackedFloat64Array()", new Color(0.522f, 0.898f, 0.973f), "PackedFloat64Array/XWInspectorPropertyEditorPackedFloat64Array.tscn", "PackedFloat64Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedStringArray, "PackedStringArray", default, "PackedStringArray()", new Color(0.561f, 0.745f, 0.953f), "PackedStringArray/XWInspectorPropertyEditorPackedStringArray.tscn", "PackedStringArray.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedVector2Array, "PackedVector2Array", default, "PackedVector2Array()", new Color(0.804f, 0.671f, 0.965f), "PackedVector2Array/XWInspectorPropertyEditorPackedVector2Array.tscn", "PackedVector2Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedVector3Array, "PackedVector3Array", default, "PackedVector3Array()", new Color(0.855f, 0.584f, 0.894f), "PackedVector3Array/XWInspectorPropertyEditorPackedVector3Array.tscn", "PackedVector3Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedColorArray, "PackedColorArray", default, "PackedColorArray()", new Color(0.502f, 1f, 0.271f), "PackedColorArray/XWInspectorPropertyEditorPackedColorArray.tscn", "PackedColorArray.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.PackedVector4Array, "PackedVector4Array", default, "PackedVector4Array()", new Color(0.8f, 0.667f, 0.965f), "PackedVector4Array/XWInspectorPropertyEditorPackedVector4Array.tscn", "PackedVector4Array.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Callable, "Callable", default, "Callable()", new Color(0.7f, 0.8f, 0.6f), "Callable/XWInspectorPropertyEditorCallable.tscn", "Callable.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Rid, "RID", default, "RID()", new Color(0.255f, 0.925f, 0.502f), "RID/XWInspectorPropertyEditorRID.tscn", "RID.svg"));
		RegisterEditor(LoadTypeData(Variant.Type.Signal, "Signal", default, "Signal()", new Color(1f, 0.373f, 0.373f), "Signal/XWInspectorPropertyEditorSignal.tscn", "Signal.svg"));
		_resourcePropertyEditorScene = LoadScene("Resource/XWInspectorPropertyEditorResource.tscn");
	}

	private XWTypeData LoadTypeData(Variant.Type type, string name, Variant defaultValue, string defaultValueString, Color color, string sceneRelativePath, string iconFileName = null)
	{
		PackedScene editor = LoadScene(sceneRelativePath);
		Texture2D icon = LoadTypeIcon(iconFileName);
		return new XWTypeData(type, name, defaultValue, defaultValueString, color, editor, icon);
	}

	private static PackedScene LoadScene(string sceneRelativePath)
	{
		string path = "res://addons/ModEditor/Inspector/GUI/Editor/" + sceneRelativePath;
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse);
	}

	private static Texture2D LoadTypeIcon(string iconFileName)
	{
		if (string.IsNullOrEmpty(iconFileName))
		{
			return null;
		}
		string path = "res://addons/ModEditor/Icons/TypeIcon/" + iconFileName;
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	public void RegisterEditor(XWTypeData data)
	{
		_typeDictionary[data.Type] = data;
	}

	public void RegisterHintEditor(int hint, PackedScene editor)
	{
		_hintEditorDictionary[hint] = editor;
	}

	public XWInspectorPropertyEditorBase GetHintEditor(int hint)
	{
		if (_hintEditorDictionary.TryGetValue(hint, out var value) && value != null)
		{
			return value.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
		}
		return null;
	}

	public XWInspectorPropertyEditorBase GetEditorFromObjectProperty(GodotObject obj, StringName propertyName = null, int hint = 0, string hintString = "")
	{
		Variant variant = ReadPropertyValue(obj, propertyName);
		if (IsContainerValueType(variant.VariantType))
		{
			XWInspectorPropertyEditorBase typeEditor = GetTypeEditor(variant.VariantType);
			if (GodotObject.IsInstanceValid(typeEditor))
			{
				return typeEditor;
			}
		}
		XWInspectorPropertyEditorBase hintEditor = GetHintEditor(hint);
		if (GodotObject.IsInstanceValid(hintEditor))
		{
			return hintEditor;
		}
		if (hint == 17)
		{
			if (variant.VariantType == Variant.Type.Object && variant.As<GodotObject>() is Resource resObj)
			{
				return GetResourceValueEditor(resObj);
			}
			return GetResourcePropertyEditor();
		}
		if (propertyName != (StringName)"" && variant.VariantType != Variant.Type.Object)
		{
			XWInspectorPropertyEditorBase typeEditor2 = GetTypeEditor(variant.VariantType);
			if (GodotObject.IsInstanceValid(typeEditor2))
			{
				if (hint == 1)
				{
					typeEditor2.SetupRangeHint(hintString);
				}
				return typeEditor2;
			}
		}
		if (variant.VariantType == Variant.Type.Object)
		{
			GodotObject godotObject = variant.As<GodotObject>();
			if (GodotObject.IsInstanceValid(godotObject) && godotObject is Resource)
			{
				return GetResourceValueEditor((Resource)godotObject);
			}
		}
		PackedScene editor = XWInspectorPropertyRegistry.GetEditor(obj);
		if (editor != null)
		{
			return editor.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
		}
		if (propertyName != (StringName)"" && variant.VariantType == Variant.Type.Object)
		{
			XWInspectorPropertyEditorBase typeEditor3 = GetTypeEditor(Variant.Type.Object);
			if (GodotObject.IsInstanceValid(typeEditor3))
			{
				return typeEditor3;
			}
		}
		return null;
	}

	private static Variant ReadPropertyValue(GodotObject obj, StringName propertyName)
	{
		if (propertyName != null && propertyName != (StringName)"" && GodotObject.IsInstanceValid(obj))
		{
			return obj.Get(propertyName);
		}
		return default;
	}

	private static bool IsContainerValueType(Variant.Type valueType)
	{
		if (valueType >= Variant.Type.Array)
		{
			return valueType <= Variant.Type.PackedVector4Array;
		}
		return false;
	}

	public XWInspectorPropertyEditorBase GetResourcePropertyEditor()
	{
		if (_resourcePropertyEditorScene == null)
		{
			return GetTypeEditor(Variant.Type.Object);
		}
		return _resourcePropertyEditorScene.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
	}

	public XWInspectorPropertyEditorBase GetResourceValueEditor(Resource resObj)
	{
		if (GodotObject.IsInstanceValid(resObj))
		{
			PackedScene editor = XWInspectorPropertyRegistry.GetEditor(resObj);
			if (editor != null)
			{
				return editor.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
			}
		}
		return GetResourcePropertyEditor();
	}

	public XWInspectorPropertyEditorBase GetTypeEditor(Variant.Type type)
	{
		if (_typeDictionary.TryGetValue(type, out var value) && value?.Editor != null)
		{
			return value.Editor.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
		}
		return null;
	}

	public bool HasType(Variant.Type type)
	{
		return _typeDictionary.ContainsKey(type);
	}

	public string GetTypeName(Variant.Type type)
	{
		if (!_typeDictionary.TryGetValue(type, out var value))
		{
			return "";
		}
		return value.Name;
	}

	public Variant GetTypeDefaultValue(Variant.Type type)
	{
		if (!_typeDictionary.TryGetValue(type, out var value))
		{
			return default;
		}
		return value.DefaultValue;
	}

	public string GetTypeDefaultValueString(Variant.Type type)
	{
		if (!_typeDictionary.TryGetValue(type, out var value))
		{
			return "";
		}
		return value.DefaultValueString;
	}

	public Color GetTypeColor(Variant.Type type)
	{
		if (!_typeDictionary.TryGetValue(type, out var value))
		{
			return Colors.Gray;
		}
		return value.Color;
	}

	public Texture2D GetTypeIcon(Variant.Type type)
	{
		if (!_typeDictionary.TryGetValue(type, out var value))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(value.Icon);
	}

	public IReadOnlyCollection<XWTypeData> GetAllType()
	{
		return _typeDictionary.Values;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadTypeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.String, "defaultValueString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sceneRelativePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconFileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sceneRelativePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadTypeIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "iconFileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterHintEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetHintEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEditorFromObjectProperty, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadPropertyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsContainerValueType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "valueType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetResourcePropertyEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetResourceValueEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resObj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeDefaultValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeDefaultValueString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Initialize && args.Count == 0)
		{
			Initialize();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadTypeData && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<XWTypeData>(LoadTypeData(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<Color>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]), VariantUtils.ConvertTo<string>(in args[6])));
			return true;
		}
		if (method == MethodName.LoadScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTypeIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterEditor && args.Count == 1)
		{
			RegisterEditor(VariantUtils.ConvertTo<XWTypeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterHintEditor && args.Count == 2)
		{
			RegisterHintEditor(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetHintEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(GetHintEditor(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEditorFromObjectProperty && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(GetEditorFromObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<string>(in args[3])));
			return true;
		}
		if (method == MethodName.ReadPropertyValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadPropertyValue(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.IsContainerValueType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsContainerValueType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetResourcePropertyEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(GetResourcePropertyEditor());
			return true;
		}
		if (method == MethodName.GetResourceValueEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(GetResourceValueEditor(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(GetTypeEditor(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.HasType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTypeName(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeDefaultValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetTypeDefaultValue(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeDefaultValueString && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTypeDefaultValueString(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetTypeColor(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetTypeIcon(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadTypeIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadPropertyValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadPropertyValue(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.IsContainerValueType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsContainerValueType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.LoadTypeData)
		{
			return true;
		}
		if (method == MethodName.LoadScene)
		{
			return true;
		}
		if (method == MethodName.LoadTypeIcon)
		{
			return true;
		}
		if (method == MethodName.RegisterEditor)
		{
			return true;
		}
		if (method == MethodName.RegisterHintEditor)
		{
			return true;
		}
		if (method == MethodName.GetHintEditor)
		{
			return true;
		}
		if (method == MethodName.GetEditorFromObjectProperty)
		{
			return true;
		}
		if (method == MethodName.ReadPropertyValue)
		{
			return true;
		}
		if (method == MethodName.IsContainerValueType)
		{
			return true;
		}
		if (method == MethodName.GetResourcePropertyEditor)
		{
			return true;
		}
		if (method == MethodName.GetResourceValueEditor)
		{
			return true;
		}
		if (method == MethodName.GetTypeEditor)
		{
			return true;
		}
		if (method == MethodName.HasType)
		{
			return true;
		}
		if (method == MethodName.GetTypeName)
		{
			return true;
		}
		if (method == MethodName.GetTypeDefaultValue)
		{
			return true;
		}
		if (method == MethodName.GetTypeDefaultValueString)
		{
			return true;
		}
		if (method == MethodName.GetTypeColor)
		{
			return true;
		}
		if (method == MethodName.GetTypeIcon)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._isInit)
		{
			_isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resourcePropertyEditorScene)
		{
			_resourcePropertyEditorScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._isInit)
		{
			value = VariantUtils.CreateFrom(in _isInit);
			return true;
		}
		if (name == PropertyName._resourcePropertyEditorScene)
		{
			value = VariantUtils.CreateFrom(in _resourcePropertyEditorScene);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePropertyEditorScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._isInit, Variant.From(in _isInit));
		info.AddProperty(PropertyName._resourcePropertyEditorScene, Variant.From(in _resourcePropertyEditorScene));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._isInit, out var value))
		{
			_isInit = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resourcePropertyEditorScene, out var value2))
		{
			_resourcePropertyEditorScene = value2.As<PackedScene>();
		}
	}
}
