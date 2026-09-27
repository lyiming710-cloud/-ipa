using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Command/BuiltinCommands.cs")]
public class BuiltinCommands : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName _CmdHelp = "_CmdHelp";

		public static readonly StringName _CmdClear = "_CmdClear";

		public static readonly StringName _CmdEcho = "_CmdEcho";

		public static readonly StringName _CmdHistory = "_CmdHistory";

		public static readonly StringName _CmdQuit = "_CmdQuit";

		public static readonly StringName _CmdFps = "_CmdFps";

		public static readonly StringName _CmdTimeScale = "_CmdTimeScale";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static void Register()
	{
		CommandRegistry.RegisterCommand("help", "显示所有可用指令", "/help [页码]", Callable.From((int page) =>
		{
			_CmdHelp(page);
		}), new Array
		{
			new CommandArg("page", 2, _required: false, 1, "页码")
		});
		CommandRegistry.RegisterCommand("clear", "清空控制台输出", "/clear", Callable.From(() =>
		{
			_CmdClear();
		}));
		CommandRegistry.RegisterCommand("echo", "输出文本到控制台", "/echo <文本>", Callable.From((string text) =>
		{
			_CmdEcho(text);
		}), new Array
		{
			new CommandArg("text", 4, _required: true, default, "要输出的文本")
		});
		CommandRegistry.RegisterCommand("history", "显示指令历史记录", "/history", Callable.From(() =>
		{
			_CmdHistory();
		}));
		CommandRegistry.RegisterCommand("quit", "关闭游戏", "/quit", Callable.From(() =>
		{
			_CmdQuit();
		}));
		CommandRegistry.RegisterCommand("fps", "显示当前帧率", "/fps", Callable.From(() =>
		{
			_CmdFps();
		}));
		CommandRegistry.RegisterCommand("timeScale", "设置游戏时间缩放", "/timeScale <倍率>", Callable.From((double scale) =>
		{
			_CmdTimeScale(scale);
		}), new Array
		{
			new CommandArg("scale", 3, _required: true, default, "时间缩放倍率")
		});
	}

	public static void _CmdHelp(int page = 1)
	{
		Array<string> commandNames = CommandRegistry.GetCommandNames();
		int num = 8;
		int num2 = Mathf.Max(1, Mathf.CeilToInt((float)commandNames.Count / (float)num));
		page = Mathf.Clamp(page, 1, num2);
		CommandConsole.Instance.PrintLine($"[color=cyan]═══════ 指令列表 (第{page}页/共{num2}页) ═══════[/color]");
		int num3 = (page - 1) * num;
		int num4 = Mathf.Min(num3 + num, commandNames.Count);
		for (int i = num3; i < num4; i++)
		{
			string text = commandNames[i];
			CommandConfig command = CommandRegistry.GetCommand(text);
			CommandConsole.Instance.PrintLine("[color=green]/" + text + "[/color] - " + command.Description);
			CommandConsole.Instance.PrintLine("[color=gray]  用法: " + command.Usage + "[/color]");
		}
		if (num2 > 1)
		{
			CommandConsole.Instance.PrintInfo($"输入 /help {page + 1} 查看下一页");
		}
	}

	public static void _CmdClear()
	{
		CommandConsole.Instance.ClearLog();
	}

	public static void _CmdEcho(string text)
	{
		CommandConsole.Instance.PrintLine(text);
	}

	public static void _CmdHistory()
	{
		Array<string> history = CommandConsole.Instance.GetHistory();
		if (history.Count == 0)
		{
			CommandConsole.Instance.PrintInfo("暂无指令历史");
			return;
		}
		CommandConsole.Instance.PrintLine("[color=cyan]═══════ 指令历史 ═══════[/color]");
		for (int i = 0; i < history.Count; i++)
		{
			CommandConsole.Instance.PrintLine($"[color=gray]{i + 1}.[/color] {history[i]}");
		}
	}

	public static void _CmdQuit()
	{
		CommandConsole.Instance.PrintWarning("正在退出游戏...");
		((SceneTree)Engine.GetMainLoop()).Quit();
	}

	public static void _CmdFps()
	{
		CommandConsole.Instance.PrintInfo($"当前FPS: {Engine.GetFramesPerSecond()}");
	}

	public static void _CmdTimeScale(double scale)
	{
		Global.TimeScale = scale;
		CommandConsole.Instance.PrintSuccess($"时间缩放已设为 {scale:F1}x");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdHelp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "page", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CmdClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdEcho, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CmdHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdQuit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdFps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdTimeScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 0)
		{
			Register();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdHelp && args.Count == 1)
		{
			_CmdHelp(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdClear && args.Count == 0)
		{
			_CmdClear();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdEcho && args.Count == 1)
		{
			_CmdEcho(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdHistory && args.Count == 0)
		{
			_CmdHistory();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdQuit && args.Count == 0)
		{
			_CmdQuit();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdFps && args.Count == 0)
		{
			_CmdFps();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdTimeScale && args.Count == 1)
		{
			_CmdTimeScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 0)
		{
			Register();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdHelp && args.Count == 1)
		{
			_CmdHelp(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdClear && args.Count == 0)
		{
			_CmdClear();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdEcho && args.Count == 1)
		{
			_CmdEcho(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdHistory && args.Count == 0)
		{
			_CmdHistory();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdQuit && args.Count == 0)
		{
			_CmdQuit();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdFps && args.Count == 0)
		{
			_CmdFps();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdTimeScale && args.Count == 1)
		{
			_CmdTimeScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName._CmdHelp)
		{
			return true;
		}
		if (method == MethodName._CmdClear)
		{
			return true;
		}
		if (method == MethodName._CmdEcho)
		{
			return true;
		}
		if (method == MethodName._CmdHistory)
		{
			return true;
		}
		if (method == MethodName._CmdQuit)
		{
			return true;
		}
		if (method == MethodName._CmdFps)
		{
			return true;
		}
		if (method == MethodName._CmdTimeScale)
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
