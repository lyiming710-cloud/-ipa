using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Base/TowerDefenseMapRevealOnEnter.cs")]
public class TowerDefenseMapRevealOnEnter : TowerDefenseMap
{
	public new class MethodName : TowerDefenseMap.MethodName
	{
	}

	public new class PropertyName : TowerDefenseMap.PropertyName
	{
	}

	public new class SignalName : TowerDefenseMap.SignalName
	{
	}

	private static readonly string[] ConventionalRevealNodes = new string[5] { "FrontlawnFloor", "FrontlawnDoor", "BackyardFloor", "BackyardDoor", "UndeadVillageDoor" };

	public TowerDefenseMapRevealOnEnter()
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
