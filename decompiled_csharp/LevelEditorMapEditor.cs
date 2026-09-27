using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/MapEditor/LevelEditorMapEditor.cs")]
public class LevelEditorMapEditor : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ApplyMapPreviewConfig = "ApplyMapPreviewConfig";

		public static readonly StringName HandleMousePanButton = "HandleMousePanButton";

		public static readonly StringName HandleMousePanMotion = "HandleMousePanMotion";

		public static readonly StringName IsMousePanButtonAvailable = "IsMousePanButtonAvailable";

		public static readonly StringName ResetMousePan = "ResetMousePan";

		public static readonly StringName HandleMouseWheelInput = "HandleMouseWheelInput";

		public static readonly StringName HandleTouchInput = "HandleTouchInput";

		public static readonly StringName CanProcessMapPreviewInput = "CanProcessMapPreviewInput";

		public static readonly StringName IsScreenPointInsideMapPreview = "IsScreenPointInsideMapPreview";

		public static readonly StringName GetMapPreviewInputRect = "GetMapPreviewInputRect";

		public static readonly StringName IsScreenPointOverEditorControls = "IsScreenPointOverEditorControls";

		public static readonly StringName IsPointerOverEditorControls = "IsPointerOverEditorControls";

		public static readonly StringName RefreshPinchBaseline = "RefreshPinchBaseline";

		public static readonly StringName ApplyMapPreviewZoomAtScreenAnchor = "ApplyMapPreviewZoomAtScreenAnchor";

		public static readonly StringName CenterMapPreview = "CenterMapPreview";

		public static readonly StringName ClampMapPreviewPosition = "ClampMapPreviewPosition";

		public static readonly StringName ClampMapPreviewAxis = "ClampMapPreviewAxis";

		public static readonly StringName GetMapPreviewOrigin = "GetMapPreviewOrigin";

		public static readonly StringName RefreshMapGridMetrics = "RefreshMapGridMetrics";

		public static readonly StringName ResetTouchGesture = "ResetTouchGesture";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName Init = "Init";

		public static readonly StringName OnChildPacketBankVisibilityChanged = "OnChildPacketBankVisibilityChanged";

		public static readonly StringName Save = "Save";

		public static readonly StringName CreatePreSpawnConfig = "CreatePreSpawnConfig";

		public static readonly StringName AddCharacterOverrideProperty = "AddCharacterOverrideProperty";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName ClearCharacter = "ClearCharacter";

		public static readonly StringName ShovelButtonPressed = "ShovelButtonPressed";

		public static readonly StringName Release = "Release";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName characterNode = "characterNode";

		public static readonly StringName MapPreviewZoom = "MapPreviewZoom";

		public static readonly StringName ShouldSuppressPlacementConfirm = "ShouldSuppressPlacementConfirm";

		public static readonly StringName IsMapPanActive = "IsMapPanActive";

		public static readonly StringName _mapViewport = "_mapViewport";

		public static readonly StringName _characterNode = "_characterNode";

		public static readonly StringName _shovelButton = "_shovelButton";

		public static readonly StringName _packetBank = "_packetBank";

		public static readonly StringName _transformNode = "_transformNode";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _previewMapConfig = "_previewMapConfig";

		public static readonly StringName _minimumMapPreviewZoom = "_minimumMapPreviewZoom";

		public static readonly StringName _previousPinchDistance = "_previousPinchDistance";

		public static readonly StringName _previousPinchCenter = "_previousPinchCenter";

		public static readonly StringName _pinchActive = "_pinchActive";

		public static readonly StringName _suppressTouchConfirm = "_suppressTouchConfirm";

		public static readonly StringName _suppressTouchConfirmThroughPhysicsFrame = "_suppressTouchConfirmThroughPhysicsFrame";

		public static readonly StringName _mousePanActive = "_mousePanActive";

		public static readonly StringName _mousePanButton = "_mousePanButton";

		public static readonly StringName _previousMousePanPosition = "_previousMousePanPosition";

		public static readonly StringName _ownsMapFeature = "_ownsMapFeature";

		public static readonly StringName _ownedPacketPickControl = "_ownedPacketPickControl";

		public static readonly StringName _ownedShovelManager = "_ownedShovelManager";

		public static readonly StringName _ownedShovelPickTool = "_ownedShovelPickTool";

		public static readonly StringName levelConfig = "levelConfig";

		public static readonly StringName isolatedPreviewMode = "isolatedPreviewMode";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName shovelShow = "shovelShow";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private static readonly Vector2 MapPreviewLogicalSize = new Vector2(1200f, 600f);

	private const float MouseWheelZoomFactor = 1.1f;

	private const float MaximumMapPreviewZoom = 2f;

	private const float MinimumPinchDistance = 12f;

	public static LevelEditorMapEditor instance;

	private Node2D _mapViewport;

	private Node2D _characterNode;

	private TextureButton _shovelButton;

	private Control _packetBank;

	private Control _transformNode;

	private TowerDefenseMapControl _mapControl;

	private TowerDefenseMapConfig _previewMapConfig;

	private readonly System.Collections.Generic.Dictionary<int, Vector2> _touchPositions = new System.Collections.Generic.Dictionary<int, Vector2>();

	private float _minimumMapPreviewZoom = 1f;

	private float _previousPinchDistance;

	private Vector2 _previousPinchCenter;

	private bool _pinchActive;

	private bool _suppressTouchConfirm;

	private ulong _suppressTouchConfirmThroughPhysicsFrame;

	private bool _mousePanActive;

	private MouseButton _mousePanButton;

	private Vector2 _previousMousePanPosition;

	private bool _ownsMapFeature;

	private PacketPickControl _ownedPacketPickControl;

	private ShovelManager _ownedShovelManager;

	private ShovelPickTool _ownedShovelPickTool;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelConfig levelConfig;

	[Export(PropertyHint.None, "")]
	public bool isolatedPreviewMode;

	public TowerDefenseBattleFeatureMap mapFeature;

	public bool shovelShow;

	public Node2D characterNode => _characterNode;

	public float MapPreviewZoom
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_mapViewport))
			{
				return 1f;
			}
			return _mapViewport.Scale.X;
		}
	}

	public bool ShouldSuppressPlacementConfirm
	{
		get
		{
			if (!_suppressTouchConfirm)
			{
				return Engine.GetPhysicsFrames() <= _suppressTouchConfirmThroughPhysicsFrame;
			}
			return true;
		}
	}

	public bool IsMapPanActive => _mousePanActive;

	public override void _Ready()
	{
		instance = this;
		_mapViewport = GetNode<Node2D>("%MapViewport");
		_characterNode = GetNode<Node2D>("%CharacterNode");
		_shovelButton = GetNode<TextureButton>("%ShovelButton");
		_packetBank = GetNode<Control>("%LevelEditorPacketBank");
		_transformNode = GetNode<Control>("TransformNode");
		_mapControl = GetNode<TowerDefenseMapControl>("%TowerDefenseMapControl");
		if (!isolatedPreviewMode)
		{
			VisibilityChanged += Save;
		}
		VisibilityChanged += OnChildPacketBankVisibilityChanged;
		_shovelButton.Pressed += ShovelButtonPressed;
		mapFeature = (isolatedPreviewMode ? null : TowerDefenseManager.GetMapFeature());
		if (!GodotObject.IsInstanceValid(mapFeature) && GodotObject.IsInstanceValid(_mapControl))
		{
			mapFeature = new TowerDefenseBattleFeatureMap();
			mapFeature.mapControl = _mapControl;
			_mapControl.mapFeature = mapFeature;
			_ownsMapFeature = true;
		}
		if (!GodotObject.IsInstanceValid(mapFeature) || !GodotObject.IsInstanceValid(mapFeature.mapControl))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(mapFeature.packetPickControl))
		{
			mapFeature.packetPickControl = new PacketPickControl();
			mapFeature.packetPickControl.Init(mapFeature.mapControl, mapFeature);
			mapFeature.mapControl.AddChild(mapFeature.packetPickControl, forceReadableName: false, InternalMode.Disabled);
			_ownedPacketPickControl = mapFeature.packetPickControl;
		}
		if (!GodotObject.IsInstanceValid(mapFeature.shovelManager))
		{
			ShovelManager shovelManager = new ShovelManager();
			shovelManager.mapShovelSprite = new Sprite2D();
			shovelManager.mapShovelSprite.Visible = false;
			if (GodotObject.IsInstanceValid(ResourceManager.Instance) && ResourceManager.Instance.SHOVELS.TryGetValue("ShovelDefault", out var value))
			{
				shovelManager.shovelConfig = value as ShovelConfig;
			}
			if (GodotObject.IsInstanceValid(shovelManager.shovelConfig))
			{
				shovelManager.mapShovelSprite.Texture = shovelManager.shovelConfig.texture;
			}
			shovelManager.shovelButton = _shovelButton;
			shovelManager.Init(mapFeature.mapControl, mapFeature);
			mapFeature.shovelManager = shovelManager;
			ShovelPickTool shovelPickTool = new ShovelPickTool();
			shovelPickTool.Init(mapFeature.mapControl);
			shovelPickTool.SetShovelManager(shovelManager);
			mapFeature.packetPickControl.RegisterTool(shovelPickTool);
			_ownedShovelManager = shovelManager;
			_ownedShovelPickTool = shovelPickTool;
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!Global.Instance.isMobile || (!(inputEvent is InputEventMouseButton) && !(inputEvent is InputEventMouseMotion)))
		{
			if (inputEvent is InputEventMouseButton mouseButton)
			{
				HandleMousePanButton(mouseButton);
				HandleMouseWheelInput(mouseButton);
			}
			else if (inputEvent is InputEventMouseMotion mouseMotion)
			{
				HandleMousePanMotion(mouseMotion);
			}
			else if (inputEvent is InputEventScreenTouch || inputEvent is InputEventScreenDrag)
			{
				HandleTouchInput(inputEvent);
			}
		}
	}

	public override void _ExitTree()
	{
		ResetMousePan();
		ResetTouchGesture();
		if (!isolatedPreviewMode)
		{
			VisibilityChanged -= Save;
		}
		VisibilityChanged -= OnChildPacketBankVisibilityChanged;
		if (GodotObject.IsInstanceValid(_shovelButton))
		{
			_shovelButton.Pressed -= ShovelButtonPressed;
		}
		PacketPickControl packetPickControl = (GodotObject.IsInstanceValid(mapFeature) ? mapFeature.packetPickControl : _ownedPacketPickControl);
		if (GodotObject.IsInstanceValid(_ownedShovelPickTool))
		{
			if (GodotObject.IsInstanceValid(packetPickControl))
			{
				packetPickControl.UnregisterTool(_ownedShovelPickTool);
			}
			_ownedShovelPickTool.Free();
			_ownedShovelPickTool = null;
		}
		if (GodotObject.IsInstanceValid(_ownedShovelManager))
		{
			if (GodotObject.IsInstanceValid(_ownedShovelManager.mapShovelSprite))
			{
				_ownedShovelManager.mapShovelSprite.QueueFree();
			}
			_ownedShovelManager.mapShovelSprite = null;
			_ownedShovelManager.mapControl = null;
			_ownedShovelManager.mapFeature = null;
			_ownedShovelManager.Free();
			_ownedShovelManager = null;
		}
		if (GodotObject.IsInstanceValid(_ownedPacketPickControl))
		{
			_ownedPacketPickControl.DisposeBattleState();
		}
		if (_ownsMapFeature && GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.packetPickControl = null;
			mapFeature.shovelManager = null;
			mapFeature.mapControl = null;
			if (GodotObject.IsInstanceValid(_mapControl))
			{
				_mapControl.mapFeature = null;
			}
		}
		_ownedPacketPickControl = null;
		_previewMapConfig = null;
		_mapViewport = null;
		_packetBank = null;
		_transformNode = null;
		mapFeature = null;
		if (instance == this)
		{
			instance = null;
		}
		base._ExitTree();
	}

	public void ApplyMapPreviewConfig(TowerDefenseMapConfig mapConfig)
	{
		if (!GodotObject.IsInstanceValid(mapConfig) || !GodotObject.IsInstanceValid(_mapViewport) || !GodotObject.IsInstanceValid(_mapControl))
		{
			return;
		}
		_previewMapConfig = mapConfig;
		if (GodotObject.IsInstanceValid(_mapControl.editorSprite))
		{
			Texture2D texture = _mapControl.editorSprite.Texture;
			if (GodotObject.IsInstanceValid(texture))
			{
				_mapControl.editorSprite.Scale = new Vector2((texture.GetWidth() > 0) ? (mapConfig.mapSize.X / (float)texture.GetWidth()) : 1f, (texture.GetHeight() > 0) ? (mapConfig.mapSize.Y / (float)texture.GetHeight()) : 1f);
			}
			else
			{
				_mapControl.editorSprite.Scale = Vector2.One;
			}
		}
		float a = ((mapConfig.mapSize.X > 0f) ? (MapPreviewLogicalSize.X / mapConfig.mapSize.X) : 1f);
		float b = ((mapConfig.mapSize.Y > 0f) ? (MapPreviewLogicalSize.Y / mapConfig.mapSize.Y) : 1f);
		_minimumMapPreviewZoom = Mathf.Clamp(Mathf.Min(1f, Mathf.Min(a, b)), 0.05f, 1f);
		_mapViewport.Scale = Vector2.One * _minimumMapPreviewZoom;
		CenterMapPreview();
		ResetMousePan();
		ResetTouchGesture();
		RefreshMapGridMetrics();
	}

	private void HandleMousePanButton(InputEventMouseButton mouseButton)
	{
		if (!mouseButton.Pressed && _mousePanActive && mouseButton.ButtonIndex == _mousePanButton)
		{
			ResetMousePan();
			GetViewport().SetInputAsHandled();
		}
		else if (mouseButton.Pressed && CanProcessMapPreviewInput(mouseButton.Position) && IsMousePanButtonAvailable(mouseButton.ButtonIndex))
		{
			_mousePanActive = true;
			_mousePanButton = mouseButton.ButtonIndex;
			_previousMousePanPosition = mouseButton.Position;
			GetViewport().SetInputAsHandled();
		}
	}

	private void HandleMousePanMotion(InputEventMouseMotion mouseMotion)
	{
		if (_mousePanActive && CanProcessMapPreviewInput())
		{
			Transform2D globalTransformWithCanvas = _transformNode.GetGlobalTransformWithCanvas();
			Vector2 vector = globalTransformWithCanvas.AffineInverse() * _previousMousePanPosition;
			Vector2 vector2 = globalTransformWithCanvas.AffineInverse() * mouseMotion.Position;
			_mapViewport.Position += vector2 - vector;
			_previousMousePanPosition = mouseMotion.Position;
			ClampMapPreviewPosition();
			RefreshMapGridMetrics();
			GetViewport().SetInputAsHandled();
		}
	}

	private bool IsMousePanButtonAvailable(MouseButton button)
	{
		switch (button)
		{
		case MouseButton.Right:
		case MouseButton.Middle:
			return true;
		default:
			return false;
		case MouseButton.Left:
			if (GodotObject.IsInstanceValid(mapFeature?.packetPickControl))
			{
				return !mapFeature.packetPickControl.IsPicking();
			}
			return true;
		}
	}

	private void ResetMousePan()
	{
		_mousePanActive = false;
		_mousePanButton = MouseButton.None;
		_previousMousePanPosition = Vector2.Zero;
	}

	private void HandleMouseWheelInput(InputEventMouseButton mouseButton)
	{
		if (CanProcessMapPreviewInput(mouseButton.Position) && mouseButton.Pressed && (mouseButton.ButtonIndex == MouseButton.WheelUp || mouseButton.ButtonIndex == MouseButton.WheelDown))
		{
			float y = Mathf.Max(1f, mouseButton.Factor);
			float x = ((mouseButton.ButtonIndex == MouseButton.WheelUp) ? 1.1f : 0.9090909f);
			ApplyMapPreviewZoomAtScreenAnchor(MapPreviewZoom * Mathf.Pow(x, y), mouseButton.Position, mouseButton.Position);
			GetViewport().SetInputAsHandled();
		}
	}

	private void HandleTouchInput(InputEvent inputEvent)
	{
		if (!CanProcessMapPreviewInput())
		{
			ResetTouchGesture();
			return;
		}
		if (inputEvent is InputEventScreenTouch inputEventScreenTouch)
		{
			if (inputEventScreenTouch.Pressed)
			{
				if (CanProcessMapPreviewInput(inputEventScreenTouch.Position))
				{
					_touchPositions[inputEventScreenTouch.Index] = inputEventScreenTouch.Position;
				}
			}
			else
			{
				_touchPositions.Remove(inputEventScreenTouch.Index);
				if (_touchPositions.Count == 0 && _suppressTouchConfirm)
				{
					_suppressTouchConfirm = false;
					_suppressTouchConfirmThroughPhysicsFrame = Engine.GetPhysicsFrames() + 1;
				}
			}
			if (_touchPositions.Count == 2)
			{
				_suppressTouchConfirm = true;
			}
			RefreshPinchBaseline();
			return;
		}
		InputEventScreenDrag inputEventScreenDrag = (InputEventScreenDrag)inputEvent;
		if (!_touchPositions.ContainsKey(inputEventScreenDrag.Index))
		{
			return;
		}
		_touchPositions[inputEventScreenDrag.Index] = inputEventScreenDrag.Position;
		if (_touchPositions.Count != 2)
		{
			RefreshPinchBaseline();
			return;
		}
		_suppressTouchConfirm = true;
		GetPinchGeometry(out var distance, out var center);
		if (!_pinchActive || _previousPinchDistance < 12f || distance < 12f)
		{
			_previousPinchDistance = distance;
			_previousPinchCenter = center;
			_pinchActive = distance >= 12f;
		}
		else
		{
			ApplyMapPreviewZoomAtScreenAnchor(MapPreviewZoom * distance / _previousPinchDistance, center, _previousPinchCenter);
			_previousPinchDistance = distance;
			_previousPinchCenter = center;
			GetViewport().SetInputAsHandled();
		}
	}

	private bool CanProcessMapPreviewInput()
	{
		if (IsVisibleInTree() && GodotObject.IsInstanceValid(_previewMapConfig) && GodotObject.IsInstanceValid(_mapViewport))
		{
			return GodotObject.IsInstanceValid(_transformNode);
		}
		return false;
	}

	private bool CanProcessMapPreviewInput(Vector2 screenPosition)
	{
		if (CanProcessMapPreviewInput() && IsScreenPointInsideMapPreview(screenPosition))
		{
			return !IsScreenPointOverEditorControls(screenPosition);
		}
		return false;
	}

	private bool IsScreenPointInsideMapPreview(Vector2 screenPosition)
	{
		Vector2 point = _transformNode.GetGlobalTransformWithCanvas().AffineInverse() * screenPosition;
		return GetMapPreviewInputRect().HasPoint(point);
	}

	private Rect2 GetMapPreviewInputRect()
	{
		Rect2 result = new Rect2(Vector2.Zero, MapPreviewLogicalSize);
		if (!GodotObject.IsInstanceValid(_packetBank) || !_packetBank.IsVisibleInTree())
		{
			return result;
		}
		Vector2 vector = _transformNode.GetGlobalTransformWithCanvas().AffineInverse() * _packetBank.GetGlobalRect().Position;
		if (vector.X > result.Position.X && vector.X < result.End.X)
		{
			result.Size = new Vector2(vector.X - result.Position.X, result.Size.Y);
		}
		return result;
	}

	private bool IsScreenPointOverEditorControls(Vector2 screenPosition)
	{
		if ((!GodotObject.IsInstanceValid(_packetBank) || !_packetBank.GetGlobalRect().HasPoint(screenPosition)) && (!GodotObject.IsInstanceValid(_shovelButton) || !_shovelButton.GetGlobalRect().HasPoint(screenPosition)))
		{
			return IsPointerOverEditorControls(screenPosition);
		}
		return true;
	}

	private bool IsPointerOverEditorControls(Vector2 screenPosition)
	{
		Control control = GetViewport()?.GuiGetHoveredControl();
		if (!GodotObject.IsInstanceValid(control) || !control.GetGlobalRect().HasPoint(screenPosition))
		{
			return false;
		}
		while (GodotObject.IsInstanceValid(control))
		{
			if (control == _packetBank || control == _shovelButton || control is BaseButton)
			{
				return true;
			}
			if (control == this)
			{
				return false;
			}
			control = control.GetParent() as Control;
		}
		return false;
	}

	private void RefreshPinchBaseline()
	{
		if (_touchPositions.Count != 2)
		{
			_pinchActive = false;
			_previousPinchDistance = 0f;
			_previousPinchCenter = Vector2.Zero;
		}
		else
		{
			GetPinchGeometry(out _previousPinchDistance, out _previousPinchCenter);
			_pinchActive = _previousPinchDistance >= 12f;
		}
	}

	private void GetPinchGeometry(out float distance, out Vector2 center)
	{
		using System.Collections.Generic.Dictionary<int, Vector2>.ValueCollection.Enumerator enumerator = _touchPositions.Values.GetEnumerator();
		enumerator.MoveNext();
		Vector2 current = enumerator.Current;
		enumerator.MoveNext();
		Vector2 current2 = enumerator.Current;
		distance = current.DistanceTo(current2);
		center = (current + current2) * 0.5f;
	}

	private void ApplyMapPreviewZoomAtScreenAnchor(float targetZoom, Vector2 currentScreenAnchor, Vector2 previousScreenAnchor)
	{
		float num = Mathf.Max(MapPreviewZoom, 0.001f);
		float num2 = Mathf.Clamp(targetZoom, _minimumMapPreviewZoom, 2f);
		Transform2D globalTransformWithCanvas = _transformNode.GetGlobalTransformWithCanvas();
		Vector2 vector = globalTransformWithCanvas.AffineInverse() * previousScreenAnchor;
		Vector2 vector2 = globalTransformWithCanvas.AffineInverse() * currentScreenAnchor;
		Vector2 vector3 = (vector - _mapViewport.Position) / num;
		_mapViewport.Scale = Vector2.One * num2;
		_mapViewport.Position = vector2 - vector3 * num2;
		ClampMapPreviewPosition();
		RefreshMapGridMetrics();
	}

	private void CenterMapPreview()
	{
		Vector2 mapPreviewOrigin = GetMapPreviewOrigin();
		Rect2 mapPreviewInputRect = GetMapPreviewInputRect();
		Vector2 vector = _previewMapConfig.mapSize * MapPreviewZoom;
		float x = ((vector.X > mapPreviewInputRect.Size.X) ? (mapPreviewInputRect.End.X - (mapPreviewOrigin.X + _previewMapConfig.mapSize.X) * MapPreviewZoom) : (mapPreviewInputRect.Position.X + (mapPreviewInputRect.Size.X - vector.X) * 0.5f - mapPreviewOrigin.X * MapPreviewZoom));
		float y = (MapPreviewLogicalSize.Y - vector.Y) * 0.5f - mapPreviewOrigin.Y * MapPreviewZoom;
		_mapViewport.Position = new Vector2(x, y);
		ClampMapPreviewPosition();
	}

	private void ClampMapPreviewPosition()
	{
		if (GodotObject.IsInstanceValid(_previewMapConfig) && GodotObject.IsInstanceValid(_mapViewport))
		{
			Vector2 mapPreviewOrigin = GetMapPreviewOrigin();
			Rect2 mapPreviewInputRect = GetMapPreviewInputRect();
			float mapPreviewZoom = MapPreviewZoom;
			_mapViewport.Position = new Vector2(ClampMapPreviewAxis(_mapViewport.Position.X, mapPreviewOrigin.X, _previewMapConfig.mapSize.X, mapPreviewInputRect.Position.X, mapPreviewInputRect.Size.X, mapPreviewZoom), ClampMapPreviewAxis(_mapViewport.Position.Y, mapPreviewOrigin.Y, _previewMapConfig.mapSize.Y, 0f, MapPreviewLogicalSize.Y, mapPreviewZoom));
		}
	}

	private static float ClampMapPreviewAxis(float position, float contentOrigin, float contentLength, float viewportStart, float viewportLength, float zoom)
	{
		float num = contentLength * zoom;
		if (num <= viewportLength)
		{
			return viewportStart + (viewportLength - num) * 0.5f - contentOrigin * zoom;
		}
		float min = viewportStart + viewportLength - (contentOrigin + contentLength) * zoom;
		float max = viewportStart - contentOrigin * zoom;
		return Mathf.Clamp(position, min, max);
	}

	private Vector2 GetMapPreviewOrigin()
	{
		return _mapControl.Position + _previewMapConfig.mapOffset;
	}

	private void RefreshMapGridMetrics()
	{
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.NotifyMapTransformChanged();
		}
	}

	private void ResetTouchGesture()
	{
		_touchPositions.Clear();
		_previousPinchDistance = 0f;
		_previousPinchCenter = Vector2.Zero;
		_pinchActive = false;
		_suppressTouchConfirm = false;
		_suppressTouchConfirmThroughPhysicsFrame = 0uL;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GodotObject.IsInstanceValid(mapFeature))
		{
			mapFeature.Process((float)delta);
		}
	}

	public void Init(TowerDefenseLevelConfig _levelConfig)
	{
		levelConfig = _levelConfig;
	}

	public void OnChildPacketBankVisibilityChanged()
	{
		if (GodotObject.IsInstanceValid(LevelEditorPacketBank.Instance))
		{
			LevelEditorPacketBank.Instance.RefreshWhenVisible();
		}
	}

	public void Save()
	{
		Save(isSave: false);
	}

	public void Save(bool isSave)
	{
		Release();
		if (!GodotObject.IsInstanceValid(levelConfig))
		{
			return;
		}
		bool flag = false;
		if (levelConfig.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.VASE)
		{
			flag = true;
		}
		if ((isSave && IsVisibleInTree()) || (!isSave && !IsVisibleInTree()))
		{
			levelConfig.preSpawnList.Clear();
			if (flag)
			{
				levelConfig.vaseManager.vaseList.Clear();
			}
			Array<TowerDefenseCharacter> array = new Array<TowerDefenseCharacter>();
			if (GodotObject.IsInstanceValid(mapFeature))
			{
				for (int i = 1; i < mapFeature.config.gridNum.X + 1; i++)
				{
					for (int j = 1; j < mapFeature.config.gridNum.Y + 1; j++)
					{
						foreach (TowerDefenseCharacter item4 in mapFeature.plantGrid[i][j].As<TowerDefenseCellInstance>().GetCharacterListSave())
						{
							if (!array.Contains(item4))
							{
								if (flag && item4 is TowerDefenseVase)
								{
									TowerDefenseLevelVaseConfig item = ((TowerDefenseVase)item4).Export();
									levelConfig.vaseManager.vaseList.Add(item);
									array.Add(item4);
								}
								else
								{
									TowerDefenseLevelPreSpawnConfig item2 = CreatePreSpawnConfig(item4, new Vector2I(i, j));
									levelConfig.preSpawnList.Add(item2);
									array.Add(item4);
								}
							}
						}
					}
				}
			}
			foreach (Variant item5 in TowerDefenseManager.Instance.GetCharacter())
			{
				TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item5;
				if (!array.Contains(towerDefenseCharacter))
				{
					TowerDefenseLevelPreSpawnConfig item3 = CreatePreSpawnConfig(towerDefenseCharacter, towerDefenseCharacter.gridPos);
					levelConfig.preSpawnList.Add(item3);
					array.Add(towerDefenseCharacter);
				}
			}
			if (levelConfig.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.WAVE)
			{
				levelConfig.vaseManager = null;
			}
			levelConfig.MarkPreSpawnDataEditedFromEditor();
			if (!isSave && !IsVisibleInTree())
			{
				ClearCharacter();
			}
		}
		else
		{
			if (!IsVisibleInTree())
			{
				return;
			}
			if (GodotObject.IsInstanceValid(mapFeature))
			{
				for (int k = 1; k < mapFeature.config.gridNum.X + 1; k++)
				{
					for (int l = 1; l < mapFeature.config.gridNum.Y + 1; l++)
					{
						mapFeature.plantGrid[k][l].As<TowerDefenseCellInstance>().Clear();
					}
				}
			}
			foreach (TowerDefenseLevelPreSpawnConfig preSpawn in levelConfig.preSpawnList)
			{
				TowerDefenseCharacter character = preSpawn.SpawnCharacter(default, editorPreviewMode: true);
				if (GodotObject.IsInstanceValid(character) && GodotObject.IsInstanceValid(preSpawn.characterOverride))
				{
					preSpawn.characterOverride.ExecuteCharacter(character);
				}
			}
			if (!GodotObject.IsInstanceValid(levelConfig.vaseManager))
			{
				return;
			}
			foreach (TowerDefenseLevelVaseConfig vase in levelConfig.vaseManager.vaseList)
			{
				TowerDefensePacketConfig towerDefensePacketConfig = null;
				string type = vase.type;
				if (type == "Plant")
				{
					towerDefensePacketConfig = TowerDefenseManager.GetPacketConfig("VasePlant");
				}
				else
				{
					towerDefensePacketConfig = ((!(type == "Zombie")) ? TowerDefenseManager.GetPacketConfig("VaseNormal") : TowerDefenseManager.GetPacketConfig("VaseZombie"));
				}
				TowerDefenseCharacter towerDefenseCharacter2 = towerDefensePacketConfig.Plant(vase.gridPos, playAudio: true, noLimit: false, default, skipPlacementCheck: false, editorPreviewMode: true);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter2) && vase.packetName != "")
				{
					((TowerDefenseVase)towerDefenseCharacter2).packetConfig = vase.GetPacket();
				}
			}
		}
	}

	private static TowerDefenseLevelPreSpawnConfig CreatePreSpawnConfig(TowerDefenseCharacter character, Vector2I gridPos)
	{
		TowerDefenseLevelPreSpawnConfig towerDefenseLevelPreSpawnConfig = new TowerDefenseLevelPreSpawnConfig
		{
			gridPos = gridPos,
			packetName = character.packet.saveKey
		};
		if (character is TowerDefenseVase towerDefenseVase && GodotObject.IsInstanceValid(towerDefenseVase.packetConfig))
		{
			AddCharacterOverrideProperty(towerDefenseLevelPreSpawnConfig, "packetName", Variant.From(in towerDefenseVase.packetConfig.saveKey));
		}
		if (character is TowerDefenseItemSheild towerDefenseItemSheild)
		{
			AddCharacterOverrideProperty(towerDefenseLevelPreSpawnConfig, "shieldType", Variant.From<StringName>(towerDefenseItemSheild.shieldType));
			double from = towerDefenseItemSheild.shieldHitpoints;
			if (GodotObject.IsInstanceValid(towerDefenseItemSheild.instance))
			{
				from = towerDefenseItemSheild.instance.hitpoints;
			}
			AddCharacterOverrideProperty(towerDefenseLevelPreSpawnConfig, "shieldHitpoints", Variant.From(in from));
		}
		return towerDefenseLevelPreSpawnConfig;
	}

	private static void AddCharacterOverrideProperty(TowerDefenseLevelPreSpawnConfig preSpawnConfig, string propertyName, Variant value)
	{
		if (!GodotObject.IsInstanceValid(preSpawnConfig.characterOverride))
		{
			preSpawnConfig.characterOverride = new TowerDefenseCharacterOverride();
		}
		TowerDefenseCharacterOverride characterOverride = preSpawnConfig.characterOverride;
		if (characterOverride.propertyChange == null)
		{
			characterOverride.propertyChange = new Array<TowerDefenseCharacterPropertyChangeConfig>();
		}
		TowerDefenseCharacterPropertyChangeConfig item = new TowerDefenseCharacterPropertyChangeConfig
		{
			propertyName = propertyName,
			value = value
		};
		preSpawnConfig.characterOverride.propertyChange.Add(item);
	}

	public void Clear()
	{
		levelConfig = null;
		ClearCharacter();
	}

	public void ClearCharacter()
	{
		foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.skipDestroySet = true;
				towerDefenseCharacter.Destroy();
			}
		}
	}

	public void ShovelButtonPressed()
	{
		if (mapFeature == null)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(mapFeature.packetPickControl))
		{
			mapFeature.packetPickControl.PacketPickRelease();
		}
		if (_shovelButton.ButtonPressed)
		{
			AudioManager.Instance.AudioPlay("Shovel");
			if (GodotObject.IsInstanceValid(mapFeature.shovelManager))
			{
				mapFeature.shovelManager.shovelPick = true;
				mapFeature.shovelManager.mapShovelSprite.Position = new Vector2(-100f, -100f);
				mapFeature.shovelManager.mapShovelSprite.Visible = true;
			}
		}
		else
		{
			AudioManager.Instance.AudioPlay("ShovelDeny");
			if (GodotObject.IsInstanceValid(mapFeature.shovelManager))
			{
				mapFeature.shovelManager.shovelPick = false;
				mapFeature.shovelManager.mapShovelSprite.Visible = false;
			}
		}
	}

	public void Release()
	{
		if (mapFeature != null)
		{
			if (GodotObject.IsInstanceValid(mapFeature.packetPickControl))
			{
				mapFeature.packetPickControl.Release();
			}
			_shovelButton.ButtonPressed = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(35)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMapPreviewConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleMousePanButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mouseButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleMousePanMotion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mouseMotion", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseMotion"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsMousePanButtonAvailable, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "button", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetMousePan, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleMouseWheelInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mouseButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleTouchInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanProcessMapPreviewInput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanProcessMapPreviewInput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsScreenPointInsideMapPreview, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapPreviewInputRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsScreenPointOverEditorControls, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPointerOverEditorControls, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPinchBaseline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyMapPreviewZoomAtScreenAnchor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "targetZoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "currentScreenAnchor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "previousScreenAnchor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CenterMapPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampMapPreviewPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampMapPreviewAxis, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "contentOrigin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "contentLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "viewportStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "viewportLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "zoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapPreviewOrigin, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshMapGridMetrics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetTouchGesture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnChildPacketBankVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isSave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePreSpawnConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddCharacterOverrideProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preSpawnConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShovelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Release, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMapPreviewConfig && args.Count == 1)
		{
			ApplyMapPreviewConfig(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleMousePanButton && args.Count == 1)
		{
			HandleMousePanButton(VariantUtils.ConvertTo<InputEventMouseButton>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleMousePanMotion && args.Count == 1)
		{
			HandleMousePanMotion(VariantUtils.ConvertTo<InputEventMouseMotion>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMousePanButtonAvailable && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMousePanButtonAvailable(VariantUtils.ConvertTo<MouseButton>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetMousePan && args.Count == 0)
		{
			ResetMousePan();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleMouseWheelInput && args.Count == 1)
		{
			HandleMouseWheelInput(VariantUtils.ConvertTo<InputEventMouseButton>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleTouchInput && args.Count == 1)
		{
			HandleTouchInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanProcessMapPreviewInput && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanProcessMapPreviewInput());
			return true;
		}
		if (method == MethodName.CanProcessMapPreviewInput && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanProcessMapPreviewInput(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.IsScreenPointInsideMapPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsScreenPointInsideMapPreview(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMapPreviewInputRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetMapPreviewInputRect());
			return true;
		}
		if (method == MethodName.IsScreenPointOverEditorControls && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsScreenPointOverEditorControls(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.IsPointerOverEditorControls && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPointerOverEditorControls(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshPinchBaseline && args.Count == 0)
		{
			RefreshPinchBaseline();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyMapPreviewZoomAtScreenAnchor && args.Count == 3)
		{
			ApplyMapPreviewZoomAtScreenAnchor(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CenterMapPreview && args.Count == 0)
		{
			CenterMapPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ClampMapPreviewPosition && args.Count == 0)
		{
			ClampMapPreviewPosition();
			ret = default;
			return true;
		}
		if (method == MethodName.ClampMapPreviewAxis && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<float>(ClampMapPreviewAxis(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]), VariantUtils.ConvertTo<float>(in args[5])));
			return true;
		}
		if (method == MethodName.GetMapPreviewOrigin && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapPreviewOrigin());
			return true;
		}
		if (method == MethodName.RefreshMapGridMetrics && args.Count == 0)
		{
			RefreshMapGridMetrics();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetTouchGesture && args.Count == 0)
		{
			ResetTouchGesture();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnChildPacketBankVisibilityChanged && args.Count == 0)
		{
			OnChildPacketBankVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 1)
		{
			Save(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePreSpawnConfig && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelPreSpawnConfig>(CreatePreSpawnConfig(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.AddCharacterOverrideProperty && args.Count == 3)
		{
			AddCharacterOverrideProperty(VariantUtils.ConvertTo<TowerDefenseLevelPreSpawnConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCharacter && args.Count == 0)
		{
			ClearCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.ShovelButtonPressed && args.Count == 0)
		{
			ShovelButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.Release && args.Count == 0)
		{
			Release();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClampMapPreviewAxis && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<float>(ClampMapPreviewAxis(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]), VariantUtils.ConvertTo<float>(in args[5])));
			return true;
		}
		if (method == MethodName.CreatePreSpawnConfig && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelPreSpawnConfig>(CreatePreSpawnConfig(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.AddCharacterOverrideProperty && args.Count == 3)
		{
			AddCharacterOverrideProperty(VariantUtils.ConvertTo<TowerDefenseLevelPreSpawnConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ApplyMapPreviewConfig)
		{
			return true;
		}
		if (method == MethodName.HandleMousePanButton)
		{
			return true;
		}
		if (method == MethodName.HandleMousePanMotion)
		{
			return true;
		}
		if (method == MethodName.IsMousePanButtonAvailable)
		{
			return true;
		}
		if (method == MethodName.ResetMousePan)
		{
			return true;
		}
		if (method == MethodName.HandleMouseWheelInput)
		{
			return true;
		}
		if (method == MethodName.HandleTouchInput)
		{
			return true;
		}
		if (method == MethodName.CanProcessMapPreviewInput)
		{
			return true;
		}
		if (method == MethodName.IsScreenPointInsideMapPreview)
		{
			return true;
		}
		if (method == MethodName.GetMapPreviewInputRect)
		{
			return true;
		}
		if (method == MethodName.IsScreenPointOverEditorControls)
		{
			return true;
		}
		if (method == MethodName.IsPointerOverEditorControls)
		{
			return true;
		}
		if (method == MethodName.RefreshPinchBaseline)
		{
			return true;
		}
		if (method == MethodName.ApplyMapPreviewZoomAtScreenAnchor)
		{
			return true;
		}
		if (method == MethodName.CenterMapPreview)
		{
			return true;
		}
		if (method == MethodName.ClampMapPreviewPosition)
		{
			return true;
		}
		if (method == MethodName.ClampMapPreviewAxis)
		{
			return true;
		}
		if (method == MethodName.GetMapPreviewOrigin)
		{
			return true;
		}
		if (method == MethodName.RefreshMapGridMetrics)
		{
			return true;
		}
		if (method == MethodName.ResetTouchGesture)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.OnChildPacketBankVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.CreatePreSpawnConfig)
		{
			return true;
		}
		if (method == MethodName.AddCharacterOverrideProperty)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.ClearCharacter)
		{
			return true;
		}
		if (method == MethodName.ShovelButtonPressed)
		{
			return true;
		}
		if (method == MethodName.Release)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._mapViewport)
		{
			_mapViewport = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._characterNode)
		{
			_characterNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._shovelButton)
		{
			_shovelButton = VariantUtils.ConvertTo<TextureButton>(in value);
			return true;
		}
		if (name == PropertyName._packetBank)
		{
			_packetBank = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._transformNode)
		{
			_transformNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			_mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName._previewMapConfig)
		{
			_previewMapConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName._minimumMapPreviewZoom)
		{
			_minimumMapPreviewZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._previousPinchDistance)
		{
			_previousPinchDistance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._previousPinchCenter)
		{
			_previousPinchCenter = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._pinchActive)
		{
			_pinchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._suppressTouchConfirm)
		{
			_suppressTouchConfirm = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._suppressTouchConfirmThroughPhysicsFrame)
		{
			_suppressTouchConfirmThroughPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._mousePanActive)
		{
			_mousePanActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mousePanButton)
		{
			_mousePanButton = VariantUtils.ConvertTo<MouseButton>(in value);
			return true;
		}
		if (name == PropertyName._previousMousePanPosition)
		{
			_previousMousePanPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._ownsMapFeature)
		{
			_ownsMapFeature = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ownedPacketPickControl)
		{
			_ownedPacketPickControl = VariantUtils.ConvertTo<PacketPickControl>(in value);
			return true;
		}
		if (name == PropertyName._ownedShovelManager)
		{
			_ownedShovelManager = VariantUtils.ConvertTo<ShovelManager>(in value);
			return true;
		}
		if (name == PropertyName._ownedShovelPickTool)
		{
			_ownedShovelPickTool = VariantUtils.ConvertTo<ShovelPickTool>(in value);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.isolatedPreviewMode)
		{
			isolatedPreviewMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName.shovelShow)
		{
			shovelShow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.characterNode)
		{
			value = VariantUtils.CreateFrom<Node2D>(characterNode);
			return true;
		}
		if (name == PropertyName.MapPreviewZoom)
		{
			value = VariantUtils.CreateFrom<float>(MapPreviewZoom);
			return true;
		}
		bool from;
		if (name == PropertyName.ShouldSuppressPlacementConfirm)
		{
			from = ShouldSuppressPlacementConfirm;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsMapPanActive)
		{
			from = IsMapPanActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._mapViewport)
		{
			value = VariantUtils.CreateFrom(in _mapViewport);
			return true;
		}
		if (name == PropertyName._characterNode)
		{
			value = VariantUtils.CreateFrom(in _characterNode);
			return true;
		}
		if (name == PropertyName._shovelButton)
		{
			value = VariantUtils.CreateFrom(in _shovelButton);
			return true;
		}
		if (name == PropertyName._packetBank)
		{
			value = VariantUtils.CreateFrom(in _packetBank);
			return true;
		}
		if (name == PropertyName._transformNode)
		{
			value = VariantUtils.CreateFrom(in _transformNode);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			value = VariantUtils.CreateFrom(in _mapControl);
			return true;
		}
		if (name == PropertyName._previewMapConfig)
		{
			value = VariantUtils.CreateFrom(in _previewMapConfig);
			return true;
		}
		if (name == PropertyName._minimumMapPreviewZoom)
		{
			value = VariantUtils.CreateFrom(in _minimumMapPreviewZoom);
			return true;
		}
		if (name == PropertyName._previousPinchDistance)
		{
			value = VariantUtils.CreateFrom(in _previousPinchDistance);
			return true;
		}
		if (name == PropertyName._previousPinchCenter)
		{
			value = VariantUtils.CreateFrom(in _previousPinchCenter);
			return true;
		}
		if (name == PropertyName._pinchActive)
		{
			value = VariantUtils.CreateFrom(in _pinchActive);
			return true;
		}
		if (name == PropertyName._suppressTouchConfirm)
		{
			value = VariantUtils.CreateFrom(in _suppressTouchConfirm);
			return true;
		}
		if (name == PropertyName._suppressTouchConfirmThroughPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _suppressTouchConfirmThroughPhysicsFrame);
			return true;
		}
		if (name == PropertyName._mousePanActive)
		{
			value = VariantUtils.CreateFrom(in _mousePanActive);
			return true;
		}
		if (name == PropertyName._mousePanButton)
		{
			value = VariantUtils.CreateFrom(in _mousePanButton);
			return true;
		}
		if (name == PropertyName._previousMousePanPosition)
		{
			value = VariantUtils.CreateFrom(in _previousMousePanPosition);
			return true;
		}
		if (name == PropertyName._ownsMapFeature)
		{
			value = VariantUtils.CreateFrom(in _ownsMapFeature);
			return true;
		}
		if (name == PropertyName._ownedPacketPickControl)
		{
			value = VariantUtils.CreateFrom(in _ownedPacketPickControl);
			return true;
		}
		if (name == PropertyName._ownedShovelManager)
		{
			value = VariantUtils.CreateFrom(in _ownedShovelManager);
			return true;
		}
		if (name == PropertyName._ownedShovelPickTool)
		{
			value = VariantUtils.CreateFrom(in _ownedShovelPickTool);
			return true;
		}
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom(in levelConfig);
			return true;
		}
		if (name == PropertyName.isolatedPreviewMode)
		{
			value = VariantUtils.CreateFrom(in isolatedPreviewMode);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName.shovelShow)
		{
			value = VariantUtils.CreateFrom(in shovelShow);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._mapViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shovelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._transformNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewMapConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._minimumMapPreviewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previousPinchDistance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousPinchCenter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pinchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suppressTouchConfirm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._suppressTouchConfirmThroughPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mousePanActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mousePanButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousMousePanPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ownsMapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ownedPacketPickControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ownedShovelManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ownedShovelPickTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.ResourceType, "TowerDefenseLevelConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isolatedPreviewMode, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shovelShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.MapPreviewZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ShouldSuppressPlacementConfirm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsMapPanActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._mapViewport, Variant.From(in _mapViewport));
		info.AddProperty(PropertyName._characterNode, Variant.From(in _characterNode));
		info.AddProperty(PropertyName._shovelButton, Variant.From(in _shovelButton));
		info.AddProperty(PropertyName._packetBank, Variant.From(in _packetBank));
		info.AddProperty(PropertyName._transformNode, Variant.From(in _transformNode));
		info.AddProperty(PropertyName._mapControl, Variant.From(in _mapControl));
		info.AddProperty(PropertyName._previewMapConfig, Variant.From(in _previewMapConfig));
		info.AddProperty(PropertyName._minimumMapPreviewZoom, Variant.From(in _minimumMapPreviewZoom));
		info.AddProperty(PropertyName._previousPinchDistance, Variant.From(in _previousPinchDistance));
		info.AddProperty(PropertyName._previousPinchCenter, Variant.From(in _previousPinchCenter));
		info.AddProperty(PropertyName._pinchActive, Variant.From(in _pinchActive));
		info.AddProperty(PropertyName._suppressTouchConfirm, Variant.From(in _suppressTouchConfirm));
		info.AddProperty(PropertyName._suppressTouchConfirmThroughPhysicsFrame, Variant.From(in _suppressTouchConfirmThroughPhysicsFrame));
		info.AddProperty(PropertyName._mousePanActive, Variant.From(in _mousePanActive));
		info.AddProperty(PropertyName._mousePanButton, Variant.From(in _mousePanButton));
		info.AddProperty(PropertyName._previousMousePanPosition, Variant.From(in _previousMousePanPosition));
		info.AddProperty(PropertyName._ownsMapFeature, Variant.From(in _ownsMapFeature));
		info.AddProperty(PropertyName._ownedPacketPickControl, Variant.From(in _ownedPacketPickControl));
		info.AddProperty(PropertyName._ownedShovelManager, Variant.From(in _ownedShovelManager));
		info.AddProperty(PropertyName._ownedShovelPickTool, Variant.From(in _ownedShovelPickTool));
		info.AddProperty(PropertyName.levelConfig, Variant.From(in levelConfig));
		info.AddProperty(PropertyName.isolatedPreviewMode, Variant.From(in isolatedPreviewMode));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName.shovelShow, Variant.From(in shovelShow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._mapViewport, out var value))
		{
			_mapViewport = value.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._characterNode, out var value2))
		{
			_characterNode = value2.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._shovelButton, out var value3))
		{
			_shovelButton = value3.As<TextureButton>();
		}
		if (info.TryGetProperty(PropertyName._packetBank, out var value4))
		{
			_packetBank = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._transformNode, out var value5))
		{
			_transformNode = value5.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value6))
		{
			_mapControl = value6.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._previewMapConfig, out var value7))
		{
			_previewMapConfig = value7.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName._minimumMapPreviewZoom, out var value8))
		{
			_minimumMapPreviewZoom = value8.As<float>();
		}
		if (info.TryGetProperty(PropertyName._previousPinchDistance, out var value9))
		{
			_previousPinchDistance = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName._previousPinchCenter, out var value10))
		{
			_previousPinchCenter = value10.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._pinchActive, out var value11))
		{
			_pinchActive = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._suppressTouchConfirm, out var value12))
		{
			_suppressTouchConfirm = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._suppressTouchConfirmThroughPhysicsFrame, out var value13))
		{
			_suppressTouchConfirmThroughPhysicsFrame = value13.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._mousePanActive, out var value14))
		{
			_mousePanActive = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mousePanButton, out var value15))
		{
			_mousePanButton = value15.As<MouseButton>();
		}
		if (info.TryGetProperty(PropertyName._previousMousePanPosition, out var value16))
		{
			_previousMousePanPosition = value16.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._ownsMapFeature, out var value17))
		{
			_ownsMapFeature = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ownedPacketPickControl, out var value18))
		{
			_ownedPacketPickControl = value18.As<PacketPickControl>();
		}
		if (info.TryGetProperty(PropertyName._ownedShovelManager, out var value19))
		{
			_ownedShovelManager = value19.As<ShovelManager>();
		}
		if (info.TryGetProperty(PropertyName._ownedShovelPickTool, out var value20))
		{
			_ownedShovelPickTool = value20.As<ShovelPickTool>();
		}
		if (info.TryGetProperty(PropertyName.levelConfig, out var value21))
		{
			levelConfig = value21.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.isolatedPreviewMode, out var value22))
		{
			isolatedPreviewMode = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value23))
		{
			mapFeature = value23.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName.shovelShow, out var value24))
		{
			shovelShow = value24.As<bool>();
		}
	}
}
