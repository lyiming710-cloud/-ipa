using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxMultiplayerLobby/DialogBoxMultiplayerLobby.cs")]
public class DialogBoxMultiplayerLobby : DialogPopup
{
	public enum STATE
	{
		SelectMode,
		InputIp,
		InRoom
	}

	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName _SetState = "_SetState";

		public static readonly StringName _UpdateUiState = "_UpdateUiState";

		public static readonly StringName _OnCreateButtonPressed = "_OnCreateButtonPressed";

		public static readonly StringName _OnJoinButtonPressed = "_OnJoinButtonPressed";

		public static readonly StringName _OnConfirmJoinButtonPressed = "_OnConfirmJoinButtonPressed";

		public static readonly StringName _OnCancelJoinButtonPressed = "_OnCancelJoinButtonPressed";

		public static readonly StringName _OnLeaveButtonPressed = "_OnLeaveButtonPressed";

		public static readonly StringName _OnMatchCreated = "_OnMatchCreated";

		public static readonly StringName _OnMatchJoined = "_OnMatchJoined";

		public static readonly StringName _OnMatchLeft = "_OnMatchLeft";

		public static readonly StringName _OnPeerJoined = "_OnPeerJoined";

		public static readonly StringName _OnPeerLeft = "_OnPeerLeft";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName statusLabel = "statusLabel";

		public static readonly StringName roomIdLabel = "roomIdLabel";

		public static readonly StringName peerListLabel = "peerListLabel";

		public static readonly StringName createButton = "createButton";

		public static readonly StringName closeButton = "closeButton";

		public static readonly StringName roomIdInput = "roomIdInput";

		public static readonly StringName confirmJoinButton = "confirmJoinButton";

		public static readonly StringName hbox = "hbox";

		public static readonly StringName hbox2 = "hbox2";

		public static readonly StringName hbox3 = "hbox3";

		public static readonly StringName _state = "_state";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public RichTextLabel statusLabel;

	public RichTextLabel roomIdLabel;

	public RichTextLabel peerListLabel;

	public TextureButton createButton;

	public TextureButton closeButton;

	public LineEdit roomIdInput;

	public TextureButton confirmJoinButton;

	public HBoxContainer hbox;

	public HBoxContainer hbox2;

	public HBoxContainer hbox3;

	public STATE _state;

	public override void _Ready()
	{
		base._Ready();
		statusLabel = GetNode<RichTextLabel>("%StatusLabel");
		roomIdLabel = GetNode<RichTextLabel>("%RoomIdLabel");
		peerListLabel = GetNode<RichTextLabel>("%PeerListLabel");
		createButton = GetNode<TextureButton>("%CreateButton");
		closeButton = GetNode<TextureButton>("%CloseButton");
		roomIdInput = GetNode<LineEdit>("%RoomIdInput");
		confirmJoinButton = GetNode<TextureButton>("%ConfirmJoinButton");
		hbox = GetNode<HBoxContainer>("Layer/Control/HBox");
		hbox2 = GetNode<HBoxContainer>("Layer/Control/HBox2");
		hbox3 = GetNode<HBoxContainer>("Layer/Control/HBox3");
		GetNode<BaseButton>("%CreateButton").Pressed += _OnCreateButtonPressed;
		GetNode<BaseButton>("%JoinButton").Pressed += _OnJoinButtonPressed;
		GetNode<BaseButton>("%SelectCloseButton").Pressed += CloseDialog;
		GetNode<BaseButton>("%LeaveButton").Pressed += _OnLeaveButtonPressed;
		GetNode<BaseButton>("%CloseButton").Pressed += CloseDialog;
		GetNode<BaseButton>("%ConfirmJoinButton").Pressed += _OnConfirmJoinButtonPressed;
		GetNode<BaseButton>("%CancelButton").Pressed += _OnCancelJoinButtonPressed;
		MultiPlayerManager.Instance.OnMatchCreated += _OnMatchCreated;
		MultiPlayerManager.Instance.OnMatchJoined += _OnMatchJoined;
		MultiPlayerManager.Instance.OnMatchLeft += _OnMatchLeft;
		MultiPlayerManager.Instance.OnPeerJoined += _OnPeerJoined;
		MultiPlayerManager.Instance.OnPeerLeft += _OnPeerLeft;
		if (MultiPlayerManager.Instance.currentMatchId != "")
		{
			_state = STATE.InRoom;
		}
		_UpdateUiState();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (MultiPlayerManager.Instance != null)
		{
			MultiPlayerManager.Instance.OnMatchCreated -= _OnMatchCreated;
			MultiPlayerManager.Instance.OnMatchJoined -= _OnMatchJoined;
			MultiPlayerManager.Instance.OnMatchLeft -= _OnMatchLeft;
			MultiPlayerManager.Instance.OnPeerJoined -= _OnPeerJoined;
			MultiPlayerManager.Instance.OnPeerLeft -= _OnPeerLeft;
		}
	}

	public void _SetState(STATE newState)
	{
		_state = newState;
		_UpdateUiState();
	}

	public void _UpdateUiState()
	{
		hbox.Visible = _state == STATE.SelectMode;
		hbox3.Visible = _state == STATE.InputIp;
		hbox2.Visible = _state == STATE.InRoom;
		roomIdInput.Visible = _state == STATE.InputIp;
		roomIdLabel.Visible = _state == STATE.InRoom;
		peerListLabel.Visible = _state == STATE.InRoom;
		switch (_state)
		{
		case STATE.SelectMode:
			statusLabel.Clear();
			statusLabel.AppendText("[center]创建或加入房间开始联机[/center]");
			break;
		case STATE.InputIp:
			statusLabel.Clear();
			statusLabel.AppendText("[center]输入房主的IP地址[/center]");
			break;
		case STATE.InRoom:
			if (MultiPlayerManager.Instance.isHost)
			{
				statusLabel.Clear();
				statusLabel.AppendText("[center]你是房主 — 关闭此窗口后自由选关[/center]");
				roomIdLabel.Clear();
				string[] allLanIps = MultiPlayerManager.Instance.GetAllLanIps();
				if (allLanIps.Length == 0)
				{
					roomIdLabel.AppendText("[center]未检测到可用IP[/center]");
				}
				else
				{
					string text = "";
					int currentPort = MultiPlayerManager.Instance.CurrentPort;
					string[] array = allLanIps;
					foreach (string arg in array)
					{
						if (text != "")
						{
							text += "\n";
						}
						text += $"{arg}:{currentPort}";
					}
					roomIdLabel.AppendText($"[center]{text}[/center]");
				}
				closeButton.Visible = true;
			}
			else
			{
				statusLabel.Clear();
				statusLabel.AppendText("[center]已加入房间 — 等待房主选关[/center]");
				roomIdLabel.Clear();
				roomIdLabel.AppendText($"[center]主机: {MultiPlayerManager.Instance.currentMatchId}[/center]");
				closeButton.Visible = false;
			}
			peerListLabel.Clear();
			peerListLabel.AppendText($"[center]在线人数: {MultiPlayerManager.Instance.matchMembers.Count}[/center]");
			break;
		}
	}

	public void _OnCreateButtonPressed()
	{
		createButton.Disabled = true;
		MultiPlayerManager.Instance.CreateMatch();
		createButton.Disabled = false;
		if (MultiPlayerManager.Instance.currentMatchId != "")
		{
			_SetState(STATE.InRoom);
		}
	}

	public void _OnJoinButtonPressed()
	{
		_SetState(STATE.InputIp);
	}

	public void _OnConfirmJoinButtonPressed()
	{
		string text = roomIdInput.Text.StripEdges();
		if (text == "")
		{
			BroadCastManager.Instance.BroadCastFloatCreate("请输入IP地址", Colors.Red);
			return;
		}
		confirmJoinButton.Disabled = true;
		MultiPlayerManager.Instance.JoinMatch(text);
		confirmJoinButton.Disabled = false;
		if (MultiPlayerManager.Instance.currentMatchId != "")
		{
			_SetState(STATE.InRoom);
		}
	}

	public void _OnCancelJoinButtonPressed()
	{
		_SetState(STATE.SelectMode);
	}

	public void _OnLeaveButtonPressed()
	{
		MultiPlayerManager.Instance.LeaveMatch();
		_SetState(STATE.SelectMode);
	}

	public void _OnMatchCreated(string matchId)
	{
		_SetState(STATE.InRoom);
	}

	public void _OnMatchJoined(string matchId)
	{
		_SetState(STATE.InRoom);
	}

	public void _OnMatchLeft()
	{
		_SetState(STATE.SelectMode);
	}

	public void _OnPeerJoined(string username)
	{
		_UpdateUiState();
		BroadCastManager.Instance.BroadCastFloatCreate($"{username} 加入了房间", Colors.Green);
	}

	public void _OnPeerLeft(string username, string peerId)
	{
		_UpdateUiState();
		BroadCastManager.Instance.BroadCastFloatCreate($"{username} 离开了房间", Colors.Red);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._SetState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "newState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UpdateUiState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnCreateButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnJoinButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnConfirmJoinButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnCancelJoinButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnLeaveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnMatchCreated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "matchId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnMatchJoined, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "matchId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnMatchLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnPeerJoined, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "username", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnPeerLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "username", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "peerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._SetState && args.Count == 1)
		{
			_SetState(VariantUtils.ConvertTo<STATE>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UpdateUiState && args.Count == 0)
		{
			_UpdateUiState();
			ret = default;
			return true;
		}
		if (method == MethodName._OnCreateButtonPressed && args.Count == 0)
		{
			_OnCreateButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._OnJoinButtonPressed && args.Count == 0)
		{
			_OnJoinButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._OnConfirmJoinButtonPressed && args.Count == 0)
		{
			_OnConfirmJoinButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._OnCancelJoinButtonPressed && args.Count == 0)
		{
			_OnCancelJoinButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._OnLeaveButtonPressed && args.Count == 0)
		{
			_OnLeaveButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._OnMatchCreated && args.Count == 1)
		{
			_OnMatchCreated(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnMatchJoined && args.Count == 1)
		{
			_OnMatchJoined(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnMatchLeft && args.Count == 0)
		{
			_OnMatchLeft();
			ret = default;
			return true;
		}
		if (method == MethodName._OnPeerJoined && args.Count == 1)
		{
			_OnPeerJoined(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnPeerLeft && args.Count == 2)
		{
			_OnPeerLeft(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._SetState)
		{
			return true;
		}
		if (method == MethodName._UpdateUiState)
		{
			return true;
		}
		if (method == MethodName._OnCreateButtonPressed)
		{
			return true;
		}
		if (method == MethodName._OnJoinButtonPressed)
		{
			return true;
		}
		if (method == MethodName._OnConfirmJoinButtonPressed)
		{
			return true;
		}
		if (method == MethodName._OnCancelJoinButtonPressed)
		{
			return true;
		}
		if (method == MethodName._OnLeaveButtonPressed)
		{
			return true;
		}
		if (method == MethodName._OnMatchCreated)
		{
			return true;
		}
		if (method == MethodName._OnMatchJoined)
		{
			return true;
		}
		if (method == MethodName._OnMatchLeft)
		{
			return true;
		}
		if (method == MethodName._OnPeerJoined)
		{
			return true;
		}
		if (method == MethodName._OnPeerLeft)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.statusLabel)
		{
			statusLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.roomIdLabel)
		{
			roomIdLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.peerListLabel)
		{
			peerListLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.createButton)
		{
			createButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.closeButton)
		{
			closeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.roomIdInput)
		{
			roomIdInput = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName.confirmJoinButton)
		{
			confirmJoinButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.hbox)
		{
			hbox = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.hbox2)
		{
			hbox2 = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.hbox3)
		{
			hbox3 = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._state)
		{
			_state = VariantUtils.ConvertTo<STATE>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.statusLabel)
		{
			value = VariantUtils.CreateFrom(in statusLabel);
			return true;
		}
		if (name == PropertyName.roomIdLabel)
		{
			value = VariantUtils.CreateFrom(in roomIdLabel);
			return true;
		}
		if (name == PropertyName.peerListLabel)
		{
			value = VariantUtils.CreateFrom(in peerListLabel);
			return true;
		}
		if (name == PropertyName.createButton)
		{
			value = VariantUtils.CreateFrom(in createButton);
			return true;
		}
		if (name == PropertyName.closeButton)
		{
			value = VariantUtils.CreateFrom(in closeButton);
			return true;
		}
		if (name == PropertyName.roomIdInput)
		{
			value = VariantUtils.CreateFrom(in roomIdInput);
			return true;
		}
		if (name == PropertyName.confirmJoinButton)
		{
			value = VariantUtils.CreateFrom(in confirmJoinButton);
			return true;
		}
		if (name == PropertyName.hbox)
		{
			value = VariantUtils.CreateFrom(in hbox);
			return true;
		}
		if (name == PropertyName.hbox2)
		{
			value = VariantUtils.CreateFrom(in hbox2);
			return true;
		}
		if (name == PropertyName.hbox3)
		{
			value = VariantUtils.CreateFrom(in hbox3);
			return true;
		}
		if (name == PropertyName._state)
		{
			value = VariantUtils.CreateFrom(in _state);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.roomIdLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.peerListLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.createButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.closeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.roomIdInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.confirmJoinButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hbox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hbox2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.hbox3, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._state, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.statusLabel, Variant.From(in statusLabel));
		info.AddProperty(PropertyName.roomIdLabel, Variant.From(in roomIdLabel));
		info.AddProperty(PropertyName.peerListLabel, Variant.From(in peerListLabel));
		info.AddProperty(PropertyName.createButton, Variant.From(in createButton));
		info.AddProperty(PropertyName.closeButton, Variant.From(in closeButton));
		info.AddProperty(PropertyName.roomIdInput, Variant.From(in roomIdInput));
		info.AddProperty(PropertyName.confirmJoinButton, Variant.From(in confirmJoinButton));
		info.AddProperty(PropertyName.hbox, Variant.From(in hbox));
		info.AddProperty(PropertyName.hbox2, Variant.From(in hbox2));
		info.AddProperty(PropertyName.hbox3, Variant.From(in hbox3));
		info.AddProperty(PropertyName._state, Variant.From(in _state));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.statusLabel, out var value))
		{
			statusLabel = value.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.roomIdLabel, out var value2))
		{
			roomIdLabel = value2.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.peerListLabel, out var value3))
		{
			peerListLabel = value3.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.createButton, out var value4))
		{
			createButton = value4.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.closeButton, out var value5))
		{
			closeButton = value5.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.roomIdInput, out var value6))
		{
			roomIdInput = value6.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName.confirmJoinButton, out var value7))
		{
			confirmJoinButton = value7.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.hbox, out var value8))
		{
			hbox = value8.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.hbox2, out var value9))
		{
			hbox2 = value9.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.hbox3, out var value10))
		{
			hbox3 = value10.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._state, out var value11))
		{
			_state = value11.As<STATE>();
		}
	}
}
