using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BuffNetworkJsonRoundTripProbe.cs")]
public class BuffNetworkJsonRoundTripProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunRuntimeCloneChecks = "RunRuntimeCloneChecks";

		public static readonly StringName RunRoundTrip = "RunRoundTrip";

		public static readonly StringName IsWireValue = "IsWireValue";

		public static readonly StringName FindBuff = "FindBuff";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string SplatScenePath = "res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn";

	private int _checks;

	private int _failures;

	public override void _Ready()
	{
		try
		{
			RunRoundTrip();
			RunRuntimeCloneChecks();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"BUFF_NETWORK_JSON_FAILURE unexpected={value}");
		}
		bool flag = _failures == 0;
		GD.Print($"BUFF_NETWORK_JSON_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void RunRuntimeCloneChecks()
	{
		string[] array = new string[19]
		{
			"Frozen", "Burn", "Hypnoses", "IceSpeedDown", "Dizziness", "Butter", "Cherry", "Pogo", "Sleep", "FireHit",
			"JalaHit", "RedHeat", "Poisoning", "Fluorescence", "NormalHit", "Squid", "TabooBean", "Coffee", "AttackSpeedDown"
		};
		foreach (string text in array)
		{
			TowerDefenseCharacterBuffConfig towerDefenseCharacterBuffConfig = TowerDefenseCharacterBuffConfig.CreateBuffByKey(text);
			TowerDefenseCharacterBuffConfig towerDefenseCharacterBuffConfig2 = towerDefenseCharacterBuffConfig?.CreateRuntimeInstance();
			Check(towerDefenseCharacterBuffConfig != null && towerDefenseCharacterBuffConfig2 != null, "Built-in Buff '" + text + "' must create a runtime root.");
			Check(towerDefenseCharacterBuffConfig2 != null && towerDefenseCharacterBuffConfig2.GetType() == towerDefenseCharacterBuffConfig.GetType(), "Built-in Buff '" + text + "' must preserve its exact runtime type.");
			Check(towerDefenseCharacterBuffConfig2 != null && towerDefenseCharacterBuffConfig2 != towerDefenseCharacterBuffConfig, "Built-in Buff '" + text + "' must isolate its mutable runtime root.");
			towerDefenseCharacterBuffConfig2?.Dispose();
			towerDefenseCharacterBuffConfig?.Dispose();
		}
		PackedScene packedScene = GD.Load<PackedScene>("res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn");
		TowerDefenseCharacterBuffBurn towerDefenseCharacterBuffBurn = new TowerDefenseCharacterBuffBurn
		{
			key = "CustomBurn",
			refresh = false,
			canFliter = false,
			time = 9.25,
			dpsAttack = 321.5,
			splatSceneType = "Sprite",
			splatScene = packedScene,
			splatInterval = 0.125,
			currentTime = 1.75,
			splatTime = 0.0625
		};
		TowerDefenseCharacterBuffBurn towerDefenseCharacterBuffBurn2 = towerDefenseCharacterBuffBurn.CreateRuntimeInstance() as TowerDefenseCharacterBuffBurn;
		Check(towerDefenseCharacterBuffBurn2 != null && towerDefenseCharacterBuffBurn2.key == towerDefenseCharacterBuffBurn.key && towerDefenseCharacterBuffBurn2.refresh == towerDefenseCharacterBuffBurn.refresh && towerDefenseCharacterBuffBurn2.canFliter == towerDefenseCharacterBuffBurn.canFliter, "Runtime Buff root must preserve base exported configuration.");
		Check(towerDefenseCharacterBuffBurn2 != null && towerDefenseCharacterBuffBurn2.time == towerDefenseCharacterBuffBurn.time && towerDefenseCharacterBuffBurn2.dpsAttack == towerDefenseCharacterBuffBurn.dpsAttack && towerDefenseCharacterBuffBurn2.splatSceneType == towerDefenseCharacterBuffBurn.splatSceneType && towerDefenseCharacterBuffBurn2.splatInterval == towerDefenseCharacterBuffBurn.splatInterval && towerDefenseCharacterBuffBurn2.currentTime == towerDefenseCharacterBuffBurn.currentTime && towerDefenseCharacterBuffBurn2.splatTime == towerDefenseCharacterBuffBurn.splatTime, "Runtime Burn root must preserve all exported scalar fields.");
		Check(towerDefenseCharacterBuffBurn2 != null && towerDefenseCharacterBuffBurn2.splatScene == packedScene, "Runtime Burn root must share its immutable nested scene Resource.");
		TowerDefenseCharacterBuffHypnoses towerDefenseCharacterBuffHypnoses = new TowerDefenseCharacterBuffHypnoses
		{
			deathFreezeOver = true,
			enableTorchwood = true,
			torchwoodAreaSize = new Vector2(37.5f, 82.25f),
			enableSunProduce = true,
			sunProduceInterval = 2.25,
			sunProduceNum = 75
		};
		TowerDefenseCharacterBuffHypnoses towerDefenseCharacterBuffHypnoses2 = towerDefenseCharacterBuffHypnoses.CreateRuntimeInstance() as TowerDefenseCharacterBuffHypnoses;
		Check(towerDefenseCharacterBuffHypnoses2 != null && towerDefenseCharacterBuffHypnoses2.deathFreezeOver && towerDefenseCharacterBuffHypnoses2.enableTorchwood && towerDefenseCharacterBuffHypnoses2.torchwoodAreaSize == towerDefenseCharacterBuffHypnoses.torchwoodAreaSize && towerDefenseCharacterBuffHypnoses2.enableSunProduce && towerDefenseCharacterBuffHypnoses2.sunProduceInterval == towerDefenseCharacterBuffHypnoses.sunProduceInterval && towerDefenseCharacterBuffHypnoses2.sunProduceNum == towerDefenseCharacterBuffHypnoses.sunProduceNum, "Runtime Hypnoses root must preserve exported fields and properties.");
		TowerDefenseCharacterBuffAttackSpeedDown towerDefenseCharacterBuffAttackSpeedDown = new TowerDefenseCharacterBuffAttackSpeedDown
		{
			timeScaleValue = 0.375,
			time = 9.5,
			currentTime = 2.25
		};
		towerDefenseCharacterBuffAttackSpeedDown._Init();
		TowerDefenseCharacterBuffAttackSpeedDown towerDefenseCharacterBuffAttackSpeedDown2 = towerDefenseCharacterBuffAttackSpeedDown.CreateRuntimeInstance() as TowerDefenseCharacterBuffAttackSpeedDown;
		Check(towerDefenseCharacterBuffAttackSpeedDown2 != null && towerDefenseCharacterBuffAttackSpeedDown2.key == "AttackSpeedDown" && towerDefenseCharacterBuffAttackSpeedDown2.timeScaleValue == towerDefenseCharacterBuffAttackSpeedDown.timeScaleValue && towerDefenseCharacterBuffAttackSpeedDown2.time == towerDefenseCharacterBuffAttackSpeedDown.time && towerDefenseCharacterBuffAttackSpeedDown2.currentTime == towerDefenseCharacterBuffAttackSpeedDown.currentTime, "Runtime AttackSpeedDown root must preserve its multiplier and timer fields.");
		TowerDefenseCharacterBuffAttackSpeedDown towerDefenseCharacterBuffAttackSpeedDown3 = new TowerDefenseCharacterBuffAttackSpeedDown
		{
			timeScaleValue = 0.5,
			time = 2.0
		};
		TowerDefenseCharacter towerDefenseCharacter = (towerDefenseCharacterBuffAttackSpeedDown3.character = new TowerDefenseCharacter
		{
			timeScale = 0.8
		});
		Check(!towerDefenseCharacterBuffAttackSpeedDown3.Step(1.0) && Mathf.IsEqualApprox((float)towerDefenseCharacter.timeScale, 0.8f) && Mathf.IsEqualApprox((float)towerDefenseCharacterBuffAttackSpeedDown3.GetAttackSpeedMultiplier(), 0.5f), "AttackSpeedDown must expose an attack-only multiplier without changing the character clock.");
		Check(towerDefenseCharacterBuffAttackSpeedDown3.Step(1.0) && Mathf.IsEqualApprox((float)towerDefenseCharacterBuffAttackSpeedDown3.GetAttackSpeedMultiplier(), 1f), "Expired AttackSpeedDown must stop affecting attack cadence.");
		TowerDefenseCharacterBuffAttackSpeedDown towerDefenseCharacterBuffAttackSpeedDown4 = new TowerDefenseCharacterBuffAttackSpeedDown
		{
			timeScaleValue = 0.5,
			time = 15.0
		};
		towerDefenseCharacterBuffAttackSpeedDown4._Init();
		BuffComponent buffComponent = new BuffComponent();
		buffComponent.buffDictionary.Add(towerDefenseCharacterBuffAttackSpeedDown4.key, towerDefenseCharacterBuffAttackSpeedDown4);
		towerDefenseCharacter.buff = buffComponent;
		towerDefenseCharacter.timeScale = 1.0;
		AttackComponent attackComponent = new AttackComponent
		{
			parent = towerDefenseCharacter,
			timer = 2.0,
			timeScale = 1.0
		};
		attackComponent.BatchUpdateValidated(1.0);
		Check(Mathf.IsEqualApprox((float)attackComponent.timer, 1.5f), "AttackComponent cooldowns must consume the attack-only multiplier.");
		towerDefenseCharacterBuffBurn2?.Dispose();
		towerDefenseCharacterBuffBurn.Dispose();
		towerDefenseCharacterBuffHypnoses2?.Dispose();
		towerDefenseCharacterBuffHypnoses.Dispose();
		towerDefenseCharacterBuffAttackSpeedDown2?.Dispose();
		towerDefenseCharacterBuffAttackSpeedDown.Dispose();
		towerDefenseCharacterBuffAttackSpeedDown3.Dispose();
		towerDefenseCharacterBuffAttackSpeedDown4.Dispose();
		towerDefenseCharacter.Free();
	}

	private void RunRoundTrip()
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn");
		Check(packedScene != null, "The real Burn splat fixture must load.");
		TowerDefenseCharacterBuffBurn towerDefenseCharacterBuffBurn = new TowerDefenseCharacterBuffBurn
		{
			splatScene = packedScene,
			splatSceneType = "Particles",
			splatInterval = 0.75
		};
		towerDefenseCharacterBuffBurn._Init();
		TowerDefenseCharacterBuffHypnoses towerDefenseCharacterBuffHypnoses = new TowerDefenseCharacterBuffHypnoses
		{
			torchwoodAreaSize = new Vector2(123.5f, 67.25f)
		};
		towerDefenseCharacterBuffHypnoses._Init();
		BuffComponent buffComponent = new BuffComponent
		{
			buffDictionary = 
			{
				{
					towerDefenseCharacterBuffBurn.key,
					(TowerDefenseCharacterBuffConfig)towerDefenseCharacterBuffBurn
				},
				{
					towerDefenseCharacterBuffHypnoses.key,
					(TowerDefenseCharacterBuffConfig)towerDefenseCharacterBuffHypnoses
				}
			}
		};
		Array<Dictionary> buffs = buffComponent.ExportSave();
		Dictionary dictionary = FindBuff(buffs, "Burn");
		Dictionary dictionary2 = FindBuff(buffs, "Hypnoses");
		Check(dictionary["splatScene"].AsGodotObject() is PackedScene, "Progress export must retain the PackedScene Variant instead of adopting the JSON wire encoding.");
		Check(dictionary2["torchwoodAreaSize"].AsVector2() == towerDefenseCharacterBuffHypnoses.torchwoodAreaSize, "Progress export must retain the native Vector2 Variant.");
		Variant variant = Json.ParseString(Json.Stringify(buffComponent.SyncSerialize()));
		Check(variant.VariantType == Variant.Type.Dictionary, "Buff component snapshot must survive JSON parsing as a Dictionary.");
		Godot.Collections.Array buffs2 = variant.AsGodotDictionary()["buffs"].AsGodotArray();
		Dictionary dictionary3 = FindBuff(buffs2, "Burn");
		Dictionary dictionary4 = FindBuff(buffs2, "Hypnoses");
		Check(IsWireValue(dictionary3, "splatScene", "resource"), "PackedScene must use an explicit JSON-safe resource envelope.");
		Dictionary dictionary5 = dictionary3["splatScene"].AsGodotDictionary();
		Check(dictionary5.GetValueOrDefault("path", "").AsString() == "res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn", "PackedScene wire envelope must retain its loadable res:// path.");
		Check(IsWireValue(dictionary4, "torchwoodAreaSize", "vector2"), "Vector2 must use an explicit JSON-safe vector envelope.");
		Dictionary dictionary6 = dictionary4["torchwoodAreaSize"].AsGodotDictionary();
		Check(Mathf.IsEqualApprox((float)dictionary6.GetValueOrDefault("x", 0.0).AsDouble(), 123.5f) && Mathf.IsEqualApprox((float)dictionary6.GetValueOrDefault("y", 0.0).AsDouble(), 67.25f), "Vector2 wire envelope must retain both numeric coordinates.");
		System.Reflection.MethodInfo method = typeof(BuffComponent).GetMethod("ApplyBuffFields", BindingFlags.Static | BindingFlags.NonPublic);
		Check(method != null, "Buff field application route must remain available to the runtime probe.");
		TowerDefenseCharacterBuffBurn towerDefenseCharacterBuffBurn2 = new TowerDefenseCharacterBuffBurn();
		towerDefenseCharacterBuffBurn2._Init();
		TowerDefenseCharacterBuffHypnoses towerDefenseCharacterBuffHypnoses2 = new TowerDefenseCharacterBuffHypnoses();
		towerDefenseCharacterBuffHypnoses2._Init();
		method?.Invoke(null, new object[2] { towerDefenseCharacterBuffBurn2, dictionary3 });
		method?.Invoke(null, new object[2] { towerDefenseCharacterBuffHypnoses2, dictionary4 });
		Check(towerDefenseCharacterBuffBurn2.splatScene != null && towerDefenseCharacterBuffBurn2.splatScene.ResourcePath == "res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn", "JSON-decoded Buff fields must restore the PackedScene Resource.");
		Check(towerDefenseCharacterBuffHypnoses2.torchwoodAreaSize == towerDefenseCharacterBuffHypnoses.torchwoodAreaSize, "JSON-decoded Buff fields must restore the native Vector2.");
		towerDefenseCharacterBuffBurn.Dispose();
		towerDefenseCharacterBuffHypnoses.Dispose();
		towerDefenseCharacterBuffBurn2.Dispose();
		towerDefenseCharacterBuffHypnoses2.Dispose();
	}

	private static bool IsWireValue(Dictionary data, string field, string type)
	{
		if (!data.TryGetValue(field, out var value) || value.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		return value.AsGodotDictionary().GetValueOrDefault("__type", "").AsString() == type;
	}

	private static Dictionary FindBuff(Array<Dictionary> buffs, string key)
	{
		for (int i = 0; i < buffs.Count; i++)
		{
			Dictionary dictionary = buffs[i];
			if (dictionary.GetValueOrDefault("key", "").AsString() == key)
			{
				return dictionary;
			}
		}
		return new Dictionary();
	}

	private static Dictionary FindBuff(Godot.Collections.Array buffs, string key)
	{
		for (int i = 0; i < buffs.Count; i++)
		{
			Dictionary dictionary = buffs[i].AsGodotDictionary();
			if (dictionary.GetValueOrDefault("key", "").AsString() == key)
			{
				return dictionary;
			}
		}
		return new Dictionary();
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("BUFF_NETWORK_JSON_FAILURE " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(6)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunRuntimeCloneChecks, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RunRoundTrip, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.IsWireValue, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindBuff, new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Array, "buffs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.RunRuntimeCloneChecks && args.Count == 0)
		{
			RunRuntimeCloneChecks();
			ret = default;
			return true;
		}
		if (method == MethodName.RunRoundTrip && args.Count == 0)
		{
			RunRoundTrip();
			ret = default;
			return true;
		}
		if (method == MethodName.IsWireValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWireValue(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.FindBuff && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(FindBuff(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsWireValue && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWireValue(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.FindBuff && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(FindBuff(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.RunRuntimeCloneChecks)
		{
			return true;
		}
		if (method == MethodName.RunRoundTrip)
		{
			return true;
		}
		if (method == MethodName.IsWireValue)
		{
			return true;
		}
		if (method == MethodName.FindBuff)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
	}
}
