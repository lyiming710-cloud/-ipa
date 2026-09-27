using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/SceneManager/SceneManager.cs")]
public class SceneManager : Node
{
	public delegate void SceneChangeEventHandler(string sceneName);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName ChangeScene = "ChangeScene";

		public static readonly StringName ReloadScene = "ReloadScene";

		public static readonly StringName BackScene = "BackScene";

		public static readonly StringName TryBeginSceneTransition = "TryBeginSceneTransition";

		public static readonly StringName EndSceneTransition = "EndSceneTransition";

		public static readonly StringName DisableCurrentSceneProcessing = "DisableCurrentSceneProcessing";

		public static readonly StringName ClearObjectsForSceneChange = "ClearObjectsForSceneChange";

		public static readonly StringName UpdateLoadingStatus = "UpdateLoadingStatus";

		public static readonly StringName TryStartThreadedSceneLoad = "TryStartThreadedSceneLoad";

		public static readonly StringName EnsureGameplayAtlasesReadyForBattleScene = "EnsureGameplayAtlasesReadyForBattleScene";

		public static readonly StringName CompleteSceneLoading = "CompleteSceneLoading";

		public static readonly StringName ExitLoadingScene = "ExitLoadingScene";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName scaneLoading = "scaneLoading";

		public static readonly StringName currentScene = "currentScene";

		public static readonly StringName currentStopAllAudio = "currentStopAllAudio";

		public static readonly StringName isLoading = "isLoading";

		public static readonly StringName isSceneChanging = "isSceneChanging";

		public static readonly StringName deferLoadingOverlayUntilSceneReady = "deferLoadingOverlayUntilSceneReady";

		public static readonly StringName currentSceneLoading = "currentSceneLoading";

		public static readonly StringName sceneStack = "sceneStack";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private static PackedScene _sceneLoading;

	private static readonly Dictionary SCENES = new Dictionary
	{
		{ "Loading", "uid://3q5h8vq3vco5" },
		{ "MainMenu", "uid://7rqvcn2algju" },
		{ "TowerDefense", "uid://4gdrrrvcjny1" },
		{ "LevelChoose", "uid://bdn3to4813v1j" },
		{ "AwardSettlement", "uid://b8de1mhphnwo7" },
		{ "LevelEditorStage", "uid://bxi6r3quc47lr" }
	};

	public SceneLoading scaneLoading;

	public string currentScene = "";

	public bool currentStopAllAudio;

	public bool isLoading;

	private bool isSceneChanging;

	private bool deferLoadingOverlayUntilSceneReady;

	public string currentSceneLoading = "";

	public Array sceneStack = new Array();

	public static SceneManager Instance;

	private static PackedScene SCENE_LOADING => _sceneLoading ?? (_sceneLoading = GD.Load<PackedScene>("res://Core/SceneManager/SceneLoadeing/SceneLoading.tscn"));

	public static string CurrentScene => Instance?.currentScene ?? "";

	public event SceneChangeEventHandler OnSceneChange;

	public override void _Ready()
	{
		Instance = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!isLoading)
		{
			return;
		}
		if (string.IsNullOrEmpty(currentSceneLoading))
		{
			isLoading = false;
			deferLoadingOverlayUntilSceneReady = false;
			ExitLoadingScene();
			EndSceneTransition();
			return;
		}
		switch (ResourceLoader.LoadThreadedGetStatus(currentSceneLoading))
		{
		case ResourceLoader.ThreadLoadStatus.Loaded:
		{
			if (deferLoadingOverlayUntilSceneReady && !EnsureGameplayAtlasesReadyForBattleScene())
			{
				break;
			}
			PackedScene packedScene = GD.Load<PackedScene>(currentSceneLoading);
			if (packedScene == null || !packedScene.CanInstantiate())
			{
				GD.PrintErr("[SceneManager] Failed to load scene (null or corrupted, cannot instantiate): " + currentSceneLoading);
				isLoading = false;
				currentSceneLoading = "";
				deferLoadingOverlayUntilSceneReady = false;
				ExitLoadingScene();
				EndSceneTransition();
				break;
			}
			if (deferLoadingOverlayUntilSceneReady)
			{
				UpdateLoadingStatus("正在初始化关卡");
			}
			GD.Print("[SceneManager] Changing scene to: " + currentSceneLoading);
			if (!deferLoadingOverlayUntilSceneReady)
			{
				ExitLoadingScene();
			}
			SceneTree tree = GetTree();
			if (GodotObject.IsInstanceValid(tree))
			{
				tree.ChangeSceneToPacked(packedScene);
			}
			currentSceneLoading = "";
			isLoading = false;
			EndSceneTransition();
			if (currentStopAllAudio)
			{
				AudioManager.Instance?.AudioStopAll();
			}
			break;
		}
		case ResourceLoader.ThreadLoadStatus.Failed:
			GD.PrintErr("[SceneManager] Scene load failed: " + currentSceneLoading);
			isLoading = false;
			currentSceneLoading = "";
			deferLoadingOverlayUntilSceneReady = false;
			ExitLoadingScene();
			EndSceneTransition();
			break;
		}
	}

	public async void ChangeScene(string scene, bool stopAllAudio = true)
	{
		SceneTree tree = GetTree();
		if (!TryBeginSceneTransition(tree))
		{
			return;
		}
		if (!TryCreateLoadingScene(out var loadingScene))
		{
			EndSceneTransition();
			return;
		}
		DisableCurrentSceneProcessing(tree);
		currentStopAllAudio = stopAllAudio;
		ClearObjectsForSceneChange();
		if (currentScene != scene)
		{
			sceneStack.Add(scene);
			currentScene = scene;
		}
		OnSceneChange?.Invoke(scene);
		TaskCompletionSource<bool> enterTcs = new TaskCompletionSource<bool>();
		loadingScene.OnEnter += OnEnterHandler;
		try
		{
			await enterTcs.Task;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(loadingScene))
			{
				loadingScene.OnEnter -= OnEnterHandler;
			}
		}
		if (!TryStartThreadedSceneLoad(scene))
		{
			ExitLoadingScene();
		}
		EndSceneTransition();
		void OnEnterHandler()
		{
			enterTcs.TrySetResult(result: true);
		}
	}

	public async void ReloadScene(bool stopAllAudio = true)
	{
		SceneTree tree = GetTree();
		if (!TryBeginSceneTransition(tree))
		{
			return;
		}
		if (!TryCreateLoadingScene(out var loadingScene))
		{
			EndSceneTransition();
			return;
		}
		DisableCurrentSceneProcessing(tree);
		currentStopAllAudio = stopAllAudio;
		ClearObjectsForSceneChange();
		OnSceneChange?.Invoke(currentScene);
		TaskCompletionSource<bool> reloadEnterTcs = new TaskCompletionSource<bool>();
		loadingScene.OnEnter += OnReloadEnterHandler;
		try
		{
			await reloadEnterTcs.Task;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(loadingScene))
			{
				loadingScene.OnEnter -= OnReloadEnterHandler;
			}
		}
		if (!TryStartThreadedSceneLoad(currentScene))
		{
			ExitLoadingScene();
		}
		EndSceneTransition();
		void OnReloadEnterHandler()
		{
			reloadEnterTcs.TrySetResult(result: true);
		}
	}

	public void BackScene()
	{
		if (sceneStack.Count > 0)
		{
			string scene = (string)sceneStack[sceneStack.Count - 1];
			sceneStack.RemoveAt(sceneStack.Count - 1);
			ChangeScene(scene);
		}
	}

	private bool TryBeginSceneTransition(SceneTree tree)
	{
		if (!GodotObject.IsInstanceValid(tree))
		{
			return false;
		}
		if (isLoading || isSceneChanging || deferLoadingOverlayUntilSceneReady)
		{
			return false;
		}
		tree.Paused = false;
		isSceneChanging = true;
		return true;
	}

	private void EndSceneTransition()
	{
		isSceneChanging = false;
	}

	private void DisableCurrentSceneProcessing(SceneTree tree)
	{
		Node node = tree.CurrentScene;
		if (GodotObject.IsInstanceValid(node))
		{
			node.ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	private void ClearObjectsForSceneChange()
	{
		ObjectManager.Instance?.Clear();
		ResourceManager.Instance?.ReleaseTransientResources();
	}

	private bool TryCreateLoadingScene(out SceneLoading loadingScene)
	{
		loadingScene = null;
		if (SCENE_LOADING == null || !SCENE_LOADING.CanInstantiate())
		{
			GD.PrintErr("[SceneManager] Failed to create loading scene.");
			return false;
		}
		loadingScene = (SceneLoading)SCENE_LOADING.Instantiate(PackedScene.GenEditState.Disabled);
		loadingScene.ProcessMode = ProcessModeEnum.Always;
		loadingScene.SetStatus("正在加载场景");
		scaneLoading = loadingScene;
		CallDeferred("add_child", loadingScene);
		return true;
	}

	public void UpdateLoadingStatus(string status)
	{
		if (GodotObject.IsInstanceValid(scaneLoading))
		{
			scaneLoading.SetStatus(status);
		}
	}

	private bool TryStartThreadedSceneLoad(string scene)
	{
		if (!SCENES.ContainsKey(scene))
		{
			GD.PrintErr("[SceneManager] Unknown scene: " + scene);
			return false;
		}
		string text = (string)SCENES[scene];
		if (!ResourceLoader.Exists(text))
		{
			GD.PrintErr("[SceneManager] Scene path does not exist: " + text);
			return false;
		}
		Error error = ResourceLoader.LoadThreadedRequest(text, "", useSubThreads: false, ResourceLoader.CacheMode.Reuse);
		if (error != Error.Ok)
		{
			GD.PrintErr($"[SceneManager] Failed to request threaded scene load: {text}, error: {error}");
			return false;
		}
		currentSceneLoading = text;
		deferLoadingOverlayUntilSceneReady = scene == "TowerDefense";
		isLoading = true;
		Global.TimeScale = 1.0;
		return true;
	}

	private bool EnsureGameplayAtlasesReadyForBattleScene()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			UpdateLoadingStatus("战斗资源管理器不可用");
			return false;
		}
		if (instance.AreGameplayAtlasesReady)
		{
			return true;
		}
		if (instance.HasGameplayAtlasLoadFailed)
		{
			UpdateLoadingStatus("战斗资源加载失败：" + instance.GameplayAtlasLoadError);
			return false;
		}
		if (instance.CurrentGameplayAtlasLoadState == ResourceManager.GameplayAtlasLoadState.NotStarted)
		{
			instance.BeginGameplayAtlasPreload();
		}
		UpdateLoadingStatus("正在准备战斗资源");
		return false;
	}

	public void CompleteSceneLoading()
	{
		if (deferLoadingOverlayUntilSceneReady)
		{
			deferLoadingOverlayUntilSceneReady = false;
			ExitLoadingScene();
		}
	}

	private void ExitLoadingScene()
	{
		if (GodotObject.IsInstanceValid(scaneLoading))
		{
			scaneLoading.Exit();
		}
		scaneLoading = null;
	}

	public SceneManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/SceneManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "stopAllAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReloadScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "stopAllAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BackScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryBeginSceneTransition, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tree", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SceneTree"), exported: false)
			}, null),
			new MethodInfo(MethodName.EndSceneTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableCurrentSceneProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tree", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SceneTree"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearObjectsForSceneChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateLoadingStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryStartThreadedSceneLoad, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureGameplayAtlasesReadyForBattleScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteSceneLoading, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExitLoadingScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeScene && args.Count == 2)
		{
			ChangeScene(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReloadScene && args.Count == 1)
		{
			ReloadScene(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BackScene && args.Count == 0)
		{
			BackScene();
			ret = default;
			return true;
		}
		if (method == MethodName.TryBeginSceneTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryBeginSceneTransition(VariantUtils.ConvertTo<SceneTree>(in args[0])));
			return true;
		}
		if (method == MethodName.EndSceneTransition && args.Count == 0)
		{
			EndSceneTransition();
			ret = default;
			return true;
		}
		if (method == MethodName.DisableCurrentSceneProcessing && args.Count == 1)
		{
			DisableCurrentSceneProcessing(VariantUtils.ConvertTo<SceneTree>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearObjectsForSceneChange && args.Count == 0)
		{
			ClearObjectsForSceneChange();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateLoadingStatus && args.Count == 1)
		{
			UpdateLoadingStatus(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryStartThreadedSceneLoad && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryStartThreadedSceneLoad(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureGameplayAtlasesReadyForBattleScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureGameplayAtlasesReadyForBattleScene());
			return true;
		}
		if (method == MethodName.CompleteSceneLoading && args.Count == 0)
		{
			CompleteSceneLoading();
			ret = default;
			return true;
		}
		if (method == MethodName.ExitLoadingScene && args.Count == 0)
		{
			ExitLoadingScene();
			ret = default;
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.ChangeScene)
		{
			return true;
		}
		if (method == MethodName.ReloadScene)
		{
			return true;
		}
		if (method == MethodName.BackScene)
		{
			return true;
		}
		if (method == MethodName.TryBeginSceneTransition)
		{
			return true;
		}
		if (method == MethodName.EndSceneTransition)
		{
			return true;
		}
		if (method == MethodName.DisableCurrentSceneProcessing)
		{
			return true;
		}
		if (method == MethodName.ClearObjectsForSceneChange)
		{
			return true;
		}
		if (method == MethodName.UpdateLoadingStatus)
		{
			return true;
		}
		if (method == MethodName.TryStartThreadedSceneLoad)
		{
			return true;
		}
		if (method == MethodName.EnsureGameplayAtlasesReadyForBattleScene)
		{
			return true;
		}
		if (method == MethodName.CompleteSceneLoading)
		{
			return true;
		}
		if (method == MethodName.ExitLoadingScene)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.scaneLoading)
		{
			scaneLoading = VariantUtils.ConvertTo<SceneLoading>(in value);
			return true;
		}
		if (name == PropertyName.currentScene)
		{
			currentScene = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentStopAllAudio)
		{
			currentStopAllAudio = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isLoading)
		{
			isLoading = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isSceneChanging)
		{
			isSceneChanging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.deferLoadingOverlayUntilSceneReady)
		{
			deferLoadingOverlayUntilSceneReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentSceneLoading)
		{
			currentSceneLoading = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.sceneStack)
		{
			sceneStack = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.scaneLoading)
		{
			value = VariantUtils.CreateFrom(in scaneLoading);
			return true;
		}
		if (name == PropertyName.currentScene)
		{
			value = VariantUtils.CreateFrom(in currentScene);
			return true;
		}
		if (name == PropertyName.currentStopAllAudio)
		{
			value = VariantUtils.CreateFrom(in currentStopAllAudio);
			return true;
		}
		if (name == PropertyName.isLoading)
		{
			value = VariantUtils.CreateFrom(in isLoading);
			return true;
		}
		if (name == PropertyName.isSceneChanging)
		{
			value = VariantUtils.CreateFrom(in isSceneChanging);
			return true;
		}
		if (name == PropertyName.deferLoadingOverlayUntilSceneReady)
		{
			value = VariantUtils.CreateFrom(in deferLoadingOverlayUntilSceneReady);
			return true;
		}
		if (name == PropertyName.currentSceneLoading)
		{
			value = VariantUtils.CreateFrom(in currentSceneLoading);
			return true;
		}
		if (name == PropertyName.sceneStack)
		{
			value = VariantUtils.CreateFrom(in sceneStack);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.scaneLoading, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.currentStopAllAudio, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isLoading, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSceneChanging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.deferLoadingOverlayUntilSceneReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentSceneLoading, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.sceneStack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.scaneLoading, Variant.From(in scaneLoading));
		info.AddProperty(PropertyName.currentScene, Variant.From(in currentScene));
		info.AddProperty(PropertyName.currentStopAllAudio, Variant.From(in currentStopAllAudio));
		info.AddProperty(PropertyName.isLoading, Variant.From(in isLoading));
		info.AddProperty(PropertyName.isSceneChanging, Variant.From(in isSceneChanging));
		info.AddProperty(PropertyName.deferLoadingOverlayUntilSceneReady, Variant.From(in deferLoadingOverlayUntilSceneReady));
		info.AddProperty(PropertyName.currentSceneLoading, Variant.From(in currentSceneLoading));
		info.AddProperty(PropertyName.sceneStack, Variant.From(in sceneStack));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.scaneLoading, out var value))
		{
			scaneLoading = value.As<SceneLoading>();
		}
		if (info.TryGetProperty(PropertyName.currentScene, out var value2))
		{
			currentScene = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentStopAllAudio, out var value3))
		{
			currentStopAllAudio = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isLoading, out var value4))
		{
			isLoading = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isSceneChanging, out var value5))
		{
			isSceneChanging = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.deferLoadingOverlayUntilSceneReady, out var value6))
		{
			deferLoadingOverlayUntilSceneReady = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentSceneLoading, out var value7))
		{
			currentSceneLoading = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.sceneStack, out var value8))
		{
			sceneStack = value8.As<Array>();
		}
	}
}
