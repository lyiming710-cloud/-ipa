using System.ComponentModel;
using Godot;
using Godot.Bridge;

[ScriptPath("res://Asset/Config/Map/CreekLawn/Scene/Day/TowerDefenseMapCreekLawnDay.cs")]
public class TowerDefenseMapCreekLawnDay : TowerDefenseMapShaderTime
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
