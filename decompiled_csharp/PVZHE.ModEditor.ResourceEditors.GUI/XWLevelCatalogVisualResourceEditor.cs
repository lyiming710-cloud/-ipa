using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWLevelCatalogVisualResourceEditor.cs")]
public class XWLevelCatalogVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName BuildWorkbench = "BuildWorkbench";

		public static readonly StringName BuildCatalogBrowser = "BuildCatalogBrowser";

		public static readonly StringName RefreshFromHistory = "RefreshFromHistory";

		public static readonly StringName NormalizeCatalogSelection = "NormalizeCatalogSelection";

		public static readonly StringName RefreshLists = "RefreshLists";

		public static readonly StringName BuildDetailFields = "BuildDetailFields";

		public static readonly StringName BuildChapterFields = "BuildChapterFields";

		public static readonly StringName BuildLevelFields = "BuildLevelFields";

		public static readonly StringName SetChapterResource = "SetChapterResource";

		public static readonly StringName SetLevelResource = "SetLevelResource";

		public static readonly StringName OpenNestedResource = "OpenNestedResource";

		public static readonly StringName AddChapter = "AddChapter";

		public static readonly StringName RemoveChapter = "RemoveChapter";

		public static readonly StringName MoveChapter = "MoveChapter";

		public static readonly StringName AddLevel = "AddLevel";

		public static readonly StringName RemoveLevel = "RemoveLevel";

		public static readonly StringName MoveLevel = "MoveLevel";

		public static readonly StringName CloneChapters = "CloneChapters";

		public static readonly StringName CloneLevels = "CloneLevels";

		public static readonly StringName SelectChapter = "SelectChapter";

		public static readonly StringName SelectLevel = "SelectLevel";

		public static readonly StringName RefreshPreviewAndLists = "RefreshPreviewAndLists";

		public static readonly StringName RefreshPreview = "RefreshPreview";

		public static readonly StringName OnEdited = "OnEdited";

		public static readonly StringName QueueRefreshFromHistory = "QueueRefreshFromHistory";

		public static readonly StringName DisposeBindings = "DisposeBindings";

		public static readonly StringName PreviewTexture = "PreviewTexture";

		public static readonly StringName Card = "Card";

		public static readonly StringName StyledPanel = "StyledPanel";

		public static readonly StringName Field = "Field";

		public static readonly StringName Number = "Number";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _catalog = "_catalog";

		public static readonly StringName _chapter = "_chapter";

		public static readonly StringName _level = "_level";

		public static readonly StringName _chapterList = "_chapterList";

		public static readonly StringName _levelList = "_levelList";

		public static readonly StringName _detailHost = "_detailHost";

		public static readonly StringName _backgroundPreview = "_backgroundPreview";

		public static readonly StringName _buildingPreview = "_buildingPreview";

		public static readonly StringName _chapterIconPreview = "_chapterIconPreview";

		public static readonly StringName _levelIconPreview = "_levelIconPreview";

		public static readonly StringName _chapterCardText = "_chapterCardText";

		public static readonly StringName _levelCardText = "_levelCardText";

		public static readonly StringName _runtimeSummary = "_runtimeSummary";

		public static readonly StringName _chapterIndex = "_chapterIndex";

		public static readonly StringName _levelIndex = "_levelIndex";

		public static readonly StringName _refreshQueued = "_refreshQueued";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private LevelCatalogConfig _catalog;

	private LevelChapterConfig _chapter;

	private LevelChooseConfig _level;

	private XWVisualPropertyBinding _rootBinding;

	private XWVisualPropertyBinding _fieldBinding;

	private ItemList _chapterList;

	private ItemList _levelList;

	private VBoxContainer _detailHost;

	private TextureRect _backgroundPreview;

	private TextureRect _buildingPreview;

	private TextureRect _chapterIconPreview;

	private TextureRect _levelIconPreview;

	private Label _chapterCardText;

	private Label _levelCardText;

	private Label _runtimeSummary;

	private int _chapterIndex = -1;

	private int _levelIndex = -1;

	private bool _refreshQueued;

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeBindings();
		if (CanvasGrid != null)
		{
			_catalog = CurrentResource as LevelCatalogConfig;
			_chapter = CurrentResource as LevelChapterConfig;
			_level = CurrentResource as LevelChooseConfig;
			if (_catalog != null || _chapter != null || _level != null)
			{
				CanvasGrid.Columns = 1;
				CanvasGrid.AddChild(BuildWorkbench(), forceReadableName: false, InternalMode.Disabled);
				RefreshFromHistory();
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		Type type = resource?.GetType();
		if (type != typeof(LevelCatalogConfig) && type != typeof(LevelChapterConfig) && type != typeof(LevelChooseConfig))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	public override void _ExitTree()
	{
		DisposeBindings();
		base._ExitTree();
	}

	private Control BuildWorkbench()
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 10);
		PanelContainer panelContainer = StyledPanel(new Color(0.035f, 0.11f, 0.16f));
		VBoxContainer vBoxContainer2 = new VBoxContainer();
		vBoxContainer2.AddChild(new Label
		{
			Text = "\ud83d\uddfa  关卡选择 · 游戏画面预览",
			ThemeTypeVariation = "HeaderLarge"
		}, forceReadableName: false, InternalMode.Disabled);
		Control control = new Control
		{
			CustomMinimumSize = new Vector2(0f, 230f),
			ClipContents = true
		};
		_backgroundPreview = PreviewTexture(control, TextureRect.StretchModeEnum.Scale, new Color(0.18f, 0.32f, 0.38f));
		_buildingPreview = PreviewTexture(control, TextureRect.StretchModeEnum.KeepAspectCentered, Colors.Transparent);
		_buildingPreview.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			AnchorLeft = 0.08f,
			AnchorRight = 0.92f,
			AnchorTop = 0.64f,
			AnchorBottom = 0.96f
		};
		hBoxContainer.AddThemeConstantOverride("separation", 14);
		_chapterIconPreview = new TextureRect
		{
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			CustomMinimumSize = new Vector2(100f, 70f)
		};
		_levelIconPreview = new TextureRect
		{
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			CustomMinimumSize = new Vector2(100f, 70f)
		};
		_chapterCardText = new Label
		{
			Text = "章节",
			HorizontalAlignment = HorizontalAlignment.Center
		};
		_levelCardText = new Label
		{
			Text = "关卡",
			HorizontalAlignment = HorizontalAlignment.Center
		};
		hBoxContainer.AddChild(Card(_chapterCardText, _chapterIconPreview), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = "➜",
			VerticalAlignment = VerticalAlignment.Center,
			ThemeTypeVariation = "HeaderLarge"
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(Card(_levelCardText, _levelIconPreview), forceReadableName: false, InternalMode.Disabled);
		control.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		_runtimeSummary = new Label
		{
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		vBoxContainer2.AddChild(_runtimeSummary, forceReadableName: false, InternalMode.Disabled);
		panelContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		if (_catalog != null)
		{
			vBoxContainer.AddChild(BuildCatalogBrowser(), forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			_detailHost = new VBoxContainer();
			vBoxContainer.AddChild(_detailHost, forceReadableName: false, InternalMode.Disabled);
		}
		return vBoxContainer;
	}

	private Control BuildCatalogBrowser()
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = "运行时目录键",
			CustomMinimumSize = new Vector2(150f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		LineEdit lineEdit = new LineEdit
		{
			Name = "CatalogKeyEdit",
			PlaceholderText = "Adventure"
		};
		lineEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(lineEdit, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HSplitContainer hSplitContainer = new HSplitContainer
		{
			CustomMinimumSize = new Vector2(0f, 520f),
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		VBoxContainer vBoxContainer2 = new VBoxContainer
		{
			CustomMinimumSize = new Vector2(330f, 0f)
		};
		vBoxContainer2.AddChild(new Label
		{
			Text = "章节",
			ThemeTypeVariation = "HeaderMedium"
		}, forceReadableName: false, InternalMode.Disabled);
		_chapterList = new ItemList
		{
			CustomMinimumSize = new Vector2(0f, 135f),
			SelectMode = ItemList.SelectModeEnum.Single
		};
		_chapterList.ItemSelected += (long index) =>
		{
			SelectChapter((int)index);
		};
		vBoxContainer2.AddChild(_chapterList, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(ActionRow(("＋章节", AddChapter), ("上移", () =>
		{
			MoveChapter(-1);
		}), ("下移", () =>
		{
			MoveChapter(1);
		}), ("删除", RemoveChapter)), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(new Label
		{
			Text = "关卡卡片",
			ThemeTypeVariation = "HeaderMedium"
		}, forceReadableName: false, InternalMode.Disabled);
		_levelList = new ItemList
		{
			CustomMinimumSize = new Vector2(0f, 180f),
			SelectMode = ItemList.SelectModeEnum.Single
		};
		_levelList.ItemSelected += (long index) =>
		{
			SelectLevel((int)index);
		};
		vBoxContainer2.AddChild(_levelList, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer2.AddChild(ActionRow(("＋关卡", AddLevel), ("上移", () =>
		{
			MoveLevel(-1);
		}), ("下移", () =>
		{
			MoveLevel(1);
		}), ("删除", RemoveLevel)), forceReadableName: false, InternalMode.Disabled);
		hSplitContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		ScrollContainer scrollContainer = new ScrollContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		_detailHost = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_detailHost.AddThemeConstantOverride("separation", 8);
		scrollContainer.AddChild(_detailHost, forceReadableName: false, InternalMode.Disabled);
		hSplitContainer.AddChild(scrollContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hSplitContainer, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	public void RefreshFromHistory()
	{
		_refreshQueued = false;
		_rootBinding?.Dispose();
		_rootBinding = null;
		if (_catalog != null)
		{
			_rootBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnEdited);
			LineEdit control = FindChild("CatalogKeyEdit", recursive: true, owned: false) as LineEdit;
			_rootBinding.BindText(control, _catalog, "catalogKey", RefreshPreview, this, "RefreshFromHistory");
			NormalizeCatalogSelection();
			RefreshLists();
		}
		BuildDetailFields();
		RefreshPreview();
	}

	private void NormalizeCatalogSelection()
	{
		if (_catalog.chapterList.Count == 0)
		{
			_chapterIndex = -1;
			_levelIndex = -1;
			_chapter = null;
			_level = null;
			return;
		}
		_chapterIndex = Mathf.Clamp((_chapterIndex >= 0) ? _chapterIndex : 0, 0, _catalog.chapterList.Count - 1);
		_chapter = _catalog.chapterList[_chapterIndex];
		if (!GodotObject.IsInstanceValid(_chapter) || _chapter.levelList.Count == 0)
		{
			_levelIndex = -1;
			_level = null;
		}
		else
		{
			_levelIndex = Mathf.Clamp((_levelIndex >= 0) ? _levelIndex : 0, 0, _chapter.levelList.Count - 1);
			_level = _chapter.levelList[_levelIndex];
		}
	}

	private void RefreshLists()
	{
		_chapterList.Clear();
		for (int i = 0; i < _catalog.chapterList.Count; i++)
		{
			LevelChapterConfig levelChapterConfig = _catalog.chapterList[i];
			_chapterList.AddItem($"{i + 1:00}  {(GodotObject.IsInstanceValid(levelChapterConfig) ? levelChapterConfig.chapterName : "空章节")}", levelChapterConfig?.unlockImage);
		}
		if (_chapterIndex >= 0)
		{
			_chapterList.Select(_chapterIndex);
		}
		_levelList.Clear();
		if (GodotObject.IsInstanceValid(_chapter))
		{
			for (int j = 0; j < _chapter.levelList.Count; j++)
			{
				LevelChooseConfig levelChooseConfig = _chapter.levelList[j];
				_levelList.AddItem($"{j + 1:00}  {(GodotObject.IsInstanceValid(levelChooseConfig) ? levelChooseConfig.saveKey : "空关卡")}", levelChooseConfig?.unlockImage);
			}
		}
		if (_levelIndex >= 0)
		{
			_levelList.Select(_levelIndex);
		}
	}

	private void BuildDetailFields()
	{
		_fieldBinding?.Dispose();
		_fieldBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnEdited);
		foreach (Node child in _detailHost.GetChildren())
		{
			child.QueueFree();
		}
		if (GodotObject.IsInstanceValid(_chapter))
		{
			BuildChapterFields();
		}
		if (GodotObject.IsInstanceValid(_level))
		{
			BuildLevelFields();
		}
		if (!GodotObject.IsInstanceValid(_chapter) && !GodotObject.IsInstanceValid(_level))
		{
			_detailHost.AddChild(new Label
			{
				Text = ((_catalog != null) ? "点击“＋章节”开始搭建关卡选择画面。" : "资源为空。")
			}, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void BuildChapterFields()
	{
		_detailHost.AddChild(new Label
		{
			Text = "章节画面",
			ThemeTypeVariation = "HeaderLarge"
		}, forceReadableName: false, InternalMode.Disabled);
		LineEdit control = new LineEdit
		{
			Name = "ChapterNameEdit"
		};
		LineEdit control2 = new LineEdit();
		SpinBox control3 = Number(0.0, 20.0, 1.0);
		CheckButton checkButton = new CheckButton
		{
			Text = "强制锁定章节"
		};
		_detailHost.AddChild(Field("章节名称", control), forceReadableName: false, InternalMode.Disabled);
		_detailHost.AddChild(Field("解锁前置键", control2), forceReadableName: false, InternalMode.Disabled);
		_detailHost.AddChild(Field("预开放关卡数", control3), forceReadableName: false, InternalMode.Disabled);
		_detailHost.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
		_fieldBinding.BindText(control, _chapter, "chapterName", RefreshPreviewAndLists, this, "RefreshFromHistory");
		_fieldBinding.BindText(control2, _chapter, "openKey", RefreshPreview, this, "RefreshFromHistory");
		_fieldBinding.BindNumber(control3, _chapter, "preOpen", RefreshPreview, this, "RefreshFromHistory");
		_fieldBinding.BindToggle(checkButton, _chapter, "locked", RefreshPreview, this, "RefreshFromHistory");
		AddPicker("章节解锁图", "Texture2D", _chapter.unlockImage, (Resource resource) =>
		{
			SetChapterResource("unlockImage", resource as Texture2D);
		});
		AddPicker("章节锁定图", "Texture2D", _chapter.lockImage, (Resource resource) =>
		{
			SetChapterResource("lockImage", resource as Texture2D);
		});
		AddPicker("场景背景", "Texture2D", _chapter.background, (Resource resource) =>
		{
			SetChapterResource("background", resource as Texture2D);
		});
		AddPicker("章节建筑", "Texture2D", _chapter.building, (Resource resource) =>
		{
			SetChapterResource("building", resource as Texture2D);
		});
	}

	private void BuildLevelFields()
	{
		_detailHost.AddChild(new HSeparator(), forceReadableName: false, InternalMode.Disabled);
		_detailHost.AddChild(new Label
		{
			Text = "关卡卡片",
			ThemeTypeVariation = "HeaderLarge"
		}, forceReadableName: false, InternalMode.Disabled);
		LineEdit control = new LineEdit();
		LineEdit control2 = new LineEdit
		{
			Name = "LevelSaveKeyEdit"
		};
		_detailHost.AddChild(Field("解锁前置键", control), forceReadableName: false, InternalMode.Disabled);
		_detailHost.AddChild(Field("存档关卡键", control2), forceReadableName: false, InternalMode.Disabled);
		_fieldBinding.BindText(control, _level, "openKey", RefreshPreview, this, "RefreshFromHistory");
		_fieldBinding.BindText(control2, _level, "saveKey", RefreshPreviewAndLists, this, "RefreshFromHistory");
		AddPicker("卡片图", "Texture2D", _level.unlockImage, (Resource resource) =>
		{
			SetLevelResource("unlockImage", resource as Texture2D);
		});
		AddPicker("普通难度", "TowerDefenseLevelBaseConfig", _level.normalLevel, (Resource resource) =>
		{
			SetLevelResource("normalLevel", resource as TowerDefenseLevelBaseConfig);
		});
		AddPicker("困难难度", "TowerDefenseLevelBaseConfig", _level.difficultLevel, (Resource resource) =>
		{
			SetLevelResource("difficultLevel", resource as TowerDefenseLevelBaseConfig);
		});
		AddPicker("终极难度", "TowerDefenseLevelBaseConfig", _level.ultimateLevel, (Resource resource) =>
		{
			SetLevelResource("ultimateLevel", resource as TowerDefenseLevelBaseConfig);
		});
	}

	private void AddPicker(string label, string type, Resource value, Action<Resource> changed)
	{
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Setup(type);
		xWResourcePicker.SetEditedResource(value);
		xWResourcePicker.ResourceChanged += (Resource resource) =>
		{
			changed(resource);
		};
		xWResourcePicker.ResourceSelected += OpenNestedResource;
		_detailHost.AddChild(Field(label, xWResourcePicker), forceReadableName: false, InternalMode.Disabled);
	}

	private void SetChapterResource(StringName property, Resource value)
	{
		_fieldBinding.SetValue(_chapter, property, value, $"更换章节 {property}", this, "RefreshFromHistory");
		RefreshFromHistory();
	}

	private void SetLevelResource(StringName property, Resource value)
	{
		_fieldBinding.SetValue(_level, property, value, $"更换关卡 {property}", this, "RefreshFromHistory");
		RefreshFromHistory();
	}

	private void OpenNestedResource(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface.Instance?.EditResource(resource);
		}
	}

	private void AddChapter()
	{
		Array<LevelChapterConfig> array = CloneChapters();
		array.Add(new LevelChapterConfig
		{
			chapterName = $"新章节 {array.Count + 1}"
		});
		_rootBinding.SetValue(_catalog, "chapterList", array, "添加关卡章节", this, "RefreshFromHistory");
		_chapterIndex = array.Count - 1;
		_levelIndex = -1;
		RefreshFromHistory();
	}

	private void RemoveChapter()
	{
		if (_chapterIndex >= 0)
		{
			Array<LevelChapterConfig> array = CloneChapters();
			array.RemoveAt(_chapterIndex);
			_rootBinding.SetValue(_catalog, "chapterList", array, "删除关卡章节", this, "RefreshFromHistory");
			_chapterIndex = Mathf.Min(_chapterIndex, array.Count - 1);
			_levelIndex = -1;
			RefreshFromHistory();
		}
	}

	private void MoveChapter(int direction)
	{
		int num = _chapterIndex + direction;
		if (_chapterIndex >= 0 && num >= 0 && num < _catalog.chapterList.Count)
		{
			Array<LevelChapterConfig> array = CloneChapters();
			LevelChapterConfig item = array[_chapterIndex];
			array.RemoveAt(_chapterIndex);
			array.Insert(num, item);
			_rootBinding.SetValue(_catalog, "chapterList", array, "移动关卡章节", this, "RefreshFromHistory");
			_chapterIndex = num;
			RefreshFromHistory();
		}
	}

	private void AddLevel()
	{
		if (GodotObject.IsInstanceValid(_chapter))
		{
			Array<LevelChooseConfig> array = CloneLevels();
			array.Add(new LevelChooseConfig
			{
				saveKey = $"Level{_chapterIndex + 1}_{array.Count + 1}"
			});
			_fieldBinding.SetValue(_chapter, "levelList", array, "添加关卡卡片", this, "RefreshFromHistory");
			_levelIndex = array.Count - 1;
			RefreshFromHistory();
		}
	}

	private void RemoveLevel()
	{
		if (GodotObject.IsInstanceValid(_chapter) && _levelIndex >= 0)
		{
			Array<LevelChooseConfig> array = CloneLevels();
			array.RemoveAt(_levelIndex);
			_fieldBinding.SetValue(_chapter, "levelList", array, "删除关卡卡片", this, "RefreshFromHistory");
			_levelIndex = Mathf.Min(_levelIndex, array.Count - 1);
			RefreshFromHistory();
		}
	}

	private void MoveLevel(int direction)
	{
		int num = _levelIndex + direction;
		if (GodotObject.IsInstanceValid(_chapter) && _levelIndex >= 0 && num >= 0 && num < _chapter.levelList.Count)
		{
			Array<LevelChooseConfig> array = CloneLevels();
			LevelChooseConfig item = array[_levelIndex];
			array.RemoveAt(_levelIndex);
			array.Insert(num, item);
			_fieldBinding.SetValue(_chapter, "levelList", array, "移动关卡卡片", this, "RefreshFromHistory");
			_levelIndex = num;
			RefreshFromHistory();
		}
	}

	private Array<LevelChapterConfig> CloneChapters()
	{
		Array<LevelChapterConfig> array = new Array<LevelChapterConfig>();
		foreach (LevelChapterConfig chapter in _catalog.chapterList)
		{
			array.Add(chapter);
		}
		return array;
	}

	private Array<LevelChooseConfig> CloneLevels()
	{
		Array<LevelChooseConfig> array = new Array<LevelChooseConfig>();
		foreach (LevelChooseConfig level in _chapter.levelList)
		{
			array.Add(level);
		}
		return array;
	}

	private void SelectChapter(int index)
	{
		_chapterIndex = index;
		_levelIndex = -1;
		RefreshFromHistory();
	}

	private void SelectLevel(int index)
	{
		_levelIndex = index;
		RefreshFromHistory();
	}

	private void RefreshPreviewAndLists()
	{
		RefreshLists();
		RefreshPreview();
	}

	private void RefreshPreview()
	{
		_backgroundPreview.Texture = _chapter?.background;
		_buildingPreview.Texture = _chapter?.building;
		_chapterIconPreview.Texture = _chapter?.unlockImage;
		_levelIconPreview.Texture = _level?.unlockImage;
		_chapterCardText.Text = _chapter?.chapterName ?? "未选择章节";
		_levelCardText.Text = _level?.saveKey ?? "未选择关卡";
		string value = _catalog?.catalogKey ?? "嵌套资源";
		string value2 = _chapter?.chapterName ?? "未选择章节";
		string value3 = _level?.saveKey ?? "未选择关卡";
		int value4 = _catalog?.chapterList.Count ?? (GodotObject.IsInstanceValid(_chapter) ? 1 : 0);
		int value5 = _chapter?.levelList.Count ?? (GodotObject.IsInstanceValid(_level) ? 1 : 0);
		_runtimeSummary.Text = $"运行键 {value}\u3000·\u3000章节 {value4}\u3000·\u3000当前 {value2} / {value3}\u3000·\u3000关卡 {value5}";
	}

	private void OnEdited(bool committed)
	{
		NotifyCurrentResourceEdited();
		RefreshPreview();
	}

	private void QueueRefreshFromHistory()
	{
		if (!_refreshQueued)
		{
			_refreshQueued = true;
			CallDeferred("RefreshFromHistory");
		}
	}

	private void DisposeBindings()
	{
		_rootBinding?.Dispose();
		_fieldBinding?.Dispose();
		_rootBinding = null;
		_fieldBinding = null;
		_catalog = null;
		_chapter = null;
		_level = null;
	}

	private static TextureRect PreviewTexture(Control parent, TextureRect.StretchModeEnum stretch, Color fallback)
	{
		TextureRect textureRect = new TextureRect
		{
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = stretch,
			Modulate = Colors.White
		};
		textureRect.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		parent.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
		ColorRect colorRect = new ColorRect
		{
			Color = fallback,
			MouseFilter = MouseFilterEnum.Ignore
		};
		colorRect.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		parent.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		parent.MoveChild(colorRect, 0);
		return textureRect;
	}

	private static PanelContainer Card(Label title, Control preview)
	{
		PanelContainer panelContainer = StyledPanel(new Color(0.12f, 0.2f, 0.14f, 0.96f));
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			CustomMinimumSize = new Vector2(140f, 110f)
		};
		vBoxContainer.AddChild(title, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(preview, forceReadableName: false, InternalMode.Disabled);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private static PanelContainer StyledPanel(Color color)
	{
		PanelContainer panelContainer = new PanelContainer();
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat
		{
			BgColor = color,
			CornerRadiusTopLeft = 9,
			CornerRadiusTopRight = 9,
			CornerRadiusBottomLeft = 9,
			CornerRadiusBottomRight = 9
		};
		float contentMarginLeft = (styleBoxFlat.ContentMarginRight = 12f);
		styleBoxFlat.ContentMarginLeft = contentMarginLeft;
		contentMarginLeft = (styleBoxFlat.ContentMarginBottom = 10f);
		styleBoxFlat.ContentMarginTop = contentMarginLeft;
		panelContainer.AddThemeStyleboxOverride("panel", styleBoxFlat);
		return panelContainer;
	}

	private static HBoxContainer Field(string label, Control control)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddChild(new Label
		{
			Text = label,
			CustomMinimumSize = new Vector2(145f, 0f)
		}, forceReadableName: false, InternalMode.Disabled);
		control.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static HBoxContainer ActionRow(params (string Text, Action Action)[] actions)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		for (int i = 0; i < actions.Length; i++)
		{
			(string Text, Action Action) tuple = actions[i];
			string item = tuple.Text;
			Action item2 = tuple.Action;
			Button button = new Button
			{
				Text = item
			};
			button.Pressed += item2;
			hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
		return hBoxContainer;
	}

	private static SpinBox Number(double min, double max, double step)
	{
		return new SpinBox
		{
			MinValue = min,
			MaxValue = max,
			Step = step,
			AllowGreater = true,
			AllowLesser = true
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(32)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildWorkbench, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildCatalogBrowser, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeCatalogSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildDetailFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildChapterFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildLevelFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetChapterResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetLevelResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenNestedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddChapter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveChapter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveChapter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneChapters, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloneLevels, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectChapter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPreviewAndLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueRefreshFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureRect"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "stretch", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Card, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "title", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false),
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.StyledPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Field, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.Number, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "min", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "max", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "step", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildWorkbench && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildWorkbench());
			return true;
		}
		if (method == MethodName.BuildCatalogBrowser && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(BuildCatalogBrowser());
			return true;
		}
		if (method == MethodName.RefreshFromHistory && args.Count == 0)
		{
			RefreshFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeCatalogSelection && args.Count == 0)
		{
			NormalizeCatalogSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshLists && args.Count == 0)
		{
			RefreshLists();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildDetailFields && args.Count == 0)
		{
			BuildDetailFields();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildChapterFields && args.Count == 0)
		{
			BuildChapterFields();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLevelFields && args.Count == 0)
		{
			BuildLevelFields();
			ret = default;
			return true;
		}
		if (method == MethodName.SetChapterResource && args.Count == 2)
		{
			SetChapterResource(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLevelResource && args.Count == 2)
		{
			SetLevelResource(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenNestedResource && args.Count == 1)
		{
			OpenNestedResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddChapter && args.Count == 0)
		{
			AddChapter();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveChapter && args.Count == 0)
		{
			RemoveChapter();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveChapter && args.Count == 1)
		{
			MoveChapter(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddLevel && args.Count == 0)
		{
			AddLevel();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveLevel && args.Count == 0)
		{
			RemoveLevel();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveLevel && args.Count == 1)
		{
			MoveLevel(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloneChapters && args.Count == 0)
		{
			Array<LevelChapterConfig> array = CloneChapters();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CloneLevels && args.Count == 0)
		{
			Array<LevelChooseConfig> array2 = CloneLevels();
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.SelectChapter && args.Count == 1)
		{
			SelectChapter(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectLevel && args.Count == 1)
		{
			SelectLevel(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPreviewAndLists && args.Count == 0)
		{
			RefreshPreviewAndLists();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPreview && args.Count == 0)
		{
			RefreshPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEdited && args.Count == 1)
		{
			OnEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueRefreshFromHistory && args.Count == 0)
		{
			QueueRefreshFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeBindings && args.Count == 0)
		{
			DisposeBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewTexture && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TextureRect>(PreviewTexture(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<TextureRect.StretchModeEnum>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.Card && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(Card(VariantUtils.ConvertTo<Label>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.StyledPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(StyledPanel(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.Field && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(Field(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.Number && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(Number(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PreviewTexture && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TextureRect>(PreviewTexture(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<TextureRect.StretchModeEnum>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.Card && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(Card(VariantUtils.ConvertTo<Label>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.StyledPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(StyledPanel(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.Field && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(Field(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.Number && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(Number(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BuildWorkbench)
		{
			return true;
		}
		if (method == MethodName.BuildCatalogBrowser)
		{
			return true;
		}
		if (method == MethodName.RefreshFromHistory)
		{
			return true;
		}
		if (method == MethodName.NormalizeCatalogSelection)
		{
			return true;
		}
		if (method == MethodName.RefreshLists)
		{
			return true;
		}
		if (method == MethodName.BuildDetailFields)
		{
			return true;
		}
		if (method == MethodName.BuildChapterFields)
		{
			return true;
		}
		if (method == MethodName.BuildLevelFields)
		{
			return true;
		}
		if (method == MethodName.SetChapterResource)
		{
			return true;
		}
		if (method == MethodName.SetLevelResource)
		{
			return true;
		}
		if (method == MethodName.OpenNestedResource)
		{
			return true;
		}
		if (method == MethodName.AddChapter)
		{
			return true;
		}
		if (method == MethodName.RemoveChapter)
		{
			return true;
		}
		if (method == MethodName.MoveChapter)
		{
			return true;
		}
		if (method == MethodName.AddLevel)
		{
			return true;
		}
		if (method == MethodName.RemoveLevel)
		{
			return true;
		}
		if (method == MethodName.MoveLevel)
		{
			return true;
		}
		if (method == MethodName.CloneChapters)
		{
			return true;
		}
		if (method == MethodName.CloneLevels)
		{
			return true;
		}
		if (method == MethodName.SelectChapter)
		{
			return true;
		}
		if (method == MethodName.SelectLevel)
		{
			return true;
		}
		if (method == MethodName.RefreshPreviewAndLists)
		{
			return true;
		}
		if (method == MethodName.RefreshPreview)
		{
			return true;
		}
		if (method == MethodName.OnEdited)
		{
			return true;
		}
		if (method == MethodName.QueueRefreshFromHistory)
		{
			return true;
		}
		if (method == MethodName.DisposeBindings)
		{
			return true;
		}
		if (method == MethodName.PreviewTexture)
		{
			return true;
		}
		if (method == MethodName.Card)
		{
			return true;
		}
		if (method == MethodName.StyledPanel)
		{
			return true;
		}
		if (method == MethodName.Field)
		{
			return true;
		}
		if (method == MethodName.Number)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._catalog)
		{
			_catalog = VariantUtils.ConvertTo<LevelCatalogConfig>(in value);
			return true;
		}
		if (name == PropertyName._chapter)
		{
			_chapter = VariantUtils.ConvertTo<LevelChapterConfig>(in value);
			return true;
		}
		if (name == PropertyName._level)
		{
			_level = VariantUtils.ConvertTo<LevelChooseConfig>(in value);
			return true;
		}
		if (name == PropertyName._chapterList)
		{
			_chapterList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._levelList)
		{
			_levelList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._detailHost)
		{
			_detailHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._backgroundPreview)
		{
			_backgroundPreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._buildingPreview)
		{
			_buildingPreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._chapterIconPreview)
		{
			_chapterIconPreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._levelIconPreview)
		{
			_levelIconPreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._chapterCardText)
		{
			_chapterCardText = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._levelCardText)
		{
			_levelCardText = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimeSummary)
		{
			_runtimeSummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._chapterIndex)
		{
			_chapterIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._levelIndex)
		{
			_levelIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._refreshQueued)
		{
			_refreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._catalog)
		{
			value = VariantUtils.CreateFrom(in _catalog);
			return true;
		}
		if (name == PropertyName._chapter)
		{
			value = VariantUtils.CreateFrom(in _chapter);
			return true;
		}
		if (name == PropertyName._level)
		{
			value = VariantUtils.CreateFrom(in _level);
			return true;
		}
		if (name == PropertyName._chapterList)
		{
			value = VariantUtils.CreateFrom(in _chapterList);
			return true;
		}
		if (name == PropertyName._levelList)
		{
			value = VariantUtils.CreateFrom(in _levelList);
			return true;
		}
		if (name == PropertyName._detailHost)
		{
			value = VariantUtils.CreateFrom(in _detailHost);
			return true;
		}
		if (name == PropertyName._backgroundPreview)
		{
			value = VariantUtils.CreateFrom(in _backgroundPreview);
			return true;
		}
		if (name == PropertyName._buildingPreview)
		{
			value = VariantUtils.CreateFrom(in _buildingPreview);
			return true;
		}
		if (name == PropertyName._chapterIconPreview)
		{
			value = VariantUtils.CreateFrom(in _chapterIconPreview);
			return true;
		}
		if (name == PropertyName._levelIconPreview)
		{
			value = VariantUtils.CreateFrom(in _levelIconPreview);
			return true;
		}
		if (name == PropertyName._chapterCardText)
		{
			value = VariantUtils.CreateFrom(in _chapterCardText);
			return true;
		}
		if (name == PropertyName._levelCardText)
		{
			value = VariantUtils.CreateFrom(in _levelCardText);
			return true;
		}
		if (name == PropertyName._runtimeSummary)
		{
			value = VariantUtils.CreateFrom(in _runtimeSummary);
			return true;
		}
		if (name == PropertyName._chapterIndex)
		{
			value = VariantUtils.CreateFrom(in _chapterIndex);
			return true;
		}
		if (name == PropertyName._levelIndex)
		{
			value = VariantUtils.CreateFrom(in _levelIndex);
			return true;
		}
		if (name == PropertyName._refreshQueued)
		{
			value = VariantUtils.CreateFrom(in _refreshQueued);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._catalog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chapter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._level, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chapterList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._detailHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._backgroundPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._buildingPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chapterIconPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelIconPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chapterCardText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._levelCardText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeSummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._chapterIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._levelIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._refreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._catalog, Variant.From(in _catalog));
		info.AddProperty(PropertyName._chapter, Variant.From(in _chapter));
		info.AddProperty(PropertyName._level, Variant.From(in _level));
		info.AddProperty(PropertyName._chapterList, Variant.From(in _chapterList));
		info.AddProperty(PropertyName._levelList, Variant.From(in _levelList));
		info.AddProperty(PropertyName._detailHost, Variant.From(in _detailHost));
		info.AddProperty(PropertyName._backgroundPreview, Variant.From(in _backgroundPreview));
		info.AddProperty(PropertyName._buildingPreview, Variant.From(in _buildingPreview));
		info.AddProperty(PropertyName._chapterIconPreview, Variant.From(in _chapterIconPreview));
		info.AddProperty(PropertyName._levelIconPreview, Variant.From(in _levelIconPreview));
		info.AddProperty(PropertyName._chapterCardText, Variant.From(in _chapterCardText));
		info.AddProperty(PropertyName._levelCardText, Variant.From(in _levelCardText));
		info.AddProperty(PropertyName._runtimeSummary, Variant.From(in _runtimeSummary));
		info.AddProperty(PropertyName._chapterIndex, Variant.From(in _chapterIndex));
		info.AddProperty(PropertyName._levelIndex, Variant.From(in _levelIndex));
		info.AddProperty(PropertyName._refreshQueued, Variant.From(in _refreshQueued));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._catalog, out var value))
		{
			_catalog = value.As<LevelCatalogConfig>();
		}
		if (info.TryGetProperty(PropertyName._chapter, out var value2))
		{
			_chapter = value2.As<LevelChapterConfig>();
		}
		if (info.TryGetProperty(PropertyName._level, out var value3))
		{
			_level = value3.As<LevelChooseConfig>();
		}
		if (info.TryGetProperty(PropertyName._chapterList, out var value4))
		{
			_chapterList = value4.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._levelList, out var value5))
		{
			_levelList = value5.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._detailHost, out var value6))
		{
			_detailHost = value6.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._backgroundPreview, out var value7))
		{
			_backgroundPreview = value7.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._buildingPreview, out var value8))
		{
			_buildingPreview = value8.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._chapterIconPreview, out var value9))
		{
			_chapterIconPreview = value9.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._levelIconPreview, out var value10))
		{
			_levelIconPreview = value10.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._chapterCardText, out var value11))
		{
			_chapterCardText = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._levelCardText, out var value12))
		{
			_levelCardText = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimeSummary, out var value13))
		{
			_runtimeSummary = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._chapterIndex, out var value14))
		{
			_chapterIndex = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._levelIndex, out var value15))
		{
			_levelIndex = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._refreshQueued, out var value16))
		{
			_refreshQueued = value16.As<bool>();
		}
	}
}
