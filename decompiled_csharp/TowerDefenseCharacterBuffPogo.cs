using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffPogo.cs")]
public class TowerDefenseCharacterBuffPogo : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName Enter = "Enter";

		public static readonly StringName ApplyMovementState = "ApplyMovementState";

		public new static readonly StringName EnterReadOnlyClient = "EnterReadOnlyClient";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Exit = "Exit";

		public new static readonly StringName ExitReadOnlyClient = "ExitReadOnlyClient";

		public new static readonly StringName Remove = "Remove";

		public new static readonly StringName Cancel = "Cancel";

		public static readonly StringName CleanupMovementState = "CleanupMovementState";

		public new static readonly StringName Refresh = "Refresh";

		public static readonly StringName OnLand = "OnLand";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public static readonly StringName time = "time";

		public static readonly StringName jumpSpeed = "jumpSpeed";

		public static readonly StringName pogoGravity = "pogoGravity";

		public static readonly StringName currentTime = "currentTime";

		public static readonly StringName _movementStateApplied = "_movementStateApplied";

		public static readonly StringName _savedGravity = "_savedGravity";

		public static readonly StringName _hasSavedGravity = "_hasSavedGravity";

		public static readonly StringName _savedHandleWaterHeight = "_savedHandleWaterHeight";

		public static readonly StringName _hasSavedHandleWaterHeight = "_hasSavedHandleWaterHeight";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double time = 8.0;

	[Export(PropertyHint.None, "")]
	public double jumpSpeed = -300.0;

	[Export(PropertyHint.None, "")]
	public double pogoGravity = 490.0;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	private bool _movementStateApplied;

	private Variant _savedGravity;

	private bool _hasSavedGravity;

	private bool _savedHandleWaterHeight;

	private bool _hasSavedHandleWaterHeight;

	public override void _Init()
	{
		key = "Pogo";
	}

	public override void Enter()
	{
		if ((character.instance.collisionFlags & 2) != 0)
		{
			character.buff.DeleteBuff("Pogo");
		}
		else if (character is TowerDefenseZombie && character.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			character.buff.DeleteBuff("Pogo");
		}
		else if (character is TowerDefenseItem)
		{
			character.buff.DeleteBuff("Pogo");
		}
		else if (character is TowerDefenseGravestone)
		{
			character.buff.DeleteBuff("Pogo");
		}
		else if (character is TowerDefenseCrater)
		{
			character.buff.DeleteBuff("Pogo");
		}
		else
		{
			ApplyMovementState();
		}
	}

	private void ApplyMovementState()
	{
		if (_movementStateApplied || !GodotObject.IsInstanceValid(character))
		{
			return;
		}
		_savedGravity = character.Get("gravity");
		_hasSavedGravity = _savedGravity.VariantType != Variant.Type.Nil;
		GroundHeightComponent groundHeightComponent = character.groundHeightComponent;
		if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
		{
			_savedHandleWaterHeight = character.groundHeightComponent.handleWaterHeight;
			_hasSavedHandleWaterHeight = true;
		}
		character.OnLand += OnLand;
		_movementStateApplied = true;
		character.Set("ySpeed", jumpSpeed);
		character.Set("gravity", pogoGravity);
		if (character.inWater)
		{
			character.groundHeight = 0.0;
			GroundHeightComponent groundHeightComponent2 = character.groundHeightComponent;
			if (groundHeightComponent2 != null && !groundHeightComponent2.IsReleased)
			{
				character.groundHeightComponent.handleWaterHeight = false;
			}
		}
	}

	public override void EnterReadOnlyClient()
	{
		ApplyMovementState();
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		if (!character.nearDie && !character.die)
		{
			return currentTime >= time;
		}
		return true;
	}

	public override void StepReadOnlyClient(double delta)
	{
		Step(delta);
	}

	public override void Exit()
	{
		CleanupMovementState();
		if (!character.instance.hypnoses)
		{
			character.instance.ArmorClear();
		}
		if (character is TowerDefenseZombie && character.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.CAR)
		{
			character.Hurt(1000000.0);
		}
	}

	public override void ExitReadOnlyClient()
	{
		CleanupMovementState();
	}

	public override void Remove()
	{
		CleanupMovementState();
	}

	public override void Cancel()
	{
		CleanupMovementState();
	}

	private void CleanupMovementState()
	{
		if (!_movementStateApplied)
		{
			return;
		}
		_movementStateApplied = false;
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		character.OnLand -= OnLand;
		if (_hasSavedGravity)
		{
			character.Set("gravity", _savedGravity);
		}
		_hasSavedGravity = false;
		if (_hasSavedHandleWaterHeight)
		{
			GroundHeightComponent groundHeightComponent = character.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				character.groundHeightComponent.handleWaterHeight = _savedHandleWaterHeight;
			}
		}
		_hasSavedHandleWaterHeight = false;
		if (!character.inWater)
		{
			return;
		}
		Variant variant = character.Get("waterHeight");
		if (variant.VariantType != Variant.Type.Nil)
		{
			character.groundHeight = 0.0 - (double)variant;
			return;
		}
		GroundHeightComponent groundHeightComponent2 = character.groundHeightComponent;
		if (groundHeightComponent2 != null && !groundHeightComponent2.IsReleased)
		{
			character.groundHeight = 0f - character.groundHeightComponent.waterHeight;
		}
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		if (config is TowerDefenseCharacterBuffPogo towerDefenseCharacterBuffPogo)
		{
			time = Mathf.Max(time, towerDefenseCharacterBuffPogo.time);
			jumpSpeed = towerDefenseCharacterBuffPogo.jumpSpeed;
			pogoGravity = towerDefenseCharacterBuffPogo.pogoGravity;
			currentTime = 0.0;
		}
	}

	public void OnLand()
	{
		character.Set("ySpeed", jumpSpeed);
		if (character.inWater)
		{
			character.groundHeight = 0.0;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMovementState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Cancel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupMovementState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnLand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ApplyMovementState && args.Count == 0)
		{
			ApplyMovementState();
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
		if (method == MethodName.Remove && args.Count == 0)
		{
			Remove();
			ret = default;
			return true;
		}
		if (method == MethodName.Cancel && args.Count == 0)
		{
			Cancel();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupMovementState && args.Count == 0)
		{
			CleanupMovementState();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLand && args.Count == 0)
		{
			OnLand();
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
		if (method == MethodName.ApplyMovementState)
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
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.ExitReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Remove)
		{
			return true;
		}
		if (method == MethodName.Cancel)
		{
			return true;
		}
		if (method == MethodName.CleanupMovementState)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.OnLand)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpSpeed)
		{
			jumpSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.pogoGravity)
		{
			pogoGravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._movementStateApplied)
		{
			_movementStateApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._savedGravity)
		{
			_savedGravity = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName._hasSavedGravity)
		{
			_hasSavedGravity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._savedHandleWaterHeight)
		{
			_savedHandleWaterHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasSavedHandleWaterHeight)
		{
			_hasSavedHandleWaterHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.jumpSpeed)
		{
			value = VariantUtils.CreateFrom(in jumpSpeed);
			return true;
		}
		if (name == PropertyName.pogoGravity)
		{
			value = VariantUtils.CreateFrom(in pogoGravity);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		if (name == PropertyName._movementStateApplied)
		{
			value = VariantUtils.CreateFrom(in _movementStateApplied);
			return true;
		}
		if (name == PropertyName._savedGravity)
		{
			value = VariantUtils.CreateFrom(in _savedGravity);
			return true;
		}
		if (name == PropertyName._hasSavedGravity)
		{
			value = VariantUtils.CreateFrom(in _hasSavedGravity);
			return true;
		}
		if (name == PropertyName._savedHandleWaterHeight)
		{
			value = VariantUtils.CreateFrom(in _savedHandleWaterHeight);
			return true;
		}
		if (name == PropertyName._hasSavedHandleWaterHeight)
		{
			value = VariantUtils.CreateFrom(in _hasSavedHandleWaterHeight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pogoGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._movementStateApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._savedGravity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasSavedGravity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._savedHandleWaterHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasSavedHandleWaterHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.jumpSpeed, Variant.From(in jumpSpeed));
		info.AddProperty(PropertyName.pogoGravity, Variant.From(in pogoGravity));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
		info.AddProperty(PropertyName._movementStateApplied, Variant.From(in _movementStateApplied));
		info.AddProperty(PropertyName._savedGravity, Variant.From(in _savedGravity));
		info.AddProperty(PropertyName._hasSavedGravity, Variant.From(in _hasSavedGravity));
		info.AddProperty(PropertyName._savedHandleWaterHeight, Variant.From(in _savedHandleWaterHeight));
		info.AddProperty(PropertyName._hasSavedHandleWaterHeight, Variant.From(in _hasSavedHandleWaterHeight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.time, out var value))
		{
			time = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpSpeed, out var value2))
		{
			jumpSpeed = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.pogoGravity, out var value3))
		{
			pogoGravity = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value4))
		{
			currentTime = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._movementStateApplied, out var value5))
		{
			_movementStateApplied = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._savedGravity, out var value6))
		{
			_savedGravity = value6.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName._hasSavedGravity, out var value7))
		{
			_hasSavedGravity = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._savedHandleWaterHeight, out var value8))
		{
			_savedHandleWaterHeight = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasSavedHandleWaterHeight, out var value9))
		{
			_hasSavedHandleWaterHeight = value9.As<bool>();
		}
	}
}
