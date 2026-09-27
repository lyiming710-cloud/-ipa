using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/KelpMine/Scene/TowerDefensePlantKelpMine.cs")]
public class TowerDefensePlantKelpMine : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public static readonly StringName DragFinished = "DragFinished";

		public static readonly StringName Timeout = "Timeout";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName MineEntered = "MineEntered";

		public static readonly StringName KelpEntered = "KelpEntered";

		public static readonly StringName ApplyKelpForm = "ApplyKelpForm";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName RestoreMineForm = "RestoreMineForm";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName mineForm = "mineForm";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string KelpStateId = "plant.kelp_mine.kelp";

	private const string MineStateId = "plant.kelp_mine.mine";

	private const string ToKelpEvent = "ToKelp";

	private const string ToMineEvent = "ToMine";

	private const string ArmedTimer = "Armed";

	private const string BackClip = "Back";

	private const string KelpIdleClip = "Idle";

	private TanglekelpComponent _tanglekelpComponent;

	private PotatoComponent _potatoComponent;

	private CharacterTimerComponent _timerComponent;

	private StateHandle _kelpState;

	private StateHandle _mineState;

	private bool _stateSignalsConnected;

	public bool mineForm { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_tanglekelpComponent = componentManager.GetRuntime<TanglekelpComponent>();
			_potatoComponent = componentManager.GetRuntime<PotatoComponent>();
			_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
			ApplyKelpForm();
			ConnectStateSignals();
			TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
			if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
			{
				_tanglekelpComponent.OnDrag += DragFinished;
			}
			CharacterTimerComponent timerComponent = _timerComponent;
			if (timerComponent != null && !timerComponent.IsReleased)
			{
				_timerComponent.OnTimeout += Timeout;
			}
		}
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
		if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
		{
			_tanglekelpComponent.OnDrag -= DragFinished;
		}
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= Timeout;
		}
		base._ExitTree();
	}

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		_kelpState = StateMachine?.GetStateById("plant.kelp_mine.kelp");
		_mineState = StateMachine?.GetStateById("plant.kelp_mine.mine");
		StateHandle mineState = _mineState;
		if (mineState != null && mineState.IsValid)
		{
			StateHandle kelpState = _kelpState;
			if (kelpState != null && kelpState.IsValid)
			{
				_mineState.Entered += MineEntered;
				_kelpState.Entered += KelpEntered;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_mineState != null)
			{
				_mineState.Entered -= MineEntered;
			}
			if (_kelpState != null)
			{
				_kelpState.Entered -= KelpEntered;
			}
			_kelpState = null;
			_mineState = null;
			_stateSignalsConnected = false;
		}
	}

	public virtual void DragFinished(TowerDefenseCharacter target, bool success)
	{
		if (success)
		{
			SendStateEvent("ToMine");
		}
	}

	public void Timeout(string timerName)
	{
		if (!(timerName != "Armed") && mineForm)
		{
			if (GodotObject.IsInstanceValid(sprite) && sprite.HasClip("Back"))
			{
				sprite.SetAnimation("Back", loop: false);
			}
			else
			{
				SendStateEvent("ToKelp");
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Back")
		{
			SendStateEvent("ToKelp");
		}
	}

	public void MineEntered()
	{
		mineForm = true;
		TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
		if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
		{
			_tanglekelpComponent.SetAlive(alive: false);
		}
		PotatoComponent potatoComponent = _potatoComponent;
		if (potatoComponent != null && !potatoComponent.IsReleased)
		{
			_potatoComponent.SetAlive(alive: true);
			_potatoComponent.ReadyRise();
		}
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased && !_timerComponent.IsRunning("Armed"))
		{
			_timerComponent.Run("Armed");
		}
	}

	public void KelpEntered()
	{
		ApplyKelpForm();
		Idle();
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.invincible = false;
			instance.invincibleHurt = false;
			instance.invincibleSmash = false;
		}
		itemLayer = TowerDefenseEnum.LAYER_GROUNDITEM.PLANT;
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.canBeCollection = true;
			instance.canCollection = true;
			instance.hologram = false;
		}
		TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
		if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
		{
			base.targetRegistrationComponent.canProjectileCheck = true;
			base.targetRegistrationComponent.RegisterTarget();
		}
		SetHitBoxEnabled(enabled: true);
		SetHitBoxMonitorable(monitorable: true);
		SetHitBoxSuppressed(HitBoxSuppressionReason.Tanglekelp, suppressed: false);
		SetHitBoxMonitorSuppressed(HitBoxSuppressionReason.Tanglekelp, suppressed: false);
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.timeScale = timeScale;
			if (sprite.HasClip("Idle"))
			{
				sprite.SetAnimation("Idle", loop: true, 0.2);
			}
		}
	}

	private void ApplyKelpForm()
	{
		mineForm = false;
		PotatoComponent potatoComponent = _potatoComponent;
		if (potatoComponent != null && !potatoComponent.IsReleased)
		{
			_potatoComponent.SetAlive(alive: false);
		}
		TanglekelpComponent tanglekelpComponent = _tanglekelpComponent;
		if (tanglekelpComponent != null && !tanglekelpComponent.IsReleased)
		{
			_tanglekelpComponent.SetAlive(alive: true);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["mineForm"] = mineForm;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		if (data.GetValueOrDefault("mineForm", false).AsBool() && !mineForm)
		{
			CallDeferred("RestoreMineForm");
		}
	}

	private void RestoreMineForm()
	{
		if (!mineForm)
		{
			SendStateEvent("ToMine");
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
			new MethodInfo(MethodName.DragFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "success", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Timeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MineEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.KelpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyKelpForm, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreMineForm, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DragFinished && args.Count == 2)
		{
			DragFinished(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Timeout && args.Count == 1)
		{
			Timeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MineEntered && args.Count == 0)
		{
			MineEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.KelpEntered && args.Count == 0)
		{
			KelpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyKelpForm && args.Count == 0)
		{
			ApplyKelpForm();
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
		if (method == MethodName.RestoreMineForm && args.Count == 0)
		{
			RestoreMineForm();
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
		if (method == MethodName.DragFinished)
		{
			return true;
		}
		if (method == MethodName.Timeout)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.MineEntered)
		{
			return true;
		}
		if (method == MethodName.KelpEntered)
		{
			return true;
		}
		if (method == MethodName.ApplyKelpForm)
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
		if (method == MethodName.RestoreMineForm)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mineForm)
		{
			mineForm = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mineForm)
		{
			value = VariantUtils.CreateFrom<bool>(mineForm);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.mineForm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mineForm, Variant.From<bool>(mineForm));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mineForm, out var value))
		{
			mineForm = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value2))
		{
			_stateSignalsConnected = value2.As<bool>();
		}
	}
}
