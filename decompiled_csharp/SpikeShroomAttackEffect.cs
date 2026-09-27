using System.ComponentModel;
using Godot;
using Godot.Bridge;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/SpikeShroom/AttackEffect/SpikeShroomAttackEffect.cs")]
public class SpikeShroomAttackEffect : AdobeAnimateSpriteBase
{
	public new class MethodName : AdobeAnimateSpriteBase.MethodName
	{
	}

	public new class PropertyName : AdobeAnimateSpriteBase.PropertyName
	{
	}

	public new class SignalName : AdobeAnimateSpriteBase.SignalName
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
