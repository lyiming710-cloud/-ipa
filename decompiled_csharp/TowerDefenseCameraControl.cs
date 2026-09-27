using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Camera/Control/TowerDefenseCameraControl.cs")]
public class TowerDefenseCameraControl : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public static readonly StringName HandleMousePanButton = "HandleMousePanButton";

		public static readonly StringName HandleMousePanMotion = "HandleMousePanMotion";

		public static readonly StringName IsMousePanButtonAvailable = "IsMousePanButtonAvailable";

		public static readonly StringName IsPointerOverBattleInterface = "IsPointerOverBattleInterface";

		public static readonly StringName ResetMousePan = "ResetMousePan";

		public static readonly StringName HandleMouseWheelInput = "HandleMouseWheelInput";

		public static readonly StringName ApplyMapConfig = "ApplyMapConfig";

		public static readonly StringName OnViewportSizeChanged = "OnViewportSizeChanged";

		public static readonly StringName ApplySize = "ApplySize";

		public static readonly StringName CanProcessBattleZoomInput = "CanProcessBattleZoomInput";

		public static readonly StringName HandleTouchInput = "HandleTouchInput";

		public static readonly StringName RefreshPinchBaseline = "RefreshPinchBaseline";

		public static readonly StringName ApplyZoomAtScreenAnchor = "ApplyZoomAtScreenAnchor";

		public static readonly StringName ClampCameraToMap = "ClampCameraToMap";

		public static readonly StringName ClampCameraAxis = "ClampCameraAxis";

		public static readonly StringName ResetTouchGesture = "ResetTouchGesture";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName BattleZoomEnabled = "BattleZoomEnabled";

		public static readonly StringName IsMousePanActive = "IsMousePanActive";

		public static readonly StringName cameraBeginMarker = "cameraBeginMarker";

		public static readonly StringName cameraRightViewMarker = "cameraRightViewMarker";

		public static readonly StringName cameraPreViewMarker = "cameraPreViewMarker";

		public static readonly StringName camera = "camera";

		public static readonly StringName downRightMarker = "downRightMarker";

		public static readonly StringName size = "size";

		public static readonly StringName _viewport = "_viewport";

		public static readonly StringName _mapConfig = "_mapConfig";

		public static readonly StringName _battleZoomEnabled = "_battleZoomEnabled";

		public static readonly StringName _cameraStateInitialized = "_cameraStateInitialized";

		public static readonly StringName _previousPinchDistance = "_previousPinchDistance";

		public static readonly StringName _previousPinchCenter = "_previousPinchCenter";

		public static readonly StringName _pinchActive = "_pinchActive";

		public static readonly StringName _mousePanActive = "_mousePanActive";

		public static readonly StringName _publishedViewportCanvasTransform = "_publishedViewportCanvasTransform";

		public static readonly StringName _hasPublishedViewportCanvasTransform = "_hasPublishedViewportCanvasTransform";

		public static readonly StringName _mousePanButton = "_mousePanButton";

		public static readonly StringName _previousMousePanPosition = "_previousMousePanPosition";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const float MinimumBattleZoom = 0.4f;

	private const float MaximumBattleZoom = 2f;

	private const float MouseWheelZoomFactor = 1.12f;

	private const float MinimumPinchDistance = 4f;

	public Marker2D cameraBeginMarker;

	public Marker2D cameraRightViewMarker;

	public Marker2D cameraPreViewMarker;

	public Camera2D camera;

	public Marker2D downRightMarker;

	public Vector2 size;

	private Viewport _viewport;

	private TowerDefenseMapConfig _mapConfig;

	private bool _battleZoomEnabled;

	private bool _cameraStateInitialized;

	private readonly Dictionary<int, Vector2> _touchPositions = new Dictionary<int, Vector2>();

	private float _previousPinchDistance;

	private Vector2 _previousPinchCenter;

	private bool _pinchActive;

	private bool _mousePanActive;

	private Transform2D _publishedViewportCanvasTransform;

	private bool _hasPublishedViewportCanvasTransform;

	private MouseButton _mousePanButton;

	private Vector2 _previousMousePanPosition;

	public bool BattleZoomEnabled => _battleZoomEnabled;

	public bool IsMousePanActive => _mousePanActive;

	public override void _Ready()
	{
		cameraBeginMarker = GetNode<Marker2D>("%CameraBeginMarker");
		cameraRightViewMarker = GetNode<Marker2D>("%CameraRightViewMarker");
		cameraPreViewMarker = GetNode<Marker2D>("%CameraPreViewMarker");
		camera = GetNode<Camera2D>("%Camera");
		downRightMarker = GetNode<Marker2D>("%DownRightMarker");
		_viewport = GetViewport();
		if (GodotObject.IsInstanceValid(_viewport))
		{
			_viewport.SizeChanged += OnViewportSizeChanged;
		}
		if (GodotObject.IsInstanceValid(_mapConfig))
		{
			ApplyMapConfig(_mapConfig);
		}
		else
		{
			ApplySize();
		}
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_viewport))
		{
			_viewport.SizeChanged -= OnViewportSizeChanged;
		}
		_viewport = null;
		_hasPublishedViewportCanvasTransform = false;
		_cameraStateInitialized = false;
		ResetMousePan();
		ResetTouchGesture();
	}

	public override void _Process(double delta)
	{
		if (GodotObject.IsInstanceValid(_viewport))
		{
			Transform2D canvasTransform = _viewport.GetCanvasTransform();
			if (!_hasPublishedViewportCanvasTransform || !(_publishedViewportCanvasTransform == canvasTransform))
			{
				_publishedViewportCanvasTransform = canvasTransform;
				_hasPublishedViewportCanvasTransform = true;
				AdobeAnimateRuntimeManager.NotifyViewportTransformChanged();
			}
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventScreenTouch || inputEvent is InputEventScreenDrag)
		{
			if (!CanProcessBattleZoomInput())
			{
				ResetTouchGesture();
			}
			else
			{
				HandleTouchInput(inputEvent);
			}
		}
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		if (!Global.Instance.isMobile)
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
		}
	}

	private void HandleMousePanButton(InputEventMouseButton mouseButton)
	{
		if (!mouseButton.Pressed && _mousePanActive && mouseButton.ButtonIndex == _mousePanButton)
		{
			ResetMousePan();
			_viewport.SetInputAsHandled();
		}
		else if (mouseButton.Pressed && CanProcessBattleZoomInput() && !IsPointerOverBattleInterface(mouseButton.Position) && IsMousePanButtonAvailable(mouseButton.ButtonIndex))
		{
			_mousePanActive = true;
			_mousePanButton = mouseButton.ButtonIndex;
			_previousMousePanPosition = mouseButton.Position;
			_viewport.SetInputAsHandled();
		}
	}

	private void HandleMousePanMotion(InputEventMouseMotion mouseMotion)
	{
		if (_mousePanActive && CanProcessBattleZoomInput())
		{
			Vector2 vector = mouseMotion.Position - _previousMousePanPosition;
			camera.GlobalPosition -= vector / camera.Zoom;
			_previousMousePanPosition = mouseMotion.Position;
			ClampCameraToMap();
			_viewport.SetInputAsHandled();
		}
	}

	private static bool IsMousePanButtonAvailable(MouseButton button)
	{
		switch (button)
		{
		case MouseButton.Right:
		case MouseButton.Middle:
			return true;
		default:
			return false;
		case MouseButton.Left:
		{
			if (GodotObject.IsInstanceValid((TowerDefenseManager.Instance?.currentControl)?.GetFeature("GemMatch")))
			{
				return false;
			}
			TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
			if (GodotObject.IsInstanceValid(mapFeature?.packetPickControl))
			{
				return !mapFeature.packetPickControl.IsPicking();
			}
			return true;
		}
		}
	}

	private bool IsPointerOverBattleInterface(Vector2 screenPosition)
	{
		Control control = _viewport?.GuiGetHoveredControl();
		if (!GodotObject.IsInstanceValid(control) || !control.GetGlobalRect().HasPoint(screenPosition))
		{
			return false;
		}
		Control control2 = control;
		while (GodotObject.IsInstanceValid(control2))
		{
			bool flag = control2.MouseFilter != Control.MouseFilterEnum.Ignore;
			if (flag)
			{
				bool flag2 = ((control2 is BaseButton || control2 is Range || control2 is LineEdit || control2 is TextEdit || control2 is ItemList || control2 is Tree || control2 is TabBar || control2 is ScrollContainer) ? true : false);
				flag = flag2;
			}
			if (flag)
			{
				return true;
			}
			control2 = control2.GetParent() as Control;
		}
		return false;
	}

	private void ResetMousePan()
	{
		_mousePanActive = false;
		_mousePanButton = MouseButton.None;
		_previousMousePanPosition = Vector2.Zero;
	}

	private void HandleMouseWheelInput(InputEventMouseButton mouseButton)
	{
		if (CanProcessBattleZoomInput() && mouseButton.Pressed && !IsPointerOverBattleInterface(mouseButton.Position) && (mouseButton.ButtonIndex == MouseButton.WheelUp || mouseButton.ButtonIndex == MouseButton.WheelDown))
		{
			float y = Mathf.Max(1f, mouseButton.Factor);
			float x = ((mouseButton.ButtonIndex == MouseButton.WheelUp) ? 1.12f : (25f / 28f));
			float targetZoom = camera.Zoom.X * Mathf.Pow(x, y);
			ApplyZoomAtScreenAnchor(targetZoom, mouseButton.Position, mouseButton.Position);
			_viewport.SetInputAsHandled();
		}
	}

	public void ApplyMapConfig(TowerDefenseMapConfig mapConfig)
	{
		if (GodotObject.IsInstanceValid(mapConfig))
		{
			bool num = _mapConfig != mapConfig || !_cameraStateInitialized;
			_mapConfig = mapConfig;
			size = mapConfig.mapSize;
			_battleZoomEnabled = mapConfig.enableBattleZoom;
			ResetMousePan();
			ResetTouchGesture();
			if (num && GodotObject.IsInstanceValid(camera))
			{
				camera.Zoom = Vector2.One * (_battleZoomEnabled ? 0.4f : 1f);
				camera.GlobalPosition = Vector2.Zero;
				_cameraStateInitialized = true;
			}
			ApplySize();
			ClampCameraToMap();
		}
	}

	private void OnViewportSizeChanged()
	{
		ApplySize();
		ClampCameraToMap();
	}

	private void ApplySize()
	{
		if (GodotObject.IsInstanceValid(cameraBeginMarker) && GodotObject.IsInstanceValid(cameraRightViewMarker) && GodotObject.IsInstanceValid(cameraPreViewMarker) && GodotObject.IsInstanceValid(downRightMarker) && GodotObject.IsInstanceValid(camera) && GodotObject.IsInstanceValid(_viewport) && !(size.X <= 0f))
		{
			float num = _viewport.GetVisibleRect().Size.X / Mathf.Max(camera.Zoom.X, 0.001f);
			float num2 = ClampCameraAxis(0f, 0f, size.X, num);
			float num3 = ((num >= size.X) ? num2 : (size.X - num));
			float x = Mathf.Clamp(num3 - 56f, num2, num3);
			cameraBeginMarker.GlobalPosition = new Vector2(num2, cameraBeginMarker.GlobalPosition.Y);
			cameraRightViewMarker.GlobalPosition = new Vector2(num3, cameraRightViewMarker.GlobalPosition.Y);
			cameraPreViewMarker.GlobalPosition = new Vector2(x, cameraPreViewMarker.GlobalPosition.Y);
			downRightMarker.GlobalPosition = size;
		}
	}

	private bool CanProcessBattleZoomInput()
	{
		if (!_battleZoomEnabled || !GodotObject.IsInstanceValid(camera) || !GodotObject.IsInstanceValid(_viewport))
		{
			return false;
		}
		TowerDefenseControlNew towerDefenseControlNew = TowerDefenseManager.Instance?.currentControl;
		if (GodotObject.IsInstanceValid(towerDefenseControlNew) && towerDefenseControlNew.isGameRunning)
		{
			return !towerDefenseControlNew.isGameFail;
		}
		return false;
	}

	private void HandleTouchInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventScreenTouch inputEventScreenTouch)
		{
			if (inputEventScreenTouch.Pressed)
			{
				if (!IsPointerOverBattleInterface(inputEventScreenTouch.Position))
				{
					_touchPositions[inputEventScreenTouch.Index] = inputEventScreenTouch.Position;
				}
			}
			else
			{
				_touchPositions.Remove(inputEventScreenTouch.Index);
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
		GetPinchGeometry(out var distance, out var center);
		if (!_pinchActive || _previousPinchDistance < 4f || distance < 4f)
		{
			_previousPinchDistance = distance;
			_previousPinchCenter = center;
			_pinchActive = distance >= 4f;
		}
		else
		{
			float targetZoom = camera.Zoom.X * distance / _previousPinchDistance;
			ApplyZoomAtScreenAnchor(targetZoom, center, _previousPinchCenter);
			_previousPinchDistance = distance;
			_previousPinchCenter = center;
			_viewport.SetInputAsHandled();
		}
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
			_pinchActive = _previousPinchDistance >= 4f;
		}
	}

	private void GetPinchGeometry(out float distance, out Vector2 center)
	{
		using Dictionary<int, Vector2>.ValueCollection.Enumerator enumerator = _touchPositions.Values.GetEnumerator();
		enumerator.MoveNext();
		Vector2 current = enumerator.Current;
		enumerator.MoveNext();
		Vector2 current2 = enumerator.Current;
		distance = current.DistanceTo(current2);
		center = (current + current2) * 0.5f;
	}

	private void ApplyZoomAtScreenAnchor(float targetZoom, Vector2 currentScreenAnchor, Vector2 previousScreenAnchor)
	{
		float num = Mathf.Max(camera.Zoom.X, 0.001f);
		float num2 = Mathf.Clamp(targetZoom, 0.4f, 2f);
		Vector2 vector = camera.GlobalPosition + previousScreenAnchor / num;
		camera.Zoom = Vector2.One * num2;
		camera.GlobalPosition = vector - currentScreenAnchor / num2;
		ApplySize();
		ClampCameraToMap();
	}

	private void ClampCameraToMap()
	{
		if (GodotObject.IsInstanceValid(camera) && GodotObject.IsInstanceValid(_viewport) && !(size.X <= 0f) && !(size.Y <= 0f))
		{
			Vector2 vector = _viewport.GetVisibleRect().Size / camera.Zoom;
			camera.GlobalPosition = new Vector2(ClampCameraAxis(camera.GlobalPosition.X, 0f, size.X, vector.X), ClampCameraAxis(camera.GlobalPosition.Y, 0f, size.Y, vector.Y));
		}
	}

	private static float ClampCameraAxis(float position, float mapStart, float mapLength, float visibleLength)
	{
		if (visibleLength >= mapLength)
		{
			return mapStart + (mapLength - visibleLength) * 0.5f;
		}
		return Mathf.Clamp(position, mapStart, mapStart + mapLength - visibleLength);
	}

	private void ResetTouchGesture()
	{
		_touchPositions.Clear();
		_pinchActive = false;
		_previousPinchDistance = 0f;
		_previousPinchCenter = Vector2.Zero;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleMousePanButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mouseButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleMousePanMotion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mouseMotion", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseMotion"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsMousePanButtonAvailable, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "button", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPointerOverBattleInterface, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "screenPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetMousePan, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleMouseWheelInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mouseButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventMouseButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyMapConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnViewportSizeChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanProcessBattleZoomInput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleTouchInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPinchBaseline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyZoomAtScreenAnchor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "targetZoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "currentScreenAnchor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "previousScreenAnchor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClampCameraToMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClampCameraAxis, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "mapStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "mapLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "visibleLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetTouchGesture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
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
		if (method == MethodName.IsPointerOverBattleInterface && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPointerOverBattleInterface(VariantUtils.ConvertTo<Vector2>(in args[0])));
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
		if (method == MethodName.ApplyMapConfig && args.Count == 1)
		{
			ApplyMapConfig(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnViewportSizeChanged && args.Count == 0)
		{
			OnViewportSizeChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySize && args.Count == 0)
		{
			ApplySize();
			ret = default;
			return true;
		}
		if (method == MethodName.CanProcessBattleZoomInput && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanProcessBattleZoomInput());
			return true;
		}
		if (method == MethodName.HandleTouchInput && args.Count == 1)
		{
			HandleTouchInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPinchBaseline && args.Count == 0)
		{
			RefreshPinchBaseline();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyZoomAtScreenAnchor && args.Count == 3)
		{
			ApplyZoomAtScreenAnchor(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClampCameraToMap && args.Count == 0)
		{
			ClampCameraToMap();
			ret = default;
			return true;
		}
		if (method == MethodName.ClampCameraAxis && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<float>(ClampCameraAxis(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.ResetTouchGesture && args.Count == 0)
		{
			ResetTouchGesture();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsMousePanButtonAvailable && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMousePanButtonAvailable(VariantUtils.ConvertTo<MouseButton>(in args[0])));
			return true;
		}
		if (method == MethodName.ClampCameraAxis && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<float>(ClampCameraAxis(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
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
		if (method == MethodName.IsPointerOverBattleInterface)
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
		if (method == MethodName.ApplyMapConfig)
		{
			return true;
		}
		if (method == MethodName.OnViewportSizeChanged)
		{
			return true;
		}
		if (method == MethodName.ApplySize)
		{
			return true;
		}
		if (method == MethodName.CanProcessBattleZoomInput)
		{
			return true;
		}
		if (method == MethodName.HandleTouchInput)
		{
			return true;
		}
		if (method == MethodName.RefreshPinchBaseline)
		{
			return true;
		}
		if (method == MethodName.ApplyZoomAtScreenAnchor)
		{
			return true;
		}
		if (method == MethodName.ClampCameraToMap)
		{
			return true;
		}
		if (method == MethodName.ClampCameraAxis)
		{
			return true;
		}
		if (method == MethodName.ResetTouchGesture)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.cameraBeginMarker)
		{
			cameraBeginMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.cameraRightViewMarker)
		{
			cameraRightViewMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.cameraPreViewMarker)
		{
			cameraPreViewMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.camera)
		{
			camera = VariantUtils.ConvertTo<Camera2D>(in value);
			return true;
		}
		if (name == PropertyName.downRightMarker)
		{
			downRightMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.size)
		{
			size = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._viewport)
		{
			_viewport = VariantUtils.ConvertTo<Viewport>(in value);
			return true;
		}
		if (name == PropertyName._mapConfig)
		{
			_mapConfig = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName._battleZoomEnabled)
		{
			_battleZoomEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cameraStateInitialized)
		{
			_cameraStateInitialized = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._mousePanActive)
		{
			_mousePanActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._publishedViewportCanvasTransform)
		{
			_publishedViewportCanvasTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._hasPublishedViewportCanvasTransform)
		{
			_hasPublishedViewportCanvasTransform = VariantUtils.ConvertTo<bool>(in value);
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
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.BattleZoomEnabled)
		{
			from = BattleZoomEnabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsMousePanActive)
		{
			from = IsMousePanActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.cameraBeginMarker)
		{
			value = VariantUtils.CreateFrom(in cameraBeginMarker);
			return true;
		}
		if (name == PropertyName.cameraRightViewMarker)
		{
			value = VariantUtils.CreateFrom(in cameraRightViewMarker);
			return true;
		}
		if (name == PropertyName.cameraPreViewMarker)
		{
			value = VariantUtils.CreateFrom(in cameraPreViewMarker);
			return true;
		}
		if (name == PropertyName.camera)
		{
			value = VariantUtils.CreateFrom(in camera);
			return true;
		}
		if (name == PropertyName.downRightMarker)
		{
			value = VariantUtils.CreateFrom(in downRightMarker);
			return true;
		}
		if (name == PropertyName.size)
		{
			value = VariantUtils.CreateFrom(in size);
			return true;
		}
		if (name == PropertyName._viewport)
		{
			value = VariantUtils.CreateFrom(in _viewport);
			return true;
		}
		if (name == PropertyName._mapConfig)
		{
			value = VariantUtils.CreateFrom(in _mapConfig);
			return true;
		}
		if (name == PropertyName._battleZoomEnabled)
		{
			value = VariantUtils.CreateFrom(in _battleZoomEnabled);
			return true;
		}
		if (name == PropertyName._cameraStateInitialized)
		{
			value = VariantUtils.CreateFrom(in _cameraStateInitialized);
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
		if (name == PropertyName._mousePanActive)
		{
			value = VariantUtils.CreateFrom(in _mousePanActive);
			return true;
		}
		if (name == PropertyName._publishedViewportCanvasTransform)
		{
			value = VariantUtils.CreateFrom(in _publishedViewportCanvasTransform);
			return true;
		}
		if (name == PropertyName._hasPublishedViewportCanvasTransform)
		{
			value = VariantUtils.CreateFrom(in _hasPublishedViewportCanvasTransform);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.cameraBeginMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cameraRightViewMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.cameraPreViewMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.camera, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.downRightMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.size, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._battleZoomEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cameraStateInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previousPinchDistance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousPinchCenter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pinchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._mousePanActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._publishedViewportCanvasTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPublishedViewportCanvasTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mousePanButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousMousePanPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.BattleZoomEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsMousePanActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.cameraBeginMarker, Variant.From(in cameraBeginMarker));
		info.AddProperty(PropertyName.cameraRightViewMarker, Variant.From(in cameraRightViewMarker));
		info.AddProperty(PropertyName.cameraPreViewMarker, Variant.From(in cameraPreViewMarker));
		info.AddProperty(PropertyName.camera, Variant.From(in camera));
		info.AddProperty(PropertyName.downRightMarker, Variant.From(in downRightMarker));
		info.AddProperty(PropertyName.size, Variant.From(in size));
		info.AddProperty(PropertyName._viewport, Variant.From(in _viewport));
		info.AddProperty(PropertyName._mapConfig, Variant.From(in _mapConfig));
		info.AddProperty(PropertyName._battleZoomEnabled, Variant.From(in _battleZoomEnabled));
		info.AddProperty(PropertyName._cameraStateInitialized, Variant.From(in _cameraStateInitialized));
		info.AddProperty(PropertyName._previousPinchDistance, Variant.From(in _previousPinchDistance));
		info.AddProperty(PropertyName._previousPinchCenter, Variant.From(in _previousPinchCenter));
		info.AddProperty(PropertyName._pinchActive, Variant.From(in _pinchActive));
		info.AddProperty(PropertyName._mousePanActive, Variant.From(in _mousePanActive));
		info.AddProperty(PropertyName._publishedViewportCanvasTransform, Variant.From(in _publishedViewportCanvasTransform));
		info.AddProperty(PropertyName._hasPublishedViewportCanvasTransform, Variant.From(in _hasPublishedViewportCanvasTransform));
		info.AddProperty(PropertyName._mousePanButton, Variant.From(in _mousePanButton));
		info.AddProperty(PropertyName._previousMousePanPosition, Variant.From(in _previousMousePanPosition));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.cameraBeginMarker, out var value))
		{
			cameraBeginMarker = value.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.cameraRightViewMarker, out var value2))
		{
			cameraRightViewMarker = value2.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.cameraPreViewMarker, out var value3))
		{
			cameraPreViewMarker = value3.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.camera, out var value4))
		{
			camera = value4.As<Camera2D>();
		}
		if (info.TryGetProperty(PropertyName.downRightMarker, out var value5))
		{
			downRightMarker = value5.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.size, out var value6))
		{
			size = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._viewport, out var value7))
		{
			_viewport = value7.As<Viewport>();
		}
		if (info.TryGetProperty(PropertyName._mapConfig, out var value8))
		{
			_mapConfig = value8.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName._battleZoomEnabled, out var value9))
		{
			_battleZoomEnabled = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cameraStateInitialized, out var value10))
		{
			_cameraStateInitialized = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previousPinchDistance, out var value11))
		{
			_previousPinchDistance = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName._previousPinchCenter, out var value12))
		{
			_previousPinchCenter = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._pinchActive, out var value13))
		{
			_pinchActive = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mousePanActive, out var value14))
		{
			_mousePanActive = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._publishedViewportCanvasTransform, out var value15))
		{
			_publishedViewportCanvasTransform = value15.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._hasPublishedViewportCanvasTransform, out var value16))
		{
			_hasPublishedViewportCanvasTransform = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mousePanButton, out var value17))
		{
			_mousePanButton = value17.As<MouseButton>();
		}
		if (info.TryGetProperty(PropertyName._previousMousePanPosition, out var value18))
		{
			_previousMousePanPosition = value18.As<Vector2>();
		}
	}
}
