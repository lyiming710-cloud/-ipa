using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/ReverseRake/Scene/TowerDefenseReverseRake.cs")]
public class TowerDefenseReverseRake : TowerDefenseGravestone, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public static readonly StringName TargetPriority = "TargetPriority";

		public static readonly StringName FindVictim = "FindVictim";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public static readonly StringName _triggered = "_triggered";

		public static readonly StringName _hitCompleted = "_hitCompleted";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private bool _triggered;

	private bool _hitCompleted;

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

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			RemoveFromGroup("Gravestone");
			TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
			if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
			{
				base.targetRegistrationComponent.canProjectileCheck = false;
			}
		}
	}

	public override void IdleEntered()
	{
		sprite.SetAnimation("Idle", loop: false);
		sprite.pause = !_triggered;
	}

	public override void ActivateGameplayProcessing()
	{
		base.ActivateGameplayProcessing();
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.pause = !_triggered;
		}
	}

	public static int TargetPriority(TowerDefenseCharacter target)
	{
		bool flag = !GodotObject.IsInstanceValid(target);
		if (!flag)
		{
			bool flag2 = ((target is TowerDefensePlant || target is TowerDefenseZombie) ? true : false);
			flag = !flag2;
		}
		if (flag || !target.inGame || target.editorPreviewMode || target.die || target.nearDie || target.isDestroy || target.IsQueuedForDeletion() || !GodotObject.IsInstanceValid(target.instance) || target.instance.hitpoints <= 0.0 || target.instance.invincible || target.instance.hologram || !target.instance.canBeCollection || !GodotObject.IsInstanceValid(target.config) || (target.instance.maskFlags & 2) != 0)
		{
			return 2147483647;
		}
		if (target is TowerDefenseZombie)
		{
			if ((target.instance.maskFlags & 1) == 0)
			{
				return 2147483647;
			}
			return 3;
		}
		if (target.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND))
		{
			return 0;
		}
		if (target.config.plantGridOverrideType == TowerDefenseEnum.PLANTGRIDTYPE.POT)
		{
			return 2;
		}
		return 1;
	}

	private TowerDefenseCharacter FindVictim()
	{
		TowerDefenseCellInstance towerDefenseCellInstance = (GodotObject.IsInstanceValid(cell) ? cell : TowerDefenseManager.GetMapCell(gridPos));
		if (!GodotObject.IsInstanceValid(towerDefenseCellInstance))
		{
			return null;
		}
		TowerDefenseCharacter result = null;
		int num = 2147483647;
		foreach (TowerDefenseCharacter character in towerDefenseCellInstance.GetCharacterList())
		{
			int num2 = TargetPriority(character);
			if (num2 < num && character.camp != camp)
			{
				num = num2;
				result = character;
			}
		}
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (num > 0 && GodotObject.IsInstanceValid(towerDefenseManager?.characterRegistry))
		{
			foreach (TowerDefenseCharacter charactersForLine in towerDefenseManager.characterRegistry.GetCharactersForLineList(gridPos.Y))
			{
				int num3 = TargetPriority(charactersForLine);
				if (num3 < num && charactersForLine.camp != camp && !((charactersForLine.ShouldUpdateGridPos() ? towerDefenseManager.GetMapGridPos(charactersForLine.GetLogicalGlobalPosition()) : charactersForLine.gridPos) != gridPos))
				{
					num = num3;
					result = charactersForLine;
				}
			}
		}
		return result;
	}

	public override void IdleProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (!IsRemoteClient && !_triggered && inGame && !editorPreviewMode && !(timeScale <= 0.0) && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && GodotObject.IsInstanceValid(FindVictim()))
		{
			_triggered = true;
			sprite.SetAnimation("Idle", loop: false);
			sprite.pause = false;
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (!(clip != "Idle") && _triggered && !_hitCompleted && !IsRemoteClient && !die && !isDestroy)
		{
			_hitCompleted = true;
			TowerDefenseCharacter towerDefenseCharacter = FindVictim();
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.Hurt(800.0);
			}
			AudioManager.Instance.AudioPlay("Bonk");
			Destroy();
		}
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (data != null && data.ContainsKey("rake_camp"))
		{
			TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = (TowerDefenseEnum.CHARACTER_CAMP)data["rake_camp"].AsInt32();
			TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP2 = ((!data.GetValueOrDefault("hypnoses", false).AsBool()) ? cHARACTER_CAMP : (cHARACTER_CAMP switch
			{
				TowerDefenseEnum.CHARACTER_CAMP.PLANT => TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE, 
				TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE => TowerDefenseEnum.CHARACTER_CAMP.PLANT, 
				_ => cHARACTER_CAMP, 
			}));
			camp = cHARACTER_CAMP2;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["triggered"] = _triggered;
		dictionary["hitCompleted"] = _hitCompleted;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_triggered = data.GetValueOrDefault("triggered", false).AsBool();
		_hitCompleted = data.GetValueOrDefault("hitCompleted", false).AsBool();
		if (_hitCompleted && !IsRemoteClient)
		{
			CallDeferred("Destroy");
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return ExportVariantSave();
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		_triggered = data.GetValueOrDefault("triggered", _triggered).AsBool();
		_hitCompleted = data.GetValueOrDefault("hitCompleted", _hitCompleted).AsBool();
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.pause = !_triggered;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TargetPriority, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindVictim, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.TargetPriority && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(TargetPriority(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.FindVictim && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindVictim());
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TargetPriority && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(TargetPriority(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.TargetPriority)
		{
			return true;
		}
		if (method == MethodName.FindVictim)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._triggered)
		{
			_triggered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitCompleted)
		{
			_hitCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._triggered)
		{
			value = VariantUtils.CreateFrom(in _triggered);
			return true;
		}
		if (name == PropertyName._hitCompleted)
		{
			value = VariantUtils.CreateFrom(in _hitCompleted);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._triggered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._triggered, Variant.From(in _triggered));
		info.AddProperty(PropertyName._hitCompleted, Variant.From(in _hitCompleted));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._triggered, out var value))
		{
			_triggered = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitCompleted, out var value2))
		{
			_hitCompleted = value2.As<bool>();
		}
	}
}
