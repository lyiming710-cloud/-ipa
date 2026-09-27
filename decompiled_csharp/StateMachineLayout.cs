using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/ResourceEditing/StateMachineLayout.cs")]
public class StateMachineLayout : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Positions = "Positions";

		public static readonly StringName Collapsed = "Collapsed";

		public static readonly StringName Comments = "Comments";

		public static readonly StringName ScrollOffset = "ScrollOffset";

		public static readonly StringName Zoom = "Zoom";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, Vector2> Positions { get; set; } = new Godot.Collections.Dictionary<string, Vector2>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, bool> Collapsed { get; set; } = new Godot.Collections.Dictionary<string, bool>();

	[Export(PropertyHint.None, "")]
	public Array<StateMachineLayoutComment> Comments { get; set; } = new Array<StateMachineLayoutComment>();

	[Export(PropertyHint.None, "")]
	public Vector2 ScrollOffset { get; set; }

	[Export(PropertyHint.None, "")]
	public float Zoom { get; set; } = 1f;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Positions)
		{
			Positions = VariantUtils.ConvertToDictionary<string, Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Collapsed)
		{
			Collapsed = VariantUtils.ConvertToDictionary<string, bool>(in value);
			return true;
		}
		if (name == PropertyName.Comments)
		{
			Comments = VariantUtils.ConvertToArray<StateMachineLayoutComment>(in value);
			return true;
		}
		if (name == PropertyName.ScrollOffset)
		{
			ScrollOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Zoom)
		{
			Zoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Positions)
		{
			value = VariantUtils.CreateFromDictionary(Positions);
			return true;
		}
		if (name == PropertyName.Collapsed)
		{
			value = VariantUtils.CreateFromDictionary(Collapsed);
			return true;
		}
		if (name == PropertyName.Comments)
		{
			value = VariantUtils.CreateFromArray(Comments);
			return true;
		}
		if (name == PropertyName.ScrollOffset)
		{
			value = VariantUtils.CreateFrom<Vector2>(ScrollOffset);
			return true;
		}
		if (name == PropertyName.Zoom)
		{
			value = VariantUtils.CreateFrom<float>(Zoom);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.Positions, PropertyHint.TypeString, "4/0:;5/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.Collapsed, PropertyHint.TypeString, "4/0:;1/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Comments, PropertyHint.TypeString, "24/17:Resource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.ScrollOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.Zoom, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Positions, Variant.CreateFrom(Positions));
		info.AddProperty(PropertyName.Collapsed, Variant.CreateFrom(Collapsed));
		info.AddProperty(PropertyName.Comments, Variant.CreateFrom(Comments));
		info.AddProperty(PropertyName.ScrollOffset, Variant.From<Vector2>(ScrollOffset));
		info.AddProperty(PropertyName.Zoom, Variant.From<float>(Zoom));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Positions, out var value))
		{
			Positions = value.AsGodotDictionary<string, Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Collapsed, out var value2))
		{
			Collapsed = value2.AsGodotDictionary<string, bool>();
		}
		if (info.TryGetProperty(PropertyName.Comments, out var value3))
		{
			Comments = value3.AsGodotArray<StateMachineLayoutComment>();
		}
		if (info.TryGetProperty(PropertyName.ScrollOffset, out var value4))
		{
			ScrollOffset = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Zoom, out var value5))
		{
			Zoom = value5.As<float>();
		}
	}
}
