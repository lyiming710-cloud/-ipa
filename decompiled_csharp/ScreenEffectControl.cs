using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/ScreenEffect/ScreenEffectControl/ScreenEffectControl.cs")]
public class ScreenEffectControl : Control
{
	public new class MethodName : Control.MethodName
	{
	}

	public new class PropertyName : Control.PropertyName
	{
	}

	public new class SignalName : Control.SignalName
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
