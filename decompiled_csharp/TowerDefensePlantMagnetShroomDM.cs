using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/MagnetShroomDM/Scene/TowerDefensePlantMagnetShroomDM.cs")]
public class TowerDefensePlantMagnetShroomDM : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnIronBreakDown = "OnIronBreakDown";

		public static readonly StringName EnterArmedState = "EnterArmedState";

		public static readonly StringName ExitArmedState = "ExitArmedState";

		public static readonly StringName OnCannonRest = "OnCannonRest";

		public static readonly StringName RestoreIdleAnimation = "RestoreIdleAnimation";

		public static readonly StringName UpdateEnergyBall = "UpdateEnergyBall";

		public static readonly StringName EnsureEnergyVisual = "EnsureEnergyVisual";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ReleaseStoredProjectile = "ReleaseStoredProjectile";

		public static readonly StringName ReleaseAllStoredProjectiles = "ReleaseAllStoredProjectiles";

		public static readonly StringName ResetCharge = "ResetCharge";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName DoDeathExplosion = "DoDeathExplosion";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName ironCount = "ironCount";

		public static readonly StringName _isArmed = "_isArmed";

		public static readonly StringName _energyVisual = "_energyVisual";

		public static readonly StringName _wrapTimer = "_wrapTimer";

		public static readonly StringName ironPerFullCharge = "ironPerFullCharge";

		public static readonly StringName deathExplosionDamage = "deathExplosionDamage";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string EnergyVisualScenePath = "res://Asset/Config/Projectile/DoomBall/Sprite/DoomBall/DoomBall.tscn";

	private MagnetComponent _magnetComponent;

	private CannonComponent _cannonComponent;

	private bool _isArmed;

	private Node2D _energyVisual;

	private double _wrapTimer;

	[Export(PropertyHint.None, "")]
	public int ironPerFullCharge = 4;

	[Export(PropertyHint.None, "")]
	public float deathExplosionDamage = 450f;

	public int ironCount { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_magnetComponent = componentManager.GetRuntime<MagnetComponent>();
			_cannonComponent = componentManager.GetRuntime<CannonComponent>();
			if (_magnetComponent == null)
			{
				GD.PushError("MagnetShroomDM is missing its Magnet resource runtime.");
			}
			if (_cannonComponent == null)
			{
				GD.PushError("MagnetShroomDM is missing its Cannon resource runtime.");
			}
			MagnetComponent magnetComponent = _magnetComponent;
			if (magnetComponent != null && !magnetComponent.IsReleased)
			{
				_magnetComponent.OnBreakDown += OnIronBreakDown;
			}
			CannonComponent cannonComponent = _cannonComponent;
			if (cannonComponent != null && !cannonComponent.IsReleased)
			{
				_cannonComponent.OnFire += ReleaseStoredProjectile;
				_cannonComponent.OnRest += OnCannonRest;
				_cannonComponent.SetAlive(alive: false);
				_cannonComponent.canFire = false;
			}
		}
	}

	public override void _ExitTree()
	{
		MagnetComponent magnetComponent = _magnetComponent;
		if (magnetComponent != null && !magnetComponent.IsReleased)
		{
			_magnetComponent.OnBreakDown -= OnIronBreakDown;
		}
		CannonComponent cannonComponent = _cannonComponent;
		if (cannonComponent != null && !cannonComponent.IsReleased)
		{
			_cannonComponent.OnFire -= ReleaseStoredProjectile;
			_cannonComponent.OnRest -= OnCannonRest;
		}
		base._ExitTree();
	}

	private void OnIronBreakDown(TowerDefenseArmorInstance _armor)
	{
		if (!_isArmed)
		{
			ironCount++;
			UpdateEnergyBall();
			if (ironCount >= ironPerFullCharge)
			{
				EnterArmedState();
			}
		}
	}

	private void EnterArmedState()
	{
		if (!_isArmed)
		{
			_isArmed = true;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAnimation("ArmedIdle");
			}
			MagnetComponent magnetComponent = _magnetComponent;
			if (magnetComponent != null && !magnetComponent.IsReleased)
			{
				_magnetComponent.SetAlive(false);
			}
			CannonComponent cannonComponent = _cannonComponent;
			if (cannonComponent != null && !cannonComponent.IsReleased)
			{
				_cannonComponent.SetAlive(alive: true);
				_cannonComponent.canFire = true;
			}
		}
	}

	private void ExitArmedState()
	{
		if (_isArmed)
		{
			_isArmed = false;
			if (GodotObject.IsInstanceValid(sprite) && idleAnimeClip != "")
			{
				sprite.SetAnimation(idleAnimeClip);
			}
			ResetCharge();
		}
	}

	private void OnCannonRest()
	{
		ExitArmedState();
		Callable.From(RestoreIdleAnimation).CallDeferred();
	}

	private void RestoreIdleAnimation()
	{
		if (GodotObject.IsInstanceValid(sprite) && idleAnimeClip != "")
		{
			sprite.SetAnimation(idleAnimeClip);
		}
	}

	private void UpdateEnergyBall()
	{
		EnsureEnergyVisual();
		float num = Mathf.Min(1f, (float)ironCount / (float)ironPerFullCharge);
		float num2 = Mathf.Max(0.1f, 0.25f + 0.75f * num);
		if (GodotObject.IsInstanceValid(_energyVisual))
		{
			_energyVisual.Scale = Vector2.One * num2;
		}
	}

	private void EnsureEnergyVisual()
	{
		if (!GodotObject.IsInstanceValid(_energyVisual) && ResourceLoader.Exists("res://Asset/Config/Projectile/DoomBall/Sprite/DoomBall/DoomBall.tscn"))
		{
			Node node = GD.Load<PackedScene>("res://Asset/Config/Projectile/DoomBall/Sprite/DoomBall/DoomBall.tscn").Instantiate(PackedScene.GenEditState.Disabled);
			if (!(node is Node2D node2D))
			{
				node.QueueFree();
				return;
			}
			AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			node2D.ZIndex = 10;
			node2D.GlobalPosition = GetLogicalGlobalPosition() + new Vector2(40f, -10f);
			_energyVisual = node2D;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (GodotObject.IsInstanceValid(_energyVisual))
		{
			_wrapTimer += delta;
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			float num = 30f + 20f * _energyVisual.Scale.X;
			_energyVisual.GlobalPosition = logicalGlobalPosition + new Vector2(Mathf.Cos((float)(_wrapTimer * 2.0)) * num, Mathf.Sin((float)_wrapTimer + 1.2f) * 8f - 10f);
			_energyVisual.Rotation += (float)delta;
		}
	}

	private void ReleaseStoredProjectile()
	{
		if (GodotObject.IsInstanceValid(_energyVisual))
		{
			_energyVisual.QueueFree();
		}
		_energyVisual = null;
	}

	private void ReleaseAllStoredProjectiles()
	{
		ReleaseStoredProjectile();
	}

	private void ResetCharge()
	{
		_isArmed = false;
		ironCount = 0;
		CannonComponent cannonComponent = _cannonComponent;
		if (cannonComponent != null && !cannonComponent.IsReleased)
		{
			_cannonComponent.SetAlive(alive: false);
			_cannonComponent.canFire = false;
		}
		MagnetComponent magnetComponent = _magnetComponent;
		if (magnetComponent != null && !magnetComponent.IsReleased)
		{
			_magnetComponent.SetAlive(true);
			_magnetComponent.ForceResetToIdle();
		}
	}

	public override void DestroySet()
	{
		ReleaseAllStoredProjectiles();
		DoDeathExplosion();
		base.DestroySet();
	}

	private void DoDeathExplosion()
	{
		if (ironCount > 0)
		{
			float num = deathExplosionDamage * (float)ironCount;
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(GD.Load<PackedScene>("uid://0hfxonqijrv0"), gridPos);
			towerDefenseEffectParticlesOnce.GlobalPosition = GetLogicalGlobalPosition();
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
			AudioManager.Instance.AudioPlay("ExplodeDoomShroom");
			TowerDefenseCharacterEventExplodeHurt towerDefenseCharacterEventExplodeHurt = new TowerDefenseCharacterEventExplodeHurt();
			towerDefenseCharacterEventExplodeHurt.num = num;
			Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase> { towerDefenseCharacterEventExplodeHurt };
			TowerDefenseExplode.CreateExplode(GetLogicalGlobalPosition(), new Vector2(3.5f, 3.5f), eventList, new Array<TowerDefenseCharacter>(), camp, -1);
			CraterCreate(nolimit: true, "CraterDayGround", halfCrater: true);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnIronBreakDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_armor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnterArmedState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitArmedState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCannonRest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreIdleAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateEnergyBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureEnergyVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseStoredProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseAllStoredProjectiles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetCharge, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoDeathExplosion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnIronBreakDown && args.Count == 1)
		{
			OnIronBreakDown(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterArmedState && args.Count == 0)
		{
			EnterArmedState();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitArmedState && args.Count == 0)
		{
			ExitArmedState();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCannonRest && args.Count == 0)
		{
			OnCannonRest();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreIdleAnimation && args.Count == 0)
		{
			RestoreIdleAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateEnergyBall && args.Count == 0)
		{
			UpdateEnergyBall();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureEnergyVisual && args.Count == 0)
		{
			EnsureEnergyVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseStoredProjectile && args.Count == 0)
		{
			ReleaseStoredProjectile();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseAllStoredProjectiles && args.Count == 0)
		{
			ReleaseAllStoredProjectiles();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCharge && args.Count == 0)
		{
			ResetCharge();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.DoDeathExplosion && args.Count == 0)
		{
			DoDeathExplosion();
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
		if (method == MethodName.OnIronBreakDown)
		{
			return true;
		}
		if (method == MethodName.EnterArmedState)
		{
			return true;
		}
		if (method == MethodName.ExitArmedState)
		{
			return true;
		}
		if (method == MethodName.OnCannonRest)
		{
			return true;
		}
		if (method == MethodName.RestoreIdleAnimation)
		{
			return true;
		}
		if (method == MethodName.UpdateEnergyBall)
		{
			return true;
		}
		if (method == MethodName.EnsureEnergyVisual)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.ReleaseStoredProjectile)
		{
			return true;
		}
		if (method == MethodName.ReleaseAllStoredProjectiles)
		{
			return true;
		}
		if (method == MethodName.ResetCharge)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.DoDeathExplosion)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ironCount)
		{
			ironCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isArmed)
		{
			_isArmed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._energyVisual)
		{
			_energyVisual = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._wrapTimer)
		{
			_wrapTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ironPerFullCharge)
		{
			ironPerFullCharge = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.deathExplosionDamage)
		{
			deathExplosionDamage = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ironCount)
		{
			value = VariantUtils.CreateFrom<int>(ironCount);
			return true;
		}
		if (name == PropertyName._isArmed)
		{
			value = VariantUtils.CreateFrom(in _isArmed);
			return true;
		}
		if (name == PropertyName._energyVisual)
		{
			value = VariantUtils.CreateFrom(in _energyVisual);
			return true;
		}
		if (name == PropertyName._wrapTimer)
		{
			value = VariantUtils.CreateFrom(in _wrapTimer);
			return true;
		}
		if (name == PropertyName.ironPerFullCharge)
		{
			value = VariantUtils.CreateFrom(in ironPerFullCharge);
			return true;
		}
		if (name == PropertyName.deathExplosionDamage)
		{
			value = VariantUtils.CreateFrom(in deathExplosionDamage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._isArmed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._energyVisual, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._wrapTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ironPerFullCharge, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.deathExplosionDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ironCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ironCount, Variant.From<int>(ironCount));
		info.AddProperty(PropertyName._isArmed, Variant.From(in _isArmed));
		info.AddProperty(PropertyName._energyVisual, Variant.From(in _energyVisual));
		info.AddProperty(PropertyName._wrapTimer, Variant.From(in _wrapTimer));
		info.AddProperty(PropertyName.ironPerFullCharge, Variant.From(in ironPerFullCharge));
		info.AddProperty(PropertyName.deathExplosionDamage, Variant.From(in deathExplosionDamage));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ironCount, out var value))
		{
			ironCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isArmed, out var value2))
		{
			_isArmed = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._energyVisual, out var value3))
		{
			_energyVisual = value3.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._wrapTimer, out var value4))
		{
			_wrapTimer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ironPerFullCharge, out var value5))
		{
			ironPerFullCharge = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.deathExplosionDamage, out var value6))
		{
			deathExplosionDamage = value6.As<float>();
		}
	}
}
