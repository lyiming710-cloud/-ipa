using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/ResourceRuntime/StateMachineSnapshot.cs")]
public class StateMachineSnapshot : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName IsSupportedExpressionVariant = "IsSupportedExpressionVariant";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName DefinitionId = "DefinitionId";

		public static readonly StringName SchemaVersion = "SchemaVersion";

		public static readonly StringName ContentHash = "ContentHash";

		public static readonly StringName ActiveStateIds = "ActiveStateIds";

		public static readonly StringName HistoryStateIds = "HistoryStateIds";

		public static readonly StringName PendingTransitionIds = "PendingTransitionIds";

		public static readonly StringName PendingDelayRemaining = "PendingDelayRemaining";

		public static readonly StringName ExpressionProperties = "ExpressionProperties";

		public static readonly StringName Revision = "Revision";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string DefinitionId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public int SchemaVersion { get; set; }

	[Export(PropertyHint.None, "")]
	public string ContentHash { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public Array<string> ActiveStateIds { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> HistoryStateIds { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<string> PendingTransitionIds { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, double> PendingDelayRemaining { get; set; } = new Godot.Collections.Dictionary<string, double>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<StringName, Variant> ExpressionProperties { get; set; } = new Godot.Collections.Dictionary<StringName, Variant>();

	[Export(PropertyHint.None, "")]
	public long Revision { get; set; }

	public static bool IsSupportedExpressionVariant(Variant.Type type)
	{
		if ((ulong)type <= 22uL)
		{
			switch ((int)type)
			{
			case 0:
				return true;
			case 1:
				return true;
			case 2:
				return true;
			case 3:
				return true;
			case 4:
				return true;
			case 21:
				return true;
			case 22:
				return true;
			case 5:
				return true;
			case 6:
				return true;
			case 7:
				return true;
			case 8:
				return true;
			case 9:
				return true;
			case 10:
				return true;
			case 11:
				return true;
			case 12:
				return true;
			case 13:
				return true;
			case 14:
				return true;
			case 15:
				return true;
			case 16:
				return true;
			case 17:
				return true;
			case 18:
				return true;
			case 19:
				return true;
			case 20:
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.IsSupportedExpressionVariant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSupportedExpressionVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedExpressionVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSupportedExpressionVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedExpressionVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsSupportedExpressionVariant)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.DefinitionId)
		{
			DefinitionId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SchemaVersion)
		{
			SchemaVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ContentHash)
		{
			ContentHash = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ActiveStateIds)
		{
			ActiveStateIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.HistoryStateIds)
		{
			HistoryStateIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.PendingTransitionIds)
		{
			PendingTransitionIds = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.PendingDelayRemaining)
		{
			PendingDelayRemaining = VariantUtils.ConvertToDictionary<string, double>(in value);
			return true;
		}
		if (name == PropertyName.ExpressionProperties)
		{
			ExpressionProperties = VariantUtils.ConvertToDictionary<StringName, Variant>(in value);
			return true;
		}
		if (name == PropertyName.Revision)
		{
			Revision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.DefinitionId)
		{
			from = DefinitionId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SchemaVersion)
		{
			value = VariantUtils.CreateFrom<int>(SchemaVersion);
			return true;
		}
		if (name == PropertyName.ContentHash)
		{
			from = ContentHash;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ActiveStateIds)
		{
			value = VariantUtils.CreateFromArray(ActiveStateIds);
			return true;
		}
		if (name == PropertyName.HistoryStateIds)
		{
			value = VariantUtils.CreateFromArray(HistoryStateIds);
			return true;
		}
		if (name == PropertyName.PendingTransitionIds)
		{
			value = VariantUtils.CreateFromArray(PendingTransitionIds);
			return true;
		}
		if (name == PropertyName.PendingDelayRemaining)
		{
			value = VariantUtils.CreateFromDictionary(PendingDelayRemaining);
			return true;
		}
		if (name == PropertyName.ExpressionProperties)
		{
			value = VariantUtils.CreateFromDictionary(ExpressionProperties);
			return true;
		}
		if (name == PropertyName.Revision)
		{
			value = VariantUtils.CreateFrom<long>(Revision);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.DefinitionId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SchemaVersion, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ContentHash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.ActiveStateIds, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.HistoryStateIds, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.PendingTransitionIds, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.PendingDelayRemaining, PropertyHint.TypeString, "4/0:;3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.ExpressionProperties, PropertyHint.TypeString, "21/0:;0/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Revision, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.DefinitionId, Variant.From<string>(DefinitionId));
		info.AddProperty(PropertyName.SchemaVersion, Variant.From<int>(SchemaVersion));
		info.AddProperty(PropertyName.ContentHash, Variant.From<string>(ContentHash));
		info.AddProperty(PropertyName.ActiveStateIds, Variant.CreateFrom(ActiveStateIds));
		info.AddProperty(PropertyName.HistoryStateIds, Variant.CreateFrom(HistoryStateIds));
		info.AddProperty(PropertyName.PendingTransitionIds, Variant.CreateFrom(PendingTransitionIds));
		info.AddProperty(PropertyName.PendingDelayRemaining, Variant.CreateFrom(PendingDelayRemaining));
		info.AddProperty(PropertyName.ExpressionProperties, Variant.CreateFrom(ExpressionProperties));
		info.AddProperty(PropertyName.Revision, Variant.From<long>(Revision));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.DefinitionId, out var value))
		{
			DefinitionId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SchemaVersion, out var value2))
		{
			SchemaVersion = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ContentHash, out var value3))
		{
			ContentHash = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ActiveStateIds, out var value4))
		{
			ActiveStateIds = value4.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.HistoryStateIds, out var value5))
		{
			HistoryStateIds = value5.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.PendingTransitionIds, out var value6))
		{
			PendingTransitionIds = value6.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.PendingDelayRemaining, out var value7))
		{
			PendingDelayRemaining = value7.AsGodotDictionary<string, double>();
		}
		if (info.TryGetProperty(PropertyName.ExpressionProperties, out var value8))
		{
			ExpressionProperties = value8.AsGodotDictionary<StringName, Variant>();
		}
		if (info.TryGetProperty(PropertyName.Revision, out var value9))
		{
			Revision = value9.As<long>();
		}
	}
}
