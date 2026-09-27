using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/MagnetBomb/Scene/TowerDefensePlantMagnetBomb.cs")]
public class TowerDefensePlantMagnetBomb : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName FireEntered = "FireEntered";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName drawCharacter = "drawCharacter";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private MagnetComponent _magnetComponent;

	private ExplodeComponent _explodeComponent;

	private StateHandle _fireState;

	private bool _roleStateSignalsConnected;

	private readonly List<TowerDefenseCharacter> _scanTargets = new List<TowerDefenseCharacter>();

	public Array<TowerDefenseCharacter> drawCharacter = new Array<TowerDefenseCharacter>();

	public bool over;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_magnetComponent = componentManager.GetRuntime<MagnetComponent>();
			MagnetComponent magnetComponent = _magnetComponent;
			if (magnetComponent != null && !magnetComponent.IsReleased)
			{
				_magnetComponent.SetAlive(false);
			}
			else
			{
				GD.PushError("Magnet Bomb is missing its Magnet resource runtime.");
			}
			_explodeComponent = componentManager.GetRuntime<ExplodeComponent>();
			ExplodeComponent explodeComponent = _explodeComponent;
			if (explodeComponent != null && !explodeComponent.IsReleased)
			{
				_explodeComponent.SetAlive(false);
			}
			else
			{
				GD.PushError("Magnet Bomb is missing its Explode resource runtime.");
			}
			_fireState = StateMachine?.GetStateById("plant.magnet_bomb.fire");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (!_roleStateSignalsConnected)
		{
			StateHandle fireState = _fireState;
			if (fireState != null && fireState.IsValid)
			{
				_fireState.Entered += FireEntered;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_fireState.Entered -= FireEntered;
			_fireState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !TowerDefenseManager.Instance.IsGameRunning() || !inGame || !componentAlive || componentRunning || over)
		{
			return;
		}
		_magnetComponent.FillCanArmorDrawCharacterList(_scanTargets);
		drawCharacter.Clear();
		foreach (TowerDefenseCharacter scanTarget in _scanTargets)
		{
			drawCharacter.Add(scanTarget);
		}
		if (drawCharacter.Count > 0)
		{
			SendStateEvent("ToFire");
			return;
		}
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.SetAlive(true);
		}
	}

	public void FireEntered()
	{
		ExplodeComponent explodeComponent = _explodeComponent;
		if (explodeComponent != null && !explodeComponent.IsReleased)
		{
			_explodeComponent.SetAlive(true);
		}
		instance.invincible = true;
		sprite.SetAnimation("Explode", loop: false);
		Vector2I[] array = new Vector2I[9]
		{
			new Vector2I(-1, -1),
			new Vector2I(0, -1),
			new Vector2I(1, -1),
			new Vector2I(-1, 0),
			new Vector2I(0, 0),
			new Vector2I(1, 0),
			new Vector2I(-1, 1),
			new Vector2I(0, 1),
			new Vector2I(1, 1)
		};
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		foreach (TowerDefenseCharacter item in drawCharacter)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				Vector2I vector2I = gridPos + array[(int)(GD.Randi() % array.Length)];
				vector2I = new Vector2I(Mathf.Clamp(vector2I.X, 1, mapGridNum.X), Mathf.Clamp(vector2I.Y, 1, mapGridNum.Y));
				Vector2 mapCellPosCenter = TowerDefenseManager.Instance.GetMapCellPosCenter(vector2I);
				item.gridPos = vector2I;
				Vector2 logicalGlobalPosition = item.GetLogicalGlobalPosition();
				Tween tween = item.CreateTween();
				tween.SetParallel();
				tween.SetEase(Tween.EaseType.Out);
				tween.SetTrans(Tween.TransitionType.Quart);
				tween.TweenMethod(Callable.From<Vector2>(item.SetLogicalGlobalPosition), logicalGlobalPosition, mapCellPosCenter, 0.5);
				ShadowComponent shadowComponent = item.shadowComponent;
				if (shadowComponent != null && !shadowComponent.IsReleased)
				{
					item.shadowComponent.TweenSaveShadowPositionY(tween, item.shadowComponent.saveShadowPosition.Y + mapCellPosCenter.Y - logicalGlobalPosition.Y, 0.5);
				}
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "over", over } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		over = (bool)data.GetValueOrDefault("over", false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FireEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FireEntered && args.Count == 0)
		{
			FireEntered();
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
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.FireEntered)
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
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.drawCharacter)
		{
			drawCharacter = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName.drawCharacter)
		{
			value = VariantUtils.CreateFromArray(drawCharacter);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.drawCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName.drawCharacter, Variant.CreateFrom(drawCharacter));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.drawCharacter, out var value2))
		{
			drawCharacter = value2.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
	}
}
