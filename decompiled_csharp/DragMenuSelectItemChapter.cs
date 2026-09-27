using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DragMenu/Select/Chapter/DragMenuSelectItemChapter.cs")]
public class DragMenuSelectItemChapter : DragMenuSelectItem
{
	public new class MethodName : DragMenuSelectItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : DragMenuSelectItem.PropertyName
	{
		public static readonly StringName cupSprite = "cupSprite";
	}

	public new class SignalName : DragMenuSelectItem.SignalName
	{
	}

	private static Texture2D _silverMedal;

	private static Texture2D _goldMedal;

	private static Texture2D _dimondMedal;

	private static Texture2D _moreGameTrophy;

	private Sprite2D cupSprite;

	private static Texture2D SILVER_MEDAL => _silverMedal ?? (_silverMedal = GD.Load<Texture2D>("uid://bch4iu18n3i44"));

	private static Texture2D GOLD_MEDAL => _goldMedal ?? (_goldMedal = GD.Load<Texture2D>("uid://dqgpqtvy8kej3"));

	private static Texture2D DIMOND_MEDAL => _dimondMedal ?? (_dimondMedal = GD.Load<Texture2D>("uid://biqnbmrdvhih7"));

	private static Texture2D MORE_GAME_TROPHY => _moreGameTrophy ?? (_moreGameTrophy = GD.Load<Texture2D>("uid://d3l1vjo8lnm8t"));

	public override void _Ready()
	{
		base._Ready();
		cupSprite = GetNode<Sprite2D>("%CupSprite");
	}

	public void Init(Dictionary chapter, XWModLevelIdentity identity = null)
	{
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		Array array = chapter["Level"].AsGodotArray();
		if (array.Count <= 0)
		{
			flag = false;
			flag2 = false;
			flag3 = false;
		}
		for (int i = 0; i < array.Count; i++)
		{
			string text = array[i].AsGodotDictionary()["SaveKey"].AsString();
			Dictionary dictionary = ((identity == null) ? GameSaveManager.Instance.GetLevelValue(text) : XWModPlayerProgressService.GetLevel(identity with
			{
				LevelSaveKey = text
			}));
			Dictionary dictionary2 = dictionary.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
			if (!dictionary.GetValueOrDefault("Difficult", false).AsBool() && !dictionary.GetValueOrDefault("Ultimate", false).AsBool())
			{
				flag = false;
			}
			if (!dictionary.GetValueOrDefault("Mower", false).AsBool())
			{
				flag2 = false;
			}
			if (dictionary2.GetValueOrDefault("Finish", 0).AsInt32() <= 0)
			{
				flag3 = false;
			}
		}
		if (Global.Instance.currentLevelChoose != "Puzzle" && Global.Instance.currentLevelChoose != "MiniGames")
		{
			if (flag3)
			{
				cupSprite.Texture = SILVER_MEDAL;
			}
			if (flag2)
			{
				cupSprite.Texture = GOLD_MEDAL;
			}
			if (flag)
			{
				cupSprite.Texture = DIMOND_MEDAL;
			}
		}
		else if (flag3 | flag2 | flag)
		{
			cupSprite.Texture = MORE_GAME_TROPHY;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.cupSprite)
		{
			cupSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.cupSprite)
		{
			value = VariantUtils.CreateFrom(in cupSprite);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.cupSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.cupSprite, Variant.From(in cupSprite));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.cupSprite, out var value))
		{
			cupSprite = value.As<Sprite2D>();
		}
	}
}
