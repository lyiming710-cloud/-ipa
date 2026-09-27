using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/MagnetshroomS/Scene/TowerDefensePlantMagnetshroomS.cs")]
public class TowerDefensePlantMagnetshroomS : TowerDefensePlant
{
	private readonly struct StoredProjectileHandle(int index, TowerDefenseProjectileConfig config)
	{
		public readonly int Index = index;

		public readonly TowerDefenseProjectileConfig Config = config;
	}

	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName BreakDown = "BreakDown";

		public static readonly StringName SpawnStoredProjectile = "SpawnStoredProjectile";

		public static readonly StringName CanReleaseStoredProjectile = "CanReleaseStoredProjectile";

		public static readonly StringName ReleaseStoredProjectile = "ReleaseStoredProjectile";

		public static readonly StringName DrawTarget = "DrawTarget";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName breakDownTime = "breakDownTime";

		public static readonly StringName drawEvent = "drawEvent";

		public static readonly StringName timer = "timer";

		public static readonly StringName _breakDownTime = "_breakDownTime";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string StoredProjectileName = "StarIron";

	private const int SpawnCount = 5;

	private const float ReleasedProjectileSpeed = 600f;

	public MagnetComponent magnetComponent;

	public FireComponent fireComponent;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> drawEvent = new Array<TowerDefenseCharacterEventBase>();

	private readonly List<StoredProjectileHandle> _storedProjectiles = new List<StoredProjectileHandle>();

	public double timer;

	private double _breakDownTime = 15.0;

	[Export(PropertyHint.None, "")]
	public double breakDownTime
	{
		get
		{
			return _breakDownTime;
		}
		set
		{
			_breakDownTime = value;
			if (IsNodeReady())
			{
				MagnetComponent magnetComponent = this.magnetComponent;
				if (magnetComponent != null && !magnetComponent.IsReleased)
				{
					this.magnetComponent.breakDownTime = (float)value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		magnetComponent = componentManager.GetRuntime<MagnetComponent>();
		if (magnetComponent == null)
		{
			GD.PushError("MagnetshroomS is missing its Magnet resource runtime.");
			return;
		}
		fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
		if (fireComponent == null)
		{
			GD.PushError("MagnetshroomS is missing its Fire resource runtime.");
			return;
		}
		magnetComponent.OnBreakDown += BreakDown;
		magnetComponent.OnDrawTarget += DrawTarget;
	}

	public override void _ExitTree()
	{
		MagnetComponent magnetComponent = this.magnetComponent;
		if (magnetComponent != null && !magnetComponent.IsReleased)
		{
			this.magnetComponent.OnBreakDown -= BreakDown;
			this.magnetComponent.OnDrawTarget -= DrawTarget;
		}
		base._ExitTree();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (_storedProjectiles.Count == 0)
		{
			return;
		}
		BulletField bulletField = BulletField.Instance;
		if (!GodotObject.IsInstanceValid(bulletField))
		{
			_storedProjectiles.Clear();
			return;
		}
		timer += delta;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		for (int num = _storedProjectiles.Count - 1; num >= 0; num--)
		{
			StoredProjectileHandle stored = _storedProjectiles[num];
			if (!IsStoredProjectileActive(bulletField, stored))
			{
				_storedProjectiles.RemoveAt(num);
			}
			else
			{
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(stored.Index);
				float num2 = (float)num * ((float)Math.PI * 2f / 5f);
				bulletDataRef.pos = logicalGlobalPosition + new Vector2(Mathf.Cos((float)(timer * 2.0) + num2) * 50f, Mathf.Sin((float)timer + num2) * 10f);
				bulletDataRef.rotation += (float)(delta * (double)bulletDataRef.rotateScale * (double)bulletDataRef.fireDirX);
			}
		}
		if (CanReleaseStoredProjectile())
		{
			ReleaseStoredProjectile();
		}
	}

	public override void DestroySet()
	{
		magnetComponent?.Destroy();
		BulletField bulletField = BulletField.Instance;
		for (int num = _storedProjectiles.Count - 1; num >= 0; num--)
		{
			StoredProjectileHandle stored = _storedProjectiles[num];
			if (GodotObject.IsInstanceValid(bulletField) && IsStoredProjectileActive(bulletField, stored))
			{
				bulletField.Despawn(stored.Index);
			}
		}
		_storedProjectiles.Clear();
	}

	public virtual void BreakDown(TowerDefenseArmorInstance _armor)
	{
		FireComponent fireComponent = this.fireComponent;
		if (fireComponent != null && !fireComponent.IsReleased)
		{
			for (int i = 0; i < 5; i++)
			{
				SpawnStoredProjectile();
			}
		}
	}

	private void SpawnStoredProjectile()
	{
		int num = BulletField.Instance?.ActiveCount ?? 0;
		TowerDefenseProjectileCreateData projectileData = new TowerDefenseProjectileCreateData("StarIron")
		{
			damageFlags = 2,
			fireMethodFlags = 32,
			collisionFlags = 11,
			overrideCatapultHeight = true,
			catapultHeight = 400.0
		};
		fireComponent.CreateProjectileByData(0, Vector2.Zero, projectileData, -1, camp, Vector2.Zero, new BulletFieldSpawnOverrides
		{
			gridYOverride = gridPos.Y
		});
		BulletField bulletField = BulletField.Instance;
		if (!GodotObject.IsInstanceValid(bulletField) || bulletField.ActiveCount <= num)
		{
			return;
		}
		int lastSpawnedIndex = bulletField.LastSpawnedIndex;
		if (lastSpawnedIndex >= 0 && bulletField.IsBulletActive(lastSpawnedIndex))
		{
			ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(lastSpawnedIndex);
			if (bulletDataRef.fireCharacter == this)
			{
				bulletDataRef.externalControlled = true;
				bulletDataRef.hitOver = true;
				bulletDataRef.vel = Vector2.Zero;
				bulletDataRef.speed = 0f;
				bulletDataRef.target = null;
				bulletDataRef.magneticTarget = null;
				bulletDataRef.pos = new Vector2(bulletDataRef.pos.X, GetLogicalGlobalPosition().Y);
				_storedProjectiles.Add(new StoredProjectileHandle(lastSpawnedIndex, bulletDataRef.config));
			}
		}
	}

	private bool CanReleaseStoredProjectile()
	{
		FireComponent fireComponent = this.fireComponent;
		if (fireComponent == null || fireComponent.IsReleased || _storedProjectiles.Count == 0)
		{
			return false;
		}
		BulletField bulletField = BulletField.Instance;
		List<StoredProjectileHandle> storedProjectiles = _storedProjectiles;
		StoredProjectileHandle stored = storedProjectiles[storedProjectiles.Count - 1];
		if (!GodotObject.IsInstanceValid(bulletField) || !IsStoredProjectileActive(bulletField, stored))
		{
			return false;
		}
		ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(stored.Index);
		return this.fireComponent.CheckTrackTarget(bulletDataRef.collisionFlags);
	}

	private bool ReleaseStoredProjectile(bool refreshFireComponent = true)
	{
		if (_storedProjectiles.Count == 0)
		{
			return false;
		}
		int index = _storedProjectiles.Count - 1;
		StoredProjectileHandle stored = _storedProjectiles[index];
		_storedProjectiles.RemoveAt(index);
		BulletField bulletField = BulletField.Instance;
		if (!GodotObject.IsInstanceValid(bulletField) || !IsStoredProjectileActive(bulletField, stored))
		{
			return false;
		}
		ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(stored.Index);
		bulletDataRef.externalControlled = false;
		bulletDataRef.hitOver = false;
		bulletDataRef.speed = 600f;
		float num = Mathf.Sign(Scale.X);
		if (Mathf.IsZeroApprox(num))
		{
			num = 1f;
		}
		bulletDataRef.vel = new Vector2(600f * num, 0f);
		bulletDataRef.fireDirX = num;
		bulletDataRef.trackOpen = true;
		bulletDataRef.checkAll = true;
		bulletDataRef.target = null;
		bulletDataRef.magneticTarget = null;
		bulletDataRef.savePos = bulletDataRef.pos;
		if (refreshFireComponent)
		{
			FireComponent fireComponent = this.fireComponent;
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				this.fireComponent.Refresh();
			}
		}
		return true;
	}

	private bool IsStoredProjectileActive(BulletField field, StoredProjectileHandle stored)
	{
		if (!GodotObject.IsInstanceValid(field) || !field.IsBulletActive(stored.Index))
		{
			return false;
		}
		ref BulletData bulletDataRef = ref field.GetBulletDataRef(stored.Index);
		if (bulletDataRef.config == stored.Config)
		{
			return bulletDataRef.fireCharacter == this;
		}
		return false;
	}

	public virtual void DrawTarget(TowerDefenseCharacter target)
	{
		TowerDefenseExplode.CreateExplode(target.GetLogicalGlobalPosition(), new Vector2(1.5f, 1.5f), drawEvent, new Array<TowerDefenseCharacter>(), camp, -1);
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "timer", timer },
			{ "breakDownTime", breakDownTime }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		timer = data.GetValueOrDefault("timer", 0.0).AsDouble();
		breakDownTime = data.GetValueOrDefault("breakDownTime", 15.0).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BreakDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_armor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnStoredProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanReleaseStoredProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseStoredProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "refreshFireComponent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.BreakDown && args.Count == 1)
		{
			BreakDown(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnStoredProjectile && args.Count == 0)
		{
			SpawnStoredProjectile();
			ret = default;
			return true;
		}
		if (method == MethodName.CanReleaseStoredProjectile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReleaseStoredProjectile());
			return true;
		}
		if (method == MethodName.ReleaseStoredProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReleaseStoredProjectile(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawTarget && args.Count == 1)
		{
			DrawTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.BreakDown)
		{
			return true;
		}
		if (method == MethodName.SpawnStoredProjectile)
		{
			return true;
		}
		if (method == MethodName.CanReleaseStoredProjectile)
		{
			return true;
		}
		if (method == MethodName.ReleaseStoredProjectile)
		{
			return true;
		}
		if (method == MethodName.DrawTarget)
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
		if (name == PropertyName.breakDownTime)
		{
			breakDownTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.drawEvent)
		{
			drawEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._breakDownTime)
		{
			_breakDownTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.breakDownTime)
		{
			value = VariantUtils.CreateFrom<double>(breakDownTime);
			return true;
		}
		if (name == PropertyName.drawEvent)
		{
			value = VariantUtils.CreateFromArray(drawEvent);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName._breakDownTime)
		{
			value = VariantUtils.CreateFrom(in _breakDownTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.drawEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.breakDownTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._breakDownTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.breakDownTime, Variant.From<double>(breakDownTime));
		info.AddProperty(PropertyName.drawEvent, Variant.CreateFrom(drawEvent));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName._breakDownTime, Variant.From(in _breakDownTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.breakDownTime, out var value))
		{
			breakDownTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.drawEvent, out var value2))
		{
			drawEvent = value2.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value3))
		{
			timer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._breakDownTime, out var value4))
		{
			_breakDownTime = value4.As<double>();
		}
	}
}
