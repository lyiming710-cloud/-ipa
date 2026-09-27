using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter6/DiscoGargantuar/Scene/TowerDefenseZombieDiscoGargantuar.cs")]
public class TowerDefenseZombieDiscoGargantuar : TowerDefenseZombieGargantuarBase, IJackson, INetworkDancerOwner
{
	public new class MethodName : TowerDefenseZombieGargantuarBase.MethodName
	{
		public static readonly StringName RemoveDancer = "RemoveDancer";

		public static readonly StringName IsSameDancer = "IsSameDancer";

		public static readonly StringName SetNetworkDancer = "SetNetworkDancer";

		public static readonly StringName GetDancer = "GetDancer";

		public static readonly StringName RefreshPendingDancerBatchEligibility = "RefreshPendingDancerBatchEligibility";

		public static readonly StringName ResolvePendingDancerRelations = "ResolvePendingDancerRelations";

		public static readonly StringName ReleaseDancerOwnership = "ReleaseDancerOwnership";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName DamagePointReach = "DamagePointReach";
	}

	public new class PropertyName : TowerDefenseZombieGargantuarBase.PropertyName
	{
		public static readonly StringName HasDancingRuntime = "HasDancingRuntime";

		public static readonly StringName HasDancingComponent = "HasDancingComponent";

		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName _pendingDancerSyncIds = "_pendingDancerSyncIds";

		public static readonly StringName _pendingDancerNodeNames = "_pendingDancerNodeNames";

		public static readonly StringName _pendingDancerResolveRemaining = "_pendingDancerResolveRemaining";

		public static readonly StringName _hasPendingDancerRelations = "_hasPendingDancerRelations";

		public static readonly StringName _dancerPacketName = "_dancerPacketName";
	}

	public new class SignalName : TowerDefenseZombieGargantuarBase.SignalName
	{
	}

	private const double PendingDancerResolveIntervalSeconds = 0.25;

	private const string ZOMBIE_BACKUP_GARGANTUAR_BODY12 = "uid://c53wvxn6c14gs";

	private const string ZOMBIE_BACKUP_GARGANTUAR_BODY13 = "uid://0f6e278qdym4";

	private DancingComponent _dancingComponent;

	private readonly int[] _pendingDancerSyncIds = new int[4] { -1, -1, -1, -1 };

	private readonly string[] _pendingDancerNodeNames = new string[4] { "", "", "", "" };

	private double _pendingDancerResolveRemaining;

	private bool _hasPendingDancerRelations;

	private string _dancerPacketName = "";

	private bool HasDancingRuntime
	{
		get
		{
			DancingComponent dancingComponent = _dancingComponent;
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
				return _dancingComponent.Lifecycle == ComponentRuntimeLifecycle.Active;
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
				_dancingComponent.dancerPacketName = value;
			}
		}
	}

	public void RemoveDancer(TowerDefenseCharacter dancer)
	{
		for (int i = 0; i < 4; i++)
		{
			bool flag = GodotObject.IsInstanceValid(dancer) && dancer.syncId >= 0 && _pendingDancerSyncIds[i] == dancer.syncId;
			if (IsSameDancer(GetDancer(i), dancer) || flag)
			{
				SetNetworkDancer(i, null);
				break;
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
		if (slot < 0 || slot >= 4)
		{
			return;
		}
		if (!HasDancingRuntime)
		{
			if (GodotObject.IsInstanceValid(dancer))
			{
				_pendingDancerSyncIds[slot] = dancer.syncId;
				_pendingDancerNodeNames[slot] = "";
			}
			else
			{
				_pendingDancerSyncIds[slot] = -1;
				_pendingDancerNodeNames[slot] = "";
			}
			RefreshPendingDancerBatchEligibility();
		}
		else
		{
			_dancingComponent.SetNetworkDancer(slot, dancer);
			_pendingDancerSyncIds[slot] = -1;
			_pendingDancerNodeNames[slot] = "";
			RefreshPendingDancerBatchEligibility();
		}
	}

	private TowerDefenseCharacter GetDancer(int slot)
	{
		if (!HasDancingRuntime)
		{
			return null;
		}
		return _dancingComponent.GetDancer(slot);
	}

	private void RefreshPendingDancerBatchEligibility()
	{
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			if (_pendingDancerSyncIds[i] >= 0 || _pendingDancerNodeNames[i] != "")
			{
				flag = true;
				break;
			}
		}
		if (_hasPendingDancerRelations != flag)
		{
			_hasPendingDancerRelations = flag;
			_pendingDancerResolveRemaining = (flag ? 0.25 : 0.0);
		}
	}

	private void ResolvePendingDancerRelations()
	{
		if (!_hasPendingDancerRelations)
		{
			return;
		}
		if (!HasDancingRuntime)
		{
			_pendingDancerResolveRemaining = 0.25;
			return;
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		for (int i = 0; i < 4; i++)
		{
			int num = _pendingDancerSyncIds[i];
			if (num >= 0 && GodotObject.IsInstanceValid(currentControl) && currentControl._syncCharacters.TryGetValue(num, out var value) && GodotObject.IsInstanceValid(value))
			{
				SetNetworkDancer(i, value);
				continue;
			}
			string text = _pendingDancerNodeNames[i];
			if (!(text == "") && GodotObject.IsInstanceValid(node2D))
			{
				TowerDefenseCharacter nodeOrNull = node2D.GetNodeOrNull<TowerDefenseCharacter>(new NodePath(text));
				if (GodotObject.IsInstanceValid(nodeOrNull))
				{
					SetNetworkDancer(i, nodeOrNull);
				}
			}
		}
		_pendingDancerResolveRemaining = (_hasPendingDancerRelations ? 0.25 : 0.0);
	}

	private void ReleaseDancerOwnership()
	{
		if (HasDancingRuntime)
		{
			for (int i = 0; i < 4; i++)
			{
				_dancingComponent.SetNetworkDancer(i, null);
			}
		}
		System.Array.Fill(_pendingDancerSyncIds, -1);
		System.Array.Fill(_pendingDancerNodeNames, "");
		RefreshPendingDancerBatchEligibility();
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
		if (!Engine.IsEditorHint())
		{
			_dancingComponent = componentManager.GetRuntime<DancingComponent>();
			if (HasDancingComponent && !string.IsNullOrEmpty(dancerPacketName))
			{
				_dancingComponent.dancerPacketName = dancerPacketName;
			}
			RefreshPendingDancerBatchEligibility();
			if (_hasPendingDancerRelations)
			{
				ResolvePendingDancerRelations();
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (_hasPendingDancerRelations)
		{
			_pendingDancerResolveRemaining -= delta;
			if (!(_pendingDancerResolveRemaining > 0.0))
			{
				ResolvePendingDancerRelations();
			}
		}
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
		if (HasDancingComponent)
		{
			_dancingComponent.OnWalkEntered();
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (HasDancingComponent)
		{
			groundMoveComponent.SetAlive(_dancingComponent.CanWalk());
		}
		base.WalkProcessing(delta);
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		if (HasDancingComponent)
		{
			_dancingComponent.OnAttackProcessing(delta);
		}
	}

	public override void Walk()
	{
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else if (!HasDancingComponent || !_dancingComponent.OnWalk())
		{
			SendStateEvent("ToWalk");
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		if (HasDancingComponent)
		{
			_dancingComponent.OnDieProcessing();
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		ReleaseDancerOwnership();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Array<int> array = new Array<int>();
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = GetDancer(i);
			array.Add(GodotObject.IsInstanceValid(dancer) ? dancer.syncId : _pendingDancerSyncIds[i]);
		}
		return new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["dancerSyncIds"] = array
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (!data.ContainsKey("dancerSyncIds"))
		{
			return;
		}
		Godot.Collections.Array array = data["dancerSyncIds"].AsGodotArray();
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		for (int i = 0; i < 4; i++)
		{
			int num = ((i < array.Count) ? array[i].AsInt32() : (-1));
			if (num < 0)
			{
				SetNetworkDancer(i, null);
				continue;
			}
			if (GodotObject.IsInstanceValid(currentControl) && currentControl._syncCharacters.TryGetValue(num, out var value) && GodotObject.IsInstanceValid(value))
			{
				SetNetworkDancer(i, value);
				continue;
			}
			SetNetworkDancer(i, null);
			_pendingDancerSyncIds[i] = num;
			_pendingDancerNodeNames[i] = "";
		}
		RefreshPendingDancerBatchEligibility();
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		int num = 17;
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = GetDancer(i);
			int num2 = (GodotObject.IsInstanceValid(dancer) ? dancer.syncId : _pendingDancerSyncIds[i]);
			num = num * 31 + num2;
		}
		return num;
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		Array<string> array = new Array<string>();
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = GetDancer(i);
			array.Add(GodotObject.IsInstanceValid(dancer) ? dancer.Name.ToString() : _pendingDancerNodeNames[i]);
		}
		dictionary["dancerNodeNames"] = array;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		if (data.ContainsKey("dancerNodeNames"))
		{
			Godot.Collections.Array array = data["dancerNodeNames"].AsGodotArray();
			for (int i = 0; i < 4; i++)
			{
				string text = ((i < array.Count) ? array[i].AsString() : "");
				SetNetworkDancer(i, null);
				_pendingDancerNodeNames[i] = text;
			}
		}
		RefreshPendingDancerBatchEligibility();
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
		}
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		RefreshPendingDancerBatchEligibility();
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (!(damangePointName == "Arm"))
		{
			if (damangePointName == "Head")
			{
				sprite.SetAtlasReplace("Zombie_BackupGargantuar_body1.png", "uid://0f6e278qdym4");
			}
		}
		else
		{
			sprite.SetAtlasReplace("Zombie_BackupGargantuar_body1.png", "uid://c53wvxn6c14gs");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
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
			new MethodInfo(MethodName.GetDancer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPendingDancerBatchEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePendingDancerRelations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseDancerOwnership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.GetDancer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetDancer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshPendingDancerBatchEligibility && args.Count == 0)
		{
			RefreshPendingDancerBatchEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingDancerRelations && args.Count == 0)
		{
			ResolvePendingDancerRelations();
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
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
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.GetDancer)
		{
			return true;
		}
		if (method == MethodName.RefreshPendingDancerBatchEligibility)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingDancerRelations)
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
		if (method == MethodName.BatchUpdate)
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
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
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
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (name == PropertyName._pendingDancerResolveRemaining)
		{
			_pendingDancerResolveRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hasPendingDancerRelations)
		{
			_hasPendingDancerRelations = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._pendingDancerSyncIds)
		{
			value = VariantUtils.CreateFrom(in _pendingDancerSyncIds);
			return true;
		}
		if (name == PropertyName._pendingDancerNodeNames)
		{
			value = VariantUtils.CreateFrom(in _pendingDancerNodeNames);
			return true;
		}
		if (name == PropertyName._pendingDancerResolveRemaining)
		{
			value = VariantUtils.CreateFrom(in _pendingDancerResolveRemaining);
			return true;
		}
		if (name == PropertyName._hasPendingDancerRelations)
		{
			value = VariantUtils.CreateFrom(in _hasPendingDancerRelations);
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
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._pendingDancerSyncIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._pendingDancerNodeNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingDancerResolveRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingDancerRelations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dancerPacketName, Variant.From<string>(dancerPacketName));
		info.AddProperty(PropertyName._pendingDancerResolveRemaining, Variant.From(in _pendingDancerResolveRemaining));
		info.AddProperty(PropertyName._hasPendingDancerRelations, Variant.From(in _hasPendingDancerRelations));
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
		if (info.TryGetProperty(PropertyName._pendingDancerResolveRemaining, out var value2))
		{
			_pendingDancerResolveRemaining = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingDancerRelations, out var value3))
		{
			_hasPendingDancerRelations = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dancerPacketName, out var value4))
		{
			_dancerPacketName = value4.As<string>();
		}
	}
}
