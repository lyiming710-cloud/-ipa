using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/WaveFeaturePauseBarrierRuntimeTest.cs")]
public sealed class WaveFeaturePauseBarrierRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateWaveFeature = "CreateWaveFeature";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName RegisterRealZombie = "RegisterRealZombie";

		public static readonly StringName RestoreRealZombie = "RestoreRealZombie";

		public static readonly StringName CountLiveCharacters = "CountLiveCharacters";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _previousPacket = "_previousPacket";

		public static readonly StringName _previousCharacter = "_previousCharacter";

		public static readonly StringName _packetWasMissing = "_packetWasMissing";

		public static readonly StringName _characterWasMissing = "_characterWasMissing";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "WAVE_FEATURE_PAUSE_BARRIER_RUNTIME_RESULT";

	private const string ZombieName = "ZombieBalloonColour";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/BalloonColour/Packet/ZombieBalloonColourColour.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/BalloonColour/Scene/TowerDefenseZombieBalloonColour.tscn";

	private const int SpawnCount = 128;

	private const int SpawnLine = 2;

	private const int PausedObservationFrameCount = 12;

	private Resource _previousPacket;

	private Resource _previousCharacter;

	private bool _packetWasMissing;

	private bool _characterWasMissing;

	public override async void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		SceneTree tree = GetTree();
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		WaveFeaturePauseBarrierControlStub control = null;
		TowerDefenseInGameLevelControl levelControl = null;
		TowerDefenseMapControl mapControl = null;
		Node2D mapIceCap = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefenseBattleFeatureWave waveFeature = null;
		bool passed = false;
		try
		{
			_ = 3;
			try
			{
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Production battle autoloads are unavailable.");
				}
				await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
				RegisterRealZombie();
				control = new WaveFeaturePauseBarrierControlStub
				{
					Name = "WaveFeaturePauseBarrierControl",
					ProcessMode = ProcessModeEnum.Pausable,
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				levelControl = (control.levelControl = new TowerDefenseInGameLevelControl
				{
					awardCreate = false
				});
				TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
				{
					gridNum = new Vector2I(27, 15),
					gridBeginPos = new Vector2(160f, 30f),
					gridSize = new Vector2(80f, 76f),
					edge = new Vector4(100f, 0f, 2400f, 1200f)
				};
				manager.currentControl = control;
				manager.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
				manager.gridSize = towerDefenseMapConfig.gridSize;
				manager.gridNum = towerDefenseMapConfig.gridNum;
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapIceCap = new Node2D
				{
					Name = "MapIceCap"
				};
				AddChild(mapIceCap, forceReadableName: false, InternalMode.Disabled);
				mapControl.mapIceCap = mapIceCap;
				mapFeature = CreateMapFeature(mapControl, towerDefenseMapConfig);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				waveFeature = CreateWaveFeature(control, mapFeature);
				control.featureDictionary[new StringName("Wave")] = waveFeature;
				control.isGameRunning = true;
				tree.Paused = true;
				Task spawnTask = waveFeature.SpawnZombie(0);
				int immediateCharacters = waveFeature.currentCharacter.Count;
				await WaitProcessFrames(12);
				int pausedCharacters = waveFeature.currentCharacter.Count;
				bool taskCompletedWhilePaused = spawnTask.IsCompleted;
				tree.Paused = false;
				await spawnTask;
				await WaitProcessFrames(4);
				int count = waveFeature.currentCharacter.Count;
				int num = CountLiveCharacters(control.characterNode);
				passed = immediateCharacters == 0 && pausedCharacters == 0 && !taskCompletedWhilePaused && count == 128 && num == 128;
				GD.Print($"{"WAVE_FEATURE_PAUSE_BARRIER_RUNTIME_RESULT"} passed={passed} requested={128} immediate={immediateCharacters} paused={pausedCharacters} taskCompletedWhilePaused={taskCompletedWhilePaused} final={count} live={num} pausedFrames={12}");
			}
			catch (Exception value)
			{
				GD.PrintErr($"{"WAVE_FEATURE_PAUSE_BARRIER_RUNTIME_RESULT"} passed=False exception={value}");
			}
		}
		finally
		{
			tree.Paused = false;
			if (GodotObject.IsInstanceValid(control))
			{
				control.isGameRunning = false;
				if (GodotObject.IsInstanceValid(control.characterNode))
				{
					foreach (Node child in control.characterNode.GetChildren())
					{
						if (GodotObject.IsInstanceValid(child) && !child.IsQueuedForDeletion())
						{
							child.QueueFree();
						}
					}
				}
			}
			await WaitProcessFrames(4);
			waveFeature?.Destroy();
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(mapIceCap) && !mapIceCap.IsQueuedForDeletion())
			{
				mapIceCap.QueueFree();
			}
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(levelControl))
			{
				levelControl.Free();
			}
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			RestoreRealZombie();
			await WaitProcessFrames(8);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		GetTree().Quit((!passed) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureWave CreateWaveFeature(WaveFeaturePauseBarrierControlStub control, TowerDefenseBattleFeatureMap mapFeature)
	{
		TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = new TowerDefenseLevelWaveConfig();
		towerDefenseLevelWaveConfig.spawn.Add(new TowerDefenseLevelSpawnConfig
		{
			zombie = "ZombieBalloonColour",
			line = 2,
			num = 128
		});
		TowerDefenseLevelWaveManagerConfig towerDefenseLevelWaveManagerConfig = new TowerDefenseLevelWaveManagerConfig
		{
			flagZombieUse = false,
			spawnMaxCharactersPerFrame = 8,
			spawnFrameBudgetMilliseconds = 16.0
		};
		towerDefenseLevelWaveManagerConfig.wave.Add(towerDefenseLevelWaveConfig);
		return new TowerDefenseBattleFeatureWave
		{
			control = control,
			levelControl = control.levelControl,
			mapFeature = mapFeature,
			config = towerDefenseLevelWaveManagerConfig,
			currentDynamic = new TowerDefenseLevelDynamicConfig()
		};
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, TowerDefenseMapConfig mapConfig)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = mapConfig
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(mapConfig.gridNum.X + 1);
		for (int i = 0; i <= mapConfig.gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(mapConfig.gridNum.Y + 1);
			for (int j = 1; j <= mapConfig.gridNum.Y; j++)
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
		towerDefenseBattleFeatureMap.lineUse.Resize(mapConfig.gridNum.Y + 1);
		for (int k = 0; k <= mapConfig.gridNum.Y; k++)
		{
			towerDefenseBattleFeatureMap.lineUse[k] = k == 2;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(mapConfig.gridNum.Y + 1);
		return towerDefenseBattleFeatureMap;
	}

	private void RegisterRealZombie()
	{
		ResourceManager instance = ResourceManager.Instance;
		_packetWasMissing = !instance.TOWERDEFENSE_PACKETS.TryGetValue("ZombieBalloonColour", out _previousPacket);
		_characterWasMissing = !instance.TOWERDEFENSE_CHARCATERS.TryGetValue("ZombieBalloonColour", out _previousCharacter);
		instance.TOWERDEFENSE_PACKETS["ZombieBalloonColour"] = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Chapter1/BalloonColour/Packet/ZombieBalloonColourColour.tres", null, ResourceLoader.CacheMode.Ignore);
		instance.TOWERDEFENSE_CHARCATERS["ZombieBalloonColour"] = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/BalloonColour/Scene/TowerDefenseZombieBalloonColour.tscn", null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreRealZombie()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			if (_packetWasMissing)
			{
				instance.TOWERDEFENSE_PACKETS.Remove("ZombieBalloonColour");
			}
			else
			{
				instance.TOWERDEFENSE_PACKETS["ZombieBalloonColour"] = _previousPacket;
			}
			if (_characterWasMissing)
			{
				instance.TOWERDEFENSE_CHARCATERS.Remove("ZombieBalloonColour");
			}
			else
			{
				instance.TOWERDEFENSE_CHARCATERS["ZombieBalloonColour"] = _previousCharacter;
			}
		}
	}

	private static int CountLiveCharacters(Node characterNode)
	{
		int num = 0;
		foreach (Node child in characterNode.GetChildren())
		{
			if (child is TowerDefenseZombie towerDefenseZombie && GodotObject.IsInstanceValid(towerDefenseZombie) && !towerDefenseZombie.IsQueuedForDeletion() && towerDefenseZombie.config?.name == "ZombieBalloonColour")
			{
				num++;
			}
		}
		return num;
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWaveFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mapFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterRealZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreRealZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountLiveCharacters, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.CreateWaveFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureWave>(CreateWaveFeature(VariantUtils.ConvertTo<WaveFeaturePauseBarrierControlStub>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterRealZombie && args.Count == 0)
		{
			RegisterRealZombie();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreRealZombie && args.Count == 0)
		{
			RestoreRealZombie();
			ret = default;
			return true;
		}
		if (method == MethodName.CountLiveCharacters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveCharacters(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateWaveFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureWave>(CreateWaveFeature(VariantUtils.ConvertTo<WaveFeaturePauseBarrierControlStub>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.CountLiveCharacters && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveCharacters(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.CreateWaveFeature)
		{
			return true;
		}
		if (method == MethodName.CreateMapFeature)
		{
			return true;
		}
		if (method == MethodName.RegisterRealZombie)
		{
			return true;
		}
		if (method == MethodName.RestoreRealZombie)
		{
			return true;
		}
		if (method == MethodName.CountLiveCharacters)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
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
		if (name == PropertyName._packetWasMissing)
		{
			_packetWasMissing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._characterWasMissing)
		{
			_characterWasMissing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
		if (name == PropertyName._packetWasMissing)
		{
			value = VariantUtils.CreateFrom(in _packetWasMissing);
			return true;
		}
		if (name == PropertyName._characterWasMissing)
		{
			value = VariantUtils.CreateFrom(in _characterWasMissing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._previousPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._packetWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._characterWasMissing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._previousPacket, Variant.From(in _previousPacket));
		info.AddProperty(PropertyName._previousCharacter, Variant.From(in _previousCharacter));
		info.AddProperty(PropertyName._packetWasMissing, Variant.From(in _packetWasMissing));
		info.AddProperty(PropertyName._characterWasMissing, Variant.From(in _characterWasMissing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._previousPacket, out var value))
		{
			_previousPacket = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._previousCharacter, out var value2))
		{
			_previousCharacter = value2.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._packetWasMissing, out var value3))
		{
			_packetWasMissing = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._characterWasMissing, out var value4))
		{
			_characterWasMissing = value4.As<bool>();
		}
	}
}
