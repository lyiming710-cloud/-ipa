using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/WaterEnvironmentComponent/WaterEnvironmentComponentDefinition.cs")]
public class WaterEnvironmentComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName waterLineSpritePath = "waterLineSpritePath";

		public static readonly StringName waterHeight = "waterHeight";

		public static readonly StringName waterIdleAnime = "waterIdleAnime";

		public static readonly StringName groundHeightLerpSpeed = "groundHeightLerpSpeed";

		public static readonly StringName environmentCheckInterval = "environmentCheckInterval";

		public static readonly StringName entryDiscardOffset = "entryDiscardOffset";

		public static readonly StringName exitDiscardStartOffset = "exitDiscardStartOffset";

		public static readonly StringName exitDiscardEndOffset = "exitDiscardEndOffset";

		public static readonly StringName exitDuration = "exitDuration";

		public static readonly StringName discardResetPosition = "discardResetPosition";

		public static readonly StringName discardShaderParameter = "discardShaderParameter";

		public static readonly StringName createEntrySplash = "createEntrySplash";

		public static readonly StringName restoreIdleOnExit = "restoreIdleOnExit";

		public static readonly StringName exitEase = "exitEase";

		public static readonly StringName exitTransition = "exitTransition";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public NodePath waterLineSpritePath = new NodePath();

	[Export(PropertyHint.None, "")]
	public float waterHeight = 35f;

	[Export(PropertyHint.None, "")]
	public string waterIdleAnime = "";

	[Export(PropertyHint.Range, "0,30,0.1")]
	public float groundHeightLerpSpeed = 3f;

	[Export(PropertyHint.Range, "1,30,1")]
	public int environmentCheckInterval = 5;

	[Export(PropertyHint.None, "")]
	public float entryDiscardOffset = 45f;

	[Export(PropertyHint.None, "")]
	public float exitDiscardStartOffset = 56f;

	[Export(PropertyHint.None, "")]
	public float exitDiscardEndOffset = 86f;

	[Export(PropertyHint.Range, "0,10,0.01")]
	public float exitDuration = 1f;

	[Export(PropertyHint.None, "")]
	public float discardResetPosition = 10000f;

	[Export(PropertyHint.None, "")]
	public StringName discardShaderParameter = "discardDownPos";

	[Export(PropertyHint.None, "")]
	public bool createEntrySplash = true;

	[Export(PropertyHint.None, "")]
	public bool restoreIdleOnExit = true;

	[Export(PropertyHint.None, "")]
	public Tween.EaseType exitEase = Tween.EaseType.Out;

	[Export(PropertyHint.None, "")]
	public Tween.TransitionType exitTransition = Tween.TransitionType.Cubic;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new WaterEnvironmentComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.waterLineSpritePath)
		{
			waterLineSpritePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.waterHeight)
		{
			waterHeight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.waterIdleAnime)
		{
			waterIdleAnime = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.groundHeightLerpSpeed)
		{
			groundHeightLerpSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.environmentCheckInterval)
		{
			environmentCheckInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.entryDiscardOffset)
		{
			entryDiscardOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.exitDiscardStartOffset)
		{
			exitDiscardStartOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.exitDiscardEndOffset)
		{
			exitDiscardEndOffset = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.exitDuration)
		{
			exitDuration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			discardResetPosition = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			discardShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.createEntrySplash)
		{
			createEntrySplash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.restoreIdleOnExit)
		{
			restoreIdleOnExit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.exitEase)
		{
			exitEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName.exitTransition)
		{
			exitTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.waterLineSpritePath)
		{
			value = VariantUtils.CreateFrom(in waterLineSpritePath);
			return true;
		}
		if (name == PropertyName.waterHeight)
		{
			value = VariantUtils.CreateFrom(in waterHeight);
			return true;
		}
		if (name == PropertyName.waterIdleAnime)
		{
			value = VariantUtils.CreateFrom(in waterIdleAnime);
			return true;
		}
		if (name == PropertyName.groundHeightLerpSpeed)
		{
			value = VariantUtils.CreateFrom(in groundHeightLerpSpeed);
			return true;
		}
		if (name == PropertyName.environmentCheckInterval)
		{
			value = VariantUtils.CreateFrom(in environmentCheckInterval);
			return true;
		}
		if (name == PropertyName.entryDiscardOffset)
		{
			value = VariantUtils.CreateFrom(in entryDiscardOffset);
			return true;
		}
		if (name == PropertyName.exitDiscardStartOffset)
		{
			value = VariantUtils.CreateFrom(in exitDiscardStartOffset);
			return true;
		}
		if (name == PropertyName.exitDiscardEndOffset)
		{
			value = VariantUtils.CreateFrom(in exitDiscardEndOffset);
			return true;
		}
		if (name == PropertyName.exitDuration)
		{
			value = VariantUtils.CreateFrom(in exitDuration);
			return true;
		}
		if (name == PropertyName.discardResetPosition)
		{
			value = VariantUtils.CreateFrom(in discardResetPosition);
			return true;
		}
		if (name == PropertyName.discardShaderParameter)
		{
			value = VariantUtils.CreateFrom(in discardShaderParameter);
			return true;
		}
		if (name == PropertyName.createEntrySplash)
		{
			value = VariantUtils.CreateFrom(in createEntrySplash);
			return true;
		}
		if (name == PropertyName.restoreIdleOnExit)
		{
			value = VariantUtils.CreateFrom(in restoreIdleOnExit);
			return true;
		}
		if (name == PropertyName.exitEase)
		{
			value = VariantUtils.CreateFrom(in exitEase);
			return true;
		}
		if (name == PropertyName.exitTransition)
		{
			value = VariantUtils.CreateFrom(in exitTransition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.waterLineSpritePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.waterHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.waterIdleAnime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.groundHeightLerpSpeed, PropertyHint.Range, "0,30,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.environmentCheckInterval, PropertyHint.Range, "1,30,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.entryDiscardOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.exitDiscardStartOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.exitDiscardEndOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.exitDuration, PropertyHint.Range, "0,10,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardResetPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.discardShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createEntrySplash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.restoreIdleOnExit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.exitEase, PropertyHint.Enum, "In,Out,InOut,OutIn", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.exitTransition, PropertyHint.Enum, "Linear,Sine,Quint,Quart,Quad,Expo,Elastic,Cubic,Circ,Bounce,Back,Spring", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.waterLineSpritePath, Variant.From(in waterLineSpritePath));
		info.AddProperty(PropertyName.waterHeight, Variant.From(in waterHeight));
		info.AddProperty(PropertyName.waterIdleAnime, Variant.From(in waterIdleAnime));
		info.AddProperty(PropertyName.groundHeightLerpSpeed, Variant.From(in groundHeightLerpSpeed));
		info.AddProperty(PropertyName.environmentCheckInterval, Variant.From(in environmentCheckInterval));
		info.AddProperty(PropertyName.entryDiscardOffset, Variant.From(in entryDiscardOffset));
		info.AddProperty(PropertyName.exitDiscardStartOffset, Variant.From(in exitDiscardStartOffset));
		info.AddProperty(PropertyName.exitDiscardEndOffset, Variant.From(in exitDiscardEndOffset));
		info.AddProperty(PropertyName.exitDuration, Variant.From(in exitDuration));
		info.AddProperty(PropertyName.discardResetPosition, Variant.From(in discardResetPosition));
		info.AddProperty(PropertyName.discardShaderParameter, Variant.From(in discardShaderParameter));
		info.AddProperty(PropertyName.createEntrySplash, Variant.From(in createEntrySplash));
		info.AddProperty(PropertyName.restoreIdleOnExit, Variant.From(in restoreIdleOnExit));
		info.AddProperty(PropertyName.exitEase, Variant.From(in exitEase));
		info.AddProperty(PropertyName.exitTransition, Variant.From(in exitTransition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.waterLineSpritePath, out var value))
		{
			waterLineSpritePath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.waterHeight, out var value2))
		{
			waterHeight = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.waterIdleAnime, out var value3))
		{
			waterIdleAnime = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.groundHeightLerpSpeed, out var value4))
		{
			groundHeightLerpSpeed = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.environmentCheckInterval, out var value5))
		{
			environmentCheckInterval = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.entryDiscardOffset, out var value6))
		{
			entryDiscardOffset = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.exitDiscardStartOffset, out var value7))
		{
			exitDiscardStartOffset = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.exitDiscardEndOffset, out var value8))
		{
			exitDiscardEndOffset = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName.exitDuration, out var value9))
		{
			exitDuration = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardResetPosition, out var value10))
		{
			discardResetPosition = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.discardShaderParameter, out var value11))
		{
			discardShaderParameter = value11.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.createEntrySplash, out var value12))
		{
			createEntrySplash = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.restoreIdleOnExit, out var value13))
		{
			restoreIdleOnExit = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.exitEase, out var value14))
		{
			exitEase = value14.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName.exitTransition, out var value15))
		{
			exitTransition = value15.As<Tween.TransitionType>();
		}
	}
}
