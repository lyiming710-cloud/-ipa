using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Icon("res://addons/godot_state_charts/not_guard.svg")]
[ScriptPath("res://addons/godot_state_charts/NotGuard.cs")]
public class NotGuard : Guard
{
	public new class MethodName : Guard.MethodName
	{
		public new static readonly StringName IsSatisfied = "IsSatisfied";

		public new static readonly StringName GetSupportedTriggerTypes = "GetSupportedTriggerTypes";
	}

	public new class PropertyName : Guard.PropertyName
	{
		public static readonly StringName guard = "guard";
	}

	public new class SignalName : Guard.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Guard guard { get; set; }

	public override bool IsSatisfied(Transition contextTransition, StateChartState contextState)
	{
		if (guard == null)
		{
			return true;
		}
		return !guard.IsSatisfied(contextTransition, contextState);
	}

	public override int GetSupportedTriggerTypes()
	{
		if (guard == null)
		{
			return 0;
		}
		return guard.GetSupportedTriggerTypes();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.IsSatisfied, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "contextTransition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "contextState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSupportedTriggerTypes, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSatisfied && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSatisfied(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSupportedTriggerTypes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSupportedTriggerTypes());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsSatisfied)
		{
			return true;
		}
		if (method == MethodName.GetSupportedTriggerTypes)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.guard)
		{
			guard = VariantUtils.ConvertTo<Guard>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.guard)
		{
			value = VariantUtils.CreateFrom<Guard>(guard);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.guard, PropertyHint.ResourceType, "Guard", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.guard, Variant.From<Guard>(guard));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.guard, out var value))
		{
			guard = value.As<Guard>();
		}
	}
}
