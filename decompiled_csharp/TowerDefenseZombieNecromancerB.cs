using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter8/NecromancerB/Scene/TowerDefenseZombieNecromancerB.cs")]
public class TowerDefenseZombieNecromancerB : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ShootingEntered = "ShootingEntered";

		public static readonly StringName ShootingProcessing = "ShootingProcessing";

		public static readonly StringName ShootingExited = "ShootingExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName shootingTime = "shootingTime";

		public static readonly StringName shootingTimer = "shootingTimer";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _shootingStateHandle;

	private bool _roleStateSignalsConnected;

	[Export(PropertyHint.None, "")]
	public double shootingTime = 10.0;

	public double shootingTimer;

	private static PackedScene _SIZE_UP;

	private static bool HasGameplayAuthority
	{
		get
		{
			if (Global.IsMultiplayerMode)
			{
				return MultiPlayerManager.IsHost;
			}
			return true;
		}
	}

	private static PackedScene SIZE_UP => _SIZE_UP ?? (_SIZE_UP = GD.Load<PackedScene>("uid://bt1c4oos7k1vo"));

	private void ConnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_shootingStateHandle = StateMachine?.GetStateById("zombie.necromancer_b.shooting");
			StateHandle shootingStateHandle = _shootingStateHandle;
			if (shootingStateHandle != null && shootingStateHandle.IsValid)
			{
				_shootingStateHandle.Entered += ShootingEntered;
				_shootingStateHandle.Exited += ShootingExited;
				_shootingStateHandle.Processing += ShootingProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_shootingStateHandle != null)
			{
				_shootingStateHandle.Entered -= ShootingEntered;
				_shootingStateHandle.Exited -= ShootingExited;
				_shootingStateHandle.Processing -= ShootingProcessing;
			}
			_shootingStateHandle = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			ConnectRoleStateSignals();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["shootingTimer"] = shootingTimer;
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		shootingTimer = Math.Max(0.0, data.GetValueOrDefault("shootingTimer", shootingTimer).AsDouble());
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && HasGameplayAuthority && TowerDefenseManager.Instance.IsGameRunning() && inGame && IsInsideComponentBattlefield && !die && !nearDie && !sprite.pause)
		{
			if (shootingTimer < shootingTime)
			{
				shootingTimer += delta * timeScale;
				return;
			}
			SendStateEvent("ToShooting");
			shootingTimer = 0.0;
		}
	}

	public void ShootingEntered()
	{
		AudioManager.Instance.AudioPlay("Skeleton");
		sprite.SetAnimation("Shooting", loop: true, 0.2);
		if (!HasGameplayAuthority)
		{
			return;
		}
		Godot.Collections.Array campFriendlyLine = TowerDefenseManager.Instance.GetCampFriendlyLine(this);
		Godot.Collections.Array array = new Godot.Collections.Array();
		float x = GetLogicalGlobalPosition().X;
		foreach (Variant item in campFriendlyLine)
		{
			if (item.AsGodotObject() is TowerDefenseCharacter towerDefenseCharacter && towerDefenseCharacter != this && towerDefenseCharacter is TowerDefenseZombie && towerDefenseCharacter.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS && !(towerDefenseCharacter is TowerDefenseZombie { sizeUpNum: <=0 }) && towerDefenseCharacter.GetLogicalGlobalPosition().X > x == instance.hypnoses)
			{
				array.Add(towerDefenseCharacter);
			}
		}
		if (array.Count != 0)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = array[GD.RandRange(0, array.Count - 1)].AsGodotObject() as TowerDefenseCharacter;
			towerDefenseCharacter2.instance.hitpointScale *= 1.5;
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(SIZE_UP, towerDefenseCharacter2.gridPos, "Idle");
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.GlobalPosition = towerDefenseCharacter2.GetLogicalGlobalPosition();
			Tween tween = CreateTween();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Quint);
			tween.TweenProperty(towerDefenseCharacter2.transformPoint, "scale", 1.2f * towerDefenseCharacter2.transformPoint.Scale, 0.4);
			if (towerDefenseCharacter2 is TowerDefenseZombie towerDefenseZombie2)
			{
				towerDefenseZombie2.sizeUpNum--;
			}
		}
	}

	public virtual void ShootingProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.5;
	}

	public void ShootingExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Shooting")
		{
			Walk();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShootingEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShootingProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShootingExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
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
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
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
		if (method == MethodName.ShootingEntered && args.Count == 0)
		{
			ShootingEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ShootingProcessing && args.Count == 1)
		{
			ShootingProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShootingExited && args.Count == 0)
		{
			ShootingExited();
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
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
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
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
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
		if (method == MethodName.ShootingEntered)
		{
			return true;
		}
		if (method == MethodName.ShootingProcessing)
		{
			return true;
		}
		if (method == MethodName.ShootingExited)
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
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shootingTime)
		{
			shootingTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.shootingTimer)
		{
			shootingTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName.shootingTime)
		{
			value = VariantUtils.CreateFrom(in shootingTime);
			return true;
		}
		if (name == PropertyName.shootingTimer)
		{
			value = VariantUtils.CreateFrom(in shootingTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootingTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootingTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName.shootingTime, Variant.From(in shootingTime));
		info.AddProperty(PropertyName.shootingTimer, Variant.From(in shootingTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shootingTime, out var value2))
		{
			shootingTime = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.shootingTimer, out var value3))
		{
			shootingTimer = value3.As<double>();
		}
	}
}
