using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ProduceComponent/ProduceComponentDefinition.cs")]
public class ProduceComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName _IZMMode = "_IZMMode";

		public static readonly StringName produceType = "produceType";

		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName num = "num";

		public static readonly StringName sunOnceMax = "sunOnceMax";

		public static readonly StringName markerPaths = "markerPaths";

		public static readonly StringName onlyEmit = "onlyEmit";

		public static readonly StringName produceGlowTargetPath = "produceGlowTargetPath";

		public static readonly StringName initialDelayRange = "initialDelayRange";

		public static readonly StringName glowLeadTime = "glowLeadTime";

		public static readonly StringName healthProductionSegments = "healthProductionSegments";

		public static readonly StringName maxCatchUpProductions = "maxCatchUpProductions";

		public static readonly StringName glowBrightness = "glowBrightness";

		public static readonly StringName glowFadeInTime = "glowFadeInTime";

		public static readonly StringName glowFadeOutTime = "glowFadeOutTime";

		public static readonly StringName coinRandom = "coinRandom";

		public static readonly StringName packetName = "packetName";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Production", "")]
	[Export(PropertyHint.None, "")]
	public bool _IZMMode { get; set; }

	[Export(PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,Packet,QXSun,MagicSun")]
	public string produceType { get; set; } = "Sun";

	[Export(PropertyHint.Range, "0,300,0.1,or_greater")]
	public float produceInterval { get; set; } = 25f;

	[Export(PropertyHint.None, "")]
	public int num { get; set; } = 25;

	[Export(PropertyHint.Range, "1,1000,1,or_greater")]
	public int sunOnceMax { get; set; } = 50;

	[Export(PropertyHint.None, "")]
	public Array<NodePath> markerPaths { get; set; } = new Array<NodePath>();

	[Export(PropertyHint.None, "")]
	public bool onlyEmit { get; set; }

	[Export(PropertyHint.None, "")]
	public NodePath produceGlowTargetPath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public Vector2 initialDelayRange { get; set; } = new Vector2(3f, 6f);

	[Export(PropertyHint.Range, "0,60,0.1,or_greater")]
	public float glowLeadTime { get; set; } = 1.5f;

	[Export(PropertyHint.Range, "1,1000,1")]
	public int healthProductionSegments { get; set; } = 6;

	[Export(PropertyHint.Range, "1,120,1")]
	public int maxCatchUpProductions { get; set; } = 1;

	[ExportGroup("Glow", "")]
	[Export(PropertyHint.None, "")]
	public float glowBrightness { get; set; } = 0.5f;

	[Export(PropertyHint.None, "")]
	public float glowFadeInTime { get; set; } = 1.5f;

	[Export(PropertyHint.None, "")]
	public float glowFadeOutTime { get; set; } = 0.5f;

	[ExportGroup("Coin", "")]
	[Export(PropertyHint.None, "")]
	public bool coinRandom { get; set; } = true;

	[ExportGroup("Packet", "")]
	[Export(PropertyHint.None, "")]
	public Array<string> packetName { get; set; } = new Array<string>();

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ProduceComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._IZMMode)
		{
			_IZMMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.produceInterval)
		{
			produceInterval = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.sunOnceMax)
		{
			sunOnceMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.markerPaths)
		{
			markerPaths = VariantUtils.ConvertToArray<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.onlyEmit)
		{
			onlyEmit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.produceGlowTargetPath)
		{
			produceGlowTargetPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.initialDelayRange)
		{
			initialDelayRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.glowLeadTime)
		{
			glowLeadTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.healthProductionSegments)
		{
			healthProductionSegments = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maxCatchUpProductions)
		{
			maxCatchUpProductions = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.glowBrightness)
		{
			glowBrightness = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.glowFadeInTime)
		{
			glowFadeInTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.glowFadeOutTime)
		{
			glowFadeOutTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.coinRandom)
		{
			coinRandom = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			packetName = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName._IZMMode)
		{
			from = _IZMMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			value = VariantUtils.CreateFrom<string>(produceType);
			return true;
		}
		float from2;
		if (name == PropertyName.produceInterval)
		{
			from2 = produceInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		int from3;
		if (name == PropertyName.num)
		{
			from3 = num;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.sunOnceMax)
		{
			from3 = sunOnceMax;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.markerPaths)
		{
			value = VariantUtils.CreateFromArray(markerPaths);
			return true;
		}
		if (name == PropertyName.onlyEmit)
		{
			from = onlyEmit;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.produceGlowTargetPath)
		{
			value = VariantUtils.CreateFrom<NodePath>(produceGlowTargetPath);
			return true;
		}
		if (name == PropertyName.initialDelayRange)
		{
			value = VariantUtils.CreateFrom<Vector2>(initialDelayRange);
			return true;
		}
		if (name == PropertyName.glowLeadTime)
		{
			from2 = glowLeadTime;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.healthProductionSegments)
		{
			from3 = healthProductionSegments;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.maxCatchUpProductions)
		{
			from3 = maxCatchUpProductions;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.glowBrightness)
		{
			from2 = glowBrightness;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.glowFadeInTime)
		{
			from2 = glowFadeInTime;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.glowFadeOutTime)
		{
			from2 = glowFadeOutTime;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.coinRandom)
		{
			from = coinRandom;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			value = VariantUtils.CreateFromArray(packetName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Production", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._IZMMode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,Packet,QXSun,MagicSun", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.Range, "0,300,0.1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunOnceMax, PropertyHint.Range, "1,1000,1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.markerPaths, PropertyHint.TypeString, "22/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.onlyEmit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.produceGlowTargetPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.initialDelayRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.glowLeadTime, PropertyHint.Range, "0,60,0.1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.healthProductionSegments, PropertyHint.Range, "1,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxCatchUpProductions, PropertyHint.Range, "1,120,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Glow", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.glowBrightness, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.glowFadeInTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.glowFadeOutTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Coin", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.coinRandom, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Packet", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetName, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._IZMMode, Variant.From<bool>(_IZMMode));
		info.AddProperty(PropertyName.produceType, Variant.From<string>(produceType));
		info.AddProperty(PropertyName.produceInterval, Variant.From<float>(produceInterval));
		info.AddProperty(PropertyName.num, Variant.From<int>(num));
		info.AddProperty(PropertyName.sunOnceMax, Variant.From<int>(sunOnceMax));
		info.AddProperty(PropertyName.markerPaths, Variant.CreateFrom(markerPaths));
		info.AddProperty(PropertyName.onlyEmit, Variant.From<bool>(onlyEmit));
		info.AddProperty(PropertyName.produceGlowTargetPath, Variant.From<NodePath>(produceGlowTargetPath));
		info.AddProperty(PropertyName.initialDelayRange, Variant.From<Vector2>(initialDelayRange));
		info.AddProperty(PropertyName.glowLeadTime, Variant.From<float>(glowLeadTime));
		info.AddProperty(PropertyName.healthProductionSegments, Variant.From<int>(healthProductionSegments));
		info.AddProperty(PropertyName.maxCatchUpProductions, Variant.From<int>(maxCatchUpProductions));
		info.AddProperty(PropertyName.glowBrightness, Variant.From<float>(glowBrightness));
		info.AddProperty(PropertyName.glowFadeInTime, Variant.From<float>(glowFadeInTime));
		info.AddProperty(PropertyName.glowFadeOutTime, Variant.From<float>(glowFadeOutTime));
		info.AddProperty(PropertyName.coinRandom, Variant.From<bool>(coinRandom));
		info.AddProperty(PropertyName.packetName, Variant.CreateFrom(packetName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._IZMMode, out var value))
		{
			_IZMMode = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.produceType, out var value2))
		{
			produceType = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.produceInterval, out var value3))
		{
			produceInterval = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value4))
		{
			num = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.sunOnceMax, out var value5))
		{
			sunOnceMax = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.markerPaths, out var value6))
		{
			markerPaths = value6.AsGodotArray<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.onlyEmit, out var value7))
		{
			onlyEmit = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.produceGlowTargetPath, out var value8))
		{
			produceGlowTargetPath = value8.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.initialDelayRange, out var value9))
		{
			initialDelayRange = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.glowLeadTime, out var value10))
		{
			glowLeadTime = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.healthProductionSegments, out var value11))
		{
			healthProductionSegments = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maxCatchUpProductions, out var value12))
		{
			maxCatchUpProductions = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.glowBrightness, out var value13))
		{
			glowBrightness = value13.As<float>();
		}
		if (info.TryGetProperty(PropertyName.glowFadeInTime, out var value14))
		{
			glowFadeInTime = value14.As<float>();
		}
		if (info.TryGetProperty(PropertyName.glowFadeOutTime, out var value15))
		{
			glowFadeOutTime = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.coinRandom, out var value16))
		{
			coinRandom = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetName, out var value17))
		{
			packetName = value17.AsGodotArray<string>();
		}
	}
}
