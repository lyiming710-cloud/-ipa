using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter5/YetiBobsledTeam/Scene/TowerDefenseZombieYetiBobsledTeam.cs")]
public class TowerDefenseZombieYetiBobsledTeam : TowerDefenseZombieBobsledVehicleBase
{
	public new class MethodName : TowerDefenseZombieBobsledVehicleBase.MethodName
	{
		public new static readonly StringName GetBreakClip = "GetBreakClip";

		public new static readonly StringName GetRidingClip = "GetRidingClip";
	}

	public new class PropertyName : TowerDefenseZombieBobsledVehicleBase.PropertyName
	{
		public new static readonly StringName IsHardControlImmune = "IsHardControlImmune";

		public new static readonly StringName UsesBreakAnimationCompletion = "UsesBreakAnimationCompletion";
	}

	public new class SignalName : TowerDefenseZombieBobsledVehicleBase.SignalName
	{
	}

	public override bool IsHardControlImmune => true;

	protected override bool UsesBreakAnimationCompletion => true;

	protected override string GetBreakClip()
	{
		return "Wheelie";
	}

	protected override string GetRidingClip()
	{
		return "Drive";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetBreakClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRidingClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetBreakClip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetBreakClip());
			return true;
		}
		if (method == MethodName.GetRidingClip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetRidingClip());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetBreakClip)
		{
			return true;
		}
		if (method == MethodName.GetRidingClip)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsHardControlImmune)
		{
			from = IsHardControlImmune;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.UsesBreakAnimationCompletion)
		{
			from = UsesBreakAnimationCompletion;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHardControlImmune, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesBreakAnimationCompletion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
