using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Behavior/Resource/BehaviorDefinitionBase.cs")]
public abstract class BehaviorDefinitionBase : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetDiagnosticName = "GetDiagnosticName";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName BehaviorTypeId = "BehaviorTypeId";

		public static readonly StringName DefinitionId = "DefinitionId";

		public static readonly StringName InstanceId = "InstanceId";

		public static readonly StringName SchemaVersion = "SchemaVersion";

		public static readonly StringName InitiallyEnabled = "InitiallyEnabled";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName BehaviorTypeId { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public StringName DefinitionId { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public StringName InstanceId { get; set; } = "";

	[Export(PropertyHint.Range, "1,65535,1")]
	public int SchemaVersion { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public bool InitiallyEnabled { get; set; } = true;

	public string GetDiagnosticName()
	{
		if (InstanceId != null && !InstanceId.IsEmpty)
		{
			return InstanceId.ToString();
		}
		if (DefinitionId != null && !DefinitionId.IsEmpty)
		{
			return DefinitionId.ToString();
		}
		if (BehaviorTypeId != null && !BehaviorTypeId.IsEmpty)
		{
			return BehaviorTypeId.ToString();
		}
		return GetType().Name;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.GetDiagnosticName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetDiagnosticName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetDiagnosticName());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetDiagnosticName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.BehaviorTypeId)
		{
			BehaviorTypeId = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.DefinitionId)
		{
			DefinitionId = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.InstanceId)
		{
			InstanceId = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.SchemaVersion)
		{
			SchemaVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.InitiallyEnabled)
		{
			InitiallyEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		StringName from;
		if (name == PropertyName.BehaviorTypeId)
		{
			from = BehaviorTypeId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DefinitionId)
		{
			from = DefinitionId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InstanceId)
		{
			from = InstanceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SchemaVersion)
		{
			value = VariantUtils.CreateFrom<int>(SchemaVersion);
			return true;
		}
		if (name == PropertyName.InitiallyEnabled)
		{
			value = VariantUtils.CreateFrom<bool>(InitiallyEnabled);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.BehaviorTypeId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.DefinitionId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.InstanceId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SchemaVersion, PropertyHint.Range, "1,65535,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.InitiallyEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.BehaviorTypeId, Variant.From<StringName>(BehaviorTypeId));
		info.AddProperty(PropertyName.DefinitionId, Variant.From<StringName>(DefinitionId));
		info.AddProperty(PropertyName.InstanceId, Variant.From<StringName>(InstanceId));
		info.AddProperty(PropertyName.SchemaVersion, Variant.From<int>(SchemaVersion));
		info.AddProperty(PropertyName.InitiallyEnabled, Variant.From<bool>(InitiallyEnabled));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.BehaviorTypeId, out var value))
		{
			BehaviorTypeId = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.DefinitionId, out var value2))
		{
			DefinitionId = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.InstanceId, out var value3))
		{
			InstanceId = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.SchemaVersion, out var value4))
		{
			SchemaVersion = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.InitiallyEnabled, out var value5))
		{
			InitiallyEnabled = value5.As<bool>();
		}
	}
}
