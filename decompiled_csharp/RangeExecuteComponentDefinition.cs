using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/RangeExecuteComponent/RangeExecuteComponentDefinition.cs")]
public class RangeExecuteComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName rangeType = "rangeType";

		public static readonly StringName executeShape = "executeShape";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName filterGravestones = "filterGravestones";

		public static readonly StringName filterVases = "filterVases";

		public static readonly StringName maxTargets = "maxTargets";

		public static readonly StringName eventOffset = "eventOffset";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.RANGE_TYPE rangeType { get; set; } = TowerDefenseEnum.RANGE_TYPE.AREA;

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource executeShape { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList { get; set; } = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public bool filterGravestones { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool filterVases { get; set; } = true;

	[Export(PropertyHint.Range, "-1,1024,1")]
	public int maxTargets { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public Vector2 eventOffset { get; set; } = Vector2.Zero;

	public int CollisionPreviewShapeCount => GodotObject.IsInstanceValid(executeShape) ? 1 : 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0)
		{
			return null;
		}
		return executeShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new RangeExecuteComponent();
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
		if (name == PropertyName.rangeType)
		{
			rangeType = VariantUtils.ConvertTo<TowerDefenseEnum.RANGE_TYPE>(in value);
			return true;
		}
		if (name == PropertyName.executeShape)
		{
			executeShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.filterGravestones)
		{
			filterGravestones = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.filterVases)
		{
			filterVases = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.maxTargets)
		{
			maxTargets = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.eventOffset)
		{
			eventOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.rangeType)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.RANGE_TYPE>(rangeType);
			return true;
		}
		if (name == PropertyName.executeShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(executeShape);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		bool from;
		if (name == PropertyName.filterGravestones)
		{
			from = filterGravestones;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.filterVases)
		{
			from = filterVases;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.maxTargets)
		{
			from2 = maxTargets;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.eventOffset)
		{
			value = VariantUtils.CreateFrom<Vector2>(eventOffset);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from2 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from2 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.rangeType, PropertyHint.Enum, "NOONE,AREA,ROW,ENEMY", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.executeShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.filterGravestones, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.filterVases, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxTargets, PropertyHint.Range, "-1,1024,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.eventOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.rangeType, Variant.From<TowerDefenseEnum.RANGE_TYPE>(rangeType));
		info.AddProperty(PropertyName.executeShape, Variant.From<AabbShape2DResource>(executeShape));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.filterGravestones, Variant.From<bool>(filterGravestones));
		info.AddProperty(PropertyName.filterVases, Variant.From<bool>(filterVases));
		info.AddProperty(PropertyName.maxTargets, Variant.From<int>(maxTargets));
		info.AddProperty(PropertyName.eventOffset, Variant.From<Vector2>(eventOffset));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.rangeType, out var value))
		{
			rangeType = value.As<TowerDefenseEnum.RANGE_TYPE>();
		}
		if (info.TryGetProperty(PropertyName.executeShape, out var value2))
		{
			executeShape = value2.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value3))
		{
			eventList = value3.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.filterGravestones, out var value4))
		{
			filterGravestones = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.filterVases, out var value5))
		{
			filterVases = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.maxTargets, out var value6))
		{
			maxTargets = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.eventOffset, out var value7))
		{
			eventOffset = value7.As<Vector2>();
		}
	}
}
