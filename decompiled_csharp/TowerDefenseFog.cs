using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Fog/Object/TowerDefenseFog.cs")]
public class TowerDefenseFog : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName SetBatchManaged = "SetBatchManaged";

		public static readonly StringName BindBatch = "BindBatch";

		public static readonly StringName SetCanVisible = "SetCanVisible";

		public static readonly StringName SetMagicColor = "SetMagicColor";

		public static readonly StringName GetMagicColor = "GetMagicColor";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName IsBatchManaged = "IsBatchManaged";

		public static readonly StringName IsLit = "IsLit";

		public static readonly StringName sprite = "sprite";

		public static readonly StringName area = "area";

		public static readonly StringName shape = "shape";

		public static readonly StringName canVisible = "canVisible";

		public static readonly StringName beginColumn = "beginColumn";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName savePos = "savePos";

		public static readonly StringName lightOverlapEnabled = "lightOverlapEnabled";

		public static readonly StringName _hasLightOverlap = "_hasLightOverlap";

		public static readonly StringName _batchManaged = "_batchManaged";

		public static readonly StringName _batch = "_batch";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const uint LightCollisionLayerMask = 8u;

	public Sprite2D sprite;

	public AabbArea2D area;

	public CollisionShape2D shape;

	public bool canVisible = true;

	public int beginColumn = 6;

	public Vector2 gridPos = Vector2.Zero;

	public Vector2 savePos;

	public bool lightOverlapEnabled = true;

	private bool _hasLightOverlap;

	private bool _batchManaged;

	private TowerDefenseFogBatch _batch;

	internal bool IsBatchManaged => _batchManaged;

	public bool IsLit => _hasLightOverlap;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("%Sprite");
		area = GetNode<AabbArea2D>("%Area");
		shape = GetNode<CollisionShape2D>("%Shape");
		savePos = GlobalPosition;
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		shape.Shape = shape.Shape.Duplicate(deep: true) as Shape2D;
		if (shape.Shape is RectangleShape2D rectangleShape2D)
		{
			rectangleShape2D.Size = mapGridSize;
		}
		sprite.Frame = GD.RandRange(0, 7);
		if (!lightOverlapEnabled)
		{
			Color modulate = sprite.Modulate;
			float a = (canVisible ? 1f : 0f);
			sprite.Modulate = new Color(modulate.R, modulate.G, modulate.B, a);
			SetPhysicsProcess(enable: false);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!lightOverlapEnabled)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		if (_batchManaged)
		{
			SetPhysicsProcess(enable: false);
			return;
		}
		IReadOnlyList<AabbAreaLayerRegistry.WorldRectSnapshot> worldRectSnapshotsForCollisionLayer = AabbAreaLayerRegistry.GetWorldRectSnapshotsForCollisionLayer(TowerDefenseManager.GetCharacterNode(), 8u);
		UpdateFogState(delta, worldRectSnapshotsForCollisionLayer);
	}

	internal void SetBatchManaged(bool managed)
	{
		_batchManaged = managed;
		if (!managed)
		{
			_batch = null;
		}
		SetPhysicsProcess(!managed);
	}

	internal void BindBatch(TowerDefenseFogBatch batch)
	{
		_batch = batch;
	}

	internal bool BatchPhysicsUpdate(double delta, IReadOnlyList<AabbAreaLayerRegistry.WorldRectSnapshot> lightAreas)
	{
		if (!_batchManaged)
		{
			return true;
		}
		return UpdateFogState(delta, lightAreas);
	}

	private bool UpdateFogState(double delta, IReadOnlyList<AabbAreaLayerRegistry.WorldRectSnapshot> lightAreas)
	{
		_hasLightOverlap = lightOverlapEnabled && HasLightOverlap(lightAreas);
		float num;
		if (!canVisible || _hasLightOverlap)
		{
			num = 0f;
		}
		else
		{
			num = ((beginColumn == (int)gridPos.X) ? 0.75f : 1f);
		}
		Color modulate = sprite.Modulate;
		float num2 = Mathf.Lerp(modulate.A, num, (float)delta * 2f);
		bool flag = Mathf.Abs(num2 - num) <= 0.001f;
		if (flag)
		{
			num2 = num;
		}
		if (!Mathf.IsEqualApprox(modulate.A, num2))
		{
			sprite.Modulate = new Color(modulate.R, modulate.G, modulate.B, num2);
		}
		return flag;
	}

	public void SetCanVisible(bool visible)
	{
		if (canVisible != visible)
		{
			canVisible = visible;
			if (!lightOverlapEnabled && GodotObject.IsInstanceValid(sprite))
			{
				Color modulate = sprite.Modulate;
				float a = (visible ? 1f : 0f);
				sprite.Modulate = new Color(modulate.R, modulate.G, modulate.B, a);
				SetPhysicsProcess(enable: false);
			}
			else if (_batchManaged)
			{
				_batch?.RequestVisibilityUpdate();
			}
		}
	}

	public void SetMagicColor(Color rgb)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			Color modulate = sprite.Modulate;
			sprite.Modulate = new Color(rgb.R, rgb.G, rgb.B, modulate.A);
		}
	}

	public Color GetMagicColor()
	{
		if (!GodotObject.IsInstanceValid(sprite))
		{
			return Colors.White;
		}
		Color modulate = sprite.Modulate;
		return new Color(modulate.R, modulate.G, modulate.B);
	}

	private bool HasLightOverlap(IReadOnlyList<AabbAreaLayerRegistry.WorldRectSnapshot> lightAreas)
	{
		if (!GodotObject.IsInstanceValid(area))
		{
			return false;
		}
		if (!area.TryGetWorldRect(out var rect))
		{
			return false;
		}
		for (int i = 0; i < lightAreas.Count; i++)
		{
			AabbAreaLayerRegistry.WorldRectSnapshot worldRectSnapshot = lightAreas[i];
			if (AabbShapeUtil.Intersects(rect, worldRectSnapshot.WorldRect))
			{
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBatchManaged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "managed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "batch", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetCanVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMagicColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "rgb", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMagicColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetBatchManaged && args.Count == 1)
		{
			SetBatchManaged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindBatch && args.Count == 1)
		{
			BindBatch(VariantUtils.ConvertTo<TowerDefenseFogBatch>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCanVisible && args.Count == 1)
		{
			SetCanVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMagicColor && args.Count == 1)
		{
			SetMagicColor(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMagicColor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Color>(GetMagicColor());
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
		if (method == MethodName.SetBatchManaged)
		{
			return true;
		}
		if (method == MethodName.BindBatch)
		{
			return true;
		}
		if (method == MethodName.SetCanVisible)
		{
			return true;
		}
		if (method == MethodName.SetMagicColor)
		{
			return true;
		}
		if (method == MethodName.GetMagicColor)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.area)
		{
			area = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.shape)
		{
			shape = VariantUtils.ConvertTo<CollisionShape2D>(in value);
			return true;
		}
		if (name == PropertyName.canVisible)
		{
			canVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.beginColumn)
		{
			beginColumn = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.savePos)
		{
			savePos = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.lightOverlapEnabled)
		{
			lightOverlapEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasLightOverlap)
		{
			_hasLightOverlap = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._batchManaged)
		{
			_batchManaged = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._batch)
		{
			_batch = VariantUtils.ConvertTo<TowerDefenseFogBatch>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsBatchManaged)
		{
			from = IsBatchManaged;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsLit)
		{
			from = IsLit;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom(in sprite);
			return true;
		}
		if (name == PropertyName.area)
		{
			value = VariantUtils.CreateFrom(in area);
			return true;
		}
		if (name == PropertyName.shape)
		{
			value = VariantUtils.CreateFrom(in shape);
			return true;
		}
		if (name == PropertyName.canVisible)
		{
			value = VariantUtils.CreateFrom(in canVisible);
			return true;
		}
		if (name == PropertyName.beginColumn)
		{
			value = VariantUtils.CreateFrom(in beginColumn);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.savePos)
		{
			value = VariantUtils.CreateFrom(in savePos);
			return true;
		}
		if (name == PropertyName.lightOverlapEnabled)
		{
			value = VariantUtils.CreateFrom(in lightOverlapEnabled);
			return true;
		}
		if (name == PropertyName._hasLightOverlap)
		{
			value = VariantUtils.CreateFrom(in _hasLightOverlap);
			return true;
		}
		if (name == PropertyName._batchManaged)
		{
			value = VariantUtils.CreateFrom(in _batchManaged);
			return true;
		}
		if (name == PropertyName._batch)
		{
			value = VariantUtils.CreateFrom(in _batch);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.area, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.shape, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.beginColumn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.savePos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.lightOverlapEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasLightOverlap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._batchManaged, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._batch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsBatchManaged, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsLit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sprite, Variant.From(in sprite));
		info.AddProperty(PropertyName.area, Variant.From(in area));
		info.AddProperty(PropertyName.shape, Variant.From(in shape));
		info.AddProperty(PropertyName.canVisible, Variant.From(in canVisible));
		info.AddProperty(PropertyName.beginColumn, Variant.From(in beginColumn));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.savePos, Variant.From(in savePos));
		info.AddProperty(PropertyName.lightOverlapEnabled, Variant.From(in lightOverlapEnabled));
		info.AddProperty(PropertyName._hasLightOverlap, Variant.From(in _hasLightOverlap));
		info.AddProperty(PropertyName._batchManaged, Variant.From(in _batchManaged));
		info.AddProperty(PropertyName._batch, Variant.From(in _batch));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sprite, out var value))
		{
			sprite = value.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.area, out var value2))
		{
			area = value2.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.shape, out var value3))
		{
			shape = value3.As<CollisionShape2D>();
		}
		if (info.TryGetProperty(PropertyName.canVisible, out var value4))
		{
			canVisible = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.beginColumn, out var value5))
		{
			beginColumn = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value6))
		{
			gridPos = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.savePos, out var value7))
		{
			savePos = value7.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.lightOverlapEnabled, out var value8))
		{
			lightOverlapEnabled = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasLightOverlap, out var value9))
		{
			_hasLightOverlap = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._batchManaged, out var value10))
		{
			_batchManaged = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._batch, out var value11))
		{
			_batch = value11.As<TowerDefenseFogBatch>();
		}
	}
}
