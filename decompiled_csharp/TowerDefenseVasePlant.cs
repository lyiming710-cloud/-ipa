using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Vase/Plant/Scene/TowerDefenseVasePlant.cs")]
public class TowerDefenseVasePlant : TowerDefenseVase
{
	public new class MethodName : TowerDefenseVase.MethodName
	{
	}

	public new class PropertyName : TowerDefenseVase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseVase.SignalName
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
