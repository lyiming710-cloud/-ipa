using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Icon("res://addons/godot_state_charts/guard.svg")]
[ScriptPath("res://addons/godot_state_charts/Guard.cs")]
public class Guard : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName IsSatisfied = "IsSatisfied";

		public static readonly StringName GetSupportedTriggerTypes = "GetSupportedTriggerTypes";
	}

	public new class PropertyName : Resource.PropertyName
	{
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public virtual bool IsSatisfied(Transition contextTransition, StateChartState contextState)
	{
		GD.PushError("Guard.IsSatisfied() is not implemented. Did you forget to override it?");
		return false;
	}

	public virtual int GetSupportedTriggerTypes()
	{
		GD.PushError("Guard.GetSupportedTriggerTypes() is not implemented. Did you forget to override it?");
		return 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
