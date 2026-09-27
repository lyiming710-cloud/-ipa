using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Other/Raker/Scene/TowerDefenseZombieRakerWallnut.cs")]
public class TowerDefenseZombieRakerWallnut : TowerDefenseZombieRaker
{
	public new class MethodName : TowerDefenseZombieRaker.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public static readonly StringName ApplyWallnutHeadState = "ApplyWallnutHeadState";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";
	}

	public new class PropertyName : TowerDefenseZombieRaker.PropertyName
	{
		public static readonly StringName _zombieSprite = "_zombieSprite";

		public static readonly StringName _wallnutHeadDamageStage = "_wallnutHeadDamageStage";

		public static readonly StringName _appliedWallnutHeadDamageStage = "_appliedWallnutHeadDamageStage";

		public static readonly StringName _headLost = "_headLost";
	}

	public new class SignalName : TowerDefenseZombieRaker.SignalName
	{
	}

	private const string WALLNUT_BODY_MEDIA = "Wallnut_body.png";

	private const string WALLNUT_CRACKED_1 = "uid://dpnwmtm6ypomi";

	private const string WALLNUT_CRACKED_2 = "uid://dq2ayfqxj5mfx";

	private ZombieRakerWallnutSprite _zombieSprite;

	private int _wallnutHeadDamageStage;

	private int _appliedWallnutHeadDamageStage = -1;

	private bool _headLost;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_zombieSprite = sprite as ZombieRakerWallnutSprite;
			ApplyWallnutHeadState();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (_appliedWallnutHeadDamageStage != _wallnutHeadDamageStage)
		{
			ApplyWallnutHeadState();
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		switch (damagePointName)
		{
		case "Damage1":
			_wallnutHeadDamageStage = 1;
			ApplyWallnutHeadState();
			break;
		case "Damage2":
			_wallnutHeadDamageStage = 2;
			ApplyWallnutHeadState();
			break;
		case "Head":
			if (!_headLost && GodotObject.IsInstanceValid(_zombieSprite?.head) && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
			{
				DamagePartCreate("Head", _zombieSprite.head, new Vector2(GD.RandRange(-100, 100), -300f), keepSlotScale: false, new Vector2(-25f, -30f), fromSync: false, null, 0L);
			}
			_headLost = true;
			ApplyWallnutHeadState();
			break;
		}
	}

	private void ApplyWallnutHeadState()
	{
		if (GodotObject.IsInstanceValid(_zombieSprite) && GodotObject.IsInstanceValid(_zombieSprite.head) && _zombieSprite.head.GetParent() == _zombieSprite)
		{
			_zombieSprite.head.Visible = !_headLost;
			string textureReference = _wallnutHeadDamageStage switch
			{
				1 => "uid://dpnwmtm6ypomi", 
				2 => "uid://dq2ayfqxj5mfx", 
				_ => string.Empty, 
			};
			if (_zombieSprite.head.SetAtlasReplace("Wallnut_body.png", textureReference))
			{
				_appliedWallnutHeadDamageStage = _wallnutHeadDamageStage;
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["wallnutHeadDamageStage"] = _wallnutHeadDamageStage;
		dictionary["wallnutHeadLost"] = _headLost;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_wallnutHeadDamageStage = Mathf.Clamp(data.GetValueOrDefault("wallnutHeadDamageStage", 0).AsInt32(), 0, 2);
		_headLost = data.GetValueOrDefault("wallnutHeadLost", false).AsBool();
		ApplyWallnutHeadState();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Dictionary dictionary = base.ExportNetworkSpecialState();
		dictionary["wallnutHeadDamageStage"] = _wallnutHeadDamageStage;
		dictionary["wallnutHeadLost"] = _headLost;
		return dictionary;
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		base.ImportNetworkSpecialState(data);
		_wallnutHeadDamageStage = Mathf.Clamp(data.GetValueOrDefault("wallnutHeadDamageStage", _wallnutHeadDamageStage).AsInt32(), 0, 2);
		_headLost = data.GetValueOrDefault("wallnutHeadLost", _headLost).AsBool();
		ApplyWallnutHeadState();
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return base.GetNetworkSpecialStateRevision() | (_wallnutHeadDamageStage << 5) | (_headLost ? 128 : 0);
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
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyWallnutHeadState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyWallnutHeadState && args.Count == 0)
		{
			ApplyWallnutHeadState();
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
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
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
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.ApplyWallnutHeadState)
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
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._zombieSprite)
		{
			_zombieSprite = VariantUtils.ConvertTo<ZombieRakerWallnutSprite>(in value);
			return true;
		}
		if (name == PropertyName._wallnutHeadDamageStage)
		{
			_wallnutHeadDamageStage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._appliedWallnutHeadDamageStage)
		{
			_appliedWallnutHeadDamageStage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._headLost)
		{
			_headLost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._zombieSprite)
		{
			value = VariantUtils.CreateFrom(in _zombieSprite);
			return true;
		}
		if (name == PropertyName._wallnutHeadDamageStage)
		{
			value = VariantUtils.CreateFrom(in _wallnutHeadDamageStage);
			return true;
		}
		if (name == PropertyName._appliedWallnutHeadDamageStage)
		{
			value = VariantUtils.CreateFrom(in _appliedWallnutHeadDamageStage);
			return true;
		}
		if (name == PropertyName._headLost)
		{
			value = VariantUtils.CreateFrom(in _headLost);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._zombieSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._wallnutHeadDamageStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._appliedWallnutHeadDamageStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._headLost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._zombieSprite, Variant.From(in _zombieSprite));
		info.AddProperty(PropertyName._wallnutHeadDamageStage, Variant.From(in _wallnutHeadDamageStage));
		info.AddProperty(PropertyName._appliedWallnutHeadDamageStage, Variant.From(in _appliedWallnutHeadDamageStage));
		info.AddProperty(PropertyName._headLost, Variant.From(in _headLost));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._zombieSprite, out var value))
		{
			_zombieSprite = value.As<ZombieRakerWallnutSprite>();
		}
		if (info.TryGetProperty(PropertyName._wallnutHeadDamageStage, out var value2))
		{
			_wallnutHeadDamageStage = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._appliedWallnutHeadDamageStage, out var value3))
		{
			_appliedWallnutHeadDamageStage = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._headLost, out var value4))
		{
			_headLost = value4.As<bool>();
		}
	}
}
