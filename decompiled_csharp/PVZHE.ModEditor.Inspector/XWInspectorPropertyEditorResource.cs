using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Resource/XWInspectorPropertyEditorResource.cs")]
public class XWInspectorPropertyEditorResource : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ApplyGodotInspectorLayout = "ApplyGodotInspectorLayout";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName UpdateResourceDisplay = "UpdateResourceDisplay";

		public static readonly StringName OnFoldButtonPressed = "OnFoldButtonPressed";

		public static readonly StringName OnResourceChanged = "OnResourceChanged";

		public static readonly StringName OnResourceSelected = "OnResourceSelected";

		public static readonly StringName OpenResourceInGlobalInspector = "OpenResourceInGlobalInspector";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _foldButton = "_foldButton";

		public static readonly StringName _resourcePicker = "_resourcePicker";

		public static readonly StringName _baseType = "_baseType";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private static Texture2D _iconArrowOpen;

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Resource/XWInspectorPropertyEditorResource.tscn";

	private const int ButtonSeparation = 2;

	private static readonly Vector2 ResourceButtonMinimumSize = new Vector2(30f, 28f);

	private Button _foldButton;

	private XWResourcePicker _resourcePicker;

	private string _baseType = "Resource";

	public static XWInspectorPropertyEditorResource Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Resource/XWInspectorPropertyEditorResource.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorResource>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		if (!GodotObject.IsInstanceValid(_iconArrowOpen))
		{
			_iconArrowOpen = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/GuiTreeArrowDown.svg", null, ResourceLoader.CacheMode.Reuse));
		}
		_foldButton = GetNode<Button>("%FoldButton");
		_resourcePicker = GetNode<XWResourcePicker>("%ResourcePicker");
		ApplyGodotInspectorLayout();
		_foldButton.Icon = _iconArrowOpen;
		_foldButton.TooltipText = "在可视化资源编辑器中编辑";
		_foldButton.Pressed += OnFoldButtonPressed;
		_resourcePicker.ResourceChanged += OnResourceChanged;
		_resourcePicker.ResourceSelected += OnResourceSelected;
	}

	private void ApplyGodotInspectorLayout()
	{
		if (GodotObject.IsInstanceValid(_foldButton))
		{
			_foldButton.CustomMinimumSize = ResourceButtonMinimumSize;
		}
		if (_foldButton?.GetParent() is HBoxContainer hBoxContainer)
		{
			hBoxContainer.AddThemeConstantOverride("separation", 2);
		}
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		if (property.Hint == PropertyHint.ResourceType)
		{
			_baseType = property.HintString;
			if (string.IsNullOrEmpty(_baseType))
			{
				_baseType = "Resource";
			}
		}
		else if (!string.IsNullOrEmpty(property.HintString))
		{
			_baseType = property.HintString;
		}
		if (GodotObject.IsInstanceValid(_resourcePicker))
		{
			_resourcePicker.Setup(_baseType);
		}
		base.SetEditProperty(property, field);
	}

	public override void UpdateValue()
	{
		UpdateResourceDisplay();
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
			}
		}
		else if (GodotObject.IsInstanceValid(_foldButton))
		{
			_foldButton.Visible = false;
		}
	}

	private void OnFoldButtonPressed()
	{
		Variant propertyValue = GetPropertyValue();
		Resource resource = ((propertyValue.VariantType == Variant.Type.Object && propertyValue.As<GodotObject>() is Resource resource2) ? resource2 : null);
		OpenResourceInGlobalInspector(resource);
	}

	private void OnResourceChanged(Resource resource)
	{
		ValueChange(resource);
	}

	private void OnResourceSelected(Resource resource)
	{
		OpenResourceInGlobalInspector(resource);
	}

	private void OpenResourceInGlobalInspector(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return;
		}
		string text = resource.ResourcePath ?? "";
		if (Property?.Object is Resource { ResourcePath: var text2 } resource2)
		{
			if (text2 == null)
			{
				text2 = "";
			}
			string ownerPath = text2;
			XWEditorInterface.Instance?.EditResource(resource, XWResourceEditContext.ForProperty(resource, resource2, text, ownerPath, Property.PropName.ToString(), -1, "inspector", XWResourceEditContext.IsBuiltInPath(text)));
		}
		else
		{
			XWEditorInterface.Instance?.EditResource(resource, XWResourceEditContext.ForRoot(resource, text, "inspector"));
		}
		XWInspector inspector = GetInspector();
		if (GodotObject.IsInstanceValid(inspector))
		{
			inspector.EmitSignal(XWInspector.SignalName.ResourceSelected, resource);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyGodotInspectorLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateResourceDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFoldButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnResourceChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenResourceInGlobalInspector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorResource>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyGodotInspectorLayout && args.Count == 0)
		{
			ApplyGodotInspectorLayout();
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
		if (method == MethodName.OpenResourceInGlobalInspector && args.Count == 1)
		{
			OpenResourceInGlobalInspector(VariantUtils.ConvertTo<Resource>(in args[0]));
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorResource>(Create());
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
		if (method == MethodName.ApplyGodotInspectorLayout)
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
		if (method == MethodName.UpdateResourceDisplay)
		{
			return true;
		}
		if (method == MethodName.OnFoldButtonPressed)
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
		if (method == MethodName.OpenResourceInGlobalInspector)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._foldButton)
		{
			_foldButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resourcePicker)
		{
			_resourcePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			_baseType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._foldButton)
		{
			value = VariantUtils.CreateFrom(in _foldButton);
			return true;
		}
		if (name == PropertyName._resourcePicker)
		{
			value = VariantUtils.CreateFrom(in _resourcePicker);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			value = VariantUtils.CreateFrom(in _baseType);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._foldButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._baseType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._foldButton, Variant.From(in _foldButton));
		info.AddProperty(PropertyName._resourcePicker, Variant.From(in _resourcePicker));
		info.AddProperty(PropertyName._baseType, Variant.From(in _baseType));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._foldButton, out var value))
		{
			_foldButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resourcePicker, out var value2))
		{
			_resourcePicker = value2.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._baseType, out var value3))
		{
			_baseType = value3.As<string>();
		}
	}
}
