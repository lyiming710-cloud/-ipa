using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorResourceChoiceBatchRuntimeProbe.cs")]
public class ModEditorResourceChoiceBatchRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InspectorUntouched = "InspectorUntouched";

		public static readonly StringName FindVisualCard = "FindVisualCard";

		public static readonly StringName VisualCardsHaveIcons = "VisualCardsHaveIcons";

		public static readonly StringName SelectCatalogChoice = "SelectCatalogChoice";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _history = "_history";

		public static readonly StringName _inspectorUntouched = "_inspectorUntouched";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWUndoRedoManager _history;

	private bool _inspectorUntouched = true;

	public override async void _Ready()
	{
		bool window = false;
		bool collectVisual = false;
		bool collectUndoRedo = false;
		bool collectResponsive = false;
		bool dropVisual = false;
		bool dropCategory = false;
		bool dropObject = false;
		bool dropHandler = false;
		bool dropResponsive = false;
		bool fallingVisual = false;
		bool fallingObject = false;
		bool weightItemObject = false;
		bool fallingResponsive = false;
		bool hiddenStopped = false;
		try
		{
			ModEditorManager modEditorManager = ModEditorManager.Instance;
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				modEditorManager = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Core/ModEditorManager.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<ModEditorManager>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(modEditorManager))
				{
					AddChild(modEditorManager, forceReadableName: false, InternalMode.Disabled);
				}
			}
			Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager could not be instantiated.");
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				Finish();
				return;
			}
			await WaitFrames(2);
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = true
			});
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = false
			});
			bool flag = await EnterEditorSurface(900);
			Require(flag, "F3 did not initialize the main ModEditor surface.");
			if (!flag)
			{
				Finish();
				return;
			}
			_history = XWEditorInterface.Instance?.GetUndoRedoManager();
			Require(GodotObject.IsInstanceValid(_history), "Global ModEditor UndoRedo manager is unavailable.");
			Window window2 = FindAncestorWindow(XWEditorInterface.Instance?.GetEditorPanel());
			if (GodotObject.IsInstanceValid(window2))
			{
				window2.Size = new Vector2I(820, 720);
				window = true;
			}
			CollectableConfig collectable = new CollectableConfig
			{
				ResourceName = "CollectableVisualProbe",
				saveKey = "collectable_visual_probe",
				config = new ShovelConfig
				{
					ResourceName = "OriginalShovelConfig"
				}
			};
			Resource originalCollectableConfig = collectable.config;
			XWCollectableVisualResourceEditor collectEditor = await OpenEditor<XWCollectableVisualResourceEditor>(collectable, "collectable_editor", "CollectableVisualEditorLayout");
			if (GodotObject.IsInstanceValid(collectEditor))
			{
				Control control = collectEditor.FindChild("CollectableVisualEditorLayout", recursive: true, owned: false) as Control;
				XWGameVisualChoiceCard xWGameVisualChoiceCard = FindVisualCard(collectEditor.FindChild("KindCards", recursive: true, owned: false) as HFlowContainer, "2");
				collectVisual = collectEditor.CollectableKindVisualCardCount == 5 && GodotObject.IsInstanceValid(xWGameVisualChoiceCard) && GodotObject.IsInstanceValid(xWGameVisualChoiceCard.Icon);
				collectResponsive = GodotObject.IsInstanceValid(control) && control.GetCombinedMinimumSize().X <= 500f && control.Size.X <= 820f;
				_history.ClearHistory();
				if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
				{
					xWGameVisualChoiceCard.EmitSignal(BaseButton.SignalName.Pressed);
					await WaitFrames(3);
					bool applied = collectable.config is AwardSettlementConfig && collectable.config.HasMeta("mod_collectable_kind") && collectable.config.GetMeta("mod_collectable_kind").AsString() == "Sun" && _history.HasUndo();
					bool undone = _history.Undo();
					await WaitFrames(3);
					undone = undone && collectable.config == originalCollectableConfig;
					bool redone = _history.Redo();
					await WaitFrames(3);
					redone = redone && collectable.config is AwardSettlementConfig;
					collectUndoRedo = applied & undone & redone;
				}
				_inspectorUntouched &= InspectorUntouched(collectEditor);
				collectEditor.Hide();
				await WaitFrames(3);
				bool collectHidden = !collectEditor.IsCollectablePreviewRendering && !collectEditor.IsProcessing();
				collectEditor.Show();
				XWEditorInterface.Instance.FocusPanel("collectable_editor");
				await WaitFrames(2);
				hiddenStopped = collectHidden;
			}
			Require(collectVisual, "Collectable kind choices are not five icon cards.");
			Require(collectUndoRedo, "Collectable kind card did not round-trip through UndoRedo.");
			Require(collectResponsive, "Collectable visual surface did not fit the 820px probe window.");
			DropItemConfig drop = new DropItemConfig
			{
				ResourceName = "DropItemVisualProbe",
				Name = "DropVisualProbe",
				Category = TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN,
				Id = ObjectManagerConfig.OBJECT.SUN,
				Value = 25,
				Handler = null
			};
			XWDropItemVisualResourceEditor dropEditor = await OpenEditor<XWDropItemVisualResourceEditor>(drop, "drop_item_editor", "DropItemVisualEditorLayout");
			if (GodotObject.IsInstanceValid(dropEditor))
			{
				Control control2 = dropEditor.FindChild("DropItemVisualEditorLayout", recursive: true, owned: false) as Control;
				HFlowContainer host = dropEditor.FindChild("CategoryCards", recursive: true, owned: false) as HFlowContainer;
				HFlowContainer host2 = dropEditor.FindChild("HandlerCards", recursive: true, owned: false) as HFlowContainer;
				_ = dropEditor.FindChild("DropItemObjectLibrary", recursive: true, owned: false) is XWObjectPoolVisualPicker;
				dropVisual = dropEditor.DropCategoryVisualCardCount == 3 && dropEditor.DropHandlerVisualCardCount >= 7 && dropEditor.HasObjectLibrarySearchAndPaging && VisualCardsHaveIcons(host) && VisualCardsHaveIcons(host2);
				dropResponsive = GodotObject.IsInstanceValid(control2) && control2.GetCombinedMinimumSize().X <= 500f && control2.Size.X <= 820f;
				_history.ClearHistory();
				XWGameVisualChoiceCard xWGameVisualChoiceCard2 = FindVisualCard(host, 1.ToString());
				if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard2))
				{
					xWGameVisualChoiceCard2.EmitSignal(BaseButton.SignalName.Pressed);
					await WaitFrames(2);
					bool collectHidden = drop.Category == TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN && _history.HasUndo();
					bool redone = _history.Undo();
					await WaitFrames(3);
					redone = redone && drop.Category == TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN;
					bool undone = _history.Redo();
					await WaitFrames(3);
					dropCategory = (collectHidden & redone & undone) && drop.Category == TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN;
				}
				XWObjectPoolVisualPicker objectLibrary = dropEditor.FindChild("DropItemObjectLibrary", recursive: true, owned: false) as XWObjectPoolVisualPicker;
				_history.ClearHistory();
				if (GodotObject.IsInstanceValid(objectLibrary))
				{
					objectLibrary.SetSearchText("COIN_GOLD");
					await WaitFrames(2);
					bool undone = SelectCatalogChoice(objectLibrary, "COIN_GOLD");
					await WaitFrames(2);
					bool redone = undone && drop.Id == ObjectManagerConfig.OBJECT.COIN_GOLD && _history.HasUndo();
					bool collectHidden = _history.Undo();
					await WaitFrames(3);
					collectHidden = collectHidden && drop.Id == ObjectManagerConfig.OBJECT.SUN;
					bool applied = _history.Redo();
					await WaitFrames(3);
					dropObject = (redone & collectHidden & applied) && drop.Id == ObjectManagerConfig.OBJECT.COIN_GOLD;
				}
				host2 = dropEditor.FindChild("HandlerCards", recursive: true, owned: false) as HFlowContainer;
				_history.ClearHistory();
				XWGameVisualChoiceCard xWGameVisualChoiceCard3 = FindVisualCard(host2, "2");
				if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard3))
				{
					xWGameVisualChoiceCard3.EmitSignal(BaseButton.SignalName.Pressed);
					await WaitFrames(2);
					bool applied = drop.Handler is CoinDropItemHandler && _history.HasUndo();
					bool collectHidden = _history.Undo();
					await WaitFrames(3);
					collectHidden = collectHidden && drop.Handler == null;
					bool redone = _history.Redo();
					await WaitFrames(3);
					dropHandler = (applied & collectHidden & redone) && drop.Handler is CoinDropItemHandler;
				}
				_inspectorUntouched &= InspectorUntouched(dropEditor);
				(dropEditor.FindChild("DropButton", recursive: true, owned: false) as Button)?.EmitSignal(BaseButton.SignalName.Pressed);
				await WaitFrames(1);
				dropEditor.Hide();
				await WaitFrames(4);
				hiddenStopped &= !dropEditor.IsDropItemPreviewRendering && !dropEditor.IsProcessing();
				dropEditor.Show();
				XWEditorInterface.Instance.FocusPanel("drop_item_editor");
				await WaitFrames(2);
			}
			Require(dropVisual, "DropItem category/object/handler visual selectors are incomplete.");
			Require(dropCategory, "DropItem category visual card did not round-trip through UndoRedo.");
			Require(dropObject, "DropItem searchable object catalog did not round-trip through UndoRedo.");
			Require(dropHandler, "DropItem handler visual card did not round-trip through UndoRedo.");
			Require(dropResponsive, "DropItem visual surface did not fit the 820px probe window.");
			FallingObjectWeightItemConfig firstWeight = new FallingObjectWeightItemConfig
			{
				ResourceName = "FirstWeight",
				item = ObjectManagerConfig.OBJECT.SUN,
				weight = 100
			};
			FallingObjectConfig fallingObjectConfig = new FallingObjectConfig
			{
				ResourceName = "FallingVisualProbe"
			};
			fallingObjectConfig.weightItem.Add(firstWeight);
			fallingObjectConfig.weightItem.Add(new FallingObjectWeightItemConfig
			{
				ResourceName = "CoinWeight",
				item = ObjectManagerConfig.OBJECT.COIN,
				weight = 50
			});
			XWFallingObjectVisualResourceEditor fallingEditor = await OpenEditor<XWFallingObjectVisualResourceEditor>(fallingObjectConfig, "falling_object_editor", "FallingObjectVisualEditorLayout");
			if (GodotObject.IsInstanceValid(fallingEditor))
			{
				Control control3 = fallingEditor.FindChild("FallingObjectVisualEditorLayout", recursive: true, owned: false) as Control;
				HFlowContainer host3 = fallingEditor.FindChild("CategoryGrid", recursive: true, owned: false) as HFlowContainer;
				_ = fallingEditor.FindChild("FallingObjectLibrary", recursive: true, owned: false) is XWObjectPoolVisualPicker;
				fallingVisual = fallingEditor.FallingCategoryVisualCardCount == 7 && fallingEditor.HasFallingObjectLibrarySearchAndPaging && VisualCardsHaveIcons(host3);
				fallingResponsive = GodotObject.IsInstanceValid(control3) && control3.GetCombinedMinimumSize().X <= 500f && control3.Size.X <= 820f;
				FindVisualCard(host3, "货币")?.EmitSignal(BaseButton.SignalName.Pressed);
				await WaitFrames(2);
				XWObjectPoolVisualPicker objectLibrary = fallingEditor.FindChild("FallingObjectLibrary", recursive: true, owned: false) as XWObjectPoolVisualPicker;
				_history.ClearHistory();
				if (GodotObject.IsInstanceValid(objectLibrary))
				{
					objectLibrary.SetSearchText("COIN_GOLD");
					await WaitFrames(2);
					bool redone = SelectCatalogChoice(objectLibrary, "COIN_GOLD");
					await WaitFrames(2);
					bool collectHidden = redone && firstWeight.item == ObjectManagerConfig.OBJECT.COIN_GOLD && _history.HasUndo();
					bool applied = _history.Undo();
					await WaitFrames(3);
					applied = applied && firstWeight.item == ObjectManagerConfig.OBJECT.SUN;
					bool undone = _history.Redo();
					await WaitFrames(3);
					fallingObject = (collectHidden & applied & undone) && firstWeight.item == ObjectManagerConfig.OBJECT.COIN_GOLD;
				}
				_inspectorUntouched &= InspectorUntouched(fallingEditor);
				(fallingEditor.FindChild("SimulateButton", recursive: true, owned: false) as Button)?.EmitSignal(BaseButton.SignalName.Pressed);
				await WaitFrames(1);
				int sequenceBeforeHide = fallingEditor.PreviewSequenceGeneration;
				fallingEditor.Hide();
				await WaitFrames(4);
				hiddenStopped &= !fallingEditor.IsFallingPreviewRendering && fallingEditor.PreviewSequenceGeneration > sequenceBeforeHide;
				fallingEditor.Show();
				XWEditorInterface.Instance.FocusPanel("falling_object_editor");
				await WaitFrames(2);
			}
			Require(fallingVisual, "FallingObject category cards or searchable object library are incomplete.");
			Require(fallingObject, "FallingObject weight object catalog did not round-trip through UndoRedo.");
			Require(fallingResponsive, "FallingObject visual surface did not fit the 820px probe window.");
			FallingObjectWeightItemConfig standaloneWeight = new FallingObjectWeightItemConfig
			{
				ResourceName = "StandaloneWeightProbe",
				item = ObjectManagerConfig.OBJECT.SUN,
				weight = 75
			};
			fallingEditor = await OpenEditor<XWFallingObjectVisualResourceEditor>(standaloneWeight, "falling_object_editor", "FallingObjectWeightItemPreview");
			if (GodotObject.IsInstanceValid(fallingEditor))
			{
				XWObjectPoolVisualPicker objectLibrary = fallingEditor.FindChild("WeightItemObjectLibrary", recursive: true, owned: false) as XWObjectPoolVisualPicker;
				_history.ClearHistory();
				if (GodotObject.IsInstanceValid(objectLibrary) && fallingEditor.HasWeightItemLibrarySearchAndPaging)
				{
					objectLibrary.SetSearchText("SUN_JALAPENO");
					await WaitFrames(2);
					bool undone = SelectCatalogChoice(objectLibrary, "SUN_JALAPENO");
					await WaitFrames(2);
					bool applied = undone && standaloneWeight.item == ObjectManagerConfig.OBJECT.SUN_JALAPENO && _history.HasUndo();
					bool collectHidden = _history.Undo();
					await WaitFrames(3);
					collectHidden = collectHidden && standaloneWeight.item == ObjectManagerConfig.OBJECT.SUN;
					bool redone = _history.Redo();
					await WaitFrames(3);
					weightItemObject = (applied & collectHidden & redone) && standaloneWeight.item == ObjectManagerConfig.OBJECT.SUN_JALAPENO;
				}
				_inspectorUntouched &= InspectorUntouched(fallingEditor);
				fallingEditor.Hide();
				await WaitFrames(3);
				hiddenStopped &= !fallingEditor.IsFallingPreviewRendering;
			}
			Require(weightItemObject, "Standalone weight-item object catalog did not round-trip through UndoRedo.");
			Require(hiddenStopped, "A hidden resource-choice editor kept processing or rendering its preview.");
			Require(_inspectorUntouched, "A target resource editor populated or relied on the raw Inspector.");
			GD.Print($"[MOD_EDITOR_RESOURCE_CHOICE_BATCH_PROBE] window={window} collectVisual={collectVisual} collectUndoRedo={collectUndoRedo} collectResponsive={collectResponsive} dropVisual={dropVisual} dropCategory={dropCategory} dropObject={dropObject} dropHandler={dropHandler} dropResponsive={dropResponsive} fallingVisual={fallingVisual} fallingObject={fallingObject} weightItemObject={weightItemObject} fallingResponsive={fallingResponsive} hiddenStopped={hiddenStopped} inspectorUntouched={_inspectorUntouched} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<T> OpenEditor<T>(Resource resource, string dockKey, string rootName) where T : XWGenericVisualResourceEditor
	{
		XWEditorInterface.Instance.EditResource(resource);
		XWEditorInterface.Instance.FocusPanel(dockKey);
		for (int i = 0; i < 360; i++)
		{
			T editor = XWEditorInterface.Instance.GetResourceEditor(dockKey) as T;
			if (GodotObject.IsInstanceValid(editor) && editor.IsVisibleInTree() && GodotObject.IsInstanceValid(editor.FindChild(rootName, recursive: true, owned: false)))
			{
				await WaitFrames(2);
				return editor;
			}
			await WaitFrames(1);
		}
		Require(condition: false, dockKey + " did not mount " + rootName + ".");
		return null;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				await WaitFrames(3);
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static bool InspectorUntouched(XWGenericVisualResourceEditor editor)
	{
		VBoxContainer vBoxContainer = editor?.FindChild("EmbeddedInspectorHost", recursive: true, owned: false) as VBoxContainer;
		if (GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0)
		{
			return editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
		}
		return false;
	}

	private static XWGameVisualChoiceCard FindVisualCard(Node host, string key)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return null;
		}
		foreach (Node child in host.GetChildren())
		{
			if (child is XWGameVisualChoiceCard xWGameVisualChoiceCard && xWGameVisualChoiceCard.ChoiceKey == key)
			{
				return xWGameVisualChoiceCard;
			}
		}
		return null;
	}

	private static bool VisualCardsHaveIcons(Node host)
	{
		if (!GodotObject.IsInstanceValid(host) || host.GetChildCount() == 0)
		{
			return false;
		}
		foreach (Node child in host.GetChildren())
		{
			if (!(child is XWGameVisualChoiceCard xWGameVisualChoiceCard) || !GodotObject.IsInstanceValid(xWGameVisualChoiceCard.Icon))
			{
				return false;
			}
		}
		return true;
	}

	private static bool SelectCatalogChoice(XWObjectPoolVisualPicker picker, string title)
	{
		ItemList itemList = picker?.FindChild("ChoiceList", recursive: true, owned: false) as ItemList;
		if (!GodotObject.IsInstanceValid(itemList))
		{
			return false;
		}
		for (int i = 0; i < itemList.ItemCount; i++)
		{
			if (string.Equals(itemList.GetItemText(i), title, StringComparison.OrdinalIgnoreCase))
			{
				itemList.Select(i);
				itemList.EmitSignal(ItemList.SignalName.ItemSelected, (long)i);
				return true;
			}
		}
		return false;
	}

	private static Window FindAncestorWindow(Node node)
	{
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 is Window result)
			{
				return result;
			}
			node2 = node2.GetParent();
		}
		return null;
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_RESOURCE_CHOICE_BATCH_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_RESOURCE_CHOICE_BATCH_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InspectorUntouched, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindVisualCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VisualCardsHaveIcons, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectCatalogChoice, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "picker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InspectorUntouched && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorUntouched(VariantUtils.ConvertTo<XWGenericVisualResourceEditor>(in args[0])));
			return true;
		}
		if (method == MethodName.FindVisualCard && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWGameVisualChoiceCard>(FindVisualCard(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.VisualCardsHaveIcons && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(VisualCardsHaveIcons(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectCatalogChoice && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectCatalogChoice(VariantUtils.ConvertTo<XWObjectPoolVisualPicker>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InspectorUntouched && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(InspectorUntouched(VariantUtils.ConvertTo<XWGenericVisualResourceEditor>(in args[0])));
			return true;
		}
		if (method == MethodName.FindVisualCard && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWGameVisualChoiceCard>(FindVisualCard(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.VisualCardsHaveIcons && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(VisualCardsHaveIcons(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectCatalogChoice && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectCatalogChoice(VariantUtils.ConvertTo<XWObjectPoolVisualPicker>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.InspectorUntouched)
		{
			return true;
		}
		if (method == MethodName.FindVisualCard)
		{
			return true;
		}
		if (method == MethodName.VisualCardsHaveIcons)
		{
			return true;
		}
		if (method == MethodName.SelectCatalogChoice)
		{
			return true;
		}
		if (method == MethodName.FindAncestorWindow)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._inspectorUntouched)
		{
			_inspectorUntouched = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
			return true;
		}
		if (name == PropertyName._inspectorUntouched)
		{
			value = VariantUtils.CreateFrom(in _inspectorUntouched);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._inspectorUntouched, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._history, Variant.From(in _history));
		info.AddProperty(PropertyName._inspectorUntouched, Variant.From(in _inspectorUntouched));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._history, out var value))
		{
			_history = value.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._inspectorUntouched, out var value2))
		{
			_inspectorUntouched = value2.As<bool>();
		}
	}
}
