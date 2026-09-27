using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWEditorSelection.cs")]
public class XWEditorSelection : RefCounted
{
	[Signal]
	public delegate void SelectionChangedEventHandler();

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SelectNode = "SelectNode";

		public static readonly StringName DeselectNode = "DeselectNode";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName IsSelected = "IsSelected";

		public static readonly StringName GetSelectedCount = "GetSelectedCount";

		public static readonly StringName CleanupInvalidNodes = "CleanupInvalidNodes";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName SelectionChanged = "SelectionChanged";
	}

	private readonly List<Node> _selectedNodes = new List<Node>();

	private SelectionChangedEventHandler backing_SelectionChanged;

	public event SelectionChangedEventHandler SelectionChanged
	{
		add
		{
			backing_SelectionChanged = (SelectionChangedEventHandler)Delegate.Combine(backing_SelectionChanged, value);
		}
		remove
		{
			backing_SelectionChanged = (SelectionChangedEventHandler)Delegate.Remove(backing_SelectionChanged, value);
		}
	}

	public void SelectNode(Node node)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			CleanupInvalidNodes();
			if (!_selectedNodes.Contains(node))
			{
				_selectedNodes.Add(node);
				EmitSignal(SignalName.SelectionChanged);
			}
		}
	}

	public void DeselectNode(Node node)
	{
		CleanupInvalidNodes();
		_selectedNodes.Remove(node);
		EmitSignal(SignalName.SelectionChanged);
	}

	public void Clear()
	{
		_selectedNodes.Clear();
		EmitSignal(SignalName.SelectionChanged);
	}

	public List<Node> GetSelectedNodes()
	{
		CleanupInvalidNodes();
		return new List<Node>(_selectedNodes);
	}

	public List<Node> GetTransformableSelectedNodes()
	{
		CleanupInvalidNodes();
		List<Node> list = new List<Node>();
		foreach (Node selectedNode in _selectedNodes)
		{
			if (selectedNode is Node3D || selectedNode is CanvasItem)
			{
				list.Add(selectedNode);
			}
		}
		return list;
	}

	public bool IsSelected(Node node)
	{
		CleanupInvalidNodes();
		return _selectedNodes.Contains(node);
	}

	public int GetSelectedCount()
	{
		CleanupInvalidNodes();
		return _selectedNodes.Count;
	}

	private void CleanupInvalidNodes()
	{
		for (int num = _selectedNodes.Count - 1; num >= 0; num--)
		{
			if (!GodotObject.IsInstanceValid(_selectedNodes[num]))
			{
				_selectedNodes.RemoveAt(num);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.SelectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.DeselectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSelected, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupInvalidNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SelectNode && args.Count == 1)
		{
			SelectNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeselectNode && args.Count == 1)
		{
			DeselectNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.IsSelected && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSelected(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSelectedCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSelectedCount());
			return true;
		}
		if (method == MethodName.CleanupInvalidNodes && args.Count == 0)
		{
			CleanupInvalidNodes();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SelectNode)
		{
			return true;
		}
		if (method == MethodName.DeselectNode)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.IsSelected)
		{
			return true;
		}
		if (method == MethodName.GetSelectedCount)
		{
			return true;
		}
		if (method == MethodName.CleanupInvalidNodes)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddSignalEventDelegate(SignalName.SelectionChanged, backing_SelectionChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetSignalEventDelegate<SelectionChangedEventHandler>(SignalName.SelectionChanged, out var value))
		{
			backing_SelectionChanged = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.SelectionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalSelectionChanged()
	{
		EmitSignal(SignalName.SelectionChanged, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.SelectionChanged && args.Count == 0)
		{
			backing_SelectionChanged?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.SelectionChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
