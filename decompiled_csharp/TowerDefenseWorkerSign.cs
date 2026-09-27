using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/WorkerSign/Scene/TowerDefenseWorkerSign.cs")]
public class TowerDefenseWorkerSign : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public static readonly StringName BindWave = "BindWave";

		public static readonly StringName UnbindWave = "UnbindWave";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public static readonly StringName _wave = "_wave";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private TowerDefenseBattleFeatureWave _wave;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode || !inGame)
		{
			return;
		}
		RemoveFromGroup("Gravestone");
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		BindWave();
		foreach (Node item in GetTree().GetNodesInGroup("WorkerSign"))
		{
			if (item is TowerDefenseWorkerSign towerDefenseWorkerSign && towerDefenseWorkerSign != this && towerDefenseWorkerSign.inGame && !towerDefenseWorkerSign.editorPreviewMode && towerDefenseWorkerSign.gridPos.Y == gridPos.Y && !towerDefenseWorkerSign.die && !towerDefenseWorkerSign.isDestroy && !towerDefenseWorkerSign.IsQueuedForDeletion())
			{
				towerDefenseWorkerSign.Destroy();
			}
		}
	}

	public override void ActivateGameplayProcessing()
	{
		base.ActivateGameplayProcessing();
		BindWave();
	}

	private void BindWave()
	{
		if (Engine.IsEditorHint() || editorPreviewMode || !inGame || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost))
		{
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		TowerDefenseBattleFeatureWave towerDefenseBattleFeatureWave = (GodotObject.IsInstanceValid(currentControl) ? (currentControl.GetFeature("Wave") as TowerDefenseBattleFeatureWave) : null);
		if (_wave != towerDefenseBattleFeatureWave)
		{
			UnbindWave();
			_wave = towerDefenseBattleFeatureWave;
			if (GodotObject.IsInstanceValid(_wave))
			{
				_wave.OnCollectWaveReinforcements += AddWaveReinforcement;
			}
		}
	}

	private void UnbindWave()
	{
		if (GodotObject.IsInstanceValid(_wave))
		{
			_wave.OnCollectWaveReinforcements -= AddWaveReinforcement;
		}
		_wave = null;
	}

	private void AddWaveReinforcement(TowerDefenseBattleFeatureWave.AddWaveReinforcementHandler addSpawn)
	{
		if (inGame && !editorPreviewMode && !die && !isDestroy && !IsQueuedForDeletion() && GodotObject.IsInstanceValid(instance) && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig = new TowerDefenseLevelSpawnConfig
			{
				zombie = "ZombieWorker"
			};
			if (instance.hypnoses)
			{
				towerDefenseLevelSpawnConfig.overrideVal = new TowerDefenseCharacterOverride
				{
					hypnoses = true
				};
			}
			addSpawn(gridPos.Y, towerDefenseLevelSpawnConfig);
		}
	}

	public override void _ExitTree()
	{
		UnbindWave();
		base._ExitTree();
	}

	public static TowerDefenseWorkerSign FindInLane(SceneTree tree, int lane, bool? hypnoses = null)
	{
		if (tree == null)
		{
			return null;
		}
		foreach (Node item in tree.GetNodesInGroup("WorkerSign"))
		{
			if (item is TowerDefenseWorkerSign { inGame: not false, editorPreviewMode: false, die: false, isDestroy: false } towerDefenseWorkerSign && !towerDefenseWorkerSign.IsQueuedForDeletion() && towerDefenseWorkerSign.gridPos.Y == lane && (!hypnoses.HasValue || (GodotObject.IsInstanceValid(towerDefenseWorkerSign.instance) && towerDefenseWorkerSign.instance.hypnoses == hypnoses.Value)))
			{
				return towerDefenseWorkerSign;
			}
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnbindWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.BindWave && args.Count == 0)
		{
			BindWave();
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindWave && args.Count == 0)
		{
			UnbindWave();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
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
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.BindWave)
		{
			return true;
		}
		if (method == MethodName.UnbindWave)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._wave)
		{
			_wave = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._wave)
		{
			value = VariantUtils.CreateFrom(in _wave);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._wave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._wave, Variant.From(in _wave));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._wave, out var value))
		{
			_wave = value.As<TowerDefenseBattleFeatureWave>();
		}
	}
}
