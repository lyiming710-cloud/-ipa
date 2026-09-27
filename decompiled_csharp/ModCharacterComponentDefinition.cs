using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/Runtime/ModCharacterComponentDefinition.cs")]
public class ModCharacterComponentDefinition : CharacterComponentDefinition
{
	private sealed record ExportMember(string Name, Type ValueType, object Value, ExportAttribute Attribute, int Order);

	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName SynchronizeConfiguration = "SynchronizeConfiguration";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName AreConfigurationTypesCompatible = "AreConfigurationTypesCompatible";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName DefinitionTypeName = "DefinitionTypeName";

		public static readonly StringName RuntimeTypeName = "RuntimeTypeName";

		public static readonly StringName Configuration = "Configuration";

		public static readonly StringName ConfigurationSchema = "ConfigurationSchema";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	private const string ConfigurationPrefix = "Configuration/";

	private static readonly System.Reflection.MethodInfo VariantFromMethod = typeof(Variant).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((System.Reflection.MethodInfo method) => method.Name == "From" && method.IsGenericMethodDefinition && method.GetParameters().Length == 1);

	[ExportGroup("Mod Component", "")]
	[Export(PropertyHint.None, "")]
	public string DefinitionTypeName { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public string RuntimeTypeName { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Dictionary Configuration { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> ConfigurationSchema { get; set; } = new Array<Dictionary>();

	public override CharacterComponentRuntime CreateRuntime()
	{
		CharacterComponentRuntime characterComponentRuntime = CharacterComponentRuntimeTypeRegistry.Create(RuntimeTypeName);
		if (characterComponentRuntime == null)
		{
			GD.PushError("Mod character component runtime is not loaded: " + RuntimeTypeName);
		}
		return characterComponentRuntime;
	}

	public bool TryGetConfiguration(StringName name, out Variant value)
	{
		string text = name.ToString();
		if (Configuration != null && Configuration.TryGetValue(text, out value))
		{
			return true;
		}
		value = default;
		return false;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Player-authored component Export members are an explicit Mod schema surface.")]
	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Player-authored component Export members are an explicit Mod schema surface.")]
	public bool SynchronizeConfiguration(CharacterComponentDefinition schema)
	{
		if (schema == null)
		{
			return false;
		}
		Dictionary from = Configuration?.Duplicate(deep: true) ?? new Dictionary();
		Array<Dictionary> from2 = new Array<Dictionary>();
		foreach (ExportMember item in EnumerateExportMembers(schema))
		{
			if (!TryConvertToVariant(item.Value, item.ValueType, out var result))
			{
				GD.PushWarning($"Unsupported Mod component Export '{item.Name}' ({item.ValueType.FullName}).");
				continue;
			}
			Variant.Type type = ResolveVariantType(item.ValueType, result);
			if (!from.TryGetValue(item.Name, out var value) || !AreConfigurationTypesCompatible(value.VariantType, type))
			{
				from[item.Name] = result;
			}
			PropertyHint propertyHint = item.Attribute.Hint;
			string text = item.Attribute.HintString ?? "";
			if (item.ValueType.IsEnum && propertyHint == PropertyHint.None)
			{
				propertyHint = PropertyHint.Enum;
				text = string.Join(",", Enum.GetNames(item.ValueType));
			}
			from2.Add(new Dictionary
			{
				{ "key", item.Name },
				{
					"type",
					(long)type
				},
				{
					"hint",
					(long)propertyHint
				},
				{ "hint_string", text },
				{
					"class_name",
					ResolveClassName(item.ValueType, propertyHint, text)
				},
				{ "default", result }
			});
		}
		string text2 = schema.GetType().FullName ?? schema.GetType().Name;
		string text3 = RuntimeTypeName;
		CharacterComponentRuntime characterComponentRuntime = null;
		try
		{
			characterComponentRuntime = schema.CreateRuntime();
			if (characterComponentRuntime != null)
			{
				text3 = characterComponentRuntime.GetType().FullName ?? characterComponentRuntime.GetType().Name;
			}
		}
		finally
		{
			characterComponentRuntime?.Release();
		}
		if (string.Equals(DefinitionTypeName, text2, StringComparison.Ordinal) && string.Equals(RuntimeTypeName, text3, StringComparison.Ordinal))
		{
			Dictionary from3 = Configuration ?? new Dictionary();
			if (Variant.From(in from3).Equals(Variant.From(in from)))
			{
				Array<Dictionary> from4 = ConfigurationSchema ?? new Array<Dictionary>();
				if (Variant.From(in from4).Equals(Variant.From(in from2)))
				{
					return false;
				}
			}
		}
		DefinitionTypeName = text2;
		RuntimeTypeName = text3;
		Configuration = from;
		ConfigurationSchema = from2;
		NotifyPropertyListChanged();
		EmitChanged();
		return true;
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		Array<Dictionary> array = new Array<Dictionary>();
		if (ConfigurationSchema == null)
		{
			return array;
		}
		foreach (Dictionary item in ConfigurationSchema)
		{
			if (!item.TryGetValue("key", out var value))
			{
				continue;
			}
			string text = value.AsString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				Dictionary dictionary = new Dictionary
				{
					{
						"name",
						"Configuration/" + text
					},
					{
						"type",
						item.TryGetValue("type", out var value2) ? value2 : ((Variant)0L)
					},
					{
						"hint",
						item.TryGetValue("hint", out var value3) ? value3 : ((Variant)0L)
					},
					{
						"hint_string",
						item.TryGetValue("hint_string", out var value4) ? value4 : ((Variant)"")
					},
					{ "usage", 4L }
				};
				if (item.TryGetValue("class_name", out var value5) && !string.IsNullOrWhiteSpace(value5.AsString()))
				{
					dictionary["class_name"] = value5;
				}
				array.Add(dictionary);
			}
		}
		return array;
	}

	public override Variant _Get(StringName property)
	{
		if (!TryGetConfigurationKey(property, out var key) || Configuration == null || !Configuration.TryGetValue(key, out var value))
		{
			return default;
		}
		return value;
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (!TryGetConfigurationKey(property, out var key) || !TryGetSchema(key, out var result))
		{
			return false;
		}
		Variant.Type expected = (Variant.Type)(result.TryGetValue("type", out var value2) ? value2.AsInt64() : 0);
		if (!AreConfigurationTypesCompatible(value.VariantType, expected))
		{
			return false;
		}
		Dictionary dictionary = Configuration?.Duplicate(deep: true) ?? new Dictionary();
		if (dictionary.TryGetValue(key, out var value3) && value3.Equals(value))
		{
			return true;
		}
		dictionary[key] = value;
		Configuration = dictionary;
		EmitChanged();
		return true;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		Dictionary result;
		if (TryGetConfigurationKey(property, out var key))
		{
			return TryGetSchema(key, out result);
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (!TryGetConfigurationKey(property, out var key) || !TryGetSchema(key, out var result) || !result.TryGetValue("default", out var value))
		{
			return default;
		}
		return value;
	}

	private bool TryGetSchema(string key, out Dictionary result)
	{
		if (ConfigurationSchema != null)
		{
			foreach (Dictionary item in ConfigurationSchema)
			{
				if (item.TryGetValue("key", out var value) && string.Equals(value.AsString(), key, StringComparison.Ordinal))
				{
					result = item;
					return true;
				}
			}
		}
		result = null;
		return false;
	}

	private static bool TryGetConfigurationKey(StringName property, out string key)
	{
		string text = property.ToString();
		if (text.StartsWith("Configuration/", StringComparison.Ordinal) && text.Length > "Configuration/".Length)
		{
			string text2 = text;
			int length = "Configuration/".Length;
			key = text2.Substring(length, text2.Length - length);
			return true;
		}
		key = "";
		return false;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Player-authored Export members are an explicit Mod schema reflection surface.")]
	private static IEnumerable<ExportMember> EnumerateExportMembers(CharacterComponentDefinition schema)
	{
		Stack<Type> stack = new Stack<Type>();
		Type type = schema.GetType();
		while (type != null && type != typeof(CharacterComponentDefinition))
		{
			stack.Push(type);
			type = type.BaseType;
		}
		List<ExportMember> list = new List<ExportMember>();
		while (stack.Count > 0)
		{
			MemberInfo[] members = stack.Pop().GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MemberInfo memberInfo in members)
			{
				ExportAttribute customAttribute = memberInfo.GetCustomAttribute<ExportAttribute>(inherit: false);
				if (customAttribute == null)
				{
					continue;
				}
				MemberInfo memberInfo2 = memberInfo;
				if (!(memberInfo2 is FieldInfo fieldInfo))
				{
					if (memberInfo2 is System.Reflection.PropertyInfo propertyInfo && propertyInfo.GetMethod != null && propertyInfo.SetMethod != null && propertyInfo.GetIndexParameters().Length == 0 && !propertyInfo.GetMethod.IsStatic)
					{
						list.Add(new ExportMember(propertyInfo.Name, propertyInfo.PropertyType, propertyInfo.GetValue(schema), customAttribute, propertyInfo.MetadataToken));
					}
				}
				else if (!fieldInfo.IsStatic)
				{
					list.Add(new ExportMember(fieldInfo.Name, fieldInfo.FieldType, fieldInfo.GetValue(schema), customAttribute, fieldInfo.MetadataToken));
				}
			}
		}
		return from @group in list.GroupBy((ExportMember member) => member.Name, StringComparer.Ordinal)
			select @group.Last() into member
			orderby member.Order
			select member;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2060", Justification = "Export values are converted to host Variants without retaining their collectible member Types.")]
	private static bool TryConvertToVariant(object value, Type declaredType, out Variant result)
	{
		if (value is Variant variant)
		{
			result = variant;
			return true;
		}
		if ((object)declaredType != null && declaredType.IsEnum)
		{
			result = Variant.From<long>((value == null) ? 0 : Convert.ToInt64(value));
			return true;
		}
		if (value == null)
		{
			result = default;
			if (!(declaredType == null) && !typeof(GodotObject).IsAssignableFrom(declaredType))
			{
				return !declaredType.IsValueType;
			}
			return true;
		}
		try
		{
			if ((VariantFromMethod?.MakeGenericMethod(value.GetType()))?.Invoke(null, new object[1] { value }) is Variant variant2)
			{
				result = variant2;
				return true;
			}
		}
		catch
		{
		}
		result = default;
		return false;
	}

	private static Variant.Type ResolveVariantType(Type declaredType, Variant value)
	{
		if (value.VariantType != Variant.Type.Nil)
		{
			return value.VariantType;
		}
		if (declaredType == null)
		{
			return Variant.Type.Nil;
		}
		if (typeof(GodotObject).IsAssignableFrom(declaredType))
		{
			return Variant.Type.Object;
		}
		if (declaredType == typeof(string))
		{
			return Variant.Type.String;
		}
		if (declaredType == typeof(StringName))
		{
			return Variant.Type.StringName;
		}
		if (declaredType == typeof(NodePath))
		{
			return Variant.Type.NodePath;
		}
		if (declaredType.IsEnum || declaredType == typeof(bool) || declaredType == typeof(byte) || declaredType == typeof(sbyte) || declaredType == typeof(short) || declaredType == typeof(ushort) || declaredType == typeof(int) || declaredType == typeof(uint) || declaredType == typeof(long) || declaredType == typeof(ulong))
		{
			if (!(declaredType == typeof(bool)))
			{
				return Variant.Type.Int;
			}
			return Variant.Type.Bool;
		}
		if (declaredType == typeof(float) || declaredType == typeof(double))
		{
			return Variant.Type.Float;
		}
		string? fullName = declaredType.FullName;
		if (fullName != null && fullName.StartsWith("Godot.Collections.Array", StringComparison.Ordinal))
		{
			return Variant.Type.Array;
		}
		string? fullName2 = declaredType.FullName;
		if (fullName2 != null && fullName2.StartsWith("Godot.Collections.Dictionary", StringComparison.Ordinal))
		{
			return Variant.Type.Dictionary;
		}
		return Variant.Type.Nil;
	}

	private static string ResolveClassName(Type type, PropertyHint hint, string hintString)
	{
		if (type != null && typeof(GodotObject).IsAssignableFrom(type))
		{
			return type.Name;
		}
		if (hint != PropertyHint.ResourceType)
		{
			return "";
		}
		return hintString ?? "";
	}

	private static bool AreConfigurationTypesCompatible(Variant.Type actual, Variant.Type expected)
	{
		if (expected != Variant.Type.Nil && actual != expected)
		{
			if (actual == Variant.Type.Nil)
			{
				return expected == Variant.Type.Object;
			}
			return false;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(7)
		{
			new Godot.Bridge.MethodInfo(MethodName.SynchronizeConfiguration, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "schema", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName._GetPropertyList, new Godot.Bridge.PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName._Get, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName._Set, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName._PropertyCanRevert, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName._PropertyGetRevert, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.AreConfigurationTypesCompatible, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "actual", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SynchronizeConfiguration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SynchronizeConfiguration(VariantUtils.ConvertTo<CharacterComponentDefinition>(in args[0])));
			return true;
		}
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.AreConfigurationTypesCompatible && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreConfigurationTypesCompatible(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AreConfigurationTypesCompatible && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreConfigurationTypesCompatible(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<Variant.Type>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SynchronizeConfiguration)
		{
			return true;
		}
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
		{
			return true;
		}
		if (method == MethodName.AreConfigurationTypesCompatible)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.DefinitionTypeName)
		{
			DefinitionTypeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeTypeName)
		{
			RuntimeTypeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.Configuration)
		{
			Configuration = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.ConfigurationSchema)
		{
			ConfigurationSchema = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.DefinitionTypeName)
		{
			from = DefinitionTypeName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RuntimeTypeName)
		{
			from = RuntimeTypeName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Configuration)
		{
			value = VariantUtils.CreateFrom<Dictionary>(Configuration);
			return true;
		}
		if (name == PropertyName.ConfigurationSchema)
		{
			value = VariantUtils.CreateFromArray(ConfigurationSchema);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "Mod Component", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.DefinitionTypeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName.RuntimeTypeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.Dictionary, PropertyName.Configuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName.ConfigurationSchema, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.DefinitionTypeName, Variant.From<string>(DefinitionTypeName));
		info.AddProperty(PropertyName.RuntimeTypeName, Variant.From<string>(RuntimeTypeName));
		info.AddProperty(PropertyName.Configuration, Variant.From<Dictionary>(Configuration));
		info.AddProperty(PropertyName.ConfigurationSchema, Variant.CreateFrom(ConfigurationSchema));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.DefinitionTypeName, out var value))
		{
			DefinitionTypeName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeTypeName, out var value2))
		{
			RuntimeTypeName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.Configuration, out var value3))
		{
			Configuration = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.ConfigurationSchema, out var value4))
		{
			ConfigurationSchema = value4.AsGodotArray<Dictionary>();
		}
	}
}
