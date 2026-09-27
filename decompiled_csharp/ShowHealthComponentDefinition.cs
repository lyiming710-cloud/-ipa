using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ShowHealthComponent/ShowHealthComponentDefinition.cs")]
public class ShowHealthComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName viewScene = "viewScene";

		public static readonly StringName textTemplate = "textTemplate";

		public static readonly StringName decimalPlaces = "decimalPlaces";

		public static readonly StringName roundHitpoints = "roundHitpoints";

		public static readonly StringName showBody = "showBody";

		public static readonly StringName showSecondaryArmor = "showSecondaryArmor";

		public static readonly StringName showHelmet = "showHelmet";

		public static readonly StringName secondaryArmorPriority = "secondaryArmorPriority";

		public static readonly StringName keepScreenAligned = "keepScreenAligned";

		public static readonly StringName displayZIndex = "displayZIndex";

		public static readonly StringName shieldColor = "shieldColor";

		public static readonly StringName helmetColor = "helmetColor";

		public static readonly StringName bodyColor = "bodyColor";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene viewScene;

	[Export(PropertyHint.None, "")]
	public string textTemplate = "HP:{0}/{1}";

	[Export(PropertyHint.Range, "0,3,1")]
	public int decimalPlaces;

	[Export(PropertyHint.None, "")]
	public bool roundHitpoints = true;

	[Export(PropertyHint.None, "")]
	public bool showBody = true;

	[Export(PropertyHint.None, "")]
	public bool showSecondaryArmor = true;

	[Export(PropertyHint.None, "")]
	public bool showHelmet = true;

	[Export(PropertyHint.None, "")]
	public ShowHealthComponent.SecondaryArmorPriority secondaryArmorPriority;

	[Export(PropertyHint.None, "")]
	public bool keepScreenAligned = true;

	[Export(PropertyHint.None, "")]
	public int displayZIndex = 10;

	[Export(PropertyHint.None, "")]
	public Color shieldColor = new Color(0f, 0.992157f, 1f);

	[Export(PropertyHint.None, "")]
	public Color helmetColor = Colors.Yellow;

	[Export(PropertyHint.None, "")]
	public Color bodyColor = Colors.Red;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ShowHealthComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.viewScene)
		{
			viewScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.textTemplate)
		{
			textTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.decimalPlaces)
		{
			decimalPlaces = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.roundHitpoints)
		{
			roundHitpoints = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showBody)
		{
			showBody = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showSecondaryArmor)
		{
			showSecondaryArmor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showHelmet)
		{
			showHelmet = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.secondaryArmorPriority)
		{
			secondaryArmorPriority = VariantUtils.ConvertTo<ShowHealthComponent.SecondaryArmorPriority>(in value);
			return true;
		}
		if (name == PropertyName.keepScreenAligned)
		{
			keepScreenAligned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.displayZIndex)
		{
			displayZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.shieldColor)
		{
			shieldColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.helmetColor)
		{
			helmetColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.bodyColor)
		{
			bodyColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.viewScene)
		{
			value = VariantUtils.CreateFrom(in viewScene);
			return true;
		}
		if (name == PropertyName.textTemplate)
		{
			value = VariantUtils.CreateFrom(in textTemplate);
			return true;
		}
		if (name == PropertyName.decimalPlaces)
		{
			value = VariantUtils.CreateFrom(in decimalPlaces);
			return true;
		}
		if (name == PropertyName.roundHitpoints)
		{
			value = VariantUtils.CreateFrom(in roundHitpoints);
			return true;
		}
		if (name == PropertyName.showBody)
		{
			value = VariantUtils.CreateFrom(in showBody);
			return true;
		}
		if (name == PropertyName.showSecondaryArmor)
		{
			value = VariantUtils.CreateFrom(in showSecondaryArmor);
			return true;
		}
		if (name == PropertyName.showHelmet)
		{
			value = VariantUtils.CreateFrom(in showHelmet);
			return true;
		}
		if (name == PropertyName.secondaryArmorPriority)
		{
			value = VariantUtils.CreateFrom(in secondaryArmorPriority);
			return true;
		}
		if (name == PropertyName.keepScreenAligned)
		{
			value = VariantUtils.CreateFrom(in keepScreenAligned);
			return true;
		}
		if (name == PropertyName.displayZIndex)
		{
			value = VariantUtils.CreateFrom(in displayZIndex);
			return true;
		}
		if (name == PropertyName.shieldColor)
		{
			value = VariantUtils.CreateFrom(in shieldColor);
			return true;
		}
		if (name == PropertyName.helmetColor)
		{
			value = VariantUtils.CreateFrom(in helmetColor);
			return true;
		}
		if (name == PropertyName.bodyColor)
		{
			value = VariantUtils.CreateFrom(in bodyColor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.viewScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.textTemplate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.decimalPlaces, PropertyHint.Range, "0,3,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.roundHitpoints, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showBody, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showSecondaryArmor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showHelmet, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.secondaryArmorPriority, PropertyHint.Enum, "ShieldFirst,HeadCoverFirst", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.keepScreenAligned, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.displayZIndex, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.shieldColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.helmetColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.bodyColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.viewScene, Variant.From(in viewScene));
		info.AddProperty(PropertyName.textTemplate, Variant.From(in textTemplate));
		info.AddProperty(PropertyName.decimalPlaces, Variant.From(in decimalPlaces));
		info.AddProperty(PropertyName.roundHitpoints, Variant.From(in roundHitpoints));
		info.AddProperty(PropertyName.showBody, Variant.From(in showBody));
		info.AddProperty(PropertyName.showSecondaryArmor, Variant.From(in showSecondaryArmor));
		info.AddProperty(PropertyName.showHelmet, Variant.From(in showHelmet));
		info.AddProperty(PropertyName.secondaryArmorPriority, Variant.From(in secondaryArmorPriority));
		info.AddProperty(PropertyName.keepScreenAligned, Variant.From(in keepScreenAligned));
		info.AddProperty(PropertyName.displayZIndex, Variant.From(in displayZIndex));
		info.AddProperty(PropertyName.shieldColor, Variant.From(in shieldColor));
		info.AddProperty(PropertyName.helmetColor, Variant.From(in helmetColor));
		info.AddProperty(PropertyName.bodyColor, Variant.From(in bodyColor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.viewScene, out var value))
		{
			viewScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.textTemplate, out var value2))
		{
			textTemplate = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.decimalPlaces, out var value3))
		{
			decimalPlaces = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.roundHitpoints, out var value4))
		{
			roundHitpoints = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showBody, out var value5))
		{
			showBody = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showSecondaryArmor, out var value6))
		{
			showSecondaryArmor = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showHelmet, out var value7))
		{
			showHelmet = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.secondaryArmorPriority, out var value8))
		{
			secondaryArmorPriority = value8.As<ShowHealthComponent.SecondaryArmorPriority>();
		}
		if (info.TryGetProperty(PropertyName.keepScreenAligned, out var value9))
		{
			keepScreenAligned = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.displayZIndex, out var value10))
		{
			displayZIndex = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.shieldColor, out var value11))
		{
			shieldColor = value11.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.helmetColor, out var value12))
		{
			helmetColor = value12.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.bodyColor, out var value13))
		{
			bodyColor = value13.As<Color>();
		}
	}
}
