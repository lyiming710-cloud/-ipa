using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/BossHealthBar/TowerDefenseBossHealthBarManager.cs")]
public class TowerDefenseBossHealthBarManager : CanvasLayer
{
	private sealed class BossEntry
	{
		public TowerDefenseZombie Boss;

		public ulong Sequence;

		public TowerDefenseBossHealthBarView View;

		public Action HitpointsEmptyHandler;

		public bool Dead;

		public bool Retired;

		public double DeathElapsed;
	}

	public new class MethodName : CanvasLayer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName SubscribeEvents = "SubscribeEvents";

		public static readonly StringName UnsubscribeEvents = "UnsubscribeEvents";

		public static readonly StringName OnShowBossHealthBar = "OnShowBossHealthBar";

		public static readonly StringName OnUiSwitched = "OnUiSwitched";

		public static readonly StringName OnScreenTransformChanged = "OnScreenTransformChanged";

		public static readonly StringName QueueLayout = "QueueLayout";

		public static readonly StringName ApplyQueuedLayout = "ApplyQueuedLayout";

		public static readonly StringName ReconcileBosses = "ReconcileBosses";

		public static readonly StringName TrackBoss = "TrackBoss";

		public static readonly StringName AssignViewsAndLayout = "AssignViewsAndLayout";

		public static readonly StringName LayoutView = "LayoutView";

		public static readonly StringName GetTopPropRect = "GetTopPropRect";

		public static readonly StringName IsBattleRunning = "IsBattleRunning";

		public static readonly StringName ClearTrackedBosses = "ClearTrackedBosses";

		public static readonly StringName IsBossCharacter = "IsBossCharacter";

		public static readonly StringName IsBossDead = "IsBossDead";
	}

	public new class PropertyName : CanvasLayer.PropertyName
	{
		public static readonly StringName VisibleBarCount = "VisibleBarCount";

		public static readonly StringName TrackedBossCount = "TrackedBossCount";

		public static readonly StringName _nextSequence = "_nextSequence";

		public static readonly StringName _lastMembershipRevision = "_lastMembershipRevision";

		public static readonly StringName _mobilePreset = "_mobilePreset";

		public static readonly StringName _showBossHealthBar = "_showBossHealthBar";

		public static readonly StringName _battleRunning = "_battleRunning";

		public static readonly StringName _viewsDirty = "_viewsDirty";

		public static readonly StringName _layoutQueued = "_layoutQueued";

		public static readonly StringName _hasPhysicalMousePosition = "_hasPhysicalMousePosition";

		public static readonly StringName _physicalMousePosition = "_physicalMousePosition";

		public static readonly StringName _eventBus = "_eventBus";
	}

	public new class SignalName : CanvasLayer.SignalName
	{
	}

	private const int MaxVisibleBosses = 3;

	private const float DesktopBarScale = 0.5f;

	private const float DesktopLeft = 12f;

	private const float DesktopTop = 88f;

	private const float DesktopGap = 4f;

	private const float MobileMaxBarScale = 0.5f;

	private const float MobileMinBarScale = 0.3f;

	private const float MobilePreferredFallbackX = 480f;

	private const float MobileMinimumX = 204f;

	private const float MobileTop = 12f;

	private const float MobileFallbackTop = 78f;

	private const float MobileBarGap = 3f;

	private const float MobileFallbackPropGap = 6f;

	private const float MobilePropGap = 8f;

	private const float MobileRightReserved = 118f;

	public const double DeathHoldSeconds = 1.5;

	public const double DeathFadeSeconds = 0.25;

	private static readonly Color ZombieBossColor = Color.Color8(152, 205, 69, 255);

	private static readonly Color ZombieBossDaveColor = Color.Color8(145, 94, 224, 255);

	private static readonly Color ZombieBossEdgarColor = Color.Color8(171, 13, 13, 255);

	private static PackedScene _viewScene;

	private static Texture2D _zombieBossIcon;

	private static Texture2D _zombieBossDaveIcon;

	private static Texture2D _zombieBossEdgarIcon;

	private readonly System.Collections.Generic.Dictionary<TowerDefenseZombie, BossEntry> _entries = new System.Collections.Generic.Dictionary<TowerDefenseZombie, BossEntry>();

	private readonly List<BossEntry> _orderedEntries = new List<BossEntry>();

	private readonly System.Collections.Generic.Dictionary<int, Vector2> _touchPositions = new System.Collections.Generic.Dictionary<int, Vector2>();

	private readonly List<BossEntry> _scratchEntries = new List<BossEntry>();

	private ulong _nextSequence;

	private ulong _lastMembershipRevision = 18446744073709551615uL;

	private bool _mobilePreset;

	private bool _showBossHealthBar;

	private bool _battleRunning;

	private bool _viewsDirty;

	private bool _layoutQueued;

	private bool _hasPhysicalMousePosition;

	private Vector2 _physicalMousePosition;

	private BattleEventBus _eventBus;

	private static PackedScene ViewScene => _viewScene ?? (_viewScene = GD.Load<PackedScene>("res://Prefab/GUI/BossHealthBar/TowerDefenseBossHealthBarView.tscn"));

	private static Texture2D ZombieBossIcon => _zombieBossIcon ?? (_zombieBossIcon = GD.Load<Texture2D>("res://Asset/Texture/GUI/TowerDefense/BossHealthBar/BossHealthBarZombieBoss.png"));

	private static Texture2D ZombieBossDaveIcon => _zombieBossDaveIcon ?? (_zombieBossDaveIcon = GD.Load<Texture2D>("res://Asset/Texture/GUI/TowerDefense/BossHealthBar/BossHealthBarZombieBossDave.png"));

	private static Texture2D ZombieBossEdgarIcon => _zombieBossEdgarIcon ?? (_zombieBossEdgarIcon = GD.Load<Texture2D>("res://Asset/Texture/GUI/TowerDefense/BossHealthBar/BossHealthBarZombieBossEdgarII.png"));

	public int VisibleBarCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < _orderedEntries.Count; i++)
			{
				if (GodotObject.IsInstanceValid(_orderedEntries[i].View))
				{
					num++;
				}
			}
			return num;
		}
	}

	public int TrackedBossCount => _entries.Count;

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			_showBossHealthBar = GameSaveManager.Instance.GetConfigValue("ShowBossHealthBar").AsBool();
			_mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
			_battleRunning = IsBattleRunning();
			Visible = false;
			_hasPhysicalMousePosition = false;
			SubscribeEvents();
			if (_battleRunning)
			{
				ReconcileBosses(force: true);
				Visible = _showBossHealthBar;
			}
		}
	}

	public override void _ExitTree()
	{
		UnsubscribeEvents();
		ClearTrackedBosses();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!(inputEvent is InputEventScreenTouch inputEventScreenTouch))
		{
			if (!(inputEvent is InputEventScreenDrag inputEventScreenDrag))
			{
				if (!(inputEvent is InputEventMouseMotion inputEventMouseMotion))
				{
					if (inputEvent is InputEventMouseButton inputEventMouseButton && (long)inputEventMouseButton.Device != -1)
					{
						_physicalMousePosition = inputEventMouseButton.Position;
						_hasPhysicalMousePosition = true;
					}
				}
				else if ((long)inputEventMouseMotion.Device != -1)
				{
					_physicalMousePosition = inputEventMouseMotion.Position;
					_hasPhysicalMousePosition = true;
				}
			}
			else
			{
				if ((long)inputEventScreenDrag.Device != -1)
				{
					_hasPhysicalMousePosition = false;
				}
				_touchPositions[inputEventScreenDrag.Index] = inputEventScreenDrag.Position;
			}
		}
		else
		{
			if ((long)inputEventScreenTouch.Device != -1)
			{
				_hasPhysicalMousePosition = false;
			}
			if (inputEventScreenTouch.Pressed)
			{
				_touchPositions[inputEventScreenTouch.Index] = inputEventScreenTouch.Position;
			}
			else
			{
				_touchPositions.Remove(inputEventScreenTouch.Index);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		bool flag = IsBattleRunning();
		if (flag != _battleRunning)
		{
			_battleRunning = flag;
			if (!_battleRunning)
			{
				Visible = false;
				ClearTrackedBosses();
				return;
			}
			ReconcileBosses(force: true);
			Visible = _showBossHealthBar;
		}
		if (!_battleRunning)
		{
			return;
		}
		ReconcileBosses(force: false);
		bool flag2 = false;
		for (int num = _orderedEntries.Count - 1; num >= 0; num--)
		{
			BossEntry bossEntry = _orderedEntries[num];
			if (!bossEntry.Retired)
			{
				if (!bossEntry.Dead && IsBossDead(bossEntry.Boss))
				{
					MarkDead(bossEntry);
				}
				double bodyRatio;
				double shieldRatio;
				if (bossEntry.Dead)
				{
					bossEntry.DeathElapsed += delta;
					if (GodotObject.IsInstanceValid(bossEntry.View))
					{
						float lifecycleAlpha = ((bossEntry.DeathElapsed <= 1.5) ? 1f : (1f - (float)((bossEntry.DeathElapsed - 1.5) / 0.25)));
						bossEntry.View.SetLifecycleAlpha(lifecycleAlpha);
					}
					if (bossEntry.DeathElapsed >= 1.75)
					{
						RetireEntry(bossEntry);
						flag2 = true;
					}
				}
				else if (GodotObject.IsInstanceValid(bossEntry.View) && TryGetBossRatios(bossEntry.Boss, out bodyRatio, out shieldRatio))
				{
					bossEntry.View.SetRatios(bodyRatio, shieldRatio);
				}
			}
		}
		if (flag2 || _viewsDirty)
		{
			_viewsDirty = false;
			AssignViewsAndLayout();
		}
	}

	public override void _Process(double delta)
	{
		if (!Visible)
		{
			return;
		}
		for (int i = 0; i < _orderedEntries.Count; i++)
		{
			TowerDefenseBossHealthBarView view = _orderedEntries[i].View;
			if (!GodotObject.IsInstanceValid(view))
			{
				continue;
			}
			bool flag = _hasPhysicalMousePosition && view.ContainsCanvasPoint(_physicalMousePosition);
			if (!flag)
			{
				foreach (Vector2 value in _touchPositions.Values)
				{
					if (view.ContainsCanvasPoint(value))
					{
						flag = true;
						break;
					}
				}
			}
			view.SetPointerOverlap(flag);
		}
	}

	private void SubscribeEvents()
	{
		_eventBus = BattleEventBus.Instance;
		if (GodotObject.IsInstanceValid(_eventBus))
		{
			_eventBus.OnShowBossHealthBar += OnShowBossHealthBar;
			_eventBus.OnUiSwitched += OnUiSwitched;
			_eventBus.OnScreenTransformChanged += OnScreenTransformChanged;
		}
	}

	private void UnsubscribeEvents()
	{
		if (GodotObject.IsInstanceValid(_eventBus))
		{
			_eventBus.OnShowBossHealthBar -= OnShowBossHealthBar;
			_eventBus.OnUiSwitched -= OnUiSwitched;
			_eventBus.OnScreenTransformChanged -= OnScreenTransformChanged;
		}
		_eventBus = null;
	}

	private void OnShowBossHealthBar(bool show)
	{
		_showBossHealthBar = show;
		Visible = show && _battleRunning;
		if (Visible)
		{
			return;
		}
		for (int i = 0; i < _orderedEntries.Count; i++)
		{
			if (GodotObject.IsInstanceValid(_orderedEntries[i].View))
			{
				_orderedEntries[i].View.SetPointerOverlap(overlaps: false);
			}
		}
	}

	private void OnUiSwitched(bool mobilePreset)
	{
		_mobilePreset = mobilePreset;
		QueueLayout();
	}

	private void OnScreenTransformChanged()
	{
		QueueLayout();
	}

	private void QueueLayout()
	{
		if (!_layoutQueued)
		{
			_layoutQueued = true;
			Callable.From(ApplyQueuedLayout).CallDeferred();
		}
	}

	private void ApplyQueuedLayout()
	{
		_layoutQueued = false;
		if (IsInsideTree() && _battleRunning)
		{
			AssignViewsAndLayout();
			_viewsDirty = false;
		}
	}

	private void ReconcileBosses(bool force)
	{
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (!GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry) || (!force && towerDefenseBattleCharacterRegistry.MembershipRevision == _lastMembershipRevision))
		{
			return;
		}
		_lastMembershipRevision = towerDefenseBattleCharacterRegistry.MembershipRevision;
		HashSet<TowerDefenseZombie> hashSet = new HashSet<TowerDefenseZombie>();
		List<TowerDefenseCharacter> activeCharacters = towerDefenseBattleCharacterRegistry.GetActiveCharacters();
		for (int i = 0; i < activeCharacters.Count; i++)
		{
			if (activeCharacters[i] is TowerDefenseZombie towerDefenseZombie && IsBossCharacter(towerDefenseZombie))
			{
				hashSet.Add(towerDefenseZombie);
				if (!_entries.ContainsKey(towerDefenseZombie))
				{
					TrackBoss(towerDefenseZombie);
				}
			}
		}
		_scratchEntries.Clear();
		_scratchEntries.AddRange(_entries.Values);
		for (int j = 0; j < _scratchEntries.Count; j++)
		{
			BossEntry bossEntry = _scratchEntries[j];
			if (!hashSet.Contains(bossEntry.Boss))
			{
				_entries.Remove(bossEntry.Boss);
				DetachHitpointsHandler(bossEntry);
				if (bossEntry.Dead || IsBossDead(bossEntry.Boss))
				{
					MarkDead(bossEntry);
				}
				else
				{
					RemoveEntry(bossEntry);
				}
			}
		}
		AssignViewsAndLayout();
		_viewsDirty = false;
	}

	private void TrackBoss(TowerDefenseZombie boss)
	{
		BossEntry entry = new BossEntry
		{
			Boss = boss,
			Sequence = _nextSequence++,
			Dead = IsBossDead(boss)
		};
		entry.HitpointsEmptyHandler = () =>
		{
			MarkDead(entry);
		};
		boss.instance.hitpointsEmpty += entry.HitpointsEmptyHandler;
		_entries[boss] = entry;
		_orderedEntries.Add(entry);
	}

	private void AssignViewsAndLayout()
	{
		if (!_battleRunning)
		{
			return;
		}
		_orderedEntries.Sort((BossEntry left, BossEntry right) => left.Sequence.CompareTo(right.Sequence));
		int num = 0;
		for (int num2 = 0; num2 < _orderedEntries.Count; num2++)
		{
			BossEntry bossEntry = _orderedEntries[num2];
			if (!bossEntry.Retired)
			{
				if (num >= 3)
				{
					ReleaseView(bossEntry);
					continue;
				}
				EnsureView(bossEntry);
				LayoutView(bossEntry.View, num);
				num++;
			}
		}
	}

	private void EnsureView(BossEntry entry)
	{
		if (!GodotObject.IsInstanceValid(entry.View))
		{
			entry.View = ViewScene.Instantiate<TowerDefenseBossHealthBarView>(PackedScene.GenEditState.Disabled);
			AddChild(entry.View, forceReadableName: false, InternalMode.Disabled);
			ResolveStyle(entry.Boss, out var icon, out var color);
			entry.View.Configure(icon, color);
			entry.View.SetDead(entry.Dead);
			entry.View.SetLifecycleAlpha(1f);
			if (!entry.Dead && TryGetBossRatios(entry.Boss, out var bodyRatio, out var shieldRatio))
			{
				entry.View.SetRatios(bodyRatio, shieldRatio);
			}
		}
	}

	private void LayoutView(TowerDefenseBossHealthBarView view, int displayIndex)
	{
		if (!GodotObject.IsInstanceValid(view))
		{
			return;
		}
		if (!_mobilePreset)
		{
			view.Scale = Vector2.One * 0.5f;
			view.SetVertical(vertical: true);
			float num = 58.5f;
			view.Position = new Vector2(12f + (float)displayIndex * num, 88f);
			return;
		}
		view.SetVertical(vertical: false);
		Vector2 size = GetViewport().GetVisibleRect().Size;
		Rect2 topPropRect = GetTopPropRect();
		float num2 = ((topPropRect.Size.X > 0f) ? (topPropRect.End.X + 8f) : 480f);
		float num3 = size.X - 118f;
		float num4 = (num3 - num2) / 575f;
		float num5;
		float x;
		float num6;
		if (num4 >= 0.3f)
		{
			num5 = Mathf.Min(0.5f, num4);
			x = num2;
			num6 = 12f;
		}
		else
		{
			num5 = 0.3f;
			x = Mathf.Max(204f, num3 - 575f * num5);
			num6 = Mathf.Max(78f, topPropRect.End.Y + 6f);
		}
		view.Scale = Vector2.One * num5;
		float num7 = 109f * num5 + 3f;
		view.Position = new Vector2(x, num6 + (float)displayIndex * num7);
	}

	private static Rect2 GetTopPropRect()
	{
		HBoxContainer hBoxContainer = TowerDefenseManager.CurrentControl?.uITopPropContainer;
		if (!GodotObject.IsInstanceValid(hBoxContainer) || !hBoxContainer.IsInsideTree())
		{
			return default;
		}
		return hBoxContainer.GetGlobalRect();
	}

	private static bool IsBattleRunning()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			return instance.IsGameRunning();
		}
		return false;
	}

	private void ClearTrackedBosses()
	{
		_scratchEntries.Clear();
		_scratchEntries.AddRange(_orderedEntries);
		foreach (BossEntry value in _entries.Values)
		{
			if (!_scratchEntries.Contains(value))
			{
				_scratchEntries.Add(value);
			}
		}
		for (int i = 0; i < _scratchEntries.Count; i++)
		{
			BossEntry entry = _scratchEntries[i];
			DetachHitpointsHandler(entry);
			ReleaseView(entry);
		}
		_entries.Clear();
		_orderedEntries.Clear();
		_touchPositions.Clear();
		_scratchEntries.Clear();
		_nextSequence = 0uL;
		_lastMembershipRevision = 18446744073709551615uL;
		_viewsDirty = false;
	}

	private void MarkDead(BossEntry entry)
	{
		if (entry != null && !entry.Dead && !entry.Retired)
		{
			entry.Dead = true;
			entry.DeathElapsed = 0.0;
			if (GodotObject.IsInstanceValid(entry.View))
			{
				entry.View.SetDead(dead: true);
				return;
			}
			RetireEntry(entry);
			_viewsDirty = true;
		}
	}

	private void RetireEntry(BossEntry entry)
	{
		entry.Retired = true;
		_orderedEntries.Remove(entry);
		DetachHitpointsHandler(entry);
		ReleaseView(entry);
	}

	private void RemoveEntry(BossEntry entry)
	{
		if (entry != null)
		{
			_entries.Remove(entry.Boss);
			_orderedEntries.Remove(entry);
			DetachHitpointsHandler(entry);
			ReleaseView(entry);
		}
	}

	private static void DetachHitpointsHandler(BossEntry entry)
	{
		if (entry != null && entry.HitpointsEmptyHandler != null && GodotObject.IsInstanceValid(entry.Boss?.instance))
		{
			entry.Boss.instance.hitpointsEmpty -= entry.HitpointsEmptyHandler;
			entry.HitpointsEmptyHandler = null;
		}
	}

	private static void ReleaseView(BossEntry entry)
	{
		if (GodotObject.IsInstanceValid(entry.View))
		{
			entry.View.QueueFree();
		}
		entry.View = null;
	}

	private static bool IsBossCharacter(TowerDefenseZombie boss)
	{
		if (GodotObject.IsInstanceValid(boss) && GodotObject.IsInstanceValid(boss.instance))
		{
			return boss.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		}
		return false;
	}

	private static bool IsBossDead(TowerDefenseZombie boss)
	{
		if (GodotObject.IsInstanceValid(boss) && GodotObject.IsInstanceValid(boss.instance) && !boss.instance.die)
		{
			return boss.instance.hitpoints <= 0.0;
		}
		return true;
	}

	private static bool TryGetBossRatios(TowerDefenseZombie boss, out double bodyRatio, out double shieldRatio)
	{
		bodyRatio = 0.0;
		shieldRatio = 0.0;
		if (!IsBossCharacter(boss))
		{
			return false;
		}
		double hitpointsSave = boss.instance.hitpointsSave;
		if (double.IsFinite(hitpointsSave) && hitpointsSave > 0.0 && double.IsFinite(boss.instance.hitpoints))
		{
			bodyRatio = Math.Clamp(boss.instance.hitpoints / hitpointsSave, 0.0, 1.0);
		}
		double num = 0.0;
		double num2 = 0.0;
		Array<TowerDefenseArmorInstance> armorList = boss.instance.armorList;
		for (int i = 0; i < armorList.Count; i++)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = armorList[i];
			if (towerDefenseArmorInstance != null && !towerDefenseArmorInstance.isRemove && (towerDefenseArmorInstance.armorMethodFlags & 0x40) != 0)
			{
				if (double.IsFinite(towerDefenseArmorInstance.hitpointsSave) && towerDefenseArmorInstance.hitpointsSave > 0.0)
				{
					num2 += towerDefenseArmorInstance.hitpointsSave;
				}
				if (double.IsFinite(towerDefenseArmorInstance.hitPoints) && towerDefenseArmorInstance.hitPoints > 0.0)
				{
					num += towerDefenseArmorInstance.hitPoints;
				}
			}
		}
		if (num2 > 0.0)
		{
			shieldRatio = Math.Clamp(num / num2, 0.0, 1.0);
		}
		return true;
	}

	private static void ResolveStyle(TowerDefenseZombie boss, out Texture2D icon, out Color color)
	{
		switch (boss.config?.name)
		{
		case "ZombieBoss":
			icon = ZombieBossIcon;
			color = ZombieBossColor;
			break;
		case "ZombieBossDave":
			icon = ZombieBossDaveIcon;
			color = ZombieBossDaveColor;
			break;
		case "ZombieBossEdgarII":
			icon = ZombieBossEdgarIcon;
			color = ZombieBossEdgarColor;
			break;
		default:
			icon = null;
			color = ZombieBossColor;
			break;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SubscribeEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnsubscribeEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnShowBossHealthBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnUiSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnScreenTransformChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyQueuedLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReconcileBosses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrackBoss, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AssignViewsAndLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LayoutView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "view", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "displayIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTopPropRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsBattleRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ClearTrackedBosses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsBossCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsBossDead, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeEvents && args.Count == 0)
		{
			SubscribeEvents();
			ret = default;
			return true;
		}
		if (method == MethodName.UnsubscribeEvents && args.Count == 0)
		{
			UnsubscribeEvents();
			ret = default;
			return true;
		}
		if (method == MethodName.OnShowBossHealthBar && args.Count == 1)
		{
			OnShowBossHealthBar(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnUiSwitched && args.Count == 1)
		{
			OnUiSwitched(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnScreenTransformChanged && args.Count == 0)
		{
			OnScreenTransformChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueLayout && args.Count == 0)
		{
			QueueLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyQueuedLayout && args.Count == 0)
		{
			ApplyQueuedLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.ReconcileBosses && args.Count == 1)
		{
			ReconcileBosses(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrackBoss && args.Count == 1)
		{
			TrackBoss(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AssignViewsAndLayout && args.Count == 0)
		{
			AssignViewsAndLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.LayoutView && args.Count == 2)
		{
			LayoutView(VariantUtils.ConvertTo<TowerDefenseBossHealthBarView>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetTopPropRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetTopPropRect());
			return true;
		}
		if (method == MethodName.IsBattleRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBattleRunning());
			return true;
		}
		if (method == MethodName.ClearTrackedBosses && args.Count == 0)
		{
			ClearTrackedBosses();
			ret = default;
			return true;
		}
		if (method == MethodName.IsBossCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossCharacter(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBossDead && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossDead(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetTopPropRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetTopPropRect());
			return true;
		}
		if (method == MethodName.IsBattleRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBattleRunning());
			return true;
		}
		if (method == MethodName.IsBossCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossCharacter(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBossDead && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossDead(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
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
		if (method == MethodName._ExitTree)
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.SubscribeEvents)
		{
			return true;
		}
		if (method == MethodName.UnsubscribeEvents)
		{
			return true;
		}
		if (method == MethodName.OnShowBossHealthBar)
		{
			return true;
		}
		if (method == MethodName.OnUiSwitched)
		{
			return true;
		}
		if (method == MethodName.OnScreenTransformChanged)
		{
			return true;
		}
		if (method == MethodName.QueueLayout)
		{
			return true;
		}
		if (method == MethodName.ApplyQueuedLayout)
		{
			return true;
		}
		if (method == MethodName.ReconcileBosses)
		{
			return true;
		}
		if (method == MethodName.TrackBoss)
		{
			return true;
		}
		if (method == MethodName.AssignViewsAndLayout)
		{
			return true;
		}
		if (method == MethodName.LayoutView)
		{
			return true;
		}
		if (method == MethodName.GetTopPropRect)
		{
			return true;
		}
		if (method == MethodName.IsBattleRunning)
		{
			return true;
		}
		if (method == MethodName.ClearTrackedBosses)
		{
			return true;
		}
		if (method == MethodName.IsBossCharacter)
		{
			return true;
		}
		if (method == MethodName.IsBossDead)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._nextSequence)
		{
			_nextSequence = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._lastMembershipRevision)
		{
			_lastMembershipRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._mobilePreset)
		{
			_mobilePreset = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._showBossHealthBar)
		{
			_showBossHealthBar = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._battleRunning)
		{
			_battleRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._viewsDirty)
		{
			_viewsDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._layoutQueued)
		{
			_layoutQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasPhysicalMousePosition)
		{
			_hasPhysicalMousePosition = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._physicalMousePosition)
		{
			_physicalMousePosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._eventBus)
		{
			_eventBus = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.VisibleBarCount)
		{
			from = VisibleBarCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TrackedBossCount)
		{
			from = TrackedBossCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._nextSequence)
		{
			value = VariantUtils.CreateFrom(in _nextSequence);
			return true;
		}
		if (name == PropertyName._lastMembershipRevision)
		{
			value = VariantUtils.CreateFrom(in _lastMembershipRevision);
			return true;
		}
		if (name == PropertyName._mobilePreset)
		{
			value = VariantUtils.CreateFrom(in _mobilePreset);
			return true;
		}
		if (name == PropertyName._showBossHealthBar)
		{
			value = VariantUtils.CreateFrom(in _showBossHealthBar);
			return true;
		}
		if (name == PropertyName._battleRunning)
		{
			value = VariantUtils.CreateFrom(in _battleRunning);
			return true;
		}
		if (name == PropertyName._viewsDirty)
		{
			value = VariantUtils.CreateFrom(in _viewsDirty);
			return true;
		}
		if (name == PropertyName._layoutQueued)
		{
			value = VariantUtils.CreateFrom(in _layoutQueued);
			return true;
		}
		if (name == PropertyName._hasPhysicalMousePosition)
		{
			value = VariantUtils.CreateFrom(in _hasPhysicalMousePosition);
			return true;
		}
		if (name == PropertyName._physicalMousePosition)
		{
			value = VariantUtils.CreateFrom(in _physicalMousePosition);
			return true;
		}
		if (name == PropertyName._eventBus)
		{
			value = VariantUtils.CreateFrom(in _eventBus);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._nextSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastMembershipRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mobilePreset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showBossHealthBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._battleRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewsDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._layoutQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPhysicalMousePosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._physicalMousePosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventBus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleBarCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TrackedBossCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nextSequence, Variant.From(in _nextSequence));
		info.AddProperty(PropertyName._lastMembershipRevision, Variant.From(in _lastMembershipRevision));
		info.AddProperty(PropertyName._mobilePreset, Variant.From(in _mobilePreset));
		info.AddProperty(PropertyName._showBossHealthBar, Variant.From(in _showBossHealthBar));
		info.AddProperty(PropertyName._battleRunning, Variant.From(in _battleRunning));
		info.AddProperty(PropertyName._viewsDirty, Variant.From(in _viewsDirty));
		info.AddProperty(PropertyName._layoutQueued, Variant.From(in _layoutQueued));
		info.AddProperty(PropertyName._hasPhysicalMousePosition, Variant.From(in _hasPhysicalMousePosition));
		info.AddProperty(PropertyName._physicalMousePosition, Variant.From(in _physicalMousePosition));
		info.AddProperty(PropertyName._eventBus, Variant.From(in _eventBus));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nextSequence, out var value))
		{
			_nextSequence = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._lastMembershipRevision, out var value2))
		{
			_lastMembershipRevision = value2.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._mobilePreset, out var value3))
		{
			_mobilePreset = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showBossHealthBar, out var value4))
		{
			_showBossHealthBar = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._battleRunning, out var value5))
		{
			_battleRunning = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._viewsDirty, out var value6))
		{
			_viewsDirty = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._layoutQueued, out var value7))
		{
			_layoutQueued = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasPhysicalMousePosition, out var value8))
		{
			_hasPhysicalMousePosition = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._physicalMousePosition, out var value9))
		{
			_physicalMousePosition = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._eventBus, out var value10))
		{
			_eventBus = value10.As<BattleEventBus>();
		}
	}
}
