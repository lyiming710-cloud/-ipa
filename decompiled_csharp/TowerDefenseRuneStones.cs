using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/RuneStones/Scene/TowerDefenseRuneStones.cs")]
public class TowerDefenseRuneStones : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DamagePointReach = "DamagePointReach";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private const string RUNE_STONES_WATER_1 = "uid://drim0rxao2i1e";

	private const string RUNE_STONES_WATER_2 = "uid://bp8idfpt8pjls";

	private const string RUNE_STONES_WATER_3 = "uid://c4fmainbpf3b7";

	private const string RUNE_STONES_WATER_4 = "uid://cnjnq72j50hji";

	private const string RUNE_STONES_WATER_5 = "uid://daebeitjtpwfk";

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			RemoveFromGroup("Gravestone");
			if (GodotObject.IsInstanceValid(cell) && cell.isWater)
			{
				shadowSprite.Visible = false;
				sprite.SetAtlasReplace("RuneStones3_1.png", "uid://drim0rxao2i1e");
			}
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (GodotObject.IsInstanceValid(cell) && cell.isWater)
		{
			switch (damagePointName)
			{
			case "Damage0":
				sprite.SetAtlasReplace("RuneStones3_1.png", "uid://drim0rxao2i1e");
				break;
			case "Damage1":
				sprite.SetAtlasReplace("RuneStones3_1.png", "uid://bp8idfpt8pjls");
				break;
			case "Damage2":
				sprite.SetAtlasReplace("RuneStones3_1.png", "uid://c4fmainbpf3b7");
				break;
			case "Damage3":
				sprite.SetAtlasReplace("RuneStones3_1.png", "uid://cnjnq72j50hji");
				break;
			case "Damage4":
				sprite.SetAtlasReplace("RuneStones3_1.png", "uid://daebeitjtpwfk");
				break;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Ready)
		{
			return true;
		}
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
