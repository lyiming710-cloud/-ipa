using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Coin/Base/TowerDefenseCoinBase.cs")]
public class TowerDefenseCoinBase : TowerDefenseGroundItemBase, IObjectPoolLifecycle
{
	public delegate void CollectEventHandler(int num);

	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public static readonly StringName _GetDropItemConfig = "_GetDropItemConfig";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName RestartCoinAnimations = "RestartCoinAnimations";

		public static readonly StringName Recycle = "Recycle";

		public static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName EnsurePoolLifecyclePolicy = "EnsurePoolLifecyclePolicy";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName ClampToViewportIfNeeded = "ClampToViewportIfNeeded";

		public static readonly StringName OnMovementActivityChanged = "OnMovementActivityChanged";

		public new static readonly StringName Collection = "Collection";

		public static readonly StringName GetCollectionTargetPosition = "GetCollectionTargetPosition";

		public static readonly StringName DieDown = "DieDown";

		public static readonly StringName FinishCollectionFlight = "FinishCollectionFlight";

		public static readonly StringName FinishCollectionFlightCallback = "FinishCollectionFlightCallback";

		public static readonly StringName FinishFadeAndDestroy = "FinishFadeAndDestroy";

		public static readonly StringName FinishFadeAndDestroyCallback = "FinishFadeAndDestroyCallback";

		public static readonly StringName HandleCollectedValue = "HandleCollectedValue";

		public static readonly StringName BeginLease = "BeginLease";

		public static readonly StringName InvalidateLease = "InvalidateLease";

		public static readonly StringName IsCurrentLease = "IsCurrentLease";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ScheduleInitAfterPhysics = "ScheduleInitAfterPhysics";

		public static readonly StringName OnInitPhysicsFrame = "OnInitPhysicsFrame";

		public static readonly StringName ScheduleAutoCollect = "ScheduleAutoCollect";

		public static readonly StringName OnAutoCollectTimeout = "OnAutoCollectTimeout";

		public static readonly StringName CancelDeferredCallbacks = "CancelDeferredCallbacks";

		public static readonly StringName CancelInitFrameCallback = "CancelInitFrameCallback";

		public static readonly StringName CancelAutoCollectTimer = "CancelAutoCollectTimer";

		public static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName coinObjectId = "coinObjectId";

		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName moveComponent = "moveComponent";

		public static readonly StringName fallAudio = "fallAudio";

		public static readonly StringName pickAudio = "pickAudio";

		public static readonly StringName num = "num";

		public static readonly StringName canMagnet = "canMagnet";

		public static readonly StringName height = "height";

		public static readonly StringName isCollect = "isCollect";

		public static readonly StringName die = "die";

		public static readonly StringName disabledInput = "disabledInput";

		public static readonly StringName over = "over";

		public static readonly StringName view = "view";

		public static readonly StringName camera = "camera";

		public static readonly StringName viewSize = "viewSize";

		public static readonly StringName autoCollect = "autoCollect";

		public static readonly StringName randFreshIndex = "randFreshIndex";

		public static readonly StringName _viewportCheckCountdown = "_viewportCheckCountdown";

		public static readonly StringName _dieDownTimer = "_dieDownTimer";

		public static readonly StringName _collectionTween = "_collectionTween";

		public static readonly StringName _fadeTween = "_fadeTween";

		public static readonly StringName _initFrameTree = "_initFrameTree";

		public static readonly StringName _autoCollectTimer = "_autoCollectTimer";

		public static readonly StringName _initFrameLeaseVersion = "_initFrameLeaseVersion";

		public static readonly StringName _autoCollectLeaseVersion = "_autoCollectLeaseVersion";

		public static readonly StringName _collectionLeaseVersion = "_collectionLeaseVersion";

		public static readonly StringName _fadeLeaseVersion = "_fadeLeaseVersion";

		public static readonly StringName _collectionTweenCallback = "_collectionTweenCallback";

		public static readonly StringName _fadeTweenCallback = "_fadeTweenCallback";

		public static readonly StringName _leaseVersion = "_leaseVersion";

		public static readonly StringName _supportsDirectPoolLifecycleDispatch = "_supportsDirectPoolLifecycleDispatch";

		public static readonly StringName _poolLifecyclePolicyInitialized = "_poolLifecyclePolicyInitialized";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private static readonly StringName CoinGroupName = new StringName("Coin");

	public Node2D spriteNode;

	public MoveComponent moveComponent;

	[Export(PropertyHint.None, "")]
	public string fallAudio = "CoinFall";

	[Export(PropertyHint.None, "")]
	public string pickAudio = "CoinPick";

	[Export(PropertyHint.None, "")]
	public int num = 10;

	public bool canMagnet = true;

	public double height = 500.0;

	public bool isCollect;

	public bool die;

	public bool disabledInput;

	public bool over;

	public Viewport view;

	public Camera2D camera;

	public Vector2 viewSize;

	public bool autoCollect;

	public int randFreshIndex;

	private int _viewportCheckCountdown = 1;

	private Timer _dieDownTimer;

	private Tween _collectionTween;

	private Tween _fadeTween;

	private SceneTree _initFrameTree;

	private Action _initFrameCallback;

	private SceneTreeTimer _autoCollectTimer;

	private Action _autoCollectCallback;

	private int _initFrameLeaseVersion = -1;

	private int _autoCollectLeaseVersion = -1;

	private int _collectionLeaseVersion = -1;

	private int _fadeLeaseVersion = -1;

	private Callable _collectionTweenCallback;

	private Callable _fadeTweenCallback;

	private int _leaseVersion;

	private bool _supportsDirectPoolLifecycleDispatch;

	private bool _poolLifecyclePolicyInitialized;

	[Export(PropertyHint.None, "")]
	public int coinObjectId { get; set; }

	bool IObjectPoolLifecycle.SupportsDirectPoolLifecycleDispatch
	{
		get
		{
			EnsurePoolLifecyclePolicy();
			return _supportsDirectPoolLifecycleDispatch;
		}
	}

	public event CollectEventHandler OnCollect;

	public DropItemConfig _GetDropItemConfig()
	{
		return DropItemRegistry.GetByCoinObjectId(coinObjectId);
	}

	public void Refresh()
	{
		BeginLease();
		SetPhysicsProcess(enable: true);
		AddToGroup(CoinGroupName, persistent: true);
		spriteNode.Position = Vector2.Zero;
		Color modulate = spriteNode.Modulate;
		modulate.A = 1f;
		spriteNode.Modulate = modulate;
		height = 0.0;
		isCollect = false;
		die = false;
		disabledInput = false;
		over = false;
		RestartCoinAnimations(spriteNode);
		_viewportCheckCountdown = 1 + (randFreshIndex & 0x7FFFFFFF) % 30;
		DropItemConfig dropItemConfig = _GetDropItemConfig();
		if (dropItemConfig != null)
		{
			fallAudio = dropItemConfig.FallAudio;
			pickAudio = dropItemConfig.PickAudio;
		}
		view = GetViewport();
		camera = view.GetCamera2D();
		viewSize = view.GetVisibleRect().Size;
		if (GodotObject.IsInstanceValid(_dieDownTimer))
		{
			_dieDownTimer.Start();
		}
		AudioManager.Instance.AudioPlay(fallAudio);
	}

	private void RestartCoinAnimations(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		foreach (Node child in node.GetChildren())
		{
			if (child is AdobeAnimateSprite adobeAnimateSprite && !string.IsNullOrEmpty(adobeAnimateSprite.clip))
			{
				adobeAnimateSprite.SetAnimation(adobeAnimateSprite.clip, adobeAnimateSprite.loop);
			}
			if (child != null)
			{
				Node node2 = child;
				RestartCoinAnimations(node2);
			}
		}
	}

	public void Recycle()
	{
		InvalidateLease();
		RemoveFromGroup(CoinGroupName);
		if (GodotObject.IsInstanceValid(_dieDownTimer))
		{
			_dieDownTimer.Stop();
		}
		moveComponent?.MoveClear();
		SetPhysicsProcess(enable: false);
		autoCollect = false;
		OnCollect = null;
		view = null;
		camera = null;
	}

	void IObjectPoolLifecycle.RefreshFromPool()
	{
		Refresh();
	}

	void IObjectPoolLifecycle.RecycleToPool()
	{
		Recycle();
	}

	public void Init(double _height = 0.0, Vector2 _velocity = default(Vector2), double _gravity = 0.0)
	{
		int leaseVersion = _leaseVersion;
		height = _height;
		moveComponent.SetVelocity(_velocity);
		moveComponent.SetGravity(_gravity);
		SetPhysicsProcess(moveComponent.HasActiveMovement);
		if (!moveComponent.HasActiveMovement)
		{
			ClampToViewportIfNeeded();
		}
		ScheduleInitAfterPhysics(leaseVersion);
	}

	public override void _Ready()
	{
		EnsurePoolLifecyclePolicy();
		if (!Engine.IsEditorHint())
		{
			spriteNode = GetNode<Node2D>("%SpriteNode");
			moveComponent = GetNode<MoveComponent>("%MoveComponent");
			moveComponent.MovementActivityChanged -= OnMovementActivityChanged;
			moveComponent.MovementActivityChanged += OnMovementActivityChanged;
			_dieDownTimer = GetNode<Timer>("DieDownTimer");
			base._Ready();
			randFreshIndex = (int)GD.Randi();
			view = GetViewport();
			camera = view.GetCamera2D();
			viewSize = view.GetVisibleRect().Size;
			_dieDownTimer.Timeout += DieDown;
		}
	}

	private void EnsurePoolLifecyclePolicy()
	{
		if (!_poolLifecyclePolicyInitialized)
		{
			_poolLifecyclePolicyInitialized = true;
			Type type = GetType();
			_supportsDirectPoolLifecycleDispatch = (type == typeof(TowerDefenseCoinBase) || type == typeof(TowerDefenseLuckyBag) || type == typeof(TowerDefenseGoldShard)) && ObjectPoolLifecyclePolicy.HasBuiltInCSharpScript(this);
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (Geometry2D.IsPointInCircle(GetGlobalMousePosition(), spriteNode.GlobalPosition, 30f * Scale.X))
		{
			Collection();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (autoCollect)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		_viewportCheckCountdown--;
		if (_viewportCheckCountdown <= 0)
		{
			_viewportCheckCountdown = 30;
			ClampToViewportIfNeeded();
		}
		if (!over && (double)spriteNode.Position.Y > height)
		{
			spriteNode.Position = new Vector2(spriteNode.Position.X, (float)height);
			over = true;
			moveComponent.MoveClear();
			ClampToViewportIfNeeded();
			SetPhysicsProcess(enable: false);
		}
	}

	private void ClampToViewportIfNeeded()
	{
		if (GodotObject.IsInstanceValid(camera) && GodotObject.IsInstanceValid(spriteNode) && !camera.GetViewportRect().HasPoint(spriteNode.GlobalPosition))
		{
			Vector2 globalPosition = camera.GlobalPosition;
			spriteNode.GlobalPosition = new Vector2(Mathf.Clamp(spriteNode.GlobalPosition.X, globalPosition.X + 40f, globalPosition.X + viewSize.X - 40f), Mathf.Clamp(spriteNode.GlobalPosition.Y, globalPosition.Y + 40f, globalPosition.Y + viewSize.Y - 40f));
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
			ClampToViewportIfNeeded();
			SetPhysicsProcess(enable: false);
		}
	}

	public override void Collection()
	{
		if (die || isCollect)
		{
			return;
		}
		GlobalFeatureManager instance = GlobalFeatureManager.Instance;
		if (instance != null && !instance.IsUnlocked("Shop"))
		{
			BroadCastConfig broadCastConfig = new BroadCastConfig();
			broadCastConfig.broadCastString = "SHOP_OPEN";
			broadCastConfig.broadCastTime = 7.5;
			BroadCastManager.Instance.BroadCastAdd(broadCastConfig);
			GlobalFeatureManager.Instance.Unlock("Shop");
		}
		AudioManager.Instance.AudioPlay(pickAudio);
		int leaseVersion = _leaseVersion;
		moveComponent.MoveClear();
		SetPhysicsProcess(enable: false);
		isCollect = true;
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.coinBank))
		{
			TowerDefenseManager.Instance.coinBank.ShowCoinBank();
		}
		RemoveFromGroup(CoinGroupName);
		KillTween(ref _collectionTween);
		_collectionTween = CreateTween();
		_collectionTween.SetEase(Tween.EaseType.Out);
		_collectionTween.SetTrans(Tween.TransitionType.Cubic);
		Vector2 flightStart = spriteNode.GlobalPosition;
		_collectionTween.TweenMethod(Callable.From((float progress) =>
		{
			if (IsCurrentLease(leaseVersion))
			{
				spriteNode.GlobalPosition = flightStart.Lerp(GetCollectionTargetPosition(), progress);
			}
		}), 0f, 1f, 1.0);
		_collectionLeaseVersion = leaseVersion;
		_collectionTweenCallback = Callable.From(FinishCollectionFlightCallback);
		_collectionTween.TweenCallback(_collectionTweenCallback);
	}

	internal Vector2 GetCollectionTargetPosition()
	{
		CoinBank coinBank = TowerDefenseManager.Instance?.coinBank;
		if (!GodotObject.IsInstanceValid(coinBank))
		{
			return spriteNode.GlobalPosition;
		}
		Vector2 vector = coinBank.GetScreenTransform() * new Vector2(34f, 23f);
		Vector2 localPoint = spriteNode.GetScreenTransform().AffineInverse() * vector;
		return spriteNode.ToGlobal(localPoint);
	}

	public void DieDown()
	{
		if (!isCollect)
		{
			moveComponent.MoveClear();
			SetPhysicsProcess(enable: false);
			RemoveFromGroup(CoinGroupName);
			die = true;
			int leaseVersion = _leaseVersion;
			KillTween(ref _fadeTween);
			_fadeTween = CreateTween();
			_fadeTween.SetEase(Tween.EaseType.Out);
			_fadeTween.SetTrans(Tween.TransitionType.Cubic);
			_fadeTween.TweenProperty(spriteNode, "modulate:a", 0.0, 0.25);
			_fadeLeaseVersion = leaseVersion;
			_fadeTweenCallback = Callable.From(FinishFadeAndDestroyCallback);
			_fadeTween.TweenCallback(_fadeTweenCallback);
		}
	}

	private void FinishCollectionFlight(int leaseVersion)
	{
		_collectionTween = null;
		if (IsCurrentLease(leaseVersion))
		{
			HandleCollectedValue(num);
			OnCollect?.Invoke(num);
			if (IsCurrentLease(leaseVersion))
			{
				KillTween(ref _fadeTween);
				_fadeTween = CreateTween();
				_fadeTween.TweenProperty(spriteNode, "modulate:a", 0.0, 0.25);
				_fadeLeaseVersion = leaseVersion;
				_fadeTweenCallback = Callable.From(FinishFadeAndDestroyCallback);
				_fadeTween.TweenCallback(_fadeTweenCallback);
			}
		}
	}

	private void FinishCollectionFlightCallback()
	{
		FinishCollectionFlight(_collectionLeaseVersion);
	}

	private void FinishFadeAndDestroy(int leaseVersion)
	{
		_fadeTween = null;
		if (IsCurrentLease(leaseVersion))
		{
			Destroy();
		}
	}

	private void FinishFadeAndDestroyCallback()
	{
		FinishFadeAndDestroy(_fadeLeaseVersion);
	}

	private void HandleCollectedValue(int value)
	{
		DropItemConfig dropItemConfig = _GetDropItemConfig();
		if (dropItemConfig?.Handler != null)
		{
			dropItemConfig.Handler.OnCollect(GlobalPosition, value);
		}
		else
		{
			TowerDefenseManager.Instance?.AddCoin(value);
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
		_initFrameLeaseVersion = -1;
		_autoCollectLeaseVersion = -1;
		_collectionLeaseVersion = -1;
		_fadeLeaseVersion = -1;
		CancelDeferredCallbacks();
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
		if (GodotObject.IsInstanceValid(_dieDownTimer))
		{
			_dieDownTimer.Stop();
		}
		OnCollect = null;
		view = null;
		camera = null;
		base._ExitTree();
	}

	private void ScheduleInitAfterPhysics(int leaseVersion)
	{
		CancelInitFrameCallback();
		_initFrameTree = GetTree();
		_initFrameLeaseVersion = leaseVersion;
		if (_initFrameCallback == null)
		{
			_initFrameCallback = OnInitPhysicsFrame;
		}
		_initFrameTree.PhysicsFrame += _initFrameCallback;
	}

	private void OnInitPhysicsFrame()
	{
		int initFrameLeaseVersion = _initFrameLeaseVersion;
		CancelInitFrameCallback();
		if (IsCurrentLease(initFrameLeaseVersion))
		{
			autoCollect = GameSaveManager.Instance.GetFeatureValue("CoinCollect") > 0 && GetTree().GetNodeCountInGroup("GoldMagnet") <= 0;
			if (autoCollect)
			{
				SetPhysicsProcess(enable: false);
				ScheduleAutoCollect(initFrameLeaseVersion);
			}
		}
	}

	private void ScheduleAutoCollect(int leaseVersion)
	{
		CancelAutoCollectTimer();
		_autoCollectLeaseVersion = leaseVersion;
		_autoCollectTimer = GetTree().CreateTimer(0.5, processAlways: false);
		if (_autoCollectCallback == null)
		{
			_autoCollectCallback = OnAutoCollectTimeout;
		}
		_autoCollectTimer.Timeout += _autoCollectCallback;
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
		CancelInitFrameCallback();
		CancelAutoCollectTimer();
	}

	private void CancelInitFrameCallback()
	{
		if (GodotObject.IsInstanceValid(_initFrameTree) && _initFrameCallback != null)
		{
			_initFrameTree.PhysicsFrame -= _initFrameCallback;
		}
		_initFrameLeaseVersion = -1;
		_initFrameTree = null;
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

	public void Destroy()
	{
		ObjectManagerConfig.OBJECT poolKeyByCoinObjectId = DropItemRegistry.GetPoolKeyByCoinObjectId(coinObjectId);
		if (poolKeyByCoinObjectId != ObjectManagerConfig.OBJECT.NOONE)
		{
			ObjectManager.PoolPush(poolKeyByCoinObjectId, this);
			return;
		}
		switch (coinObjectId)
		{
		case 0:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_SILVER, this);
			break;
		case 1:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_GOLD, this);
			break;
		case 2:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_DIAMOND, this);
			break;
		case 3:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_LUCKY_BAG, this);
			break;
		case 4:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_TQ, this);
			break;
		case 5:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_YB1, this);
			break;
		case 6:
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.COIN_YB2, this);
			break;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(31)
		{
			new MethodInfo(MethodName._GetDropItemConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestartCoinAnimations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePoolLifecyclePolicy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampToViewportIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMovementActivityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Collection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCollectionTargetPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishCollectionFlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishCollectionFlightCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishFadeAndDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishFadeAndDestroyCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleCollectedValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginLease, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateLease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCurrentLease, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleInitAfterPhysics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnInitPhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleAutoCollect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "leaseVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAutoCollectTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelDeferredCallbacks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelInitFrameCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelAutoCollectTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetDropItemConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<DropItemConfig>(_GetDropItemConfig());
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.RestartCoinAnimations && args.Count == 1)
		{
			RestartCoinAnimations(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Recycle && args.Count == 0)
		{
			Recycle();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 3)
		{
			Init(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
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
		if (method == MethodName.ClampToViewportIfNeeded && args.Count == 0)
		{
			ClampToViewportIfNeeded();
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
		if (method == MethodName.GetCollectionTargetPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCollectionTargetPosition());
			return true;
		}
		if (method == MethodName.DieDown && args.Count == 0)
		{
			DieDown();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishCollectionFlight && args.Count == 1)
		{
			FinishCollectionFlight(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishCollectionFlightCallback && args.Count == 0)
		{
			FinishCollectionFlightCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishFadeAndDestroy && args.Count == 1)
		{
			FinishFadeAndDestroy(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishFadeAndDestroyCallback && args.Count == 0)
		{
			FinishFadeAndDestroyCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleCollectedValue && args.Count == 1)
		{
			HandleCollectedValue(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.ScheduleInitAfterPhysics && args.Count == 1)
		{
			ScheduleInitAfterPhysics(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnInitPhysicsFrame && args.Count == 0)
		{
			OnInitPhysicsFrame();
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
		if (method == MethodName.CancelInitFrameCallback && args.Count == 0)
		{
			CancelInitFrameCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelAutoCollectTimer && args.Count == 0)
		{
			CancelAutoCollectTimer();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetDropItemConfig)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.RestartCoinAnimations)
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.EnsurePoolLifecyclePolicy)
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
		if (method == MethodName.ClampToViewportIfNeeded)
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
		if (method == MethodName.GetCollectionTargetPosition)
		{
			return true;
		}
		if (method == MethodName.DieDown)
		{
			return true;
		}
		if (method == MethodName.FinishCollectionFlight)
		{
			return true;
		}
		if (method == MethodName.FinishCollectionFlightCallback)
		{
			return true;
		}
		if (method == MethodName.FinishFadeAndDestroy)
		{
			return true;
		}
		if (method == MethodName.FinishFadeAndDestroyCallback)
		{
			return true;
		}
		if (method == MethodName.HandleCollectedValue)
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
		if (method == MethodName.ScheduleInitAfterPhysics)
		{
			return true;
		}
		if (method == MethodName.OnInitPhysicsFrame)
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
		if (method == MethodName.CancelInitFrameCallback)
		{
			return true;
		}
		if (method == MethodName.CancelAutoCollectTimer)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.coinObjectId)
		{
			coinObjectId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.moveComponent)
		{
			moveComponent = VariantUtils.ConvertTo<MoveComponent>(in value);
			return true;
		}
		if (name == PropertyName.fallAudio)
		{
			fallAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pickAudio)
		{
			pickAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.canMagnet)
		{
			canMagnet = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.disabledInput)
		{
			disabledInput = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.randFreshIndex)
		{
			randFreshIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._viewportCheckCountdown)
		{
			_viewportCheckCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._dieDownTimer)
		{
			_dieDownTimer = VariantUtils.ConvertTo<Timer>(in value);
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
		if (name == PropertyName._initFrameTree)
		{
			_initFrameTree = VariantUtils.ConvertTo<SceneTree>(in value);
			return true;
		}
		if (name == PropertyName._autoCollectTimer)
		{
			_autoCollectTimer = VariantUtils.ConvertTo<SceneTreeTimer>(in value);
			return true;
		}
		if (name == PropertyName._initFrameLeaseVersion)
		{
			_initFrameLeaseVersion = VariantUtils.ConvertTo<int>(in value);
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.coinObjectId)
		{
			value = VariantUtils.CreateFrom<int>(coinObjectId);
			return true;
		}
		if (name == PropertyName.spriteNode)
		{
			value = VariantUtils.CreateFrom(in spriteNode);
			return true;
		}
		if (name == PropertyName.moveComponent)
		{
			value = VariantUtils.CreateFrom(in moveComponent);
			return true;
		}
		if (name == PropertyName.fallAudio)
		{
			value = VariantUtils.CreateFrom(in fallAudio);
			return true;
		}
		if (name == PropertyName.pickAudio)
		{
			value = VariantUtils.CreateFrom(in pickAudio);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		if (name == PropertyName.canMagnet)
		{
			value = VariantUtils.CreateFrom(in canMagnet);
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
		if (name == PropertyName.disabledInput)
		{
			value = VariantUtils.CreateFrom(in disabledInput);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
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
		if (name == PropertyName.randFreshIndex)
		{
			value = VariantUtils.CreateFrom(in randFreshIndex);
			return true;
		}
		if (name == PropertyName._viewportCheckCountdown)
		{
			value = VariantUtils.CreateFrom(in _viewportCheckCountdown);
			return true;
		}
		if (name == PropertyName._dieDownTimer)
		{
			value = VariantUtils.CreateFrom(in _dieDownTimer);
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
		if (name == PropertyName._initFrameTree)
		{
			value = VariantUtils.CreateFrom(in _initFrameTree);
			return true;
		}
		if (name == PropertyName._autoCollectTimer)
		{
			value = VariantUtils.CreateFrom(in _autoCollectTimer);
			return true;
		}
		if (name == PropertyName._initFrameLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in _initFrameLeaseVersion);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moveComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.fallAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.pickAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canMagnet, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCollect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.die, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.disabledInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.view, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.camera, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.viewSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoCollect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.randFreshIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._viewportCheckCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.coinObjectId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._dieDownTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectionTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fadeTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initFrameTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoCollectTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._initFrameLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._autoCollectLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._collectionLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._fadeLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._collectionTweenCallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._fadeTweenCallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._leaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._supportsDirectPoolLifecycleDispatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._poolLifecyclePolicyInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.coinObjectId, Variant.From<int>(coinObjectId));
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.moveComponent, Variant.From(in moveComponent));
		info.AddProperty(PropertyName.fallAudio, Variant.From(in fallAudio));
		info.AddProperty(PropertyName.pickAudio, Variant.From(in pickAudio));
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName.canMagnet, Variant.From(in canMagnet));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.isCollect, Variant.From(in isCollect));
		info.AddProperty(PropertyName.die, Variant.From(in die));
		info.AddProperty(PropertyName.disabledInput, Variant.From(in disabledInput));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.view, Variant.From(in view));
		info.AddProperty(PropertyName.camera, Variant.From(in camera));
		info.AddProperty(PropertyName.viewSize, Variant.From(in viewSize));
		info.AddProperty(PropertyName.autoCollect, Variant.From(in autoCollect));
		info.AddProperty(PropertyName.randFreshIndex, Variant.From(in randFreshIndex));
		info.AddProperty(PropertyName._viewportCheckCountdown, Variant.From(in _viewportCheckCountdown));
		info.AddProperty(PropertyName._dieDownTimer, Variant.From(in _dieDownTimer));
		info.AddProperty(PropertyName._collectionTween, Variant.From(in _collectionTween));
		info.AddProperty(PropertyName._fadeTween, Variant.From(in _fadeTween));
		info.AddProperty(PropertyName._initFrameTree, Variant.From(in _initFrameTree));
		info.AddProperty(PropertyName._autoCollectTimer, Variant.From(in _autoCollectTimer));
		info.AddProperty(PropertyName._initFrameLeaseVersion, Variant.From(in _initFrameLeaseVersion));
		info.AddProperty(PropertyName._autoCollectLeaseVersion, Variant.From(in _autoCollectLeaseVersion));
		info.AddProperty(PropertyName._collectionLeaseVersion, Variant.From(in _collectionLeaseVersion));
		info.AddProperty(PropertyName._fadeLeaseVersion, Variant.From(in _fadeLeaseVersion));
		info.AddProperty(PropertyName._collectionTweenCallback, Variant.From(in _collectionTweenCallback));
		info.AddProperty(PropertyName._fadeTweenCallback, Variant.From(in _fadeTweenCallback));
		info.AddProperty(PropertyName._leaseVersion, Variant.From(in _leaseVersion));
		info.AddProperty(PropertyName._supportsDirectPoolLifecycleDispatch, Variant.From(in _supportsDirectPoolLifecycleDispatch));
		info.AddProperty(PropertyName._poolLifecyclePolicyInitialized, Variant.From(in _poolLifecyclePolicyInitialized));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.coinObjectId, out var value))
		{
			coinObjectId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spriteNode, out var value2))
		{
			spriteNode = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.moveComponent, out var value3))
		{
			moveComponent = value3.As<MoveComponent>();
		}
		if (info.TryGetProperty(PropertyName.fallAudio, out var value4))
		{
			fallAudio = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pickAudio, out var value5))
		{
			pickAudio = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value6))
		{
			num = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.canMagnet, out var value7))
		{
			canMagnet = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value8))
		{
			height = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isCollect, out var value9))
		{
			isCollect = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.die, out var value10))
		{
			die = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.disabledInput, out var value11))
		{
			disabledInput = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value12))
		{
			over = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.view, out var value13))
		{
			view = value13.As<Viewport>();
		}
		if (info.TryGetProperty(PropertyName.camera, out var value14))
		{
			camera = value14.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName.viewSize, out var value15))
		{
			viewSize = value15.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.autoCollect, out var value16))
		{
			autoCollect = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.randFreshIndex, out var value17))
		{
			randFreshIndex = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._viewportCheckCountdown, out var value18))
		{
			_viewportCheckCountdown = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._dieDownTimer, out var value19))
		{
			_dieDownTimer = value19.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._collectionTween, out var value20))
		{
			_collectionTween = value20.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._fadeTween, out var value21))
		{
			_fadeTween = value21.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._initFrameTree, out var value22))
		{
			_initFrameTree = value22.As<SceneTree>();
		}
		if (info.TryGetProperty(PropertyName._autoCollectTimer, out var value23))
		{
			_autoCollectTimer = value23.As<SceneTreeTimer>();
		}
		if (info.TryGetProperty(PropertyName._initFrameLeaseVersion, out var value24))
		{
			_initFrameLeaseVersion = value24.As<int>();
		}
		if (info.TryGetProperty(PropertyName._autoCollectLeaseVersion, out var value25))
		{
			_autoCollectLeaseVersion = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._collectionLeaseVersion, out var value26))
		{
			_collectionLeaseVersion = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._fadeLeaseVersion, out var value27))
		{
			_fadeLeaseVersion = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._collectionTweenCallback, out var value28))
		{
			_collectionTweenCallback = value28.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._fadeTweenCallback, out var value29))
		{
			_fadeTweenCallback = value29.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._leaseVersion, out var value30))
		{
			_leaseVersion = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._supportsDirectPoolLifecycleDispatch, out var value31))
		{
			_supportsDirectPoolLifecycleDispatch = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._poolLifecyclePolicyInitialized, out var value32))
		{
			_poolLifecyclePolicyInitialized = value32.As<bool>();
		}
	}
}
