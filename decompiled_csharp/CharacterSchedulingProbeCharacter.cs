using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CharacterSchedulingProbeCharacter.cs")]
public class CharacterSchedulingProbeCharacter : TowerDefenseCharacter
{
	public new class MethodName : TowerDefenseCharacter.MethodName
	{
		public static readonly StringName ArmExistingPhysicsProbe = "ArmExistingPhysicsProbe";

		public static readonly StringName ProbePhysicsFrame = "ProbePhysicsFrame";

		public new static readonly StringName ShouldUpdateGridPos = "ShouldUpdateGridPos";
	}

	public new class PropertyName : TowerDefenseCharacter.PropertyName
	{
		public static readonly StringName ExistingPhysicsUpdateCount = "ExistingPhysicsUpdateCount";
	}

	public new class SignalName : TowerDefenseCharacter.SignalName
	{
	}

	public int ExistingPhysicsUpdateCount { get; private set; }

	public List<string> SchedulingTrace { get; } = new List<string>();

	public void ArmExistingPhysicsProbe(ulong physicsFrame)
	{
		ulong num = 5uL;
		randFreshIndex = (int)((num - physicsFrame % num) % num);
	}

	public void ProbePhysicsFrame(double delta, ulong physicsFrame)
	{
		PhysicsProcessWithFrame(delta, physicsFrame);
	}

	public override bool ShouldUpdateGridPos()
	{
		ExistingPhysicsUpdateCount++;
		SchedulingTrace.Add("existingPhysics");
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.ArmExistingPhysicsProbe, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProbePhysicsFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldUpdateGridPos, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ArmExistingPhysicsProbe && args.Count == 1)
		{
			ArmExistingPhysicsProbe(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProbePhysicsFrame && args.Count == 2)
		{
			ProbePhysicsFrame(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldUpdateGridPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUpdateGridPos());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ArmExistingPhysicsProbe)
		{
			return true;
		}
		if (method == MethodName.ProbePhysicsFrame)
		{
			return true;
		}
		if (method == MethodName.ShouldUpdateGridPos)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ExistingPhysicsUpdateCount)
		{
			ExistingPhysicsUpdateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ExistingPhysicsUpdateCount)
		{
			value = VariantUtils.CreateFrom<int>(ExistingPhysicsUpdateCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ExistingPhysicsUpdateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ExistingPhysicsUpdateCount, Variant.From<int>(ExistingPhysicsUpdateCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ExistingPhysicsUpdateCount, out var value))
		{
			ExistingPhysicsUpdateCount = value.As<int>();
		}
	}
}
