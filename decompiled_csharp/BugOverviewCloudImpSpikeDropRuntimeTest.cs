using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewCloudImpSpikeDropRuntimeTest.cs")]
public class BugOverviewCloudImpSpikeDropRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RegisterSpikeImpFixtures = "RegisterSpikeImpFixtures";

		public static readonly StringName RestoreSpikeImpFixtures = "RestoreSpikeImpFixtures";

		public static readonly StringName CountSpikeImps = "CountSpikeImps";

		public static readonly StringName CountSpikeBallVisuals = "CountSpikeBallVisuals";

		public static readonly StringName FindNewestSpikeImp = "FindNewestSpikeImp";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _previousPacket = "_previousPacket";

		public static readonly StringName _previousCharacter = "_previousCharacter";

		public static readonly StringName _hadPacket = "_hadPacket";

		public static readonly StringName _hadCharacter = "_hadCharacter";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string CloudScenePath = "res://Asset/Anime/Character/Zombie/Chapter8/ImpCloud/Scene/TowerDefenseZombieImpCloud.tscn";

	private const string SpikePacketPath = "res://Asset/Anime/Character/Zombie/Chapter8/ImpDiggerSpike/Packet/ZombieImpDiggerSpike.tres";

	private const string SpikeScenePath = "res://Asset/Anime/Character/Zombie/Chapter8/ImpDiggerSpike/Scene/TowerDefenseZombieImpDiggerSpike.tscn";

	private static readonly Vector2I CloudGrid = new Vector2I(5, 2);

	private int _checks;

	private int _failures;

	private Resource _previousPacket;

	private Resource _previousCharacter;

	private bool _hadPacket;

	private bool _hadCharacter;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		CloudImpSpikeDropRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefenseZombieImpCloud cloud = null;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_00e1;
				}
				RegisterSpikeImpFixtures();
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance.TOWERDEFENSE_PACKETS["ZombieImpDiggerSpike"]) && GodotObject.IsInstanceValid(ResourceManager.Instance.TOWERDEFENSE_CHARCATERS["ZombieImpDiggerSpike"]), "The real Spike Imp packet and scene must be registered.");
				control = new CloudImpSpikeDropRuntimeControlStub
				{
					Name = "CloudImpSpikeDropRuntimeControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				Node2D characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(characterNode, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = characterNode;
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				cloud = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter8/ImpCloud/Scene/TowerDefenseZombieImpCloud.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieImpCloud>(PackedScene.GenEditState.Disabled);
				Check(GodotObject.IsInstanceValid(cloud), "The real Cloud Imp scene must instantiate.");
				if (!GodotObject.IsInstanceValid(cloud))
				{
					goto end_IL_00e1;
				}
				cloud.editorPreviewMode = true;
				cloud.inGame = true;
				cloud.gridPos = CloudGrid;
				cloud.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(CloudGrid);
				characterNode.AddChild(cloud, forceReadableName: false, InternalMode.Disabled);
				await WaitFrames(4);
				Check(GodotObject.IsInstanceValid(cloud.sprite) && cloud.sprite.flashAnimeData.mediaDictionary.ContainsKey("Zombie_cloud_ball.png"), "The real Cloud Imp animation must expose its authored spike-ball media.");
				Check(CountSpikeImps(characterNode) == 0, "The focused fixture must begin without a spawned Spike Imp.");
				cloud.AnimeEvent("fire", default);
				await WaitFrames(2);
				Check(CountSpikeBallVisuals(characterNode) == 1, "The Cloud Imp throw must render exactly one texture-array-compatible spike-ball visual.");
				await WaitSeconds(0.85);
				await WaitFrames(3);
				TowerDefenseZombieImpDiggerSpike towerDefenseZombieImpDiggerSpike = FindNewestSpikeImp(characterNode);
				Check(CountSpikeImps(characterNode) == 1 && GodotObject.IsInstanceValid(towerDefenseZombieImpDiggerSpike), "The Cloud Imp fire action must drop exactly one real Spike Imp.");
				Check(towerDefenseZombieImpDiggerSpike != null && towerDefenseZombieImpDiggerSpike.inGame && towerDefenseZombieImpDiggerSpike.gridPos.Y == CloudGrid.Y, "The dropped Spike Imp must enter gameplay on the Cloud Imp's row.");
				Check(towerDefenseZombieImpDiggerSpike != null && towerDefenseZombieImpDiggerSpike.instance?.hypnoses == false, "A normal Cloud Imp must drop a normal-camp Spike Imp.");
				cloud.AnimeEvent("fire", default);
				cloud.QueueFree();
				cloud = null;
				await WaitSeconds(0.85);
				await WaitFrames(3);
				Check(CountSpikeImps(characterNode) == 2, "A spike ball already thrown must still land after its Cloud Imp is removed.");
				goto end_IL_00be;
				end_IL_00e1:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewCloudImpSpikeDropRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00be;
			}
			return;
			end_IL_00be:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(cloud))
			{
				cloud.QueueFree();
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			RestoreSpikeImpFixtures();
			await WaitFrames(2);
		}
		bool flag = _failures == 0 && _checks == 11;
		GD.Print($"CLOUD_IMP_SPIKE_DROP_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig()
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(gridNum.X + 1);
		for (int i = 0; i <= gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(gridNum.Y + 1);
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(new TowerDefenseCellConfig());
				array[j] = towerDefenseCellInstance;
			}
			towerDefenseBattleFeatureMap.plantGrid[i] = array;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(gridNum.Y + 1);
		return towerDefenseBattleFeatureMap;
	}

	private void RegisterSpikeImpFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		_hadPacket = instance.TOWERDEFENSE_PACKETS.TryGetValue("ZombieImpDiggerSpike", out _previousPacket);
		_hadCharacter = instance.TOWERDEFENSE_CHARCATERS.TryGetValue("ZombieImpDiggerSpike", out _previousCharacter);
		instance.TOWERDEFENSE_PACKETS["ZombieImpDiggerSpike"] = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Chapter8/ImpDiggerSpike/Packet/ZombieImpDiggerSpike.tres", null, ResourceLoader.CacheMode.Ignore);
		instance.TOWERDEFENSE_CHARCATERS["ZombieImpDiggerSpike"] = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter8/ImpDiggerSpike/Scene/TowerDefenseZombieImpDiggerSpike.tscn", null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreSpikeImpFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			if (_hadPacket)
			{
				instance.TOWERDEFENSE_PACKETS["ZombieImpDiggerSpike"] = _previousPacket;
			}
			else
			{
				instance.TOWERDEFENSE_PACKETS.Remove("ZombieImpDiggerSpike");
			}
			if (_hadCharacter)
			{
				instance.TOWERDEFENSE_CHARCATERS["ZombieImpDiggerSpike"] = _previousCharacter;
			}
			else
			{
				instance.TOWERDEFENSE_CHARCATERS.Remove("ZombieImpDiggerSpike");
			}
		}
	}

	private static int CountSpikeImps(Node parent)
	{
		int num = 0;
		foreach (Node child in parent.GetChildren())
		{
			if (child is TowerDefenseZombieImpDiggerSpike)
			{
				num++;
			}
		}
		return num;
	}

	private static int CountSpikeBallVisuals(Node parent)
	{
		int num = 0;
		foreach (Node child in parent.GetChildren())
		{
			if (child is AdobeAnimatePart)
			{
				num++;
			}
		}
		return num;
	}

	private static TowerDefenseZombieImpDiggerSpike FindNewestSpikeImp(Node parent)
	{
		TowerDefenseZombieImpDiggerSpike result = null;
		foreach (Node child in parent.GetChildren())
		{
			if (child is TowerDefenseZombieImpDiggerSpike towerDefenseZombieImpDiggerSpike)
			{
				result = towerDefenseZombieImpDiggerSpike;
			}
		}
		return result;
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task WaitSeconds(double seconds)
	{
		await ToSignal(GetTree().CreateTimer(seconds), SceneTreeTimer.SignalName.Timeout);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewCloudImpSpikeDropRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterSpikeImpFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreSpikeImpFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountSpikeImps, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountSpikeBallVisuals, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindNewestSpikeImp, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterSpikeImpFixtures && args.Count == 0)
		{
			RegisterSpikeImpFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreSpikeImpFixtures && args.Count == 0)
		{
			RestoreSpikeImpFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.CountSpikeImps && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountSpikeImps(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountSpikeBallVisuals && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountSpikeBallVisuals(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindNewestSpikeImp && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieImpDiggerSpike>(FindNewestSpikeImp(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountSpikeImps && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountSpikeImps(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.CountSpikeBallVisuals && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountSpikeBallVisuals(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindNewestSpikeImp && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseZombieImpDiggerSpike>(FindNewestSpikeImp(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CreateMapFeature)
		{
			return true;
		}
		if (method == MethodName.RegisterSpikeImpFixtures)
		{
			return true;
		}
		if (method == MethodName.RestoreSpikeImpFixtures)
		{
			return true;
		}
		if (method == MethodName.CountSpikeImps)
		{
			return true;
		}
		if (method == MethodName.CountSpikeBallVisuals)
		{
			return true;
		}
		if (method == MethodName.FindNewestSpikeImp)
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
		if (name == PropertyName._previousPacket)
		{
			_previousPacket = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._previousCharacter)
		{
			_previousCharacter = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._hadPacket)
		{
			_hadPacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hadCharacter)
		{
			_hadCharacter = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._previousPacket)
		{
			value = VariantUtils.CreateFrom(in _previousPacket);
			return true;
		}
		if (name == PropertyName._previousCharacter)
		{
			value = VariantUtils.CreateFrom(in _previousCharacter);
			return true;
		}
		if (name == PropertyName._hadPacket)
		{
			value = VariantUtils.CreateFrom(in _hadPacket);
			return true;
		}
		if (name == PropertyName._hadCharacter)
		{
			value = VariantUtils.CreateFrom(in _hadCharacter);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hadPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hadCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._previousPacket, Variant.From(in _previousPacket));
		info.AddProperty(PropertyName._previousCharacter, Variant.From(in _previousCharacter));
		info.AddProperty(PropertyName._hadPacket, Variant.From(in _hadPacket));
		info.AddProperty(PropertyName._hadCharacter, Variant.From(in _hadCharacter));
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
		if (info.TryGetProperty(PropertyName._previousPacket, out var value3))
		{
			_previousPacket = value3.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._previousCharacter, out var value4))
		{
			_previousCharacter = value4.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._hadPacket, out var value5))
		{
			_hadPacket = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hadCharacter, out var value6))
		{
			_hadCharacter = value6.As<bool>();
		}
	}
}
