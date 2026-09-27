using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/Event/ShovelEventSealMagicConfig.cs")]
public class ShovelEventSealMagicConfig : ShovelEventConfig
{
	public new class MethodName : ShovelEventConfig.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public static readonly StringName FindZombieNearestHome = "FindZombieNearestHome";
	}

	public new class PropertyName : ShovelEventConfig.PropertyName
	{
		public static readonly StringName costPercentage = "costPercentage";
	}

	public new class SignalName : ShovelEventConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double costPercentage = 0.03;

	public override void Execute(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character) && !(character.cost <= 0.0) && (!Global.Instance.isMultiplayerMode || MultiPlayerManager.Instance.isHost))
		{
			TowerDefenseCharacter towerDefenseCharacter = FindZombieNearestHome();
			if (towerDefenseCharacter != null)
			{
				TowerDefenseCharacterBuffSealMagic buffConfig = new TowerDefenseCharacterBuffSealMagic
				{
					time = character.cost * costPercentage
				};
				towerDefenseCharacter.buff.AddBuff(buffConfig);
			}
		}
	}

	private static TowerDefenseCharacter FindZombieNearestHome()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			return null;
		}
		TowerDefenseCharacter result = null;
		float num = 1f / 0f;
		List<TowerDefenseCharacter> cleanCharactersList = instance.characterRegistry.GetCleanCharactersList();
		for (int i = 0; i < cleanCharactersList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = cleanCharactersList[i];
			if (towerDefenseCharacter is TowerDefenseZombie && towerDefenseCharacter.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && !towerDefenseCharacter.instance.hypnoses && towerDefenseCharacter.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS && (towerDefenseCharacter.buff == null || !towerDefenseCharacter.buff.BuffHas("SealMagic")) && TowerDefenseCharacterBuffSealMagic.CanCarrySeal(towerDefenseCharacter))
			{
				float x = towerDefenseCharacter.WorldHitRect.Position.X;
				if (x < num)
				{
					num = x;
					result = towerDefenseCharacter;
				}
			}
		}
		return result;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindZombieNearestHome, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 1)
		{
			Execute(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindZombieNearestHome && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindZombieNearestHome());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindZombieNearestHome && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindZombieNearestHome());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.FindZombieNearestHome)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.costPercentage)
		{
			costPercentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.costPercentage)
		{
			value = VariantUtils.CreateFrom(in costPercentage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.costPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.costPercentage, Variant.From(in costPercentage));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.costPercentage, out var value))
		{
			costPercentage = value.As<double>();
		}
	}
}
