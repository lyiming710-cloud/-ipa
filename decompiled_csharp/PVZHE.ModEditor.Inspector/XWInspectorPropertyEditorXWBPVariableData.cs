using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.Registry;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/XWBPVariableData/XWInspectorPropertyEditorXWBPVariableData.cs")]
public class XWInspectorPropertyEditorXWBPVariableData : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName OnDefaultValueChange = "OnDefaultValueChange";

		public static readonly StringName OnTypeButtonPressed = "OnTypeButtonPressed";

		public static readonly StringName OnTypeSelected = "OnTypeSelected";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _variableTypeButton = "_variableTypeButton";

		public static readonly StringName _variableDefaultContainer = "_variableDefaultContainer";

		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName _typeSelectorWindow = "_typeSelectorWindow";

		public static readonly StringName _editor = "_editor";

		public static readonly StringName _currentType = "_currentType";

		public static readonly StringName _variableData = "_variableData";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/ResourceInspectorExtend/XWBPVariableData/XWInspectorPropertyEditorXWBPVariableData.tscn";

	private Button _variableTypeButton;

	private HBoxContainer _variableDefaultContainer;

	private PanelContainer _editorContainer;

	private XWWindowTypeSelector _typeSelectorWindow;

	private XWInspectorPropertyEditorBase _editor;

	private Variant.Type _currentType;

	private XWBPVariableData _variableData;

	public static XWInspectorPropertyEditorXWBPVariableData Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/ResourceInspectorExtend/XWBPVariableData/XWInspectorPropertyEditorXWBPVariableData.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorXWBPVariableData>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_variableTypeButton = GetNode<Button>("%VariableTypeButton");
		_variableDefaultContainer = GetNode<HBoxContainer>("%VariableDefaultContainer");
		_editorContainer = GetNode<PanelContainer>("%EditorContainer");
		_variableTypeButton.Pressed += OnTypeButtonPressed;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		_variableData = GetPropertyValue().As<XWBPVariableData>();
		Refresh();
	}

	public override void UpdateValue()
	{
		_variableData = GetPropertyValue().As<XWBPVariableData>();
		Refresh();
	}

	public override Variant GetValue()
	{
		return _variableData;
	}

	private void Refresh()
	{
		if (!GodotObject.IsInstanceValid(_variableData))
		{
			return;
		}
		if (_variableData.Type == Variant.Type.Object && XWClassRegistry.Instance.HasClass(_variableData.ClassName))
		{
			_variableTypeButton.Icon = XWClassRegistry.Instance.GetClassIcon(_variableData.ClassName);
			_variableTypeButton.Text = _variableData.ClassName;
		}
		else if (XWTypeRegistry.Instance.HasType(_variableData.Type))
		{
			_variableTypeButton.Icon = XWTypeRegistry.Instance.GetTypeIcon(_variableData.Type);
			_variableTypeButton.Text = XWTypeRegistry.Instance.GetTypeName(_variableData.Type);
		}
		if (_currentType == _variableData.Type)
		{
			return;
		}
		_currentType = _variableData.Type;
		_variableDefaultContainer.Visible = false;
		if (GodotObject.IsInstanceValid(_editor))
		{
			_editor.QueueFree();
			_editor = null;
		}
		if (XWTypeRegistry.Instance.HasType(_currentType) && _variableData.Type != Variant.Type.Nil)
		{
			_variableDefaultContainer.Visible = true;
			XWInspectorProperty property = new XWInspectorProperty(_variableData, "defaultValue", XWTypeRegistry.Instance.GetTypeDefaultValue(_currentType), null, null, PropertyHint.None);
			_editor = XWTypeRegistry.Instance.GetTypeEditor(_currentType);
			if (GodotObject.IsInstanceValid(_editor))
			{
				_editor.IsInner = true;
				_editor.PropertyNameLock = true;
				_editor.ValueChanged += OnDefaultValueChange;
				_editorContainer.AddChild(_editor, forceReadableName: false, InternalMode.Disabled);
				_editor.SetEditProperty(property);
				_editor.PropertyNameLabel.Text = "默认值";
			}
		}
	}

	private void OnDefaultValueChange(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (GodotObject.IsInstanceValid(_variableData))
		{
			_variableData.DefaultValue = value;
		}
	}

	private void OnTypeButtonPressed()
	{
		_typeSelectorWindow = XWWindowTypeSelector.Create();
		_typeSelectorWindow.TypeSelect += OnTypeSelected;
		AddChild(_typeSelectorWindow, forceReadableName: false, InternalMode.Disabled);
		_typeSelectorWindow.PopupCentered();
	}

	private void OnTypeSelected(Variant.Type type, StringName className)
	{
		if (GodotObject.IsInstanceValid(_variableData))
		{
			if (_currentType != type)
			{
				_variableData.DefaultValue = (XWTypeRegistry.Instance.HasType(type) ? XWTypeRegistry.Instance.GetTypeDefaultValue(type) : default(Variant));
			}
			_variableData.Type = type;
			_variableData.ClassName = className.ToString();
			Refresh();
			Property.SetCall();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDefaultValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTypeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorXWBPVariableData>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditProperty && args.Count == 2)
		{
			SetEditProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDefaultValueChange && args.Count == 4)
		{
			OnDefaultValueChange(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTypeButtonPressed && args.Count == 0)
		{
			OnTypeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTypeSelected && args.Count == 2)
		{
			OnTypeSelected(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorXWBPVariableData>(Create());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetEditProperty)
		{
			return true;
		}
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.OnDefaultValueChange)
		{
			return true;
		}
		if (method == MethodName.OnTypeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnTypeSelected)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._variableTypeButton)
		{
			_variableTypeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._variableDefaultContainer)
		{
			_variableDefaultContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			_editorContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._typeSelectorWindow)
		{
			_typeSelectorWindow = VariantUtils.ConvertTo<XWWindowTypeSelector>(in value);
			return true;
		}
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		if (name == PropertyName._currentType)
		{
			_currentType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._variableData)
		{
			_variableData = VariantUtils.ConvertTo<XWBPVariableData>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._variableTypeButton)
		{
			value = VariantUtils.CreateFrom(in _variableTypeButton);
			return true;
		}
		if (name == PropertyName._variableDefaultContainer)
		{
			value = VariantUtils.CreateFrom(in _variableDefaultContainer);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			value = VariantUtils.CreateFrom(in _editorContainer);
			return true;
		}
		if (name == PropertyName._typeSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _typeSelectorWindow);
			return true;
		}
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._currentType)
		{
			value = VariantUtils.CreateFrom(in _currentType);
			return true;
		}
		if (name == PropertyName._variableData)
		{
			value = VariantUtils.CreateFrom(in _variableData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._variableTypeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._variableDefaultContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._variableData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._variableTypeButton, Variant.From(in _variableTypeButton));
		info.AddProperty(PropertyName._variableDefaultContainer, Variant.From(in _variableDefaultContainer));
		info.AddProperty(PropertyName._editorContainer, Variant.From(in _editorContainer));
		info.AddProperty(PropertyName._typeSelectorWindow, Variant.From(in _typeSelectorWindow));
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._currentType, Variant.From(in _currentType));
		info.AddProperty(PropertyName._variableData, Variant.From(in _variableData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._variableTypeButton, out var value))
		{
			_variableTypeButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._variableDefaultContainer, out var value2))
		{
			_variableDefaultContainer = value2.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._editorContainer, out var value3))
		{
			_editorContainer = value3.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._typeSelectorWindow, out var value4))
		{
			_typeSelectorWindow = value4.As<XWWindowTypeSelector>();
		}
		if (info.TryGetProperty(PropertyName._editor, out var value5))
		{
			_editor = value5.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetProperty(PropertyName._currentType, out var value6))
		{
			_currentType = value6.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._variableData, out var value7))
		{
			_variableData = value7.As<XWBPVariableData>();
		}
	}
}
