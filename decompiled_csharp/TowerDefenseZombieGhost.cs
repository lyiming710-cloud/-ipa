using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter8/Ghost/Scene/TowerDefenseZombieGhost.cs")]
public class TowerDefenseZombieGhost : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	private struct GhostOverlapSelector(TowerDefenseZombieGhost owner, TowerDefenseZombie currentTarget, bool needsTarget, bool needsCarrier) : ITowerDefenseCharacterRectSelector
	{
		private readonly TowerDefenseZombieGhost _owner = owner;

		private readonly TowerDefenseZombie _currentTarget = currentTarget;

		private readonly bool _needsTarget = needsTarget;

		private readonly bool _needsCarrier = needsCarrier;

		private bool _candidateIsTarget = false;

		private bool _candidateIsCarrier = false;

		public TowerDefenseZombie FirstTarget { get; private set; } = null;

		public bool CurrentTargetFound { get; private set; } = false;

		public TowerDefenseZombie Carrier { get; private set; } = null;

		public bool CanConsider(TowerDefenseCharacter character)
		{
			_candidateIsTarget = false;
			_candidateIsCarrier = false;
			if (!(character is TowerDefenseZombie towerDefenseZombie) || towerDefenseZombie == _owner)
			{
				return false;
			}
			_candidateIsTarget = _needsTarget && towerDefenseZombie.config.name == _owner.config.name && towerDefenseZombie.camp != _owner.camp;
			_candidateIsCarrier = _needsCarrier && _owner.CanCarryZombie(towerDefenseZombie);
			if (!_candidateIsTarget)
			{
				return _candidateIsCarrier;
			}
			return true;
		}

		public bool Visit(TowerDefenseCharacter character, ref TowerDefenseCharacter match)
		{
			TowerDefenseZombie towerDefenseZombie = (TowerDefenseZombie)character;
			if (_candidateIsTarget)
			{
				if (FirstTarget == null)
				{
					TowerDefenseZombie towerDefenseZombie2 = (FirstTarget = towerDefenseZombie);
				}
				if (towerDefenseZombie == _currentTarget)
				{
					CurrentTargetFound = true;
				}
			}
			if (_candidateIsCarrier && Carrier == null)
			{
				TowerDefenseZombie towerDefenseZombie2 = (Carrier = towerDefenseZombie);
			}
			bool num = !_needsTarget || (GodotObject.IsInstanceValid(_currentTarget) ? CurrentTargetFound : GodotObject.IsInstanceValid(FirstTarget));
			bool flag = !_needsCarrier || GodotObject.IsInstanceValid(Carrier);
			if (!num || !flag)
			{
				return false;
			}
			ref TowerDefenseCharacter reference = ref match;
			TowerDefenseZombie towerDefenseZombie5;
			if (GodotObject.IsInstanceValid(Carrier))
			{
				towerDefenseZombie5 = Carrier;
			}
			else
			{
				towerDefenseZombie5 = (GodotObject.IsInstanceValid(_currentTarget) ? _currentTarget : FirstTarget);
			}
			reference = towerDefenseZombie5;
			return true;
		}
	}

	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ApplyGhostPresentation = "ApplyGhostPresentation";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public static readonly StringName QueueNetworkCarrier = "QueueNetworkCarrier";

		public static readonly StringName ResolvePendingCarrier = "ResolvePendingCarrier";

		public static readonly StringName ResolvePendingProgressCarrier = "ResolvePendingProgressCarrier";

		public static readonly StringName ProcessHitBoxOverlaps = "ProcessHitBoxOverlaps";

		public static readonly StringName CanCarryZombie = "CanCarryZombie";

		public static readonly StringName Carry = "Carry";

		public static readonly StringName DetachGhost = "DetachGhost";

		public static readonly StringName AttachCarrierRelation = "AttachCarrierRelation";

		public static readonly StringName ApplyCarrierPresentation = "ApplyCarrierPresentation";

		public static readonly StringName UpdateCarrierPresentation = "UpdateCarrierPresentation";

		public static readonly StringName DetachCarrierRelation = "DetachCarrierRelation";

		public static readonly StringName _ReconnectHitBox = "_ReconnectHitBox";

		public static readonly StringName CheckGhost = "CheckGhost";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public static readonly StringName TryResolveLegacyCarrier = "TryResolveLegacyCarrier";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName ghost = "ghost";

		public static readonly StringName carryCharacter = "carryCharacter";

		public static readonly StringName ghostTimeScaleSave = "ghostTimeScaleSave";

		public static readonly StringName _pendingCarrierSyncId = "_pendingCarrierSyncId";

		public static readonly StringName _pendingCarrierBaseTimeScale = "_pendingCarrierBaseTimeScale";

		public static readonly StringName _pendingCarrierNodeName = "_pendingCarrierNodeName";

		public static readonly StringName _carrierRelationApplied = "_carrierRelationApplied";

		public static readonly StringName _suppressAutomaticCarrierSearch = "_suppressAutomaticCarrierSearch";

		public static readonly StringName _ghost = "_ghost";

		public static readonly StringName _ghostVisualApplied = "_ghostVisualApplied";

		public static readonly StringName _ghostOpaqueAlpha = "_ghostOpaqueAlpha";

		public static readonly StringName timer = "timer";

		public static readonly StringName time = "time";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public TowerDefenseZombie carryCharacter;

	public double ghostTimeScaleSave = 1.0;

	private int _pendingCarrierSyncId = -1;

	private double _pendingCarrierBaseTimeScale = 0.0 / 0.0;

	private string _pendingCarrierNodeName = "";

	private bool _carrierRelationApplied;

	private bool _suppressAutomaticCarrierSearch;

	private bool _ghost = true;

	private bool _ghostVisualApplied;

	private float _ghostOpaqueAlpha = 1f;

	public double timer;

	public double time = 40.0;

	public bool ghost
	{
		get
		{
			return _ghost;
		}
		set
		{
			_ghost = value;
			ApplyGhostPresentation();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode)
		{
			return;
		}
		targetRegistrationComponent.canCarry = false;
		ghost = true;
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			if (_carrierRelationApplied)
			{
				ApplyCarrierPresentation();
			}
			else
			{
				Carry(carryCharacter);
			}
		}
		else
		{
			ResolvePendingCarrier();
			ResolvePendingProgressCarrier();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint())
		{
			return;
		}
		ApplyGhostPresentation();
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) || !TowerDefenseManager.Instance.currentControl.isGameRunning || !inGame)
		{
			return;
		}
		ResolvePendingCarrier();
		ResolvePendingProgressCarrier();
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			UpdateCarrierPresentation();
		}
		if (IsRemoteNetworkReplica || die || nearDie)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			if (carryCharacter.nearDie || carryCharacter.die)
			{
				Die();
				return;
			}
			if (carryCharacter.isDestroy)
			{
				float x = carryCharacter.GetLogicalGlobalPosition().X;
				if ((double)x > carryCharacter.groundRight + 150.0 || (double)x < TowerDefenseManager.Instance.GetMapGroundLeft() - 150.0)
				{
					DetachGhost();
				}
				else
				{
					Die();
				}
				return;
			}
		}
		else if (carryCharacter != null)
		{
			DetachGhost();
			return;
		}
		if (!IsInsideComponentBattlefield)
		{
			return;
		}
		bool flag = CheckGhost();
		if (ghost != flag)
		{
			ghost = flag;
		}
		ProcessHitBoxOverlaps();
		if (instance.hypnoses && !sprite.pause)
		{
			if (timer < time)
			{
				timer += delta * timeScale;
			}
			else if (!GodotObject.IsInstanceValid(carryCharacter))
			{
				Die();
			}
		}
	}

	private void ApplyGhostPresentation()
	{
		if (GodotObject.IsInstanceValid(sprite) && GodotObject.IsInstanceValid(instance))
		{
			if (!_ghostVisualApplied)
			{
				_ghostOpaqueAlpha = sprite.meshColor.A;
			}
			Color meshColor = sprite.meshColor;
			meshColor.A = (_ghost ? (_ghostOpaqueAlpha * 0.5f) : _ghostOpaqueAlpha);
			sprite.meshColor = meshColor;
			instance.canBeCollection = !_ghost;
			TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
			if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
			{
				base.targetRegistrationComponent.canProjectileCheck = !_ghost;
			}
			if (!_ghost)
			{
				instance.maskFlags = 9;
			}
			_ghostVisualApplied = true;
		}
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		if (ghost && !nearDie && !sprite.pause && sprite.timeScale > 0.0 && useAttackDps && attackComponent.target != null)
		{
			attackComponent.AttackDpsExecute(delta, ((TowerDefenseZombieConfig)config).attack);
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		int num = (GodotObject.IsInstanceValid(carryCharacter) ? carryCharacter.syncId : _pendingCarrierSyncId);
		return new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["ghost"] = ghost,
			["carrierSyncId"] = num,
			["ghostTimeScaleSave"] = ghostTimeScaleSave
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ghost = data.GetValueOrDefault("ghost", ghost).AsBool();
		QueueNetworkCarrier(data);
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (data != null)
		{
			QueueNetworkCarrier(data);
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		int num = (GodotObject.IsInstanceValid(carryCharacter) ? carryCharacter.syncId : _pendingCarrierSyncId);
		return (int)((ghost ? 1u : 0u) ^ (uint)(num * 397)) ^ (Mathf.RoundToInt(ghostTimeScaleSave * 1000.0) * 31);
	}

	private void QueueNetworkCarrier(Dictionary data)
	{
		int num = data.GetValueOrDefault("carrierSyncId", -1).AsInt32();
		if (num < 0)
		{
			_pendingCarrierSyncId = -1;
			_pendingCarrierBaseTimeScale = 0.0 / 0.0;
			DetachCarrierRelation();
		}
		else
		{
			_pendingCarrierSyncId = num;
			_pendingCarrierBaseTimeScale = data.GetValueOrDefault("ghostTimeScaleSave", ghostTimeScaleSave).AsDouble();
			ResolvePendingCarrier();
		}
	}

	private void ResolvePendingCarrier()
	{
		if (_pendingCarrierSyncId >= 0)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl) && currentControl._syncCharacters.TryGetValue(_pendingCarrierSyncId, out var value) && value is TowerDefenseZombie carrier && GodotObject.IsInstanceValid(carrier))
			{
				double baseTimeScale = (double.IsNaN(_pendingCarrierBaseTimeScale) ? ghostTimeScaleSave : _pendingCarrierBaseTimeScale);
				_pendingCarrierSyncId = -1;
				_pendingCarrierBaseTimeScale = 0.0 / 0.0;
				AttachCarrierRelation(carrier, baseTimeScale);
			}
		}
	}

	private void ResolvePendingProgressCarrier()
	{
		if (string.IsNullOrEmpty(_pendingCarrierNodeName))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(node2D))
		{
			TowerDefenseZombie nodeOrNull = node2D.GetNodeOrNull<TowerDefenseZombie>(new NodePath(_pendingCarrierNodeName));
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				_pendingCarrierNodeName = "";
				AttachCarrierRelation(nodeOrNull, ghostTimeScaleSave);
			}
		}
	}

	private void ProcessHitBoxOverlaps()
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) || !TowerDefenseManager.Instance.currentControl.isGameRunning || !inGame || nearDie || die)
		{
			return;
		}
		TowerDefenseBattleCharacterRegistry characterRegistry = TowerDefenseManager.Instance.characterRegistry;
		bool flag = ghost && characterRegistry.HasCharacterOutsideCampWithConfigName(camp, config.name);
		bool flag2 = !_suppressAutomaticCarrierSearch && !GodotObject.IsInstanceValid(carryCharacter) && characterRegistry.HasCharacterInCampWithDifferentConfigName(camp, config.name);
		Rect2 rect;
		if (!flag && !flag2)
		{
			attackComponent.target = null;
		}
		else if (TryGetActiveWorldHitRect(out rect))
		{
			TowerDefenseZombie towerDefenseZombie = attackComponent.target as TowerDefenseZombie;
			if (!GodotObject.IsInstanceValid(towerDefenseZombie) || towerDefenseZombie.config.name != config.name || towerDefenseZombie.camp == camp)
			{
				towerDefenseZombie = null;
			}
			GhostOverlapSelector selector = new GhostOverlapSelector(this, towerDefenseZombie, flag, flag2);
			characterRegistry.TrySelectCharacterIntersectingRectClassified(rect, camp, config.name, flag, flag2, ref selector, out var _);
			if (flag2 && GodotObject.IsInstanceValid(selector.Carrier))
			{
				Carry(selector.Carrier);
			}
			ref TowerDefenseCharacter target = ref attackComponent.target;
			TowerDefenseCharacter towerDefenseCharacter;
			if (ghost)
			{
				towerDefenseCharacter = (selector.CurrentTargetFound ? towerDefenseZombie : selector.FirstTarget);
			}
			else
			{
				towerDefenseCharacter = null;
			}
			target = towerDefenseCharacter;
		}
	}

	private bool CanCarryZombie(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return false;
		}
		if (zombie.isRise)
		{
			return false;
		}
		if (zombie.config.name == config.name)
		{
			return false;
		}
		if (zombie.hasGhost)
		{
			return false;
		}
		if ((zombie.instance.maskFlags & 0x10) != 0 && (zombie.instance.maskFlags & 1) == 0)
		{
			return false;
		}
		if (zombie.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			return false;
		}
		if (zombie.camp != camp)
		{
			return false;
		}
		if (!zombie.targetRegistrationComponent.canCarry)
		{
			return false;
		}
		return true;
	}

	public void Carry(TowerDefenseZombie character)
	{
		if (GodotObject.IsInstanceValid(character) && (!_carrierRelationApplied || carryCharacter != character))
		{
			double baseTimeScale = character.timeScaleInit;
			AttachCarrierRelation(character, baseTimeScale);
			character.Health(instance.hitpoints);
		}
	}

	public void DetachGhost()
	{
		DetachCarrierRelation();
	}

	private void AttachCarrierRelation(TowerDefenseZombie carrier, double baseTimeScale)
	{
		if (GodotObject.IsInstanceValid(carrier))
		{
			if (GodotObject.IsInstanceValid(carryCharacter) && carryCharacter != carrier)
			{
				DetachCarrierRelation(reconnectHitBox: false);
			}
			carryCharacter = carrier;
			ghostTimeScaleSave = ((baseTimeScale > 0.0) ? baseTimeScale : 1.0);
			carryCharacter.hasGhost = true;
			carryCharacter.ghostCharacter = this;
			carryCharacter.timeScaleInit = ghostTimeScaleSave * 2.0;
			_carrierRelationApplied = true;
			_suppressAutomaticCarrierSearch = false;
			UpdateCarrierPresentation();
		}
	}

	private void ApplyCarrierPresentation()
	{
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			groundHeight = carryCharacter.groundHeight;
			z = 40.0;
		}
	}

	private void UpdateCarrierPresentation()
	{
		ApplyCarrierPresentation();
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			ulong num = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (num == 18446744073709551615uL)
			{
				num = Engine.GetPhysicsFrames();
			}
			Vector2 globalPositionForPhysicsFrame = carryCharacter.GetGlobalPositionForPhysicsFrame(num);
			float x = ((GodotObject.IsInstanceValid(carryCharacter.instance) && carryCharacter.instance.hypnoses) ? (-5f) : 5f);
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame + new Vector2(x, 0f), num);
			gridPos = new Vector2I(gridPos.X, carryCharacter.gridPos.Y);
		}
	}

	private void DetachCarrierRelation(bool reconnectHitBox = true)
	{
		if (GodotObject.IsInstanceValid(carryCharacter) && (carryCharacter.ghostCharacter == this || !GodotObject.IsInstanceValid(carryCharacter.ghostCharacter)))
		{
			carryCharacter.timeScaleInit = ghostTimeScaleSave;
			carryCharacter.hasGhost = false;
			carryCharacter.ghostCharacter = null;
		}
		carryCharacter = null;
		_carrierRelationApplied = false;
		_pendingCarrierSyncId = -1;
		_pendingCarrierBaseTimeScale = 0.0 / 0.0;
		groundHeight = 0.0;
		z = 0.0;
		if (reconnectHitBox)
		{
			_ReconnectHitBox();
		}
	}

	public void _ReconnectHitBox()
	{
		if (HasHitBox)
		{
			SetHitBoxEnabled(enabled: true);
			if (!IsRemoteNetworkReplica)
			{
				ProcessHitBoxOverlaps();
			}
		}
	}

	public bool CheckGhost()
	{
		bool flag = true;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + new Vector2I(i, j));
				if (GodotObject.IsInstanceValid(mapCell) && mapCell.HasLight())
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		DetachCarrierRelation();
	}

	public override async void DestroySet()
	{
		DetachCarrierRelation(reconnectHitBox: false);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			{ "ghost", ghost },
			{ "ghostTimeScaleSave", ghostTimeScaleSave },
			{ "timer", timer },
			{ "time", time },
			{ "carrierRelationVersion", 1 }
		};
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			dictionary["carryCharacterNodeName"] = carryCharacter.Name;
		}
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		ghost = data.GetValueOrDefault("ghost", true).AsBool();
		ghostTimeScaleSave = data.GetValueOrDefault("ghostTimeScaleSave", 1.0).AsDouble();
		timer = data.GetValueOrDefault("timer", timer).AsDouble();
		time = data.GetValueOrDefault("time", time).AsDouble();
		_pendingCarrierNodeName = data.GetValueOrDefault("carryCharacterNodeName", "").AsString();
		_suppressAutomaticCarrierSearch = !data.ContainsKey("carrierRelationVersion") && string.IsNullOrEmpty(_pendingCarrierNodeName);
		ResolvePendingProgressCarrier();
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		ResolvePendingProgressCarrier();
		if (_suppressAutomaticCarrierSearch)
		{
			CallDeferred("TryResolveLegacyCarrier");
		}
	}

	private void TryResolveLegacyCarrier()
	{
		if (!_suppressAutomaticCarrierSearch || GodotObject.IsInstanceValid(carryCharacter))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		TowerDefenseZombie carrier = null;
		foreach (Node child in node2D.GetChildren())
		{
			if (child is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie != this && towerDefenseZombie.hasGhost && !GodotObject.IsInstanceValid(towerDefenseZombie.ghostCharacter) && towerDefenseZombie.camp == camp && Mathf.IsEqualApprox(towerDefenseZombie.timeScaleInit, ghostTimeScaleSave * 2.0))
			{
				if (GodotObject.IsInstanceValid(carrier))
				{
					return;
				}
				carrier = towerDefenseZombie;
			}
		}
		if (GodotObject.IsInstanceValid(carrier))
		{
			AttachCarrierRelation(carrier, ghostTimeScaleSave);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(28)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyGhostPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueNetworkCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePendingCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePendingProgressCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessHitBoxOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCarryZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Carry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DetachGhost, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttachCarrierRelation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "carrier", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "baseTimeScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCarrierPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCarrierPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetachCarrierRelation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "reconnectHitBox", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ReconnectHitBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckGhost, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryResolveLegacyCarrier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyGhostPresentation && args.Count == 0)
		{
			ApplyGhostPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
			return true;
		}
		if (method == MethodName.QueueNetworkCarrier && args.Count == 1)
		{
			QueueNetworkCarrier(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingCarrier && args.Count == 0)
		{
			ResolvePendingCarrier();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingProgressCarrier && args.Count == 0)
		{
			ResolvePendingProgressCarrier();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps && args.Count == 0)
		{
			ProcessHitBoxOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.CanCarryZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCarryZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.Carry && args.Count == 1)
		{
			Carry(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachGhost && args.Count == 0)
		{
			DetachGhost();
			ret = default;
			return true;
		}
		if (method == MethodName.AttachCarrierRelation && args.Count == 2)
		{
			AttachCarrierRelation(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCarrierPresentation && args.Count == 0)
		{
			ApplyCarrierPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCarrierPresentation && args.Count == 0)
		{
			UpdateCarrierPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.DetachCarrierRelation && args.Count == 1)
		{
			DetachCarrierRelation(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ReconnectHitBox && args.Count == 0)
		{
			_ReconnectHitBox();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckGhost && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckGhost());
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
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
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.TryResolveLegacyCarrier && args.Count == 0)
		{
			TryResolveLegacyCarrier();
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ApplyGhostPresentation)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		if (method == MethodName.QueueNetworkCarrier)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingCarrier)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingProgressCarrier)
		{
			return true;
		}
		if (method == MethodName.ProcessHitBoxOverlaps)
		{
			return true;
		}
		if (method == MethodName.CanCarryZombie)
		{
			return true;
		}
		if (method == MethodName.Carry)
		{
			return true;
		}
		if (method == MethodName.DetachGhost)
		{
			return true;
		}
		if (method == MethodName.AttachCarrierRelation)
		{
			return true;
		}
		if (method == MethodName.ApplyCarrierPresentation)
		{
			return true;
		}
		if (method == MethodName.UpdateCarrierPresentation)
		{
			return true;
		}
		if (method == MethodName.DetachCarrierRelation)
		{
			return true;
		}
		if (method == MethodName._ReconnectHitBox)
		{
			return true;
		}
		if (method == MethodName.CheckGhost)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
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
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.TryResolveLegacyCarrier)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ghost)
		{
			ghost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			carryCharacter = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		if (name == PropertyName.ghostTimeScaleSave)
		{
			ghostTimeScaleSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingCarrierSyncId)
		{
			_pendingCarrierSyncId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pendingCarrierBaseTimeScale)
		{
			_pendingCarrierBaseTimeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingCarrierNodeName)
		{
			_pendingCarrierNodeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._carrierRelationApplied)
		{
			_carrierRelationApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._suppressAutomaticCarrierSearch)
		{
			_suppressAutomaticCarrierSearch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ghost)
		{
			_ghost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ghostVisualApplied)
		{
			_ghostVisualApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ghostOpaqueAlpha)
		{
			_ghostOpaqueAlpha = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ghost)
		{
			value = VariantUtils.CreateFrom<bool>(ghost);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			value = VariantUtils.CreateFrom(in carryCharacter);
			return true;
		}
		if (name == PropertyName.ghostTimeScaleSave)
		{
			value = VariantUtils.CreateFrom(in ghostTimeScaleSave);
			return true;
		}
		if (name == PropertyName._pendingCarrierSyncId)
		{
			value = VariantUtils.CreateFrom(in _pendingCarrierSyncId);
			return true;
		}
		if (name == PropertyName._pendingCarrierBaseTimeScale)
		{
			value = VariantUtils.CreateFrom(in _pendingCarrierBaseTimeScale);
			return true;
		}
		if (name == PropertyName._pendingCarrierNodeName)
		{
			value = VariantUtils.CreateFrom(in _pendingCarrierNodeName);
			return true;
		}
		if (name == PropertyName._carrierRelationApplied)
		{
			value = VariantUtils.CreateFrom(in _carrierRelationApplied);
			return true;
		}
		if (name == PropertyName._suppressAutomaticCarrierSearch)
		{
			value = VariantUtils.CreateFrom(in _suppressAutomaticCarrierSearch);
			return true;
		}
		if (name == PropertyName._ghost)
		{
			value = VariantUtils.CreateFrom(in _ghost);
			return true;
		}
		if (name == PropertyName._ghostVisualApplied)
		{
			value = VariantUtils.CreateFrom(in _ghostVisualApplied);
			return true;
		}
		if (name == PropertyName._ghostOpaqueAlpha)
		{
			value = VariantUtils.CreateFrom(in _ghostOpaqueAlpha);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.carryCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.ghostTimeScaleSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingCarrierSyncId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingCarrierBaseTimeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingCarrierNodeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._carrierRelationApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suppressAutomaticCarrierSearch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ghost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ghostVisualApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._ghostOpaqueAlpha, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ghost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ghost, Variant.From<bool>(ghost));
		info.AddProperty(PropertyName.carryCharacter, Variant.From(in carryCharacter));
		info.AddProperty(PropertyName.ghostTimeScaleSave, Variant.From(in ghostTimeScaleSave));
		info.AddProperty(PropertyName._pendingCarrierSyncId, Variant.From(in _pendingCarrierSyncId));
		info.AddProperty(PropertyName._pendingCarrierBaseTimeScale, Variant.From(in _pendingCarrierBaseTimeScale));
		info.AddProperty(PropertyName._pendingCarrierNodeName, Variant.From(in _pendingCarrierNodeName));
		info.AddProperty(PropertyName._carrierRelationApplied, Variant.From(in _carrierRelationApplied));
		info.AddProperty(PropertyName._suppressAutomaticCarrierSearch, Variant.From(in _suppressAutomaticCarrierSearch));
		info.AddProperty(PropertyName._ghost, Variant.From(in _ghost));
		info.AddProperty(PropertyName._ghostVisualApplied, Variant.From(in _ghostVisualApplied));
		info.AddProperty(PropertyName._ghostOpaqueAlpha, Variant.From(in _ghostOpaqueAlpha));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.time, Variant.From(in time));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ghost, out var value))
		{
			ghost = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.carryCharacter, out var value2))
		{
			carryCharacter = value2.As<TowerDefenseZombie>();
		}
		if (info.TryGetProperty(PropertyName.ghostTimeScaleSave, out var value3))
		{
			ghostTimeScaleSave = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingCarrierSyncId, out var value4))
		{
			_pendingCarrierSyncId = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pendingCarrierBaseTimeScale, out var value5))
		{
			_pendingCarrierBaseTimeScale = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingCarrierNodeName, out var value6))
		{
			_pendingCarrierNodeName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._carrierRelationApplied, out var value7))
		{
			_carrierRelationApplied = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._suppressAutomaticCarrierSearch, out var value8))
		{
			_suppressAutomaticCarrierSearch = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ghost, out var value9))
		{
			_ghost = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ghostVisualApplied, out var value10))
		{
			_ghostVisualApplied = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ghostOpaqueAlpha, out var value11))
		{
			_ghostOpaqueAlpha = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value12))
		{
			timer = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value13))
		{
			time = value13.As<double>();
		}
	}
}
