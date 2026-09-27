using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorLevelPerformanceHudRuntimeProbe.cs")]
public class ModEditorLevelPerformanceHudRuntimeProbe : Node
{
	private readonly struct FrameStats(double p50, double p95, double p99)
	{
		public readonly double P50 = p50;

		public readonly double P95 = p95;

		public readonly double P99 = p99;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConfigureWaveStressFixture = "ConfigureWaveStressFixture";

		public static readonly StringName RegistryMatches = "RegistryMatches";

		public static readonly StringName ReadPercentile = "ReadPercentile";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _history = "_history";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string FixturePath = "user://ModEditorLevelPerformanceHudRuntimeProbe.tres";

	private readonly List<string> _failures = new List<string>();

	private XWLevelVisualResourceEditor _editor;

	private XWUndoRedoManager _history;

	public override async void _Ready()
	{
		_ = 17;
		try
		{
			ModEditorManager modEditorManager = ModEditorManager.Instance;
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				modEditorManager = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Core/ModEditorManager.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<ModEditorManager>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(modEditorManager))
				{
					AddChild(modEditorManager, forceReadableName: false, InternalMode.Disabled);
				}
			}
			Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager could not be instantiated.");
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				Finish();
				return;
			}
			await WaitFrames(2);
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = true
			});
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = false
			});
			bool flag = await WaitForEditor(900);
			if (flag)
			{
				flag = await EnterEditorSurface(900);
			}
			bool f3 = flag;
			Require(f3, "F3 did not initialize the level editor.");
			if (!f3)
			{
				Finish();
				return;
			}
			TowerDefenseBattleCharacterRegistry battleRegistry = TowerDefenseManager.Instance?.characterRegistry;
			Require(GodotObject.IsInstanceValid(battleRegistry), "The isolated F3 probe has no battle character registry.");
			if (!GodotObject.IsInstanceValid(battleRegistry))
			{
				Finish();
				return;
			}
			int registryBaselineCount = battleRegistry.ActiveCharacterCount;
			ulong registryBaselineRevision = battleRegistry.QueryRevision;
			TowerDefenseLevelNewConfig level = XWNewLevelResourceDefaults.Create("LevelPerfProbe", "原始关卡名");
			level.description = "原始关卡说明";
			ConfigureWaveStressFixture(level);
			Require(level.processData["Wave"].AsGodotArray().Count == 24, "The level performance fixture did not retain its 24-wave continuous workload.");
			Error error = ResourceSaver.Save(level, "user://ModEditorLevelPerformanceHudRuntimeProbe.tres", ResourceSaver.SaverFlags.None);
			Require(error == Error.Ok, $"Level fixture save failed: {error}.");
			level.TakeOverPath("user://ModEditorLevelPerformanceHudRuntimeProbe.tres");
			Require(level.ResourcePath == "user://ModEditorLevelPerformanceHudRuntimeProbe.tres", "Level fixture did not take ownership of its saved path.");
			XWEditorInterface.Instance.EditResource(level);
			XWEditorInterface.Instance.FocusPanel("level_editor");
			await WaitFrames(12);
			List<TowerDefenseCharacter> list = CollectWavePreviewCharacters();
			bool previewIdentitySafe = list.Count == 15;
			foreach (TowerDefenseCharacter item in list)
			{
				previewIdentitySafe &= !item.inGame && item.editorPreviewMode && !item.IsOwnerBatchRegistered && GodotObject.IsInstanceValid(item.sprite) && item.sprite.preview && item.sprite.forceLocalRender && !item.sprite.refreshEveryFrame;
			}
			bool registryClean = RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			Require(previewIdentitySafe, "Wave preview characters entered the tree without complete preview identity, animation preparation, or batch isolation.");
			Require(registryClean, "Opening the wave preview mutated the real battle character registry.");
			LineEdit mainName = Find<LineEdit>("LevelDisplayNameLineEdit");
			LineEdit hudName = Find<LineEdit>("LevelNameHudEdit");
			LineEdit instance = Find<LineEdit>("DescriptionHudEdit");
			Button play = Find<Button>("AutoButton");
			Button reset = Find<Button>("ResetButton");
			Button next = Find<Button>("NextButton");
			bool hud = GodotObject.IsInstanceValid(mainName) && GodotObject.IsInstanceValid(play) && GodotObject.IsInstanceValid(reset) && GodotObject.IsInstanceValid(next) && GodotObject.IsInstanceValid(hudName) && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(Find<Label>("SunHudLabel")) && GodotObject.IsInstanceValid(Find<HBoxContainer>("SeedBankSlots")) && GodotObject.IsInstanceValid(Find<Label>("WaveHudLabel")) && GodotObject.IsInstanceValid(Find<ProgressBar>("InGameWaveProgress")) && GodotObject.IsInstanceValid(Find<Label>("MowerHudLabel")) && GodotObject.IsInstanceValid(Find<ColorRect>("FogOverlay")) && GodotObject.IsInstanceValid(Find<Label>("FogHudLabel"));
			Require(hud, "The in-game level HUD is missing SeedBank/Sun/Wave/Progress/Mower/Fog or inline text controls.");
			if (!hud)
			{
				Finish();
				return;
			}
			VBoxContainer vBoxContainer = Find<VBoxContainer>("EmbeddedInspectorHost");
			bool inspectorUntouched = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0 && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
			Require(inspectorUntouched, "Level HUD edit mounted or touched the raw Inspector.");
			int rebuildBeforeType = _editor.WaveRuntimeBattlefieldRebuildCount;
			string originalName = level.levelName;
			string editedName = "实战画面原子关卡名";
			_history.ClearHistory();
			mainName.EmitSignal(Control.SignalName.FocusEntered);
			for (int i = 1; i <= editedName.Length; i++)
			{
				string text = (mainName.Text = editedName.Substring(0, i));
				mainName.EmitSignal(LineEdit.SignalName.TextChanged, text);
			}
			await WaitFrames(2);
			bool singleTypingNoRebuild = level.levelName == originalName && _editor.WaveRuntimeBattlefieldRebuildCount == rebuildBeforeType && hudName.Text == editedName;
			mainName.EmitSignal(Control.SignalName.FocusExited);
			await WaitFrames(3);
			bool atomicCommit = level.levelName == editedName && _history.HasUndo() && _editor.LevelAtomicCommitCount > 0 && _editor.WaveRuntimeBattlefieldRebuildCount == rebuildBeforeType;
			bool undone = _history.Undo();
			await WaitFrames(3);
			undone = undone && level.levelName == originalName && hudName.Text == originalName;
			bool redone = _history.Redo();
			await WaitFrames(3);
			redone = redone && level.levelName == editedName && hudName.Text == editedName;
			bool undoRedo = atomicCommit & undone & redone;
			Require(singleTypingNoRebuild, "A single text typing session changed the resource early or rebuilt wave characters.");
			Require(undoRedo, "Atomic level-name edit did not round-trip through global UndoRedo.");
			await ToSignal(GetTree().CreateTimer(0.55), SceneTreeTimer.SignalName.Timeout);
			TowerDefenseLevelNewConfig towerDefenseLevelNewConfig = ResourceLoader.Load<TowerDefenseLevelNewConfig>("user://ModEditorLevelPerformanceHudRuntimeProbe.tres", "", ResourceLoader.CacheMode.Ignore);
			bool saveReload = GodotObject.IsInstanceValid(towerDefenseLevelNewConfig) && towerDefenseLevelNewConfig.levelName == editedName;
			Require(saveReload, "Committed level text did not save and reload through the debounce boundary.");
			bool defaultStatic = !_editor.IsWaveRuntimePlaying && !_editor.IsWaveRuntimeProcessing;
			int instantiationsBeforeNavigation = _editor.WaveRuntimeCharacterInstantiationCount;
			int reuseBeforeNavigation = _editor.WaveRuntimeCharacterReuseCount;
			bool allWavesVerified = _editor.WaveRuntimeIndex == 0;
			for (int expectedIndex = 1; expectedIndex < 24; expectedIndex++)
			{
				next.EmitSignal(BaseButton.SignalName.Pressed);
				await WaitFrames(2);
				List<TowerDefenseCharacter> list2 = CollectWavePreviewCharacters();
				bool flag2 = list2.Count == 15 && _editor.WaveRuntimeIndex == expectedIndex && _editor.WaveRuntimeUntrackedCharacterCount == 0;
				foreach (TowerDefenseCharacter item2 in list2)
				{
					flag2 &= !item2.inGame && item2.editorPreviewMode && !item2.IsOwnerBatchRegistered && GodotObject.IsInstanceValid(item2.sprite) && item2.sprite.preview && item2.sprite.forceLocalRender && !item2.sprite.refreshEveryFrame;
				}
				allWavesVerified &= flag2 && RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			}
			reset.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			allWavesVerified &= _editor.WaveRuntimeIndex == 0;
			bool pooledCharacters = _editor.WaveRuntimeCharacterInstantiationCount == instantiationsBeforeNavigation && _editor.WaveRuntimeCharacterReuseCount > reuseBeforeNavigation && CollectWavePreviewCharacters().Count == 15;
			registryClean &= RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			FrameStats pausedStats = await MeasureFrameGaps(180);
			list = CollectWavePreviewCharacters();
			AdobeAnimateSprite previewSprite = ((list.Count > 0) ? list[0].sprite : null);
			int animationFrameBefore = (GodotObject.IsInstanceValid(previewSprite) ? previewSprite.frameIndex : (-1));
			double animationElapsedBefore = (GodotObject.IsInstanceValid(previewSprite) ? previewSprite.elapsedTimer : (-1.0));
			int ticksBeforePlay = _editor.WaveRuntimeProcessTickCount;
			play.EmitSignal(BaseButton.SignalName.Pressed);
			FrameStats playingStats = await MeasureFrameGaps(600);
			int waveRuntimeProcessTickCount = _editor.WaveRuntimeProcessTickCount;
			bool playTicks = _editor.IsWaveRuntimePlaying && _editor.IsWaveRuntimeProcessing && waveRuntimeProcessTickCount > ticksBeforePlay;
			bool animationAdvanced = GodotObject.IsInstanceValid(previewSprite) && (previewSprite.frameIndex != animationFrameBefore || Math.Abs(previewSprite.elapsedTimer - animationElapsedBefore) > 0.001);
			double p95DeltaMs = playingStats.P95 - pausedStats.P95;
			double p99DeltaMs = playingStats.P99 - pausedStats.P99;
			bool responsive = p95DeltaMs <= 2.0 && p99DeltaMs <= 4.0;
			registryClean &= RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			play.EmitSignal(BaseButton.SignalName.Pressed);
			int ticksBeforePause = _editor.WaveRuntimeProcessTickCount;
			await WaitFrames(6);
			bool pauseIdle = !_editor.IsWaveRuntimePlaying && !_editor.IsWaveRuntimeProcessing && _editor.WaveRuntimeProcessTickCount == ticksBeforePause;
			registryClean &= RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			play.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(12);
			int pooledBeforeHidden = _editor.WaveRuntimePooledCharacterCount;
			int animationFrameBeforeHidden = (GodotObject.IsInstanceValid(previewSprite) ? previewSprite.frameIndex : (-1));
			double animationElapsedBeforeHidden = (GodotObject.IsInstanceValid(previewSprite) ? previewSprite.elapsedTimer : (-1.0));
			_editor.Hide();
			int ticksBeforeHidden = _editor.WaveRuntimeProcessTickCount;
			await WaitFrames(30);
			bool hiddenIdle = !_editor.IsWaveRuntimeProcessing && _editor.WaveRuntimeProcessTickCount == ticksBeforeHidden && _editor.WaveRuntimePooledCharacterCount == pooledBeforeHidden && _editor.WaveRuntimeUntrackedCharacterCount == 0;
			bool hiddenAnimationStopped = GodotObject.IsInstanceValid(previewSprite) && previewSprite.frameIndex == animationFrameBeforeHidden && Math.Abs(previewSprite.elapsedTimer - animationElapsedBeforeHidden) <= 1E-06;
			registryClean &= RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			_editor.Show();
			XWEditorInterface.Instance.FocusPanel("level_editor");
			await WaitFrames(2);
			if (_editor.IsWaveRuntimePlaying)
			{
				play.EmitSignal(BaseButton.SignalName.Pressed);
			}
			TowerDefenseLevelNewConfig res = XWNewLevelResourceDefaults.Create("LevelPerfProbeReplacement", "切换后的关卡");
			XWEditorInterface.Instance.EditResource(res);
			XWEditorInterface.Instance.FocusPanel("level_editor");
			await WaitFrames(8);
			bool flag3 = _editor.WaveRuntimePooledCharacterCount == 0 && _editor.WaveRuntimeUntrackedCharacterCount == 0;
			registryClean &= RegistryMatches(battleRegistry, registryBaselineCount, registryBaselineRevision);
			Require(defaultStatic, "Wave preview requested processing before explicit playback.");
			Require(allWavesVerified, "One or more of the 24 wave-preview pages lost characters, isolation, pooling, or registry cleanliness.");
			Require(pooledCharacters, "Reset rebuilt the same wave character scene instead of reusing the pool.");
			Require(playTicks, "Explicit playback did not start wave processing.");
			Require(animationAdvanced, "Prepared wave-preview animation did not advance during explicit playback.");
			Require(responsive, $"15-character wave preview exceeded the paused-baseline frame budget: P95 delta={p95DeltaMs:0.###} ms, P99 delta={p99DeltaMs:0.###} ms.");
			Require(pauseIdle, "Paused wave preview continued processing.");
			Require(hiddenIdle, "Hidden wave preview continued processing.");
			Require(hiddenAnimationStopped, "Hidden wave preview animation continued advancing.");
			Require(flag3, "Switching level resources left pooled or untracked wave characters behind.");
			Require(registryClean, "Wave preview mutated the real battle registry during reset, playback, hiding, or resource switching.");
			GD.Print($"[MOD_EDITOR_LEVEL_PERFORMANCE_HUD_PROBE] f3={f3} hud={hud} singleTypingNoRebuild={singleTypingNoRebuild} atomicCommit={atomicCommit} undoRedo={undoRedo} saveReload={saveReload} inspectorUntouched={inspectorUntouched} previewIdentitySafe={previewIdentitySafe} registryClean={registryClean} defaultStatic={defaultStatic} allWavesVerified={allWavesVerified} pooledCharacters={pooledCharacters} playTicks={playTicks} animationAdvanced={animationAdvanced} responsive={responsive} pauseIdle={pauseIdle} hiddenIdle={hiddenIdle} hiddenAnimationStopped={hiddenAnimationStopped} resourceSwitchClean={flag3} pausedP50Ms={pausedStats.P50:0.###} pausedP95Ms={pausedStats.P95:0.###} pausedP99Ms={pausedStats.P99:0.###} playingP50Ms={playingStats.P50:0.###} playingP95Ms={playingStats.P95:0.###} playingP99Ms={playingStats.P99:0.###} p95DeltaMs={p95DeltaMs:0.###} p99DeltaMs={p99DeltaMs:0.###} rebuilds={_editor.WaveRuntimeBattlefieldRebuildCount} instantiations={_editor.WaveRuntimeCharacterInstantiationCount} reuses={_editor.WaveRuntimeCharacterReuseCount} ticks={_editor.WaveRuntimeProcessTickCount} failures={_failures.Count}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private static void ConfigureWaveStressFixture(TowerDefenseLevelNewConfig level)
	{
		if (!GodotObject.IsInstanceValid(level) || level.processData == null || !level.processData.ContainsKey("Wave") || level.processData["Wave"].VariantType != Variant.Type.Array)
		{
			return;
		}
		Godot.Collections.Array array = level.processData["Wave"].AsGodotArray();
		array.Clear();
		for (int i = 0; i < 24; i++)
		{
			Godot.Collections.Array array2 = new Godot.Collections.Array();
			for (int j = 0; j < 5; j++)
			{
				array2.Add(new Dictionary
				{
					["Zombie"] = "ZombieNormal",
					["Num"] = 3,
					["Line"] = j + 1
				});
			}
			array.Add(new Dictionary { ["Spawn"] = array2 });
		}
		level.processData["Wave"] = array;
	}

	private List<TowerDefenseCharacter> CollectWavePreviewCharacters()
	{
		List<TowerDefenseCharacter> result = new List<TowerDefenseCharacter>();
		CollectWavePreviewCharactersRecursive(_editor?.FindChild("ZombieRoot", recursive: true, owned: false), result);
		return result;
	}

	private static void CollectWavePreviewCharactersRecursive(Node node, List<TowerDefenseCharacter> result)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is TowerDefenseCharacter item)
		{
			result.Add(item);
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			CollectWavePreviewCharactersRecursive(child, result);
		}
	}

	private static bool RegistryMatches(TowerDefenseBattleCharacterRegistry registry, int count, ulong revision)
	{
		if (GodotObject.IsInstanceValid(registry) && registry.ActiveCharacterCount == count)
		{
			return registry.QueryRevision == revision;
		}
		return false;
	}

	private async Task<FrameStats> MeasureFrameGaps(int frameCount)
	{
		frameCount = Math.Max(1, frameCount);
		double[] samples = new double[frameCount];
		long previous = Stopwatch.GetTimestamp();
		for (int index = 0; index < frameCount; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			long timestamp = Stopwatch.GetTimestamp();
			samples[index] = (double)(timestamp - previous) * 1000.0 / (double)Stopwatch.Frequency;
			previous = timestamp;
		}
		System.Array.Sort(samples);
		return new FrameStats(ReadPercentile(samples, 0.5), ReadPercentile(samples, 0.95), ReadPercentile(samples, 0.99));
	}

	private static double ReadPercentile(double[] sorted, double percentile)
	{
		if (sorted == null || sorted.Length == 0)
		{
			return 0.0;
		}
		int num = Math.Clamp((int)Math.Ceiling(percentile * (double)sorted.Length) - 1, 0, sorted.Length - 1);
		return sorted[num];
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int index = 0; index < maxFrames; index++)
		{
			if (XWEditorInterface.Instance?.GetResourceEditor("level_editor") is XWLevelVisualResourceEditor xWLevelVisualResourceEditor && GodotObject.IsInstanceValid(xWLevelVisualResourceEditor))
			{
				_editor = xWLevelVisualResourceEditor;
				_history = XWEditorInterface.Instance.GetUndoRedoManager();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int index = 0; index < maxFrames; index++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				XWEditorInterface.Instance.FocusPanel("level_editor");
				await WaitFrames(3);
				return _editor.IsVisibleInTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private T Find<T>(string name) where T : Node
	{
		return _editor?.FindChild(name, recursive: true, owned: false) as T;
	}

	private async Task WaitFrames(int count)
	{
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_LEVEL_PERFORMANCE_HUD_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_LEVEL_PERFORMANCE_HUD_PROBE_FAILURE] " + failure);
		}
		try
		{
			DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath("user://ModEditorLevelPerformanceHudRuntimeProbe.tres"));
		}
		catch
		{
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureWaveStressFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RegistryMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "registry", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "revision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadPercentile, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat64Array, "sorted", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "percentile", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ConfigureWaveStressFixture && args.Count == 1)
		{
			ConfigureWaveStressFixture(VariantUtils.ConvertTo<TowerDefenseLevelNewConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegistryMatches && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(RegistryMatches(VariantUtils.ConvertTo<TowerDefenseBattleCharacterRegistry>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<ulong>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadPercentile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ReadPercentile(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigureWaveStressFixture && args.Count == 1)
		{
			ConfigureWaveStressFixture(VariantUtils.ConvertTo<TowerDefenseLevelNewConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegistryMatches && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(RegistryMatches(VariantUtils.ConvertTo<TowerDefenseBattleCharacterRegistry>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<ulong>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadPercentile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ReadPercentile(VariantUtils.ConvertTo<double[]>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
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
		if (method == MethodName.ConfigureWaveStressFixture)
		{
			return true;
		}
		if (method == MethodName.RegistryMatches)
		{
			return true;
		}
		if (method == MethodName.ReadPercentile)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWLevelVisualResourceEditor>(in value);
			return true;
		}
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._history, Variant.From(in _history));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWLevelVisualResourceEditor>();
		}
		if (info.TryGetProperty(PropertyName._history, out var value2))
		{
			_history = value2.As<XWUndoRedoManager>();
		}
	}
}
