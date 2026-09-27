using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Behavior/Armor/ArmorBehaviorDefinition.cs")]
public abstract class ArmorBehaviorDefinition : BehaviorDefinitionBase
{
	public new class MethodName : BehaviorDefinitionBase.MethodName
	{
	}

	public new class PropertyName : BehaviorDefinitionBase.PropertyName
	{
	}

	public new class SignalName : BehaviorDefinitionBase.SignalName
	{
	}

	public ArmorBehaviorDefinition()
	{
		BehaviorTypeId = new StringName("armor");
	}

	public abstract ArmorBehaviorRuntime CreateRuntime();

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
