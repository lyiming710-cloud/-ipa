using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/EdgarResourceUidTool.cs")]
public class EdgarResourceUidTool : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static readonly string[] ResourcePaths = new string[6] { "res://Asset/Anime/Character/Zombie/Boss/EdgarII/ZombieBossEdgarII.tscn", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Effect/EdgarSPDown.tscn", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Config/TowerDefenseZombieBossEdgarII.tres", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Packet/ZombieBossEdgarII.tres", "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Scene/TowerDefenseZombieBossEdgarII.tscn", "res://Asset/Config/Projectile/EdgarII/ZombieBossEdgarIIFireball.tres" };

	public override async void _Ready()
	{
		ProjectResourceUidRepairResult characterRepair = await ProjectResourceUidRepairTool.RepairProjectAsync("res://Asset/Anime/Character/Zombie/Boss/EdgarII");
		ProjectResourceUidRepairResult projectResourceUidRepairResult = await ProjectResourceUidRepairTool.RepairProjectAsync("res://Asset/Config/Projectile/EdgarII");
		ProjectResourceUidRegistrationResult projectResourceUidRegistrationResult = ProjectResourceUidRepairTool.RegisterUidMappings(characterRepair.UidMappings);
		ProjectResourceUidRegistrationResult projectResourceUidRegistrationResult2 = ProjectResourceUidRepairTool.RegisterUidMappings(projectResourceUidRepairResult.UidMappings);
		bool flag = characterRepair.Errors == 0 && projectResourceUidRepairResult.Errors == 0 && projectResourceUidRegistrationResult.Errors == 0 && projectResourceUidRegistrationResult2.Errors == 0;
		string[] resourcePaths = ResourcePaths;
		foreach (string text in resourcePaths)
		{
			long resourceUid = ResourceLoader.GetResourceUid(text);
			string text2 = ((resourceUid == -1) ? "invalid" : ResourceUid.IdToText(resourceUid));
			if (resourceUid == -1)
			{
				flag = false;
			}
			GD.Print("EDGAR_UID path=" + text + " uid=" + text2);
		}
		GD.Print($"EDGAR_UID_RESULT passed={flag}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
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
