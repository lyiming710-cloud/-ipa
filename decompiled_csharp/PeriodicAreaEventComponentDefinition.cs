using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/PeriodicAreaEventComponent/PeriodicAreaEventComponentDefinition.cs")]
public class PeriodicAreaEventComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName attackMethod = "attackMethod";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName staticTime = "staticTime";

		public static readonly StringName dpsEffectInterval = "dpsEffectInterval";

		public static readonly StringName maxIntervalTriggersPerFrame = "maxIntervalTriggersPerFrame";

		public static readonly StringName targetEffectType = "targetEffectType";

		public static readonly StringName targetEffectScene = "targetEffectScene";

		public static readonly StringName targetEffectSpawnPolicy = "targetEffectSpawnPolicy";

		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName checkAllLine = "checkAllLine";

		public static readonly StringName skipVases = "skipVases";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Enum, "StaticTime,Dps")]
	public string attackMethod { get; set; } = "StaticTime";

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList { get; set; } = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.Range, "0.001,60,0.001,or_greater")]
	public float staticTime { get; set; } = 1f;

	[Export(PropertyHint.Range, "0.001,10,0.001,or_greater")]
	public float dpsEffectInterval { get; set; } = 0.1f;

	[Export(PropertyHint.Range, "1,16,1")]
	public int maxIntervalTriggersPerFrame { get; set; } = 1;

	[Export(PropertyHint.Enum, "Particles,Sprite,Disabled")]
	public string targetEffectType { get; set; } = "Particles";

	[Export(PropertyHint.None, "")]
	public PackedScene targetEffectScene { get; set; }

	[Export(PropertyHint.None, "")]
	public PeriodicAreaEventComponent.TargetEffectSpawnPolicy targetEffectSpawnPolicy { get; set; }

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool checkAllLine { get; set; }

	[Export(PropertyHint.None, "")]
	public bool skipVases { get; set; } = true;

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
		return new PeriodicAreaEventComponent();
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
		if (name == PropertyName.attackMethod)
		{
			attackMethod = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.staticTime)
		{
			staticTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.dpsEffectInterval)
		{
			dpsEffectInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.maxIntervalTriggersPerFrame)
		{
			maxIntervalTriggersPerFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.targetEffectType)
		{
			targetEffectType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.targetEffectScene)
		{
			targetEffectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.targetEffectSpawnPolicy)
		{
			targetEffectSpawnPolicy = VariantUtils.ConvertTo<PeriodicAreaEventComponent.TargetEffectSpawnPolicy>(in value);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.checkAllLine)
		{
			checkAllLine = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.skipVases)
		{
			skipVases = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.attackMethod)
		{
			from = attackMethod;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		float from2;
		if (name == PropertyName.staticTime)
		{
			from2 = staticTime;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.dpsEffectInterval)
		{
			from2 = dpsEffectInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		int from3;
		if (name == PropertyName.maxIntervalTriggersPerFrame)
		{
			from3 = maxIntervalTriggersPerFrame;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.targetEffectType)
		{
			from = targetEffectType;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.targetEffectScene)
		{
			value = VariantUtils.CreateFrom<PackedScene>(targetEffectScene);
			return true;
		}
		if (name == PropertyName.targetEffectSpawnPolicy)
		{
			value = VariantUtils.CreateFrom<PeriodicAreaEventComponent.TargetEffectSpawnPolicy>(targetEffectSpawnPolicy);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(checkShape);
			return true;
		}
		bool from4;
		if (name == PropertyName.checkAllLine)
		{
			from4 = checkAllLine;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.skipVases)
		{
			from4 = skipVases;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from3 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from3 = CollisionPreviewRayCount;
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
			new PropertyInfo(Variant.Type.String, PropertyName.attackMethod, PropertyHint.Enum, "StaticTime,Dps", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.staticTime, PropertyHint.Range, "0.001,60,0.001,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dpsEffectInterval, PropertyHint.Range, "0.001,10,0.001,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxIntervalTriggersPerFrame, PropertyHint.Range, "1,16,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.targetEffectType, PropertyHint.Enum, "Particles,Sprite,Disabled", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.targetEffectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.targetEffectSpawnPolicy, PropertyHint.Enum, "PerEvent,PerTarget", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkAllLine, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skipVases, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.attackMethod, Variant.From<string>(attackMethod));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.staticTime, Variant.From<float>(staticTime));
		info.AddProperty(PropertyName.dpsEffectInterval, Variant.From<float>(dpsEffectInterval));
		info.AddProperty(PropertyName.maxIntervalTriggersPerFrame, Variant.From<int>(maxIntervalTriggersPerFrame));
		info.AddProperty(PropertyName.targetEffectType, Variant.From<string>(targetEffectType));
		info.AddProperty(PropertyName.targetEffectScene, Variant.From<PackedScene>(targetEffectScene));
		info.AddProperty(PropertyName.targetEffectSpawnPolicy, Variant.From<PeriodicAreaEventComponent.TargetEffectSpawnPolicy>(targetEffectSpawnPolicy));
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.checkAllLine, Variant.From<bool>(checkAllLine));
		info.AddProperty(PropertyName.skipVases, Variant.From<bool>(skipVases));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.attackMethod, out var value))
		{
			attackMethod = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value2))
		{
			eventList = value2.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.staticTime, out var value3))
		{
			staticTime = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.dpsEffectInterval, out var value4))
		{
			dpsEffectInterval = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.maxIntervalTriggersPerFrame, out var value5))
		{
			maxIntervalTriggersPerFrame = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.targetEffectType, out var value6))
		{
			targetEffectType = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.targetEffectScene, out var value7))
		{
			targetEffectScene = value7.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.targetEffectSpawnPolicy, out var value8))
		{
			targetEffectSpawnPolicy = value8.As<PeriodicAreaEventComponent.TargetEffectSpawnPolicy>();
		}
		if (info.TryGetProperty(PropertyName.checkShape, out var value9))
		{
			checkShape = value9.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.checkAllLine, out var value10))
		{
			checkAllLine = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.skipVases, out var value11))
		{
			skipVases = value11.As<bool>();
		}
	}
}
