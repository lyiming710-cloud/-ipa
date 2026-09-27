using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Scene/TowerDefesne/TowerDefenseNew/RemoteCursor.cs")]
public class RemoteCursor : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetPlayerInfo = "SetPlayerInfo";

		public static readonly StringName UpdatePosition = "UpdatePosition";

		public static readonly StringName UpdatePick = "UpdatePick";

		public static readonly StringName _UpdatePickSprite = "_UpdatePickSprite";

		public static readonly StringName UpdatePickNodePosition = "UpdatePickNodePosition";

		public static readonly StringName EscapeBbcode = "EscapeBbcode";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName _Hide = "_Hide";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _cursorSprite = "_cursorSprite";

		public static readonly StringName _label = "_label";

		public static readonly StringName _pickNode = "_pickNode";

		public static readonly StringName _currentPickType = "_currentPickType";

		public static readonly StringName _currentPickName = "_currentPickName";

		public static readonly StringName _targetPosition = "_targetPosition";

		public static readonly StringName _currentPosition = "_currentPosition";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static PackedScene _packetShowScene;

	private static Texture2D _gloveTexture;

	private static readonly Color[] PlayerColors = new Color[4]
	{
		new Color(0.3f, 0.7f, 1f),
		new Color(1f, 0.5f, 0.3f),
		new Color(0.5f, 1f, 0.5f),
		new Color(1f, 1f, 0.3f)
	};

	private Sprite2D _cursorSprite;

	private RichTextLabel _label;

	private CanvasItem _pickNode;

	private string _currentPickType = "";

	private string _currentPickName = "";

	private Vector2 _targetPosition = new Vector2(-100f, -100f);

	private Vector2 _currentPosition = new Vector2(-100f, -100f);

	private static readonly Vector2 PickOffset = new Vector2(0f, -30f);

	public object PacketConfig => null;

	public override void _Ready()
	{
		_cursorSprite = GetNode<Sprite2D>("%CursorSprite");
		_label = GetNode<RichTextLabel>("%Label");
		_label.BbcodeEnabled = true;
		_label.AddThemeFontSizeOverride("normal_font_size", 14);
		_label.AddThemeColorOverride("font_outline_color", Colors.Black);
		_label.AddThemeConstantOverride("outline_size", 3);
	}

	public void SetPlayerInfo(int playerIndex, string playerName = "")
	{
		Color modulate = PlayerColors[playerIndex % PlayerColors.Length];
		_cursorSprite.Modulate = modulate;
		string text = ((playerName != "") ? playerName : $"P{playerIndex + 1}");
		string value = modulate.ToHtml(includeAlpha: false);
		_label.Text = $"[color=#{value}]{EscapeBbcode(text)}[/color]";
		_label.Visible = true;
	}

	public void UpdatePosition(float posX, float posY)
	{
		_targetPosition = new Vector2(posX, posY);
		_cursorSprite.Visible = true;
	}

	public void UpdatePick(string pickType, string pickName)
	{
		if (!(pickType == _currentPickType) || !(pickName == _currentPickName))
		{
			_currentPickType = pickType;
			_currentPickName = pickName;
			_UpdatePickSprite();
		}
	}

	private void _UpdatePickSprite()
	{
		if (GodotObject.IsInstanceValid(_pickNode))
		{
			_pickNode.QueueFree();
			_pickNode = null;
		}
		if (_currentPickType == "" || _currentPickName == "")
		{
			return;
		}
		switch (_currentPickType)
		{
		case "plant":
		{
			TowerDefensePacketConfig towerDefensePacketConfig = TowerDefenseManager.GetPacketConfigReadOnly(_currentPickName);
			if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				towerDefensePacketConfig = TowerDefenseManager.GetPacketConfigReadOnlyByCharacterName(_currentPickName);
			}
			if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				return;
			}
			if (_packetShowScene == null)
			{
				_packetShowScene = GD.Load<PackedScene>("uid://bhqecss20rwpb");
			}
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = (TowerDefenseInGamePacketShow)_packetShowScene.Instantiate(PackedScene.GenEditState.Disabled);
			towerDefenseInGamePacketShow.Modulate = new Color(towerDefenseInGamePacketShow.Modulate, 0.7f);
			towerDefenseInGamePacketShow.ZIndex = 1900;
			towerDefenseInGamePacketShow.ZAsRelative = false;
			towerDefenseInGamePacketShow.MouseFilter = Control.MouseFilterEnum.Ignore;
			_pickNode = towerDefenseInGamePacketShow;
			AddChild(_pickNode, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGamePacketShow.Init(towerDefensePacketConfig);
			towerDefenseInGamePacketShow.button.MouseFilter = Control.MouseFilterEnum.Ignore;
			break;
		}
		case "shovel":
		{
			ShovelConfig shovel = TowerDefenseManager.GetShovel(_currentPickName);
			if (GodotObject.IsInstanceValid(shovel) && GodotObject.IsInstanceValid(shovel.texture))
			{
				Sprite2D sprite2D2 = new Sprite2D();
				sprite2D2.Texture = shovel.texture;
				sprite2D2.Modulate = new Color(sprite2D2.Modulate, 0.5f);
				sprite2D2.ZIndex = 1900;
				sprite2D2.ZAsRelative = false;
				sprite2D2.Scale = Vector2.One * 80f / shovel.texture.GetWidth();
				_pickNode = sprite2D2;
				AddChild(_pickNode, forceReadableName: false, InternalMode.Disabled);
			}
			break;
		}
		case "glove":
			if (_gloveTexture == null)
			{
				_gloveTexture = GD.Load<Texture2D>("res://Asset/Texture/GUI/General/Glove/Glove.png");
			}
			if (GodotObject.IsInstanceValid(_gloveTexture))
			{
				Sprite2D sprite2D = new Sprite2D();
				sprite2D.Texture = _gloveTexture;
				sprite2D.Modulate = new Color(sprite2D.Modulate, 0.5f);
				sprite2D.ZIndex = 1900;
				sprite2D.ZAsRelative = false;
				sprite2D.Scale = Vector2.One * 80f / _gloveTexture.GetWidth();
				_pickNode = sprite2D;
				AddChild(_pickNode, forceReadableName: false, InternalMode.Disabled);
			}
			break;
		}
		UpdatePickNodePosition();
	}

	private void UpdatePickNodePosition()
	{
		if (GodotObject.IsInstanceValid(_pickNode))
		{
			Vector2 position = _currentPosition + PickOffset;
			if (_pickNode is Control control)
			{
				control.Position = position;
			}
			else if (_pickNode is Node2D node2D)
			{
				node2D.Position = position;
			}
		}
	}

	private static string EscapeBbcode(string text)
	{
		return (text ?? "").Replace("[", "[lb]");
	}

	public override void _Process(double delta)
	{
		if (_cursorSprite.Visible)
		{
			_currentPosition = _currentPosition.Lerp(_targetPosition, Mathf.Min(1f, (float)delta * 20f));
			_cursorSprite.Position = _currentPosition;
			_label.Position = _currentPosition + new Vector2(16f, -8f);
			UpdatePickNodePosition();
		}
	}

	public void _Hide()
	{
		_cursorSprite.Visible = false;
		_label.Visible = false;
		if (GodotObject.IsInstanceValid(_pickNode))
		{
			_pickNode.Visible = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPlayerInfo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "playerIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "playerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "pickType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "pickName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UpdatePickSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePickNodePosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EscapeBbcode, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Hide, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetPlayerInfo && args.Count == 2)
		{
			SetPlayerInfo(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePosition && args.Count == 2)
		{
			UpdatePosition(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePick && args.Count == 2)
		{
			UpdatePick(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._UpdatePickSprite && args.Count == 0)
		{
			_UpdatePickSprite();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePickNodePosition && args.Count == 0)
		{
			UpdatePickNodePosition();
			ret = default;
			return true;
		}
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Hide && args.Count == 0)
		{
			_Hide();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.SetPlayerInfo)
		{
			return true;
		}
		if (method == MethodName.UpdatePosition)
		{
			return true;
		}
		if (method == MethodName.UpdatePick)
		{
			return true;
		}
		if (method == MethodName._UpdatePickSprite)
		{
			return true;
		}
		if (method == MethodName.UpdatePickNodePosition)
		{
			return true;
		}
		if (method == MethodName.EscapeBbcode)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._Hide)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._cursorSprite)
		{
			_cursorSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._label)
		{
			_label = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._pickNode)
		{
			_pickNode = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		if (name == PropertyName._currentPickType)
		{
			_currentPickType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._currentPickName)
		{
			_currentPickName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._targetPosition)
		{
			_targetPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._currentPosition)
		{
			_currentPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._cursorSprite)
		{
			value = VariantUtils.CreateFrom(in _cursorSprite);
			return true;
		}
		if (name == PropertyName._label)
		{
			value = VariantUtils.CreateFrom(in _label);
			return true;
		}
		if (name == PropertyName._pickNode)
		{
			value = VariantUtils.CreateFrom(in _pickNode);
			return true;
		}
		if (name == PropertyName._currentPickType)
		{
			value = VariantUtils.CreateFrom(in _currentPickType);
			return true;
		}
		if (name == PropertyName._currentPickName)
		{
			value = VariantUtils.CreateFrom(in _currentPickName);
			return true;
		}
		if (name == PropertyName._targetPosition)
		{
			value = VariantUtils.CreateFrom(in _targetPosition);
			return true;
		}
		if (name == PropertyName._currentPosition)
		{
			value = VariantUtils.CreateFrom(in _currentPosition);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._cursorSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._label, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pickNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentPickType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentPickName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._targetPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._currentPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._cursorSprite, Variant.From(in _cursorSprite));
		info.AddProperty(PropertyName._label, Variant.From(in _label));
		info.AddProperty(PropertyName._pickNode, Variant.From(in _pickNode));
		info.AddProperty(PropertyName._currentPickType, Variant.From(in _currentPickType));
		info.AddProperty(PropertyName._currentPickName, Variant.From(in _currentPickName));
		info.AddProperty(PropertyName._targetPosition, Variant.From(in _targetPosition));
		info.AddProperty(PropertyName._currentPosition, Variant.From(in _currentPosition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._cursorSprite, out var value))
		{
			_cursorSprite = value.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._label, out var value2))
		{
			_label = value2.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._pickNode, out var value3))
		{
			_pickNode = value3.As<CanvasItem>();
		}
		if (info.TryGetProperty(PropertyName._currentPickType, out var value4))
		{
			_currentPickType = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._currentPickName, out var value5))
		{
			_currentPickName = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._targetPosition, out var value6))
		{
			_targetPosition = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._currentPosition, out var value7))
		{
			_currentPosition = value7.As<Vector2>();
		}
	}
}
