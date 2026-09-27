using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Prefab/Particles/Splats/GloomSplats/GloomSplatsPartcles.cs")]
public class GloomSplatsPartcles : GPUParticles2DOnece
{
	public new class MethodName : GPUParticles2DOnece.MethodName
	{
	}

	public new class PropertyName : GPUParticles2DOnece.PropertyName
	{
	}

	public new class SignalName : GPUParticles2DOnece.SignalName
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
