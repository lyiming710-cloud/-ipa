using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/RuneStorm/TowerDefenseLevelEventRuneStorm.cs")]
public class TowerDefenseLevelEventRuneStorm : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName ResolveDuration = "ResolveDuration";

		public static readonly StringName ScheduleStorm = "ScheduleStorm";

		public static readonly StringName SaveStorms = "SaveStorms";

		public static readonly StringName RestoreStorms = "RestoreStorms";

		public static readonly StringName ResolveColor = "ResolveColor";

		public static readonly StringName StormTipKey = "StormTipKey";

		public static readonly StringName SpawnStormEffect = "SpawnStormEffect";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName type = "type";

		public static readonly StringName durationMin = "durationMin";

		public static readonly StringName durationMax = "durationMax";

		public static readonly StringName formingDelay = "formingDelay";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	private const int ScreenEffectLayer = 10;

	private const double FormingTipDuration = 2.0;

	private const double StormTipDuration = 2.0;

	private const string FormingTipKey = "TOWERDEFENSE_TIPS_RUNESTORM_FORMING";

	private const string TimerStateKey = "rune_storm_state";

	private static PackedScene _runeStormEffectScene;

	[Export(PropertyHint.None, "")]
	public string type = "Random";

	[Export(PropertyHint.None, "")]
	public double durationMin = 10.0;

	[Export(PropertyHint.None, "")]
	public double durationMax = 15.0;

	[Export(PropertyHint.None, "")]
	public double formingDelay = 2.0;

	private static PackedScene RuneStormEffectScene => _runeStormEffectScene ?? (_runeStormEffectScene = GD.Load<PackedScene>("res://Prefab/ScreenEffect/RuneStorm/RuneStorm.tscn"));

	public override string _GetName()
	{
		return "LEVLE_EVENT_RUNE_STORM";
	}

	public override void Init(Dictionary valueDictionary)
	{
		type = valueDictionary.GetValueOrDefault("Type", "Random").AsString();
		durationMin = Mathf.Max(0.1, valueDictionary.GetValueOrDefault("DurationMin", 10.0).AsDouble());
		durationMax = Mathf.Max(durationMin, valueDictionary.GetValueOrDefault("DurationMax", 15.0).AsDouble());
		formingDelay = Mathf.Max(0.0, valueDictionary.GetValueOrDefault("FormingDelay", 2.0).AsDouble());
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "RuneStorm",
			["Value"] = new Dictionary
			{
				["Type"] = type,
				["DurationMax"] = durationMax,
				["DurationMin"] = durationMin,
				["FormingDelay"] = formingDelay
			}
		};
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["符文风暴"] = new Dictionary
		{
			["风暴类型"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Enum",
				["Property"] = "type",
				["Hint"] = new Dictionary
				{
					["随机"] = "Random",
					["紫色"] = "Purple",
					["蓝色"] = "Blue",
					["红色"] = "Red",
					["橙色"] = "Orange",
					["绿色"] = "Green"
				},
				["Rest"] = "Random"
			},
			["随机下限"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Float",
				["Property"] = "durationMin",
				["Rest"] = 10.0
			},
			["随机上限"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Float",
				["Property"] = "durationMax",
				["Rest"] = 15.0
			},
			["形成延时"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Float",
				["Property"] = "formingDelay",
				["Rest"] = 2.0
			}
		};
		return property;
	}

	public override void Execute()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			TowerDefenseControlNew currentControl = instance.currentControl;
			if (GodotObject.IsInstanceValid(currentControl) && currentControl.IsInsideTree())
			{
				TowerDefenseRuneMagicColor color = ResolveColor();
				double safeDuration = ResolveDuration();
				double num = Mathf.Max(0.0, formingDelay);
				ScheduleStorm(currentControl, color, safeDuration, num, Mathf.Min(2.0, num));
			}
		}
	}

	private double ResolveDuration()
	{
		double num = Mathf.Max(0.1, Mathf.Min(durationMin, durationMax));
		double to = Mathf.Max(num, Mathf.Max(durationMin, durationMax));
		return GD.RandRange(num, to);
	}

	private static void ScheduleStorm(TowerDefenseControlNew control, TowerDefenseRuneMagicColor color, double safeDuration, double safeDelay, double noticeRemaining)
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		if (noticeRemaining > 0.0)
		{
			RuneWeatherNotice.Show(control, "TOWERDEFENSE_TIPS_RUNESTORM_FORMING", noticeRemaining);
		}
		if (safeDelay <= 0.0)
		{
			BeginStorm();
			return;
		}
		Timer timer = new Timer
		{
			OneShot = true,
			WaitTime = safeDelay
		};
		timer.SetMeta("rune_storm_state", new Dictionary
		{
			["color"] = TowerDefenseRuneMagicUtil.ToKey(color),
			["duration"] = safeDuration,
			["delay"] = safeDelay,
			["noticeRemaining"] = noticeRemaining
		});
		control.AddChild(timer, forceReadableName: false, Node.InternalMode.Disabled);
		timer.Timeout += () =>
		{
			timer.QueueFree();
			BeginStorm();
		};
		timer.Start();
		void BeginStorm()
		{
			if (GodotObject.IsInstanceValid(control) && control.IsInsideTree() && !control.IsQueuedForDeletion() && manager.currentControl == control)
			{
				TowerDefenseBattleProcess process = control.process;
				if (process == null || process.IsLifetimeActive)
				{
					manager.TipsPlay(StormTipKey(color));
					TowerDefenseRuneMagicUtil.ApplyStormInstant(color, safeDuration);
					SpawnStormEffect(control, color, safeDuration);
				}
			}
		}
	}

	public static Dictionary SaveStorms(TowerDefenseControlNew control)
	{
		Array array = new Array();
		Array array2 = new Array();
		foreach (Node child in control.GetChildren())
		{
			if (child is Timer timer && !timer.IsQueuedForDeletion() && timer.HasMeta("rune_storm_state"))
			{
				Dictionary dictionary = timer.GetMeta("rune_storm_state").AsGodotDictionary().Duplicate(deep: true);
				double num = dictionary["delay"].AsDouble() - timer.TimeLeft;
				dictionary["noticeRemaining"] = Mathf.Max(0.0, dictionary["noticeRemaining"].AsDouble() - num);
				dictionary["delay"] = timer.TimeLeft;
				array.Add(dictionary);
			}
		}
		if (control.uilayerDictionary.TryGetValue(10, out var value))
		{
			foreach (Node child2 in value.GetChildren())
			{
				if (child2 is RuneStormEffect runeStormEffect && !runeStormEffect.IsQueuedForDeletion())
				{
					array2.Add(runeStormEffect.SaveState());
				}
			}
		}
		return new Dictionary
		{
			["forming"] = array,
			["active"] = array2
		};
	}

	public static void RestoreStorms(TowerDefenseControlNew control, Dictionary state)
	{
		foreach (Variant item in state.GetValueOrDefault("forming", new Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			if (TowerDefenseRuneMagicUtil.TryParse(dictionary.GetValueOrDefault("color", "").AsString(), out var color))
			{
				ScheduleStorm(control, color, Mathf.Max(0.1, dictionary["duration"].AsDouble()), Mathf.Max(0.0, dictionary["delay"].AsDouble()), Mathf.Max(0.0, dictionary["noticeRemaining"].AsDouble()));
			}
		}
		foreach (Variant item2 in state.GetValueOrDefault("active", new Array()).AsGodotArray())
		{
			Dictionary dictionary2 = item2.AsGodotDictionary();
			if (TowerDefenseRuneMagicUtil.TryParse(dictionary2.GetValueOrDefault("color", "").AsString(), out var color2))
			{
				RuneStormEffect runeStormEffect = RuneStormEffectScene.Instantiate<RuneStormEffect>(PackedScene.GenEditState.Disabled);
				runeStormEffect.LoadState(dictionary2);
				control.AddUI(runeStormEffect, 10);
				double num = 2.0 - dictionary2.GetValueOrDefault("elapsed", 0.0).AsDouble();
				if (num > 0.0)
				{
					TowerDefenseManager.Instance.TipsPlay(StormTipKey(color2), num);
				}
			}
		}
	}

	private TowerDefenseRuneMagicColor ResolveColor()
	{
		if (TowerDefenseRuneMagicUtil.TryParse(type, out var color))
		{
			return color;
		}
		return TowerDefenseRuneMagicUtil.RandomStormColor();
	}

	private static string StormTipKey(TowerDefenseRuneMagicColor color)
	{
		return color switch
		{
			TowerDefenseRuneMagicColor.Purple => "TOWERDEFENSE_TIPS_RUNESTORM_PURPLE", 
			TowerDefenseRuneMagicColor.Blue => "TOWERDEFENSE_TIPS_RUNESTORM_BLUE", 
			TowerDefenseRuneMagicColor.Red => "TOWERDEFENSE_TIPS_RUNESTORM_RED", 
			TowerDefenseRuneMagicColor.Orange => "TOWERDEFENSE_TIPS_RUNESTORM_ORANGE", 
			TowerDefenseRuneMagicColor.Green => "TOWERDEFENSE_TIPS_RUNESTORM_GREEN", 
			_ => "TOWERDEFENSE_TIPS_RUNESTORM_FORMING", 
		};
	}

	private static void SpawnStormEffect(TowerDefenseControlNew control, TowerDefenseRuneMagicColor color, double time)
	{
		if (GodotObject.IsInstanceValid(control) && GodotObject.IsInstanceValid(RuneStormEffectScene))
		{
			RuneStormEffect runeStormEffect = RuneStormEffectScene.Instantiate<RuneStormEffect>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(runeStormEffect))
			{
				runeStormEffect.Setup(color, time);
				control.AddUI(runeStormEffect, 10);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveDuration, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleStorm, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "safeDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "safeDelay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "noticeRemaining", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveStorms, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreStorms, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveColor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StormTipKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnStormEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
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
		if (method == MethodName.GetProperty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetProperty());
			return true;
		}
		if (method == MethodName.Execute && args.Count == 0)
		{
			Execute();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDuration && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(ResolveDuration());
			return true;
		}
		if (method == MethodName.ScheduleStorm && args.Count == 5)
		{
			ScheduleStorm(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveStorms && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveStorms(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0])));
			return true;
		}
		if (method == MethodName.RestoreStorms && args.Count == 2)
		{
			RestoreStorms(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveColor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseRuneMagicColor>(ResolveColor());
			return true;
		}
		if (method == MethodName.StormTipKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StormTipKey(VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[0])));
			return true;
		}
		if (method == MethodName.SpawnStormEffect && args.Count == 3)
		{
			SpawnStormEffect(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ScheduleStorm && args.Count == 5)
		{
			ScheduleStorm(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveStorms && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveStorms(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0])));
			return true;
		}
		if (method == MethodName.RestoreStorms && args.Count == 2)
		{
			RestoreStorms(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StormTipKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StormTipKey(VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[0])));
			return true;
		}
		if (method == MethodName.SpawnStormEffect && args.Count == 3)
		{
			SpawnStormEffect(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.GetProperty)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.ResolveDuration)
		{
			return true;
		}
		if (method == MethodName.ScheduleStorm)
		{
			return true;
		}
		if (method == MethodName.SaveStorms)
		{
			return true;
		}
		if (method == MethodName.RestoreStorms)
		{
			return true;
		}
		if (method == MethodName.ResolveColor)
		{
			return true;
		}
		if (method == MethodName.StormTipKey)
		{
			return true;
		}
		if (method == MethodName.SpawnStormEffect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.durationMin)
		{
			durationMin = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.durationMax)
		{
			durationMax = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.formingDelay)
		{
			formingDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.durationMin)
		{
			value = VariantUtils.CreateFrom(in durationMin);
			return true;
		}
		if (name == PropertyName.durationMax)
		{
			value = VariantUtils.CreateFrom(in durationMax);
			return true;
		}
		if (name == PropertyName.formingDelay)
		{
			value = VariantUtils.CreateFrom(in formingDelay);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.durationMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.durationMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.formingDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.durationMin, Variant.From(in durationMin));
		info.AddProperty(PropertyName.durationMax, Variant.From(in durationMax));
		info.AddProperty(PropertyName.formingDelay, Variant.From(in formingDelay));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.type, out var value))
		{
			type = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.durationMin, out var value2))
		{
			durationMin = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.durationMax, out var value3))
		{
			durationMax = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.formingDelay, out var value4))
		{
			formingDelay = value4.As<double>();
		}
	}
}
