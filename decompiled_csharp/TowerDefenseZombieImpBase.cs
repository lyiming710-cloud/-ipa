using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseZombieImpBase.cs")]
public class TowerDefenseZombieImpBase : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public static readonly StringName StartThrowTrajectory = "StartThrowTrajectory";

		public static readonly StringName SetThrowTweenX = "SetThrowTweenX";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public static readonly StringName Fly = "Fly";

		public static readonly StringName Land = "Land";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName flyAnimeClip = "flyAnimeClip";

		public static readonly StringName landAnimeClip = "landAnimeClip";

		public static readonly StringName collectonFlagsSave = "collectonFlagsSave";

		public static readonly StringName maskFlagsSave = "maskFlagsSave";

		public static readonly StringName _throw = "_throw";

		public static readonly StringName landOver = "landOver";

		public static readonly StringName _throwLandPosX = "_throwLandPosX";

		public static readonly StringName _throwFallDuration = "_throwFallDuration";

		public static readonly StringName _throwEase = "_throwEase";

		public static readonly StringName _throwTransition = "_throwTransition";

		public static readonly StringName _throwTrajectoryPending = "_throwTrajectoryPending";

		public static readonly StringName _throwTrajectoryStarted = "_throwTrajectoryStarted";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string flyAnimeClip = "Fly";

	[Export(PropertyHint.None, "")]
	public string landAnimeClip = "Land";

	public int collectonFlagsSave;

	public int maskFlagsSave;

	public bool _throw;

	public bool landOver;

	public ImpFlightComponent impFlightComponent;

	private double _throwLandPosX;

	private double _throwFallDuration;

	private Tween.EaseType _throwEase = Tween.EaseType.Out;

	private Tween.TransitionType _throwTransition = Tween.TransitionType.Quad;

	private bool _throwTrajectoryPending;

	private bool _throwTrajectoryStarted;

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			if (_throw && !landOver)
			{
				isGround = false;
			}
			base._Ready();
			if (GodotObject.IsInstanceValid(componentManager))
			{
				impFlightComponent = componentManager.GetRuntime<ImpFlightComponent>();
			}
			collectonFlagsSave = instance.collisionFlags;
			maskFlagsSave = instance.maskFlags;
			if (_throw)
			{
				StartThrowTrajectory();
				CallDeferred(MethodName.Fly);
			}
		}
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (data == null)
		{
			return;
		}
		_throw = data.GetValueOrDefault("throw", _throw).AsBool();
		if (_throw && !landOver)
		{
			isGround = false;
		}
		ySpeed = data.GetValueOrDefault("y_speed", ySpeed).AsDouble();
		if (data.ContainsKey("land_pos_x") && data.ContainsKey("fall_duration"))
		{
			_throwLandPosX = data["land_pos_x"].AsDouble();
			_throwFallDuration = Mathf.Max(0.001, data["fall_duration"].AsDouble());
			_throwEase = (Tween.EaseType)data.GetValueOrDefault("throw_ease", 1).AsInt32();
			_throwTransition = (Tween.TransitionType)data.GetValueOrDefault("throw_transition", 4).AsInt32();
			_throwTrajectoryPending = _throw;
		}
		if (IsNodeReady())
		{
			StartThrowTrajectory();
			if (_throw)
			{
				CallDeferred(MethodName.Fly);
			}
		}
	}

	private void StartThrowTrajectory()
	{
		if (_throwTrajectoryPending && !_throwTrajectoryStarted && IsInsideTree())
		{
			_throwTrajectoryPending = false;
			_throwTrajectoryStarted = true;
			Tween tween = CreateTween();
			tween.SetEase(_throwEase);
			tween.SetTrans(_throwTransition);
			tween.TweenMethod(Callable.From<double>(SetThrowTweenX), (double)GetLogicalGlobalPosition().X, _throwLandPosX, _throwFallDuration);
		}
	}

	private void SetThrowTweenX(double value)
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		logicalGlobalPosition.X = (float)value;
		SetLogicalGlobalPosition(logicalGlobalPosition);
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		if (_throw)
		{
			return !landOver;
		}
		return false;
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
	}

	public void Fly()
	{
		GetFlightRuntime()?.Fly();
	}

	public void Land()
	{
		ImpFlightComponent flightRuntime = GetFlightRuntime();
		if (flightRuntime != null)
		{
			flightRuntime.Land();
			return;
		}
		landOver = true;
		Walk();
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		ImpFlightComponent flightRuntime = GetFlightRuntime();
		if ((flightRuntime == null || !flightRuntime.AnimeCompleted(clip)) && clip == landAnimeClip)
		{
			Walk();
		}
	}

	private ImpFlightComponent GetFlightRuntime()
	{
		ImpFlightComponent impFlightComponent = this.impFlightComponent;
		if (impFlightComponent != null && !impFlightComponent.IsReleased)
		{
			return this.impFlightComponent;
		}
		this.impFlightComponent = (GodotObject.IsInstanceValid(componentManager) ? componentManager.GetRuntime<ImpFlightComponent>() : null);
		return this.impFlightComponent;
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["throw"] = _throw,
			["landOver"] = landOver,
			["land_pos_x"] = _throwLandPosX,
			["fall_duration"] = _throwFallDuration,
			["throw_ease"] = (int)_throwEase,
			["throw_transition"] = (int)_throwTransition
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_throw = data.GetValueOrDefault("throw", false).AsBool();
		landOver = data.GetValueOrDefault("landOver", false).AsBool();
		if (_throw && !landOver)
		{
			isGround = false;
		}
		if (data.ContainsKey("land_pos_x") && data.ContainsKey("fall_duration"))
		{
			_throwLandPosX = data.GetValueOrDefault("land_pos_x", GetLogicalGlobalPosition().X).AsDouble();
			_throwFallDuration = Mathf.Max(0.001, data.GetValueOrDefault("fall_duration", 0.001).AsDouble());
			_throwEase = (Tween.EaseType)data.GetValueOrDefault("throw_ease", 1).AsInt32();
			_throwTransition = (Tween.TransitionType)data.GetValueOrDefault("throw_transition", 4).AsInt32();
			_throwTrajectoryPending = _throw && !landOver && !_throwTrajectoryStarted;
			if (IsNodeReady())
			{
				StartThrowTrajectory();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartThrowTrajectory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetThrowTweenX, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsNetworkSpecialMovementActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Fly, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Land, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartThrowTrajectory && args.Count == 0)
		{
			StartThrowTrajectory();
			ret = default;
			return true;
		}
		if (method == MethodName.SetThrowTweenX && args.Count == 1)
		{
			SetThrowTweenX(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsNetworkSpecialMovementActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNetworkSpecialMovementActive());
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.Fly && args.Count == 0)
		{
			Fly();
			ret = default;
			return true;
		}
		if (method == MethodName.Land && args.Count == 0)
		{
			Land();
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.StartThrowTrajectory)
		{
			return true;
		}
		if (method == MethodName.SetThrowTweenX)
		{
			return true;
		}
		if (method == MethodName.IsNetworkSpecialMovementActive)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.Fly)
		{
			return true;
		}
		if (method == MethodName.Land)
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
		if (name == PropertyName.flyAnimeClip)
		{
			flyAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.landAnimeClip)
		{
			landAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.collectonFlagsSave)
		{
			collectonFlagsSave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maskFlagsSave)
		{
			maskFlagsSave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._throw)
		{
			_throw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.landOver)
		{
			landOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._throwLandPosX)
		{
			_throwLandPosX = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._throwFallDuration)
		{
			_throwFallDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._throwEase)
		{
			_throwEase = VariantUtils.ConvertTo<Tween.EaseType>(in value);
			return true;
		}
		if (name == PropertyName._throwTransition)
		{
			_throwTransition = VariantUtils.ConvertTo<Tween.TransitionType>(in value);
			return true;
		}
		if (name == PropertyName._throwTrajectoryPending)
		{
			_throwTrajectoryPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._throwTrajectoryStarted)
		{
			_throwTrajectoryStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.flyAnimeClip)
		{
			value = VariantUtils.CreateFrom(in flyAnimeClip);
			return true;
		}
		if (name == PropertyName.landAnimeClip)
		{
			value = VariantUtils.CreateFrom(in landAnimeClip);
			return true;
		}
		if (name == PropertyName.collectonFlagsSave)
		{
			value = VariantUtils.CreateFrom(in collectonFlagsSave);
			return true;
		}
		if (name == PropertyName.maskFlagsSave)
		{
			value = VariantUtils.CreateFrom(in maskFlagsSave);
			return true;
		}
		if (name == PropertyName._throw)
		{
			value = VariantUtils.CreateFrom(in _throw);
			return true;
		}
		if (name == PropertyName.landOver)
		{
			value = VariantUtils.CreateFrom(in landOver);
			return true;
		}
		if (name == PropertyName._throwLandPosX)
		{
			value = VariantUtils.CreateFrom(in _throwLandPosX);
			return true;
		}
		if (name == PropertyName._throwFallDuration)
		{
			value = VariantUtils.CreateFrom(in _throwFallDuration);
			return true;
		}
		if (name == PropertyName._throwEase)
		{
			value = VariantUtils.CreateFrom(in _throwEase);
			return true;
		}
		if (name == PropertyName._throwTransition)
		{
			value = VariantUtils.CreateFrom(in _throwTransition);
			return true;
		}
		if (name == PropertyName._throwTrajectoryPending)
		{
			value = VariantUtils.CreateFrom(in _throwTrajectoryPending);
			return true;
		}
		if (name == PropertyName._throwTrajectoryStarted)
		{
			value = VariantUtils.CreateFrom(in _throwTrajectoryStarted);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.flyAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.landAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collectonFlagsSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.maskFlagsSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._throw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.landOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._throwLandPosX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._throwFallDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._throwEase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._throwTransition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._throwTrajectoryPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._throwTrajectoryStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.flyAnimeClip, Variant.From(in flyAnimeClip));
		info.AddProperty(PropertyName.landAnimeClip, Variant.From(in landAnimeClip));
		info.AddProperty(PropertyName.collectonFlagsSave, Variant.From(in collectonFlagsSave));
		info.AddProperty(PropertyName.maskFlagsSave, Variant.From(in maskFlagsSave));
		info.AddProperty(PropertyName._throw, Variant.From(in _throw));
		info.AddProperty(PropertyName.landOver, Variant.From(in landOver));
		info.AddProperty(PropertyName._throwLandPosX, Variant.From(in _throwLandPosX));
		info.AddProperty(PropertyName._throwFallDuration, Variant.From(in _throwFallDuration));
		info.AddProperty(PropertyName._throwEase, Variant.From(in _throwEase));
		info.AddProperty(PropertyName._throwTransition, Variant.From(in _throwTransition));
		info.AddProperty(PropertyName._throwTrajectoryPending, Variant.From(in _throwTrajectoryPending));
		info.AddProperty(PropertyName._throwTrajectoryStarted, Variant.From(in _throwTrajectoryStarted));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.flyAnimeClip, out var value))
		{
			flyAnimeClip = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.landAnimeClip, out var value2))
		{
			landAnimeClip = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.collectonFlagsSave, out var value3))
		{
			collectonFlagsSave = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maskFlagsSave, out var value4))
		{
			maskFlagsSave = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._throw, out var value5))
		{
			_throw = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.landOver, out var value6))
		{
			landOver = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._throwLandPosX, out var value7))
		{
			_throwLandPosX = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._throwFallDuration, out var value8))
		{
			_throwFallDuration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._throwEase, out var value9))
		{
			_throwEase = value9.As<Tween.EaseType>();
		}
		if (info.TryGetProperty(PropertyName._throwTransition, out var value10))
		{
			_throwTransition = value10.As<Tween.TransitionType>();
		}
		if (info.TryGetProperty(PropertyName._throwTrajectoryPending, out var value11))
		{
			_throwTrajectoryPending = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._throwTrajectoryStarted, out var value12))
		{
			_throwTrajectoryStarted = value12.As<bool>();
		}
	}
}
