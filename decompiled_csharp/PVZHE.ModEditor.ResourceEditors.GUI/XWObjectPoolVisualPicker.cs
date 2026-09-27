using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWObjectPoolVisualPicker.cs")]
public class XWObjectPoolVisualPicker : VBoxContainer
{
	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetSelectedId = "SetSelectedId";

		public static readonly StringName SetSearchText = "SetSearchText";

		public static readonly StringName OnSearchChanged = "OnSearchChanged";

		public static readonly StringName RebuildFilteredChoices = "RebuildFilteredChoices";

		public static readonly StringName RebuildPage = "RebuildPage";

		public static readonly StringName SelectVisibleChoice = "SelectVisibleChoice";

		public static readonly StringName OnItemSelected = "OnItemSelected";

		public static readonly StringName ChangePage = "ChangePage";

		public static readonly StringName RefreshSelectedLabel = "RefreshSelectedLabel";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName SelectedChoiceId = "SelectedChoiceId";

		public static readonly StringName TotalChoiceCount = "TotalChoiceCount";

		public static readonly StringName VisibleChoiceCount = "VisibleChoiceCount";

		public static readonly StringName PageCount = "PageCount";

		public static readonly StringName HasSearchAndPaging = "HasSearchAndPaging";

		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _choiceList = "_choiceList";

		public static readonly StringName _selectedLabel = "_selectedLabel";

		public static readonly StringName _pageLabel = "_pageLabel";

		public static readonly StringName _previousButton = "_previousButton";

		public static readonly StringName _nextButton = "_nextButton";

		public static readonly StringName _selectedId = "_selectedId";

		public static readonly StringName _page = "_page";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	private const int PageSize = 18;

	private readonly List<XWObjectPoolVisualChoice> _choices = new List<XWObjectPoolVisualChoice>();

	private readonly List<XWObjectPoolVisualChoice> _filteredChoices = new List<XWObjectPoolVisualChoice>();

	private LineEdit _searchEdit;

	private ItemList _choiceList;

	private Label _selectedLabel;

	private Label _pageLabel;

	private Button _previousButton;

	private Button _nextButton;

	private int _selectedId;

	private int _page;

	private bool _updating;

	public int SelectedChoiceId => _selectedId;

	public int TotalChoiceCount => _choices.Count;

	public int VisibleChoiceCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_choiceList))
			{
				return 0;
			}
			return _choiceList.ItemCount;
		}
	}

	public int PageCount => Math.Max(1, Mathf.CeilToInt((float)_filteredChoices.Count / 18f));

	public bool HasSearchAndPaging
	{
		get
		{
			if (GodotObject.IsInstanceValid(_searchEdit) && GodotObject.IsInstanceValid(_previousButton))
			{
				return GodotObject.IsInstanceValid(_nextButton);
			}
			return false;
		}
	}

	public event Action<int> ChoiceSelected;

	public override void _Ready()
	{
		_searchEdit = GetNode<LineEdit>("%SearchEdit");
		_choiceList = GetNode<ItemList>("%ChoiceList");
		_selectedLabel = GetNode<Label>("%SelectedLabel");
		_pageLabel = GetNode<Label>("%PageLabel");
		_previousButton = GetNode<Button>("%PreviousButton");
		_nextButton = GetNode<Button>("%NextButton");
		_searchEdit.TextChanged += OnSearchChanged;
		_choiceList.ItemSelected += OnItemSelected;
		_previousButton.Pressed += () =>
		{
			ChangePage(-1);
		};
		_nextButton.Pressed += () =>
		{
			ChangePage(1);
		};
		RebuildFilteredChoices();
	}

	public void Configure(IEnumerable<XWObjectPoolVisualChoice> choices, int selectedId)
	{
		_choices.Clear();
		if (choices != null)
		{
			_choices.AddRange(choices);
		}
		_selectedId = selectedId;
		_page = 0;
		RebuildFilteredChoices();
	}

	public void SetSelectedId(int selectedId)
	{
		_selectedId = selectedId;
		RefreshSelectedLabel();
		SelectVisibleChoice();
	}

	public void SetSearchText(string text)
	{
		if (GodotObject.IsInstanceValid(_searchEdit))
		{
			_searchEdit.Text = text ?? string.Empty;
			OnSearchChanged(_searchEdit.Text);
		}
	}

	private void OnSearchChanged(string _)
	{
		_page = 0;
		RebuildFilteredChoices();
	}

	private void RebuildFilteredChoices()
	{
		_filteredChoices.Clear();
		string text = (GodotObject.IsInstanceValid(_searchEdit) ? _searchEdit.Text.Trim() : string.Empty);
		foreach (XWObjectPoolVisualChoice choice in _choices)
		{
			if (text.Length == 0 || choice.Title.Contains(text, StringComparison.OrdinalIgnoreCase) || choice.Detail.Contains(text, StringComparison.OrdinalIgnoreCase) || choice.Id.ToString().Contains(text, StringComparison.OrdinalIgnoreCase))
			{
				_filteredChoices.Add(choice);
			}
		}
		_page = Math.Clamp(_page, 0, PageCount - 1);
		RebuildPage();
	}

	private void RebuildPage()
	{
		if (!GodotObject.IsInstanceValid(_choiceList))
		{
			return;
		}
		_updating = true;
		try
		{
			_choiceList.Clear();
			int num = _page * 18;
			int num2 = Math.Min(_filteredChoices.Count, num + 18);
			for (int i = num; i < num2; i++)
			{
				XWObjectPoolVisualChoice xWObjectPoolVisualChoice = _filteredChoices[i];
				int idx = _choiceList.AddItem(xWObjectPoolVisualChoice.Title, xWObjectPoolVisualChoice.Icon);
				_choiceList.SetItemMetadata(idx, xWObjectPoolVisualChoice.Id);
				_choiceList.SetItemTooltip(idx, string.IsNullOrWhiteSpace(xWObjectPoolVisualChoice.Detail) ? $"{xWObjectPoolVisualChoice.Title} · ID {xWObjectPoolVisualChoice.Id}" : $"{xWObjectPoolVisualChoice.Title} · {xWObjectPoolVisualChoice.Detail} · ID {xWObjectPoolVisualChoice.Id}");
			}
			SelectVisibleChoice();
		}
		finally
		{
			_updating = false;
		}
		if (GodotObject.IsInstanceValid(_pageLabel))
		{
			_pageLabel.Text = $"{_page + 1} / {PageCount} · {_filteredChoices.Count} 项";
		}
		if (GodotObject.IsInstanceValid(_previousButton))
		{
			_previousButton.Disabled = _page <= 0;
		}
		if (GodotObject.IsInstanceValid(_nextButton))
		{
			_nextButton.Disabled = _page >= PageCount - 1;
		}
		RefreshSelectedLabel();
	}

	private void SelectVisibleChoice()
	{
		if (!GodotObject.IsInstanceValid(_choiceList))
		{
			return;
		}
		for (int i = 0; i < _choiceList.ItemCount; i++)
		{
			if (_choiceList.GetItemMetadata(i).AsInt32() == _selectedId)
			{
				_choiceList.Select(i);
				return;
			}
		}
		_choiceList.DeselectAll();
	}

	private void OnItemSelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_choiceList) && index >= 0 && index < _choiceList.ItemCount)
		{
			_selectedId = _choiceList.GetItemMetadata((int)index).AsInt32();
			RefreshSelectedLabel();
			ChoiceSelected?.Invoke(_selectedId);
		}
	}

	private void ChangePage(int direction)
	{
		_page = Math.Clamp(_page + direction, 0, PageCount - 1);
		RebuildPage();
	}

	private void RefreshSelectedLabel()
	{
		if (GodotObject.IsInstanceValid(_selectedLabel))
		{
			XWObjectPoolVisualChoice xWObjectPoolVisualChoice = _choices.Find((XWObjectPoolVisualChoice choice) => choice.Id == _selectedId);
			_selectedLabel.Text = ((xWObjectPoolVisualChoice == null) ? $"当前 ID {_selectedId}" : ("当前：" + xWObjectPoolVisualChoice.Title + " · " + xWObjectPoolVisualChoice.Detail));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSelectedId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "selectedId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSearchText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSearchChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildFilteredChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectVisibleChoice, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangePage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSelectedLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetSelectedId && args.Count == 1)
		{
			SetSelectedId(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSearchText && args.Count == 1)
		{
			SetSearchText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchChanged && args.Count == 1)
		{
			OnSearchChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildFilteredChoices && args.Count == 0)
		{
			RebuildFilteredChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildPage && args.Count == 0)
		{
			RebuildPage();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectVisibleChoice && args.Count == 0)
		{
			SelectVisibleChoice();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemSelected && args.Count == 1)
		{
			OnItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangePage && args.Count == 1)
		{
			ChangePage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSelectedLabel && args.Count == 0)
		{
			RefreshSelectedLabel();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetSelectedId)
		{
			return true;
		}
		if (method == MethodName.SetSearchText)
		{
			return true;
		}
		if (method == MethodName.OnSearchChanged)
		{
			return true;
		}
		if (method == MethodName.RebuildFilteredChoices)
		{
			return true;
		}
		if (method == MethodName.RebuildPage)
		{
			return true;
		}
		if (method == MethodName.SelectVisibleChoice)
		{
			return true;
		}
		if (method == MethodName.OnItemSelected)
		{
			return true;
		}
		if (method == MethodName.ChangePage)
		{
			return true;
		}
		if (method == MethodName.RefreshSelectedLabel)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._choiceList)
		{
			_choiceList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._selectedLabel)
		{
			_selectedLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pageLabel)
		{
			_pageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			_previousButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			_nextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._selectedId)
		{
			_selectedId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._page)
		{
			_page = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._updating)
		{
			_updating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.SelectedChoiceId)
		{
			from = SelectedChoiceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TotalChoiceCount)
		{
			from = TotalChoiceCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisibleChoiceCount)
		{
			from = VisibleChoiceCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PageCount)
		{
			from = PageCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasSearchAndPaging)
		{
			value = VariantUtils.CreateFrom<bool>(HasSearchAndPaging);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._choiceList)
		{
			value = VariantUtils.CreateFrom(in _choiceList);
			return true;
		}
		if (name == PropertyName._selectedLabel)
		{
			value = VariantUtils.CreateFrom(in _selectedLabel);
			return true;
		}
		if (name == PropertyName._pageLabel)
		{
			value = VariantUtils.CreateFrom(in _pageLabel);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			value = VariantUtils.CreateFrom(in _previousButton);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			value = VariantUtils.CreateFrom(in _nextButton);
			return true;
		}
		if (name == PropertyName._selectedId)
		{
			value = VariantUtils.CreateFrom(in _selectedId);
			return true;
		}
		if (name == PropertyName._page)
		{
			value = VariantUtils.CreateFrom(in _page);
			return true;
		}
		if (name == PropertyName._updating)
		{
			value = VariantUtils.CreateFrom(in _updating);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._choiceList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._page, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedChoiceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TotalChoiceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleChoiceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PageCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasSearchAndPaging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._choiceList, Variant.From(in _choiceList));
		info.AddProperty(PropertyName._selectedLabel, Variant.From(in _selectedLabel));
		info.AddProperty(PropertyName._pageLabel, Variant.From(in _pageLabel));
		info.AddProperty(PropertyName._previousButton, Variant.From(in _previousButton));
		info.AddProperty(PropertyName._nextButton, Variant.From(in _nextButton));
		info.AddProperty(PropertyName._selectedId, Variant.From(in _selectedId));
		info.AddProperty(PropertyName._page, Variant.From(in _page));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._searchEdit, out var value))
		{
			_searchEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._choiceList, out var value2))
		{
			_choiceList = value2.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._selectedLabel, out var value3))
		{
			_selectedLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pageLabel, out var value4))
		{
			_pageLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previousButton, out var value5))
		{
			_previousButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextButton, out var value6))
		{
			_nextButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._selectedId, out var value7))
		{
			_selectedId = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._page, out var value8))
		{
			_page = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value9))
		{
			_updating = value9.As<bool>();
		}
	}
}
