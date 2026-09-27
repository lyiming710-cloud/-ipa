using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Tests/CharacterComponentProbeDefinition.cs")]
public class CharacterComponentProbeDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName ProbePower = "ProbePower";

		public static readonly StringName ProbeMode = "ProbeMode";

		public static readonly StringName ProbeTags = "ProbeTags";

		public static readonly StringName ProbeWeights = "ProbeWeights";

		public static readonly StringName ProbePackedFrames = "ProbePackedFrames";

		public static readonly StringName ProbeOffset = "ProbeOffset";

		public static readonly StringName ProbeTint = "ProbeTint";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,99,1")]
	public int ProbePower { get; set; } = 17;

	[Export(PropertyHint.None, "")]
	public CharacterComponentProbeMode ProbeMode { get; set; } = CharacterComponentProbeMode.Armed;

	[Export(PropertyHint.None, "")]
	public Array<string> ProbeTags { get; set; } = new Array<string> { "alpha", "beta" };

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, int> ProbeWeights { get; set; } = new Godot.Collections.Dictionary<string, int> { ["initial"] = 7 };

	[Export(PropertyHint.None, "")]
	public int[] ProbePackedFrames { get; set; } = new int[3] { 2, 4, 8 };

	[Export(PropertyHint.None, "")]
	public Vector2 ProbeOffset { get; set; } = new Vector2(12.5f, -7.25f);

	[Export(PropertyHint.None, "")]
	public Color ProbeTint { get; set; } = new Color(0.2f, 0.4f, 0.8f, 0.65f);

	public static int RuntimeCreateCount { get; private set; }

	public static CharacterComponentProbeRuntime LastRuntime { get; private set; }

	public override CharacterComponentRuntime CreateRuntime()
	{
		RuntimeCreateCount++;
		LastRuntime = new CharacterComponentProbeRuntime();
		return LastRuntime;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProbePower)
		{
			ProbePower = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ProbeMode)
		{
			ProbeMode = VariantUtils.ConvertTo<CharacterComponentProbeMode>(in value);
			return true;
		}
		if (name == PropertyName.ProbeTags)
		{
			ProbeTags = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.ProbeWeights)
		{
			ProbeWeights = VariantUtils.ConvertToDictionary<string, int>(in value);
			return true;
		}
		if (name == PropertyName.ProbePackedFrames)
		{
			ProbePackedFrames = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.ProbeOffset)
		{
			ProbeOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.ProbeTint)
		{
			ProbeTint = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ProbePower)
		{
			value = VariantUtils.CreateFrom<int>(ProbePower);
			return true;
		}
		if (name == PropertyName.ProbeMode)
		{
			value = VariantUtils.CreateFrom<CharacterComponentProbeMode>(ProbeMode);
			return true;
		}
		if (name == PropertyName.ProbeTags)
		{
			value = VariantUtils.CreateFromArray(ProbeTags);
			return true;
		}
		if (name == PropertyName.ProbeWeights)
		{
			value = VariantUtils.CreateFromDictionary(ProbeWeights);
			return true;
		}
		if (name == PropertyName.ProbePackedFrames)
		{
			value = VariantUtils.CreateFrom<int[]>(ProbePackedFrames);
			return true;
		}
		if (name == PropertyName.ProbeOffset)
		{
			value = VariantUtils.CreateFrom<Vector2>(ProbeOffset);
			return true;
		}
		if (name == PropertyName.ProbeTint)
		{
			value = VariantUtils.CreateFrom<Color>(ProbeTint);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ProbePower, PropertyHint.Range, "0,99,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ProbeMode, PropertyHint.Enum, "Idle,Armed,Triggered", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.ProbeTags, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.ProbeWeights, PropertyHint.TypeString, "4/0:;2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.ProbePackedFrames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.ProbeOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.ProbeTint, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProbePower, Variant.From<int>(ProbePower));
		info.AddProperty(PropertyName.ProbeMode, Variant.From<CharacterComponentProbeMode>(ProbeMode));
		info.AddProperty(PropertyName.ProbeTags, Variant.CreateFrom(ProbeTags));
		info.AddProperty(PropertyName.ProbeWeights, Variant.CreateFrom(ProbeWeights));
		info.AddProperty(PropertyName.ProbePackedFrames, Variant.From<int[]>(ProbePackedFrames));
		info.AddProperty(PropertyName.ProbeOffset, Variant.From<Vector2>(ProbeOffset));
		info.AddProperty(PropertyName.ProbeTint, Variant.From<Color>(ProbeTint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProbePower, out var value))
		{
			ProbePower = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ProbeMode, out var value2))
		{
			ProbeMode = value2.As<CharacterComponentProbeMode>();
		}
		if (info.TryGetProperty(PropertyName.ProbeTags, out var value3))
		{
			ProbeTags = value3.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.ProbeWeights, out var value4))
		{
			ProbeWeights = value4.AsGodotDictionary<string, int>();
		}
		if (info.TryGetProperty(PropertyName.ProbePackedFrames, out var value5))
		{
			ProbePackedFrames = value5.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.ProbeOffset, out var value6))
		{
			ProbeOffset = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.ProbeTint, out var value7))
		{
			ProbeTint = value7.As<Color>();
		}
	}
}
