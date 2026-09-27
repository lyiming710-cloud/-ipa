using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/PacketBankColdPreloadRuntimeTest.cs")]
public class PacketBankColdPreloadRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ValidateFullRegistryMemoryHits = "ValidateFullRegistryMemoryHits";

		public static readonly StringName Check = "Check";

		public static readonly StringName Fail = "Fail";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	public override async void _Ready()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(resourceManager))
		{
			Fail("ResourceManager is unavailable");
			Finish();
			return;
		}
		resourceManager.BeginLoad();
		for (int frame = 0; frame < 7200; frame++)
		{
			if (resourceManager.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Ready)
			{
				break;
			}
			if (resourceManager.CurrentGameplayResourceLoadState == GameplayResourceLoadState.Failed)
			{
				break;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		if (!resourceManager.AreFullGameplayResourcesReady)
		{
			Fail($"ResourceManager full readiness failed: state={resourceManager.CurrentGameplayResourceLoadState}, error={resourceManager.FullGameplayResourceLoadError}");
			Finish();
		}
		else
		{
			ValidateFullRegistryMemoryHits(resourceManager);
			Finish();
		}
	}

	private void ValidateFullRegistryMemoryHits(ResourceManager resourceManager)
	{
		int lateCharacterResourceLoadCount = resourceManager.LateCharacterResourceLoadCount;
		foreach (string key in resourceManager.TOWERDEFENSE_CHARCATERS.Keys)
		{
			Check(GodotObject.IsInstanceValid(resourceManager.GetCharacterScene(key)), "Character Scene is invalid: " + key);
		}
		foreach (string key2 in resourceManager.CHARCTAER_SPRITE.Keys)
		{
			Check(GodotObject.IsInstanceValid(resourceManager.GetCharacterSprite(key2)), "Character Sprite is invalid: " + key2);
		}
		foreach (string packetName in resourceManager.GetPacketNames())
		{
			Variant packet = resourceManager.GetPacket(packetName);
			Check(packet.VariantType == Variant.Type.Object && GodotObject.IsInstanceValid(packet.AsGodotObject()), "Packet config is invalid: " + packetName);
		}
		foreach (string requiredRuntimePacketBank in FullGameplayResourceManifest.RequiredRuntimePacketBanks)
		{
			TowerDefensePacketBankData packetBankData = TowerDefenseManager.GetPacketBankData(requiredRuntimePacketBank);
			Check(GodotObject.IsInstanceValid(packetBankData), "Required PacketBank is invalid: " + requiredRuntimePacketBank);
			if (!GodotObject.IsInstanceValid(packetBankData))
			{
				continue;
			}
			foreach (Variant packet2 in packetBankData.GetPacketList())
			{
				string text = packet2.AsString();
				Check(GodotObject.IsInstanceValid(TowerDefenseManager.GetPacketConfigReadOnly(text)), "PacketBank candidate is not resident: bank=" + requiredRuntimePacketBank + ", packet=" + text);
			}
		}
		FullGameplayResourceLoadMetricsSnapshot fullGameplayResourceLoadMetrics = resourceManager.FullGameplayResourceLoadMetrics;
		Check(resourceManager.LateCharacterResourceLoadCount == lateCharacterResourceLoadCount, $"lateCharacterResourceLoadCount changed from {lateCharacterResourceLoadCount} to {resourceManager.LateCharacterResourceLoadCount}");
		Check(fullGameplayResourceLoadMetrics.FailureCount == 0, $"full resource failure count was {fullGameplayResourceLoadMetrics.FailureCount}");
		Check(fullGameplayResourceLoadMetrics.PacketBankMissingCount == 0, $"PacketBank missing count was {fullGameplayResourceLoadMetrics.PacketBankMissingCount}");
		Check(fullGameplayResourceLoadMetrics.FullWallMilliseconds <= 15000.0, $"process-cold full readiness took {fullGameplayResourceLoadMetrics.FullWallMilliseconds:F3} ms, expected at most 15000 ms");
		foreach (FullGameplayRootLoadMetric rootMetric in fullGameplayResourceLoadMetrics.RootMetrics)
		{
			Check(rootMetric.QueueWaitMilliseconds == 0.0, $"root queue wait was not zero: category={rootMetric.Category}, path={rootMetric.Path}, queueWaitMs={rootMetric.QueueWaitMilliseconds:F3}");
		}
		GD.Print($"FULL_GAMEPLAY_RESOURCE_READY_RESULT passed={_failures.Count == 0} characters={resourceManager.TOWERDEFENSE_CHARCATERS.Count} sprites={resourceManager.CHARCTAER_SPRITE.Count} packets={resourceManager.TOWERDEFENSE_PACKETS.Count} sceneRoots={fullGameplayResourceLoadMetrics.CategoryMetrics.GetValueOrDefault("CharacterScene")?.RootCount ?? 0} independentSpriteRoots={fullGameplayResourceLoadMetrics.CategoryMetrics.GetValueOrDefault("IndependentSprite")?.RootCount ?? 0} packetRoots={fullGameplayResourceLoadMetrics.CategoryMetrics.GetValueOrDefault("Packet")?.RootCount ?? 0} gameplayMs={fullGameplayResourceLoadMetrics.CharacterSceneMilliseconds + fullGameplayResourceLoadMetrics.SpriteMilliseconds + fullGameplayResourceLoadMetrics.PacketMilliseconds:F3} fullWallMs={fullGameplayResourceLoadMetrics.FullWallMilliseconds:F3} lateCharacterResourceLoadCount={resourceManager.LateCharacterResourceLoadCount} failures={_failures.Count}");
	}

	private void Check(bool condition, string message)
	{
		if (!condition)
		{
			Fail(message);
		}
	}

	private void Fail(string message)
	{
		_failures.Add(message);
		GD.PrintErr("FULL_GAMEPLAY_RESOURCE_READY_FAILURE " + message);
	}

	private void Finish()
	{
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ValidateFullRegistryMemoryHits, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resourceManager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Fail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ValidateFullRegistryMemoryHits && args.Count == 1)
		{
			ValidateFullRegistryMemoryHits(VariantUtils.ConvertTo<ResourceManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Fail && args.Count == 1)
		{
			Fail(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
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
		if (method == MethodName.ValidateFullRegistryMemoryHits)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Fail)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
