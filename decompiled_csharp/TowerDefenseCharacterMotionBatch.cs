using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseCharacterMotionBatch.cs")]
public sealed class TowerDefenseCharacterMotionBatch : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName Unregister = "Unregister";

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

	private readonly List<TowerDefenseCharacter> _active = new List<TowerDefenseCharacter>();

	private readonly Dictionary<TowerDefenseCharacter, int> _activeIndices = new Dictionary<TowerDefenseCharacter, int>(ReferenceEqualityComparer.Instance);

	public static TowerDefenseCharacterMotionBatch Instance { get; private set; }

	public static bool Register(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character) || !character.IsInsideTree())
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(Instance))
		{
			TryMount(character);
		}
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return false;
		}
		if (Instance._activeIndices.TryAdd(character, Instance._active.Count))
		{
			Instance._active.Add(character);
		}
		Instance.SetProcess(enable: true);
		return true;
	}

	public static void Unregister(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(Instance) && character != null)
		{
			if (Instance._activeIndices.TryGetValue(character, out var value))
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
				TowerDefenseCharacterMotionBatch towerDefenseCharacterMotionBatch = (Instance = new TowerDefenseCharacterMotionBatch());
				_mountRequested = true;
				node.CallDeferred(Node.MethodName.AddChild, towerDefenseCharacterMotionBatch);
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
		ulong physicsFrames = Engine.GetPhysicsFrames();
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		int num = 0;
		Node node = null;
		bool flag = false;
		bool flag2 = false;
		int num2 = 0;
		while (num2 < _active.Count)
		{
			TowerDefenseCharacter towerDefenseCharacter = _active[num2];
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !towerDefenseCharacter.IsBowlingImpactDisplacementActive)
			{
				RemoveAtSwap(num2);
				continue;
			}
			bool flag3;
			if (towerDefenseCharacter.BatchUsesInheritedProcessMode)
			{
				Node batchProcessModeParent = towerDefenseCharacter.BatchProcessModeParent;
				if (!flag2 || node != batchProcessModeParent)
				{
					node = batchProcessModeParent;
					flag = TowerDefenseProcessModeDispatch.ShouldDispatchInherited(batchProcessModeParent, treePaused);
					flag2 = true;
				}
				flag3 = flag;
			}
			else
			{
				flag3 = TowerDefenseProcessModeDispatch.ShouldDispatch(towerDefenseCharacter, treePaused);
			}
			if (!flag3)
			{
				num2++;
				continue;
			}
			num++;
			if (!towerDefenseCharacter.UpdateBowlingImpactDisplacement(delta, physicsFrames))
			{
				RemoveAtSwap(num2);
			}
			else
			{
				num2++;
			}
		}
		TowerDefensePerfProfiler.End("batch.characterMotion.bowlingImpact", startTicks, num);
		if (_active.Count == 0)
		{
			SetProcess(enable: false);
		}
	}

	private void RemoveAtSwap(int index)
	{
		int num = _active.Count - 1;
		TowerDefenseCharacter key = _active[index];
		if (index != num)
		{
			TowerDefenseCharacter towerDefenseCharacter = _active[num];
			_active[index] = towerDefenseCharacter;
			_activeIndices[towerDefenseCharacter] = index;
		}
		_active.RemoveAt(num);
		_activeIndices.Remove(key);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
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
		if (method == MethodName.Register && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.Register && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.Unregister)
		{
			return true;
		}
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
