using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Tests/XWDirectPropertyProbeResource.cs")]
public class XWDirectPropertyProbeResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Caption = "Caption";

		public static readonly StringName Enabled = "Enabled";

		public static readonly StringName Count = "Count";

		public static readonly StringName Speed = "Speed";

		public static readonly StringName SmallMode = "SmallMode";

		public static readonly StringName LargeMode = "LargeMode";

		public static readonly StringName Tint = "Tint";

		public static readonly StringName Offset = "Offset";

		public static readonly StringName Icon = "Icon";

		public static readonly StringName Tags = "Tags";

		public static readonly StringName Costs = "Costs";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[ExportGroup("Identity", "")]
	[Export(PropertyHint.None, "")]
	public string Caption { get; set; } = "Original caption";

	[Export(PropertyHint.None, "")]
	public bool Enabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public int Count { get; set; } = 3;

	[Export(PropertyHint.None, "")]
	public float Speed { get; set; } = 1.25f;

	[Export(PropertyHint.Enum, "Idle,Walk,Attack")]
	public int SmallMode { get; set; } = 1;

	[Export(PropertyHint.Enum, "Pea,Sunflower,Wallnut,Cherry,Potato,SnowPea,Chomper,Repeater")]
	public int LargeMode { get; set; } = 2;

	[ExportGroup("Visual", "")]
	[Export(PropertyHint.None, "")]
	public Color Tint { get; set; } = new Color(0.4f, 0.8f, 0.3f);

	[Export(PropertyHint.None, "")]
	public Vector2 Offset { get; set; } = new Vector2(8f, 12f);

	[Export(PropertyHint.None, "")]
	public Texture2D Icon { get; set; }

	[ExportGroup("Collections", "")]
	[Export(PropertyHint.None, "")]
	public Array<string> Tags { get; set; } = new Array<string> { "plant", "probe" };

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, int> Costs { get; set; } = new Godot.Collections.Dictionary<string, int> { ["sun"] = 50 };

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Caption)
		{
			Caption = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Enabled)
		{
			Enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Count)
		{
			Count = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Speed)
		{
			Speed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.SmallMode)
		{
			SmallMode = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LargeMode)
		{
			LargeMode = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Tint)
		{
			Tint = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.Offset)
		{
			Offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			Icon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.Tags)
		{
			Tags = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.Costs)
		{
			Costs = VariantUtils.ConvertToDictionary<string, int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Caption)
		{
			value = VariantUtils.CreateFrom<string>(Caption);
			return true;
		}
		if (name == PropertyName.Enabled)
		{
			value = VariantUtils.CreateFrom<bool>(Enabled);
			return true;
		}
		int from;
		if (name == PropertyName.Count)
		{
			from = Count;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Speed)
		{
			value = VariantUtils.CreateFrom<float>(Speed);
			return true;
		}
		if (name == PropertyName.SmallMode)
		{
			from = SmallMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.LargeMode)
		{
			from = LargeMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Tint)
		{
			value = VariantUtils.CreateFrom<Color>(Tint);
			return true;
		}
		if (name == PropertyName.Offset)
		{
			value = VariantUtils.CreateFrom<Vector2>(Offset);
			return true;
		}
		if (name == PropertyName.Icon)
		{
			value = VariantUtils.CreateFrom<Texture2D>(Icon);
			return true;
		}
		if (name == PropertyName.Tags)
		{
			value = VariantUtils.CreateFromArray(Tags);
			return true;
		}
		if (name == PropertyName.Costs)
		{
			value = VariantUtils.CreateFromDictionary(Costs);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Identity", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.Caption, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Enabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Count, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.Speed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SmallMode, PropertyHint.Enum, "Idle,Walk,Attack", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.LargeMode, PropertyHint.Enum, "Pea,Sunflower,Wallnut,Cherry,Potato,SnowPea,Chomper,Repeater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Visual", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.Tint, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.Icon, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Collections", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.Tags, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.Costs, PropertyHint.TypeString, "4/0:;2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Caption, Variant.From<string>(Caption));
		info.AddProperty(PropertyName.Enabled, Variant.From<bool>(Enabled));
		info.AddProperty(PropertyName.Count, Variant.From<int>(Count));
		info.AddProperty(PropertyName.Speed, Variant.From<float>(Speed));
		info.AddProperty(PropertyName.SmallMode, Variant.From<int>(SmallMode));
		info.AddProperty(PropertyName.LargeMode, Variant.From<int>(LargeMode));
		info.AddProperty(PropertyName.Tint, Variant.From<Color>(Tint));
		info.AddProperty(PropertyName.Offset, Variant.From<Vector2>(Offset));
		info.AddProperty(PropertyName.Icon, Variant.From<Texture2D>(Icon));
		info.AddProperty(PropertyName.Tags, Variant.CreateFrom(Tags));
		info.AddProperty(PropertyName.Costs, Variant.CreateFrom(Costs));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Caption, out var value))
		{
			Caption = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Enabled, out var value2))
		{
			Enabled = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Count, out var value3))
		{
			Count = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Speed, out var value4))
		{
			Speed = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.SmallMode, out var value5))
		{
			SmallMode = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LargeMode, out var value6))
		{
			LargeMode = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Tint, out var value7))
		{
			Tint = value7.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.Offset, out var value8))
		{
			Offset = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Icon, out var value9))
		{
			Icon = value9.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.Tags, out var value10))
		{
			Tags = value10.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.Costs, out var value11))
		{
			Costs = value11.AsGodotDictionary<string, int>();
		}
	}
}
