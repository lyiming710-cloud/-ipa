using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Object/XWInspectorPropertyEditorObject.cs")]
public class XWInspectorPropertyEditorObject : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public static readonly StringName SetupResourceEditor = "SetupResourceEditor";

		public static readonly StringName SetupObjectEditor = "SetupObjectEditor";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName UpdateResourceDisplay = "UpdateResourceDisplay";

		public static readonly StringName OnFoldButtonPressed = "OnFoldButtonPressed";

		public static readonly StringName ShowSubInspector = "ShowSubInspector";

		public static readonly StringName HideSubInspector = "HideSubInspector";

		public static readonly StringName OnResourceChanged = "OnResourceChanged";

		public static readonly StringName OnResourceSelected = "OnResourceSelected";

		public new static readonly StringName _Notification = "_Notification";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _objectLabel = "_objectLabel";

		public static readonly StringName _resourcePicker = "_resourcePicker";

		public static readonly StringName _subInspector = "_subInspector";

		public static readonly StringName _foldButton = "_foldButton";

		public static readonly StringName _isFolded = "_isFolded";

		public static readonly StringName _baseType = "_baseType";

		public static readonly StringName _subInspectorContainer = "_subInspectorContainer";

		public static readonly StringName _isResource = "_isResource";

		public static readonly StringName _pickerContainer = "_pickerContainer";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private static readonly Texture2D IconArrowClose = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GuiTreeArrowRight.svg", null, ResourceLoader.CacheMode.Reuse));

	private static readonly Texture2D IconArrowOpen = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GuiTreeArrowDown.svg", null, ResourceLoader.CacheMode.Reuse));

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Object/XWInspectorPropertyEditorObject.tscn";

	private Label _objectLabel;

	private XWResourcePicker _resourcePicker;

	private XWInspector _subInspector;

	private Button _foldButton;

	private bool _isFolded = true;

	private string _baseType = "";

	private VBoxContainer _subInspectorContainer;

	private bool _isResource;

	private HBoxContainer _pickerContainer;

	public static XWInspectorPropertyEditorObject Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Object/XWInspectorPropertyEditorObject.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorObject>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_objectLabel = GetNode<Label>("%ObjectLabel");
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		Variant variant = default;
		if (property.PropName != null && property.PropName != (StringName)"" && GodotObject.IsInstanceValid(property.Object))
		{
			variant = XWInspectorPropertyEditorBase.ReadObjectProperty(property.Object, property.PropName);
		}
		_isResource = variant.VariantType == Variant.Type.Object && variant.As<GodotObject>() is Resource;
		if (property.Hint == PropertyHint.ResourceType)
		{
			_isResource = true;
			_baseType = property.HintString;
			if (string.IsNullOrEmpty(_baseType))
			{
				_baseType = "Resource";
			}
		}
		if (_isResource)
		{
			IsResourceEditor = true;
		}
		base.SetEditProperty(property, field);
		if (_isResource)
		{
			SetupResourceEditor();
		}
		else
		{
			SetupObjectEditor();
		}
	}

	private void SetupResourceEditor()
	{
		_objectLabel.Visible = false;
		if (!GodotObject.IsInstanceValid(_pickerContainer))
		{
			_pickerContainer = new HBoxContainer
			{
				Name = "PickerContainer",
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			_foldButton = new Button
			{
				Icon = IconArrowClose,
				CustomMinimumSize = new Vector2(20f, 24f),
				Flat = true,
				TooltipText = "展开/折叠资源属性"
			};
			_foldButton.Pressed += OnFoldButtonPressed;
			_pickerContainer.AddChild(_foldButton, forceReadableName: false, InternalMode.Disabled);
			_resourcePicker = XWResourcePicker.Create();
			_resourcePicker.Name = "ResourcePicker";
			_resourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			_resourcePicker.ResourceChanged += OnResourceChanged;
			_resourcePicker.ResourceSelected += OnResourceSelected;
			_pickerContainer.AddChild(_resourcePicker, forceReadableName: false, InternalMode.Disabled);
			PropertyEditor.AddChild(_pickerContainer, forceReadableName: false, InternalMode.Disabled);
			NormalizePropertyEditorLayout();
		}
		if (GodotObject.IsInstanceValid(PropertyEditor))
		{
			PropertyEditor.Visible = true;
		}
		_pickerContainer.Visible = true;
		_resourcePicker.Setup(string.IsNullOrEmpty(_baseType) ? "Resource" : _baseType);
		UpdateResourceDisplay();
	}

	private void SetupObjectEditor()
	{
		if (GodotObject.IsInstanceValid(_pickerContainer))
		{
			_pickerContainer.Visible = false;
		}
		if (GodotObject.IsInstanceValid(PropertyEditor))
		{
			PropertyEditor.Visible = true;
		}
		_objectLabel.Visible = true;
		UpdateValue();
	}

	public override void UpdateValue()
	{
		if (_isResource)
		{
			UpdateResourceDisplay();
			return;
		}
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Object && GodotObject.IsInstanceValid(propertyValue.As<GodotObject>()))
		{
			GodotObject godotObject = propertyValue.As<GodotObject>();
			Variant script = godotObject.GetScript();
			string text = godotObject.GetClass();
			GodotObject godotObject2 = script.As<GodotObject>();
			if (GodotObject.IsInstanceValid(godotObject2) && godotObject2 is Script script2)
			{
				string text2 = script2.GetGlobalName();
				if (text2 != "")
				{
					text = text2 + " (" + godotObject.GetClass() + ")";
				}
			}
			_objectLabel.Text = text;
			_objectLabel.TooltipText = $"类: {godotObject.GetClass()}\n实例ID: {godotObject.GetInstanceId()}";
		}
		else
		{
			_objectLabel.Text = "<null>";
			_objectLabel.TooltipText = "";
		}
	}

	public override Variant GetValue()
	{
		return GetPropertyValue();
	}

	private void UpdateResourceDisplay()
	{
		if (!GodotObject.IsInstanceValid(_resourcePicker))
		{
			return;
		}
		Variant propertyValue = GetPropertyValue();
		Resource resource = ((propertyValue.VariantType == Variant.Type.Object && propertyValue.As<GodotObject>() is Resource resource2) ? resource2 : null);
		_resourcePicker.SetEditedResource(resource);
		if (GodotObject.IsInstanceValid(resource))
		{
			if (GodotObject.IsInstanceValid(_foldButton))
			{
				_foldButton.Visible = true;
				_foldButton.Icon = (_isFolded ? IconArrowClose : IconArrowOpen);
			}
			if (!_isFolded)
			{
				ShowSubInspector(resource);
			}
		}
		else
		{
			if (GodotObject.IsInstanceValid(_foldButton))
			{
				_foldButton.Visible = false;
			}
			HideSubInspector();
		}
	}

	private void OnFoldButtonPressed()
	{
		_isFolded = !_isFolded;
		if (GodotObject.IsInstanceValid(_foldButton))
		{
			_foldButton.Icon = (_isFolded ? IconArrowClose : IconArrowOpen);
		}
		if (!_isFolded)
		{
			Variant propertyValue = GetPropertyValue();
			Resource resource = ((propertyValue.VariantType == Variant.Type.Object && propertyValue.As<GodotObject>() is Resource resource2) ? resource2 : null);
			if (GodotObject.IsInstanceValid(resource))
			{
				ShowSubInspector(resource);
			}
		}
		else
		{
			HideSubInspector();
		}
	}

	private void ShowSubInspector(Resource res)
	{
		if (!GodotObject.IsInstanceValid(_subInspector))
		{
			_subInspectorContainer = new VBoxContainer
			{
				Name = "SubInspectorContainer",
				Visible = false
			};
			_subInspectorContainer.AddThemeConstantOverride("separation", 0);
			HBoxContainer hBoxContainer = new HBoxContainer
			{
				Name = "IndentContainer"
			};
			ColorRect node = new ColorRect
			{
				CustomMinimumSize = new Vector2(12f, 0f),
				Color = new Color(0.04f, 0.54f, 1f, 0.12f)
			};
			hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			MarginContainer marginContainer = new MarginContainer();
			marginContainer.AddThemeConstantOverride("margin_left", 16);
			marginContainer.AddThemeConstantOverride("margin_right", 4);
			marginContainer.AddThemeConstantOverride("margin_top", 2);
			marginContainer.AddThemeConstantOverride("margin_bottom", 2);
			marginContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			_subInspector = XWInspector.CreateSubInspector();
			_subInspector.Name = "SubInspector";
			_subInspector.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			marginContainer.AddChild(_subInspector, forceReadableName: false, InternalMode.Disabled);
			hBoxContainer.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
			_subInspectorContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		}
		VBoxContainer vBoxContainer = GetParent() as VBoxContainer;
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			if (!_subInspectorContainer.IsInsideTree())
			{
				int num = vBoxContainer.GetChildren().IndexOf(this);
				vBoxContainer.AddChild(_subInspectorContainer, forceReadableName: false, InternalMode.Disabled);
				vBoxContainer.MoveChild(_subInspectorContainer, num + 1);
			}
			_subInspectorContainer.Visible = true;
			_subInspector.SetObject(res);
		}
	}

	private void HideSubInspector()
	{
		if (GodotObject.IsInstanceValid(_subInspectorContainer))
		{
			_subInspectorContainer.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_subInspector))
		{
			_subInspector.SetObject(null);
		}
	}

	private void OnResourceChanged(Resource resource)
	{
		ValueChange(resource);
		if (GodotObject.IsInstanceValid(resource) && !_isFolded)
		{
			ShowSubInspector(resource);
		}
		else if (!GodotObject.IsInstanceValid(resource))
		{
			HideSubInspector();
		}
	}

	private void OnResourceSelected(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			_isFolded = false;
			if (GodotObject.IsInstanceValid(_foldButton))
			{
				_foldButton.Icon = IconArrowOpen;
			}
			ShowSubInspector(resource);
			XWInspector inspector = GetInspector();
			if (GodotObject.IsInstanceValid(inspector))
			{
				inspector.EmitSignal(XWInspector.SignalName.ResourceSelected, resource);
			}
		}
	}

	public override void _Notification(int what)
	{
		if ((long)what == 1 && GodotObject.IsInstanceValid(_subInspectorContainer) && _subInspectorContainer.IsInsideTree())
		{
			_subInspectorContainer.QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupResourceEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupObjectEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateResourceDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFoldButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowSubInspector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "res", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HideSubInspector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnResourceChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorObject>(Create());
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
		if (method == MethodName.SetupResourceEditor && args.Count == 0)
		{
			SetupResourceEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupObjectEditor && args.Count == 0)
		{
			SetupObjectEditor();
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
		if (method == MethodName.UpdateResourceDisplay && args.Count == 0)
		{
			UpdateResourceDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFoldButtonPressed && args.Count == 0)
		{
			OnFoldButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowSubInspector && args.Count == 1)
		{
			ShowSubInspector(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HideSubInspector && args.Count == 0)
		{
			HideSubInspector();
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceChanged && args.Count == 1)
		{
			OnResourceChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceSelected && args.Count == 1)
		{
			OnResourceSelected(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorObject>(Create());
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
		if (method == MethodName.SetupResourceEditor)
		{
			return true;
		}
		if (method == MethodName.SetupObjectEditor)
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
		if (method == MethodName.UpdateResourceDisplay)
		{
			return true;
		}
		if (method == MethodName.OnFoldButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ShowSubInspector)
		{
			return true;
		}
		if (method == MethodName.HideSubInspector)
		{
			return true;
		}
		if (method == MethodName.OnResourceChanged)
		{
			return true;
		}
		if (method == MethodName.OnResourceSelected)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._objectLabel)
		{
			_objectLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourcePicker)
		{
			_resourcePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._subInspector)
		{
			_subInspector = VariantUtils.ConvertTo<XWInspector>(in value);
			return true;
		}
		if (name == PropertyName._foldButton)
		{
			_foldButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._isFolded)
		{
			_isFolded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			_baseType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._subInspectorContainer)
		{
			_subInspectorContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._isResource)
		{
			_isResource = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pickerContainer)
		{
			_pickerContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._objectLabel)
		{
			value = VariantUtils.CreateFrom(in _objectLabel);
			return true;
		}
		if (name == PropertyName._resourcePicker)
		{
			value = VariantUtils.CreateFrom(in _resourcePicker);
			return true;
		}
		if (name == PropertyName._subInspector)
		{
			value = VariantUtils.CreateFrom(in _subInspector);
			return true;
		}
		if (name == PropertyName._foldButton)
		{
			value = VariantUtils.CreateFrom(in _foldButton);
			return true;
		}
		if (name == PropertyName._isFolded)
		{
			value = VariantUtils.CreateFrom(in _isFolded);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			value = VariantUtils.CreateFrom(in _baseType);
			return true;
		}
		if (name == PropertyName._subInspectorContainer)
		{
			value = VariantUtils.CreateFrom(in _subInspectorContainer);
			return true;
		}
		if (name == PropertyName._isResource)
		{
			value = VariantUtils.CreateFrom(in _isResource);
			return true;
		}
		if (name == PropertyName._pickerContainer)
		{
			value = VariantUtils.CreateFrom(in _pickerContainer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._objectLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._subInspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._foldButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isFolded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._baseType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._subInspectorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pickerContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._objectLabel, Variant.From(in _objectLabel));
		info.AddProperty(PropertyName._resourcePicker, Variant.From(in _resourcePicker));
		info.AddProperty(PropertyName._subInspector, Variant.From(in _subInspector));
		info.AddProperty(PropertyName._foldButton, Variant.From(in _foldButton));
		info.AddProperty(PropertyName._isFolded, Variant.From(in _isFolded));
		info.AddProperty(PropertyName._baseType, Variant.From(in _baseType));
		info.AddProperty(PropertyName._subInspectorContainer, Variant.From(in _subInspectorContainer));
		info.AddProperty(PropertyName._isResource, Variant.From(in _isResource));
		info.AddProperty(PropertyName._pickerContainer, Variant.From(in _pickerContainer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._objectLabel, out var value))
		{
			_objectLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourcePicker, out var value2))
		{
			_resourcePicker = value2.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._subInspector, out var value3))
		{
			_subInspector = value3.As<XWInspector>();
		}
		if (info.TryGetProperty(PropertyName._foldButton, out var value4))
		{
			_foldButton = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._isFolded, out var value5))
		{
			_isFolded = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._baseType, out var value6))
		{
			_baseType = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._subInspectorContainer, out var value7))
		{
			_subInspectorContainer = value7.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._isResource, out var value8))
		{
			_isResource = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pickerContainer, out var value9))
		{
			_pickerContainer = value9.As<HBoxContainer>();
		}
	}
}
