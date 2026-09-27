using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/SlotMachine/SlotMachineControl.cs")]
public class SlotMachineControl : Control
{
	private sealed class ReelView
	{
		public Control Clip;

		public Control Track;

		public CardFace FinalCard;

		public float Offset;

		public float TargetOffset;

		public double StopTime = 0.1;

		public bool Stopped = true;
	}

	private sealed class CardFace
	{
		public Control Root;

		public TowerDefenseInGamePacketShow Packet;

		public string ItemKey = "";

		public string DisplayKey = "";
	}

	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _GuiInput = "_GuiInput";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName SetStatusText = "SetStatusText";

		public static readonly StringName SetInteractable = "SetInteractable";

		public static readonly StringName BindSceneNodes = "BindSceneNodes";

		public static readonly StringName ConfigurePacketShow = "ConfigurePacketShow";

		public static readonly StringName ShowPacketViewport = "ShowPacketViewport";

		public static readonly StringName GetReelStopTime = "GetReelStopTime";

		public static readonly StringName SetFinalItems = "SetFinalItems";

		public static readonly StringName SetReelsToFinalOffset = "SetReelsToFinalOffset";

		public static readonly StringName ScheduleSpinCompletion = "ScheduleSpinCompletion";

		public static readonly StringName CompleteSpinOnNextFrame = "CompleteSpinOnNextFrame";

		public static readonly StringName CancelSpinSettleCallback = "CancelSpinSettleCallback";

		public static readonly StringName CancelPendingWork = "CancelPendingWork";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GetCurrentItems = "GetCurrentItems";

		public static readonly StringName SetReelItem = "SetReelItem";

		public static readonly StringName GetFinalItem = "GetFinalItem";

		public static readonly StringName GetRandomSpinItem = "GetRandomSpinItem";

		public static readonly StringName GetDisplayPacketName = "GetDisplayPacketName";

		public static readonly StringName SetHandleProgress = "SetHandleProgress";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _feature = "_feature";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _shaftTexture = "_shaftTexture";

		public static readonly StringName _ballTexture = "_ballTexture";

		public static readonly StringName _audioPlayer = "_audioPlayer";

		public static readonly StringName _spinItems = "_spinItems";

		public static readonly StringName _finalItems = "_finalItems";

		public static readonly StringName _spinTimer = "_spinTimer";

		public static readonly StringName _spinDuration = "_spinDuration";

		public static readonly StringName _spinning = "_spinning";

		public static readonly StringName _spinSettleTree = "_spinSettleTree";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const float ReelWidth = 49f;

	private const float ReelHeight = 47f;

	private const float ReelStep = 82f;

	private const float PacketScale = 0.86f;

	private const float HandleShaftLength = 56f;

	private const float HandleRestDegrees = -50f;

	private const float HandlePullDegrees = 35f;

	private static readonly Vector2 PacketPosition = new Vector2(23.5f, 20f);

	private static readonly Vector2 HandlePivot = new Vector2(286f, 73.5f);

	private static readonly Vector2 HandleBaseVisibleCenter = new Vector2(12.5f, 77f);

	private static readonly Vector2 HandleShaftPivot = new Vector2(4f, 5.5f);

	private static readonly Vector2 HandleBallSize = new Vector2(25f, 25f);

	private static readonly string[] FallbackSpinItems = new string[4] { "PlantPeaShooterSingle", "PlantWallnut", "PlantSnowPea", "PlantSunFlower" };

	private TowerDefenseBattleFeatureSlotMachine _feature;

	private readonly ReelView[] _reels = new ReelView[3];

	private Label _statusLabel;

	private TextureRect _shaftTexture;

	private TextureRect _ballTexture;

	private AudioStreamPlayer _audioPlayer;

	private string[] _spinItems = FallbackSpinItems;

	private string[] _finalItems = new string[3] { "PlantPeaShooterSingle", "PlantWallnut", "PlantSnowPea" };

	private Action<string[]> _spinFinished;

	private double _spinTimer;

	private double _spinDuration = 1.2;

	private bool _spinning;

	private SceneTree _spinSettleTree;

	private Action _spinSettleHandler;

	public void Init(TowerDefenseBattleFeatureSlotMachine feature)
	{
		_feature = feature;
		_spinItems = _feature.GetSpinItemKeys();
		if (_spinItems.Length == 0)
		{
			_spinItems = FallbackSpinItems;
		}
		Name = "SlotMachineControl";
		CustomMinimumSize = new Vector2(340f, 108f);
		Size = CustomMinimumSize;
		MouseFilter = MouseFilterEnum.Stop;
		MouseDefaultCursorShape = CursorShape.PointingHand;
		if (IsNodeReady())
		{
			BindSceneNodes();
		}
		SetStatusText(_feature.GetStatusText());
	}

	public override void _Ready()
	{
		BindSceneNodes();
		for (int i = 0; i < _reels.Length; i++)
		{
			SetReelItem(i, GetFinalItem(i));
		}
		if (_feature != null)
		{
			SetStatusText(_feature.GetStatusText());
		}
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left && inputEventMouseButton.Pressed)
		{
			_feature?.RequestSpin();
		}
	}

	public override void _Process(double delta)
	{
		if (!_spinning)
		{
			return;
		}
		_spinTimer -= delta;
		double elapsed = _spinDuration - _spinTimer;
		bool flag = true;
		for (int i = 0; i < _reels.Length; i++)
		{
			ReelView reelView = _reels[i];
			if (reelView != null)
			{
				SpinReel(reelView, elapsed);
				flag &= reelView.Stopped;
			}
		}
		double num = 1.0 - (double)Mathf.Clamp((float)(_spinTimer / _spinDuration), 0f, 1f);
		SetHandleProgress(Mathf.Sin((float)num * (float)Math.PI));
		if ((_spinTimer <= 0.0) | flag)
		{
			_spinning = false;
			SetReelsToFinalOffset();
			SetHandleProgress(0f);
			_audioPlayer?.Stop();
			ScheduleSpinCompletion();
		}
	}

	public void PlaySpin(string[] items, double duration, Action<string[]> spinFinished)
	{
		_finalItems = items;
		_spinFinished = spinFinished;
		_spinDuration = Mathf.Max(0.35f, (float)duration);
		_spinTimer = _spinDuration;
		_spinning = true;
		for (int i = 0; i < _reels.Length; i++)
		{
			ReelView reelView = _reels[i];
			if (reelView != null)
			{
				string[] array = new string[8 + i * 2];
				for (int j = 0; j < array.Length - 1; j++)
				{
					array[j] = GetRandomSpinItem();
				}
				array[^1] = GetFinalItem(i);
				BuildReelStrip(reelView, array);
				reelView.Offset = 0f;
				reelView.TargetOffset = (float)(array.Length - 1) * 82f;
				reelView.StopTime = Math.Max(0.1, GetReelStopTime(i));
				reelView.Stopped = false;
				ApplyReelPosition(reelView);
			}
		}
		_audioPlayer?.Play();
	}

	public void SetStatusText(string text)
	{
		if (_statusLabel != null)
		{
			_statusLabel.Text = text;
		}
	}

	public void SetInteractable(bool interactable)
	{
		if (_spinning)
		{
			Modulate = Colors.White;
			MouseDefaultCursorShape = CursorShape.Arrow;
		}
		else
		{
			Modulate = (interactable ? Colors.White : new Color(0.72f, 0.72f, 0.72f, 0.92f));
			MouseDefaultCursorShape = (CursorShape)(interactable ? 2 : 0);
		}
	}

	private void BindSceneNodes()
	{
		for (int i = 0; i < _reels.Length; i++)
		{
			Control nodeOrNull = GetNodeOrNull<Control>($"%Reel{i + 1}");
			Control nodeOrNull2 = GetNodeOrNull<Control>($"%Reel{i + 1}Track");
			if (!GodotObject.IsInstanceValid(nodeOrNull) || !GodotObject.IsInstanceValid(nodeOrNull2))
			{
				GD.PushWarning($"[SlotMachine] Missing reel scene nodes: Reel{i + 1}");
			}
			else
			{
				_reels[i] = new ReelView
				{
					Clip = nodeOrNull,
					Track = nodeOrNull2,
					Stopped = true
				};
			}
		}
		_statusLabel = GetNodeOrNull<Label>("%StatusLabel");
		_shaftTexture = GetNodeOrNull<TextureRect>("%HandleShaft");
		_ballTexture = GetNodeOrNull<TextureRect>("%HandleBall");
		_audioPlayer = GetNodeOrNull<AudioStreamPlayer>("%SlotMachineAudio");
		SetHandleProgress(0f);
	}

	private void BuildReelStrip(ReelView reel, string[] itemKeys)
	{
		ClearTrack(reel);
		for (int i = 0; i < itemKeys.Length; i++)
		{
			CardFace cardFace = CreateCard(itemKeys[i], new Vector2(0f, (float)(-i) * 82f));
			reel.Track.AddChild(cardFace.Root, forceReadableName: false, InternalMode.Disabled);
			ApplyCard(cardFace);
			if (i == itemKeys.Length - 1)
			{
				reel.FinalCard = cardFace;
			}
		}
	}

	private void ClearTrack(ReelView reel)
	{
		reel.FinalCard = null;
		foreach (Node child in reel.Track.GetChildren())
		{
			reel.Track.RemoveChild(child);
			child.QueueFree();
		}
	}

	private CardFace CreateCard(string itemKey, Vector2 position)
	{
		Control control = new Control
		{
			Position = position,
			Size = new Vector2(49f, 47f),
			MouseFilter = MouseFilterEnum.Ignore
		};
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
		towerDefenseInGamePacketShow.Name = "PacketShow";
		towerDefenseInGamePacketShow.Position = PacketPosition;
		towerDefenseInGamePacketShow.Scale = new Vector2(0.86f, 0.86f);
		towerDefenseInGamePacketShow.MouseFilter = MouseFilterEnum.Ignore;
		control.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
		return new CardFace
		{
			Root = control,
			Packet = towerDefenseInGamePacketShow,
			ItemKey = itemKey,
			DisplayKey = GetDisplayPacketName(itemKey)
		};
	}

	private void ApplyCard(CardFace card)
	{
		if (!GodotObject.IsInstanceValid(card?.Packet))
		{
			return;
		}
		ConfigurePacketShow(card.Packet);
		if (!card.Packet.IsNodeReady())
		{
			card.Packet.Visible = false;
			GD.PushWarning("[SlotMachine] Packet preview did not become ready after entering the reel tree.");
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(card.DisplayKey);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			GD.PushWarning("[SlotMachine] Invalid packet display: " + card.DisplayKey);
			packetConfig = TowerDefenseManager.GetPacketConfig(FallbackSpinItems[0]);
		}
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			card.Packet.Visible = false;
			return;
		}
		card.Packet.Visible = true;
		card.Packet.Init(packetConfig);
		ConfigurePacketShow(card.Packet);
		ShowPacketViewport(card.Packet);
	}

	private void ConfigurePacketShow(TowerDefenseInGamePacketShow packet)
	{
		packet.onlyDraw = true;
		packet.showCost = false;
		packet.showLove = false;
		packet.start = false;
		packet.select = false;
		packet.alive = true;
		packet.@lock = false;
		packet.coldDownOpen = false;
		packet.coldDownTimer = 0.0;
		packet.pressDelayTimer = 0.0;
		packet.MouseFilter = MouseFilterEnum.Ignore;
		packet.Position = PacketPosition;
		packet.Scale = new Vector2(0.86f, 0.86f);
		packet.ClearEventHandlers();
		if (GodotObject.IsInstanceValid(packet.itemCostLabel))
		{
			packet.itemCostLabel.Visible = false;
		}
		if (GodotObject.IsInstanceValid(packet.button))
		{
			packet.button.Visible = false;
			packet.button.MouseFilter = MouseFilterEnum.Ignore;
		}
		if (GodotObject.IsInstanceValid(packet.coldDownProgressBar))
		{
			packet.coldDownProgressBar.Visible = false;
		}
		if (GodotObject.IsInstanceValid(packet.loveButton))
		{
			packet.loveButton.Visible = false;
			packet.loveButton.MouseFilter = MouseFilterEnum.Ignore;
		}
		if (GodotObject.IsInstanceValid(packet.selectTexture))
		{
			packet.selectTexture.Visible = false;
		}
	}

	private void ShowPacketViewport(TowerDefenseInGamePacketShow packet)
	{
		if (GodotObject.IsInstanceValid(packet.previewClip))
		{
			packet.previewClip.Visible = true;
		}
		if (GodotObject.IsInstanceValid(packet.sprite))
		{
			packet.sprite.Visible = true;
			packet.sprite.pause = false;
			packet.sprite.ProcessMode = ProcessModeEnum.Inherit;
		}
	}

	private void SpinReel(ReelView reel, double elapsed)
	{
		if (!reel.Stopped)
		{
			float num = Mathf.Clamp((float)(elapsed / reel.StopTime), 0f, 1f);
			float num2 = 1f - Mathf.Pow(1f - num, 3f);
			reel.Offset = reel.TargetOffset * num2;
			if (num >= 1f)
			{
				reel.Offset = reel.TargetOffset;
				reel.Stopped = true;
			}
			ApplyReelPosition(reel);
		}
	}

	private void ApplyReelPosition(ReelView reel)
	{
		reel.Track.Position = new Vector2(0f, reel.Offset);
	}

	private double GetReelStopTime(int index)
	{
		double num = Math.Min(0.55, Math.Max(0.18, _spinDuration * 0.5));
		double num2 = num / (double)_reels.Length;
		return _spinDuration - num + num2 * ((double)index + 0.65);
	}

	private void SetFinalItems()
	{
		for (int i = 0; i < _reels.Length; i++)
		{
			SetReelItem(i, GetFinalItem(i));
		}
	}

	private void SetReelsToFinalOffset()
	{
		for (int i = 0; i < _reels.Length; i++)
		{
			ReelView reelView = _reels[i];
			if (reelView != null)
			{
				reelView.Offset = reelView.TargetOffset;
				reelView.Stopped = true;
				ApplyReelPosition(reelView);
			}
		}
	}

	private void ScheduleSpinCompletion()
	{
		if (_spinFinished != null && _spinSettleHandler == null && IsInsideTree())
		{
			_spinSettleTree = GetTree();
			if (GodotObject.IsInstanceValid(_spinSettleTree))
			{
				_spinSettleHandler = CompleteSpinOnNextFrame;
				_spinSettleTree.ProcessFrame += _spinSettleHandler;
			}
		}
	}

	private void CompleteSpinOnNextFrame()
	{
		CancelSpinSettleCallback();
		Action<string[]> spinFinished = _spinFinished;
		_spinFinished = null;
		if (IsInsideTree())
		{
			spinFinished?.Invoke(GetCurrentItems());
		}
	}

	private void CancelSpinSettleCallback()
	{
		if (GodotObject.IsInstanceValid(_spinSettleTree) && _spinSettleHandler != null)
		{
			_spinSettleTree.ProcessFrame -= _spinSettleHandler;
		}
		_spinSettleTree = null;
		_spinSettleHandler = null;
	}

	public void CancelPendingWork()
	{
		CancelSpinSettleCallback();
		_spinFinished = null;
		_spinning = false;
		_spinTimer = 0.0;
		_audioPlayer?.Stop();
		_feature = null;
	}

	public override void _ExitTree()
	{
		CancelPendingWork();
		base._ExitTree();
	}

	private string[] GetCurrentItems()
	{
		string[] array = new string[_reels.Length];
		for (int i = 0; i < _reels.Length; i++)
		{
			array[i] = (_reels[i]?.FinalCard)?.ItemKey ?? GetFinalItem(i);
		}
		return array;
	}

	private void SetReelItem(int index, string itemKey)
	{
		ReelView reelView = _reels[index];
		if (reelView != null)
		{
			BuildReelStrip(reelView, new string[1] { itemKey });
			reelView.Offset = 0f;
			reelView.TargetOffset = 0f;
			reelView.StopTime = 0.1;
			reelView.Stopped = true;
			ApplyReelPosition(reelView);
		}
	}

	private string GetFinalItem(int index)
	{
		if (index >= 0 && index < _finalItems.Length)
		{
			return _finalItems[index];
		}
		return FallbackSpinItems[0];
	}

	private string GetRandomSpinItem()
	{
		return _spinItems[GD.RandRange(0, _spinItems.Length - 1)];
	}

	private static string GetDisplayPacketName(string itemKey)
	{
		string text = (itemKey ?? "").Trim();
		int num = text.LastIndexOf('@');
		if (num >= 0)
		{
			text = text.Substring(0, num).Trim();
		}
		if (text.StartsWith("SUN:", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "SUN", StringComparison.OrdinalIgnoreCase))
		{
			return "PlantSunFlower";
		}
		if (text.StartsWith("BRAIN:", StringComparison.OrdinalIgnoreCase) || text.StartsWith("BRAINSUN:", StringComparison.OrdinalIgnoreCase) || text.StartsWith("SUNBRAIN:", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "BRAIN", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "BRAINSUN", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "SUNBRAIN", StringComparison.OrdinalIgnoreCase))
		{
			return "ItemBrain";
		}
		if (text.StartsWith("DIAMOND:", StringComparison.OrdinalIgnoreCase) || text.StartsWith("COIN:", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "DIAMOND", StringComparison.OrdinalIgnoreCase) || string.Equals(text, "COIN", StringComparison.OrdinalIgnoreCase))
		{
			return "PlantDiamondseed";
		}
		return itemKey;
	}

	private void SetHandleProgress(float progress)
	{
		float num = Mathf.Lerp(-50f, 35f, Mathf.Clamp(progress, 0f, 1f));
		float s = Mathf.DegToRad(num);
		Vector2 vector = new Vector2(Mathf.Cos(s), Mathf.Sin(s));
		if (_shaftTexture != null)
		{
			_shaftTexture.Position = HandlePivot - HandleShaftPivot;
			_shaftTexture.RotationDegrees = num;
		}
		if (_ballTexture != null)
		{
			Vector2 vector2 = HandlePivot + vector * 56f;
			_ballTexture.Position = vector2 - HandleBallSize * 0.5f;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "feature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStatusText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetInteractable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "interactable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindSceneNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigurePacketShow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPacketViewport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetReelStopTime, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFinalItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetReelsToFinalOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleSpinCompletion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteSpinOnNextFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelSpinSettleCallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelPendingWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentItems, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetReelItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "itemKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFinalItem, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRandomSpinItem, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetDisplayPacketName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "itemKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHandleProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "progress", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseBattleFeatureSlotMachine>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._GuiInput && args.Count == 1)
		{
			_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetStatusText && args.Count == 1)
		{
			SetStatusText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetInteractable && args.Count == 1)
		{
			SetInteractable(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSceneNodes && args.Count == 0)
		{
			BindSceneNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePacketShow && args.Count == 1)
		{
			ConfigurePacketShow(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPacketViewport && args.Count == 1)
		{
			ShowPacketViewport(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetReelStopTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetReelStopTime(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFinalItems && args.Count == 0)
		{
			SetFinalItems();
			ret = default;
			return true;
		}
		if (method == MethodName.SetReelsToFinalOffset && args.Count == 0)
		{
			SetReelsToFinalOffset();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleSpinCompletion && args.Count == 0)
		{
			ScheduleSpinCompletion();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteSpinOnNextFrame && args.Count == 0)
		{
			CompleteSpinOnNextFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelSpinSettleCallback && args.Count == 0)
		{
			CancelSpinSettleCallback();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingWork && args.Count == 0)
		{
			CancelPendingWork();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentItems && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetCurrentItems());
			return true;
		}
		if (method == MethodName.SetReelItem && args.Count == 2)
		{
			SetReelItem(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFinalItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFinalItem(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRandomSpinItem && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetRandomSpinItem());
			return true;
		}
		if (method == MethodName.GetDisplayPacketName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDisplayPacketName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetHandleProgress && args.Count == 1)
		{
			SetHandleProgress(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetDisplayPacketName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDisplayPacketName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._GuiInput)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.SetStatusText)
		{
			return true;
		}
		if (method == MethodName.SetInteractable)
		{
			return true;
		}
		if (method == MethodName.BindSceneNodes)
		{
			return true;
		}
		if (method == MethodName.ConfigurePacketShow)
		{
			return true;
		}
		if (method == MethodName.ShowPacketViewport)
		{
			return true;
		}
		if (method == MethodName.GetReelStopTime)
		{
			return true;
		}
		if (method == MethodName.SetFinalItems)
		{
			return true;
		}
		if (method == MethodName.SetReelsToFinalOffset)
		{
			return true;
		}
		if (method == MethodName.ScheduleSpinCompletion)
		{
			return true;
		}
		if (method == MethodName.CompleteSpinOnNextFrame)
		{
			return true;
		}
		if (method == MethodName.CancelSpinSettleCallback)
		{
			return true;
		}
		if (method == MethodName.CancelPendingWork)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.GetCurrentItems)
		{
			return true;
		}
		if (method == MethodName.SetReelItem)
		{
			return true;
		}
		if (method == MethodName.GetFinalItem)
		{
			return true;
		}
		if (method == MethodName.GetRandomSpinItem)
		{
			return true;
		}
		if (method == MethodName.GetDisplayPacketName)
		{
			return true;
		}
		if (method == MethodName.SetHandleProgress)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._feature)
		{
			_feature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureSlotMachine>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._shaftTexture)
		{
			_shaftTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._ballTexture)
		{
			_ballTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._audioPlayer)
		{
			_audioPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._spinItems)
		{
			_spinItems = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._finalItems)
		{
			_finalItems = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._spinTimer)
		{
			_spinTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spinDuration)
		{
			_spinDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spinning)
		{
			_spinning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spinSettleTree)
		{
			_spinSettleTree = VariantUtils.ConvertTo<SceneTree>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._feature)
		{
			value = VariantUtils.CreateFrom(in _feature);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._shaftTexture)
		{
			value = VariantUtils.CreateFrom(in _shaftTexture);
			return true;
		}
		if (name == PropertyName._ballTexture)
		{
			value = VariantUtils.CreateFrom(in _ballTexture);
			return true;
		}
		if (name == PropertyName._audioPlayer)
		{
			value = VariantUtils.CreateFrom(in _audioPlayer);
			return true;
		}
		if (name == PropertyName._spinItems)
		{
			value = VariantUtils.CreateFrom(in _spinItems);
			return true;
		}
		if (name == PropertyName._finalItems)
		{
			value = VariantUtils.CreateFrom(in _finalItems);
			return true;
		}
		if (name == PropertyName._spinTimer)
		{
			value = VariantUtils.CreateFrom(in _spinTimer);
			return true;
		}
		if (name == PropertyName._spinDuration)
		{
			value = VariantUtils.CreateFrom(in _spinDuration);
			return true;
		}
		if (name == PropertyName._spinning)
		{
			value = VariantUtils.CreateFrom(in _spinning);
			return true;
		}
		if (name == PropertyName._spinSettleTree)
		{
			value = VariantUtils.CreateFrom(in _spinSettleTree);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._feature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shaftTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ballTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._spinItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._finalItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spinTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spinDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spinning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spinSettleTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._feature, Variant.From(in _feature));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._shaftTexture, Variant.From(in _shaftTexture));
		info.AddProperty(PropertyName._ballTexture, Variant.From(in _ballTexture));
		info.AddProperty(PropertyName._audioPlayer, Variant.From(in _audioPlayer));
		info.AddProperty(PropertyName._spinItems, Variant.From(in _spinItems));
		info.AddProperty(PropertyName._finalItems, Variant.From(in _finalItems));
		info.AddProperty(PropertyName._spinTimer, Variant.From(in _spinTimer));
		info.AddProperty(PropertyName._spinDuration, Variant.From(in _spinDuration));
		info.AddProperty(PropertyName._spinning, Variant.From(in _spinning));
		info.AddProperty(PropertyName._spinSettleTree, Variant.From(in _spinSettleTree));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._feature, out var value))
		{
			_feature = value.As<TowerDefenseBattleFeatureSlotMachine>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value2))
		{
			_statusLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._shaftTexture, out var value3))
		{
			_shaftTexture = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._ballTexture, out var value4))
		{
			_ballTexture = value4.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._audioPlayer, out var value5))
		{
			_audioPlayer = value5.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._spinItems, out var value6))
		{
			_spinItems = value6.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._finalItems, out var value7))
		{
			_finalItems = value7.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._spinTimer, out var value8))
		{
			_spinTimer = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spinDuration, out var value9))
		{
			_spinDuration = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spinning, out var value10))
		{
			_spinning = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spinSettleTree, out var value11))
		{
			_spinSettleTree = value11.As<SceneTree>();
		}
	}
}
