using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Award/Purse/TowerDefenseAwardPurse.cs")]
public class TowerDefenseAwardPurse : TowerDefenseAwardBase
{
	public new class MethodName : TowerDefenseAwardBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Pressed = "Pressed";

		public static readonly StringName CreateCoin = "CreateCoin";
	}

	public new class PropertyName : TowerDefenseAwardBase.PropertyName
	{
		public new static readonly StringName awardRay = "awardRay";

		public new static readonly StringName awardPickupGlow = "awardPickupGlow";

		public new static readonly StringName downArrow = "downArrow";

		public static readonly StringName num = "num";
	}

	public new class SignalName : TowerDefenseAwardBase.SignalName
	{
	}

	private static PackedScene _towerDefenseCoinGold;

	private static PackedScene _towerDefenseCoinSilver;

	private AwardRay awardRay;

	private Sprite2D awardPickupGlow;

	private Sprite2D downArrow;

	public int num = 250;

	private static PackedScene TOWER_DEFENSE_COIN_GOLD => _towerDefenseCoinGold ?? (_towerDefenseCoinGold = GD.Load<PackedScene>("uid://kbif4idtgolo"));

	private static PackedScene TOWER_DEFENSE_COIN_SILVER => _towerDefenseCoinSilver ?? (_towerDefenseCoinSilver = GD.Load<PackedScene>("uid://csynbfevdbiju"));

	public override void _Ready()
	{
		base._Ready();
		awardRay = GetNodeOrNull<AwardRay>("%AwardRay");
		awardPickupGlow = GetNodeOrNull<Sprite2D>("%AwardPickupGlow");
		downArrow = GetNodeOrNull<Sprite2D>("%DownArrow");
		GetNode<Button>("%Button").Pressed += Pressed;
	}

	public override void Init(string value)
	{
		num = int.Parse(value);
	}

	public override async void Pressed()
	{
		if (press)
		{
			return;
		}
		press = true;
		CreateCoin();
		AudioManager.Instance.AudioStopAll();
		AudioManager.Instance.AudioPlay("Win", AudioManagerEnum.TYPE.MUSIC);
		AudioManager.Instance.AudioPlay("AwardLightFill", AudioManagerEnum.TYPE.MUSIC);
		awardPickupGlow.Visible = false;
		downArrow.Visible = false;
		awardRay.Emit();
		Vector2 screenCenterPosition = GetViewport().GetCamera2D().GetScreenCenterPosition();
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Quart);
		tween.TweenProperty(this, "global_position", screenCenterPosition, 5.0);
		tween.TweenProperty(this, "scale", Vector2.One * 2f, 7.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		Global.Instance.currentAwardMode = true;
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			MultiPlayerManager.Instance.SendClientReady();
			SceneManager.Instance.ChangeScene("MainMenu");
			return;
		}
		switch (Global.Instance.enterLevelMode)
		{
		case "LevelChoose":
			if (Global.Instance.currentLevelChoose != "TryLevel")
			{
				TrySelectNextLevelFromLevelChoose();
				SceneManager.Instance.ChangeScene("LevelChoose");
			}
			else
			{
				SceneManager.Instance.ChangeScene("MainMenu");
			}
			break;
		case "DailyLevel":
			SceneManager.Instance.ChangeScene("MainMenu");
			break;
		case "DiyLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		case "LoadLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		case "OnlineLevel":
			SceneManager.Instance.ChangeScene("LevelEditorStage");
			break;
		}
	}

	public async void CreateCoin()
	{
		while (num >= 1000)
		{
			TowerDefenseCoinBase towerDefenseCoinBase = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, GlobalPosition - new Vector2(0f, 40f), 120.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0) as TowerDefenseCoinBase;
			towerDefenseCoinBase.gridPos = new Vector2I(towerDefenseCoinBase.gridPos.X, 200);
			towerDefenseCoinBase.canMagnet = false;
			GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase.moveComponent.MoveClear;
			GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase.Collection;
			num -= 1000;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 50)
		{
			TowerDefenseCoinBase towerDefenseCoinBase2 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, GlobalPosition - new Vector2(0f, 40f), 120.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0) as TowerDefenseCoinBase;
			towerDefenseCoinBase2.gridPos = new Vector2I(towerDefenseCoinBase2.gridPos.X, 200);
			towerDefenseCoinBase2.canMagnet = false;
			GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase2.moveComponent.MoveClear;
			GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase2.Collection;
			num -= 50;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
		while (num >= 10)
		{
			TowerDefenseCoinBase towerDefenseCoinBase3 = TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, GlobalPosition - new Vector2(0f, 40f), 120.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0) as TowerDefenseCoinBase;
			towerDefenseCoinBase3.gridPos = new Vector2I(towerDefenseCoinBase3.gridPos.X, 200);
			towerDefenseCoinBase3.canMagnet = false;
			GetTree().CreateTimer(1.0, processAlways: false).Timeout += towerDefenseCoinBase3.moveComponent.MoveClear;
			GetTree().CreateTimer(1.5, processAlways: false).Timeout += towerDefenseCoinBase3.Collection;
			num -= 10;
			await ToSignal(GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Pressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Pressed && args.Count == 0)
		{
			Pressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCoin && args.Count == 0)
		{
			CreateCoin();
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Pressed)
		{
			return true;
		}
		if (method == MethodName.CreateCoin)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.awardRay)
		{
			awardRay = VariantUtils.ConvertTo<AwardRay>(in value);
			return true;
		}
		if (name == PropertyName.awardPickupGlow)
		{
			awardPickupGlow = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.downArrow)
		{
			downArrow = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.awardRay)
		{
			value = VariantUtils.CreateFrom(in awardRay);
			return true;
		}
		if (name == PropertyName.awardPickupGlow)
		{
			value = VariantUtils.CreateFrom(in awardPickupGlow);
			return true;
		}
		if (name == PropertyName.downArrow)
		{
			value = VariantUtils.CreateFrom(in downArrow);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.awardRay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.awardPickupGlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.downArrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.awardRay, Variant.From(in awardRay));
		info.AddProperty(PropertyName.awardPickupGlow, Variant.From(in awardPickupGlow));
		info.AddProperty(PropertyName.downArrow, Variant.From(in downArrow));
		info.AddProperty(PropertyName.num, Variant.From(in num));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.awardRay, out var value))
		{
			awardRay = value.As<AwardRay>();
		}
		if (info.TryGetProperty(PropertyName.awardPickupGlow, out var value2))
		{
			awardPickupGlow = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.downArrow, out var value3))
		{
			downArrow = value3.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value4))
		{
			num = value4.As<int>();
		}
	}
}
