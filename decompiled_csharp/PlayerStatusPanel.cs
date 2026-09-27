using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Scene/TowerDefesne/TowerDefenseNew/PlayerStatusPanel.cs")]
public class PlayerStatusPanel : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName ShowPanel = "ShowPanel";

		public static readonly StringName HidePanel = "HidePanel";

		public static readonly StringName _RebuildRows = "_RebuildRows";

		public static readonly StringName _CreatePlayerRow = "_CreatePlayerRow";

		public static readonly StringName _UpdateRowData = "_UpdateRowData";

		public static readonly StringName _RefreshAllRows = "_RefreshAllRows";

		public static readonly StringName _OnPingUpdated = "_OnPingUpdated";

		public static readonly StringName _OnPeerChanged = "_OnPeerChanged";

		public static readonly StringName _OnPeerLeft = "_OnPeerLeft";

		public static readonly StringName _OnMatchLeft = "_OnMatchLeft";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _playerList = "_playerList";

		public static readonly StringName _refreshTimer = "_refreshTimer";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static readonly Color[] PlayerColors = new Color[4]
	{
		new Color(0.3f, 0.7f, 1f),
		new Color(1f, 0.5f, 0.3f),
		new Color(0.5f, 1f, 0.5f),
		new Color(1f, 1f, 0.3f)
	};

	private const string SignalBarFull = "■";

	private const string SignalBarEmpty = "□";

	private VBoxContainer _playerList;

	private readonly System.Collections.Generic.Dictionary<string, HBoxContainer> _playerRows = new System.Collections.Generic.Dictionary<string, HBoxContainer>();

	private double _refreshTimer;

	private const double RefreshInterval = 0.5;

	private readonly StringBuilder _signalTextBuilder = new StringBuilder(16);

	public override void _Ready()
	{
		_playerList = GetNode<VBoxContainer>("%PlayerList");
		Visible = false;
		if (MultiPlayerManager.Instance != null)
		{
			MultiPlayerManager.Instance.OnPingUpdated += _OnPingUpdated;
			MultiPlayerManager.Instance.OnPeerJoined += _OnPeerChanged;
			MultiPlayerManager.Instance.OnPeerLeft += _OnPeerLeft;
			MultiPlayerManager.Instance.OnMatchLeft += _OnMatchLeft;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (MultiPlayerManager.Instance != null)
		{
			MultiPlayerManager.Instance.OnPingUpdated -= _OnPingUpdated;
			MultiPlayerManager.Instance.OnPeerJoined -= _OnPeerChanged;
			MultiPlayerManager.Instance.OnPeerLeft -= _OnPeerLeft;
			MultiPlayerManager.Instance.OnMatchLeft -= _OnMatchLeft;
		}
	}

	public override void _Process(double delta)
	{
		if (Visible)
		{
			_refreshTimer += delta;
			if (_refreshTimer >= 0.5)
			{
				_refreshTimer = 0.0;
				_RefreshAllRows();
			}
		}
	}

	public void ShowPanel()
	{
		Visible = true;
		_refreshTimer = 0.0;
		_RebuildRows();
	}

	public void HidePanel()
	{
		Visible = false;
	}

	private void _RebuildRows()
	{
		foreach (Node child in _playerList.GetChildren())
		{
			child.QueueFree();
		}
		_playerRows.Clear();
		if (MultiPlayerManager.Instance != null && Global.IsMultiplayerMode)
		{
			Array array = MultiPlayerManager.Instance.matchMembers.Duplicate();
			if (!array.Contains(MultiPlayerManager.Instance.peerId))
			{
				array.Insert(0, MultiPlayerManager.Instance.peerId);
			}
			for (int i = 0; i < array.Count; i++)
			{
				string text = (string)array[i];
				HBoxContainer hBoxContainer = _CreatePlayerRow(text, i);
				_playerList.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
				_playerRows[text] = hBoxContainer;
			}
		}
	}

	private HBoxContainer _CreatePlayerRow(string peerIdStr, int index)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		hBoxContainer.CustomMinimumSize = new Vector2(340f, 28f);
		hBoxContainer.MouseFilter = MouseFilterEnum.Ignore;
		ColorRect colorRect = new ColorRect();
		colorRect.CustomMinimumSize = new Vector2(4f, 20f);
		colorRect.Color = PlayerColors[index % PlayerColors.Length];
		colorRect.MouseFilter = MouseFilterEnum.Ignore;
		hBoxContainer.AddChild(colorRect, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label();
		string text = ((MultiPlayerManager.Instance != null) ? MultiPlayerManager.Instance.GetPeerName(peerIdStr) : "Player");
		label.Text = text;
		label.AddThemeFontSizeOverride("font_size", 16);
		label.AddThemeColorOverride("font_color", new Color(1f, 1f, 1f, 0.9f));
		label.CustomMinimumSize = new Vector2(120f, 0f);
		label.MouseFilter = MouseFilterEnum.Ignore;
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Label label2 = new Label();
		label2.AddThemeFontSizeOverride("font_size", 16);
		label2.CustomMinimumSize = new Vector2(110f, 0f);
		label2.MouseFilter = MouseFilterEnum.Ignore;
		hBoxContainer.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		Label label3 = new Label();
		label3.AddThemeFontSizeOverride("font_size", 16);
		label3.CustomMinimumSize = new Vector2(80f, 0f);
		label3.HorizontalAlignment = HorizontalAlignment.Right;
		label3.MouseFilter = MouseFilterEnum.Ignore;
		hBoxContainer.AddChild(label3, forceReadableName: false, InternalMode.Disabled);
		_UpdateRowData(hBoxContainer, peerIdStr);
		return hBoxContainer;
	}

	private void _UpdateRowData(HBoxContainer row, string peerIdStr)
	{
		Array<Node> children = row.GetChildren();
		if (children.Count < 4)
		{
			return;
		}
		Label label = (Label)children[2];
		Label label2 = (Label)children[3];
		if (MultiPlayerManager.Instance == null)
		{
			return;
		}
		int peerLatency = MultiPlayerManager.Instance.GetPeerLatency(peerIdStr);
		int signalLevel = MultiPlayerManager.Instance.GetSignalLevel(peerIdStr);
		_signalTextBuilder.Clear();
		for (int i = 0; i < 5; i++)
		{
			_signalTextBuilder.Append((i < signalLevel) ? "■" : "□");
		}
		_signalTextBuilder.Append(' ');
		Color color;
		switch (signalLevel)
		{
		case 5:
			color = new Color(0.3f, 1f, 0.3f);
			_signalTextBuilder.Append("极好");
			break;
		case 4:
			color = new Color(0.5f, 1f, 0.5f);
			_signalTextBuilder.Append("良好");
			break;
		case 3:
			color = new Color(1f, 1f, 0.3f);
			_signalTextBuilder.Append("一般");
			break;
		case 2:
			color = new Color(1f, 0.6f, 0.2f);
			_signalTextBuilder.Append("较差");
			break;
		case 1:
			color = new Color(1f, 0.3f, 0.3f);
			_signalTextBuilder.Append("极差");
			break;
		default:
			color = new Color(0.5f, 0.5f, 0.5f);
			_signalTextBuilder.Append("无信号");
			break;
		}
		label.Text = _signalTextBuilder.ToString();
		label.AddThemeColorOverride("font_color", color);
		if (peerLatency > 0)
		{
			label2.Text = peerLatency + " ms";
			if (peerLatency <= 50)
			{
				label2.AddThemeColorOverride("font_color", new Color(0.3f, 1f, 0.3f));
			}
			else if (peerLatency <= 100)
			{
				label2.AddThemeColorOverride("font_color", new Color(0.5f, 1f, 0.5f));
			}
			else if (peerLatency <= 200)
			{
				label2.AddThemeColorOverride("font_color", new Color(1f, 1f, 0.3f));
			}
			else if (peerLatency <= 400)
			{
				label2.AddThemeColorOverride("font_color", new Color(1f, 0.6f, 0.2f));
			}
			else
			{
				label2.AddThemeColorOverride("font_color", new Color(1f, 0.3f, 0.3f));
			}
		}
		else
		{
			label2.Text = "--- ms";
			label2.AddThemeColorOverride("font_color", new Color(0.5f, 0.5f, 0.5f));
		}
	}

	private void _RefreshAllRows()
	{
		foreach (var (peerIdStr, hBoxContainer2) in _playerRows)
		{
			if (GodotObject.IsInstanceValid(hBoxContainer2))
			{
				_UpdateRowData(hBoxContainer2, peerIdStr);
			}
		}
	}

	private void _OnPingUpdated(string peerId, int latencyMs)
	{
		if (Visible && _playerRows.TryGetValue(peerId, out var value) && GodotObject.IsInstanceValid(value))
		{
			_UpdateRowData(value, peerId);
		}
	}

	private void _OnPeerChanged(string username)
	{
		if (Visible)
		{
			_RebuildRows();
		}
	}

	private void _OnPeerLeft(string username, string peerId)
	{
		if (Visible)
		{
			_RebuildRows();
		}
	}

	private void _OnMatchLeft()
	{
		Visible = false;
		foreach (Node child in _playerList.GetChildren())
		{
			child.QueueFree();
		}
		_playerRows.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HidePanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._RebuildRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._CreatePlayerRow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "peerIdStr", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UpdateRowData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "row", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "peerIdStr", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RefreshAllRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnPingUpdated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "peerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "latencyMs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnPeerChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "username", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnPeerLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "username", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "peerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnMatchLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPanel && args.Count == 0)
		{
			ShowPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.HidePanel && args.Count == 0)
		{
			HidePanel();
			ret = default;
			return true;
		}
		if (method == MethodName._RebuildRows && args.Count == 0)
		{
			_RebuildRows();
			ret = default;
			return true;
		}
		if (method == MethodName._CreatePlayerRow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<HBoxContainer>(_CreatePlayerRow(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName._UpdateRowData && args.Count == 2)
		{
			_UpdateRowData(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._RefreshAllRows && args.Count == 0)
		{
			_RefreshAllRows();
			ret = default;
			return true;
		}
		if (method == MethodName._OnPingUpdated && args.Count == 2)
		{
			_OnPingUpdated(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnPeerChanged && args.Count == 1)
		{
			_OnPeerChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnPeerLeft && args.Count == 2)
		{
			_OnPeerLeft(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnMatchLeft && args.Count == 0)
		{
			_OnMatchLeft();
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.ShowPanel)
		{
			return true;
		}
		if (method == MethodName.HidePanel)
		{
			return true;
		}
		if (method == MethodName._RebuildRows)
		{
			return true;
		}
		if (method == MethodName._CreatePlayerRow)
		{
			return true;
		}
		if (method == MethodName._UpdateRowData)
		{
			return true;
		}
		if (method == MethodName._RefreshAllRows)
		{
			return true;
		}
		if (method == MethodName._OnPingUpdated)
		{
			return true;
		}
		if (method == MethodName._OnPeerChanged)
		{
			return true;
		}
		if (method == MethodName._OnPeerLeft)
		{
			return true;
		}
		if (method == MethodName._OnMatchLeft)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._playerList)
		{
			_playerList = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._refreshTimer)
		{
			_refreshTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._playerList)
		{
			value = VariantUtils.CreateFrom(in _playerList);
			return true;
		}
		if (name == PropertyName._refreshTimer)
		{
			value = VariantUtils.CreateFrom(in _refreshTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._playerList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._refreshTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._playerList, Variant.From(in _playerList));
		info.AddProperty(PropertyName._refreshTimer, Variant.From(in _refreshTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._playerList, out var value))
		{
			_playerList = value.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._refreshTimer, out var value2))
		{
			_refreshTimer = value2.As<double>();
		}
	}
}
