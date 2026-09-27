using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseCharacterBatch.cs")]
public sealed class TowerDefenseCharacterBatch : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName UpdateProcessDispatch = "UpdateProcessDispatch";

		public static readonly StringName Unregister = "Unregister";

		public static readonly StringName TryMount = "TryMount";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName GetPhysicsMetricName = "GetPhysicsMetricName";

		public static readonly StringName RemoveAtSwap = "RemoveAtSwap";

		public static readonly StringName UpdateProcessMembership = "UpdateProcessMembership";

		public static readonly StringName RemoveProcess = "RemoveProcess";

		public static readonly StringName RemoveProcessAtSwap = "RemoveProcessAtSwap";
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

	private readonly List<TowerDefenseCharacter> _processActive = new List<TowerDefenseCharacter>();

	private readonly Dictionary<TowerDefenseCharacter, int> _processActiveIndices = new Dictionary<TowerDefenseCharacter, int>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<Type, string> _physicsMetricNames = new Dictionary<Type, string>();

	public static TowerDefenseCharacterBatch Instance { get; private set; }

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
		character.PrepareBatchRegistration();
		if (Instance._activeIndices.TryAdd(character, Instance._active.Count))
		{
			Instance._active.Add(character);
		}
		Instance.UpdateProcessMembership(character);
		character.SetOwnerBatchRegistration(registered: true);
		Instance.SetProcess(Instance._processActive.Count > 0);
		Instance.SetPhysicsProcess(enable: true);
		return true;
	}

	public static void UpdateProcessDispatch(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(Instance) && Instance._activeIndices.ContainsKey(character))
		{
			Instance.UpdateProcessMembership(character);
			Instance.SetProcess(Instance._processActive.Count > 0);
		}
	}

	public static void Unregister(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(Instance))
		{
			character?.SetOwnerBatchRegistration(registered: false);
			return;
		}
		if (Instance._activeIndices.TryGetValue(character, out var value))
		{
			Instance.RemoveAtSwap(value);
		}
		character?.SetOwnerBatchRegistration(registered: false);
		Instance.SetProcess(Instance._processActive.Count > 0);
		if (Instance._active.Count == 0)
		{
			Instance.SetPhysicsProcess(enable: false);
		}
	}

	private static void TryMount(TowerDefenseCharacter source)
	{
		if (!_mountRequested || !GodotObject.IsInstanceValid(Instance))
		{
			Node node = TowerDefenseProcessModeDispatch.ResolveBatchParent(source);
			if (GodotObject.IsInstanceValid(node))
			{
				TowerDefenseCharacterBatch towerDefenseCharacterBatch = (Instance = new TowerDefenseCharacterBatch());
				_mountRequested = true;
				node.CallDeferred(Node.MethodName.AddChild, towerDefenseCharacterBatch);
			}
		}
	}

	public override void _Ready()
	{
		Instance = this;
		_mountRequested = false;
		ProcessMode = ProcessModeEnum.Always;
		SetProcess(_processActive.Count > 0);
		SetPhysicsProcess(_active.Count > 0);
	}

	public override void _ExitTree()
	{
		if (Instance == this)
		{
			Instance = null;
			_mountRequested = false;
		}
		for (int i = 0; i < _active.Count; i++)
		{
			if (GodotObject.IsInstanceValid(_active[i]))
			{
				_active[i].SetOwnerBatchRegistration(registered: false);
			}
		}
		_active.Clear();
		_activeIndices.Clear();
		_processActive.Clear();
		_processActiveIndices.Clear();
		_physicsMetricNames.Clear();
	}

	public override void _Process(double delta)
	{
		if (_processActive.Count == 0)
		{
			SetProcess(enable: false);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool treePaused = GetTree()?.Paused ?? false;
		TowerDefenseProcessModeDispatch.BeginDispatchPass();
		int num = 0;
		int num2 = 0;
		while (num2 < _processActive.Count)
		{
			TowerDefenseCharacter towerDefenseCharacter = _processActive[num2];
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !towerDefenseCharacter.IsOwnerBatchDispatchActive || !towerDefenseCharacter.WantsMainStateMachineProcessDispatch)
			{
				RemoveProcessAtSwap(num2);
				continue;
			}
			if (!(towerDefenseCharacter.BatchUsesInheritedProcessMode ? TowerDefenseProcessModeDispatch.ShouldDispatchInherited(towerDefenseCharacter.BatchProcessModeParent, treePaused) : TowerDefenseProcessModeDispatch.ShouldDispatch(towerDefenseCharacter, treePaused)))
			{
				num2++;
				continue;
			}
			towerDefenseCharacter.BatchProcessUpdate(delta);
			num++;
			if (num2 < _processActive.Count && _processActive[num2] == towerDefenseCharacter)
			{
				num2++;
			}
		}
		TowerDefensePerfProfiler.End("batch.characterProcess", startTicks, num);
		if (_processActive.Count == 0)
		{
			SetProcess(enable: false);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_active.Count == 0)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		TowerDefenseProcessModeDispatch.EnterCharacterPhysicsBatchDispatch();
		try
		{
			long startTicks = TowerDefensePerfProfiler.Begin();
			bool treePaused = GetTree()?.Paused ?? false;
			TowerDefenseProcessModeDispatch.BeginFrame();
			TowerDefenseProcessModeDispatch.BeginDispatchPass();
			bool flag = TowerDefensePerfProfiler.Enabled && TowerDefensePerfProfiler.DetailedHotPathMetrics;
			int num = 0;
			int num2 = 0;
			while (num2 < _active.Count)
			{
				TowerDefenseCharacter towerDefenseCharacter = _active[num2];
				if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !towerDefenseCharacter.IsOwnerBatchDispatchActive)
				{
					RemoveAtSwap(num2);
					continue;
				}
				if (!(towerDefenseCharacter.BatchUsesInheritedProcessMode ? TowerDefenseProcessModeDispatch.ShouldDispatchInherited(towerDefenseCharacter.BatchProcessModeParent, treePaused) : TowerDefenseProcessModeDispatch.ShouldDispatch(towerDefenseCharacter, treePaused)))
				{
					num2++;
					continue;
				}
				if (flag)
				{
					long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
					towerDefenseCharacter.BatchUpdate(delta);
					TowerDefensePerfProfiler.End(GetPhysicsMetricName(towerDefenseCharacter), startTicks2);
				}
				else
				{
					towerDefenseCharacter.BatchUpdate(delta);
				}
				num++;
				if (num2 < _active.Count && _active[num2] == towerDefenseCharacter)
				{
					num2++;
				}
			}
			TowerDefensePerfProfiler.End("batch.character", startTicks, num);
			TowerDefensePerfProfiler.DumpIfNeeded();
			if (_active.Count == 0)
			{
				SetPhysicsProcess(enable: false);
			}
		}
		finally
		{
			TowerDefenseProcessModeDispatch.ExitCharacterPhysicsBatchDispatch();
		}
	}

	private string GetPhysicsMetricName(TowerDefenseCharacter character)
	{
		Type type = character.GetType();
		if (_physicsMetricNames.TryGetValue(type, out var value))
		{
			return value;
		}
		value = "batch.character.type." + type.Name;
		_physicsMetricNames.Add(type, value);
		return value;
	}

	private void RemoveAtSwap(int index)
	{
		int num = _active.Count - 1;
		TowerDefenseCharacter towerDefenseCharacter = _active[index];
		RemoveProcess(towerDefenseCharacter);
		if (index != num)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = _active[num];
			_active[index] = towerDefenseCharacter2;
			_activeIndices[towerDefenseCharacter2] = index;
		}
		_active.RemoveAt(num);
		_activeIndices.Remove(towerDefenseCharacter);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			towerDefenseCharacter.SetOwnerBatchRegistration(registered: false);
		}
	}

	private void UpdateProcessMembership(TowerDefenseCharacter character)
	{
		if (character.WantsMainStateMachineProcessDispatch)
		{
			if (_processActiveIndices.TryAdd(character, _processActive.Count))
			{
				_processActive.Add(character);
			}
		}
		else
		{
			RemoveProcess(character);
		}
	}

	private void RemoveProcess(TowerDefenseCharacter character)
	{
		if (_processActiveIndices.TryGetValue(character, out var value))
		{
			RemoveProcessAtSwap(value);
		}
	}

	private void RemoveProcessAtSwap(int index)
	{
		int num = _processActive.Count - 1;
		TowerDefenseCharacter key = _processActive[index];
		if (index != num)
		{
			TowerDefenseCharacter towerDefenseCharacter = _processActive[num];
			_processActive[index] = towerDefenseCharacter;
			_processActiveIndices[towerDefenseCharacter] = index;
		}
		_processActive.RemoveAt(num);
		_processActiveIndices.Remove(key);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessDispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPhysicsMetricName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessMembership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveProcessAtSwap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.UpdateProcessDispatch && args.Count == 1)
		{
			UpdateProcessDispatch(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPhysicsMetricName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPhysicsMetricName(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveAtSwap && args.Count == 1)
		{
			RemoveAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessMembership && args.Count == 1)
		{
			UpdateProcessMembership(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProcess && args.Count == 1)
		{
			RemoveProcess(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveProcessAtSwap && args.Count == 1)
		{
			RemoveProcessAtSwap(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.UpdateProcessDispatch && args.Count == 1)
		{
			UpdateProcessDispatch(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
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
		if (method == MethodName.UpdateProcessDispatch)
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.GetPhysicsMetricName)
		{
			return true;
		}
		if (method == MethodName.RemoveAtSwap)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessMembership)
		{
			return true;
		}
		if (method == MethodName.RemoveProcess)
		{
			return true;
		}
		if (method == MethodName.RemoveProcessAtSwap)
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
