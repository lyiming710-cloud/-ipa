using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/ConveyorBelt/ConveyorBelt/ConveyorBeltManager.cs")]
public class ConveyorBeltManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName GetCardScroll = "GetCardScroll";

		public static readonly StringName RefreshCardScrolls = "RefreshCardScrolls";

		public static readonly StringName SetHudWidth = "SetHudWidth";

		public static readonly StringName SetHudSingleColumn = "SetHudSingleColumn";

		public static readonly StringName GetBoundSun = "GetBoundSun";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ApplyMode = "ApplyMode";

		public static readonly StringName SyncConveyorBelt = "SyncConveyorBelt";

		public static readonly StringName Init = "Init";

		public static readonly StringName ApplyConfiguredType = "ApplyConfiguredType";

		public static readonly StringName UpdateBeltAnimation = "UpdateBeltAnimation";

		public static readonly StringName GetPacketPos = "GetPacketPos";

		public static readonly StringName GetPacketChildren = "GetPacketChildren";

		public static readonly StringName GetPacketCount = "GetPacketCount";

		public static readonly StringName ResetPacketPositions = "ResetPacketPositions";

		public static readonly StringName AddPacketToUI = "AddPacketToUI";

		public static readonly StringName ShowMobileSunBar = "ShowMobileSunBar";

		public static readonly StringName GetMobileSunLabel = "GetMobileSunLabel";

		public static readonly StringName GetPCSunLabel = "GetPCSunLabel";

		public static readonly StringName UpdateSunDisplay = "UpdateSunDisplay";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName HasSunAccount = "HasSunAccount";

		public static readonly StringName pcConveyorBeltContainer = "pcConveyorBeltContainer";

		public static readonly StringName mobileConveyorBeltContainer = "mobileConveyorBeltContainer";

		public static readonly StringName pcControl = "pcControl";

		public static readonly StringName mobileControl = "mobileControl";

		public static readonly StringName packetContainer = "packetContainer";

		public static readonly StringName belt = "belt";

		public static readonly StringName mobileBelt1 = "mobileBelt1";

		public static readonly StringName mobilePacketContainer1 = "mobilePacketContainer1";

		public static readonly StringName mobileBelt2 = "mobileBelt2";

		public static readonly StringName mobilePacketContainer2 = "mobilePacketContainer2";

		public static readonly StringName conveyorBeltRectPC = "conveyorBeltRectPC";

		public static readonly StringName conveyorBeltRectMobile1 = "conveyorBeltRectMobile1";

		public static readonly StringName conveyorBeltRectMobile2 = "conveyorBeltRectMobile2";

		public static readonly StringName mobileConveyorBeltSunBarTexture = "mobileConveyorBeltSunBarTexture";

		public static readonly StringName mobileConveyorBeltSunLabel = "mobileConveyorBeltSunLabel";

		public static readonly StringName pcConveyorBeltSunBarTexture = "pcConveyorBeltSunBarTexture";

		public static readonly StringName pcConveyorBeltSunLabel = "pcConveyorBeltSunLabel";

		public static readonly StringName beltTime = "beltTime";

		public static readonly StringName isMobileUI = "isMobileUI";

		public static readonly StringName conveyorBeltContainer = "conveyorBeltContainer";

		public static readonly StringName isSunType = "isSunType";

		public static readonly StringName sunNumShow = "sunNumShow";

		public static readonly StringName _configuredType = "_configuredType";

		public static readonly StringName _hudSingleColumn = "_hudSingleColumn";

		public static readonly StringName _pcCardClip = "_pcCardClip";

		public static readonly StringName _mobileCardClip1 = "_mobileCardClip1";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static Texture2D _conveyorBeltSun;

	private static Texture2D _conveyorBeltSunBackdrop;

	public HBoxContainer pcConveyorBeltContainer;

	public VBoxContainer mobileConveyorBeltContainer;

	public Control pcControl;

	public Control mobileControl;

	public Control packetContainer;

	public TextureRect belt;

	public Sprite2D mobileBelt1;

	public Control mobilePacketContainer1;

	public Sprite2D mobileBelt2;

	public Control mobilePacketContainer2;

	public NinePatchRect conveyorBeltRectPC;

	public NinePatchRect conveyorBeltRectMobile1;

	public NinePatchRect conveyorBeltRectMobile2;

	public TextureRect mobileConveyorBeltSunBarTexture;

	public Label mobileConveyorBeltSunLabel;

	public TextureRect pcConveyorBeltSunBarTexture;

	public Label pcConveyorBeltSunLabel;

	public double beltTime;

	public bool isMobileUI;

	public Container conveyorBeltContainer;

	public bool isSunType;

	public long sunNumShow;

	private string _configuredType = "Default";

	private bool _hudSingleColumn;

	private Control _pcCardClip;

	private Control _mobileCardClip1;

	private (TowerDefenseCardScroll Scroll, Control Content, Control Clip, Control Packets, bool Vertical)[] _cardScrolls;

	private EconomyAccountId _sunAccountId;

	private static Texture2D ConveyorBeltSun => _conveyorBeltSun ?? (_conveyorBeltSun = GD.Load<Texture2D>("uid://i6rwl358wldw"));

	private static Texture2D ConveyorBeltSunBackdrop => _conveyorBeltSunBackdrop ?? (_conveyorBeltSunBackdrop = GD.Load<Texture2D>("uid://6h0njlvpp86"));

	public EconomyAccountId SunAccountId => _sunAccountId;

	public bool HasSunAccount => _sunAccountId.IsValid;

	public TowerDefenseCardScroll GetCardScroll()
	{
		return _cardScrolls?[isMobileUI ? 1u : 0u].Scroll;
	}

	private (TowerDefenseCardScroll, Control, Control, Control, bool) InstallCardScroll(Control packets, Control clip, bool vertical)
	{
		Node parent = packets.GetParent();
		TowerDefenseCardScroll towerDefenseCardScroll = new TowerDefenseCardScroll
		{
			Name = "CardsScroll",
			MouseFilter = MouseFilterEnum.Pass,
			ClipContents = true,
			ScrollDeadzone = 16
		};
		Control control = new Control
		{
			MouseFilter = MouseFilterEnum.Ignore
		};
		parent.AddChild(towerDefenseCardScroll, forceReadableName: false, InternalMode.Disabled);
		towerDefenseCardScroll.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		packets.Reparent(control);
		return (towerDefenseCardScroll, control, clip, packets, vertical);
	}

	private void RefreshCardScrolls()
	{
		if (_cardScrolls != null)
		{
			(TowerDefenseCardScroll, Control, Control, Control, bool)[] cardScrolls = _cardScrolls;
			for (int i = 0; i < cardScrolls.Length; i++)
			{
				(TowerDefenseCardScroll, Control, Control, Control, bool) tuple = cardScrolls[i];
				Vector2 size = tuple.Item3.Size;
				float num = tuple.Item4.GetChildCount() * (tuple.Item5 ? 61 : 53) + 7;
				bool enabled = num > (tuple.Item5 ? size.Y : size.X);
				tuple.Item1.ConfigureScrolling(enabled, tuple.Item5);
				Vector2 vector = (tuple.Item5 ? new Vector2(size.X, Math.Max(size.Y, num)) : new Vector2(Math.Max(size.X, num), size.Y));
				tuple.Item2.CustomMinimumSize = vector;
				tuple.Item2.Size = vector;
				tuple.Item1.Size = size;
			}
		}
	}

	public void SetHudWidth(float width)
	{
		pcControl.CustomMinimumSize = new Vector2(width, 0f);
		conveyorBeltRectPC.CustomMinimumSize = new Vector2(Mathf.Min(518f, width), 86f);
		conveyorBeltRectPC.GetParent<Control>().Size = new Vector2(width, 86f);
		_pcCardClip.Size = new Vector2(width - 14f, 74f);
		pcConveyorBeltContainer.Size = new Vector2(width + (float)(pcConveyorBeltSunBarTexture.Visible ? 80 : 0), 86f);
	}

	public void SetHudSingleColumn(bool enabled)
	{
		if (_hudSingleColumn != enabled && isMobileUI)
		{
			Array<Node> packetChildren = GetPacketChildren(mobilePreset: true);
			_hudSingleColumn = enabled;
			for (int i = 0; i < packetChildren.Count; i++)
			{
				Control control = (Control)packetChildren[i];
				Control control2 = ((enabled || i % 2 == 0) ? mobilePacketContainer1 : mobilePacketContainer2);
				if (control.GetParent() != control2)
				{
					control.Reparent(control2);
				}
				control2.MoveChild(control, -1);
				control.Position = new Vector2(0f, 16 + 61 * (enabled ? i : (i / 2)));
			}
		}
		_hudSingleColumn = enabled;
		float num = 600f;
		mobileControl.CustomMinimumSize = new Vector2(enabled ? 110 : 220, num);
		conveyorBeltRectMobile2.GetParent().GetParent<Control>().Visible = !enabled;
		conveyorBeltRectMobile1.GetParent<Control>().Size = new Vector2(110f, num);
		_mobileCardClip1.Size = new Vector2(96f, num - 12f);
	}

	public bool TryBindSunAccount(EconomyAccountId accountId)
	{
		if (!accountId.IsValid)
		{
			return false;
		}
		if (_sunAccountId.IsValid)
		{
			return _sunAccountId == accountId;
		}
		_sunAccountId = accountId;
		return true;
	}

	private long GetBoundSun()
	{
		if (!HasSunAccount)
		{
			return TowerDefenseManager.Instance.GetSun();
		}
		return TowerDefenseManager.Instance.GetSun(_sunAccountId);
	}

	public override void _Ready()
	{
		pcConveyorBeltContainer = GetNode<HBoxContainer>("%PCConveyorBeltContainer");
		mobileConveyorBeltContainer = GetNode<VBoxContainer>("%MobileConveyorBeltContainer");
		pcControl = GetNode<Control>("%PCControl");
		mobileControl = GetNode<Control>("%MobileControl");
		packetContainer = GetNode<Control>("%PacketContainer");
		belt = GetNode<TextureRect>("%Belt");
		mobileBelt1 = GetNode<Sprite2D>("%MobileBelt1");
		mobilePacketContainer1 = GetNode<Control>("%MobilePacketContainer1");
		mobileBelt2 = GetNode<Sprite2D>("%MobileBelt2");
		mobilePacketContainer2 = GetNode<Control>("%MobilePacketContainer2");
		conveyorBeltRectPC = GetNode<NinePatchRect>("%ConveyorBeltRectPC");
		conveyorBeltRectMobile1 = GetNode<NinePatchRect>("%ConveyorBeltRectMobile1");
		conveyorBeltRectMobile2 = GetNode<NinePatchRect>("%ConveyorBeltRectMobile2");
		mobileConveyorBeltSunBarTexture = GetNode<TextureRect>("%MobileConveyorBeltSunBarTexture");
		mobileConveyorBeltSunLabel = GetNode<Label>("%MobileConveyorBeltSunLabel");
		pcConveyorBeltSunBarTexture = GetNode<TextureRect>("%PCConveyorBeltSunBarTexture");
		pcConveyorBeltSunLabel = GetNode<Label>("%PCConveyorBeltSunLabel");
		_pcCardClip = packetContainer.GetParent().GetParent<Control>();
		_mobileCardClip1 = mobilePacketContainer1.GetParent().GetParent<Control>();
		Control parent = mobilePacketContainer2.GetParent().GetParent<Control>();
		_cardScrolls = new (TowerDefenseCardScroll, Control, Control, Control, bool)[3]
		{
			InstallCardScroll(packetContainer, _pcCardClip, vertical: false),
			InstallCardScroll(mobilePacketContainer1, _mobileCardClip1, vertical: true),
			InstallCardScroll(mobilePacketContainer2, parent, vertical: true)
		};
		RefreshCardScrolls();
		BattleEventBus.Instance.OnUiSwitched += ApplyMode;
		isMobileUI = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		conveyorBeltContainer = (isMobileUI ? ((BoxContainer)mobileConveyorBeltContainer) : ((BoxContainer)pcConveyorBeltContainer));
		ApplyMode(isMobileUI);
		ApplyConfiguredType();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched -= ApplyMode;
		}
	}

	public void ApplyMode(bool mobilePreset)
	{
		if (mobilePreset)
		{
			conveyorBeltContainer = mobileConveyorBeltContainer;
			pcConveyorBeltContainer.Visible = false;
			CustomMinimumSize = new Vector2(0f, CustomMinimumSize.Y);
		}
		else
		{
			conveyorBeltContainer = pcConveyorBeltContainer;
			mobileConveyorBeltContainer.Visible = false;
			CustomMinimumSize = new Vector2(781 + (isSunType ? 80 : 0), CustomMinimumSize.Y);
		}
		isMobileUI = mobilePreset;
		conveyorBeltContainer.Visible = true;
		SyncConveyorBelt(isMobileUI);
		if (isSunType)
		{
			if (GodotObject.IsInstanceValid(pcConveyorBeltSunBarTexture))
			{
				pcConveyorBeltSunBarTexture.Visible = !mobilePreset;
			}
			if (GodotObject.IsInstanceValid(pcConveyorBeltSunLabel))
			{
				pcConveyorBeltSunLabel.Visible = !mobilePreset;
			}
			ShowMobileSunBar(mobilePreset);
		}
	}

	public void SyncConveyorBelt(bool mobilePreset)
	{
		Array<Node> packetChildren = GetPacketChildren(!mobilePreset);
		int num = 0;
		foreach (Node item in packetChildren)
		{
			if (!(item is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow))
			{
				continue;
			}
			if (mobilePreset)
			{
				if (_hudSingleColumn || num % 2 == 0)
				{
					towerDefenseInGamePacketShow.Reparent(mobilePacketContainer1);
					towerDefenseInGamePacketShow.Position = new Vector2(0f, 16f + 61f * (_hudSingleColumn ? ((float)num) : Mathf.Floor((float)num / 2f)));
				}
				else
				{
					towerDefenseInGamePacketShow.Reparent(mobilePacketContainer2);
					towerDefenseInGamePacketShow.Position = new Vector2(0f, 16f + 61f * Mathf.Floor((float)num / 2f));
				}
			}
			else
			{
				towerDefenseInGamePacketShow.Reparent(packetContainer);
				towerDefenseInGamePacketShow.Position = new Vector2(13f + 53f * (float)num, 0f);
			}
			num++;
		}
	}

	public void Init(string type)
	{
		_configuredType = (string.IsNullOrWhiteSpace(type) ? "Default" : type);
		if (IsNodeReady())
		{
			ApplyConfiguredType();
		}
	}

	private void ApplyConfiguredType()
	{
		if (_configuredType == "Sun")
		{
			isSunType = true;
			if (GodotObject.IsInstanceValid(conveyorBeltRectPC))
			{
				conveyorBeltRectPC.Texture = ConveyorBeltSunBackdrop;
			}
			if (GodotObject.IsInstanceValid(belt))
			{
				belt.Texture = ConveyorBeltSun;
			}
			if (GodotObject.IsInstanceValid(conveyorBeltRectMobile1))
			{
				conveyorBeltRectMobile1.Texture = ConveyorBeltSunBackdrop;
			}
			if (GodotObject.IsInstanceValid(conveyorBeltRectMobile2))
			{
				conveyorBeltRectMobile2.Texture = ConveyorBeltSunBackdrop;
			}
			if (GodotObject.IsInstanceValid(mobileBelt1))
			{
				mobileBelt1.Texture = ConveyorBeltSun;
			}
			if (GodotObject.IsInstanceValid(mobileBelt2))
			{
				mobileBelt2.Texture = ConveyorBeltSun;
			}
			if (GodotObject.IsInstanceValid(pcConveyorBeltSunBarTexture))
			{
				pcConveyorBeltSunBarTexture.Visible = !isMobileUI;
			}
			if (GodotObject.IsInstanceValid(pcConveyorBeltSunLabel))
			{
				pcConveyorBeltSunLabel.Visible = !isMobileUI;
			}
			ShowMobileSunBar(isMobileUI);
			if (!isMobileUI)
			{
				CustomMinimumSize = new Vector2(861f, CustomMinimumSize.Y);
			}
			long boundSun = GetBoundSun();
			if (boundSun >= 0)
			{
				sunNumShow = boundSun;
				if (GodotObject.IsInstanceValid(pcConveyorBeltSunLabel))
				{
					pcConveyorBeltSunLabel.Text = sunNumShow.ToString();
				}
				if (GodotObject.IsInstanceValid(mobileConveyorBeltSunLabel))
				{
					mobileConveyorBeltSunLabel.Text = sunNumShow.ToString();
				}
			}
		}
		else
		{
			isSunType = false;
		}
	}

	public void UpdateBeltAnimation(double delta)
	{
		RefreshCardScrolls();
		if (!Visible)
		{
			return;
		}
		if (TowerDefenseManager.Instance.currentControl.isGameRunning)
		{
			beltTime += delta;
		}
		if (!isMobileUI)
		{
			if (GodotObject.IsInstanceValid(belt) && GodotObject.IsInstanceValid(belt.Material))
			{
				((ShaderMaterial)belt.Material).SetShaderParameter("time", beltTime);
			}
			int childCount = packetContainer.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				TowerDefenseInGamePacketShow childOrNull = packetContainer.GetChildOrNull<TowerDefenseInGamePacketShow>(i);
				if (childOrNull != null)
				{
					Vector2 packetPos = GetPacketPos(i);
					if (childOrNull.Position.X > packetPos.X)
					{
						childOrNull.Position = new Vector2(childOrNull.Position.X - (float)delta * 50f, childOrNull.Position.Y);
					}
					else
					{
						childOrNull.Position = new Vector2(packetPos.X, childOrNull.Position.Y);
					}
				}
			}
			return;
		}
		if (GodotObject.IsInstanceValid(mobileBelt1) && GodotObject.IsInstanceValid(mobileBelt1.Material))
		{
			((ShaderMaterial)mobileBelt1.Material).SetShaderParameter("time", beltTime);
		}
		if (GodotObject.IsInstanceValid(mobileBelt2) && GodotObject.IsInstanceValid(mobileBelt2.Material))
		{
			((ShaderMaterial)mobileBelt2.Material).SetShaderParameter("time", beltTime);
		}
		for (int j = 0; j < mobilePacketContainer1.GetChildCount(); j++)
		{
			TowerDefenseInGamePacketShow childOrNull2 = mobilePacketContainer1.GetChildOrNull<TowerDefenseInGamePacketShow>(j);
			if (childOrNull2 != null)
			{
				Vector2 vector = mobilePacketContainer1.Position + new Vector2(0f, 61 * j);
				if (childOrNull2.Position.Y > vector.Y)
				{
					childOrNull2.Position = new Vector2(childOrNull2.Position.X, childOrNull2.Position.Y - (float)delta * 50f);
				}
				else
				{
					childOrNull2.Position = new Vector2(childOrNull2.Position.X, vector.Y);
				}
			}
		}
		for (int k = 0; k < mobilePacketContainer2.GetChildCount(); k++)
		{
			TowerDefenseInGamePacketShow childOrNull3 = mobilePacketContainer2.GetChildOrNull<TowerDefenseInGamePacketShow>(k);
			if (childOrNull3 != null)
			{
				Vector2 vector2 = mobilePacketContainer2.Position + new Vector2(0f, 61 * k);
				if (childOrNull3.Position.Y > vector2.Y)
				{
					childOrNull3.Position = new Vector2(childOrNull3.Position.X, childOrNull3.Position.Y - (float)delta * 50f);
				}
				else
				{
					childOrNull3.Position = new Vector2(childOrNull3.Position.X, vector2.Y);
				}
			}
		}
	}

	public Vector2 GetPacketPos(int id)
	{
		return packetContainer.Position + new Vector2(53f * (float)id, 0f);
	}

	public Array<Node> GetPacketChildren(bool mobilePreset)
	{
		Array<Node> array = new Array<Node>();
		if (mobilePreset)
		{
			int num = (GodotObject.IsInstanceValid(mobilePacketContainer1) ? mobilePacketContainer1.GetChildCount() : 0);
			int num2 = (GodotObject.IsInstanceValid(mobilePacketContainer2) ? mobilePacketContainer2.GetChildCount() : 0);
			for (int i = 0; i < Math.Max(num, num2); i++)
			{
				if (i < num)
				{
					array.Add(mobilePacketContainer1.GetChild(i));
				}
				if (i < num2)
				{
					array.Add(mobilePacketContainer2.GetChild(i));
				}
			}
		}
		else if (GodotObject.IsInstanceValid(packetContainer))
		{
			array.AddRange(packetContainer.GetChildren());
		}
		return array;
	}

	public int GetPacketCount()
	{
		if (!isMobileUI)
		{
			return packetContainer.GetChildCount();
		}
		return mobilePacketContainer1.GetChildCount() + mobilePacketContainer2.GetChildCount();
	}

	public void ResetPacketPositions()
	{
		if (!isMobileUI)
		{
			int num = 0;
			{
				foreach (Node child in packetContainer.GetChildren())
				{
					if (child is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow)
					{
						towerDefenseInGamePacketShow.Position = new Vector2(13f + 53f * (float)num, 0f);
						num++;
					}
				}
				return;
			}
		}
		int num2 = 0;
		foreach (Node child2 in mobilePacketContainer1.GetChildren())
		{
			if (child2 is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow2)
			{
				towerDefenseInGamePacketShow2.Position = new Vector2(0f, 16f + 61f * (float)num2);
				num2++;
			}
		}
		int num3 = 0;
		foreach (Node child3 in mobilePacketContainer2.GetChildren())
		{
			if (child3 is TowerDefenseInGamePacketShow towerDefenseInGamePacketShow3)
			{
				towerDefenseInGamePacketShow3.Position = new Vector2(0f, 16f + 61f * (float)num3);
				num3++;
			}
		}
	}

	public void AddPacketToUI(TowerDefenseInGamePacketShow packet)
	{
		if (isMobileUI)
		{
			packet.Position = new Vector2(0f, 680f);
			if (_hudSingleColumn || mobilePacketContainer1.GetChildCount() <= mobilePacketContainer2.GetChildCount())
			{
				if (_hudSingleColumn)
				{
					packet.Position = new Vector2(0f, Mathf.Max(680, 61 * GetPacketCount() + 80));
				}
				mobilePacketContainer1.AddChild(packet, forceReadableName: false, InternalMode.Disabled);
			}
			else
			{
				mobilePacketContainer2.AddChild(packet, forceReadableName: false, InternalMode.Disabled);
			}
		}
		else
		{
			packet.Position = new Vector2(868f, 0f);
			packetContainer.AddChild(packet, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void ShowMobileSunBar(bool visible)
	{
		if (GodotObject.IsInstanceValid(mobileConveyorBeltSunBarTexture))
		{
			mobileConveyorBeltSunBarTexture.Visible = visible;
		}
	}

	public Label GetMobileSunLabel()
	{
		return mobileConveyorBeltSunLabel;
	}

	public Label GetPCSunLabel()
	{
		return pcConveyorBeltSunLabel;
	}

	public void UpdateSunDisplay()
	{
		if (!isSunType)
		{
			return;
		}
		long boundSun = GetBoundSun();
		if (sunNumShow != boundSun)
		{
			sunNumShow = boundSun;
			if (GodotObject.IsInstanceValid(pcConveyorBeltSunLabel))
			{
				pcConveyorBeltSunLabel.Text = sunNumShow.ToString();
			}
			if (GodotObject.IsInstanceValid(mobileConveyorBeltSunLabel))
			{
				mobileConveyorBeltSunLabel.Text = sunNumShow.ToString();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName.GetCardScroll, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ScrollContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCardScrolls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetHudWidth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHudSingleColumn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetBoundSun, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncConveyorBelt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyConfiguredType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateBeltAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketPos, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketChildren, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "mobilePreset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPacketPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddPacketToUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowMobileSunBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMobileSunLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPCSunLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSunDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetCardScroll && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCardScroll>(GetCardScroll());
			return true;
		}
		if (method == MethodName.RefreshCardScrolls && args.Count == 0)
		{
			RefreshCardScrolls();
			ret = default;
			return true;
		}
		if (method == MethodName.SetHudWidth && args.Count == 1)
		{
			SetHudWidth(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHudSingleColumn && args.Count == 1)
		{
			SetHudSingleColumn(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetBoundSun && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetBoundSun());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMode && args.Count == 1)
		{
			ApplyMode(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncConveyorBelt && args.Count == 1)
		{
			SyncConveyorBelt(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyConfiguredType && args.Count == 0)
		{
			ApplyConfiguredType();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateBeltAnimation && args.Count == 1)
		{
			UpdateBeltAnimation(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPacketPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPacketPos(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketChildren && args.Count == 1)
		{
			Array<Node> packetChildren = GetPacketChildren(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = VariantUtils.CreateFromArray(packetChildren);
			return true;
		}
		if (method == MethodName.GetPacketCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPacketCount());
			return true;
		}
		if (method == MethodName.ResetPacketPositions && args.Count == 0)
		{
			ResetPacketPositions();
			ret = default;
			return true;
		}
		if (method == MethodName.AddPacketToUI && args.Count == 1)
		{
			AddPacketToUI(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowMobileSunBar && args.Count == 1)
		{
			ShowMobileSunBar(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMobileSunLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(GetMobileSunLabel());
			return true;
		}
		if (method == MethodName.GetPCSunLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(GetPCSunLabel());
			return true;
		}
		if (method == MethodName.UpdateSunDisplay && args.Count == 0)
		{
			UpdateSunDisplay();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetCardScroll)
		{
			return true;
		}
		if (method == MethodName.RefreshCardScrolls)
		{
			return true;
		}
		if (method == MethodName.SetHudWidth)
		{
			return true;
		}
		if (method == MethodName.SetHudSingleColumn)
		{
			return true;
		}
		if (method == MethodName.GetBoundSun)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ApplyMode)
		{
			return true;
		}
		if (method == MethodName.SyncConveyorBelt)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ApplyConfiguredType)
		{
			return true;
		}
		if (method == MethodName.UpdateBeltAnimation)
		{
			return true;
		}
		if (method == MethodName.GetPacketPos)
		{
			return true;
		}
		if (method == MethodName.GetPacketChildren)
		{
			return true;
		}
		if (method == MethodName.GetPacketCount)
		{
			return true;
		}
		if (method == MethodName.ResetPacketPositions)
		{
			return true;
		}
		if (method == MethodName.AddPacketToUI)
		{
			return true;
		}
		if (method == MethodName.ShowMobileSunBar)
		{
			return true;
		}
		if (method == MethodName.GetMobileSunLabel)
		{
			return true;
		}
		if (method == MethodName.GetPCSunLabel)
		{
			return true;
		}
		if (method == MethodName.UpdateSunDisplay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pcConveyorBeltContainer)
		{
			pcConveyorBeltContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.mobileConveyorBeltContainer)
		{
			mobileConveyorBeltContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.pcControl)
		{
			pcControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.mobileControl)
		{
			mobileControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			packetContainer = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.belt)
		{
			belt = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.mobileBelt1)
		{
			mobileBelt1 = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.mobilePacketContainer1)
		{
			mobilePacketContainer1 = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.mobileBelt2)
		{
			mobileBelt2 = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.mobilePacketContainer2)
		{
			mobilePacketContainer2 = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectPC)
		{
			conveyorBeltRectPC = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectMobile1)
		{
			conveyorBeltRectMobile1 = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectMobile2)
		{
			conveyorBeltRectMobile2 = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.mobileConveyorBeltSunBarTexture)
		{
			mobileConveyorBeltSunBarTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.mobileConveyorBeltSunLabel)
		{
			mobileConveyorBeltSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.pcConveyorBeltSunBarTexture)
		{
			pcConveyorBeltSunBarTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.pcConveyorBeltSunLabel)
		{
			pcConveyorBeltSunLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.beltTime)
		{
			beltTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isMobileUI)
		{
			isMobileUI = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.conveyorBeltContainer)
		{
			conveyorBeltContainer = VariantUtils.ConvertTo<Container>(in value);
			return true;
		}
		if (name == PropertyName.isSunType)
		{
			isSunType = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sunNumShow)
		{
			sunNumShow = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._configuredType)
		{
			_configuredType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._hudSingleColumn)
		{
			_hudSingleColumn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pcCardClip)
		{
			_pcCardClip = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._mobileCardClip1)
		{
			_mobileCardClip1 = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HasSunAccount)
		{
			value = VariantUtils.CreateFrom<bool>(HasSunAccount);
			return true;
		}
		if (name == PropertyName.pcConveyorBeltContainer)
		{
			value = VariantUtils.CreateFrom(in pcConveyorBeltContainer);
			return true;
		}
		if (name == PropertyName.mobileConveyorBeltContainer)
		{
			value = VariantUtils.CreateFrom(in mobileConveyorBeltContainer);
			return true;
		}
		if (name == PropertyName.pcControl)
		{
			value = VariantUtils.CreateFrom(in pcControl);
			return true;
		}
		if (name == PropertyName.mobileControl)
		{
			value = VariantUtils.CreateFrom(in mobileControl);
			return true;
		}
		if (name == PropertyName.packetContainer)
		{
			value = VariantUtils.CreateFrom(in packetContainer);
			return true;
		}
		if (name == PropertyName.belt)
		{
			value = VariantUtils.CreateFrom(in belt);
			return true;
		}
		if (name == PropertyName.mobileBelt1)
		{
			value = VariantUtils.CreateFrom(in mobileBelt1);
			return true;
		}
		if (name == PropertyName.mobilePacketContainer1)
		{
			value = VariantUtils.CreateFrom(in mobilePacketContainer1);
			return true;
		}
		if (name == PropertyName.mobileBelt2)
		{
			value = VariantUtils.CreateFrom(in mobileBelt2);
			return true;
		}
		if (name == PropertyName.mobilePacketContainer2)
		{
			value = VariantUtils.CreateFrom(in mobilePacketContainer2);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectPC)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltRectPC);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectMobile1)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltRectMobile1);
			return true;
		}
		if (name == PropertyName.conveyorBeltRectMobile2)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltRectMobile2);
			return true;
		}
		if (name == PropertyName.mobileConveyorBeltSunBarTexture)
		{
			value = VariantUtils.CreateFrom(in mobileConveyorBeltSunBarTexture);
			return true;
		}
		if (name == PropertyName.mobileConveyorBeltSunLabel)
		{
			value = VariantUtils.CreateFrom(in mobileConveyorBeltSunLabel);
			return true;
		}
		if (name == PropertyName.pcConveyorBeltSunBarTexture)
		{
			value = VariantUtils.CreateFrom(in pcConveyorBeltSunBarTexture);
			return true;
		}
		if (name == PropertyName.pcConveyorBeltSunLabel)
		{
			value = VariantUtils.CreateFrom(in pcConveyorBeltSunLabel);
			return true;
		}
		if (name == PropertyName.beltTime)
		{
			value = VariantUtils.CreateFrom(in beltTime);
			return true;
		}
		if (name == PropertyName.isMobileUI)
		{
			value = VariantUtils.CreateFrom(in isMobileUI);
			return true;
		}
		if (name == PropertyName.conveyorBeltContainer)
		{
			value = VariantUtils.CreateFrom(in conveyorBeltContainer);
			return true;
		}
		if (name == PropertyName.isSunType)
		{
			value = VariantUtils.CreateFrom(in isSunType);
			return true;
		}
		if (name == PropertyName.sunNumShow)
		{
			value = VariantUtils.CreateFrom(in sunNumShow);
			return true;
		}
		if (name == PropertyName._configuredType)
		{
			value = VariantUtils.CreateFrom(in _configuredType);
			return true;
		}
		if (name == PropertyName._hudSingleColumn)
		{
			value = VariantUtils.CreateFrom(in _hudSingleColumn);
			return true;
		}
		if (name == PropertyName._pcCardClip)
		{
			value = VariantUtils.CreateFrom(in _pcCardClip);
			return true;
		}
		if (name == PropertyName._mobileCardClip1)
		{
			value = VariantUtils.CreateFrom(in _mobileCardClip1);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.pcConveyorBeltContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileConveyorBeltContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.belt, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileBelt1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobilePacketContainer1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileBelt2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobilePacketContainer2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltRectPC, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltRectMobile1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltRectMobile2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileConveyorBeltSunBarTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileConveyorBeltSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcConveyorBeltSunBarTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pcConveyorBeltSunLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.beltTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMobileUI, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.conveyorBeltContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSunType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNumShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._configuredType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hudSingleColumn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pcCardClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mobileCardClip1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasSunAccount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pcConveyorBeltContainer, Variant.From(in pcConveyorBeltContainer));
		info.AddProperty(PropertyName.mobileConveyorBeltContainer, Variant.From(in mobileConveyorBeltContainer));
		info.AddProperty(PropertyName.pcControl, Variant.From(in pcControl));
		info.AddProperty(PropertyName.mobileControl, Variant.From(in mobileControl));
		info.AddProperty(PropertyName.packetContainer, Variant.From(in packetContainer));
		info.AddProperty(PropertyName.belt, Variant.From(in belt));
		info.AddProperty(PropertyName.mobileBelt1, Variant.From(in mobileBelt1));
		info.AddProperty(PropertyName.mobilePacketContainer1, Variant.From(in mobilePacketContainer1));
		info.AddProperty(PropertyName.mobileBelt2, Variant.From(in mobileBelt2));
		info.AddProperty(PropertyName.mobilePacketContainer2, Variant.From(in mobilePacketContainer2));
		info.AddProperty(PropertyName.conveyorBeltRectPC, Variant.From(in conveyorBeltRectPC));
		info.AddProperty(PropertyName.conveyorBeltRectMobile1, Variant.From(in conveyorBeltRectMobile1));
		info.AddProperty(PropertyName.conveyorBeltRectMobile2, Variant.From(in conveyorBeltRectMobile2));
		info.AddProperty(PropertyName.mobileConveyorBeltSunBarTexture, Variant.From(in mobileConveyorBeltSunBarTexture));
		info.AddProperty(PropertyName.mobileConveyorBeltSunLabel, Variant.From(in mobileConveyorBeltSunLabel));
		info.AddProperty(PropertyName.pcConveyorBeltSunBarTexture, Variant.From(in pcConveyorBeltSunBarTexture));
		info.AddProperty(PropertyName.pcConveyorBeltSunLabel, Variant.From(in pcConveyorBeltSunLabel));
		info.AddProperty(PropertyName.beltTime, Variant.From(in beltTime));
		info.AddProperty(PropertyName.isMobileUI, Variant.From(in isMobileUI));
		info.AddProperty(PropertyName.conveyorBeltContainer, Variant.From(in conveyorBeltContainer));
		info.AddProperty(PropertyName.isSunType, Variant.From(in isSunType));
		info.AddProperty(PropertyName.sunNumShow, Variant.From(in sunNumShow));
		info.AddProperty(PropertyName._configuredType, Variant.From(in _configuredType));
		info.AddProperty(PropertyName._hudSingleColumn, Variant.From(in _hudSingleColumn));
		info.AddProperty(PropertyName._pcCardClip, Variant.From(in _pcCardClip));
		info.AddProperty(PropertyName._mobileCardClip1, Variant.From(in _mobileCardClip1));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pcConveyorBeltContainer, out var value))
		{
			pcConveyorBeltContainer = value.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.mobileConveyorBeltContainer, out var value2))
		{
			mobileConveyorBeltContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.pcControl, out var value3))
		{
			pcControl = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.mobileControl, out var value4))
		{
			mobileControl = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.packetContainer, out var value5))
		{
			packetContainer = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.belt, out var value6))
		{
			belt = value6.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.mobileBelt1, out var value7))
		{
			mobileBelt1 = value7.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.mobilePacketContainer1, out var value8))
		{
			mobilePacketContainer1 = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.mobileBelt2, out var value9))
		{
			mobileBelt2 = value9.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.mobilePacketContainer2, out var value10))
		{
			mobilePacketContainer2 = value10.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.conveyorBeltRectPC, out var value11))
		{
			conveyorBeltRectPC = value11.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.conveyorBeltRectMobile1, out var value12))
		{
			conveyorBeltRectMobile1 = value12.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.conveyorBeltRectMobile2, out var value13))
		{
			conveyorBeltRectMobile2 = value13.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.mobileConveyorBeltSunBarTexture, out var value14))
		{
			mobileConveyorBeltSunBarTexture = value14.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.mobileConveyorBeltSunLabel, out var value15))
		{
			mobileConveyorBeltSunLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.pcConveyorBeltSunBarTexture, out var value16))
		{
			pcConveyorBeltSunBarTexture = value16.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.pcConveyorBeltSunLabel, out var value17))
		{
			pcConveyorBeltSunLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.beltTime, out var value18))
		{
			beltTime = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isMobileUI, out var value19))
		{
			isMobileUI = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.conveyorBeltContainer, out var value20))
		{
			conveyorBeltContainer = value20.As<Container>();
		}
		if (info.TryGetProperty(PropertyName.isSunType, out var value21))
		{
			isSunType = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sunNumShow, out var value22))
		{
			sunNumShow = value22.As<long>();
		}
		if (info.TryGetProperty(PropertyName._configuredType, out var value23))
		{
			_configuredType = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName._hudSingleColumn, out var value24))
		{
			_hudSingleColumn = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pcCardClip, out var value25))
		{
			_pcCardClip = value25.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._mobileCardClip1, out var value26))
		{
			_mobileCardClip1 = value26.As<Control>();
		}
	}
}
