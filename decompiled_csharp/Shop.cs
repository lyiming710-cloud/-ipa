using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/Shop/Shop.cs")]
public class Shop : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName PageLabelFresh = "PageLabelFresh";

		public static readonly StringName PageFresh = "PageFresh";

		public static readonly StringName EditPreviewConfig = "EditPreviewConfig";

		public static readonly StringName RefreshEditorPreview = "RefreshEditorPreview";

		public static readonly StringName RefreshEditorPreviewPage = "RefreshEditorPreviewPage";

		public static readonly StringName GetPageCacheKey = "GetPageCacheKey";

		public static readonly StringName HideCachedPageRoots = "HideCachedPageRoots";

		public static readonly StringName GetOrCreatePageRoot = "GetOrCreatePageRoot";

		public static readonly StringName RefreshPageRoot = "RefreshPageRoot";

		public static readonly StringName PreButtonPressed = "PreButtonPressed";

		public static readonly StringName NextButtonpressed = "NextButtonpressed";

		public static readonly StringName MainMenuButtonPressed = "MainMenuButtonPressed";

		public static readonly StringName NpcTalk = "NpcTalk";

		public static readonly StringName ItemPressed = "ItemPressed";

		public static readonly StringName Sale = "Sale";

		public static readonly StringName CategoryButtonPressed = "CategoryButtonPressed";

		public static readonly StringName TotalButtonPressed = "TotalButtonPressed";

		public static readonly StringName ItemButtonPressed = "ItemButtonPressed";

		public static readonly StringName PacketButtonPressed = "PacketButtonPressed";

		public static readonly StringName CustomButtonPressed = "CustomButtonPressed";

		public static readonly StringName GardenButtomPressed = "GardenButtomPressed";

		public static readonly StringName TryLevelButtonPressed = "TryLevelButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName pageNum = "pageNum";

		public static readonly StringName currentPage = "currentPage";

		public static readonly StringName currentCategory = "currentCategory";

		public static readonly StringName editorPreviewMode = "editorPreviewMode";

		public static readonly StringName itemNode = "itemNode";

		public static readonly StringName pageLabel = "pageLabel";

		public static readonly StringName npcWeiWeiMi = "npcWeiWeiMi";

		public static readonly StringName animationPlayer = "animationPlayer";

		public static readonly StringName animationPlayerCategory = "animationPlayerCategory";

		public static readonly StringName _pageRefreshGeneration = "_pageRefreshGeneration";

		public static readonly StringName config = "config";

		public static readonly StringName _pageNum = "_pageNum";

		public static readonly StringName _currentPage = "_currentPage";

		public static readonly StringName talkTimer = "talkTimer";

		public static readonly StringName talkInterval = "talkInterval";

		public static readonly StringName categoryOpen = "categoryOpen";

		public static readonly StringName _currentCategory = "_currentCategory";

		public static readonly StringName currentPageList = "currentPageList";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool editorPreviewMode;

	private static PackedScene _shopItem;

	private Control itemNode;

	private Label pageLabel;

	private NpcBase npcWeiWeiMi;

	private AnimationPlayer animationPlayer;

	private AnimationPlayer animationPlayerCategory;

	private readonly System.Collections.Generic.Dictionary<string, Control> _pageCache = new System.Collections.Generic.Dictionary<string, Control>();

	private int _pageRefreshGeneration;

	private static readonly string[] IdleNpcTalk = new string[4] { "SHOP_NPC_TALK_IDLE_1", "SHOP_NPC_TALK_IDLE_2", "SHOP_NPC_TALK_IDLE_3", "SHOP_NPC_TALK_IDLE_4" };

	private static readonly Vector2[] ItemPos = new Vector2[8]
	{
		new Vector2(270f, 120f),
		new Vector2(345f, 120f),
		new Vector2(420f, 120f),
		new Vector2(495f, 120f),
		new Vector2(230f, 233f),
		new Vector2(305f, 233f),
		new Vector2(380f, 233f),
		new Vector2(455f, 233f)
	};

	public ShopConfig config;

	private int _pageNum = 1;

	private int _currentPage;

	public double talkTimer;

	public double talkInterval = 20.0;

	public bool categoryOpen = true;

	private string _currentCategory = "";

	public Array<ShopPageConfig> currentPageList = new Array<ShopPageConfig>();

	private static PackedScene SHOP_ITEM => _shopItem ?? (_shopItem = GD.Load<PackedScene>("res://Prefab/GUI/DialogBox/Shop/ShopItem/ShopItem.tscn"));

	public int pageNum
	{
		get
		{
			return _pageNum;
		}
		set
		{
			_pageNum = value;
			PageLabelFresh();
		}
	}

	public int currentPage
	{
		get
		{
			return _currentPage;
		}
		set
		{
			_currentPage = value;
			PageLabelFresh();
		}
	}

	public string currentCategory
	{
		get
		{
			return _currentCategory;
		}
		set
		{
			if (value != _currentCategory)
			{
				_currentCategory = value;
				currentPage = 0;
				currentPageList = config.GetPageTypeList(_currentCategory);
				pageNum = currentPageList.Count;
				PageFresh();
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		itemNode = GetNode<Control>("%ItemNode");
		pageLabel = GetNode<Label>("%PageLabel");
		npcWeiWeiMi = GetNode<NpcBase>("%NpcWeiWeiMi");
		Control node = GetNode<Control>("%NpcRenderMount");
		npcWeiWeiMi.sprite.forceLocalRender = true;
		npcWeiWeiMi.sprite.SetRenderClipControl(node);
		animationPlayer = GetNode<AnimationPlayer>("%AnimationPlayer");
		animationPlayerCategory = GetNode<AnimationPlayer>("%AnimationPlayerCategory");
		GetNode<TextureButton>("%NextButton").Pressed += NextButtonpressed;
		GetNode<TextureButton>("%PreButton").Pressed += PreButtonPressed;
		GetNode<TextureButton>("%MainMenuButton").Pressed += MainMenuButtonPressed;
		GetNode<TextureButton>("CategoryButton").Pressed += CategoryButtonPressed;
		GetNode<TextureButton>("CategoryButton/TotalButton").Pressed += TotalButtonPressed;
		GetNode<TextureButton>("CategoryButton/TotalButton/ItemButton").Pressed += ItemButtonPressed;
		GetNode<TextureButton>("CategoryButton/TotalButton/ItemButton/PacketButton").Pressed += PacketButtonPressed;
		GetNode<TextureButton>("CategoryButton/TotalButton/ItemButton/PacketButton/CustomButtom").Pressed += CustomButtonPressed;
		GetNode<TextureButton>("CategoryButton/TotalButton/ItemButton/PacketButton/CustomButtom/GardenButtom").Pressed += GardenButtomPressed;
		GetNode<TextureButton>("ButtonNode/TryLevelButton").Pressed += TryLevelButtonPressed;
		if (editorPreviewMode)
		{
			SetPhysicsProcess(enable: false);
			if (GodotObject.IsInstanceValid(config))
			{
				RefreshEditorPreview();
			}
		}
		else
		{
			TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(900f, 557f), still: true);
			config = (ShopConfig)ResourceManager.Instance.SHOPS["WWMShop"];
			config.Init();
			currentCategory = "Total";
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!editorPreviewMode)
		{
			talkTimer -= delta;
			if (talkTimer <= 0.0)
			{
				NpcTalk(IdleNpcTalk[GD.Randi() % (uint)IdleNpcTalk.Length]);
				talkTimer = talkInterval;
			}
		}
	}

	public void PageLabelFresh()
	{
		if (pageLabel != null)
		{
			pageLabel.Text = $"第{currentPage + 1}页，共{pageNum}页";
		}
	}

	public async void PageFresh()
	{
		if (editorPreviewMode)
		{
			RefreshEditorPreviewPage();
			return;
		}
		int generation = ++_pageRefreshGeneration;
		HideCachedPageRoots();
		animationPlayer.Play("PageChange");
		AudioManager.Instance.AudioPlay("ShopClose");
		await ToSignal(animationPlayer, AnimationMixer.SignalName.AnimationFinished);
		if (generation == _pageRefreshGeneration && currentPageList.Count != 0 && currentPage >= 0 && currentPage < currentPageList.Count)
		{
			ShopPageConfig page = currentPageList[currentPage];
			string pageCacheKey = GetPageCacheKey();
			Control orCreatePageRoot = GetOrCreatePageRoot(pageCacheKey, page);
			RefreshPageRoot(orCreatePageRoot);
			orCreatePageRoot.Visible = true;
		}
	}

	public void EditPreviewConfig(ShopConfig previewConfig)
	{
		editorPreviewMode = true;
		config = previewConfig;
		if (IsNodeReady())
		{
			RefreshEditorPreview();
		}
	}

	private void RefreshEditorPreview()
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			return;
		}
		foreach (Control value in _pageCache.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.QueueFree();
			}
		}
		_pageCache.Clear();
		_currentCategory = "Total";
		currentPageList = config.GetPageTypeList(_currentCategory);
		pageNum = Math.Max(1, currentPageList.Count);
		currentPage = 0;
		RefreshEditorPreviewPage();
	}

	private void RefreshEditorPreviewPage()
	{
		if (GodotObject.IsInstanceValid(config) && currentPageList.Count != 0 && currentPage >= 0 && currentPage < currentPageList.Count)
		{
			HideCachedPageRoots();
			ShopPageConfig page = currentPageList[currentPage];
			Control orCreatePageRoot = GetOrCreatePageRoot(GetPageCacheKey(), page);
			RefreshPageRoot(orCreatePageRoot);
			orCreatePageRoot.Visible = true;
		}
	}

	private string GetPageCacheKey()
	{
		return _currentCategory + ":" + currentPage;
	}

	private void HideCachedPageRoots()
	{
		foreach (Control value in _pageCache.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.Visible = false;
			}
		}
	}

	private Control GetOrCreatePageRoot(string pageCacheKey, ShopPageConfig page)
	{
		if (_pageCache.TryGetValue(pageCacheKey, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		Control control = new Control();
		control.Name = "Page_" + pageCacheKey.Replace(':', '_');
		control.Visible = false;
		itemNode.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		for (int i = 0; i < page.itemList.Count; i++)
		{
			ShopItemConfig shopItemConfig = page.itemList[i];
			ShopItem shopItem = (ShopItem)SHOP_ITEM.Instantiate(PackedScene.GenEditState.Disabled);
			shopItem.Position = ItemPos[i];
			shopItem.editorPreviewMode = editorPreviewMode;
			shopItem.OnTalk += NpcTalk;
			shopItem.OnPressedSignal += ItemPressed;
			control.AddChild(shopItem, forceReadableName: false, InternalMode.Disabled);
			shopItem.Init(shopItemConfig);
		}
		_pageCache[pageCacheKey] = control;
		return control;
	}

	private static void RefreshPageRoot(Control pageRoot)
	{
		foreach (Node child in pageRoot.GetChildren())
		{
			if (child is ShopItem shopItem)
			{
				shopItem.Refresh();
			}
		}
	}

	public void PreButtonPressed()
	{
		currentPage = (currentPage - 1 + pageNum) % pageNum;
		PageFresh();
	}

	public void NextButtonpressed()
	{
		currentPage = (currentPage + 1 + pageNum) % pageNum;
		PageFresh();
	}

	public void MainMenuButtonPressed()
	{
		TowerDefenseManager.Instance.coinBank.CallDeferred("StartHide");
		CloseDialog();
	}

	public void NpcTalk(string text)
	{
		npcWeiWeiMi.Talk(text, "Talk", "");
		talkTimer = talkInterval;
	}

	public async void ItemPressed(ShopItem item)
	{
		if (editorPreviewMode)
		{
			item?.HandleTalk();
			return;
		}
		if (item.cost > TowerDefenseManager.Instance.GetCoin())
		{
			await DialogCreate("ShopCantSale").WaitForClose();
			return;
		}
		DialogBoxShopSale dialogBoxShopSale = (DialogBoxShopSale)DialogCreate("ShopSale");
		dialogBoxShopSale.item = item;
		dialogBoxShopSale.OnSale += Sale;
		await dialogBoxShopSale.WaitForClose();
	}

	public void Sale(ShopItem item)
	{
		TowerDefenseManager.Instance.UseCoin(item.cost);
		item.Sale();
	}

	public void CategoryButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		categoryOpen = !categoryOpen;
		if (categoryOpen)
		{
			animationPlayerCategory.Play("Show");
		}
		else
		{
			animationPlayerCategory.Play("Hide");
		}
	}

	public void TotalButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		currentCategory = "Total";
	}

	public void ItemButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		currentCategory = "Item";
	}

	public void PacketButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		currentCategory = "Packet";
	}

	public void CustomButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		currentCategory = "Custom";
	}

	public void GardenButtomPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		currentCategory = "Garden";
	}

	public async void TryLevelButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("TryLevel", new Dictionary { { "openedFromShop", true } });
		TowerDefenseManager.Instance.coinBank.CallDeferred("StartHide");
		Visible = false;
		await dialogBoxBase.WaitForClose();
		Visible = true;
		TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(900f, 557f), still: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PageLabelFresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PageFresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EditPreviewConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "previewConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEditorPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEditorPreviewPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPageCacheKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideCachedPageRoots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetOrCreatePageRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "pageCacheKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "page", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPageRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "pageRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.PreButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextButtonpressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MainMenuButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NpcTalk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ItemPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.Sale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CategoryButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TotalButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ItemButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PacketButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CustomButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GardenButtomPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PageLabelFresh && args.Count == 0)
		{
			PageLabelFresh();
			ret = default;
			return true;
		}
		if (method == MethodName.PageFresh && args.Count == 0)
		{
			PageFresh();
			ret = default;
			return true;
		}
		if (method == MethodName.EditPreviewConfig && args.Count == 1)
		{
			EditPreviewConfig(VariantUtils.ConvertTo<ShopConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEditorPreview && args.Count == 0)
		{
			RefreshEditorPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEditorPreviewPage && args.Count == 0)
		{
			RefreshEditorPreviewPage();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPageCacheKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetPageCacheKey());
			return true;
		}
		if (method == MethodName.HideCachedPageRoots && args.Count == 0)
		{
			HideCachedPageRoots();
			ret = default;
			return true;
		}
		if (method == MethodName.GetOrCreatePageRoot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(GetOrCreatePageRoot(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<ShopPageConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshPageRoot && args.Count == 1)
		{
			RefreshPageRoot(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreButtonPressed && args.Count == 0)
		{
			PreButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NextButtonpressed && args.Count == 0)
		{
			NextButtonpressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MainMenuButtonPressed && args.Count == 0)
		{
			MainMenuButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NpcTalk && args.Count == 1)
		{
			NpcTalk(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ItemPressed && args.Count == 1)
		{
			ItemPressed(VariantUtils.ConvertTo<ShopItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Sale && args.Count == 1)
		{
			Sale(VariantUtils.ConvertTo<ShopItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CategoryButtonPressed && args.Count == 0)
		{
			CategoryButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.TotalButtonPressed && args.Count == 0)
		{
			TotalButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ItemButtonPressed && args.Count == 0)
		{
			ItemButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PacketButtonPressed && args.Count == 0)
		{
			PacketButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CustomButtonPressed && args.Count == 0)
		{
			CustomButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.GardenButtomPressed && args.Count == 0)
		{
			GardenButtomPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.TryLevelButtonPressed && args.Count == 0)
		{
			TryLevelButtonPressed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RefreshPageRoot && args.Count == 1)
		{
			RefreshPageRoot(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.PageLabelFresh)
		{
			return true;
		}
		if (method == MethodName.PageFresh)
		{
			return true;
		}
		if (method == MethodName.EditPreviewConfig)
		{
			return true;
		}
		if (method == MethodName.RefreshEditorPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshEditorPreviewPage)
		{
			return true;
		}
		if (method == MethodName.GetPageCacheKey)
		{
			return true;
		}
		if (method == MethodName.HideCachedPageRoots)
		{
			return true;
		}
		if (method == MethodName.GetOrCreatePageRoot)
		{
			return true;
		}
		if (method == MethodName.RefreshPageRoot)
		{
			return true;
		}
		if (method == MethodName.PreButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NextButtonpressed)
		{
			return true;
		}
		if (method == MethodName.MainMenuButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NpcTalk)
		{
			return true;
		}
		if (method == MethodName.ItemPressed)
		{
			return true;
		}
		if (method == MethodName.Sale)
		{
			return true;
		}
		if (method == MethodName.CategoryButtonPressed)
		{
			return true;
		}
		if (method == MethodName.TotalButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ItemButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PacketButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CustomButtonPressed)
		{
			return true;
		}
		if (method == MethodName.GardenButtomPressed)
		{
			return true;
		}
		if (method == MethodName.TryLevelButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pageNum)
		{
			pageNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentPage)
		{
			currentPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentCategory)
		{
			currentCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			editorPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.itemNode)
		{
			itemNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.pageLabel)
		{
			pageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.npcWeiWeiMi)
		{
			npcWeiWeiMi = VariantUtils.ConvertTo<NpcBase>(in value);
			return true;
		}
		if (name == PropertyName.animationPlayer)
		{
			animationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName.animationPlayerCategory)
		{
			animationPlayerCategory = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName._pageRefreshGeneration)
		{
			_pageRefreshGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<ShopConfig>(in value);
			return true;
		}
		if (name == PropertyName._pageNum)
		{
			_pageNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._currentPage)
		{
			_currentPage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.talkTimer)
		{
			talkTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.talkInterval)
		{
			talkInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.categoryOpen)
		{
			categoryOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._currentCategory)
		{
			_currentCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentPageList)
		{
			currentPageList = VariantUtils.ConvertToArray<ShopPageConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.pageNum)
		{
			from = pageNum;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.currentPage)
		{
			from = currentPage;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.currentCategory)
		{
			value = VariantUtils.CreateFrom<string>(currentCategory);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			value = VariantUtils.CreateFrom(in editorPreviewMode);
			return true;
		}
		if (name == PropertyName.itemNode)
		{
			value = VariantUtils.CreateFrom(in itemNode);
			return true;
		}
		if (name == PropertyName.pageLabel)
		{
			value = VariantUtils.CreateFrom(in pageLabel);
			return true;
		}
		if (name == PropertyName.npcWeiWeiMi)
		{
			value = VariantUtils.CreateFrom(in npcWeiWeiMi);
			return true;
		}
		if (name == PropertyName.animationPlayer)
		{
			value = VariantUtils.CreateFrom(in animationPlayer);
			return true;
		}
		if (name == PropertyName.animationPlayerCategory)
		{
			value = VariantUtils.CreateFrom(in animationPlayerCategory);
			return true;
		}
		if (name == PropertyName._pageRefreshGeneration)
		{
			value = VariantUtils.CreateFrom(in _pageRefreshGeneration);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._pageNum)
		{
			value = VariantUtils.CreateFrom(in _pageNum);
			return true;
		}
		if (name == PropertyName._currentPage)
		{
			value = VariantUtils.CreateFrom(in _currentPage);
			return true;
		}
		if (name == PropertyName.talkTimer)
		{
			value = VariantUtils.CreateFrom(in talkTimer);
			return true;
		}
		if (name == PropertyName.talkInterval)
		{
			value = VariantUtils.CreateFrom(in talkInterval);
			return true;
		}
		if (name == PropertyName.categoryOpen)
		{
			value = VariantUtils.CreateFrom(in categoryOpen);
			return true;
		}
		if (name == PropertyName._currentCategory)
		{
			value = VariantUtils.CreateFrom(in _currentCategory);
			return true;
		}
		if (name == PropertyName.currentPageList)
		{
			value = VariantUtils.CreateFromArray(currentPageList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorPreviewMode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.itemNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.npcWeiWeiMi, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.animationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.animationPlayerCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pageRefreshGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pageNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.pageNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentPage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.talkTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.talkInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.categoryOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentPageList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pageNum, Variant.From<int>(pageNum));
		info.AddProperty(PropertyName.currentPage, Variant.From<int>(currentPage));
		info.AddProperty(PropertyName.currentCategory, Variant.From<string>(currentCategory));
		info.AddProperty(PropertyName.editorPreviewMode, Variant.From(in editorPreviewMode));
		info.AddProperty(PropertyName.itemNode, Variant.From(in itemNode));
		info.AddProperty(PropertyName.pageLabel, Variant.From(in pageLabel));
		info.AddProperty(PropertyName.npcWeiWeiMi, Variant.From(in npcWeiWeiMi));
		info.AddProperty(PropertyName.animationPlayer, Variant.From(in animationPlayer));
		info.AddProperty(PropertyName.animationPlayerCategory, Variant.From(in animationPlayerCategory));
		info.AddProperty(PropertyName._pageRefreshGeneration, Variant.From(in _pageRefreshGeneration));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._pageNum, Variant.From(in _pageNum));
		info.AddProperty(PropertyName._currentPage, Variant.From(in _currentPage));
		info.AddProperty(PropertyName.talkTimer, Variant.From(in talkTimer));
		info.AddProperty(PropertyName.talkInterval, Variant.From(in talkInterval));
		info.AddProperty(PropertyName.categoryOpen, Variant.From(in categoryOpen));
		info.AddProperty(PropertyName._currentCategory, Variant.From(in _currentCategory));
		info.AddProperty(PropertyName.currentPageList, Variant.CreateFrom(currentPageList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pageNum, out var value))
		{
			pageNum = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentPage, out var value2))
		{
			currentPage = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentCategory, out var value3))
		{
			currentCategory = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.editorPreviewMode, out var value4))
		{
			editorPreviewMode = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.itemNode, out var value5))
		{
			itemNode = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.pageLabel, out var value6))
		{
			pageLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.npcWeiWeiMi, out var value7))
		{
			npcWeiWeiMi = value7.As<NpcBase>();
		}
		if (info.TryGetProperty(PropertyName.animationPlayer, out var value8))
		{
			animationPlayer = value8.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName.animationPlayerCategory, out var value9))
		{
			animationPlayerCategory = value9.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName._pageRefreshGeneration, out var value10))
		{
			_pageRefreshGeneration = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value11))
		{
			config = value11.As<ShopConfig>();
		}
		if (info.TryGetProperty(PropertyName._pageNum, out var value12))
		{
			_pageNum = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._currentPage, out var value13))
		{
			_currentPage = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.talkTimer, out var value14))
		{
			talkTimer = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.talkInterval, out var value15))
		{
			talkInterval = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.categoryOpen, out var value16))
		{
			categoryOpen = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._currentCategory, out var value17))
		{
			_currentCategory = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentPageList, out var value18))
		{
			currentPageList = value18.AsGodotArray<ShopPageConfig>();
		}
	}
}
