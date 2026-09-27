using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Scene/IceShroom/TowerDefenseZombieZamboniIceShroom.cs")]
public class TowerDefenseZombieZamboniIceShroom : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName CreateEffect = "CreateEffect";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName GlobalPositionX = "GlobalPositionX";

		public static readonly StringName _zamboniSmoke = "_zamboniSmoke";

		public static readonly StringName _iceCapMarker = "_iceCapMarker";

		public static readonly StringName speed = "speed";

		public static readonly StringName audioPlay = "audioPlay";

		public static readonly StringName timer = "timer";

		public static readonly StringName time = "time";

		public static readonly StringName over = "over";

		public static readonly StringName explode = "explode";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private static PackedScene _ZAMBONI_EXPLOSION;

	private GpuParticles2D _zamboniSmoke;

	private Marker2D _iceCapMarker;

	public double speed = 30.0;

	public bool audioPlay;

	public double timer;

	public double time;

	public bool over;

	public bool explode;

	private static PackedScene ZAMBONI_EXPLOSION => _ZAMBONI_EXPLOSION ?? (_ZAMBONI_EXPLOSION = GD.Load<PackedScene>("uid://bbsti03vlotx6"));

	private float GlobalPositionX
	{
		get
		{
			return GetLogicalGlobalPosition().X;
		}
		set
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X = value;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["timer"] = timer;
		dictionary["time"] = time;
		dictionary["explode"] = explode;
		dictionary["over"] = over;
		dictionary["speed"] = speed;
		dictionary["audioPlay"] = audioPlay;
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		timer = Math.Max(0.0, data.GetValueOrDefault("timer", timer).AsDouble());
		if (data.ContainsKey("time"))
		{
			time = Math.Max(0.0, data["time"].AsDouble());
		}
		else if (time <= 0.0)
		{
			time = GD.RandRange(25.0, 30.0);
		}
		explode = data.GetValueOrDefault("explode", explode).AsBool();
		over = data.GetValueOrDefault("over", over).AsBool();
		speed = Math.Max(0.0, data.GetValueOrDefault("speed", speed).AsDouble());
		audioPlay = data.GetValueOrDefault("audioPlay", audioPlay).AsBool();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_zamboniSmoke = GetNode<GpuParticles2D>("%ZamboniSmoke");
			_iceCapMarker = GetNode<Marker2D>("%IceCapMarker");
			if (!IsProgressRestoreInFlight)
			{
				time = GD.RandRange(25.0, 30.0);
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || TowerDefenseManager.CurrentControl == null || !TowerDefenseManager.CurrentControl.isGameRunning || !IsInsideComponentBattlefield || sprite.pause)
		{
			return;
		}
		if (timer < time)
		{
			timer += delta * timeScale;
			return;
		}
		explode = true;
		if (!nearDie && !die)
		{
			TowerDefenseCharacter.CreateColdEffect(camp, gridPos);
			Destroy();
		}
	}

	public override void DestroySet()
	{
		if (!over)
		{
			over = true;
			AudioManager.Instance.AudioPlay("ReverseExplosion");
			sprite.GetNode<AdobeAnimateSprite>("Head").timeScale = 1.0;
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition());
			CreateEffect();
			Destroy();
		}
	}

	public override void WalkProcessing(double delta)
	{
		sprite.timeScale = timeScale * 0.5;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		if (!sprite.pause)
		{
			if ((double)GlobalPositionX > TowerDefenseManager.Instance.GetMapGroundRight())
			{
				GlobalPositionX -= (float)(speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * 2.0 * (double)((!sprite.playBack) ? 1 : (-1)));
			}
			else
			{
				GlobalPositionX -= (float)(speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * (double)((!sprite.playBack) ? 1 : (-1)));
			}
		}
		if (!audioPlay && (double)GlobalPositionX < TowerDefenseManager.Instance.GetMapGroundRight())
		{
			AudioManager.Instance.AudioPlay("Zamboni");
			audioPlay = true;
		}
		if (!instance.hypnoses)
		{
			TowerDefenseManager.Instance.SetIceCapPos(gridPos.Y, GetLogicalGlobalPosition(_iceCapMarker));
		}
		if (attackComponent.CanAttack())
		{
			if (attackComponent.target is TowerDefensePlant && GodotObject.IsInstanceValid(attackComponent.target.cell) && attackComponent.target.cell.HasSpike())
			{
				attackComponent.target = attackComponent.target.cell.GetSpike();
			}
			if ((attackComponent.target.instance.physiqueTypeFlags & 0x10) == 0)
			{
				attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
			}
			else
			{
				if (attackComponent.target.instance.spikeHurt != -1.0)
				{
					TowerDefenseCharacter target = attackComponent.target;
					double spikeHurt = attackComponent.target.instance.spikeHurt;
					target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
				}
				Die();
			}
		}
		if (instance.hitpoints < (config.hitpoints + config.hitpointsNearDeath) * 0.2)
		{
			if (speed > 5.0)
			{
				speed -= delta * 1.0;
			}
			((ZombieZamboniIceShroomSprite)sprite).shake = true;
		}
	}

	public override void HitpointsEmpty()
	{
		base.HitpointsEmpty();
		if (!isExplode)
		{
			Destroy();
		}
		else
		{
			spritePause = true;
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (!(damangePointName == "DamagePoint2"))
		{
			if (damangePointName == "DamagePoint3")
			{
				speed = 20.0;
				_zamboniSmoke.Visible = true;
			}
		}
		else
		{
			speed = 25.0;
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Wheelie"))
		{
			if (clip == "Wheelie2")
			{
				CreateEffect();
			}
		}
		else
		{
			CreateEffect();
		}
	}

	public void CreateEffect()
	{
		AudioManager.Instance.AudioPlay("ZamboniExplosion");
		ViewManager.Instance.CameraShake(new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)), 5.0, 0.05, 4);
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		if (!TowerDefenseManager.TryCreateEffectSceneOnceFast(ZAMBONI_EXPLOSION, node2D, gridPos, logicalGlobalPosition, preferSprite: false))
		{
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(ZAMBONI_EXPLOSION, gridPos);
			towerDefenseEffectParticlesOnce.GlobalPosition = logicalGlobalPosition;
			node2D.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantIceShroom");
		if (cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos);
			towerDefenseCharacter.WakeUp();
			if (instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantIceShroom", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsEmpty && args.Count == 0)
		{
			HitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateEffect && args.Count == 0)
		{
			CreateEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.HitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CreateEffect)
		{
			return true;
		}
		if (method == MethodName.Purify)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			GlobalPositionX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._zamboniSmoke)
		{
			_zamboniSmoke = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._iceCapMarker)
		{
			_iceCapMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			audioPlay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.explode)
		{
			explode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			value = VariantUtils.CreateFrom<float>(GlobalPositionX);
			return true;
		}
		if (name == PropertyName._zamboniSmoke)
		{
			value = VariantUtils.CreateFrom(in _zamboniSmoke);
			return true;
		}
		if (name == PropertyName._iceCapMarker)
		{
			value = VariantUtils.CreateFrom(in _iceCapMarker);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			value = VariantUtils.CreateFrom(in audioPlay);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.explode)
		{
			value = VariantUtils.CreateFrom(in explode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zamboniSmoke, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iceCapMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.audioPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.explode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.GlobalPositionX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GlobalPositionX, Variant.From<float>(GlobalPositionX));
		info.AddProperty(PropertyName._zamboniSmoke, Variant.From(in _zamboniSmoke));
		info.AddProperty(PropertyName._iceCapMarker, Variant.From(in _iceCapMarker));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.audioPlay, Variant.From(in audioPlay));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.explode, Variant.From(in explode));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GlobalPositionX, out var value))
		{
			GlobalPositionX = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName._zamboniSmoke, out var value2))
		{
			_zamboniSmoke = value2.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._iceCapMarker, out var value3))
		{
			_iceCapMarker = value3.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value4))
		{
			speed = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.audioPlay, out var value5))
		{
			audioPlay = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value6))
		{
			timer = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value7))
		{
			time = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value8))
		{
			over = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.explode, out var value9))
		{
			explode = value9.As<bool>();
		}
	}
}
