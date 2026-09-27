using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/OnlineLevelExchange/OnlineLevelExchange.cs")]
public class OnlineLevelExchange : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnExchangeIndexChanged = "OnExchangeIndexChanged";

		public static readonly StringName OnDialogVisibilityChanged = "OnDialogVisibilityChanged";

		public static readonly StringName RefreshExchangeState = "RefreshExchangeState";

		public static readonly StringName PlayButtonPressed = "PlayButtonPressed";

		public static readonly StringName Setup = "Setup";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";

		public static readonly StringName AlmanacButtonPressed = "AlmanacButtonPressed";

		public static readonly StringName Fade = "Fade";

		public static readonly StringName SwitchModeButtonPressed = "SwitchModeButtonPressed";

		public static readonly StringName ExchangeButtonPressed = "ExchangeButtonPressed";

		public static readonly StringName ReadButtonPressed = "ReadButtonPressed";

		public static readonly StringName ReadpaperTextureGuiInput = "ReadpaperTextureGuiInput";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName dragMenu = "dragMenu";

		public static readonly StringName costLabel = "costLabel";

		public static readonly StringName exchangeButton = "exchangeButton";

		public static readonly StringName crystalNumLabel = "crystalNumLabel";

		public static readonly StringName readpaperTexture = "readpaperTexture";

		public static readonly StringName switchModeButton = "switchModeButton";

		public static readonly StringName readButton = "readButton";

		public static readonly StringName currentGroup = "currentGroup";

		public static readonly StringName currentMode = "currentMode";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private static Json _onlineLevelExchangeResource;

	private static Texture2D _texturePlant;

	private static Texture2D _textureCustom;

	private DragMenu dragMenu;

	private Label costLabel;

	private TextureButton exchangeButton;

	private Label crystalNumLabel;

	private TextureRect readpaperTexture;

	private TextureButton switchModeButton;

	private TextureButton readButton;

	public string currentGroup;

	public string currentMode = "Plant";

	public List<Dictionary> filteredExchangeList = new List<Dictionary>();

	private static Json ONLINE_LEVEL_EXCHANGE_RESOURCE => _onlineLevelExchangeResource ?? (_onlineLevelExchangeResource = GD.Load<Json>("res://Asset/Config/OnlineLevelExchange/OnlineLevelExchangeResource.json"));

	private static Texture2D TEXTURE_PLANT => _texturePlant ?? (_texturePlant = GD.Load<Texture2D>("res://Asset/Texture/GUI/OnlineLevelExchange/OnlineLevelExchangeSelectPlant.png"));

	private static Texture2D TEXTURE_CUSTOM => _textureCustom ?? (_textureCustom = GD.Load<Texture2D>("res://Asset/Texture/GUI/OnlineLevelExchange/OnlineLevelExchangeSelectCustom.png"));

	public override void _Ready()
	{
		base._Ready();
		dragMenu = GetNode<DragMenu>("%DragMenu");
		costLabel = GetNode<Label>("%CostLabel");
		exchangeButton = GetNode<TextureButton>("%ExchangeButton");
		crystalNumLabel = GetNode<Label>("%CrystalNumLabel");
		readpaperTexture = GetNode<TextureRect>("%ReadpaperTexture");
		switchModeButton = GetNode<TextureButton>("%SwitchModeButton");
		readButton = GetNode<TextureButton>("%ReadButton");
		GetNode<TextureButton>("GroupButtonNode/AlmanacButton").Pressed += AlmanacButtonPressed;
		switchModeButton.Pressed += SwitchModeButtonPressed;
		exchangeButton.Pressed += ExchangeButtonPressed;
		GetNode<TextureButton>("%BackButton").Pressed += BackButtonPressed;
		readButton.Pressed += ReadButtonPressed;
		readpaperTexture.GuiInput += ReadpaperTextureGuiInput;
		dragMenu.CurrentIndexChanged += OnExchangeIndexChanged;
		VisibilityChanged += OnDialogVisibilityChanged;
		Setup();
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(dragMenu))
		{
			dragMenu.CurrentIndexChanged -= OnExchangeIndexChanged;
		}
		VisibilityChanged -= OnDialogVisibilityChanged;
		base._ExitTree();
	}

	private void OnExchangeIndexChanged(int index)
	{
		RefreshExchangeState();
	}

	private void OnDialogVisibilityChanged()
	{
		if (Visible)
		{
			RefreshExchangeState();
		}
	}

	private void RefreshExchangeState()
	{
		if (!GodotObject.IsInstanceValid(dragMenu) || filteredExchangeList.Count == 0)
		{
			return;
		}
		int index = Mathf.Clamp(dragMenu.currentIndex, 0, filteredExchangeList.Count - 1);
		Dictionary dictionary = filteredExchangeList[index];
		string text = dictionary.GetValueOrDefault("Type", "Packet").AsString();
		if (!(text == "Packet"))
		{
			if (text == "Feature")
			{
				if (GameSaveManager.Instance.GetFeatureValue(dictionary.GetValueOrDefault("Key", "").AsString()) > 0)
				{
					costLabel.Visible = false;
					exchangeButton.Disabled = true;
				}
				else
				{
					exchangeButton.Disabled = false;
					costLabel.Visible = true;
					costLabel.Text = dictionary.GetValueOrDefault("CrystalNum", 0).AsInt32().ToString();
				}
			}
		}
		else if (GameSaveManager.Instance.GetTowerDefensePacketValue(dictionary.GetValueOrDefault("Key", "").AsString())["Unlock"].AsBool())
		{
			costLabel.Visible = false;
			exchangeButton.Disabled = true;
		}
		else
		{
			exchangeButton.Disabled = false;
			costLabel.Visible = true;
			costLabel.Text = dictionary.GetValueOrDefault("CrystalNum", 0).AsInt32().ToString();
		}
		crystalNumLabel.Text = GameSaveManager.Instance.GetKeyValue("CrystalNum").AsInt32().ToString();
	}

	public void PlayButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
	}

	public void Setup()
	{
		foreach (Node child in dragMenu.GetChildren())
		{
			child.QueueFree();
		}
		Array array = ((Dictionary)ONLINE_LEVEL_EXCHANGE_RESOURCE.Data)["Exchange"].AsGodotArray();
		filteredExchangeList.Clear();
		string text = ((currentMode == "Plant") ? "Packet" : "Feature");
		foreach (Variant item in array)
		{
			Dictionary dictionary = item.AsGodotDictionary();
			if (dictionary.GetValueOrDefault("Type", "Packet").AsString() != text)
			{
				continue;
			}
			filteredExchangeList.Add(dictionary);
			string text2 = dictionary.GetValueOrDefault("Type", "Packet").AsString();
			if (!(text2 == "Packet"))
			{
				if (text2 == "Feature")
				{
					Control control = new Control();
					dragMenu.AddChild(control, forceReadableName: false, InternalMode.Disabled);
					TextureRect textureRect = new TextureRect();
					textureRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
					textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
					textureRect.Texture = GD.Load<Texture2D>(dictionary.GetValueOrDefault("Texture", "").AsString());
					textureRect.CustomMinimumSize = new Vector2(80f, 80f);
					textureRect.Position = new Vector2(-40f, -40f);
					control.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
				}
			}
			else
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(dictionary.GetValueOrDefault("Key", "").AsString());
				TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
				towerDefenseInGamePacketShow.onlyDraw = true;
				towerDefenseInGamePacketShow.setMobileLayout = true;
				dragMenu.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
				towerDefenseInGamePacketShow.Init(packetConfig);
			}
		}
		dragMenu.CallDeferred("SetPos", 0);
		Callable.From(RefreshExchangeState).CallDeferred();
	}

	public void BackButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		CloseDialog();
	}

	public async void AlmanacButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("Almanac");
		Visible = false;
		await dialogBoxBase.WaitForClose();
		Visible = true;
		RefreshExchangeState();
	}

	private Tween Fade(Control node, bool fadeIn)
	{
		Tween tween = CreateTween();
		if (fadeIn)
		{
			node.Modulate = new Color(1f, 1f, 1f, 0f);
			node.Visible = true;
			tween.TweenProperty(node, "modulate:a", 1f, 0.2);
		}
		else
		{
			tween.TweenProperty(node, "modulate:a", 0f, 0.2);
			tween.Finished += () =>
			{
				node.Visible = false;
				node.Modulate = new Color(1f, 1f, 1f);
			};
		}
		return tween;
	}

	public async void SwitchModeButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		await ToSignal(Fade(switchModeButton, fadeIn: false), "finished");
		currentMode = ((currentMode == "Plant") ? "Custom" : "Plant");
		switchModeButton.TextureNormal = ((currentMode == "Custom") ? TEXTURE_CUSTOM : TEXTURE_PLANT);
		Setup();
		Fade(switchModeButton, fadeIn: true);
	}

	public void ExchangeButtonPressed()
	{
		if (GameSaveManager.Instance.GetKeyValue("CrystalNum").AsInt32() >= int.Parse(costLabel.Text))
		{
			Dictionary dictionary = filteredExchangeList[dragMenu.currentIndex];
			string text = dictionary.GetValueOrDefault("Type", "Packet").AsString();
			if (!(text == "Packet"))
			{
				if (text == "Feature")
				{
					GameSaveManager.Instance.SetFeatureValue(dictionary.GetValueOrDefault("Key", "").AsString(), true);
				}
			}
			else
			{
				Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue(dictionary.GetValueOrDefault("Key", "").AsString());
				towerDefensePacketValue["Unlock"] = true;
				GameSaveManager.Instance.SetTowerDefensePacketValue(dictionary.GetValueOrDefault("Key", "").AsString(), towerDefensePacketValue);
			}
			GameSaveManager.Instance.SetKeyValue("CrystalNum", GameSaveManager.Instance.GetKeyValue("CrystalNum").AsInt32() - int.Parse(costLabel.Text));
			GameSaveManager.Instance.Save();
			RefreshExchangeState();
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]成功兑换[/font_size][/center]");
		}
		else
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]您的水晶不足[/font_size][/center]");
		}
	}

	public void ReadButtonPressed()
	{
		Fade(readpaperTexture, !readpaperTexture.Visible);
	}

	public void ReadpaperTextureGuiInput(InputEvent _event)
	{
		if (Input.IsActionJustPressed("Press") && readpaperTexture.Visible)
		{
			ReadButtonPressed();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExchangeIndexChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDialogVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshExchangeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Setup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AlmanacButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Fade, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "fadeIn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SwitchModeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExchangeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadpaperTextureGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
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
		if (method == MethodName.OnExchangeIndexChanged && args.Count == 1)
		{
			OnExchangeIndexChanged(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDialogVisibilityChanged && args.Count == 0)
		{
			OnDialogVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshExchangeState && args.Count == 0)
		{
			RefreshExchangeState();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayButtonPressed && args.Count == 0)
		{
			PlayButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.Setup && args.Count == 0)
		{
			Setup();
			ret = default;
			return true;
		}
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed && args.Count == 0)
		{
			AlmanacButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.Fade && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Tween>(Fade(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.SwitchModeButtonPressed && args.Count == 0)
		{
			SwitchModeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ExchangeButtonPressed && args.Count == 0)
		{
			ExchangeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadButtonPressed && args.Count == 0)
		{
			ReadButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadpaperTextureGuiInput && args.Count == 1)
		{
			ReadpaperTextureGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
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
		if (method == MethodName.OnExchangeIndexChanged)
		{
			return true;
		}
		if (method == MethodName.OnDialogVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshExchangeState)
		{
			return true;
		}
		if (method == MethodName.PlayButtonPressed)
		{
			return true;
		}
		if (method == MethodName.Setup)
		{
			return true;
		}
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed)
		{
			return true;
		}
		if (method == MethodName.Fade)
		{
			return true;
		}
		if (method == MethodName.SwitchModeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ExchangeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ReadButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ReadpaperTextureGuiInput)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dragMenu)
		{
			dragMenu = VariantUtils.ConvertTo<DragMenu>(in value);
			return true;
		}
		if (name == PropertyName.costLabel)
		{
			costLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.exchangeButton)
		{
			exchangeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.crystalNumLabel)
		{
			crystalNumLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.readpaperTexture)
		{
			readpaperTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.switchModeButton)
		{
			switchModeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.readButton)
		{
			readButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.currentGroup)
		{
			currentGroup = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.currentMode)
		{
			currentMode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dragMenu)
		{
			value = VariantUtils.CreateFrom(in dragMenu);
			return true;
		}
		if (name == PropertyName.costLabel)
		{
			value = VariantUtils.CreateFrom(in costLabel);
			return true;
		}
		if (name == PropertyName.exchangeButton)
		{
			value = VariantUtils.CreateFrom(in exchangeButton);
			return true;
		}
		if (name == PropertyName.crystalNumLabel)
		{
			value = VariantUtils.CreateFrom(in crystalNumLabel);
			return true;
		}
		if (name == PropertyName.readpaperTexture)
		{
			value = VariantUtils.CreateFrom(in readpaperTexture);
			return true;
		}
		if (name == PropertyName.switchModeButton)
		{
			value = VariantUtils.CreateFrom(in switchModeButton);
			return true;
		}
		if (name == PropertyName.readButton)
		{
			value = VariantUtils.CreateFrom(in readButton);
			return true;
		}
		if (name == PropertyName.currentGroup)
		{
			value = VariantUtils.CreateFrom(in currentGroup);
			return true;
		}
		if (name == PropertyName.currentMode)
		{
			value = VariantUtils.CreateFrom(in currentMode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.dragMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.costLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.exchangeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.crystalNumLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.readpaperTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.switchModeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.readButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentGroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dragMenu, Variant.From(in dragMenu));
		info.AddProperty(PropertyName.costLabel, Variant.From(in costLabel));
		info.AddProperty(PropertyName.exchangeButton, Variant.From(in exchangeButton));
		info.AddProperty(PropertyName.crystalNumLabel, Variant.From(in crystalNumLabel));
		info.AddProperty(PropertyName.readpaperTexture, Variant.From(in readpaperTexture));
		info.AddProperty(PropertyName.switchModeButton, Variant.From(in switchModeButton));
		info.AddProperty(PropertyName.readButton, Variant.From(in readButton));
		info.AddProperty(PropertyName.currentGroup, Variant.From(in currentGroup));
		info.AddProperty(PropertyName.currentMode, Variant.From(in currentMode));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dragMenu, out var value))
		{
			dragMenu = value.As<DragMenu>();
		}
		if (info.TryGetProperty(PropertyName.costLabel, out var value2))
		{
			costLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.exchangeButton, out var value3))
		{
			exchangeButton = value3.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.crystalNumLabel, out var value4))
		{
			crystalNumLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.readpaperTexture, out var value5))
		{
			readpaperTexture = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.switchModeButton, out var value6))
		{
			switchModeButton = value6.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.readButton, out var value7))
		{
			readButton = value7.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.currentGroup, out var value8))
		{
			currentGroup = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.currentMode, out var value9))
		{
			currentMode = value9.As<string>();
		}
	}
}
