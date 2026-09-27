using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/StarExchange/StarExchangeContainner/StarExchangeContainner.cs")]
public class StarExchangeContainner : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName AddItem = "AddItem";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName chapterTexture = "chapterTexture";

		public static readonly StringName finishNumLabel = "finishNumLabel";

		public static readonly StringName itemContainer = "itemContainer";

		public static readonly StringName data = "data";

		public static readonly StringName finishNum = "finishNum";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static PackedScene _starExchangeItem;

	private TextureRect chapterTexture;

	private Label finishNumLabel;

	private HBoxContainer itemContainer;

	public Dictionary data;

	public int finishNum;

	private static PackedScene STAR_EXCHANGE_ITEM => _starExchangeItem ?? (_starExchangeItem = GD.Load<PackedScene>("uid://byuv6jflrtcul"));

	public override void _Ready()
	{
		chapterTexture = GetNode<TextureRect>("%ChapterTexture");
		finishNumLabel = GetNode<Label>("%FinishNumLabel");
		itemContainer = GetNode<HBoxContainer>("%ItemContainer");
	}

	public void Init(Dictionary _data)
	{
		data = _data;
		chapterTexture.Texture = GD.Load<Texture2D>(data["ChapterImage"].AsString());
		finishNum = TowerDefenseManager.Instance.GetLevelChapterFinishNum(data["LevelList"].AsString(), data["Chapter"].AsString());
		finishNumLabel.Text = $"x{finishNum}";
		foreach (Variant item in data["Award"].AsGodotArray())
		{
			AddItem(item.AsGodotDictionary());
		}
	}

	public void AddItem(Dictionary _data)
	{
		StarExchangeItem starExchangeItem = (StarExchangeItem)STAR_EXCHANGE_ITEM.Instantiate(PackedScene.GenEditState.Disabled);
		itemContainer.AddChild(starExchangeItem, forceReadableName: false, InternalMode.Disabled);
		starExchangeItem.Init(_data, finishNum);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.AddItem && args.Count == 1)
		{
			AddItem(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.AddItem)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.chapterTexture)
		{
			chapterTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName.finishNumLabel)
		{
			finishNumLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.itemContainer)
		{
			itemContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			finishNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.chapterTexture)
		{
			value = VariantUtils.CreateFrom(in chapterTexture);
			return true;
		}
		if (name == PropertyName.finishNumLabel)
		{
			value = VariantUtils.CreateFrom(in finishNumLabel);
			return true;
		}
		if (name == PropertyName.itemContainer)
		{
			value = VariantUtils.CreateFrom(in itemContainer);
			return true;
		}
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom(in data);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			value = VariantUtils.CreateFrom(in finishNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.chapterTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.finishNumLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.itemContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finishNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.chapterTexture, Variant.From(in chapterTexture));
		info.AddProperty(PropertyName.finishNumLabel, Variant.From(in finishNumLabel));
		info.AddProperty(PropertyName.itemContainer, Variant.From(in itemContainer));
		info.AddProperty(PropertyName.data, Variant.From(in data));
		info.AddProperty(PropertyName.finishNum, Variant.From(in finishNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.chapterTexture, out var value))
		{
			chapterTexture = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName.finishNumLabel, out var value2))
		{
			finishNumLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.itemContainer, out var value3))
		{
			itemContainer = value3.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.data, out var value4))
		{
			data = value4.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.finishNum, out var value5))
		{
			finishNum = value5.As<int>();
		}
	}
}
