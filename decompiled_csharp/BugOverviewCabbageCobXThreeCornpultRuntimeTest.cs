using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewCabbageCobXThreeCornpultRuntimeTest.cs")]
public class BugOverviewCabbageCobXThreeCornpultRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName CreateControlWithMap = "CreateControlWithMap";

		public static readonly StringName CreateOccupiedCell = "CreateOccupiedCell";

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

	private const string CabbageCobXPath = "res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Packet/PlantCabbageCobX.tres";

	private const string ThreeCornpultPath = "res://Asset/Anime/Character/Plant/Other/ThreeCornpult/Packet/PlantThreeCornpult.tres";

	private const string CabbageCobPath = "res://Asset/Anime/Character/Plant/Chapter5/CabbageCob/Packet/PlantCabbageCob.tres";

	private const string CabbagepultPath = "res://Asset/Anime/Character/Plant/Chapter0/Cabbagepult/Packet/PlantCabbagepult.tres";

	private const string CabbagepultInsidePath = "res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Packet/PlantCabbagepultInside.tres";

	private static readonly Vector2I TestGridPosition = new Vector2I(2, 1);

	private int _checks;

	private int _failures;

	private readonly List<TowerDefensePlant> _occupants = new List<TowerDefensePlant>();

	public override void _Ready()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		TowerDefenseControlNew currentControl = instance?.currentControl;
		TowerDefenseControlNew towerDefenseControlNew = null;
		try
		{
			Check(GodotObject.IsInstanceValid(instance), "TowerDefenseManager autoload must be available.");
			if (!GodotObject.IsInstanceValid(instance))
			{
				return;
			}
			TowerDefensePacketConfig towerDefensePacketConfig = LoadPacket("res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Packet/PlantCabbageCobX.tres");
			TowerDefensePacketConfig towerDefensePacketConfig2 = LoadPacket("res://Asset/Anime/Character/Plant/Other/ThreeCornpult/Packet/PlantThreeCornpult.tres");
			TowerDefensePacketConfig towerDefensePacketConfig3 = LoadPacket("res://Asset/Anime/Character/Plant/Chapter5/CabbageCob/Packet/PlantCabbageCob.tres");
			TowerDefensePacketConfig towerDefensePacketConfig4 = LoadPacket("res://Asset/Anime/Character/Plant/Chapter0/Cabbagepult/Packet/PlantCabbagepult.tres");
			TowerDefensePacketConfig towerDefensePacketConfig5 = LoadPacket("res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Packet/PlantCabbagepultInside.tres");
			Check(GodotObject.IsInstanceValid(towerDefensePacketConfig), "The real CabbageCobX packet must load.");
			Check(GodotObject.IsInstanceValid(towerDefensePacketConfig2), "The real ThreeCornpult packet must load.");
			Check(GodotObject.IsInstanceValid(towerDefensePacketConfig3) && GodotObject.IsInstanceValid(towerDefensePacketConfig4) && GodotObject.IsInstanceValid(towerDefensePacketConfig5), "All three documented CabbageCobX base packets must load.");
			if (!GodotObject.IsInstanceValid(towerDefensePacketConfig) || !GodotObject.IsInstanceValid(towerDefensePacketConfig2) || !GodotObject.IsInstanceValid(towerDefensePacketConfig3) || !GodotObject.IsInstanceValid(towerDefensePacketConfig4) || !GodotObject.IsInstanceValid(towerDefensePacketConfig5))
			{
				return;
			}
			Array<string> plantCover = towerDefensePacketConfig.GetPlantCover();
			Check(plantCover.Count == 3, $"CabbageCobX must have exactly three upgrade bases; got {plantCover.Count}.");
			Check(plantCover.Contains("PlantCabbageCob"), "CabbageCobX must remain plantable on CabbageCob.");
			Check(plantCover.Contains("PlantCabbagepult"), "CabbageCobX must remain plantable on Cabbagepult.");
			Check(plantCover.Contains("PlantCabbagepultInside"), "CabbageCobX must remain plantable on CabbagepultInside.");
			Check(!plantCover.Contains("PlantThreeCornpult"), "CabbageCobX must not list ThreeCornpult as an upgrade base.");
			Check(towerDefensePacketConfig.characterConfig.plantCoverRecycle.Count == plantCover.Count, "CabbageCobX cover refunds must stay aligned with its three allowed bases.");
			Check(towerDefensePacketConfig.characterConfig.plantCoverRecycle.Count == 3 && towerDefensePacketConfig.characterConfig.plantCoverRecycle[0] == 200 && towerDefensePacketConfig.characterConfig.plantCoverRecycle[1] == 0 && towerDefensePacketConfig.characterConfig.plantCoverRecycle[2] == 0, "CabbageCobX refunds must remain [200, 0, 0] for the documented bases.");
			Check(!towerDefensePacketConfig.GetCoverCanDirectPlant(), "CabbageCobX must still require an allowed base plant.");
			towerDefenseControlNew = (instance.currentControl = CreateControlWithMap());
			Check(!CreateOccupiedCell(towerDefensePacketConfig2.characterConfig).CanPacketPlant(towerDefensePacketConfig), "The real cell placement path must reject CabbageCobX on ThreeCornpult.");
			Check(CreateOccupiedCell(towerDefensePacketConfig3.characterConfig).CanPacketPlant(towerDefensePacketConfig), "The real cell placement path must accept CabbageCobX on CabbageCob.");
			Check(CreateOccupiedCell(towerDefensePacketConfig4.characterConfig).CanPacketPlant(towerDefensePacketConfig), "The real cell placement path must accept CabbageCobX on Cabbagepult.");
			Check(CreateOccupiedCell(towerDefensePacketConfig5.characterConfig).CanPacketPlant(towerDefensePacketConfig), "The real cell placement path must accept CabbageCobX on CabbagepultInside.");
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[BugOverviewCabbageCobXThreeCornpultRuntimeTest] Unexpected exception: {value}");
		}
		finally
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.currentControl = currentControl;
			}
			foreach (TowerDefensePlant occupant in _occupants)
			{
				if (GodotObject.IsInstanceValid(occupant))
				{
					occupant.Free();
				}
			}
			if (GodotObject.IsInstanceValid(towerDefenseControlNew))
			{
				towerDefenseControlNew.featureDictionary.Clear();
				towerDefenseControlNew.Free();
			}
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_CABBAGE_COB_X_THREE_CORN_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private static TowerDefenseControlNew CreateControlWithMap()
	{
		TowerDefenseControlNew towerDefenseControlNew = new TowerDefenseControlNew();
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = new TowerDefenseBattleFeatureMap();
		towerDefenseBattleFeatureMap.iceCapList.Resize(3);
		towerDefenseControlNew.featureDictionary[new StringName("Map")] = towerDefenseBattleFeatureMap;
		return towerDefenseControlNew;
	}

	private TowerDefenseCellInstance CreateOccupiedCell(TowerDefenseCharacterConfig occupantConfig)
	{
		TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
		{
			gridPos = TestGridPosition,
			slot = 
			{
				[TowerDefenseEnum.PLANTGRIDTYPE.GROUND] = null,
				[TowerDefenseEnum.PLANTGRIDTYPE.AIR] = null
			}
		};
		TowerDefensePlant towerDefensePlant = new TowerDefensePlant
		{
			config = occupantConfig,
			instance = new TowerDefenseCharacterInstance
			{
				config = occupantConfig
			}
		};
		_occupants.Add(towerDefensePlant);
		towerDefenseCellInstance.characterList.Add(towerDefensePlant);
		towerDefenseCellInstance.characterSlotDictionary[towerDefensePlant] = null;
		towerDefenseCellInstance.slot[TowerDefenseEnum.PLANTGRIDTYPE.GROUND] = towerDefensePlant;
		return towerDefenseCellInstance;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewCabbageCobXThreeCornpultRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateControlWithMap, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateOccupiedCell, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "occupantConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.CreateControlWithMap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseControlNew>(CreateControlWithMap());
			return true;
		}
		if (method == MethodName.CreateOccupiedCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCellInstance>(CreateOccupiedCell(VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[0])));
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
		if (method == MethodName.CreateControlWithMap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseControlNew>(CreateControlWithMap());
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
		if (method == MethodName.CreateControlWithMap)
		{
			return true;
		}
		if (method == MethodName.CreateOccupiedCell)
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
