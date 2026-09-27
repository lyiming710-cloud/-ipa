using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/AdobeAnimateStaticAtlasPathLayoutCacheRuntimeTest.cs")]
public sealed class AdobeAnimateStaticAtlasPathLayoutCacheRuntimeTest : Node
{
	private readonly struct FirstApplyDiagnosticResult(double meanMilliseconds, double p50Milliseconds, double p95Milliseconds, double p99Milliseconds, double maxMilliseconds, long allocatedBytes, int gen0, int gen1, int gen2, bool stable)
	{
		public double MeanMilliseconds { get; } = meanMilliseconds;

		public double P50Milliseconds { get; } = p50Milliseconds;

		public double P95Milliseconds { get; } = p95Milliseconds;

		public double P99Milliseconds { get; } = p99Milliseconds;

		public double MaxMilliseconds { get; } = maxMilliseconds;

		public long AllocatedBytes { get; } = allocatedBytes;

		public int Gen0 { get; } = gen0;

		public int Gen1 { get; } = gen1;

		public int Gen2 { get; } = gen2;

		public bool Stable { get; } = stable;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName CreateConfiguredOwner = "CreateConfiguredOwner";

		public static readonly StringName Percentile = "Percentile";

		public static readonly StringName ExecuteCacheHitBatch = "ExecuteCacheHitBatch";

		public static readonly StringName VerifyPeerIsolation = "VerifyPeerIsolation";

		public static readonly StringName VerifyClearRestoreQueueFalse = "VerifyClearRestoreQueueFalse";

		public static readonly StringName VerifyDetachedInPlaceMutation = "VerifyDetachedInPlaceMutation";

		public static readonly StringName ReleaseOwner = "ReleaseOwner";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _owners = "_owners";

		public static readonly StringName _data = "_data";

		public static readonly StringName _hitSink = "_hitSink";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int WarmupSampleCount = 240;

	private const int SampleCount = 1200;

	private const int ReplacementMediaId = 6;

	private const string ReplacementMediaName = "Zombie_sleep_0010_23.png";

	private const string DataPath = "res://Asset/Anime/Character/Zombie/Chapter1/Sleeper/ZombieSleeper.tres";

	private const string ReplacementPath = "res://Asset/AtlasSource/Armor/Texture/Character/Armor/SleepHat/ZombieSleepHat1.png";

	private readonly AdobeAnimateSprite[] _owners = new AdobeAnimateSprite[1000];

	private AdobeAnimateData _data;

	private long _hitSink;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		Callable.From(Run).CallDeferred();
	}

	private void Run()
	{
		AdobeAnimateSprite adobeAnimateSprite = null;
		AdobeAnimateSprite adobeAnimateSprite2 = null;
		AdobeAnimateSprite adobeAnimateSprite3 = null;
		Node node = null;
		AdobeAnimateSprite adobeAnimateSprite4 = null;
		try
		{
			_data = GD.Load<AdobeAnimateData>("res://Asset/Anime/Character/Zombie/Chapter1/Sleeper/ZombieSleeper.tres");
			if (!GodotObject.IsInstanceValid(_data) || _data.mediaDictionary == null || !_data.mediaDictionary.ContainsKey("Zombie_sleep_0010_23.png") || (int)_data.mediaDictionary["Zombie_sleep_0010_23.png"] != 6)
			{
				throw new InvalidOperationException("The production Sleeper replacement fixture is unavailable.");
			}
			AdobeAnimateSprite.ClearStaticAtlasPathLayoutCacheForTests();
			adobeAnimateSprite = CreateConfiguredOwner();
			long timestamp = Stopwatch.GetTimestamp();
			adobeAnimateSprite.UpdateMediaReplaceData();
			double value = (double)(Stopwatch.GetTimestamp() - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
			bool flag = adobeAnimateSprite.mediaReplaceAtlasShared && adobeAnimateSprite.mediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(adobeAnimateSprite.mediaReplaceAtlasArray) && adobeAnimateSprite.mediaReplaceRect.Count == _data.mediaDictionary.Count && AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests == 1;
			FirstApplyDiagnosticResult result = MeasureFirstApply(bypassStaticCache: true);
			FirstApplyDiagnosticResult result2 = MeasureFirstApply(bypassStaticCache: false);
			bool flag2 = result2.Stable && result2.P99Milliseconds < 0.2 && result2.AllocatedBytes == 0L && result2.Gen0 == 0 && result2.Gen1 == 0 && result2.Gen2 == 0;
			for (int i = 0; i < _owners.Length; i++)
			{
				_owners[i] = CreateConfiguredOwner();
				if (!_owners[i].TryApplyStaticAtlasPathLayoutCacheForTests())
				{
					throw new InvalidOperationException($"Static layout cache missed owner {i}.");
				}
				_owners[i].needMediaReplaceUpdate = false;
			}
			node = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Sleeper/Scene/TowerDefenseZombieSleeper.tscn")?.Instantiate(PackedScene.GenEditState.Disabled);
			adobeAnimateSprite4 = node?.GetNodeOrNull<AdobeAnimateSprite>("SpriteGroup/TransformPoint/ZombieSleeper");
			bool flag3 = GodotObject.IsInstanceValid(adobeAnimateSprite4) && adobeAnimateSprite4.mediaReplaceUse.Count > 6 && adobeAnimateSprite4.mediaReplaceUse[6] && adobeAnimateSprite4.GetAtlasReplacePath("Zombie_sleep_0010_23.png") == "res://Asset/AtlasSource/Armor/Texture/Character/Armor/SleepHat/ZombieSleepHat1.png" && adobeAnimateSprite4.TryApplyStaticAtlasPathLayoutCacheForTests() && adobeAnimateSprite4.mediaReplaceAtlasShared;
			bool flag4 = VerifyPeerIsolation();
			bool flag5 = VerifyClearRestoreQueueFalse(_owners[0]);
			bool flag6 = VerifyDetachedInPlaceMutation();
			adobeAnimateSprite3 = CreateConfiguredOwner();
			adobeAnimateSprite3.atlasProfileOverride = new AdobeAnimateAtlasProfile();
			bool flag7 = !adobeAnimateSprite3.TryApplyStaticAtlasPathLayoutCacheForTests();
			adobeAnimateSprite2 = CreateConfiguredOwner();
			AddChild(adobeAnimateSprite2, forceReadableName: false, InternalMode.Disabled);
			bool flag8 = adobeAnimateSprite2.SetAtlasReplace("Zombie_sleep_0010_23.png", "res://Asset/AtlasSource/Armor/Texture/Character/Armor/SleepHat/ZombieSleepHat1.png", queueUpdate: false);
			int staticAtlasPathLayoutCacheEntryCountForTests = AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests;
			bool flag9 = flag8 && !adobeAnimateSprite2.TryApplyStaticAtlasPathLayoutCacheForTests();
			adobeAnimateSprite2.CreateMediaReplaceAtlas();
			bool flag10 = adobeAnimateSprite2.mediaReplaceAtlasShared && adobeAnimateSprite2.mediaReplaceAtlasUsesTextureArray && AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests == staticAtlasPathLayoutCacheEntryCountForTests;
			AdobeAnimateSprite.InvalidateStaticAtlasPathLayoutCacheForGpuReset();
			_owners[1].InvalidateRuntimeRenderAfterGpuCacheReset();
			bool num = _owners[1].needMediaReplaceUpdate && !_owners[1].CanReuseAppliedStaticAtlasPathLayoutForTests();
			_owners[1].UpdateMediaReplaceData();
			bool flag11 = num && !_owners[1].needMediaReplaceUpdate && _owners[1].mediaReplaceAtlasShared && _owners[1].CanReuseAppliedStaticAtlasPathLayoutForTests() && AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests == 1;
			bool flag12 = _owners[2].CorruptStaticAtlasPathLayoutEntryForTests() && !_owners[2].CanReuseAppliedStaticAtlasPathLayoutForTests();
			_owners[2].CreateMediaReplaceAtlas();
			bool flag13 = flag12 && _owners[2].mediaReplaceAtlasShared && _owners[2].mediaReplaceAtlasUsesTextureArray && _owners[2].CanReuseAppliedStaticAtlasPathLayoutForTests() && AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests == 1;
			int cacheVersion = AdobeAnimateGlobalAtlasCache.CacheVersion;
			AdobeAnimateGlobalAtlasCache.Clear();
			bool flag14 = AdobeAnimateGlobalAtlasCache.CacheVersion != cacheVersion && !_owners[0].TryApplyStaticAtlasPathLayoutCacheForTests() && AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests == 0;
			adobeAnimateSprite.UpdateMediaReplaceData();
			bool flag15 = adobeAnimateSprite.mediaReplaceAtlasShared && AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests == 1;
			bool flag16 = true;
			int num2 = 0;
			for (int j = 0; j < _owners.Length; j++)
			{
				flag16 &= _owners[j].TryApplyStaticAtlasPathLayoutCacheForTests();
				_owners[j].needMediaReplaceUpdate = false;
				if (_owners[j].CanReuseAppliedStaticAtlasPathLayoutForTests())
				{
					num2++;
				}
			}
			OptimizationBatchSampler.PrepareForWarmup();
			long num3 = 0L;
			bool flag17 = true;
			for (int k = 0; k < 240; k++)
			{
				long startTicks = OptimizationBatchSampler.BeginSample();
				flag17 &= ExecuteCacheHitBatch();
				num3 += OptimizationBatchSampler.EndWarmupSample(startTicks);
			}
			long hitSink = _hitSink;
			OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
			bool flag18 = true;
			optimizationBatchSampler.BeginMeasurement();
			for (int l = 0; l < 1200; l++)
			{
				long startTicks2 = OptimizationBatchSampler.BeginSample();
				flag18 &= ExecuteCacheHitBatch();
				optimizationBatchSampler.EndSample(startTicks2);
			}
			OptimizationBatchResult result3 = optimizationBatchSampler.Complete();
			long num4 = 1200000L;
			bool functionalPassed = ((((flag && result.Stable) & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag9 & flag10 & flag11 & flag12 & flag13 & flag14 & flag15 & flag16) && num2 == 1000) & flag17 & flag18) && num3 >= 0 && _hitSink - hitSink == num4;
			OptimizationResultIdentity identity = new OptimizationResultIdentity("adobe_animate_static_atlas_path_layout_cache", OptimizationWorkloadKind.BareFunction, "zombie_sleeper_static_replacement", "res://Asset/Anime/Character/Zombie/Chapter1/Sleeper/Scene/TowerDefenseZombieSleeper.tscn", "AdobeAnimateSprite", "none", "none", "none", "applied_layout_reuse", "steady_applied_layout_reuse", OptimizationScheduleKind.BackToBack, "headless", Math.Max(1, Engine.PhysicsTicksPerSecond), 240);
			bool flag19 = OptimizationPerformanceGate.IsBareResultPassed(in result3, functionalPassed, 1000, 240, 1000, 1000, in identity);
			GD.Print(OptimizationPerformanceGate.FormatBareResult(in identity, in result3, functionalPassed, flag19, 1000, 240, 1000, 1000));
			GD.Print("ADOBE_ANIMATE_STATIC_ATLAS_PATH_LAYOUT_COLD_DIAGNOSTIC " + $"coldBuildMs={value:F6} " + $"cacheEntries={AdobeAnimateSprite.StaticAtlasPathLayoutCacheEntryCountForTests} " + $"shared={adobeAnimateSprite.mediaReplaceAtlasShared} " + $"usesTextureArray={adobeAnimateSprite.mediaReplaceAtlasUsesTextureArray}");
			PrintFirstApplyDiagnostic("baseline_slow_path", in result);
			PrintFirstApplyDiagnostic("cached_first_apply", in result2);
			GD.Print("ADOBE_ANIMATE_STATIC_ATLAS_PATH_LAYOUT_SEMANTICS " + $"coldBuildSucceeded={flag} " + $"cachedFirstApplyPassed={flag2} " + $"productionSceneDeserialized={flag3} " + $"peerIsolation={flag4} " + $"clearRestoreQueueFalse={flag5} " + $"detachedInPlaceMutationPreserved={flag6} " + $"profileMismatchBypassed={flag7} " + $"dynamicMutationBypassed={flag9} " + $"dynamicMutationSlowPathPreserved={flag10} " + $"gpuResetReparsed={flag11} " + $"freedAppliedRejected={flag12} " + $"staleEntryRepublished={flag13} " + $"staleVersionRejected={flag14} " + $"versionRebuilt={flag15} " + $"everyOwnerRebound={flag16} " + $"appliedReuseCount={num2}");
			GetTree().Quit((!flag19) ? 2 : 0);
		}
		catch (Exception value2)
		{
			GD.PrintErr("ADOBE_ANIMATE_STATIC_ATLAS_PATH_LAYOUT_SEMANTICS " + $"passed=False exception={value2}");
			GetTree().Quit(2);
		}
		finally
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite4))
			{
				adobeAnimateSprite4.flashAnimeData = null;
			}
			if (GodotObject.IsInstanceValid(node))
			{
				node.Free();
			}
			ReleaseOwner(adobeAnimateSprite3);
			if (GodotObject.IsInstanceValid(adobeAnimateSprite2))
			{
				if (adobeAnimateSprite2.GetParent() == this)
				{
					RemoveChild(adobeAnimateSprite2);
				}
				ReleaseOwner(adobeAnimateSprite2);
			}
			for (int m = 0; m < _owners.Length; m++)
			{
				ReleaseOwner(_owners[m]);
				_owners[m] = null;
			}
			ReleaseOwner(adobeAnimateSprite);
			AdobeAnimateSprite.ClearStaticAtlasPathLayoutCacheForTests();
		}
	}

	private AdobeAnimateSprite CreateConfiguredOwner()
	{
		AdobeAnimateSprite adobeAnimateSprite = new AdobeAnimateSprite
		{
			flashAnimeData = _data
		};
		Array<string> array = new Array<string>();
		array.Resize(_data.mediaDictionary.Count);
		array[6] = "res://Asset/AtlasSource/Armor/Texture/Character/Armor/SleepHat/ZombieSleepHat1.png";
		adobeAnimateSprite.mediaReplaceAtlasPaths = array;
		adobeAnimateSprite.mediaReplaceUse[6] = true;
		return adobeAnimateSprite;
	}

	private FirstApplyDiagnosticResult MeasureFirstApply(bool bypassStaticCache)
	{
		for (int i = 0; i < 240; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = CreateConfiguredOwner();
			if (bypassStaticCache)
			{
				adobeAnimateSprite.CreateMediaReplaceAtlasWithoutStaticCacheForTests();
			}
			else
			{
				adobeAnimateSprite.TryApplyStaticAtlasPathLayoutCacheForTests();
			}
			ReleaseOwner(adobeAnimateSprite);
		}
		AdobeAnimateSprite[] array = new AdobeAnimateSprite[1000];
		double[] array2 = new double[1000];
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = CreateConfiguredOwner();
		}
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		int num = GC.CollectionCount(0);
		int num2 = GC.CollectionCount(1);
		int num3 = GC.CollectionCount(2);
		bool flag = true;
		for (int k = 0; k < array.Length; k++)
		{
			AdobeAnimateSprite adobeAnimateSprite2 = array[k];
			long timestamp = Stopwatch.GetTimestamp();
			bool flag2;
			if (bypassStaticCache)
			{
				adobeAnimateSprite2.CreateMediaReplaceAtlasWithoutStaticCacheForTests();
				flag2 = adobeAnimateSprite2.mediaReplaceAtlasShared && adobeAnimateSprite2.mediaReplaceAtlasUsesTextureArray;
			}
			else
			{
				flag2 = adobeAnimateSprite2.TryApplyStaticAtlasPathLayoutCacheForTests();
			}
			long timestamp2 = Stopwatch.GetTimestamp();
			array2[k] = (double)(timestamp2 - timestamp) * 1000.0 / (double)Stopwatch.Frequency;
			flag &= flag2 && adobeAnimateSprite2.mediaReplaceAtlasShared && adobeAnimateSprite2.mediaReplaceAtlasUsesTextureArray;
		}
		long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread;
		int gen = GC.CollectionCount(0) - num;
		int gen2 = GC.CollectionCount(1) - num2;
		int gen3 = GC.CollectionCount(2) - num3;
		double num4 = 0.0;
		for (int l = 0; l < array2.Length; l++)
		{
			num4 += array2[l];
		}
		System.Array.Sort(array2);
		FirstApplyDiagnosticResult result = new FirstApplyDiagnosticResult(num4 / (double)array2.Length, Percentile(array2, 0.5), Percentile(array2, 0.95), Percentile(array2, 0.99), array2[^1], allocatedBytes, gen, gen2, gen3, flag);
		for (int m = 0; m < array.Length; m++)
		{
			ReleaseOwner(array[m]);
		}
		return result;
	}

	private static double Percentile(double[] sortedValues, double percentile)
	{
		int num = Math.Clamp((int)Math.Ceiling((double)sortedValues.Length * percentile) - 1, 0, sortedValues.Length - 1);
		return sortedValues[num];
	}

	private static void PrintFirstApplyDiagnostic(string variant, in FirstApplyDiagnosticResult result)
	{
		GD.Print("ADOBE_ANIMATE_STATIC_ATLAS_PATH_LAYOUT_FIRST_APPLY_DIAGNOSTIC " + $"variant={variant} instances={1000} " + $"warmupOwners={240} " + $"meanMs={result.MeanMilliseconds:F6} " + $"p50Ms={result.P50Milliseconds:F6} " + $"p95Ms={result.P95Milliseconds:F6} " + $"p99Ms={result.P99Milliseconds:F6} " + $"maxMs={result.MaxMilliseconds:F6} " + $"allocatedBytes={result.AllocatedBytes} " + $"gen0={result.Gen0} gen1={result.Gen1} " + $"gen2={result.Gen2} stable={result.Stable}");
	}

	private bool ExecuteCacheHitBatch()
	{
		int num = 0;
		for (int i = 0; i < _owners.Length; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _owners[i];
			if (adobeAnimateSprite.CanReuseAppliedStaticAtlasPathLayoutForTests() && adobeAnimateSprite.mediaReplaceAtlasShared && adobeAnimateSprite.mediaReplaceAtlasUsesTextureArray)
			{
				num++;
			}
		}
		_hitSink += num;
		return num == 1000;
	}

	private bool VerifyPeerIsolation()
	{
		AdobeAnimateSprite obj = _owners[0];
		AdobeAnimateSprite adobeAnimateSprite = _owners[999];
		Rect2 rect = obj.mediaReplaceRect[6];
		adobeAnimateSprite.mediaReplaceRect[6] = new Rect2(999f, 999f, 1f, 1f);
		bool num = obj.mediaReplaceRect[6] == rect && adobeAnimateSprite.mediaReplaceRect[6] != rect;
		bool flag = adobeAnimateSprite.TryApplyStaticAtlasPathLayoutCacheForTests() && adobeAnimateSprite.mediaReplaceRect[6] == rect;
		return num & flag;
	}

	private static bool VerifyClearRestoreQueueFalse(AdobeAnimateSprite owner)
	{
		bool flag = owner.SetAtlasReplace("Zombie_sleep_0010_23.png", string.Empty, queueUpdate: false);
		bool flag2 = !owner.needMediaReplaceUpdate;
		owner.CreateMediaReplaceAtlas();
		bool flag3 = !owner.mediaReplaceAtlasShared && !owner.mediaReplaceAtlasUsesTextureArray && owner.mediaReplaceAtlasArray == null;
		bool flag4 = owner.SetAtlasReplace("Zombie_sleep_0010_23.png", "res://Asset/AtlasSource/Armor/Texture/Character/Armor/SleepHat/ZombieSleepHat1.png", queueUpdate: false);
		bool flag5 = !owner.needMediaReplaceUpdate;
		owner.CreateMediaReplaceAtlas();
		bool flag6 = owner.mediaReplaceAtlasShared && owner.mediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(owner.mediaReplaceAtlasArray);
		return flag & flag2 & flag3 & flag4 & flag5 & flag6;
	}

	private bool VerifyDetachedInPlaceMutation()
	{
		AdobeAnimateSprite adobeAnimateSprite = CreateConfiguredOwner();
		try
		{
			bool num = adobeAnimateSprite.TryApplyStaticAtlasPathLayoutCacheForTests() && adobeAnimateSprite.mediaReplaceAtlasShared && adobeAnimateSprite.mediaReplaceAtlasUsesTextureArray;
			adobeAnimateSprite.mediaReplaceUse[6] = false;
			adobeAnimateSprite.UpdateMediaReplaceData();
			bool flag = !adobeAnimateSprite.mediaReplaceUse[6] && !adobeAnimateSprite.mediaReplaceAtlasShared && !adobeAnimateSprite.mediaReplaceAtlasUsesTextureArray && adobeAnimateSprite.mediaReplaceAtlasArray == null;
			adobeAnimateSprite.mediaReplaceUse[6] = true;
			adobeAnimateSprite.UpdateMediaReplaceData();
			bool flag2 = adobeAnimateSprite.mediaReplaceUse[6] && adobeAnimateSprite.mediaReplaceAtlasShared && adobeAnimateSprite.mediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(adobeAnimateSprite.mediaReplaceAtlasArray);
			return num & flag & flag2;
		}
		finally
		{
			ReleaseOwner(adobeAnimateSprite);
		}
	}

	private static void ReleaseOwner(AdobeAnimateSprite owner)
	{
		if (GodotObject.IsInstanceValid(owner))
		{
			owner.flashAnimeData = null;
			owner.Free();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateConfiguredOwner, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Percentile, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat64Array, "sortedValues", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "percentile", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteCacheHitBatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyPeerIsolation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifyClearRestoreQueueFalse, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.VerifyDetachedInPlaceMutation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseOwner, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.Run && args.Count == 0)
		{
			Run();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateConfiguredOwner && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(CreateConfiguredOwner());
			return true;
		}
		if (method == MethodName.Percentile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(Percentile(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.ExecuteCacheHitBatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ExecuteCacheHitBatch());
			return true;
		}
		if (method == MethodName.VerifyPeerIsolation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyPeerIsolation());
			return true;
		}
		if (method == MethodName.VerifyClearRestoreQueueFalse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyClearRestoreQueueFalse(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.VerifyDetachedInPlaceMutation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyDetachedInPlaceMutation());
			return true;
		}
		if (method == MethodName.ReleaseOwner && args.Count == 1)
		{
			ReleaseOwner(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Percentile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(Percentile(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.VerifyClearRestoreQueueFalse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(VerifyClearRestoreQueueFalse(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReleaseOwner && args.Count == 1)
		{
			ReleaseOwner(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
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
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.CreateConfiguredOwner)
		{
			return true;
		}
		if (method == MethodName.Percentile)
		{
			return true;
		}
		if (method == MethodName.ExecuteCacheHitBatch)
		{
			return true;
		}
		if (method == MethodName.VerifyPeerIsolation)
		{
			return true;
		}
		if (method == MethodName.VerifyClearRestoreQueueFalse)
		{
			return true;
		}
		if (method == MethodName.VerifyDetachedInPlaceMutation)
		{
			return true;
		}
		if (method == MethodName.ReleaseOwner)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._data)
		{
			_data = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._hitSink)
		{
			_hitSink = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._owners)
		{
			GodotObject[] owners = _owners;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(owners);
			return true;
		}
		if (name == PropertyName._data)
		{
			value = VariantUtils.CreateFrom(in _data);
			return true;
		}
		if (name == PropertyName._hitSink)
		{
			value = VariantUtils.CreateFrom(in _hitSink);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._owners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hitSink, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._data, Variant.From(in _data));
		info.AddProperty(PropertyName._hitSink, Variant.From(in _hitSink));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._data, out var value))
		{
			_data = value.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._hitSink, out var value2))
		{
			_hitSink = value2.As<long>();
		}
	}
}
