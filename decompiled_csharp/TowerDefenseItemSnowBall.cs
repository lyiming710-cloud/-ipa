using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/SnowBall/Scene/TowerDefenseItemSnowBall.cs")]
public class TowerDefenseItemSnowBall : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName ExplodeHurt = "ExplodeHurt";

		public static readonly StringName SetSize = "SetSize";

		public static readonly StringName ApplySizeConfiguration = "ApplySizeConfiguration";

		public static readonly StringName ConfigureShadowScale = "ConfigureShadowScale";

		public static readonly StringName Pressed = "Pressed";

		public static readonly StringName Bowling = "Bowling";

		public static readonly StringName EdgeRebound = "EdgeRebound";

		public static readonly StringName HitNumCheck = "HitNumCheck";

		public new static readonly StringName InWater = "InWater";

		public static readonly StringName ProcessSnowballOverlaps = "ProcessSnowballOverlaps";

		public static readonly StringName HandleSnowballEntered = "HandleSnowballEntered";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ResolveSavedSize = "ResolveSavedSize";

		public static readonly StringName ApplyHitProgressInvariants = "ApplyHitProgressInvariants";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName hitNum = "hitNum";

		public static readonly StringName _hitNum = "_hitNum";

		public static readonly StringName hitFinish = "hitFinish";

		public static readonly StringName _size = "_size";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private CharacterMoveComponent moveComponent;

	private BowlingComponent bowlingComponent;

	private MousePressComponent mousePressComponent;

	private int _hitNum;

	public bool hitFinish;

	private string _size = "";

	private readonly HashSet<TowerDefenseCharacter> _overlappingSnowballs = new HashSet<TowerDefenseCharacter>();

	private readonly HashSet<TowerDefenseCharacter> _overlapScratch = new HashSet<TowerDefenseCharacter>();

	public int hitNum
	{
		get
		{
			return _hitNum;
		}
		set
		{
			_hitNum = value;
			HitNumCheck();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			moveComponent = componentManager.GetRuntime<CharacterMoveComponent>();
			bowlingComponent = componentManager.GetRuntime<BowlingComponent>();
			mousePressComponent = componentManager.GetRuntime<MousePressComponent>();
			bowlingComponent.OnBowling += Bowling;
			bowlingComponent.OnEdgeRebound += EdgeRebound;
			mousePressComponent.OnPressed += Pressed;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		BowlingComponent bowlingComponent = this.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased)
		{
			this.bowlingComponent.OnBowling -= Bowling;
			this.bowlingComponent.OnEdgeRebound -= EdgeRebound;
		}
		if (mousePressComponent != null)
		{
			mousePressComponent.OnPressed -= Pressed;
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
			ShadowComponent shadowComponent = base.shadowComponent;
			if (shadowComponent != null && !shadowComponent.IsReleased)
			{
				base.shadowComponent.SetSaveShadowPosition(new Vector2(globalPositionForPhysicsFrame.X, globalPositionForPhysicsFrame.Y + 30f));
			}
			base.BatchUpdate(delta);
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(globalPositionForPhysicsFrame);
			ProcessSnowballOverlaps();
		}
	}

	public override void IdleEntered()
	{
		sprite.SetAnimation("Rise", loop: false);
		sprite.AddAnimation("Idle", 0.0);
	}

	public override double ExplodeHurt(double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		if (sprite.clip == "Roll" && damageKind == TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.JALA)
		{
			Destroy();
		}
		return 0.0;
	}

	public void SetSize(string size)
	{
		ApplySizeConfiguration(size, applyInitialRuntimeState: true);
	}

	private bool ApplySizeConfiguration(string size, bool applyInitialRuntimeState)
	{
		switch (size)
		{
		case "Small":
		{
			TowerDefenseCharacterEventBase towerDefenseCharacterEventBase2 = bowlingComponent.hitEvent[0].Duplicate(deep: true) as TowerDefenseCharacterEventBase;
			towerDefenseCharacterEventBase2.Set("num", 100);
			bowlingComponent.hitEvent[0] = towerDefenseCharacterEventBase2;
			transformPoint.Scale = 0.5f * Vector2.One;
			ConfigureShadowScale(1f * Vector2.One);
			break;
		}
		case "Normal":
		{
			TowerDefenseCharacterEventBase towerDefenseCharacterEventBase3 = bowlingComponent.hitEvent[0].Duplicate(deep: true) as TowerDefenseCharacterEventBase;
			towerDefenseCharacterEventBase3.Set("num", 200);
			bowlingComponent.hitEvent[0] = towerDefenseCharacterEventBase3;
			transformPoint.Scale = 0.8f * Vector2.One;
			ConfigureShadowScale(1.6f * Vector2.One);
			break;
		}
		case "Large":
		{
			TowerDefenseCharacterEventBase towerDefenseCharacterEventBase4 = bowlingComponent.hitEvent[0].Duplicate(deep: true) as TowerDefenseCharacterEventBase;
			towerDefenseCharacterEventBase4.Set("num", 400);
			bowlingComponent.hitEvent[0] = towerDefenseCharacterEventBase4;
			transformPoint.Scale = 1.25f * Vector2.One;
			ConfigureShadowScale(2.5f * Vector2.One);
			break;
		}
		case "Max":
		{
			bowlingComponent.rollXVelocityMax = 150.0;
			bowlingComponent.rollXVelocityMin = 100.0;
			bowlingComponent.edgeReboundUse = false;
			bowlingComponent.hitLineUse = true;
			bowlingComponent.hitLineBackUse = false;
			if (applyInitialRuntimeState)
			{
				bowlingComponent.SetAlive(alive: true);
				mousePressComponent.SetAlive(alive: false);
				hitFinish = true;
			}
			TowerDefenseCharacterEventBase towerDefenseCharacterEventBase = bowlingComponent.hitEvent[0].Duplicate(deep: true) as TowerDefenseCharacterEventBase;
			towerDefenseCharacterEventBase.Set("num", 400);
			bowlingComponent.hitEvent[0] = towerDefenseCharacterEventBase;
			transformPoint.Scale = 1.5f * Vector2.One;
			ConfigureShadowScale(3f * Vector2.One);
			break;
		}
		default:
			return false;
		}
		_size = size;
		return true;
	}

	private void ConfigureShadowScale(Vector2 scale)
	{
		ShadowComponent shadowComponent = base.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			base.shadowComponent.saveShadowScale = scale;
			base.shadowComponent.saveTransformPointScale = transformPoint.Scale;
			base.shadowComponent.MarkDirty();
		}
	}

	public void Pressed(Vector2 pos)
	{
		bowlingComponent.SetAlive(alive: true);
		mousePressComponent.SetAlive(alive: false);
	}

	public void Bowling(TowerDefenseCharacter character)
	{
		hitNum++;
	}

	public void EdgeRebound()
	{
		hitNum++;
	}

	public void HitNumCheck()
	{
		if (!hitFinish)
		{
			CharacterMoveComponent characterMoveComponent = moveComponent;
			if (characterMoveComponent != null && !characterMoveComponent.IsReleased && !(moveComponent.velocity.X < 0f) && hitNum >= 5)
			{
				bowlingComponent.edgeReboundUse = false;
				bowlingComponent.hitLineUse = false;
				hitFinish = true;
			}
		}
	}

	public override void InWater()
	{
		base.InWater();
		if (!(Mathf.Abs(transformPoint.Scale.X) >= 0.75f))
		{
			CreateSplash();
			Destroy();
		}
	}

	private void ProcessSnowballOverlaps()
	{
		_overlapScratch.Clear();
		if (!TryGetActiveWorldHitRect(out var rect))
		{
			_overlappingSnowballs.Clear();
			return;
		}
		List<TowerDefenseCharacter> charactersIntersectingRectList = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectList(rect);
		for (int i = 0; i < charactersIntersectingRectList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = charactersIntersectingRectList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter != this && !(towerDefenseCharacter.config.name != "ItemSnowBall"))
			{
				_overlapScratch.Add(towerDefenseCharacter);
				if (!_overlappingSnowballs.Contains(towerDefenseCharacter))
				{
					HandleSnowballEntered(towerDefenseCharacter);
				}
			}
		}
		_overlappingSnowballs.RemoveWhere((TowerDefenseCharacter tdChar) => !GodotObject.IsInstanceValid(tdChar) || !_overlapScratch.Contains(tdChar));
		foreach (TowerDefenseCharacter item in _overlapScratch)
		{
			_overlappingSnowballs.Add(item);
		}
	}

	private void HandleSnowballEntered(TowerDefenseCharacter tdChar)
	{
		if (!(tdChar is TowerDefenseItemSnowBall { bowlingComponent: { Alive: not false } }))
		{
			tdChar.Set("hitNum", tdChar.Get("hitNum").AsInt32() + 1);
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		if (instance.hypnoses)
		{
			CharacterMoveComponent characterMoveComponent = moveComponent;
			if (characterMoveComponent != null && !characterMoveComponent.IsReleased)
			{
				moveComponent.moveScale = -1.0;
			}
			bowlingComponent.SetAlive(alive: true);
			mousePressComponent.SetAlive(alive: false);
		}
		else
		{
			CharacterMoveComponent characterMoveComponent2 = moveComponent;
			if (characterMoveComponent2 != null && !characterMoveComponent2.IsReleased)
			{
				moveComponent.moveScale = 1.0;
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "hitNum", _hitNum },
			{ "hitFinish", hitFinish },
			{ "size", _size }
		};
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		string text = ResolveSavedSize(data);
		if (!string.IsNullOrEmpty(text))
		{
			ApplySizeConfiguration(text, applyInitialRuntimeState: false);
		}
		_hitNum = Math.Max(0, data.GetValueOrDefault("hitNum", _hitNum).AsInt32());
		hitFinish = data.GetValueOrDefault("hitFinish", hitFinish).AsBool();
		ApplyHitProgressInvariants();
	}

	private string ResolveSavedSize(Dictionary data)
	{
		string text = data.GetValueOrDefault("size", _size).AsString();
		if (!string.IsNullOrEmpty(text) || !GodotObject.IsInstanceValid(transformPoint))
		{
			return text;
		}
		float a = Mathf.Abs(transformPoint.Scale.X);
		if (Mathf.IsEqualApprox(a, 0.5f))
		{
			return "Small";
		}
		if (Mathf.IsEqualApprox(a, 0.8f))
		{
			return "Normal";
		}
		if (Mathf.IsEqualApprox(a, 1.25f))
		{
			return "Large";
		}
		if (Mathf.IsEqualApprox(a, 1.5f))
		{
			return "Max";
		}
		return "";
	}

	private void ApplyHitProgressInvariants()
	{
		if (hitFinish && !(_size == "Max") && bowlingComponent != null)
		{
			bowlingComponent.edgeReboundUse = false;
			bowlingComponent.hitLineUse = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySizeConfiguration, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "applyInitialRuntimeState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureShadowScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Bowling, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.EdgeRebound, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitNumCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessSnowballOverlaps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleSnowballEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tdChar", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSavedSize, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyHitProgressInvariants, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ExplodeHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ExplodeHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.SetSize && args.Count == 1)
		{
			SetSize(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySizeConfiguration && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplySizeConfiguration(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ConfigureShadowScale && args.Count == 1)
		{
			ConfigureShadowScale(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 1)
		{
			Pressed(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Bowling && args.Count == 1)
		{
			Bowling(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EdgeRebound && args.Count == 0)
		{
			EdgeRebound();
			ret = default;
			return true;
		}
		if (method == MethodName.HitNumCheck && args.Count == 0)
		{
			HitNumCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessSnowballOverlaps && args.Count == 0)
		{
			ProcessSnowballOverlaps();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleSnowballEntered && args.Count == 1)
		{
			HandleSnowballEntered(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
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
		if (method == MethodName.ResolveSavedSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveSavedSize(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyHitProgressInvariants && args.Count == 0)
		{
			ApplyHitProgressInvariants();
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
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.ExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.SetSize)
		{
			return true;
		}
		if (method == MethodName.ApplySizeConfiguration)
		{
			return true;
		}
		if (method == MethodName.ConfigureShadowScale)
		{
			return true;
		}
		if (method == MethodName.Pressed)
		{
			return true;
		}
		if (method == MethodName.Bowling)
		{
			return true;
		}
		if (method == MethodName.EdgeRebound)
		{
			return true;
		}
		if (method == MethodName.HitNumCheck)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.ProcessSnowballOverlaps)
		{
			return true;
		}
		if (method == MethodName.HandleSnowballEntered)
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
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ResolveSavedSize)
		{
			return true;
		}
		if (method == MethodName.ApplyHitProgressInvariants)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.hitNum)
		{
			hitNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hitNum)
		{
			_hitNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hitFinish)
		{
			hitFinish = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._size)
		{
			_size = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.hitNum)
		{
			value = VariantUtils.CreateFrom<int>(hitNum);
			return true;
		}
		if (name == PropertyName._hitNum)
		{
			value = VariantUtils.CreateFrom(in _hitNum);
			return true;
		}
		if (name == PropertyName.hitFinish)
		{
			value = VariantUtils.CreateFrom(in hitFinish);
			return true;
		}
		if (name == PropertyName._size)
		{
			value = VariantUtils.CreateFrom(in _size);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._hitNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.hitNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitFinish, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._size, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hitNum, Variant.From<int>(hitNum));
		info.AddProperty(PropertyName._hitNum, Variant.From(in _hitNum));
		info.AddProperty(PropertyName.hitFinish, Variant.From(in hitFinish));
		info.AddProperty(PropertyName._size, Variant.From(in _size));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hitNum, out var value))
		{
			hitNum = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hitNum, out var value2))
		{
			_hitNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hitFinish, out var value3))
		{
			hitFinish = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._size, out var value4))
		{
			_size = value4.As<string>();
		}
	}
}
