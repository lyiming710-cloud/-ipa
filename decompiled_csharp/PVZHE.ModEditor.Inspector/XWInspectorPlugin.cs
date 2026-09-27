using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/RefCounted/XWInspectorPlugin.cs")]
public class XWInspectorPlugin : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName _CanHandle = "_CanHandle";

		public static readonly StringName _ParseBegin = "_ParseBegin";

		public static readonly StringName _ParseCategory = "_ParseCategory";

		public static readonly StringName _ParseGroup = "_ParseGroup";

		public static readonly StringName _ParseProperty = "_ParseProperty";

		public static readonly StringName _ParseEnd = "_ParseEnd";

		public static readonly StringName CanHandle = "CanHandle";

		public static readonly StringName ParseBegin = "ParseBegin";

		public static readonly StringName ParseCategory = "ParseCategory";

		public static readonly StringName ParseGroup = "ParseGroup";

		public static readonly StringName ParseProperty = "ParseProperty";

		public static readonly StringName ParseEnd = "ParseEnd";

		public static readonly StringName AddCustomControl = "AddCustomControl";

		public static readonly StringName SetInspector = "SetInspector";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _inspector = "_inspector";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	protected XWInspector _inspector;

	public virtual bool _CanHandle(GodotObject obj)
	{
		return false;
	}

	public virtual Control _ParseBegin(GodotObject obj)
	{
		return null;
	}

	public virtual void _ParseCategory(GodotObject obj, string category)
	{
	}

	public virtual void _ParseGroup(GodotObject obj, string group)
	{
	}

	public virtual bool _ParseProperty(GodotObject obj, Variant.Type type, string name, PropertyHint hint, string hintText, long usage, bool wide)
	{
		return false;
	}

	public virtual Control _ParseEnd(GodotObject obj)
	{
		return null;
	}

	public bool CanHandle(GodotObject obj)
	{
		return _CanHandle(obj);
	}

	public Control ParseBegin(GodotObject obj)
	{
		return _ParseBegin(obj);
	}

	public void ParseCategory(GodotObject obj, string category)
	{
		_ParseCategory(obj, category);
	}

	public void ParseGroup(GodotObject obj, string group)
	{
		_ParseGroup(obj, group);
	}

	public bool ParseProperty(GodotObject obj, Variant.Type type, string name, PropertyHint hint, string hintText, long usage, bool wide)
	{
		return _ParseProperty(obj, type, name, hint, hintText, usage, wide);
	}

	public Control ParseEnd(GodotObject obj)
	{
		return _ParseEnd(obj);
	}

	public void AddCustomControl(Control control)
	{
		if (GodotObject.IsInstanceValid(_inspector))
		{
			_inspector.PropertyContainer.AddChild(control, forceReadableName: false, Node.InternalMode.Disabled);
		}
	}

	public void SetInspector(XWInspector inspector)
	{
		_inspector = inspector;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._CanHandle, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName._ParseBegin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName._ParseCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ParseGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "group", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ParseProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "usage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "wide", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ParseEnd, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanHandle, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.ParseBegin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.ParseCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ParseGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "group", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ParseProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hintText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "usage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "wide", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ParseEnd, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddCustomControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetInspector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inspector", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._CanHandle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanHandle(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName._ParseBegin && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(_ParseBegin(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName._ParseCategory && args.Count == 2)
		{
			_ParseCategory(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._ParseGroup && args.Count == 2)
		{
			_ParseGroup(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._ParseProperty && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<bool>(_ParseProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<PropertyHint>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<long>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName._ParseEnd && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(_ParseEnd(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.CanHandle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanHandle(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.ParseBegin && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(ParseBegin(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.ParseCategory && args.Count == 2)
		{
			ParseCategory(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ParseGroup && args.Count == 2)
		{
			ParseGroup(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ParseProperty && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<bool>(ParseProperty(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<PropertyHint>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<long>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6])));
			return true;
		}
		if (method == MethodName.ParseEnd && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(ParseEnd(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.AddCustomControl && args.Count == 1)
		{
			AddCustomControl(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetInspector && args.Count == 1)
		{
			SetInspector(VariantUtils.ConvertTo<XWInspector>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._CanHandle)
		{
			return true;
		}
		if (method == MethodName._ParseBegin)
		{
			return true;
		}
		if (method == MethodName._ParseCategory)
		{
			return true;
		}
		if (method == MethodName._ParseGroup)
		{
			return true;
		}
		if (method == MethodName._ParseProperty)
		{
			return true;
		}
		if (method == MethodName._ParseEnd)
		{
			return true;
		}
		if (method == MethodName.CanHandle)
		{
			return true;
		}
		if (method == MethodName.ParseBegin)
		{
			return true;
		}
		if (method == MethodName.ParseCategory)
		{
			return true;
		}
		if (method == MethodName.ParseGroup)
		{
			return true;
		}
		if (method == MethodName.ParseProperty)
		{
			return true;
		}
		if (method == MethodName.ParseEnd)
		{
			return true;
		}
		if (method == MethodName.AddCustomControl)
		{
			return true;
		}
		if (method == MethodName.SetInspector)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._inspector)
		{
			_inspector = VariantUtils.ConvertTo<XWInspector>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._inspector)
		{
			value = VariantUtils.CreateFrom(in _inspector);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._inspector, Variant.From(in _inspector));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._inspector, out var value))
		{
			_inspector = value.As<XWInspector>();
		}
	}
}
