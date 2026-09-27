using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ResourceSpawnComponent/ResourceSpawnComponentDefinition.cs")]
public class ResourceSpawnComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName randomVelocityXRange = "randomVelocityXRange";

		public static readonly StringName defaultLaunchVelocityY = "defaultLaunchVelocityY";

		public static readonly StringName groundHeightMultiplier = "groundHeightMultiplier";

		public static readonly StringName maxEffectCount = "maxEffectCount";

		public static readonly StringName healthEffectScene = "healthEffectScene";

		public static readonly StringName healthEffectClip = "healthEffectClip";

		public static readonly StringName showHealthEffect = "showHealthEffect";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	public static readonly Vector2 DefaultRandomVelocityXRange = new Vector2(-50f, 50f);

	public const float DefaultLaunchVelocityY = -400f;

	public const float DefaultGroundHeightMultiplier = 2f;

	[Export(PropertyHint.None, "")]
	public Vector2 randomVelocityXRange = DefaultRandomVelocityXRange;

	[Export(PropertyHint.None, "")]
	public float defaultLaunchVelocityY = -400f;

	[Export(PropertyHint.Range, "0,10,0.01")]
	public float groundHeightMultiplier = 2f;

	[Export(PropertyHint.Range, "0,1000,1")]
	public int maxEffectCount = 100;

	[Export(PropertyHint.None, "")]
	public PackedScene healthEffectScene;

	[Export(PropertyHint.None, "")]
	public string healthEffectClip = "Idle";

	[Export(PropertyHint.None, "")]
	public bool showHealthEffect = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ResourceSpawnComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.randomVelocityXRange)
		{
			randomVelocityXRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.defaultLaunchVelocityY)
		{
			defaultLaunchVelocityY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.groundHeightMultiplier)
		{
			groundHeightMultiplier = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxEffectCount)
		{
			maxEffectCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.healthEffectScene)
		{
			healthEffectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.healthEffectClip)
		{
			healthEffectClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.showHealthEffect)
		{
			showHealthEffect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.randomVelocityXRange)
		{
			value = VariantUtils.CreateFrom(in randomVelocityXRange);
			return true;
		}
		if (name == PropertyName.defaultLaunchVelocityY)
		{
			value = VariantUtils.CreateFrom(in defaultLaunchVelocityY);
			return true;
		}
		if (name == PropertyName.groundHeightMultiplier)
		{
			value = VariantUtils.CreateFrom(in groundHeightMultiplier);
			return true;
		}
		if (name == PropertyName.maxEffectCount)
		{
			value = VariantUtils.CreateFrom(in maxEffectCount);
			return true;
		}
		if (name == PropertyName.healthEffectScene)
		{
			value = VariantUtils.CreateFrom(in healthEffectScene);
			return true;
		}
		if (name == PropertyName.healthEffectClip)
		{
			value = VariantUtils.CreateFrom(in healthEffectClip);
			return true;
		}
		if (name == PropertyName.showHealthEffect)
		{
			value = VariantUtils.CreateFrom(in showHealthEffect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName.randomVelocityXRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.defaultLaunchVelocityY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.groundHeightMultiplier, PropertyHint.Range, "0,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxEffectCount, PropertyHint.Range, "0,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.healthEffectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.healthEffectClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showHealthEffect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.randomVelocityXRange, Variant.From(in randomVelocityXRange));
		info.AddProperty(PropertyName.defaultLaunchVelocityY, Variant.From(in defaultLaunchVelocityY));
		info.AddProperty(PropertyName.groundHeightMultiplier, Variant.From(in groundHeightMultiplier));
		info.AddProperty(PropertyName.maxEffectCount, Variant.From(in maxEffectCount));
		info.AddProperty(PropertyName.healthEffectScene, Variant.From(in healthEffectScene));
		info.AddProperty(PropertyName.healthEffectClip, Variant.From(in healthEffectClip));
		info.AddProperty(PropertyName.showHealthEffect, Variant.From(in showHealthEffect));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.randomVelocityXRange, out var value))
		{
			randomVelocityXRange = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.defaultLaunchVelocityY, out var value2))
		{
			defaultLaunchVelocityY = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.groundHeightMultiplier, out var value3))
		{
			groundHeightMultiplier = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxEffectCount, out var value4))
		{
			maxEffectCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.healthEffectScene, out var value5))
		{
			healthEffectScene = value5.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.healthEffectClip, out var value6))
		{
			healthEffectClip = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.showHealthEffect, out var value7))
		{
			showHealthEffect = value7.As<bool>();
		}
	}
}
