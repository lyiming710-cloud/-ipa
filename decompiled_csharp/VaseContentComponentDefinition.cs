using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/VaseContentComponent/VaseContentComponentDefinition.cs")]
public class VaseContentComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName playBreakAudio = "playBreakAudio";

		public static readonly StringName breakAudio = "breakAudio";

		public static readonly StringName breakEffectOffset = "breakEffectOffset";

		public static readonly StringName packetHorizontalSpeedRange = "packetHorizontalSpeedRange";

		public static readonly StringName packetVerticalSpeed = "packetVerticalSpeed";

		public static readonly StringName packetGravity = "packetGravity";

		public static readonly StringName packetAliveTime = "packetAliveTime";

		public static readonly StringName readyWaitFrameLimit = "readyWaitFrameLimit";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Break Visual", "")]
	[Export(PropertyHint.None, "")]
	public bool playBreakAudio = true;

	[Export(PropertyHint.None, "")]
	public string breakAudio = "VaseBreaking";

	[Export(PropertyHint.None, "")]
	public Vector2 breakEffectOffset = new Vector2(0f, -30f);

	[ExportGroup("Packet Drop", "")]
	[Export(PropertyHint.None, "")]
	public Vector2 packetHorizontalSpeedRange = new Vector2(30f, 50f);

	[Export(PropertyHint.None, "")]
	public float packetVerticalSpeed = -300f;

	[Export(PropertyHint.None, "")]
	public float packetGravity = 980f;

	[Export(PropertyHint.Range, "0,120,0.1")]
	public float packetAliveTime = 15f;

	[Export(PropertyHint.Range, "1,30,1")]
	public int readyWaitFrameLimit = 2;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new VaseContentComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.playBreakAudio)
		{
			playBreakAudio = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.breakAudio)
		{
			breakAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.breakEffectOffset)
		{
			breakEffectOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.packetHorizontalSpeedRange)
		{
			packetHorizontalSpeedRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.packetVerticalSpeed)
		{
			packetVerticalSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.packetGravity)
		{
			packetGravity = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.packetAliveTime)
		{
			packetAliveTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.readyWaitFrameLimit)
		{
			readyWaitFrameLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.playBreakAudio)
		{
			value = VariantUtils.CreateFrom(in playBreakAudio);
			return true;
		}
		if (name == PropertyName.breakAudio)
		{
			value = VariantUtils.CreateFrom(in breakAudio);
			return true;
		}
		if (name == PropertyName.breakEffectOffset)
		{
			value = VariantUtils.CreateFrom(in breakEffectOffset);
			return true;
		}
		if (name == PropertyName.packetHorizontalSpeedRange)
		{
			value = VariantUtils.CreateFrom(in packetHorizontalSpeedRange);
			return true;
		}
		if (name == PropertyName.packetVerticalSpeed)
		{
			value = VariantUtils.CreateFrom(in packetVerticalSpeed);
			return true;
		}
		if (name == PropertyName.packetGravity)
		{
			value = VariantUtils.CreateFrom(in packetGravity);
			return true;
		}
		if (name == PropertyName.packetAliveTime)
		{
			value = VariantUtils.CreateFrom(in packetAliveTime);
			return true;
		}
		if (name == PropertyName.readyWaitFrameLimit)
		{
			value = VariantUtils.CreateFrom(in readyWaitFrameLimit);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Break Visual", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.playBreakAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.breakAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.breakEffectOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Packet Drop", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.packetHorizontalSpeedRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetVerticalSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetGravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetAliveTime, PropertyHint.Range, "0,120,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.readyWaitFrameLimit, PropertyHint.Range, "1,30,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.playBreakAudio, Variant.From(in playBreakAudio));
		info.AddProperty(PropertyName.breakAudio, Variant.From(in breakAudio));
		info.AddProperty(PropertyName.breakEffectOffset, Variant.From(in breakEffectOffset));
		info.AddProperty(PropertyName.packetHorizontalSpeedRange, Variant.From(in packetHorizontalSpeedRange));
		info.AddProperty(PropertyName.packetVerticalSpeed, Variant.From(in packetVerticalSpeed));
		info.AddProperty(PropertyName.packetGravity, Variant.From(in packetGravity));
		info.AddProperty(PropertyName.packetAliveTime, Variant.From(in packetAliveTime));
		info.AddProperty(PropertyName.readyWaitFrameLimit, Variant.From(in readyWaitFrameLimit));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.playBreakAudio, out var value))
		{
			playBreakAudio = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.breakAudio, out var value2))
		{
			breakAudio = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.breakEffectOffset, out var value3))
		{
			breakEffectOffset = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.packetHorizontalSpeedRange, out var value4))
		{
			packetHorizontalSpeedRange = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.packetVerticalSpeed, out var value5))
		{
			packetVerticalSpeed = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.packetGravity, out var value6))
		{
			packetGravity = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.packetAliveTime, out var value7))
		{
			packetAliveTime = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.readyWaitFrameLimit, out var value8))
		{
			readyWaitFrameLimit = value8.As<int>();
		}
	}
}
