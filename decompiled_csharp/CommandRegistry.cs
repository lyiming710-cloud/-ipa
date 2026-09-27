using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Command/CommandRegistry.cs")]
public class CommandRegistry : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName RegisterInit = "RegisterInit";

		public static readonly StringName RegisterCommand = "RegisterCommand";

		public static readonly StringName UnregisterCommand = "UnregisterCommand";

		public static readonly StringName HasCommand = "HasCommand";

		public static readonly StringName GetCommand = "GetCommand";

		public static readonly StringName GetCommandNames = "GetCommandNames";

		public static readonly StringName ExecuteCommand = "ExecuteCommand";

		public static readonly StringName _ParseCommand = "_ParseCommand";

		public static readonly StringName _ConvertArgs = "_ConvertArgs";

		public static readonly StringName _ConvertValue = "_ConvertValue";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static bool IsInit = false;

	public static System.Collections.Generic.Dictionary<string, CommandConfig> CommandDictionary = new System.Collections.Generic.Dictionary<string, CommandConfig>();

	public static void Init()
	{
		if (!IsInit)
		{
			IsInit = true;
			RegisterInit();
		}
	}

	public static void RegisterInit()
	{
		BuiltinCommands.Register();
		DebugCommands.Register();
	}

	public static void RegisterCommand(string name, string description, string usage, Callable callback, Array argsInfo = null)
	{
		string text = name.ToLower();
		CommandConfig commandConfig = new CommandConfig();
		commandConfig.Name = text;
		commandConfig.Description = description;
		commandConfig.Usage = usage;
		commandConfig.Callback = callback;
		Array<CommandArg> array = new Array<CommandArg>();
		if (argsInfo != null)
		{
			foreach (Variant item in argsInfo)
			{
				array.Add((CommandArg)(GodotObject)item);
			}
		}
		commandConfig.ArgsInfo = array;
		CommandDictionary[text] = commandConfig;
	}

	public static void UnregisterCommand(string name)
	{
		CommandDictionary.Remove(name.ToLower());
	}

	public static bool HasCommand(string name)
	{
		return CommandDictionary.ContainsKey(name.ToLower());
	}

	public static CommandConfig GetCommand(string name)
	{
		return CommandDictionary.GetValueOrDefault(name.ToLower());
	}

	public static System.Collections.Generic.Dictionary<string, CommandConfig> GetAllCommands()
	{
		return CommandDictionary;
	}

	public static Array<string> GetCommandNames()
	{
		Array<string> array = new Array<string>();
		foreach (string key in CommandDictionary.Keys)
		{
			array.Add(key);
		}
		array.Sort();
		return array;
	}

	public static void ExecuteCommand(string input, Callable outputCallback)
	{
		string text = input.StripEdges();
		if (text == "")
		{
			return;
		}
		if (!text.StartsWith("/"))
		{
			outputCallback.Call("[color=red]指令必须以 / 开头[/color]");
			return;
		}
		Array<string> array = _ParseCommand(text.Substr(1, text.Length - 1));
		if (array.Count == 0)
		{
			return;
		}
		string text2 = array[0].ToLower();
		Array<string> array2 = new Array<string>();
		if (array.Count > 1)
		{
			for (int i = 1; i < array.Count; i++)
			{
				array2.Add(array[i]);
			}
		}
		if (!CommandDictionary.ContainsKey(text2))
		{
			outputCallback.Call("[color=red]未知指令: /" + text2 + "  输入 /help 查看所有指令[/color]");
			return;
		}
		CommandConfig commandConfig = CommandDictionary[text2];
		Array array3 = _ConvertArgs(array2, commandConfig.ArgsInfo, outputCallback);
		if (array3 == null)
		{
			outputCallback.Call("[color=red]参数错误！用法: " + commandConfig.Usage + "[/color]");
		}
		else
		{
			commandConfig.Callback.Call(array3);
		}
	}

	public static Array<string> _ParseCommand(string input)
	{
		Array<string> array = new Array<string>();
		string text = "";
		bool flag = false;
		for (int i = 0; i < input.Length; i++)
		{
			string text2 = input.Substr(i, 1);
			if (text2 == "\"")
			{
				flag = !flag;
			}
			else if (text2 == " " && !flag)
			{
				if (text != "")
				{
					array.Add(text);
					text = "";
				}
			}
			else
			{
				text += text2;
			}
		}
		if (text != "")
		{
			array.Add(text);
		}
		return array;
	}

	private static Array _ConvertArgs(Array<string> args, Array<CommandArg> argsInfo, Callable outputCallback)
	{
		Array array = new Array();
		int i = 0;
		foreach (CommandArg item2 in argsInfo)
		{
			if (i >= args.Count)
			{
				if (item2.Required)
				{
					return null;
				}
				array.Add(item2.DefaultValue);
				continue;
			}
			Variant item = _ConvertValue(args[i], item2.Type);
			if (item.VariantType == Variant.Type.Nil)
			{
				outputCallback.Call($"[color=red]参数 '{item2.Name}' 类型错误，期望类型: {((Variant.Type)item2.Type/*cast due to constrained. prefix*/).ToString()}[/color]");
				return null;
			}
			array.Add(item);
			i++;
		}
		for (; i < args.Count; i++)
		{
			array.Add(args[i]);
		}
		return array;
	}

	private static Variant _ConvertValue(string value, int targetType)
	{
		Variant.Type type = (Variant.Type)targetType;
		Variant.Type num = type - 1;
		if ((ulong)num <= 3uL)
		{
			switch ((int)num)
			{
			case 3:
				return value;
			case 1:
			{
				if (int.TryParse(value, out var result2))
				{
					return result2;
				}
				return default;
			}
			case 2:
			{
				if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
				{
					return (float)result;
				}
				return default;
			}
			case 0:
				switch (value.ToLower())
				{
				case "1":
				case "yes":
				case "on":
				case "true":
					return true;
				case "0":
				case "off":
				case "no":
				case "false":
					return false;
				default:
					return default;
				}
			}
		}
		return value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RegisterCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "description", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "usage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Callable, "callback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "argsInfo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UnregisterCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasCommand, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCommand, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCommandNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ExecuteCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "input", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Callable, "outputCallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ParseCommand, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "input", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ConvertArgs, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "args", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "argsInfo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Callable, "outputCallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ConvertValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterCommand && args.Count == 5)
		{
			RegisterCommand(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Callable>(in args[3]), VariantUtils.ConvertTo<Array>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterCommand && args.Count == 1)
		{
			UnregisterCommand(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasCommand && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCommand(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCommand && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CommandConfig>(GetCommand(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCommandNames && args.Count == 0)
		{
			Array<string> commandNames = GetCommandNames();
			ret = VariantUtils.CreateFromArray(commandNames);
			return true;
		}
		if (method == MethodName.ExecuteCommand && args.Count == 2)
		{
			ExecuteCommand(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Callable>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._ParseCommand && args.Count == 1)
		{
			Array<string> array = _ParseCommand(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._ConvertArgs && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Array>(_ConvertArgs(VariantUtils.ConvertToArray<string>(in args[0]), VariantUtils.ConvertToArray<CommandArg>(in args[1]), VariantUtils.ConvertTo<Callable>(in args[2])));
			return true;
		}
		if (method == MethodName._ConvertValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(_ConvertValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterInit && args.Count == 0)
		{
			RegisterInit();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterCommand && args.Count == 5)
		{
			RegisterCommand(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Callable>(in args[3]), VariantUtils.ConvertTo<Array>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterCommand && args.Count == 1)
		{
			UnregisterCommand(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasCommand && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCommand(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCommand && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CommandConfig>(GetCommand(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCommandNames && args.Count == 0)
		{
			Array<string> commandNames = GetCommandNames();
			ret = VariantUtils.CreateFromArray(commandNames);
			return true;
		}
		if (method == MethodName.ExecuteCommand && args.Count == 2)
		{
			ExecuteCommand(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Callable>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._ParseCommand && args.Count == 1)
		{
			Array<string> array = _ParseCommand(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._ConvertArgs && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Array>(_ConvertArgs(VariantUtils.ConvertToArray<string>(in args[0]), VariantUtils.ConvertToArray<CommandArg>(in args[1]), VariantUtils.ConvertTo<Callable>(in args[2])));
			return true;
		}
		if (method == MethodName._ConvertValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(_ConvertValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.RegisterInit)
		{
			return true;
		}
		if (method == MethodName.RegisterCommand)
		{
			return true;
		}
		if (method == MethodName.UnregisterCommand)
		{
			return true;
		}
		if (method == MethodName.HasCommand)
		{
			return true;
		}
		if (method == MethodName.GetCommand)
		{
			return true;
		}
		if (method == MethodName.GetCommandNames)
		{
			return true;
		}
		if (method == MethodName.ExecuteCommand)
		{
			return true;
		}
		if (method == MethodName._ParseCommand)
		{
			return true;
		}
		if (method == MethodName._ConvertArgs)
		{
			return true;
		}
		if (method == MethodName._ConvertValue)
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
