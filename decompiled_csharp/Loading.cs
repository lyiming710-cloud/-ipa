using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Scene/Loading/Loading.cs")]
public class Loading : Control
{
	private enum ModInstallConfirmation
	{
		Confirmed,
		Cancelled,
		Unavailable
	}

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BindBootstrapAtlasProfile = "BindBootstrapAtlasProfile";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName UpdateLoadingAnimationPlayback = "UpdateLoadingAnimationPlayback";

		public static readonly StringName ResumeLoadingAnimation = "ResumeLoadingAnimation";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName StartResourceLoadAfterFirstDraw = "StartResourceLoadAfterFirstDraw";

		public static readonly StringName ApplyDeferredLoadingFont = "ApplyDeferredLoadingFont";

		public static readonly StringName ActivateLoadingAnimations = "ActivateLoadingAnimations";

		public static readonly StringName ActivateLoadingAnimation = "ActivateLoadingAnimation";

		public static readonly StringName DisconnectLoadingAnimationCompletion = "DisconnectLoadingAnimationCompletion";

		public static readonly StringName OnLoadingAnimationCompleted = "OnLoadingAnimationCompleted";

		public static readonly StringName ShouldIgnorePressStart = "ShouldIgnorePressStart";

		public static readonly StringName IsClearCacheDialogVisible = "IsClearCacheDialogVisible";

		public static readonly StringName IsClearCacheButtonFocused = "IsClearCacheButtonFocused";

		public static readonly StringName IsPointerOverClearCacheButton = "IsPointerOverClearCacheButton";

		public static readonly StringName OnlineButtonPressed = "OnlineButtonPressed";

		public static readonly StringName OfflineButtonPressed = "OfflineButtonPressed";

		public static readonly StringName SetPercentage = "SetPercentage";

		public static readonly StringName LoadOver = "LoadOver";

		public static readonly StringName LoadStartupModsOnce = "LoadStartupModsOnce";

		public static readonly StringName LoadEnabledStartupMods = "LoadEnabledStartupMods";

		public static readonly StringName SafeDialogField = "SafeDialogField";

		public static readonly StringName ManifestIdSha256 = "ManifestIdSha256";

		public static readonly StringName ReportStartupModImportResult = "ReportStartupModImportResult";

		public static readonly StringName ShowStartupModImportResult = "ShowStartupModImportResult";

		public static readonly StringName EscapeBbcode = "EscapeBbcode";

		public static readonly StringName TryFinishLoadingPresentation = "TryFinishLoadingPresentation";

		public static readonly StringName LoadFailed = "LoadFailed";

		public static readonly StringName UpdateLoadingStepText = "UpdateLoadingStepText";

		public static readonly StringName BuildLoadingText = "BuildLoadingText";

		public static readonly StringName TranslateLoadingStep = "TranslateLoadingStep";

		public static readonly StringName EnsureSodRollCapTexture = "EnsureSodRollCapTexture";

		public static readonly StringName CanAnimateSodRollCap = "CanAnimateSodRollCap";

		public static readonly StringName ShowClearLoadingCacheDialog = "ShowClearLoadingCacheDialog";

		public static readonly StringName BuildClearCacheDialogText = "BuildClearCacheDialogText";

		public static readonly StringName ClearLoadingCacheConfirmed = "ClearLoadingCacheConfirmed";

		public static readonly StringName ClearLoadedMapResourceCaches = "ClearLoadedMapResourceCaches";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName LoadingFirstFramePresentedUsec = "LoadingFirstFramePresentedUsec";

		public static readonly StringName BootstrapFirstFramePresentedUsec = "BootstrapFirstFramePresentedUsec";

		public static readonly StringName GameplayResourceLoadRequestedUsec = "GameplayResourceLoadRequestedUsec";

		public static readonly StringName _loadLabel = "_loadLabel";

		public static readonly StringName _offlineButton = "_offlineButton";

		public static readonly StringName _progressBar = "_progressBar";

		public static readonly StringName _startButton = "_startButton";

		public static readonly StringName _clearCacheButton = "_clearCacheButton";

		public static readonly StringName _clearCacheDialog = "_clearCacheDialog";

		public static readonly StringName _sodRollCap = "_sodRollCap";

		public static readonly StringName _loadBarSprout = "_loadBarSprout";

		public static readonly StringName _loadBarSprout2 = "_loadBarSprout2";

		public static readonly StringName _loadBarSprout3 = "_loadBarSprout3";

		public static readonly StringName _loadBarSprout4 = "_loadBarSprout4";

		public static readonly StringName _loadBarZombieHead = "_loadBarZombieHead";

		public static readonly StringName _percentage = "_percentage";

		public static readonly StringName _isStart = "_isStart";

		public static readonly StringName _currentValue = "_currentValue";

		public static readonly StringName _loadCompleted = "_loadCompleted";

		public static readonly StringName _loadingPresentationCompleted = "_loadingPresentationCompleted";

		public static readonly StringName _startupModFlowStarted = "_startupModFlowStarted";

		public static readonly StringName _startupModsReady = "_startupModsReady";

		public static readonly StringName _completedLoadingAnimationCount = "_completedLoadingAnimationCount";

		public static readonly StringName _startupImportMessage = "_startupImportMessage";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string SodRollCapTexturePath = "res://Asset/Texture/GUI/General/Loading/SodRollCap.png";

	private const string LoadingFontPath = "res://Asset/Font/fzkt.ttf";

	private const string BootstrapAtlasProfilePath = "res://addons/AdobeAnimateEditor/GeneratedAtlas/Bootstrap/AdobeAnimateBootstrapAtlasProfile.tres";

	private const string LoadingAnimationClip = "Idle";

	private const int LoadingAnimationCount = 5;

	private const float LoadingPresentationCompleteProgress = 0.95f;

	private const float MaxPresentationDeltaSeconds = 1f / 30f;

	private Label _loadLabel;

	private MainButton _offlineButton;

	private TextureProgressBar _progressBar;

	private Button _startButton;

	private MainButton _clearCacheButton;

	private DialogBoxChoose _clearCacheDialog;

	private Sprite2D _sodRollCap;

	private AdobeAnimateSpriteBase _loadBarSprout;

	private AdobeAnimateSpriteBase _loadBarSprout2;

	private AdobeAnimateSpriteBase _loadBarSprout3;

	private AdobeAnimateSpriteBase _loadBarSprout4;

	private AdobeAnimateSpriteBase _loadBarZombieHead;

	private float _percentage;

	private bool _isStart;

	private float _currentValue;

	private bool _loadCompleted;

	private bool _loadingPresentationCompleted;

	private static bool _startupModsLoaded = false;

	private static readonly SemaphoreSlim StartupModsGate = new SemaphoreSlim(1, 1);

	private bool _startupModFlowStarted;

	private bool _startupModsReady;

	private int _completedLoadingAnimationCount;

	private bool? _startupImportSuccess;

	private string _startupImportMessage = "";

	private XWModStartupReport _startupReport;

	public ulong LoadingFirstFramePresentedUsec { get; private set; }

	public ulong BootstrapFirstFramePresentedUsec { get; private set; }

	public ulong GameplayResourceLoadRequestedUsec { get; private set; }

	public override void _Ready()
	{
		StartupLoadDiagnostics.Mark("loading.ready.begin");
		_loadLabel = GetNode<Label>("%LoadLabel");
		_offlineButton = GetNode<MainButton>("%OfflineButton");
		_progressBar = GetNode<TextureProgressBar>("%ProgressBar");
		_startButton = GetNode<Button>("%StartButton");
		_clearCacheButton = GetNode<MainButton>("%ClearCacheButton");
		_sodRollCap = GetNode<Sprite2D>("%SodRollCap");
		EnsureSodRollCapTexture();
		_startButton.Text = Tr("LOADING_IN_PROGRESS");
		_clearCacheButton.text = Tr("CLEAR_LOADING_CACHE");
		_clearCacheButton.Visible = false;
		_clearCacheButton.Disabled = true;
		_clearCacheButton.Pressed += ShowClearLoadingCacheDialog;
		_loadBarSprout = GetNode<AdobeAnimateSpriteBase>("%LoadBarSprout");
		_loadBarSprout2 = GetNode<AdobeAnimateSpriteBase>("%LoadBarSprout2");
		_loadBarSprout3 = GetNode<AdobeAnimateSpriteBase>("%LoadBarSprout3");
		_loadBarSprout4 = GetNode<AdobeAnimateSpriteBase>("%LoadBarSprout4");
		_loadBarZombieHead = GetNode<AdobeAnimateSpriteBase>("%LoadBarZombieHead");
		BindBootstrapAtlasProfile();
		ResourceManager.Instance.OnLoadPercentage += SetPercentage;
		ResourceManager.Instance.OnLoadOver += LoadOver;
		ResourceManager.Instance.OnLoadFailed += LoadFailed;
		GetNode<BaseButton>("HBoxContainer/OnlineButton").Pressed += OnlineButtonPressed;
		GetNode<BaseButton>("%OfflineButton").Pressed += OfflineButtonPressed;
		StartResourceLoadAfterFirstDraw();
		StartupLoadDiagnostics.Mark("loading.ready.end");
		StartupLoadDiagnostics.StopObservingTree();
	}

	private void BindBootstrapAtlasProfile()
	{
		AdobeAnimateAtlasProfile adobeAnimateAtlasProfile = ResourceLoader.Load<AdobeAnimateAtlasProfile>("res://addons/AdobeAnimateEditor/GeneratedAtlas/Bootstrap/AdobeAnimateBootstrapAtlasProfile.tres", null, ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(adobeAnimateAtlasProfile))
		{
			throw new InvalidOperationException("Bootstrap Adobe Animate atlas profile is unavailable: res://addons/AdobeAnimateEditor/GeneratedAtlas/Bootstrap/AdobeAnimateBootstrapAtlasProfile.tres");
		}
		_loadBarSprout.atlasProfileOverride = adobeAnimateAtlasProfile;
		_loadBarSprout2.atlasProfileOverride = adobeAnimateAtlasProfile;
		_loadBarSprout3.atlasProfileOverride = adobeAnimateAtlasProfile;
		_loadBarSprout4.atlasProfileOverride = adobeAnimateAtlasProfile;
		_loadBarZombieHead.atlasProfileOverride = adobeAnimateAtlasProfile;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		DisconnectLoadingAnimationCompletion(_loadBarSprout);
		DisconnectLoadingAnimationCompletion(_loadBarSprout2);
		DisconnectLoadingAnimationCompletion(_loadBarSprout3);
		DisconnectLoadingAnimationCompletion(_loadBarSprout4);
		DisconnectLoadingAnimationCompletion(_loadBarZombieHead);
		if (ResourceManager.Instance != null)
		{
			ResourceManager.Instance.OnLoadPercentage -= SetPercentage;
			ResourceManager.Instance.OnLoadOver -= LoadOver;
			ResourceManager.Instance.OnLoadFailed -= LoadFailed;
		}
	}

	public override void _Process(double delta)
	{
		if (_currentValue < _percentage)
		{
			float num = Mathf.Min((float)delta, 1f / 30f);
			_currentValue = Mathf.Lerp(_currentValue, _percentage, Mathf.Min((_currentValue * 2f + 0.5f) * num, 1f));
			_progressBar.Value = _currentValue;
			UpdateLoadingAnimationPlayback(_currentValue);
			if (CanAnimateSodRollCap())
			{
				_sodRollCap.Position = new Vector2(_currentValue * 315f, -26f + _currentValue * 30f);
				_sodRollCap.Rotation = (float)Math.PI * 4f * _currentValue;
				_sodRollCap.Scale = Vector2.One * (1f - _currentValue * 0.75f);
			}
		}
		if (_currentValue >= 0.95f && GodotObject.IsInstanceValid(_sodRollCap))
		{
			_sodRollCap.Visible = false;
		}
		TryFinishLoadingPresentation();
	}

	private void UpdateLoadingAnimationPlayback(float progress, bool playAudio = true)
	{
		ResumeLoadingAnimation(_loadBarSprout, progress, 0.15f, playAudio);
		ResumeLoadingAnimation(_loadBarSprout2, progress, 0.3f, playAudio);
		ResumeLoadingAnimation(_loadBarSprout3, progress, 0.4f, playAudio);
		ResumeLoadingAnimation(_loadBarSprout4, progress, 0.6f, playAudio);
		if (GodotObject.IsInstanceValid(_loadBarZombieHead) && _loadBarZombieHead.pause && !(progress < 0.8f))
		{
			if (playAudio)
			{
				AudioManager.Instance.AudioPlay("LoadingBarFlower");
				AudioManager.Instance.AudioPlay("LoadingBarZombie");
			}
			_loadBarZombieHead.SetAnimation("Idle", loop: false);
			_loadBarZombieHead.pause = false;
		}
	}

	private static void ResumeLoadingAnimation(AdobeAnimateSpriteBase animation, float progress, float threshold, bool playAudio)
	{
		if (GodotObject.IsInstanceValid(animation) && animation.pause && !(progress < threshold))
		{
			if (playAudio)
			{
				AudioManager.Instance.AudioPlay("LoadingBarFlower");
			}
			animation.SetAnimation("Idle", loop: false);
			animation.pause = false;
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!_isStart && !_loadLabel.Visible && Input.IsActionJustPressed("Press") && !ShouldIgnorePressStart(inputEvent))
		{
			AudioManager.Instance.AudioPlay("ButtonPress");
			OfflineButtonPressed();
		}
	}

	private async void StartResourceLoadAfterFirstDraw()
	{
		UpdateLoadingStepText("LOAD_STARTUP");
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		LoadingFirstFramePresentedUsec = Time.GetTicksUsec();
		StartupLoadDiagnostics.Mark("loading.first_frame");
		if (!IsInsideTree())
		{
			return;
		}
		StartupLoadDiagnostics.Mark("loading.font.begin");
		ApplyDeferredLoadingFont();
		StartupLoadDiagnostics.Mark("loading.font.end");
		ResourceManager resourceManager = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(resourceManager))
		{
			GD.PushError("Loading cannot continue because ResourceManager is unavailable.");
			return;
		}
		StartupLoadDiagnostics.Mark("loading.bootstrap.begin");
		ActivateLoadingAnimations();
		StartupLoadDiagnostics.Mark("loading.bootstrap.end");
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		if (IsInsideTree())
		{
			BootstrapFirstFramePresentedUsec = Time.GetTicksUsec();
			StartupLoadDiagnostics.Mark("loading.bootstrap_frame");
			GameplayResourceLoadRequestedUsec = Time.GetTicksUsec();
			resourceManager.BeginGameplayAtlasPreload();
			StartupLoadDiagnostics.Mark("loading.savedata.begin");
			GameSaveManager.Instance?.EnsureLoaded();
			StartupLoadDiagnostics.Mark("loading.savedata.end");
			resourceManager.BeginLoad();
		}
	}

	private void ApplyDeferredLoadingFont()
	{
		if (GodotObject.IsInstanceValid(_startButton))
		{
			FontFile fontFile = GD.Load<FontFile>("res://Asset/Font/fzkt.ttf");
			if (GodotObject.IsInstanceValid(fontFile))
			{
				_startButton.AddThemeFontOverride("font", fontFile);
			}
		}
	}

	private void ActivateLoadingAnimations()
	{
		ActivateLoadingAnimation(_loadBarSprout);
		ActivateLoadingAnimation(_loadBarSprout2);
		ActivateLoadingAnimation(_loadBarSprout3);
		ActivateLoadingAnimation(_loadBarSprout4);
		ActivateLoadingAnimation(_loadBarZombieHead);
	}

	private void ActivateLoadingAnimation(AdobeAnimateSpriteBase animation)
	{
		if (GodotObject.IsInstanceValid(animation))
		{
			animation.Visible = true;
			animation.pause = true;
			animation.OnAnimeCompleted -= OnLoadingAnimationCompleted;
			animation.OnAnimeCompleted += OnLoadingAnimationCompleted;
			animation.SetAnimation("Idle", loop: false);
		}
	}

	private void DisconnectLoadingAnimationCompletion(AdobeAnimateSpriteBase animation)
	{
		if (GodotObject.IsInstanceValid(animation))
		{
			animation.OnAnimeCompleted -= OnLoadingAnimationCompleted;
		}
	}

	private void OnLoadingAnimationCompleted(string clipName)
	{
		if (!(clipName != "Idle") && _completedLoadingAnimationCount < 5)
		{
			_completedLoadingAnimationCount++;
			TryFinishLoadingPresentation();
		}
	}

	private bool ShouldIgnorePressStart(InputEvent inputEvent)
	{
		if (!IsClearCacheDialogVisible() && !IsPointerOverClearCacheButton(inputEvent))
		{
			return IsClearCacheButtonFocused();
		}
		return true;
	}

	private bool IsClearCacheDialogVisible()
	{
		if (GodotObject.IsInstanceValid(_clearCacheDialog))
		{
			return _clearCacheDialog.Visible;
		}
		return false;
	}

	private bool IsClearCacheButtonFocused()
	{
		if (GodotObject.IsInstanceValid(_clearCacheButton) && _clearCacheButton.Visible)
		{
			return _clearCacheButton.HasFocus();
		}
		return false;
	}

	private bool IsPointerOverClearCacheButton(InputEvent inputEvent)
	{
		if (!GodotObject.IsInstanceValid(_clearCacheButton) || !_clearCacheButton.Visible || _clearCacheButton.Disabled)
		{
			return false;
		}
		if (!(inputEvent is InputEventMouseButton { Pressed: not false } inputEventMouseButton))
		{
			return false;
		}
		return _clearCacheButton.GetGlobalRect().HasPoint(inputEventMouseButton.Position);
	}

	public void OnlineButtonPressed()
	{
		ResourceManager.Instance.RequireFullGameplayResourcesReady("OnlineButtonPressed");
		_isStart = true;
		GameSaveManager.Instance.EnsureLoaded();
		SceneManager.Instance.ChangeScene("MainMenu", stopAllAudio: false);
	}

	public void OfflineButtonPressed()
	{
		ResourceManager.Instance.RequireFullGameplayResourcesReady("OfflineButtonPressed");
		_isStart = true;
		GameSaveManager.Instance.EnsureLoaded();
		SceneManager.Instance.ChangeScene("MainMenu", stopAllAudio: false);
	}

	public void SetPercentage(double percentage, string stepName, string resourceName, int resourceIndex, int resourceTotal)
	{
		_percentage = (float)percentage;
		UpdateLoadingStepText(stepName, resourceName, resourceIndex, resourceTotal);
	}

	public void LoadOver()
	{
		_loadCompleted = true;
		_percentage = 1f;
		ThumbnailBinaryResourceCache.ClearOutdatedVersionCaches();
		ThumbnailBinaryResourceCache.ClearLegacyCharacterThumbnailCategories();
		LoadStartupModsOnce();
		TryFinishLoadingPresentation();
		AudioManager.Instance.AudioPlay("MainMenu", AudioManagerEnum.TYPE.MUSIC);
	}

	private async void LoadStartupModsOnce()
	{
		if (_startupModFlowStarted)
		{
			return;
		}
		_startupModFlowStarted = true;
		await StartupModsGate.WaitAsync();
		try
		{
			if (_startupModsLoaded || !IsInsideTree())
			{
				return;
			}
			XWModPackageInstaller.CleanupInstallArtifacts();
			AndroidModInstallIntent.PrepareResult prepareResult = AndroidModInstallIntent.PrepareStartupIntent();
			if (prepareResult.RequiresConfirmation)
			{
				using AndroidModInstallIntent.PreparedStartupRequest request = prepareResult.Request;
				switch (await ConfirmStartupModInstall(request))
				{
				case ModInstallConfirmation.Confirmed:
				{
					AndroidModInstallIntent.StartupResult startupResult = AndroidModInstallIntent.InstallPreparedRequest(request);
					ReportStartupModImportResult(startupResult.Success, startupResult.Message);
					return;
				}
				case ModInstallConfirmation.Cancelled:
					GD.Print("[Loading] Android Mod import cancelled: " + request.Manifest?.Id);
					return;
				}
				if (IsInsideTree())
				{
					ReportStartupModImportResult(success: false, "无法显示 Mod 安装确认窗口，已取消本次安装。");
				}
			}
			else if (prepareResult.Handled)
			{
				ReportStartupModImportResult(prepareResult.Success, prepareResult.Message);
			}
		}
		catch (Exception ex)
		{
			GD.PushError("[Loading] Mod startup load failed: " + ex.GetBaseException().Message);
		}
		finally
		{
			if (!_startupModsLoaded && IsInsideTree())
			{
				LoadEnabledStartupMods();
				_startupModsLoaded = true;
			}
			if (_startupModsLoaded && IsInsideTree())
			{
				_startupModsReady = true;
				TryFinishLoadingPresentation();
			}
			StartupModsGate.Release();
		}
	}

	private void LoadEnabledStartupMods()
	{
		try
		{
			_startupReport = ModLoader.LoadAllDetailed();
			int legacyCount = _startupReport.LegacyCount;
			if (legacyCount < 0)
			{
				GD.PushWarning("[Loading] Mod startup load completed with a blocked rollback; see Mod diagnostics");
				return;
			}
			GD.Print($"[Loading] loaded {legacyCount} enabled Mod package(s)");
		}
		catch (Exception ex)
		{
			GD.PushError("[Loading] Mod startup load failed: " + ex.GetBaseException().Message);
			_startupReport = new XWModStartupReport
			{
				LegacyCount = -1,
				BlockingReason = ex.GetBaseException().Message
			};
		}
		finally
		{
			if (_startupImportSuccess.HasValue)
			{
				goto IL_00c5;
			}
			XWModStartupReport startupReport = _startupReport;
			if (startupReport != null && startupReport.HasFailures)
			{
				goto IL_00c5;
			}
			goto end_IL_00a4;
			IL_00c5:
			CallDeferred("ShowStartupModImportResult", _startupImportSuccess ?? true, _startupImportMessage);
			end_IL_00a4:;
		}
	}

	private async Task<ModInstallConfirmation> ConfirmStartupModInstall(AndroidModInstallIntent.PreparedStartupRequest request)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		if (!IsInsideTree() || !GodotObject.IsInstanceValid(DialogManager.Instance))
		{
			return ModInstallConfirmation.Unavailable;
		}
		DialogBoxChoose dialog = DialogManager.Instance.DialogCreate("DialogBoxChoose") as DialogBoxChoose;
		if (!GodotObject.IsInstanceValid(dialog))
		{
			return ModInstallConfirmation.Unavailable;
		}
		TaskCompletionSource<ModInstallConfirmation> completion = new TaskCompletionSource<ModInstallConfirmation>();
		dialog.text = BuildStartupModInstallConfirmation(request);
		dialog.OnChooseTrue += Confirmed;
		dialog.OnChooseFalse += Cancelled;
		dialog.OnClose += DialogClosed;
		dialog.TreeExited += DialogExited;
		TreeExited += LoadingExited;
		try
		{
			return await completion.Task;
		}
		finally
		{
			TreeExited -= LoadingExited;
			if (GodotObject.IsInstanceValid(dialog))
			{
				dialog.OnChooseTrue -= Confirmed;
				dialog.OnChooseFalse -= Cancelled;
				dialog.OnClose -= DialogClosed;
				dialog.TreeExited -= DialogExited;
			}
		}
		void Cancelled()
		{
			completion.TrySetResult(ModInstallConfirmation.Cancelled);
		}
		void Confirmed()
		{
			completion.TrySetResult(ModInstallConfirmation.Confirmed);
		}
		void DialogClosed()
		{
			completion.TrySetResult(ModInstallConfirmation.Cancelled);
		}
		void DialogExited()
		{
			completion.TrySetResult(ModInstallConfirmation.Unavailable);
		}
		void LoadingExited()
		{
			completion.TrySetResult(ModInstallConfirmation.Unavailable);
		}
	}

	private static string BuildStartupModInstallConfirmation(AndroidModInstallIntent.PreparedStartupRequest request)
	{
		XWModManifest manifest = request.Manifest;
		string value = (string.IsNullOrWhiteSpace(manifest?.Author) ? "未声明" : manifest.Author);
		return "[center][font_size=24]Mod 安装确认[/font_size][/center]\n[font_size=17]名称：" + SafeDialogField(manifest?.Name, 80) + "\nID：" + SafeDialogField(manifest?.Id, 128) + "\nID SHA256：" + ManifestIdSha256(manifest?.Id) + "\n版本：" + SafeDialogField(manifest?.Version, 64) + "\n作者：" + SafeDialogField(value, 80) + "\n来源：" + SafeDialogField(request.SourceAuthority, 128) + "\nSHA256：" + SafeDialogField(request.Sha256, 64) + "[/font_size]\n[center]确认后将安装并启用此 Mod。[/center]";
	}

	private static string SafeDialogField(string value, int maxLength)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = value ?? "";
		foreach (char c in text)
		{
			UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
			if (char.IsControl(c) || unicodeCategory == UnicodeCategory.Format)
			{
				stringBuilder.Append(' ');
			}
			else
			{
				stringBuilder.Append(char.IsWhiteSpace(c) ? ' ' : c);
			}
		}
		string text2 = string.Join(" ", stringBuilder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries));
		if (text2.Length > maxLength)
		{
			int num = maxLength;
			if (num > 0 && char.IsHighSurrogate(text2[num - 1]) && char.IsLowSurrogate(text2[num]))
			{
				num--;
			}
			text2 = text2.Substring(0, num) + "…";
		}
		return EscapeBbcode(text2);
	}

	private static string ManifestIdSha256(string value)
	{
		return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value ?? "")));
	}

	private void ReportStartupModImportResult(bool success, string message)
	{
		if (success)
		{
			GD.Print("[Loading] Android Mod import completed: " + message);
		}
		else
		{
			GD.PushWarning("[Loading] Android Mod import rejected: " + message);
		}
		_startupImportSuccess = success;
		_startupImportMessage = message;
	}

	private void ShowStartupModImportResult(bool success, string message)
	{
		if (DialogManager.Instance == null || !(DialogManager.Instance.DialogCreate("DialogBoxTips") is DialogBoxTips dialogBoxTips))
		{
			return;
		}
		string value;
		if (_startupImportSuccess.HasValue)
		{
			value = (success ? "Mod 安装完成" : "Mod 安装失败");
		}
		else
		{
			value = "Mod 加载结果";
		}
		string value2 = ((!success) ? (message + "\n") : "") + (_startupReport?.PlayerSummary() ?? "加载结果暂不可用，请查看 Mod 管理。");
		dialogBoxTips.text = $"[center][font_size=24]{value}[/font_size]\n{SafeDialogField(value2, 1600)}[/center]";
		XWModStartupReport startupReport = _startupReport;
		if (startupReport == null || !startupReport.HasFailures)
		{
			return;
		}
		dialogBoxTips.GetNode<BaseButton>("%ConfirmButton").Set("text", "查看 Mod 管理");
		dialogBoxTips.OnClose += () =>
		{
			if (DialogManager.Instance.DialogCreate("MainMenuOption") is DialogMainMenuOption dialogMainMenuOption)
			{
				dialogMainMenuOption.AllowModNavigation = false;
				Callable.From(dialogMainMenuOption.OpenManagement).CallDeferred();
			}
		};
	}

	private static string EscapeBbcode(string value)
	{
		return (value ?? "").Replace("[", "［", StringComparison.Ordinal).Replace("]", "］", StringComparison.Ordinal);
	}

	private void TryFinishLoadingPresentation()
	{
		if (_loadCompleted && _startupModsReady && !_loadingPresentationCompleted && !(_currentValue < 0.95f) && _completedLoadingAnimationCount >= 5)
		{
			_loadingPresentationCompleted = true;
			_currentValue = 1f;
			_progressBar.Value = 1.0;
			if (GodotObject.IsInstanceValid(_sodRollCap))
			{
				_sodRollCap.Visible = false;
			}
			_startButton.Disabled = false;
			_startButton.Text = Tr("CLICK_TO_START");
			_clearCacheButton.Visible = true;
			_clearCacheButton.Disabled = false;
			_loadLabel.Visible = false;
		}
	}

	private void LoadFailed(string error)
	{
		_loadCompleted = false;
		_loadingPresentationCompleted = false;
		_startButton.Disabled = true;
		_startButton.Text = Tr("LOADING_FAILED");
		_loadLabel.Visible = true;
		_loadLabel.Text = Tr("LOADING_FAILED") + ": " + error;
		_clearCacheButton.Visible = true;
		_clearCacheButton.Disabled = false;
	}

	private void UpdateLoadingStepText(string stepName, string resourceName = "", int resourceIndex = 0, int resourceTotal = 0)
	{
		if (!string.IsNullOrEmpty(stepName))
		{
			_loadLabel.Text = BuildLoadingText(stepName, resourceName, resourceIndex, resourceTotal);
		}
	}

	private string BuildLoadingText(string stepName, string resourceName, int resourceIndex, int resourceTotal)
	{
		string text = TranslateLoadingStep(stepName);
		if (!string.IsNullOrEmpty(resourceName))
		{
			text = text + ": " + TranslateLoadingStep(resourceName);
		}
		if (resourceTotal > 0)
		{
			text += $" {resourceIndex}/{resourceTotal}";
		}
		return text;
	}

	private string TranslateLoadingStep(string stepName)
	{
		return TranslationServer.Translate(stepName);
	}

	private void EnsureSodRollCapTexture()
	{
		if (GodotObject.IsInstanceValid(_sodRollCap) && !GodotObject.IsInstanceValid(_sodRollCap.Texture))
		{
			_sodRollCap.Texture = GD.Load<Texture2D>("res://Asset/Texture/GUI/General/Loading/SodRollCap.png");
			if (!GodotObject.IsInstanceValid(_sodRollCap.Texture))
			{
				_sodRollCap.Visible = false;
			}
		}
	}

	private bool CanAnimateSodRollCap()
	{
		if (GodotObject.IsInstanceValid(_sodRollCap))
		{
			return GodotObject.IsInstanceValid(_sodRollCap.Texture);
		}
		return false;
	}

	private void ShowClearLoadingCacheDialog()
	{
		if (!IsClearCacheDialogVisible() && GodotObject.IsInstanceValid(DialogManager.Instance))
		{
			_clearCacheDialog = (DialogBoxChoose)DialogManager.Instance.DialogCreate("DialogBoxChoose");
			_clearCacheDialog.text = BuildClearCacheDialogText();
			_clearCacheDialog.OnChooseTrue += ClearLoadingCacheConfirmed;
		}
	}

	private string BuildClearCacheDialogText()
	{
		return $"[center][font_size=24]{Tr("CLEAR_LOADING_CACHE")}[/font_size][/center]\n[center][font_size=16]{Tr("CLEAR_LOADING_CACHE_CONFIRM")}[/font_size][/center]";
	}

	private void ClearLoadingCacheConfirmed()
	{
		int value = CharacterBinaryResourceCache.ClearAllCaches();
		int value2 = ClearLoadedMapResourceCaches();
		GD.Print($"Loading cache cleared: characterBinaryEntries={value}, mapConfigs={value2}");
		if (GodotObject.IsInstanceValid(_clearCacheButton))
		{
			_clearCacheButton.text = Tr("CLEAR_LOADING_CACHE_DONE");
		}
	}

	private int ClearLoadedMapResourceCaches()
	{
		if (ResourceManager.Instance == null)
		{
			return 0;
		}
		int num = 0;
		foreach (Resource value in ResourceManager.Instance.MAPS.Values)
		{
			if (value is TowerDefenseMapConfig towerDefenseMapConfig)
			{
				towerDefenseMapConfig.ClearLoadedMapResources(clearThumbnail: true);
				num++;
			}
		}
		return num;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(39)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindBootstrapAtlasProfile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateLoadingAnimationPlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "progress", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResumeLoadingAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "progress", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "threshold", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.StartResourceLoadAfterFirstDraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyDeferredLoadingFont, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateLoadingAnimations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateLoadingAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisconnectLoadingAnimationCompletion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnLoadingAnimationCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldIgnorePressStart, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsClearCacheDialogVisible, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsClearCacheButtonFocused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPointerOverClearCacheButton, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnlineButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OfflineButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPercentage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "stepName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadStartupModsOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadEnabledStartupMods, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SafeDialogField, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maxLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ManifestIdSha256, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReportStartupModImportResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "success", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowStartupModImportResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "success", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EscapeBbcode, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryFinishLoadingPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFailed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateLoadingStepText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stepName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildLoadingText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stepName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "resourceTotal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TranslateLoadingStep, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stepName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureSodRollCapTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanAnimateSodRollCap, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowClearLoadingCacheDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildClearCacheDialogText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearLoadingCacheConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearLoadedMapResourceCaches, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BindBootstrapAtlasProfile && args.Count == 0)
		{
			BindBootstrapAtlasProfile();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateLoadingAnimationPlayback && args.Count == 2)
		{
			UpdateLoadingAnimationPlayback(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeLoadingAnimation && args.Count == 4)
		{
			ResumeLoadingAnimation(VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartResourceLoadAfterFirstDraw && args.Count == 0)
		{
			StartResourceLoadAfterFirstDraw();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDeferredLoadingFont && args.Count == 0)
		{
			ApplyDeferredLoadingFont();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateLoadingAnimations && args.Count == 0)
		{
			ActivateLoadingAnimations();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateLoadingAnimation && args.Count == 1)
		{
			ActivateLoadingAnimation(VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectLoadingAnimationCompletion && args.Count == 1)
		{
			DisconnectLoadingAnimationCompletion(VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLoadingAnimationCompleted && args.Count == 1)
		{
			OnLoadingAnimationCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldIgnorePressStart && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldIgnorePressStart(VariantUtils.ConvertTo<InputEvent>(in args[0])));
			return true;
		}
		if (method == MethodName.IsClearCacheDialogVisible && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsClearCacheDialogVisible());
			return true;
		}
		if (method == MethodName.IsClearCacheButtonFocused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsClearCacheButtonFocused());
			return true;
		}
		if (method == MethodName.IsPointerOverClearCacheButton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPointerOverClearCacheButton(VariantUtils.ConvertTo<InputEvent>(in args[0])));
			return true;
		}
		if (method == MethodName.OnlineButtonPressed && args.Count == 0)
		{
			OnlineButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OfflineButtonPressed && args.Count == 0)
		{
			OfflineButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPercentage && args.Count == 5)
		{
			SetPercentage(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadOver && args.Count == 0)
		{
			LoadOver();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadStartupModsOnce && args.Count == 0)
		{
			LoadStartupModsOnce();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadEnabledStartupMods && args.Count == 0)
		{
			LoadEnabledStartupMods();
			ret = default;
			return true;
		}
		if (method == MethodName.SafeDialogField && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(SafeDialogField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ManifestIdSha256 && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ManifestIdSha256(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReportStartupModImportResult && args.Count == 2)
		{
			ReportStartupModImportResult(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowStartupModImportResult && args.Count == 2)
		{
			ShowStartupModImportResult(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryFinishLoadingPresentation && args.Count == 0)
		{
			TryFinishLoadingPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadFailed && args.Count == 1)
		{
			LoadFailed(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateLoadingStepText && args.Count == 4)
		{
			UpdateLoadingStepText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLoadingText && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<string>(BuildLoadingText(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.TranslateLoadingStep && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(TranslateLoadingStep(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureSodRollCapTexture && args.Count == 0)
		{
			EnsureSodRollCapTexture();
			ret = default;
			return true;
		}
		if (method == MethodName.CanAnimateSodRollCap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAnimateSodRollCap());
			return true;
		}
		if (method == MethodName.ShowClearLoadingCacheDialog && args.Count == 0)
		{
			ShowClearLoadingCacheDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildClearCacheDialogText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildClearCacheDialogText());
			return true;
		}
		if (method == MethodName.ClearLoadingCacheConfirmed && args.Count == 0)
		{
			ClearLoadingCacheConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearLoadedMapResourceCaches && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ClearLoadedMapResourceCaches());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResumeLoadingAnimation && args.Count == 4)
		{
			ResumeLoadingAnimation(VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SafeDialogField && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(SafeDialogField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ManifestIdSha256 && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ManifestIdSha256(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.BindBootstrapAtlasProfile)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.UpdateLoadingAnimationPlayback)
		{
			return true;
		}
		if (method == MethodName.ResumeLoadingAnimation)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.StartResourceLoadAfterFirstDraw)
		{
			return true;
		}
		if (method == MethodName.ApplyDeferredLoadingFont)
		{
			return true;
		}
		if (method == MethodName.ActivateLoadingAnimations)
		{
			return true;
		}
		if (method == MethodName.ActivateLoadingAnimation)
		{
			return true;
		}
		if (method == MethodName.DisconnectLoadingAnimationCompletion)
		{
			return true;
		}
		if (method == MethodName.OnLoadingAnimationCompleted)
		{
			return true;
		}
		if (method == MethodName.ShouldIgnorePressStart)
		{
			return true;
		}
		if (method == MethodName.IsClearCacheDialogVisible)
		{
			return true;
		}
		if (method == MethodName.IsClearCacheButtonFocused)
		{
			return true;
		}
		if (method == MethodName.IsPointerOverClearCacheButton)
		{
			return true;
		}
		if (method == MethodName.OnlineButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OfflineButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SetPercentage)
		{
			return true;
		}
		if (method == MethodName.LoadOver)
		{
			return true;
		}
		if (method == MethodName.LoadStartupModsOnce)
		{
			return true;
		}
		if (method == MethodName.LoadEnabledStartupMods)
		{
			return true;
		}
		if (method == MethodName.SafeDialogField)
		{
			return true;
		}
		if (method == MethodName.ManifestIdSha256)
		{
			return true;
		}
		if (method == MethodName.ReportStartupModImportResult)
		{
			return true;
		}
		if (method == MethodName.ShowStartupModImportResult)
		{
			return true;
		}
		if (method == MethodName.EscapeBbcode)
		{
			return true;
		}
		if (method == MethodName.TryFinishLoadingPresentation)
		{
			return true;
		}
		if (method == MethodName.LoadFailed)
		{
			return true;
		}
		if (method == MethodName.UpdateLoadingStepText)
		{
			return true;
		}
		if (method == MethodName.BuildLoadingText)
		{
			return true;
		}
		if (method == MethodName.TranslateLoadingStep)
		{
			return true;
		}
		if (method == MethodName.EnsureSodRollCapTexture)
		{
			return true;
		}
		if (method == MethodName.CanAnimateSodRollCap)
		{
			return true;
		}
		if (method == MethodName.ShowClearLoadingCacheDialog)
		{
			return true;
		}
		if (method == MethodName.BuildClearCacheDialogText)
		{
			return true;
		}
		if (method == MethodName.ClearLoadingCacheConfirmed)
		{
			return true;
		}
		if (method == MethodName.ClearLoadedMapResourceCaches)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LoadingFirstFramePresentedUsec)
		{
			LoadingFirstFramePresentedUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.BootstrapFirstFramePresentedUsec)
		{
			BootstrapFirstFramePresentedUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.GameplayResourceLoadRequestedUsec)
		{
			GameplayResourceLoadRequestedUsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._loadLabel)
		{
			_loadLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._offlineButton)
		{
			_offlineButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._progressBar)
		{
			_progressBar = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._startButton)
		{
			_startButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._clearCacheButton)
		{
			_clearCacheButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._clearCacheDialog)
		{
			_clearCacheDialog = VariantUtils.ConvertTo<DialogBoxChoose>(in value);
			return true;
		}
		if (name == PropertyName._sodRollCap)
		{
			_sodRollCap = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._loadBarSprout)
		{
			_loadBarSprout = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._loadBarSprout2)
		{
			_loadBarSprout2 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._loadBarSprout3)
		{
			_loadBarSprout3 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._loadBarSprout4)
		{
			_loadBarSprout4 = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._loadBarZombieHead)
		{
			_loadBarZombieHead = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._percentage)
		{
			_percentage = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._isStart)
		{
			_isStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._currentValue)
		{
			_currentValue = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._loadCompleted)
		{
			_loadCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._loadingPresentationCompleted)
		{
			_loadingPresentationCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._startupModFlowStarted)
		{
			_startupModFlowStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._startupModsReady)
		{
			_startupModsReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._completedLoadingAnimationCount)
		{
			_completedLoadingAnimationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._startupImportMessage)
		{
			_startupImportMessage = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		ulong from;
		if (name == PropertyName.LoadingFirstFramePresentedUsec)
		{
			from = LoadingFirstFramePresentedUsec;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BootstrapFirstFramePresentedUsec)
		{
			from = BootstrapFirstFramePresentedUsec;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.GameplayResourceLoadRequestedUsec)
		{
			from = GameplayResourceLoadRequestedUsec;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._loadLabel)
		{
			value = VariantUtils.CreateFrom(in _loadLabel);
			return true;
		}
		if (name == PropertyName._offlineButton)
		{
			value = VariantUtils.CreateFrom(in _offlineButton);
			return true;
		}
		if (name == PropertyName._progressBar)
		{
			value = VariantUtils.CreateFrom(in _progressBar);
			return true;
		}
		if (name == PropertyName._startButton)
		{
			value = VariantUtils.CreateFrom(in _startButton);
			return true;
		}
		if (name == PropertyName._clearCacheButton)
		{
			value = VariantUtils.CreateFrom(in _clearCacheButton);
			return true;
		}
		if (name == PropertyName._clearCacheDialog)
		{
			value = VariantUtils.CreateFrom(in _clearCacheDialog);
			return true;
		}
		if (name == PropertyName._sodRollCap)
		{
			value = VariantUtils.CreateFrom(in _sodRollCap);
			return true;
		}
		if (name == PropertyName._loadBarSprout)
		{
			value = VariantUtils.CreateFrom(in _loadBarSprout);
			return true;
		}
		if (name == PropertyName._loadBarSprout2)
		{
			value = VariantUtils.CreateFrom(in _loadBarSprout2);
			return true;
		}
		if (name == PropertyName._loadBarSprout3)
		{
			value = VariantUtils.CreateFrom(in _loadBarSprout3);
			return true;
		}
		if (name == PropertyName._loadBarSprout4)
		{
			value = VariantUtils.CreateFrom(in _loadBarSprout4);
			return true;
		}
		if (name == PropertyName._loadBarZombieHead)
		{
			value = VariantUtils.CreateFrom(in _loadBarZombieHead);
			return true;
		}
		if (name == PropertyName._percentage)
		{
			value = VariantUtils.CreateFrom(in _percentage);
			return true;
		}
		if (name == PropertyName._isStart)
		{
			value = VariantUtils.CreateFrom(in _isStart);
			return true;
		}
		if (name == PropertyName._currentValue)
		{
			value = VariantUtils.CreateFrom(in _currentValue);
			return true;
		}
		if (name == PropertyName._loadCompleted)
		{
			value = VariantUtils.CreateFrom(in _loadCompleted);
			return true;
		}
		if (name == PropertyName._loadingPresentationCompleted)
		{
			value = VariantUtils.CreateFrom(in _loadingPresentationCompleted);
			return true;
		}
		if (name == PropertyName._startupModFlowStarted)
		{
			value = VariantUtils.CreateFrom(in _startupModFlowStarted);
			return true;
		}
		if (name == PropertyName._startupModsReady)
		{
			value = VariantUtils.CreateFrom(in _startupModsReady);
			return true;
		}
		if (name == PropertyName._completedLoadingAnimationCount)
		{
			value = VariantUtils.CreateFrom(in _completedLoadingAnimationCount);
			return true;
		}
		if (name == PropertyName._startupImportMessage)
		{
			value = VariantUtils.CreateFrom(in _startupImportMessage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._loadLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offlineButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._progressBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._startButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearCacheButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearCacheDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sodRollCap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadBarSprout, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadBarSprout2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadBarSprout3, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadBarSprout4, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadBarZombieHead, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._percentage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._currentValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._loadCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._loadingPresentationCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._startupModFlowStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._startupModsReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._completedLoadingAnimationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LoadingFirstFramePresentedUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.BootstrapFirstFramePresentedUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GameplayResourceLoadRequestedUsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._startupImportMessage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LoadingFirstFramePresentedUsec, Variant.From<ulong>(LoadingFirstFramePresentedUsec));
		info.AddProperty(PropertyName.BootstrapFirstFramePresentedUsec, Variant.From<ulong>(BootstrapFirstFramePresentedUsec));
		info.AddProperty(PropertyName.GameplayResourceLoadRequestedUsec, Variant.From<ulong>(GameplayResourceLoadRequestedUsec));
		info.AddProperty(PropertyName._loadLabel, Variant.From(in _loadLabel));
		info.AddProperty(PropertyName._offlineButton, Variant.From(in _offlineButton));
		info.AddProperty(PropertyName._progressBar, Variant.From(in _progressBar));
		info.AddProperty(PropertyName._startButton, Variant.From(in _startButton));
		info.AddProperty(PropertyName._clearCacheButton, Variant.From(in _clearCacheButton));
		info.AddProperty(PropertyName._clearCacheDialog, Variant.From(in _clearCacheDialog));
		info.AddProperty(PropertyName._sodRollCap, Variant.From(in _sodRollCap));
		info.AddProperty(PropertyName._loadBarSprout, Variant.From(in _loadBarSprout));
		info.AddProperty(PropertyName._loadBarSprout2, Variant.From(in _loadBarSprout2));
		info.AddProperty(PropertyName._loadBarSprout3, Variant.From(in _loadBarSprout3));
		info.AddProperty(PropertyName._loadBarSprout4, Variant.From(in _loadBarSprout4));
		info.AddProperty(PropertyName._loadBarZombieHead, Variant.From(in _loadBarZombieHead));
		info.AddProperty(PropertyName._percentage, Variant.From(in _percentage));
		info.AddProperty(PropertyName._isStart, Variant.From(in _isStart));
		info.AddProperty(PropertyName._currentValue, Variant.From(in _currentValue));
		info.AddProperty(PropertyName._loadCompleted, Variant.From(in _loadCompleted));
		info.AddProperty(PropertyName._loadingPresentationCompleted, Variant.From(in _loadingPresentationCompleted));
		info.AddProperty(PropertyName._startupModFlowStarted, Variant.From(in _startupModFlowStarted));
		info.AddProperty(PropertyName._startupModsReady, Variant.From(in _startupModsReady));
		info.AddProperty(PropertyName._completedLoadingAnimationCount, Variant.From(in _completedLoadingAnimationCount));
		info.AddProperty(PropertyName._startupImportMessage, Variant.From(in _startupImportMessage));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LoadingFirstFramePresentedUsec, out var value))
		{
			LoadingFirstFramePresentedUsec = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.BootstrapFirstFramePresentedUsec, out var value2))
		{
			BootstrapFirstFramePresentedUsec = value2.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.GameplayResourceLoadRequestedUsec, out var value3))
		{
			GameplayResourceLoadRequestedUsec = value3.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._loadLabel, out var value4))
		{
			_loadLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._offlineButton, out var value5))
		{
			_offlineButton = value5.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._progressBar, out var value6))
		{
			_progressBar = value6.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._startButton, out var value7))
		{
			_startButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearCacheButton, out var value8))
		{
			_clearCacheButton = value8.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._clearCacheDialog, out var value9))
		{
			_clearCacheDialog = value9.As<DialogBoxChoose>();
		}
		if (info.TryGetProperty(PropertyName._sodRollCap, out var value10))
		{
			_sodRollCap = value10.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._loadBarSprout, out var value11))
		{
			_loadBarSprout = value11.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._loadBarSprout2, out var value12))
		{
			_loadBarSprout2 = value12.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._loadBarSprout3, out var value13))
		{
			_loadBarSprout3 = value13.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._loadBarSprout4, out var value14))
		{
			_loadBarSprout4 = value14.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._loadBarZombieHead, out var value15))
		{
			_loadBarZombieHead = value15.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._percentage, out var value16))
		{
			_percentage = value16.As<float>();
		}
		if (info.TryGetProperty(PropertyName._isStart, out var value17))
		{
			_isStart = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._currentValue, out var value18))
		{
			_currentValue = value18.As<float>();
		}
		if (info.TryGetProperty(PropertyName._loadCompleted, out var value19))
		{
			_loadCompleted = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._loadingPresentationCompleted, out var value20))
		{
			_loadingPresentationCompleted = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._startupModFlowStarted, out var value21))
		{
			_startupModFlowStarted = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._startupModsReady, out var value22))
		{
			_startupModsReady = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._completedLoadingAnimationCount, out var value23))
		{
			_completedLoadingAnimationCount = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._startupImportMessage, out var value24))
		{
			_startupImportMessage = value24.As<string>();
		}
	}
}
