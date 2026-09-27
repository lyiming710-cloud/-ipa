using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter1/Magnetnut/Scene/TowerDefensePlantBowlingMagnetnut.cs")]
public class TowerDefensePlantBowlingMagnetnut : TowerDefensePlantBowlingBase
{
	public new class MethodName : TowerDefensePlantBowlingBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ProcessArmorDrawAsync = "ProcessArmorDrawAsync";

		public static readonly StringName Bowling = "Bowling";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlantBowlingBase.PropertyName
	{
		public static readonly StringName hitArmor = "hitArmor";

		public static readonly StringName isDraw = "isDraw";

		public static readonly StringName _isProcessing = "_isProcessing";
	}

	public new class SignalName : TowerDefensePlantBowlingBase.SignalName
	{
	}

	private MagnetComponent magnetComponent;

	public bool hitArmor;

	public bool isDraw;

	private bool _isProcessing;

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
			GD.PushError("Bowling Magnet-nut is missing its Magnet resource runtime.");
		}
		bowlingComponent.OnBowling += Bowling;
		if (config.customData != null)
		{
			string text = GameSaveManager.Instance.GetTowerDefensePacketValue("PlantMagnetnut").GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
				.GetValueOrDefault("Custom", "")
				.AsString();
			if (text != "")
			{
				currentCustom = new Array<string> { text };
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		BowlingComponent bowlingComponent = base.bowlingComponent;
		if (bowlingComponent != null && !bowlingComponent.IsReleased)
		{
			base.bowlingComponent.OnBowling -= Bowling;
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			if (GodotObject.IsInstanceValid(magnetComponent.magnet))
			{
				TowerDefenseCharacterEventBowlingHurt towerDefenseCharacterEventBowlingHurt = (TowerDefenseCharacterEventBowlingHurt)bowlingComponent.hitEvent[0].Duplicate();
				towerDefenseCharacterEventBowlingHurt.num = 1800.0 + magnetComponent.breakDownArmor.hitPoints * 4.5;
				bowlingComponent.hitEvent[0] = towerDefenseCharacterEventBowlingHurt;
			}
			else if (!isDraw && !_isProcessing)
			{
				_isProcessing = true;
				ProcessArmorDrawAsync();
			}
		}
	}

	private async void ProcessArmorDrawAsync()
	{
		try
		{
			bool flag = await magnetComponent.CanArmorDraw();
			if (GodotObject.IsInstanceValid(this))
			{
				if (flag)
				{
					isDraw = true;
					magnetComponent.ArmorDrawNear();
					isDraw = false;
				}
				else
				{
					TowerDefenseCharacterEventBowlingHurt towerDefenseCharacterEventBowlingHurt = (TowerDefenseCharacterEventBowlingHurt)bowlingComponent.hitEvent[0].Duplicate();
					towerDefenseCharacterEventBowlingHurt.num = 1800.0;
					bowlingComponent.hitEvent[0] = towerDefenseCharacterEventBowlingHurt;
				}
			}
		}
		finally
		{
			_isProcessing = false;
		}
	}

	public void Bowling(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(magnetComponent.breakDownArmor))
		{
			magnetComponent.BreakDownOver();
		}
	}

	public override void DestroySet()
	{
		magnetComponent.Destroy();
		base.DestroySet();
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "hitArmor", hitArmor },
			{ "isDraw", isDraw }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		hitArmor = data.GetValueOrDefault("hitArmor", false).AsBool();
		isDraw = data.GetValueOrDefault("isDraw", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessArmorDrawAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Bowling, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ProcessArmorDrawAsync && args.Count == 0)
		{
			ProcessArmorDrawAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.Bowling && args.Count == 1)
		{
			Bowling(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.ProcessArmorDrawAsync)
		{
			return true;
		}
		if (method == MethodName.Bowling)
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
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.hitArmor)
		{
			hitArmor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isDraw)
		{
			isDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isProcessing)
		{
			_isProcessing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.hitArmor)
		{
			value = VariantUtils.CreateFrom(in hitArmor);
			return true;
		}
		if (name == PropertyName.isDraw)
		{
			value = VariantUtils.CreateFrom(in isDraw);
			return true;
		}
		if (name == PropertyName._isProcessing)
		{
			value = VariantUtils.CreateFrom(in _isProcessing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isDraw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isProcessing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hitArmor, Variant.From(in hitArmor));
		info.AddProperty(PropertyName.isDraw, Variant.From(in isDraw));
		info.AddProperty(PropertyName._isProcessing, Variant.From(in _isProcessing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hitArmor, out var value))
		{
			hitArmor = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isDraw, out var value2))
		{
			isDraw = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isProcessing, out var value3))
		{
			_isProcessing = value3.As<bool>();
		}
	}
}
