using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Base/TowerDefenseMapRevealShaderTime.cs")]
public class TowerDefenseMapRevealShaderTime : TowerDefenseMapShaderTime
{
	public new class MethodName : TowerDefenseMapShaderTime.MethodName
	{
	}

	public new class PropertyName : TowerDefenseMapShaderTime.PropertyName
	{
	}

	public new class SignalName : TowerDefenseMapShaderTime.SignalName
	{
	}

	private static readonly string[] ConventionalRevealNodes = new string[5] { "FrontlawnFloor", "FrontlawnDoor", "BackyardFloor", "BackyardDoor", "UndeadVillageDoor" };

	public TowerDefenseMapRevealShaderTime()
	{
		ConfigureEnterRoomRevealNodes(ConventionalRevealNodes);
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
