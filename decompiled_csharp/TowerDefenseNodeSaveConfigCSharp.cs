using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Save/Node/TowerDefenseNodeSaveConfigCSharp.cs")]
public class TowerDefenseNodeSaveConfigCSharp : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
	}

	public new class SignalName : Resource.SignalName
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
