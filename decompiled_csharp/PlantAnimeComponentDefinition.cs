using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/PlantAnimeComponent/PlantAnimeComponentDefinition.cs")]
public class PlantAnimeComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName fallbackToIdle = "fallbackToIdle";

		public static readonly StringName syncSpriteTimeScale = "syncSpriteTimeScale";

		public static readonly StringName loopAnimation = "loopAnimation";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool fallbackToIdle = true;

	[Export(PropertyHint.None, "")]
	public bool syncSpriteTimeScale = true;

	[Export(PropertyHint.None, "")]
	public bool loopAnimation;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new PlantAnimeComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.fallbackToIdle)
		{
			fallbackToIdle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.syncSpriteTimeScale)
		{
			syncSpriteTimeScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.loopAnimation)
		{
			loopAnimation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fallbackToIdle)
		{
			value = VariantUtils.CreateFrom(in fallbackToIdle);
			return true;
		}
		if (name == PropertyName.syncSpriteTimeScale)
		{
			value = VariantUtils.CreateFrom(in syncSpriteTimeScale);
			return true;
		}
		if (name == PropertyName.loopAnimation)
		{
			value = VariantUtils.CreateFrom(in loopAnimation);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.fallbackToIdle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.syncSpriteTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.loopAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fallbackToIdle, Variant.From(in fallbackToIdle));
		info.AddProperty(PropertyName.syncSpriteTimeScale, Variant.From(in syncSpriteTimeScale));
		info.AddProperty(PropertyName.loopAnimation, Variant.From(in loopAnimation));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fallbackToIdle, out var value))
		{
			fallbackToIdle = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.syncSpriteTimeScale, out var value2))
		{
			syncSpriteTimeScale = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.loopAnimation, out var value3))
		{
			loopAnimation = value3.As<bool>();
		}
	}
}
