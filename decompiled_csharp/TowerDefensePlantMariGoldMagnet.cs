using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter6/MariGoldMagnet/Scene/TowerDefensePlantMariGoldMagnet.cs")]
public class TowerDefensePlantMariGoldMagnet : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName MagnetEntered = "MagnetEntered";

		public static readonly StringName MagnetProcessing = "MagnetProcessing";

		public static readonly StringName MagnetExited = "MagnetExited";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName CoinGet = "CoinGet";

		public static readonly StringName CreateProjectile = "CreateProjectile";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName produceType = "produceType";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _produceType = "_produceType";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName coinNumList = "coinNumList";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	public ProduceComponent produceComponent;

	public MagnetCoinComponent magnetCoinComponent;

	public FireComponent fireComponent;

	private StateHandle _magnetState;

	private bool _roleStateSignalsConnected;

	private double _produceInterval = 25.0;

	private string _produceType = "Sun";

	public string skinName = "Default";

	public Array<int> coinNumList = new Array<int> { 0, 0, 0 };

	[Export(PropertyHint.None, "")]
	public double produceInterval
	{
		get
		{
			return _produceInterval;
		}
		set
		{
			_produceInterval = value;
			if (IsNodeReady() && this.produceComponent != null)
			{
				ProduceComponent produceComponent = this.produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					this.produceComponent.produceInterval = (float)value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public string produceType
	{
		get
		{
			return _produceType;
		}
		set
		{
			_produceType = value;
			if (IsNodeReady() && this.produceComponent != null)
			{
				ProduceComponent produceComponent = this.produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					this.produceComponent.produceType = value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			produceComponent = componentManager.GetRuntime<ProduceComponent>();
			magnetCoinComponent = componentManager.GetRuntime<MagnetCoinComponent>();
			if (magnetCoinComponent != null)
			{
				magnetCoinComponent.OnCoinGet += CoinGet;
			}
			fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			AddToGroup("GoldMagnet");
			produceComponent.produceInterval = (float)produceInterval;
			if (currentCustom.Contains("Custom0"))
			{
				skinName = "Sycee";
			}
			_magnetState = StateMachine?.GetStateById("plant.mari_gold_magnet.magnet");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		MagnetCoinComponent magnetCoinComponent = this.magnetCoinComponent;
		if (magnetCoinComponent != null && !magnetCoinComponent.IsReleased)
		{
			this.magnetCoinComponent.OnCoinGet -= CoinGet;
		}
		base._ExitTree();
	}

	private void ConnectRoleStateSignals()
	{
		if (!_roleStateSignalsConnected)
		{
			StateHandle magnetState = _magnetState;
			if (magnetState != null && magnetState.IsValid)
			{
				_magnetState.Entered += MagnetEntered;
				_magnetState.Exited += MagnetExited;
				_magnetState.PhysicsProcessing += MagnetProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			_magnetState.Entered -= MagnetEntered;
			_magnetState.Exited -= MagnetExited;
			_magnetState.PhysicsProcessing -= MagnetProcessing;
			_magnetState = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			skinName = "Sycee";
		}
		else
		{
			skinName = "Default";
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		MagnetCoinComponent magnetCoinComponent = this.magnetCoinComponent;
		if (magnetCoinComponent != null && magnetCoinComponent.CanCoinDraw())
		{
			SendStateEvent("ToMagnet");
		}
	}

	public virtual void MagnetEntered()
	{
		sprite.SetAnimation("Attack", loop: false, 0.2);
	}

	public virtual void MagnetProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public virtual void MagnetExited()
	{
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "action")
		{
			magnetCoinComponent?.CoinDraw();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Attack")
		{
			CreateProjectile();
			Idle();
		}
	}

	public virtual void CoinGet(TowerDefenseCoinBase coin)
	{
		switch (coin.num)
		{
		case 10:
			coinNumList[0]++;
			break;
		case 50:
			coinNumList[1]++;
			break;
		case 1000:
			coinNumList[2]++;
			break;
		}
	}

	public async void CreateProjectile()
	{
		if (!fireComponent.CanFireCheckOnceByName("CoinSilver", instance.collisionFlags))
		{
			return;
		}
		if (coinNumList[2] > 0)
		{
			for (int i = 0; i < 5; i++)
			{
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData("CoinDiamond");
				towerDefenseProjectileCreateData.skinName = skinName;
				towerDefenseProjectileCreateData.baseDamage = 1000.0;
				towerDefenseProjectileCreateData.collisionFlags = 11;
				towerDefenseProjectileCreateData.fireMethodFlags = 32;
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					gridYOverride = gridPos.Y,
					flipXOverride = (Scale.X < 0f)
				};
				fireComponent.CreateProjectileByData(0, new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, camp, Vector2.Zero, overrides);
				coinNumList[2]--;
				if (coinNumList[2] <= 0)
				{
					break;
				}
				await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
			}
		}
		else if (coinNumList[1] > 0)
		{
			for (int i = 0; i < 5; i++)
			{
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData("CoinGold");
				towerDefenseProjectileCreateData.skinName = skinName;
				towerDefenseProjectileCreateData.baseDamage = 500.0;
				towerDefenseProjectileCreateData.collisionFlags = 11;
				towerDefenseProjectileCreateData.fireMethodFlags = 32;
				BulletFieldSpawnOverrides overrides2 = new BulletFieldSpawnOverrides
				{
					gridYOverride = gridPos.Y,
					flipXOverride = (Scale.X < 0f)
				};
				fireComponent.CreateProjectileByData(0, new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, camp, Vector2.Zero, overrides2);
				coinNumList[1]--;
				if (coinNumList[1] <= 0)
				{
					break;
				}
				await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
			}
		}
		else
		{
			if (coinNumList[0] <= 0)
			{
				return;
			}
			for (int i = 0; i < 5; i++)
			{
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData("CoinSilver");
				towerDefenseProjectileCreateData.skinName = skinName;
				towerDefenseProjectileCreateData.baseDamage = 100.0;
				towerDefenseProjectileCreateData.collisionFlags = 11;
				towerDefenseProjectileCreateData.fireMethodFlags = 32;
				BulletFieldSpawnOverrides overrides3 = new BulletFieldSpawnOverrides
				{
					gridYOverride = gridPos.Y,
					flipXOverride = (Scale.X < 0f)
				};
				fireComponent.CreateProjectileByData(0, new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, camp, Vector2.Zero, overrides3);
				coinNumList[0]--;
				if (coinNumList[0] <= 0)
				{
					break;
				}
				await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "produceInterval", produceInterval },
			{ "skinName", skinName }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		produceInterval = data.GetValueOrDefault("produceInterval", 25.0).AsDouble();
		skinName = data.GetValueOrDefault("skinName", "Default").AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MagnetEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MagnetProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MagnetExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CoinGet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "coin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MagnetEntered && args.Count == 0)
		{
			MagnetEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.MagnetProcessing && args.Count == 1)
		{
			MagnetProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MagnetExited && args.Count == 0)
		{
			MagnetExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CoinGet && args.Count == 1)
		{
			CoinGet(VariantUtils.ConvertTo<TowerDefenseCoinBase>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.OnCustomSwitched)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.MagnetEntered)
		{
			return true;
		}
		if (method == MethodName.MagnetProcessing)
		{
			return true;
		}
		if (method == MethodName.MagnetExited)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CoinGet)
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
		if (name == PropertyName.produceInterval)
		{
			produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._produceInterval)
		{
			_produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			_produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.coinNumList)
		{
			coinNumList = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.produceInterval)
		{
			value = VariantUtils.CreateFrom<double>(produceInterval);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			value = VariantUtils.CreateFrom<string>(produceType);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._produceInterval)
		{
			value = VariantUtils.CreateFrom(in _produceInterval);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			value = VariantUtils.CreateFrom(in _produceType);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			value = VariantUtils.CreateFrom(in skinName);
			return true;
		}
		if (name == PropertyName.coinNumList)
		{
			value = VariantUtils.CreateFromArray(coinNumList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._produceInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._produceType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.coinNumList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
		info.AddProperty(PropertyName.produceType, Variant.From<string>(produceType));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._produceType, Variant.From(in _produceType));
		info.AddProperty(PropertyName.skinName, Variant.From(in skinName));
		info.AddProperty(PropertyName.coinNumList, Variant.CreateFrom(coinNumList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.produceInterval, out var value))
		{
			produceInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.produceType, out var value2))
		{
			produceType = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value3))
		{
			_roleStateSignalsConnected = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._produceInterval, out var value4))
		{
			_produceInterval = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._produceType, out var value5))
		{
			_produceType = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.skinName, out var value6))
		{
			skinName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.coinNumList, out var value7))
		{
			coinNumList = value7.AsGodotArray<int>();
		}
	}
}
