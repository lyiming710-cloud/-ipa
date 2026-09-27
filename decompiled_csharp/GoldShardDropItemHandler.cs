using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/DropItem/Handler/GoldShardDropItemHandler.cs")]
public class GoldShardDropItemHandler : DropItemHandler
{
	public new class MethodName : DropItemHandler.MethodName
	{
		public new static readonly StringName OnCollect = "OnCollect";

		public new static readonly StringName Reset = "Reset";
	}

	public new class PropertyName : DropItemHandler.PropertyName
	{
		public static readonly StringName GoldShardNum = "GoldShardNum";
	}

	public new class SignalName : DropItemHandler.SignalName
	{
	}

	public int GoldShardNum;

	public override void OnCollect(Vector2 pos, long value)
	{
		GoldShardNum++;
		if (GoldShardNum >= 4 && GoldShardNum % 4 == 0)
		{
			for (int i = 0; i < GoldShardNum / 4; i++)
			{
				string packetName = (string)TowerDefenseManager.GetPacketBankData("GeneralPlant").GetCategory("Gold").PickRandom();
				TowerDefenseManager.Instance.SpawnPacket(TowerDefenseManager.GetPacketConfig(packetName), pos, 15.0, isFall: false);
			}
			Reset();
		}
	}

	public override void Reset()
	{
		GoldShardNum = 0;
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
		if (name == PropertyName.GoldShardNum)
		{
			GoldShardNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GoldShardNum)
		{
			value = VariantUtils.CreateFrom(in GoldShardNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.GoldShardNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GoldShardNum, Variant.From(in GoldShardNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GoldShardNum, out var value))
		{
			GoldShardNum = value.As<int>();
		}
	}
}
