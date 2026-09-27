using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseVase.cs")]
public class TowerDefenseVase : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public static readonly StringName SetContentConfig = "SetContentConfig";

		public static readonly StringName ShouldAlwaysShowPacket = "ShouldAlwaysShowPacket";

		public static readonly StringName SetFrontShellRevealed = "SetFrontShellRevealed";

		public static readonly StringName RefreshPacketShowFromConfig = "RefreshPacketShowFromConfig";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName SetZ = "SetZ";

		public static readonly StringName HandleVasePressed = "HandleVasePressed";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName SmashDestroy = "SmashDestroy";

		public static readonly StringName MultiplayerBreak = "MultiplayerBreak";

		public static readonly StringName HammerAnimeCompleted = "HammerAnimeCompleted";

		public static readonly StringName Export = "Export";

		public new static readonly StringName BlowBack = "BlowBack";

		public static readonly StringName SetSpriteDiscardDownPos = "SetSpriteDiscardDownPos";

		public static readonly StringName SetBackSpriteDiscardDownPos = "SetBackSpriteDiscardDownPos";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public new static readonly StringName PreserveDeathTransformation = "PreserveDeathTransformation";

		public static readonly StringName backSprite = "backSprite";

		public static readonly StringName packetCanShow = "packetCanShow";

		public static readonly StringName waterLineSprite = "waterLineSprite";

		public static readonly StringName chunkParticles = "chunkParticles";

		public static readonly StringName packetBank = "packetBank";

		public static readonly StringName packetName = "packetName";

		public static readonly StringName packetConfig = "packetConfig";

		public static readonly StringName useEnterAnime = "useEnterAnime";

		public static readonly StringName showPacket = "showPacket";

		public static readonly StringName _hammer = "_hammer";

		public static readonly StringName _packetShow = "_packetShow";

		public static readonly StringName _packetName = "_packetName";

		public static readonly StringName _packetConfig = "_packetConfig";

		public static readonly StringName pressed = "pressed";

		public static readonly StringName over = "over";

		public static readonly StringName _showPacket = "_showPacket";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	public WaterEnvironmentComponent waterEnvironmentComponent;

	public VaseContentComponent vaseContentComponent;

	public EntryAnimationComponent entryAnimationComponent;

	public LightDetectionComponent lightDetectionComponent;

	private MousePressComponent _mousePressComponent;

	private AdobeAnimateSpriteBase _hammer;

	private TowerDefenseInGamePacketShow _packetShow;

	private string _packetName = "";

	private TowerDefensePacketConfig _packetConfig;

	public bool pressed;

	public bool over;

	private bool _showPacket;

	public override bool PreserveDeathTransformation => true;

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite backSprite { get; set; }

	[Export(PropertyHint.None, "")]
	public bool packetCanShow { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSpriteBase waterLineSprite { get; set; }

	[Export(PropertyHint.None, "")]
	public PackedScene chunkParticles { get; set; }

	[Export(PropertyHint.None, "")]
	public string packetBank { get; set; } = "Total";

	[Export(PropertyHint.None, "")]
	public string packetName
	{
		get
		{
			return _packetName;
		}
		set
		{
			string text = value ?? "";
			if (!(_packetName == text))
			{
				_packetName = text;
				packetConfig = ((_packetName == "") ? null : TowerDefenseManager.GetPacketConfig(_packetName));
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public TowerDefensePacketConfig packetConfig
	{
		get
		{
			return _packetConfig;
		}
		set
		{
			_packetConfig = value;
			VaseContentComponent vaseContentComponent = this.vaseContentComponent;
			if (vaseContentComponent != null && !vaseContentComponent.IsReleased)
			{
				this.vaseContentComponent.NotifyContentConfigAssigned(value);
			}
			RefreshPacketShowFromConfig();
		}
	}

	[Export(PropertyHint.None, "")]
	public bool useEnterAnime { get; set; } = true;

	public bool showPacket
	{
		get
		{
			return _showPacket;
		}
		set
		{
			_showPacket = value;
			if (GodotObject.IsInstanceValid(_packetShow))
			{
				if (!_showPacket)
				{
					_packetShow.Visible = false;
				}
				else
				{
					_packetShow.Visible = true;
				}
			}
		}
	}

	public void SetContentConfig(TowerDefensePacketConfig contentConfig)
	{
		_packetName = (GodotObject.IsInstanceValid(contentConfig) ? contentConfig.saveKey : "");
		packetConfig = contentConfig;
	}

	public bool ShouldAlwaysShowPacket()
	{
		if (Global.IsEditor)
		{
			return SceneManager.CurrentScene == "LevelEditorStage";
		}
		return false;
	}

	private void SetFrontShellRevealed(bool revealed)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.Visible = true;
			float num = (revealed ? 0f : 1f);
			if (!Mathf.IsEqualApprox(sprite.SelfModulate.A, num))
			{
				Color selfModulate = sprite.SelfModulate;
				sprite.SetRenderSelfModulate(new Color(selfModulate.R, selfModulate.G, selfModulate.B, num));
			}
		}
	}

	private void RefreshPacketShowFromConfig()
	{
		if (!IsNodeReady() || !GodotObject.IsInstanceValid(_packetShow))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(_packetConfig))
		{
			_packetShow.Init(_packetConfig);
			_packetShow.button.Visible = false;
			_packetShow.showCost = false;
			if (ShouldAlwaysShowPacket())
			{
				showPacket = true;
				SetFrontShellRevealed(revealed: true);
				_packetShow.Modulate = new Color(_packetShow.Modulate.R, _packetShow.Modulate.G, _packetShow.Modulate.B);
			}
		}
		else
		{
			_packetShow.Clear();
			_packetShow.button.Visible = false;
			_packetShow.showCost = false;
			if (ShouldAlwaysShowPacket())
			{
				showPacket = false;
				SetFrontShellRevealed(revealed: false);
				_packetShow.Modulate = new Color(_packetShow.Modulate.R, _packetShow.Modulate.G, _packetShow.Modulate.B, 0f);
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (editorPreviewMode)
		{
			_packetShow = GetNode<TowerDefenseInGamePacketShow>("%PacketShow");
			_packetShow.button.Visible = false;
			_packetShow.showCost = false;
			Callable.From(RefreshPacketShowFromConfig).CallDeferred();
		}
		else
		{
			if (Engine.IsEditorHint())
			{
				return;
			}
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.runtimeViewportCullingEnabled = false;
			}
			if (GodotObject.IsInstanceValid(backSprite))
			{
				backSprite.runtimeViewportCullingEnabled = false;
			}
			_hammer = GetNode<AdobeAnimateSpriteBase>("%Hammer");
			_hammer.OnAnimeCompleted += HammerAnimeCompleted;
			_packetShow = GetNode<TowerDefenseInGamePacketShow>("%PacketShow");
			if (GodotObject.IsInstanceValid(componentManager))
			{
				waterEnvironmentComponent = componentManager.GetRuntime<WaterEnvironmentComponent>();
				lightDetectionComponent = componentManager.GetRuntime<LightDetectionComponent>();
				this.entryAnimationComponent = componentManager.GetRuntime<EntryAnimationComponent>();
				vaseContentComponent = componentManager.GetRuntime<VaseContentComponent>();
				_mousePressComponent = componentManager.GetRuntime<MousePressComponent>();
				MousePressComponent mousePressComponent = _mousePressComponent;
				if (mousePressComponent != null && !mousePressComponent.IsReleased)
				{
					_mousePressComponent.OnPressed += HandleVasePressed;
				}
			}
			targetRegistrationComponent.canProjectileCheck = false;
			showPacket = true;
			showPacket = false;
			_packetShow.button.Visible = false;
			_packetShow.showCost = false;
			RefreshPacketShowFromConfig();
			AddToGroup("Vase", persistent: true);
			if (GodotObject.IsInstanceValid(cell) && cell.isWater)
			{
				groundHeight = 0f - waterEnvironmentComponent.waterHeight;
				inGame = false;
				waterEnvironmentComponent.inWater = true;
				inGame = true;
				shadowSprite.Visible = false;
				ShadowComponent shadowComponent = base.shadowComponent;
				if (shadowComponent != null && !shadowComponent.IsReleased)
				{
					base.shadowComponent.SetShadowVisible(visible: false);
				}
				Transform2D screenTransform = GetViewport().GetScreenTransform();
				screenTransform = new Transform2D(screenTransform.X, screenTransform.Y, Vector2.Zero);
				Vector2 logicalGlobalPosition = GetLogicalGlobalPosition(spriteGroup);
				float y = (screenTransform * (logicalGlobalPosition + new Vector2(0f, 100f))).Y;
				backSprite.SetDiscardDownPos(y);
				SetSpriteGroupShaderParameter("discardDownPos", y);
				float y2 = (screenTransform * (logicalGlobalPosition + new Vector2(0f, 48f))).Y;
				Tween tween = CreateTween();
				tween.SetParallel();
				tween.SetEase(Tween.EaseType.Out);
				tween.SetTrans(Tween.TransitionType.Cubic);
				tween.TweenMethod(Callable.From((double v) =>
				{
					SetSpriteDiscardDownPos(v);
				}), y, y2, 1.0);
				tween.TweenMethod(Callable.From((double v) =>
				{
					SetBackSpriteDiscardDownPos(v);
				}), y, y2, 1.0);
				CreateSplash();
				if (GodotObject.IsInstanceValid(waterLineSprite))
				{
					waterLineSprite.Visible = true;
				}
			}
			if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
			{
				return;
			}
			bool flag = GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.hasProgress;
			if (useEnterAnime && !flag)
			{
				EntryAnimationComponent entryAnimationComponent = this.entryAnimationComponent;
				if (entryAnimationComponent != null && !entryAnimationComponent.IsReleased)
				{
					this.entryAnimationComponent.PlayConfiguredFallBounce((float)GD.RandRange(0.25, 1.0));
				}
			}
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

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (TowerDefenseCharacter.CachedEditorHint || !GodotObject.IsInstanceValid(_packetShow) || !GodotObject.IsInstanceValid(spriteGroup) || !GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		if ((ulong)((long)TowerDefenseProcessModeDispatch.CurrentPhysicsFrame + (long)randFreshIndex) % 5uL == 0L)
		{
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
			if ((double)globalPositionForPhysicsFrame.X > groundRight - 30.0)
			{
				globalPositionForPhysicsFrame.X = (float)(groundRight - 30.0);
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			_packetShow.Position = new Vector2(_packetShow.Position.X, spriteGroup.Position.Y);
		}
		int num;
		if (GodotObject.IsInstanceValid(packetConfig))
		{
			LightDetectionComponent lightDetectionComponent = this.lightDetectionComponent;
			num = (((lightDetectionComponent != null && lightDetectionComponent.CheckShow()) || ShouldAlwaysShowPacket()) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		showPacket = (byte)num != 0;
		if (showPacket)
		{
			_packetShow.Modulate = new Color(_packetShow.Modulate.R, _packetShow.Modulate.G, _packetShow.Modulate.B, (float)Mathf.Lerp(_packetShow.Modulate.A, 1.0, delta * 2.0));
			SetFrontShellRevealed(revealed: true);
		}
		else
		{
			_packetShow.Modulate = new Color(_packetShow.Modulate.R, _packetShow.Modulate.G, _packetShow.Modulate.B, (float)Mathf.Lerp(_packetShow.Modulate.A, 0.0, delta * 5.0));
			SetFrontShellRevealed(revealed: false);
		}
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		if (useEnterAnime)
		{
			isGround = true;
			z = groundHeight;
			ySpeed = 0.0;
			if (GodotObject.IsInstanceValid(transformPoint))
			{
				Marker2D marker2D = transformPoint;
				EntryAnimationComponent entryAnimationComponent = this.entryAnimationComponent;
				marker2D.Scale = ((entryAnimationComponent != null && !entryAnimationComponent.IsReleased) ? this.entryAnimationComponent.settleScale : Vector2.One);
			}
		}
	}

	public override void SetZ()
	{
		base.SetZ();
		if (GodotObject.IsInstanceValid(backSprite))
		{
			backSprite.NotifyAncestorTransformChangedForRender();
		}
	}

	private void HandleVasePressed(Vector2 pointerPosition)
	{
		if (pressed || !GodotObject.IsInstanceValid(_hammer))
		{
			return;
		}
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(towerDefenseManager) && !(towerDefenseManager.GetMapGridPosFromMouse(pointerPosition) != gridPos))
		{
			PacketPickControl packetPickControl = towerDefenseManager.GetPacketPickControl();
			if (!GodotObject.IsInstanceValid(packetPickControl) || !GodotObject.IsInstanceValid(packetPickControl.packetPick))
			{
				_hammer.Visible = true;
				AudioManager.Instance.AudioPlay("Swing");
				_hammer.SetAnimation("OpenPot", loop: false);
				pressed = true;
			}
		}
	}

	public override void DestroySet()
	{
		VaseContentComponent vaseContentComponent = this.vaseContentComponent;
		if (vaseContentComponent != null && !vaseContentComponent.IsReleased)
		{
			this.vaseContentComponent.DestroySet();
		}
	}

	public override void SmashDestroy()
	{
		if (!isDestroy)
		{
			isDestroy = true;
			DestroySet();
			TowerDefenseManager.CurrentControl?.CleanupCharacterCell(this);
			TowerDefenseManager.Instance.CharacterUnregister(this);
			RemoveFromGroup("Character");
			QueueFree();
		}
	}

	public void MultiplayerBreak()
	{
		if (!over)
		{
			over = true;
			isDestroy = true;
			TowerDefenseManager.CurrentControl?.CleanupCharacterCell(this);
			if (GodotObject.IsInstanceValid(TowerDefenseInGameLevelControl.instance))
			{
				TowerDefenseInGameLevelControl.instance.hasSpawn = true;
			}
			VaseContentComponent vaseContentComponent = this.vaseContentComponent;
			if (vaseContentComponent != null && !vaseContentComponent.IsReleased)
			{
				this.vaseContentComponent.PlayBreakVisuals();
			}
			TowerDefenseManager.Instance.CharacterUnregister(this);
			RemoveFromGroup("Character");
			QueueFree();
		}
	}

	public void HammerAnimeCompleted(string clip)
	{
		if (clip == "OpenPot")
		{
			_hammer.Visible = false;
			if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
			{
				MultiPlayerManager.Instance.SendVaseBreakRequest(gridPos.X, gridPos.Y);
			}
			else
			{
				Destroy();
			}
		}
	}

	public TowerDefenseLevelVaseConfig Export()
	{
		TowerDefenseLevelVaseConfig towerDefenseLevelVaseConfig = new TowerDefenseLevelVaseConfig();
		towerDefenseLevelVaseConfig.gridPos = gridPos;
		if (GodotObject.IsInstanceValid(packetConfig))
		{
			towerDefenseLevelVaseConfig.packetName = packetConfig.saveKey;
		}
		switch (config.name)
		{
		case "VaseNormal":
			towerDefenseLevelVaseConfig.type = "Normal";
			break;
		case "VasePlant":
			towerDefenseLevelVaseConfig.type = "Plant";
			break;
		case "VaseZombie":
			towerDefenseLevelVaseConfig.type = "Zombie";
			break;
		}
		return towerDefenseLevelVaseConfig;
	}

	public override void BlowBack(double num, double time = 1.0)
	{
	}

	private void SetSpriteDiscardDownPos(double value)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.SetDiscardDownPos((float)value);
		}
	}

	private void SetBackSpriteDiscardDownPos(double value)
	{
		if (GodotObject.IsInstanceValid(backSprite))
		{
			backSprite.SetDiscardDownPos((float)value);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName.SetContentConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "contentConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldAlwaysShowPacket, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFrontShellRevealed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "revealed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPacketShowFromConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetZ, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleVasePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pointerPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SmashDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MultiplayerBreak, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HammerAnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlowBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpriteDiscardDownPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBackSpriteDiscardDownPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetContentConfig && args.Count == 1)
		{
			SetContentConfig(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldAlwaysShowPacket && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAlwaysShowPacket());
			return true;
		}
		if (method == MethodName.SetFrontShellRevealed && args.Count == 1)
		{
			SetFrontShellRevealed(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketShowFromConfig && args.Count == 0)
		{
			RefreshPacketShowFromConfig();
			ret = default;
			return true;
		}
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.SetZ && args.Count == 0)
		{
			SetZ();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleVasePressed && args.Count == 1)
		{
			HandleVasePressed(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.SmashDestroy && args.Count == 0)
		{
			SmashDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.MultiplayerBreak && args.Count == 0)
		{
			MultiplayerBreak();
			ret = default;
			return true;
		}
		if (method == MethodName.HammerAnimeCompleted && args.Count == 1)
		{
			HammerAnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelVaseConfig>(Export());
			return true;
		}
		if (method == MethodName.BlowBack && args.Count == 2)
		{
			BlowBack(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpriteDiscardDownPos && args.Count == 1)
		{
			SetSpriteDiscardDownPos(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetBackSpriteDiscardDownPos && args.Count == 1)
		{
			SetBackSpriteDiscardDownPos(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetContentConfig)
		{
			return true;
		}
		if (method == MethodName.ShouldAlwaysShowPacket)
		{
			return true;
		}
		if (method == MethodName.SetFrontShellRevealed)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketShowFromConfig)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.SetZ)
		{
			return true;
		}
		if (method == MethodName.HandleVasePressed)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.SmashDestroy)
		{
			return true;
		}
		if (method == MethodName.MultiplayerBreak)
		{
			return true;
		}
		if (method == MethodName.HammerAnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.BlowBack)
		{
			return true;
		}
		if (method == MethodName.SetSpriteDiscardDownPos)
		{
			return true;
		}
		if (method == MethodName.SetBackSpriteDiscardDownPos)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.backSprite)
		{
			backSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.packetCanShow)
		{
			packetCanShow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.waterLineSprite)
		{
			waterLineSprite = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName.chunkParticles)
		{
			chunkParticles = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.packetBank)
		{
			packetBank = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			packetName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.packetConfig)
		{
			packetConfig = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.useEnterAnime)
		{
			useEnterAnime = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showPacket)
		{
			showPacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hammer)
		{
			_hammer = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._packetShow)
		{
			_packetShow = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			_packetName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._packetConfig)
		{
			_packetConfig = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
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
		if (name == PropertyName._showPacket)
		{
			_showPacket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.PreserveDeathTransformation)
		{
			from = PreserveDeathTransformation;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.backSprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSprite>(backSprite);
			return true;
		}
		if (name == PropertyName.packetCanShow)
		{
			from = packetCanShow;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.waterLineSprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSpriteBase>(waterLineSprite);
			return true;
		}
		if (name == PropertyName.chunkParticles)
		{
			value = VariantUtils.CreateFrom<PackedScene>(chunkParticles);
			return true;
		}
		string from2;
		if (name == PropertyName.packetBank)
		{
			from2 = packetBank;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.packetName)
		{
			from2 = packetName;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.packetConfig)
		{
			value = VariantUtils.CreateFrom<TowerDefensePacketConfig>(packetConfig);
			return true;
		}
		if (name == PropertyName.useEnterAnime)
		{
			from = useEnterAnime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.showPacket)
		{
			from = showPacket;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._hammer)
		{
			value = VariantUtils.CreateFrom(in _hammer);
			return true;
		}
		if (name == PropertyName._packetShow)
		{
			value = VariantUtils.CreateFrom(in _packetShow);
			return true;
		}
		if (name == PropertyName._packetName)
		{
			value = VariantUtils.CreateFrom(in _packetName);
			return true;
		}
		if (name == PropertyName._packetConfig)
		{
			value = VariantUtils.CreateFrom(in _packetConfig);
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
		if (name == PropertyName._showPacket)
		{
			value = VariantUtils.CreateFrom(in _showPacket);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.PreserveDeathTransformation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hammer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.backSprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.packetCanShow, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.waterLineSprite, PropertyHint.NodeType, "AdobeAnimateSpriteBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.chunkParticles, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.packetBank, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._packetName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.packetName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetConfig, PropertyHint.ResourceType, "TowerDefensePacketConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useEnterAnime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.backSprite, Variant.From<AdobeAnimateSprite>(backSprite));
		info.AddProperty(PropertyName.packetCanShow, Variant.From<bool>(packetCanShow));
		info.AddProperty(PropertyName.waterLineSprite, Variant.From<AdobeAnimateSpriteBase>(waterLineSprite));
		info.AddProperty(PropertyName.chunkParticles, Variant.From<PackedScene>(chunkParticles));
		info.AddProperty(PropertyName.packetBank, Variant.From<string>(packetBank));
		info.AddProperty(PropertyName.packetName, Variant.From<string>(packetName));
		info.AddProperty(PropertyName.packetConfig, Variant.From<TowerDefensePacketConfig>(packetConfig));
		info.AddProperty(PropertyName.useEnterAnime, Variant.From<bool>(useEnterAnime));
		info.AddProperty(PropertyName.showPacket, Variant.From<bool>(showPacket));
		info.AddProperty(PropertyName._hammer, Variant.From(in _hammer));
		info.AddProperty(PropertyName._packetShow, Variant.From(in _packetShow));
		info.AddProperty(PropertyName._packetName, Variant.From(in _packetName));
		info.AddProperty(PropertyName._packetConfig, Variant.From(in _packetConfig));
		info.AddProperty(PropertyName.pressed, Variant.From(in pressed));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName._showPacket, Variant.From(in _showPacket));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.backSprite, out var value))
		{
			backSprite = value.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.packetCanShow, out var value2))
		{
			packetCanShow = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.waterLineSprite, out var value3))
		{
			waterLineSprite = value3.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName.chunkParticles, out var value4))
		{
			chunkParticles = value4.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.packetBank, out var value5))
		{
			packetBank = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.packetName, out var value6))
		{
			packetName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.packetConfig, out var value7))
		{
			packetConfig = value7.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.useEnterAnime, out var value8))
		{
			useEnterAnime = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showPacket, out var value9))
		{
			showPacket = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hammer, out var value10))
		{
			_hammer = value10.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._packetShow, out var value11))
		{
			_packetShow = value11.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._packetName, out var value12))
		{
			_packetName = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName._packetConfig, out var value13))
		{
			_packetConfig = value13.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.pressed, out var value14))
		{
			pressed = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value15))
		{
			over = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showPacket, out var value16))
		{
			_showPacket = value16.As<bool>();
		}
	}
}
