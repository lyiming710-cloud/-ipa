using System.ComponentModel;
using Godot;
using Godot.Bridge;

[ScriptPath("res://Asset/Config/Map/OutsideBackyard/Scene/Night/TowerDefenseMapOutsideBackyardNight.cs")]
public class TowerDefenseMapOutsideBackyardNight : TowerDefenseMapRevealShaderTime
{
	public new class MethodName : TowerDefenseMapRevealShaderTime.MethodName
	{
	}

	public new class PropertyName : TowerDefenseMapRevealShaderTime.PropertyName
	{
	}

	public new class SignalName : TowerDefenseMapRevealShaderTime.SignalName
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
