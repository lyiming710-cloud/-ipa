using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter2/BobsledTeam/Scene/TowerDefenseZombieBobsledTeam.cs")]
public class TowerDefenseZombieBobsledTeam : TowerDefenseZombieBobsledVehicleBase
{
	public new class MethodName : TowerDefenseZombieBobsledVehicleBase.MethodName
	{
		public new static readonly StringName GetBreakClip = "GetBreakClip";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName ActivateLevelEntryPreview = "ActivateLevelEntryPreview";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName OnPhaseChanged = "OnPhaseChanged";

		public static readonly StringName UpdateEntryJumpPresentation = "UpdateEntryJumpPresentation";

		public static readonly StringName GetEntryJumpDriver = "GetEntryJumpDriver";

		public static readonly StringName ApplyRuntimeCrewVisibility = "ApplyRuntimeCrewVisibility";

		public static readonly StringName GetReleasedSlotMask = "GetReleasedSlotMask";

		public static readonly StringName ApplyLevelEntryPreviewPresentation = "ApplyLevelEntryPreviewPresentation";

		public static readonly StringName PrepareHeldPoseSprite = "PrepareHeldPoseSprite";

		public static readonly StringName ApplyCrewPositions = "ApplyCrewPositions";

		public static readonly StringName ResetCrewPositions = "ResetCrewPositions";

		public static readonly StringName RefreshCrewSprite = "RefreshCrewSprite";

		public static readonly StringName RefreshCompositeVisuals = "RefreshCompositeVisuals";

		public static readonly StringName SyncCrewVisualPresentation = "SyncCrewVisualPresentation";

		public static readonly StringName SyncVisualPresentation = "SyncVisualPresentation";

		public static readonly StringName BindEntryCompletionSprites = "BindEntryCompletionSprites";

		public static readonly StringName UnbindEntryCompletionSprites = "UnbindEntryCompletionSprites";

		public static readonly StringName OnEntryCrewCompleted = "OnEntryCrewCompleted";

		public static readonly StringName CompleteAlignedEntryJump = "CompleteAlignedEntryJump";

		public static readonly StringName CanCompleteEntryJumpPresentation = "CanCompleteEntryJumpPresentation";
	}

	public new class PropertyName : TowerDefenseZombieBobsledVehicleBase.PropertyName
	{
		public new static readonly StringName UsesBreakAnimationCompletion = "UsesBreakAnimationCompletion";

		public static readonly StringName _pushCrew = "_pushCrew";

		public static readonly StringName _seatCrew = "_seatCrew";

		public static readonly StringName _entryCompletionSpritesBound = "_entryCompletionSpritesBound";

		public static readonly StringName _entryJumpCompletionPending = "_entryJumpCompletionPending";
	}

	public new class SignalName : TowerDefenseZombieBobsledVehicleBase.SignalName
	{
	}

	private static readonly Vector2[] PushCrewPositions = new Vector2[4]
	{
		new Vector2(-27f, -49f),
		new Vector2(23f, -68f),
		new Vector2(73f, -52f),
		new Vector2(123f, -68f)
	};

	private static readonly Vector2[] JumpStartLocalPositions = new Vector2[4]
	{
		new Vector2(0f, 9f),
		new Vector2(0f, -8f),
		new Vector2(0f, 9f),
		new Vector2(0f, -8f)
	};

	private readonly AdobeAnimateSprite[] _pushCrew = new AdobeAnimateSprite[4];

	private readonly AdobeAnimateSprite[] _seatCrew = new AdobeAnimateSprite[4];

	private bool _entryCompletionSpritesBound;

	private bool _entryJumpCompletionPending;

	protected override bool UsesBreakAnimationCompletion => false;

	protected override string GetBreakClip()
	{
		return "Broken";
	}

	public override void _Ready()
	{
		base._Ready();
		for (int i = 0; i < 4; i++)
		{
			_pushCrew[i] = GetNodeOrNull<AdobeAnimateSprite>($"SpriteGroup/TransformPoint/ZombieBobsledVehicle/PushCrew{i}");
			_seatCrew[i] = GetNodeOrNull<AdobeAnimateSprite>($"SpriteGroup/TransformPoint/ZombieBobsledVehicle/Passenger{i}Slot/SeatCrew{i}");
		}
		BindEntryCompletionSprites();
		OnPhaseChanged(CurrentPhase);
		SyncCrewVisualPresentation();
	}

	public override void _ExitTree()
	{
		UnbindEntryCompletionSprites();
		base._ExitTree();
	}

	public override void ActivateLevelEntryPreview(string animationClip)
	{
		base.ActivateLevelEntryPreview(animationClip);
		ApplyLevelEntryPreviewPresentation();
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		UpdateEntryJumpPresentation();
		SyncCrewVisualPresentation();
		if (!isShow)
		{
			ApplyRuntimeCrewVisibility(CurrentPhase);
		}
		if (_entryJumpCompletionPending && CanCompleteEntryJumpPresentation())
		{
			_entryJumpCompletionPending = false;
			CompleteAlignedEntryJump();
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		BobsledVehiclePhase currentPhase = CurrentPhase;
		bool flag = (uint)(currentPhase - 3) <= 1u;
		if (!flag && GodotObject.IsInstanceValid(sprite))
		{
			if (damagePointName == "DamagePoint2")
			{
				sprite.SetAnimation("Damaged1");
			}
			else if (damagePointName == "DamagePoint3")
			{
				sprite.SetAnimation("Damaged2");
			}
		}
	}

	protected override void OnPhaseChanged(BobsledVehiclePhase phase)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (phase != BobsledVehiclePhase.EnteringJump)
		{
			_entryJumpCompletionPending = false;
		}
		if (isShow)
		{
			ApplyLevelEntryPreviewPresentation();
			return;
		}
		switch (phase)
		{
		case BobsledVehiclePhase.EnteringPush:
			ApplyCrewPositions(_pushCrew, PushCrewPositions);
			break;
		case BobsledVehiclePhase.EnteringJump:
			ApplyCrewPositions(_seatCrew, JumpStartLocalPositions);
			break;
		case BobsledVehiclePhase.Riding:
			ResetCrewPositions(_seatCrew);
			break;
		}
		for (int i = 0; i < 4; i++)
		{
			if (GodotObject.IsInstanceValid(_pushCrew[i]))
			{
				_pushCrew[i].SetFrozenPreview(frozen: false);
				if (phase == BobsledVehiclePhase.EnteringPush)
				{
					_pushCrew[i].SetAnimation("Push");
				}
				RefreshCrewSprite(_pushCrew[i]);
			}
			if (GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				_seatCrew[i].SetFrozenPreview(frozen: false);
				switch (phase)
				{
				case BobsledVehiclePhase.EnteringJump:
					_seatCrew[i].SetAnimation("Jump", loop: false);
					break;
				case BobsledVehiclePhase.Riding:
					PrepareHeldPoseSprite(_seatCrew[i], "Jump", frozenPreview: false);
					break;
				default:
					RefreshCrewSprite(_seatCrew[i]);
					break;
				}
			}
		}
		ApplyRuntimeCrewVisibility(phase);
		RefreshCompositeVisuals();
	}

	private void UpdateEntryJumpPresentation()
	{
		if (CurrentPhase != BobsledVehiclePhase.EnteringJump)
		{
			return;
		}
		AdobeAnimateSprite entryJumpDriver = GetEntryJumpDriver();
		if (!GodotObject.IsInstanceValid(entryJumpDriver))
		{
			if (CanCompleteEntryJumpPresentation())
			{
				CompleteAlignedEntryJump();
			}
			else
			{
				_entryJumpCompletionPending = true;
			}
			return;
		}
		int x = entryJumpDriver.clipRange.X;
		int num = Mathf.Max(x, entryJumpDriver.clipRange.Y - 1);
		float weight = ((num <= x) ? 1f : Mathf.Clamp(((float)(entryJumpDriver.frameIndex - x) + Mathf.Clamp((float)entryJumpDriver.elapsedTimer, 0f, 1f)) / (float)(num - x), 0f, 1f));
		for (int i = 0; i < 4; i++)
		{
			if (GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				_seatCrew[i].Position = JumpStartLocalPositions[i].Lerp(Vector2.Zero, weight);
			}
		}
	}

	private AdobeAnimateSprite GetEntryJumpDriver()
	{
		int releasedSlotMask = GetReleasedSlotMask();
		for (int i = 0; i < _seatCrew.Length; i++)
		{
			if ((releasedSlotMask & (1 << i)) == 0 && GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				return _seatCrew[i];
			}
		}
		return null;
	}

	private void ApplyRuntimeCrewVisibility(BobsledVehiclePhase phase)
	{
		bool flag = GodotObject.IsInstanceValid(sprite) && sprite.Visible;
		bool flag2 = isPause || !GodotObject.IsInstanceValid(sprite) || sprite.pause;
		bool flag3 = phase == BobsledVehiclePhase.EnteringPush;
		bool flag4 = (uint)(phase - 1) <= 1u;
		bool flag5 = flag4;
		int releasedSlotMask = GetReleasedSlotMask();
		for (int i = 0; i < 4; i++)
		{
			bool flag6 = (releasedSlotMask & (1 << i)) != 0;
			if (GodotObject.IsInstanceValid(_pushCrew[i]))
			{
				_pushCrew[i].Visible = (flag & flag3) && !flag6;
				_pushCrew[i].pause = !flag3 | flag2;
			}
			if (GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				bool flag7 = flag5 && !flag6;
				_seatCrew[i].Visible = flag & flag7;
				_seatCrew[i].pause = !flag7 | flag2;
			}
		}
	}

	private int GetReleasedSlotMask()
	{
		if (TeamComponent == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (TeamComponent.IsSlotReleased(i))
			{
				num |= 1 << i;
			}
		}
		return num;
	}

	private void ApplyLevelEntryPreviewPresentation()
	{
		ResetCrewPositions(_seatCrew);
		for (int i = 0; i < 4; i++)
		{
			if (GodotObject.IsInstanceValid(_pushCrew[i]))
			{
				_pushCrew[i].Visible = false;
				_pushCrew[i].SetFrozenPreview(frozen: true);
				RefreshCrewSprite(_pushCrew[i]);
			}
			if (GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				AdobeAnimateSprite obj = _seatCrew[i];
				obj.Visible = true;
				PrepareHeldPoseSprite(obj, "Jump", frozenPreview: true);
			}
		}
		RefreshCompositeVisuals();
	}

	private static void PrepareHeldPoseSprite(AdobeAnimateSprite visual, string clip, bool frozenPreview)
	{
		if (frozenPreview)
		{
			visual.ProcessMode = ProcessModeEnum.Always;
		}
		visual.SetFrozenPreview(frozen: false);
		visual.SetAnimation(clip, loop: false);
		visual.frameIndex = Mathf.Max(visual.clipRange.X, visual.clipRange.Y - 1);
		visual.elapsedTimer = 0.0;
		visual.clipOver = true;
		visual.UpdateMediaReplaceData();
		visual.UpdateChild();
		visual.RefreshManagedSlotSpriteCacheForRender();
		visual.SetFrozenPreview(frozenPreview);
		RefreshCrewSprite(visual);
	}

	private static void ApplyCrewPositions(AdobeAnimateSprite[] crew, Vector2[] positions)
	{
		for (int i = 0; i < crew.Length && i < positions.Length; i++)
		{
			if (GodotObject.IsInstanceValid(crew[i]))
			{
				crew[i].Position = positions[i];
			}
		}
	}

	private static void ResetCrewPositions(AdobeAnimateSprite[] crew)
	{
		for (int i = 0; i < crew.Length; i++)
		{
			if (GodotObject.IsInstanceValid(crew[i]))
			{
				crew[i].Position = Vector2.Zero;
			}
		}
	}

	private static void RefreshCrewSprite(AdobeAnimateSprite crew)
	{
		if (GodotObject.IsInstanceValid(crew))
		{
			crew.RefreshProcessScheduling();
			crew.QueueRedraw();
		}
	}

	private void RefreshCompositeVisuals()
	{
		RefreshCrewSprite(sprite);
		SyncCrewVisualPresentation();
	}

	private void SyncCrewVisualPresentation()
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			for (int i = 0; i < 4; i++)
			{
				SyncVisualPresentation(_pushCrew[i]);
				SyncVisualPresentation(_seatCrew[i]);
			}
		}
	}

	private void SyncVisualPresentation(AdobeAnimateSprite visual)
	{
		if (GodotObject.IsInstanceValid(visual) && visual != sprite)
		{
			if (visual.meshColor != sprite.meshColor)
			{
				visual.meshColor = sprite.meshColor;
			}
			if (visual.LightMask != sprite.LightMask)
			{
				visual.LightMask = sprite.LightMask;
			}
			if (!Mathf.IsEqualApprox((float)visual.timeScale, (float)sprite.timeScale))
			{
				visual.timeScale = sprite.timeScale;
			}
			if (visual.playBack != sprite.playBack)
			{
				visual.playBack = sprite.playBack;
			}
		}
	}

	private void BindEntryCompletionSprites()
	{
		if (Engine.IsEditorHint() || _entryCompletionSpritesBound)
		{
			return;
		}
		for (int i = 0; i < _seatCrew.Length; i++)
		{
			if (GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				_seatCrew[i].OnAnimeCompleted += OnEntryCrewCompleted;
			}
		}
		_entryCompletionSpritesBound = true;
	}

	private void UnbindEntryCompletionSprites()
	{
		if (!_entryCompletionSpritesBound)
		{
			return;
		}
		for (int i = 0; i < _seatCrew.Length; i++)
		{
			if (GodotObject.IsInstanceValid(_seatCrew[i]))
			{
				_seatCrew[i].OnAnimeCompleted -= OnEntryCrewCompleted;
			}
		}
		_entryCompletionSpritesBound = false;
	}

	private void OnEntryCrewCompleted(string clip)
	{
		if (!(clip != "Jump") && CurrentPhase == BobsledVehiclePhase.EnteringJump)
		{
			if (!CanCompleteEntryJumpPresentation())
			{
				_entryJumpCompletionPending = true;
			}
			else
			{
				CompleteAlignedEntryJump();
			}
		}
	}

	private void CompleteAlignedEntryJump()
	{
		ResetCrewPositions(_seatCrew);
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			CompleteEntryJump();
		}
	}

	private bool CanCompleteEntryJumpPresentation()
	{
		if (!isPause && GodotObject.IsInstanceValid(sprite) && !sprite.pause)
		{
			return !Mathf.IsZeroApprox((float)sprite.timeScale);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName.GetBreakClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateLevelEntryPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "animationClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPhaseChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateEntryJumpPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEntryJumpDriver, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRuntimeCrewVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetReleasedSlotMask, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyLevelEntryPreviewPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareHeldPoseSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "visual", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "frozenPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCrewPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "crew", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedVector2Array, "positions", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCrewPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "crew", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCrewSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "crew", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCompositeVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncCrewVisualPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncVisualPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "visual", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindEntryCompletionSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnbindEntryCompletionSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEntryCrewCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompleteAlignedEntryJump, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCompleteEntryJumpPresentation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetBreakClip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetBreakClip());
			return true;
		}
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
		if (method == MethodName.ActivateLevelEntryPreview && args.Count == 1)
		{
			ActivateLevelEntryPreview(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPhaseChanged && args.Count == 1)
		{
			OnPhaseChanged(VariantUtils.ConvertTo<BobsledVehiclePhase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateEntryJumpPresentation && args.Count == 0)
		{
			UpdateEntryJumpPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEntryJumpDriver && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetEntryJumpDriver());
			return true;
		}
		if (method == MethodName.ApplyRuntimeCrewVisibility && args.Count == 1)
		{
			ApplyRuntimeCrewVisibility(VariantUtils.ConvertTo<BobsledVehiclePhase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetReleasedSlotMask && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetReleasedSlotMask());
			return true;
		}
		if (method == MethodName.ApplyLevelEntryPreviewPresentation && args.Count == 0)
		{
			ApplyLevelEntryPreviewPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareHeldPoseSprite && args.Count == 3)
		{
			PrepareHeldPoseSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCrewPositions && args.Count == 2)
		{
			ApplyCrewPositions(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<Vector2[]>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCrewPositions && args.Count == 1)
		{
			ResetCrewPositions(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCrewSprite && args.Count == 1)
		{
			RefreshCrewSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCompositeVisuals && args.Count == 0)
		{
			RefreshCompositeVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCrewVisualPresentation && args.Count == 0)
		{
			SyncCrewVisualPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncVisualPresentation && args.Count == 1)
		{
			SyncVisualPresentation(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindEntryCompletionSprites && args.Count == 0)
		{
			BindEntryCompletionSprites();
			ret = default;
			return true;
		}
		if (method == MethodName.UnbindEntryCompletionSprites && args.Count == 0)
		{
			UnbindEntryCompletionSprites();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEntryCrewCompleted && args.Count == 1)
		{
			OnEntryCrewCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteAlignedEntryJump && args.Count == 0)
		{
			CompleteAlignedEntryJump();
			ret = default;
			return true;
		}
		if (method == MethodName.CanCompleteEntryJumpPresentation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCompleteEntryJumpPresentation());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PrepareHeldPoseSprite && args.Count == 3)
		{
			PrepareHeldPoseSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCrewPositions && args.Count == 2)
		{
			ApplyCrewPositions(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<Vector2[]>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCrewPositions && args.Count == 1)
		{
			ResetCrewPositions(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCrewSprite && args.Count == 1)
		{
			RefreshCrewSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetBreakClip)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ActivateLevelEntryPreview)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.OnPhaseChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateEntryJumpPresentation)
		{
			return true;
		}
		if (method == MethodName.GetEntryJumpDriver)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeCrewVisibility)
		{
			return true;
		}
		if (method == MethodName.GetReleasedSlotMask)
		{
			return true;
		}
		if (method == MethodName.ApplyLevelEntryPreviewPresentation)
		{
			return true;
		}
		if (method == MethodName.PrepareHeldPoseSprite)
		{
			return true;
		}
		if (method == MethodName.ApplyCrewPositions)
		{
			return true;
		}
		if (method == MethodName.ResetCrewPositions)
		{
			return true;
		}
		if (method == MethodName.RefreshCrewSprite)
		{
			return true;
		}
		if (method == MethodName.RefreshCompositeVisuals)
		{
			return true;
		}
		if (method == MethodName.SyncCrewVisualPresentation)
		{
			return true;
		}
		if (method == MethodName.SyncVisualPresentation)
		{
			return true;
		}
		if (method == MethodName.BindEntryCompletionSprites)
		{
			return true;
		}
		if (method == MethodName.UnbindEntryCompletionSprites)
		{
			return true;
		}
		if (method == MethodName.OnEntryCrewCompleted)
		{
			return true;
		}
		if (method == MethodName.CompleteAlignedEntryJump)
		{
			return true;
		}
		if (method == MethodName.CanCompleteEntryJumpPresentation)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._entryCompletionSpritesBound)
		{
			_entryCompletionSpritesBound = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._entryJumpCompletionPending)
		{
			_entryJumpCompletionPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.UsesBreakAnimationCompletion)
		{
			value = VariantUtils.CreateFrom<bool>(UsesBreakAnimationCompletion);
			return true;
		}
		if (name == PropertyName._pushCrew)
		{
			GodotObject[] pushCrew = _pushCrew;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(pushCrew);
			return true;
		}
		if (name == PropertyName._seatCrew)
		{
			GodotObject[] pushCrew = _seatCrew;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(pushCrew);
			return true;
		}
		if (name == PropertyName._entryCompletionSpritesBound)
		{
			value = VariantUtils.CreateFrom(in _entryCompletionSpritesBound);
			return true;
		}
		if (name == PropertyName._entryJumpCompletionPending)
		{
			value = VariantUtils.CreateFrom(in _entryJumpCompletionPending);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._pushCrew, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._seatCrew, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._entryCompletionSpritesBound, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._entryJumpCompletionPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesBreakAnimationCompletion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._entryCompletionSpritesBound, Variant.From(in _entryCompletionSpritesBound));
		info.AddProperty(PropertyName._entryJumpCompletionPending, Variant.From(in _entryJumpCompletionPending));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._entryCompletionSpritesBound, out var value))
		{
			_entryCompletionSpritesBound = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._entryJumpCompletionPending, out var value2))
		{
			_entryJumpCompletionPending = value2.As<bool>();
		}
	}
}
