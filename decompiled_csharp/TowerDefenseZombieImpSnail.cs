using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter6/ImpSnail/Scene/TowerDefenseZombieImpSnail.cs")]
public class TowerDefenseZombieImpSnail : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName hasShell = "hasShell";

		public static readonly StringName _hasShell = "_hasShell";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private static PackedScene _ZOMBIE_SNAIL_SHELL;

	private bool _hasShell = true;

	public bool over;

	private static PackedScene ZOMBIE_SNAIL_SHELL => _ZOMBIE_SNAIL_SHELL ?? (_ZOMBIE_SNAIL_SHELL = GD.Load<PackedScene>("uid://bhwtjd6lkwqhh"));

	public bool hasShell
	{
		get
		{
			return _hasShell;
		}
		set
		{
			_hasShell = value;
			if (_hasShell)
			{
				dieAnimeClip = "Death2";
				dieWaterAnimeClip = "Death2";
			}
			else
			{
				dieAnimeClip = "Death";
				dieWaterAnimeClip = "Death";
			}
		}
	}

	public override void HitpointsNearDie()
	{
		if (!die)
		{
			Die();
		}
	}

	public override void _Ready()
	{
		for (int i = 0; i < currentArmor.Count; i++)
		{
			if (currentArmor[i] == "Shell")
			{
				currentArmor[i] = "ShellArmored";
			}
		}
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			instance.keepArmor = true;
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			if (TowerDefenseManager.Instance.IsGameRunning() && inGame && !die && !nearDie && GodotObject.IsInstanceValid(cell) && cell.isWater)
			{
				Die();
			}
		}
	}

	public override void DieEntered()
	{
		if (!die)
		{
			HitpointsEmpty();
			die = true;
		}
		if (!nearDie)
		{
			HitpointsNearDie();
			nearDie = true;
		}
		if (camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && GameSaveManager.Instance.GetFeatureValue("Coins") != 0)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = TowerDefenseManager.Instance.FallingObjectCreate(logicalGlobalPosition, GetGroundHeight(logicalGlobalPosition.Y), new Vector2((float)GD.RandRange(-50.0, 50.0), -400f), 980.0);
			if (towerDefenseGroundItemBase != null)
			{
				towerDefenseGroundItemBase.gridPos = gridPos;
			}
		}
		sprite.SetAnimation(dieAnimeClip, loop: false);
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "ShellArmored")
		{
			hasShell = false;
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(ZOMBIE_SNAIL_SHELL, gridPos, "Death");
			towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition();
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "hasShell", hasShell },
			{ "over", over }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		hasShell = data.GetValueOrDefault("hasShell", true).AsBool();
		over = data.GetValueOrDefault("over", false).AsBool();
	}

	public override async void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!dieAnimeClip.Split("&").Contains(clip) || over)
		{
			return;
		}
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			over = true;
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			Destroy();
			return;
		}
		over = true;
		if (!hasShell)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieSnailShell");
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseCharacter shell = packetConfig.Create(logicalGlobalPosition, gridPos);
		TowerDefenseGroundItemBase.characterNode.AddChild(shell, forceReadableName: false, InternalMode.Disabled);
		Vector2 _scale = transformPoint.Scale;
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(shell))
			{
				if (GodotObject.IsInstanceValid(shell.instance))
				{
					shell.instance.hitpointScale = instance.hitpointScale;
				}
				if (GodotObject.IsInstanceValid(shell.transformPoint))
				{
					shell.transformPoint.Scale = _scale;
				}
			}
		}).CallDeferred();
		shell.SetDeferred("invisible", invisible);
		TowerDefenseArmorInstance armorFromName = shell.GetArmorFromName("Shell");
		TowerDefenseArmorInstance armorFromName2 = GetArmorFromName("ShellArmored");
		armorFromName.Hurt(armorFromName2.damagePointBase - armorFromName2.hitPoints / armorFromName2.hitpointScale);
		if (instance.hypnoses)
		{
			shell.BuffAdd(new TowerDefenseCharacterBuffHypnoses
			{
				canFliter = false
			});
		}
		shell.CallDeferred("Walk");
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.HitpointsNearDie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HitpointsNearDie && args.Count == 0)
		{
			HitpointsNearDie();
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
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.HitpointsNearDie)
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
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
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
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.hasShell)
		{
			hasShell = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasShell)
		{
			_hasShell = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.hasShell)
		{
			value = VariantUtils.CreateFrom<bool>(hasShell);
			return true;
		}
		if (name == PropertyName._hasShell)
		{
			value = VariantUtils.CreateFrom(in _hasShell);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasShell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasShell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hasShell, Variant.From<bool>(hasShell));
		info.AddProperty(PropertyName._hasShell, Variant.From(in _hasShell));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hasShell, out var value))
		{
			hasShell = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasShell, out var value2))
		{
			_hasShell = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
	}
}
