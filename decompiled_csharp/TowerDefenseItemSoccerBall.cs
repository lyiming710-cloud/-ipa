using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/SoccerBall/Scene/TowerDefenseItemSoccerBall.cs")]
public class TowerDefenseItemSoccerBall : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName StartKickedRoll = "StartKickedRoll";

		public static readonly StringName EnsureRollClipConfigured = "EnsureRollClipConfigured";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName Hypnoses = "Hypnoses";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName _destroyAnimePlaying = "_destroyAnimePlaying";

		public static readonly StringName _destroyFreeInstance = "_destroyFreeInstance";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private const string IdleClip = "Idle";

	private const string DeathClip = "Death";

	private const string RollClip = "Roll";

	public CharacterMoveComponent moveComponent;

	public WaterEnvironmentComponent waterEnvironmentComponent;

	public BowlingComponent bowlingComponent;

	private bool _destroyAnimePlaying;

	private bool _destroyFreeInstance = true;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			if (GodotObject.IsInstanceValid(componentManager))
			{
				moveComponent = componentManager.GetRuntime<CharacterMoveComponent>();
				waterEnvironmentComponent = componentManager.GetRuntime<WaterEnvironmentComponent>();
				this.bowlingComponent = componentManager.GetRuntime<BowlingComponent>();
			}
			BowlingComponent bowlingComponent = this.bowlingComponent;
			if (bowlingComponent != null && !bowlingComponent.IsReleased)
			{
				EnsureRollClipConfigured();
				this.bowlingComponent.SetAlive(alive: false);
			}
			if (GodotObject.IsInstanceValid(sprite) && sprite.HasClip("Idle"))
			{
				sprite.SetAnimation("Idle");
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(TowerDefenseProcessModeDispatch.CurrentPhysicsFrame);
			ShadowComponent shadowComponent = base.shadowComponent;
			if (shadowComponent != null && !shadowComponent.IsReleased)
			{
				base.shadowComponent.saveShadowPosition = new Vector2(base.shadowComponent.saveShadowPosition.X, globalPositionForPhysicsFrame.Y + 30f);
			}
			if (TowerDefenseManager.Instance != null)
			{
				gridPos = TowerDefenseManager.Instance.GetMapGridPos(globalPositionForPhysicsFrame);
			}
		}
	}

	public override void IdleEntered()
	{
		BowlingComponent bowlingComponent = this.bowlingComponent;
		if (bowlingComponent == null || bowlingComponent.IsReleased || !this.bowlingComponent.isRoll)
		{
			base.IdleEntered();
			BowlingComponent bowlingComponent2 = this.bowlingComponent;
			if (bowlingComponent2 != null && !bowlingComponent2.IsReleased)
			{
				this.bowlingComponent.SetAlive(alive: false);
			}
		}
	}

	public void StartKickedRoll()
	{
		useIdleAnimeReset = false;
		EnsureRollClipConfigured();
		BowlingComponent bowlingComponent = this.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased)
		{
			this.bowlingComponent.StartRoll();
		}
	}

	private void EnsureRollClipConfigured()
	{
		BowlingComponent bowlingComponent = this.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased && string.IsNullOrEmpty(this.bowlingComponent.rollAnimeClips))
		{
			this.bowlingComponent.rollAnimeClips = "Roll";
		}
	}

	public override void InWater()
	{
		base.InWater();
		CreateSplash();
		base.Destroy();
	}

	public override void Destroy(bool freeInstance = true)
	{
		if (isDestroy || _destroyAnimePlaying)
		{
			return;
		}
		if (inWater || !GodotObject.IsInstanceValid(sprite) || !sprite.HasClip("Death"))
		{
			base.Destroy(freeInstance);
			return;
		}
		_destroyAnimePlaying = true;
		_destroyFreeInstance = freeInstance;
		HitBoxDestroy();
		CharacterMoveComponent characterMoveComponent = moveComponent;
		if (characterMoveComponent != null && !characterMoveComponent.IsReleased)
		{
			moveComponent.velocity = Vector2.Zero;
		}
		BowlingComponent bowlingComponent = this.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased)
		{
			this.bowlingComponent.SetAlive(alive: false);
			this.bowlingComponent.isRoll = false;
		}
		sprite.SetAnimation("Death", loop: false);
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (_destroyAnimePlaying && !(clip != "Death"))
		{
			_destroyAnimePlaying = false;
			base.Destroy(_destroyFreeInstance);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["destroyAnimePlaying"] = _destroyAnimePlaying,
			["destroyFreeInstance"] = _destroyFreeInstance
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_destroyAnimePlaying = data.GetValueOrDefault("destroyAnimePlaying", _destroyAnimePlaying).AsBool();
		_destroyFreeInstance = data.GetValueOrDefault("destroyFreeInstance", _destroyFreeInstance).AsBool();
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		CharacterMoveComponent characterMoveComponent = moveComponent;
		if (characterMoveComponent != null && !characterMoveComponent.IsReleased)
		{
			moveComponent.moveScale = (instance.hypnoses ? (-1.0) : 1.0);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartKickedRoll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureRollClipConfigured, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "freeInstance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.StartKickedRoll && args.Count == 0)
		{
			StartKickedRoll();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureRollClipConfigured && args.Count == 0)
		{
			EnsureRollClipConfigured();
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 1)
		{
			Destroy(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.StartKickedRoll)
		{
			return true;
		}
		if (method == MethodName.EnsureRollClipConfigured)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
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
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._destroyAnimePlaying)
		{
			_destroyAnimePlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._destroyFreeInstance)
		{
			_destroyFreeInstance = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._destroyAnimePlaying)
		{
			value = VariantUtils.CreateFrom(in _destroyAnimePlaying);
			return true;
		}
		if (name == PropertyName._destroyFreeInstance)
		{
			value = VariantUtils.CreateFrom(in _destroyFreeInstance);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroyAnimePlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroyFreeInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._destroyAnimePlaying, Variant.From(in _destroyAnimePlaying));
		info.AddProperty(PropertyName._destroyFreeInstance, Variant.From(in _destroyFreeInstance));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._destroyAnimePlaying, out var value))
		{
			_destroyAnimePlaying = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._destroyFreeInstance, out var value2))
		{
			_destroyFreeInstance = value2.As<bool>();
		}
	}
}
