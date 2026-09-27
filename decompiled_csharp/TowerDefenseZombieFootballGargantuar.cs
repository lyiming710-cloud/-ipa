using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/FootballGargantuar/Scene/Base/TowerDefenseZombieFootballGargantuar.cs")]
public class TowerDefenseZombieFootballGargantuar : TowerDefenseZombieGargantuarBase
{
	public new class MethodName : TowerDefenseZombieGargantuarBase.MethodName
	{
		public new static readonly StringName DamagePointReach = "DamagePointReach";
	}

	public new class PropertyName : TowerDefenseZombieGargantuarBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseZombieGargantuarBase.SignalName
	{
	}

	private const string ZOMBIE_FOOTBALL_GARGANTUAR_BODY12 = "uid://cho4e0w7335lx";

	private const string ZOMBIE_FOOTBALL_GARGANTUAR_BODY13 = "uid://rm3iebyhewoi";

	private const string ZOMBIE_FOOTBALL_GARGANTUAR_HEAD2 = "uid://d2h300mowbih5";

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (!(damagePointName == "Arm"))
		{
			if (damagePointName == "Head")
			{
				sprite.SetAtlasReplace("Zombie_football_gargantuar_head.png", "uid://d2h300mowbih5");
				sprite.SetAtlasReplace("Zombie_football_gargantuar_body1.png", "uid://rm3iebyhewoi");
			}
		}
		else
		{
			sprite.SetAtlasReplace("Zombie_football_gargantuar_body1.png", "uid://cho4e0w7335lx");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
