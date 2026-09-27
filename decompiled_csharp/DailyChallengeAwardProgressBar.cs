using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/Award/Progress/DailyChallengeAwardProgressBar.cs")]
public class DailyChallengeAwardProgressBar : TextureProgressBar
{
	public new class MethodName : TextureProgressBar.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";
	}

	public new class PropertyName : TextureProgressBar.PropertyName
	{
		public static readonly StringName tickNode = "tickNode";

		public static readonly StringName awardList = "awardList";

		public static readonly StringName finish = "finish";

		public static readonly StringName dayAll = "dayAll";
	}

	public new class SignalName : TextureProgressBar.SignalName
	{
	}

	private static Texture2D _rewardProgressTick;

	public Control tickNode;

	public Array awardList;

	public int finish;

	public int dayAll = 30;

	private static Texture2D REWARD_PROGRESS_TICK => _rewardProgressTick ?? (_rewardProgressTick = GD.Load<Texture2D>("res://Asset/Texture/GUI/DailyChallenge/Award/RewardProgressTick.png"));

	public override void _Ready()
	{
		tickNode = GetNode<Control>("%TickNode");
	}

	public void Init(Array _awardList, int _finish, int _dayAll)
	{
		awardList = _awardList;
		finish = _finish;
		dayAll = _dayAll;
		MaxValue = dayAll;
		Value = finish;
		foreach (Variant _award in _awardList)
		{
			Dictionary dictionary = (Dictionary)_award;
			TextureRect textureRect = new TextureRect();
			textureRect.Texture = REWARD_PROGRESS_TICK;
			Array array = (Array)dictionary["ConditionArg"];
			textureRect.Position = new Vector2((float)(int)array[0] / (float)dayAll * Size.X, 5f);
			tickNode.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
			TextureRect textureRect2 = new TextureRect();
			textureRect2.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			textureRect2.Texture = GD.Load<Texture2D>((string)dictionary["Icon"]);
			textureRect2.Size = new Vector2(54f, 54f);
			textureRect2.Position = -new Vector2(27f, 27f) - new Vector2(0f, 35f);
			textureRect.AddChild(textureRect2, forceReadableName: false, InternalMode.Disabled);
			Label label = new Label();
			label.Text = ((int)array[0]).ToString();
			label.AddThemeConstantOverride("outline_size", 5);
			label.Position = new Vector2(0f, 30f);
			textureRect.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "_awardList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_finish", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_dayAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 3)
		{
			Init(VariantUtils.ConvertTo<Array>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.tickNode)
		{
			tickNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.awardList)
		{
			awardList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.finish)
		{
			finish = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.dayAll)
		{
			dayAll = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.tickNode)
		{
			value = VariantUtils.CreateFrom(in tickNode);
			return true;
		}
		if (name == PropertyName.awardList)
		{
			value = VariantUtils.CreateFrom(in awardList);
			return true;
		}
		if (name == PropertyName.finish)
		{
			value = VariantUtils.CreateFrom(in finish);
			return true;
		}
		if (name == PropertyName.dayAll)
		{
			value = VariantUtils.CreateFrom(in dayAll);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.tickNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.awardList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finish, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.dayAll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.tickNode, Variant.From(in tickNode));
		info.AddProperty(PropertyName.awardList, Variant.From(in awardList));
		info.AddProperty(PropertyName.finish, Variant.From(in finish));
		info.AddProperty(PropertyName.dayAll, Variant.From(in dayAll));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.tickNode, out var value))
		{
			tickNode = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.awardList, out var value2))
		{
			awardList = value2.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.finish, out var value3))
		{
			finish = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.dayAll, out var value4))
		{
			dayAll = value4.As<int>();
		}
	}
}
