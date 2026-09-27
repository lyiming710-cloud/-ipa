using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/Resource/Event/ShovelEventCreateProjectileConfig.cs")]
public class ShovelEventCreateProjectileConfig : ShovelEventConfig
{
	public new class MethodName : ShovelEventConfig.MethodName
	{
		public new static readonly StringName Execute = "Execute";
	}

	public new class PropertyName : ShovelEventConfig.PropertyName
	{
		public static readonly StringName everyNum = "everyNum";

		public static readonly StringName projecileName = "projecileName";

		public static readonly StringName projectilebaseDamage = "projectilebaseDamage";
	}

	public new class SignalName : ShovelEventConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int everyNum = 10;

	[Export(PropertyHint.None, "")]
	public string projecileName;

	[Export(PropertyHint.None, "")]
	public double projectilebaseDamage = 20.0;

	public override void Execute(TowerDefenseCharacter character)
	{
		Vector2 logicalGlobalPosition = character.GetLogicalGlobalPosition();
		double groundHeight = character.GetGroundHeight(logicalGlobalPosition.Y);
		for (int i = 0; i < (int)Mathf.Floor(character.cost / (double)everyNum); i++)
		{
			double num = GD.RandRange(-10.0, 40.0);
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData(new StringName(projecileName));
			towerDefenseProjectileCreateData.baseDamage = projectilebaseDamage;
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = character.gridPos.Y
			};
			FireComponent.CreateProjectilePositionByData(character, null, groundHeight + num - 20.0, logicalGlobalPosition + new Vector2(GD.RandRange(-10, 10), -20f), new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, character.camp, default, overrides);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.everyNum)
		{
			everyNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projecileName)
		{
			projecileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.projectilebaseDamage)
		{
			projectilebaseDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.everyNum)
		{
			value = VariantUtils.CreateFrom(in everyNum);
			return true;
		}
		if (name == PropertyName.projecileName)
		{
			value = VariantUtils.CreateFrom(in projecileName);
			return true;
		}
		if (name == PropertyName.projectilebaseDamage)
		{
			value = VariantUtils.CreateFrom(in projectilebaseDamage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.everyNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projecileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.projectilebaseDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.everyNum, Variant.From(in everyNum));
		info.AddProperty(PropertyName.projecileName, Variant.From(in projecileName));
		info.AddProperty(PropertyName.projectilebaseDamage, Variant.From(in projectilebaseDamage));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.everyNum, out var value))
		{
			everyNum = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projecileName, out var value2))
		{
			projecileName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.projectilebaseDamage, out var value3))
		{
			projectilebaseDamage = value3.As<double>();
		}
	}
}
