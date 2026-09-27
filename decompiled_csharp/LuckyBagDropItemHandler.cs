using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/DropItem/Handler/LuckyBagDropItemHandler.cs")]
public class LuckyBagDropItemHandler : DropItemHandler
{
	public new class MethodName : DropItemHandler.MethodName
	{
		public new static readonly StringName OnCollect = "OnCollect";

		public new static readonly StringName Reset = "Reset";
	}

	public new class PropertyName : DropItemHandler.PropertyName
	{
		public static readonly StringName LuckyBagNum = "LuckyBagNum";
	}

	public new class SignalName : DropItemHandler.SignalName
	{
	}

	public int LuckyBagNum;

	public override void OnCollect(Vector2 pos, long value)
	{
		LuckyBagNum++;
		switch (LuckyBagNum)
		{
		case 10:
			TowerDefenseManager.Instance.SunCreate(pos, 100L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 10.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
			break;
		case 30:
			TowerDefenseManager.Instance.SpawnPacket(TowerDefenseManager.GetPacketConfigCostUpperWithTypeList(300, new Array<TowerDefenseEnum.PACKET_TYPE>
			{
				TowerDefenseEnum.PACKET_TYPE.WHITE,
				TowerDefenseEnum.PACKET_TYPE.ORIGINAL
			}).PickRandom(), pos, 15.0, isFall: false);
			break;
		case 60:
			TowerDefenseManager.Instance.SpawnPacket(TowerDefenseManager.GetPacketConfigCostUpper(0, TowerDefenseEnum.PACKET_TYPE.GOLD).PickRandom(), pos, 15.0, isFall: false);
			break;
		case 100:
			TowerDefenseManager.Instance.SunCreate(pos, 1000L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, 10.0, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
			break;
		}
		if (LuckyBagNum > 100 && LuckyBagNum % 100 == 0)
		{
			for (int i = 0; i < LuckyBagNum / 100; i++)
			{
				TowerDefenseManager.Instance.SpawnPacket(TowerDefenseManager.GetPacketConfigCostUpper(0, TowerDefenseEnum.PACKET_TYPE.GOLD).PickRandom(), pos, 15.0, isFall: false);
			}
		}
	}

	public override void Reset()
	{
		LuckyBagNum = 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.OnCollect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Reset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OnCollect && args.Count == 2)
		{
			OnCollect(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Reset && args.Count == 0)
		{
			Reset();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.OnCollect)
		{
			return true;
		}
		if (method == MethodName.Reset)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LuckyBagNum)
		{
			LuckyBagNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.LuckyBagNum)
		{
			value = VariantUtils.CreateFrom(in LuckyBagNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.LuckyBagNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LuckyBagNum, Variant.From(in LuckyBagNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LuckyBagNum, out var value))
		{
			LuckyBagNum = value.As<int>();
		}
	}
}
