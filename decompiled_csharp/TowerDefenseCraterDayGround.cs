using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Crater/CraterDayGround/Scene/TowerDefenseCraterDayGround.cs")]
public class TowerDefenseCraterDayGround : TowerDefenseCrater
{
	public new class MethodName : TowerDefenseCrater.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCrater.PropertyName
	{
	}

	public new class SignalName : TowerDefenseCrater.SignalName
	{
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
