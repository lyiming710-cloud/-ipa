using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/Window/Selector/XWWindowSelector.cs")]
public class XWWindowSelector : ConfirmationDialog
{
	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnCloseRequested = "OnCloseRequested";

		public static readonly StringName OnConfirmed = "OnConfirmed";

		public static readonly StringName BuildTree = "BuildTree";

		public static readonly StringName OnItemSelected = "OnItemSelected";

		public static readonly StringName ShowDescription = "ShowDescription";

		public static readonly StringName OnSearchTextChanged = "OnSearchTextChanged";

		public static readonly StringName FilterTree = "FilterTree";

		public static readonly StringName CanFilterTreeItem = "CanFilterTreeItem";

		public static readonly StringName Search = "Search";

		public static readonly StringName AddToFavorites = "AddToFavorites";

		public static readonly StringName RemoveFromFavorites = "RemoveFromFavorites";

		public static readonly StringName ToggleFavorite = "ToggleFavorite";

		public static readonly StringName AddToRecent = "AddToRecent";

		public static readonly StringName SaveFavorites = "SaveFavorites";

		public static readonly StringName LoadFavorites = "LoadFavorites";

		public static readonly StringName SaveRecent = "SaveRecent";

		public static readonly StringName LoadRecent = "LoadRecent";

		public static readonly StringName BuildFavoritesPanel = "BuildFavoritesPanel";

		public static readonly StringName BuildRecentPanel = "BuildRecentPanel";

		public static readonly StringName OnFavoriteClicked = "OnFavoriteClicked";

		public static readonly StringName OnRecentClicked = "OnRecentClicked";

		public static readonly StringName SelectItemByTypeId = "SelectItemByTypeId";

		public static readonly StringName FindItemByTypeId = "FindItemByTypeId";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _tree = "_tree";

		public static readonly StringName _nameLabel = "_nameLabel";

		public static readonly StringName _describeLabel = "_describeLabel";

		public static readonly StringName _searchLineEdit = "_searchLineEdit";

		public static readonly StringName _favoritesContainer = "_favoritesContainer";

		public static readonly StringName _recentContainer = "_recentContainer";

		public static readonly StringName Root = "Root";

		public static readonly StringName AllCategories = "AllCategories";

		public static readonly StringName MaxRecentCount = "MaxRecentCount";

		public static readonly StringName FavoritesFilePath = "FavoritesFilePath";

		public static readonly StringName RecentFilePath = "RecentFilePath";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
	}

	protected Tree _tree;

	protected RichTextLabel _nameLabel;

	protected RichTextLabel _describeLabel;

	protected LineEdit _searchLineEdit;

	protected VBoxContainer _favoritesContainer;

	protected VBoxContainer _recentContainer;

	protected TreeItem Root;

	protected Dictionary AllCategories = new Dictionary();

	protected List<TreeItem> CurrentResult = new List<TreeItem>();

	protected List<StringName> FavoriteTypes = new List<StringName>();

	protected List<StringName> RecentTypes = new List<StringName>();

	protected int MaxRecentCount = 10;

	protected string FavoritesFilePath = "user://xweditor_favorites.cfg";

	protected string RecentFilePath = "user://xweditor_recent.cfg";

	public override void _Ready()
	{
		_tree = GetNode<Tree>("%Tree");
		_nameLabel = GetNode<RichTextLabel>("%NameLabel");
		_describeLabel = GetNode<RichTextLabel>("%DescribeLabel");
		_searchLineEdit = GetNode<LineEdit>("%SearchLineEdit");
		_favoritesContainer = GetNode<VBoxContainer>("%FavoritesContainer");
		_recentContainer = GetNode<VBoxContainer>("%RecentContainer");
		CloseRequested += OnCloseRequested;
		Confirmed += OnConfirmed;
		LoadFavorites();
		LoadRecent();
		BuildTree();
		BuildFavoritesPanel();
		BuildRecentPanel();
		_tree.ItemSelected += OnItemSelected;
		_tree.ItemActivated += OnConfirmed;
		_searchLineEdit.TextChanged += OnSearchTextChanged;
	}

	protected virtual void OnCloseRequested()
	{
		QueueFree();
	}

	protected virtual void OnConfirmed()
	{
		TreeItem selected = _tree.GetSelected();
		if (GodotObject.IsInstanceValid(selected))
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary = metadata.As<Dictionary>();
				if (dictionary.ContainsKey("type_id"))
				{
					AddToRecent(dictionary["type_id"].AsStringName());
				}
			}
		}
		QueueFree();
	}

	protected virtual void BuildTree()
	{
		_tree.Clear();
		Root = _tree.CreateItem();
	}

	protected virtual void OnItemSelected()
	{
		TreeItem selected = _tree.GetSelected();
		if (GodotObject.IsInstanceValid(selected))
		{
			Variant metadata = selected.GetMetadata(0);
			ShowDescription(metadata);
		}
	}

	protected virtual void ShowDescription(Variant meta)
	{
		if (meta.VariantType == Variant.Type.Nil)
		{
			_nameLabel.Text = "";
			_describeLabel.Text = "";
			return;
		}
		if (meta.VariantType == Variant.Type.Object)
		{
			GodotObject godotObject = meta.As<GodotObject>();
			_nameLabel.Text = godotObject.GetClass();
		}
		_describeLabel.Text = "";
	}

	protected virtual void OnSearchTextChanged(string text)
	{
		FilterTree(text);
	}

	protected void FilterTree(string query)
	{
		TreeItem treeItem = Root;
		if (string.IsNullOrEmpty(query))
		{
			while (treeItem != null)
			{
				treeItem.Visible = true;
				treeItem.Collapsed = false;
				treeItem = treeItem.GetNextInTree();
			}
			return;
		}
		string lowerQuery = query.ToLower();
		while (treeItem != null)
		{
			if (treeItem.Visible = CanFilterTreeItem(lowerQuery, treeItem))
			{
				TreeItem treeItem2 = treeItem;
				treeItem2.Collapsed = true;
				while (treeItem2 != null)
				{
					treeItem2.Visible = true;
					treeItem2 = treeItem2.GetParent();
				}
				treeItem.UncollapseTree();
			}
			treeItem = treeItem.GetNextInTree();
		}
	}

	protected virtual bool CanFilterTreeItem(string lowerQuery, TreeItem treeItem)
	{
		return treeItem.GetText(0).ToLower().Contains(lowerQuery);
	}

	public void Search(string text)
	{
		_searchLineEdit.Text = text;
		_searchLineEdit.EmitSignal(LineEdit.SignalName.TextChanged, text);
	}

	protected void AddToFavorites(StringName typeId)
	{
		if (!FavoriteTypes.Contains(typeId))
		{
			FavoriteTypes.Add(typeId);
			SaveFavorites();
			BuildFavoritesPanel();
		}
	}

	protected void RemoveFromFavorites(StringName typeId)
	{
		FavoriteTypes.Remove(typeId);
		SaveFavorites();
		BuildFavoritesPanel();
	}

	protected void ToggleFavorite(StringName typeId)
	{
		if (FavoriteTypes.Contains(typeId))
		{
			RemoveFromFavorites(typeId);
		}
		else
		{
			AddToFavorites(typeId);
		}
	}

	protected void AddToRecent(StringName typeId)
	{
		RecentTypes.Remove(typeId);
		RecentTypes.Insert(0, typeId);
		if (RecentTypes.Count > MaxRecentCount)
		{
			RecentTypes.RemoveRange(MaxRecentCount, RecentTypes.Count - MaxRecentCount);
		}
		SaveRecent();
		BuildRecentPanel();
	}

	protected void SaveFavorites()
	{
		ConfigFile configFile = new ConfigFile();
		Array array = new Array();
		foreach (StringName favoriteType in FavoriteTypes)
		{
			array.Add(favoriteType);
		}
		configFile.SetValue("favorites", "types", array);
		configFile.Save(FavoritesFilePath);
	}

	protected void LoadFavorites()
	{
		ConfigFile configFile = new ConfigFile();
		if (configFile.Load(FavoritesFilePath) != Error.Ok)
		{
			return;
		}
		Array array = configFile.GetValue("favorites", "types", new Array()).As<Array>();
		FavoriteTypes.Clear();
		foreach (Variant item in array)
		{
			FavoriteTypes.Add(item.AsStringName());
		}
	}

	protected void SaveRecent()
	{
		ConfigFile configFile = new ConfigFile();
		Array array = new Array();
		foreach (StringName recentType in RecentTypes)
		{
			array.Add(recentType);
		}
		configFile.SetValue("recent", "types", array);
		configFile.Save(RecentFilePath);
	}

	protected void LoadRecent()
	{
		ConfigFile configFile = new ConfigFile();
		if (configFile.Load(RecentFilePath) != Error.Ok)
		{
			return;
		}
		Array array = configFile.GetValue("recent", "types", new Array()).As<Array>();
		RecentTypes.Clear();
		foreach (Variant item in array)
		{
			RecentTypes.Add(item.AsStringName());
		}
	}

	protected void BuildFavoritesPanel()
	{
		if (!GodotObject.IsInstanceValid(_favoritesContainer))
		{
			return;
		}
		foreach (Node child in _favoritesContainer.GetChildren())
		{
			child.QueueFree();
		}
		if (FavoriteTypes.Count == 0)
		{
			Label label = new Label
			{
				Text = "无收藏"
			};
			label.AddThemeColorOverride("font_color", new Color(0.5f, 0.5f, 0.5f));
			_favoritesContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		foreach (StringName typeId in FavoriteTypes)
		{
			Button button = new Button
			{
				Text = typeId.ToString(),
				Flat = true,
				Alignment = HorizontalAlignment.Left
			};
			button.Pressed += () =>
			{
				OnFavoriteClicked(typeId);
			};
			_favoritesContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	protected void BuildRecentPanel()
	{
		if (!GodotObject.IsInstanceValid(_recentContainer))
		{
			return;
		}
		foreach (Node child in _recentContainer.GetChildren())
		{
			child.QueueFree();
		}
		if (RecentTypes.Count == 0)
		{
			Label label = new Label
			{
				Text = "无最近使用"
			};
			label.AddThemeColorOverride("font_color", new Color(0.5f, 0.5f, 0.5f));
			_recentContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		foreach (StringName typeId in RecentTypes)
		{
			Button button = new Button
			{
				Text = typeId.ToString(),
				Flat = true,
				Alignment = HorizontalAlignment.Left
			};
			button.Pressed += () =>
			{
				OnRecentClicked(typeId);
			};
			_recentContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void OnFavoriteClicked(StringName typeId)
	{
		SelectItemByTypeId(typeId);
	}

	private void OnRecentClicked(StringName typeId)
	{
		SelectItemByTypeId(typeId);
		AddToRecent(typeId);
	}

	private void SelectItemByTypeId(StringName typeId)
	{
		if (GodotObject.IsInstanceValid(Root))
		{
			TreeItem treeItem = FindItemByTypeId(Root, typeId);
			if (GodotObject.IsInstanceValid(treeItem))
			{
				treeItem.Select(0);
				_tree.EnsureCursorIsVisible();
			}
		}
	}

	private TreeItem FindItemByTypeId(TreeItem item, StringName typeId)
	{
		if (!GodotObject.IsInstanceValid(item))
		{
			return null;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary = metadata.As<Dictionary>();
			if (dictionary.ContainsKey("type_id") && dictionary["type_id"].AsStringName() == typeId)
			{
				return item;
			}
		}
		TreeItem treeItem = item.GetFirstChild();
		while (GodotObject.IsInstanceValid(treeItem))
		{
			TreeItem treeItem2 = FindItemByTypeId(treeItem, typeId);
			if (GodotObject.IsInstanceValid(treeItem2))
			{
				return treeItem2;
			}
			treeItem = treeItem.GetNext();
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCloseRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowDescription, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "meta", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSearchTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FilterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanFilterTreeItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "lowerQuery", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.Search, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddToFavorites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFromFavorites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToggleFavorite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddToRecent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFavorites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFavorites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveRecent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadRecent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildFavoritesPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildRecentPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFavoriteClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRecentClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectItemByTypeId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindItemByTypeId, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.OnCloseRequested && args.Count == 0)
		{
			OnCloseRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTree && args.Count == 0)
		{
			BuildTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemSelected && args.Count == 0)
		{
			OnItemSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowDescription && args.Count == 1)
		{
			ShowDescription(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchTextChanged && args.Count == 1)
		{
			OnSearchTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FilterTree && args.Count == 1)
		{
			FilterTree(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanFilterTreeItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFilterTreeItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		if (method == MethodName.Search && args.Count == 1)
		{
			Search(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddToFavorites && args.Count == 1)
		{
			AddToFavorites(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveFromFavorites && args.Count == 1)
		{
			RemoveFromFavorites(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleFavorite && args.Count == 1)
		{
			ToggleFavorite(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddToRecent && args.Count == 1)
		{
			AddToRecent(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFavorites && args.Count == 0)
		{
			SaveFavorites();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadFavorites && args.Count == 0)
		{
			LoadFavorites();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveRecent && args.Count == 0)
		{
			SaveRecent();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadRecent && args.Count == 0)
		{
			LoadRecent();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildFavoritesPanel && args.Count == 0)
		{
			BuildFavoritesPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRecentPanel && args.Count == 0)
		{
			BuildRecentPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFavoriteClicked && args.Count == 1)
		{
			OnFavoriteClicked(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRecentClicked && args.Count == 1)
		{
			OnRecentClicked(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectItemByTypeId && args.Count == 1)
		{
			SelectItemByTypeId(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindItemByTypeId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(FindItemByTypeId(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
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
		if (method == MethodName.OnCloseRequested)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		if (method == MethodName.BuildTree)
		{
			return true;
		}
		if (method == MethodName.OnItemSelected)
		{
			return true;
		}
		if (method == MethodName.ShowDescription)
		{
			return true;
		}
		if (method == MethodName.OnSearchTextChanged)
		{
			return true;
		}
		if (method == MethodName.FilterTree)
		{
			return true;
		}
		if (method == MethodName.CanFilterTreeItem)
		{
			return true;
		}
		if (method == MethodName.Search)
		{
			return true;
		}
		if (method == MethodName.AddToFavorites)
		{
			return true;
		}
		if (method == MethodName.RemoveFromFavorites)
		{
			return true;
		}
		if (method == MethodName.ToggleFavorite)
		{
			return true;
		}
		if (method == MethodName.AddToRecent)
		{
			return true;
		}
		if (method == MethodName.SaveFavorites)
		{
			return true;
		}
		if (method == MethodName.LoadFavorites)
		{
			return true;
		}
		if (method == MethodName.SaveRecent)
		{
			return true;
		}
		if (method == MethodName.LoadRecent)
		{
			return true;
		}
		if (method == MethodName.BuildFavoritesPanel)
		{
			return true;
		}
		if (method == MethodName.BuildRecentPanel)
		{
			return true;
		}
		if (method == MethodName.OnFavoriteClicked)
		{
			return true;
		}
		if (method == MethodName.OnRecentClicked)
		{
			return true;
		}
		if (method == MethodName.SelectItemByTypeId)
		{
			return true;
		}
		if (method == MethodName.FindItemByTypeId)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._tree)
		{
			_tree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._nameLabel)
		{
			_nameLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._describeLabel)
		{
			_describeLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._searchLineEdit)
		{
			_searchLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._favoritesContainer)
		{
			_favoritesContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._recentContainer)
		{
			_recentContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.Root)
		{
			Root = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName.AllCategories)
		{
			AllCategories = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.MaxRecentCount)
		{
			MaxRecentCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.FavoritesFilePath)
		{
			FavoritesFilePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RecentFilePath)
		{
			RecentFilePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._tree)
		{
			value = VariantUtils.CreateFrom(in _tree);
			return true;
		}
		if (name == PropertyName._nameLabel)
		{
			value = VariantUtils.CreateFrom(in _nameLabel);
			return true;
		}
		if (name == PropertyName._describeLabel)
		{
			value = VariantUtils.CreateFrom(in _describeLabel);
			return true;
		}
		if (name == PropertyName._searchLineEdit)
		{
			value = VariantUtils.CreateFrom(in _searchLineEdit);
			return true;
		}
		if (name == PropertyName._favoritesContainer)
		{
			value = VariantUtils.CreateFrom(in _favoritesContainer);
			return true;
		}
		if (name == PropertyName._recentContainer)
		{
			value = VariantUtils.CreateFrom(in _recentContainer);
			return true;
		}
		if (name == PropertyName.Root)
		{
			value = VariantUtils.CreateFrom(in Root);
			return true;
		}
		if (name == PropertyName.AllCategories)
		{
			value = VariantUtils.CreateFrom(in AllCategories);
			return true;
		}
		if (name == PropertyName.MaxRecentCount)
		{
			value = VariantUtils.CreateFrom(in MaxRecentCount);
			return true;
		}
		if (name == PropertyName.FavoritesFilePath)
		{
			value = VariantUtils.CreateFrom(in FavoritesFilePath);
			return true;
		}
		if (name == PropertyName.RecentFilePath)
		{
			value = VariantUtils.CreateFrom(in RecentFilePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._tree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._describeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._favoritesContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._recentContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.AllCategories, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.MaxRecentCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.FavoritesFilePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.RecentFilePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._tree, Variant.From(in _tree));
		info.AddProperty(PropertyName._nameLabel, Variant.From(in _nameLabel));
		info.AddProperty(PropertyName._describeLabel, Variant.From(in _describeLabel));
		info.AddProperty(PropertyName._searchLineEdit, Variant.From(in _searchLineEdit));
		info.AddProperty(PropertyName._favoritesContainer, Variant.From(in _favoritesContainer));
		info.AddProperty(PropertyName._recentContainer, Variant.From(in _recentContainer));
		info.AddProperty(PropertyName.Root, Variant.From(in Root));
		info.AddProperty(PropertyName.AllCategories, Variant.From(in AllCategories));
		info.AddProperty(PropertyName.MaxRecentCount, Variant.From(in MaxRecentCount));
		info.AddProperty(PropertyName.FavoritesFilePath, Variant.From(in FavoritesFilePath));
		info.AddProperty(PropertyName.RecentFilePath, Variant.From(in RecentFilePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._tree, out var value))
		{
			_tree = value.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._nameLabel, out var value2))
		{
			_nameLabel = value2.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._describeLabel, out var value3))
		{
			_describeLabel = value3.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._searchLineEdit, out var value4))
		{
			_searchLineEdit = value4.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._favoritesContainer, out var value5))
		{
			_favoritesContainer = value5.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._recentContainer, out var value6))
		{
			_recentContainer = value6.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.Root, out var value7))
		{
			Root = value7.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName.AllCategories, out var value8))
		{
			AllCategories = value8.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.MaxRecentCount, out var value9))
		{
			MaxRecentCount = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.FavoritesFilePath, out var value10))
		{
			FavoritesFilePath = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RecentFilePath, out var value11))
		{
			RecentFilePath = value11.As<string>();
		}
	}
}
