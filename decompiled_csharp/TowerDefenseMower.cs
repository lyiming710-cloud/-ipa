using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseMower.cs")]
public class TowerDefenseMower : TowerDefenseItem
{
	public delegate void RunningEventHandler(TowerDefenseMower mower);

	public new class MethodName : TowerDefenseItem.MethodName
	{
		public static readonly StringName CanStartRun = "CanStartRun";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public static readonly StringName ConnectRunStateHandle = "ConnectRunStateHandle";

		public static readonly StringName DisconnectRunStateHandle = "DisconnectRunStateHandle";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName RunEntered = "RunEntered";

		public static readonly StringName RunProcessing = "RunProcessing";

		public static readonly StringName RunExited = "RunExited";

		public static readonly StringName HitCheck = "HitCheck";

		public static readonly StringName ProcessAabbHits = "ProcessAabbHits";

		public static readonly StringName Hit = "Hit";

		public static readonly StringName Run = "Run";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName BlowBack = "BlowBack";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName runAnimeClips = "runAnimeClips";

		public static readonly StringName runWaterAnimeClips = "runWaterAnimeClips";

		public static readonly StringName attackAnimeClips = "attackAnimeClips";

		public static readonly StringName attackWaterAnimeClips = "attackWaterAnimeClips";

		public static readonly StringName _runStateHandleConnected = "_runStateHandleConnected";

		public static readonly StringName _gridSize = "_gridSize";

		public static readonly StringName _isIZM2Mode = "_isIZM2Mode";

		public static readonly StringName run = "run";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private static PackedScene _MOWER_SPAWN;

	[Export(PropertyHint.None, "")]
	public string runAnimeClips = "Normal";

	[Export(PropertyHint.None, "")]
	public string runWaterAnimeClips = "Normal";

	[Export(PropertyHint.None, "")]
	public string attackAnimeClips = "";

	[Export(PropertyHint.None, "")]
	public string attackWaterAnimeClips = "";

	public CharacterMoveComponent moveComponent;

	public MowerHitComponent mowerHitComponent;

	private StateHandle _runStateHandle;

	private bool _runStateHandleConnected;

	private Vector2 _gridSize;

	private bool _isIZM2Mode;

	public bool run;

	private static PackedScene MOWER_SPAWN => _MOWER_SPAWN ?? (_MOWER_SPAWN = GD.Load<PackedScene>("uid://dy8bwagg440x0"));

	public event RunningEventHandler OnRunning;

	public bool CanStartRun()
	{
		if (TowerDefenseManager.Instance == null)
		{
			return false;
		}
		return TowerDefenseManager.Instance.IsGameRunning();
	}

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base._Ready();
		if (!editorPreviewMode)
		{
			if (GodotObject.IsInstanceValid(componentManager))
			{
				moveComponent = componentManager.GetRuntime<CharacterMoveComponent>();
				mowerHitComponent = componentManager.GetRuntime<MowerHitComponent>();
			}
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.SetAlive(alive: false);
			}
			_gridSize = TowerDefenseManager.Instance.GetMapGridSize();
			_isIZM2Mode = TowerDefenseManager.Instance.IsIZM2Mode();
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(MOWER_SPAWN, gridPos);
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint);
			sprite.pause = true;
			instance.invincible = true;
			AddToGroup("Mower", persistent: true);
			ConnectRunStateHandle();
			ActivateGameplayProcessing();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void ActivateGameplayProcessing()
	{
		base.ActivateGameplayProcessing();
		if (!run && GodotObject.IsInstanceValid(sprite))
		{
			sprite.pause = true;
		}
	}

	private void ConnectRunStateHandle()
	{
		if (_runStateHandleConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_runStateHandle = StateMachine?.GetStateById("mower.run");
			StateHandle runStateHandle = _runStateHandle;
			if (runStateHandle != null && runStateHandle.IsValid)
			{
				_runStateHandle.Entered += RunEntered;
				_runStateHandle.Exited += RunExited;
				_runStateHandle.PhysicsProcessing += RunProcessing;
				_runStateHandleConnected = true;
			}
		}
	}

	private void DisconnectRunStateHandle()
	{
		if (_runStateHandleConnected && _runStateHandle != null)
		{
			_runStateHandle.Entered -= RunEntered;
			_runStateHandle.Exited -= RunExited;
			_runStateHandle.PhysicsProcessing -= RunProcessing;
		}
		_runStateHandle = null;
		_runStateHandleConnected = false;
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (TowerDefenseCharacter.CachedEditorHint)
		{
			return;
		}
		ProcessAabbHits();
		if (!run)
		{
			return;
		}
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (globalPositionForPhysicsFrame.X > mapFeature.config.edge.Z)
		{
			Destroy();
		}
		if (_isIZM2Mode && globalPositionForPhysicsFrame.X > TowerDefenseManager.Instance.GetMapCellPos(new Vector2I((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 3f) + 1, 0)).X)
		{
			TowerDefenseManager.Instance.CreateMower(gridPos.Y);
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(MOWER_SPAWN, gridPos);
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint);
			Destroy();
		}
		if (mapFeature != null)
		{
			if (GodotObject.IsInstanceValid(cell))
			{
				Vector2 mapCellPos = TowerDefenseManager.Instance.GetMapCellPos(gridPos);
				cellPercentage = (globalPositionForPhysicsFrame - mapCellPos).X / _gridSize.X;
				inWater = cell.isWater;
			}
			else
			{
				inWater = false;
			}
		}
		if (!inWater && GodotObject.IsInstanceValid(cell))
		{
			double num = cell.GetGroundHeight(cellPercentage);
			if (Mathf.Abs(groundHeight - num) > 0.1)
			{
				groundHeight = Mathf.Lerp((float)groundHeight, (float)num, (float)(3.0 * delta));
			}
			else
			{
				groundHeight = num;
			}
		}
	}

	public void RunEntered()
	{
		OnRunning?.Invoke(this);
		CharacterMoveComponent characterMoveComponent = moveComponent;
		if (characterMoveComponent != null && !characterMoveComponent.IsReleased)
		{
			moveComponent.SetVelocity(Vector2.Right * 200f);
		}
		sprite.pause = false;
		AudioManager.Instance.AudioPlay("Mower");
		sprite.SetAnimation("Normal");
	}

	public void RunProcessing(double delta)
	{
		sprite.timeScale = timeScale * 3.0;
		if (CanSleep())
		{
			Sleep();
		}
	}

	public void RunExited()
	{
	}

	public void HitCheck(AabbArea2D area)
	{
		MowerHitComponent mowerHitComponent = this.mowerHitComponent;
		if (mowerHitComponent != null && !mowerHitComponent.IsReleased)
		{
			this.mowerHitComponent.HitCheck(area);
		}
	}

	private void ProcessAabbHits()
	{
		if (!CanStartRun() || !IsHitBoxEnabled)
		{
			return;
		}
		MowerHitComponent mowerHitComponent = this.mowerHitComponent;
		if (mowerHitComponent != null && !mowerHitComponent.IsReleased && TowerDefenseManager.Instance != null && TowerDefenseManager.Instance.characterRegistry != null)
		{
			Rect2 worldHitRect = WorldHitRect;
			List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(worldHitRect, gridPos.Y, includeAllLineCheck: true);
			for (int i = 0; i < charactersIntersectingRectList.Count; i++)
			{
				this.mowerHitComponent.HitCheckCharacter(charactersIntersectingRectList[i]);
			}
		}
	}

	public void Hit(TowerDefenseCharacter character)
	{
		MowerHitComponent mowerHitComponent = this.mowerHitComponent;
		if (mowerHitComponent != null && !mowerHitComponent.IsReleased)
		{
			this.mowerHitComponent.Hit(character);
		}
	}

	public void Run()
	{
		if (CanStartRun())
		{
			ActivateGameplayProcessing();
			SendStateEvent("ToRun");
		}
	}

	public override void InWater()
	{
		base.InWater();
		if (runWaterAnimeClips != "")
		{
			sprite.SetAnimation(runWaterAnimeClips, loop: true, 0.1);
		}
	}

	public override void OutWater()
	{
		base.OutWater();
		if (runAnimeClips != "")
		{
			sprite.SetAnimation(runAnimeClips, loop: true, 0.1);
		}
	}

	public override void BlowBack(double num, double time = 1.0)
	{
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["run"] = run };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		run = data.GetValueOrDefault("run", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.CanStartRun, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRunStateHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRunStateHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "area", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessAabbHits, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlowBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.CanStartRun && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanStartRun());
			return true;
		}
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
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectRunStateHandle && args.Count == 0)
		{
			ConnectRunStateHandle();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRunStateHandle && args.Count == 0)
		{
			DisconnectRunStateHandle();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunEntered && args.Count == 0)
		{
			RunEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RunProcessing && args.Count == 1)
		{
			RunProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunExited && args.Count == 0)
		{
			RunExited();
			ret = default;
			return true;
		}
		if (method == MethodName.HitCheck && args.Count == 1)
		{
			HitCheck(VariantUtils.ConvertTo<AabbArea2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessAabbHits && args.Count == 0)
		{
			ProcessAabbHits();
			ret = default;
			return true;
		}
		if (method == MethodName.Hit && args.Count == 1)
		{
			Hit(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 0)
		{
			Run();
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.BlowBack && args.Count == 2)
		{
			BlowBack(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CanStartRun)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.ConnectRunStateHandle)
		{
			return true;
		}
		if (method == MethodName.DisconnectRunStateHandle)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.RunEntered)
		{
			return true;
		}
		if (method == MethodName.RunProcessing)
		{
			return true;
		}
		if (method == MethodName.RunExited)
		{
			return true;
		}
		if (method == MethodName.HitCheck)
		{
			return true;
		}
		if (method == MethodName.ProcessAabbHits)
		{
			return true;
		}
		if (method == MethodName.Hit)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.BlowBack)
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
		if (name == PropertyName.runAnimeClips)
		{
			runAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.runWaterAnimeClips)
		{
			runWaterAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackAnimeClips)
		{
			attackAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackWaterAnimeClips)
		{
			attackWaterAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._runStateHandleConnected)
		{
			_runStateHandleConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gridSize)
		{
			_gridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._isIZM2Mode)
		{
			_isIZM2Mode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.run)
		{
			run = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.runAnimeClips)
		{
			value = VariantUtils.CreateFrom(in runAnimeClips);
			return true;
		}
		if (name == PropertyName.runWaterAnimeClips)
		{
			value = VariantUtils.CreateFrom(in runWaterAnimeClips);
			return true;
		}
		if (name == PropertyName.attackAnimeClips)
		{
			value = VariantUtils.CreateFrom(in attackAnimeClips);
			return true;
		}
		if (name == PropertyName.attackWaterAnimeClips)
		{
			value = VariantUtils.CreateFrom(in attackWaterAnimeClips);
			return true;
		}
		if (name == PropertyName._runStateHandleConnected)
		{
			value = VariantUtils.CreateFrom(in _runStateHandleConnected);
			return true;
		}
		if (name == PropertyName._gridSize)
		{
			value = VariantUtils.CreateFrom(in _gridSize);
			return true;
		}
		if (name == PropertyName._isIZM2Mode)
		{
			value = VariantUtils.CreateFrom(in _isIZM2Mode);
			return true;
		}
		if (name == PropertyName.run)
		{
			value = VariantUtils.CreateFrom(in run);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.runAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.runWaterAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.attackWaterAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runStateHandleConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._gridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isIZM2Mode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.run, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.runAnimeClips, Variant.From(in runAnimeClips));
		info.AddProperty(PropertyName.runWaterAnimeClips, Variant.From(in runWaterAnimeClips));
		info.AddProperty(PropertyName.attackAnimeClips, Variant.From(in attackAnimeClips));
		info.AddProperty(PropertyName.attackWaterAnimeClips, Variant.From(in attackWaterAnimeClips));
		info.AddProperty(PropertyName._runStateHandleConnected, Variant.From(in _runStateHandleConnected));
		info.AddProperty(PropertyName._gridSize, Variant.From(in _gridSize));
		info.AddProperty(PropertyName._isIZM2Mode, Variant.From(in _isIZM2Mode));
		info.AddProperty(PropertyName.run, Variant.From(in run));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.runAnimeClips, out var value))
		{
			runAnimeClips = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.runWaterAnimeClips, out var value2))
		{
			runWaterAnimeClips = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackAnimeClips, out var value3))
		{
			attackAnimeClips = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackWaterAnimeClips, out var value4))
		{
			attackWaterAnimeClips = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._runStateHandleConnected, out var value5))
		{
			_runStateHandleConnected = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gridSize, out var value6))
		{
			_gridSize = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._isIZM2Mode, out var value7))
		{
			_isIZM2Mode = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.run, out var value8))
		{
			run = value8.As<bool>();
		}
	}
}
