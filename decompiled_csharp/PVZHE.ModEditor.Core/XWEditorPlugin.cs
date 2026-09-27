using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWEditorPlugin.cs")]
public class XWEditorPlugin : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Initialize = "Initialize";

		public static readonly StringName _Handles = "_Handles";

		public static readonly StringName _MakeVisible = "_MakeVisible";

		public static readonly StringName _HasMainScreen = "_HasMainScreen";

		public static readonly StringName _GetPluginName = "_GetPluginName";

		public static readonly StringName _GetWindowLayout = "_GetWindowLayout";

		public static readonly StringName _SetWindowLayout = "_SetWindowLayout";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName AddControlToDock = "AddControlToDock";

		public static readonly StringName RemoveControlFromDock = "RemoveControlFromDock";

		public static readonly StringName AddMainScreenPlugin = "AddMainScreenPlugin";

		public static readonly StringName RemoveMainScreenPlugin = "RemoveMainScreenPlugin";

		public static readonly StringName AddInspectorPlugin = "AddInspectorPlugin";

		public static readonly StringName RemoveInspectorPlugin = "RemoveInspectorPlugin";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName EditorInterface = "EditorInterface";
	}

	public new class SignalName : Node.SignalName
	{
	}

	protected XWEditorInterface EditorInterface { get; private set; }

	public void Initialize(XWEditorInterface editorInterface)
	{
		EditorInterface = editorInterface;
		_Ready();
	}

	public virtual bool _Handles(GodotObject obj)
	{
		return false;
	}

	public virtual void _MakeVisible(bool visible)
	{
	}

	public virtual bool _HasMainScreen()
	{
		return false;
	}

	public virtual string _GetPluginName()
	{
		return "";
	}

	public virtual void _GetWindowLayout(Dictionary configuration)
	{
	}

	public virtual void _SetWindowLayout(Dictionary configuration)
	{
	}

	public new virtual void _EnterTree()
	{
	}

	public new virtual void _ExitTree()
	{
	}

	public void AddControlToDock(int dockSlot, Control control)
	{
		EditorInterface?.GetLayoutManager()?.AddControlToDock(dockSlot, control);
	}

	public void RemoveControlFromDock(Control control)
	{
		EditorInterface?.GetLayoutManager()?.RemoveControlFromDock(control);
	}

	public void AddMainScreenPlugin(XWEditorPlugin plugin)
	{
		EditorInterface?.GetLayoutManager()?.AddMainScreenPlugin(plugin);
	}

	public void RemoveMainScreenPlugin(XWEditorPlugin plugin)
	{
		EditorInterface?.GetLayoutManager()?.RemoveMainScreenPlugin(plugin);
	}

	public void AddInspectorPlugin(RefCounted plugin)
	{
		EditorInterface?.GetInspector()?.Call("add_inspector_plugin", plugin);
	}

	public void RemoveInspectorPlugin(RefCounted plugin)
	{
		EditorInterface?.GetInspector()?.Call("remove_inspector_plugin", plugin);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editorInterface", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName._Handles, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName._MakeVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HasMainScreen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetPluginName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetWindowLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "configuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._SetWindowLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "configuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddControlToDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "dockSlot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveControlFromDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddMainScreenPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveMainScreenPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddInspectorPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveInspectorPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Initialize && args.Count == 1)
		{
			Initialize(VariantUtils.ConvertTo<XWEditorInterface>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Handles && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_Handles(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName._MakeVisible && args.Count == 1)
		{
			_MakeVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._HasMainScreen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(_HasMainScreen());
			return true;
		}
		if (method == MethodName._GetPluginName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetPluginName());
			return true;
		}
		if (method == MethodName._GetWindowLayout && args.Count == 1)
		{
			_GetWindowLayout(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._SetWindowLayout && args.Count == 1)
		{
			_SetWindowLayout(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.AddControlToDock && args.Count == 2)
		{
			AddControlToDock(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveControlFromDock && args.Count == 1)
		{
			RemoveControlFromDock(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddMainScreenPlugin && args.Count == 1)
		{
			AddMainScreenPlugin(VariantUtils.ConvertTo<XWEditorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveMainScreenPlugin && args.Count == 1)
		{
			RemoveMainScreenPlugin(VariantUtils.ConvertTo<XWEditorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddInspectorPlugin && args.Count == 1)
		{
			AddInspectorPlugin(VariantUtils.ConvertTo<RefCounted>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveInspectorPlugin && args.Count == 1)
		{
			RemoveInspectorPlugin(VariantUtils.ConvertTo<RefCounted>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName._Handles)
		{
			return true;
		}
		if (method == MethodName._MakeVisible)
		{
			return true;
		}
		if (method == MethodName._HasMainScreen)
		{
			return true;
		}
		if (method == MethodName._GetPluginName)
		{
			return true;
		}
		if (method == MethodName._GetWindowLayout)
		{
			return true;
		}
		if (method == MethodName._SetWindowLayout)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.AddControlToDock)
		{
			return true;
		}
		if (method == MethodName.RemoveControlFromDock)
		{
			return true;
		}
		if (method == MethodName.AddMainScreenPlugin)
		{
			return true;
		}
		if (method == MethodName.RemoveMainScreenPlugin)
		{
			return true;
		}
		if (method == MethodName.AddInspectorPlugin)
		{
			return true;
		}
		if (method == MethodName.RemoveInspectorPlugin)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.EditorInterface)
		{
			EditorInterface = VariantUtils.ConvertTo<XWEditorInterface>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.EditorInterface)
		{
			value = VariantUtils.CreateFrom<XWEditorInterface>(EditorInterface);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.EditorInterface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.EditorInterface, Variant.From<XWEditorInterface>(EditorInterface));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.EditorInterface, out var value))
		{
			EditorInterface = value.As<XWEditorInterface>();
		}
	}
}
