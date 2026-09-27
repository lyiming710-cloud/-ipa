using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Registry;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/XWInspectorPropertyEditorPackedArrayBase.cs")]
public abstract class XWInspectorPropertyEditorPackedArrayBase : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName GetDefaultElement = "GetDefaultElement";

		public static readonly StringName GetArraySize = "GetArraySize";

		public static readonly StringName GetElement = "GetElement";

		public static readonly StringName SetElement = "SetElement";

		public static readonly StringName AppendElement = "AppendElement";

		public static readonly StringName RemoveAt = "RemoveAt";

		public static readonly StringName GetArrayAsVariant = "GetArrayAsVariant";

		public static readonly StringName LoadArrayFromVariant = "LoadArrayFromVariant";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName RefreshProperty = "RefreshProperty";

		public static readonly StringName InnerValueChange = "InnerValueChange";

		public static readonly StringName WriteBack = "WriteBack";

		public static readonly StringName AddElement = "AddElement";

		public static readonly StringName RemoveItem = "RemoveItem";

		public static readonly StringName ArrayButtonPressed = "ArrayButtonPressed";

		public static readonly StringName AddButtonPressed = "AddButtonPressed";

		public new static readonly StringName HideEditor = "HideEditor";

		public new static readonly StringName ShowEditor = "ShowEditor";

		public static readonly StringName SizeUpdate = "SizeUpdate";

		public static readonly StringName GetElementEditorValue = "GetElementEditorValue";

		public static readonly StringName HasFocusedElementEditor = "HasFocusedElementEditor";

		public static readonly StringName IsControlTreeFocused = "IsControlTreeFocused";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName ArrayTypeName = "ArrayTypeName";

		public static readonly StringName ElementType = "ElementType";

		public static readonly StringName _arrayButton = "_arrayButton";

		public static readonly StringName _addButton = "_addButton";

		public static readonly StringName _editContainer = "_editContainer";

		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName _preserveElementEditorsWhileFocused = "_preserveElementEditorsWhileFocused";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	protected Button _arrayButton;

	protected Button _addButton;

	protected PanelContainer _editContainer;

	protected VBoxContainer _editorContainer;

	private bool _preserveElementEditorsWhileFocused;

	protected abstract string ArrayTypeName { get; }

	protected abstract Variant.Type ElementType { get; }

	protected abstract Variant GetDefaultElement();

	protected abstract int GetArraySize();

	protected abstract Variant GetElement(int index);

	protected abstract void SetElement(int index, Variant value);

	protected abstract void AppendElement(Variant value);

	protected abstract void RemoveAt(int index);

	protected abstract Variant GetArrayAsVariant();

	protected abstract void LoadArrayFromVariant(Variant value);

	public override void _Ready()
	{
		base._Ready();
		_arrayButton = GetNode<Button>("%ArrayButton");
		_addButton = GetNode<Button>("%AddButton");
		_editContainer = GetNode<PanelContainer>("%EditContainer");
		_editorContainer = GetNode<VBoxContainer>("%EditorContainer");
		_arrayButton.Toggled += ArrayButtonPressed;
		_addButton.Pressed += AddButtonPressed;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		LoadArrayFromVariant(GetPropertyValue());
		SizeUpdate();
		if (!_preserveElementEditorsWhileFocused || !HasFocusedElementEditor())
		{
			_preserveElementEditorsWhileFocused = false;
			RefreshProperty();
		}
	}

	public override void UpdateValue()
	{
		LoadArrayFromVariant(GetPropertyValue());
		SizeUpdate();
		if (!_preserveElementEditorsWhileFocused || !HasFocusedElementEditor())
		{
			_preserveElementEditorsWhileFocused = false;
			RefreshProperty();
		}
	}

	public override Variant GetValue()
	{
		return GetArrayAsVariant();
	}

	protected void RefreshProperty()
	{
		foreach (Node child in _editorContainer.GetChildren())
		{
			child.QueueFree();
		}
		int arraySize = GetArraySize();
		for (int i = 0; i < arraySize; i++)
		{
			XWInspectorPropertyEditorArrayItemContainer xWInspectorPropertyEditorArrayItemContainer = XWInspectorPropertyEditorArrayItemContainer.Create();
			_editorContainer.AddChild(xWInspectorPropertyEditorArrayItemContainer, forceReadableName: false, InternalMode.Disabled);
			xWInspectorPropertyEditorArrayItemContainer.Remove += RemoveItem;
			XWInspectorPropertyEditorBase typeEditor = XWTypeRegistry.Instance.GetTypeEditor(ElementType);
			if (typeEditor != null)
			{
				typeEditor.IsInner = true;
				xWInspectorPropertyEditorArrayItemContainer.AddEditor(typeEditor);
				typeEditor.SetEditProperty(Property, new StringName(i.ToString()));
				int itemIndex = i;
				XWInspectorPropertyEditorBase itemEditor = typeEditor;
				typeEditor.ValueChanged += (GodotObject obj, StringName prop, StringName fld, Variant val) =>
				{
					InnerValueChange(itemIndex, itemEditor, val);
				};
			}
		}
	}

	protected void InnerValueChange(int index, XWInspectorPropertyEditorBase editor, Variant fallbackValue)
	{
		if (index >= 0 && index < GetArraySize())
		{
			SetElement(index, GetElementEditorValue(editor, fallbackValue, GetElement(index)));
			_preserveElementEditorsWhileFocused = true;
			ValueChange(GetArrayAsVariant());
			SizeUpdate();
			if (!_preserveElementEditorsWhileFocused || !HasFocusedElementEditor())
			{
				_preserveElementEditorsWhileFocused = false;
				RefreshProperty();
			}
		}
	}

	protected void WriteBack()
	{
		if (GodotObject.IsInstanceValid(Property) && GodotObject.IsInstanceValid(Property.Object))
		{
			XWInspectorPropertyEditorBase.WriteObjectProperty(Property.Object, Property.PropName, GetArrayAsVariant());
		}
	}

	protected void AddElement()
	{
		_preserveElementEditorsWhileFocused = false;
		AppendElement(GetDefaultElement());
		WriteBack();
		Property?.SetCall();
		SizeUpdate();
		RefreshProperty();
	}

	protected void RemoveItem(XWInspectorPropertyEditorArrayItemContainer itemContainer)
	{
		int num = _editorContainer.GetChildren().IndexOf(itemContainer);
		if (num != -1)
		{
			_preserveElementEditorsWhileFocused = false;
			RemoveAt(num);
			WriteBack();
			itemContainer.QueueFree();
			Property?.SetCall();
			SizeUpdate();
			RefreshProperty();
		}
	}

	protected void ArrayButtonPressed(bool toggledOn)
	{
		_editContainer.Visible = toggledOn;
	}

	protected void AddButtonPressed()
	{
		AddElement();
	}

	public override void HideEditor()
	{
		base.HideEditor();
		_arrayButton.ButtonPressed = false;
		_editContainer.Visible = false;
	}

	public override void ShowEditor()
	{
		base.ShowEditor();
		_arrayButton.ButtonPressed = false;
		_editContainer.Visible = true;
	}

	protected void SizeUpdate()
	{
		_arrayButton.Text = $"{ArrayTypeName}( 大小 {GetArraySize()} )";
	}

	private static Variant GetElementEditorValue(XWInspectorPropertyEditorBase editor, Variant fallbackValue, Variant currentValue)
	{
		if (!GodotObject.IsInstanceValid(editor))
		{
			return fallbackValue;
		}
		Variant value = editor.GetValue();
		if (value.VariantType == Variant.Type.Nil && fallbackValue.VariantType != Variant.Type.Nil)
		{
			return fallbackValue;
		}
		if (fallbackValue.VariantType != Variant.Type.Nil && value.Equals(currentValue) && !fallbackValue.Equals(currentValue))
		{
			return fallbackValue;
		}
		return value;
	}

	private bool HasFocusedElementEditor()
	{
		if (GodotObject.IsInstanceValid(_editorContainer))
		{
			return IsControlTreeFocused(_editorContainer);
		}
		return false;
	}

	private static bool IsControlTreeFocused(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		if (node is Control control && control.HasFocus())
		{
			return true;
		}
		foreach (Node child in node.GetChildren())
		{
			if (child != null)
			{
				Node node2 = child;
				if (IsControlTreeFocused(node2))
				{
					return true;
				}
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName.GetDefaultElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetArraySize, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AppendElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArrayAsVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadArrayFromVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InnerValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "fallbackValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.WriteBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "itemContainer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ArrayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SizeUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetElementEditorValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "fallbackValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "currentValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFocusedElementEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsControlTreeFocused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetDefaultElement && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetDefaultElement());
			return true;
		}
		if (method == MethodName.GetArraySize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetArraySize());
			return true;
		}
		if (method == MethodName.GetElement && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetElement(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetElement && args.Count == 2)
		{
			SetElement(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AppendElement && args.Count == 1)
		{
			AppendElement(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveAt && args.Count == 1)
		{
			RemoveAt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetArrayAsVariant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetArrayAsVariant());
			return true;
		}
		if (method == MethodName.LoadArrayFromVariant && args.Count == 1)
		{
			LoadArrayFromVariant(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
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
		if (method == MethodName.RefreshProperty && args.Count == 0)
		{
			RefreshProperty();
			ret = default;
			return true;
		}
		if (method == MethodName.InnerValueChange && args.Count == 3)
		{
			InnerValueChange(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.WriteBack && args.Count == 0)
		{
			WriteBack();
			ret = default;
			return true;
		}
		if (method == MethodName.AddElement && args.Count == 0)
		{
			AddElement();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveItem && args.Count == 1)
		{
			RemoveItem(VariantUtils.ConvertTo<XWInspectorPropertyEditorArrayItemContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArrayButtonPressed && args.Count == 1)
		{
			ArrayButtonPressed(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddButtonPressed && args.Count == 0)
		{
			AddButtonPressed();
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
		if (method == MethodName.SizeUpdate && args.Count == 0)
		{
			SizeUpdate();
			ret = default;
			return true;
		}
		if (method == MethodName.GetElementEditorValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetElementEditorValue(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2])));
			return true;
		}
		if (method == MethodName.HasFocusedElementEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFocusedElementEditor());
			return true;
		}
		if (method == MethodName.IsControlTreeFocused && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsControlTreeFocused(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetElementEditorValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetElementEditorValue(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2])));
			return true;
		}
		if (method == MethodName.IsControlTreeFocused && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsControlTreeFocused(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetDefaultElement)
		{
			return true;
		}
		if (method == MethodName.GetArraySize)
		{
			return true;
		}
		if (method == MethodName.GetElement)
		{
			return true;
		}
		if (method == MethodName.SetElement)
		{
			return true;
		}
		if (method == MethodName.AppendElement)
		{
			return true;
		}
		if (method == MethodName.RemoveAt)
		{
			return true;
		}
		if (method == MethodName.GetArrayAsVariant)
		{
			return true;
		}
		if (method == MethodName.LoadArrayFromVariant)
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
		if (method == MethodName.RefreshProperty)
		{
			return true;
		}
		if (method == MethodName.InnerValueChange)
		{
			return true;
		}
		if (method == MethodName.WriteBack)
		{
			return true;
		}
		if (method == MethodName.AddElement)
		{
			return true;
		}
		if (method == MethodName.RemoveItem)
		{
			return true;
		}
		if (method == MethodName.ArrayButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AddButtonPressed)
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
		if (method == MethodName.SizeUpdate)
		{
			return true;
		}
		if (method == MethodName.GetElementEditorValue)
		{
			return true;
		}
		if (method == MethodName.HasFocusedElementEditor)
		{
			return true;
		}
		if (method == MethodName.IsControlTreeFocused)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._arrayButton)
		{
			_arrayButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._addButton)
		{
			_addButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._editContainer)
		{
			_editContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			_editorContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._preserveElementEditorsWhileFocused)
		{
			_preserveElementEditorsWhileFocused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ArrayTypeName)
		{
			value = VariantUtils.CreateFrom<string>(ArrayTypeName);
			return true;
		}
		if (name == PropertyName.ElementType)
		{
			value = VariantUtils.CreateFrom<Variant.Type>(ElementType);
			return true;
		}
		if (name == PropertyName._arrayButton)
		{
			value = VariantUtils.CreateFrom(in _arrayButton);
			return true;
		}
		if (name == PropertyName._addButton)
		{
			value = VariantUtils.CreateFrom(in _addButton);
			return true;
		}
		if (name == PropertyName._editContainer)
		{
			value = VariantUtils.CreateFrom(in _editContainer);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			value = VariantUtils.CreateFrom(in _editorContainer);
			return true;
		}
		if (name == PropertyName._preserveElementEditorsWhileFocused)
		{
			value = VariantUtils.CreateFrom(in _preserveElementEditorsWhileFocused);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._arrayButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveElementEditorsWhileFocused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ArrayTypeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ElementType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._arrayButton, Variant.From(in _arrayButton));
		info.AddProperty(PropertyName._addButton, Variant.From(in _addButton));
		info.AddProperty(PropertyName._editContainer, Variant.From(in _editContainer));
		info.AddProperty(PropertyName._editorContainer, Variant.From(in _editorContainer));
		info.AddProperty(PropertyName._preserveElementEditorsWhileFocused, Variant.From(in _preserveElementEditorsWhileFocused));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._arrayButton, out var value))
		{
			_arrayButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._addButton, out var value2))
		{
			_addButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._editContainer, out var value3))
		{
			_editContainer = value3.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._editorContainer, out var value4))
		{
			_editorContainer = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._preserveElementEditorsWhileFocused, out var value5))
		{
			_preserveElementEditorsWhileFocused = value5.As<bool>();
		}
	}
}
