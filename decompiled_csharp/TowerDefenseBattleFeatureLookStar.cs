using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/LookStar/TowerDefenseBattleFeatureLookStar.cs")]
public class TowerDefenseBattleFeatureLookStar : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Process = "Process";

		public static readonly StringName FreshGround = "FreshGround";

		public static readonly StringName IsOpen = "IsOpen";

		public static readonly StringName OnWaveReachFinal = "OnWaveReachFinal";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName InitializeChecks = "InitializeChecks";

		public static readonly StringName EnsureCheckPreview = "EnsureCheckPreview";

		public static readonly StringName SetAllPreviewsVisible = "SetAllPreviewsVisible";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName config = "config";

		public static readonly StringName characterLayer = "characterLayer";

		public static readonly StringName checkDictionary = "checkDictionary";

		public static readonly StringName isfinish = "isfinish";

		public static readonly StringName _lookStarNode = "_lookStarNode";

		public static readonly StringName _checkTimer = "_checkTimer";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public TowerDefenseLevelLookStarManagerConfig config;

	public CanvasLayer characterLayer;

	public Dictionary checkDictionary = new Dictionary();

	public bool isfinish;

	private Node2D _lookStarNode;

	private double _checkTimer;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseLevelLookStarManagerConfig();
		config.Init(data);
		characterLayer = new CanvasLayer();
		characterLayer.FollowViewportEnabled = true;
		characterLayer.Layer = config.previewLayer;
		_lookStarNode = new Node2D();
		control.AddNode(_lookStarNode, 2);
		_lookStarNode.AddChild(characterLayer, forceReadableName: false, Node.InternalMode.Disabled);
	}

	public override Task GameInit()
	{
		InitializeChecks(createPreviews: true);
		FreshGround();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		InitializeChecks(createPreviews: false);
		return Task.CompletedTask;
	}

	public override void Process(double _delta)
	{
		if (config.open && TowerDefenseManager.Instance.IsGameRunning() && !isfinish)
		{
			_checkTimer += _delta;
			if (!(_checkTimer < config.checkInterval))
			{
				_checkTimer %= config.checkInterval;
				FreshGround();
			}
		}
	}

	public void FreshGround()
	{
		if (config.checkList.Count == 0)
		{
			return;
		}
		bool flag = true;
		bool flag2 = false;
		foreach (TowerDefenseLevelLookStarCheckConfig check in config.checkList)
		{
			if (check == null)
			{
				continue;
			}
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(check.gridPos);
			if (!GodotObject.IsInstanceValid(mapCell))
			{
				continue;
			}
			flag2 = true;
			bool flag3 = mapCell.HasCharacter(check.packetName);
			if (checkDictionary.TryGetValue(check, out var value))
			{
				AdobeAnimateSprite adobeAnimateSprite = value.As<AdobeAnimateSprite>();
				if (GodotObject.IsInstanceValid(adobeAnimateSprite))
				{
					adobeAnimateSprite.Visible = !flag3;
				}
				if (!flag3)
				{
					flag = false;
				}
			}
		}
		if (!(flag2 & flag))
		{
			return;
		}
		TowerDefenseControlNew towerDefenseControlNew = control;
		if (towerDefenseControlNew != null && towerDefenseControlNew.process?.TryFinish() == true)
		{
			TowerDefenseBattleFeatureWave towerDefenseBattleFeatureWave = GetFeature("Wave") as TowerDefenseBattleFeatureWave;
			if (GodotObject.IsInstanceValid(towerDefenseBattleFeatureWave))
			{
				towerDefenseBattleFeatureWave.waveFinal = true;
				towerDefenseBattleFeatureWave.spawnOver = true;
			}
			isfinish = true;
		}
	}

	public bool IsOpen()
	{
		return config.open;
	}

	public bool OnWaveReachFinal(TowerDefenseBattleFeatureWave waveFeature)
	{
		if (!config.open || isfinish)
		{
			return false;
		}
		int num = Mathf.Max(1, waveFeature.config.flagWaveInterval);
		int currentWave = Mathf.Max(0, (waveFeature.config.wave.Count - 1) / num * num);
		waveFeature.waveFinal = false;
		waveFeature.currentWave = currentWave;
		waveFeature.spawnOver = false;
		waveFeature.nextWaveTime = waveFeature.config.spawnColStart;
		return true;
	}

	public override Dictionary SaveFeature()
	{
		return new Dictionary
		{
			["isfinish"] = isfinish,
			["checkTimer"] = _checkTimer
		};
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary { ["isfinish"] = isfinish };
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (_data.ContainsKey("isfinish"))
		{
			isfinish = _data["isfinish"].AsBool();
			if (isfinish)
			{
				SetAllPreviewsVisible(visible: false);
			}
			else
			{
				FreshGround();
			}
		}
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		isfinish = _data.GetValueOrDefault("isfinish", false).AsBool();
		_checkTimer = Math.Max(0.0, _data.GetValueOrDefault("checkTimer", 0.0).AsDouble());
		if (isfinish)
		{
			SetAllPreviewsVisible(visible: false);
			return;
		}
		InitializeChecks(createPreviews: true);
		FreshGround();
	}

	private void InitializeChecks(bool createPreviews)
	{
		foreach (TowerDefenseLevelLookStarCheckConfig check in config.checkList)
		{
			if (check != null && !string.IsNullOrWhiteSpace(check.packetName))
			{
				if (!checkDictionary.ContainsKey(check))
				{
					checkDictionary[check] = Variant.From<GodotObject>((GodotObject)null);
				}
				if (createPreviews)
				{
					EnsureCheckPreview(check);
				}
			}
		}
	}

	private void EnsureCheckPreview(TowerDefenseLevelLookStarCheckConfig check)
	{
		if (!GodotObject.IsInstanceValid(characterLayer) || (checkDictionary.TryGetValue(check, out var value) && GodotObject.IsInstanceValid(value.AsGodotObject())) || !GodotObject.IsInstanceValid(TowerDefenseManager.GetPacketConfig(check.packetName)))
		{
			return;
		}
		AdobeAnimateSprite characterSprite = TowerDefenseManager.GetCharacterSprite(check.packetName);
		if (GodotObject.IsInstanceValid(characterSprite))
		{
			characterLayer.AddChild(characterSprite, forceReadableName: false, Node.InternalMode.Disabled);
			characterSprite.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(check.gridPos);
			Color meshColor = characterSprite.meshColor;
			meshColor.A = config.previewOpacity;
			characterSprite.meshColor = meshColor;
			characterSprite.pause = true;
			checkDictionary[check] = characterSprite;
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(check.gridPos);
			if (GodotObject.IsInstanceValid(mapCell))
			{
				characterSprite.GlobalPosition = new Vector2(characterSprite.GlobalPosition.X, (float)((double)characterSprite.GlobalPosition.Y - mapCell.GetGroundHeight()));
			}
		}
	}

	private void SetAllPreviewsVisible(bool visible)
	{
		foreach (Variant value in checkDictionary.Values)
		{
			AdobeAnimateSprite adobeAnimateSprite = value.As<AdobeAnimateSprite>();
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.Visible = visible;
			}
		}
	}

	public override void Destroy()
	{
		if (GodotObject.IsInstanceValid(_lookStarNode))
		{
			_lookStarNode.QueueFree();
		}
		_lookStarNode = null;
		characterLayer = null;
		checkDictionary.Clear();
		config = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreshGround, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsOpen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnWaveReachFinal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "waveFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeChecks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "createPreviews", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCheckPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "check", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetAllPreviewsVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreshGround && args.Count == 0)
		{
			FreshGround();
			ret = default;
			return true;
		}
		if (method == MethodName.IsOpen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsOpen());
			return true;
		}
		if (method == MethodName.OnWaveReachFinal && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(OnWaveReachFinal(VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeChecks && args.Count == 1)
		{
			InitializeChecks(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCheckPreview && args.Count == 1)
		{
			EnsureCheckPreview(VariantUtils.ConvertTo<TowerDefenseLevelLookStarCheckConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAllPreviewsVisible && args.Count == 1)
		{
			SetAllPreviewsVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.FreshGround)
		{
			return true;
		}
		if (method == MethodName.IsOpen)
		{
			return true;
		}
		if (method == MethodName.OnWaveReachFinal)
		{
			return true;
		}
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.InitializeChecks)
		{
			return true;
		}
		if (method == MethodName.EnsureCheckPreview)
		{
			return true;
		}
		if (method == MethodName.SetAllPreviewsVisible)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseLevelLookStarManagerConfig>(in value);
			return true;
		}
		if (name == PropertyName.characterLayer)
		{
			characterLayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.checkDictionary)
		{
			checkDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.isfinish)
		{
			isfinish = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lookStarNode)
		{
			_lookStarNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._checkTimer)
		{
			_checkTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.characterLayer)
		{
			value = VariantUtils.CreateFrom(in characterLayer);
			return true;
		}
		if (name == PropertyName.checkDictionary)
		{
			value = VariantUtils.CreateFrom(in checkDictionary);
			return true;
		}
		if (name == PropertyName.isfinish)
		{
			value = VariantUtils.CreateFrom(in isfinish);
			return true;
		}
		if (name == PropertyName._lookStarNode)
		{
			value = VariantUtils.CreateFrom(in _lookStarNode);
			return true;
		}
		if (name == PropertyName._checkTimer)
		{
			value = VariantUtils.CreateFrom(in _checkTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.checkDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isfinish, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lookStarNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._checkTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.characterLayer, Variant.From(in characterLayer));
		info.AddProperty(PropertyName.checkDictionary, Variant.From(in checkDictionary));
		info.AddProperty(PropertyName.isfinish, Variant.From(in isfinish));
		info.AddProperty(PropertyName._lookStarNode, Variant.From(in _lookStarNode));
		info.AddProperty(PropertyName._checkTimer, Variant.From(in _checkTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.config, out var value))
		{
			config = value.As<TowerDefenseLevelLookStarManagerConfig>();
		}
		if (info.TryGetProperty(PropertyName.characterLayer, out var value2))
		{
			characterLayer = value2.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.checkDictionary, out var value3))
		{
			checkDictionary = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.isfinish, out var value4))
		{
			isfinish = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lookStarNode, out var value5))
		{
			_lookStarNode = value5.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._checkTimer, out var value6))
		{
			_checkTimer = value6.As<double>();
		}
	}
}
