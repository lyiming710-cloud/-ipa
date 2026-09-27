using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/PotatoComponent/PotatoComponentDefinition.cs")]
public class PotatoComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName readyTime = "readyTime";

		public static readonly StringName readyTimerName = "readyTimerName";

		public static readonly StringName autoRise = "autoRise";

		public static readonly StringName componentStateUse = "componentStateUse";

		public static readonly StringName riseEffect = "riseEffect";

		public static readonly StringName riseAudio = "riseAudio";

		public static readonly StringName createRiseEffect = "createRiseEffect";

		public static readonly StringName setSmashInvincibleOnCharge = "setSmashInvincibleOnCharge";

		public static readonly StringName destroyAfterExplode = "destroyAfterExplode";

		public static readonly StringName autoExplodeOnCharge = "autoExplodeOnCharge";

		public static readonly StringName readyStateEvent = "readyStateEvent";

		public static readonly StringName riseStateEvent = "riseStateEvent";

		public static readonly StringName chargeStateEvent = "chargeStateEvent";

		public static readonly StringName idleStateEvent = "idleStateEvent";

		public static readonly StringName readyAnimeClips = "readyAnimeClips";

		public static readonly StringName readyAnimeTimeScale = "readyAnimeTimeScale";

		public static readonly StringName riseAnimeClips = "riseAnimeClips";

		public static readonly StringName riseAnimeTimeScale = "riseAnimeTimeScale";

		public static readonly StringName chargeAnimeClips = "chargeAnimeClips";

		public static readonly StringName chargeAnimeTimeScale = "chargeAnimeTimeScale";

		public static readonly StringName attackComponentName = "attackComponentName";

		public static readonly StringName explodeComponentName = "explodeComponentName";

		public static readonly StringName spritePath = "spritePath";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.Range, "0,300,0.1")]
	public float readyTime { get; set; } = 15f;

	[Export(PropertyHint.None, "")]
	public StringName readyTimerName { get; set; } = "Ready";

	[Export(PropertyHint.None, "")]
	public bool autoRise { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool componentStateUse { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public PackedScene riseEffect { get; set; }

	[Export(PropertyHint.None, "")]
	public string riseAudio { get; set; } = "GravestoneRumble";

	[Export(PropertyHint.None, "")]
	public bool createRiseEffect { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool setSmashInvincibleOnCharge { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool destroyAfterExplode { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool autoExplodeOnCharge { get; set; } = true;

	[ExportSubgroup("StateEvents", "")]
	[Export(PropertyHint.None, "")]
	public StringName readyStateEvent { get; set; } = "ToReady";

	[Export(PropertyHint.None, "")]
	public StringName riseStateEvent { get; set; } = "ToRise";

	[Export(PropertyHint.None, "")]
	public StringName chargeStateEvent { get; set; } = "ToCharge";

	[Export(PropertyHint.None, "")]
	public StringName idleStateEvent { get; set; } = "ToIdle";

	[ExportSubgroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public string readyAnimeClips { get; set; } = "Ready";

	[Export(PropertyHint.None, "")]
	public float readyAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string riseAnimeClips { get; set; } = "Rise";

	[Export(PropertyHint.None, "")]
	public float riseAnimeTimeScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public string chargeAnimeClips { get; set; } = "Idle";

	[Export(PropertyHint.None, "")]
	public float chargeAnimeTimeScale { get; set; } = 1f;

	[ExportSubgroup("Dependencies", "")]
	[Export(PropertyHint.None, "")]
	public StringName attackComponentName { get; set; } = "AttackComponent";

	[Export(PropertyHint.None, "")]
	public StringName explodeComponentName { get; set; } = "ExplodeComponent";

	[Export(PropertyHint.None, "")]
	public NodePath spritePath { get; set; } = new NodePath();

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new PotatoComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.readyTime)
		{
			readyTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.readyTimerName)
		{
			readyTimerName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.autoRise)
		{
			autoRise = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.componentStateUse)
		{
			componentStateUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.riseEffect)
		{
			riseEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.riseAudio)
		{
			riseAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.createRiseEffect)
		{
			createRiseEffect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.setSmashInvincibleOnCharge)
		{
			setSmashInvincibleOnCharge = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.destroyAfterExplode)
		{
			destroyAfterExplode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.autoExplodeOnCharge)
		{
			autoExplodeOnCharge = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.readyStateEvent)
		{
			readyStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.riseStateEvent)
		{
			riseStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.chargeStateEvent)
		{
			chargeStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			idleStateEvent = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.readyAnimeClips)
		{
			readyAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.readyAnimeTimeScale)
		{
			readyAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.riseAnimeClips)
		{
			riseAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.riseAnimeTimeScale)
		{
			riseAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.chargeAnimeClips)
		{
			chargeAnimeClips = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.chargeAnimeTimeScale)
		{
			chargeAnimeTimeScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.attackComponentName)
		{
			attackComponentName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.explodeComponentName)
		{
			explodeComponentName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			spritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		float from;
		if (name == PropertyName.readyTime)
		{
			from = readyTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		StringName from2;
		if (name == PropertyName.readyTimerName)
		{
			from2 = readyTimerName;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.autoRise)
		{
			from3 = autoRise;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.componentStateUse)
		{
			from3 = componentStateUse;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.riseEffect)
		{
			value = VariantUtils.CreateFrom<PackedScene>(riseEffect);
			return true;
		}
		string from4;
		if (name == PropertyName.riseAudio)
		{
			from4 = riseAudio;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.createRiseEffect)
		{
			from3 = createRiseEffect;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.setSmashInvincibleOnCharge)
		{
			from3 = setSmashInvincibleOnCharge;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.destroyAfterExplode)
		{
			from3 = destroyAfterExplode;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.autoExplodeOnCharge)
		{
			from3 = autoExplodeOnCharge;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.readyStateEvent)
		{
			from2 = readyStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.riseStateEvent)
		{
			from2 = riseStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.chargeStateEvent)
		{
			from2 = chargeStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.idleStateEvent)
		{
			from2 = idleStateEvent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.readyAnimeClips)
		{
			from4 = readyAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.readyAnimeTimeScale)
		{
			from = readyAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.riseAnimeClips)
		{
			from4 = riseAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.riseAnimeTimeScale)
		{
			from = riseAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.chargeAnimeClips)
		{
			from4 = chargeAnimeClips;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.chargeAnimeTimeScale)
		{
			from = chargeAnimeTimeScale;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.attackComponentName)
		{
			from2 = attackComponentName;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.explodeComponentName)
		{
			from2 = explodeComponentName;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.spritePath)
		{
			value = VariantUtils.CreateFrom<NodePath>(spritePath);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.readyTime, PropertyHint.Range, "0,300,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.readyTimerName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoRise, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.componentStateUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.riseEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.riseAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createRiseEffect, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.setSmashInvincibleOnCharge, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.destroyAfterExplode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoExplodeOnCharge, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "StateEvents", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.readyStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.riseStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.chargeStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.idleStateEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.readyAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.readyAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.riseAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.riseAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.chargeAnimeClips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.chargeAnimeTimeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Dependencies", PropertyHint.None, "", PropertyUsageFlags.Subgroup, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.attackComponentName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.explodeComponentName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.spritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.readyTime, Variant.From<float>(readyTime));
		info.AddProperty(PropertyName.readyTimerName, Variant.From<StringName>(readyTimerName));
		info.AddProperty(PropertyName.autoRise, Variant.From<bool>(autoRise));
		info.AddProperty(PropertyName.componentStateUse, Variant.From<bool>(componentStateUse));
		info.AddProperty(PropertyName.riseEffect, Variant.From<PackedScene>(riseEffect));
		info.AddProperty(PropertyName.riseAudio, Variant.From<string>(riseAudio));
		info.AddProperty(PropertyName.createRiseEffect, Variant.From<bool>(createRiseEffect));
		info.AddProperty(PropertyName.setSmashInvincibleOnCharge, Variant.From<bool>(setSmashInvincibleOnCharge));
		info.AddProperty(PropertyName.destroyAfterExplode, Variant.From<bool>(destroyAfterExplode));
		info.AddProperty(PropertyName.autoExplodeOnCharge, Variant.From<bool>(autoExplodeOnCharge));
		info.AddProperty(PropertyName.readyStateEvent, Variant.From<StringName>(readyStateEvent));
		info.AddProperty(PropertyName.riseStateEvent, Variant.From<StringName>(riseStateEvent));
		info.AddProperty(PropertyName.chargeStateEvent, Variant.From<StringName>(chargeStateEvent));
		info.AddProperty(PropertyName.idleStateEvent, Variant.From<StringName>(idleStateEvent));
		info.AddProperty(PropertyName.readyAnimeClips, Variant.From<string>(readyAnimeClips));
		info.AddProperty(PropertyName.readyAnimeTimeScale, Variant.From<float>(readyAnimeTimeScale));
		info.AddProperty(PropertyName.riseAnimeClips, Variant.From<string>(riseAnimeClips));
		info.AddProperty(PropertyName.riseAnimeTimeScale, Variant.From<float>(riseAnimeTimeScale));
		info.AddProperty(PropertyName.chargeAnimeClips, Variant.From<string>(chargeAnimeClips));
		info.AddProperty(PropertyName.chargeAnimeTimeScale, Variant.From<float>(chargeAnimeTimeScale));
		info.AddProperty(PropertyName.attackComponentName, Variant.From<StringName>(attackComponentName));
		info.AddProperty(PropertyName.explodeComponentName, Variant.From<StringName>(explodeComponentName));
		info.AddProperty(PropertyName.spritePath, Variant.From<NodePath>(spritePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.readyTime, out var value))
		{
			readyTime = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.readyTimerName, out var value2))
		{
			readyTimerName = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.autoRise, out var value3))
		{
			autoRise = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.componentStateUse, out var value4))
		{
			componentStateUse = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.riseEffect, out var value5))
		{
			riseEffect = value5.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.riseAudio, out var value6))
		{
			riseAudio = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.createRiseEffect, out var value7))
		{
			createRiseEffect = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.setSmashInvincibleOnCharge, out var value8))
		{
			setSmashInvincibleOnCharge = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.destroyAfterExplode, out var value9))
		{
			destroyAfterExplode = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.autoExplodeOnCharge, out var value10))
		{
			autoExplodeOnCharge = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.readyStateEvent, out var value11))
		{
			readyStateEvent = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.riseStateEvent, out var value12))
		{
			riseStateEvent = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.chargeStateEvent, out var value13))
		{
			chargeStateEvent = value13.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.idleStateEvent, out var value14))
		{
			idleStateEvent = value14.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.readyAnimeClips, out var value15))
		{
			readyAnimeClips = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.readyAnimeTimeScale, out var value16))
		{
			readyAnimeTimeScale = value16.As<float>();
		}
		if (info.TryGetProperty(PropertyName.riseAnimeClips, out var value17))
		{
			riseAnimeClips = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.riseAnimeTimeScale, out var value18))
		{
			riseAnimeTimeScale = value18.As<float>();
		}
		if (info.TryGetProperty(PropertyName.chargeAnimeClips, out var value19))
		{
			chargeAnimeClips = value19.As<string>();
		}
		if (info.TryGetProperty(PropertyName.chargeAnimeTimeScale, out var value20))
		{
			chargeAnimeTimeScale = value20.As<float>();
		}
		if (info.TryGetProperty(PropertyName.attackComponentName, out var value21))
		{
			attackComponentName = value21.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.explodeComponentName, out var value22))
		{
			explodeComponentName = value22.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.spritePath, out var value23))
		{
			spritePath = value23.As<NodePath>();
		}
	}
}
