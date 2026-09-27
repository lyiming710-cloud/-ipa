using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/XWInspectorPropertyEditorBase.cs")]
public class XWInspectorPropertyEditorBase : PanelContainer
{
	[Signal]
	public delegate void ValueChangedEventHandler(GodotObject obj, StringName property, StringName field, Variant value);

	public new class MethodName : PanelContainer.MethodName
	{
		public static readonly StringName LoadIcons = "LoadIcons";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CreateKeyButton = "CreateKeyButton";

		public static readonly StringName NormalizePropertyEditorLayout = "NormalizePropertyEditorLayout";

		public static readonly StringName NormalizeInspectorRowFill = "NormalizeInspectorRowFill";

		public static readonly StringName NormalizeEditorRoot = "NormalizeEditorRoot";

		public static readonly StringName WithMinimumHeight = "WithMinimumHeight";

		public static readonly StringName NormalizeIconButton = "NormalizeIconButton";

		public static readonly StringName NormalizeControlTree = "NormalizeControlTree";

		public static readonly StringName NormalizeSpinBox = "NormalizeSpinBox";

		public static readonly StringName IsSingleLineEditor = "IsSingleLineEditor";

		public static readonly StringName OnKeyButtonPressed = "OnKeyButtonPressed";

		public static readonly StringName ApplySplitRatio = "ApplySplitRatio";

		public static readonly StringName SetSplitRatio = "SetSplitRatio";

		public static readonly StringName ValueChange = "ValueChange";

		public static readonly StringName GetValue = "GetValue";

		public static readonly StringName SetEditProperty = "SetEditProperty";

		public static readonly StringName UpdateValue = "UpdateValue";

		public static readonly StringName UpdateValueSafely = "UpdateValueSafely";

		public static readonly StringName BeginContinuousEdit = "BeginContinuousEdit";

		public static readonly StringName CommitContinuousEdit = "CommitContinuousEdit";

		public static readonly StringName CancelContinuousEdit = "CancelContinuousEdit";

		public static readonly StringName PreviewContinuousValue = "PreviewContinuousValue";

		public static readonly StringName WriteContinuousProperty = "WriteContinuousProperty";

		public static readonly StringName ClearContinuousEditSession = "ClearContinuousEditSession";

		public static readonly StringName BindContinuousInputControls = "BindContinuousInputControls";

		public static readonly StringName BindContinuousInputControlTree = "BindContinuousInputControlTree";

		public static readonly StringName BindContinuousInputControl = "BindContinuousInputControl";

		public static readonly StringName CommitContinuousEditIfFocusLeft = "CommitContinuousEditIfFocusLeft";

		public static readonly StringName ApplySpinBoxTextPreview = "ApplySpinBoxTextPreview";

		public static readonly StringName HasFocusedContinuousInput = "HasFocusedContinuousInput";

		public static readonly StringName SetLineEditTextPreservingCaret = "SetLineEditTextPreservingCaret";

		public static readonly StringName SetTextEditTextPreservingCaret = "SetTextEditTextPreservingCaret";

		public static readonly StringName SetMixedValue = "SetMixedValue";

		public static readonly StringName UpdateResetButton = "UpdateResetButton";

		public static readonly StringName ShouldShowResetButton = "ShouldShowResetButton";

		public static readonly StringName UpdateKeyButton = "UpdateKeyButton";

		public static readonly StringName GetPropertyValue = "GetPropertyValue";

		public static readonly StringName ResetButtonPressed = "ResetButtonPressed";

		public static readonly StringName GetRevertValue = "GetRevertValue";

		public static readonly StringName RefreshEditProperty = "RefreshEditProperty";

		public static readonly StringName SetupRangeHint = "SetupRangeHint";

		public static readonly StringName DoSelect = "DoSelect";

		public static readonly StringName Deselect = "Deselect";

		public static readonly StringName HideEditor = "HideEditor";

		public static readonly StringName ShowEditor = "ShowEditor";

		public static readonly StringName OnGuiInput = "OnGuiInput";

		public static readonly StringName OnMouseEntered = "OnMouseEntered";

		public static readonly StringName OnMouseExited = "OnMouseExited";

		public new static readonly StringName _Draw = "_Draw";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName CopyValue = "CopyValue";

		public static readonly StringName PasteValue = "PasteValue";

		public static readonly StringName CopyPropertyPath = "CopyPropertyPath";

		public static readonly StringName DeleteProperty = "DeleteProperty";

		public static readonly StringName GetInspector = "GetInspector";

		public static readonly StringName ReadObjectProperty = "ReadObjectProperty";

		public static readonly StringName WriteObjectProperty = "WriteObjectProperty";

		public static readonly StringName ReadIndexedObjectProperty = "ReadIndexedObjectProperty";

		public static readonly StringName WriteIndexedObjectProperty = "WriteIndexedObjectProperty";

		public static readonly StringName GetVariantArrayElement = "GetVariantArrayElement";

		public static readonly StringName CapitalizePropertyName = "CapitalizePropertyName";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName PropertyEditorPath = "PropertyEditorPath";

		public static readonly StringName Select = "Select";

		public static readonly StringName IsInner = "IsInner";

		public static readonly StringName HistoryTypeOverride = "HistoryTypeOverride";

		public static readonly StringName IsContinuousEditActive = "IsContinuousEditActive";

		public static readonly StringName ContinuousEditOwner = "ContinuousEditOwner";

		public static readonly StringName ContinuousInputControlCount = "ContinuousInputControlCount";

		public static readonly StringName ContinuousBindingRootDescription = "ContinuousBindingRootDescription";

		public static readonly StringName SplitRatio = "SplitRatio";

		public static readonly StringName IsResourceEditor = "IsResourceEditor";

		public static readonly StringName PropertyNameLabel = "PropertyNameLabel";

		public static readonly StringName ResetButton = "ResetButton";

		public static readonly StringName InformationContainner = "InformationContainner";

		public static readonly StringName PropertyEditor = "PropertyEditor";

		public static readonly StringName Property = "Property";

		public static readonly StringName Field = "Field";

		public static readonly StringName PropertyNameLock = "PropertyNameLock";

		public static readonly StringName UndoRedoManager = "UndoRedoManager";

		public static readonly StringName LastValue = "LastValue";

		public static readonly StringName _hover = "_hover";

		public static readonly StringName _isMixedValue = "_isMixedValue";

		public static readonly StringName _keyButton = "_keyButton";

		public static readonly StringName _continuousEditActive = "_continuousEditActive";

		public static readonly StringName _continuousEditObject = "_continuousEditObject";

		public static readonly StringName _continuousEditProperty = "_continuousEditProperty";

		public static readonly StringName _continuousEditRootProperty = "_continuousEditRootProperty";

		public static readonly StringName _continuousEditField = "_continuousEditField";

		public static readonly StringName _continuousEditOldValue = "_continuousEditOldValue";

		public static readonly StringName _continuousEditPreviewValue = "_continuousEditPreviewValue";

		public static readonly StringName _continuousEditHasPreview = "_continuousEditHasPreview";

		public static readonly StringName _refreshingEditorValue = "_refreshingEditorValue";

		public static readonly StringName _continuousBindingPassPending = "_continuousBindingPassPending";
	}

	public new class SignalName : PanelContainer.SignalName
	{
		public static readonly StringName ValueChanged = "ValueChanged";
	}

	public Label PropertyNameLabel;

	public Button ResetButton;

	protected HBoxContainer InformationContainner;

	protected Control PropertyEditor;

	internal const float InspectorPropertyHeight = 28f;

	public XWInspectorProperty Property;

	public StringName Field = "";

	public bool PropertyNameLock;

	public XWUndoRedoManager UndoRedoManager;

	public Variant LastValue;

	private bool _hover;

	private bool _isMixedValue;

	private Button _keyButton;

	private readonly List<Control> _continuousInputControls = new List<Control>();

	private readonly HashSet<ulong> _continuousInputControlIds = new HashSet<ulong>();

	private readonly HashSet<ulong> _spinBoxTextPreviewIds = new HashSet<ulong>();

	private bool _continuousEditActive;

	private GodotObject _continuousEditObject;

	private StringName _continuousEditProperty = "";

	private StringName _continuousEditRootProperty = "";

	private StringName _continuousEditField = "";

	private Variant _continuousEditOldValue;

	private Variant _continuousEditPreviewValue;

	private bool _continuousEditHasPreview;

	private bool _refreshingEditorValue;

	private bool _continuousBindingPassPending;

	private static Texture2D _iconReload;

	private static Texture2D _iconPin;

	private static Texture2D _iconFavorite;

	private static Texture2D _iconCopy;

	private static Texture2D _iconPaste;

	private static Texture2D _iconKey;

	private ValueChangedEventHandler backing_ValueChanged;

	[Export(PropertyHint.None, "")]
	public NodePath PropertyEditorPath { get; set; }

	public bool Select { get; set; }

	public bool IsInner { get; set; }

	public int HistoryTypeOverride { get; set; } = -1;

	public bool IsContinuousEditActive => _continuousEditActive;

	public XWInspectorPropertyEditorBase ContinuousEditOwner { get; set; }

	public int ContinuousInputControlCount => _continuousInputControls.Count;

	public string ContinuousBindingRootDescription
	{
		get
		{
			if (!GodotObject.IsInstanceValid(this))
			{
				return "invalid";
			}
			return $"{Name}:{GetType().Name}:children={GetChildCount()}";
		}
	}

	public float SplitRatio { get; set; } = 0.5f;

	public bool IsResourceEditor { get; protected set; }

	public event ValueChangedEventHandler ValueChanged
	{
		add
		{
			backing_ValueChanged = (ValueChangedEventHandler)Delegate.Combine(backing_ValueChanged, value);
		}
		remove
		{
			backing_ValueChanged = (ValueChangedEventHandler)Delegate.Remove(backing_ValueChanged, value);
		}
	}

	private static void LoadIcons()
	{
		_iconReload = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/IconReload.svg", null, ResourceLoader.CacheMode.Reuse));
		_iconPin = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Pin.svg", null, ResourceLoader.CacheMode.Reuse));
		_iconFavorite = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Favorites.svg", null, ResourceLoader.CacheMode.Reuse));
		_iconCopy = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionCopy.svg", null, ResourceLoader.CacheMode.Reuse));
		_iconPaste = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionPaste.svg", null, ResourceLoader.CacheMode.Reuse));
		_iconKey = XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/Key.svg", null, ResourceLoader.CacheMode.Reuse));
	}

	public override void _Ready()
	{
		if (_iconReload == null)
		{
			LoadIcons();
		}
		PropertyNameLabel = GetNode<Label>("%PropertyNameLabel");
		ResetButton = GetNode<Button>("%ResetButton");
		InformationContainner = GetNode<HBoxContainer>("%InformationContainner");
		if (PropertyEditorPath != null && !PropertyEditorPath.IsEmpty)
		{
			PropertyEditor = GetNode<Control>(PropertyEditorPath);
		}
		else
		{
			PropertyEditor = GetNodeOrNull<Control>("%PropertyEditor");
		}
		ResetButton.Pressed += ResetButtonPressed;
		CreateKeyButton();
		NormalizePropertyEditorLayout();
		ApplySplitRatio();
		GuiInput += OnGuiInput;
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		_continuousBindingPassPending = true;
		SetProcess(enable: true);
	}

	public override void _Process(double delta)
	{
		if (_continuousBindingPassPending)
		{
			BindContinuousInputControls();
			_continuousBindingPassPending = false;
			SetProcess(enable: false);
		}
	}

	public override void _ExitTree()
	{
		CancelContinuousEdit();
		base._ExitTree();
	}

	private void CreateKeyButton()
	{
		if (GodotObject.IsInstanceValid(InformationContainner))
		{
			_keyButton = new Button
			{
				Name = "KeyButton",
				Icon = _iconKey,
				CustomMinimumSize = new Vector2(16f, 16f),
				Flat = true,
				TooltipText = "添加关键帧",
				Visible = false
			};
			_keyButton.Pressed += OnKeyButtonPressed;
			InformationContainner.AddChild(_keyButton, forceReadableName: false, InternalMode.Disabled);
		}
	}

	protected void NormalizePropertyEditorLayout()
	{
		CustomMinimumSize = WithMinimumHeight(CustomMinimumSize, 28f);
		NormalizeInspectorRowFill();
		if (GodotObject.IsInstanceValid(InformationContainner))
		{
			InformationContainner.CustomMinimumSize = WithMinimumHeight(InformationContainner.CustomMinimumSize, 28f);
			InformationContainner.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			InformationContainner.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		}
		if (GodotObject.IsInstanceValid(PropertyNameLabel))
		{
			PropertyNameLabel.VerticalAlignment = VerticalAlignment.Center;
			PropertyNameLabel.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
			PropertyNameLabel.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		}
		NormalizeIconButton(ResetButton, 16f);
		NormalizeIconButton(_keyButton, 16f);
		if (GodotObject.IsInstanceValid(PropertyEditor))
		{
			PropertyEditor.CustomMinimumSize = WithMinimumHeight(PropertyEditor.CustomMinimumSize, 28f);
			PropertyEditor.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			NormalizeEditorRoot(PropertyEditor);
			NormalizeControlTree(PropertyEditor);
		}
	}

	private void NormalizeInspectorRowFill()
	{
		Control nodeOrNull = GetNodeOrNull<Control>("MarginContainer");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		}
		Control nodeOrNull2 = GetNodeOrNull<Control>("MarginContainer/VBoxContainer");
		if (GodotObject.IsInstanceValid(nodeOrNull2))
		{
			nodeOrNull2.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		}
		Control nodeOrNull3 = GetNodeOrNull<Control>("MarginContainer/VBoxContainer/HBoxContainer");
		if (GodotObject.IsInstanceValid(nodeOrNull3))
		{
			nodeOrNull3.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		}
	}

	private void NormalizeEditorRoot(Control control)
	{
		if (GodotObject.IsInstanceValid(control) && control == PropertyEditor)
		{
			control.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		}
	}

	private static Vector2 WithMinimumHeight(Vector2 size, float height)
	{
		if (size.Y >= height)
		{
			return size;
		}
		return new Vector2(size.X, height);
	}

	private static void NormalizeIconButton(Button button, float size)
	{
		if (GodotObject.IsInstanceValid(button))
		{
			button.CustomMinimumSize = new Vector2(size, size);
			button.SizeFlagsHorizontal = SizeFlags.ShrinkCenter;
			button.SizeFlagsVertical = SizeFlags.ShrinkCenter;
			button.Flat = true;
		}
	}

	private static void NormalizeControlTree(Control control)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return;
		}
		if (control is Label label)
		{
			label.VerticalAlignment = VerticalAlignment.Center;
			label.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
			label.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		}
		if (control is SpinBox spinBox)
		{
			NormalizeSpinBox(spinBox);
		}
		if (IsSingleLineEditor(control))
		{
			control.CustomMinimumSize = WithMinimumHeight(control.CustomMinimumSize, 28f);
			control.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		}
		if (control is Button button)
		{
			button.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
		}
		foreach (Node child in control.GetChildren())
		{
			if (child is Control control2)
			{
				NormalizeControlTree(control2);
			}
		}
	}

	internal static void NormalizeSpinBox(SpinBox spinBox)
	{
		if (GodotObject.IsInstanceValid(spinBox))
		{
			spinBox.Alignment = HorizontalAlignment.Right;
			spinBox.UpdateOnTextChanged = true;
			spinBox.CustomMinimumSize = WithMinimumHeight(spinBox.CustomMinimumSize, 28f);
			spinBox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			spinBox.SizeFlagsVertical = SizeFlags.ShrinkCenter;
			LineEdit lineEdit = spinBox.GetLineEdit();
			if (GodotObject.IsInstanceValid(lineEdit))
			{
				lineEdit.Alignment = HorizontalAlignment.Right;
				lineEdit.CaretBlink = true;
				lineEdit.SelectAllOnFocus = true;
			}
		}
	}

	private static bool IsSingleLineEditor(Control control)
	{
		if (!(control is LineEdit) && !(control is SpinBox) && !(control is Button) && !(control is OptionButton))
		{
			return control is ColorPickerButton;
		}
		return true;
	}

	private void OnKeyButtonPressed()
	{
		if (!GodotObject.IsInstanceValid(Property))
		{
			return;
		}
		XWEditorInterface instance = XWEditorInterface.Instance;
		if (instance != null && instance.HasAnimationEditor())
		{
			XWInspector inspector = GetInspector();
			if (GodotObject.IsInstanceValid(inspector))
			{
				inspector.EmitSignal(XWInspector.SignalName.PropertyKeyed, Property.PropName, GetPropertyValue());
			}
		}
	}

	private void ApplySplitRatio()
	{
		if (GodotObject.IsInstanceValid(InformationContainner))
		{
			InformationContainner.SizeFlagsStretchRatio = SplitRatio;
			if (GodotObject.IsInstanceValid(PropertyEditor))
			{
				PropertyEditor.SizeFlagsStretchRatio = 1f - SplitRatio;
			}
		}
	}

	public void SetSplitRatio(float ratio)
	{
		SplitRatio = Mathf.Clamp(ratio, 0.1f, 0.9f);
		ApplySplitRatio();
	}

	public virtual void ValueChange(Variant value, StringName field = null)
	{
		if (!GodotObject.IsInstanceValid(Property))
		{
			return;
		}
		if (field == null || field.ToString() == "")
		{
			field = Field;
		}
		if (_refreshingEditorValue)
		{
			return;
		}
		if (_continuousInputControls.Count == 0)
		{
			BindContinuousInputControls();
		}
		if (!IsInner && (_continuousEditActive || HasFocusedContinuousInput()))
		{
			if (!_continuousEditActive)
			{
				BeginContinuousEdit(field);
			}
			if (_continuousEditActive)
			{
				PreviewContinuousValue(value, field);
				return;
			}
		}
		if (IsInner)
		{
			EmitSignal(SignalName.ValueChanged, Property.Object, Property.PropName, field, value);
			UpdateResetButton();
			return;
		}
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Array && field != null && field.ToString() != "")
		{
			Godot.Collections.Array array = propertyValue.As<Godot.Collections.Array>();
			int index = field.ToString().ToInt();
			array[index] = value;
			WriteObjectProperty(Property.Object, Property.PropName, propertyValue);
			EmitSignal(SignalName.ValueChanged, Property.Object, Property.PropName, field, value);
			Property.SetCall();
			return;
		}
		if (propertyValue.VariantType == Variant.Type.Dictionary && field != null && field.ToString() != "")
		{
			Dictionary dictionary = propertyValue.As<Dictionary>();
			string text = field.ToString();
			if (dictionary.ContainsKey(field))
			{
				dictionary[field] = value;
			}
			else if (dictionary.ContainsKey(text))
			{
				dictionary[text] = value;
			}
			else
			{
				dictionary[field] = value;
			}
			WriteObjectProperty(Property.Object, Property.PropName, propertyValue);
			EmitSignal(SignalName.ValueChanged, Property.Object, Property.PropName, field, value);
			Property.SetCall();
			return;
		}
		bool flag = field != null && field.ToString() != "";
		StringName stringName = ((!flag) ? Property.PropName : new StringName($"{Property.PropName}:{field}"));
		if (GodotObject.IsInstanceValid(Property.Object))
		{
			Variant value2 = (flag ? ReadIndexedObjectProperty(Property.Object, stringName) : ReadObjectProperty(Property.Object, stringName));
			if (flag)
			{
				WriteIndexedObjectProperty(Property.Object, stringName, value);
			}
			else
			{
				WriteObjectProperty(Property.Object, stringName, value);
			}
			if (GodotObject.IsInstanceValid(UndoRedoManager))
			{
				UndoRedoManager.CreateAction($"Set Property: {stringName}", mergeMode: false, HistoryTypeOverride);
				UndoRedoManager.AddDoProperty(Property.Object, stringName, value);
				UndoRedoManager.AddUndoProperty(Property.Object, stringName, value2);
				UndoRedoManager.CommitAction();
			}
		}
		UpdateResetButton();
		EmitSignal(SignalName.ValueChanged, Property.Object, Property.PropName, Field, value);
		Property.SetCall();
	}

	public virtual Variant GetValue()
	{
		return default;
	}

	public virtual void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		CancelContinuousEdit();
		Property = property;
		if (!PropertyNameLock)
		{
			if (!IsInner)
			{
				string text = property.PropName.ToString();
				if (property.LabelOverride != "")
				{
					text = property.LabelOverride;
				}
				XWInspector inspector = GetInspector();
				if (GodotObject.IsInstanceValid(inspector) && inspector.PropertyNameStyleMode == XWInspector.PropertyNameStyle.StyleCapitalized)
				{
					PropertyNameLabel.Text = CapitalizePropertyName(text);
				}
				else
				{
					PropertyNameLabel.Text = text;
				}
			}
			else
			{
				PropertyNameLabel.Text = field?.ToString() ?? "";
			}
		}
		Field = field ?? ((StringName)"");
		if (property.ReadOnly)
		{
			HideEditor();
		}
		else
		{
			ShowEditor();
		}
		if (property.Description != "")
		{
			TooltipText = property.Description;
		}
		LastValue = GetPropertyValue();
		UpdateResetButton();
		UpdateKeyButton();
		UpdateValueSafely();
		_continuousBindingPassPending = true;
		SetProcess(enable: true);
	}

	public virtual void UpdateValue()
	{
	}

	public void UpdateValueSafely()
	{
		bool refreshingEditorValue = _refreshingEditorValue;
		_refreshingEditorValue = true;
		try
		{
			UpdateValue();
		}
		finally
		{
			_refreshingEditorValue = refreshingEditorValue;
		}
	}

	public void BeginContinuousEdit(StringName field = null)
	{
		if (_refreshingEditorValue || _continuousEditActive || !GodotObject.IsInstanceValid(Property) || !GodotObject.IsInstanceValid(Property.Object))
		{
			return;
		}
		if (IsInner)
		{
			if (GodotObject.IsInstanceValid(ContinuousEditOwner) && ContinuousEditOwner != this)
			{
				ContinuousEditOwner.BeginContinuousEdit();
			}
			return;
		}
		if (field == null || field.ToString() == "")
		{
			field = Field;
		}
		bool flag = field != null && field.ToString() != "";
		StringName stringName = (flag ? new StringName($"{Property.PropName}:{field}") : Property.PropName);
		_continuousEditActive = true;
		_continuousEditObject = Property.Object;
		_continuousEditProperty = stringName;
		_continuousEditRootProperty = Property.PropName;
		_continuousEditField = field ?? ((StringName)"");
		_continuousEditOldValue = ReadObjectProperty(Property.Object, Property.PropName);
		_continuousEditPreviewValue = (flag ? ReadIndexedObjectProperty(Property.Object, stringName) : _continuousEditOldValue);
		_continuousEditHasPreview = false;
	}

	public void CommitContinuousEdit()
	{
		if (!_continuousEditActive)
		{
			if (IsInner && GodotObject.IsInstanceValid(ContinuousEditOwner) && ContinuousEditOwner != this)
			{
				ContinuousEditOwner.CommitContinuousEdit();
			}
			return;
		}
		GodotObject continuousEditObject = _continuousEditObject;
		StringName continuousEditProperty = _continuousEditProperty;
		StringName continuousEditRootProperty = _continuousEditRootProperty;
		StringName continuousEditField = _continuousEditField;
		Variant continuousEditOldValue = _continuousEditOldValue;
		Variant continuousEditPreviewValue = _continuousEditPreviewValue;
		bool continuousEditHasPreview = _continuousEditHasPreview;
		Variant variant = (GodotObject.IsInstanceValid(continuousEditObject) ? ReadObjectProperty(continuousEditObject, continuousEditRootProperty) : continuousEditPreviewValue);
		ClearContinuousEditSession();
		if (continuousEditHasPreview && GodotObject.IsInstanceValid(continuousEditObject) && !continuousEditOldValue.Equals(variant))
		{
			if (GodotObject.IsInstanceValid(UndoRedoManager))
			{
				UndoRedoManager.CreateAction($"Set Property: {continuousEditProperty}", mergeMode: false, HistoryTypeOverride);
				UndoRedoManager.AddDoProperty(continuousEditObject, continuousEditRootProperty, variant);
				UndoRedoManager.AddUndoProperty(continuousEditObject, continuousEditRootProperty, continuousEditOldValue);
				UndoRedoManager.CommitAction();
			}
			else
			{
				WriteObjectProperty(continuousEditObject, continuousEditRootProperty, variant);
			}
			LastValue = GetPropertyValue();
			UpdateResetButton();
			EmitSignal(SignalName.ValueChanged, continuousEditObject, continuousEditRootProperty, continuousEditField, continuousEditPreviewValue);
			Property.SetCall();
		}
	}

	public void CancelContinuousEdit()
	{
		if (!_continuousEditActive)
		{
			if (IsInner && GodotObject.IsInstanceValid(ContinuousEditOwner) && ContinuousEditOwner != this)
			{
				ContinuousEditOwner.CancelContinuousEdit();
			}
			return;
		}
		GodotObject continuousEditObject = _continuousEditObject;
		StringName continuousEditRootProperty = _continuousEditRootProperty;
		Variant continuousEditOldValue = _continuousEditOldValue;
		bool continuousEditHasPreview = _continuousEditHasPreview;
		ClearContinuousEditSession();
		if (continuousEditHasPreview && GodotObject.IsInstanceValid(continuousEditObject))
		{
			WriteObjectProperty(continuousEditObject, continuousEditRootProperty, continuousEditOldValue);
			if (GodotObject.IsInstanceValid(Property))
			{
				Property.SetCall();
			}
		}
	}

	private void PreviewContinuousValue(Variant value, StringName field)
	{
		if (_continuousEditActive && GodotObject.IsInstanceValid(_continuousEditObject))
		{
			StringName stringName = field ?? ((StringName)"");
			if (((stringName.ToString() == "") ? Property.PropName : new StringName($"{Property.PropName}:{stringName}")) != _continuousEditProperty)
			{
				CommitContinuousEdit();
				BeginContinuousEdit(stringName);
			}
			if (_continuousEditActive && !((_continuousEditField != null && _continuousEditField.ToString() != "") ? ReadIndexedObjectProperty(_continuousEditObject, _continuousEditProperty) : ReadObjectProperty(_continuousEditObject, _continuousEditProperty)).Equals(value))
			{
				WriteContinuousProperty(_continuousEditObject, _continuousEditProperty, _continuousEditField, value);
				_continuousEditPreviewValue = value;
				_continuousEditHasPreview = true;
				LastValue = GetPropertyValue();
				UpdateResetButton();
				Property.SetCall();
			}
		}
	}

	private static void WriteContinuousProperty(GodotObject editObject, StringName propertyTrue, StringName field, Variant value)
	{
		if (field != null && field.ToString() != "")
		{
			WriteIndexedObjectProperty(editObject, propertyTrue, value);
		}
		else
		{
			WriteObjectProperty(editObject, propertyTrue, value);
		}
	}

	private void ClearContinuousEditSession()
	{
		_continuousEditActive = false;
		_continuousEditObject = null;
		_continuousEditProperty = "";
		_continuousEditRootProperty = "";
		_continuousEditField = "";
		_continuousEditOldValue = default;
		_continuousEditPreviewValue = default;
		_continuousEditHasPreview = false;
	}

	private void BindContinuousInputControls()
	{
		BindContinuousInputControlTree(this);
	}

	private void BindContinuousInputControlTree(Control control)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return;
		}
		if (control is LineEdit || control is TextEdit || control is SpinBox || control is Slider || control is ColorPickerButton)
		{
			BindContinuousInputControl(control);
		}
		SpinBox spinBox = control as SpinBox;
		if (spinBox != null)
		{
			spinBox.UpdateOnTextChanged = true;
			LineEdit lineEdit = spinBox.GetLineEdit();
			if (GodotObject.IsInstanceValid(lineEdit))
			{
				BindContinuousInputControl(lineEdit);
				if (_spinBoxTextPreviewIds.Add(spinBox.GetInstanceId()))
				{
					lineEdit.TextChanged += (string text) =>
					{
						ApplySpinBoxTextPreview(spinBox, text);
					};
				}
			}
		}
		foreach (Node child in control.GetChildren())
		{
			if (child is Control control2)
			{
				BindContinuousInputControlTree(control2);
			}
		}
	}

	private void BindContinuousInputControl(Control control)
	{
		ulong instanceId = control.GetInstanceId();
		if (!_continuousInputControlIds.Add(instanceId))
		{
			return;
		}
		_continuousInputControls.Add(control);
		if (control is ColorPickerButton colorPickerButton)
		{
			colorPickerButton.Pressed += () =>
			{
				BeginContinuousEdit();
			};
			colorPickerButton.PopupClosed += CommitContinuousEdit;
			ColorPicker picker = colorPickerButton.GetPicker();
			if (!GodotObject.IsInstanceValid(picker))
			{
				return;
			}
			picker.ColorChanged += (Color color) =>
			{
				if (_continuousEditActive)
				{
					ValueChange(color);
				}
			};
			return;
		}
		control.FocusEntered += () =>
		{
			BeginContinuousEdit();
		};
		control.FocusExited += () =>
		{
			Callable.From(CommitContinuousEditIfFocusLeft).CallDeferred();
		};
		if (control is LineEdit lineEdit)
		{
			lineEdit.TextSubmitted += (string _) =>
			{
				CommitContinuousEdit();
			};
		}
	}

	private void CommitContinuousEditIfFocusLeft()
	{
		if (!HasFocusedContinuousInput())
		{
			CommitContinuousEdit();
		}
	}

	private void ApplySpinBoxTextPreview(SpinBox spinBox, string text)
	{
		if (!_refreshingEditorValue && GodotObject.IsInstanceValid(spinBox) && !string.IsNullOrWhiteSpace(text))
		{
			string text2 = text.Trim();
			if (!string.IsNullOrEmpty(spinBox.Prefix) && text2.StartsWith(spinBox.Prefix))
			{
				string text3 = text2;
				int length = spinBox.Prefix.Length;
				text2 = text3.Substring(length, text3.Length - length).TrimStart();
			}
			if (!string.IsNullOrEmpty(spinBox.Suffix) && text2.EndsWith(spinBox.Suffix))
			{
				string text3 = text2;
				int length = spinBox.Suffix.Length;
				text2 = text3.Substring(0, text3.Length - length).TrimEnd();
			}
			if (double.TryParse(text2, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) || double.TryParse(text2, NumberStyles.Float, CultureInfo.CurrentCulture, out result))
			{
				spinBox.Value = result;
			}
		}
	}

	private bool HasFocusedContinuousInput()
	{
		foreach (Control continuousInputControl in _continuousInputControls)
		{
			if (GodotObject.IsInstanceValid(continuousInputControl) && continuousInputControl.HasFocus())
			{
				return true;
			}
		}
		return false;
	}

	protected static void SetLineEditTextPreservingCaret(LineEdit lineEdit, string text)
	{
		if (GodotObject.IsInstanceValid(lineEdit))
		{
			if (text == null)
			{
				text = "";
			}
			if (!(lineEdit.Text == text) && !lineEdit.HasFocus())
			{
				lineEdit.Text = text;
			}
		}
	}

	protected static void SetTextEditTextPreservingCaret(TextEdit textEdit, string text)
	{
		if (GodotObject.IsInstanceValid(textEdit))
		{
			if (text == null)
			{
				text = "";
			}
			if (!(textEdit.Text == text) && !textEdit.HasFocus())
			{
				textEdit.Text = text;
			}
		}
	}

	public void SetMixedValue()
	{
		_isMixedValue = true;
		if (GodotObject.IsInstanceValid(PropertyNameLabel))
		{
			PropertyNameLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.8f));
		}
		if (GodotObject.IsInstanceValid(PropertyEditor))
		{
			PropertyEditor.TooltipText = "多选节点值不一致";
		}
	}

	private void UpdateResetButton()
	{
		if (GodotObject.IsInstanceValid(Property) && GodotObject.IsInstanceValid(Property.Object) && GodotObject.IsInstanceValid(ResetButton))
		{
			ResetButton.Visible = ShouldShowResetButton();
		}
	}

	private bool ShouldShowResetButton()
	{
		if (Property.RestValue.VariantType != Variant.Type.Nil)
		{
			Variant propertyValue = GetPropertyValue();
			if (propertyValue.VariantType == Property.RestValue.VariantType)
			{
				return !propertyValue.Equals(Property.RestValue);
			}
			return false;
		}
		if (TryGetCustomRevertValue(out var revertValue))
		{
			return !GetPropertyValue().Equals(revertValue);
		}
		Variant variant = ClassDB.ClassGetPropertyDefaultValue(new StringName(Property.Object.GetClass()), Property.PropName);
		if (variant.VariantType != Variant.Type.Nil)
		{
			return !GetPropertyValue().Equals(variant);
		}
		return false;
	}

	private bool TryGetCustomRevertValue(out Variant revertValue)
	{
		revertValue = default;
		if (!GodotObject.IsInstanceValid(Property) || !GodotObject.IsInstanceValid(Property.Object))
		{
			return false;
		}
		if (Property.PropName == (StringName)"")
		{
			return false;
		}
		if (!Property.Object.HasMethod(new StringName("property_can_revert")))
		{
			return false;
		}
		if (!Property.Object.Call("property_can_revert", Property.PropName).AsBool())
		{
			return false;
		}
		revertValue = GetRevertValue();
		return true;
	}

	private void UpdateKeyButton()
	{
		if (GodotObject.IsInstanceValid(_keyButton))
		{
			if (!GodotObject.IsInstanceValid(Property) || !GodotObject.IsInstanceValid(Property.Object))
			{
				_keyButton.Visible = false;
			}
			else if (Property.ReadOnly)
			{
				_keyButton.Visible = false;
			}
			else
			{
				_keyButton.Visible = Property.Object is Node && (XWEditorInterface.Instance?.HasAnimationEditor() ?? false);
			}
		}
	}

	public Variant GetPropertyValue()
	{
		if (!GodotObject.IsInstanceValid(Property) || !GodotObject.IsInstanceValid(Property.Object))
		{
			return default;
		}
		if (!IsInner && Property.PropName == (StringName)"")
		{
			return Variant.From<GodotObject>(Property.Object);
		}
		Variant variant = ReadObjectProperty(Property.Object, Property.PropName);
		if (!IsInner)
		{
			if (Property.PropName == (StringName)"")
			{
				return Property.Object;
			}
			return variant;
		}
		Variant.Type variantType = variant.VariantType;
		if (variantType >= Variant.Type.Array && variantType <= Variant.Type.PackedVector4Array && Field != null && Field.ToString() != "")
		{
			int idx = Field.ToString().ToInt();
			return GetVariantArrayElement(variant, variantType, idx);
		}
		if (variantType == Variant.Type.Dictionary && Field != null && Field.ToString() != "")
		{
			Dictionary dictionary = variant.As<Dictionary>();
			if (dictionary.ContainsKey(Field))
			{
				return dictionary[Field];
			}
			string text = Field.ToString();
			if (dictionary.ContainsKey(text))
			{
				return dictionary[text];
			}
		}
		return variant;
	}

	public void ResetButtonPressed()
	{
		if (!GodotObject.IsInstanceValid(Property) || !GodotObject.IsInstanceValid(Property.Object))
		{
			return;
		}
		Variant revertValue = GetRevertValue();
		if (revertValue.VariantType != Variant.Type.Nil || Property.RestValue.VariantType != Variant.Type.Nil)
		{
			Variant value = ((Property.RestValue.VariantType != Variant.Type.Nil) ? Property.RestValue : revertValue);
			Variant value2 = ReadObjectProperty(Property.Object, Property.PropName);
			WriteObjectProperty(Property.Object, Property.PropName, value);
			if (GodotObject.IsInstanceValid(UndoRedoManager))
			{
				UndoRedoManager.CreateAction($"Reset Property: {Property.PropName}", mergeMode: false, HistoryTypeOverride);
				UndoRedoManager.AddDoProperty(Property.Object, Property.PropName, value);
				UndoRedoManager.AddUndoProperty(Property.Object, Property.PropName, value2);
				UndoRedoManager.CommitAction();
			}
			RefreshEditProperty();
		}
	}

	private Variant GetRevertValue()
	{
		if (!GodotObject.IsInstanceValid(Property) || !GodotObject.IsInstanceValid(Property.Object))
		{
			return default;
		}
		if (Property.Object.HasMethod(new StringName("property_get_revert")) && Property.PropName != (StringName)"" && Property.Object.Call("property_can_revert", Property.PropName).AsBool())
		{
			return Property.Object.Call("property_get_revert", Property.PropName);
		}
		return ClassDB.ClassGetPropertyDefaultValue(new StringName(Property.Object.GetClass()), Property.PropName);
	}

	public void RefreshEditProperty()
	{
		SetEditProperty(Property, Field);
	}

	public virtual void SetupRangeHint(string hintString)
	{
	}

	public void DoSelect()
	{
		Select = true;
		QueueRedraw();
	}

	public void Deselect()
	{
		Select = false;
		QueueRedraw();
	}

	public virtual void HideEditor()
	{
		if (GodotObject.IsInstanceValid(PropertyEditor))
		{
			PropertyEditor.Visible = false;
		}
	}

	public virtual void ShowEditor()
	{
		if (GodotObject.IsInstanceValid(PropertyEditor))
		{
			PropertyEditor.Visible = true;
		}
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (!(@event is InputEventMouseButton inputEventMouseButton))
		{
			return;
		}
		if (inputEventMouseButton.ButtonIndex == MouseButton.Right && inputEventMouseButton.Pressed)
		{
			if (GodotObject.IsInstanceValid(Property))
			{
				XWInspector inspector = GetInspector();
				if (GodotObject.IsInstanceValid(inspector))
				{
					inspector.ShowPropertyContextMenu(Property.PropName, GetGlobalMousePosition(), this);
				}
			}
		}
		else if (inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed)
		{
			XWInspector inspector2 = GetInspector();
			if (GodotObject.IsInstanceValid(inspector2))
			{
				inspector2.SelectEditor(this);
			}
		}
	}

	private void OnMouseEntered()
	{
		_hover = true;
		QueueRedraw();
	}

	private void OnMouseExited()
	{
		_hover = false;
		QueueRedraw();
	}

	public override void _Draw()
	{
		Vector2 size = Size;
		if (Select)
		{
			StyleBoxFlat styleBoxFlat = new StyleBoxFlat
			{
				BgColor = new Color(1f, 1f, 1f, 0.06f)
			};
			styleBoxFlat.SetContentMarginAll(0f);
			DrawStyleBox(styleBoxFlat, new Rect2(Vector2.Zero, size));
		}
		else if (_hover)
		{
			StyleBoxFlat styleBoxFlat2 = new StyleBoxFlat
			{
				BgColor = new Color(1f, 1f, 1f, 0.03f)
			};
			styleBoxFlat2.SetContentMarginAll(0f);
			DrawStyleBox(styleBoxFlat2, new Rect2(Vector2.Zero, size));
		}
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		if (!GodotObject.IsInstanceValid(Property) || Property.ReadOnly)
		{
			return false;
		}
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary = data.As<Dictionary>();
		if (!dictionary.ContainsKey("files"))
		{
			return false;
		}
		if (Property.Hint != PropertyHint.ResourceType)
		{
			return false;
		}
		string[] array = dictionary["files"].As<string[]>();
		if (array.Length == 0)
		{
			return false;
		}
		Resource resource = ResourceLoader.Load(array[0], "", ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		return resource.IsClass(Property.HintString);
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = data.As<Dictionary>();
		if (!dictionary.ContainsKey("files"))
		{
			return;
		}
		string[] array = dictionary["files"].As<string[]>();
		if (array.Length != 0)
		{
			Resource resource = ResourceLoader.Load(array[0], "", ResourceLoader.CacheMode.Reuse);
			if (GodotObject.IsInstanceValid(resource))
			{
				ValueChange(resource);
			}
		}
	}

	private void CopyValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			DisplayServer.ClipboardSet(propertyValue.ToString());
		}
	}

	private void PasteValue()
	{
		string text = DisplayServer.ClipboardGet();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Nil)
		{
			return;
		}
		Variant.Type variantType = propertyValue.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num > 8uL)
		{
			return;
		}
		switch ((int)num)
		{
		case 3:
			ValueChange(text);
			break;
		case 1:
			ValueChange(text.ToInt());
			break;
		case 2:
			ValueChange(text.ToFloat());
			break;
		case 0:
			ValueChange(text.ToLower() == "true" || text == "1");
			break;
		case 4:
		{
			string[] array2 = text.Replace("(", "").Replace(")", "").Split(",");
			if (array2.Length >= 2)
			{
				ValueChange(new Vector2(array2[0].ToFloat(), array2[1].ToFloat()));
			}
			break;
		}
		case 8:
		{
			string[] array = text.Replace("(", "").Replace(")", "").Split(",");
			if (array.Length >= 3)
			{
				ValueChange(new Vector3(array[0].ToFloat(), array[1].ToFloat(), array[2].ToFloat()));
			}
			break;
		}
		case 5:
		case 6:
		case 7:
			break;
		}
	}

	private void CopyPropertyPath()
	{
		if (GodotObject.IsInstanceValid(Property))
		{
			DisplayServer.ClipboardSet(Property.PropName.ToString());
		}
	}

	private void DeleteProperty()
	{
		if (GodotObject.IsInstanceValid(Property) && GodotObject.IsInstanceValid(Property.Object))
		{
			if (Property.Object.HasMethod(new StringName("remove_property")))
			{
				Property.Object.Call("remove_property", Property.PropName);
			}
			else if (Property.Object is Node node)
			{
				node.SetMeta(Property.PropName.ToString(), default);
			}
		}
	}

	protected XWInspector GetInspector()
	{
		for (Node parent = GetParent(); parent != null; parent = parent.GetParent())
		{
			if (parent is XWInspector result)
			{
				return result;
			}
		}
		return null;
	}

	internal static Variant ReadObjectProperty(GodotObject obj, StringName propertyName)
	{
		if (!GodotObject.IsInstanceValid(obj) || propertyName == null || propertyName == (StringName)"")
		{
			return default;
		}
		return obj.Get(propertyName);
	}

	internal static void WriteObjectProperty(GodotObject obj, StringName propertyName, Variant value)
	{
		if (GodotObject.IsInstanceValid(obj) && !(propertyName == null) && !(propertyName == (StringName)""))
		{
			obj.Set(propertyName, value);
		}
	}

	internal static Variant ReadIndexedObjectProperty(GodotObject obj, StringName propertyPath)
	{
		if (!GodotObject.IsInstanceValid(obj) || propertyPath == null || propertyPath == (StringName)"")
		{
			return default;
		}
		return obj.GetIndexed(new NodePath(propertyPath.ToString()));
	}

	internal static void WriteIndexedObjectProperty(GodotObject obj, StringName propertyPath, Variant value)
	{
		if (GodotObject.IsInstanceValid(obj) && !(propertyPath == null) && !(propertyPath == (StringName)""))
		{
			obj.SetIndexed(new NodePath(propertyPath.ToString()), value);
		}
	}

	private static Variant GetVariantArrayElement(Variant value, Variant.Type vt, int idx)
	{
		Variant.Type num = vt - 28;
		if ((ulong)num <= 10uL)
		{
			switch ((int)num)
			{
			case 0:
			{
				Godot.Collections.Array array5 = value.As<Godot.Collections.Array>();
				if (idx < 0 || idx >= array5.Count)
				{
					return default;
				}
				return array5[idx];
			}
			case 1:
			{
				byte[] array7 = value.As<byte[]>();
				return (byte)((idx >= 0 && idx < array7.Length) ? array7[idx] : 0);
			}
			case 2:
			{
				int[] array3 = value.As<int[]>();
				return (idx >= 0 && idx < array3.Length) ? array3[idx] : 0;
			}
			case 3:
			{
				long[] array8 = value.As<long[]>();
				return (idx >= 0 && idx < array8.Length) ? array8[idx] : 0;
			}
			case 4:
			{
				float[] array4 = value.As<float[]>();
				return (idx >= 0 && idx < array4.Length) ? array4[idx] : 0f;
			}
			case 5:
			{
				double[] array9 = value.As<double[]>();
				return (idx >= 0 && idx < array9.Length) ? array9[idx] : 0.0;
			}
			case 6:
			{
				string[] array2 = value.As<string[]>();
				return (idx >= 0 && idx < array2.Length) ? array2[idx] : null;
			}
			case 7:
			{
				Vector2[] array10 = value.As<Vector2[]>();
				return (idx >= 0 && idx < array10.Length) ? array10[idx] : default(Vector2);
			}
			case 8:
			{
				Vector3[] array11 = value.As<Vector3[]>();
				return (idx >= 0 && idx < array11.Length) ? array11[idx] : default(Vector3);
			}
			case 9:
			{
				Color[] array6 = value.As<Color[]>();
				return (idx >= 0 && idx < array6.Length) ? array6[idx] : default(Color);
			}
			case 10:
			{
				Vector4[] array = value.As<Vector4[]>();
				return (idx >= 0 && idx < array.Length) ? array[idx] : default(Vector4);
			}
			}
		}
		return default;
	}

	private static string CapitalizePropertyName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return name;
		}
		if (name.Length == 1)
		{
			return name.ToUpper();
		}
		bool flag = false;
		string text2;
		for (int i = 0; i < name.Length; i++)
		{
			if (name[i] == '_')
			{
				string text = name.Substring(0, i);
				text2 = name;
				int num = i + 1;
				name = text + " " + text2.Substring(num, text2.Length - num);
				flag = true;
			}
		}
		if (!flag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < name.Length; j++)
			{
				if (j > 0 && char.IsUpper(name[j]) && !char.IsUpper(name[j - 1]))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(name[j]);
			}
			name = stringBuilder.ToString();
		}
		ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(char.ToUpper(name[0]));
		text2 = name;
		return string.Concat(readOnlySpan, text2.Substring(1, text2.Length - 1));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(65)
		{
			new MethodInfo(MethodName.LoadIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateKeyButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizePropertyEditorLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeInspectorRowFill, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeEditorRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.WithMinimumHeight, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeIconButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.Float, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeControlTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeSpinBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spinBox", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSingleLineEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnKeyButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySplitRatio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSplitRatio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "ratio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValueSafely, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginContinuousEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitContinuousEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelContinuousEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewContinuousValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WriteContinuousProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyTrue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearContinuousEditSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindContinuousInputControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindContinuousInputControlTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindContinuousInputControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitContinuousEditIfFocusLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySpinBoxTextPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spinBox", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFocusedContinuousInput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetLineEditTextPreservingCaret, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "lineEdit", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("LineEdit"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTextEditTextPreservingCaret, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "textEdit", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextEdit"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMixedValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateResetButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldShowResetButton, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateKeyButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPropertyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRevertValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupRangeHint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoSelect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Deselect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnMouseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMouseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._CanDropData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._DropData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PasteValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopyPropertyPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetInspector, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadObjectProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WriteObjectProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadIndexedObjectProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WriteIndexedObjectProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetVariantArrayElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Int, "vt", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "idx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CapitalizePropertyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadIcons && args.Count == 0)
		{
			LoadIcons();
			ret = default;
			return true;
		}
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateKeyButton && args.Count == 0)
		{
			CreateKeyButton();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizePropertyEditorLayout && args.Count == 0)
		{
			NormalizePropertyEditorLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeInspectorRowFill && args.Count == 0)
		{
			NormalizeInspectorRowFill();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeEditorRoot && args.Count == 1)
		{
			NormalizeEditorRoot(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WithMinimumHeight && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(WithMinimumHeight(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeIconButton && args.Count == 2)
		{
			NormalizeIconButton(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeControlTree && args.Count == 1)
		{
			NormalizeControlTree(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeSpinBox && args.Count == 1)
		{
			NormalizeSpinBox(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSingleLineEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSingleLineEditor(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.OnKeyButtonPressed && args.Count == 0)
		{
			OnKeyButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySplitRatio && args.Count == 0)
		{
			ApplySplitRatio();
			ret = default;
			return true;
		}
		if (method == MethodName.SetSplitRatio && args.Count == 1)
		{
			SetSplitRatio(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ValueChange && args.Count == 2)
		{
			ValueChange(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
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
		if (method == MethodName.UpdateValueSafely && args.Count == 0)
		{
			UpdateValueSafely();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginContinuousEdit && args.Count == 1)
		{
			BeginContinuousEdit(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitContinuousEdit && args.Count == 0)
		{
			CommitContinuousEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelContinuousEdit && args.Count == 0)
		{
			CancelContinuousEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewContinuousValue && args.Count == 2)
		{
			PreviewContinuousValue(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.WriteContinuousProperty && args.Count == 4)
		{
			WriteContinuousProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearContinuousEditSession && args.Count == 0)
		{
			ClearContinuousEditSession();
			ret = default;
			return true;
		}
		if (method == MethodName.BindContinuousInputControls && args.Count == 0)
		{
			BindContinuousInputControls();
			ret = default;
			return true;
		}
		if (method == MethodName.BindContinuousInputControlTree && args.Count == 1)
		{
			BindContinuousInputControlTree(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindContinuousInputControl && args.Count == 1)
		{
			BindContinuousInputControl(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitContinuousEditIfFocusLeft && args.Count == 0)
		{
			CommitContinuousEditIfFocusLeft();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpinBoxTextPreview && args.Count == 2)
		{
			ApplySpinBoxTextPreview(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasFocusedContinuousInput && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFocusedContinuousInput());
			return true;
		}
		if (method == MethodName.SetLineEditTextPreservingCaret && args.Count == 2)
		{
			SetLineEditTextPreservingCaret(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTextEditTextPreservingCaret && args.Count == 2)
		{
			SetTextEditTextPreservingCaret(VariantUtils.ConvertTo<TextEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMixedValue && args.Count == 0)
		{
			SetMixedValue();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateResetButton && args.Count == 0)
		{
			UpdateResetButton();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldShowResetButton && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldShowResetButton());
			return true;
		}
		if (method == MethodName.UpdateKeyButton && args.Count == 0)
		{
			UpdateKeyButton();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPropertyValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetPropertyValue());
			return true;
		}
		if (method == MethodName.ResetButtonPressed && args.Count == 0)
		{
			ResetButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.GetRevertValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetRevertValue());
			return true;
		}
		if (method == MethodName.RefreshEditProperty && args.Count == 0)
		{
			RefreshEditProperty();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupRangeHint && args.Count == 1)
		{
			SetupRangeHint(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoSelect && args.Count == 0)
		{
			DoSelect();
			ret = default;
			return true;
		}
		if (method == MethodName.Deselect && args.Count == 0)
		{
			Deselect();
			ret = default;
			return true;
		}
		if (method == MethodName.HideEditor && args.Count == 0)
		{
			HideEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowEditor && args.Count == 0)
		{
			ShowEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGuiInput && args.Count == 1)
		{
			OnGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMouseEntered && args.Count == 0)
		{
			OnMouseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMouseExited && args.Count == 0)
		{
			OnMouseExited();
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName._CanDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanDropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._DropData && args.Count == 2)
		{
			_DropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyValue && args.Count == 0)
		{
			CopyValue();
			ret = default;
			return true;
		}
		if (method == MethodName.PasteValue && args.Count == 0)
		{
			PasteValue();
			ret = default;
			return true;
		}
		if (method == MethodName.CopyPropertyPath && args.Count == 0)
		{
			CopyPropertyPath();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteProperty && args.Count == 0)
		{
			DeleteProperty();
			ret = default;
			return true;
		}
		if (method == MethodName.GetInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(GetInspector());
			return true;
		}
		if (method == MethodName.ReadObjectProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.WriteObjectProperty && args.Count == 3)
		{
			WriteObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadIndexedObjectProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadIndexedObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.WriteIndexedObjectProperty && args.Count == 3)
		{
			WriteIndexedObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetVariantArrayElement && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetVariantArrayElement(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.CapitalizePropertyName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CapitalizePropertyName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadIcons && args.Count == 0)
		{
			LoadIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.WithMinimumHeight && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(WithMinimumHeight(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeIconButton && args.Count == 2)
		{
			NormalizeIconButton(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeControlTree && args.Count == 1)
		{
			NormalizeControlTree(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeSpinBox && args.Count == 1)
		{
			NormalizeSpinBox(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSingleLineEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSingleLineEditor(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.WriteContinuousProperty && args.Count == 4)
		{
			WriteContinuousProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLineEditTextPreservingCaret && args.Count == 2)
		{
			SetLineEditTextPreservingCaret(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTextEditTextPreservingCaret && args.Count == 2)
		{
			SetTextEditTextPreservingCaret(VariantUtils.ConvertTo<TextEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadObjectProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.WriteObjectProperty && args.Count == 3)
		{
			WriteObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadIndexedObjectProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadIndexedObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.WriteIndexedObjectProperty && args.Count == 3)
		{
			WriteIndexedObjectProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetVariantArrayElement && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetVariantArrayElement(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.CapitalizePropertyName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CapitalizePropertyName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.LoadIcons)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CreateKeyButton)
		{
			return true;
		}
		if (method == MethodName.NormalizePropertyEditorLayout)
		{
			return true;
		}
		if (method == MethodName.NormalizeInspectorRowFill)
		{
			return true;
		}
		if (method == MethodName.NormalizeEditorRoot)
		{
			return true;
		}
		if (method == MethodName.WithMinimumHeight)
		{
			return true;
		}
		if (method == MethodName.NormalizeIconButton)
		{
			return true;
		}
		if (method == MethodName.NormalizeControlTree)
		{
			return true;
		}
		if (method == MethodName.NormalizeSpinBox)
		{
			return true;
		}
		if (method == MethodName.IsSingleLineEditor)
		{
			return true;
		}
		if (method == MethodName.OnKeyButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ApplySplitRatio)
		{
			return true;
		}
		if (method == MethodName.SetSplitRatio)
		{
			return true;
		}
		if (method == MethodName.ValueChange)
		{
			return true;
		}
		if (method == MethodName.GetValue)
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
		if (method == MethodName.UpdateValueSafely)
		{
			return true;
		}
		if (method == MethodName.BeginContinuousEdit)
		{
			return true;
		}
		if (method == MethodName.CommitContinuousEdit)
		{
			return true;
		}
		if (method == MethodName.CancelContinuousEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewContinuousValue)
		{
			return true;
		}
		if (method == MethodName.WriteContinuousProperty)
		{
			return true;
		}
		if (method == MethodName.ClearContinuousEditSession)
		{
			return true;
		}
		if (method == MethodName.BindContinuousInputControls)
		{
			return true;
		}
		if (method == MethodName.BindContinuousInputControlTree)
		{
			return true;
		}
		if (method == MethodName.BindContinuousInputControl)
		{
			return true;
		}
		if (method == MethodName.CommitContinuousEditIfFocusLeft)
		{
			return true;
		}
		if (method == MethodName.ApplySpinBoxTextPreview)
		{
			return true;
		}
		if (method == MethodName.HasFocusedContinuousInput)
		{
			return true;
		}
		if (method == MethodName.SetLineEditTextPreservingCaret)
		{
			return true;
		}
		if (method == MethodName.SetTextEditTextPreservingCaret)
		{
			return true;
		}
		if (method == MethodName.SetMixedValue)
		{
			return true;
		}
		if (method == MethodName.UpdateResetButton)
		{
			return true;
		}
		if (method == MethodName.ShouldShowResetButton)
		{
			return true;
		}
		if (method == MethodName.UpdateKeyButton)
		{
			return true;
		}
		if (method == MethodName.GetPropertyValue)
		{
			return true;
		}
		if (method == MethodName.ResetButtonPressed)
		{
			return true;
		}
		if (method == MethodName.GetRevertValue)
		{
			return true;
		}
		if (method == MethodName.RefreshEditProperty)
		{
			return true;
		}
		if (method == MethodName.SetupRangeHint)
		{
			return true;
		}
		if (method == MethodName.DoSelect)
		{
			return true;
		}
		if (method == MethodName.Deselect)
		{
			return true;
		}
		if (method == MethodName.HideEditor)
		{
			return true;
		}
		if (method == MethodName.ShowEditor)
		{
			return true;
		}
		if (method == MethodName.OnGuiInput)
		{
			return true;
		}
		if (method == MethodName.OnMouseEntered)
		{
			return true;
		}
		if (method == MethodName.OnMouseExited)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName._CanDropData)
		{
			return true;
		}
		if (method == MethodName._DropData)
		{
			return true;
		}
		if (method == MethodName.CopyValue)
		{
			return true;
		}
		if (method == MethodName.PasteValue)
		{
			return true;
		}
		if (method == MethodName.CopyPropertyPath)
		{
			return true;
		}
		if (method == MethodName.DeleteProperty)
		{
			return true;
		}
		if (method == MethodName.GetInspector)
		{
			return true;
		}
		if (method == MethodName.ReadObjectProperty)
		{
			return true;
		}
		if (method == MethodName.WriteObjectProperty)
		{
			return true;
		}
		if (method == MethodName.ReadIndexedObjectProperty)
		{
			return true;
		}
		if (method == MethodName.WriteIndexedObjectProperty)
		{
			return true;
		}
		if (method == MethodName.GetVariantArrayElement)
		{
			return true;
		}
		if (method == MethodName.CapitalizePropertyName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.PropertyEditorPath)
		{
			PropertyEditorPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.Select)
		{
			Select = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsInner)
		{
			IsInner = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.HistoryTypeOverride)
		{
			HistoryTypeOverride = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ContinuousEditOwner)
		{
			ContinuousEditOwner = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		if (name == PropertyName.SplitRatio)
		{
			SplitRatio = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.IsResourceEditor)
		{
			IsResourceEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PropertyNameLabel)
		{
			PropertyNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.ResetButton)
		{
			ResetButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.InformationContainner)
		{
			InformationContainner = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.PropertyEditor)
		{
			PropertyEditor = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.Property)
		{
			Property = VariantUtils.ConvertTo<XWInspectorProperty>(in value);
			return true;
		}
		if (name == PropertyName.Field)
		{
			Field = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.PropertyNameLock)
		{
			PropertyNameLock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.UndoRedoManager)
		{
			UndoRedoManager = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName.LastValue)
		{
			LastValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName._hover)
		{
			_hover = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isMixedValue)
		{
			_isMixedValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._keyButton)
		{
			_keyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditActive)
		{
			_continuousEditActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditObject)
		{
			_continuousEditObject = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditProperty)
		{
			_continuousEditProperty = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditRootProperty)
		{
			_continuousEditRootProperty = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditField)
		{
			_continuousEditField = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditOldValue)
		{
			_continuousEditOldValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditPreviewValue)
		{
			_continuousEditPreviewValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName._continuousEditHasPreview)
		{
			_continuousEditHasPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._refreshingEditorValue)
		{
			_refreshingEditorValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._continuousBindingPassPending)
		{
			_continuousBindingPassPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.PropertyEditorPath)
		{
			value = VariantUtils.CreateFrom<NodePath>(PropertyEditorPath);
			return true;
		}
		bool from;
		if (name == PropertyName.Select)
		{
			from = Select;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsInner)
		{
			from = IsInner;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.HistoryTypeOverride)
		{
			from2 = HistoryTypeOverride;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsContinuousEditActive)
		{
			from = IsContinuousEditActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ContinuousEditOwner)
		{
			value = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(ContinuousEditOwner);
			return true;
		}
		if (name == PropertyName.ContinuousInputControlCount)
		{
			from2 = ContinuousInputControlCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ContinuousBindingRootDescription)
		{
			value = VariantUtils.CreateFrom<string>(ContinuousBindingRootDescription);
			return true;
		}
		if (name == PropertyName.SplitRatio)
		{
			value = VariantUtils.CreateFrom<float>(SplitRatio);
			return true;
		}
		if (name == PropertyName.IsResourceEditor)
		{
			from = IsResourceEditor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PropertyNameLabel)
		{
			value = VariantUtils.CreateFrom(in PropertyNameLabel);
			return true;
		}
		if (name == PropertyName.ResetButton)
		{
			value = VariantUtils.CreateFrom(in ResetButton);
			return true;
		}
		if (name == PropertyName.InformationContainner)
		{
			value = VariantUtils.CreateFrom(in InformationContainner);
			return true;
		}
		if (name == PropertyName.PropertyEditor)
		{
			value = VariantUtils.CreateFrom(in PropertyEditor);
			return true;
		}
		if (name == PropertyName.Property)
		{
			value = VariantUtils.CreateFrom(in Property);
			return true;
		}
		if (name == PropertyName.Field)
		{
			value = VariantUtils.CreateFrom(in Field);
			return true;
		}
		if (name == PropertyName.PropertyNameLock)
		{
			value = VariantUtils.CreateFrom(in PropertyNameLock);
			return true;
		}
		if (name == PropertyName.UndoRedoManager)
		{
			value = VariantUtils.CreateFrom(in UndoRedoManager);
			return true;
		}
		if (name == PropertyName.LastValue)
		{
			value = VariantUtils.CreateFrom(in LastValue);
			return true;
		}
		if (name == PropertyName._hover)
		{
			value = VariantUtils.CreateFrom(in _hover);
			return true;
		}
		if (name == PropertyName._isMixedValue)
		{
			value = VariantUtils.CreateFrom(in _isMixedValue);
			return true;
		}
		if (name == PropertyName._keyButton)
		{
			value = VariantUtils.CreateFrom(in _keyButton);
			return true;
		}
		if (name == PropertyName._continuousEditActive)
		{
			value = VariantUtils.CreateFrom(in _continuousEditActive);
			return true;
		}
		if (name == PropertyName._continuousEditObject)
		{
			value = VariantUtils.CreateFrom(in _continuousEditObject);
			return true;
		}
		if (name == PropertyName._continuousEditProperty)
		{
			value = VariantUtils.CreateFrom(in _continuousEditProperty);
			return true;
		}
		if (name == PropertyName._continuousEditRootProperty)
		{
			value = VariantUtils.CreateFrom(in _continuousEditRootProperty);
			return true;
		}
		if (name == PropertyName._continuousEditField)
		{
			value = VariantUtils.CreateFrom(in _continuousEditField);
			return true;
		}
		if (name == PropertyName._continuousEditOldValue)
		{
			value = VariantUtils.CreateFrom(in _continuousEditOldValue);
			return true;
		}
		if (name == PropertyName._continuousEditPreviewValue)
		{
			value = VariantUtils.CreateFrom(in _continuousEditPreviewValue);
			return true;
		}
		if (name == PropertyName._continuousEditHasPreview)
		{
			value = VariantUtils.CreateFrom(in _continuousEditHasPreview);
			return true;
		}
		if (name == PropertyName._refreshingEditorValue)
		{
			value = VariantUtils.CreateFrom(in _refreshingEditorValue);
			return true;
		}
		if (name == PropertyName._continuousBindingPassPending)
		{
			value = VariantUtils.CreateFrom(in _continuousBindingPassPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.PropertyEditorPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.PropertyNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ResetButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.InformationContainner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.PropertyEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Property, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Select, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.Field, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PropertyNameLock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.UndoRedoManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.HistoryTypeOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.LastValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsContinuousEditActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ContinuousEditOwner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ContinuousInputControlCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ContinuousBindingRootDescription, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.SplitRatio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsResourceEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hover, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isMixedValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._keyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._continuousEditActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._continuousEditObject, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._continuousEditProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._continuousEditRootProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._continuousEditField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._continuousEditOldValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._continuousEditPreviewValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._continuousEditHasPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._refreshingEditorValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._continuousBindingPassPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.PropertyEditorPath, Variant.From<NodePath>(PropertyEditorPath));
		info.AddProperty(PropertyName.Select, Variant.From<bool>(Select));
		info.AddProperty(PropertyName.IsInner, Variant.From<bool>(IsInner));
		info.AddProperty(PropertyName.HistoryTypeOverride, Variant.From<int>(HistoryTypeOverride));
		info.AddProperty(PropertyName.ContinuousEditOwner, Variant.From<XWInspectorPropertyEditorBase>(ContinuousEditOwner));
		info.AddProperty(PropertyName.SplitRatio, Variant.From<float>(SplitRatio));
		info.AddProperty(PropertyName.IsResourceEditor, Variant.From<bool>(IsResourceEditor));
		info.AddProperty(PropertyName.PropertyNameLabel, Variant.From(in PropertyNameLabel));
		info.AddProperty(PropertyName.ResetButton, Variant.From(in ResetButton));
		info.AddProperty(PropertyName.InformationContainner, Variant.From(in InformationContainner));
		info.AddProperty(PropertyName.PropertyEditor, Variant.From(in PropertyEditor));
		info.AddProperty(PropertyName.Property, Variant.From(in Property));
		info.AddProperty(PropertyName.Field, Variant.From(in Field));
		info.AddProperty(PropertyName.PropertyNameLock, Variant.From(in PropertyNameLock));
		info.AddProperty(PropertyName.UndoRedoManager, Variant.From(in UndoRedoManager));
		info.AddProperty(PropertyName.LastValue, Variant.From(in LastValue));
		info.AddProperty(PropertyName._hover, Variant.From(in _hover));
		info.AddProperty(PropertyName._isMixedValue, Variant.From(in _isMixedValue));
		info.AddProperty(PropertyName._keyButton, Variant.From(in _keyButton));
		info.AddProperty(PropertyName._continuousEditActive, Variant.From(in _continuousEditActive));
		info.AddProperty(PropertyName._continuousEditObject, Variant.From(in _continuousEditObject));
		info.AddProperty(PropertyName._continuousEditProperty, Variant.From(in _continuousEditProperty));
		info.AddProperty(PropertyName._continuousEditRootProperty, Variant.From(in _continuousEditRootProperty));
		info.AddProperty(PropertyName._continuousEditField, Variant.From(in _continuousEditField));
		info.AddProperty(PropertyName._continuousEditOldValue, Variant.From(in _continuousEditOldValue));
		info.AddProperty(PropertyName._continuousEditPreviewValue, Variant.From(in _continuousEditPreviewValue));
		info.AddProperty(PropertyName._continuousEditHasPreview, Variant.From(in _continuousEditHasPreview));
		info.AddProperty(PropertyName._refreshingEditorValue, Variant.From(in _refreshingEditorValue));
		info.AddProperty(PropertyName._continuousBindingPassPending, Variant.From(in _continuousBindingPassPending));
		info.AddSignalEventDelegate(SignalName.ValueChanged, backing_ValueChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.PropertyEditorPath, out var value))
		{
			PropertyEditorPath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.Select, out var value2))
		{
			Select = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsInner, out var value3))
		{
			IsInner = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.HistoryTypeOverride, out var value4))
		{
			HistoryTypeOverride = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ContinuousEditOwner, out var value5))
		{
			ContinuousEditOwner = value5.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetProperty(PropertyName.SplitRatio, out var value6))
		{
			SplitRatio = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.IsResourceEditor, out var value7))
		{
			IsResourceEditor = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PropertyNameLabel, out var value8))
		{
			PropertyNameLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.ResetButton, out var value9))
		{
			ResetButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.InformationContainner, out var value10))
		{
			InformationContainner = value10.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.PropertyEditor, out var value11))
		{
			PropertyEditor = value11.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.Property, out var value12))
		{
			Property = value12.As<XWInspectorProperty>();
		}
		if (info.TryGetProperty(PropertyName.Field, out var value13))
		{
			Field = value13.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.PropertyNameLock, out var value14))
		{
			PropertyNameLock = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.UndoRedoManager, out var value15))
		{
			UndoRedoManager = value15.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName.LastValue, out var value16))
		{
			LastValue = value16.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName._hover, out var value17))
		{
			_hover = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isMixedValue, out var value18))
		{
			_isMixedValue = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._keyButton, out var value19))
		{
			_keyButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditActive, out var value20))
		{
			_continuousEditActive = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditObject, out var value21))
		{
			_continuousEditObject = value21.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditProperty, out var value22))
		{
			_continuousEditProperty = value22.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditRootProperty, out var value23))
		{
			_continuousEditRootProperty = value23.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditField, out var value24))
		{
			_continuousEditField = value24.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditOldValue, out var value25))
		{
			_continuousEditOldValue = value25.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditPreviewValue, out var value26))
		{
			_continuousEditPreviewValue = value26.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName._continuousEditHasPreview, out var value27))
		{
			_continuousEditHasPreview = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._refreshingEditorValue, out var value28))
		{
			_refreshingEditorValue = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._continuousBindingPassPending, out var value29))
		{
			_continuousBindingPassPending = value29.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<ValueChangedEventHandler>(SignalName.ValueChanged, out var value30))
		{
			backing_ValueChanged = value30;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.ValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	protected void EmitSignalValueChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		StringName valueChanged = SignalName.ValueChanged;
		_003C_003Ey__InlineArray4<Variant> buffer = default;
		buffer[0] = obj;
		buffer[1] = property;
		buffer[2] = field;
		buffer[3] = value;
		EmitSignal(valueChanged, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ValueChanged && args.Count == 4)
		{
			backing_ValueChanged?.Invoke(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ValueChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
