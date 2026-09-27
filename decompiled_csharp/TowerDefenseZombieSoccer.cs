using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Soccer/Scene/TowerDefenseZombieSoccer.cs")]
public class TowerDefenseZombieSoccer : TowerDefenseZombie
{
	private struct IdleSoccerBallPredicate : ITowerDefenseCharacterRectPredicate, ITowerDefenseCharacterCandidatePredicate
	{
		public TowerDefenseEnum.CHARACTER_CAMP Camp;

		public bool CanConsider(TowerDefenseCharacter candidate)
		{
			if (candidate is TowerDefenseItemSoccerBall ball)
			{
				return IsKickableSameCampBall(ball, Camp);
			}
			return false;
		}

		public bool Matches(TowerDefenseCharacter candidate)
		{
			return true;
		}
	}

	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName GetPutTimerScale = "GetPutTimerScale";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnTimerTimeout = "OnTimerTimeout";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public static readonly StringName StartPut = "StartPut";

		public static readonly StringName StartKick = "StartKick";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName FindIdleSoccerBallInActionArea = "FindIdleSoccerBallInActionArea";

		public static readonly StringName IsKickableSameCampBall = "IsKickableSameCampBall";

		public static readonly StringName PlaceHeldBall = "PlaceHeldBall";

		public static readonly StringName ApplyNoBallVisuals = "ApplyNoBallVisuals";

		public static readonly StringName SpawnSoccerBall = "SpawnSoccerBall";

		public static readonly StringName ApplyOwnerStateToSoccerBall = "ApplyOwnerStateToSoccerBall";

		public static readonly StringName KickSoccerBall = "KickSoccerBall";

		public static readonly StringName GetKickDirection = "GetKickDirection";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName hasBall = "hasBall";

		public static readonly StringName isPutting = "isPutting";

		public static readonly StringName isKicking = "isKicking";

		public static readonly StringName _wantPut = "_wantPut";

		public static readonly StringName _ballPlacedDuringPut = "_ballPlacedDuringPut";

		public static readonly StringName _kickTarget = "_kickTarget";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const string ClipPut = "Put";

	private const string ClipKick = "Kick";

	private const string TimerPut = "Put";

	public bool hasBall = true;

	private bool isPutting;

	private bool isKicking;

	private bool _wantPut;

	private bool _ballPlacedDuringPut;

	private CharacterTimerComponent _timerComponent;

	private AttackComponent _attackComponent2;

	private TowerDefenseItemSoccerBall _kickTarget;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			_timerComponent.OnTimeout += OnTimerTimeout;
			_timerComponent.Run("Put");
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			CharacterTimerComponent timerComponent = _timerComponent;
			if (timerComponent != null && !timerComponent.IsReleased)
			{
				_timerComponent.timeScale = GetPutTimerScale();
			}
		}
	}

	private double GetPutTimerScale()
	{
		if (die || !hasBall || isPutting || isKicking)
		{
			return 0.0;
		}
		if (GodotObject.IsInstanceValid(sprite) && sprite.pause)
		{
			return 0.0;
		}
		return Math.Max(0.0, timeScale);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= OnTimerTimeout;
		}
	}

	private void OnTimerTimeout(string timerName)
	{
		if (!die && hasBall && !isPutting && !isKicking && timerName == "Put")
		{
			_wantPut = true;
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (isPutting || isKicking || die)
		{
			return;
		}
		bool flag = false;
		if (hasBall && (_wantPut || FindIdleSoccerBallInActionArea() != null))
		{
			_wantPut = false;
			StartPut();
			flag = true;
		}
		else if (!hasBall)
		{
			TowerDefenseItemSoccerBall towerDefenseItemSoccerBall = FindIdleSoccerBallInActionArea();
			if (towerDefenseItemSoccerBall != null)
			{
				StartKick(towerDefenseItemSoccerBall);
				flag = true;
			}
		}
		if (!flag)
		{
			base.WalkProcessing(delta);
		}
	}

	private void StartPut()
	{
		isPutting = true;
		_ballPlacedDuringPut = false;
		GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			base.groundMoveComponent.SetAlive(false);
		}
		sprite.SetAnimation("Put", loop: false, 0.2);
	}

	private void StartKick(TowerDefenseItemSoccerBall target)
	{
		isKicking = true;
		_kickTarget = target;
		GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			base.groundMoveComponent.SetAlive(false);
		}
		sprite.SetAnimation("Kick", loop: false, 0.2);
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!(command == "kick"))
		{
			if (command == "put")
			{
				PlaceHeldBall();
			}
		}
		else
		{
			KickSoccerBall(_kickTarget);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (die)
		{
			return;
		}
		if (clip == "Put")
		{
			PlaceHeldBall();
			isPutting = false;
			GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
			if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
			{
				base.groundMoveComponent.SetAlive(true);
			}
			sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
		}
		else if (clip == "Kick")
		{
			_kickTarget = null;
			isKicking = false;
			GroundMoveComponent groundMoveComponent2 = base.groundMoveComponent;
			if (groundMoveComponent2 != null && !groundMoveComponent2.IsReleased)
			{
				base.groundMoveComponent.SetAlive(true);
			}
			sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
		}
	}

	private TowerDefenseItemSoccerBall FindIdleSoccerBallInActionArea()
	{
		AttackComponent attackComponent = _attackComponent2;
		if (attackComponent == null || attackComponent.IsReleased || !_attackComponent2.TryGetCheckAreaWorldRect(out var worldRect))
		{
			return null;
		}
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (!GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry))
		{
			return null;
		}
		IdleSoccerBallPredicate predicate = new IdleSoccerBallPredicate
		{
			Camp = camp
		};
		if (!towerDefenseBattleCharacterRegistry.TryFindItemIntersectingRect(worldRect, gridPos.Y, includeAllLineCheck: true, ref predicate, out var match))
		{
			return null;
		}
		return (TowerDefenseItemSoccerBall)match;
	}

	private bool IsKickableSameCampBall(TowerDefenseItemSoccerBall ball)
	{
		return IsKickableSameCampBall(ball, camp);
	}

	private static bool IsKickableSameCampBall(TowerDefenseItemSoccerBall ball, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		if (!GodotObject.IsInstanceValid(ball))
		{
			return false;
		}
		if (ball.die || ball.nearDie || ball.inWater)
		{
			return false;
		}
		if (ball.camp != camp)
		{
			return false;
		}
		BowlingComponent bowlingComponent = ball.bowlingComponent;
		if (bowlingComponent == null || bowlingComponent.IsReleased)
		{
			return false;
		}
		if (!ball.bowlingComponent.Alive)
		{
			return !ball.bowlingComponent.isRoll;
		}
		return false;
	}

	private void PlaceHeldBall()
	{
		if (hasBall)
		{
			SpawnSoccerBall();
			ApplyNoBallVisuals();
			hasBall = false;
			_ballPlacedDuringPut = true;
		}
	}

	private void ApplyNoBallVisuals()
	{
		sprite.SetFliter("ball", open: false);
		walkAnimeClip = "Walk2";
		attackAnimeClip = "Eat2";
	}

	private void SpawnSoccerBall()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ItemSoccerBall");
		if (packetConfig == null)
		{
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		if ((EconomyOwnerAccountId.IsValid ? packetConfig.Create(EconomyOwnerAccountId, logicalGlobalPosition, gridPos, groundHeight) : packetConfig.Create(logicalGlobalPosition, gridPos, groundHeight)) is TowerDefenseItemSoccerBall towerDefenseItemSoccerBall)
		{
			towerDefenseItemSoccerBall.groundHeight = groundHeight;
			Node2D node2D = TowerDefenseManager.GetCharacterNode();
			if (GodotObject.IsInstanceValid(node2D))
			{
				ApplyOwnerStateToSoccerBall(towerDefenseItemSoccerBall);
				node2D.AddChild(towerDefenseItemSoccerBall, forceReadableName: false, InternalMode.Disabled);
				ApplyOwnerStateToSoccerBall(towerDefenseItemSoccerBall);
				TowerDefenseManager.PublishSpawnedCharacter("ItemSoccerBall", towerDefenseItemSoccerBall, useCreate: true);
			}
		}
	}

	private void ApplyOwnerStateToSoccerBall(TowerDefenseItemSoccerBall ball)
	{
		if (GodotObject.IsInstanceValid(ball))
		{
			ball.camp = camp;
			if (GodotObject.IsInstanceValid(ball.instance) && GodotObject.IsInstanceValid(instance))
			{
				ball.instance.hypnoses = instance.hypnoses;
			}
			CharacterMoveComponent moveComponent = ball.moveComponent;
			if (moveComponent != null && !moveComponent.IsReleased && GodotObject.IsInstanceValid(ball.instance))
			{
				ball.moveComponent.moveScale = (ball.instance.hypnoses ? (-1.0) : 1.0);
			}
		}
	}

	private void KickSoccerBall(TowerDefenseItemSoccerBall ball)
	{
		if (!IsKickableSameCampBall(ball))
		{
			return;
		}
		ApplyOwnerStateToSoccerBall(ball);
		ball.BuffDelete("Frozen");
		CharacterMoveComponent moveComponent = ball.moveComponent;
		if (moveComponent == null || moveComponent.IsReleased)
		{
			return;
		}
		BowlingComponent bowlingComponent = ball.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased)
		{
			double num = Math.Abs(ball.bowlingComponent.rollXVelocityMin);
			double num2 = Math.Abs(ball.bowlingComponent.rollXVelocityMax);
			if (num > num2)
			{
				double num3 = num2;
				num2 = num;
				num = num3;
			}
			double num4 = ball.moveComponent.moveScale;
			if (Math.Abs(num4) <= 0.01)
			{
				num4 = 1.0;
			}
			double num5 = GD.RandRange(num, num2);
			ball.moveComponent.velocity = new Vector2((float)(GetKickDirection() * num5 / num4), ball.moveComponent.velocity.Y);
			ball.StartKickedRoll();
		}
	}

	private double GetKickDirection()
	{
		if (camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT)
		{
			return 1.0;
		}
		if (camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			return -1.0;
		}
		if (!(Scale.X < 0f))
		{
			return -1.0;
		}
		return 1.0;
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["hasBall"] = hasBall;
		dictionary["isPutting"] = isPutting;
		dictionary["isKicking"] = isKicking;
		dictionary["wantPut"] = _wantPut;
		dictionary["ballPlacedDuringPut"] = _ballPlacedDuringPut;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		hasBall = data.GetValueOrDefault("hasBall", true).AsBool();
		isPutting = data.GetValueOrDefault("isPutting", false).AsBool();
		isKicking = data.GetValueOrDefault("isKicking", false).AsBool();
		_wantPut = data.GetValueOrDefault("wantPut", false).AsBool();
		_ballPlacedDuringPut = data.GetValueOrDefault("ballPlacedDuringPut", false).AsBool();
		if (!hasBall)
		{
			ApplyNoBallVisuals();
		}
		if (isPutting || isKicking)
		{
			GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
			if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
			{
				base.groundMoveComponent.SetAlive(false);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPutTimerScale, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartPut, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartKick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
			new MethodInfo(MethodName.FindIdleSoccerBallInActionArea, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsKickableSameCampBall, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "ball", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsKickableSameCampBall, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "ball", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlaceHeldBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyNoBallVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnSoccerBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyOwnerStateToSoccerBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "ball", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.KickSoccerBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "ball", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetKickDirection, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.GetPutTimerScale && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetPutTimerScale());
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTimerTimeout && args.Count == 1)
		{
			OnTimerTimeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartPut && args.Count == 0)
		{
			StartPut();
			ret = default;
			return true;
		}
		if (method == MethodName.StartKick && args.Count == 1)
		{
			StartKick(VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in args[0]));
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
		if (method == MethodName.FindIdleSoccerBallInActionArea && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseItemSoccerBall>(FindIdleSoccerBallInActionArea());
			return true;
		}
		if (method == MethodName.IsKickableSameCampBall && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsKickableSameCampBall(VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in args[0])));
			return true;
		}
		if (method == MethodName.IsKickableSameCampBall && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsKickableSameCampBall(VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.PlaceHeldBall && args.Count == 0)
		{
			PlaceHeldBall();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNoBallVisuals && args.Count == 0)
		{
			ApplyNoBallVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnSoccerBall && args.Count == 0)
		{
			SpawnSoccerBall();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyOwnerStateToSoccerBall && args.Count == 1)
		{
			ApplyOwnerStateToSoccerBall(VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.KickSoccerBall && args.Count == 1)
		{
			KickSoccerBall(VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetKickDirection && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetKickDirection());
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsKickableSameCampBall && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsKickableSameCampBall(VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName.GetPutTimerScale)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.OnTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.StartPut)
		{
			return true;
		}
		if (method == MethodName.StartKick)
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
		if (method == MethodName.FindIdleSoccerBallInActionArea)
		{
			return true;
		}
		if (method == MethodName.IsKickableSameCampBall)
		{
			return true;
		}
		if (method == MethodName.PlaceHeldBall)
		{
			return true;
		}
		if (method == MethodName.ApplyNoBallVisuals)
		{
			return true;
		}
		if (method == MethodName.SpawnSoccerBall)
		{
			return true;
		}
		if (method == MethodName.ApplyOwnerStateToSoccerBall)
		{
			return true;
		}
		if (method == MethodName.KickSoccerBall)
		{
			return true;
		}
		if (method == MethodName.GetKickDirection)
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
		if (name == PropertyName.hasBall)
		{
			hasBall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isPutting)
		{
			isPutting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isKicking)
		{
			isKicking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._wantPut)
		{
			_wantPut = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ballPlacedDuringPut)
		{
			_ballPlacedDuringPut = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._kickTarget)
		{
			_kickTarget = VariantUtils.ConvertTo<TowerDefenseItemSoccerBall>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.hasBall)
		{
			value = VariantUtils.CreateFrom(in hasBall);
			return true;
		}
		if (name == PropertyName.isPutting)
		{
			value = VariantUtils.CreateFrom(in isPutting);
			return true;
		}
		if (name == PropertyName.isKicking)
		{
			value = VariantUtils.CreateFrom(in isKicking);
			return true;
		}
		if (name == PropertyName._wantPut)
		{
			value = VariantUtils.CreateFrom(in _wantPut);
			return true;
		}
		if (name == PropertyName._ballPlacedDuringPut)
		{
			value = VariantUtils.CreateFrom(in _ballPlacedDuringPut);
			return true;
		}
		if (name == PropertyName._kickTarget)
		{
			value = VariantUtils.CreateFrom(in _kickTarget);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasBall, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPutting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isKicking, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._wantPut, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ballPlacedDuringPut, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._kickTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hasBall, Variant.From(in hasBall));
		info.AddProperty(PropertyName.isPutting, Variant.From(in isPutting));
		info.AddProperty(PropertyName.isKicking, Variant.From(in isKicking));
		info.AddProperty(PropertyName._wantPut, Variant.From(in _wantPut));
		info.AddProperty(PropertyName._ballPlacedDuringPut, Variant.From(in _ballPlacedDuringPut));
		info.AddProperty(PropertyName._kickTarget, Variant.From(in _kickTarget));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hasBall, out var value))
		{
			hasBall = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isPutting, out var value2))
		{
			isPutting = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isKicking, out var value3))
		{
			isKicking = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._wantPut, out var value4))
		{
			_wantPut = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ballPlacedDuringPut, out var value5))
		{
			_ballPlacedDuringPut = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._kickTarget, out var value6))
		{
			_kickTarget = value6.As<TowerDefenseItemSoccerBall>();
		}
	}
}
