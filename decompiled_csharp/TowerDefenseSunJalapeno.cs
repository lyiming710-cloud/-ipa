using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Sun/Jalapeno/TowerDefenseSunJalapeno.cs")]
public class TowerDefenseSunJalapeno : TowerDefenseSunBase
{
	public new class MethodName : TowerDefenseSunBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName GetGroupName = "GetGroupName";

		public new static readonly StringName GetPoolKey = "GetPoolKey";

		public new static readonly StringName OnCollectStart = "OnCollectStart";

		public new static readonly StringName GetCollectValue = "GetCollectValue";

		public new static readonly StringName ShouldAutoCollect = "ShouldAutoCollect";

		public new static readonly StringName OnDieDown = "OnDieDown";

		public new static readonly StringName DieDown = "DieDown";

		public new static readonly StringName OnRefresh = "OnRefresh";

		public static readonly StringName Explode = "Explode";
	}

	public new class PropertyName : TowerDefenseSunBase.PropertyName
	{
		public static readonly StringName zombieCamp = "zombieCamp";
	}

	public new class SignalName : TowerDefenseSunBase.SignalName
	{
	}

	public bool zombieCamp;

	public override void _Ready()
	{
		base._Ready();
		dieDownTimer.Timeout += DieDown;
	}

	public override string GetGroupName()
	{
		return "JalapenoSun";
	}

	public override ObjectManagerConfig.OBJECT GetPoolKey()
	{
		return ObjectManagerConfig.OBJECT.SUN_JALAPENO;
	}

	public override void OnCollectStart()
	{
		if (gridPos.X < 0 || gridPos.Y < 0)
		{
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(sprite.GlobalPosition);
		}
		Explode();
	}

	public override long GetCollectValue()
	{
		return sunNum;
	}

	public override bool ShouldAutoCollect()
	{
		autoCollect = false;
		return base.ShouldAutoCollect();
	}

	public override bool OnDieDown()
	{
		return false;
	}

	public override void DieDown()
	{
		if (!isCollect)
		{
			if (GameSaveManager.Instance.GetFeatureValue("SunCollect") != 0)
			{
				Collection();
			}
			else
			{
				base.DieDown();
			}
		}
	}

	public override void OnRefresh()
	{
		gridPos = new Vector2I(-1, -1);
		zombieCamp = false;
	}

	public void Explode()
	{
		TowerDefenseCharacter.CreateJalapenoFire(zombieCamp ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT, gridPos, (double)sunNum * 10.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGroupName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPoolKey, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCollectStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCollectValue, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldAutoCollect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDieDown, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieDown, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.GetGroupName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetGroupName());
			return true;
		}
		if (method == MethodName.GetPoolKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(GetPoolKey());
			return true;
		}
		if (method == MethodName.OnCollectStart && args.Count == 0)
		{
			OnCollectStart();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCollectValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetCollectValue());
			return true;
		}
		if (method == MethodName.ShouldAutoCollect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAutoCollect());
			return true;
		}
		if (method == MethodName.OnDieDown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OnDieDown());
			return true;
		}
		if (method == MethodName.DieDown && args.Count == 0)
		{
			DieDown();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRefresh && args.Count == 0)
		{
			OnRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.Explode && args.Count == 0)
		{
			Explode();
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
		if (method == MethodName.GetGroupName)
		{
			return true;
		}
		if (method == MethodName.GetPoolKey)
		{
			return true;
		}
		if (method == MethodName.OnCollectStart)
		{
			return true;
		}
		if (method == MethodName.GetCollectValue)
		{
			return true;
		}
		if (method == MethodName.ShouldAutoCollect)
		{
			return true;
		}
		if (method == MethodName.OnDieDown)
		{
			return true;
		}
		if (method == MethodName.DieDown)
		{
			return true;
		}
		if (method == MethodName.OnRefresh)
		{
			return true;
		}
		if (method == MethodName.Explode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.zombieCamp)
		{
			zombieCamp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.zombieCamp)
		{
			value = VariantUtils.CreateFrom(in zombieCamp);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.zombieCamp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.zombieCamp, Variant.From(in zombieCamp));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.zombieCamp, out var value))
		{
			zombieCamp = value.As<bool>();
		}
	}
}
