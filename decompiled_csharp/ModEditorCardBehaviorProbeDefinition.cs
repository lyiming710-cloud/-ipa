using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ModEditorCardBehaviorProbeDefinition : CardBehaviorDefinition
{
	private sealed class ProbeRuntime : CardBehaviorRuntime
	{
		protected override void OnBound()
		{
			BoundCalls++;
		}

		public override void OnPressed()
		{
			PressedCalls++;
		}

		public override void OnUseSucceeded(TowerDefenseCharacter createdCharacter)
		{
			UseSucceededCalls++;
		}
	}

	public new class MethodName : CardBehaviorDefinition.MethodName
	{
		public static readonly StringName ResetCounters = "ResetCounters";
	}

	public new class PropertyName : CardBehaviorDefinition.PropertyName
	{
		public static readonly StringName eventName = "eventName";
	}

	public new class SignalName : CardBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string eventName = "示例行为中文名";

	public static int CreateRuntimeCalls;

	public static int ModifyCostCalls;

	public static int BoundCalls;

	public static int PressedCalls;

	public static int UseSucceededCalls;

	public static void ResetCounters()
	{
		CreateRuntimeCalls = 0;
		ModifyCostCalls = 0;
		BoundCalls = 0;
		PressedCalls = 0;
		UseSucceededCalls = 0;
	}

	public override CardBehaviorRuntime CreateRuntime()
	{
		CreateRuntimeCalls++;
		return new ProbeRuntime();
	}

	public override void ModifyCost(ref CardCostContext context)
	{
		ModifyCostCalls++;
		context.Cost += 777;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.ResetCounters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResetCounters && args.Count == 0)
		{
			ResetCounters();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResetCounters && args.Count == 0)
		{
			ResetCounters();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ResetCounters)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.eventName)
		{
			eventName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.eventName)
		{
			value = VariantUtils.CreateFrom(in eventName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.eventName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventName, Variant.From(in eventName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventName, out var value))
		{
			eventName = value.As<string>();
		}
	}
}
