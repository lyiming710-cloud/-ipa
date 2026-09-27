using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Registry/Battle/Feature/Wave/TrioAmbush/TrioAmbushRuntime.cs")]
public class TrioAmbushRuntime : Node
{
	private sealed class Ticket
	{
		public int Operation;

		public double Due;

		public bool Coral;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnCold = "OnCold";

		public static readonly StringName Schedule = "Schedule";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName SpawnGroup = "SpawnGroup";

		public static readonly StringName Cancel = "Cancel";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName Wave = "Wave";

		public static readonly StringName _bus = "_bus";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public TowerDefenseBattleFeatureWave Wave;

	private BattleEventBus _bus;

	private readonly List<Ticket> _tickets = new List<Ticket>();

	private readonly List<TrioAmbushMember> _members = new List<TrioAmbushMember>();

	public TrioAmbushClock Clock { get; } = new TrioAmbushClock();

	public override void _Ready()
	{
		ProcessPhysicsPriority = 10000;
		ProcessMode = ProcessModeEnum.Pausable;
		_bus = BattleEventBus.Instance;
		if (GodotObject.IsInstanceValid(_bus))
		{
			_bus.OnColdEffectEmit += OnCold;
		}
	}

	private void OnCold()
	{
		TowerDefenseBattleFeatureWave wave = Wave;
		if (wave != null && wave.IsLifetimeActive)
		{
			Clock.Freeze();
		}
	}

	public void Schedule(bool coral)
	{
		TowerDefenseBattleFeatureWave wave = Wave;
		if (wave != null && wave.IsLifetimeActive && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			_tickets.Add(new Ticket
			{
				Operation = Wave.BeginPendingSpawnOperation(),
				Due = Clock.Time + 2.0,
				Coral = coral
			});
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		TowerDefenseBattleFeatureWave wave = Wave;
		if (wave == null || !wave.IsLifetimeActive || !GodotObject.IsInstanceValid(Wave.control))
		{
			Cancel();
		}
		else
		{
			if (!Wave.control.isGameRunning || GetTree().Paused)
			{
				return;
			}
			CommandManager instance = CommandManager.Instance;
			if (instance != null && instance.debug && instance.debugWavePaused)
			{
				return;
			}
			Clock.Advance(delta);
			for (int num = _tickets.Count - 1; num >= 0; num--)
			{
				Ticket ticket = _tickets[num];
				if (!(ticket.Due - Clock.Time > 1E-09))
				{
					_tickets.RemoveAt(num);
					try
					{
						if (Wave.IsPendingSpawnOperationCurrent(ticket.Operation) && !Clock.Frozen && !Wave.control.levelControl.awardCreate)
						{
							SpawnGroup(ticket.Coral);
						}
					}
					catch (Exception value)
					{
						GD.PushError($"[TrioAmbush] Spawn failed: {value}");
					}
					finally
					{
						Wave.CompletePendingSpawnOperation(ticket.Operation);
					}
				}
			}
			_members.RemoveAll((TrioAmbushMember m) => !GodotObject.IsInstanceValid(m));
		}
	}

	private void SpawnGroup(bool coral)
	{
		foreach (Vector2I item in TrioAmbushRules.PickCells(TrioAmbushRules.CandidateCells(TowerDefenseManager.Instance.gridNum, coral, (Vector2I pos) =>
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(pos);
			return GodotObject.IsInstanceValid(mapCell) && (!coral || mapCell.isWater);
		}), coral, (int max) => GD.RandRange(0, max - 1)))
		{
			string text = TrioAmbushRules.PickZombie(GD.RandRange(0, 10999));
			TowerDefenseZombie towerDefenseZombie = TowerDefenseManager.GetPacketConfig(text)?.Plant(item, playAudio: false) as TowerDefenseZombie;
			if (!GodotObject.IsInstanceValid(towerDefenseZombie))
			{
				throw new InvalidOperationException($"Cannot spawn {text} at {item}.");
			}
			int operation = Wave.BeginPendingSpawnOperation();
			TrioAmbushMember trioAmbushMember = TrioAmbushMember.Attach(towerDefenseZombie, coral, (!coral) ? GD.RandRange(3000, 3150) : 0, Wave, operation);
			_members.Add(trioAmbushMember);
			Wave.AddSpawnCharacter(towerDefenseZombie);
			trioAmbushMember.SyncId = TowerDefenseManager.PublishSpawnedCharacter(text, towerDefenseZombie, useCreate: true, 0.0, walkAfterSpawn: false, "", TrioAmbushMember.ExportSpawnState(towerDefenseZombie));
		}
	}

	public void Cancel()
	{
		foreach (Ticket ticket in _tickets)
		{
			Wave?.CompletePendingSpawnOperation(ticket.Operation);
		}
		_tickets.Clear();
		TrioAmbushMember[] array = _members.ToArray();
		foreach (TrioAmbushMember trioAmbushMember in array)
		{
			if (GodotObject.IsInstanceValid(trioAmbushMember))
			{
				trioAmbushMember.CancelEntry();
			}
		}
		_members.Clear();
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_bus))
		{
			_bus.OnColdEffectEmit -= OnCold;
		}
		Cancel();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCold, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Schedule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "coral", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "coral", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Cancel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnCold && args.Count == 0)
		{
			OnCold();
			ret = default;
			return true;
		}
		if (method == MethodName.Schedule && args.Count == 1)
		{
			Schedule(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnGroup && args.Count == 1)
		{
			SpawnGroup(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Cancel && args.Count == 0)
		{
			Cancel();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
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
		if (method == MethodName.OnCold)
		{
			return true;
		}
		if (method == MethodName.Schedule)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SpawnGroup)
		{
			return true;
		}
		if (method == MethodName.Cancel)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Wave)
		{
			Wave = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		if (name == PropertyName._bus)
		{
			_bus = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Wave)
		{
			value = VariantUtils.CreateFrom(in Wave);
			return true;
		}
		if (name == PropertyName._bus)
		{
			value = VariantUtils.CreateFrom(in _bus);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Wave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Wave, Variant.From(in Wave));
		info.AddProperty(PropertyName._bus, Variant.From(in _bus));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Wave, out var value))
		{
			Wave = value.As<TowerDefenseBattleFeatureWave>();
		}
		if (info.TryGetProperty(PropertyName._bus, out var value2))
		{
			_bus = value2.As<BattleEventBus>();
		}
	}
}
