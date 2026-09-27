using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Scene/AwardSettlement/AwardSettlement.cs")]
public class AwardSettlement : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitPacket = "InitPacket";

		public static readonly StringName InitCollectable = "InitCollectable";

		public static readonly StringName MenuButtonPressed = "MenuButtonPressed";

		public static readonly StringName NextLevelButtonPressed = "NextLevelButtonPressed";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName awardSettlementBackground = "awardSettlementBackground";

		public static readonly StringName awardShowNode = "awardShowNode";

		public static readonly StringName imageShowNode = "imageShowNode";

		public static readonly StringName typeLabel = "typeLabel";

		public static readonly StringName nameLabel = "nameLabel";

		public static readonly StringName describeLabel = "describeLabel";

		public static readonly StringName marker = "marker";

		public static readonly StringName imageSprite = "imageSprite";

		public static readonly StringName menuButton = "menuButton";

		public static readonly StringName nextLevelButton = "nextLevelButton";

		public static readonly StringName moreBackButton = "moreBackButton";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public TextureRect awardSettlementBackground;

	public Control awardShowNode;

	public Control imageShowNode;

	public Label typeLabel;

	public Label nameLabel;

	public Label describeLabel;

	public Control marker;

	public Sprite2D imageSprite;

	public NinePatchButtonBase menuButton;

	public NinePatchButtonBase nextLevelButton;

	public TextureButton moreBackButton;

	public override void _Ready()
	{
		awardSettlementBackground = GetNode<TextureRect>("%AwardSettlementBackground");
		awardShowNode = GetNode<Control>("%AwardShowNode");
		imageShowNode = GetNode<Control>("%ImageShowNode");
		typeLabel = GetNode<Label>("%TypeLabel");
		nameLabel = GetNode<Label>("%NameLabel");
		describeLabel = GetNode<Label>("%DescribeLabel");
		marker = GetNode<Control>("%Marker");
		imageSprite = GetNode<Sprite2D>("%ImageSprite");
		menuButton = GetNode<NinePatchButtonBase>("%MenuButton");
		nextLevelButton = GetNode<NinePatchButtonBase>("%NextLevelButton");
		moreBackButton = GetNode<TextureButton>("MoreBackButton");
		AudioManager.Instance.AudioPlay("ZenGarden", AudioManagerEnum.TYPE.MUSIC);
		if (Global.Instance.enterLevelMode == "ModLevel")
		{
			typeLabel.Text = "Mod 关卡完成";
			nameLabel.Text = XWModLevelSession.Current?.LevelSaveKey ?? "";
			describeLabel.Text = "进度独立保存在当前 Mod 中。";
		}
		switch ((TowerDefenseEnum.LEVEL_REWARDTYPE)Global.Instance.currentAwardType)
		{
		case TowerDefenseEnum.LEVEL_REWARDTYPE.PACKET:
			InitPacket(Global.Instance.currentAwardValue);
			break;
		case TowerDefenseEnum.LEVEL_REWARDTYPE.COLLECTABLE:
			InitCollectable(Global.Instance.currentAwardValue);
			break;
		}
		if (TowerDefenseManager.Instance.SetNextLevel(Global.Instance.currentLevelChoose, Global.Instance.currentChapterId, Global.Instance.currentLevelId) != null)
		{
			nextLevelButton.Visible = true;
			menuButton.Visible = false;
		}
		menuButton.OnPressed += MenuButtonPressed;
		nextLevelButton.OnPressed += NextLevelButtonPressed;
		moreBackButton.Pressed += MenuButtonPressed;
	}

	public void InitPacket(string packetName)
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		if (packetConfig.characterConfig is TowerDefensePlantConfig)
		{
			typeLabel.Text = "AWARD_PACKET_PLANT";
		}
		if (packetConfig.characterConfig is TowerDefenseZombieConfig)
		{
			typeLabel.Text = "AWARD_PACKET_ZOMBIE";
		}
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
		towerDefenseInGamePacketShow.setMobileLayout = GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool();
		marker.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
		towerDefenseInGamePacketShow.Init(packetConfig, skipGlobalChangeCost: true);
		towerDefenseInGamePacketShow.onlyDraw = true;
		towerDefenseInGamePacketShow.Scale = 1.25f * Vector2.One;
		nameLabel.Text = packetConfig.name;
		describeLabel.Text = packetConfig.describe;
	}

	public void InitCollectable(string collectableName)
	{
		CollectableConfig collectable = TowerDefenseManager.GetCollectable(collectableName);
		if (collectable.config is ShovelConfig shovelConfig)
		{
			typeLabel.Text = "AWARD_COLLECTABLE_SHOVEL";
			Sprite2D sprite2D = new Sprite2D();
			sprite2D.Texture = shovelConfig.texture;
			sprite2D.Scale = Vector2.One * (80f / (float)sprite2D.Texture.GetWidth());
			marker.AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
			nameLabel.Text = shovelConfig.name;
			describeLabel.Text = shovelConfig.describe;
		}
		if (collectable.config is AwardSettlementConfig awardSettlementConfig)
		{
			awardShowNode.Visible = false;
			imageShowNode.Visible = true;
			awardSettlementBackground.Texture = awardSettlementConfig.background;
			imageSprite.Texture = awardSettlementConfig.image;
			menuButton.Position = new Vector2(menuButton.Position.X, 520f);
			nextLevelButton.Position = new Vector2(nextLevelButton.Position.X, 520f);
		}
	}

	public void MenuButtonPressed()
	{
		Global.Instance.currentAwardMode = true;
		switch (Global.Instance.enterLevelMode)
		{
		case "ModLevel":
			Global.Instance.currentAwardMode = false;
			Global.Instance.currentLevelChoose = "__ModLevels";
			SceneManager.Instance.ChangeScene("LevelChoose");
			break;
		case "LevelChoose":
			SceneManager.Instance.ChangeScene("LevelChoose");
			break;
		case "DailyLevel":
			SceneManager.Instance.ChangeScene("MainMenu");
			break;
		case "DiyLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		case "LoadLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		case "OnlineLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		}
	}

	public async void NextLevelButtonPressed()
	{
		if (Global.Instance.enterLevelMode == "ModLevel" && Global.IsMultiplayerMode)
		{
			if (MultiPlayerManager.Instance.isHost)
			{
				MultiPlayerManager.Instance.SendSelectLevel(XWModLevelSession.Current.LevelSaveKey);
				await MultiPlayerManager.Instance.StartCompatibleGameAsync();
			}
		}
		else
		{
			SceneManager.Instance.ChangeScene("TowerDefense");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitCollectable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "collectableName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MenuButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitPacket && args.Count == 1)
		{
			InitPacket(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitCollectable && args.Count == 1)
		{
			InitCollectable(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MenuButtonPressed && args.Count == 0)
		{
			MenuButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NextLevelButtonPressed && args.Count == 0)
		{
			NextLevelButtonPressed();
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
		if (method == MethodName.InitPacket)
		{
			return true;
		}
		if (method == MethodName.InitCollectable)
		{
			return true;
		}
		if (method == MethodName.MenuButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NextLevelButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.awardSettlementBackground)
		{
			awardSettlementBackground = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.awardShowNode)
		{
			awardShowNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.imageShowNode)
		{
			imageShowNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.typeLabel)
		{
			typeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.nameLabel)
		{
			nameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.describeLabel)
		{
			describeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.marker)
		{
			marker = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.imageSprite)
		{
			imageSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.menuButton)
		{
			menuButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName.nextLevelButton)
		{
			nextLevelButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName.moreBackButton)
		{
			moreBackButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.awardSettlementBackground)
		{
			value = VariantUtils.CreateFrom(in awardSettlementBackground);
			return true;
		}
		if (name == PropertyName.awardShowNode)
		{
			value = VariantUtils.CreateFrom(in awardShowNode);
			return true;
		}
		if (name == PropertyName.imageShowNode)
		{
			value = VariantUtils.CreateFrom(in imageShowNode);
			return true;
		}
		if (name == PropertyName.typeLabel)
		{
			value = VariantUtils.CreateFrom(in typeLabel);
			return true;
		}
		if (name == PropertyName.nameLabel)
		{
			value = VariantUtils.CreateFrom(in nameLabel);
			return true;
		}
		if (name == PropertyName.describeLabel)
		{
			value = VariantUtils.CreateFrom(in describeLabel);
			return true;
		}
		if (name == PropertyName.marker)
		{
			value = VariantUtils.CreateFrom(in marker);
			return true;
		}
		if (name == PropertyName.imageSprite)
		{
			value = VariantUtils.CreateFrom(in imageSprite);
			return true;
		}
		if (name == PropertyName.menuButton)
		{
			value = VariantUtils.CreateFrom(in menuButton);
			return true;
		}
		if (name == PropertyName.nextLevelButton)
		{
			value = VariantUtils.CreateFrom(in nextLevelButton);
			return true;
		}
		if (name == PropertyName.moreBackButton)
		{
			value = VariantUtils.CreateFrom(in moreBackButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.awardSettlementBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardShowNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.imageShowNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.typeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.describeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.marker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.imageSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.menuButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nextLevelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moreBackButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.awardSettlementBackground, Variant.From(in awardSettlementBackground));
		info.AddProperty(PropertyName.awardShowNode, Variant.From(in awardShowNode));
		info.AddProperty(PropertyName.imageShowNode, Variant.From(in imageShowNode));
		info.AddProperty(PropertyName.typeLabel, Variant.From(in typeLabel));
		info.AddProperty(PropertyName.nameLabel, Variant.From(in nameLabel));
		info.AddProperty(PropertyName.describeLabel, Variant.From(in describeLabel));
		info.AddProperty(PropertyName.marker, Variant.From(in marker));
		info.AddProperty(PropertyName.imageSprite, Variant.From(in imageSprite));
		info.AddProperty(PropertyName.menuButton, Variant.From(in menuButton));
		info.AddProperty(PropertyName.nextLevelButton, Variant.From(in nextLevelButton));
		info.AddProperty(PropertyName.moreBackButton, Variant.From(in moreBackButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.awardSettlementBackground, out var value))
		{
			awardSettlementBackground = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.awardShowNode, out var value2))
		{
			awardShowNode = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.imageShowNode, out var value3))
		{
			imageShowNode = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.typeLabel, out var value4))
		{
			typeLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.nameLabel, out var value5))
		{
			nameLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.describeLabel, out var value6))
		{
			describeLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.marker, out var value7))
		{
			marker = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.imageSprite, out var value8))
		{
			imageSprite = value8.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.menuButton, out var value9))
		{
			menuButton = value9.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName.nextLevelButton, out var value10))
		{
			nextLevelButton = value10.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName.moreBackButton, out var value11))
		{
			moreBackButton = value11.As<TextureButton>();
		}
	}
}
