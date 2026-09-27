using System.ComponentModel;
using Godot;
using Godot.Bridge;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Base/TowerDefenseMapShaderTime.cs")]
public class TowerDefenseMapShaderTime : TowerDefenseMap
{
	public new class MethodName : TowerDefenseMap.MethodName
	{
	}

	public new class PropertyName : TowerDefenseMap.PropertyName
	{
	}

	public new class SignalName : TowerDefenseMap.SignalName
	{
	}

	private static readonly string[] ConventionalShaderTimeNodes = new string[3] { "Pool", "Pool1", "Pool2" };

	public TowerDefenseMapShaderTime()
	{
		ConfigureShaderTimeNodes(ConventionalShaderTimeNodes);
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
