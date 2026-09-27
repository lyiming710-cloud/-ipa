using System.ComponentModel;
using Godot.Bridge;

public class VaseExplosionDerivedChangeCost : TowerDefensePacketChangeCost
{
	public new class MethodName : TowerDefensePacketChangeCost.MethodName
	{
	}

	public new class PropertyName : TowerDefensePacketChangeCost.PropertyName
	{
	}

	public new class SignalName : TowerDefensePacketChangeCost.SignalName
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
