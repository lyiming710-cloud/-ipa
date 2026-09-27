using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/InformationPanel/InformationPanel.cs")]
public class InformationPanel : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitPacket = "InitPacket";

		public static readonly StringName InitShovel = "InitShovel";

		public static readonly StringName InitMower = "InitMower";

		public static readonly StringName EnterInformation = "EnterInformation";

		public static readonly StringName EnterCustom = "EnterCustom";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName DisposePreviewNode = "DisposePreviewNode";

		public static readonly StringName ReleaseLocalPreviewSubmissions = "ReleaseLocalPreviewSubmissions";

		public static readonly StringName TabButtonPressed = "TabButtonPressed";

		public static readonly StringName EquipmentButtonPressed = "EquipmentButtonPressed";

		public static readonly StringName SaveCurrentPacketState = "SaveCurrentPacketState";

		public static readonly StringName Freshbackground = "Freshbackground";

		public static readonly StringName FreshCustom = "FreshCustom";

		public static readonly StringName PreCustomButtonPressed = "PreCustomButtonPressed";

		public static readonly StringName NextCustomButtonPressed = "NextCustomButtonPressed";

		public static readonly StringName SetLocalPreviewRender = "SetLocalPreviewRender";

		public static readonly StringName FlattenLocalPreviewCanvasZ = "FlattenLocalPreviewCanvasZ";

		public new static readonly StringName SetLightMask = "SetLightMask";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName currentCustomId = "currentCustomId";

		public static readonly StringName tabTexture = "tabTexture";

		public static readonly StringName groundTexture = "groundTexture";

		public static readonly StringName informationNode = "informationNode";

		public static readonly StringName customNode = "customNode";

		public static readonly StringName spriteNode = "spriteNode";

		public static readonly StringName previewClip = "previewClip";

		public static readonly StringName nameLabel = "nameLabel";

		public static readonly StringName expressionLabel = "expressionLabel";

		public static readonly StringName handbookExpressionLabel = "handbookExpressionLabel";

		public static readonly StringName handbookStoryLabel = "handbookStoryLabel";

		public static readonly StringName informationCostLabel = "informationCostLabel";

		public static readonly StringName informationColdDownLabel = "informationColdDownLabel";

		public static readonly StringName scrollContainer = "scrollContainer";

		public static readonly StringName informationTabButton = "informationTabButton";

		public static readonly StringName informationTabLabel = "informationTabLabel";

		public static readonly StringName customTabButton = "customTabButton";

		public static readonly StringName customTabLabel = "customTabLabel";

		public static readonly StringName preCustomButton = "preCustomButton";

		public static readonly StringName equipmentButton = "equipmentButton";

		public static readonly StringName nextCustomButton = "nextCustomButton";

		public static readonly StringName nooneCustomLabel = "nooneCustomLabel";

		public static readonly StringName customNameLabel = "customNameLabel";

		public static readonly StringName customAccessLabel = "customAccessLabel";

		public static readonly StringName customStoryLabel = "customStoryLabel";

		public static readonly StringName currentCharcter = "currentCharcter";

		public static readonly StringName currentSpriteImage = "currentSpriteImage";

		public static readonly StringName currentPacketConfig = "currentPacketConfig";

		public static readonly StringName currentCustom = "currentCustom";

		public static readonly StringName _currentCustomId = "_currentCustomId";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static Texture2D _almanacGroundDay;

	private static Texture2D _almanacGroundNight;

	private static Texture2D _almanacGroundNightPool;

	private static Texture2D _almanacGroundPool;

	private TextureRect tabTexture;

	private TextureRect groundTexture;

	private Control informationNode;

	private Control customNode;

	private Control spriteNode;

	private Control previewClip;

	private Label nameLabel;

	private RichTextLabel expressionLabel;

	private RichTextLabel handbookExpressionLabel;

	private RichTextLabel handbookStoryLabel;

	private RichTextLabel informationCostLabel;

	private RichTextLabel informationColdDownLabel;

	private ScrollContainer scrollContainer;

	private TextureButton informationTabButton;

	private Label informationTabLabel;

	private TextureButton customTabButton;

	private Label customTabLabel;

	private SpriteBrightButton preCustomButton;

	private NinePatchButtonBase equipmentButton;

	private SpriteBrightButton nextCustomButton;

	private Label nooneCustomLabel;

	private RichTextLabel customNameLabel;

	private RichTextLabel customAccessLabel;

	private RichTextLabel customStoryLabel;

	public TowerDefenseCharacter currentCharcter;

	public Sprite2D currentSpriteImage;

	public TowerDefensePacketConfig currentPacketConfig;

	public string currentCustom = "";

	private int _currentCustomId;

	private static Texture2D ALMANAC_GROUND_DAY => _almanacGroundDay ?? (_almanacGroundDay = GD.Load<Texture2D>("uid://ceqaimccnt56q"));

	private static Texture2D ALMANAC_GROUND_NIGHT => _almanacGroundNight ?? (_almanacGroundNight = GD.Load<Texture2D>("uid://wjso8qh4jnie"));

	private static Texture2D ALMANAC_GROUND_NIGHT_POOL => _almanacGroundNightPool ?? (_almanacGroundNightPool = GD.Load<Texture2D>("uid://y166oxgfiohu"));

	private static Texture2D ALMANAC_GROUND_POOL => _almanacGroundPool ?? (_almanacGroundPool = GD.Load<Texture2D>("uid://cpfhqdus53fr0"));

	public int currentCustomId
	{
		get
		{
			return _currentCustomId;
		}
		set
		{
			_currentCustomId = value;
			FreshCustom();
		}
	}

	public override void _Ready()
	{
		tabTexture = GetNode<TextureRect>("%TabTexture");
		groundTexture = GetNode<TextureRect>("%GroundTexture");
		informationNode = GetNode<Control>("%InformationNode");
		customNode = GetNode<Control>("%CustomNode");
		spriteNode = GetNode<Control>("%SpriteNode");
		previewClip = spriteNode.GetParent() as Control;
		nameLabel = GetNode<Label>("%NameLabel");
		expressionLabel = GetNode<RichTextLabel>("%ExpressionLabel");
		handbookExpressionLabel = GetNode<RichTextLabel>("%HandbookExpressionLabel");
		handbookStoryLabel = GetNode<RichTextLabel>("%HandbookStoryLabel");
		informationCostLabel = GetNode<RichTextLabel>("%InformationCostLabel");
		informationColdDownLabel = GetNode<RichTextLabel>("%InformationColdDownLabel");
		scrollContainer = GetNode<ScrollContainer>("%ScrollContainer");
		informationTabButton = GetNode<TextureButton>("%InformationTabButton");
		informationTabLabel = GetNode<Label>("%InformationTabLabel");
		customTabButton = GetNode<TextureButton>("%CustomTabButton");
		customTabLabel = GetNode<Label>("%CustomTabLabel");
		preCustomButton = GetNode<SpriteBrightButton>("%PreCustomButton");
		equipmentButton = GetNode<NinePatchButtonBase>("%EquipmentButton");
		nextCustomButton = GetNode<SpriteBrightButton>("%NextCustomButton");
		preCustomButton.OnPressed += PreCustomButtonPressed;
		equipmentButton.OnPressed += EquipmentButtonPressed;
		nextCustomButton.OnPressed += NextCustomButtonPressed;
		nooneCustomLabel = GetNode<Label>("%NooneCustomLabel");
		customNameLabel = GetNode<RichTextLabel>("%CustomNameLabel");
		customAccessLabel = GetNode<RichTextLabel>("%CustomAccessLabel");
		customStoryLabel = GetNode<RichTextLabel>("%CustomStoryLabel");
		GetNode<BaseButton>("%InformationTabButton").Pressed += TabButtonPressed;
		GetNode<BaseButton>("%CustomTabButton").Pressed += TabButtonPressed;
	}

	public void InitPacket(TowerDefensePacketConfig packetConfig)
	{
		Clear();
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(packetConfig.characterConfig))
		{
			GD.PushError("[InformationPanel] Cannot preview an invalid packet or character config.");
			return;
		}
		TowerDefenseCharacterConfig characterConfig = packetConfig.characterConfig;
		if (string.IsNullOrWhiteSpace(characterConfig.name))
		{
			GD.PushError("[InformationPanel] Packet '" + packetConfig.saveKey + "' has an empty character name.");
			return;
		}
		Freshbackground(characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER), characterConfig.sleepTime == "Day");
		PackedScene chacraterScene = TowerDefenseManager.GetChacraterScene(characterConfig.name);
		if (!GodotObject.IsInstanceValid(chacraterScene))
		{
			GD.PushError("[InformationPanel] Character scene not found for '" + characterConfig.name + "'.");
			return;
		}
		Node node = chacraterScene.Instantiate(PackedScene.GenEditState.Disabled);
		currentCharcter = node as TowerDefenseCharacter;
		if (!GodotObject.IsInstanceValid(currentCharcter) || !GodotObject.IsInstanceValid(currentCharcter.config) || !GodotObject.IsInstanceValid(currentCharcter.sprite))
		{
			GD.PushError("[InformationPanel] Character preview '" + characterConfig.name + "' has incompatible serialized properties.");
			node?.QueueFree();
			currentCharcter = null;
			return;
		}
		currentPacketConfig = packetConfig;
		currentCharcter.inGame = false;
		currentCharcter.packet = packetConfig;
		SetLocalPreviewRender(currentCharcter);
		spriteNode.AddChild(currentCharcter, forceReadableName: false, InternalMode.Disabled);
		SetLocalPreviewRender(currentCharcter);
		currentCharcter.sprite.ProcessMode = ProcessModeEnum.Always;
		currentCharcter.SetMainStateMachineDispatchEnabled(enabled: false);
		currentCharcter.sprite.SetAnimation(packetConfig.packetAnimeClip);
		currentCharcter.gridPos = new Vector2I(currentCharcter.gridPos.X, 10);
		currentCharcter.ZIndex = 0;
		currentCharcter.SetLogicalGlobalPosition(spriteNode.GetGlobalTransform() * packetConfig.handbookPacketAnimeOffset);
		ShadowComponent shadowComponent = currentCharcter.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			currentCharcter.shadowComponent.preferMultiMesh = false;
			currentCharcter.shadowComponent.Init();
			currentCharcter.shadowComponent.BatchUpdate();
		}
		currentCharcter.shadowSprite.ZIndex = -1;
		FlattenLocalPreviewCanvasZ(currentCharcter.sprite);
		if (characterConfig is TowerDefensePlantConfig towerDefensePlantConfig)
		{
			Vector2 logicalGlobalPosition = currentCharcter.GetLogicalGlobalPosition();
			foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
			{
				logicalGlobalPosition.X -= 40 * item.X;
			}
			currentCharcter.SetLogicalGlobalPosition(logicalGlobalPosition);
		}
		if (characterConfig.armorData != null && packetConfig.initArmor != null && packetConfig.initArmor.Count > 0)
		{
			foreach (string item2 in packetConfig.initArmor)
			{
				ArmorSlotConfig slotConfig = characterConfig.armorData.GetSlotConfig(item2);
				TowerDefenseArmorTypeData typeData = characterConfig.armorData.GetTypeData(item2);
				if (typeData == null)
				{
					continue;
				}
				string replaceMethod = slotConfig.replaceMethod;
				if (!(replaceMethod == "Media"))
				{
					if (replaceMethod == "Sprite")
					{
						AdobeAnimatePart adobeAnimatePart = CharacterArmorData.CreateArmorPartNode(currentCharcter.sprite, slotConfig, typeData);
						if (GodotObject.IsInstanceValid(adobeAnimatePart))
						{
							adobeAnimatePart.LightMask = 0;
						}
					}
				}
				else
				{
					characterConfig.armorData.OpenArmorFliters(currentCharcter.sprite, item2);
					characterConfig.armorData.SetArmorReplace(currentCharcter.sprite, item2, 0);
				}
			}
		}
		SetLocalPreviewRender(currentCharcter);
		FlattenLocalPreviewCanvasZ(currentCharcter.sprite);
		scrollContainer.ScrollVertical = 0;
		if (characterConfig is TowerDefensePlantConfig)
		{
			scrollContainer.Size = new Vector2(scrollContainer.Size.X, 210f);
		}
		if (characterConfig is TowerDefenseZombieConfig)
		{
			scrollContainer.Size = new Vector2(scrollContainer.Size.X, 140f);
		}
		nameLabel.Text = packetConfig.name;
		expressionLabel.Visible = packetConfig.describe != "";
		expressionLabel.Text = $"[color=2f375e]{Tr(packetConfig.describe)}[/color]";
		handbookExpressionLabel.Visible = packetConfig.handbookDescribe != "";
		handbookExpressionLabel.Text = packetConfig.handbookDescribe;
		handbookStoryLabel.Visible = packetConfig.handbookStory != "";
		handbookStoryLabel.Text = packetConfig.handbookStory;
		informationCostLabel.Text = $"[color=ab5e57]花费:{characterConfig.cost}[/color]";
		informationColdDownLabel.Text = $"[color=ab5e57]冷却时间:{characterConfig.packetCooldown:F1}[/color]";
		SetLightMask(currentCharcter);
		if (customTabButton.ButtonPressed)
		{
			EnterCustom();
		}
	}

	public void InitShovel(ShovelConfig shovelConfig)
	{
		Clear();
		Freshbackground();
		scrollContainer.Size = new Vector2(scrollContainer.Size.X, 180f);
		tabTexture.Visible = false;
		informationTabButton.Visible = false;
		customTabButton.Visible = false;
		currentSpriteImage = new Sprite2D();
		currentSpriteImage.Texture = shovelConfig.texture;
		currentSpriteImage.LightMask = 0;
		spriteNode.AddChild(currentSpriteImage, forceReadableName: false, InternalMode.Disabled);
		nameLabel.Text = shovelConfig.name;
		expressionLabel.Visible = shovelConfig.describe != "";
		expressionLabel.Text = $"[color=2f375e]{Tr(shovelConfig.describe)}[/color]";
		handbookExpressionLabel.Visible = shovelConfig.handbookDescribe != "";
		handbookExpressionLabel.Text = shovelConfig.handbookDescribe;
		handbookStoryLabel.Visible = shovelConfig.handbookStory != "";
		handbookStoryLabel.Text = shovelConfig.handbookStory;
		informationCostLabel.Text = "";
		informationColdDownLabel.Text = "";
	}

	public void InitMower(MowerConfig mowerConfig)
	{
		Clear();
		Freshbackground();
		scrollContainer.Size = new Vector2(scrollContainer.Size.X, 180f);
		tabTexture.Visible = false;
		informationTabButton.Visible = false;
		customTabButton.Visible = false;
		currentSpriteImage = new Sprite2D();
		currentSpriteImage.Texture = mowerConfig.texture;
		currentSpriteImage.Scale = Vector2.One * 0.6f;
		currentSpriteImage.LightMask = 0;
		spriteNode.AddChild(currentSpriteImage, forceReadableName: false, InternalMode.Disabled);
		nameLabel.Text = mowerConfig.name;
		expressionLabel.Visible = mowerConfig.describe != "";
		expressionLabel.Text = $"[color=2f375e]{Tr(mowerConfig.describe)}[/color]";
		handbookExpressionLabel.Visible = mowerConfig.handbookDescribe != "";
		handbookExpressionLabel.Text = mowerConfig.handbookDescribe;
		handbookStoryLabel.Visible = mowerConfig.handbookStory != "";
		handbookStoryLabel.Text = mowerConfig.handbookStory;
		informationCostLabel.Text = "";
		informationColdDownLabel.Text = "";
	}

	public void EnterInformation()
	{
		if (currentPacketConfig.characterConfig.customData != null)
		{
			Dictionary packetState = XWModPlayerProgressService.GetPacketState(currentPacketConfig.saveKey);
			if (packetState.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Custom", "")
				.AsString() != "")
			{
				currentCharcter.currentCustom = new Array<string> { (string)packetState["Key"].AsGodotDictionary()["Custom"] };
			}
			else
			{
				currentCharcter.currentCustom = new Array<string>();
			}
		}
		SetLightMask(currentCharcter);
	}

	public async void EnterCustom()
	{
		TowerDefenseCharacterConfig characterConfig = currentPacketConfig.characterConfig;
		if (characterConfig.customData != null)
		{
			nooneCustomLabel.Visible = false;
			customNode.Visible = true;
			Dictionary dictionary = XWModPlayerProgressService.GetPacketState(currentPacketConfig.saveKey).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
			if (dictionary.GetValueOrDefault("Custom", "").AsString() != "")
			{
				for (int i = 0; i < characterConfig.customData.customList.Count; i++)
				{
					if (characterConfig.customData.customList[i].customName == dictionary["Custom"].AsString())
					{
						currentCustomId = i;
						break;
					}
				}
			}
			else
			{
				currentCustomId = 0;
			}
		}
		else
		{
			nooneCustomLabel.Visible = true;
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			customNode.Visible = false;
		}
	}

	public void Clear()
	{
		DisposePreviewNode(currentSpriteImage);
		DisposePreviewNode(currentCharcter);
		currentSpriteImage = null;
		currentCharcter = null;
		currentPacketConfig = null;
	}

	private static void DisposePreviewNode(Node previewNode)
	{
		if (GodotObject.IsInstanceValid(previewNode))
		{
			ReleaseLocalPreviewSubmissions(previewNode);
			Node parent = previewNode.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(previewNode);
			}
			previewNode.QueueFree();
		}
	}

	private static void ReleaseLocalPreviewSubmissions(Node node)
	{
		if (node is AdobeAnimateSprite sprite)
		{
			AdobeAnimateRenderManager.ReleaseImmediateSubmission(sprite);
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			ReleaseLocalPreviewSubmissions(child);
		}
	}

	public void TabButtonPressed()
	{
		nooneCustomLabel.Visible = false;
		informationNode.Visible = false;
		customNode.Visible = false;
		if (informationTabButton.ButtonPressed)
		{
			EnterInformation();
			informationNode.Visible = true;
			informationTabLabel.AddThemeColorOverride("font_color", Colors.Yellow);
		}
		else
		{
			informationTabLabel.AddThemeColorOverride("font_color", new Color("c35b25"));
		}
		if (customTabButton.ButtonPressed)
		{
			EnterCustom();
			customNode.Visible = true;
			customTabLabel.AddThemeColorOverride("font_color", Colors.Yellow);
		}
		else
		{
			customTabLabel.AddThemeColorOverride("font_color", new Color("c35b25"));
		}
	}

	public void EquipmentButtonPressed()
	{
		string text = equipmentButton.text;
		if (!(text == "装备装扮"))
		{
			if (text == "卸下装扮")
			{
				Dictionary packetState = XWModPlayerProgressService.GetPacketState(currentPacketConfig.saveKey);
				if (!packetState.ContainsKey("Key"))
				{
					packetState["Key"] = new Dictionary();
				}
				packetState["Key"].AsGodotDictionary()["Custom"] = "";
				SaveCurrentPacketState(packetState);
				BattleEventBus.Instance.EmitCharacterSkinSwitched(currentPacketConfig.saveKey, "");
				equipmentButton.text = "装备装扮";
			}
		}
		else
		{
			Dictionary packetState2 = XWModPlayerProgressService.GetPacketState(currentPacketConfig.saveKey);
			if (!packetState2.ContainsKey("Key"))
			{
				packetState2["Key"] = new Dictionary();
			}
			packetState2["Key"].AsGodotDictionary()["Custom"] = currentCustom;
			SaveCurrentPacketState(packetState2);
			BattleEventBus.Instance.EmitCharacterSkinSwitched(currentPacketConfig.saveKey, currentCustom);
			equipmentButton.text = "卸下装扮";
		}
	}

	private void SaveCurrentPacketState(Dictionary value)
	{
		if (XWModContentCatalog.GetContentOwner("Packet", currentPacketConfig.saveKey).Length > 0)
		{
			XWModPlayerProgressService.SetPacketState(currentPacketConfig.saveKey, value);
		}
		else
		{
			GameSaveManager.Instance.SetTowerDefensePacketValue(currentPacketConfig.saveKey, value);
		}
	}

	public void Freshbackground(bool isWater = false, bool isNight = false)
	{
		if (!isWater)
		{
			if (!isNight)
			{
				groundTexture.Texture = ALMANAC_GROUND_DAY;
			}
			else
			{
				groundTexture.Texture = ALMANAC_GROUND_NIGHT;
			}
		}
		else if (!isNight)
		{
			groundTexture.Texture = ALMANAC_GROUND_POOL;
		}
		else
		{
			groundTexture.Texture = ALMANAC_GROUND_NIGHT_POOL;
		}
	}

	public void FreshCustom()
	{
		TowerDefenseCharacterConfig characterConfig = currentPacketConfig.characterConfig;
		Dictionary packetState = XWModPlayerProgressService.GetPacketState(currentPacketConfig.saveKey);
		currentCustom = characterConfig.customData.customList[currentCustomId].customName;
		currentCharcter.currentCustom = new Array<string> { currentCustom };
		SetLightMask(currentCharcter);
		string openKey = characterConfig.customData.customList[currentCustomId].openKey;
		string contentOwner = XWModContentCatalog.GetContentOwner("Packet", currentPacketConfig.saveKey);
		if (CommandManager.Instance.debugOpenAllCustom || openKey == "" || ((contentOwner.Length > 0) ? XWModPlayerProgressService.IsUnlocked(contentOwner, "Custom", openKey) : (GameSaveManager.Instance.GetFeatureValue(openKey) > 0)))
		{
			equipmentButton.disable = false;
			if (packetState.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Custom", "")
				.AsString() == currentCustom)
			{
				equipmentButton.text = "卸下装扮";
			}
			else
			{
				equipmentButton.text = "装备装扮";
			}
		}
		else
		{
			equipmentButton.disable = true;
			equipmentButton.text = "未获得";
		}
		customNameLabel.Text = $"[color=ff3eff]{Tr(characterConfig.customData.customList[currentCustomId].customHandbookName)}[/color]";
		customAccessLabel.Text = $"[color=red]获取方式[/color]:{Tr(characterConfig.customData.customList[currentCustomId].customHandbookAccess)}";
		customStoryLabel.Text = characterConfig.customData.customList[currentCustomId].customHandbookStory;
	}

	public void PreCustomButtonPressed()
	{
		TowerDefenseCharacterConfig characterConfig = currentPacketConfig.characterConfig;
		currentCustomId = (currentCustomId - 1 + characterConfig.customData.customList.Count) % characterConfig.customData.customList.Count;
	}

	public void NextCustomButtonPressed()
	{
		TowerDefenseCharacterConfig characterConfig = currentPacketConfig.characterConfig;
		currentCustomId = (currentCustomId + 1 + characterConfig.customData.customList.Count) % characterConfig.customData.customList.Count;
	}

	private void SetLocalPreviewRender(Node node)
	{
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.forceLocalRender = true;
			adobeAnimateSprite.SetRenderClipControl(previewClip);
			adobeAnimateSprite.ProcessMode = ProcessModeEnum.Always;
			adobeAnimateSprite.pause = false;
			adobeAnimateSprite.UpdateChild();
			adobeAnimateSprite.QueueRedraw();
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			SetLocalPreviewRender(child);
		}
	}

	private void FlattenLocalPreviewCanvasZ(Node node)
	{
		if (node is CanvasItem canvasItem)
		{
			canvasItem.ZIndex = 0;
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			FlattenLocalPreviewCanvasZ(child);
		}
	}

	private void SetLightMask(Node node)
	{
		if (node is CanvasItem canvasItem)
		{
			canvasItem.LightMask = 0;
		}
		if (node is Light2D light2D)
		{
			light2D.Visible = false;
		}
		foreach (Node child in node.GetChildren(includeInternal: true))
		{
			SetLightMask(child);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitMower, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mowerConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnterInformation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterCustom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposePreviewNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "previewNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseLocalPreviewSubmissions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.TabButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EquipmentButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCurrentPacketState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Freshbackground, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isWater", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isNight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreshCustom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreCustomButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextCustomButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetLocalPreviewRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FlattenLocalPreviewCanvasZ, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetLightMask, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName.InitPacket && args.Count == 1)
		{
			InitPacket(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitShovel && args.Count == 1)
		{
			InitShovel(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitMower && args.Count == 1)
		{
			InitMower(VariantUtils.ConvertTo<MowerConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterInformation && args.Count == 0)
		{
			EnterInformation();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterCustom && args.Count == 0)
		{
			EnterCustom();
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposePreviewNode && args.Count == 1)
		{
			DisposePreviewNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseLocalPreviewSubmissions && args.Count == 1)
		{
			ReleaseLocalPreviewSubmissions(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TabButtonPressed && args.Count == 0)
		{
			TabButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.EquipmentButtonPressed && args.Count == 0)
		{
			EquipmentButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCurrentPacketState && args.Count == 1)
		{
			SaveCurrentPacketState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Freshbackground && args.Count == 2)
		{
			Freshbackground(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreshCustom && args.Count == 0)
		{
			FreshCustom();
			ret = default;
			return true;
		}
		if (method == MethodName.PreCustomButtonPressed && args.Count == 0)
		{
			PreCustomButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.NextCustomButtonPressed && args.Count == 0)
		{
			NextCustomButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SetLocalPreviewRender && args.Count == 1)
		{
			SetLocalPreviewRender(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlattenLocalPreviewCanvasZ && args.Count == 1)
		{
			FlattenLocalPreviewCanvasZ(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLightMask && args.Count == 1)
		{
			SetLightMask(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DisposePreviewNode && args.Count == 1)
		{
			DisposePreviewNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseLocalPreviewSubmissions && args.Count == 1)
		{
			ReleaseLocalPreviewSubmissions(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
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
		if (method == MethodName.InitPacket)
		{
			return true;
		}
		if (method == MethodName.InitShovel)
		{
			return true;
		}
		if (method == MethodName.InitMower)
		{
			return true;
		}
		if (method == MethodName.EnterInformation)
		{
			return true;
		}
		if (method == MethodName.EnterCustom)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.DisposePreviewNode)
		{
			return true;
		}
		if (method == MethodName.ReleaseLocalPreviewSubmissions)
		{
			return true;
		}
		if (method == MethodName.TabButtonPressed)
		{
			return true;
		}
		if (method == MethodName.EquipmentButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentPacketState)
		{
			return true;
		}
		if (method == MethodName.Freshbackground)
		{
			return true;
		}
		if (method == MethodName.FreshCustom)
		{
			return true;
		}
		if (method == MethodName.PreCustomButtonPressed)
		{
			return true;
		}
		if (method == MethodName.NextCustomButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SetLocalPreviewRender)
		{
			return true;
		}
		if (method == MethodName.FlattenLocalPreviewCanvasZ)
		{
			return true;
		}
		if (method == MethodName.SetLightMask)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.currentCustomId)
		{
			currentCustomId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.tabTexture)
		{
			tabTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.groundTexture)
		{
			groundTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.informationNode)
		{
			informationNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.customNode)
		{
			customNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.spriteNode)
		{
			spriteNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.previewClip)
		{
			previewClip = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.nameLabel)
		{
			nameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.expressionLabel)
		{
			expressionLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.handbookExpressionLabel)
		{
			handbookExpressionLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.handbookStoryLabel)
		{
			handbookStoryLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.informationCostLabel)
		{
			informationCostLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.informationColdDownLabel)
		{
			informationColdDownLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.scrollContainer)
		{
			scrollContainer = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName.informationTabButton)
		{
			informationTabButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.informationTabLabel)
		{
			informationTabLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.customTabButton)
		{
			customTabButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.customTabLabel)
		{
			customTabLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.preCustomButton)
		{
			preCustomButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		if (name == PropertyName.equipmentButton)
		{
			equipmentButton = VariantUtils.ConvertTo<NinePatchButtonBase>(in value);
			return true;
		}
		if (name == PropertyName.nextCustomButton)
		{
			nextCustomButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		if (name == PropertyName.nooneCustomLabel)
		{
			nooneCustomLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.customNameLabel)
		{
			customNameLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.customAccessLabel)
		{
			customAccessLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.customStoryLabel)
		{
			customStoryLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.currentCharcter)
		{
			currentCharcter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.currentSpriteImage)
		{
			currentSpriteImage = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.currentPacketConfig)
		{
			currentPacketConfig = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.currentCustom)
		{
			currentCustom = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._currentCustomId)
		{
			_currentCustomId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.currentCustomId)
		{
			value = VariantUtils.CreateFrom<int>(currentCustomId);
			return true;
		}
		if (name == PropertyName.tabTexture)
		{
			value = VariantUtils.CreateFrom(in tabTexture);
			return true;
		}
		if (name == PropertyName.groundTexture)
		{
			value = VariantUtils.CreateFrom(in groundTexture);
			return true;
		}
		if (name == PropertyName.informationNode)
		{
			value = VariantUtils.CreateFrom(in informationNode);
			return true;
		}
		if (name == PropertyName.customNode)
		{
			value = VariantUtils.CreateFrom(in customNode);
			return true;
		}
		if (name == PropertyName.spriteNode)
		{
			value = VariantUtils.CreateFrom(in spriteNode);
			return true;
		}
		if (name == PropertyName.previewClip)
		{
			value = VariantUtils.CreateFrom(in previewClip);
			return true;
		}
		if (name == PropertyName.nameLabel)
		{
			value = VariantUtils.CreateFrom(in nameLabel);
			return true;
		}
		if (name == PropertyName.expressionLabel)
		{
			value = VariantUtils.CreateFrom(in expressionLabel);
			return true;
		}
		if (name == PropertyName.handbookExpressionLabel)
		{
			value = VariantUtils.CreateFrom(in handbookExpressionLabel);
			return true;
		}
		if (name == PropertyName.handbookStoryLabel)
		{
			value = VariantUtils.CreateFrom(in handbookStoryLabel);
			return true;
		}
		if (name == PropertyName.informationCostLabel)
		{
			value = VariantUtils.CreateFrom(in informationCostLabel);
			return true;
		}
		if (name == PropertyName.informationColdDownLabel)
		{
			value = VariantUtils.CreateFrom(in informationColdDownLabel);
			return true;
		}
		if (name == PropertyName.scrollContainer)
		{
			value = VariantUtils.CreateFrom(in scrollContainer);
			return true;
		}
		if (name == PropertyName.informationTabButton)
		{
			value = VariantUtils.CreateFrom(in informationTabButton);
			return true;
		}
		if (name == PropertyName.informationTabLabel)
		{
			value = VariantUtils.CreateFrom(in informationTabLabel);
			return true;
		}
		if (name == PropertyName.customTabButton)
		{
			value = VariantUtils.CreateFrom(in customTabButton);
			return true;
		}
		if (name == PropertyName.customTabLabel)
		{
			value = VariantUtils.CreateFrom(in customTabLabel);
			return true;
		}
		if (name == PropertyName.preCustomButton)
		{
			value = VariantUtils.CreateFrom(in preCustomButton);
			return true;
		}
		if (name == PropertyName.equipmentButton)
		{
			value = VariantUtils.CreateFrom(in equipmentButton);
			return true;
		}
		if (name == PropertyName.nextCustomButton)
		{
			value = VariantUtils.CreateFrom(in nextCustomButton);
			return true;
		}
		if (name == PropertyName.nooneCustomLabel)
		{
			value = VariantUtils.CreateFrom(in nooneCustomLabel);
			return true;
		}
		if (name == PropertyName.customNameLabel)
		{
			value = VariantUtils.CreateFrom(in customNameLabel);
			return true;
		}
		if (name == PropertyName.customAccessLabel)
		{
			value = VariantUtils.CreateFrom(in customAccessLabel);
			return true;
		}
		if (name == PropertyName.customStoryLabel)
		{
			value = VariantUtils.CreateFrom(in customStoryLabel);
			return true;
		}
		if (name == PropertyName.currentCharcter)
		{
			value = VariantUtils.CreateFrom(in currentCharcter);
			return true;
		}
		if (name == PropertyName.currentSpriteImage)
		{
			value = VariantUtils.CreateFrom(in currentSpriteImage);
			return true;
		}
		if (name == PropertyName.currentPacketConfig)
		{
			value = VariantUtils.CreateFrom(in currentPacketConfig);
			return true;
		}
		if (name == PropertyName.currentCustom)
		{
			value = VariantUtils.CreateFrom(in currentCustom);
			return true;
		}
		if (name == PropertyName._currentCustomId)
		{
			value = VariantUtils.CreateFrom(in _currentCustomId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.tabTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.groundTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.informationNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.customNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spriteNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.previewClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.expressionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.handbookExpressionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.handbookStoryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.informationCostLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.informationColdDownLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.scrollContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.informationTabButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.informationTabLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.customTabButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.customTabLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.preCustomButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.equipmentButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nextCustomButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nooneCustomLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.customNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.customAccessLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.customStoryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentCharcter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentSpriteImage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.currentPacketConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentCustom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentCustomId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentCustomId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.currentCustomId, Variant.From<int>(currentCustomId));
		info.AddProperty(PropertyName.tabTexture, Variant.From(in tabTexture));
		info.AddProperty(PropertyName.groundTexture, Variant.From(in groundTexture));
		info.AddProperty(PropertyName.informationNode, Variant.From(in informationNode));
		info.AddProperty(PropertyName.customNode, Variant.From(in customNode));
		info.AddProperty(PropertyName.spriteNode, Variant.From(in spriteNode));
		info.AddProperty(PropertyName.previewClip, Variant.From(in previewClip));
		info.AddProperty(PropertyName.nameLabel, Variant.From(in nameLabel));
		info.AddProperty(PropertyName.expressionLabel, Variant.From(in expressionLabel));
		info.AddProperty(PropertyName.handbookExpressionLabel, Variant.From(in handbookExpressionLabel));
		info.AddProperty(PropertyName.handbookStoryLabel, Variant.From(in handbookStoryLabel));
		info.AddProperty(PropertyName.informationCostLabel, Variant.From(in informationCostLabel));
		info.AddProperty(PropertyName.informationColdDownLabel, Variant.From(in informationColdDownLabel));
		info.AddProperty(PropertyName.scrollContainer, Variant.From(in scrollContainer));
		info.AddProperty(PropertyName.informationTabButton, Variant.From(in informationTabButton));
		info.AddProperty(PropertyName.informationTabLabel, Variant.From(in informationTabLabel));
		info.AddProperty(PropertyName.customTabButton, Variant.From(in customTabButton));
		info.AddProperty(PropertyName.customTabLabel, Variant.From(in customTabLabel));
		info.AddProperty(PropertyName.preCustomButton, Variant.From(in preCustomButton));
		info.AddProperty(PropertyName.equipmentButton, Variant.From(in equipmentButton));
		info.AddProperty(PropertyName.nextCustomButton, Variant.From(in nextCustomButton));
		info.AddProperty(PropertyName.nooneCustomLabel, Variant.From(in nooneCustomLabel));
		info.AddProperty(PropertyName.customNameLabel, Variant.From(in customNameLabel));
		info.AddProperty(PropertyName.customAccessLabel, Variant.From(in customAccessLabel));
		info.AddProperty(PropertyName.customStoryLabel, Variant.From(in customStoryLabel));
		info.AddProperty(PropertyName.currentCharcter, Variant.From(in currentCharcter));
		info.AddProperty(PropertyName.currentSpriteImage, Variant.From(in currentSpriteImage));
		info.AddProperty(PropertyName.currentPacketConfig, Variant.From(in currentPacketConfig));
		info.AddProperty(PropertyName.currentCustom, Variant.From(in currentCustom));
		info.AddProperty(PropertyName._currentCustomId, Variant.From(in _currentCustomId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.currentCustomId, out var value))
		{
			currentCustomId = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.tabTexture, out var value2))
		{
			tabTexture = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.groundTexture, out var value3))
		{
			groundTexture = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.informationNode, out var value4))
		{
			informationNode = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.customNode, out var value5))
		{
			customNode = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.spriteNode, out var value6))
		{
			spriteNode = value6.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.previewClip, out var value7))
		{
			previewClip = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.nameLabel, out var value8))
		{
			nameLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.expressionLabel, out var value9))
		{
			expressionLabel = value9.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.handbookExpressionLabel, out var value10))
		{
			handbookExpressionLabel = value10.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.handbookStoryLabel, out var value11))
		{
			handbookStoryLabel = value11.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.informationCostLabel, out var value12))
		{
			informationCostLabel = value12.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.informationColdDownLabel, out var value13))
		{
			informationColdDownLabel = value13.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.scrollContainer, out var value14))
		{
			scrollContainer = value14.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName.informationTabButton, out var value15))
		{
			informationTabButton = value15.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.informationTabLabel, out var value16))
		{
			informationTabLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.customTabButton, out var value17))
		{
			customTabButton = value17.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.customTabLabel, out var value18))
		{
			customTabLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.preCustomButton, out var value19))
		{
			preCustomButton = value19.As<SpriteBrightButton>();
		}
		if (info.TryGetProperty(PropertyName.equipmentButton, out var value20))
		{
			equipmentButton = value20.As<NinePatchButtonBase>();
		}
		if (info.TryGetProperty(PropertyName.nextCustomButton, out var value21))
		{
			nextCustomButton = value21.As<SpriteBrightButton>();
		}
		if (info.TryGetProperty(PropertyName.nooneCustomLabel, out var value22))
		{
			nooneCustomLabel = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.customNameLabel, out var value23))
		{
			customNameLabel = value23.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.customAccessLabel, out var value24))
		{
			customAccessLabel = value24.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.customStoryLabel, out var value25))
		{
			customStoryLabel = value25.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.currentCharcter, out var value26))
		{
			currentCharcter = value26.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.currentSpriteImage, out var value27))
		{
			currentSpriteImage = value27.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.currentPacketConfig, out var value28))
		{
			currentPacketConfig = value28.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.currentCustom, out var value29))
		{
			currentCustom = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName._currentCustomId, out var value30))
		{
			_currentCustomId = value30.As<int>();
		}
	}
}
