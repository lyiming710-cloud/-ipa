using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Sun/TowerDefenseSunBase.cs")]
public class TowerDefenseSunBase : Node2D, IObjectPoolLifecycle
{
	public delegate void CollectEventHandler(long num);

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName GetGroupName = "GetGroupName";

		public static readonly StringName GetPoolKey = "GetPoolKey";

		public static readonly StringName GetDropItemConfig = "GetDropItemConfig";

		public static readonly StringName OnCollectStart = "OnCollectStart";

		public static readonly StringName GetCollectValue = "GetCollectValue";

		public static readonly StringName OnDieDown = "OnDieDown";

		public static readonly StringName ShouldAutoCollect = "ShouldAutoCollect";

		public static readonly StringName OnReady = "OnReady";

		public static readonly StringName OnRefresh = "OnRefresh";

		public static readonly StringName OnInitScaleTween = "OnInitScaleTween";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName Recycle = "Recycle";

		public static readonly StringName Init = "Init";

		public static readonly StringName RestoreAutoCollect = "RestoreAutoCollect";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName EnsurePoolLifecyclePolicy = "EnsurePoolLifecyclePolicy";

		public static readonly StringName RestartSunAnimation = "RestartSunAnimation";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName OnMovementActivityChanged = "OnMovementActivityChanged";

		public static readonly StringName Collection = "Collection";

		public static readonly StringName FinishCollectionFlightCallback = "FinishCollectionFlightCallback";

		public static readonly StringName StartFadeAndDestroy = "StartFadeAndDestroy";

		public static readonly StringName FinishFadeAndDestroyCallback = "FinishFadeAndDestroyCallback";

		public static readonly StringName DieDown = "DieDown";

		public static readonly StringName Destroy = "Destroy";

		public static readonly StringName RemoveSunGroups = "RemoveSunGroups";

		public static readonly StringName BeginLease = "BeginLease";

		public static readonly StringName InvalidateLease = "InvalidateLease";

		public static readonly StringName IsCurrentLease = "IsCurrentLease";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ScheduleRefreshAfterPhysics = "ScheduleRefreshAfterPhysics";

		public static readonly StringName OnRefreshPhysicsFrame = "OnRefreshPhysicsFrame";

		public static readonly StringName ScheduleMoveStop = "ScheduleMoveStop";

		public static readonly StringName OnMoveStopTimeout = "OnMoveStopTimeout";

		public static readonly StringName ScheduleAutoCollect = "ScheduleAutoCollect";

		public static readonly StringName OnAutoCollectTimeout = "OnAutoCollectTimeout";

		public static readonly StringName CancelDeferredCallbacks = "CancelDeferredCallbacks";

		public static readonly StringName CancelRefreshFrameCallback = "CancelRefreshFrameCallback";

		public static readonly StringName CancelMoveStopTimer = "CancelMoveStopTimer";

		public static readonly StringName CancelAutoCollectTimer = "CancelAutoCollectTimer";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName sprite = "sprite";

		public static readonly StringName moveComponent = "moveComponent";

		public static readonly StringName light = "light";

		public static readonly StringName dieDownTimer = "dieDownTimer";

		public static readonly StringName OwnershipPolicy = "OwnershipPolicy";

		public static readonly StringName UsesLegacyLocalEconomy = "UsesLegacyLocalEconomy";

		public static readonly StringName _sprite = "_sprite";

		public static readonly StringName _moveComponent = "_moveComponent";

		public static readonly StringName _light = "_light";

		public static readonly StringName _dieDownTimer = "_dieDownTimer";

		public static readonly StringName _initScaleTween = "_initScaleTween";

		public static readonly StringName _collectionTween = "_collectionTween";

		public static readonly StringName _fadeTween = "_fadeTween";

		public static readonly StringName _refreshFrameTree = "_refreshFrameTree";

		public static readonly StringName _moveStopTimer = "_moveStopTimer";

		public static readonly StringName _autoCollectTimer = "_autoCollectTimer";

		public static readonly StringName _groupName = "_groupName";

		public static readonly StringName _refreshFrameLeaseVersion = "_refreshFrameLeaseVersion";

		public static readonly StringName _moveStopLeaseVersion = "_moveStopLeaseVersion";

		public static readonly StringName _autoCollectLeaseVersion = "_autoCollectLeaseVersion";

		public static readonly StringName _collectionLeaseVersion = "_collectionLeaseVersion";

		public static readonly StringName _fadeLeaseVersion = "_fadeLeaseVersion";

		public static readonly StringName _collectionValue = "_collectionValue";

		public static readonly StringName _collectionTweenCallback = "_collectionTweenCallback";

		public static readonly StringName _fadeTweenCallback = "_fadeTweenCallback";

		public static readonly StringName _leaseVersion = "_leaseVersion";

		public static readonly StringName _restoredLeaseVersion = "_restoredLeaseVersion";

		public static readonly StringName _autoCollectScheduledLease = "_autoCollectScheduledLease";

		public static readonly StringName _supportsDirectPoolLifecycleDispatch = "_supportsDirectPoolLifecycleDispatch";

		public static readonly StringName _poolLifecyclePolicyInitialized = "_poolLifecyclePolicyInitialized";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName height = "height";

		public static readonly StringName isCollect = "isCollect";

		public static readonly StringName die = "die";

		public static readonly StringName movingMethod = "movingMethod";

		public static readonly StringName over = "over";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName view = "view";

		public static readonly StringName camera = "camera";

		public static readonly StringName viewSize = "viewSize";

		public static readonly StringName autoCollect = "autoCollect";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public const string SaveGroupName = "SunDropItem";

	private const float InitialVisualScaleFactor = 0.25f;

	private static readonly StringName SaveGroupNameKey = new StringName("SunDropItem");

	private AdobeAnimateSprite _sprite;

	private MoveComponent _moveComponent;

	private PointLight2D _light;

	private Timer _dieDownTimer;

	private Tween _initScaleTween;

	private Tween _collectionTween;

	private Tween _fadeTween;

	private SceneTree _refreshFrameTree;

	private Action _refreshFrameCallback;

	private SceneTreeTimer _moveStopTimer;

	private Action _moveStopCallback;

	private SceneTreeTimer _autoCollectTimer;

	private Action _autoCollectCallback;

	private StringName _groupName;

	private int _refreshFrameLeaseVersion = -1;

	private int _moveStopLeaseVersion = -1;

	private int _autoCollectLeaseVersion = -1;

	private int _collectionLeaseVersion = -1;

	private int _fadeLeaseVersion = -1;

	private DropItemCollectionContext _collectionContext;

	private long _collectionValue;

	private Callable _collectionTweenCallback;

	private Callable _fadeTweenCallback;

	private int _leaseVersion;

	private int _restoredLeaseVersion = -1;

	private int _autoCollectScheduledLease = -1;

	private bool _supportsDirectPoolLifecycleDispatch;

	private bool _poolLifecyclePolicyInitialized;

	public long sunNum = 25L;

	public double height = 500.0;

	public bool isCollect;

	public bool die;

	public TowerDefenseEnum.SUN_MOVING_METHOD movingMethod;

	public bool over;

	public Vector2I gridPos = new Vector2I(-1, -1);

	public Viewport view;

	public Camera2D camera;

	public Vector2 viewSize;

	public bool autoCollect;

	public AdobeAnimateSprite sprite => _sprite;

	public MoveComponent moveComponent => _moveComponent;

	public PointLight2D light => _light;

	public Timer dieDownTimer => _dieDownTimer;

	public EconomyAccountId AccountId { get; private set; }

	public SunDropOwnershipPolicy OwnershipPolicy { get; private set; }

	public bool UsesLegacyLocalEconomy => OwnershipPolicy == SunDropOwnershipPolicy.LegacySharedReplica;

	bool IObjectPoolLifecycle.SupportsDirectPoolLifecycleDispatch
	{
		get
		{
			EnsurePoolLifecyclePolicy();
			return _supportsDirectPoolLifecycleDispatch;
		}
	}

	public event CollectEventHandler OnCollect;

	public virtual string GetGroupName()
	{
		return "Sun";
	}

	public virtual ObjectManagerConfig.OBJECT GetPoolKey()
	{
		return ObjectManagerConfig.OBJECT.SUN;
	}

	public DropItemConfig GetDropItemConfig()
	{
		return DropItemRegistry.GetById(GetPoolKey());
	}

	public virtual void OnCollectStart()
	{
	}

	public virtual long GetCollectValue()
	{
		DropItemConfig dropItemConfig = GetDropItemConfig();
		if (dropItemConfig != null && dropItemConfig.Handler != null)
		{
			return dropItemConfig.Handler.GetCollectValue(CreateCollectionContext(sunNum));
		}
		return sunNum;
	}

	public virtual bool OnDieDown()
	{
		return false;
	}

	public virtual bool ShouldAutoCollect()
	{
		DropItemConfig dropItemConfig = GetDropItemConfig();
		if (dropItemConfig != null && dropItemConfig.Handler != null && dropItemConfig.Handler.ShouldAutoCollect(CreateCollectionContext(sunNum)))
		{
			return true;
		}
		return autoCollect;
	}

	public virtual void OnReady()
	{
	}

	public virtual void OnRefresh()
	{
	}

	public virtual void OnInitScaleTween(Tween tween)
	{
	}

	public void Refresh()
	{
		int leaseVersion = BeginLease();
		SetPhysicsProcess(enable: true);
		if (_groupName == null || _groupName.IsEmpty)
		{
			_groupName = new StringName(GetGroupName());
		}
		AddToGroup(_groupName, persistent: true);
		AddToGroup(SaveGroupNameKey, persistent: true);
		AccountId = default;
		OwnershipPolicy = SunDropOwnershipPolicy.LegacySharedReplica;
		_sprite.Position = Vector2.Zero;
		sunNum = 25L;
		height = 500.0;
		isCollect = false;
		die = false;
		over = false;
		_sprite.Scale = Vector2.One * 0.25f;
		view = GetViewport();
		camera = view.GetCamera2D();
		viewSize = view.GetVisibleRect().Size;
		if (GodotObject.IsInstanceValid(_light))
		{
			_light.Visible = TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
		}
		RestartSunAnimation();
		Color modulate = _sprite.Modulate;
		modulate.A = 1f;
		_sprite.Modulate = modulate;
		if (GodotObject.IsInstanceValid(_dieDownTimer))
		{
			_dieDownTimer.Start();
		}
		OnRefresh();
		ScheduleRefreshAfterPhysics(leaseVersion);
	}

	public virtual void Recycle()
	{
		InvalidateLease();
		RemoveSunGroups();
		if (GodotObject.IsInstanceValid(_dieDownTimer))
		{
			_dieDownTimer.Stop();
		}
		_moveComponent?.MoveClear();
		SetPhysicsProcess(enable: false);
		autoCollect = false;
		OnCollect = null;
		AccountId = default;
		OwnershipPolicy = SunDropOwnershipPolicy.LegacySharedReplica;
	}

	void IObjectPoolLifecycle.RefreshFromPool()
	{
		Refresh();
	}

	void IObjectPoolLifecycle.RecycleToPool()
	{
		Recycle();
	}

	public void Init(long _sunNum, TowerDefenseEnum.SUN_MOVING_METHOD _movingMethod, double _height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0, double moverStopTime = -1.0)
	{
		InitCore(EconomyAccountId.Local, SunDropOwnershipPolicy.LegacySharedReplica, _sunNum, _movingMethod, _height, velocity, gravity, moverStopTime);
	}

	public void Init(EconomyAccountId accountId, long _sunNum, TowerDefenseEnum.SUN_MOVING_METHOD _movingMethod, double _height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0, double moverStopTime = -1.0)
	{
		InitCore(accountId.IsValid ? accountId : EconomyAccountId.Local, accountId.IsValid ? SunDropOwnershipPolicy.AccountOwned : SunDropOwnershipPolicy.LegacySharedReplica, _sunNum, _movingMethod, _height, velocity, gravity, moverStopTime);
	}

	public void InitFromSave(EconomyAccountId accountId, SunDropOwnershipPolicy ownershipPolicy, long _sunNum, TowerDefenseEnum.SUN_MOVING_METHOD _movingMethod, double _height = 0.0, Vector2 velocity = default(Vector2), double gravity = 0.0, double moverStopTime = -1.0)
	{
		bool flag = !accountId.IsValid || ownershipPolicy != SunDropOwnershipPolicy.AccountOwned;
		InitCore(flag ? EconomyAccountId.Local : accountId, (!flag) ? SunDropOwnershipPolicy.AccountOwned : SunDropOwnershipPolicy.LegacySharedReplica, _sunNum, _movingMethod, _height, velocity, gravity, moverStopTime);
		_restoredLeaseVersion = _leaseVersion;
	}

	public void RestoreAutoCollect(bool savedAutoCollect)
	{
		autoCollect = savedAutoCollect;
		_restoredLeaseVersion = _leaseVersion;
		if (ShouldAutoCollect())
		{
			SetPhysicsProcess(enable: false);
			ScheduleAutoCollect(_leaseVersion);
		}
	}

	private void InitCore(EconomyAccountId accountId, SunDropOwnershipPolicy ownershipPolicy, long _sunNum, TowerDefenseEnum.SUN_MOVING_METHOD _movingMethod, double _height, Vector2 velocity, double gravity, double moverStopTime)
	{
		int leaseVersion = _leaseVersion;
		AccountId = accountId;
		OwnershipPolicy = ownershipPolicy;
		sunNum = _sunNum;
		movingMethod = _movingMethod;
		height = _height;
		switch (_movingMethod)
		{
		case TowerDefenseEnum.SUN_MOVING_METHOD.LAND:
			_moveComponent.SetVelocity(velocity);
			break;
		case TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY:
			_moveComponent.SetVelocity(velocity);
			_moveComponent.SetGravity(gravity);
			break;
		}
		SetPhysicsProcess(_moveComponent.HasActiveMovement);
		float num = Mathf.Abs((float)_sunNum / 25f);
		if (num > 2f)
		{
			num = 2f;
		}
		KillTween(ref _initScaleTween);
		Vector2 vector = Vector2.One * num * 0.25f;
		_sprite.Scale = vector;
		_initScaleTween = CreateTween();
		_initScaleTween.TweenProperty(_sprite, "scale", Vector2.One * num, 0.1).From(vector);
		OnInitScaleTween(_initScaleTween);
		if (moverStopTime != -1.0)
		{
			ScheduleMoveStop(leaseVersion, moverStopTime);
		}
	}

	public override void _Ready()
	{
		EnsurePoolLifecyclePolicy();
		_sprite = GetNodeOrNull<AdobeAnimateSprite>("%SunSprite");
		_moveComponent = GetNodeOrNull<MoveComponent>("%MoveComponent");
		if (GodotObject.IsInstanceValid(_moveComponent))
		{
			_moveComponent.MovementActivityChanged -= OnMovementActivityChanged;
			_moveComponent.MovementActivityChanged += OnMovementActivityChanged;
		}
		_light = GetNodeOrNull<PointLight2D>("%Light");
		_dieDownTimer = GetNodeOrNull<Timer>("%DieDownTimer");
		view = GetViewport();
		camera = view.GetCamera2D();
		viewSize = view.GetVisibleRect().Size;
		RestartSunAnimation();
		OnReady();
	}

	private void EnsurePoolLifecyclePolicy()
	{
		if (!_poolLifecyclePolicyInitialized)
		{
			_poolLifecyclePolicyInitialized = true;
			Type type = GetType();
			_supportsDirectPoolLifecycleDispatch = (type == typeof(TowerDefenseSunBase) || type == typeof(TowerDefenseSun) || type == typeof(TowerDefenseBrainSun) || type == typeof(TowerDefenseSunJalapeno) || type == typeof(TowerDefenseSunQX) || type == typeof(TowerDefenseSunMagic)) && ObjectPoolLifecyclePolicy.HasBuiltInCSharpScript(this);
		}
	}

	private void RestartSunAnimation()
	{
		if (GodotObject.IsInstanceValid(_sprite))
		{
			_sprite.Visible = true;
			_sprite.ProcessMode = ProcessModeEnum.Inherit;
			_sprite.pause = false;
			_sprite.SetAnimation("Idle");
			_sprite.UpdateChild();
			_sprite.RefreshManagedSlotSpriteCacheForRender();
			_sprite.RefreshProcessScheduling();
			_sprite.QueueRedraw();
		}
	}

	public override void _Input(InputEvent _event)
	{
		if (!isCollect && Geometry2D.IsPointInCircle(GetGlobalMousePosition(), _sprite.GlobalPosition, 40f * Scale.X))
		{
			Collection();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (autoCollect)
		{
			SetPhysicsProcess(enable: false);
		}
		else if (!over && (double)_sprite.Position.Y > height && _moveComponent.velocity.Y > 0f)
		{
			_sprite.Position = new Vector2(_sprite.Position.X, (float)height);
			over = true;
			_moveComponent.MoveClear();
			SetPhysicsProcess(enable: false);
		}
	}

	private void OnMovementActivityChanged(bool active)
	{
		if (active && !autoCollect && !isCollect && !die)
		{
			over = false;
			SetPhysicsProcess(enable: true);
		}
		else if (!active)
		{
			SetPhysicsProcess(enable: false);
		}
	}

	public void Collection()
	{
		if (!die && !isCollect)
		{
			OnCollectStart();
			int leaseVersion = _leaseVersion;
			if (GodotObject.IsInstanceValid(_light))
			{
				_light.Visible = false;
			}
			long collectValue = GetCollectValue();
			DropItemCollectionContext collectionContext = CreateCollectionContext(collectValue);
			DropItemConfig dropItemConfig = GetDropItemConfig();
			string stream = "Sun";
			if (dropItemConfig != null)
			{
				stream = dropItemConfig.PickAudio;
			}
			if (collectValue != 0L)
			{
				AudioManager.Instance.AudioPlay(stream);
			}
			_moveComponent.MoveClear();
			SetPhysicsProcess(enable: false);
			isCollect = true;
			Vector2 vector = (GodotObject.IsInstanceValid(camera) ? camera.GlobalPosition : Vector2.Zero);
			KillTween(ref _collectionTween);
			_collectionTween = CreateTween();
			_collectionTween.SetEase(Tween.EaseType.Out);
			_collectionTween.SetTrans(Tween.TransitionType.Cubic);
			_collectionTween.TweenProperty(_sprite, "global_position", vector + new Vector2(32f, 32f), 1.0);
			_collectionLeaseVersion = leaseVersion;
			_collectionContext = collectionContext;
			_collectionValue = collectValue;
			_collectionTweenCallback = Callable.From(FinishCollectionFlightCallback);
			_collectionTween.TweenCallback(_collectionTweenCallback);
		}
	}

	private void FinishCollectionFlight(int leaseVersion, DropItemCollectionContext collectionContext, long collectValue)
	{
		_collectionTween = null;
		if (IsCurrentLease(leaseVersion))
		{
			HandleCollectedValue(collectionContext);
			OnCollect?.Invoke(collectValue);
			if (IsCurrentLease(leaseVersion))
			{
				RemoveSunGroups();
				StartFadeAndDestroy(leaseVersion);
			}
		}
	}

	private void FinishCollectionFlightCallback()
	{
		FinishCollectionFlight(_collectionLeaseVersion, _collectionContext, _collectionValue);
	}

	private void StartFadeAndDestroy(int leaseVersion)
	{
		KillTween(ref _fadeTween);
		_fadeTween = CreateTween();
		_fadeTween.TweenProperty(_sprite, "modulate:a", 0.0, 0.25);
		_fadeLeaseVersion = leaseVersion;
		_fadeTweenCallback = Callable.From(FinishFadeAndDestroyCallback);
		_fadeTween.TweenCallback(_fadeTweenCallback);
	}

	private void FinishFadeAndDestroyCallback()
	{
		_fadeTween = null;
		if (IsCurrentLease(_fadeLeaseVersion))
		{
			Destroy();
		}
	}

	public virtual void DieDown()
	{
		if (!isCollect && !OnDieDown())
		{
			_moveComponent.MoveClear();
			SetPhysicsProcess(enable: false);
			RemoveSunGroups();
			die = true;
			int leaseVersion = _leaseVersion;
			KillTween(ref _fadeTween);
			_fadeTween = CreateTween();
			_fadeTween.SetEase(Tween.EaseType.Out);
			_fadeTween.SetTrans(Tween.TransitionType.Cubic);
			_fadeTween.TweenProperty(_sprite, "modulate:a", 0.0, 0.25);
			_fadeLeaseVersion = leaseVersion;
			_fadeTweenCallback = Callable.From(FinishFadeAndDestroyCallback);
			_fadeTween.TweenCallback(_fadeTweenCallback);
		}
	}

	public void Destroy()
	{
		RemoveSunGroups();
		ObjectManager.PoolPush(GetPoolKey(), this);
	}

	private void RemoveSunGroups()
	{
		if (_groupName != null)
		{
			RemoveFromGroup(_groupName);
		}
		RemoveFromGroup(SaveGroupNameKey);
	}

	private DropItemCollectionContext CreateCollectionContext(long value)
	{
		return new DropItemCollectionContext(this, GlobalPosition, value, AccountId, OwnershipPolicy);
	}

	private void HandleCollectedValue(DropItemCollectionContext context)
	{
		DropItemConfig dropItemConfig = GetDropItemConfig();
		if (dropItemConfig?.Handler != null)
		{
			dropItemConfig.Handler.OnCollect(context);
		}
		else if (context.UsesLegacyLocalEconomy)
		{
			TowerDefenseManager.Instance?.AddSun(context.Value);
		}
		else
		{
			SunDropItemHandler.ApplyAccountValue(context);
		}
	}

	private int BeginLease()
	{
		InvalidateLease();
		return _leaseVersion;
	}

	private void InvalidateLease()
	{
		_leaseVersion++;
		_refreshFrameLeaseVersion = -1;
		_moveStopLeaseVersion = -1;
		_autoCollectLeaseVersion = -1;
		_collectionLeaseVersion = -1;
		_fadeLeaseVersion = -1;
		_restoredLeaseVersion = -1;
		_autoCollectScheduledLease = -1;
		CancelDeferredCallbacks();
		KillTween(ref _initScaleTween);
		KillTween(ref _collectionTween);
		KillTween(ref _fadeTween);
	}

	private bool IsCurrentLease(int leaseVersion)
	{
		if (leaseVersion == _leaseVersion)
		{
			return IsInsideTree();
		}
		return false;
	}

	public override void _ExitTree()
	{
		InvalidateLease();
		view = null;
		camera = null;
		OnCollect = null;
	}

	private void ScheduleRefreshAfterPhysics(int leaseVersion)
	{
		CancelRefreshFrameCallback();
		_refreshFrameTree = GetTree();
		_refreshFrameLeaseVersion = leaseVersion;
		if (_refreshFrameCallback == null)
		{
			_refreshFrameCallback = OnRefreshPhysicsFrame;
		}
		_refreshFrameTree.PhysicsFrame += _refreshFrameCallback;
	}

	private void OnRefreshPhysicsFrame()
	{
		int refreshFrameLeaseVersion = _refreshFrameLeaseVersion;
		CancelRefreshFrameCallback();
		if (IsCurrentLease(refreshFrameLeaseVersion))
		{
			if (_restoredLeaseVersion != refreshFrameLeaseVersion)
			{
				autoCollect = GameSaveManager.Instance.GetFeatureValue("SunCollect") != 0;
			}
			if (ShouldAutoCollect())
			{
				SetPhysicsProcess(enable: false);
				ScheduleAutoCollect(refreshFrameLeaseVersion);
			}
		}
	}

	private void ScheduleMoveStop(int leaseVersion, double delay)
	{
		CancelMoveStopTimer();
		_moveStopLeaseVersion = leaseVersion;
		_moveStopTimer = GetTree().CreateTimer(delay, processAlways: false);
		if (_moveStopCallback == null)
		{
			_moveStopCallback = OnMoveStopTimeout;
		}
		_moveStopTimer.Timeout += _moveStopCallback;
	}

	private void OnMoveStopTimeout()
	{
		int moveStopLeaseVersion = _moveStopLeaseVersion;
		CancelMoveStopTimer();
		if (IsCurrentLease(moveStopLeaseVersion))
		{
			_moveComponent.MoveClear();
			SetPhysicsProcess(enable: false);
		}
	}

	private void ScheduleAutoCollect(int leaseVersion)
	{
		if (IsCurrentLease(leaseVersion) && _autoCollectScheduledLease != leaseVersion)
		{
			_autoCollectScheduledLease = leaseVersion;
			CancelAutoCollectTimer();
			_autoCollectLeaseVersion = leaseVersion;
			_autoCollectTimer = GetTree().CreateTimer(0.5, processAlways: false);
			if (_autoCollectCallback == null)
			{
				_autoCollectCallback = OnAutoCollectTimeout;
			}
			_autoCollectTimer.Timeout += _autoCollectCallback;
		}
	}

	private void OnAutoCollectTimeout()
	{
		int autoCollectLeaseVersion = _autoCollectLeaseVersion;
		CancelAutoCollectTimer();
		if (IsCurrentLease(autoCollectLeaseVersion))
		{
			Collection();
		}
	}

	private void CancelDeferredCallbacks()
	{
		CancelRefreshFrameCallback();
		CancelMoveStopTimer();
		CancelAutoCollectTimer();
	}

	private void CancelRefreshFrameCallback()
	{
		if (GodotObject.IsInstanceValid(_refreshFrameTree) && _refreshFrameCallback != null)
		{
			_refreshFrameTree.PhysicsFrame -= _refreshFrameCallback;
		}
		_refreshFrameLeaseVersion = -1;
		_refreshFrameTree = null;
	}

	private void CancelMoveStopTimer()
	{
		if (GodotObject.IsInstanceValid(_moveStopTimer) && _moveStopCallback != null)
		{
			_moveStopTimer.Timeout -= _moveStopCallback;
		}
		_moveStopLeaseVersion = -1;
		_moveStopTimer = null;
	}

	private void CancelAutoCollectTimer()
	{
		if (GodotObject.IsInstanceValid(_autoCollectTimer) && _autoCollectCallback != null)
		{
			_autoCollectTimer.Timeout -= _autoCollectCallback;
		}
		_autoCollectLeaseVersion = -1;
		_autoCollectTimer = null;
	}

	private static void KillTween(ref Tween tween)
	{
		if (GodotObject.IsInstanceValid(tween))
		{
			tween.Kill();
		}
		tween = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(41)
		{
			new MethodInfo(MethodName.GetGroupName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPoolKey, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetDropItemConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCollectStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCollectValue, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDieDown, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldAutoCollect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInitScaleTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tween", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_sunNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_movingMethod", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "moverStopTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreAutoCollect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "savedAutoCollect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePoolLifecyclePolicy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestartSunAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMovementActivityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Collection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishCollectionFlightCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartFadeAndDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishFadeAndDestroyCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSunGroups, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginLease, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateLease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCurrentLease, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleRefreshAfterPhysics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRefreshPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleMoveStop, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMoveStopTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleAutoCollect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAutoCollectTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelDeferredCallbacks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelRefreshFrameCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelMoveStopTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelAutoCollectTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetGroupName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetGroupName());
			return true;
		}
		if (method == MethodName.GetPoolKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(GetPoolKey());
			return true;
		}
		if (method == MethodName.GetDropItemConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(GetDropItemConfig());
			return true;
		}
		if (method == MethodName.OnCollectStart && args.Count == 0)
		{
			OnCollectStart();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCollectValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetCollectValue());
			return true;
		}
		if (method == MethodName.OnDieDown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OnDieDown());
			return true;
		}
		if (method == MethodName.ShouldAutoCollect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAutoCollect());
			return true;
		}
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRefresh && args.Count == 0)
		{
			OnRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.OnInitScaleTween && args.Count == 1)
		{
			OnInitScaleTween(VariantUtils.ConvertTo<Tween>(in args[0]));
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
		if (method == MethodName.Init && args.Count == 6)
		{
			Init(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreAutoCollect && args.Count == 1)
		{
			RestoreAutoCollect(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePoolLifecyclePolicy && args.Count == 0)
		{
			EnsurePoolLifecyclePolicy();
			ret = default;
			return true;
		}
		if (method == MethodName.RestartSunAnimation && args.Count == 0)
		{
			RestartSunAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMovementActivityChanged && args.Count == 1)
		{
			OnMovementActivityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Collection && args.Count == 0)
		{
			Collection();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishCollectionFlightCallback && args.Count == 0)
		{
			FinishCollectionFlightCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.StartFadeAndDestroy && args.Count == 1)
		{
			StartFadeAndDestroy(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishFadeAndDestroyCallback && args.Count == 0)
		{
			FinishFadeAndDestroyCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.DieDown && args.Count == 0)
		{
			DieDown();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSunGroups && args.Count == 0)
		{
			RemoveSunGroups();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginLease && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BeginLease());
			return true;
		}
		if (method == MethodName.InvalidateLease && args.Count == 0)
		{
			InvalidateLease();
			ret = default;
			return true;
		}
		if (method == MethodName.IsCurrentLease && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentLease(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleRefreshAfterPhysics && args.Count == 1)
		{
			ScheduleRefreshAfterPhysics(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRefreshPhysicsFrame && args.Count == 0)
		{
			OnRefreshPhysicsFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleMoveStop && args.Count == 2)
		{
			ScheduleMoveStop(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMoveStopTimeout && args.Count == 0)
		{
			OnMoveStopTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleAutoCollect && args.Count == 1)
		{
			ScheduleAutoCollect(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAutoCollectTimeout && args.Count == 0)
		{
			OnAutoCollectTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelDeferredCallbacks && args.Count == 0)
		{
			CancelDeferredCallbacks();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelRefreshFrameCallback && args.Count == 0)
		{
			CancelRefreshFrameCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelMoveStopTimer && args.Count == 0)
		{
			CancelMoveStopTimer();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelAutoCollectTimer && args.Count == 0)
		{
			CancelAutoCollectTimer();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetGroupName)
		{
			return true;
		}
		if (method == MethodName.GetPoolKey)
		{
			return true;
		}
		if (method == MethodName.GetDropItemConfig)
		{
			return true;
		}
		if (method == MethodName.OnCollectStart)
		{
			return true;
		}
		if (method == MethodName.GetCollectValue)
		{
			return true;
		}
		if (method == MethodName.OnDieDown)
		{
			return true;
		}
		if (method == MethodName.ShouldAutoCollect)
		{
			return true;
		}
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.OnRefresh)
		{
			return true;
		}
		if (method == MethodName.OnInitScaleTween)
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
		if (method == MethodName.RestoreAutoCollect)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.EnsurePoolLifecyclePolicy)
		{
			return true;
		}
		if (method == MethodName.RestartSunAnimation)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.OnMovementActivityChanged)
		{
			return true;
		}
		if (method == MethodName.Collection)
		{
			return true;
		}
		if (method == MethodName.FinishCollectionFlightCallback)
		{
			return true;
		}
		if (method == MethodName.StartFadeAndDestroy)
		{
			return true;
		}
		if (method == MethodName.FinishFadeAndDestroyCallback)
		{
			return true;
		}
		if (method == MethodName.DieDown)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.RemoveSunGroups)
		{
			return true;
		}
		if (method == MethodName.BeginLease)
		{
			return true;
		}
		if (method == MethodName.InvalidateLease)
		{
			return true;
		}
		if (method == MethodName.IsCurrentLease)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ScheduleRefreshAfterPhysics)
		{
			return true;
		}
		if (method == MethodName.OnRefreshPhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.ScheduleMoveStop)
		{
			return true;
		}
		if (method == MethodName.OnMoveStopTimeout)
		{
			return true;
		}
		if (method == MethodName.ScheduleAutoCollect)
		{
			return true;
		}
		if (method == MethodName.OnAutoCollectTimeout)
		{
			return true;
		}
		if (method == MethodName.CancelDeferredCallbacks)
		{
			return true;
		}
		if (method == MethodName.CancelRefreshFrameCallback)
		{
			return true;
		}
		if (method == MethodName.CancelMoveStopTimer)
		{
			return true;
		}
		if (method == MethodName.CancelAutoCollectTimer)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.OwnershipPolicy)
		{
			OwnershipPolicy = VariantUtils.ConvertTo<SunDropOwnershipPolicy>(in value);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			_sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._moveComponent)
		{
			_moveComponent = VariantUtils.ConvertTo<MoveComponent>(in value);
			return true;
		}
		if (name == PropertyName._light)
		{
			_light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		if (name == PropertyName._dieDownTimer)
		{
			_dieDownTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._initScaleTween)
		{
			_initScaleTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._collectionTween)
		{
			_collectionTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._fadeTween)
		{
			_fadeTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._refreshFrameTree)
		{
			_refreshFrameTree = VariantUtils.ConvertTo<SceneTree>(in value);
			return true;
		}
		if (name == PropertyName._moveStopTimer)
		{
			_moveStopTimer = VariantUtils.ConvertTo<SceneTreeTimer>(in value);
			return true;
		}
		if (name == PropertyName._autoCollectTimer)
		{
			_autoCollectTimer = VariantUtils.ConvertTo<SceneTreeTimer>(in value);
			return true;
		}
		if (name == PropertyName._groupName)
		{
			_groupName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._refreshFrameLeaseVersion)
		{
			_refreshFrameLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._moveStopLeaseVersion)
		{
			_moveStopLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._autoCollectLeaseVersion)
		{
			_autoCollectLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._collectionLeaseVersion)
		{
			_collectionLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._fadeLeaseVersion)
		{
			_fadeLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._collectionValue)
		{
			_collectionValue = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._collectionTweenCallback)
		{
			_collectionTweenCallback = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._fadeTweenCallback)
		{
			_fadeTweenCallback = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._leaseVersion)
		{
			_leaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._restoredLeaseVersion)
		{
			_restoredLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._autoCollectScheduledLease)
		{
			_autoCollectScheduledLease = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._supportsDirectPoolLifecycleDispatch)
		{
			_supportsDirectPoolLifecycleDispatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._poolLifecyclePolicyInitialized)
		{
			_poolLifecyclePolicyInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isCollect)
		{
			isCollect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.die)
		{
			die = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			movingMethod = VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.view)
		{
			view = VariantUtils.ConvertTo<Viewport>(in value);
			return true;
		}
		if (name == PropertyName.camera)
		{
			camera = VariantUtils.ConvertTo<Camera2D>(in value);
			return true;
		}
		if (name == PropertyName.viewSize)
		{
			viewSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.autoCollect)
		{
			autoCollect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSprite>(sprite);
			return true;
		}
		if (name == PropertyName.moveComponent)
		{
			value = VariantUtils.CreateFrom<MoveComponent>(moveComponent);
			return true;
		}
		if (name == PropertyName.light)
		{
			value = VariantUtils.CreateFrom<PointLight2D>(light);
			return true;
		}
		if (name == PropertyName.dieDownTimer)
		{
			value = VariantUtils.CreateFrom<Timer>(dieDownTimer);
			return true;
		}
		if (name == PropertyName.OwnershipPolicy)
		{
			value = VariantUtils.CreateFrom<SunDropOwnershipPolicy>(OwnershipPolicy);
			return true;
		}
		if (name == PropertyName.UsesLegacyLocalEconomy)
		{
			value = VariantUtils.CreateFrom<bool>(UsesLegacyLocalEconomy);
			return true;
		}
		if (name == PropertyName._sprite)
		{
			value = VariantUtils.CreateFrom(in _sprite);
			return true;
		}
		if (name == PropertyName._moveComponent)
		{
			value = VariantUtils.CreateFrom(in _moveComponent);
			return true;
		}
		if (name == PropertyName._light)
		{
			value = VariantUtils.CreateFrom(in _light);
			return true;
		}
		if (name == PropertyName._dieDownTimer)
		{
			value = VariantUtils.CreateFrom(in _dieDownTimer);
			return true;
		}
		if (name == PropertyName._initScaleTween)
		{
			value = VariantUtils.CreateFrom(in _initScaleTween);
			return true;
		}
		if (name == PropertyName._collectionTween)
		{
			value = VariantUtils.CreateFrom(in _collectionTween);
			return true;
		}
		if (name == PropertyName._fadeTween)
		{
			value = VariantUtils.CreateFrom(in _fadeTween);
			return true;
		}
		if (name == PropertyName._refreshFrameTree)
		{
			value = VariantUtils.CreateFrom(in _refreshFrameTree);
			return true;
		}
		if (name == PropertyName._moveStopTimer)
		{
			value = VariantUtils.CreateFrom(in _moveStopTimer);
			return true;
		}
		if (name == PropertyName._autoCollectTimer)
		{
			value = VariantUtils.CreateFrom(in _autoCollectTimer);
			return true;
		}
		if (name == PropertyName._groupName)
		{
			value = VariantUtils.CreateFrom(in _groupName);
			return true;
		}
		if (name == PropertyName._refreshFrameLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _refreshFrameLeaseVersion);
			return true;
		}
		if (name == PropertyName._moveStopLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _moveStopLeaseVersion);
			return true;
		}
		if (name == PropertyName._autoCollectLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _autoCollectLeaseVersion);
			return true;
		}
		if (name == PropertyName._collectionLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _collectionLeaseVersion);
			return true;
		}
		if (name == PropertyName._fadeLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _fadeLeaseVersion);
			return true;
		}
		if (name == PropertyName._collectionValue)
		{
			value = VariantUtils.CreateFrom(in _collectionValue);
			return true;
		}
		if (name == PropertyName._collectionTweenCallback)
		{
			value = VariantUtils.CreateFrom(in _collectionTweenCallback);
			return true;
		}
		if (name == PropertyName._fadeTweenCallback)
		{
			value = VariantUtils.CreateFrom(in _fadeTweenCallback);
			return true;
		}
		if (name == PropertyName._leaseVersion)
		{
			value = VariantUtils.CreateFrom(in _leaseVersion);
			return true;
		}
		if (name == PropertyName._restoredLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _restoredLeaseVersion);
			return true;
		}
		if (name == PropertyName._autoCollectScheduledLease)
		{
			value = VariantUtils.CreateFrom(in _autoCollectScheduledLease);
			return true;
		}
		if (name == PropertyName._supportsDirectPoolLifecycleDispatch)
		{
			value = VariantUtils.CreateFrom(in _supportsDirectPoolLifecycleDispatch);
			return true;
		}
		if (name == PropertyName._poolLifecyclePolicyInitialized)
		{
			value = VariantUtils.CreateFrom(in _poolLifecyclePolicyInitialized);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom(in sunNum);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.isCollect)
		{
			value = VariantUtils.CreateFrom(in isCollect);
			return true;
		}
		if (name == PropertyName.die)
		{
			value = VariantUtils.CreateFrom(in die);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			value = VariantUtils.CreateFrom(in movingMethod);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.view)
		{
			value = VariantUtils.CreateFrom(in view);
			return true;
		}
		if (name == PropertyName.camera)
		{
			value = VariantUtils.CreateFrom(in camera);
			return true;
		}
		if (name == PropertyName.viewSize)
		{
			value = VariantUtils.CreateFrom(in viewSize);
			return true;
		}
		if (name == PropertyName.autoCollect)
		{
			value = VariantUtils.CreateFrom(in autoCollect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dieDownTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initScaleTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectionTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fadeTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._refreshFrameTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveStopTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoCollectTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._groupName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._refreshFrameLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._moveStopLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._autoCollectLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._collectionLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._fadeLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._collectionValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._collectionTweenCallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._fadeTweenCallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._leaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._restoredLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._autoCollectScheduledLease, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._supportsDirectPoolLifecycleDispatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._poolLifecyclePolicyInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moveComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dieDownTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.OwnershipPolicy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesLegacyLocalEconomy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCollect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.die, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.movingMethod, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.view, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.camera, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.viewSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoCollect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.OwnershipPolicy, Variant.From<SunDropOwnershipPolicy>(OwnershipPolicy));
		info.AddProperty(PropertyName._sprite, Variant.From(in _sprite));
		info.AddProperty(PropertyName._moveComponent, Variant.From(in _moveComponent));
		info.AddProperty(PropertyName._light, Variant.From(in _light));
		info.AddProperty(PropertyName._dieDownTimer, Variant.From(in _dieDownTimer));
		info.AddProperty(PropertyName._initScaleTween, Variant.From(in _initScaleTween));
		info.AddProperty(PropertyName._collectionTween, Variant.From(in _collectionTween));
		info.AddProperty(PropertyName._fadeTween, Variant.From(in _fadeTween));
		info.AddProperty(PropertyName._refreshFrameTree, Variant.From(in _refreshFrameTree));
		info.AddProperty(PropertyName._moveStopTimer, Variant.From(in _moveStopTimer));
		info.AddProperty(PropertyName._autoCollectTimer, Variant.From(in _autoCollectTimer));
		info.AddProperty(PropertyName._groupName, Variant.From(in _groupName));
		info.AddProperty(PropertyName._refreshFrameLeaseVersion, Variant.From(in _refreshFrameLeaseVersion));
		info.AddProperty(PropertyName._moveStopLeaseVersion, Variant.From(in _moveStopLeaseVersion));
		info.AddProperty(PropertyName._autoCollectLeaseVersion, Variant.From(in _autoCollectLeaseVersion));
		info.AddProperty(PropertyName._collectionLeaseVersion, Variant.From(in _collectionLeaseVersion));
		info.AddProperty(PropertyName._fadeLeaseVersion, Variant.From(in _fadeLeaseVersion));
		info.AddProperty(PropertyName._collectionValue, Variant.From(in _collectionValue));
		info.AddProperty(PropertyName._collectionTweenCallback, Variant.From(in _collectionTweenCallback));
		info.AddProperty(PropertyName._fadeTweenCallback, Variant.From(in _fadeTweenCallback));
		info.AddProperty(PropertyName._leaseVersion, Variant.From(in _leaseVersion));
		info.AddProperty(PropertyName._restoredLeaseVersion, Variant.From(in _restoredLeaseVersion));
		info.AddProperty(PropertyName._autoCollectScheduledLease, Variant.From(in _autoCollectScheduledLease));
		info.AddProperty(PropertyName._supportsDirectPoolLifecycleDispatch, Variant.From(in _supportsDirectPoolLifecycleDispatch));
		info.AddProperty(PropertyName._poolLifecyclePolicyInitialized, Variant.From(in _poolLifecyclePolicyInitialized));
		info.AddProperty(PropertyName.sunNum, Variant.From(in sunNum));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.isCollect, Variant.From(in isCollect));
		info.AddProperty(PropertyName.die, Variant.From(in die));
		info.AddProperty(PropertyName.movingMethod, Variant.From(in movingMethod));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.view, Variant.From(in view));
		info.AddProperty(PropertyName.camera, Variant.From(in camera));
		info.AddProperty(PropertyName.viewSize, Variant.From(in viewSize));
		info.AddProperty(PropertyName.autoCollect, Variant.From(in autoCollect));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.OwnershipPolicy, out var value))
		{
			OwnershipPolicy = value.As<SunDropOwnershipPolicy>();
		}
		if (info.TryGetProperty(PropertyName._sprite, out var value2))
		{
			_sprite = value2.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._moveComponent, out var value3))
		{
			_moveComponent = value3.As<MoveComponent>();
		}
		if (info.TryGetProperty(PropertyName._light, out var value4))
		{
			_light = value4.As<PointLight2D>();
		}
		if (info.TryGetProperty(PropertyName._dieDownTimer, out var value5))
		{
			_dieDownTimer = value5.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._initScaleTween, out var value6))
		{
			_initScaleTween = value6.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._collectionTween, out var value7))
		{
			_collectionTween = value7.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._fadeTween, out var value8))
		{
			_fadeTween = value8.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._refreshFrameTree, out var value9))
		{
			_refreshFrameTree = value9.As<SceneTree>();
		}
		if (info.TryGetProperty(PropertyName._moveStopTimer, out var value10))
		{
			_moveStopTimer = value10.As<SceneTreeTimer>();
		}
		if (info.TryGetProperty(PropertyName._autoCollectTimer, out var value11))
		{
			_autoCollectTimer = value11.As<SceneTreeTimer>();
		}
		if (info.TryGetProperty(PropertyName._groupName, out var value12))
		{
			_groupName = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._refreshFrameLeaseVersion, out var value13))
		{
			_refreshFrameLeaseVersion = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._moveStopLeaseVersion, out var value14))
		{
			_moveStopLeaseVersion = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._autoCollectLeaseVersion, out var value15))
		{
			_autoCollectLeaseVersion = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._collectionLeaseVersion, out var value16))
		{
			_collectionLeaseVersion = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._fadeLeaseVersion, out var value17))
		{
			_fadeLeaseVersion = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._collectionValue, out var value18))
		{
			_collectionValue = value18.As<long>();
		}
		if (info.TryGetProperty(PropertyName._collectionTweenCallback, out var value19))
		{
			_collectionTweenCallback = value19.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._fadeTweenCallback, out var value20))
		{
			_fadeTweenCallback = value20.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._leaseVersion, out var value21))
		{
			_leaseVersion = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._restoredLeaseVersion, out var value22))
		{
			_restoredLeaseVersion = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._autoCollectScheduledLease, out var value23))
		{
			_autoCollectScheduledLease = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._supportsDirectPoolLifecycleDispatch, out var value24))
		{
			_supportsDirectPoolLifecycleDispatch = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._poolLifecyclePolicyInitialized, out var value25))
		{
			_poolLifecyclePolicyInitialized = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunNum, out var value26))
		{
			sunNum = value26.As<long>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value27))
		{
			height = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isCollect, out var value28))
		{
			isCollect = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.die, out var value29))
		{
			die = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.movingMethod, out var value30))
		{
			movingMethod = value30.As<TowerDefenseEnum.SUN_MOVING_METHOD>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value31))
		{
			over = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value32))
		{
			gridPos = value32.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.view, out var value33))
		{
			view = value33.As<Viewport>();
		}
		if (info.TryGetProperty(PropertyName.camera, out var value34))
		{
			camera = value34.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName.viewSize, out var value35))
		{
			viewSize = value35.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.autoCollect, out var value36))
		{
			autoCollect = value36.As<bool>();
		}
	}
}
