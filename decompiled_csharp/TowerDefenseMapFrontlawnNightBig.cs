using System.ComponentModel;
using Godot;
using Godot.Bridge;

[ScriptPath("res://Asset/Config/Map/Frontlawn/Scene/NightBig/TowerDefenseMapFrontlawnNightBig.cs")]
public class TowerDefenseMapFrontlawnNightBig : TowerDefenseMapRevealOnEnter
{
	public new class MethodName : TowerDefenseMapRevealOnEnter.MethodName
	{
	}

	public new class PropertyName : TowerDefenseMapRevealOnEnter.PropertyName
	{
	}

	public new class SignalName : TowerDefenseMapRevealOnEnter.SignalName
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
