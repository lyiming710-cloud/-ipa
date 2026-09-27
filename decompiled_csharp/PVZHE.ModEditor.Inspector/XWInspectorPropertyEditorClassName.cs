using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/ClassName/XWInspectorPropertyEditorClassName.cs")]
public class XWInspectorPropertyEditorClassName : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName PopulateClassList = "PopulateClassList";

		public static readonly StringName OnTextChanged = "OnTextChanged";

		public static readonly StringName OnBrowsePressed = "OnBrowsePressed";

		public static readonly StringName OnSearchChanged = "OnSearchChanged";

		public static readonly StringName OnClassSelected = "OnClassSelected";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _lineEdit = "_lineEdit";

		public static readonly StringName _browseButton = "_browseButton";

		public static readonly StringName _classPopup = "_classPopup";

		public static readonly StringName _classList = "_classList";

		public static readonly StringName _classSearch = "_classSearch";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/ClassName/XWInspectorPropertyEditorClassName.tscn";

	private LineEdit _lineEdit;

	private Button _browseButton;

	private PopupPanel _classPopup;

	private ItemList _classList;

	private LineEdit _classSearch;

	public static XWInspectorPropertyEditorClassName Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/ClassName/XWInspectorPropertyEditorClassName.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorClassName>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_lineEdit = GetNode<LineEdit>("%LineEdit");
		_browseButton = GetNode<Button>("%BrowseButton");
		_classPopup = GetNode<PopupPanel>("%ClassPopup");
		_classList = GetNode<ItemList>("%ClassList");
		_classSearch = GetNode<LineEdit>("%ClassSearch");
		_lineEdit.TextChanged += OnTextChanged;
		_browseButton.Pressed += OnBrowsePressed;
		_classSearch.TextChanged += OnSearchChanged;
		_classList.ItemSelected += OnClassSelected;
		PopulateClassList("");
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			XWInspectorPropertyEditorBase.SetLineEditTextPreservingCaret(_lineEdit, propertyValue.AsString());
		}
	}

	public override Variant GetValue()
	{
		return _lineEdit.Text;
	}

	private void PopulateClassList(string filter)
	{
		_classList.Clear();
		string[] classList = ClassDB.GetClassList();
		List<string> list = new List<string>();
		string[] array = classList;
		foreach (string text in array)
		{
			list.Add(text.ToString());
		}
		list.Sort();
		string value = filter.ToLower();
		foreach (string item in list)
		{
			if (string.IsNullOrEmpty(filter) || item.ToLower().Contains(value))
			{
				_classList.AddItem(item);
			}
		}
	}

	private void OnTextChanged(string newText)
	{
		ValueChange(newText);
	}

	private void OnBrowsePressed()
	{
		_classSearch.Text = "";
		PopulateClassList("");
		_classPopup.Position = new Vector2I((int)GetGlobalMousePosition().X, (int)GetGlobalMousePosition().Y);
		_classPopup.Popup();
	}

	private void OnSearchChanged(string filter)
	{
		PopulateClassList(filter);
	}

	private void OnClassSelected(long index)
	{
		int num = (int)index;
		if (num >= 0 && num < _classList.ItemCount)
		{
			string itemText = _classList.GetItemText(num);
			_lineEdit.Text = itemText;
			ValueChange(itemText);
			_classPopup.Hide();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateClassList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBrowsePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSearchChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnClassSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorClassName>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
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
		if (method == MethodName.PopulateClassList && args.Count == 1)
		{
			PopulateClassList(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextChanged && args.Count == 1)
		{
			OnTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBrowsePressed && args.Count == 0)
		{
			OnBrowsePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchChanged && args.Count == 1)
		{
			OnSearchChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnClassSelected && args.Count == 1)
		{
			OnClassSelected(VariantUtils.ConvertTo<long>(in args[0]));
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorClassName>(Create());
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
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.PopulateClassList)
		{
			return true;
		}
		if (method == MethodName.OnTextChanged)
		{
			return true;
		}
		if (method == MethodName.OnBrowsePressed)
		{
			return true;
		}
		if (method == MethodName.OnSearchChanged)
		{
			return true;
		}
		if (method == MethodName.OnClassSelected)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._lineEdit)
		{
			_lineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._browseButton)
		{
			_browseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._classPopup)
		{
			_classPopup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._classList)
		{
			_classList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._classSearch)
		{
			_classSearch = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._lineEdit)
		{
			value = VariantUtils.CreateFrom(in _lineEdit);
			return true;
		}
		if (name == PropertyName._browseButton)
		{
			value = VariantUtils.CreateFrom(in _browseButton);
			return true;
		}
		if (name == PropertyName._classPopup)
		{
			value = VariantUtils.CreateFrom(in _classPopup);
			return true;
		}
		if (name == PropertyName._classList)
		{
			value = VariantUtils.CreateFrom(in _classList);
			return true;
		}
		if (name == PropertyName._classSearch)
		{
			value = VariantUtils.CreateFrom(in _classSearch);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._lineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._browseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._classPopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._classList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._classSearch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._lineEdit, Variant.From(in _lineEdit));
		info.AddProperty(PropertyName._browseButton, Variant.From(in _browseButton));
		info.AddProperty(PropertyName._classPopup, Variant.From(in _classPopup));
		info.AddProperty(PropertyName._classList, Variant.From(in _classList));
		info.AddProperty(PropertyName._classSearch, Variant.From(in _classSearch));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._lineEdit, out var value))
		{
			_lineEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._browseButton, out var value2))
		{
			_browseButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._classPopup, out var value3))
		{
			_classPopup = value3.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._classList, out var value4))
		{
			_classList = value4.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._classSearch, out var value5))
		{
			_classSearch = value5.As<LineEdit>();
		}
	}
}
