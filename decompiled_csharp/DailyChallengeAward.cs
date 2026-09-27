using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/Award/DailyChallengeAward.cs")]
public class DailyChallengeAward : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitDialog = "InitDialog";

		public static readonly StringName CloseButtonPressed = "CloseButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName dailyChallengeAwardProgressBar = "dailyChallengeAwardProgressBar";

		public static readonly StringName itemContainer = "itemContainer";

		public static readonly StringName textLabel = "textLabel";

		public static readonly StringName year = "year";

		public static readonly StringName month = "month";

		public static readonly StringName finish = "finish";

		public static readonly StringName dayAll = "dayAll";

		public static readonly StringName currentAwardList = "currentAwardList";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private static PackedScene _dailyChallengeAwardItem;

	public TextureProgressBar dailyChallengeAwardProgressBar;

	public VBoxContainer itemContainer;

	public Label textLabel;

	public int year = 2025;

	public int month = 1;

	public int finish;

	public int dayAll = 1;

	public Array currentAwardList;

	private static PackedScene DAILY_CHALLENGE_AWARD_ITEM => _dailyChallengeAwardItem ?? (_dailyChallengeAwardItem = GD.Load<PackedScene>("uid://b1cbbcykkioe0"));

	public override void _Ready()
	{
		base._Ready();
		dailyChallengeAwardProgressBar = GetNode<TextureProgressBar>("%DailyChallengeAwardProgressBar");
		itemContainer = GetNode<VBoxContainer>("%ItemContainer");
		textLabel = GetNode<Label>("%TextLabel");
		GetNode<TextureButton>("Layer/BackgroundTexture/CloseButton").Pressed += CloseButtonPressed;
		GetNode<BaseButton>("%BackButton").Pressed += CloseButtonPressed;
	}

	public void InitDialog(int _year, int _month, int _finish, int _dayAll)
	{
		year = _year;
		month = _month;
		finish = _finish;
		dayAll = _dayAll;
		textLabel.Text = $"{year}/{month}月奖励";
		Dictionary dictionary = ResourceManager.DAILY_LEVEL_AWARD.Data.AsGodotDictionary();
		string text = $"{year}-{month:D2}";
		if (dictionary.ContainsKey(text))
		{
			currentAwardList = (Array)dictionary[text];
		}
		else
		{
			currentAwardList = new Array();
		}
		((DailyChallengeAwardProgressBar)dailyChallengeAwardProgressBar).Init(currentAwardList, finish, dayAll);
		foreach (Variant currentAward in currentAwardList)
		{
			DailyChallengeAwardItem dailyChallengeAwardItem = DAILY_CHALLENGE_AWARD_ITEM.Instantiate<DailyChallengeAwardItem>(PackedScene.GenEditState.Disabled);
			itemContainer.AddChild(dailyChallengeAwardItem, forceReadableName: false, InternalMode.Disabled);
			dailyChallengeAwardItem.Init((Dictionary)currentAward, finish);
		}
	}

	public void CloseButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_year", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_month", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_finish", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_dayAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitDialog && args.Count == 4)
		{
			InitDialog(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseButtonPressed && args.Count == 0)
		{
			CloseButtonPressed();
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
		if (method == MethodName.InitDialog)
		{
			return true;
		}
		if (method == MethodName.CloseButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dailyChallengeAwardProgressBar)
		{
			dailyChallengeAwardProgressBar = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName.itemContainer)
		{
			itemContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.textLabel)
		{
			textLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.year)
		{
			year = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.month)
		{
			month = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName.currentAwardList)
		{
			currentAwardList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dailyChallengeAwardProgressBar)
		{
			value = VariantUtils.CreateFrom(in dailyChallengeAwardProgressBar);
			return true;
		}
		if (name == PropertyName.itemContainer)
		{
			value = VariantUtils.CreateFrom(in itemContainer);
			return true;
		}
		if (name == PropertyName.textLabel)
		{
			value = VariantUtils.CreateFrom(in textLabel);
			return true;
		}
		if (name == PropertyName.year)
		{
			value = VariantUtils.CreateFrom(in year);
			return true;
		}
		if (name == PropertyName.month)
		{
			value = VariantUtils.CreateFrom(in month);
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
		if (name == PropertyName.currentAwardList)
		{
			value = VariantUtils.CreateFrom(in currentAwardList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.dailyChallengeAwardProgressBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.itemContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.textLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.year, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.month, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finish, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.dayAll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.currentAwardList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dailyChallengeAwardProgressBar, Variant.From(in dailyChallengeAwardProgressBar));
		info.AddProperty(PropertyName.itemContainer, Variant.From(in itemContainer));
		info.AddProperty(PropertyName.textLabel, Variant.From(in textLabel));
		info.AddProperty(PropertyName.year, Variant.From(in year));
		info.AddProperty(PropertyName.month, Variant.From(in month));
		info.AddProperty(PropertyName.finish, Variant.From(in finish));
		info.AddProperty(PropertyName.dayAll, Variant.From(in dayAll));
		info.AddProperty(PropertyName.currentAwardList, Variant.From(in currentAwardList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dailyChallengeAwardProgressBar, out var value))
		{
			dailyChallengeAwardProgressBar = value.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName.itemContainer, out var value2))
		{
			itemContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.textLabel, out var value3))
		{
			textLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.year, out var value4))
		{
			year = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.month, out var value5))
		{
			month = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.finish, out var value6))
		{
			finish = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.dayAll, out var value7))
		{
			dayAll = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentAwardList, out var value8))
		{
			currentAwardList = value8.As<Array>();
		}
	}
}
