using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PreSpawn/Resource/TowerDefenseBattleFeaturePreSpawnConfig.cs")]
public class TowerDefenseBattleFeaturePreSpawnConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName preSpawnList = "preSpawnList";

		public static readonly StringName maxRetryPasses = "maxRetryPasses";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelPreSpawnConfig> preSpawnList = new Array<TowerDefenseLevelPreSpawnConfig>();

	[Export(PropertyHint.None, "")]
	public int maxRetryPasses = 8;

	public void Init(Dictionary data)
	{
		Clear();
		maxRetryPasses = Math.Max(1, data.GetValueOrDefault("MaxRetryPasses", 8).AsInt32());
		foreach (Variant item in data.GetValueOrDefault("Packet", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary spawnDictionary = item.AsGodotDictionary();
			TowerDefenseLevelPreSpawnConfig towerDefenseLevelPreSpawnConfig = new TowerDefenseLevelPreSpawnConfig();
			towerDefenseLevelPreSpawnConfig.Init(spawnDictionary);
			preSpawnList.Add(towerDefenseLevelPreSpawnConfig);
		}
	}

	public void Clear()
	{
		preSpawnList.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.preSpawnList)
		{
			preSpawnList = VariantUtils.ConvertToArray<TowerDefenseLevelPreSpawnConfig>(in value);
			return true;
		}
		if (name == PropertyName.maxRetryPasses)
		{
			maxRetryPasses = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.preSpawnList)
		{
			value = VariantUtils.CreateFromArray(preSpawnList);
			return true;
		}
		if (name == PropertyName.maxRetryPasses)
		{
			value = VariantUtils.CreateFrom(in maxRetryPasses);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.preSpawnList, PropertyHint.TypeString, "24/17:TowerDefenseLevelPreSpawnConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxRetryPasses, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.preSpawnList, Variant.CreateFrom(preSpawnList));
		info.AddProperty(PropertyName.maxRetryPasses, Variant.From(in maxRetryPasses));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.preSpawnList, out var value))
		{
			preSpawnList = value.AsGodotArray<TowerDefenseLevelPreSpawnConfig>();
		}
		if (info.TryGetProperty(PropertyName.maxRetryPasses, out var value2))
		{
			maxRetryPasses = value2.As<int>();
		}
	}
}
