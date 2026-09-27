using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/CardBehaviorDefinition.cs")]
public abstract class CardBehaviorDefinition : BehaviorDefinitionBase
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

	public CardBehaviorDefinition()
	{
		BehaviorTypeId = new StringName("card");
	}

	public virtual CardBehaviorRuntime CreateRuntime()
	{
		return null;
	}

	public virtual void ModifyCost(ref CardCostContext context)
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
