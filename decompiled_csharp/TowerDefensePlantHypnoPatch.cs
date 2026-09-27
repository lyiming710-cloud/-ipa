using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Cover/HypnoPatch/Scene/TowerDefensePlantHypnoPatch.cs")]
public class TowerDefensePlantHypnoPatch : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName StartSummonedZombieWalking = "StartSummonedZombieWalking";

		public new static readonly StringName AttackDeal = "AttackDeal";

		public static readonly StringName ApplyCapturedVisualState = "ApplyCapturedVisualState";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName Cover = "Cover";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName over = "over";

		public static readonly StringName eatTime = "eatTime";

		public static readonly StringName spawnPacket = "spawnPacket";

		public static readonly StringName maxHitpoint = "maxHitpoint";

		public static readonly StringName spawnTimer = "spawnTimer";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string HYPNO_PATCH_BODY2 = "uid://trisvt6u5l4b";

	private const string HYPNO_PATCH_BODY3 = "uid://byo6orm75quhp";

	private const string HYPNO_PATCH_BODY = "uid://btsule42cwjrl";

	private const string HYPNO_PATCH_HEAD2 = "uid://bwfpe1t5e5b75";

	private const string HYPNO_PATCH_HEAD3 = "uid://by6prmaaf1ocu";

	private const string HYPNO_PATCH_HEAD = "uid://2hdb3rhrsahj";

	public bool over;

	public int eatTime;

	public TowerDefensePacketConfig spawnPacket;

	public double maxHitpoint = -1.0;

	public double spawnTimer;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			ApplyCapturedVisualState();
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (!TowerDefenseManager.HasGameplayAuthority || eatTime <= 0)
		{
			return;
		}
		spawnTimer += delta;
		if (!(spawnTimer >= 30.0) || !GodotObject.IsInstanceValid(spawnPacket))
		{
			return;
		}
		sprite.SetAnimation("Spawn", loop: false, 0.2);
		spawnTimer = 0.0;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseCharacter towerDefenseCharacter = (HasEconomyOwner ? spawnPacket.Create(EconomyOwnerAccountId, logicalGlobalPosition, gridPos, groundHeight) : spawnPacket.Create(logicalGlobalPosition, gridPos, groundHeight));
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
			Vector2 logicalGlobalPosition2 = towerDefenseCharacter.GetLogicalGlobalPosition();
			towerDefenseCharacter.SetLogicalGlobalPosition(new Vector2(logicalGlobalPosition.X, logicalGlobalPosition2.Y));
			if (!instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			TowerDefenseManager.PublishSpawnedCharacter(spawnPacket.saveKey, towerDefenseCharacter, useCreate: true, 0.0, walkAfterSpawn: true);
			Tween tween = towerDefenseCharacter.CreateTween();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Back);
			tween.SetParallel();
			tween.TweenProperty(towerDefenseCharacter.transformPoint, "scale", Vector2.One, 0.5).From(Vector2.One * 0.5f);
			towerDefenseCharacter.sprite.pause = false;
			towerDefenseCharacter.SetHitBoxMonitorable(monitorable: true);
			if (towerDefenseCharacter is TowerDefenseZombie zombie)
			{
				StartSummonedZombieWalking(zombie);
			}
		}
	}

	private async void StartSummonedZombieWalking(TowerDefenseZombie zombie)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (GodotObject.IsInstanceValid(zombie))
		{
			zombie.Walk();
		}
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		base.AttackDeal(character, type, num);
		if (!TowerDefenseManager.HasGameplayAuthority || instance.sleep || !GodotObject.IsInstanceValid(character) || !(type == "Eat"))
		{
			return;
		}
		character.Hypnoses();
		if (instance.hypnoses != character.instance.hypnoses)
		{
			eatTime++;
			double totalHitPoint = character.GetTotalHitPoint();
			if (totalHitPoint > maxHitpoint)
			{
				maxHitpoint = totalHitPoint;
				spawnPacket = character.packet;
			}
			ApplyCapturedVisualState();
		}
		if (eatTime >= 3)
		{
			Destroy();
		}
	}

	private void ApplyCapturedVisualState()
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			int num = Mathf.Clamp(eatTime, 0, 2);
			switch (num)
			{
			case 1:
				sprite.SetAtlasReplace("HypnoPatch_body.png", "uid://trisvt6u5l4b");
				sprite.SetAtlasReplace("HypnoPatch_head.png", "uid://bwfpe1t5e5b75");
				break;
			case 2:
				sprite.SetAtlasReplace("HypnoPatch_body.png", "uid://byo6orm75quhp");
				sprite.SetAtlasReplace("HypnoPatch_head.png", "uid://by6prmaaf1ocu");
				break;
			default:
				sprite.SetAtlasReplace("HypnoPatch_body.png", "uid://btsule42cwjrl");
				sprite.SetAtlasReplace("HypnoPatch_head.png", "uid://2hdb3rhrsahj");
				break;
			}
			sprite.SetFliters(new Array { "piece1" }, num < 1);
			sprite.SetFliters(new Array { "piece4_1", "piece4_2" }, num < 2);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Spawn")
		{
			Idle();
		}
	}

	public override void Cover(TowerDefenseCharacter character)
	{
		base.Cover(character);
		if (character.config.name == "PlantHypnoShroom" && character.instance.wakeUp)
		{
			instance.wakeUp = true;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			["over"] = over,
			["eatTime"] = eatTime,
			["maxHitpoint"] = maxHitpoint,
			["spawnTimer"] = spawnTimer
		};
		if (GodotObject.IsInstanceValid(spawnPacket))
		{
			dictionary["spawnPacketName"] = spawnPacket.saveKey;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		over = data.ContainsKey("over") && data["over"].AsBool();
		eatTime = (data.ContainsKey("eatTime") ? data["eatTime"].AsInt32() : 0);
		maxHitpoint = (data.ContainsKey("maxHitpoint") ? data["maxHitpoint"].AsDouble() : (-1.0));
		spawnTimer = (data.ContainsKey("spawnTimer") ? data["spawnTimer"].AsDouble() : 0.0);
		if (data.ContainsKey("spawnPacketName"))
		{
			spawnPacket = TowerDefenseManager.GetPacketConfig(data["spawnPacketName"].AsString());
		}
		ApplyCapturedVisualState();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Dictionary dictionary = base.ExportNetworkSpecialState();
		dictionary["eat_time"] = eatTime;
		return dictionary;
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (data != null)
		{
			base.ImportNetworkSpecialState(data);
			if (data.ContainsKey("eat_time"))
			{
				eatTime = data["eat_time"].AsInt32();
			}
			ApplyCapturedVisualState();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartSummonedZombieWalking, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCapturedVisualState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Cover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartSummonedZombieWalking && args.Count == 1)
		{
			StartSummonedZombieWalking(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCapturedVisualState && args.Count == 0)
		{
			ApplyCapturedVisualState();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Cover && args.Count == 1)
		{
			Cover(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.StartSummonedZombieWalking)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
		{
			return true;
		}
		if (method == MethodName.ApplyCapturedVisualState)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Cover)
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
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.eatTime)
		{
			eatTime = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spawnPacket)
		{
			spawnPacket = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.maxHitpoint)
		{
			maxHitpoint = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnTimer)
		{
			spawnTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.eatTime)
		{
			value = VariantUtils.CreateFrom(in eatTime);
			return true;
		}
		if (name == PropertyName.spawnPacket)
		{
			value = VariantUtils.CreateFrom(in spawnPacket);
			return true;
		}
		if (name == PropertyName.maxHitpoint)
		{
			value = VariantUtils.CreateFrom(in maxHitpoint);
			return true;
		}
		if (name == PropertyName.spawnTimer)
		{
			value = VariantUtils.CreateFrom(in spawnTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.eatTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spawnPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.maxHitpoint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.eatTime, Variant.From(in eatTime));
		info.AddProperty(PropertyName.spawnPacket, Variant.From(in spawnPacket));
		info.AddProperty(PropertyName.maxHitpoint, Variant.From(in maxHitpoint));
		info.AddProperty(PropertyName.spawnTimer, Variant.From(in spawnTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.over, out var value))
		{
			over = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.eatTime, out var value2))
		{
			eatTime = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spawnPacket, out var value3))
		{
			spawnPacket = value3.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.maxHitpoint, out var value4))
		{
			maxHitpoint = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnTimer, out var value5))
		{
			spawnTimer = value5.As<double>();
		}
	}
}
