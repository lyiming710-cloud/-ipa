using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/LevelEditor/OnlineLevel/Item/LevelEditorOnlineLevelItem.cs")]
public class LevelEditorOnlineLevelItem : Control
{
	public delegate void SelectEventHandler(string url, string id);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName SelectButtonPressed = "SelectButtonPressed";

		public static readonly StringName EmitSelect = "EmitSelect";

		public static readonly StringName SetTags = "SetTags";

		public static readonly StringName AddIconTexture = "AddIconTexture";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _nameLabel = "_nameLabel";

		public static readonly StringName _mapTexture = "_mapTexture";

		public static readonly StringName _finishTexture = "_finishTexture";

		public static readonly StringName _hBoxContainer = "_hBoxContainer";

		public static readonly StringName _centerContainer = "_centerContainer";

		public static readonly StringName _tagsLabel = "_tagsLabel";

		public static readonly StringName _recommendedLabel = "_recommendedLabel";

		public static readonly StringName isBattle = "isBattle";

		public static readonly StringName cost = "cost";

		public static readonly StringName id = "id";

		public static readonly StringName lucky = "lucky";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string PATH = "user://Csharp/OnlineLevel";

	private static Texture2D _izIcon;

	private static Texture2D _iz2Icon;

	private static Texture2D _vrIcon;

	private static Texture2D _endlessIcon;

	private static Texture2D _survivalIcon;

	private static Texture2D _luckyIcon;

	private Label _nameLabel;

	private TextureRect _mapTexture;

	private TextureRect _finishTexture;

	private HBoxContainer _hBoxContainer;

	private CenterContainer _centerContainer;

	private Label _tagsLabel;

	private Label _recommendedLabel;

	[Export(PropertyHint.None, "")]
	public bool isBattle;

	[Export(PropertyHint.None, "")]
	public int cost = 1000;

	public string id;

	public bool lucky;

	private static Texture2D IZ_ICON => _izIcon ?? (_izIcon = GD.Load<Texture2D>("uid://ciyj1718ypbih"));

	private static Texture2D IZ2_ICON => _iz2Icon ?? (_iz2Icon = GD.Load<Texture2D>("uid://b8rsot26eb7od"));

	private static Texture2D Vr_ICON => _vrIcon ?? (_vrIcon = GD.Load<Texture2D>("uid://24j6iw1ww08b"));

	private static Texture2D ENDLESS_ICON => _endlessIcon ?? (_endlessIcon = GD.Load<Texture2D>("uid://bcjx34t665il2"));

	private static Texture2D SURVIVAL_ICON => _survivalIcon ?? (_survivalIcon = GD.Load<Texture2D>("uid://crvtrh32bqu0r"));

	private static Texture2D LUCKY_ICON => _luckyIcon ?? (_luckyIcon = GD.Load<Texture2D>("uid://caqbmeu30cbsa"));

	public event SelectEventHandler OnSelect;

	public event Action<string, string> OnAuthorSelected;

	public override void _Ready()
	{
		GetNode<TextureButton>("SelectButton").Pressed += SelectButtonPressed;
	}

	public async void Init(Dictionary data)
	{
		_nameLabel = GetNode<Label>("%NameLabel");
		_mapTexture = GetNode<TextureRect>("%MapTexture");
		_finishTexture = GetNode<TextureRect>("%FinishTexture");
		_hBoxContainer = GetNode<HBoxContainer>("%HBoxContainer");
		_centerContainer = GetNode<CenterContainer>("%CenterContainer");
		_tagsLabel = GetNode<Label>("%TagsLabel");
		_recommendedLabel = GetNode<Label>("%RecommendedLabel");
		_centerContainer.Visible = false;
		foreach (Node child in _hBoxContainer.GetChildren())
		{
			child.QueueFree();
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		if (!GodotObject.IsInstanceValid(this) || !IsInsideTree() || IsQueuedForDeletion())
		{
			return;
		}
		id = data.GetValueOrDefault("id", "-1").AsString();
		_nameLabel.Text = data.GetValueOrDefault("name", "").AsString();
		lucky = data.GetValueOrDefault("lucky", false).AsBool();
		_recommendedLabel.Visible = data.GetValueOrDefault("recommended", false).AsBool();
		SetTags(data);
		TowerDefenseMapConfig mapConfig = TowerDefenseManager.Instance.GetMapConfig(data.GetValueOrDefault("map", "Frontlawn").AsString());
		TowerDefenseMapConfig.ApplyMapPreviewTexture(_mapTexture, mapConfig?.GetMapThumbnail());
		string key = "OnlineLevel-" + id;
		if (GameSaveManager.Instance.GetLevelValue(key).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
			.GetValueOrDefault("Finish", 0)
			.AsInt32() > 0)
		{
			_finishTexture.Visible = true;
		}
		switch (data.GetValueOrDefault("finishMethod", "WAVE").AsString().ToUpper())
		{
		case "WAVE":
			_centerContainer.Visible = false;
			if (data.ContainsKey("survivalRoundlimit") && data["survivalRoundlimit"].VariantType != Variant.Type.Nil)
			{
				int num = data.GetValueOrDefault("survivalRoundlimit", -1).AsInt32();
				if (num == -1)
				{
					_centerContainer.Visible = true;
					AddIconTexture(ENDLESS_ICON);
				}
				else if (num >= 0)
				{
					_centerContainer.Visible = true;
					AddIconTexture(SURVIVAL_ICON);
				}
			}
			break;
		case "VASE":
			_centerContainer.Visible = true;
			AddIconTexture(Vr_ICON);
			break;
		case "IZM":
			_centerContainer.Visible = true;
			AddIconTexture(IZ_ICON);
			break;
		case "IZM2":
			_centerContainer.Visible = true;
			AddIconTexture(IZ2_ICON);
			break;
		}
		if (lucky)
		{
			_centerContainer.Visible = true;
			AddIconTexture(LUCKY_ICON);
		}
	}

	public void SelectButtonPressed()
	{
		if (isBattle && TowerDefenseManager.Instance.GetCoin() < cost)
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", "[center][font_size=24]您的金币不足[/font_size][/center]");
			return;
		}
		OnlineLevelPreview onlineLevelPreview = (OnlineLevelPreview)DialogManager.Instance.DialogCreate("OnlineLevelPreview");
		if (OnAuthorSelected != null)
		{
			onlineLevelPreview.OnAuthorSelected += (string uid, string name) =>
			{
				OnAuthorSelected?.Invoke(uid, name);
			};
		}
		onlineLevelPreview.InitDialog(id);
		onlineLevelPreview.OnSelect += EmitSelect;
	}

	public void EmitSelect(string url)
	{
		OnSelect?.Invoke(url, id);
	}

	private void SetTags(Dictionary data)
	{
		if (_tagsLabel == null)
		{
			return;
		}
		if (!data.ContainsKey("tags") || data["tags"].VariantType != Variant.Type.Array)
		{
			_tagsLabel.Visible = false;
			return;
		}
		Godot.Collections.Array array = data["tags"].AsGodotArray();
		List<string> list = new List<string>();
		for (int i = 0; i < array.Count; i++)
		{
			string text = array[i].AsString();
			if (text != "")
			{
				list.Add(text);
			}
		}
		if (list.Count == 0)
		{
			_tagsLabel.Visible = false;
			return;
		}
		_tagsLabel.Visible = true;
		_tagsLabel.Text = string.Join("\u3000", list);
	}

	public void AddIconTexture(Texture2D iconTexture)
	{
		if (_hBoxContainer.GetChildCount() < 2)
		{
			TextureRect textureRect = new TextureRect();
			textureRect.Texture = iconTexture;
			textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
			textureRect.ExpandMode = TextureRect.ExpandModeEnum.KeepSize;
			textureRect.MouseFilter = MouseFilterEnum.Ignore;
			_hBoxContainer.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitSelect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "url", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddIconTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "iconTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectButtonPressed && args.Count == 0)
		{
			SelectButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitSelect && args.Count == 1)
		{
			EmitSelect(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTags && args.Count == 1)
		{
			SetTags(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddIconTexture && args.Count == 1)
		{
			AddIconTexture(VariantUtils.ConvertTo<Texture2D>(in args[0]));
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
		if (method == MethodName.SelectButtonPressed)
		{
			return true;
		}
		if (method == MethodName.EmitSelect)
		{
			return true;
		}
		if (method == MethodName.SetTags)
		{
			return true;
		}
		if (method == MethodName.AddIconTexture)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._nameLabel)
		{
			_nameLabel = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName._hBoxContainer)
		{
			_hBoxContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._centerContainer)
		{
			_centerContainer = VariantUtils.ConvertTo<CenterContainer>(in value);
			return true;
		}
		if (name == PropertyName._tagsLabel)
		{
			_tagsLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._recommendedLabel)
		{
			_recommendedLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.isBattle)
		{
			isBattle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.id)
		{
			id = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.lucky)
		{
			lucky = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._nameLabel)
		{
			value = VariantUtils.CreateFrom(in _nameLabel);
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
		if (name == PropertyName._hBoxContainer)
		{
			value = VariantUtils.CreateFrom(in _hBoxContainer);
			return true;
		}
		if (name == PropertyName._centerContainer)
		{
			value = VariantUtils.CreateFrom(in _centerContainer);
			return true;
		}
		if (name == PropertyName._tagsLabel)
		{
			value = VariantUtils.CreateFrom(in _tagsLabel);
			return true;
		}
		if (name == PropertyName._recommendedLabel)
		{
			value = VariantUtils.CreateFrom(in _recommendedLabel);
			return true;
		}
		if (name == PropertyName.isBattle)
		{
			value = VariantUtils.CreateFrom(in isBattle);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.id)
		{
			value = VariantUtils.CreateFrom(in id);
			return true;
		}
		if (name == PropertyName.lucky)
		{
			value = VariantUtils.CreateFrom(in lucky);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._mapTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._finishTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hBoxContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._centerContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tagsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._recommendedLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isBattle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.lucky, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nameLabel, Variant.From(in _nameLabel));
		info.AddProperty(PropertyName._mapTexture, Variant.From(in _mapTexture));
		info.AddProperty(PropertyName._finishTexture, Variant.From(in _finishTexture));
		info.AddProperty(PropertyName._hBoxContainer, Variant.From(in _hBoxContainer));
		info.AddProperty(PropertyName._centerContainer, Variant.From(in _centerContainer));
		info.AddProperty(PropertyName._tagsLabel, Variant.From(in _tagsLabel));
		info.AddProperty(PropertyName._recommendedLabel, Variant.From(in _recommendedLabel));
		info.AddProperty(PropertyName.isBattle, Variant.From(in isBattle));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.id, Variant.From(in id));
		info.AddProperty(PropertyName.lucky, Variant.From(in lucky));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nameLabel, out var value))
		{
			_nameLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mapTexture, out var value2))
		{
			_mapTexture = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._finishTexture, out var value3))
		{
			_finishTexture = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._hBoxContainer, out var value4))
		{
			_hBoxContainer = value4.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._centerContainer, out var value5))
		{
			_centerContainer = value5.As<CenterContainer>();
		}
		if (info.TryGetProperty(PropertyName._tagsLabel, out var value6))
		{
			_tagsLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._recommendedLabel, out var value7))
		{
			_recommendedLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.isBattle, out var value8))
		{
			isBattle = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value9))
		{
			cost = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.id, out var value10))
		{
			id = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.lucky, out var value11))
		{
			lucky = value11.As<bool>();
		}
	}
}
