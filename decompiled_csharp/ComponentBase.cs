using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Component/ComponentBase.cs")]
public class ComponentBase : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _EnterTree = "_EnterTree";

		public static readonly StringName OnSharedBatchTreeExiting = "OnSharedBatchTreeExiting";

		public static readonly StringName PrepareSharedBatchRegistration = "PrepareSharedBatchRegistration";

		public static readonly StringName SharedBatchPhysicsProcess = "SharedBatchPhysicsProcess";

		public static readonly StringName SetSharedPhysicsBatchWorkEnabled = "SetSharedPhysicsBatchWorkEnabled";

		public static readonly StringName _GetName = "_GetName";

		public static readonly StringName ExportComponentSave = "ExportComponentSave";

		public static readonly StringName ImportComponentSave = "ImportComponentSave";

		public static readonly StringName SyncSerialize = "SyncSerialize";

		public static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName StartComponentTimer = "StartComponentTimer";

		public static readonly StringName StopComponentTimer = "StopComponentTimer";

		public static readonly StringName PauseComponentTimer = "PauseComponentTimer";

		public static readonly StringName ResumeComponentTimer = "ResumeComponentTimer";

		public static readonly StringName IsComponentTimerRunning = "IsComponentTimerRunning";

		public static readonly StringName GetComponentTimerRemaining = "GetComponentTimerRemaining";

		public static readonly StringName OnComponentTimerTimeout = "OnComponentTimerTimeout";

		public static readonly StringName DispatchComponentTimerTimeout = "DispatchComponentTimerTimeout";

		public static readonly StringName ApplyAuthoritativeSync = "ApplyAuthoritativeSync";

		public static readonly StringName CaptureStateMachineSnapshotData = "CaptureStateMachineSnapshotData";

		public static readonly StringName RestoreStateMachineSnapshot = "RestoreStateMachineSnapshot";

		public static readonly StringName CanRestoreStateMachineSnapshot = "CanRestoreStateMachineSnapshot";

		public static readonly StringName SetAlive = "SetAlive";

		public static readonly StringName SendStateEvent = "SendStateEvent";

		public static readonly StringName OnStateRuntimeAttached = "OnStateRuntimeAttached";

		public static readonly StringName OnStateRuntimeRegistered = "OnStateRuntimeRegistered";

		public static readonly StringName OnStateRuntimeDetaching = "OnStateRuntimeDetaching";

		public static readonly StringName OnAuthoritativeStateRestorePreparing = "OnAuthoritativeStateRestorePreparing";

		public static readonly StringName OnAuthoritativeStateRestored = "OnAuthoritativeStateRestored";

		public static readonly StringName RefreshStateMachineDispatchEligibility = "RefreshStateMachineDispatchEligibility";

		public static readonly StringName BeginAuthoritativeStateRestore = "BeginAuthoritativeStateRestore";

		public static readonly StringName EndAuthoritativeStateRestore = "EndAuthoritativeStateRestore";

		public static readonly StringName AttachToManager = "AttachToManager";

		public static readonly StringName DetachFromManager = "DetachFromManager";

		public static readonly StringName AttachStateRuntime = "AttachStateRuntime";

		public static readonly StringName ApplyPendingStateMachineSnapshot = "ApplyPendingStateMachineSnapshot";

		public static readonly StringName ApplyStateMachineSnapshot = "ApplyStateMachineSnapshot";

		public static readonly StringName EnsureStateMachineInitialized = "EnsureStateMachineInitialized";

		public static readonly StringName DisposeStateMachine = "DisposeStateMachine";

		public static readonly StringName SetWireSlotToken = "SetWireSlotToken";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName ConfigureSharedPhysicsBatch = "ConfigureSharedPhysicsBatch";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName alive = "alive";

		public static readonly StringName IsSharedBatchDispatchActive = "IsSharedBatchDispatchActive";

		public static readonly StringName SharedBatchUsesInheritedProcessMode = "SharedBatchUsesInheritedProcessMode";

		public static readonly StringName UseSharedPhysicsBatch = "UseSharedPhysicsBatch";

		public static readonly StringName AllowPhysicsOutsideComponentBattlefield = "AllowPhysicsOutsideComponentBattlefield";

		public static readonly StringName AllowStateMachineOutsideComponentBattlefield = "AllowStateMachineOutsideComponentBattlefield";

		public static readonly StringName IsInsideComponentBattlefield = "IsInsideComponentBattlefield";

		public static readonly StringName CanDispatchPhysicsWork = "CanDispatchPhysicsWork";

		public static readonly StringName CanDispatchStateMachineWork = "CanDispatchStateMachineWork";

		public static readonly StringName StateMachineDefinition = "StateMachineDefinition";

		public static readonly StringName IsStateMachineDispatchEligible = "IsStateMachineDispatchEligible";

		public static readonly StringName IsStateMachineRegistered = "IsStateMachineRegistered";

		public static readonly StringName HasStateMachine = "HasStateMachine";

		public static readonly StringName HasConfiguredStateMachine = "HasConfiguredStateMachine";

		public static readonly StringName HasStateMachineProcessWork = "HasStateMachineProcessWork";

		public static readonly StringName HasStateMachinePhysicsWork = "HasStateMachinePhysicsWork";

		public static readonly StringName ShouldApplyLegacyStateField = "ShouldApplyLegacyStateField";

		public static readonly StringName _alive = "_alive";

		public static readonly StringName _stateMachineDefinition = "_stateMachineDefinition";

		public static readonly StringName _stateMachineManager = "_stateMachineManager";

		public static readonly StringName _stateRuntimeAttached = "_stateRuntimeAttached";

		public static readonly StringName _stateMachineInitialEntered = "_stateMachineInitialEntered";

		public static readonly StringName _stateMachineDisposed = "_stateMachineDisposed";

		public static readonly StringName _authoritativeStateRestoreDepth = "_authoritativeStateRestoreDepth";

		public static readonly StringName _pendingStateMachineSnapshot = "_pendingStateMachineSnapshot";

		public static readonly StringName _pendingStateMachineSnapshotIsRemote = "_pendingStateMachineSnapshotIsRemote";

		public static readonly StringName _pendingStateMachineSnapshotSuppressEffects = "_pendingStateMachineSnapshotSuppressEffects";

		public static readonly StringName _lastRemoteStateMachineRevision = "_lastRemoteStateMachineRevision";

		public static readonly StringName _wireManagerInstanceId = "_wireManagerInstanceId";

		public static readonly StringName _wireTypeName = "_wireTypeName";

		public static readonly StringName _wireTypeIndex = "_wireTypeIndex";

		public static readonly StringName _usingSharedPhysicsBatch = "_usingSharedPhysicsBatch";

		public static readonly StringName _sharedPhysicsBatchWorkEnabled = "_sharedPhysicsBatchWorkEnabled";

		public static readonly StringName _sharedBatchUsesInheritedProcessMode = "_sharedBatchUsesInheritedProcessMode";

		public static readonly StringName _sharedBatchTreeExitConnected = "_sharedBatchTreeExitConnected";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private bool _alive = true;

	private StateMachineDefinition _stateMachineDefinition;

	private ComponentManager _stateMachineManager;

	private bool _stateRuntimeAttached;

	private bool _stateMachineInitialEntered;

	private bool _stateMachineDisposed;

	private int _authoritativeStateRestoreDepth;

	private StateMachineSnapshot _pendingStateMachineSnapshot;

	private bool _pendingStateMachineSnapshotIsRemote;

	private bool _pendingStateMachineSnapshotSuppressEffects;

	private long _lastRemoteStateMachineRevision = -1L;

	private ulong _wireManagerInstanceId;

	private string _wireTypeName = string.Empty;

	private int _wireTypeIndex = -1;

	private bool _usingSharedPhysicsBatch;

	private bool _sharedPhysicsBatchWorkEnabled = true;

	private bool _sharedBatchUsesInheritedProcessMode = true;

	private bool _sharedBatchTreeExitConnected;

	[Export(PropertyHint.None, "")]
	public bool alive
	{
		get
		{
			return _alive;
		}
		set
		{
			SetAlive(value);
		}
	}

	internal bool IsSharedBatchDispatchActive
	{
		get
		{
			if (_usingSharedPhysicsBatch && _sharedPhysicsBatchWorkEnabled && _alive)
			{
				return CanDispatchPhysicsWork;
			}
			return false;
		}
	}

	internal bool SharedBatchUsesInheritedProcessMode => _sharedBatchUsesInheritedProcessMode;

	protected virtual bool UseSharedPhysicsBatch => false;

	protected virtual bool AllowPhysicsOutsideComponentBattlefield => false;

	protected virtual bool AllowStateMachineOutsideComponentBattlefield => false;

	private bool IsInsideComponentBattlefield
	{
		get
		{
			TowerDefenseCharacter parentCharacter = GetParentCharacter<TowerDefenseCharacter>();
			if (GodotObject.IsInstanceValid(parentCharacter))
			{
				return parentCharacter.IsInsideComponentBattlefield;
			}
			return true;
		}
	}

	internal bool CanDispatchPhysicsWork
	{
		get
		{
			if (!AllowPhysicsOutsideComponentBattlefield)
			{
				return IsInsideComponentBattlefield;
			}
			return true;
		}
	}

	internal bool CanDispatchStateMachineWork
	{
		get
		{
			if (!AllowStateMachineOutsideComponentBattlefield)
			{
				return IsInsideComponentBattlefield;
			}
			return true;
		}
	}

	[Export(PropertyHint.None, "")]
	public StateMachineDefinition StateMachineDefinition
	{
		get
		{
			return _stateMachineDefinition;
		}
		set
		{
			if (_stateMachineDefinition == value)
			{
				return;
			}
			_stateMachineDefinition = value;
			if (StateMachine != null || _stateRuntimeAttached || GodotObject.IsInstanceValid(_stateMachineManager))
			{
				DisposeStateMachine();
				if (GodotObject.IsInstanceValid(_stateMachineManager))
				{
					AttachStateRuntime();
				}
			}
		}
	}

	public IStateMachineController StateMachine { get; private set; }

	protected virtual bool IsStateMachineDispatchEligible => true;

	protected bool IsStateMachineRegistered
	{
		get
		{
			if (IsInsideTree() && GodotObject.IsInstanceValid(_stateMachineManager))
			{
				return GetParent() == _stateMachineManager.parent;
			}
			return false;
		}
	}

	internal bool HasStateMachine
	{
		get
		{
			if (StateMachine != null)
			{
				return StateMachine.IsInitialized;
			}
			return false;
		}
	}

	internal bool HasConfiguredStateMachine => StateMachineDefinition != null;

	internal bool HasStateMachineProcessWork
	{
		get
		{
			if (alive && IsStateMachineDispatchEligible)
			{
				return StateMachine?.HasProcessWork ?? false;
			}
			return false;
		}
	}

	internal bool HasStateMachinePhysicsWork
	{
		get
		{
			if (alive && IsStateMachineDispatchEligible)
			{
				return StateMachine?.HasPhysicsWork ?? false;
			}
			return false;
		}
	}

	protected bool ShouldApplyLegacyStateField => _authoritativeStateRestoreDepth == 0;

	public override void _EnterTree()
	{
		if (!_sharedBatchTreeExitConnected)
		{
			TreeExiting += OnSharedBatchTreeExiting;
			_sharedBatchTreeExitConnected = true;
		}
		if (Global.Instance != null && Global.Instance.isEditor && SceneManager.CurrentScene == "LevelEditorStage")
		{
			ProcessMode = ProcessModeEnum.Disabled;
		}
		else
		{
			Callable.From(ConfigureSharedPhysicsBatch).CallDeferred();
		}
	}

	private void OnSharedBatchTreeExiting()
	{
		if (_usingSharedPhysicsBatch)
		{
			ComponentPhysicsBatch.Unregister(this);
			_usingSharedPhysicsBatch = false;
		}
	}

	internal void PrepareSharedBatchRegistration()
	{
		_sharedBatchUsesInheritedProcessMode = ProcessMode == ProcessModeEnum.Inherit;
	}

	internal virtual void SharedBatchPhysicsProcess(double delta)
	{
		_PhysicsProcess(delta);
	}

	protected void SetSharedPhysicsBatchWorkEnabled(bool enabled)
	{
		_sharedPhysicsBatchWorkEnabled = enabled;
		if (!IsInsideTree())
		{
			return;
		}
		if (!UseSharedPhysicsBatch || Engine.IsEditorHint())
		{
			SetPhysicsProcess(_alive & enabled);
		}
		else if (!enabled)
		{
			if (_usingSharedPhysicsBatch)
			{
				ComponentPhysicsBatch.Unregister(this);
				_usingSharedPhysicsBatch = false;
			}
			SetPhysicsProcess(enable: false);
		}
		else
		{
			SetPhysicsProcess(_alive);
			ConfigureSharedPhysicsBatch();
		}
	}

	public virtual string _GetName()
	{
		return "";
	}

	public virtual Dictionary ExportComponentSave()
	{
		return new Dictionary();
	}

	public virtual void ImportComponentSave(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
	}

	public virtual Dictionary SyncSerialize()
	{
		return new Dictionary();
	}

	public virtual void SyncDeserialize(Dictionary _data)
	{
	}

	protected bool StartComponentTimer(StringName timerName, double duration, bool useOwnerTimeScale = false)
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager))
		{
			return _stateMachineManager.StartComponentTimer(this, timerName, duration, useOwnerTimeScale);
		}
		return false;
	}

	protected bool StopComponentTimer(StringName timerName)
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager))
		{
			return _stateMachineManager.StopComponentTimer(this, timerName);
		}
		return false;
	}

	protected bool PauseComponentTimer(StringName timerName)
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager))
		{
			return _stateMachineManager.PauseComponentTimer(this, timerName);
		}
		return false;
	}

	protected bool ResumeComponentTimer(StringName timerName)
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager))
		{
			return _stateMachineManager.ResumeComponentTimer(this, timerName);
		}
		return false;
	}

	protected bool IsComponentTimerRunning(StringName timerName)
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager))
		{
			return _stateMachineManager.IsComponentTimerRunning(this, timerName);
		}
		return false;
	}

	protected double GetComponentTimerRemaining(StringName timerName)
	{
		if (!GodotObject.IsInstanceValid(_stateMachineManager))
		{
			return 0.0;
		}
		return _stateMachineManager.GetComponentTimerRemaining(this, timerName);
	}

	protected virtual void OnComponentTimerTimeout(StringName timerName)
	{
	}

	internal void DispatchComponentTimerTimeout(StringName timerName)
	{
		OnComponentTimerTimeout(timerName);
	}

	internal void ApplyAuthoritativeSync(Dictionary data)
	{
		if (data == null)
		{
			return;
		}
		StateMachineSnapshot snapshot = null;
		bool flag = data.ContainsKey("sm") && TryDecodeStateMachineSnapshot(data["sm"].AsGodotDictionary(), out snapshot) && CanRestoreStateMachineSnapshot(snapshot);
		bool flag2 = HasConfiguredStateMachine | flag;
		if (flag2)
		{
			BeginAuthoritativeStateRestore();
		}
		try
		{
			SyncDeserialize(data);
		}
		finally
		{
			if (flag2)
			{
				EndAuthoritativeStateRestore();
			}
		}
		if (flag)
		{
			RestoreStateMachineSnapshot(snapshot, remote: true, suppressEntryEffects: true);
		}
	}

	internal Dictionary CaptureStateMachineSnapshotData()
	{
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return new Dictionary();
		}
		return StateMachineSnapshotCodec.Encode(StateMachine.CaptureSnapshot());
	}

	internal static bool TryDecodeStateMachineSnapshot(Dictionary data, out StateMachineSnapshot snapshot)
	{
		snapshot = StateMachineSnapshotCodec.Decode(data);
		return snapshot != null;
	}

	internal bool RestoreStateMachineSnapshot(StateMachineSnapshot snapshot, bool remote, bool suppressEntryEffects, bool progressCompatible = false)
	{
		if (snapshot == null || !CanRestoreStateMachineSnapshot(snapshot, progressCompatible))
		{
			return false;
		}
		if (remote && snapshot.Revision < _lastRemoteStateMachineRevision)
		{
			return true;
		}
		if (IsStateMachineRegistered)
		{
			IStateMachineController stateMachine = StateMachine;
			if (stateMachine != null && stateMachine.IsInitialized)
			{
				return ApplyStateMachineSnapshot(snapshot, remote, suppressEntryEffects);
			}
		}
		if (_pendingStateMachineSnapshot == null || !remote || !_pendingStateMachineSnapshotIsRemote || snapshot.Revision >= _pendingStateMachineSnapshot.Revision)
		{
			_pendingStateMachineSnapshot = snapshot;
			_pendingStateMachineSnapshotIsRemote = remote;
			_pendingStateMachineSnapshotSuppressEffects = suppressEntryEffects;
		}
		return true;
	}

	internal bool CanRestoreStateMachineSnapshot(StateMachineSnapshot snapshot, bool progressCompatible = false)
	{
		if (snapshot == null || !EnsureStateMachineInitialized())
		{
			return false;
		}
		if (progressCompatible)
		{
			StateMachineSnapshot stateMachineSnapshot = StateMachine.CaptureSnapshot();
			if (stateMachineSnapshot == null || !string.Equals(snapshot.DefinitionId, stateMachineSnapshot.DefinitionId, StringComparison.Ordinal) || snapshot.SchemaVersion != stateMachineSnapshot.SchemaVersion)
			{
				return false;
			}
			snapshot.ContentHash = stateMachineSnapshot.ContentHash;
		}
		return StateMachine.CanRestoreSnapshot(snapshot);
	}

	public virtual void SetAlive(bool _alive)
	{
		bool flag = this._alive != _alive;
		this._alive = _alive;
		SetPhysicsProcess(_alive && _sharedPhysicsBatchWorkEnabled);
		ConfigureSharedPhysicsBatch();
		if (flag && GodotObject.IsInstanceValid(_stateMachineManager))
		{
			_stateMachineManager.NotifyComponentAliveChanged(this);
		}
	}

	public bool SendStateEvent(StringName eventName, bool allowWhenInactive = false)
	{
		if (!IsStateMachineRegistered || _authoritativeStateRestoreDepth > 0 || !CanDispatchStateMachineWork || (!allowWhenInactive && !alive))
		{
			return false;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			return StateMachine.SendEvent(eventName);
		}
		return false;
	}

	protected virtual void OnStateRuntimeAttached()
	{
	}

	protected virtual void OnStateRuntimeRegistered()
	{
	}

	protected virtual void OnStateRuntimeDetaching()
	{
	}

	protected virtual void OnAuthoritativeStateRestorePreparing(bool remote)
	{
	}

	protected virtual void OnAuthoritativeStateRestored(StateMachineSnapshot snapshot, bool remote)
	{
	}

	protected void RefreshStateMachineDispatchEligibility()
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager))
		{
			_stateMachineManager.RefreshStateMachineRegistration(this);
		}
	}

	internal void BeginAuthoritativeStateRestore()
	{
		_authoritativeStateRestoreDepth++;
	}

	internal void EndAuthoritativeStateRestore()
	{
		if (_authoritativeStateRestoreDepth > 0)
		{
			_authoritativeStateRestoreDepth--;
		}
	}

	internal void AttachToManager(ComponentManager manager)
	{
		if (!GodotObject.IsInstanceValid(manager) || !IsInsideTree() || GetParent() != manager.parent)
		{
			return;
		}
		if (_stateMachineManager == manager && _stateRuntimeAttached)
		{
			manager.RefreshStateMachineRegistration(this);
			return;
		}
		if (GodotObject.IsInstanceValid(_stateMachineManager) && _stateMachineManager != manager)
		{
			_stateMachineManager.RefreshStateMachineRegistration(this);
		}
		_stateMachineManager = manager;
		AttachStateRuntime();
	}

	internal void DetachFromManager(ComponentManager manager)
	{
		if (_stateMachineManager == manager)
		{
			_stateMachineManager = null;
		}
	}

	private void AttachStateRuntime()
	{
		ComponentManager stateMachineManager = _stateMachineManager;
		if (!GodotObject.IsInstanceValid(stateMachineManager))
		{
			return;
		}
		if (!EnsureStateMachineInitialized())
		{
			stateMachineManager.RefreshStateMachineRegistration(this);
			return;
		}
		if (!_stateRuntimeAttached)
		{
			OnStateRuntimeAttached();
			_stateRuntimeAttached = true;
		}
		stateMachineManager.RefreshStateMachineRegistration(this);
		if (!_stateMachineInitialEntered)
		{
			_stateMachineInitialEntered = true;
			if (_pendingStateMachineSnapshot != null)
			{
				ApplyPendingStateMachineSnapshot();
			}
			else
			{
				StateMachine.EnterInitialState();
			}
		}
		else
		{
			ApplyPendingStateMachineSnapshot();
		}
		OnStateRuntimeRegistered();
	}

	private void ApplyPendingStateMachineSnapshot()
	{
		if (_pendingStateMachineSnapshot != null)
		{
			IStateMachineController stateMachine = StateMachine;
			if (stateMachine != null && stateMachine.IsInitialized)
			{
				StateMachineSnapshot pendingStateMachineSnapshot = _pendingStateMachineSnapshot;
				bool pendingStateMachineSnapshotIsRemote = _pendingStateMachineSnapshotIsRemote;
				bool pendingStateMachineSnapshotSuppressEffects = _pendingStateMachineSnapshotSuppressEffects;
				_pendingStateMachineSnapshot = null;
				_pendingStateMachineSnapshotIsRemote = false;
				_pendingStateMachineSnapshotSuppressEffects = false;
				ApplyStateMachineSnapshot(pendingStateMachineSnapshot, pendingStateMachineSnapshotIsRemote, pendingStateMachineSnapshotSuppressEffects);
			}
		}
	}

	private bool ApplyStateMachineSnapshot(StateMachineSnapshot snapshot, bool remote, bool suppressEntryEffects)
	{
		if (remote && snapshot.Revision < _lastRemoteStateMachineRevision)
		{
			return true;
		}
		OnAuthoritativeStateRestorePreparing(remote);
		bool flag = StateMachine.RestoreSnapshot(snapshot, suppressEntryEffects);
		if (flag & remote)
		{
			_lastRemoteStateMachineRevision = Math.Max(_lastRemoteStateMachineRevision, snapshot.Revision);
		}
		if (flag)
		{
			OnAuthoritativeStateRestored(snapshot, remote);
		}
		return flag;
	}

	private bool EnsureStateMachineInitialized()
	{
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			return true;
		}
		if (StateMachineDefinition == null)
		{
			return false;
		}
		_stateMachineDisposed = false;
		StateMachine = new StateMachineController();
		if (StateMachine.Initialize(StateMachineDefinition, this))
		{
			return true;
		}
		GD.PushError($"Component '{Name}' state machine initialization failed: {StateMachine.InitializationError}");
		StateMachine.Dispose();
		StateMachine = null;
		return false;
	}

	private void DisposeStateMachine()
	{
		if (!_stateMachineDisposed || StateMachine != null)
		{
			_stateMachineDisposed = true;
			if (_stateRuntimeAttached)
			{
				OnStateRuntimeDetaching();
				_stateRuntimeAttached = false;
			}
			_stateMachineInitialEntered = false;
			StateMachine?.Dispose();
			StateMachine = null;
			if (GodotObject.IsInstanceValid(_stateMachineManager))
			{
				_stateMachineManager.RefreshStateMachineRegistration(this);
			}
		}
	}

	internal bool TryGetWireSlotToken(ulong managerInstanceId, string typeName, out int typeIndex)
	{
		typeIndex = _wireTypeIndex;
		if (_wireManagerInstanceId == managerInstanceId && _wireTypeIndex >= 0)
		{
			return string.Equals(_wireTypeName, typeName, StringComparison.Ordinal);
		}
		return false;
	}

	internal void SetWireSlotToken(ulong managerInstanceId, string typeName, int typeIndex)
	{
		_wireManagerInstanceId = managerInstanceId;
		_wireTypeName = typeName ?? string.Empty;
		_wireTypeIndex = typeIndex;
	}

	public override void _Notification(int what)
	{
		if ((long)what == 1)
		{
			DisposeStateMachine();
		}
	}

	private void ConfigureSharedPhysicsBatch()
	{
		if (!UseSharedPhysicsBatch || Engine.IsEditorHint() || !IsInsideTree())
		{
			return;
		}
		if (_alive && _sharedPhysicsBatchWorkEnabled)
		{
			_usingSharedPhysicsBatch = ComponentPhysicsBatch.Register(this);
			if (_usingSharedPhysicsBatch)
			{
				SetPhysicsProcess(enable: false);
			}
			return;
		}
		if (_usingSharedPhysicsBatch)
		{
			ComponentPhysicsBatch.Unregister(this);
			_usingSharedPhysicsBatch = false;
		}
		SetPhysicsProcess(enable: false);
	}

	protected T GetParentCharacter<T>() where T : class
	{
		if (GodotObject.IsInstanceValid(_stateMachineManager?.parent))
		{
			return _stateMachineManager.parent as T;
		}
		Node parent = GetParent();
		if (parent is T result)
		{
			return result;
		}
		return parent?.GetParent() as T;
	}

	protected static bool SyncForceState(IStateMachineController state, string targetState)
	{
		if (state == null || !state.IsInitialized || string.IsNullOrEmpty(targetState))
		{
			return false;
		}
		StateHandle currentStateHandle = state.CurrentStateHandle;
		StateHandle stateHandle = state.ResolveState(targetState, allowDisplayNameFallback: true);
		if (currentStateHandle == null || !currentStateHandle.IsValid || stateHandle == null || !stateHandle.IsValid)
		{
			return false;
		}
		if (currentStateHandle.StableId == stateHandle.StableId)
		{
			return true;
		}
		StateMachineSnapshot stateMachineSnapshot = state.CaptureSnapshot();
		if (stateMachineSnapshot == null)
		{
			return false;
		}
		stateMachineSnapshot.ActiveStateIds.Clear();
		stateMachineSnapshot.ActiveStateIds.Add(stateHandle.StableId);
		stateMachineSnapshot.PendingTransitionIds.Clear();
		stateMachineSnapshot.PendingDelayRemaining.Clear();
		return state.RestoreSnapshot(stateMachineSnapshot);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(42)
		{
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSharedBatchTreeExiting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareSharedBatchRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SharedBatchPhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSharedPhysicsBatchWorkEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportComponentSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportComponentSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartComponentTimer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useOwnerTimeScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopComponentTimer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PauseComponentTimer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResumeComponentTimer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsComponentTimerRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetComponentTimerRemaining, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnComponentTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchComponentTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyAuthoritativeSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureStateMachineSnapshotData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreStateMachineSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressEntryEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "progressCompatible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanRestoreStateMachineSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "progressCompatible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAlive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_alive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendStateEvent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "allowWhenInactive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnStateRuntimeAttached, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStateRuntimeRegistered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStateRuntimeDetaching, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAuthoritativeStateRestorePreparing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAuthoritativeStateRestored, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshStateMachineDispatchEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginAuthoritativeStateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndAuthoritativeStateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttachToManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DetachFromManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AttachStateRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingStateMachineSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyStateMachineSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "remote", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressEntryEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureStateMachineInitialized, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeStateMachine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetWireSlotToken, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "managerInstanceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "typeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureSharedPhysicsBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSharedBatchTreeExiting && args.Count == 0)
		{
			OnSharedBatchTreeExiting();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSharedBatchRegistration && args.Count == 0)
		{
			PrepareSharedBatchRegistration();
			ret = default;
			return true;
		}
		if (method == MethodName.SharedBatchPhysicsProcess && args.Count == 1)
		{
			SharedBatchPhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSharedPhysicsBatchWorkEnabled && args.Count == 1)
		{
			SetSharedPhysicsBatchWorkEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName.ExportComponentSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportComponentSave());
			return true;
		}
		if (method == MethodName.ImportComponentSave && args.Count == 2)
		{
			ImportComponentSave(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartComponentTimer && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(StartComponentTimer(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.StopComponentTimer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(StopComponentTimer(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.PauseComponentTimer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PauseComponentTimer(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.ResumeComponentTimer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ResumeComponentTimer(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsComponentTimerRunning && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComponentTimerRunning(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetComponentTimerRemaining && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetComponentTimerRemaining(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.OnComponentTimerTimeout && args.Count == 1)
		{
			OnComponentTimerTimeout(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchComponentTimerTimeout && args.Count == 1)
		{
			DispatchComponentTimerTimeout(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAuthoritativeSync && args.Count == 1)
		{
			ApplyAuthoritativeSync(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureStateMachineSnapshotData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureStateMachineSnapshotData());
			return true;
		}
		if (method == MethodName.RestoreStateMachineSnapshot && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreStateMachineSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.CanRestoreStateMachineSnapshot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanRestoreStateMachineSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.SetAlive && args.Count == 1)
		{
			SetAlive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendStateEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SendStateEvent(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.OnStateRuntimeAttached && args.Count == 0)
		{
			OnStateRuntimeAttached();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStateRuntimeRegistered && args.Count == 0)
		{
			OnStateRuntimeRegistered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnStateRuntimeDetaching && args.Count == 0)
		{
			OnStateRuntimeDetaching();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAuthoritativeStateRestorePreparing && args.Count == 1)
		{
			OnAuthoritativeStateRestorePreparing(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAuthoritativeStateRestored && args.Count == 2)
		{
			OnAuthoritativeStateRestored(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStateMachineDispatchEligibility && args.Count == 0)
		{
			RefreshStateMachineDispatchEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginAuthoritativeStateRestore && args.Count == 0)
		{
			BeginAuthoritativeStateRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.EndAuthoritativeStateRestore && args.Count == 0)
		{
			EndAuthoritativeStateRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.AttachToManager && args.Count == 1)
		{
			AttachToManager(VariantUtils.ConvertTo<ComponentManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachFromManager && args.Count == 1)
		{
			DetachFromManager(VariantUtils.ConvertTo<ComponentManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttachStateRuntime && args.Count == 0)
		{
			AttachStateRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingStateMachineSnapshot && args.Count == 0)
		{
			ApplyPendingStateMachineSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyStateMachineSnapshot && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplyStateMachineSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.EnsureStateMachineInitialized && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureStateMachineInitialized());
			return true;
		}
		if (method == MethodName.DisposeStateMachine && args.Count == 0)
		{
			DisposeStateMachine();
			ret = default;
			return true;
		}
		if (method == MethodName.SetWireSlotToken && args.Count == 3)
		{
			SetWireSlotToken(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureSharedPhysicsBatch && args.Count == 0)
		{
			ConfigureSharedPhysicsBatch();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName.OnSharedBatchTreeExiting)
		{
			return true;
		}
		if (method == MethodName.PrepareSharedBatchRegistration)
		{
			return true;
		}
		if (method == MethodName.SharedBatchPhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SetSharedPhysicsBatchWorkEnabled)
		{
			return true;
		}
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.ExportComponentSave)
		{
			return true;
		}
		if (method == MethodName.ImportComponentSave)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.StartComponentTimer)
		{
			return true;
		}
		if (method == MethodName.StopComponentTimer)
		{
			return true;
		}
		if (method == MethodName.PauseComponentTimer)
		{
			return true;
		}
		if (method == MethodName.ResumeComponentTimer)
		{
			return true;
		}
		if (method == MethodName.IsComponentTimerRunning)
		{
			return true;
		}
		if (method == MethodName.GetComponentTimerRemaining)
		{
			return true;
		}
		if (method == MethodName.OnComponentTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.DispatchComponentTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.ApplyAuthoritativeSync)
		{
			return true;
		}
		if (method == MethodName.CaptureStateMachineSnapshotData)
		{
			return true;
		}
		if (method == MethodName.RestoreStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.CanRestoreStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.SetAlive)
		{
			return true;
		}
		if (method == MethodName.SendStateEvent)
		{
			return true;
		}
		if (method == MethodName.OnStateRuntimeAttached)
		{
			return true;
		}
		if (method == MethodName.OnStateRuntimeRegistered)
		{
			return true;
		}
		if (method == MethodName.OnStateRuntimeDetaching)
		{
			return true;
		}
		if (method == MethodName.OnAuthoritativeStateRestorePreparing)
		{
			return true;
		}
		if (method == MethodName.OnAuthoritativeStateRestored)
		{
			return true;
		}
		if (method == MethodName.RefreshStateMachineDispatchEligibility)
		{
			return true;
		}
		if (method == MethodName.BeginAuthoritativeStateRestore)
		{
			return true;
		}
		if (method == MethodName.EndAuthoritativeStateRestore)
		{
			return true;
		}
		if (method == MethodName.AttachToManager)
		{
			return true;
		}
		if (method == MethodName.DetachFromManager)
		{
			return true;
		}
		if (method == MethodName.AttachStateRuntime)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.ApplyStateMachineSnapshot)
		{
			return true;
		}
		if (method == MethodName.EnsureStateMachineInitialized)
		{
			return true;
		}
		if (method == MethodName.DisposeStateMachine)
		{
			return true;
		}
		if (method == MethodName.SetWireSlotToken)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.ConfigureSharedPhysicsBatch)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.alive)
		{
			alive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.StateMachineDefinition)
		{
			StateMachineDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName._alive)
		{
			_alive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateMachineDefinition)
		{
			_stateMachineDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName._stateMachineManager)
		{
			_stateMachineManager = VariantUtils.ConvertTo<ComponentManager>(in value);
			return true;
		}
		if (name == PropertyName._stateRuntimeAttached)
		{
			_stateRuntimeAttached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateMachineInitialEntered)
		{
			_stateMachineInitialEntered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateMachineDisposed)
		{
			_stateMachineDisposed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._authoritativeStateRestoreDepth)
		{
			_authoritativeStateRestoreDepth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pendingStateMachineSnapshot)
		{
			_pendingStateMachineSnapshot = VariantUtils.ConvertTo<StateMachineSnapshot>(in value);
			return true;
		}
		if (name == PropertyName._pendingStateMachineSnapshotIsRemote)
		{
			_pendingStateMachineSnapshotIsRemote = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingStateMachineSnapshotSuppressEffects)
		{
			_pendingStateMachineSnapshotSuppressEffects = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastRemoteStateMachineRevision)
		{
			_lastRemoteStateMachineRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._wireManagerInstanceId)
		{
			_wireManagerInstanceId = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._wireTypeName)
		{
			_wireTypeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._wireTypeIndex)
		{
			_wireTypeIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._usingSharedPhysicsBatch)
		{
			_usingSharedPhysicsBatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sharedPhysicsBatchWorkEnabled)
		{
			_sharedPhysicsBatchWorkEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sharedBatchUsesInheritedProcessMode)
		{
			_sharedBatchUsesInheritedProcessMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sharedBatchTreeExitConnected)
		{
			_sharedBatchTreeExitConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.alive)
		{
			from = alive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsSharedBatchDispatchActive)
		{
			from = IsSharedBatchDispatchActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SharedBatchUsesInheritedProcessMode)
		{
			from = SharedBatchUsesInheritedProcessMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.UseSharedPhysicsBatch)
		{
			from = UseSharedPhysicsBatch;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AllowPhysicsOutsideComponentBattlefield)
		{
			from = AllowPhysicsOutsideComponentBattlefield;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AllowStateMachineOutsideComponentBattlefield)
		{
			from = AllowStateMachineOutsideComponentBattlefield;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsInsideComponentBattlefield)
		{
			from = IsInsideComponentBattlefield;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CanDispatchPhysicsWork)
		{
			from = CanDispatchPhysicsWork;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CanDispatchStateMachineWork)
		{
			from = CanDispatchStateMachineWork;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.StateMachineDefinition)
		{
			value = VariantUtils.CreateFrom<StateMachineDefinition>(StateMachineDefinition);
			return true;
		}
		if (name == PropertyName.IsStateMachineDispatchEligible)
		{
			from = IsStateMachineDispatchEligible;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsStateMachineRegistered)
		{
			from = IsStateMachineRegistered;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasStateMachine)
		{
			from = HasStateMachine;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasConfiguredStateMachine)
		{
			from = HasConfiguredStateMachine;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasStateMachineProcessWork)
		{
			from = HasStateMachineProcessWork;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasStateMachinePhysicsWork)
		{
			from = HasStateMachinePhysicsWork;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ShouldApplyLegacyStateField)
		{
			from = ShouldApplyLegacyStateField;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._alive)
		{
			value = VariantUtils.CreateFrom(in _alive);
			return true;
		}
		if (name == PropertyName._stateMachineDefinition)
		{
			value = VariantUtils.CreateFrom(in _stateMachineDefinition);
			return true;
		}
		if (name == PropertyName._stateMachineManager)
		{
			value = VariantUtils.CreateFrom(in _stateMachineManager);
			return true;
		}
		if (name == PropertyName._stateRuntimeAttached)
		{
			value = VariantUtils.CreateFrom(in _stateRuntimeAttached);
			return true;
		}
		if (name == PropertyName._stateMachineInitialEntered)
		{
			value = VariantUtils.CreateFrom(in _stateMachineInitialEntered);
			return true;
		}
		if (name == PropertyName._stateMachineDisposed)
		{
			value = VariantUtils.CreateFrom(in _stateMachineDisposed);
			return true;
		}
		if (name == PropertyName._authoritativeStateRestoreDepth)
		{
			value = VariantUtils.CreateFrom(in _authoritativeStateRestoreDepth);
			return true;
		}
		if (name == PropertyName._pendingStateMachineSnapshot)
		{
			value = VariantUtils.CreateFrom(in _pendingStateMachineSnapshot);
			return true;
		}
		if (name == PropertyName._pendingStateMachineSnapshotIsRemote)
		{
			value = VariantUtils.CreateFrom(in _pendingStateMachineSnapshotIsRemote);
			return true;
		}
		if (name == PropertyName._pendingStateMachineSnapshotSuppressEffects)
		{
			value = VariantUtils.CreateFrom(in _pendingStateMachineSnapshotSuppressEffects);
			return true;
		}
		if (name == PropertyName._lastRemoteStateMachineRevision)
		{
			value = VariantUtils.CreateFrom(in _lastRemoteStateMachineRevision);
			return true;
		}
		if (name == PropertyName._wireManagerInstanceId)
		{
			value = VariantUtils.CreateFrom(in _wireManagerInstanceId);
			return true;
		}
		if (name == PropertyName._wireTypeName)
		{
			value = VariantUtils.CreateFrom(in _wireTypeName);
			return true;
		}
		if (name == PropertyName._wireTypeIndex)
		{
			value = VariantUtils.CreateFrom(in _wireTypeIndex);
			return true;
		}
		if (name == PropertyName._usingSharedPhysicsBatch)
		{
			value = VariantUtils.CreateFrom(in _usingSharedPhysicsBatch);
			return true;
		}
		if (name == PropertyName._sharedPhysicsBatchWorkEnabled)
		{
			value = VariantUtils.CreateFrom(in _sharedPhysicsBatchWorkEnabled);
			return true;
		}
		if (name == PropertyName._sharedBatchUsesInheritedProcessMode)
		{
			value = VariantUtils.CreateFrom(in _sharedBatchUsesInheritedProcessMode);
			return true;
		}
		if (name == PropertyName._sharedBatchTreeExitConnected)
		{
			value = VariantUtils.CreateFrom(in _sharedBatchTreeExitConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.alive, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._alive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stateMachineDefinition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stateMachineManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateRuntimeAttached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateMachineInitialEntered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateMachineDisposed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._authoritativeStateRestoreDepth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingStateMachineSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingStateMachineSnapshotIsRemote, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingStateMachineSnapshotSuppressEffects, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastRemoteStateMachineRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._wireManagerInstanceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._wireTypeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._wireTypeIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._usingSharedPhysicsBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sharedPhysicsBatchWorkEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sharedBatchUsesInheritedProcessMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sharedBatchTreeExitConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSharedBatchDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.SharedBatchUsesInheritedProcessMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseSharedPhysicsBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AllowPhysicsOutsideComponentBattlefield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AllowStateMachineOutsideComponentBattlefield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInsideComponentBattlefield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanDispatchPhysicsWork, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CanDispatchStateMachineWork, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.StateMachineDefinition, PropertyHint.ResourceType, "StateMachineDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsStateMachineDispatchEligible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsStateMachineRegistered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasStateMachine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasConfiguredStateMachine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasStateMachineProcessWork, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasStateMachinePhysicsWork, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ShouldApplyLegacyStateField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.alive, Variant.From<bool>(alive));
		info.AddProperty(PropertyName.StateMachineDefinition, Variant.From<StateMachineDefinition>(StateMachineDefinition));
		info.AddProperty(PropertyName._alive, Variant.From(in _alive));
		info.AddProperty(PropertyName._stateMachineDefinition, Variant.From(in _stateMachineDefinition));
		info.AddProperty(PropertyName._stateMachineManager, Variant.From(in _stateMachineManager));
		info.AddProperty(PropertyName._stateRuntimeAttached, Variant.From(in _stateRuntimeAttached));
		info.AddProperty(PropertyName._stateMachineInitialEntered, Variant.From(in _stateMachineInitialEntered));
		info.AddProperty(PropertyName._stateMachineDisposed, Variant.From(in _stateMachineDisposed));
		info.AddProperty(PropertyName._authoritativeStateRestoreDepth, Variant.From(in _authoritativeStateRestoreDepth));
		info.AddProperty(PropertyName._pendingStateMachineSnapshot, Variant.From(in _pendingStateMachineSnapshot));
		info.AddProperty(PropertyName._pendingStateMachineSnapshotIsRemote, Variant.From(in _pendingStateMachineSnapshotIsRemote));
		info.AddProperty(PropertyName._pendingStateMachineSnapshotSuppressEffects, Variant.From(in _pendingStateMachineSnapshotSuppressEffects));
		info.AddProperty(PropertyName._lastRemoteStateMachineRevision, Variant.From(in _lastRemoteStateMachineRevision));
		info.AddProperty(PropertyName._wireManagerInstanceId, Variant.From(in _wireManagerInstanceId));
		info.AddProperty(PropertyName._wireTypeName, Variant.From(in _wireTypeName));
		info.AddProperty(PropertyName._wireTypeIndex, Variant.From(in _wireTypeIndex));
		info.AddProperty(PropertyName._usingSharedPhysicsBatch, Variant.From(in _usingSharedPhysicsBatch));
		info.AddProperty(PropertyName._sharedPhysicsBatchWorkEnabled, Variant.From(in _sharedPhysicsBatchWorkEnabled));
		info.AddProperty(PropertyName._sharedBatchUsesInheritedProcessMode, Variant.From(in _sharedBatchUsesInheritedProcessMode));
		info.AddProperty(PropertyName._sharedBatchTreeExitConnected, Variant.From(in _sharedBatchTreeExitConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.alive, out var value))
		{
			alive = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.StateMachineDefinition, out var value2))
		{
			StateMachineDefinition = value2.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName._alive, out var value3))
		{
			_alive = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateMachineDefinition, out var value4))
		{
			_stateMachineDefinition = value4.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName._stateMachineManager, out var value5))
		{
			_stateMachineManager = value5.As<ComponentManager>();
		}
		if (info.TryGetProperty(PropertyName._stateRuntimeAttached, out var value6))
		{
			_stateRuntimeAttached = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateMachineInitialEntered, out var value7))
		{
			_stateMachineInitialEntered = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateMachineDisposed, out var value8))
		{
			_stateMachineDisposed = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._authoritativeStateRestoreDepth, out var value9))
		{
			_authoritativeStateRestoreDepth = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pendingStateMachineSnapshot, out var value10))
		{
			_pendingStateMachineSnapshot = value10.As<StateMachineSnapshot>();
		}
		if (info.TryGetProperty(PropertyName._pendingStateMachineSnapshotIsRemote, out var value11))
		{
			_pendingStateMachineSnapshotIsRemote = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingStateMachineSnapshotSuppressEffects, out var value12))
		{
			_pendingStateMachineSnapshotSuppressEffects = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastRemoteStateMachineRevision, out var value13))
		{
			_lastRemoteStateMachineRevision = value13.As<long>();
		}
		if (info.TryGetProperty(PropertyName._wireManagerInstanceId, out var value14))
		{
			_wireManagerInstanceId = value14.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._wireTypeName, out var value15))
		{
			_wireTypeName = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName._wireTypeIndex, out var value16))
		{
			_wireTypeIndex = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._usingSharedPhysicsBatch, out var value17))
		{
			_usingSharedPhysicsBatch = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sharedPhysicsBatchWorkEnabled, out var value18))
		{
			_sharedPhysicsBatchWorkEnabled = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sharedBatchUsesInheritedProcessMode, out var value19))
		{
			_sharedBatchUsesInheritedProcessMode = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sharedBatchTreeExitConnected, out var value20))
		{
			_sharedBatchTreeExitConnected = value20.As<bool>();
		}
	}
}
