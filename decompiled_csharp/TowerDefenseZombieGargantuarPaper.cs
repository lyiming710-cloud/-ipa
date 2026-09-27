using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/GargantuarPaper/Scene/TowerDefenseZombieGargantuarPaper.cs")]
public class TowerDefenseZombieGargantuarPaper : TowerDefenseZombieGargantuarBase
{
	public new class MethodName : TowerDefenseZombieGargantuarBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public static readonly StringName GaspEntered = "GaspEntered";

		public static readonly StringName GaspProcessing = "GaspProcessing";

		public static readonly StringName GaspExited = "GaspExited";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ApplyAngryPresentation = "ApplyAngryPresentation";

		public static readonly StringName ApplyAngryHitpointBonus = "ApplyAngryHitpointBonus";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombieGargantuarBase.PropertyName
	{
		public static readonly StringName angry = "angry";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _angry = "_angry";

		public static readonly StringName _gasping = "_gasping";

		public static readonly StringName _keepAliveBeforeGasp = "_keepAliveBeforeGasp";

		public static readonly StringName _damageHitpointsFloorBeforeGasp = "_damageHitpointsFloorBeforeGasp";

		public static readonly StringName _angryBonusApplied = "_angryBonusApplied";
	}

	public new class SignalName : TowerDefenseZombieGargantuarBase.SignalName
	{
	}

	private const string GaspClip = "Gasp";

	private const string AngryWalkClip = "WalkNoPaper";

	private const string AngryAttackClip = "SmashNoPaper";

	private const string AngryDeathClip = "DeathNoPaper";

	private const double AngryTimeScale = 7.5;

	private const double AngryBonusHitpoints = 3000.0;

	private StateHandle _gaspStateHandle;

	private bool _stateSignalsConnected;

	private bool _angry;

	private bool _gasping;

	private bool _keepAliveBeforeGasp;

	private double _damageHitpointsFloorBeforeGasp;

	private bool _angryBonusApplied;

	public bool angry
	{
		get
		{
			return _angry;
		}
		set
		{
			_angry = value;
			ApplyAngryPresentation();
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			impThrowFlag = false;
			ConnectStateSignals();
		}
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		base._ExitTree();
	}

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_gaspStateHandle = StateMachine?.GetStateById("zombie.gargantuar_paper.gasp");
			StateHandle gaspStateHandle = _gaspStateHandle;
			if (gaspStateHandle != null && gaspStateHandle.IsValid)
			{
				_gaspStateHandle.Entered += GaspEntered;
				_gaspStateHandle.Exited += GaspExited;
				_gaspStateHandle.PhysicsProcessing += GaspProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_gaspStateHandle != null)
			{
				_gaspStateHandle.Entered -= GaspEntered;
				_gaspStateHandle.Exited -= GaspExited;
				_gaspStateHandle.PhysicsProcessing -= GaspProcessing;
			}
			_gaspStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public virtual void GaspEntered()
	{
		if (GodotObject.IsInstanceValid(instance))
		{
			if (!_gasping)
			{
				_keepAliveBeforeGasp = instance.keepAlive;
				_damageHitpointsFloorBeforeGasp = instance.damageHitpointsFloor;
			}
			instance.keepAlive = true;
			instance.damageHitpointsFloor = Math.Max(1.0, _damageHitpointsFloorBeforeGasp);
		}
		_gasping = true;
		sprite.SetAnimation("Gasp", loop: false);
		AudioManager.Instance.AudioPlay("NewspaperRip");
	}

	public virtual void GaspProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void GaspExited()
	{
		_gasping = false;
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.keepAlive = _keepAliveBeforeGasp;
			instance.damageHitpointsFloor = _damageHitpointsFloorBeforeGasp;
		}
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Paper" && !_angry && !_gasping && !die && !isDestroy && GodotObject.IsInstanceValid(instance) && !instance.die && instance.hitpoints > 0.0)
		{
			SendStateEvent("ToGasp");
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Gasp" && _gasping)
		{
			StateHandle gaspStateHandle = _gaspStateHandle;
			if (gaspStateHandle != null && gaspStateHandle.IsActive && !die && !isDestroy && GodotObject.IsInstanceValid(instance) && !instance.die)
			{
				AudioManager.Instance.AudioPlay("NewspaperRarrgh");
				timeScaleInit = 7.5;
				angry = true;
				ApplyAngryHitpointBonus();
				Walk();
			}
		}
	}

	private void ApplyAngryPresentation()
	{
		if (_angry)
		{
			walkAnimeClip = "WalkNoPaper";
			swimAnimeClip = "WalkNoPaper";
			attackAnimeClip = "SmashNoPaper";
			attackWaterAnimeClip = "SmashNoPaper";
			dieAnimeClip = "DeathNoPaper";
		}
	}

	private void ApplyAngryHitpointBonus()
	{
		if (!_angryBonusApplied && GodotObject.IsInstanceValid(instance) && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			_angryBonusApplied = true;
			instance.hitpointsSave += 3000.0;
			instance.hitpoints += 3000.0;
			showHealthComponent?.MarkDirty();
			instance.RefreshDamagePoint();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["angry"] = _angry;
		dictionary["angryBonusApplied"] = _angryBonusApplied;
		dictionary["gasping"] = _gasping;
		dictionary["keepAliveBeforeGasp"] = _keepAliveBeforeGasp;
		dictionary["damageHitpointsFloorBeforeGasp"] = _damageHitpointsFloorBeforeGasp;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		angry = data.GetValueOrDefault("angry", false).AsBool();
		_angryBonusApplied = data.GetValueOrDefault("angryBonusApplied", _angry).AsBool();
		_gasping = data.GetValueOrDefault("gasping", !_angry && instance.keepAlive).AsBool();
		_keepAliveBeforeGasp = data.GetValueOrDefault("keepAliveBeforeGasp", false).AsBool();
		_damageHitpointsFloorBeforeGasp = data.GetValueOrDefault("damageHitpointsFloorBeforeGasp", 0.0).AsDouble();
		if (_gasping)
		{
			instance.keepAlive = true;
			instance.damageHitpointsFloor = Math.Max(1.0, _damageHitpointsFloorBeforeGasp);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GaspEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GaspProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyAngryPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyAngryHitpointBonus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ConnectStateSignals && args.Count == 0)
		{
			ConnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectStateSignals && args.Count == 0)
		{
			DisconnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.GaspEntered && args.Count == 0)
		{
			GaspEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GaspProcessing && args.Count == 1)
		{
			GaspProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GaspExited && args.Count == 0)
		{
			GaspExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAngryPresentation && args.Count == 0)
		{
			ApplyAngryPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAngryHitpointBonus && args.Count == 0)
		{
			ApplyAngryHitpointBonus();
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
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.GaspEntered)
		{
			return true;
		}
		if (method == MethodName.GaspProcessing)
		{
			return true;
		}
		if (method == MethodName.GaspExited)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ApplyAngryPresentation)
		{
			return true;
		}
		if (method == MethodName.ApplyAngryHitpointBonus)
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
		if (name == PropertyName.angry)
		{
			angry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._angry)
		{
			_angry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gasping)
		{
			_gasping = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._keepAliveBeforeGasp)
		{
			_keepAliveBeforeGasp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._damageHitpointsFloorBeforeGasp)
		{
			_damageHitpointsFloorBeforeGasp = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._angryBonusApplied)
		{
			_angryBonusApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.angry)
		{
			value = VariantUtils.CreateFrom<bool>(angry);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._angry)
		{
			value = VariantUtils.CreateFrom(in _angry);
			return true;
		}
		if (name == PropertyName._gasping)
		{
			value = VariantUtils.CreateFrom(in _gasping);
			return true;
		}
		if (name == PropertyName._keepAliveBeforeGasp)
		{
			value = VariantUtils.CreateFrom(in _keepAliveBeforeGasp);
			return true;
		}
		if (name == PropertyName._damageHitpointsFloorBeforeGasp)
		{
			value = VariantUtils.CreateFrom(in _damageHitpointsFloorBeforeGasp);
			return true;
		}
		if (name == PropertyName._angryBonusApplied)
		{
			value = VariantUtils.CreateFrom(in _angryBonusApplied);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gasping, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._keepAliveBeforeGasp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._damageHitpointsFloorBeforeGasp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._angryBonusApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.angry, Variant.From<bool>(angry));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._angry, Variant.From(in _angry));
		info.AddProperty(PropertyName._gasping, Variant.From(in _gasping));
		info.AddProperty(PropertyName._keepAliveBeforeGasp, Variant.From(in _keepAliveBeforeGasp));
		info.AddProperty(PropertyName._damageHitpointsFloorBeforeGasp, Variant.From(in _damageHitpointsFloorBeforeGasp));
		info.AddProperty(PropertyName._angryBonusApplied, Variant.From(in _angryBonusApplied));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.angry, out var value))
		{
			angry = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value2))
		{
			_stateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._angry, out var value3))
		{
			_angry = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gasping, out var value4))
		{
			_gasping = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._keepAliveBeforeGasp, out var value5))
		{
			_keepAliveBeforeGasp = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._damageHitpointsFloorBeforeGasp, out var value6))
		{
			_damageHitpointsFloorBeforeGasp = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._angryBonusApplied, out var value7))
		{
			_angryBonusApplied = value7.As<bool>();
		}
	}
}
