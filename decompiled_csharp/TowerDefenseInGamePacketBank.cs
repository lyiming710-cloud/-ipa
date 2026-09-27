using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/PacketBank/TowerDefenseInGamePacketBank.cs")]
public class TowerDefenseInGamePacketBank : Control
{
	private sealed class PendingPacketAnimation
	{
		public TowerDefenseInGamePacketShow AnimationPacket;

		public TowerDefenseInGamePacketShow TargetPacket;

		public Tween Tween;
	}

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FinalizeReady = "FinalizeReady";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetMobileMode = "SetMobileMode";

		public static readonly StringName ApplyDisplayPosition = "ApplyDisplayPosition";

		public static readonly StringName ApplyMode = "ApplyMode";

		public static readonly StringName GetCameraPos = "GetCameraPos";

		public static readonly StringName CreateAnime = "CreateAnime";

		public static readonly StringName ClearAnimeNode = "ClearAnimeNode";

		public static readonly StringName CancelPacketAnimation = "CancelPacketAnimation";

		public static readonly StringName ClearPackets = "ClearPackets";

		public static readonly StringName ReturnPacketToPool = "ReturnPacketToPool";

		public static readonly StringName DisposeOwnedPackets = "DisposeOwnedPackets";

		public static readonly StringName GetPacketFromPool = "GetPacketFromPool";

		public static readonly StringName UpdateVirtualizedContentSize = "UpdateVirtualizedContentSize";

		public static readonly StringName OnPacketBankScrolled = "OnPacketBankScrolled";

		public static readonly StringName QueueVirtualizedPacketRefresh = "QueueVirtualizedPacketRefresh";

		public static readonly StringName RefreshVirtualizedPacketBindings = "RefreshVirtualizedPacketBindings";

		public static readonly StringName PositionVirtualizedPacket = "PositionVirtualizedPacket";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName LogicalPacketCount = "LogicalPacketCount";

		public static readonly StringName VisiblePacketCount = "VisiblePacketCount";

		public static readonly StringName VirtualColumnCount = "VirtualColumnCount";

		public static readonly StringName VirtualCellStride = "VirtualCellStride";

		public static readonly StringName VirtualCardFootprint = "VirtualCardFootprint";

		public static readonly StringName translate = "translate";

		public static readonly StringName packetBankPanelTexture = "packetBankPanelTexture";

		public static readonly StringName packetBankScroll = "packetBankScroll";

		public static readonly StringName packetBankMargin = "packetBankMargin";

		public static readonly StringName packetContainer = "packetContainer";

		public static readonly StringName animeNode = "animeNode";

		public static readonly StringName cardSort = "cardSort";

		public static readonly StringName packetBankAnimationPlayer = "packetBankAnimationPlayer";

		public static readonly StringName cardZombie = "cardZombie";

		public static readonly StringName cardItem = "cardItem";

		public static readonly StringName cardGraveStone = "cardGraveStone";

		public static readonly StringName seedBank = "seedBank";

		public static readonly StringName mobilePreset = "mobilePreset";

		public static readonly StringName _packetPool = "_packetPool";

		public static readonly StringName _virtualPacketRefreshQueued = "_virtualPacketRefreshQueued";

		public static readonly StringName packetBankFeature = "packetBankFeature";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static Texture2D _packetBankZombiePanel;

	public Control translate;

	public NinePatchRect packetBankPanelTexture;

	public ScrollContainer packetBankScroll;

	public MarginContainer packetBankMargin;

	public Control packetContainer;

	public Control animeNode;

	public TextureRect cardSort;

	public AnimationPlayer packetBankAnimationPlayer;

	public Control cardZombie;

	public Control cardItem;

	public Control cardGraveStone;

	public TowerDefenseInGameSeedBank seedBank;

	public bool mobilePreset;

	private Array<TowerDefenseInGamePacketShow> _packetPool = new Array<TowerDefenseInGamePacketShow>();

	private readonly List<PendingPacketAnimation> _pendingPacketAnimations = new List<PendingPacketAnimation>();

	private readonly List<TowerDefensePacketConfig> _virtualPacketConfigs = new List<TowerDefensePacketConfig>();

	private readonly System.Collections.Generic.Dictionary<int, TowerDefenseInGamePacketShow> _visiblePackets = new System.Collections.Generic.Dictionary<int, TowerDefenseInGamePacketShow>();

	private bool _virtualPacketRefreshQueued;

	public TowerDefenseBattleFeaturePacketBank packetBankFeature;

	private static Texture2D PACKET_BANK_ZOMBIE_PANEL => _packetBankZombiePanel ?? (_packetBankZombiePanel = GD.Load<Texture2D>("uid://cpd1x4qxqksuh"));

	public int LogicalPacketCount => _virtualPacketConfigs.Count;

	public int VisiblePacketCount => _visiblePackets.Count;

	private int VirtualColumnCount
	{
		get
		{
			if (!mobilePreset)
			{
				return 11;
			}
			return 6;
		}
	}

	private Vector2 VirtualCellStride
	{
		get
		{
			if (!mobilePreset)
			{
				return new Vector2(52f, 70f);
			}
			return new Vector2(98f, 62f);
		}
	}

	private Vector2 VirtualCardFootprint
	{
		get
		{
			if (!mobilePreset)
			{
				return new Vector2(50f, 70f);
			}
			return new Vector2(96f, 60f);
		}
	}

	public override void _Ready()
	{
		translate = GetNode<Control>("%Translate");
		packetBankPanelTexture = GetNode<NinePatchRect>("%PacketBankPanelTexture");
		packetBankScroll = GetNode<ScrollContainer>("%PacketBankScroll");
		packetBankMargin = GetNode<MarginContainer>("%PacketBankMargin");
		packetContainer = GetNode<Control>("%PacketContainer");
		animeNode = GetNode<Control>("%AnimeNode");
		cardSort = GetNode<TextureRect>("%CardSort");
		packetBankAnimationPlayer = GetNode<AnimationPlayer>("%PacketBankAnimationPlayer");
		cardZombie = GetNode<Control>("%CardZombie");
		cardItem = GetNode<Control>("%CardItem");
		cardGraveStone = GetNode<Control>("%CardGraveStone");
		packetBankScroll.GetVScrollBar().ValueChanged += OnPacketBankScrolled;
		VisibilityChanged += QueueVirtualizedPacketRefresh;
		BattleEventBus.Instance.OnUiSwitched += SetMobileMode;
		mobilePreset = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		ApplyMode();
		if (mobilePreset)
		{
			translate.Position = new Vector2(210f, 600f);
		}
		else
		{
			translate.Position = new Vector2(0f, 600f);
		}
		bool visible = CommandManager.Instance.debugPacketOpenAll || Global.Instance.enterLevelMode == "OnlineLevel" || Global.Instance.enterLevelMode == "LoadLevel" || Global.Instance.enterLevelMode == "DiyLevel";
		cardZombie.Visible = visible;
		cardItem.Visible = visible;
		cardGraveStone.Visible = visible;
		Callable.From(FinalizeReady).CallDeferred();
	}

	private void FinalizeReady()
	{
		if (IsInsideTree() && GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode()))
		{
			cardSort.Visible = false;
			packetBankPanelTexture.Texture = PACKET_BANK_ZOMBIE_PANEL;
		}
	}

	public override void _ExitTree()
	{
		_virtualPacketRefreshQueued = false;
		base._ExitTree();
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched -= SetMobileMode;
		}
	}

	public void SetMobileMode(bool enabled)
	{
		if (mobilePreset == enabled)
		{
			return;
		}
		bool isHidden = !Visible || translate.Position.Y >= 500f;
		if (GodotObject.IsInstanceValid(packetBankAnimationPlayer))
		{
			packetBankAnimationPlayer.Stop();
		}
		mobilePreset = enabled;
		ApplyMode();
		ApplyDisplayPosition(isHidden);
		foreach (TowerDefenseInGamePacketShow value in _visiblePackets.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.SetMobileMode(mobilePreset);
			}
		}
		RefreshVirtualizedPacketBindings();
	}

	private void ApplyDisplayPosition(bool isHidden = false)
	{
		if (mobilePreset)
		{
			if (isHidden)
			{
				translate.Position = new Vector2(210f, 600f);
			}
			else
			{
				translate.Position = new Vector2(210f, 72f);
			}
		}
		else if (isHidden)
		{
			translate.Position = new Vector2(0f, 600f);
		}
		else
		{
			translate.Position = new Vector2(0f, 86f);
		}
	}

	private void ApplyMode()
	{
		if (mobilePreset)
		{
			packetBankScroll.Size = new Vector2(590f, 422f);
			packetBankScroll.Position = new Vector2(10f, 34f);
			packetBankMargin.AddThemeConstantOverride("margin_left", 48);
			packetBankMargin.AddThemeConstantOverride("margin_top", 30);
			packetBankMargin.AddThemeConstantOverride("margin_right", 48);
			packetBankMargin.AddThemeConstantOverride("margin_bottom", 30);
		}
		else
		{
			packetBankScroll.Position = new Vector2(17f, 34f);
			packetBankScroll.Size = new Vector2(580f, 422f);
			packetBankMargin.AddThemeConstantOverride("margin_left", 25);
			packetBankMargin.AddThemeConstantOverride("margin_top", 32);
			packetBankMargin.AddThemeConstantOverride("margin_right", 25);
			packetBankMargin.AddThemeConstantOverride("margin_bottom", 38);
		}
		UpdateVirtualizedContentSize();
	}

	public Vector2 GetCameraPos()
	{
		return GetViewport().GetCamera2D().GlobalPosition;
	}

	public void CreateAnime(TowerDefensePacketConfig config, Vector2 pos)
	{
		if (IsInsideTree() && GodotObject.IsInstanceValid(config) && GodotObject.IsInstanceValid(seedBank) && GodotObject.IsInstanceValid(animeNode))
		{
			Vector2 globalPosition = Global.Instance.GetViewport().GetCamera2D().GlobalPosition;
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			animeNode.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGamePacketShow.Init(config);
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.GlobalPosition = pos - globalPosition;
			Vector2 packetPos = seedBank.GetPacketPos(seedBank.packetNum);
			TowerDefensePacketConfig packetConfig = config.Duplicate(deep: true) as TowerDefensePacketConfig;
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow2 = seedBank.AddPacket(packetConfig, TowerDefenseManager.Instance.IsGameRunning());
			if (towerDefenseInGamePacketShow2 != null)
			{
				towerDefenseInGamePacketShow2.Visible = false;
			}
			PendingPacketAnimation pending = new PendingPacketAnimation
			{
				AnimationPacket = towerDefenseInGamePacketShow,
				TargetPacket = towerDefenseInGamePacketShow2
			};
			Tween tween = towerDefenseInGamePacketShow.CreateTween();
			pending.Tween = tween;
			_pendingPacketAnimations.Add(pending);
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Quart);
			tween.TweenProperty(towerDefenseInGamePacketShow, "global_position", packetPos, 0.5);
			tween.TweenCallback(Callable.From(() =>
			{
				CompletePacketAnimation(pending, cancelled: false);
			}));
		}
	}

	public void ClearAnimeNode()
	{
		PendingPacketAnimation[] array = _pendingPacketAnimations.ToArray();
		foreach (PendingPacketAnimation pending in array)
		{
			CompletePacketAnimation(pending, cancelled: true);
		}
		foreach (Node child in animeNode.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void CancelPacketAnimation(TowerDefenseInGamePacketShow targetPacket)
	{
		PendingPacketAnimation[] array = _pendingPacketAnimations.ToArray();
		foreach (PendingPacketAnimation pendingPacketAnimation in array)
		{
			if (pendingPacketAnimation.TargetPacket == targetPacket)
			{
				CompletePacketAnimation(pendingPacketAnimation, cancelled: true);
				break;
			}
		}
	}

	private void CompletePacketAnimation(PendingPacketAnimation pending, bool cancelled)
	{
		if (pending != null && _pendingPacketAnimations.Remove(pending))
		{
			if (cancelled && GodotObject.IsInstanceValid(pending.Tween))
			{
				pending.Tween.Kill();
			}
			if (GodotObject.IsInstanceValid(pending.TargetPacket))
			{
				pending.TargetPacket.Visible = true;
			}
			if (GodotObject.IsInstanceValid(pending.AnimationPacket))
			{
				pending.AnimationPacket.QueueFree();
			}
		}
	}

	public void ClearPackets()
	{
		_virtualPacketRefreshQueued = false;
		_virtualPacketConfigs.Clear();
		foreach (TowerDefenseInGamePacketShow value in _visiblePackets.Values)
		{
			ReturnPacketToPool(value);
		}
		_visiblePackets.Clear();
		UpdateVirtualizedContentSize();
	}

	private void ReturnPacketToPool(TowerDefenseInGamePacketShow packet)
	{
		if (GodotObject.IsInstanceValid(packet))
		{
			if (GodotObject.IsInstanceValid(packetBankFeature))
			{
				packetBankFeature.UnbindVirtualizedPacket(packet);
			}
			packet.ResetForPool();
			if (packet.GetParent() == packetContainer)
			{
				packetContainer.RemoveChild(packet);
			}
			int num = Math.Max(0, packetBankFeature?.config?.maxPoolSize ?? 96);
			if (_packetPool.Count < num)
			{
				_packetPool.Add(packet);
			}
			else
			{
				packet.QueueFree();
			}
		}
	}

	public void DisposeOwnedPackets()
	{
		if (GodotObject.IsInstanceValid(packetContainer))
		{
			ClearPackets();
		}
		if (GodotObject.IsInstanceValid(animeNode))
		{
			ClearAnimeNode();
		}
		foreach (TowerDefenseInGamePacketShow item in _packetPool)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.QueueFree();
			}
		}
		_packetPool.Clear();
		seedBank = null;
		packetBankFeature = null;
	}

	public TowerDefenseInGamePacketShow GetPacketFromPool()
	{
		if (_packetPool.Count > 0)
		{
			TowerDefenseInGamePacketShow result = _packetPool[_packetPool.Count - 1];
			_packetPool.RemoveAt(_packetPool.Count - 1);
			return result;
		}
		return TowerDefenseManager.CreatePacketShow();
	}

	public void SetVirtualizedPackets(IReadOnlyList<TowerDefensePacketConfig> configs)
	{
		ClearPackets();
		if (configs != null)
		{
			for (int i = 0; i < configs.Count; i++)
			{
				TowerDefensePacketConfig towerDefensePacketConfig = configs[i];
				if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
				{
					_virtualPacketConfigs.Add(towerDefensePacketConfig);
				}
			}
		}
		packetBankScroll.ScrollVertical = 0;
		UpdateVirtualizedContentSize();
		RefreshVirtualizedPacketBindings();
	}

	private void UpdateVirtualizedContentSize()
	{
		if (GodotObject.IsInstanceValid(packetContainer))
		{
			int virtualColumnCount = VirtualColumnCount;
			int num = ((_virtualPacketConfigs.Count != 0) ? Mathf.CeilToInt((float)_virtualPacketConfigs.Count / (float)virtualColumnCount) : 0);
			Vector2 virtualCellStride = VirtualCellStride;
			packetContainer.CustomMinimumSize = ((num == 0) ? Vector2.Zero : new Vector2((float)(virtualColumnCount - 1) * virtualCellStride.X, (float)(num - 1) * virtualCellStride.Y));
		}
	}

	private void OnPacketBankScrolled(double value)
	{
		QueueVirtualizedPacketRefresh();
	}

	public void QueueVirtualizedPacketRefresh()
	{
		if (!_virtualPacketRefreshQueued && IsInsideTree())
		{
			_virtualPacketRefreshQueued = true;
			CallDeferred("RefreshVirtualizedPacketBindings");
		}
	}

	public void RefreshVirtualizedPacketBindings()
	{
		_virtualPacketRefreshQueued = false;
		if (!IsInsideTree() || !GodotObject.IsInstanceValid(packetBankScroll) || !GodotObject.IsInstanceValid(packetContainer) || _virtualPacketConfigs.Count == 0)
		{
			return;
		}
		int virtualColumnCount = VirtualColumnCount;
		float y = VirtualCellStride.Y;
		float num = VirtualCardFootprint.Y / 2f;
		float num2 = packetBankMargin.GetThemeConstant("margin_top");
		float num3 = Math.Max(0f, packetBankScroll.ScrollVertical);
		float num4 = num3 + packetBankScroll.Size.Y;
		int num5 = Math.Max(0, Mathf.CeilToInt((num3 - num2 - num) / y));
		int num6 = Math.Max(num5, Mathf.FloorToInt((num4 - num2 + num) / y));
		int num7 = num5 * virtualColumnCount;
		int num8 = Math.Min(_virtualPacketConfigs.Count - 1, (num6 + 1) * virtualColumnCount - 1);
		List<TowerDefenseInGamePacketShow> list = new List<TowerDefenseInGamePacketShow>();
		List<int> list2 = new List<int>();
		foreach (var (num10, towerDefenseInGamePacketShow2) in _visiblePackets)
		{
			if (num10 < num7 || num10 > num8)
			{
				list2.Add(num10);
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow2))
				{
					list.Add(towerDefenseInGamePacketShow2);
				}
			}
		}
		for (int i = 0; i < list2.Count; i++)
		{
			_visiblePackets.Remove(list2[i]);
		}
		int j = 0;
		for (int k = num7; k <= num8; k++)
		{
			if (_visiblePackets.TryGetValue(k, out var value) && GodotObject.IsInstanceValid(value))
			{
				PositionVirtualizedPacket(value, k);
				continue;
			}
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow3;
			if (j < list.Count)
			{
				towerDefenseInGamePacketShow3 = list[j++];
				if (GodotObject.IsInstanceValid(packetBankFeature))
				{
					packetBankFeature.UnbindVirtualizedPacket(towerDefenseInGamePacketShow3);
				}
				towerDefenseInGamePacketShow3.ResetForPool();
			}
			else
			{
				towerDefenseInGamePacketShow3 = GetPacketFromPool();
				if (towerDefenseInGamePacketShow3.GetParent() != packetContainer)
				{
					packetContainer.AddChild(towerDefenseInGamePacketShow3, forceReadableName: false, InternalMode.Disabled);
				}
			}
			towerDefenseInGamePacketShow3.SetPreviewCreationDeferred(deferred: false);
			towerDefenseInGamePacketShow3.SetMobileMode(mobilePreset);
			towerDefenseInGamePacketShow3.Visible = true;
			PositionVirtualizedPacket(towerDefenseInGamePacketShow3, k);
			if (GodotObject.IsInstanceValid(packetBankFeature))
			{
				packetBankFeature.BindVirtualizedPacket(towerDefenseInGamePacketShow3, _virtualPacketConfigs[k]);
			}
			else
			{
				towerDefenseInGamePacketShow3.Init(_virtualPacketConfigs[k]);
			}
			_visiblePackets[k] = towerDefenseInGamePacketShow3;
		}
		for (; j < list.Count; j++)
		{
			ReturnPacketToPool(list[j]);
		}
	}

	private void PositionVirtualizedPacket(TowerDefenseInGamePacketShow packet, int itemIndex)
	{
		int virtualColumnCount = VirtualColumnCount;
		int num = itemIndex % virtualColumnCount;
		int num2 = itemIndex / virtualColumnCount;
		Vector2 virtualCellStride = VirtualCellStride;
		packet.Position = new Vector2((float)num * virtualCellStride.X, (float)num2 * virtualCellStride.Y);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetMobileMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyDisplayPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isHidden", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCameraPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateAnime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearAnimeNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelPacketAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetPacket", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPackets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReturnPacketToPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeOwnedPackets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPacketFromPool, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateVirtualizedContentSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPacketBankScrolled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueVirtualizedPacketRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshVirtualizedPacketBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PositionVirtualizedPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "itemIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FinalizeReady && args.Count == 0)
		{
			FinalizeReady();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.SetMobileMode && args.Count == 1)
		{
			SetMobileMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDisplayPosition && args.Count == 1)
		{
			ApplyDisplayPosition(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMode && args.Count == 0)
		{
			ApplyMode();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCameraPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetCameraPos());
			return true;
		}
		if (method == MethodName.CreateAnime && args.Count == 2)
		{
			CreateAnime(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearAnimeNode && args.Count == 0)
		{
			ClearAnimeNode();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPacketAnimation && args.Count == 1)
		{
			CancelPacketAnimation(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPackets && args.Count == 0)
		{
			ClearPackets();
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnPacketToPool && args.Count == 1)
		{
			ReturnPacketToPool(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeOwnedPackets && args.Count == 0)
		{
			DisposeOwnedPackets();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPacketFromPool && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(GetPacketFromPool());
			return true;
		}
		if (method == MethodName.UpdateVirtualizedContentSize && args.Count == 0)
		{
			UpdateVirtualizedContentSize();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketBankScrolled && args.Count == 1)
		{
			OnPacketBankScrolled(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueVirtualizedPacketRefresh && args.Count == 0)
		{
			QueueVirtualizedPacketRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVirtualizedPacketBindings && args.Count == 0)
		{
			RefreshVirtualizedPacketBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.PositionVirtualizedPacket && args.Count == 2)
		{
			PositionVirtualizedPacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
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
		if (method == MethodName.FinalizeReady)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.SetMobileMode)
		{
			return true;
		}
		if (method == MethodName.ApplyDisplayPosition)
		{
			return true;
		}
		if (method == MethodName.ApplyMode)
		{
			return true;
		}
		if (method == MethodName.GetCameraPos)
		{
			return true;
		}
		if (method == MethodName.CreateAnime)
		{
			return true;
		}
		if (method == MethodName.ClearAnimeNode)
		{
			return true;
		}
		if (method == MethodName.CancelPacketAnimation)
		{
			return true;
		}
		if (method == MethodName.ClearPackets)
		{
			return true;
		}
		if (method == MethodName.ReturnPacketToPool)
		{
			return true;
		}
		if (method == MethodName.DisposeOwnedPackets)
		{
			return true;
		}
		if (method == MethodName.GetPacketFromPool)
		{
			return true;
		}
		if (method == MethodName.UpdateVirtualizedContentSize)
		{
			return true;
		}
		if (method == MethodName.OnPacketBankScrolled)
		{
			return true;
		}
		if (method == MethodName.QueueVirtualizedPacketRefresh)
		{
			return true;
		}
		if (method == MethodName.RefreshVirtualizedPacketBindings)
		{
			return true;
		}
		if (method == MethodName.PositionVirtualizedPacket)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.translate)
		{
			translate = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.packetBankPanelTexture)
		{
			packetBankPanelTexture = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.packetBankScroll)
		{
			packetBankScroll = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName.packetBankMargin)
		{
			packetBankMargin = VariantUtils.ConvertTo<MarginContainer>(in value);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			packetContainer = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.animeNode)
		{
			animeNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.cardSort)
		{
			cardSort = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.packetBankAnimationPlayer)
		{
			packetBankAnimationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName.cardZombie)
		{
			cardZombie = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.cardItem)
		{
			cardItem = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.cardGraveStone)
		{
			cardGraveStone = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.seedBank)
		{
			seedBank = VariantUtils.ConvertTo<TowerDefenseInGameSeedBank>(in value);
			return true;
		}
		if (name == PropertyName.mobilePreset)
		{
			mobilePreset = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._packetPool)
		{
			_packetPool = VariantUtils.ConvertToArray<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._virtualPacketRefreshQueued)
		{
			_virtualPacketRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetBankFeature)
		{
			packetBankFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeaturePacketBank>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.LogicalPacketCount)
		{
			from = LogicalPacketCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisiblePacketCount)
		{
			from = VisiblePacketCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VirtualColumnCount)
		{
			from = VirtualColumnCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.VirtualCellStride)
		{
			from2 = VirtualCellStride;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.VirtualCardFootprint)
		{
			from2 = VirtualCardFootprint;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.translate)
		{
			value = VariantUtils.CreateFrom(in translate);
			return true;
		}
		if (name == PropertyName.packetBankPanelTexture)
		{
			value = VariantUtils.CreateFrom(in packetBankPanelTexture);
			return true;
		}
		if (name == PropertyName.packetBankScroll)
		{
			value = VariantUtils.CreateFrom(in packetBankScroll);
			return true;
		}
		if (name == PropertyName.packetBankMargin)
		{
			value = VariantUtils.CreateFrom(in packetBankMargin);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			value = VariantUtils.CreateFrom(in packetContainer);
			return true;
		}
		if (name == PropertyName.animeNode)
		{
			value = VariantUtils.CreateFrom(in animeNode);
			return true;
		}
		if (name == PropertyName.cardSort)
		{
			value = VariantUtils.CreateFrom(in cardSort);
			return true;
		}
		if (name == PropertyName.packetBankAnimationPlayer)
		{
			value = VariantUtils.CreateFrom(in packetBankAnimationPlayer);
			return true;
		}
		if (name == PropertyName.cardZombie)
		{
			value = VariantUtils.CreateFrom(in cardZombie);
			return true;
		}
		if (name == PropertyName.cardItem)
		{
			value = VariantUtils.CreateFrom(in cardItem);
			return true;
		}
		if (name == PropertyName.cardGraveStone)
		{
			value = VariantUtils.CreateFrom(in cardGraveStone);
			return true;
		}
		if (name == PropertyName.seedBank)
		{
			value = VariantUtils.CreateFrom(in seedBank);
			return true;
		}
		if (name == PropertyName.mobilePreset)
		{
			value = VariantUtils.CreateFrom(in mobilePreset);
			return true;
		}
		if (name == PropertyName._packetPool)
		{
			value = VariantUtils.CreateFromArray(_packetPool);
			return true;
		}
		if (name == PropertyName._virtualPacketRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _virtualPacketRefreshQueued);
			return true;
		}
		if (name == PropertyName.packetBankFeature)
		{
			value = VariantUtils.CreateFrom(in packetBankFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.translate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankPanelTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankScroll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankMargin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.animeNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cardSort, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankAnimationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cardZombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cardItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cardGraveStone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.seedBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mobilePreset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._packetPool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._virtualPacketRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetBankFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LogicalPacketCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisiblePacketCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VirtualColumnCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.VirtualCellStride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.VirtualCardFootprint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.translate, Variant.From(in translate));
		info.AddProperty(PropertyName.packetBankPanelTexture, Variant.From(in packetBankPanelTexture));
		info.AddProperty(PropertyName.packetBankScroll, Variant.From(in packetBankScroll));
		info.AddProperty(PropertyName.packetBankMargin, Variant.From(in packetBankMargin));
		info.AddProperty(PropertyName.packetContainer, Variant.From(in packetContainer));
		info.AddProperty(PropertyName.animeNode, Variant.From(in animeNode));
		info.AddProperty(PropertyName.cardSort, Variant.From(in cardSort));
		info.AddProperty(PropertyName.packetBankAnimationPlayer, Variant.From(in packetBankAnimationPlayer));
		info.AddProperty(PropertyName.cardZombie, Variant.From(in cardZombie));
		info.AddProperty(PropertyName.cardItem, Variant.From(in cardItem));
		info.AddProperty(PropertyName.cardGraveStone, Variant.From(in cardGraveStone));
		info.AddProperty(PropertyName.seedBank, Variant.From(in seedBank));
		info.AddProperty(PropertyName.mobilePreset, Variant.From(in mobilePreset));
		info.AddProperty(PropertyName._packetPool, Variant.CreateFrom(_packetPool));
		info.AddProperty(PropertyName._virtualPacketRefreshQueued, Variant.From(in _virtualPacketRefreshQueued));
		info.AddProperty(PropertyName.packetBankFeature, Variant.From(in packetBankFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.translate, out var value))
		{
			translate = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.packetBankPanelTexture, out var value2))
		{
			packetBankPanelTexture = value2.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.packetBankScroll, out var value3))
		{
			packetBankScroll = value3.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName.packetBankMargin, out var value4))
		{
			packetBankMargin = value4.As<MarginContainer>();
		}
		if (info.TryGetProperty(PropertyName.packetContainer, out var value5))
		{
			packetContainer = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.animeNode, out var value6))
		{
			animeNode = value6.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.cardSort, out var value7))
		{
			cardSort = value7.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.packetBankAnimationPlayer, out var value8))
		{
			packetBankAnimationPlayer = value8.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName.cardZombie, out var value9))
		{
			cardZombie = value9.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.cardItem, out var value10))
		{
			cardItem = value10.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.cardGraveStone, out var value11))
		{
			cardGraveStone = value11.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.seedBank, out var value12))
		{
			seedBank = value12.As<TowerDefenseInGameSeedBank>();
		}
		if (info.TryGetProperty(PropertyName.mobilePreset, out var value13))
		{
			mobilePreset = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._packetPool, out var value14))
		{
			_packetPool = value14.AsGodotArray<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._virtualPacketRefreshQueued, out var value15))
		{
			_virtualPacketRefreshQueued = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetBankFeature, out var value16))
		{
			packetBankFeature = value16.As<TowerDefenseBattleFeaturePacketBank>();
		}
	}
}
