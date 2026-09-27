using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketPick/Resource/TowerDefenseBattleFeaturePacketPickConfig.cs")]
public class TowerDefenseBattleFeaturePacketPickConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName ClampFinite = "ClampFinite";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName pendingRequestTimeoutSeconds = "pendingRequestTimeoutSeconds";

		public static readonly StringName pendingRequestSweepIntervalSeconds = "pendingRequestSweepIntervalSeconds";

		public static readonly StringName packetSelectionDebounceFrames = "packetSelectionDebounceFrames";

		public static readonly StringName toolActivationGraceFrames = "toolActivationGraceFrames";

		public static readonly StringName characterTargetRadiusScale = "characterTargetRadiusScale";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.Range, "1,120,0.5,or_greater")]
	public double pendingRequestTimeoutSeconds = 15.0;

	[Export(PropertyHint.Range, "0.05,5,0.05,or_greater")]
	public double pendingRequestSweepIntervalSeconds = 0.5;

	[Export(PropertyHint.Range, "0,30,1,or_greater")]
	public int packetSelectionDebounceFrames = 5;

	[Export(PropertyHint.Range, "0,30,1,or_greater")]
	public int toolActivationGraceFrames = 5;

	[Export(PropertyHint.Range, "0.05,2,0.05,or_greater")]
	public double characterTargetRadiusScale = 0.6;

	public void Init(Dictionary data)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		pendingRequestTimeoutSeconds = ClampFinite(data.GetValueOrDefault("PendingRequestTimeoutSeconds", 15.0).AsDouble(), 1.0, 120.0, 15.0);
		pendingRequestSweepIntervalSeconds = ClampFinite(data.GetValueOrDefault("PendingRequestSweepIntervalSeconds", 0.5).AsDouble(), 0.05, 5.0, 0.5);
		packetSelectionDebounceFrames = Math.Clamp(data.GetValueOrDefault("PacketSelectionDebounceFrames", 5).AsInt32(), 0, 30);
		toolActivationGraceFrames = Math.Clamp(data.GetValueOrDefault("ToolActivationGraceFrames", 5).AsInt32(), 0, 30);
		characterTargetRadiusScale = ClampFinite(data.GetValueOrDefault("CharacterTargetRadiusScale", 0.6).AsDouble(), 0.05, 2.0, 0.6);
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["PendingRequestTimeoutSeconds"] = pendingRequestTimeoutSeconds,
			["PendingRequestSweepIntervalSeconds"] = pendingRequestSweepIntervalSeconds,
			["PacketSelectionDebounceFrames"] = packetSelectionDebounceFrames,
			["ToolActivationGraceFrames"] = toolActivationGraceFrames,
			["CharacterTargetRadiusScale"] = characterTargetRadiusScale
		};
	}

	private static double ClampFinite(double value, double minimum, double maximum, double fallback)
	{
		if (!double.IsFinite(value))
		{
			return fallback;
		}
		return Math.Clamp(value, minimum, maximum);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampFinite, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.ClampFinite && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ClampFinite(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClampFinite && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ClampFinite(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.ClampFinite)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pendingRequestTimeoutSeconds)
		{
			pendingRequestTimeoutSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.pendingRequestSweepIntervalSeconds)
		{
			pendingRequestSweepIntervalSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetSelectionDebounceFrames)
		{
			packetSelectionDebounceFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.toolActivationGraceFrames)
		{
			toolActivationGraceFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.characterTargetRadiusScale)
		{
			characterTargetRadiusScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.pendingRequestTimeoutSeconds)
		{
			value = VariantUtils.CreateFrom(in pendingRequestTimeoutSeconds);
			return true;
		}
		if (name == PropertyName.pendingRequestSweepIntervalSeconds)
		{
			value = VariantUtils.CreateFrom(in pendingRequestSweepIntervalSeconds);
			return true;
		}
		if (name == PropertyName.packetSelectionDebounceFrames)
		{
			value = VariantUtils.CreateFrom(in packetSelectionDebounceFrames);
			return true;
		}
		if (name == PropertyName.toolActivationGraceFrames)
		{
			value = VariantUtils.CreateFrom(in toolActivationGraceFrames);
			return true;
		}
		if (name == PropertyName.characterTargetRadiusScale)
		{
			value = VariantUtils.CreateFrom(in characterTargetRadiusScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.pendingRequestTimeoutSeconds, PropertyHint.Range, "1,120,0.5,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pendingRequestSweepIntervalSeconds, PropertyHint.Range, "0.05,5,0.05,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.packetSelectionDebounceFrames, PropertyHint.Range, "0,30,1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.toolActivationGraceFrames, PropertyHint.Range, "0,30,1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.characterTargetRadiusScale, PropertyHint.Range, "0.05,2,0.05,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pendingRequestTimeoutSeconds, Variant.From(in pendingRequestTimeoutSeconds));
		info.AddProperty(PropertyName.pendingRequestSweepIntervalSeconds, Variant.From(in pendingRequestSweepIntervalSeconds));
		info.AddProperty(PropertyName.packetSelectionDebounceFrames, Variant.From(in packetSelectionDebounceFrames));
		info.AddProperty(PropertyName.toolActivationGraceFrames, Variant.From(in toolActivationGraceFrames));
		info.AddProperty(PropertyName.characterTargetRadiusScale, Variant.From(in characterTargetRadiusScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pendingRequestTimeoutSeconds, out var value))
		{
			pendingRequestTimeoutSeconds = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.pendingRequestSweepIntervalSeconds, out var value2))
		{
			pendingRequestSweepIntervalSeconds = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetSelectionDebounceFrames, out var value3))
		{
			packetSelectionDebounceFrames = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.toolActivationGraceFrames, out var value4))
		{
			toolActivationGraceFrames = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.characterTargetRadiusScale, out var value5))
		{
			characterTargetRadiusScale = value5.As<double>();
		}
	}
}
