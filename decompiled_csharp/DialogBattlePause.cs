using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/MenuDialog/DialogBattlePause/DialogBattlePause.cs")]
public class DialogBattlePause : MenuDialogBase
{
	public new class MethodName : MenuDialogBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName HandbookButtonPressed = "HandbookButtonPressed";

		public static readonly StringName RestartButtonPressed = "RestartButtonPressed";

		public static readonly StringName MainMenuButtonPressed = "MainMenuButtonPressed";

		public static readonly StringName LevelEditorButtonPressed = "LevelEditorButtonPressed";

		public new static readonly StringName _Input = "_Input";
	}

	public new class PropertyName : MenuDialogBase.PropertyName
	{
		public static readonly StringName handbookButton = "handbookButton";

		public static readonly StringName restartButton = "restartButton";

		public static readonly StringName mainMenuButton = "mainMenuButton";

		public static readonly StringName levelEditorButton = "levelEditorButton";
	}

	public new class SignalName : MenuDialogBase.SignalName
	{
	}

	private MainButton handbookButton;

	private MainButton restartButton;

	private MainButton mainMenuButton;

	private MainButton levelEditorButton;

	public override void _Ready()
	{
		handbookButton = GetNode<MainButton>("%HandbookButton");
		restartButton = GetNode<MainButton>("%RestartButton");
		mainMenuButton = GetNode<MainButton>("%MainMenuButton");
		levelEditorButton = GetNode<MainButton>("%LevelEditorButton");
		GetNode<BaseButton>("%HandbookButton").Pressed += HandbookButtonPressed;
		GetNode<BaseButton>("%LevelEditorButton").Pressed += LevelEditorButtonPressed;
		GetNode<BaseButton>("%RestartButton").Pressed += RestartButtonPressed;
		GetNode<BaseButton>("%MainMenuButton").Pressed += MainMenuButtonPressed;
		base._Ready();
		if (Global.Instance.isEditor)
		{
			levelEditorButton.Visible = true;
			handbookButton.Visible = false;
			mainMenuButton.Visible = false;
		}
		if (Global.Instance.enterLevelIsBattle)
		{
			restartButton.Visible = false;
			mainMenuButton.Visible = false;
		}
		if (TowerDefenseManager.GetGameMethod() == TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ && TowerDefenseManager.Instance.IsGameRunning())
		{
			restartButton.Visible = false;
		}
		if (Global.Instance.isMultiplayerMode)
		{
			pasue = false;
			if (GetTree().Paused)
			{
				GetTree().Paused = false;
			}
			restartButton.Visible = false;
			mainMenuButton.Visible = false;
			levelEditorButton.Visible = false;
			if (!MultiPlayerManager.Instance.isHost)
			{
				handbookButton.Visible = false;
			}
		}
	}

	public void HandbookButtonPressed()
	{
		if (!Global.Instance.isMultiplayerMode)
		{
			GameSaveManager.Instance.SaveLevelProgress(TowerDefenseManager.Instance.currentControl.levelConfig.name, TowerDefenseManager.Instance.currentControl.ModLevelIdentity);
		}
		if (Global.Instance.isMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.SendMatchState("game_result", MatchStateSerializer.Serialize(new GameResultDto
			{
				leave = true
			}));
		}
		switch (Global.Instance.enterLevelMode)
		{
		case "ModLevel":
		case "LevelChoose":
			if (Global.Instance.currentLevelChoose != "TryLevel")
			{
				SceneManager.Instance.ChangeScene("LevelChoose");
			}
			else
			{
				SceneManager.Instance.ChangeScene("MainMenu");
			}
			break;
		case "DailyLevel":
			SceneManager.Instance.ChangeScene("MainMenu");
			break;
		case "DiyLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		case "LoadLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		case "OnlineLevel":
			InternetServerManager.Instance.OnlineLevelPost(Global.Instance.enterLevelId);
			if (Global.Instance.enterLevelIsBattle)
			{
				Global.Instance.enterLevelIsBattleFinish = false;
			}
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		}
	}

	public void RestartButtonPressed()
	{
		DialogCreate("ReStart");
	}

	public void MainMenuButtonPressed()
	{
		if (!Global.Instance.isMultiplayerMode)
		{
			GameSaveManager.Instance.SaveLevelProgress(TowerDefenseManager.Instance.currentControl.levelConfig.name, TowerDefenseManager.Instance.currentControl.ModLevelIdentity);
		}
		if (Global.Instance.enterLevelMode == "OnlineLevel")
		{
			InternetServerManager.Instance.OnlineLevelPost(Global.Instance.enterLevelId);
		}
		SceneManager.Instance.ChangeScene("MainMenu");
	}

	public void LevelEditorButtonPressed()
	{
		if (!Global.Instance.isMultiplayerMode)
		{
			GameSaveManager.Instance.SaveLevelProgress(TowerDefenseManager.Instance.currentControl.levelConfig.name, TowerDefenseManager.Instance.currentControl.ModLevelIdentity);
		}
		if (Global.Instance.enterLevelMode == "OnlineLevel")
		{
			InternetServerManager.Instance.OnlineLevelPost(Global.Instance.enterLevelId);
		}
		SceneManager.Instance.ChangeScene("LevelEditorStage");
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!IsQueuedForDeletion() && inputEvent.IsActionPressed("Pause"))
		{
			GetViewport().SetInputAsHandled();
			CloseDialog();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandbookButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestartButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MainMenuButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LevelEditorButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
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
		if (method == MethodName.HandbookButtonPressed && args.Count == 0)
		{
			HandbookButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.RestartButtonPressed && args.Count == 0)
		{
			RestartButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.MainMenuButtonPressed && args.Count == 0)
		{
			MainMenuButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.LevelEditorButtonPressed && args.Count == 0)
		{
			LevelEditorButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
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
		if (method == MethodName.HandbookButtonPressed)
		{
			return true;
		}
		if (method == MethodName.RestartButtonPressed)
		{
			return true;
		}
		if (method == MethodName.MainMenuButtonPressed)
		{
			return true;
		}
		if (method == MethodName.LevelEditorButtonPressed)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.handbookButton)
		{
			handbookButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.restartButton)
		{
			restartButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.mainMenuButton)
		{
			mainMenuButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.levelEditorButton)
		{
			levelEditorButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.handbookButton)
		{
			value = VariantUtils.CreateFrom(in handbookButton);
			return true;
		}
		if (name == PropertyName.restartButton)
		{
			value = VariantUtils.CreateFrom(in restartButton);
			return true;
		}
		if (name == PropertyName.mainMenuButton)
		{
			value = VariantUtils.CreateFrom(in mainMenuButton);
			return true;
		}
		if (name == PropertyName.levelEditorButton)
		{
			value = VariantUtils.CreateFrom(in levelEditorButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.handbookButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.restartButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mainMenuButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelEditorButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.handbookButton, Variant.From(in handbookButton));
		info.AddProperty(PropertyName.restartButton, Variant.From(in restartButton));
		info.AddProperty(PropertyName.mainMenuButton, Variant.From(in mainMenuButton));
		info.AddProperty(PropertyName.levelEditorButton, Variant.From(in levelEditorButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.handbookButton, out var value))
		{
			handbookButton = value.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.restartButton, out var value2))
		{
			restartButton = value2.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.mainMenuButton, out var value3))
		{
			mainMenuButton = value3.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.levelEditorButton, out var value4))
		{
			levelEditorButton = value4.As<MainButton>();
		}
	}
}
