using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Mower/PoolCleaner/Scene/TowerDefenseMowerPoolCleaner.cs")]
public class TowerDefenseMowerPoolCleaner : TowerDefenseMower
{
	public new class MethodName : TowerDefenseMower.MethodName
	{
	}

	public new class PropertyName : TowerDefenseMower.PropertyName
	{
	}

	public new class SignalName : TowerDefenseMower.SignalName
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
