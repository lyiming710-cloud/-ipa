using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/SquashCandy/Scene/TowerDefensePlantSquashCandy.cs")]
public class TowerDefensePlantSquashCandy : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName JumpDownSmash = "JumpDownSmash";

		public static readonly StringName CreateParticlesEffect = "CreateParticlesEffect";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName eventList = "eventList";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _SQUASH_CANDY_THROW_PARTICLES;

	private AttackComponent _attackComponent;

	private SquashComponent _squashComponent;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	private static PackedScene SQUASH_CANDY_THROW_PARTICLES => _SQUASH_CANDY_THROW_PARTICLES ?? (_SQUASH_CANDY_THROW_PARTICLES = GD.Load<PackedScene>("uid://c4qigqg84djb"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			_squashComponent = componentManager.GetRuntime<SquashComponent>();
			if (_squashComponent != null)
			{
				_squashComponent.OnJumpDownSmash += JumpDownSmash;
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SquashComponent squashComponent = _squashComponent;
		if (squashComponent != null && !squashComponent.IsReleased)
		{
			_squashComponent.OnJumpDownSmash -= JumpDownSmash;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		sprite.timeScale = timeScale;
	}

	public void JumpDownSmash()
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 effectPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
		CreateParticlesEffect(effectPosition);
		TowerDefenseExplode.CreateExplode(logicalGlobalPosition, new Vector2(1.4f, 1.4f), eventList, new Array<TowerDefenseCharacter>(), camp, instance.collisionFlags | 0x10);
	}

	public TowerDefenseEffectParticlesOnce CreateParticlesEffect(Vector2 effectPosition)
	{
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(SQUASH_CANDY_THROW_PARTICLES, gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = effectPosition;
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		return towerDefenseEffectParticlesOnce;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.JumpDownSmash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateParticlesEffect, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "effectPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.JumpDownSmash && args.Count == 0)
		{
			JumpDownSmash();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateParticlesEffect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectParticlesOnce>(CreateParticlesEffect(VariantUtils.ConvertTo<Vector2>(in args[0])));
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
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.JumpDownSmash)
		{
			return true;
		}
		if (method == MethodName.CreateParticlesEffect)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventList, out var value))
		{
			eventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
