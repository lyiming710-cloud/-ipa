using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/CannonComponentHotPathRuntimeTest.cs")]
public class CannonComponentHotPathRuntimeTest : Node
{
	private readonly struct SegmentResult(bool passed, double mean, double p99, long allocatedBytes, int gen0, int activeInstances, int wantsPhysics, int dispatched)
	{
		public readonly bool Passed = passed;

		public readonly double Mean = mean;

		public readonly double P99 = p99;

		public readonly long AllocatedBytes = allocatedBytes;

		public readonly int Gen0 = gen0;

		public readonly int ActiveInstances = activeInstances;

		public readonly int WantsPhysics = wantsPhysics;

		public readonly int Dispatched = dispatched;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateWorkload = "CreateWorkload";

		public static readonly StringName RunFunctionalContract = "RunFunctionalContract";

		public static readonly StringName ConfigureSegment = "ConfigureSegment";

		public static readonly StringName Percentile = "Percentile";

		public static readonly StringName CountTreeOwners = "CountTreeOwners";

		public static readonly StringName ReleaseWorkload = "ReleaseWorkload";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _managers = "_managers";

		public static readonly StringName _samples = "_samples";

		public static readonly StringName _definition = "_definition";

		public static readonly StringName _ownerMount = "_ownerMount";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private readonly CannonComponent[] _components = new CannonComponent[1000];

	private readonly TowerDefenseCharacter[] _owners = new TowerDefenseCharacter[1000];

	private readonly ComponentManager[] _managers = new ComponentManager[1000];

	private readonly double[] _samples = new double[1200];

	private CannonComponentDefinition _definition;

	private Node2D _ownerMount;

	private readonly FieldInfo _runtimeInitializedField = typeof(CannonComponent).GetField("_runtimeInitialized", BindingFlags.Instance | BindingFlags.NonPublic);

	public override void _Ready()
	{
		bool flag = false;
		try
		{
			CreateWorkload();
			if (!RunFunctionalContract())
			{
				throw new InvalidOperationException("Cannon timer/ready lifecycle contract failed.");
			}
			SegmentResult segmentResult = RunSegment(0);
			SegmentResult segmentResult2 = RunSegment(1);
			SegmentResult segmentResult3 = RunSegment(2);
			int num = CountTreeOwners();
			flag = num == 1000 && segmentResult.Passed && segmentResult2.Passed && segmentResult3.Passed;
			GD.Print($"CANNON_COMPONENT_HOT_PATH_RESULT passed={flag} instances={1000} treeOwners={num} warmupSamples={240} measuredSamples={1200} idlePassed={segmentResult.Passed} idleP99Ms={segmentResult.P99:F6} idleAllocatedBytes={segmentResult.AllocatedBytes} idleGen0={segmentResult.Gen0} idleActiveInstances={segmentResult.ActiveInstances} idleWantsPhysicsInstances={segmentResult.WantsPhysics} idleDispatchedInstances={segmentResult.Dispatched} restTimerPassed={segmentResult2.Passed} restTimerP99Ms={segmentResult2.P99:F6} restTimerAllocatedBytes={segmentResult2.AllocatedBytes} restTimerGen0={segmentResult2.Gen0} restTimerActiveInstances={segmentResult2.ActiveInstances} restTimerWantsPhysicsInstances={segmentResult2.WantsPhysics} restTimerDispatchedInstances={segmentResult2.Dispatched} readyAutoAttackPassed={segmentResult3.Passed} readyAutoAttackP99Ms={segmentResult3.P99:F6} readyAutoAttackAllocatedBytes={segmentResult3.AllocatedBytes} readyAutoAttackGen0={segmentResult3.Gen0} readyAutoAttackActiveInstances={segmentResult3.ActiveInstances} readyAutoAttackWantsPhysicsInstances={segmentResult3.WantsPhysics} readyAutoAttackDispatchedInstances={segmentResult3.Dispatched}");
		}
		catch (Exception ex)
		{
			GD.PrintErr("CANNON_COMPONENT_HOT_PATH_EXCEPTION " + ex);
		}
		finally
		{
			ReleaseWorkload();
		}
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void CreateWorkload()
	{
		_ownerMount = new Node2D
		{
			Name = "CannonOwnerMount"
		};
		AddChild(_ownerMount, forceReadableName: false, InternalMode.Disabled);
		_definition = new CannonComponentDefinition
		{
			ComponentTypeId = "CannonComponent",
			DefinitionId = "cannon.hotpath.runtime",
			InstanceId = "cannon.hotpath.runtime",
			WireIndex = 0,
			mode = "Marker"
		};
		for (int i = 0; i < 1000; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = new CannonHotPathProbeOwner
			{
				Name = "CannonHotPathOwner" + i,
				inGame = false,
				ProcessMode = ProcessModeEnum.Disabled
			};
			_ownerMount.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
			ComponentManager componentManager = new ComponentManager();
			CannonComponent cannonComponent = new CannonComponent();
			cannonComponent.Bind(componentManager, towerDefenseCharacter, _definition);
			cannonComponent.Activate();
			_components[i] = cannonComponent;
			_owners[i] = towerDefenseCharacter;
			_managers[i] = componentManager;
		}
	}

	private bool RunFunctionalContract()
	{
		CannonComponent cannonComponent = _components[0];
		TowerDefenseCharacter towerDefenseCharacter = _owners[0];
		cannonComponent.autoAttack = true;
		cannonComponent.canFire = true;
		cannonComponent.SyncDeserialize(new Dictionary
		{
			["restTimerRunning"] = true,
			["restTimerRemaining"] = 0.25,
			["restTimerPaused"] = false,
			["pendingRestTimeout"] = false
		});
		Dictionary dictionary = cannonComponent.SyncSerialize();
		bool num = dictionary.GetValueOrDefault("restTimerRunning", false).AsBool() && dictionary.GetValueOrDefault("restTimerRemaining", 0.0).AsDouble() > 0.0;
		cannonComponent.SyncDeserialize(new Dictionary
		{
			["restTimerRunning"] = false,
			["restTimerRemaining"] = 0.5,
			["restTimerPaused"] = true,
			["pendingRestTimeout"] = false
		});
		bool flag = !cannonComponent.SyncSerialize().GetValueOrDefault("restTimerRunning", true).AsBool();
		cannonComponent.Timeout("Rest");
		bool flag2 = cannonComponent.SyncSerialize().GetValueOrDefault("pendingRestTimeout", false).AsBool();
		cannonComponent.canFire = false;
		cannonComponent.SetAlive(alive: false);
		bool flag3 = !cannonComponent.canFire;
		cannonComponent.SetAlive(alive: true);
		cannonComponent.FireAt(Vector2.One);
		cannonComponent.Detach(ComponentDetachReason.TemporaryTreeExit);
		cannonComponent.Bind(_managers[0], towerDefenseCharacter, _definition);
		cannonComponent.Activate();
		bool flag4 = cannonComponent.Lifecycle == ComponentRuntimeLifecycle.Active && cannonComponent.parent == towerDefenseCharacter;
		return num & flag & flag2 & flag3 & flag4;
	}

	private SegmentResult RunSegment(int mode)
	{
		ConfigureSegment(mode);
		for (int i = 0; i < 240; i++)
		{
			for (int j = 0; j < 1000; j++)
			{
				if (_components[j].WantsPhysicsProcess)
				{
					_components[j].PhysicsProcess(0.0005, (ulong)i);
				}
			}
		}
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		int num = GC.CollectionCount(0);
		int num2 = 0;
		for (int k = 0; k < 1200; k++)
		{
			long timestamp = Stopwatch.GetTimestamp();
			for (int l = 0; l < 1000; l++)
			{
				if (_components[l].WantsPhysicsProcess)
				{
					_components[l].PhysicsProcess(0.0005, (ulong)(240 + k));
					num2++;
				}
			}
			_samples[k] = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
		}
		long num3 = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		int num4 = GC.CollectionCount(0) - num;
		System.Array.Sort(_samples);
		double num5 = 0.0;
		for (int m = 0; m < _samples.Length; m++)
		{
			num5 += _samples[m];
		}
		num5 /= (double)_samples.Length;
		double num6 = Percentile(0.99);
		int num7 = 0;
		int num8 = 0;
		for (int n = 0; n < 1000; n++)
		{
			if (_components[n].Lifecycle == ComponentRuntimeLifecycle.Active)
			{
				num7++;
			}
			if (_components[n].WantsPhysicsProcess)
			{
				num8++;
			}
		}
		return new SegmentResult(num7 == 1000 && num3 == 0L && num4 == 0 && num6 < 0.2, num5, num6, num3, num4, num7, num8, num2);
	}

	private void ConfigureSegment(int mode)
	{
		for (int i = 0; i < 1000; i++)
		{
			CannonComponent cannonComponent = _components[i];
			TowerDefenseCharacter obj = _owners[i];
			cannonComponent.SetAlive(alive: true);
			obj.inGame = mode != 0;
			cannonComponent.autoAttack = mode == 2;
			cannonComponent.canFire = mode == 2;
			_runtimeInitializedField?.SetValue(cannonComponent, mode != 0);
			cannonComponent.SyncDeserialize(new Dictionary
			{
				["restTimerRunning"] = mode == 1,
				["restTimerRemaining"] = ((mode == 1) ? 30.0 : 0.5),
				["restTimerPaused"] = mode == 2,
				["pendingRestTimeout"] = false
			});
		}
	}

	private double Percentile(double fraction)
	{
		int num = Math.Clamp((int)Math.Ceiling(fraction * (double)_samples.Length) - 1, 0, _samples.Length - 1);
		return _samples[num];
	}

	private int CountTreeOwners()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			TowerDefenseCharacter obj = _owners[i];
			if (obj != null && obj.IsInsideTree())
			{
				num++;
			}
		}
		return num;
	}

	private void ReleaseWorkload()
	{
		for (int i = 0; i < 1000; i++)
		{
			_components[i]?.Release();
			if (GodotObject.IsInstanceValid(_owners[i]))
			{
				_owners[i].Free();
			}
			_components[i] = null;
			_owners[i] = null;
			_managers[i] = null;
		}
		if (GodotObject.IsInstanceValid(_ownerMount))
		{
			RemoveChild(_ownerMount);
			_ownerMount.Free();
		}
		_ownerMount = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(7)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateWorkload, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunFunctionalContract, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ConfigureSegment, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Percentile, new Godot.Bridge.PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Float, "fraction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CountTreeOwners, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ReleaseWorkload, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.CreateWorkload && args.Count == 0)
		{
			CreateWorkload();
			ret = default;
			return true;
		}
		if (method == MethodName.RunFunctionalContract && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RunFunctionalContract());
			return true;
		}
		if (method == MethodName.ConfigureSegment && args.Count == 1)
		{
			ConfigureSegment(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Percentile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(Percentile(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.CountTreeOwners && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountTreeOwners());
			return true;
		}
		if (method == MethodName.ReleaseWorkload && args.Count == 0)
		{
			ReleaseWorkload();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.CreateWorkload)
		{
			return true;
		}
		if (method == MethodName.RunFunctionalContract)
		{
			return true;
		}
		if (method == MethodName.ConfigureSegment)
		{
			return true;
		}
		if (method == MethodName.Percentile)
		{
			return true;
		}
		if (method == MethodName.CountTreeOwners)
		{
			return true;
		}
		if (method == MethodName.ReleaseWorkload)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<CannonComponentDefinition>(in value);
			return true;
		}
		if (name == PropertyName._ownerMount)
		{
			_ownerMount = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._owners)
		{
			GodotObject[] owners = _owners;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(owners);
			return true;
		}
		if (name == PropertyName._managers)
		{
			GodotObject[] owners = _managers;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(owners);
			return true;
		}
		if (name == PropertyName._samples)
		{
			value = VariantUtils.CreateFrom(in _samples);
			return true;
		}
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
			return true;
		}
		if (name == PropertyName._ownerMount)
		{
			value = VariantUtils.CreateFrom(in _ownerMount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._owners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._managers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._samples, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._ownerMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._ownerMount, Variant.From(in _ownerMount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._definition, out var value))
		{
			_definition = value.As<CannonComponentDefinition>();
		}
		if (info.TryGetProperty(PropertyName._ownerMount, out var value2))
		{
			_ownerMount = value2.As<Node2D>();
		}
	}
}
