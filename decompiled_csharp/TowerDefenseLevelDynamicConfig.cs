using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelDynamicConfig.cs")]
public class TowerDefenseLevelDynamicConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName pointIncrementPerWave = "pointIncrementPerWave";

		public static readonly StringName startingPoints = "startingPoints";

		public static readonly StringName startingWave = "startingWave";

		public static readonly StringName zombiePool = "zombiePool";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int pointIncrementPerWave;

	[Export(PropertyHint.None, "")]
	public int startingPoints;

	[Export(PropertyHint.None, "")]
	public int startingWave;

	[Export(PropertyHint.None, "")]
	public Array<string> zombiePool = new Array<string>();

	public void Init(Dictionary dynamicDictionary)
	{
		pointIncrementPerWave = dynamicDictionary.GetValueOrDefault("PointIncrementPerWave", 0).AsInt32();
		startingPoints = dynamicDictionary.GetValueOrDefault("StartingPoints", 0).AsInt32();
		startingWave = dynamicDictionary.GetValueOrDefault("StartingWave", 0).AsInt32();
		Array array = dynamicDictionary.GetValueOrDefault("ZombiePool", new Array()).AsGodotArray();
		zombiePool = new Array<string>();
		foreach (Variant item in array)
		{
			zombiePool.Add(item.AsString());
		}
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["PointIncrementPerWave"] = pointIncrementPerWave,
			["StartingPoints"] = startingPoints,
			["StartingWave"] = startingWave,
			["ZombiePool"] = zombiePool
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dynamicDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
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
		if (method == MethodName.Export)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pointIncrementPerWave)
		{
			pointIncrementPerWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.startingPoints)
		{
			startingPoints = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.startingWave)
		{
			startingWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.zombiePool)
		{
			zombiePool = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.pointIncrementPerWave)
		{
			value = VariantUtils.CreateFrom(in pointIncrementPerWave);
			return true;
		}
		if (name == PropertyName.startingPoints)
		{
			value = VariantUtils.CreateFrom(in startingPoints);
			return true;
		}
		if (name == PropertyName.startingWave)
		{
			value = VariantUtils.CreateFrom(in startingWave);
			return true;
		}
		if (name == PropertyName.zombiePool)
		{
			value = VariantUtils.CreateFromArray(zombiePool);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.pointIncrementPerWave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.startingPoints, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.startingWave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.zombiePool, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pointIncrementPerWave, Variant.From(in pointIncrementPerWave));
		info.AddProperty(PropertyName.startingPoints, Variant.From(in startingPoints));
		info.AddProperty(PropertyName.startingWave, Variant.From(in startingWave));
		info.AddProperty(PropertyName.zombiePool, Variant.CreateFrom(zombiePool));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pointIncrementPerWave, out var value))
		{
			pointIncrementPerWave = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.startingPoints, out var value2))
		{
			startingPoints = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.startingWave, out var value3))
		{
			startingWave = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.zombiePool, out var value4))
		{
			zombiePool = value4.AsGodotArray<string>();
		}
	}
}
