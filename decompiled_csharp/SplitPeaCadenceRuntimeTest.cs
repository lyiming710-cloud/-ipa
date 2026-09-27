using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/SplitPeaCadenceRuntimeTest.cs")]
public class SplitPeaCadenceRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

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

	private const string DoubleGatePath = "res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_14.tres";

	private const string SplitPeaCatScenePath = "res://Asset/Anime/Character/Plant/Other/SplitPeaCat/Scene/TowerDefensePlantSplitPeaCat.tscn";

	private const string CatPumpkinScenePath = "res://Asset/Anime/Character/Plant/Chapter4/CatPumpkin/Scene/TowerDefensePlantCatPumpkin.tscn";

	private const string SplitPeaScenePath = "res://Asset/Anime/Character/Plant/Chapter0/SplitPea/Scene/TowerDefensePlantSplitPea.tscn";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		_ = 1;
		try
		{
			await VerifyDoubleGateCatPumpkinCadence();
			await VerifyOriginalSplitPeaCadenceIsIntentional();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[SplitPeaCadenceRuntimeTest] Unexpected exception: {value}");
		}
		bool flag = _failures == 0;
		GD.Print($"SPLIT_PEA_CADENCE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyDoubleGateCatPumpkinCadence()
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_14.tres", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && towerDefenseLevelConfig.levelName == "双鬼拍门", "The real Double Gate level resource must load.");
		TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = FindPacket(towerDefenseLevelConfig, "PlantSplitPeaCat", (TowerDefenseLevelPacketConfig packet) => HasPropertyValue(packet.@override?.characterOverride, "fireInterval", (Variant value) => Math.Abs(value.AsDouble() - 0.05) < 0.0001) && HasPropertyValue(packet.@override?.characterOverride, "fireNum", (Variant value) => value.AsInt32() == 5) && HasPropertyValue(packet.@override?.characterOverride, "projectileName", (Variant value) => value.AsString() == "IceFirePeaTrack"));
		Check(GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig), "Double Gate's real upgrade chain must contain PlantSplitPeaCat.");
		if (!GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig))
		{
			return;
		}
		TowerDefenseCharacterOverride characterOverride = towerDefenseLevelPacketConfig.@override?.characterOverride;
		Check(GodotObject.IsInstanceValid(characterOverride), "Double Gate PlantSplitPeaCat must carry a character override.");
		Check(HasPropertyValue(characterOverride, "fireInterval", (Variant value) => Math.Abs(value.AsDouble() - 0.05) < 0.0001), "Double Gate PlantSplitPeaCat must use fireInterval=0.05.");
		Check(HasPropertyValue(characterOverride, "fireNum", (Variant value) => value.AsInt32() == 5), "Double Gate PlantSplitPeaCat must use fireNum=5.");
		Check(HasPropertyValue(characterOverride, "projectileName", (Variant value) => value.AsString() == "IceFirePeaTrack"), "Double Gate PlantSplitPeaCat must use the real IceFirePeaTrack projectile override.");
		TowerDefensePlantSplitPeaCat plant = Instantiate<TowerDefensePlantSplitPeaCat>("res://Asset/Anime/Character/Plant/Other/SplitPeaCat/Scene/TowerDefensePlantSplitPeaCat.tscn");
		TowerDefensePlantCatPumpkin catPumpkin = Instantiate<TowerDefensePlantCatPumpkin>("res://Asset/Anime/Character/Plant/Chapter4/CatPumpkin/Scene/TowerDefensePlantCatPumpkin.tscn");
		Check(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(catPumpkin), "The real SplitPeaCat and CatPumpkin scenes must instantiate.");
		if (!GodotObject.IsInstanceValid(plant) || !GodotObject.IsInstanceValid(catPumpkin))
		{
			return;
		}
		plant.inGame = false;
		plant.editorPreviewMode = true;
		catPumpkin.inGame = false;
		catPumpkin.editorPreviewMode = true;
		AddChild(plant, forceReadableName: false, InternalMode.Disabled);
		AddChild(catPumpkin, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		FireComponent fire = plant.componentManager?.GetRuntime<FireComponent>("character.fire");
		Check(fire != null && !fire.IsReleased, "SplitPeaCat must expose its real FireComponent runtime.");
		if (fire != null && !fire.IsReleased)
		{
			characterOverride.ExecuteCharacter(plant);
			Check(Math.Abs(plant.fireInterval - 0.05) < 0.0001 && fire.fireNum == 5, "The real Double Gate override must reach the live SplitPeaCat FireComponent.");
			TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance();
			towerDefenseCellInstance.characterList.Add(plant);
			towerDefenseCellInstance.characterList.Add(catPumpkin);
			plant.cell = towerDefenseCellInstance;
			catPumpkin.cell = towerDefenseCellInstance;
			Check((plant.instance.physiqueTypeFlags & 0x100) != 0, "SplitPeaCat must retain the CAT physique flag consumed by CatPumpkin acceleration.");
			Check(towerDefenseCellInstance.HasCharacter("PlantCatPumpkin"), "The same real cell must expose PlantCatPumpkin to FireComponent.");
			while (Engine.GetPhysicsFrames() < 30)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			}
			fire.hasCatPumpkin = false;
			fire._catPumpkinCheckFrame = 0;
			fire.PhysicsProcess(0.0, Engine.GetPhysicsFrames());
			Check(fire.hasCatPumpkin, "The live FireComponent must detect the real CatPumpkin in SplitPeaCat's cell.");
			fire.hasCatPumpkin = false;
			fire.AttackProcessing(0.0);
			double timeScale = fire.sprite.timeScale;
			fire.hasCatPumpkin = true;
			fire.AttackProcessing(0.0);
			double timeScale2 = fire.sprite.timeScale;
			Check(timeScale > 0.0, "The Double Gate five-shot attack animation must have a positive baseline cadence.");
			Check(Math.Abs(timeScale2 - timeScale * 2.0) < 0.001, $"CatPumpkin must double the five-shot attack animation cadence; normal={timeScale:F4}, boosted={timeScale2:F4}.");
			fire.timer = 1f;
			fire.hasCatPumpkin = true;
			fire.PhysicsProcess(0.1, Engine.GetPhysicsFrames());
			Check((double)Math.Abs(fire.timer - 0.8f) < 0.001, $"CatPumpkin must continue to double cooldown countdown; timer={fire.timer:F4}.");
			plant.QueueFree();
			catPumpkin.QueueFree();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task VerifyOriginalSplitPeaCadenceIsIntentional()
	{
		TowerDefensePlantSplitPea plant = Instantiate<TowerDefensePlantSplitPea>("res://Asset/Anime/Character/Plant/Chapter0/SplitPea/Scene/TowerDefensePlantSplitPea.tscn");
		Check(GodotObject.IsInstanceValid(plant), "The real original SplitPea scene must instantiate.");
		if (GodotObject.IsInstanceValid(plant))
		{
			plant.inGame = false;
			plant.editorPreviewMode = true;
			AddChild(plant, forceReadableName: false, InternalMode.Disabled);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			FireComponent fireComponent = plant.componentManager?.GetRuntime<FireComponent>("character.fire");
			Check(fireComponent != null && !fireComponent.IsReleased, "Original SplitPea must expose its real FireComponent runtime.");
			if (fireComponent != null && !fireComponent.IsReleased)
			{
				plant.projectileName = "FirePea";
				Check(fireComponent.fireCheckList.Count == 1 && fireComponent.fireCheckList[0].projectile.GetProjectile().projectileName.ToString() == "FirePea", "Editing original SplitPea's projectile must update the one shared projectile source used by both heads.");
				Check(fireComponent.fireProjectileList.Count == 2 && fireComponent.fireProjectileList[0].checkProjectileId == 0 && fireComponent.fireProjectileList[1].checkProjectileId == 0, "Both original SplitPea heads must continue to consume the edited shared projectile source.");
				FireComponentFireProjectileConfig fireComponentFireProjectileConfig = fireComponent.fireProjectileList[0];
				FireComponentFireProjectileConfig fireComponentFireProjectileConfig2 = fireComponent.fireProjectileList[1];
				Check(fireComponentFireProjectileConfig.firePosId == 0 && fireComponentFireProjectileConfig.fireNumSkip == 1 && fireComponentFireProjectileConfig.speed > 0f, "The forward head intentionally emits once per two-shot cycle.");
				Check(fireComponentFireProjectileConfig2.firePosId == 1 && fireComponentFireProjectileConfig2.fireNumSkip == -1 && fireComponentFireProjectileConfig2.speed < 0f, "The rear head intentionally emits on both shots of the cycle.");
				Check(fireComponent.fireNum == 2, "Original SplitPea must retain its two-shot rear-head cycle.");
				Vector2I clip = fireComponent.sprite.flashAnimeData.GetClip("LeftFire");
				Vector2I clip2 = fireComponent.sprite.flashAnimeData.GetClip("RightFire");
				int num = clip.Y - clip.X;
				int num2 = clip2.Y - clip2.X;
				Check(num2 == num * 2, $"The one 24-frame forward animation must stay synchronized with two 12-frame rear animations; left={num}, right={num2}.");
				plant.QueueFree();
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
		}
	}

	private static T Instantiate<T>(string path) where T : Node
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			return null;
		}
		return packedScene.Instantiate<T>(PackedScene.GenEditState.Disabled);
	}

	private static TowerDefenseLevelPacketConfig FindPacket(TowerDefenseLevelConfig level, string packetName, Func<TowerDefenseLevelPacketConfig, bool> predicate)
	{
		if (!GodotObject.IsInstanceValid(level))
		{
			return null;
		}
		HashSet<ulong> visited = new HashSet<ulong>();
		foreach (Variant packetBank in level.packetBankList)
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = FindPacket(packetBank.AsGodotObject() as TowerDefenseLevelPacketConfig, packetName, predicate, visited);
			if (GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig))
			{
				return towerDefenseLevelPacketConfig;
			}
		}
		return null;
	}

	private static TowerDefenseLevelPacketConfig FindPacket(TowerDefenseLevelPacketConfig packet, string packetName, Func<TowerDefenseLevelPacketConfig, bool> predicate, HashSet<ulong> visited)
	{
		if (!GodotObject.IsInstanceValid(packet) || !visited.Add(packet.GetInstanceId()))
		{
			return null;
		}
		if (packet.packetName == packetName && predicate(packet))
		{
			return packet;
		}
		if (!GodotObject.IsInstanceValid(packet.@override))
		{
			return null;
		}
		foreach (CardActionBehaviorDefinition useSucceededAction in packet.@override.useSucceededActions)
		{
			if (useSucceededAction is CardActionBehaviorChangePacket cardActionBehaviorChangePacket)
			{
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = FindPacket(cardActionBehaviorChangePacket.levelPacketConfig, packetName, predicate, visited);
				if (GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig))
				{
					return towerDefenseLevelPacketConfig;
				}
			}
		}
		return null;
	}

	private static bool HasPropertyValue(TowerDefenseCharacterOverride characterOverride, string propertyName, Func<Variant, bool> predicate)
	{
		if (!GodotObject.IsInstanceValid(characterOverride))
		{
			return false;
		}
		foreach (TowerDefenseCharacterPropertyChangeConfig item in characterOverride.propertyChange)
		{
			if (GodotObject.IsInstanceValid(item) && item.propertyName == propertyName)
			{
				return predicate(item.value);
			}
		}
		return false;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[SplitPeaCadenceRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
