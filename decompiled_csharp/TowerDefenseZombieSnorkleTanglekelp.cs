using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter3/Snorkle/Scene/Tanglekelp/TowerDefenseZombieSnorkleTanglekelp.cs")]
public class TowerDefenseZombieSnorkleTanglekelp : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName AttackEntered = "AttackEntered";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName AttackExited = "AttackExited";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName DieEntered = "DieEntered";

		public static readonly StringName AnimeStarted = "AnimeStarted";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName Purify = "Purify";

		public static readonly StringName DragBegin = "DragBegin";

		public static readonly StringName IsWaterDragOnlyActive = "IsWaterDragOnlyActive";

		public static readonly StringName EnableWaterDragIfSubmerged = "EnableWaterDragIfSubmerged";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName GlobalPositionX = "GlobalPositionX";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private TanglekelpComponent _tanglekelpComponent;

	private float GlobalPositionX
	{
		get
		{
			return GetLogicalGlobalPosition().X;
		}
		set
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X = value;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_tanglekelpComponent = componentManager.GetRuntime<TanglekelpComponent>();
			if (_tanglekelpComponent != null)
			{
				_tanglekelpComponent.OnDragBegin += DragBegin;
			}
			sprite.OnAnimeStarted += AnimeStarted;
			EnableWaterDragIfSubmerged();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
		if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
		{
			_tanglekelpComponent.OnDragBegin -= DragBegin;
		}
	}

	public override void AttackEntered()
	{
		if (IsWaterDragOnlyActive())
		{
			instance.maskFlags = 32;
			Walk();
			return;
		}
		base.AttackEntered();
		if (inWater)
		{
			instance.maskFlags = 9;
		}
	}

	public override void AttackProcessing(double delta)
	{
		if (IsWaterDragOnlyActive())
		{
			instance.maskFlags = 32;
			Walk();
		}
		else
		{
			base.AttackProcessing(delta);
		}
	}

	public override void AttackExited()
	{
		base.AttackExited();
		if (inWater)
		{
			instance.maskFlags = 32;
		}
	}

	public override void InWater()
	{
		base.InWater();
		EnableWaterDragIfSubmerged();
	}

	public override void WalkProcessing(double delta)
	{
		if (IsWaterDragOnlyActive())
		{
			SwimComponent swimComponent = base.swimComponent;
			if (swimComponent != null && !swimComponent.IsReleased)
			{
				base.swimComponent.WalkProcessing((float)delta);
			}
		}
		else
		{
			base.WalkProcessing(delta);
		}
	}

	public override void OutWater()
	{
		TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
		if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
		{
			_tanglekelpComponent.SetAlive(alive: false);
		}
		groundHeight = -100.0;
		z = -100.0;
		base.OutWater();
		CreateTween().TweenProperty(sprite, "offset", new Vector2(-50f, -80f), 0.25);
		GlobalPositionX -= Scale.X * transformPoint.Scale.X * 30f;
		instance.maskFlags = 9;
	}

	public override void DieEntered()
	{
		base.DieEntered();
		sprite.offset = new Vector2(-50f, -80f);
	}

	public void AnimeStarted(string clip)
	{
		if (clip == "Swim")
		{
			sprite.offset = new Vector2(-10f, -100f);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (clip == "Jump")
		{
			instance.maskFlags = 32;
			sprite.offset = new Vector2(-10f, -100f);
			GlobalPositionX -= Scale.X * transformPoint.Scale.X * 40f;
		}
		base.AnimeCompleted(clip);
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
			if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
			{
				_tanglekelpComponent.SetAlive(alive: false);
			}
			sprite.GetNode<AdobeAnimateSpriteBase>("%Head").Visible = false;
		}
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantTanglekelp");
		if (cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos);
			towerDefenseCharacter.WakeUp();
			if (instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantTanglekelp", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	public void DragBegin(TowerDefenseCharacter target)
	{
		spritePause = true;
	}

	private bool IsWaterDragOnlyActive()
	{
		if (inWater)
		{
			TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
			if (tanglekelpComponent != null && tanglekelpComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
			{
				return _tanglekelpComponent.Alive;
			}
		}
		return false;
	}

	private void EnableWaterDragIfSubmerged()
	{
		if (inWater)
		{
			TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
			if (tanglekelpComponent != null && tanglekelpComponent.Lifecycle == ComponentRuntimeLifecycle.Active)
			{
				_tanglekelpComponent.SetAlive(alive: true);
				instance.maskFlags = 32;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DragBegin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsWaterDragOnlyActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnableWaterDragIfSubmerged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackExited && args.Count == 0)
		{
			AttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeStarted && args.Count == 1)
		{
			AnimeStarted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
			ret = default;
			return true;
		}
		if (method == MethodName.DragBegin && args.Count == 1)
		{
			DragBegin(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsWaterDragOnlyActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWaterDragOnlyActive());
			return true;
		}
		if (method == MethodName.EnableWaterDragIfSubmerged && args.Count == 0)
		{
			EnableWaterDragIfSubmerged();
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
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackExited)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.AnimeStarted)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.Purify)
		{
			return true;
		}
		if (method == MethodName.DragBegin)
		{
			return true;
		}
		if (method == MethodName.IsWaterDragOnlyActive)
		{
			return true;
		}
		if (method == MethodName.EnableWaterDragIfSubmerged)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			GlobalPositionX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			value = VariantUtils.CreateFrom<float>(GlobalPositionX);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.GlobalPositionX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GlobalPositionX, Variant.From<float>(GlobalPositionX));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GlobalPositionX, out var value))
		{
			GlobalPositionX = value.As<float>();
		}
	}
}
