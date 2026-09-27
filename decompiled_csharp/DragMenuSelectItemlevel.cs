using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DragMenu/Select/Level/DragMenuSelectItemlevel.cs")]
public class DragMenuSelectItemlevel : DragMenuSelectItem
{
	public new class MethodName : DragMenuSelectItem.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : DragMenuSelectItem.PropertyName
	{
		public static readonly StringName cupSprite = "cupSprite";

		public static readonly StringName survivalLabel = "survivalLabel";
	}

	public new class SignalName : DragMenuSelectItem.SignalName
	{
	}

	private static Texture2D _silverTrophy;

	private static Texture2D _goldTrophy;

	private static Texture2D _diamondTrophy;

	private static Texture2D _moreGameStar;

	private Sprite2D cupSprite;

	private Label survivalLabel;

	private static Texture2D SILVER_TROPHY => _silverTrophy ?? (_silverTrophy = GD.Load<Texture2D>("uid://wrip0sbul8kd"));

	private static Texture2D GOLD_TROPHY => _goldTrophy ?? (_goldTrophy = GD.Load<Texture2D>("uid://bilmjv3kylwye"));

	private static Texture2D DIAMOND_TROPHY => _diamondTrophy ?? (_diamondTrophy = GD.Load<Texture2D>("uid://bg1gnn1guh1sl"));

	private static Texture2D MORE_GAME_STAR => _moreGameStar ?? (_moreGameStar = GD.Load<Texture2D>("uid://cvth13h0ui3cq"));

	public override void _Ready()
	{
		base._Ready();
		cupSprite = GetNode<Sprite2D>("%CupSprite");
		survivalLabel = GetNode<Label>("%SurvivalLabel");
	}

	public void Init(string levelKey, XWModLevelIdentity identity = null)
	{
		cupSprite.Texture = null;
		survivalLabel.Visible = false;
		survivalLabel.Text = string.Empty;
		Dictionary dictionary = ((identity == null) ? GameSaveManager.Instance.GetLevelValue(levelKey) : XWModPlayerProgressService.GetLevel(identity));
		bool flag = dictionary.GetValueOrDefault("Difficult", false).AsBool() || dictionary.GetValueOrDefault("Ultimate", false).AsBool();
		bool flag2 = Global.Instance.currentLevelChoose != "Puzzle" && Global.Instance.currentLevelChoose != "MiniGames";
		if (flag2 & flag)
		{
			cupSprite.Texture = DIAMOND_TROPHY;
		}
		else if (flag2 && dictionary.GetValueOrDefault("Mower", false).AsBool())
		{
			cupSprite.Texture = GOLD_TROPHY;
		}
		else if (dictionary.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary().GetValueOrDefault("Finish", 0)
			.AsInt32() > 0)
		{
			cupSprite.Texture = (flag2 ? SILVER_TROPHY : MORE_GAME_STAR);
		}
		TowerDefenseLevelSaveConfigCSharp levelProgress = GameSaveManager.Instance.GetLevelProgress(levelKey, identity);
		if (levelProgress != null)
		{
			if (!TryGetSaveSection(levelProgress.featureSave, "Wave", out var section))
			{
				TryGetSaveSection(levelProgress.processSave, "main", out section);
			}
			if (section != null && section.GetValueOrDefault("isSurvival", false).AsBool())
			{
				survivalLabel.Visible = true;
				survivalLabel.Text = string.Format("{0}轮完成", section.GetValueOrDefault("survivalRoundNum", 0).AsInt32());
			}
		}
	}

	private static bool TryGetSaveSection(Dictionary container, string sectionName, out Dictionary section)
	{
		if (container != null)
		{
			foreach (Variant key in container.Keys)
			{
				if (!(key.AsString() != sectionName))
				{
					section = container[key].AsGodotDictionary();
					return section != null;
				}
			}
		}
		section = null;
		return false;
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
		if (name == PropertyName.survivalLabel)
		{
			survivalLabel = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName.survivalLabel)
		{
			value = VariantUtils.CreateFrom(in survivalLabel);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.cupSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.survivalLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.cupSprite, Variant.From(in cupSprite));
		info.AddProperty(PropertyName.survivalLabel, Variant.From(in survivalLabel));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.cupSprite, out var value))
		{
			cupSprite = value.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.survivalLabel, out var value2))
		{
			survivalLabel = value2.As<Label>();
		}
	}
}
