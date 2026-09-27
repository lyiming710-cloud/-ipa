using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/Worker/Scene/TowerDefenseZombieWorker.cs")]
public class TowerDefenseZombieWorker : TowerDefenseZombieDigger
{
	public new class MethodName : TowerDefenseZombieDigger.MethodName
	{
		public new static readonly StringName DigEntered = "DigEntered";

		public new static readonly StringName DigProcessing = "DigProcessing";

		public new static readonly StringName DrillEntered = "DrillEntered";

		public new static readonly StringName LandEntered = "LandEntered";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public static readonly StringName PlaceSign = "PlaceSign";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";
	}

	public new class PropertyName : TowerDefenseZombieDigger.PropertyName
	{
		public static readonly StringName _signPlaced = "_signPlaced";

		public static readonly StringName _emergeGrid = "_emergeGrid";
	}

	public new class SignalName : TowerDefenseZombieDigger.SignalName
	{
	}

	private bool _signPlaced;

	private Vector2I _emergeGrid;

	private static bool IsRemoteClient
	{
		get
		{
			if (Global.IsMultiplayerMode)
			{
				return !MultiPlayerManager.IsHost;
			}
			return false;
		}
	}

	public override void DigEntered()
	{
		base.DigEntered();
		UpdateFacing(true);
	}

	public override void DigProcessing(double delta)
	{
		if (IsRemoteClient)
		{
			sprite.timeScale = timeScale;
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		float x = globalPositionForPhysicsFrame.X;
		if (attackComponent.CanAttack())
		{
			if (!nearDie && !sprite.pause && sprite.timeScale > 0.0 && useAttackDps && GodotObject.IsInstanceValid(attackComponent.target))
			{
				if ((attackComponent.target.instance.collisionFlags & 0x10) != 0)
				{
					attackComponent.AttackDpsExecute(delta, ((TowerDefenseZombieConfig)config).attack);
				}
				else
				{
					attackComponent.target = null;
				}
			}
		}
		else if (!sprite.pause)
		{
			double num = speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)((!sprite.playBack) ? 1 : (-1));
			if ((double)globalPositionForPhysicsFrame.X > TowerDefenseManager.Instance.GetMapGroundRight())
			{
				num *= 2.0;
			}
			globalPositionForPhysicsFrame.X -= (float)num;
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		}
		sprite.timeScale = timeScale;
		if (digOver || sprite.pause || timeScale <= 0.0)
		{
			return;
		}
		TowerDefenseWorkerSign towerDefenseWorkerSign = TowerDefenseWorkerSign.FindInLane(GetTree(), gridPos.Y, instance.hypnoses);
		if (GodotObject.IsInstanceValid(towerDefenseWorkerSign))
		{
			float x2 = towerDefenseWorkerSign.GetLogicalGlobalPosition().X;
			if ((x >= x2 && globalPositionForPhysicsFrame.X <= x2) || Mathf.Abs(globalPositionForPhysicsFrame.X - x2) < 1f)
			{
				globalPositionForPhysicsFrame.X = x2;
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
				gridPos = towerDefenseWorkerSign.gridPos;
				digOver = true;
				SendStateEvent("ToDrill");
				return;
			}
		}
		if ((double)globalPositionForPhysicsFrame.X < TowerDefenseManager.Instance.GetMapGroundLeft() + 30.0)
		{
			digOver = true;
			SendStateEvent("ToDrill");
		}
	}

	public override void DrillEntered()
	{
		if (!IsRemoteClient && TowerDefenseManager.Instance != null)
		{
			_emergeGrid = TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition());
			Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
			_emergeGrid = new Vector2I(Mathf.Clamp(_emergeGrid.X, 1, mapGridNum.X), gridPos.Y);
		}
		UpdateFacing(false);
		base.DrillEntered();
	}

	public override void LandEntered()
	{
		instance.collisionFlags = 1;
		instance.maskFlags = 9;
		sprite.SetAnimation("Land", loop: false);
		sprite.SetFliter("Zombie_digger_pickaxe", open: false);
		if (!IsRemoteClient)
		{
			UpdateFacing(false);
			PlaceSign();
		}
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		UpdateFacing();
	}

	private void UpdateFacing(bool? digging = null)
	{
		if (!IsRemoteClient && GodotObject.IsInstanceValid(instance))
		{
			int num = TowerDefenseManager.Instance?.GetMapGridNum().X ?? 0;
			bool flag = (digging ?? (CurrentStateHandle?.StableId == "zombie.digger.dig")) || (!instance.hypnoses && (!digOver || (_emergeGrid.X >= 3 && _emergeGrid.X <= num)));
			float num2 = Mathf.Abs(Scale.X) * (float)(flag ? 1 : (-1));
			if (Scale.X != num2)
			{
				Scale = new Vector2(num2, Scale.Y);
				sprite?.NotifyAncestorTransformChangedForRender();
				groundMoveComponent?.RefreshDirectionCache();
			}
		}
	}

	private void PlaceSign()
	{
		if (_signPlaced || !inGame || editorPreviewMode || suppressDeathrattles || IsRemoteClient || !GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(_emergeGrid)))
		{
			return;
		}
		bool flag = GodotObject.IsInstanceValid(instance) && instance.hypnoses;
		TowerDefenseCharacter towerDefenseCharacter = TowerDefenseManager.GetPacketConfig("WorkerSign").Plant(_emergeGrid, playAudio: false, noLimit: true, default, skipPlacementCheck: true);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			_signPlaced = true;
			if (flag)
			{
				towerDefenseCharacter.Hypnoses();
			}
			TowerDefenseManager.PublishSpawnedCharacter("WorkerSign", towerDefenseCharacter, useCreate: false, 0.0, walkAfterSpawn: false, "", new Dictionary
			{
				["plant_skip_placement_check"] = true,
				["plant_play_audio"] = false,
				["hypnoses"] = flag
			});
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (clip == "Land")
		{
			if (!IsRemoteClient && !die && !nearDie)
			{
				WalkWithoutTransitionDelay();
			}
		}
		else if (!IsRemoteClient)
		{
			base.AnimeCompleted(clip);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["signPlaced"] = _signPlaced;
		dictionary["emergeGrid"] = _emergeGrid;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_signPlaced = data.GetValueOrDefault("signPlaced", false).AsBool();
		_emergeGrid = data.GetValueOrDefault("emergeGrid", gridPos).AsVector2I();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Dictionary dictionary = base.ExportNetworkSpecialState();
		dictionary["digOver"] = digOver;
		dictionary["signPlaced"] = _signPlaced;
		dictionary["emergeGrid"] = _emergeGrid;
		return dictionary;
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		digOver = data.GetValueOrDefault("digOver", digOver).AsBool();
		_signPlaced = data.GetValueOrDefault("signPlaced", _signPlaced).AsBool();
		_emergeGrid = data.GetValueOrDefault("emergeGrid", _emergeGrid).AsVector2I();
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return (int)((digOver ? 1u : 0u) | (uint)(_signPlaced ? 2 : 0)) | (_emergeGrid.X << 2);
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		switch (CurrentStateHandle?.StableId)
		{
		case "zombie.digger.dig":
		case "zombie.digger.drill":
		case "zombie.digger.land":
			return true;
		default:
			return false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.DigEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DigProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrillEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LandEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlaceSign, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNetworkSpecialMovementActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DigEntered && args.Count == 0)
		{
			DigEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DigProcessing && args.Count == 1)
		{
			DigProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrillEntered && args.Count == 0)
		{
			DrillEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.LandEntered && args.Count == 0)
		{
			LandEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.PlaceSign && args.Count == 0)
		{
			PlaceSign();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.IsNetworkSpecialMovementActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNetworkSpecialMovementActive());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.DigEntered)
		{
			return true;
		}
		if (method == MethodName.DigProcessing)
		{
			return true;
		}
		if (method == MethodName.DrillEntered)
		{
			return true;
		}
		if (method == MethodName.LandEntered)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.PlaceSign)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
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
		if (method == MethodName.IsNetworkSpecialMovementActive)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._signPlaced)
		{
			_signPlaced = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._emergeGrid)
		{
			_emergeGrid = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._signPlaced)
		{
			value = VariantUtils.CreateFrom(in _signPlaced);
			return true;
		}
		if (name == PropertyName._emergeGrid)
		{
			value = VariantUtils.CreateFrom(in _emergeGrid);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._signPlaced, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._emergeGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._signPlaced, Variant.From(in _signPlaced));
		info.AddProperty(PropertyName._emergeGrid, Variant.From(in _emergeGrid));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._signPlaced, out var value))
		{
			_signPlaced = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._emergeGrid, out var value2))
		{
			_emergeGrid = value2.As<Vector2I>();
		}
	}
}
