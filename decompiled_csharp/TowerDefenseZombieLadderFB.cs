using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/LadderFB/Scene/TowerDefenseZombieLadderFB.cs")]
public class TowerDefenseZombieLadderFB : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LadderEntered = "LadderEntered";

		public static readonly StringName LadderProcessing = "LadderProcessing";

		public static readonly StringName LadderExited = "LadderExited";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName LadderPlace = "LadderPlace";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName ladderOver = "ladderOver";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _ladderOver = "_ladderOver";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _ladderStateHandle;

	private bool _stateSignalsConnected;

	private bool _ladderOver;

	public bool ladderOver
	{
		get
		{
			return _ladderOver;
		}
		set
		{
			_ladderOver = value;
			if (_ladderOver)
			{
				swimAnimeClip = "Walk";
				walkAnimeClip = "Walk";
				attackAnimeClip = "Eat";
			}
			else
			{
				swimAnimeClip = "LaderWalk";
				walkAnimeClip = "LaderWalk";
				attackAnimeClip = "LadderEat";
			}
		}
	}

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_ladderStateHandle = StateMachine?.GetStateById("zombie.ladder.ladder");
			StateHandle ladderStateHandle = _ladderStateHandle;
			if (ladderStateHandle != null && ladderStateHandle.IsValid)
			{
				_ladderStateHandle.Entered += LadderEntered;
				_ladderStateHandle.Exited += LadderExited;
				_ladderStateHandle.PhysicsProcessing += LadderProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_ladderStateHandle != null)
			{
				_ladderStateHandle.Entered -= LadderEntered;
				_ladderStateHandle.Exited -= LadderExited;
				_ladderStateHandle.PhysicsProcessing -= LadderProcessing;
			}
			_ladderStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			ConnectStateSignals();
		}
	}

	public virtual void LadderEntered()
	{
		sprite.SetAnimation("Ladder", loop: false, 0.2);
	}

	public virtual void LadderProcessing(double delta)
	{
		sprite.timeScale = timeScale * walkSpeedScale * 1.0;
	}

	public virtual void LadderExited()
	{
	}

	public override void AttackProcessing(double delta)
	{
		if (!ladderOver && !sprite.pause && attackComponent.CanAttack() && GodotObject.IsInstanceValid(attackComponent.target))
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(attackComponent.target.gridPos);
			if (GodotObject.IsInstanceValid(mapCell) && mapCell.HasWallnut())
			{
				SendStateEvent("ToLadder");
				return;
			}
		}
		base.AttackProcessing(delta);
		if (!ladderOver)
		{
			sprite.timeScale = timeScale * 4.0;
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (!ladderOver && !sprite.pause && attackComponent.CanAttack() && GodotObject.IsInstanceValid(attackComponent.target))
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(attackComponent.target.gridPos);
			if (GodotObject.IsInstanceValid(mapCell) && mapCell.HasWallnut())
			{
				SendStateEvent("ToLadder");
				return;
			}
		}
		base.WalkProcessing(delta);
		sprite.timeScale = timeScale * walkSpeedScale;
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Ladder")
		{
			ladderOver = true;
			Walk();
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "place_ladder")
		{
			LadderPlace();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Ladder")
		{
			ladderOver = true;
			Walk();
		}
	}

	public void LadderPlace()
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ItemLadder");
		if (GodotObject.IsInstanceValid(attackComponent.target))
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(attackComponent.target.gridPos);
			if (GodotObject.IsInstanceValid(mapCell) && mapCell.HasWallnut() && mapCell.CanPacketPlant(packetConfig))
			{
				packetConfig.Plant(attackComponent.target.gridPos);
				ladderOver = true;
				instance.ArmorDelete("Ladder", createDamagePart: false);
				attackComponent.target = null;
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "ladderOver", ladderOver } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		ladderOver = data.GetValueOrDefault("ladderOver", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LadderEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LadderProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LadderExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LadderPlace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConnectStateSignals && args.Count == 0)
		{
			ConnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectStateSignals && args.Count == 0)
		{
			DisconnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.LadderEntered && args.Count == 0)
		{
			LadderEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.LadderProcessing && args.Count == 1)
		{
			LadderProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LadderExited && args.Count == 0)
		{
			LadderExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LadderPlace && args.Count == 0)
		{
			LadderPlace();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.LadderEntered)
		{
			return true;
		}
		if (method == MethodName.LadderProcessing)
		{
			return true;
		}
		if (method == MethodName.LadderExited)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.LadderPlace)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ladderOver)
		{
			ladderOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ladderOver)
		{
			_ladderOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ladderOver)
		{
			value = VariantUtils.CreateFrom<bool>(ladderOver);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._ladderOver)
		{
			value = VariantUtils.CreateFrom(in _ladderOver);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ladderOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ladderOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ladderOver, Variant.From<bool>(ladderOver));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._ladderOver, Variant.From(in _ladderOver));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ladderOver, out var value))
		{
			ladderOver = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value2))
		{
			_stateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ladderOver, out var value3))
		{
			_ladderOver = value3.As<bool>();
		}
	}
}
