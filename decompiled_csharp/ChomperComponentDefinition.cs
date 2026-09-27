using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ChomperComponent/ChomperComponentDefinition.cs")]
public class ChomperComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName chewTime = "chewTime";

		public static readonly StringName chewTimePercentage = "chewTimePercentage";

		public static readonly StringName biteAttack = "biteAttack";

		public static readonly StringName biteEvent = "biteEvent";

		public static readonly StringName biteOnly = "biteOnly";

		public static readonly StringName biteNoLimit = "biteNoLimit";

		public static readonly StringName ignoreBiteHurt = "ignoreBiteHurt";

		public static readonly StringName healthPercentage = "healthPercentage";

		public static readonly StringName biteEventName = "biteEventName";

		public static readonly StringName biteStartAnimeClips = "biteStartAnimeClips";

		public static readonly StringName biteStartAnimeTimeScale = "biteStartAnimeTimeScale";

		public static readonly StringName biteLoopAnimeClips = "biteLoopAnimeClips";

		public static readonly StringName biteLoopAnimeTimeScale = "biteLoopAnimeTimeScale";

		public static readonly StringName biteEndAnimeClips = "biteEndAnimeClips";

		public static readonly StringName biteEndAnimeTimeScale = "biteEndAnimeTimeScale";

		public static readonly StringName chewReadyAnimeClips = "chewReadyAnimeClips";

		public static readonly StringName chewReadyAnimeTimeScale = "chewReadyAnimeTimeScale";

		public static readonly StringName chewAnimeClips = "chewAnimeClips";

		public static readonly StringName chewAnimeTimeScale = "chewAnimeTimeScale";

		public static readonly StringName swallowAnimeClips = "swallowAnimeClips";

		public static readonly StringName swallowAnimeTimeScale = "swallowAnimeTimeScale";

		public static readonly StringName isPartSprite = "isPartSprite";

		public static readonly StringName partIdleAnimeClips = "partIdleAnimeClips";

		public static readonly StringName partIdleAnimeTimeScale = "partIdleAnimeTimeScale";

		public static readonly StringName playBiteAudio = "playBiteAudio";

		public static readonly StringName biteAudioName = "biteAudioName";

		public static readonly StringName attackStateEvent = "attackStateEvent";

		public static readonly StringName chewStateEvent = "chewStateEvent";

		public static readonly StringName swallowStateEvent = "swallowStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName suckUse = "suckUse";

		public static readonly StringName suckShape = "suckShape";

		public static readonly StringName attackComponentPath = "attackComponentPath";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Bite", "")]
	[Export(PropertyHint.None, "")]
	public float chewTime { get; set; } = 30f;

	[Export(PropertyHint.None, "")]
	public float chewTimePercentage { get; set; } = -1f;

	[Export(PropertyHint.None, "")]
	public float biteAttack { get; set; } = -1f;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> biteEvent { get; set; } = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public bool biteOnly { get; set; }

	[Export(PropertyHint.None, "")]
	public bool biteNoLimit { get; set; }

	[Export(PropertyHint.None, "")]
	public bool ignoreBiteHurt { get; set; }

	[Export(PropertyHint.None, "")]
	public float healthPercentage { get; set; } = -1f;

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string biteEventName { get; set; } = "attack";

	[Export(PropertyHint.None, "")]
	public string biteStartAnimeClips { get; set; } = "Bite";

	[Export(PropertyHint.None, "")]
	public float biteStartAnimeTimeScale { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public string biteLoopAnimeClips { get; set; } = "BiteLoop";

	[Export(PropertyHint.None, "")]
	public float biteLoopAnimeTimeScale { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public string biteEndAnimeClips { get; set; } = "BiteEnd";

	[Export(PropertyHint.None, "")]
	public float biteEndAnimeTimeScale { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public string chewReadyAnimeClips { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public float chewReadyAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string chewAnimeClips { get; set; } = "Chew";

	[Export(PropertyHint.None, "")]
	public float chewAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string swallowAnimeClips { get; set; } = "Swallow";

	[Export(PropertyHint.None, "")]
	public float swallowAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public bool isPartSprite { get; set; }

	[Export(PropertyHint.None, "")]
	public string partIdleAnimeClips { get; set; } = "Idle";

	[Export(PropertyHint.None, "")]
	public float partIdleAnimeTimeScale { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public bool playBiteAudio { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public string biteAudioName { get; set; } = "BigChomp";

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName attackStateEvent { get; set; } = "ToAttack";

	[Export(PropertyHint.None, "")]
	public StringName chewStateEvent { get; set; } = "ToChew";

	[Export(PropertyHint.None, "")]
	public StringName swallowStateEvent { get; set; } = "ToSwallow";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[ExportGroup("Suction", "")]
	[Export(PropertyHint.None, "")]
	public bool suckUse { get; set; }

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource suckShape { get; set; }

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public NodePath attackComponentPath { get; set; }

	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; }

	public int CollisionPreviewShapeCount
	{
		get
		{
			if (!suckUse || !GodotObject.IsInstanceValid(suckShape))
			{
				return 0;
			}
			return 1;
		}
	}

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0 || !suckUse)
		{
			return null;
		}
		return suckShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ChomperComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetCollisionPreviewShape, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCollisionPreviewRay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetCollisionPreviewShape && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbShape2DResource>(GetCollisionPreviewShape(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbRay2DResource>(GetCollisionPreviewRay(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetCollisionPreviewShape)
		{
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.chewTime)
		{
			chewTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.chewTimePercentage)
		{
			chewTimePercentage = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.biteAttack)
		{
			biteAttack = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.biteEvent)
		{
			biteEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.biteOnly)
		{
			biteOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.biteNoLimit)
		{
			biteNoLimit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ignoreBiteHurt)
		{
			ignoreBiteHurt = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.healthPercentage)
		{
			healthPercentage = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.biteEventName)
		{
			biteEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.biteStartAnimeClips)
		{
			biteStartAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.biteStartAnimeTimeScale)
		{
			biteStartAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.biteLoopAnimeClips)
		{
			biteLoopAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.biteLoopAnimeTimeScale)
		{
			biteLoopAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.biteEndAnimeClips)
		{
			biteEndAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.biteEndAnimeTimeScale)
		{
			biteEndAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.chewReadyAnimeClips)
		{
			chewReadyAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.chewReadyAnimeTimeScale)
		{
			chewReadyAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.chewAnimeClips)
		{
			chewAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.chewAnimeTimeScale)
		{
			chewAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.swallowAnimeClips)
		{
			swallowAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.swallowAnimeTimeScale)
		{
			swallowAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.isPartSprite)
		{
			isPartSprite = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.partIdleAnimeClips)
		{
			partIdleAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.partIdleAnimeTimeScale)
		{
			partIdleAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.playBiteAudio)
		{
			playBiteAudio = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.biteAudioName)
		{
			biteAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.attackStateEvent)
		{
			attackStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.chewStateEvent)
		{
			chewStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.swallowStateEvent)
		{
			swallowStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.suckUse)
		{
			suckUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.suckShape)
		{
			suckShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.attackComponentPath)
		{
			attackComponentPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		float from;
		if (name == PropertyName.chewTime)
		{
			from = chewTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.chewTimePercentage)
		{
			from = chewTimePercentage;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.biteAttack)
		{
			from = biteAttack;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.biteEvent)
		{
			value = VariantUtils.CreateFromArray(biteEvent);
			return true;
		}
		bool from2;
		if (name == PropertyName.biteOnly)
		{
			from2 = biteOnly;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.biteNoLimit)
		{
			from2 = biteNoLimit;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ignoreBiteHurt)
		{
			from2 = ignoreBiteHurt;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.healthPercentage)
		{
			from = healthPercentage;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		string from3;
		if (name == PropertyName.biteEventName)
		{
			from3 = biteEventName;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.biteStartAnimeClips)
		{
			from3 = biteStartAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.biteStartAnimeTimeScale)
		{
			from = biteStartAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.biteLoopAnimeClips)
		{
			from3 = biteLoopAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.biteLoopAnimeTimeScale)
		{
			from = biteLoopAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.biteEndAnimeClips)
		{
			from3 = biteEndAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.biteEndAnimeTimeScale)
		{
			from = biteEndAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.chewReadyAnimeClips)
		{
			from3 = chewReadyAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.chewReadyAnimeTimeScale)
		{
			from = chewReadyAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.chewAnimeClips)
		{
			from3 = chewAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.chewAnimeTimeScale)
		{
			from = chewAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.swallowAnimeClips)
		{
			from3 = swallowAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.swallowAnimeTimeScale)
		{
			from = swallowAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.isPartSprite)
		{
			from2 = isPartSprite;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.partIdleAnimeClips)
		{
			from3 = partIdleAnimeClips;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.partIdleAnimeTimeScale)
		{
			from = partIdleAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.playBiteAudio)
		{
			from2 = playBiteAudio;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.biteAudioName)
		{
			from3 = biteAudioName;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		StringName from4;
		if (name == PropertyName.attackStateEvent)
		{
			from4 = attackStateEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.chewStateEvent)
		{
			from4 = chewStateEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.swallowStateEvent)
		{
			from4 = swallowStateEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from4 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.suckUse)
		{
			from2 = suckUse;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.suckShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(suckShape);
			return true;
		}
		NodePath from5;
		if (name == PropertyName.attackComponentPath)
		{
			from5 = attackComponentPath;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			from5 = spritePath;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		int from6;
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from6 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from6 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Bite", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.chewTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.chewTimePercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.biteAttack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.biteEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.biteOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.biteNoLimit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ignoreBiteHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.healthPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.biteEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.biteStartAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.biteStartAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.biteLoopAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.biteLoopAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.biteEndAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.biteEndAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.chewReadyAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.chewReadyAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.chewAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.chewAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.swallowAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.swallowAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPartSprite, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.partIdleAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.partIdleAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.playBiteAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.biteAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.attackStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.chewStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.swallowStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Suction", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suckUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.suckShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.attackComponentPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.chewTime, Variant.From<float>(chewTime));
		info.AddProperty(PropertyName.chewTimePercentage, Variant.From<float>(chewTimePercentage));
		info.AddProperty(PropertyName.biteAttack, Variant.From<float>(biteAttack));
		info.AddProperty(PropertyName.biteEvent, Variant.CreateFrom(biteEvent));
		info.AddProperty(PropertyName.biteOnly, Variant.From<bool>(biteOnly));
		info.AddProperty(PropertyName.biteNoLimit, Variant.From<bool>(biteNoLimit));
		info.AddProperty(PropertyName.ignoreBiteHurt, Variant.From<bool>(ignoreBiteHurt));
		info.AddProperty(PropertyName.healthPercentage, Variant.From<float>(healthPercentage));
		info.AddProperty(PropertyName.biteEventName, Variant.From<string>(biteEventName));
		info.AddProperty(PropertyName.biteStartAnimeClips, Variant.From<string>(biteStartAnimeClips));
		info.AddProperty(PropertyName.biteStartAnimeTimeScale, Variant.From<float>(biteStartAnimeTimeScale));
		info.AddProperty(PropertyName.biteLoopAnimeClips, Variant.From<string>(biteLoopAnimeClips));
		info.AddProperty(PropertyName.biteLoopAnimeTimeScale, Variant.From<float>(biteLoopAnimeTimeScale));
		info.AddProperty(PropertyName.biteEndAnimeClips, Variant.From<string>(biteEndAnimeClips));
		info.AddProperty(PropertyName.biteEndAnimeTimeScale, Variant.From<float>(biteEndAnimeTimeScale));
		info.AddProperty(PropertyName.chewReadyAnimeClips, Variant.From<string>(chewReadyAnimeClips));
		info.AddProperty(PropertyName.chewReadyAnimeTimeScale, Variant.From<float>(chewReadyAnimeTimeScale));
		info.AddProperty(PropertyName.chewAnimeClips, Variant.From<string>(chewAnimeClips));
		info.AddProperty(PropertyName.chewAnimeTimeScale, Variant.From<float>(chewAnimeTimeScale));
		info.AddProperty(PropertyName.swallowAnimeClips, Variant.From<string>(swallowAnimeClips));
		info.AddProperty(PropertyName.swallowAnimeTimeScale, Variant.From<float>(swallowAnimeTimeScale));
		info.AddProperty(PropertyName.isPartSprite, Variant.From<bool>(isPartSprite));
		info.AddProperty(PropertyName.partIdleAnimeClips, Variant.From<string>(partIdleAnimeClips));
		info.AddProperty(PropertyName.partIdleAnimeTimeScale, Variant.From<float>(partIdleAnimeTimeScale));
		info.AddProperty(PropertyName.playBiteAudio, Variant.From<bool>(playBiteAudio));
		info.AddProperty(PropertyName.biteAudioName, Variant.From<string>(biteAudioName));
		info.AddProperty(PropertyName.attackStateEvent, Variant.From<StringName>(attackStateEvent));
		info.AddProperty(PropertyName.chewStateEvent, Variant.From<StringName>(chewStateEvent));
		info.AddProperty(PropertyName.swallowStateEvent, Variant.From<StringName>(swallowStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.suckUse, Variant.From<bool>(suckUse));
		info.AddProperty(PropertyName.suckShape, Variant.From<AabbShape2DResource>(suckShape));
		info.AddProperty(PropertyName.attackComponentPath, Variant.From<NodePath>(attackComponentPath));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.chewTime, out var value))
		{
			chewTime = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.chewTimePercentage, out var value2))
		{
			chewTimePercentage = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.biteAttack, out var value3))
		{
			biteAttack = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.biteEvent, out var value4))
		{
			biteEvent = value4.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.biteOnly, out var value5))
		{
			biteOnly = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.biteNoLimit, out var value6))
		{
			biteNoLimit = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ignoreBiteHurt, out var value7))
		{
			ignoreBiteHurt = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.healthPercentage, out var value8))
		{
			healthPercentage = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.biteEventName, out var value9))
		{
			biteEventName = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.biteStartAnimeClips, out var value10))
		{
			biteStartAnimeClips = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.biteStartAnimeTimeScale, out var value11))
		{
			biteStartAnimeTimeScale = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.biteLoopAnimeClips, out var value12))
		{
			biteLoopAnimeClips = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.biteLoopAnimeTimeScale, out var value13))
		{
			biteLoopAnimeTimeScale = value13.As<float>();
		}
		if (info.TryGetProperty(PropertyName.biteEndAnimeClips, out var value14))
		{
			biteEndAnimeClips = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.biteEndAnimeTimeScale, out var value15))
		{
			biteEndAnimeTimeScale = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.chewReadyAnimeClips, out var value16))
		{
			chewReadyAnimeClips = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName.chewReadyAnimeTimeScale, out var value17))
		{
			chewReadyAnimeTimeScale = value17.As<float>();
		}
		if (info.TryGetProperty(PropertyName.chewAnimeClips, out var value18))
		{
			chewAnimeClips = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.chewAnimeTimeScale, out var value19))
		{
			chewAnimeTimeScale = value19.As<float>();
		}
		if (info.TryGetProperty(PropertyName.swallowAnimeClips, out var value20))
		{
			swallowAnimeClips = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.swallowAnimeTimeScale, out var value21))
		{
			swallowAnimeTimeScale = value21.As<float>();
		}
		if (info.TryGetProperty(PropertyName.isPartSprite, out var value22))
		{
			isPartSprite = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.partIdleAnimeClips, out var value23))
		{
			partIdleAnimeClips = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName.partIdleAnimeTimeScale, out var value24))
		{
			partIdleAnimeTimeScale = value24.As<float>();
		}
		if (info.TryGetProperty(PropertyName.playBiteAudio, out var value25))
		{
			playBiteAudio = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.biteAudioName, out var value26))
		{
			biteAudioName = value26.As<string>();
		}
		if (info.TryGetProperty(PropertyName.attackStateEvent, out var value27))
		{
			attackStateEvent = value27.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.chewStateEvent, out var value28))
		{
			chewStateEvent = value28.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.swallowStateEvent, out var value29))
		{
			swallowStateEvent = value29.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value30))
		{
			idleStateEvent = value30.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.suckUse, out var value31))
		{
			suckUse = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.suckShape, out var value32))
		{
			suckShape = value32.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.attackComponentPath, out var value33))
		{
			attackComponentPath = value33.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value34))
		{
			spritePath = value34.As<NodePath>();
		}
	}
}
