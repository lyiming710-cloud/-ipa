using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Diamond/MarigoldG/Scene/TowerDefensePlantMarigoldG.cs")]
public class TowerDefensePlantMarigoldG : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public static readonly StringName ApplySkinNameToProjectile = "ApplySkinNameToProjectile";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName _fireBurstRunning = "_fireBurstRunning";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";

		public static readonly StringName _skinName = "_skinName";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private FireComponent _fireComponent;

	private bool _fireBurstRunning;

	private double _fireInterval = 6.0;

	private int _fireNum = 8;

	private string _skinName = "Default";

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

	public string skinName
	{
		get
		{
			return _skinName;
		}
		set
		{
			_skinName = value;
			if (!IsNodeReady() || _fireComponent == null)
			{
				return;
			}
			FireComponent fireComponent = _fireComponent;
			if (fireComponent == null || fireComponent.IsReleased || _fireComponent.fireCheckList.Count <= 0)
			{
				return;
			}
			foreach (FireComponentCheckConfig fireCheck in _fireComponent.fireCheckList)
			{
				if (fireCheck != null)
				{
					ApplySkinNameToProjectile(fireCheck.projectile, value);
				}
			}
		}
	}

	private static void ApplySkinNameToProjectile(FireComponentProjectileResource projectile, string value)
	{
		if (projectile == null)
		{
			return;
		}
		if (projectile is FireComponentProjectileSingle fireComponentProjectileSingle)
		{
			if (fireComponentProjectileSingle.projectileData != null)
			{
				fireComponentProjectileSingle.projectileData.skinName = value;
			}
		}
		else
		{
			if (!(projectile is FireComponentProjectileWeight fireComponentProjectileWeight))
			{
				return;
			}
			if (fireComponentProjectileWeight.projectileData != null)
			{
				fireComponentProjectileWeight.projectileData.skinName = value;
			}
			foreach (FireComponentProjectileWeightItem item in fireComponentProjectileWeight.projectileWeight)
			{
				if (item != null)
				{
					ApplySkinNameToProjectile(item.projectileResource, value);
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			if (currentCustom.Contains("Custom0"))
			{
				skinName = "Sycee";
			}
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
		sprite.timeScale = timeScale;
		if (!_fireBurstRunning && _fireComponent.CanFireByData(_fireComponent.fireCheckList[0].projectile.GetProjetile()))
		{
			_fireComponent.Refresh();
			TowerDefenseProjectileCreateData projetile = _fireComponent.fireCheckList[0].projectile.GetProjetile();
			_fireBurstRunning = true;
			FireBurstAsync(CurrentStateHandle, projetile);
		}
	}

	private async Task FireBurstAsync(StateHandle stateHandle, TowerDefenseProjectileCreateData projectileData)
	{
		try
		{
			for (int i = 0; i < fireNum; i++)
			{
				if (_fireComponent?.IsReleased ?? true)
				{
					break;
				}
				if (stateHandle == null || !stateHandle.IsActive)
				{
					break;
				}
				Vector2 vector = Vector2.FromAngle(Mathf.DegToRad(360f / (float)fireNum * (float)i));
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					spawnTweenOffset = vector * 50f,
					spawnTweenDuration = 0.5f,
					spawnTweenEase = Tween.EaseType.Out,
					spawnTweenTrans = Tween.TransitionType.Quart
				};
				_fireComponent.CreateProjectileByData(0, new Vector2(600f, 0f), projectileData, -1, camp, Vector2.Zero, overrides);
				bool flag = i + 1 < fireNum;
				if (flag)
				{
					flag = !(await WaitForStateDelayAsync(stateHandle, 0.1));
				}
				if (flag)
				{
					break;
				}
			}
		}
		finally
		{
			_fireBurstRunning = false;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["fireNum"] = fireNum,
			["skinName"] = skinName,
			["fireInterval"] = fireInterval
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		fireNum = (data.ContainsKey("fireNum") ? data["fireNum"].AsInt32() : 8);
		skinName = (data.ContainsKey("skinName") ? data["skinName"].AsString() : "Default");
		fireInterval = (data.ContainsKey("fireInterval") ? data["fireInterval"].AsDouble() : 6.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.ApplySkinNameToProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ApplySkinNameToProjectile && args.Count == 2)
		{
			ApplySkinNameToProjectile(VariantUtils.ConvertTo<FireComponentProjectileResource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplySkinNameToProjectile && args.Count == 2)
		{
			ApplySkinNameToProjectile(VariantUtils.ConvertTo<FireComponentProjectileResource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ApplySkinNameToProjectile)
		{
			return true;
		}
		if (method == MethodName._Ready)
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
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._fireBurstRunning)
		{
			_fireBurstRunning = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._skinName)
		{
			_skinName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.skinName)
		{
			value = VariantUtils.CreateFrom<string>(skinName);
			return true;
		}
		if (name == PropertyName._fireBurstRunning)
		{
			value = VariantUtils.CreateFrom(in _fireBurstRunning);
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
		if (name == PropertyName._skinName)
		{
			value = VariantUtils.CreateFrom(in _skinName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._fireBurstRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName.skinName, Variant.From<string>(skinName));
		info.AddProperty(PropertyName._fireBurstRunning, Variant.From(in _fireBurstRunning));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
		info.AddProperty(PropertyName._skinName, Variant.From(in _skinName));
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
		if (info.TryGetProperty(PropertyName.skinName, out var value3))
		{
			skinName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._fireBurstRunning, out var value4))
		{
			_fireBurstRunning = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value5))
		{
			_fireInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value6))
		{
			_fireNum = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._skinName, out var value7))
		{
			_skinName = value7.As<string>();
		}
	}
}
