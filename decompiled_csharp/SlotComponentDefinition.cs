using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/SlotComponent/SlotComponentDefinition.cs")]
public class SlotComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName posMarkPath = "posMarkPath";

		public static readonly StringName heightFollow = "heightFollow";

		public static readonly StringName hideShadow = "hideShadow";

		public static readonly StringName occupantRefreshInterval = "occupantRefreshInterval";

		public static readonly StringName restoreGroundHeightOnRelease = "restoreGroundHeightOnRelease";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public NodePath posMarkPath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public bool heightFollow { get; set; }

	[Export(PropertyHint.None, "")]
	public bool hideShadow { get; set; } = true;

	[Export(PropertyHint.Range, "1,30,1")]
	public int occupantRefreshInterval { get; set; } = 3;

	[Export(PropertyHint.None, "")]
	public bool restoreGroundHeightOnRelease { get; set; } = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new SlotComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.posMarkPath)
		{
			posMarkPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.heightFollow)
		{
			heightFollow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hideShadow)
		{
			hideShadow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.occupantRefreshInterval)
		{
			occupantRefreshInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.restoreGroundHeightOnRelease)
		{
			restoreGroundHeightOnRelease = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.posMarkPath)
		{
			value = VariantUtils.CreateFrom<NodePath>(posMarkPath);
			return true;
		}
		bool from;
		if (name == PropertyName.heightFollow)
		{
			from = heightFollow;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.hideShadow)
		{
			from = hideShadow;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.occupantRefreshInterval)
		{
			value = VariantUtils.CreateFrom<int>(occupantRefreshInterval);
			return true;
		}
		if (name == PropertyName.restoreGroundHeightOnRelease)
		{
			from = restoreGroundHeightOnRelease;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.posMarkPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.heightFollow, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideShadow, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.occupantRefreshInterval, PropertyHint.Range, "1,30,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.restoreGroundHeightOnRelease, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.posMarkPath, Variant.From<NodePath>(posMarkPath));
		info.AddProperty(PropertyName.heightFollow, Variant.From<bool>(heightFollow));
		info.AddProperty(PropertyName.hideShadow, Variant.From<bool>(hideShadow));
		info.AddProperty(PropertyName.occupantRefreshInterval, Variant.From<int>(occupantRefreshInterval));
		info.AddProperty(PropertyName.restoreGroundHeightOnRelease, Variant.From<bool>(restoreGroundHeightOnRelease));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.posMarkPath, out var value))
		{
			posMarkPath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.heightFollow, out var value2))
		{
			heightFollow = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hideShadow, out var value3))
		{
			hideShadow = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.occupantRefreshInterval, out var value4))
		{
			occupantRefreshInterval = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.restoreGroundHeightOnRelease, out var value5))
		{
			restoreGroundHeightOnRelease = value5.As<bool>();
		}
	}
}
