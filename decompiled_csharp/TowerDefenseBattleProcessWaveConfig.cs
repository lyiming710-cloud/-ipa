using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Wave/Resource/TowerDefenseBattleProcessWaveConfig.cs")]
public class TowerDefenseBattleProcessWaveConfig : TowerDefenseBattleProcessWaveEntryConfig
{
	public new class MethodName : TowerDefenseBattleProcessWaveEntryConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseBattleProcessWaveEntryConfig.PropertyName
	{
	}

	public new class SignalName : TowerDefenseBattleProcessWaveEntryConfig.SignalName
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
