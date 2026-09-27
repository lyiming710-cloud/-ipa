using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Colour/MoneyNut/Scene/TowerDefensePlantMoneyNut.cs")]
public class TowerDefensePlantMoneyNut : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName MagnetEntered = "MagnetEntered";

		public static readonly StringName MagnetProcessing = "MagnetProcessing";

		public static readonly StringName MagnetExited = "MagnetExited";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName CoinGet = "CoinGet";

		public static readonly StringName CreateProjectile = "CreateProjectile";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName coinNumList = "coinNumList";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private MagnetCoinComponent _magnetCoinComponent;

	private StateHandle _magnetState;

	private bool _roleStateSignalsConnected;

	public int[] coinNumList = new int[3];

	public bool over;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_magnetCoinComponent = componentManager.GetRuntime<MagnetCoinComponent>();
			if (_magnetCoinComponent != null)
			{
				_magnetCoinComponent.OnCoinGet += CoinGet;
			}
			AddToGroup("GoldMagnet");
			_magnetState = StateMachine?.GetStateById("plant.money_nut.magnet");
			ConnectRoleStateSignals();
		}
	}

	public override void _ExitTree()
	{
		MagnetCoinComponent magnetCoinComponent = _magnetCoinComponent;
		if (magnetCoinComponent != null && !magnetCoinComponent.IsReleased)
		{
			_magnetCoinComponent.OnCoinGet -= CoinGet;
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

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		MagnetCoinComponent magnetCoinComponent = _magnetCoinComponent;
		if (magnetCoinComponent != null && magnetCoinComponent.CanCoinDraw())
		{
			SendStateEvent("ToMagnet");
		}
	}

	public void MagnetEntered()
	{
		sprite.SetAnimation("Action", loop: false, 0.2);
	}

	public void MagnetProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void MagnetExited()
	{
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "action")
		{
			_magnetCoinComponent?.CoinDraw();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Action")
		{
			Idle();
		}
	}

	public void CoinGet(TowerDefenseCoinBase coin)
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

	public void CreateProjectile()
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		if (coinNumList[2] > 0)
		{
			for (int i = 0; i < coinNumList[2]; i++)
			{
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = new TowerDefenseProjectileCreateData("CoinDiamond");
				towerDefenseProjectileCreateData.baseDamage = 1000.0;
				towerDefenseProjectileCreateData.fireMethodFlags = 32;
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					gridYOverride = gridPos.Y,
					flipXOverride = (Scale.X < 0f)
				};
				FireComponent.CreateProjectilePositionByData(null, null, 0.0, logicalGlobalPosition, new Vector2(300f, 0f), towerDefenseProjectileCreateData, -1, camp, Vector2.Zero, overrides);
			}
			coinNumList[2] = 0;
		}
		if (coinNumList[1] > 0)
		{
			for (int j = 0; j < coinNumList[1]; j++)
			{
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData2 = new TowerDefenseProjectileCreateData("CoinGold");
				towerDefenseProjectileCreateData2.baseDamage = 500.0;
				towerDefenseProjectileCreateData2.fireMethodFlags = 32;
				BulletFieldSpawnOverrides overrides2 = new BulletFieldSpawnOverrides
				{
					gridYOverride = gridPos.Y,
					flipXOverride = (Scale.X < 0f)
				};
				FireComponent.CreateProjectilePositionByData(null, null, 0.0, logicalGlobalPosition, new Vector2(300f, 0f), towerDefenseProjectileCreateData2, -1, camp, Vector2.Zero, overrides2);
			}
			coinNumList[1] = 0;
		}
		if (coinNumList[0] > 0)
		{
			for (int k = 0; k < coinNumList[0]; k++)
			{
				TowerDefenseProjectileCreateData towerDefenseProjectileCreateData3 = new TowerDefenseProjectileCreateData("CoinSilver");
				towerDefenseProjectileCreateData3.baseDamage = 100.0;
				towerDefenseProjectileCreateData3.fireMethodFlags = 32;
				BulletFieldSpawnOverrides overrides3 = new BulletFieldSpawnOverrides
				{
					gridYOverride = gridPos.Y,
					flipXOverride = (Scale.X < 0f)
				};
				FireComponent.CreateProjectilePositionByData(null, null, 0.0, logicalGlobalPosition, new Vector2(300f, 0f), towerDefenseProjectileCreateData3, -1, camp, Vector2.Zero, overrides3);
			}
			coinNumList[0] = 0;
		}
	}

	public override void DestroySet()
	{
		if (!over)
		{
			over = true;
			CreateProjectile();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["coinNumList"] = new Array(coinNumList.Select((int x) => Variant.From(in x)).ToArray()) };
	}

	public override async void ImportVariantSave(Dictionary data)
	{
		if (data.ContainsKey("coinNumList"))
		{
			Array array = data["coinNumList"].AsGodotArray();
			coinNumList = new int[3]
			{
				array[0].AsInt32(),
				array[1].AsInt32(),
				array[2].AsInt32()
			};
		}
		else
		{
			coinNumList = new int[3];
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
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
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.coinNumList)
		{
			coinNumList = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName.coinNumList)
		{
			value = VariantUtils.CreateFrom(in coinNumList);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
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
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.coinNumList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName.coinNumList, Variant.From(in coinNumList));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.coinNumList, out var value2))
		{
			coinNumList = value2.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value3))
		{
			over = value3.As<bool>();
		}
	}
}
