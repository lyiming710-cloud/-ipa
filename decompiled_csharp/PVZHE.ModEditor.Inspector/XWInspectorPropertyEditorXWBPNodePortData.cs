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

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/XWBPNodePortData/XWInspectorPropertyEditorXWBPNodePortData.cs")]
public class XWInspectorPropertyEditorXWBPNodePortData : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName OnNameChange = "OnNameChange";

		public static readonly StringName OnTypeButtonPressed = "OnTypeButtonPressed";

		public static readonly StringName OnTypeSelected = "OnTypeSelected";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _nameLineEdit = "_nameLineEdit";

		public static readonly StringName _typeButton = "_typeButton";

		public static readonly StringName _typeSelectorWindow = "_typeSelectorWindow";

		public static readonly StringName _portData = "_portData";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/ResourceInspectorExtend/XWBPNodePortData/XWInspectorPropertyEditorXWBPNodePortData.tscn";

	private LineEdit _nameLineEdit;

	private Button _typeButton;

	private XWWindowTypeSelector _typeSelectorWindow;

	private XWBPNodePortData _portData;

	public static XWInspectorPropertyEditorXWBPNodePortData Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/ResourceInspectorExtend/XWBPNodePortData/XWInspectorPropertyEditorXWBPNodePortData.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorXWBPNodePortData>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_nameLineEdit = GetNode<LineEdit>("%LineEdit");
		_typeButton = GetNode<Button>("%TypeButton");
		_nameLineEdit.TextChanged += OnNameChange;
		_typeButton.Pressed += OnTypeButtonPressed;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		_portData = GetPropertyValue().As<XWBPNodePortData>();
		if (GodotObject.IsInstanceValid(_portData))
		{
			_nameLineEdit.Text = _portData.Name;
		}
		Refresh();
	}

	public override void UpdateValue()
	{
		_portData = GetPropertyValue().As<XWBPNodePortData>();
		if (GodotObject.IsInstanceValid(_portData))
		{
			_nameLineEdit.Text = _portData.Name;
		}
		Refresh();
	}

	public override Variant GetValue()
	{
		return _portData;
	}

	private void Refresh()
	{
		if (!GodotObject.IsInstanceValid(_portData))
		{
			return;
		}
		if (_portData.PortTypeValue == XWBPNodePortData.PortType.Object && XWClassRegistry.Instance.HasClass(_portData.ClassName.ToString()))
		{
			_typeButton.Icon = XWClassRegistry.Instance.GetClassIcon(_portData.ClassName.ToString());
			_typeButton.Text = _portData.ClassName.ToString();
			return;
		}
		Variant.Type type = (Variant.Type)_portData.PortTypeValue;
		if (XWTypeRegistry.Instance.HasType(type))
		{
			_typeButton.Icon = XWTypeRegistry.Instance.GetTypeIcon(type);
			_typeButton.Text = XWTypeRegistry.Instance.GetTypeName(type);
		}
	}

	private void OnNameChange(string newText)
	{
		if (GodotObject.IsInstanceValid(_portData))
		{
			_portData.Name = newText;
			Property.SetCall();
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
		if (GodotObject.IsInstanceValid(_portData))
		{
			_portData.PortTypeValue = (XWBPNodePortData.PortType)type;
			_portData.ClassName = className;
			Property.SetCall();
			Refresh();
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
			new MethodInfo(MethodName.OnNameChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorXWBPNodePortData>(Create());
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
		if (method == MethodName.OnNameChange && args.Count == 1)
		{
			OnNameChange(VariantUtils.ConvertTo<string>(in args[0]));
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorXWBPNodePortData>(Create());
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
		if (method == MethodName.OnNameChange)
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
		if (name == PropertyName._nameLineEdit)
		{
			_nameLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._typeButton)
		{
			_typeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._typeSelectorWindow)
		{
			_typeSelectorWindow = VariantUtils.ConvertTo<XWWindowTypeSelector>(in value);
			return true;
		}
		if (name == PropertyName._portData)
		{
			_portData = VariantUtils.ConvertTo<XWBPNodePortData>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._nameLineEdit)
		{
			value = VariantUtils.CreateFrom(in _nameLineEdit);
			return true;
		}
		if (name == PropertyName._typeButton)
		{
			value = VariantUtils.CreateFrom(in _typeButton);
			return true;
		}
		if (name == PropertyName._typeSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _typeSelectorWindow);
			return true;
		}
		if (name == PropertyName._portData)
		{
			value = VariantUtils.CreateFrom(in _portData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._nameLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._portData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nameLineEdit, Variant.From(in _nameLineEdit));
		info.AddProperty(PropertyName._typeButton, Variant.From(in _typeButton));
		info.AddProperty(PropertyName._typeSelectorWindow, Variant.From(in _typeSelectorWindow));
		info.AddProperty(PropertyName._portData, Variant.From(in _portData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nameLineEdit, out var value))
		{
			_nameLineEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._typeButton, out var value2))
		{
			_typeButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._typeSelectorWindow, out var value3))
		{
			_typeSelectorWindow = value3.As<XWWindowTypeSelector>();
		}
		if (info.TryGetProperty(PropertyName._portData, out var value4))
		{
			_portData = value4.As<XWBPNodePortData>();
		}
	}
}
