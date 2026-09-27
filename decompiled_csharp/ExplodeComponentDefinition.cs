using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ExplodeComponent/ExplodeComponentDefinition.cs")]
public class ExplodeComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName explodeAnimeClips = "explodeAnimeClips";

		public static readonly StringName explodeAnimeTimeScale = "explodeAnimeTimeScale";

		public static readonly StringName hostAuthoritative = "hostAuthoritative";

		public static readonly StringName skipGameRunning = "skipGameRunning";

		public static readonly StringName checkIZM = "checkIZM";

		public static readonly StringName explodeMethod = "explodeMethod";

		public static readonly StringName explodeUse = "explodeUse";

		public static readonly StringName reload = "reload";

		public static readonly StringName explodeOnce = "explodeOnce";

		public static readonly StringName reverseAudio = "reverseAudio";

		public static readonly StringName explodeAudio = "explodeAudio";

		public static readonly StringName explodeEvent = "explodeEvent";

		public static readonly StringName explodeEffect = "explodeEffect";

		public static readonly StringName explodeShape = "explodeShape";

		public static readonly StringName scaleExplodeShapeToMapGrid = "scaleExplodeShapeToMapGrid";

		public static readonly StringName explodeRange = "explodeRange";

		public static readonly StringName explodeStateEvent = "explodeStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName explodeJalaFireType = "explodeJalaFireType";

		public static readonly StringName explodeJalaNum = "explodeJalaNum";

		public static readonly StringName explodeJalaOffset = "explodeJalaOffset";

		public static readonly StringName cameraShakeUse = "cameraShakeUse";

		public static readonly StringName cameraShakeOffset = "cameraShakeOffset";

		public static readonly StringName cameraShakeForce = "cameraShakeForce";

		public static readonly StringName cameraShakeInterval = "cameraShakeInterval";

		public static readonly StringName cameraShakeTime = "cameraShakeTime";

		public static readonly StringName screenColorBlinkUse = "screenColorBlinkUse";

		public static readonly StringName screenColorBlinkColor = "screenColorBlinkColor";

		public static readonly StringName screenColorBlinkDuration = "screenColorBlinkDuration";

		public static readonly StringName screenColorBlinkRise = "screenColorBlinkRise";

		public static readonly StringName craterCreateUse = "craterCreateUse";

		public static readonly StringName craterCreatePacketName = "craterCreatePacketName";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public string explodeAnimeClips { get; set; } = "Explode";

	[Export(PropertyHint.None, "")]
	public float explodeAnimeTimeScale { get; set; } = 1f;

	[ExportGroup("Explosion", "")]
	[Export(PropertyHint.None, "")]
	public bool hostAuthoritative { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool skipGameRunning { get; set; }

	[Export(PropertyHint.None, "")]
	public bool checkIZM { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public string explodeMethod { get; set; } = "Range";

	[Export(PropertyHint.None, "")]
	public bool explodeUse { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool reload { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool explodeOnce { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public string reverseAudio { get; set; } = "ReverseExplosion";

	[Export(PropertyHint.None, "")]
	public string explodeAudio { get; set; } = "ExplodeCherrybomb";

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> explodeEvent { get; set; } = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public PackedScene explodeEffect { get; set; }

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource explodeShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool scaleExplodeShapeToMapGrid { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Vector2 explodeRange { get; set; } = new Vector2(1.5f, 1.5f);

	[Export(PropertyHint.None, "")]
	public StringName explodeStateEvent { get; set; } = "ToExplode";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[ExportGroup("Jalapeno", "")]
	[Export(PropertyHint.None, "")]
	public string explodeJalaFireType { get; set; } = "Fire";

	[Export(PropertyHint.None, "")]
	public float explodeJalaNum { get; set; } = 1800f;

	[Export(PropertyHint.None, "")]
	public Array<int> explodeJalaOffset { get; set; } = new Array<int>();

	[ExportGroup("Camera", "")]
	[Export(PropertyHint.None, "")]
	public bool cameraShakeUse { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Vector2 cameraShakeOffset { get; set; } = Vector2.One;

	[Export(PropertyHint.None, "")]
	public float cameraShakeForce { get; set; } = 5f;

	[Export(PropertyHint.None, "")]
	public float cameraShakeInterval { get; set; } = 0.05f;

	[Export(PropertyHint.None, "")]
	public int cameraShakeTime { get; set; } = 4;

	[ExportGroup("Screen", "")]
	[Export(PropertyHint.None, "")]
	public bool screenColorBlinkUse { get; set; }

	[Export(PropertyHint.None, "")]
	public Color screenColorBlinkColor { get; set; } = Colors.DarkSlateBlue;

	[Export(PropertyHint.None, "")]
	public float screenColorBlinkDuration { get; set; } = 0.5f;

	[Export(PropertyHint.None, "")]
	public bool screenColorBlinkRise { get; set; }

	[ExportGroup("Extension", "")]
	[Export(PropertyHint.None, "")]
	public bool craterCreateUse { get; set; }

	[Export(PropertyHint.None, "")]
	public string craterCreatePacketName { get; set; } = "CraterDayGround";

	public int CollisionPreviewShapeCount => GodotObject.IsInstanceValid(explodeShape) ? 1 : 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0)
		{
			return null;
		}
		return explodeShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ExplodeComponent();
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
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.explodeAnimeClips)
		{
			explodeAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explodeAnimeTimeScale)
		{
			explodeAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.hostAuthoritative)
		{
			hostAuthoritative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.skipGameRunning)
		{
			skipGameRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkIZM)
		{
			checkIZM = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.explodeMethod)
		{
			explodeMethod = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explodeUse)
		{
			explodeUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.reload)
		{
			reload = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.explodeOnce)
		{
			explodeOnce = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.reverseAudio)
		{
			reverseAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explodeAudio)
		{
			explodeAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explodeEvent)
		{
			explodeEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.explodeEffect)
		{
			explodeEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.explodeShape)
		{
			explodeShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.scaleExplodeShapeToMapGrid)
		{
			scaleExplodeShapeToMapGrid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.explodeRange)
		{
			explodeRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.explodeStateEvent)
		{
			explodeStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.explodeJalaFireType)
		{
			explodeJalaFireType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explodeJalaNum)
		{
			explodeJalaNum = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.explodeJalaOffset)
		{
			explodeJalaOffset = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeUse)
		{
			cameraShakeUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeOffset)
		{
			cameraShakeOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeForce)
		{
			cameraShakeForce = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeInterval)
		{
			cameraShakeInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.cameraShakeTime)
		{
			cameraShakeTime = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.screenColorBlinkUse)
		{
			screenColorBlinkUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.screenColorBlinkColor)
		{
			screenColorBlinkColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.screenColorBlinkDuration)
		{
			screenColorBlinkDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.screenColorBlinkRise)
		{
			screenColorBlinkRise = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.craterCreateUse)
		{
			craterCreateUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.craterCreatePacketName)
		{
			craterCreatePacketName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.explodeAnimeClips)
		{
			from = explodeAnimeClips;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		float from2;
		if (name == PropertyName.explodeAnimeTimeScale)
		{
			from2 = explodeAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.hostAuthoritative)
		{
			from3 = hostAuthoritative;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.skipGameRunning)
		{
			from3 = skipGameRunning;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.checkIZM)
		{
			from3 = checkIZM;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.explodeMethod)
		{
			from = explodeMethod;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.explodeUse)
		{
			from3 = explodeUse;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.reload)
		{
			from3 = reload;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.explodeOnce)
		{
			from3 = explodeOnce;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.reverseAudio)
		{
			from = reverseAudio;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.explodeAudio)
		{
			from = explodeAudio;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.explodeEvent)
		{
			value = VariantUtils.CreateFromArray(explodeEvent);
			return true;
		}
		if (name == PropertyName.explodeEffect)
		{
			value = VariantUtils.CreateFrom<PackedScene>(explodeEffect);
			return true;
		}
		if (name == PropertyName.explodeShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(explodeShape);
			return true;
		}
		if (name == PropertyName.scaleExplodeShapeToMapGrid)
		{
			from3 = scaleExplodeShapeToMapGrid;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		Vector2 from4;
		if (name == PropertyName.explodeRange)
		{
			from4 = explodeRange;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		StringName from5;
		if (name == PropertyName.explodeStateEvent)
		{
			from5 = explodeStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from5 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.explodeJalaFireType)
		{
			from = explodeJalaFireType;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.explodeJalaNum)
		{
			from2 = explodeJalaNum;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.explodeJalaOffset)
		{
			value = VariantUtils.CreateFromArray(explodeJalaOffset);
			return true;
		}
		if (name == PropertyName.cameraShakeUse)
		{
			from3 = cameraShakeUse;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.cameraShakeOffset)
		{
			from4 = cameraShakeOffset;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.cameraShakeForce)
		{
			from2 = cameraShakeForce;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.cameraShakeInterval)
		{
			from2 = cameraShakeInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		int from6;
		if (name == PropertyName.cameraShakeTime)
		{
			from6 = cameraShakeTime;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.screenColorBlinkUse)
		{
			from3 = screenColorBlinkUse;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.screenColorBlinkColor)
		{
			value = VariantUtils.CreateFrom<Color>(screenColorBlinkColor);
			return true;
		}
		if (name == PropertyName.screenColorBlinkDuration)
		{
			from2 = screenColorBlinkDuration;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.screenColorBlinkRise)
		{
			from3 = screenColorBlinkRise;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.craterCreateUse)
		{
			from3 = craterCreateUse;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.craterCreatePacketName)
		{
			from = craterCreatePacketName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
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
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.explodeAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explodeAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Explosion", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hostAuthoritative, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skipGameRunning, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkIZM, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.explodeMethod, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.explodeUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.reload, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.explodeOnce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.reverseAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.explodeAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.explodeEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.explodeEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.explodeShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.scaleExplodeShapeToMapGrid, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.explodeRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.explodeStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Jalapeno", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.explodeJalaFireType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explodeJalaNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.explodeJalaOffset, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Camera", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.cameraShakeUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.cameraShakeOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeForce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraShakeInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cameraShakeTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Screen", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.screenColorBlinkUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.screenColorBlinkColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.screenColorBlinkDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.screenColorBlinkRise, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Extension", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.craterCreateUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.craterCreatePacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.explodeAnimeClips, Variant.From<string>(explodeAnimeClips));
		info.AddProperty(PropertyName.explodeAnimeTimeScale, Variant.From<float>(explodeAnimeTimeScale));
		info.AddProperty(PropertyName.hostAuthoritative, Variant.From<bool>(hostAuthoritative));
		info.AddProperty(PropertyName.skipGameRunning, Variant.From<bool>(skipGameRunning));
		info.AddProperty(PropertyName.checkIZM, Variant.From<bool>(checkIZM));
		info.AddProperty(PropertyName.explodeMethod, Variant.From<string>(explodeMethod));
		info.AddProperty(PropertyName.explodeUse, Variant.From<bool>(explodeUse));
		info.AddProperty(PropertyName.reload, Variant.From<bool>(reload));
		info.AddProperty(PropertyName.explodeOnce, Variant.From<bool>(explodeOnce));
		info.AddProperty(PropertyName.reverseAudio, Variant.From<string>(reverseAudio));
		info.AddProperty(PropertyName.explodeAudio, Variant.From<string>(explodeAudio));
		info.AddProperty(PropertyName.explodeEvent, Variant.CreateFrom(explodeEvent));
		info.AddProperty(PropertyName.explodeEffect, Variant.From<PackedScene>(explodeEffect));
		info.AddProperty(PropertyName.explodeShape, Variant.From<AabbShape2DResource>(explodeShape));
		info.AddProperty(PropertyName.scaleExplodeShapeToMapGrid, Variant.From<bool>(scaleExplodeShapeToMapGrid));
		info.AddProperty(PropertyName.explodeRange, Variant.From<Vector2>(explodeRange));
		info.AddProperty(PropertyName.explodeStateEvent, Variant.From<StringName>(explodeStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.explodeJalaFireType, Variant.From<string>(explodeJalaFireType));
		info.AddProperty(PropertyName.explodeJalaNum, Variant.From<float>(explodeJalaNum));
		info.AddProperty(PropertyName.explodeJalaOffset, Variant.CreateFrom(explodeJalaOffset));
		info.AddProperty(PropertyName.cameraShakeUse, Variant.From<bool>(cameraShakeUse));
		info.AddProperty(PropertyName.cameraShakeOffset, Variant.From<Vector2>(cameraShakeOffset));
		info.AddProperty(PropertyName.cameraShakeForce, Variant.From<float>(cameraShakeForce));
		info.AddProperty(PropertyName.cameraShakeInterval, Variant.From<float>(cameraShakeInterval));
		info.AddProperty(PropertyName.cameraShakeTime, Variant.From<int>(cameraShakeTime));
		info.AddProperty(PropertyName.screenColorBlinkUse, Variant.From<bool>(screenColorBlinkUse));
		info.AddProperty(PropertyName.screenColorBlinkColor, Variant.From<Color>(screenColorBlinkColor));
		info.AddProperty(PropertyName.screenColorBlinkDuration, Variant.From<float>(screenColorBlinkDuration));
		info.AddProperty(PropertyName.screenColorBlinkRise, Variant.From<bool>(screenColorBlinkRise));
		info.AddProperty(PropertyName.craterCreateUse, Variant.From<bool>(craterCreateUse));
		info.AddProperty(PropertyName.craterCreatePacketName, Variant.From<string>(craterCreatePacketName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spritePath, out var value))
		{
			spritePath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.explodeAnimeClips, out var value2))
		{
			explodeAnimeClips = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explodeAnimeTimeScale, out var value3))
		{
			explodeAnimeTimeScale = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.hostAuthoritative, out var value4))
		{
			hostAuthoritative = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.skipGameRunning, out var value5))
		{
			skipGameRunning = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkIZM, out var value6))
		{
			checkIZM = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.explodeMethod, out var value7))
		{
			explodeMethod = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explodeUse, out var value8))
		{
			explodeUse = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.reload, out var value9))
		{
			reload = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.explodeOnce, out var value10))
		{
			explodeOnce = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.reverseAudio, out var value11))
		{
			reverseAudio = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explodeAudio, out var value12))
		{
			explodeAudio = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explodeEvent, out var value13))
		{
			explodeEvent = value13.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.explodeEffect, out var value14))
		{
			explodeEffect = value14.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.explodeShape, out var value15))
		{
			explodeShape = value15.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.scaleExplodeShapeToMapGrid, out var value16))
		{
			scaleExplodeShapeToMapGrid = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.explodeRange, out var value17))
		{
			explodeRange = value17.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.explodeStateEvent, out var value18))
		{
			explodeStateEvent = value18.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value19))
		{
			idleStateEvent = value19.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.explodeJalaFireType, out var value20))
		{
			explodeJalaFireType = value20.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explodeJalaNum, out var value21))
		{
			explodeJalaNum = value21.As<float>();
		}
		if (info.TryGetProperty(PropertyName.explodeJalaOffset, out var value22))
		{
			explodeJalaOffset = value22.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeUse, out var value23))
		{
			cameraShakeUse = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeOffset, out var value24))
		{
			cameraShakeOffset = value24.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeForce, out var value25))
		{
			cameraShakeForce = value25.As<float>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeInterval, out var value26))
		{
			cameraShakeInterval = value26.As<float>();
		}
		if (info.TryGetProperty(PropertyName.cameraShakeTime, out var value27))
		{
			cameraShakeTime = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName.screenColorBlinkUse, out var value28))
		{
			screenColorBlinkUse = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.screenColorBlinkColor, out var value29))
		{
			screenColorBlinkColor = value29.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.screenColorBlinkDuration, out var value30))
		{
			screenColorBlinkDuration = value30.As<float>();
		}
		if (info.TryGetProperty(PropertyName.screenColorBlinkRise, out var value31))
		{
			screenColorBlinkRise = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.craterCreateUse, out var value32))
		{
			craterCreateUse = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.craterCreatePacketName, out var value33))
		{
			craterCreatePacketName = value33.As<string>();
		}
	}
}
