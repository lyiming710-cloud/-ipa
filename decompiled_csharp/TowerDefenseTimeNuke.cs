using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/TimeNuke/Scene/TowerDefenseTimeNuke.cs")]
public class TowerDefenseTimeNuke : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName GetCountdownRemaining = "GetCountdownRemaining";

		public static readonly StringName OnTimeout = "OnTimeout";

		public static readonly StringName ReadyEntered = "ReadyEntered";

		public static readonly StringName ReadyProcessing = "ReadyProcessing";

		public static readonly StringName UpdateRedFilter = "UpdateRedFilter";

		public static readonly StringName UpdateNumberDisplay = "UpdateNumberDisplay";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName Explode = "Explode";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public static readonly StringName countdownTime = "countdownTime";

		public static readonly StringName readyDuration = "readyDuration";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _readyStarted = "_readyStarted";

		public static readonly StringName _exploded = "_exploded";

		public static readonly StringName _lastDisplayedSecond = "_lastDisplayedSecond";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double countdownTime = 120.0;

	[Export(PropertyHint.None, "")]
	public double readyDuration = 1.5;

	private CharacterTimerComponent _timerComponent;

	private ExplodeComponent _explodeComponent;

	private StateHandle _readyState;

	private bool _stateSignalsConnected;

	private bool _readyStarted;

	private bool _exploded;

	private int _lastDisplayedSecond = -1;

	private const string NumberTexturePath = "res://Asset/AtlasSource/DamagePoint/Anime/Character/GraveStone/TimeBomb/DamagePoint/Config/TimeBombNum_";

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode)
		{
			return;
		}
		_timerComponent = componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout += OnTimeout;
		}
		_explodeComponent = componentManager.GetRuntime<ExplodeComponent>("character.explode");
		_readyState = StateMachine?.GetStateById("timenuke.ready");
		ConnectStateSignals();
		RemoveFromGroup("Gravestone");
		if (_readyStarted)
		{
			SendStateEvent("ToReady");
			return;
		}
		UpdateNumberDisplay(countdownTime);
		CharacterTimerComponent timerComponent2 = _timerComponent;
		if (timerComponent2 != null && !timerComponent2.IsReleased && !_timerComponent.IsRunning("Countdown"))
		{
			_timerComponent.Run("Countdown", countdownTime);
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased)
		{
			_timerComponent.OnTimeout -= OnTimeout;
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
			StateHandle readyState = _readyState;
			if (readyState != null && readyState.IsValid)
			{
				_readyState.Entered += ReadyEntered;
				_readyState.PhysicsProcessing += ReadyProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (!_readyStarted)
		{
			UpdateRedFilter();
			double countdownRemaining = GetCountdownRemaining();
			int num = (int)Math.Ceiling(countdownRemaining);
			if (num != _lastDisplayedSecond)
			{
				UpdateNumberDisplay(countdownRemaining);
				_lastDisplayedSecond = num;
			}
		}
	}

	private double GetCountdownRemaining()
	{
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent == null || timerComponent.IsReleased)
		{
			return countdownTime;
		}
		double num = (_timerComponent.timerWaitTime.TryGetValue("Countdown", out var value) ? value : countdownTime);
		double num2 = (_timerComponent.timerCurrent.TryGetValue("Countdown", out var value2) ? value2 : 0.0);
		return num - num2;
	}

	private void OnTimeout(string timerName)
	{
		if (!(timerName == "Countdown"))
		{
			if (timerName == "Ready")
			{
				Explode();
			}
		}
		else
		{
			_readyStarted = true;
			SendStateEvent("ToReady");
		}
	}

	private void ReadyEntered()
	{
		if (sprite != null && GodotObject.IsInstanceValid(sprite))
		{
			if (sprite.HasClip("Ready"))
			{
				sprite.SetAnimation("Ready");
				sprite.timeScale = 1.0;
			}
			sprite.SetRenderColorMultiplier(new Color(1f, 0.4f, 0.4f));
		}
		instance.invincible = true;
		CharacterTimerComponent timerComponent = _timerComponent;
		if (timerComponent != null && !timerComponent.IsReleased && !_timerComponent.IsRunning("Ready"))
		{
			_timerComponent.Run("Ready", readyDuration);
		}
	}

	private void ReadyProcessing(double delta)
	{
		if (sprite != null && GodotObject.IsInstanceValid(sprite))
		{
			sprite.timeScale = Math.Min(sprite.timeScale + delta * 2.0, 4.0);
		}
	}

	private void UpdateRedFilter()
	{
		if (sprite != null && GodotObject.IsInstanceValid(sprite))
		{
			float num = Mathf.Clamp((Mathf.Clamp(1f - (float)(GetCountdownRemaining() / countdownTime), 0f, 1f) - 0.5f) * 2f, 0f, 1f);
			float num2 = 1f - num * 0.6f;
			sprite.SetRenderColorMultiplier(new Color(1f, num2, num2));
		}
	}

	private void UpdateNumberDisplay(double remaining)
	{
		if (sprite != null && GodotObject.IsInstanceValid(sprite))
		{
			int num = ((!(remaining <= 0.0)) ? ((int)Math.Ceiling(remaining)) : 0);
			int num2 = num / 60;
			int num3 = num % 60;
			int[] array = new int[4]
			{
				Math.Min(num2 / 10, 9),
				num2 % 10,
				num3 / 10 % 10,
				num3 % 10
			};
			for (int i = 0; i < 4; i++)
			{
				sprite.SetAtlasReplace("TimeBombNum_" + i + ".png", "res://Asset/AtlasSource/DamagePoint/Anime/Character/GraveStone/TimeBomb/DamagePoint/Config/TimeBombNum_" + array[i] + ".png");
			}
		}
	}

	public override void DestroySet()
	{
		if (!_exploded)
		{
			if (sprite != null && GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetRenderColorMultiplier(Colors.White);
			}
			base.DestroySet();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["readyStarted"] = _readyStarted };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_readyStarted = data.GetValueOrDefault("readyStarted", false).AsBool();
	}

	private void Explode()
	{
		if (!_exploded)
		{
			_exploded = true;
			ExplodeComponent explodeComponent = _explodeComponent;
			if (explodeComponent != null && !explodeComponent.IsReleased)
			{
				_explodeComponent.Explode();
			}
			Destroy();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCountdownRemaining, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadyEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadyProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRedFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateNumberDisplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "remaining", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCountdownRemaining && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetCountdownRemaining());
			return true;
		}
		if (method == MethodName.OnTimeout && args.Count == 1)
		{
			OnTimeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyEntered && args.Count == 0)
		{
			ReadyEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyProcessing && args.Count == 1)
		{
			ReadyProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRedFilter && args.Count == 0)
		{
			UpdateRedFilter();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateNumberDisplay && args.Count == 1)
		{
			UpdateNumberDisplay(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
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
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.GetCountdownRemaining)
		{
			return true;
		}
		if (method == MethodName.OnTimeout)
		{
			return true;
		}
		if (method == MethodName.ReadyEntered)
		{
			return true;
		}
		if (method == MethodName.ReadyProcessing)
		{
			return true;
		}
		if (method == MethodName.UpdateRedFilter)
		{
			return true;
		}
		if (method == MethodName.UpdateNumberDisplay)
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
		if (method == MethodName.Explode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.countdownTime)
		{
			countdownTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.readyDuration)
		{
			readyDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._readyStarted)
		{
			_readyStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._exploded)
		{
			_exploded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastDisplayedSecond)
		{
			_lastDisplayedSecond = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.countdownTime)
		{
			value = VariantUtils.CreateFrom(in countdownTime);
			return true;
		}
		if (name == PropertyName.readyDuration)
		{
			value = VariantUtils.CreateFrom(in readyDuration);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._readyStarted)
		{
			value = VariantUtils.CreateFrom(in _readyStarted);
			return true;
		}
		if (name == PropertyName._exploded)
		{
			value = VariantUtils.CreateFrom(in _exploded);
			return true;
		}
		if (name == PropertyName._lastDisplayedSecond)
		{
			value = VariantUtils.CreateFrom(in _lastDisplayedSecond);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.countdownTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.readyDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._readyStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._exploded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastDisplayedSecond, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.countdownTime, Variant.From(in countdownTime));
		info.AddProperty(PropertyName.readyDuration, Variant.From(in readyDuration));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._readyStarted, Variant.From(in _readyStarted));
		info.AddProperty(PropertyName._exploded, Variant.From(in _exploded));
		info.AddProperty(PropertyName._lastDisplayedSecond, Variant.From(in _lastDisplayedSecond));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.countdownTime, out var value))
		{
			countdownTime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.readyDuration, out var value2))
		{
			readyDuration = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value3))
		{
			_stateSignalsConnected = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._readyStarted, out var value4))
		{
			_readyStarted = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._exploded, out var value5))
		{
			_exploded = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastDisplayedSecond, out var value6))
		{
			_lastDisplayedSecond = value6.As<int>();
		}
	}
}
