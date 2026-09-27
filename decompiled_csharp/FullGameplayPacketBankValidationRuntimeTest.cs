using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/FullGameplayPacketBankValidationRuntimeTest.cs")]
public class FullGameplayPacketBankValidationRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunFixtures = "RunFixtures";

		public static readonly StringName CreateValidRegistry = "CreateValidRegistry";

		public static readonly StringName GetBank = "GetBank";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checkCount = "_checkCount";

		public static readonly StringName _failureCount = "_failureCount";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "FULL_GAMEPLAY_PACKET_BANK_VALIDATION_RESULT";

	private const string ValidRegistrySource = "{\n\t\"GeneralPlant\": { \"Include\": [], \"Category\": { \"White\": [\"PlantPacket\"] } },\n\t\"PlantPresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"PlantPacket\"] } },\n\t\"PresentBoxNoAshPlant\": { \"Include\": [], \"Category\": { \"White\": [\"PlantPacket\"] } },\n\t\"ZombiePresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } },\n\t\"EliteZombie\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } },\n\t\"ZombieImpPresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } },\n\t\"ZombieGargantuarPresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } }\n}";

	private int _checkCount;

	private int _failureCount;

	public override void _Ready()
	{
		try
		{
			RunFixtures();
		}
		catch (Exception value)
		{
			_failureCount++;
			GD.PushError($"[FullGameplayPacketBankValidationRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failureCount == 0;
		GD.Print($"{"FULL_GAMEPLAY_PACKET_BANK_VALIDATION_RESULT"} passed={flag} checks={_checkCount} failures={_failureCount}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void RunFixtures()
	{
		FullGameplayRegistryRoots roots = CreateRegistryRoots();
		CheckNoFailures(FullGameplayResourceManifest.ValidatePacketBankClosure(CreateValidRegistry(), roots), "valid registry");
		Json json = CreateValidRegistry();
		json.Data.AsGodotDictionary()["BrokenBank"] = new Godot.Collections.Array();
		CheckContains(FullGameplayResourceManifest.ValidatePacketBankClosure(json, roots), "PacketBank entry is not an object: BrokenBank", "non-object bank");
		Json json2 = CreateValidRegistry();
		GetBank(json2, "GeneralPlant")["Category"] = new Godot.Collections.Array();
		CheckContains(FullGameplayResourceManifest.ValidatePacketBankClosure(json2, roots), "PacketBank Category is missing or is not an object: GeneralPlant", "wrong Category type");
		Json json3 = CreateValidRegistry();
		GetBank(json3, "GeneralPlant")["Include"] = new Dictionary();
		CheckContains(FullGameplayResourceManifest.ValidatePacketBankClosure(json3, roots), "PacketBank Include is missing or is not an array: GeneralPlant", "wrong Include type");
		Json json4 = CreateValidRegistry();
		GetBank(json4, "PlantPresentBox")["Category"] = new Dictionary { ["White"] = new Godot.Collections.Array() };
		List<string> failures = FullGameplayResourceManifest.ValidatePacketBankClosure(json4, roots);
		CheckContains(failures, "required PacketBank expands to an empty candidate pool: PlantPresentBox", "empty required pool");
		CheckContains(failures, "plant PresentBox PacketBank has no registered Plant candidate: PlantPresentBox", "empty plant PresentBox pool");
		Json json5 = CreateValidRegistry();
		GetBank(json5, "PlantPresentBox")["Category"] = new Dictionary { ["White"] = new Godot.Collections.Array { "ZombiePacket" } };
		CheckContains(FullGameplayResourceManifest.ValidatePacketBankClosure(json5, roots), "plant PresentBox PacketBank has no registered Plant candidate: PlantPresentBox", "wrong plant PresentBox candidate type");
		Json json6 = CreateValidRegistry();
		GetBank(json6, "ZombiePresentBox")["Category"] = new Dictionary { ["White"] = new Godot.Collections.Array { "PlantPacket" } };
		CheckContains(FullGameplayResourceManifest.ValidatePacketBankClosure(json6, roots), "zombie PresentBox PacketBank has no registered Zombie candidate: ZombiePresentBox", "wrong zombie PresentBox candidate type");
		Json json7 = CreateValidRegistry();
		GetBank(json7, "PlantPresentBox")["Include"] = new Godot.Collections.Array { "PresentBoxNoAshPlant" };
		GetBank(json7, "PresentBoxNoAshPlant")["Include"] = new Godot.Collections.Array { "PlantPresentBox" };
		CheckContains(FullGameplayResourceManifest.ValidatePacketBankClosure(json7, roots), "PacketBank Include cycle detected:", "Include cycle");
	}

	private static FullGameplayRegistryRoots CreateRegistryRoots()
	{
		System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["PlantFixture"] = "res://Test/Fixture/PlantFixture.tscn",
			["ZombieFixture"] = "res://Test/Fixture/ZombieFixture.tscn"
		};
		System.Collections.Generic.Dictionary<string, string> dictionary2 = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["PlantFixture"] = "res://Test/Fixture/PlantFixtureSprite.tscn",
			["ZombieFixture"] = "res://Test/Fixture/ZombieFixtureSprite.tscn"
		};
		System.Collections.Generic.Dictionary<string, string> dictionary3 = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["PlantPacket"] = "res://Test/Fixture/PlantPacket.tres",
			["ZombiePacket"] = "res://Test/Fixture/ZombiePacket.tres"
		};
		System.Collections.Generic.Dictionary<string, string> characterNameByPacket = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			["PlantPacket"] = "PlantFixture",
			["ZombiePacket"] = "ZombieFixture"
		};
		return new FullGameplayRegistryRoots(dictionary, dictionary2, dictionary3, characterNameByPacket, new List<string>(dictionary.Values), new List<string>(dictionary2.Values), new List<string>(dictionary3.Values), dictionary.Count);
	}

	private static Json CreateValidRegistry()
	{
		Json json = new Json();
		Error error = json.Parse("{\n\t\"GeneralPlant\": { \"Include\": [], \"Category\": { \"White\": [\"PlantPacket\"] } },\n\t\"PlantPresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"PlantPacket\"] } },\n\t\"PresentBoxNoAshPlant\": { \"Include\": [], \"Category\": { \"White\": [\"PlantPacket\"] } },\n\t\"ZombiePresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } },\n\t\"EliteZombie\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } },\n\t\"ZombieImpPresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } },\n\t\"ZombieGargantuarPresentBox\": { \"Include\": [], \"Category\": { \"White\": [\"ZombiePacket\"] } }\n}");
		if (error != Error.Ok)
		{
			throw new InvalidOperationException($"Unable to parse valid PacketBank fixture: {error}");
		}
		return json;
	}

	private static Dictionary GetBank(Json registry, string bankName)
	{
		return registry.Data.AsGodotDictionary()[bankName].AsGodotDictionary();
	}

	private void CheckNoFailures(IReadOnlyCollection<string> failures, string context)
	{
		_checkCount++;
		if (failures.Count != 0)
		{
			_failureCount++;
			GD.PushError("[FullGameplayPacketBankValidationRuntimeTest] " + context + " unexpectedly failed: " + string.Join(" | ", failures));
		}
	}

	private void CheckContains(IEnumerable<string> failures, string expectedText, string context)
	{
		_checkCount++;
		foreach (string failure in failures)
		{
			if (failure.Contains(expectedText, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}
		_failureCount++;
		GD.PushError("[FullGameplayPacketBankValidationRuntimeTest] " + context + " did not report: " + expectedText);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateValidRegistry, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetBank, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "registry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false),
				new PropertyInfo(Variant.Type.String, "bankName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RunFixtures && args.Count == 0)
		{
			RunFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateValidRegistry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Json>(CreateValidRegistry());
			return true;
		}
		if (method == MethodName.GetBank && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetBank(VariantUtils.ConvertTo<Json>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateValidRegistry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Json>(CreateValidRegistry());
			return true;
		}
		if (method == MethodName.GetBank && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetBank(VariantUtils.ConvertTo<Json>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.RunFixtures)
		{
			return true;
		}
		if (method == MethodName.CreateValidRegistry)
		{
			return true;
		}
		if (method == MethodName.GetBank)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checkCount)
		{
			_checkCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failureCount)
		{
			_failureCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checkCount)
		{
			value = VariantUtils.CreateFrom(in _checkCount);
			return true;
		}
		if (name == PropertyName._failureCount)
		{
			value = VariantUtils.CreateFrom(in _failureCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checkCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failureCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checkCount, Variant.From(in _checkCount));
		info.AddProperty(PropertyName._failureCount, Variant.From(in _failureCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checkCount, out var value))
		{
			_checkCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failureCount, out var value2))
		{
			_failureCount = value2.As<int>();
		}
	}
}
