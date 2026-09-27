using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/DayVase/Scene/TowerDefensePlantDayVase.cs")]
public class TowerDefensePlantDayVase : TowerDefensePlant
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

		public static readonly StringName TryStartDayNightSwitch = "TryStartDayNightSwitch";

		public static readonly StringName AttackProcessing = "AttackProcessing";

		public static readonly StringName AttackExited = "AttackExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName Change = "Change";

		public static readonly StringName Break = "Break";

		public static readonly StringName HammerAnimeCompleted = "HammerAnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _hammer = "_hammer";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName isMoseIn = "isMoseIn";

		public static readonly StringName pressed = "pressed";

		public static readonly StringName over = "over";

		public static readonly StringName chunksEffect = "chunksEffect";

		public static readonly StringName pressAwait = "pressAwait";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _DAY_VASE_CHUNKS;

	private AdobeAnimateSpriteBase _hammer;

	private MousePressComponent _mousePressComponent;

	private StateHandle _attackState;

	private bool _roleStateSignalsConnected;

	public bool isMoseIn;

	public bool pressed;

	public bool over;

	public PackedScene chunksEffect;

	public bool pressAwait;

	private static PackedScene DAY_VASE_CHUNKS => _DAY_VASE_CHUNKS ?? (_DAY_VASE_CHUNKS = GD.Load<PackedScene>("uid://ctsxe8qhievw0"));

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
			chunksEffect = DAY_VASE_CHUNKS;
			instance.ClearHitpointsEmptyListeners();
			instance.hitpointsEmpty += Change;
			_attackState = StateMachine?.GetStateById("plant.day_vase.attack");
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
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			isMoseIn = _mousePressComponent?.IsMouseInside ?? false;
			if (!Global.IsEditor || !(SceneManager.CurrentScene == "LevelEditorStage"))
			{
				TowerDefenseManager.Instance.IsGameRunning();
			}
		}
	}

	private void HandleVasePressed(Vector2 pointerPosition)
	{
		if (pressAwait || instance.hypnoses || pressed)
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
				pressAwait = true;
				CallDeferred("_ResetPressAwait");
			}
		}
	}

	private async void _ResetPressAwait()
	{
		await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		pressAwait = false;
	}

	public virtual void AttackEntered()
	{
		if (TowerDefenseManager.GetMapIsNight())
		{
			sprite.SetAnimation("Day", loop: false, 0.1);
		}
		else
		{
			sprite.SetAnimation("Night", loop: false, 0.1);
		}
		if (!over)
		{
			TryStartDayNightSwitch();
			over = true;
		}
	}

	private void TryStartDayNightSwitch()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature) || !mapFeature.isChange)
		{
			TowerDefenseManager.Instance.MapDayNightSwitch(5.0);
		}
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
		if (clip == "Day" || clip == "Night")
		{
			TowerDefenseManager.Instance.CharacterUnregister(this);
			RemoveFromGroup("Character");
			QueueFree();
		}
	}

	public void Change()
	{
		pressed = true;
		SendStateEvent("ToAttack");
		Break();
		Destroy(freeInstance: false);
	}

	public async void Break()
	{
		AudioManager.Instance.AudioPlay("VaseBreaking");
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(chunksEffect, gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	public void HammerAnimeCompleted(string clip)
	{
		if (clip == "OpenPot")
		{
			_hammer.Visible = false;
			Change();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "isMoseIn", isMoseIn },
			{ "pressed", pressed },
			{ "over", over },
			{ "pressAwait", pressAwait }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isMoseIn = data.GetValueOrDefault("isMoseIn", Variant.From<bool>(false)).AsBool();
		pressed = data.GetValueOrDefault("pressed", Variant.From<bool>(false)).AsBool();
		over = data.GetValueOrDefault("over", Variant.From<bool>(false)).AsBool();
		pressAwait = data.GetValueOrDefault("pressAwait", Variant.From<bool>(false)).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
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
			new MethodInfo(MethodName.TryStartDayNightSwitch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Change, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Break, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HammerAnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.TryStartDayNightSwitch && args.Count == 0)
		{
			TryStartDayNightSwitch();
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
		if (method == MethodName.Change && args.Count == 0)
		{
			Change();
			ret = default;
			return true;
		}
		if (method == MethodName.Break && args.Count == 0)
		{
			Break();
			ret = default;
			return true;
		}
		if (method == MethodName.HammerAnimeCompleted && args.Count == 1)
		{
			HammerAnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.TryStartDayNightSwitch)
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
		if (method == MethodName.Change)
		{
			return true;
		}
		if (method == MethodName.Break)
		{
			return true;
		}
		if (method == MethodName.HammerAnimeCompleted)
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
		if (name == PropertyName.chunksEffect)
		{
			chunksEffect = VariantUtils.ConvertTo<PackedScene>(in value);
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
		if (name == PropertyName.chunksEffect)
		{
			value = VariantUtils.CreateFrom(in chunksEffect);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.chunksEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
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
		info.AddProperty(PropertyName.chunksEffect, Variant.From(in chunksEffect));
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
		if (info.TryGetProperty(PropertyName.chunksEffect, out var value6))
		{
			chunksEffect = value6.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.pressAwait, out var value7))
		{
			pressAwait = value7.As<bool>();
		}
	}
}
