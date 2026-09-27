using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/PaperYeti/Scene/TowerDefenseZombiePaperYeti.cs")]
public class TowerDefenseZombiePaperYeti : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName GaspEntered = "GaspEntered";

		public static readonly StringName GaspProcessing = "GaspProcessing";

		public static readonly StringName GaspExited = "GaspExited";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName StartFleeing = "StartFleeing";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public static readonly StringName RefreshFleeDirection = "RefreshFleeDirection";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName HasEscapedScreen = "HasEscapedScreen";

		public static readonly StringName RespawnNewspaperYeti = "RespawnNewspaperYeti";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _headFollowSlotIdBeforeGasp = "_headFollowSlotIdBeforeGasp";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _fleeing = "_fleeing";

		public static readonly StringName _respawned = "_respawned";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const int GaspHeadFollowSlotId = 18;

	private const double FleeTimeScale = 4.0;

	private const string FleeWalkClip = "AngryWalk";

	private const string FleeAttackClip = "AngryEat";

	private int _headFollowSlotIdBeforeGasp = -1;

	private StateHandle _gaspStateHandle;

	private bool _stateSignalsConnected;

	private bool _fleeing;

	private bool _respawned;

	private const double InitialTimeScale = 0.6;

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_gaspStateHandle = StateMachine?.GetStateById("zombie.paper_yeti.gasp");
			StateHandle gaspStateHandle = _gaspStateHandle;
			if (gaspStateHandle != null && gaspStateHandle.IsValid)
			{
				_gaspStateHandle.Entered += GaspEntered;
				_gaspStateHandle.Exited += GaspExited;
				_gaspStateHandle.PhysicsProcessing += GaspProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_gaspStateHandle != null)
			{
				_gaspStateHandle.Entered -= GaspEntered;
				_gaspStateHandle.Exited -= GaspExited;
				_gaspStateHandle.PhysicsProcessing -= GaspProcessing;
			}
			_gaspStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			timeScaleInit = 0.6;
			ConnectStateSignals();
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void GaspEntered()
	{
		if (GodotObject.IsInstanceValid(headSlot))
		{
			if (_headFollowSlotIdBeforeGasp < 0)
			{
				_headFollowSlotIdBeforeGasp = headSlot.followSlotId;
			}
			headSlot.followSlotId = 18;
		}
		sprite.SetAnimation("Gasp", loop: false);
	}

	public virtual void GaspProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void GaspExited()
	{
		if (GodotObject.IsInstanceValid(headSlot) && _headFollowSlotIdBeforeGasp >= 0)
		{
			headSlot.followSlotId = _headFollowSlotIdBeforeGasp;
		}
		_headFollowSlotIdBeforeGasp = -1;
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Paper")
		{
			SendStateEvent("ToGasp");
			AudioManager.Instance.AudioPlay("NewspaperRip");
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Gasp")
		{
			AudioManager.Instance.AudioPlay("NewspaperRarrgh");
			StartFleeing();
		}
	}

	private void StartFleeing()
	{
		_fleeing = true;
		timeScaleInit = 4.0;
		walkAnimeClip = "AngryWalk";
		swimAnimeClip = "AngryWalk";
		attackAnimeClip = "AngryEat";
		attackWaterAnimeClip = "AngryEat";
		RefreshFleeDirection();
		Walk();
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		if (_fleeing)
		{
			RefreshFleeDirection();
		}
	}

	private void RefreshFleeDirection()
	{
		float num = (instance.hypnoses ? 1f : (-1f));
		Scale = new Vector2(num * Mathf.Abs(Scale.X), Scale.Y);
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.NotifyAncestorTransformChangedForRender();
		}
		GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			base.groundMoveComponent.RefreshDirectionCache();
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (_fleeing)
		{
			SwimComponent swimComponent = base.swimComponent;
			if (swimComponent != null && !swimComponent.IsReleased)
			{
				swimComponent.WalkProcessing((float)delta);
			}
			sprite.timeScale = timeScale;
			if (HasEscapedScreen())
			{
				RespawnNewspaperYeti();
				Destroy();
			}
		}
		else
		{
			base.WalkProcessing(delta);
		}
	}

	public override async void DestroySet()
	{
		if (_fleeing && !_respawned && HasEscapedScreen())
		{
			RespawnNewspaperYeti();
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	private bool HasEscapedScreen()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return false;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		if (!instance.hypnoses)
		{
			return (double)logicalGlobalPosition.X > TowerDefenseManager.Instance.GetMapGroundRight() + 150.0;
		}
		return (double)logicalGlobalPosition.X < TowerDefenseManager.Instance.GetMapGroundLeft() - 150.0;
	}

	private void RespawnNewspaperYeti()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			return;
		}
		_respawned = true;
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombiePaperYeti");
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(transformPoint))
		{
			return;
		}
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		int num = gridPos.Y;
		if (mapGridNum.Y > 1)
		{
			int num2 = GD.RandRange(1, mapGridNum.Y);
			if (num2 == gridPos.Y)
			{
				num2 = ((num2 >= mapGridNum.Y) ? (mapGridNum.Y - 1) : (num2 + 1));
			}
			num = Mathf.Clamp(num2, 1, mapGridNum.Y);
		}
		bool hypnoses = instance.hypnoses;
		float x = (float)(hypnoses ? (TowerDefenseManager.Instance.GetMapGroundLeft() - 100.0) : (TowerDefenseManager.Instance.GetMapGroundRight() + 100.0));
		float y = (float)TowerDefenseManager.GetMapLineY(num);
		Vector2 pos = new Vector2(x, y);
		Vector2I vector2I = new Vector2I((!hypnoses) ? (mapGridNum.X + 1) : 0, num);
		TowerDefenseCharacter zombie = (EconomyOwnerAccountId.IsValid ? packetConfig.Create(EconomyOwnerAccountId, pos, vector2I) : packetConfig.Create(pos, vector2I));
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return;
		}
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", zombie);
		double hitpointScale = instance.hitpointScale;
		double remainingHitpoints = instance.hitpoints;
		Vector2 scale = transformPoint.Scale;
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				if (GodotObject.IsInstanceValid(zombie.instance))
				{
					zombie.instance.hitpointScale = hitpointScale;
					zombie.instance.hitpoints = Math.Min(remainingHitpoints, zombie.instance.hitpointsSave);
					zombie.instance.RefreshDamagePoint();
					zombie.showHealthComponent?.MarkDirty();
				}
				if (GodotObject.IsInstanceValid(zombie.transformPoint))
				{
					zombie.transformPoint.Scale = scale;
				}
			}
		}).CallDeferred();
		zombie.SetDeferred("invisible", invisible);
		if (hypnoses)
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(zombie))
				{
					zombie.Hypnoses();
				}
			}).CallDeferred();
		}
		zombie.CallDeferred("WalkReady");
		Dictionary spawnState = new Dictionary
		{
			["hypnoses"] = hypnoses,
			["spawn_invisible"] = invisible,
			["walk_ready_after_spawn"] = true
		};
		TowerDefenseManager.PublishSpawnedCharacter("ZombiePaperYeti", zombie, useCreate: true, 0.0, walkAfterSpawn: false, "", spawnState);
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "fleeing", _fleeing },
			{ "respawned", _respawned }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_fleeing = data.GetValueOrDefault("fleeing", false).AsBool();
		_respawned = data.GetValueOrDefault("respawned", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GaspProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartFleeing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFleeDirection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasEscapedScreen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RespawnNewspaperYeti, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GaspEntered && args.Count == 0)
		{
			GaspEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GaspProcessing && args.Count == 1)
		{
			GaspProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GaspExited && args.Count == 0)
		{
			GaspExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartFleeing && args.Count == 0)
		{
			StartFleeing();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFleeDirection && args.Count == 0)
		{
			RefreshFleeDirection();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.HasEscapedScreen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEscapedScreen());
			return true;
		}
		if (method == MethodName.RespawnNewspaperYeti && args.Count == 0)
		{
			RespawnNewspaperYeti();
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.GaspEntered)
		{
			return true;
		}
		if (method == MethodName.GaspProcessing)
		{
			return true;
		}
		if (method == MethodName.GaspExited)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.StartFleeing)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshFleeDirection)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.HasEscapedScreen)
		{
			return true;
		}
		if (method == MethodName.RespawnNewspaperYeti)
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
		if (name == PropertyName._headFollowSlotIdBeforeGasp)
		{
			_headFollowSlotIdBeforeGasp = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fleeing)
		{
			_fleeing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._respawned)
		{
			_respawned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._headFollowSlotIdBeforeGasp)
		{
			value = VariantUtils.CreateFrom(in _headFollowSlotIdBeforeGasp);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._fleeing)
		{
			value = VariantUtils.CreateFrom(in _fleeing);
			return true;
		}
		if (name == PropertyName._respawned)
		{
			value = VariantUtils.CreateFrom(in _respawned);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._headFollowSlotIdBeforeGasp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._fleeing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._respawned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._headFollowSlotIdBeforeGasp, Variant.From(in _headFollowSlotIdBeforeGasp));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._fleeing, Variant.From(in _fleeing));
		info.AddProperty(PropertyName._respawned, Variant.From(in _respawned));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._headFollowSlotIdBeforeGasp, out var value))
		{
			_headFollowSlotIdBeforeGasp = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value2))
		{
			_stateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fleeing, out var value3))
		{
			_fleeing = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._respawned, out var value4))
		{
			_respawned = value4.As<bool>();
		}
	}
}
