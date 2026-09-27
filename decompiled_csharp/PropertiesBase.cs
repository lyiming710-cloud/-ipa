using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Base/PropertiesBase.cs")]
public class PropertiesBase : PanelContainer
{
	public delegate void DeleteEventHandler(string propertyName, string key);

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName CanSetValue = "CanSetValue";

		public static readonly StringName _GetType = "_GetType";

		public static readonly StringName GetObjName = "GetObjName";

		public static readonly StringName GetPropertyNameString = "GetPropertyNameString";

		public static readonly StringName GetPropertyName = "GetPropertyName";

		public static readonly StringName GetKey = "GetKey";

		public static readonly StringName GetValue = "GetValue";

		public static readonly StringName CloseButtonPressed = "CloseButtonPressed";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _closeButton = "_closeButton";

		public static readonly StringName inspector = "inspector";

		public static readonly StringName container = "container";

		public static readonly StringName obj = "obj";

		public static readonly StringName propertyName = "propertyName";

		public static readonly StringName propertiesBox = "propertiesBox";

		public static readonly StringName key = "key";

		public static readonly StringName path = "path";

		public static readonly StringName isTargetObj = "isTargetObj";

		public static readonly StringName showCloseButton = "showCloseButton";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Button _closeButton;

	public LevelEditorInspector inspector;

	public LevelEditorPropertiesContainer container;

	public GodotObject obj;

	public string propertyName = "";

	public PropertiesBox propertiesBox;

	public string key;

	public string path = "";

	public bool isTargetObj = true;

	public bool showCloseButton = true;

	public event DeleteEventHandler OnDelete;

	public override void _Ready()
	{
		_closeButton = GetNode<Button>("%CloseButton");
		_closeButton.Pressed += CloseButtonPressed;
	}

	public override void _Process(double delta)
	{
		if (Visible && propertiesBox != null && obj != null && propertyName != "" && CanSetValue())
		{
			Variant value = GetValue();
			propertiesBox.set_value(key, value);
			if (_closeButton != null)
			{
				_closeButton.Visible = showCloseButton && !isTargetObj;
			}
		}
	}

	public virtual bool CanSetValue()
	{
		return true;
	}

	public virtual string _GetType()
	{
		return "";
	}

	public virtual string GetObjName()
	{
		if (!isTargetObj)
		{
			return "";
		}
		return obj.Get("name").AsString();
	}

	public string GetPropertyNameString()
	{
		Variant indexed = obj.GetIndexed(propertyName);
		if (isTargetObj)
		{
			return key;
		}
		if (indexed.VariantType == Variant.Type.Dictionary)
		{
			return propertyName + "[" + key + "]";
		}
		if (indexed.VariantType == Variant.Type.Array)
		{
			return propertyName + "[" + key + "]";
		}
		return "";
	}

	public string GetPropertyName()
	{
		return propertyName;
	}

	public string GetKey()
	{
		return key;
	}

	public Variant GetValue()
	{
		Variant indexed = obj.GetIndexed(propertyName);
		if (isTargetObj)
		{
			return indexed;
		}
		if (indexed.VariantType == Variant.Type.Dictionary)
		{
			return indexed.AsGodotDictionary()[key];
		}
		if (indexed.VariantType == Variant.Type.Array)
		{
			return indexed.AsGodotArray()[key.ToInt()];
		}
		return default;
	}

	public void CloseButtonPressed()
	{
		if (!isTargetObj)
		{
			Variant indexed = obj.GetIndexed(propertyName);
			if (indexed.VariantType == Variant.Type.Dictionary)
			{
				indexed.AsGodotDictionary().Remove(key);
				OnDelete?.Invoke(propertyName, key);
			}
			if (indexed.VariantType == Variant.Type.Array)
			{
				indexed.AsGodotArray().RemoveAt(key.ToInt());
				OnDelete?.Invoke(propertyName, key.ToInt().ToString());
			}
			propertiesBox.delete_value(key);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSetValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetObjName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPropertyNameString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPropertyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.CanSetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSetValue());
			return true;
		}
		if (method == MethodName._GetType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetType());
			return true;
		}
		if (method == MethodName.GetObjName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjName());
			return true;
		}
		if (method == MethodName.GetPropertyNameString && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetPropertyNameString());
			return true;
		}
		if (method == MethodName.GetPropertyName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetPropertyName());
			return true;
		}
		if (method == MethodName.GetKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetKey());
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.CloseButtonPressed && args.Count == 0)
		{
			CloseButtonPressed();
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
		if (method == MethodName.CanSetValue)
		{
			return true;
		}
		if (method == MethodName._GetType)
		{
			return true;
		}
		if (method == MethodName.GetObjName)
		{
			return true;
		}
		if (method == MethodName.GetPropertyNameString)
		{
			return true;
		}
		if (method == MethodName.GetPropertyName)
		{
			return true;
		}
		if (method == MethodName.GetKey)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.CloseButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._closeButton)
		{
			_closeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.inspector)
		{
			inspector = VariantUtils.ConvertTo<LevelEditorInspector>(in value);
			return true;
		}
		if (name == PropertyName.container)
		{
			container = VariantUtils.ConvertTo<LevelEditorPropertiesContainer>(in value);
			return true;
		}
		if (name == PropertyName.obj)
		{
			obj = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName.propertyName)
		{
			propertyName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.propertiesBox)
		{
			propertiesBox = VariantUtils.ConvertTo<PropertiesBox>(in value);
			return true;
		}
		if (name == PropertyName.key)
		{
			key = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.path)
		{
			path = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.isTargetObj)
		{
			isTargetObj = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showCloseButton)
		{
			showCloseButton = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._closeButton)
		{
			value = VariantUtils.CreateFrom(in _closeButton);
			return true;
		}
		if (name == PropertyName.inspector)
		{
			value = VariantUtils.CreateFrom(in inspector);
			return true;
		}
		if (name == PropertyName.container)
		{
			value = VariantUtils.CreateFrom(in container);
			return true;
		}
		if (name == PropertyName.obj)
		{
			value = VariantUtils.CreateFrom(in obj);
			return true;
		}
		if (name == PropertyName.propertyName)
		{
			value = VariantUtils.CreateFrom(in propertyName);
			return true;
		}
		if (name == PropertyName.propertiesBox)
		{
			value = VariantUtils.CreateFrom(in propertiesBox);
			return true;
		}
		if (name == PropertyName.key)
		{
			value = VariantUtils.CreateFrom(in key);
			return true;
		}
		if (name == PropertyName.path)
		{
			value = VariantUtils.CreateFrom(in path);
			return true;
		}
		if (name == PropertyName.isTargetObj)
		{
			value = VariantUtils.CreateFrom(in isTargetObj);
			return true;
		}
		if (name == PropertyName.showCloseButton)
		{
			value = VariantUtils.CreateFrom(in showCloseButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._closeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.container, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.obj, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.propertyName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propertiesBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.key, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.path, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isTargetObj, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showCloseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._closeButton, Variant.From(in _closeButton));
		info.AddProperty(PropertyName.inspector, Variant.From(in inspector));
		info.AddProperty(PropertyName.container, Variant.From(in container));
		info.AddProperty(PropertyName.obj, Variant.From(in obj));
		info.AddProperty(PropertyName.propertyName, Variant.From(in propertyName));
		info.AddProperty(PropertyName.propertiesBox, Variant.From(in propertiesBox));
		info.AddProperty(PropertyName.key, Variant.From(in key));
		info.AddProperty(PropertyName.path, Variant.From(in path));
		info.AddProperty(PropertyName.isTargetObj, Variant.From(in isTargetObj));
		info.AddProperty(PropertyName.showCloseButton, Variant.From(in showCloseButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._closeButton, out var value))
		{
			_closeButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.inspector, out var value2))
		{
			inspector = value2.As<LevelEditorInspector>();
		}
		if (info.TryGetProperty(PropertyName.container, out var value3))
		{
			container = value3.As<LevelEditorPropertiesContainer>();
		}
		if (info.TryGetProperty(PropertyName.obj, out var value4))
		{
			obj = value4.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName.propertyName, out var value5))
		{
			propertyName = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.propertiesBox, out var value6))
		{
			propertiesBox = value6.As<PropertiesBox>();
		}
		if (info.TryGetProperty(PropertyName.key, out var value7))
		{
			key = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.path, out var value8))
		{
			path = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.isTargetObj, out var value9))
		{
			isTargetObj = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showCloseButton, out var value10))
		{
			showCloseButton = value10.As<bool>();
		}
	}
}
