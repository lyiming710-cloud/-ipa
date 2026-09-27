using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/LevelEditor/QuizLevelItem/LevelEditorQuizLevelItem.cs")]
public class LevelEditorQuizLevelItem : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Init = "Init";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _nameLabel = "_nameLabel";

		public static readonly StringName _mapTexture = "_mapTexture";

		public static readonly StringName map = "map";

		public static readonly StringName levelConfig = "levelConfig";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private Label _nameLabel;

	private TextureRect _mapTexture;

	public string map;

	public TowerDefenseLevelConfig levelConfig;

	public void Init(string _map)
	{
		_nameLabel = GetNode<Label>("%NameLabel");
		_mapTexture = GetNode<TextureRect>("%MapTexture");
		map = _map;
		TowerDefenseMapConfig mapConfig = TowerDefenseManager.Instance.GetMapConfig(map);
		TowerDefenseMapConfig.ApplyMapPreviewTexture(_mapTexture, mapConfig?.GetMapThumbnail());
		_nameLabel.Text = mapConfig?.translate ?? "";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_map", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
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
		if (name == PropertyName.map)
		{
			map = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.map)
		{
			value = VariantUtils.CreateFrom(in map);
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
			new PropertyInfo(Variant.Type.String, PropertyName.map, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nameLabel, Variant.From(in _nameLabel));
		info.AddProperty(PropertyName._mapTexture, Variant.From(in _mapTexture));
		info.AddProperty(PropertyName.map, Variant.From(in map));
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
		if (info.TryGetProperty(PropertyName.map, out var value3))
		{
			map = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelConfig, out var value4))
		{
			levelConfig = value4.As<TowerDefenseLevelConfig>();
		}
	}
}
