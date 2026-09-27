using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/DamagePart/DamagePartDrop.cs")]
public class DamagePartDrop : TowerDefenseGroundItemBase, IObjectPoolLifecycle
{
	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName EnsurePoolLifecyclePolicy = "EnsurePoolLifecyclePolicy";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName Recycle = "Recycle";

		public static readonly StringName Init = "Init";

		public static readonly StringName InitializeRenderBucket = "InitializeRenderBucket";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName EnsureFlightState = "EnsureFlightState";

		public static readonly StringName ApplyFlightTransform = "ApplyFlightTransform";

		public static readonly StringName EnsureLandingPlane = "EnsureLandingPlane";

		public static readonly StringName Over = "Over";

		public static readonly StringName CreateSplash = "CreateSplash";

		public static readonly StringName BeginLease = "BeginLease";

		public static readonly StringName InvalidateLease = "InvalidateLease";

		public static readonly StringName ScheduleLandingFade = "ScheduleLandingFade";

		public static readonly StringName OnLandingFadeTimerTimeout = "OnLandingFadeTimerTimeout";

		public static readonly StringName OnLandingFadeFinished = "OnLandingFadeFinished";

		public static readonly StringName CancelLandingFadeTimer = "CancelLandingFadeTimer";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName shadowSprite = "shadowSprite";

		public static readonly StringName spriteGroup = "spriteGroup";

		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName moveComponent = "moveComponent";

		public static readonly StringName landingPlaneReady = "landingPlaneReady";

		public static readonly StringName landingWorldY = "landingWorldY";

		public static readonly StringName flightStateReady = "flightStateReady";

		public static readonly StringName flightWorldPosition = "flightWorldPosition";

		public static readonly StringName shadowWorldPosition = "shadowWorldPosition";

		public static readonly StringName flightVelocity = "flightVelocity";

		public static readonly StringName flightWorldTransform = "flightWorldTransform";

		public static readonly StringName flightRootWorldInverse = "flightRootWorldInverse";

		public static readonly StringName supportsDirectPoolLifecycleDispatch = "supportsDirectPoolLifecycleDispatch";

		public static readonly StringName poolLifecyclePolicyInitialized = "poolLifecyclePolicyInitialized";

		public static readonly StringName landingFadeTimer = "landingFadeTimer";

		public static readonly StringName landingFadeTween = "landingFadeTween";

		public static readonly StringName leaseVersion = "leaseVersion";

		public static readonly StringName landingFadeLeaseVersion = "landingFadeLeaseVersion";

		public static readonly StringName poolPushQueued = "poolPushQueued";

		public static readonly StringName height = "height";

		public static readonly StringName jumpSpeed = "jumpSpeed";

		public static readonly StringName jumpTime = "jumpTime";

		public static readonly StringName initVelocity = "initVelocity";

		public static readonly StringName over = "over";

		public static readonly StringName sprite = "sprite";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private static readonly StringName EffectGroupName = new StringName("Effect");

	private const float DropGravity = 1960f;

	private const int MinimumDamagePartZIndex = -4095;

	private TowerDefenseShadowVisual shadowSprite;

	private Node2D spriteGroup;

	private Node2D spriteNode;

	private MoveComponent moveComponent;

	private bool landingPlaneReady;

	private float landingWorldY;

	private bool flightStateReady;

	private Vector2 flightWorldPosition;

	private Vector2 shadowWorldPosition;

	private Vector2 flightVelocity;

	private Transform2D flightWorldTransform;

	private Transform2D flightRootWorldInverse;

	private bool supportsDirectPoolLifecycleDispatch;

	private bool poolLifecyclePolicyInitialized;

	private SceneTreeTimer landingFadeTimer;

	private Action landingFadeTimerCallback;

	private Tween landingFadeTween;

	private int leaseVersion;

	private int landingFadeLeaseVersion = -1;

	private bool poolPushQueued;

	public double height = 60.0;

	public double jumpSpeed = 300.0;

	public int jumpTime = 3;

	public Vector2 initVelocity = Vector2.Zero;

	public bool over;

	public Node2D sprite;

	bool IObjectPoolLifecycle.SupportsDirectPoolLifecycleDispatch
	{
		get
		{
			EnsurePoolLifecyclePolicy();
			return supportsDirectPoolLifecycleDispatch;
		}
	}

	public override void _Ready()
	{
		EnsurePoolLifecyclePolicy();
		shadowSprite = TowerDefenseShadowVisual.Capture(this, GetNodeOrNull<Sprite2D>("%ShadowSprite"));
		spriteGroup = GetNode<Node2D>("%SpriteGroup");
		spriteNode = GetNode<Node2D>("%Sprite");
		moveComponent = GetNode<MoveComponent>("%MoveComponent");
		landingFadeTimerCallback = OnLandingFadeTimerTimeout;
		SetProcess(enable: false);
	}

	private void EnsurePoolLifecyclePolicy()
	{
		if (!poolLifecyclePolicyInitialized)
		{
			poolLifecyclePolicyInitialized = true;
			Type type = GetType();
			supportsDirectPoolLifecycleDispatch = type == typeof(DamagePartDrop) && ObjectPoolLifecyclePolicy.HasBuiltInCSharpScript(this);
		}
	}

	public void Refresh()
	{
		BeginLease();
		Visible = false;
		AddToGroup(EffectGroupName, persistent: true);
		moveComponent?.MoveClear();
		SetPhysicsProcess(enable: false);
		SetProcess(enable: true);
		spriteNode.Position = Vector2.Zero;
		spriteNode.Rotation = 0f;
		spriteGroup.Position = Vector2.Zero;
		Scale = Vector2.One;
		Rotation = 0f;
		height = 100.0;
		initVelocity = Vector2.Zero;
		Modulate = new Color(Modulate);
		jumpSpeed = 300.0;
		jumpTime = 3;
		over = false;
		poolPushQueued = false;
		landingPlaneReady = false;
		landingWorldY = 0f;
		flightStateReady = false;
		flightWorldPosition = Vector2.Zero;
		shadowWorldPosition = Vector2.Zero;
		flightVelocity = Vector2.Zero;
		flightWorldTransform = Transform2D.Identity;
		flightRootWorldInverse = Transform2D.Identity;
		sprite = null;
	}

	public void Recycle()
	{
		InvalidateLease();
		Visible = false;
		RemoveFromGroup(EffectGroupName);
		moveComponent?.MoveClear();
		SetPhysicsProcess(enable: false);
		SetProcess(enable: false);
		over = true;
		poolPushQueued = false;
		landingPlaneReady = false;
		flightStateReady = false;
		flightVelocity = Vector2.Zero;
		if (GodotObject.IsInstanceValid(sprite))
		{
			if (GodotObject.IsInstanceValid(sprite.GetParent()))
			{
				sprite.GetParent().RemoveChild(sprite);
			}
			sprite.QueueFree();
		}
		sprite = null;
	}

	void IObjectPoolLifecycle.RefreshFromPool()
	{
		Refresh();
	}

	void IObjectPoolLifecycle.RecycleToPool()
	{
		Recycle();
	}

	public void Init(Node2D _sprite, double _height, Vector2 _initVelocity = default(Vector2), bool preserveSpritePosition = false)
	{
		if (GodotObject.IsInstanceValid(_sprite) && GodotObject.IsInstanceValid(spriteGroup))
		{
			sprite = _sprite;
			if (!preserveSpritePosition)
			{
				sprite.Position = Vector2.Zero;
			}
			spriteGroup.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
			height = _height;
			initVelocity = _initVelocity;
			landingPlaneReady = false;
			flightStateReady = false;
			flightVelocity = initVelocity;
			moveComponent.MoveClear();
			SetPhysicsProcess(enable: true);
			SetProcess(enable: true);
		}
	}

	public void InitializeRenderBucket(Vector2I sourceGridPosition, Vector2 worldPosition)
	{
		Vector2I vector2I = sourceGridPosition;
		if (vector2I.Y < 0 && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			Vector2I mapGridPos = TowerDefenseManager.Instance.GetMapGridPos(worldPosition - new Vector2(0f, 20f));
			if (mapGridPos.Y >= 0)
			{
				vector2I = mapGridPos;
			}
		}
		if (vector2I.Y >= 0)
		{
			gridPos = vector2I;
			FreshZIndex();
		}
		else
		{
			gridPos = new Vector2I(-1, -1);
			SetTransientRenderZIndex(-4095, "damage-part-invalid-grid");
		}
	}

	public override void _Process(double delta)
	{
		if (GodotObject.IsInstanceValid(shadowSprite) && !shadowSprite.RequiresLegacyRendering && shadowSprite.Visible && !(Modulate.A <= 0f))
		{
			TowerDefenseShadowMultiMeshRenderer.SubmitVisual(shadowSprite, gridPos.Y);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (over)
		{
			return;
		}
		EnsureLandingPlane();
		EnsureFlightState();
		float num = (float)delta;
		flightVelocity.Y += 1960f * num;
		flightWorldPosition += flightVelocity * num;
		shadowWorldPosition = new Vector2(flightWorldPosition.X, landingWorldY);
		shadowSprite.Position = flightRootWorldInverse * shadowWorldPosition;
		if (flightWorldPosition.Y > landingWorldY)
		{
			Vector2I mapGridPos = TowerDefenseManager.Instance.GetMapGridPos(shadowWorldPosition - new Vector2(0f, 20f));
			InitializeRenderBucket((mapGridPos.Y >= 0) ? mapGridPos : gridPos, shadowWorldPosition);
			cell = TowerDefenseManager.GetMapCell(gridPos);
			if (GodotObject.IsInstanceValid(cell) && cell.isWater)
			{
				ApplyFlightTransform(0f);
				CreateSplash();
				Over();
				return;
			}
			flightWorldPosition.Y = landingWorldY - 1f;
			ApplyFlightTransform(0f);
			jumpTime--;
			if (jumpTime > 0 && jumpSpeed >= 10.0)
			{
				flightVelocity = new Vector2(flightVelocity.X / 2f, (float)(0.0 - jumpSpeed));
				jumpSpeed /= 2.0;
				return;
			}
			moveComponent.MoveClear();
			flightVelocity = Vector2.Zero;
			over = true;
			SetPhysicsProcess(enable: false);
			ScheduleLandingFade(leaseVersion);
		}
		else
		{
			float rotationDelta = ((flightVelocity.X != 0f) ? (flightVelocity.X / 40f * num) : 0f);
			ApplyFlightTransform(rotationDelta);
		}
	}

	private void EnsureFlightState()
	{
		if (!flightStateReady)
		{
			flightWorldPosition = spriteNode.GlobalPosition;
			flightWorldTransform = spriteNode.GlobalTransform;
			flightRootWorldInverse = GlobalTransform.AffineInverse();
			flightStateReady = true;
		}
	}

	private void ApplyFlightTransform(float rotationDelta)
	{
		if (rotationDelta != 0f)
		{
			float num = Mathf.Sin(rotationDelta);
			float num2 = Mathf.Cos(rotationDelta);
			Vector2 x = flightWorldTransform.X;
			Vector2 y = flightWorldTransform.Y;
			flightWorldTransform.X = new Vector2(x.X * num2 - x.Y * num, x.X * num + x.Y * num2);
			flightWorldTransform.Y = new Vector2(y.X * num2 - y.Y * num, y.X * num + y.Y * num2);
		}
		flightWorldTransform.Origin = flightWorldPosition;
		spriteNode.Transform = flightRootWorldInverse * flightWorldTransform;
	}

	private void EnsureLandingPlane()
	{
		if (!landingPlaneReady)
		{
			landingWorldY = GlobalPosition.Y + (float)height;
			landingPlaneReady = true;
		}
	}

	public void Over()
	{
		if (!poolPushQueued)
		{
			poolPushQueued = true;
			InvalidateLease();
			moveComponent?.MoveClear();
			flightVelocity = Vector2.Zero;
			SetPhysicsProcess(enable: false);
			over = true;
			ObjectManager.PoolPush(ObjectManagerConfig.OBJECT.damagePart, this);
		}
	}

	public TowerDefenseEffectSpriteOnce CreateSplash()
	{
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return null;
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = ObjectManager.PoolPop(ObjectManagerConfig.OBJECT.PARTICLES_SPLASH, node2D) as TowerDefenseEffectSpriteOnce;
		if (!GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
		{
			return null;
		}
		towerDefenseEffectSpriteOnce.gridPos = gridPos;
		towerDefenseEffectSpriteOnce.GlobalPosition = shadowWorldPosition - new Vector2(0f, 20f);
		return towerDefenseEffectSpriteOnce;
	}

	private void BeginLease()
	{
		InvalidateLease();
		poolPushQueued = false;
	}

	private void InvalidateLease()
	{
		leaseVersion++;
		landingFadeLeaseVersion = -1;
		CancelLandingFadeTimer();
		KillTween(ref landingFadeTween);
	}

	private void ScheduleLandingFade(int currentLease)
	{
		CancelLandingFadeTimer();
		landingFadeLeaseVersion = currentLease;
		landingFadeTimer = GetTree().CreateTimer(0.5, processAlways: false);
		landingFadeTimer.Timeout += landingFadeTimerCallback;
	}

	private void OnLandingFadeTimerTimeout()
	{
		if (GodotObject.IsInstanceValid(landingFadeTimer))
		{
			landingFadeTimer.Timeout -= landingFadeTimerCallback;
		}
		landingFadeTimer = null;
		if (landingFadeLeaseVersion == leaseVersion && IsInsideTree())
		{
			KillTween(ref landingFadeTween);
			landingFadeTween = CreateTween();
			landingFadeTween.TweenProperty(this, "modulate:a", 0.0, 0.1);
			landingFadeTween.Finished += OnLandingFadeFinished;
		}
	}

	private void OnLandingFadeFinished()
	{
		landingFadeTween = null;
		if (landingFadeLeaseVersion == leaseVersion)
		{
			Over();
		}
	}

	private void CancelLandingFadeTimer()
	{
		if (GodotObject.IsInstanceValid(landingFadeTimer) && landingFadeTimerCallback != null)
		{
			landingFadeTimer.Timeout -= landingFadeTimerCallback;
		}
		landingFadeTimer = null;
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
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePoolLifecyclePolicy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "_height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_initVelocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preserveSpritePosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeRenderBucket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "sourceGridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "worldPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureFlightState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFlightTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "rotationDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureLandingPlane, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Over, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSplash, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginLease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateLease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleLandingFade, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "currentLease", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnLandingFadeTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnLandingFadeFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelLandingFadeTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.EnsurePoolLifecyclePolicy && args.Count == 0)
		{
			EnsurePoolLifecyclePolicy();
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
		if (method == MethodName.Init && args.Count == 4)
		{
			Init(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeRenderBucket && args.Count == 2)
		{
			InitializeRenderBucket(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureFlightState && args.Count == 0)
		{
			EnsureFlightState();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFlightTransform && args.Count == 1)
		{
			ApplyFlightTransform(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureLandingPlane && args.Count == 0)
		{
			EnsureLandingPlane();
			ret = default;
			return true;
		}
		if (method == MethodName.Over && args.Count == 0)
		{
			Over();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSplash && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(CreateSplash());
			return true;
		}
		if (method == MethodName.BeginLease && args.Count == 0)
		{
			BeginLease();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateLease && args.Count == 0)
		{
			InvalidateLease();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleLandingFade && args.Count == 1)
		{
			ScheduleLandingFade(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLandingFadeTimerTimeout && args.Count == 0)
		{
			OnLandingFadeTimerTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.OnLandingFadeFinished && args.Count == 0)
		{
			OnLandingFadeFinished();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelLandingFadeTimer && args.Count == 0)
		{
			CancelLandingFadeTimer();
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
		if (method == MethodName.EnsurePoolLifecyclePolicy)
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
		if (method == MethodName.InitializeRenderBucket)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.EnsureFlightState)
		{
			return true;
		}
		if (method == MethodName.ApplyFlightTransform)
		{
			return true;
		}
		if (method == MethodName.EnsureLandingPlane)
		{
			return true;
		}
		if (method == MethodName.Over)
		{
			return true;
		}
		if (method == MethodName.CreateSplash)
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
		if (method == MethodName.ScheduleLandingFade)
		{
			return true;
		}
		if (method == MethodName.OnLandingFadeTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.OnLandingFadeFinished)
		{
			return true;
		}
		if (method == MethodName.CancelLandingFadeTimer)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.shadowSprite)
		{
			shadowSprite = VariantUtils.ConvertTo<TowerDefenseShadowVisual>(in value);
			return true;
		}
		if (name == PropertyName.spriteGroup)
		{
			spriteGroup = VariantUtils.ConvertTo<Node2D>(in value);
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
		if (name == PropertyName.landingPlaneReady)
		{
			landingPlaneReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.landingWorldY)
		{
			landingWorldY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.flightStateReady)
		{
			flightStateReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.flightWorldPosition)
		{
			flightWorldPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.shadowWorldPosition)
		{
			shadowWorldPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.flightVelocity)
		{
			flightVelocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.flightWorldTransform)
		{
			flightWorldTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.flightRootWorldInverse)
		{
			flightRootWorldInverse = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.supportsDirectPoolLifecycleDispatch)
		{
			supportsDirectPoolLifecycleDispatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.poolLifecyclePolicyInitialized)
		{
			poolLifecyclePolicyInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.landingFadeTimer)
		{
			landingFadeTimer = VariantUtils.ConvertTo<SceneTreeTimer>(in value);
			return true;
		}
		if (name == PropertyName.landingFadeTween)
		{
			landingFadeTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.leaseVersion)
		{
			leaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.landingFadeLeaseVersion)
		{
			landingFadeLeaseVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.poolPushQueued)
		{
			poolPushQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpSpeed)
		{
			jumpSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpTime)
		{
			jumpTime = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.initVelocity)
		{
			initVelocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.shadowSprite)
		{
			value = VariantUtils.CreateFrom(in shadowSprite);
			return true;
		}
		if (name == PropertyName.spriteGroup)
		{
			value = VariantUtils.CreateFrom(in spriteGroup);
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
		if (name == PropertyName.landingPlaneReady)
		{
			value = VariantUtils.CreateFrom(in landingPlaneReady);
			return true;
		}
		if (name == PropertyName.landingWorldY)
		{
			value = VariantUtils.CreateFrom(in landingWorldY);
			return true;
		}
		if (name == PropertyName.flightStateReady)
		{
			value = VariantUtils.CreateFrom(in flightStateReady);
			return true;
		}
		if (name == PropertyName.flightWorldPosition)
		{
			value = VariantUtils.CreateFrom(in flightWorldPosition);
			return true;
		}
		if (name == PropertyName.shadowWorldPosition)
		{
			value = VariantUtils.CreateFrom(in shadowWorldPosition);
			return true;
		}
		if (name == PropertyName.flightVelocity)
		{
			value = VariantUtils.CreateFrom(in flightVelocity);
			return true;
		}
		if (name == PropertyName.flightWorldTransform)
		{
			value = VariantUtils.CreateFrom(in flightWorldTransform);
			return true;
		}
		if (name == PropertyName.flightRootWorldInverse)
		{
			value = VariantUtils.CreateFrom(in flightRootWorldInverse);
			return true;
		}
		if (name == PropertyName.supportsDirectPoolLifecycleDispatch)
		{
			value = VariantUtils.CreateFrom(in supportsDirectPoolLifecycleDispatch);
			return true;
		}
		if (name == PropertyName.poolLifecyclePolicyInitialized)
		{
			value = VariantUtils.CreateFrom(in poolLifecyclePolicyInitialized);
			return true;
		}
		if (name == PropertyName.landingFadeTimer)
		{
			value = VariantUtils.CreateFrom(in landingFadeTimer);
			return true;
		}
		if (name == PropertyName.landingFadeTween)
		{
			value = VariantUtils.CreateFrom(in landingFadeTween);
			return true;
		}
		if (name == PropertyName.leaseVersion)
		{
			value = VariantUtils.CreateFrom(in leaseVersion);
			return true;
		}
		if (name == PropertyName.landingFadeLeaseVersion)
		{
			value = VariantUtils.CreateFrom(in landingFadeLeaseVersion);
			return true;
		}
		if (name == PropertyName.poolPushQueued)
		{
			value = VariantUtils.CreateFrom(in poolPushQueued);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.jumpSpeed)
		{
			value = VariantUtils.CreateFrom(in jumpSpeed);
			return true;
		}
		if (name == PropertyName.jumpTime)
		{
			value = VariantUtils.CreateFrom(in jumpTime);
			return true;
		}
		if (name == PropertyName.initVelocity)
		{
			value = VariantUtils.CreateFrom(in initVelocity);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.shadowSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteGroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moveComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.landingPlaneReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.landingWorldY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.flightStateReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.flightWorldPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.shadowWorldPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.flightVelocity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.flightWorldTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.flightRootWorldInverse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.supportsDirectPoolLifecycleDispatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.poolLifecyclePolicyInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.landingFadeTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.landingFadeTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.leaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.landingFadeLeaseVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.poolPushQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.height, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpSpeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.jumpTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.initVelocity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shadowSprite, Variant.From(in shadowSprite));
		info.AddProperty(PropertyName.spriteGroup, Variant.From(in spriteGroup));
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.moveComponent, Variant.From(in moveComponent));
		info.AddProperty(PropertyName.landingPlaneReady, Variant.From(in landingPlaneReady));
		info.AddProperty(PropertyName.landingWorldY, Variant.From(in landingWorldY));
		info.AddProperty(PropertyName.flightStateReady, Variant.From(in flightStateReady));
		info.AddProperty(PropertyName.flightWorldPosition, Variant.From(in flightWorldPosition));
		info.AddProperty(PropertyName.shadowWorldPosition, Variant.From(in shadowWorldPosition));
		info.AddProperty(PropertyName.flightVelocity, Variant.From(in flightVelocity));
		info.AddProperty(PropertyName.flightWorldTransform, Variant.From(in flightWorldTransform));
		info.AddProperty(PropertyName.flightRootWorldInverse, Variant.From(in flightRootWorldInverse));
		info.AddProperty(PropertyName.supportsDirectPoolLifecycleDispatch, Variant.From(in supportsDirectPoolLifecycleDispatch));
		info.AddProperty(PropertyName.poolLifecyclePolicyInitialized, Variant.From(in poolLifecyclePolicyInitialized));
		info.AddProperty(PropertyName.landingFadeTimer, Variant.From(in landingFadeTimer));
		info.AddProperty(PropertyName.landingFadeTween, Variant.From(in landingFadeTween));
		info.AddProperty(PropertyName.leaseVersion, Variant.From(in leaseVersion));
		info.AddProperty(PropertyName.landingFadeLeaseVersion, Variant.From(in landingFadeLeaseVersion));
		info.AddProperty(PropertyName.poolPushQueued, Variant.From(in poolPushQueued));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.jumpSpeed, Variant.From(in jumpSpeed));
		info.AddProperty(PropertyName.jumpTime, Variant.From(in jumpTime));
		info.AddProperty(PropertyName.initVelocity, Variant.From(in initVelocity));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shadowSprite, out var value))
		{
			shadowSprite = value.As<TowerDefenseShadowVisual>();
		}
		if (info.TryGetProperty(PropertyName.spriteGroup, out var value2))
		{
			spriteGroup = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.spriteNode, out var value3))
		{
			spriteNode = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.moveComponent, out var value4))
		{
			moveComponent = value4.As<MoveComponent>();
		}
		if (info.TryGetProperty(PropertyName.landingPlaneReady, out var value5))
		{
			landingPlaneReady = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.landingWorldY, out var value6))
		{
			landingWorldY = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.flightStateReady, out var value7))
		{
			flightStateReady = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.flightWorldPosition, out var value8))
		{
			flightWorldPosition = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.shadowWorldPosition, out var value9))
		{
			shadowWorldPosition = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.flightVelocity, out var value10))
		{
			flightVelocity = value10.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.flightWorldTransform, out var value11))
		{
			flightWorldTransform = value11.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.flightRootWorldInverse, out var value12))
		{
			flightRootWorldInverse = value12.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.supportsDirectPoolLifecycleDispatch, out var value13))
		{
			supportsDirectPoolLifecycleDispatch = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.poolLifecyclePolicyInitialized, out var value14))
		{
			poolLifecyclePolicyInitialized = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.landingFadeTimer, out var value15))
		{
			landingFadeTimer = value15.As<SceneTreeTimer>();
		}
		if (info.TryGetProperty(PropertyName.landingFadeTween, out var value16))
		{
			landingFadeTween = value16.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.leaseVersion, out var value17))
		{
			leaseVersion = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.landingFadeLeaseVersion, out var value18))
		{
			landingFadeLeaseVersion = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName.poolPushQueued, out var value19))
		{
			poolPushQueued = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value20))
		{
			height = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpSpeed, out var value21))
		{
			jumpSpeed = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpTime, out var value22))
		{
			jumpTime = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName.initVelocity, out var value23))
		{
			initVelocity = value23.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value24))
		{
			over = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value25))
		{
			sprite = value25.As<Node2D>();
		}
	}
}
