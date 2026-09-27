using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffRadiance.cs")]
public class TowerDefenseCharacterBuffRadiance : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public static readonly StringName CaptureBaselineCampOnce = "CaptureBaselineCampOnce";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public new static readonly StringName Cancel = "Cancel";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName Refresh = "Refresh";

		public static readonly StringName UpdateRadianceState = "UpdateRadianceState";

		public static readonly StringName ShouldShowRadiance = "ShouldShowRadiance";

		public static readonly StringName IsCampFlipped = "IsCampFlipped";

		public static readonly StringName ShowVisuals = "ShowVisuals";

		public static readonly StringName HideVisuals = "HideVisuals";

		public static readonly StringName ApplyGameplayState = "ApplyGameplayState";

		public static readonly StringName RestoreGameplayState = "RestoreGameplayState";

		public static readonly StringName PlayDeathFlash = "PlayDeathFlash";

		public static readonly StringName ResolveFlashGridPos = "ResolveFlashGridPos";

		public static readonly StringName PlayFlashEffect = "PlayFlashEffect";

		public static readonly StringName ApplyFlashDizziness = "ApplyFlashDizziness";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public static readonly StringName permanent = "permanent";

		public static readonly StringName time = "time";

		public static readonly StringName flashOnDeath = "flashOnDeath";

		public static readonly StringName radianceRemoveOnCampFlip = "radianceRemoveOnCampFlip";

		public static readonly StringName deathFlashRequireSameCamp = "deathFlashRequireSameCamp";

		public static readonly StringName baselineCamp = "baselineCamp";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName fog = "fog";

		public static readonly StringName light = "light";

		public static readonly StringName _gameplayStateApplied = "_gameplayStateApplied";

		public static readonly StringName _flashPlayed = "_flashPlayed";

		public static readonly StringName _visualActive = "_visualActive";

		public static readonly StringName _baselineCaptured = "_baselineCaptured";

		public static readonly StringName _originalHadLightPhysique = "_originalHadLightPhysique";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	private static PackedScene _LIGHT_FOG;

	private static PackedScene _LIGHT_AREA;

	private const string FlashEffectSceneUid = "uid://cxghtqumcubh4";

	private const string FlashEffectClip = "Fire";

	private const int FlashRange = 1;

	private const double FlashDizzinessTime = 5.0;

	private static PackedScene _flashEffectScene;

	[Export(PropertyHint.None, "")]
	public bool permanent = true;

	[Export(PropertyHint.None, "")]
	public double time = 30.0;

	[Export(PropertyHint.None, "")]
	public bool flashOnDeath = true;

	[Export(PropertyHint.None, "")]
	public bool radianceRemoveOnCampFlip = true;

	[Export(PropertyHint.None, "")]
	public bool deathFlashRequireSameCamp = true;

	[Export(PropertyHint.None, "")]
	public int baselineCamp = -1;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	public Node fog;

	public Node light;

	private bool _gameplayStateApplied;

	private bool _flashPlayed;

	private bool _visualActive;

	private bool _baselineCaptured;

	private bool _originalHadLightPhysique;

	private static PackedScene LIGHT_FOG => _LIGHT_FOG ?? (_LIGHT_FOG = GD.Load<PackedScene>("uid://dsc2tkcmoc8l1"));

	private static PackedScene LIGHT_AREA => _LIGHT_AREA ?? (_LIGHT_AREA = GD.Load<PackedScene>("uid://byee3s263f1rj"));

	private static PackedScene FlashEffectScene => _flashEffectScene ?? (_flashEffectScene = GD.Load<PackedScene>("uid://cxghtqumcubh4"));

	public override void _Init()
	{
		key = "Radiance";
	}

	public override void Enter()
	{
		ShowVisuals();
		ApplyGameplayState();
		_visualActive = true;
	}

	public override void EnterReadOnlyClient()
	{
		ShowVisuals();
		_visualActive = true;
	}

	public override bool Step(double delta)
	{
		CaptureBaselineCampOnce();
		UpdateRadianceState(applyGameplayState: true);
		if (permanent)
		{
			return false;
		}
		currentTime += delta;
		return currentTime >= time;
	}

	public override void StepReadOnlyClient(double delta)
	{
		CaptureBaselineCampOnce();
		UpdateRadianceState(applyGameplayState: false);
		if (!permanent)
		{
			currentTime += delta;
		}
	}

	private void CaptureBaselineCampOnce()
	{
		if (!_baselineCaptured && GodotObject.IsInstanceValid(character))
		{
			baselineCamp = (int)character.camp;
			_baselineCaptured = true;
		}
	}

	public override void Exit()
	{
		RestoreGameplayState();
		HideVisuals();
		_visualActive = false;
	}

	public override void ExitReadOnlyClient()
	{
		HideVisuals();
		_visualActive = false;
	}

	public override void Cancel()
	{
		RestoreGameplayState();
		HideVisuals();
		_visualActive = false;
	}

	public override void Destroy()
	{
		PlayDeathFlash();
		RestoreGameplayState();
		HideVisuals();
		_visualActive = false;
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		if (config is TowerDefenseCharacterBuffRadiance towerDefenseCharacterBuffRadiance)
		{
			permanent = permanent || towerDefenseCharacterBuffRadiance.permanent;
			time = Mathf.Max(time, towerDefenseCharacterBuffRadiance.time + GD.RandRange(-0.2, 0.2));
			flashOnDeath = flashOnDeath || towerDefenseCharacterBuffRadiance.flashOnDeath;
		}
		currentTime = 0.0;
	}

	private void UpdateRadianceState(bool applyGameplayState)
	{
		bool flag = ShouldShowRadiance();
		if (flag == _visualActive)
		{
			return;
		}
		if (flag)
		{
			ShowVisuals();
			if (applyGameplayState)
			{
				ApplyGameplayState();
			}
		}
		else
		{
			if (applyGameplayState)
			{
				RestoreGameplayState();
			}
			HideVisuals();
		}
		_visualActive = flag;
	}

	private bool ShouldShowRadiance()
	{
		if (!GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(character.instance))
		{
			return false;
		}
		if (radianceRemoveOnCampFlip && IsCampFlipped())
		{
			return false;
		}
		if (character.nearDie || character.die || character.instance.die)
		{
			return false;
		}
		return true;
	}

	private bool IsCampFlipped()
	{
		if (GodotObject.IsInstanceValid(character))
		{
			return character.camp != (TowerDefenseEnum.CHARACTER_CAMP)baselineCamp;
		}
		return false;
	}

	private void ShowVisuals()
	{
		if (!GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(character.spriteGroup))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(fog))
		{
			PackedScene lIGHT_FOG = LIGHT_FOG;
			if (GodotObject.IsInstanceValid(lIGHT_FOG))
			{
				fog = lIGHT_FOG.Instantiate(PackedScene.GenEditState.Disabled);
				character.spriteGroup.AddChild(fog, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
		if (!GodotObject.IsInstanceValid(light))
		{
			PackedScene lIGHT_AREA = LIGHT_AREA;
			if (GodotObject.IsInstanceValid(lIGHT_AREA))
			{
				light = lIGHT_AREA.Instantiate(PackedScene.GenEditState.Disabled);
				character.spriteGroup.AddChild(light, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
	}

	private void HideVisuals()
	{
		Node node = fog;
		fog = null;
		if (GodotObject.IsInstanceValid(node))
		{
			node.QueueFree();
		}
		Node node2 = light;
		light = null;
		if (GodotObject.IsInstanceValid(node2))
		{
			node2.QueueFree();
		}
	}

	private void ApplyGameplayState()
	{
		if (!_gameplayStateApplied && GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance))
		{
			int num = 64;
			_originalHadLightPhysique = (character.instance.physiqueTypeFlags & num) != 0;
			_gameplayStateApplied = true;
			character.instance.physiqueTypeFlags |= num;
		}
	}

	private void RestoreGameplayState()
	{
		if (!_gameplayStateApplied)
		{
			return;
		}
		_gameplayStateApplied = false;
		if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(character.instance))
		{
			int num = 64;
			if (_originalHadLightPhysique)
			{
				character.instance.physiqueTypeFlags |= num;
			}
			else
			{
				character.instance.physiqueTypeFlags &= ~num;
			}
		}
	}

	private void PlayDeathFlash()
	{
		if (flashOnDeath && !_flashPlayed)
		{
			_flashPlayed = true;
			if (GodotObject.IsInstanceValid(character) && (!deathFlashRequireSameCamp || !IsCampFlipped()))
			{
				Vector2I vector2I = ResolveFlashGridPos();
				PlayFlashEffect(vector2I);
				ApplyFlashDizziness(vector2I);
			}
		}
	}

	private Vector2I ResolveFlashGridPos()
	{
		Vector2I gridPos = character.gridPos;
		if (gridPos.X >= 0 && gridPos.Y >= 0)
		{
			return gridPos;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return new Vector2I(-1, -1);
		}
		return instance.GetMapGridPos(character.GetLogicalGlobalPosition());
	}

	private void PlayFlashEffect(Vector2I gridPos)
	{
		PackedScene flashEffectScene = FlashEffectScene;
		if (!GodotObject.IsInstanceValid(flashEffectScene))
		{
			return;
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(flashEffectScene, gridPos, "Fire");
		if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
		{
			Node2D characterNode = TowerDefenseManager.GetCharacterNode();
			if (!GodotObject.IsInstanceValid(characterNode))
			{
				towerDefenseEffectSpriteOnce.QueueFree();
				return;
			}
			characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, Node.InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.GlobalPosition = character.GetLogicalGlobalPosition();
		}
	}

	private void ApplyFlashDizziness(Vector2I center)
	{
		if (center.X < 0 || center.Y < 0 || !GodotObject.IsInstanceValid(character))
		{
			return;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			return;
		}
		Vector2 mapCellPosCenter = instance.GetMapCellPosCenter(center);
		Vector2 size = instance.GetMapGridSize() * 3f;
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		instance.characterRegistry.FillCharactersIntersectingRectListExcludingCamp(AabbShapeUtil.RectFromCenter(mapCellPosCenter, size), character.camp, list);
		for (int i = 0; i < list.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = list[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter != character && towerDefenseCharacter.camp != character.camp && !towerDefenseCharacter.die && !towerDefenseCharacter.isDestroy)
			{
				towerDefenseCharacter.BuffAdd(new TowerDefenseCharacterBuffDizziness
				{
					time = 5.0
				});
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureBaselineCampOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Cancel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRadianceState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "applyGameplayState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldShowRadiance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCampFlipped, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyGameplayState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreGameplayState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayDeathFlash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveFlashGridPos, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayFlashEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFlashDizziness, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Init && args.Count == 0)
		{
			_Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Enter && args.Count == 0)
		{
			Enter();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient && args.Count == 0)
		{
			EnterReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.Step && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Step(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.StepReadOnlyClient && args.Count == 1)
		{
			StepReadOnlyClient(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureBaselineCampOnce && args.Count == 0)
		{
			CaptureBaselineCampOnce();
			ret = default;
			return true;
		}
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient && args.Count == 0)
		{
			ExitReadOnlyClient();
			ret = default;
			return true;
		}
		if (method == MethodName.Cancel && args.Count == 0)
		{
			Cancel();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRadianceState && args.Count == 1)
		{
			UpdateRadianceState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldShowRadiance && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldShowRadiance());
			return true;
		}
		if (method == MethodName.IsCampFlipped && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCampFlipped());
			return true;
		}
		if (method == MethodName.ShowVisuals && args.Count == 0)
		{
			ShowVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.HideVisuals && args.Count == 0)
		{
			HideVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyGameplayState && args.Count == 0)
		{
			ApplyGameplayState();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreGameplayState && args.Count == 0)
		{
			RestoreGameplayState();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayDeathFlash && args.Count == 0)
		{
			PlayDeathFlash();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveFlashGridPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveFlashGridPos());
			return true;
		}
		if (method == MethodName.PlayFlashEffect && args.Count == 1)
		{
			PlayFlashEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFlashDizziness && args.Count == 1)
		{
			ApplyFlashDizziness(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.EnterReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		if (method == MethodName.StepReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.CaptureBaselineCampOnce)
		{
			return true;
		}
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Cancel)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.UpdateRadianceState)
		{
			return true;
		}
		if (method == MethodName.ShouldShowRadiance)
		{
			return true;
		}
		if (method == MethodName.IsCampFlipped)
		{
			return true;
		}
		if (method == MethodName.ShowVisuals)
		{
			return true;
		}
		if (method == MethodName.HideVisuals)
		{
			return true;
		}
		if (method == MethodName.ApplyGameplayState)
		{
			return true;
		}
		if (method == MethodName.RestoreGameplayState)
		{
			return true;
		}
		if (method == MethodName.PlayDeathFlash)
		{
			return true;
		}
		if (method == MethodName.ResolveFlashGridPos)
		{
			return true;
		}
		if (method == MethodName.PlayFlashEffect)
		{
			return true;
		}
		if (method == MethodName.ApplyFlashDizziness)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.permanent)
		{
			permanent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.flashOnDeath)
		{
			flashOnDeath = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.radianceRemoveOnCampFlip)
		{
			radianceRemoveOnCampFlip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.deathFlashRequireSameCamp)
		{
			deathFlashRequireSameCamp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.baselineCamp)
		{
			baselineCamp = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fog)
		{
			fog = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.light)
		{
			light = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._gameplayStateApplied)
		{
			_gameplayStateApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._flashPlayed)
		{
			_flashPlayed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visualActive)
		{
			_visualActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._baselineCaptured)
		{
			_baselineCaptured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalHadLightPhysique)
		{
			_originalHadLightPhysique = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.permanent)
		{
			value = VariantUtils.CreateFrom(in permanent);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.flashOnDeath)
		{
			value = VariantUtils.CreateFrom(in flashOnDeath);
			return true;
		}
		if (name == PropertyName.radianceRemoveOnCampFlip)
		{
			value = VariantUtils.CreateFrom(in radianceRemoveOnCampFlip);
			return true;
		}
		if (name == PropertyName.deathFlashRequireSameCamp)
		{
			value = VariantUtils.CreateFrom(in deathFlashRequireSameCamp);
			return true;
		}
		if (name == PropertyName.baselineCamp)
		{
			value = VariantUtils.CreateFrom(in baselineCamp);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName.fog)
		{
			value = VariantUtils.CreateFrom(in fog);
			return true;
		}
		if (name == PropertyName.light)
		{
			value = VariantUtils.CreateFrom(in light);
			return true;
		}
		if (name == PropertyName._gameplayStateApplied)
		{
			value = VariantUtils.CreateFrom(in _gameplayStateApplied);
			return true;
		}
		if (name == PropertyName._flashPlayed)
		{
			value = VariantUtils.CreateFrom(in _flashPlayed);
			return true;
		}
		if (name == PropertyName._visualActive)
		{
			value = VariantUtils.CreateFrom(in _visualActive);
			return true;
		}
		if (name == PropertyName._baselineCaptured)
		{
			value = VariantUtils.CreateFrom(in _baselineCaptured);
			return true;
		}
		if (name == PropertyName._originalHadLightPhysique)
		{
			value = VariantUtils.CreateFrom(in _originalHadLightPhysique);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.permanent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.flashOnDeath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.radianceRemoveOnCampFlip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.deathFlashRequireSameCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.baselineCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.fog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gameplayStateApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._flashPlayed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._visualActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._baselineCaptured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalHadLightPhysique, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.permanent, Variant.From(in permanent));
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.flashOnDeath, Variant.From(in flashOnDeath));
		info.AddProperty(PropertyName.radianceRemoveOnCampFlip, Variant.From(in radianceRemoveOnCampFlip));
		info.AddProperty(PropertyName.deathFlashRequireSameCamp, Variant.From(in deathFlashRequireSameCamp));
		info.AddProperty(PropertyName.baselineCamp, Variant.From(in baselineCamp));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName.fog, Variant.From(in fog));
		info.AddProperty(PropertyName.light, Variant.From(in light));
		info.AddProperty(PropertyName._gameplayStateApplied, Variant.From(in _gameplayStateApplied));
		info.AddProperty(PropertyName._flashPlayed, Variant.From(in _flashPlayed));
		info.AddProperty(PropertyName._visualActive, Variant.From(in _visualActive));
		info.AddProperty(PropertyName._baselineCaptured, Variant.From(in _baselineCaptured));
		info.AddProperty(PropertyName._originalHadLightPhysique, Variant.From(in _originalHadLightPhysique));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.permanent, out var value))
		{
			permanent = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value2))
		{
			time = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.flashOnDeath, out var value3))
		{
			flashOnDeath = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.radianceRemoveOnCampFlip, out var value4))
		{
			radianceRemoveOnCampFlip = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.deathFlashRequireSameCamp, out var value5))
		{
			deathFlashRequireSameCamp = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.baselineCamp, out var value6))
		{
			baselineCamp = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value7))
		{
			currentTime = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fog, out var value8))
		{
			fog = value8.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.light, out var value9))
		{
			light = value9.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._gameplayStateApplied, out var value10))
		{
			_gameplayStateApplied = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._flashPlayed, out var value11))
		{
			_flashPlayed = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visualActive, out var value12))
		{
			_visualActive = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._baselineCaptured, out var value13))
		{
			_baselineCaptured = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalHadLightPhysique, out var value14))
		{
			_originalHadLightPhysique = value14.As<bool>();
		}
	}
}
