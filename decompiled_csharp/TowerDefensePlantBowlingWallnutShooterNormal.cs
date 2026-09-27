using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/WallnutShooter/Scene/BowlingNormal/TowerDefensePlantBowlingWallnutShooterNormal.cs")]
public class TowerDefensePlantBowlingWallnutShooterNormal : TowerDefensePlantBowlingBase
{
	public new class MethodName : TowerDefensePlantBowlingBase.MethodName
	{
	}

	public new class PropertyName : TowerDefensePlantBowlingBase.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlantBowlingBase.SignalName
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
