using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/TryLevel/TryLevel.cs")]
public class TryLevel : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PlayButtonPressed = "PlayButtonPressed";

		public static readonly StringName SelectGroup = "SelectGroup";

		public static readonly StringName DetachActiveGroupPreviews = "DetachActiveGroupPreviews";

		public static readonly StringName SetTryLevelFinishMarkerVisible = "SetTryLevelFinishMarkerVisible";

		public static readonly StringName SelectPurple = "SelectPurple";

		public static readonly StringName SelectStar = "SelectStar";

		public static readonly StringName SelectColour = "SelectColour";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";

		public static readonly StringName ShopButtonPressed = "ShopButtonPressed";

		public static readonly StringName StarExchangeButtonPressed = "StarExchangeButtonPressed";

		public static readonly StringName AlmanacButtonPressed = "AlmanacButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName dragMenu = "dragMenu";

		public static readonly StringName shopButton = "shopButton";

		public static readonly StringName starExchangeButton = "starExchangeButton";

		public static readonly StringName purpleButton = "purpleButton";

		public static readonly StringName starButton = "starButton";

		public static readonly StringName colourButton = "colourButton";

		public static readonly StringName currentGroup = "currentGroup";

		public static readonly StringName openedFromShop = "openedFromShop";

		public static readonly StringName openedFromStarExchange = "openedFromStarExchange";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private static Json _tryLevelResource;

	private static Texture2D _debuffFinish;

	private DragMenu dragMenu;

	private TextureButton shopButton;

	private TextureButton starExchangeButton;

	private TextureButton purpleButton;

	private TextureButton starButton;

	private TextureButton colourButton;

	private readonly System.Collections.Generic.Dictionary<string, List<Control>> _groupPreviewCache = new System.Collections.Generic.Dictionary<string, List<Control>>();

	private readonly List<Control> _activeGroupPreviews = new List<Control>();

	public string currentGroup;

	public bool openedFromShop;

	public bool openedFromStarExchange;

	private static Json TRY_LEVEL_RESOURCE => _tryLevelResource ?? (_tryLevelResource = GD.Load<Json>("res://Asset/Config/Level/TryLevelResource.json"));

	private static Texture2D DEBUFF_FINISH => _debuffFinish ?? (_debuffFinish = GD.Load<Texture2D>("uid://dpm2kwh72n3p4"));

	public override void Init(Dictionary data)
	{
		openedFromShop = data.GetValueOrDefault("openedFromShop", false).AsBool();
		openedFromStarExchange = data.GetValueOrDefault("openedFromStarExchange", false).AsBool();
	}

	public override void _Ready()
	{
		base._Ready();
		dragMenu = GetNode<DragMenu>("%DragMenu");
		shopButton = GetNode<TextureButton>("%ShopButton");
		shopButton.Visible = openedFromShop || (GlobalFeatureManager.Instance?.IsUnlocked("Shop") ?? false);
		starExchangeButton = GetNode<TextureButton>("%StarExchangeButton");
		purpleButton = GetNode<TextureButton>("%PurpleButton");
		starButton = GetNode<TextureButton>("%StarButton");
		colourButton = GetNode<TextureButton>("%ColourButton");
		purpleButton.Pressed += SelectPurple;
		starButton.Pressed += SelectStar;
		colourButton.Pressed += SelectColour;
		shopButton.Pressed += ShopButtonPressed;
		starExchangeButton.Pressed += StarExchangeButtonPressed;
		GetNode<TextureButton>("GroupButtonNode/AlmanacButton").Pressed += AlmanacButtonPressed;
		GetNode<TextureButton>("PlayButton").Pressed += PlayButtonPressed;
		GetNode<TextureButton>("%BackButton").Pressed += BackButtonPressed;
		if (Global.Instance.enterTryLevelGroup == "")
		{
			SelectPurple();
			return;
		}
		switch (Global.Instance.enterTryLevelGroup)
		{
		case "Purple":
			SelectPurple();
			break;
		case "Star":
			SelectStar();
			break;
		case "Colour":
			SelectColour();
			break;
		}
		Global.Instance.enterTryLevelGroup = "";
	}

	public void PlayButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		Array array = ((Dictionary)TRY_LEVEL_RESOURCE.Data)[currentGroup].AsGodotArray();
		string text = GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString();
		Dictionary dictionary = array[dragMenu.currentIndex].AsGodotDictionary()["Level"].AsGodotDictionary();
		if (dictionary.ContainsKey(text) && dictionary[text].AsString() != "")
		{
			TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>(dictionary[text].AsString());
		}
		else
		{
			TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>(dictionary["Normal"].AsString());
		}
		Global.Instance.enterLevelMode = "LevelChoose";
		Global.Instance.currentLevelChoose = "TryLevel";
		Global.Instance.enterTryLevelGroup = currentGroup;
		SceneManager.Instance.ChangeScene("TowerDefense");
	}

	public void SelectGroup(string groupName)
	{
		currentGroup = groupName;
		DetachActiveGroupPreviews();
		foreach (Control orCreateGroupPreview in GetOrCreateGroupPreviews(groupName))
		{
			if (orCreateGroupPreview.GetParent() != dragMenu)
			{
				dragMenu.AddChild(orCreateGroupPreview, forceReadableName: false, InternalMode.Disabled);
			}
			_activeGroupPreviews.Add(orCreateGroupPreview);
		}
		dragMenu.CallDeferred("SetPos", 0);
	}

	private void DetachActiveGroupPreviews()
	{
		foreach (Control activeGroupPreview in _activeGroupPreviews)
		{
			if (GodotObject.IsInstanceValid(activeGroupPreview) && activeGroupPreview.GetParent() == dragMenu)
			{
				dragMenu.RemoveChild(activeGroupPreview);
			}
		}
		_activeGroupPreviews.Clear();
	}

	private List<Control> GetOrCreateGroupPreviews(string groupName)
	{
		if (_groupPreviewCache.TryGetValue(groupName, out var value))
		{
			RefreshGroupPreviewFinishState(groupName, value);
			return value;
		}
		List<Control> list = new List<Control>();
		foreach (Variant item in ((Dictionary)TRY_LEVEL_RESOURCE.Data)[groupName].AsGodotArray())
		{
			TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(item.AsGodotDictionary().GetValueOrDefault("Character", "").AsString());
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.setMobileLayout = true;
			dragMenu.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			if (GodotObject.IsInstanceValid(packetConfigReadOnly))
			{
				towerDefenseInGamePacketShow.Init(packetConfigReadOnly);
			}
			list.Add(towerDefenseInGamePacketShow);
		}
		_groupPreviewCache[groupName] = list;
		RefreshGroupPreviewFinishState(groupName, list);
		return list;
	}

	private void RefreshGroupPreviewFinishState(string groupName, List<Control> previews)
	{
		Array array = ((Dictionary)TRY_LEVEL_RESOURCE.Data)[groupName].AsGodotArray();
		for (int i = 0; i < previews.Count && i < array.Count; i++)
		{
			if (previews[i] is TowerDefenseInGamePacketShow packet)
			{
				Dictionary dictionary = array[i].AsGodotDictionary();
				Dictionary dictionary2 = GameSaveManager.Instance.GetLevelValue(dictionary["SaveKey"].AsString()).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
				bool visible = CommandManager.Instance.debugOpenAllLevel || dictionary2.GetValueOrDefault("Finish", 0).AsInt32() > 0;
				SetTryLevelFinishMarkerVisible(packet, visible);
			}
		}
	}

	private static void SetTryLevelFinishMarkerVisible(TowerDefenseInGamePacketShow packet, bool visible)
	{
		Sprite2D sprite2D = packet.GetNodeOrNull<Sprite2D>("FinishMarker");
		if (sprite2D == null)
		{
			sprite2D = new Sprite2D();
			sprite2D.Name = "FinishMarker";
			sprite2D.Texture = DEBUFF_FINISH;
			sprite2D.Scale = Vector2.One * 0.5f;
			sprite2D.Position = new Vector2(40f, 25f);
			packet.AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
		}
		sprite2D.Visible = visible;
	}

	public void SelectPurple()
	{
		SelectGroup("Purple");
		purpleButton.ButtonPressed = true;
		shopButton.Visible = true;
		starExchangeButton.Visible = false;
	}

	public void SelectStar()
	{
		SelectGroup("Star");
		starButton.ButtonPressed = true;
		shopButton.Visible = false;
		starExchangeButton.Visible = true;
	}

	public void SelectColour()
	{
		SelectGroup("Colour");
		colourButton.ButtonPressed = true;
		shopButton.Visible = true;
		starExchangeButton.Visible = false;
	}

	public void BackButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		CloseDialog();
	}

	public void ShopButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (openedFromShop)
		{
			CloseDialog();
			return;
		}
		GlobalFeatureManager instance = GlobalFeatureManager.Instance;
		if (instance != null && instance.IsUnlocked("Shop"))
		{
			DialogManager.Instance.DialogCreate("Shop");
			CloseDialog();
		}
	}

	public async void StarExchangeButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		if (openedFromStarExchange)
		{
			CloseDialog();
			return;
		}
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("StarExchange");
		Visible = false;
		await dialogBoxBase.WaitForClose();
		Visible = true;
	}

	public async void AlmanacButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("Almanac");
		Visible = false;
		await dialogBoxBase.WaitForClose();
		Visible = true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "groupName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DetachActiveGroupPreviews, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetTryLevelFinishMarkerVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectPurple, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectStar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectColour, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShopButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StarExchangeButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AlmanacButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayButtonPressed && args.Count == 0)
		{
			PlayButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectGroup && args.Count == 1)
		{
			SelectGroup(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachActiveGroupPreviews && args.Count == 0)
		{
			DetachActiveGroupPreviews();
			ret = default;
			return true;
		}
		if (method == MethodName.SetTryLevelFinishMarkerVisible && args.Count == 2)
		{
			SetTryLevelFinishMarkerVisible(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectPurple && args.Count == 0)
		{
			SelectPurple();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectStar && args.Count == 0)
		{
			SelectStar();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectColour && args.Count == 0)
		{
			SelectColour();
			ret = default;
			return true;
		}
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ShopButtonPressed && args.Count == 0)
		{
			ShopButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.StarExchangeButtonPressed && args.Count == 0)
		{
			StarExchangeButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed && args.Count == 0)
		{
			AlmanacButtonPressed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetTryLevelFinishMarkerVisible && args.Count == 2)
		{
			SetTryLevelFinishMarkerVisible(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
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
		if (method == MethodName.PlayButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SelectGroup)
		{
			return true;
		}
		if (method == MethodName.DetachActiveGroupPreviews)
		{
			return true;
		}
		if (method == MethodName.SetTryLevelFinishMarkerVisible)
		{
			return true;
		}
		if (method == MethodName.SelectPurple)
		{
			return true;
		}
		if (method == MethodName.SelectStar)
		{
			return true;
		}
		if (method == MethodName.SelectColour)
		{
			return true;
		}
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ShopButtonPressed)
		{
			return true;
		}
		if (method == MethodName.StarExchangeButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AlmanacButtonPressed)
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
		if (name == PropertyName.shopButton)
		{
			shopButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.starExchangeButton)
		{
			starExchangeButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.purpleButton)
		{
			purpleButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.starButton)
		{
			starButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.colourButton)
		{
			colourButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.currentGroup)
		{
			currentGroup = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.openedFromShop)
		{
			openedFromShop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.openedFromStarExchange)
		{
			openedFromStarExchange = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.shopButton)
		{
			value = VariantUtils.CreateFrom(in shopButton);
			return true;
		}
		if (name == PropertyName.starExchangeButton)
		{
			value = VariantUtils.CreateFrom(in starExchangeButton);
			return true;
		}
		if (name == PropertyName.purpleButton)
		{
			value = VariantUtils.CreateFrom(in purpleButton);
			return true;
		}
		if (name == PropertyName.starButton)
		{
			value = VariantUtils.CreateFrom(in starButton);
			return true;
		}
		if (name == PropertyName.colourButton)
		{
			value = VariantUtils.CreateFrom(in colourButton);
			return true;
		}
		if (name == PropertyName.currentGroup)
		{
			value = VariantUtils.CreateFrom(in currentGroup);
			return true;
		}
		if (name == PropertyName.openedFromShop)
		{
			value = VariantUtils.CreateFrom(in openedFromShop);
			return true;
		}
		if (name == PropertyName.openedFromStarExchange)
		{
			value = VariantUtils.CreateFrom(in openedFromStarExchange);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.shopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.starExchangeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.purpleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.starButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.colourButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentGroup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.openedFromShop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.openedFromStarExchange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dragMenu, Variant.From(in dragMenu));
		info.AddProperty(PropertyName.shopButton, Variant.From(in shopButton));
		info.AddProperty(PropertyName.starExchangeButton, Variant.From(in starExchangeButton));
		info.AddProperty(PropertyName.purpleButton, Variant.From(in purpleButton));
		info.AddProperty(PropertyName.starButton, Variant.From(in starButton));
		info.AddProperty(PropertyName.colourButton, Variant.From(in colourButton));
		info.AddProperty(PropertyName.currentGroup, Variant.From(in currentGroup));
		info.AddProperty(PropertyName.openedFromShop, Variant.From(in openedFromShop));
		info.AddProperty(PropertyName.openedFromStarExchange, Variant.From(in openedFromStarExchange));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dragMenu, out var value))
		{
			dragMenu = value.As<DragMenu>();
		}
		if (info.TryGetProperty(PropertyName.shopButton, out var value2))
		{
			shopButton = value2.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.starExchangeButton, out var value3))
		{
			starExchangeButton = value3.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.purpleButton, out var value4))
		{
			purpleButton = value4.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.starButton, out var value5))
		{
			starButton = value5.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.colourButton, out var value6))
		{
			colourButton = value6.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.currentGroup, out var value7))
		{
			currentGroup = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.openedFromShop, out var value8))
		{
			openedFromShop = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.openedFromStarExchange, out var value9))
		{
			openedFromStarExchange = value9.As<bool>();
		}
	}
}
