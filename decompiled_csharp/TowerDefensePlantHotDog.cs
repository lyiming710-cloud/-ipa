using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/HotDog/Scene/TowerDefensePlantHotDog.cs")]
public class TowerDefensePlantHotDog : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string HOT_DOG_SKIN31 = "uid://b15hq5knf1462";

	private const string HOT_DOG_SKIN32 = "uid://dpb542keiv7j8";

	private const string HOT_DOG_SKIN33 = "uid://dp84ritbd4lbw";

	private const string HOT_DOG_SKIN41 = "uid://bwhn21u7l6hxa";

	private const string HOT_DOG_SKIN42 = "uid://b3c8rks3u7ldu";

	private const string HOT_DOG_SKIN43 = "uid://doxybk50iiwhe";

	private const string HOT_DOG_SKIN61 = "uid://b6lxv4s7mmh1c";

	private const string HOT_DOG_SKIN62 = "uid://bbbscktm0lt4a";

	private const string HOT_DOG_SKIN63 = "uid://dciued0lcnkxo";

	private const string HOT_DOG_SKIN72 = "uid://kn34pg26s2n";

	private const string HOT_DOG_SKIN722 = "uid://ciw3bqm4vccey";

	private const string HOT_DOG_SKIN723 = "uid://bj8vrr866kdo2";

	private const string HOT_DOG_SKIN73 = "uid://ja25qci6b505";

	private const string HOT_DOG_SKIN733 = "uid://bw0wgcemhi40e";

	private const string HOT_DOG_SKIN74 = "uid://b2iah3f5ft2t5";

	private const string HOT_DOG_SKIN742 = "uid://cii3syd5r3q5q";

	private const string HOT_DOG_SKIN743 = "uid://dlsvytiqbcbt2";

	private const string HOT_DOG_SKIN75 = "uid://cghcs27icauh4";

	private const string HOT_DOG_SKIN753 = "uid://bdwsmked5boiu";

	private const string HOT_DOG_SKIN76 = "uid://df0v50tov07ky";

	private const string HOT_DOG_SKIN762 = "uid://b3r7s1txwktrs";

	private const string HOT_DOG_SKIN763 = "uid://cd6irfup6sokx";

	private const string HOT_DOG00019 = "uid://6xhwx5v733xs";

	private const string HOT_DOG00019_CRACKED1 = "uid://kddmvlhxk80f";

	private const string HOT_DOG00019_CRACKED2 = "uid://bt2crx2aj2b2b";

	private const string HOT_DOG00028 = "uid://8dnfdnmf4xxe";

	private const string HOT_DOG00028_CRACKED1 = "uid://u14pwe08qshs";

	private const string HOT_DOG00028_CRACKED2 = "uid://vtktjc5ueuwf";

	private const string HOT_DOG00046 = "uid://bmgefsot5bcmf";

	private const string HOT_DOG00046_CRACKED1 = "uid://clxq2sslv2v7o";

	private const string HOT_DOG00046_CRACKED2 = "uid://b7ejbiskk5r6v";

	private ProduceComponent _produceComponent;

	private FireComponent _fireComponent;

	private double _produceInterval = 25.0;

	private int _sunNum = 50;

	private double _fireInterval = 2.0;

	private int _fireNum = 1;

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
			if (IsNodeReady() && _produceComponent != null)
			{
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.produceInterval = (float)value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int sunNum
	{
		get
		{
			return _sunNum;
		}
		set
		{
			_sunNum = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.num = value;
				}
			}
		}
	}

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

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_produceComponent = componentManager.GetRuntime<ProduceComponent>();
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			if (currentCustom.Contains("Custom0"))
			{
				((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[0].projectileResource).projectileData.skinName = "Pow";
				((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[1].projectileResource).projectileData.skinName = "Pow";
				((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[2].projectileResource).projectileData.skinName = "Cask";
				((FireComponentProjectileSingle)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[1].projectileResource).projectileData.skinName = "Cask";
			}
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[0].projectileResource).projectileData.skinName = "Pow";
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[1].projectileResource).projectileData.skinName = "Pow";
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[2].projectileResource).projectileData.skinName = "Cask";
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[1].projectileResource).projectileData.skinName = "Cask";
		}
		else
		{
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[0].projectileResource).projectileData.skinName = "Default";
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[1].projectileResource).projectileData.skinName = "Default";
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[0].projectileResource).projectileWeight[2].projectileResource).projectileData.skinName = "Default";
			((FireComponentProjectileSingle)((FireComponentProjectileWeight)_fireComponent.fireCheckList[0].projectile).projectileWeight[1].projectileResource).projectileData.skinName = "Default";
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		switch (damangePointName)
		{
		case "Damage0":
			sprite.SetAtlasReplace("HotDog_0001_9.png", "uid://6xhwx5v733xs");
			sprite.SetAtlasReplace("HotDog_0002_8.png", "uid://8dnfdnmf4xxe");
			sprite.SetAtlasReplace("HotDog_0004_6.png", "uid://bmgefsot5bcmf");
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("HotDog_skin3_1.png", "uid://b15hq5knf1462");
				sprite.SetAtlasReplace("HotDog_skin4_1.png", "uid://bwhn21u7l6hxa");
				sprite.SetAtlasReplace("HotDog_skin6_1.png", "uid://b6lxv4s7mmh1c");
				sprite.SetAtlasReplace("HotDog_skin7_2.png", "uid://kn34pg26s2n");
				sprite.SetAtlasReplace("HotDog_skin7_3.png", "uid://ja25qci6b505");
				sprite.SetAtlasReplace("HotDog_skin7_4.png", "uid://b2iah3f5ft2t5");
				sprite.SetAtlasReplace("HotDog_skin7_5.png", "uid://cghcs27icauh4");
				sprite.SetAtlasReplace("HotDog_skin7_6.png", "uid://df0v50tov07ky");
			}
			break;
		case "Damage1":
			sprite.SetAtlasReplace("HotDog_0001_9.png", "uid://kddmvlhxk80f");
			sprite.SetAtlasReplace("HotDog_0002_8.png", "uid://u14pwe08qshs");
			sprite.SetAtlasReplace("HotDog_0004_6.png", "uid://clxq2sslv2v7o");
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("HotDog_skin3_1.png", "uid://dpb542keiv7j8");
				sprite.SetAtlasReplace("HotDog_skin4_1.png", "uid://b3c8rks3u7ldu");
				sprite.SetAtlasReplace("HotDog_skin6_1.png", "uid://bbbscktm0lt4a");
				sprite.SetAtlasReplace("HotDog_skin7_2.png", "uid://ciw3bqm4vccey");
				sprite.SetAtlasReplace("HotDog_skin7_4.png", "uid://cii3syd5r3q5q");
				sprite.SetAtlasReplace("HotDog_skin7_6.png", "uid://b3r7s1txwktrs");
			}
			break;
		case "Damage2":
			sprite.SetAtlasReplace("HotDog_0001_9.png", "uid://bt2crx2aj2b2b");
			sprite.SetAtlasReplace("HotDog_0002_8.png", "uid://vtktjc5ueuwf");
			sprite.SetAtlasReplace("HotDog_0004_6.png", "uid://b7ejbiskk5r6v");
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("HotDog_skin3_1.png", "uid://dp84ritbd4lbw");
				sprite.SetAtlasReplace("HotDog_skin4_1.png", "uid://doxybk50iiwhe");
				sprite.SetAtlasReplace("HotDog_skin6_1.png", "uid://dciued0lcnkxo");
				sprite.SetAtlasReplace("HotDog_skin7_2.png", "uid://bj8vrr866kdo2");
				sprite.SetAtlasReplace("HotDog_skin7_3.png", "uid://bw0wgcemhi40e");
				sprite.SetAtlasReplace("HotDog_skin7_4.png", "uid://dlsvytiqbcbt2");
				sprite.SetAtlasReplace("HotDog_skin7_5.png", "uid://bdwsmked5boiu");
				sprite.SetAtlasReplace("HotDog_skin7_6.png", "uid://cd6irfup6sokx");
			}
			break;
		}
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		_produceComponent.produceType = (instance.hypnoses ? "BrainSun" : "Sun");
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["produceInterval"] = produceInterval,
			["sunNum"] = sunNum,
			["fireNum"] = fireNum,
			["fireInterval"] = fireInterval
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		produceInterval = (data.ContainsKey("produceInterval") ? data["produceInterval"].AsDouble() : 25.0);
		sunNum = (data.ContainsKey("sunNum") ? data["sunNum"].AsInt32() : 50);
		fireNum = ((!data.ContainsKey("fireNum")) ? 1 : data["fireNum"].AsInt32());
		fireInterval = (data.ContainsKey("fireInterval") ? data["fireInterval"].AsDouble() : 2.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
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
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
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
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
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
		if (name == PropertyName._produceInterval)
		{
			_produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			_sunNum = VariantUtils.ConvertTo<int>(in value);
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		double from;
		if (name == PropertyName.produceInterval)
		{
			from = produceInterval;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.sunNum)
		{
			from2 = sunNum;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			from = fireInterval;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			from2 = fireNum;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._produceInterval)
		{
			value = VariantUtils.CreateFrom(in _produceInterval);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			value = VariantUtils.CreateFrom(in _sunNum);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._produceInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
		info.AddProperty(PropertyName.sunNum, Variant.From<int>(sunNum));
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.produceInterval, out var value))
		{
			produceInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.sunNum, out var value2))
		{
			sunNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireInterval, out var value3))
		{
			fireInterval = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value4))
		{
			fireNum = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._produceInterval, out var value5))
		{
			_produceInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunNum, out var value6))
		{
			_sunNum = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value7))
		{
			_fireInterval = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value8))
		{
			_fireNum = value8.As<int>();
		}
	}
}
