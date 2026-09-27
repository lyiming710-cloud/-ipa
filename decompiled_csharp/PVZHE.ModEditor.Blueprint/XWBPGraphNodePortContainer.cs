using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.Registry;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/GraphNode/PortWidget/XWBPGraphNodePortContainer.cs")]
public class XWBPGraphNodePortContainer : MarginContainer
{
	[Signal]
	public delegate void InputValueChangeEventHandler(XWBPNodePortData portData, Variant oldValue, Variant newValue);

	public new class MethodName : MarginContainer.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetVisibility = "SetVisibility";

		public static readonly StringName InputSet = "InputSet";

		public static readonly StringName OutputSet = "OutputSet";

		public static readonly StringName ShouldCreateInputEditor = "ShouldCreateInputEditor";

		public static readonly StringName NormalizeEmbeddedEditor = "NormalizeEmbeddedEditor";

		public static readonly StringName NormalizeEmbeddedControl = "NormalizeEmbeddedControl";

		public static readonly StringName OnInputEditorValueChanged = "OnInputEditorValueChanged";
	}

	public new class PropertyName : MarginContainer.PropertyName
	{
		public static readonly StringName InputPortData = "InputPortData";

		public static readonly StringName OutputPortData = "OutputPortData";

		public static readonly StringName InputEditor = "InputEditor";

		public static readonly StringName _inputContainer = "_inputContainer";

		public static readonly StringName _outputContainer = "_outputContainer";

		public static readonly StringName _inputLabel = "_inputLabel";

		public static readonly StringName _outputLabel = "_outputLabel";

		public static readonly StringName _inputTypeIcon = "_inputTypeIcon";

		public static readonly StringName _outputTypeIcon = "_outputTypeIcon";

		public static readonly StringName _inputEditorContainer = "_inputEditorContainer";

		public static readonly StringName _inputEditor = "_inputEditor";

		public static readonly StringName _lastInputValue = "_lastInputValue";
	}

	public new class SignalName : MarginContainer.SignalName
	{
		public static readonly StringName InputValueChange = "InputValueChange";
	}

	private const float PortEditorHeight = 24f;

	private const string ScenePath = "res://addons/ModEditor/Blueprint/GUI/GraphNode/PortWidget/XWBPGraphNodePortContainer.tscn";

	private static PackedScene _scene;

	private PanelContainer _inputContainer;

	private PanelContainer _outputContainer;

	private Label _inputLabel;

	private Label _outputLabel;

	private TextureRect _inputTypeIcon;

	private TextureRect _outputTypeIcon;

	private PanelContainer _inputEditorContainer;

	private XWInspectorPropertyEditorBase _inputEditor;

	private Variant _lastInputValue;

	private InputValueChangeEventHandler backing_InputValueChange;

	public XWBPNodePortData InputPortData { get; private set; }

	public XWBPNodePortData OutputPortData { get; private set; }

	public XWInspectorPropertyEditorBase InputEditor => _inputEditor;

	public event InputValueChangeEventHandler InputValueChange
	{
		add
		{
			backing_InputValueChange = (InputValueChangeEventHandler)Delegate.Combine(backing_InputValueChange, value);
		}
		remove
		{
			backing_InputValueChange = (InputValueChangeEventHandler)Delegate.Remove(backing_InputValueChange, value);
		}
	}

	public static XWBPGraphNodePortContainer Create()
	{
		if (_scene == null)
		{
			_scene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Blueprint/GUI/GraphNode/PortWidget/XWBPGraphNodePortContainer.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		return _scene.Instantiate<XWBPGraphNodePortContainer>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		_inputContainer = GetNode<PanelContainer>("%InputContainer");
		_outputContainer = GetNode<PanelContainer>("%OutputContainer");
		_inputTypeIcon = GetNode<TextureRect>("%InputTypeIcon");
		_inputLabel = GetNode<Label>("%InputLabel");
		_inputEditorContainer = GetNode<PanelContainer>("%InputEditorContainer");
		_outputLabel = GetNode<Label>("%OutputLabel");
		_outputTypeIcon = GetNode<TextureRect>("%OutputTypeIcon");
		SetVisibility();
	}

	private void SetVisibility()
	{
		if (_inputContainer != null)
		{
			_inputContainer.Visible = InputPortData != null;
			_inputLabel.Visible = InputPortData != null;
			if (GodotObject.IsInstanceValid(_inputEditorContainer))
			{
				_inputEditorContainer.Visible = false;
			}
			_outputContainer.Visible = OutputPortData != null;
			_outputLabel.Visible = OutputPortData != null;
		}
	}

	public void InputSet(XWBPNodePortData portData)
	{
		InputPortData = portData;
		_inputContainer.Visible = true;
		_inputLabel.Visible = true;
		_inputLabel.Text = portData.Name;
		_inputEditor = null;
		if (portData.PortTypeValue >= XWBPNodePortData.PortType.Flow)
		{
			return;
		}
		bool flag = ShouldCreateInputEditor(portData);
		_inputEditorContainer.Visible = flag;
		if (flag)
		{
			if (portData.Value.VariantType == Variant.Type.Nil && portData.DefaultValue.VariantType != Variant.Type.Nil)
			{
				portData.Value = portData.DefaultValue;
			}
			_lastInputValue = portData.Value;
			Variant.Type type = (Variant.Type)portData.PortTypeValue;
			_inputEditor = XWTypeRegistry.Instance.GetTypeEditor(type);
			if (GodotObject.IsInstanceValid(_inputEditor))
			{
				XWInspectorProperty property = new XWInspectorProperty(portData, "Value", default, null, null, PropertyHint.None);
				_inputEditor.IsInner = true;
				_inputEditor.ValueChanged += OnInputEditorValueChanged;
				_inputEditorContainer.AddChild(_inputEditor, forceReadableName: false, InternalMode.Disabled);
				_inputEditor.SetEditProperty(property, "Value");
				_inputEditor.PropertyNameLabel.Text = portData.Name;
				NormalizeEmbeddedEditor(_inputEditor);
				Control nodeOrNull = _inputEditor.GetNodeOrNull<Control>("%InformationContainner");
				if (GodotObject.IsInstanceValid(nodeOrNull))
				{
					nodeOrNull.Visible = false;
				}
			}
			else
			{
				_inputEditorContainer.Visible = false;
			}
		}
		_inputTypeIcon.Texture = XWBPNodePortData.GetTypeIcon(portData.PortTypeValue, portData.ClassName);
		_inputTypeIcon.Visible = true;
	}

	public void OutputSet(XWBPNodePortData portData)
	{
		OutputPortData = portData;
		_outputContainer.Visible = true;
		_outputLabel.Visible = true;
		_outputLabel.Text = portData.Name;
		if (portData.PortTypeValue < XWBPNodePortData.PortType.Flow)
		{
			_outputTypeIcon.Texture = XWBPNodePortData.GetTypeIcon(portData.PortTypeValue, portData.ClassName);
			_outputTypeIcon.Visible = true;
		}
	}

	private static bool ShouldCreateInputEditor(XWBPNodePortData portData)
	{
		if (GodotObject.IsInstanceValid(portData) && portData.PortTypeValue < XWBPNodePortData.PortType.Flow && !XWBPNodePortData.IsObjectPortType(portData.PortTypeValue))
		{
			return XWTypeRegistry.Instance.HasType((Variant.Type)portData.PortTypeValue);
		}
		return false;
	}

	private static void NormalizeEmbeddedEditor(XWInspectorPropertyEditorBase editor)
	{
		if (GodotObject.IsInstanceValid(editor))
		{
			editor.CustomMinimumSize = new Vector2(0f, 24f);
			editor.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			editor.SizeFlagsVertical = SizeFlags.ShrinkCenter;
			MarginContainer nodeOrNull = editor.GetNodeOrNull<MarginContainer>("MarginContainer");
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				nodeOrNull.AddThemeConstantOverride("margin_left", 0);
				nodeOrNull.AddThemeConstantOverride("margin_right", 0);
				nodeOrNull.AddThemeConstantOverride("margin_top", 0);
				nodeOrNull.AddThemeConstantOverride("margin_bottom", 0);
			}
			HBoxContainer nodeOrNull2 = editor.GetNodeOrNull<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer");
			if (GodotObject.IsInstanceValid(nodeOrNull2))
			{
				nodeOrNull2.CustomMinimumSize = new Vector2(0f, 24f);
				nodeOrNull2.SizeFlagsVertical = SizeFlags.ShrinkCenter;
			}
			NormalizeEmbeddedControl(editor.GetNodeOrNull<Control>("%PropertyEditor") ?? editor.GetNodeOrNull<Control>("MarginContainer/VBoxContainer/HBoxContainer/CheckBox"));
		}
	}

	private static void NormalizeEmbeddedControl(Control control)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return;
		}
		control.CustomMinimumSize = new Vector2(control.CustomMinimumSize.X, 24f);
		control.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		if (control is Button button)
		{
			button.VerticalIconAlignment = VerticalAlignment.Center;
			button.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
		}
		foreach (Node child in control.GetChildren())
		{
			if (child is Control control2)
			{
				NormalizeEmbeddedControl(control2);
			}
		}
	}

	private void OnInputEditorValueChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (GodotObject.IsInstanceValid(InputPortData))
		{
			Variant lastInputValue = _lastInputValue;
			if (!lastInputValue.Equals(value))
			{
				InputPortData.Value = value;
				_lastInputValue = value;
				EmitSignal(SignalName.InputValueChange, InputPortData, lastInputValue, value);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MarginContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InputSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "portData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OutputSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "portData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldCreateInputEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "portData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeEmbeddedEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeEmbeddedControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnInputEditorValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNodePortContainer>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SetVisibility && args.Count == 0)
		{
			SetVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.InputSet && args.Count == 1)
		{
			InputSet(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OutputSet && args.Count == 1)
		{
			OutputSet(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldCreateInputEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldCreateInputEditor(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeEmbeddedEditor && args.Count == 1)
		{
			NormalizeEmbeddedEditor(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeEmbeddedControl && args.Count == 1)
		{
			NormalizeEmbeddedControl(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnInputEditorValueChanged && args.Count == 4)
		{
			OnInputEditorValueChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNodePortContainer>(Create());
			return true;
		}
		if (method == MethodName.ShouldCreateInputEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldCreateInputEditor(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeEmbeddedEditor && args.Count == 1)
		{
			NormalizeEmbeddedEditor(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeEmbeddedControl && args.Count == 1)
		{
			NormalizeEmbeddedControl(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
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
		if (method == MethodName.SetVisibility)
		{
			return true;
		}
		if (method == MethodName.InputSet)
		{
			return true;
		}
		if (method == MethodName.OutputSet)
		{
			return true;
		}
		if (method == MethodName.ShouldCreateInputEditor)
		{
			return true;
		}
		if (method == MethodName.NormalizeEmbeddedEditor)
		{
			return true;
		}
		if (method == MethodName.NormalizeEmbeddedControl)
		{
			return true;
		}
		if (method == MethodName.OnInputEditorValueChanged)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.InputPortData)
		{
			InputPortData = VariantUtils.ConvertTo<XWBPNodePortData>(in value);
			return true;
		}
		if (name == PropertyName.OutputPortData)
		{
			OutputPortData = VariantUtils.ConvertTo<XWBPNodePortData>(in value);
			return true;
		}
		if (name == PropertyName._inputContainer)
		{
			_inputContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._outputContainer)
		{
			_outputContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._inputLabel)
		{
			_inputLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._outputLabel)
		{
			_outputLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._inputTypeIcon)
		{
			_inputTypeIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._outputTypeIcon)
		{
			_outputTypeIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._inputEditorContainer)
		{
			_inputEditorContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._inputEditor)
		{
			_inputEditor = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		if (name == PropertyName._lastInputValue)
		{
			_lastInputValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		XWBPNodePortData from;
		if (name == PropertyName.InputPortData)
		{
			from = InputPortData;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.OutputPortData)
		{
			from = OutputPortData;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InputEditor)
		{
			value = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(InputEditor);
			return true;
		}
		if (name == PropertyName._inputContainer)
		{
			value = VariantUtils.CreateFrom(in _inputContainer);
			return true;
		}
		if (name == PropertyName._outputContainer)
		{
			value = VariantUtils.CreateFrom(in _outputContainer);
			return true;
		}
		if (name == PropertyName._inputLabel)
		{
			value = VariantUtils.CreateFrom(in _inputLabel);
			return true;
		}
		if (name == PropertyName._outputLabel)
		{
			value = VariantUtils.CreateFrom(in _outputLabel);
			return true;
		}
		if (name == PropertyName._inputTypeIcon)
		{
			value = VariantUtils.CreateFrom(in _inputTypeIcon);
			return true;
		}
		if (name == PropertyName._outputTypeIcon)
		{
			value = VariantUtils.CreateFrom(in _outputTypeIcon);
			return true;
		}
		if (name == PropertyName._inputEditorContainer)
		{
			value = VariantUtils.CreateFrom(in _inputEditorContainer);
			return true;
		}
		if (name == PropertyName._inputEditor)
		{
			value = VariantUtils.CreateFrom(in _inputEditor);
			return true;
		}
		if (name == PropertyName._lastInputValue)
		{
			value = VariantUtils.CreateFrom(in _lastInputValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._inputContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outputContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inputLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outputLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inputTypeIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outputTypeIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inputEditorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inputEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._lastInputValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.InputPortData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.OutputPortData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.InputEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.InputPortData, Variant.From<XWBPNodePortData>(InputPortData));
		info.AddProperty(PropertyName.OutputPortData, Variant.From<XWBPNodePortData>(OutputPortData));
		info.AddProperty(PropertyName._inputContainer, Variant.From(in _inputContainer));
		info.AddProperty(PropertyName._outputContainer, Variant.From(in _outputContainer));
		info.AddProperty(PropertyName._inputLabel, Variant.From(in _inputLabel));
		info.AddProperty(PropertyName._outputLabel, Variant.From(in _outputLabel));
		info.AddProperty(PropertyName._inputTypeIcon, Variant.From(in _inputTypeIcon));
		info.AddProperty(PropertyName._outputTypeIcon, Variant.From(in _outputTypeIcon));
		info.AddProperty(PropertyName._inputEditorContainer, Variant.From(in _inputEditorContainer));
		info.AddProperty(PropertyName._inputEditor, Variant.From(in _inputEditor));
		info.AddProperty(PropertyName._lastInputValue, Variant.From(in _lastInputValue));
		info.AddSignalEventDelegate(SignalName.InputValueChange, backing_InputValueChange);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.InputPortData, out var value))
		{
			InputPortData = value.As<XWBPNodePortData>();
		}
		if (info.TryGetProperty(PropertyName.OutputPortData, out var value2))
		{
			OutputPortData = value2.As<XWBPNodePortData>();
		}
		if (info.TryGetProperty(PropertyName._inputContainer, out var value3))
		{
			_inputContainer = value3.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._outputContainer, out var value4))
		{
			_outputContainer = value4.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._inputLabel, out var value5))
		{
			_inputLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._outputLabel, out var value6))
		{
			_outputLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._inputTypeIcon, out var value7))
		{
			_inputTypeIcon = value7.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._outputTypeIcon, out var value8))
		{
			_outputTypeIcon = value8.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._inputEditorContainer, out var value9))
		{
			_inputEditorContainer = value9.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._inputEditor, out var value10))
		{
			_inputEditor = value10.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetProperty(PropertyName._lastInputValue, out var value11))
		{
			_lastInputValue = value11.As<Variant>();
		}
		if (info.TryGetSignalEventDelegate<InputValueChangeEventHandler>(SignalName.InputValueChange, out var value12))
		{
			backing_InputValueChange = value12;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.InputValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "portData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "oldValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	protected void EmitSignalInputValueChange(XWBPNodePortData portData, Variant oldValue, Variant newValue)
	{
		StringName inputValueChange = SignalName.InputValueChange;
		_003C_003Ey__InlineArray3<Variant> buffer = default;
		buffer[0] = portData;
		buffer[1] = oldValue;
		buffer[2] = newValue;
		EmitSignal(inputValueChange, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.InputValueChange && args.Count == 3)
		{
			backing_InputValueChange?.Invoke(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.InputValueChange)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
