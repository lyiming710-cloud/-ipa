using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/PumpkinRobot/Scene/TowerDefensePlantPumpkinRobot.cs")]
public class TowerDefensePlantPumpkinRobot : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName StrengthenOtherPumpkinRobots = "StrengthenOtherPumpkinRobots";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _destroySetOver = "_destroySetOver";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ExplodeComponent _explodeComponent;

	private const double MaxHp = 8000.0;

	private const double HpPerDeathrattle = 2000.0;

	private bool _destroySetOver;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		_explodeComponent = componentManager.GetRuntime<ExplodeComponent>("character.explode");
		if (!inGame && !editorMapPreviewMode)
		{
			PumpkinSprite pumpkinSprite = sprite as PumpkinSprite;
			if (GodotObject.IsInstanceValid(pumpkinSprite) && GodotObject.IsInstanceValid(pumpkinSprite.back))
			{
				pumpkinSprite.back.ZIndex = 0;
			}
		}
	}

	public override void DestroySet()
	{
		if (_destroySetOver)
		{
			return;
		}
		_destroySetOver = true;
		if (!isShovel)
		{
			if (_explodeComponent != null && !_explodeComponent.IsReleased)
			{
				_explodeComponent.Explode();
			}
			StrengthenOtherPumpkinRobots();
		}
		base.DestroySet();
	}

	private void StrengthenOtherPumpkinRobots()
	{
		if (config == null)
		{
			return;
		}
		foreach (Node item in GetTree().GetNodesInGroup(config.name))
		{
			if (item is TowerDefensePlantPumpkinRobot towerDefensePlantPumpkinRobot && towerDefensePlantPumpkinRobot != this && GodotObject.IsInstanceValid(towerDefensePlantPumpkinRobot))
			{
				towerDefensePlantPumpkinRobot.instance.hitpointsSave = Mathf.Min(8000.0, towerDefensePlantPumpkinRobot.instance.hitpointsSave + 2000.0);
				towerDefensePlantPumpkinRobot.Health(2000.0);
				towerDefensePlantPumpkinRobot.instance.hitpoints = Mathf.Min(towerDefensePlantPumpkinRobot.instance.hitpoints, towerDefensePlantPumpkinRobot.instance.hitpointsSave);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StrengthenOtherPumpkinRobots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.StrengthenOtherPumpkinRobots && args.Count == 0)
		{
			StrengthenOtherPumpkinRobots();
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
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.StrengthenOtherPumpkinRobots)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._destroySetOver)
		{
			_destroySetOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._destroySetOver)
		{
			value = VariantUtils.CreateFrom(in _destroySetOver);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroySetOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._destroySetOver, Variant.From(in _destroySetOver));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._destroySetOver, out var value))
		{
			_destroySetOver = value.As<bool>();
		}
	}
}
