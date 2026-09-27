using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseCrater.cs")]
public class TowerDefenseCrater : TowerDefenseCharacter
{
	public new class MethodName : TowerDefenseCharacter.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName EnsureSpriteBound = "EnsureSpriteBound";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName SetFliter = "SetFliter";

		public static readonly StringName ApplyHalfCraterStage = "ApplyHalfCraterStage";

		public static readonly StringName SetFrame = "SetFrame";

		public static readonly StringName DieDown = "DieDown";

		public static readonly StringName SpawnWeatheredZombie = "SpawnWeatheredZombie";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseCharacter.PropertyName
	{
		public static readonly StringName HasPendingRevival = "HasPendingRevival";

		public static readonly StringName RevivalCamp = "RevivalCamp";

		public static readonly StringName halfCrater = "halfCrater";

		public static readonly StringName isWater = "isWater";

		public static readonly StringName isNight = "isNight";

		public static readonly StringName dieDownTimer = "dieDownTimer";

		public static readonly StringName stage = "stage";

		public static readonly StringName stageMax = "stageMax";

		public static readonly StringName timer = "timer";

		public static readonly StringName weatheringDisabled = "weatheringDisabled";

		public static readonly StringName spawnHypnoses = "spawnHypnoses";

		public static readonly StringName _halfCrater = "_halfCrater";

		public static readonly StringName _isWater = "_isWater";

		public static readonly StringName _isNight = "_isNight";
	}

	public new class SignalName : TowerDefenseCharacter.SignalName
	{
	}

	public WeatheringComponent weatheringComponent;

	public EnvironmentAnimeComponent environmentAnimeComponent;

	public double dieDownTimer;

	public int stage;

	public int stageMax;

	public double timer;

	public bool weatheringDisabled;

	public bool spawnHypnoses;

	private bool _halfCrater;

	private bool _isWater;

	private bool _isNight;

	public virtual bool HasPendingRevival => false;

	public TowerDefenseEnum.CHARACTER_CAMP RevivalCamp
	{
		get
		{
			if (!spawnHypnoses)
			{
				return TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
			}
			return TowerDefenseEnum.CHARACTER_CAMP.PLANT;
		}
	}

	public bool halfCrater
	{
		get
		{
			return _halfCrater;
		}
		set
		{
			if (_halfCrater != value)
			{
				_halfCrater = value;
				if (value)
				{
					ApplyHalfCraterStage();
				}
			}
		}
	}

	public bool isWater
	{
		get
		{
			return _isWater;
		}
		set
		{
			if (_isWater != value)
			{
				_isWater = value;
				sprite.Position = new Vector2(sprite.Position.X, 0f);
				timer = 0.0;
				SetFrame();
			}
		}
	}

	public bool isNight
	{
		get
		{
			return _isNight;
		}
		set
		{
			if (_isNight != value)
			{
				_isNight = value;
				SetFrame();
			}
		}
	}

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		EnsureSpriteBound();
		base._Ready();
		if (editorPreviewMode)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(componentManager))
		{
			weatheringComponent = componentManager.GetRuntime<WeatheringComponent>();
			environmentAnimeComponent = componentManager.GetRuntime<EnvironmentAnimeComponent>();
		}
		HitBoxDestroy();
		AddToGroup("Crater", persistent: true);
		if (config is TowerDefenseCraterConfig towerDefenseCraterConfig)
		{
			stageMax = towerDefenseCraterConfig.dieDownFliters.Count;
			if (stageMax > 0)
			{
				stage = Mathf.Clamp(stage, 0, stageMax - 1);
				SetFliter(stage);
			}
		}
		if (_halfCrater)
		{
			ApplyHalfCraterStage();
		}
		if (GodotObject.IsInstanceValid(cell))
		{
			isWater = cell.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER);
		}
	}

	private void EnsureSpriteBound()
	{
		if (!GodotObject.IsInstanceValid(sprite) && config is TowerDefenseCraterConfig towerDefenseCraterConfig && !string.IsNullOrEmpty(towerDefenseCraterConfig.name))
		{
			AdobeAnimateSprite nodeOrNull = GetNodeOrNull<AdobeAnimateSprite>("SpriteGroup/TransformPoint/" + towerDefenseCraterConfig.name);
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				sprite = nodeOrNull;
			}
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		isNight = TowerDefenseManager.GetMapIsNight();
		if (isWater)
		{
			timer = ((double?)environmentAnimeComponent?.WaterBob((float)delta, (float)timeScale)) ?? timer;
		}
		if (!weatheringDisabled)
		{
			weatheringComponent?.Processing((float)delta);
		}
	}

	public void SetFliter(int _stage)
	{
		if (!(config is TowerDefenseCraterConfig towerDefenseCraterConfig))
		{
			return;
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (string dieDownFliter in towerDefenseCraterConfig.dieDownFliters)
		{
			array.Add(dieDownFliter);
		}
		sprite.SetFliters(array, open: false);
		sprite.SetFliter(towerDefenseCraterConfig.dieDownFliters[_stage], open: true);
	}

	private void ApplyHalfCraterStage()
	{
		stage = 1;
		if (stageMax > 0 && GodotObject.IsInstanceValid(sprite))
		{
			SetFliter(stage);
		}
	}

	public void SetFrame()
	{
		environmentAnimeComponent?.SetFrame(isNight, isWater);
	}

	public virtual void DieDown()
	{
		Destroy();
	}

	protected void SpawnWeatheredZombie(string packetName)
	{
		if (isDestroy)
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			Destroy();
			return;
		}
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseZombie towerDefenseZombie = packetConfig.Create(logicalGlobalPosition, gridPos, groundHeight) as TowerDefenseZombie;
		if (!GodotObject.IsInstanceValid(towerDefenseZombie))
		{
			Destroy();
			return;
		}
		towerDefenseZombie.Set("over", true);
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseZombie, forceReadableName: false, InternalMode.Disabled);
		if (spawnHypnoses)
		{
			towerDefenseZombie.Hypnoses();
		}
		towerDefenseZombie.CallDeferred("Walk");
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseZombie);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, gridPos.X, gridPos.Y, nextSyncId, 1.0, 1.0, spawnHypnoses, 0.0, useCreate: true, logicalGlobalPosition.X, logicalGlobalPosition.Y, walkAfterSpawn: true, groundHeight);
			}
		}
		Destroy();
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "stage", stage },
			{ "dieDownTimer", dieDownTimer },
			{ "weatheringDisabled", weatheringDisabled },
			{ "spawnHypnoses", spawnHypnoses }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		int num = data.GetValueOrDefault("stage", stage).AsInt32();
		stage = ((stageMax > 0) ? Mathf.Clamp(num, 0, stageMax - 1) : Math.Max(0, num));
		dieDownTimer = Math.Max(0.0, data.GetValueOrDefault("dieDownTimer", dieDownTimer).AsDouble());
		weatheringDisabled = data.GetValueOrDefault("weatheringDisabled", weatheringDisabled).AsBool();
		spawnHypnoses = data.GetValueOrDefault("spawnHypnoses", false).AsBool();
		if (stageMax > 0 && GodotObject.IsInstanceValid(sprite))
		{
			SetFliter(stage);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureSpriteBound, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFliter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyHalfCraterStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnWeatheredZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.EnsureSpriteBound && args.Count == 0)
		{
			EnsureSpriteBound();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFliter && args.Count == 1)
		{
			SetFliter(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyHalfCraterStage && args.Count == 0)
		{
			ApplyHalfCraterStage();
			ret = default;
			return true;
		}
		if (method == MethodName.SetFrame && args.Count == 0)
		{
			SetFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.DieDown && args.Count == 0)
		{
			DieDown();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnWeatheredZombie && args.Count == 1)
		{
			SpawnWeatheredZombie(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.EnsureSpriteBound)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.SetFliter)
		{
			return true;
		}
		if (method == MethodName.ApplyHalfCraterStage)
		{
			return true;
		}
		if (method == MethodName.SetFrame)
		{
			return true;
		}
		if (method == MethodName.DieDown)
		{
			return true;
		}
		if (method == MethodName.SpawnWeatheredZombie)
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
		if (name == PropertyName.halfCrater)
		{
			halfCrater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isWater)
		{
			isWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isNight)
		{
			isNight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dieDownTimer)
		{
			dieDownTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.stage)
		{
			stage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.stageMax)
		{
			stageMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.weatheringDisabled)
		{
			weatheringDisabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spawnHypnoses)
		{
			spawnHypnoses = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._halfCrater)
		{
			_halfCrater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isWater)
		{
			_isWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isNight)
		{
			_isNight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.HasPendingRevival)
		{
			from = HasPendingRevival;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RevivalCamp)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.CHARACTER_CAMP>(RevivalCamp);
			return true;
		}
		if (name == PropertyName.halfCrater)
		{
			from = halfCrater;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.isWater)
		{
			from = isWater;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.isNight)
		{
			from = isNight;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dieDownTimer)
		{
			value = VariantUtils.CreateFrom(in dieDownTimer);
			return true;
		}
		if (name == PropertyName.stage)
		{
			value = VariantUtils.CreateFrom(in stage);
			return true;
		}
		if (name == PropertyName.stageMax)
		{
			value = VariantUtils.CreateFrom(in stageMax);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.weatheringDisabled)
		{
			value = VariantUtils.CreateFrom(in weatheringDisabled);
			return true;
		}
		if (name == PropertyName.spawnHypnoses)
		{
			value = VariantUtils.CreateFrom(in spawnHypnoses);
			return true;
		}
		if (name == PropertyName._halfCrater)
		{
			value = VariantUtils.CreateFrom(in _halfCrater);
			return true;
		}
		if (name == PropertyName._isWater)
		{
			value = VariantUtils.CreateFrom(in _isWater);
			return true;
		}
		if (name == PropertyName._isNight)
		{
			value = VariantUtils.CreateFrom(in _isNight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.dieDownTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stageMax, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.weatheringDisabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spawnHypnoses, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasPendingRevival, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RevivalCamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._halfCrater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.halfCrater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isWater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isWater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isNight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isNight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.halfCrater, Variant.From<bool>(halfCrater));
		info.AddProperty(PropertyName.isWater, Variant.From<bool>(isWater));
		info.AddProperty(PropertyName.isNight, Variant.From<bool>(isNight));
		info.AddProperty(PropertyName.dieDownTimer, Variant.From(in dieDownTimer));
		info.AddProperty(PropertyName.stage, Variant.From(in stage));
		info.AddProperty(PropertyName.stageMax, Variant.From(in stageMax));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.weatheringDisabled, Variant.From(in weatheringDisabled));
		info.AddProperty(PropertyName.spawnHypnoses, Variant.From(in spawnHypnoses));
		info.AddProperty(PropertyName._halfCrater, Variant.From(in _halfCrater));
		info.AddProperty(PropertyName._isWater, Variant.From(in _isWater));
		info.AddProperty(PropertyName._isNight, Variant.From(in _isNight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.halfCrater, out var value))
		{
			halfCrater = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isWater, out var value2))
		{
			isWater = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isNight, out var value3))
		{
			isNight = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dieDownTimer, out var value4))
		{
			dieDownTimer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.stage, out var value5))
		{
			stage = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.stageMax, out var value6))
		{
			stageMax = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value7))
		{
			timer = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.weatheringDisabled, out var value8))
		{
			weatheringDisabled = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spawnHypnoses, out var value9))
		{
			spawnHypnoses = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._halfCrater, out var value10))
		{
			_halfCrater = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isWater, out var value11))
		{
			_isWater = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isNight, out var value12))
		{
			_isNight = value12.As<bool>();
		}
	}
}
