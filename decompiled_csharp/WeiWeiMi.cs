using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Prefab/Npc/WeiWeiMi/WeiWeiMi.cs")]
public class WeiWeiMi : NpcBase
{
	public new class MethodName : NpcBase.MethodName
	{
	}

	public new class PropertyName : NpcBase.PropertyName
	{
	}

	public new class SignalName : NpcBase.SignalName
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
