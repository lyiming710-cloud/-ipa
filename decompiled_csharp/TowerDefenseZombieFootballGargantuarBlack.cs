using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/FootballGargantuar/Scene/Black/TowerDefenseZombieFootballGargantuarBlack.cs")]
public class TowerDefenseZombieFootballGargantuarBlack : TowerDefenseZombieGargantuarBase
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

	private const string ZOMBIE_FOOTBALL_GARGANTUAR_HEAD2_REDEYE = "uid://ceopccduykwvs";

	private const string ZOMBIE_FOOTBALL_GARGANTUAR_R_BODY12 = "uid://pcqrhw4twpms";

	private const string ZOMBIE_FOOTBALL_GARGANTUAR_R_BODY13 = "uid://v14ui28vj0nv";

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (!(damagePointName == "Arm"))
		{
			if (damagePointName == "Head")
			{
				sprite.SetAtlasReplace("Zombie_football_gargantuar_head.png", "uid://ceopccduykwvs");
				sprite.SetAtlasReplace("Zombie_football_gargantuar_body1.png", "uid://v14ui28vj0nv");
			}
		}
		else
		{
			sprite.SetAtlasReplace("Zombie_football_gargantuar_body1.png", "uid://pcqrhw4twpms");
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
