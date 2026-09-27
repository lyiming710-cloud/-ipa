using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

[ScriptPath("res://Tests/ModEditorInspectorHintEditorsRuntimeProbe.cs")]
public class ModEditorInspectorHintEditorsRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _watch = Stopwatch.StartNew();

	public override async void _Ready()
	{
		_ = 6;
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
			XWInspector inspector = await WaitForInspector(900);
			Require(GodotObject.IsInstanceValid(inspector), "F3 did not mount the Inspector.");
			if (!GodotObject.IsInstanceValid(inspector))
			{
				Finish();
				return;
			}
			Node target = new Node
			{
				Name = "InspectorHintJumpTarget"
			};
			AddChild(target, forceReadableName: false, InternalMode.Disabled);
			InspectorHintProbeResource resource = new InspectorHintProbeResource
			{
				TargetObjectId = (long)target.GetInstanceId()
			};
			XWEditorInterface.Instance.InspectObject(resource);
			XWEditorInterface.Instance.FocusPanel("inspector");
			await WaitFrames(8);
			XWInspectorPropertyEditorTextEnum xWInspectorPropertyEditorTextEnum = FindNode<XWInspectorPropertyEditorTextEnum>(inspector);
			XWInspectorPropertyEditorLocale instance = FindNode<XWInspectorPropertyEditorLocale>(inspector);
			XWInspectorPropertyEditorObjectID objectEditor = FindNode<XWInspectorPropertyEditorObjectID>(inspector);
			bool textEnum = GodotObject.IsInstanceValid(xWInspectorPropertyEditorTextEnum);
			bool locale = GodotObject.IsInstanceValid(instance);
			Require(textEnum, "EnumSuggestion did not use the visual TextEnum editor.");
			Require(locale, "LocaleId did not use the visual locale selector.");
			Require(GodotObject.IsInstanceValid(objectEditor), "ObjectId did not use the object jump editor.");
			XWUndoRedoManager history = XWEditorInterface.Instance.GetUndoRedoManager();
			history.ClearHistory();
			OptionButton optionButton = xWInspectorPropertyEditorTextEnum?.FindChild("OptionButton", recursive: true, owned: false) as OptionButton;
			if (GodotObject.IsInstanceValid(optionButton))
			{
				optionButton.Select(1);
				optionButton.EmitSignal(OptionButton.SignalName.ItemSelected, 1L);
			}
			await WaitFrames(3);
			bool applied = resource.SuggestedCard == "sun" && history.HasUndo();
			bool undoCalled = history.Undo();
			await WaitFrames(2);
			bool undone = undoCalled && resource.SuggestedCard == "pea";
			bool redoCalled = history.Redo();
			await WaitFrames(2);
			bool flag = redoCalled && resource.SuggestedCard == "sun";
			bool undoRedo = applied & undone & flag;
			Require(undoRedo, "TextEnum selection did not round-trip through Inspector UndoRedo.");
			Button button = objectEditor?.FindChild("JumpButton", recursive: true, owned: false) as Button;
			if (GodotObject.IsInstanceValid(button))
			{
				button.EmitSignal(BaseButton.SignalName.Pressed);
			}
			await WaitFrames(3);
			bool flag2 = inspector.CurrentObject == target;
			Require(flag2, "ObjectId jump did not navigate the real Inspector to the target object.");
			bool value = FindAncestorWindow(inspector) != null;
			GD.Print($"[MOD_EDITOR_INSPECTOR_HINT_PROBE] window={value} textEnum={textEnum} locale={locale} objectJump={flag2} undoRedo={undoRedo} failures={_failures.Count} elapsedMs={_watch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<XWInspector> WaitForInspector(int frames)
	{
		for (int i = 0; i < frames; i++)
		{
			if (XWEditorInterface.Instance?.GetInspector() is XWInspector xWInspector && GodotObject.IsInstanceValid(xWInspector))
			{
				return xWInspector;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private static T FindNode<T>(Node root) where T : Node
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		if (root is T result)
		{
			return result;
		}
		foreach (Node child in root.GetChildren())
		{
			T val = FindNode<T>(child);
			if (GodotObject.IsInstanceValid(val))
			{
				return val;
			}
		}
		return null;
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
			GD.PrintErr("[MOD_EDITOR_INSPECTOR_HINT_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_INSPECTOR_HINT_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
