using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors.GUI;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

[ScriptPath("res://Tests/ModEditorCardIncrementalPreviewRuntimeProbe.cs")]
public class ModEditorCardIncrementalPreviewRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName IsStableLightPreview = "IsStableLightPreview";

		public static readonly StringName PressF3 = "PressF3";

		public static readonly StringName SetTextSession = "SetTextSession";

		public static readonly StringName SetSpinSession = "SetSpinSession";

		public static readonly StringName FindDifferentClip = "FindDifferentClip";

		public static readonly StringName SameResourcePath = "SameResourcePath";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

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

	private const string PacketPath = "res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Packet/PlantPeaShooter.tres";

	private const string SunFlowerPath = "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Config/TowerDefensePlantSunFlower.tres";

	private const string DraftPath = "user://mod_editor_card_incremental_preview_probe.tres";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWCardVisualResourceEditor _editor;

	private XWUndoRedoManager _history;

	public override async void _Ready()
	{
		bool window = false;
		bool inspectorHidden = false;
		bool continuousText = false;
		bool continuousNumber = false;
		bool lightIdentity = false;
		bool frameStable = false;
		bool clipInPlace = false;
		bool transformInPlace = false;
		bool flipInPlace = false;
		bool negativeSunDirect = false;
		bool negativeSunPreview = false;
		bool armorSingleRebuild = false;
		bool characterSingleRebuild = false;
		bool undoRedo = false;
		bool saveReload = false;
		bool inspectorUntouched = false;
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
				Finish(window, inspectorHidden, continuousText, continuousNumber, lightIdentity, frameStable, clipInPlace, transformInPlace, flipInPlace, negativeSunDirect, negativeSunPreview, armorSingleRebuild, characterSingleRebuild, undoRedo, saveReload, inspectorUntouched);
				return;
			}
			await WaitFrames(2);
			PressF3();
			bool flag = await WaitForEditor(900);
			Require(flag, "F3 did not initialize the Card editor within 900 frames.");
			bool flag2 = flag;
			if (flag2)
			{
				flag2 = await EnterEditorSurface(900);
			}
			bool flag3 = flag2;
			Require(flag3, "F3 editor did not finish loading its main editing surface.");
			if (!flag3)
			{
				Finish(window, inspectorHidden, continuousText, continuousNumber, lightIdentity, frameStable, clipInPlace, transformInPlace, flipInPlace, negativeSunDirect, negativeSunPreview, armorSingleRebuild, characterSingleRebuild, undoRedo, saveReload, inspectorUntouched);
				return;
			}
			window = FindAncestorWindow(_editor) != null;
			TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Chapter0/PeaShooter/Packet/PlantPeaShooter.tres", "", ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
			Require(GodotObject.IsInstanceValid(towerDefensePacketConfig), "Peashooter packet fixture could not be duplicated.");
			if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				Finish(window, inspectorHidden, continuousText, continuousNumber, lightIdentity, frameStable, clipInPlace, transformInPlace, flipInPlace, negativeSunDirect, negativeSunPreview, armorSingleRebuild, characterSingleRebuild, undoRedo, saveReload, inspectorUntouched);
				return;
			}
			towerDefensePacketConfig.overrideCost = 75;
			towerDefensePacketConfig.overridePacketCooldown = 0.8;
			Require(ResourceSaver.Save(towerDefensePacketConfig, "user://mod_editor_card_incremental_preview_probe.tres", ResourceSaver.SaverFlags.None) == Error.Ok, "Card incremental preview draft could not be saved.");
			TowerDefensePacketConfig packet = ResourceLoader.Load<TowerDefensePacketConfig>("user://mod_editor_card_incremental_preview_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			Require(GodotObject.IsInstanceValid(packet), "Saved card incremental preview draft could not be loaded.");
			if (!GodotObject.IsInstanceValid(packet))
			{
				Finish(window, inspectorHidden, continuousText, continuousNumber, lightIdentity, frameStable, clipInPlace, transformInPlace, flipInPlace, negativeSunDirect, negativeSunPreview, armorSingleRebuild, characterSingleRebuild, undoRedo, saveReload, inspectorUntouched);
				return;
			}
			Node inspectorSentinel = new Node
			{
				Name = "CardIncrementalPreviewInspectorSentinel"
			};
			AddChild(inspectorSentinel, forceReadableName: false, InternalMode.Disabled);
			XWInspector inspector = XWEditorInterface.Instance.GetInspector() as XWInspector;
			XWEditorInterface.Instance.InspectObject(inspectorSentinel);
			await WaitFrames(2);
			XWEditorInterface.Instance.EditResource(packet, XWResourceEditContext.ForRoot(packet, "user://mod_editor_card_incremental_preview_probe.tres", "card_editor"));
			XWEditorInterface.Instance.FocusPanel("card_editor");
			await WaitFrames(14);
			VBoxContainer vBoxContainer = FindControl<VBoxContainer>("EmbeddedInspectorHost");
			inspectorHidden = GodotObject.IsInstanceValid(vBoxContainer) && vBoxContainer.GetChildCount() == 0 && _editor.FindChild("EmbeddedResourceInspector", recursive: true, owned: false) == null;
			Require(inspectorHidden, "Exact packet resource opened the raw embedded Inspector.");
			TowerDefenseInGamePacketShow packetShow = _editor.FindChild("CardPacketShowPreview", recursive: true, owned: false) as TowerDefenseInGamePacketShow;
			AdobeAnimateSprite sprite = packetShow?.sprite;
			LineEdit lineEdit = FindControl<LineEdit>("CardNameLabel");
			SpinBox spinBox = FindControl<SpinBox>("InlineCostSpinBox");
			LineEdit lineEdit2 = FindControl<LineEdit>("PacketAnimeClipLineEdit") ?? FindControl<LineEdit>("InlineAnimationLineEdit");
			SpinBox offsetX = FindControl<SpinBox>("PacketAnimeOffsetX");
			SpinBox offsetY = FindControl<SpinBox>("PacketAnimeOffsetY");
			SpinBox scaleX = FindControl<SpinBox>("PacketAnimeScaleX");
			SpinBox scaleY = FindControl<SpinBox>("PacketAnimeScaleY");
			CheckButton flip = FindControl<CheckButton>("PacketFlipCheck");
			CheckButton disableWhenSunNegative = FindControl<CheckButton>("DisableWhenSunNegativeCheck");
			SpinBox previewSun = FindControl<SpinBox>("PreviewSunSpinBox");
			Label battleStatus = FindControl<Label>("BattleStatusLabel");
			LineEdit armorValue = FindControl<LineEdit>("ArmorValueEdit");
			Button addArmor = FindControl<Button>("AddArmorButton");
			Button characterButton = FindControl<Button>("CharacterBindingWindowButton") ?? FindControl<Button>("InlineCharacterBindingButton");
			bool flag4 = GodotObject.IsInstanceValid(packetShow) && GodotObject.IsInstanceValid(sprite) && GodotObject.IsInstanceValid(lineEdit) && GodotObject.IsInstanceValid(spinBox) && GodotObject.IsInstanceValid(lineEdit2) && GodotObject.IsInstanceValid(offsetX) && GodotObject.IsInstanceValid(offsetY) && GodotObject.IsInstanceValid(scaleX) && GodotObject.IsInstanceValid(scaleY) && GodotObject.IsInstanceValid(flip) && GodotObject.IsInstanceValid(disableWhenSunNegative) && GodotObject.IsInstanceValid(previewSun) && GodotObject.IsInstanceValid(battleStatus) && GodotObject.IsInstanceValid(armorValue) && GodotObject.IsInstanceValid(addArmor) && GodotObject.IsInstanceValid(characterButton);
			Require(flag4, "Card incremental preview controls or live PacketShow/Sprite are missing.");
			if (!flag4)
			{
				Finish(window, inspectorHidden, continuousText, continuousNumber, lightIdentity, frameStable, clipInPlace, transformInPlace, flipInPlace, negativeSunDirect, negativeSunPreview, armorSingleRebuild, characterSingleRebuild, undoRedo, saveReload, inspectorUntouched);
				return;
			}
			int outerBuilds = _editor.CardPacketShowPreviewBuildCount;
			int spriteRebuilds = _editor.CardPacketShowSpriteRebuildCount;
			sprite.SetFrozenPreview(frozen: true);
			int num = (sprite.frameIndex = ((sprite.clipRange.Y > sprite.clipRange.X) ? Math.Min(sprite.clipRange.Y - 1, sprite.clipRange.X + 2) : sprite.frameIndex));
			sprite.elapsedTimer = 0.5;
			string text = packet.name + "_incremental_a";
			string editedName = packet.name + "_incremental_b";
			lineEdit.EmitSignal(Control.SignalName.FocusEntered);
			lineEdit.Text = text;
			lineEdit.EmitSignal(LineEdit.SignalName.TextChanged, text);
			bool flag5 = IsStableLightPreview(packetShow, sprite, outerBuilds, spriteRebuilds, num, 0.5);
			lineEdit.Text = editedName;
			lineEdit.EmitSignal(LineEdit.SignalName.TextChanged, editedName);
			bool flag6 = IsStableLightPreview(packetShow, sprite, outerBuilds, spriteRebuilds, num, 0.5);
			lineEdit.EmitSignal(Control.SignalName.FocusExited);
			bool flag7 = packet.name == editedName && IsStableLightPreview(packetShow, sprite, outerBuilds, spriteRebuilds, num, 0.5);
			continuousText = flag5 & flag6 & flag7;
			Require(continuousText, "Continuous card-name input rebuilt PacketShow/Sprite or changed the frozen frame.");
			spinBox.EmitSignal(Control.SignalName.FocusEntered);
			spinBox.SetValueNoSignal(81.0);
			spinBox.EmitSignal(Godot.Range.SignalName.ValueChanged, 81.0);
			bool flag8 = IsStableLightPreview(packetShow, sprite, outerBuilds, spriteRebuilds, num, 0.5);
			spinBox.SetValueNoSignal(87.0);
			spinBox.EmitSignal(Godot.Range.SignalName.ValueChanged, 87.0);
			bool flag9 = IsStableLightPreview(packetShow, sprite, outerBuilds, spriteRebuilds, num, 0.5);
			spinBox.EmitSignal(Control.SignalName.FocusExited);
			bool flag10 = packet.overrideCost == 87 && IsStableLightPreview(packetShow, sprite, outerBuilds, spriteRebuilds, num, 0.5);
			continuousNumber = flag8 & flag9 & flag10;
			Require(continuousNumber, "Continuous card-cost input rebuilt PacketShow/Sprite or changed the frozen frame.");
			lightIdentity = (continuousText & continuousNumber) && packetShow == _editor.FindChild("CardPacketShowPreview", recursive: true, owned: false) && sprite == packetShow.sprite && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds;
			frameStable = sprite.frameIndex == num && Math.Abs(sprite.elapsedTimer - 0.5) < 1E-06;
			Require(lightIdentity, "Lightweight card fields did not preserve PacketShow and Sprite identity.");
			Require(frameStable, "Lightweight card fields reset the frozen animation pose.");
			string targetClip = FindDifferentClip(sprite, packet.packetAnimeClip);
			Require(!string.IsNullOrWhiteSpace(targetClip), "Packet Sprite has no second valid animation Clip.");
			if (!string.IsNullOrWhiteSpace(targetClip))
			{
				SetTextSession(lineEdit2, targetClip);
				await WaitFrames(2);
				clipInPlace = packet.packetAnimeClip == targetClip && packetShow == _editor.FindChild("CardPacketShowPreview", recursive: true, owned: false) && sprite == packetShow.sprite && sprite.clip == targetClip && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds;
			}
			Require(clipInPlace, "Changing a valid Clip did not update the existing Sprite in place.");
			Vector2 targetOffset = packet.packetAnimeOffset + new Vector2(3f, -2f);
			Vector2 targetScale = new Vector2(Math.Max(0.1f, packet.packetAnimeScale.X + 0.1f), Math.Max(0.1f, packet.packetAnimeScale.Y + 0.05f));
			SetSpinSession(offsetX, targetOffset.X);
			SetSpinSession(offsetY, targetOffset.Y);
			SetSpinSession(scaleX, targetScale.X);
			SetSpinSession(scaleY, targetScale.Y);
			await WaitFrames(2);
			transformInPlace = packet.packetAnimeOffset.IsEqualApprox(targetOffset) && packet.packetAnimeScale.IsEqualApprox(targetScale) && sprite.Position.IsEqualApprox(targetOffset) && sprite.Scale.IsEqualApprox(targetScale) && sprite == packetShow.sprite && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds;
			Require(transformInPlace, "Packet offset/scale did not update the existing Sprite in place.");
			bool targetFlip = !packet.packetFlip;
			flip.SetPressedNoSignal(targetFlip);
			flip.EmitSignal(BaseButton.SignalName.Toggled, targetFlip);
			await WaitFrames(2);
			Vector2 other = (targetFlip ? new Vector2(0f - targetScale.X, targetScale.Y) : targetScale);
			flipInPlace = packet.packetFlip == targetFlip && sprite.Scale.IsEqualApprox(other) && sprite == packetShow.sprite && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds;
			Require(flipInPlace, "Packet flip rebuilt the Sprite or failed to update its visual scale.");
			disableWhenSunNegative.SetPressedNoSignal(pressed: true);
			disableWhenSunNegative.EmitSignal(BaseButton.SignalName.Toggled, true);
			await WaitFrames(2);
			negativeSunDirect = packet.disableWhenSunNegative && disableWhenSunNegative.ButtonPressed && sprite == packetShow.sprite && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds;
			Require(negativeSunDirect, "The direct negative-sun rule toggle did not update the packet without rebuilding its preview.");
			previewSun.SetValueNoSignal(-25.0);
			previewSun.EmitSignal(Godot.Range.SignalName.ValueChanged, -25.0);
			await WaitFrames(2);
			negativeSunPreview = previewSun.MinValue <= -99999.0 && previewSun.Value < 0.0 && packetShow.openShadow && !packetShow.alive && battleStatus.Text.Contains("阳光不足", StringComparison.Ordinal) && sprite == packetShow.sprite && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds;
			Require(negativeSunPreview, $"Negative preview sun did not put the real PacketShow into the unavailable visual state: min={previewSun.MinValue:0.###} value={previewSun.Value:0.###} rule={packet.disableWhenSunNegative} shadow={packetShow.openShadow} alive={packetShow.alive} status={battleStatus.Text}.");
			AdobeAnimateSprite beforeArmorSprite = packetShow.sprite;
			int armorSpriteBuilds = _editor.CardPacketShowSpriteRebuildCount;
			int armorCharacterBuilds = _editor.CardCharacterPreviewBuildCount;
			string armorToken = (armorValue.Text = "incremental_probe_armor");
			addArmor.EmitSignal(BaseButton.SignalName.Pressed);
			await WaitFrames(3);
			armorSingleRebuild = packet.initArmor != null && packet.initArmor.Contains(armorToken) && _editor.CardPacketShowSpriteRebuildCount == armorSpriteBuilds + 1 && _editor.CardCharacterPreviewBuildCount == armorCharacterBuilds + 1 && _editor.CardPacketShowPreviewBuildCount == outerBuilds && packetShow == _editor.FindChild("CardPacketShowPreview", recursive: true, owned: false) && beforeArmorSprite != packetShow.sprite;
			Require(armorSingleRebuild, "Adding initial armor did not perform exactly one inner Sprite/character-preview rebuild.");
			_history.ClearHistory();
			AdobeAnimateSprite beforeCharacterSprite = packetShow.sprite;
			int characterSpriteBuilds = _editor.CardPacketShowSpriteRebuildCount;
			int characterPreviewBuilds = _editor.CardCharacterPreviewBuildCount;
			TowerDefenseCharacterConfig originalCharacter = packet.characterConfig;
			characterButton.EmitSignal(BaseButton.SignalName.Pressed);
			XWGameplayResourcePickerWindow xWGameplayResourcePickerWindow = await WaitForCharacterPicker(900);
			bool picked = GodotObject.IsInstanceValid(xWGameplayResourcePickerWindow) && xWGameplayResourcePickerWindow.TryConfirmIndexedResource("res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Config/TowerDefensePlantSunFlower.tres");
			await WaitFrames(3);
			characterSingleRebuild = picked && SameResourcePath(packet.characterConfig, "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Config/TowerDefensePlantSunFlower.tres") && _editor.CardPacketShowSpriteRebuildCount == characterSpriteBuilds + 1 && _editor.CardCharacterPreviewBuildCount == characterPreviewBuilds + 1 && _editor.CardPacketShowPreviewBuildCount == outerBuilds && packetShow == _editor.FindChild("CardPacketShowPreview", recursive: true, owned: false) && beforeCharacterSprite != packetShow.sprite && _history.HasUndo();
			Require(characterSingleRebuild, "Character picker did not change the binding with exactly one inner Sprite rebuild.");
			int beforeUndoSpriteBuilds = _editor.CardPacketShowSpriteRebuildCount;
			bool undone = _history.Undo();
			await WaitFrames(3);
			bool undoState = undone && packet.characterConfig == originalCharacter && _editor.CardPacketShowSpriteRebuildCount == beforeUndoSpriteBuilds + 1 && _editor.CardPacketShowPreviewBuildCount == outerBuilds;
			int beforeRedoSpriteBuilds = _editor.CardPacketShowSpriteRebuildCount;
			bool redone = _history.Redo();
			await WaitFrames(3);
			bool flag11 = redone && SameResourcePath(packet.characterConfig, "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Config/TowerDefensePlantSunFlower.tres") && _editor.CardPacketShowSpriteRebuildCount == beforeRedoSpriteBuilds + 1 && _editor.CardPacketShowPreviewBuildCount == outerBuilds;
			undoRedo = undoState & flag11;
			Require(undoRedo, "Character binding Undo/Redo did not rebuild the inner Sprite exactly once.");
			TowerDefensePacketConfig towerDefensePacketConfig2 = ResourceLoader.Load<TowerDefensePacketConfig>("user://mod_editor_card_incremental_preview_probe.tres", "", ResourceLoader.CacheMode.Ignore);
			saveReload = GodotObject.IsInstanceValid(towerDefensePacketConfig2) && towerDefensePacketConfig2.name == editedName && towerDefensePacketConfig2.overrideCost == 87 && towerDefensePacketConfig2.packetAnimeClip == targetClip && towerDefensePacketConfig2.packetAnimeOffset.IsEqualApprox(targetOffset) && towerDefensePacketConfig2.packetAnimeScale.IsEqualApprox(targetScale) && towerDefensePacketConfig2.packetFlip == targetFlip && towerDefensePacketConfig2.disableWhenSunNegative && towerDefensePacketConfig2.initArmor != null && towerDefensePacketConfig2.initArmor.Contains(armorToken) && SameResourcePath(towerDefensePacketConfig2.characterConfig, "res://Asset/Anime/Character/Plant/Chapter0/SunFlower/Config/TowerDefensePlantSunFlower.tres");
			Require(saveReload, "Incremental card edits did not survive a CacheMode.Ignore disk reload.");
			inspectorUntouched = inspector == null || inspector.CurrentObject == inspectorSentinel;
			Require(inspectorUntouched, "Card incremental editing redirected the raw Inspector.");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish(window, inspectorHidden, continuousText, continuousNumber, lightIdentity, frameStable, clipInPlace, transformInPlace, flipInPlace, negativeSunDirect, negativeSunPreview, armorSingleRebuild, characterSingleRebuild, undoRedo, saveReload, inspectorUntouched);
	}

	private bool IsStableLightPreview(TowerDefenseInGamePacketShow packetShow, AdobeAnimateSprite sprite, int outerBuilds, int spriteRebuilds, int frame, double elapsed)
	{
		if (GodotObject.IsInstanceValid(packetShow) && GodotObject.IsInstanceValid(sprite) && packetShow == _editor.FindChild("CardPacketShowPreview", recursive: true, owned: false) && sprite == packetShow.sprite && _editor.CardPacketShowPreviewBuildCount == outerBuilds && _editor.CardPacketShowSpriteRebuildCount == spriteRebuilds && sprite.frameIndex == frame)
		{
			return Math.Abs(sprite.elapsedTimer - elapsed) < 1E-06;
		}
		return false;
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWEditorInterface.Instance?.GetResourceEditor("card_editor") is XWCardVisualResourceEditor xWCardVisualResourceEditor && GodotObject.IsInstanceValid(xWCardVisualResourceEditor))
			{
				_editor = xWCardVisualResourceEditor;
				_history = XWEditorInterface.Instance.GetUndoRedoManager();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				XWEditorInterface.Instance.FocusPanel("card_editor");
				await WaitFrames(3);
				return _editor.IsVisibleInTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<XWGameplayResourcePickerWindow> WaitForCharacterPicker(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			XWGameplayResourcePickerWindow xWGameplayResourcePickerWindow = _editor.FindChildren("*", "", recursive: true, owned: false).OfType<XWGameplayResourcePickerWindow>().LastOrDefault((XWGameplayResourcePickerWindow candidate) => GodotObject.IsInstanceValid(candidate) && candidate.Visible);
			if (GodotObject.IsInstanceValid(xWGameplayResourcePickerWindow) && !xWGameplayResourcePickerWindow.IsResourceLibraryIndexing)
			{
				return xWGameplayResourcePickerWindow;
			}
			await WaitFrames(1);
		}
		return null;
	}

	private T FindControl<T>(string name) where T : Node
	{
		return _editor?.FindChild(name, recursive: true, owned: false) as T;
	}

	private static void PressF3()
	{
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
	}

	private static void SetTextSession(LineEdit control, string value)
	{
		control.EmitSignal(Control.SignalName.FocusEntered);
		control.Text = value;
		control.EmitSignal(LineEdit.SignalName.TextChanged, value);
		control.EmitSignal(LineEdit.SignalName.TextSubmitted, value);
		control.EmitSignal(Control.SignalName.FocusExited);
	}

	private static void SetSpinSession(SpinBox control, double value)
	{
		control.EmitSignal(Control.SignalName.FocusEntered);
		control.SetValueNoSignal(value);
		control.EmitSignal(Godot.Range.SignalName.ValueChanged, value);
		control.EmitSignal(Control.SignalName.FocusExited);
	}

	private static string FindDifferentClip(AdobeAnimateSprite sprite, string current)
	{
		if (!GodotObject.IsInstanceValid(sprite?.flashAnimeData) || sprite.flashAnimeData.clips == null)
		{
			return "";
		}
		foreach (Variant key in sprite.flashAnimeData.clips.Keys)
		{
			string text = key.AsString();
			if (!string.IsNullOrWhiteSpace(text) && !string.Equals(text, current, StringComparison.Ordinal))
			{
				return text;
			}
		}
		return "";
	}

	private static bool SameResourcePath(Resource resource, string expectedPath)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		string a = ProjectSettings.LocalizePath(resource.ResourcePath ?? "").Replace('\\', '/').Trim();
		string b = ProjectSettings.LocalizePath(expectedPath ?? "").Replace('\\', '/').Trim();
		return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
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

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_CARD_INCREMENTAL_PREVIEW_PROBE_FAILURE] " + message);
		}
	}

	private void Finish(bool window, bool inspectorHidden, bool continuousText, bool continuousNumber, bool lightIdentity, bool frameStable, bool clipInPlace, bool transformInPlace, bool flipInPlace, bool negativeSunDirect, bool negativeSunPreview, bool armorSingleRebuild, bool characterSingleRebuild, bool undoRedo, bool saveReload, bool inspectorUntouched)
	{
		GD.Print($"[MOD_EDITOR_CARD_INCREMENTAL_PREVIEW_PROBE] window={window} inspectorHidden={inspectorHidden} continuousText={continuousText} continuousNumber={continuousNumber} lightIdentity={lightIdentity} frameStable={frameStable} clipInPlace={clipInPlace} transformInPlace={transformInPlace} flipInPlace={flipInPlace} negativeSunDirect={negativeSunDirect} negativeSunPreview={negativeSunPreview} armorSingleRebuild={armorSingleRebuild} characterSingleRebuild={characterSingleRebuild} undoRedo={undoRedo} saveReload={saveReload} inspectorUntouched={inspectorUntouched} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_CARD_INCREMENTAL_PREVIEW_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsStableLightPreview, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "outerBuilds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "spriteRebuilds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "elapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PressF3, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SetTextSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("LineEdit"), exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpinSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindDifferentClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SameResourcePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "expectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "window", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "inspectorHidden", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "continuousText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "continuousNumber", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "lightIdentity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "frameStable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "clipInPlace", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "transformInPlace", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "flipInPlace", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "negativeSunDirect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "negativeSunPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "armorSingleRebuild", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "characterSingleRebuild", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "undoRedo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "saveReload", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "inspectorUntouched", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.IsStableLightPreview && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStableLightPreview(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.PressF3 && args.Count == 0)
		{
			PressF3();
			ret = default;
			return true;
		}
		if (method == MethodName.SetTextSession && args.Count == 2)
		{
			SetTextSession(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpinSession && args.Count == 2)
		{
			SetSpinSession(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindDifferentClip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FindDifferentClip(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SameResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SameResourcePath(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 16)
		{
			Finish(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]), VariantUtils.ConvertTo<bool>(in args[10]), VariantUtils.ConvertTo<bool>(in args[11]), VariantUtils.ConvertTo<bool>(in args[12]), VariantUtils.ConvertTo<bool>(in args[13]), VariantUtils.ConvertTo<bool>(in args[14]), VariantUtils.ConvertTo<bool>(in args[15]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PressF3 && args.Count == 0)
		{
			PressF3();
			ret = default;
			return true;
		}
		if (method == MethodName.SetTextSession && args.Count == 2)
		{
			SetTextSession(VariantUtils.ConvertTo<LineEdit>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpinSession && args.Count == 2)
		{
			SetSpinSession(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindDifferentClip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FindDifferentClip(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SameResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SameResourcePath(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.IsStableLightPreview)
		{
			return true;
		}
		if (method == MethodName.PressF3)
		{
			return true;
		}
		if (method == MethodName.SetTextSession)
		{
			return true;
		}
		if (method == MethodName.SetSpinSession)
		{
			return true;
		}
		if (method == MethodName.FindDifferentClip)
		{
			return true;
		}
		if (method == MethodName.SameResourcePath)
		{
			return true;
		}
		if (method == MethodName.FindAncestorWindow)
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
			_editor = VariantUtils.ConvertTo<XWCardVisualResourceEditor>(in value);
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
			_editor = value.As<XWCardVisualResourceEditor>();
		}
		if (info.TryGetProperty(PropertyName._history, out var value2))
		{
			_history = value2.As<XWUndoRedoManager>();
		}
	}
}
