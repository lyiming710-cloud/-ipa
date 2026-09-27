using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Component/MoveComponent/MoveComponent.cs")]
public class MoveComponent : ComponentBase
{
	public delegate void MovementActivityChangedEventHandler(bool active);

	public new class MethodName : ComponentBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName RefreshMovementProcessing = "RefreshMovementProcessing";

		public static readonly StringName ClearStoppedMovementRenderInterpolation = "ClearStoppedMovementRenderInterpolation";

		public new static readonly StringName GetGlobalPosition = "GetGlobalPosition";

		public new static readonly StringName SetGlobalPosition = "SetGlobalPosition";

		public static readonly StringName SetVelocity = "SetVelocity";

		public static readonly StringName SetGravity = "SetGravity";

		public static readonly StringName MoveClear = "MoveClear";

		public new static readonly StringName ExportComponentSave = "ExportComponentSave";

		public new static readonly StringName ImportComponentSave = "ImportComponentSave";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";
	}

	public new class PropertyName : ComponentBase.PropertyName
	{
		public new static readonly StringName UseSharedPhysicsBatch = "UseSharedPhysicsBatch";

		public static readonly StringName velocity = "velocity";

		public static readonly StringName gravity = "gravity";

		public static readonly StringName HasActiveMovement = "HasActiveMovement";

		public static readonly StringName _velocity = "_velocity";

		public static readonly StringName _gravity = "_gravity";

		public static readonly StringName _movementActive = "_movementActive";

		public static readonly StringName moveScale = "moveScale";

		public static readonly StringName parent = "parent";
	}

	public new class SignalName : ComponentBase.SignalName
	{
	}

	private Vector2 _velocity = Vector2.Zero;

	private double _gravity;

	private bool _movementActive;

	[Export(PropertyHint.None, "")]
	public double moveScale = 1.0;

	public CanvasItem parent;

	protected override bool UseSharedPhysicsBatch => true;

	[Export(PropertyHint.None, "")]
	public Vector2 velocity
	{
		get
		{
			return _velocity;
		}
		set
		{
			if (!(_velocity == value))
			{
				_velocity = value;
				RefreshMovementProcessing();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double gravity
	{
		get
		{
			return _gravity;
		}
		set
		{
			if (_gravity != value)
			{
				_gravity = value;
				RefreshMovementProcessing();
			}
		}
	}

	public bool HasActiveMovement
	{
		get
		{
			if (!(_velocity != Vector2.Zero))
			{
				return _gravity != 0.0;
			}
			return true;
		}
	}

	public event MovementActivityChangedEventHandler MovementActivityChanged;

	public override string _GetName()
	{
		return "MoveComponent";
	}

	public override void _Ready()
	{
		parent = GetParent() as CanvasItem;
		RefreshMovementProcessing();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!alive || parent == null)
		{
			return;
		}
		if (!HasActiveMovement)
		{
			SetSharedPhysicsBatchWorkEnabled(enabled: false);
			return;
		}
		if (_gravity != 0.0)
		{
			_velocity = new Vector2(_velocity.X, _velocity.Y + (float)(_gravity * delta * moveScale));
		}
		Vector2 globalPosition = GetGlobalPosition(parent);
		SetGlobalPosition(parent, globalPosition + _velocity * (float)(delta * moveScale));
	}

	private void RefreshMovementProcessing()
	{
		bool hasActiveMovement = HasActiveMovement;
		SetSharedPhysicsBatchWorkEnabled(hasActiveMovement);
		if (_movementActive != hasActiveMovement)
		{
			bool num = _movementActive && !hasActiveMovement;
			_movementActive = hasActiveMovement;
			if (num)
			{
				ClearStoppedMovementRenderInterpolation();
			}
			MovementActivityChanged?.Invoke(hasActiveMovement);
		}
	}

	private void ClearStoppedMovementRenderInterpolation()
	{
		Node node = parent;
		if (node != null)
		{
			ClearStoppedMovementRenderInterpolation(node);
		}
	}

	private static void ClearStoppedMovementRenderInterpolation(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.ClearRetainedRootMotionForStoppedMovement();
		}
		foreach (Node child in node.GetChildren())
		{
			ClearStoppedMovementRenderInterpolation(child);
		}
	}

	private static Vector2 GetGlobalPosition(CanvasItem node)
	{
		if (node is Node2D node2D)
		{
			return node2D.GlobalPosition;
		}
		if (node is Control control)
		{
			return control.GlobalPosition;
		}
		return Vector2.Zero;
	}

	private static void SetGlobalPosition(CanvasItem node, Vector2 pos)
	{
		if (node is Node2D node2D)
		{
			node2D.GlobalPosition = pos;
		}
		else if (node is Control control)
		{
			control.GlobalPosition = pos;
		}
	}

	public void SetVelocity(Vector2 _velocity)
	{
		velocity = _velocity;
	}

	public void SetGravity(double _gravity)
	{
		gravity = _gravity;
	}

	public void MoveClear()
	{
		_velocity = Vector2.Zero;
		_gravity = 0.0;
		RefreshMovementProcessing();
	}

	public override Dictionary ExportComponentSave()
	{
		return new Dictionary
		{
			{ "velocityX", _velocity.X },
			{ "velocityY", _velocity.Y },
			{ "gravity", _gravity },
			{ "moveScale", moveScale }
		};
	}

	public override void ImportComponentSave(Dictionary _data, TowerDefenseLevelSaveConfigCSharp _owner)
	{
		_velocity = new Vector2((float)_data.GetValueOrDefault("velocityX", 0.0).AsDouble(), (float)_data.GetValueOrDefault("velocityY", 0.0).AsDouble());
		_gravity = _data.GetValueOrDefault("gravity", 0.0).AsDouble();
		moveScale = _data.GetValueOrDefault("moveScale", 1.0).AsDouble();
		RefreshMovementProcessing();
	}

	public override Dictionary SyncSerialize()
	{
		return new Dictionary
		{
			{ "velocityX", _velocity.X },
			{ "velocityY", _velocity.Y },
			{ "gravity", _gravity },
			{ "moveScale", moveScale }
		};
	}

	public override void SyncDeserialize(Dictionary data)
	{
		_velocity = new Vector2((float)data.GetValueOrDefault("velocityX", 0.0).AsDouble(), (float)data.GetValueOrDefault("velocityY", 0.0).AsDouble());
		_gravity = data.GetValueOrDefault("gravity", 0.0).AsDouble();
		moveScale = data.GetValueOrDefault("moveScale", 1.0).AsDouble();
		RefreshMovementProcessing();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMovementProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearStoppedMovementRenderInterpolation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearStoppedMovementRenderInterpolation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetGlobalPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetGlobalPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasItem"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVelocity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "_velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetGravity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportComponentSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportComponentSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "_data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.RefreshMovementProcessing && args.Count == 0)
		{
			RefreshMovementProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearStoppedMovementRenderInterpolation && args.Count == 0)
		{
			ClearStoppedMovementRenderInterpolation();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearStoppedMovementRenderInterpolation && args.Count == 1)
		{
			ClearStoppedMovementRenderInterpolation(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetGlobalPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetGlobalPosition(VariantUtils.ConvertTo<CanvasItem>(in args[0])));
			return true;
		}
		if (method == MethodName.SetGlobalPosition && args.Count == 2)
		{
			SetGlobalPosition(VariantUtils.ConvertTo<CanvasItem>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetVelocity && args.Count == 1)
		{
			SetVelocity(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGravity && args.Count == 1)
		{
			SetGravity(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveClear && args.Count == 0)
		{
			MoveClear();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportComponentSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportComponentSave());
			return true;
		}
		if (method == MethodName.ImportComponentSave && args.Count == 2)
		{
			ImportComponentSave(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClearStoppedMovementRenderInterpolation && args.Count == 1)
		{
			ClearStoppedMovementRenderInterpolation(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetGlobalPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetGlobalPosition(VariantUtils.ConvertTo<CanvasItem>(in args[0])));
			return true;
		}
		if (method == MethodName.SetGlobalPosition && args.Count == 2)
		{
			SetGlobalPosition(VariantUtils.ConvertTo<CanvasItem>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.RefreshMovementProcessing)
		{
			return true;
		}
		if (method == MethodName.ClearStoppedMovementRenderInterpolation)
		{
			return true;
		}
		if (method == MethodName.GetGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.SetGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.SetVelocity)
		{
			return true;
		}
		if (method == MethodName.SetGravity)
		{
			return true;
		}
		if (method == MethodName.MoveClear)
		{
			return true;
		}
		if (method == MethodName.ExportComponentSave)
		{
			return true;
		}
		if (method == MethodName.ImportComponentSave)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.velocity)
		{
			velocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			gravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._velocity)
		{
			_velocity = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._gravity)
		{
			_gravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._movementActive)
		{
			_movementActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.moveScale)
		{
			moveScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.parent)
		{
			parent = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.UseSharedPhysicsBatch)
		{
			from = UseSharedPhysicsBatch;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.velocity)
		{
			value = VariantUtils.CreateFrom<Vector2>(velocity);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			value = VariantUtils.CreateFrom<double>(gravity);
			return true;
		}
		if (name == PropertyName.HasActiveMovement)
		{
			from = HasActiveMovement;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._velocity)
		{
			value = VariantUtils.CreateFrom(in _velocity);
			return true;
		}
		if (name == PropertyName._gravity)
		{
			value = VariantUtils.CreateFrom(in _gravity);
			return true;
		}
		if (name == PropertyName._movementActive)
		{
			value = VariantUtils.CreateFrom(in _movementActive);
			return true;
		}
		if (name == PropertyName.moveScale)
		{
			value = VariantUtils.CreateFrom(in moveScale);
			return true;
		}
		if (name == PropertyName.parent)
		{
			value = VariantUtils.CreateFrom(in parent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseSharedPhysicsBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._velocity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._gravity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._movementActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.velocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.moveScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.parent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasActiveMovement, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.velocity, Variant.From<Vector2>(velocity));
		info.AddProperty(PropertyName.gravity, Variant.From<double>(gravity));
		info.AddProperty(PropertyName._velocity, Variant.From(in _velocity));
		info.AddProperty(PropertyName._gravity, Variant.From(in _gravity));
		info.AddProperty(PropertyName._movementActive, Variant.From(in _movementActive));
		info.AddProperty(PropertyName.moveScale, Variant.From(in moveScale));
		info.AddProperty(PropertyName.parent, Variant.From(in parent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.velocity, out var value))
		{
			velocity = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.gravity, out var value2))
		{
			gravity = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._velocity, out var value3))
		{
			_velocity = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._gravity, out var value4))
		{
			_gravity = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._movementActive, out var value5))
		{
			_movementActive = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.moveScale, out var value6))
		{
			moveScale = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.parent, out var value7))
		{
			parent = value7.As<CanvasItem>();
		}
	}
}
