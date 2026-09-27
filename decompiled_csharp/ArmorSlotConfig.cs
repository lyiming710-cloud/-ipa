using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/General/Character/Armor/ArmorSlotConfig.cs")]
public class ArmorSlotConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName openFliter = "openFliter";

		public static readonly StringName closeFliter = "closeFliter";

		public static readonly StringName destroyFliter = "destroyFliter";

		public static readonly StringName armorName = "armorName";

		public static readonly StringName replaceMethod = "replaceMethod";

		public static readonly StringName replaceMediaName = "replaceMediaName";

		public static readonly StringName slotPath = "slotPath";

		public static readonly StringName offset = "offset";

		public static readonly StringName rotation = "rotation";

		public static readonly StringName scale = "scale";

		public static readonly StringName alphaMultiplier = "alphaMultiplier";

		public static readonly StringName _openFliter = "_openFliter";

		public static readonly StringName _closeFliter = "_closeFliter";

		public static readonly StringName _destroyFliter = "_destroyFliter";

		public static readonly StringName damagePoint = "damagePoint";

		public static readonly StringName behaviorIds = "behaviorIds";

		public static readonly StringName behaviors = "behaviors";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string armorName = "";

	[Export(PropertyHint.Enum, "Media,Sprite")]
	public string replaceMethod = "Media";

	[Export(PropertyHint.None, "")]
	public StringName replaceMediaName = "";

	[Export(PropertyHint.None, "")]
	public NodePath slotPath = new NodePath();

	[Export(PropertyHint.None, "")]
	public Vector2 offset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public double rotation;

	[Export(PropertyHint.None, "")]
	public Vector2 scale = Vector2.One;

	[Export(PropertyHint.Range, "0,2,0.05,or_greater")]
	public float alphaMultiplier = 1f;

	private string _openFliter = "";

	private string _closeFliter = "";

	private string _destroyFliter = "";

	[Export(PropertyHint.None, "")]
	public double damagePoint = -1.0;

	[ExportCategory("Behavior")]
	[Export(PropertyHint.None, "")]
	public Array<StringName> behaviorIds = new Array<StringName>();

	[Export(PropertyHint.None, "")]
	public Array<ArmorBehaviorDefinition> behaviors = new Array<ArmorBehaviorDefinition>();

	[Export(PropertyHint.MultilineText, "")]
	public string openFliter
	{
		get
		{
			return _openFliter;
		}
		set
		{
			_openFliter = value;
			EmitChanged();
		}
	}

	[Export(PropertyHint.MultilineText, "")]
	public string closeFliter
	{
		get
		{
			return _closeFliter;
		}
		set
		{
			_closeFliter = value;
			EmitChanged();
		}
	}

	[Export(PropertyHint.MultilineText, "")]
	public string destroyFliter
	{
		get
		{
			return _destroyFliter;
		}
		set
		{
			_destroyFliter = value;
			EmitChanged();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.openFliter)
		{
			openFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.closeFliter)
		{
			closeFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.destroyFliter)
		{
			destroyFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.armorName)
		{
			armorName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.replaceMethod)
		{
			replaceMethod = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.replaceMediaName)
		{
			replaceMediaName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.slotPath)
		{
			slotPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.offset)
		{
			offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.rotation)
		{
			rotation = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.scale)
		{
			scale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.alphaMultiplier)
		{
			alphaMultiplier = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._openFliter)
		{
			_openFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._closeFliter)
		{
			_closeFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._destroyFliter)
		{
			_destroyFliter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.damagePoint)
		{
			damagePoint = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			behaviorIds = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			behaviors = VariantUtils.ConvertToArray<ArmorBehaviorDefinition>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.openFliter)
		{
			from = openFliter;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.closeFliter)
		{
			from = closeFliter;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.destroyFliter)
		{
			from = destroyFliter;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.armorName)
		{
			value = VariantUtils.CreateFrom(in armorName);
			return true;
		}
		if (name == PropertyName.replaceMethod)
		{
			value = VariantUtils.CreateFrom(in replaceMethod);
			return true;
		}
		if (name == PropertyName.replaceMediaName)
		{
			value = VariantUtils.CreateFrom(in replaceMediaName);
			return true;
		}
		if (name == PropertyName.slotPath)
		{
			value = VariantUtils.CreateFrom(in slotPath);
			return true;
		}
		if (name == PropertyName.offset)
		{
			value = VariantUtils.CreateFrom(in offset);
			return true;
		}
		if (name == PropertyName.rotation)
		{
			value = VariantUtils.CreateFrom(in rotation);
			return true;
		}
		if (name == PropertyName.scale)
		{
			value = VariantUtils.CreateFrom(in scale);
			return true;
		}
		if (name == PropertyName.alphaMultiplier)
		{
			value = VariantUtils.CreateFrom(in alphaMultiplier);
			return true;
		}
		if (name == PropertyName._openFliter)
		{
			value = VariantUtils.CreateFrom(in _openFliter);
			return true;
		}
		if (name == PropertyName._closeFliter)
		{
			value = VariantUtils.CreateFrom(in _closeFliter);
			return true;
		}
		if (name == PropertyName._destroyFliter)
		{
			value = VariantUtils.CreateFrom(in _destroyFliter);
			return true;
		}
		if (name == PropertyName.damagePoint)
		{
			value = VariantUtils.CreateFrom(in damagePoint);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			value = VariantUtils.CreateFromArray(behaviorIds);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			value = VariantUtils.CreateFromArray(behaviors);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.armorName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.replaceMethod, PropertyHint.Enum, "Media,Sprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.replaceMediaName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.slotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rotation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.alphaMultiplier, PropertyHint.Range, "0,2,0.05,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._openFliter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._closeFliter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._destroyFliter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.openFliter, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.closeFliter, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.destroyFliter, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damagePoint, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Behavior", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviorIds, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviors, PropertyHint.TypeString, "24/17:ArmorBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.openFliter, Variant.From<string>(openFliter));
		info.AddProperty(PropertyName.closeFliter, Variant.From<string>(closeFliter));
		info.AddProperty(PropertyName.destroyFliter, Variant.From<string>(destroyFliter));
		info.AddProperty(PropertyName.armorName, Variant.From(in armorName));
		info.AddProperty(PropertyName.replaceMethod, Variant.From(in replaceMethod));
		info.AddProperty(PropertyName.replaceMediaName, Variant.From(in replaceMediaName));
		info.AddProperty(PropertyName.slotPath, Variant.From(in slotPath));
		info.AddProperty(PropertyName.offset, Variant.From(in offset));
		info.AddProperty(PropertyName.rotation, Variant.From(in rotation));
		info.AddProperty(PropertyName.scale, Variant.From(in scale));
		info.AddProperty(PropertyName.alphaMultiplier, Variant.From(in alphaMultiplier));
		info.AddProperty(PropertyName._openFliter, Variant.From(in _openFliter));
		info.AddProperty(PropertyName._closeFliter, Variant.From(in _closeFliter));
		info.AddProperty(PropertyName._destroyFliter, Variant.From(in _destroyFliter));
		info.AddProperty(PropertyName.damagePoint, Variant.From(in damagePoint));
		info.AddProperty(PropertyName.behaviorIds, Variant.CreateFrom(behaviorIds));
		info.AddProperty(PropertyName.behaviors, Variant.CreateFrom(behaviors));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.openFliter, out var value))
		{
			openFliter = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.closeFliter, out var value2))
		{
			closeFliter = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.destroyFliter, out var value3))
		{
			destroyFliter = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.armorName, out var value4))
		{
			armorName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.replaceMethod, out var value5))
		{
			replaceMethod = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.replaceMediaName, out var value6))
		{
			replaceMediaName = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.slotPath, out var value7))
		{
			slotPath = value7.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.offset, out var value8))
		{
			offset = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.rotation, out var value9))
		{
			rotation = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.scale, out var value10))
		{
			scale = value10.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.alphaMultiplier, out var value11))
		{
			alphaMultiplier = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName._openFliter, out var value12))
		{
			_openFliter = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName._closeFliter, out var value13))
		{
			_closeFliter = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName._destroyFliter, out var value14))
		{
			_destroyFliter = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.damagePoint, out var value15))
		{
			damagePoint = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.behaviorIds, out var value16))
		{
			behaviorIds = value16.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.behaviors, out var value17))
		{
			behaviors = value17.AsGodotArray<ArmorBehaviorDefinition>();
		}
	}
}
