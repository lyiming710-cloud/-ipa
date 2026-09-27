using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseShieldImpactBatch.cs")]
public sealed class TowerDefenseShieldImpactBatch : Node
{
	private struct ShieldImpactEntry
	{
		public TowerDefenseArmorInstance Armor;

		public AdobeAnimateSlot Slot;

		public double RemainingSeconds;
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Prepare = "Prepare";

		public static readonly StringName Schedule = "Schedule";

		public static readonly StringName TryMount = "TryMount";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName ResetVisual = "ResetVisual";

		public static readonly StringName RemoveAtSwap = "RemoveAtSwap";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InitialCapacity = 256;

	private static bool _mountRequested;

	private readonly List<ShieldImpactEntry> _active = new List<ShieldImpactEntry>(256);

	private readonly Dictionary<TowerDefenseArmorInstance, int> _activeIndices = new Dictionary<TowerDefenseArmorInstance, int>(256, ReferenceEqualityComparer.Instance);

	public static TowerDefenseShieldImpactBatch Instance { get; private set; }

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

	public static bool Prepare(TowerDefenseCharacter source)
	{
		if (!GodotObject.IsInstanceValid(source) || !source.IsInsideTree())
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(Instance))
		{
			TryMount(source);
		}
		return GodotObject.IsInstanceValid(Instance);
	}

	public static bool Schedule(TowerDefenseArmorInstance armor, AdobeAnimateSlot slot, double durationSeconds)
	{
		TowerDefenseCharacter towerDefenseCharacter = armor?.character;
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !towerDefenseCharacter.IsInsideTree() || !GodotObject.IsInstanceValid(slot) || !slot.IsInsideTree())
		{
			return false;
		}
		if (!Prepare(towerDefenseCharacter))
		{
			return false;
		}
		durationSeconds = ((durationSeconds > 0.0) ? durationSeconds : 1E-06);
		if (Instance._activeIndices.TryGetValue(armor, out var value))
		{
			ShieldImpactEntry value2 = Instance._active[value];
			value2.Slot = slot;
			value2.RemainingSeconds = durationSeconds;
			Instance._active[value] = value2;
		}
		else
		{
			Instance._activeIndices.Add(armor, Instance._active.Count);
			Instance._active.Add(new ShieldImpactEntry
			{
				Armor = armor,
				Slot = slot,
				RemainingSeconds = durationSeconds
			});
		}
		Instance.SetProcess(enable: true);
		return true;
	}

	private static void TryMount(TowerDefenseCharacter source)
	{
		if (!_mountRequested || !GodotObject.IsInstanceValid(Instance))
		{
			Node node = TowerDefenseProcessModeDispatch.ResolveBatchParent(source);
			if (GodotObject.IsInstanceValid(node))
			{
				TowerDefenseShieldImpactBatch towerDefenseShieldImpactBatch = (Instance = new TowerDefenseShieldImpactBatch());
				_mountRequested = true;
				node.CallDeferred(Node.MethodName.AddChild, towerDefenseShieldImpactBatch);
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
		for (int i = 0; i < _active.Count; i++)
		{
			ResetVisual(_active[i].Slot);
		}
		_active.Clear();
		_activeIndices.Clear();
		if (Instance == this)
		{
			Instance = null;
			_mountRequested = false;
		}
	}

	public override void _Process(double delta)
	{
		if (_active.Count == 0)
		{
			SetProcess(enable: false);
			return;
		}
		SceneTree tree = GetTree();
		if (tree != null && tree.Paused)
		{
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		int num = 0;
		double num2 = ((delta > 0.0) ? delta : 0.0);
		int num3 = 0;
		while (num3 < _active.Count)
		{
			ShieldImpactEntry value = _active[num3];
			if (!GodotObject.IsInstanceValid(value.Armor) || !GodotObject.IsInstanceValid(value.Slot) || !value.Slot.IsInsideTree())
			{
				RemoveAtSwap(num3);
				continue;
			}
			num++;
			value.RemainingSeconds -= num2;
			if (value.RemainingSeconds <= 0.0)
			{
				ResetVisual(value.Slot);
				RemoveAtSwap(num3);
			}
			else
			{
				_active[num3] = value;
				num3++;
			}
		}
		TowerDefensePerfProfiler.End("batch.characterFeedback.shieldImpact", startTicks, num);
		if (_active.Count == 0)
		{
			SetProcess(enable: false);
		}
	}

	private static void ResetVisual(AdobeAnimateSlot slot)
	{
		if (GodotObject.IsInstanceValid(slot))
		{
			slot.SetRuntimeVisualOffset(Vector2.Zero);
		}
	}

	private void RemoveAtSwap(int index)
	{
		int num = _active.Count - 1;
		TowerDefenseArmorInstance armor = _active[index].Armor;
		if (index != num)
		{
			ShieldImpactEntry value = _active[num];
			_active[index] = value;
			_activeIndices[value.Armor] = index;
		}
		_active.RemoveAt(num);
		_activeIndices.Remove(armor);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Prepare, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Schedule, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "armor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "durationSeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.ResetVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.Prepare && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Prepare(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Schedule && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(Schedule(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
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
		if (method == MethodName.ResetVisual && args.Count == 1)
		{
			ResetVisual(VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[0]));
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
		if (method == MethodName.Prepare && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Prepare(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.Schedule && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(Schedule(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetVisual && args.Count == 1)
		{
			ResetVisual(VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Prepare)
		{
			return true;
		}
		if (method == MethodName.Schedule)
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
		if (method == MethodName.ResetVisual)
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
