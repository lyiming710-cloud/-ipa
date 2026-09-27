using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/PumpSled/Scene/TowerDefensePlantPumpSled.cs")]
public class TowerDefensePlantPumpSled : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleExited = "IdleExited";

		public new static readonly StringName SleepProcessing = "SleepProcessing";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public static readonly StringName RunMovement = "RunMovement";

		public static readonly StringName RunStart = "RunStart";

		public static readonly StringName CheckCanRun = "CheckCanRun";

		public static readonly StringName CheckIceCap = "CheckIceCap";

		public static readonly StringName CheckSameCharacter = "CheckSameCharacter";

		public static readonly StringName CanCarryCharacterWithinSledCells = "CanCarryCharacterWithinSledCells";

		public static readonly StringName IsCarryCell = "IsCarryCell";

		public static readonly StringName ProcessAabbAttacks = "ProcessAabbAttacks";

		public static readonly StringName CheckAttack = "CheckAttack";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName allEventList = "allEventList";

		public static readonly StringName carrayCharacter1 = "carrayCharacter1";

		public static readonly StringName carrayCharacter2 = "carrayCharacter2";

		public static readonly StringName hitCharacterList = "hitCharacterList";

		public static readonly StringName run = "run";

		public static readonly StringName speed = "speed";

		public static readonly StringName _pendingCarrayCharacter1Name = "_pendingCarrayCharacter1Name";

		public static readonly StringName _pendingCarrayCharacter2Name = "_pendingCarrayCharacter2Name";

		public static readonly StringName _pendingHitCharacterListNames = "_pendingHitCharacterListNames";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> allEventList = new Array<TowerDefenseCharacterEventBase>();

	public TowerDefenseCharacter carrayCharacter1;

	public TowerDefenseCharacter carrayCharacter2;

	public Array<TowerDefenseCharacter> hitCharacterList = new Array<TowerDefenseCharacter>();

	public bool run;

	public double speed = 200.0;

	private string _pendingCarrayCharacter1Name = "";

	private string _pendingCarrayCharacter2Name = "";

	private Array _pendingHitCharacterListNames = new Array();

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !inGame && !editorMapPreviewMode)
		{
			((PumpSledSprite)sprite).Back.ZIndex = 0;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (_pendingCarrayCharacter1Name != "")
		{
			Node2D node2D = TowerDefenseManager.GetCharacterNode();
			if (GodotObject.IsInstanceValid(node2D))
			{
				Node nodeOrNull = node2D.GetNodeOrNull(_pendingCarrayCharacter1Name);
				if (GodotObject.IsInstanceValid(nodeOrNull))
				{
					carrayCharacter1 = (TowerDefenseCharacter)nodeOrNull;
				}
			}
			_pendingCarrayCharacter1Name = "";
		}
		if (_pendingCarrayCharacter2Name != "")
		{
			Node2D node2D2 = TowerDefenseManager.GetCharacterNode();
			if (GodotObject.IsInstanceValid(node2D2))
			{
				Node nodeOrNull2 = node2D2.GetNodeOrNull(_pendingCarrayCharacter2Name);
				if (GodotObject.IsInstanceValid(nodeOrNull2))
				{
					carrayCharacter2 = (TowerDefenseCharacter)nodeOrNull2;
				}
			}
			_pendingCarrayCharacter2Name = "";
		}
		if (_pendingHitCharacterListNames.Count > 0)
		{
			Node2D node2D3 = TowerDefenseManager.GetCharacterNode();
			if (GodotObject.IsInstanceValid(node2D3))
			{
				foreach (Variant pendingHitCharacterListName in _pendingHitCharacterListNames)
				{
					string text = (string)pendingHitCharacterListName;
					Node nodeOrNull3 = node2D3.GetNodeOrNull(text);
					if (GodotObject.IsInstanceValid(nodeOrNull3))
					{
						hitCharacterList.Add((TowerDefenseCharacter)nodeOrNull3);
					}
				}
			}
			_pendingHitCharacterListNames = new Array();
		}
		ProcessAabbAttacks();
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
		if (!run)
		{
			PuzzleShaderComponent puzzleShaderComponent = base.puzzleShaderComponent;
			if (puzzleShaderComponent != null && !puzzleShaderComponent.IsReleased)
			{
				base.puzzleShaderComponent.IdleEntered();
			}
		}
	}

	public override void IdleExited()
	{
		base.IdleExited();
		if (!run)
		{
			PuzzleShaderComponent puzzleShaderComponent = base.puzzleShaderComponent;
			if (puzzleShaderComponent != null && !puzzleShaderComponent.IsReleased)
			{
				base.puzzleShaderComponent.IdleExited();
			}
		}
	}

	public override void SleepProcessing(double delta)
	{
		base.SleepProcessing(delta);
		if (!run)
		{
			if (CheckCanRun())
			{
				RunStart();
			}
		}
		else
		{
			RunMovement(delta);
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (!run)
		{
			if (CheckCanRun())
			{
				RunStart();
			}
		}
		else
		{
			RunMovement(delta);
		}
	}

	public void RunMovement(double delta)
	{
		if (sprite.timeScale == 0.0)
		{
			sprite.timeScale = timeScaleSave;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		globalPositionForPhysicsFrame.X += (float)(speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X);
		SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		if (GodotObject.IsInstanceValid(carrayCharacter1))
		{
			Vector2 logicalGlobalPosition = carrayCharacter1.GetLogicalGlobalPosition();
			logicalGlobalPosition.X = globalPositionForPhysicsFrame.X;
			carrayCharacter1.SetLogicalGlobalPosition(logicalGlobalPosition);
		}
		if (GodotObject.IsInstanceValid(carrayCharacter2))
		{
			Vector2 logicalGlobalPosition2 = carrayCharacter2.GetLogicalGlobalPosition();
			logicalGlobalPosition2.X = globalPositionForPhysicsFrame.X + 90f;
			carrayCharacter2.SetLogicalGlobalPosition(logicalGlobalPosition2);
		}
		if ((double)globalPositionForPhysicsFrame.X > TowerDefenseManager.Instance.GetMapGroundRight() + 200.0 || (double)globalPositionForPhysicsFrame.X < TowerDefenseManager.Instance.GetMapGroundLeft() - 200.0)
		{
			QueueFree();
			if (GodotObject.IsInstanceValid(carrayCharacter1))
			{
				carrayCharacter1.QueueFree();
			}
			if (GodotObject.IsInstanceValid(carrayCharacter2))
			{
				carrayCharacter2.QueueFree();
			}
		}
		if (Engine.GetPhysicsFrames() % 2 == 0L)
		{
			TowerDefenseExplode.CreateExplode(globalPositionForPhysicsFrame + new Vector2(46f, 0f), new Vector2(0.75f, 0.25f), allEventList, new Array<TowerDefenseCharacter>(), TowerDefenseEnum.CHARACTER_CAMP.ALL, -1);
		}
	}

	public void RunStart()
	{
		run = true;
		if (TowerDefenseManager.Instance.IsIZMMode())
		{
			timeScaleInit = timeScaleSave;
			timeScale = timeScaleSave;
			sprite.timeScale = timeScaleSave;
			Idle();
		}
		EmitDestroy();
		if (GodotObject.IsInstanceValid(carrayCharacter1))
		{
			carrayCharacter1.Destroy(freeInstance: false);
		}
		if (GodotObject.IsInstanceValid(carrayCharacter2))
		{
			carrayCharacter2.Destroy(freeInstance: false);
		}
	}

	public bool CheckCanRun()
	{
		if (CheckIceCap())
		{
			return true;
		}
		if (CheckSameCharacter())
		{
			return true;
		}
		return false;
	}

	public bool CheckIceCap()
	{
		TowerDefenseIceCap towerDefenseIceCap = TowerDefenseManager.Instance.GetMapIceCapList()[gridPos.Y].As<TowerDefenseIceCap>();
		if (GodotObject.IsInstanceValid(towerDefenseIceCap) && TowerDefenseManager.Instance.GetMapGridPos(towerDefenseIceCap.iceCapSprite.GlobalPosition).X <= gridPos.X + 1)
		{
			return true;
		}
		return false;
	}

	public bool CheckSameCharacter()
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(gridPos + new Vector2I(1, 0));
		Array<TowerDefenseCharacter> array = new Array<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter character in mapCell.GetCharacterList())
		{
			if (character is TowerDefensePlant && !character.isDestroy && (character.config.physiqueTypeFlags & 2) == 0 && (character.config.physiqueTypeFlags & 4) == 0 && character != this && !character.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && CanCarryCharacterWithinSledCells(character))
			{
				array.Add(character);
			}
		}
		if (GodotObject.IsInstanceValid(mapCell2))
		{
			foreach (TowerDefenseCharacter character2 in mapCell2.GetCharacterList())
			{
				if (character2 is TowerDefensePlant && !character2.isDestroy && (character2.config.physiqueTypeFlags & 2) == 0 && (character2.config.physiqueTypeFlags & 4) == 0 && character2 != this && !character2.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && CanCarryCharacterWithinSledCells(character2))
				{
					array.Add(character2);
				}
			}
		}
		for (int i = 0; i < array.Count; i++)
		{
			for (int j = i + 1; j < array.Count; j++)
			{
				if (array[i] != array[j] && array[i].config.name == array[j].config.name)
				{
					carrayCharacter1 = array[i];
					carrayCharacter2 = array[j];
					return true;
				}
			}
		}
		return false;
	}

	private bool CanCarryCharacterWithinSledCells(TowerDefenseCharacter characterCheck)
	{
		if (!IsCarryCell(characterCheck.gridPos))
		{
			return false;
		}
		if (!(characterCheck.config is TowerDefensePlantConfig towerDefensePlantConfig))
		{
			return true;
		}
		foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
		{
			Vector2I vector2I = characterCheck.gridPos + item;
			if (vector2I != gridPos && vector2I != gridPos + new Vector2I(1, 0))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsCarryCell(Vector2I cell)
	{
		if (!(cell == gridPos))
		{
			return cell == gridPos + new Vector2I(1, 0);
		}
		return true;
	}

	private void ProcessAabbAttacks()
	{
		if (run && TryGetActiveWorldHitRect(out var rect))
		{
			List<TowerDefenseCharacter> charactersIntersectingRectListExcludingCamp = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectListExcludingCamp(rect, camp);
			for (int i = 0; i < charactersIntersectingRectListExcludingCamp.Count; i++)
			{
				CheckAttack(charactersIntersectingRectListExcludingCamp[i]);
			}
		}
	}

	public void CheckAttack(TowerDefenseCharacter tdCharacter)
	{
		if (run && GodotObject.IsInstanceValid(tdCharacter) && tdCharacter != this && !hitCharacterList.Contains(tdCharacter) && CanCollision(tdCharacter.instance.maskFlags) && CanTarget(tdCharacter) && tdCharacter.instance.canBeCollection)
		{
			double num = 1800.0;
			if (GodotObject.IsInstanceValid(carrayCharacter1))
			{
				num += carrayCharacter1.GetCurrentHitPoint();
			}
			if (GodotObject.IsInstanceValid(carrayCharacter2))
			{
				num += carrayCharacter2.GetCurrentHitPoint();
			}
			tdCharacter.Hurt(num);
			hitCharacterList.Add(tdCharacter);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			{ "run", run },
			{ "speed", speed }
		};
		if (GodotObject.IsInstanceValid(carrayCharacter1))
		{
			dictionary["carrayCharacter1NodeName"] = carrayCharacter1.Name;
		}
		if (GodotObject.IsInstanceValid(carrayCharacter2))
		{
			dictionary["carrayCharacter2NodeName"] = carrayCharacter2.Name;
		}
		if (hitCharacterList.Count > 0)
		{
			Array array = new Array();
			foreach (TowerDefenseCharacter hitCharacter in hitCharacterList)
			{
				if (GodotObject.IsInstanceValid(hitCharacter))
				{
					array.Add(hitCharacter.Name);
				}
			}
			dictionary["hitCharacterNodeNames"] = array;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		run = data.GetValueOrDefault("run", Variant.From<bool>(false)).AsBool();
		speed = data.GetValueOrDefault("speed", Variant.From<double>(200.0)).AsDouble();
		if (data.ContainsKey("carrayCharacter1NodeName"))
		{
			_pendingCarrayCharacter1Name = data["carrayCharacter1NodeName"].AsString();
		}
		if (data.ContainsKey("carrayCharacter2NodeName"))
		{
			_pendingCarrayCharacter2Name = data["carrayCharacter2NodeName"].AsString();
		}
		if (data.ContainsKey("hitCharacterNodeNames"))
		{
			_pendingHitCharacterListNames = (Array)data["hitCharacterNodeNames"];
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(17)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SleepProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunMovement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckCanRun, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckIceCap, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckSameCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCarryCharacterWithinSledCells, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "characterCheck", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsCarryCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessAabbAttacks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tdCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleExited && args.Count == 0)
		{
			IdleExited();
			ret = default;
			return true;
		}
		if (method == MethodName.SleepProcessing && args.Count == 1)
		{
			SleepProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunMovement && args.Count == 1)
		{
			RunMovement(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunStart && args.Count == 0)
		{
			RunStart();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckCanRun && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckCanRun());
			return true;
		}
		if (method == MethodName.CheckIceCap && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckIceCap());
			return true;
		}
		if (method == MethodName.CheckSameCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckSameCharacter());
			return true;
		}
		if (method == MethodName.CanCarryCharacterWithinSledCells && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCarryCharacterWithinSledCells(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCarryCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCarryCell(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessAabbAttacks && args.Count == 0)
		{
			ProcessAabbAttacks();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckAttack && args.Count == 1)
		{
			CheckAttack(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IdleExited)
		{
			return true;
		}
		if (method == MethodName.SleepProcessing)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.RunMovement)
		{
			return true;
		}
		if (method == MethodName.RunStart)
		{
			return true;
		}
		if (method == MethodName.CheckCanRun)
		{
			return true;
		}
		if (method == MethodName.CheckIceCap)
		{
			return true;
		}
		if (method == MethodName.CheckSameCharacter)
		{
			return true;
		}
		if (method == MethodName.CanCarryCharacterWithinSledCells)
		{
			return true;
		}
		if (method == MethodName.IsCarryCell)
		{
			return true;
		}
		if (method == MethodName.ProcessAabbAttacks)
		{
			return true;
		}
		if (method == MethodName.CheckAttack)
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
		if (name == PropertyName.allEventList)
		{
			allEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.carrayCharacter1)
		{
			carrayCharacter1 = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.carrayCharacter2)
		{
			carrayCharacter2 = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.hitCharacterList)
		{
			hitCharacterList = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.run)
		{
			run = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingCarrayCharacter1Name)
		{
			_pendingCarrayCharacter1Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingCarrayCharacter2Name)
		{
			_pendingCarrayCharacter2Name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingHitCharacterListNames)
		{
			_pendingHitCharacterListNames = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.allEventList)
		{
			value = VariantUtils.CreateFromArray(allEventList);
			return true;
		}
		if (name == PropertyName.carrayCharacter1)
		{
			value = VariantUtils.CreateFrom(in carrayCharacter1);
			return true;
		}
		if (name == PropertyName.carrayCharacter2)
		{
			value = VariantUtils.CreateFrom(in carrayCharacter2);
			return true;
		}
		if (name == PropertyName.hitCharacterList)
		{
			value = VariantUtils.CreateFromArray(hitCharacterList);
			return true;
		}
		if (name == PropertyName.run)
		{
			value = VariantUtils.CreateFrom(in run);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName._pendingCarrayCharacter1Name)
		{
			value = VariantUtils.CreateFrom(in _pendingCarrayCharacter1Name);
			return true;
		}
		if (name == PropertyName._pendingCarrayCharacter2Name)
		{
			value = VariantUtils.CreateFrom(in _pendingCarrayCharacter2Name);
			return true;
		}
		if (name == PropertyName._pendingHitCharacterListNames)
		{
			value = VariantUtils.CreateFrom(in _pendingHitCharacterListNames);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.allEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.carrayCharacter1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.carrayCharacter2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitCharacterList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.run, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingCarrayCharacter1Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingCarrayCharacter2Name, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._pendingHitCharacterListNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.allEventList, Variant.CreateFrom(allEventList));
		info.AddProperty(PropertyName.carrayCharacter1, Variant.From(in carrayCharacter1));
		info.AddProperty(PropertyName.carrayCharacter2, Variant.From(in carrayCharacter2));
		info.AddProperty(PropertyName.hitCharacterList, Variant.CreateFrom(hitCharacterList));
		info.AddProperty(PropertyName.run, Variant.From(in run));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName._pendingCarrayCharacter1Name, Variant.From(in _pendingCarrayCharacter1Name));
		info.AddProperty(PropertyName._pendingCarrayCharacter2Name, Variant.From(in _pendingCarrayCharacter2Name));
		info.AddProperty(PropertyName._pendingHitCharacterListNames, Variant.From(in _pendingHitCharacterListNames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.allEventList, out var value))
		{
			allEventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.carrayCharacter1, out var value2))
		{
			carrayCharacter1 = value2.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.carrayCharacter2, out var value3))
		{
			carrayCharacter2 = value3.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.hitCharacterList, out var value4))
		{
			hitCharacterList = value4.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.run, out var value5))
		{
			run = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value6))
		{
			speed = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingCarrayCharacter1Name, out var value7))
		{
			_pendingCarrayCharacter1Name = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingCarrayCharacter2Name, out var value8))
		{
			_pendingCarrayCharacter2Name = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingHitCharacterListNames, out var value9))
		{
			_pendingHitCharacterListNames = value9.As<Array>();
		}
	}
}
