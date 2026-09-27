using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Command/ShovelCommand/ShovelCommand.cs")]
public class ShovelCommand : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName ListShovels = "ListShovels";

		public static readonly StringName ChangeShovel = "ChangeShovel";

		public static readonly StringName GetShovelNames = "GetShovelNames";

		public static readonly StringName _GetCurrentShovel = "_GetCurrentShovel";

		public static readonly StringName _UpdateShovelManager = "_UpdateShovelManager";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static string currentShovel = "";

	public static void ListShovels()
	{
		Array shovelList = TowerDefenseManager.GetShovelList();
		string text = _GetCurrentShovel();
		CommandConsole.Instance.PrintLine("[color=cyan]═══════ 铲子列表 ═══════[/color]");
		foreach (Variant item in shovelList)
		{
			string text2 = (string)item;
			ShovelConfig shovel = TowerDefenseManager.GetShovel(text2);
			string value = ((text2 == text) ? "[color=green]►[/color] " : "  ");
			string value2 = ((shovel != null) ? ((string?)TranslationServer.Translate(shovel.name)) : text2);
			CommandConsole.Instance.PrintLine($"{value}{value2} [color=gray]({text2})[/color]");
		}
	}

	public static void ChangeShovel(string shovelName)
	{
		ShovelConfig shovel = TowerDefenseManager.GetShovel(shovelName);
		if (shovel == null)
		{
			CommandConsole.Instance.PrintError("未找到铲子: " + shovelName + "  输入 /shovel list 查看所有铲子");
			return;
		}
		currentShovel = shovelName;
		GameSaveManager.Instance.SetKeyValue("CurrentShovel", shovelName);
		_UpdateShovelManager(shovel);
		CommandConsole.Instance.PrintSuccess("已更换铲子: " + TranslationServer.Translate(shovel.name));
	}

	public static Array GetShovelNames()
	{
		Array array = new Array();
		array.Add("list");
		array.AddRange(TowerDefenseManager.GetShovelList());
		return array;
	}

	public static string _GetCurrentShovel()
	{
		if (currentShovel == "")
		{
			currentShovel = (string)GameSaveManager.Instance.GetKeyValue("CurrentShovel");
			if (string.IsNullOrEmpty(currentShovel))
			{
				currentShovel = "ShovelDefault";
			}
		}
		return currentShovel;
	}

	public static void _UpdateShovelManager(ShovelConfig shovelConfig)
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.Instance.currentControl;
		if (!GodotObject.IsInstanceValid(currentControl))
		{
			return;
		}
		TowerDefenseBattleFeatureShovel towerDefenseBattleFeatureShovel = currentControl.GetFeature("Shovel") as TowerDefenseBattleFeatureShovel;
		if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureShovel) && GodotObject.IsInstanceValid(towerDefenseBattleFeatureShovel.shovelManager))
		{
			towerDefenseBattleFeatureShovel.shovelManager.shovelConfig = shovelConfig;
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureShovel.shovelManager.shovelSprite))
			{
				towerDefenseBattleFeatureShovel.shovelManager.shovelSprite.Texture = shovelConfig.texture;
			}
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureShovel.shovelManager.mapShovelSprite))
			{
				towerDefenseBattleFeatureShovel.shovelManager.mapShovelSprite.Texture = shovelConfig.texture;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.ListShovels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ChangeShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "shovelName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetShovelNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._GetCurrentShovel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._UpdateShovelManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shovelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ListShovels && args.Count == 0)
		{
			ListShovels();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeShovel && args.Count == 1)
		{
			ChangeShovel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetShovelNames && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetShovelNames());
			return true;
		}
		if (method == MethodName._GetCurrentShovel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetCurrentShovel());
			return true;
		}
		if (method == MethodName._UpdateShovelManager && args.Count == 1)
		{
			_UpdateShovelManager(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ListShovels && args.Count == 0)
		{
			ListShovels();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeShovel && args.Count == 1)
		{
			ChangeShovel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetShovelNames && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Array>(GetShovelNames());
			return true;
		}
		if (method == MethodName._GetCurrentShovel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetCurrentShovel());
			return true;
		}
		if (method == MethodName._UpdateShovelManager && args.Count == 1)
		{
			_UpdateShovelManager(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ListShovels)
		{
			return true;
		}
		if (method == MethodName.ChangeShovel)
		{
			return true;
		}
		if (method == MethodName.GetShovelNames)
		{
			return true;
		}
		if (method == MethodName._GetCurrentShovel)
		{
			return true;
		}
		if (method == MethodName._UpdateShovelManager)
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
