using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class PortalProjectileTransportPorTallnutStub : TowerDefensePlantPorTallnut
{
	public new class MethodName : TowerDefensePlantPorTallnut.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : TowerDefensePlantPorTallnut.PropertyName
	{
	}

	public new class SignalName : TowerDefensePlantPorTallnut.SignalName
	{
	}

	private const string PortalSpriteScenePath = "res://Asset/Anime/Character/Plant/Star/PorTallnut/PorTallnut.tscn";

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Disabled;
		instance = new TowerDefenseCharacterInstance
		{
			canBeCollection = false,
			hypnoses = false
		};
		open = true;
		AddToGroup("PorTallnut");
		sprite = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Star/PorTallnut/PorTallnut.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(sprite))
		{
			throw new InvalidOperationException("Could not instantiate res://Asset/Anime/Character/Plant/Star/PorTallnut/PorTallnut.tscn.");
		}
		AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
		OpenIdleEntered();
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
