using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

[ScriptPath("res://Tests/ModEditorBlueprintNodeSelectorRuntimeProbe.cs")]
public class ModEditorBlueprintNodeSelectorRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FirstButton = "FirstButton";

		public static readonly StringName FirstGraph = "FirstGraph";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWBPEditor _editor;

	public override async void _Ready()
	{
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
			bool flag = await WaitForEditor(900);
			Require(flag, "F3 did not initialize the real blueprint editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			XWBPScript xWBPScript = XWBPScript.Create();
			xWBPScript.ResourceName = "BlueprintVisualSelectorProbe";
			_editor.Init(xWBPScript);
			await WaitFrames(4);
			XWBPGraphData graph = FirstGraph(_editor.BpScriptData);
			Require(graph != null, "Blueprint probe has no editable graph.");
			if (graph == null)
			{
				Finish();
				return;
			}
			XWInspector inspector = XWEditorInterface.Instance?.GetInspector() as XWInspector;
			Node sentinel = new Node
			{
				Name = "BlueprintSelectorInspectorSentinel"
			};
			AddChild(sentinel, forceReadableName: false, InternalMode.Disabled);
			XWEditorInterface.Instance?.InspectObject(sentinel);
			XWWindowBPNodeSelector selector = XWWindowBPNodeSelector.Create();
			selector.Editor = _editor;
			selector.CurrentGraph = graph;
			_editor.AddChild(selector, forceReadableName: false, InternalMode.Disabled);
			selector.PopupCentered();
			await WaitFrames(5);
			selector.Size = new Vector2I(720, 520);
			await WaitFrames(3);
			HFlowContainer cardGrid = selector.GetNodeOrNull<HFlowContainer>("%NodeCardGrid");
			HBoxContainer nodeOrNull = selector.GetNodeOrNull<HBoxContainer>("%CategoryChips");
			TabContainer tabs = selector.GetNodeOrNull<TabContainer>("%WorkspaceTabs");
			MarginContainer marginContainer = selector.FindChild("MarginContainer", recursive: true, owned: false) as MarginContainer;
			Label nodeOrNull2 = selector.GetNodeOrNull<Label>("%PageLabel");
			Button nodeOrNull3 = selector.GetNodeOrNull<Button>("%PreviousPageButton");
			Button nodeOrNull4 = selector.GetNodeOrNull<Button>("%NextPageButton");
			List<Button> initialCards = CollectCards(cardGrid);
			bool responsive = selector.Size.X <= 720 && selector.Size.Y <= 520 && GodotObject.IsInstanceValid(marginContainer) && marginContainer.Size.X <= (float)selector.Size.X && marginContainer.Size.Y <= (float)selector.Size.Y;
			bool visualCards = initialCards.Count > 0 && initialCards.Count <= 36 && GodotObject.IsInstanceValid(initialCards[0].Icon) && initialCards[0].Text.Contains('\n') && initialCards[0].GetThemeStylebox("normal") is StyleBoxFlat;
			bool categoryCards = GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull.GetChildCount() >= 3 && nodeOrNull.GetChild(0) is Button button && GodotObject.IsInstanceValid(button.Icon);
			bool pagination = GodotObject.IsInstanceValid(nodeOrNull2) && nodeOrNull2.Text.Contains("/") && GodotObject.IsInstanceValid(nodeOrNull3) && nodeOrNull3.Disabled && GodotObject.IsInstanceValid(nodeOrNull4);
			bool pagesReachable = GodotObject.IsInstanceValid(tabs) && tabs.GetTabCount() == 3;
			Require(responsive, "Blueprint node selector did not fit the 720x520 responsive probe size.");
			Require(visualCards, "Blueprint node selector did not render paged icon/color node cards.");
			Require(categoryCards, "Blueprint node selector did not render visual category chips.");
			Require(pagination, "Blueprint node selector pagination controls are incomplete.");
			Require(pagesReachable, "Blueprint visual, directory and collection pages are not all reachable.");
			selector.Search("分支");
			await WaitFrames(3);
			List<Button> list = CollectCards(cardGrid);
			bool search = list.Count > 0 && list.Count <= initialCards.Count;
			Require(search, "Blueprint visual cards did not respond to search.");
			Button button2 = ((list.Count > 0) ? list[0] : null);
			if (GodotObject.IsInstanceValid(button2))
			{
				button2.EmitSignal(Control.SignalName.GuiInput, new InputEventMouseButton
				{
					ButtonIndex = MouseButton.Right,
					Pressed = true,
					Position = new Vector2(8f, 8f)
				});
				await WaitFrames(3);
			}
			List<Button> list2 = CollectCards(cardGrid);
			bool favorite = false;
			foreach (Button item in list2)
			{
				if (item.Text.StartsWith("★", StringComparison.Ordinal))
				{
					favorite = true;
					break;
				}
			}
			Require(favorite, "Right-click did not visually favorite a blueprint node card.");
			button2 = ((list2.Count > 0) ? list2[0] : null);
			if (GodotObject.IsInstanceValid(button2))
			{
				button2.EmitSignal(BaseButton.SignalName.Pressed);
			}
			await WaitFrames(2);
			RichTextLabel nodeOrNull5 = selector.GetNodeOrNull<RichTextLabel>("%NameLabel");
			bool selected = !selector.GetOkButton().Disabled && GodotObject.IsInstanceValid(nodeOrNull5) && !string.IsNullOrWhiteSpace(nodeOrNull5.Text);
			Require(selected, "Visual node card selection did not populate the direct description or enable Add Node.");
			tabs.CurrentTab = 1;
			await WaitFrames(1);
			Tree tree = selector.GetNodeOrNull<Tree>("%Tree");
			bool directory = GodotObject.IsInstanceValid(tree) && tree.IsVisibleInTree();
			tabs.CurrentTab = 2;
			await WaitFrames(1);
			VBoxContainer nodeOrNull6 = selector.GetNodeOrNull<VBoxContainer>("%FavoritesContainer");
			bool collections = GodotObject.IsInstanceValid(nodeOrNull6) && nodeOrNull6.IsVisibleInTree();
			Button button3 = FirstButton(nodeOrNull6);
			string favoriteTypeId = ((GodotObject.IsInstanceValid(button3) && button3.HasMeta("xw_bp_node_type_id")) ? button3.GetMeta("xw_bp_node_type_id").AsString() : "");
			if (GodotObject.IsInstanceValid(tree))
			{
				tree.DeselectAll();
			}
			if (GodotObject.IsInstanceValid(button3))
			{
				button3.EmitSignal(BaseButton.SignalName.Pressed);
			}
			await WaitFrames(2);
			TreeItem treeItem = (GodotObject.IsInstanceValid(tree) ? tree.GetSelected() : null);
			Variant variant = (GodotObject.IsInstanceValid(treeItem) ? treeItem.GetMetadata(0) : default(Variant));
			int num;
			if (variant.VariantType == Variant.Type.Object)
			{
				XWBPNodeType xWBPNodeType = variant.As<XWBPNodeType>();
				if (xWBPNodeType != null)
				{
					num = (string.Equals(xWBPNodeType.TypeId.ToString(), favoriteTypeId, StringComparison.Ordinal) ? 1 : 0);
					goto IL_0c69;
				}
			}
			num = 0;
			goto IL_0c69;
			IL_0c69:
			bool collectionSelect = (byte)num != 0;
			Require(collectionSelect, "Favorite collection card did not select its object-backed blueprint node type.");
			tabs.CurrentTab = 0;
			Require(directory & collections, "Directory or favorite/recent fallback page is not reachable.");
			selector.Hide();
			await WaitFrames(2);
			bool hiddenIdle = !selector.IsProcessing() && !selector.IsPhysicsProcessing();
			selector.Show();
			await WaitFrames(2);
			Require(hiddenIdle, "Hidden blueprint selector still requested frame or physics processing.");
			XWBPNodeType chosen = null;
			int nodeCountBefore = graph.Nodes.Count;
			selector.NodeTypeSelected += (XWBPNodeType nodeType) =>
			{
				chosen = nodeType;
				_editor.GetGraphEditor().AddNode(nodeType, new Vector2(240f, 160f));
			};
			selector.EmitSignal(AcceptDialog.SignalName.Confirmed);
			await WaitFrames(5);
			bool flag2 = chosen != null && graph.Nodes.Count == nodeCountBefore + 1;
			bool flag3 = inspector == null || inspector.CurrentObject == sentinel;
			Require(flag2, "Confirming a visual node card did not create a node through the real GraphEdit workflow.");
			Require(flag3, "Blueprint node selector replaced the raw Inspector object.");
			GD.Print($"[MOD_EDITOR_BLUEPRINT_NODE_SELECTOR_PROBE] window=True responsive={responsive} visualCards={visualCards} categoryCards={categoryCards} pagination={pagination} pagesReachable={pagesReachable} search={search} favorite={favorite} selected={selected} collectionSelect={collectionSelect} directory={directory} collections={collections} hiddenIdle={hiddenIdle} created={flag2} inspectorUntouched={flag3} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static List<Button> CollectCards(HFlowContainer grid)
	{
		List<Button> list = new List<Button>();
		if (!GodotObject.IsInstanceValid(grid))
		{
			return list;
		}
		foreach (Node child in grid.GetChildren())
		{
			if (child is Button button && GodotObject.IsInstanceValid(button))
			{
				list.Add(button);
			}
		}
		return list;
	}

	private static Button FirstButton(Node parent)
	{
		if (!GodotObject.IsInstanceValid(parent))
		{
			return null;
		}
		foreach (Node child in parent.GetChildren())
		{
			if (child is Button button && GodotObject.IsInstanceValid(button))
			{
				return button;
			}
		}
		return null;
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWEditorInterface.Instance?.GetBlueprintEditor() is XWBPEditor xWBPEditor && GodotObject.IsInstanceValid(xWBPEditor) && xWBPEditor.IsInsideTree())
			{
				_editor = xWBPEditor;
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private static XWBPGraphData FirstGraph(XWBPScriptData data)
	{
		if (data == null)
		{
			return null;
		}
		using (Dictionary<int, XWBPGraphData>.ValueCollection.Enumerator enumerator = data.Graphs.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return null;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_BLUEPRINT_NODE_SELECTOR_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_BLUEPRINT_NODE_SELECTOR_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FirstButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FirstGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
		if (method == MethodName.FirstButton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Button>(FirstButton(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(FirstGraph(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
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
		if (method == MethodName.FirstButton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Button>(FirstButton(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(FirstGraph(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
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
		if (method == MethodName.FirstButton)
		{
			return true;
		}
		if (method == MethodName.FirstGraph)
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
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWBPEditor>();
		}
	}
}
