using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter5/YetiFootball/Scene/TowerDefenseZombieYetiFootball.cs")]
public class TowerDefenseZombieYetiFootball : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName TryConsumeIncomingBuff = "TryConsumeIncomingBuff";

		public static readonly StringName GetBuffAdd = "GetBuffAdd";

		public static readonly StringName ApplyTemperatureResponse = "ApplyTemperatureResponse";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			timeScaleInit = 1.5;
			sprite.SetFliters(new Array { "anim_face" }, open: false);
		}
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		sprite.timeScale = timeScale * 1.0;
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 1.0;
	}

	public override async void DestroySet()
	{
		if ((!Global.Instance.isEditor || !(Global.Instance.enterLevelMode == "DiyLevel")) && !(Global.Instance.enterLevelMode == "LoadLevel") && !(Global.Instance.enterLevelMode == "OnlineLevel"))
		{
			float num = GD.Randf();
			Vector2 pos = GetLogicalGlobalPosition() - new Vector2(0f, 40f);
			if ((double)num <= 0.02)
			{
				TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_DIAMOND, pos, 120.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0).gridPos = gridPos;
			}
			else if ((double)num < 0.2)
			{
				TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_GOLD, pos, 120.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0).gridPos = gridPos;
			}
			else
			{
				TowerDefenseManager.Instance.FallingObjectItemCreate(ObjectManagerConfig.OBJECT.COIN_SILVER, pos, 120.0, new Vector2((float)GD.RandRange(-100.0, 100.0), -400f), 980.0).gridPos = gridPos;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	public override bool TryConsumeIncomingBuff(TowerDefenseCharacterBuffConfig buffConfig)
	{
		if (buffConfig != null)
		{
			return ApplyTemperatureResponse(buffConfig.key);
		}
		return false;
	}

	public void GetBuffAdd(string key)
	{
		if (ApplyTemperatureResponse(key))
		{
			buff?.DeleteBuff(key);
		}
	}

	private bool ApplyTemperatureResponse(string key)
	{
		switch (key)
		{
		case "IceSpeedDown":
		case "Frozen":
			timeScaleInit = 3.0;
			timeScale = timeScaleInit;
			sprite.SetFliters(new Array { "anim_face" }, open: false);
			return true;
		case "RedHeat":
		case "FireHit":
			timeScaleInit = 0.5;
			timeScale = timeScaleInit;
			sprite.SetFliters(new Array { "anim_face" }, open: true);
			return true;
		default:
			return false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryConsumeIncomingBuff, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "buffConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetBuffAdd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTemperatureResponse, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.TryConsumeIncomingBuff && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryConsumeIncomingBuff(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetBuffAdd && args.Count == 1)
		{
			GetBuffAdd(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTemperatureResponse && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplyTemperatureResponse(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.TryConsumeIncomingBuff)
		{
			return true;
		}
		if (method == MethodName.GetBuffAdd)
		{
			return true;
		}
		if (method == MethodName.ApplyTemperatureResponse)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
