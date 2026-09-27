using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/TowerDefenseBattleFeatureMap.cs")]
public class TowerDefenseBattleFeatureMap : TowerDefenseBattleFeature, ITowerDefenseProgressSaveGuard
{
	private sealed class MapChangeOperation
	{
		public int Id = -1;

		public TowerDefenseMapConfig TargetConfig;

		public TowerDefenseMapConfig PreviousConfig;

		public PackedScene TargetScene;

		public TowerDefenseMap SourceMap;

		public TowerDefenseMap CandidateMap;

		public GradientTexture1D Gradient;

		public Tween Tween;

		public SceneTreeTimer DelayTimer;

		public Action DelayFinishedHandler;

		public Action TweenFinishedHandler;

		public TaskCompletionSource<bool> Waiter;

		public Color SourceMapModulate;

		public Color SourceCanvasColor;

		public bool HasSourceMapModulate;

		public bool HasSourceCanvasColor;

		public bool OperationCompleted;

		public bool Committed;
	}

	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName TryApplySavedMapConfigBeforeLoadingCells = "TryApplySavedMapConfigBeforeLoadingCells";

		public static readonly StringName _SaveCell = "_SaveCell";

		public static readonly StringName _LoadCellInto = "_LoadCellInto";

		public new static readonly StringName Init = "Init";

		public static readonly StringName CanDrainPendingMapActions = "CanDrainPendingMapActions";

		public static readonly StringName SchedulePendingMapActionDrain = "SchedulePendingMapActionDrain";

		public static readonly StringName DrainPendingMapActions = "DrainPendingMapActions";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public static readonly StringName GetSyncLineUseSnapshot = "GetSyncLineUseSnapshot";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName MapInit = "MapInit";

		public static readonly StringName EnsureIceCapRuntimeCache = "EnsureIceCapRuntimeCache";

		public static readonly StringName HasGridStorageSize = "HasGridStorageSize";

		public static readonly StringName ClearIceCapNodes = "ClearIceCapNodes";

		public static readonly StringName UpdateIsPlantColumn = "UpdateIsPlantColumn";

		public static readonly StringName GetGroundHeight = "GetGroundHeight";

		public static readonly StringName PlantGridInit = "PlantGridInit";

		public static readonly StringName EnsurePlantGridRuntimeCache = "EnsurePlantGridRuntimeCache";

		public static readonly StringName GetPlantGridCell = "GetPlantGridCell";

		public static readonly StringName ApplySavedLineUse = "ApplySavedLineUse";

		public static readonly StringName ProcessInput = "ProcessInput";

		public static readonly StringName ResolveViewportInputPosition = "ResolveViewportInputPosition";

		public static readonly StringName ResolveInputGridPosition = "ResolveInputGridPosition";

		public static readonly StringName NotifyMapTransformChanged = "NotifyMapTransformChanged";

		public static readonly StringName MapChange = "MapChange";

		public static readonly StringName IsActiveMapChangeTarget = "IsActiveMapChangeTarget";

		public static readonly StringName ApplyCommittedMapState = "ApplyCommittedMapState";

		public static readonly StringName ApplyMapFrameRateLimit = "ApplyMapFrameRateLimit";

		public static readonly StringName RefreshMapFrameRateLimit = "RefreshMapFrameRateLimit";

		public static readonly StringName RestoreMapFrameRateLimit = "RestoreMapFrameRateLimit";

		public static readonly StringName ResolveMapMaximumFps = "ResolveMapMaximumFps";

		public static readonly StringName BuildProjectileBoundaryRect = "BuildProjectileBoundaryRect";

		public static readonly StringName CancelActiveMapChange = "CancelActiveMapChange";

		public static readonly StringName NotifyMapChanged = "NotifyMapChanged";

		public static readonly StringName AdvanceMapRevision = "AdvanceMapRevision";

		public static readonly StringName GetRegisteredMapId = "GetRegisteredMapId";

		public static readonly StringName GetMapConfigPath = "GetMapConfigPath";

		public static readonly StringName HasStableMapIdentity = "HasStableMapIdentity";

		public static readonly StringName ResolveMapConfigIdentity = "ResolveMapConfigIdentity";

		public static readonly StringName ReportUnresolvedSyncedMap = "ReportUnresolvedSyncedMap";

		public static readonly StringName IsRemoteMultiplayerClient = "IsRemoteMultiplayerClient";

		public static readonly StringName IsRemoteMultiplayerHost = "IsRemoteMultiplayerHost";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName IsSameMapConfig = "IsSameMapConfig";

		public static readonly StringName MapDayNightSwitch = "MapDayNightSwitch";

		public static readonly StringName UpdateSwitchTimer = "UpdateSwitchTimer";

		public static readonly StringName RestoreMapSwitchSchedule = "RestoreMapSwitchSchedule";

		public static readonly StringName StopMapSwitchSchedule = "StopMapSwitchSchedule";

		public static readonly StringName NormalizeNonNegativeFinite = "NormalizeNonNegativeFinite";

		public static readonly StringName SetGridType = "SetGridType";

		public static readonly StringName SetLineUse = "SetLineUse";

		public static readonly StringName LineHasType = "LineHasType";

		public static readonly StringName GetCellPlantNum = "GetCellPlantNum";

		public static readonly StringName SetIceCapPos = "SetIceCapPos";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName PlantGridRevision = "PlantGridRevision";

		public static readonly StringName mapControl = "mapControl";

		public static readonly StringName mapConfig = "mapConfig";

		public static readonly StringName editorPreviewMode = "editorPreviewMode";

		public static readonly StringName config = "config";

		public static readonly StringName firstConfig = "firstConfig";

		public static readonly StringName changeConfig = "changeConfig";

		public static readonly StringName isSwitch = "isSwitch";

		public static readonly StringName switchTimer = "switchTimer";

		public static readonly StringName switchTween = "switchTween";

		public static readonly StringName stripeRow = "stripeRow";

		public static readonly StringName lineUse = "lineUse";

		public static readonly StringName plantGrid = "plantGrid";

		public static readonly StringName iceCapList = "iceCapList";

		public static readonly StringName currentMap = "currentMap";

		public static readonly StringName nextMap = "nextMap";

		public static readonly StringName isChange = "isChange";

		public static readonly StringName currentGradientPos = "currentGradientPos";

		public static readonly StringName currentGradient = "currentGradient";

		public static readonly StringName rect = "rect";

		public static readonly StringName groundRect = "groundRect";

		public static readonly StringName isPlantColumn = "isPlantColumn";

		public static readonly StringName shovelManager = "shovelManager";

		public static readonly StringName gloveManager = "gloveManager";

		public static readonly StringName packetPickControl = "packetPickControl";

		public static readonly StringName _destroyed = "_destroyed";

		public static readonly StringName _mapRevision = "_mapRevision";

		public static readonly StringName _lastAppliedMapRevision = "_lastAppliedMapRevision";

		public static readonly StringName _lastUnresolvedMapRevision = "_lastUnresolvedMapRevision";

		public static readonly StringName _lastUnresolvedMapIdentity = "_lastUnresolvedMapIdentity";

		public static readonly StringName _switchReturnConfig = "_switchReturnConfig";

		public static readonly StringName _switchResidenceConfig = "_switchResidenceConfig";

		public static readonly StringName _switchReturnDuration = "_switchReturnDuration";

		public static readonly StringName _isDrainingMapActions = "_isDrainingMapActions";

		public static readonly StringName _mapActionDrainScheduled = "_mapActionDrainScheduled";

		public static readonly StringName _gridReady = "_gridReady";

		public static readonly StringName _lastInputPhysicsFrame = "_lastInputPhysicsFrame";

		public static readonly StringName _hasCachedInputGridPosition = "_hasCachedInputGridPosition";

		public static readonly StringName _cachedInputMousePosition = "_cachedInputMousePosition";

		public static readonly StringName _cachedInputGridPosition = "_cachedInputGridPosition";

		public static readonly StringName _committedMapId = "_committedMapId";

		public static readonly StringName _committedMapPath = "_committedMapPath";

		public static readonly StringName _maximumFpsBeforeMap = "_maximumFpsBeforeMap";

		public static readonly StringName _currentMapMaximumFps = "_currentMapMaximumFps";

		public static readonly StringName _mapFrameRateLimitActive = "_mapFrameRateLimitActive";

		public static readonly StringName _syncLineUseSnapshot = "_syncLineUseSnapshot";

		public static readonly StringName _syncLineUseDirty = "_syncLineUseDirty";

		public static readonly StringName _iceCapRuntimeCache = "_iceCapRuntimeCache";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _towerDefenseMapControl;

	private static PackedScene _towerDefenseIceCap;

	private const int MaxPendingMapActions = 512;

	private const int MaxPendingMapActionsPerDrain = 64;

	public TowerDefenseMapControl mapControl;

	public TowerDefenseMapConfig mapConfig;

	public bool editorPreviewMode;

	public TowerDefenseMapConfig config;

	public TowerDefenseMapConfig firstConfig;

	public TowerDefenseMapConfig changeConfig;

	public bool isSwitch;

	public double switchTimer = 100.0;

	public Tween switchTween;

	public int stripeRow = -1;

	public Array<bool> lineUse = new Array<bool>();

	public Array<Godot.Collections.Array> plantGrid = new Array<Godot.Collections.Array>();

	public Godot.Collections.Array iceCapList = new Godot.Collections.Array();

	public TowerDefenseMap currentMap;

	public TowerDefenseMap nextMap;

	public bool isChange;

	public double currentGradientPos;

	public GradientTexture1D currentGradient;

	public Rect2 rect;

	public Rect2 groundRect;

	public bool isPlantColumn;

	public ShovelManager shovelManager;

	public GloveManager gloveManager;

	public PacketPickControl packetPickControl;

	private MapChangeOperation _activeMapChange;

	private bool _destroyed;

	private long _mapRevision;

	private long _lastAppliedMapRevision = -1L;

	private long _lastUnresolvedMapRevision = -1L;

	private string _lastUnresolvedMapIdentity = "";

	private TowerDefenseMapConfig _switchReturnConfig;

	private TowerDefenseMapConfig _switchResidenceConfig;

	private double _switchReturnDuration = 2.0;

	private readonly Queue<Action> _pendingMapActions = new Queue<Action>();

	private bool _isDrainingMapActions;

	private bool _mapActionDrainScheduled;

	private bool _gridReady;

	private ulong _lastInputPhysicsFrame = 18446744073709551615uL;

	private bool _hasCachedInputGridPosition;

	private Vector2 _cachedInputMousePosition;

	private Vector2I _cachedInputGridPosition;

	private string _committedMapId = "";

	private string _committedMapPath = "";

	private int _maximumFpsBeforeMap;

	private int _currentMapMaximumFps = -1;

	private bool _mapFrameRateLimitActive;

	private Godot.Collections.Array _syncLineUseSnapshot = new Godot.Collections.Array();

	private bool _syncLineUseDirty = true;

	private TowerDefenseIceCap[] _iceCapRuntimeCache = System.Array.Empty<TowerDefenseIceCap>();

	private readonly HashSet<int> _warnedInvalidIceCapLines = new HashSet<int>();

	private TowerDefenseCellInstance[][] _plantGridRuntimeCache = System.Array.Empty<TowerDefenseCellInstance[]>();

	private static PackedScene TOWER_DEFENSE_MAP_CONTROL => _towerDefenseMapControl ?? (_towerDefenseMapControl = GD.Load<PackedScene>("uid://q2eo6wgcbefw"));

	private static PackedScene TOWER_DEFENSE_ICE_CAP => _towerDefenseIceCap ?? (_towerDefenseIceCap = GD.Load<PackedScene>("uid://b7eq2mfpja1dy"));

	public ulong PlantGridRevision { get; private set; }

	public bool CanSaveProgress(out string reason)
	{
		if (_isDrainingMapActions || _pendingMapActions.Count > 0)
		{
			reason = "pending map actions have not been applied";
			return false;
		}
		if (!GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(currentMap))
		{
			reason = "the committed map is unavailable";
			return false;
		}
		if (!currentMap.CanSaveProgress(out reason))
		{
			return false;
		}
		if (!HasStableMapIdentity(config))
		{
			reason = "the committed map has no registered id or resource path";
			return false;
		}
		if (isSwitch && (!HasStableMapIdentity(_switchReturnConfig) || !HasStableMapIdentity(_switchResidenceConfig)))
		{
			reason = "the temporary map schedule has no stable source or target identity";
			return false;
		}
		reason = "";
		return true;
	}

	public override Dictionary SaveFeature()
	{
		GD.Print("[Save] 保存Feature[Map]...");
		Godot.Collections.Array array = new Godot.Collections.Array();
		int num = (GodotObject.IsInstanceValid(config) ? Mathf.Clamp(config.gridNum.X + 1, 0, plantGrid.Count) : plantGrid.Count);
		for (int i = 0; i < num; i++)
		{
			Godot.Collections.Array array2 = new Godot.Collections.Array();
			int num2 = (GodotObject.IsInstanceValid(config) ? Mathf.Clamp(config.gridNum.Y + 1, 0, plantGrid[i].Count) : plantGrid[i].Count);
			for (int j = 0; j < num2; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = plantGrid[i][j].As<TowerDefenseCellInstance>();
				if (GodotObject.IsInstanceValid(towerDefenseCellInstance))
				{
					array2.Add(_SaveCell(towerDefenseCellInstance));
				}
				else
				{
					array2.Add(default);
				}
			}
			array.Add(array2);
		}
		Godot.Collections.Array array3 = new Godot.Collections.Array();
		TowerDefenseBattleFeatureMower mowerFeature = TowerDefenseManager.Instance.GetMowerFeature();
		if (mowerFeature != null)
		{
			foreach (TowerDefenseMower item in mowerFeature.mowerLine)
			{
				if (GodotObject.IsInstanceValid(item))
				{
					array3.Add(item.Name.ToString().ValidateNodeName());
				}
				else
				{
					array3.Add("");
				}
			}
		}
		Godot.Collections.Array array4 = new Godot.Collections.Array();
		TowerDefenseBattleFeatureBrain brainFeature = TowerDefenseManager.Instance.GetBrainFeature();
		if (brainFeature != null)
		{
			foreach (TowerDefenseItem item2 in brainFeature.brainLine)
			{
				if (GodotObject.IsInstanceValid(item2))
				{
					array4.Add(item2.Name.ToString().ValidateNodeName());
				}
				else
				{
					array4.Add("");
				}
			}
		}
		Godot.Collections.Array array5 = new Godot.Collections.Array();
		if (mowerFeature != null)
		{
			foreach (TowerDefenseCharacter item3 in mowerFeature.targetZombieLine)
			{
				if (GodotObject.IsInstanceValid(item3))
				{
					array5.Add(item3.Name.ToString().ValidateNodeName());
				}
				else
				{
					array5.Add("");
				}
			}
		}
		Dictionary dictionary = new Dictionary();
		if (GodotObject.IsInstanceValid(currentMap))
		{
			dictionary = currentMap.SaveMapBase();
		}
		return new Dictionary
		{
			{ "isSwitch", isSwitch },
			{
				"switchTimer",
				double.IsFinite(switchTimer) ? switchTimer : 0.0
			},
			{
				"switchReturnMapId",
				GetRegisteredMapId(_switchReturnConfig)
			},
			{
				"switchReturnMapPath",
				GetMapConfigPath(_switchReturnConfig)
			},
			{
				"switchResidenceMapId",
				GetRegisteredMapId(_switchResidenceConfig)
			},
			{
				"switchResidenceMapPath",
				GetMapConfigPath(_switchResidenceConfig)
			},
			{ "switchReturnDuration", _switchReturnDuration },
			{ "stripeRow", stripeRow },
			{ "isPlantColumn", isPlantColumn },
			{
				"lineUse",
				lineUse.Duplicate(deep: true)
			},
			{ "plantGrid", array },
			{ "mowerLine", array3 },
			{ "brainLine", array4 },
			{ "targetZombieLine", array5 },
			{
				"mowerHasRun",
				mowerFeature?.mowerHasRun ?? false
			},
			{ "mapId", _committedMapId },
			{ "mapPath", _committedMapPath },
			{ "mapBase", dictionary }
		};
	}

	public override void LoadFeature(Dictionary data, TowerDefenseLevelSaveConfigCSharp owner)
	{
		ProgressRestoreReport report = owner.RestoreReport;
		bool accepted = false;
		report.Try("Map", () =>
		{
			accepted = TryPrepareProgressLoad(data, out var reason);
			if (!accepted)
			{
				report.Record("Map", reason);
			}
		});
		isChange = false;
		currentGradientPos = 0.0;
		stripeRow = -1;
		if (!accepted)
		{
			StopMapSwitchSchedule();
		}
		else
		{
			report.Try("Map", () =>
			{
				isSwitch = owner.ReadBool(data, "isSwitch", owner.ReadBool(data, "isSwich", fallback: false));
				switchTimer = owner.ReadFinite(data, "switchTimer", 0.0);
				stripeRow = data.GetValueOrDefault("stripeRow", data.GetValueOrDefault("strigeRow", -1)).AsInt32();
				isPlantColumn = owner.ReadBool(data, "isPlantColumn", fallback: false);
				Godot.Collections.Array array2 = ReadProgressArray(data, "lineUse", report);
				if (array2.Count > 0)
				{
					ApplySavedLineUse(array2);
				}
			});
		}
		Godot.Collections.Array array = ReadProgressArray(data, "plantGrid", report);
		for (int num = 0; num < Math.Min(array.Count, plantGrid.Count); num++)
		{
			if (array[num].VariantType != Variant.Type.Array)
			{
				report.Record("Map", "Skipped invalid grid column.");
				continue;
			}
			Godot.Collections.Array column = array[num].AsGodotArray();
			for (int num2 = 0; num2 < Math.Min(column.Count, plantGrid[num].Count); num2++)
			{
				if (column[num2].VariantType == Variant.Type.Nil)
				{
					continue;
				}
				int columnIndex = num;
				int rowIndex = num2;
				report.Try("Map", () =>
				{
					if (column[rowIndex].VariantType != Variant.Type.Dictionary)
					{
						throw new InvalidOperationException("Invalid grid cell.");
					}
					_LoadCellInto(plantGrid[columnIndex][rowIndex].As<TowerDefenseCellInstance>(), column[rowIndex].AsGodotDictionary(), owner, accepted);
				});
			}
			if (column.Count > plantGrid[num].Count)
			{
				report.Record("Map", "Trimmed out-of-bounds grid cells.");
			}
		}
		if (array.Count > plantGrid.Count)
		{
			report.Record("Map", "Trimmed out-of-bounds grid columns.");
		}
		TowerDefenseBattleFeatureMower mowerFeature = TowerDefenseManager.Instance.GetMowerFeature();
		if (mowerFeature != null)
		{
			mowerFeature.mowerHasRun = owner.ReadBool(data, "mowerHasRun", fallback: false);
			RestoreProgressLine(mowerFeature.mowerLine, ReadProgressArray(data, "mowerLine", report), 51, owner);
			RestoreProgressLine(mowerFeature.targetZombieLine, ReadProgressArray(data, "targetZombieLine", report), 51, owner);
		}
		TowerDefenseBattleFeatureBrain brainFeature = TowerDefenseManager.Instance.GetBrainFeature();
		if (brainFeature != null)
		{
			brainFeature.NormalizeBrainLineSize();
			RestoreProgressLine(brainFeature.brainLine, ReadProgressArray(data, "brainLine", report), brainFeature.brainLine.Count, owner);
			brainFeature.RebindBrainDestroySignals();
		}
		if (!accepted)
		{
			return;
		}
		report.Try("Map", () =>
		{
			RestoreMapSwitchSchedule(data);
		});
		report.Try("Map", () =>
		{
			if (TowerDefenseLevelSaveConfigCSharp.TryGetSection(data, "mapBase", out var dictionary) && GodotObject.IsInstanceValid(currentMap))
			{
				currentMap.LoadMapBase(dictionary);
			}
		});
		report.Try("Map", () =>
		{
			if (stripeRow >= 0 && GodotObject.IsInstanceValid(currentMap) && GodotObject.IsInstanceValid(currentMap.stripe))
			{
				currentMap.UseStripe(stripeRow);
			}
		});
	}

	private static Godot.Collections.Array ReadProgressArray(Dictionary data, string key, ProgressRestoreReport report)
	{
		if (!data.TryGetValue(key, out var value))
		{
			return new Godot.Collections.Array();
		}
		if (value.VariantType == Variant.Type.Array)
		{
			return value.AsGodotArray();
		}
		report.Record("Map", "Invalid map array: " + key + ".");
		return new Godot.Collections.Array();
	}

	private static void RestoreProgressLine<[MustBeVariant] T>(Array<T> target, Godot.Collections.Array saved, int size, TowerDefenseLevelSaveConfigCSharp owner) where T : TowerDefenseCharacter
	{
		target.Clear();
		target.Resize(size);
		for (int i = 0; i < Math.Min(size, saved.Count); i++)
		{
			int index = i;
			owner.RestoreReport.Try("Map", () =>
			{
				string text = saved[index].AsString();
				if (!string.IsNullOrEmpty(text) && owner.charcterDicionary.TryGetValue(text, out var value))
				{
					target[index] = value as T;
				}
			});
		}
	}

	public bool TryPrepareProgressLoad(Dictionary savedData, out string reason)
	{
		CancelActiveMapChange();
		if (!TryApplySavedMapConfigBeforeLoadingCells(savedData))
		{
			reason = "the saved map identity is unavailable or incompatible";
			return false;
		}
		changeConfig = config;
		isChange = false;
		currentGradientPos = 0.0;
		currentGradient = null;
		reason = "";
		return true;
	}

	public bool CanPrepareProgressLoad(Dictionary savedData, out string reason)
	{
		if (!TryResolveProgressMapConfig(savedData, out var savedConfig))
		{
			reason = "the saved map identity cannot be resolved";
			return false;
		}
		if (!GodotObject.IsInstanceValid(savedConfig))
		{
			reason = "";
			return true;
		}
		if (!savedConfig.TryValidateRuntime(out reason))
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(mapConfig) && !mapConfig.IsRuntimeTopologyCompatibleWith(savedConfig, out reason))
		{
			return false;
		}
		PackedScene mapScene = savedConfig.GetMapScene(cache: false);
		if (!GodotObject.IsInstanceValid(mapScene) || !mapScene.CanInstantiate())
		{
			savedConfig.ClearLoadedMapResources();
			reason = "the saved map scene is unavailable";
			return false;
		}
		Node node = null;
		try
		{
			node = mapScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (!(node is TowerDefenseMap towerDefenseMap))
			{
				reason = "the saved map scene root is not a TowerDefenseMap";
				return false;
			}
			if (!GodotObject.IsInstanceValid(towerDefenseMap.canvasModulateGradient) || !GodotObject.IsInstanceValid(towerDefenseMap.canvasModulateGradient.Gradient))
			{
				reason = "the saved map scene has no valid canvas gradient";
				return false;
			}
			if (savedData.GetValueOrDefault("stripeRow", savedData.GetValueOrDefault("strigeRow", -1)).AsInt32() >= 0 && !GodotObject.IsInstanceValid(towerDefenseMap.stripe))
			{
				reason = "the saved map scene cannot restore its stripe";
				return false;
			}
		}
		catch (Exception ex)
		{
			reason = "the saved map scene cannot be instantiated: " + ex.Message;
			return false;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(node))
			{
				node.Free();
			}
			savedConfig.ClearLoadedMapResources();
		}
		reason = "";
		return true;
	}

	private bool TryApplySavedMapConfigBeforeLoadingCells(Dictionary _data)
	{
		if (!TryResolveProgressMapConfig(_data, out var savedConfig))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(savedConfig))
		{
			return true;
		}
		if (!savedConfig.TryValidateRuntime(out var reason) || (GodotObject.IsInstanceValid(mapConfig) && !mapConfig.IsRuntimeTopologyCompatibleWith(savedConfig, out reason)))
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(currentMap) && IsSameMapConfig(config, savedConfig))
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(config))
		{
			MapInit(savedConfig);
			if (GodotObject.IsInstanceValid(currentMap))
			{
				return IsSameMapConfig(config, savedConfig);
			}
			return false;
		}
		MapChange(savedConfig);
		int num;
		if (GodotObject.IsInstanceValid(currentMap))
		{
			num = (IsSameMapConfig(config, savedConfig) ? 1 : 0);
			if (num != 0)
			{
				PlantGridInit();
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	private static bool TryResolveProgressMapConfig(Dictionary savedData, out TowerDefenseMapConfig savedConfig)
	{
		bool flag = savedData.ContainsKey("mapId") || savedData.ContainsKey("mapPath");
		bool flag2 = savedData.ContainsKey("configPath");
		string text = savedData.GetValueOrDefault("mapId", "").AsString();
		string text2 = savedData.GetValueOrDefault("mapPath", savedData.GetValueOrDefault("configPath", "")).AsString();
		if (string.IsNullOrWhiteSpace(text) && string.IsNullOrWhiteSpace(text2))
		{
			savedConfig = null;
			if (!flag)
			{
				if (flag2)
				{
					return string.IsNullOrWhiteSpace(text2);
				}
				return true;
			}
			return false;
		}
		savedConfig = ResolveMapConfigIdentity(text, text2);
		return GodotObject.IsInstanceValid(savedConfig);
	}

	private Dictionary _SaveCell(TowerDefenseCellInstance cellInstance)
	{
		cellInstance.ClearEmpty();
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseCharacter character in cellInstance.characterList)
		{
			if (GodotObject.IsInstanceValid(character))
			{
				array.Add(character.Name.ToString().ValidateNodeName());
			}
		}
		Dictionary dictionary = new Dictionary();
		foreach (TowerDefenseCharacter key in cellInstance.characterSlotDictionary.Keys)
		{
			if (GodotObject.IsInstanceValid(key))
			{
				TowerDefenseCharacter towerDefenseCharacter = cellInstance.characterSlotDictionary[key];
				if (GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					dictionary[key.Name.ToString().ValidateNodeName()] = towerDefenseCharacter.Name.ToString().ValidateNodeName();
				}
				else
				{
					dictionary[key.Name.ToString().ValidateNodeName()] = "";
				}
			}
		}
		Dictionary dictionary2 = new Dictionary();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE key2 in cellInstance.slot.Keys)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = cellInstance.slot[key2];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter2))
			{
				dictionary2[(int)key2] = towerDefenseCharacter2.Name.ToString().ValidateNodeName();
			}
			else
			{
				dictionary2[(int)key2] = "";
			}
		}
		return new Dictionary
		{
			{ "gridType", cellInstance.gridType },
			{ "elementFlags", cellInstance.elementFlags },
			{ "isWater", cellInstance.isWater },
			{
				"gridPosX",
				cellInstance.gridPos.X
			},
			{
				"gridPosY",
				cellInstance.gridPos.Y
			},
			{ "characterList", array },
			{ "characterSlotDictionary", dictionary },
			{ "slot", dictionary2 },
			{
				"characterSurround",
				GodotObject.IsInstanceValid(cellInstance.characterSurround) ? cellInstance.characterSurround.Name.ToString().ValidateNodeName() : ""
			},
			{
				"characterLadder",
				GodotObject.IsInstanceValid(cellInstance.characterLadder) ? cellInstance.characterLadder.Name.ToString().ValidateNodeName() : ""
			},
			{
				"itemShield",
				GodotObject.IsInstanceValid(cellInstance.itemShield) ? cellInstance.itemShield.Name.ToString().ValidateNodeName() : ""
			}
		};
	}

	private void _LoadCellInto(TowerDefenseCellInstance cellInstance, Dictionary cellData, TowerDefenseLevelSaveConfigCSharp _owner, bool restoreTerrain = true)
	{
		if (!GodotObject.IsInstanceValid(cellInstance))
		{
			return;
		}
		Godot.Collections.Array array = cellData.GetValueOrDefault("gridType", new Godot.Collections.Array()).AsGodotArray();
		if (restoreTerrain && array.Count > 0)
		{
			cellInstance.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>(array.Select((Variant v) => (TowerDefenseEnum.PLANTGRIDTYPE)v.AsInt32()));
		}
		if (restoreTerrain)
		{
			cellInstance.elementFlags = cellData.GetValueOrDefault("elementFlags", 0).AsInt32();
			cellInstance.isWater = cellData.GetValueOrDefault("isWater", false).AsBool();
		}
		cellInstance.characterList.Clear();
		cellInstance.characterSlotDictionary.Clear();
		cellInstance.slot.Clear();
		cellInstance.characterSurround = null;
		cellInstance.characterLadder = null;
		cellInstance.itemShield = null;
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in cellInstance.gridType)
		{
			cellInstance.slot[item] = null;
		}
		foreach (Variant item2 in cellData.GetValueOrDefault("characterList", new Godot.Collections.Array()).AsGodotArray())
		{
			StringName key = item2.AsStringName();
			if (_owner.charcterDicionary.ContainsKey(key))
			{
				TowerDefenseCharacter towerDefenseCharacter = _owner.charcterDicionary[key];
				cellInstance.characterList.Add(towerDefenseCharacter);
				towerDefenseCharacter.OnDestroy += cellInstance.CharacterDestroy;
				towerDefenseCharacter.cell = cellInstance;
			}
		}
		Dictionary dictionary = cellData.GetValueOrDefault("characterSlotDictionary", new Dictionary()).AsGodotDictionary();
		foreach (Variant key4 in dictionary.Keys)
		{
			StringName key2 = key4.AsStringName();
			string text = dictionary[key4].AsString();
			if (_owner.charcterDicionary.ContainsKey(key2))
			{
				if (text != "" && _owner.charcterDicionary.ContainsKey(text))
				{
					cellInstance.characterSlotDictionary[_owner.charcterDicionary[key2]] = _owner.charcterDicionary[text];
				}
				else
				{
					cellInstance.characterSlotDictionary[_owner.charcterDicionary[key2]] = null;
				}
			}
		}
		Dictionary dictionary2 = cellData.GetValueOrDefault("slot", new Dictionary()).AsGodotDictionary();
		foreach (Variant key5 in dictionary2.Keys)
		{
			string text2 = dictionary2[key5].AsString();
			TowerDefenseEnum.PLANTGRIDTYPE key3 = (TowerDefenseEnum.PLANTGRIDTYPE)(int)key5;
			if (text2 != "" && _owner.charcterDicionary.ContainsKey(text2))
			{
				cellInstance.slot[key3] = _owner.charcterDicionary[text2];
			}
			else
			{
				cellInstance.slot[key3] = null;
			}
		}
		string text3 = cellData.GetValueOrDefault("characterSurround", "").AsString();
		if (text3 != "" && _owner.charcterDicionary.ContainsKey(text3))
		{
			cellInstance.characterSurround = _owner.charcterDicionary[text3];
		}
		string text4 = cellData.GetValueOrDefault("characterLadder", "").AsString();
		if (text4 != "" && _owner.charcterDicionary.ContainsKey(text4))
		{
			cellInstance.characterLadder = _owner.charcterDicionary[text4];
		}
		string text5 = cellData.GetValueOrDefault("itemShield", "").AsString();
		if (text5 != "" && _owner.charcterDicionary.TryGetValue(text5, out var value) && value is TowerDefenseItemSheild towerDefenseItemSheild)
		{
			cellInstance.itemShield = towerDefenseItemSheild;
			towerDefenseItemSheild.cell = cellInstance;
		}
	}

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		_destroyed = false;
		_mapRevision = 0L;
		_lastAppliedMapRevision = -1L;
		_lastUnresolvedMapRevision = -1L;
		_lastUnresolvedMapIdentity = "";
		_switchReturnConfig = null;
		_switchResidenceConfig = null;
		_switchReturnDuration = 2.0;
		_gridReady = false;
		_lastInputPhysicsFrame = 18446744073709551615uL;
		_hasCachedInputGridPosition = false;
		_committedMapId = "";
		_committedMapPath = "";
		_syncLineUseSnapshot = new Godot.Collections.Array();
		_syncLineUseDirty = true;
		_plantGridRuntimeCache = System.Array.Empty<TowerDefenseCellInstance[]>();
		string text = data.GetValueOrDefault("MapName", "").AsString();
		mapConfig = TowerDefenseManager.Instance.GetMapConfig(text);
		if (!GodotObject.IsInstanceValid(mapConfig))
		{
			throw new InvalidOperationException("Map config is not registered: '" + text + "'.");
		}
		if (!EnsureGridStorage(mapConfig, out var reason))
		{
			throw new InvalidOperationException("Map grid initialization failed: " + reason);
		}
		PackedScene tOWER_DEFENSE_MAP_CONTROL = TOWER_DEFENSE_MAP_CONTROL;
		if (!GodotObject.IsInstanceValid(tOWER_DEFENSE_MAP_CONTROL))
		{
			throw new InvalidOperationException("Map control scene is unavailable.");
		}
		mapControl = tOWER_DEFENSE_MAP_CONTROL.Instantiate<TowerDefenseMapControl>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(mapControl))
		{
			throw new InvalidOperationException("Map control scene could not be instantiated.");
		}
		control.AddNode(mapControl, 0);
		mapControl.mapFeature = this;
	}

	public override Task GameInit()
	{
		mapControl.SetCharacterCanvasModulate(control.characterCanvasModulate);
		if (!MapInit(mapConfig))
		{
			throw new InvalidOperationException("Map failed to initialize for a new battle.");
		}
		if (stripeRow >= 0 && GodotObject.IsInstanceValid(currentMap))
		{
			currentMap.UseStripe(stripeRow);
		}
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		mapControl.SetCharacterCanvasModulate(control.characterCanvasModulate);
		if (!MapInit(mapConfig))
		{
			throw new InvalidOperationException("Map failed to initialize before progress restore.");
		}
		return Task.CompletedTask;
	}

	public bool TryEnqueuePendingMapAction(Action action)
	{
		if (_destroyed || action == null)
		{
			return false;
		}
		if (_pendingMapActions.Count >= 512)
		{
			GD.PushWarning($"[Map] Pending action limit reached ({512}); action rejected.");
			return false;
		}
		_pendingMapActions.Enqueue(action);
		SchedulePendingMapActionDrain();
		return true;
	}

	private bool CanDrainPendingMapActions()
	{
		if (!_destroyed && _activeMapChange == null && GodotObject.IsInstanceValid(mapControl))
		{
			return GodotObject.IsInstanceValid(currentMap);
		}
		return false;
	}

	private void SchedulePendingMapActionDrain()
	{
		if (!_mapActionDrainScheduled && !_isDrainingMapActions && CanDrainPendingMapActions())
		{
			_mapActionDrainScheduled = true;
			Callable.From(() =>
			{
				_mapActionDrainScheduled = false;
				DrainPendingMapActions();
			}).CallDeferred();
		}
	}

	private void DrainPendingMapActions()
	{
		if (_isDrainingMapActions || !CanDrainPendingMapActions() || _pendingMapActions.Count == 0)
		{
			return;
		}
		_isDrainingMapActions = true;
		int num = Math.Min(_pendingMapActions.Count, 64);
		int i = 0;
		try
		{
			for (; i < num; i++)
			{
				if (_pendingMapActions.Count <= 0)
				{
					break;
				}
				if (!CanDrainPendingMapActions())
				{
					break;
				}
				Action action = _pendingMapActions.Dequeue();
				try
				{
					action();
				}
				catch (Exception value)
				{
					GD.PushError($"[Map] Pending action failed: {value}");
				}
			}
		}
		finally
		{
			_isDrainingMapActions = false;
		}
		if (_pendingMapActions.Count > 0)
		{
			SchedulePendingMapActionDrain();
		}
	}

	public override void Process(double _delta)
	{
		if (GodotObject.IsInstanceValid(mapControl))
		{
			mapControl.UpdateCanvasModulate(_delta);
			UpdateSwitchTimer(_delta);
			ProcessInput();
		}
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary
		{
			["map_revision"] = _mapRevision,
			["map_id"] = _committedMapId,
			["map_path"] = _committedMapPath,
			["is_switch"] = isSwitch,
			["is_swich"] = isSwitch,
			["switch_timer"] = (double.IsFinite(switchTimer) ? Mathf.Snapped(switchTimer, 0.1) : 0.0),
			["line_use"] = GetSyncLineUseSnapshot()
		};
	}

	private Godot.Collections.Array GetSyncLineUseSnapshot()
	{
		if (!_syncLineUseDirty)
		{
			return _syncLineUseSnapshot;
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		if (GodotObject.IsInstanceValid(config))
		{
			for (int i = 1; i <= config.gridNum.Y; i++)
			{
				array.Add(i < lineUse.Count && lineUse[i]);
			}
		}
		_syncLineUseSnapshot = array;
		_syncLineUseDirty = false;
		return _syncLineUseSnapshot;
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (IsRemoteMultiplayerHost())
		{
			return;
		}
		long num = _data.GetValueOrDefault("map_revision", -1L).AsInt64();
		if ((num >= 0 && num < _lastAppliedMapRevision) || (num < 0 && _lastAppliedMapRevision >= 0))
		{
			return;
		}
		isSwitch = _data.GetValueOrDefault("is_switch", _data.GetValueOrDefault("is_swich", false)).AsBool();
		switchTimer = _data.GetValueOrDefault("switch_timer", 0.0).AsDouble();
		if (num > _lastAppliedMapRevision)
		{
			string text = _data.GetValueOrDefault("map_id", "").AsString();
			string text2 = _data.GetValueOrDefault("map_path", "").AsString();
			TowerDefenseMapConfig towerDefenseMapConfig = ResolveMapConfigIdentity(text, text2);
			if (GodotObject.IsInstanceValid(towerDefenseMapConfig))
			{
				_lastUnresolvedMapRevision = -1L;
				_lastUnresolvedMapIdentity = "";
				if (_activeMapChange != null)
				{
					CancelActiveMapChange();
				}
				if (!GodotObject.IsInstanceValid(currentMap) || !IsSameMapConfig(config, towerDefenseMapConfig))
				{
					MapChange(towerDefenseMapConfig);
				}
				if (GodotObject.IsInstanceValid(currentMap) && IsSameMapConfig(config, towerDefenseMapConfig))
				{
					_mapRevision = num;
					_lastAppliedMapRevision = num;
				}
			}
			else if (!string.IsNullOrWhiteSpace(text) || !string.IsNullOrWhiteSpace(text2))
			{
				ReportUnresolvedSyncedMap(num, text, text2);
				return;
			}
		}
		if (_data.ContainsKey("line_use") && GodotObject.IsInstanceValid(config))
		{
			Godot.Collections.Array array = _data["line_use"].AsGodotArray();
			for (int i = 0; i < Mathf.Min(array.Count, config.gridNum.Y); i++)
			{
				TowerDefenseManager.Instance.SetMapLineUse(i + 1, array[i].AsBool());
			}
		}
	}

	public bool MapInit(TowerDefenseMapConfig _config)
	{
		if (GodotObject.IsInstanceValid(currentMap) && (!GodotObject.IsInstanceValid(config) || !IsSameMapConfig(config, _config)))
		{
			GD.PushError("MapInit only initializes the committed Map; use MapChange for runtime transitions.");
			return false;
		}
		if (!EnsureGridStorage(_config, out var reason))
		{
			GD.PushError("MapInit failed: " + reason);
			return false;
		}
		config = _config;
		if (!GodotObject.IsInstanceValid(firstConfig))
		{
			firstConfig = _config;
		}
		UpdateIsPlantColumn();
		PlantGridInit();
		MapChange(_config);
		if (!editorPreviewMode && (!Global.IsEditor || !(SceneManager.CurrentScene == "LevelEditorStage")) && !GodotObject.IsInstanceValid(currentMap))
		{
			_gridReady = false;
			GD.PushError("MapInit failed to instantiate map scene: " + _config.mapScenePath);
			return false;
		}
		ApplyCommittedMapState(_config);
		return true;
	}

	private bool EnsureGridStorage(TowerDefenseMapConfig targetConfig, out string reason)
	{
		if (!GodotObject.IsInstanceValid(targetConfig))
		{
			reason = "map config is null or freed";
			return false;
		}
		if (!targetConfig.TryValidateRuntime(out reason))
		{
			return false;
		}
		int num = targetConfig.gridNum.X + 1;
		int num2 = targetConfig.gridNum.Y + 1;
		if (HasGridStorageSize(num, num2))
		{
			EnsureIceCapRuntimeCache(num2);
			reason = "";
			return true;
		}
		ClearIceCapNodes();
		_warnedInvalidIceCapLines.Clear();
		_gridReady = false;
		_plantGridRuntimeCache = System.Array.Empty<TowerDefenseCellInstance[]>();
		plantGrid.Clear();
		for (int i = 0; i < num; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(num2);
			plantGrid.Add(array);
		}
		lineUse.Clear();
		for (int j = 0; j < num2; j++)
		{
			lineUse.Add(item: false);
		}
		iceCapList.Clear();
		iceCapList.Resize(num2);
		_iceCapRuntimeCache = new TowerDefenseIceCap[num2];
		_syncLineUseDirty = true;
		reason = "";
		return true;
	}

	private void EnsureIceCapRuntimeCache(int rowCount)
	{
		if (_iceCapRuntimeCache.Length != rowCount)
		{
			_iceCapRuntimeCache = new TowerDefenseIceCap[rowCount];
		}
	}

	private bool HasGridStorageSize(int columnCount, int rowCount)
	{
		if (plantGrid.Count != columnCount || lineUse.Count != rowCount || iceCapList.Count != rowCount)
		{
			return false;
		}
		for (int i = 0; i < plantGrid.Count; i++)
		{
			if (plantGrid[i] == null || plantGrid[i].Count != rowCount)
			{
				return false;
			}
		}
		return true;
	}

	private void ClearIceCapNodes()
	{
		foreach (Variant iceCap in iceCapList)
		{
			Node node = iceCap.AsGodotObject() as Node;
			if (GodotObject.IsInstanceValid(node) && !node.IsQueuedForDeletion())
			{
				node.QueueFree();
			}
		}
		System.Array.Clear(_iceCapRuntimeCache, 0, _iceCapRuntimeCache.Length);
	}

	public void UpdateIsPlantColumn()
	{
		isPlantColumn = false;
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (GodotObject.IsInstanceValid(currentControl))
		{
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			TowerDefenseBattleFeatureSeedBank towerDefenseBattleFeatureSeedBank = (GodotObject.IsInstanceValid(instance) ? instance.GetSeedBankFeature() : null);
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureSeedBank) && GodotObject.IsInstanceValid(towerDefenseBattleFeatureSeedBank.config))
			{
				isPlantColumn = towerDefenseBattleFeatureSeedBank.config.plantColumn;
			}
			else if (currentControl.levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
			{
				isPlantColumn = towerDefenseLevelConfig.plantColumn;
			}
		}
	}

	public double GetGroundHeight(TowerDefenseCellInstance _cell)
	{
		if (GodotObject.IsInstanceValid(_cell.groundHeightCurve))
		{
			return _cell.groundHeightCurve.Curve.Sample(0.5f) * mapControl.GlobalScale.Y;
		}
		return 0.0;
	}

	public void PlantGridInit()
	{
		if (!EnsureGridStorage(config, out var reason))
		{
			GD.PushError("PlantGridInit failed: " + reason);
			return;
		}
		EnsurePlantGridRuntimeCache(config.gridNum.X + 1, config.gridNum.Y + 1);
		for (int i = 1; i <= config.gridNum.X; i++)
		{
			for (int j = 1; j <= config.gridNum.Y; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance();
				towerDefenseCellInstance.gridPos = new Vector2I(i, j);
				plantGrid[i][j] = towerDefenseCellInstance;
				_plantGridRuntimeCache[i][j] = towerDefenseCellInstance;
			}
		}
		for (int k = 0; k < lineUse.Count; k++)
		{
			lineUse[k] = false;
		}
		if (config.cellConfig != null)
		{
			foreach (TowerDefenseCellConfig item in config.cellConfig)
			{
				SetGridType(item);
			}
		}
		if (config.lineUse != null)
		{
			foreach (int item2 in config.lineUse)
			{
				SetLineUse(item2, use: true);
			}
		}
		_syncLineUseDirty = true;
		_gridReady = true;
		PlantGridRevision = ((PlantGridRevision == 18446744073709551615uL) ? 1 : (PlantGridRevision + 1));
	}

	private void EnsurePlantGridRuntimeCache(int columnCount, int rowCount)
	{
		if (_plantGridRuntimeCache.Length != columnCount)
		{
			_plantGridRuntimeCache = new TowerDefenseCellInstance[columnCount][];
		}
		for (int i = 0; i < columnCount; i++)
		{
			if (_plantGridRuntimeCache[i] == null || _plantGridRuntimeCache[i].Length != rowCount)
			{
				_plantGridRuntimeCache[i] = new TowerDefenseCellInstance[rowCount];
			}
		}
	}

	public TowerDefenseCellInstance GetPlantGridCell(Vector2I gridPos)
	{
		if (gridPos.X <= 0 || gridPos.X >= _plantGridRuntimeCache.Length)
		{
			return null;
		}
		TowerDefenseCellInstance[] array = _plantGridRuntimeCache[gridPos.X];
		if (array == null || gridPos.Y <= 0 || gridPos.Y >= array.Length)
		{
			return null;
		}
		return array[gridPos.Y];
	}

	private void ApplySavedLineUse(Godot.Collections.Array savedLineUse)
	{
		for (int i = 0; i < lineUse.Count; i++)
		{
			lineUse[i] = false;
		}
		bool flag = GodotObject.IsInstanceValid(config) && savedLineUse.Count == config.gridNum.Y;
		for (int j = 1; j < lineUse.Count; j++)
		{
			int num = (flag ? (j - 1) : j);
			if (num >= 0 && num < savedLineUse.Count)
			{
				lineUse[j] = savedLineUse[num].AsBool();
			}
		}
		_syncLineUseDirty = true;
	}

	public void ProcessInput()
	{
		bool flag = editorPreviewMode || (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage");
		if (!_gridReady || !GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(mapControl) || (!flag && !GodotObject.IsInstanceValid(currentMap)))
		{
			return;
		}
		TowerDefenseMowerManager towerDefenseMowerManager = null;
		if (TowerDefenseManager.Instance.IsGameRunning())
		{
			TowerDefenseBattleFeatureMower mowerFeature = TowerDefenseManager.Instance.GetMowerFeature();
			if (GodotObject.IsInstanceValid(mowerFeature) && GodotObject.IsInstanceValid(mowerFeature.mowerManager))
			{
				towerDefenseMowerManager = mowerFeature.mowerManager;
			}
		}
		bool flag2 = GodotObject.IsInstanceValid(towerDefenseMowerManager) && towerDefenseMowerManager.NeedsInputProcessing();
		bool flag3 = GodotObject.IsInstanceValid(packetPickControl) && packetPickControl.NeedsInputProcessing();
		if (!flag2 && !flag3)
		{
			return;
		}
		ulong physicsFrames = Engine.GetPhysicsFrames();
		if (_lastInputPhysicsFrame != physicsFrames)
		{
			_lastInputPhysicsFrame = physicsFrames;
			Vector2 mousePos = ResolveViewportInputPosition(mapControl.GetViewport().GetMousePosition());
			Vector2I gridPos = ResolveInputGridPosition(mousePos);
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
			if (flag2)
			{
				towerDefenseMowerManager.ProcessMowerInput(mapCell, gridPos);
			}
			if ((!flag3 || packetPickControl.packetPick == null || !packetPickControl.ProcessPacketPick(mapCell, gridPos, mousePos)) && flag3)
			{
				packetPickControl.ProcessTools(mapCell, gridPos, mousePos);
				packetPickControl.ProcessReleaseInput(mousePos);
			}
		}
	}

	internal Vector2 ResolveViewportInputPosition(Vector2 viewportPosition)
	{
		return mapControl.GetCanvasTransform().AffineInverse() * viewportPosition;
	}

	internal Vector2I ResolveInputGridPosition(Vector2 mousePos)
	{
		if (_hasCachedInputGridPosition && mousePos == _cachedInputMousePosition)
		{
			return _cachedInputGridPosition;
		}
		_cachedInputMousePosition = mousePos;
		_cachedInputGridPosition = TowerDefenseManager.Instance.GetMapGridPosFromMouse(mousePos);
		_hasCachedInputGridPosition = true;
		return _cachedInputGridPosition;
	}

	public void NotifyMapTransformChanged()
	{
		_lastInputPhysicsFrame = 18446744073709551615uL;
		_hasCachedInputGridPosition = false;
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseManager.Instance.MapIsChange();
		}
		AdobeAnimateRuntimeManager.NotifyRenderAncestorTransformChanged();
	}

	public void MapChange(TowerDefenseMapConfig _config, double duration = 0.0, double delay = 0.0)
	{
		if (!GodotObject.IsInstanceValid(_config))
		{
			GD.PushError("MapChange failed: map config is null or freed.");
			return;
		}
		bool flag = editorPreviewMode || (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage");
		if (_destroyed || !GodotObject.IsInstanceValid(mapControl) || (!flag && !GodotObject.IsInstanceValid(control)))
		{
			return;
		}
		if (!_config.TryValidateRuntime(out var reason))
		{
			GD.PushError("MapChange failed: " + reason);
			return;
		}
		_config.RefreshSpecialRuleRuntimeCache();
		if (GodotObject.IsInstanceValid(config) && !config.IsRuntimeTopologyCompatibleWith(_config, out var reason2))
		{
			GD.PushError("MapChange rejected incompatible runtime topology: " + reason2);
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				TowerDefenseManager.Instance.TipsPlay("地图网格结构不兼容，无法切换", 5.0);
			}
			return;
		}
		duration = NormalizeNonNegativeFinite(duration);
		delay = NormalizeNonNegativeFinite(delay);
		if (flag)
		{
			if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.mapNode))
			{
				CancelActiveMapChange();
				if (!GodotObject.IsInstanceValid(mapControl.editorSprite))
				{
					mapControl.editorSprite = new Sprite2D();
					mapControl.editorSprite.Centered = false;
					mapControl.mapNode.AddChild(mapControl.editorSprite, forceReadableName: false, Node.InternalMode.Disabled);
				}
				Texture2D mapTexture = _config.GetMapTexture(cache: false);
				mapControl.editorSprite.Texture = mapTexture;
				mapControl.editorSprite.Position = _config.mapOffset;
				if (GodotObject.IsInstanceValid(mapTexture) && mapTexture.GetHeight() > 0)
				{
					mapControl.editorSprite.Scale = Vector2.One * (600f / (float)mapTexture.GetHeight());
				}
				bool applyCellEnvironment = !IsSameMapConfig(config, _config);
				config = _config;
				changeConfig = _config;
				ApplyCommittedMapState(_config, applyCellEnvironment);
				NotifyMapChanged();
				SchedulePendingMapActionDrain();
			}
			return;
		}
		bool flag2 = !GodotObject.IsInstanceValid(currentMap) || !IsSameMapConfig(config, _config);
		if (IsActiveMapChangeTarget(_config))
		{
			return;
		}
		PackedScene packedScene = null;
		if (flag2)
		{
			packedScene = _config.GetMapScene(cache: false);
			if (!GodotObject.IsInstanceValid(packedScene) || !packedScene.CanInstantiate())
			{
				GD.PushError("Map scene failed to load: " + _config.mapScenePath);
				_config.ClearLoadedMapResources();
				return;
			}
		}
		CancelActiveMapChange();
		changeConfig = _config;
		if (!flag2)
		{
			config = _config;
			ApplyCommittedMapState(_config, applyCellEnvironment: false);
			NotifyMapChanged();
			SchedulePendingMapActionDrain();
			return;
		}
		MapChangeOperation mapChangeOperation = new MapChangeOperation
		{
			Id = control.BeginPendingBattleOperation(),
			TargetConfig = _config,
			PreviousConfig = config,
			TargetScene = packedScene,
			SourceMap = currentMap
		};
		if (GodotObject.IsInstanceValid(mapChangeOperation.SourceMap))
		{
			mapChangeOperation.SourceMapModulate = mapChangeOperation.SourceMap.Modulate;
			mapChangeOperation.HasSourceMapModulate = true;
		}
		_activeMapChange = mapChangeOperation;
		ExecuteMapChangeAsync(mapChangeOperation, duration, delay);
	}

	private async Task ExecuteMapChangeAsync(MapChangeOperation operation, double duration, double delay)
	{
		_ = 1;
		try
		{
			bool flag = delay > 0.0;
			if (flag)
			{
				flag = !(await WaitForMapChangeDelayAsync(operation, delay));
			}
			if (flag || !IsMapChangeOperationCurrent(operation))
			{
				return;
			}
			operation.CandidateMap = operation.TargetScene.Instantiate(PackedScene.GenEditState.Disabled) as TowerDefenseMap;
			if (!GodotObject.IsInstanceValid(operation.CandidateMap))
			{
				GD.PushError("Map scene is not a TowerDefenseMap: " + operation.TargetConfig.mapScenePath);
			}
			else
			{
				if (!IsMapChangeOperationCurrent(operation))
				{
					return;
				}
				if (GodotObject.IsInstanceValid(operation.SourceMap) && GodotObject.IsInstanceValid(operation.SourceMap.stripe) && operation.SourceMap.stripe.Visible)
				{
					operation.CandidateMap.UseStripe(stripeRow);
				}
				mapControl.changeMapLayer.AddChild(operation.CandidateMap, forceReadableName: false, Node.InternalMode.Disabled);
				if (!IsMapChangeOperationCurrent(operation))
				{
					return;
				}
				operation.Gradient = operation.CandidateMap.canvasModulateGradient?.Duplicate(deep: true) as GradientTexture1D;
				if (!GodotObject.IsInstanceValid(operation.Gradient) || !GodotObject.IsInstanceValid(operation.Gradient.Gradient))
				{
					GD.PushError("Map scene has no valid canvas gradient: " + operation.TargetConfig.mapScenePath);
					return;
				}
				nextMap = operation.CandidateMap;
				currentGradient = operation.Gradient;
				currentGradientPos = 0.0;
				if (GodotObject.IsInstanceValid(mapControl.canvasModulate))
				{
					operation.SourceCanvasColor = mapControl.canvasModulate.Color;
					operation.HasSourceCanvasColor = true;
				}
				if (duration > 0.0 && GodotObject.IsInstanceValid(operation.SourceMap))
				{
					operation.Gradient.Gradient.SetColor(0, operation.SourceCanvasColor);
					operation.Tween = mapControl.CreateTween();
					switchTween = operation.Tween;
					operation.Tween.SetParallel();
					operation.Tween.TweenProperty(operation.CandidateMap, "modulate:a", 1.0, duration).From(0.0);
					operation.Tween.TweenProperty(operation.SourceMap, "modulate:a", 0.0, duration).From(1.0);
					operation.Tween.TweenProperty(this, "currentGradientPos", 1.0, duration).From(0.0);
					isChange = true;
					if (!(await WaitForMapChangeTweenAsync(operation)))
					{
						return;
					}
				}
				if (IsMapChangeOperationCurrent(operation))
				{
					CommitMapChange(operation);
				}
			}
		}
		catch (Exception value)
		{
			GD.PushError($"MapChange failed for {operation.TargetConfig?.mapScenePath}: {value}");
		}
		finally
		{
			FinishMapChangeOperation(operation);
			operation.TargetConfig?.ClearLoadedMapResources();
		}
	}

	private async Task<bool> WaitForMapChangeDelayAsync(MapChangeOperation operation, double delay)
	{
		SceneTree sceneTree = (GodotObject.IsInstanceValid(mapControl) ? mapControl.GetTree() : null);
		if (!GodotObject.IsInstanceValid(sceneTree))
		{
			return false;
		}
		TaskCompletionSource<bool> waiter = new TaskCompletionSource<bool>();
		operation.Waiter = waiter;
		operation.DelayTimer = sceneTree.CreateTimer(delay, processAlways: false);
		operation.DelayFinishedHandler = () =>
		{
			waiter.TrySetResult(result: true);
		};
		operation.DelayTimer.Timeout += operation.DelayFinishedHandler;
		bool flag = await waiter.Task;
		DetachMapChangeWaitSignals(operation);
		if (operation.Waiter == waiter)
		{
			operation.Waiter = null;
		}
		return flag && IsMapChangeOperationCurrent(operation);
	}

	private async Task<bool> WaitForMapChangeTweenAsync(MapChangeOperation operation)
	{
		if (!GodotObject.IsInstanceValid(operation.Tween))
		{
			return false;
		}
		TaskCompletionSource<bool> waiter = new TaskCompletionSource<bool>();
		operation.Waiter = waiter;
		operation.TweenFinishedHandler = () =>
		{
			waiter.TrySetResult(result: true);
		};
		operation.Tween.Finished += operation.TweenFinishedHandler;
		bool flag = await waiter.Task;
		DetachMapChangeWaitSignals(operation);
		if (operation.Waiter == waiter)
		{
			operation.Waiter = null;
		}
		return flag && IsMapChangeOperationCurrent(operation);
	}

	private bool IsMapChangeOperationCurrent(MapChangeOperation operation)
	{
		if (!_destroyed && _activeMapChange == operation && GodotObject.IsInstanceValid(control) && GodotObject.IsInstanceValid(mapControl))
		{
			return control.IsPendingBattleOperationCurrent(operation.Id);
		}
		return false;
	}

	private bool IsActiveMapChangeTarget(TowerDefenseMapConfig targetConfig)
	{
		if (_activeMapChange != null && IsMapChangeOperationCurrent(_activeMapChange))
		{
			return IsSameMapConfig(_activeMapChange.TargetConfig, targetConfig);
		}
		return false;
	}

	private void CommitMapChange(MapChangeOperation operation)
	{
		if (IsMapChangeOperationCurrent(operation) && GodotObject.IsInstanceValid(operation.CandidateMap) && GodotObject.IsInstanceValid(mapControl.mapNode))
		{
			Color color = operation.Gradient.Gradient.Sample(1f);
			if (GodotObject.IsInstanceValid(mapControl.canvasModulate))
			{
				mapControl.canvasModulate.Color = color;
			}
			if (GodotObject.IsInstanceValid(mapControl.canvasModulateCharacter))
			{
				mapControl.canvasModulateCharacter.Color = color;
			}
			operation.CandidateMap.Modulate = new Color(operation.CandidateMap.Modulate);
			operation.CandidateMap.Reparent(mapControl.mapNode);
			currentMap = operation.CandidateMap;
			config = operation.TargetConfig;
			changeConfig = operation.TargetConfig;
			ApplyCommittedMapState(operation.TargetConfig);
			operation.Committed = true;
			AdvanceMapRevision();
			isChange = false;
			currentGradientPos = 1.0;
			NotifyMapChanged();
			if (GodotObject.IsInstanceValid(operation.SourceMap) && operation.SourceMap != currentMap)
			{
				operation.SourceMap.QueueFree();
			}
			if (GodotObject.IsInstanceValid(operation.PreviousConfig) && !IsSameMapConfig(operation.PreviousConfig, operation.TargetConfig))
			{
				operation.PreviousConfig.ClearLoadedMapResources();
			}
		}
	}

	private void ApplyCommittedMapState(TowerDefenseMapConfig targetConfig, bool applyCellEnvironment = true)
	{
		if (!GodotObject.IsInstanceValid(targetConfig))
		{
			return;
		}
		ApplyMapFrameRateLimit(targetConfig);
		TowerDefenseBattleFeatureCamera feature = GetFeature<TowerDefenseBattleFeatureCamera>("Camera");
		if (GodotObject.IsInstanceValid(feature?.cameraControl))
		{
			feature.cameraControl.ApplyMapConfig(targetConfig);
		}
		_committedMapId = GetRegisteredMapId(targetConfig);
		_committedMapPath = GetMapConfigPath(targetConfig);
		_lastInputPhysicsFrame = 18446744073709551615uL;
		_hasCachedInputGridPosition = false;
		rect = BuildProjectileBoundaryRect(targetConfig);
		groundRect = new Rect2(targetConfig.gridBeginPos.X, targetConfig.gridBeginPos.Y, targetConfig.gridSize.X * (float)targetConfig.gridNum.X, targetConfig.gridSize.Y * (float)targetConfig.gridNum.Y);
		if (!applyCellEnvironment)
		{
			return;
		}
		for (int i = 1; i <= targetConfig.gridNum.X && i < plantGrid.Count; i++)
		{
			Godot.Collections.Array array = plantGrid[i];
			for (int j = 1; j <= targetConfig.gridNum.Y && j < array.Count; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = array[j].As<TowerDefenseCellInstance>();
				if (GodotObject.IsInstanceValid(towerDefenseCellInstance))
				{
					TowerDefenseCellConfig towerDefenseCellConfig = (towerDefenseCellInstance.config = targetConfig.GetEffectiveCellConfig(i, j));
					towerDefenseCellInstance.elementFlags = (GodotObject.IsInstanceValid(towerDefenseCellConfig) ? towerDefenseCellConfig.ElementFlags : 0);
				}
			}
		}
	}

	internal void ApplyMapFrameRateLimit(TowerDefenseMapConfig targetConfig)
	{
		if (!GodotObject.IsInstanceValid(targetConfig) || editorPreviewMode || (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage"))
		{
			return;
		}
		if (!_mapFrameRateLimitActive)
		{
			_maximumFpsBeforeMap = Engine.MaxFps;
			_mapFrameRateLimitActive = true;
			if (GodotObject.IsInstanceValid(Global.Instance))
			{
				Global.Instance.OnAnimeFrameRateChange += RefreshMapFrameRateLimit;
			}
		}
		_currentMapMaximumFps = targetConfig.maximumFps;
		RefreshMapFrameRateLimit();
	}

	private void RefreshMapFrameRateLimit()
	{
		if (_mapFrameRateLimitActive)
		{
			Engine.MaxFps = ResolveMapMaximumFps(GodotObject.IsInstanceValid(Global.Instance) ? Global.Instance.effectiveAnimeFrameRate : _maximumFpsBeforeMap, _currentMapMaximumFps);
		}
	}

	internal void RestoreMapFrameRateLimit()
	{
		if (_mapFrameRateLimitActive)
		{
			if (GodotObject.IsInstanceValid(Global.Instance))
			{
				Global.Instance.OnAnimeFrameRateChange -= RefreshMapFrameRateLimit;
				Engine.MaxFps = Global.Instance.effectiveAnimeFrameRate;
			}
			else
			{
				Engine.MaxFps = _maximumFpsBeforeMap;
			}
			_currentMapMaximumFps = -1;
			_mapFrameRateLimitActive = false;
		}
	}

	internal static int ResolveMapMaximumFps(int preferredMaximumFps, int mapMaximumFps)
	{
		if (mapMaximumFps == -1)
		{
			return preferredMaximumFps;
		}
		if (preferredMaximumFps <= 0)
		{
			return mapMaximumFps;
		}
		return Math.Min(preferredMaximumFps, mapMaximumFps);
	}

	internal static Rect2 BuildProjectileBoundaryRect(TowerDefenseMapConfig targetConfig)
	{
		if (!GodotObject.IsInstanceValid(targetConfig))
		{
			return default;
		}
		return new Rect2(new Vector2(-100f, 0f), targetConfig.mapSize + new Vector2(200f, 0f));
	}

	private void CancelActiveMapChange()
	{
		MapChangeOperation activeMapChange = _activeMapChange;
		if (activeMapChange != null)
		{
			_activeMapChange = null;
			DetachMapChangeWaitSignals(activeMapChange);
			if (GodotObject.IsInstanceValid(activeMapChange.Tween))
			{
				activeMapChange.Tween.Kill();
			}
			CompleteMapChangeOperation(activeMapChange);
			CleanupMapChangeVisuals(activeMapChange, restoreSource: true);
			activeMapChange.Waiter?.TrySetResult(result: false);
			activeMapChange.Waiter = null;
			SchedulePendingMapActionDrain();
		}
	}

	private void FinishMapChangeOperation(MapChangeOperation operation)
	{
		DetachMapChangeWaitSignals(operation);
		CompleteMapChangeOperation(operation);
		if (_activeMapChange == operation)
		{
			_activeMapChange = null;
			CleanupMapChangeVisuals(operation, !operation.Committed);
			if (!operation.Committed)
			{
				changeConfig = config;
			}
			SchedulePendingMapActionDrain();
		}
	}

	private void CompleteMapChangeOperation(MapChangeOperation operation)
	{
		if (!operation.OperationCompleted)
		{
			operation.OperationCompleted = true;
			if (GodotObject.IsInstanceValid(control))
			{
				control.CompletePendingBattleOperation(operation.Id);
			}
		}
	}

	private void CleanupMapChangeVisuals(MapChangeOperation operation, bool restoreSource)
	{
		TowerDefenseMap candidateMap = operation.CandidateMap;
		if (restoreSource && operation.HasSourceMapModulate && GodotObject.IsInstanceValid(operation.SourceMap))
		{
			operation.SourceMap.Modulate = operation.SourceMapModulate;
		}
		if (restoreSource && operation.HasSourceCanvasColor && GodotObject.IsInstanceValid(mapControl))
		{
			if (GodotObject.IsInstanceValid(mapControl.canvasModulate))
			{
				mapControl.canvasModulate.Color = operation.SourceCanvasColor;
			}
			if (GodotObject.IsInstanceValid(mapControl.canvasModulateCharacter))
			{
				mapControl.canvasModulateCharacter.Color = operation.SourceCanvasColor;
			}
		}
		if (!operation.Committed && GodotObject.IsInstanceValid(candidateMap))
		{
			Node parent = candidateMap.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(candidateMap);
			}
			candidateMap.QueueFree();
			operation.CandidateMap = null;
		}
		if (nextMap == candidateMap || !GodotObject.IsInstanceValid(nextMap))
		{
			nextMap = null;
		}
		if (switchTween == operation.Tween)
		{
			switchTween = null;
		}
		if (currentGradient == operation.Gradient)
		{
			currentGradient = null;
		}
		isChange = false;
		if (!operation.Committed)
		{
			currentGradientPos = 0.0;
		}
	}

	private static void DetachMapChangeWaitSignals(MapChangeOperation operation)
	{
		if (GodotObject.IsInstanceValid(operation.DelayTimer) && operation.DelayFinishedHandler != null)
		{
			operation.DelayTimer.Timeout -= operation.DelayFinishedHandler;
		}
		if (GodotObject.IsInstanceValid(operation.Tween) && operation.TweenFinishedHandler != null)
		{
			operation.Tween.Finished -= operation.TweenFinishedHandler;
		}
		operation.DelayTimer = null;
		operation.DelayFinishedHandler = null;
		operation.TweenFinishedHandler = null;
	}

	private static void NotifyMapChanged()
	{
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseManager.Instance.MapIsChange();
		}
	}

	private void AdvanceMapRevision()
	{
		_mapRevision = ((_mapRevision == 9223372036854775807L) ? 1 : (_mapRevision + 1));
	}

	private static string GetRegisteredMapId(TowerDefenseMapConfig mapConfig)
	{
		if (!GodotObject.IsInstanceValid(mapConfig) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			return "";
		}
		foreach (KeyValuePair<string, Resource> mAP in ResourceManager.Instance.MAPS)
		{
			if (mAP.Value is TowerDefenseMapConfig towerDefenseMapConfig && towerDefenseMapConfig == mapConfig)
			{
				return mAP.Key;
			}
		}
		if (string.IsNullOrWhiteSpace(mapConfig.ResourcePath))
		{
			return "";
		}
		foreach (KeyValuePair<string, Resource> mAP2 in ResourceManager.Instance.MAPS)
		{
			if (mAP2.Value is TowerDefenseMapConfig towerDefenseMapConfig2 && GodotObject.IsInstanceValid(towerDefenseMapConfig2) && string.Equals(towerDefenseMapConfig2.ResourcePath, mapConfig.ResourcePath, StringComparison.Ordinal))
			{
				return mAP2.Key;
			}
		}
		return "";
	}

	private static string GetMapConfigPath(TowerDefenseMapConfig mapConfig)
	{
		if (!GodotObject.IsInstanceValid(mapConfig))
		{
			return "";
		}
		return mapConfig.ResourcePath;
	}

	private static bool HasStableMapIdentity(TowerDefenseMapConfig mapConfig)
	{
		if (GodotObject.IsInstanceValid(mapConfig))
		{
			if (string.IsNullOrWhiteSpace(GetRegisteredMapId(mapConfig)))
			{
				return !string.IsNullOrWhiteSpace(GetMapConfigPath(mapConfig));
			}
			return true;
		}
		return false;
	}

	private static TowerDefenseMapConfig ResolveMapConfigIdentity(string mapId, string mapPath)
	{
		if (!string.IsNullOrWhiteSpace(mapId) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseMapConfig towerDefenseMapConfig = TowerDefenseManager.Instance.GetMapConfig(mapId);
			if (GodotObject.IsInstanceValid(towerDefenseMapConfig))
			{
				return towerDefenseMapConfig;
			}
		}
		if (!string.IsNullOrWhiteSpace(mapPath) && ResourceLoader.Exists(mapPath))
		{
			return GD.Load<TowerDefenseMapConfig>(mapPath);
		}
		return null;
	}

	private void ReportUnresolvedSyncedMap(long revision, string mapId, string mapPath)
	{
		string text = mapId + "\n" + mapPath;
		if (_lastUnresolvedMapRevision != revision || !(_lastUnresolvedMapIdentity == text))
		{
			_lastUnresolvedMapRevision = revision;
			_lastUnresolvedMapIdentity = text;
			GD.PushError("Unable to resolve synchronized map: id=" + mapId + ", path=" + mapPath);
		}
	}

	private static bool IsRemoteMultiplayerClient()
	{
		if (Global.IsMultiplayerMode)
		{
			return !MultiPlayerManager.IsHost;
		}
		return false;
	}

	private static bool IsRemoteMultiplayerHost()
	{
		if (Global.IsMultiplayerMode)
		{
			return MultiPlayerManager.IsHost;
		}
		return false;
	}

	public override void Destroy()
	{
		RestoreMapFrameRateLimit();
		_destroyed = true;
		CancelActiveMapChange();
		StopMapSwitchSchedule();
		_pendingMapActions.Clear();
		_isDrainingMapActions = false;
		_mapActionDrainScheduled = false;
		_gridReady = false;
		_lastInputPhysicsFrame = 18446744073709551615uL;
		_hasCachedInputGridPosition = false;
		_committedMapId = "";
		_committedMapPath = "";
		_syncLineUseSnapshot = new Godot.Collections.Array();
		_syncLineUseDirty = true;
		isChange = false;
		nextMap = null;
		switchTween = null;
		currentGradient = null;
		if (GodotObject.IsInstanceValid(mapControl))
		{
			mapControl.mapFeature = null;
			if (!mapControl.IsQueuedForDeletion())
			{
				mapControl.QueueFree();
			}
		}
		mapControl = null;
		base.Destroy();
	}

	private static bool IsSameMapConfig(TowerDefenseMapConfig left, TowerDefenseMapConfig right)
	{
		if (left == right)
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(left) || !GodotObject.IsInstanceValid(right) || string.IsNullOrEmpty(left.ResourcePath) || string.IsNullOrEmpty(right.ResourcePath))
		{
			return false;
		}
		return string.Equals(left.ResourcePath, right.ResourcePath, StringComparison.Ordinal);
	}

	public void MapDayNightSwitch(double duration = 2.0, double _switchTimer = 100.0, double returnDuration = 2.0)
	{
		if (IsRemoteMultiplayerClient() || _destroyed || !GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance) || string.IsNullOrWhiteSpace(config.dayNightSwitching))
		{
			return;
		}
		TowerDefenseMapConfig towerDefenseMapConfig = TowerDefenseManager.Instance.GetMapConfig(config.dayNightSwitching);
		if (!GodotObject.IsInstanceValid(towerDefenseMapConfig))
		{
			GD.PushError("Map day/night target is not registered: " + config.dayNightSwitching);
		}
		else if (config.gridNum != towerDefenseMapConfig.gridNum)
		{
			TowerDefenseManager.Instance.TipsPlay("格子数量不同的地图无法切换", 5.0);
		}
		else
		{
			if (IsSameMapConfig(config, towerDefenseMapConfig))
			{
				return;
			}
			bool flag = isSwitch && GodotObject.IsInstanceValid(_switchReturnConfig) && IsSameMapConfig(towerDefenseMapConfig, _switchReturnConfig);
			TowerDefenseMapConfig switchReturnConfig = config;
			MapChange(towerDefenseMapConfig, NormalizeNonNegativeFinite(duration));
			if (IsActiveMapChangeTarget(towerDefenseMapConfig) || (GodotObject.IsInstanceValid(currentMap) && IsSameMapConfig(config, towerDefenseMapConfig)))
			{
				if ((_switchTimer == -1.0) | flag)
				{
					StopMapSwitchSchedule();
					return;
				}
				_switchReturnConfig = switchReturnConfig;
				_switchResidenceConfig = towerDefenseMapConfig;
				_switchReturnDuration = NormalizeNonNegativeFinite(returnDuration, 2.0);
				switchTimer = NormalizeNonNegativeFinite(_switchTimer);
				isSwitch = true;
			}
		}
	}

	public void UpdateSwitchTimer(double delta)
	{
		if (IsRemoteMultiplayerClient() || !isSwitch || _activeMapChange != null)
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(_switchReturnConfig) || !GodotObject.IsInstanceValid(_switchResidenceConfig) || !IsSameMapConfig(config, _switchResidenceConfig))
		{
			StopMapSwitchSchedule();
			return;
		}
		switchTimer = Math.Max(0.0, NormalizeNonNegativeFinite(switchTimer) - NormalizeNonNegativeFinite(delta));
		if (!(switchTimer > 0.0))
		{
			TowerDefenseMapConfig switchReturnConfig = _switchReturnConfig;
			double switchReturnDuration = _switchReturnDuration;
			StopMapSwitchSchedule();
			MapChange(switchReturnConfig, switchReturnDuration);
		}
	}

	private void RestoreMapSwitchSchedule(Dictionary savedData)
	{
		_switchReturnDuration = NormalizeNonNegativeFinite(savedData.GetValueOrDefault("switchReturnDuration", 2.0).AsDouble(), 2.0);
		if (!isSwitch)
		{
			_switchReturnConfig = null;
			_switchResidenceConfig = null;
			return;
		}
		_switchReturnConfig = ResolveMapConfigIdentity(savedData.GetValueOrDefault("switchReturnMapId", "").AsString(), savedData.GetValueOrDefault("switchReturnMapPath", "").AsString());
		_switchResidenceConfig = ResolveMapConfigIdentity(savedData.GetValueOrDefault("switchResidenceMapId", "").AsString(), savedData.GetValueOrDefault("switchResidenceMapPath", "").AsString());
		if (!GodotObject.IsInstanceValid(_switchReturnConfig))
		{
			_switchReturnConfig = firstConfig;
		}
		if (!GodotObject.IsInstanceValid(_switchResidenceConfig))
		{
			_switchResidenceConfig = config;
		}
		if (!GodotObject.IsInstanceValid(_switchReturnConfig) || !GodotObject.IsInstanceValid(_switchResidenceConfig) || IsSameMapConfig(_switchReturnConfig, _switchResidenceConfig) || !IsSameMapConfig(config, _switchResidenceConfig))
		{
			StopMapSwitchSchedule();
		}
	}

	private void StopMapSwitchSchedule()
	{
		isSwitch = false;
		switchTimer = 0.0;
		_switchReturnConfig = null;
		_switchResidenceConfig = null;
		_switchReturnDuration = 2.0;
	}

	private static double NormalizeNonNegativeFinite(double value, double fallback = 0.0)
	{
		if (!double.IsFinite(value))
		{
			return Math.Max(0.0, fallback);
		}
		return Math.Max(0.0, value);
	}

	public void SetGridType(TowerDefenseCellConfig cellConfig)
	{
		if (!GodotObject.IsInstanceValid(cellConfig) || !GodotObject.IsInstanceValid(config))
		{
			return;
		}
		int num = Mathf.Max(1, cellConfig.pos.X);
		int num2 = Mathf.Min(config.gridNum.X, cellConfig.pos.Z);
		int num3 = Mathf.Max(1, cellConfig.pos.Y);
		int num4 = Mathf.Min(config.gridNum.Y, cellConfig.pos.W);
		for (int i = num; i <= num2; i++)
		{
			if (i < 0 || i >= plantGrid.Count)
			{
				continue;
			}
			Godot.Collections.Array array = plantGrid[i];
			for (int j = num3; j <= num4; j++)
			{
				if (j >= 0 && j < array.Count)
				{
					TowerDefenseCellInstance towerDefenseCellInstance = array[j].As<TowerDefenseCellInstance>();
					if (GodotObject.IsInstanceValid(towerDefenseCellInstance))
					{
						towerDefenseCellInstance.Init(cellConfig);
					}
				}
			}
		}
		_lastInputPhysicsFrame = 18446744073709551615uL;
		_hasCachedInputGridPosition = false;
	}

	public bool SetLineUse(int line, bool use)
	{
		if (!GodotObject.IsInstanceValid(config) || line < 1 || line > config.gridNum.Y || line >= lineUse.Count)
		{
			GD.PushWarning($"[Map] Ignored invalid line index {line}.");
			return false;
		}
		if (lineUse[line] == use)
		{
			return true;
		}
		lineUse[line] = use;
		_syncLineUseDirty = true;
		return true;
	}

	public bool LineHasType(int line, TowerDefenseEnum.PLANTGRIDTYPE type)
	{
		if (!GodotObject.IsInstanceValid(config) || line < 1 || line > config.gridNum.Y)
		{
			return false;
		}
		if (type == TowerDefenseEnum.PLANTGRIDTYPE.ICECAP)
		{
			if (line >= iceCapList.Count)
			{
				return false;
			}
			TowerDefenseIceCap towerDefenseIceCap = iceCapList[line].AsGodotObject() as TowerDefenseIceCap;
			if (GodotObject.IsInstanceValid(towerDefenseIceCap) && !towerDefenseIceCap.IsQueuedForDeletion())
			{
				return towerDefenseIceCap.length > 0.0;
			}
			return false;
		}
		if (plantGrid.Count == 0)
		{
			return false;
		}
		for (int i = 1; i <= config.gridNum.X && i < plantGrid.Count; i++)
		{
			Godot.Collections.Array array = plantGrid[i];
			if (line >= 0 && line < array.Count)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = array[line].As<TowerDefenseCellInstance>();
				if (GodotObject.IsInstanceValid(towerDefenseCellInstance) && towerDefenseCellInstance.gridType.Contains(type))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool TryGetIceCapFrontX(int line, out float frontX)
	{
		frontX = 0f;
		if (!GodotObject.IsInstanceValid(config) || line < 1 || line > config.gridNum.Y || line >= iceCapList.Count)
		{
			return false;
		}
		TowerDefenseIceCap towerDefenseIceCap = iceCapList[line].AsGodotObject() as TowerDefenseIceCap;
		if (!GodotObject.IsInstanceValid(towerDefenseIceCap) || towerDefenseIceCap.IsQueuedForDeletion() || towerDefenseIceCap.length <= 0.0)
		{
			return false;
		}
		frontX = config.mapSize.X - (float)towerDefenseIceCap.length;
		return true;
	}

	public int GetCellPlantNum()
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			return 0;
		}
		int num = 0;
		for (int i = 1; i <= config.gridNum.X; i++)
		{
			for (int j = 1; j <= config.gridNum.Y; j++)
			{
				if (plantGrid[i][j].As<TowerDefenseCellInstance>().HasPlant())
				{
					num++;
				}
			}
		}
		return num;
	}

	public void SetIceCapPos(int line, Vector2 pos)
	{
		if (!GodotObject.IsInstanceValid(config) || !GodotObject.IsInstanceValid(mapControl) || line < 1 || line > config.gridNum.Y || line >= iceCapList.Count)
		{
			if (_warnedInvalidIceCapLines.Add(line))
			{
				GD.PushWarning($"[Map] Ignored invalid ice-cap line index {line}.");
			}
			return;
		}
		EnsureIceCapRuntimeCache(config.gridNum.Y + 1);
		TowerDefenseIceCap towerDefenseIceCap = _iceCapRuntimeCache[line];
		if (!GodotObject.IsInstanceValid(towerDefenseIceCap))
		{
			towerDefenseIceCap = iceCapList[line].AsGodotObject() as TowerDefenseIceCap;
			if (!GodotObject.IsInstanceValid(towerDefenseIceCap))
			{
				towerDefenseIceCap = TOWER_DEFENSE_ICE_CAP.Instantiate<TowerDefenseIceCap>(PackedScene.GenEditState.Disabled);
				iceCapList[line] = towerDefenseIceCap;
				towerDefenseIceCap.gridPos = new Vector2I(0, line);
			}
			_iceCapRuntimeCache[line] = towerDefenseIceCap;
			towerDefenseIceCap.GlobalPosition = new Vector2(config.mapSize.X, TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, line)).Y);
			if (towerDefenseIceCap.GetParent() == null)
			{
				mapControl.mapIceCap.AddChild(towerDefenseIceCap, forceReadableName: false, Node.InternalMode.Disabled);
			}
		}
		float a = (float)towerDefenseIceCap.length;
		towerDefenseIceCap.length = Mathf.Max(a, config.mapSize.X - pos.X);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(57)
		{
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryApplySavedMapConfigBeforeLoadingCells, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._SaveCell, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cellInstance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._LoadCellInto, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cellInstance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "cellData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "restoreTerrain", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanDrainPendingMapActions, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SchedulePendingMapActionDrain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrainPendingMapActions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSyncLineUseSnapshot, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapInit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureIceCapRuntimeCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "rowCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasGridStorageSize, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "columnCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rowCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearIceCapNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateIsPlantColumn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGroundHeight, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlantGridInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePlantGridRuntimeCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "columnCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rowCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPlantGridCell, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySavedLineUse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "savedLineUse", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveViewportInputPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "viewportPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveInputGridPosition, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyMapTransformChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MapChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsActiveMapChangeTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCommittedMapState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "applyCellEnvironment", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMapFrameRateLimit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMapFrameRateLimit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreMapFrameRateLimit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveMapMaximumFps, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "preferredMaximumFps", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mapMaximumFps", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildProjectileBoundaryRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CancelActiveMapChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyMapChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.AdvanceMapRevision, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRegisteredMapId, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapConfigPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasStableMapIdentity, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveMapConfigIdentity, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mapId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "mapPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReportUnresolvedSyncedMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "revision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "mapId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "mapPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRemoteMultiplayerClient, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsRemoteMultiplayerHost, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSameMapConfig, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MapDayNightSwitch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_switchTimer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "returnDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSwitchTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreMapSwitchSchedule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "savedData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopMapSwitchSchedule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeNonNegativeFinite, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGridType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cellConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetLineUse, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "use", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LineHasType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCellPlantNum, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetIceCapPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplySavedMapConfigBeforeLoadingCells && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryApplySavedMapConfigBeforeLoadingCells(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName._SaveCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(_SaveCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName._LoadCellInto && args.Count == 4)
		{
			_LoadCellInto(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanDrainPendingMapActions && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDrainPendingMapActions());
			return true;
		}
		if (method == MethodName.SchedulePendingMapActionDrain && args.Count == 0)
		{
			SchedulePendingMapActionDrain();
			ret = default;
			return true;
		}
		if (method == MethodName.DrainPendingMapActions && args.Count == 0)
		{
			DrainPendingMapActions();
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.GetSyncLineUseSnapshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetSyncLineUseSnapshot());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MapInit && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MapInit(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureIceCapRuntimeCache && args.Count == 1)
		{
			EnsureIceCapRuntimeCache(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasGridStorageSize && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasGridStorageSize(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearIceCapNodes && args.Count == 0)
		{
			ClearIceCapNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateIsPlantColumn && args.Count == 0)
		{
			UpdateIsPlantColumn();
			ret = default;
			return true;
		}
		if (method == MethodName.GetGroundHeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetGroundHeight(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.PlantGridInit && args.Count == 0)
		{
			PlantGridInit();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePlantGridRuntimeCache && args.Count == 2)
		{
			EnsurePlantGridRuntimeCache(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPlantGridCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCellInstance>(GetPlantGridCell(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplySavedLineUse && args.Count == 1)
		{
			ApplySavedLineUse(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessInput && args.Count == 0)
		{
			ProcessInput();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveViewportInputPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolveViewportInputPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveInputGridPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveInputGridPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.NotifyMapTransformChanged && args.Count == 0)
		{
			NotifyMapTransformChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.MapChange && args.Count == 3)
		{
			MapChange(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsActiveMapChangeTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsActiveMapChangeTarget(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyCommittedMapState && args.Count == 2)
		{
			ApplyCommittedMapState(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMapFrameRateLimit && args.Count == 1)
		{
			ApplyMapFrameRateLimit(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMapFrameRateLimit && args.Count == 0)
		{
			RefreshMapFrameRateLimit();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreMapFrameRateLimit && args.Count == 0)
		{
			RestoreMapFrameRateLimit();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveMapMaximumFps && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveMapMaximumFps(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildProjectileBoundaryRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(BuildProjectileBoundaryRect(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelActiveMapChange && args.Count == 0)
		{
			CancelActiveMapChange();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyMapChanged && args.Count == 0)
		{
			NotifyMapChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceMapRevision && args.Count == 0)
		{
			AdvanceMapRevision();
			ret = default;
			return true;
		}
		if (method == MethodName.GetRegisteredMapId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetRegisteredMapId(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapConfigPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetMapConfigPath(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.HasStableMapIdentity && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasStableMapIdentity(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveMapConfigIdentity && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(ResolveMapConfigIdentity(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReportUnresolvedSyncedMap && args.Count == 3)
		{
			ReportUnresolvedSyncedMap(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRemoteMultiplayerClient && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteMultiplayerClient());
			return true;
		}
		if (method == MethodName.IsRemoteMultiplayerHost && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteMultiplayerHost());
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.IsSameMapConfig && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameMapConfig(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.MapDayNightSwitch && args.Count == 3)
		{
			MapDayNightSwitch(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSwitchTimer && args.Count == 1)
		{
			UpdateSwitchTimer(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreMapSwitchSchedule && args.Count == 1)
		{
			RestoreMapSwitchSchedule(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StopMapSwitchSchedule && args.Count == 0)
		{
			StopMapSwitchSchedule();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeNonNegativeFinite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(NormalizeNonNegativeFinite(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.SetGridType && args.Count == 1)
		{
			SetGridType(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLineUse && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetLineUse(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.LineHasType && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(LineHasType(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.PLANTGRIDTYPE>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCellPlantNum && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCellPlantNum());
			return true;
		}
		if (method == MethodName.SetIceCapPos && args.Count == 2)
		{
			SetIceCapPos(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveMapMaximumFps && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveMapMaximumFps(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildProjectileBoundaryRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(BuildProjectileBoundaryRect(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.NotifyMapChanged && args.Count == 0)
		{
			NotifyMapChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.GetRegisteredMapId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetRegisteredMapId(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapConfigPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetMapConfigPath(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.HasStableMapIdentity && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasStableMapIdentity(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveMapConfigIdentity && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(ResolveMapConfigIdentity(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsRemoteMultiplayerClient && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteMultiplayerClient());
			return true;
		}
		if (method == MethodName.IsRemoteMultiplayerHost && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRemoteMultiplayerHost());
			return true;
		}
		if (method == MethodName.IsSameMapConfig && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameMapConfig(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeNonNegativeFinite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(NormalizeNonNegativeFinite(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.TryApplySavedMapConfigBeforeLoadingCells)
		{
			return true;
		}
		if (method == MethodName._SaveCell)
		{
			return true;
		}
		if (method == MethodName._LoadCellInto)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.CanDrainPendingMapActions)
		{
			return true;
		}
		if (method == MethodName.SchedulePendingMapActionDrain)
		{
			return true;
		}
		if (method == MethodName.DrainPendingMapActions)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.GetSyncLineUseSnapshot)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.MapInit)
		{
			return true;
		}
		if (method == MethodName.EnsureIceCapRuntimeCache)
		{
			return true;
		}
		if (method == MethodName.HasGridStorageSize)
		{
			return true;
		}
		if (method == MethodName.ClearIceCapNodes)
		{
			return true;
		}
		if (method == MethodName.UpdateIsPlantColumn)
		{
			return true;
		}
		if (method == MethodName.GetGroundHeight)
		{
			return true;
		}
		if (method == MethodName.PlantGridInit)
		{
			return true;
		}
		if (method == MethodName.EnsurePlantGridRuntimeCache)
		{
			return true;
		}
		if (method == MethodName.GetPlantGridCell)
		{
			return true;
		}
		if (method == MethodName.ApplySavedLineUse)
		{
			return true;
		}
		if (method == MethodName.ProcessInput)
		{
			return true;
		}
		if (method == MethodName.ResolveViewportInputPosition)
		{
			return true;
		}
		if (method == MethodName.ResolveInputGridPosition)
		{
			return true;
		}
		if (method == MethodName.NotifyMapTransformChanged)
		{
			return true;
		}
		if (method == MethodName.MapChange)
		{
			return true;
		}
		if (method == MethodName.IsActiveMapChangeTarget)
		{
			return true;
		}
		if (method == MethodName.ApplyCommittedMapState)
		{
			return true;
		}
		if (method == MethodName.ApplyMapFrameRateLimit)
		{
			return true;
		}
		if (method == MethodName.RefreshMapFrameRateLimit)
		{
			return true;
		}
		if (method == MethodName.RestoreMapFrameRateLimit)
		{
			return true;
		}
		if (method == MethodName.ResolveMapMaximumFps)
		{
			return true;
		}
		if (method == MethodName.BuildProjectileBoundaryRect)
		{
			return true;
		}
		if (method == MethodName.CancelActiveMapChange)
		{
			return true;
		}
		if (method == MethodName.NotifyMapChanged)
		{
			return true;
		}
		if (method == MethodName.AdvanceMapRevision)
		{
			return true;
		}
		if (method == MethodName.GetRegisteredMapId)
		{
			return true;
		}
		if (method == MethodName.GetMapConfigPath)
		{
			return true;
		}
		if (method == MethodName.HasStableMapIdentity)
		{
			return true;
		}
		if (method == MethodName.ResolveMapConfigIdentity)
		{
			return true;
		}
		if (method == MethodName.ReportUnresolvedSyncedMap)
		{
			return true;
		}
		if (method == MethodName.IsRemoteMultiplayerClient)
		{
			return true;
		}
		if (method == MethodName.IsRemoteMultiplayerHost)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.IsSameMapConfig)
		{
			return true;
		}
		if (method == MethodName.MapDayNightSwitch)
		{
			return true;
		}
		if (method == MethodName.UpdateSwitchTimer)
		{
			return true;
		}
		if (method == MethodName.RestoreMapSwitchSchedule)
		{
			return true;
		}
		if (method == MethodName.StopMapSwitchSchedule)
		{
			return true;
		}
		if (method == MethodName.NormalizeNonNegativeFinite)
		{
			return true;
		}
		if (method == MethodName.SetGridType)
		{
			return true;
		}
		if (method == MethodName.SetLineUse)
		{
			return true;
		}
		if (method == MethodName.LineHasType)
		{
			return true;
		}
		if (method == MethodName.GetCellPlantNum)
		{
			return true;
		}
		if (method == MethodName.SetIceCapPos)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.PlantGridRevision)
		{
			PlantGridRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.mapControl)
		{
			mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName.mapConfig)
		{
			mapConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			editorPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName.firstConfig)
		{
			firstConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName.changeConfig)
		{
			changeConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName.isSwitch)
		{
			isSwitch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.switchTimer)
		{
			switchTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.switchTween)
		{
			switchTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.stripeRow)
		{
			stripeRow = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.lineUse)
		{
			lineUse = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName.plantGrid)
		{
			plantGrid = VariantUtils.ConvertToArray<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.iceCapList)
		{
			iceCapList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.currentMap)
		{
			currentMap = VariantUtils.ConvertTo<TowerDefenseMap>(in value);
			return true;
		}
		if (name == PropertyName.nextMap)
		{
			nextMap = VariantUtils.ConvertTo<TowerDefenseMap>(in value);
			return true;
		}
		if (name == PropertyName.isChange)
		{
			isChange = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentGradientPos)
		{
			currentGradientPos = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentGradient)
		{
			currentGradient = VariantUtils.ConvertTo<GradientTexture1D>(in value);
			return true;
		}
		if (name == PropertyName.rect)
		{
			rect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.groundRect)
		{
			groundRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.isPlantColumn)
		{
			isPlantColumn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shovelManager)
		{
			shovelManager = VariantUtils.ConvertTo<ShovelManager>(in value);
			return true;
		}
		if (name == PropertyName.gloveManager)
		{
			gloveManager = VariantUtils.ConvertTo<GloveManager>(in value);
			return true;
		}
		if (name == PropertyName.packetPickControl)
		{
			packetPickControl = VariantUtils.ConvertTo<PacketPickControl>(in value);
			return true;
		}
		if (name == PropertyName._destroyed)
		{
			_destroyed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mapRevision)
		{
			_mapRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastAppliedMapRevision)
		{
			_lastAppliedMapRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastUnresolvedMapRevision)
		{
			_lastUnresolvedMapRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastUnresolvedMapIdentity)
		{
			_lastUnresolvedMapIdentity = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._switchReturnConfig)
		{
			_switchReturnConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName._switchResidenceConfig)
		{
			_switchResidenceConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName._switchReturnDuration)
		{
			_switchReturnDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._isDrainingMapActions)
		{
			_isDrainingMapActions = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mapActionDrainScheduled)
		{
			_mapActionDrainScheduled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gridReady)
		{
			_gridReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastInputPhysicsFrame)
		{
			_lastInputPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._hasCachedInputGridPosition)
		{
			_hasCachedInputGridPosition = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedInputMousePosition)
		{
			_cachedInputMousePosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._cachedInputGridPosition)
		{
			_cachedInputGridPosition = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._committedMapId)
		{
			_committedMapId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._committedMapPath)
		{
			_committedMapPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._maximumFpsBeforeMap)
		{
			_maximumFpsBeforeMap = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._currentMapMaximumFps)
		{
			_currentMapMaximumFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._mapFrameRateLimitActive)
		{
			_mapFrameRateLimitActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._syncLineUseSnapshot)
		{
			_syncLineUseSnapshot = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName._syncLineUseDirty)
		{
			_syncLineUseDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._iceCapRuntimeCache)
		{
			_iceCapRuntimeCache = VariantUtils.ConvertToSystemArrayOfGodotObject<TowerDefenseIceCap>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.PlantGridRevision)
		{
			value = VariantUtils.CreateFrom<ulong>(PlantGridRevision);
			return true;
		}
		if (name == PropertyName.mapControl)
		{
			value = VariantUtils.CreateFrom(in mapControl);
			return true;
		}
		if (name == PropertyName.mapConfig)
		{
			value = VariantUtils.CreateFrom(in mapConfig);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			value = VariantUtils.CreateFrom(in editorPreviewMode);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.firstConfig)
		{
			value = VariantUtils.CreateFrom(in firstConfig);
			return true;
		}
		if (name == PropertyName.changeConfig)
		{
			value = VariantUtils.CreateFrom(in changeConfig);
			return true;
		}
		if (name == PropertyName.isSwitch)
		{
			value = VariantUtils.CreateFrom(in isSwitch);
			return true;
		}
		if (name == PropertyName.switchTimer)
		{
			value = VariantUtils.CreateFrom(in switchTimer);
			return true;
		}
		if (name == PropertyName.switchTween)
		{
			value = VariantUtils.CreateFrom(in switchTween);
			return true;
		}
		if (name == PropertyName.stripeRow)
		{
			value = VariantUtils.CreateFrom(in stripeRow);
			return true;
		}
		if (name == PropertyName.lineUse)
		{
			value = VariantUtils.CreateFromArray(lineUse);
			return true;
		}
		if (name == PropertyName.plantGrid)
		{
			value = VariantUtils.CreateFromArray(plantGrid);
			return true;
		}
		if (name == PropertyName.iceCapList)
		{
			value = VariantUtils.CreateFrom(in iceCapList);
			return true;
		}
		if (name == PropertyName.currentMap)
		{
			value = VariantUtils.CreateFrom(in currentMap);
			return true;
		}
		if (name == PropertyName.nextMap)
		{
			value = VariantUtils.CreateFrom(in nextMap);
			return true;
		}
		if (name == PropertyName.isChange)
		{
			value = VariantUtils.CreateFrom(in isChange);
			return true;
		}
		if (name == PropertyName.currentGradientPos)
		{
			value = VariantUtils.CreateFrom(in currentGradientPos);
			return true;
		}
		if (name == PropertyName.currentGradient)
		{
			value = VariantUtils.CreateFrom(in currentGradient);
			return true;
		}
		if (name == PropertyName.rect)
		{
			value = VariantUtils.CreateFrom(in rect);
			return true;
		}
		if (name == PropertyName.groundRect)
		{
			value = VariantUtils.CreateFrom(in groundRect);
			return true;
		}
		if (name == PropertyName.isPlantColumn)
		{
			value = VariantUtils.CreateFrom(in isPlantColumn);
			return true;
		}
		if (name == PropertyName.shovelManager)
		{
			value = VariantUtils.CreateFrom(in shovelManager);
			return true;
		}
		if (name == PropertyName.gloveManager)
		{
			value = VariantUtils.CreateFrom(in gloveManager);
			return true;
		}
		if (name == PropertyName.packetPickControl)
		{
			value = VariantUtils.CreateFrom(in packetPickControl);
			return true;
		}
		if (name == PropertyName._destroyed)
		{
			value = VariantUtils.CreateFrom(in _destroyed);
			return true;
		}
		if (name == PropertyName._mapRevision)
		{
			value = VariantUtils.CreateFrom(in _mapRevision);
			return true;
		}
		if (name == PropertyName._lastAppliedMapRevision)
		{
			value = VariantUtils.CreateFrom(in _lastAppliedMapRevision);
			return true;
		}
		if (name == PropertyName._lastUnresolvedMapRevision)
		{
			value = VariantUtils.CreateFrom(in _lastUnresolvedMapRevision);
			return true;
		}
		if (name == PropertyName._lastUnresolvedMapIdentity)
		{
			value = VariantUtils.CreateFrom(in _lastUnresolvedMapIdentity);
			return true;
		}
		if (name == PropertyName._switchReturnConfig)
		{
			value = VariantUtils.CreateFrom(in _switchReturnConfig);
			return true;
		}
		if (name == PropertyName._switchResidenceConfig)
		{
			value = VariantUtils.CreateFrom(in _switchResidenceConfig);
			return true;
		}
		if (name == PropertyName._switchReturnDuration)
		{
			value = VariantUtils.CreateFrom(in _switchReturnDuration);
			return true;
		}
		if (name == PropertyName._isDrainingMapActions)
		{
			value = VariantUtils.CreateFrom(in _isDrainingMapActions);
			return true;
		}
		if (name == PropertyName._mapActionDrainScheduled)
		{
			value = VariantUtils.CreateFrom(in _mapActionDrainScheduled);
			return true;
		}
		if (name == PropertyName._gridReady)
		{
			value = VariantUtils.CreateFrom(in _gridReady);
			return true;
		}
		if (name == PropertyName._lastInputPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _lastInputPhysicsFrame);
			return true;
		}
		if (name == PropertyName._hasCachedInputGridPosition)
		{
			value = VariantUtils.CreateFrom(in _hasCachedInputGridPosition);
			return true;
		}
		if (name == PropertyName._cachedInputMousePosition)
		{
			value = VariantUtils.CreateFrom(in _cachedInputMousePosition);
			return true;
		}
		if (name == PropertyName._cachedInputGridPosition)
		{
			value = VariantUtils.CreateFrom(in _cachedInputGridPosition);
			return true;
		}
		if (name == PropertyName._committedMapId)
		{
			value = VariantUtils.CreateFrom(in _committedMapId);
			return true;
		}
		if (name == PropertyName._committedMapPath)
		{
			value = VariantUtils.CreateFrom(in _committedMapPath);
			return true;
		}
		if (name == PropertyName._maximumFpsBeforeMap)
		{
			value = VariantUtils.CreateFrom(in _maximumFpsBeforeMap);
			return true;
		}
		if (name == PropertyName._currentMapMaximumFps)
		{
			value = VariantUtils.CreateFrom(in _currentMapMaximumFps);
			return true;
		}
		if (name == PropertyName._mapFrameRateLimitActive)
		{
			value = VariantUtils.CreateFrom(in _mapFrameRateLimitActive);
			return true;
		}
		if (name == PropertyName._syncLineUseSnapshot)
		{
			value = VariantUtils.CreateFrom(in _syncLineUseSnapshot);
			return true;
		}
		if (name == PropertyName._syncLineUseDirty)
		{
			value = VariantUtils.CreateFrom(in _syncLineUseDirty);
			return true;
		}
		if (name == PropertyName._iceCapRuntimeCache)
		{
			GodotObject[] iceCapRuntimeCache = _iceCapRuntimeCache;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(iceCapRuntimeCache);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorPreviewMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.firstConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.changeConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSwitch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.switchTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.switchTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stripeRow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.lineUse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.iceCapList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nextMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isChange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentGradientPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentGradient, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.rect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.groundRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPlantColumn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.gloveManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetPickControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._destroyed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mapRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PlantGridRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastAppliedMapRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastUnresolvedMapRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastUnresolvedMapIdentity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._switchReturnConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._switchResidenceConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._switchReturnDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isDrainingMapActions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mapActionDrainScheduled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gridReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastInputPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCachedInputGridPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cachedInputMousePosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._cachedInputGridPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._committedMapId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._committedMapPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._maximumFpsBeforeMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentMapMaximumFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mapFrameRateLimitActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._syncLineUseSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._syncLineUseDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._iceCapRuntimeCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.PlantGridRevision, Variant.From<ulong>(PlantGridRevision));
		info.AddProperty(PropertyName.mapControl, Variant.From(in mapControl));
		info.AddProperty(PropertyName.mapConfig, Variant.From(in mapConfig));
		info.AddProperty(PropertyName.editorPreviewMode, Variant.From(in editorPreviewMode));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.firstConfig, Variant.From(in firstConfig));
		info.AddProperty(PropertyName.changeConfig, Variant.From(in changeConfig));
		info.AddProperty(PropertyName.isSwitch, Variant.From(in isSwitch));
		info.AddProperty(PropertyName.switchTimer, Variant.From(in switchTimer));
		info.AddProperty(PropertyName.switchTween, Variant.From(in switchTween));
		info.AddProperty(PropertyName.stripeRow, Variant.From(in stripeRow));
		info.AddProperty(PropertyName.lineUse, Variant.CreateFrom(lineUse));
		info.AddProperty(PropertyName.plantGrid, Variant.CreateFrom(plantGrid));
		info.AddProperty(PropertyName.iceCapList, Variant.From(in iceCapList));
		info.AddProperty(PropertyName.currentMap, Variant.From(in currentMap));
		info.AddProperty(PropertyName.nextMap, Variant.From(in nextMap));
		info.AddProperty(PropertyName.isChange, Variant.From(in isChange));
		info.AddProperty(PropertyName.currentGradientPos, Variant.From(in currentGradientPos));
		info.AddProperty(PropertyName.currentGradient, Variant.From(in currentGradient));
		info.AddProperty(PropertyName.rect, Variant.From(in rect));
		info.AddProperty(PropertyName.groundRect, Variant.From(in groundRect));
		info.AddProperty(PropertyName.isPlantColumn, Variant.From(in isPlantColumn));
		info.AddProperty(PropertyName.shovelManager, Variant.From(in shovelManager));
		info.AddProperty(PropertyName.gloveManager, Variant.From(in gloveManager));
		info.AddProperty(PropertyName.packetPickControl, Variant.From(in packetPickControl));
		info.AddProperty(PropertyName._destroyed, Variant.From(in _destroyed));
		info.AddProperty(PropertyName._mapRevision, Variant.From(in _mapRevision));
		info.AddProperty(PropertyName._lastAppliedMapRevision, Variant.From(in _lastAppliedMapRevision));
		info.AddProperty(PropertyName._lastUnresolvedMapRevision, Variant.From(in _lastUnresolvedMapRevision));
		info.AddProperty(PropertyName._lastUnresolvedMapIdentity, Variant.From(in _lastUnresolvedMapIdentity));
		info.AddProperty(PropertyName._switchReturnConfig, Variant.From(in _switchReturnConfig));
		info.AddProperty(PropertyName._switchResidenceConfig, Variant.From(in _switchResidenceConfig));
		info.AddProperty(PropertyName._switchReturnDuration, Variant.From(in _switchReturnDuration));
		info.AddProperty(PropertyName._isDrainingMapActions, Variant.From(in _isDrainingMapActions));
		info.AddProperty(PropertyName._mapActionDrainScheduled, Variant.From(in _mapActionDrainScheduled));
		info.AddProperty(PropertyName._gridReady, Variant.From(in _gridReady));
		info.AddProperty(PropertyName._lastInputPhysicsFrame, Variant.From(in _lastInputPhysicsFrame));
		info.AddProperty(PropertyName._hasCachedInputGridPosition, Variant.From(in _hasCachedInputGridPosition));
		info.AddProperty(PropertyName._cachedInputMousePosition, Variant.From(in _cachedInputMousePosition));
		info.AddProperty(PropertyName._cachedInputGridPosition, Variant.From(in _cachedInputGridPosition));
		info.AddProperty(PropertyName._committedMapId, Variant.From(in _committedMapId));
		info.AddProperty(PropertyName._committedMapPath, Variant.From(in _committedMapPath));
		info.AddProperty(PropertyName._maximumFpsBeforeMap, Variant.From(in _maximumFpsBeforeMap));
		info.AddProperty(PropertyName._currentMapMaximumFps, Variant.From(in _currentMapMaximumFps));
		info.AddProperty(PropertyName._mapFrameRateLimitActive, Variant.From(in _mapFrameRateLimitActive));
		info.AddProperty(PropertyName._syncLineUseSnapshot, Variant.From(in _syncLineUseSnapshot));
		info.AddProperty(PropertyName._syncLineUseDirty, Variant.From(in _syncLineUseDirty));
		StringName iceCapRuntimeCache = PropertyName._iceCapRuntimeCache;
		GodotObject[] iceCapRuntimeCache2 = _iceCapRuntimeCache;
		info.AddProperty(iceCapRuntimeCache, Variant.CreateFrom(iceCapRuntimeCache2));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.PlantGridRevision, out var value))
		{
			PlantGridRevision = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.mapControl, out var value2))
		{
			mapControl = value2.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName.mapConfig, out var value3))
		{
			mapConfig = value3.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName.editorPreviewMode, out var value4))
		{
			editorPreviewMode = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value5))
		{
			config = value5.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName.firstConfig, out var value6))
		{
			firstConfig = value6.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName.changeConfig, out var value7))
		{
			changeConfig = value7.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName.isSwitch, out var value8))
		{
			isSwitch = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.switchTimer, out var value9))
		{
			switchTimer = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.switchTween, out var value10))
		{
			switchTween = value10.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.stripeRow, out var value11))
		{
			stripeRow = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.lineUse, out var value12))
		{
			lineUse = value12.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName.plantGrid, out var value13))
		{
			plantGrid = value13.AsGodotArray<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.iceCapList, out var value14))
		{
			iceCapList = value14.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.currentMap, out var value15))
		{
			currentMap = value15.As<TowerDefenseMap>();
		}
		if (info.TryGetProperty(PropertyName.nextMap, out var value16))
		{
			nextMap = value16.As<TowerDefenseMap>();
		}
		if (info.TryGetProperty(PropertyName.isChange, out var value17))
		{
			isChange = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentGradientPos, out var value18))
		{
			currentGradientPos = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentGradient, out var value19))
		{
			currentGradient = value19.As<GradientTexture1D>();
		}
		if (info.TryGetProperty(PropertyName.rect, out var value20))
		{
			rect = value20.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.groundRect, out var value21))
		{
			groundRect = value21.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.isPlantColumn, out var value22))
		{
			isPlantColumn = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shovelManager, out var value23))
		{
			shovelManager = value23.As<ShovelManager>();
		}
		if (info.TryGetProperty(PropertyName.gloveManager, out var value24))
		{
			gloveManager = value24.As<GloveManager>();
		}
		if (info.TryGetProperty(PropertyName.packetPickControl, out var value25))
		{
			packetPickControl = value25.As<PacketPickControl>();
		}
		if (info.TryGetProperty(PropertyName._destroyed, out var value26))
		{
			_destroyed = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mapRevision, out var value27))
		{
			_mapRevision = value27.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastAppliedMapRevision, out var value28))
		{
			_lastAppliedMapRevision = value28.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastUnresolvedMapRevision, out var value29))
		{
			_lastUnresolvedMapRevision = value29.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastUnresolvedMapIdentity, out var value30))
		{
			_lastUnresolvedMapIdentity = value30.As<string>();
		}
		if (info.TryGetProperty(PropertyName._switchReturnConfig, out var value31))
		{
			_switchReturnConfig = value31.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName._switchResidenceConfig, out var value32))
		{
			_switchResidenceConfig = value32.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName._switchReturnDuration, out var value33))
		{
			_switchReturnDuration = value33.As<double>();
		}
		if (info.TryGetProperty(PropertyName._isDrainingMapActions, out var value34))
		{
			_isDrainingMapActions = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mapActionDrainScheduled, out var value35))
		{
			_mapActionDrainScheduled = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gridReady, out var value36))
		{
			_gridReady = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastInputPhysicsFrame, out var value37))
		{
			_lastInputPhysicsFrame = value37.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._hasCachedInputGridPosition, out var value38))
		{
			_hasCachedInputGridPosition = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedInputMousePosition, out var value39))
		{
			_cachedInputMousePosition = value39.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._cachedInputGridPosition, out var value40))
		{
			_cachedInputGridPosition = value40.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._committedMapId, out var value41))
		{
			_committedMapId = value41.As<string>();
		}
		if (info.TryGetProperty(PropertyName._committedMapPath, out var value42))
		{
			_committedMapPath = value42.As<string>();
		}
		if (info.TryGetProperty(PropertyName._maximumFpsBeforeMap, out var value43))
		{
			_maximumFpsBeforeMap = value43.As<int>();
		}
		if (info.TryGetProperty(PropertyName._currentMapMaximumFps, out var value44))
		{
			_currentMapMaximumFps = value44.As<int>();
		}
		if (info.TryGetProperty(PropertyName._mapFrameRateLimitActive, out var value45))
		{
			_mapFrameRateLimitActive = value45.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._syncLineUseSnapshot, out var value46))
		{
			_syncLineUseSnapshot = value46.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName._syncLineUseDirty, out var value47))
		{
			_syncLineUseDirty = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._iceCapRuntimeCache, out var value48))
		{
			_iceCapRuntimeCache = value48.AsGodotObjectArray<TowerDefenseIceCap>();
		}
	}
}
