using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxBattleFail/DialogBoxBattleFail.cs")]
public class DialogBoxBattleFail : DialogPopup
{
	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName _OnHostLeft = "_OnHostLeft";

		public static readonly StringName RestartButtonPressed = "RestartButtonPressed";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";

		public static readonly StringName BackBattleButtonPressed = "BackBattleButtonPressed";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
		public static readonly StringName restartButton = "restartButton";

		public static readonly StringName backButton = "backButton";

		public static readonly StringName backBattleButton = "backBattleButton";
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public MainButton restartButton;

	public MainButton backButton;

	public MainButton backBattleButton;

	public override void _Ready()
	{
		base._Ready();
		restartButton = GetNode<MainButton>("%RestartButton");
		backButton = GetNode<MainButton>("%BackButton");
		backBattleButton = GetNode<MainButton>("%BackBattleButton");
		GetNode<BaseButton>("%RestartButton").Pressed += RestartButtonPressed;
		GetNode<BaseButton>("%BackButton").Pressed += BackButtonPressed;
		GetNode<BaseButton>("%BackBattleButton").Pressed += BackBattleButtonPressed;
		if (Global.Instance.enterLevelMode == "OnlineLevel")
		{
			InternetServerManager.Instance.OnlineLevelPost(Global.Instance.enterLevelId, "failure");
			if (Global.Instance.enterLevelIsBattle)
			{
				Global.Instance.enterLevelIsBattleFinish = false;
				backBattleButton.Visible = true;
				restartButton.Visible = false;
				backButton.Visible = false;
			}
		}
		if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			restartButton.Visible = false;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			restartButton.Visible = false;
			backButton.Visible = false;
			backBattleButton.Visible = false;
			textLabel.Clear();
			textLabel.AppendText("[center]游戏结束\n等待房主操作...[/center]");
			MultiPlayerManager.Instance.OnMatchLeft += _OnHostLeft;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (MultiPlayerManager.Instance != null)
		{
			MultiPlayerManager.Instance.OnMatchLeft -= _OnHostLeft;
		}
	}

	public void _OnHostLeft()
	{
		MultiPlayerManager.Instance.SendClientReady();
		SceneManager.Instance.ChangeScene("MainMenu");
	}

	public void RestartButtonPressed()
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.SendMatchState("game_result", MatchStateSerializer.Serialize(new GameResultDto
			{
				leave = true
			}));
		}
		GameSaveManager.Instance.DeleteLevelProgress(TowerDefenseManager.Instance.currentControl.levelConfig.name, TowerDefenseManager.Instance.currentControl.ModLevelIdentity);
		SceneManager.Instance.ReloadScene();
		CloseDialog();
	}

	public void BackButtonPressed()
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
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
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		}
		CloseDialog();
	}

	public void BackBattleButtonPressed()
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.SendMatchState("game_result", MatchStateSerializer.Serialize(new GameResultDto
			{
				leave = true
			}));
		}
		SceneManager.Instance.ChangeScene("LevelEditorStage");
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnHostLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestartButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackBattleButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._OnHostLeft && args.Count == 0)
		{
			_OnHostLeft();
			ret = default;
			return true;
		}
		if (method == MethodName.RestartButtonPressed && args.Count == 0)
		{
			RestartButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.BackBattleButtonPressed && args.Count == 0)
		{
			BackBattleButtonPressed();
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
		if (method == MethodName._OnHostLeft)
		{
			return true;
		}
		if (method == MethodName.RestartButtonPressed)
		{
			return true;
		}
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.BackBattleButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.restartButton)
		{
			restartButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.backButton)
		{
			backButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		if (name == PropertyName.backBattleButton)
		{
			backBattleButton = VariantUtils.ConvertTo<MainButton>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.restartButton)
		{
			value = VariantUtils.CreateFrom(in restartButton);
			return true;
		}
		if (name == PropertyName.backButton)
		{
			value = VariantUtils.CreateFrom(in backButton);
			return true;
		}
		if (name == PropertyName.backBattleButton)
		{
			value = VariantUtils.CreateFrom(in backBattleButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.restartButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backBattleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.restartButton, Variant.From(in restartButton));
		info.AddProperty(PropertyName.backButton, Variant.From(in backButton));
		info.AddProperty(PropertyName.backBattleButton, Variant.From(in backBattleButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.restartButton, out var value))
		{
			restartButton = value.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.backButton, out var value2))
		{
			backButton = value2.As<MainButton>();
		}
		if (info.TryGetProperty(PropertyName.backBattleButton, out var value3))
		{
			backBattleButton = value3.As<MainButton>();
		}
	}
}
