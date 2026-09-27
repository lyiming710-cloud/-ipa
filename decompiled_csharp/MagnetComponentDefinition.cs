using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/MagnetComponent/MagnetComponentDefinition.cs")]
public class MagnetComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName scaleCheckShapeToMapGrid = "scaleCheckShapeToMapGrid";

		public static readonly StringName checkRange = "checkRange";

		public static readonly StringName checkAll = "checkAll";

		public static readonly StringName hostAuthoritative = "hostAuthoritative";

		public static readonly StringName posMarkerPath = "posMarkerPath";

		public static readonly StringName breakDownTime = "breakDownTime";

		public static readonly StringName magnetMoveSpeed = "magnetMoveSpeed";

		public static readonly StringName arriveDistance = "arriveDistance";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName drawEventName = "drawEventName";

		public static readonly StringName shootBeginAnimeClips = "shootBeginAnimeClips";

		public static readonly StringName shootBeginAnimeTimeScale = "shootBeginAnimeTimeScale";

		public static readonly StringName shootBeginAnimeStartPosition = "shootBeginAnimeStartPosition";

		public static readonly StringName shootAnimeClips = "shootAnimeClips";

		public static readonly StringName shootAnimeTimeScale = "shootAnimeTimeScale";

		public static readonly StringName shootAnimeStartPosition = "shootAnimeStartPosition";

		public static readonly StringName shootEndAnimeClips = "shootEndAnimeClips";

		public static readonly StringName shootEndAnimeTimeScale = "shootEndAnimeTimeScale";

		public static readonly StringName shootEndAnimeStartPosition = "shootEndAnimeStartPosition";

		public static readonly StringName noActiveAnimeClips = "noActiveAnimeClips";

		public static readonly StringName noActiveAnimeTimeScale = "noActiveAnimeTimeScale";

		public static readonly StringName noActiveAnimeStartPosition = "noActiveAnimeStartPosition";

		public static readonly StringName drawAudioName = "drawAudioName";

		public static readonly StringName beginStateEvent = "beginStateEvent";

		public static readonly StringName shootStateEvent = "shootStateEvent";

		public static readonly StringName noActiveStateEvent = "noActiveStateEvent";

		public static readonly StringName endStateEvent = "endStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Targeting", "")]
	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool scaleCheckShapeToMapGrid { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Vector2 checkRange { get; set; } = new Vector2(2.5f, 2.5f);

	[Export(PropertyHint.None, "")]
	public bool checkAll { get; set; }

	[Export(PropertyHint.None, "")]
	public bool hostAuthoritative { get; set; } = true;

	[ExportGroup("Attachment", "")]
	[Export(PropertyHint.None, "")]
	public NodePath posMarkerPath { get; set; } = new NodePath();

	[Export(PropertyHint.Range, "0,120,0.01,or_greater")]
	public float breakDownTime { get; set; } = 15f;

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public float magnetMoveSpeed { get; set; } = 10f;

	[Export(PropertyHint.Range, "0,100,0.001,or_greater")]
	public float arriveDistance { get; set; } = 0.01f;

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public string drawEventName { get; set; } = "action";

	[Export(PropertyHint.None, "")]
	public string shootBeginAnimeClips { get; set; } = "Begin";

	[Export(PropertyHint.None, "")]
	public float shootBeginAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public float shootBeginAnimeStartPosition { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public string shootAnimeClips { get; set; } = "Shooting";

	[Export(PropertyHint.None, "")]
	public float shootAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public float shootAnimeStartPosition { get; set; }

	[Export(PropertyHint.None, "")]
	public string shootEndAnimeClips { get; set; } = "End";

	[Export(PropertyHint.None, "")]
	public float shootEndAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public float shootEndAnimeStartPosition { get; set; } = 0.2f;

	[Export(PropertyHint.None, "")]
	public string noActiveAnimeClips { get; set; } = "NonActiveIdle2";

	[Export(PropertyHint.None, "")]
	public float noActiveAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public float noActiveAnimeStartPosition { get; set; } = 0.2f;

	[ExportGroup("Audio", "")]
	[Export(PropertyHint.None, "")]
	public string drawAudioName { get; set; } = "Magnet";

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName beginStateEvent { get; set; } = "ToBegin";

	[Export(PropertyHint.None, "")]
	public StringName shootStateEvent { get; set; } = "ToShoot";

	[Export(PropertyHint.None, "")]
	public StringName noActiveStateEvent { get; set; } = "ToNoActive";

	[Export(PropertyHint.None, "")]
	public StringName endStateEvent { get; set; } = "ToEnd";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	public int CollisionPreviewShapeCount => GodotObject.IsInstanceValid(checkShape) ? 1 : 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0)
		{
			return null;
		}
		return checkShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new MagnetComponent();
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
		if (name == PropertyName.checkShape)
		{
			checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.scaleCheckShapeToMapGrid)
		{
			scaleCheckShapeToMapGrid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.checkRange)
		{
			checkRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			checkAll = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hostAuthoritative)
		{
			hostAuthoritative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.posMarkerPath)
		{
			posMarkerPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.breakDownTime)
		{
			breakDownTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.magnetMoveSpeed)
		{
			magnetMoveSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.arriveDistance)
		{
			arriveDistance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.drawEventName)
		{
			drawEventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shootBeginAnimeClips)
		{
			shootBeginAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shootBeginAnimeTimeScale)
		{
			shootBeginAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.shootBeginAnimeStartPosition)
		{
			shootBeginAnimeStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.shootAnimeClips)
		{
			shootAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shootAnimeTimeScale)
		{
			shootAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.shootAnimeStartPosition)
		{
			shootAnimeStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.shootEndAnimeClips)
		{
			shootEndAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.shootEndAnimeTimeScale)
		{
			shootEndAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.shootEndAnimeStartPosition)
		{
			shootEndAnimeStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.noActiveAnimeClips)
		{
			noActiveAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.noActiveAnimeTimeScale)
		{
			noActiveAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.noActiveAnimeStartPosition)
		{
			noActiveAnimeStartPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.drawAudioName)
		{
			drawAudioName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.beginStateEvent)
		{
			beginStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.shootStateEvent)
		{
			shootStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.noActiveStateEvent)
		{
			noActiveStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.endStateEvent)
		{
			endStateEvent = VariantUtils.ConvertTo<StringName>(in value);
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
		if (name == PropertyName.checkShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(checkShape);
			return true;
		}
		bool from;
		if (name == PropertyName.scaleCheckShapeToMapGrid)
		{
			from = scaleCheckShapeToMapGrid;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.checkRange)
		{
			value = VariantUtils.CreateFrom<Vector2>(checkRange);
			return true;
		}
		if (name == PropertyName.checkAll)
		{
			from = checkAll;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.hostAuthoritative)
		{
			from = hostAuthoritative;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		NodePath from2;
		if (name == PropertyName.posMarkerPath)
		{
			from2 = posMarkerPath;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		float from3;
		if (name == PropertyName.breakDownTime)
		{
			from3 = breakDownTime;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.magnetMoveSpeed)
		{
			from3 = magnetMoveSpeed;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.arriveDistance)
		{
			from3 = arriveDistance;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			from2 = spritePath;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		string from4;
		if (name == PropertyName.drawEventName)
		{
			from4 = drawEventName;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.shootBeginAnimeClips)
		{
			from4 = shootBeginAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.shootBeginAnimeTimeScale)
		{
			from3 = shootBeginAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.shootBeginAnimeStartPosition)
		{
			from3 = shootBeginAnimeStartPosition;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.shootAnimeClips)
		{
			from4 = shootAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.shootAnimeTimeScale)
		{
			from3 = shootAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.shootAnimeStartPosition)
		{
			from3 = shootAnimeStartPosition;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.shootEndAnimeClips)
		{
			from4 = shootEndAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.shootEndAnimeTimeScale)
		{
			from3 = shootEndAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.shootEndAnimeStartPosition)
		{
			from3 = shootEndAnimeStartPosition;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.noActiveAnimeClips)
		{
			from4 = noActiveAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.noActiveAnimeTimeScale)
		{
			from3 = noActiveAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.noActiveAnimeStartPosition)
		{
			from3 = noActiveAnimeStartPosition;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.drawAudioName)
		{
			from4 = drawAudioName;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		StringName from5;
		if (name == PropertyName.beginStateEvent)
		{
			from5 = beginStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.shootStateEvent)
		{
			from5 = shootStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.noActiveStateEvent)
		{
			from5 = noActiveStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.endStateEvent)
		{
			from5 = endStateEvent;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from5 = idleStateEvent;
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
			new PropertyInfo(Variant.Type.Nil, "Targeting", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.scaleCheckShapeToMapGrid, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.checkRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAll, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hostAuthoritative, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Attachment", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.posMarkerPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.breakDownTime, PropertyHint.Range, "0,120,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.magnetMoveSpeed, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.arriveDistance, PropertyHint.Range, "0,100,0.001,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.drawEventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.shootBeginAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootBeginAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootBeginAnimeStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.shootAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootAnimeStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.shootEndAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootEndAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shootEndAnimeStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.noActiveAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.noActiveAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.noActiveAnimeStartPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Audio", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.drawAudioName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.beginStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.shootStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.noActiveStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.endStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.scaleCheckShapeToMapGrid, Variant.From<bool>(scaleCheckShapeToMapGrid));
		info.AddProperty(PropertyName.checkRange, Variant.From<Vector2>(checkRange));
		info.AddProperty(PropertyName.checkAll, Variant.From<bool>(checkAll));
		info.AddProperty(PropertyName.hostAuthoritative, Variant.From<bool>(hostAuthoritative));
		info.AddProperty(PropertyName.posMarkerPath, Variant.From<NodePath>(posMarkerPath));
		info.AddProperty(PropertyName.breakDownTime, Variant.From<float>(breakDownTime));
		info.AddProperty(PropertyName.magnetMoveSpeed, Variant.From<float>(magnetMoveSpeed));
		info.AddProperty(PropertyName.arriveDistance, Variant.From<float>(arriveDistance));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.drawEventName, Variant.From<string>(drawEventName));
		info.AddProperty(PropertyName.shootBeginAnimeClips, Variant.From<string>(shootBeginAnimeClips));
		info.AddProperty(PropertyName.shootBeginAnimeTimeScale, Variant.From<float>(shootBeginAnimeTimeScale));
		info.AddProperty(PropertyName.shootBeginAnimeStartPosition, Variant.From<float>(shootBeginAnimeStartPosition));
		info.AddProperty(PropertyName.shootAnimeClips, Variant.From<string>(shootAnimeClips));
		info.AddProperty(PropertyName.shootAnimeTimeScale, Variant.From<float>(shootAnimeTimeScale));
		info.AddProperty(PropertyName.shootAnimeStartPosition, Variant.From<float>(shootAnimeStartPosition));
		info.AddProperty(PropertyName.shootEndAnimeClips, Variant.From<string>(shootEndAnimeClips));
		info.AddProperty(PropertyName.shootEndAnimeTimeScale, Variant.From<float>(shootEndAnimeTimeScale));
		info.AddProperty(PropertyName.shootEndAnimeStartPosition, Variant.From<float>(shootEndAnimeStartPosition));
		info.AddProperty(PropertyName.noActiveAnimeClips, Variant.From<string>(noActiveAnimeClips));
		info.AddProperty(PropertyName.noActiveAnimeTimeScale, Variant.From<float>(noActiveAnimeTimeScale));
		info.AddProperty(PropertyName.noActiveAnimeStartPosition, Variant.From<float>(noActiveAnimeStartPosition));
		info.AddProperty(PropertyName.drawAudioName, Variant.From<string>(drawAudioName));
		info.AddProperty(PropertyName.beginStateEvent, Variant.From<StringName>(beginStateEvent));
		info.AddProperty(PropertyName.shootStateEvent, Variant.From<StringName>(shootStateEvent));
		info.AddProperty(PropertyName.noActiveStateEvent, Variant.From<StringName>(noActiveStateEvent));
		info.AddProperty(PropertyName.endStateEvent, Variant.From<StringName>(endStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.checkShape, out var value))
		{
			checkShape = value.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.scaleCheckShapeToMapGrid, out var value2))
		{
			scaleCheckShapeToMapGrid = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.checkRange, out var value3))
		{
			checkRange = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.checkAll, out var value4))
		{
			checkAll = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hostAuthoritative, out var value5))
		{
			hostAuthoritative = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.posMarkerPath, out var value6))
		{
			posMarkerPath = value6.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.breakDownTime, out var value7))
		{
			breakDownTime = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.magnetMoveSpeed, out var value8))
		{
			magnetMoveSpeed = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.arriveDistance, out var value9))
		{
			arriveDistance = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value10))
		{
			spritePath = value10.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.drawEventName, out var value11))
		{
			drawEventName = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shootBeginAnimeClips, out var value12))
		{
			shootBeginAnimeClips = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shootBeginAnimeTimeScale, out var value13))
		{
			shootBeginAnimeTimeScale = value13.As<float>();
		}
		if (info.TryGetProperty(PropertyName.shootBeginAnimeStartPosition, out var value14))
		{
			shootBeginAnimeStartPosition = value14.As<float>();
		}
		if (info.TryGetProperty(PropertyName.shootAnimeClips, out var value15))
		{
			shootAnimeClips = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shootAnimeTimeScale, out var value16))
		{
			shootAnimeTimeScale = value16.As<float>();
		}
		if (info.TryGetProperty(PropertyName.shootAnimeStartPosition, out var value17))
		{
			shootAnimeStartPosition = value17.As<float>();
		}
		if (info.TryGetProperty(PropertyName.shootEndAnimeClips, out var value18))
		{
			shootEndAnimeClips = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.shootEndAnimeTimeScale, out var value19))
		{
			shootEndAnimeTimeScale = value19.As<float>();
		}
		if (info.TryGetProperty(PropertyName.shootEndAnimeStartPosition, out var value20))
		{
			shootEndAnimeStartPosition = value20.As<float>();
		}
		if (info.TryGetProperty(PropertyName.noActiveAnimeClips, out var value21))
		{
			noActiveAnimeClips = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName.noActiveAnimeTimeScale, out var value22))
		{
			noActiveAnimeTimeScale = value22.As<float>();
		}
		if (info.TryGetProperty(PropertyName.noActiveAnimeStartPosition, out var value23))
		{
			noActiveAnimeStartPosition = value23.As<float>();
		}
		if (info.TryGetProperty(PropertyName.drawAudioName, out var value24))
		{
			drawAudioName = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName.beginStateEvent, out var value25))
		{
			beginStateEvent = value25.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.shootStateEvent, out var value26))
		{
			shootStateEvent = value26.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.noActiveStateEvent, out var value27))
		{
			noActiveStateEvent = value27.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.endStateEvent, out var value28))
		{
			endStateEvent = value28.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value29))
		{
			idleStateEvent = value29.As<StringName>();
		}
	}
}
