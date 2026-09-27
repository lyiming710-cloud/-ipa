using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ArmorVisualComponent/ArmorVisualComponentDefinition.cs")]
public class ArmorVisualComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ArmorVisualComponent();
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
