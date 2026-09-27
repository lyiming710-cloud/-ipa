using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/GroundMoveComponent/GroundMoveComponentDefinition.cs")]
public class GroundMoveComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName groundLayerName = "groundLayerName";

		public static readonly StringName groundLayerOffset = "groundLayerOffset";

		public static readonly StringName groundSlotPath = "groundSlotPath";

		public static readonly StringName delay = "delay";

		public static readonly StringName moveYAxis = "moveYAxis";

		public static readonly StringName syncNetworkPosition = "syncNetworkPosition";

		public static readonly StringName networkSnapDistance = "networkSnapDistance";

		public static readonly StringName networkCorrectionWeight = "networkCorrectionWeight";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public NodePath groundSlotPath = new NodePath("GroundSlot");

	[Export(PropertyHint.None, "")]
	public float delay = 0.2f;

	[Export(PropertyHint.None, "")]
	public bool moveYAxis;

	[Export(PropertyHint.None, "")]
	public bool syncNetworkPosition = true;

	[Export(PropertyHint.Range, "0,512,0.1,or_greater")]
	public float networkSnapDistance = 32f;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public float networkCorrectionWeight = 0.35f;

	[Export(PropertyHint.None, "")]
	public StringName groundLayerName { get; set; }

	[Export(PropertyHint.None, "")]
	public Vector2 groundLayerOffset { get; set; } = Vector2.Zero;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new GroundMoveComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.groundLayerName)
		{
			groundLayerName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.groundLayerOffset)
		{
			groundLayerOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.groundSlotPath)
		{
			groundSlotPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.delay)
		{
			delay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.moveYAxis)
		{
			moveYAxis = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.syncNetworkPosition)
		{
			syncNetworkPosition = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.networkSnapDistance)
		{
			networkSnapDistance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.networkCorrectionWeight)
		{
			networkCorrectionWeight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.groundLayerName)
		{
			value = VariantUtils.CreateFrom<StringName>(groundLayerName);
			return true;
		}
		if (name == PropertyName.groundLayerOffset)
		{
			value = VariantUtils.CreateFrom<Vector2>(groundLayerOffset);
			return true;
		}
		if (name == PropertyName.groundSlotPath)
		{
			value = VariantUtils.CreateFrom(in groundSlotPath);
			return true;
		}
		if (name == PropertyName.delay)
		{
			value = VariantUtils.CreateFrom(in delay);
			return true;
		}
		if (name == PropertyName.moveYAxis)
		{
			value = VariantUtils.CreateFrom(in moveYAxis);
			return true;
		}
		if (name == PropertyName.syncNetworkPosition)
		{
			value = VariantUtils.CreateFrom(in syncNetworkPosition);
			return true;
		}
		if (name == PropertyName.networkSnapDistance)
		{
			value = VariantUtils.CreateFrom(in networkSnapDistance);
			return true;
		}
		if (name == PropertyName.networkCorrectionWeight)
		{
			value = VariantUtils.CreateFrom(in networkCorrectionWeight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.groundLayerName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.groundLayerOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.groundSlotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.delay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.moveYAxis, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.syncNetworkPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.networkSnapDistance, PropertyHint.Range, "0,512,0.1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.networkCorrectionWeight, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.groundLayerName, Variant.From<StringName>(groundLayerName));
		info.AddProperty(PropertyName.groundLayerOffset, Variant.From<Vector2>(groundLayerOffset));
		info.AddProperty(PropertyName.groundSlotPath, Variant.From(in groundSlotPath));
		info.AddProperty(PropertyName.delay, Variant.From(in delay));
		info.AddProperty(PropertyName.moveYAxis, Variant.From(in moveYAxis));
		info.AddProperty(PropertyName.syncNetworkPosition, Variant.From(in syncNetworkPosition));
		info.AddProperty(PropertyName.networkSnapDistance, Variant.From(in networkSnapDistance));
		info.AddProperty(PropertyName.networkCorrectionWeight, Variant.From(in networkCorrectionWeight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.groundLayerName, out var value))
		{
			groundLayerName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.groundLayerOffset, out var value2))
		{
			groundLayerOffset = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.groundSlotPath, out var value3))
		{
			groundSlotPath = value3.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.delay, out var value4))
		{
			delay = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.moveYAxis, out var value5))
		{
			moveYAxis = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.syncNetworkPosition, out var value6))
		{
			syncNetworkPosition = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.networkSnapDistance, out var value7))
		{
			networkSnapDistance = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.networkCorrectionWeight, out var value8))
		{
			networkCorrectionWeight = value8.As<float>();
		}
	}
}
