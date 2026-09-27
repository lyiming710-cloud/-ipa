using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/LuckyBloverIceKingCooldownRuntimeTest.cs")]
public class LuckyBloverIceKingCooldownRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadPacket = "LoadPacket";

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

	private const string IceKingPacketPath = "res://Asset/Anime/Character/Plant/Diamond/IceKing/Packet/PlantIceKing.tres";

	private const string IceKingScenePath = "res://Asset/Anime/Character/Plant/Diamond/IceKing/Scene/TowerDefensePlantIceKing.tscn";

	private const string PeaShooterPacketPath = "res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Packet/PlantPeaShooter.tres";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew control = null;
		TowerDefenseInGameSeedBank seedBank = null;
		TowerDefensePlantIceKing iceKing = null;
		TowerDefenseInGamePacketShow transformedIceSlot = null;
		TowerDefenseInGamePacketShow ordinarySlot = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager))
				{
					goto end_IL_007c;
				}
				control = new TowerDefenseControlNew
				{
					isGameRunning = true
				};
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				seedBank = new TowerDefenseInGameSeedBank();
				TowerDefenseBattleFeatureSeedBank value = new TowerDefenseBattleFeatureSeedBank
				{
					seedBank = seedBank,
					config = new TowerDefenseLevelSeedBankConfig
					{
						method = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE
					}
				};
				control.featureDictionary[new StringName("SeedBank")] = value;
				TowerDefensePacketConfig iceConfig = LoadPacket("res://Asset/Anime/Character/Plant/Diamond/IceKing/Packet/PlantIceKing.tres");
				TowerDefensePacketConfig peaConfig = LoadPacket("res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Packet/PlantPeaShooter.tres");
				Check(GodotObject.IsInstanceValid(iceConfig) && GodotObject.IsInstanceValid(peaConfig), "Real IceKing and PeaShooter packet resources must load.");
				if (!GodotObject.IsInstanceValid(iceConfig) || !GodotObject.IsInstanceValid(peaConfig))
				{
					goto end_IL_007c;
				}
				transformedIceSlot = TowerDefenseManager.CreatePacketShow();
				ordinarySlot = TowerDefenseManager.CreatePacketShow();
				control.characterNode.AddChild(transformedIceSlot, forceReadableName: false, InternalMode.Disabled);
				control.characterNode.AddChild(ordinarySlot, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				transformedIceSlot.Init(iceConfig);
				transformedIceSlot.originalSaveKey = "PlantLuckyBlover";
				transformedIceSlot.coldDownTimer = iceConfig.GetPacketCooldown();
				ordinarySlot.Init(peaConfig);
				ordinarySlot.originalSaveKey = "PlantPeaShooter";
				ordinarySlot.coldDownTimer = 12.0;
				seedBank.packetList.Add(transformedIceSlot);
				seedBank.packetList.Add(ordinarySlot);
				iceKing = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Diamond/IceKing/Scene/TowerDefensePlantIceKing.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefensePlantIceKing>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(iceKing), "Real IceKing scene must instantiate.");
				if (!GodotObject.IsInstanceValid(iceKing))
				{
					goto end_IL_007c;
				}
				iceKing.inGame = false;
				iceKing.editorPreviewMode = true;
				iceKing.packet = iceConfig;
				control.characterNode.AddChild(iceKing, forceReadableName: false, InternalMode.Disabled);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				double coldDownTimer = transformedIceSlot.coldDownTimer;
				iceKing.inGame = true;
				iceKing.run = true;
				iceKing.over = false;
				iceKing.AnimeCompleted("Idle");
				Check(iceKing.over, "IceKing Idle completion must execute its one-shot effect.");
				Check(Math.Abs(transformedIceSlot.coldDownTimer - (coldDownTimer + 50.0)) < 0.0001, $"LuckyBlover-transformed IceKing slot must keep and extend its own cooldown; got {transformedIceSlot.coldDownTimer}.");
				Check(Math.Abs(ordinarySlot.coldDownTimer) < 0.0001, "IceKing must still clear a different card's cooldown.");
				Check(Math.Abs(iceConfig.overridePacketCooldown - (iceConfig.characterConfig.packetCooldown + 50.0)) < 0.0001, "The transformed IceKing card must retain its cumulative +50 second cooldown rule.");
				goto end_IL_0065;
				end_IL_007c:;
			}
			catch (Exception value2)
			{
				_failures++;
				GD.PushError($"[LuckyBloverIceKingCooldownRuntimeTest] Unexpected exception: {value2}");
				goto end_IL_0065;
			}
			return;
			end_IL_0065:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = null;
			}
			if (GodotObject.IsInstanceValid(iceKing) && !iceKing.IsQueuedForDeletion())
			{
				iceKing.QueueFree();
			}
			if (GodotObject.IsInstanceValid(transformedIceSlot))
			{
				transformedIceSlot.QueueFree();
			}
			if (GodotObject.IsInstanceValid(ordinarySlot))
			{
				ordinarySlot.QueueFree();
			}
			if (GodotObject.IsInstanceValid(seedBank))
			{
				seedBank.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.Free();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0;
		GD.Print($"LUCKY_BLOVER_ICE_KING_COOLDOWN_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[LuckyBloverIceKingCooldownRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadPacket)
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
