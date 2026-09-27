using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter9/SunTransfer/Scene/TowerDefensePlantSunTransfer.cs")]
public class TowerDefensePlantSunTransfer : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName PrepareForProgressRestore = "PrepareForProgressRestore";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName LifetimeTimeout = "LifetimeTimeout";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName ShootEntered = "ShootEntered";

		public static readonly StringName ShootProcessing = "ShootProcessing";

		public static readonly StringName TransferEntered = "TransferEntered";

		public static readonly StringName TransferProcessing = "TransferProcessing";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _shootPending = "_shootPending";

		public static readonly StringName _produced = "_produced";

		public static readonly StringName _restoredFromProgress = "_restoredFromProgress";

		public static readonly StringName _over = "_over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private StateHandle _shootState;

	private StateHandle _transferState;

	private bool _stateSignalsConnected;

	private bool _shootPending;

	private bool _produced;

	private bool _restoredFromProgress;

	private bool _over;

	private CharacterTimerComponent _lifetimeTimer;

	public override void PrepareForProgressRestore()
	{
		base.PrepareForProgressRestore();
		_restoredFromProgress = true;
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode || !inGame)
		{
			return;
		}
		useIdleAnimeReset = false;
		_lifetimeTimer = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
		_lifetimeTimer.OnTimeout += LifetimeTimeout;
		if (!_lifetimeTimer.IsRunning("Destroy"))
		{
			_lifetimeTimer.Run("Destroy");
		}
		if (!_produced && !_restoredFromProgress)
		{
			_produced = true;
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition(spriteGroup);
			if (instance.hypnoses)
			{
				BrainSunCreate(logicalGlobalPosition, 300L);
			}
			else
			{
				SunCreate(logicalGlobalPosition, 300L);
			}
		}
		_shootState = StateMachine?.GetStateById("plant.sun_transfer.shoot");
		_transferState = StateMachine?.GetStateById("plant.sun_transfer.transfer");
		ConnectStateSignals();
		_shootPending = true;
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		CharacterTimerComponent lifetimeTimer = _lifetimeTimer;
		if (lifetimeTimer != null && !lifetimeTimer.IsReleased)
		{
			_lifetimeTimer.OnTimeout -= LifetimeTimeout;
		}
		_lifetimeTimer = null;
		base._ExitTree();
	}

	private void LifetimeTimeout(string timerName)
	{
		if (!(timerName != "Destroy"))
		{
			Destroy();
		}
	}

	private void ConnectStateSignals()
	{
		if (!_stateSignalsConnected)
		{
			StateHandle shootState = _shootState;
			if (shootState != null && shootState.IsValid)
			{
				_shootState.Entered += ShootEntered;
				_shootState.PhysicsProcessing += ShootProcessing;
			}
			StateHandle transferState = _transferState;
			if (transferState != null && transferState.IsValid)
			{
				_transferState.Entered += TransferEntered;
				_transferState.PhysicsProcessing += TransferProcessing;
			}
			_stateSignalsConnected = true;
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			StateHandle shootState = _shootState;
			if (shootState != null && shootState.IsValid)
			{
				_shootState.Entered -= ShootEntered;
				_shootState.PhysicsProcessing -= ShootProcessing;
			}
			StateHandle transferState = _transferState;
			if (transferState != null && transferState.IsValid)
			{
				_transferState.Entered -= TransferEntered;
				_transferState.PhysicsProcessing -= TransferProcessing;
			}
			_shootState = null;
			_transferState = null;
			_stateSignalsConnected = false;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (_shootPending)
		{
			_shootPending = false;
			SendStateEvent("ToShoot");
		}
	}

	private void ShootEntered()
	{
		if (sprite != null && sprite.HasClip("Shooting"))
		{
			sprite.SetAnimation("Shooting", loop: false);
		}
	}

	private void ShootProcessing(double delta)
	{
		if (sprite != null)
		{
			sprite.timeScale = timeScale;
		}
	}

	private void TransferEntered()
	{
		if (sprite != null && sprite.HasClip("IdleB"))
		{
			sprite.SetAnimation("IdleB");
		}
	}

	private void TransferProcessing(double delta)
	{
		if (sprite != null)
		{
			sprite.timeScale = timeScale;
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Shooting")
		{
			SendStateEvent("ToTransfer");
		}
	}

	public override void DestroySet()
	{
		if (!_over)
		{
			_over = true;
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition(spriteGroup);
			ReplicatedEventSunCreate(logicalGlobalPosition, 300L, !instance.hypnoses);
			base.DestroySet();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["over"] = _over,
			["produced"] = _produced
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_over = data.GetValueOrDefault("over", false).AsBool();
		_produced = data.GetValueOrDefault("produced", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.PrepareForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LifetimeTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShootEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShootProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TransferEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TransferProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.PrepareForProgressRestore && args.Count == 0)
		{
			PrepareForProgressRestore();
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
		if (method == MethodName.LifetimeTimeout && args.Count == 1)
		{
			LifetimeTimeout(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShootEntered && args.Count == 0)
		{
			ShootEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ShootProcessing && args.Count == 1)
		{
			ShootProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TransferEntered && args.Count == 0)
		{
			TransferEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.TransferProcessing && args.Count == 1)
		{
			TransferProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.PrepareForProgressRestore)
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
		if (method == MethodName.LifetimeTimeout)
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
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.ShootEntered)
		{
			return true;
		}
		if (method == MethodName.ShootProcessing)
		{
			return true;
		}
		if (method == MethodName.TransferEntered)
		{
			return true;
		}
		if (method == MethodName.TransferProcessing)
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
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._shootPending)
		{
			_shootPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._produced)
		{
			_produced = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			_restoredFromProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._over)
		{
			_over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._shootPending)
		{
			value = VariantUtils.CreateFrom(in _shootPending);
			return true;
		}
		if (name == PropertyName._produced)
		{
			value = VariantUtils.CreateFrom(in _produced);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			value = VariantUtils.CreateFrom(in _restoredFromProgress);
			return true;
		}
		if (name == PropertyName._over)
		{
			value = VariantUtils.CreateFrom(in _over);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName._shootPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._produced, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._restoredFromProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._shootPending, Variant.From(in _shootPending));
		info.AddProperty(PropertyName._produced, Variant.From(in _produced));
		info.AddProperty(PropertyName._restoredFromProgress, Variant.From(in _restoredFromProgress));
		info.AddProperty(PropertyName._over, Variant.From(in _over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._shootPending, out var value2))
		{
			_shootPending = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._produced, out var value3))
		{
			_produced = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._restoredFromProgress, out var value4))
		{
			_restoredFromProgress = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._over, out var value5))
		{
			_over = value5.As<bool>();
		}
	}
}
