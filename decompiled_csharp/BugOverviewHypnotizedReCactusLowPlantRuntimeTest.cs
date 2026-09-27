using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewHypnotizedReCactusLowPlantRuntimeTest.cs")]
public class BugOverviewHypnotizedReCactusLowPlantRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Place = "Place";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ReCactusScenePath = "res://Asset/Anime/Character/Plant/Chapter1/ReCactus/Scene/TowerDefensePlantReCactus.tscn";

	private const string PotatoMineScenePath = "res://Asset/Anime/Character/Plant/Chapter0/PotatoMine/Scene/TowerDefensePlantPotatoMine.tscn";

	private const string WallnutScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn";

	private static readonly Vector2I ShooterGrid = new Vector2I(7, 2);

	private static readonly Vector2I LowPlantGrid = new Vector2I(5, 2);

	private static readonly Vector2I NormalPlantGrid = new Vector2I(3, 2);

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProcessModeEnum previousProjectileProcessMode = ProjectileUpdateManager.Instance?.ProcessMode ?? ProcessModeEnum.Inherit;
		HypnotizedReCactusLowPlantRuntimeControlStub control = null;
		BulletField bulletField = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ObjectManager.Instance), "TowerDefenseManager and ObjectManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ObjectManager.Instance))
				{
					goto end_IL_00f4;
				}
				TowerDefenseProjectileRegistry.Init();
				if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
				{
					ProjectileUpdateManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
				}
				control = new HypnotizedReCactusLowPlantRuntimeControlStub
				{
					Name = "HypnotizedReCactusLowPlantRuntimeControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				TowerDefensePlantReCactus reCactus = Instantiate<TowerDefensePlantReCactus>("res://Asset/Anime/Character/Plant/Chapter1/ReCactus/Scene/TowerDefensePlantReCactus.tscn");
				TowerDefensePlantPotatoMine potatoMine = Instantiate<TowerDefensePlantPotatoMine>("res://Asset/Anime/Character/Plant/Chapter0/PotatoMine/Scene/TowerDefensePlantPotatoMine.tscn");
				TowerDefensePlantWallnut wallnut = Instantiate<TowerDefensePlantWallnut>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn");
				Check(GodotObject.IsInstanceValid(reCactus) && GodotObject.IsInstanceValid(potatoMine) && GodotObject.IsInstanceValid(wallnut) && reCactus.config?.name == "PlantReCactus" && potatoMine.config?.name == "PlantPotatoMine" && wallnut.config?.name == "PlantWallnut", "The regression must instantiate the real ReCactus, Potato Mine, and Wall-nut scenes.");
				if (!GodotObject.IsInstanceValid(reCactus) || !GodotObject.IsInstanceValid(potatoMine) || !GodotObject.IsInstanceValid(wallnut))
				{
					goto end_IL_00f4;
				}
				control.characterNode.AddChild(reCactus, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(potatoMine, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(wallnut, forceReadableName: false, InternalMode.Disabled);
				Place(reCactus, ShooterGrid);
				Place(potatoMine, LowPlantGrid);
				Place(wallnut, NormalPlantGrid);
				await WaitFrames(5);
				reCactus.Hypnoses();
				await WaitFrames(3);
				reCactus.ProcessMode = ProcessModeEnum.Disabled;
				potatoMine.ProcessMode = ProcessModeEnum.Disabled;
				wallnut.ProcessMode = ProcessModeEnum.Disabled;
				manager.CharacterRegister(reCactus);
				manager.CharacterRegister(potatoMine);
				manager.CharacterRegister(wallnut);
				reCactus.InvalidateHitBoxBounds();
				potatoMine.InvalidateHitBoxBounds();
				wallnut.InvalidateHitBoxBounds();
				Check(potatoMine.instance.height == TowerDefenseEnum.CHARACTER_HEIGHT.LOW && wallnut.instance.height == TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL, "The real Potato Mine must be LOW while the Wall-nut remains NORMAL.");
				Check(reCactus.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && reCactus.instance.hypnoses && reCactus.Scale.X < 0f, "The real ReCactus must be hypnotized into Zombie camp and face left.");
				Check(reCactus.CanTarget(potatoMine) && reCactus.CanTarget(wallnut), "Both opposing plants must remain camp-valid targets before height filtering.");
				FireComponent fire = reCactus.componentManager?.GetRuntime<FireComponent>("character.fire");
				Check(fire != null && !fire.IsReleased, "The real ReCactus FireComponent must be active.");
				if (fire == null || fire.IsReleased)
				{
					goto end_IL_00f4;
				}
				Check(fire.checkHeight, "ReCactus must enable height filtering for both target scans and live Spike collisions.");
				control.isGameRunning = true;
				fire.groundRight = 10000f;
				fire.timer = 0f;
				fire.checkInterval = 0;
				fire.checkIntreval = 0;
				int collectionFlag = 9;
				Check(fire.CanFireCheckOnce(null, collectionFlag) && fire.firstCharacter == wallnut, "The hypnotized ReCactus ray must skip the nearer LOW Potato Mine and select the NORMAL Wall-nut.");
				FireComponentCheckConfig groundCheck = ((fire.fireCheckList.Count > 1) ? fire.fireCheckList[1] : null);
				Check(GodotObject.IsInstanceValid(groundCheck) && groundCheck.useParentCollision, "The second authored ReCactus check must remain its ground-target path.");
				if (!GodotObject.IsInstanceValid(groundCheck))
				{
					goto end_IL_00f4;
				}
				bulletField = BulletField.EnsureMountedOnCharacterNode();
				Check(GodotObject.IsInstanceValid(bulletField), "The production BulletField must mount for the live Spike collision check.");
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					goto end_IL_00f4;
				}
				bulletField.ClearActiveBullets();
				await WaitFrames(2);
				fire.alive = true;
				fire.runningCheck = groundCheck;
				fire.runningCheckId = 1;
				fire.currentFireNum = 0;
				double hitpoints = potatoMine.instance.hitpoints;
				DispatchRealFireEvent(fire);
				int lastSpawnedIndex = bulletField.LastSpawnedIndex;
				Check(lastSpawnedIndex >= 0 && bulletField.IsBulletActive(lastSpawnedIndex), "The real ReCactus animation event must spawn a live Spike.");
				if (lastSpawnedIndex >= 0 && bulletField.IsBulletActive(lastSpawnedIndex))
				{
					ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(lastSpawnedIndex);
					Check(bulletDataRef.checkHeight && bulletDataRef.projectileHeight == TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL && bulletDataRef.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && bulletDataRef.vel.X < 0f, "The hypnotized Spike must retain NORMAL height filtering, Zombie camp, and leftward motion.");
				}
				else
				{
					Check(condition: false, "The spawned ReCactus Spike data must be inspectable.");
				}
				ulong physicsFrames = Engine.GetPhysicsFrames();
				bulletField.TeleportBullet(lastSpawnedIndex, potatoMine.WorldHitRect.GetCenter(), LowPlantGrid.Y);
				List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
				manager.characterRegistry.FillCharactersForWorldRectCandidatesList(potatoMine.WorldHitRect, list, LowPlantGrid.Y, includeAllLineCheck: true);
				Check(bulletField.IsBulletIntersectingRect(lastSpawnedIndex, potatoMine.WorldHitRect) && list.Contains(potatoMine), "The live Spike and registry broad phase must both overlap the LOW Potato Mine.");
				bulletField.Update(0.0, physicsFrames + 1);
				Check(Math.Abs(potatoMine.instance.hitpoints - hitpoints) < 0.001, $"The live Spike must pass over the LOW Potato Mine; before={hitpoints}, after={potatoMine.instance.hitpoints}.");
				goto end_IL_00dd;
				end_IL_00f4:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[HypnotizedReCactusLowPlant] Unexpected exception: {value}");
				goto end_IL_00dd;
			}
			return;
			end_IL_00dd:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(bulletField))
			{
				bulletField.ClearActiveBullets();
			}
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
			{
				ProjectileUpdateManager.Instance.ProcessMode = previousProjectileProcessMode;
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(8);
		}
		bool flag = _failures == 0 && _checks == 14;
		GD.Print($"HYPNOTIZED_RECACTUS_LOW_PLANT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static void Place(TowerDefenseCharacter character, Vector2I grid)
	{
		character.inGame = true;
		character.editorPreviewMode = false;
		character.gridPos = grid;
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		character.GlobalPosition = instance.gridBeginPos + new Vector2((float)grid.X * instance.gridSize.X, (float)grid.Y * instance.gridSize.Y);
	}

	private static void DispatchRealFireEvent(FireComponent fire)
	{
		string command = (string.IsNullOrWhiteSpace(fire.fireEventName) ? "Fire" : fire.fireEventName.Split('&', StringSplitOptions.RemoveEmptyEntries)[0]);
		fire.AnimeEvent(command, default);
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
			GD.PushError("[HypnotizedReCactusLowPlant] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Place, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.Place && args.Count == 2)
		{
			Place(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
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
		if (method == MethodName.Place && args.Count == 2)
		{
			Place(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
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
		if (method == MethodName.Place)
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
