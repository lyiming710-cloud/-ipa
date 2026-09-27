using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/GoldHammer/Scene/TowerDefensePlantGoldHammer.cs")]
public class TowerDefensePlantGoldHammer : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName _DeferredReady = "_DeferredReady";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Explode = "Explode";

		public static readonly StringName ResolveRepairableCrater = "ResolveRepairableCrater";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName crater = "crater";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ExplodeComponent _explodeComponent;

	public TowerDefenseCharacter crater;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			_explodeComponent.OnExplode += Explode;
			zombiePlaceDamage = 3000.0;
			CallDeferred("_DeferredReady");
		}
	}

	private async void _DeferredReady()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (GodotObject.IsInstanceValid(targetZombie))
		{
			_explodeComponent.explodeAnimeTimeScale = 2f;
			_explodeComponent.explodeAnimeClips = "WhackZombie";
		}
		else if (GodotObject.IsInstanceValid(cell))
		{
			crater = ResolveRepairableCrater();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= Explode;
		}
	}

	public void Explode()
	{
		if (GodotObject.IsInstanceValid(targetZombie))
		{
			AudioManager.Instance.AudioPlay("Bonk");
			_explodeComponent.CreateParticlesEffect();
			targetZombie.Hurt(zombiePlaceDamage, playSplatAudio: true, Vector2.Zero, createDamagePart: false);
			targetZombie.AttackDeal(null, "Explode", zombiePlaceDamage);
			return;
		}
		TowerDefenseCrater towerDefenseCrater = ResolveRepairableCrater();
		if (GodotObject.IsInstanceValid(towerDefenseCrater))
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			towerDefenseCrater.Destroy();
			if (!instance.hypnoses)
			{
				TowerDefenseCharacterEventGoldShardCreate towerDefenseCharacterEventGoldShardCreate = new TowerDefenseCharacterEventGoldShardCreate();
				towerDefenseCharacterEventGoldShardCreate.num = 1;
				towerDefenseCharacterEventGoldShardCreate.Execute(logicalGlobalPosition, this);
			}
		}
	}

	private TowerDefenseCrater ResolveRepairableCrater()
	{
		if (crater is TowerDefenseCrater towerDefenseCrater && GodotObject.IsInstanceValid(towerDefenseCrater) && !towerDefenseCrater.isDestroy)
		{
			return towerDefenseCrater;
		}
		crater = null;
		if (!GodotObject.IsInstanceValid(cell))
		{
			return null;
		}
		TowerDefenseCrater repairableCrater = cell.GetRepairableCrater();
		if (GodotObject.IsInstanceValid(repairableCrater))
		{
			crater = repairableCrater;
		}
		return repairableCrater;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._DeferredReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveRepairableCrater, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._DeferredReady && args.Count == 0)
		{
			_DeferredReady();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveRepairableCrater && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCrater>(ResolveRepairableCrater());
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
		if (method == MethodName._DeferredReady)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		if (method == MethodName.ResolveRepairableCrater)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.crater)
		{
			crater = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.crater)
		{
			value = VariantUtils.CreateFrom(in crater);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.crater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.crater, Variant.From(in crater));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.crater, out var value))
		{
			crater = value.As<TowerDefenseCharacter>();
		}
	}
}
