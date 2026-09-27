using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/EMPCherry/Scene/TowerDefensePlantEMPCherry.cs")]
public class TowerDefensePlantEMPCherry : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnExplodeHandler = "OnExplodeHandler";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _empRange = "_empRange";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _empShockScene;

	private ExplodeComponent _explodeComponent;

	private Vector2 _empRange;

	private static PackedScene EMP_SHOCK_SCENE => _empShockScene ?? (_empShockScene = GD.Load<PackedScene>("uid://fbt1dfatem6i"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode && inGame)
		{
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			if (_explodeComponent != null)
			{
				_explodeComponent.OnExplode += OnExplodeHandler;
			}
			_empRange = TowerDefenseManager.Instance.GetMapGridSize() * 2.75f * 0.5f;
		}
	}

	public override void _ExitTree()
	{
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.OnExplode -= OnExplodeHandler;
		}
		base._ExitTree();
	}

	private void OnExplodeHandler()
	{
		if (GodotObject.IsInstanceValid(EMP_SHOCK_SCENE))
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(EMP_SHOCK_SCENE, gridPos, "Idle");
			if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
			{
				towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint);
				TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			}
		}
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		Array campTarget = TowerDefenseManager.Instance.GetCampTarget(camp);
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		foreach (Variant item in campTarget)
		{
			TowerDefenseZombie towerDefenseZombie = item.As<TowerDefenseZombie>();
			if (towerDefenseZombie != null && (towerDefenseZombie.instance.physiqueTypeFlags & 0x800) != 0 && towerDefenseZombie.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
			{
				Vector2 vector = (towerDefenseZombie.GetLogicalGlobalPosition() - logicalGlobalPosition).Abs();
				if (!(vector.X > _empRange.X) && !(vector.Y > _empRange.Y))
				{
					TowerDefenseCharacterBuffEMP towerDefenseCharacterBuffEMP = new TowerDefenseCharacterBuffEMP();
					towerDefenseCharacterBuffEMP.time = 8.0;
					towerDefenseZombie.buff.AddBuff(towerDefenseCharacterBuffEMP);
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExplodeHandler, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnExplodeHandler && args.Count == 0)
		{
			OnExplodeHandler();
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
		if (method == MethodName.OnExplodeHandler)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._empRange)
		{
			_empRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._empRange)
		{
			value = VariantUtils.CreateFrom(in _empRange);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName._empRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._empRange, Variant.From(in _empRange));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._empRange, out var value))
		{
			_empRange = value.As<Vector2>();
		}
	}
}
