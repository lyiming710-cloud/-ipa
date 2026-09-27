using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/SpikeRockShieldDamageRuntimeTest.cs")]
public class SpikeRockShieldDamageRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SmashCell = "SmashCell";

		public static readonly StringName Bite = "Bite";

		public static readonly StringName DriveOverSpike = "DriveOverSpike";

		public static readonly StringName ApplyScaledDamage = "ApplyScaledDamage";

		public static readonly StringName CreateMap = "CreateMap";

		public static readonly StringName ReleaseCharacters = "ReleaseCharacters";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _control = "_control";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _cases = "_cases";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string SpikeRockPacketPath = "res://Asset/Anime/Character/Plant/Cover/SpikeRock/Packet/PlantSpikeRock.tres";

	private const string ShieldPacketPath = "res://Asset/Anime/Character/Item/Sheild/Packet/ItemSheild.tres";

	private const string GargantuarPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Base/ZombieGargantuar.tres";

	private const string ChomperPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Plant/ZombieNormalChomper.tres";

	private const string ZamboniPacketPath = "res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Packet/ZombieZamboni.tres";

	private static readonly Vector2I TargetGrid = new Vector2I(5, 3);

	private readonly List<TowerDefenseCharacter> _spawned = new List<TowerDefenseCharacter>();

	private SpikeRockShieldDamageControl _control;

	private int _checks;

	private int _failures;

	private int _cases;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager.currentControl;
		Vector2 previousBegin = manager.gridBeginPos;
		Vector2 previousSize = manager.gridSize;
		Vector2I previousNum = manager.gridNum;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap map = null;
		try
		{
			_ = 21;
			try
			{
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				_control = new SpikeRockShieldDamageControl
				{
					Name = "ShieldDamageControl",
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig(),
					characterNode = new Node2D()
				};
				AddChild(_control, forceReadableName: false, InternalMode.Disabled);
				_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = _control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					mapIceCap = new Node2D()
				};
				_control.AddChild(mapControl.mapIceCap, forceReadableName: false, InternalMode.Disabled);
				map = CreateMap(mapControl, manager.gridNum);
				map.control = _control;
				_control.featureDictionary[new StringName("Map")] = map;
				await RunCase("爆炸优先消耗护盾", 1000.0, 700.0, 450.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(300.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				});
				await RunCase("爆炸破盾后本体限伤", 100.0, 0.0, 400.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(300.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				});
				await RunCase("爆炸破盾后仅扣剩余伤害", 100.0, 0.0, 430.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(120.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				});
				await RunCase("爆炸恰好耗尽护盾", 300.0, 0.0, 450.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(300.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				});
				await RunCase("爆炸无盾仍限伤50", 0.0, 0.0, 400.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(300.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				});
				await RunCase("致命爆炸消耗一层", 2000.0, 1000.0, 450.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(1800.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				});
				await RunCase("直接粉碎消耗一层", 2000.0, 1000.0, 450.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.SmashHurt(10000.0, playSplatAudio: false);
				});
				await RunCase("粉碎无盾仍限伤50", 0.0, 0.0, 400.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.SmashHurt(10000.0, playSplatAudio: false);
				});
				await RunCase("普通伤害不受特殊限伤影响", 100.0, 0.0, 250.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.Hurt(300.0, playSplatAudio: false);
				});
				await RunCase("有限伤害先扣护盾", 1000.0, 700.0, 450.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.Hurt(300.0, playSplatAudio: false, default, createDamagePart: true, 50.0);
				});
				await RunCase("有限伤害溢出仍限伤50", 100.0, 0.0, 400.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.Hurt(300.0, playSplatAudio: false, default, createDamagePart: true, 50.0);
				});
				await RunCase("有限伤害只扣真实溢出", 100.0, 0.0, 430.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.Hurt(120.0, playSplatAudio: false, default, createDamagePart: true, 50.0);
				});
				await RunCase("伤害倍率和固定减伤保持生效", 100.0, 0.0, 425.0, ApplyScaledDamage);
				await RunCase("无盾倍率和固定减伤保持生效", 0.0, 0.0, 425.0, ApplyScaledDamage);
				await RunCase("敌方护盾不保护地刺王", 1000.0, 1000.0, 400.0, (TowerDefensePlantSpikeRock body, TowerDefenseZombie _) =>
				{
					body.ExplodeHurt(300.0, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio: false);
				}, null, opposingShield: true);
				await RunCase("巨人砸整格消耗一层", 2000.0, 1000.0, 450.0, SmashCell, "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Base/ZombieGargantuar.tres");
				await RunCase("巨人无盾仍限伤50", 0.0, 0.0, 400.0, SmashCell, "res://Asset/Anime/Character/Zombie/Chapter1/Gargantuar/Packet/Base/ZombieGargantuar.tres");
				await RunCase("大嘴吞咬消耗一层", 2000.0, 1000.0, 450.0, Bite, "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Plant/ZombieNormalChomper.tres");
				await RunCase("大嘴吞咬无盾仍限伤50", 0.0, 0.0, 400.0, Bite, "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Plant/ZombieNormalChomper.tres");
				await RunCase("冰车碾压消耗一层", 2000.0, 1000.0, 450.0, DriveOverSpike, "res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Packet/ZombieZamboni.tres");
				await RunCase("冰车无盾仍限伤50", 0.0, 0.0, 400.0, DriveOverSpike, "res://Asset/Anime/Character/Zombie/Chapter2/Zamboni/Packet/ZombieZamboni.tres");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"SPIKE_ROCK_SHIELD_EXCEPTION {value}");
			}
		}
		finally
		{
			ReleaseCharacters();
			await WaitFrames(3);
			manager.currentControl = previousControl;
			manager.gridBeginPos = previousBegin;
			manager.gridSize = previousSize;
			manager.gridNum = previousNum;
			map?.Destroy();
			mapControl?.Free();
			_control?.QueueFree();
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _cases == 21;
		GD.Print($"SPIKE_ROCK_SHIELD_DAMAGE_RESULT passed={flag} cases={_cases} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task RunCase(string name, double shieldHitpoints, double expectedShield, double expectedBody, Action<TowerDefensePlantSpikeRock, TowerDefenseZombie> damage, string attackerPacketPath = null, bool opposingShield = false)
	{
		ReleaseCharacters();
		await WaitFrames(3);
		TowerDefensePlantSpikeRock body = Spawn<TowerDefensePlantSpikeRock>("res://Asset/Anime/Character/Plant/Cover/SpikeRock/Packet/PlantSpikeRock.tres");
		TowerDefenseItemSheild shield = ((shieldHitpoints > 0.0) ? Spawn<TowerDefenseItemSheild>("res://Asset/Anime/Character/Item/Sheild/Packet/ItemSheild.tres") : null);
		TowerDefenseZombie attacker = ((attackerPacketPath == null) ? null : Spawn<TowerDefenseZombie>(attackerPacketPath));
		await WaitFrames(3);
		Check(body.instance.hitpoints == 450.0 && body.instance.smashHurt == 50.0 && body.instance.explosionHurt == 50.0 && body.instance.spikeHurt == 50.0 && body.instance.biteHurt == 50.0, name + "：必须使用正式地刺王的血量与限伤配置");
		if (shield != null)
		{
			shield.shieldHitpoints = shieldHitpoints;
			shield.instance.hypnoses = opposingShield;
			Check(body.cell.itemShield == shield, name + "：护盾必须注册在地刺王所在格子");
		}
		int bodyDamage = 0;
		body.OnBodyHurt += (int amount) =>
		{
			bodyDamage += amount;
		};
		_control.isGameRunning = true;
		damage(body, attacker);
		_control.isGameRunning = false;
		double num = shield?.instance.hitpoints ?? 0.0;
		double hitpoints = body.instance.hitpoints;
		Check(Math.Abs(num - expectedShield) < 0.001, $"{name}：护盾预期 {expectedShield}，实际 {num}");
		Check(Math.Abs(hitpoints - expectedBody) < 0.001, $"{name}：本体预期 {expectedBody}，实际 {hitpoints}");
		Check(bodyDamage == (int)(450.0 - expectedBody), $"{name}：本体受击事件预期 {450.0 - expectedBody}，实际 {bodyDamage}");
		_cases++;
		GD.Print($"SPIKE_ROCK_SHIELD_CASE name={name} shield={num} body={hitpoints} damageEvents={bodyDamage}");
	}

	private static void SmashCell(TowerDefensePlantSpikeRock body, TowerDefenseZombie attacker)
	{
		attacker.attackComponent.target = body;
		attacker.attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)attacker.config).smashAttack);
	}

	private static void Bite(TowerDefensePlantSpikeRock body, TowerDefenseZombie attacker)
	{
		body.instance.maskFlags |= attacker.instance.collisionFlags;
		((TowerDefenseZombieNormalChomper)attacker).chomperComponent.BitCharacter(body);
	}

	private void DriveOverSpike(TowerDefensePlantSpikeRock body, TowerDefenseZombie attacker)
	{
		attacker.attackComponent.target = body;
		attacker.WalkProcessing(0.0);
		Check(attacker.die || attacker.nearDie || attacker.isDestroy, "冰车必须实际触发地刺爆胎");
	}

	private static void ApplyScaledDamage(TowerDefensePlantSpikeRock body, TowerDefenseZombie attacker)
	{
		body.instance.dealHurtScale = 0.5;
		body.instance.dealHurtReduce = 5.0;
		body.Hurt(400.0, playSplatAudio: false, default, createDamagePart: true, 60.0);
	}

	private T Spawn<T>(string path) where T : TowerDefenseCharacter
	{
		if (!(GD.Load<TowerDefensePacketConfig>(path).Plant(TargetGrid, playAudio: false, noLimit: true, default, skipPlacementCheck: true) is T val))
		{
			throw new InvalidOperationException("正式卡片放置失败：" + path);
		}
		val.ProcessMode = ProcessModeEnum.Disabled;
		_spawned.Add(val);
		return val;
	}

	private static TowerDefenseBattleFeatureMap CreateMap(TowerDefenseMapControl control, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = new TowerDefenseBattleFeatureMap
		{
			mapControl = control,
			config = new TowerDefenseMapConfig
			{
				gridNum = gridNum,
				gridBeginPos = Vector2.Zero,
				gridSize = new Vector2(100f, 76f),
				plantOffset = 50.0
			}
		};
		towerDefenseBattleFeatureMap.mapConfig = towerDefenseBattleFeatureMap.config;
		control.mapFeature = towerDefenseBattleFeatureMap;
		towerDefenseBattleFeatureMap.PlantGridInit();
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j)).Init(new TowerDefenseCellConfig());
			}
		}
		return towerDefenseBattleFeatureMap;
	}

	private void ReleaseCharacters()
	{
		if (_control != null)
		{
			_control.isGameRunning = false;
		}
		foreach (TowerDefenseCharacter item in _spawned)
		{
			if (GodotObject.IsInstanceValid(item) && !item.IsQueuedForDeletion())
			{
				item.QueueFree();
			}
		}
		_spawned.Clear();
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SmashCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "body", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "attacker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Bite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "body", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "attacker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DriveOverSpike, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "body", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "attacker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyScaledDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "body", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "attacker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMap, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SmashCell && args.Count == 2)
		{
			SmashCell(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Bite && args.Count == 2)
		{
			Bite(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DriveOverSpike && args.Count == 2)
		{
			DriveOverSpike(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyScaledDamage && args.Count == 2)
		{
			ApplyScaledDamage(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateMap && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMap(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.ReleaseCharacters && args.Count == 0)
		{
			ReleaseCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SmashCell && args.Count == 2)
		{
			SmashCell(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Bite && args.Count == 2)
		{
			Bite(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyScaledDamage && args.Count == 2)
		{
			ApplyScaledDamage(VariantUtils.ConvertTo<TowerDefensePlantSpikeRock>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateMap && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMap(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
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
		if (method == MethodName.SmashCell)
		{
			return true;
		}
		if (method == MethodName.Bite)
		{
			return true;
		}
		if (method == MethodName.DriveOverSpike)
		{
			return true;
		}
		if (method == MethodName.ApplyScaledDamage)
		{
			return true;
		}
		if (method == MethodName.CreateMap)
		{
			return true;
		}
		if (method == MethodName.ReleaseCharacters)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<SpikeRockShieldDamageControl>(in value);
			return true;
		}
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cases)
		{
			_cases = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._cases)
		{
			value = VariantUtils.CreateFrom(in _cases);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cases, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._cases, Variant.From(in _cases));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._control, out var value))
		{
			_control = value.As<SpikeRockShieldDamageControl>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value2))
		{
			_checks = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value3))
		{
			_failures = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cases, out var value4))
		{
			_cases = value4.As<int>();
		}
	}
}
