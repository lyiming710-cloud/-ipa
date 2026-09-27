using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateFullRegistryPageCrowdVisibilityRuntimeTest.cs")]
public sealed class AdobeAnimateFullRegistryPageCrowdVisibilityRuntimeTest : Node
{
	private sealed class CharacterEntry
	{
		public string ScenePath = string.Empty;

		public TowerDefenseCharacter Character;

		public Node2D Holder;

		public readonly List<AdobeAnimateSprite> RenderRoots = new List<AdobeAnimateSprite>();

		public int Page;

		public Rect2I ScreenRegion;
	}

	private sealed class PublicationSnapshot
	{
		public readonly Dictionary<int, int> CrowdVisibleInstancesByZ = new Dictionary<int, int>();

		public int CrowdVisibleInstances;

		public int OffPageCrowdVisibleInstances;

		public bool CurrentPageCrowdMatchesExpected;

		public int FallbackVisibleInstances;

		public long RdQueueDepth;

		public long RdLastQueuedFrameVersion;

		public long RdLastAppliedFrameVersion;

		public long RdFailedBatches;

		public long RdBufferUpdateFailures;

		public long RdInvalidTargets;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PrepareRuntime = "PrepareRuntime";

		public static readonly StringName CollectRegisteredCharacterScenePaths = "CollectRegisteredCharacterScenePaths";

		public static readonly StringName SetFirstPageRenderRootsVisible = "SetFirstPageRenderRootsVisible";

		public static readonly StringName FirstPageRenderRootsMatchVisibility = "FirstPageRenderRootsMatchVisibility";

		public static readonly StringName AllHoldersRemainVisible = "AllHoldersRemainVisible";

		public static readonly StringName SetCatalogPageVisibility = "SetCatalogPageVisibility";

		public static readonly StringName HoldersMatchCatalogPageVisibility = "HoldersMatchCatalogPageVisibility";

		public static readonly StringName GetNativeVisibleInstanceCount = "GetNativeVisibleInstanceCount";

		public static readonly StringName DescribeVisibleCrowdBuckets = "DescribeVisibleCrowdBuckets";

		public static readonly StringName RecordMismatch = "RecordMismatch";

		public static readonly StringName CountSignalPixels = "CountSignalPixels";

		public static readonly StringName CountChangedPixels = "CountChangedPixels";

		public static readonly StringName FreezeDetachedAnimationTree = "FreezeDetachedAnimationTree";

		public static readonly StringName EnsureCatalogAnimation = "EnsureCatalogAnimation";

		public static readonly StringName EnsureValidAnimation = "EnsureValidAnimation";

		public static readonly StringName GetFirstConfiguredClip = "GetFirstConfiguredClip";

		public static readonly StringName IsValidBounds = "IsValidBounds";

		public static readonly StringName TransformRect = "TransformRect";

		public static readonly StringName MergeBounds = "MergeBounds";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalBackend = "_originalBackend";

		public static readonly StringName _originalRasterCompositeEnabled = "_originalRasterCompositeEnabled";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";

		public static readonly StringName _runtimePrepared = "_runtimePrepared";

		public static readonly StringName _world = "_world";

		public static readonly StringName _camera = "_camera";

		public static readonly StringName _expectedCharacterCount = "_expectedCharacterCount";

		public static readonly StringName _craterGSprite = "_craterGSprite";

		public static readonly StringName _firstMismatch = "_firstMismatch";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT";

	private const string CharacterRegistryPath = "res://Asset/Config/Character/CharacterResource.json";

	private const string CraterGCharacterName = "CraterG";

	private const string CrowdBucketPrefix = "AdobeAnimateCrowdZ_";

	private const string FallbackBucketPrefix = "AdobeAnimateSnapshotFallbackZ_";

	private const int ViewportWidth = 1200;

	private const int ViewportHeight = 700;

	private const int PageColumns = 6;

	private const int PageRows = 3;

	private const int CharactersPerPage = 18;

	private const int CellWidth = 190;

	private const int CellHeight = 210;

	private const int GridMarginX = 30;

	private const int GridMarginY = 30;

	private const float PageStride = 1500f;

	private const int MinimumAnimationDeltaPixels = 12;

	private static readonly Color BackgroundColor = new Color(0.013f, 0.017f, 0.023f);

	private readonly List<string> _candidateScenePaths = new List<string>(560);

	private readonly List<PackedScene> _retainedScenes = new List<PackedScene>(560);

	private readonly List<CharacterEntry> _characters = new List<CharacterEntry>(560);

	private readonly Dictionary<string, string> _registeredNameByScenePath = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private readonly List<AdobeAnimateSprite> _firstPageRenderRoots = new List<AdobeAnimateSprite>(32);

	private readonly Dictionary<int, int> _firstPageExpectedInstancesByZ = new Dictionary<int, int>();

	private AdobeAnimateRenderBackend _originalBackend;

	private bool _originalRasterCompositeEnabled;

	private int _originalMaxFps;

	private bool _runtimePrepared;

	private Node2D _world;

	private Camera2D _camera;

	private int _expectedCharacterCount;

	private CharacterEntry _craterGEntry;

	private AdobeAnimateSprite _craterGSprite;

	private string _firstMismatch = "none";

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		int exitCode = 2;
		try
		{
			_ = 9;
			try
			{
				PrepareRuntime();
				CollectRegisteredCharacterScenePaths();
				await InstantiateAllCharacters();
				await ConfigureAllCharacters();
				Require(_characters.Count == _expectedCharacterCount, $"正式角色装配不完整: {_characters.Count}/{_expectedCharacterCount}");
				Require(GodotObject.IsInstanceValid(_craterGSprite) && _craterGEntry != null, "完整注册表中未找到正式 CraterG");
				Require(AllHoldersRemainVisible(), "装配阶段错误隐藏了 Holder");
				Require(_firstPageRenderRoots.Count > 0, "正式第一页没有独立动画渲染根");
				await WaitPhysicsFrames(6);
				SetCatalogPageVisibility(0);
				_camera.Position = new Vector2(600f, 350f);
				AdobeAnimateRuntimeManager.NotifyViewportTransformChanged();
				await WaitPhysicsFrames(5);
				await WaitProcessFrames(3);
				using Image visibleImage = await CaptureImage();
				PublicationSnapshot visibleSnapshot = ReadPublicationSnapshot();
				int visiblePixels = CountSignalPixels(visibleImage, _craterGEntry.ScreenRegion);
				bool visibleRoots = FirstPageRenderRootsMatchVisibility(visible: true);
				bool visiblePageScope = HoldersMatchCatalogPageVisibility(0);
				SetFirstPageRenderRootsVisible(visible: false);
				bool hiddenRoots = FirstPageRenderRootsMatchVisibility(visible: false);
				bool hiddenPageScope = HoldersMatchCatalogPageVisibility(0);
				await WaitPhysicsFrames(3);
				using Image hiddenImage = await CaptureImage();
				PublicationSnapshot hiddenSnapshot = ReadPublicationSnapshot();
				int hiddenViewportPixels = CountSignalPixels(hiddenImage, new Rect2I(0, 0, 1200, 700));
				int visibleDelta = CountChangedPixels(visibleImage, hiddenImage, _craterGEntry.ScreenRegion);
				SetFirstPageRenderRootsVisible(visible: true);
				bool restoredRoots = FirstPageRenderRootsMatchVisibility(visible: true);
				bool restoredPageScope = HoldersMatchCatalogPageVisibility(0);
				await WaitPhysicsFrames(4);
				using Image image = await CaptureImage();
				PublicationSnapshot publicationSnapshot = ReadPublicationSnapshot();
				int num = CountSignalPixels(image, _craterGEntry.ScreenRegion);
				int num2 = CountChangedPixels(image, hiddenImage, _craterGEntry.ScreenRegion);
				EvaluateVisiblePhase(visibleSnapshot, visiblePixels, visibleRoots, visiblePageScope);
				EvaluateHiddenPhase(hiddenSnapshot, hiddenViewportPixels, visibleDelta, hiddenRoots, hiddenPageScope);
				EvaluateRestoredPhase(publicationSnapshot, num, num2, restoredRoots, restoredPageScope);
				bool flag = _firstMismatch == "none";
				GD.Print($"{"ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT"} phase=visible pixels={visiblePixels} roots={visibleRoots} pageScope={visiblePageScope} {FormatSnapshot(visibleSnapshot)}");
				GD.Print($"{"ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT"} phase=hidden viewportPixels={hiddenViewportPixels} delta={visibleDelta} roots={hiddenRoots} pageScope={hiddenPageScope} {FormatSnapshot(hiddenSnapshot)}");
				GD.Print($"{"ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT"} phase=restored pixels={num} delta={num2} roots={restoredRoots} pageScope={restoredPageScope} {FormatSnapshot(publicationSnapshot)}");
				if (hiddenSnapshot.CrowdVisibleInstances > 0)
				{
					GD.PrintErr("ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT hiddenCrowdBuckets=" + DescribeVisibleCrowdBuckets());
				}
				GD.Print($"{"ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT"} passed={flag} checks={_checks} failures={_failures} characters={_characters.Count}/{_expectedCharacterCount} pageRoots={_firstPageRenderRoots.Count} firstMismatch={_firstMismatch} offPageCrowdVisible={visibleSnapshot.OffPageCrowdVisibleInstances} hiddenCrowdVisible={hiddenSnapshot.CrowdVisibleInstances} hiddenFallbackVisible={hiddenSnapshot.FallbackVisibleInstances} hiddenViewportPixels={hiddenViewportPixels} visiblePixels={visiblePixels} restoredPixels={num} visibleDelta={visibleDelta} restoredDelta={num2} rdFailures={publicationSnapshot.RdFailedBatches}/{publicationSnapshot.RdBufferUpdateFailures}/{publicationSnapshot.RdInvalidTargets} renderer={RenderingServer.GetCurrentRenderingMethod()}");
				exitCode = ((!flag) ? 2 : 0);
			}
			catch (Exception value)
			{
				_failures++;
				GD.PrintErr($"{"ADOBE_ANIMATE_FULL_REGISTRY_PAGE_CROWD_VISIBILITY_RESULT"} passed=False exception={value}");
			}
		}
		finally
		{
			await RestoreRuntime();
		}
		GetTree().Quit(exitCode);
	}

	private void PrepareRuntime()
	{
		if (!GodotObject.IsInstanceValid(Global.Instance))
		{
			throw new InvalidOperationException("Global autoload is unavailable.");
		}
		ProcessMode = ProcessModeEnum.Always;
		_originalBackend = Global.Instance.adobeAnimateRenderBackend;
		_originalRasterCompositeEnabled = AdobeAnimateRenderManager.RasterCompositeEnabled;
		_originalMaxFps = Engine.MaxFps;
		_runtimePrepared = true;
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
		AdobeAnimateRenderManager.RasterCompositeEnabled = false;
		Engine.MaxFps = Math.Max(120, Engine.PhysicsTicksPerSecond * 2);
		CanvasLayer canvasLayer = new CanvasLayer
		{
			Name = "BackgroundLayer",
			Layer = -100,
			ProcessMode = ProcessModeEnum.Always
		};
		AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
		canvasLayer.AddChild(new ColorRect
		{
			Name = "Background",
			Color = BackgroundColor,
			Position = Vector2.Zero,
			Size = new Vector2(1200f, 700f),
			MouseFilter = Control.MouseFilterEnum.Ignore
		}, forceReadableName: false, InternalMode.Disabled);
		_world = new Node2D
		{
			Name = "AllCharacterWorld",
			ProcessMode = ProcessModeEnum.Always
		};
		AddChild(_world, forceReadableName: false, InternalMode.Disabled);
		_camera = new Camera2D
		{
			Name = "FirstPageCamera",
			Position = new Vector2(600f, 350f),
			Enabled = true,
			ProcessMode = ProcessModeEnum.Always
		};
		_world.AddChild(_camera, forceReadableName: false, InternalMode.Disabled);
	}

	private void CollectRegisteredCharacterScenePaths()
	{
		Json json = ResourceLoader.Load<Json>("res://Asset/Config/Character/CharacterResource.json", null, ResourceLoader.CacheMode.Ignore);
		Require(GodotObject.IsInstanceValid(json), "正式角色注册表无法加载");
		FullGameplayRegistryRoots registryRoots = FullGameplayResourceManifest.GetRegistryRoots(json);
		_expectedCharacterCount = registryRoots.RegisteredCharacterCount;
		_candidateScenePaths.Clear();
		_registeredNameByScenePath.Clear();
		foreach (var (value, text3) in registryRoots.ScenePathByCharacter)
		{
			if (!_registeredNameByScenePath.TryAdd(text3, value))
			{
				throw new InvalidOperationException("正式角色注册表包含重复场景: " + text3);
			}
		}
		_candidateScenePaths.AddRange(registryRoots.UniqueSceneRoots);
		Require(_candidateScenePaths.Count == _expectedCharacterCount, $"正式角色场景清单不完整: {_candidateScenePaths.Count}/{_expectedCharacterCount}");
	}

	private async Task InstantiateAllCharacters()
	{
		for (int sceneIndex = 0; sceneIndex < _candidateScenePaths.Count; sceneIndex++)
		{
			string text = _candidateScenePaths[sceneIndex];
			PackedScene packedScene = ResourceLoader.Load<PackedScene>(text, null, ResourceLoader.CacheMode.Reuse);
			Require(GodotObject.IsInstanceValid(packedScene), "正式角色场景无法加载: " + text);
			_retainedScenes.Add(packedScene);
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			Require(node is TowerDefenseCharacter, "正式角色场景根类型错误: " + text + ", actual=" + node?.GetType().Name);
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)node;
			FreezeDetachedAnimationTree(towerDefenseCharacter);
			towerDefenseCharacter.inGame = false;
			towerDefenseCharacter.skipDestroySet = true;
			towerDefenseCharacter.editorPreviewMode = true;
			towerDefenseCharacter.forceLocalRenderDuringZMotion = false;
			towerDefenseCharacter.ProcessMode = ProcessModeEnum.Disabled;
			Node2D node2D = new Node2D
			{
				Name = $"CharacterHolder{_characters.Count}",
				Visible = true,
				ProcessMode = ProcessModeEnum.Always
			};
			_world.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			node2D.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
			CharacterEntry characterEntry = new CharacterEntry
			{
				ScenePath = text,
				Character = towerDefenseCharacter,
				Holder = node2D
			};
			_characters.Add(characterEntry);
			if (_registeredNameByScenePath.TryGetValue(text, out var value) && value == "CraterG")
			{
				_craterGEntry = characterEntry;
			}
			if ((sceneIndex + 1) % 8 == 0)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
		}
	}

	private async Task ConfigureAllCharacters()
	{
		for (int index = 0; index < _characters.Count; index++)
		{
			CharacterEntry characterEntry = _characters[index];
			List<AdobeAnimateSprite> list = new List<AdobeAnimateSprite>();
			CollectSprites(characterEntry.Character, list);
			Rect2 rect = default;
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = list[i];
				bool flag2 = adobeAnimateSprite == characterEntry.Character.sprite;
				adobeAnimateSprite.ProcessMode = ProcessModeEnum.Always;
				adobeAnimateSprite.forceLocalRender = false;
				adobeAnimateSprite.pause = false;
				adobeAnimateSprite.timeScale = 0.0;
				if (flag2)
				{
					adobeAnimateSprite.Visible = true;
					EnsureCatalogAnimation(characterEntry.Character, adobeAnimateSprite);
				}
				else
				{
					EnsureValidAnimation(adobeAnimateSprite);
				}
				adobeAnimateSprite.RefreshProcessScheduling();
				if (adobeAnimateSprite.IsVisibleInTree())
				{
					AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(adobeAnimateSprite.flashAnimeData, adobeAnimateSprite.atlasProfileOverride);
					Rect2 currentFrameBounds = GetCurrentFrameBounds(adobeAnimateSprite, orBuild);
					if (IsValidBounds(currentFrameBounds))
					{
						Transform2D transform = characterEntry.Holder.GlobalTransform.AffineInverse() * adobeAnimateSprite.GlobalTransform;
						Rect2 rect2 = new Rect2(currentFrameBounds.Position + adobeAnimateSprite.offset, currentFrameBounds.Size);
						Rect2 rect3 = TransformRect(transform, rect2);
						rect = (flag ? MergeBounds(rect, rect3) : rect3);
						flag = true;
					}
					if (!adobeAnimateSprite.IsRenderedByParentSpriteForRender() && !adobeAnimateSprite.IsEmptyHiddenGpuGraphPlaceholderForRender())
					{
						Require(AdobeAnimateGpuRenderGraphBuilder.TryBuild(adobeAnimateSprite, out var _, out var _, out var failureReason), $"正式角色 GPU Graph 被拒绝: {characterEntry.ScenePath}:{adobeAnimateSprite.GetPath()}, reason={failureReason}");
						characterEntry.RenderRoots.Add(adobeAnimateSprite);
					}
				}
			}
			Require(characterEntry.RenderRoots.Count > 0, "正式角色没有独立动画渲染根: " + characterEntry.ScenePath);
			if (!flag)
			{
				rect = new Rect2(-64f, -96f, 128f, 192f);
			}
			PlaceCharacterEntry(characterEntry, index, rect);
			if (characterEntry.Page == 0)
			{
				_firstPageRenderRoots.AddRange(characterEntry.RenderRoots);
			}
			if (characterEntry == _craterGEntry)
			{
				_craterGSprite = characterEntry.Character.sprite;
			}
			if ((index + 1) % 8 == 0)
			{
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}
		}
		Require(_craterGEntry != null && _craterGEntry.Page == 0, $"CraterG 不在正式第一页: page={_craterGEntry?.Page}");
		Require(GodotObject.IsInstanceValid(_craterGSprite), "CraterG 主动画根无效");
		_firstPageExpectedInstancesByZ.Clear();
		for (int j = 0; j < _firstPageRenderRoots.Count; j++)
		{
			AdobeAnimateSprite adobeAnimateSprite2 = _firstPageRenderRoots[j];
			AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = adobeAnimateSprite2.TryBuildCrowdRenderState(out var state);
			Require(adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted && state != null && state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph, $"第一页动画根未进入 GPU Graph: {adobeAnimateSprite2.GetPath()}, result={adobeAnimateCrowdRenderStateResult}, mode={state?.Mode}");
			_firstPageExpectedInstancesByZ.TryGetValue(state.EffectiveZIndex, out var value);
			_firstPageExpectedInstancesByZ[state.EffectiveZIndex] = value + 1;
		}
	}

	private void SetFirstPageRenderRootsVisible(bool visible)
	{
		for (int i = 0; i < _firstPageRenderRoots.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _firstPageRenderRoots[i];
			Require(GodotObject.IsInstanceValid(adobeAnimateSprite), $"第一页渲染根在切换 visible={visible} 前失效");
			adobeAnimateSprite.Visible = visible;
		}
	}

	private bool FirstPageRenderRootsMatchVisibility(bool visible)
	{
		for (int i = 0; i < _firstPageRenderRoots.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _firstPageRenderRoots[i];
			if (!GodotObject.IsInstanceValid(adobeAnimateSprite) || adobeAnimateSprite.Visible != visible || adobeAnimateSprite.IsVisibleInTree() != visible)
			{
				return false;
			}
		}
		return true;
	}

	private bool AllHoldersRemainVisible()
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			Node2D holder = _characters[i].Holder;
			if (!GodotObject.IsInstanceValid(holder) || !holder.Visible || !holder.IsVisibleInTree())
			{
				return false;
			}
		}
		return true;
	}

	private void SetCatalogPageVisibility(int page)
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			CharacterEntry characterEntry = _characters[i];
			characterEntry.Holder.Visible = characterEntry.Page == page;
		}
	}

	private bool HoldersMatchCatalogPageVisibility(int page)
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			CharacterEntry characterEntry = _characters[i];
			bool flag = characterEntry.Page == page;
			if (!GodotObject.IsInstanceValid(characterEntry.Holder) || characterEntry.Holder.Visible != flag || characterEntry.Holder.IsVisibleInTree() != flag)
			{
				return false;
			}
		}
		return true;
	}

	private PublicationSnapshot ReadPublicationSnapshot()
	{
		AdobeAnimateRenderManager.ResolveRenderMount(_craterGSprite, out var mountParent, out var _);
		PublicationSnapshot publicationSnapshot = new PublicationSnapshot();
		AccumulatePublicationNodes(mountParent, publicationSnapshot);
		FinalizeCurrentPageCrowdCounts(publicationSnapshot);
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot statistics = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
		publicationSnapshot.RdQueueDepth = statistics.CurrentQueueDepth;
		publicationSnapshot.RdLastQueuedFrameVersion = statistics.LastQueuedFrameVersion;
		publicationSnapshot.RdLastAppliedFrameVersion = statistics.LastAppliedFrameVersion;
		publicationSnapshot.RdFailedBatches = statistics.FailedBatchCount;
		publicationSnapshot.RdBufferUpdateFailures = statistics.BufferUpdateFailureCount;
		publicationSnapshot.RdInvalidTargets = statistics.InvalidTargetCount;
		return publicationSnapshot;
	}

	private void AccumulatePublicationNodes(Node node, PublicationSnapshot snapshot)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		string text = node.Name.ToString();
		if (node is MultiMeshInstance2D multiMeshInstance2D && text.StartsWith("AdobeAnimateCrowdZ_", StringComparison.Ordinal) && GodotObject.IsInstanceValid(multiMeshInstance2D.Multimesh))
		{
			int nativeVisibleInstanceCount = GetNativeVisibleInstanceCount(multiMeshInstance2D);
			snapshot.CrowdVisibleInstances += nativeVisibleInstanceCount;
			if (nativeVisibleInstanceCount > 0)
			{
				snapshot.CrowdVisibleInstancesByZ.TryGetValue(multiMeshInstance2D.ZIndex, out var value);
				snapshot.CrowdVisibleInstancesByZ[multiMeshInstance2D.ZIndex] = value + nativeVisibleInstanceCount;
			}
		}
		if (node is AdobeAnimateMultiMeshBatcher adobeAnimateMultiMeshBatcher && text.StartsWith("AdobeAnimateSnapshotFallbackZ_", StringComparison.Ordinal))
		{
			snapshot.FallbackVisibleInstances += Math.Max(0, adobeAnimateMultiMeshBatcher.GetVisibleInstanceCountForTest());
		}
		int childCount = node.GetChildCount(includeInternal: true);
		for (int i = 0; i < childCount; i++)
		{
			AccumulatePublicationNodes(node.GetChild(i, includeInternal: true), snapshot);
		}
	}

	private void FinalizeCurrentPageCrowdCounts(PublicationSnapshot snapshot)
	{
		bool currentPageCrowdMatchesExpected = true;
		foreach (KeyValuePair<int, int> item in snapshot.CrowdVisibleInstancesByZ)
		{
			_firstPageExpectedInstancesByZ.TryGetValue(item.Key, out var value);
			if (item.Value > value)
			{
				snapshot.OffPageCrowdVisibleInstances += item.Value - value;
			}
			if (item.Value != value)
			{
				currentPageCrowdMatchesExpected = false;
			}
		}
		foreach (KeyValuePair<int, int> item2 in _firstPageExpectedInstancesByZ)
		{
			snapshot.CrowdVisibleInstancesByZ.TryGetValue(item2.Key, out var value2);
			if (value2 != item2.Value)
			{
				currentPageCrowdMatchesExpected = false;
			}
		}
		snapshot.CurrentPageCrowdMatchesExpected = currentPageCrowdMatchesExpected;
	}

	private static int GetNativeVisibleInstanceCount(MultiMeshInstance2D instance)
	{
		int num = RenderingServer.MultimeshGetVisibleInstances(instance.Multimesh.GetRid());
		if (num < 0)
		{
			return Math.Max(0, instance.Multimesh.InstanceCount);
		}
		return num;
	}

	private string DescribeVisibleCrowdBuckets()
	{
		AdobeAnimateRenderManager.ResolveRenderMount(_craterGSprite, out var mountParent, out var _);
		List<string> list = new List<string>();
		CollectVisibleCrowdBucketDescriptions(mountParent, list);
		if (list.Count != 0)
		{
			return string.Join(";", list);
		}
		return "none";
	}

	private static void CollectVisibleCrowdBucketDescriptions(Node node, List<string> descriptions)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is MultiMeshInstance2D multiMeshInstance2D && node.Name.ToString().StartsWith("AdobeAnimateCrowdZ_", StringComparison.Ordinal) && GodotObject.IsInstanceValid(multiMeshInstance2D.Multimesh))
		{
			int nativeVisibleInstanceCount = GetNativeVisibleInstanceCount(multiMeshInstance2D);
			if (nativeVisibleInstanceCount > 0)
			{
				descriptions.Add($"{multiMeshInstance2D.Name}[z={multiMeshInstance2D.ZIndex},instances={nativeVisibleInstanceCount}/{multiMeshInstance2D.Multimesh.InstanceCount}]");
			}
		}
		int childCount = node.GetChildCount(includeInternal: true);
		for (int i = 0; i < childCount; i++)
		{
			CollectVisibleCrowdBucketDescriptions(node.GetChild(i, includeInternal: true), descriptions);
		}
	}

	private void EvaluateVisiblePhase(PublicationSnapshot snapshot, int visiblePixels, bool rootsVisible, bool pageScopeCorrect)
	{
		RecordMismatch(rootsVisible & pageScopeCorrect, "visible_scope");
		RecordMismatch(visiblePixels >= 12, "visible_pixels");
		RecordMismatch(snapshot.CrowdVisibleInstances == _firstPageRenderRoots.Count && snapshot.CurrentPageCrowdMatchesExpected, "visible_crowd");
		RecordMismatch(snapshot.OffPageCrowdVisibleInstances == 0, "off_page_crowd");
		RecordMismatch(snapshot.FallbackVisibleInstances == 0, "visible_fallback");
		RecordMismatch(IsRdHealthy(snapshot), "visible_rd");
	}

	private void EvaluateHiddenPhase(PublicationSnapshot snapshot, int hiddenViewportPixels, int visibleDelta, bool rootsHidden, bool pageScopeCorrect)
	{
		RecordMismatch(rootsHidden & pageScopeCorrect, "hidden_scope");
		RecordMismatch(snapshot.CrowdVisibleInstances == 0, "hidden_crowd");
		RecordMismatch(snapshot.FallbackVisibleInstances == 0, "hidden_fallback");
		RecordMismatch(hiddenViewportPixels == 0 && visibleDelta >= 12, "hidden_pixels");
		RecordMismatch(IsRdHealthy(snapshot), "hidden_rd");
	}

	private void EvaluateRestoredPhase(PublicationSnapshot snapshot, int restoredPixels, int restoredDelta, bool rootsVisible, bool pageScopeCorrect)
	{
		RecordMismatch(rootsVisible & pageScopeCorrect, "restored_scope");
		RecordMismatch(snapshot.CrowdVisibleInstances == _firstPageRenderRoots.Count && snapshot.CurrentPageCrowdMatchesExpected && snapshot.OffPageCrowdVisibleInstances == 0, "restored_crowd");
		RecordMismatch(snapshot.FallbackVisibleInstances == 0, "restored_fallback");
		RecordMismatch(restoredPixels >= 12 && restoredDelta >= 12, "restored_pixels");
		RecordMismatch(IsRdHealthy(snapshot), "restored_rd");
	}

	private static bool IsRdHealthy(PublicationSnapshot snapshot)
	{
		if (snapshot.RdQueueDepth == 0L && snapshot.RdLastAppliedFrameVersion >= snapshot.RdLastQueuedFrameVersion && snapshot.RdFailedBatches == 0L && snapshot.RdBufferUpdateFailures == 0L)
		{
			return snapshot.RdInvalidTargets == 0;
		}
		return false;
	}

	private void RecordMismatch(bool condition, string layer)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			if (_firstMismatch == "none")
			{
				_firstMismatch = layer;
			}
		}
	}

	private async Task WaitProcessFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task WaitPhysicsFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private async Task<Image> CaptureImage()
	{
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		return GetViewport().GetTexture().GetImage();
	}

	private static int CountSignalPixels(Image image, Rect2I region)
	{
		int num = 0;
		int num2 = Math.Min(image.GetWidth(), region.End.X);
		int num3 = Math.Min(image.GetHeight(), region.End.Y);
		for (int i = Math.Max(0, region.Position.Y); i < num3; i++)
		{
			for (int j = Math.Max(0, region.Position.X); j < num2; j++)
			{
				Color pixel = image.GetPixel(j, i);
				if (Mathf.Abs(pixel.R - BackgroundColor.R) + Mathf.Abs(pixel.G - BackgroundColor.G) + Mathf.Abs(pixel.B - BackgroundColor.B) > 0.08f)
				{
					num++;
				}
			}
		}
		return num;
	}

	private static int CountChangedPixels(Image first, Image second, Rect2I region)
	{
		int num = 0;
		int num2 = Math.Min(Math.Min(first.GetWidth(), second.GetWidth()), region.End.X);
		int num3 = Math.Min(Math.Min(first.GetHeight(), second.GetHeight()), region.End.Y);
		for (int i = Math.Max(0, region.Position.Y); i < num3; i++)
		{
			for (int j = Math.Max(0, region.Position.X); j < num2; j++)
			{
				Color pixel = first.GetPixel(j, i);
				Color pixel2 = second.GetPixel(j, i);
				if (Mathf.Abs(pixel.R - pixel2.R) + Mathf.Abs(pixel.G - pixel2.G) + Mathf.Abs(pixel.B - pixel2.B) + Mathf.Abs(pixel.A - pixel2.A) > 0.12f)
				{
					num++;
				}
			}
		}
		return num;
	}

	private static void CollectSprites(Node node, List<AdobeAnimateSprite> output)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			if (node is AdobeAnimateSprite item)
			{
				output.Add(item);
			}
			for (int i = 0; i < node.GetChildCount(); i++)
			{
				CollectSprites(node.GetChild(i), output);
			}
		}
	}

	private static void FreezeDetachedAnimationTree(Node node)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			if (node is AdobeAnimateSprite adobeAnimateSprite)
			{
				adobeAnimateSprite.timeScale = 0.0;
				adobeAnimateSprite.pause = false;
			}
			for (int i = 0; i < node.GetChildCount(); i++)
			{
				FreezeDetachedAnimationTree(node.GetChild(i));
			}
		}
	}

	private static Rect2 GetCurrentFrameBounds(AdobeAnimateSprite sprite, AdobeAnimateRuntimeDefinition definition)
	{
		if (definition == null)
		{
			return default;
		}
		if (sprite.frameIndex >= 0 && sprite.frameIndex < definition.FrameLocalBounds.Length)
		{
			Rect2 rect = definition.FrameLocalBounds[sprite.frameIndex];
			if (IsValidBounds(rect))
			{
				return rect;
			}
		}
		return definition.LocalBounds;
	}

	private static void EnsureCatalogAnimation(TowerDefenseCharacter character, AdobeAnimateSprite sprite)
	{
		AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(sprite.flashAnimeData, sprite.atlasProfileOverride);
		if (orBuild == null)
		{
			return;
		}
		if (character is TowerDefenseZombie towerDefenseZombie)
		{
			string firstConfiguredClip = GetFirstConfiguredClip(towerDefenseZombie.walkAnimeClip);
			if (orBuild.HasClip(firstConfiguredClip))
			{
				sprite.SetAnimation(firstConfiguredClip);
				return;
			}
		}
		if (character is TowerDefensePlant towerDefensePlant)
		{
			string firstConfiguredClip2 = GetFirstConfiguredClip(towerDefensePlant.plantAnimeClip);
			if (orBuild.HasClip(firstConfiguredClip2))
			{
				sprite.SetAnimation(firstConfiguredClip2);
				return;
			}
		}
		if (orBuild.HasClip("Idle"))
		{
			sprite.SetAnimation("Idle");
		}
		else if (orBuild.HasClip("Walk"))
		{
			sprite.SetAnimation("Walk");
		}
		else
		{
			EnsureValidAnimation(sprite);
		}
	}

	private static void EnsureValidAnimation(AdobeAnimateSprite sprite)
	{
		AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(sprite.flashAnimeData, sprite.atlasProfileOverride);
		if (orBuild == null || orBuild.Clips.Length == 0)
		{
			return;
		}
		if (orBuild.HasClip(sprite.clip))
		{
			if (sprite.clipOver)
			{
				sprite.SetAnimation(sprite.clip);
			}
			return;
		}
		for (int i = 0; i < orBuild.Clips.Length; i++)
		{
			PackedClip packedClip = orBuild.Clips[i];
			if (packedClip.Range.Y >= packedClip.Range.X)
			{
				sprite.SetAnimation(packedClip.Name.ToString());
				break;
			}
		}
	}

	private static string GetFirstConfiguredClip(string clips)
	{
		if (string.IsNullOrWhiteSpace(clips))
		{
			return string.Empty;
		}
		string[] array = clips.Split('&', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		if (array.Length == 0)
		{
			return string.Empty;
		}
		return array[0];
	}

	private static void PlaceCharacterEntry(CharacterEntry entry, int index, Rect2 bounds)
	{
		int num = index / 18;
		int num2 = index % 18;
		int num3 = num2 % 6;
		int num4 = num2 / 6;
		Rect2I screenRegion = new Rect2I(30 + num3 * 190, 30 + num4 * 210, 170, 190);
		float num5 = (float)screenRegion.Size.X - 20f;
		float value = Math.Min(val2: ((float)screenRegion.Size.Y - 20f) / Math.Max(1f, bounds.Size.Y), val1: num5 / Math.Max(1f, bounds.Size.X));
		value = Mathf.Clamp(value, 0.015f, 1.35f);
		entry.Page = num;
		entry.ScreenRegion = screenRegion;
		entry.Holder.Scale = Vector2.One * value;
		Vector2 vector = new Vector2((float)num * 1500f, 0f) + screenRegion.GetCenter();
		entry.Holder.Position = vector - bounds.GetCenter() * value;
	}

	private static bool IsValidBounds(Rect2 bounds)
	{
		if (float.IsFinite(bounds.Position.X) && float.IsFinite(bounds.Position.Y) && float.IsFinite(bounds.Size.X) && float.IsFinite(bounds.Size.Y) && bounds.Size.X > 0f)
		{
			return bounds.Size.Y > 0f;
		}
		return false;
	}

	private static Rect2 TransformRect(Transform2D transform, Rect2 rect)
	{
		Vector2 position = transform * rect.Position;
		Vector2 to = transform * (rect.Position + new Vector2(rect.Size.X, 0f));
		Vector2 to2 = transform * (rect.Position + rect.Size);
		Vector2 to3 = transform * (rect.Position + new Vector2(0f, rect.Size.Y));
		return new Rect2(position, Vector2.Zero).Expand(to).Expand(to2).Expand(to3)
			.Abs();
	}

	private static Rect2 MergeBounds(Rect2 left, Rect2 right)
	{
		Vector2 to = right.Position + right.Size;
		return left.Expand(right.Position).Expand(new Vector2(to.X, right.Position.Y)).Expand(to)
			.Expand(new Vector2(right.Position.X, to.Y))
			.Abs();
	}

	private static string FormatSnapshot(PublicationSnapshot snapshot)
	{
		return $"crowdVisible={snapshot.CrowdVisibleInstances} crowdMatchesPage={snapshot.CurrentPageCrowdMatchesExpected} offPageCrowdVisible={snapshot.OffPageCrowdVisibleInstances} fallbackVisible={snapshot.FallbackVisibleInstances} rdQueue={snapshot.RdQueueDepth} rdQueued={snapshot.RdLastQueuedFrameVersion} rdApplied={snapshot.RdLastAppliedFrameVersion} rdFailures={snapshot.RdFailedBatches}/{snapshot.RdBufferUpdateFailures}/{snapshot.RdInvalidTargets}";
	}

	private async Task RestoreRuntime()
	{
		if (GodotObject.IsInstanceValid(_world) && !_world.IsQueuedForDeletion())
		{
			_world.QueueFree();
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
		AdobeAnimateDefinitionCache.Clear();
		_retainedScenes.Clear();
		_characters.Clear();
		_firstPageRenderRoots.Clear();
		_firstPageExpectedInstancesByZ.Clear();
		if (_runtimePrepared && GodotObject.IsInstanceValid(Global.Instance))
		{
			Global.Instance.adobeAnimateRenderBackend = _originalBackend;
		}
		if (_runtimePrepared)
		{
			AdobeAnimateRenderManager.RasterCompositeEnabled = _originalRasterCompositeEnabled;
			Engine.MaxFps = _originalMaxFps;
		}
	}

	private void Require(bool condition, string message)
	{
		_checks++;
		if (condition)
		{
			return;
		}
		_failures++;
		throw new InvalidOperationException(message);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CollectRegisteredCharacterScenePaths, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFirstPageRenderRootsVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FirstPageRenderRootsMatchVisibility, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AllHoldersRemainVisible, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCatalogPageVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "page", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HoldersMatchCatalogPageVisibility, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "page", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNativeVisibleInstanceCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MultiMeshInstance2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DescribeVisibleCrowdBuckets, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RecordMismatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "layer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountSignalPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Rect2I, "region", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountChangedPixels, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "first", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Object, "second", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Rect2I, "region", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreezeDetachedAnimationTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCatalogAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureValidAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetFirstConfiguredClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clips", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidBounds, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "bounds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TransformRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MergeBounds, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.PrepareRuntime && args.Count == 0)
		{
			PrepareRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.CollectRegisteredCharacterScenePaths && args.Count == 0)
		{
			CollectRegisteredCharacterScenePaths();
			ret = default;
			return true;
		}
		if (method == MethodName.SetFirstPageRenderRootsVisible && args.Count == 1)
		{
			SetFirstPageRenderRootsVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstPageRenderRootsMatchVisibility && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(FirstPageRenderRootsMatchVisibility(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.AllHoldersRemainVisible && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AllHoldersRemainVisible());
			return true;
		}
		if (method == MethodName.SetCatalogPageVisibility && args.Count == 1)
		{
			SetCatalogPageVisibility(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HoldersMatchCatalogPageVisibility && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HoldersMatchCatalogPageVisibility(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetNativeVisibleInstanceCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetNativeVisibleInstanceCount(VariantUtils.ConvertTo<MultiMeshInstance2D>(in args[0])));
			return true;
		}
		if (method == MethodName.DescribeVisibleCrowdBuckets && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(DescribeVisibleCrowdBuckets());
			return true;
		}
		if (method == MethodName.RecordMismatch && args.Count == 2)
		{
			RecordMismatch(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountSignalPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountSignalPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountChangedPixels && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(CountChangedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1]), VariantUtils.ConvertTo<Rect2I>(in args[2])));
			return true;
		}
		if (method == MethodName.FreezeDetachedAnimationTree && args.Count == 1)
		{
			FreezeDetachedAnimationTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCatalogAnimation && args.Count == 2)
		{
			EnsureCatalogAnimation(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureValidAnimation && args.Count == 1)
		{
			EnsureValidAnimation(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFirstConfiguredClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFirstConfiguredClip(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidBounds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidBounds(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.TransformRect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(TransformRect(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.MergeBounds && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(MergeBounds(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetNativeVisibleInstanceCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetNativeVisibleInstanceCount(VariantUtils.ConvertTo<MultiMeshInstance2D>(in args[0])));
			return true;
		}
		if (method == MethodName.CountSignalPixels && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CountSignalPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CountChangedPixels && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(CountChangedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1]), VariantUtils.ConvertTo<Rect2I>(in args[2])));
			return true;
		}
		if (method == MethodName.FreezeDetachedAnimationTree && args.Count == 1)
		{
			FreezeDetachedAnimationTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCatalogAnimation && args.Count == 2)
		{
			EnsureCatalogAnimation(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureValidAnimation && args.Count == 1)
		{
			EnsureValidAnimation(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFirstConfiguredClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFirstConfiguredClip(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidBounds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidBounds(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.TransformRect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(TransformRect(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.MergeBounds && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(MergeBounds(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
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
		if (method == MethodName.PrepareRuntime)
		{
			return true;
		}
		if (method == MethodName.CollectRegisteredCharacterScenePaths)
		{
			return true;
		}
		if (method == MethodName.SetFirstPageRenderRootsVisible)
		{
			return true;
		}
		if (method == MethodName.FirstPageRenderRootsMatchVisibility)
		{
			return true;
		}
		if (method == MethodName.AllHoldersRemainVisible)
		{
			return true;
		}
		if (method == MethodName.SetCatalogPageVisibility)
		{
			return true;
		}
		if (method == MethodName.HoldersMatchCatalogPageVisibility)
		{
			return true;
		}
		if (method == MethodName.GetNativeVisibleInstanceCount)
		{
			return true;
		}
		if (method == MethodName.DescribeVisibleCrowdBuckets)
		{
			return true;
		}
		if (method == MethodName.RecordMismatch)
		{
			return true;
		}
		if (method == MethodName.CountSignalPixels)
		{
			return true;
		}
		if (method == MethodName.CountChangedPixels)
		{
			return true;
		}
		if (method == MethodName.FreezeDetachedAnimationTree)
		{
			return true;
		}
		if (method == MethodName.EnsureCatalogAnimation)
		{
			return true;
		}
		if (method == MethodName.EnsureValidAnimation)
		{
			return true;
		}
		if (method == MethodName.GetFirstConfiguredClip)
		{
			return true;
		}
		if (method == MethodName.IsValidBounds)
		{
			return true;
		}
		if (method == MethodName.TransformRect)
		{
			return true;
		}
		if (method == MethodName.MergeBounds)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._originalBackend)
		{
			_originalBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._originalRasterCompositeEnabled)
		{
			_originalRasterCompositeEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			_originalMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimePrepared)
		{
			_runtimePrepared = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._world)
		{
			_world = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._camera)
		{
			_camera = VariantUtils.ConvertTo<Camera2D>(in value);
			return true;
		}
		if (name == PropertyName._expectedCharacterCount)
		{
			_expectedCharacterCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._craterGSprite)
		{
			_craterGSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._firstMismatch)
		{
			_firstMismatch = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
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
		if (name == PropertyName._originalBackend)
		{
			value = VariantUtils.CreateFrom(in _originalBackend);
			return true;
		}
		if (name == PropertyName._originalRasterCompositeEnabled)
		{
			value = VariantUtils.CreateFrom(in _originalRasterCompositeEnabled);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			value = VariantUtils.CreateFrom(in _originalMaxFps);
			return true;
		}
		if (name == PropertyName._runtimePrepared)
		{
			value = VariantUtils.CreateFrom(in _runtimePrepared);
			return true;
		}
		if (name == PropertyName._world)
		{
			value = VariantUtils.CreateFrom(in _world);
			return true;
		}
		if (name == PropertyName._camera)
		{
			value = VariantUtils.CreateFrom(in _camera);
			return true;
		}
		if (name == PropertyName._expectedCharacterCount)
		{
			value = VariantUtils.CreateFrom(in _expectedCharacterCount);
			return true;
		}
		if (name == PropertyName._craterGSprite)
		{
			value = VariantUtils.CreateFrom(in _craterGSprite);
			return true;
		}
		if (name == PropertyName._firstMismatch)
		{
			value = VariantUtils.CreateFrom(in _firstMismatch);
			return true;
		}
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
			new PropertyInfo(Variant.Type.Int, PropertyName._originalBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalRasterCompositeEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimePrepared, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._world, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._camera, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._expectedCharacterCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._craterGSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._firstMismatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalBackend, Variant.From(in _originalBackend));
		info.AddProperty(PropertyName._originalRasterCompositeEnabled, Variant.From(in _originalRasterCompositeEnabled));
		info.AddProperty(PropertyName._originalMaxFps, Variant.From(in _originalMaxFps));
		info.AddProperty(PropertyName._runtimePrepared, Variant.From(in _runtimePrepared));
		info.AddProperty(PropertyName._world, Variant.From(in _world));
		info.AddProperty(PropertyName._camera, Variant.From(in _camera));
		info.AddProperty(PropertyName._expectedCharacterCount, Variant.From(in _expectedCharacterCount));
		info.AddProperty(PropertyName._craterGSprite, Variant.From(in _craterGSprite));
		info.AddProperty(PropertyName._firstMismatch, Variant.From(in _firstMismatch));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalBackend, out var value))
		{
			_originalBackend = value.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._originalRasterCompositeEnabled, out var value2))
		{
			_originalRasterCompositeEnabled = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalMaxFps, out var value3))
		{
			_originalMaxFps = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimePrepared, out var value4))
		{
			_runtimePrepared = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._world, out var value5))
		{
			_world = value5.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._camera, out var value6))
		{
			_camera = value6.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName._expectedCharacterCount, out var value7))
		{
			_expectedCharacterCount = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._craterGSprite, out var value8))
		{
			_craterGSprite = value8.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._firstMismatch, out var value9))
		{
			_firstMismatch = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value10))
		{
			_checks = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value11))
		{
			_failures = value11.As<int>();
		}
	}
}
