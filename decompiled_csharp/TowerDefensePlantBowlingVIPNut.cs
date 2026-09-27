using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/VIPNut/Scene/TowerDefensePlantBowlingVIPNut.cs")]
public class TowerDefensePlantBowlingVIPNut : TowerDefensePlantBowlingBase
{
	public new class MethodName : TowerDefensePlantBowlingBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : TowerDefensePlantBowlingBase.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlantBowlingBase.SignalName
	{
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && config.customData != null)
		{
			Dictionary dictionary = (Dictionary)GameSaveManager.Instance.GetTowerDefensePacketValue("PlantVIPNut").GetValueOrDefault("Key", new Dictionary());
			if ((string)dictionary.GetValueOrDefault("Custom", "") != "")
			{
				currentCustom = new Array<string> { (string)dictionary["Custom"] };
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
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
