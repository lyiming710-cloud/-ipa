using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Paper/Scene/TowerDefenseZombiePaper.cs")]
public class TowerDefenseZombiePaper : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName GaspEntered = "GaspEntered";

		public static readonly StringName GaspProcessing = "GaspProcessing";

		public static readonly StringName GaspExited = "GaspExited";

		public static readonly StringName ApplyAngryPresentation = "ApplyAngryPresentation";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName angry = "angry";

		public static readonly StringName _headFollowSlotIdBeforeGasp = "_headFollowSlotIdBeforeGasp";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _angry = "_angry";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const int GaspHeadFollowSlotId = 18;

	private const double AngryTimeScale = 3.0;

	private const string AngryWalkClip = "AngryWalk";

	private const string AngryAttackClip = "AngryEat";

	private const string ZOMBIE_PAPER_MADHEAD = "uid://s4hnj2igecma";

	private int _headFollowSlotIdBeforeGasp = -1;

	private StateHandle _gaspStateHandle;

	private bool _stateSignalsConnected;

	private bool _angry;

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

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_gaspStateHandle = StateMachine?.GetStateById("zombie.paper.gasp");
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

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			ConnectStateSignals();
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void GaspEntered()
	{
		if (GodotObject.IsInstanceValid(headSlot))
		{
			if (_headFollowSlotIdBeforeGasp < 0)
			{
				_headFollowSlotIdBeforeGasp = headSlot.followSlotId;
			}
			headSlot.followSlotId = 18;
		}
		sprite.SetAnimation("Gasp", loop: false);
	}

	public virtual void GaspProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void GaspExited()
	{
		if (GodotObject.IsInstanceValid(headSlot) && _headFollowSlotIdBeforeGasp >= 0)
		{
			headSlot.followSlotId = _headFollowSlotIdBeforeGasp;
		}
		_headFollowSlotIdBeforeGasp = -1;
	}

	private void ApplyAngryPresentation()
	{
		if (_angry)
		{
			walkAnimeClip = "AngryWalk";
			swimAnimeClip = "AngryWalk";
			attackAnimeClip = "AngryEat";
			attackWaterAnimeClip = "AngryEat";
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAtlasReplace("Zombie_head.png", "uid://s4hnj2igecma");
			}
		}
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Paper")
		{
			SendStateEvent("ToGasp");
			AudioManager.Instance.AudioPlay("NewspaperRip");
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Gasp")
		{
			AudioManager.Instance.AudioPlay("NewspaperRarrgh");
			timeScaleInit = 3.0;
			angry = true;
			Walk();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "angry", angry } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		angry = (bool)data.GetValueOrDefault("angry", false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GaspProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyAngryPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.ApplyAngryPresentation && args.Count == 0)
		{
			ApplyAngryPresentation();
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
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
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
		if (method == MethodName.ApplyAngryPresentation)
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
		if (name == PropertyName._headFollowSlotIdBeforeGasp)
		{
			_headFollowSlotIdBeforeGasp = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._headFollowSlotIdBeforeGasp)
		{
			value = VariantUtils.CreateFrom(in _headFollowSlotIdBeforeGasp);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._headFollowSlotIdBeforeGasp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.angry, Variant.From<bool>(angry));
		info.AddProperty(PropertyName._headFollowSlotIdBeforeGasp, Variant.From(in _headFollowSlotIdBeforeGasp));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._angry, Variant.From(in _angry));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.angry, out var value))
		{
			angry = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._headFollowSlotIdBeforeGasp, out var value2))
		{
			_headFollowSlotIdBeforeGasp = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value3))
		{
			_stateSignalsConnected = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._angry, out var value4))
		{
			_angry = value4.As<bool>();
		}
	}
}
