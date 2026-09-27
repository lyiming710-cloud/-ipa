using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/LevelEditor/MyLevelItem/LevelEditorMyLevelItem.cs")]
public class LevelEditorMyLevelItem : Control
{
	public delegate void SelectEventHandler(string uid);

	public delegate void DeleteEventHandler(string uid);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName DeleteButtonPressed = "DeleteButtonPressed";

		public static readonly StringName SelectButtonPressed = "SelectButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _nameLabel = "_nameLabel";

		public static readonly StringName _mapTexture = "_mapTexture";

		public static readonly StringName _iconTexture = "_iconTexture";

		public static readonly StringName uid = "uid";

		public static readonly StringName levelConfig = "levelConfig";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string PATH = "user://Csharp/Diy";

	private static Texture2D _izIcon;

	private static Texture2D _iz2Icon;

	private static Texture2D _vrIcon;

	private Label _nameLabel;

	private TextureRect _mapTexture;

	private TextureRect _iconTexture;

	public string uid;

	public TowerDefenseLevelConfig levelConfig;

	private static Texture2D IZ_ICON => _izIcon ?? (_izIcon = GD.Load<Texture2D>("uid://ciyj1718ypbih"));

	private static Texture2D IZ2_ICON => _iz2Icon ?? (_iz2Icon = GD.Load<Texture2D>("uid://b8rsot26eb7od"));

	private static Texture2D Vr_ICON => _vrIcon ?? (_vrIcon = GD.Load<Texture2D>("uid://24j6iw1ww08b"));

	public event SelectEventHandler OnSelect;

	public event DeleteEventHandler OnDelete;

	public override void _Ready()
	{
		GetNode<TextureButton>("SelectButton").Pressed += SelectButtonPressed;
		GetNode<TextureButton>("DeleteButton").Pressed += DeleteButtonPressed;
	}

	public void Init(string _uid)
	{
		uid = _uid;
		_nameLabel = GetNode<Label>("%NameLabel");
		_mapTexture = GetNode<TextureRect>("%MapTexture");
		_iconTexture = GetNode<TextureRect>("%IconTexture");
		string text = "user://Csharp/Diy/" + uid + ".tres";
		if ((LevelEditorStage.MigrateDiyLevelTresIfNeeded(text) ? ResourceLoader.Load(text, "", ResourceLoader.CacheMode.Ignore) : GD.Load(text)) is TowerDefenseLevelConfig towerDefenseLevelConfig)
		{
			levelConfig = towerDefenseLevelConfig;
			_nameLabel.Text = levelConfig.levelName;
			TowerDefenseMapConfig mapConfig = TowerDefenseManager.Instance.GetMapConfig(levelConfig.map);
			TowerDefenseMapConfig.ApplyMapPreviewTexture(_mapTexture, mapConfig?.GetMapThumbnail());
			switch (levelConfig.finishMethod)
			{
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE:
				_iconTexture.Visible = false;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE:
				_iconTexture.Visible = true;
				_iconTexture.Texture = Vr_ICON;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM:
				_iconTexture.Visible = true;
				_iconTexture.Texture = IZ_ICON;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2:
				_iconTexture.Visible = true;
				_iconTexture.Texture = IZ2_ICON;
				break;
			case TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ:
				break;
			}
		}
	}

	public void DeleteButtonPressed()
	{
		DialogBoxMyLevelDelete dialogBoxMyLevelDelete = (DialogBoxMyLevelDelete)DialogManager.Instance.DialogCreate("MyLevelDelete");
		string _uid = uid;
		dialogBoxMyLevelDelete.OnPressDelete += () =>
		{
			OnDelete?.Invoke(_uid);
			QueueFree();
		};
	}

	public void SelectButtonPressed()
	{
		OnSelect?.Invoke(uid);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_uid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeleteButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteButtonPressed && args.Count == 0)
		{
			DeleteButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectButtonPressed && args.Count == 0)
		{
			SelectButtonPressed();
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
		if (method == MethodName.DeleteButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SelectButtonPressed)
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
		if (name == PropertyName._iconTexture)
		{
			_iconTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.uid)
		{
			uid = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
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
		if (name == PropertyName._iconTexture)
		{
			value = VariantUtils.CreateFrom(in _iconTexture);
			return true;
		}
		if (name == PropertyName.uid)
		{
			value = VariantUtils.CreateFrom(in uid);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom(in levelConfig);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._iconTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.uid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nameLabel, Variant.From(in _nameLabel));
		info.AddProperty(PropertyName._mapTexture, Variant.From(in _mapTexture));
		info.AddProperty(PropertyName._iconTexture, Variant.From(in _iconTexture));
		info.AddProperty(PropertyName.uid, Variant.From(in uid));
		info.AddProperty(PropertyName.levelConfig, Variant.From(in levelConfig));
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
		if (info.TryGetProperty(PropertyName._iconTexture, out var value3))
		{
			_iconTexture = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.uid, out var value4))
		{
			uid = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelConfig, out var value5))
		{
			levelConfig = value5.As<TowerDefenseLevelConfig>();
		}
	}
}
