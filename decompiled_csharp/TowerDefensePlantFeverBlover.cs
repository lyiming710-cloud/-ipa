using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/FeverBlover/Scene/TowerDefensePlantFeverBlover.cs")]
public class TowerDefensePlantFeverBlover : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName Run = "Run";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName ExplodeHurt = "ExplodeHurt";

		public static readonly StringName BlowOver = "BlowOver";

		public static readonly StringName _SafetyDestroy = "_SafetyDestroy";

		public static readonly StringName CoinGet = "CoinGet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName _projectileName = "_projectileName";

		public static readonly StringName _skinName = "_skinName";

		public static readonly StringName run = "run";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private BloverComponent _bloverComponent;

	private MagnetCoinComponent _magnetCoinComponent;

	private FireComponent _fireComponent;

	private string _projectileName = "Spike";

	private string _skinName = "Default";

	public bool run;

	public string projectileName
	{
		get
		{
			return _projectileName;
		}
		set
		{
			_projectileName = value;
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData(value);
			towerDefenseProjectileCreateData.skinName = skinName;
			BloverComponent bloverComponent = _bloverComponent;
			if (bloverComponent != null && !bloverComponent.IsReleased)
			{
				_bloverComponent.projectileDataList = new Array<TowerDefenseProjectileCreateData> { towerDefenseProjectileCreateData };
			}
		}
	}

	public string skinName
	{
		get
		{
			return _skinName;
		}
		set
		{
			_skinName = value;
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData(projectileName);
			towerDefenseProjectileCreateData.skinName = value;
			BloverComponent bloverComponent = _bloverComponent;
			if (bloverComponent != null && !bloverComponent.IsReleased)
			{
				_bloverComponent.projectileDataList = new Array<TowerDefenseProjectileCreateData> { towerDefenseProjectileCreateData };
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_bloverComponent = componentManager.GetRuntime<BloverComponent>();
			BloverComponent bloverComponent = _bloverComponent;
			if (bloverComponent != null && !bloverComponent.IsReleased)
			{
				_bloverComponent.OnBlowOver += BlowOver;
			}
			_magnetCoinComponent = componentManager.GetRuntime<MagnetCoinComponent>();
			MagnetCoinComponent magnetCoinComponent = _magnetCoinComponent;
			if (magnetCoinComponent != null && !magnetCoinComponent.IsReleased)
			{
				_magnetCoinComponent.OnCoinGet += CoinGet;
			}
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			AddToGroup("GoldMagnet");
			if (currentCustom.Contains("Custom0"))
			{
				skinName = "Knife";
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		BloverComponent bloverComponent = _bloverComponent;
		if (bloverComponent != null && !bloverComponent.IsReleased)
		{
			_bloverComponent.OnBlowOver -= BlowOver;
		}
		MagnetCoinComponent magnetCoinComponent = _magnetCoinComponent;
		if (magnetCoinComponent != null && !magnetCoinComponent.IsReleased)
		{
			_magnetCoinComponent.OnCoinGet -= CoinGet;
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			skinName = "Knife";
		}
		else
		{
			skinName = "Default";
		}
	}

	public override void IdleEntered()
	{
		if (inGame && TowerDefenseManager.Instance.currentControl.isGameRunning)
		{
			Run();
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
		if (!inGame || !TowerDefenseManager.Instance.currentControl.isGameRunning)
		{
			return;
		}
		if (!run)
		{
			Run();
			return;
		}
		MagnetCoinComponent magnetCoinComponent = _magnetCoinComponent;
		if (magnetCoinComponent != null && !magnetCoinComponent.IsReleased && _magnetCoinComponent.CanCoinDraw())
		{
			_magnetCoinComponent.CoinDraw();
		}
	}

	public void Run()
	{
		sprite.SetAnimation("Blow", loop: false, 0.2);
		sprite.AddAnimation("Loop", 0.0);
		instance.invincible = true;
		run = true;
		GetTree().CreateTimer(5.0, processAlways: false).Timeout += _SafetyDestroy;
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "blow")
		{
			BloverComponent bloverComponent = _bloverComponent;
			if (bloverComponent != null && !bloverComponent.IsReleased)
			{
				_bloverComponent.Execult();
			}
			for (int i = 1; i <= TowerDefenseManager.Instance.GetMapGridNum().Y; i++)
			{
				TowerDefenseCharacter.CreateJalapenoFire(camp, new Vector2I(gridPos.X, i), 500.0, new Array<TowerDefenseCharacterEventBase>(), new Array<TowerDefenseCharacterEventBase>());
			}
		}
	}

	public override double ExplodeHurt(double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		instance.invincible = false;
		return base.ExplodeHurt(num, damageKind, playSplatAudio, velocity);
	}

	public void BlowOver()
	{
		Destroy();
	}

	private void _SafetyDestroy()
	{
		if (!isDestroy)
		{
			Destroy();
		}
	}

	public void CoinGet(TowerDefenseCoinBase coin)
	{
		switch (coin.num)
		{
		case 10:
		{
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData3 = new TowerDefenseProjectileCreateData("CoinSilver");
			towerDefenseProjectileCreateData3.baseDamage = 100.0;
			towerDefenseProjectileCreateData3.fireMethodFlags = 32;
			BulletFieldSpawnOverrides overrides3 = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y,
				flipXOverride = (Scale.X < 0f)
			};
			_fireComponent.CreateProjectileByData(0, new Vector2(300f, 0f), towerDefenseProjectileCreateData3, -1, camp, Vector2.Zero, overrides3);
			break;
		}
		case 50:
		{
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData2 = new TowerDefenseProjectileCreateData("CoinGold");
			towerDefenseProjectileCreateData2.baseDamage = 500.0;
			towerDefenseProjectileCreateData2.fireMethodFlags = 32;
			BulletFieldSpawnOverrides overrides2 = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y,
				flipXOverride = (Scale.X < 0f)
			};
			_fireComponent.CreateProjectileByData(0, new Vector2(300f, 0f), towerDefenseProjectileCreateData2, -1, camp, Vector2.Zero, overrides2);
			break;
		}
		case 1000:
		{
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData("CoinDiamond");
			towerDefenseProjectileCreateData.baseDamage = 1000.0;
			towerDefenseProjectileCreateData.fireMethodFlags = 32;
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y,
				flipXOverride = (Scale.X < 0f)
			};
			_fireComponent.CreateProjectileByData(0, new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, camp, Vector2.Zero, overrides);
			break;
		}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["projectileName"] = projectileName,
			["skinName"] = skinName,
			["run"] = run
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		projectileName = (data.ContainsKey("projectileName") ? data["projectileName"].AsString() : "Spike");
		skinName = (data.ContainsKey("skinName") ? data["skinName"].AsString() : "Default");
		run = data.ContainsKey("run") && data["run"].AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BlowOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._SafetyDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CoinGet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "coin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 0)
		{
			Run();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExplodeHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ExplodeHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.BlowOver && args.Count == 0)
		{
			BlowOver();
			ret = default;
			return true;
		}
		if (method == MethodName._SafetyDestroy && args.Count == 0)
		{
			_SafetyDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.CoinGet && args.Count == 1)
		{
			CoinGet(VariantUtils.ConvertTo<TowerDefenseCoinBase>(in args[0]));
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
		if (method == MethodName.OnCustomSwitched)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.ExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.BlowOver)
		{
			return true;
		}
		if (method == MethodName._SafetyDestroy)
		{
			return true;
		}
		if (method == MethodName.CoinGet)
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
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			_skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.run)
		{
			run = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.projectileName)
		{
			from = projectileName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			from = skinName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			value = VariantUtils.CreateFrom(in _skinName);
			return true;
		}
		if (name == PropertyName.run)
		{
			value = VariantUtils.CreateFrom(in run);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.run, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
		info.AddProperty(PropertyName.skinName, Variant.From<string>(skinName));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
		info.AddProperty(PropertyName._skinName, Variant.From(in _skinName));
		info.AddProperty(PropertyName.run, Variant.From(in run));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileName, out var value))
		{
			projectileName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.skinName, out var value2))
		{
			skinName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value3))
		{
			_projectileName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._skinName, out var value4))
		{
			_skinName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.run, out var value5))
		{
			run = value5.As<bool>();
		}
	}
}
