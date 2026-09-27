using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/ResourceRuntime/StateMachineStateDefinition.cs")]
public class StateMachineStateDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetLifecycleCallbackKey = "GetLifecycleCallbackKey";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName StableId = "StableId";

		public static readonly StringName DisplayName = "DisplayName";

		public static readonly StringName Kind = "Kind";

		public static readonly StringName ParentId = "ParentId";

		public static readonly StringName InitialChildId = "InitialChildId";

		public static readonly StringName ProcessFlags = "ProcessFlags";

		public static readonly StringName CallbackKey = "CallbackKey";

		public static readonly StringName EnterCallbackKey = "EnterCallbackKey";

		public static readonly StringName ExitCallbackKey = "ExitCallbackKey";

		public static readonly StringName ProcessCallbackKey = "ProcessCallbackKey";

		public static readonly StringName PhysicsProcessCallbackKey = "PhysicsProcessCallbackKey";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string StableId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public StringName DisplayName { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public StateMachineStateKind Kind { get; set; }

	[Export(PropertyHint.None, "")]
	public string ParentId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public string InitialChildId { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public StateMachineProcessFlags ProcessFlags { get; set; }

	[Export(PropertyHint.None, "")]
	public StringName CallbackKey { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public StringName EnterCallbackKey { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public StringName ExitCallbackKey { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public StringName ProcessCallbackKey { get; set; } = new StringName();

	[Export(PropertyHint.None, "")]
	public StringName PhysicsProcessCallbackKey { get; set; } = new StringName();

	public StringName GetLifecycleCallbackKey(StateMachineCallbackPhase phase)
	{
		StringName stringName = phase switch
		{
			StateMachineCallbackPhase.Enter => EnterCallbackKey, 
			StateMachineCallbackPhase.Exit => ExitCallbackKey, 
			StateMachineCallbackPhase.Process => ProcessCallbackKey, 
			StateMachineCallbackPhase.PhysicsProcess => PhysicsProcessCallbackKey, 
			_ => new StringName(), 
		};
		if (!string.IsNullOrWhiteSpace(stringName.ToString()))
		{
			return stringName;
		}
		return CallbackKey;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.GetLifecycleCallbackKey, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetLifecycleCallbackKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetLifecycleCallbackKey(VariantUtils.ConvertTo<StateMachineCallbackPhase>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetLifecycleCallbackKey)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.StableId)
		{
			StableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.DisplayName)
		{
			DisplayName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.Kind)
		{
			Kind = VariantUtils.ConvertTo<StateMachineStateKind>(in value);
			return true;
		}
		if (name == PropertyName.ParentId)
		{
			ParentId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.InitialChildId)
		{
			InitialChildId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ProcessFlags)
		{
			ProcessFlags = VariantUtils.ConvertTo<StateMachineProcessFlags>(in value);
			return true;
		}
		if (name == PropertyName.CallbackKey)
		{
			CallbackKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.EnterCallbackKey)
		{
			EnterCallbackKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.ExitCallbackKey)
		{
			ExitCallbackKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.ProcessCallbackKey)
		{
			ProcessCallbackKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.PhysicsProcessCallbackKey)
		{
			PhysicsProcessCallbackKey = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.StableId)
		{
			from = StableId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		StringName from2;
		if (name == PropertyName.DisplayName)
		{
			from2 = DisplayName;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.Kind)
		{
			value = VariantUtils.CreateFrom<StateMachineStateKind>(Kind);
			return true;
		}
		if (name == PropertyName.ParentId)
		{
			from = ParentId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InitialChildId)
		{
			from = InitialChildId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ProcessFlags)
		{
			value = VariantUtils.CreateFrom<StateMachineProcessFlags>(ProcessFlags);
			return true;
		}
		if (name == PropertyName.CallbackKey)
		{
			from2 = CallbackKey;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.EnterCallbackKey)
		{
			from2 = EnterCallbackKey;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ExitCallbackKey)
		{
			from2 = ExitCallbackKey;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ProcessCallbackKey)
		{
			from2 = ProcessCallbackKey;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PhysicsProcessCallbackKey)
		{
			from2 = PhysicsProcessCallbackKey;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.StableId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.DisplayName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Kind, PropertyHint.Enum, "Atomic,Compound,Parallel,History", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ParentId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.InitialChildId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ProcessFlags, PropertyHint.Flags, "None:0,Process:1,PhysicsProcess:2,Input:4,UnhandledInput:8", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.CallbackKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.EnterCallbackKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ExitCallbackKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ProcessCallbackKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.PhysicsProcessCallbackKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.StableId, Variant.From<string>(StableId));
		info.AddProperty(PropertyName.DisplayName, Variant.From<StringName>(DisplayName));
		info.AddProperty(PropertyName.Kind, Variant.From<StateMachineStateKind>(Kind));
		info.AddProperty(PropertyName.ParentId, Variant.From<string>(ParentId));
		info.AddProperty(PropertyName.InitialChildId, Variant.From<string>(InitialChildId));
		info.AddProperty(PropertyName.ProcessFlags, Variant.From<StateMachineProcessFlags>(ProcessFlags));
		info.AddProperty(PropertyName.CallbackKey, Variant.From<StringName>(CallbackKey));
		info.AddProperty(PropertyName.EnterCallbackKey, Variant.From<StringName>(EnterCallbackKey));
		info.AddProperty(PropertyName.ExitCallbackKey, Variant.From<StringName>(ExitCallbackKey));
		info.AddProperty(PropertyName.ProcessCallbackKey, Variant.From<StringName>(ProcessCallbackKey));
		info.AddProperty(PropertyName.PhysicsProcessCallbackKey, Variant.From<StringName>(PhysicsProcessCallbackKey));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.StableId, out var value))
		{
			StableId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.DisplayName, out var value2))
		{
			DisplayName = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Kind, out var value3))
		{
			Kind = value3.As<StateMachineStateKind>();
		}
		if (info.TryGetProperty(PropertyName.ParentId, out var value4))
		{
			ParentId = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.InitialChildId, out var value5))
		{
			InitialChildId = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ProcessFlags, out var value6))
		{
			ProcessFlags = value6.As<StateMachineProcessFlags>();
		}
		if (info.TryGetProperty(PropertyName.CallbackKey, out var value7))
		{
			CallbackKey = value7.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.EnterCallbackKey, out var value8))
		{
			EnterCallbackKey = value8.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.ExitCallbackKey, out var value9))
		{
			ExitCallbackKey = value9.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.ProcessCallbackKey, out var value10))
		{
			ProcessCallbackKey = value10.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.PhysicsProcessCallbackKey, out var value11))
		{
			PhysicsProcessCallbackKey = value11.As<StringName>();
		}
	}
}
