using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelSurvivalRunner.cs")]
public class TowerDefenseLevelSurvivalRunner : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName WaveReach = "WaveReach";

		public static readonly StringName RoundReach = "RoundReach";

		public static readonly StringName AdvanceRoundState = "AdvanceRoundState";

		public static readonly StringName RestoreProgressState = "RestoreProgressState";

		public static readonly StringName BeginRoundTransition = "BeginRoundTransition";

		public static readonly StringName CalculateCurrentRoundPointBudget = "CalculateCurrentRoundPointBudget";

		public static readonly StringName RefreshCurrentZombiePool = "RefreshCurrentZombiePool";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName config = "config";

		public static readonly StringName point = "point";

		public static readonly StringName zombiePool = "zombiePool";

		public static readonly StringName addZombiePoolReachId = "addZombiePoolReachId";

		public static readonly StringName roundNum = "roundNum";

		public static readonly StringName currentZombiePool = "currentZombiePool";

		public static readonly StringName _currentZombiePoolRound = "_currentZombiePoolRound";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelSurvivalConfig config;

	[Export(PropertyHint.None, "")]
	public int point;

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Array zombiePool = new Godot.Collections.Array();

	[Export(PropertyHint.None, "")]
	public int addZombiePoolReachId;

	[Export(PropertyHint.None, "")]
	public int roundNum;

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Array currentZombiePool = new Godot.Collections.Array();

	private int _currentZombiePoolRound = -2147483648;

	public void Init(TowerDefenseLevelSurvivalConfig _config)
	{
		config = _config.Duplicate() as TowerDefenseLevelSurvivalConfig;
		point = config.pointBegin;
		zombiePool = config.zombiePoolBase.Duplicate();
		addZombiePoolReachId = 0;
		roundNum = 0;
		if (currentZombiePool == null)
		{
			currentZombiePool = new Godot.Collections.Array();
		}
		_currentZombiePoolRound = -2147483648;
		if (point > config.pointMax)
		{
			point = config.pointMax;
		}
		RefreshCurrentZombiePool();
	}

	public void WaveReach(int waveId, bool isBigWave)
	{
		if (!isBigWave)
		{
			point += config.pointIncrementPerWave;
		}
		else
		{
			point += config.pointIncrementPerBigWave;
		}
		if (point > config.pointMax)
		{
			point = config.pointMax;
		}
	}

	public void RoundReach(int _roundNum)
	{
		AdvanceRoundState(_roundNum);
		BeginRoundTransition();
	}

	public void AdvanceRoundState(int _roundNum)
	{
		roundNum = _roundNum;
		point += config.pointIncrementPerRound;
		if (point > config.pointMax)
		{
			point = config.pointMax;
		}
		for (int i = Mathf.Clamp(addZombiePoolReachId, 0, config.zombiePoolRoundAdd.Count); i < config.zombiePoolRoundAdd.Count; i++)
		{
			TowerDefenseLevelSurvivalZombiePoolRoundAddConfig towerDefenseLevelSurvivalZombiePoolRoundAddConfig = config.zombiePoolRoundAdd[i];
			if (towerDefenseLevelSurvivalZombiePoolRoundAddConfig.round > roundNum)
			{
				break;
			}
			foreach (Variant zombie in towerDefenseLevelSurvivalZombiePoolRoundAddConfig.zombieList)
			{
				zombiePool.Add(zombie);
			}
			addZombiePoolReachId = i + 1;
		}
		RefreshCurrentZombiePool();
	}

	public void RestoreProgressState(int savedPoint, int savedRoundNum, Godot.Collections.Array savedCurrentZombiePool)
	{
		point = Mathf.Clamp(savedPoint, 0, config.pointMax);
		roundNum = Mathf.Max(0, savedRoundNum);
		zombiePool = config.zombiePoolBase?.Duplicate() ?? new Godot.Collections.Array();
		addZombiePoolReachId = 0;
		for (int i = 0; i < config.zombiePoolRoundAdd.Count; i++)
		{
			TowerDefenseLevelSurvivalZombiePoolRoundAddConfig towerDefenseLevelSurvivalZombiePoolRoundAddConfig = config.zombiePoolRoundAdd[i];
			if (towerDefenseLevelSurvivalZombiePoolRoundAddConfig.round > roundNum)
			{
				break;
			}
			foreach (Variant zombie in towerDefenseLevelSurvivalZombiePoolRoundAddConfig.zombieList)
			{
				zombiePool.Add(zombie);
			}
			addZombiePoolReachId = i + 1;
		}
		currentZombiePool = savedCurrentZombiePool?.Duplicate() ?? new Godot.Collections.Array();
		_currentZombiePoolRound = ((currentZombiePool.Count > 0) ? roundNum : (-2147483648));
		RefreshCurrentZombiePool();
	}

	public void BeginRoundTransition()
	{
		if (config.roundDayNightChange)
		{
			TowerDefenseManager.Instance.MapDayNightSwitch(5.0, -1.0);
		}
	}

	public long CalculateCurrentRoundPointBudget(int waveCount, int flagWaveInterval, int completedWaveCount = 0)
	{
		if (waveCount <= 0)
		{
			return 0L;
		}
		int num = point;
		long num2 = 0L;
		long num5;
		int num4;
		int b;
		int num3;
		for (int i = Mathf.Clamp(completedWaveCount, 0, waveCount) + 1; i <= waveCount; num5 = num4, num = (int)Math.Min(num + num5, config.pointMax), b = ((num3 != 0) ? ((int)Mathf.Floor((double)num * config.pointBigWaveScale)) : num), num2 += Mathf.Max(0, b), i++)
		{
			if (flagWaveInterval > 0)
			{
				num3 = ((i % flagWaveInterval == 0) ? 1 : 0);
				if (num3 != 0)
				{
					num4 = config.pointIncrementPerBigWave;
					continue;
				}
			}
			else
			{
				num3 = 0;
			}
			num4 = config.pointIncrementPerWave;
		}
		return num2;
	}

	public Godot.Collections.Array RefreshCurrentZombiePool()
	{
		if (_currentZombiePoolRound == roundNum)
		{
			return currentZombiePool;
		}
		currentZombiePool.Clear();
		Godot.Collections.Array array = zombiePool.Duplicate();
		int num = Mathf.Min(GD.RandRange(7, 9), array.Count);
		for (int i = 0; i < num; i++)
		{
			int index = GD.RandRange(0, array.Count - 1);
			currentZombiePool.Add(array[index]);
			array.RemoveAt(index);
		}
		_currentZombiePoolRound = roundNum;
		return currentZombiePool;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.WaveReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isBigWave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RoundReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_roundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceRoundState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_roundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreProgressState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "savedPoint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "savedRoundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "savedCurrentZombiePool", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginRoundTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CalculateCurrentRoundPointBudget, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "flagWaveInterval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "completedWaveCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCurrentZombiePool, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelSurvivalConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WaveReach && args.Count == 2)
		{
			WaveReach(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RoundReach && args.Count == 1)
		{
			RoundReach(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceRoundState && args.Count == 1)
		{
			AdvanceRoundState(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreProgressState && args.Count == 3)
		{
			RestoreProgressState(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginRoundTransition && args.Count == 0)
		{
			BeginRoundTransition();
			ret = default;
			return true;
		}
		if (method == MethodName.CalculateCurrentRoundPointBudget && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<long>(CalculateCurrentRoundPointBudget(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.RefreshCurrentZombiePool && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(RefreshCurrentZombiePool());
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
		if (method == MethodName.WaveReach)
		{
			return true;
		}
		if (method == MethodName.RoundReach)
		{
			return true;
		}
		if (method == MethodName.AdvanceRoundState)
		{
			return true;
		}
		if (method == MethodName.RestoreProgressState)
		{
			return true;
		}
		if (method == MethodName.BeginRoundTransition)
		{
			return true;
		}
		if (method == MethodName.CalculateCurrentRoundPointBudget)
		{
			return true;
		}
		if (method == MethodName.RefreshCurrentZombiePool)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelSurvivalConfig>(in value);
			return true;
		}
		if (name == PropertyName.point)
		{
			point = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.zombiePool)
		{
			zombiePool = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.addZombiePoolReachId)
		{
			addZombiePoolReachId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.roundNum)
		{
			roundNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentZombiePool)
		{
			currentZombiePool = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName._currentZombiePoolRound)
		{
			_currentZombiePoolRound = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.point)
		{
			value = VariantUtils.CreateFrom(in point);
			return true;
		}
		if (name == PropertyName.zombiePool)
		{
			value = VariantUtils.CreateFrom(in zombiePool);
			return true;
		}
		if (name == PropertyName.addZombiePoolReachId)
		{
			value = VariantUtils.CreateFrom(in addZombiePoolReachId);
			return true;
		}
		if (name == PropertyName.roundNum)
		{
			value = VariantUtils.CreateFrom(in roundNum);
			return true;
		}
		if (name == PropertyName.currentZombiePool)
		{
			value = VariantUtils.CreateFrom(in currentZombiePool);
			return true;
		}
		if (name == PropertyName._currentZombiePoolRound)
		{
			value = VariantUtils.CreateFrom(in _currentZombiePoolRound);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.ResourceType, "TowerDefenseLevelSurvivalConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.point, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.zombiePool, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.addZombiePoolReachId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.roundNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentZombiePool, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentZombiePoolRound, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.point, Variant.From(in point));
		info.AddProperty(PropertyName.zombiePool, Variant.From(in zombiePool));
		info.AddProperty(PropertyName.addZombiePoolReachId, Variant.From(in addZombiePoolReachId));
		info.AddProperty(PropertyName.roundNum, Variant.From(in roundNum));
		info.AddProperty(PropertyName.currentZombiePool, Variant.From(in currentZombiePool));
		info.AddProperty(PropertyName._currentZombiePoolRound, Variant.From(in _currentZombiePoolRound));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.config, out var value))
		{
			config = value.As<TowerDefenseLevelSurvivalConfig>();
		}
		if (info.TryGetProperty(PropertyName.point, out var value2))
		{
			point = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.zombiePool, out var value3))
		{
			zombiePool = value3.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.addZombiePoolReachId, out var value4))
		{
			addZombiePoolReachId = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.roundNum, out var value5))
		{
			roundNum = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentZombiePool, out var value6))
		{
			currentZombiePool = value6.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName._currentZombiePoolRound, out var value7))
		{
			_currentZombiePoolRound = value7.As<int>();
		}
	}
}
