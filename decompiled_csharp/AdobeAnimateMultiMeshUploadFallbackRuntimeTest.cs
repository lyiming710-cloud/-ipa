using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateMultiMeshUploadFallbackRuntimeTest.cs")]
public class AdobeAnimateMultiMeshUploadFallbackRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Record = "Record";

		public static readonly StringName Submit = "Submit";

		public static readonly StringName CheckColor = "CheckColor";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _viewport = "_viewport";

		public static readonly StringName _multiMesh = "_multiMesh";

		public static readonly StringName _mesh = "_mesh";

		public static readonly StringName _frameVersion = "_frameVersion";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly AdobeAnimateMultiMeshRdUploadDispatcher.GenerationToken _generation = new AdobeAnimateMultiMeshRdUploadDispatcher.GenerationToken();

	private SubViewport _viewport;

	private MultiMesh _multiMesh;

	private QuadMesh _mesh;

	private long _frameVersion = 1000000L;

	public override async void _Ready()
	{
		int exitCode = 2;
		try
		{
			_ = 8;
			try
			{
				_viewport = new SubViewport
				{
					Size = new Vector2I(400, 160),
					TransparentBg = true,
					RenderTargetUpdateMode = SubViewport.UpdateMode.Always
				};
				AddChild(_viewport, forceReadableName: false, InternalMode.Disabled);
				_mesh = new QuadMesh
				{
					Size = new Vector2(32f, 32f)
				};
				_multiMesh = new MultiMesh
				{
					TransformFormat = MultiMesh.TransformFormatEnum.Transform2D,
					UseColors = true,
					UseCustomData = true,
					Mesh = _mesh,
					InstanceCount = 4,
					VisibleInstanceCount = 0
				};
				_viewport.AddChild(new MultiMeshInstance2D
				{
					Multimesh = _multiMesh
				}, forceReadableName: false, InternalMode.Disabled);
				_generation.Advance();
				AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot before = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
				float[] array = new float[32];
				Record(80f, Colors.Green).CopyTo(array, 0);
				Record(200f, Colors.Red).CopyTo(array, 16);
				Submit(array, 0u, 2);
				await VerifyPixels(80, Colors.Green, 200, Colors.Red);
				Submit(Record(300f, Colors.Blue), 64u, 2);
				await VerifyPixels(80, Colors.Green, 300, Colors.Blue, 200);
				Submit(null, 0u, 1);
				await VerifyPixels(80, Colors.Green, 80, Colors.Green, 300);
				Submit(null, 0u, 2);
				await VerifyPixels(80, Colors.Green, 300, Colors.Blue);
				using (AdobeAnimateMultiMeshRdUploadDispatcher.Batch batch = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.BeginBatch(++_frameVersion))
				{
					long currentGeneration = _generation.CurrentGeneration;
					Require(batch.TryQueueUpload(_multiMesh, _generation, currentGeneration, 64u, MemoryMarshal.AsBytes(Record(200f, Colors.Red).AsSpan())), "旧代际测试上传未入队。");
					Require(batch.TryQueueVisibility(_multiMesh, _generation, currentGeneration, 0), "旧代际隐藏命令未入队。");
					_generation.Advance();
					Require(batch.EndBatch(), "旧代际测试批次未提交。");
				}
				await VerifyPixels(80, Colors.Green, 300, Colors.Blue, 200);
				_multiMesh.InstanceCount = 0;
				_multiMesh.InstanceCount = 4;
				_generation.Advance();
				Submit(Record(80f, Colors.Green), 0u, 2);
				await VerifyPixels(80, Colors.Green, 80, Colors.Green, 300);
				_multiMesh.InstanceCount = 8;
				_generation.Advance();
				Submit(Record(80f, Colors.Green), 0u, 2);
				await VerifyPixels(80, Colors.Green, 80, Colors.Green, 300);
				Submit(Record(240f, Colors.Yellow), 64u, 2);
				await VerifyPixels(80, Colors.Green, 240, Colors.Yellow);
				Submit(null, 0u, 0);
				await VerifyPixels(80, Colors.Transparent, 240, Colors.Transparent);
				AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot statistics = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
				Require(statistics.AppliedUploadCount - before.AppliedUploadCount == 5, "实际上传次数不符。");
				Require(statistics.StaleGenerationDropCount - before.StaleGenerationDropCount == 2, "旧代际上传和隐藏命令没有同时淘汰。");
				Require(statistics.FailedBatchCount == before.FailedBatchCount && statistics.InvalidTargetCount == before.InvalidTargetCount, "上传过程中出现失败批次或失效目标。");
				Require((!(RenderingServer.GetCurrentRenderingMethod() == "gl_compatibility")) ? (statistics.RenderingDeviceUnavailableCount == before.RenderingDeviceUnavailableCount && statistics.BufferRidRefreshCount > before.BufferRidRefreshCount) : (statistics.RenderingDeviceUnavailableCount > before.RenderingDeviceUnavailableCount && statistics.BufferRidRefreshCount == before.BufferRidRefreshCount), "没有选择正确的设备上传路径。");
				GD.Print("ADOBE_ANIMATE_MULTIMESH_UPLOAD_FALLBACK_RESULT passed=True partial=True visibility=True recreated=True resized=True staleDrops=2 uploads=5 renderer=" + RenderingServer.GetCurrentRenderingMethod());
				exitCode = 0;
			}
			catch (Exception value)
			{
				GD.PrintErr($"ADOBE_ANIMATE_MULTIMESH_UPLOAD_FALLBACK_RESULT passed=False exception={value}");
			}
		}
		finally
		{
			_generation.Advance();
			if (GodotObject.IsInstanceValid(_viewport))
			{
				_viewport.QueueFree();
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			_multiMesh?.Dispose();
			_mesh?.Dispose();
		}
		GetTree().Quit(exitCode);
	}

	private static float[] Record(float x, Color color)
	{
		float[] array = new float[16]
		{
			1f, 0f, 0f, 0f, 0f, 1f, 0f, 80f, 0f, 0f,
			0f, 0f, 0f, 0f, 0f, 0f
		};
		array[3] = x;
		array[8] = color.R;
		array[9] = color.G;
		array[10] = color.B;
		array[11] = color.A;
		return array;
	}

	private void Submit(float[] data, uint offset, int visibleCount)
	{
		using AdobeAnimateMultiMeshRdUploadDispatcher.Batch batch = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.BeginBatch(++_frameVersion);
		long currentGeneration = _generation.CurrentGeneration;
		if (data != null)
		{
			Require(batch.TryQueueUpload(_multiMesh, _generation, currentGeneration, offset, MemoryMarshal.AsBytes(data.AsSpan())), "上传命令未入队。");
		}
		Require(batch.TryQueueVisibility(_multiMesh, _generation, currentGeneration, visibleCount), "可见数命令未入队。");
		Require(batch.EndBatch(), "批次未提交。");
	}

	private async Task VerifyPixels(int firstX, Color firstColor, int secondX, Color secondColor, int emptyX = -1)
	{
		for (int frame = 0; frame < 3; frame++)
		{
			await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		}
		using Image image = _viewport.GetTexture().GetImage();
		CheckColor(image, firstX, firstColor);
		CheckColor(image, secondX, secondColor);
		if (emptyX >= 0)
		{
			CheckColor(image, emptyX, Colors.Transparent);
		}
	}

	private static void CheckColor(Image image, int centerX, Color expected)
	{
		for (int i = 76; i <= 84; i++)
		{
			for (int j = centerX - 4; j <= centerX + 4; j++)
			{
				Color pixel = image.GetPixel(j, i);
				Require(Math.Abs(pixel.A - expected.A) < 0.08f && (expected.A == 0f || (Math.Abs(pixel.R - expected.R) < 0.08f && Math.Abs(pixel.G - expected.G) < 0.08f && Math.Abs(pixel.B - expected.B) < 0.08f)), $"像素不符: ({j},{i}) expected={expected} actual={pixel}");
			}
		}
	}

	private static void Require(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Record, new PropertyInfo(Variant.Type.PackedFloat32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Submit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat32Array, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "visibleCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "image", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new PropertyInfo(Variant.Type.Int, "centerX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.Record && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float[]>(Record(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.Submit && args.Count == 3)
		{
			Submit(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<uint>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckColor && args.Count == 3)
		{
			CheckColor(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]));
			ret = default;
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
		if (method == MethodName.Record && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float[]>(Record(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		if (method == MethodName.CheckColor && args.Count == 3)
		{
			CheckColor(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
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
		if (method == MethodName.Record)
		{
			return true;
		}
		if (method == MethodName.Submit)
		{
			return true;
		}
		if (method == MethodName.CheckColor)
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
		if (name == PropertyName._viewport)
		{
			_viewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._multiMesh)
		{
			_multiMesh = VariantUtils.ConvertTo<MultiMesh>(in value);
			return true;
		}
		if (name == PropertyName._mesh)
		{
			_mesh = VariantUtils.ConvertTo<QuadMesh>(in value);
			return true;
		}
		if (name == PropertyName._frameVersion)
		{
			_frameVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._viewport)
		{
			value = VariantUtils.CreateFrom(in _viewport);
			return true;
		}
		if (name == PropertyName._multiMesh)
		{
			value = VariantUtils.CreateFrom(in _multiMesh);
			return true;
		}
		if (name == PropertyName._mesh)
		{
			value = VariantUtils.CreateFrom(in _mesh);
			return true;
		}
		if (name == PropertyName._frameVersion)
		{
			value = VariantUtils.CreateFrom(in _frameVersion);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._viewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._multiMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._viewport, Variant.From(in _viewport));
		info.AddProperty(PropertyName._multiMesh, Variant.From(in _multiMesh));
		info.AddProperty(PropertyName._mesh, Variant.From(in _mesh));
		info.AddProperty(PropertyName._frameVersion, Variant.From(in _frameVersion));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._viewport, out var value))
		{
			_viewport = value.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._multiMesh, out var value2))
		{
			_multiMesh = value2.As<MultiMesh>();
		}
		if (info.TryGetProperty(PropertyName._mesh, out var value3))
		{
			_mesh = value3.As<QuadMesh>();
		}
		if (info.TryGetProperty(PropertyName._frameVersion, out var value4))
		{
			_frameVersion = value4.As<long>();
		}
	}
}
