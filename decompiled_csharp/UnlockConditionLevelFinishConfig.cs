using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/UnlockCondition/UnlockConditionLevelFinishConfig.cs")]
public class UnlockConditionLevelFinishConfig : UnlockConditionBaseConfig
{
	public new class MethodName : UnlockConditionBaseConfig.MethodName
	{
		public new static readonly StringName Check = "Check";
	}

	public new class PropertyName : UnlockConditionBaseConfig.PropertyName
	{
		public static readonly StringName levelSaveKey = "levelSaveKey";
	}

	public new class SignalName : UnlockConditionBaseConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string levelSaveKey = "";

	public override bool Check()
	{
		if (levelSaveKey == "Unlock")
		{
			return true;
		}
		return GameSaveManager.Instance.GetLevelValue(levelSaveKey).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary()
			.GetValueOrDefault("Finish", 0.0)
			.AsDouble() > 0.0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Check)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.levelSaveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.levelSaveKey, Variant.From(in levelSaveKey));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.levelSaveKey, out var value))
		{
			levelSaveKey = value.As<string>();
		}
	}
}
