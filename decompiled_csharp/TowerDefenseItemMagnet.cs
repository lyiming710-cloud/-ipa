using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/Magnet/Scene/TowerDefenseItemMagnet.cs")]
public class TowerDefenseItemMagnet : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitMagnet = "InitMagnet";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName eventList = "eventList";

		public static readonly StringName armor = "armor";

		public static readonly StringName over = "over";

		public static readonly StringName _drawResolved = "_drawResolved";

		public static readonly StringName _destroyCountdownActive = "_destroyCountdownActive";

		public static readonly StringName _destroyTimer = "_destroyTimer";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private static PackedScene _CHERRY_BOMB_EXPLOSION;

	private MagnetComponent magnetComponent;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	public TowerDefenseArmorInstance armor;

	public bool over;

	private bool _drawResolved;

	private bool _destroyCountdownActive;

	private double _destroyTimer = 1.0;

	private static PackedScene CHERRY_BOMB_EXPLOSION => _CHERRY_BOMB_EXPLOSION ?? (_CHERRY_BOMB_EXPLOSION = GD.Load<PackedScene>("uid://cibtjjjomdxnh"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			magnetComponent = componentManager.GetRuntime<MagnetComponent>();
			if (magnetComponent == null)
			{
				GD.PushError("Magnet item is missing its Magnet resource runtime.");
			}
			HitBoxDestroy();
			if (!IsProgressRestoreInFlight)
			{
				InitMagnet();
			}
		}
	}

	private async void InitMagnet()
	{
		if (_drawResolved || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || magnetComponent == null)
		{
			return;
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (GodotObject.IsInstanceValid(this) && !isDestroy)
		{
			if (await magnetComponent.CanArmorDraw())
			{
				magnetComponent.ArmorDrawNear();
			}
			_drawResolved = true;
			_destroyTimer = 1.0;
			_destroyCountdownActive = true;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && _destroyCountdownActive && !isDestroy)
		{
			_destroyTimer = Math.Max(0.0, _destroyTimer - Math.Max(0.0, delta));
			if (!(_destroyTimer > 0.0))
			{
				_destroyCountdownActive = false;
				Destroy();
			}
		}
	}

	public override void Destroy(bool freeInstance = true)
	{
		_destroyCountdownActive = false;
		magnetComponent?.Destroy();
		base.Destroy(freeInstance);
	}

	public override async void DestroySet()
	{
		if (!over)
		{
			over = true;
			ViewManager.Instance.CameraShake(new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)), 5.0, 0.05, 4);
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(CHERRY_BOMB_EXPLOSION, gridPos);
			towerDefenseEffectParticlesOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseExplode.CreateExplode(GetLogicalGlobalPosition(), new Vector2(1.5f, 1.5f), eventList, new Array<TowerDefenseCharacter>(), camp, -1);
			AudioManager.Instance.AudioPlay("ExplodeCherrybomb");
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["drawResolved"] = _drawResolved,
			["destroyCountdownActive"] = _destroyCountdownActive,
			["destroyTimer"] = _destroyTimer,
			["over"] = over
		};
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		if (data == null || !data.ContainsKey("drawResolved"))
		{
			_drawResolved = true;
			_destroyCountdownActive = true;
			_destroyTimer = 0.0;
		}
		else
		{
			_drawResolved = data.GetValueOrDefault("drawResolved", false).AsBool();
			_destroyCountdownActive = data.GetValueOrDefault("destroyCountdownActive", false).AsBool();
			_destroyTimer = Math.Max(0.0, data.GetValueOrDefault("destroyTimer", 0.0).AsDouble());
			over = data.GetValueOrDefault("over", over).AsBool();
		}
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		if (!_drawResolved && !_destroyCountdownActive && !over)
		{
			InitMagnet();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitMagnet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "freeInstance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitMagnet && args.Count == 0)
		{
			InitMagnet();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 1)
		{
			Destroy(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
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
		if (method == MethodName.InitMagnet)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore)
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
		if (name == PropertyName.armor)
		{
			armor = VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._drawResolved)
		{
			_drawResolved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._destroyCountdownActive)
		{
			_destroyCountdownActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._destroyTimer)
		{
			_destroyTimer = VariantUtils.ConvertTo<double>(in value);
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
		if (name == PropertyName.armor)
		{
			value = VariantUtils.CreateFrom(in armor);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName._drawResolved)
		{
			value = VariantUtils.CreateFrom(in _drawResolved);
			return true;
		}
		if (name == PropertyName._destroyCountdownActive)
		{
			value = VariantUtils.CreateFrom(in _destroyCountdownActive);
			return true;
		}
		if (name == PropertyName._destroyTimer)
		{
			value = VariantUtils.CreateFrom(in _destroyTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.armor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._drawResolved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroyCountdownActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._destroyTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.armor, Variant.From(in armor));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName._drawResolved, Variant.From(in _drawResolved));
		info.AddProperty(PropertyName._destroyCountdownActive, Variant.From(in _destroyCountdownActive));
		info.AddProperty(PropertyName._destroyTimer, Variant.From(in _destroyTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventList, out var value))
		{
			eventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.armor, out var value2))
		{
			armor = value2.As<TowerDefenseArmorInstance>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._drawResolved, out var value4))
		{
			_drawResolved = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._destroyCountdownActive, out var value5))
		{
			_destroyCountdownActive = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._destroyTimer, out var value6))
		{
			_destroyTimer = value6.As<double>();
		}
	}
}
