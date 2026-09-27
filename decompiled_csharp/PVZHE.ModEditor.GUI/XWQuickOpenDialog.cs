using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.FileSystem;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWQuickOpenDialog.cs")]
public class XWQuickOpenDialog : ConfirmationDialog
{
	[Signal]
	public delegate void FileOpenedEventHandler(string path);

	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BuildTypeFilter = "BuildTypeFilter";

		public static readonly StringName BindTypeChips = "BindTypeChips";

		public static readonly StringName SelectType = "SelectType";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName FilterItems = "FilterItems";

		public static readonly StringName CollectMatching = "CollectMatching";

		public static readonly StringName RenderResultPage = "RenderResultPage";

		public static readonly StringName ChangePage = "ChangePage";

		public static readonly StringName OnSearchGuiInput = "OnSearchGuiInput";

		public static readonly StringName MatchesSelectedType = "MatchesSelectedType";

		public static readonly StringName GetFileTypeIcon = "GetFileTypeIcon";

		public static readonly StringName OnConfirmed = "OnConfirmed";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _typeFilter = "_typeFilter";

		public static readonly StringName _searchLine = "_searchLine";

		public static readonly StringName _resultList = "_resultList";

		public static readonly StringName _resultCount = "_resultCount";

		public static readonly StringName _typeChips = "_typeChips";

		public static readonly StringName _previousPageButton = "_previousPageButton";

		public static readonly StringName _nextPageButton = "_nextPageButton";

		public static readonly StringName _pageLabel = "_pageLabel";

		public static readonly StringName _selectedType = "_selectedType";

		public static readonly StringName _currentPage = "_currentPage";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
		public static readonly StringName FileOpened = "FileOpened";
	}

	private const int PageSize = 120;

	private OptionButton _typeFilter;

	private LineEdit _searchLine;

	private ItemList _resultList;

	private Label _resultCount;

	private Button[] _typeChips;

	private readonly List<(string Name, string Path)> _matches = new List<(string, string)>();

	private Button _previousPageButton;

	private Button _nextPageButton;

	private Label _pageLabel;

	private int _selectedType;

	private int _currentPage;

	private FileOpenedEventHandler backing_FileOpened;

	public event FileOpenedEventHandler FileOpened
	{
		add
		{
			backing_FileOpened = (FileOpenedEventHandler)Delegate.Combine(backing_FileOpened, value);
		}
		remove
		{
			backing_FileOpened = (FileOpenedEventHandler)Delegate.Remove(backing_FileOpened, value);
		}
	}

	public override void _Ready()
	{
		_typeFilter = GetNode<OptionButton>("%TypeFilter");
		_searchLine = GetNode<LineEdit>("%SearchLine");
		_resultList = GetNode<ItemList>("%ResultList");
		_resultCount = GetNode<Label>("%ResultCount");
		_previousPageButton = GetNode<Button>("%PreviousPageButton");
		_nextPageButton = GetNode<Button>("%NextPageButton");
		_pageLabel = GetNode<Label>("%PageLabel");
		_typeChips = new Button[4]
		{
			GetNode<Button>("%AllTypeChip"),
			GetNode<Button>("%ScriptTypeChip"),
			GetNode<Button>("%SceneTypeChip"),
			GetNode<Button>("%ResourceTypeChip")
		};
		_searchLine.TextChanged += (string _) =>
		{
			FilterItems();
		};
		_searchLine.GuiInput += OnSearchGuiInput;
		_resultList.ItemActivated += (long _) =>
		{
			OnConfirmed();
		};
		_previousPageButton.Pressed += () =>
		{
			ChangePage(-1);
		};
		_nextPageButton.Pressed += () =>
		{
			ChangePage(1);
		};
		Confirmed += OnConfirmed;
		BuildTypeFilter();
		BindTypeChips();
	}

	private void BuildTypeFilter()
	{
		_typeFilter.Clear();
		_typeFilter.AddItem("全部", 0);
		_typeFilter.AddItem("脚本", 1);
		_typeFilter.AddItem("场景", 2);
		_typeFilter.AddItem("资源", 3);
	}

	private void BindTypeChips()
	{
		ButtonGroup buttonGroup = new ButtonGroup
		{
			AllowUnpress = false
		};
		for (int i = 0; i < _typeChips.Length; i++)
		{
			int type = i;
			Button obj = _typeChips[i];
			obj.ButtonGroup = buttonGroup;
			obj.Pressed += () =>
			{
				SelectType(type);
			};
		}
		_typeFilter.ItemSelected += (long index) =>
		{
			SelectType((int)index);
		};
		SelectType(0);
	}

	private void SelectType(int type)
	{
		_selectedType = Mathf.Clamp(type, 0, _typeChips.Length - 1);
		_typeFilter.Select(_selectedType);
		for (int i = 0; i < _typeChips.Length; i++)
		{
			_typeChips[i].SetPressedNoSignal(i == _selectedType);
		}
		FilterItems();
	}

	public void Refresh()
	{
		FilterItems();
	}

	private void FilterItems()
	{
		_matches.Clear();
		string text = _searchLine.Text;
		XWFileSystemDirectory filesystem = XWFileSystem.GetSingleton().GetFilesystem();
		if (filesystem == null)
		{
			_resultCount.Text = "0 个结果";
			RenderResultPage();
		}
		else
		{
			CollectMatching(filesystem, text);
			_currentPage = 0;
			RenderResultPage();
		}
	}

	private void CollectMatching(XWFileSystemDirectory dir, string search)
	{
		foreach (XWFileInfo file in dir.Files)
		{
			if ((string.IsNullOrEmpty(search) || file.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) && MatchesSelectedType(file.Path))
			{
				_matches.Add((file.Name, file.Path));
			}
		}
		foreach (XWFileSystemDirectory subdir in dir.Subdirs)
		{
			CollectMatching(subdir, search);
		}
	}

	private void RenderResultPage()
	{
		_resultList.Clear();
		int num = Math.Max(1, (_matches.Count + 120 - 1) / 120);
		_currentPage = Mathf.Clamp(_currentPage, 0, num - 1);
		int num2 = _currentPage * 120;
		int num3 = Math.Min(num2 + 120, _matches.Count);
		for (int i = num2; i < num3; i++)
		{
			(string, string) tuple = _matches[i];
			int idx = _resultList.AddItem(tuple.Item1, GetFileTypeIcon(tuple.Item2));
			_resultList.SetItemMetadata(idx, tuple.Item2);
		}
		_resultCount.Text = $"{_matches.Count} 个结果 · 当前 {num3 - num2} 个";
		_pageLabel.Text = $"第 {_currentPage + 1} / {num} 页";
		_previousPageButton.Disabled = _currentPage <= 0;
		_nextPageButton.Disabled = _currentPage >= num - 1;
	}

	private void ChangePage(int direction)
	{
		_currentPage += direction;
		RenderResultPage();
		if (_resultList.ItemCount > 0)
		{
			_resultList.Select(0);
			_resultList.GrabFocus();
		}
	}

	private void OnSearchGuiInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey { Pressed: not false, Echo: false } inputEventKey && inputEventKey.Keycode == Key.Down && _resultList.ItemCount > 0)
		{
			_resultList.Select(0);
			_resultList.GrabFocus();
			_searchLine.AcceptEvent();
		}
	}

	private bool MatchesSelectedType(string path)
	{
		if (_selectedType == 0)
		{
			return true;
		}
		string text = Path.GetExtension(path)?.ToLowerInvariant() ?? "";
		return _selectedType switch
		{
			1 => text == ".cs", 
			2 => (text == ".tscn" || text == ".scn") ? true : false, 
			3 => (text == ".tres" || text == ".res") ? true : false, 
			_ => true, 
		};
	}

	private static Texture2D GetFileTypeIcon(string path)
	{
		string path2;
		switch (Path.GetExtension(path)?.ToLowerInvariant() ?? "")
		{
		case ".cs":
			path2 = "res://addons/ModEditor/Icons/Script.svg";
			break;
		case ".tscn":
		case ".scn":
			path2 = "res://addons/ModEditor/Icons/PackedScene.svg";
			break;
		case ".tres":
		case ".res":
			path2 = "res://addons/ModEditor/Icons/FileThumbnail.svg";
			break;
		default:
			path2 = "res://addons/ModEditor/Icons/File.svg";
			break;
		}
		return ResourceLoader.Load<Texture2D>(path2, null, ResourceLoader.CacheMode.Reuse);
	}

	private void OnConfirmed()
	{
		int[] selectedItems = _resultList.GetSelectedItems();
		if (selectedItems.Length != 0)
		{
			string text = _resultList.GetItemMetadata(selectedItems[0]).AsString();
			EmitSignal(SignalName.FileOpened, text);
			Hide();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTypeFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindTypeChips, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FilterItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CollectMatching, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "search", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderResultPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChangePage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSearchGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.MatchesSelectedType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFileTypeIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BuildTypeFilter && args.Count == 0)
		{
			BuildTypeFilter();
			ret = default;
			return true;
		}
		if (method == MethodName.BindTypeChips && args.Count == 0)
		{
			BindTypeChips();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectType && args.Count == 1)
		{
			SelectType(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.FilterItems && args.Count == 0)
		{
			FilterItems();
			ret = default;
			return true;
		}
		if (method == MethodName.CollectMatching && args.Count == 2)
		{
			CollectMatching(VariantUtils.ConvertTo<XWFileSystemDirectory>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderResultPage && args.Count == 0)
		{
			RenderResultPage();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangePage && args.Count == 1)
		{
			ChangePage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchGuiInput && args.Count == 1)
		{
			OnSearchGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MatchesSelectedType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesSelectedType(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetFileTypeIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetFileTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetFileTypeIcon(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.BuildTypeFilter)
		{
			return true;
		}
		if (method == MethodName.BindTypeChips)
		{
			return true;
		}
		if (method == MethodName.SelectType)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.FilterItems)
		{
			return true;
		}
		if (method == MethodName.CollectMatching)
		{
			return true;
		}
		if (method == MethodName.RenderResultPage)
		{
			return true;
		}
		if (method == MethodName.ChangePage)
		{
			return true;
		}
		if (method == MethodName.OnSearchGuiInput)
		{
			return true;
		}
		if (method == MethodName.MatchesSelectedType)
		{
			return true;
		}
		if (method == MethodName.GetFileTypeIcon)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._typeFilter)
		{
			_typeFilter = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._searchLine)
		{
			_searchLine = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._resultList)
		{
			_resultList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._resultCount)
		{
			_resultCount = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._typeChips)
		{
			_typeChips = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
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
		if (name == PropertyName._selectedType)
		{
			_selectedType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._currentPage)
		{
			_currentPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._typeFilter)
		{
			value = VariantUtils.CreateFrom(in _typeFilter);
			return true;
		}
		if (name == PropertyName._searchLine)
		{
			value = VariantUtils.CreateFrom(in _searchLine);
			return true;
		}
		if (name == PropertyName._resultList)
		{
			value = VariantUtils.CreateFrom(in _resultList);
			return true;
		}
		if (name == PropertyName._resultCount)
		{
			value = VariantUtils.CreateFrom(in _resultCount);
			return true;
		}
		if (name == PropertyName._typeChips)
		{
			GodotObject[] typeChips = _typeChips;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(typeChips);
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
		if (name == PropertyName._selectedType)
		{
			value = VariantUtils.CreateFrom(in _selectedType);
			return true;
		}
		if (name == PropertyName._currentPage)
		{
			value = VariantUtils.CreateFrom(in _currentPage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._typeFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._typeChips, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextPageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._typeFilter, Variant.From(in _typeFilter));
		info.AddProperty(PropertyName._searchLine, Variant.From(in _searchLine));
		info.AddProperty(PropertyName._resultList, Variant.From(in _resultList));
		info.AddProperty(PropertyName._resultCount, Variant.From(in _resultCount));
		StringName typeChips = PropertyName._typeChips;
		GodotObject[] typeChips2 = _typeChips;
		info.AddProperty(typeChips, Variant.CreateFrom(typeChips2));
		info.AddProperty(PropertyName._previousPageButton, Variant.From(in _previousPageButton));
		info.AddProperty(PropertyName._nextPageButton, Variant.From(in _nextPageButton));
		info.AddProperty(PropertyName._pageLabel, Variant.From(in _pageLabel));
		info.AddProperty(PropertyName._selectedType, Variant.From(in _selectedType));
		info.AddProperty(PropertyName._currentPage, Variant.From(in _currentPage));
		info.AddSignalEventDelegate(SignalName.FileOpened, backing_FileOpened);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._typeFilter, out var value))
		{
			_typeFilter = value.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._searchLine, out var value2))
		{
			_searchLine = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._resultList, out var value3))
		{
			_resultList = value3.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._resultCount, out var value4))
		{
			_resultCount = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._typeChips, out var value5))
		{
			_typeChips = value5.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._previousPageButton, out var value6))
		{
			_previousPageButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextPageButton, out var value7))
		{
			_nextPageButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pageLabel, out var value8))
		{
			_pageLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._selectedType, out var value9))
		{
			_selectedType = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._currentPage, out var value10))
		{
			_currentPage = value10.As<int>();
		}
		if (info.TryGetSignalEventDelegate<FileOpenedEventHandler>(SignalName.FileOpened, out var value11))
		{
			backing_FileOpened = value11;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.FileOpened, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalFileOpened(string path)
	{
		EmitSignal(SignalName.FileOpened, new ReadOnlySpan<Variant>((Variant)path));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.FileOpened && args.Count == 1)
		{
			backing_FileOpened?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.FileOpened)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
