using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/HurtComponent/HurtComponentDefinition.cs")]
public class HurtComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName healthEffectScene = "healthEffectScene";

		public static readonly StringName maxHealthEffectCount = "maxHealthEffectCount";

		public static readonly StringName healthEffectAnimation = "healthEffectAnimation";

		public static readonly StringName flashOnDamage = "flashOnDamage";

		public static readonly StringName flashOnHeal = "flashOnHeal";

		public static readonly StringName markHealthBarDirty = "markHealthBarDirty";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene healthEffectScene;

	[Export(PropertyHint.Range, "-1,1000,1")]
	public int maxHealthEffectCount = 100;

	[Export(PropertyHint.None, "")]
	public StringName healthEffectAnimation = "Idle";

	[Export(PropertyHint.None, "")]
	public bool flashOnDamage = true;

	[Export(PropertyHint.None, "")]
	public bool flashOnHeal = true;

	[Export(PropertyHint.None, "")]
	public bool markHealthBarDirty = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new HurtComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.healthEffectScene)
		{
			healthEffectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.maxHealthEffectCount)
		{
			maxHealthEffectCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.healthEffectAnimation)
		{
			healthEffectAnimation = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.flashOnDamage)
		{
			flashOnDamage = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.flashOnHeal)
		{
			flashOnHeal = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.markHealthBarDirty)
		{
			markHealthBarDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.healthEffectScene)
		{
			value = VariantUtils.CreateFrom(in healthEffectScene);
			return true;
		}
		if (name == PropertyName.maxHealthEffectCount)
		{
			value = VariantUtils.CreateFrom(in maxHealthEffectCount);
			return true;
		}
		if (name == PropertyName.healthEffectAnimation)
		{
			value = VariantUtils.CreateFrom(in healthEffectAnimation);
			return true;
		}
		if (name == PropertyName.flashOnDamage)
		{
			value = VariantUtils.CreateFrom(in flashOnDamage);
			return true;
		}
		if (name == PropertyName.flashOnHeal)
		{
			value = VariantUtils.CreateFrom(in flashOnHeal);
			return true;
		}
		if (name == PropertyName.markHealthBarDirty)
		{
			value = VariantUtils.CreateFrom(in markHealthBarDirty);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.healthEffectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxHealthEffectCount, PropertyHint.Range, "-1,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.healthEffectAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.flashOnDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.flashOnHeal, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.markHealthBarDirty, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.healthEffectScene, Variant.From(in healthEffectScene));
		info.AddProperty(PropertyName.maxHealthEffectCount, Variant.From(in maxHealthEffectCount));
		info.AddProperty(PropertyName.healthEffectAnimation, Variant.From(in healthEffectAnimation));
		info.AddProperty(PropertyName.flashOnDamage, Variant.From(in flashOnDamage));
		info.AddProperty(PropertyName.flashOnHeal, Variant.From(in flashOnHeal));
		info.AddProperty(PropertyName.markHealthBarDirty, Variant.From(in markHealthBarDirty));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.healthEffectScene, out var value))
		{
			healthEffectScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.maxHealthEffectCount, out var value2))
		{
			maxHealthEffectCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.healthEffectAnimation, out var value3))
		{
			healthEffectAnimation = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.flashOnDamage, out var value4))
		{
			flashOnDamage = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.flashOnHeal, out var value5))
		{
			flashOnHeal = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.markHealthBarDirty, out var value6))
		{
			markHealthBarDirty = value6.As<bool>();
		}
	}
}
