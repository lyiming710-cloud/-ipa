using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/TanglekelpComponent/TanglekelpComponentDefinition.cs")]
public class TanglekelpComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName attackComponentName = "attackComponentName";

		public static readonly StringName grabNum = "grabNum";

		public static readonly StringName destroyUse = "destroyUse";

		public static readonly StringName targetDestroyUse = "targetDestroyUse";

		public static readonly StringName dragDelay = "dragDelay";

		public static readonly StringName dragAudio = "dragAudio";

		public static readonly StringName entrySplashAudio = "entrySplashAudio";

		public static readonly StringName submergeAudio = "submergeAudio";

		public static readonly StringName createTargetSplash = "createTargetSplash";

		public static readonly StringName createParentSplash = "createParentSplash";

		public static readonly StringName dragStateEvent = "dragStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName grabSpriteScene = "grabSpriteScene";

		public static readonly StringName grabAnimeClips = "grabAnimeClips";

		public static readonly StringName grabAnimeTimeScale = "grabAnimeTimeScale";

		public static readonly StringName grabFliterOpen = "grabFliterOpen";

		public static readonly StringName grabFliterClose = "grabFliterClose";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportSubgroup("Dependencies", "")]
	[Export(PropertyHint.None, "")]
	public StringName attackComponentName { get; set; } = "AttackComponent";

	[ExportSubgroup("Drag", "")]
	[Export(PropertyHint.Range, "1,100,1")]
	public int grabNum { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public bool destroyUse { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool targetDestroyUse { get; set; } = true;

	[Export(PropertyHint.Range, "0,10,0.01")]
	public float dragDelay { get; set; } = 0.5f;

	[Export(PropertyHint.None, "")]
	public string dragAudio { get; set; } = "Floop";

	[Export(PropertyHint.None, "")]
	public string entrySplashAudio { get; set; } = "PlantWater";

	[Export(PropertyHint.None, "")]
	public string submergeAudio { get; set; } = "ZombieEnteringWater";

	[Export(PropertyHint.None, "")]
	public bool createTargetSplash { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool createParentSplash { get; set; } = true;

	[ExportSubgroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName dragStateEvent { get; set; } = "ToDrag";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[ExportSubgroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public PackedScene grabSpriteScene { get; set; }

	[Export(PropertyHint.None, "")]
	public string grabAnimeClips { get; set; } = "Grab";

	[Export(PropertyHint.Range, "0.01,10,0.01")]
	public float grabAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public Array<string> grabFliterOpen { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> grabFliterClose { get; set; } = new Array<string>();

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new TanglekelpComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.attackComponentName)
		{
			attackComponentName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.grabNum)
		{
			grabNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.destroyUse)
		{
			destroyUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.targetDestroyUse)
		{
			targetDestroyUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dragDelay)
		{
			dragDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dragAudio)
		{
			dragAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.entrySplashAudio)
		{
			entrySplashAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.submergeAudio)
		{
			submergeAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.createTargetSplash)
		{
			createTargetSplash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.createParentSplash)
		{
			createParentSplash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dragStateEvent)
		{
			dragStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.grabSpriteScene)
		{
			grabSpriteScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.grabAnimeClips)
		{
			grabAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.grabAnimeTimeScale)
		{
			grabAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.grabFliterOpen)
		{
			grabFliterOpen = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.grabFliterClose)
		{
			grabFliterClose = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		StringName from;
		if (name == PropertyName.attackComponentName)
		{
			from = attackComponentName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.grabNum)
		{
			value = VariantUtils.CreateFrom<int>(grabNum);
			return true;
		}
		bool from2;
		if (name == PropertyName.destroyUse)
		{
			from2 = destroyUse;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.targetDestroyUse)
		{
			from2 = targetDestroyUse;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		float from3;
		if (name == PropertyName.dragDelay)
		{
			from3 = dragDelay;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		string from4;
		if (name == PropertyName.dragAudio)
		{
			from4 = dragAudio;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.entrySplashAudio)
		{
			from4 = entrySplashAudio;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.submergeAudio)
		{
			from4 = submergeAudio;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.createTargetSplash)
		{
			from2 = createTargetSplash;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.createParentSplash)
		{
			from2 = createParentSplash;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.dragStateEvent)
		{
			from = dragStateEvent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from = idleStateEvent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.grabSpriteScene)
		{
			value = VariantUtils.CreateFrom<PackedScene>(grabSpriteScene);
			return true;
		}
		if (name == PropertyName.grabAnimeClips)
		{
			from4 = grabAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.grabAnimeTimeScale)
		{
			from3 = grabAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.grabFliterOpen)
		{
			value = VariantUtils.CreateFromArray(grabFliterOpen);
			return true;
		}
		if (name == PropertyName.grabFliterClose)
		{
			value = VariantUtils.CreateFromArray(grabFliterClose);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Dependencies", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.attackComponentName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Drag", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.grabNum, PropertyHint.Range, "1,100,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.destroyUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.targetDestroyUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dragDelay, PropertyHint.Range, "0,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dragAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.entrySplashAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.submergeAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createTargetSplash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createParentSplash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.dragStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.grabSpriteScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.grabAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.grabAnimeTimeScale, PropertyHint.Range, "0.01,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.grabFliterOpen, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.grabFliterClose, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.attackComponentName, Variant.From<StringName>(attackComponentName));
		info.AddProperty(PropertyName.grabNum, Variant.From<int>(grabNum));
		info.AddProperty(PropertyName.destroyUse, Variant.From<bool>(destroyUse));
		info.AddProperty(PropertyName.targetDestroyUse, Variant.From<bool>(targetDestroyUse));
		info.AddProperty(PropertyName.dragDelay, Variant.From<float>(dragDelay));
		info.AddProperty(PropertyName.dragAudio, Variant.From<string>(dragAudio));
		info.AddProperty(PropertyName.entrySplashAudio, Variant.From<string>(entrySplashAudio));
		info.AddProperty(PropertyName.submergeAudio, Variant.From<string>(submergeAudio));
		info.AddProperty(PropertyName.createTargetSplash, Variant.From<bool>(createTargetSplash));
		info.AddProperty(PropertyName.createParentSplash, Variant.From<bool>(createParentSplash));
		info.AddProperty(PropertyName.dragStateEvent, Variant.From<StringName>(dragStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.grabSpriteScene, Variant.From<PackedScene>(grabSpriteScene));
		info.AddProperty(PropertyName.grabAnimeClips, Variant.From<string>(grabAnimeClips));
		info.AddProperty(PropertyName.grabAnimeTimeScale, Variant.From<float>(grabAnimeTimeScale));
		info.AddProperty(PropertyName.grabFliterOpen, Variant.CreateFrom(grabFliterOpen));
		info.AddProperty(PropertyName.grabFliterClose, Variant.CreateFrom(grabFliterClose));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.attackComponentName, out var value))
		{
			attackComponentName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.grabNum, out var value2))
		{
			grabNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.destroyUse, out var value3))
		{
			destroyUse = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.targetDestroyUse, out var value4))
		{
			targetDestroyUse = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dragDelay, out var value5))
		{
			dragDelay = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dragAudio, out var value6))
		{
			dragAudio = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.entrySplashAudio, out var value7))
		{
			entrySplashAudio = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.submergeAudio, out var value8))
		{
			submergeAudio = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.createTargetSplash, out var value9))
		{
			createTargetSplash = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.createParentSplash, out var value10))
		{
			createParentSplash = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dragStateEvent, out var value11))
		{
			dragStateEvent = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value12))
		{
			idleStateEvent = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.grabSpriteScene, out var value13))
		{
			grabSpriteScene = value13.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.grabAnimeClips, out var value14))
		{
			grabAnimeClips = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.grabAnimeTimeScale, out var value15))
		{
			grabAnimeTimeScale = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.grabFliterOpen, out var value16))
		{
			grabFliterOpen = value16.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.grabFliterClose, out var value17))
		{
			grabFliterClose = value17.AsGodotArray<string>();
		}
	}
}
