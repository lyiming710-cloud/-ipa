using System.ComponentModel;
using Godot.Bridge;

public class VaseExplosionDerivedPacketEvent : CardActionBehaviorDefinition
{
	public new class MethodName : CardActionBehaviorDefinition.MethodName
	{
	}

	public new class PropertyName : CardActionBehaviorDefinition.PropertyName
	{
	}

	public new class SignalName : CardActionBehaviorDefinition.SignalName
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
