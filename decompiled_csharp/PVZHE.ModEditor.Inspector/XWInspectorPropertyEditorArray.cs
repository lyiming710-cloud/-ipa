using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Registry;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Array/XWInspectorPropertyEditorArray.cs")]
public class XWInspectorPropertyEditorArray : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName DetectElementType = "DetectElementType";

		public static readonly StringName RefreshProperty = "RefreshProperty";

		public static readonly StringName ClearElementEditors = "ClearElementEditors";

		public static readonly StringName BuildArrayFingerprint = "BuildArrayFingerprint";

		public static readonly StringName FormatFingerprintValue = "FormatFingerprintValue";

		public static readonly StringName FormatObjectFingerprint = "FormatObjectFingerprint";

		public static readonly StringName HasRenderedElementEditors = "HasRenderedElementEditors";

		public static readonly StringName CreatePaginatorControls = "CreatePaginatorControls";

		public static readonly StringName UpdatePaginator = "UpdatePaginator";

		public static readonly StringName ChangePage = "ChangePage";

		public static readonly StringName GetMaxPage = "GetMaxPage";

		public static readonly StringName GetPageStart = "GetPageStart";

		public static readonly StringName GetPageEnd = "GetPageEnd";

		public static readonly StringName InnerValueChange = "InnerValueChange";

		public static readonly StringName AddElement = "AddElement";

		public static readonly StringName RemoveItem = "RemoveItem";

		public static readonly StringName ReadArrayValue = "ReadArrayValue";

		public static readonly StringName DuplicateArray = "DuplicateArray";

		public static readonly StringName CommitArrayChange = "CommitArrayChange";

		public static readonly StringName GetElementEditorValue = "GetElementEditorValue";

		public static readonly StringName HasFocusedElementEditor = "HasFocusedElementEditor";

		public static readonly StringName IsControlTreeFocused = "IsControlTreeFocused";

		public static readonly StringName GetElementEditorType = "GetElementEditorType";

		public static readonly StringName CreateElementEditor = "CreateElementEditor";

		public static readonly StringName CreateResourceElementEditor = "CreateResourceElementEditor";

		public static readonly StringName CreateDefaultElement = "CreateDefaultElement";

		public static readonly StringName ParseArrayHint = "ParseArrayHint";

		public static readonly StringName ArrayButtonPressed = "ArrayButtonPressed";

		public static readonly StringName AddButtonPressed = "AddButtonPressed";

		public new static readonly StringName HideEditor = "HideEditor";

		public new static readonly StringName ShowEditor = "ShowEditor";

		public static readonly StringName SizeUpdate = "SizeUpdate";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _arrayButton = "_arrayButton";

		public static readonly StringName _addButton = "_addButton";

		public static readonly StringName _editContainer = "_editContainer";

		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName _previousPageButton = "_previousPageButton";

		public static readonly StringName _nextPageButton = "_nextPageButton";

		public static readonly StringName _pageLabel = "_pageLabel";

		public static readonly StringName _elementType = "_elementType";

		public static readonly StringName _hintElementType = "_hintElementType";

		public static readonly StringName _hintElementPropertyHint = "_hintElementPropertyHint";

		public static readonly StringName _hintElementString = "_hintElementString";

		public static readonly StringName _array = "_array";

		public static readonly StringName _preserveElementEditorsWhileFocused = "_preserveElementEditorsWhileFocused";

		public static readonly StringName _isExpanded = "_isExpanded";

		public static readonly StringName _pageIndex = "_pageIndex";

		public static readonly StringName _lastRenderedFingerprint = "_lastRenderedFingerprint";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const int PageLength = 20;

	private Button _arrayButton;

	private Button _addButton;

	private PanelContainer _editContainer;

	private VBoxContainer _editorContainer;

	private Button _previousPageButton;

	private Button _nextPageButton;

	private Label _pageLabel;

	private Variant.Type _elementType;

	private Variant.Type _hintElementType;

	private PropertyHint _hintElementPropertyHint;

	private string _hintElementString = "";

	private Godot.Collections.Array _array = new Godot.Collections.Array();

	private bool _preserveElementEditorsWhileFocused;

	private bool _isExpanded;

	private int _pageIndex;

	private string _lastRenderedFingerprint = "";

	public override void _Ready()
	{
		base._Ready();
		_arrayButton = GetNode<Button>("%ArrayButton");
		_addButton = GetNode<Button>("%AddButton");
		_editContainer = GetNode<PanelContainer>("%EditContainer");
		_editorContainer = GetNode<VBoxContainer>("%EditorContainer");
		_arrayButton.Toggled += ArrayButtonPressed;
		_addButton.Pressed += AddButtonPressed;
		CreatePaginatorControls();
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		ParseArrayHint(property?.HintString ?? "");
		base.SetEditProperty(property, field);
	}

	public override void UpdateValue()
	{
		_array = ReadArrayValue();
		DetectElementType();
		SizeUpdate();
		UpdatePaginator();
		string text = BuildArrayFingerprint(_array);
		if (!_isExpanded)
		{
			ClearElementEditors();
		}
		else if ((!(text == _lastRenderedFingerprint) || !HasRenderedElementEditors()) && (!_preserveElementEditorsWhileFocused || (!IsContinuousEditActive && !HasFocusedElementEditor())))
		{
			_preserveElementEditorsWhileFocused = false;
			RefreshProperty();
		}
	}

	public override Variant GetValue()
	{
		return _array;
	}

	private void DetectElementType()
	{
		_elementType = _hintElementType;
		if (_elementType != Variant.Type.Nil || _array.Count == 0)
		{
			return;
		}
		Variant.Type variantType = _array[0].VariantType;
		for (int i = 1; i < _array.Count; i++)
		{
			if (_array[i].VariantType != variantType)
			{
				return;
			}
		}
		_elementType = variantType;
	}

	private void RefreshProperty()
	{
		ClearElementEditors();
		UpdatePaginator();
		for (int i = GetPageStart(); i < GetPageEnd(); i++)
		{
			XWInspectorPropertyEditorArrayItemContainer xWInspectorPropertyEditorArrayItemContainer = XWInspectorPropertyEditorArrayItemContainer.Create();
			_editorContainer.AddChild(xWInspectorPropertyEditorArrayItemContainer, forceReadableName: false, InternalMode.Disabled);
			xWInspectorPropertyEditorArrayItemContainer.Remove += RemoveItem;
			Variant value = _array[i];
			Variant.Type elementEditorType = GetElementEditorType(value);
			XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = CreateElementEditor(elementEditorType, value);
			if (xWInspectorPropertyEditorBase != null)
			{
				GodotObject obj = Property.Object;
				StringName propName = Property.PropName;
				PropertyHint hintElementPropertyHint = _hintElementPropertyHint;
				string hintElementString = _hintElementString;
				XWInspectorProperty property = new XWInspectorProperty(obj, propName, default, null, null, hintElementPropertyHint, hintElementString)
				{
					ReadOnly = Property.ReadOnly,
					Description = Property.Description,
					PropertyUsage = Property.PropertyUsage
				};
				xWInspectorPropertyEditorBase.IsInner = true;
				xWInspectorPropertyEditorBase.ContinuousEditOwner = this;
				xWInspectorPropertyEditorArrayItemContainer.AddEditor(xWInspectorPropertyEditorBase);
				xWInspectorPropertyEditorBase.SetEditProperty(property, new StringName(i.ToString()));
				int itemIndex = i;
				XWInspectorPropertyEditorBase itemEditor = xWInspectorPropertyEditorBase;
				xWInspectorPropertyEditorBase.ValueChanged += (GodotObject godotObject, StringName prop, StringName fld, Variant val) =>
				{
					InnerValueChange(itemIndex, itemEditor, val);
				};
			}
		}
		_lastRenderedFingerprint = BuildArrayFingerprint(_array);
	}

	private void ClearElementEditors()
	{
		if (!GodotObject.IsInstanceValid(_editorContainer))
		{
			return;
		}
		foreach (Node child in _editorContainer.GetChildren())
		{
			child.QueueFree();
		}
		_lastRenderedFingerprint = "";
	}

	private string BuildArrayFingerprint(Godot.Collections.Array array)
	{
		if (array == null)
		{
			return $"{_pageIndex}:null";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_pageIndex).Append('|').Append(array.Count);
		int num = Mathf.Clamp(_pageIndex * 20, 0, array.Count);
		int num2 = Mathf.Clamp(num + 20, 0, array.Count);
		for (int i = num; i < num2; i++)
		{
			Variant value = array[i];
			stringBuilder.Append('|').Append(value.VariantType).Append(':')
				.Append(FormatFingerprintValue(value));
		}
		return stringBuilder.ToString();
	}

	private static string FormatFingerprintValue(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 24;
		if ((ulong)num <= 14uL)
		{
			switch ((int)num)
			{
			case 4:
				return value.AsGodotArray().Count.ToString();
			case 3:
				return value.AsGodotDictionary().Count.ToString();
			case 0:
				return FormatObjectFingerprint(value);
			case 5:
				return value.As<byte[]>().Length.ToString();
			case 6:
				return value.As<int[]>().Length.ToString();
			case 7:
				return value.As<long[]>().Length.ToString();
			case 8:
				return value.As<float[]>().Length.ToString();
			case 9:
				return value.As<double[]>().Length.ToString();
			case 10:
				return value.As<string[]>().Length.ToString();
			case 11:
				return value.As<Vector2[]>().Length.ToString();
			case 12:
				return value.As<Vector3[]>().Length.ToString();
			case 13:
				return value.As<Color[]>().Length.ToString();
			case 14:
				return value.As<Vector4[]>().Length.ToString();
			}
		}
		return value.AsString();
	}

	private static string FormatObjectFingerprint(Variant value)
	{
		GodotObject godotObject = value.AsGodotObject();
		if (!GodotObject.IsInstanceValid(godotObject))
		{
			return "null";
		}
		return godotObject.GetInstanceId().ToString();
	}

	private bool HasRenderedElementEditors()
	{
		if (_lastRenderedFingerprint != "")
		{
			if (_array.Count != 0)
			{
				if (GodotObject.IsInstanceValid(_editorContainer))
				{
					return _editorContainer.GetChildCount() > 0;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private void CreatePaginatorControls()
	{
		if (!GodotObject.IsInstanceValid(_addButton) || GodotObject.IsInstanceValid(_pageLabel))
		{
			return;
		}
		HBoxContainer hBoxContainer = _addButton.GetParent() as HBoxContainer;
		if (GodotObject.IsInstanceValid(hBoxContainer))
		{
			Control node = new Control
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			_previousPageButton = new Button
			{
				Text = "<",
				CustomMinimumSize = new Vector2(28f, 26f),
				TooltipText = "上一页"
			};
			_previousPageButton.Pressed += () =>
			{
				ChangePage(_pageIndex - 1);
			};
			hBoxContainer.AddChild(_previousPageButton, forceReadableName: false, InternalMode.Disabled);
			_pageLabel = new Label
			{
				Text = "1/1",
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				CustomMinimumSize = new Vector2(52f, 26f)
			};
			hBoxContainer.AddChild(_pageLabel, forceReadableName: false, InternalMode.Disabled);
			_nextPageButton = new Button
			{
				Text = ">",
				CustomMinimumSize = new Vector2(28f, 26f),
				TooltipText = "下一页"
			};
			_nextPageButton.Pressed += () =>
			{
				ChangePage(_pageIndex + 1);
			};
			hBoxContainer.AddChild(_nextPageButton, forceReadableName: false, InternalMode.Disabled);
			UpdatePaginator();
		}
	}

	private void UpdatePaginator()
	{
		int maxPage = GetMaxPage(_array.Count);
		_pageIndex = Mathf.Clamp(_pageIndex, 0, maxPage);
		bool visible = _isExpanded && maxPage > 0;
		if (GodotObject.IsInstanceValid(_previousPageButton))
		{
			_previousPageButton.Visible = visible;
			_previousPageButton.Disabled = _pageIndex <= 0;
		}
		if (GodotObject.IsInstanceValid(_nextPageButton))
		{
			_nextPageButton.Visible = visible;
			_nextPageButton.Disabled = _pageIndex >= maxPage;
		}
		if (GodotObject.IsInstanceValid(_pageLabel))
		{
			_pageLabel.Visible = visible;
			_pageLabel.Text = $"{_pageIndex + 1}/{maxPage + 1}";
		}
		if (GodotObject.IsInstanceValid(_addButton))
		{
			_addButton.Visible = !_isExpanded || _pageIndex == maxPage;
		}
	}

	private void ChangePage(int page)
	{
		_pageIndex = Mathf.Clamp(page, 0, GetMaxPage(_array.Count));
		if (_isExpanded)
		{
			RefreshProperty();
		}
		else
		{
			UpdatePaginator();
		}
	}

	private int GetMaxPage(int count)
	{
		if (count > 0)
		{
			return (count - 1) / 20;
		}
		return 0;
	}

	private int GetPageStart()
	{
		return Mathf.Clamp(_pageIndex * 20, 0, _array.Count);
	}

	private int GetPageEnd()
	{
		return Mathf.Clamp(GetPageStart() + 20, 0, _array.Count);
	}

	private void InnerValueChange(int index, XWInspectorPropertyEditorBase editor, Variant fallbackValue)
	{
		if (index >= 0 && index < _array.Count)
		{
			Godot.Collections.Array array = DuplicateArray(_array);
			array[index] = GetElementEditorValue(editor, fallbackValue, _array[index]);
			CommitArrayChange(array, preserveElementEditors: true);
		}
	}

	private void AddElement()
	{
		Godot.Collections.Array array = DuplicateArray(_array);
		array.Add(CreateDefaultElement());
		_pageIndex = GetMaxPage(array.Count);
		CommitArrayChange(array);
	}

	private void RemoveItem(XWInspectorPropertyEditorArrayItemContainer itemContainer)
	{
		int num = _editorContainer.GetChildren().IndexOf(itemContainer);
		if (num != -1)
		{
			int num2 = GetPageStart() + num;
			if (num2 >= 0 && num2 < _array.Count)
			{
				Godot.Collections.Array array = DuplicateArray(_array);
				array.RemoveAt(num2);
				itemContainer.QueueFree();
				CommitArrayChange(array);
			}
		}
	}

	private Godot.Collections.Array ReadArrayValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Array)
		{
			return DuplicateArray(propertyValue.As<Godot.Collections.Array>());
		}
		return new Godot.Collections.Array();
	}

	private static Godot.Collections.Array DuplicateArray(Godot.Collections.Array source)
	{
		if (source != null)
		{
			return source.Duplicate();
		}
		return new Godot.Collections.Array();
	}

	private void CommitArrayChange(Godot.Collections.Array nextArray, bool preserveElementEditors = false)
	{
		_array = DuplicateArray(nextArray);
		DetectElementType();
		_preserveElementEditorsWhileFocused = preserveElementEditors;
		ValueChange(Variant.From(in nextArray));
		SizeUpdate();
		UpdatePaginator();
		if (!_preserveElementEditorsWhileFocused || (!IsContinuousEditActive && !HasFocusedElementEditor()))
		{
			_preserveElementEditorsWhileFocused = false;
			RefreshProperty();
		}
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

	private Variant.Type GetElementEditorType(Variant value)
	{
		if (_hintElementType != Variant.Type.Nil)
		{
			return _hintElementType;
		}
		if (_elementType != Variant.Type.Nil)
		{
			return _elementType;
		}
		if (value.VariantType != Variant.Type.Nil)
		{
			return value.VariantType;
		}
		return Variant.Type.Nil;
	}

	private XWInspectorPropertyEditorBase CreateElementEditor(Variant.Type type, Variant value)
	{
		if (_hintElementPropertyHint != PropertyHint.None)
		{
			XWInspectorPropertyEditorBase hintEditor = XWTypeRegistry.Instance.GetHintEditor((int)_hintElementPropertyHint);
			if (GodotObject.IsInstanceValid(hintEditor))
			{
				return hintEditor;
			}
		}
		if (type == Variant.Type.Object || _hintElementPropertyHint == PropertyHint.ResourceType)
		{
			XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = CreateResourceElementEditor(value);
			if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
			{
				return xWInspectorPropertyEditorBase;
			}
		}
		if (_hintElementPropertyHint == PropertyHint.ResourceType)
		{
			return XWTypeRegistry.Instance.GetResourcePropertyEditor();
		}
		return XWTypeRegistry.Instance.GetTypeEditor(type);
	}

	private XWInspectorPropertyEditorBase CreateResourceElementEditor(Variant value)
	{
		if (value.VariantType != Variant.Type.Object)
		{
			return null;
		}
		Resource resource = value.As<GodotObject>() as Resource;
		if (!GodotObject.IsInstanceValid(resource))
		{
			if (_hintElementPropertyHint != PropertyHint.ResourceType)
			{
				return null;
			}
			return XWTypeRegistry.Instance.GetResourcePropertyEditor();
		}
		PackedScene editor = XWInspectorPropertyRegistry.GetEditor(resource);
		if (editor != null)
		{
			return editor.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
		}
		return XWTypeRegistry.Instance.GetResourcePropertyEditor();
	}

	private Variant CreateDefaultElement()
	{
		Variant.Type type = ((_hintElementType != Variant.Type.Nil) ? _hintElementType : _elementType);
		switch (type)
		{
		case Variant.Type.Nil:
			return "";
		case Variant.Type.Array:
			return new Godot.Collections.Array();
		case Variant.Type.Dictionary:
			return new Dictionary();
		case Variant.Type.Object:
			return Variant.From<GodotObject>((GodotObject)null);
		default:
			if (!XWTypeRegistry.Instance.HasType(type))
			{
				return default;
			}
			return XWTypeRegistry.Instance.GetTypeDefaultValue(type);
		}
	}

	private void ParseArrayHint(string hintString)
	{
		_hintElementType = Variant.Type.Nil;
		_hintElementPropertyHint = PropertyHint.None;
		_hintElementString = "";
		if (string.IsNullOrWhiteSpace(hintString))
		{
			return;
		}
		string text = hintString;
		int num = hintString.IndexOf(':');
		if (num >= 0)
		{
			text = hintString.Substring(0, num);
			string text2 = hintString;
			int num2 = num + 1;
			_hintElementString = text2.Substring(num2, text2.Length - num2);
		}
		int num3 = text.IndexOf('/');
		if (num3 >= 0)
		{
			string text2 = text;
			int num2 = num3 + 1;
			if (int.TryParse(text2.Substring(num2, text2.Length - num2), out var result))
			{
				_hintElementPropertyHint = (PropertyHint)result;
			}
			text = text.Substring(0, num3);
		}
		if (TryParseVariantType(text, out var type))
		{
			_hintElementType = type;
			return;
		}
		_hintElementType = Variant.Type.Object;
		_hintElementPropertyHint = PropertyHint.ResourceType;
		_hintElementString = hintString;
	}

	private static bool TryParseVariantType(string text, out Variant.Type type)
	{
		type = Variant.Type.Nil;
		if (string.IsNullOrWhiteSpace(text))
		{
			return false;
		}
		Variant.Type[] values;
		if (long.TryParse(text, out var result))
		{
			values = Enum.GetValues<Variant.Type>();
			foreach (Variant.Type type2 in values)
			{
				if (Convert.ToInt64(type2) == result)
				{
					type = type2;
					return true;
				}
			}
		}
		values = Enum.GetValues<Variant.Type>();
		for (int i = 0; i < values.Length; i++)
		{
			Variant.Type type3 = values[i];
			if (string.Equals(type3.ToString(), text, StringComparison.OrdinalIgnoreCase))
			{
				type = type3;
				return true;
			}
		}
		foreach (XWTypeData item in XWTypeRegistry.Instance.GetAllType())
		{
			if (string.Equals(item.Name, text, StringComparison.OrdinalIgnoreCase))
			{
				type = item.Type;
				return true;
			}
		}
		return false;
	}

	private void ArrayButtonPressed(bool toggledOn)
	{
		_isExpanded = toggledOn;
		_editContainer.Visible = toggledOn;
		UpdatePaginator();
		if (toggledOn)
		{
			RefreshProperty();
		}
		else
		{
			ClearElementEditors();
		}
	}

	private void AddButtonPressed()
	{
		AddElement();
	}

	public override void HideEditor()
	{
		base.HideEditor();
		_isExpanded = false;
		_arrayButton.ButtonPressed = false;
		_editContainer.Visible = false;
		ClearElementEditors();
		UpdatePaginator();
	}

	public override void ShowEditor()
	{
		base.ShowEditor();
		_arrayButton.ButtonPressed = _isExpanded;
		_editContainer.Visible = _isExpanded;
		if (!_isExpanded)
		{
			ClearElementEditors();
		}
		UpdatePaginator();
	}

	private void SizeUpdate()
	{
		string value = ((_hintElementType != Variant.Type.Nil) ? ("[" + XWTypeRegistry.Instance.GetTypeName(_hintElementType) + "]") : "");
		_arrayButton.Text = $"Array{value}( 大小 {_array.Count} )";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(36)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetectElementType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearElementEditors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildArrayFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "array", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatFingerprintValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatObjectFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.HasRenderedElementEditors, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePaginatorControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePaginator, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChangePage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "page", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMaxPage, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPageStart, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPageEnd, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InnerValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "fallbackValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "itemContainer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadArrayValue, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitArrayChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nextArray", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preserveElementEditors", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			}, null),
			new MethodInfo(MethodName.GetElementEditorType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateElementEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateResourceElementEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDefaultElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ParseArrayHint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArrayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SizeUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DetectElementType && args.Count == 0)
		{
			DetectElementType();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProperty && args.Count == 0)
		{
			RefreshProperty();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearElementEditors && args.Count == 0)
		{
			ClearElementEditors();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildArrayFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildArrayFingerprint(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatFingerprintValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatFingerprintValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObjectFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObjectFingerprint(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.HasRenderedElementEditors && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRenderedElementEditors());
			return true;
		}
		if (method == MethodName.CreatePaginatorControls && args.Count == 0)
		{
			CreatePaginatorControls();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePaginator && args.Count == 0)
		{
			UpdatePaginator();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangePage && args.Count == 1)
		{
			ChangePage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMaxPage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetMaxPage(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPageStart && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPageStart());
			return true;
		}
		if (method == MethodName.GetPageEnd && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPageEnd());
			return true;
		}
		if (method == MethodName.InnerValueChange && args.Count == 3)
		{
			InnerValueChange(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
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
		if (method == MethodName.ReadArrayValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(ReadArrayValue());
			return true;
		}
		if (method == MethodName.DuplicateArray && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(DuplicateArray(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.CommitArrayChange && args.Count == 2)
		{
			CommitArrayChange(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName.GetElementEditorType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant.Type>(GetElementEditorType(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateElementEditor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(CreateElementEditor(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateResourceElementEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(CreateResourceElementEditor(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateDefaultElement && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(CreateDefaultElement());
			return true;
		}
		if (method == MethodName.ParseArrayHint && args.Count == 1)
		{
			ParseArrayHint(VariantUtils.ConvertTo<string>(in args[0]));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FormatFingerprintValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatFingerprintValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObjectFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObjectFingerprint(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateArray && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(DuplicateArray(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
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
		if (method == MethodName.DetectElementType)
		{
			return true;
		}
		if (method == MethodName.RefreshProperty)
		{
			return true;
		}
		if (method == MethodName.ClearElementEditors)
		{
			return true;
		}
		if (method == MethodName.BuildArrayFingerprint)
		{
			return true;
		}
		if (method == MethodName.FormatFingerprintValue)
		{
			return true;
		}
		if (method == MethodName.FormatObjectFingerprint)
		{
			return true;
		}
		if (method == MethodName.HasRenderedElementEditors)
		{
			return true;
		}
		if (method == MethodName.CreatePaginatorControls)
		{
			return true;
		}
		if (method == MethodName.UpdatePaginator)
		{
			return true;
		}
		if (method == MethodName.ChangePage)
		{
			return true;
		}
		if (method == MethodName.GetMaxPage)
		{
			return true;
		}
		if (method == MethodName.GetPageStart)
		{
			return true;
		}
		if (method == MethodName.GetPageEnd)
		{
			return true;
		}
		if (method == MethodName.InnerValueChange)
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
		if (method == MethodName.ReadArrayValue)
		{
			return true;
		}
		if (method == MethodName.DuplicateArray)
		{
			return true;
		}
		if (method == MethodName.CommitArrayChange)
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
		if (method == MethodName.GetElementEditorType)
		{
			return true;
		}
		if (method == MethodName.CreateElementEditor)
		{
			return true;
		}
		if (method == MethodName.CreateResourceElementEditor)
		{
			return true;
		}
		if (method == MethodName.CreateDefaultElement)
		{
			return true;
		}
		if (method == MethodName.ParseArrayHint)
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
		if (name == PropertyName._previousPageButton)
		{
			_previousPageButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextPageButton)
		{
			_nextPageButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pageLabel)
		{
			_pageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._elementType)
		{
			_elementType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._hintElementType)
		{
			_hintElementType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._hintElementPropertyHint)
		{
			_hintElementPropertyHint = VariantUtils.ConvertTo<PropertyHint>(in value);
			return true;
		}
		if (name == PropertyName._hintElementString)
		{
			_hintElementString = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._array)
		{
			_array = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName._preserveElementEditorsWhileFocused)
		{
			_preserveElementEditorsWhileFocused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isExpanded)
		{
			_isExpanded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pageIndex)
		{
			_pageIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastRenderedFingerprint)
		{
			_lastRenderedFingerprint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
		if (name == PropertyName._previousPageButton)
		{
			value = VariantUtils.CreateFrom(in _previousPageButton);
			return true;
		}
		if (name == PropertyName._nextPageButton)
		{
			value = VariantUtils.CreateFrom(in _nextPageButton);
			return true;
		}
		if (name == PropertyName._pageLabel)
		{
			value = VariantUtils.CreateFrom(in _pageLabel);
			return true;
		}
		if (name == PropertyName._elementType)
		{
			value = VariantUtils.CreateFrom(in _elementType);
			return true;
		}
		if (name == PropertyName._hintElementType)
		{
			value = VariantUtils.CreateFrom(in _hintElementType);
			return true;
		}
		if (name == PropertyName._hintElementPropertyHint)
		{
			value = VariantUtils.CreateFrom(in _hintElementPropertyHint);
			return true;
		}
		if (name == PropertyName._hintElementString)
		{
			value = VariantUtils.CreateFrom(in _hintElementString);
			return true;
		}
		if (name == PropertyName._array)
		{
			value = VariantUtils.CreateFrom(in _array);
			return true;
		}
		if (name == PropertyName._preserveElementEditorsWhileFocused)
		{
			value = VariantUtils.CreateFrom(in _preserveElementEditorsWhileFocused);
			return true;
		}
		if (name == PropertyName._isExpanded)
		{
			value = VariantUtils.CreateFrom(in _isExpanded);
			return true;
		}
		if (name == PropertyName._pageIndex)
		{
			value = VariantUtils.CreateFrom(in _pageIndex);
			return true;
		}
		if (name == PropertyName._lastRenderedFingerprint)
		{
			value = VariantUtils.CreateFrom(in _lastRenderedFingerprint);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._elementType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hintElementType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hintElementPropertyHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._hintElementString, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._array, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveElementEditorsWhileFocused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isExpanded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pageIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastRenderedFingerprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
		info.AddProperty(PropertyName._previousPageButton, Variant.From(in _previousPageButton));
		info.AddProperty(PropertyName._nextPageButton, Variant.From(in _nextPageButton));
		info.AddProperty(PropertyName._pageLabel, Variant.From(in _pageLabel));
		info.AddProperty(PropertyName._elementType, Variant.From(in _elementType));
		info.AddProperty(PropertyName._hintElementType, Variant.From(in _hintElementType));
		info.AddProperty(PropertyName._hintElementPropertyHint, Variant.From(in _hintElementPropertyHint));
		info.AddProperty(PropertyName._hintElementString, Variant.From(in _hintElementString));
		info.AddProperty(PropertyName._array, Variant.From(in _array));
		info.AddProperty(PropertyName._preserveElementEditorsWhileFocused, Variant.From(in _preserveElementEditorsWhileFocused));
		info.AddProperty(PropertyName._isExpanded, Variant.From(in _isExpanded));
		info.AddProperty(PropertyName._pageIndex, Variant.From(in _pageIndex));
		info.AddProperty(PropertyName._lastRenderedFingerprint, Variant.From(in _lastRenderedFingerprint));
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
		if (info.TryGetProperty(PropertyName._previousPageButton, out var value5))
		{
			_previousPageButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextPageButton, out var value6))
		{
			_nextPageButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pageLabel, out var value7))
		{
			_pageLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._elementType, out var value8))
		{
			_elementType = value8.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._hintElementType, out var value9))
		{
			_hintElementType = value9.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._hintElementPropertyHint, out var value10))
		{
			_hintElementPropertyHint = value10.As<PropertyHint>();
		}
		if (info.TryGetProperty(PropertyName._hintElementString, out var value11))
		{
			_hintElementString = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName._array, out var value12))
		{
			_array = value12.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName._preserveElementEditorsWhileFocused, out var value13))
		{
			_preserveElementEditorsWhileFocused = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isExpanded, out var value14))
		{
			_isExpanded = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pageIndex, out var value15))
		{
			_pageIndex = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastRenderedFingerprint, out var value16))
		{
			_lastRenderedFingerprint = value16.As<string>();
		}
	}
}
