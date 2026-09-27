using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter0/Gravebuster/Scene/TowerDefensePlantGravebuster.cs")]
public class TowerDefensePlantGravebuster : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlant.SignalName
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
