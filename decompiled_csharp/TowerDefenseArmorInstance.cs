using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Instance/TowerDefenseArmorInstance.cs")]
public class TowerDefenseArmorInstance : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Hurt = "Hurt";

		public static readonly StringName RebuildStageThresholds = "RebuildStageThresholds";

		public static readonly StringName RefreshDamageStageFromHitPoints = "RefreshDamageStageFromHitPoints";

		public static readonly StringName DealHurt = "DealHurt";

		public static readonly StringName QueueShieldImpactVisual = "QueueShieldImpactVisual";

		public static readonly StringName IsMetallic = "IsMetallic";

		public static readonly StringName RemoveArmor = "RemoveArmor";

		public static readonly StringName Draw = "Draw";

		public static readonly StringName DamagePartCreate = "DamagePartCreate";

		public static readonly StringName SetDamageStage = "SetDamageStage";

		public static readonly StringName DispatchStageChanged = "DispatchStageChanged";

		public static readonly StringName DispatchRemoved = "DispatchRemoved";

		public static readonly StringName Export = "Export";

		public static readonly StringName ExportSave = "ExportSave";

		public static readonly StringName ImportSave = "ImportSave";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName stagePersontage = "stagePersontage";

		public static readonly StringName hitpointScale = "hitpointScale";

		public static readonly StringName character = "character";

		public static readonly StringName sprite = "sprite";

		public static readonly StringName typeData = "typeData";

		public static readonly StringName slotConfig = "slotConfig";

		public static readonly StringName damagePointBase = "damagePointBase";

		public static readonly StringName hitpointsSave = "hitpointsSave";

		public static readonly StringName hitPoints = "hitPoints";

		public static readonly StringName armorMethodFlags = "armorMethodFlags";

		public static readonly StringName _stagePersontage = "_stagePersontage";

		public static readonly StringName _stageThresholds = "_stageThresholds";

		public static readonly StringName stageIndex = "stageIndex";

		public static readonly StringName isRemove = "isRemove";

		public static readonly StringName damagePartDropped = "damagePartDropped";

		public static readonly StringName hitpointScaleSave = "hitpointScaleSave";

		public static readonly StringName _hitpointScale = "_hitpointScale";

		public static readonly StringName _lastShieldImpactFrame = "_lastShieldImpactFrame";

		public static readonly StringName _shieldImpactSlot = "_shieldImpactSlot";

		public static readonly StringName _behaviorsRemoved = "_behaviorsRemoved";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private const double ShieldImpactDurationSeconds = 0.025;

	public TowerDefenseCharacter character;

	public AdobeAnimatePart sprite;

	public TowerDefenseArmorTypeData typeData;

	public ArmorSlotConfig slotConfig;

	public double damagePointBase;

	[Export(PropertyHint.None, "")]
	public double hitpointsSave;

	[Export(PropertyHint.None, "")]
	public double hitPoints;

	[Export(PropertyHint.None, "")]
	public int armorMethodFlags;

	private Array<double> _stagePersontage = new Array<double>();

	private double[] _stageThresholds = System.Array.Empty<double>();

	[Export(PropertyHint.None, "")]
	public int stageIndex;

	[Export(PropertyHint.None, "")]
	public bool isRemove;

	public bool damagePartDropped;

	[Export(PropertyHint.None, "")]
	public double hitpointScaleSave = 1.0;

	private double _hitpointScale = 1.0;

	private ulong _lastShieldImpactFrame = 18446744073709551615uL;

	private AdobeAnimateSlot _shieldImpactSlot;

	private readonly List<ArmorBehaviorRuntime> _behaviorRuntimes = new List<ArmorBehaviorRuntime>();

	private bool _behaviorsRemoved;

	[Export(PropertyHint.None, "")]
	public Array<double> stagePersontage
	{
		get
		{
			return _stagePersontage;
		}
		set
		{
			_stagePersontage = value ?? new Array<double>();
			RebuildStageThresholds();
		}
	}

	[Export(PropertyHint.None, "")]
	public double hitpointScale
	{
		get
		{
			return _hitpointScale;
		}
		set
		{
			if (_hitpointScale != value)
			{
				_hitpointScale = value;
				hitPoints *= value / hitpointScaleSave;
				hitpointsSave *= value / hitpointScaleSave;
				hitpointScaleSave = _hitpointScale;
			}
		}
	}

	public event Action<TowerDefenseArmorInstance, int> damagePointReach;

	public event Action<TowerDefenseArmorInstance> hitpointsEmpty;

	public event Action<TowerDefenseArmorInstance> remove;

	public TowerDefenseArmorInstance()
	{
	}

	public TowerDefenseArmorInstance(TowerDefenseCharacter _character, ArmorSlotConfig _slotConfig)
	{
		character = _character;
		slotConfig = _slotConfig;
		if (slotConfig == null)
		{
			isRemove = true;
			return;
		}
		TowerDefenseArmorRegistry.Init();
		typeData = TowerDefenseArmorRegistry.GetArmorType(slotConfig.armorName);
		if (typeData == null)
		{
			isRemove = true;
			return;
		}
		damagePointBase = ((slotConfig.damagePoint >= 0.0) ? slotConfig.damagePoint : typeData.damagePoint);
		hitPoints = damagePointBase;
		hitpointsSave = damagePointBase;
		armorMethodFlags = typeData.armorMethodFlags;
		if ((armorMethodFlags & 0x40) != 0)
		{
			stagePersontage = typeData.stagePersontage;
		}
		string replaceMethod = slotConfig.replaceMethod;
		if (!(replaceMethod == "Media"))
		{
			if (replaceMethod == "Sprite")
			{
				character.ClearArmor(slotConfig.armorName);
				sprite = CharacterArmorData.CreateArmorPartNode(character.sprite, slotConfig, typeData);
			}
		}
		else
		{
			character.SetArmor(slotConfig.armorName, 0);
		}
		if ((armorMethodFlags & 4) != 0)
		{
			TowerDefenseShieldImpactBatch.Prepare(character);
		}
		BindBehaviors(TowerDefenseBehaviorRegistry.Resolve(typeData.behaviorIds, typeData.behaviors));
		BindBehaviors(TowerDefenseBehaviorRegistry.Resolve(slotConfig.behaviorIds, slotConfig.behaviors));
	}

	public double Hurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, bool ignoreLimit = false)
	{
		num = DealHurt(num, playSplatAudio, velocity, createDamagePart, ignoreLimit);
		return num;
	}

	private bool TryApplyStableDamage(double num, bool playSplatAudio, Vector2 velocity, bool createDamagePart, bool ignoreLimit, out double remainingDamage)
	{
		remainingDamage = num;
		if (isRemove || _behaviorRuntimes.Count != 0 || character == null || typeData == null || !double.IsFinite(num) || num < 0.0)
		{
			return false;
		}
		if (!ignoreLimit && typeData.limitMaxHit != -1.0)
		{
			num = Mathf.Min(num, typeData.limitMaxHit);
		}
		if (hitPoints > num)
		{
			if (character.HasArmorHurtSubscribers)
			{
				character.EmitArmorHurt((int)num);
			}
			hitPoints -= num;
			num = 0.0;
		}
		else
		{
			double num2 = hitPoints;
			if (character.HasArmorHurtSubscribers)
			{
				character.EmitArmorHurt((int)num2);
			}
			num -= num2;
			hitPoints = 0.0;
		}
		if ((armorMethodFlags & 4) != 0)
		{
			QueueShieldImpactVisual();
		}
		if (playSplatAudio && !string.IsNullOrEmpty(typeData.impactAudio))
		{
			AudioManager.Instance.AudioPlay(typeData.impactAudio);
		}
		if (hitPoints > 0.0)
		{
			RefreshDamageStageFromHitPoints();
		}
		else if (hitPoints <= 0.0)
		{
			if (createDamagePart && (armorMethodFlags & 0x80) != 0)
			{
				velocity = ((!(velocity == Vector2.Zero)) ? new Vector2(velocity.X * (float)GD.RandRange(0.75, 1.25), velocity.Y * (float)GD.RandRange(0.75, 1.25)) : new Vector2((float)GD.RandRange(-100, 100) * (float)GD.RandRange(0.75, 1.25), -300f * (float)GD.RandRange(0.75, 1.25)));
				DamagePartCreate(velocity);
			}
			RemoveArmor(ArmorRemovalReason.Broken, dispatchBehavior: false);
		}
		remainingDamage = (((armorMethodFlags & 0x200) != 0) ? 0.0 : num);
		if (hitPoints <= 0.0)
		{
			DispatchRemoved(ArmorRemovalReason.Broken);
			hitpointsEmpty?.Invoke(this);
		}
		return true;
	}

	private void RebuildStageThresholds()
	{
		int count = _stagePersontage.Count;
		if (count == 0)
		{
			_stageThresholds = System.Array.Empty<double>();
			return;
		}
		_stageThresholds = new double[count];
		for (int i = 0; i < count; i++)
		{
			_stageThresholds[i] = _stagePersontage[i];
		}
	}

	public void RefreshDamageStageFromHitPoints()
	{
		if (isRemove || typeData == null || hitPoints <= 0.0 || (armorMethodFlags & 0x40) == 0)
		{
			return;
		}
		double num = damagePointBase * hitpointScale;
		if (double.IsFinite(num) && !(num <= 0.0))
		{
			double num2 = hitPoints / num;
			int i;
			for (i = 0; (uint)i < (uint)_stageThresholds.Length && num2 <= _stageThresholds[i]; i++)
			{
			}
			while (stageIndex < i)
			{
				stageIndex++;
				SetDamageStage(stageIndex);
			}
		}
	}

	public double DealHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, bool ignoreLimit = false)
	{
		if (isRemove)
		{
			return num;
		}
		if (TryApplyStableDamage(num, playSplatAudio, velocity, createDamagePart, ignoreLimit, out var remainingDamage))
		{
			return remainingDamage;
		}
		ArmorDamageContext context = new ArmorDamageContext(this, num, playSplatAudio, velocity, createDamagePart, ignoreLimit);
		DispatchBeforeDamage(ref context);
		if (context.Cancel)
		{
			return context.RemainingDamage;
		}
		num = context.Damage;
		playSplatAudio = context.PlaySplatAudio;
		velocity = context.Velocity;
		createDamagePart = context.CreateDamagePart;
		ignoreLimit = context.IgnoreLimit;
		double num2 = hitPoints;
		if (!ignoreLimit && typeData.limitMaxHit != -1.0)
		{
			num = Mathf.Min(num, typeData.limitMaxHit);
		}
		if (hitPoints > num)
		{
			character.EmitArmorHurt((int)num);
			hitPoints -= num;
			num = 0.0;
		}
		else
		{
			character.EmitArmorHurt((int)hitPoints);
			num -= hitPoints;
			hitPoints = 0.0;
		}
		if ((armorMethodFlags & 4) != 0)
		{
			QueueShieldImpactVisual();
		}
		string impactAudio = typeData.impactAudio;
		if (playSplatAudio && impactAudio != "")
		{
			AudioManager.Instance.AudioPlay(impactAudio);
		}
		if (hitPoints > 0.0)
		{
			RefreshDamageStageFromHitPoints();
		}
		else
		{
			if (createDamagePart && (armorMethodFlags & 0x80) != 0)
			{
				velocity = ((!(velocity == Vector2.Zero)) ? new Vector2(velocity.X * (float)GD.RandRange(0.75, 1.25), velocity.Y * (float)GD.RandRange(0.75, 1.25)) : new Vector2((float)GD.RandRange(-100, 100) * (float)GD.RandRange(0.75, 1.25), -300f * (float)GD.RandRange(0.75, 1.25)));
				DamagePartCreate(velocity);
			}
			RemoveArmor(ArmorRemovalReason.Broken, dispatchBehavior: false);
		}
		context.AppliedDamage = num2 - hitPoints;
		if ((armorMethodFlags & 0x200) != 0)
		{
			context.RemainingDamage = 0.0;
		}
		else
		{
			context.RemainingDamage = num;
		}
		DispatchAfterDamage(ref context);
		if (hitPoints <= 0.0)
		{
			DispatchRemoved(ArmorRemovalReason.Broken);
			hitpointsEmpty?.Invoke(this);
		}
		return context.RemainingDamage;
	}

	private void QueueShieldImpactVisual()
	{
		ulong physicsFrames = Engine.GetPhysicsFrames();
		if (_lastShieldImpactFrame == physicsFrames)
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(_shieldImpactSlot))
		{
			if (character == null || slotConfig == null || !character.damagePartSlot.TryGetValue(slotConfig.armorName, out var value))
			{
				return;
			}
			_shieldImpactSlot = character.GetNodeOrNull<AdobeAnimateSlot>(value.AsString());
		}
		if (GodotObject.IsInstanceValid(_shieldImpactSlot) && _shieldImpactSlot.IsInsideTree())
		{
			_lastShieldImpactFrame = physicsFrames;
			_shieldImpactSlot.AddRuntimeVisualOffset(new Vector2(GD.RandRange(-2, 2), GD.RandRange(-2, 2)));
			if (!TowerDefenseShieldImpactBatch.Schedule(this, _shieldImpactSlot, 0.025))
			{
				_shieldImpactSlot.SetRuntimeVisualOffset(Vector2.Zero);
			}
		}
	}

	public bool IsMetallic()
	{
		return (armorMethodFlags & 0x10) != 0;
	}

	public void RemoveArmor()
	{
		RemoveArmor(ArmorRemovalReason.Unknown);
	}

	public void RemoveArmor(ArmorRemovalReason reason, bool dispatchBehavior = true)
	{
		string damageAudio = typeData.damageAudio;
		if (damageAudio != "")
		{
			AudioManager.Instance.AudioPlay(damageAudio);
		}
		string replaceMethod = slotConfig.replaceMethod;
		if (!(replaceMethod == "Media"))
		{
			if (replaceMethod == "Sprite")
			{
				character.ClearArmor(slotConfig.armorName);
				if (GodotObject.IsInstanceValid(sprite))
				{
					if (sprite.GetParent() != null)
					{
						sprite.GetParent().RemoveChild(sprite);
					}
					sprite.QueueFree();
				}
				sprite = null;
			}
		}
		else
		{
			character.ClearArmor(slotConfig.armorName);
			if (GodotObject.IsInstanceValid(character.sprite))
			{
				character.sprite.SetAtlasReplace(slotConfig.replaceMediaName, string.Empty);
			}
		}
		if (dispatchBehavior)
		{
			DispatchRemoved(reason);
		}
	}

	public TowerDefenseMagnet Draw()
	{
		if (isRemove)
		{
			return null;
		}
		TowerDefenseMagnet towerDefenseMagnet = character.MagnetCreate(this, sprite);
		if (!GodotObject.IsInstanceValid(towerDefenseMagnet))
		{
			return null;
		}
		sprite = null;
		RemoveArmor(ArmorRemovalReason.Drawn);
		isRemove = true;
		remove?.Invoke(this);
		return towerDefenseMagnet;
	}

	public void DamagePartCreate(Vector2 velocity = default(Vector2))
	{
		character.DamagePartCreate(slotConfig.armorName, sprite, velocity, keepSlotScale: true, default, fromSync: false, null, 0L);
	}

	public void SetDamageStage(int index)
	{
		int num = ((typeData != null && typeData.stageAnimeTexturePaths != null && typeData.stageAnimeTexturePaths.Count > 0) ? Mathf.Clamp(index, 0, typeData.stageAnimeTexturePaths.Count - 1) : index);
		string replaceMethod = slotConfig.replaceMethod;
		if (!(replaceMethod == "Media"))
		{
			if (replaceMethod == "Sprite" && sprite != null && typeData != null && typeData.stageAnimeTexturePaths != null && typeData.stageAnimeTexturePaths.Count > 0)
			{
				sprite.externalAtlasTexturePath = typeData.stageAnimeTexturePaths[num];
			}
		}
		else
		{
			character.SetArmor(slotConfig.armorName, num);
		}
		damagePointReach?.Invoke(this, stageIndex);
		DispatchStageChanged(stageIndex);
	}

	private void BindBehaviors(IEnumerable<ArmorBehaviorDefinition> definitions)
	{
		if (definitions == null)
		{
			return;
		}
		foreach (ArmorBehaviorDefinition definition in definitions)
		{
			if (!GodotObject.IsInstanceValid(definition) || !definition.InitiallyEnabled)
			{
				continue;
			}
			try
			{
				ArmorBehaviorRuntime armorBehaviorRuntime = definition.CreateRuntime();
				if (armorBehaviorRuntime != null)
				{
					armorBehaviorRuntime.Bind(definition, this);
					_behaviorRuntimes.Add(armorBehaviorRuntime);
				}
			}
			catch (Exception ex)
			{
				GD.PushError($"[ArmorBehavior:E_BIND] behavior='{definition.GetDiagnosticName()}' armor='{slotConfig?.armorName}' reason='{ex.Message}'");
			}
		}
	}

	private void DispatchBeforeDamage(ref ArmorDamageContext context)
	{
		for (int i = 0; i < _behaviorRuntimes.Count; i++)
		{
			ArmorBehaviorRuntime armorBehaviorRuntime = _behaviorRuntimes[i];
			if (armorBehaviorRuntime.Enabled)
			{
				try
				{
					armorBehaviorRuntime.BeforeDamage(ref context);
				}
				catch (Exception exception)
				{
					DisableFaultedBehavior(armorBehaviorRuntime, "E_BEFORE_DAMAGE", exception);
				}
			}
		}
	}

	private void DispatchAfterDamage(ref ArmorDamageContext context)
	{
		for (int i = 0; i < _behaviorRuntimes.Count; i++)
		{
			ArmorBehaviorRuntime armorBehaviorRuntime = _behaviorRuntimes[i];
			if (armorBehaviorRuntime.Enabled)
			{
				try
				{
					armorBehaviorRuntime.AfterDamage(ref context);
				}
				catch (Exception exception)
				{
					DisableFaultedBehavior(armorBehaviorRuntime, "E_AFTER_DAMAGE", exception);
				}
			}
		}
	}

	private void DispatchStageChanged(int stage)
	{
		for (int i = 0; i < _behaviorRuntimes.Count; i++)
		{
			ArmorBehaviorRuntime armorBehaviorRuntime = _behaviorRuntimes[i];
			if (armorBehaviorRuntime.Enabled)
			{
				try
				{
					armorBehaviorRuntime.OnStageChanged(stage);
				}
				catch (Exception exception)
				{
					DisableFaultedBehavior(armorBehaviorRuntime, "E_STAGE", exception);
				}
			}
		}
	}

	private void DispatchRemoved(ArmorRemovalReason reason)
	{
		if (_behaviorsRemoved)
		{
			return;
		}
		_behaviorsRemoved = true;
		for (int i = 0; i < _behaviorRuntimes.Count; i++)
		{
			ArmorBehaviorRuntime armorBehaviorRuntime = _behaviorRuntimes[i];
			if (armorBehaviorRuntime.Enabled)
			{
				try
				{
					armorBehaviorRuntime.OnRemoved(reason);
				}
				catch (Exception exception)
				{
					DisableFaultedBehavior(armorBehaviorRuntime, "E_REMOVE", exception);
				}
			}
			try
			{
				armorBehaviorRuntime.Release();
			}
			catch (Exception ex)
			{
				GD.PushError($"[ArmorBehavior:E_RELEASE] armor='{slotConfig?.armorName}' reason='{ex.Message}'");
			}
		}
		_behaviorRuntimes.Clear();
	}

	private void DisableFaultedBehavior(ArmorBehaviorRuntime runtime, string errorCode, Exception exception)
	{
		runtime.Disable();
		GD.PushError($"[ArmorBehavior:{errorCode}] behavior='{runtime.Definition?.GetDiagnosticName()}' armor='{slotConfig?.armorName}' reason='{exception.Message}'");
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["slotConfig"] = slotConfig,
			["hitpointsSave"] = hitpointsSave,
			["hitPoints"] = hitPoints,
			["armorMethodFlags"] = armorMethodFlags,
			["stageIndex"] = stageIndex,
			["isRemove"] = isRemove
		};
	}

	public Dictionary ExportSave()
	{
		return new Dictionary
		{
			["armorName"] = slotConfig?.armorName ?? "",
			["hitpointsSave"] = hitpointsSave,
			["hitPoints"] = hitPoints,
			["stageIndex"] = stageIndex,
			["isRemove"] = isRemove,
			["hitpointScale"] = hitpointScale,
			["hitpointScaleSave"] = hitpointScaleSave
		};
	}

	public void ImportSave(Dictionary data)
	{
		hitpointScaleSave = data.GetValueOrDefault("hitpointScaleSave", hitpointScaleSave).AsDouble();
		hitpointScale = data.GetValueOrDefault("hitpointScale", hitpointScale).AsDouble();
		hitpointsSave = data.GetValueOrDefault("hitpointsSave", hitpointsSave).AsDouble();
		hitPoints = data.GetValueOrDefault("hitPoints", hitPoints).AsDouble();
		int num = data.GetValueOrDefault("stageIndex", 0).AsInt32();
		if (num > stageIndex)
		{
			stageIndex = num;
			SetDamageStage(stageIndex);
		}
		RefreshDamageStageFromHitPoints();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.Hurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "ignoreLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildStageThresholds, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshDamageStageFromHitPoints, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DealHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "ignoreLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueShieldImpactVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsMetallic, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveArmor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "dispatchBehavior", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Draw, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePartCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDamageStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchStageChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DispatchRemoved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Hurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(Hurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.RebuildStageThresholds && args.Count == 0)
		{
			RebuildStageThresholds();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDamageStageFromHitPoints && args.Count == 0)
		{
			RefreshDamageStageFromHitPoints();
			ret = default;
			return true;
		}
		if (method == MethodName.DealHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(DealHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.QueueShieldImpactVisual && args.Count == 0)
		{
			QueueShieldImpactVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.IsMetallic && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMetallic());
			return true;
		}
		if (method == MethodName.RemoveArmor && args.Count == 0)
		{
			RemoveArmor();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveArmor && args.Count == 2)
		{
			RemoveArmor(VariantUtils.ConvertTo<ArmorRemovalReason>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Draw && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMagnet>(Draw());
			return true;
		}
		if (method == MethodName.DamagePartCreate && args.Count == 1)
		{
			DamagePartCreate(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDamageStage && args.Count == 1)
		{
			SetDamageStage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchStageChanged && args.Count == 1)
		{
			DispatchStageChanged(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DispatchRemoved && args.Count == 1)
		{
			DispatchRemoved(VariantUtils.ConvertTo<ArmorRemovalReason>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.ExportSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportSave());
			return true;
		}
		if (method == MethodName.ImportSave && args.Count == 1)
		{
			ImportSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Hurt)
		{
			return true;
		}
		if (method == MethodName.RebuildStageThresholds)
		{
			return true;
		}
		if (method == MethodName.RefreshDamageStageFromHitPoints)
		{
			return true;
		}
		if (method == MethodName.DealHurt)
		{
			return true;
		}
		if (method == MethodName.QueueShieldImpactVisual)
		{
			return true;
		}
		if (method == MethodName.IsMetallic)
		{
			return true;
		}
		if (method == MethodName.RemoveArmor)
		{
			return true;
		}
		if (method == MethodName.Draw)
		{
			return true;
		}
		if (method == MethodName.DamagePartCreate)
		{
			return true;
		}
		if (method == MethodName.SetDamageStage)
		{
			return true;
		}
		if (method == MethodName.DispatchStageChanged)
		{
			return true;
		}
		if (method == MethodName.DispatchRemoved)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.ExportSave)
		{
			return true;
		}
		if (method == MethodName.ImportSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.stagePersontage)
		{
			stagePersontage = VariantUtils.ConvertToArray<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpointScale)
		{
			hitpointScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.character)
		{
			character = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimatePart>(in value);
			return true;
		}
		if (name == PropertyName.typeData)
		{
			typeData = VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in value);
			return true;
		}
		if (name == PropertyName.slotConfig)
		{
			slotConfig = VariantUtils.ConvertTo<ArmorSlotConfig>(in value);
			return true;
		}
		if (name == PropertyName.damagePointBase)
		{
			damagePointBase = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpointsSave)
		{
			hitpointsSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitPoints)
		{
			hitPoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.armorMethodFlags)
		{
			armorMethodFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._stagePersontage)
		{
			_stagePersontage = VariantUtils.ConvertToArray<double>(in value);
			return true;
		}
		if (name == PropertyName._stageThresholds)
		{
			_stageThresholds = VariantUtils.ConvertTo<double[]>(in value);
			return true;
		}
		if (name == PropertyName.stageIndex)
		{
			stageIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isRemove)
		{
			isRemove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.damagePartDropped)
		{
			damagePartDropped = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hitpointScaleSave)
		{
			hitpointScaleSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hitpointScale)
		{
			_hitpointScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._lastShieldImpactFrame)
		{
			_lastShieldImpactFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._shieldImpactSlot)
		{
			_shieldImpactSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._behaviorsRemoved)
		{
			_behaviorsRemoved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.stagePersontage)
		{
			value = VariantUtils.CreateFromArray(stagePersontage);
			return true;
		}
		if (name == PropertyName.hitpointScale)
		{
			value = VariantUtils.CreateFrom<double>(hitpointScale);
			return true;
		}
		if (name == PropertyName.character)
		{
			value = VariantUtils.CreateFrom(in character);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		if (name == PropertyName.typeData)
		{
			value = VariantUtils.CreateFrom(in typeData);
			return true;
		}
		if (name == PropertyName.slotConfig)
		{
			value = VariantUtils.CreateFrom(in slotConfig);
			return true;
		}
		if (name == PropertyName.damagePointBase)
		{
			value = VariantUtils.CreateFrom(in damagePointBase);
			return true;
		}
		if (name == PropertyName.hitpointsSave)
		{
			value = VariantUtils.CreateFrom(in hitpointsSave);
			return true;
		}
		if (name == PropertyName.hitPoints)
		{
			value = VariantUtils.CreateFrom(in hitPoints);
			return true;
		}
		if (name == PropertyName.armorMethodFlags)
		{
			value = VariantUtils.CreateFrom(in armorMethodFlags);
			return true;
		}
		if (name == PropertyName._stagePersontage)
		{
			value = VariantUtils.CreateFromArray(_stagePersontage);
			return true;
		}
		if (name == PropertyName._stageThresholds)
		{
			value = VariantUtils.CreateFrom(in _stageThresholds);
			return true;
		}
		if (name == PropertyName.stageIndex)
		{
			value = VariantUtils.CreateFrom(in stageIndex);
			return true;
		}
		if (name == PropertyName.isRemove)
		{
			value = VariantUtils.CreateFrom(in isRemove);
			return true;
		}
		if (name == PropertyName.damagePartDropped)
		{
			value = VariantUtils.CreateFrom(in damagePartDropped);
			return true;
		}
		if (name == PropertyName.hitpointScaleSave)
		{
			value = VariantUtils.CreateFrom(in hitpointScaleSave);
			return true;
		}
		if (name == PropertyName._hitpointScale)
		{
			value = VariantUtils.CreateFrom(in _hitpointScale);
			return true;
		}
		if (name == PropertyName._lastShieldImpactFrame)
		{
			value = VariantUtils.CreateFrom(in _lastShieldImpactFrame);
			return true;
		}
		if (name == PropertyName._shieldImpactSlot)
		{
			value = VariantUtils.CreateFrom(in _shieldImpactSlot);
			return true;
		}
		if (name == PropertyName._behaviorsRemoved)
		{
			value = VariantUtils.CreateFrom(in _behaviorsRemoved);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.character, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.typeData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.slotConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.damagePointBase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointsSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitPoints, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.armorMethodFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._stagePersontage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._stageThresholds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.stagePersontage, PropertyHint.TypeString, "3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.stageIndex, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRemove, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.damagePartDropped, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointScaleSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._hitpointScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastShieldImpactFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shieldImpactSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._behaviorsRemoved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.stagePersontage, Variant.CreateFrom(stagePersontage));
		info.AddProperty(PropertyName.hitpointScale, Variant.From<double>(hitpointScale));
		info.AddProperty(PropertyName.character, Variant.From(in character));
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
		info.AddProperty(PropertyName.typeData, Variant.From(in typeData));
		info.AddProperty(PropertyName.slotConfig, Variant.From(in slotConfig));
		info.AddProperty(PropertyName.damagePointBase, Variant.From(in damagePointBase));
		info.AddProperty(PropertyName.hitpointsSave, Variant.From(in hitpointsSave));
		info.AddProperty(PropertyName.hitPoints, Variant.From(in hitPoints));
		info.AddProperty(PropertyName.armorMethodFlags, Variant.From(in armorMethodFlags));
		info.AddProperty(PropertyName._stagePersontage, Variant.CreateFrom(_stagePersontage));
		info.AddProperty(PropertyName._stageThresholds, Variant.From(in _stageThresholds));
		info.AddProperty(PropertyName.stageIndex, Variant.From(in stageIndex));
		info.AddProperty(PropertyName.isRemove, Variant.From(in isRemove));
		info.AddProperty(PropertyName.damagePartDropped, Variant.From(in damagePartDropped));
		info.AddProperty(PropertyName.hitpointScaleSave, Variant.From(in hitpointScaleSave));
		info.AddProperty(PropertyName._hitpointScale, Variant.From(in _hitpointScale));
		info.AddProperty(PropertyName._lastShieldImpactFrame, Variant.From(in _lastShieldImpactFrame));
		info.AddProperty(PropertyName._shieldImpactSlot, Variant.From(in _shieldImpactSlot));
		info.AddProperty(PropertyName._behaviorsRemoved, Variant.From(in _behaviorsRemoved));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.stagePersontage, out var value))
		{
			stagePersontage = value.AsGodotArray<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpointScale, out var value2))
		{
			hitpointScale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.character, out var value3))
		{
			character = value3.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.sprite, out var value4))
		{
			sprite = value4.As<AdobeAnimatePart>();
		}
		if (info.TryGetProperty(PropertyName.typeData, out var value5))
		{
			typeData = value5.As<TowerDefenseArmorTypeData>();
		}
		if (info.TryGetProperty(PropertyName.slotConfig, out var value6))
		{
			slotConfig = value6.As<ArmorSlotConfig>();
		}
		if (info.TryGetProperty(PropertyName.damagePointBase, out var value7))
		{
			damagePointBase = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpointsSave, out var value8))
		{
			hitpointsSave = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitPoints, out var value9))
		{
			hitPoints = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.armorMethodFlags, out var value10))
		{
			armorMethodFlags = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._stagePersontage, out var value11))
		{
			_stagePersontage = value11.AsGodotArray<double>();
		}
		if (info.TryGetProperty(PropertyName._stageThresholds, out var value12))
		{
			_stageThresholds = value12.As<double[]>();
		}
		if (info.TryGetProperty(PropertyName.stageIndex, out var value13))
		{
			stageIndex = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isRemove, out var value14))
		{
			isRemove = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.damagePartDropped, out var value15))
		{
			damagePartDropped = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hitpointScaleSave, out var value16))
		{
			hitpointScaleSave = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hitpointScale, out var value17))
		{
			_hitpointScale = value17.As<double>();
		}
		if (info.TryGetProperty(PropertyName._lastShieldImpactFrame, out var value18))
		{
			_lastShieldImpactFrame = value18.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._shieldImpactSlot, out var value19))
		{
			_shieldImpactSlot = value19.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._behaviorsRemoved, out var value20))
		{
			_behaviorsRemoved = value20.As<bool>();
		}
	}
}
