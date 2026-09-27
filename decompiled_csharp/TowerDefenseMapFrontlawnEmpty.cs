using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Asset/Config/Map/Frontlawn/Scene/Empty/TowerDefenseMapFrontlawnEmpty.cs")]
public class TowerDefenseMapFrontlawnEmpty : TowerDefenseMapRevealOnEnter
{
	public new class MethodName : TowerDefenseMapRevealOnEnter.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName SaveMapBase = "SaveMapBase";

		public new static readonly StringName LoadMapBase = "LoadMapBase";

		public static readonly StringName LoadLegacyRevealVisibility = "LoadLegacyRevealVisibility";

		public static readonly StringName Row1Create = "Row1Create";

		public static readonly StringName Row1Finish = "Row1Finish";

		public static readonly StringName Row3CreateFromRow1 = "Row3CreateFromRow1";

		public static readonly StringName Row3Finish = "Row3Finish";

		public static readonly StringName Row5CreateFromRow3 = "Row5CreateFromRow3";

		public static readonly StringName Row5Finish = "Row5Finish";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName Delete = "Delete";

		public static readonly StringName CreateSodRoll = "CreateSodRoll";

		public static readonly StringName ShowRow1 = "ShowRow1";

		public static readonly StringName ShowRow3 = "ShowRow3";

		public static readonly StringName ShowRow5 = "ShowRow5";
	}

	public new class PropertyName : TowerDefenseMapRevealOnEnter.PropertyName
	{
		public static readonly StringName _frontlawnRow = "_frontlawnRow";

		public static readonly StringName _rowBeginForm = "_rowBeginForm";

		public static readonly StringName _rowEndForm = "_rowEndForm";

		public static readonly StringName alive = "alive";

		public static readonly StringName createRowId = "createRowId";

		public static readonly StringName followSodRoll = "followSodRoll";
	}

	public new class SignalName : TowerDefenseMapRevealOnEnter.SignalName
	{
	}

	private const string SodRollPath = "res://Asset/Anime/Effect/SodRoll/SodRoll.tscn";

	private const double SodRollRevealDistance = 790.0;

	private Sprite2D[] _frontlawnRow;

	private double[] _rowBeginForm;

	private double[] _rowEndForm;

	public bool alive;

	public int createRowId = -1;

	public AdobeAnimateSprite followSodRoll;

	protected override void RegisterEventFunctions(IDictionary<string, int> functions)
	{
		base.RegisterEventFunctions(functions);
		TowerDefenseMap.RegisterEventFunction(functions, "Row1Create", 0);
		TowerDefenseMap.RegisterEventFunction(functions, "Row3CreateFromRow1", 0);
		TowerDefenseMap.RegisterEventFunction(functions, "Row5CreateFromRow3", 0);
		TowerDefenseMap.RegisterEventFunction(functions, "ShowRow1", 0);
		TowerDefenseMap.RegisterEventFunction(functions, "ShowRow3", 0);
		TowerDefenseMap.RegisterEventFunction(functions, "ShowRow5", 0);
	}

	public override void _Ready()
	{
		base._Ready();
		_frontlawnRow = new Sprite2D[3]
		{
			GetNode<Sprite2D>("%FrontlawnSod1Row"),
			GetNode<Sprite2D>("%FrontlawnSod3Row"),
			GetNode<Sprite2D>("%FrontlawnSod5Row")
		};
		_rowBeginForm = new double[3] { 0.0, 0.0, 10.0 };
		_rowEndForm = new double[3] { 0.0, 2.0, 0.0 };
		for (int i = 0; i < _frontlawnRow.Length; i++)
		{
			_frontlawnRow[i].Visible = false;
			Rect2 regionRect = _frontlawnRow[i].RegionRect;
			_frontlawnRow[i].RegionRect = new Rect2(regionRect.Position, new Vector2((float)_rowBeginForm[i], regionRect.Size.Y));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (alive && GodotObject.IsInstanceValid(followSodRoll))
		{
			double progress = followSodRoll.GetProgress();
			Rect2 regionRect = _frontlawnRow[createRowId].RegionRect;
			_frontlawnRow[createRowId].RegionRect = new Rect2(regionRect.Position, new Vector2((float)(_rowBeginForm[createRowId] + progress * 790.0 + _rowEndForm[createRowId]), regionRect.Size.Y));
		}
	}

	public override Dictionary SaveMapBase()
	{
		Dictionary dictionary = base.SaveMapBase();
		Array array = new Array();
		Array array2 = new Array();
		if (_frontlawnRow != null)
		{
			Sprite2D[] frontlawnRow = _frontlawnRow;
			foreach (Sprite2D sprite2D in frontlawnRow)
			{
				if (GodotObject.IsInstanceValid(sprite2D))
				{
					array.Add(sprite2D.Visible);
					array2.Add(sprite2D.RegionRect.Size.X);
				}
				else
				{
					array.Add(false);
					array2.Add(0.0);
				}
			}
		}
		dictionary["rowVisible"] = array;
		dictionary["rowRegionWidth"] = array2;
		return dictionary;
	}

	public override void LoadMapBase(Dictionary data)
	{
		base.LoadMapBase(data);
		if (_frontlawnRow == null)
		{
			return;
		}
		Array array = data.GetValueOrDefault("rowVisible", new Array()).AsGodotArray();
		Array array2 = data.GetValueOrDefault("rowRegionWidth", new Array()).AsGodotArray();
		for (int i = 0; i < _frontlawnRow.Length; i++)
		{
			if (GodotObject.IsInstanceValid(_frontlawnRow[i]))
			{
				if (i < array.Count)
				{
					_frontlawnRow[i].Visible = array[i].AsBool();
				}
				if (i < array2.Count)
				{
					Rect2 regionRect = _frontlawnRow[i].RegionRect;
					_frontlawnRow[i].RegionRect = new Rect2(regionRect.Position, new Vector2(array2[i].AsSingle(), regionRect.Size.Y));
				}
			}
		}
		LoadLegacyRevealVisibility(data, "frontlawnFloorVisible", "%FrontlawnFloor");
		LoadLegacyRevealVisibility(data, "frontlawnDoorVisible", "%FrontlawnDoor");
		alive = false;
		createRowId = -1;
		followSodRoll = null;
	}

	public override bool CanSaveProgress(out string reason)
	{
		if (alive || GodotObject.IsInstanceValid(followSodRoll))
		{
			reason = "the lawn expansion animation is still running";
			return false;
		}
		return base.CanSaveProgress(out reason);
	}

	private void LoadLegacyRevealVisibility(Dictionary data, string stateKey, string nodePath)
	{
		if (data.ContainsKey(stateKey))
		{
			CanvasItem nodeOrNull = GetNodeOrNull<CanvasItem>(nodePath);
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				nodeOrNull.Visible = data[stateKey].AsBool();
			}
		}
	}

	public void Row1Create()
	{
		AudioManager.Instance.AudioPlay("DirtRiseLong");
		TowerDefenseManager.Instance.SetMapLineUse(3, use: true);
		_frontlawnRow[0].Visible = true;
		TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
		towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
		{
			TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
			TowerDefenseEnum.PLANTGRIDTYPE.AIR
		};
		towerDefenseCellConfig.pos = new Vector4I(1, 3, 9, 3);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
		followSodRoll = CreateSodRoll(2);
		followSodRoll.OnAnimeCompleted += Finish;
		createRowId = 0;
		alive = true;
	}

	public void Row1Finish(string clip = "")
	{
		TowerDefenseManager.Instance.SetMapLineUse(3, use: true);
		_frontlawnRow[0].Visible = true;
		_frontlawnRow[1].Visible = false;
		_frontlawnRow[2].Visible = false;
		TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
		towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
		{
			TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
			TowerDefenseEnum.PLANTGRIDTYPE.AIR
		};
		towerDefenseCellConfig.pos = new Vector4I(1, 3, 9, 3);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
	}

	public void Row3CreateFromRow1()
	{
		AudioManager.Instance.AudioPlay("DirtRiseLong");
		TowerDefenseManager.Instance.SetMapLineUse(2, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(4, use: true);
		TowerDefenseManager.Instance.CreateMower(2);
		TowerDefenseManager.Instance.CreateMower(4);
		followSodRoll = CreateSodRoll(1);
		followSodRoll.OnAnimeCompleted += (string clip) =>
		{
			_frontlawnRow[0].Visible = false;
			Finish();
		};
		CreateSodRoll(3);
		_frontlawnRow[0].Visible = true;
		_frontlawnRow[1].Visible = true;
		TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
		towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
		{
			TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
			TowerDefenseEnum.PLANTGRIDTYPE.AIR
		};
		towerDefenseCellConfig.pos = new Vector4I(1, 2, 9, 2);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
		towerDefenseCellConfig.pos = new Vector4I(1, 4, 9, 4);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
		createRowId = 1;
		alive = true;
	}

	public void Row3Finish(string clip = "")
	{
		TowerDefenseManager.Instance.SetMapLineUse(2, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(3, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(4, use: true);
		_frontlawnRow[0].Visible = false;
		_frontlawnRow[1].Visible = true;
		_frontlawnRow[2].Visible = false;
		TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
		towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
		{
			TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
			TowerDefenseEnum.PLANTGRIDTYPE.AIR
		};
		towerDefenseCellConfig.pos = new Vector4I(1, 2, 9, 4);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
	}

	public void Row5CreateFromRow3()
	{
		AudioManager.Instance.AudioPlay("DirtRiseLong");
		TowerDefenseManager.Instance.SetMapLineUse(1, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(5, use: true);
		TowerDefenseManager.Instance.CreateMower(1);
		TowerDefenseManager.Instance.CreateMower(5);
		followSodRoll = CreateSodRoll(0);
		followSodRoll.OnAnimeCompleted += (string clip) =>
		{
			_frontlawnRow[1].Visible = false;
			Finish();
		};
		CreateSodRoll(4);
		_frontlawnRow[1].Visible = true;
		_frontlawnRow[2].Visible = true;
		TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
		towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
		{
			TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
			TowerDefenseEnum.PLANTGRIDTYPE.AIR
		};
		towerDefenseCellConfig.pos = new Vector4I(1, 1, 9, 1);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
		towerDefenseCellConfig.pos = new Vector4I(1, 5, 9, 5);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
		createRowId = 2;
		alive = true;
	}

	public void Row5Finish(string clip = "")
	{
		TowerDefenseManager.Instance.SetMapLineUse(1, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(2, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(3, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(4, use: true);
		TowerDefenseManager.Instance.SetMapLineUse(5, use: true);
		_frontlawnRow[0].Visible = false;
		_frontlawnRow[1].Visible = false;
		_frontlawnRow[2].Visible = true;
		TowerDefenseCellConfig towerDefenseCellConfig = new TowerDefenseCellConfig();
		towerDefenseCellConfig.gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
		{
			TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
			TowerDefenseEnum.PLANTGRIDTYPE.AIR
		};
		towerDefenseCellConfig.pos = new Vector4I(1, 1, 9, 1);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
		towerDefenseCellConfig.pos = new Vector4I(1, 5, 9, 5);
		TowerDefenseManager.Instance.SetMapGridType(towerDefenseCellConfig);
	}

	public void Finish(string clip = "")
	{
		Rect2 regionRect = _frontlawnRow[createRowId].RegionRect;
		_frontlawnRow[createRowId].RegionRect = new Rect2(regionRect.Position, new Vector2((float)(_rowBeginForm[createRowId] + 790.0 + _rowEndForm[createRowId]), regionRect.Size.Y));
		alive = false;
		createRowId = -1;
		followSodRoll = null;
	}

	public void Delete(string clip = "", AdobeAnimateSprite instance = null)
	{
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.QueueFree();
		}
	}

	public AdobeAnimateSprite CreateSodRoll(int line)
	{
		PackedScene packedScene = GD.Load<PackedScene>("res://Asset/Anime/Effect/SodRoll/SodRoll.tscn");
		AdobeAnimateSprite instance = packedScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		instance.Position = new Vector2(210f, 110 + 100 * line);
		instance.SetAnimation("Idle");
		instance.OnAnimeCompleted += (string clip) =>
		{
			Delete(clip, instance);
		};
		AddChild(instance, forceReadableName: false, InternalMode.Disabled);
		return instance;
	}

	public void ShowRow1()
	{
		_frontlawnRow[0].Visible = true;
		Rect2 regionRect = _frontlawnRow[0].RegionRect;
		_frontlawnRow[0].RegionRect = new Rect2(regionRect.Position, new Vector2((float)(_rowBeginForm[0] + 790.0 + _rowEndForm[0]), regionRect.Size.Y));
		Row1Finish();
	}

	public void ShowRow3()
	{
		_frontlawnRow[1].Visible = true;
		Rect2 regionRect = _frontlawnRow[1].RegionRect;
		_frontlawnRow[1].RegionRect = new Rect2(regionRect.Position, new Vector2((float)(_rowBeginForm[1] + 790.0 + _rowEndForm[1]), regionRect.Size.Y));
		Row3Finish();
	}

	public void ShowRow5()
	{
		_frontlawnRow[2].Visible = true;
		Rect2 regionRect = _frontlawnRow[2].RegionRect;
		_frontlawnRow[2].RegionRect = new Rect2(regionRect.Position, new Vector2((float)(_rowBeginForm[2] + 790.0 + _rowEndForm[2]), regionRect.Size.Y));
		Row5Finish();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveMapBase, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadMapBase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadLegacyRevealVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "stateKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "nodePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Row1Create, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Row1Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Row3CreateFromRow1, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Row3Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Row5CreateFromRow3, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Row5Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Delete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSodRoll, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowRow1, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowRow3, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowRow5, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveMapBase && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveMapBase());
			return true;
		}
		if (method == MethodName.LoadMapBase && args.Count == 1)
		{
			LoadMapBase(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadLegacyRevealVisibility && args.Count == 3)
		{
			LoadLegacyRevealVisibility(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Row1Create && args.Count == 0)
		{
			Row1Create();
			ret = default;
			return true;
		}
		if (method == MethodName.Row1Finish && args.Count == 1)
		{
			Row1Finish(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Row3CreateFromRow1 && args.Count == 0)
		{
			Row3CreateFromRow1();
			ret = default;
			return true;
		}
		if (method == MethodName.Row3Finish && args.Count == 1)
		{
			Row3Finish(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Row5CreateFromRow3 && args.Count == 0)
		{
			Row5CreateFromRow3();
			ret = default;
			return true;
		}
		if (method == MethodName.Row5Finish && args.Count == 1)
		{
			Row5Finish(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 1)
		{
			Finish(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Delete && args.Count == 2)
		{
			Delete(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSodRoll && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(CreateSodRoll(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowRow1 && args.Count == 0)
		{
			ShowRow1();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRow3 && args.Count == 0)
		{
			ShowRow3();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRow5 && args.Count == 0)
		{
			ShowRow5();
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SaveMapBase)
		{
			return true;
		}
		if (method == MethodName.LoadMapBase)
		{
			return true;
		}
		if (method == MethodName.LoadLegacyRevealVisibility)
		{
			return true;
		}
		if (method == MethodName.Row1Create)
		{
			return true;
		}
		if (method == MethodName.Row1Finish)
		{
			return true;
		}
		if (method == MethodName.Row3CreateFromRow1)
		{
			return true;
		}
		if (method == MethodName.Row3Finish)
		{
			return true;
		}
		if (method == MethodName.Row5CreateFromRow3)
		{
			return true;
		}
		if (method == MethodName.Row5Finish)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.Delete)
		{
			return true;
		}
		if (method == MethodName.CreateSodRoll)
		{
			return true;
		}
		if (method == MethodName.ShowRow1)
		{
			return true;
		}
		if (method == MethodName.ShowRow3)
		{
			return true;
		}
		if (method == MethodName.ShowRow5)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._frontlawnRow)
		{
			_frontlawnRow = VariantUtils.ConvertToSystemArrayOfGodotObject<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._rowBeginForm)
		{
			_rowBeginForm = VariantUtils.ConvertTo<double[]>(in value);
			return true;
		}
		if (name == PropertyName._rowEndForm)
		{
			_rowEndForm = VariantUtils.ConvertTo<double[]>(in value);
			return true;
		}
		if (name == PropertyName.alive)
		{
			alive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.createRowId)
		{
			createRowId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.followSodRoll)
		{
			followSodRoll = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._frontlawnRow)
		{
			GodotObject[] frontlawnRow = _frontlawnRow;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(frontlawnRow);
			return true;
		}
		if (name == PropertyName._rowBeginForm)
		{
			value = VariantUtils.CreateFrom(in _rowBeginForm);
			return true;
		}
		if (name == PropertyName._rowEndForm)
		{
			value = VariantUtils.CreateFrom(in _rowEndForm);
			return true;
		}
		if (name == PropertyName.alive)
		{
			value = VariantUtils.CreateFrom(in alive);
			return true;
		}
		if (name == PropertyName.createRowId)
		{
			value = VariantUtils.CreateFrom(in createRowId);
			return true;
		}
		if (name == PropertyName.followSodRoll)
		{
			value = VariantUtils.CreateFrom(in followSodRoll);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._frontlawnRow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._rowBeginForm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._rowEndForm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.alive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.createRowId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.followSodRoll, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		StringName frontlawnRow = PropertyName._frontlawnRow;
		GodotObject[] frontlawnRow2 = _frontlawnRow;
		info.AddProperty(frontlawnRow, Variant.CreateFrom(frontlawnRow2));
		info.AddProperty(PropertyName._rowBeginForm, Variant.From(in _rowBeginForm));
		info.AddProperty(PropertyName._rowEndForm, Variant.From(in _rowEndForm));
		info.AddProperty(PropertyName.alive, Variant.From(in alive));
		info.AddProperty(PropertyName.createRowId, Variant.From(in createRowId));
		info.AddProperty(PropertyName.followSodRoll, Variant.From(in followSodRoll));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._frontlawnRow, out var value))
		{
			_frontlawnRow = value.AsGodotObjectArray<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._rowBeginForm, out var value2))
		{
			_rowBeginForm = value2.As<double[]>();
		}
		if (info.TryGetProperty(PropertyName._rowEndForm, out var value3))
		{
			_rowEndForm = value3.As<double[]>();
		}
		if (info.TryGetProperty(PropertyName.alive, out var value4))
		{
			alive = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.createRowId, out var value5))
		{
			createRowId = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.followSodRoll, out var value6))
		{
			followSodRoll = value6.As<AdobeAnimateSprite>();
		}
	}
}
