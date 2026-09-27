using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter10/PuffShroomElf/Scene/TowerDefensePlantPuffShroomElf.cs")]
public class TowerDefensePlantPuffShroomElf : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName TickLifetime = "TickLifetime";

		public static readonly StringName BeginFadeVanish = "BeginFadeVanish";

		public static readonly StringName FinishFadeVanish = "FinishFadeVanish";

		public static readonly StringName PlayElfFade = "PlayElfFade";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName _lifetimeRemaining = "_lifetimeRemaining";

		public static readonly StringName _lifetimeLastPhysicsFrame = "_lifetimeLastPhysicsFrame";

		public static readonly StringName _vanishing = "_vanishing";

		public static readonly StringName _fadePlaying = "_fadePlaying";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";

		public static readonly StringName _projectileName = "_projectileName";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string ElfFadeSceneUid = "uid://bq3mw7keas2vt";

	private const string ElfFadeClip = "Quake";

	private const string FadeAnimeClip = "Fade";

	private const double LifetimeSeconds = 50.0;

	private FireComponent _fireComponent;

	private double _lifetimeRemaining = 50.0;

	private ulong _lifetimeLastPhysicsFrame = 18446744073709551615uL;

	private bool _vanishing;

	private bool _fadePlaying;

	private double _fireInterval = 1.5;

	private int _fireNum = 1;

	private string _projectileName = "PuffShroomElfPuff";

	[Export(PropertyHint.None, "")]
	public double fireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					_fireComponent.fireInterval = (float)value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int fireNum
	{
		get
		{
			return _fireNum;
		}
		set
		{
			_fireNum = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					_fireComponent.fireNum = value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public string projectileName
	{
		get
		{
			return _projectileName;
		}
		set
		{
			_projectileName = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased && _fireComponent.fireCheckList.Count > 0)
				{
					((FireComponentProjectileSingle)_fireComponent.fireCheckList[0].projectile).projectileName = value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
			if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
			{
				base.targetRegistrationComponent.UnregisterTarget();
			}
			if (inGame)
			{
				_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		TickLifetime(delta);
	}

	private void TickLifetime(double delta)
	{
		if (Engine.IsEditorHint() || editorPreviewMode || _vanishing || !inGame || die || isDestroy || !TowerDefenseManager._IsGameRunning())
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (currentPhysicsFrame != 18446744073709551615uL)
		{
			if (_lifetimeLastPhysicsFrame == currentPhysicsFrame)
			{
				return;
			}
			_lifetimeLastPhysicsFrame = currentPhysicsFrame;
		}
		_lifetimeRemaining -= delta;
		if (!(_lifetimeRemaining > 0.0))
		{
			_lifetimeRemaining = 0.0;
			_vanishing = true;
			BeginFadeVanish();
		}
	}

	private void BeginFadeVanish()
	{
		die = true;
		TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
		if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
		{
			base.targetRegistrationComponent.UnregisterTarget();
		}
		_fadePlaying = true;
		if (GodotObject.IsInstanceValid(sprite) && sprite.HasClip("Fade"))
		{
			sprite.SetAnimation("Fade", loop: false);
		}
		else
		{
			FinishFadeVanish();
		}
	}

	private void FinishFadeVanish()
	{
		_fadePlaying = false;
		PlayElfFade();
		Destroy();
	}

	private void PlayElfFade()
	{
		PackedScene packedScene = GD.Load<PackedScene>("uid://bq3mw7keas2vt");
		if (packedScene == null)
		{
			return;
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(packedScene, gridPos, "Quake");
		if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
		{
			Node2D node2D = TowerDefenseManager.GetCharacterNode();
			if (!GodotObject.IsInstanceValid(node2D))
			{
				towerDefenseEffectSpriteOnce.QueueFree();
				return;
			}
			node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.gridPos = gridPos;
			towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (_fadePlaying && !(clip != "Fade"))
		{
			FinishFadeVanish();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["fireNum"] = fireNum,
			["projectileName"] = projectileName,
			["fireInterval"] = fireInterval,
			["lifetimeRemaining"] = _lifetimeRemaining
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		fireNum = ((!data.ContainsKey("fireNum")) ? 1 : data["fireNum"].AsInt32());
		projectileName = (data.ContainsKey("projectileName") ? data["projectileName"].AsString() : "PuffShroomElfPuff");
		fireInterval = (data.ContainsKey("fireInterval") ? data["fireInterval"].AsDouble() : 1.5);
		_lifetimeRemaining = (data.ContainsKey("lifetimeRemaining") ? Mathf.Max(0.0, data["lifetimeRemaining"].AsDouble()) : 50.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TickLifetime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginFadeVanish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishFadeVanish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayElfFade, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TickLifetime && args.Count == 1)
		{
			TickLifetime(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginFadeVanish && args.Count == 0)
		{
			BeginFadeVanish();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishFadeVanish && args.Count == 0)
		{
			FinishFadeVanish();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayElfFade && args.Count == 0)
		{
			PlayElfFade();
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.TickLifetime)
		{
			return true;
		}
		if (method == MethodName.BeginFadeVanish)
		{
			return true;
		}
		if (method == MethodName.FinishFadeVanish)
		{
			return true;
		}
		if (method == MethodName.PlayElfFade)
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
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._lifetimeRemaining)
		{
			_lifetimeRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._lifetimeLastPhysicsFrame)
		{
			_lifetimeLastPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._vanishing)
		{
			_vanishing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fadePlaying)
		{
			_fadePlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			_fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom<double>(fireInterval);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			value = VariantUtils.CreateFrom<int>(fireNum);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName._lifetimeRemaining)
		{
			value = VariantUtils.CreateFrom(in _lifetimeRemaining);
			return true;
		}
		if (name == PropertyName._lifetimeLastPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _lifetimeLastPhysicsFrame);
			return true;
		}
		if (name == PropertyName._vanishing)
		{
			value = VariantUtils.CreateFrom(in _vanishing);
			return true;
		}
		if (name == PropertyName._fadePlaying)
		{
			value = VariantUtils.CreateFrom(in _fadePlaying);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			value = VariantUtils.CreateFrom(in _fireNum);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._lifetimeRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lifetimeLastPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._vanishing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._fadePlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
		info.AddProperty(PropertyName._lifetimeRemaining, Variant.From(in _lifetimeRemaining));
		info.AddProperty(PropertyName._lifetimeLastPhysicsFrame, Variant.From(in _lifetimeLastPhysicsFrame));
		info.AddProperty(PropertyName._vanishing, Variant.From(in _vanishing));
		info.AddProperty(PropertyName._fadePlaying, Variant.From(in _fadePlaying));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value2))
		{
			fireNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value3))
		{
			projectileName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._lifetimeRemaining, out var value4))
		{
			_lifetimeRemaining = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._lifetimeLastPhysicsFrame, out var value5))
		{
			_lifetimeLastPhysicsFrame = value5.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._vanishing, out var value6))
		{
			_vanishing = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fadePlaying, out var value7))
		{
			_fadePlaying = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value8))
		{
			_fireInterval = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value9))
		{
			_fireNum = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value10))
		{
			_projectileName = value10.As<string>();
		}
	}
}
