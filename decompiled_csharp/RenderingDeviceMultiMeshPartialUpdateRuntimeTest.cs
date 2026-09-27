using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/RenderingDeviceMultiMeshPartialUpdateRuntimeTest.cs")]
public sealed class RenderingDeviceMultiMeshPartialUpdateRuntimeTest : Node
{
	private enum ProbeColorClass
	{
		Red,
		Green,
		Blue,
		Yellow
	}

	private sealed class DispatcherBatchResult
	{
		public long AppliedUploadDelta { get; }

		public long AppliedVisibilityDelta { get; }

		public long BufferRidRefreshDelta { get; }

		public long BufferUpdateFailureDelta { get; }

		public long StaleGenerationDropDelta { get; }

		public long FailedBatchDelta { get; }

		public DispatcherBatchResult(AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot before, AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot after)
		{
			AppliedUploadDelta = after.AppliedUploadCount - before.AppliedUploadCount;
			AppliedVisibilityDelta = after.AppliedVisibilityCount - before.AppliedVisibilityCount;
			BufferRidRefreshDelta = after.BufferRidRefreshCount - before.BufferRidRefreshCount;
			BufferUpdateFailureDelta = after.BufferUpdateFailureCount - before.BufferUpdateFailureCount;
			StaleGenerationDropDelta = after.StaleGenerationDropCount - before.StaleGenerationDropCount;
			FailedBatchDelta = after.FailedBatchCount - before.FailedBatchCount;
		}
	}

	private sealed class RenderThreadUpdateResult
	{
		public bool ExecutedOnRenderThread { get; set; }

		public bool RenderingDeviceAvailable { get; set; }

		public bool BufferRidValid { get; set; }

		public ulong BufferRidId { get; set; }

		public Error UpdateError { get; set; } = Error.Failed;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateProbeScene = "CreateProbeScene";

		public static readonly StringName CreateZInterleaveProbe = "CreateZInterleaveProbe";

		public static readonly StringName CreateSingleInstanceMultiMesh = "CreateSingleInstanceMultiMesh";

		public static readonly StringName RebuildCapacity = "RebuildCapacity";

		public static readonly StringName CreateBaselineActivePrefix = "CreateBaselineActivePrefix";

		public static readonly StringName CreateInstanceRecord = "CreateInstanceRecord";

		public static readonly StringName CaptureCurrentImage = "CaptureCurrentImage";

		public static readonly StringName IsRegionColor = "IsRegionColor";

		public static readonly StringName IsRegionEmpty = "IsRegionEmpty";

		public static readonly StringName VerifyZInterleave = "VerifyZInterleave";

		public static readonly StringName IsZSampleColor = "IsZSampleColor";

		public static readonly StringName IsSignalPixel = "IsSignalPixel";

		public static readonly StringName MatchesColorClass = "MatchesColorClass";

		public static readonly StringName Require = "Require";

		public static readonly StringName SanitizeResultValue = "SanitizeResultValue";

		public static readonly StringName DisposeProbeScene = "DisposeProbeScene";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _probeViewport = "_probeViewport";

		public static readonly StringName _multiMeshInstance = "_multiMeshInstance";

		public static readonly StringName _multiMesh = "_multiMesh";

		public static readonly StringName _quadMesh = "_quadMesh";

		public static readonly StringName _shaderMaterial = "_shaderMaterial";

		public static readonly StringName _shader = "_shader";

		public static readonly StringName _zLowInstance = "_zLowInstance";

		public static readonly StringName _zHighInstance = "_zHighInstance";

		public static readonly StringName _zLowMultiMesh = "_zLowMultiMesh";

		public static readonly StringName _zHighMultiMesh = "_zHighMultiMesh";

		public static readonly StringName _zLowQuadMesh = "_zLowQuadMesh";

		public static readonly StringName _zHighQuadMesh = "_zHighQuadMesh";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "RENDERING_DEVICE_MULTIMESH_PARTIAL_UPDATE_RESULT";

	private const int InstanceStrideFloats = 16;

	private const int InstanceStrideBytes = 64;

	private const uint SecondInstanceByteOffset = 64u;

	private const int InitialCapacity = 4;

	private const int RebuiltCapacity = 8;

	private const int StabilizationFrameCount = 3;

	private const int RenderThreadTimeoutMilliseconds = 10000;

	private const int DispatcherCompletionTimeoutFrames = 120;

	private const int MinimumMatchingPixelCount = 256;

	private const int MinimumZSamplePixelCount = 16;

	private const int MaximumUnexpectedPixelCount = 8;

	private const long InitialBufferGeneration = 1L;

	private const long RebuiltBufferGeneration = 2L;

	private const long DispatcherFirstFrameVersion = 900001L;

	private static readonly Vector2I ProbeViewportSize = new Vector2I(800, 300);

	private static readonly Color BackgroundColor = new Color(0.015f, 0.015f, 0.02f);

	private static readonly Vector2 FirstInstancePosition = new Vector2(120f, 140f);

	private static readonly Vector2 BaselineSecondPosition = new Vector2(300f, 140f);

	private static readonly Vector2 FirstUpdatedSecondPosition = new Vector2(500f, 140f);

	private static readonly Vector2 RebuiltUpdatedSecondPosition = new Vector2(680f, 140f);

	private static readonly Vector2 ZLowInstancePosition = new Vector2(400f, 245f);

	private static readonly Vector2 ZHighInstancePosition = new Vector2(415f, 245f);

	private static readonly Vector2 ZLowSamplePosition = new Vector2(374f, 245f);

	private static readonly Vector2 ZMiddleSamplePosition = new Vector2(394f, 245f);

	private static readonly Vector2 ZHighSamplePosition = new Vector2(415f, 245f);

	private static readonly Color FirstInstanceColor = new Color(0.35f, 1f, 0.35f);

	private static readonly Color FirstInstanceCustomData = new Color(0.25f, 1f, 0.25f);

	private static readonly Color BaselineSecondColor = new Color(1f, 0.55f, 0.55f);

	private static readonly Color BaselineSecondCustomData = new Color(1f, 0.15f, 0.15f);

	private static readonly Color FirstUpdatedSecondColor = new Color(1f, 0.55f, 0.55f);

	private static readonly Color FirstUpdatedSecondCustomData = new Color(0.1f, 0.1f, 1f);

	private static readonly Color RebuiltUpdatedSecondColor = new Color(1f, 1f, 0.3f);

	private static readonly Color RebuiltUpdatedSecondCustomData = new Color(1f, 0.85f, 0.1f);

	private SubViewport _probeViewport;

	private MultiMeshInstance2D _multiMeshInstance;

	private MultiMesh _multiMesh;

	private QuadMesh _quadMesh;

	private ShaderMaterial _shaderMaterial;

	private Shader _shader;

	private MultiMeshInstance2D _zLowInstance;

	private MultiMeshInstance2D _zHighInstance;

	private MultiMesh _zLowMultiMesh;

	private MultiMesh _zHighMultiMesh;

	private QuadMesh _zLowQuadMesh;

	private QuadMesh _zHighQuadMesh;

	private readonly AdobeAnimateMultiMeshRdUploadDispatcher.GenerationToken _dispatcherGenerationToken = new AdobeAnimateMultiMeshRdUploadDispatcher.GenerationToken();

	public override async void _Ready()
	{
		bool passed = false;
		int exitCode = 2;
		string failure = "none";
		bool initialBaselinePassed = false;
		bool zInterleavePassed = false;
		bool firstPartialUpdatePassed = false;
		bool sameBatchPublicationPassed = false;
		bool visibleHidePassed = false;
		bool visibleRestorePassed = false;
		bool visibleRestoreUsedBaselineRecord = false;
		bool rebuildBaselinePassed = false;
		bool rebuiltPartialUpdatePassed = false;
		bool staleGenerationPassed = false;
		RenderThreadUpdateResult initialBaselineUploadResult = null;
		RenderThreadUpdateResult firstUpdateResult = null;
		RenderThreadUpdateResult rebuildBaselineUploadResult = null;
		DispatcherBatchResult firstPublicationResult = null;
		DispatcherBatchResult hidePublicationResult = null;
		DispatcherBatchResult restorePublicationResult = null;
		DispatcherBatchResult rebuiltPublicationResult = null;
		DispatcherBatchResult stalePublicationResult = null;
		try
		{
			ProcessMode = ProcessModeEnum.Always;
			CreateProbeScene();
			byte[] data = CreateBaselineActivePrefix();
			initialBaselineUploadResult = await UpdateMultiMeshBufferOnRenderThread(data, 0u);
			RequireSuccessfulRenderThreadUpdate(initialBaselineUploadResult, "初始 RD 基线上传");
			_multiMesh.VisibleInstanceCount = 2;
			await WaitForRenderedFrames(3);
			using (Image image = CaptureCurrentImage())
			{
				initialBaselinePassed = IsRegionColor(image, FirstInstancePosition, ProbeColorClass.Green) && IsRegionColor(image, BaselineSecondPosition, ProbeColorClass.Red) && IsRegionEmpty(image, FirstUpdatedSecondPosition) && IsRegionEmpty(image, RebuiltUpdatedSecondPosition);
				zInterleavePassed = VerifyZInterleave(image);
			}
			Require(initialBaselinePassed, "初始 MultiMesh 像素基线未通过");
			Require(zInterleavePassed, "Z=0 MultiMesh、Z=1 CanvasItem 与 Z=2 MultiMesh 的像素插层未通过");
			byte[] firstUpdatedRecord = CreateInstanceRecord(FirstUpdatedSecondPosition, FirstUpdatedSecondColor, FirstUpdatedSecondCustomData);
			firstUpdateResult = await UpdateMultiMeshBufferOnRenderThread(firstUpdatedRecord, 64u);
			RequireSuccessfulRenderThreadUpdate(firstUpdateResult, "首次非零偏移 RD 上传");
			await WaitForRenderedFrames(3);
			using (Image image2 = CaptureCurrentImage())
			{
				firstPartialUpdatePassed = IsRegionColor(image2, FirstInstancePosition, ProbeColorClass.Green) && IsRegionEmpty(image2, BaselineSecondPosition) && IsRegionColor(image2, FirstUpdatedSecondPosition, ProbeColorClass.Blue);
			}
			Require(firstPartialUpdatePassed, "非零偏移 RD 更新没有生成预期蓝色像素");
			firstPublicationResult = await QueueDispatcherBatch(900001L, 1L, firstUpdatedRecord, 64u, 2);
			sameBatchPublicationPassed = firstPublicationResult.AppliedUploadDelta == 1 && firstPublicationResult.AppliedVisibilityDelta == 1 && firstPublicationResult.FailedBatchDelta == 0;
			Require(sameBatchPublicationPassed, "同一 RD 调度批次未先完成 BufferUpdate 再发布 VisibleInstanceCount=2");
			hidePublicationResult = await QueueDispatcherBatch(900002L, 1L, null, 0u, 1);
			Require(hidePublicationResult.AppliedVisibilityDelta == 1 && hidePublicationResult.FailedBatchDelta == 0, "调度器未发布 VisibleInstanceCount 2->1");
			await WaitForRenderedFrames(3);
			using (Image image3 = CaptureCurrentImage())
			{
				visibleHidePassed = IsRegionColor(image3, FirstInstancePosition, ProbeColorClass.Green) && IsRegionEmpty(image3, FirstUpdatedSecondPosition);
			}
			Require(visibleHidePassed, "VisibleInstanceCount 2->1 未隐藏第二个实例");
			restorePublicationResult = await QueueDispatcherBatch(900003L, 1L, null, 0u, 2);
			Require(restorePublicationResult.AppliedVisibilityDelta == 1 && restorePublicationResult.FailedBatchDelta == 0, "调度器未发布 VisibleInstanceCount 1->2");
			await WaitForRenderedFrames(3);
			using (Image image4 = CaptureCurrentImage())
			{
				visibleRestorePassed = IsRegionColor(image4, FirstInstancePosition, ProbeColorClass.Green) && IsRegionColor(image4, FirstUpdatedSecondPosition, ProbeColorClass.Blue);
				visibleRestoreUsedBaselineRecord = IsRegionColor(image4, BaselineSecondPosition, ProbeColorClass.Red);
			}
			Require(visibleRestorePassed, $"VisibleInstanceCount 1->2 未恢复 RD 更新后的实例, baselineRecord={visibleRestoreUsedBaselineRecord}");
			RebuildCapacity();
			await WaitForRenderedFrames(3);
			byte[] data2 = CreateBaselineActivePrefix();
			rebuildBaselineUploadResult = await UpdateMultiMeshBufferOnRenderThread(data2, 0u);
			RequireSuccessfulRenderThreadUpdate(rebuildBaselineUploadResult, "容量重建后 RD 基线上传");
			_multiMesh.VisibleInstanceCount = 2;
			await WaitForRenderedFrames(3);
			using (Image image5 = CaptureCurrentImage())
			{
				rebuildBaselinePassed = IsRegionColor(image5, FirstInstancePosition, ProbeColorClass.Green) && IsRegionColor(image5, BaselineSecondPosition, ProbeColorClass.Red) && IsRegionEmpty(image5, FirstUpdatedSecondPosition);
			}
			Require(rebuildBaselinePassed, "容量重建后的 MultiMesh 像素基线未通过");
			byte[] uploadData = CreateInstanceRecord(RebuiltUpdatedSecondPosition, RebuiltUpdatedSecondColor, RebuiltUpdatedSecondCustomData);
			rebuiltPublicationResult = await QueueDispatcherBatch(900004L, 2L, uploadData, 64u, 2);
			Require(rebuiltPublicationResult.AppliedUploadDelta == 1 && rebuiltPublicationResult.AppliedVisibilityDelta == 1 && rebuiltPublicationResult.BufferRidRefreshDelta >= 1 && rebuiltPublicationResult.FailedBatchDelta == 0, "容量重建后调度器未重新取得 RID 并完成同批发布");
			await WaitForRenderedFrames(3);
			using (Image image6 = CaptureCurrentImage())
			{
				rebuiltPartialUpdatePassed = IsRegionColor(image6, FirstInstancePosition, ProbeColorClass.Green) && IsRegionEmpty(image6, BaselineSecondPosition) && IsRegionEmpty(image6, FirstUpdatedSecondPosition) && IsRegionColor(image6, RebuiltUpdatedSecondPosition, ProbeColorClass.Yellow);
			}
			Require(rebuiltPartialUpdatePassed, "容量重建后重新取得 RID 的局部更新没有生成预期黄色像素");
			byte[] uploadData2 = CreateInstanceRecord(BaselineSecondPosition, BaselineSecondColor, BaselineSecondCustomData);
			stalePublicationResult = await QueueDispatcherBatchThenInvalidateGeneration(900005L, uploadData2, 64u, 0);
			await WaitForRenderedFrames(3);
			using (Image image7 = CaptureCurrentImage())
			{
				staleGenerationPassed = stalePublicationResult.StaleGenerationDropDelta >= 2 && stalePublicationResult.AppliedUploadDelta == 0L && stalePublicationResult.AppliedVisibilityDelta == 0L && stalePublicationResult.FailedBatchDelta == 0L && IsRegionColor(image7, FirstInstancePosition, ProbeColorClass.Green) && IsRegionColor(image7, RebuiltUpdatedSecondPosition, ProbeColorClass.Yellow) && IsRegionEmpty(image7, BaselineSecondPosition);
			}
			Require(staleGenerationPassed, "旧缓冲代际仍然覆盖了新缓冲或可见数");
			passed = initialBaselinePassed & zInterleavePassed & firstPartialUpdatePassed & sameBatchPublicationPassed & visibleHidePassed & visibleRestorePassed & rebuildBaselinePassed & rebuiltPartialUpdatePassed & staleGenerationPassed;
			exitCode = ((!passed) ? 2 : 0);
		}
		catch (Exception ex)
		{
			failure = SanitizeResultValue(ex.GetType().Name + ":" + ex.Message);
		}
		finally
		{
			ulong num = initialBaselineUploadResult?.BufferRidId ?? firstUpdateResult?.BufferRidId ?? 0;
			ulong num2 = rebuildBaselineUploadResult?.BufferRidId ?? 0;
			bool flag = num != 0 && num2 != 0;
			bool value = flag && num != num2;
			string value2 = initialBaselineUploadResult?.UpdateError.ToString() ?? "NotRun";
			string value3 = firstUpdateResult?.UpdateError.ToString() ?? "NotRun";
			string value4 = rebuildBaselineUploadResult?.UpdateError.ToString() ?? "NotRun";
			string value5 = SanitizeResultValue(RenderingServer.GetCurrentRenderingMethod());
			string value6 = SanitizeResultValue(RenderingServer.GetCurrentRenderingDriverName());
			GD.Print($"{"RENDERING_DEVICE_MULTIMESH_PARTIAL_UPDATE_RESULT"} passed={passed} initialBaseline={initialBaselinePassed} zInterleave={zInterleavePassed} firstPartialUpdate={firstPartialUpdatePassed} sameBatchPublication={sameBatchPublicationPassed} visible2To1={visibleHidePassed} visible1To2={visibleRestorePassed} visibleRestoreUsedBaselineRecord={visibleRestoreUsedBaselineRecord} rebuildBaseline={rebuildBaselinePassed} rebuiltPartialUpdate={rebuiltPartialUpdatePassed} staleGeneration={staleGenerationPassed} lifecycleInvalidation={staleGenerationPassed} initialCapacity={4} rebuiltCapacity={8} rawBaselineUploadCalls={((initialBaselineUploadResult != null) ? 1 : 0) + ((rebuildBaselineUploadResult != null) ? 1 : 0)} rawPartialUpdateCalls={((firstUpdateResult != null) ? 1 : 0)} dispatcherAppliedUploads={(firstPublicationResult?.AppliedUploadDelta ?? 0) + (rebuiltPublicationResult?.AppliedUploadDelta ?? 0)} dispatcherAppliedVisibility={(firstPublicationResult?.AppliedVisibilityDelta ?? 0) + (hidePublicationResult?.AppliedVisibilityDelta ?? 0) + (restorePublicationResult?.AppliedVisibilityDelta ?? 0) + (rebuiltPublicationResult?.AppliedVisibilityDelta ?? 0)} staleDropDelta={stalePublicationResult?.StaleGenerationDropDelta ?? 0} partialUpdateOffset={64u} partialUpdateBytes={64} initialBaselineOnRenderThread={initialBaselineUploadResult?.ExecutedOnRenderThread ?? false} firstOnRenderThread={firstUpdateResult?.ExecutedOnRenderThread ?? false} rebuildBaselineOnRenderThread={rebuildBaselineUploadResult?.ExecutedOnRenderThread ?? false} firstBufferRid={num} rebuiltBufferRid={num2} bufferRidReacquired={flag} bufferRidChanged={value} initialBaselineUploadError={value2} firstUpdateError={value3} rebuildBaselineUploadError={value4} dispatcherBufferUpdateFailures={rebuiltPublicationResult?.BufferUpdateFailureDelta ?? 0} bufferReadbacks=0 viewportImageCaptures=7 syncCalls=0 multimeshSetBufferCalls=0 renderer={value5} driver={value6} failure={failure}");
			DisposeProbeScene();
			GetTree().Quit(exitCode);
		}
	}

	private void CreateProbeScene()
	{
		_probeViewport = new SubViewport();
		_probeViewport.Name = "RenderingDeviceMultiMeshProbeViewport";
		_probeViewport.Size = ProbeViewportSize;
		_probeViewport.Disable3D = true;
		_probeViewport.TransparentBg = false;
		_probeViewport.RenderTargetUpdateMode = SubViewport.UpdateMode.Always;
		AddChild(_probeViewport, forceReadableName: false, InternalMode.Disabled);
		ColorRect colorRect = new ColorRect();
		colorRect.Name = "ProbeBackground";
		colorRect.Color = BackgroundColor;
		colorRect.Position = Vector2.Zero;
		colorRect.Size = ProbeViewportSize;
		colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
		colorRect.ZIndex = -10;
		_probeViewport.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		_shader = new Shader();
		_shader.Code = "shader_type canvas_item;\nrender_mode unshaded, blend_mix;\n\nvarying vec4 probe_instance_color;\n\nvoid vertex() {\n\tprobe_instance_color = COLOR * INSTANCE_CUSTOM;\n}\n\nvoid fragment() {\n\tCOLOR = probe_instance_color;\n}";
		_shaderMaterial = new ShaderMaterial();
		_shaderMaterial.Shader = _shader;
		_quadMesh = new QuadMesh();
		_quadMesh.Size = new Vector2(56f, 56f);
		_multiMesh = new MultiMesh();
		_multiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform2D;
		_multiMesh.UseColors = true;
		_multiMesh.UseCustomData = true;
		_multiMesh.Mesh = _quadMesh;
		_multiMesh.CustomAabb = new Aabb(new Vector3(0f, 0f, -1f), new Vector3(ProbeViewportSize.X, ProbeViewportSize.Y, 2f));
		_multiMesh.InstanceCount = 4;
		_multiMesh.VisibleInstanceCount = 0;
		Require(_dispatcherGenerationToken.Advance() == 1, "探针初始缓冲代际不正确");
		_multiMeshInstance = new MultiMeshInstance2D();
		_multiMeshInstance.Name = "RenderingDeviceMultiMeshProbe";
		_multiMeshInstance.Multimesh = _multiMesh;
		_multiMeshInstance.Material = _shaderMaterial;
		_multiMeshInstance.ZIndex = 1;
		_probeViewport.AddChild(_multiMeshInstance, forceReadableName: false, InternalMode.Disabled);
		CreateZInterleaveProbe();
	}

	private void CreateZInterleaveProbe()
	{
		_zLowQuadMesh = new QuadMesh();
		_zLowQuadMesh.Size = new Vector2(80f, 80f);
		_zLowMultiMesh = CreateSingleInstanceMultiMesh(_zLowQuadMesh, ZLowInstancePosition, new Color(0.1f, 1f, 0.1f));
		_zLowInstance = new MultiMeshInstance2D();
		_zLowInstance.Name = "RenderingDeviceZLowMultiMesh";
		_zLowInstance.Multimesh = _zLowMultiMesh;
		_zLowInstance.Material = _shaderMaterial;
		_zLowInstance.ZAsRelative = false;
		_zLowInstance.ZIndex = 0;
		_probeViewport.AddChild(_zLowInstance, forceReadableName: false, InternalMode.Disabled);
		ColorRect colorRect = new ColorRect();
		colorRect.Name = "RenderingDeviceZMiddleCanvasItem";
		colorRect.Color = new Color(1f, 0.08f, 0.08f);
		colorRect.Position = new Vector2(390f, 215f);
		colorRect.Size = new Vector2(60f, 60f);
		colorRect.MouseFilter = Control.MouseFilterEnum.Ignore;
		colorRect.ZAsRelative = false;
		colorRect.ZIndex = 1;
		_probeViewport.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		_zHighQuadMesh = new QuadMesh();
		_zHighQuadMesh.Size = new Vector2(30f, 30f);
		_zHighMultiMesh = CreateSingleInstanceMultiMesh(_zHighQuadMesh, ZHighInstancePosition, new Color(0.08f, 0.08f, 1f));
		_zHighInstance = new MultiMeshInstance2D();
		_zHighInstance.Name = "RenderingDeviceZHighMultiMesh";
		_zHighInstance.Multimesh = _zHighMultiMesh;
		_zHighInstance.Material = _shaderMaterial;
		_zHighInstance.ZAsRelative = false;
		_zHighInstance.ZIndex = 2;
		_probeViewport.AddChild(_zHighInstance, forceReadableName: false, InternalMode.Disabled);
	}

	private static MultiMesh CreateSingleInstanceMultiMesh(QuadMesh mesh, Vector2 position, Color color)
	{
		MultiMesh multiMesh = new MultiMesh();
		multiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform2D;
		multiMesh.UseColors = true;
		multiMesh.UseCustomData = true;
		multiMesh.Mesh = mesh;
		multiMesh.CustomAabb = new Aabb(new Vector3(0f, 0f, -1f), new Vector3(ProbeViewportSize.X, ProbeViewportSize.Y, 2f));
		multiMesh.InstanceCount = 1;
		multiMesh.SetInstanceTransform2D(0, new Transform2D(0f, position));
		multiMesh.SetInstanceColor(0, color);
		multiMesh.SetInstanceCustomData(0, Colors.White);
		multiMesh.VisibleInstanceCount = 1;
		return multiMesh;
	}

	private void RebuildCapacity()
	{
		_multiMesh.VisibleInstanceCount = 0;
		_multiMesh.InstanceCount = 8;
		Require(_dispatcherGenerationToken.Advance() == 2, "探针容量重建后的缓冲代际不正确");
	}

	private static byte[] CreateBaselineActivePrefix()
	{
		byte[] src = CreateInstanceRecord(FirstInstancePosition, FirstInstanceColor, FirstInstanceCustomData);
		byte[] src2 = CreateInstanceRecord(BaselineSecondPosition, BaselineSecondColor, BaselineSecondCustomData);
		byte[] array = new byte[128];
		Buffer.BlockCopy(src, 0, array, 0, 64);
		Buffer.BlockCopy(src2, 0, array, 64, 64);
		return array;
	}

	private async Task<DispatcherBatchResult> QueueDispatcherBatch(long frameVersion, long bufferGeneration, byte[] uploadData, uint destinationByteOffset, int visibleInstanceCount)
	{
		AdobeAnimateMultiMeshRdUploadDispatcher shared = AdobeAnimateMultiMeshRdUploadDispatcher.Shared;
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot before = shared.Statistics;
		AdobeAnimateMultiMeshRdUploadDispatcher.Batch batch = shared.BeginBatch(frameVersion);
		if (uploadData != null)
		{
			Require(batch.TryQueueUpload(_multiMesh, _dispatcherGenerationToken, bufferGeneration, destinationByteOffset, uploadData), $"调度器拒绝了帧 {frameVersion} 的上传命令");
		}
		Require(batch.TryQueueVisibility(_multiMesh, _dispatcherGenerationToken, bufferGeneration, visibleInstanceCount), $"调度器拒绝了帧 {frameVersion} 的可见数命令");
		Require(batch.EndBatch(), $"调度器未能封结帧 {frameVersion} 的批次");
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot after = await WaitForDispatcherBatch(frameVersion, before.CurrentQueueDepth);
		return new DispatcherBatchResult(before, after);
	}

	private async Task<DispatcherBatchResult> QueueDispatcherBatchThenInvalidateGeneration(long frameVersion, byte[] uploadData, uint destinationByteOffset, int visibleInstanceCount)
	{
		AdobeAnimateMultiMeshRdUploadDispatcher shared = AdobeAnimateMultiMeshRdUploadDispatcher.Shared;
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot before = shared.Statistics;
		long currentGeneration = _dispatcherGenerationToken.CurrentGeneration;
		AdobeAnimateMultiMeshRdUploadDispatcher.Batch batch = shared.BeginBatch(frameVersion);
		Require(batch.TryQueueUpload(_multiMesh, _dispatcherGenerationToken, currentGeneration, destinationByteOffset, uploadData), $"调度器拒绝了帧 {frameVersion} 的生命周期竞态上传命令");
		Require(batch.TryQueueVisibility(_multiMesh, _dispatcherGenerationToken, currentGeneration, visibleInstanceCount), $"调度器拒绝了帧 {frameVersion} 的生命周期竞态可见数命令");
		Require(_dispatcherGenerationToken.Advance() == currentGeneration + 1, "生命周期推进没有使已捕获命令立即失效");
		Require(batch.EndBatch(), $"调度器未能封结帧 {frameVersion} 的生命周期竞态批次");
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot after = await WaitForDispatcherBatch(frameVersion, before.CurrentQueueDepth);
		return new DispatcherBatchResult(before, after);
	}

	private async Task<AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot> WaitForDispatcherBatch(long frameVersion, long queueDepthBefore)
	{
		for (int frame = 0; frame < 120; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot statistics = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
			if (statistics.LastAppliedFrameVersion >= frameVersion && statistics.CurrentQueueDepth <= queueDepthBefore)
			{
				return statistics;
			}
		}
		throw new TimeoutException($"等待 RD 调度批次 {frameVersion} 完成超时");
	}

	private async Task<RenderThreadUpdateResult> UpdateMultiMeshBufferOnRenderThread(byte[] data, uint destinationByteOffset)
	{
		if (data == null || data.Length == 0 || data.Length % 64 != 0)
		{
			throw new ArgumentException($"上传数据长度必须为 {64} 的正整数倍", "data");
		}
		uint num;
		uint num2;
		checked
		{
			num = (uint)(_multiMesh.InstanceCount * 64);
			num2 = (uint)data.Length;
		}
		if (num2 > num || destinationByteOffset > num - num2)
		{
			throw new ArgumentOutOfRangeException("destinationByteOffset", "上传区间超出当前 MultiMesh GPU 容量");
		}
		TaskCompletionSource<RenderThreadUpdateResult> completionSource = new TaskCompletionSource<RenderThreadUpdateResult>(TaskCreationOptions.RunContinuationsAsynchronously);
		Rid multiMeshRid = _multiMesh.GetRid();
		RenderingServer.CallOnRenderThread(Callable.From(() =>
		{
			ExecuteRenderThreadUpdate(multiMeshRid, data, destinationByteOffset, completionSource);
		}));
		if (await Task.WhenAny(completionSource.Task, Task.Delay(10000)) != completionSource.Task)
		{
			throw new TimeoutException("等待 RenderingServer.CallOnRenderThread 超时");
		}
		return await completionSource.Task;
	}

	private static void ExecuteRenderThreadUpdate(Rid multiMeshRid, byte[] data, uint destinationByteOffset, TaskCompletionSource<RenderThreadUpdateResult> completionSource)
	{
		RenderThreadUpdateResult renderThreadUpdateResult = new RenderThreadUpdateResult();
		try
		{
			renderThreadUpdateResult.ExecutedOnRenderThread = RenderingServer.IsOnRenderThread();
			RenderingDevice renderingDevice = RenderingServer.GetRenderingDevice();
			renderThreadUpdateResult.RenderingDeviceAvailable = GodotObject.IsInstanceValid(renderingDevice);
			if (!renderThreadUpdateResult.RenderingDeviceAvailable)
			{
				completionSource.TrySetResult(renderThreadUpdateResult);
				return;
			}
			Rid buffer = RenderingServer.MultimeshGetBufferRdRid(multiMeshRid);
			renderThreadUpdateResult.BufferRidId = buffer.Id;
			renderThreadUpdateResult.BufferRidValid = buffer.IsValid;
			if (!renderThreadUpdateResult.BufferRidValid)
			{
				completionSource.TrySetResult(renderThreadUpdateResult);
				return;
			}
			renderThreadUpdateResult.UpdateError = renderingDevice.BufferUpdate(buffer, destinationByteOffset, checked((uint)data.Length), data);
			completionSource.TrySetResult(renderThreadUpdateResult);
		}
		catch (Exception exception)
		{
			completionSource.TrySetException(exception);
		}
	}

	private static void RequireSuccessfulRenderThreadUpdate(RenderThreadUpdateResult result, string stage)
	{
		Require(result != null, stage + ": 未返回结果");
		Require(result.RenderingDeviceAvailable, stage + ": 全局 RenderingDevice 不可用");
		Require(result.ExecutedOnRenderThread, stage + ": BufferUpdate 未在渲染线程执行");
		Require(result.BufferRidValid, stage + ": MultiMesh 底层 RD 缓冲 RID 无效");
		Require(result.UpdateError == Error.Ok, $"{stage}: BufferUpdate 失败 {result.UpdateError}");
	}

	private static byte[] CreateInstanceRecord(Vector2 position, Color color, Color customData)
	{
		float[] src = new float[16]
		{
			1f, 0f, 0f, position.X, 0f, 1f, 0f, position.Y, color.R, color.G,
			color.B, color.A, customData.R, customData.G, customData.B, customData.A
		};
		byte[] array = new byte[64];
		Buffer.BlockCopy(src, 0, array, 0, 64);
		return array;
	}

	private async Task WaitForRenderedFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		}
	}

	private Image CaptureCurrentImage()
	{
		ViewportTexture texture = _probeViewport.GetTexture();
		if (!GodotObject.IsInstanceValid(texture))
		{
			throw new InvalidOperationException("探针视口纹理不可用");
		}
		Image image = texture.GetImage();
		if (!GodotObject.IsInstanceValid(image) || image.IsEmpty())
		{
			image?.Dispose();
			throw new InvalidOperationException("探针视口没有生成可读的完整帧");
		}
		return image;
	}

	private static bool IsRegionColor(Image image, Vector2 center, ProbeColorClass expectedColor)
	{
		return CountRegionPixels(image, center, (Color color) => MatchesColorClass(color, expectedColor)) >= 256;
	}

	private static bool IsRegionEmpty(Image image, Vector2 center)
	{
		return CountRegionPixels(image, center, IsSignalPixel) <= 8;
	}

	private static bool VerifyZInterleave(Image image)
	{
		if (IsZSampleColor(image, ZLowSamplePosition, ProbeColorClass.Green) && IsZSampleColor(image, ZMiddleSamplePosition, ProbeColorClass.Red))
		{
			return IsZSampleColor(image, ZHighSamplePosition, ProbeColorClass.Blue);
		}
		return false;
	}

	private static bool IsZSampleColor(Image image, Vector2 center, ProbeColorClass expectedColor)
	{
		return CountPixelsAround(image, center, 3, (Color color) => MatchesColorClass(color, expectedColor)) >= 16;
	}

	private static int CountRegionPixels(Image image, Vector2 center, Func<Color, bool> predicate)
	{
		return CountPixelsAround(image, center, 20, predicate);
	}

	private static int CountPixelsAround(Image image, Vector2 center, int halfExtent, Func<Color, bool> predicate)
	{
		int num = Mathf.RoundToInt(center.X);
		int num2 = Mathf.RoundToInt(center.Y);
		int num3 = Math.Max(0, num - halfExtent);
		int num4 = Math.Min(image.GetWidth(), num + halfExtent);
		int num5 = Math.Max(0, num2 - halfExtent);
		int num6 = Math.Min(image.GetHeight(), num2 + halfExtent);
		int num7 = 0;
		for (int i = num5; i < num6; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (predicate(image.GetPixel(j, i)))
				{
					num7++;
				}
			}
		}
		return num7;
	}

	private static bool IsSignalPixel(Color color)
	{
		float num = Mathf.Abs(color.R - BackgroundColor.R) + Mathf.Abs(color.G - BackgroundColor.G) + Mathf.Abs(color.B - BackgroundColor.B);
		if (color.A > 0.8f)
		{
			return num > 0.18f;
		}
		return false;
	}

	private static bool MatchesColorClass(Color color, ProbeColorClass expectedColor)
	{
		if (color.A < 0.8f)
		{
			return false;
		}
		switch (expectedColor)
		{
		case ProbeColorClass.Red:
			if (color.R > color.G + 0.25f)
			{
				return color.R > color.B + 0.25f;
			}
			return false;
		case ProbeColorClass.Green:
			if (color.G > color.R + 0.25f)
			{
				return color.G > color.B + 0.25f;
			}
			return false;
		case ProbeColorClass.Blue:
			if (color.B > color.R + 0.25f)
			{
				return color.B > color.G + 0.25f;
			}
			return false;
		default:
			if (color.R > 0.45f && color.G > 0.35f)
			{
				return color.B < Math.Min(color.R, color.G) * 0.55f;
			}
			return false;
		}
	}

	private static void Require(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
	}

	private static string SanitizeResultValue(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return "none";
		}
		return value.Replace('\r', ' ').Replace('\n', ' ').Replace(' ', '_');
	}

	private void DisposeProbeScene()
	{
		if (GodotObject.IsInstanceValid(_zLowInstance))
		{
			_zLowInstance.Multimesh = null;
			_zLowInstance.Material = null;
			_zLowInstance.QueueFree();
		}
		_zLowInstance = null;
		if (GodotObject.IsInstanceValid(_zHighInstance))
		{
			_zHighInstance.Multimesh = null;
			_zHighInstance.Material = null;
			_zHighInstance.QueueFree();
		}
		_zHighInstance = null;
		if (GodotObject.IsInstanceValid(_zLowMultiMesh))
		{
			_zLowMultiMesh.Mesh = null;
			_zLowMultiMesh.Dispose();
		}
		_zLowMultiMesh = null;
		if (GodotObject.IsInstanceValid(_zHighMultiMesh))
		{
			_zHighMultiMesh.Mesh = null;
			_zHighMultiMesh.Dispose();
		}
		_zHighMultiMesh = null;
		if (GodotObject.IsInstanceValid(_zLowQuadMesh))
		{
			_zLowQuadMesh.Dispose();
		}
		_zLowQuadMesh = null;
		if (GodotObject.IsInstanceValid(_zHighQuadMesh))
		{
			_zHighQuadMesh.Dispose();
		}
		_zHighQuadMesh = null;
		if (GodotObject.IsInstanceValid(_multiMeshInstance))
		{
			_multiMeshInstance.Multimesh = null;
			_multiMeshInstance.Material = null;
			_multiMeshInstance.QueueFree();
		}
		_multiMeshInstance = null;
		if (GodotObject.IsInstanceValid(_multiMesh))
		{
			_multiMesh.Mesh = null;
			_multiMesh.Dispose();
		}
		_multiMesh = null;
		if (GodotObject.IsInstanceValid(_quadMesh))
		{
			_quadMesh.Dispose();
		}
		_quadMesh = null;
		if (GodotObject.IsInstanceValid(_shaderMaterial))
		{
			_shaderMaterial.Shader = null;
			_shaderMaterial.Dispose();
		}
		_shaderMaterial = null;
		if (GodotObject.IsInstanceValid(_shader))
		{
			_shader.Dispose();
		}
		_shader = null;
		if (GodotObject.IsInstanceValid(_probeViewport))
		{
			_probeViewport.QueueFree();
		}
		_probeViewport = null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateProbeScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateZInterleaveProbe, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSingleInstanceMultiMesh, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MultiMesh"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mesh", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("QuadMesh"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBaselineActivePrefix, new PropertyInfo(Variant.Type.PackedByteArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateInstanceRecord, new PropertyInfo(Variant.Type.PackedByteArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "customData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureCurrentImage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRegionColor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRegionEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.VerifyZInterleave, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsZSampleColor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSignalPixel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MatchesColorClass, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SanitizeResultValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeProbeScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.CreateProbeScene && args.Count == 0)
		{
			CreateProbeScene();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateZInterleaveProbe && args.Count == 0)
		{
			CreateZInterleaveProbe();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSingleInstanceMultiMesh && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<MultiMesh>(CreateSingleInstanceMultiMesh(VariantUtils.ConvertTo<QuadMesh>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.RebuildCapacity && args.Count == 0)
		{
			RebuildCapacity();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBaselineActivePrefix && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<byte[]>(CreateBaselineActivePrefix());
			return true;
		}
		if (method == MethodName.CreateInstanceRecord && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<byte[]>(CreateInstanceRecord(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.CaptureCurrentImage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Image>(CaptureCurrentImage());
			return true;
		}
		if (method == MethodName.IsRegionColor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRegionColor(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<ProbeColorClass>(in args[2])));
			return true;
		}
		if (method == MethodName.IsRegionEmpty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRegionEmpty(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.VerifyZInterleave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyZInterleave(VariantUtils.ConvertTo<Image>(in args[0])));
			return true;
		}
		if (method == MethodName.IsZSampleColor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZSampleColor(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<ProbeColorClass>(in args[2])));
			return true;
		}
		if (method == MethodName.IsSignalPixel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSignalPixel(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.MatchesColorClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesColorClass(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<ProbeColorClass>(in args[1])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SanitizeResultValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(SanitizeResultValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposeProbeScene && args.Count == 0)
		{
			DisposeProbeScene();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateSingleInstanceMultiMesh && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<MultiMesh>(CreateSingleInstanceMultiMesh(VariantUtils.ConvertTo<QuadMesh>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateBaselineActivePrefix && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<byte[]>(CreateBaselineActivePrefix());
			return true;
		}
		if (method == MethodName.CreateInstanceRecord && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<byte[]>(CreateInstanceRecord(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2])));
			return true;
		}
		if (method == MethodName.IsRegionColor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRegionColor(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<ProbeColorClass>(in args[2])));
			return true;
		}
		if (method == MethodName.IsRegionEmpty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRegionEmpty(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.VerifyZInterleave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyZInterleave(VariantUtils.ConvertTo<Image>(in args[0])));
			return true;
		}
		if (method == MethodName.IsZSampleColor && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZSampleColor(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<ProbeColorClass>(in args[2])));
			return true;
		}
		if (method == MethodName.IsSignalPixel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSignalPixel(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.MatchesColorClass && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesColorClass(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<ProbeColorClass>(in args[1])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SanitizeResultValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(SanitizeResultValue(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.CreateProbeScene)
		{
			return true;
		}
		if (method == MethodName.CreateZInterleaveProbe)
		{
			return true;
		}
		if (method == MethodName.CreateSingleInstanceMultiMesh)
		{
			return true;
		}
		if (method == MethodName.RebuildCapacity)
		{
			return true;
		}
		if (method == MethodName.CreateBaselineActivePrefix)
		{
			return true;
		}
		if (method == MethodName.CreateInstanceRecord)
		{
			return true;
		}
		if (method == MethodName.CaptureCurrentImage)
		{
			return true;
		}
		if (method == MethodName.IsRegionColor)
		{
			return true;
		}
		if (method == MethodName.IsRegionEmpty)
		{
			return true;
		}
		if (method == MethodName.VerifyZInterleave)
		{
			return true;
		}
		if (method == MethodName.IsZSampleColor)
		{
			return true;
		}
		if (method == MethodName.IsSignalPixel)
		{
			return true;
		}
		if (method == MethodName.MatchesColorClass)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		if (method == MethodName.SanitizeResultValue)
		{
			return true;
		}
		if (method == MethodName.DisposeProbeScene)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._probeViewport)
		{
			_probeViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._multiMeshInstance)
		{
			_multiMeshInstance = VariantUtils.ConvertTo<MultiMeshInstance2D>(in value);
			return true;
		}
		if (name == PropertyName._multiMesh)
		{
			_multiMesh = VariantUtils.ConvertTo<MultiMesh>(in value);
			return true;
		}
		if (name == PropertyName._quadMesh)
		{
			_quadMesh = VariantUtils.ConvertTo<QuadMesh>(in value);
			return true;
		}
		if (name == PropertyName._shaderMaterial)
		{
			_shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(in value);
			return true;
		}
		if (name == PropertyName._shader)
		{
			_shader = VariantUtils.ConvertTo<Shader>(in value);
			return true;
		}
		if (name == PropertyName._zLowInstance)
		{
			_zLowInstance = VariantUtils.ConvertTo<MultiMeshInstance2D>(in value);
			return true;
		}
		if (name == PropertyName._zHighInstance)
		{
			_zHighInstance = VariantUtils.ConvertTo<MultiMeshInstance2D>(in value);
			return true;
		}
		if (name == PropertyName._zLowMultiMesh)
		{
			_zLowMultiMesh = VariantUtils.ConvertTo<MultiMesh>(in value);
			return true;
		}
		if (name == PropertyName._zHighMultiMesh)
		{
			_zHighMultiMesh = VariantUtils.ConvertTo<MultiMesh>(in value);
			return true;
		}
		if (name == PropertyName._zLowQuadMesh)
		{
			_zLowQuadMesh = VariantUtils.ConvertTo<QuadMesh>(in value);
			return true;
		}
		if (name == PropertyName._zHighQuadMesh)
		{
			_zHighQuadMesh = VariantUtils.ConvertTo<QuadMesh>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._probeViewport)
		{
			value = VariantUtils.CreateFrom(in _probeViewport);
			return true;
		}
		if (name == PropertyName._multiMeshInstance)
		{
			value = VariantUtils.CreateFrom(in _multiMeshInstance);
			return true;
		}
		if (name == PropertyName._multiMesh)
		{
			value = VariantUtils.CreateFrom(in _multiMesh);
			return true;
		}
		if (name == PropertyName._quadMesh)
		{
			value = VariantUtils.CreateFrom(in _quadMesh);
			return true;
		}
		if (name == PropertyName._shaderMaterial)
		{
			value = VariantUtils.CreateFrom(in _shaderMaterial);
			return true;
		}
		if (name == PropertyName._shader)
		{
			value = VariantUtils.CreateFrom(in _shader);
			return true;
		}
		if (name == PropertyName._zLowInstance)
		{
			value = VariantUtils.CreateFrom(in _zLowInstance);
			return true;
		}
		if (name == PropertyName._zHighInstance)
		{
			value = VariantUtils.CreateFrom(in _zHighInstance);
			return true;
		}
		if (name == PropertyName._zLowMultiMesh)
		{
			value = VariantUtils.CreateFrom(in _zLowMultiMesh);
			return true;
		}
		if (name == PropertyName._zHighMultiMesh)
		{
			value = VariantUtils.CreateFrom(in _zHighMultiMesh);
			return true;
		}
		if (name == PropertyName._zLowQuadMesh)
		{
			value = VariantUtils.CreateFrom(in _zLowQuadMesh);
			return true;
		}
		if (name == PropertyName._zHighQuadMesh)
		{
			value = VariantUtils.CreateFrom(in _zHighQuadMesh);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._probeViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._multiMeshInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._multiMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quadMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shaderMaterial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shader, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zLowInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zHighInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zLowMultiMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zHighMultiMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zLowQuadMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zHighQuadMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._probeViewport, Variant.From(in _probeViewport));
		info.AddProperty(PropertyName._multiMeshInstance, Variant.From(in _multiMeshInstance));
		info.AddProperty(PropertyName._multiMesh, Variant.From(in _multiMesh));
		info.AddProperty(PropertyName._quadMesh, Variant.From(in _quadMesh));
		info.AddProperty(PropertyName._shaderMaterial, Variant.From(in _shaderMaterial));
		info.AddProperty(PropertyName._shader, Variant.From(in _shader));
		info.AddProperty(PropertyName._zLowInstance, Variant.From(in _zLowInstance));
		info.AddProperty(PropertyName._zHighInstance, Variant.From(in _zHighInstance));
		info.AddProperty(PropertyName._zLowMultiMesh, Variant.From(in _zLowMultiMesh));
		info.AddProperty(PropertyName._zHighMultiMesh, Variant.From(in _zHighMultiMesh));
		info.AddProperty(PropertyName._zLowQuadMesh, Variant.From(in _zLowQuadMesh));
		info.AddProperty(PropertyName._zHighQuadMesh, Variant.From(in _zHighQuadMesh));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._probeViewport, out var value))
		{
			_probeViewport = value.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._multiMeshInstance, out var value2))
		{
			_multiMeshInstance = value2.As<MultiMeshInstance2D>();
		}
		if (info.TryGetProperty(PropertyName._multiMesh, out var value3))
		{
			_multiMesh = value3.As<MultiMesh>();
		}
		if (info.TryGetProperty(PropertyName._quadMesh, out var value4))
		{
			_quadMesh = value4.As<QuadMesh>();
		}
		if (info.TryGetProperty(PropertyName._shaderMaterial, out var value5))
		{
			_shaderMaterial = value5.As<ShaderMaterial>();
		}
		if (info.TryGetProperty(PropertyName._shader, out var value6))
		{
			_shader = value6.As<Shader>();
		}
		if (info.TryGetProperty(PropertyName._zLowInstance, out var value7))
		{
			_zLowInstance = value7.As<MultiMeshInstance2D>();
		}
		if (info.TryGetProperty(PropertyName._zHighInstance, out var value8))
		{
			_zHighInstance = value8.As<MultiMeshInstance2D>();
		}
		if (info.TryGetProperty(PropertyName._zLowMultiMesh, out var value9))
		{
			_zLowMultiMesh = value9.As<MultiMesh>();
		}
		if (info.TryGetProperty(PropertyName._zHighMultiMesh, out var value10))
		{
			_zHighMultiMesh = value10.As<MultiMesh>();
		}
		if (info.TryGetProperty(PropertyName._zLowQuadMesh, out var value11))
		{
			_zLowQuadMesh = value11.As<QuadMesh>();
		}
		if (info.TryGetProperty(PropertyName._zHighQuadMesh, out var value12))
		{
			_zHighQuadMesh = value12.As<QuadMesh>();
		}
	}
}
