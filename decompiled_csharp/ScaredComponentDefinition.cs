using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ScaredComponent/ScaredComponentDefinition.cs")]
public class ScaredComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName scareShape = "scareShape";

		public static readonly StringName scaleScareShapeToMapGrid = "scaleScareShapeToMapGrid";

		public static readonly StringName scareGridSpan = "scareGridSpan";

		public static readonly StringName hostAuthoritative = "hostAuthoritative";

		public static readonly StringName scaredHeight = "scaredHeight";

		public static readonly StringName spritePath = "spritePath";

		public static readonly StringName controlledNodePaths = "controlledNodePaths";

		public static readonly StringName controlledRuntimeInstanceIds = "controlledRuntimeInstanceIds";

		public static readonly StringName controlledRuntimeTypeIds = "controlledRuntimeTypeIds";

		public static readonly StringName scaredDownStateEvent = "scaredDownStateEvent";

		public static readonly StringName scaredGrowStateEvent = "scaredGrowStateEvent";

		public static readonly StringName scaredIdleStateEvent = "scaredIdleStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName scaredDownAnimeClip = "scaredDownAnimeClip";

		public static readonly StringName scaredDownAnimeTimeScale = "scaredDownAnimeTimeScale";

		public static readonly StringName scaredIdleAnimeClip = "scaredIdleAnimeClip";

		public static readonly StringName scaredIdleAnimeTimeScale = "scaredIdleAnimeTimeScale";

		public static readonly StringName scaredGrowAnimeClip = "scaredGrowAnimeClip";

		public static readonly StringName scaredGrowAnimeTimeScale = "scaredGrowAnimeTimeScale";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Detection", "")]
	[Export(PropertyHint.None, "")]
	public AabbShape2DResource scareShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool scaleScareShapeToMapGrid { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Vector2 scareGridSpan { get; set; } = new Vector2(2.75f, 2.75f);

	[Export(PropertyHint.None, "")]
	public bool hostAuthoritative { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.CHARACTER_HEIGHT scaredHeight { get; set; } = TowerDefenseEnum.CHARACTER_HEIGHT.LOW;

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<NodePath> controlledNodePaths { get; set; } = new Array<NodePath>();

	[Export(PropertyHint.None, "")]
	public Array<string> controlledRuntimeInstanceIds { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> controlledRuntimeTypeIds { get; set; } = new Array<string>();

	[ExportGroup("State Events", "")]
	[Export(PropertyHint.None, "")]
	public StringName scaredDownStateEvent { get; set; } = "ToScaredDown";

	[Export(PropertyHint.None, "")]
	public StringName scaredGrowStateEvent { get; set; } = "ToScaredGrow";

	[Export(PropertyHint.None, "")]
	public StringName scaredIdleStateEvent { get; set; } = "ToScaredIdle";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string scaredDownAnimeClip { get; set; } = "Scared";

	[Export(PropertyHint.None, "")]
	public float scaredDownAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string scaredIdleAnimeClip { get; set; } = "ScaredIdle";

	[Export(PropertyHint.None, "")]
	public float scaredIdleAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string scaredGrowAnimeClip { get; set; } = "Grow";

	[Export(PropertyHint.None, "")]
	public float scaredGrowAnimeTimeScale { get; set; } = 1f;

	public int CollisionPreviewShapeCount => GodotObject.IsInstanceValid(scareShape) ? 1 : 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0)
		{
			return null;
		}
		return scareShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ScaredComponent();
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
		if (name == PropertyName.scareShape)
		{
			scareShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.scaleScareShapeToMapGrid)
		{
			scaleScareShapeToMapGrid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.scareGridSpan)
		{
			scareGridSpan = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.hostAuthoritative)
		{
			hostAuthoritative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.scaredHeight)
		{
			scaredHeight = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_HEIGHT>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.controlledNodePaths)
		{
			controlledNodePaths = VariantUtils.ConvertToArray<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.controlledRuntimeInstanceIds)
		{
			controlledRuntimeInstanceIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.controlledRuntimeTypeIds)
		{
			controlledRuntimeTypeIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.scaredDownStateEvent)
		{
			scaredDownStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.scaredGrowStateEvent)
		{
			scaredGrowStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.scaredIdleStateEvent)
		{
			scaredIdleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.scaredDownAnimeClip)
		{
			scaredDownAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.scaredDownAnimeTimeScale)
		{
			scaredDownAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.scaredIdleAnimeClip)
		{
			scaredIdleAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.scaredIdleAnimeTimeScale)
		{
			scaredIdleAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.scaredGrowAnimeClip)
		{
			scaredGrowAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.scaredGrowAnimeTimeScale)
		{
			scaredGrowAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.scareShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(scareShape);
			return true;
		}
		bool from;
		if (name == PropertyName.scaleScareShapeToMapGrid)
		{
			from = scaleScareShapeToMapGrid;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.scareGridSpan)
		{
			value = VariantUtils.CreateFrom<Vector2>(scareGridSpan);
			return true;
		}
		if (name == PropertyName.hostAuthoritative)
		{
			from = hostAuthoritative;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.scaredHeight)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.CHARACTER_HEIGHT>(scaredHeight);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom<NodePath>(spritePath);
			return true;
		}
		if (name == PropertyName.controlledNodePaths)
		{
			value = VariantUtils.CreateFromArray(controlledNodePaths);
			return true;
		}
		if (name == PropertyName.controlledRuntimeInstanceIds)
		{
			value = VariantUtils.CreateFromArray(controlledRuntimeInstanceIds);
			return true;
		}
		if (name == PropertyName.controlledRuntimeTypeIds)
		{
			value = VariantUtils.CreateFromArray(controlledRuntimeTypeIds);
			return true;
		}
		StringName from2;
		if (name == PropertyName.scaredDownStateEvent)
		{
			from2 = scaredDownStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.scaredGrowStateEvent)
		{
			from2 = scaredGrowStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.scaredIdleStateEvent)
		{
			from2 = scaredIdleStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from2 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		string from3;
		if (name == PropertyName.scaredDownAnimeClip)
		{
			from3 = scaredDownAnimeClip;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		float from4;
		if (name == PropertyName.scaredDownAnimeTimeScale)
		{
			from4 = scaredDownAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.scaredIdleAnimeClip)
		{
			from3 = scaredIdleAnimeClip;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.scaredIdleAnimeTimeScale)
		{
			from4 = scaredIdleAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.scaredGrowAnimeClip)
		{
			from3 = scaredGrowAnimeClip;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.scaredGrowAnimeTimeScale)
		{
			from4 = scaredGrowAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		int from5;
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from5 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from5 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Detection", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.scareShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.scaleScareShapeToMapGrid, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scareGridSpan, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hostAuthoritative, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.scaredHeight, PropertyHint.Enum, "GROUND,LOW,NORMAL,TALL", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.controlledNodePaths, PropertyHint.TypeString, "22/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.controlledRuntimeInstanceIds, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.controlledRuntimeTypeIds, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "State Events", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.scaredDownStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.scaredGrowStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.scaredIdleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.scaredDownAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaredDownAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.scaredIdleAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaredIdleAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.scaredGrowAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.scaredGrowAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.scareShape, Variant.From<AabbShape2DResource>(scareShape));
		info.AddProperty(PropertyName.scaleScareShapeToMapGrid, Variant.From<bool>(scaleScareShapeToMapGrid));
		info.AddProperty(PropertyName.scareGridSpan, Variant.From<Vector2>(scareGridSpan));
		info.AddProperty(PropertyName.hostAuthoritative, Variant.From<bool>(hostAuthoritative));
		info.AddProperty(PropertyName.scaredHeight, Variant.From<TowerDefenseEnum.CHARACTER_HEIGHT>(scaredHeight));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
		info.AddProperty(PropertyName.controlledNodePaths, Variant.CreateFrom(controlledNodePaths));
		info.AddProperty(PropertyName.controlledRuntimeInstanceIds, Variant.CreateFrom(controlledRuntimeInstanceIds));
		info.AddProperty(PropertyName.controlledRuntimeTypeIds, Variant.CreateFrom(controlledRuntimeTypeIds));
		info.AddProperty(PropertyName.scaredDownStateEvent, Variant.From<StringName>(scaredDownStateEvent));
		info.AddProperty(PropertyName.scaredGrowStateEvent, Variant.From<StringName>(scaredGrowStateEvent));
		info.AddProperty(PropertyName.scaredIdleStateEvent, Variant.From<StringName>(scaredIdleStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.scaredDownAnimeClip, Variant.From<string>(scaredDownAnimeClip));
		info.AddProperty(PropertyName.scaredDownAnimeTimeScale, Variant.From<float>(scaredDownAnimeTimeScale));
		info.AddProperty(PropertyName.scaredIdleAnimeClip, Variant.From<string>(scaredIdleAnimeClip));
		info.AddProperty(PropertyName.scaredIdleAnimeTimeScale, Variant.From<float>(scaredIdleAnimeTimeScale));
		info.AddProperty(PropertyName.scaredGrowAnimeClip, Variant.From<string>(scaredGrowAnimeClip));
		info.AddProperty(PropertyName.scaredGrowAnimeTimeScale, Variant.From<float>(scaredGrowAnimeTimeScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.scareShape, out var value))
		{
			scareShape = value.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.scaleScareShapeToMapGrid, out var value2))
		{
			scaleScareShapeToMapGrid = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.scareGridSpan, out var value3))
		{
			scareGridSpan = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.hostAuthoritative, out var value4))
		{
			hostAuthoritative = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.scaredHeight, out var value5))
		{
			scaredHeight = value5.As<TowerDefenseEnum.CHARACTER_HEIGHT>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value6))
		{
			spritePath = value6.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.controlledNodePaths, out var value7))
		{
			controlledNodePaths = value7.AsGodotArray<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.controlledRuntimeInstanceIds, out var value8))
		{
			controlledRuntimeInstanceIds = value8.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.controlledRuntimeTypeIds, out var value9))
		{
			controlledRuntimeTypeIds = value9.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.scaredDownStateEvent, out var value10))
		{
			scaredDownStateEvent = value10.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.scaredGrowStateEvent, out var value11))
		{
			scaredGrowStateEvent = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.scaredIdleStateEvent, out var value12))
		{
			scaredIdleStateEvent = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value13))
		{
			idleStateEvent = value13.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.scaredDownAnimeClip, out var value14))
		{
			scaredDownAnimeClip = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName.scaredDownAnimeTimeScale, out var value15))
		{
			scaredDownAnimeTimeScale = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.scaredIdleAnimeClip, out var value16))
		{
			scaredIdleAnimeClip = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName.scaredIdleAnimeTimeScale, out var value17))
		{
			scaredIdleAnimeTimeScale = value17.As<float>();
		}
		if (info.TryGetProperty(PropertyName.scaredGrowAnimeClip, out var value18))
		{
			scaredGrowAnimeClip = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.scaredGrowAnimeTimeScale, out var value19))
		{
			scaredGrowAnimeTimeScale = value19.As<float>();
		}
	}
}
