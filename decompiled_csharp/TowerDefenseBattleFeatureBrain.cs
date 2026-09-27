using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Brain/TowerDefenseBattleFeatureBrain.cs")]
public class TowerDefenseBattleFeatureBrain : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public static readonly StringName BrainInit = "BrainInit";

		public static readonly StringName CreateBrain = "CreateBrain";

		public static readonly StringName NormalizeBrainLineSize = "NormalizeBrainLineSize";

		public static readonly StringName RebindBrainDestroySignals = "RebindBrainDestroySignals";

		public static readonly StringName BindBrainDestroySignal = "BindBrainDestroySignal";

		public static readonly StringName GetAliveBrainCount = "GetAliveBrainCount";

		public static readonly StringName BrainDestroy = "BrainDestroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName brainLine = "brainLine";

		public static readonly StringName config = "config";

		public static readonly StringName _brainPacket = "_brainPacket";

		public static readonly StringName _progressFeature = "_progressFeature";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	public const int GridMaxSize = 51;

	public Array<TowerDefenseItem> brainLine = new Array<TowerDefenseItem>();

	public TowerDefenseBattleFeatureBrainConfig config;

	private TowerDefensePacketConfig _brainPacket;

	private TowerDefenseBattleFeatureProgress _progressFeature;

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		config = new TowerDefenseBattleFeatureBrainConfig();
		config.Init(data);
		brainLine.Clear();
		brainLine.Resize(51);
	}

	public override void OnReady()
	{
		_brainPacket = TowerDefenseManager.GetPacketConfig(config.packetName);
		_progressFeature = GetFeature("Progress") as TowerDefenseBattleFeatureProgress;
	}

	public override Task GameReady()
	{
		return Task.CompletedTask;
	}

	public override void Destroy()
	{
		for (int i = 0; i < brainLine.Count; i++)
		{
			if (brainLine[i] is TowerDefenseItemBrain towerDefenseItemBrain && GodotObject.IsInstanceValid(towerDefenseItemBrain))
			{
				towerDefenseItemBrain.OnBrainDestroy -= BrainDestroy;
			}
		}
		_brainPacket = null;
		config = null;
		_progressFeature = null;
		brainLine.Clear();
		base.Destroy();
	}

	public override Dictionary SyncSerialize()
	{
		NormalizeBrainLineSize();
		Array array = new Array();
		Array array2 = new Array();
		for (int i = 0; i < 51; i++)
		{
			if (GodotObject.IsInstanceValid(brainLine[i]) && !brainLine[i].isDestroy)
			{
				array.Add(i);
				array2.Add(new Dictionary
				{
					["line"] = i,
					["sync_id"] = brainLine[i].syncId
				});
			}
		}
		return new Dictionary
		{
			["brains"] = array2,
			["brain_lines"] = array
		};
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (!_data.ContainsKey("brains") && !_data.ContainsKey("brain_lines"))
		{
			return;
		}
		NormalizeBrainLineSize();
		System.Collections.Generic.Dictionary<int, int> dictionary = new System.Collections.Generic.Dictionary<int, int>();
		if (_data.TryGetValue("brains", out var value) && value.VariantType == Variant.Type.Array)
		{
			foreach (Variant item in value.AsGodotArray())
			{
				if (item.VariantType == Variant.Type.Dictionary)
				{
					Dictionary dictionary2 = item.AsGodotDictionary();
					int num = dictionary2.GetValueOrDefault("line", -1).AsInt32();
					if ((uint)num < 51u)
					{
						dictionary[num] = dictionary2.GetValueOrDefault("sync_id", -1).AsInt32();
					}
				}
			}
		}
		else
		{
			foreach (Variant item2 in _data["brain_lines"].AsGodotArray())
			{
				int num2 = item2.AsInt32();
				if ((uint)num2 < 51u)
				{
					dictionary[num2] = -1;
				}
			}
		}
		for (int i = 0; i < 51; i++)
		{
			if (GodotObject.IsInstanceValid(brainLine[i]) && !brainLine[i].isDestroy && !dictionary.ContainsKey(i))
			{
				DestroyComponent destroyComponent = brainLine[i].destroyComponent;
				if (destroyComponent != null && !destroyComponent.IsReleased)
				{
					brainLine[i].destroyComponent.isRemoteDestroy = true;
				}
				brainLine[i].Destroy();
				brainLine[i] = null;
			}
		}
		foreach (KeyValuePair<int, int> item3 in dictionary)
		{
			int key = item3.Key;
			int value2 = item3.Value;
			TowerDefenseItem towerDefenseItem = brainLine[key];
			if (!GodotObject.IsInstanceValid(towerDefenseItem) || towerDefenseItem.isDestroy)
			{
				towerDefenseItem = CreateBrain(key, value2);
			}
			else if (value2 >= 0 && towerDefenseItem.syncId != value2 && GodotObject.IsInstanceValid(control))
			{
				control.DetachSyncCharacter(towerDefenseItem);
				control.RegisterSyncCharacter(value2, towerDefenseItem);
			}
		}
	}

	public void BrainInit()
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (mapFeature == null)
		{
			return;
		}
		if (_brainPacket == null)
		{
			_brainPacket = TowerDefenseManager.GetPacketConfig(config.packetName);
		}
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			if (mapFeature.lineUse[i])
			{
				CreateBrain(i);
			}
		}
	}

	public TowerDefenseItem CreateBrain(int line, int syncId = -1)
	{
		if (line <= 0 || line >= 51)
		{
			return null;
		}
		NormalizeBrainLineSize();
		if (GodotObject.IsInstanceValid(brainLine[line]))
		{
			return null;
		}
		if (_brainPacket == null)
		{
			_brainPacket = TowerDefenseManager.GetPacketConfig(config.packetName);
		}
		if (!GodotObject.IsInstanceValid(_brainPacket))
		{
			return null;
		}
		Vector2 pos = TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, line)) + new Vector2((float)config.horizontalOffset, 0f);
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(1, line));
		if (!GodotObject.IsInstanceValid(characterNode) || !GodotObject.IsInstanceValid(mapCell))
		{
			return null;
		}
		TowerDefenseItemBrain towerDefenseItemBrain = _brainPacket.Create(pos, new Vector2I(0, line)) as TowerDefenseItemBrain;
		if (!GodotObject.IsInstanceValid(towerDefenseItemBrain))
		{
			return null;
		}
		towerDefenseItemBrain.characterFilter = config.characterFilter;
		BindBrainDestroySignal(towerDefenseItemBrain);
		towerDefenseItemBrain.groundHeight = mapCell.GetGroundHeight(0.0);
		towerDefenseItemBrain.z = towerDefenseItemBrain.groundHeight;
		characterNode.AddChild(towerDefenseItemBrain, forceReadableName: false, Node.InternalMode.Disabled);
		brainLine[line] = towerDefenseItemBrain;
		if (Global.IsMultiplayerMode && GodotObject.IsInstanceValid(control))
		{
			if (syncId < 0 && MultiPlayerManager.Instance != null && MultiPlayerManager.Instance.isHost)
			{
				syncId = control.GetNextSyncId();
			}
			if (syncId >= 0)
			{
				control.RegisterSyncCharacter(syncId, towerDefenseItemBrain);
			}
		}
		return towerDefenseItemBrain;
	}

	public void NormalizeBrainLineSize()
	{
		for (int i = 51; i < brainLine.Count; i++)
		{
			if (brainLine[i] is TowerDefenseItemBrain towerDefenseItemBrain && GodotObject.IsInstanceValid(towerDefenseItemBrain))
			{
				towerDefenseItemBrain.OnBrainDestroy -= BrainDestroy;
			}
		}
		if (brainLine.Count != 51)
		{
			brainLine.Resize(51);
		}
	}

	public void RebindBrainDestroySignals()
	{
		NormalizeBrainLineSize();
		for (int i = 0; i < brainLine.Count; i++)
		{
			if (brainLine[i] is TowerDefenseItemBrain towerDefenseItemBrain && GodotObject.IsInstanceValid(towerDefenseItemBrain))
			{
				BindBrainDestroySignal(towerDefenseItemBrain);
			}
			else if (!GodotObject.IsInstanceValid(brainLine[i]))
			{
				brainLine[i] = null;
			}
		}
	}

	private void BindBrainDestroySignal(TowerDefenseItemBrain brain)
	{
		brain.OnBrainDestroy -= BrainDestroy;
		brain.OnBrainDestroy += BrainDestroy;
	}

	public int GetAliveBrainCount()
	{
		NormalizeBrainLineSize();
		int num = 0;
		for (int i = 0; i < brainLine.Count; i++)
		{
			TowerDefenseItem towerDefenseItem = brainLine[i];
			if (GodotObject.IsInstanceValid(towerDefenseItem) && !towerDefenseItem.isDestroy && !towerDefenseItem.IsDie())
			{
				num++;
			}
		}
		return num;
	}

	public bool TryGetAliveBrain(int line, out TowerDefenseItemBrain brain)
	{
		NormalizeBrainLineSize();
		brain = null;
		if (line <= 0 || line >= brainLine.Count)
		{
			return false;
		}
		if (!(brainLine[line] is TowerDefenseItemBrain towerDefenseItemBrain) || !GodotObject.IsInstanceValid(towerDefenseItemBrain) || towerDefenseItemBrain.isDestroy || towerDefenseItemBrain.IsDie())
		{
			return false;
		}
		brain = towerDefenseItemBrain;
		return true;
	}

	public bool TryGetFirstAliveBrain(out TowerDefenseItem brain)
	{
		NormalizeBrainLineSize();
		for (int i = 0; i < brainLine.Count; i++)
		{
			brain = brainLine[i];
			if (GodotObject.IsInstanceValid(brain) && !brain.isDestroy && !brain.IsDie())
			{
				return true;
			}
		}
		brain = null;
		return false;
	}

	public void BrainDestroy(TowerDefenseItemBrain brain)
	{
		if (GodotObject.IsInstanceValid(brain))
		{
			int y = brain.gridPos.Y;
			if ((uint)y < (uint)brainLine.Count && brainLine[y] == brain)
			{
				brainLine[y] = null;
			}
			brain.OnBrainDestroy -= BrainDestroy;
		}
		if (!Global.IsMultiplayerMode || (MultiPlayerManager.Instance != null && MultiPlayerManager.Instance.isHost))
		{
			if (_progressFeature == null)
			{
				_progressFeature = GetFeature("Progress") as TowerDefenseBattleFeatureProgress;
			}
			_progressFeature?.IncrementPreviewWave();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BrainInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBrain, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeBrainLineSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebindBrainDestroySignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindBrainDestroySignal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "brain", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetAliveBrainCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BrainDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "brain", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
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
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
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
		if (method == MethodName.BrainInit && args.Count == 0)
		{
			BrainInit();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBrain && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseItem>(CreateBrain(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeBrainLineSize && args.Count == 0)
		{
			NormalizeBrainLineSize();
			ret = default;
			return true;
		}
		if (method == MethodName.RebindBrainDestroySignals && args.Count == 0)
		{
			RebindBrainDestroySignals();
			ret = default;
			return true;
		}
		if (method == MethodName.BindBrainDestroySignal && args.Count == 1)
		{
			BindBrainDestroySignal(VariantUtils.ConvertTo<TowerDefenseItemBrain>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAliveBrainCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetAliveBrainCount());
			return true;
		}
		if (method == MethodName.BrainDestroy && args.Count == 1)
		{
			BrainDestroy(VariantUtils.ConvertTo<TowerDefenseItemBrain>(in args[0]));
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
		if (method == MethodName.OnReady)
		{
			return true;
		}
		if (method == MethodName.Destroy)
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
		if (method == MethodName.BrainInit)
		{
			return true;
		}
		if (method == MethodName.CreateBrain)
		{
			return true;
		}
		if (method == MethodName.NormalizeBrainLineSize)
		{
			return true;
		}
		if (method == MethodName.RebindBrainDestroySignals)
		{
			return true;
		}
		if (method == MethodName.BindBrainDestroySignal)
		{
			return true;
		}
		if (method == MethodName.GetAliveBrainCount)
		{
			return true;
		}
		if (method == MethodName.BrainDestroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.brainLine)
		{
			brainLine = VariantUtils.ConvertToArray<TowerDefenseItem>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseBattleFeatureBrainConfig>(in value);
			return true;
		}
		if (name == PropertyName._brainPacket)
		{
			_brainPacket = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName._progressFeature)
		{
			_progressFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureProgress>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.brainLine)
		{
			value = VariantUtils.CreateFromArray(brainLine);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName._brainPacket)
		{
			value = VariantUtils.CreateFrom(in _brainPacket);
			return true;
		}
		if (name == PropertyName._progressFeature)
		{
			value = VariantUtils.CreateFrom(in _progressFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.brainLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brainPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._progressFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.brainLine, Variant.CreateFrom(brainLine));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName._brainPacket, Variant.From(in _brainPacket));
		info.AddProperty(PropertyName._progressFeature, Variant.From(in _progressFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.brainLine, out var value))
		{
			brainLine = value.AsGodotArray<TowerDefenseItem>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value2))
		{
			config = value2.As<TowerDefenseBattleFeatureBrainConfig>();
		}
		if (info.TryGetProperty(PropertyName._brainPacket, out var value3))
		{
			_brainPacket = value3.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName._progressFeature, out var value4))
		{
			_progressFeature = value4.As<TowerDefenseBattleFeatureProgress>();
		}
	}
}
