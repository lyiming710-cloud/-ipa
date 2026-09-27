using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Explode/TowerDefenseExplode.cs")]
public class TowerDefenseExplode : Node2D
{
	private enum ExplosionKind
	{
		Projectile,
		Area,
		Line,
		Column
	}

	private sealed class ExplosionWorkItem
	{
		public ExplosionKind Kind;

		public ulong EarliestPhysicsFrame;

		public Vector2 Position;

		public Vector2 Size;

		public int LineOrColumn;

		public TowerDefenseEnum.CHARACTER_CAMP Camp;

		public int CollisionFlags;

		public bool CanHitShieldedTargets;

		public bool SuppressDeathrattles;

		public TowerDefenseProjectileConfig ProjectileConfig;

		public ProjectileHitInfo ProjectileHitInfo;

		public TowerDefenseCharacterEventBase[] ProjectileEvents;

		public readonly List<TowerDefenseCharacterEventBase> Events = new List<TowerDefenseCharacterEventBase>();

		public TowerDefenseCharacter ExcludedCharacter;

		public HashSet<TowerDefenseCharacter> ExcludeSet;

		public List<TowerDefenseCharacter> Characters;

		public int CharacterIndex;

		public void Reset()
		{
			Kind = ExplosionKind.Projectile;
			EarliestPhysicsFrame = 0uL;
			Position = default;
			Size = default;
			LineOrColumn = 0;
			Camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
			CollisionFlags = 0;
			CanHitShieldedTargets = false;
			SuppressDeathrattles = false;
			ProjectileConfig = null;
			ProjectileHitInfo = default;
			ProjectileEvents = null;
			Events.Clear();
			ExcludedCharacter = null;
			ExcludeSet = null;
			Characters = null;
			CharacterIndex = 0;
		}
	}

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName GetOrCreateDispatcher = "GetOrCreateDispatcher";

		public static readonly StringName CreateProjectileExplode = "CreateProjectileExplode";

		public static readonly StringName CreateProjectileExplodeSingleExclude = "CreateProjectileExplodeSingleExclude";

		public static readonly StringName QueueProjectileExplode = "QueueProjectileExplode";

		public static readonly StringName CreateExplode = "CreateExplode";

		public static readonly StringName CreateExplodeLine = "CreateExplodeLine";

		public static readonly StringName CreateExplodeColumn = "CreateExplodeColumn";

		public static readonly StringName QueueLineColumnExplosion = "QueueLineColumnExplosion";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName GetPendingWorkItemCountForTest = "GetPendingWorkItemCountForTest";

		public static readonly StringName DrainPendingWorkForTest = "DrainPendingWorkForTest";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CanExplosionCollect = "CanExplosionCollect";

		public static readonly StringName PassesCollisionFilter = "PassesCollisionFilter";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _dispatching = "_dispatching";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const int MaxCharacterSnapshotPoolSize = 32;

	private const int MaxExcludeSetPoolSize = 32;

	private const int MaxWorkItemPoolSize = 2048;

	private const string DispatcherNodeName = "ExplosionDispatchQueue";

	private static readonly HashSet<TowerDefenseCharacter> _emptyExclude = new HashSet<TowerDefenseCharacter>();

	private static readonly Stack<List<TowerDefenseCharacter>> _characterSnapshotPool = new Stack<List<TowerDefenseCharacter>>();

	private static readonly Stack<HashSet<TowerDefenseCharacter>> _excludeSetPool = new Stack<HashSet<TowerDefenseCharacter>>();

	private static readonly Stack<ExplosionWorkItem> _workItemPool = new Stack<ExplosionWorkItem>();

	private static TowerDefenseExplode _dispatcher;

	private static Node2D _dispatcherRoot;

	private readonly Queue<ExplosionWorkItem> _pending = new Queue<ExplosionWorkItem>();

	private bool _dispatching;

	private static List<TowerDefenseCharacter> RentCharacterSnapshot()
	{
		if (_characterSnapshotPool.Count == 0)
		{
			return new List<TowerDefenseCharacter>();
		}
		return _characterSnapshotPool.Pop();
	}

	private static void ReturnCharacterSnapshot(List<TowerDefenseCharacter> snapshot)
	{
		if (snapshot != null)
		{
			snapshot.Clear();
			if (_characterSnapshotPool.Count < 32)
			{
				_characterSnapshotPool.Push(snapshot);
			}
		}
	}

	private static HashSet<TowerDefenseCharacter> RentExcludeSet(Array<TowerDefenseCharacter> exclude)
	{
		if (exclude == null)
		{
			return _emptyExclude;
		}
		int count = exclude.Count;
		if (count == 0)
		{
			return _emptyExclude;
		}
		HashSet<TowerDefenseCharacter> hashSet = ((_excludeSetPool.Count == 0) ? new HashSet<TowerDefenseCharacter>() : _excludeSetPool.Pop());
		for (int i = 0; i < count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = exclude[i];
			if (towerDefenseCharacter != null)
			{
				hashSet.Add(towerDefenseCharacter);
			}
		}
		return hashSet;
	}

	private static void CaptureExclusions(ExplosionWorkItem item, Array<TowerDefenseCharacter> exclude)
	{
		if (exclude == null || exclude.Count == 0)
		{
			item.ExcludeSet = _emptyExclude;
		}
		else if (exclude.Count == 1)
		{
			item.ExcludedCharacter = exclude[0];
			item.ExcludeSet = _emptyExclude;
		}
		else
		{
			item.ExcludeSet = RentExcludeSet(exclude);
		}
	}

	private static void ReturnExcludeSet(HashSet<TowerDefenseCharacter> set)
	{
		if (set != null && set != _emptyExclude)
		{
			set.Clear();
			if (_excludeSetPool.Count < 32)
			{
				_excludeSetPool.Push(set);
			}
		}
	}

	private static ExplosionWorkItem RentWorkItem()
	{
		if (_workItemPool.Count != 0)
		{
			return _workItemPool.Pop();
		}
		return new ExplosionWorkItem();
	}

	private static void ReturnWorkItem(ExplosionWorkItem item)
	{
		if (item != null)
		{
			ReturnExcludeSet(item.ExcludeSet);
			ReturnCharacterSnapshot(item.Characters);
			item.Reset();
			if (_workItemPool.Count < 2048)
			{
				_workItemPool.Push(item);
			}
		}
	}

	private static void CopyEvents(Array<TowerDefenseCharacterEventBase> source, List<TowerDefenseCharacterEventBase> destination)
	{
		if (source != null)
		{
			int count = source.Count;
			for (int i = 0; i < count; i++)
			{
				destination.Add(source[i]);
			}
		}
	}

	private static TowerDefenseExplode GetOrCreateDispatcher()
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return null;
		}
		if (_dispatcher != null && !_dispatcher.IsQueuedForDeletion() && _dispatcherRoot == characterNode)
		{
			return _dispatcher;
		}
		TowerDefenseExplode nodeOrNull = characterNode.GetNodeOrNull<TowerDefenseExplode>("ExplosionDispatchQueue");
		if (GodotObject.IsInstanceValid(nodeOrNull) && !nodeOrNull.IsQueuedForDeletion())
		{
			_dispatcher = nodeOrNull;
			_dispatcherRoot = characterNode;
			return nodeOrNull;
		}
		_dispatcher = new TowerDefenseExplode
		{
			Name = "ExplosionDispatchQueue"
		};
		characterNode.AddChild(_dispatcher, forceReadableName: false, InternalMode.Disabled);
		_dispatcherRoot = characterNode;
		return _dispatcher;
	}

	private static void Enqueue(ExplosionWorkItem item)
	{
		TowerDefenseExplode orCreateDispatcher = GetOrCreateDispatcher();
		if (orCreateDispatcher == null)
		{
			ReturnWorkItem(item);
			return;
		}
		item.EarliestPhysicsFrame = Engine.GetPhysicsFrames() + 1;
		orCreateDispatcher._pending.Enqueue(item);
		orCreateDispatcher.SetPhysicsProcess(enable: true);
	}

	public static TowerDefenseExplode CreateProjectileExplode(Vector2 pos, TowerDefenseProjectileConfig projectileConfig, Array<TowerDefenseCharacter> exclude, TowerDefenseEnum.CHARACTER_CAMP camp, bool canHitShieldedTargets = false)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		if (!GodotObject.IsInstanceValid(projectileConfig))
		{
			return null;
		}
		QueueProjectileExplode(pos, projectileConfig, exclude, null, camp, canHitShieldedTargets, projectileConfig.baseDamage, projectileConfig.damageFlags, projectileConfig.fireMethodFlags, projectileConfig.collisionFlags);
		return null;
	}

	public static TowerDefenseExplode CreateProjectileExplode(Vector2 pos, TowerDefenseProjectileConfig projectileConfig, Array<TowerDefenseCharacter> exclude, TowerDefenseEnum.CHARACTER_CAMP camp, bool canHitShieldedTargets, double runtimeDamage, int runtimeDamageFlags, int runtimeFireMethodFlags, int runtimeCollisionFlags)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		if (!GodotObject.IsInstanceValid(projectileConfig))
		{
			return null;
		}
		QueueProjectileExplode(pos, projectileConfig, exclude, null, camp, canHitShieldedTargets, runtimeDamage, runtimeDamageFlags, runtimeFireMethodFlags, runtimeCollisionFlags);
		return null;
	}

	internal static TowerDefenseExplode CreateProjectileExplodeSingleExclude(Vector2 pos, TowerDefenseProjectileConfig projectileConfig, TowerDefenseCharacter excludedCharacter, TowerDefenseEnum.CHARACTER_CAMP camp, bool canHitShieldedTargets = false)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		if (!GodotObject.IsInstanceValid(projectileConfig))
		{
			return null;
		}
		QueueProjectileExplode(pos, projectileConfig, null, excludedCharacter, camp, canHitShieldedTargets, projectileConfig.baseDamage, projectileConfig.damageFlags, projectileConfig.fireMethodFlags, projectileConfig.collisionFlags);
		return null;
	}

	internal static TowerDefenseExplode CreateProjectileExplodeSingleExclude(Vector2 pos, TowerDefenseProjectileConfig projectileConfig, TowerDefenseCharacter excludedCharacter, TowerDefenseEnum.CHARACTER_CAMP camp, bool canHitShieldedTargets, double runtimeDamage, int runtimeDamageFlags, int runtimeFireMethodFlags, int runtimeCollisionFlags)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		if (!GodotObject.IsInstanceValid(projectileConfig))
		{
			return null;
		}
		QueueProjectileExplode(pos, projectileConfig, null, excludedCharacter, camp, canHitShieldedTargets, runtimeDamage, runtimeDamageFlags, runtimeFireMethodFlags, runtimeCollisionFlags);
		return null;
	}

	private static void QueueProjectileExplode(Vector2 pos, TowerDefenseProjectileConfig projectileConfig, Array<TowerDefenseCharacter> exclude, TowerDefenseCharacter excludedCharacter, TowerDefenseEnum.CHARACTER_CAMP camp, bool canHitShieldedTargets, double runtimeDamage, int runtimeDamageFlags, int runtimeFireMethodFlags, int runtimeCollisionFlags)
	{
		ExplosionWorkItem explosionWorkItem = RentWorkItem();
		explosionWorkItem.Kind = ExplosionKind.Projectile;
		explosionWorkItem.Position = pos;
		explosionWorkItem.Camp = camp;
		explosionWorkItem.CanHitShieldedTargets = canHitShieldedTargets;
		explosionWorkItem.CollisionFlags = runtimeCollisionFlags;
		explosionWorkItem.ProjectileConfig = projectileConfig;
		explosionWorkItem.ProjectileHitInfo = new ProjectileHitInfo
		{
			damage = runtimeDamage,
			damageFlags = runtimeDamageFlags,
			useRuntimeOverrides = true,
			fireMethodFlags = runtimeFireMethodFlags,
			position = pos,
			projectileHeight = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL,
			config = projectileConfig,
			camp = camp,
			collisionFlags = runtimeCollisionFlags
		};
		explosionWorkItem.SuppressDeathrattles = (runtimeDamageFlags & 0x20) != 0;
		explosionWorkItem.ProjectileEvents = projectileConfig.HitCharacterEvents;
		explosionWorkItem.ExcludedCharacter = excludedCharacter;
		if (excludedCharacter == null)
		{
			CaptureExclusions(explosionWorkItem, exclude);
		}
		Enqueue(explosionWorkItem);
	}

	public static TowerDefenseExplode CreateExplode(Vector2 pos, Vector2 size, Array<TowerDefenseCharacterEventBase> eventList, Array<TowerDefenseCharacter> exclude, TowerDefenseEnum.CHARACTER_CAMP camp, int collisionFlags, bool suppressDeathrattles = false)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		if (eventList == null || eventList.Count == 0)
		{
			return null;
		}
		ExplosionWorkItem explosionWorkItem = RentWorkItem();
		explosionWorkItem.Kind = ExplosionKind.Area;
		explosionWorkItem.Position = pos;
		explosionWorkItem.Size = size;
		explosionWorkItem.Camp = camp;
		explosionWorkItem.CollisionFlags = collisionFlags;
		explosionWorkItem.SuppressDeathrattles = suppressDeathrattles;
		CaptureExclusions(explosionWorkItem, exclude);
		CopyEvents(eventList, explosionWorkItem.Events);
		Enqueue(explosionWorkItem);
		return null;
	}

	public static TowerDefenseExplode CreateExplodeLine(int line, Array<TowerDefenseCharacterEventBase> eventList, Array<TowerDefenseCharacter> exclude, TowerDefenseEnum.CHARACTER_CAMP camp, int collisionFlags, bool suppressDeathrattles = false)
	{
		return QueueLineColumnExplosion(ExplosionKind.Line, line, eventList, exclude, camp, collisionFlags, suppressDeathrattles);
	}

	public static TowerDefenseExplode CreateExplodeColumn(int column, Array<TowerDefenseCharacterEventBase> eventList, Array<TowerDefenseCharacter> exclude, TowerDefenseEnum.CHARACTER_CAMP camp, int collisionFlags, bool suppressDeathrattles = false)
	{
		return QueueLineColumnExplosion(ExplosionKind.Column, column, eventList, exclude, camp, collisionFlags, suppressDeathrattles);
	}

	private static TowerDefenseExplode QueueLineColumnExplosion(ExplosionKind kind, int lineOrColumn, Array<TowerDefenseCharacterEventBase> eventList, Array<TowerDefenseCharacter> exclude, TowerDefenseEnum.CHARACTER_CAMP camp, int collisionFlags, bool suppressDeathrattles = false)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return null;
		}
		if (eventList == null || eventList.Count == 0)
		{
			return null;
		}
		ExplosionWorkItem explosionWorkItem = RentWorkItem();
		explosionWorkItem.Kind = kind;
		explosionWorkItem.LineOrColumn = lineOrColumn;
		explosionWorkItem.Camp = camp;
		explosionWorkItem.CollisionFlags = collisionFlags;
		explosionWorkItem.SuppressDeathrattles = suppressDeathrattles;
		CaptureExclusions(explosionWorkItem, exclude);
		CopyEvents(eventList, explosionWorkItem.Events);
		Enqueue(explosionWorkItem);
		return null;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_dispatching)
		{
			return;
		}
		_dispatching = true;
		int items = 0;
		int explosionCount = 0;
		try
		{
			items = DispatchReadyExplosions(respectEarliestPhysicsFrame: true, out explosionCount);
		}
		finally
		{
			_dispatching = false;
			TowerDefensePerfProfiler.SampleHotPath("explode.dispatch.workUnits", items);
			TowerDefensePerfProfiler.SampleHotPath("explode.dispatch.explosions", explosionCount);
			if (_pending.Count == 0)
			{
				SetPhysicsProcess(enable: false);
			}
		}
	}

	private int DispatchReadyExplosions(bool respectEarliestPhysicsFrame, out int explosionCount)
	{
		ulong num = (respectEarliestPhysicsFrame ? Engine.GetPhysicsFrames() : 18446744073709551615uL);
		int num2 = 0;
		explosionCount = 0;
		while (_pending.Count > 0)
		{
			ExplosionWorkItem explosionWorkItem = _pending.Peek();
			if (explosionWorkItem.EarliestPhysicsFrame > num)
			{
				break;
			}
			_pending.Dequeue();
			explosionCount++;
			try
			{
				PrepareCharacters(explosionWorkItem);
				num2++;
				while (explosionWorkItem.CharacterIndex < explosionWorkItem.Characters.Count)
				{
					ExecuteNextCharacter(explosionWorkItem);
					num2++;
				}
			}
			catch (Exception value)
			{
				GD.PushError($"TowerDefenseExplode atomic dispatch failed: {value}");
			}
			finally
			{
				ReturnWorkItem(explosionWorkItem);
			}
		}
		return num2;
	}

	internal static int GetPendingWorkItemCountForTest()
	{
		if (!GodotObject.IsInstanceValid(_dispatcher))
		{
			return 0;
		}
		return _dispatcher._pending.Count;
	}

	internal static int DrainPendingWorkForTest()
	{
		if (!GodotObject.IsInstanceValid(_dispatcher) || _dispatcher._dispatching)
		{
			return 0;
		}
		TowerDefenseExplode dispatcher = _dispatcher;
		dispatcher._dispatching = true;
		try
		{
			int explosionCount;
			return dispatcher.DispatchReadyExplosions(respectEarliestPhysicsFrame: false, out explosionCount);
		}
		finally
		{
			dispatcher._dispatching = false;
			if (dispatcher._pending.Count == 0)
			{
				dispatcher.SetPhysicsProcess(enable: false);
			}
		}
	}

	private static void PrepareCharacters(ExplosionWorkItem item)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			item.Characters = RentCharacterSnapshot();
			return;
		}
		item.Characters = RentCharacterSnapshot();
		switch (item.Kind)
		{
		case ExplosionKind.Projectile:
			if (item.ProjectileConfig != null)
			{
				Vector2 size2 = instance.GetMapGridSize() * 2f * item.ProjectileConfig.rangeSize;
				instance.characterRegistry.FillCharactersIntersectingRectListExcludingCamp(AabbShapeUtil.RectFromCenter(item.Position, size2), item.Camp, item.Characters);
			}
			break;
		case ExplosionKind.Area:
		{
			Vector2 size = instance.GetMapGridSize() * 2f * item.Size;
			instance.characterRegistry.FillCharactersIntersectingRectListExcludingCamp(AabbShapeUtil.RectFromCenter(item.Position, size), item.Camp, item.Characters);
			break;
		}
		case ExplosionKind.Line:
			item.Characters.AddRange(instance.GetCharacterLineList(item.LineOrColumn, fliterGraveStone: false));
			break;
		case ExplosionKind.Column:
			item.Characters.AddRange(instance.GetCharacterColumnList(item.LineOrColumn, fliterGraveStone: false));
			break;
		}
	}

	private static void ExecuteNextCharacter(ExplosionWorkItem item)
	{
		TowerDefenseCharacter towerDefenseCharacter = item.Characters[item.CharacterIndex++];
		if (!PassesBaseFilter(towerDefenseCharacter, item.Camp, item.ExcludedCharacter, item.ExcludeSet, out var instance) || !towerDefenseCharacter.CanReceiveExplosionHit())
		{
			return;
		}
		bool allowItemLadder = item.Kind == ExplosionKind.Line || item.Kind == ExplosionKind.Column;
		if (!PassesCollisionFilter(towerDefenseCharacter, instance, item.CollisionFlags, allowItemLadder))
		{
			return;
		}
		bool suppressDeathrattles = towerDefenseCharacter.suppressDeathrattles;
		towerDefenseCharacter.suppressDeathrattles |= item.SuppressDeathrattles;
		try
		{
			ExecuteCharacterEvents(item, towerDefenseCharacter, instance);
		}
		finally
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && !towerDefenseCharacter.isDestroy && !instance.die && !instance.nearDie)
			{
				towerDefenseCharacter.suppressDeathrattles = suppressDeathrattles;
			}
		}
	}

	private static void ExecuteCharacterEvents(ExplosionWorkItem item, TowerDefenseCharacter character, TowerDefenseCharacterInstance instance)
	{
		if (item.Kind == ExplosionKind.Projectile)
		{
			bool flag = (item.ProjectileHitInfo.fireMethodFlags & 2) != 0;
			if (!instance.HasShieldArmor || flag || item.CanHitShieldedTargets)
			{
				TowerDefenseCharacterEventBase[] projectileEvents = item.ProjectileEvents;
				for (int i = 0; i < projectileEvents.Length; i++)
				{
					projectileEvents[i].Execute(character.GetLogicalGlobalPosition(), character);
				}
				character.ProjectileHurt(in item.ProjectileHitInfo, item.ProjectileConfig, playSplatAudio: true, Vector2.Zero, isRange: true);
			}
		}
		else
		{
			Vector2 pos = ((item.Kind == ExplosionKind.Area) ? item.Position : (character.GetLogicalGlobalPosition() + new Vector2((float)GD.RandRange(-25.0, 25.0), 0f)));
			for (int j = 0; j < item.Events.Count; j++)
			{
				item.Events[j].Execute(pos, character);
			}
		}
	}

	public override void _ExitTree()
	{
		while (_pending.Count > 0)
		{
			ReturnWorkItem(_pending.Dequeue());
		}
		if (_dispatcher == this)
		{
			_dispatcher = null;
			_dispatcherRoot = null;
		}
	}

	private static bool PassesBaseFilter(TowerDefenseCharacter character, TowerDefenseEnum.CHARACTER_CAMP camp, TowerDefenseCharacter excludedCharacter, HashSet<TowerDefenseCharacter> exclude, out TowerDefenseCharacterInstance instance)
	{
		instance = null;
		if (character == null)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(character))
		{
			return false;
		}
		if (character.die || character.nearDie)
		{
			return false;
		}
		if (character.camp == camp)
		{
			return false;
		}
		if (character == excludedCharacter)
		{
			return false;
		}
		if (exclude != null && exclude.Contains(character))
		{
			return false;
		}
		TowerDefenseCharacterInstance instance2 = character.instance;
		if (!GodotObject.IsInstanceValid(instance2))
		{
			return false;
		}
		if (!CanExplosionCollect(character, instance2))
		{
			return false;
		}
		instance = instance2;
		return true;
	}

	private static bool CanExplosionCollect(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance)
	{
		if (instance.canBeCollection)
		{
			return true;
		}
		return character is TowerDefenseZombieGhost;
	}

	private static bool PassesCollisionFilter(TowerDefenseCharacter character, TowerDefenseCharacterInstance instance, int collisionFlags, bool allowItemLadder)
	{
		if (allowItemLadder && character.config.name == "ItemLadder")
		{
			return true;
		}
		if ((collisionFlags & instance.maskFlags) == 0 && collisionFlags != -1)
		{
			return false;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.GetOrCreateDispatcher, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateProjectileExplode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canHitShieldedTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateProjectileExplode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canHitShieldedTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "runtimeDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeDamageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeFireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeCollisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateProjectileExplodeSingleExclude, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "excludedCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canHitShieldedTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateProjectileExplodeSingleExclude, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "excludedCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canHitShieldedTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "runtimeDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeDamageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeFireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeCollisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueProjectileExplode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "excludedCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canHitShieldedTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "runtimeDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeDamageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeFireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runtimeCollisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateExplode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "eventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressDeathrattles", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateExplodeLine, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "eventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressDeathrattles", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateExplodeColumn, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "eventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressDeathrattles", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueLineColumnExplosion, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "lineOrColumn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "eventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "exclude", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressDeathrattles", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPendingWorkItemCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.DrainPendingWorkForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanExplosionCollect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PassesCollisionFilter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "allowItemLadder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetOrCreateDispatcher && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(GetOrCreateDispatcher());
			return true;
		}
		if (method == MethodName.CreateProjectileExplode && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateProjectileExplode && args.Count == 9)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8])));
			return true;
		}
		if (method == MethodName.CreateProjectileExplodeSingleExclude && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplodeSingleExclude(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateProjectileExplodeSingleExclude && args.Count == 9)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplodeSingleExclude(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8])));
			return true;
		}
		if (method == MethodName.QueueProjectileExplode && args.Count == 10)
		{
			QueueProjectileExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateExplode && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.CreateExplodeLine && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateExplodeLine(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.CreateExplodeColumn && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateExplodeColumn(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.QueueLineColumnExplosion && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(QueueLineColumnExplosion(VariantUtils.ConvertTo<ExplosionKind>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPendingWorkItemCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPendingWorkItemCountForTest());
			return true;
		}
		if (method == MethodName.DrainPendingWorkForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(DrainPendingWorkForTest());
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CanExplosionCollect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanExplosionCollect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1])));
			return true;
		}
		if (method == MethodName.PassesCollisionFilter && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(PassesCollisionFilter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetOrCreateDispatcher && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(GetOrCreateDispatcher());
			return true;
		}
		if (method == MethodName.CreateProjectileExplode && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateProjectileExplode && args.Count == 9)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8])));
			return true;
		}
		if (method == MethodName.CreateProjectileExplodeSingleExclude && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplodeSingleExclude(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.CreateProjectileExplodeSingleExclude && args.Count == 9)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateProjectileExplodeSingleExclude(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8])));
			return true;
		}
		if (method == MethodName.QueueProjectileExplode && args.Count == 10)
		{
			QueueProjectileExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateExplode && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateExplode(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.CreateExplodeLine && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateExplodeLine(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.CreateExplodeColumn && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(CreateExplodeColumn(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.QueueLineColumnExplosion && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseExplode>(QueueLineColumnExplosion(VariantUtils.ConvertTo<ExplosionKind>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacter>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.GetPendingWorkItemCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPendingWorkItemCountForTest());
			return true;
		}
		if (method == MethodName.DrainPendingWorkForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(DrainPendingWorkForTest());
			return true;
		}
		if (method == MethodName.CanExplosionCollect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanExplosionCollect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1])));
			return true;
		}
		if (method == MethodName.PassesCollisionFilter && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(PassesCollisionFilter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterInstance>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetOrCreateDispatcher)
		{
			return true;
		}
		if (method == MethodName.CreateProjectileExplode)
		{
			return true;
		}
		if (method == MethodName.CreateProjectileExplodeSingleExclude)
		{
			return true;
		}
		if (method == MethodName.QueueProjectileExplode)
		{
			return true;
		}
		if (method == MethodName.CreateExplode)
		{
			return true;
		}
		if (method == MethodName.CreateExplodeLine)
		{
			return true;
		}
		if (method == MethodName.CreateExplodeColumn)
		{
			return true;
		}
		if (method == MethodName.QueueLineColumnExplosion)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.GetPendingWorkItemCountForTest)
		{
			return true;
		}
		if (method == MethodName.DrainPendingWorkForTest)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CanExplosionCollect)
		{
			return true;
		}
		if (method == MethodName.PassesCollisionFilter)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._dispatching)
		{
			_dispatching = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._dispatching)
		{
			value = VariantUtils.CreateFrom(in _dispatching);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._dispatching, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._dispatching, Variant.From(in _dispatching));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._dispatching, out var value))
		{
			_dispatching = value.As<bool>();
		}
	}
}
