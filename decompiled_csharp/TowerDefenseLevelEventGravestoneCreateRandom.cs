using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Event/Resource/Gravestone/TowerDefenseLevelEventGravestoneCreateRandom.cs")]
public class TowerDefenseLevelEventGravestoneCreateRandom : TowerDefenseLevelEventBase
{
	public new class MethodName : TowerDefenseLevelEventBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName Execute = "Execute";

		public static readonly StringName ExecuteInternal = "ExecuteInternal";

		public static readonly StringName CanCreateGravestoneAtCell = "CanCreateGravestoneAtCell";

		public static readonly StringName HasPendingPreSpawnGravestoneReservation = "HasPendingPreSpawnGravestoneReservation";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public new static readonly StringName GetProperty = "GetProperty";
	}

	public new class PropertyName : TowerDefenseLevelEventBase.PropertyName
	{
		public static readonly StringName gravestoneNames = "gravestoneNames";

		public static readonly StringName gravestoneNum = "gravestoneNum";

		public static readonly StringName gravestonePos = "gravestonePos";
	}

	public new class SignalName : TowerDefenseLevelEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array gravestoneNames = new Array { "GraveStoneDefault" };

	[Export(PropertyHint.None, "")]
	public int gravestoneNum = 5;

	[Export(PropertyHint.None, "")]
	public Vector4I gravestonePos = new Vector4I(3, 1, 9, 5);

	public override string _GetName()
	{
		return "LEVLE_EVENT_GRAVESTONE_CREATE_RANDOM";
	}

	public override void Execute()
	{
		if (!(gravestonePos == new Vector4I(-1, -1, -1, -1)) && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance.GetCurrentMap()))
			{
				TowerDefenseManager.GetMapFeature()?.TryEnqueuePendingMapAction(ExecuteInternal);
			}
			else
			{
				ExecuteInternal();
			}
		}
	}

	private void ExecuteInternal()
	{
		Array<Vector2I> array = new Array<Vector2I>();
		for (int i = gravestonePos.X; i <= gravestonePos.Z; i++)
		{
			for (int j = gravestonePos.Y; j <= gravestonePos.W; j++)
			{
				Vector2I vector2I = new Vector2I(i, j);
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I);
				bool flag = true;
				if (GodotObject.IsInstanceValid(mapCell.GetSurround()))
				{
					flag = false;
				}
				else
				{
					foreach (Variant gravestoneName in gravestoneNames)
					{
						TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig((string)gravestoneName);
						if (!CanCreateGravestoneAtCell(mapCell, packetConfig))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					array.Add(vector2I);
				}
			}
		}
		for (int num = Mathf.Min(array.Count, gravestoneNum); num > 0; num--)
		{
			TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig((string)gravestoneNames[GD.RandRange(0, gravestoneNames.Count - 1)]);
			Vector2I vector2I2 = array[GD.RandRange(0, array.Count - 1)];
			TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(vector2I2);
			if (CanCreateGravestoneAtCell(mapCell2, packetConfig2))
			{
				TowerDefenseCharacter towerDefenseCharacter = packetConfig2.Plant(vector2I2, playAudio: false);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					mapCell2.ReserveRandomGravestonePlacementUntilDeferredBind();
				}
				if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
				{
					TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
					if (GodotObject.IsInstanceValid(currentControl) && GodotObject.IsInstanceValid(towerDefenseCharacter))
					{
						int nextSyncId = currentControl.GetNextSyncId();
						currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
						MultiPlayerManager.Instance.SendSpawnCharacterAt(packetConfig2.saveKey, vector2I2.X, vector2I2.Y, nextSyncId);
					}
				}
			}
			array.Remove(vector2I2);
		}
	}

	private static bool CanCreateGravestoneAtCell(TowerDefenseCellInstance cell, TowerDefensePacketConfig packet)
	{
		if (!GodotObject.IsInstanceValid(cell) || !GodotObject.IsInstanceValid(packet))
		{
			return false;
		}
		if (!cell.CanPacketPlant(packet))
		{
			return false;
		}
		if (cell.HasPendingRandomGravestonePlacement())
		{
			return false;
		}
		if (HasPendingPreSpawnGravestoneReservation(cell.gridPos))
		{
			return false;
		}
		if (packet.characterConfig.name != "GraveStoneTargetQX")
		{
			return true;
		}
		bool flag = false;
		bool flag2 = false;
		foreach (TowerDefenseCharacter item in cell.GetCharacterListSave())
		{
			if (GodotObject.IsInstanceValid(item) && item is TowerDefensePlant)
			{
				flag = true;
				if ((item.config.physiqueTypeFlags & 2) != 0)
				{
					flag2 = true;
				}
			}
		}
		return !flag | flag2;
	}

	private static bool HasPendingPreSpawnGravestoneReservation(Vector2I gridPos)
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (!GodotObject.IsInstanceValid(currentControl))
		{
			return false;
		}
		TowerDefenseBattleFeaturePreSpawn feature = currentControl.GetFeature<TowerDefenseBattleFeaturePreSpawn>("PreSpawn");
		if (GodotObject.IsInstanceValid(feature))
		{
			return feature.HasPendingGravestoneReservation(gridPos);
		}
		return false;
	}

	public override void Init(Dictionary valueDictionary)
	{
		gravestoneNames = valueDictionary.GetValueOrDefault("GravestoneNames", new Array()).AsGodotArray();
		gravestoneNum = valueDictionary.GetValueOrDefault("GravestoneNum", 1).AsInt32();
		Dictionary dictionary = valueDictionary.GetValueOrDefault("GravestonePos", new Dictionary()).AsGodotDictionary();
		gravestonePos = new Vector4I(dictionary.GetValueOrDefault("x", -1).AsInt32(), dictionary.GetValueOrDefault("y", -1).AsInt32(), dictionary.GetValueOrDefault("z", -1).AsInt32(), dictionary.GetValueOrDefault("w", -1).AsInt32());
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "GravestoneCreateRandom",
			["Value"] = new Dictionary
			{
				["GravestoneNames"] = gravestoneNames,
				["GravestoneNum"] = gravestoneNum,
				["GravestonePos"] = new Dictionary
				{
					["x"] = gravestonePos.X,
					["y"] = gravestonePos.Y,
					["z"] = gravestonePos.Z,
					["w"] = gravestonePos.W
				}
			}
		};
	}

	public override Dictionary GetProperty()
	{
		Dictionary property = base.GetProperty();
		property["随机创建墓碑"] = new Dictionary
		{
			["数量"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Int",
				["Property"] = "gravestoneNum",
				["Rest"] = 5
			},
			["范围"] = new Dictionary
			{
				["Object"] = this,
				["Type"] = "Vector4I",
				["Property"] = "gravestonePos",
				["Rest"] = new Vector4I(3, 1, 9, 5)
			}
		};
		return property;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExecuteInternal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCreateGravestoneAtCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasPendingPreSpawnGravestoneReservation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProperty, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName.Execute && args.Count == 0)
		{
			Execute();
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteInternal && args.Count == 0)
		{
			ExecuteInternal();
			ret = default;
			return true;
		}
		if (method == MethodName.CanCreateGravestoneAtCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateGravestoneAtCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.HasPendingPreSpawnGravestoneReservation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPendingPreSpawnGravestoneReservation(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.GetProperty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetProperty());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CanCreateGravestoneAtCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateGravestoneAtCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.HasPendingPreSpawnGravestoneReservation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPendingPreSpawnGravestoneReservation(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.ExecuteInternal)
		{
			return true;
		}
		if (method == MethodName.CanCreateGravestoneAtCell)
		{
			return true;
		}
		if (method == MethodName.HasPendingPreSpawnGravestoneReservation)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.GetProperty)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.gravestoneNames)
		{
			gravestoneNames = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.gravestoneNum)
		{
			gravestoneNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.gravestonePos)
		{
			gravestonePos = VariantUtils.ConvertTo<Vector4I>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.gravestoneNames)
		{
			value = VariantUtils.CreateFrom(in gravestoneNames);
			return true;
		}
		if (name == PropertyName.gravestoneNum)
		{
			value = VariantUtils.CreateFrom(in gravestoneNum);
			return true;
		}
		if (name == PropertyName.gravestonePos)
		{
			value = VariantUtils.CreateFrom(in gravestonePos);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.gravestoneNames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.gravestoneNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector4I, PropertyName.gravestonePos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.gravestoneNames, Variant.From(in gravestoneNames));
		info.AddProperty(PropertyName.gravestoneNum, Variant.From(in gravestoneNum));
		info.AddProperty(PropertyName.gravestonePos, Variant.From(in gravestonePos));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.gravestoneNames, out var value))
		{
			gravestoneNames = value.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.gravestoneNum, out var value2))
		{
			gravestoneNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.gravestonePos, out var value3))
		{
			gravestonePos = value3.As<Vector4I>();
		}
	}
}
