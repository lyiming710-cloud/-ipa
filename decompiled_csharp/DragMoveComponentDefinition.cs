using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/DragMoveComponent/DragMoveComponentDefinition.cs")]
public class DragMoveComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName allowMouseDrag = "allowMouseDrag";

		public static readonly StringName allowDirectionalInput = "allowDirectionalInput";

		public static readonly StringName pressAction = "pressAction";

		public static readonly StringName moveUpAction = "moveUpAction";

		public static readonly StringName moveDownAction = "moveDownAction";

		public static readonly StringName moveLeftAction = "moveLeftAction";

		public static readonly StringName moveRightAction = "moveRightAction";

		public static readonly StringName requestRetryPhysicsFrames = "requestRetryPhysicsFrames";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool allowMouseDrag = true;

	[Export(PropertyHint.None, "")]
	public bool allowDirectionalInput = true;

	[Export(PropertyHint.None, "")]
	public StringName pressAction = "Press";

	[Export(PropertyHint.None, "")]
	public StringName moveUpAction = "P1Up";

	[Export(PropertyHint.None, "")]
	public StringName moveDownAction = "P1Down";

	[Export(PropertyHint.None, "")]
	public StringName moveLeftAction = "P1Left";

	[Export(PropertyHint.None, "")]
	public StringName moveRightAction = "P1Right";

	[Export(PropertyHint.Range, "1,300,1")]
	public int requestRetryPhysicsFrames = 30;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new DragMoveComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.allowMouseDrag)
		{
			allowMouseDrag = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.allowDirectionalInput)
		{
			allowDirectionalInput = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pressAction)
		{
			pressAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.moveUpAction)
		{
			moveUpAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.moveDownAction)
		{
			moveDownAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.moveLeftAction)
		{
			moveLeftAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.moveRightAction)
		{
			moveRightAction = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.requestRetryPhysicsFrames)
		{
			requestRetryPhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.allowMouseDrag)
		{
			value = VariantUtils.CreateFrom(in allowMouseDrag);
			return true;
		}
		if (name == PropertyName.allowDirectionalInput)
		{
			value = VariantUtils.CreateFrom(in allowDirectionalInput);
			return true;
		}
		if (name == PropertyName.pressAction)
		{
			value = VariantUtils.CreateFrom(in pressAction);
			return true;
		}
		if (name == PropertyName.moveUpAction)
		{
			value = VariantUtils.CreateFrom(in moveUpAction);
			return true;
		}
		if (name == PropertyName.moveDownAction)
		{
			value = VariantUtils.CreateFrom(in moveDownAction);
			return true;
		}
		if (name == PropertyName.moveLeftAction)
		{
			value = VariantUtils.CreateFrom(in moveLeftAction);
			return true;
		}
		if (name == PropertyName.moveRightAction)
		{
			value = VariantUtils.CreateFrom(in moveRightAction);
			return true;
		}
		if (name == PropertyName.requestRetryPhysicsFrames)
		{
			value = VariantUtils.CreateFrom(in requestRetryPhysicsFrames);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.allowMouseDrag, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.allowDirectionalInput, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.pressAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.moveUpAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.moveDownAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.moveLeftAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.moveRightAction, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.requestRetryPhysicsFrames, PropertyHint.Range, "1,300,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.allowMouseDrag, Variant.From(in allowMouseDrag));
		info.AddProperty(PropertyName.allowDirectionalInput, Variant.From(in allowDirectionalInput));
		info.AddProperty(PropertyName.pressAction, Variant.From(in pressAction));
		info.AddProperty(PropertyName.moveUpAction, Variant.From(in moveUpAction));
		info.AddProperty(PropertyName.moveDownAction, Variant.From(in moveDownAction));
		info.AddProperty(PropertyName.moveLeftAction, Variant.From(in moveLeftAction));
		info.AddProperty(PropertyName.moveRightAction, Variant.From(in moveRightAction));
		info.AddProperty(PropertyName.requestRetryPhysicsFrames, Variant.From(in requestRetryPhysicsFrames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.allowMouseDrag, out var value))
		{
			allowMouseDrag = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.allowDirectionalInput, out var value2))
		{
			allowDirectionalInput = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pressAction, out var value3))
		{
			pressAction = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.moveUpAction, out var value4))
		{
			moveUpAction = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.moveDownAction, out var value5))
		{
			moveDownAction = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.moveLeftAction, out var value6))
		{
			moveLeftAction = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.moveRightAction, out var value7))
		{
			moveRightAction = value7.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.requestRetryPhysicsFrames, out var value8))
		{
			requestRetryPhysicsFrames = value8.As<int>();
		}
	}
}
