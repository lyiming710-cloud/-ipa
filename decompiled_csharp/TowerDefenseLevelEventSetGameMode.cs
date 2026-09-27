using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Game/TowerDefenseLevelEventSetGameMode.cs")]
public class TowerDefenseLevelEventSetGameMode : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName gameMode = "gameMode";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.GAMEMODE gameMode;

	public override string _GetName()
	{
		return "LEVLE_EVENT_SET_GAME_MODE";
	}

	public override void Execute()
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (!GodotObject.IsInstanceValid(currentControl))
		{
			return;
		}
		switch (gameMode)
		{
		case TowerDefenseEnum.GAMEMODE.HAMMER:
			if (!currentControl.featureDictionary.ContainsKey("Hammer"))
			{
				currentControl.AddFeature("Hammer", new Dictionary());
			}
			break;
		case TowerDefenseEnum.GAMEMODE.TOWERDEFENSE:
			if (currentControl.featureDictionary.ContainsKey("Hammer"))
			{
				currentControl.RemoveFeature("Hammer");
			}
			break;
		}
	}

	public override void Init(Dictionary valueDictionary)
	{
		string value = valueDictionary.GetValueOrDefault("GameMode", "TOWERDEFENSE").AsString().ToUpper();
		gameMode = Enum.Parse<TowerDefenseEnum.GAMEMODE>(value);
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "SetGameMode",
			["Value"] = new Dictionary { ["GameMode"] = Enum.GetName(gameMode) }
		};
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["游戏模式"] = new Dictionary { ["模式"] = new Dictionary
		{
			["Object"] = this,
			["Type"] = "Enum",
			["Property"] = "gameMode",
			["Hint"] = 0,
			["Rest"] = 0
		} };
		return property;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName.Execute && args.Count == 0)
		{
			Execute();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.GetProperty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetProperty());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.GetProperty)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.gameMode)
		{
			gameMode = VariantUtils.ConvertTo<TowerDefenseEnum.GAMEMODE>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.gameMode)
		{
			value = VariantUtils.CreateFrom(in gameMode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.gameMode, PropertyHint.Enum, "TOWERDEFENSE,HAMMER", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.gameMode, Variant.From(in gameMode));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.gameMode, out var value))
		{
			gameMode = value.As<TowerDefenseEnum.GAMEMODE>();
		}
	}
}
