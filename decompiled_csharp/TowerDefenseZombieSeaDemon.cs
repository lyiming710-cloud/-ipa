using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter3/SeaDemon/Scene/Base/TowerDefenseZombieSeaDemon.cs")]
public class TowerDefenseZombieSeaDemon : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName DieEntered = "DieEntered";

		public static readonly StringName PointEntered = "PointEntered";

		public static readonly StringName PointProcessing = "PointProcessing";

		public static readonly StringName PointExited = "PointExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName SpawnSnorkle = "SpawnSnorkle";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName GlobalPositionX = "GlobalPositionX";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName timer = "timer";

		public static readonly StringName spawnNext = "spawnNext";

		public static readonly StringName jumpMove = "jumpMove";

		public static readonly StringName isPointing = "isPointing";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _pointStateHandle;

	private bool _stateSignalsConnected;

	public double timer;

	public bool spawnNext;

	public bool jumpMove;

	public bool isPointing;

	private float GlobalPositionX
	{
		get
		{
			return GetLogicalGlobalPosition().X;
		}
		set
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X = value;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["timer"] = timer;
		dictionary["spawnNext"] = spawnNext;
		dictionary["jumpMove"] = jumpMove;
		dictionary["isPointing"] = isPointing;
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		timer = Math.Max(0.0, data.GetValueOrDefault("timer", timer).AsDouble());
		spawnNext = data.GetValueOrDefault("spawnNext", spawnNext).AsBool();
		jumpMove = data.GetValueOrDefault("jumpMove", jumpMove).AsBool();
		isPointing = data.GetValueOrDefault("isPointing", isPointing).AsBool();
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
			_pointStateHandle = StateMachine?.GetStateById("zombie.sea_demon.point");
			StateHandle pointStateHandle = _pointStateHandle;
			if (pointStateHandle != null && pointStateHandle.IsValid)
			{
				_pointStateHandle.Entered += PointEntered;
				_pointStateHandle.Exited += PointExited;
				_pointStateHandle.PhysicsProcessing += PointProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_pointStateHandle != null)
			{
				_pointStateHandle.Entered -= PointEntered;
				_pointStateHandle.Exited -= PointExited;
				_pointStateHandle.PhysicsProcessing -= PointProcessing;
			}
			_pointStateHandle = null;
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

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && inGame && IsInsideComponentBattlefield && !spawnNext)
		{
			if (timer < 15.0)
			{
				timer += delta * timeScale;
				return;
			}
			spawnNext = true;
			timer = 0.0;
		}
	}

	public override void WalkEntered()
	{
		if (inWater)
		{
			if (!inSwimPlay && inSwimAnimeClip != "")
			{
				sprite.SetAnimation(inSwimAnimeClip, loop: false, 0.2);
				sprite.AddAnimation(swimAnimeClip, 0.0);
				inSwimPlay = true;
			}
			else
			{
				sprite.SetAnimation(swimAnimeClip, loop: true, 0.2);
			}
			ActivateGroundMovementAfterStateDelay(WalkStateHandle);
		}
		else if (jumpMove)
		{
			jumpMove = false;
			if (inWater)
			{
				sprite.SetAnimation(swimAnimeClip);
			}
			else
			{
				sprite.SetAnimation(walkAnimeClip);
			}
			ActivateGroundMovementAfterStateDelay(WalkStateHandle);
		}
		else
		{
			base.WalkEntered();
		}
	}

	public override void OutWater()
	{
		base.OutWater();
		GlobalPositionX -= Scale.X * transformPoint.Scale.X * 20f;
	}

	public override void DieEntered()
	{
		base.DieEntered();
		sprite.offset = new Vector2(-50f, -80f);
	}

	public void PointEntered()
	{
		isPointing = true;
		if (inWater)
		{
			sprite.SetAnimation("SwimPoint", loop: false);
		}
		else
		{
			sprite.SetAnimation("Point", loop: false);
		}
	}

	public void PointProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void PointExited()
	{
		isPointing = false;
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!die && !nearDie && spawnNext && !isPointing && SendStateEvent("ToPoint"))
		{
			spawnNext = false;
			return;
		}
		switch (clip)
		{
		case "Point":
		case "SwimPoint":
			isPointing = false;
			Walk();
			break;
		case "Jump":
			jumpMove = true;
			GlobalPositionX -= Scale.X * transformPoint.Scale.X * 60f;
			break;
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!(command == "attack"))
		{
			if (command == "spawn")
			{
				SpawnSnorkle();
			}
		}
		else
		{
			attackComponent.AttackExecute(((TowerDefenseZombieConfig)config).smashAttack);
		}
	}

	public void SpawnSnorkle()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieSnorkleTanglekelp");
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		double hitpointScale = instance.hitpointScale;
		Vector2 scale = transformPoint.Scale;
		float x = GetLogicalGlobalPosition().X;
		if (gridPos.Y > 1)
		{
			TowerDefenseCharacter snorkle = packetConfig.Create(new Vector2(x, (float)TowerDefenseManager.GetMapLineY(gridPos.Y - 1)), gridPos - new Vector2I(0, 1));
			TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", snorkle);
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(snorkle))
				{
					if (GodotObject.IsInstanceValid(snorkle.instance))
					{
						snorkle.instance.hitpointScale = hitpointScale;
					}
					if (GodotObject.IsInstanceValid(snorkle.transformPoint))
					{
						snorkle.transformPoint.Scale = scale;
					}
				}
			}).CallDeferred();
			Callable.From(() =>
			{
				snorkle.Rise(1.5);
			}).CallDeferred();
			snorkle.invisible = invisible;
			if (instance.hypnoses)
			{
				Callable.From(() =>
				{
					if (GodotObject.IsInstanceValid(snorkle))
					{
						snorkle.Hypnoses();
					}
				}).CallDeferred();
			}
		}
		if (gridPos.Y < mapGridNum.Y)
		{
			TowerDefenseCharacter snorkle2 = packetConfig.Create(new Vector2(x, (float)TowerDefenseManager.GetMapLineY(gridPos.Y + 1)), gridPos + new Vector2I(0, 1));
			TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", snorkle2);
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(snorkle2))
				{
					if (GodotObject.IsInstanceValid(snorkle2.instance))
					{
						snorkle2.instance.hitpointScale = hitpointScale;
					}
					if (GodotObject.IsInstanceValid(snorkle2.transformPoint))
					{
						snorkle2.transformPoint.Scale = scale;
					}
				}
			}).CallDeferred();
			Callable.From(() =>
			{
				snorkle2.Rise(1.5);
			}).CallDeferred();
			snorkle2.invisible = invisible;
			if (instance.hypnoses)
			{
				Callable.From(() =>
				{
					if (GodotObject.IsInstanceValid(snorkle2))
					{
						snorkle2.Hypnoses();
					}
				}).CallDeferred();
			}
		}
		TowerDefenseCharacter snorkle3 = packetConfig.Create(new Vector2(x, (float)TowerDefenseManager.GetMapLineY(gridPos.Y)), gridPos);
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", snorkle3);
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(snorkle3))
			{
				if (GodotObject.IsInstanceValid(snorkle3.instance))
				{
					snorkle3.instance.hitpointScale = hitpointScale;
				}
				if (GodotObject.IsInstanceValid(snorkle3.transformPoint))
				{
					snorkle3.transformPoint.Scale = scale;
				}
			}
		}).CallDeferred();
		Callable.From(() =>
		{
			snorkle3.Rise(1.5);
		}).CallDeferred();
		snorkle3.invisible = invisible;
		if (!instance.hypnoses)
		{
			return;
		}
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(snorkle3))
			{
				snorkle3.Hypnoses();
			}
		}).CallDeferred();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PointEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PointProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PointExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnSnorkle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PointEntered && args.Count == 0)
		{
			PointEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PointProcessing && args.Count == 1)
		{
			PointProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PointExited && args.Count == 0)
		{
			PointExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnSnorkle && args.Count == 0)
		{
			SpawnSnorkle();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.PointEntered)
		{
			return true;
		}
		if (method == MethodName.PointProcessing)
		{
			return true;
		}
		if (method == MethodName.PointExited)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.SpawnSnorkle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			GlobalPositionX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnNext)
		{
			spawnNext = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpMove)
		{
			jumpMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isPointing)
		{
			isPointing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			value = VariantUtils.CreateFrom<float>(GlobalPositionX);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.spawnNext)
		{
			value = VariantUtils.CreateFrom(in spawnNext);
			return true;
		}
		if (name == PropertyName.jumpMove)
		{
			value = VariantUtils.CreateFrom(in jumpMove);
			return true;
		}
		if (name == PropertyName.isPointing)
		{
			value = VariantUtils.CreateFrom(in isPointing);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spawnNext, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.jumpMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPointing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.GlobalPositionX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GlobalPositionX, Variant.From<float>(GlobalPositionX));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.spawnNext, Variant.From(in spawnNext));
		info.AddProperty(PropertyName.jumpMove, Variant.From(in jumpMove));
		info.AddProperty(PropertyName.isPointing, Variant.From(in isPointing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GlobalPositionX, out var value))
		{
			GlobalPositionX = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value2))
		{
			_stateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value3))
		{
			timer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnNext, out var value4))
		{
			spawnNext = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpMove, out var value5))
		{
			jumpMove = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isPointing, out var value6))
		{
			isPointing = value6.As<bool>();
		}
	}
}
