using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/Raker/Scene/TowerDefenseZombieRaker.cs")]
public class TowerDefenseZombieRaker : TowerDefenseZombiePolevaulter
{
	public new class MethodName : TowerDefenseZombiePolevaulter.MethodName
	{
		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName RefreshRakeVisual = "RefreshRakeVisual";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public static readonly StringName RefreshRakerAttachments = "RefreshRakerAttachments";

		public new static readonly StringName JumpEntered = "JumpEntered";

		public new static readonly StringName JumpExited = "JumpExited";

		public new static readonly StringName Block = "Block";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName DieEntered = "DieEntered";

		public static readonly StringName PlaceRake = "PlaceRake";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";
	}

	public new class PropertyName : TowerDefenseZombiePolevaulter.PropertyName
	{
		public static readonly StringName _rakePlaced = "_rakePlaced";

		public static readonly StringName _deathRakePending = "_deathRakePending";

		public static readonly StringName _rakeVisualHidden = "_rakeVisualHidden";

		public static readonly StringName _jumpGrid = "_jumpGrid";

		public static readonly StringName _jumpDirection = "_jumpDirection";
	}

	public new class SignalName : TowerDefenseZombiePolevaulter.SignalName
	{
	}

	private static readonly string[] RakeLayers = new string[2] { "Zombie_polevaulter_pole2 复制", "Zombie_polevaulter_pole 复制" };

	private bool _rakePlaced;

	private bool _deathRakePending;

	private bool _rakeVisualHidden;

	private Vector2I _jumpGrid;

	private int _jumpDirection = -1;

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

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		RefreshRakeVisual();
	}

	private void RefreshRakeVisual()
	{
		if (!_rakeVisualHidden && _rakePlaced && GodotObject.IsInstanceValid(sprite))
		{
			Array array = new Array();
			string[] rakeLayers = RakeLayers;
			foreach (string text in rakeLayers)
			{
				array.Add(text);
			}
			sprite.SetFliters(array, open: false);
			_rakeVisualHidden = true;
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (damagePointName == "Head")
		{
			RefreshRakerAttachments();
		}
	}

	private void RefreshRakerAttachments()
	{
		if (sprite is ZombieRakerSprite zombieRakerSprite)
		{
			zombieRakerSprite.RefreshAttachments();
		}
		else if (sprite is ZombieRakerWallnutSprite zombieRakerWallnutSprite)
		{
			zombieRakerWallnutSprite.RefreshRakeAttachment();
		}
	}

	public override void JumpEntered()
	{
		if (!IsRemoteClient)
		{
			TowerDefenseCharacter towerDefenseCharacter = componentManager.GetRuntime<AttackComponent>("character.attack.1")?.target;
			_jumpGrid = (GodotObject.IsInstanceValid(towerDefenseCharacter) ? towerDefenseCharacter.gridPos : gridPos);
			_jumpDirection = ((!(Scale.X * transformPoint.Scale.X >= 0f)) ? 1 : (-1));
		}
		base.JumpEntered();
	}

	public override void JumpExited()
	{
		if (jumpOver && !die && !nearDie)
		{
			PlaceRake(_jumpGrid + new Vector2I(isBlock ? (-_jumpDirection) : _jumpDirection, 0));
		}
		base.JumpExited();
	}

	public override void Block(TowerDefenseCharacter target)
	{
		if (!IsRemoteClient && GodotObject.IsInstanceValid(target))
		{
			_jumpGrid = target.gridPos;
			_jumpDirection = ((!(Scale.X * transformPoint.Scale.X >= 0f)) ? 1 : (-1));
			PlaceRake(_jumpGrid + new Vector2I(-_jumpDirection, 0));
		}
		base.Block(target);
	}

	public override void DestroySet()
	{
		if ((_deathRakePending || !jumpOver) && TowerDefenseManager.Instance != null)
		{
			PlaceRake(TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition()));
		}
		base.DestroySet();
	}

	public override void DieEntered()
	{
		if (!_rakePlaced)
		{
			_deathRakePending = true;
		}
		base.DieEntered();
	}

	private void PlaceRake(Vector2I at)
	{
		if (_rakePlaced || IsRemoteClient || !inGame || editorPreviewMode || suppressDeathrattles || !GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(at)))
		{
			return;
		}
		bool hypnoses = GodotObject.IsInstanceValid(instance) && instance.hypnoses;
		TowerDefenseEnum.CHARACTER_CAMP sourceCamp = camp;
		TowerDefenseCharacter rake = TowerDefenseManager.GetPacketConfig("ReverseRake").Plant(at, playAudio: false, noLimit: true, default, skipPlacementCheck: true);
		if (!GodotObject.IsInstanceValid(rake))
		{
			return;
		}
		_rakePlaced = true;
		_deathRakePending = false;
		RefreshRakeVisual();
		Dictionary spawnState = new Dictionary
		{
			["plant_skip_placement_check"] = true,
			["plant_play_audio"] = false,
			["hypnoses"] = hypnoses,
			["rake_camp"] = (int)sourceCamp
		};
		if (rake is TowerDefenseReverseRake towerDefenseReverseRake)
		{
			towerDefenseReverseRake.ImportNetworkSpawnState(spawnState);
		}
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(rake) && !rake.IsQueuedForDeletion() && !rake.isDestroy)
			{
				if (hypnoses && !rake.instance.hypnoses)
				{
					rake.Hypnoses();
				}
				rake.camp = sourceCamp;
				TowerDefenseManager.PublishSpawnedCharacter("ReverseRake", rake, useCreate: false, 0.0, walkAfterSpawn: false, "", spawnState);
			}
		}).CallDeferred();
	}

	public override void AnimeCompleted(string clip)
	{
		if (!IsRemoteClient || (!(clip == "Jump") && !(clip == "SwimJump")))
		{
			base.AnimeCompleted(clip);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["rakePlaced"] = _rakePlaced;
		dictionary["deathRakePending"] = _deathRakePending;
		dictionary["jumpGrid"] = _jumpGrid;
		dictionary["jumpDirection"] = _jumpDirection;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_rakePlaced = data.GetValueOrDefault("rakePlaced", false).AsBool();
		_deathRakePending = data.GetValueOrDefault("deathRakePending", false).AsBool();
		_jumpGrid = data.GetValueOrDefault("jumpGrid", gridPos).AsVector2I();
		_jumpDirection = data.GetValueOrDefault("jumpDirection", -1).AsInt32();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Dictionary dictionary = base.ExportNetworkSpecialState();
		dictionary["rakePlaced"] = _rakePlaced;
		dictionary["deathRakePending"] = _deathRakePending;
		return dictionary;
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		base.ImportNetworkSpecialState(data);
		_rakePlaced = data.GetValueOrDefault("rakePlaced", _rakePlaced).AsBool();
		_deathRakePending = data.GetValueOrDefault("deathRakePending", _deathRakePending).AsBool();
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return base.GetNetworkSpecialStateRevision() | (_rakePlaced ? 16 : 0) | (_deathRakePending ? 256 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRakeVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRakerAttachments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlaceRake, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "at", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRakeVisual && args.Count == 0)
		{
			RefreshRakeVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRakerAttachments && args.Count == 0)
		{
			RefreshRakerAttachments();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpEntered && args.Count == 0)
		{
			JumpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpExited && args.Count == 0)
		{
			JumpExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Block && args.Count == 1)
		{
			Block(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PlaceRake && args.Count == 1)
		{
			PlaceRake(VariantUtils.ConvertTo<Vector2I>(in args[0]));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.RefreshRakeVisual)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.RefreshRakerAttachments)
		{
			return true;
		}
		if (method == MethodName.JumpEntered)
		{
			return true;
		}
		if (method == MethodName.JumpExited)
		{
			return true;
		}
		if (method == MethodName.Block)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.PlaceRake)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._rakePlaced)
		{
			_rakePlaced = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._deathRakePending)
		{
			_deathRakePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rakeVisualHidden)
		{
			_rakeVisualHidden = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._jumpGrid)
		{
			_jumpGrid = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._jumpDirection)
		{
			_jumpDirection = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._rakePlaced)
		{
			value = VariantUtils.CreateFrom(in _rakePlaced);
			return true;
		}
		if (name == PropertyName._deathRakePending)
		{
			value = VariantUtils.CreateFrom(in _deathRakePending);
			return true;
		}
		if (name == PropertyName._rakeVisualHidden)
		{
			value = VariantUtils.CreateFrom(in _rakeVisualHidden);
			return true;
		}
		if (name == PropertyName._jumpGrid)
		{
			value = VariantUtils.CreateFrom(in _jumpGrid);
			return true;
		}
		if (name == PropertyName._jumpDirection)
		{
			value = VariantUtils.CreateFrom(in _jumpDirection);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._rakePlaced, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._deathRakePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rakeVisualHidden, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._jumpGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._jumpDirection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._rakePlaced, Variant.From(in _rakePlaced));
		info.AddProperty(PropertyName._deathRakePending, Variant.From(in _deathRakePending));
		info.AddProperty(PropertyName._rakeVisualHidden, Variant.From(in _rakeVisualHidden));
		info.AddProperty(PropertyName._jumpGrid, Variant.From(in _jumpGrid));
		info.AddProperty(PropertyName._jumpDirection, Variant.From(in _jumpDirection));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._rakePlaced, out var value))
		{
			_rakePlaced = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._deathRakePending, out var value2))
		{
			_deathRakePending = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rakeVisualHidden, out var value3))
		{
			_rakeVisualHidden = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._jumpGrid, out var value4))
		{
			_jumpGrid = value4.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._jumpDirection, out var value5))
		{
			_jumpDirection = value5.As<int>();
		}
	}
}
