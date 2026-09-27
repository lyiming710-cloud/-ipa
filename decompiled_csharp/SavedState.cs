using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://addons/godot_state_charts/SavedState.cs")]
public class SavedState : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName AddSubstate = "AddSubstate";

		public static readonly StringName GetSubstateOrNull = "GetSubstateOrNull";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName childStates = "childStates";

		public static readonly StringName pendingTransitionName = "pendingTransitionName";

		public static readonly StringName pendingTransitionRemainingDelay = "pendingTransitionRemainingDelay";

		public static readonly StringName pendingTransitionInitialDelay = "pendingTransitionInitialDelay";

		public static readonly StringName history = "history";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Dictionary childStates { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public NodePath pendingTransitionName { get; set; }

	[Export(PropertyHint.None, "")]
	public float pendingTransitionRemainingDelay { get; set; }

	[Export(PropertyHint.None, "")]
	public float pendingTransitionInitialDelay { get; set; }

	[Export(PropertyHint.None, "")]
	public SavedState history { get; set; }

	public void AddSubstate(StateChartState state, SavedState savedState)
	{
		childStates[state.Name] = savedState;
	}

	public SavedState GetSubstateOrNull(StateChartState state)
	{
		if (childStates.ContainsKey(state.Name))
		{
			return childStates[state.Name].As<SavedState>();
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.AddSubstate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "state", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSubstateOrNull, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "state", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddSubstate && args.Count == 2)
		{
			AddSubstate(VariantUtils.ConvertTo<StateChartState>(in args[0]), VariantUtils.ConvertTo<SavedState>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSubstateOrNull && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<SavedState>(GetSubstateOrNull(VariantUtils.ConvertTo<StateChartState>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddSubstate)
		{
			return true;
		}
		if (method == MethodName.GetSubstateOrNull)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.childStates)
		{
			childStates = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.pendingTransitionName)
		{
			pendingTransitionName = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.pendingTransitionRemainingDelay)
		{
			pendingTransitionRemainingDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.pendingTransitionInitialDelay)
		{
			pendingTransitionInitialDelay = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.history)
		{
			history = VariantUtils.ConvertTo<SavedState>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.childStates)
		{
			value = VariantUtils.CreateFrom<Dictionary>(childStates);
			return true;
		}
		if (name == PropertyName.pendingTransitionName)
		{
			value = VariantUtils.CreateFrom<NodePath>(pendingTransitionName);
			return true;
		}
		float from;
		if (name == PropertyName.pendingTransitionRemainingDelay)
		{
			from = pendingTransitionRemainingDelay;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.pendingTransitionInitialDelay)
		{
			from = pendingTransitionInitialDelay;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.history)
		{
			value = VariantUtils.CreateFrom<SavedState>(history);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.childStates, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.pendingTransitionName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pendingTransitionRemainingDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pendingTransitionInitialDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.history, PropertyHint.ResourceType, "SavedState", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.childStates, Variant.From<Dictionary>(childStates));
		info.AddProperty(PropertyName.pendingTransitionName, Variant.From<NodePath>(pendingTransitionName));
		info.AddProperty(PropertyName.pendingTransitionRemainingDelay, Variant.From<float>(pendingTransitionRemainingDelay));
		info.AddProperty(PropertyName.pendingTransitionInitialDelay, Variant.From<float>(pendingTransitionInitialDelay));
		info.AddProperty(PropertyName.history, Variant.From<SavedState>(history));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.childStates, out var value))
		{
			childStates = value.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.pendingTransitionName, out var value2))
		{
			pendingTransitionName = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.pendingTransitionRemainingDelay, out var value3))
		{
			pendingTransitionRemainingDelay = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.pendingTransitionInitialDelay, out var value4))
		{
			pendingTransitionInitialDelay = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.history, out var value5))
		{
			history = value5.As<SavedState>();
		}
	}
}
