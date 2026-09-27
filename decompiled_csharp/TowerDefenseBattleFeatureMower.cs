using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Mower/TowerDefenseBattleFeatureMower.cs")]
public class TowerDefenseBattleFeatureMower : TowerDefenseBattleFeature
{
	private sealed class MowerSyncState
	{
		public int SyncId = -1;

		public bool Running;

		public Vector2 Position;

		public bool HasPosition;
	}

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName MowerInit = "MowerInit";

		public static readonly StringName CreateMower = "CreateMower";

		public static readonly StringName MowerRun = "MowerRun";

		public static readonly StringName IZM2Init = "IZM2Init";

		public static readonly StringName CreateTargetZombie = "CreateTargetZombie";

		public static readonly StringName CheckIZM2Fail = "CheckIZM2Fail";

		public static readonly StringName TargetZombieDestroy = "TargetZombieDestroy";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName ApplyTargetState = "ApplyTargetState";

		public static readonly StringName BindNetworkCharacter = "BindNetworkCharacter";

		public static readonly StringName DestroyRemoteMower = "DestroyRemoteMower";

		public static readonly StringName DestroyRemoteTarget = "DestroyRemoteTarget";

		public static readonly StringName MarkRemoteDestroy = "MarkRemoteDestroy";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName IsValidLine = "IsValidLine";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName mowerManager = "mowerManager";

		public static readonly StringName config = "config";

		public static readonly StringName mowerLine = "mowerLine";

		public static readonly StringName mowerHasRun = "mowerHasRun";

		public static readonly StringName targetZombieLine = "targetZombieLine";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public const int GridMaxSize = 51;

	private static PackedScene _towerDefenseMowerManager;

	public TowerDefenseMowerManager mowerManager;

	public TowerDefenseBattleFeatureMowerConfig config;

	public Array<TowerDefenseMower> mowerLine = new Array<TowerDefenseMower>();

	public bool mowerHasRun;

	public Array<TowerDefenseCharacter> targetZombieLine = new Array<TowerDefenseCharacter>();

	private static PackedScene TOWER_DEFENSE_MOWER_MANAGER => _towerDefenseMowerManager ?? (_towerDefenseMowerManager = GD.Load<PackedScene>("uid://c0xrja6fjw4rr"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseBattleFeatureMowerConfig();
		config.Init(data);
		mowerLine.Clear();
		targetZombieLine.Clear();
		mowerLine.Resize(51);
		targetZombieLine.Resize(51);
		mowerManager = TOWER_DEFENSE_MOWER_MANAGER.Instantiate<TowerDefenseMowerManager>(PackedScene.GenEditState.Disabled);
		mowerManager.mowerFeature = this;
		mowerManager.config = config;
		control.AddNode(mowerManager);
	}

	public void MowerInit()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return;
		}
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			if (mapFeature.lineUse[i])
			{
				CreateMower(i);
			}
		}
	}

	public TowerDefenseMower CreateMower(int line, int syncId = -1)
	{
		if (!IsValidLine(line))
		{
			return null;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(mowerLine[line]))
		{
			return null;
		}
		string text = config.mowerPacketName;
		if (text == "")
		{
			text = GameSaveManager.Instance.GetKeyValue("CurrentMower").AsString();
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
		if (mapFeature.LineHasType(line, TowerDefenseEnum.PLANTGRIDTYPE.WATER))
		{
			packetConfig = TowerDefenseManager.GetPacketConfig(config.waterMowerPacketName);
		}
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return null;
		}
		Vector2 pos = TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, line)) + new Vector2((float)config.mowerSpawnOffsetX, 0f);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefenseMower towerDefenseMower = packetConfig.Create(pos, new Vector2I(0, line)) as TowerDefenseMower;
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(1, line));
		if (!GodotObject.IsInstanceValid(towerDefenseMower) || !GodotObject.IsInstanceValid(characterNode) || !GodotObject.IsInstanceValid(mapCell))
		{
			if (GodotObject.IsInstanceValid(towerDefenseMower))
			{
				towerDefenseMower.QueueFree();
			}
			return null;
		}
		towerDefenseMower.characterFilter = true;
		towerDefenseMower.OnRunning += MowerRun;
		towerDefenseMower.groundHeight = mapCell.GetGroundHeight(0.0);
		towerDefenseMower.z = towerDefenseMower.groundHeight;
		characterNode.AddChild(towerDefenseMower, forceReadableName: false, Node.InternalMode.Disabled);
		mowerLine[line] = towerDefenseMower;
		RegisterNetworkCharacter(towerDefenseMower, ref syncId);
		return towerDefenseMower;
	}

	public void MowerRun(TowerDefenseMower mower)
	{
		if (GodotObject.IsInstanceValid(mower))
		{
			mower.OnRunning -= MowerRun;
			int num = mowerLine.IndexOf(mower);
			if (num >= 0)
			{
				mowerLine[num] = null;
			}
			mowerHasRun = true;
		}
	}

	public void IZM2Init()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return;
		}
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			if (mapFeature.lineUse[i])
			{
				CreateTargetZombie(i);
			}
		}
	}

	public TowerDefenseCharacter CreateTargetZombie(int line, int syncId = -1)
	{
		if (!IsValidLine(line))
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(targetZombieLine[line]))
		{
			return null;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(config.targetZombiePacketName);
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(instance))
		{
			return null;
		}
		Vector2 pos = new Vector2((float)(instance.GetMapGroundRight() + config.targetSpawnOffsetX), TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, line)).Y);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Create(pos, new Vector2I(0, line));
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(1, line));
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !GodotObject.IsInstanceValid(characterNode) || !GodotObject.IsInstanceValid(mapCell))
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.QueueFree();
			}
			return null;
		}
		towerDefenseCharacter.OnDestroy += TargetZombieDestroy;
		towerDefenseCharacter.groundHeight = mapCell.GetGroundHeight(0.0);
		towerDefenseCharacter.z = towerDefenseCharacter.groundHeight;
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
		towerDefenseCharacter.Rise();
		targetZombieLine[line] = towerDefenseCharacter;
		RegisterNetworkCharacter(towerDefenseCharacter, ref syncId);
		return towerDefenseCharacter;
	}

	public void CheckIZM2Fail()
	{
		if ((Global.IsMultiplayerMode && (MultiPlayerManager.Instance == null || !MultiPlayerManager.Instance.isHost)) || (CommandManager.Instance.debug && CommandManager.Instance.debugNoLose))
		{
			return;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return;
		}
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			if (mapFeature.lineUse[i] && GodotObject.IsInstanceValid(targetZombieLine[i]))
			{
				TowerDefenseManager.CurrentControl.GameFail(null);
			}
		}
	}

	public void TargetZombieDestroy(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.OnDestroy -= TargetZombieDestroy;
		}
		int num = targetZombieLine.IndexOf(character);
		if (num >= 0)
		{
			targetZombieLine[num] = null;
		}
		if (IsLifetimeActive && (!Global.IsMultiplayerMode || (MultiPlayerManager.Instance != null && MultiPlayerManager.Instance.isHost)) && (!CommandManager.Instance.debug || !CommandManager.Instance.debugNoLose))
		{
			TowerDefenseInGameLevelControl instance = TowerDefenseInGameLevelControl.instance;
			if (!GodotObject.IsInstanceValid(instance) || !instance.awardCreate)
			{
				TowerDefenseManager.CurrentControl.GameFail(null);
			}
		}
	}

	public override void GameFail()
	{
		foreach (TowerDefenseMower item in mowerLine)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.ProcessMode = Node.ProcessModeEnum.Disabled;
			}
		}
	}

	public override Dictionary SyncSerialize()
	{
		Array array = new Array();
		foreach (KeyValuePair<int, TowerDefenseMower> item in GetLiveMowersByLine())
		{
			TowerDefenseMower value = item.Value;
			Vector2 logicalGlobalPosition = value.GetLogicalGlobalPosition();
			array.Add(new Dictionary
			{
				["line"] = item.Key,
				["sync_id"] = value.syncId,
				["running"] = value.run,
				["x"] = logicalGlobalPosition.X,
				["y"] = logicalGlobalPosition.Y
			});
		}
		Array array2 = new Array();
		for (int i = 1; i < targetZombieLine.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = targetZombieLine[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.isDestroy && !towerDefenseCharacter.IsDie())
			{
				array2.Add(new Dictionary
				{
					["line"] = i,
					["sync_id"] = towerDefenseCharacter.syncId
				});
			}
		}
		return new Dictionary
		{
			["mowers"] = array,
			["targets"] = array2,
			["mower_has_run"] = mowerHasRun
		};
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		mowerHasRun = _data.GetValueOrDefault("mower_has_run", mowerHasRun).AsBool();
		if (_data.TryGetValue("mowers", out var value) && value.VariantType == Variant.Type.Array)
		{
			System.Collections.Generic.Dictionary<int, MowerSyncState> dictionary = new System.Collections.Generic.Dictionary<int, MowerSyncState>();
			foreach (Variant item in value.AsGodotArray())
			{
				if (item.VariantType == Variant.Type.Dictionary)
				{
					Dictionary dictionary2 = item.AsGodotDictionary();
					int num = dictionary2.GetValueOrDefault("line", 0).AsInt32();
					if (IsValidLine(num))
					{
						dictionary[num] = new MowerSyncState
						{
							SyncId = dictionary2.GetValueOrDefault("sync_id", -1).AsInt32(),
							Running = dictionary2.GetValueOrDefault("running", false).AsBool(),
							Position = new Vector2((float)dictionary2.GetValueOrDefault("x", 0.0).AsDouble(), (float)dictionary2.GetValueOrDefault("y", 0.0).AsDouble()),
							HasPosition = (dictionary2.ContainsKey("x") && dictionary2.ContainsKey("y"))
						};
					}
				}
			}
			System.Collections.Generic.Dictionary<int, TowerDefenseMower> liveMowersByLine = GetLiveMowersByLine();
			foreach (KeyValuePair<int, TowerDefenseMower> item2 in liveMowersByLine)
			{
				if (!dictionary.ContainsKey(item2.Key))
				{
					DestroyRemoteMower(item2.Key, item2.Value);
				}
			}
			foreach (KeyValuePair<int, MowerSyncState> item3 in dictionary)
			{
				if (!liveMowersByLine.TryGetValue(item3.Key, out var value2) || !GodotObject.IsInstanceValid(value2))
				{
					value2 = CreateMower(item3.Key, item3.Value.SyncId);
				}
				else
				{
					BindNetworkCharacter(value2, item3.Value.SyncId);
				}
				if (!GodotObject.IsInstanceValid(value2))
				{
					continue;
				}
				if (!item3.Value.Running && value2.run)
				{
					DestroyRemoteMower(item3.Key, value2);
					value2 = CreateMower(item3.Key, item3.Value.SyncId);
					if (!GodotObject.IsInstanceValid(value2))
					{
						continue;
					}
				}
				if (item3.Value.HasPosition && float.IsFinite(item3.Value.Position.X) && float.IsFinite(item3.Value.Position.Y))
				{
					value2.SetLogicalGlobalPosition(item3.Value.Position);
				}
				if (item3.Value.Running && !value2.run)
				{
					value2.Run();
					value2.run = true;
				}
			}
		}
		if (_data.TryGetValue("targets", out var value3) && value3.VariantType == Variant.Type.Array)
		{
			ApplyTargetState(value3.AsGodotArray());
		}
	}

	private System.Collections.Generic.Dictionary<int, TowerDefenseMower> GetLiveMowersByLine()
	{
		System.Collections.Generic.Dictionary<int, TowerDefenseMower> dictionary = new System.Collections.Generic.Dictionary<int, TowerDefenseMower>();
		foreach (TowerDefenseMower item in mowerLine)
		{
			if (GodotObject.IsInstanceValid(item) && !item.isDestroy && !item.IsDie())
			{
				dictionary[item.gridPos.Y] = item;
			}
		}
		if (Engine.GetMainLoop() is SceneTree sceneTree)
		{
			foreach (Node item2 in sceneTree.GetNodesInGroup("Mower"))
			{
				if (item2 is TowerDefenseMower towerDefenseMower && GodotObject.IsInstanceValid(towerDefenseMower) && !towerDefenseMower.isDestroy && !towerDefenseMower.IsDie())
				{
					dictionary[towerDefenseMower.gridPos.Y] = towerDefenseMower;
				}
			}
		}
		return dictionary;
	}

	private void ApplyTargetState(Array targets)
	{
		System.Collections.Generic.Dictionary<int, int> dictionary = new System.Collections.Generic.Dictionary<int, int>();
		foreach (Variant target in targets)
		{
			if (target.VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary2 = target.AsGodotDictionary();
				int num = dictionary2.GetValueOrDefault("line", 0).AsInt32();
				if (IsValidLine(num))
				{
					dictionary[num] = dictionary2.GetValueOrDefault("sync_id", -1).AsInt32();
				}
			}
		}
		for (int i = 1; i < targetZombieLine.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = targetZombieLine[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !dictionary.ContainsKey(i))
			{
				DestroyRemoteTarget(i, towerDefenseCharacter);
			}
		}
		foreach (KeyValuePair<int, int> item in dictionary)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = targetZombieLine[item.Key];
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter2) || towerDefenseCharacter2.isDestroy)
			{
				towerDefenseCharacter2 = CreateTargetZombie(item.Key, item.Value);
			}
			else
			{
				BindNetworkCharacter(towerDefenseCharacter2, item.Value);
			}
		}
	}

	private void RegisterNetworkCharacter(TowerDefenseCharacter character, ref int syncId)
	{
		if (Global.IsMultiplayerMode && GodotObject.IsInstanceValid(control) && GodotObject.IsInstanceValid(character))
		{
			if (syncId < 0 && MultiPlayerManager.Instance != null && MultiPlayerManager.Instance.isHost)
			{
				syncId = control.GetNextSyncId();
			}
			if (syncId >= 0)
			{
				control.RegisterSyncCharacter(syncId, character);
			}
		}
	}

	private void BindNetworkCharacter(TowerDefenseCharacter character, int syncId)
	{
		if (syncId >= 0 && GodotObject.IsInstanceValid(control) && GodotObject.IsInstanceValid(character) && character.syncId != syncId)
		{
			control.DetachSyncCharacter(character);
			control.RegisterSyncCharacter(syncId, character);
		}
	}

	private void DestroyRemoteMower(int line, TowerDefenseMower mower)
	{
		if (IsValidLine(line) && mowerLine[line] == mower)
		{
			mowerLine[line] = null;
		}
		mower.OnRunning -= MowerRun;
		MarkRemoteDestroy(mower);
	}

	private void DestroyRemoteTarget(int line, TowerDefenseCharacter target)
	{
		if (IsValidLine(line) && targetZombieLine[line] == target)
		{
			targetZombieLine[line] = null;
		}
		target.OnDestroy -= TargetZombieDestroy;
		MarkRemoteDestroy(target);
	}

	private static void MarkRemoteDestroy(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			DestroyComponent destroyComponent = character.destroyComponent;
			if (destroyComponent != null && !destroyComponent.IsReleased)
			{
				character.destroyComponent.isRemoteDestroy = true;
			}
			character.Destroy();
		}
	}

	public override Dictionary SaveFeature()
	{
		return new Dictionary { ["mowerHasRun"] = mowerHasRun };
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		mowerHasRun = _data.GetValueOrDefault("mowerHasRun", false).AsBool();
	}

	private bool IsValidLine(int line)
	{
		if (line > 0 && line < mowerLine.Count)
		{
			return line < targetZombieLine.Count;
		}
		return false;
	}

	public override void Destroy()
	{
		foreach (TowerDefenseMower item in mowerLine)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.OnRunning -= MowerRun;
			}
		}
		foreach (TowerDefenseCharacter item2 in targetZombieLine)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				item2.OnDestroy -= TargetZombieDestroy;
			}
		}
		mowerLine.Clear();
		targetZombieLine.Clear();
		if (GodotObject.IsInstanceValid(mowerManager))
		{
			mowerManager.DisposeBattleState();
			mowerManager.QueueFree();
		}
		mowerManager = null;
		config = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(20)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MowerInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMower, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MowerRun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IZM2Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateTargetZombie, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckIZM2Fail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TargetZombieDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTargetState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "targets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindNetworkCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyRemoteMower, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "mower", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyRemoteTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.MarkRemoteDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MowerInit && args.Count == 0)
		{
			MowerInit();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateMower && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMower>(CreateMower(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.MowerRun && args.Count == 1)
		{
			MowerRun(VariantUtils.ConvertTo<TowerDefenseMower>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IZM2Init && args.Count == 0)
		{
			IZM2Init();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateTargetZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateTargetZombie(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.CheckIZM2Fail && args.Count == 0)
		{
			CheckIZM2Fail();
			ret = default;
			return true;
		}
		if (method == MethodName.TargetZombieDestroy && args.Count == 1)
		{
			TargetZombieDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
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
		if (method == MethodName.ApplyTargetState && args.Count == 1)
		{
			ApplyTargetState(VariantUtils.ConvertTo<Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindNetworkCharacter && args.Count == 2)
		{
			BindNetworkCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyRemoteMower && args.Count == 2)
		{
			DestroyRemoteMower(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMower>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyRemoteTarget && args.Count == 2)
		{
			DestroyRemoteTarget(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkRemoteDestroy && args.Count == 1)
		{
			MarkRemoteDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsValidLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidLine(VariantUtils.ConvertTo<int>(in args[0])));
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MarkRemoteDestroy && args.Count == 1)
		{
			MarkRemoteDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.MowerInit)
		{
			return true;
		}
		if (method == MethodName.CreateMower)
		{
			return true;
		}
		if (method == MethodName.MowerRun)
		{
			return true;
		}
		if (method == MethodName.IZM2Init)
		{
			return true;
		}
		if (method == MethodName.CreateTargetZombie)
		{
			return true;
		}
		if (method == MethodName.CheckIZM2Fail)
		{
			return true;
		}
		if (method == MethodName.TargetZombieDestroy)
		{
			return true;
		}
		if (method == MethodName.GameFail)
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
		if (method == MethodName.ApplyTargetState)
		{
			return true;
		}
		if (method == MethodName.BindNetworkCharacter)
		{
			return true;
		}
		if (method == MethodName.DestroyRemoteMower)
		{
			return true;
		}
		if (method == MethodName.DestroyRemoteTarget)
		{
			return true;
		}
		if (method == MethodName.MarkRemoteDestroy)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.IsValidLine)
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
		if (name == PropertyName.mowerManager)
		{
			mowerManager = VariantUtils.ConvertTo<TowerDefenseMowerManager>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMowerConfig>(in value);
			return true;
		}
		if (name == PropertyName.mowerLine)
		{
			mowerLine = VariantUtils.ConvertToArray<TowerDefenseMower>(in value);
			return true;
		}
		if (name == PropertyName.mowerHasRun)
		{
			mowerHasRun = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.targetZombieLine)
		{
			targetZombieLine = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mowerManager)
		{
			value = VariantUtils.CreateFrom(in mowerManager);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.mowerLine)
		{
			value = VariantUtils.CreateFromArray(mowerLine);
			return true;
		}
		if (name == PropertyName.mowerHasRun)
		{
			value = VariantUtils.CreateFrom(in mowerHasRun);
			return true;
		}
		if (name == PropertyName.targetZombieLine)
		{
			value = VariantUtils.CreateFromArray(targetZombieLine);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mowerManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mowerLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerHasRun, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.targetZombieLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mowerManager, Variant.From(in mowerManager));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.mowerLine, Variant.CreateFrom(mowerLine));
		info.AddProperty(PropertyName.mowerHasRun, Variant.From(in mowerHasRun));
		info.AddProperty(PropertyName.targetZombieLine, Variant.CreateFrom(targetZombieLine));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mowerManager, out var value))
		{
			mowerManager = value.As<TowerDefenseMowerManager>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseBattleFeatureMowerConfig>();
		}
		if (info.TryGetProperty(PropertyName.mowerLine, out var value3))
		{
			mowerLine = value3.AsGodotArray<TowerDefenseMower>();
		}
		if (info.TryGetProperty(PropertyName.mowerHasRun, out var value4))
		{
			mowerHasRun = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.targetZombieLine, out var value5))
		{
			targetZombieLine = value5.AsGodotArray<TowerDefenseCharacter>();
		}
	}
}
