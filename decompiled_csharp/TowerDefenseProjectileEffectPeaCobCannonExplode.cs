using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/PeaCobCannonExplode/TowerDefenseProjectileEffectPeaCobCannonExplode.cs")]
public class TowerDefenseProjectileEffectPeaCobCannonExplode : TowerDefenseProjectileEffectCherryPea
{
	public new class MethodName : TowerDefenseProjectileEffectCherryPea.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : TowerDefenseProjectileEffectCherryPea.PropertyName
	{
		public new static readonly StringName NetworkEffectId = "NetworkEffectId";
	}

	public new class SignalName : TowerDefenseProjectileEffectCherryPea.SignalName
	{
	}

	public override string NetworkEffectId => "pea_cob_cannon_burst";

	public override void _Ready()
	{
		if (PrepareNetworkEffect())
		{
			TowerDefenseExplode.CreateExplode(GlobalPosition, new Vector2(1.5f, 1.5f), Eventlist, new Array<TowerDefenseCharacter>(), camp, -1);
			AttackCreateAsync();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.NetworkEffectId)
		{
			value = VariantUtils.CreateFrom<string>(NetworkEffectId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.NetworkEffectId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
