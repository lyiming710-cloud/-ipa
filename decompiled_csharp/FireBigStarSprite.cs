using System.ComponentModel;
using Godot;
using Godot.Bridge;

[ScriptPath("res://Asset/Config/Projectile/Star/FireBigStar/FireBigStarSprite.cs")]
public class FireBigStarSprite : AdobeAnimateSpriteBase
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
