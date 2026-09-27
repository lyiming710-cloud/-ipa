using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Resource/TowerDefense/Character/Event/TowerDefenseCharacterEventMathine.cs")]
public class TowerDefenseCharacterEventMathine : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName EventGet = "EventGet";
	}

	public new class PropertyName : Resource.PropertyName
	{
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public static TowerDefenseCharacterEventBase EventGet(string eventName)
	{
		return eventName switch
		{
			"CreateAddPacket" => (TowerDefenseCharacterEventBase)new TowerDefenseCharacterEventCreateAddPacket(), 
			"LuckyBagCreate" => new TowerDefenseCharacterEventLuckyBagCreate(), 
			"GoldShardCreate" => new TowerDefenseCharacterEventGoldShardCreate(), 
			"SnowBallSpawn" => new TowerDefenseCharacterEventSnowBallSpawn(), 
			"YBCreate" => new TowerDefenseCharacterEventYBCreate(), 
			"SunCreate" => new TowerDefenseCharacterEventSunCreate(), 
			"CoinCreate" => new TowerDefenseCharacterEventCoinCreate(), 
			"CreateProjectile" => new TowerDefenseCharacterEventCreateProjectile(), 
			"ProjectileSplit" => new TowerDefenseCharacterEventProjectileSplit(), 
			"CraterCreate" => new TowerDefenseCharacterEventCraterCreate(), 
			"Forzen" => new TowerDefenseCharacterEventForzen(), 
			"Hypnoses" => new TowerDefenseCharacterEventHypnoses(), 
			"IceSpeedDown" => new TowerDefenseCharacterEventIceSpeedDown(), 
			"Destroy" => new TowerDefenseCharacterEventDestroy(), 
			"Purify" => new TowerDefenseCharacterEventPurify(), 
			"WakeUp" => new TowerDefenseCharacterEventWakeUp(), 
			"PacketCreate" => new TowerDefenseCharacterEventPacketCreate(), 
			"PacketSpawn" => new TowerDefenseCharacterEventPacketSpawn(), 
			_ => null, 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.EventGet, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EventGet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterEventBase>(EventGet(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EventGet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterEventBase>(EventGet(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EventGet)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
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
