using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/GarlicComponent/GarlicComponentDefinition.cs")]
public class GarlicComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName grossoutTexture = "grossoutTexture";

		public static readonly StringName reactionDelay = "reactionDelay";

		public static readonly StringName grossoutDuration = "grossoutDuration";

		public static readonly StringName changeLineDuration = "changeLineDuration";

		public static readonly StringName moveDownChance = "moveDownChance";

		public static readonly StringName reactionAudio = "reactionAudio";

		public static readonly StringName garlicStateEvent = "garlicStateEvent";

		public static readonly StringName changeLineEase = "changeLineEase";

		public static readonly StringName changeLineTransition = "changeLineTransition";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	private const string DefaultGrossoutTexturePath = "uid://bbidqxovk4j7y";

	private static Texture2D _defaultGrossoutTextureCache;

	private static Texture2D DefaultGrossoutTexture => _defaultGrossoutTextureCache ?? (_defaultGrossoutTextureCache = GD.Load<Texture2D>("uid://bbidqxovk4j7y"));

	[Export(PropertyHint.None, "")]
	public Texture2D grossoutTexture { get; set; } = DefaultGrossoutTexture;

	[Export(PropertyHint.Range, "0,5,0.01")]
	public float reactionDelay { get; set; } = 0.5f;

	[Export(PropertyHint.Range, "0,5,0.01")]
	public float grossoutDuration { get; set; } = 0.5f;

	[Export(PropertyHint.Range, "0.01,10,0.01")]
	public float changeLineDuration { get; set; } = 1f;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public float moveDownChance { get; set; } = 0.5f;

	[Export(PropertyHint.None, "")]
	public string reactionAudio { get; set; } = "Yuck";

	[Export(PropertyHint.None, "")]
	public StringName garlicStateEvent { get; set; } = "ToGarlic";

	[Export(PropertyHint.None, "")]
	public Tween.EaseType changeLineEase { get; set; } = Tween.EaseType.InOut;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType changeLineTransition { get; set; }

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new GarlicComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.grossoutTexture)
		{
			grossoutTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.reactionDelay)
		{
			reactionDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.grossoutDuration)
		{
			grossoutDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.changeLineDuration)
		{
			changeLineDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.moveDownChance)
		{
			moveDownChance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.reactionAudio)
		{
			reactionAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.garlicStateEvent)
		{
			garlicStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.changeLineEase)
		{
			changeLineEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.changeLineTransition)
		{
			changeLineTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.grossoutTexture)
		{
			value = VariantUtils.CreateFrom<Texture2D>(grossoutTexture);
			return true;
		}
		float from;
		if (name == PropertyName.reactionDelay)
		{
			from = reactionDelay;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.grossoutDuration)
		{
			from = grossoutDuration;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.changeLineDuration)
		{
			from = changeLineDuration;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.moveDownChance)
		{
			from = moveDownChance;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.reactionAudio)
		{
			value = VariantUtils.CreateFrom<string>(reactionAudio);
			return true;
		}
		if (name == PropertyName.garlicStateEvent)
		{
			value = VariantUtils.CreateFrom<StringName>(garlicStateEvent);
			return true;
		}
		if (name == PropertyName.changeLineEase)
		{
			value = VariantUtils.CreateFrom<Tween.EaseType>(changeLineEase);
			return true;
		}
		if (name == PropertyName.changeLineTransition)
		{
			value = VariantUtils.CreateFrom<Tween.TransitionType>(changeLineTransition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.grossoutTexture, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.reactionDelay, PropertyHint.Range, "0,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.grossoutDuration, PropertyHint.Range, "0,5,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.changeLineDuration, PropertyHint.Range, "0.01,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.moveDownChance, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.reactionAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.garlicStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.changeLineEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.changeLineTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.grossoutTexture, Variant.From<Texture2D>(grossoutTexture));
		info.AddProperty(PropertyName.reactionDelay, Variant.From<float>(reactionDelay));
		info.AddProperty(PropertyName.grossoutDuration, Variant.From<float>(grossoutDuration));
		info.AddProperty(PropertyName.changeLineDuration, Variant.From<float>(changeLineDuration));
		info.AddProperty(PropertyName.moveDownChance, Variant.From<float>(moveDownChance));
		info.AddProperty(PropertyName.reactionAudio, Variant.From<string>(reactionAudio));
		info.AddProperty(PropertyName.garlicStateEvent, Variant.From<StringName>(garlicStateEvent));
		info.AddProperty(PropertyName.changeLineEase, Variant.From<Tween.EaseType>(changeLineEase));
		info.AddProperty(PropertyName.changeLineTransition, Variant.From<Tween.TransitionType>(changeLineTransition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.grossoutTexture, out var value))
		{
			grossoutTexture = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.reactionDelay, out var value2))
		{
			reactionDelay = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.grossoutDuration, out var value3))
		{
			grossoutDuration = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.changeLineDuration, out var value4))
		{
			changeLineDuration = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.moveDownChance, out var value5))
		{
			moveDownChance = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.reactionAudio, out var value6))
		{
			reactionAudio = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.garlicStateEvent, out var value7))
		{
			garlicStateEvent = value7.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.changeLineEase, out var value8))
		{
			changeLineEase = value8.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.changeLineTransition, out var value9))
		{
			changeLineTransition = value9.As<Tween.TransitionType>();
		}
	}
}
