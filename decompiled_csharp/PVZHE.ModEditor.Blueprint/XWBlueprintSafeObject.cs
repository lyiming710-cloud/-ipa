using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

public sealed class XWBlueprintSafeObject : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName MatchesClass = "MatchesClass";

		public static readonly StringName TrySetProperty = "TrySetProperty";

		public static readonly StringName IsMethodAllowed = "IsMethodAllowed";

		public static readonly StringName IsSafeIdentifier = "IsSafeIdentifier";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName ClassName = "ClassName";

		public static readonly StringName SourcePath = "SourcePath";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private readonly Dictionary<string, Variant> _properties;

	private readonly HashSet<string> _writableProperties;

	private readonly HashSet<string> _allowedMethods;

	public string ClassName { get; }

	public string SourcePath { get; }

	internal XWBlueprintSafeObject(string className, string sourcePath, Dictionary<string, Variant> properties, IEnumerable<string> writableProperties, IEnumerable<string> allowedMethods)
	{
		ClassName = (string.IsNullOrWhiteSpace(className) ? "Object" : className.Trim());
		SourcePath = sourcePath ?? string.Empty;
		_properties = properties ?? new Dictionary<string, Variant>(StringComparer.Ordinal);
		_writableProperties = new HashSet<string>(writableProperties ?? Array.Empty<string>(), StringComparer.Ordinal);
		_allowedMethods = new HashSet<string>(allowedMethods ?? Array.Empty<string>(), StringComparer.Ordinal);
	}

	internal bool MatchesClass(string expectedClass)
	{
		bool flag = string.IsNullOrWhiteSpace(expectedClass);
		if (!flag)
		{
			bool flag2 = ((expectedClass == "Object" || expectedClass == "GodotObject") ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			return true;
		}
		if (expectedClass == "Resource")
		{
			return !string.IsNullOrWhiteSpace(SourcePath);
		}
		return string.Equals(ClassName, expectedClass.Trim(), StringComparison.Ordinal);
	}

	internal bool TryGetProperty(string name, out Variant value)
	{
		value = default;
		if (IsSafeIdentifier(name))
		{
			return _properties.TryGetValue(name, out value);
		}
		return false;
	}

	internal bool TrySetProperty(string name, Variant value)
	{
		if (!IsSafeIdentifier(name) || !_writableProperties.Contains(name))
		{
			return false;
		}
		_properties[name] = value;
		return true;
	}

	internal bool IsMethodAllowed(string methodName)
	{
		if (IsSafeIdentifier(methodName))
		{
			return _allowedMethods.Contains(methodName);
		}
		return false;
	}

	internal bool TryInvoke(string methodName, IReadOnlyList<Variant> arguments, out Variant result)
	{
		result = default;
		if (!IsMethodAllowed(methodName))
		{
			return false;
		}
		switch (methodName)
		{
		case "has_property":
		{
			bool flag = arguments.Count == 1;
			if (flag)
			{
				Variant.Type variantType = arguments[0].VariantType;
				bool flag2 = ((variantType == Variant.Type.String || variantType == Variant.Type.StringName) ? true : false);
				flag = flag2;
			}
			if (flag)
			{
				result = Variant.From<bool>(_properties.ContainsKey(arguments[0].AsString()));
				return true;
			}
			break;
		}
		case "get_property_count":
			if (arguments.Count == 0)
			{
				result = Variant.From<long>((long)_properties.Count);
				return true;
			}
			break;
		case "get_type":
			if (arguments.Count == 0)
			{
				result = Variant.From<string>(ClassName);
				return true;
			}
			break;
		case "get_resource_name":
		{
			if (arguments.Count == 0 && _properties.TryGetValue("resource_name", out var value))
			{
				result = value;
				return true;
			}
			break;
		}
		case "get_resource_path":
			if (arguments.Count == 0)
			{
				result = Variant.From<string>(SourcePath);
				return true;
			}
			break;
		}
		return false;
	}

	internal static bool IsSafeIdentifier(string value)
	{
		if (string.IsNullOrWhiteSpace(value) || value.Length > 128)
		{
			return false;
		}
		if (!char.IsLetter(value[0]) && value[0] != '_')
		{
			return false;
		}
		for (int i = 1; i < value.Length; i++)
		{
			char c = value[i];
			if (!char.IsLetterOrDigit(c) && c != '_')
			{
				return false;
			}
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.MatchesClass, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "expectedClass", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrySetProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMethodAllowed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "methodName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSafeIdentifier, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.MatchesClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TrySetProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySetProperty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.IsMethodAllowed && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMethodAllowed(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSafeIdentifier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSafeIdentifier(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSafeIdentifier && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSafeIdentifier(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.MatchesClass)
		{
			return true;
		}
		if (method == MethodName.TrySetProperty)
		{
			return true;
		}
		if (method == MethodName.IsMethodAllowed)
		{
			return true;
		}
		if (method == MethodName.IsSafeIdentifier)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ClassName)
		{
			from = ClassName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SourcePath)
		{
			from = SourcePath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ClassName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.SourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
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
