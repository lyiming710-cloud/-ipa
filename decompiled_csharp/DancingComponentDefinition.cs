using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/DancingComponent/DancingComponentDefinition.cs")]
public class DancingComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName dancerRiseDuration = "dancerRiseDuration";

		public static readonly StringName dancerWalkSpeedScaleMultiplier = "dancerWalkSpeedScaleMultiplier";

		public static readonly StringName syncDancerAnimation = "syncDancerAnimation";

		public static readonly StringName walkTimeInit = "walkTimeInit";

		public static readonly StringName danceTimeInit = "danceTimeInit";

		public static readonly StringName moonWalkGridDistance = "moonWalkGridDistance";

		public static readonly StringName pointStateEvent = "pointStateEvent";

		public static readonly StringName moonWalkStateEvent = "moonWalkStateEvent";

		public static readonly StringName danceStateEvent = "danceStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName moonWalkAnimeClip = "moonWalkAnimeClip";

		public static readonly StringName moonWalkAnimeTimeScale = "moonWalkAnimeTimeScale";

		public static readonly StringName armRiseAnimeClip = "armRiseAnimeClip";

		public static readonly StringName armRiseAnimeTimeScale = "armRiseAnimeTimeScale";

		public static readonly StringName pointUpAnimeClip = "pointUpAnimeClip";

		public static readonly StringName pointDownAnimeClip = "pointDownAnimeClip";

		public static readonly StringName pointAnimeTimeScale = "pointAnimeTimeScale";

		public static readonly StringName pointDownDelay = "pointDownDelay";

		public static readonly StringName walkAnimeClip = "walkAnimeClip";

		public static readonly StringName dieAnimeTimeScale = "dieAnimeTimeScale";

		public static readonly StringName moonWalkSpriteScaleX = "moonWalkSpriteScaleX";

		public static readonly StringName normalSpriteScaleX = "normalSpriteScaleX";

		public static readonly StringName danceSpriteScaleX = "danceSpriteScaleX";

		public static readonly StringName armRiseFlipSprite = "armRiseFlipSprite";

		public static readonly StringName attackComponentName = "attackComponentName";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName spotlightPath = "spotlightPath";

		public static readonly StringName spotlight2Path = "spotlight2Path";

		public static readonly StringName spotlightGradient = "spotlightGradient";

		public static readonly StringName spotlightChangeInterval = "spotlightChangeInterval";

		public static readonly StringName spotlightAudioName = "spotlightAudioName";

		public static readonly StringName spotlightColorCycleEnabled = "spotlightColorCycleEnabled";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportSubgroup("Dancer", "")]
	[Export(PropertyHint.None, "")]
	public string dancerPacketName { get; set; } = "ZombieDancer";

	[Export(PropertyHint.Range, "0,30,0.05")]
	public float dancerRiseDuration { get; set; } = 1.5f;

	[Export(PropertyHint.Range, "0,10,0.05")]
	public float dancerWalkSpeedScaleMultiplier { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public bool syncDancerAnimation { get; set; } = true;

	[ExportSubgroup("Timing", "")]
	[Export(PropertyHint.Range, "1,100,1")]
	public int walkTimeInit { get; set; } = 4;

	[Export(PropertyHint.Range, "1,100,1")]
	public int danceTimeInit { get; set; } = 2;

	[Export(PropertyHint.Range, "0,20,0.05")]
	public float moonWalkGridDistance { get; set; } = 2.5f;

	[ExportSubgroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName pointStateEvent { get; set; } = "ToPoint";

	[Export(PropertyHint.None, "")]
	public StringName moonWalkStateEvent { get; set; } = "ToMoonWalk";

	[Export(PropertyHint.None, "")]
	public StringName danceStateEvent { get; set; } = "ToDance";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[ExportSubgroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string moonWalkAnimeClip { get; set; } = "MoonWalk";

	[Export(PropertyHint.None, "")]
	public float moonWalkAnimeTimeScale { get; set; } = 2f;

	[Export(PropertyHint.None, "")]
	public string armRiseAnimeClip { get; set; } = "ArmRise";

	[Export(PropertyHint.None, "")]
	public float armRiseAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string pointUpAnimeClip { get; set; } = "PointUp";

	[Export(PropertyHint.None, "")]
	public string pointDownAnimeClip { get; set; } = "PointDown";

	[Export(PropertyHint.None, "")]
	public float pointAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.Range, "0,30,0.05")]
	public float pointDownDelay { get; set; } = 0.75f;

	[Export(PropertyHint.None, "")]
	public string walkAnimeClip { get; set; } = "Walk";

	[Export(PropertyHint.None, "")]
	public float dieAnimeTimeScale { get; set; } = 2f;

	[ExportSubgroup("Sprite Scale", "")]
	[Export(PropertyHint.None, "")]
	public float moonWalkSpriteScaleX { get; set; } = -1f;

	[Export(PropertyHint.None, "")]
	public float normalSpriteScaleX { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public float danceSpriteScaleX { get; set; } = -1f;

	[Export(PropertyHint.None, "")]
	public bool armRiseFlipSprite { get; set; } = true;

	[ExportSubgroup("Dependencies", "")]
	[Export(PropertyHint.None, "")]
	public StringName attackComponentName { get; set; } = "AttackComponent";

	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public NodePath spotlightPath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public NodePath spotlight2Path { get; set; } = new NodePath();

	[ExportSubgroup("Spotlight", "")]
	[Export(PropertyHint.None, "")]
	public Gradient spotlightGradient { get; set; }

	[Export(PropertyHint.Range, "0,60,0.05")]
	public float spotlightChangeInterval { get; set; } = 3f;

	[Export(PropertyHint.None, "")]
	public string spotlightAudioName { get; set; } = "Dancer";

	[Export(PropertyHint.None, "")]
	public bool spotlightColorCycleEnabled { get; set; } = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new DancingComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dancerPacketName)
		{
			dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dancerRiseDuration)
		{
			dancerRiseDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dancerWalkSpeedScaleMultiplier)
		{
			dancerWalkSpeedScaleMultiplier = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.syncDancerAnimation)
		{
			syncDancerAnimation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.walkTimeInit)
		{
			walkTimeInit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.danceTimeInit)
		{
			danceTimeInit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.moonWalkGridDistance)
		{
			moonWalkGridDistance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.pointStateEvent)
		{
			pointStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.moonWalkStateEvent)
		{
			moonWalkStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.danceStateEvent)
		{
			danceStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.moonWalkAnimeClip)
		{
			moonWalkAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.moonWalkAnimeTimeScale)
		{
			moonWalkAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.armRiseAnimeClip)
		{
			armRiseAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.armRiseAnimeTimeScale)
		{
			armRiseAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.pointUpAnimeClip)
		{
			pointUpAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pointDownAnimeClip)
		{
			pointDownAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pointAnimeTimeScale)
		{
			pointAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.pointDownDelay)
		{
			pointDownDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.walkAnimeClip)
		{
			walkAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dieAnimeTimeScale)
		{
			dieAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.moonWalkSpriteScaleX)
		{
			moonWalkSpriteScaleX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.normalSpriteScaleX)
		{
			normalSpriteScaleX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.danceSpriteScaleX)
		{
			danceSpriteScaleX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.armRiseFlipSprite)
		{
			armRiseFlipSprite = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.attackComponentName)
		{
			attackComponentName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spotlightPath)
		{
			spotlightPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spotlight2Path)
		{
			spotlight2Path = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spotlightGradient)
		{
			spotlightGradient = VariantUtils.ConvertTo<Gradient>(in value);
			return true;
		}
		if (name == PropertyName.spotlightChangeInterval)
		{
			spotlightChangeInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.spotlightAudioName)
		{
			spotlightAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spotlightColorCycleEnabled)
		{
			spotlightColorCycleEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.dancerPacketName)
		{
			from = dancerPacketName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from2;
		if (name == PropertyName.dancerRiseDuration)
		{
			from2 = dancerRiseDuration;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.dancerWalkSpeedScaleMultiplier)
		{
			from2 = dancerWalkSpeedScaleMultiplier;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.syncDancerAnimation)
		{
			from3 = syncDancerAnimation;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		int from4;
		if (name == PropertyName.walkTimeInit)
		{
			from4 = walkTimeInit;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.danceTimeInit)
		{
			from4 = danceTimeInit;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.moonWalkGridDistance)
		{
			from2 = moonWalkGridDistance;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		StringName from5;
		if (name == PropertyName.pointStateEvent)
		{
			from5 = pointStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.moonWalkStateEvent)
		{
			from5 = moonWalkStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.danceStateEvent)
		{
			from5 = danceStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from5 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.moonWalkAnimeClip)
		{
			from = moonWalkAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.moonWalkAnimeTimeScale)
		{
			from2 = moonWalkAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.armRiseAnimeClip)
		{
			from = armRiseAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.armRiseAnimeTimeScale)
		{
			from2 = armRiseAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.pointUpAnimeClip)
		{
			from = pointUpAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.pointDownAnimeClip)
		{
			from = pointDownAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.pointAnimeTimeScale)
		{
			from2 = pointAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.pointDownDelay)
		{
			from2 = pointDownDelay;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.walkAnimeClip)
		{
			from = walkAnimeClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dieAnimeTimeScale)
		{
			from2 = dieAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.moonWalkSpriteScaleX)
		{
			from2 = moonWalkSpriteScaleX;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.normalSpriteScaleX)
		{
			from2 = normalSpriteScaleX;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.danceSpriteScaleX)
		{
			from2 = danceSpriteScaleX;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.armRiseFlipSprite)
		{
			from3 = armRiseFlipSprite;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.attackComponentName)
		{
			from5 = attackComponentName;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		NodePath from6;
		if (name == PropertyName.spritePath)
		{
			from6 = spritePath;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.spotlightPath)
		{
			from6 = spotlightPath;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.spotlight2Path)
		{
			from6 = spotlight2Path;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.spotlightGradient)
		{
			value = VariantUtils.CreateFrom<Gradient>(spotlightGradient);
			return true;
		}
		if (name == PropertyName.spotlightChangeInterval)
		{
			from2 = spotlightChangeInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.spotlightAudioName)
		{
			from = spotlightAudioName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.spotlightColorCycleEnabled)
		{
			from3 = spotlightColorCycleEnabled;
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
			new PropertyInfo(Variant.Type.Nil, "Dancer", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dancerRiseDuration, PropertyHint.Range, "0,30,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dancerWalkSpeedScaleMultiplier, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.syncDancerAnimation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Timing", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.walkTimeInit, PropertyHint.Range, "1,100,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.danceTimeInit, PropertyHint.Range, "1,100,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.moonWalkGridDistance, PropertyHint.Range, "0,20,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.pointStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.moonWalkStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.danceStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.moonWalkAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.moonWalkAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.armRiseAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.armRiseAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.pointUpAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.pointDownAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pointAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pointDownDelay, PropertyHint.Range, "0,30,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.walkAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dieAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Sprite Scale", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.moonWalkSpriteScaleX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.normalSpriteScaleX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.danceSpriteScaleX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.armRiseFlipSprite, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Dependencies", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.attackComponentName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spotlightPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spotlight2Path, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Spotlight", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.spotlightGradient, PropertyHint.ResourceType, "Gradient", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spotlightChangeInterval, PropertyHint.Range, "0,60,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.spotlightAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spotlightColorCycleEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dancerPacketName, Variant.From<string>(dancerPacketName));
		info.AddProperty(PropertyName.dancerRiseDuration, Variant.From<float>(dancerRiseDuration));
		info.AddProperty(PropertyName.dancerWalkSpeedScaleMultiplier, Variant.From<float>(dancerWalkSpeedScaleMultiplier));
		info.AddProperty(PropertyName.syncDancerAnimation, Variant.From<bool>(syncDancerAnimation));
		info.AddProperty(PropertyName.walkTimeInit, Variant.From<int>(walkTimeInit));
		info.AddProperty(PropertyName.danceTimeInit, Variant.From<int>(danceTimeInit));
		info.AddProperty(PropertyName.moonWalkGridDistance, Variant.From<float>(moonWalkGridDistance));
		info.AddProperty(PropertyName.pointStateEvent, Variant.From<StringName>(pointStateEvent));
		info.AddProperty(PropertyName.moonWalkStateEvent, Variant.From<StringName>(moonWalkStateEvent));
		info.AddProperty(PropertyName.danceStateEvent, Variant.From<StringName>(danceStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.moonWalkAnimeClip, Variant.From<string>(moonWalkAnimeClip));
		info.AddProperty(PropertyName.moonWalkAnimeTimeScale, Variant.From<float>(moonWalkAnimeTimeScale));
		info.AddProperty(PropertyName.armRiseAnimeClip, Variant.From<string>(armRiseAnimeClip));
		info.AddProperty(PropertyName.armRiseAnimeTimeScale, Variant.From<float>(armRiseAnimeTimeScale));
		info.AddProperty(PropertyName.pointUpAnimeClip, Variant.From<string>(pointUpAnimeClip));
		info.AddProperty(PropertyName.pointDownAnimeClip, Variant.From<string>(pointDownAnimeClip));
		info.AddProperty(PropertyName.pointAnimeTimeScale, Variant.From<float>(pointAnimeTimeScale));
		info.AddProperty(PropertyName.pointDownDelay, Variant.From<float>(pointDownDelay));
		info.AddProperty(PropertyName.walkAnimeClip, Variant.From<string>(walkAnimeClip));
		info.AddProperty(PropertyName.dieAnimeTimeScale, Variant.From<float>(dieAnimeTimeScale));
		info.AddProperty(PropertyName.moonWalkSpriteScaleX, Variant.From<float>(moonWalkSpriteScaleX));
		info.AddProperty(PropertyName.normalSpriteScaleX, Variant.From<float>(normalSpriteScaleX));
		info.AddProperty(PropertyName.danceSpriteScaleX, Variant.From<float>(danceSpriteScaleX));
		info.AddProperty(PropertyName.armRiseFlipSprite, Variant.From<bool>(armRiseFlipSprite));
		info.AddProperty(PropertyName.attackComponentName, Variant.From<StringName>(attackComponentName));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.spotlightPath, Variant.From<NodePath>(spotlightPath));
		info.AddProperty(PropertyName.spotlight2Path, Variant.From<NodePath>(spotlight2Path));
		info.AddProperty(PropertyName.spotlightGradient, Variant.From<Gradient>(spotlightGradient));
		info.AddProperty(PropertyName.spotlightChangeInterval, Variant.From<float>(spotlightChangeInterval));
		info.AddProperty(PropertyName.spotlightAudioName, Variant.From<string>(spotlightAudioName));
		info.AddProperty(PropertyName.spotlightColorCycleEnabled, Variant.From<bool>(spotlightColorCycleEnabled));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dancerPacketName, out var value))
		{
			dancerPacketName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dancerRiseDuration, out var value2))
		{
			dancerRiseDuration = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dancerWalkSpeedScaleMultiplier, out var value3))
		{
			dancerWalkSpeedScaleMultiplier = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.syncDancerAnimation, out var value4))
		{
			syncDancerAnimation = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.walkTimeInit, out var value5))
		{
			walkTimeInit = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.danceTimeInit, out var value6))
		{
			danceTimeInit = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.moonWalkGridDistance, out var value7))
		{
			moonWalkGridDistance = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.pointStateEvent, out var value8))
		{
			pointStateEvent = value8.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.moonWalkStateEvent, out var value9))
		{
			moonWalkStateEvent = value9.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.danceStateEvent, out var value10))
		{
			danceStateEvent = value10.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value11))
		{
			idleStateEvent = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.moonWalkAnimeClip, out var value12))
		{
			moonWalkAnimeClip = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.moonWalkAnimeTimeScale, out var value13))
		{
			moonWalkAnimeTimeScale = value13.As<float>();
		}
		if (info.TryGetProperty(PropertyName.armRiseAnimeClip, out var value14))
		{
			armRiseAnimeClip = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.armRiseAnimeTimeScale, out var value15))
		{
			armRiseAnimeTimeScale = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.pointUpAnimeClip, out var value16))
		{
			pointUpAnimeClip = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pointDownAnimeClip, out var value17))
		{
			pointDownAnimeClip = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pointAnimeTimeScale, out var value18))
		{
			pointAnimeTimeScale = value18.As<float>();
		}
		if (info.TryGetProperty(PropertyName.pointDownDelay, out var value19))
		{
			pointDownDelay = value19.As<float>();
		}
		if (info.TryGetProperty(PropertyName.walkAnimeClip, out var value20))
		{
			walkAnimeClip = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dieAnimeTimeScale, out var value21))
		{
			dieAnimeTimeScale = value21.As<float>();
		}
		if (info.TryGetProperty(PropertyName.moonWalkSpriteScaleX, out var value22))
		{
			moonWalkSpriteScaleX = value22.As<float>();
		}
		if (info.TryGetProperty(PropertyName.normalSpriteScaleX, out var value23))
		{
			normalSpriteScaleX = value23.As<float>();
		}
		if (info.TryGetProperty(PropertyName.danceSpriteScaleX, out var value24))
		{
			danceSpriteScaleX = value24.As<float>();
		}
		if (info.TryGetProperty(PropertyName.armRiseFlipSprite, out var value25))
		{
			armRiseFlipSprite = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.attackComponentName, out var value26))
		{
			attackComponentName = value26.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value27))
		{
			spritePath = value27.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spotlightPath, out var value28))
		{
			spotlightPath = value28.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spotlight2Path, out var value29))
		{
			spotlight2Path = value29.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spotlightGradient, out var value30))
		{
			spotlightGradient = value30.As<Gradient>();
		}
		if (info.TryGetProperty(PropertyName.spotlightChangeInterval, out var value31))
		{
			spotlightChangeInterval = value31.As<float>();
		}
		if (info.TryGetProperty(PropertyName.spotlightAudioName, out var value32))
		{
			spotlightAudioName = value32.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spotlightColorCycleEnabled, out var value33))
		{
			spotlightColorCycleEnabled = value33.As<bool>();
		}
	}
}
