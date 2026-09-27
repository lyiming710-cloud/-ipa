using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/Shop/ShopItem/ShopItem.cs")]
public class ShopItem : Control
{
	public delegate void PressedSignalEventHandler(ShopItem item);

	public delegate void TalkEventHandler(string text);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName ShowUnavailable = "ShowUnavailable";

		public static readonly StringName RefreshEditorPreview = "RefreshEditorPreview";

		public static readonly StringName EnsurePacketPreview = "EnsurePacketPreview";

		public static readonly StringName Sale = "Sale";

		public static readonly StringName Pressed = "Pressed";

		public static readonly StringName HandleTalk = "HandleTalk";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName cost = "cost";

		public static readonly StringName editorPreviewMode = "editorPreviewMode";

		public static readonly StringName costLabel = "costLabel";

		public static readonly StringName describeLabel = "describeLabel";

		public static readonly StringName soldOutLabel = "soldOutLabel";

		public static readonly StringName itemButton = "itemButton";

		public static readonly StringName packetNode = "packetNode";

		public static readonly StringName _packetPreview = "_packetPreview";

		public static readonly StringName config = "config";

		public static readonly StringName currentStageId = "currentStageId";

		public static readonly StringName _cost = "_cost";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public bool editorPreviewMode;

	private Label costLabel;

	private Label describeLabel;

	private Label soldOutLabel;

	private SpriteBrightButton itemButton;

	private Control packetNode;

	private TowerDefenseInGamePacketShow _packetPreview;

	public ShopItemConfig config;

	public int currentStageId = -1;

	private int _cost;

	public int cost
	{
		get
		{
			return _cost;
		}
		set
		{
			_cost = value;
			costLabel.Text = $"$ {_cost}";
		}
	}

	public event PressedSignalEventHandler OnPressedSignal;

	public event TalkEventHandler OnTalk;

	public override void _Ready()
	{
		costLabel = GetNode<Label>("%CostLabel");
		describeLabel = GetNode<Label>("%DescribeLabel");
		soldOutLabel = GetNode<Label>("%SoldOutLabel");
		itemButton = GetNode<SpriteBrightButton>("%ItemButton");
		packetNode = GetNode<Control>("%PacketNode");
		describeLabel.MouseFilter = MouseFilterEnum.Ignore;
		soldOutLabel.MouseFilter = MouseFilterEnum.Ignore;
		itemButton.OnMouseEntered += HandleTalk;
		itemButton.OnPressed += Pressed;
	}

	public void Init(ShopItemConfig _config)
	{
		config = _config;
		Refresh();
	}

	public void Refresh()
	{
		if (editorPreviewMode)
		{
			RefreshEditorPreview();
			return;
		}
		currentStageId = -1;
		soldOutLabel.Visible = false;
		describeLabel.Visible = true;
		itemButton.disabled = false;
		itemButton.Modulate = Colors.White;
		if (GodotObject.IsInstanceValid(_packetPreview))
		{
			_packetPreview.Visible = false;
		}
		if (!GodotObject.IsInstanceValid(config) || config.stageList == null || config.stageList.Count == 0)
		{
			ShowUnavailable("未配置商品阶段");
			return;
		}
		for (int i = 0; i < config.stageList.Count; i++)
		{
			ShopItemStageConfig shopItemStageConfig = config.stageList[i];
			if (!GodotObject.IsInstanceValid(shopItemStageConfig))
			{
				continue;
			}
			string saveType = shopItemStageConfig.saveType;
			if (!(saveType == "Feature"))
			{
				if (!(saveType == "TowerDefensePacket"))
				{
					continue;
				}
				itemButton.Modulate = new Color(1f, 1f, 1f, 0f);
				TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(shopItemStageConfig.saveKey);
				if (GodotObject.IsInstanceValid(packetConfigReadOnly))
				{
					TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = EnsurePacketPreview();
					towerDefenseInGamePacketShow.ResetForPool();
					towerDefenseInGamePacketShow.setPcLayout = true;
					towerDefenseInGamePacketShow.onlyDraw = true;
					towerDefenseInGamePacketShow.Visible = true;
					if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.button))
					{
						towerDefenseInGamePacketShow.button.MouseFilter = MouseFilterEnum.Ignore;
					}
					towerDefenseInGamePacketShow.Init(packetConfigReadOnly);
					if (!packetConfigReadOnly.Unlock())
					{
						cost = shopItemStageConfig.cost;
						describeLabel.Text = shopItemStageConfig.describe;
						currentStageId = i;
						return;
					}
				}
			}
			else
			{
				int num = GlobalFeatureManager.Instance?.GetValue(shopItemStageConfig.saveKey) ?? 0;
				if (num >= shopItemStageConfig.openMinNum && num < shopItemStageConfig.openMaxNum)
				{
					cost = shopItemStageConfig.cost;
					itemButton.Texture = shopItemStageConfig.texture;
					describeLabel.Text = shopItemStageConfig.describe;
					currentStageId = i;
					return;
				}
			}
		}
		ShopItemStageConfig shopItemStageConfig2 = null;
		for (int num2 = config.stageList.Count - 1; num2 >= 0; num2--)
		{
			if (GodotObject.IsInstanceValid(config.stageList[num2]))
			{
				shopItemStageConfig2 = config.stageList[num2];
				break;
			}
		}
		if (!GodotObject.IsInstanceValid(shopItemStageConfig2))
		{
			ShowUnavailable("商品阶段为空");
			return;
		}
		itemButton.Texture = shopItemStageConfig2.texture;
		describeLabel.Text = shopItemStageConfig2.describe;
		cost = shopItemStageConfig2.cost;
		soldOutLabel.Visible = true;
		describeLabel.Visible = false;
		itemButton.disabled = true;
	}

	private void ShowUnavailable(string message)
	{
		currentStageId = -1;
		cost = 0;
		describeLabel.Text = message;
		describeLabel.Visible = true;
		soldOutLabel.Visible = false;
		itemButton.disabled = true;
		itemButton.Modulate = Colors.White;
		if (GodotObject.IsInstanceValid(_packetPreview))
		{
			_packetPreview.Visible = false;
		}
	}

	private void RefreshEditorPreview()
	{
		currentStageId = -1;
		soldOutLabel.Visible = false;
		describeLabel.Visible = true;
		itemButton.disabled = false;
		itemButton.Modulate = Colors.White;
		if (GodotObject.IsInstanceValid(_packetPreview))
		{
			_packetPreview.Visible = false;
		}
		if (!GodotObject.IsInstanceValid(config) || config.stageList == null || config.stageList.Count == 0)
		{
			ShowUnavailable("未配置商品阶段");
			return;
		}
		ShopItemStageConfig shopItemStageConfig = null;
		for (int i = 0; i < config.stageList.Count; i++)
		{
			if (GodotObject.IsInstanceValid(config.stageList[i]))
			{
				shopItemStageConfig = config.stageList[i];
				currentStageId = i;
				break;
			}
		}
		if (!GodotObject.IsInstanceValid(shopItemStageConfig))
		{
			ShowUnavailable("商品阶段为空");
			return;
		}
		cost = shopItemStageConfig.cost;
		itemButton.Texture = shopItemStageConfig.texture;
		describeLabel.Text = shopItemStageConfig.describe;
		if (shopItemStageConfig.saveType != "TowerDefensePacket")
		{
			return;
		}
		TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(shopItemStageConfig.saveKey);
		if (GodotObject.IsInstanceValid(packetConfigReadOnly))
		{
			itemButton.Modulate = new Color(1f, 1f, 1f, 0f);
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = EnsurePacketPreview();
			towerDefenseInGamePacketShow.ResetForPool();
			towerDefenseInGamePacketShow.setPcLayout = true;
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.Visible = true;
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow.button))
			{
				towerDefenseInGamePacketShow.button.MouseFilter = MouseFilterEnum.Ignore;
			}
			towerDefenseInGamePacketShow.Init(packetConfigReadOnly);
		}
	}

	private TowerDefenseInGamePacketShow EnsurePacketPreview()
	{
		if (GodotObject.IsInstanceValid(_packetPreview))
		{
			return _packetPreview;
		}
		foreach (Node child in packetNode.GetChildren())
		{
			child.QueueFree();
		}
		_packetPreview = TowerDefenseManager.CreatePacketShow();
		_packetPreview.setPcLayout = true;
		_packetPreview.onlyDraw = true;
		_packetPreview.MouseFilter = MouseFilterEnum.Ignore;
		packetNode.AddChild(_packetPreview, forceReadableName: false, InternalMode.Disabled);
		if (GodotObject.IsInstanceValid(_packetPreview.button))
		{
			_packetPreview.button.MouseFilter = MouseFilterEnum.Ignore;
		}
		return _packetPreview;
	}

	public void Sale()
	{
		if (!TryGetCurrentStage(out var stage))
		{
			return;
		}
		string saveType = stage.saveType;
		if (!(saveType == "Feature"))
		{
			if (saveType == "TowerDefensePacket")
			{
				Dictionary towerDefensePacketValue = GameSaveManager.Instance.GetTowerDefensePacketValue(stage.saveKey);
				towerDefensePacketValue["Unlock"] = true;
				GameSaveManager.Instance.SetTowerDefensePacketValue(stage.saveKey, towerDefensePacketValue);
			}
		}
		else
		{
			GlobalFeatureManager.Instance?.AddValue(stage.saveKey, stage.addNum, saveImmediately: false);
		}
		GameSaveManager.Instance.Save();
		Refresh();
	}

	public void Pressed()
	{
		OnPressedSignal?.Invoke(this);
	}

	public void HandleTalk()
	{
		if (TryGetTalkStage(out var stage))
		{
			OnTalk?.Invoke(stage.npcTalk);
		}
	}

	private bool TryGetCurrentStage(out ShopItemStageConfig stage)
	{
		stage = null;
		if (config == null || currentStageId < 0 || currentStageId >= config.stageList.Count)
		{
			return false;
		}
		stage = config.stageList[currentStageId];
		return true;
	}

	private bool TryGetTalkStage(out ShopItemStageConfig stage)
	{
		if (TryGetCurrentStage(out stage))
		{
			return true;
		}
		stage = null;
		if (config == null || config.stageList.Count == 0)
		{
			return false;
		}
		stage = config.stageList[config.stageList.Count - 1];
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowUnavailable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEditorPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePacketPreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Sale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleTalk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<ShopItemConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowUnavailable && args.Count == 1)
		{
			ShowUnavailable(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEditorPreview && args.Count == 0)
		{
			RefreshEditorPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePacketPreview && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseInGamePacketShow>(EnsurePacketPreview());
			return true;
		}
		if (method == MethodName.Sale && args.Count == 0)
		{
			Sale();
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 0)
		{
			Pressed();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleTalk && args.Count == 0)
		{
			HandleTalk();
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.ShowUnavailable)
		{
			return true;
		}
		if (method == MethodName.RefreshEditorPreview)
		{
			return true;
		}
		if (method == MethodName.EnsurePacketPreview)
		{
			return true;
		}
		if (method == MethodName.Sale)
		{
			return true;
		}
		if (method == MethodName.Pressed)
		{
			return true;
		}
		if (method == MethodName.HandleTalk)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			editorPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.costLabel)
		{
			costLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.describeLabel)
		{
			describeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.soldOutLabel)
		{
			soldOutLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.itemButton)
		{
			itemButton = VariantUtils.ConvertTo<SpriteBrightButton>(in value);
			return true;
		}
		if (name == PropertyName.packetNode)
		{
			packetNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._packetPreview)
		{
			_packetPreview = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<ShopItemConfig>(in value);
			return true;
		}
		if (name == PropertyName.currentStageId)
		{
			currentStageId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cost)
		{
			_cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom<int>(cost);
			return true;
		}
		if (name == PropertyName.editorPreviewMode)
		{
			value = VariantUtils.CreateFrom(in editorPreviewMode);
			return true;
		}
		if (name == PropertyName.costLabel)
		{
			value = VariantUtils.CreateFrom(in costLabel);
			return true;
		}
		if (name == PropertyName.describeLabel)
		{
			value = VariantUtils.CreateFrom(in describeLabel);
			return true;
		}
		if (name == PropertyName.soldOutLabel)
		{
			value = VariantUtils.CreateFrom(in soldOutLabel);
			return true;
		}
		if (name == PropertyName.itemButton)
		{
			value = VariantUtils.CreateFrom(in itemButton);
			return true;
		}
		if (name == PropertyName.packetNode)
		{
			value = VariantUtils.CreateFrom(in packetNode);
			return true;
		}
		if (name == PropertyName._packetPreview)
		{
			value = VariantUtils.CreateFrom(in _packetPreview);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.currentStageId)
		{
			value = VariantUtils.CreateFrom(in currentStageId);
			return true;
		}
		if (name == PropertyName._cost)
		{
			value = VariantUtils.CreateFrom(in _cost);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.editorPreviewMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.costLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.describeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.soldOutLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.itemButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentStageId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.cost, Variant.From<int>(cost));
		info.AddProperty(PropertyName.editorPreviewMode, Variant.From(in editorPreviewMode));
		info.AddProperty(PropertyName.costLabel, Variant.From(in costLabel));
		info.AddProperty(PropertyName.describeLabel, Variant.From(in describeLabel));
		info.AddProperty(PropertyName.soldOutLabel, Variant.From(in soldOutLabel));
		info.AddProperty(PropertyName.itemButton, Variant.From(in itemButton));
		info.AddProperty(PropertyName.packetNode, Variant.From(in packetNode));
		info.AddProperty(PropertyName._packetPreview, Variant.From(in _packetPreview));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.currentStageId, Variant.From(in currentStageId));
		info.AddProperty(PropertyName._cost, Variant.From(in _cost));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.cost, out var value))
		{
			cost = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.editorPreviewMode, out var value2))
		{
			editorPreviewMode = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.costLabel, out var value3))
		{
			costLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.describeLabel, out var value4))
		{
			describeLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.soldOutLabel, out var value5))
		{
			soldOutLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.itemButton, out var value6))
		{
			itemButton = value6.As<SpriteBrightButton>();
		}
		if (info.TryGetProperty(PropertyName.packetNode, out var value7))
		{
			packetNode = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._packetPreview, out var value8))
		{
			_packetPreview = value8.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value9))
		{
			config = value9.As<ShopItemConfig>();
		}
		if (info.TryGetProperty(PropertyName.currentStageId, out var value10))
		{
			currentStageId = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cost, out var value11))
		{
			_cost = value11.As<int>();
		}
	}
}
