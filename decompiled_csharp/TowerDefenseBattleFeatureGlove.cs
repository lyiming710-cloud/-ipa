using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Glove/TowerDefenseBattleFeatureGlove.cs")]
public class TowerDefenseBattleFeatureGlove : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName InitializeManager = "InitializeManager";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName Destroy = "Destroy";

		public static readonly StringName PickGlove = "PickGlove";

		public static readonly StringName ProcessGlovePick = "ProcessGlovePick";

		public static readonly StringName ProcessGloveSelect = "ProcessGloveSelect";

		public static readonly StringName ProcessGlovePlace = "ProcessGlovePlace";

		public static readonly StringName SelectCharacter = "SelectCharacter";

		public static readonly StringName PlaceCharacter = "PlaceCharacter";

		public static readonly StringName FinishGlovePlacement = "FinishGlovePlacement";

		public static readonly StringName ClearPickedState = "ClearPickedState";

		public static readonly StringName GloveRelease = "GloveRelease";

		public static readonly StringName GloveReset = "GloveReset";

		public static readonly StringName GloveButtonPressed = "GloveButtonPressed";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName gloveManager = "gloveManager";

		public static readonly StringName glovePickTool = "glovePickTool";

		public static readonly StringName glovePick = "glovePick";

		public static readonly StringName pickedCharacter = "pickedCharacter";

		public static readonly StringName sourceGridPos = "sourceGridPos";

		public static readonly StringName sourceCell = "sourceCell";

		public static readonly StringName _mapFeature = "_mapFeature";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _gloveManager;

	public GloveManager gloveManager;

	public GlovePickTool glovePickTool;

	public bool glovePick;

	public TowerDefenseCharacter pickedCharacter;

	public Vector2I sourceGridPos = Vector2I.Zero;

	public TowerDefenseCellInstance sourceCell;

	private TowerDefenseBattleFeatureMap _mapFeature;

	private static PackedScene GLOVE_MANAGER => _gloveManager ?? (_gloveManager = GD.Load<PackedScene>("uid://d2i0ohax0btl4"));

	public override void Init(Dictionary _data)
	{
		base.Init(_data);
		gloveManager = GLOVE_MANAGER.Instantiate<GloveManager>(PackedScene.GenEditState.Disabled);
		control.AddUIToTopPropContainer(gloveManager);
	}

	public override Task GameInit()
	{
		InitializeManager();
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		InitializeManager();
		return Task.CompletedTask;
	}

	public override Task GameStart()
	{
		if (!GodotObject.IsInstanceValid(gloveManager))
		{
			return Task.CompletedTask;
		}
		if (GodotObject.IsInstanceValid(_mapFeature) && GodotObject.IsInstanceValid(_mapFeature.packetPickControl) && !GodotObject.IsInstanceValid(glovePickTool))
		{
			glovePickTool = new GlovePickTool();
			glovePickTool.Init(_mapFeature.mapControl);
			glovePickTool.SetGloveFeature(this);
			_mapFeature.packetPickControl.RegisterTool(glovePickTool);
		}
		return Task.CompletedTask;
	}

	private void InitializeManager()
	{
		if (GodotObject.IsInstanceValid(gloveManager))
		{
			_mapFeature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
			if (GodotObject.IsInstanceValid(_mapFeature) && GodotObject.IsInstanceValid(_mapFeature.mapControl))
			{
				_mapFeature.gloveManager = gloveManager;
				gloveManager.Init(_mapFeature.mapControl, _mapFeature);
			}
		}
	}

	public override void Process(double _delta)
	{
		if (!GodotObject.IsInstanceValid(gloveManager))
		{
			return;
		}
		if (glovePick)
		{
			TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
			if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.mapControl))
			{
				gloveManager.UpdateGloveSprite(mapFeature.mapControl);
			}
		}
		if (!GodotObject.IsInstanceValid(pickedCharacter) || !GodotObject.IsInstanceValid(gloveManager.previewSprite))
		{
			return;
		}
		gloveManager.previewSprite.Visible = true;
		TowerDefenseBattleFeatureMap mapFeature2 = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature2) && GodotObject.IsInstanceValid(mapFeature2.mapControl))
		{
			Vector2 globalMousePosition = mapFeature2.mapControl.GetGlobalMousePosition();
			Vector2I mapGridPosFromMouse = TowerDefenseManager.Instance.GetMapGridPosFromMouse(globalMousePosition);
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(mapGridPosFromMouse);
			bool flag = false;
			if (mapGridPosFromMouse != sourceGridPos && TowerDefenseManager.Instance.CheckMapGridPosIn(mapGridPosFromMouse) && GodotObject.IsInstanceValid(mapCell) && GodotObject.IsInstanceValid(pickedCharacter) && GodotObject.IsInstanceValid(pickedCharacter.packet))
			{
				flag = mapCell.CanPacketPlant(pickedCharacter.packet);
			}
			if (flag)
			{
				Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(mapGridPosFromMouse);
				gloveManager.previewSprite.GlobalPosition = mapCellPlantPos;
			}
			else
			{
				gloveManager.previewSprite.Position = mapFeature2.mapControl.spriteNode.GetLocalMousePosition() - new Vector2(0f, 20f);
			}
		}
	}

	public override void Destroy()
	{
		if (GodotObject.IsInstanceValid(_mapFeature) && GodotObject.IsInstanceValid(_mapFeature.packetPickControl) && GodotObject.IsInstanceValid(glovePickTool))
		{
			_mapFeature.packetPickControl.UnregisterTool(glovePickTool);
		}
		if (GodotObject.IsInstanceValid(glovePickTool))
		{
			glovePickTool.SetGloveFeature(null);
			glovePickTool.Free();
		}
		glovePickTool = null;
		if (GodotObject.IsInstanceValid(_mapFeature) && _mapFeature.gloveManager == gloveManager)
		{
			_mapFeature.gloveManager = null;
		}
		ClearPickedState();
		if (GodotObject.IsInstanceValid(gloveManager))
		{
			gloveManager.glovePressedAwait = false;
			gloveManager.QueueFree();
		}
		gloveManager = null;
		_mapFeature = null;
		base.Destroy();
	}

	public void PickGlove(bool open)
	{
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.packetPickControl))
		{
			mapFeature.packetPickControl.PacketPickRelease();
		}
		if (open)
		{
			AudioManager.Instance.AudioPlay("Shovel");
			glovePick = true;
			gloveManager.ShowMapGloveSprite(show: true);
			gloveManager.SetMapGloveSpritePos(new Vector2(-100f, -100f));
		}
		else
		{
			AudioManager.Instance.AudioPlay("ShovelDeny");
			glovePick = false;
			gloveManager.ShowMapGloveSprite(show: false);
			ClearPickedState();
		}
	}

	public void ProcessGlovePick(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		if (!GodotObject.IsInstanceValid(pickedCharacter))
		{
			ProcessGloveSelect(cell, gridPos, mousePos);
		}
		else
		{
			ProcessGlovePlace(cell, gridPos, mousePos);
		}
	}

	public void ProcessGloveSelect(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		if (TowerDefenseManager.Instance.CheckMapGridPosIn(gridPos) && GodotObject.IsInstanceValid(cell))
		{
			double groundHeight = TowerDefenseManager.GetMapFeature().GetGroundHeight(cell);
			Vector2 mapCellPos = TowerDefenseManager.Instance.GetMapCellPos(gridPos);
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			double percentage = (mousePos - mapCellPos + new Vector2(0f, (float)groundHeight)).Y / mapGridSize.Y;
			TowerDefenseCharacter shovelCharacter = cell.GetShovelCharacter(percentage);
			shovelCharacter?.Bright();
			if (cell.CanShovel(percentage) && shovelCharacter is TowerDefensePlant && GodotObject.IsInstanceValid(gloveManager) && gloveManager.mapControl.IsConfirmInput())
			{
				SelectCharacter(shovelCharacter, cell, gridPos);
			}
		}
	}

	public void ProcessGlovePlace(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		if (gridPos == sourceGridPos || !TowerDefenseManager.Instance.CheckMapGridPosIn(gridPos) || !GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		bool flag = false;
		if (GodotObject.IsInstanceValid(pickedCharacter) && GodotObject.IsInstanceValid(pickedCharacter.packet))
		{
			flag = cell.CanPacketPlant(pickedCharacter.packet);
		}
		if (flag)
		{
			Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPos);
			if (GodotObject.IsInstanceValid(gloveManager) && GodotObject.IsInstanceValid(gloveManager.previewSprite))
			{
				gloveManager.previewSprite.GlobalPosition = mapCellPlantPos;
			}
			if (GodotObject.IsInstanceValid(gloveManager) && gloveManager.mapControl.IsConfirmInput())
			{
				PlaceCharacter(cell, gridPos);
			}
		}
	}

	public void SelectCharacter(TowerDefenseCharacter character, TowerDefenseCellInstance cell, Vector2I gridPos)
	{
		pickedCharacter = character;
		sourceGridPos = gridPos;
		sourceCell = cell;
		AudioManager.Instance.AudioPlay("ShovelDig");
		gloveManager.CreatePreviewSprite(character);
	}

	public void PlaceCharacter(TowerDefenseCellInstance targetCell, Vector2I targetGridPos)
	{
		if (!GodotObject.IsInstanceValid(pickedCharacter) || !GodotObject.IsInstanceValid(sourceCell) || !GodotObject.IsInstanceValid(targetCell) || targetGridPos == sourceGridPos)
		{
			ClearPickedState();
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = pickedCharacter;
		Vector2I fromGridPos = sourceGridPos;
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			if (towerDefenseCharacter.syncId >= 0)
			{
				MultiPlayerManager.Instance?.SendMoveCharacter(towerDefenseCharacter.syncId, fromGridPos, targetGridPos, "glove");
			}
			FinishGlovePlacement();
			return;
		}
		towerDefenseCharacter.EmitDestroy();
		sourceCell.MoveCharacterToCell(towerDefenseCharacter, targetCell);
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && towerDefenseCharacter.syncId >= 0)
		{
			MultiPlayerManager.Instance?.SendMoveCharacter(towerDefenseCharacter.syncId, fromGridPos, targetGridPos, "glove");
		}
		AudioManager.Instance.AudioPlay("ShovelDig");
		FinishGlovePlacement();
	}

	private void FinishGlovePlacement()
	{
		ClearPickedState();
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(mapFeature.packetPickControl))
		{
			mapFeature.packetPickControl.Release();
		}
	}

	public void ClearPickedState()
	{
		pickedCharacter = null;
		sourceCell = null;
		sourceGridPos = Vector2I.Zero;
		if (GodotObject.IsInstanceValid(gloveManager))
		{
			gloveManager.FreePreviewSprite();
		}
	}

	public void GloveRelease()
	{
		if (glovePick)
		{
			AudioManager.Instance.AudioPlay("ShovelDeny");
		}
		ClearPickedState();
		glovePick = false;
		gloveManager.ShowMapGloveSprite(show: false);
		gloveManager.SetGloveButtonPressed(pressed: false);
	}

	public void GloveReset()
	{
		ClearPickedState();
		glovePick = false;
		gloveManager.ShowMapGloveSprite(show: false);
		gloveManager.SetGloveButtonPressed(pressed: false);
	}

	public void GloveButtonPressed()
	{
		RunLifetimeTask(GloveButtonPressedAsync, "GloveButtonPressed");
	}

	private async Task GloveButtonPressedAsync()
	{
		if (!IsLifetimeActive || !GodotObject.IsInstanceValid(gloveManager))
		{
			return;
		}
		if (!gloveManager.gloveShow && (!GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning))
		{
			gloveManager.SetGloveButtonPressed(pressed: false);
		}
		else if (!gloveManager.glovePressedAwait)
		{
			ClearPickedState();
			PickGlove(gloveManager.IsGloveButtonPressed());
			gloveManager.glovePressedAwait = true;
			await ToSignal(control.GetTree().CreateTimer(0.1, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			if (IsLifetimeActive && GodotObject.IsInstanceValid(gloveManager))
			{
				gloveManager.glovePressedAwait = false;
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PickGlove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessGlovePick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessGloveSelect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessGlovePlace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlaceCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetCell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "targetGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishGlovePlacement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPickedState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GloveRelease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GloveReset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GloveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.InitializeManager && args.Count == 0)
		{
			InitializeManager();
			ret = default;
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
			return true;
		}
		if (method == MethodName.PickGlove && args.Count == 1)
		{
			PickGlove(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessGlovePick && args.Count == 3)
		{
			ProcessGlovePick(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessGloveSelect && args.Count == 3)
		{
			ProcessGloveSelect(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessGlovePlace && args.Count == 3)
		{
			ProcessGlovePlace(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectCharacter && args.Count == 3)
		{
			SelectCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlaceCharacter && args.Count == 2)
		{
			PlaceCharacter(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishGlovePlacement && args.Count == 0)
		{
			FinishGlovePlacement();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPickedState && args.Count == 0)
		{
			ClearPickedState();
			ret = default;
			return true;
		}
		if (method == MethodName.GloveRelease && args.Count == 0)
		{
			GloveRelease();
			ret = default;
			return true;
		}
		if (method == MethodName.GloveReset && args.Count == 0)
		{
			GloveReset();
			ret = default;
			return true;
		}
		if (method == MethodName.GloveButtonPressed && args.Count == 0)
		{
			GloveButtonPressed();
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
		if (method == MethodName.InitializeManager)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.PickGlove)
		{
			return true;
		}
		if (method == MethodName.ProcessGlovePick)
		{
			return true;
		}
		if (method == MethodName.ProcessGloveSelect)
		{
			return true;
		}
		if (method == MethodName.ProcessGlovePlace)
		{
			return true;
		}
		if (method == MethodName.SelectCharacter)
		{
			return true;
		}
		if (method == MethodName.PlaceCharacter)
		{
			return true;
		}
		if (method == MethodName.FinishGlovePlacement)
		{
			return true;
		}
		if (method == MethodName.ClearPickedState)
		{
			return true;
		}
		if (method == MethodName.GloveRelease)
		{
			return true;
		}
		if (method == MethodName.GloveReset)
		{
			return true;
		}
		if (method == MethodName.GloveButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.gloveManager)
		{
			gloveManager = VariantUtils.ConvertTo<GloveManager>(in value);
			return true;
		}
		if (name == PropertyName.glovePickTool)
		{
			glovePickTool = VariantUtils.ConvertTo<GlovePickTool>(in value);
			return true;
		}
		if (name == PropertyName.glovePick)
		{
			glovePick = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pickedCharacter)
		{
			pickedCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.sourceGridPos)
		{
			sourceGridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.sourceCell)
		{
			sourceCell = VariantUtils.ConvertTo<TowerDefenseCellInstance>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.gloveManager)
		{
			value = VariantUtils.CreateFrom(in gloveManager);
			return true;
		}
		if (name == PropertyName.glovePickTool)
		{
			value = VariantUtils.CreateFrom(in glovePickTool);
			return true;
		}
		if (name == PropertyName.glovePick)
		{
			value = VariantUtils.CreateFrom(in glovePick);
			return true;
		}
		if (name == PropertyName.pickedCharacter)
		{
			value = VariantUtils.CreateFrom(in pickedCharacter);
			return true;
		}
		if (name == PropertyName.sourceGridPos)
		{
			value = VariantUtils.CreateFrom(in sourceGridPos);
			return true;
		}
		if (name == PropertyName.sourceCell)
		{
			value = VariantUtils.CreateFrom(in sourceCell);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.gloveManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.glovePickTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.glovePick, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.pickedCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.sourceGridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.sourceCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.gloveManager, Variant.From(in gloveManager));
		info.AddProperty(PropertyName.glovePickTool, Variant.From(in glovePickTool));
		info.AddProperty(PropertyName.glovePick, Variant.From(in glovePick));
		info.AddProperty(PropertyName.pickedCharacter, Variant.From(in pickedCharacter));
		info.AddProperty(PropertyName.sourceGridPos, Variant.From(in sourceGridPos));
		info.AddProperty(PropertyName.sourceCell, Variant.From(in sourceCell));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.gloveManager, out var value))
		{
			gloveManager = value.As<GloveManager>();
		}
		if (info.TryGetProperty(PropertyName.glovePickTool, out var value2))
		{
			glovePickTool = value2.As<GlovePickTool>();
		}
		if (info.TryGetProperty(PropertyName.glovePick, out var value3))
		{
			glovePick = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pickedCharacter, out var value4))
		{
			pickedCharacter = value4.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.sourceGridPos, out var value5))
		{
			sourceGridPos = value5.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.sourceCell, out var value6))
		{
			sourceCell = value6.As<TowerDefenseCellInstance>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value7))
		{
			_mapFeature = value7.As<TowerDefenseBattleFeatureMap>();
		}
	}
}
