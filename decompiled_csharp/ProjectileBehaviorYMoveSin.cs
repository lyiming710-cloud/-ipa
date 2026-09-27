using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Projectile/ProjectileBehaviorYMoveSin.cs")]
public class ProjectileBehaviorYMoveSin : ProjectileBehaviorDefinition
{
	private sealed class SineKernel : ProjectileBehaviorKernel
	{
		private readonly double[] _timers;

		private readonly double _strength;

		private readonly double _speed;

		public SineKernel(int capacity, double strength, double speed)
		{
			_timers = new double[capacity];
			_strength = strength;
			_speed = speed;
		}

		public override void OnSpawn(int index, ref BulletData bullet)
		{
			_timers[index] = 0.0;
		}

		public override void Process(int index, ref BulletData bullet, double delta)
		{
			double num = _timers[index] + delta;
			_timers[index] = num;
			bullet.pos.Y = bullet.savePos.Y + (float)Mathf.Sin(num * _speed) * (float)_strength;
		}

		public override void OnDespawn(int index, ref BulletData bullet)
		{
			_timers[index] = 0.0;
		}
	}

	public new class MethodName : ProjectileBehaviorDefinition.MethodName
	{
	}

	public new class PropertyName : ProjectileBehaviorDefinition.PropertyName
	{
		public static readonly StringName Strength = "Strength";

		public static readonly StringName Speed = "Speed";
	}

	public new class SignalName : ProjectileBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double Strength = 30.0;

	[Export(PropertyHint.None, "")]
	public double Speed = 10.0;

	public ProjectileBehaviorYMoveSin()
	{
	}

	public ProjectileBehaviorYMoveSin(double _strength, double _speed)
	{
		Strength = _strength;
		Speed = _speed;
	}

	public override ProjectileBehaviorKernel CreateBulletFieldKernel(int capacity)
	{
		return new SineKernel(capacity, Strength, Speed);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Strength)
		{
			Strength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.Speed)
		{
			Speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Strength)
		{
			value = VariantUtils.CreateFrom(in Strength);
			return true;
		}
		if (name == PropertyName.Speed)
		{
			value = VariantUtils.CreateFrom(in Speed);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.Strength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.Speed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Strength, Variant.From(in Strength));
		info.AddProperty(PropertyName.Speed, Variant.From(in Speed));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Strength, out var value))
		{
			Strength = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.Speed, out var value2))
		{
			Speed = value2.As<double>();
		}
	}
}
