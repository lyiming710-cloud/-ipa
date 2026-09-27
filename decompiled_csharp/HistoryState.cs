using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[Icon("res://addons/godot_state_charts/history_state.svg")]
[ScriptPath("res://addons/godot_state_charts/HistoryState.cs")]
public class HistoryState : StateChartState
{
	public new class MethodName : StateChartState.MethodName
	{
		public new static readonly StringName _StateSave = "_StateSave";

		public new static readonly StringName _StateRestore = "_StateRestore";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";
	}

	public new class PropertyName : StateChartState.PropertyName
	{
		public static readonly StringName defaultState = "defaultState";

		public static readonly StringName deep = "deep";

		public static readonly StringName _defaultStatePath = "_defaultStatePath";

		public static readonly StringName history = "history";
	}

	public new class SignalName : StateChartState.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool deep;

	private NodePath _defaultStatePath = new NodePath();

	public SavedState history;

	[Export(PropertyHint.None, "")]
	public NodePath defaultState
	{
		get
		{
			return _defaultStatePath;
		}
		set
		{
			_defaultStatePath = value;
			UpdateConfigurationWarnings();
		}
	}

	public override void _StateSave(SavedState savedState, int childLevels = -1)
	{
		SavedState savedState2 = new SavedState();
		savedState2.history = history;
		savedState.AddSubstate(this, savedState2);
	}

	public override void _StateRestore(SavedState savedState, int childLevels = -1)
	{
		SavedState substateOrNull = savedState.GetSubstateOrNull(this);
		if (substateOrNull != null)
		{
			history = substateOrNull.history;
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>(base._GetConfigurationWarnings());
		if (!(GetParent() is CompoundState))
		{
			list.Add("A history state must be a child of a compound state.");
		}
		StateChartState nodeOrNull = GetNodeOrNull<StateChartState>(_defaultStatePath);
		if (nodeOrNull == null)
		{
			list.Add("The default state is not set or is not a state.");
		}
		else if (!GetParent().IsAncestorOf(nodeOrNull))
		{
			list.Add("The default state must be a child of the parent state.");
		}
		if (GetChildCount() > 0)
		{
			list.Add("History states cannot have child nodes.");
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._StateSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childLevels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._StateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childLevels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._StateSave && args.Count == 2)
		{
			_StateSave(VariantUtils.ConvertTo<SavedState>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._StateRestore && args.Count == 2)
		{
			_StateRestore(VariantUtils.ConvertTo<SavedState>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(_GetConfigurationWarnings());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._StateSave)
		{
			return true;
		}
		if (method == MethodName._StateRestore)
		{
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.defaultState)
		{
			defaultState = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.deep)
		{
			deep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._defaultStatePath)
		{
			_defaultStatePath = VariantUtils.ConvertTo<NodePath>(in value);
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
		if (name == PropertyName.defaultState)
		{
			value = VariantUtils.CreateFrom<NodePath>(defaultState);
			return true;
		}
		if (name == PropertyName.deep)
		{
			value = VariantUtils.CreateFrom(in deep);
			return true;
		}
		if (name == PropertyName._defaultStatePath)
		{
			value = VariantUtils.CreateFrom(in _defaultStatePath);
			return true;
		}
		if (name == PropertyName.history)
		{
			value = VariantUtils.CreateFrom(in history);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.deep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.defaultState, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName._defaultStatePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.defaultState, Variant.From<NodePath>(defaultState));
		info.AddProperty(PropertyName.deep, Variant.From(in deep));
		info.AddProperty(PropertyName._defaultStatePath, Variant.From(in _defaultStatePath));
		info.AddProperty(PropertyName.history, Variant.From(in history));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.defaultState, out var value))
		{
			defaultState = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.deep, out var value2))
		{
			deep = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._defaultStatePath, out var value3))
		{
			_defaultStatePath = value3.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.history, out var value4))
		{
			history = value4.As<SavedState>();
		}
	}
}
