using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/GravebusterComponent/GravebusterComponentDefinition.cs")]
public class GravebusterComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName landAnimeClips = "landAnimeClips";

		public static readonly StringName landAnimeTimeScale = "landAnimeTimeScale";

		public static readonly StringName gravebusterAnimeClips = "gravebusterAnimeClips";

		public static readonly StringName gravebusterTimeScale = "gravebusterTimeScale";

		public static readonly StringName requireGravestoneTarget = "requireGravestoneTarget";

		public static readonly StringName consumeDuration = "consumeDuration";

		public static readonly StringName landStartPositionY = "landStartPositionY";

		public static readonly StringName consumeEndPositionY = "consumeEndPositionY";

		public static readonly StringName discardStartOffset = "discardStartOffset";

		public static readonly StringName discardEndOffset = "discardEndOffset";

		public static readonly StringName discardResetPosition = "discardResetPosition";

		public static readonly StringName discardShaderParameter = "discardShaderParameter";

		public static readonly StringName consumeEase = "consumeEase";

		public static readonly StringName consumeTransition = "consumeTransition";

		public static readonly StringName consumeAudio = "consumeAudio";

		public static readonly StringName createDrop = "createDrop";

		public static readonly StringName dropFeatureName = "dropFeatureName";

		public static readonly StringName dropVelocityXRange = "dropVelocityXRange";

		public static readonly StringName dropVelocityY = "dropVelocityY";

		public static readonly StringName dropGravity = "dropGravity";

		public static readonly StringName gravebusterStateEvent = "gravebusterStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public string landAnimeClips { get; set; } = "Land";

	[Export(PropertyHint.None, "")]
	public float landAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string gravebusterAnimeClips { get; set; } = "Idle";

	[Export(PropertyHint.None, "")]
	public float gravebusterTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public bool requireGravestoneTarget { get; set; } = true;

	[Export(PropertyHint.Range, "0,30,0.01")]
	public float consumeDuration { get; set; } = 5f;

	[Export(PropertyHint.None, "")]
	public float landStartPositionY { get; set; } = -70f;

	[Export(PropertyHint.None, "")]
	public float consumeEndPositionY { get; set; } = -30f;

	[Export(PropertyHint.None, "")]
	public float discardStartOffset { get; set; } = -30f;

	[Export(PropertyHint.None, "")]
	public float discardEndOffset { get; set; }

	[Export(PropertyHint.None, "")]
	public float discardResetPosition { get; set; } = -10000f;

	[Export(PropertyHint.None, "")]
	public StringName discardShaderParameter { get; set; } = "discardUpPos";

	[Export(PropertyHint.None, "")]
	public Tween.EaseType consumeEase { get; set; } = Tween.EaseType.InOut;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType consumeTransition { get; set; }

	[ExportGroup("Audio And Drop", "")]
	[Export(PropertyHint.None, "")]
	public string consumeAudio { get; set; } = "GraveBusterChomp";

	[Export(PropertyHint.None, "")]
	public bool createDrop { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public string dropFeatureName { get; set; } = "Coins";

	[Export(PropertyHint.None, "")]
	public Vector2 dropVelocityXRange { get; set; } = new Vector2(-50f, 50f);

	[Export(PropertyHint.None, "")]
	public float dropVelocityY { get; set; } = -400f;

	[Export(PropertyHint.None, "")]
	public float dropGravity { get; set; } = 980f;

	[Export(PropertyHint.None, "")]
	public StringName gravebusterStateEvent { get; set; } = "ToGravebuster";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new GravebusterComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.landAnimeClips)
		{
			landAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.landAnimeTimeScale)
		{
			landAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.gravebusterAnimeClips)
		{
			gravebusterAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.gravebusterTimeScale)
		{
			gravebusterTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.requireGravestoneTarget)
		{
			requireGravestoneTarget = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.consumeDuration)
		{
			consumeDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.landStartPositionY)
		{
			landStartPositionY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.consumeEndPositionY)
		{
			consumeEndPositionY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardStartOffset)
		{
			discardStartOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardEndOffset)
		{
			discardEndOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			discardResetPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			discardShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.consumeEase)
		{
			consumeEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.consumeTransition)
		{
			consumeTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		if (name == PropertyName.consumeAudio)
		{
			consumeAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.createDrop)
		{
			createDrop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dropFeatureName)
		{
			dropFeatureName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dropVelocityXRange)
		{
			dropVelocityXRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.dropVelocityY)
		{
			dropVelocityY = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dropGravity)
		{
			dropGravity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.gravebusterStateEvent)
		{
			gravebusterStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom<NodePath>(spritePath);
			return true;
		}
		string from;
		if (name == PropertyName.landAnimeClips)
		{
			from = landAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from2;
		if (name == PropertyName.landAnimeTimeScale)
		{
			from2 = landAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.gravebusterAnimeClips)
		{
			from = gravebusterAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.gravebusterTimeScale)
		{
			from2 = gravebusterTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.requireGravestoneTarget)
		{
			from3 = requireGravestoneTarget;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.consumeDuration)
		{
			from2 = consumeDuration;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.landStartPositionY)
		{
			from2 = landStartPositionY;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.consumeEndPositionY)
		{
			from2 = consumeEndPositionY;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.discardStartOffset)
		{
			from2 = discardStartOffset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.discardEndOffset)
		{
			from2 = discardEndOffset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			from2 = discardResetPosition;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		StringName from4;
		if (name == PropertyName.discardShaderParameter)
		{
			from4 = discardShaderParameter;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.consumeEase)
		{
			value = VariantUtils.CreateFrom<Tween.EaseType>(consumeEase);
			return true;
		}
		if (name == PropertyName.consumeTransition)
		{
			value = VariantUtils.CreateFrom<Tween.TransitionType>(consumeTransition);
			return true;
		}
		if (name == PropertyName.consumeAudio)
		{
			from = consumeAudio;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.createDrop)
		{
			from3 = createDrop;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.dropFeatureName)
		{
			from = dropFeatureName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dropVelocityXRange)
		{
			value = VariantUtils.CreateFrom<Vector2>(dropVelocityXRange);
			return true;
		}
		if (name == PropertyName.dropVelocityY)
		{
			from2 = dropVelocityY;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.dropGravity)
		{
			from2 = dropGravity;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.gravebusterStateEvent)
		{
			from4 = gravebusterStateEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from4 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.landAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.landAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.gravebusterAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravebusterTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.requireGravestoneTarget, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.consumeDuration, PropertyHint.Range, "0,30,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.landStartPositionY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.consumeEndPositionY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardStartOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardEndOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardResetPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.discardShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.consumeEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.consumeTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Audio And Drop", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.consumeAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createDrop, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dropFeatureName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.dropVelocityXRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dropVelocityY, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dropGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.gravebusterStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.landAnimeClips, Variant.From<string>(landAnimeClips));
		info.AddProperty(PropertyName.landAnimeTimeScale, Variant.From<float>(landAnimeTimeScale));
		info.AddProperty(PropertyName.gravebusterAnimeClips, Variant.From<string>(gravebusterAnimeClips));
		info.AddProperty(PropertyName.gravebusterTimeScale, Variant.From<float>(gravebusterTimeScale));
		info.AddProperty(PropertyName.requireGravestoneTarget, Variant.From<bool>(requireGravestoneTarget));
		info.AddProperty(PropertyName.consumeDuration, Variant.From<float>(consumeDuration));
		info.AddProperty(PropertyName.landStartPositionY, Variant.From<float>(landStartPositionY));
		info.AddProperty(PropertyName.consumeEndPositionY, Variant.From<float>(consumeEndPositionY));
		info.AddProperty(PropertyName.discardStartOffset, Variant.From<float>(discardStartOffset));
		info.AddProperty(PropertyName.discardEndOffset, Variant.From<float>(discardEndOffset));
		info.AddProperty(PropertyName.discardResetPosition, Variant.From<float>(discardResetPosition));
		info.AddProperty(PropertyName.discardShaderParameter, Variant.From<StringName>(discardShaderParameter));
		info.AddProperty(PropertyName.consumeEase, Variant.From<Tween.EaseType>(consumeEase));
		info.AddProperty(PropertyName.consumeTransition, Variant.From<Tween.TransitionType>(consumeTransition));
		info.AddProperty(PropertyName.consumeAudio, Variant.From<string>(consumeAudio));
		info.AddProperty(PropertyName.createDrop, Variant.From<bool>(createDrop));
		info.AddProperty(PropertyName.dropFeatureName, Variant.From<string>(dropFeatureName));
		info.AddProperty(PropertyName.dropVelocityXRange, Variant.From<Vector2>(dropVelocityXRange));
		info.AddProperty(PropertyName.dropVelocityY, Variant.From<float>(dropVelocityY));
		info.AddProperty(PropertyName.dropGravity, Variant.From<float>(dropGravity));
		info.AddProperty(PropertyName.gravebusterStateEvent, Variant.From<StringName>(gravebusterStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spritePath, out var value))
		{
			spritePath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.landAnimeClips, out var value2))
		{
			landAnimeClips = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.landAnimeTimeScale, out var value3))
		{
			landAnimeTimeScale = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.gravebusterAnimeClips, out var value4))
		{
			gravebusterAnimeClips = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.gravebusterTimeScale, out var value5))
		{
			gravebusterTimeScale = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.requireGravestoneTarget, out var value6))
		{
			requireGravestoneTarget = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.consumeDuration, out var value7))
		{
			consumeDuration = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.landStartPositionY, out var value8))
		{
			landStartPositionY = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.consumeEndPositionY, out var value9))
		{
			consumeEndPositionY = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardStartOffset, out var value10))
		{
			discardStartOffset = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardEndOffset, out var value11))
		{
			discardEndOffset = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardResetPosition, out var value12))
		{
			discardResetPosition = value12.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardShaderParameter, out var value13))
		{
			discardShaderParameter = value13.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.consumeEase, out var value14))
		{
			consumeEase = value14.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.consumeTransition, out var value15))
		{
			consumeTransition = value15.As<Tween.TransitionType>();
		}
		if (info.TryGetProperty(PropertyName.consumeAudio, out var value16))
		{
			consumeAudio = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName.createDrop, out var value17))
		{
			createDrop = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dropFeatureName, out var value18))
		{
			dropFeatureName = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dropVelocityXRange, out var value19))
		{
			dropVelocityXRange = value19.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.dropVelocityY, out var value20))
		{
			dropVelocityY = value20.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dropGravity, out var value21))
		{
			dropGravity = value21.As<float>();
		}
		if (info.TryGetProperty(PropertyName.gravebusterStateEvent, out var value22))
		{
			gravebusterStateEvent = value22.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value23))
		{
			idleStateEvent = value23.As<StringName>();
		}
	}
}
