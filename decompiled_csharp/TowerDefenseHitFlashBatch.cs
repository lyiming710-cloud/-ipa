using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Script/Component/TowerDefense/Character/HitFlashComponent/TowerDefenseHitFlashBatch.cs")]
public sealed class TowerDefenseHitFlashBatch : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName TryMount = "TryMount";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName RemoveAtSwap = "RemoveAtSwap";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static bool _mountRequested;

	private readonly List<HitFlashComponent> _active = new List<HitFlashComponent>();

	private readonly Dictionary<HitFlashComponent, int> _activeIndices = new Dictionary<HitFlashComponent, int>(ReferenceEqualityComparer.Instance);

	public static TowerDefenseHitFlashBatch Instance { get; private set; }

	public static int ActiveCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._active.Count;
		}
	}

	public static bool Register(HitFlashComponent component)
	{
		TowerDefenseCharacter towerDefenseCharacter = component?.parent;
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !towerDefenseCharacter.IsInsideTree())
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(Instance))
		{
			TryMount(towerDefenseCharacter);
		}
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return false;
		}
		if (Instance._activeIndices.TryAdd(component, Instance._active.Count))
		{
			Instance._active.Add(component);
		}
		Instance.SetProcess(enable: true);
		return true;
	}

	public static void Unregister(HitFlashComponent component)
	{
		if (GodotObject.IsInstanceValid(Instance) && component != null)
		{
			if (Instance._activeIndices.TryGetValue(component, out var value))
			{
				Instance.RemoveAtSwap(value);
			}
			if (Instance._active.Count == 0)
			{
				Instance.SetProcess(enable: false);
			}
		}
	}

	private static void TryMount(TowerDefenseCharacter source)
	{
		if (!_mountRequested || !GodotObject.IsInstanceValid(Instance))
		{
			Node node = TowerDefenseProcessModeDispatch.ResolveBatchParent(source);
			if (GodotObject.IsInstanceValid(node))
			{
				TowerDefenseHitFlashBatch towerDefenseHitFlashBatch = (Instance = new TowerDefenseHitFlashBatch());
				_mountRequested = true;
				node.CallDeferred(Node.MethodName.AddChild, towerDefenseHitFlashBatch);
			}
		}
	}

	public override void _Ready()
	{
		Instance = this;
		_mountRequested = false;
		ProcessMode = ProcessModeEnum.Always;
		SetProcess(_active.Count > 0);
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
			_mountRequested = false;
		}
		_active.Clear();
		_activeIndices.Clear();
	}

	public override void _Process(double delta)
	{
		if (_active.Count == 0)
		{
			SetProcess(enable: false);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool treePaused = GetTree()?.Paused ?? false;
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		int num = 0;
		int num2 = 0;
		while (num2 < _active.Count)
		{
			HitFlashComponent hitFlashComponent = _active[num2];
			TowerDefenseCharacter towerDefenseCharacter = hitFlashComponent?.parent;
			if (hitFlashComponent == null || !hitFlashComponent.IsBatchUpdateActive || !GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				RemoveAtSwap(num2);
				continue;
			}
			if (!(towerDefenseCharacter.BatchUsesInheritedProcessMode ? TowerDefenseProcessModeDispatch.ShouldDispatchInherited(towerDefenseCharacter.BatchProcessModeParent, treePaused) : TowerDefenseProcessModeDispatch.ShouldDispatch(towerDefenseCharacter, treePaused)))
			{
				num2++;
				continue;
			}
			num++;
			if (!hitFlashComponent.UpdateBatch(delta))
			{
				RemoveAtSwap(num2);
			}
			else
			{
				num2++;
			}
		}
		TowerDefensePerfProfiler.End("batch.characterFeedback.hitFlash", startTicks, num);
		if (_active.Count == 0)
		{
			SetProcess(enable: false);
		}
	}

	private void RemoveAtSwap(int index)
	{
		int num = _active.Count - 1;
		HitFlashComponent key = _active[index];
		if (index != num)
		{
			HitFlashComponent hitFlashComponent = _active[num];
			_active[index] = hitFlashComponent;
			_activeIndices[hitFlashComponent] = index;
		}
		_active.RemoveAt(num);
		_activeIndices.Remove(key);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.TryMount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveAtSwap && args.Count == 1)
		{
			RemoveAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.TryMount)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.RemoveAtSwap)
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
