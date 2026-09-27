using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/TrioAmbushInspection.cs")]
public class TrioAmbushInspection : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName _mcp_state = "_mcp_state";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName MapName = "MapName";
	}

	public new class SignalName : Node.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string MapName = "Backyard";

	public override async void _Ready()
	{
		await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
		GameSaveManager.Instance.EnsureLoaded();
		GameSaveManager.Instance.EnsureUser();
		Global.Instance.isMultiplayerMode = false;
		Global.Instance.enterLevelMode = "LevelTest";
		Dictionary dictionary = Json.ParseString(FileAccess.GetFileAsString("res://Asset/Config/Level/TowerDefense/Chapter3/Level3_1.json")).AsGodotDictionary();
		dictionary["Name"] = "";
		dictionary["LevelName"] = "三人组出场验收";
		dictionary["Map"] = MapName;
		dictionary["Talk"] = "";
		dictionary["MowerUse"] = false;
		dictionary["Reward"] = new Dictionary();
		dictionary["PacketBank"] = new Dictionary
		{
			["Method"] = "Preset",
			["ColdDownStart"] = false,
			["ColdDownUse"] = false,
			["Value"] = new Array { "PlantIceShroom", "PlantCoffeebean", "PlantUmbrellaleaf", "PlantLilyPad", "PlantPot", "PlantCherryBomb" }
		};
		dictionary["SunManager"] = new Dictionary
		{
			["Open"] = true,
			["Begin"] = 9990
		};
		Array array = new Array();
		for (int i = 0; i < 3; i++)
		{
			array.Add(new Dictionary
			{
				["Spawn"] = new Array(),
				["Event"] = new Array
				{
					new Dictionary { ["EventName"] = ((MapName.StartsWith("Roof") || i % 2 == 1) ? "BungiTrioSpawn" : "CoralTrioSpawn") }
				}
			});
		}
		dictionary["WaveManager"] = new Dictionary
		{
			["BeginCol"] = 5.0,
			["SpawnColStart"] = 12.0,
			["SpawnColEnd"] = 20.0,
			["FlagWaveInterval"] = 99,
			["Dynamic"] = new Array(),
			["Wave"] = array
		};
		TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig
		{
			data = new Json
			{
				Data = dictionary
			}
		};
		towerDefenseLevelConfig.Init();
		TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelConfig;
		Node node = GD.Load<PackedScene>("res://Scene/TowerDefesne/TowerDefenseNew/TowerDefenseControlNew.tscn").Instantiate(PackedScene.GenEditState.Disabled);
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	public Dictionary _mcp_state()
	{
		TowerDefenseControlNew nodeOrNull = GetNodeOrNull<TowerDefenseControlNew>("TowerDefenseControlNew");
		TowerDefenseBattleFeatureWave towerDefenseBattleFeatureWave = nodeOrNull?.GetFeature("Wave") as TowerDefenseBattleFeatureWave;
		Dictionary dictionary = new Dictionary
		{
			["running"] = nodeOrNull?.isGameRunning ?? false,
			["ready"] = towerDefenseBattleFeatureWave?.readySetPlantOver ?? false,
			["wave"] = towerDefenseBattleFeatureWave?.SaveFeature() ?? new Dictionary()
		};
		Array array = new Array();
		if (GodotObject.IsInstanceValid(nodeOrNull?.characterNode))
		{
			foreach (Node child in nodeOrNull.characterNode.GetChildren())
			{
				if (child is TowerDefenseZombie towerDefenseZombie)
				{
					array.Add(new Dictionary
					{
						["name"] = towerDefenseZombie.Name,
						["packet"] = towerDefenseZombie.packet?.saveKey ?? "carrier",
						["grid"] = towerDefenseZombie.gridPos,
						["z"] = towerDefenseZombie.z,
						["ground"] = towerDefenseZombie.isGround,
						["frozen"] = towerDefenseZombie.buff?.buffDictionary.ContainsKey("Frozen") ?? false,
						["trio"] = TrioAmbushMember.ExportSpawnState(towerDefenseZombie)
					});
				}
			}
		}
		dictionary["characters"] = array;
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._mcp_state, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._mcp_state && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(_mcp_state());
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
		if (method == MethodName._mcp_state)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.MapName)
		{
			MapName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.MapName)
		{
			value = VariantUtils.CreateFrom(in MapName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.MapName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.MapName, Variant.From(in MapName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.MapName, out var value))
		{
			MapName = value.As<string>();
		}
	}
}
