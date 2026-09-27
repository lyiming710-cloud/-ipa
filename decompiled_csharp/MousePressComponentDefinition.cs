using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/MousePressComponent/MousePressComponentDefinition.cs")]
public class MousePressComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName clickShape = "clickShape";

		public static readonly StringName toggleTargetPath = "toggleTargetPath";

		public static readonly StringName pressAction = "pressAction";

		public static readonly StringName releaseAction = "releaseAction";

		public static readonly StringName toggleMode = "toggleMode";

		public static readonly StringName enableDoublePress = "enableDoublePress";

		public static readonly StringName followPointerWhileAiming = "followPointerWhileAiming";

		public static readonly StringName hostAuthorityOnly = "hostAuthorityOnly";

		public static readonly StringName doublePressWindow = "doublePressWindow";

		public static readonly StringName interactionCooldown = "interactionCooldown";

		public static readonly StringName hoverBrightness = "hoverBrightness";

		public static readonly StringName brightnessShaderParameter = "brightnessShaderParameter";

		public static readonly StringName enabledCursorShape = "enabledCursorShape";

		public static readonly StringName disabledCursorShape = "disabledCursorShape";

		public static readonly StringName requireGroundForFinish = "requireGroundForFinish";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Owner-relative References", "")]
	[Export(PropertyHint.None, "")]
	public AabbShape2DResource clickShape;

	[Export(PropertyHint.None, "")]
	public NodePath toggleTargetPath = new NodePath();

	[ExportGroup("Input", "")]
	[Export(PropertyHint.None, "")]
	public StringName pressAction = "Press";

	[Export(PropertyHint.None, "")]
	public StringName releaseAction = "Release";

	[Export(PropertyHint.None, "")]
	public bool toggleMode;

	[Export(PropertyHint.None, "")]
	public bool enableDoublePress = true;

	[Export(PropertyHint.None, "")]
	public bool followPointerWhileAiming = true;

	[Export(PropertyHint.None, "")]
	public bool hostAuthorityOnly = true;

	[ExportGroup("Timing", "")]
	[Export(PropertyHint.Range, "0,2,0.01")]
	public float doublePressWindow = 0.25f;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public float interactionCooldown = 0.1f;

	[ExportGroup("Presentation", "")]
	[Export(PropertyHint.Range, "0,2,0.01")]
	public float hoverBrightness = 0.3f;

	[Export(PropertyHint.None, "")]
	public StringName brightnessShaderParameter = "brightStrength";

	[Export(PropertyHint.None, "")]
	public Control.CursorShape enabledCursorShape = Control.CursorShape.PointingHand;

	[Export(PropertyHint.None, "")]
	public Control.CursorShape disabledCursorShape;

	[ExportGroup("Aiming", "")]
	[Export(PropertyHint.None, "")]
	public bool requireGroundForFinish = true;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new MousePressComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.clickShape)
		{
			clickShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.toggleTargetPath)
		{
			toggleTargetPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.pressAction)
		{
			pressAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.releaseAction)
		{
			releaseAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.toggleMode)
		{
			toggleMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.enableDoublePress)
		{
			enableDoublePress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.followPointerWhileAiming)
		{
			followPointerWhileAiming = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hostAuthorityOnly)
		{
			hostAuthorityOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.doublePressWindow)
		{
			doublePressWindow = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.interactionCooldown)
		{
			interactionCooldown = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.hoverBrightness)
		{
			hoverBrightness = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.brightnessShaderParameter)
		{
			brightnessShaderParameter = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.enabledCursorShape)
		{
			enabledCursorShape = VariantUtils.ConvertTo<Control.CursorShape>(in value);
			return true;
		}
		if (name == PropertyName.disabledCursorShape)
		{
			disabledCursorShape = VariantUtils.ConvertTo<Control.CursorShape>(in value);
			return true;
		}
		if (name == PropertyName.requireGroundForFinish)
		{
			requireGroundForFinish = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.clickShape)
		{
			value = VariantUtils.CreateFrom(in clickShape);
			return true;
		}
		if (name == PropertyName.toggleTargetPath)
		{
			value = VariantUtils.CreateFrom(in toggleTargetPath);
			return true;
		}
		if (name == PropertyName.pressAction)
		{
			value = VariantUtils.CreateFrom(in pressAction);
			return true;
		}
		if (name == PropertyName.releaseAction)
		{
			value = VariantUtils.CreateFrom(in releaseAction);
			return true;
		}
		if (name == PropertyName.toggleMode)
		{
			value = VariantUtils.CreateFrom(in toggleMode);
			return true;
		}
		if (name == PropertyName.enableDoublePress)
		{
			value = VariantUtils.CreateFrom(in enableDoublePress);
			return true;
		}
		if (name == PropertyName.followPointerWhileAiming)
		{
			value = VariantUtils.CreateFrom(in followPointerWhileAiming);
			return true;
		}
		if (name == PropertyName.hostAuthorityOnly)
		{
			value = VariantUtils.CreateFrom(in hostAuthorityOnly);
			return true;
		}
		if (name == PropertyName.doublePressWindow)
		{
			value = VariantUtils.CreateFrom(in doublePressWindow);
			return true;
		}
		if (name == PropertyName.interactionCooldown)
		{
			value = VariantUtils.CreateFrom(in interactionCooldown);
			return true;
		}
		if (name == PropertyName.hoverBrightness)
		{
			value = VariantUtils.CreateFrom(in hoverBrightness);
			return true;
		}
		if (name == PropertyName.brightnessShaderParameter)
		{
			value = VariantUtils.CreateFrom(in brightnessShaderParameter);
			return true;
		}
		if (name == PropertyName.enabledCursorShape)
		{
			value = VariantUtils.CreateFrom(in enabledCursorShape);
			return true;
		}
		if (name == PropertyName.disabledCursorShape)
		{
			value = VariantUtils.CreateFrom(in disabledCursorShape);
			return true;
		}
		if (name == PropertyName.requireGroundForFinish)
		{
			value = VariantUtils.CreateFrom(in requireGroundForFinish);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Owner-relative References", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.clickShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.toggleTargetPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Input", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.pressAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.releaseAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.toggleMode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enableDoublePress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.followPointerWhileAiming, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hostAuthorityOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Timing", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.doublePressWindow, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.interactionCooldown, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Presentation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hoverBrightness, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.brightnessShaderParameter, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.enabledCursorShape, PropertyHint.Enum, "Arrow,Ibeam,PointingHand,Cross,Wait,Busy,Drag,CanDrop,Forbidden,Vsize,Hsize,Bdiagsize,Fdiagsize,Move,Vsplit,Hsplit,Help", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.disabledCursorShape, PropertyHint.Enum, "Arrow,Ibeam,PointingHand,Cross,Wait,Busy,Drag,CanDrop,Forbidden,Vsize,Hsize,Bdiagsize,Fdiagsize,Move,Vsplit,Hsplit,Help", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Aiming", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.requireGroundForFinish, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.clickShape, Variant.From(in clickShape));
		info.AddProperty(PropertyName.toggleTargetPath, Variant.From(in toggleTargetPath));
		info.AddProperty(PropertyName.pressAction, Variant.From(in pressAction));
		info.AddProperty(PropertyName.releaseAction, Variant.From(in releaseAction));
		info.AddProperty(PropertyName.toggleMode, Variant.From(in toggleMode));
		info.AddProperty(PropertyName.enableDoublePress, Variant.From(in enableDoublePress));
		info.AddProperty(PropertyName.followPointerWhileAiming, Variant.From(in followPointerWhileAiming));
		info.AddProperty(PropertyName.hostAuthorityOnly, Variant.From(in hostAuthorityOnly));
		info.AddProperty(PropertyName.doublePressWindow, Variant.From(in doublePressWindow));
		info.AddProperty(PropertyName.interactionCooldown, Variant.From(in interactionCooldown));
		info.AddProperty(PropertyName.hoverBrightness, Variant.From(in hoverBrightness));
		info.AddProperty(PropertyName.brightnessShaderParameter, Variant.From(in brightnessShaderParameter));
		info.AddProperty(PropertyName.enabledCursorShape, Variant.From(in enabledCursorShape));
		info.AddProperty(PropertyName.disabledCursorShape, Variant.From(in disabledCursorShape));
		info.AddProperty(PropertyName.requireGroundForFinish, Variant.From(in requireGroundForFinish));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.clickShape, out var value))
		{
			clickShape = value.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.toggleTargetPath, out var value2))
		{
			toggleTargetPath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.pressAction, out var value3))
		{
			pressAction = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.releaseAction, out var value4))
		{
			releaseAction = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.toggleMode, out var value5))
		{
			toggleMode = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.enableDoublePress, out var value6))
		{
			enableDoublePress = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.followPointerWhileAiming, out var value7))
		{
			followPointerWhileAiming = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hostAuthorityOnly, out var value8))
		{
			hostAuthorityOnly = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.doublePressWindow, out var value9))
		{
			doublePressWindow = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.interactionCooldown, out var value10))
		{
			interactionCooldown = value10.As<float>();
		}
		if (info.TryGetProperty(PropertyName.hoverBrightness, out var value11))
		{
			hoverBrightness = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.brightnessShaderParameter, out var value12))
		{
			brightnessShaderParameter = value12.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.enabledCursorShape, out var value13))
		{
			enabledCursorShape = value13.As<Control.CursorShape>();
		}
		if (info.TryGetProperty(PropertyName.disabledCursorShape, out var value14))
		{
			disabledCursorShape = value14.As<Control.CursorShape>();
		}
		if (info.TryGetProperty(PropertyName.requireGroundForFinish, out var value15))
		{
			requireGroundForFinish = value15.As<bool>();
		}
	}
}
