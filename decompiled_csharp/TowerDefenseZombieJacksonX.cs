using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/JacksonX/Scene/TowerDefenseZombieJacksonX.cs")]
public class TowerDefenseZombieJacksonX : TowerDefenseZombie, IJackson, INetworkDancerOwner
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName RemoveDancer = "RemoveDancer";

		public static readonly StringName IsSameDancer = "IsSameDancer";

		public static readonly StringName SetNetworkDancer = "SetNetworkDancer";

		public static readonly StringName ApplyPendingNetworkDancers = "ApplyPendingNetworkDancers";

		public static readonly StringName ReleaseDancerOwnership = "ReleaseDancerOwnership";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName SpawnDisco = "SpawnDisco";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName BatchUpdate = "BatchUpdate";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName HasDancingRuntime = "HasDancingRuntime";

		public static readonly StringName HasDancingComponent = "HasDancingComponent";

		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName _pendingNetworkDancers = "_pendingNetworkDancers";

		public static readonly StringName disco = "disco";

		public static readonly StringName isSpawnrackup = "isSpawnrackup";

		public static readonly StringName _pendingDiscoName = "_pendingDiscoName";

		public static readonly StringName _dancerPacketName = "_dancerPacketName";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public DancingComponent dancingComponent;

	private readonly TowerDefenseCharacter[] _pendingNetworkDancers = new TowerDefenseCharacter[4];

	public TowerDefenseCharacter disco;

	public bool isSpawnrackup;

	private string _pendingDiscoName = "";

	private string _dancerPacketName = "";

	private bool HasDancingRuntime
	{
		get
		{
			DancingComponent dancingComponent = this.dancingComponent;
			if (dancingComponent == null)
			{
				return false;
			}
			return !dancingComponent.IsReleased;
		}
	}

	private bool HasDancingComponent
	{
		get
		{
			if (HasDancingRuntime)
			{
				return dancingComponent.Lifecycle == ComponentRuntimeLifecycle.Active;
			}
			return false;
		}
	}

	[Export(PropertyHint.None, "")]
	public string dancerPacketName
	{
		get
		{
			return _dancerPacketName;
		}
		set
		{
			_dancerPacketName = value;
			if (!string.IsNullOrEmpty(value) && HasDancingComponent)
			{
				dancingComponent.dancerPacketName = value;
			}
		}
	}

	public void RemoveDancer(TowerDefenseCharacter dancer)
	{
		if (HasDancingRuntime)
		{
			dancingComponent.RemoveDancer(dancer);
		}
		for (int i = 0; i < 4; i++)
		{
			if (IsSameDancer(_pendingNetworkDancers[i], dancer))
			{
				_pendingNetworkDancers[i] = null;
			}
		}
	}

	private static bool IsSameDancer(TowerDefenseCharacter candidate, TowerDefenseCharacter dancer)
	{
		if (candidate != dancer)
		{
			if (GodotObject.IsInstanceValid(candidate) && GodotObject.IsInstanceValid(dancer) && candidate.syncId >= 0)
			{
				return candidate.syncId == dancer.syncId;
			}
			return false;
		}
		return true;
	}

	public void SetNetworkDancer(int slot, TowerDefenseCharacter dancer)
	{
		if (slot >= 0 && slot < 4)
		{
			if (!HasDancingRuntime)
			{
				_pendingNetworkDancers[slot] = dancer;
				return;
			}
			dancingComponent.SetNetworkDancer(slot, dancer);
			_pendingNetworkDancers[slot] = null;
		}
	}

	private void ApplyPendingNetworkDancers()
	{
		if (!HasDancingRuntime)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = _pendingNetworkDancers[i];
			if (GodotObject.IsInstanceValid(dancer))
			{
				SetNetworkDancer(i, dancer);
			}
			else
			{
				_pendingNetworkDancers[i] = null;
			}
		}
	}

	private void ReleaseDancerOwnership()
	{
		if (HasDancingRuntime)
		{
			for (int i = 0; i < 4; i++)
			{
				dancingComponent.SetNetworkDancer(i, null);
			}
		}
		System.Array.Fill(_pendingNetworkDancers, null);
	}

	public override void _ExitTree()
	{
		if (IsQueuedForDeletion())
		{
			ReleaseDancerOwnership();
		}
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		dancingComponent = componentManager.GetRuntime<DancingComponent>();
		if (HasDancingComponent)
		{
			sprite.OnAnimeCompleted -= dancingComponent.AnimeCompleted;
			sprite.OnAnimeEvent -= dancingComponent.AnimeEvent;
			if (!string.IsNullOrEmpty(dancerPacketName))
			{
				dancingComponent.dancerPacketName = dancerPacketName;
			}
		}
		ApplyPendingNetworkDancers();
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
		if (HasDancingComponent)
		{
			dancingComponent.OnWalkEntered();
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (HasDancingComponent)
		{
			groundMoveComponent.SetAlive(dancingComponent.CanWalk());
		}
		base.WalkProcessing(delta);
	}

	public override void Walk()
	{
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else if (!HasDancingComponent || !dancingComponent.OnWalk())
		{
			SendStateEvent("ToWalk");
		}
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		if (HasDancingComponent)
		{
			dancingComponent.OnAttackProcessing(delta);
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		if (HasDancingComponent)
		{
			dancingComponent.OnDieProcessing();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!inGame || !TowerDefenseManager.Instance.IsGameRunning() || !HasDancingComponent)
		{
			return;
		}
		if (clip == dancingComponent.pointDownAnimeClip)
		{
			if (isSpawnrackup)
			{
				dancingComponent.isPointing = false;
				dancingComponent.HideSpotlights();
				foreach (TowerDefenseCharacter dancer in dancingComponent.dancerList)
				{
					if (GodotObject.IsInstanceValid(dancer) && !dancer.die && !dancer.nearDie)
					{
						((TowerDefenseZombie)dancer).Walk();
					}
				}
				Walk();
				dancingComponent.RequestIdle();
				isSpawnrackup = false;
			}
			else
			{
				JacksonXPointDownAsync();
			}
		}
		else if (clip == dancingComponent.walkAnimeClip)
		{
			dancingComponent.walkTime--;
			if (!die && !nearDie)
			{
				if (dancingComponent.walkTime > 0)
				{
					return;
				}
				if (dancingComponent.CanSpawnDancer())
				{
					Component();
					dancingComponent.RequestPoint();
					return;
				}
				foreach (TowerDefenseCharacter dancer2 in dancingComponent.dancerList)
				{
					if (GodotObject.IsInstanceValid(dancer2) && !dancer2.die && !dancer2.nearDie && dancer2.sprite.clip == dancingComponent.walkAnimeClip)
					{
						dancer2.SendStateEvent("ToDance");
					}
				}
				Component();
				dancingComponent.RequestDance();
			}
			else
			{
				Die();
			}
		}
		else
		{
			if (!(clip == dancingComponent.armRiseAnimeClip))
			{
				return;
			}
			if (dancingComponent.armRiseFlipSprite)
			{
				sprite.Scale = new Vector2(0f - sprite.Scale.X, sprite.Scale.Y);
			}
			dancingComponent.danceTime--;
			if (!die && !nearDie)
			{
				if (dancingComponent.danceTime > 0)
				{
					return;
				}
				if (dancingComponent.CanSpawnDancer())
				{
					Component();
					dancingComponent.RequestPoint();
					return;
				}
				foreach (TowerDefenseCharacter dancer3 in dancingComponent.dancerList)
				{
					if (GodotObject.IsInstanceValid(dancer3) && !dancer3.die && !dancer3.nearDie && dancer3.sprite.clip == dancingComponent.armRiseAnimeClip)
					{
						((TowerDefenseZombie)dancer3).Walk();
					}
				}
				Walk();
				dancingComponent.RequestIdle();
			}
			else
			{
				Die();
			}
		}
	}

	private async Task JacksonXPointDownAsync()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (GodotObject.IsInstanceValid(disco))
		{
			if (disco is TowerDefenseZombieDisco towerDefenseZombieDisco)
			{
				towerDefenseZombieDisco.moonWalkOver = true;
			}
			disco.SendStateEvent("ToPoint");
		}
		dancingComponent.RequestPoint();
		isSpawnrackup = true;
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!HasDancingComponent || !(command == "spawn") || die || nearDie)
		{
			return;
		}
		if (isSpawnrackup)
		{
			dancingComponent.SpawnDancer();
		}
		else
		{
			SpawnDisco();
		}
		if (GodotObject.IsInstanceValid(dancingComponent.spotlight))
		{
			dancingComponent.spotlight.Visible = true;
		}
		if (GodotObject.IsInstanceValid(dancingComponent.spotlight2))
		{
			dancingComponent.spotlight2.Visible = true;
		}
		dancingComponent.ChangeSpotlightColor();
		if (!dancingComponent.firstSpawn)
		{
			dancingComponent.firstSpawn = true;
			if (dancingComponent.spotlightAudioName != "")
			{
				AudioManager.Instance.AudioPlay(dancingComponent.spotlightAudioName);
			}
		}
	}

	public void SpawnDisco()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieDisco");
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		Array<Vector2I> array = new Array<Vector2I>
		{
			new Vector2I(1, 0),
			new Vector2I(-1, 0)
		};
		if (gridPos.Y > 1)
		{
			array.Add(new Vector2I(0, -1));
		}
		if (gridPos.Y < mapGridNum.Y)
		{
			array.Add(new Vector2I(0, 1));
		}
		Vector2I vector2I = array[GD.RandRange(0, array.Count - 1)];
		double hitpointScale = instance.hitpointScale;
		Vector2 scale = transformPoint.Scale;
		Vector2I vector2I2 = gridPos + vector2I;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 pos = ((vector2I.X == 0) ? new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y + vector2I.Y)) : (logicalGlobalPosition + new Vector2(mapGridSize.X * (float)vector2I.X * 1.25f, 0f)));
		disco = packetConfig.Create(pos, vector2I2);
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", disco);
		disco.CallDeferred("SetHitpointAndScale", hitpointScale, scale);
		Callable.From(() =>
		{
			disco.Rise(1.0);
		}).CallDeferred();
		disco.invisible = invisible;
		if (instance.hypnoses)
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(disco))
				{
					disco.Hypnoses();
				}
			}).CallDeferred();
		}
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, disco);
				MultiPlayerManager.Instance.SendSpawnCharacterAt("ZombieDisco", vector2I2.X, vector2I2.Y, nextSyncId, hitpointScale, scale.X, instance.hypnoses, 1.0, useCreate: true, pos.X, pos.Y);
			}
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		ReleaseDancerOwnership();
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary { ["isSpawnBackup"] = isSpawnrackup };
		if (GodotObject.IsInstanceValid(disco))
		{
			dictionary["discoNodeName"] = disco.Name;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isSpawnrackup = data.GetValueOrDefault("isSpawnBackup", false).AsBool();
		if (data.ContainsKey("discoNodeName"))
		{
			_pendingDiscoName = data["discoNodeName"].AsString();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!(_pendingDiscoName != ""))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(node2D))
		{
			Node nodeOrNull = node2D.GetNodeOrNull(_pendingDiscoName);
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				disco = nodeOrNull as TowerDefenseCharacter;
			}
		}
		_pendingDiscoName = "";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.RemoveDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSameDancer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNetworkDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingNetworkDancers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseDancerOwnership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnDisco, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RemoveDancer && args.Count == 1)
		{
			RemoveDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSameDancer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.SetNetworkDancer && args.Count == 2)
		{
			SetNetworkDancer(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingNetworkDancers && args.Count == 0)
		{
			ApplyPendingNetworkDancers();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseDancerOwnership && args.Count == 0)
		{
			ReleaseDancerOwnership();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnDisco && args.Count == 0)
		{
			SpawnDisco();
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSameDancer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RemoveDancer)
		{
			return true;
		}
		if (method == MethodName.IsSameDancer)
		{
			return true;
		}
		if (method == MethodName.SetNetworkDancer)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingNetworkDancers)
		{
			return true;
		}
		if (method == MethodName.ReleaseDancerOwnership)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.SpawnDisco)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dancerPacketName)
		{
			dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.disco)
		{
			disco = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.isSpawnrackup)
		{
			isSpawnrackup = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingDiscoName)
		{
			_pendingDiscoName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._dancerPacketName)
		{
			_dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.HasDancingRuntime)
		{
			from = HasDancingRuntime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasDancingComponent)
		{
			from = HasDancingComponent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dancerPacketName)
		{
			value = VariantUtils.CreateFrom<string>(dancerPacketName);
			return true;
		}
		if (name == PropertyName._pendingNetworkDancers)
		{
			GodotObject[] pendingNetworkDancers = _pendingNetworkDancers;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(pendingNetworkDancers);
			return true;
		}
		if (name == PropertyName.disco)
		{
			value = VariantUtils.CreateFrom(in disco);
			return true;
		}
		if (name == PropertyName.isSpawnrackup)
		{
			value = VariantUtils.CreateFrom(in isSpawnrackup);
			return true;
		}
		if (name == PropertyName._pendingDiscoName)
		{
			value = VariantUtils.CreateFrom(in _pendingDiscoName);
			return true;
		}
		if (name == PropertyName._dancerPacketName)
		{
			value = VariantUtils.CreateFrom(in _dancerPacketName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._pendingNetworkDancers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.disco, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSpawnrackup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingDiscoName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dancerPacketName, Variant.From<string>(dancerPacketName));
		info.AddProperty(PropertyName.disco, Variant.From(in disco));
		info.AddProperty(PropertyName.isSpawnrackup, Variant.From(in isSpawnrackup));
		info.AddProperty(PropertyName._pendingDiscoName, Variant.From(in _pendingDiscoName));
		info.AddProperty(PropertyName._dancerPacketName, Variant.From(in _dancerPacketName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dancerPacketName, out var value))
		{
			dancerPacketName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.disco, out var value2))
		{
			disco = value2.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.isSpawnrackup, out var value3))
		{
			isSpawnrackup = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingDiscoName, out var value4))
		{
			_pendingDiscoName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._dancerPacketName, out var value5))
		{
			_dancerPacketName = value5.As<string>();
		}
	}
}
