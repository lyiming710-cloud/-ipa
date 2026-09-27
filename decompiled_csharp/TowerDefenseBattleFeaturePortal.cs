using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Portal/TowerDefenseBattleFeaturePortal.cs")]
public class TowerDefenseBattleFeaturePortal : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public static readonly StringName PortalCreate = "PortalCreate";

		public static readonly StringName ProtalCreate = "ProtalCreate";

		public static readonly StringName PortalChangePos = "PortalChangePos";

		public static readonly StringName ProtalChangePos = "ProtalChangePos";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";

		public new static readonly StringName SaveFeature = "SaveFeature";

		public new static readonly StringName LoadFeature = "LoadFeature";

		public static readonly StringName ApplyPortalState = "ApplyPortalState";

		public static readonly StringName ClearPortals = "ClearPortals";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName portalList = "portalList";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _towerDefensePortal;

	public Array<TowerDefensePortal> portalList = new Array<TowerDefensePortal>();

	private static PackedScene TOWER_DEFENSE_PORTAL => _towerDefensePortal ?? (_towerDefensePortal = GD.Load<PackedScene>("uid://dral054wqsme5"));

	public TowerDefensePortal PortalCreate(string shape, Vector4I posRange, double changeTime = 0.0)
	{
		bool flag = !IsLifetimeActive;
		if (!flag)
		{
			bool flag2;
			switch (shape)
			{
			case "Circle":
			case "Square":
			case "Rhombus":
				flag2 = true;
				break;
			default:
				flag2 = false;
				break;
			}
			flag = !flag2;
		}
		if (flag)
		{
			GD.PushWarning("[Portal] Unsupported portal shape '" + shape + "'.");
			return null;
		}
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return null;
		}
		TowerDefensePortal towerDefensePortal = TOWER_DEFENSE_PORTAL.Instantiate<TowerDefensePortal>(PackedScene.GenEditState.Disabled);
		characterNode.AddChild(towerDefensePortal, forceReadableName: false, Node.InternalMode.Disabled);
		towerDefensePortal.Init(shape, posRange, changeTime);
		portalList.Add(towerDefensePortal);
		return towerDefensePortal;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ProtalCreate(string shape, Vector4I posRange, double changeTime = 0.0)
	{
		PortalCreate(shape, posRange, changeTime);
	}

	public void PortalChangePos()
	{
		foreach (TowerDefensePortal portal in portalList)
		{
			if (GodotObject.IsInstanceValid(portal))
			{
				portal.ChangePos();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ProtalChangePos()
	{
		PortalChangePos();
	}

	public override void GameFail()
	{
		ClearPortals();
	}

	public override Dictionary SyncSerialize()
	{
		Array array = new Array();
		foreach (TowerDefensePortal portal in portalList)
		{
			if (GodotObject.IsInstanceValid(portal))
			{
				array.Add(new Dictionary
				{
					["shape"] = portal.shape,
					["pos_range_x"] = portal.posRange.X,
					["pos_range_y"] = portal.posRange.Y,
					["pos_range_z"] = portal.posRange.Z,
					["pos_range_w"] = portal.posRange.W,
					["change_time"] = portal.changeTime,
					["change_timer"] = portal.changeTimer,
					["grid_pos1_x"] = portal.gridPos1.X,
					["grid_pos1_y"] = portal.gridPos1.Y,
					["grid_pos2_x"] = portal.gridPos2.X,
					["grid_pos2_y"] = portal.gridPos2.Y
				});
			}
		}
		return new Dictionary { ["portals"] = array };
	}

	public override void SyncDeserialize(Dictionary _data)
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			return;
		}
		Array array = _data.GetValueOrDefault("portals", new Array()).AsGodotArray();
		Dictionary dictionary = new Dictionary();
		foreach (TowerDefensePortal portal in portalList)
		{
			if (GodotObject.IsInstanceValid(portal))
			{
				dictionary[portal.GetInstanceId()] = true;
			}
		}
		foreach (Variant item in array)
		{
			if (item.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary2 = item.AsGodotDictionary();
			string text = dictionary2.GetValueOrDefault("shape", "").AsString();
			int x = dictionary2.GetValueOrDefault("pos_range_x", 0).AsInt32();
			int y = dictionary2.GetValueOrDefault("pos_range_y", 0).AsInt32();
			int z = dictionary2.GetValueOrDefault("pos_range_z", 0).AsInt32();
			int w = dictionary2.GetValueOrDefault("pos_range_w", 0).AsInt32();
			double changeTime = dictionary2.GetValueOrDefault("change_time", 0.0).AsDouble();
			double changeTimer = dictionary2.GetValueOrDefault("change_timer", 0.0).AsDouble();
			int x2 = dictionary2.GetValueOrDefault("grid_pos1_x", 0).AsInt32();
			int y2 = dictionary2.GetValueOrDefault("grid_pos1_y", 0).AsInt32();
			int x3 = dictionary2.GetValueOrDefault("grid_pos2_x", 0).AsInt32();
			int y3 = dictionary2.GetValueOrDefault("grid_pos2_y", 0).AsInt32();
			bool flag = false;
			foreach (TowerDefensePortal portal2 in portalList)
			{
				if (GodotObject.IsInstanceValid(portal2) && portal2.shape == text && dictionary.ContainsKey(portal2.GetInstanceId()))
				{
					ApplyPortalState(portal2, new Vector4I(x, y, z, w), changeTime, changeTimer, new Vector2I(x2, y2), new Vector2I(x3, y3));
					dictionary.Remove(portal2.GetInstanceId());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Vector4I posRange = new Vector4I(x, y, z, w);
				TowerDefensePortal towerDefensePortal = PortalCreate(text, posRange, changeTime);
				if (GodotObject.IsInstanceValid(towerDefensePortal))
				{
					ApplyPortalState(towerDefensePortal, posRange, changeTime, changeTimer, new Vector2I(x2, y2), new Vector2I(x3, y3));
				}
			}
		}
		for (int num = portalList.Count - 1; num >= 0; num--)
		{
			TowerDefensePortal towerDefensePortal2 = portalList[num];
			if (!GodotObject.IsInstanceValid(towerDefensePortal2) || dictionary.ContainsKey(towerDefensePortal2.GetInstanceId()))
			{
				if (GodotObject.IsInstanceValid(towerDefensePortal2))
				{
					towerDefensePortal2.QueueFree();
				}
				portalList.RemoveAt(num);
			}
		}
	}

	public override Dictionary SaveFeature()
	{
		Array array = new Array();
		foreach (TowerDefensePortal portal in portalList)
		{
			if (GodotObject.IsInstanceValid(portal))
			{
				array.Add(new Dictionary
				{
					["shape"] = portal.shape,
					["pos_range_x"] = portal.posRange.X,
					["pos_range_y"] = portal.posRange.Y,
					["pos_range_z"] = portal.posRange.Z,
					["pos_range_w"] = portal.posRange.W,
					["change_time"] = portal.changeTime,
					["change_timer"] = portal.changeTimer,
					["grid_pos1_x"] = portal.gridPos1.X,
					["grid_pos1_y"] = portal.gridPos1.Y,
					["grid_pos2_x"] = portal.gridPos2.X,
					["grid_pos2_y"] = portal.gridPos2.Y
				});
			}
		}
		return new Dictionary { ["portals"] = array };
	}

	public override void LoadFeature(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		ClearPortals();
		foreach (Variant item in _data.GetValueOrDefault("portals", new Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			string shape = dictionary.GetValueOrDefault("shape", "").AsString();
			int x = dictionary.GetValueOrDefault("pos_range_x", 0).AsInt32();
			int y = dictionary.GetValueOrDefault("pos_range_y", 0).AsInt32();
			int z = dictionary.GetValueOrDefault("pos_range_z", 0).AsInt32();
			int w = dictionary.GetValueOrDefault("pos_range_w", 0).AsInt32();
			double changeTime = dictionary.GetValueOrDefault("change_time", 0.0).AsDouble();
			double changeTimer = dictionary.GetValueOrDefault("change_timer", 0.0).AsDouble();
			int x2 = dictionary.GetValueOrDefault("grid_pos1_x", 0).AsInt32();
			int y2 = dictionary.GetValueOrDefault("grid_pos1_y", 0).AsInt32();
			int x3 = dictionary.GetValueOrDefault("grid_pos2_x", 0).AsInt32();
			int y3 = dictionary.GetValueOrDefault("grid_pos2_y", 0).AsInt32();
			Vector4I posRange = new Vector4I(x, y, z, w);
			TowerDefensePortal towerDefensePortal = PortalCreate(shape, posRange, changeTime);
			if (GodotObject.IsInstanceValid(towerDefensePortal))
			{
				ApplyPortalState(towerDefensePortal, posRange, changeTime, changeTimer, new Vector2I(x2, y2), new Vector2I(x3, y3));
			}
		}
	}

	private static void ApplyPortalState(TowerDefensePortal portal, Vector4I posRange, double changeTime, double changeTimer, Vector2I gridPos1, Vector2I gridPos2)
	{
		portal.posRange = posRange;
		portal.changeTime = Mathf.Max(0.0, changeTime);
		portal.changeTimer = ((portal.changeTime > 0.0) ? Mathf.Clamp(changeTimer, 0.0, portal.changeTime) : 0.0);
		portal.gridPos1 = gridPos1;
		portal.gridPos2 = gridPos2;
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		portal.GetNode<Node2D>("%ProtalNode1").GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos1) + new Vector2(mapGridSize.X / 2f, 0f);
		portal.GetNode<Node2D>("%ProtalNode2").GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos2) + new Vector2(mapGridSize.X / 2f, 0f);
		portal.protalSprite1.ZIndex = gridPos1.Y * 15;
		portal.protalSprite2.ZIndex = gridPos2.Y * 15;
	}

	private void ClearPortals()
	{
		foreach (TowerDefensePortal portal in portalList)
		{
			if (GodotObject.IsInstanceValid(portal))
			{
				portal.QueueFree();
			}
		}
		portalList.Clear();
	}

	public override void Destroy()
	{
		ClearPortals();
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.PortalCreate, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "posRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "changeTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProtalCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "shape", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "posRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "changeTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PortalChangePos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProtalChangePos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFeature, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPortalState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "portal", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "posRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "changeTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "changeTimer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos1", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos2", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPortals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.PortalCreate && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePortal>(PortalCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ProtalCreate && args.Count == 3)
		{
			ProtalCreate(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PortalChangePos && args.Count == 0)
		{
			PortalChangePos();
			ret = default;
			return true;
		}
		if (method == MethodName.ProtalChangePos && args.Count == 0)
		{
			ProtalChangePos();
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
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
		if (method == MethodName.SaveFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveFeature());
			return true;
		}
		if (method == MethodName.LoadFeature && args.Count == 2)
		{
			LoadFeature(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPortalState && args.Count == 6)
		{
			ApplyPortalState(VariantUtils.ConvertTo<TowerDefensePortal>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2I>(in args[4]), VariantUtils.ConvertTo<Vector2I>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPortals && args.Count == 0)
		{
			ClearPortals();
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
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyPortalState && args.Count == 6)
		{
			ApplyPortalState(VariantUtils.ConvertTo<TowerDefensePortal>(in args[0]), VariantUtils.ConvertTo<Vector4I>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Vector2I>(in args[4]), VariantUtils.ConvertTo<Vector2I>(in args[5]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.PortalCreate)
		{
			return true;
		}
		if (method == MethodName.ProtalCreate)
		{
			return true;
		}
		if (method == MethodName.PortalChangePos)
		{
			return true;
		}
		if (method == MethodName.ProtalChangePos)
		{
			return true;
		}
		if (method == MethodName.GameFail)
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
		if (method == MethodName.SaveFeature)
		{
			return true;
		}
		if (method == MethodName.LoadFeature)
		{
			return true;
		}
		if (method == MethodName.ApplyPortalState)
		{
			return true;
		}
		if (method == MethodName.ClearPortals)
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
		if (name == PropertyName.portalList)
		{
			portalList = VariantUtils.ConvertToArray<TowerDefensePortal>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.portalList)
		{
			value = VariantUtils.CreateFromArray(portalList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.portalList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.portalList, Variant.CreateFrom(portalList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.portalList, out var value))
		{
			portalList = value.AsGodotArray<TowerDefensePortal>();
		}
	}
}
