using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/CharacterMoveComponent/CharacterMoveComponentDefinition.cs")]
public class CharacterMoveComponentDefinition : CharacterComponentDefinition
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName velocity = "velocity";

		public static readonly StringName gravity = "gravity";

		public static readonly StringName moveScale = "moveScale";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Vector2 velocity = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public double gravity;

	[Export(PropertyHint.None, "")]
	public double moveScale = 1.0;

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new CharacterMoveComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.velocity)
		{
			velocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			gravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.moveScale)
		{
			moveScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.velocity)
		{
			value = VariantUtils.CreateFrom(in velocity);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			value = VariantUtils.CreateFrom(in gravity);
			return true;
		}
		if (name == PropertyName.moveScale)
		{
			value = VariantUtils.CreateFrom(in moveScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.moveScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.velocity, Variant.From(in velocity));
		info.AddProperty(PropertyName.gravity, Variant.From(in gravity));
		info.AddProperty(PropertyName.moveScale, Variant.From(in moveScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.velocity, out var value))
		{
			velocity = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gravity, out var value2))
		{
			gravity = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.moveScale, out var value3))
		{
			moveScale = value3.As<double>();
		}
	}
}
