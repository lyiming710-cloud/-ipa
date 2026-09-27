using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/DailyChallenge.cs")]
public class DailyChallenge : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RefreshPlayButtonState = "RefreshPlayButtonState";

		public static readonly StringName CalendarSelect = "CalendarSelect";

		public static readonly StringName _OnDailyLevelMonthLoaded = "_OnDailyLevelMonthLoaded";

		public static readonly StringName CloseButtonPressed = "CloseButtonPressed";

		public static readonly StringName PlayButtonPressed = "PlayButtonPressed";

		public static readonly StringName RefreshTodayAwardText = "RefreshTodayAwardText";

		public static readonly StringName RefreshMonthFinish = "RefreshMonthFinish";

		public static readonly StringName AwardButtonPressed = "AwardButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName finishLabel = "finishLabel";

		public static readonly StringName calendar = "calendar";

		public static readonly StringName playButton = "playButton";

		public static readonly StringName awardLabel = "awardLabel";

		public static readonly StringName year = "year";

		public static readonly StringName month = "month";

		public static readonly StringName day = "day";

		public static readonly StringName currentDate = "currentDate";

		public static readonly StringName finishNum = "finishNum";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	public RichTextLabel finishLabel;

	public Calendar calendar;

	public MainButton playButton;

	public RichTextLabel awardLabel;

	public int year = 2025;

	public int month = 1;

	public int day = 1;

	public string currentDate = "-1--1--1";

	public int finishNum;

	public static int GetRequiredClearCount(System.Collections.Generic.Dictionary<string, Variant> dailyLevelData, string date, int levelCount)
	{
		int b = 1;
		if (dailyLevelData != null && dailyLevelData.ContainsKey("LevelDateMeta") && dailyLevelData["LevelDateMeta"].VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary = dailyLevelData["LevelDateMeta"].AsGodotDictionary();
			if (dictionary != null && dictionary.ContainsKey(date) && dictionary[date].VariantType == Variant.Type.Dictionary)
			{
				Dictionary dictionary2 = dictionary[date].AsGodotDictionary();
				if (dictionary2 != null && dictionary2.ContainsKey("minClearCount"))
				{
					b = dictionary2["minClearCount"].AsInt32();
				}
			}
		}
		return Mathf.Max(Mathf.Min(levelCount, b), 1);
	}

	public override void _Ready()
	{
		base._Ready();
		finishLabel = GetNode<RichTextLabel>("%FinishLabel");
		calendar = GetNode<Calendar>("%Calendar");
		calendar.OnSelect += CalendarSelect;
		year = calendar.year;
		month = calendar.month;
		day = calendar.day;
		currentDate = $"{year}-{month:D2}-{day:D2}";
		playButton = GetNode<MainButton>("%PlayButton");
		awardLabel = GetNode<RichTextLabel>("%AwardLabel");
		GetNode<BaseButton>("Layer/BackgroundTexture/CloseButton").Pressed += CloseButtonPressed;
		GetNode<BaseButton>("%PlayButton").Pressed += PlayButtonPressed;
		GetNode<NinePatchButtonBase>("Layer/BackgroundTexture/AwardButton").OnPressed += AwardButtonPressed;
		InternetServerManager.Instance.OnDailyLevelMonthLoaded += _OnDailyLevelMonthLoaded;
		if (InternetServerManager.Instance.DailyLevelMonthNeedRefresh(calendar.year, calendar.month))
		{
			bool forceRefresh = InternetServerManager.Instance.dailyLevelLoadedMonths.ContainsKey($"{calendar.year}-{calendar.month:D2}");
			InternetServerManager.Instance.GetDailyLevel(calendar.year, calendar.month, forceRefresh);
		}
		RefreshMonthFinish();
		RefreshTodayAwardText();
		RefreshPlayButtonState();
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		if (dAILY_LEVEL_DATA == null || dAILY_LEVEL_DATA.Count <= 0 || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap"))
		{
			return;
		}
		Dictionary dictionary = (Dictionary)dAILY_LEVEL_DATA["LevelDateMap"];
		Dictionary dictionary2 = (Dictionary)dAILY_LEVEL_DATA["LevelMeta"];
		if (!dictionary.ContainsKey(calendar.today))
		{
			return;
		}
		Array array = (Array)dictionary[calendar.today];
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < array.Count; i++)
		{
			string arg = (string)array[i];
			string key = $"DailyLevel-{calendar.today}-{arg}";
			Dictionary dictionary3 = GameSaveManager.Instance.GetLevelValue(key).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
			if (dictionary3 == null || (int)dictionary3.GetValueOrDefault("Finish", 0) <= 0)
			{
				if (num2 < 0)
				{
					num2 = i;
				}
			}
			else
			{
				num++;
			}
		}
		if (num >= GetRequiredClearCount(dAILY_LEVEL_DATA, calendar.today, array.Count))
		{
			awardLabel.Text = "今日悬赏已完成";
		}
		else if (num2 >= 0)
		{
			string text = (string)array[num2];
			Dictionary dictionary4 = (Dictionary)((Dictionary)dictionary2[text])["reward"];
			if ((string)dictionary4["type"] == "Coin")
			{
				awardLabel.Text = string.Format("当天还需要通过[color=red]{0}[/color]关", num2 + 1, dictionary4["value"]);
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (InternetServerManager.Instance != null)
		{
			InternetServerManager.Instance.OnDailyLevelMonthLoaded -= _OnDailyLevelMonthLoaded;
		}
	}

	private void RefreshPlayButtonState()
	{
		if (GodotObject.IsInstanceValid(playButton))
		{
			System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
			bool disabled = dAILY_LEVEL_DATA == null || dAILY_LEVEL_DATA.Count == 0 || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap") || !((Dictionary)dAILY_LEVEL_DATA["LevelDateMap"]).ContainsKey(currentDate);
			playButton.Disabled = disabled;
		}
	}

	public void CalendarSelect(int _year, int _month, int _day)
	{
		if (_year <= 0 || _month <= 0)
		{
			year = _year;
			month = _month;
			day = _day;
			currentDate = $"{year}-{month:D2}-{day:D2}";
			RefreshPlayButtonState();
			return;
		}
		if (month != _month)
		{
			Dictionary datetimeDictFromSystem = Time.GetDatetimeDictFromSystem();
			int num = (int)(long)datetimeDictFromSystem["year"];
			int num2 = (int)(long)datetimeDictFromSystem["month"];
			if (_year <= num && (_year != num || _month <= num2) && InternetServerManager.Instance.DailyLevelMonthNeedRefresh(_year, _month))
			{
				bool forceRefresh = InternetServerManager.Instance.dailyLevelLoadedMonths.ContainsKey($"{_year}-{_month:D2}");
				InternetServerManager.Instance.GetDailyLevel(_year, _month, forceRefresh);
			}
			RefreshMonthFinish();
			RefreshTodayAwardText();
		}
		year = _year;
		month = _month;
		day = _day;
		currentDate = $"{year}-{month:D2}-{day:D2}";
		RefreshPlayButtonState();
		RefreshTodayAwardText();
	}

	public void _OnDailyLevelMonthLoaded(int loadedYear, int loadedMonth)
	{
		if (loadedYear == calendar.year && loadedMonth == calendar.month)
		{
			calendar.SetupCalendar();
			RefreshMonthFinish();
			RefreshTodayAwardText();
			RefreshPlayButtonState();
		}
	}

	public void CloseButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		CloseDialog();
	}

	public void PlayButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		DialogCreate("DailyChallengeLevelChoose", new Dictionary { ["date"] = currentDate });
	}

	public void RefreshTodayAwardText()
	{
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		if (dAILY_LEVEL_DATA == null || dAILY_LEVEL_DATA.Count == 0 || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap"))
		{
			awardLabel.Text = "";
			return;
		}
		Dictionary dictionary = (Dictionary)dAILY_LEVEL_DATA["LevelDateMap"];
		if (!dictionary.ContainsKey(currentDate))
		{
			awardLabel.Text = "";
			return;
		}
		Array array = (Array)dictionary[currentDate];
		int num = 0;
		foreach (Variant item in array)
		{
			string arg = (string)item;
			string key = $"DailyLevel-{currentDate}-{arg}";
			Dictionary dictionary2 = GameSaveManager.Instance.GetLevelValue(key).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
			if (dictionary2 != null && (int)dictionary2.GetValueOrDefault("Finish", 0) > 0)
			{
				num++;
			}
		}
		int num2 = Mathf.Max(GetRequiredClearCount(dAILY_LEVEL_DATA, currentDate, array.Count) - num, 0);
		awardLabel.Text = ((num2 <= 0) ? "今日悬赏已完成" : $"当天还需要通过[color=red]{num2}[/color]关");
	}

	public async void RefreshMonthFinish()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		finishNum = 0;
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		if (dAILY_LEVEL_DATA == null || dAILY_LEVEL_DATA.Count == 0 || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap"))
		{
			finishLabel.Text = $"本月已完成挑战[color=red]{finishNum}[/color]天";
			return;
		}
		Dictionary dictionary = (Dictionary)dAILY_LEVEL_DATA["LevelDateMap"];
		foreach (Variant key2 in dictionary.Keys)
		{
			string text = (string)key2;
			string[] array = text.Split("-");
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			if (year != num || month != num2)
			{
				continue;
			}
			bool flag = true;
			int num3 = 0;
			Array array2 = (Array)dictionary[text];
			foreach (Variant item in array2)
			{
				string arg = (string)item;
				string key = $"DailyLevel-{text}-{arg}";
				Dictionary dictionary2 = GameSaveManager.Instance.GetLevelValue(key).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
				if (dictionary2 == null || (int)dictionary2.GetValueOrDefault("Finish", 0) <= 0)
				{
					flag = false;
				}
				else
				{
					num3++;
				}
			}
			if (flag)
			{
				finishNum++;
			}
			else if (num3 >= GetRequiredClearCount(dAILY_LEVEL_DATA, text, array2.Count))
			{
				finishNum++;
			}
		}
		finishLabel.Text = $"本月已完成挑战[color=red]{finishNum}[/color]天";
	}

	public void AwardButtonPressed()
	{
		DialogCreate("DailyChallengeLevelAward").Call("InitDialog", calendar.year, calendar.month, finishNum, calendar._GetDaysInMonth(calendar.month, calendar.year));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPlayButtonState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CalendarSelect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_year", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_month", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_day", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnDailyLevelMonthLoaded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "loadedYear", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "loadedMonth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTodayAwardText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshMonthFinish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AwardButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPlayButtonState && args.Count == 0)
		{
			RefreshPlayButtonState();
			ret = default;
			return true;
		}
		if (method == MethodName.CalendarSelect && args.Count == 3)
		{
			CalendarSelect(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnDailyLevelMonthLoaded && args.Count == 2)
		{
			_OnDailyLevelMonthLoaded(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseButtonPressed && args.Count == 0)
		{
			CloseButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayButtonPressed && args.Count == 0)
		{
			PlayButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTodayAwardText && args.Count == 0)
		{
			RefreshTodayAwardText();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMonthFinish && args.Count == 0)
		{
			RefreshMonthFinish();
			ret = default;
			return true;
		}
		if (method == MethodName.AwardButtonPressed && args.Count == 0)
		{
			AwardButtonPressed();
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.RefreshPlayButtonState)
		{
			return true;
		}
		if (method == MethodName.CalendarSelect)
		{
			return true;
		}
		if (method == MethodName._OnDailyLevelMonthLoaded)
		{
			return true;
		}
		if (method == MethodName.CloseButtonPressed)
		{
			return true;
		}
		if (method == MethodName.PlayButtonPressed)
		{
			return true;
		}
		if (method == MethodName.RefreshTodayAwardText)
		{
			return true;
		}
		if (method == MethodName.RefreshMonthFinish)
		{
			return true;
		}
		if (method == MethodName.AwardButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.finishLabel)
		{
			finishLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.calendar)
		{
			calendar = VariantUtils.ConvertTo<Calendar>(in value);
			return true;
		}
		if (name == PropertyName.playButton)
		{
			playButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.awardLabel)
		{
			awardLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
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
		if (name == PropertyName.day)
		{
			day = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentDate)
		{
			currentDate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			finishNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.finishLabel)
		{
			value = VariantUtils.CreateFrom(in finishLabel);
			return true;
		}
		if (name == PropertyName.calendar)
		{
			value = VariantUtils.CreateFrom(in calendar);
			return true;
		}
		if (name == PropertyName.playButton)
		{
			value = VariantUtils.CreateFrom(in playButton);
			return true;
		}
		if (name == PropertyName.awardLabel)
		{
			value = VariantUtils.CreateFrom(in awardLabel);
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
		if (name == PropertyName.day)
		{
			value = VariantUtils.CreateFrom(in day);
			return true;
		}
		if (name == PropertyName.currentDate)
		{
			value = VariantUtils.CreateFrom(in currentDate);
			return true;
		}
		if (name == PropertyName.finishNum)
		{
			value = VariantUtils.CreateFrom(in finishNum);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.finishLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.calendar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.playButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.year, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.month, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.day, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentDate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.finishNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.finishLabel, Variant.From(in finishLabel));
		info.AddProperty(PropertyName.calendar, Variant.From(in calendar));
		info.AddProperty(PropertyName.playButton, Variant.From(in playButton));
		info.AddProperty(PropertyName.awardLabel, Variant.From(in awardLabel));
		info.AddProperty(PropertyName.year, Variant.From(in year));
		info.AddProperty(PropertyName.month, Variant.From(in month));
		info.AddProperty(PropertyName.day, Variant.From(in day));
		info.AddProperty(PropertyName.currentDate, Variant.From(in currentDate));
		info.AddProperty(PropertyName.finishNum, Variant.From(in finishNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.finishLabel, out var value))
		{
			finishLabel = value.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.calendar, out var value2))
		{
			calendar = value2.As<Calendar>();
		}
		if (info.TryGetProperty(PropertyName.playButton, out var value3))
		{
			playButton = value3.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.awardLabel, out var value4))
		{
			awardLabel = value4.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.year, out var value5))
		{
			year = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.month, out var value6))
		{
			month = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.day, out var value7))
		{
			day = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentDate, out var value8))
		{
			currentDate = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.finishNum, out var value9))
		{
			finishNum = value9.As<int>();
		}
	}
}
