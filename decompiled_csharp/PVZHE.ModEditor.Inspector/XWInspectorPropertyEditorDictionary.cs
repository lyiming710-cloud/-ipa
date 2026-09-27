using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.Registry;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Dictionary/XWInspectorPropertyEditorDictionary.cs")]
public class XWInspectorPropertyEditorDictionary : XWInspectorPropertyEditorBase
{
	public class XWDictionaryEntryProxy : RefCounted
	{
		public new class MethodName : RefCounted.MethodName
		{
			public new static readonly StringName _Get = "_Get";

			public new static readonly StringName _Set = "_Set";

			public new static readonly StringName _GetPropertyList = "_GetPropertyList";
		}

		public new class PropertyName : RefCounted.PropertyName
		{
			public static readonly StringName Key = "Key";

			public static readonly StringName Value = "Value";
		}

		public new class SignalName : RefCounted.SignalName
		{
		}

		public Variant Key { get; set; }

		public Variant Value { get; set; }

		public override Variant _Get(StringName property)
		{
			if (property == (StringName)"key")
			{
				return Key;
			}
			if (property == (StringName)"value")
			{
				return Value;
			}
			return default;
		}

		public override bool _Set(StringName property, Variant value)
		{
			if (property == (StringName)"key")
			{
				Key = value;
				return true;
			}
			if (property == (StringName)"value")
			{
				Value = value;
				return true;
			}
			return false;
		}

		public override Array<Dictionary> _GetPropertyList()
		{
			return new Array<Dictionary>
			{
				new Dictionary
				{
					{ "name", "key" },
					{ "type", 0 },
					{ "usage", 4098 }
				},
				new Dictionary
				{
					{ "name", "value" },
					{ "type", 0 },
					{ "usage", 4098 }
				}
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<MethodInfo> GetGodotMethodList()
		{
			return new List<MethodInfo>(3)
			{
				new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
				}, null),
				new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
				{
					new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
					new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
				}, null),
				new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
		{
			if (method == MethodName._Get && args.Count == 1)
			{
				ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
				return true;
			}
			if (method == MethodName._Set && args.Count == 2)
			{
				ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
				return true;
			}
			if (method == MethodName._GetPropertyList && args.Count == 0)
			{
				Array<Dictionary> array = _GetPropertyList();
				ret = VariantUtils.CreateFromArray(array);
				return true;
			}
			return base.InvokeGodotClassMethod(in method, args, out ret);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool HasGodotClassMethod(in godot_string_name method)
		{
			if (method == MethodName._Get)
			{
				return true;
			}
			if (method == MethodName._Set)
			{
				return true;
			}
			if (method == MethodName._GetPropertyList)
			{
				return true;
			}
			return base.HasGodotClassMethod(in method);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
		{
			if (name == PropertyName.Key)
			{
				Key = VariantUtils.ConvertTo<Variant>(in value);
				return true;
			}
			if (name == PropertyName.Value)
			{
				Value = VariantUtils.ConvertTo<Variant>(in value);
				return true;
			}
			return base.SetGodotClassPropertyValue(in name, in value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
		{
			Variant from;
			if (name == PropertyName.Key)
			{
				from = Key;
				value = VariantUtils.CreateFrom(in from);
				return true;
			}
			if (name == PropertyName.Value)
			{
				from = Value;
				value = VariantUtils.CreateFrom(in from);
				return true;
			}
			return base.GetGodotClassPropertyValue(in name, out value);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal static List<PropertyInfo> GetGodotPropertyList()
		{
			return new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, PropertyName.Key, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
				new PropertyInfo(Variant.Type.Nil, PropertyName.Value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
			};
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void SaveGodotObjectData(GodotSerializationInfo info)
		{
			base.SaveGodotObjectData(info);
			info.AddProperty(PropertyName.Key, Variant.From<Variant>(Key));
			info.AddProperty(PropertyName.Value, Variant.From<Variant>(Value));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void RestoreGodotObjectData(GodotSerializationInfo info)
		{
			base.RestoreGodotObjectData(info);
			if (info.TryGetProperty(PropertyName.Key, out var value))
			{
				Key = value.As<Variant>();
			}
			if (info.TryGetProperty(PropertyName.Value, out var value2))
			{
				Value = value2.As<Variant>();
			}
		}
	}

	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName RefreshProperty = "RefreshProperty";

		public static readonly StringName ClearEntryEditors = "ClearEntryEditors";

		public static readonly StringName ClearNewEntryEditors = "ClearNewEntryEditors";

		public static readonly StringName CreatePaginatorControls = "CreatePaginatorControls";

		public static readonly StringName UpdatePaginator = "UpdatePaginator";

		public static readonly StringName ChangePage = "ChangePage";

		public static readonly StringName GetMaxPage = "GetMaxPage";

		public static readonly StringName GetPageStart = "GetPageStart";

		public static readonly StringName GetPageEnd = "GetPageEnd";

		public static readonly StringName BuildDictionaryFingerprint = "BuildDictionaryFingerprint";

		public static readonly StringName FormatFingerprintValue = "FormatFingerprintValue";

		public static readonly StringName FormatObjectFingerprint = "FormatObjectFingerprint";

		public static readonly StringName HasRenderedEntryEditors = "HasRenderedEntryEditors";

		public static readonly StringName HasFocusedEntryEditor = "HasFocusedEntryEditor";

		public static readonly StringName IsControlTreeFocused = "IsControlTreeFocused";

		public static readonly StringName GetKeyAtIndex = "GetKeyAtIndex";

		public static readonly StringName RefreshNewEntryEditors = "RefreshNewEntryEditors";

		public static readonly StringName OnEntryValueChanged = "OnEntryValueChanged";

		public static readonly StringName OnAddButtonPressed = "OnAddButtonPressed";

		public static readonly StringName OnRemoveItem = "OnRemoveItem";

		public static readonly StringName ReadDictionaryValue = "ReadDictionaryValue";

		public static readonly StringName DuplicateDictionary = "DuplicateDictionary";

		public static readonly StringName CommitDictionaryChange = "CommitDictionaryChange";

		public static readonly StringName RemoveDictionaryKey = "RemoveDictionaryKey";

		public static readonly StringName SetDictionaryValue = "SetDictionaryValue";

		public static readonly StringName GetKeyEditorType = "GetKeyEditorType";

		public static readonly StringName GetValueEditorType = "GetValueEditorType";

		public static readonly StringName CreateEditorForType = "CreateEditorForType";

		public static readonly StringName ShouldUseReadOnlyVariantSummary = "ShouldUseReadOnlyVariantSummary";

		public static readonly StringName IsComplexVariantValue = "IsComplexVariantValue";

		public static readonly StringName CreateValueSummaryControl = "CreateValueSummaryControl";

		public static readonly StringName FormatVariantSummary = "FormatVariantSummary";

		public static readonly StringName FormatObjectSummary = "FormatObjectSummary";

		public static readonly StringName CreateResourceEntryEditor = "CreateResourceEntryEditor";

		public static readonly StringName CreateDefaultValue = "CreateDefaultValue";

		public static readonly StringName ParseDictionaryHint = "ParseDictionaryHint";

		public static readonly StringName OnKeyTypeButtonPressed = "OnKeyTypeButtonPressed";

		public static readonly StringName OnValueTypeButtonPressed = "OnValueTypeButtonPressed";

		public static readonly StringName OnKeyTypeSelected = "OnKeyTypeSelected";

		public static readonly StringName OnValueTypeSelected = "OnValueTypeSelected";

		public static readonly StringName UpdateTypeButtons = "UpdateTypeButtons";

		public static readonly StringName UpdateTypeButton = "UpdateTypeButton";

		public static readonly StringName OnDictButtonToggled = "OnDictButtonToggled";

		public new static readonly StringName HideEditor = "HideEditor";

		public new static readonly StringName ShowEditor = "ShowEditor";

		public static readonly StringName SizeUpdate = "SizeUpdate";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _dictButton = "_dictButton";

		public static readonly StringName _addButton = "_addButton";

		public static readonly StringName _editContainer = "_editContainer";

		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName _keyTypeButton = "_keyTypeButton";

		public static readonly StringName _valueTypeButton = "_valueTypeButton";

		public static readonly StringName _newKeyContainer = "_newKeyContainer";

		public static readonly StringName _newValueContainer = "_newValueContainer";

		public static readonly StringName _previousPageButton = "_previousPageButton";

		public static readonly StringName _nextPageButton = "_nextPageButton";

		public static readonly StringName _pageLabel = "_pageLabel";

		public static readonly StringName _dictionary = "_dictionary";

		public static readonly StringName _keyType = "_keyType";

		public static readonly StringName _valueType = "_valueType";

		public static readonly StringName _keyPropertyHint = "_keyPropertyHint";

		public static readonly StringName _valuePropertyHint = "_valuePropertyHint";

		public static readonly StringName _keyHintString = "_keyHintString";

		public static readonly StringName _valueHintString = "_valueHintString";

		public static readonly StringName _typeSelectorWindow = "_typeSelectorWindow";

		public static readonly StringName _newEntryProxy = "_newEntryProxy";

		public static readonly StringName _newKeyEditor = "_newKeyEditor";

		public static readonly StringName _newValueEditor = "_newValueEditor";

		public static readonly StringName _preserveEntryEditorsWhileFocused = "_preserveEntryEditorsWhileFocused";

		public static readonly StringName _isExpanded = "_isExpanded";

		public static readonly StringName _pageIndex = "_pageIndex";

		public static readonly StringName _lastRenderedFingerprint = "_lastRenderedFingerprint";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const int PageLength = 20;

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Dictionary/XWInspectorPropertyEditorDictionary.tscn";

	private Button _dictButton;

	private Button _addButton;

	private PanelContainer _editContainer;

	private VBoxContainer _editorContainer;

	private Button _keyTypeButton;

	private Button _valueTypeButton;

	private PanelContainer _newKeyContainer;

	private PanelContainer _newValueContainer;

	private Button _previousPageButton;

	private Button _nextPageButton;

	private Label _pageLabel;

	private Dictionary _dictionary = new Dictionary();

	private Variant.Type _keyType = Variant.Type.String;

	private Variant.Type _valueType;

	private PropertyHint _keyPropertyHint;

	private PropertyHint _valuePropertyHint;

	private string _keyHintString = "";

	private string _valueHintString = "";

	private XWWindowTypeSelector _typeSelectorWindow;

	private XWDictionaryEntryProxy _newEntryProxy;

	private XWInspectorPropertyEditorBase _newKeyEditor;

	private XWInspectorPropertyEditorBase _newValueEditor;

	private bool _preserveEntryEditorsWhileFocused;

	private bool _isExpanded;

	private int _pageIndex;

	private string _lastRenderedFingerprint = "";

	public static XWInspectorPropertyEditorDictionary Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Dictionary/XWInspectorPropertyEditorDictionary.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorDictionary>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_dictButton = GetNode<Button>("%DictButton");
		_addButton = GetNode<Button>("%AddButton");
		_editContainer = GetNode<PanelContainer>("%EditContainer");
		_editorContainer = GetNode<VBoxContainer>("%EditorContainer");
		_keyTypeButton = GetNode<Button>("%KeyTypeButton");
		_valueTypeButton = GetNode<Button>("%ValueTypeButton");
		_newKeyContainer = GetNode<PanelContainer>("%NewKeyContainer");
		_newValueContainer = GetNode<PanelContainer>("%NewValueContainer");
		_dictButton.Toggled += OnDictButtonToggled;
		_addButton.Pressed += OnAddButtonPressed;
		_keyTypeButton.Pressed += OnKeyTypeButtonPressed;
		_valueTypeButton.Pressed += OnValueTypeButtonPressed;
		CreatePaginatorControls();
		_newEntryProxy = new XWDictionaryEntryProxy
		{
			Key = "",
			Value = ""
		};
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		ParseDictionaryHint(property?.HintString ?? "");
		base.SetEditProperty(property, field);
	}

	public override void UpdateValue()
	{
		_dictionary = ReadDictionaryValue();
		SizeUpdate();
		UpdatePaginator();
		string text = BuildDictionaryFingerprint(_dictionary);
		if (!_isExpanded)
		{
			ClearEntryEditors();
			ClearNewEntryEditors();
		}
		else if ((!_isExpanded || !(text == _lastRenderedFingerprint) || !HasRenderedEntryEditors()) && (!_preserveEntryEditorsWhileFocused || (!IsContinuousEditActive && !HasFocusedEntryEditor())))
		{
			_preserveEntryEditorsWhileFocused = false;
			RefreshProperty();
		}
	}

	public override Variant GetValue()
	{
		return _dictionary;
	}

	private void RefreshProperty()
	{
		ClearEntryEditors();
		UpdatePaginator();
		UpdateTypeButtons();
		RefreshNewEntryEditors();
		for (int i = GetPageStart(); i < GetPageEnd(); i++)
		{
			Variant keyAtIndex = GetKeyAtIndex(i);
			Variant value = _dictionary[keyAtIndex];
			XWInspectorPropertyEditorDictionaryItemContainer container = XWInspectorPropertyEditorDictionaryItemContainer.Create();
			_editorContainer.AddChild(container, forceReadableName: false, InternalMode.Disabled);
			container.Remove += OnRemoveItem;
			XWDictionaryEntryProxy xWDictionaryEntryProxy = new XWDictionaryEntryProxy
			{
				Key = keyAtIndex,
				Value = value
			};
			container.EntryProxy = xWDictionaryEntryProxy;
			XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = CreateEditorForType(GetKeyEditorType(keyAtIndex), _keyPropertyHint, _keyHintString, keyAtIndex);
			if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
			{
				StringName propName = "key";
				PropertyHint keyPropertyHint = _keyPropertyHint;
				string keyHintString = _keyHintString;
				XWInspectorProperty property = new XWInspectorProperty(xWDictionaryEntryProxy, propName, default, null, null, keyPropertyHint, keyHintString);
				xWInspectorPropertyEditorBase.IsInner = true;
				xWInspectorPropertyEditorBase.ContinuousEditOwner = this;
				xWInspectorPropertyEditorBase.PropertyNameLock = true;
				xWInspectorPropertyEditorBase.ValueChanged += (GodotObject obj, StringName prop, StringName fld, Variant val) =>
				{
					OnEntryValueChanged(container, prop, val);
				};
				container.GetKeyEditorContainer().AddChild(xWInspectorPropertyEditorBase, forceReadableName: false, InternalMode.Disabled);
				xWInspectorPropertyEditorBase.SetEditProperty(property);
				xWInspectorPropertyEditorBase.PropertyNameLabel.Text = "Key";
			}
			if (ShouldUseReadOnlyVariantSummary(value))
			{
				container.GetValueEditorContainer().AddChild(CreateValueSummaryControl(value), forceReadableName: false, InternalMode.Disabled);
				continue;
			}
			XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase2 = CreateEditorForType(GetValueEditorType(value), _valuePropertyHint, _valueHintString, value);
			if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase2))
			{
				StringName propName2 = "value";
				PropertyHint keyPropertyHint = _valuePropertyHint;
				string keyHintString = _valueHintString;
				XWInspectorProperty property2 = new XWInspectorProperty(xWDictionaryEntryProxy, propName2, default, null, null, keyPropertyHint, keyHintString);
				xWInspectorPropertyEditorBase2.IsInner = true;
				xWInspectorPropertyEditorBase2.ContinuousEditOwner = this;
				xWInspectorPropertyEditorBase2.PropertyNameLock = true;
				xWInspectorPropertyEditorBase2.ValueChanged += (GodotObject obj, StringName prop, StringName fld, Variant val) =>
				{
					OnEntryValueChanged(container, prop, val);
				};
				container.GetValueEditorContainer().AddChild(xWInspectorPropertyEditorBase2, forceReadableName: false, InternalMode.Disabled);
				xWInspectorPropertyEditorBase2.SetEditProperty(property2);
				xWInspectorPropertyEditorBase2.PropertyNameLabel.Text = "Value";
			}
		}
		_lastRenderedFingerprint = BuildDictionaryFingerprint(_dictionary);
	}

	private void ClearEntryEditors()
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

	private void ClearNewEntryEditors()
	{
		if (GodotObject.IsInstanceValid(_newKeyEditor))
		{
			_newKeyEditor.QueueFree();
			_newKeyEditor = null;
		}
		if (GodotObject.IsInstanceValid(_newValueEditor))
		{
			_newValueEditor.QueueFree();
			_newValueEditor = null;
		}
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
		int maxPage = GetMaxPage(_dictionary.Count);
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
		_pageIndex = Mathf.Clamp(page, 0, GetMaxPage(_dictionary.Count));
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
		return Mathf.Clamp(_pageIndex * 20, 0, _dictionary.Count);
	}

	private int GetPageEnd()
	{
		return Mathf.Clamp(GetPageStart() + 20, 0, _dictionary.Count);
	}

	private string BuildDictionaryFingerprint(Dictionary dictionary)
	{
		if (dictionary == null)
		{
			return $"{_pageIndex}:null";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_pageIndex).Append('|').Append(dictionary.Count);
		int num = Mathf.Clamp(_pageIndex * 20, 0, dictionary.Count);
		int num2 = Mathf.Clamp(num + 20, 0, dictionary.Count);
		int num3 = 0;
		foreach (Variant key in dictionary.Keys)
		{
			if (num3 >= num && num3 < num2)
			{
				Variant value = dictionary[key];
				stringBuilder.Append('|').Append(key.VariantType).Append(':')
					.Append(FormatFingerprintValue(key))
					.Append('=')
					.Append(value.VariantType)
					.Append(':')
					.Append(FormatFingerprintValue(value));
			}
			num3++;
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

	private bool HasRenderedEntryEditors()
	{
		if (_lastRenderedFingerprint != "")
		{
			if (_dictionary.Count != 0)
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

	private bool HasFocusedEntryEditor()
	{
		if ((!GodotObject.IsInstanceValid(_editorContainer) || !IsControlTreeFocused(_editorContainer)) && (!GodotObject.IsInstanceValid(_newKeyContainer) || !IsControlTreeFocused(_newKeyContainer)))
		{
			if (GodotObject.IsInstanceValid(_newValueContainer))
			{
				return IsControlTreeFocused(_newValueContainer);
			}
			return false;
		}
		return true;
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

	private Variant GetKeyAtIndex(int index)
	{
		int num = 0;
		foreach (Variant key in _dictionary.Keys)
		{
			if (num == index)
			{
				return key;
			}
			num++;
		}
		return default;
	}

	private void RefreshNewEntryEditors()
	{
		ClearNewEntryEditors();
		_newEntryProxy.Key = CreateDefaultValue(_keyType);
		_newEntryProxy.Value = CreateDefaultValue(_valueType);
		XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = CreateEditorForType(_keyType, _keyPropertyHint, _keyHintString, _newEntryProxy.Key);
		if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
		{
			XWDictionaryEntryProxy newEntryProxy = _newEntryProxy;
			StringName propName = "key";
			PropertyHint keyPropertyHint = _keyPropertyHint;
			string keyHintString = _keyHintString;
			XWInspectorProperty property = new XWInspectorProperty(newEntryProxy, propName, default, null, null, keyPropertyHint, keyHintString);
			xWInspectorPropertyEditorBase.IsInner = true;
			xWInspectorPropertyEditorBase.PropertyNameLock = true;
			xWInspectorPropertyEditorBase.ValueChanged += (GodotObject obj, StringName prop, StringName fld, Variant val) =>
			{
				_newEntryProxy.Key = val;
			};
			_newKeyContainer.AddChild(xWInspectorPropertyEditorBase, forceReadableName: false, InternalMode.Disabled);
			xWInspectorPropertyEditorBase.SetEditProperty(property);
			xWInspectorPropertyEditorBase.PropertyNameLabel.Text = "Key";
			_newKeyEditor = xWInspectorPropertyEditorBase;
		}
		XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase2 = CreateEditorForType(_valueType, _valuePropertyHint, _valueHintString, _newEntryProxy.Value);
		if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase2))
		{
			XWDictionaryEntryProxy newEntryProxy2 = _newEntryProxy;
			StringName propName2 = "value";
			PropertyHint keyPropertyHint = _valuePropertyHint;
			string keyHintString = _valueHintString;
			XWInspectorProperty property2 = new XWInspectorProperty(newEntryProxy2, propName2, default, null, null, keyPropertyHint, keyHintString);
			xWInspectorPropertyEditorBase2.IsInner = true;
			xWInspectorPropertyEditorBase2.PropertyNameLock = true;
			xWInspectorPropertyEditorBase2.ValueChanged += (GodotObject obj, StringName prop, StringName fld, Variant val) =>
			{
				_newEntryProxy.Value = val;
			};
			_newValueContainer.AddChild(xWInspectorPropertyEditorBase2, forceReadableName: false, InternalMode.Disabled);
			xWInspectorPropertyEditorBase2.SetEditProperty(property2);
			xWInspectorPropertyEditorBase2.PropertyNameLabel.Text = "Value";
			_newValueEditor = xWInspectorPropertyEditorBase2;
		}
	}

	private void OnEntryValueChanged(XWInspectorPropertyEditorDictionaryItemContainer container, StringName property, Variant value)
	{
		if (!(container.EntryProxy is XWDictionaryEntryProxy xWDictionaryEntryProxy))
		{
			return;
		}
		Dictionary dictionary = DuplicateDictionary(_dictionary);
		if (property == (StringName)"key")
		{
			Variant key = xWDictionaryEntryProxy.Key;
			Variant variant = value;
			if (key.VariantType != variant.VariantType || !key.Equals(variant))
			{
				if (dictionary.ContainsKey(variant))
				{
					xWDictionaryEntryProxy.Key = key;
					RefreshProperty();
					return;
				}
				Variant value2 = xWDictionaryEntryProxy.Value;
				RemoveDictionaryKey(dictionary, key);
				dictionary[variant] = value2;
				xWDictionaryEntryProxy.Key = variant;
				CommitDictionaryChange(dictionary);
			}
		}
		else if (property == (StringName)"value")
		{
			Variant key2 = xWDictionaryEntryProxy.Key;
			SetDictionaryValue(dictionary, key2, value);
			xWDictionaryEntryProxy.Value = value;
			CommitDictionaryChange(dictionary, preserveEntryEditors: true);
		}
	}

	private void OnAddButtonPressed()
	{
		Variant key = _newEntryProxy.Key;
		Variant value = _newEntryProxy.Value;
		Dictionary dictionary = DuplicateDictionary(_dictionary);
		if (!dictionary.ContainsKey(key))
		{
			dictionary[key] = value;
			_pageIndex = GetMaxPage(dictionary.Count);
			CommitDictionaryChange(dictionary);
		}
	}

	private void OnRemoveItem(XWInspectorPropertyEditorDictionaryItemContainer container)
	{
		if (container.EntryProxy is XWDictionaryEntryProxy { Key: var key })
		{
			Dictionary dictionary = DuplicateDictionary(_dictionary);
			RemoveDictionaryKey(dictionary, key);
			container.QueueFree();
			CommitDictionaryChange(dictionary);
		}
	}

	private Dictionary ReadDictionaryValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Dictionary)
		{
			return DuplicateDictionary(propertyValue.As<Dictionary>());
		}
		return new Dictionary();
	}

	private static Dictionary DuplicateDictionary(Dictionary source)
	{
		if (source != null)
		{
			return source.Duplicate();
		}
		return new Dictionary();
	}

	private void CommitDictionaryChange(Dictionary nextDictionary, bool preserveEntryEditors = false)
	{
		_dictionary = DuplicateDictionary(nextDictionary);
		_preserveEntryEditorsWhileFocused = preserveEntryEditors;
		ValueChange(Variant.From(in nextDictionary));
		SizeUpdate();
		UpdatePaginator();
		if (!_preserveEntryEditorsWhileFocused || (!IsContinuousEditActive && !HasFocusedEntryEditor()))
		{
			_preserveEntryEditorsWhileFocused = false;
			RefreshProperty();
		}
	}

	private static void RemoveDictionaryKey(Dictionary dictionary, Variant key)
	{
		if (dictionary.ContainsKey(key))
		{
			dictionary.Remove(key);
			return;
		}
		string text = key.AsString();
		if (dictionary.ContainsKey(text))
		{
			dictionary.Remove(text);
		}
	}

	private static void SetDictionaryValue(Dictionary dictionary, Variant key, Variant value)
	{
		if (dictionary.ContainsKey(key))
		{
			dictionary[key] = value;
			return;
		}
		string text = key.AsString();
		if (dictionary.ContainsKey(text))
		{
			dictionary[text] = value;
		}
		else
		{
			dictionary[key] = value;
		}
	}

	private Variant.Type GetKeyEditorType(Variant key)
	{
		if (_keyType != Variant.Type.Nil)
		{
			return _keyType;
		}
		if (key.VariantType == Variant.Type.Nil)
		{
			return Variant.Type.Nil;
		}
		return key.VariantType;
	}

	private Variant.Type GetValueEditorType(Variant value)
	{
		if (_valueType != Variant.Type.Nil)
		{
			return _valueType;
		}
		if (value.VariantType == Variant.Type.Nil)
		{
			return Variant.Type.Nil;
		}
		return value.VariantType;
	}

	private XWInspectorPropertyEditorBase CreateEditorForType(Variant.Type type, PropertyHint hint, string hintString, Variant value)
	{
		if (hint != PropertyHint.None)
		{
			XWInspectorPropertyEditorBase hintEditor = XWTypeRegistry.Instance.GetHintEditor((int)hint);
			if (GodotObject.IsInstanceValid(hintEditor))
			{
				return hintEditor;
			}
		}
		if (type == Variant.Type.Object || hint == PropertyHint.ResourceType)
		{
			XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = CreateResourceEntryEditor(value, hint);
			if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
			{
				return xWInspectorPropertyEditorBase;
			}
		}
		if (hint == PropertyHint.ResourceType)
		{
			return XWTypeRegistry.Instance.GetResourcePropertyEditor();
		}
		return XWTypeRegistry.Instance.GetTypeEditor(type);
	}

	private bool ShouldUseReadOnlyVariantSummary(Variant value)
	{
		if (_valueType == Variant.Type.Nil)
		{
			return IsComplexVariantValue(value);
		}
		return false;
	}

	private static bool IsComplexVariantValue(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		if (variantType == Variant.Type.Object || (ulong)(variantType - 27) <= 11uL)
		{
			return true;
		}
		return false;
	}

	private Control CreateValueSummaryControl(Variant value)
	{
		return new Button
		{
			Text = FormatVariantSummary(value),
			TooltipText = FormatVariantSummary(value),
			Disabled = true,
			CustomMinimumSize = new Vector2(0f, 28f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ShrinkCenter,
			TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis
		};
	}

	private static string FormatVariantSummary(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 24;
		if ((ulong)num <= 14uL)
		{
			switch ((int)num)
			{
			case 4:
				return $"Array(大小 {value.AsGodotArray().Count})";
			case 3:
				return $"Dictionary(大小 {value.AsGodotDictionary().Count})";
			case 0:
				return FormatObjectSummary(value);
			case 5:
				return $"PackedByteArray(大小 {value.As<byte[]>().Length})";
			case 6:
				return $"PackedInt32Array(大小 {value.As<int[]>().Length})";
			case 7:
				return $"PackedInt64Array(大小 {value.As<long[]>().Length})";
			case 8:
				return $"PackedFloat32Array(大小 {value.As<float[]>().Length})";
			case 9:
				return $"PackedFloat64Array(大小 {value.As<double[]>().Length})";
			case 10:
				return $"PackedStringArray(大小 {value.As<string[]>().Length})";
			case 11:
				return $"PackedVector2Array(大小 {value.As<Vector2[]>().Length})";
			case 12:
				return $"PackedVector3Array(大小 {value.As<Vector3[]>().Length})";
			case 13:
				return $"PackedColorArray(大小 {value.As<Color[]>().Length})";
			case 14:
				return $"PackedVector4Array(大小 {value.As<Vector4[]>().Length})";
			}
		}
		return value.AsString();
	}

	private static string FormatObjectSummary(Variant value)
	{
		GodotObject godotObject = value.AsGodotObject();
		if (!GodotObject.IsInstanceValid(godotObject))
		{
			return "Object(null)";
		}
		if (!(godotObject is Resource resource))
		{
			return godotObject.GetClass();
		}
		return resource.GetClass() + "(Resource)";
	}

	private XWInspectorPropertyEditorBase CreateResourceEntryEditor(Variant value, PropertyHint hint)
	{
		if (value.VariantType != Variant.Type.Object)
		{
			return null;
		}
		Resource resource = value.As<GodotObject>() as Resource;
		if (!GodotObject.IsInstanceValid(resource))
		{
			if (hint != PropertyHint.ResourceType)
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

	private Variant CreateDefaultValue(Variant.Type type)
	{
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

	private void ParseDictionaryHint(string hintString)
	{
		_keyType = Variant.Type.String;
		_valueType = Variant.Type.Nil;
		_keyPropertyHint = PropertyHint.None;
		_valuePropertyHint = PropertyHint.None;
		_keyHintString = "";
		_valueHintString = "";
		if (!string.IsNullOrWhiteSpace(hintString))
		{
			string[] array = hintString.Split(';', 2);
			ParseTypeDescriptor(array[0], ref _keyType, ref _keyPropertyHint, ref _keyHintString);
			if (array.Length > 1)
			{
				ParseTypeDescriptor(array[1], ref _valueType, ref _valuePropertyHint, ref _valueHintString);
			}
		}
	}

	private static void ParseTypeDescriptor(string descriptor, ref Variant.Type type, ref PropertyHint hint, ref string hintString)
	{
		if (string.IsNullOrWhiteSpace(descriptor))
		{
			return;
		}
		string text = descriptor;
		int num = descriptor.IndexOf(':');
		if (num >= 0)
		{
			text = descriptor.Substring(0, num);
			string text2 = descriptor;
			int num2 = num + 1;
			hintString = text2.Substring(num2, text2.Length - num2);
		}
		int num3 = text.IndexOf('/');
		if (num3 >= 0)
		{
			string text2 = text;
			int num2 = num3 + 1;
			if (int.TryParse(text2.Substring(num2, text2.Length - num2), out var result))
			{
				hint = (PropertyHint)result;
			}
			text = text.Substring(0, num3);
		}
		if (TryParseVariantType(text, out var type2))
		{
			type = type2;
			return;
		}
		type = Variant.Type.Object;
		hint = PropertyHint.ResourceType;
		hintString = descriptor;
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

	private void OnKeyTypeButtonPressed()
	{
		_typeSelectorWindow = XWWindowTypeSelector.Create();
		_typeSelectorWindow.TypeSelect += OnKeyTypeSelected;
		AddChild(_typeSelectorWindow, forceReadableName: false, InternalMode.Disabled);
		_typeSelectorWindow.PopupCentered();
	}

	private void OnValueTypeButtonPressed()
	{
		_typeSelectorWindow = XWWindowTypeSelector.Create();
		_typeSelectorWindow.TypeSelect += OnValueTypeSelected;
		AddChild(_typeSelectorWindow, forceReadableName: false, InternalMode.Disabled);
		_typeSelectorWindow.PopupCentered();
	}

	private void OnKeyTypeSelected(Variant.Type type, StringName className)
	{
		_keyType = type;
		_keyPropertyHint = PropertyHint.None;
		_keyHintString = "";
		if (type == Variant.Type.Object && className != null && className.ToString() != "")
		{
			_keyPropertyHint = PropertyHint.ResourceType;
			_keyHintString = className.ToString();
		}
		UpdateTypeButtons();
		RefreshNewEntryEditors();
	}

	private void OnValueTypeSelected(Variant.Type type, StringName className)
	{
		_valueType = type;
		_valuePropertyHint = PropertyHint.None;
		_valueHintString = "";
		if (type == Variant.Type.Object && className != null && className.ToString() != "")
		{
			_valuePropertyHint = PropertyHint.ResourceType;
			_valueHintString = className.ToString();
		}
		UpdateTypeButtons();
		RefreshNewEntryEditors();
	}

	private void UpdateTypeButtons()
	{
		UpdateTypeButton(_keyTypeButton, "Key", _keyType, _keyHintString);
		UpdateTypeButton(_valueTypeButton, "Value", _valueType, _valueHintString);
	}

	private static void UpdateTypeButton(Button button, string label, Variant.Type type, string hintString)
	{
		if (GodotObject.IsInstanceValid(button))
		{
			if (type == Variant.Type.Nil)
			{
				button.Icon = null;
				button.TooltipText = label + ": Variant";
			}
			else if (XWTypeRegistry.Instance.HasType(type))
			{
				button.Icon = XWTypeRegistry.Instance.GetTypeIcon(type);
				string typeName = XWTypeRegistry.Instance.GetTypeName(type);
				button.TooltipText = (string.IsNullOrEmpty(hintString) ? (label + ": " + typeName) : $"{label}: {typeName} ({hintString})");
			}
			else
			{
				button.Icon = null;
				button.TooltipText = label + ": Variant";
			}
		}
	}

	private void OnDictButtonToggled(bool toggledOn)
	{
		_isExpanded = toggledOn;
		_editContainer.Visible = toggledOn;
		UpdatePaginator();
		if (toggledOn)
		{
			RefreshProperty();
			return;
		}
		ClearEntryEditors();
		ClearNewEntryEditors();
	}

	public override void HideEditor()
	{
		base.HideEditor();
		_isExpanded = false;
		_dictButton.ButtonPressed = false;
		_editContainer.Visible = false;
		ClearEntryEditors();
		ClearNewEntryEditors();
		UpdatePaginator();
	}

	public override void ShowEditor()
	{
		base.ShowEditor();
		_dictButton.ButtonPressed = _isExpanded;
		_editContainer.Visible = _isExpanded;
		if (!_isExpanded)
		{
			ClearEntryEditors();
			ClearNewEntryEditors();
		}
		UpdatePaginator();
	}

	private void SizeUpdate()
	{
		string value = ((_keyType != Variant.Type.Nil) ? ("[" + XWTypeRegistry.Instance.GetTypeName(_keyType)) : "[Variant");
		string value2 = ((_valueType != Variant.Type.Nil) ? XWTypeRegistry.Instance.GetTypeName(_valueType) : "Variant");
		_dictButton.Text = $"Dictionary{value}, {value2}]( 大小 {_dictionary.Count} )";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(51)
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
			new MethodInfo(MethodName.RefreshProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearEntryEditors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearNewEntryEditors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.BuildDictionaryFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatFingerprintValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatObjectFingerprint, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.HasRenderedEntryEditors, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasFocusedEntryEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsControlTreeFocused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetKeyAtIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshNewEntryEditors, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEntryValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAddButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRemoveItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadDictionaryValue, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitDictionaryChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "nextDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preserveEntryEditors", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveDictionaryKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "key", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDictionaryValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "key", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetKeyEditorType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "key", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetValueEditorType, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEditorForType, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldUseReadOnlyVariantSummary, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.IsComplexVariantValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateValueSummaryControl, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariantSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatObjectSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateResourceEntryEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDefaultValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ParseDictionaryHint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnKeyTypeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnValueTypeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnKeyTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnValueTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateTypeButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateTypeButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDictButtonToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SizeUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorDictionary>(Create());
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
		if (method == MethodName.ClearEntryEditors && args.Count == 0)
		{
			ClearEntryEditors();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearNewEntryEditors && args.Count == 0)
		{
			ClearNewEntryEditors();
			ret = default;
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
		if (method == MethodName.BuildDictionaryFingerprint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDictionaryFingerprint(VariantUtils.ConvertTo<Dictionary>(in args[0])));
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
		if (method == MethodName.HasRenderedEntryEditors && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRenderedEntryEditors());
			return true;
		}
		if (method == MethodName.HasFocusedEntryEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFocusedEntryEditor());
			return true;
		}
		if (method == MethodName.IsControlTreeFocused && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsControlTreeFocused(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetKeyAtIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetKeyAtIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshNewEntryEditors && args.Count == 0)
		{
			RefreshNewEntryEditors();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEntryValueChanged && args.Count == 3)
		{
			OnEntryValueChanged(VariantUtils.ConvertTo<XWInspectorPropertyEditorDictionaryItemContainer>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAddButtonPressed && args.Count == 0)
		{
			OnAddButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRemoveItem && args.Count == 1)
		{
			OnRemoveItem(VariantUtils.ConvertTo<XWInspectorPropertyEditorDictionaryItemContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadDictionaryValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ReadDictionaryValue());
			return true;
		}
		if (method == MethodName.DuplicateDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(DuplicateDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.CommitDictionaryChange && args.Count == 2)
		{
			CommitDictionaryChange(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveDictionaryKey && args.Count == 2)
		{
			RemoveDictionaryKey(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDictionaryValue && args.Count == 3)
		{
			SetDictionaryValue(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetKeyEditorType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant.Type>(GetKeyEditorType(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.GetValueEditorType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant.Type>(GetValueEditorType(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateEditorForType && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(CreateEditorForType(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<PropertyHint>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3])));
			return true;
		}
		if (method == MethodName.ShouldUseReadOnlyVariantSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUseReadOnlyVariantSummary(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.IsComplexVariantValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComplexVariantValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateValueSummaryControl && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateValueSummaryControl(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariantSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariantSummary(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObjectSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObjectSummary(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateResourceEntryEditor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(CreateResourceEntryEditor(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<PropertyHint>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateDefaultValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(CreateDefaultValue(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.ParseDictionaryHint && args.Count == 1)
		{
			ParseDictionaryHint(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnKeyTypeButtonPressed && args.Count == 0)
		{
			OnKeyTypeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnValueTypeButtonPressed && args.Count == 0)
		{
			OnValueTypeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnKeyTypeSelected && args.Count == 2)
		{
			OnKeyTypeSelected(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnValueTypeSelected && args.Count == 2)
		{
			OnValueTypeSelected(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateTypeButtons && args.Count == 0)
		{
			UpdateTypeButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateTypeButton && args.Count == 4)
		{
			UpdateTypeButton(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDictButtonToggled && args.Count == 1)
		{
			OnDictButtonToggled(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorDictionary>(Create());
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
		if (method == MethodName.IsControlTreeFocused && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsControlTreeFocused(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(DuplicateDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveDictionaryKey && args.Count == 2)
		{
			RemoveDictionaryKey(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDictionaryValue && args.Count == 3)
		{
			SetDictionaryValue(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsComplexVariantValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComplexVariantValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVariantSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariantSummary(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObjectSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObjectSummary(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateTypeButton && args.Count == 4)
		{
			UpdateTypeButton(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
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
		if (method == MethodName.ClearEntryEditors)
		{
			return true;
		}
		if (method == MethodName.ClearNewEntryEditors)
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
		if (method == MethodName.BuildDictionaryFingerprint)
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
		if (method == MethodName.HasRenderedEntryEditors)
		{
			return true;
		}
		if (method == MethodName.HasFocusedEntryEditor)
		{
			return true;
		}
		if (method == MethodName.IsControlTreeFocused)
		{
			return true;
		}
		if (method == MethodName.GetKeyAtIndex)
		{
			return true;
		}
		if (method == MethodName.RefreshNewEntryEditors)
		{
			return true;
		}
		if (method == MethodName.OnEntryValueChanged)
		{
			return true;
		}
		if (method == MethodName.OnAddButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnRemoveItem)
		{
			return true;
		}
		if (method == MethodName.ReadDictionaryValue)
		{
			return true;
		}
		if (method == MethodName.DuplicateDictionary)
		{
			return true;
		}
		if (method == MethodName.CommitDictionaryChange)
		{
			return true;
		}
		if (method == MethodName.RemoveDictionaryKey)
		{
			return true;
		}
		if (method == MethodName.SetDictionaryValue)
		{
			return true;
		}
		if (method == MethodName.GetKeyEditorType)
		{
			return true;
		}
		if (method == MethodName.GetValueEditorType)
		{
			return true;
		}
		if (method == MethodName.CreateEditorForType)
		{
			return true;
		}
		if (method == MethodName.ShouldUseReadOnlyVariantSummary)
		{
			return true;
		}
		if (method == MethodName.IsComplexVariantValue)
		{
			return true;
		}
		if (method == MethodName.CreateValueSummaryControl)
		{
			return true;
		}
		if (method == MethodName.FormatVariantSummary)
		{
			return true;
		}
		if (method == MethodName.FormatObjectSummary)
		{
			return true;
		}
		if (method == MethodName.CreateResourceEntryEditor)
		{
			return true;
		}
		if (method == MethodName.CreateDefaultValue)
		{
			return true;
		}
		if (method == MethodName.ParseDictionaryHint)
		{
			return true;
		}
		if (method == MethodName.OnKeyTypeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnValueTypeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnKeyTypeSelected)
		{
			return true;
		}
		if (method == MethodName.OnValueTypeSelected)
		{
			return true;
		}
		if (method == MethodName.UpdateTypeButtons)
		{
			return true;
		}
		if (method == MethodName.UpdateTypeButton)
		{
			return true;
		}
		if (method == MethodName.OnDictButtonToggled)
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
		if (name == PropertyName._dictButton)
		{
			_dictButton = VariantUtils.ConvertTo<Button>(in value);
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
		if (name == PropertyName._keyTypeButton)
		{
			_keyTypeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._valueTypeButton)
		{
			_valueTypeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._newKeyContainer)
		{
			_newKeyContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._newValueContainer)
		{
			_newValueContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
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
		if (name == PropertyName._dictionary)
		{
			_dictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._keyType)
		{
			_keyType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._valueType)
		{
			_valueType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._keyPropertyHint)
		{
			_keyPropertyHint = VariantUtils.ConvertTo<PropertyHint>(in value);
			return true;
		}
		if (name == PropertyName._valuePropertyHint)
		{
			_valuePropertyHint = VariantUtils.ConvertTo<PropertyHint>(in value);
			return true;
		}
		if (name == PropertyName._keyHintString)
		{
			_keyHintString = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._valueHintString)
		{
			_valueHintString = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._typeSelectorWindow)
		{
			_typeSelectorWindow = VariantUtils.ConvertTo<XWWindowTypeSelector>(in value);
			return true;
		}
		if (name == PropertyName._newEntryProxy)
		{
			_newEntryProxy = VariantUtils.ConvertTo<XWDictionaryEntryProxy>(in value);
			return true;
		}
		if (name == PropertyName._newKeyEditor)
		{
			_newKeyEditor = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		if (name == PropertyName._newValueEditor)
		{
			_newValueEditor = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		if (name == PropertyName._preserveEntryEditorsWhileFocused)
		{
			_preserveEntryEditorsWhileFocused = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._dictButton)
		{
			value = VariantUtils.CreateFrom(in _dictButton);
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
		if (name == PropertyName._keyTypeButton)
		{
			value = VariantUtils.CreateFrom(in _keyTypeButton);
			return true;
		}
		if (name == PropertyName._valueTypeButton)
		{
			value = VariantUtils.CreateFrom(in _valueTypeButton);
			return true;
		}
		if (name == PropertyName._newKeyContainer)
		{
			value = VariantUtils.CreateFrom(in _newKeyContainer);
			return true;
		}
		if (name == PropertyName._newValueContainer)
		{
			value = VariantUtils.CreateFrom(in _newValueContainer);
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
		if (name == PropertyName._dictionary)
		{
			value = VariantUtils.CreateFrom(in _dictionary);
			return true;
		}
		if (name == PropertyName._keyType)
		{
			value = VariantUtils.CreateFrom(in _keyType);
			return true;
		}
		if (name == PropertyName._valueType)
		{
			value = VariantUtils.CreateFrom(in _valueType);
			return true;
		}
		if (name == PropertyName._keyPropertyHint)
		{
			value = VariantUtils.CreateFrom(in _keyPropertyHint);
			return true;
		}
		if (name == PropertyName._valuePropertyHint)
		{
			value = VariantUtils.CreateFrom(in _valuePropertyHint);
			return true;
		}
		if (name == PropertyName._keyHintString)
		{
			value = VariantUtils.CreateFrom(in _keyHintString);
			return true;
		}
		if (name == PropertyName._valueHintString)
		{
			value = VariantUtils.CreateFrom(in _valueHintString);
			return true;
		}
		if (name == PropertyName._typeSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _typeSelectorWindow);
			return true;
		}
		if (name == PropertyName._newEntryProxy)
		{
			value = VariantUtils.CreateFrom(in _newEntryProxy);
			return true;
		}
		if (name == PropertyName._newKeyEditor)
		{
			value = VariantUtils.CreateFrom(in _newKeyEditor);
			return true;
		}
		if (name == PropertyName._newValueEditor)
		{
			value = VariantUtils.CreateFrom(in _newValueEditor);
			return true;
		}
		if (name == PropertyName._preserveEntryEditorsWhileFocused)
		{
			value = VariantUtils.CreateFrom(in _preserveEntryEditorsWhileFocused);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._dictButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._keyTypeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueTypeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newKeyContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newValueContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._dictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._keyType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._valueType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._keyPropertyHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._valuePropertyHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._keyHintString, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._valueHintString, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newEntryProxy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newKeyEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newValueEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveEntryEditorsWhileFocused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isExpanded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pageIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastRenderedFingerprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._dictButton, Variant.From(in _dictButton));
		info.AddProperty(PropertyName._addButton, Variant.From(in _addButton));
		info.AddProperty(PropertyName._editContainer, Variant.From(in _editContainer));
		info.AddProperty(PropertyName._editorContainer, Variant.From(in _editorContainer));
		info.AddProperty(PropertyName._keyTypeButton, Variant.From(in _keyTypeButton));
		info.AddProperty(PropertyName._valueTypeButton, Variant.From(in _valueTypeButton));
		info.AddProperty(PropertyName._newKeyContainer, Variant.From(in _newKeyContainer));
		info.AddProperty(PropertyName._newValueContainer, Variant.From(in _newValueContainer));
		info.AddProperty(PropertyName._previousPageButton, Variant.From(in _previousPageButton));
		info.AddProperty(PropertyName._nextPageButton, Variant.From(in _nextPageButton));
		info.AddProperty(PropertyName._pageLabel, Variant.From(in _pageLabel));
		info.AddProperty(PropertyName._dictionary, Variant.From(in _dictionary));
		info.AddProperty(PropertyName._keyType, Variant.From(in _keyType));
		info.AddProperty(PropertyName._valueType, Variant.From(in _valueType));
		info.AddProperty(PropertyName._keyPropertyHint, Variant.From(in _keyPropertyHint));
		info.AddProperty(PropertyName._valuePropertyHint, Variant.From(in _valuePropertyHint));
		info.AddProperty(PropertyName._keyHintString, Variant.From(in _keyHintString));
		info.AddProperty(PropertyName._valueHintString, Variant.From(in _valueHintString));
		info.AddProperty(PropertyName._typeSelectorWindow, Variant.From(in _typeSelectorWindow));
		info.AddProperty(PropertyName._newEntryProxy, Variant.From(in _newEntryProxy));
		info.AddProperty(PropertyName._newKeyEditor, Variant.From(in _newKeyEditor));
		info.AddProperty(PropertyName._newValueEditor, Variant.From(in _newValueEditor));
		info.AddProperty(PropertyName._preserveEntryEditorsWhileFocused, Variant.From(in _preserveEntryEditorsWhileFocused));
		info.AddProperty(PropertyName._isExpanded, Variant.From(in _isExpanded));
		info.AddProperty(PropertyName._pageIndex, Variant.From(in _pageIndex));
		info.AddProperty(PropertyName._lastRenderedFingerprint, Variant.From(in _lastRenderedFingerprint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._dictButton, out var value))
		{
			_dictButton = value.As<Button>();
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
		if (info.TryGetProperty(PropertyName._keyTypeButton, out var value5))
		{
			_keyTypeButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._valueTypeButton, out var value6))
		{
			_valueTypeButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._newKeyContainer, out var value7))
		{
			_newKeyContainer = value7.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._newValueContainer, out var value8))
		{
			_newValueContainer = value8.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._previousPageButton, out var value9))
		{
			_previousPageButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextPageButton, out var value10))
		{
			_nextPageButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pageLabel, out var value11))
		{
			_pageLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._dictionary, out var value12))
		{
			_dictionary = value12.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._keyType, out var value13))
		{
			_keyType = value13.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._valueType, out var value14))
		{
			_valueType = value14.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._keyPropertyHint, out var value15))
		{
			_keyPropertyHint = value15.As<PropertyHint>();
		}
		if (info.TryGetProperty(PropertyName._valuePropertyHint, out var value16))
		{
			_valuePropertyHint = value16.As<PropertyHint>();
		}
		if (info.TryGetProperty(PropertyName._keyHintString, out var value17))
		{
			_keyHintString = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName._valueHintString, out var value18))
		{
			_valueHintString = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName._typeSelectorWindow, out var value19))
		{
			_typeSelectorWindow = value19.As<XWWindowTypeSelector>();
		}
		if (info.TryGetProperty(PropertyName._newEntryProxy, out var value20))
		{
			_newEntryProxy = value20.As<XWDictionaryEntryProxy>();
		}
		if (info.TryGetProperty(PropertyName._newKeyEditor, out var value21))
		{
			_newKeyEditor = value21.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetProperty(PropertyName._newValueEditor, out var value22))
		{
			_newValueEditor = value22.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetProperty(PropertyName._preserveEntryEditorsWhileFocused, out var value23))
		{
			_preserveEntryEditorsWhileFocused = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isExpanded, out var value24))
		{
			_isExpanded = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pageIndex, out var value25))
		{
			_pageIndex = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastRenderedFingerprint, out var value26))
		{
			_lastRenderedFingerprint = value26.As<string>();
		}
	}
}
