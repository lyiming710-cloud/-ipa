using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.Blueprint;

[GlobalClass]
[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Node/XWBPNodePortData.cs")]
public class XWBPNodePortData : RefCounted
{
	[Signal]
	public delegate void NameChangeEventHandler();

	public enum Direction
	{
		Input,
		Output
	}

	public enum PortType
	{
		Unknown = 1000000,
		Flow = 100,
		Any = 0,
		Bool = 1,
		Integer = 2,
		Float = 3,
		String = 4,
		Vector2 = 5,
		Vector2I = 6,
		Rect2 = 7,
		Rect2I = 8,
		Vector3 = 9,
		Vector3I = 10,
		Transform2D = 11,
		Vector4 = 12,
		Vector4I = 13,
		Color = 14,
		StringName = 15,
		NodePath = 16,
		Array = 22,
		Dictionary = 21,
		Object = 18
	}

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName SetPortType = "SetPortType";

		public static readonly StringName IsObjectPortType = "IsObjectPortType";

		public static readonly StringName CanConnectTo = "CanConnectTo";

		public static readonly StringName GetTypeIcon = "GetTypeIcon";

		public static readonly StringName LoadIcon = "LoadIcon";

		public static readonly StringName GetTypeColor = "GetTypeColor";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Name = "Name";

		public static readonly StringName PortDirection = "PortDirection";

		public static readonly StringName PortTypeValue = "PortTypeValue";

		public static readonly StringName ClassName = "ClassName";

		public static readonly StringName DefaultValue = "DefaultValue";

		public static readonly StringName Value = "Value";

		public static readonly StringName _name = "_name";

		public static readonly StringName _portType = "_portType";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName NameChange = "NameChange";
	}

	private const string FlowPortIconPath = "res://addons/ModEditor/Icons/FlowPort.svg";

	private static Texture2D _flowPortIcon;

	private string _name = "新端口";

	private PortType _portType;

	private NameChangeEventHandler backing_NameChange;

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
			EmitSignal(SignalName.NameChange);
		}
	}

	public Direction PortDirection { get; set; }

	public PortType PortTypeValue
	{
		get
		{
			return _portType;
		}
		set
		{
			SetPortType(value);
		}
	}

	public StringName ClassName { get; set; } = "";

	public Variant DefaultValue { get; set; }

	[Export(PropertyHint.None, "")]
	public Variant Value { get; set; }

	public event NameChangeEventHandler NameChange
	{
		add
		{
			backing_NameChange = (NameChangeEventHandler)Delegate.Combine(backing_NameChange, value);
		}
		remove
		{
			backing_NameChange = (NameChangeEventHandler)Delegate.Remove(backing_NameChange, value);
		}
	}

	public XWBPNodePortData()
	{
	}

	public XWBPNodePortData(string name, Direction direction, PortType portType, StringName className, Variant defaultValue)
	{
		_name = name;
		PortDirection = direction;
		_portType = portType;
		ClassName = className;
		DefaultValue = defaultValue;
		Value = defaultValue;
	}

	public void SetPortType(PortType newPortType)
	{
		if (_portType == newPortType)
		{
			return;
		}
		_portType = newPortType;
		if (newPortType < PortType.Flow)
		{
			Variant.Type type = (Variant.Type)newPortType;
			if (XWTypeRegistry.Instance.HasType(type))
			{
				DefaultValue = XWTypeRegistry.Instance.GetTypeDefaultValue(type);
			}
			else
			{
				DefaultValue = default;
			}
		}
		else
		{
			DefaultValue = default;
		}
		Value = DefaultValue;
	}

	public static bool IsObjectPortType(PortType portType)
	{
		if (portType != PortType.Object)
		{
			return portType == (PortType)24;
		}
		return true;
	}

	public bool CanConnectTo(XWBPNodePortData other)
	{
		if (other == null)
		{
			return false;
		}
		if (_portType == PortType.Any || other._portType == PortType.Any)
		{
			return true;
		}
		if (_portType == other._portType)
		{
			if (IsObjectPortType(_portType))
			{
				if (ClassName == other.ClassName)
				{
					return true;
				}
				return XWClassRegistry.Instance.IsParentClass(ClassName.ToString(), other.ClassName.ToString());
			}
			return true;
		}
		if (IsObjectPortType(_portType) && IsObjectPortType(other._portType))
		{
			if (ClassName == other.ClassName)
			{
				return true;
			}
			return XWClassRegistry.Instance.IsClassInstanceOf(ClassName.ToString(), other.ClassName.ToString());
		}
		if (_portType == PortType.Integer && other._portType == PortType.Float)
		{
			return true;
		}
		if (_portType == PortType.Float && other._portType == PortType.Integer)
		{
			return true;
		}
		if (_portType == PortType.Integer && other._portType == PortType.Bool)
		{
			return true;
		}
		if (_portType == PortType.Bool && other._portType == PortType.Integer)
		{
			return true;
		}
		return false;
	}

	public static Texture2D GetTypeIcon(PortType portType, StringName className = null)
	{
		if (portType == PortType.Flow)
		{
			return _flowPortIcon ?? (_flowPortIcon = LoadIcon("res://addons/ModEditor/Icons/FlowPort.svg"));
		}
		if (!IsObjectPortType(portType))
		{
			Variant.Type type = (Variant.Type)portType;
			if (XWTypeRegistry.Instance.HasType(type))
			{
				return XWTypeRegistry.Instance.GetTypeIcon(type);
			}
		}
		else
		{
			string text = className.ToString();
			if (!string.IsNullOrEmpty(text) && XWClassRegistry.Instance.HasClass(text))
			{
				return XWClassRegistry.Instance.GetClassIcon(text);
			}
		}
		return null;
	}

	private static Texture2D LoadIcon(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	public static Color GetTypeColor(PortType portType, StringName className = null)
	{
		if (portType == PortType.Flow)
		{
			return Colors.White;
		}
		if (!IsObjectPortType(portType))
		{
			Variant.Type type = (Variant.Type)portType;
			if (XWTypeRegistry.Instance.HasType(type))
			{
				return XWTypeRegistry.Instance.GetTypeColor(type);
			}
		}
		else
		{
			string text = className.ToString();
			if (!string.IsNullOrEmpty(text) && XWClassRegistry.Instance.HasClass(text))
			{
				return XWClassRegistry.Instance.GetClassColor(text);
			}
		}
		return Colors.Gray;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.SetPortType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "newPortType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsObjectPortType, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "portType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanConnectTo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "other", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "portType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "portType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetPortType && args.Count == 1)
		{
			SetPortType(VariantUtils.ConvertTo<PortType>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsObjectPortType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsObjectPortType(VariantUtils.ConvertTo<PortType>(in args[0])));
			return true;
		}
		if (method == MethodName.CanConnectTo && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanConnectTo(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeIcon && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetTypeIcon(VariantUtils.ConvertTo<PortType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeColor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Color>(GetTypeColor(VariantUtils.ConvertTo<PortType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsObjectPortType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsObjectPortType(VariantUtils.ConvertTo<PortType>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeIcon && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetTypeIcon(VariantUtils.ConvertTo<PortType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTypeColor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Color>(GetTypeColor(VariantUtils.ConvertTo<PortType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetPortType)
		{
			return true;
		}
		if (method == MethodName.IsObjectPortType)
		{
			return true;
		}
		if (method == MethodName.CanConnectTo)
		{
			return true;
		}
		if (method == MethodName.GetTypeIcon)
		{
			return true;
		}
		if (method == MethodName.LoadIcon)
		{
			return true;
		}
		if (method == MethodName.GetTypeColor)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.PortDirection)
		{
			PortDirection = VariantUtils.ConvertTo<Direction>(in value);
			return true;
		}
		if (name == PropertyName.PortTypeValue)
		{
			PortTypeValue = VariantUtils.ConvertTo<PortType>(in value);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			ClassName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.DefaultValue)
		{
			DefaultValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.Value)
		{
			Value = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName._name)
		{
			_name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._portType)
		{
			_portType = VariantUtils.ConvertTo<PortType>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom<string>(Name);
			return true;
		}
		if (name == PropertyName.PortDirection)
		{
			value = VariantUtils.CreateFrom<Direction>(PortDirection);
			return true;
		}
		if (name == PropertyName.PortTypeValue)
		{
			value = VariantUtils.CreateFrom<PortType>(PortTypeValue);
			return true;
		}
		if (name == PropertyName.ClassName)
		{
			value = VariantUtils.CreateFrom<StringName>(ClassName);
			return true;
		}
		Variant from;
		if (name == PropertyName.DefaultValue)
		{
			from = DefaultValue;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Value)
		{
			from = Value;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._name)
		{
			value = VariantUtils.CreateFrom(in _name);
			return true;
		}
		if (name == PropertyName._portType)
		{
			value = VariantUtils.CreateFrom(in _portType);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._portType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PortDirection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PortTypeValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.ClassName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.DefaultValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.Value, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Name, Variant.From<string>(Name));
		info.AddProperty(PropertyName.PortDirection, Variant.From<Direction>(PortDirection));
		info.AddProperty(PropertyName.PortTypeValue, Variant.From<PortType>(PortTypeValue));
		info.AddProperty(PropertyName.ClassName, Variant.From<StringName>(ClassName));
		info.AddProperty(PropertyName.DefaultValue, Variant.From<Variant>(DefaultValue));
		info.AddProperty(PropertyName.Value, Variant.From<Variant>(Value));
		info.AddProperty(PropertyName._name, Variant.From(in _name));
		info.AddProperty(PropertyName._portType, Variant.From(in _portType));
		info.AddSignalEventDelegate(SignalName.NameChange, backing_NameChange);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Name, out var value))
		{
			Name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.PortDirection, out var value2))
		{
			PortDirection = value2.As<Direction>();
		}
		if (info.TryGetProperty(PropertyName.PortTypeValue, out var value3))
		{
			PortTypeValue = value3.As<PortType>();
		}
		if (info.TryGetProperty(PropertyName.ClassName, out var value4))
		{
			ClassName = value4.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.DefaultValue, out var value5))
		{
			DefaultValue = value5.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.Value, out var value6))
		{
			Value = value6.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName._name, out var value7))
		{
			_name = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._portType, out var value8))
		{
			_portType = value8.As<PortType>();
		}
		if (info.TryGetSignalEventDelegate<NameChangeEventHandler>(SignalName.NameChange, out var value9))
		{
			backing_NameChange = value9;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.NameChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalNameChange()
	{
		EmitSignal(SignalName.NameChange, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.NameChange && args.Count == 0)
		{
			backing_NameChange?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.NameChange)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
