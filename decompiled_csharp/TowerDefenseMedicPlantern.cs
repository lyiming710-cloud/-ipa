using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/MedicPlantern/Scene/TowerDefenseMedicPlantern.cs")]
public class TowerDefenseMedicPlantern : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName Timeout = "Timeout";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _healRange = "_healRange";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private CharacterAabbAreaComponent _checkArea;

	private CharacterTimerComponent _timerComponent;

	private Vector2 _healRange;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_checkArea = componentManager.GetRuntime<CharacterAabbAreaComponent>("character.aabb_area.0");
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			_timerComponent.OnTimeout += Timeout;
			AudioManager.Instance.AudioPlay("Plantern");
			_checkArea.SetRuntimeRectangleSize(0, TowerDefenseManager.Instance.GetMapGridSize() * 2.75f);
			_healRange = TowerDefenseManager.Instance.GetMapGridSize() * 2.75f * 0.5f;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= Timeout;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && TowerDefenseManager.Instance.IsGameRunning() && inGame && !_timerComponent.IsRunning("Spawn"))
		{
			_timerComponent.Run("Spawn", 1.0);
		}
	}

	public void Timeout(string timerName)
	{
		if (!(timerName == "Spawn"))
		{
			return;
		}
		if (!sprite.pause && !instance.sleep)
		{
			Array campFriendly = TowerDefenseManager.Instance.GetCampFriendly(camp);
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			foreach (Variant item in campFriendly)
			{
				TowerDefenseCharacter towerDefenseCharacter = item.As<TowerDefenseCharacter>();
				if (towerDefenseCharacter is TowerDefensePlantBowlingBase || towerDefenseCharacter.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS || towerDefenseCharacter.instance.hitpoints >= towerDefenseCharacter.instance.hitpointsSave)
				{
					continue;
				}
				Vector2 vector = (towerDefenseCharacter.GetLogicalGlobalPosition() - logicalGlobalPosition).Abs();
				if (!(vector.X > _healRange.X) && !(vector.Y > _healRange.Y))
				{
					towerDefenseCharacter.Health(0.01 * towerDefenseCharacter.instance.hitpointsSave);
					if (towerDefenseCharacter.instance.hitpoints >= towerDefenseCharacter.instance.hitpointsSave)
					{
						towerDefenseCharacter.instance.hitpoints = towerDefenseCharacter.instance.hitpointsSave;
					}
				}
			}
		}
		_timerComponent.Run("Spawn", 1.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.Timeout)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._healRange)
		{
			_healRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._healRange)
		{
			value = VariantUtils.CreateFrom(in _healRange);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName._healRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._healRange, Variant.From(in _healRange));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._healRange, out var value))
		{
			_healRange = value.As<Vector2>();
		}
	}
}
