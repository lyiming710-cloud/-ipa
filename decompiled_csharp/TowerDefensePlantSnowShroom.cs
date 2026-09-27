using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter2/SnowShroom/Scene/TowerDefensePlantSnowShroom.cs")]
public class TowerDefensePlantSnowShroom : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName SleepEntered = "SleepEntered";

		public new static readonly StringName SleepProcessing = "SleepProcessing";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName Activate = "Activate";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName CreateProjectile = "CreateProjectile";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName eventList = "eventList";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName _activated = "_activated";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	public string projectileName = "SnowPea";

	public string skinName = "Default";

	private bool _activated;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && currentCustom.Contains("Custom0"))
		{
			skinName = "Sward";
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			skinName = "Sward";
		}
		else
		{
			skinName = "Default";
		}
	}

	public override void SleepEntered()
	{
		base.SleepEntered();
		instance.invincible = false;
	}

	public override void SleepProcessing(double delta)
	{
		base.SleepProcessing(delta);
		instance.invincible = false;
	}

	public override void IdleEntered()
	{
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) && TowerDefenseManager.Instance.currentControl.isGameRunning && inGame)
		{
			if (CanSleep())
			{
				Sleep();
			}
			else if (IsIzmOneShotGateOpen())
			{
				Activate();
			}
		}
	}

	private void Activate()
	{
		if (!_activated)
		{
			_activated = true;
			base.IdleEntered();
			CreateProjectile();
			HitBoxDestroy();
			instance.invincible = true;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (!_activated && IsIzmOneShotGateOpen())
		{
			Activate();
		}
	}

	public async void CreateProjectile()
	{
		bool isRemoteClient = Global.IsMultiplayerMode && !MultiPlayerManager.IsHost;
		Vector2I gridNum = default;
		Vector2 gridSize = default;
		double height = 600.0;
		double ownerGroundHeight = 0.0;
		TowerDefenseProjectileConfig projectileConfig = null;
		if (!isRemoteClient)
		{
			gridNum = TowerDefenseManager.Instance.GetMapGridNum();
			gridSize = TowerDefenseManager.Instance.GetMapGridSize();
			TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData(projectileName);
			towerDefenseProjectileCreateData.skinName = skinName;
			towerDefenseProjectileCreateData.baseDamage = 50.0;
			towerDefenseProjectileCreateData.damageFlags = 2;
			towerDefenseProjectileCreateData.collisionFlags = 35;
			projectileConfig = towerDefenseProjectileCreateData.BuildConfig();
			ownerGroundHeight = GetGroundHeight(GetLogicalGlobalPosition().Y);
		}
		for (int id = 0; id < 25; id++)
		{
			if (!isRemoteClient)
			{
				for (int i = 1; i <= gridNum.X; i++)
				{
					for (int j = 1; j <= gridNum.Y; j++)
					{
						TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(i, j));
						Vector2 pos = TowerDefenseManager.GetMapCellPlantPos(new Vector2I(i, j)) - new Vector2(150f, 0f) + new Vector2((float)GD.RandRange((0f - gridSize.X) / 2f, gridSize.X / 2f), 0f);
						double num = GD.RandRange(0, 200);
						BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
						{
							useFall = true,
							gridYOverride = j,
							zOverride = height + num,
							ySpeedOverride = 400.0
						};
						FireComponent.CreateProjectilePositionByConfig(null, null, (float)(ownerGroundHeight + 30.0 - mapCell.GetGroundHeight()), pos, new Vector2(GD.RandRange(50, 150), 0f), projectileConfig, -1, camp, default, overrides);
					}
				}
			}
			await ToSignal(GetTree().CreateTimer(0.3, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		if (isRemoteClient)
		{
			TowerDefenseCharacter.CreateColdVisualEffect(gridPos);
		}
		else
		{
			TowerDefenseCharacter.CreateColdEffect(camp, gridPos);
		}
		Destroy();
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "projectileName", projectileName },
			{ "skinName", skinName }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		projectileName = data.GetValueOrDefault("projectileName", "SnowPea").AsString();
		skinName = data.GetValueOrDefault("skinName", "Default").AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SleepEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SleepProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Activate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SleepEntered && args.Count == 0)
		{
			SleepEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.SleepProcessing && args.Count == 1)
		{
			SleepProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.Activate && args.Count == 0)
		{
			Activate();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateProjectile && args.Count == 0)
		{
			CreateProjectile();
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
		if (method == MethodName.OnCustomSwitched)
		{
			return true;
		}
		if (method == MethodName.SleepEntered)
		{
			return true;
		}
		if (method == MethodName.SleepProcessing)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.Activate)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.CreateProjectile)
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
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
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
		if (name == PropertyName._activated)
		{
			_activated = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom(in projectileName);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			value = VariantUtils.CreateFrom(in skinName);
			return true;
		}
		if (name == PropertyName._activated)
		{
			value = VariantUtils.CreateFrom(in _activated);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._activated, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.projectileName, Variant.From(in projectileName));
		info.AddProperty(PropertyName.skinName, Variant.From(in skinName));
		info.AddProperty(PropertyName._activated, Variant.From(in _activated));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventList, out var value))
		{
			eventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value2))
		{
			projectileName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.skinName, out var value3))
		{
			skinName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._activated, out var value4))
		{
			_activated = value4.As<bool>();
		}
	}
}
