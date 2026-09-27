using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/DisguiserBlover/Scene/TowerDefensePlantDisguiserBlover.cs")]
public class TowerDefensePlantDisguiserBlover : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName BlowMethod = "BlowMethod";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName hpNext = "hpNext";

		public static readonly StringName hpNextInterval = "hpNextInterval";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _BLOW;

	public double hpNext;

	public double hpNextInterval;

	public bool over;

	private static PackedScene BLOW => _BLOW ?? (_BLOW = GD.Load<PackedScene>("uid://j7kll4h65yex"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			hpNextInterval = instance.hitpoints / 6.0;
			hpNext = instance.hitpoints - hpNextInterval;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint())
		{
			while (instance.hitpoints <= hpNext)
			{
				hpNext -= hpNextInterval;
				BlowMethod();
			}
		}
	}

	public override void DestroySet()
	{
		if (!isShovel && !over)
		{
			over = true;
			double num = 0.0;
			while (hpNext >= 0.0)
			{
				hpNext -= hpNextInterval;
				num++;
			}
			BlowMethod(num);
		}
	}

	public void BlowMethod(double blowScale = 1.0)
	{
		BattleEventBus.Instance.EmitBlowAllEffectEmit();
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(BLOW, gridPos, "Idle");
		towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		foreach (TowerDefenseCharacter item in TowerDefenseManager.Instance.GetCharacterTarget(this))
		{
			if (item is TowerDefenseZombie && item.instance.zombiePhysique >= TowerDefenseEnum.ZOMBIE_PHYSIQUE.HUGE)
			{
				item.BlowBack(0.25 * blowScale, 0.25);
			}
			else if (item.instance.unUseBuffFlags != -1 && (item.instance.unUseBuffFlags & 0x200) == 0 && !(item is TowerDefensePlant) && !(item is TowerDefenseGravestone) && !(item is TowerDefenseItem) && (item.instance.collisionFlags & 0x10) == 0 && (item.instance.collisionFlags & 0x20) == 0)
			{
				if (item.instance.collisionFlags != 0)
				{
					item.BlowBack(0.5 * blowScale, 0.25);
				}
				if ((item.instance.collisionFlags & 2) != 0)
				{
					item.Blow();
				}
			}
		}
		AudioManager.Instance.AudioPlay("Blover");
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "hpNext", hpNext },
			{ "hpNextInterval", hpNextInterval },
			{ "over", over }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		hpNext = data.GetValueOrDefault("hpNext", Variant.From<double>(0.0)).AsDouble();
		hpNextInterval = data.GetValueOrDefault("hpNextInterval", Variant.From<double>(0.0)).AsDouble();
		over = data.GetValueOrDefault("over", Variant.From<bool>(false)).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlowMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "blowScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.BlowMethod && args.Count == 1)
		{
			BlowMethod(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.BlowMethod)
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
		if (name == PropertyName.hpNext)
		{
			hpNext = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hpNextInterval)
		{
			hpNextInterval = VariantUtils.ConvertTo<double>(in value);
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
		if (name == PropertyName.hpNext)
		{
			value = VariantUtils.CreateFrom(in hpNext);
			return true;
		}
		if (name == PropertyName.hpNextInterval)
		{
			value = VariantUtils.CreateFrom(in hpNextInterval);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.hpNext, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.hpNextInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hpNext, Variant.From(in hpNext));
		info.AddProperty(PropertyName.hpNextInterval, Variant.From(in hpNextInterval));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hpNext, out var value))
		{
			hpNext = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hpNextInterval, out var value2))
		{
			hpNextInterval = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
	}
}
