using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/TutorialManager/Resource/TutorialStep/TutorialStepConfig.cs")]
public class TutorialStepConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Enter = "Enter";

		public static readonly StringName Exit = "Exit";

		public static readonly StringName Step = "Step";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName broadCastUse = "broadCastUse";

		public static readonly StringName broadCastConfig = "broadCastConfig";

		public static readonly StringName conditionList = "conditionList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool broadCastUse;

	[Export(PropertyHint.None, "")]
	public BroadCastConfig broadCastConfig;

	[Export(PropertyHint.None, "")]
	public Array<TutorialConditionConfig> conditionList = new Array<TutorialConditionConfig>();

	public void Init(Dictionary data)
	{
		broadCastUse = false;
		broadCastConfig = null;
		conditionList.Clear();
		if (data.ContainsKey("BroadCast"))
		{
			broadCastUse = true;
			broadCastConfig = new BroadCastConfig();
			Dictionary dictionary = data["BroadCast"].AsGodotDictionary();
			broadCastConfig.broadCastString = dictionary.GetValueOrDefault("Text", "").AsString();
			Variant valueOrDefault = dictionary.GetValueOrDefault("Time", -1.0);
			broadCastConfig.broadCastTime = ((valueOrDefault.VariantType == Variant.Type.Nil) ? (-1.0) : valueOrDefault.AsDouble());
		}
		foreach (Variant item in data.GetValueOrDefault("Condition", new Array()).AsGodotArray())
		{
			Dictionary dictionary2 = item.AsGodotDictionary();
			TutorialConditionConfig condition = TutorialEnum.GetCondition(dictionary2["Name"].AsString());
			condition.Init(dictionary2["Data"].AsGodotDictionary());
			conditionList.Add(condition);
		}
	}

	public void Enter()
	{
		foreach (TutorialConditionConfig condition in conditionList)
		{
			condition.Enter();
		}
	}

	public void Exit()
	{
	}

	public bool Step()
	{
		foreach (TutorialConditionConfig condition in conditionList)
		{
			if (!condition.Step())
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
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Exit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Enter && args.Count == 0)
		{
			Enter();
			ret = default;
			return true;
		}
		if (method == MethodName.Exit && args.Count == 0)
		{
			Exit();
			ret = default;
			return true;
		}
		if (method == MethodName.Step && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Step());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.Exit)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.broadCastUse)
		{
			broadCastUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.broadCastConfig)
		{
			broadCastConfig = VariantUtils.ConvertTo<BroadCastConfig>(in value);
			return true;
		}
		if (name == PropertyName.conditionList)
		{
			conditionList = VariantUtils.ConvertToArray<TutorialConditionConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.broadCastUse)
		{
			value = VariantUtils.CreateFrom(in broadCastUse);
			return true;
		}
		if (name == PropertyName.broadCastConfig)
		{
			value = VariantUtils.CreateFrom(in broadCastConfig);
			return true;
		}
		if (name == PropertyName.conditionList)
		{
			value = VariantUtils.CreateFromArray(conditionList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.broadCastUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.broadCastConfig, PropertyHint.ResourceType, "BroadCastConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.conditionList, PropertyHint.TypeString, "24/17:TutorialConditionConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.broadCastUse, Variant.From(in broadCastUse));
		info.AddProperty(PropertyName.broadCastConfig, Variant.From(in broadCastConfig));
		info.AddProperty(PropertyName.conditionList, Variant.CreateFrom(conditionList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.broadCastUse, out var value))
		{
			broadCastUse = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.broadCastConfig, out var value2))
		{
			broadCastConfig = value2.As<BroadCastConfig>();
		}
		if (info.TryGetProperty(PropertyName.conditionList, out var value3))
		{
			conditionList = value3.AsGodotArray<TutorialConditionConfig>();
		}
	}
}
