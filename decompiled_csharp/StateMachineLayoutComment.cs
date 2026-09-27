using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
public class StateMachineLayoutComment : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName StableId = "StableId";

		public static readonly StringName Text = "Text";

		public static readonly StringName Position = "Position";

		public static readonly StringName Size = "Size";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string StableId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string Text { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public Vector2 Position { get; set; }

	[Export(PropertyHint.None, "")]
	public Vector2 Size { get; set; } = new Vector2(240f, 120f);

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.StableId)
		{
			StableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Text)
		{
			Text = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Position)
		{
			Position = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Size)
		{
			Size = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.StableId)
		{
			from = StableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Text)
		{
			from = Text;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.Position)
		{
			from2 = Position;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.Size)
		{
			from2 = Size;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.StableId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.Text, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Position, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.StableId, Variant.From<string>(StableId));
		info.AddProperty(PropertyName.Text, Variant.From<string>(Text));
		info.AddProperty(PropertyName.Position, Variant.From<Vector2>(Position));
		info.AddProperty(PropertyName.Size, Variant.From<Vector2>(Size));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.StableId, out var value))
		{
			StableId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Text, out var value2))
		{
			Text = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Position, out var value3))
		{
			Position = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Size, out var value4))
		{
			Size = value4.As<Vector2>();
		}
	}
}
