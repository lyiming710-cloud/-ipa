using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Progress/Manager/TowerDefenseProgressManager.cs")]
public class TowerDefenseProgressManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupUI = "SetupUI";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName levelNameLabel = "levelNameLabel";

		public static readonly StringName difficultLabel = "difficultLabel";

		public static readonly StringName survivalLabel = "survivalLabel";

		public static readonly StringName progressMeter = "progressMeter";

		public static readonly StringName progressBar = "progressBar";

		public static readonly StringName progressFeature = "progressFeature";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public Label levelNameLabel;

	public Label difficultLabel;

	public Label survivalLabel;

	public GeneralProgressMeter progressMeter;

	public TextureProgressBar progressBar;

	public TowerDefenseBattleFeatureProgress progressFeature;

	public override void _Ready()
	{
		levelNameLabel = GetNode<Label>("%LevelNameLabel");
		difficultLabel = GetNode<Label>("%DifficultLabel");
		survivalLabel = GetNode<Label>("%SurvivalLabel");
		progressMeter = GetNode<GeneralProgressMeter>("%GeneralProgressMeter");
		progressBar = progressMeter.GetNode<TextureProgressBar>("%ProgressBar");
	}

	public void SetupUI(string difficult, string levelName, bool isSurvival = false, int survivalRoundNum = 0)
	{
		string newValue = "";
		switch (difficult)
		{
		case "Normal":
			newValue = "正常";
			difficultLabel.Modulate = new Color(difficultLabel.Modulate.R, 1f, difficultLabel.Modulate.B, difficultLabel.Modulate.A);
			break;
		case "Difficult":
			newValue = "困难";
			difficultLabel.Modulate = new Color(difficultLabel.Modulate.R, 0f, difficultLabel.Modulate.B, difficultLabel.Modulate.A);
			break;
		case "Ultimate":
			newValue = "极限";
			difficultLabel.Modulate = new Color(difficultLabel.Modulate.R, 0f, difficultLabel.Modulate.B, difficultLabel.Modulate.A);
			break;
		}
		levelNameLabel.Text = levelName;
		difficultLabel.Text = Tr("INGAME_DIFFICULT").Replace("%s", newValue);
		if (isSurvival)
		{
			survivalLabel.Text = Tr("PLAYERS_SURVIVAL_LEVEL_DESCRIBE").Replace("%d", survivalRoundNum.ToString());
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "difficult", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "levelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isSurvival", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "survivalRoundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetupUI && args.Count == 4)
		{
			SetupUI(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
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
		if (method == MethodName.SetupUI)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.levelNameLabel)
		{
			levelNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.difficultLabel)
		{
			difficultLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.survivalLabel)
		{
			survivalLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.progressMeter)
		{
			progressMeter = VariantUtils.ConvertTo<GeneralProgressMeter>(in value);
			return true;
		}
		if (name == PropertyName.progressBar)
		{
			progressBar = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName.progressFeature)
		{
			progressFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureProgress>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.levelNameLabel)
		{
			value = VariantUtils.CreateFrom(in levelNameLabel);
			return true;
		}
		if (name == PropertyName.difficultLabel)
		{
			value = VariantUtils.CreateFrom(in difficultLabel);
			return true;
		}
		if (name == PropertyName.survivalLabel)
		{
			value = VariantUtils.CreateFrom(in survivalLabel);
			return true;
		}
		if (name == PropertyName.progressMeter)
		{
			value = VariantUtils.CreateFrom(in progressMeter);
			return true;
		}
		if (name == PropertyName.progressBar)
		{
			value = VariantUtils.CreateFrom(in progressBar);
			return true;
		}
		if (name == PropertyName.progressFeature)
		{
			value = VariantUtils.CreateFrom(in progressFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.levelNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.difficultLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.survivalLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressMeter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.levelNameLabel, Variant.From(in levelNameLabel));
		info.AddProperty(PropertyName.difficultLabel, Variant.From(in difficultLabel));
		info.AddProperty(PropertyName.survivalLabel, Variant.From(in survivalLabel));
		info.AddProperty(PropertyName.progressMeter, Variant.From(in progressMeter));
		info.AddProperty(PropertyName.progressBar, Variant.From(in progressBar));
		info.AddProperty(PropertyName.progressFeature, Variant.From(in progressFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.levelNameLabel, out var value))
		{
			levelNameLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.difficultLabel, out var value2))
		{
			difficultLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.survivalLabel, out var value3))
		{
			survivalLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.progressMeter, out var value4))
		{
			progressMeter = value4.As<GeneralProgressMeter>();
		}
		if (info.TryGetProperty(PropertyName.progressBar, out var value5))
		{
			progressBar = value5.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName.progressFeature, out var value6))
		{
			progressFeature = value6.As<TowerDefenseBattleFeatureProgress>();
		}
	}
}
