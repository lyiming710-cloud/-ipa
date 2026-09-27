using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Projectile/TowerDefenseProjectile.cs")]
public class TowerDefenseProjectile : TowerDefenseGroundItemBase
{
	public delegate void LandOverEventHandler(Vector2 pos, Vector2I gridPos);

	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public static readonly StringName IsHitBoxOutOfBounds = "IsHitBoxOutOfBounds";

		public static readonly StringName _CanSpawnEffectThisFrame = "_CanSpawnEffectThisFrame";

		public static readonly StringName EndGodotArrayProjectileMetric = "EndGodotArrayProjectileMetric";

		public static readonly StringName SetMonitorable = "SetMonitorable";

		public static readonly StringName SetHitBoxProcessMode = "SetHitBoxProcessMode";

		public static readonly StringName RecycleBodyChildren = "RecycleBodyChildren";

		public static readonly StringName ApplyFallVisual = "ApplyFallVisual";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName Recycle = "Recycle";

		public static readonly StringName Init = "Init";

		public static readonly StringName CompleteInit = "CompleteInit";

		public static readonly StringName Change = "Change";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Update = "Update";

		public static readonly StringName SetMonitoring = "SetMonitoring";

		public static readonly StringName CanProcessHit = "CanProcessHit";

		public static readonly StringName ShouldExpireTrackWithoutTarget = "ShouldExpireTrackWithoutTarget";

		public static readonly StringName CanHitCharacter = "CanHitCharacter";

		public static readonly StringName IsValidMapLine = "IsValidMapLine";

		public static readonly StringName ShouldFilterCollisionLine = "ShouldFilterCollisionLine";

		public static readonly StringName IsPenetrateDamage = "IsPenetrateDamage";

		public static readonly StringName BeginPenetrateOverlapScan = "BeginPenetrateOverlapScan";

		public static readonly StringName EndPenetrateOverlapScan = "EndPenetrateOverlapScan";

		public static readonly StringName TryRegisterPenetrateHit = "TryRegisterPenetrateHit";

		public static readonly StringName CheckAabbHits = "CheckAabbHits";

		public static readonly StringName ProcessShooter = "ProcessShooter";

		public static readonly StringName ProcessTrack = "ProcessTrack";

		public static readonly StringName ProcessTrackCheck = "ProcessTrackCheck";

		public static readonly StringName ProcessCatapult = "ProcessCatapult";

		public static readonly StringName Move = "Move";

		public new static readonly StringName SetZ = "SetZ";

		public static readonly StringName HitCharacter = "HitCharacter";

		public static readonly StringName HitCheck = "HitCheck";

		public static readonly StringName HitEffect = "HitEffect";

		public static readonly StringName HitEffectWithCharacterPosition = "HitEffectWithCharacterPosition";

		public static readonly StringName CreatSplat = "CreatSplat";

		public static readonly StringName CreatSplatWithCharacterPosition = "CreatSplatWithCharacterPosition";

		public static readonly StringName PlaySplat = "PlaySplat";

		public static readonly StringName CanCollision = "CanCollision";

		public static readonly StringName CanTarget = "CanTarget";

		public static readonly StringName CheckDifferentCamp = "CheckDifferentCamp";

		public static readonly StringName CheckSameLine = "CheckSameLine";

		public static readonly StringName Land = "Land";

		public static readonly StringName Over = "Over";

		public static readonly StringName OverFromExtension = "OverFromExtension";

		public static readonly StringName HitTargetFromExtension = "HitTargetFromExtension";

		public static readonly StringName LandFromExtension = "LandFromExtension";

		public static readonly StringName UpdateGridPos = "UpdateGridPos";

		public static readonly StringName CheckShooterHeight = "CheckShooterHeight";

		public static readonly StringName ShouldRefreshGrid = "ShouldRefreshGrid";

		public static readonly StringName EnableMonitoring = "EnableMonitoring";

		public static readonly StringName DisableMonitoring = "DisableMonitoring";

		public static readonly StringName TargetDiedFromExtension = "TargetDiedFromExtension";

		public static readonly StringName FindTrackTargetCached = "FindTrackTargetCached";

		public static readonly StringName FindTrackTarget = "FindTrackTarget";

		public static readonly StringName FindTrackTargetInternal = "FindTrackTargetInternal";

		public static readonly StringName BlockedBounce = "BlockedBounce";

		public static readonly StringName UpdateCatapultTarget = "UpdateCatapultTarget";

		public static readonly StringName CreateSplash = "CreateSplash";

		public static readonly StringName SetTrack = "SetTrack";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName useFall = "useFall";

		public static readonly StringName useGravity = "useGravity";

		public static readonly StringName WorldHitRect = "WorldHitRect";

		public static readonly StringName projectileBodyNode = "projectileBodyNode";

		public static readonly StringName shadowSprite = "shadowSprite";

		public static readonly StringName hitBox = "hitBox";

		public static readonly StringName over = "over";

		public static readonly StringName rememberPos = "rememberPos";

		public static readonly StringName metaData = "metaData";

		public static readonly StringName checkAll = "checkAll";

		public static readonly StringName suppressGameplay = "suppressGameplay";

		public static readonly StringName _useFall = "_useFall";

		public static readonly StringName _useGravity = "_useGravity";

		public static readonly StringName fireCharacter = "fireCharacter";

		public static readonly StringName velocity = "velocity";

		public static readonly StringName speed = "speed";

		public static readonly StringName config = "config";

		public static readonly StringName camp = "camp";

		public static readonly StringName damage = "damage";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName fireMethodFlags = "fireMethodFlags";

		public static readonly StringName projectileHeight = "projectileHeight";

		public static readonly StringName checkHeight = "checkHeight";

		public static readonly StringName height = "height";

		public static readonly StringName target = "target";

		public static readonly StringName magneticTarget = "magneticTarget";

		public static readonly StringName catapultTime = "catapultTime";

		public static readonly StringName catapultTimer = "catapultTimer";

		public static readonly StringName catapultTargetPos = "catapultTargetPos";

		public static readonly StringName catapultControlPoint = "catapultControlPoint";

		public static readonly StringName catapulCheckLast = "catapulCheckLast";

		public static readonly StringName penetrateNum = "penetrateNum";

		public static readonly StringName projectileSprite = "projectileSprite";

		public static readonly StringName hitOver = "hitOver";

		public static readonly StringName savePos = "savePos";

		public static readonly StringName checkDistance = "checkDistance";

		public static readonly StringName fireLength = "fireLength";

		public static readonly StringName rect = "rect";

		public static readonly StringName trackOpen = "trackOpen";

		public static readonly StringName catapultOpen = "catapultOpen";

		public static readonly StringName shadowScaleSave = "shadowScaleSave";

		public static readonly StringName initSet = "initSet";

		public static readonly StringName initHitList = "initHitList";

		public static readonly StringName setZInterval = "setZInterval";

		public static readonly StringName moveTween = "moveTween";

		public static readonly StringName randFreshIndex = "randFreshIndex";

		public static readonly StringName _gridRefreshScheduleInitialized = "_gridRefreshScheduleInitialized";

		public static readonly StringName _nextGridRefreshFrame = "_nextGridRefreshFrame";

		public static readonly StringName _cellGroundHeightCacheInitialized = "_cellGroundHeightCacheInitialized";

		public static readonly StringName _cellGroundHeightCacheValid = "_cellGroundHeightCacheValid";

		public static readonly StringName _cellGroundHeightCacheCell = "_cellGroundHeightCacheCell";

		public static readonly StringName _cellGroundHeightCacheCurve = "_cellGroundHeightCacheCurve";

		public static readonly StringName _cellGroundHeightCacheValue = "_cellGroundHeightCacheValue";

		public static readonly StringName fireDirX = "fireDirX";

		public static readonly StringName isShooter = "isShooter";

		public static readonly StringName extId = "extId";

		public static readonly StringName blocked = "blocked";

		public static readonly StringName trackCacheFrame = "trackCacheFrame";

		public static readonly StringName trackCacheTarget = "trackCacheTarget";

		public static readonly StringName trackCacheValid = "trackCacheValid";

		public static readonly StringName trackSearchInterval = "trackSearchInterval";

		public static readonly StringName trackNoTargetInterval = "trackNoTargetInterval";

		public static readonly StringName trackConsecutiveNoTarget = "trackConsecutiveNoTarget";

		public static readonly StringName trackMaxConsecutiveNoTarget = "trackMaxConsecutiveNoTarget";

		public static readonly StringName trackForceSearch = "trackForceSearch";

		public static readonly StringName _cacheRotateFollowVelocity = "_cacheRotateFollowVelocity";

		public static readonly StringName _cacheRotateScale = "_cacheRotateScale";

		public static readonly StringName _monitoringState = "_monitoringState";

		public static readonly StringName _monitorableState = "_monitorableState";

		public static readonly StringName _processModeEnabled = "_processModeEnabled";

		public static readonly StringName _bodyCleaned = "_bodyCleaned";

		public static readonly StringName _initPending = "_initPending";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	public Node2D projectileBodyNode;

	public TowerDefenseShadowVisual shadowSprite;

	public AabbArea2D hitBox;

	public bool over;

	public Vector2 rememberPos = Vector2.Zero;

	public Variant metaData;

	internal BulletFieldStoredProjectile[] eventProjectiles;

	public bool checkAll;

	public bool suppressGameplay;

	private bool _useFall;

	private bool _useGravity;

	public TowerDefenseCharacter fireCharacter;

	public Vector2 velocity = Vector2.Zero;

	public double speed;

	public TowerDefenseProjectileConfig config;

	public TowerDefenseEnum.CHARACTER_CAMP camp = TowerDefenseEnum.CHARACTER_CAMP.ALL;

	public double damage = 20.0;

	public int collisionFlags;

	public int damageFlags;

	public int fireMethodFlags;

	public TowerDefenseEnum.CHARACTER_HEIGHT projectileHeight = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL;

	public bool checkHeight;

	public double height;

	public TowerDefenseCharacter target;

	public TowerDefenseCharacter magneticTarget;

	public double catapultTime;

	public double catapultTimer;

	public Vector2 catapultTargetPos = Vector2.Zero;

	public Vector2 catapultControlPoint = Vector2.Zero;

	public bool catapulCheckLast;

	public int penetrateNum;

	public Node2D projectileSprite;

	public bool hitOver;

	public Vector2 savePos = Vector2.Zero;

	public double checkDistance;

	public double fireLength = -1.0;

	public Rect2 rect;

	private readonly HashSet<ulong> _penetratingTargetIds = new HashSet<ulong>();

	private readonly HashSet<ulong> _penetratingTargetIdsThisFrame = new HashSet<ulong>();

	private readonly List<ulong> _penetratingTargetIdsToRemove = new List<ulong>();

	private readonly List<TowerDefenseCharacter> _hitCandidates = new List<TowerDefenseCharacter>();

	public bool trackOpen;

	public bool catapultOpen;

	public Vector2 shadowScaleSave = Vector2.One;

	public bool initSet;

	public Array<AabbArea2D> initHitList;

	public int setZInterval = 2;

	public Tween moveTween;

	public int randFreshIndex;

	private bool _gridRefreshScheduleInitialized;

	private ulong _nextGridRefreshFrame;

	private bool _cellGroundHeightCacheInitialized;

	private bool _cellGroundHeightCacheValid;

	private TowerDefenseCellInstance _cellGroundHeightCacheCell;

	private CurveTexture _cellGroundHeightCacheCurve;

	private double _cellGroundHeightCacheValue;

	public int fireDirX = 1;

	public bool isShooter;

	public int extId = -1;

	public bool blocked;

	public int trackCacheFrame = -1;

	public TowerDefenseCharacter trackCacheTarget;

	public bool trackCacheValid;

	public int trackSearchInterval = 15;

	public int trackNoTargetInterval = 60;

	public int trackConsecutiveNoTarget;

	public int trackMaxConsecutiveNoTarget = 8;

	public bool trackForceSearch;

	public static int trackSearchBudget = 10;

	public static int trackSearchBudgetUsed = 0;

	public static int trackSearchBudgetFrame = -1;

	public static int effectSpawnBudget = 50;

	public static int effectSpawnUsed = 0;

	public static long effectSpawnFrame = -1L;

	private bool _cacheRotateFollowVelocity;

	private float _cacheRotateScale;

	private bool _monitoringState;

	private bool _monitorableState;

	private bool _processModeEnabled;

	private bool _bodyCleaned;

	private bool _initPending;

	public bool useFall
	{
		get
		{
			return _useFall;
		}
		set
		{
			_useFall = value;
			if (_useFall)
			{
				isGround = false;
				ApplyFallVisual();
			}
		}
	}

	public bool useGravity
	{
		get
		{
			return _useGravity;
		}
		set
		{
			_useGravity = value;
			if (_useGravity)
			{
				ApplyFallVisual();
			}
		}
	}

	public Rect2 WorldHitRect
	{
		get
		{
			if (!GodotObject.IsInstanceValid(hitBox))
			{
				return AabbShapeUtil.RectFromCenter(GlobalPosition, AabbShapeUtil.DefaultAreaSize);
			}
			return AabbShapeUtil.ComputeAreaWorldRect(hitBox);
		}
	}

	public event LandOverEventHandler OnLandOver;

	private bool IsHitBoxOutOfBounds()
	{
		return !AabbShapeUtil.Intersects(rect, WorldHitRect);
	}

	private static bool _CanSpawnEffectThisFrame()
	{
		long physicsFrames = (long)Engine.GetPhysicsFrames();
		if (physicsFrames != effectSpawnFrame)
		{
			effectSpawnFrame = physicsFrames;
			effectSpawnUsed = 0;
		}
		if (effectSpawnUsed >= effectSpawnBudget)
		{
			return false;
		}
		effectSpawnUsed++;
		return true;
	}

	private static void EndGodotArrayProjectileMetric(long startTicks, int items)
	{
		TowerDefensePerfProfiler.End("godotArray.projectile", startTicks, items);
	}

	private void SetMonitorable(bool value)
	{
		if (_monitorableState != value)
		{
			hitBox.SetDeferred("monitorable", value);
			_monitorableState = value;
		}
	}

	private void SetHitBoxProcessMode(bool enabled)
	{
		if (_processModeEnabled != enabled)
		{
			hitBox.SetDeferred("process_mode", (!enabled) ? 4 : 0);
			_processModeEnabled = enabled;
		}
	}

	private void RecycleBodyChildren()
	{
		foreach (Node child in projectileBodyNode.GetChildren())
		{
			if (child.HasMeta("_projectile_pool_scene"))
			{
				TowerDefenseProjectilePool.Push(child);
			}
			else if (config != null && config.projectileObject != ObjectManagerConfig.OBJECT.NOONE)
			{
				ObjectManager.PoolPush(config.projectileObject, child);
			}
			else
			{
				child.QueueFree();
			}
		}
	}

	private void ApplyFallVisual()
	{
		projectileBodyNode.Rotation = new Vector2(velocity.X, (float)ySpeed).Angle();
		shadowSprite.Position = new Vector2(shadowSprite.Position.X, (float)height);
		shadowSprite.Scale = Vector2.Zero;
	}

	public void Refresh()
	{
		ResetPhysicsInterpolation();
		AddToGroup("Projectile", persistent: true);
		metaData = default;
		eventProjectiles = null;
		over = false;
		foreach (StringName meta in GetMetaList())
		{
			RemoveMeta(meta);
		}
		gravityUse = true;
		ySpeed = 0.0;
		z = 0.0;
		Scale = Vector2.One;
		checkAll = false;
		suppressGameplay = false;
		useFall = false;
		useGravity = false;
		fireCharacter = null;
		projectileHeight = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL;
		_gridRefreshScheduleInitialized = false;
		_cellGroundHeightCacheInitialized = false;
		_cellGroundHeightCacheValid = false;
		_cellGroundHeightCacheCell = null;
		_cellGroundHeightCacheCurve = null;
		_cellGroundHeightCacheValue = 0.0;
		target = null;
		magneticTarget = null;
		projectileSprite = null;
		fireLength = -1.0;
		projectileBodyNode.Rotation = 0f;
		projectileBodyNode.Scale = new Vector2(1f, projectileBodyNode.Scale.Y);
		projectileBodyNode.Position = Vector2.Zero;
		shadowSprite.Visible = true;
		shadowSprite.Scale = Vector2.Zero;
		shadowSprite.Position = new Vector2(shadowSprite.Position.X, 20f);
		hitBox.Scale = Vector2.One;
		SetMonitoring(value: false);
		SetMonitorable(value: true);
		trackOpen = false;
		catapultOpen = false;
		isShooter = false;
		extId = -1;
		blocked = false;
		trackCacheFrame = -1;
		trackCacheTarget = null;
		trackCacheValid = false;
		trackConsecutiveNoTarget = 0;
		OnLandOver = null;
		_penetratingTargetIds.Clear();
		_penetratingTargetIdsThisFrame.Clear();
		_penetratingTargetIdsToRemove.Clear();
		moveTween = null;
		hitOver = false;
		_cacheRotateFollowVelocity = false;
		_cacheRotateScale = 0f;
		if (!_bodyCleaned)
		{
			RecycleBodyChildren();
		}
		_bodyCleaned = false;
		SetHitBoxProcessMode(enabled: true);
	}

	public void Recycle()
	{
		if (GodotObject.IsInstanceValid(moveTween) && moveTween.IsRunning())
		{
			moveTween.Kill();
		}
		SetMonitoring(value: false);
		SetMonitorable(value: false);
		SetHitBoxProcessMode(enabled: false);
		hitOver = true;
		RecycleBodyChildren();
		_bodyCleaned = true;
		RemoveFromGroup("Projectile");
		ySpeed = 0.0;
	}

	public void Init(TowerDefenseCharacter _fireCharacter, Vector2 _velocity, TowerDefenseProjectileConfig _config, int _collisionFlags = -1, TowerDefenseEnum.CHARACTER_CAMP _camp = TowerDefenseEnum.CHARACTER_CAMP.ALL, double _height = 0.0, TowerDefenseCharacter _target = null, bool isChange = false)
	{
		_penetratingTargetIds.Clear();
		_penetratingTargetIdsThisFrame.Clear();
		_penetratingTargetIdsToRemove.Clear();
		config = _config;
		if (config == null)
		{
			Over();
			return;
		}
		_cacheRotateFollowVelocity = config.rotateFollowVelocity;
		_cacheRotateScale = (float)config.rotateScale;
		projectileSprite = TowerDefenseProjectilePool.Pop(config.projectileScene, projectileBodyNode);
		projectileSprite.Scale = config.scale;
		fireCharacter = _fireCharacter;
		camp = _camp;
		velocity = _velocity;
		speed = velocity.Length();
		target = _target;
		fireDirX = Mathf.Sign(velocity.X);
		shadowSprite.Scale = config.size / new Vector2(65f, 65f);
		shadowScaleSave = shadowSprite.Scale;
		if (!isChange)
		{
			rememberPos = GlobalPosition;
			height = _height;
			if (_collisionFlags == -1)
			{
				if (GodotObject.IsInstanceValid(fireCharacter))
				{
					collisionFlags = fireCharacter.instance.collisionFlags;
				}
				else
				{
					collisionFlags = config.collisionFlags;
				}
			}
			else
			{
				collisionFlags = _collisionFlags;
			}
			projectileBodyNode.Rotation = 0f;
			if (GodotObject.IsInstanceValid(fireCharacter))
			{
				shadowSprite.Position = new Vector2(shadowSprite.Position.X, 0f - (float)fireCharacter.GetGroundHeight(GlobalPosition.Y));
			}
			else
			{
				shadowSprite.Position = new Vector2(shadowSprite.Position.X, 20f);
			}
			projectileBodyNode.Position = new Vector2(projectileBodyNode.Position.X, 0f - (float)height);
			hitBox.Position = new Vector2(hitBox.Position.X, -20f);
			savePos = shadowSprite.GlobalPosition;
			if (GodotObject.IsInstanceValid(fireCharacter))
			{
				projectileHeight = (TowerDefenseEnum.CHARACTER_HEIGHT)Mathf.Min(2, (int)fireCharacter.instance.height);
			}
			checkHeight = false;
		}
		else
		{
			projectileBodyNode.Position = new Vector2(projectileBodyNode.Position.X, 0f - (float)height);
			hitBox.Position = new Vector2(hitBox.Position.X, -20f);
		}
		damage = config.baseDamage;
		damageFlags = config.damageFlags;
		fireMethodFlags = config.fireMethodFlags;
		isShooter = (fireMethodFlags & 1) != 0 && !useGravity && !useFall;
		if ((fireMethodFlags & 0x20) != 0)
		{
			SetTrack(isChange);
		}
		if ((fireMethodFlags & 4) != 0)
		{
			penetrateNum = config.penetrateNum;
		}
		if ((fireMethodFlags & 2) != 0)
		{
			Vector2 vector = new Vector2((float)TowerDefenseManager.Instance.GetMapGroundRight(), GlobalPosition.Y);
			if (GodotObject.IsInstanceValid(target))
			{
				Vector2 logicalGlobalPosition = target.GetLogicalGlobalPosition();
				catapultTargetPos = logicalGlobalPosition + new Vector2(-10f * target.Scale.X * target.spriteGroup.Scale.X, 0f);
			}
			else
			{
				catapultTargetPos = vector + new Vector2(60f, 0f);
			}
			shadowSprite.Visible = true;
			shadowSprite.Scale = Vector2.Zero;
			double num = 2000.0;
			double catapultHeight = config.catapultHeight;
			double num2 = Mathf.Max(height, 0.0);
			ySpeed = 0.0 - Mathf.Sqrt(2.0 * num * catapultHeight);
			double num3 = Mathf.Sqrt(2.0 * catapultHeight / num);
			double num4 = Mathf.Sqrt(2.0 * (num2 + catapultHeight) / num);
			double num5 = num3 + num4;
			velocity = (catapultTargetPos - GlobalPosition) / (float)num5;
			z = num2;
			isGround = false;
			catapultTime = num5;
			catapultTimer = 0.0;
			catapultControlPoint = Vector2.Zero;
			catapulCheckLast = false;
			catapultOpen = true;
		}
		_initPending = true;
	}

	private void CompleteInit()
	{
		if (fireLength != -1.0)
		{
			checkDistance = (double)TowerDefenseManager.Instance.GetMapGridSize().X * fireLength;
		}
		if (GodotObject.IsInstanceValid(target) || useFall || useGravity)
		{
			SetMonitoring(value: false);
		}
		else
		{
			SetMonitoring(value: true);
		}
		SetMonitorable(value: true);
	}

	public void Change(TowerDefenseProjectileConfig _config, StringName projectileName, StringName changeprojectileName, TowerDefenseCharacter changeCharacter)
	{
		if (!GodotObject.IsInstanceValid(fireCharacter))
		{
			fireCharacter = null;
		}
		if (!GodotObject.IsInstanceValid(target))
		{
			target = null;
		}
		RecycleBodyChildren();
		extId = -1;
		Init(fireCharacter, velocity, _config, collisionFlags, camp, height, target, isChange: true);
	}

	public override void _Ready()
	{
		projectileBodyNode = GetNode<Node2D>("%ProjectileBodyNode");
		shadowSprite = TowerDefenseShadowVisual.Capture(this, GetNodeOrNull<Sprite2D>("%ShadowSprite"));
		hitBox = GetNode<AabbArea2D>("%HitBox");
		randFreshIndex = (int)GD.Randi();
		rect = FireComponent.ComputeProjectileMapRect();
		OnLand += Land;
		SetPhysicsProcess(enable: false);
	}

	public void Update(float delta, ulong currentFrame)
	{
		if (over)
		{
			return;
		}
		if (_initPending)
		{
			_initPending = false;
			if (!over)
			{
				CompleteInit();
			}
			return;
		}
		if (trackOpen && extId >= 0)
		{
			ProcessTrackCheck();
			return;
		}
		if (!_cacheRotateFollowVelocity)
		{
			if (_cacheRotateScale != 0f)
			{
				projectileSprite.Rotation += delta * _cacheRotateScale * (float)fireDirX;
			}
		}
		else if (!catapultOpen)
		{
			projectileBodyNode.Rotation = velocity.Angle();
		}
		if (isShooter && !useFall && !useGravity && extId < 0 && !trackOpen)
		{
			ProcessShooter(delta, currentFrame);
			return;
		}
		if (useGravity)
		{
			PhysiceUpdate(delta);
			hitBox.Position = new Vector2(hitBox.Position.X, 0f - (float)z);
			shadowSprite.Scale = shadowScaleSave * (float)Mathf.Max(1.0 - z / 600.0, 0.0);
			if (_cacheRotateFollowVelocity)
			{
				Vector2 vector = new Vector2(velocity.X, (float)ySpeed);
				if ((double)vector.Length() > 0.1)
				{
					projectileBodyNode.Rotation = vector.Angle();
				}
			}
			if (ySpeed >= 0.0 && z - groundHeight < 60.0)
			{
				SetMonitoring(value: true);
			}
			if (z <= groundHeight)
			{
				SetMonitoring(value: false);
				Land();
				return;
			}
		}
		if (useFall)
		{
			if (ShouldRefreshGrid(currentFrame))
			{
				gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
			}
			if (TryGetCachedCellGroundHeight(out var cellGroundHeight) && groundHeight != cellGroundHeight)
			{
				groundHeight = cellGroundHeight;
			}
			shadowSprite.Position = new Vector2(shadowSprite.Position.X, 0f - (float)groundHeight);
			if (!(z > groundHeight))
			{
				SetMonitoring(value: false);
				gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
				Land();
				return;
			}
			ySpeed += gravity * gravityScale * (double)delta;
			z -= ySpeed * (double)delta;
			SetMonitoring(value: false);
			if (z - groundHeight <= 100.0)
			{
				SetMonitoring(value: true);
				hitBox.ProcessMode = ProcessModeEnum.Inherit;
			}
		}
		if (trackOpen)
		{
			ProcessTrack(delta, currentFrame);
		}
		if (!catapultOpen)
		{
			Move(delta);
		}
		if (fireLength != -1.0 && (double)savePos.DistanceSquaredTo(GlobalPosition) > checkDistance * checkDistance)
		{
			Over();
			return;
		}
		if (catapultOpen)
		{
			ProcessCatapult(delta, currentFrame);
		}
		CheckAabbHits();
	}

	private void SetMonitoring(bool value)
	{
		if (_monitoringState != value)
		{
			hitBox.SetDeferred("monitoring", value);
			_monitoringState = value;
		}
	}

	private bool CanProcessHit()
	{
		if (hitOver)
		{
			return false;
		}
		if (catapultOpen && (ySpeed < 0.0 || z - groundHeight > 60.0))
		{
			return false;
		}
		if (trackOpen)
		{
			return false;
		}
		if (!_monitoringState && !hitBox.Monitoring)
		{
			return false;
		}
		if (!_processModeEnabled && hitBox.ProcessMode == ProcessModeEnum.Disabled)
		{
			return false;
		}
		if (useGravity && (ySpeed < 0.0 || z - groundHeight > 60.0))
		{
			return false;
		}
		return true;
	}

	private bool ShouldExpireTrackWithoutTarget()
	{
		if (trackOpen && !hitOver && trackMaxConsecutiveNoTarget > 0 && trackConsecutiveNoTarget >= trackMaxConsecutiveNoTarget)
		{
			return !GodotObject.IsInstanceValid(target);
		}
		return false;
	}

	private bool CanHitCharacter(TowerDefenseCharacter tdChar)
	{
		if (!GodotObject.IsInstanceValid(tdChar))
		{
			return false;
		}
		if (ShouldFilterCollisionLine() && !tdChar.IsTargetableFromLine(gridPos.Y))
		{
			return false;
		}
		if (!tdChar.targetRegistrationComponent.canProjectileCheck)
		{
			return false;
		}
		if (!tdChar.instance.canBeCollection)
		{
			return false;
		}
		if ((collisionFlags & tdChar.instance.maskFlags) == 0)
		{
			return false;
		}
		if (!CanTarget(tdChar))
		{
			return false;
		}
		if (checkHeight && projectileHeight > tdChar.instance.height)
		{
			return false;
		}
		return true;
	}

	private static bool IsValidMapLine(int line)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || instance.gridNum.Y <= 0)
		{
			return line != -2147483648;
		}
		if (line >= 1)
		{
			return line <= instance.gridNum.Y;
		}
		return false;
	}

	private bool ShouldFilterCollisionLine()
	{
		if (!checkAll)
		{
			return IsValidMapLine(gridPos.Y);
		}
		return false;
	}

	private bool IsPenetrateDamage()
	{
		if ((fireMethodFlags & 4) != 0)
		{
			return !trackOpen;
		}
		return false;
	}

	private void BeginPenetrateOverlapScan()
	{
		if (IsPenetrateDamage())
		{
			_penetratingTargetIdsThisFrame.Clear();
		}
	}

	private void EndPenetrateOverlapScan()
	{
		if (!IsPenetrateDamage())
		{
			return;
		}
		_penetratingTargetIdsToRemove.Clear();
		foreach (ulong penetratingTargetId in _penetratingTargetIds)
		{
			if (!_penetratingTargetIdsThisFrame.Contains(penetratingTargetId))
			{
				_penetratingTargetIdsToRemove.Add(penetratingTargetId);
			}
		}
		foreach (ulong item in _penetratingTargetIdsToRemove)
		{
			_penetratingTargetIds.Remove(item);
		}
		_penetratingTargetIdsToRemove.Clear();
	}

	private bool TryRegisterPenetrateHit(TowerDefenseCharacter character)
	{
		if (!IsPenetrateDamage())
		{
			return true;
		}
		ulong instanceId = character.GetInstanceId();
		_penetratingTargetIdsThisFrame.Add(instanceId);
		if (_penetratingTargetIds.Contains(instanceId))
		{
			return false;
		}
		_penetratingTargetIds.Add(instanceId);
		return true;
	}

	private void CheckAabbHits()
	{
		if (!CanProcessHit())
		{
			return;
		}
		BeginPenetrateOverlapScan();
		try
		{
			Rect2 checkRect = AabbShapeUtil.ComputeAreaWorldRect(hitBox);
			bool flag = ShouldFilterCollisionLine();
			int line = (flag ? gridPos.Y : (-2147483648));
			_hitCandidates.Clear();
			_hitCandidates.AddRange(TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(checkRect, line, flag));
			foreach (TowerDefenseCharacter hitCandidate in _hitCandidates)
			{
				if (!CanProcessHit())
				{
					break;
				}
				if (CanHitCharacter(hitCandidate))
				{
					HitCharacter(hitCandidate);
				}
			}
		}
		finally
		{
			EndPenetrateOverlapScan();
		}
	}

	public void ProcessShooter(float delta, ulong currentFrame)
	{
		if (ShouldRefreshGrid(currentFrame))
		{
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
		}
		if (CheckShooterHeight())
		{
			return;
		}
		if (extId < 0)
		{
			GlobalPosition += velocity * delta;
			if (IsHitBoxOutOfBounds())
			{
				Over();
				return;
			}
			if (fireLength != -1.0 && (double)savePos.DistanceSquaredTo(GlobalPosition) > checkDistance * checkDistance)
			{
				Over();
			}
		}
		CheckAabbHits();
	}

	public void ProcessTrack(float delta, ulong currentFrame)
	{
		bool flag = GodotObject.IsInstanceValid(target);
		if (flag && !target.targetRegistrationComponent.canProjectileCheck)
		{
			flag = false;
		}
		if (flag && (target.nearDie || target.die || !CanTarget(target) || !CanCollision(target.instance.maskFlags)))
		{
			flag = false;
		}
		if (flag && !target.IsHitBoxEnabled)
		{
			flag = false;
		}
		if (!flag)
		{
			target = null;
			FindTrackTargetCached();
		}
		int num = (GodotObject.IsInstanceValid(target) ? trackSearchInterval : trackNoTargetInterval);
		if (currentFrame % (ulong)num == 0L)
		{
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
			FindTrackTargetCached();
		}
		if (ShouldExpireTrackWithoutTarget())
		{
			Over();
		}
		else if (GodotObject.IsInstanceValid(target))
		{
			Vector2 globalPosition = GlobalPosition;
			Vector2 globalPositionForPhysicsFrame = target.GetGlobalPositionForPhysicsFrame(currentFrame);
			Vector2 vector = (globalPositionForPhysicsFrame - globalPosition).Normalized();
			projectileBodyNode.Rotation = (float)Mathf.LerpAngle(projectileBodyNode.Rotation, vector.Angle(), (double)delta * 5.0);
			velocity = vector * (float)speed;
			if (Geometry2D.IsPointInCircle(globalPosition, globalPositionForPhysicsFrame, 30f))
			{
				HitCharacter(target);
			}
		}
		else
		{
			velocity = new Vector2(Mathf.Cos(projectileBodyNode.Rotation), Mathf.Sin(projectileBodyNode.Rotation)) * (float)speed;
			if (IsHitBoxOutOfBounds())
			{
				Over();
			}
		}
	}

	public void ProcessTrackCheck()
	{
		if (over)
		{
			return;
		}
		int num = (GodotObject.IsInstanceValid(target) ? trackSearchInterval : trackNoTargetInterval);
		if ((ulong)((long)Engine.GetPhysicsFrames() + (long)randFreshIndex) % (ulong)num == 0L)
		{
			bool flag = GodotObject.IsInstanceValid(target);
			if (flag && !target.targetRegistrationComponent.canProjectileCheck)
			{
				flag = false;
			}
			if (flag && (target.nearDie || target.die || !CanTarget(target) || !CanCollision(target.instance.maskFlags)))
			{
				flag = false;
			}
			if (flag && !target.IsHitBoxEnabled)
			{
				flag = false;
			}
			if (!flag)
			{
				target = null;
				FindTrackTargetCached();
			}
			if (ShouldExpireTrackWithoutTarget())
			{
				Over();
			}
			else if (!GodotObject.IsInstanceValid(target) && IsHitBoxOutOfBounds())
			{
				Over();
			}
		}
	}

	public void ProcessCatapult(float delta)
	{
		ProcessCatapult(delta, Engine.GetPhysicsFrames());
	}

	private void ProcessCatapult(float delta, ulong physicsFrame)
	{
		catapultTimer += delta;
		double num = 2000.0;
		ySpeed += num * (double)delta;
		z -= ySpeed * (double)delta;
		GlobalPosition += velocity * delta;
		projectileBodyNode.Position = new Vector2(projectileBodyNode.Position.X, 0f - (float)z);
		shadowSprite.Scale = shadowScaleSave * (float)Mathf.Max(1.0 - z / 600.0, 0.0);
		hitBox.Position = new Vector2(hitBox.Position.X, 0f - (float)z);
		if (_cacheRotateFollowVelocity)
		{
			Vector2 vector = new Vector2(velocity.X, (float)ySpeed);
			if ((double)vector.Length() > 0.1)
			{
				projectileBodyNode.Rotation = vector.Angle();
			}
		}
		if (ySpeed > 0.0 && z - groundHeight < 100.0 && !blocked)
		{
			SetMonitoring(value: true);
			if (!hitOver && GodotObject.IsInstanceValid(target) && !target.die && target.targetRegistrationComponent.canProjectileCheck && CanTarget(target))
			{
				Vector2 globalPositionForPhysicsFrame = target.GetGlobalPositionForPhysicsFrame(physicsFrame);
				if (GlobalPosition.DistanceSquaredTo(globalPositionForPhysicsFrame) < 625f)
				{
					HitCharacter(target);
					return;
				}
			}
		}
		if (z <= groundHeight && ySpeed >= 0.0)
		{
			z = groundHeight;
			ySpeed = 0.0;
			isGround = true;
			Land();
		}
	}

	public void Move(float delta)
	{
		GlobalPosition += velocity * delta;
		if (!useFall && !useGravity && IsHitBoxOutOfBounds())
		{
			Over();
		}
	}

	public override void SetZ()
	{
		projectileBodyNode.Position = new Vector2(projectileBodyNode.Position.X, 0f - (float)z);
		shadowSprite.Scale = shadowScaleSave * (float)Mathf.Max(1.0 - z / 600.0, 0.0);
	}

	public void HitCharacter(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		if (character is TowerDefensePlant)
		{
			TowerDefenseCellInstance towerDefenseCellInstance = character.cell;
			if (GodotObject.IsInstanceValid(towerDefenseCellInstance))
			{
				character = towerDefenseCellInstance.GetTarget(collisionFlags, camp, checkInvincible: true, catapultOpen);
			}
		}
		if (!GodotObject.IsInstanceValid(character) || !TryRegisterPenetrateHit(character))
		{
			return;
		}
		if (suppressGameplay)
		{
			HitEffect(character);
			Over();
			return;
		}
		character.ProjectileHurt(this, config);
		foreach (TowerDefenseCharacterEventBase hitTargetEvent in config.hitTargetEventList)
		{
			hitTargetEvent.ExecuteProject(this, character);
		}
		Vector2 globalPosition = GlobalPosition;
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		if (!character.HasShield() || (damageFlags & 1) == 0 || ((fireMethodFlags & 1) != 0 && (((double)character.Scale.X > 0.0 && globalPosition.X > logicalGlobalPosition.X + 30f) || ((double)character.Scale.X < 0.0 && globalPosition.X < logicalGlobalPosition.X - 30f))) || (fireMethodFlags & 2) != 0)
		{
			if (!character.ProjectileEffectsBlocked(damageFlags, fireMethodFlags))
			{
				foreach (TowerDefenseCharacterEventBase hitCharacterEvent in config.hitCharacterEventList)
				{
					hitCharacterEvent.ExecuteProject(this, character);
				}
			}
			if (config.useRange)
			{
				Vector2 mapCellPosCenter = TowerDefenseManager.Instance.GetMapCellPosCenter(TowerDefenseManager.Instance.GetMapGridPos(logicalGlobalPosition));
				mapCellPosCenter = new Vector2(logicalGlobalPosition.X, mapCellPosCenter.Y);
				TowerDefenseExplode.CreateProjectileExplodeSingleExclude(mapCellPosCenter, config, character, camp, useFall && config.UsesExplosionDamage);
			}
		}
		HitEffectWithCharacterPosition(character, logicalGlobalPosition, hasCharacterPosition: true);
		bool flag = true;
		if ((fireMethodFlags & 4) != 0 && !trackOpen)
		{
			if (config.penetrateNum != -1)
			{
				penetrateNum--;
				if (penetrateNum > 0)
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
		}
		if (flag)
		{
			hitOver = true;
			Over();
		}
	}

	public void HitCheck(AabbArea2D area)
	{
		if (!CanProcessHit())
		{
			return;
		}
		Node parent = area.GetParent();
		if (parent is TowerDefenseCharacter)
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)parent;
			if (CanHitCharacter(towerDefenseCharacter))
			{
				HitCharacter(towerDefenseCharacter);
			}
		}
	}

	public void HitEffect(TowerDefenseCharacter character)
	{
		bool flag = GodotObject.IsInstanceValid(character);
		Vector2 characterPosition = (flag ? character.GetLogicalGlobalPosition() : default(Vector2));
		HitEffectWithCharacterPosition(character, characterPosition, flag);
	}

	private void HitEffectWithCharacterPosition(TowerDefenseCharacter character, Vector2 characterPosition, bool hasCharacterPosition)
	{
		bool flag = _CanSpawnEffectThisFrame();
		if (flag && config.hitEffect != null)
		{
			Vector2 globalPosition = hitBox.GlobalPosition;
			Vector2I gridPosition = (GodotObject.IsInstanceValid(character) ? character.gridPos : Vector2I.Zero);
			if (!GodotObject.IsInstanceValid(TowerDefenseGroundItemBase.characterNode) || !TowerDefenseManager.TryCreateEffectSceneOnceFast(config.hitEffect, TowerDefenseGroundItemBase.characterNode, gridPosition, globalPosition, preferSprite: false))
			{
				Node node = config.hitEffect.Instantiate(PackedScene.GenEditState.Disabled);
				Node node2;
				if (node is GPUParticles2DOnece scene)
				{
					node2 = TowerDefenseManager.Instance.CreateEffectParticlesSceneOnce(scene, gridPosition);
				}
				else
				{
					node2 = node;
					if (node2 is TowerDefenseProjectileEffectBase towerDefenseProjectileEffectBase)
					{
						towerDefenseProjectileEffectBase.suppressDeathrattles = (damageFlags & 0x20) != 0;
						if (GodotObject.IsInstanceValid(character))
						{
							towerDefenseProjectileEffectBase.Init(gridPos, camp, collisionFlags, character, character.groundHeight);
						}
						else if (GodotObject.IsInstanceValid(cell))
						{
							towerDefenseProjectileEffectBase.Init(gridPos, camp, collisionFlags, null, cell.GetGroundHeight());
						}
					}
				}
				if (node2 is Node2D node2D && GodotObject.IsInstanceValid(TowerDefenseGroundItemBase.characterNode))
				{
					node2D.GlobalPosition = globalPosition;
					TowerDefenseGroundItemBase.characterNode.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
				}
				else
				{
					node2.QueueFree();
				}
			}
		}
		if (flag && TowerDefenseManager.Instance.GetEffectCount() < 100)
		{
			CreatSplatWithCharacterPosition(character, characterPosition, hasCharacterPosition);
		}
		if (config.splatAudio != "SplatNormal" || (character != null && !character.instance.HasAnyArmor))
		{
			PlaySplat();
		}
	}

	public void CreatSplat(TowerDefenseCharacter character)
	{
		bool flag = GodotObject.IsInstanceValid(character);
		Vector2 characterPosition = (flag ? character.GetLogicalGlobalPosition() : default(Vector2));
		CreatSplatWithCharacterPosition(character, characterPosition, flag);
	}

	private void CreatSplatWithCharacterPosition(TowerDefenseCharacter character, Vector2 characterPosition, bool hasCharacterPosition)
	{
		if (config.splatScene == null)
		{
			return;
		}
		int y = character?.gridPos.Y ?? gridPos.Y;
		Vector2 globalPosition = ((!config.hitBody || !hasCharacterPosition) ? projectileBodyNode.GlobalPosition : (characterPosition - new Vector2(0f, 20f)));
		if (!GodotObject.IsInstanceValid(TowerDefenseGroundItemBase.characterNode) || !TowerDefenseManager.TryCreateEffectSceneOnceFast(config.splatScene, TowerDefenseGroundItemBase.characterNode, new Vector2I(gridPos.X, y), globalPosition, string.Equals(config.splatSceneType, "Sprite", StringComparison.OrdinalIgnoreCase)))
		{
			Node node = config.splatScene.Instantiate(PackedScene.GenEditState.Disabled);
			TowerDefenseEffectBase towerDefenseEffectBase = null;
			if (node is GPUParticles2DOnece scene)
			{
				towerDefenseEffectBase = TowerDefenseManager.Instance.CreateEffectParticlesSceneOnce(scene, gridPos);
			}
			else if (node is AdobeAnimateSprite scene2)
			{
				towerDefenseEffectBase = TowerDefenseManager.Instance.CreateEffectSpriteSceneOnce(scene2, gridPos);
			}
			else if (node is TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce)
			{
				towerDefenseEffectBase = towerDefenseEffectSpriteOnce;
			}
			else if (node is TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce)
			{
				towerDefenseEffectBase = towerDefenseEffectParticlesOnce;
			}
			if (towerDefenseEffectBase == null)
			{
				node.QueueFree();
				return;
			}
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectBase, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectBase.gridPos = new Vector2I(towerDefenseEffectBase.gridPos.X, y);
			towerDefenseEffectBase.GlobalPosition = globalPosition;
		}
	}

	public void PlaySplat()
	{
		AudioManager.Instance.AudioPlay(config.splatAudio);
	}

	public bool CanCollision(int maskFlags)
	{
		return (maskFlags & collisionFlags) != 0;
	}

	public bool CanTarget(TowerDefenseCharacter character)
	{
		if (fireCharacter is TowerDefenseZombie && character is TowerDefenseGravestone)
		{
			return false;
		}
		return CheckDifferentCamp(character.camp);
	}

	public bool CheckDifferentCamp(TowerDefenseEnum.CHARACTER_CAMP _camp)
	{
		return camp != _camp;
	}

	public bool CheckSameLine(int line)
	{
		return line == gridPos.Y;
	}

	public void Land()
	{
		gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
		if (!blocked)
		{
			if (!suppressGameplay)
			{
				foreach (TowerDefenseCharacterEventBase hitGroundEvent in config.hitGroundEventList)
				{
					hitGroundEvent.ExecuteGroundProject(this, GlobalPosition, gridPos);
				}
				if (config.useRange)
				{
					TowerDefenseExplode.CreateProjectileExplode(GlobalPosition, config, null, camp, useFall && config.UsesExplosionDamage);
				}
			}
			HitEffect(null);
			PlaySplat();
		}
		if (!suppressGameplay)
		{
			OnLandOver?.Invoke(GlobalPosition, gridPos);
		}
		if (GodotObject.IsInstanceValid(cell) && cell.isWater)
		{
			CreateSplash();
			Over();
		}
		else
		{
			Over();
		}
	}

	public void Over()
	{
		if (!over)
		{
			over = true;
			hitOver = true;
			GlobalPosition = new Vector2(-100f, -100f);
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.PROJECTILE, this);
		}
	}

	public void OverFromExtension()
	{
		if (!over)
		{
			extId = -1;
			Over();
		}
	}

	public void HitTargetFromExtension()
	{
		if (!over)
		{
			if (GodotObject.IsInstanceValid(target))
			{
				HitCharacter(target);
			}
			else
			{
				Over();
			}
		}
	}

	public void LandFromExtension()
	{
		if (!over)
		{
			extId = -1;
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
			Land();
		}
	}

	public void UpdateGridPos()
	{
		gridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
	}

	public bool CheckShooterHeight()
	{
		if (TryGetCachedCellGroundHeight(out var cellGroundHeight))
		{
			double num = cellGroundHeight;
			if (projectileHeight < TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL)
			{
				num -= 50.0;
			}
			else if (projectileHeight == TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL)
			{
				num -= 20.0;
			}
			if (height < num)
			{
				HitEffect(null);
				Over();
				return true;
			}
		}
		return false;
	}

	private bool ShouldRefreshGrid(ulong currentFrame)
	{
		if (!_gridRefreshScheduleInitialized)
		{
			ulong num = (ulong)randFreshIndex % 5uL;
			ulong num2 = (currentFrame + num) % 5;
			_nextGridRefreshFrame = currentFrame + (5 - num2) % 5;
			_gridRefreshScheduleInitialized = true;
		}
		if (currentFrame < _nextGridRefreshFrame)
		{
			return false;
		}
		do
		{
			_nextGridRefreshFrame += 5uL;
		}
		while (_nextGridRefreshFrame <= currentFrame);
		return true;
	}

	private bool TryGetCachedCellGroundHeight(out double cellGroundHeight)
	{
		CurveTexture curveTexture = cell?.groundHeightCurve;
		if (_cellGroundHeightCacheInitialized && _cellGroundHeightCacheCell == cell && _cellGroundHeightCacheCurve == curveTexture)
		{
			cellGroundHeight = _cellGroundHeightCacheValue;
			return _cellGroundHeightCacheValid;
		}
		_cellGroundHeightCacheInitialized = true;
		_cellGroundHeightCacheCell = cell;
		_cellGroundHeightCacheCurve = curveTexture;
		_cellGroundHeightCacheValid = GodotObject.IsInstanceValid(cell);
		_cellGroundHeightCacheValue = (_cellGroundHeightCacheValid ? cell.GetGroundHeight() : 0.0);
		cellGroundHeight = _cellGroundHeightCacheValue;
		return _cellGroundHeightCacheValid;
	}

	public void EnableMonitoring()
	{
		SetMonitoring(value: true);
		hitBox.ProcessMode = ProcessModeEnum.Inherit;
	}

	public void DisableMonitoring()
	{
		SetMonitoring(value: false);
	}

	public void TargetDiedFromExtension()
	{
		target = null;
		trackCacheValid = false;
		FindTrackTargetCached();
	}

	public void FindTrackTargetCached()
	{
		ulong physicsFrames = Engine.GetPhysicsFrames();
		if ((int)physicsFrames != trackSearchBudgetFrame)
		{
			trackSearchBudgetFrame = (int)physicsFrames;
			trackSearchBudgetUsed = 0;
		}
		if (!trackForceSearch && trackSearchBudgetUsed >= trackSearchBudget)
		{
			return;
		}
		if (trackCacheValid && GodotObject.IsInstanceValid(trackCacheTarget))
		{
			bool flag = true;
			if (!trackCacheTarget.targetRegistrationComponent.canProjectileCheck)
			{
				flag = false;
			}
			if (trackCacheTarget.nearDie || trackCacheTarget.die || !CanTarget(trackCacheTarget) || !CanCollision(trackCacheTarget.instance.maskFlags))
			{
				flag = false;
			}
			if (!trackCacheTarget.IsHitBoxEnabled)
			{
				flag = false;
			}
			if (flag)
			{
				target = trackCacheTarget;
				trackConsecutiveNoTarget = 0;
				return;
			}
		}
		FindTrackTargetInternal();
		trackSearchBudgetUsed++;
		trackForceSearch = false;
		trackCacheFrame = (int)physicsFrames;
		trackCacheTarget = target;
		trackCacheValid = GodotObject.IsInstanceValid(target);
		if (GodotObject.IsInstanceValid(target))
		{
			trackConsecutiveNoTarget = 0;
		}
		else
		{
			trackConsecutiveNoTarget++;
		}
	}

	public void FindTrackTarget()
	{
		FindTrackTargetInternal();
	}

	public void FindTrackTargetInternal()
	{
		if (GodotObject.IsInstanceValid(target))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(magneticTarget) && !magneticTarget.nearDie && !magneticTarget.die && CanTarget(magneticTarget) && CanCollision(magneticTarget.instance.maskFlags) && magneticTarget.targetRegistrationComponent.canProjectileCheck && magneticTarget.IsHitBoxEnabled)
		{
			target = magneticTarget;
			return;
		}
		TowerDefenseCharacter projectileTargetNearest = TowerDefenseManager.Instance.GetProjectileTargetNearest(this, collisionFlags);
		if (GodotObject.IsInstanceValid(projectileTargetNearest))
		{
			target = projectileTargetNearest;
			if (target is TowerDefensePlant && GodotObject.IsInstanceValid(cell))
			{
				target = cell.GetTarget(collisionFlags, camp);
			}
		}
	}

	public void BlockedBounce()
	{
		if (!over && !hitOver)
		{
			blocked = true;
			target = null;
			hitOver = true;
			SetMonitoring(value: false);
			SetMonitorable(value: false);
			ySpeed = -500.0;
			velocity *= 0.5f;
		}
	}

	public void UpdateCatapultTarget()
	{
		if (GodotObject.IsInstanceValid(target))
		{
			Vector2 logicalGlobalPosition = target.GetLogicalGlobalPosition();
			Vector2 logicalGlobalPosition2 = target.GetLogicalGlobalPosition(target.transformPoint);
			catapultTargetPos = logicalGlobalPosition2 + new Vector2((float)(-10 * fireDirX) * target.Scale.X * target.spriteGroup.Scale.X, -10f);
			catapultTargetPos = new Vector2(catapultTargetPos.X, Mathf.Max(catapultTargetPos.Y, logicalGlobalPosition.Y) - 30f);
		}
	}

	public TowerDefenseEffectSpriteOnce CreateSplash()
	{
		if (TowerDefenseManager.Instance.GetEffectCount() > 100)
		{
			return null;
		}
		TowerDefenseEffectSpriteOnce obj = ObjectManager.PoolPop(ObjectManagerConfig.OBJECT.PARTICLES_SPLASH, TowerDefenseGroundItemBase.characterNode) as TowerDefenseEffectSpriteOnce;
		obj.gridPos = gridPos;
		obj.GlobalPosition = shadowSprite.GlobalPosition - new Vector2(0f, 20f);
		return obj;
	}

	public void SetTrack(bool isChange = false)
	{
		gridPos = new Vector2I(gridPos.X, 10);
		trackOpen = true;
		checkAll = true;
		shadowSprite.Visible = false;
		penetrateNum = 0;
		if (!isChange)
		{
			GlobalPosition = new Vector2(GlobalPosition.X, projectileBodyNode.GlobalPosition.Y);
			projectileBodyNode.Position = new Vector2(projectileBodyNode.Position.X, 0f);
		}
		trackCacheFrame = -1;
		trackCacheTarget = null;
		trackCacheValid = false;
		trackConsecutiveNoTarget = 0;
		trackForceSearch = true;
		if (config != null)
		{
			trackSearchInterval = config.trackSearchInterval;
			trackNoTargetInterval = config.trackSearchInterval * 4;
		}
		hitBox.Position = new Vector2(hitBox.Position.X, -20f);
		hitBox.SetDeferred("position", new Vector2(hitBox.Position.X, 0f));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(61)
		{
			new MethodInfo(MethodName.IsHitBoxOutOfBounds, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._CanSpawnEffectThisFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.EndGodotArrayProjectileMetric, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "startTicks", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "items", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMonitorable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitBoxProcessMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RecycleBodyChildren, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFallVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_fireCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "_collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isChange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompleteInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Change, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "changeprojectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "changeCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Update, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "currentFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMonitoring, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanProcessHit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldExpireTrackWithoutTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanHitCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tdChar", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidMapLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldFilterCollisionLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPenetrateDamage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginPenetrateOverlapScan, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndPenetrateOverlapScan, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRegisterPenetrateHit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CheckAabbHits, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessShooter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "currentFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessTrack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "currentFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessTrackCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessCatapult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessCatapult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Move, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetZ, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HitCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "area", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HitEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.HitEffectWithCharacterPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "characterPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hasCharacterPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatSplat, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatSplatWithCharacterPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "characterPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hasCharacterPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlaySplat, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCollision, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maskFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CheckDifferentCamp, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckSameLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Land, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Over, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OverFromExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitTargetFromExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LandFromExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateGridPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckShooterHeight, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldRefreshGrid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "currentFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnableMonitoring, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableMonitoring, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TargetDiedFromExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindTrackTargetCached, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindTrackTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindTrackTargetInternal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockedBounce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCatapultTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSplash, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetTrack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isChange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsHitBoxOutOfBounds && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsHitBoxOutOfBounds());
			return true;
		}
		if (method == MethodName._CanSpawnEffectThisFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanSpawnEffectThisFrame());
			return true;
		}
		if (method == MethodName.EndGodotArrayProjectileMetric && args.Count == 2)
		{
			EndGodotArrayProjectileMetric(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMonitorable && args.Count == 1)
		{
			SetMonitorable(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitBoxProcessMode && args.Count == 1)
		{
			SetHitBoxProcessMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RecycleBodyChildren && args.Count == 0)
		{
			RecycleBodyChildren();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFallVisual && args.Count == 0)
		{
			ApplyFallVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.Recycle && args.Count == 0)
		{
			Recycle();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 8)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteInit && args.Count == 0)
		{
			CompleteInit();
			ret = default;
			return true;
		}
		if (method == MethodName.Change && args.Count == 4)
		{
			Change(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.Update && args.Count == 2)
		{
			Update(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMonitoring && args.Count == 1)
		{
			SetMonitoring(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanProcessHit && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanProcessHit());
			return true;
		}
		if (method == MethodName.ShouldExpireTrackWithoutTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldExpireTrackWithoutTarget());
			return true;
		}
		if (method == MethodName.CanHitCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanHitCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidMapLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidMapLine(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldFilterCollisionLine && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldFilterCollisionLine());
			return true;
		}
		if (method == MethodName.IsPenetrateDamage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPenetrateDamage());
			return true;
		}
		if (method == MethodName.BeginPenetrateOverlapScan && args.Count == 0)
		{
			BeginPenetrateOverlapScan();
			ret = default;
			return true;
		}
		if (method == MethodName.EndPenetrateOverlapScan && args.Count == 0)
		{
			EndPenetrateOverlapScan();
			ret = default;
			return true;
		}
		if (method == MethodName.TryRegisterPenetrateHit && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryRegisterPenetrateHit(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CheckAabbHits && args.Count == 0)
		{
			CheckAabbHits();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessShooter && args.Count == 2)
		{
			ProcessShooter(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessTrack && args.Count == 2)
		{
			ProcessTrack(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessTrackCheck && args.Count == 0)
		{
			ProcessTrackCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessCatapult && args.Count == 1)
		{
			ProcessCatapult(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessCatapult && args.Count == 2)
		{
			ProcessCatapult(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Move && args.Count == 1)
		{
			Move(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetZ && args.Count == 0)
		{
			SetZ();
			ret = default;
			return true;
		}
		if (method == MethodName.HitCharacter && args.Count == 1)
		{
			HitCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HitCheck && args.Count == 1)
		{
			HitCheck(VariantUtils.ConvertTo<AabbArea2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HitEffect && args.Count == 1)
		{
			HitEffect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HitEffectWithCharacterPosition && args.Count == 3)
		{
			HitEffectWithCharacterPosition(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatSplat && args.Count == 1)
		{
			CreatSplat(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatSplatWithCharacterPosition && args.Count == 3)
		{
			CreatSplatWithCharacterPosition(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlaySplat && args.Count == 0)
		{
			PlaySplat();
			ret = default;
			return true;
		}
		if (method == MethodName.CanCollision && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCollision(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CanTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CheckDifferentCamp && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckDifferentCamp(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0])));
			return true;
		}
		if (method == MethodName.CheckSameLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckSameLine(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.Land && args.Count == 0)
		{
			Land();
			ret = default;
			return true;
		}
		if (method == MethodName.Over && args.Count == 0)
		{
			Over();
			ret = default;
			return true;
		}
		if (method == MethodName.OverFromExtension && args.Count == 0)
		{
			OverFromExtension();
			ret = default;
			return true;
		}
		if (method == MethodName.HitTargetFromExtension && args.Count == 0)
		{
			HitTargetFromExtension();
			ret = default;
			return true;
		}
		if (method == MethodName.LandFromExtension && args.Count == 0)
		{
			LandFromExtension();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateGridPos && args.Count == 0)
		{
			UpdateGridPos();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckShooterHeight && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckShooterHeight());
			return true;
		}
		if (method == MethodName.ShouldRefreshGrid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldRefreshGrid(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.EnableMonitoring && args.Count == 0)
		{
			EnableMonitoring();
			ret = default;
			return true;
		}
		if (method == MethodName.DisableMonitoring && args.Count == 0)
		{
			DisableMonitoring();
			ret = default;
			return true;
		}
		if (method == MethodName.TargetDiedFromExtension && args.Count == 0)
		{
			TargetDiedFromExtension();
			ret = default;
			return true;
		}
		if (method == MethodName.FindTrackTargetCached && args.Count == 0)
		{
			FindTrackTargetCached();
			ret = default;
			return true;
		}
		if (method == MethodName.FindTrackTarget && args.Count == 0)
		{
			FindTrackTarget();
			ret = default;
			return true;
		}
		if (method == MethodName.FindTrackTargetInternal && args.Count == 0)
		{
			FindTrackTargetInternal();
			ret = default;
			return true;
		}
		if (method == MethodName.BlockedBounce && args.Count == 0)
		{
			BlockedBounce();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCatapultTarget && args.Count == 0)
		{
			UpdateCatapultTarget();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSplash && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(CreateSplash());
			return true;
		}
		if (method == MethodName.SetTrack && args.Count == 1)
		{
			SetTrack(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._CanSpawnEffectThisFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanSpawnEffectThisFrame());
			return true;
		}
		if (method == MethodName.EndGodotArrayProjectileMetric && args.Count == 2)
		{
			EndGodotArrayProjectileMetric(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsValidMapLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidMapLine(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsHitBoxOutOfBounds)
		{
			return true;
		}
		if (method == MethodName._CanSpawnEffectThisFrame)
		{
			return true;
		}
		if (method == MethodName.EndGodotArrayProjectileMetric)
		{
			return true;
		}
		if (method == MethodName.SetMonitorable)
		{
			return true;
		}
		if (method == MethodName.SetHitBoxProcessMode)
		{
			return true;
		}
		if (method == MethodName.RecycleBodyChildren)
		{
			return true;
		}
		if (method == MethodName.ApplyFallVisual)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.Recycle)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.CompleteInit)
		{
			return true;
		}
		if (method == MethodName.Change)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.Update)
		{
			return true;
		}
		if (method == MethodName.SetMonitoring)
		{
			return true;
		}
		if (method == MethodName.CanProcessHit)
		{
			return true;
		}
		if (method == MethodName.ShouldExpireTrackWithoutTarget)
		{
			return true;
		}
		if (method == MethodName.CanHitCharacter)
		{
			return true;
		}
		if (method == MethodName.IsValidMapLine)
		{
			return true;
		}
		if (method == MethodName.ShouldFilterCollisionLine)
		{
			return true;
		}
		if (method == MethodName.IsPenetrateDamage)
		{
			return true;
		}
		if (method == MethodName.BeginPenetrateOverlapScan)
		{
			return true;
		}
		if (method == MethodName.EndPenetrateOverlapScan)
		{
			return true;
		}
		if (method == MethodName.TryRegisterPenetrateHit)
		{
			return true;
		}
		if (method == MethodName.CheckAabbHits)
		{
			return true;
		}
		if (method == MethodName.ProcessShooter)
		{
			return true;
		}
		if (method == MethodName.ProcessTrack)
		{
			return true;
		}
		if (method == MethodName.ProcessTrackCheck)
		{
			return true;
		}
		if (method == MethodName.ProcessCatapult)
		{
			return true;
		}
		if (method == MethodName.Move)
		{
			return true;
		}
		if (method == MethodName.SetZ)
		{
			return true;
		}
		if (method == MethodName.HitCharacter)
		{
			return true;
		}
		if (method == MethodName.HitCheck)
		{
			return true;
		}
		if (method == MethodName.HitEffect)
		{
			return true;
		}
		if (method == MethodName.HitEffectWithCharacterPosition)
		{
			return true;
		}
		if (method == MethodName.CreatSplat)
		{
			return true;
		}
		if (method == MethodName.CreatSplatWithCharacterPosition)
		{
			return true;
		}
		if (method == MethodName.PlaySplat)
		{
			return true;
		}
		if (method == MethodName.CanCollision)
		{
			return true;
		}
		if (method == MethodName.CanTarget)
		{
			return true;
		}
		if (method == MethodName.CheckDifferentCamp)
		{
			return true;
		}
		if (method == MethodName.CheckSameLine)
		{
			return true;
		}
		if (method == MethodName.Land)
		{
			return true;
		}
		if (method == MethodName.Over)
		{
			return true;
		}
		if (method == MethodName.OverFromExtension)
		{
			return true;
		}
		if (method == MethodName.HitTargetFromExtension)
		{
			return true;
		}
		if (method == MethodName.LandFromExtension)
		{
			return true;
		}
		if (method == MethodName.UpdateGridPos)
		{
			return true;
		}
		if (method == MethodName.CheckShooterHeight)
		{
			return true;
		}
		if (method == MethodName.ShouldRefreshGrid)
		{
			return true;
		}
		if (method == MethodName.EnableMonitoring)
		{
			return true;
		}
		if (method == MethodName.DisableMonitoring)
		{
			return true;
		}
		if (method == MethodName.TargetDiedFromExtension)
		{
			return true;
		}
		if (method == MethodName.FindTrackTargetCached)
		{
			return true;
		}
		if (method == MethodName.FindTrackTarget)
		{
			return true;
		}
		if (method == MethodName.FindTrackTargetInternal)
		{
			return true;
		}
		if (method == MethodName.BlockedBounce)
		{
			return true;
		}
		if (method == MethodName.UpdateCatapultTarget)
		{
			return true;
		}
		if (method == MethodName.CreateSplash)
		{
			return true;
		}
		if (method == MethodName.SetTrack)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.useFall)
		{
			useFall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useGravity)
		{
			useGravity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.projectileBodyNode)
		{
			projectileBodyNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.shadowSprite)
		{
			shadowSprite = VariantUtils.ConvertTo<TowerDefenseShadowVisual>(in value);
			return true;
		}
		if (name == PropertyName.hitBox)
		{
			hitBox = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.rememberPos)
		{
			rememberPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.metaData)
		{
			metaData = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			checkAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.suppressGameplay)
		{
			suppressGameplay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._useFall)
		{
			_useFall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._useGravity)
		{
			_useGravity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.fireCharacter)
		{
			fireCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			velocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName.camp)
		{
			camp = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in value);
			return true;
		}
		if (name == PropertyName.damage)
		{
			damage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			fireMethodFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileHeight)
		{
			projectileHeight = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_HEIGHT>(in value);
			return true;
		}
		if (name == PropertyName.checkHeight)
		{
			checkHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.target)
		{
			target = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.magneticTarget)
		{
			magneticTarget = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.catapultTime)
		{
			catapultTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.catapultTimer)
		{
			catapultTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.catapultTargetPos)
		{
			catapultTargetPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.catapultControlPoint)
		{
			catapultControlPoint = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.catapulCheckLast)
		{
			catapulCheckLast = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.penetrateNum)
		{
			penetrateNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileSprite)
		{
			projectileSprite = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.hitOver)
		{
			hitOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.savePos)
		{
			savePos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.checkDistance)
		{
			checkDistance = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireLength)
		{
			fireLength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.rect)
		{
			rect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.trackOpen)
		{
			trackOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.catapultOpen)
		{
			catapultOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shadowScaleSave)
		{
			shadowScaleSave = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.initSet)
		{
			initSet = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.initHitList)
		{
			initHitList = VariantUtils.ConvertToArray<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.setZInterval)
		{
			setZInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.moveTween)
		{
			moveTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.randFreshIndex)
		{
			randFreshIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gridRefreshScheduleInitialized)
		{
			_gridRefreshScheduleInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nextGridRefreshFrame)
		{
			_nextGridRefreshFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheInitialized)
		{
			_cellGroundHeightCacheInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheValid)
		{
			_cellGroundHeightCacheValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheCell)
		{
			_cellGroundHeightCacheCell = VariantUtils.ConvertTo<TowerDefenseCellInstance>(in value);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheCurve)
		{
			_cellGroundHeightCacheCurve = VariantUtils.ConvertTo<CurveTexture>(in value);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheValue)
		{
			_cellGroundHeightCacheValue = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireDirX)
		{
			fireDirX = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isShooter)
		{
			isShooter = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.extId)
		{
			extId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.blocked)
		{
			blocked = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.trackCacheFrame)
		{
			trackCacheFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.trackCacheTarget)
		{
			trackCacheTarget = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.trackCacheValid)
		{
			trackCacheValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.trackSearchInterval)
		{
			trackSearchInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.trackNoTargetInterval)
		{
			trackNoTargetInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.trackConsecutiveNoTarget)
		{
			trackConsecutiveNoTarget = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.trackMaxConsecutiveNoTarget)
		{
			trackMaxConsecutiveNoTarget = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.trackForceSearch)
		{
			trackForceSearch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cacheRotateFollowVelocity)
		{
			_cacheRotateFollowVelocity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cacheRotateScale)
		{
			_cacheRotateScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._monitoringState)
		{
			_monitoringState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._monitorableState)
		{
			_monitorableState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._processModeEnabled)
		{
			_processModeEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._bodyCleaned)
		{
			_bodyCleaned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._initPending)
		{
			_initPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.useFall)
		{
			from = useFall;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.useGravity)
		{
			from = useGravity;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.WorldHitRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(WorldHitRect);
			return true;
		}
		if (name == PropertyName.projectileBodyNode)
		{
			value = VariantUtils.CreateFrom(in projectileBodyNode);
			return true;
		}
		if (name == PropertyName.shadowSprite)
		{
			value = VariantUtils.CreateFrom(in shadowSprite);
			return true;
		}
		if (name == PropertyName.hitBox)
		{
			value = VariantUtils.CreateFrom(in hitBox);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.rememberPos)
		{
			value = VariantUtils.CreateFrom(in rememberPos);
			return true;
		}
		if (name == PropertyName.metaData)
		{
			value = VariantUtils.CreateFrom(in metaData);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			value = VariantUtils.CreateFrom(in checkAll);
			return true;
		}
		if (name == PropertyName.suppressGameplay)
		{
			value = VariantUtils.CreateFrom(in suppressGameplay);
			return true;
		}
		if (name == PropertyName._useFall)
		{
			value = VariantUtils.CreateFrom(in _useFall);
			return true;
		}
		if (name == PropertyName._useGravity)
		{
			value = VariantUtils.CreateFrom(in _useGravity);
			return true;
		}
		if (name == PropertyName.fireCharacter)
		{
			value = VariantUtils.CreateFrom(in fireCharacter);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			value = VariantUtils.CreateFrom(in velocity);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.camp)
		{
			value = VariantUtils.CreateFrom(in camp);
			return true;
		}
		if (name == PropertyName.damage)
		{
			value = VariantUtils.CreateFrom(in damage);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			value = VariantUtils.CreateFrom(in damageFlags);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			value = VariantUtils.CreateFrom(in fireMethodFlags);
			return true;
		}
		if (name == PropertyName.projectileHeight)
		{
			value = VariantUtils.CreateFrom(in projectileHeight);
			return true;
		}
		if (name == PropertyName.checkHeight)
		{
			value = VariantUtils.CreateFrom(in checkHeight);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.target)
		{
			value = VariantUtils.CreateFrom(in target);
			return true;
		}
		if (name == PropertyName.magneticTarget)
		{
			value = VariantUtils.CreateFrom(in magneticTarget);
			return true;
		}
		if (name == PropertyName.catapultTime)
		{
			value = VariantUtils.CreateFrom(in catapultTime);
			return true;
		}
		if (name == PropertyName.catapultTimer)
		{
			value = VariantUtils.CreateFrom(in catapultTimer);
			return true;
		}
		if (name == PropertyName.catapultTargetPos)
		{
			value = VariantUtils.CreateFrom(in catapultTargetPos);
			return true;
		}
		if (name == PropertyName.catapultControlPoint)
		{
			value = VariantUtils.CreateFrom(in catapultControlPoint);
			return true;
		}
		if (name == PropertyName.catapulCheckLast)
		{
			value = VariantUtils.CreateFrom(in catapulCheckLast);
			return true;
		}
		if (name == PropertyName.penetrateNum)
		{
			value = VariantUtils.CreateFrom(in penetrateNum);
			return true;
		}
		if (name == PropertyName.projectileSprite)
		{
			value = VariantUtils.CreateFrom(in projectileSprite);
			return true;
		}
		if (name == PropertyName.hitOver)
		{
			value = VariantUtils.CreateFrom(in hitOver);
			return true;
		}
		if (name == PropertyName.savePos)
		{
			value = VariantUtils.CreateFrom(in savePos);
			return true;
		}
		if (name == PropertyName.checkDistance)
		{
			value = VariantUtils.CreateFrom(in checkDistance);
			return true;
		}
		if (name == PropertyName.fireLength)
		{
			value = VariantUtils.CreateFrom(in fireLength);
			return true;
		}
		if (name == PropertyName.rect)
		{
			value = VariantUtils.CreateFrom(in rect);
			return true;
		}
		if (name == PropertyName.trackOpen)
		{
			value = VariantUtils.CreateFrom(in trackOpen);
			return true;
		}
		if (name == PropertyName.catapultOpen)
		{
			value = VariantUtils.CreateFrom(in catapultOpen);
			return true;
		}
		if (name == PropertyName.shadowScaleSave)
		{
			value = VariantUtils.CreateFrom(in shadowScaleSave);
			return true;
		}
		if (name == PropertyName.initSet)
		{
			value = VariantUtils.CreateFrom(in initSet);
			return true;
		}
		if (name == PropertyName.initHitList)
		{
			value = VariantUtils.CreateFromArray(initHitList);
			return true;
		}
		if (name == PropertyName.setZInterval)
		{
			value = VariantUtils.CreateFrom(in setZInterval);
			return true;
		}
		if (name == PropertyName.moveTween)
		{
			value = VariantUtils.CreateFrom(in moveTween);
			return true;
		}
		if (name == PropertyName.randFreshIndex)
		{
			value = VariantUtils.CreateFrom(in randFreshIndex);
			return true;
		}
		if (name == PropertyName._gridRefreshScheduleInitialized)
		{
			value = VariantUtils.CreateFrom(in _gridRefreshScheduleInitialized);
			return true;
		}
		if (name == PropertyName._nextGridRefreshFrame)
		{
			value = VariantUtils.CreateFrom(in _nextGridRefreshFrame);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheInitialized)
		{
			value = VariantUtils.CreateFrom(in _cellGroundHeightCacheInitialized);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheValid)
		{
			value = VariantUtils.CreateFrom(in _cellGroundHeightCacheValid);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheCell)
		{
			value = VariantUtils.CreateFrom(in _cellGroundHeightCacheCell);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheCurve)
		{
			value = VariantUtils.CreateFrom(in _cellGroundHeightCacheCurve);
			return true;
		}
		if (name == PropertyName._cellGroundHeightCacheValue)
		{
			value = VariantUtils.CreateFrom(in _cellGroundHeightCacheValue);
			return true;
		}
		if (name == PropertyName.fireDirX)
		{
			value = VariantUtils.CreateFrom(in fireDirX);
			return true;
		}
		if (name == PropertyName.isShooter)
		{
			value = VariantUtils.CreateFrom(in isShooter);
			return true;
		}
		if (name == PropertyName.extId)
		{
			value = VariantUtils.CreateFrom(in extId);
			return true;
		}
		if (name == PropertyName.blocked)
		{
			value = VariantUtils.CreateFrom(in blocked);
			return true;
		}
		if (name == PropertyName.trackCacheFrame)
		{
			value = VariantUtils.CreateFrom(in trackCacheFrame);
			return true;
		}
		if (name == PropertyName.trackCacheTarget)
		{
			value = VariantUtils.CreateFrom(in trackCacheTarget);
			return true;
		}
		if (name == PropertyName.trackCacheValid)
		{
			value = VariantUtils.CreateFrom(in trackCacheValid);
			return true;
		}
		if (name == PropertyName.trackSearchInterval)
		{
			value = VariantUtils.CreateFrom(in trackSearchInterval);
			return true;
		}
		if (name == PropertyName.trackNoTargetInterval)
		{
			value = VariantUtils.CreateFrom(in trackNoTargetInterval);
			return true;
		}
		if (name == PropertyName.trackConsecutiveNoTarget)
		{
			value = VariantUtils.CreateFrom(in trackConsecutiveNoTarget);
			return true;
		}
		if (name == PropertyName.trackMaxConsecutiveNoTarget)
		{
			value = VariantUtils.CreateFrom(in trackMaxConsecutiveNoTarget);
			return true;
		}
		if (name == PropertyName.trackForceSearch)
		{
			value = VariantUtils.CreateFrom(in trackForceSearch);
			return true;
		}
		if (name == PropertyName._cacheRotateFollowVelocity)
		{
			value = VariantUtils.CreateFrom(in _cacheRotateFollowVelocity);
			return true;
		}
		if (name == PropertyName._cacheRotateScale)
		{
			value = VariantUtils.CreateFrom(in _cacheRotateScale);
			return true;
		}
		if (name == PropertyName._monitoringState)
		{
			value = VariantUtils.CreateFrom(in _monitoringState);
			return true;
		}
		if (name == PropertyName._monitorableState)
		{
			value = VariantUtils.CreateFrom(in _monitorableState);
			return true;
		}
		if (name == PropertyName._processModeEnabled)
		{
			value = VariantUtils.CreateFrom(in _processModeEnabled);
			return true;
		}
		if (name == PropertyName._bodyCleaned)
		{
			value = VariantUtils.CreateFrom(in _bodyCleaned);
			return true;
		}
		if (name == PropertyName._initPending)
		{
			value = VariantUtils.CreateFrom(in _initPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileBodyNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shadowSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hitBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.rememberPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.metaData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suppressGameplay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._useFall, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useFall, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._useGravity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useGravity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fireCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.camp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.damage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireMethodFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.projectileHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.magneticTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.catapultTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.catapultTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.catapultTargetPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.catapultControlPoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.catapulCheckLast, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.penetrateNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.savePos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.checkDistance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireLength, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.rect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldHitRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.catapultOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.shadowScaleSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.initSet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.initHitList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.setZInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moveTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.randFreshIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gridRefreshScheduleInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextGridRefreshFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cellGroundHeightCacheInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cellGroundHeightCacheValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cellGroundHeightCacheCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cellGroundHeightCacheCurve, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cellGroundHeightCacheValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireDirX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isShooter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.extId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.blocked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.trackCacheFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.trackCacheTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackCacheValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.trackSearchInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.trackNoTargetInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.trackConsecutiveNoTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.trackMaxConsecutiveNoTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackForceSearch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cacheRotateFollowVelocity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cacheRotateScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._monitoringState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._monitorableState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._processModeEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bodyCleaned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._initPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.useFall, Variant.From<bool>(useFall));
		info.AddProperty(PropertyName.useGravity, Variant.From<bool>(useGravity));
		info.AddProperty(PropertyName.projectileBodyNode, Variant.From(in projectileBodyNode));
		info.AddProperty(PropertyName.shadowSprite, Variant.From(in shadowSprite));
		info.AddProperty(PropertyName.hitBox, Variant.From(in hitBox));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.rememberPos, Variant.From(in rememberPos));
		info.AddProperty(PropertyName.metaData, Variant.From(in metaData));
		info.AddProperty(PropertyName.checkAll, Variant.From(in checkAll));
		info.AddProperty(PropertyName.suppressGameplay, Variant.From(in suppressGameplay));
		info.AddProperty(PropertyName._useFall, Variant.From(in _useFall));
		info.AddProperty(PropertyName._useGravity, Variant.From(in _useGravity));
		info.AddProperty(PropertyName.fireCharacter, Variant.From(in fireCharacter));
		info.AddProperty(PropertyName.velocity, Variant.From(in velocity));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.camp, Variant.From(in camp));
		info.AddProperty(PropertyName.damage, Variant.From(in damage));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
		info.AddProperty(PropertyName.damageFlags, Variant.From(in damageFlags));
		info.AddProperty(PropertyName.fireMethodFlags, Variant.From(in fireMethodFlags));
		info.AddProperty(PropertyName.projectileHeight, Variant.From(in projectileHeight));
		info.AddProperty(PropertyName.checkHeight, Variant.From(in checkHeight));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.target, Variant.From(in target));
		info.AddProperty(PropertyName.magneticTarget, Variant.From(in magneticTarget));
		info.AddProperty(PropertyName.catapultTime, Variant.From(in catapultTime));
		info.AddProperty(PropertyName.catapultTimer, Variant.From(in catapultTimer));
		info.AddProperty(PropertyName.catapultTargetPos, Variant.From(in catapultTargetPos));
		info.AddProperty(PropertyName.catapultControlPoint, Variant.From(in catapultControlPoint));
		info.AddProperty(PropertyName.catapulCheckLast, Variant.From(in catapulCheckLast));
		info.AddProperty(PropertyName.penetrateNum, Variant.From(in penetrateNum));
		info.AddProperty(PropertyName.projectileSprite, Variant.From(in projectileSprite));
		info.AddProperty(PropertyName.hitOver, Variant.From(in hitOver));
		info.AddProperty(PropertyName.savePos, Variant.From(in savePos));
		info.AddProperty(PropertyName.checkDistance, Variant.From(in checkDistance));
		info.AddProperty(PropertyName.fireLength, Variant.From(in fireLength));
		info.AddProperty(PropertyName.rect, Variant.From(in rect));
		info.AddProperty(PropertyName.trackOpen, Variant.From(in trackOpen));
		info.AddProperty(PropertyName.catapultOpen, Variant.From(in catapultOpen));
		info.AddProperty(PropertyName.shadowScaleSave, Variant.From(in shadowScaleSave));
		info.AddProperty(PropertyName.initSet, Variant.From(in initSet));
		info.AddProperty(PropertyName.initHitList, Variant.CreateFrom(initHitList));
		info.AddProperty(PropertyName.setZInterval, Variant.From(in setZInterval));
		info.AddProperty(PropertyName.moveTween, Variant.From(in moveTween));
		info.AddProperty(PropertyName.randFreshIndex, Variant.From(in randFreshIndex));
		info.AddProperty(PropertyName._gridRefreshScheduleInitialized, Variant.From(in _gridRefreshScheduleInitialized));
		info.AddProperty(PropertyName._nextGridRefreshFrame, Variant.From(in _nextGridRefreshFrame));
		info.AddProperty(PropertyName._cellGroundHeightCacheInitialized, Variant.From(in _cellGroundHeightCacheInitialized));
		info.AddProperty(PropertyName._cellGroundHeightCacheValid, Variant.From(in _cellGroundHeightCacheValid));
		info.AddProperty(PropertyName._cellGroundHeightCacheCell, Variant.From(in _cellGroundHeightCacheCell));
		info.AddProperty(PropertyName._cellGroundHeightCacheCurve, Variant.From(in _cellGroundHeightCacheCurve));
		info.AddProperty(PropertyName._cellGroundHeightCacheValue, Variant.From(in _cellGroundHeightCacheValue));
		info.AddProperty(PropertyName.fireDirX, Variant.From(in fireDirX));
		info.AddProperty(PropertyName.isShooter, Variant.From(in isShooter));
		info.AddProperty(PropertyName.extId, Variant.From(in extId));
		info.AddProperty(PropertyName.blocked, Variant.From(in blocked));
		info.AddProperty(PropertyName.trackCacheFrame, Variant.From(in trackCacheFrame));
		info.AddProperty(PropertyName.trackCacheTarget, Variant.From(in trackCacheTarget));
		info.AddProperty(PropertyName.trackCacheValid, Variant.From(in trackCacheValid));
		info.AddProperty(PropertyName.trackSearchInterval, Variant.From(in trackSearchInterval));
		info.AddProperty(PropertyName.trackNoTargetInterval, Variant.From(in trackNoTargetInterval));
		info.AddProperty(PropertyName.trackConsecutiveNoTarget, Variant.From(in trackConsecutiveNoTarget));
		info.AddProperty(PropertyName.trackMaxConsecutiveNoTarget, Variant.From(in trackMaxConsecutiveNoTarget));
		info.AddProperty(PropertyName.trackForceSearch, Variant.From(in trackForceSearch));
		info.AddProperty(PropertyName._cacheRotateFollowVelocity, Variant.From(in _cacheRotateFollowVelocity));
		info.AddProperty(PropertyName._cacheRotateScale, Variant.From(in _cacheRotateScale));
		info.AddProperty(PropertyName._monitoringState, Variant.From(in _monitoringState));
		info.AddProperty(PropertyName._monitorableState, Variant.From(in _monitorableState));
		info.AddProperty(PropertyName._processModeEnabled, Variant.From(in _processModeEnabled));
		info.AddProperty(PropertyName._bodyCleaned, Variant.From(in _bodyCleaned));
		info.AddProperty(PropertyName._initPending, Variant.From(in _initPending));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.useFall, out var value))
		{
			useFall = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useGravity, out var value2))
		{
			useGravity = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.projectileBodyNode, out var value3))
		{
			projectileBodyNode = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.shadowSprite, out var value4))
		{
			shadowSprite = value4.As<TowerDefenseShadowVisual>();
		}
		if (info.TryGetProperty(PropertyName.hitBox, out var value5))
		{
			hitBox = value5.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value6))
		{
			over = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rememberPos, out var value7))
		{
			rememberPos = value7.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.metaData, out var value8))
		{
			metaData = value8.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.checkAll, out var value9))
		{
			checkAll = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.suppressGameplay, out var value10))
		{
			suppressGameplay = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._useFall, out var value11))
		{
			_useFall = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._useGravity, out var value12))
		{
			_useGravity = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.fireCharacter, out var value13))
		{
			fireCharacter = value13.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.velocity, out var value14))
		{
			velocity = value14.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value15))
		{
			speed = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value16))
		{
			config = value16.As<TowerDefenseProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName.camp, out var value17))
		{
			camp = value17.As<TowerDefenseEnum.CHARACTER_CAMP>();
		}
		if (info.TryGetProperty(PropertyName.damage, out var value18))
		{
			damage = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value19))
		{
			collisionFlags = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName.damageFlags, out var value20))
		{
			damageFlags = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireMethodFlags, out var value21))
		{
			fireMethodFlags = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileHeight, out var value22))
		{
			projectileHeight = value22.As<TowerDefenseEnum.CHARACTER_HEIGHT>();
		}
		if (info.TryGetProperty(PropertyName.checkHeight, out var value23))
		{
			checkHeight = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value24))
		{
			height = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName.target, out var value25))
		{
			target = value25.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.magneticTarget, out var value26))
		{
			magneticTarget = value26.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.catapultTime, out var value27))
		{
			catapultTime = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.catapultTimer, out var value28))
		{
			catapultTimer = value28.As<double>();
		}
		if (info.TryGetProperty(PropertyName.catapultTargetPos, out var value29))
		{
			catapultTargetPos = value29.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.catapultControlPoint, out var value30))
		{
			catapultControlPoint = value30.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.catapulCheckLast, out var value31))
		{
			catapulCheckLast = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.penetrateNum, out var value32))
		{
			penetrateNum = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileSprite, out var value33))
		{
			projectileSprite = value33.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.hitOver, out var value34))
		{
			hitOver = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.savePos, out var value35))
		{
			savePos = value35.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.checkDistance, out var value36))
		{
			checkDistance = value36.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireLength, out var value37))
		{
			fireLength = value37.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rect, out var value38))
		{
			rect = value38.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.trackOpen, out var value39))
		{
			trackOpen = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.catapultOpen, out var value40))
		{
			catapultOpen = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shadowScaleSave, out var value41))
		{
			shadowScaleSave = value41.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.initSet, out var value42))
		{
			initSet = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.initHitList, out var value43))
		{
			initHitList = value43.AsGodotArray<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.setZInterval, out var value44))
		{
			setZInterval = value44.As<int>();
		}
		if (info.TryGetProperty(PropertyName.moveTween, out var value45))
		{
			moveTween = value45.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.randFreshIndex, out var value46))
		{
			randFreshIndex = value46.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gridRefreshScheduleInitialized, out var value47))
		{
			_gridRefreshScheduleInitialized = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nextGridRefreshFrame, out var value48))
		{
			_nextGridRefreshFrame = value48.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cellGroundHeightCacheInitialized, out var value49))
		{
			_cellGroundHeightCacheInitialized = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cellGroundHeightCacheValid, out var value50))
		{
			_cellGroundHeightCacheValid = value50.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cellGroundHeightCacheCell, out var value51))
		{
			_cellGroundHeightCacheCell = value51.As<TowerDefenseCellInstance>();
		}
		if (info.TryGetProperty(PropertyName._cellGroundHeightCacheCurve, out var value52))
		{
			_cellGroundHeightCacheCurve = value52.As<CurveTexture>();
		}
		if (info.TryGetProperty(PropertyName._cellGroundHeightCacheValue, out var value53))
		{
			_cellGroundHeightCacheValue = value53.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireDirX, out var value54))
		{
			fireDirX = value54.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isShooter, out var value55))
		{
			isShooter = value55.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.extId, out var value56))
		{
			extId = value56.As<int>();
		}
		if (info.TryGetProperty(PropertyName.blocked, out var value57))
		{
			blocked = value57.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.trackCacheFrame, out var value58))
		{
			trackCacheFrame = value58.As<int>();
		}
		if (info.TryGetProperty(PropertyName.trackCacheTarget, out var value59))
		{
			trackCacheTarget = value59.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.trackCacheValid, out var value60))
		{
			trackCacheValid = value60.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.trackSearchInterval, out var value61))
		{
			trackSearchInterval = value61.As<int>();
		}
		if (info.TryGetProperty(PropertyName.trackNoTargetInterval, out var value62))
		{
			trackNoTargetInterval = value62.As<int>();
		}
		if (info.TryGetProperty(PropertyName.trackConsecutiveNoTarget, out var value63))
		{
			trackConsecutiveNoTarget = value63.As<int>();
		}
		if (info.TryGetProperty(PropertyName.trackMaxConsecutiveNoTarget, out var value64))
		{
			trackMaxConsecutiveNoTarget = value64.As<int>();
		}
		if (info.TryGetProperty(PropertyName.trackForceSearch, out var value65))
		{
			trackForceSearch = value65.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cacheRotateFollowVelocity, out var value66))
		{
			_cacheRotateFollowVelocity = value66.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cacheRotateScale, out var value67))
		{
			_cacheRotateScale = value67.As<float>();
		}
		if (info.TryGetProperty(PropertyName._monitoringState, out var value68))
		{
			_monitoringState = value68.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._monitorableState, out var value69))
		{
			_monitorableState = value69.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._processModeEnabled, out var value70))
		{
			_processModeEnabled = value70.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bodyCleaned, out var value71))
		{
			_bodyCleaned = value71.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._initPending, out var value72))
		{
			_initPending = value72.As<bool>();
		}
	}
}
