using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/Preview/DailyChallengeLevelPreview.cs")]
public class DailyChallengeLevelPreview : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName Bind = "Bind";

		public static readonly StringName IsValidApiPath = "IsValidApiPath";

		public static readonly StringName SetSelected = "SetSelected";

		public static readonly StringName SetInteractionEnabled = "SetInteractionEnabled";

		public static readonly StringName HandleCardInput = "HandleCardInput";

		public static readonly StringName HandlePointerButton = "HandlePointerButton";

		public static readonly StringName UpdatePointerDrag = "UpdatePointerDrag";

		public static readonly StringName ResetPointerGesture = "ResetPointerGesture";

		public static readonly StringName FindScrollContainer = "FindScrollContainer";

		public static readonly StringName RefreshVisuals = "RefreshVisuals";

		public static readonly StringName RefreshAuthorAndDifficulty = "RefreshAuthorAndDifficulty";

		public static readonly StringName GetOptionalMetadataText = "GetOptionalMetadataText";

		public static readonly StringName SetMapPreview = "SetMapPreview";

		public static readonly StringName RefreshTags = "RefreshTags";

		public static readonly StringName AddTagChip = "AddTagChip";

		public static readonly StringName BuildRewardText = "BuildRewardText";

		public static readonly StringName SetFinishIcons = "SetFinishIcons";

		public static readonly StringName AddIcon = "AddIcon";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName LevelId = "LevelId";

		public static readonly StringName ApiPath = "ApiPath";

		public static readonly StringName DisplayName = "DisplayName";

		public static readonly StringName Metadata = "Metadata";

		public static readonly StringName IsAvailable = "IsAvailable";

		public static readonly StringName IsFinished = "IsFinished";

		public static readonly StringName IsSelected = "IsSelected";

		public static readonly StringName IsInteractionEnabled = "IsInteractionEnabled";

		public static readonly StringName _nameLabel = "_nameLabel";

		public static readonly StringName _typeLabel = "_typeLabel";

		public static readonly StringName _authorLabel = "_authorLabel";

		public static readonly StringName _difficultyLabel = "_difficultyLabel";

		public static readonly StringName _difficultyBadge = "_difficultyBadge";

		public static readonly StringName _rewardLabel = "_rewardLabel";

		public static readonly StringName _unavailableLabel = "_unavailableLabel";

		public static readonly StringName _mapTexture = "_mapTexture";

		public static readonly StringName _finishTexture = "_finishTexture";

		public static readonly StringName _iconBox = "_iconBox";

		public static readonly StringName _tagFlow = "_tagFlow";

		public static readonly StringName _tagChipTemplate = "_tagChipTemplate";

		public static readonly StringName _selectionCorners = "_selectionCorners";

		public static readonly StringName _interactionEnabled = "_interactionEnabled";

		public static readonly StringName _pointerPressed = "_pointerPressed";

		public static readonly StringName _pointerDragged = "_pointerDragged";

		public static readonly StringName _touchIndex = "_touchIndex";

		public static readonly StringName _pressPosition = "_pressPosition";

		public static readonly StringName _pressScrollOffset = "_pressScrollOffset";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public const float DragDeadzone = 20f;

	public const int MaxVisibleTags = 8;

	private static Texture2D _izIcon;

	private static Texture2D _iz2Icon;

	private static Texture2D _vrIcon;

	private static Texture2D _endlessIcon;

	private static Texture2D _survivalIcon;

	private static Texture2D _luckyIcon;

	private Label _nameLabel;

	private Label _typeLabel;

	private Label _authorLabel;

	private Label _difficultyLabel;

	private Control _difficultyBadge;

	private Label _rewardLabel;

	private Label _unavailableLabel;

	private TextureRect _mapTexture;

	private TextureRect _finishTexture;

	private HBoxContainer _iconBox;

	private HFlowContainer _tagFlow;

	private PanelContainer _tagChipTemplate;

	private Control _selectionCorners;

	private bool _interactionEnabled = true;

	private bool _pointerPressed;

	private bool _pointerDragged;

	private int _touchIndex = -1;

	private Vector2 _pressPosition;

	private int _pressScrollOffset;

	private static Texture2D IZ_ICON => _izIcon ?? (_izIcon = GD.Load<Texture2D>("uid://ciyj1718ypbih"));

	private static Texture2D IZ2_ICON => _iz2Icon ?? (_iz2Icon = GD.Load<Texture2D>("uid://b8rsot26eb7od"));

	private static Texture2D VR_ICON => _vrIcon ?? (_vrIcon = GD.Load<Texture2D>("uid://24j6iw1ww08b"));

	private static Texture2D ENDLESS_ICON => _endlessIcon ?? (_endlessIcon = GD.Load<Texture2D>("uid://bcjx34t665il2"));

	private static Texture2D SURVIVAL_ICON => _survivalIcon ?? (_survivalIcon = GD.Load<Texture2D>("uid://crvtrh32bqu0r"));

	private static Texture2D LUCKY_ICON => _luckyIcon ?? (_luckyIcon = GD.Load<Texture2D>("uid://caqbmeu30cbsa"));

	public string LevelId { get; private set; } = "";

	public string ApiPath { get; private set; } = "";

	public string DisplayName { get; private set; } = "";

	public Dictionary Metadata { get; private set; } = new Dictionary();

	public bool IsAvailable { get; private set; }

	public bool IsFinished { get; private set; }

	public bool IsSelected { get; private set; }

	public bool IsInteractionEnabled
	{
		get
		{
			if (_interactionEnabled)
			{
				return IsAvailable;
			}
			return false;
		}
	}

	public event Action<DailyChallengeLevelPreview> OnSelected;

	public override void _Ready()
	{
		_nameLabel = GetNode<Label>("%NameLabel");
		_typeLabel = GetNode<Label>("%TypeLabel");
		_authorLabel = GetNode<Label>("%AuthorLabel");
		_difficultyLabel = GetNode<Label>("%DifficultyLabel");
		_difficultyBadge = GetNode<Control>("%DifficultyBadge");
		_rewardLabel = GetNode<Label>("%RewardLabel");
		_unavailableLabel = GetNode<Label>("%UnavailableLabel");
		_mapTexture = GetNode<TextureRect>("%MapTexture");
		_finishTexture = GetNode<TextureRect>("%FinishTexture");
		_iconBox = GetNode<HBoxContainer>("%IconBox");
		_tagFlow = GetNode<HFlowContainer>("%TagFlow");
		_tagChipTemplate = _tagFlow.GetNode<PanelContainer>("TagChipTemplate");
		_selectionCorners = GetNode<Control>("%SelectionCorners");
		GuiInput += HandleCardInput;
		RefreshVisuals();
	}

	public override void _Notification(int what)
	{
		if ((long)what == 47)
		{
			ResetPointerGesture();
		}
	}

	public void Bind(string levelId, Dictionary metadata, bool finished)
	{
		LevelId = levelId ?? "";
		Metadata = metadata?.Duplicate(deep: true) ?? new Dictionary();
		ApiPath = Metadata.GetValueOrDefault("api", "").AsString().Trim();
		DisplayName = Metadata.GetValueOrDefault("name", LevelId).AsString();
		if (string.IsNullOrWhiteSpace(DisplayName))
		{
			DisplayName = LevelId;
		}
		IsFinished = finished;
		IsAvailable = !string.IsNullOrWhiteSpace(LevelId) && IsValidApiPath(ApiPath);
		if (IsNodeReady())
		{
			RefreshVisuals();
		}
	}

	public static bool IsValidApiPath(string apiPath)
	{
		if (string.IsNullOrWhiteSpace(apiPath) || !apiPath.StartsWith("/", StringComparison.Ordinal) || apiPath.StartsWith("//", StringComparison.Ordinal))
		{
			return false;
		}
		for (int i = 0; i < apiPath.Length; i++)
		{
			if (char.IsControl(apiPath[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void SetSelected(bool selected)
	{
		IsSelected = selected;
		if (GodotObject.IsInstanceValid(_selectionCorners))
		{
			_selectionCorners.Visible = selected;
		}
	}

	public void SetInteractionEnabled(bool enabled)
	{
		_interactionEnabled = enabled;
		MouseFilter = (MouseFilterEnum)(IsInteractionEnabled ? 1 : 2);
		MouseDefaultCursorShape = (CursorShape)(IsInteractionEnabled ? 2 : 0);
		DailyChallengeLevelPreview dailyChallengeLevelPreview = this;
		Color modulate;
		if (IsAvailable)
		{
			modulate = (enabled ? Colors.White : new Color(0.78f, 0.78f, 0.78f));
		}
		else
		{
			modulate = new Color(0.55f, 0.55f, 0.55f);
		}
		dailyChallengeLevelPreview.Modulate = modulate;
		if (!IsInteractionEnabled)
		{
			ResetPointerGesture();
		}
	}

	public void HandleCardInput(InputEvent inputEvent)
	{
		if (!IsInteractionEnabled || inputEvent == null)
		{
			return;
		}
		if (!(inputEvent is InputEventMouseButton inputEventMouseButton))
		{
			if (!(inputEvent is InputEventMouseMotion inputEventMouseMotion))
			{
				if (!(inputEvent is InputEventScreenTouch inputEventScreenTouch))
				{
					if (inputEvent is InputEventScreenDrag inputEventScreenDrag && inputEventScreenDrag.Index == _touchIndex)
					{
						UpdatePointerDrag(inputEventScreenDrag.Position);
					}
				}
				else if (inputEventScreenTouch.Pressed)
				{
					if (_touchIndex < 0)
					{
						HandlePointerButton(pressed: true, inputEventScreenTouch.Position, inputEventScreenTouch.Index);
					}
				}
				else if (inputEventScreenTouch.Index == _touchIndex)
				{
					HandlePointerButton(pressed: false, inputEventScreenTouch.Position, inputEventScreenTouch.Index);
				}
			}
			else if (_pointerPressed && _touchIndex < 0)
			{
				UpdatePointerDrag(inputEventMouseMotion.Position);
			}
		}
		else if (inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			HandlePointerButton(inputEventMouseButton.Pressed, inputEventMouseButton.Position, -1);
		}
	}

	private void HandlePointerButton(bool pressed, Vector2 position, int touchIndex)
	{
		if (pressed)
		{
			_pointerPressed = true;
			_pointerDragged = false;
			_touchIndex = touchIndex;
			_pressPosition = position;
			_pressScrollOffset = FindScrollContainer()?.ScrollHorizontal ?? 0;
		}
		else if (_pointerPressed && touchIndex == _touchIndex)
		{
			UpdatePointerDrag(position);
			ScrollContainer scrollContainer = FindScrollContainer();
			bool flag = scrollContainer != null && scrollContainer.ScrollHorizontal != _pressScrollOffset;
			bool num = !_pointerDragged && !flag && new Rect2(Vector2.Zero, Size).HasPoint(position);
			ResetPointerGesture();
			if (num)
			{
				OnSelected?.Invoke(this);
			}
		}
	}

	private void UpdatePointerDrag(Vector2 position)
	{
		if (_pressPosition.DistanceTo(position) >= 20f)
		{
			_pointerDragged = true;
		}
	}

	private void ResetPointerGesture()
	{
		_pointerPressed = false;
		_pointerDragged = false;
		_touchIndex = -1;
	}

	private ScrollContainer FindScrollContainer()
	{
		for (Node parent = GetParent(); parent != null; parent = parent.GetParent())
		{
			if (parent is ScrollContainer result)
			{
				return result;
			}
		}
		return null;
	}

	private void RefreshVisuals()
	{
		if (GodotObject.IsInstanceValid(_nameLabel))
		{
			_nameLabel.Text = (string.IsNullOrWhiteSpace(DisplayName) ? "关卡数据缺失" : DisplayName);
			_typeLabel.Text = "类型：" + Metadata.GetValueOrDefault("type", "未知").AsString();
			RefreshAuthorAndDifficulty();
			RefreshTags();
			_rewardLabel.Text = BuildRewardText();
			_finishTexture.Visible = IsFinished;
			_unavailableLabel.Visible = !IsAvailable;
			SetMapPreview();
			SetFinishIcons();
			SetSelected(IsSelected);
			SetInteractionEnabled(_interactionEnabled);
		}
	}

	private void RefreshAuthorAndDifficulty()
	{
		string optionalMetadataText = GetOptionalMetadataText("authorName");
		_authorLabel.Text = ((optionalMetadataText.Length > 0) ? ("作者：" + optionalMetadataText) : "");
		_authorLabel.TooltipText = _authorLabel.Text;
		_authorLabel.Visible = optionalMetadataText.Length > 0;
		string optionalMetadataText2 = GetOptionalMetadataText("difficulty");
		string text = optionalMetadataText2 switch
		{
			"easy" => "休闲", 
			"normal" => "普通", 
			"hard" => "挑战", 
			_ => optionalMetadataText2, 
		};
		_difficultyLabel.Text = ((text.Length > 0) ? ("难度：" + text) : "");
		_difficultyBadge.TooltipText = _difficultyLabel.Text;
		_difficultyBadge.Visible = text.Length > 0;
	}

	private string GetOptionalMetadataText(string key)
	{
		Variant valueOrDefault = Metadata.GetValueOrDefault(key, "");
		if (valueOrDefault.VariantType != Variant.Type.String)
		{
			return "";
		}
		return valueOrDefault.AsString().Trim();
	}

	private void SetMapPreview()
	{
		string mapName = Metadata.GetValueOrDefault("map", "Frontlawn").AsString();
		TowerDefenseMapConfig towerDefenseMapConfig = null;
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			towerDefenseMapConfig = TowerDefenseManager.Instance.GetMapConfig(mapName);
		}
		TowerDefenseMapConfig.ApplyMapPreviewTexture(_mapTexture, towerDefenseMapConfig?.GetMapThumbnail());
	}

	private void RefreshTags()
	{
		if (!GodotObject.IsInstanceValid(_tagFlow) || !GodotObject.IsInstanceValid(_tagChipTemplate))
		{
			return;
		}
		foreach (Node child in _tagFlow.GetChildren())
		{
			if (child != _tagChipTemplate)
			{
				_tagFlow.RemoveChild(child);
				child.QueueFree();
			}
		}
		List<string> list = BuildTags();
		if (list.Count == 0)
		{
			list.Add("无");
		}
		foreach (string item in list)
		{
			AddTagChip(item);
		}
	}

	private void AddTagChip(string tag)
	{
		PanelContainer panelContainer = _tagChipTemplate.Duplicate() as PanelContainer;
		if (GodotObject.IsInstanceValid(panelContainer))
		{
			panelContainer.Name = "TagChip";
			panelContainer.Visible = true;
			panelContainer.TooltipText = tag;
			panelContainer.MouseFilter = MouseFilterEnum.Ignore;
			Label node = panelContainer.GetNode<Label>("Text");
			node.Text = tag;
			node.TooltipText = tag;
			node.MouseFilter = MouseFilterEnum.Ignore;
			_tagFlow.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private List<string> BuildTags()
	{
		List<string> list = new List<string>();
		AddTagValue(Metadata.GetValueOrDefault("tag", ""), list);
		AddTagValue(Metadata.GetValueOrDefault("tags", new Godot.Collections.Array()), list);
		return list;
	}

	private static void AddTagValue(Variant value, List<string> tags)
	{
		if (tags.Count >= 8 || value.VariantType == Variant.Type.Nil)
		{
			return;
		}
		if (value.VariantType == Variant.Type.Array)
		{
			foreach (Variant item in value.AsGodotArray())
			{
				AddTagValue(item, tags);
				if (tags.Count >= 8)
				{
					break;
				}
			}
			return;
		}
		string text = value.AsString().Trim();
		if (text.Length > 0 && !tags.Contains(text))
		{
			tags.Add(text);
		}
	}

	private string BuildRewardText()
	{
		if (!Metadata.ContainsKey("reward") || Metadata["reward"].VariantType != Variant.Type.Dictionary)
		{
			return "奖励：未知";
		}
		Dictionary dictionary = Metadata["reward"].AsGodotDictionary();
		string text = dictionary.GetValueOrDefault("type", "").AsString();
		string text2 = dictionary.GetValueOrDefault("value", "").AsString();
		if (string.IsNullOrWhiteSpace(text))
		{
			return "奖励：未知";
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return "奖励：" + text + " " + text2;
		}
		return "奖励：" + text;
	}

	private void SetFinishIcons()
	{
		while (_iconBox.GetChildCount() > 0)
		{
			Node child = _iconBox.GetChild(0);
			_iconBox.RemoveChild(child);
			child.QueueFree();
		}
		switch (Metadata.GetValueOrDefault("finishMethod", "WAVE").AsString().ToUpperInvariant())
		{
		case "VASE":
			AddIcon(VR_ICON);
			break;
		case "IZM":
			AddIcon(IZ_ICON);
			break;
		case "IZM2":
			AddIcon(IZ2_ICON);
			break;
		case "WAVE":
			if (Metadata.ContainsKey("survivalRoundlimit") && Metadata["survivalRoundlimit"].VariantType != Variant.Type.Nil)
			{
				int num = Metadata["survivalRoundlimit"].AsInt32();
				if (num == -1)
				{
					AddIcon(ENDLESS_ICON);
				}
				else if (num >= 0)
				{
					AddIcon(SURVIVAL_ICON);
				}
			}
			break;
		}
		if (Metadata.GetValueOrDefault("lucky", false).AsBool())
		{
			AddIcon(LUCKY_ICON);
		}
	}

	private void AddIcon(Texture2D texture)
	{
		if (GodotObject.IsInstanceValid(texture) && _iconBox.GetChildCount() < 2)
		{
			TextureRect node = new TextureRect
			{
				Texture = texture,
				CustomMinimumSize = new Vector2(50f, 50f),
				ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
				StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
				MouseFilter = MouseFilterEnum.Ignore
			};
			_iconBox.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(20)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "metadata", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "finished", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidApiPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "apiPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetInteractionEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandleCardInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandlePointerButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "touchIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePointerDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetPointerGesture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindScrollContainer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ScrollContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshAuthorAndDifficulty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetOptionalMetadataText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddTagChip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRewardText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFinishIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Bind && args.Count == 3)
		{
			Bind(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsValidApiPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidApiPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelected && args.Count == 1)
		{
			SetSelected(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetInteractionEnabled && args.Count == 1)
		{
			SetInteractionEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleCardInput && args.Count == 1)
		{
			HandleCardInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandlePointerButton && args.Count == 3)
		{
			HandlePointerButton(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePointerDrag && args.Count == 1)
		{
			UpdatePointerDrag(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPointerGesture && args.Count == 0)
		{
			ResetPointerGesture();
			ret = default;
			return true;
		}
		if (method == MethodName.FindScrollContainer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ScrollContainer>(FindScrollContainer());
			return true;
		}
		if (method == MethodName.RefreshVisuals && args.Count == 0)
		{
			RefreshVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAuthorAndDifficulty && args.Count == 0)
		{
			RefreshAuthorAndDifficulty();
			ret = default;
			return true;
		}
		if (method == MethodName.GetOptionalMetadataText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetOptionalMetadataText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetMapPreview && args.Count == 0)
		{
			SetMapPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTags && args.Count == 0)
		{
			RefreshTags();
			ret = default;
			return true;
		}
		if (method == MethodName.AddTagChip && args.Count == 1)
		{
			AddTagChip(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRewardText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildRewardText());
			return true;
		}
		if (method == MethodName.SetFinishIcons && args.Count == 0)
		{
			SetFinishIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.AddIcon && args.Count == 1)
		{
			AddIcon(VariantUtils.ConvertTo<Texture2D>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsValidApiPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidApiPath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName.IsValidApiPath)
		{
			return true;
		}
		if (method == MethodName.SetSelected)
		{
			return true;
		}
		if (method == MethodName.SetInteractionEnabled)
		{
			return true;
		}
		if (method == MethodName.HandleCardInput)
		{
			return true;
		}
		if (method == MethodName.HandlePointerButton)
		{
			return true;
		}
		if (method == MethodName.UpdatePointerDrag)
		{
			return true;
		}
		if (method == MethodName.ResetPointerGesture)
		{
			return true;
		}
		if (method == MethodName.FindScrollContainer)
		{
			return true;
		}
		if (method == MethodName.RefreshVisuals)
		{
			return true;
		}
		if (method == MethodName.RefreshAuthorAndDifficulty)
		{
			return true;
		}
		if (method == MethodName.GetOptionalMetadataText)
		{
			return true;
		}
		if (method == MethodName.SetMapPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshTags)
		{
			return true;
		}
		if (method == MethodName.AddTagChip)
		{
			return true;
		}
		if (method == MethodName.BuildRewardText)
		{
			return true;
		}
		if (method == MethodName.SetFinishIcons)
		{
			return true;
		}
		if (method == MethodName.AddIcon)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LevelId)
		{
			LevelId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ApiPath)
		{
			ApiPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			DisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Metadata)
		{
			Metadata = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.IsAvailable)
		{
			IsAvailable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsFinished)
		{
			IsFinished = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsSelected)
		{
			IsSelected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nameLabel)
		{
			_nameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._typeLabel)
		{
			_typeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._authorLabel)
		{
			_authorLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._difficultyLabel)
		{
			_difficultyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._difficultyBadge)
		{
			_difficultyBadge = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._rewardLabel)
		{
			_rewardLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._unavailableLabel)
		{
			_unavailableLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mapTexture)
		{
			_mapTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._finishTexture)
		{
			_finishTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._iconBox)
		{
			_iconBox = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._tagFlow)
		{
			_tagFlow = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._tagChipTemplate)
		{
			_tagChipTemplate = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._selectionCorners)
		{
			_selectionCorners = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._interactionEnabled)
		{
			_interactionEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pointerPressed)
		{
			_pointerPressed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pointerDragged)
		{
			_pointerDragged = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._touchIndex)
		{
			_touchIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pressPosition)
		{
			_pressPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._pressScrollOffset)
		{
			_pressScrollOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.LevelId)
		{
			from = LevelId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ApiPath)
		{
			from = ApiPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			from = DisplayName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Metadata)
		{
			value = VariantUtils.CreateFrom<Dictionary>(Metadata);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsAvailable)
		{
			from2 = IsAvailable;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsFinished)
		{
			from2 = IsFinished;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsSelected)
		{
			from2 = IsSelected;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsInteractionEnabled)
		{
			from2 = IsInteractionEnabled;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._nameLabel)
		{
			value = VariantUtils.CreateFrom(in _nameLabel);
			return true;
		}
		if (name == PropertyName._typeLabel)
		{
			value = VariantUtils.CreateFrom(in _typeLabel);
			return true;
		}
		if (name == PropertyName._authorLabel)
		{
			value = VariantUtils.CreateFrom(in _authorLabel);
			return true;
		}
		if (name == PropertyName._difficultyLabel)
		{
			value = VariantUtils.CreateFrom(in _difficultyLabel);
			return true;
		}
		if (name == PropertyName._difficultyBadge)
		{
			value = VariantUtils.CreateFrom(in _difficultyBadge);
			return true;
		}
		if (name == PropertyName._rewardLabel)
		{
			value = VariantUtils.CreateFrom(in _rewardLabel);
			return true;
		}
		if (name == PropertyName._unavailableLabel)
		{
			value = VariantUtils.CreateFrom(in _unavailableLabel);
			return true;
		}
		if (name == PropertyName._mapTexture)
		{
			value = VariantUtils.CreateFrom(in _mapTexture);
			return true;
		}
		if (name == PropertyName._finishTexture)
		{
			value = VariantUtils.CreateFrom(in _finishTexture);
			return true;
		}
		if (name == PropertyName._iconBox)
		{
			value = VariantUtils.CreateFrom(in _iconBox);
			return true;
		}
		if (name == PropertyName._tagFlow)
		{
			value = VariantUtils.CreateFrom(in _tagFlow);
			return true;
		}
		if (name == PropertyName._tagChipTemplate)
		{
			value = VariantUtils.CreateFrom(in _tagChipTemplate);
			return true;
		}
		if (name == PropertyName._selectionCorners)
		{
			value = VariantUtils.CreateFrom(in _selectionCorners);
			return true;
		}
		if (name == PropertyName._interactionEnabled)
		{
			value = VariantUtils.CreateFrom(in _interactionEnabled);
			return true;
		}
		if (name == PropertyName._pointerPressed)
		{
			value = VariantUtils.CreateFrom(in _pointerPressed);
			return true;
		}
		if (name == PropertyName._pointerDragged)
		{
			value = VariantUtils.CreateFrom(in _pointerDragged);
			return true;
		}
		if (name == PropertyName._touchIndex)
		{
			value = VariantUtils.CreateFrom(in _touchIndex);
			return true;
		}
		if (name == PropertyName._pressPosition)
		{
			value = VariantUtils.CreateFrom(in _pressPosition);
			return true;
		}
		if (name == PropertyName._pressScrollOffset)
		{
			value = VariantUtils.CreateFrom(in _pressScrollOffset);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._nameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._authorLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._difficultyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._difficultyBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rewardLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unavailableLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._finishTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tagFlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tagChipTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectionCorners, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._interactionEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pointerPressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pointerDragged, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._touchIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._pressPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pressScrollOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ApiPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.DisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.Metadata, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsAvailable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsFinished, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSelected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsInteractionEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LevelId, Variant.From<string>(LevelId));
		info.AddProperty(PropertyName.ApiPath, Variant.From<string>(ApiPath));
		info.AddProperty(PropertyName.DisplayName, Variant.From<string>(DisplayName));
		info.AddProperty(PropertyName.Metadata, Variant.From<Dictionary>(Metadata));
		info.AddProperty(PropertyName.IsAvailable, Variant.From<bool>(IsAvailable));
		info.AddProperty(PropertyName.IsFinished, Variant.From<bool>(IsFinished));
		info.AddProperty(PropertyName.IsSelected, Variant.From<bool>(IsSelected));
		info.AddProperty(PropertyName._nameLabel, Variant.From(in _nameLabel));
		info.AddProperty(PropertyName._typeLabel, Variant.From(in _typeLabel));
		info.AddProperty(PropertyName._authorLabel, Variant.From(in _authorLabel));
		info.AddProperty(PropertyName._difficultyLabel, Variant.From(in _difficultyLabel));
		info.AddProperty(PropertyName._difficultyBadge, Variant.From(in _difficultyBadge));
		info.AddProperty(PropertyName._rewardLabel, Variant.From(in _rewardLabel));
		info.AddProperty(PropertyName._unavailableLabel, Variant.From(in _unavailableLabel));
		info.AddProperty(PropertyName._mapTexture, Variant.From(in _mapTexture));
		info.AddProperty(PropertyName._finishTexture, Variant.From(in _finishTexture));
		info.AddProperty(PropertyName._iconBox, Variant.From(in _iconBox));
		info.AddProperty(PropertyName._tagFlow, Variant.From(in _tagFlow));
		info.AddProperty(PropertyName._tagChipTemplate, Variant.From(in _tagChipTemplate));
		info.AddProperty(PropertyName._selectionCorners, Variant.From(in _selectionCorners));
		info.AddProperty(PropertyName._interactionEnabled, Variant.From(in _interactionEnabled));
		info.AddProperty(PropertyName._pointerPressed, Variant.From(in _pointerPressed));
		info.AddProperty(PropertyName._pointerDragged, Variant.From(in _pointerDragged));
		info.AddProperty(PropertyName._touchIndex, Variant.From(in _touchIndex));
		info.AddProperty(PropertyName._pressPosition, Variant.From(in _pressPosition));
		info.AddProperty(PropertyName._pressScrollOffset, Variant.From(in _pressScrollOffset));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LevelId, out var value))
		{
			LevelId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ApiPath, out var value2))
		{
			ApiPath = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DisplayName, out var value3))
		{
			DisplayName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Metadata, out var value4))
		{
			Metadata = value4.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.IsAvailable, out var value5))
		{
			IsAvailable = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsFinished, out var value6))
		{
			IsFinished = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsSelected, out var value7))
		{
			IsSelected = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nameLabel, out var value8))
		{
			_nameLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._typeLabel, out var value9))
		{
			_typeLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._authorLabel, out var value10))
		{
			_authorLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._difficultyLabel, out var value11))
		{
			_difficultyLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._difficultyBadge, out var value12))
		{
			_difficultyBadge = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._rewardLabel, out var value13))
		{
			_rewardLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._unavailableLabel, out var value14))
		{
			_unavailableLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mapTexture, out var value15))
		{
			_mapTexture = value15.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._finishTexture, out var value16))
		{
			_finishTexture = value16.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._iconBox, out var value17))
		{
			_iconBox = value17.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._tagFlow, out var value18))
		{
			_tagFlow = value18.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._tagChipTemplate, out var value19))
		{
			_tagChipTemplate = value19.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._selectionCorners, out var value20))
		{
			_selectionCorners = value20.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._interactionEnabled, out var value21))
		{
			_interactionEnabled = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pointerPressed, out var value22))
		{
			_pointerPressed = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pointerDragged, out var value23))
		{
			_pointerDragged = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._touchIndex, out var value24))
		{
			_touchIndex = value24.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pressPosition, out var value25))
		{
			_pressPosition = value25.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._pressScrollOffset, out var value26))
		{
			_pressScrollOffset = value26.As<int>();
		}
	}
}
