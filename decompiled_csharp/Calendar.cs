using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DailyChallenge/Calendar/Calendar.cs")]
public class Calendar : Control
{
	public delegate void SelectEventHandler(int year, int month, int day);

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupCalendar = "SetupCalendar";

		public static readonly StringName _ClearCalendar = "_ClearCalendar";

		public static readonly StringName _GetCurrentDate = "_GetCurrentDate";

		public static readonly StringName _GenerateWeekdays = "_GenerateWeekdays";

		public static readonly StringName _GenerateDates = "_GenerateDates";

		public static readonly StringName _GetDaysInMonth = "_GetDaysInMonth";

		public static readonly StringName _OnDateSelected = "_OnDateSelected";

		public static readonly StringName _OnPrevMonth = "_OnPrevMonth";

		public static readonly StringName _OnNextMonth = "_OnNextMonth";

		public static readonly StringName _UpdateMonthLabel = "_UpdateMonthLabel";

		public static readonly StringName _GetMonthName = "_GetMonthName";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName panelContainer = "panelContainer";

		public static readonly StringName gridDates = "gridDates";

		public static readonly StringName labelMonthYear = "labelMonthYear";

		public static readonly StringName btnPrevMonth = "btnPrevMonth";

		public static readonly StringName btnNextMonth = "btnNextMonth";

		public static readonly StringName year = "year";

		public static readonly StringName month = "month";

		public static readonly StringName day = "day";

		public static readonly StringName daysInMonth = "daysInMonth";

		public static readonly StringName firstDayOfWeek = "firstDayOfWeek";

		public static readonly StringName buttons = "buttons";

		public static readonly StringName selectedButton = "selectedButton";

		public static readonly StringName today = "today";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static Texture2D _dailyChallengeTodayMarker;

	private static PackedScene _buttonScene;

	private static ButtonGroup _dateButtonGroup;

	private static readonly string[] WEEKDAYS = new string[7] { "日", "一", "二", "三", "四", "五", "六" };

	public PanelContainer panelContainer;

	public GridContainer gridDates;

	public Label labelMonthYear;

	public Button btnPrevMonth;

	public Button btnNextMonth;

	public int year = 2025;

	public int month = 1;

	public int day = 1;

	public int daysInMonth;

	public int firstDayOfWeek;

	public Array buttons = new Array();

	public Button selectedButton;

	public string today = "";

	private static Texture2D DAILY_CHALLENGE_TODAY_MARKER => _dailyChallengeTodayMarker ?? (_dailyChallengeTodayMarker = GD.Load<Texture2D>("uid://cq333hjurbav3"));

	private static PackedScene BUTTON_SCENE => _buttonScene ?? (_buttonScene = GD.Load<PackedScene>("uid://da2qq6lpmnu45"));

	private static ButtonGroup DATE_BUTTON_GROUP => _dateButtonGroup ?? (_dateButtonGroup = GD.Load<ButtonGroup>("uid://bptsnmi53e12v"));

	public event SelectEventHandler OnSelect;

	public override void _Ready()
	{
		panelContainer = GetNode<PanelContainer>("PanelContainer");
		gridDates = GetNode<GridContainer>("PanelContainer/CalendarContainer/Dates");
		labelMonthYear = GetNode<Label>("PanelContainer/CalendarContainer/MonthYearContainer/MonthYear");
		btnPrevMonth = GetNode<Button>("PanelContainer/CalendarContainer/MonthYearContainer/PrevButton");
		btnNextMonth = GetNode<Button>("PanelContainer/CalendarContainer/MonthYearContainer/NextButton");
		_GetCurrentDate();
		btnPrevMonth.Pressed += _OnPrevMonth;
		btnNextMonth.Pressed += _OnNextMonth;
		SetupCalendar();
	}

	public void SetupCalendar()
	{
		_ClearCalendar();
		_GenerateWeekdays();
		_GenerateDates();
		_UpdateMonthLabel();
		OnSelect?.Invoke(year, month, day);
	}

	public void _ClearCalendar()
	{
		foreach (Node child in gridDates.GetChildren())
		{
			child.QueueFree();
		}
		buttons.Clear();
		OnSelect?.Invoke(-1, -1, -1);
	}

	public void _GetCurrentDate()
	{
		Dictionary datetimeDictFromSystem = Time.GetDatetimeDictFromSystem();
		year = (int)datetimeDictFromSystem["year"];
		month = (int)datetimeDictFromSystem["month"];
		day = (int)datetimeDictFromSystem["day"];
	}

	public void _GenerateWeekdays(bool useLetters = false)
	{
		string[] wEEKDAYS = WEEKDAYS;
		foreach (string obj in wEEKDAYS)
		{
			Label label = new Label();
			string text = obj;
			if (useLetters)
			{
				text = text.Substring(0, 1);
			}
			label.SetText(text);
			label.AddThemeColorOverride("font_color", Colors.Black);
			label.AddThemeFontSizeOverride("font_size", 24);
			label.HorizontalAlignment = HorizontalAlignment.Center;
			gridDates.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void _GenerateDates()
	{
		Dictionary datetimeDictFromUnixTime = Time.GetDatetimeDictFromUnixTime(Time.GetUnixTimeFromDatetimeDict(new Dictionary
		{
			["year"] = year,
			["month"] = month,
			["day"] = 1
		}));
		daysInMonth = _GetDaysInMonth(month, year);
		firstDayOfWeek = (int)datetimeDictFromUnixTime["weekday"];
		for (int i = 0; i < firstDayOfWeek; i++)
		{
			Label node = new Label();
			gridDates.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
		Dictionary dictionary = new Dictionary();
		System.Collections.Generic.Dictionary<string, Variant> dAILY_LEVEL_DATA = ResourceManager.Instance.DAILY_LEVEL_DATA;
		if (dAILY_LEVEL_DATA != null && dAILY_LEVEL_DATA.Count > 0 && dAILY_LEVEL_DATA.ContainsKey("LevelDateMap"))
		{
			dictionary = (Dictionary)dAILY_LEVEL_DATA["LevelDateMap"];
		}
		for (int j = 1; j <= daysInMonth; j++)
		{
			DateButton dateButton = BUTTON_SCENE.Instantiate<DateButton>(PackedScene.GenEditState.Disabled);
			dateButton.SetText(j.ToString());
			int capturedDay = j;
			DateButton capturedButton = dateButton;
			dateButton.Pressed += () =>
			{
				_OnDateSelected(capturedDay, capturedButton);
			};
			gridDates.AddChild(dateButton, forceReadableName: false, InternalMode.Disabled);
			buttons.Add(dateButton);
			dateButton.ButtonGroup = DATE_BUTTON_GROUP;
			string text = $"{year}-{month:D2}-{j:D2}";
			dateButton.Disabled = dAILY_LEVEL_DATA == null || dAILY_LEVEL_DATA.Count == 0 || !dAILY_LEVEL_DATA.ContainsKey("LevelDateMap") || !((Dictionary)dAILY_LEVEL_DATA["LevelDateMap"]).ContainsKey(text);
			if (dateButton.Disabled)
			{
				dateButton.FocusMode = FocusModeEnum.None;
			}
			else if (dictionary.ContainsKey(text))
			{
				int num = 0;
				Array array = (Array)dictionary[text];
				foreach (Variant item in array)
				{
					string arg = (string)item;
					string key4 = $"DailyLevel-{text}-{arg}";
					Dictionary dictionary2 = GameSaveManager.Instance.GetLevelValue(key4).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
					if (dictionary2 != null && (int)dictionary2.GetValueOrDefault("Finish", 0) > 0)
					{
						num++;
					}
				}
				if (num >= DailyChallenge.GetRequiredClearCount(dAILY_LEVEL_DATA, text, array.Count))
				{
					dateButton.finishTexture.Visible = true;
				}
			}
			Dictionary datetimeDictFromSystem = Time.GetDatetimeDictFromSystem();
			if (j == (int)datetimeDictFromSystem["day"] && year == (int)datetimeDictFromSystem["year"] && month == (int)datetimeDictFromSystem["month"])
			{
				today = $"{year}-{month:D2}-{j:D2}";
				dateButton.ButtonPressed = true;
				dateButton.Icon = DAILY_CHALLENGE_TODAY_MARKER;
				selectedButton = dateButton;
				selectedButton.AddThemeColorOverride("font_color", selectedButton.GetThemeColor("font_focus_color"));
			}
			else
			{
				dateButton.AddThemeColorOverride("font_hover_pressed_color", Colors.Black);
				dateButton.AddThemeColorOverride("font_hover_color", Colors.Black);
				dateButton.AddThemeColorOverride("font_disabled_color", Colors.Black);
				dateButton.AddThemeColorOverride("font_color", Colors.Black);
				dateButton.AddThemeColorOverride("font_focus_color", Colors.Black);
				dateButton.AddThemeColorOverride("font_pressed_color", Colors.Black);
			}
		}
	}

	public int _GetDaysInMonth(int targetMonth, int targetYear)
	{
		int num = ((targetMonth >= 12) ? 1 : (targetMonth + 1));
		int num2 = ((targetMonth < 12) ? targetYear : (targetYear + 1));
		return (int)Time.GetDatetimeDictFromUnixTime((long)(double)Time.GetUnixTimeFromDatetimeDict(new Dictionary
		{
			["year"] = num2,
			["month"] = num,
			["day"] = 1
		}) - 86400)["day"];
	}

	public void _OnDateSelected(int newDay, Button button)
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		day = newDay;
		OnSelect?.Invoke(year, month, day);
		selectedButton = button;
	}

	public void _OnPrevMonth()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		month--;
		if (month < 1)
		{
			month = 12;
			year--;
		}
		SetupCalendar();
	}

	public void _OnNextMonth()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		month++;
		if (month > 12)
		{
			month = 1;
			year++;
		}
		SetupCalendar();
	}

	public void _UpdateMonthLabel()
	{
		labelMonthYear.SetText($"{year}/{_GetMonthName(month)} ");
	}

	public string _GetMonthName(int m)
	{
		return (new string[12]
		{
			"一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月",
			"十一月", "十二月"
		})[m - 1];
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupCalendar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ClearCalendar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetCurrentDate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GenerateWeekdays, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "useLetters", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GenerateDates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetDaysInMonth, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "targetMonth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetYear", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnDateSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "newDay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false)
			}, null),
			new MethodInfo(MethodName._OnPrevMonth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnNextMonth, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._UpdateMonthLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetMonthName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "m", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetupCalendar && args.Count == 0)
		{
			SetupCalendar();
			ret = default;
			return true;
		}
		if (method == MethodName._ClearCalendar && args.Count == 0)
		{
			_ClearCalendar();
			ret = default;
			return true;
		}
		if (method == MethodName._GetCurrentDate && args.Count == 0)
		{
			_GetCurrentDate();
			ret = default;
			return true;
		}
		if (method == MethodName._GenerateWeekdays && args.Count == 1)
		{
			_GenerateWeekdays(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._GenerateDates && args.Count == 0)
		{
			_GenerateDates();
			ret = default;
			return true;
		}
		if (method == MethodName._GetDaysInMonth && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(_GetDaysInMonth(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName._OnDateSelected && args.Count == 2)
		{
			_OnDateSelected(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Button>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnPrevMonth && args.Count == 0)
		{
			_OnPrevMonth();
			ret = default;
			return true;
		}
		if (method == MethodName._OnNextMonth && args.Count == 0)
		{
			_OnNextMonth();
			ret = default;
			return true;
		}
		if (method == MethodName._UpdateMonthLabel && args.Count == 0)
		{
			_UpdateMonthLabel();
			ret = default;
			return true;
		}
		if (method == MethodName._GetMonthName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(_GetMonthName(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.SetupCalendar)
		{
			return true;
		}
		if (method == MethodName._ClearCalendar)
		{
			return true;
		}
		if (method == MethodName._GetCurrentDate)
		{
			return true;
		}
		if (method == MethodName._GenerateWeekdays)
		{
			return true;
		}
		if (method == MethodName._GenerateDates)
		{
			return true;
		}
		if (method == MethodName._GetDaysInMonth)
		{
			return true;
		}
		if (method == MethodName._OnDateSelected)
		{
			return true;
		}
		if (method == MethodName._OnPrevMonth)
		{
			return true;
		}
		if (method == MethodName._OnNextMonth)
		{
			return true;
		}
		if (method == MethodName._UpdateMonthLabel)
		{
			return true;
		}
		if (method == MethodName._GetMonthName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.panelContainer)
		{
			panelContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName.gridDates)
		{
			gridDates = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName.labelMonthYear)
		{
			labelMonthYear = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.btnPrevMonth)
		{
			btnPrevMonth = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.btnNextMonth)
		{
			btnNextMonth = VariantUtils.ConvertTo<Button>(in value);
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
		if (name == PropertyName.daysInMonth)
		{
			daysInMonth = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.firstDayOfWeek)
		{
			firstDayOfWeek = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.buttons)
		{
			buttons = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.selectedButton)
		{
			selectedButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.today)
		{
			today = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.panelContainer)
		{
			value = VariantUtils.CreateFrom(in panelContainer);
			return true;
		}
		if (name == PropertyName.gridDates)
		{
			value = VariantUtils.CreateFrom(in gridDates);
			return true;
		}
		if (name == PropertyName.labelMonthYear)
		{
			value = VariantUtils.CreateFrom(in labelMonthYear);
			return true;
		}
		if (name == PropertyName.btnPrevMonth)
		{
			value = VariantUtils.CreateFrom(in btnPrevMonth);
			return true;
		}
		if (name == PropertyName.btnNextMonth)
		{
			value = VariantUtils.CreateFrom(in btnNextMonth);
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
		if (name == PropertyName.daysInMonth)
		{
			value = VariantUtils.CreateFrom(in daysInMonth);
			return true;
		}
		if (name == PropertyName.firstDayOfWeek)
		{
			value = VariantUtils.CreateFrom(in firstDayOfWeek);
			return true;
		}
		if (name == PropertyName.buttons)
		{
			value = VariantUtils.CreateFrom(in buttons);
			return true;
		}
		if (name == PropertyName.selectedButton)
		{
			value = VariantUtils.CreateFrom(in selectedButton);
			return true;
		}
		if (name == PropertyName.today)
		{
			value = VariantUtils.CreateFrom(in today);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.panelContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.gridDates, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.labelMonthYear, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.btnPrevMonth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.btnNextMonth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.year, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.month, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.day, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.daysInMonth, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.firstDayOfWeek, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.buttons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.selectedButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.today, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.panelContainer, Variant.From(in panelContainer));
		info.AddProperty(PropertyName.gridDates, Variant.From(in gridDates));
		info.AddProperty(PropertyName.labelMonthYear, Variant.From(in labelMonthYear));
		info.AddProperty(PropertyName.btnPrevMonth, Variant.From(in btnPrevMonth));
		info.AddProperty(PropertyName.btnNextMonth, Variant.From(in btnNextMonth));
		info.AddProperty(PropertyName.year, Variant.From(in year));
		info.AddProperty(PropertyName.month, Variant.From(in month));
		info.AddProperty(PropertyName.day, Variant.From(in day));
		info.AddProperty(PropertyName.daysInMonth, Variant.From(in daysInMonth));
		info.AddProperty(PropertyName.firstDayOfWeek, Variant.From(in firstDayOfWeek));
		info.AddProperty(PropertyName.buttons, Variant.From(in buttons));
		info.AddProperty(PropertyName.selectedButton, Variant.From(in selectedButton));
		info.AddProperty(PropertyName.today, Variant.From(in today));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.panelContainer, out var value))
		{
			panelContainer = value.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName.gridDates, out var value2))
		{
			gridDates = value2.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName.labelMonthYear, out var value3))
		{
			labelMonthYear = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.btnPrevMonth, out var value4))
		{
			btnPrevMonth = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.btnNextMonth, out var value5))
		{
			btnNextMonth = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.year, out var value6))
		{
			year = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.month, out var value7))
		{
			month = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.day, out var value8))
		{
			day = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.daysInMonth, out var value9))
		{
			daysInMonth = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.firstDayOfWeek, out var value10))
		{
			firstDayOfWeek = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.buttons, out var value11))
		{
			buttons = value11.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.selectedButton, out var value12))
		{
			selectedButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.today, out var value13))
		{
			today = value13.As<string>();
		}
	}
}
