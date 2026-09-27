using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Scene/TowerDefensePlantPlanternTanglekelp.cs")]
public class TowerDefensePlantPlanternTanglekelp : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName DragTargetIntoWaterOnDestroy = "DragTargetIntoWaterOnDestroy";

		public static readonly StringName Hitrack = "Hitrack";

		public static readonly StringName IsVehicleDizzinessImmune = "IsVehicleDizzinessImmune";

		public static readonly StringName SetLogicalGlobalPositionX = "SetLogicalGlobalPositionX";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName eventList = "eventList";

		public static readonly StringName attackInterval = "attackInterval";

		public static readonly StringName over = "over";

		public static readonly StringName attackTimer = "attackTimer";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public TanglekelpComponent tanglekelpComponent;

	public AttackComponent attackComponent;

	public AttackComponent attackComponent2;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public double attackInterval = 0.5;

	public bool over;

	public double attackTimer;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			tanglekelpComponent = componentManager.GetRuntime<TanglekelpComponent>();
			attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			attackComponent2.SetCheckAreaRectangleSize(0, mapGridSize * 2.75f);
			attackComponent.SetCheckAreaRectangleWidth(0, mapGridSize.X * 2.75f);
			if (currentCustom.Contains("Custom0"))
			{
				tanglekelpComponent.grabFliterOpen = new string[2] { "skin7", "skin8" };
				tanglekelpComponent.grabFliterClose = new string[2] { "Layer 29", "Layer 32" };
			}
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			tanglekelpComponent.grabFliterOpen = new string[2] { "skin7", "skin8" };
			tanglekelpComponent.grabFliterClose = new string[2] { "Layer 29", "Layer 32" };
		}
		else
		{
			tanglekelpComponent.grabFliterOpen = System.Array.Empty<string>();
			tanglekelpComponent.grabFliterClose = System.Array.Empty<string>();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (attackTimer >= attackInterval)
		{
			if (attackComponent2.CanAttack())
			{
				TowerDefenseExplode.CreateExplode(GetLogicalGlobalPosition(), new Vector2(1.3f, 1.3f), eventList, null, camp, instance.collisionFlags);
				attackTimer = 0.0;
			}
		}
		else
		{
			attackTimer += delta;
		}
	}

	public override async void DestroySet()
	{
		if (over)
		{
			return;
		}
		over = true;
		instance.collisionFlags = 33;
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>(attackComponent.GetTargetList());
		if (list.Count <= 0)
		{
			return;
		}
		if (inWater)
		{
			foreach (TowerDefenseCharacter item in list)
			{
				if (GodotObject.IsInstanceValid(item) && !item.IsHardControlImmune && CanCollision(item.instance.maskFlags) && item.config.canDragIntoWater)
				{
					DragTargetIntoWaterOnDestroy(item);
				}
			}
		}
		else
		{
			foreach (TowerDefenseCharacter item2 in list)
			{
				if (GodotObject.IsInstanceValid(item2) && CanCollision(item2.instance.maskFlags))
				{
					Hitrack(item2);
				}
			}
		}
		await ToSignal(GetTree().CreateTimer(0.6, processAlways: false), SceneTreeTimer.SignalName.Timeout);
	}

	private async void DragTargetIntoWaterOnDestroy(TowerDefenseCharacter target)
	{
		bool flag = target is TowerDefenseZombie && GodotObject.IsInstanceValid(target.instance) && target.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		if ((!GodotObject.IsInstanceValid(target) || target.IsHardControlImmune) | flag)
		{
			return;
		}
		TanglekelpComponent tanglekelpComponent = this.tanglekelpComponent;
		if (tanglekelpComponent == null || tanglekelpComponent.Lifecycle != ComponentRuntimeLifecycle.Active || !this.tanglekelpComponent.Alive || !GodotObject.IsInstanceValid(this.tanglekelpComponent.grabSpriteScene))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		Node2D runner = new Node2D();
		node2D.AddChild(runner, forceReadableName: false, InternalMode.Disabled);
		Vector2 globalPosition = (runner.GlobalPosition = target.GetLogicalGlobalPosition());
		AdobeAnimateSprite adobeAnimateSprite = this.tanglekelpComponent.grabSpriteScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		Godot.Collections.Array array = new Godot.Collections.Array();
		string[] grabFliterOpen = this.tanglekelpComponent.grabFliterOpen;
		foreach (string text in grabFliterOpen)
		{
			array.Add(text);
		}
		Godot.Collections.Array array2 = new Godot.Collections.Array();
		grabFliterOpen = this.tanglekelpComponent.grabFliterClose;
		foreach (string text2 in grabFliterOpen)
		{
			array2.Add(text2);
		}
		try
		{
			adobeAnimateSprite.SetFliters(array, open: true);
			adobeAnimateSprite.SetFliters(array2, open: false);
			runner.AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
			adobeAnimateSprite.Visible = true;
			adobeAnimateSprite.SetAnimation(this.tanglekelpComponent.grabAnimeClips, loop: false);
			adobeAnimateSprite.GlobalPosition = globalPosition;
			AudioManager.Instance.AudioPlay("Floop");
			AudioManager.Instance.AudioPlay("PlantWater");
			target.CreateSplash();
			if (GodotObject.IsInstanceValid(target.sprite))
			{
				target.sprite.pause = true;
			}
			target.SetHitBoxMonitorSuppressed(HitBoxSuppressionReason.PlanternTanglekelp, suppressed: true);
			await runner.ToSignal(runner.GetTree().CreateTimer(0.5, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (!GodotObject.IsInstanceValid(target))
			{
				return;
			}
			AudioManager.Instance.AudioPlay("ZombieEnteringWater");
			if (target.config.dragHurt != -1.0)
			{
				double dragHurt = target.config.dragHurt;
				target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, dragHurt);
				if (GodotObject.IsInstanceValid(target.sprite))
				{
					target.sprite.pause = false;
				}
				target.SetHitBoxMonitorSuppressed(HitBoxSuppressionReason.PlanternTanglekelp, suppressed: false);
			}
			else
			{
				target.die = true;
				target.Destroy();
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(runner))
			{
				runner.QueueFree();
			}
		}
	}

	public virtual void Hitrack(TowerDefenseCharacter character)
	{
		bool flag = character is TowerDefenseZombie && GodotObject.IsInstanceValid(character.instance) && character.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		if ((!GodotObject.IsInstanceValid(character) || character.IsHardControlImmune) | flag)
		{
			return;
		}
		if (character is TowerDefenseZombie)
		{
			character.ySpeed = -200.0;
			Tween tween = character.CreateTween();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Cubic);
			tween.TweenMethod(to: instance.hypnoses ? ((float)(TowerDefenseManager.Instance.GetMapGroundLeft() + 60.0)) : ((float)(character.groundRight - 60.0)), method: Callable.From((float value) =>
			{
				SetLogicalGlobalPositionX(character, value);
			}), from: character.GetLogicalGlobalPosition().X, duration: 2.0);
		}
		if (!IsVehicleDizzinessImmune(character))
		{
			TowerDefenseCharacterBuffDizziness towerDefenseCharacterBuffDizziness = new TowerDefenseCharacterBuffDizziness();
			towerDefenseCharacterBuffDizziness.time = 3.0;
			character.BuffAdd(towerDefenseCharacterBuffDizziness);
		}
	}

	private static bool IsVehicleDizzinessImmune(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character) && character.config is TowerDefenseZombieConfig towerDefenseZombieConfig)
		{
			return towerDefenseZombieConfig.physique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR;
		}
		return false;
	}

	private static void SetLogicalGlobalPositionX(TowerDefenseCharacter character, float value)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.SetLogicalGlobalPosition(new Vector2(value, character.GetLogicalGlobalPosition().Y));
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "attackInterval", attackInterval },
			{ "over", over },
			{ "attackTimer", attackTimer }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		attackInterval = data.GetValueOrDefault("attackInterval", 0.5).AsDouble();
		over = data.GetValueOrDefault("over", false).AsBool();
		attackTimer = data.GetValueOrDefault("attackTimer", 0.0).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DragTargetIntoWaterOnDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Hitrack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsVehicleDizzinessImmune, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetLogicalGlobalPositionX, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.DragTargetIntoWaterOnDestroy && args.Count == 1)
		{
			DragTargetIntoWaterOnDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hitrack && args.Count == 1)
		{
			Hitrack(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsVehicleDizzinessImmune && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVehicleDizzinessImmune(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.SetLogicalGlobalPositionX && args.Count == 2)
		{
			SetLogicalGlobalPositionX(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsVehicleDizzinessImmune && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVehicleDizzinessImmune(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.SetLogicalGlobalPositionX && args.Count == 2)
		{
			SetLogicalGlobalPositionX(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
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
		if (method == MethodName.OnCustomSwitched)
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
		if (method == MethodName.DragTargetIntoWaterOnDestroy)
		{
			return true;
		}
		if (method == MethodName.Hitrack)
		{
			return true;
		}
		if (method == MethodName.IsVehicleDizzinessImmune)
		{
			return true;
		}
		if (method == MethodName.SetLogicalGlobalPositionX)
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
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.attackInterval)
		{
			attackInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			attackTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.attackInterval)
		{
			value = VariantUtils.CreateFrom(in attackInterval);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			value = VariantUtils.CreateFrom(in attackTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.attackInterval, Variant.From(in attackInterval));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.attackTimer, Variant.From(in attackTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventList, out var value))
		{
			eventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.attackInterval, out var value2))
		{
			attackInterval = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.attackTimer, out var value4))
		{
			attackTimer = value4.As<double>();
		}
	}
}
