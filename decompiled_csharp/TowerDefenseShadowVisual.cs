using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Rendering/TowerDefenseShadowVisual.cs")]
public sealed class TowerDefenseShadowVisual : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName IsVisibleInTree = "IsVisibleInTree";

		public static readonly StringName IsInsideTree = "IsInsideTree";

		public static readonly StringName GetParent = "GetParent";

		public new static readonly StringName GetPath = "GetPath";

		public static readonly StringName GetEffectiveColor = "GetEffectiveColor";

		public static readonly StringName Capture = "Capture";

		public static readonly StringName Create = "Create";

		public static readonly StringName CanDetach = "CanDetach";

		public static readonly StringName CaptureAllState = "CaptureAllState";

		public static readonly StringName CaptureMutableState = "CaptureMutableState";

		public static readonly StringName GetOwnerGlobalTransform = "GetOwnerGlobalTransform";

		public static readonly StringName MarkChanged = "MarkChanged";

		public static readonly StringName Multiply = "Multiply";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName OwnerNode = "OwnerNode";

		public static readonly StringName LegacySprite = "LegacySprite";

		public static readonly StringName UsesLegacyNode = "UsesLegacyNode";

		public static readonly StringName RequiresLegacyRendering = "RequiresLegacyRendering";

		public static readonly StringName Revision = "Revision";

		public static readonly StringName Texture = "Texture";

		public static readonly StringName Position = "Position";

		public static readonly StringName GlobalPosition = "GlobalPosition";

		public static readonly StringName Scale = "Scale";

		public static readonly StringName Rotation = "Rotation";

		public static readonly StringName Skew = "Skew";

		public static readonly StringName Transform = "Transform";

		public static readonly StringName GlobalTransform = "GlobalTransform";

		public static readonly StringName Offset = "Offset";

		public static readonly StringName Centered = "Centered";

		public static readonly StringName FlipH = "FlipH";

		public static readonly StringName FlipV = "FlipV";

		public static readonly StringName Visible = "Visible";

		public static readonly StringName Modulate = "Modulate";

		public static readonly StringName SelfModulate = "SelfModulate";

		public static readonly StringName ZIndex = "ZIndex";

		public static readonly StringName ZAsRelative = "ZAsRelative";

		public static readonly StringName VisibilityLayer = "VisibilityLayer";

		public static readonly StringName TopLevel = "TopLevel";

		public static readonly StringName _owner = "_owner";

		public static readonly StringName _legacySprite = "_legacySprite";

		public static readonly StringName _requiresLegacyRendering = "_requiresLegacyRendering";

		public static readonly StringName _texture = "_texture";

		public static readonly StringName _position = "_position";

		public static readonly StringName _scale = "_scale";

		public static readonly StringName _rotation = "_rotation";

		public static readonly StringName _skew = "_skew";

		public static readonly StringName _offset = "_offset";

		public static readonly StringName _centered = "_centered";

		public static readonly StringName _flipH = "_flipH";

		public static readonly StringName _flipV = "_flipV";

		public static readonly StringName _visible = "_visible";

		public static readonly StringName _modulate = "_modulate";

		public static readonly StringName _selfModulate = "_selfModulate";

		public static readonly StringName _zIndex = "_zIndex";

		public static readonly StringName _zAsRelative = "_zAsRelative";

		public static readonly StringName _visibilityLayer = "_visibilityLayer";

		public static readonly StringName _topLevel = "_topLevel";

		public static readonly StringName _revision = "_revision";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private const string BuiltInCharacterShadowScript = "res://Prefab/TowerDefense/Character/TowerDefenseCharacterShadowSprite.cs";

	private Node2D _owner;

	private Sprite2D _legacySprite;

	private bool _requiresLegacyRendering;

	private Texture2D _texture;

	private Vector2 _position;

	private Vector2 _scale = Vector2.One;

	private float _rotation;

	private float _skew;

	private Vector2 _offset;

	private bool _centered = true;

	private bool _flipH;

	private bool _flipV;

	private bool _visible = true;

	private Color _modulate = Colors.White;

	private Color _selfModulate = Colors.White;

	private int _zIndex;

	private bool _zAsRelative = true;

	private uint _visibilityLayer = 1u;

	private bool _topLevel;

	private ulong _revision = 1uL;

	internal Node2D OwnerNode => _owner;

	internal Sprite2D LegacySprite => _legacySprite;

	internal bool UsesLegacyNode => _legacySprite != null;

	internal bool RequiresLegacyRendering => _requiresLegacyRendering;

	internal ulong Revision
	{
		get
		{
			if (!(_legacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return _revision;
			}
			return (_revision * 1099511628211L) ^ towerDefenseCharacterShadowSprite.LocalTransformRevision;
		}
	}

	public Texture2D Texture
	{
		get
		{
			return LegacySprite?.Texture ?? _texture;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Texture = value;
			}
			if (_texture != value)
			{
				_texture = value;
				MarkChanged();
			}
		}
	}

	public Vector2 Position
	{
		get
		{
			if (!(LegacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return LegacySprite?.Position ?? _position;
			}
			return towerDefenseCharacterShadowSprite.CachedLocalPose.Position;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Position = value;
			}
			if (!_position.IsEqualApprox(value))
			{
				_position = value;
				MarkChanged();
			}
		}
	}

	public Vector2 GlobalPosition
	{
		get
		{
			return LegacySprite?.GlobalPosition ?? (GetOwnerGlobalTransform() * _position);
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.GlobalPosition = value;
				CaptureMutableState(legacySprite);
				MarkChanged();
			}
			else
			{
				Position = GetOwnerGlobalTransform().AffineInverse() * value;
			}
		}
	}

	public Vector2 Scale
	{
		get
		{
			if (!(LegacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return LegacySprite?.Scale ?? _scale;
			}
			return towerDefenseCharacterShadowSprite.CachedLocalPose.Scale;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Scale = value;
			}
			if (!_scale.IsEqualApprox(value))
			{
				_scale = value;
				MarkChanged();
			}
		}
	}

	public float Rotation
	{
		get
		{
			if (!(LegacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return LegacySprite?.Rotation ?? _rotation;
			}
			return towerDefenseCharacterShadowSprite.CachedLocalPose.Rotation;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Rotation = value;
			}
			if (!Mathf.IsEqualApprox(_rotation, value))
			{
				_rotation = value;
				MarkChanged();
			}
		}
	}

	public float Skew
	{
		get
		{
			if (!(LegacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return LegacySprite?.Skew ?? _skew;
			}
			return towerDefenseCharacterShadowSprite.CachedLocalPose.Skew;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Skew = value;
			}
			if (!Mathf.IsEqualApprox(_skew, value))
			{
				_skew = value;
				MarkChanged();
			}
		}
	}

	public Transform2D Transform
	{
		get
		{
			if (!(LegacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return LegacySprite?.Transform ?? new Transform2D(_rotation, _scale, _skew, _position);
			}
			return towerDefenseCharacterShadowSprite.CachedLocalTransform;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Transform = value;
			}
			_position = value.Origin;
			_scale = value.Scale;
			_rotation = value.Rotation;
			_skew = value.Skew;
			MarkChanged();
		}
	}

	public Transform2D GlobalTransform
	{
		get
		{
			return LegacySprite?.GlobalTransform ?? (GetOwnerGlobalTransform() * Transform);
		}
		set
		{
			Transform = GetOwnerGlobalTransform().AffineInverse() * value;
		}
	}

	public Vector2 Offset
	{
		get
		{
			return LegacySprite?.Offset ?? _offset;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Offset = value;
			}
			if (!_offset.IsEqualApprox(value))
			{
				_offset = value;
				MarkChanged();
			}
		}
	}

	public bool Centered
	{
		get
		{
			return LegacySprite?.Centered ?? _centered;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Centered = value;
			}
			if (_centered != value)
			{
				_centered = value;
				MarkChanged();
			}
		}
	}

	public bool FlipH
	{
		get
		{
			return LegacySprite?.FlipH ?? _flipH;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.FlipH = value;
			}
			if (_flipH != value)
			{
				_flipH = value;
				MarkChanged();
			}
		}
	}

	public bool FlipV
	{
		get
		{
			return LegacySprite?.FlipV ?? _flipV;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.FlipV = value;
			}
			if (_flipV != value)
			{
				_flipV = value;
				MarkChanged();
			}
		}
	}

	public bool Visible
	{
		get
		{
			return LegacySprite?.Visible ?? _visible;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Visible = value;
			}
			if (_visible != value)
			{
				_visible = value;
				MarkChanged();
			}
		}
	}

	public Color Modulate
	{
		get
		{
			return LegacySprite?.Modulate ?? _modulate;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.Modulate = value;
			}
			if (!_modulate.IsEqualApprox(value))
			{
				_modulate = value;
				MarkChanged();
			}
		}
	}

	public Color SelfModulate
	{
		get
		{
			return LegacySprite?.SelfModulate ?? _selfModulate;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.SelfModulate = value;
			}
			if (!_selfModulate.IsEqualApprox(value))
			{
				_selfModulate = value;
				MarkChanged();
			}
		}
	}

	public int ZIndex
	{
		get
		{
			return LegacySprite?.ZIndex ?? _zIndex;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.ZIndex = value;
			}
			_zIndex = value;
		}
	}

	public bool ZAsRelative
	{
		get
		{
			return LegacySprite?.ZAsRelative ?? _zAsRelative;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.ZAsRelative = value;
			}
			_zAsRelative = value;
		}
	}

	public uint VisibilityLayer
	{
		get
		{
			return LegacySprite?.VisibilityLayer ?? _visibilityLayer;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.VisibilityLayer = value;
			}
			_visibilityLayer = value;
		}
	}

	public bool TopLevel
	{
		get
		{
			if (!(LegacySprite is TowerDefenseCharacterShadowSprite { LocalPoseReady: not false } towerDefenseCharacterShadowSprite))
			{
				return LegacySprite?.TopLevel ?? _topLevel;
			}
			return towerDefenseCharacterShadowSprite.CachedLocalPose.TopLevel;
		}
		set
		{
			Sprite2D legacySprite = LegacySprite;
			if (legacySprite != null)
			{
				legacySprite.TopLevel = value;
			}
			_topLevel = value;
			MarkChanged();
		}
	}

	public bool IsVisibleInTree()
	{
		Sprite2D legacySprite = LegacySprite;
		if (legacySprite == null)
		{
			if (_visible && GodotObject.IsInstanceValid(_owner))
			{
				return _owner.IsVisibleInTree();
			}
			return false;
		}
		return legacySprite.IsVisibleInTree();
	}

	public bool IsInsideTree()
	{
		return LegacySprite?.IsInsideTree() ?? false;
	}

	public Node GetParent()
	{
		return LegacySprite?.GetParent() ?? _owner;
	}

	public new NodePath GetPath()
	{
		return LegacySprite?.GetPath() ?? new NodePath("@ShadowData");
	}

	internal Color GetEffectiveColor()
	{
		Color color = Multiply(Modulate, SelfModulate);
		if (GodotObject.IsInstanceValid(_owner))
		{
			color = Multiply(color, Multiply(_owner.Modulate, _owner.SelfModulate));
		}
		return color;
	}

	internal static TowerDefenseShadowVisual Capture(Node2D owner, Sprite2D source, bool detachCompatibleNode = true)
	{
		if (!GodotObject.IsInstanceValid(source))
		{
			return null;
		}
		TowerDefenseShadowVisual towerDefenseShadowVisual = new TowerDefenseShadowVisual
		{
			_owner = owner
		};
		towerDefenseShadowVisual.CaptureAllState(source);
		bool flag = CanDetach(source);
		towerDefenseShadowVisual._requiresLegacyRendering = !flag;
		if (detachCompatibleNode & flag)
		{
			Node parent = source.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(source);
			}
			source.Free();
		}
		else
		{
			towerDefenseShadowVisual._legacySprite = source;
		}
		return towerDefenseShadowVisual;
	}

	internal static TowerDefenseShadowVisual Create(Node2D owner)
	{
		return new TowerDefenseShadowVisual
		{
			_owner = owner
		};
	}

	public static implicit operator TowerDefenseShadowVisual(Sprite2D sprite)
	{
		return Capture(sprite?.GetParent() as Node2D, sprite, detachCompatibleNode: false);
	}

	public static implicit operator Sprite2D(TowerDefenseShadowVisual visual)
	{
		return visual?.LegacySprite;
	}

	private static bool CanDetach(Sprite2D source)
	{
		if (source.GetChildCount() != 0)
		{
			return false;
		}
		if (source.TopLevel)
		{
			return false;
		}
		Variant script = source.GetScript();
		if (script.VariantType == Variant.Type.Nil)
		{
			return true;
		}
		if (!(script.AsGodotObject() is Script script2))
		{
			return false;
		}
		return script2.ResourcePath == "res://Prefab/TowerDefense/Character/TowerDefenseCharacterShadowSprite.cs";
	}

	private void CaptureAllState(Sprite2D source)
	{
		CaptureMutableState(source);
		_texture = source.Texture;
		_offset = source.Offset;
		_centered = source.Centered;
		_flipH = source.FlipH;
		_flipV = source.FlipV;
		_visible = source.Visible;
		_modulate = source.Modulate;
		_selfModulate = source.SelfModulate;
		_zIndex = source.ZIndex;
		_zAsRelative = source.ZAsRelative;
		_visibilityLayer = source.VisibilityLayer;
		_topLevel = source.TopLevel;
	}

	private void CaptureMutableState(Sprite2D source)
	{
		Transform2D transform = source.Transform;
		_position = transform.Origin;
		_scale = transform.Scale;
		_rotation = transform.Rotation;
		_skew = transform.Skew;
	}

	private Transform2D GetOwnerGlobalTransform()
	{
		if (!GodotObject.IsInstanceValid(_owner))
		{
			return Transform2D.Identity;
		}
		if (_owner is TowerDefenseCharacter towerDefenseCharacter)
		{
			ulong num = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (num == 18446744073709551615uL)
			{
				num = Engine.GetPhysicsFrames();
			}
			return towerDefenseCharacter.GetGlobalTransformForShadow(num);
		}
		return _owner.GlobalTransform;
	}

	private void MarkChanged()
	{
		_revision++;
		if (_revision == 0L)
		{
			_revision = 1uL;
		}
		EmitChanged();
	}

	private static Color Multiply(Color a, Color b)
	{
		return new Color(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName.IsVisibleInTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsInsideTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetParent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPath, new PropertyInfo(Variant.Type.NodePath, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectiveColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Capture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "detachCompatibleNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanDetach, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureAllState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureMutableState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetOwnerGlobalTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Multiply, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "a", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "b", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsVisibleInTree && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsVisibleInTree());
			return true;
		}
		if (method == MethodName.IsInsideTree && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsInsideTree());
			return true;
		}
		if (method == MethodName.GetParent && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetParent());
			return true;
		}
		if (method == MethodName.GetPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<NodePath>(GetPath());
			return true;
		}
		if (method == MethodName.GetEffectiveColor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Color>(GetEffectiveColor());
			return true;
		}
		if (method == MethodName.Capture && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseShadowVisual>(Capture(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<Sprite2D>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.Create && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseShadowVisual>(Create(VariantUtils.ConvertTo<Node2D>(in args[0])));
			return true;
		}
		if (method == MethodName.CanDetach && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDetach(VariantUtils.ConvertTo<Sprite2D>(in args[0])));
			return true;
		}
		if (method == MethodName.CaptureAllState && args.Count == 1)
		{
			CaptureAllState(VariantUtils.ConvertTo<Sprite2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureMutableState && args.Count == 1)
		{
			CaptureMutableState(VariantUtils.ConvertTo<Sprite2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOwnerGlobalTransform && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetOwnerGlobalTransform());
			return true;
		}
		if (method == MethodName.MarkChanged && args.Count == 0)
		{
			MarkChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.Multiply && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Color>(Multiply(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Capture && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseShadowVisual>(Capture(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<Sprite2D>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.Create && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseShadowVisual>(Create(VariantUtils.ConvertTo<Node2D>(in args[0])));
			return true;
		}
		if (method == MethodName.CanDetach && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDetach(VariantUtils.ConvertTo<Sprite2D>(in args[0])));
			return true;
		}
		if (method == MethodName.Multiply && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Color>(Multiply(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsVisibleInTree)
		{
			return true;
		}
		if (method == MethodName.IsInsideTree)
		{
			return true;
		}
		if (method == MethodName.GetParent)
		{
			return true;
		}
		if (method == MethodName.GetPath)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveColor)
		{
			return true;
		}
		if (method == MethodName.Capture)
		{
			return true;
		}
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.CanDetach)
		{
			return true;
		}
		if (method == MethodName.CaptureAllState)
		{
			return true;
		}
		if (method == MethodName.CaptureMutableState)
		{
			return true;
		}
		if (method == MethodName.GetOwnerGlobalTransform)
		{
			return true;
		}
		if (method == MethodName.MarkChanged)
		{
			return true;
		}
		if (method == MethodName.Multiply)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Texture)
		{
			Texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.Position)
		{
			Position = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.GlobalPosition)
		{
			GlobalPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Scale)
		{
			Scale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Rotation)
		{
			Rotation = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.Skew)
		{
			Skew = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.Transform)
		{
			Transform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.GlobalTransform)
		{
			GlobalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.Offset)
		{
			Offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.Centered)
		{
			Centered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.FlipH)
		{
			FlipH = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.FlipV)
		{
			FlipV = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Visible)
		{
			Visible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.Modulate)
		{
			Modulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.SelfModulate)
		{
			SelfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.ZIndex)
		{
			ZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ZAsRelative)
		{
			ZAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.VisibilityLayer)
		{
			VisibilityLayer = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.TopLevel)
		{
			TopLevel = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._owner)
		{
			_owner = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._legacySprite)
		{
			_legacySprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._requiresLegacyRendering)
		{
			_requiresLegacyRendering = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._texture)
		{
			_texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._position)
		{
			_position = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._scale)
		{
			_scale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._rotation)
		{
			_rotation = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._skew)
		{
			_skew = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._offset)
		{
			_offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._centered)
		{
			_centered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._flipH)
		{
			_flipH = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._flipV)
		{
			_flipV = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visible)
		{
			_visible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._modulate)
		{
			_modulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._selfModulate)
		{
			_selfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._zIndex)
		{
			_zIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._zAsRelative)
		{
			_zAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visibilityLayer)
		{
			_visibilityLayer = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName._topLevel)
		{
			_topLevel = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._revision)
		{
			_revision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.OwnerNode)
		{
			value = VariantUtils.CreateFrom<Node2D>(OwnerNode);
			return true;
		}
		if (name == PropertyName.LegacySprite)
		{
			value = VariantUtils.CreateFrom<Sprite2D>(LegacySprite);
			return true;
		}
		bool from;
		if (name == PropertyName.UsesLegacyNode)
		{
			from = UsesLegacyNode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RequiresLegacyRendering)
		{
			from = RequiresLegacyRendering;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Revision)
		{
			value = VariantUtils.CreateFrom<ulong>(Revision);
			return true;
		}
		if (name == PropertyName.Texture)
		{
			value = VariantUtils.CreateFrom<Texture2D>(Texture);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.Position)
		{
			from2 = Position;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.GlobalPosition)
		{
			from2 = GlobalPosition;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.Scale)
		{
			from2 = Scale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		float from3;
		if (name == PropertyName.Rotation)
		{
			from3 = Rotation;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.Skew)
		{
			from3 = Skew;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		Transform2D from4;
		if (name == PropertyName.Transform)
		{
			from4 = Transform;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.GlobalTransform)
		{
			from4 = GlobalTransform;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.Offset)
		{
			from2 = Offset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.Centered)
		{
			from = Centered;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.FlipH)
		{
			from = FlipH;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.FlipV)
		{
			from = FlipV;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.Visible)
		{
			from = Visible;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Color from5;
		if (name == PropertyName.Modulate)
		{
			from5 = Modulate;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.SelfModulate)
		{
			from5 = SelfModulate;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.ZIndex)
		{
			value = VariantUtils.CreateFrom<int>(ZIndex);
			return true;
		}
		if (name == PropertyName.ZAsRelative)
		{
			from = ZAsRelative;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.VisibilityLayer)
		{
			value = VariantUtils.CreateFrom<uint>(VisibilityLayer);
			return true;
		}
		if (name == PropertyName.TopLevel)
		{
			from = TopLevel;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._owner)
		{
			value = VariantUtils.CreateFrom(in _owner);
			return true;
		}
		if (name == PropertyName._legacySprite)
		{
			value = VariantUtils.CreateFrom(in _legacySprite);
			return true;
		}
		if (name == PropertyName._requiresLegacyRendering)
		{
			value = VariantUtils.CreateFrom(in _requiresLegacyRendering);
			return true;
		}
		if (name == PropertyName._texture)
		{
			value = VariantUtils.CreateFrom(in _texture);
			return true;
		}
		if (name == PropertyName._position)
		{
			value = VariantUtils.CreateFrom(in _position);
			return true;
		}
		if (name == PropertyName._scale)
		{
			value = VariantUtils.CreateFrom(in _scale);
			return true;
		}
		if (name == PropertyName._rotation)
		{
			value = VariantUtils.CreateFrom(in _rotation);
			return true;
		}
		if (name == PropertyName._skew)
		{
			value = VariantUtils.CreateFrom(in _skew);
			return true;
		}
		if (name == PropertyName._offset)
		{
			value = VariantUtils.CreateFrom(in _offset);
			return true;
		}
		if (name == PropertyName._centered)
		{
			value = VariantUtils.CreateFrom(in _centered);
			return true;
		}
		if (name == PropertyName._flipH)
		{
			value = VariantUtils.CreateFrom(in _flipH);
			return true;
		}
		if (name == PropertyName._flipV)
		{
			value = VariantUtils.CreateFrom(in _flipV);
			return true;
		}
		if (name == PropertyName._visible)
		{
			value = VariantUtils.CreateFrom(in _visible);
			return true;
		}
		if (name == PropertyName._modulate)
		{
			value = VariantUtils.CreateFrom(in _modulate);
			return true;
		}
		if (name == PropertyName._selfModulate)
		{
			value = VariantUtils.CreateFrom(in _selfModulate);
			return true;
		}
		if (name == PropertyName._zIndex)
		{
			value = VariantUtils.CreateFrom(in _zIndex);
			return true;
		}
		if (name == PropertyName._zAsRelative)
		{
			value = VariantUtils.CreateFrom(in _zAsRelative);
			return true;
		}
		if (name == PropertyName._visibilityLayer)
		{
			value = VariantUtils.CreateFrom(in _visibilityLayer);
			return true;
		}
		if (name == PropertyName._topLevel)
		{
			value = VariantUtils.CreateFrom(in _topLevel);
			return true;
		}
		if (name == PropertyName._revision)
		{
			value = VariantUtils.CreateFrom(in _revision);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._legacySprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._requiresLegacyRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._position, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._scale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._rotation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._skew, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._offset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._centered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._flipH, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._flipV, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._visible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._modulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._selfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._zIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._visibilityLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._topLevel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._revision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.OwnerNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.LegacySprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesLegacyNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RequiresLegacyRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.Revision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Texture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Position, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.GlobalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Scale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.Rotation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.Skew, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.Transform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.GlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Offset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Centered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.FlipH, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.FlipV, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.Visible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.Modulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.SelfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ZAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibilityLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.TopLevel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Texture, Variant.From<Texture2D>(Texture));
		info.AddProperty(PropertyName.Position, Variant.From<Vector2>(Position));
		info.AddProperty(PropertyName.GlobalPosition, Variant.From<Vector2>(GlobalPosition));
		info.AddProperty(PropertyName.Scale, Variant.From<Vector2>(Scale));
		info.AddProperty(PropertyName.Rotation, Variant.From<float>(Rotation));
		info.AddProperty(PropertyName.Skew, Variant.From<float>(Skew));
		info.AddProperty(PropertyName.Transform, Variant.From<Transform2D>(Transform));
		info.AddProperty(PropertyName.GlobalTransform, Variant.From<Transform2D>(GlobalTransform));
		info.AddProperty(PropertyName.Offset, Variant.From<Vector2>(Offset));
		info.AddProperty(PropertyName.Centered, Variant.From<bool>(Centered));
		info.AddProperty(PropertyName.FlipH, Variant.From<bool>(FlipH));
		info.AddProperty(PropertyName.FlipV, Variant.From<bool>(FlipV));
		info.AddProperty(PropertyName.Visible, Variant.From<bool>(Visible));
		info.AddProperty(PropertyName.Modulate, Variant.From<Color>(Modulate));
		info.AddProperty(PropertyName.SelfModulate, Variant.From<Color>(SelfModulate));
		info.AddProperty(PropertyName.ZIndex, Variant.From<int>(ZIndex));
		info.AddProperty(PropertyName.ZAsRelative, Variant.From<bool>(ZAsRelative));
		info.AddProperty(PropertyName.VisibilityLayer, Variant.From<uint>(VisibilityLayer));
		info.AddProperty(PropertyName.TopLevel, Variant.From<bool>(TopLevel));
		info.AddProperty(PropertyName._owner, Variant.From(in _owner));
		info.AddProperty(PropertyName._legacySprite, Variant.From(in _legacySprite));
		info.AddProperty(PropertyName._requiresLegacyRendering, Variant.From(in _requiresLegacyRendering));
		info.AddProperty(PropertyName._texture, Variant.From(in _texture));
		info.AddProperty(PropertyName._position, Variant.From(in _position));
		info.AddProperty(PropertyName._scale, Variant.From(in _scale));
		info.AddProperty(PropertyName._rotation, Variant.From(in _rotation));
		info.AddProperty(PropertyName._skew, Variant.From(in _skew));
		info.AddProperty(PropertyName._offset, Variant.From(in _offset));
		info.AddProperty(PropertyName._centered, Variant.From(in _centered));
		info.AddProperty(PropertyName._flipH, Variant.From(in _flipH));
		info.AddProperty(PropertyName._flipV, Variant.From(in _flipV));
		info.AddProperty(PropertyName._visible, Variant.From(in _visible));
		info.AddProperty(PropertyName._modulate, Variant.From(in _modulate));
		info.AddProperty(PropertyName._selfModulate, Variant.From(in _selfModulate));
		info.AddProperty(PropertyName._zIndex, Variant.From(in _zIndex));
		info.AddProperty(PropertyName._zAsRelative, Variant.From(in _zAsRelative));
		info.AddProperty(PropertyName._visibilityLayer, Variant.From(in _visibilityLayer));
		info.AddProperty(PropertyName._topLevel, Variant.From(in _topLevel));
		info.AddProperty(PropertyName._revision, Variant.From(in _revision));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Texture, out var value))
		{
			Texture = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.Position, out var value2))
		{
			Position = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.GlobalPosition, out var value3))
		{
			GlobalPosition = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Scale, out var value4))
		{
			Scale = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Rotation, out var value5))
		{
			Rotation = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.Skew, out var value6))
		{
			Skew = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.Transform, out var value7))
		{
			Transform = value7.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.GlobalTransform, out var value8))
		{
			GlobalTransform = value8.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.Offset, out var value9))
		{
			Offset = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.Centered, out var value10))
		{
			Centered = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.FlipH, out var value11))
		{
			FlipH = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.FlipV, out var value12))
		{
			FlipV = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Visible, out var value13))
		{
			Visible = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Modulate, out var value14))
		{
			Modulate = value14.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.SelfModulate, out var value15))
		{
			SelfModulate = value15.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.ZIndex, out var value16))
		{
			ZIndex = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ZAsRelative, out var value17))
		{
			ZAsRelative = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.VisibilityLayer, out var value18))
		{
			VisibilityLayer = value18.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.TopLevel, out var value19))
		{
			TopLevel = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._owner, out var value20))
		{
			_owner = value20.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._legacySprite, out var value21))
		{
			_legacySprite = value21.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._requiresLegacyRendering, out var value22))
		{
			_requiresLegacyRendering = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._texture, out var value23))
		{
			_texture = value23.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._position, out var value24))
		{
			_position = value24.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._scale, out var value25))
		{
			_scale = value25.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._rotation, out var value26))
		{
			_rotation = value26.As<float>();
		}
		if (info.TryGetProperty(PropertyName._skew, out var value27))
		{
			_skew = value27.As<float>();
		}
		if (info.TryGetProperty(PropertyName._offset, out var value28))
		{
			_offset = value28.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._centered, out var value29))
		{
			_centered = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._flipH, out var value30))
		{
			_flipH = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._flipV, out var value31))
		{
			_flipV = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visible, out var value32))
		{
			_visible = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._modulate, out var value33))
		{
			_modulate = value33.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._selfModulate, out var value34))
		{
			_selfModulate = value34.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._zIndex, out var value35))
		{
			_zIndex = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName._zAsRelative, out var value36))
		{
			_zAsRelative = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visibilityLayer, out var value37))
		{
			_visibilityLayer = value37.As<uint>();
		}
		if (info.TryGetProperty(PropertyName._topLevel, out var value38))
		{
			_topLevel = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._revision, out var value39))
		{
			_revision = value39.As<ulong>();
		}
	}
}
