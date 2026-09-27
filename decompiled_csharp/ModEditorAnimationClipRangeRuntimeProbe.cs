using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.AdobeAnimateEditor.Inspector;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://Tests/ModEditorAnimationClipRangeRuntimeProbe.cs")]
public class ModEditorAnimationClipRangeRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindClipIndex = "FindClipIndex";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName FindRenderManager = "FindRenderManager";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string FixturePath = "res://Asset/Anime/Splat/Spike/SpikeSplat.tres";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWGenericVisualResourceEditor _editor;

	public override async void _Ready()
	{
		_ = 14;
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
			Require(flag, "F3 did not initialize the animation editor within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool flag2 = await EnterEditorSurface(900);
			Require(flag2, "F3 editor did not finish loading its main editing surface.");
			if (!flag2)
			{
				Finish();
				return;
			}
			AdobeAnimateData animation = ResourceLoader.Load<AdobeAnimateData>("res://Asset/Anime/Splat/Spike/SpikeSplat.tres", null, ResourceLoader.CacheMode.Reuse);
			Require(GodotObject.IsInstanceValid(animation) && animation.HasPackedRuntimeData(), "Real SpikeSplat animation fixture is unavailable.");
			if (!GodotObject.IsInstanceValid(animation) || !animation.HasPackedRuntimeData())
			{
				Finish();
				return;
			}
			XWEditorInterface.Instance.EditResource(animation, XWResourceEditContext.ForRoot(animation, "res://Asset/Anime/Splat/Spike/SpikeSplat.tres", "animation_editor"));
			XWEditorInterface.Instance.FocusPanel("animation_editor");
			AdobeAnimateInspectorPreview preview = await WaitForPreview(900);
			Require(GodotObject.IsInstanceValid(preview), "Animation main panel did not mount its runtime preview.");
			if (!GodotObject.IsInstanceValid(preview))
			{
				Finish();
				return;
			}
			AdobeAnimateSprite sprite = preview.FindChild("InspectorAnimationSprite", recursive: true, owned: false) as AdobeAnimateSprite;
			OptionButton optionButton = preview.FindChild("ClipOption", recursive: true, owned: false) as OptionButton;
			HSlider frameSlider = preview.FindChild("FrameSlider", recursive: true, owned: false) as HSlider;
			Button nextButton = preview.FindChild("NextButton", recursive: true, owned: false) as Button;
			Button playButton = preview.FindChild("PlayButton", recursive: true, owned: false) as Button;
			AdobeAnimateCpuPreviewCanvas cpuCanvas = preview.FindChild("CpuPreviewCanvas", recursive: true, owned: false) as AdobeAnimateCpuPreviewCanvas;
			XWAnimationVisualResourceEditor animationEditor = _editor as XWAnimationVisualResourceEditor;
			Label playbackStateLabel = _editor.FindChild("PlaybackStateLabel", recursive: true, owned: false) as Label;
			VBoxContainer vBoxContainer = _editor.FindChild("EmbeddedInspectorHost", recursive: true, owned: false) as VBoxContainer;
			bool window = FindAncestorWindow(_editor) != null;
			bool previewVisible = preview.IsVisibleInTree() && GodotObject.IsInstanceValid(sprite) && GodotObject.IsInstanceValid(cpuCanvas) && cpuCanvas.IsVisibleInTree();
			Require(previewVisible, "Runtime animation preview is not visibly mounted in the F3 main panel.");
			bool flag3 = GodotObject.IsInstanceValid(optionButton) && GodotObject.IsInstanceValid(frameSlider) && GodotObject.IsInstanceValid(nextButton) && GodotObject.IsInstanceValid(playButton);
			Require(flag3, "Animation preview controls are incomplete.");
			if (!previewVisible || !flag3)
			{
				Finish();
				return;
			}
			bool inspectorHidden = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0;
			bool stateSurface = GodotObject.IsInstanceValid(animationEditor) && GodotObject.IsInstanceValid(playbackStateLabel) && playbackStateLabel.Text.Contains("播放中", StringComparison.Ordinal) && playbackStateLabel.Text.Contains("全部动画", StringComparison.Ordinal);
			Require(inspectorHidden, "Animation main panel seized the raw embedded Inspector instead of keeping the visual runtime surface direct-edit only.");
			Require(stateSurface, "Animation playback state was not bridged into the main game-like surface: '" + playbackStateLabel?.Text + "'.");
			bool allRangeExclusive = sprite.clipRange == new Vector2I(0, animation.frameMax) && Math.Abs(frameSlider.MaxValue - (double)(animation.frameMax - 1)) < 0.01;
			Require(allRangeExclusive, $"Whole animation range is not end-exclusive: {sprite.clipRange}, slider={frameSlider.MaxValue}.");
			frameSlider.SetValueNoSignal(animation.frameMax - 1);
			frameSlider.EmitSignal(Godot.Range.SignalName.ValueChanged, (double)(animation.frameMax - 1));
			bool allLastFrameReachable = sprite.pause && sprite.frameIndex == animation.frameMax - 1;
			Require(allLastFrameReachable, $"Whole animation slider cannot reach its final renderable frame: {sprite.frameIndex}/{animation.frameMax - 1}.");
			Vector2I animationClip = animation.GetClip("animation");
			int num = FindClipIndex(optionButton, "animation");
			bool clipLabel = num > 0 && optionButton.GetItemText(num).Contains($"({animationClip.X}-{animationClip.Y - 1})", StringComparison.Ordinal);
			Require(clipLabel, "Clip selector still presents the exclusive endpoint as a visible frame.");
			if (num > 0)
			{
				optionButton.Select(num);
				optionButton.EmitSignal(OptionButton.SignalName.ItemSelected, (long)num);
			}
			await WaitFrames(2);
			bool clipRange = sprite.clipRange == animationClip;
			bool sliderBound = Math.Abs(frameSlider.MinValue - (double)animationClip.X) < 0.01 && Math.Abs(frameSlider.MaxValue - (double)(animationClip.Y - 1)) < 0.01;
			bool clipState = playbackStateLabel.Text.Contains("animation", StringComparison.Ordinal) && playbackStateLabel.Text.Contains($"{animationClip.X}-{animationClip.Y - 1}", StringComparison.Ordinal);
			Require(clipRange, $"Selected clip range changed contract: {sprite.clipRange} != {animationClip}.");
			Require(sliderBound, $"Selected clip slider exposes blank endpoint {frameSlider.MinValue}-{frameSlider.MaxValue}.");
			Require(clipState, "Selected clip state did not reach the parent animation surface: '" + playbackStateLabel.Text + "'.");
			frameSlider.SetValueNoSignal(animationClip.Y - 2);
			frameSlider.EmitSignal(Godot.Range.SignalName.ValueChanged, (double)(animationClip.Y - 2));
			nextButton.EmitSignal(BaseButton.SignalName.Pressed);
			nextButton.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool nextClamped = sprite.pause && sprite.frameIndex == animationClip.Y - 1;
			Require(nextClamped, $"Next-frame interaction entered the exclusive blank endpoint: frame={sprite.frameIndex}, end={animationClip.Y}.");
			bool lastFrameRendered = cpuCanvas.TryGetLocalRenderBounds(out var bounds) && bounds.Size.X > 0.01f && bounds.Size.Y > 0.01f;
			Require(lastFrameRendered, "The final playable clip frame has no visible CPU preview bounds.");
			playButton.EmitSignal(BaseButton.SignalName.Pressed);
			sprite.BatchProcessUpdate(1.0 / animation.frameRate + 0.001);
			bool playbackLoopSafe = !sprite.pause && sprite.frameIndex == animationClip.X;
			Require(playbackLoopSafe, $"Clip playback entered the exclusive blank endpoint instead of looping: frame={sprite.frameIndex}, expected={animationClip.X}.");
			await WaitFrames(2);
			bool playProcessActive = preview.IsProcessing();
			Require(playProcessActive, "Visible animation playback does not keep the inspector controls synchronized.");
			_editor.Hide();
			await WaitFrames(2);
			bool hiddenProcessIdle = !preview.IsProcessing() && sprite.pause;
			Require(hiddenProcessIdle, "Hidden animation preview still owns an idle per-frame process loop.");
			_editor.Show();
			await WaitFrames(2);
			bool resumeProcessActive = preview.IsProcessing() && !sprite.pause;
			bool stateRecovered = resumeProcessActive && playbackStateLabel.Text.Contains("播放中", StringComparison.Ordinal) && playbackStateLabel.Text.Contains("animation", StringComparison.Ordinal);
			Require(resumeProcessActive, "Animation playback did not resume its gated process loop after the editor became visible.");
			Require(stateRecovered, "Animation playback state did not recover after returning to the editor: '" + playbackStateLabel.Text + "'.");
			playButton.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(2);
			bool pausedProcessIdle = sprite.pause && !preview.IsProcessing();
			bool pausedState = playbackStateLabel.Text.Contains("已暂停", StringComparison.Ordinal) && playbackStateLabel.Text.Contains("animation", StringComparison.Ordinal);
			Require(pausedProcessIdle, "Paused animation preview still executes an unconditional process loop.");
			Require(pausedState, "Paused playback state did not reach the parent animation surface: '" + playbackStateLabel.Text + "'.");
			int controlRefreshBeforeIdle = preview.ControlRefreshCount;
			int stateRefreshBeforeIdle = animationEditor.AnimationStateUpdateCount;
			await WaitFrames(12);
			bool pausedUiStable = preview.ControlRefreshCount == controlRefreshBeforeIdle && animationEditor.AnimationStateUpdateCount == stateRefreshBeforeIdle && !preview.IsProcessing();
			Require(pausedUiStable, "Paused animation preview still rewrites unchanged controls or parent state while idle.");
			AdobeAnimateRenderManager.WarmupRenderMount(sprite);
			await WaitFrames(2);
			AdobeAnimateRenderManager adobeAnimateRenderManager = FindRenderManager(sprite);
			bool renderManagerFound = GodotObject.IsInstanceValid(adobeAnimateRenderManager);
			bool renderManagerIdle = renderManagerFound && !adobeAnimateRenderManager.IsProcessing();
			Require(renderManagerFound, "Shared Adobe Animate render manager was not mounted by the real preview.");
			Require(renderManagerIdle, "Shared Adobe Animate render manager still executes an idle per-frame process loop.");
			AdobeAnimateSprite runtimeSprite = new AdobeAnimateSprite
			{
				Name = "RuntimeManagerEligibilityProbe"
			};
			AddChild(runtimeSprite, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(1);
			AdobeAnimateRuntimeManager instance = AdobeAnimateRuntimeManager.Instance;
			int runtimeBaseline = (GodotObject.IsInstanceValid(instance) ? instance.RegisteredSpriteCount : 0);
			bool runtimeRegistered = AdobeAnimateRuntimeManager.Register(runtimeSprite);
			await WaitFrames(3);
			AdobeAnimateRuntimeManager runtimeManager = AdobeAnimateRuntimeManager.Instance;
			bool runtimeManagerWoke = runtimeRegistered && GodotObject.IsInstanceValid(runtimeManager) && runtimeManager.IsProcessing() && runtimeManager.RegisteredSpriteCount == runtimeBaseline + 1;
			AdobeAnimateRuntimeManager.Unregister(runtimeSprite);
			await WaitFrames(3);
			bool flag4 = GodotObject.IsInstanceValid(runtimeManager) && runtimeManager.RegisteredSpriteCount == runtimeBaseline && (runtimeBaseline > 0 || !runtimeManager.IsProcessing());
			Require(runtimeManagerWoke, "Registering the first runtime animation did not wake its shared manager.");
			Require(flag4, "Removing the final runtime animation left its shared manager processing every frame.");
			runtimeSprite.QueueFree();
			GD.Print($"[MOD_EDITOR_ANIMATION_CLIP_RANGE_PROBE] window={window} previewVisible={previewVisible} inspectorHidden={inspectorHidden} stateSurface={stateSurface} clipState={clipState} stateRecovered={stateRecovered} pausedState={pausedState} pausedUiStable={pausedUiStable} allRangeExclusive={allRangeExclusive} allLastFrameReachable={allLastFrameReachable} clipLabel={clipLabel} clipRange={clipRange} sliderBound={sliderBound} nextClamped={nextClamped} lastFrameRendered={lastFrameRendered} playbackLoopSafe={playbackLoopSafe} playProcessActive={playProcessActive} hiddenProcessIdle={hiddenProcessIdle} resumeProcessActive={resumeProcessActive} pausedProcessIdle={pausedProcessIdle} renderManagerFound={renderManagerFound} renderManagerIdle={renderManagerIdle} runtimeManagerWoke={runtimeManagerWoke} runtimeManagerIdle={flag4} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			if (XWEditorInterface.Instance?.GetResourceEditor("animation_editor") is XWGenericVisualResourceEditor xWGenericVisualResourceEditor && GodotObject.IsInstanceValid(xWGenericVisualResourceEditor))
			{
				_editor = xWGenericVisualResourceEditor;
				return true;
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				XWEditorInterface.Instance.FocusPanel("animation_editor");
				await WaitFrames(3);
				return _editor.IsVisibleInTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<AdobeAnimateInspectorPreview> WaitForPreview(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			AdobeAnimateInspectorPreview adobeAnimateInspectorPreview = _editor?.FindChild("RuntimeInspectorPreview", recursive: true, owned: false) as AdobeAnimateInspectorPreview;
			AdobeAnimateSprite instance = adobeAnimateInspectorPreview?.FindChild("InspectorAnimationSprite", recursive: true, owned: false) as AdobeAnimateSprite;
			if (GodotObject.IsInstanceValid(adobeAnimateInspectorPreview) && GodotObject.IsInstanceValid(instance))
			{
				return adobeAnimateInspectorPreview;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private static int FindClipIndex(OptionButton clipOption, string clipName)
	{
		if (!GodotObject.IsInstanceValid(clipOption))
		{
			return -1;
		}
		string value = clipName + " (";
		for (int i = 0; i < clipOption.ItemCount; i++)
		{
			if (clipOption.GetItemText(i).StartsWith(value, StringComparison.Ordinal))
			{
				return i;
			}
		}
		return -1;
	}

	private static Window FindAncestorWindow(Node node)
	{
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 is Window result)
			{
				return result;
			}
			node2 = node2.GetParent();
		}
		return null;
	}

	private static AdobeAnimateRenderManager FindRenderManager(Node source)
	{
		AdobeAnimateRenderManager.ResolveRenderMount(source, out var mountParent, out var _);
		return mountParent?.FindChild("AdobeAnimateRenderManager", recursive: true, owned: false) as AdobeAnimateRenderManager;
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_ANIMATION_CLIP_RANGE_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_ANIMATION_CLIP_RANGE_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindClipIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "clipOption", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindRenderManager, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.FindClipIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindClipIndex(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRenderManager && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateRenderManager>(FindRenderManager(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindClipIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindClipIndex(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindRenderManager && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateRenderManager>(FindRenderManager(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.FindClipIndex)
		{
			return true;
		}
		if (method == MethodName.FindAncestorWindow)
		{
			return true;
		}
		if (method == MethodName.FindRenderManager)
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
			_editor = VariantUtils.ConvertTo<XWGenericVisualResourceEditor>(in value);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWGenericVisualResourceEditor>();
		}
	}
}
