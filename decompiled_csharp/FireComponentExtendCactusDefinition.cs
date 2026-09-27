using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/ExtendComponent/FireComponentExtendCactus/FireComponentExtendCactusDefinition.cs")]
public class FireComponentExtendCactusDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName fireComponentInstanceId = "fireComponentInstanceId";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName upAnimeClips = "upAnimeClips";

		public static readonly StringName upAnimeTimeScale = "upAnimeTimeScale";

		public static readonly StringName downAnimeClips = "downAnimeClips";

		public static readonly StringName downAnimeTimeScale = "downAnimeTimeScale";

		public static readonly StringName upFireAnimeClips = "upFireAnimeClips";

		public static readonly StringName downFireAnimeClips = "downFireAnimeClips";

		public static readonly StringName transitionAnimationStartPosition = "transitionAnimationStartPosition";

		public static readonly StringName upStateEvent = "upStateEvent";

		public static readonly StringName downStateEvent = "downStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("References", "")]
	[Export(PropertyHint.None, "")]
	public string fireComponentInstanceId { get; set; } = "character.fire";

	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string upAnimeClips { get; set; } = "Up";

	[Export(PropertyHint.None, "")]
	public float upAnimeTimeScale { get; set; } = 2f;

	[Export(PropertyHint.None, "")]
	public string downAnimeClips { get; set; } = "Down";

	[Export(PropertyHint.None, "")]
	public float downAnimeTimeScale { get; set; } = 2f;

	[Export(PropertyHint.None, "")]
	public string upFireAnimeClips { get; set; } = "UpFire";

	[Export(PropertyHint.None, "")]
	public string downFireAnimeClips { get; set; } = "Fire";

	[Export(PropertyHint.None, "")]
	public float transitionAnimationStartPosition { get; set; } = 0.2f;

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName upStateEvent { get; set; } = "ToUp";

	[Export(PropertyHint.None, "")]
	public StringName downStateEvent { get; set; } = "ToDown";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new FireComponentExtendCactus();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.fireComponentInstanceId)
		{
			fireComponentInstanceId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.upAnimeClips)
		{
			upAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.upAnimeTimeScale)
		{
			upAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.downAnimeClips)
		{
			downAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.downAnimeTimeScale)
		{
			downAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.upFireAnimeClips)
		{
			upFireAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.downFireAnimeClips)
		{
			downFireAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.transitionAnimationStartPosition)
		{
			transitionAnimationStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.upStateEvent)
		{
			upStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.downStateEvent)
		{
			downStateEvent = VariantUtils.ConvertTo<StringName>(in value);
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
		string from;
		if (name == PropertyName.fireComponentInstanceId)
		{
			from = fireComponentInstanceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom<NodePath>(spritePath);
			return true;
		}
		if (name == PropertyName.upAnimeClips)
		{
			from = upAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from2;
		if (name == PropertyName.upAnimeTimeScale)
		{
			from2 = upAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.downAnimeClips)
		{
			from = downAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.downAnimeTimeScale)
		{
			from2 = downAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.upFireAnimeClips)
		{
			from = upFireAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.downFireAnimeClips)
		{
			from = downFireAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.transitionAnimationStartPosition)
		{
			from2 = transitionAnimationStartPosition;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		StringName from3;
		if (name == PropertyName.upStateEvent)
		{
			from3 = upStateEvent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.downStateEvent)
		{
			from3 = downStateEvent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from3 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.fireComponentInstanceId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.upAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.upAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.downAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.downAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.upFireAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.downFireAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.transitionAnimationStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.upStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.downStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireComponentInstanceId, Variant.From<string>(fireComponentInstanceId));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.upAnimeClips, Variant.From<string>(upAnimeClips));
		info.AddProperty(PropertyName.upAnimeTimeScale, Variant.From<float>(upAnimeTimeScale));
		info.AddProperty(PropertyName.downAnimeClips, Variant.From<string>(downAnimeClips));
		info.AddProperty(PropertyName.downAnimeTimeScale, Variant.From<float>(downAnimeTimeScale));
		info.AddProperty(PropertyName.upFireAnimeClips, Variant.From<string>(upFireAnimeClips));
		info.AddProperty(PropertyName.downFireAnimeClips, Variant.From<string>(downFireAnimeClips));
		info.AddProperty(PropertyName.transitionAnimationStartPosition, Variant.From<float>(transitionAnimationStartPosition));
		info.AddProperty(PropertyName.upStateEvent, Variant.From<StringName>(upStateEvent));
		info.AddProperty(PropertyName.downStateEvent, Variant.From<StringName>(downStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireComponentInstanceId, out var value))
		{
			fireComponentInstanceId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value2))
		{
			spritePath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.upAnimeClips, out var value3))
		{
			upAnimeClips = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.upAnimeTimeScale, out var value4))
		{
			upAnimeTimeScale = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.downAnimeClips, out var value5))
		{
			downAnimeClips = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.downAnimeTimeScale, out var value6))
		{
			downAnimeTimeScale = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.upFireAnimeClips, out var value7))
		{
			upFireAnimeClips = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.downFireAnimeClips, out var value8))
		{
			downFireAnimeClips = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.transitionAnimationStartPosition, out var value9))
		{
			transitionAnimationStartPosition = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.upStateEvent, out var value10))
		{
			upStateEvent = value10.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.downStateEvent, out var value11))
		{
			downStateEvent = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value12))
		{
			idleStateEvent = value12.As<StringName>();
		}
	}
}
