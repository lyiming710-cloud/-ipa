using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Portal/Scene/TowerDefenseZombiePortal.cs")]
public class TowerDefenseZombiePortal : TowerDefenseZombie, IProjectileZone
{
	private readonly struct PortalProjectileReleaseContext(BulletField bulletField, Vector2 spawnPosition, Vector2I destinationGrid, Rect2 projectileMapRect, double groundHeight, bool hypnoses, TowerDefenseEnum.CHARACTER_CAMP outputCamp, BulletFieldSpawnOverrides overrides)
	{
		public readonly BulletField BulletField = bulletField;

		public readonly Vector2 SpawnPosition = spawnPosition;

		public readonly Vector2I DestinationGrid = destinationGrid;

		public readonly Rect2 ProjectileMapRect = projectileMapRect;

		public readonly double GroundHeight = groundHeight;

		public readonly bool Hypnoses = hypnoses;

		public readonly TowerDefenseEnum.CHARACTER_CAMP OutputCamp = outputCamp;

		public readonly BulletFieldSpawnOverrides Overrides = overrides;
	}

	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName AttackEntered = "AttackEntered";

		public new static readonly StringName AttackExited = "AttackExited";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ArmorDamagePointReach = "ArmorDamagePointReach";

		public static readonly StringName UpdateRect = "UpdateRect";

		public static readonly StringName GetAbsorbTargetPosition = "GetAbsorbTargetPosition";

		public static readonly StringName OnProjectileIntersect = "OnProjectileIntersect";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName DieEntered = "DieEntered";

		public static readonly StringName ToPortal = "ToPortal";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName Portal = "Portal";

		public static readonly StringName ReleaseStoredProjectile = "ReleaseStoredProjectile";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName WorldRect = "WorldRect";

		public static readonly StringName GridY = "GridY";

		public static readonly StringName RowSpan = "RowSpan";

		public static readonly StringName halfHp = "halfHp";

		public static readonly StringName isAttack = "isAttack";

		public static readonly StringName timer = "timer";

		public static readonly StringName time = "time";

		public static readonly StringName over = "over";

		public static readonly StringName audioPlay = "audioPlay";

		public static readonly StringName absorbDuration = "absorbDuration";

		public static readonly StringName absorbSpin = "absorbSpin";

		public static readonly StringName absorbCurveSign = "absorbCurveSign";

		public static readonly StringName absorbCurveRatio = "absorbCurveRatio";

		public static readonly StringName _zoneRect = "_zoneRect";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public bool halfHp;

	public bool isAttack;

	public double timer;

	public double time = 20.0;

	public bool over;

	public bool audioPlay;

	[Export(PropertyHint.None, "")]
	public double absorbDuration = 0.26;

	[Export(PropertyHint.None, "")]
	public double absorbSpin = 13.0;

	[Export(PropertyHint.None, "")]
	public double absorbCurveSign = -1.0;

	[Export(PropertyHint.None, "")]
	public double absorbCurveRatio = 0.18;

	public List<TowerDefenseProjectileConfig> projectileConfigList = new List<TowerDefenseProjectileConfig>();

	private CharacterAabbAreaComponent _checkArea;

	private Rect2 _zoneRect;

	public Rect2 WorldRect => _zoneRect;

	public int GridY => gridPos.Y;

	public int RowSpan => 0;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		_checkArea = componentManager.GetRuntime<CharacterAabbAreaComponent>("character.aabb_area.0");
		_checkArea?.SetEnabled(!over);
		sprite.SetFliters((Array?)new Array<string> { "light1", "light2", "light3" }, open: true);
		if ((double)GD.Randf() > 0.5)
		{
			sprite.SetFliter("anim_tongue", open: true);
		}
		ConfigureWaterLineVisualLayers("Zombie_duckytube", "Zombie_whitewater", "Zombie_whitewater2");
		if (!over)
		{
			AddToGroup("ZombiePortalGroup");
			if (BulletField.Instance != null)
			{
				BulletField.Instance.RegisterZone(this);
			}
		}
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint() && BulletField.Instance != null)
		{
			BulletField.Instance.UnregisterZone(this);
		}
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning || !IsInsideComponentBattlefield)
		{
			return;
		}
		if (!audioPlay)
		{
			AudioManager.Instance.AudioPlay("Portal");
			audioPlay = true;
		}
		if (!over && !sprite.pause)
		{
			if (timer < time)
			{
				timer += delta * timeScale;
			}
			else
			{
				over = true;
				if (nearDie || die)
				{
					return;
				}
				AudioManager.Instance.AudioPlay("Portal");
				ToPortal();
			}
		}
		if (TowerDefenseManager.Instance.IsGameRunning())
		{
			_ = inGame;
		}
	}

	public override void AttackEntered()
	{
		base.AttackEntered();
		isAttack = true;
		if (over)
		{
			sprite.SetFliters((Array?)new Array<string> { "light1", "light2", "light3" }, open: false);
		}
		if (HasShield())
		{
			sprite.SetFliters((Array?)new Array<string> { "Zombie_outerarm_upper" }, open: true);
			if (!halfHp)
			{
				sprite.SetFliters((Array?)new Array<string> { "Zombie_outerarm_hand", "Zombie_outerarm_lower" }, open: true);
			}
		}
	}

	public override void AttackExited()
	{
		base.AttackExited();
		isAttack = false;
		if (HasShield())
		{
			sprite.SetFliters((Array?)new Array<string> { "Zombie_outerarm_upper", "Zombie_outerarm_hand", "Zombie_outerarm_lower" }, open: false);
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (damagePointName == "Arm")
		{
			halfHp = true;
			sprite.SetFliters((Array?)new Array<string> { "Zombie_outerarm_upper" }, open: true);
		}
	}

	public override void ArmorDamagePointReach(string armorName, int stage)
	{
		base.ArmorDamagePointReach(armorName, stage);
		if (armorName == "Portal" && over)
		{
			sprite.SetFliters((Array?)new Array<string> { "light1", "light2", "light3" }, open: false);
		}
		if (isAttack && HasShield() && stage > 0)
		{
			sprite.SetFliters((Array?)new Array<string> { "Zombie_outerarm_upper" }, open: true);
			if (!halfHp)
			{
				sprite.SetFliters((Array?)new Array<string> { "Zombie_outerarm_hand", "Zombie_outerarm_lower" }, open: true);
			}
		}
	}

	public void UpdateRect()
	{
		if (!IsInsideComponentBattlefield || over || _checkArea == null || !_checkArea.TryGetWorldRect(out _zoneRect))
		{
			_zoneRect = new Rect2(Vector2.Zero, Vector2.Zero);
		}
	}

	public void OnBulletIntersect(ref BulletData b, int index)
	{
		if (!IsInsideComponentBattlefield || b.portalReleased || b.absorbOpen || over)
		{
			return;
		}
		CharacterAabbAreaComponent checkArea = _checkArea;
		if (checkArea != null && checkArea.Enabled && b.active && b.config != null && !b.over && b.camp != camp && (b.fireMethodFlags & 2) == 0)
		{
			projectileConfigList.Add(b.config);
			if (BulletField.Instance != null && !BulletField.Instance.BeginAbsorb(index, GetAbsorbTargetPosition(), (float)absorbDuration, (float)absorbSpin, (float)absorbCurveSign, (float)absorbCurveRatio))
			{
				BulletField.Instance.Despawn(index);
			}
		}
	}

	private Vector2 GetAbsorbTargetPosition()
	{
		if (damagePartSlot.TryGetValue("Portal", out var value))
		{
			Node2D nodeOrNull = GetNodeOrNull<Node2D>(value.AsString());
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				return GetLogicalGlobalPosition(nodeOrNull);
			}
		}
		return _zoneRect.GetCenter();
	}

	public void OnProjectileIntersect(TowerDefenseProjectile projectile)
	{
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Portal")
		{
			over = true;
			ToPortal();
		}
	}

	public override void DieEntered()
	{
		base.DieEntered();
		over = true;
		ToPortal();
	}

	public void ToPortal()
	{
		if (BulletField.Instance != null)
		{
			BulletField.Instance.UnregisterZone(this);
		}
		_zoneRect = new Rect2(Vector2.Zero, Vector2.Zero);
		_checkArea?.SetEnabled(enabled: false);
		RemoveFromGroup("ZombiePortalGroup");
		if (!nearDie && !die && GetHasArmor("Portal"))
		{
			if (inWater)
			{
				sprite.SetAnimation("WaterClose", loop: false);
			}
			else
			{
				sprite.SetAnimation("Close", loop: false);
			}
		}
		else
		{
			Portal();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Close" || clip == "WaterClose")
		{
			Portal();
		}
	}

	public void Portal()
	{
		if (!nearDie && !die)
		{
			if (isAttack)
			{
				if (inWater && attackWaterAnimeClip != "")
				{
					sprite.SetAnimation(attackWaterAnimeClip, loop: true, 0.2);
				}
				else
				{
					sprite.SetAnimation(attackAnimeClip, loop: true, 0.2);
				}
			}
			else
			{
				sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
			}
		}
		sprite.SetFliters((Array?)new Array<string> { "light1", "light2", "light3" }, open: false);
		Array<Node> nodesInGroup = GetTree().GetNodesInGroup("ZombiePortalGroup");
		if (nodesInGroup.Count == 0)
		{
			return;
		}
		Node destination = nodesInGroup[GD.RandRange(0, nodesInGroup.Count - 1)];
		if (projectileConfigList.Count == 0)
		{
			return;
		}
		if (!TryPrepareProjectileRelease(destination, out var context))
		{
			projectileConfigList.Clear();
			return;
		}
		for (int i = 0; i < projectileConfigList.Count; i++)
		{
			TowerDefenseProjectileConfig towerDefenseProjectileConfig = projectileConfigList[i];
			if (towerDefenseProjectileConfig != null)
			{
				ReleaseStoredProjectilePrepared(towerDefenseProjectileConfig, in context);
			}
		}
		projectileConfigList.Clear();
	}

	internal int ReleaseStoredProjectile(TowerDefenseProjectileConfig storedProjectileConfig, GodotObject destination)
	{
		if (!TryPrepareProjectileRelease(destination, out var context))
		{
			return -1;
		}
		return ReleaseStoredProjectilePrepared(storedProjectileConfig, in context);
	}

	private bool TryPrepareProjectileRelease(GodotObject destination, out PortalProjectileReleaseContext context)
	{
		context = default;
		if (!(destination is TowerDefenseCharacter towerDefenseCharacter) || !GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			GD.PushError("[BulletField:E_PORTAL_DESTINATION_INVALID] zombie portal destination is unavailable");
			return false;
		}
		if (!GodotObject.IsInstanceValid(instance))
		{
			GD.PushError("[BulletField:E_PORTAL_SOURCE_INVALID] zombie portal source instance is unavailable");
			return false;
		}
		BulletField bulletField = BulletField.EnsureMountedOnCharacterNode();
		if (!GodotObject.IsInstanceValid(bulletField))
		{
			GD.PushError("[BulletField:E_FIELD_UNAVAILABLE] zombie portal could not mount BulletField");
			return false;
		}
		bool hypnoses = instance.hypnoses;
		Vector2I destinationGrid = towerDefenseCharacter.gridPos;
		BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
		{
			checkAllOverride = true,
			fireMethodFlagsOverride = 1,
			flipXOverride = (towerDefenseCharacter.Scale.X > 0f),
			portalReleasedOverride = true,
			gridYOverride = destinationGrid.Y
		};
		Vector2 spawnPosition = towerDefenseCharacter.GetLogicalGlobalPosition() + new Vector2(hypnoses ? 50f : (-50f), 20f);
		double num = GetGroundHeight(GetLogicalGlobalPosition().Y);
		TowerDefenseEnum.CHARACTER_CAMP outputCamp = (hypnoses ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		context = new PortalProjectileReleaseContext(bulletField, spawnPosition, destinationGrid, FireComponent.ComputeProjectileMapRect(), num, hypnoses, outputCamp, overrides);
		return true;
	}

	private static int ReleaseStoredProjectilePrepared(TowerDefenseProjectileConfig storedProjectileConfig, in PortalProjectileReleaseContext context)
	{
		float num = (context.Hypnoses ? ((float)GD.RandRange(250, 500)) : ((float)GD.RandRange(-500, -250)));
		Vector2 vector = new Vector2(num, 0f);
		double height = context.GroundHeight + (double)GD.RandRange(0, 70);
		return context.BulletField.TrySpawnFromConfig(storedProjectileConfig, context.SpawnPosition, vector, Mathf.Abs(num), null, context.OutputCamp, context.DestinationGrid, context.DestinationGrid.Y, context.ProjectileMapRect, null, collisionFlagsOverride: storedProjectileConfig?.collisionFlags ?? (-1), height: height, groundHeight: context.GroundHeight, fireLength: -1f, checkHeight: false, checkAll: false, useFall: false, useGravity: false, gravityScale: 1.5f, yOffsetTarget: 0f, yOffsetDuration: 0f, overrides: context.Overrides);
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		projectileConfigList.Clear();
	}

	public override Dictionary ExportVariantSave()
	{
		Array array = new Array();
		for (int i = 0; i < projectileConfigList.Count; i++)
		{
			TowerDefenseProjectileConfig towerDefenseProjectileConfig = projectileConfigList[i];
			if (GodotObject.IsInstanceValid(towerDefenseProjectileConfig) && !string.IsNullOrEmpty(towerDefenseProjectileConfig.name))
			{
				array.Add(towerDefenseProjectileConfig.name);
			}
		}
		return new Dictionary
		{
			["timer"] = timer,
			["time"] = time,
			["over"] = over,
			["audioPlay"] = audioPlay,
			["projectileConfigNames"] = array
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		timer = data.GetValueOrDefault("timer", 0.0).AsDouble();
		time = data.GetValueOrDefault("time", 20.0).AsDouble();
		over = data.GetValueOrDefault("over", false).AsBool();
		audioPlay = data.GetValueOrDefault("audioPlay", false).AsBool();
		_checkArea?.SetEnabled(!over);
		if (over)
		{
			if (BulletField.Instance != null)
			{
				BulletField.Instance.UnregisterZone(this);
			}
			RemoveFromGroup("ZombiePortalGroup");
		}
		projectileConfigList.Clear();
		foreach (Variant item in data.GetValueOrDefault("projectileConfigNames", new Array()).AsGodotArray())
		{
			string text = item.AsString();
			if (!string.IsNullOrEmpty(text))
			{
				TowerDefenseProjectileConfig projectileConfig = TowerDefenseManager.GetProjectileConfig(text);
				if (GodotObject.IsInstanceValid(projectileConfig))
				{
					projectileConfigList.Add(projectileConfig);
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(20)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAbsorbTargetPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnProjectileIntersect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToPortal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Portal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseStoredProjectile, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "storedProjectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "destination", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackExited && args.Count == 0)
		{
			AttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach && args.Count == 2)
		{
			ArmorDamagePointReach(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRect && args.Count == 0)
		{
			UpdateRect();
			ret = default;
			return true;
		}
		if (method == MethodName.GetAbsorbTargetPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetAbsorbTargetPosition());
			return true;
		}
		if (method == MethodName.OnProjectileIntersect && args.Count == 1)
		{
			OnProjectileIntersect(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ToPortal && args.Count == 0)
		{
			ToPortal();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Portal && args.Count == 0)
		{
			Portal();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseStoredProjectile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ReleaseStoredProjectile(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0]), VariantUtils.ConvertTo<GodotObject>(in args[1])));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.AttackExited)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.ArmorDamagePointReach)
		{
			return true;
		}
		if (method == MethodName.UpdateRect)
		{
			return true;
		}
		if (method == MethodName.GetAbsorbTargetPosition)
		{
			return true;
		}
		if (method == MethodName.OnProjectileIntersect)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.ToPortal)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Portal)
		{
			return true;
		}
		if (method == MethodName.ReleaseStoredProjectile)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.halfHp)
		{
			halfHp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isAttack)
		{
			isAttack = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.audioPlay)
		{
			audioPlay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.absorbDuration)
		{
			absorbDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.absorbSpin)
		{
			absorbSpin = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.absorbCurveSign)
		{
			absorbCurveSign = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.absorbCurveRatio)
		{
			absorbCurveRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._zoneRect)
		{
			_zoneRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.WorldRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(WorldRect);
			return true;
		}
		int from;
		if (name == PropertyName.GridY)
		{
			from = GridY;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RowSpan)
		{
			from = RowSpan;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.halfHp)
		{
			value = VariantUtils.CreateFrom(in halfHp);
			return true;
		}
		if (name == PropertyName.isAttack)
		{
			value = VariantUtils.CreateFrom(in isAttack);
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
		if (name == PropertyName.audioPlay)
		{
			value = VariantUtils.CreateFrom(in audioPlay);
			return true;
		}
		if (name == PropertyName.absorbDuration)
		{
			value = VariantUtils.CreateFrom(in absorbDuration);
			return true;
		}
		if (name == PropertyName.absorbSpin)
		{
			value = VariantUtils.CreateFrom(in absorbSpin);
			return true;
		}
		if (name == PropertyName.absorbCurveSign)
		{
			value = VariantUtils.CreateFrom(in absorbCurveSign);
			return true;
		}
		if (name == PropertyName.absorbCurveRatio)
		{
			value = VariantUtils.CreateFrom(in absorbCurveRatio);
			return true;
		}
		if (name == PropertyName._zoneRect)
		{
			value = VariantUtils.CreateFrom(in _zoneRect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.halfHp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isAttack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.audioPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.absorbDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.absorbSpin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.absorbCurveSign, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.absorbCurveRatio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._zoneRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.WorldRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GridY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RowSpan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.halfHp, Variant.From(in halfHp));
		info.AddProperty(PropertyName.isAttack, Variant.From(in isAttack));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.audioPlay, Variant.From(in audioPlay));
		info.AddProperty(PropertyName.absorbDuration, Variant.From(in absorbDuration));
		info.AddProperty(PropertyName.absorbSpin, Variant.From(in absorbSpin));
		info.AddProperty(PropertyName.absorbCurveSign, Variant.From(in absorbCurveSign));
		info.AddProperty(PropertyName.absorbCurveRatio, Variant.From(in absorbCurveRatio));
		info.AddProperty(PropertyName._zoneRect, Variant.From(in _zoneRect));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.halfHp, out var value))
		{
			halfHp = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isAttack, out var value2))
		{
			isAttack = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value3))
		{
			timer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value4))
		{
			time = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value5))
		{
			over = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.audioPlay, out var value6))
		{
			audioPlay = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.absorbDuration, out var value7))
		{
			absorbDuration = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.absorbSpin, out var value8))
		{
			absorbSpin = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.absorbCurveSign, out var value9))
		{
			absorbCurveSign = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.absorbCurveRatio, out var value10))
		{
			absorbCurveRatio = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._zoneRect, out var value11))
		{
			_zoneRect = value11.As<Rect2>();
		}
	}
}
