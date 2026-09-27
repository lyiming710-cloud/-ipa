using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/DailyChallengeLevelCardsRuntimeTest.cs")]
public class DailyChallengeLevelCardsRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InjectDailyData = "InjectDailyData";

		public static readonly StringName BuildMetadata = "BuildMetadata";

		public static readonly StringName MouseButtonEvent = "MouseButtonEvent";

		public static readonly StringName MouseMotionEvent = "MouseMotionEvent";

		public static readonly StringName EmulatedMouseButtonEvent = "EmulatedMouseButtonEvent";

		public static readonly StringName EmulatedMouseMotionEvent = "EmulatedMouseMotionEvent";

		public static readonly StringName Utf8 = "Utf8";

		public static readonly StringName RestoreDailyData = "RestoreDailyData";

		public static readonly StringName Check = "Check";

		public static readonly StringName Fail = "Fail";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _originalDailyData = "_originalDailyData";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _cases = "_cases";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "DAILY_CHALLENGE_LEVEL_CARDS_RESULT";

	private const string TestDate = "2026-08-30";

	private const string ChooseScenePath = "res://Prefab/GUI/DialogBox/DailyChallenge/LevelChoose/DailyChallengeLevelChoose.tscn";

	private const string CardScenePath = "res://Prefab/GUI/DialogBox/DailyChallenge/Preview/DailyChallengeLevelPreview.tscn";

	private readonly List<string> _failures = new List<string>();

	private Godot.Collections.Dictionary<string, Variant> _originalDailyData;

	private int _checks;

	private int _cases;

	public override async void _Ready()
	{
		if (!GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			Fail("ResourceManager is unavailable");
			Finish();
			return;
		}
		_originalDailyData = new Godot.Collections.Dictionary<string, Variant>();
		foreach (KeyValuePair<string, Variant> dAILY_LEVEL_DATum in ResourceManager.Instance.DAILY_LEVEL_DATA)
		{
			_originalDailyData[dAILY_LEVEL_DATum.Key] = dAILY_LEVEL_DATum.Value;
		}
		try
		{
			await ValidateSingleCardLayout();
			await ValidateMetadataAndCompletionVisuals();
			await ValidateTwoCardViewport();
			await ValidateThreeCardScrollAndInput();
			await ValidateFiveCardPagination();
			await ValidateEmptyAndInvalidData();
		}
		catch (Exception value)
		{
			Fail($"Unhandled test exception: {value}");
		}
		finally
		{
			RestoreDailyData();
		}
		Finish();
	}

	private async Task ValidateSingleCardLayout()
	{
		_cases++;
		InjectDailyData(1);
		DailyChallengeLevelChoose dailyChallengeLevelChoose = await CreateDialog();
		Check(dailyChallengeLevelChoose.Cards.Count == 1, $"single-card count mismatch: {dailyChallengeLevelChoose.Cards.Count}");
		if (dailyChallengeLevelChoose.Cards.Count == 1)
		{
			DailyChallengeLevelPreview dailyChallengeLevelPreview = dailyChallengeLevelChoose.Cards[0];
			Rect2 globalRect = dailyChallengeLevelChoose.CardsScroll.GetGlobalRect();
			Rect2 globalRect2 = dailyChallengeLevelPreview.GetGlobalRect();
			Check(Mathf.Abs(globalRect2.GetCenter().X - globalRect.GetCenter().X) <= 1f, $"single card is not centered: card={globalRect2.GetCenter().X:F2}, scroll={globalRect.GetCenter().X:F2}");
			Check(dailyChallengeLevelPreview.IsSelected && dailyChallengeLevelChoose.SelectedCard == dailyChallengeLevelPreview, "first available card was not selected by default");
			Check(dailyChallengeLevelPreview.Size.IsEqualApprox(new Vector2(226f, 340f)), $"card size mismatch: {dailyChallengeLevelPreview.Size}");
			Control node = dailyChallengeLevelPreview.GetNode<Control>("%SelectionCorners");
			Check(node.Visible, "default selection corners are hidden");
			Check(node.Size.IsEqualApprox(dailyChallengeLevelPreview.Size), $"selection corner group does not cover the card: {node.Size}");
			Check(node.MouseFilter == Control.MouseFilterEnum.Ignore, "selection corner group intercepts card input");
			(string, Rect2, Vector2)[] array = new (string, Rect2, Vector2)[4]
			{
				("TopLeft", new Rect2(0f, 0f, 44f, 36f), new Vector2(-6f, -6f)),
				("TopRight", new Rect2(60f, 0f, 44f, 36f), new Vector2(180f, -6f)),
				("BottomLeft", new Rect2(0f, 108f, 44f, 36f), new Vector2(-6f, 303f)),
				("BottomRight", new Rect2(60f, 108f, 44f, 36f), new Vector2(180f, 303f))
			};
			for (int i = 0; i < array.Length; i++)
			{
				(string, Rect2, Vector2) tuple = array[i];
				string item = tuple.Item1;
				Rect2 item2 = tuple.Item2;
				Vector2 item3 = tuple.Item3;
				TextureRect node2 = node.GetNode<TextureRect>(item);
				AtlasTexture atlasTexture = node2.Texture as AtlasTexture;
				Check(node2.Size.IsEqualApprox(new Vector2(52f, 43f)) && node2.Position.IsEqualApprox(item3), $"selection corner {item} geometry mismatch: position={node2.Position}, size={node2.Size}");
				Check(GodotObject.IsInstanceValid(atlasTexture) && atlasTexture.Region.IsEqualApprox(item2) && atlasTexture.FilterClip && atlasTexture.Atlas?.ResourcePath == "res://Asset/Texture/TowerDefense/Packet/PacketSelect.png", "selection corner " + item + " does not use the expected packet-selector crop");
				Rect2 globalRect3 = node2.GetGlobalRect();
				Check(globalRect3.Position.X >= globalRect.Position.X - 1f && globalRect3.Position.Y >= globalRect.Position.Y - 1f && globalRect3.End.X <= globalRect.End.X + 1f && globalRect3.End.Y <= globalRect.End.Y + 1f, $"selection corner {item} is clipped by the viewport: corner={globalRect3}, scroll={globalRect}");
			}
			Check(dailyChallengeLevelPreview.Modulate.IsEqualApprox(Colors.White), "selection changed the card content color");
			Label node3 = dailyChallengeLevelChoose.GetNode<Label>("%DateLabel");
			Check(node3.GetThemeColor("font_color").IsEqualApprox(new Color(1f, 0.94902f, 0.737255f)), "date label does not use the high-contrast cream foreground");
			Check(node3.GetThemeColor("font_outline_color").IsEqualApprox(new Color(0.145098f, 0.0862745f, 0.0313726f)), "date label does not use the dark brown outline");
			Check(node3.GetThemeConstant("outline_size") == 4 && node3.GetThemeFontSize("font_size") == 21, "date label contrast sizing is incorrect");
			Control node4 = dailyChallengeLevelChoose.GetNode<Control>("Layer/BackgroundTexture");
			Check(node4.Size.IsEqualApprox(new Vector2(620f, 560f)), $"level chooser background size mismatch: {node4.Size}");
			NinePatchRect node5 = node4.GetNode<NinePatchRect>("BoardTexture");
			Check(node5.Size.IsEqualApprox(new Vector2(806f, 728f)) && node5.Scale.IsEqualApprox(new Vector2(0.769231f, 0.769231f)), $"wide board nine-patch sizing is incorrect: size={node5.Size}, scale={node5.Scale}");
			Check(Mathf.IsEqualApprox(dailyChallengeLevelChoose.CardsScroll.Position.X, 72f) && Mathf.IsEqualApprox(node4.Size.X - dailyChallengeLevelChoose.CardsScroll.Position.X - dailyChallengeLevelChoose.CardsScroll.Size.X, 72f), $"card viewport side margins are not symmetric: position={dailyChallengeLevelChoose.CardsScroll.Position}, size={dailyChallengeLevelChoose.CardsScroll.Size}");
			TextureButton node6 = dailyChallengeLevelChoose.GetNode<TextureButton>("%PreviousPageButton");
			TextureButton node7 = dailyChallengeLevelChoose.GetNode<TextureButton>("%NextPageButton");
			Check(!node6.Visible && !node7.Visible, "single-card layout exposed pagination buttons");
			Check(!dailyChallengeLevelChoose.StatusText.StartsWith("第 ", StringComparison.Ordinal), "single-card layout exposed a redundant page prefix");
			Check(!dailyChallengeLevelChoose.playButton.Disabled, "start button is disabled with a valid default selection");
		}
		await DestroyDialog(dailyChallengeLevelChoose);
	}

	private async Task ValidateMetadataAndCompletionVisuals()
	{
		_cases++;
		InjectDailyData(2);
		DailyChallengeLevelChoose dialog = await CreateDialog();
		Check(dialog.Cards.Count == 2, $"two-card count mismatch: {dialog.Cards.Count}");
		if (dialog.Cards.Count == 2)
		{
			Check(dialog.Cards[0].LevelId == "level-2-1" && dialog.Cards[1].LevelId == "level-2-2", "cards did not preserve LevelDateMap array order");
			Check(dialog.Cards[0].DisplayName == "每日挑战 1" && dialog.Cards[1].DisplayName == "每日挑战 2", "card metadata mapping is incorrect");
			ResourceManager.Instance.DAILY_LEVEL_DATA["LevelMeta"].AsGodotDictionary()["level-2-1"].AsGodotDictionary()["name"] = "已被外部修改";
			Check(dialog.Cards[0].DisplayName == "每日挑战 1", "card did not retain its metadata snapshot");
		}
		DailyChallengeLevelPreview completedCard = GD.Load<PackedScene>("res://Prefab/GUI/DialogBox/DailyChallenge/Preview/DailyChallengeLevelPreview.tscn")?.Instantiate<DailyChallengeLevelPreview>(PackedScene.GenEditState.Disabled);
		Check(GodotObject.IsInstanceValid(completedCard), "unable to instantiate completion-card fixture");
		if (GodotObject.IsInstanceValid(completedCard))
		{
			AddChild(completedCard, forceReadableName: false, InternalMode.Disabled);
			Dictionary dictionary = BuildMetadata("completed", 0, malformedApi: false);
			dictionary["tag"] = "首领";
			dictionary["tags"] = new Godot.Collections.Array
			{
				"布局",
				new Godot.Collections.Array { "夜间", "首领", "水池" },
				" ",
				"限时",
				"传送带",
				"小游戏",
				"超长标签完整名称",
				"不应显示"
			};
			completedCard.Bind("completed", dictionary, finished: true);
			await WaitFrames(2);
			Check(completedCard.IsFinished, "completion state was not retained by the card");
			Check(completedCard.GetNode<TextureRect>("%FinishTexture").Visible, "completion marker is hidden");
			HFlowContainer tagFlow = completedCard.GetNode<HFlowContainer>("%TagFlow");
			List<PanelContainer> visibleTagChips = GetVisibleTagChips(tagFlow);
			string[] array = new string[8] { "首领", "布局", "夜间", "水池", "限时", "传送带", "小游戏", "超长标签完整名称" };
			Check(tagFlow.Size.IsEqualApprox(new Vector2(210f, 68f)), $"tag flow size mismatch: {tagFlow.Size}");
			Check(visibleTagChips.Count == 8, $"eight-tag metadata created {visibleTagChips.Count} visible chips");
			Check(completedCard.GetNodeOrNull<Label>("%TagLabel") == null, "legacy joined tag label still exists");
			System.Collections.Generic.Dictionary<int, int> dictionary2 = new System.Collections.Generic.Dictionary<int, int>();
			Rect2 globalRect = tagFlow.GetGlobalRect();
			for (int i = 0; i < visibleTagChips.Count; i++)
			{
				PanelContainer panelContainer = visibleTagChips[i];
				Label node = panelContainer.GetNode<Label>("Text");
				Check(i < array.Length && node.Text == array[i], $"tag chip order/text mismatch at {i}: {node.Text}");
				Check(!node.Text.Contains("标签：", StringComparison.Ordinal) && !node.Text.Contains('、'), "tag chip retained the legacy prefix or delimiter: " + node.Text);
				Check(panelContainer.Size.X >= 67f && panelContainer.Size.X <= 68f && Mathf.IsEqualApprox(panelContainer.Size.Y, 20f), $"tag chip size mismatch at {i}: {panelContainer.Size}");
				Rect2 globalRect2 = panelContainer.GetGlobalRect();
				Check(globalRect2.Position.X >= globalRect.Position.X - 1f && globalRect2.Position.Y >= globalRect.Position.Y - 1f && globalRect2.End.X <= globalRect.End.X + 1f && globalRect2.End.Y <= globalRect.End.Y + 1f, $"tag chip escaped the tag area at {i}: chip={globalRect2}, flow={globalRect}");
				int key = Mathf.RoundToInt(panelContainer.Position.Y);
				dictionary2[key] = dictionary2.GetValueOrDefault(key, 0) + 1;
			}
			Check(dictionary2.Count == 3, $"eight tags did not use exactly three rows: {dictionary2.Count}");
			foreach (int value in dictionary2.Values)
			{
				Check(value <= 3, $"tag row exceeded three chips: {value}");
			}
			if (visibleTagChips.Count == array.Length)
			{
				List<PanelContainer> list = visibleTagChips;
				PanelContainer panelContainer2 = list[list.Count - 1];
				Label node2 = panelContainer2.GetNode<Label>("Text");
				Check(panelContainer2.TooltipText == array[^1] && node2.ClipText && node2.TextOverrunBehavior == TextServer.OverrunBehavior.TrimEllipsis, "long tag does not expose its full tooltip with ellipsis behavior");
			}
			Dictionary dictionary3 = BuildMetadata("completed", 0, malformedApi: false);
			dictionary3.Remove("tags");
			dictionary3["tag"] = "  ";
			completedCard.Bind("completed", dictionary3, finished: true);
			await WaitFrames(2);
			visibleTagChips = GetVisibleTagChips(tagFlow);
			Check(visibleTagChips.Count == 1 && visibleTagChips[0].GetNode<Label>("Text").Text == "无", "rebinding empty tags did not replace old chips with the empty chip");
			completedCard.QueueFree();
			await WaitFrames(2);
		}
		await DestroyDialog(dialog);
	}

	private async Task ValidateTwoCardViewport()
	{
		_cases++;
		InjectDailyData(2);
		DailyChallengeLevelChoose dailyChallengeLevelChoose = await CreateDialog();
		Check(dailyChallengeLevelChoose.Cards.Count == 2, $"two-card count mismatch: {dailyChallengeLevelChoose.Cards.Count}");
		Rect2 globalRect = dailyChallengeLevelChoose.CardsScroll.GetGlobalRect();
		Check(dailyChallengeLevelChoose.CardsScroll.Size.IsEqualApprox(new Vector2(476f, 356f)), $"two-card viewport size mismatch: {dailyChallengeLevelChoose.CardsScroll.Size}");
		float num = -1f / 0f;
		float num2 = ((dailyChallengeLevelChoose.Cards.Count > 0) ? dailyChallengeLevelChoose.Cards[0].GlobalPosition.Y : 0f);
		foreach (DailyChallengeLevelPreview card in dailyChallengeLevelChoose.Cards)
		{
			Rect2 globalRect2 = card.GetGlobalRect();
			Check(globalRect2.Position.X >= globalRect.Position.X - 1f && globalRect2.End.X <= globalRect.End.X + 1f, $"two-card layout clips {card.LevelId}: card={globalRect2}, scroll={globalRect}");
			Check(globalRect2.Position.X > num, "two-card order is not strictly horizontal");
			Check(Mathf.Abs(card.GlobalPosition.Y - num2) <= 1f, "two-card layout wrapped to another row");
			num = globalRect2.Position.X;
		}
		ScrollBar hScrollBar = dailyChallengeLevelChoose.CardsScroll.GetHScrollBar();
		Check(hScrollBar.MaxValue <= hScrollBar.Page + 1.0, $"two cards unexpectedly require scrolling: max={hScrollBar.MaxValue}, page={hScrollBar.Page}");
		Check(dailyChallengeLevelChoose.Cards.Count < 2 || Mathf.Abs(dailyChallengeLevelChoose.Cards[0].GetGlobalRect().Position.X - (globalRect.Position.X + 6f)) <= 1f, "two-card layout did not reserve the left selection-corner inset");
		DailyChallengeLevelCardsRuntimeTest dailyChallengeLevelCardsRuntimeTest = this;
		int condition;
		if (dailyChallengeLevelChoose.Cards.Count >= 2)
		{
			Array<DailyChallengeLevelPreview> cards = dailyChallengeLevelChoose.Cards;
			condition = ((Mathf.Abs(cards[cards.Count - 1].GetGlobalRect().End.X - (globalRect.End.X - 6f)) <= 1f) ? 1 : 0);
		}
		else
		{
			condition = 1;
		}
		dailyChallengeLevelCardsRuntimeTest.Check((byte)condition != 0, "two-card layout did not reserve the right selection-corner inset");
		TextureButton node = dailyChallengeLevelChoose.GetNode<TextureButton>("%PreviousPageButton");
		TextureButton node2 = dailyChallengeLevelChoose.GetNode<TextureButton>("%NextPageButton");
		Check(!node.Visible && !node2.Visible, "two-card layout exposed pagination buttons");
		Check(!dailyChallengeLevelChoose.StatusText.StartsWith("第 ", StringComparison.Ordinal), "two-card layout exposed a redundant page prefix");
		await DestroyDialog(dailyChallengeLevelChoose);
	}

	private async Task ValidateThreeCardScrollAndInput()
	{
		_cases++;
		InjectDailyData(3);
		DailyChallengeLevelChoose dialog = await CreateDialog();
		Check(dialog.Cards.Count == 3, $"three-card count mismatch: {dialog.Cards.Count}");
		if (dialog.Cards.Count < 2)
		{
			await DestroyDialog(dialog);
			return;
		}
		float y = dialog.Cards[0].GlobalPosition.Y;
		float num = -1f / 0f;
		foreach (DailyChallengeLevelPreview card in dialog.Cards)
		{
			Check(Mathf.Abs(card.GlobalPosition.Y - y) <= 1f, "three-card layout wrapped to another row");
			Check(card.GlobalPosition.X > num, "three-card order is not strictly horizontal");
			num = card.GlobalPosition.X;
		}
		ScrollBar hScrollBar = dialog.CardsScroll.GetHScrollBar();
		Check(hScrollBar.MaxValue > hScrollBar.Page + 1.0, $"three cards did not create horizontal range: max={hScrollBar.MaxValue}, page={hScrollBar.Page}");
		int maximumScroll = Mathf.Max(0, Mathf.CeilToInt(hScrollBar.MaxValue - hScrollBar.Page));
		TextureButton previous = dialog.GetNode<TextureButton>("%PreviousPageButton");
		TextureButton next = dialog.GetNode<TextureButton>("%NextPageButton");
		DailyChallengeLevelPreview first = dialog.Cards[0];
		DailyChallengeLevelPreview second = dialog.Cards[1];
		Check(previous.Visible && next.Visible, "three-card layout did not expose pagination buttons");
		Check(previous.Size.IsEqualApprox(new Vector2(60f, 60f)) && next.Size.IsEqualApprox(new Vector2(60f, 60f)) && Mathf.IsEqualApprox(previous.Position.X, 10f) && Mathf.IsEqualApprox(next.Position.X, 550f), "pagination buttons do not use the expected side placement");
		Check(previous.TextureNormal?.ResourcePath == "res://Asset/Texture/GUI/General/General/ArrowButton.png" && previous.TexturePressed?.ResourcePath == "res://Asset/Texture/GUI/General/General/ArrowButtonDown.png" && !previous.FlipH && next.FlipH, "pagination buttons do not reuse and mirror the canonical arrow textures");
		Check(!hScrollBar.Visible, "native horizontal scrollbar remained visible beside the page controls");
		Check(previous.Disabled && !next.Disabled, "first page did not disable only the previous-page button");
		Check(!previous.Modulate.IsEqualApprox(Colors.White) && next.Modulate.IsEqualApprox(Colors.White), "first-page button dimming is incorrect");
		Check(dialog.StatusText.StartsWith("第 1/2 页 · ", StringComparison.Ordinal), "first-page status prefix is incorrect: " + dialog.StatusText);
		next.EmitSignal(BaseButton.SignalName.Pressed);
		Check(previous.Disabled && next.Disabled, "page buttons accepted repeated input during the paging tween");
		Check(dialog.SelectedCard == first, "next-page button changed the selected card");
		await WaitFrames(24);
		Check(Mathf.Abs(dialog.CardsScroll.ScrollHorizontal - maximumScroll) <= 1, $"next-page button did not reach the final page: scroll={dialog.CardsScroll.ScrollHorizontal}, max={maximumScroll}");
		Check(!previous.Disabled && next.Disabled, "last page did not disable only the next-page button");
		Check(previous.Modulate.IsEqualApprox(Colors.White) && !next.Modulate.IsEqualApprox(Colors.White), "last-page button dimming is incorrect");
		Check(dialog.StatusText.StartsWith("第 2/2 页 · ", StringComparison.Ordinal), "last-page status prefix is incorrect: " + dialog.StatusText);
		Rect2 globalRect = dialog.CardsScroll.GetGlobalRect();
		Array<DailyChallengeLevelPreview> cards = dialog.Cards;
		Rect2 globalRect2 = cards[cards.Count - 1].GetGlobalRect();
		Check(globalRect2.End.X <= globalRect.End.X - 5f && globalRect2.End.X >= globalRect.End.X - 7f, $"last card is not reachable at maximum scroll: card={globalRect2}, scroll={globalRect}");
		previous.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(24);
		Check(dialog.CardsScroll.ScrollHorizontal <= 1 && previous.Disabled && !next.Disabled && dialog.StatusText.StartsWith("第 1/2 页 · ", StringComparison.Ordinal), "previous-page button did not return to the first page");
		Vector2 touchStart = second.GetGlobalRect().GetCenter();
		bool originalEmulateTouch = Input.EmulateTouchFromMouse;
		try
		{
			Input.EmulateTouchFromMouse = true;
			GetViewport().NotifyMouseEntered();
			await PushViewportInput(EmulatedMouseMotionEvent(touchStart, Vector2.Zero));
			await PushViewportInput(EmulatedMouseButtonEvent(pressed: true, touchStart));
			await PushViewportInput(EmulatedMouseMotionEvent(touchStart - new Vector2(32f, 0f), new Vector2(-32f, 0f)));
			await PushViewportInput(EmulatedMouseMotionEvent(touchStart - new Vector2(180f, 0f), new Vector2(-148f, 0f)));
			await PushViewportInput(EmulatedMouseButtonEvent(pressed: false, touchStart - new Vector2(180f, 0f)));
			await WaitFrames(8);
		}
		finally
		{
			Input.EmulateTouchFromMouse = originalEmulateTouch;
		}
		Check(dialog.CardsScroll.ScrollHorizontal >= maximumScroll / 2, "viewport touch drag did not move far enough to update the page");
		Check(dialog.SelectedCard == first, "viewport touch drag selected a card");
		Check(!previous.Disabled && next.Disabled && dialog.StatusText.StartsWith("第 2/2 页 · ", StringComparison.Ordinal), "viewport touch drag did not synchronize the page indicator and arrows");
		dialog.CardsScroll.ScrollHorizontal = 0;
		await WaitFrames(3);
		Vector2 pressPosition = new Vector2(84f, 150f);
		second.HandleCardInput(MouseButtonEvent(pressed: true, pressPosition));
		second.HandleCardInput(MouseMotionEvent(new Vector2(112f, 150f), new Vector2(28f, 0f)));
		second.HandleCardInput(MouseButtonEvent(pressed: false, new Vector2(112f, 150f)));
		Check(dialog.SelectedCard == first, "drag gesture selected a card");
		second.HandleCardInput(MouseButtonEvent(pressed: true, pressPosition));
		dialog.CardsScroll.ScrollHorizontal = Math.Min(10, maximumScroll);
		second.HandleCardInput(MouseButtonEvent(pressed: false, pressPosition));
		Check(dialog.SelectedCard == first, "scroll-offset change selected a card on release");
		dialog.CardsScroll.ScrollHorizontal = 0;
		await WaitFrames(3);
		Vector2 secondCenter = second.GetGlobalRect().GetCenter();
		await PushViewportInput(new InputEventScreenTouch
		{
			Index = 1,
			Pressed = true,
			Position = secondCenter
		});
		await PushViewportInput(new InputEventScreenTouch
		{
			Index = 1,
			Pressed = false,
			Position = secondCenter
		});
		await WaitFrames(2);
		Check(dialog.SelectedCard == second && second.IsSelected && !first.IsSelected, "viewport click did not switch the single selection");
		Check(second.GetNode<Control>("%SelectionCorners").Visible && !first.GetNode<Control>("%SelectionCorners").Visible, "selection corners did not move exclusively to the clicked card");
		Check(!dialog.IsBusy && dialog.StatusText.Contains("每日挑战 2", StringComparison.Ordinal), "card selection started loading instead of only updating selection");
		int requestCount = 0;
		string requestedUrl = "";
		dialog.LevelRequestOverride = (string url, string[] requestHeaders) =>
		{
			requestCount++;
			requestedUrl = url;
			return Error.Ok;
		};
		Json battleJson = null;
		dialog.BattleStartOverride = (Json json) =>
		{
			battleJson = json;
		};
		dialog.PlayButtonPressed();
		Check(dialog.IsBusy && dialog.playButton.Disabled, "shared start button did not enter the loading state");
		Check(previous.Disabled && next.Disabled, "loading state did not disable both page buttons");
		Check(requestCount == 1 && requestedUrl.EndsWith("/test/daily/level-3-2", StringComparison.Ordinal), $"shared start requested the wrong level or request count: count={requestCount}, url={requestedUrl}");
		Check(!first.IsInteractionEnabled && !second.IsInteractionEnabled, "loading state did not disable card selection");
		dialog.PlayButtonPressed();
		Check(requestCount == 1, "repeated start while busy created a duplicate request");
		first.HandleCardInput(MouseButtonEvent(pressed: true, pressPosition));
		first.HandleCardInput(MouseButtonEvent(pressed: false, pressPosition));
		Check(dialog.SelectedCard == second, "busy-state input changed the pending selection");
		dialog.LevelHttpRequestCompleted(1L, 0L, System.Array.Empty<string>(), System.Array.Empty<byte>());
		Check(!dialog.IsBusy && dialog.SelectedCard == second && !dialog.playButton.Disabled, "transport failure did not restore the selected card and start button");
		Check(first.IsInteractionEnabled && second.IsInteractionEnabled, "transport failure did not restore card interaction");
		Check(previous.Disabled && !next.Disabled, "transport failure did not restore the current page button states");
		dialog.PlayButtonPressed();
		dialog.LevelHttpRequestCompleted(0L, 503L, System.Array.Empty<string>(), Utf8("{}"));
		Check(!dialog.IsBusy && dialog.StatusText.Contains("HTTP 503", StringComparison.Ordinal), "HTTP failure was not rejected and surfaced");
		dialog.PlayButtonPressed();
		dialog.LevelHttpRequestCompleted(0L, 200L, System.Array.Empty<string>(), Utf8("not-json"));
		Check(!dialog.IsBusy && dialog.StatusText.Contains("格式错误", StringComparison.Ordinal), "invalid JSON did not restore the dialog");
		dialog.PlayButtonPressed();
		dialog.LevelHttpRequestCompleted(0L, 200L, System.Array.Empty<string>(), Utf8("[]"));
		Check(!dialog.IsBusy && dialog.StatusText.Contains("格式错误", StringComparison.Ordinal), "non-dictionary JSON did not restore the dialog");
		dialog.PlayButtonPressed();
		dialog.LevelHttpRequestCompleted(0L, 200L, System.Array.Empty<string>(), Utf8("{\"error\":\"not found\"}"));
		Check(!dialog.IsBusy && dialog.StatusText.Contains("有效关卡", StringComparison.Ordinal), "server error payload did not restore the dialog");
		Variant value = second.Metadata["reward"];
		second.Metadata.Remove("reward");
		dialog.PlayButtonPressed();
		dialog.LevelHttpRequestCompleted(0L, 200L, System.Array.Empty<string>(), Utf8("{}"));
		Check(!dialog.IsBusy && dialog.StatusText.Contains("奖励数据无效", StringComparison.Ordinal), "missing reward metadata did not restore the dialog");
		second.Metadata["reward"] = value;
		dialog.PlayButtonPressed();
		ResourceManager.Instance.DAILY_LEVEL_DATA["LevelMeta"].AsGodotDictionary()["level-3-2"].AsGodotDictionary()["reward"] = new Dictionary
		{
			["type"] = "Coin",
			["value"] = 9999
		};
		dialog.LevelHttpRequestCompleted(0L, 200L, System.Array.Empty<string>(), Utf8("{\"Map\":\"Frontlawn\",\"FinishMethod\":\"WAVE\"}"));
		Check(requestCount == 7, $"request state-machine count mismatch: {requestCount}");
		Check(dialog.IsBusy && dialog.playButton.Disabled && dialog.GetNode<TextureButton>("%CloseButton").Disabled, "successful transition did not keep the dialog locked");
		Check(battleJson != null && battleJson.Data.VariantType == Variant.Type.Dictionary, "successful response did not reach the battle transition");
		if (battleJson != null && battleJson.Data.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary = battleJson.Data.AsGodotDictionary();
			Check(dictionary.GetValueOrDefault("Name", "").AsString() == "DailyLevel-2026-08-30-level-3-2", "successful response did not use the pending level snapshot");
			Dictionary dictionary2 = dictionary.GetValueOrDefault("Reward", new Dictionary()).AsGodotDictionary();
			Check(dictionary2.GetValueOrDefault("RewardType", "").AsString() == "Coin" && dictionary2.GetValueOrDefault("RewardFirst", 0).AsInt32() == 101, "successful response did not use the pending reward snapshot");
		}
		await DestroyDialog(dialog);
	}

	private async Task ValidateFiveCardPagination()
	{
		_cases++;
		InjectDailyData(5);
		DailyChallengeLevelChoose dialog = await CreateDialog();
		Check(dialog.Cards.Count == 5, $"five-card count mismatch: {dialog.Cards.Count}");
		if (dialog.Cards.Count != 5)
		{
			await DestroyDialog(dialog);
			return;
		}
		TextureButton previous = dialog.GetNode<TextureButton>("%PreviousPageButton");
		TextureButton next = dialog.GetNode<TextureButton>("%NextPageButton");
		ScrollBar hScrollBar = dialog.CardsScroll.GetHScrollBar();
		int pageWidth = Mathf.RoundToInt(dialog.CardsScroll.Size.X);
		int maximumScroll = Mathf.Max(0, Mathf.CeilToInt(hScrollBar.MaxValue - hScrollBar.Page));
		DailyChallengeLevelPreview originalSelection = dialog.SelectedCard;
		Check(dialog.StatusText.StartsWith("第 1/3 页 · ", StringComparison.Ordinal), "five-card first-page status prefix is incorrect: " + dialog.StatusText);
		next.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(24);
		Check(Mathf.Abs(dialog.CardsScroll.ScrollHorizontal - pageWidth) <= 1, $"five-card second page did not align to one viewport: scroll={dialog.CardsScroll.ScrollHorizontal}, page={pageWidth}");
		Check(!previous.Disabled && !next.Disabled && dialog.StatusText.StartsWith("第 2/3 页 · ", StringComparison.Ordinal), "five-card middle page controls or status are incorrect");
		Check(dialog.SelectedCard == originalSelection, "moving to the five-card middle page changed the selected card");
		next.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(24);
		Check(Mathf.Abs(dialog.CardsScroll.ScrollHorizontal - maximumScroll) <= 1, $"five-card final page did not clamp to maximum scroll: scroll={dialog.CardsScroll.ScrollHorizontal}, max={maximumScroll}");
		Check(!previous.Disabled && next.Disabled && dialog.StatusText.StartsWith("第 3/3 页 · ", StringComparison.Ordinal), "five-card final page controls or status are incorrect");
		Check(dialog.SelectedCard == originalSelection, "moving to the five-card final page changed the selected card");
		Rect2 globalRect = dialog.CardsScroll.GetGlobalRect();
		Array<DailyChallengeLevelPreview> cards = dialog.Cards;
		Rect2 globalRect2 = cards[cards.Count - 1].GetGlobalRect();
		Check(globalRect2.End.X <= globalRect.End.X - 5f && globalRect2.End.X >= globalRect.End.X - 7f, $"five-card final page leaves unnecessary trailing space: card={globalRect2}, scroll={globalRect}");
		previous.EmitSignal(BaseButton.SignalName.Pressed);
		await WaitFrames(24);
		Check(Mathf.Abs(dialog.CardsScroll.ScrollHorizontal - pageWidth) <= 1 && dialog.StatusText.StartsWith("第 2/3 页 · ", StringComparison.Ordinal), "five-card previous-page navigation skipped the middle page");
		await DestroyDialog(dialog);
	}

	private async Task ValidateEmptyAndInvalidData()
	{
		_cases++;
		InjectDailyData(0);
		DailyChallengeLevelChoose dailyChallengeLevelChoose = await CreateDialog();
		Check(dailyChallengeLevelChoose.Cards.Count == 0, "empty date created cards");
		Check(dailyChallengeLevelChoose.playButton.Disabled, "empty date enabled the start button");
		Check(dailyChallengeLevelChoose.StatusText.Contains("暂无", StringComparison.Ordinal), "empty date did not show an empty state");
		await DestroyDialog(dailyChallengeLevelChoose);
		InjectDailyData(1, -1, 0);
		DailyChallengeLevelChoose dailyChallengeLevelChoose2 = await CreateDialog();
		Check(dailyChallengeLevelChoose2.Cards.Count == 1 && !dailyChallengeLevelChoose2.Cards[0].IsAvailable, "missing API metadata was not represented as an unavailable card");
		Check(dailyChallengeLevelChoose2.SelectedCard == null && dailyChallengeLevelChoose2.playButton.Disabled, "invalid-only date retained a selection or enabled start");
		Check(dailyChallengeLevelChoose2.Cards[0].GetNode<Label>("%UnavailableLabel").Visible, "invalid card did not show its unavailable marker");
		await DestroyDialog(dailyChallengeLevelChoose2);
		InjectDailyData(2, 0);
		DailyChallengeLevelChoose dailyChallengeLevelChoose3 = await CreateDialog();
		Check(!dailyChallengeLevelChoose3.Cards[0].IsAvailable && dailyChallengeLevelChoose3.SelectedLevelId == "level-2-2", "malformed first API was selected instead of the first valid card");
		await DestroyDialog(dailyChallengeLevelChoose3);
	}

	private async Task<DailyChallengeLevelChoose> CreateDialog()
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Prefab/GUI/DialogBox/DailyChallenge/LevelChoose/DailyChallengeLevelChoose.tscn");
		Check(GodotObject.IsInstanceValid(packedScene), "unable to load choose scene: res://Prefab/GUI/DialogBox/DailyChallenge/LevelChoose/DailyChallengeLevelChoose.tscn");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return null;
		}
		DailyChallengeLevelChoose dialog = packedScene.Instantiate<DailyChallengeLevelChoose>(PackedScene.GenEditState.Disabled);
		dialog.Init(new Dictionary { ["date"] = "2026-08-30" });
		AddChild(dialog, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(4);
		return dialog;
	}

	private async Task DestroyDialog(DailyChallengeLevelChoose dialog)
	{
		if (GodotObject.IsInstanceValid(dialog))
		{
			dialog.QueueFree();
		}
		await WaitFrames(3);
	}

	private void InjectDailyData(int count, int malformedApiIndex = -1, int missingApiIndex = -1)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		Dictionary dictionary = new Dictionary();
		for (int i = 0; i < count; i++)
		{
			string text = $"level-{count}-{i + 1}";
			array.Add(text);
			dictionary[text] = BuildMetadata(text, i, malformedApiIndex == i, missingApiIndex == i);
		}
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		dAILY_LEVEL_DATA.Clear();
		dAILY_LEVEL_DATA["LevelDateMap"] = new Dictionary { ["2026-08-30"] = array };
		dAILY_LEVEL_DATA["LevelMeta"] = dictionary;
		dAILY_LEVEL_DATA["LevelDateMeta"] = new Dictionary();
	}

	private static Dictionary BuildMetadata(string levelId, int index, bool malformedApi, bool missingApi = false)
	{
		Dictionary dictionary = new Dictionary
		{
			["id"] = levelId,
			["name"] = $"每日挑战 {index + 1}",
			["type"] = ((index % 2 == 0) ? "普通" : "特殊"),
			["map"] = "Frontlawn",
			["finishMethod"] = ((index % 2 == 0) ? "WAVE" : "VASE"),
			["survivalRoundlimit"] = ((index % 2 == 0) ? (-1) : 0),
			["tags"] = new Godot.Collections.Array
			{
				"布局",
				$"标签{index + 1}"
			},
			["reward"] = new Dictionary
			{
				["type"] = "Coin",
				["value"] = 100 + index
			}
		};
		if (!missingApi)
		{
			dictionary["api"] = (malformedApi ? "invalid-api" : ("/test/daily/" + levelId));
		}
		return dictionary;
	}

	private static List<PanelContainer> GetVisibleTagChips(HFlowContainer tagFlow)
	{
		List<PanelContainer> list = new List<PanelContainer>();
		foreach (Node child in tagFlow.GetChildren())
		{
			if (child is PanelContainer { Visible: not false } panelContainer)
			{
				list.Add(panelContainer);
			}
		}
		return list;
	}

	private static InputEventMouseButton MouseButtonEvent(bool pressed, Vector2 position)
	{
		return new InputEventMouseButton
		{
			ButtonIndex = MouseButton.Left,
			Pressed = pressed,
			Position = position
		};
	}

	private static InputEventMouseMotion MouseMotionEvent(Vector2 position, Vector2 relative)
	{
		return new InputEventMouseMotion
		{
			Position = position,
			Relative = relative
		};
	}

	private static InputEventMouseButton EmulatedMouseButtonEvent(bool pressed, Vector2 position)
	{
		return new InputEventMouseButton
		{
			Device = -1,
			ButtonIndex = MouseButton.Left,
			ButtonMask = (MouseButtonMask)(pressed ? 1 : 0),
			Pressed = pressed,
			Position = position,
			GlobalPosition = position
		};
	}

	private static InputEventMouseMotion EmulatedMouseMotionEvent(Vector2 position, Vector2 relative)
	{
		return new InputEventMouseMotion
		{
			Device = -1,
			ButtonMask = (MouseButtonMask)((relative == Vector2.Zero) ? 0 : 1),
			Position = position,
			GlobalPosition = position,
			Relative = relative,
			ScreenRelative = relative
		};
	}

	private async Task PushViewportInput(InputEvent inputEvent)
	{
		GetViewport().PushInput(inputEvent, inLocalCoords: true);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		inputEvent.Dispose();
	}

	private static byte[] Utf8(string text)
	{
		return Encoding.UTF8.GetBytes(text);
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void RestoreDailyData()
	{
		if (!GodotObject.IsInstanceValid(ResourceManager.Instance) || _originalDailyData == null)
		{
			return;
		}
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		dAILY_LEVEL_DATA.Clear();
		foreach (KeyValuePair<string, Variant> originalDailyDatum in _originalDailyData)
		{
			dAILY_LEVEL_DATA[originalDailyDatum.Key] = originalDailyDatum.Value;
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			Fail(message);
		}
	}

	private void Fail(string message)
	{
		_failures.Add(message);
		GD.PrintErr("DAILY_CHALLENGE_LEVEL_CARDS_FAILURE " + message);
	}

	private void Finish()
	{
		bool flag = _failures.Count == 0;
		GD.Print($"{"DAILY_CHALLENGE_LEVEL_CARDS_RESULT"} passed={flag} cases={_cases} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InjectDailyData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "malformedApiIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "missingApiIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildMetadata, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "malformedApi", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "missingApi", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MouseButtonEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MouseMotionEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseMotion"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "relative", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmulatedMouseButtonEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmulatedMouseMotionEvent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseMotion"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "relative", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Utf8, new PropertyInfo(Variant.Type.PackedByteArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreDailyData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Fail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
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
		if (method == MethodName.InjectDailyData && args.Count == 3)
		{
			InjectDailyData(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildMetadata && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildMetadata(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.MouseButtonEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseButton>(MouseButtonEvent(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.MouseMotionEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseMotion>(MouseMotionEvent(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.EmulatedMouseButtonEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseButton>(EmulatedMouseButtonEvent(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.EmulatedMouseMotionEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseMotion>(EmulatedMouseMotionEvent(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.Utf8 && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<byte[]>(Utf8(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RestoreDailyData && args.Count == 0)
		{
			RestoreDailyData();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Fail && args.Count == 1)
		{
			Fail(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BuildMetadata && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildMetadata(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.MouseButtonEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseButton>(MouseButtonEvent(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.MouseMotionEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseMotion>(MouseMotionEvent(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.EmulatedMouseButtonEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseButton>(EmulatedMouseButtonEvent(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.EmulatedMouseMotionEvent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<InputEventMouseMotion>(EmulatedMouseMotionEvent(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.Utf8 && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<byte[]>(Utf8(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.InjectDailyData)
		{
			return true;
		}
		if (method == MethodName.BuildMetadata)
		{
			return true;
		}
		if (method == MethodName.MouseButtonEvent)
		{
			return true;
		}
		if (method == MethodName.MouseMotionEvent)
		{
			return true;
		}
		if (method == MethodName.EmulatedMouseButtonEvent)
		{
			return true;
		}
		if (method == MethodName.EmulatedMouseMotionEvent)
		{
			return true;
		}
		if (method == MethodName.Utf8)
		{
			return true;
		}
		if (method == MethodName.RestoreDailyData)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Fail)
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
		if (name == PropertyName._originalDailyData)
		{
			_originalDailyData = VariantUtils.ConvertToDictionary<string, Variant>(in value);
			return true;
		}
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cases)
		{
			_cases = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._originalDailyData)
		{
			value = VariantUtils.CreateFromDictionary(_originalDailyData);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._cases)
		{
			value = VariantUtils.CreateFrom(in _cases);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._originalDailyData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cases, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._originalDailyData, Variant.CreateFrom(_originalDailyData));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._cases, Variant.From(in _cases));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._originalDailyData, out var value))
		{
			_originalDailyData = value.AsGodotDictionary<string, Variant>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value2))
		{
			_checks = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cases, out var value3))
		{
			_cases = value3.As<int>();
		}
	}
}
