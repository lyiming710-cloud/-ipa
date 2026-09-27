using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Fog/TowerDefenseBattleFeatureFog.cs")]
public class TowerDefenseBattleFeatureFog : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName GameFail = "GameFail";

		public static readonly StringName HideFog = "HideFog";

		public static readonly StringName IsLineBlown = "IsLineBlown";

		public new static readonly StringName Process = "Process";

		public static readonly StringName ProcessMagicColorChange = "ProcessMagicColorChange";

		public static readonly StringName ResolveInitialMagicColor = "ResolveInitialMagicColor";

		public static readonly StringName PickNewMagicColor = "PickNewMagicColor";

		public static readonly StringName ApplyMagicColorToFog = "ApplyMagicColorToFog";

		public static readonly StringName ApplyMagicFogEffects = "ApplyMagicFogEffects";

		public static readonly StringName ApplyMagicCharacterEffect = "ApplyMagicCharacterEffect";

		public static readonly StringName ClearMagicZombieBuffs = "ClearMagicZombieBuffs";

		public static readonly StringName ClearMagicColorTweens = "ClearMagicColorTweens";

		public static readonly StringName FogBlow = "FogBlow";

		public static readonly StringName FogBack = "FogBack";

		public static readonly StringName BlowAllEffectEmit = "BlowAllEffectEmit";

		public static readonly StringName BlowLineEffectEmit = "BlowLineEffectEmit";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName SerializeRuntimeState = "SerializeRuntimeState";

		public static readonly StringName ApplyRuntimeState = "ApplyRuntimeState";

		public static readonly StringName SetLineBlownState = "SetLineBlownState";

		public static readonly StringName IsValidLine = "IsValidLine";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName fogNode = "fogNode";

		public static readonly StringName fogBatch = "fogBatch";

		public static readonly StringName config = "config";

		public static readonly StringName beginColumn = "beginColumn";

		public static readonly StringName mapGridNum = "mapGridNum";

		public static readonly StringName timerList = "timerList";

		public static readonly StringName tweenList = "tweenList";

		public static readonly StringName fogLine = "fogLine";

		public static readonly StringName fogNodeInitX = "fogNodeInitX";

		public static readonly StringName _magicElapsed = "_magicElapsed";

		public static readonly StringName _magicTickTimer = "_magicTickTimer";

		public static readonly StringName _magicChangeIndex = "_magicChangeIndex";

		public static readonly StringName _magicWarningIndex = "_magicWarningIndex";

		public static readonly StringName _magicFadeRemaining = "_magicFadeRemaining";

		public static readonly StringName _restoreMagicFade = "_restoreMagicFade";

		public static readonly StringName _magicColor = "_magicColor";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private const int FogRightOverlapColumns = 3;

	private const int FogBottomOverlapRows = 1;

	private static PackedScene _towerDefenseFog;

	public Node2D fogNode;

	public TowerDefenseFogBatch fogBatch;

	public TowerDefenseLevelFogManagerConfig config;

	public int beginColumn;

	public Vector2I mapGridNum;

	public Array<Timer> timerList = new Array<Timer>();

	public Array<Array<Tween>> tweenList = new Array<Array<Tween>>();

	public Array<Array<TowerDefenseFog>> fogLine = new Array<Array<TowerDefenseFog>>();

	public double fogNodeInitX;

	private const double MagicTickInterval = 0.25;

	private const double MagicColorFadeDuration = 1.5;

	private const double MagicChangingTipDuration = 2.0;

	private const string MagicChangingTipKey = "TOWERDEFENSE_TIPS_MAGICFOG_CHANGING";

	private double _magicElapsed;

	private double _magicTickTimer;

	private int _magicChangeIndex;

	private int _magicWarningIndex = -1;

	private double _magicFadeRemaining;

	private bool _restoreMagicFade;

	private TowerDefenseRuneMagicColor _magicColor;

	private readonly HashSet<Vector2I> _magicCoveredCells = new HashSet<Vector2I>();

	private readonly List<Tween> _magicColorTweens = new List<Tween>();

	private static PackedScene TOWER_DEFENSE_FOG => _towerDefenseFog ?? (_towerDefenseFog = GD.Load<PackedScene>("uid://cu8nc147admpx"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelFogManagerConfig();
		config.Init(data);
		beginColumn = config.beginColumn;
		TowerDefenseBattleFeatureMap feature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
		if (!GodotObject.IsInstanceValid(feature) || !GodotObject.IsInstanceValid(feature.mapConfig))
		{
			throw new InvalidOperationException("Fog requires an initialized Map feature configuration.");
		}
		mapGridNum = feature.mapConfig.gridNum;
		fogNode = new Node2D();
		control.AddNode(fogNode, 1);
		fogBatch = new TowerDefenseFogBatch();
		fogNode.AddChild(fogBatch, forceReadableName: false, Node.InternalMode.Disabled);
		int num = Math.Max(2, mapGridNum.Y + 2);
		for (int i = 0; i < num; i++)
		{
			Timer timer = new Timer();
			timer.OneShot = true;
			timer.Autostart = false;
			timer.WaitTime = config.blowReturnDelay;
			int lineIdx = i;
			timer.Timeout += () =>
			{
				FogBack(lineIdx);
			};
			timerList.Add(timer);
			fogNode.AddChild(timer, forceReadableName: false, Node.InternalMode.Disabled);
		}
		for (int num2 = 0; num2 < num; num2++)
		{
			fogLine.Add(new Array<TowerDefenseFog>());
			tweenList.Add(new Array<Tween>());
		}
		BattleEventBus.Instance.OnBlowAllEffectEmit += BlowAllEffectEmit;
		BattleEventBus.Instance.OnBlowLineEffectEmit += BlowLineEffectEmit;
		BattleEventBus.Instance.OnGameVictory += HideFog;
	}

	public override void Destroy()
	{
		ClearMagicColorTweens();
		TowerDefenseRuneMagicUtil.ForEachZombie(ClearMagicZombieBuffs);
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnBlowAllEffectEmit -= BlowAllEffectEmit;
			BattleEventBus.Instance.OnBlowLineEffectEmit -= BlowLineEffectEmit;
			BattleEventBus.Instance.OnGameVictory -= HideFog;
		}
		foreach (Array<Tween> tween in tweenList)
		{
			foreach (Tween item in tween)
			{
				if (GodotObject.IsInstanceValid(item))
				{
					item.Kill();
				}
			}
			tween.Clear();
		}
		timerList.Clear();
		tweenList.Clear();
		fogLine.Clear();
		if (GodotObject.IsInstanceValid(fogNode))
		{
			fogNode.QueueFree();
		}
		fogBatch = null;
		fogNode = null;
		base.Destroy();
	}

	public override Task GameInit()
	{
		for (int i = beginColumn; i < mapGridNum.X + config.extraColumns; i++)
		{
			for (int j = 1; j <= mapGridNum.Y + 1; j++)
			{
				Vector2 mapCellPosCenter = TowerDefenseManager.Instance.GetMapCellPosCenter(new Vector2I(i, j));
				TowerDefenseFog towerDefenseFog = TOWER_DEFENSE_FOG.Instantiate<TowerDefenseFog>(PackedScene.GenEditState.Disabled);
				towerDefenseFog.GlobalPosition = mapCellPosCenter;
				towerDefenseFog.gridPos = new Vector2(i, j);
				towerDefenseFog.beginColumn = beginColumn;
				towerDefenseFog.lightOverlapEnabled = i <= mapGridNum.X + 3 && j <= mapGridNum.Y + 1;
				fogLine[j].Add(towerDefenseFog);
				fogNode.AddChild(towerDefenseFog, forceReadableName: false, Node.InternalMode.Disabled);
				if (towerDefenseFog.lightOverlapEnabled)
				{
					fogBatch.Register(towerDefenseFog);
				}
				else
				{
					towerDefenseFog.area.QueueFree();
				}
			}
		}
		Vector2 mapCellPosCenter2 = TowerDefenseManager.Instance.GetMapCellPosCenter(new Vector2I(beginColumn, 0));
		fogNodeInitX = config.entryStartX - mapCellPosCenter2.X;
		fogNode.GlobalPosition = new Vector2((float)fogNodeInitX, fogNode.GlobalPosition.Y);
		if (config.magicOpen)
		{
			_magicColor = ResolveInitialMagicColor();
			ApplyMagicColorToFog(animated: false);
		}
		return Task.CompletedTask;
	}

	public override async Task GameInitFromProgress()
	{
		await GameInit();
	}

	public override Task GameEntry()
	{
		if (control.hasProgress)
		{
			return Task.CompletedTask;
		}
		for (int i = 1; i <= mapGridNum.Y + 1; i++)
		{
			foreach (Tween item in tweenList[i])
			{
				if (GodotObject.IsInstanceValid(item))
				{
					item.Kill();
				}
			}
			tweenList[i].Clear();
			if (GodotObject.IsInstanceValid(timerList[i]))
			{
				timerList[i].Stop();
			}
			foreach (TowerDefenseFog item2 in fogLine[i])
			{
				if (GodotObject.IsInstanceValid(item2))
				{
					item2.Position = item2.savePos;
					if (item2.gridPos.X > (float)(mapGridNum.X + 1))
					{
						item2.SetCanVisible(visible: false);
					}
					else
					{
						item2.SetCanVisible(visible: true);
					}
				}
			}
		}
		if (GodotObject.IsInstanceValid(fogNode))
		{
			fogNode.GlobalPosition = new Vector2((float)fogNodeInitX, fogNode.GlobalPosition.Y);
		}
		return Task.CompletedTask;
	}

	public override Task GameReady()
	{
		for (int i = 1; i <= mapGridNum.Y + 1; i++)
		{
			foreach (TowerDefenseFog item in fogLine[i])
			{
				if (GodotObject.IsInstanceValid(item) && item.gridPos.X > (float)(mapGridNum.X + 1))
				{
					item.SetCanVisible(visible: true);
				}
			}
		}
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		Tween tween = fogNode.CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(fogNode, "global_position:x", 0f, config.entryDuration);
		return Task.CompletedTask;
	}

	public override Task GameStartFromProgress()
	{
		if (GodotObject.IsInstanceValid(fogNode))
		{
			fogNode.GlobalPosition = new Vector2(0f, fogNode.GlobalPosition.Y);
		}
		if (_restoreMagicFade)
		{
			ApplyMagicColorToFog(animated: true, _magicFadeRemaining);
			_restoreMagicFade = false;
		}
		if (config.magicOpen)
		{
			ProcessMagicColorChange();
		}
		return Task.CompletedTask;
	}

	public override Dictionary SyncSerialize()
	{
		return SerializeRuntimeState();
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		ApplyRuntimeState(_data, animateColor: true);
	}

	public override void GameFail()
	{
		HideFog();
	}

	public void HideFog()
	{
		if (GodotObject.IsInstanceValid(fogNode))
		{
			fogNode.Visible = false;
		}
	}

	public bool IsLineBlown(int line)
	{
		if (IsValidLine(line) && GodotObject.IsInstanceValid(timerList[line]))
		{
			return !timerList[line].IsStopped();
		}
		return false;
	}

	public override void Process(double delta)
	{
		if (!GodotObject.IsInstanceValid(config) || !config.magicOpen || !TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		double num = Mathf.Max(0.0, delta);
		if (!(num <= 0.0))
		{
			_magicElapsed += num;
			_magicFadeRemaining = Math.Max(0.0, _magicFadeRemaining - num);
			ProcessMagicColorChange();
			_magicTickTimer += num;
			if (!(_magicTickTimer < 0.25))
			{
				double magicTickTimer = _magicTickTimer;
				_magicTickTimer = 0.0;
				ApplyMagicFogEffects(magicTickTimer);
			}
		}
	}

	private void ProcessMagicColorChange()
	{
		while (config.magicChangeInterval > 0.0)
		{
			double num = ((double)_magicChangeIndex + 1.0) * config.magicChangeInterval - _magicElapsed;
			if (_magicWarningIndex != _magicChangeIndex && num <= 2.0)
			{
				_magicWarningIndex = _magicChangeIndex;
				RuneWeatherNotice.Show(control, "TOWERDEFENSE_TIPS_MAGICFOG_CHANGING", Math.Max(0.01, num));
			}
			if (!(num > 0.0))
			{
				_magicChangeIndex++;
				PickNewMagicColor(animated: true);
				continue;
			}
			break;
		}
	}

	private TowerDefenseRuneMagicColor ResolveInitialMagicColor()
	{
		if (TowerDefenseRuneMagicUtil.TryParse(config.magicColor, out var color) && color != TowerDefenseRuneMagicColor.Orange)
		{
			return color;
		}
		return TowerDefenseRuneMagicUtil.RandomFogColor();
	}

	private void PickNewMagicColor(bool animated)
	{
		_magicColor = TowerDefenseRuneMagicUtil.RandomFogColorExcept(_magicColor);
		ApplyMagicColorToFog(animated);
	}

	private void ApplyMagicColorToFog(bool animated, double fadeDuration = 1.5)
	{
		ClearMagicColorTweens();
		_magicFadeRemaining = (animated ? fadeDuration : 0.0);
		Color color = TowerDefenseRuneMagicUtil.ToFogTint(_magicColor);
		foreach (Array<TowerDefenseFog> item in fogLine)
		{
			foreach (TowerDefenseFog fog in item)
			{
				if (!GodotObject.IsInstanceValid(fog))
				{
					continue;
				}
				if (!animated)
				{
					fog.SetMagicColor(color);
					continue;
				}
				Tween tween = fog.CreateTween();
				_magicColorTweens.Add(tween);
				tween.TweenMethod(Callable.From((Color magicColor) =>
				{
					fog.SetMagicColor(magicColor);
				}), fog.GetMagicColor(), color, fadeDuration);
			}
		}
	}

	private void ApplyMagicFogEffects(double tickDelta)
	{
		_magicCoveredCells.Clear();
		double refreshTime = 0.75;
		for (int i = 1; i <= mapGridNum.Y; i++)
		{
			if (!GodotObject.IsInstanceValid(fogNode) || !fogNode.Visible || !IsValidLine(i) || IsLineBlown(i))
			{
				continue;
			}
			foreach (TowerDefenseFog item in fogLine[i])
			{
				if (GodotObject.IsInstanceValid(item) && item.canVisible && !item.IsLit)
				{
					Vector2I vector2I = new Vector2I(Mathf.RoundToInt(item.gridPos.X), Mathf.RoundToInt(item.gridPos.Y));
					if (GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(vector2I)))
					{
						_magicCoveredCells.Add(vector2I);
					}
				}
			}
		}
		TowerDefenseRuneMagicUtil.ForEachPlant((TowerDefenseCharacter character) =>
		{
			ApplyMagicCharacterEffect(character, tickDelta, refreshTime);
		});
		TowerDefenseRuneMagicUtil.ForEachZombie((TowerDefenseCharacter character) =>
		{
			ApplyMagicCharacterEffect(character, tickDelta, refreshTime);
		});
	}

	private void ApplyMagicCharacterEffect(TowerDefenseCharacter character, double tickDelta, double refreshTime)
	{
		bool flag = _magicCoveredCells.Contains(character.gridPos);
		if (!flag || _magicColor != TowerDefenseRuneMagicColor.Purple)
		{
			character.buff.DeleteBuff("RuneFogDizzyImmune");
		}
		if (!flag || _magicColor != TowerDefenseRuneMagicColor.Blue)
		{
			character.buff.DeleteBuff("RuneFogHaste");
		}
		if (flag)
		{
			TowerDefenseRuneMagicUtil.ApplyFogEffect(_magicColor, character, refreshTime, config.magicDamagePercentPerSecond, config.magicHealPerSecond, tickDelta);
		}
	}

	private static void ClearMagicZombieBuffs(TowerDefenseCharacter character)
	{
		character.buff.DeleteBuff("RuneFogDizzyImmune");
		character.buff.DeleteBuff("RuneFogHaste");
	}

	private void ClearMagicColorTweens()
	{
		foreach (Tween magicColorTween in _magicColorTweens)
		{
			if (GodotObject.IsInstanceValid(magicColorTween))
			{
				magicColorTween.Kill();
			}
		}
		_magicColorTweens.Clear();
	}

	public void FogBlow(int line)
	{
		if (!IsValidLine(line))
		{
			return;
		}
		foreach (Tween item in tweenList[line])
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.Kill();
			}
		}
		tweenList[line].Clear();
		foreach (TowerDefenseFog item2 in fogLine[line])
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				Tween tween = item2.CreateTween();
				tween.SetEase(Tween.EaseType.Out);
				tween.SetTrans(Tween.TransitionType.Cubic);
				tween.TweenProperty(item2, "global_position:x", item2.savePos.X + config.blowDistance, config.blowDuration);
				tweenList[line].Add(tween);
			}
		}
	}

	public void FogBack(int line)
	{
		if (!IsValidLine(line))
		{
			return;
		}
		foreach (Tween item in tweenList[line])
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.Kill();
			}
		}
		tweenList[line].Clear();
		foreach (TowerDefenseFog item2 in fogLine[line])
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				Tween tween = item2.CreateTween();
				tween.SetEase(Tween.EaseType.Out);
				tween.SetTrans(Tween.TransitionType.Cubic);
				tween.TweenProperty(item2, "global_position:x", item2.savePos.X, config.returnDuration);
				tweenList[line].Add(tween);
			}
		}
	}

	public void BlowAllEffectEmit()
	{
		for (int i = 1; i <= mapGridNum.Y + 1; i++)
		{
			if (GodotObject.IsInstanceValid(timerList[i]))
			{
				timerList[i].Start(config.blowReturnDelay);
			}
			FogBlow(i);
		}
	}

	public void BlowLineEffectEmit(int line)
	{
		if (IsValidLine(line))
		{
			if (GodotObject.IsInstanceValid(timerList[line]))
			{
				timerList[line].Start(config.blowReturnDelay);
			}
			FogBlow(line);
		}
	}

	public override Dictionary SaveFeature()
	{
		return SerializeRuntimeState();
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		ApplyRuntimeState(_data);
	}

	private Dictionary SerializeRuntimeState()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Timer timer in timerList)
		{
			if (GodotObject.IsInstanceValid(timer) && !timer.IsStopped())
			{
				array.Add(timer.TimeLeft);
			}
			else
			{
				array.Add(-1.0);
			}
		}
		double num = 0.0;
		if (GodotObject.IsInstanceValid(fogNode))
		{
			num = fogNode.GlobalPosition.X;
		}
		Color color = TowerDefenseRuneMagicUtil.ToColor(_magicColor);
		foreach (Array<TowerDefenseFog> item in fogLine)
		{
			if (item.Count > 0 && GodotObject.IsInstanceValid(item[0]))
			{
				color = TowerDefenseRuneMagicUtil.UntintFogColor(item[0].GetMagicColor());
				break;
			}
		}
		return new Dictionary
		{
			["timerRemains"] = array,
			["fogNodePosX"] = num,
			["fogNodeVisible"] = GodotObject.IsInstanceValid(fogNode) && fogNode.Visible,
			["magicColor"] = TowerDefenseRuneMagicUtil.ToKey(_magicColor),
			["magicElapsed"] = _magicElapsed,
			["magicTickTimer"] = _magicTickTimer,
			["magicChangeIndex"] = _magicChangeIndex,
			["magicFadeRemaining"] = _magicFadeRemaining,
			["magicVisualColor"] = new Godot.Collections.Array { color.R, color.G, color.B }
		};
	}

	private void ApplyRuntimeState(Dictionary state, bool animateColor = false)
	{
		if (config.magicOpen && state.ContainsKey("magicColor"))
		{
			if (TowerDefenseRuneMagicUtil.TryParse(state["magicColor"].AsString(), out var color) && color != TowerDefenseRuneMagicColor.Orange)
			{
				bool flag = _magicColor != color;
				_magicColor = color;
				if (animateColor & flag)
				{
					ApplyMagicColorToFog(animated: true);
				}
				else if (!animateColor)
				{
					ClearMagicColorTweens();
					Color magicColor = TowerDefenseRuneMagicUtil.ToFogTint(color);
					Godot.Collections.Array array = state.GetValueOrDefault("magicVisualColor", new Godot.Collections.Array()).AsGodotArray();
					if (array.Count >= 3)
					{
						magicColor = TowerDefenseRuneMagicUtil.ToFogTint(new Color((float)array[0].AsDouble(), (float)array[1].AsDouble(), (float)array[2].AsDouble()));
					}
					foreach (Array<TowerDefenseFog> item in fogLine)
					{
						foreach (TowerDefenseFog item2 in item)
						{
							if (GodotObject.IsInstanceValid(item2))
							{
								item2.SetMagicColor(magicColor);
							}
						}
					}
					_magicFadeRemaining = Math.Max(0.0, state.GetValueOrDefault("magicFadeRemaining", 0.0).AsDouble());
					_restoreMagicFade = _magicFadeRemaining > 0.0;
				}
			}
			_magicElapsed = Math.Max(0.0, state.GetValueOrDefault("magicElapsed", 0.0).AsDouble());
			_magicTickTimer = Mathf.Clamp(state.GetValueOrDefault("magicTickTimer", 0.0).AsDouble(), 0.0, 0.25);
			_magicChangeIndex = Math.Max(0, state.GetValueOrDefault("magicChangeIndex", 0).AsInt32());
			_magicWarningIndex = -1;
		}
		double num = state.GetValueOrDefault("fogNodePosX", 0.0).AsDouble();
		if (GodotObject.IsInstanceValid(fogNode))
		{
			fogNode.GlobalPosition = new Vector2((float)num, fogNode.GlobalPosition.Y);
			fogNode.Visible = state.GetValueOrDefault("fogNodeVisible", fogNode.Visible).AsBool();
		}
		Godot.Collections.Array array2 = state.GetValueOrDefault("timerRemains", new Godot.Collections.Array()).AsGodotArray();
		for (int i = 0; i < timerList.Count; i++)
		{
			double num2 = ((i < array2.Count) ? array2[i].AsDouble() : (-1.0));
			if (num2 >= 0.0 && GodotObject.IsInstanceValid(timerList[i]))
			{
				timerList[i].Start(Math.Max(0.001, num2));
				SetLineBlownState(i, blown: true);
			}
			else if (GodotObject.IsInstanceValid(timerList[i]))
			{
				timerList[i].Stop();
				SetLineBlownState(i, blown: false);
			}
		}
	}

	private void SetLineBlownState(int line, bool blown)
	{
		if (!IsValidLine(line))
		{
			return;
		}
		foreach (Tween item in tweenList[line])
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.Kill();
			}
		}
		tweenList[line].Clear();
		foreach (TowerDefenseFog item2 in fogLine[line])
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				item2.GlobalPosition = new Vector2(item2.savePos.X + (blown ? config.blowDistance : 0f), item2.GlobalPosition.Y);
			}
		}
	}

	private bool IsValidLine(int line)
	{
		if (line >= 0 && line < timerList.Count && line < tweenList.Count)
		{
			return line < fogLine.Count;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideFog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsLineBlown, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessMagicColorChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveInitialMagicColor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PickNewMagicColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "animated", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMagicColorToFog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "animated", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fadeDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMagicFogEffects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "tickDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMagicCharacterEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "tickDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "refreshTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearMagicZombieBuffs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearMagicColorTweens, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FogBlow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FogBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BlowAllEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlowLineEffectEmit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SerializeRuntimeState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRuntimeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "animateColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLineBlownState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "blown", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
			ret = default;
			return true;
		}
		if (method == MethodName.HideFog && args.Count == 0)
		{
			HideFog();
			ret = default;
			return true;
		}
		if (method == MethodName.IsLineBlown && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLineBlown(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessMagicColorChange && args.Count == 0)
		{
			ProcessMagicColorChange();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveInitialMagicColor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseRuneMagicColor>(ResolveInitialMagicColor());
			return true;
		}
		if (method == MethodName.PickNewMagicColor && args.Count == 1)
		{
			PickNewMagicColor(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMagicColorToFog && args.Count == 2)
		{
			ApplyMagicColorToFog(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMagicFogEffects && args.Count == 1)
		{
			ApplyMagicFogEffects(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMagicCharacterEffect && args.Count == 3)
		{
			ApplyMagicCharacterEffect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearMagicZombieBuffs && args.Count == 1)
		{
			ClearMagicZombieBuffs(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearMagicColorTweens && args.Count == 0)
		{
			ClearMagicColorTweens();
			ret = default;
			return true;
		}
		if (method == MethodName.FogBlow && args.Count == 1)
		{
			FogBlow(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FogBack && args.Count == 1)
		{
			FogBack(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BlowAllEffectEmit && args.Count == 0)
		{
			BlowAllEffectEmit();
			ret = default;
			return true;
		}
		if (method == MethodName.BlowLineEffectEmit && args.Count == 1)
		{
			BlowLineEffectEmit(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SerializeRuntimeState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SerializeRuntimeState());
			return true;
		}
		if (method == MethodName.ApplyRuntimeState && args.Count == 2)
		{
			ApplyRuntimeState(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLineBlownState && args.Count == 2)
		{
			SetLineBlownState(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsValidLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidLine(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClearMagicZombieBuffs && args.Count == 1)
		{
			ClearMagicZombieBuffs(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.HideFog)
		{
			return true;
		}
		if (method == MethodName.IsLineBlown)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.ProcessMagicColorChange)
		{
			return true;
		}
		if (method == MethodName.ResolveInitialMagicColor)
		{
			return true;
		}
		if (method == MethodName.PickNewMagicColor)
		{
			return true;
		}
		if (method == MethodName.ApplyMagicColorToFog)
		{
			return true;
		}
		if (method == MethodName.ApplyMagicFogEffects)
		{
			return true;
		}
		if (method == MethodName.ApplyMagicCharacterEffect)
		{
			return true;
		}
		if (method == MethodName.ClearMagicZombieBuffs)
		{
			return true;
		}
		if (method == MethodName.ClearMagicColorTweens)
		{
			return true;
		}
		if (method == MethodName.FogBlow)
		{
			return true;
		}
		if (method == MethodName.FogBack)
		{
			return true;
		}
		if (method == MethodName.BlowAllEffectEmit)
		{
			return true;
		}
		if (method == MethodName.BlowLineEffectEmit)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.SerializeRuntimeState)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeState)
		{
			return true;
		}
		if (method == MethodName.SetLineBlownState)
		{
			return true;
		}
		if (method == MethodName.IsValidLine)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.fogNode)
		{
			fogNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.fogBatch)
		{
			fogBatch = VariantUtils.ConvertTo<TowerDefenseFogBatch>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelFogManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.beginColumn)
		{
			beginColumn = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.mapGridNum)
		{
			mapGridNum = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.timerList)
		{
			timerList = VariantUtils.ConvertToArray<Timer>(in value);
			return true;
		}
		if (name == PropertyName.tweenList)
		{
			tweenList = VariantUtils.ConvertToArray<Array<Tween>>(in value);
			return true;
		}
		if (name == PropertyName.fogLine)
		{
			fogLine = VariantUtils.ConvertToArray<Array<TowerDefenseFog>>(in value);
			return true;
		}
		if (name == PropertyName.fogNodeInitX)
		{
			fogNodeInitX = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._magicElapsed)
		{
			_magicElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._magicTickTimer)
		{
			_magicTickTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._magicChangeIndex)
		{
			_magicChangeIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._magicWarningIndex)
		{
			_magicWarningIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._magicFadeRemaining)
		{
			_magicFadeRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._restoreMagicFade)
		{
			_restoreMagicFade = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._magicColor)
		{
			_magicColor = VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fogNode)
		{
			value = VariantUtils.CreateFrom(in fogNode);
			return true;
		}
		if (name == PropertyName.fogBatch)
		{
			value = VariantUtils.CreateFrom(in fogBatch);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.beginColumn)
		{
			value = VariantUtils.CreateFrom(in beginColumn);
			return true;
		}
		if (name == PropertyName.mapGridNum)
		{
			value = VariantUtils.CreateFrom(in mapGridNum);
			return true;
		}
		if (name == PropertyName.timerList)
		{
			value = VariantUtils.CreateFromArray(timerList);
			return true;
		}
		if (name == PropertyName.tweenList)
		{
			value = VariantUtils.CreateFromArray(tweenList);
			return true;
		}
		if (name == PropertyName.fogLine)
		{
			value = VariantUtils.CreateFromArray(fogLine);
			return true;
		}
		if (name == PropertyName.fogNodeInitX)
		{
			value = VariantUtils.CreateFrom(in fogNodeInitX);
			return true;
		}
		if (name == PropertyName._magicElapsed)
		{
			value = VariantUtils.CreateFrom(in _magicElapsed);
			return true;
		}
		if (name == PropertyName._magicTickTimer)
		{
			value = VariantUtils.CreateFrom(in _magicTickTimer);
			return true;
		}
		if (name == PropertyName._magicChangeIndex)
		{
			value = VariantUtils.CreateFrom(in _magicChangeIndex);
			return true;
		}
		if (name == PropertyName._magicWarningIndex)
		{
			value = VariantUtils.CreateFrom(in _magicWarningIndex);
			return true;
		}
		if (name == PropertyName._magicFadeRemaining)
		{
			value = VariantUtils.CreateFrom(in _magicFadeRemaining);
			return true;
		}
		if (name == PropertyName._restoreMagicFade)
		{
			value = VariantUtils.CreateFrom(in _restoreMagicFade);
			return true;
		}
		if (name == PropertyName._magicColor)
		{
			value = VariantUtils.CreateFrom(in _magicColor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.fogNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.fogBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.beginColumn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.mapGridNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.timerList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.tweenList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.fogLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fogNodeInitX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._magicElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._magicTickTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._magicChangeIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._magicWarningIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._magicFadeRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._restoreMagicFade, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._magicColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fogNode, Variant.From(in fogNode));
		info.AddProperty(PropertyName.fogBatch, Variant.From(in fogBatch));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.beginColumn, Variant.From(in beginColumn));
		info.AddProperty(PropertyName.mapGridNum, Variant.From(in mapGridNum));
		info.AddProperty(PropertyName.timerList, Variant.CreateFrom(timerList));
		info.AddProperty(PropertyName.tweenList, Variant.CreateFrom(tweenList));
		info.AddProperty(PropertyName.fogLine, Variant.CreateFrom(fogLine));
		info.AddProperty(PropertyName.fogNodeInitX, Variant.From(in fogNodeInitX));
		info.AddProperty(PropertyName._magicElapsed, Variant.From(in _magicElapsed));
		info.AddProperty(PropertyName._magicTickTimer, Variant.From(in _magicTickTimer));
		info.AddProperty(PropertyName._magicChangeIndex, Variant.From(in _magicChangeIndex));
		info.AddProperty(PropertyName._magicWarningIndex, Variant.From(in _magicWarningIndex));
		info.AddProperty(PropertyName._magicFadeRemaining, Variant.From(in _magicFadeRemaining));
		info.AddProperty(PropertyName._restoreMagicFade, Variant.From(in _restoreMagicFade));
		info.AddProperty(PropertyName._magicColor, Variant.From(in _magicColor));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fogNode, out var value))
		{
			fogNode = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.fogBatch, out var value2))
		{
			fogBatch = value2.As<TowerDefenseFogBatch>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value3))
		{
			config = value3.As<TowerDefenseLevelFogManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.beginColumn, out var value4))
		{
			beginColumn = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.mapGridNum, out var value5))
		{
			mapGridNum = value5.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.timerList, out var value6))
		{
			timerList = value6.AsGodotArray<Timer>();
		}
		if (info.TryGetProperty(PropertyName.tweenList, out var value7))
		{
			tweenList = value7.AsGodotArray<Array<Tween>>();
		}
		if (info.TryGetProperty(PropertyName.fogLine, out var value8))
		{
			fogLine = value8.AsGodotArray<Array<TowerDefenseFog>>();
		}
		if (info.TryGetProperty(PropertyName.fogNodeInitX, out var value9))
		{
			fogNodeInitX = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._magicElapsed, out var value10))
		{
			_magicElapsed = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._magicTickTimer, out var value11))
		{
			_magicTickTimer = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._magicChangeIndex, out var value12))
		{
			_magicChangeIndex = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._magicWarningIndex, out var value13))
		{
			_magicWarningIndex = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._magicFadeRemaining, out var value14))
		{
			_magicFadeRemaining = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName._restoreMagicFade, out var value15))
		{
			_restoreMagicFade = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._magicColor, out var value16))
		{
			_magicColor = value16.As<TowerDefenseRuneMagicColor>();
		}
	}
}
