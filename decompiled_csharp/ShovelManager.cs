using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/ShovelManager/ShovelManager.cs")]
public class ShovelManager : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnGlobalFeatureChanged = "OnGlobalFeatureChanged";

		public static readonly StringName CanUseShovel = "CanUseShovel";

		public static readonly StringName UpdateModeVisibility = "UpdateModeVisibility";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName UpdateShovelSlotDisplay = "UpdateShovelSlotDisplay";

		public static readonly StringName ApplyShovelPickState = "ApplyShovelPickState";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName ShovelButtonPressed = "ShovelButtonPressed";

		public static readonly StringName OnPressDebounceTimeout = "OnPressDebounceTimeout";

		public static readonly StringName ClearPressDebounce = "ClearPressDebounce";

		public static readonly StringName Init = "Init";

		public static readonly StringName PickShovel = "PickShovel";

		public static readonly StringName ProcessShovelPick = "ProcessShovelPick";

		public static readonly StringName SetToolHighlight = "SetToolHighlight";

		public static readonly StringName ShovelRelease = "ShovelRelease";

		public static readonly StringName ShovelReset = "ShovelReset";

		public static readonly StringName Start = "Start";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName shovelButton = "shovelButton";

		public static readonly StringName shovelSprite = "shovelSprite";

		public static readonly StringName shovelConfig = "shovelConfig";

		public static readonly StringName shovelShow = "shovelShow";

		public static readonly StringName shovelPressedAwait = "shovelPressedAwait";

		public static readonly StringName mapControl = "mapControl";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName shovelPick = "shovelPick";

		public static readonly StringName mapShovelSprite = "mapShovelSprite";

		public static readonly StringName lastShovelPlant = "lastShovelPlant";

		public static readonly StringName _pressDebounceTimer = "_pressDebounceTimer";

		public static readonly StringName _globalFeatureManager = "_globalFeatureManager";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string ToolHighlightShaderParameter = "toolHighlightStrength";

	private const float ToolHighlightStrength = 0.5f;

	public TextureButton shovelButton;

	public Sprite2D shovelSprite;

	public ShovelConfig shovelConfig;

	public bool shovelShow;

	public bool shovelPressedAwait;

	public TowerDefenseMapControl mapControl;

	public TowerDefenseBattleFeatureMap mapFeature;

	public bool shovelPick;

	public Sprite2D mapShovelSprite;

	public TowerDefenseCharacter lastShovelPlant;

	private SceneTreeTimer _pressDebounceTimer;

	private Action _pressDebounceHandler;

	private GlobalFeatureManager _globalFeatureManager;

	public override void _Ready()
	{
		shovelButton = GetNode<TextureButton>("%ShovelButton");
		shovelSprite = GetNode<Sprite2D>("%ShovelSprite");
		_globalFeatureManager = GlobalFeatureManager.Instance;
		if (_globalFeatureManager != null)
		{
			_globalFeatureManager.FeatureChanged += OnGlobalFeatureChanged;
		}
		if (GodotObject.IsInstanceValid(shovelButton))
		{
			shovelButton.ActionMode = BaseButton.ActionModeEnum.Press;
			shovelButton.Pressed += ShovelButtonPressed;
		}
		string text = GameSaveManager.Instance.GetKeyValue("CurrentShovel").AsString();
		if (text == "")
		{
			text = "ShovelDefault";
		}
		shovelConfig = TowerDefenseManager.GetShovel(text);
		if (GodotObject.IsInstanceValid(shovelSprite))
		{
			shovelSprite.Texture = shovelConfig.texture;
		}
		mapShovelSprite = new Sprite2D();
		mapShovelSprite.Visible = false;
		if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode))
		{
			mapControl.spriteNode.AddChild(mapShovelSprite, forceReadableName: false, InternalMode.Disabled);
		}
		if (GodotObject.IsInstanceValid(shovelConfig))
		{
			mapShovelSprite.Texture = shovelConfig.texture;
		}
		SetPhysicsProcess(enable: false);
		Callable.From(UpdateModeVisibility).CallDeferred();
	}

	private void OnGlobalFeatureChanged(string featureId, int _oldValue, int _newValue)
	{
		if (featureId == "Shovel")
		{
			UpdateModeVisibility();
		}
	}

	private bool CanUseShovel()
	{
		CommandManager instance = CommandManager.Instance;
		if (instance == null || !instance.debugPacketOpenAll)
		{
			GlobalFeatureManager globalFeatureManager = _globalFeatureManager;
			if (globalFeatureManager == null || !globalFeatureManager.IsUnlocked("Shovel"))
			{
				goto IL_0049;
			}
		}
		if (!TowerDefenseManager.Instance.IsIZMMode())
		{
			return !TowerDefenseManager.Instance.IsIZM2Mode();
		}
		goto IL_0049;
		IL_0049:
		return false;
	}

	private void UpdateModeVisibility()
	{
		if (IsInsideTree() && GodotObject.IsInstanceValid(shovelButton) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			bool flag = CanUseShovel();
			shovelButton.Visible = flag;
			if (!flag)
			{
				ShovelReset();
			}
		}
	}

	public override void _ExitTree()
	{
		if (_globalFeatureManager != null)
		{
			_globalFeatureManager.FeatureChanged -= OnGlobalFeatureChanged;
		}
		_globalFeatureManager = null;
		ClearPressDebounce();
		shovelPressedAwait = false;
		if (GodotObject.IsInstanceValid(shovelButton))
		{
			shovelButton.Pressed -= ShovelButtonPressed;
		}
		ShovelReset();
		if (GodotObject.IsInstanceValid(mapShovelSprite))
		{
			mapShovelSprite.QueueFree();
		}
		mapShovelSprite = null;
		mapControl = null;
		mapFeature = null;
		shovelConfig = null;
		base._ExitTree();
	}

	private void UpdateShovelSlotDisplay()
	{
		if (GodotObject.IsInstanceValid(shovelSprite))
		{
			shovelSprite.Visible = !shovelPick;
			if (shovelSprite.Visible && GodotObject.IsInstanceValid(shovelSprite.Texture))
			{
				shovelSprite.Scale = Vector2.One * 80f / shovelSprite.Texture.GetWidth();
			}
		}
	}

	private void ApplyShovelPickState(bool picked)
	{
		shovelPick = picked;
		if (GodotObject.IsInstanceValid(shovelButton))
		{
			shovelButton.SetPressedNoSignal(picked);
		}
		if (GodotObject.IsInstanceValid(mapShovelSprite))
		{
			mapShovelSprite.Visible = picked;
		}
		UpdateShovelSlotDisplay();
	}

	public override void _Input(InputEvent _event)
	{
		if (GodotObject.IsInstanceValid(shovelButton) && CanUseShovel() && !shovelPressedAwait && Input.IsActionJustPressed("Shovel") && !TowerDefenseManager.Instance.IsIZMMode() && !TowerDefenseManager.Instance.IsIZM2Mode())
		{
			shovelButton.ButtonPressed = !shovelButton.ButtonPressed;
			ShovelButtonPressed();
		}
	}

	public void ShovelButtonPressed()
	{
		if (!CanUseShovel())
		{
			ApplyShovelPickState(picked: false);
		}
		else if (!shovelShow && !TowerDefenseManager.Instance.currentControl.isGameRunning)
		{
			ApplyShovelPickState(picked: false);
		}
		else if (!shovelPressedAwait)
		{
			PickShovel(shovelButton.ButtonPressed);
			shovelPressedAwait = true;
			ClearPressDebounce();
			_pressDebounceTimer = GetTree().CreateTimer(0.1, processAlways: false);
			_pressDebounceHandler = OnPressDebounceTimeout;
			_pressDebounceTimer.Timeout += _pressDebounceHandler;
		}
	}

	private void OnPressDebounceTimeout()
	{
		ClearPressDebounce();
		if (IsInsideTree())
		{
			shovelPressedAwait = false;
		}
	}

	private void ClearPressDebounce()
	{
		if (GodotObject.IsInstanceValid(_pressDebounceTimer) && _pressDebounceHandler != null)
		{
			_pressDebounceTimer.Timeout -= _pressDebounceHandler;
		}
		_pressDebounceTimer = null;
		_pressDebounceHandler = null;
	}

	public void Init(TowerDefenseMapControl _mapControl, TowerDefenseBattleFeatureMap _mapFeature)
	{
		mapControl = _mapControl;
		mapFeature = _mapFeature;
		if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode) && GodotObject.IsInstanceValid(mapShovelSprite) && mapShovelSprite.GetParent() == null)
		{
			mapControl.spriteNode.AddChild(mapShovelSprite, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void PickShovel(bool open)
	{
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.packetPickControl))
		{
			mapFeature.packetPickControl.PacketPickRelease();
		}
		if (open)
		{
			AudioManager.Instance.AudioPlay("Shovel");
			if (GodotObject.IsInstanceValid(mapShovelSprite))
			{
				mapShovelSprite.Position = new Vector2(-100f, -100f);
			}
		}
		else
		{
			AudioManager.Instance.AudioPlay("ShovelDeny");
		}
		ApplyShovelPickState(open);
	}

	public void ProcessShovelPick(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		if (mapShovelSprite != null)
		{
			mapShovelSprite.Visible = true;
			if (mapShovelSprite.Texture != null)
			{
				mapShovelSprite.Scale = Vector2.One * 80f / mapShovelSprite.Texture.GetWidth();
			}
			if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode))
			{
				mapControl.PositionMapOverlayAtWorldPoint(mapShovelSprite, mousePos, new Vector2(35f, -35f));
			}
		}
		if (TowerDefenseManager.Instance.CheckMapGridPosIn(gridPos))
		{
			double groundHeight = mapFeature.GetGroundHeight(cell);
			Vector2 mapCellPos = TowerDefenseManager.Instance.GetMapCellPos(gridPos);
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			double percentage = (mousePos - mapCellPos + new Vector2(0f, (float)groundHeight)).Y / mapGridSize.Y;
			TowerDefenseCharacter shovelCharacter = cell.GetShovelCharacter(percentage, shovelConfig);
			if (shovelCharacter != lastShovelPlant)
			{
				SetToolHighlight(lastShovelPlant, 0f);
				lastShovelPlant = shovelCharacter;
				SetToolHighlight(lastShovelPlant, 0.5f);
			}
			if (!cell.CanShovel(percentage, shovelConfig) || !mapControl.IsConfirmInput())
			{
				return;
			}
			AudioManager.Instance.AudioPlay("ShovelDig");
			if (Global.Instance.isMultiplayerMode)
			{
				MultiPlayerManager.Instance.SendRemovePlant(gridPos.X, gridPos.Y);
				cell.Shovel(shovelConfig, percentage);
				mapFeature.packetPickControl.Release();
			}
			else if (!Global.Instance.isEditor || SceneManager.Instance.currentScene != "LevelEditorStage")
			{
				cell.Shovel(shovelConfig, percentage);
				mapFeature.packetPickControl.Release();
			}
			else
			{
				cell.Shovel(TowerDefenseManager.GetShovel("ShovelDefault"), percentage);
				if (GodotObject.IsInstanceValid(LevelEditorMapEditor.instance) && GodotObject.IsInstanceValid(LevelEditorMapEditor.instance.levelConfig))
				{
					LevelEditorMapEditor.instance.levelConfig.canExport = false;
				}
			}
			lastShovelPlant = null;
		}
		else if (GodotObject.IsInstanceValid(lastShovelPlant))
		{
			SetToolHighlight(lastShovelPlant, 0f);
			lastShovelPlant = null;
		}
	}

	private static void SetToolHighlight(TowerDefenseCharacter character, float strength)
	{
		if (GodotObject.IsInstanceValid(character))
		{
			character.SetSpriteGroupShaderParameter("toolHighlightStrength", strength);
		}
	}

	public void ShovelRelease()
	{
		if (shovelPick)
		{
			AudioManager.Instance.AudioPlay("ShovelDeny");
		}
		ApplyShovelPickState(picked: false);
		if (GodotObject.IsInstanceValid(lastShovelPlant))
		{
			SetToolHighlight(lastShovelPlant, 0f);
			lastShovelPlant = null;
		}
	}

	public void ShovelReset()
	{
		ApplyShovelPickState(picked: false);
		if (GodotObject.IsInstanceValid(lastShovelPlant))
		{
			SetToolHighlight(lastShovelPlant, 0f);
			lastShovelPlant = null;
		}
	}

	public void Start()
	{
		if (GodotObject.IsInstanceValid(shovelButton))
		{
			UpdateModeVisibility();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGlobalFeatureChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "featureId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_oldValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanUseShovel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateModeVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateShovelSlotDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyShovelPickState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "picked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShovelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPressDebounceTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPressDebounce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "_mapFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PickShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessShovelPick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetToolHighlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "strength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShovelRelease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShovelReset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Start, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnGlobalFeatureChanged && args.Count == 3)
		{
			OnGlobalFeatureChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanUseShovel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanUseShovel());
			return true;
		}
		if (method == MethodName.UpdateModeVisibility && args.Count == 0)
		{
			UpdateModeVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateShovelSlotDisplay && args.Count == 0)
		{
			UpdateShovelSlotDisplay();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyShovelPickState && args.Count == 1)
		{
			ApplyShovelPickState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShovelButtonPressed && args.Count == 0)
		{
			ShovelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPressDebounceTimeout && args.Count == 0)
		{
			OnPressDebounceTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPressDebounce && args.Count == 0)
		{
			ClearPressDebounce();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PickShovel && args.Count == 1)
		{
			PickShovel(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessShovelPick && args.Count == 3)
		{
			ProcessShovelPick(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetToolHighlight && args.Count == 2)
		{
			SetToolHighlight(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShovelRelease && args.Count == 0)
		{
			ShovelRelease();
			ret = default;
			return true;
		}
		if (method == MethodName.ShovelReset && args.Count == 0)
		{
			ShovelReset();
			ret = default;
			return true;
		}
		if (method == MethodName.Start && args.Count == 0)
		{
			Start();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetToolHighlight && args.Count == 2)
		{
			SetToolHighlight(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.OnGlobalFeatureChanged)
		{
			return true;
		}
		if (method == MethodName.CanUseShovel)
		{
			return true;
		}
		if (method == MethodName.UpdateModeVisibility)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.UpdateShovelSlotDisplay)
		{
			return true;
		}
		if (method == MethodName.ApplyShovelPickState)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.ShovelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnPressDebounceTimeout)
		{
			return true;
		}
		if (method == MethodName.ClearPressDebounce)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.PickShovel)
		{
			return true;
		}
		if (method == MethodName.ProcessShovelPick)
		{
			return true;
		}
		if (method == MethodName.SetToolHighlight)
		{
			return true;
		}
		if (method == MethodName.ShovelRelease)
		{
			return true;
		}
		if (method == MethodName.ShovelReset)
		{
			return true;
		}
		if (method == MethodName.Start)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.shovelButton)
		{
			shovelButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName.shovelSprite)
		{
			shovelSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.shovelConfig)
		{
			shovelConfig = VariantUtils.ConvertTo<ShovelConfig>(in value);
			return true;
		}
		if (name == PropertyName.shovelShow)
		{
			shovelShow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shovelPressedAwait)
		{
			shovelPressedAwait = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mapControl)
		{
			mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.shovelPick)
		{
			shovelPick = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mapShovelSprite)
		{
			mapShovelSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.lastShovelPlant)
		{
			lastShovelPlant = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._pressDebounceTimer)
		{
			_pressDebounceTimer = VariantUtils.ConvertTo<SceneTreeTimer>(in value);
			return true;
		}
		if (name == PropertyName._globalFeatureManager)
		{
			_globalFeatureManager = VariantUtils.ConvertTo<GlobalFeatureManager>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.shovelButton)
		{
			value = VariantUtils.CreateFrom(in shovelButton);
			return true;
		}
		if (name == PropertyName.shovelSprite)
		{
			value = VariantUtils.CreateFrom(in shovelSprite);
			return true;
		}
		if (name == PropertyName.shovelConfig)
		{
			value = VariantUtils.CreateFrom(in shovelConfig);
			return true;
		}
		if (name == PropertyName.shovelShow)
		{
			value = VariantUtils.CreateFrom(in shovelShow);
			return true;
		}
		if (name == PropertyName.shovelPressedAwait)
		{
			value = VariantUtils.CreateFrom(in shovelPressedAwait);
			return true;
		}
		if (name == PropertyName.mapControl)
		{
			value = VariantUtils.CreateFrom(in mapControl);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName.shovelPick)
		{
			value = VariantUtils.CreateFrom(in shovelPick);
			return true;
		}
		if (name == PropertyName.mapShovelSprite)
		{
			value = VariantUtils.CreateFrom(in mapShovelSprite);
			return true;
		}
		if (name == PropertyName.lastShovelPlant)
		{
			value = VariantUtils.CreateFrom(in lastShovelPlant);
			return true;
		}
		if (name == PropertyName._pressDebounceTimer)
		{
			value = VariantUtils.CreateFrom(in _pressDebounceTimer);
			return true;
		}
		if (name == PropertyName._globalFeatureManager)
		{
			value = VariantUtils.CreateFrom(in _globalFeatureManager);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shovelShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shovelPressedAwait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shovelPick, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapShovelSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.lastShovelPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pressDebounceTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._globalFeatureManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shovelButton, Variant.From(in shovelButton));
		info.AddProperty(PropertyName.shovelSprite, Variant.From(in shovelSprite));
		info.AddProperty(PropertyName.shovelConfig, Variant.From(in shovelConfig));
		info.AddProperty(PropertyName.shovelShow, Variant.From(in shovelShow));
		info.AddProperty(PropertyName.shovelPressedAwait, Variant.From(in shovelPressedAwait));
		info.AddProperty(PropertyName.mapControl, Variant.From(in mapControl));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.shovelPick, Variant.From(in shovelPick));
		info.AddProperty(PropertyName.mapShovelSprite, Variant.From(in mapShovelSprite));
		info.AddProperty(PropertyName.lastShovelPlant, Variant.From(in lastShovelPlant));
		info.AddProperty(PropertyName._pressDebounceTimer, Variant.From(in _pressDebounceTimer));
		info.AddProperty(PropertyName._globalFeatureManager, Variant.From(in _globalFeatureManager));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shovelButton, out var value))
		{
			shovelButton = value.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName.shovelSprite, out var value2))
		{
			shovelSprite = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.shovelConfig, out var value3))
		{
			shovelConfig = value3.As<ShovelConfig>();
		}
		if (info.TryGetProperty(PropertyName.shovelShow, out var value4))
		{
			shovelShow = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shovelPressedAwait, out var value5))
		{
			shovelPressedAwait = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mapControl, out var value6))
		{
			mapControl = value6.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value7))
		{
			mapFeature = value7.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.shovelPick, out var value8))
		{
			shovelPick = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mapShovelSprite, out var value9))
		{
			mapShovelSprite = value9.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.lastShovelPlant, out var value10))
		{
			lastShovelPlant = value10.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._pressDebounceTimer, out var value11))
		{
			_pressDebounceTimer = value11.As<SceneTreeTimer>();
		}
		if (info.TryGetProperty(PropertyName._globalFeatureManager, out var value12))
		{
			_globalFeatureManager = value12.As<GlobalFeatureManager>();
		}
	}
}
