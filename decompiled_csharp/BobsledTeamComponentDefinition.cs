using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BobsledTeamComponent/BobsledTeamComponentDefinition.cs")]
public class BobsledTeamComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName slotCount = "slotCount";

		public static readonly StringName passengerPacketNames = "passengerPacketNames";

		public static readonly StringName passengerMarkerPaths = "passengerMarkerPaths";

		public static readonly StringName passengerOffsets = "passengerOffsets";

		public static readonly StringName hideAttachedPassengers = "hideAttachedPassengers";

		public static readonly StringName passengerHeightOffset = "passengerHeightOffset";

		public static readonly StringName autoSpawnPassengers = "autoSpawnPassengers";

		public static readonly StringName ownerDestroyMode = "ownerDestroyMode";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "1,4,1")]
	public int slotCount { get; set; } = 4;

	[Export(PropertyHint.None, "")]
	public Array<string> passengerPacketNames { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<NodePath> passengerMarkerPaths { get; set; } = new Array<NodePath>();

	[Export(PropertyHint.None, "")]
	public Array<Vector2> passengerOffsets { get; set; } = new Array<Vector2>();

	[Export(PropertyHint.None, "")]
	public bool hideAttachedPassengers { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public double passengerHeightOffset { get; set; } = 20.0;

	[Export(PropertyHint.None, "")]
	public bool autoSpawnPassengers { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public BobsledOwnerDestroyMode ownerDestroyMode { get; set; }

	public BobsledTeamComponentDefinition()
	{
		ComponentTypeId = "BobsledTeamComponent";
		InstanceId = "character.bobsled_team";
		WireIndex = 0;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new BobsledTeamComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.slotCount)
		{
			slotCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.passengerPacketNames)
		{
			passengerPacketNames = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.passengerMarkerPaths)
		{
			passengerMarkerPaths = VariantUtils.ConvertToArray<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.passengerOffsets)
		{
			passengerOffsets = VariantUtils.ConvertToArray<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.hideAttachedPassengers)
		{
			hideAttachedPassengers = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.passengerHeightOffset)
		{
			passengerHeightOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.autoSpawnPassengers)
		{
			autoSpawnPassengers = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ownerDestroyMode)
		{
			ownerDestroyMode = VariantUtils.ConvertTo<BobsledOwnerDestroyMode>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.slotCount)
		{
			value = VariantUtils.CreateFrom<int>(slotCount);
			return true;
		}
		if (name == PropertyName.passengerPacketNames)
		{
			value = VariantUtils.CreateFromArray(passengerPacketNames);
			return true;
		}
		if (name == PropertyName.passengerMarkerPaths)
		{
			value = VariantUtils.CreateFromArray(passengerMarkerPaths);
			return true;
		}
		if (name == PropertyName.passengerOffsets)
		{
			value = VariantUtils.CreateFromArray(passengerOffsets);
			return true;
		}
		bool from;
		if (name == PropertyName.hideAttachedPassengers)
		{
			from = hideAttachedPassengers;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.passengerHeightOffset)
		{
			value = VariantUtils.CreateFrom<double>(passengerHeightOffset);
			return true;
		}
		if (name == PropertyName.autoSpawnPassengers)
		{
			from = autoSpawnPassengers;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ownerDestroyMode)
		{
			value = VariantUtils.CreateFrom<BobsledOwnerDestroyMode>(ownerDestroyMode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.slotCount, PropertyHint.Range, "1,4,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.passengerPacketNames, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.passengerMarkerPaths, PropertyHint.TypeString, "22/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.passengerOffsets, PropertyHint.TypeString, "5/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideAttachedPassengers, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.passengerHeightOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoSpawnPassengers, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ownerDestroyMode, PropertyHint.Enum, "Release,Cascade", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.slotCount, Variant.From<int>(slotCount));
		info.AddProperty(PropertyName.passengerPacketNames, Variant.CreateFrom(passengerPacketNames));
		info.AddProperty(PropertyName.passengerMarkerPaths, Variant.CreateFrom(passengerMarkerPaths));
		info.AddProperty(PropertyName.passengerOffsets, Variant.CreateFrom(passengerOffsets));
		info.AddProperty(PropertyName.hideAttachedPassengers, Variant.From<bool>(hideAttachedPassengers));
		info.AddProperty(PropertyName.passengerHeightOffset, Variant.From<double>(passengerHeightOffset));
		info.AddProperty(PropertyName.autoSpawnPassengers, Variant.From<bool>(autoSpawnPassengers));
		info.AddProperty(PropertyName.ownerDestroyMode, Variant.From<BobsledOwnerDestroyMode>(ownerDestroyMode));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.slotCount, out var value))
		{
			slotCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.passengerPacketNames, out var value2))
		{
			passengerPacketNames = value2.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.passengerMarkerPaths, out var value3))
		{
			passengerMarkerPaths = value3.AsGodotArray<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.passengerOffsets, out var value4))
		{
			passengerOffsets = value4.AsGodotArray<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.hideAttachedPassengers, out var value5))
		{
			hideAttachedPassengers = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.passengerHeightOffset, out var value6))
		{
			passengerHeightOffset = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.autoSpawnPassengers, out var value7))
		{
			autoSpawnPassengers = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ownerDestroyMode, out var value8))
		{
			ownerDestroyMode = value8.As<BobsledOwnerDestroyMode>();
		}
	}
}
