using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/JalaVase/Scene/TowerDefensePlantJalaVase.cs")]
public class TowerDefensePlantJalaVase : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName HandleVasePressed = "HandleVasePressed";

		public static readonly StringName _ResetPressAwait = "_ResetPressAwait";

		public static readonly StringName AttackEntered = "AttackEntered";

		public static readonly StringName AttackProcessing = "AttackProcessing";

		public static readonly StringName AttackExited = "AttackExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public static readonly StringName HammerAnimeCompleted = "HammerAnimeCompleted";

		public static readonly StringName CanAddJala = "CanAddJala";

		public static readonly StringName AddJala = "AddJala";

		public static readonly StringName TryAddJala = "TryAddJala";

		public static readonly StringName ExecuteJala = "ExecuteJala";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName RestoreJalaPacket = "RestoreJalaPacket";

		public static readonly StringName ApplyJalaPresentation = "ApplyJalaPresentation";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _hammer = "_hammer";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName isMoseIn = "isMoseIn";

		public static readonly StringName pressed = "pressed";

		public static readonly StringName over = "over";

		public static readonly StringName jalaNameList = "jalaNameList";

		public static readonly StringName chunksEffect = "chunksEffect";

		public static readonly StringName jalaList = "jalaList";

		public static readonly StringName timerList = "timerList";

		public static readonly StringName timeNeed = "timeNeed";

		public static readonly StringName pressAwait = "pressAwait";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const int MAX_JALA_COUNT = 4;

	private const string JALA_VASE_BACK5 = "uid://pnu58qk7jitu";

	private const string JALA_VASE_BODY2 = "uid://3jm2ghlcepiq";

	private const string JALA_VASE_BODY3 = "uid://co3xrnatu6j8a";

	private const string JALA_VASE_BODY4 = "uid://bap8un78l21il";

	private const string JALA_VASE_BODY5 = "uid://j6cc4gemmsje";

	private const string JALA_VASE_JALA4 = "uid://buvtla2up6tvu";

	private const string JALA_VASE_JALA5 = "uid://b57i3wamhxyrb";

	private static PackedScene _JALA_VASE_CHUNKS2;

	private static PackedScene _JALA_VASE_CHUNKS;

	private AdobeAnimateSpriteBase _hammer;

	private MousePressComponent _mousePressComponent;

	private StateHandle _attackState;

	private bool _roleStateSignalsConnected;

	public bool isMoseIn;

	public bool pressed;

	public bool over;

	public Godot.Collections.Array jalaNameList = new Godot.Collections.Array
	{
		"PlantJalapeno", "PlantJalaNut", "PlantJalaNutBowling", "PlantJalaCherryBomb", "PlantJalaSunShroom", "PlantJalaPurify", "PlantJalaTorch", "PlantJalaJoker", "PlantJalapenopepe", "PlantGarlicJalapeno",
		"PlantJalaGhost"
	};

	public PackedScene chunksEffect;

	public Array<TowerDefensePacketConfig> jalaList = new Array<TowerDefensePacketConfig>();

	public Array<double> timerList = new Array<double> { 0.0, 0.0, 0.0, 0.0 };

	public double timeNeed = 50.0;

	public bool pressAwait;

	private static PackedScene JALA_VASE_CHUNKS2 => _JALA_VASE_CHUNKS2 ?? (_JALA_VASE_CHUNKS2 = GD.Load<PackedScene>("uid://btoja37o43wvj"));

	private static PackedScene JALA_VASE_CHUNKS => _JALA_VASE_CHUNKS ?? (_JALA_VASE_CHUNKS = GD.Load<PackedScene>("uid://tw8msyx3iwy"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_hammer = GetNode<AdobeAnimateSpriteBase>("%Hammer");
			_hammer.OnAnimeCompleted += HammerAnimeCompleted;
			_mousePressComponent = componentManager?.GetRuntime<MousePressComponent>();
			MousePressComponent mousePressComponent = _mousePressComponent;
			if (mousePressComponent != null && !mousePressComponent.IsReleased)
			{
				_mousePressComponent.OnPressed += HandleVasePressed;
			}
			chunksEffect = JALA_VASE_CHUNKS;
			_attackState = StateMachine?.GetStateById("plant.jala_vase.attack");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		MousePressComponent mousePressComponent = _mousePressComponent;
		if (mousePressComponent != null && !mousePressComponent.IsReleased)
		{
			_mousePressComponent.OnPressed -= HandleVasePressed;
		}
		_mousePressComponent = null;
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (!_roleStateSignalsConnected)
		{
			StateHandle attackState = _attackState;
			if (attackState != null && attackState.IsValid)
			{
				_attackState.Entered += AttackEntered;
				_attackState.Exited += AttackExited;
				_attackState.PhysicsProcessing += AttackProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_attackState.Entered -= AttackEntered;
			_attackState.Exited -= AttackExited;
			_attackState.PhysicsProcessing -= AttackProcessing;
			_attackState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base.BatchUpdate(delta);
		isMoseIn = _mousePressComponent?.IsMouseInside ?? false;
		if ((Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage") || !TowerDefenseManager.Instance.IsGameRunning())
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < jalaList.Count; i++)
		{
			if (timerList[i] < timeNeed)
			{
				timerList[i] += delta;
				continue;
			}
			timerList[i] = 0.0;
			ExecuteJala(jalaList[i]);
			flag = true;
		}
		if (flag)
		{
			SendStateEvent("ToAttack");
		}
	}

	private void HandleVasePressed(Vector2 pointerPosition)
	{
		if (pressAwait || pressed)
		{
			return;
		}
		PacketPickControl packetPickControl = TowerDefenseManager.Instance.GetPacketPickControl();
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = (GodotObject.IsInstanceValid(packetPickControl) ? packetPickControl.packetPick : null);
		if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
		{
			if (!instance.hypnoses)
			{
				_hammer.Visible = true;
				AudioManager.Instance.AudioPlay("Swing");
				_hammer.SetAnimation("OpenPot", loop: false);
				pressed = true;
			}
		}
		else if (CanAddJala(towerDefenseInGamePacketShow.config))
		{
			TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
			List<TowerDefenseInGamePacketShow.ColumnPlacement> placements = ((GodotObject.IsInstanceValid(mapFeature) && mapFeature.isPlantColumn) ? packetPickControl.CollectColumnPlacements(gridPos.X) : new List<TowerDefenseInGamePacketShow.ColumnPlacement>
			{
				new TowerDefenseInGamePacketShow.ColumnPlacement(gridPos, this, "jala_vase")
			});
			if (packetPickControl.ProcessColumnPlacements(placements))
			{
				if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
				{
					LevelEditorMapEditor.instance.levelConfig.canExport = false;
				}
				else
				{
					packetPickControl.Release();
				}
			}
		}
		pressAwait = true;
		CallDeferred("_ResetPressAwait");
	}

	private async void _ResetPressAwait()
	{
		await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		pressAwait = false;
	}

	public virtual void AttackEntered()
	{
		sprite.SetAnimation("Fire", loop: false, 0.1);
	}

	public virtual void AttackProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void AttackExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Fire"))
		{
			if (clip == "Load")
			{
				Idle();
			}
		}
		else
		{
			Idle();
		}
	}

	public override async void DestroySet()
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 globalPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
		AudioManager.Instance.AudioPlay("VaseBreaking");
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(chunksEffect, gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = globalPosition;
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		foreach (TowerDefensePacketConfig jala in jalaList)
		{
			if (instance.hypnoses)
			{
				jala.overrideHypnoses = true;
			}
			TowerDefenseManager.Instance.SpawnPacket(jala, logicalGlobalPosition + new Vector2(0f, 0f - (float)groundHeight), 15.0, isFall: false);
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		foreach (TowerDefensePacketConfig jala in jalaList)
		{
			jala.overrideHypnoses = instance.hypnoses;
		}
	}

	public void HammerAnimeCompleted(string clip)
	{
		if (clip == "OpenPot")
		{
			_hammer.Visible = false;
			Destroy();
		}
	}

	public bool CanAddJala(TowerDefensePacketConfig packetConfig)
	{
		if (!isDestroy && !die && !nearDie && !pressed && GodotObject.IsInstanceValid(instance) && !instance.hologram && !instance.invincible && instance.canBeCollection && jalaList.Count < 4 && GodotObject.IsInstanceValid(packetConfig) && packetConfig.characterConfig is TowerDefensePlantConfig && (packetConfig.characterConfig.physiqueTypeFlags & 0x80) != 0)
		{
			return instance.hypnoses == packetConfig.GetHypnoses();
		}
		return false;
	}

	public void AddJala(TowerDefensePacketConfig packetConfig)
	{
		TryAddJala(packetConfig);
	}

	public bool TryAddJala(TowerDefensePacketConfig packetConfig)
	{
		if (!CanAddJala(packetConfig))
		{
			return false;
		}
		if (packetConfig.saveKey == "PlantPresentBox")
		{
			packetConfig = TowerDefenseManager.GetPacketConfig(jalaNameList.PickRandom().AsString());
		}
		else if (packetConfig.saveKey == "PlantPresentBoxGreen")
		{
			packetConfig = TowerDefenseManager.GetPacketConfig(jalaNameList.PickRandom().AsString());
			EconomyAccountId accountId = (HasEconomyOwner ? EconomyOwnerAccountId : EconomyAccountId.Local);
			long num = (GodotObject.IsInstanceValid(packetConfig) ? packetConfig.characterConfig.cost : 0);
			SunSpendReceipt receipt = null;
			if (num > 0 && TowerDefenseManager.Instance.TryBeginSunSpend(accountId, num, out receipt))
			{
				receipt?.TryCommit();
			}
			else if (num != 0L)
			{
				long sun = TowerDefenseManager.Instance.GetSun(accountId);
				if (sun >= 0)
				{
					TowerDefenseManager.Instance.SetSun(accountId, sun - num, SunTransactionReason.Cost);
				}
			}
		}
		if (instance.hypnoses)
		{
			packetConfig.overrideHypnoses = true;
		}
		packetConfig.ColdDownDecreaseAdd(this, "JalaVase", 0.25);
		sprite.SetAnimation("Load", loop: false, 0.1);
		jalaList.Add(packetConfig);
		ApplyJalaPresentation();
		ExecuteJala(packetConfig);
		return true;
	}

	public void ExecuteJala(TowerDefensePacketConfig packetConfig)
	{
		string saveKey = packetConfig.saveKey;
		if (saveKey == null)
		{
			return;
		}
		switch (saveKey.Length)
		{
		case 19:
			switch (saveKey[9])
			{
			default:
				return;
			case 'N':
				break;
			case 'C':
				if (saveKey == "PlantJalaCherryBomb")
				{
					if (gridPos.Y > 1)
					{
						TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos - new Vector2I(0, 1), 450.0);
					}
					TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos, 450.0);
					if (gridPos.Y < TowerDefenseManager.Instance.GetMapGridNum().Y)
					{
						TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos + new Vector2I(0, 1), 450.0);
					}
				}
				return;
			case 'i':
				if (saveKey == "PlantGarlicJalapeno")
				{
					TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos, 125.0);
				}
				return;
			}
			if (!(saveKey == "PlantJalaNutBowling"))
			{
				break;
			}
			goto IL_0125;
		case 14:
			switch (saveKey[9])
			{
			default:
				return;
			case 'T':
				break;
			case 'J':
				if (saveKey == "PlantJalaJoker")
				{
					TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos, 450.0);
					TowerDefenseCharacter.CreateJalapenoFireColumn(camp, gridPos, 450.0);
				}
				return;
			case 'G':
				if (saveKey == "PlantJalaGhost")
				{
					TowerDefenseCharacter.CreateJalapenoFireSlash(camp, gridPos, 750.0);
				}
				return;
			}
			if (!(saveKey == "PlantJalaTorch"))
			{
				break;
			}
			goto IL_0149;
		case 13:
			if (!(saveKey == "PlantJalapeno"))
			{
				break;
			}
			goto IL_0125;
		case 12:
			if (!(saveKey == "PlantJalaNut"))
			{
				break;
			}
			goto IL_0125;
		case 17:
			if (!(saveKey == "PlantJalapenopepe"))
			{
				break;
			}
			goto IL_0149;
		case 18:
			if (saveKey == "PlantJalaSunShroom")
			{
				Bright(0.0, 0.0, 0.5, 1.5, 0.5);
				if (JalapenoSunCreate(GetLogicalGlobalPosition(), 15L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f)) is TowerDefenseSunJalapeno towerDefenseSunJalapeno && instance.hypnoses)
				{
					towerDefenseSunJalapeno.zombieCamp = true;
				}
			}
			break;
		case 15:
			if (saveKey == "PlantJalaPurify")
			{
				TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos, 450.0);
			}
			break;
		case 16:
			break;
			IL_0125:
			TowerDefenseCharacter.CreateJalapenoFire(camp, gridPos, 450.0);
			break;
			IL_0149:
			TowerDefenseCharacter.CreateJalapenoFireColumn(camp, gridPos, 450.0);
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		Array<double> array2 = new Array<double>();
		for (int i = 0; i < jalaList.Count && i < 4; i++)
		{
			TowerDefensePacketConfig towerDefensePacketConfig = jalaList[i];
			if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				Dictionary dictionary = new Dictionary
				{
					["saveKey"] = towerDefensePacketConfig.saveKey,
					["overrideHypnoses"] = towerDefensePacketConfig.overrideHypnoses,
					["overrideCostRise"] = towerDefensePacketConfig.overrideCostRise,
					["overrideCost"] = towerDefensePacketConfig.overrideCost,
					["overridePacketCooldown"] = towerDefensePacketConfig.overridePacketCooldown,
					["overrideStartingCooldown"] = towerDefensePacketConfig.overrideStartingCooldown,
					["overrideWeight"] = towerDefensePacketConfig.overrideWeight,
					["overrideWavePointCost"] = towerDefensePacketConfig.overrideWavePointCost
				};
				TowerDefensePacketRuntimeState.Write(towerDefensePacketConfig, dictionary, "overrideSave", "canChangeCost", "changeCostList");
				array.Add(dictionary);
				array2.Add((i < timerList.Count) ? timerList[i] : 0.0);
			}
		}
		return new Dictionary
		{
			{ "isMoseIn", isMoseIn },
			{ "pressed", pressed },
			{ "over", over },
			{ "timeNeed", timeNeed },
			{ "pressAwait", pressAwait },
			{ "jalaList", array },
			{ "timerList", array2 }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isMoseIn = data.GetValueOrDefault("isMoseIn", Variant.From<bool>(false)).AsBool();
		pressed = data.GetValueOrDefault("pressed", Variant.From<bool>(false)).AsBool();
		over = data.GetValueOrDefault("over", Variant.From<bool>(false)).AsBool();
		timeNeed = data.GetValueOrDefault("timeNeed", Variant.From<double>(50.0)).AsDouble();
		pressAwait = data.GetValueOrDefault("pressAwait", Variant.From<bool>(false)).AsBool();
		jalaList.Clear();
		timerList = new Array<double> { 0.0, 0.0, 0.0, 0.0 };
		Godot.Collections.Array array = data.GetValueOrDefault("jalaList", new Godot.Collections.Array()).AsGodotArray();
		Godot.Collections.Array array2 = data.GetValueOrDefault("timerList", new Godot.Collections.Array()).AsGodotArray();
		int num = Math.Min(array.Count, 4);
		for (int i = 0; i < num; i++)
		{
			Dictionary packetData = array[i].AsGodotDictionary();
			TowerDefensePacketConfig towerDefensePacketConfig = RestoreJalaPacket(packetData);
			if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
			{
				towerDefensePacketConfig.ColdDownDecreaseAdd(this, "JalaVase", 0.25);
				jalaList.Add(towerDefensePacketConfig);
				int index = jalaList.Count - 1;
				timerList[index] = ((i < array2.Count) ? Math.Max(0.0, array2[i].AsDouble()) : 0.0);
			}
		}
		ApplyJalaPresentation();
	}

	private TowerDefensePacketConfig RestoreJalaPacket(Dictionary packetData)
	{
		string text = packetData.GetValueOrDefault("saveKey", "").AsString();
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		if (!TowerDefensePacketRuntimeState.TryCreate(text, packetData, "overrideSave", "canChangeCost", "changeCostList", out var packetConfig))
		{
			return null;
		}
		packetConfig.overrideHypnoses = packetData.GetValueOrDefault("overrideHypnoses", packetConfig.overrideHypnoses).AsBool();
		packetConfig.overrideCostRise = packetData.GetValueOrDefault("overrideCostRise", packetConfig.overrideCostRise).AsInt32();
		packetConfig.overrideCost = packetData.GetValueOrDefault("overrideCost", packetConfig.overrideCost).AsInt32();
		packetConfig.overridePacketCooldown = packetData.GetValueOrDefault("overridePacketCooldown", packetConfig.overridePacketCooldown).AsDouble();
		packetConfig.overrideStartingCooldown = packetData.GetValueOrDefault("overrideStartingCooldown", packetConfig.overrideStartingCooldown).AsDouble();
		packetConfig.overrideWeight = packetData.GetValueOrDefault("overrideWeight", packetConfig.overrideWeight).AsInt32();
		packetConfig.overrideWavePointCost = packetData.GetValueOrDefault("overrideWavePointCost", packetConfig.overrideWavePointCost).AsInt32();
		if (instance.hypnoses)
		{
			packetConfig.overrideHypnoses = true;
		}
		return packetConfig;
	}

	private void ApplyJalaPresentation()
	{
		chunksEffect = JALA_VASE_CHUNKS;
		if (GodotObject.IsInstanceValid(sprite))
		{
			switch (jalaList.Count)
			{
			case 1:
				sprite.SetAtlasReplace("JalaVase_body.png", "uid://3jm2ghlcepiq");
				break;
			case 2:
				sprite.SetAtlasReplace("JalaVase_body.png", "uid://co3xrnatu6j8a");
				break;
			case 3:
				sprite.SetAtlasReplace("JalaVase_body.png", "uid://bap8un78l21il");
				sprite.SetAtlasReplace("JalaVase_jala.png", "uid://buvtla2up6tvu");
				break;
			case 4:
				sprite.SetAtlasReplace("JalaVase_body.png", "uid://j6cc4gemmsje");
				sprite.SetAtlasReplace("JalaVase_jala.png", "uid://b57i3wamhxyrb");
				sprite.SetAtlasReplace("JalaVase_back.png", "uid://pnu58qk7jitu");
				chunksEffect = JALA_VASE_CHUNKS2;
				break;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandleVasePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pointerPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ResetPressAwait, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HammerAnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanAddJala, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddJala, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryAddJala, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteJala, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreJalaPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "packetData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyJalaPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleVasePressed && args.Count == 1)
		{
			HandleVasePressed(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ResetPressAwait && args.Count == 0)
		{
			_ResetPressAwait();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackExited && args.Count == 0)
		{
			AttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.HammerAnimeCompleted && args.Count == 1)
		{
			HammerAnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanAddJala && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAddJala(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AddJala && args.Count == 1)
		{
			AddJala(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryAddJala && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAddJala(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ExecuteJala && args.Count == 1)
		{
			ExecuteJala(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreJalaPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(RestoreJalaPacket(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyJalaPresentation && args.Count == 0)
		{
			ApplyJalaPresentation();
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
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.HandleVasePressed)
		{
			return true;
		}
		if (method == MethodName._ResetPressAwait)
		{
			return true;
		}
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackExited)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.HammerAnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CanAddJala)
		{
			return true;
		}
		if (method == MethodName.AddJala)
		{
			return true;
		}
		if (method == MethodName.TryAddJala)
		{
			return true;
		}
		if (method == MethodName.ExecuteJala)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.RestoreJalaPacket)
		{
			return true;
		}
		if (method == MethodName.ApplyJalaPresentation)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._hammer)
		{
			_hammer = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isMoseIn)
		{
			isMoseIn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pressed)
		{
			pressed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jalaNameList)
		{
			jalaNameList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.chunksEffect)
		{
			chunksEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.jalaList)
		{
			jalaList = VariantUtils.ConvertToArray<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.timerList)
		{
			timerList = VariantUtils.ConvertToArray<double>(in value);
			return true;
		}
		if (name == PropertyName.timeNeed)
		{
			timeNeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.pressAwait)
		{
			pressAwait = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._hammer)
		{
			value = VariantUtils.CreateFrom(in _hammer);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName.isMoseIn)
		{
			value = VariantUtils.CreateFrom(in isMoseIn);
			return true;
		}
		if (name == PropertyName.pressed)
		{
			value = VariantUtils.CreateFrom(in pressed);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.jalaNameList)
		{
			value = VariantUtils.CreateFrom(in jalaNameList);
			return true;
		}
		if (name == PropertyName.chunksEffect)
		{
			value = VariantUtils.CreateFrom(in chunksEffect);
			return true;
		}
		if (name == PropertyName.jalaList)
		{
			value = VariantUtils.CreateFromArray(jalaList);
			return true;
		}
		if (name == PropertyName.timerList)
		{
			value = VariantUtils.CreateFromArray(timerList);
			return true;
		}
		if (name == PropertyName.timeNeed)
		{
			value = VariantUtils.CreateFrom(in timeNeed);
			return true;
		}
		if (name == PropertyName.pressAwait)
		{
			value = VariantUtils.CreateFrom(in pressAwait);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._hammer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMoseIn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.jalaNameList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.chunksEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.jalaList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.timerList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeNeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pressAwait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._hammer, Variant.From(in _hammer));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName.isMoseIn, Variant.From(in isMoseIn));
		info.AddProperty(PropertyName.pressed, Variant.From(in pressed));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.jalaNameList, Variant.From(in jalaNameList));
		info.AddProperty(PropertyName.chunksEffect, Variant.From(in chunksEffect));
		info.AddProperty(PropertyName.jalaList, Variant.CreateFrom(jalaList));
		info.AddProperty(PropertyName.timerList, Variant.CreateFrom(timerList));
		info.AddProperty(PropertyName.timeNeed, Variant.From(in timeNeed));
		info.AddProperty(PropertyName.pressAwait, Variant.From(in pressAwait));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._hammer, out var value))
		{
			_hammer = value.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value2))
		{
			_roleStateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isMoseIn, out var value3))
		{
			isMoseIn = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pressed, out var value4))
		{
			pressed = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value5))
		{
			over = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jalaNameList, out var value6))
		{
			jalaNameList = value6.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.chunksEffect, out var value7))
		{
			chunksEffect = value7.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.jalaList, out var value8))
		{
			jalaList = value8.AsGodotArray<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.timerList, out var value9))
		{
			timerList = value9.AsGodotArray<double>();
		}
		if (info.TryGetProperty(PropertyName.timeNeed, out var value10))
		{
			timeNeed = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.pressAwait, out var value11))
		{
			pressAwait = value11.As<bool>();
		}
	}
}
