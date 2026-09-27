using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter8/PumpLantern/Scene/TowerDefensePumpLantern.cs")]
public class TowerDefensePumpLantern : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName PrepareForProgressRestore = "PrepareForProgressRestore";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName OnBuffDelete = "OnBuffDelete";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName _ShieldStartTimer = "_ShieldStartTimer";

		public static readonly StringName _GenerateShields = "_GenerateShields";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public static readonly StringName _SyncShieldHypnoses = "_SyncShieldHypnoses";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _light = "_light";

		public static readonly StringName _shieldTimer = "_shieldTimer";

		public static readonly StringName _restoredFromProgress = "_restoredFromProgress";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public const double SHIELD_HP = 500.0;

	public const double SHIELD_INTERVAL = 50.0;

	public static readonly StringName SHIELD_TYPE = new StringName("CommonShield");

	public static readonly Vector2I[] SURROUND_OFFSETS = new Vector2I[8]
	{
		new Vector2I(-1, -1),
		new Vector2I(0, -1),
		new Vector2I(1, -1),
		new Vector2I(-1, 0),
		new Vector2I(1, 0),
		new Vector2I(-1, 1),
		new Vector2I(0, 1),
		new Vector2I(1, 1)
	};

	private PointLight2D _light;

	private Timer _shieldTimer;

	private bool _restoredFromProgress;

	public override void PrepareForProgressRestore()
	{
		base.PrepareForProgressRestore();
		_restoredFromProgress = true;
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_light = GetNodeOrNull<PointLight2D>("%Light");
			AudioManager.Instance.AudioPlay("Plantern");
			if (!_restoredFromProgress)
			{
				_GenerateShields();
			}
			_ShieldStartTimer();
			BuffComponent buffComponent = buff;
			if (buffComponent != null && !buffComponent.IsReleased)
			{
				buff.OnBuffDelete += OnBuffDelete;
			}
		}
	}

	public override void _ExitTree()
	{
		BuffComponent buffComponent = buff;
		if (buffComponent != null && !buffComponent.IsReleased)
		{
			buff.OnBuffDelete -= OnBuffDelete;
		}
		base._ExitTree();
		if (GodotObject.IsInstanceValid(_shieldTimer))
		{
			_shieldTimer.QueueFree();
		}
	}

	private void OnBuffDelete(string key)
	{
		if (key == "Hypnoses")
		{
			_SyncShieldHypnoses();
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			if (GodotObject.IsInstanceValid(_light))
			{
				_light.Visible = TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
			}
		}
	}

	private void _ShieldStartTimer()
	{
		_shieldTimer = new Timer();
		_shieldTimer.WaitTime = 50.0;
		_shieldTimer.Autostart = false;
		_shieldTimer.OneShot = false;
		_shieldTimer.Timeout += _GenerateShields;
		AddChild(_shieldTimer, forceReadableName: false, InternalMode.Disabled);
		_shieldTimer.Start();
	}

	private void _GenerateShields()
	{
		Vector2I[] sURROUND_OFFSETS = SURROUND_OFFSETS;
		foreach (Vector2I vector2I in sURROUND_OFFSETS)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + vector2I);
			if (GodotObject.IsInstanceValid(mapCell))
			{
				TowerDefenseItemSheild.CreateOnCellWithHP(mapCell, SHIELD_TYPE, 500.0);
			}
		}
		if (instance.hypnoses)
		{
			Callable.From(_SyncShieldHypnoses).CallDeferred();
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		_SyncShieldHypnoses();
	}

	private void _SyncShieldHypnoses()
	{
		bool hypnoses = instance.hypnoses;
		Vector2I[] sURROUND_OFFSETS = SURROUND_OFFSETS;
		foreach (Vector2I vector2I in sURROUND_OFFSETS)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos + vector2I);
			if (GodotObject.IsInstanceValid(mapCell) && GodotObject.IsInstanceValid(mapCell.itemShield))
			{
				TowerDefenseItemSheild itemShield = mapCell.itemShield;
				if (itemShield.instance.hypnoses != hypnoses)
				{
					itemShield.Hypnoses();
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.PrepareForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBuffDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ShieldStartTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GenerateShields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName._SyncShieldHypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnBuffDelete && args.Count == 1)
		{
			OnBuffDelete(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ShieldStartTimer && args.Count == 0)
		{
			_ShieldStartTimer();
			ret = default;
			return true;
		}
		if (method == MethodName._GenerateShields && args.Count == 0)
		{
			_GenerateShields();
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._SyncShieldHypnoses && args.Count == 0)
		{
			_SyncShieldHypnoses();
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
		if (method == MethodName.OnBuffDelete)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName._ShieldStartTimer)
		{
			return true;
		}
		if (method == MethodName._GenerateShields)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName._SyncShieldHypnoses)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._light)
		{
			_light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		if (name == PropertyName._shieldTimer)
		{
			_shieldTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			_restoredFromProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._light)
		{
			value = VariantUtils.CreateFrom(in _light);
			return true;
		}
		if (name == PropertyName._shieldTimer)
		{
			value = VariantUtils.CreateFrom(in _shieldTimer);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			value = VariantUtils.CreateFrom(in _restoredFromProgress);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shieldTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._restoredFromProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._light, Variant.From(in _light));
		info.AddProperty(PropertyName._shieldTimer, Variant.From(in _shieldTimer));
		info.AddProperty(PropertyName._restoredFromProgress, Variant.From(in _restoredFromProgress));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._light, out var value))
		{
			_light = value.As<PointLight2D>();
		}
		if (info.TryGetProperty(PropertyName._shieldTimer, out var value2))
		{
			_shieldTimer = value2.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._restoredFromProgress, out var value3))
		{
			_restoredFromProgress = value3.As<bool>();
		}
	}
}
