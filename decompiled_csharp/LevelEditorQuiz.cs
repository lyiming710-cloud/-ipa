using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/LevelEditor/Quiz/LevelEditorQuiz.cs")]
public class LevelEditorQuiz : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName Enter = "Enter";

		public static readonly StringName MapChooseButtonPressed = "MapChooseButtonPressed";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _quizDragMenu = "_quizDragMenu";

		public static readonly StringName _mapChooseButton = "_mapChooseButton";

		public static readonly StringName _quizMapChooseRect = "_quizMapChooseRect";

		public static readonly StringName mapChooseReady = "mapChooseReady";

		public static readonly StringName mapChooseOver = "mapChooseOver";

		public static readonly StringName currentChoose = "currentChoose";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string DisabledSuperBigMapName = "FrontlawnSuperBig";

	private const string DisabledSuperBigNightMapName = "FrontlawnSuperBigNight";

	private static PackedScene _levelEditorQuizLevelItem;

	private DragMenu _quizDragMenu;

	private MainButton _mapChooseButton;

	private ReferenceRect _quizMapChooseRect;

	public bool mapChooseReady;

	public bool mapChooseOver;

	public double currentChoose;

	private static PackedScene LEVEL_EDITOR_QUIZ_LEVEL_ITEM => _levelEditorQuizLevelItem ?? (_levelEditorQuizLevelItem = GD.Load<PackedScene>("uid://3rogaxqqkxn2"));

	public override void _Ready()
	{
		_quizDragMenu = GetNode<DragMenu>("%QuizDragMenu");
		_mapChooseButton = GetNode<MainButton>("%MapChooseButton");
		_quizMapChooseRect = GetNode<ReferenceRect>("%QuizMapChooseRect");
		_mapChooseButton.Pressed += MapChooseButtonPressed;
		foreach (string key in ResourceManager.Instance.MAPS.Keys)
		{
			if (!(key == "FrontlawnSuperBig") && !(key == "FrontlawnSuperBigNight"))
			{
				LevelEditorQuizLevelItem levelEditorQuizLevelItem = LEVEL_EDITOR_QUIZ_LEVEL_ITEM.Instantiate<LevelEditorQuizLevelItem>(PackedScene.GenEditState.Disabled);
				_quizDragMenu.AddChild(levelEditorQuizLevelItem, forceReadableName: false, InternalMode.Disabled);
				levelEditorQuizLevelItem.Init(key);
			}
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (mapChooseReady)
		{
			currentChoose = Mathf.PosMod(currentChoose + delta * (double)GD.Randf() * 50.0, _quizDragMenu.GetChildCount());
			_quizDragMenu.currentIndex = (int)Mathf.Floor(currentChoose);
		}
	}

	public void Enter()
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = GD.Load<TowerDefenseLevelConfig>("uid://ccdkb1f5papjm").Duplicate(deep: true) as TowerDefenseLevelConfig;
		towerDefenseLevelConfig.Init();
		LevelEditorQuizLevelItem levelEditorQuizLevelItem = _quizDragMenu.GetChild(_quizDragMenu.currentIndex) as LevelEditorQuizLevelItem;
		towerDefenseLevelConfig.map = levelEditorQuizLevelItem.map;
		towerDefenseLevelConfig.featureData["Map"]["MapName"] = towerDefenseLevelConfig.map;
		TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelConfig;
		SceneManager.Instance.ChangeScene("TowerDefense");
	}

	public async void MapChooseButtonPressed()
	{
		_mapChooseButton.Visible = false;
		_quizMapChooseRect.Visible = true;
		_quizDragMenu.alive = false;
		mapChooseOver = true;
		mapChooseReady = true;
		await ToSignal(GetTree().CreateTimer(GD.RandRange(3.0, 5.0), processAlways: false), SceneTreeTimer.SignalName.Timeout);
		mapChooseReady = false;
		await ToSignal(GetTree().CreateTimer(1.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		Enter();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Enter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MapChooseButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Enter && args.Count == 0)
		{
			Enter();
			ret = default;
			return true;
		}
		if (method == MethodName.MapChooseButtonPressed && args.Count == 0)
		{
			MapChooseButtonPressed();
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.Enter)
		{
			return true;
		}
		if (method == MethodName.MapChooseButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._quizDragMenu)
		{
			_quizDragMenu = VariantUtils.ConvertTo<DragMenu>(in value);
			return true;
		}
		if (name == PropertyName._mapChooseButton)
		{
			_mapChooseButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName._quizMapChooseRect)
		{
			_quizMapChooseRect = VariantUtils.ConvertTo<ReferenceRect>(in value);
			return true;
		}
		if (name == PropertyName.mapChooseReady)
		{
			mapChooseReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mapChooseOver)
		{
			mapChooseOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.currentChoose)
		{
			currentChoose = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._quizDragMenu)
		{
			value = VariantUtils.CreateFrom(in _quizDragMenu);
			return true;
		}
		if (name == PropertyName._mapChooseButton)
		{
			value = VariantUtils.CreateFrom(in _mapChooseButton);
			return true;
		}
		if (name == PropertyName._quizMapChooseRect)
		{
			value = VariantUtils.CreateFrom(in _quizMapChooseRect);
			return true;
		}
		if (name == PropertyName.mapChooseReady)
		{
			value = VariantUtils.CreateFrom(in mapChooseReady);
			return true;
		}
		if (name == PropertyName.mapChooseOver)
		{
			value = VariantUtils.CreateFrom(in mapChooseOver);
			return true;
		}
		if (name == PropertyName.currentChoose)
		{
			value = VariantUtils.CreateFrom(in currentChoose);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._quizDragMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapChooseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quizMapChooseRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mapChooseReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mapChooseOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentChoose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._quizDragMenu, Variant.From(in _quizDragMenu));
		info.AddProperty(PropertyName._mapChooseButton, Variant.From(in _mapChooseButton));
		info.AddProperty(PropertyName._quizMapChooseRect, Variant.From(in _quizMapChooseRect));
		info.AddProperty(PropertyName.mapChooseReady, Variant.From(in mapChooseReady));
		info.AddProperty(PropertyName.mapChooseOver, Variant.From(in mapChooseOver));
		info.AddProperty(PropertyName.currentChoose, Variant.From(in currentChoose));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._quizDragMenu, out var value))
		{
			_quizDragMenu = value.As<DragMenu>();
		}
		if (info.TryGetProperty(PropertyName._mapChooseButton, out var value2))
		{
			_mapChooseButton = value2.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName._quizMapChooseRect, out var value3))
		{
			_quizMapChooseRect = value3.As<ReferenceRect>();
		}
		if (info.TryGetProperty(PropertyName.mapChooseReady, out var value4))
		{
			mapChooseReady = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mapChooseOver, out var value5))
		{
			mapChooseOver = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.currentChoose, out var value6))
		{
			currentChoose = value6.As<double>();
		}
	}
}
