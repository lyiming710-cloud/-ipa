using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Quiz/TowerDefenseBattleProcessQuiz.cs")]
public class TowerDefenseBattleProcessQuiz : TowerDefenseBattleProcess
{
	private sealed class LineButtonBinding
	{
		public MainButton Button { get; }

		public Action Handler { get; }

		public LineButtonBinding(MainButton button, Action handler)
		{
			Button = button;
			Handler = handler;
		}
	}

	public new class MethodName : TowerDefenseBattleProcess.MethodName
	{
		public new static readonly StringName CanLoadProgress = "CanLoadProgress";

		public new static readonly StringName OnReady = "OnReady";

		public static readonly StringName SetupUI = "SetupUI";

		public static readonly StringName CreateUI = "CreateUI";

		public new static readonly StringName Init = "Init";

		public static readonly StringName StartQuiz = "StartQuiz";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName ClearCell = "ClearCell";

		public static readonly StringName CreatePresentBox = "CreatePresentBox";

		public static readonly StringName CreateZombieVase = "CreateZombieVase";

		public static readonly StringName ChangePresentBox = "ChangePresentBox";

		public static readonly StringName ChangePresentBoxButtonPressed = "ChangePresentBoxButtonPressed";

		public static readonly StringName RunButtonPressed = "RunButtonPressed";

		public static readonly StringName ConfirmPendingRun = "ConfirmPendingRun";

		public static readonly StringName DisconnectPendingRunDialog = "DisconnectPendingRunDialog";

		public static readonly StringName RemoveLineButtonBinding = "RemoveLineButtonBinding";

		public static readonly StringName DestroyQuizUI = "DestroyQuizUI";

		public static readonly StringName Run = "Run";

		public new static readonly StringName CheckFinal = "CheckFinal";

		public new static readonly StringName CheckFail = "CheckFail";

		public static readonly StringName Final = "Final";

		public static readonly StringName CreateCoin = "CreateCoin";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public new static readonly StringName Finish = "Finish";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName SaveProcess = "SaveProcess";

		public new static readonly StringName LoadProcess = "LoadProcess";

		public new static readonly StringName PhysicsProcess = "PhysicsProcess";
	}

	public new class PropertyName : TowerDefenseBattleProcess.PropertyName
	{
		public static readonly StringName stripPos = "stripPos";

		public static readonly StringName running = "running";

		public static readonly StringName multiple = "multiple";

		public static readonly StringName over = "over";

		public static readonly StringName useCoin = "useCoin";

		public static readonly StringName config = "config";

		public static readonly StringName quizStarted = "quizStarted";

		public static readonly StringName quizUI = "quizUI";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName progressFeature = "progressFeature";

		public static readonly StringName brainFeature = "brainFeature";

		public static readonly StringName _pendingRunDialog = "_pendingRunDialog";

		public static readonly StringName _pendingRunCost = "_pendingRunCost";
	}

	public new class SignalName : TowerDefenseBattleProcess.SignalName
	{
	}

	private static PackedScene _quizControlScene;

	private static PackedScene _changePresentBoxButtonScene;

	private static PackedScene _betPanelScene;

	public int stripPos = 5;

	public bool running;

	public List<TowerDefenseCharacter> vaseList = new List<TowerDefenseCharacter>();

	public int multiple = 1;

	public bool over;

	public int useCoin;

	public TowerDefenseBattleProcessQuizConfig config;

	public bool quizStarted;

	public QuizControl quizUI;

	public TowerDefenseBattleFeatureMap mapFeature;

	public TowerDefenseBattleFeatureProgress progressFeature;

	public TowerDefenseBattleFeatureBrain brainFeature;

	private readonly List<LineButtonBinding> _lineButtonBindings = new List<LineButtonBinding>();

	private DialogBoxChoose _pendingRunDialog;

	private DialogBoxChoose.ChooseTrueEventHandler _pendingRunHandler;

	private int _pendingRunCost;

	private static PackedScene QuizControlScene => _quizControlScene ?? (_quizControlScene = GD.Load<PackedScene>("uid://dlywvjp6hpbgu"));

	private static PackedScene ChangePresentBoxButtonScene => _changePresentBoxButtonScene ?? (_changePresentBoxButtonScene = GD.Load<PackedScene>("uid://bjtvgkckq2ft5"));

	private static PackedScene BetPanelScene => _betPanelScene ?? (_betPanelScene = GD.Load<PackedScene>("uid://knp6uvl6ja3d"));

	public override bool CanLoadProgress()
	{
		return false;
	}

	public override void OnReady()
	{
	}

	public void SetupUI()
	{
		RunLifetimeTask(SetupUIAsync, "SetupUI");
	}

	private async Task SetupUIAsync()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (IsLifetimeActive && GodotObject.IsInstanceValid(progressFeature) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			string levelName = Tr(TowerDefenseManager.Instance.currentLevelConfig.levelName).Replace("{LevelNumber}", TowerDefenseManager.Instance.currentLevelConfig.levelNumber.ToString());
			progressFeature.SetLevelName(levelName);
			progressFeature.SetDifficultVisible(visible: false);
		}
	}

	public override Task GameInit()
	{
		CreateUI();
		SetupUI();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		CreateUI();
		SetupUI();
		return Task.CompletedTask;
	}

	public void CreateUI()
	{
		DestroyQuizUI();
		quizUI = QuizControlScene.Instantiate<QuizControl>(PackedScene.GenEditState.Disabled);
		control.AddUI(quizUI, 4);
		quizUI.OnRunButtonPressed += RunButtonPressed;
		TowerDefenseManager.Instance.coinBank.ShowCoinBank(new Vector2(250f, 557f), still: true);
		quizUI.Visible = true;
		quizUI.startGUINode.Visible = true;
		Vector2I gridNum = mapFeature.config.gridNum;
		for (int i = 1; i <= gridNum.Y; i++)
		{
			MainButton button = ChangePresentBoxButtonScene.Instantiate<MainButton>(PackedScene.GenEditState.Disabled);
			button.GlobalPosition = new Vector2((float)TowerDefenseManager.Instance.GetMapGroundRight(), TowerDefenseManager.Instance.GetMapCellPos(new Vector2I(0, i)).Y + 20f);
			quizUI.changePresentBoxButtonNode.AddChild(button, forceReadableName: false, Node.InternalMode.Disabled);
			int lineY = i;
			Action action = () =>
			{
				ChangePresentBoxButtonPressed(lineY, button);
			};
			button.Pressed += action;
			_lineButtonBindings.Add(new LineButtonBinding(button, action));
			BetPanel betPanel = BetPanelScene.Instantiate<BetPanel>(PackedScene.GenEditState.Disabled);
			betPanel.GlobalPosition = new Vector2(0f, TowerDefenseManager.Instance.GetMapCellPos(new Vector2I(0, i)).Y + 30f);
			quizUI.betPanelNode.AddChild(betPanel, forceReadableName: false, Node.InternalMode.Disabled);
			betPanel.Init(i);
		}
	}

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseBattleProcessQuizConfig();
		config.Init(data);
		mapFeature = GetFeature("Map") as TowerDefenseBattleFeatureMap;
		progressFeature = GetFeature("Progress") as TowerDefenseBattleFeatureProgress;
		brainFeature = GetFeature("Brain") as TowerDefenseBattleFeatureBrain;
	}

	public void StartQuiz()
	{
		if (quizStarted)
		{
			return;
		}
		quizStarted = true;
		Vector2I gridNum = mapFeature.config.gridNum;
		stripPos = config.ResolveStripColumn(gridNum.X);
		mapFeature.currentMap.UseStripe(stripPos);
		if (brainFeature != null)
		{
			brainFeature.BrainInit();
		}
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				Vector2I gridPos = new Vector2I(i, j);
				if (i <= stripPos)
				{
					CreatePresentBox(gridPos);
				}
				else
				{
					CreateZombieVase(gridPos);
				}
			}
		}
	}

	public void Refresh()
	{
		RunLifetimeTask(RefreshAsync, "Refresh");
	}

	private async Task RefreshAsync()
	{
		if (quizUI == null)
		{
			return;
		}
		TowerDefenseManager.Instance.currentControl.isGameRunning = false;
		useCoin = 0;
		running = false;
		over = false;
		multiple = 1;
		quizStarted = false;
		quizUI.coinLabel.Text = $"金币倍数：{multiple}倍";
		vaseList.Clear();
		Vector2I gridNum = mapFeature.config.gridNum;
		stripPos = config.ResolveStripColumn(gridNum.X);
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				Vector2I gridPos = new Vector2I(i, j);
				ClearCell(gridPos);
			}
		}
		if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
		{
			ProjectileUpdateManager.Instance.Clear();
		}
		foreach (Node item in GetTree().GetNodesInGroup("Projectile"))
		{
			item.QueueFree();
		}
		foreach (Variant item2 in TowerDefenseManager.Instance.GetCharacter())
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item2;
			towerDefenseCharacter.skipDestroySet = true;
			TowerDefenseManager.Instance.CharacterUnregister(towerDefenseCharacter);
			towerDefenseCharacter.RemoveFromGroup("Character");
			towerDefenseCharacter.QueueFree();
		}
		foreach (Variant iceCap in mapFeature.iceCapList)
		{
			Node node = (Node)(GodotObject)iceCap;
			if (GodotObject.IsInstanceValid(node))
			{
				node.QueueFree();
			}
		}
		foreach (Node item3 in GetTree().GetNodesInGroup("Brain"))
		{
			item3.QueueFree();
		}
		DestroyQuizUI();
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (IsLifetimeActive)
		{
			CreateUI();
			StartQuiz();
		}
	}

	public void ClearCell(Vector2I gridPos)
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter character in mapCell.characterList)
		{
			list.Add(character);
		}
		foreach (TowerDefenseCharacter item in list)
		{
			item.skipDestroySet = true;
		}
		mapCell.Clear();
	}

	public void CreatePresentBox(Vector2I gridPos, bool open = true)
	{
		RunLifetimeTask(() => CreatePresentBoxAsync(gridPos, open), "CreatePresentBox");
	}

	private async Task CreatePresentBoxAsync(Vector2I gridPos, bool open)
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		if (GodotObject.IsInstanceValid(mapCell))
		{
			if (mapCell.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.BRICK) || mapCell.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.SOIL))
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(config.potPacketName);
				if (GodotObject.IsInstanceValid(packetConfig))
				{
					packetConfig.Plant(gridPos);
				}
			}
			if (mapCell.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER))
			{
				TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig(config.lilyPadPacketName);
				if (GodotObject.IsInstanceValid(packetConfig2))
				{
					packetConfig2.Plant(gridPos);
				}
			}
		}
		TowerDefensePacketConfig packetConfig3 = TowerDefenseManager.GetPacketConfig(config.presentBoxPacketName);
		if (!GodotObject.IsInstanceValid(packetConfig3))
		{
			return;
		}
		TowerDefenseCharacter presentBox = packetConfig3.Plant(gridPos);
		if (!GodotObject.IsInstanceValid(presentBox))
		{
			return;
		}
		presentBox.AddToGroup("PresentBox");
		presentBox.ProcessMode = Node.ProcessModeEnum.Disabled;
		presentBox.Set("packetBank", config.presentBoxPacketBank);
		if (open)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			if (IsLifetimeActive && GodotObject.IsInstanceValid(presentBox))
			{
				presentBox.ProcessMode = Node.ProcessModeEnum.Inherit;
				presentBox.instance.invincible = true;
				presentBox.sprite.SetAnimation("Open", loop: false);
			}
		}
	}

	public void CreateZombieVase(Vector2I gridPos)
	{
		RunLifetimeTask(() => CreateZombieVaseAsync(gridPos), "CreateZombieVase");
	}

	private async Task CreateZombieVaseAsync(Vector2I gridPos)
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(config.zombieVasePacketName);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return;
		}
		TowerDefenseCharacter zombieVase = packetConfig.Plant(gridPos);
		if (zombieVase == null)
		{
			return;
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (IsLifetimeActive && GodotObject.IsInstanceValid(zombieVase))
		{
			IStateMachineController stateMachine = zombieVase.StateMachine;
			if (stateMachine != null && stateMachine.IsInitialized)
			{
				zombieVase.SetMainStateMachineDispatchEnabled(enabled: true);
				zombieVase.Set("packetBank", config.zombieVasePacketBank);
				vaseList.Add(zombieVase);
			}
		}
	}

	public void ChangePresentBox(int line)
	{
		RunLifetimeTask(() => ChangePresentBoxAsync(line), "ChangePresentBox");
	}

	private async Task ChangePresentBoxAsync(int line)
	{
		for (int x = 1; x <= stripPos; x++)
		{
			Vector2I gridPos = new Vector2I(x, line);
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
			List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
			foreach (TowerDefenseCharacter character in mapCell.characterList)
			{
				list.Add(character);
			}
			foreach (TowerDefenseCharacter item in list)
			{
				item.skipDestroySet = true;
			}
			mapCell.Clear();
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			if (!IsLifetimeActive)
			{
				break;
			}
			CreatePresentBox(gridPos, open: false);
		}
	}

	public void ChangePresentBoxButtonPressed(int line, MainButton button)
	{
		if (quizUI == null)
		{
			return;
		}
		int num = 0;
		foreach (Node child in quizUI.betPanelNode.GetChildren())
		{
			if (child is BetPanel betPanel)
			{
				num += (int)betPanel.betCoinSpinBox.Value;
			}
		}
		if (TowerDefenseManager.Instance.GetCoin() < num * (multiple + 2))
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", $"[center][font_size=24]您的金币不足[/font_size][/center]\n[center][font_size=24]至少需要{num * (multiple + 2)}金币[/font_size][/center]");
			return;
		}
		ChangePresentBox(line);
		RemoveLineButtonBinding(button);
		button.QueueFree();
		multiple++;
		quizUI.coinLabel.Text = $"金币倍数：{multiple}倍";
	}

	public void RunButtonPressed()
	{
		if (quizUI == null)
		{
			return;
		}
		int num = 0;
		foreach (Node child in quizUI.betPanelNode.GetChildren())
		{
			if (child is BetPanel betPanel)
			{
				num += (int)betPanel.betCoinSpinBox.Value;
			}
		}
		if (TowerDefenseManager.Instance.GetCoin() < num * (multiple + 1))
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", $"[center][font_size=24]您的金币不足[/font_size][/center]\n[center][font_size=24]至少需要{num * (multiple + 1)}金币[/font_size][/center]");
			return;
		}
		DisconnectPendingRunDialog();
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxChoose");
		dialogBoxBase.Set("text", $"[center][font_size=24]是否使用{num * multiple}金币入场[/font_size][/center]");
		_pendingRunDialog = dialogBoxBase as DialogBoxChoose;
		if (GodotObject.IsInstanceValid(_pendingRunDialog))
		{
			_pendingRunCost = num * multiple;
			_pendingRunHandler = ConfirmPendingRun;
			_pendingRunDialog.OnChooseTrue += _pendingRunHandler;
		}
	}

	private void ConfirmPendingRun()
	{
		int pendingRunCost = _pendingRunCost;
		DisconnectPendingRunDialog();
		Run(pendingRunCost);
	}

	private void DisconnectPendingRunDialog()
	{
		if (GodotObject.IsInstanceValid(_pendingRunDialog) && _pendingRunHandler != null)
		{
			_pendingRunDialog.OnChooseTrue -= _pendingRunHandler;
		}
		_pendingRunDialog = null;
		_pendingRunHandler = null;
		_pendingRunCost = 0;
	}

	private void RemoveLineButtonBinding(MainButton button)
	{
		for (int num = _lineButtonBindings.Count - 1; num >= 0; num--)
		{
			LineButtonBinding lineButtonBinding = _lineButtonBindings[num];
			if (lineButtonBinding.Button == button)
			{
				if (GodotObject.IsInstanceValid(lineButtonBinding.Button))
				{
					lineButtonBinding.Button.Pressed -= lineButtonBinding.Handler;
				}
				_lineButtonBindings.RemoveAt(num);
			}
		}
	}

	private void DestroyQuizUI()
	{
		DisconnectPendingRunDialog();
		foreach (LineButtonBinding lineButtonBinding in _lineButtonBindings)
		{
			if (GodotObject.IsInstanceValid(lineButtonBinding.Button))
			{
				lineButtonBinding.Button.Pressed -= lineButtonBinding.Handler;
			}
		}
		_lineButtonBindings.Clear();
		if (GodotObject.IsInstanceValid(quizUI))
		{
			quizUI.OnRunButtonPressed -= RunButtonPressed;
			quizUI.QueueFree();
		}
		quizUI = null;
	}

	public void Run(int needCoin)
	{
		RunLifetimeTask(() => RunAsync(needCoin), "Run");
	}

	private async Task RunAsync(int needCoin)
	{
		if (quizUI == null)
		{
			return;
		}
		useCoin = needCoin;
		TowerDefenseManager.Instance.UseCoin(useCoin);
		running = true;
		quizUI.startGUINode.Visible = false;
		TowerDefenseManager.Instance.currentControl.isGameRunning = true;
		foreach (TowerDefenseCharacter vase in vaseList)
		{
			if (GodotObject.IsInstanceValid(vase))
			{
				vase.Destroy();
			}
		}
		foreach (Node child in quizUI.betPanelNode.GetChildren())
		{
			if (child is BetPanel betPanel)
			{
				betPanel.Finish();
			}
		}
		foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (!(towerDefenseCharacter is TowerDefensePlantPresentBox))
			{
				towerDefenseCharacter.SetMainStateMachineDispatchEnabled(enabled: true);
				towerDefenseCharacter.Idle();
			}
		}
		foreach (Node item2 in GetTree().GetNodesInGroup("PresentBox"))
		{
			item2.ProcessMode = Node.ProcessModeEnum.Inherit;
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(quizUI))
		{
			return;
		}
		foreach (Node item3 in GetTree().GetNodesInGroup("PresentBox"))
		{
			if (item3 is TowerDefensePlantPresentBox towerDefensePlantPresentBox && GodotObject.IsInstanceValid(towerDefensePlantPresentBox.instance))
			{
				towerDefensePlantPresentBox.instance.invincible = true;
				towerDefensePlantPresentBox.sprite.SetAnimation("Open", loop: false);
			}
		}
		GameSaveManager.Instance.Save();
	}

	public override bool CheckFinal()
	{
		if (TowerDefenseManager.Instance.GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP.PLANT).Count <= 0)
		{
			if (GetTree().GetNodeCountInGroup("Vase") > 0)
			{
				foreach (Node item in GetTree().GetNodesInGroup("Vase"))
				{
					if (item is TowerDefenseCharacter towerDefenseCharacter)
					{
						towerDefenseCharacter.Destroy();
					}
				}
				return false;
			}
			return true;
		}
		if (CheckFail())
		{
			return true;
		}
		return false;
	}

	public override bool CheckFail()
	{
		if (GetTree().GetNodeCountInGroup("Vase") > 0)
		{
			return false;
		}
		foreach (Variant item in TowerDefenseManager.Instance.GetCampTarget(TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE))
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (!towerDefenseCharacter.instance.die && !towerDefenseCharacter.instance.nearDie)
			{
				return false;
			}
		}
		return true;
	}

	public void Final()
	{
		RunLifetimeTask(FinalAsync, "Final");
	}

	private async Task FinalAsync()
	{
		if (over)
		{
			return;
		}
		over = true;
		if (quizUI == null)
		{
			return;
		}
		TowerDefenseManager.Instance.currentControl.checkBox2X.ButtonPressed = false;
		running = false;
		int coinNum = 0;
		Vector2I gridNum = mapFeature.config.gridNum;
		bool[] zombieWinLine = new bool[gridNum.Y + 1];
		for (int i = 0; i < zombieWinLine.Length; i++)
		{
			zombieWinLine[i] = true;
		}
		foreach (Node item in GetTree().GetNodesInGroup("Brain"))
		{
			if (item is TowerDefenseCharacter towerDefenseCharacter)
			{
				zombieWinLine[towerDefenseCharacter.gridPos.Y] = false;
			}
		}
		string numText = "";
		for (int y = 1; y <= gridNum.Y; y++)
		{
			bool flag = false;
			bool flag2 = false;
			BetPanel obj = quizUI.betPanelNode.GetChild(y - 1) as BetPanel;
			int num = (int)obj.betCoinSpinBox.Value;
			switch (obj.chooseLabel.Text)
			{
			case "赢":
				if (zombieWinLine[y])
				{
					flag = true;
				}
				break;
			case "输":
				if (!zombieWinLine[y])
				{
					flag = true;
				}
				break;
			case "跳":
				flag2 = true;
				break;
			}
			BroadCastManager.Instance.BraodCastClear();
			BroadCastConfig broadCastConfig = new BroadCastConfig();
			if (flag2)
			{
				broadCastConfig.broadCastString = $"第{y}行:不结算";
			}
			else if (flag)
			{
				broadCastConfig.broadCastString = $"第{y}行:判断成功 结算:{num}金币";
				numText = ((y != 1) ? (numText + $"+{num}") : (numText + $"{num}"));
				coinNum += num * (multiple + 1);
			}
			else
			{
				broadCastConfig.broadCastString = $"第{y}行:判断失败 结算:-{num}金币";
				numText = ((y != 1) ? (numText + $"-{num}") : (numText + $"-{num}"));
				coinNum -= num * (multiple + 1);
			}
			BroadCastManager.Instance.BroadCastAdd(broadCastConfig);
			await ToSignal(GetTree().CreateTimer(config.settlementLineDelay, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (!IsLifetimeActive)
			{
				return;
			}
		}
		BroadCastManager.Instance.BraodCastClear();
		BroadCastConfig broadCastConfig2 = new BroadCastConfig();
		if (coinNum > 0)
		{
			broadCastConfig2.broadCastString = $"总结算:({numText})x{multiple + 1}={coinNum}\n获得{coinNum}金币\n返还{useCoin}金币";
		}
		else if (coinNum < 0)
		{
			if (coinNum + useCoin > 0)
			{
				broadCastConfig2.broadCastString = $"总结算:({numText})x{multiple + 1}={coinNum}\n返还{coinNum + useCoin}金币";
			}
			else
			{
				broadCastConfig2.broadCastString = $"总结算:({numText})x{multiple + 1}={coinNum}\n扣除{Math.Abs(coinNum + useCoin)}金币";
			}
		}
		BroadCastManager.Instance.BroadCastAdd(broadCastConfig2);
		if (coinNum > 0)
		{
			await CreateCoinAsync(coinNum + useCoin);
		}
		else if (coinNum < 0)
		{
			if (coinNum + useCoin > 0)
			{
				await CreateCoinAsync(coinNum + useCoin);
			}
			else
			{
				TowerDefenseManager.Instance.UseCoin(Math.Abs(coinNum + useCoin));
			}
		}
		if (IsLifetimeActive)
		{
			await ToSignal(GetTree().CreateTimer(config.settlementSummaryDelay, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (IsLifetimeActive)
			{
				BroadCastManager.Instance.BraodCastClear();
				ViewManager.Instance.FullScreenColorBlink(Colors.White, 0.2, rise: false);
				AudioManager.Instance.AudioPlay("WaveHuge");
				Refresh();
				GameSaveManager.Instance.Save();
			}
		}
	}

	public void CreateCoin(int num)
	{
		RunLifetimeTask(() => CreateCoinAsync(num), "CreateCoin");
	}

	private async Task CreateCoinAsync(int num)
	{
		if (quizUI == null)
		{
			return;
		}
		Vector2I gridNum = mapFeature.config.gridNum;
		while (num >= 1000)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, TowerDefenseManager.Instance.GetMapCellPosCenter(new Vector2I(GD.RandRange(1, gridNum.X), GD.RandRange(1, gridNum.Y))), 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			TowerDefenseCoinBase coinItem = towerDefenseGroundItemBase as TowerDefenseCoinBase;
			if (!GodotObject.IsInstanceValid(coinItem))
			{
				return;
			}
			coinItem.canMagnet = false;
			coinItem.gridPos = new Vector2I(coinItem.gridPos.X, 200);
			coinItem.Reparent(quizUI, keepGlobalTransform: false);
			RunLifetimeTask(() => SettleQuizCoinAsync(coinItem), "SettleQuizCoinAsync");
			num -= 1000;
			await ToSignal(GetTree().CreateTimer(config.coinSpawnInterval, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (!IsLifetimeActive)
			{
				return;
			}
		}
		while (num >= 50)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase2 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, TowerDefenseManager.Instance.GetMapCellPosCenter(new Vector2I(GD.RandRange(1, gridNum.X), GD.RandRange(1, gridNum.Y))), 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			TowerDefenseCoinBase coinItem2 = towerDefenseGroundItemBase2 as TowerDefenseCoinBase;
			if (!GodotObject.IsInstanceValid(coinItem2))
			{
				return;
			}
			coinItem2.canMagnet = false;
			coinItem2.gridPos = new Vector2I(coinItem2.gridPos.X, 200);
			coinItem2.Reparent(quizUI, keepGlobalTransform: false);
			RunLifetimeTask(() => SettleQuizCoinAsync(coinItem2), "SettleQuizCoinAsync");
			num -= 50;
			await ToSignal(GetTree().CreateTimer(config.coinSpawnInterval, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (!IsLifetimeActive)
			{
				return;
			}
		}
		while (num >= 10)
		{
			TowerDefenseGroundItemBase towerDefenseGroundItemBase3 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, TowerDefenseManager.Instance.GetMapCellPosCenter(new Vector2I(GD.RandRange(1, gridNum.X), GD.RandRange(1, gridNum.Y))), 30.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0);
			TowerDefenseCoinBase coinItem3 = towerDefenseGroundItemBase3 as TowerDefenseCoinBase;
			if (!GodotObject.IsInstanceValid(coinItem3))
			{
				break;
			}
			coinItem3.canMagnet = false;
			coinItem3.gridPos = new Vector2I(coinItem3.gridPos.X, 200);
			coinItem3.Reparent(quizUI, keepGlobalTransform: false);
			RunLifetimeTask(() => SettleQuizCoinAsync(coinItem3), "SettleQuizCoinAsync");
			num -= 10;
			await ToSignal(GetTree().CreateTimer(config.coinSpawnInterval, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (!IsLifetimeActive)
			{
				break;
			}
		}
	}

	private async Task SettleQuizCoinAsync(TowerDefenseCoinBase coinItem)
	{
		await ToSignal(GetTree().CreateTimer(config.coinFlightDelay, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (IsLifetimeActive && GodotObject.IsInstanceValid(coinItem))
		{
			if (GodotObject.IsInstanceValid(coinItem.moveComponent))
			{
				coinItem.moveComponent.MoveClear();
			}
			await ToSignal(GetTree().CreateTimer(config.coinCollectDelay, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (IsLifetimeActive && GodotObject.IsInstanceValid(coinItem))
			{
				coinItem.Collection();
			}
		}
	}

	public override async Task GameEntry()
	{
		await GameReady();
	}

	public override Task GameReady()
	{
		if (progressFeature != null)
		{
			progressFeature.SetProgressMeterMaxValue(GetTree().GetNodeCountInGroup("Brain"));
			progressFeature.SetProgressMeterWaveNum(GetTree().GetNodeCountInGroup("Brain"));
			progressFeature.SetProgressMeterPreviewWave(0);
			progressFeature.SetProgressMeterValue(0.0);
			progressFeature.SetLevelNameVisible(visible: true);
			if (Global.Instance.enterLevelMode == "DailyLevel" || Global.Instance.enterLevelMode == "OnlineLevel" || Global.Instance.enterLevelMode == "LevelTest" || Global.Instance.enterLevelMode == "DiyLevel")
			{
				progressFeature.SetDifficultVisible(visible: false);
			}
		}
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		StartQuiz();
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		StartQuiz();
		return Task.CompletedTask;
	}

	public override void GameFail(TowerDefenseCharacter enterCharacter)
	{
	}

	public override void ZombieEnterHouse(TowerDefenseCharacter character)
	{
		character.Destroy();
	}

	public override void Finish()
	{
	}

	public override void Destroy()
	{
		base.Destroy();
		running = false;
		over = true;
		DestroyQuizUI();
		if (GodotObject.IsInstanceValid(BroadCastManager.Instance))
		{
			BroadCastManager.Instance.BraodCastClear();
		}
		vaseList.Clear();
		mapFeature = null;
		progressFeature = null;
		brainFeature = null;
		config = null;
	}

	public override Dictionary SaveProcess()
	{
		GD.Print("[Save] 保存Process[Quiz]...");
		Dictionary result = new Dictionary
		{
			{ "stripPos", stripPos },
			{ "running", running },
			{ "multiple", multiple },
			{ "over", over },
			{ "useCoin", useCoin }
		};
		GD.Print($"[Save] Process[Quiz]保存完成: stripPos={stripPos}, running={running}, multiple={multiple}, over={over}, useCoin={useCoin}");
		return result;
	}

	public override void LoadProcess(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		GD.Print($"[Load] 加载Process[Quiz]... (数据项: {_data.Count})");
		stripPos = _data.GetValueOrDefault("stripPos", 5).AsInt32();
		running = _data.GetValueOrDefault("running", false).AsBool();
		multiple = _data.GetValueOrDefault("multiple", 1).AsInt32();
		over = _data.GetValueOrDefault("over", false).AsBool();
		useCoin = _data.GetValueOrDefault("useCoin", 0).AsInt32();
		GD.Print($"[Load] Process[Quiz]加载完成: stripPos={stripPos}, running={running}, multiple={multiple}, over={over}, useCoin={useCoin}");
	}

	public override void PhysicsProcess(double delta)
	{
		if (running && CheckFinal())
		{
			Final();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(29)
		{
			new MethodInfo(MethodName.CanLoadProgress, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartQuiz, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePresentBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateZombieVase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangePresentBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangePresentBoxButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.RunButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmPendingRun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectPendingRunDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveLineButtonBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyQuizUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "needCoin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CheckFinal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckFail, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Final, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveProcess, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CanLoadProgress && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanLoadProgress());
			return true;
		}
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupUI && args.Count == 0)
		{
			SetupUI();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateUI && args.Count == 0)
		{
			CreateUI();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartQuiz && args.Count == 0)
		{
			StartQuiz();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCell && args.Count == 1)
		{
			ClearCell(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePresentBox && args.Count == 2)
		{
			CreatePresentBox(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateZombieVase && args.Count == 1)
		{
			CreateZombieVase(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangePresentBox && args.Count == 1)
		{
			ChangePresentBox(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangePresentBoxButtonPressed && args.Count == 2)
		{
			ChangePresentBoxButtonPressed(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<MainButton>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunButtonPressed && args.Count == 0)
		{
			RunButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmPendingRun && args.Count == 0)
		{
			ConfirmPendingRun();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectPendingRunDialog && args.Count == 0)
		{
			DisconnectPendingRunDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveLineButtonBinding && args.Count == 1)
		{
			RemoveLineButtonBinding(VariantUtils.ConvertTo<MainButton>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyQuizUI && args.Count == 0)
		{
			DestroyQuizUI();
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 1)
		{
			Run(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CheckFinal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFinal());
			return true;
		}
		if (method == MethodName.CheckFail && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckFail());
			return true;
		}
		if (method == MethodName.Final && args.Count == 0)
		{
			Final();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCoin && args.Count == 1)
		{
			CreateCoin(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 1)
		{
			GameFail(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieEnterHouse && args.Count == 1)
		{
			ZombieEnterHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveProcess && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveProcess());
			return true;
		}
		if (method == MethodName.LoadProcess && args.Count == 2)
		{
			LoadProcess(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PhysicsProcess && args.Count == 1)
		{
			PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CanLoadProgress)
		{
			return true;
		}
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.SetupUI)
		{
			return true;
		}
		if (method == MethodName.CreateUI)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.StartQuiz)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.ClearCell)
		{
			return true;
		}
		if (method == MethodName.CreatePresentBox)
		{
			return true;
		}
		if (method == MethodName.CreateZombieVase)
		{
			return true;
		}
		if (method == MethodName.ChangePresentBox)
		{
			return true;
		}
		if (method == MethodName.ChangePresentBoxButtonPressed)
		{
			return true;
		}
		if (method == MethodName.RunButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ConfirmPendingRun)
		{
			return true;
		}
		if (method == MethodName.DisconnectPendingRunDialog)
		{
			return true;
		}
		if (method == MethodName.RemoveLineButtonBinding)
		{
			return true;
		}
		if (method == MethodName.DestroyQuizUI)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.CheckFinal)
		{
			return true;
		}
		if (method == MethodName.CheckFail)
		{
			return true;
		}
		if (method == MethodName.Final)
		{
			return true;
		}
		if (method == MethodName.CreateCoin)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.ZombieEnterHouse)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.SaveProcess)
		{
			return true;
		}
		if (method == MethodName.LoadProcess)
		{
			return true;
		}
		if (method == MethodName.PhysicsProcess)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.stripPos)
		{
			stripPos = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.running)
		{
			running = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.multiple)
		{
			multiple = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useCoin)
		{
			useCoin = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleProcessQuizConfig>(in value);
			return true;
		}
		if (name == PropertyName.quizStarted)
		{
			quizStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.quizUI)
		{
			quizUI = VariantUtils.ConvertTo<QuizControl>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.progressFeature)
		{
			progressFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureProgress>(in value);
			return true;
		}
		if (name == PropertyName.brainFeature)
		{
			brainFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureBrain>(in value);
			return true;
		}
		if (name == PropertyName._pendingRunDialog)
		{
			_pendingRunDialog = VariantUtils.ConvertTo<DialogBoxChoose>(in value);
			return true;
		}
		if (name == PropertyName._pendingRunCost)
		{
			_pendingRunCost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.stripPos)
		{
			value = VariantUtils.CreateFrom(in stripPos);
			return true;
		}
		if (name == PropertyName.running)
		{
			value = VariantUtils.CreateFrom(in running);
			return true;
		}
		if (name == PropertyName.multiple)
		{
			value = VariantUtils.CreateFrom(in multiple);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.useCoin)
		{
			value = VariantUtils.CreateFrom(in useCoin);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.quizStarted)
		{
			value = VariantUtils.CreateFrom(in quizStarted);
			return true;
		}
		if (name == PropertyName.quizUI)
		{
			value = VariantUtils.CreateFrom(in quizUI);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName.progressFeature)
		{
			value = VariantUtils.CreateFrom(in progressFeature);
			return true;
		}
		if (name == PropertyName.brainFeature)
		{
			value = VariantUtils.CreateFrom(in brainFeature);
			return true;
		}
		if (name == PropertyName._pendingRunDialog)
		{
			value = VariantUtils.CreateFrom(in _pendingRunDialog);
			return true;
		}
		if (name == PropertyName._pendingRunCost)
		{
			value = VariantUtils.CreateFrom(in _pendingRunCost);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.stripPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.running, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.multiple, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.useCoin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.quizStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.quizUI, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.brainFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingRunDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingRunCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.stripPos, Variant.From(in stripPos));
		info.AddProperty(PropertyName.running, Variant.From(in running));
		info.AddProperty(PropertyName.multiple, Variant.From(in multiple));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.useCoin, Variant.From(in useCoin));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.quizStarted, Variant.From(in quizStarted));
		info.AddProperty(PropertyName.quizUI, Variant.From(in quizUI));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.progressFeature, Variant.From(in progressFeature));
		info.AddProperty(PropertyName.brainFeature, Variant.From(in brainFeature));
		info.AddProperty(PropertyName._pendingRunDialog, Variant.From(in _pendingRunDialog));
		info.AddProperty(PropertyName._pendingRunCost, Variant.From(in _pendingRunCost));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.stripPos, out var value))
		{
			stripPos = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.running, out var value2))
		{
			running = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.multiple, out var value3))
		{
			multiple = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value4))
		{
			over = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useCoin, out var value5))
		{
			useCoin = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value6))
		{
			config = value6.As<TowerDefenseBattleProcessQuizConfig>();
		}
		if (info.TryGetProperty(PropertyName.quizStarted, out var value7))
		{
			quizStarted = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.quizUI, out var value8))
		{
			quizUI = value8.As<QuizControl>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value9))
		{
			mapFeature = value9.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.progressFeature, out var value10))
		{
			progressFeature = value10.As<TowerDefenseBattleFeatureProgress>();
		}
		if (info.TryGetProperty(PropertyName.brainFeature, out var value11))
		{
			brainFeature = value11.As<TowerDefenseBattleFeatureBrain>();
		}
		if (info.TryGetProperty(PropertyName._pendingRunDialog, out var value12))
		{
			_pendingRunDialog = value12.As<DialogBoxChoose>();
		}
		if (info.TryGetProperty(PropertyName._pendingRunCost, out var value13))
		{
			_pendingRunCost = value13.As<int>();
		}
	}
}
