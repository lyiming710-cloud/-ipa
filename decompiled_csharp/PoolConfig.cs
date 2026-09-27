using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/ObjectManager/PoolConfig.cs")]
public class PoolConfig : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName GetPopCallableSN = "GetPopCallableSN";

		public static readonly StringName GetPushCallableSN = "GetPushCallableSN";

		public static readonly StringName Push = "Push";

		public static readonly StringName DeferredPushBatch = "DeferredPushBatch";

		public static readonly StringName Pop = "Pop";

		public static readonly StringName DispatchPopLifecycle = "DispatchPopLifecycle";

		public static readonly StringName DispatchPushLifecycle = "DispatchPushLifecycle";

		public static readonly StringName Maintenance = "Maintenance";

		public static readonly StringName GetScene = "GetScene";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName ReleaseImmediately = "ReleaseImmediately";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName scene = "scene";

		public static readonly StringName scenePath = "scenePath";

		public static readonly StringName maxNum = "maxNum";

		public static readonly StringName idleRetainNum = "idleRetainNum";

		public static readonly StringName idleTrimFrames = "idleTrimFrames";

		public static readonly StringName popCallable = "popCallable";

		public static readonly StringName pushCallable = "pushCallable";

		public static readonly StringName _popCallableSN = "_popCallableSN";

		public static readonly StringName _pushCallableSN = "_pushCallableSN";

		public static readonly StringName _deferredPushScheduled = "_deferredPushScheduled";

		public static readonly StringName _lastTouchedFrame = "_lastTouchedFrame";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	internal static readonly StringName RefreshCallbackName = new StringName("Refresh");

	internal static readonly StringName RecycleCallbackName = new StringName("Recycle");

	[Export(PropertyHint.None, "")]
	public PackedScene scene;

	[Export(PropertyHint.None, "")]
	public string scenePath = "";

	[Export(PropertyHint.None, "")]
	public int maxNum = 100;

	[Export(PropertyHint.None, "")]
	public int idleRetainNum = 16;

	[Export(PropertyHint.None, "")]
	public ulong idleTrimFrames = 600uL;

	[Export(PropertyHint.None, "")]
	public string popCallable = "";

	[Export(PropertyHint.None, "")]
	public string pushCallable = "";

	public List<Node> stack = new List<Node>();

	private StringName _popCallableSN;

	private StringName _pushCallableSN;

	private readonly List<Node> _pendingPush = new List<Node>();

	private bool _deferredPushScheduled;

	private ulong _lastTouchedFrame;

	private StringName GetPopCallableSN()
	{
		if (_popCallableSN == null && popCallable != "")
		{
			_popCallableSN = new StringName(popCallable);
		}
		return _popCallableSN;
	}

	private StringName GetPushCallableSN()
	{
		if (_pushCallableSN == null && pushCallable != "")
		{
			_pushCallableSN = new StringName(pushCallable);
		}
		return _pushCallableSN;
	}

	public void Push(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(node.GetParent()))
		{
			node.QueueFree();
			return;
		}
		StringName pushCallableSN = GetPushCallableSN();
		DispatchPushLifecycle(node, pushCallableSN);
		_lastTouchedFrame = Engine.GetPhysicsFrames();
		_pendingPush.Add(node);
		if (!_deferredPushScheduled)
		{
			_deferredPushScheduled = true;
			CallDeferred("DeferredPushBatch");
		}
	}

	private void DeferredPushBatch()
	{
		for (int i = 0; i < _pendingPush.Count; i++)
		{
			Node node = _pendingPush[i];
			if (!GodotObject.IsInstanceValid(node))
			{
				continue;
			}
			Node parent = node.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(node);
				if (maxNum == -1 || stack.Count < maxNum)
				{
					stack.Add(node);
				}
				else
				{
					node.QueueFree();
				}
			}
		}
		_pendingPush.Clear();
		_deferredPushScheduled = false;
	}

	public Node Pop(Node parent)
	{
		_lastTouchedFrame = Engine.GetPhysicsFrames();
		Node node;
		if (stack.Count > 0)
		{
			int index = stack.Count - 1;
			node = stack[index];
			stack.RemoveAt(index);
		}
		else
		{
			PackedScene packedScene = GetScene();
			if (packedScene == null)
			{
				GD.PushError("Object pool scene is missing: " + scenePath);
				return null;
			}
			node = packedScene.Instantiate<Node>(PackedScene.GenEditState.Disabled);
		}
		Node parent2 = node.GetParent();
		if (parent2 != parent)
		{
			if (parent2 != null)
			{
				node.Reparent(parent);
			}
			else
			{
				parent.AddChild(node, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
		StringName popCallableSN = GetPopCallableSN();
		DispatchPopLifecycle(node, popCallableSN);
		return node;
	}

	internal static void DispatchPopLifecycle(Node node, StringName callback)
	{
		if (!(callback == null) && GodotObject.IsInstanceValid(node))
		{
			if (callback == RefreshCallbackName && node is IObjectPoolLifecycle { SupportsDirectPoolLifecycleDispatch: not false } objectPoolLifecycle)
			{
				objectPoolLifecycle.RefreshFromPool();
			}
			else if (node.HasMethod(callback))
			{
				node.Call(callback);
			}
		}
	}

	internal static void DispatchPushLifecycle(Node node, StringName callback)
	{
		if (!(callback == null) && GodotObject.IsInstanceValid(node))
		{
			if (callback == RecycleCallbackName && node is IObjectPoolLifecycle { SupportsDirectPoolLifecycleDispatch: not false } objectPoolLifecycle)
			{
				objectPoolLifecycle.RecycleToPool();
			}
			else if (node.HasMethod(callback))
			{
				node.Call(callback);
			}
		}
	}

	public void Maintenance(ulong frame)
	{
		int num = Math.Max(0, idleRetainNum);
		if (maxNum >= 0)
		{
			num = Math.Min(num, maxNum);
		}
		if (stack.Count <= num || frame < _lastTouchedFrame || frame - _lastTouchedFrame < idleTrimFrames)
		{
			return;
		}
		while (stack.Count > num)
		{
			int index = stack.Count - 1;
			Node node = stack[index];
			stack.RemoveAt(index);
			if (GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion())
			{
				node.QueueFree();
			}
		}
	}

	private PackedScene GetScene()
	{
		if (scene != null)
		{
			return scene;
		}
		if (string.IsNullOrEmpty(scenePath))
		{
			return null;
		}
		string path = ProjectResourceUidCache.ResolveResourcePath(scenePath);
		scene = GD.Load<PackedScene>(path);
		return scene;
	}

	public void Clear()
	{
		foreach (Node item in stack)
		{
			if (GodotObject.IsInstanceValid(item) && !item.IsQueuedForDeletion())
			{
				item.QueueFree();
			}
		}
		stack.Clear();
		_pendingPush.Clear();
		_deferredPushScheduled = false;
		_lastTouchedFrame = 0uL;
	}

	public void ReleaseImmediately()
	{
		foreach (Node item in stack)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.Free();
			}
		}
		foreach (Node item2 in _pendingPush)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				item2.Free();
			}
		}
		stack.Clear();
		_pendingPush.Clear();
		_deferredPushScheduled = false;
		_lastTouchedFrame = 0uL;
		scene = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.GetPopCallableSN, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPushCallableSN, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Push, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.DeferredPushBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Pop, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchPopLifecycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "callback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchPushLifecycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "callback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Maintenance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseImmediately, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetPopCallableSN && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetPopCallableSN());
			return true;
		}
		if (method == MethodName.GetPushCallableSN && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetPushCallableSN());
			return true;
		}
		if (method == MethodName.Push && args.Count == 1)
		{
			Push(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeferredPushBatch && args.Count == 0)
		{
			DeferredPushBatch();
			ret = default;
			return true;
		}
		if (method == MethodName.Pop && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(Pop(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.DispatchPopLifecycle && args.Count == 2)
		{
			DispatchPopLifecycle(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchPushLifecycle && args.Count == 2)
		{
			DispatchPushLifecycle(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Maintenance && args.Count == 1)
		{
			Maintenance(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(GetScene());
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseImmediately && args.Count == 0)
		{
			ReleaseImmediately();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DispatchPopLifecycle && args.Count == 2)
		{
			DispatchPopLifecycle(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchPushLifecycle && args.Count == 2)
		{
			DispatchPushLifecycle(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetPopCallableSN)
		{
			return true;
		}
		if (method == MethodName.GetPushCallableSN)
		{
			return true;
		}
		if (method == MethodName.Push)
		{
			return true;
		}
		if (method == MethodName.DeferredPushBatch)
		{
			return true;
		}
		if (method == MethodName.Pop)
		{
			return true;
		}
		if (method == MethodName.DispatchPopLifecycle)
		{
			return true;
		}
		if (method == MethodName.DispatchPushLifecycle)
		{
			return true;
		}
		if (method == MethodName.Maintenance)
		{
			return true;
		}
		if (method == MethodName.GetScene)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.ReleaseImmediately)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.scene)
		{
			scene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.scenePath)
		{
			scenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.maxNum)
		{
			maxNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.idleRetainNum)
		{
			idleRetainNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.idleTrimFrames)
		{
			idleTrimFrames = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.popCallable)
		{
			popCallable = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pushCallable)
		{
			pushCallable = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._popCallableSN)
		{
			_popCallableSN = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._pushCallableSN)
		{
			_pushCallableSN = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._deferredPushScheduled)
		{
			_deferredPushScheduled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastTouchedFrame)
		{
			_lastTouchedFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.scene)
		{
			value = VariantUtils.CreateFrom(in scene);
			return true;
		}
		if (name == PropertyName.scenePath)
		{
			value = VariantUtils.CreateFrom(in scenePath);
			return true;
		}
		if (name == PropertyName.maxNum)
		{
			value = VariantUtils.CreateFrom(in maxNum);
			return true;
		}
		if (name == PropertyName.idleRetainNum)
		{
			value = VariantUtils.CreateFrom(in idleRetainNum);
			return true;
		}
		if (name == PropertyName.idleTrimFrames)
		{
			value = VariantUtils.CreateFrom(in idleTrimFrames);
			return true;
		}
		if (name == PropertyName.popCallable)
		{
			value = VariantUtils.CreateFrom(in popCallable);
			return true;
		}
		if (name == PropertyName.pushCallable)
		{
			value = VariantUtils.CreateFrom(in pushCallable);
			return true;
		}
		if (name == PropertyName._popCallableSN)
		{
			value = VariantUtils.CreateFrom(in _popCallableSN);
			return true;
		}
		if (name == PropertyName._pushCallableSN)
		{
			value = VariantUtils.CreateFrom(in _pushCallableSN);
			return true;
		}
		if (name == PropertyName._deferredPushScheduled)
		{
			value = VariantUtils.CreateFrom(in _deferredPushScheduled);
			return true;
		}
		if (name == PropertyName._lastTouchedFrame)
		{
			value = VariantUtils.CreateFrom(in _lastTouchedFrame);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.scene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.scenePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.idleRetainNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.idleTrimFrames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.popCallable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.pushCallable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName._popCallableSN, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._pushCallableSN, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._deferredPushScheduled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastTouchedFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.scene, Variant.From(in scene));
		info.AddProperty(PropertyName.scenePath, Variant.From(in scenePath));
		info.AddProperty(PropertyName.maxNum, Variant.From(in maxNum));
		info.AddProperty(PropertyName.idleRetainNum, Variant.From(in idleRetainNum));
		info.AddProperty(PropertyName.idleTrimFrames, Variant.From(in idleTrimFrames));
		info.AddProperty(PropertyName.popCallable, Variant.From(in popCallable));
		info.AddProperty(PropertyName.pushCallable, Variant.From(in pushCallable));
		info.AddProperty(PropertyName._popCallableSN, Variant.From(in _popCallableSN));
		info.AddProperty(PropertyName._pushCallableSN, Variant.From(in _pushCallableSN));
		info.AddProperty(PropertyName._deferredPushScheduled, Variant.From(in _deferredPushScheduled));
		info.AddProperty(PropertyName._lastTouchedFrame, Variant.From(in _lastTouchedFrame));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.scene, out var value))
		{
			scene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.scenePath, out var value2))
		{
			scenePath = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.maxNum, out var value3))
		{
			maxNum = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.idleRetainNum, out var value4))
		{
			idleRetainNum = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.idleTrimFrames, out var value5))
		{
			idleTrimFrames = value5.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.popCallable, out var value6))
		{
			popCallable = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pushCallable, out var value7))
		{
			pushCallable = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._popCallableSN, out var value8))
		{
			_popCallableSN = value8.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._pushCallableSN, out var value9))
		{
			_pushCallableSN = value9.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._deferredPushScheduled, out var value10))
		{
			_deferredPushScheduled = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastTouchedFrame, out var value11))
		{
			_lastTouchedFrame = value11.As<ulong>();
		}
	}
}
