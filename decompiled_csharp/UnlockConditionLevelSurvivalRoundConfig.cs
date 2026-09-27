using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/UnlockCondition/UnlockConditionLevelSurvivalRoundConfig.cs")]
public class UnlockConditionLevelSurvivalRoundConfig : UnlockConditionBaseConfig
{
	public new class MethodName : UnlockConditionBaseConfig.MethodName
	{
		public new static readonly StringName Check = "Check";

		public static readonly StringName CheckProgress = "CheckProgress";

		public static readonly StringName HasRequiredRound = "HasRequiredRound";
	}

	public new class PropertyName : UnlockConditionBaseConfig.PropertyName
	{
		public static readonly StringName levelSaveKey = "levelSaveKey";

		public static readonly StringName roundNum = "roundNum";
	}

	public new class SignalName : UnlockConditionBaseConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string levelSaveKey = "";

	[Export(PropertyHint.None, "")]
	public int roundNum = 10;

	public override bool Check()
	{
		if (GameSaveManager.Instance == null || !GameSaveManager.Instance.HasLevelProgress(levelSaveKey))
		{
			return false;
		}
		return CheckProgress(GameSaveManager.Instance.GetLevelProgress(levelSaveKey));
	}

	public bool CheckProgress(TowerDefenseLevelSaveConfigCSharp progress)
	{
		if (progress == null)
		{
			return false;
		}
		if (TryGetSaveSection(progress.featureSave, "Wave", out var section) && HasRequiredRound(section))
		{
			return true;
		}
		if (TryGetSaveSection(progress.processSave, "main", out var section2))
		{
			return HasRequiredRound(section2);
		}
		return false;
	}

	private bool HasRequiredRound(Dictionary data)
	{
		if (data.GetValueOrDefault("isSurvival", false).AsBool())
		{
			return data.GetValueOrDefault("survivalRoundNum", 0).AsInt32() >= roundNum;
		}
		return false;
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
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckProgress, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "progress", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasRequiredRound, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Check && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Check());
			return true;
		}
		if (method == MethodName.CheckProgress && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckProgress(VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[0])));
			return true;
		}
		if (method == MethodName.HasRequiredRound && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRequiredRound(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.CheckProgress)
		{
			return true;
		}
		if (method == MethodName.HasRequiredRound)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.levelSaveKey)
		{
			levelSaveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.roundNum)
		{
			roundNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.levelSaveKey)
		{
			value = VariantUtils.CreateFrom(in levelSaveKey);
			return true;
		}
		if (name == PropertyName.roundNum)
		{
			value = VariantUtils.CreateFrom(in roundNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.levelSaveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.roundNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.levelSaveKey, Variant.From(in levelSaveKey));
		info.AddProperty(PropertyName.roundNum, Variant.From(in roundNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.levelSaveKey, out var value))
		{
			levelSaveKey = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.roundNum, out var value2))
		{
			roundNum = value2.As<int>();
		}
	}
}
