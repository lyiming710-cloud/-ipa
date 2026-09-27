using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Portal/Object/TowerDefensePortal.cs")]
public class TowerDefensePortal : TowerDefenseGroundItemBase
{
	private class PortalZone : IProjectileZone
	{
		private readonly TowerDefensePortal _portal;

		private readonly bool _isPortal1;

		private Rect2 _rect;

		public Rect2 WorldRect => _rect;

		public int GridY
		{
			get
			{
				if (!_isPortal1)
				{
					return _portal.gridPos2.Y;
				}
				return _portal.gridPos1.Y;
			}
		}

		public int RowSpan => 0;

		public PortalZone(TowerDefensePortal portal, bool isPortal1)
		{
			_portal = portal;
			_isPortal1 = isPortal1;
		}

		public void UpdateRect()
		{
			AabbArea2D aabbArea2D = (_isPortal1 ? _portal.hitBox1 : _portal.hitBox2);
			if (!GodotObject.IsInstanceValid(aabbArea2D) || !AabbShapeUtil.TryComputeAreaWorldRect(aabbArea2D, out _rect))
			{
				_rect = new Rect2(Vector2.Zero, Vector2.Zero);
			}
		}

		public void OnBulletIntersect(ref BulletData b, int index)
		{
			_portal.TeleportBulletData(ref b, index, _isPortal1);
		}

		public void OnProjectileIntersect(TowerDefenseProjectile projectile)
		{
			_portal.TeleportProjectile(projectile, _isPortal1);
		}
	}

	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName RefreshProjectileExitGuards = "RefreshProjectileExitGuards";

		public static readonly StringName IsBulletInsideEitherPortal = "IsBulletInsideEitherPortal";

		public static readonly StringName IsProjectileInsideEitherPortal = "IsProjectileInsideEitherPortal";

		public static readonly StringName Init = "Init";

		public static readonly StringName ChangePos = "ChangePos";

		public static readonly StringName ProcessPortalAreas = "ProcessPortalAreas";

		public static readonly StringName ComputeGroundItemRect = "ComputeGroundItemRect";

		public static readonly StringName TryTeleport = "TryTeleport";

		public static readonly StringName TeleportProjectile = "TeleportProjectile";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName protalNode1 = "protalNode1";

		public static readonly StringName protalNode2 = "protalNode2";

		public static readonly StringName hitBox1 = "hitBox1";

		public static readonly StringName hitBox2 = "hitBox2";

		public static readonly StringName posRange = "posRange";

		public static readonly StringName protalSprite1 = "protalSprite1";

		public static readonly StringName protalSprite2 = "protalSprite2";

		public static readonly StringName gridPos1 = "gridPos1";

		public static readonly StringName gridPos2 = "gridPos2";

		public static readonly StringName shape = "shape";

		public static readonly StringName changeTime = "changeTime";

		public static readonly StringName changeTimer = "changeTimer";

		public static readonly StringName isChange = "isChange";

		public static readonly StringName exclude = "exclude";

		public static readonly StringName gridSize = "gridSize";

		public static readonly StringName _zonesRegistered = "_zonesRegistered";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private static PackedScene _portalCircle;

	private static PackedScene _portalSquare;

	private static PackedScene _portalRhombus;

	public Node2D protalNode1;

	public Node2D protalNode2;

	public AabbArea2D hitBox1;

	public AabbArea2D hitBox2;

	public Vector4I posRange;

	public AdobeAnimateSprite protalSprite1;

	public AdobeAnimateSprite protalSprite2;

	public Vector2I gridPos1;

	public Vector2I gridPos2;

	public string shape = "";

	public double changeTime;

	public double changeTimer;

	public bool isChange;

	public Array<TowerDefenseGroundItemBase> exclude = new Array<TowerDefenseGroundItemBase>();

	private readonly HashSet<TowerDefenseGroundItemBase> _portal1Overlaps = new HashSet<TowerDefenseGroundItemBase>();

	private readonly HashSet<TowerDefenseGroundItemBase> _portal2Overlaps = new HashSet<TowerDefenseGroundItemBase>();

	private readonly HashSet<TowerDefenseGroundItemBase> _portal1Scratch = new HashSet<TowerDefenseGroundItemBase>();

	private readonly HashSet<TowerDefenseGroundItemBase> _portal2Scratch = new HashSet<TowerDefenseGroundItemBase>();

	private readonly HashSet<int> _bulletExclude = new HashSet<int>();

	public Vector2 gridSize;

	private PortalZone _zone1;

	private PortalZone _zone2;

	private bool _zonesRegistered;

	private readonly TaskCompletionSource<bool> _exitWaiter = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

	private static PackedScene PortalCircle => _portalCircle ?? (_portalCircle = GD.Load<PackedScene>("uid://duxojv3j24ulx"));

	private static PackedScene PortalSquare => _portalSquare ?? (_portalSquare = GD.Load<PackedScene>("uid://c0h2t4iq8wbd"));

	private static PackedScene PortalRhombus => _portalRhombus ?? (_portalRhombus = GD.Load<PackedScene>("uid://du34u3ii3ujh7"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			protalNode1 = GetNode<Node2D>("%ProtalNode1");
			protalNode2 = GetNode<Node2D>("%ProtalNode2");
			hitBox1 = GetNode<AabbArea2D>("%HitBox1");
			hitBox2 = GetNode<AabbArea2D>("%HitBox2");
			AddToGroup("Portal");
			gridPos = new Vector2I(gridPos.X, 0);
			gridSize = TowerDefenseManager.Instance.GetMapGridSize();
			_zone1 = new PortalZone(this, isPortal1: true);
			_zone2 = new PortalZone(this, isPortal1: false);
		}
	}

	public override void _ExitTree()
	{
		_exitWaiter.TrySetResult(result: true);
		isChange = false;
		if (_zonesRegistered && GodotObject.IsInstanceValid(BulletField.Instance))
		{
			BulletField.Instance.UnregisterZone(_zone1);
			BulletField.Instance.UnregisterZone(_zone2);
			_zonesRegistered = false;
		}
		_bulletExclude.Clear();
		_portal1Overlaps.Clear();
		_portal2Overlaps.Clear();
		_portal1Scratch.Clear();
		_portal2Scratch.Clear();
		base._ExitTree();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_zonesRegistered && GodotObject.IsInstanceValid(BulletField.Instance))
		{
			BulletField.Instance.RegisterZone(_zone1);
			BulletField.Instance.RegisterZone(_zone2);
			_zonesRegistered = true;
		}
		RefreshProjectileExitGuards();
		ProcessPortalAreas();
		if (changeTime > 0.0 && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			if (changeTimer < changeTime)
			{
				changeTimer += delta;
				return;
			}
			changeTimer -= changeTime;
			ChangePos();
		}
	}

	private void RefreshProjectileExitGuards()
	{
		BulletField bulletField = (GodotObject.IsInstanceValid(BulletField.Instance) ? BulletField.Instance : null);
		if (_bulletExclude.Count > 0)
		{
			if (bulletField == null)
			{
				_bulletExclude.Clear();
			}
			else
			{
				_bulletExclude.RemoveWhere((int index) => !bulletField.IsBulletActive(index) || !IsBulletInsideEitherPortal(bulletField, index));
			}
		}
		for (int num = exclude.Count - 1; num >= 0; num--)
		{
			if (exclude[num] is TowerDefenseProjectile towerDefenseProjectile && (!GodotObject.IsInstanceValid(towerDefenseProjectile) || towerDefenseProjectile.over || towerDefenseProjectile.hitOver || !IsProjectileInsideEitherPortal(towerDefenseProjectile)))
			{
				exclude.RemoveAt(num);
			}
		}
	}

	private bool IsBulletInsideEitherPortal(BulletField bulletField, int index)
	{
		if (!TryGetPortalWorldRect(hitBox1, out var worldRect) || !bulletField.IsBulletIntersectingRect(index, worldRect))
		{
			if (TryGetPortalWorldRect(hitBox2, out var worldRect2))
			{
				return bulletField.IsBulletIntersectingRect(index, worldRect2);
			}
			return false;
		}
		return true;
	}

	private bool IsProjectileInsideEitherPortal(TowerDefenseProjectile projectile)
	{
		Rect2 worldHitRect = projectile.WorldHitRect;
		if (!TryGetPortalWorldRect(hitBox1, out var worldRect) || !AabbShapeUtil.Intersects(worldHitRect, worldRect))
		{
			if (TryGetPortalWorldRect(hitBox2, out var worldRect2))
			{
				return AabbShapeUtil.Intersects(worldHitRect, worldRect2);
			}
			return false;
		}
		return true;
	}

	private static bool TryGetPortalWorldRect(AabbArea2D hitBox, out Rect2 worldRect)
	{
		worldRect = default;
		if (GodotObject.IsInstanceValid(hitBox))
		{
			return AabbShapeUtil.TryComputeAreaWorldRect(hitBox, out worldRect);
		}
		return false;
	}

	public void Init(string _shape, Vector4I _posRange, double _changeTime = 0.0)
	{
		posRange = _posRange;
		changeTime = Mathf.Max(0.0, _changeTime);
		shape = _shape;
		switch (shape)
		{
		case "Circle":
			protalSprite1 = PortalCircle.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			protalSprite2 = PortalCircle.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			break;
		case "Square":
			protalSprite1 = PortalSquare.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			protalSprite2 = PortalSquare.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			break;
		case "Rhombus":
			protalSprite1 = PortalRhombus.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			protalSprite2 = PortalRhombus.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			break;
		}
		protalSprite1.SetAnimation("Appear", loop: false, 0.2);
		protalSprite1.AddAnimation("Pulse", 0.0, loop: true, 0.2);
		protalNode1.AddChild(protalSprite1, forceReadableName: false, InternalMode.Disabled);
		protalSprite2.SetAnimation("Appear", loop: false, 0.2);
		protalSprite2.AddAnimation("Pulse", 0.0, loop: true, 0.2);
		protalNode2.AddChild(protalSprite2, forceReadableName: false, InternalMode.Disabled);
		ChangePos(isInit: true);
	}

	public void ChangePos(bool isInit = false)
	{
		if (!isChange && IsInsideTree() && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			ObserveChangePosAsync(isInit);
		}
	}

	private async Task ObserveChangePosAsync(bool isInit)
	{
		try
		{
			await ChangePosAsync(isInit);
		}
		catch (Exception value)
		{
			GD.PushError($"[Portal] ChangePos failed: {value}");
		}
		finally
		{
			isChange = false;
		}
	}

	private async Task ChangePosAsync(bool isInit)
	{
		if (isChange || !IsInsideTree())
		{
			return;
		}
		AudioManager.Instance.AudioPlay("Portal");
		isChange = true;
		TaskCompletionSource<bool> tcs;
		if (!isInit)
		{
			protalSprite1.SetAnimation("Dissappar");
			protalSprite2.SetAnimation("Dissappar");
			tcs = new TaskCompletionSource<bool>();
			protalSprite1.OnAnimeCompleted += Handler;
			Task task;
			try
			{
				task = await Task.WhenAny(tcs.Task, _exitWaiter.Task);
			}
			finally
			{
				if (GodotObject.IsInstanceValid(protalSprite1))
				{
					protalSprite1.OnAnimeCompleted -= Handler;
				}
			}
			if (task != tcs.Task || !IsInsideTree())
			{
				return;
			}
		}
		HashSet<Vector2I> hashSet = new HashSet<Vector2I>();
		foreach (Node item2 in GetTree().GetNodesInGroup("Portal"))
		{
			TowerDefensePortal towerDefensePortal = item2 as TowerDefensePortal;
			if (GodotObject.IsInstanceValid(towerDefensePortal) && towerDefensePortal != this)
			{
				hashSet.Add(towerDefensePortal.gridPos1);
				hashSet.Add(towerDefensePortal.gridPos2);
			}
		}
		List<Vector2I> list = new List<Vector2I>();
		int num = Mathf.Min(posRange.X, posRange.Z);
		int num2 = Mathf.Max(posRange.X, posRange.Z);
		int num3 = Mathf.Min(posRange.Y, posRange.W);
		int num4 = Mathf.Max(posRange.Y, posRange.W);
		for (int i = num; i <= num2; i++)
		{
			for (int j = num3; j <= num4; j++)
			{
				Vector2I item = new Vector2I(i, j);
				if (!hashSet.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		if (list.Count < 2)
		{
			GD.PushWarning($"[Portal] Position range {posRange} has fewer than two free cells.");
			return;
		}
		int index = (int)(GD.Randi() % (uint)list.Count);
		gridPos1 = list[index];
		list.RemoveAt(index);
		gridPos2 = list[(int)(GD.Randi() % (uint)list.Count)];
		protalNode1.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos1) + new Vector2(gridSize.X / 2f, 0f);
		protalNode2.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos2) + new Vector2(gridSize.X / 2f, 0f);
		protalSprite1.SetAnimation("Appear", loop: false, 0.2);
		protalSprite1.AddAnimation("Pulse", 0.0, loop: true, 0.2);
		protalSprite2.SetAnimation("Appear", loop: false, 0.2);
		protalSprite2.AddAnimation("Pulse", 0.0, loop: true, 0.2);
		protalSprite1.ZIndex = gridPos1.Y * 15;
		protalSprite2.ZIndex = gridPos2.Y * 15;
		void Handler(string clip)
		{
			tcs.TrySetResult(result: true);
		}
	}

	private void ProcessPortalAreas()
	{
		bool flag = GodotObject.IsInstanceValid(hitBox1) && hitBox1.ProcessMode != ProcessModeEnum.Disabled;
		bool flag2 = GodotObject.IsInstanceValid(hitBox2) && hitBox2.ProcessMode != ProcessModeEnum.Disabled;
		if (!flag)
		{
			_portal1Overlaps.Clear();
		}
		if (!flag2)
		{
			_portal2Overlaps.Clear();
		}
		if (!flag && !flag2)
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		Rect2 a = (flag ? AabbShapeUtil.ComputeAreaWorldRect(hitBox1) : default(Rect2));
		Rect2 a2 = (flag2 ? AabbShapeUtil.ComputeAreaWorldRect(hitBox2) : default(Rect2));
		_portal1Scratch.Clear();
		_portal2Scratch.Clear();
		foreach (Node child in node2D.GetChildren())
		{
			if (!(child is TowerDefenseGroundItemBase towerDefenseGroundItemBase) || towerDefenseGroundItemBase == this || !GodotObject.IsInstanceValid(towerDefenseGroundItemBase) || towerDefenseGroundItemBase is TowerDefenseProjectile)
			{
				continue;
			}
			Rect2 b = ComputeGroundItemRect(towerDefenseGroundItemBase);
			if (flag && AabbShapeUtil.Intersects(a, b))
			{
				_portal1Scratch.Add(towerDefenseGroundItemBase);
				if (!_portal1Overlaps.Contains(towerDefenseGroundItemBase))
				{
					TryTeleport(towerDefenseGroundItemBase, fromPortal1: true);
				}
				b = ComputeGroundItemRect(towerDefenseGroundItemBase);
			}
			if (flag2 && AabbShapeUtil.Intersects(a2, b))
			{
				_portal2Scratch.Add(towerDefenseGroundItemBase);
				if (!_portal2Overlaps.Contains(towerDefenseGroundItemBase))
				{
					TryTeleport(towerDefenseGroundItemBase, fromPortal1: false);
				}
			}
		}
		UpdateOverlaps(_portal1Overlaps, _portal1Scratch, flag);
		UpdateOverlaps(_portal2Overlaps, _portal2Scratch, flag2);
	}

	private static void UpdateOverlaps(HashSet<TowerDefenseGroundItemBase> currentOverlaps, HashSet<TowerDefenseGroundItemBase> scratch, bool active)
	{
		if (!active)
		{
			currentOverlaps.Clear();
			return;
		}
		currentOverlaps.RemoveWhere((TowerDefenseGroundItemBase item) => !GodotObject.IsInstanceValid(item) || !scratch.Contains(item));
		currentOverlaps.UnionWith(scratch);
	}

	private Rect2 ComputeGroundItemRect(TowerDefenseGroundItemBase groundItem)
	{
		if (groundItem is TowerDefenseCharacter towerDefenseCharacter)
		{
			return towerDefenseCharacter.WorldHitRect;
		}
		if (groundItem is TowerDefenseProjectile towerDefenseProjectile && GodotObject.IsInstanceValid(towerDefenseProjectile.hitBox))
		{
			return AabbShapeUtil.ComputeAreaWorldRect(towerDefenseProjectile.hitBox);
		}
		return AabbShapeUtil.RectFromCenter(groundItem.GlobalPosition, gridSize);
	}

	private void TryTeleport(TowerDefenseGroundItemBase groundItem, bool fromPortal1)
	{
		Vector2I vector2I = (fromPortal1 ? gridPos1 : gridPos2);
		Vector2I vector2I2 = (fromPortal1 ? gridPos2 : gridPos1);
		Node2D node2D = (fromPortal1 ? protalNode2 : protalNode1);
		if (groundItem.gridPos.Y != vector2I.Y)
		{
			return;
		}
		if (exclude.Contains(groundItem))
		{
			exclude.Remove(groundItem);
		}
		else if ((!(groundItem is TowerDefensePlant) || groundItem is TowerDefensePlantBowlingBase) && (!(groundItem is TowerDefenseItem) || groundItem is TowerDefenseMower) && !(groundItem is TowerDefenseCrater) && !(groundItem is TowerDefenseGravestone))
		{
			if (groundItem is TowerDefenseCharacter towerDefenseCharacter)
			{
				Vector2 logicalGlobalPosition = towerDefenseCharacter.GetLogicalGlobalPosition();
				towerDefenseCharacter.shadowComponent.saveShadowPosition = new Vector2(towerDefenseCharacter.shadowComponent.saveShadowPosition.X, towerDefenseCharacter.shadowComponent.saveShadowPosition.Y + (node2D.GlobalPosition.Y - logicalGlobalPosition.Y));
				towerDefenseCharacter.SetLogicalGlobalPosition(node2D.GlobalPosition);
			}
			else if (groundItem is TowerDefenseProjectile towerDefenseProjectile)
			{
				towerDefenseProjectile.GlobalPosition = new Vector2(node2D.GlobalPosition.X, towerDefenseProjectile.GlobalPosition.Y + gridSize.Y * (float)(vector2I2.Y - towerDefenseProjectile.gridPos.Y));
			}
			else
			{
				groundItem.GlobalPosition = node2D.GlobalPosition;
			}
			groundItem.gridPos = vector2I2;
			exclude.Add(groundItem);
		}
	}

	public void TeleportBulletData(ref BulletData b, int index, bool fromPortal1)
	{
		Vector2I vector2I = (fromPortal1 ? gridPos1 : gridPos2);
		Vector2I vector2I2 = (fromPortal1 ? gridPos2 : gridPos1);
		Node2D node2D = (fromPortal1 ? protalNode2 : protalNode1);
		if (b.gridY == vector2I.Y && !_bulletExclude.Contains(index))
		{
			Vector2 newPos = new Vector2(node2D.GlobalPosition.X, b.pos.Y + gridSize.Y * (float)(vector2I2.Y - b.gridY));
			BulletField.Instance.TeleportBullet(index, newPos, vector2I2.Y);
			_bulletExclude.Add(index);
		}
	}

	public void TeleportProjectile(TowerDefenseProjectile projectile, bool fromPortal1)
	{
		if (GodotObject.IsInstanceValid(projectile) && !projectile.over && !projectile.hitOver)
		{
			Vector2I vector2I = (fromPortal1 ? gridPos1 : gridPos2);
			Vector2I vector2I2 = (fromPortal1 ? gridPos2 : gridPos1);
			Node2D node2D = (fromPortal1 ? protalNode2 : protalNode1);
			if (projectile.gridPos.Y == vector2I.Y && !exclude.Contains(projectile))
			{
				projectile.GlobalPosition = new Vector2(node2D.GlobalPosition.X, projectile.GlobalPosition.Y + gridSize.Y * (float)(vector2I2.Y - projectile.gridPos.Y));
				projectile.gridPos = vector2I2;
				exclude.Add(projectile);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshProjectileExitGuards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsBulletInsideEitherPortal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bulletField", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsProjectileInsideEitherPortal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_shape", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "_posRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_changeTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangePos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isInit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessPortalAreas, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeGroundItemRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "groundItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryTeleport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "groundItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "fromPortal1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TeleportProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "fromPortal1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProjectileExitGuards && args.Count == 0)
		{
			RefreshProjectileExitGuards();
			ret = default;
			return true;
		}
		if (method == MethodName.IsBulletInsideEitherPortal && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBulletInsideEitherPortal(VariantUtils.ConvertTo<BulletField>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.IsProjectileInsideEitherPortal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProjectileInsideEitherPortal(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0])));
			return true;
		}
		if (method == MethodName.Init && args.Count == 3)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangePos && args.Count == 1)
		{
			ChangePos(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessPortalAreas && args.Count == 0)
		{
			ProcessPortalAreas();
			ret = default;
			return true;
		}
		if (method == MethodName.ComputeGroundItemRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(ComputeGroundItemRect(VariantUtils.ConvertTo<TowerDefenseGroundItemBase>(in args[0])));
			return true;
		}
		if (method == MethodName.TryTeleport && args.Count == 2)
		{
			TryTeleport(VariantUtils.ConvertTo<TowerDefenseGroundItemBase>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportProjectile && args.Count == 2)
		{
			TeleportProjectile(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.RefreshProjectileExitGuards)
		{
			return true;
		}
		if (method == MethodName.IsBulletInsideEitherPortal)
		{
			return true;
		}
		if (method == MethodName.IsProjectileInsideEitherPortal)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ChangePos)
		{
			return true;
		}
		if (method == MethodName.ProcessPortalAreas)
		{
			return true;
		}
		if (method == MethodName.ComputeGroundItemRect)
		{
			return true;
		}
		if (method == MethodName.TryTeleport)
		{
			return true;
		}
		if (method == MethodName.TeleportProjectile)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.protalNode1)
		{
			protalNode1 = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.protalNode2)
		{
			protalNode2 = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.hitBox1)
		{
			hitBox1 = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.hitBox2)
		{
			hitBox2 = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.posRange)
		{
			posRange = VariantUtils.ConvertTo<Vector4I>(in value);
			return true;
		}
		if (name == PropertyName.protalSprite1)
		{
			protalSprite1 = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.protalSprite2)
		{
			protalSprite2 = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.gridPos1)
		{
			gridPos1 = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.gridPos2)
		{
			gridPos2 = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.shape)
		{
			shape = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.changeTime)
		{
			changeTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.changeTimer)
		{
			changeTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isChange)
		{
			isChange = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.exclude)
		{
			exclude = VariantUtils.ConvertToArray<TowerDefenseGroundItemBase>(in value);
			return true;
		}
		if (name == PropertyName.gridSize)
		{
			gridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._zonesRegistered)
		{
			_zonesRegistered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.protalNode1)
		{
			value = VariantUtils.CreateFrom(in protalNode1);
			return true;
		}
		if (name == PropertyName.protalNode2)
		{
			value = VariantUtils.CreateFrom(in protalNode2);
			return true;
		}
		if (name == PropertyName.hitBox1)
		{
			value = VariantUtils.CreateFrom(in hitBox1);
			return true;
		}
		if (name == PropertyName.hitBox2)
		{
			value = VariantUtils.CreateFrom(in hitBox2);
			return true;
		}
		if (name == PropertyName.posRange)
		{
			value = VariantUtils.CreateFrom(in posRange);
			return true;
		}
		if (name == PropertyName.protalSprite1)
		{
			value = VariantUtils.CreateFrom(in protalSprite1);
			return true;
		}
		if (name == PropertyName.protalSprite2)
		{
			value = VariantUtils.CreateFrom(in protalSprite2);
			return true;
		}
		if (name == PropertyName.gridPos1)
		{
			value = VariantUtils.CreateFrom(in gridPos1);
			return true;
		}
		if (name == PropertyName.gridPos2)
		{
			value = VariantUtils.CreateFrom(in gridPos2);
			return true;
		}
		if (name == PropertyName.shape)
		{
			value = VariantUtils.CreateFrom(in shape);
			return true;
		}
		if (name == PropertyName.changeTime)
		{
			value = VariantUtils.CreateFrom(in changeTime);
			return true;
		}
		if (name == PropertyName.changeTimer)
		{
			value = VariantUtils.CreateFrom(in changeTimer);
			return true;
		}
		if (name == PropertyName.isChange)
		{
			value = VariantUtils.CreateFrom(in isChange);
			return true;
		}
		if (name == PropertyName.exclude)
		{
			value = VariantUtils.CreateFromArray(exclude);
			return true;
		}
		if (name == PropertyName.gridSize)
		{
			value = VariantUtils.CreateFrom(in gridSize);
			return true;
		}
		if (name == PropertyName._zonesRegistered)
		{
			value = VariantUtils.CreateFrom(in _zonesRegistered);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.protalNode1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.protalNode2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hitBox1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hitBox2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector4I, PropertyName.posRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.protalSprite1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.protalSprite2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.shape, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.changeTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.changeTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isChange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.exclude, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.gridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zonesRegistered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.protalNode1, Variant.From(in protalNode1));
		info.AddProperty(PropertyName.protalNode2, Variant.From(in protalNode2));
		info.AddProperty(PropertyName.hitBox1, Variant.From(in hitBox1));
		info.AddProperty(PropertyName.hitBox2, Variant.From(in hitBox2));
		info.AddProperty(PropertyName.posRange, Variant.From(in posRange));
		info.AddProperty(PropertyName.protalSprite1, Variant.From(in protalSprite1));
		info.AddProperty(PropertyName.protalSprite2, Variant.From(in protalSprite2));
		info.AddProperty(PropertyName.gridPos1, Variant.From(in gridPos1));
		info.AddProperty(PropertyName.gridPos2, Variant.From(in gridPos2));
		info.AddProperty(PropertyName.shape, Variant.From(in shape));
		info.AddProperty(PropertyName.changeTime, Variant.From(in changeTime));
		info.AddProperty(PropertyName.changeTimer, Variant.From(in changeTimer));
		info.AddProperty(PropertyName.isChange, Variant.From(in isChange));
		info.AddProperty(PropertyName.exclude, Variant.CreateFrom(exclude));
		info.AddProperty(PropertyName.gridSize, Variant.From(in gridSize));
		info.AddProperty(PropertyName._zonesRegistered, Variant.From(in _zonesRegistered));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.protalNode1, out var value))
		{
			protalNode1 = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.protalNode2, out var value2))
		{
			protalNode2 = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.hitBox1, out var value3))
		{
			hitBox1 = value3.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.hitBox2, out var value4))
		{
			hitBox2 = value4.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.posRange, out var value5))
		{
			posRange = value5.As<Vector4I>();
		}
		if (info.TryGetProperty(PropertyName.protalSprite1, out var value6))
		{
			protalSprite1 = value6.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.protalSprite2, out var value7))
		{
			protalSprite2 = value7.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.gridPos1, out var value8))
		{
			gridPos1 = value8.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.gridPos2, out var value9))
		{
			gridPos2 = value9.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.shape, out var value10))
		{
			shape = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.changeTime, out var value11))
		{
			changeTime = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.changeTimer, out var value12))
		{
			changeTimer = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isChange, out var value13))
		{
			isChange = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.exclude, out var value14))
		{
			exclude = value14.AsGodotArray<TowerDefenseGroundItemBase>();
		}
		if (info.TryGetProperty(PropertyName.gridSize, out var value15))
		{
			gridSize = value15.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._zonesRegistered, out var value16))
		{
			_zonesRegistered = value16.As<bool>();
		}
	}
}
