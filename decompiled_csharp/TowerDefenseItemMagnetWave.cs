using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/MagnetWave/Scene/TowerDefenseItemMagnetWave.cs")]
public class TowerDefenseItemMagnetWave : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitMagnet = "InitMagnet";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName armorList = "armorList";

		public static readonly StringName over = "over";

		public static readonly StringName drawNum = "drawNum";

		public static readonly StringName _drawCompleted = "_drawCompleted";

		public static readonly StringName _drawLoopRunning = "_drawLoopRunning";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	private MagnetComponent magnetComponent;

	public Array<TowerDefenseMagnet> armorList = new Array<TowerDefenseMagnet>();

	public bool over;

	public int drawNum = 1;

	private int _drawCompleted;

	private bool _drawLoopRunning;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			magnetComponent = componentManager.GetRuntime<MagnetComponent>();
			if (magnetComponent == null)
			{
				GD.PushError("Magnet Wave is missing its Magnet resource runtime.");
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
		if (_drawLoopRunning || _drawCompleted >= drawNum || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || magnetComponent == null)
		{
			return;
		}
		_drawLoopRunning = true;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		while (_drawCompleted < drawNum && GodotObject.IsInstanceValid(this) && !isDestroy)
		{
			if (await magnetComponent.CanArmorDraw())
			{
				magnetComponent.ArmorDrawNear();
				TowerDefenseMagnet drawnMagnet = magnetComponent.magnet;
				magnetComponent.magnet = null;
				magnetComponent.breakDownArmor = null;
				_drawCompleted++;
				await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
				if (GodotObject.IsInstanceValid(drawnMagnet))
				{
					armorList.Add(drawnMagnet);
				}
			}
			else
			{
				_drawCompleted++;
			}
		}
		_drawLoopRunning = false;
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint())
		{
			return;
		}
		foreach (TowerDefenseMagnet armor in armorList)
		{
			if (!GodotObject.IsInstanceValid(armor))
			{
				continue;
			}
			Marker2D marker2D = magnetComponent?.posMarker;
			if (marker2D != null)
			{
				Vector2 logicalGlobalPosition = GetLogicalGlobalPosition(marker2D);
				if ((double)armor.GlobalPosition.DistanceTo(logicalGlobalPosition) >= 0.01)
				{
					armor.GlobalPosition = armor.GlobalPosition.Lerp(logicalGlobalPosition, (float)(10.0 * delta));
					armor.Scale = armor.Scale.Lerp(Vector2.Zero, (float)(10.0 * delta));
				}
			}
		}
	}

	public override void Destroy(bool freeInstance = true)
	{
		magnetComponent?.Destroy();
		base.Destroy(freeInstance);
	}

	public override void AnimeCompleted(string clip)
	{
		if (clip == "Idle")
		{
			Destroy();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["drawCompleted"] = _drawCompleted };
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_drawLoopRunning = false;
		if (data == null || !data.ContainsKey("drawCompleted"))
		{
			_drawCompleted = Mathf.Max(0, drawNum);
		}
		else
		{
			_drawCompleted = Mathf.Clamp(data.GetValueOrDefault("drawCompleted", 0).AsInt32(), 0, Mathf.Max(0, drawNum));
		}
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		if (_drawCompleted < drawNum)
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
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.AnimeCompleted)
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
		if (name == PropertyName.armorList)
		{
			armorList = VariantUtils.ConvertToArray<TowerDefenseMagnet>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.drawNum)
		{
			drawNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._drawCompleted)
		{
			_drawCompleted = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._drawLoopRunning)
		{
			_drawLoopRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.armorList)
		{
			value = VariantUtils.CreateFromArray(armorList);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.drawNum)
		{
			value = VariantUtils.CreateFrom(in drawNum);
			return true;
		}
		if (name == PropertyName._drawCompleted)
		{
			value = VariantUtils.CreateFrom(in _drawCompleted);
			return true;
		}
		if (name == PropertyName._drawLoopRunning)
		{
			value = VariantUtils.CreateFrom(in _drawLoopRunning);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.armorList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.drawNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._drawCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._drawLoopRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.armorList, Variant.CreateFrom(armorList));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.drawNum, Variant.From(in drawNum));
		info.AddProperty(PropertyName._drawCompleted, Variant.From(in _drawCompleted));
		info.AddProperty(PropertyName._drawLoopRunning, Variant.From(in _drawLoopRunning));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.armorList, out var value))
		{
			armorList = value.AsGodotArray<TowerDefenseMagnet>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value2))
		{
			over = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.drawNum, out var value3))
		{
			drawNum = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._drawCompleted, out var value4))
		{
			_drawCompleted = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._drawLoopRunning, out var value5))
		{
			_drawLoopRunning = value5.As<bool>();
		}
	}
}
