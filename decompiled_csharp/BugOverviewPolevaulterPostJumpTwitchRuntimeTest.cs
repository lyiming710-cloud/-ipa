using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewPolevaulterPostJumpTwitchRuntimeTest.cs")]
public class BugOverviewPolevaulterPostJumpTwitchRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareCharacter = "PrepareCharacter";

		public static readonly StringName FacingMatches = "FacingMatches";

		public static readonly StringName Check = "Check";

		public static readonly StringName CheckClip = "CheckClip";

		public static readonly StringName CheckTransform = "CheckTransform";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _clipChecks = "_clipChecks";

		public static readonly StringName _clipFailures = "_clipFailures";

		public static readonly StringName _transformChecks = "_transformChecks";

		public static readonly StringName _transformFailures = "_transformFailures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string PolevaulterScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Polevaulter/Scene/TowerDefenseZombiePolevaulter.tscn";

	private const string WallnutScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn";

	private static readonly Vector2I EncounterGrid = new Vector2I(4, 2);

	private static readonly Vector2 EncounterPosition = new Vector2(400f, 252f);

	private int _checks;

	private int _failures;

	private int _clipChecks;

	private int _clipFailures;

	private int _transformChecks;

	private int _transformFailures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		bool previousBackZombie = manager?.backZombie ?? false;
		bool previousUseBatch = TowerDefenseZombie.UseBatch;
		BugOverviewPolevaulterPostJumpTwitchControlStub control = null;
		TowerDefenseZombiePolevaulter polevaulter = null;
		TowerDefensePlantWallnut wallnut = null;
		PackedScene polevaulterScene = null;
		PackedScene wallnutScene = null;
		int walkStartCount = 0;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					return;
				}
				control = new BugOverviewPolevaulterPostJumpTwitchControlStub
				{
					Name = "PolevaulterPostJumpTwitchControl",
					isGameRunning = false,
					isInit = true
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
				manager.backZombie = false;
				TowerDefenseZombie.UseBatch = false;
				polevaulterScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Polevaulter/Scene/TowerDefenseZombiePolevaulter.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				wallnutScene = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn", null, ResourceLoader.CacheMode.IgnoreDeep);
				Check(GodotObject.IsInstanceValid(polevaulterScene) && GodotObject.IsInstanceValid(wallnutScene), "The production Polevaulter and Wall-nut scenes must load.");
				polevaulter = polevaulterScene?.Instantiate<TowerDefenseZombiePolevaulter>(PackedScene.GenEditState.Disabled);
				wallnut = wallnutScene?.Instantiate<TowerDefensePlantWallnut>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(polevaulter) && GodotObject.IsInstanceValid(wallnut), "The production Polevaulter and Wall-nut scenes must instantiate.");
				if (!GodotObject.IsInstanceValid(polevaulter) || !GodotObject.IsInstanceValid(wallnut))
				{
					return;
				}
				PrepareCharacter(wallnut, EncounterGrid, EncounterPosition);
				PrepareCharacter(polevaulter, EncounterGrid, EncounterPosition);
				control.characterNode.AddChild(wallnut, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(polevaulter, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(5);
				polevaulter.ProcessMode = ProcessModeEnum.Disabled;
				wallnut.ProcessMode = ProcessModeEnum.Disabled;
				PrepareCharacter(wallnut, EncounterGrid, EncounterPosition);
				PrepareCharacter(polevaulter, EncounterGrid, EncounterPosition);
				polevaulter.groundMoveComponent?.SetAlive(false);
				await WaitFrames(2);
				Check(polevaulter.SceneFilePath == "res://Asset/Anime/Character/Zombie/Chapter1/Polevaulter/Scene/TowerDefenseZombiePolevaulter.tscn" && wallnut.SceneFilePath == "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn" && polevaulter.config?.name == "ZombiePolevaulter" && wallnut.config?.name == "PlantWallnut", "The fixture must use the reported Chapter1 Polevaulter and a real Wall-nut.");
				Check((polevaulter.StateMachine?.IsInitialized ?? false) && (polevaulter.sprite?.HasClip("Jump") ?? false) && polevaulter.sprite.HasClip(polevaulter.walkAnimeClip), "The real Polevaulter state machine and authored Jump/Walk clips must be active.");
				AttackComponent attackComponent = polevaulter.componentManager?.GetRuntime<AttackComponent>("character.attack.1");
				BugOverviewPolevaulterPostJumpTwitchRuntimeTest bugOverviewPolevaulterPostJumpTwitchRuntimeTest = this;
				int condition;
				if (attackComponent != null && !attackComponent.IsReleased && attackComponent.checkTall)
				{
					GroundMoveComponent groundMoveComponent = polevaulter.groundMoveComponent;
					if (groundMoveComponent != null && !groundMoveComponent.IsReleased && polevaulter.HasHitBox && wallnut.HasHitBox && polevaulter.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
					{
						condition = ((wallnut.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? 1 : 0);
						goto IL_0640;
					}
				}
				condition = 0;
				goto IL_0640;
				IL_0640:
				bugOverviewPolevaulterPostJumpTwitchRuntimeTest.Check((byte)condition != 0, "The authored jump targeter, movement source, camps, and collision bodies must be active.");
				BugOverviewPolevaulterPostJumpTwitchRuntimeTest bugOverviewPolevaulterPostJumpTwitchRuntimeTest2 = this;
				TowerDefenseCharacterInstance instance = wallnut.instance;
				bugOverviewPolevaulterPostJumpTwitchRuntimeTest2.Check(instance != null && instance.height < TowerDefenseEnum.CHARACTER_HEIGHT.TALL && polevaulter.gridPos == EncounterGrid && wallnut.gridPos == EncounterGrid, "The jump target must be a real short plant in the same encounter cell.");
				if (attackComponent == null || attackComponent.IsReleased)
				{
					goto end_IL_013f;
				}
				control.isGameRunning = true;
				attackComponent.target = wallnut;
				attackComponent.alive = true;
				attackComponent.timer = 0.0;
				attackComponent.checkIntrevalNow = 0;
				Check(attackComponent.HasAttackGridTargetCandidates() && attackComponent.CanAttack() && attackComponent.target == wallnut, "The real Polevaulter jump targeter must acquire the overlapping Wall-nut.");
				control.isGameRunning = false;
				polevaulter.sprite.OnAnimeStarted += TrackAnimationStart;
				bool flag = polevaulter.SendStateEvent("ToJump");
				CheckClip(flag && polevaulter.CurrentStateHandle?.StableId == "zombie.polevaulter.jump" && polevaulter.sprite.clip == "Jump", "The real state machine must enter the authored Jump clip.");
				polevaulter.AnimeEvent("check", default);
				polevaulter.AnimeEvent("jumpOver", default);
				float x = polevaulter.GlobalPosition.X;
				float outerScaleX = polevaulter.Scale.X;
				float transformScaleX = polevaulter.transformPoint.Scale.X;
				bool playbackBeforeLanding = polevaulter.sprite.playBack;
				float expectedLandingX = x - (float)((double)(outerScaleX * transformScaleX) * 148.0);
				polevaulter.AnimeCompleted("Jump");
				await WaitFrames(1);
				string settledClip = polevaulter.sprite.clip;
				string settledState = polevaulter.CurrentStateHandle?.StableId ?? string.Empty;
				float settledX = polevaulter.GlobalPosition.X;
				CheckClip(settledState == "zombie.walk" && settledClip == polevaulter.walkAnimeClip && walkStartCount == 1 && !polevaulter.jumpMove, "The first valid Jump completion must enter Walk exactly once.");
				CheckTransform(Mathf.IsEqualApprox(settledX, expectedLandingX), $"The first completion must apply exactly one authored 148px landing displacement; expected={expectedLandingX}, actual={settledX}.");
				CheckTransform(Mathf.IsEqualApprox(polevaulter.Scale.X, outerScaleX) && Mathf.IsEqualApprox(polevaulter.transformPoint.Scale.X, transformScaleX) && polevaulter.sprite.playBack == playbackBeforeLanding, "The valid landing must preserve the Polevaulter's authored facing.");
				await WaitFrames(8);
				CheckClip(polevaulter.CurrentStateHandle?.StableId == settledState && polevaulter.sprite.clip == settledClip && walkStartCount == 1, "The settled Walk clip must not restart while the old WalkEntered delay expires.");
				CheckTransform(Mathf.IsEqualApprox(polevaulter.GlobalPosition.X, settledX) && FacingMatches(polevaulter, outerScaleX, transformScaleX, playbackBeforeLanding), "The expired WalkEntered delay must not change the settled position or facing.");
				polevaulter.AnimeCompleted("Jump");
				CheckClip(polevaulter.CurrentStateHandle?.StableId == settledState && polevaulter.sprite.clip == settledClip && walkStartCount == 1, "A stale Jump completion must not re-enter or restart Walk.");
				CheckTransform(Mathf.IsEqualApprox(polevaulter.GlobalPosition.X, settledX) && FacingMatches(polevaulter, outerScaleX, transformScaleX, playbackBeforeLanding), "A stale Jump completion must not immediately apply a second movement or flip.");
				await WaitFrames(12);
				polevaulter.AnimeCompleted("Jump");
				await WaitFrames(2);
				CheckClip(polevaulter.CurrentStateHandle?.StableId == settledState && polevaulter.sprite.clip == settledClip && walkStartCount == 1, "Repeated late Jump completions must leave one uninterrupted Walk clip.");
				CheckTransform(Mathf.IsEqualApprox(polevaulter.GlobalPosition.X, settledX) && FacingMatches(polevaulter, outerScaleX, transformScaleX, playbackBeforeLanding), "Repeated late Jump completions must leave the settled transform unchanged.");
				goto end_IL_011c;
				end_IL_013f:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[{"BugOverviewPolevaulterPostJumpTwitchRuntimeTest"}] Unexpected exception: {value}");
				goto end_IL_011c;
			}
			return;
			end_IL_011c:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(polevaulter?.sprite))
			{
				polevaulter.sprite.OnAnimeStarted -= TrackAnimationStart;
			}
			if (GodotObject.IsInstanceValid(polevaulter))
			{
				polevaulter.QueueFree();
			}
			if (GodotObject.IsInstanceValid(wallnut))
			{
				wallnut.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
				manager.backZombie = previousBackZombie;
			}
			TowerDefenseZombie.UseBatch = previousUseBatch;
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			await WaitFrames(8);
			ObjectManager.Instance?.Clear();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			polevaulterScene?.Dispose();
			wallnutScene?.Dispose();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			await WaitFrames(4);
		}
		bool flag2 = _clipFailures == 0 && _clipChecks == 5;
		bool flag3 = _transformFailures == 0 && _transformChecks == 5;
		bool flag4 = (_failures == 0 && _checks == 18) & flag2 & flag3;
		GD.Print($"POLEVAULTER_POST_JUMP_CLIP_STABILITY_RESULT passed={flag2} checks={_clipChecks} failures={_clipFailures}");
		GD.Print($"POLEVAULTER_POST_JUMP_TRANSFORM_STABILITY_RESULT passed={flag3} checks={_transformChecks} failures={_transformFailures}");
		GD.Print($"BUG_OVERVIEW_POLEVAULTER_POST_JUMP_TWITCH_RESULT passed={flag4} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag4) ? 2 : 0);
		void TrackAnimationStart(string clip)
		{
			if (GodotObject.IsInstanceValid(polevaulter) && clip == polevaulter.walkAnimeClip)
			{
				walkStartCount++;
			}
		}
	}

	private static void PrepareCharacter(TowerDefenseCharacter character, Vector2I gridPos, Vector2 position)
	{
		character.editorPreviewMode = false;
		character.inGame = true;
		character.gridPos = gridPos;
		character.GlobalPosition = position;
	}

	private static bool FacingMatches(TowerDefenseZombiePolevaulter polevaulter, float outerScaleX, float transformScaleX, bool playback)
	{
		if (Mathf.IsEqualApprox(polevaulter.Scale.X, outerScaleX) && Mathf.IsEqualApprox(polevaulter.transformPoint.Scale.X, transformScaleX))
		{
			return polevaulter.sprite.playBack == playback;
		}
		return false;
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
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
			GD.PushError("[BugOverviewPolevaulterPostJumpTwitchRuntimeTest] " + message);
		}
	}

	private void CheckClip(bool condition, string message)
	{
		_checks++;
		_clipChecks++;
		if (!condition)
		{
			_failures++;
			_clipFailures++;
			GD.PushError("[PolevaulterPostJumpClipStability] " + message);
		}
	}

	private void CheckTransform(bool condition, string message)
	{
		_checks++;
		_transformChecks++;
		if (!condition)
		{
			_failures++;
			_transformFailures++;
			GD.PushError("[PolevaulterPostJumpTransformStability] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FacingMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "polevaulter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "outerScaleX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "transformScaleX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FacingMatches && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(FacingMatches(VariantUtils.ConvertTo<TowerDefenseZombiePolevaulter>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckClip && args.Count == 2)
		{
			CheckClip(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckTransform && args.Count == 2)
		{
			CheckTransform(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PrepareCharacter && args.Count == 3)
		{
			PrepareCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FacingMatches && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(FacingMatches(VariantUtils.ConvertTo<TowerDefenseZombiePolevaulter>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
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
		if (method == MethodName.PrepareCharacter)
		{
			return true;
		}
		if (method == MethodName.FacingMatches)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.CheckClip)
		{
			return true;
		}
		if (method == MethodName.CheckTransform)
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
		if (name == PropertyName._clipChecks)
		{
			_clipChecks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._clipFailures)
		{
			_clipFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._transformChecks)
		{
			_transformChecks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._transformFailures)
		{
			_transformFailures = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._clipChecks)
		{
			value = VariantUtils.CreateFrom(in _clipChecks);
			return true;
		}
		if (name == PropertyName._clipFailures)
		{
			value = VariantUtils.CreateFrom(in _clipFailures);
			return true;
		}
		if (name == PropertyName._transformChecks)
		{
			value = VariantUtils.CreateFrom(in _transformChecks);
			return true;
		}
		if (name == PropertyName._transformFailures)
		{
			value = VariantUtils.CreateFrom(in _transformFailures);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._clipChecks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._clipFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._transformChecks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._transformFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._clipChecks, Variant.From(in _clipChecks));
		info.AddProperty(PropertyName._clipFailures, Variant.From(in _clipFailures));
		info.AddProperty(PropertyName._transformChecks, Variant.From(in _transformChecks));
		info.AddProperty(PropertyName._transformFailures, Variant.From(in _transformFailures));
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
		if (info.TryGetProperty(PropertyName._clipChecks, out var value3))
		{
			_clipChecks = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._clipFailures, out var value4))
		{
			_clipFailures = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._transformChecks, out var value5))
		{
			_transformChecks = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._transformFailures, out var value6))
		{
			_transformFailures = value6.As<int>();
		}
	}
}
