using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/ModSystem/XWModProgressUnlockCondition.cs")]
public class XWModProgressUnlockCondition : UnlockConditionBaseConfig
{
	public new class MethodName : UnlockConditionBaseConfig.MethodName
	{
		public new static readonly StringName Check = "Check";
	}

	public new class PropertyName : UnlockConditionBaseConfig.PropertyName
	{
		public static readonly StringName modId = "modId";

		public static readonly StringName catalogKey = "catalogKey";

		public static readonly StringName levelSaveKey = "levelSaveKey";

		public static readonly StringName requiredFinishCount = "requiredFinishCount";
	}

	public new class SignalName : UnlockConditionBaseConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string modId = "";

	[Export(PropertyHint.None, "")]
	public string catalogKey = "";

	[Export(PropertyHint.None, "")]
	public string levelSaveKey = "";

	[Export(PropertyHint.Range, "1,1000,1")]
	public int requiredFinishCount = 1;

	public override bool Check()
	{
		if (!string.IsNullOrWhiteSpace(modId) && !string.IsNullOrWhiteSpace(catalogKey) && !string.IsNullOrWhiteSpace(levelSaveKey))
		{
			return XWModPlayerProgressService.FinishCount(new XWModLevelIdentity(modId, catalogKey, levelSaveKey, "Normal")) >= requiredFinishCount;
		}
		return false;
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
		if (name == PropertyName.modId)
		{
			modId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.catalogKey)
		{
			catalogKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.levelSaveKey)
		{
			levelSaveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.requiredFinishCount)
		{
			requiredFinishCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.modId)
		{
			value = VariantUtils.CreateFrom(in modId);
			return true;
		}
		if (name == PropertyName.catalogKey)
		{
			value = VariantUtils.CreateFrom(in catalogKey);
			return true;
		}
		if (name == PropertyName.levelSaveKey)
		{
			value = VariantUtils.CreateFrom(in levelSaveKey);
			return true;
		}
		if (name == PropertyName.requiredFinishCount)
		{
			value = VariantUtils.CreateFrom(in requiredFinishCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.modId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.catalogKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.levelSaveKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.requiredFinishCount, PropertyHint.Range, "1,1000,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.modId, Variant.From(in modId));
		info.AddProperty(PropertyName.catalogKey, Variant.From(in catalogKey));
		info.AddProperty(PropertyName.levelSaveKey, Variant.From(in levelSaveKey));
		info.AddProperty(PropertyName.requiredFinishCount, Variant.From(in requiredFinishCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.modId, out var value))
		{
			modId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.catalogKey, out var value2))
		{
			catalogKey = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.levelSaveKey, out var value3))
		{
			levelSaveKey = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.requiredFinishCount, out var value4))
		{
			requiredFinishCount = value4.As<int>();
		}
	}
}
