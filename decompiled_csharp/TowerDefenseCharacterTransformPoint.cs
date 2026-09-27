using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseCharacterTransformPoint.cs")]
public sealed class TowerDefenseCharacterTransformPoint : Marker2D
{
	public new class MethodName : Marker2D.MethodName
	{
		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName AdvanceScaleRevision = "AdvanceScaleRevision";

		public static readonly StringName AdvanceLocalTransformRevision = "AdvanceLocalTransformRevision";
	}

	public new class PropertyName : Marker2D.PropertyName
	{
		public static readonly StringName CachedScale = "CachedScale";

		public static readonly StringName ScaleRevision = "ScaleRevision";

		public static readonly StringName CachedLocalTransform = "CachedLocalTransform";

		public static readonly StringName LocalTransformRevision = "LocalTransformRevision";

		public static readonly StringName _cachedScale = "_cachedScale";

		public static readonly StringName _scaleRevision = "_scaleRevision";

		public static readonly StringName _cachedLocalTransform = "_cachedLocalTransform";

		public static readonly StringName _localTransformRevision = "_localTransformRevision";

		public static readonly StringName _owner = "_owner";
	}

	public new class SignalName : Marker2D.SignalName
	{
	}

	private Vector2 _cachedScale = Vector2.One;

	private ulong _scaleRevision = 1uL;

	private Transform2D _cachedLocalTransform = Transform2D.Identity;

	private ulong _localTransformRevision = 1uL;

	private TowerDefenseCharacter _owner;

	internal Vector2 CachedScale => _cachedScale;

	internal ulong ScaleRevision => _scaleRevision;

	internal Transform2D CachedLocalTransform => _cachedLocalTransform;

	internal ulong LocalTransformRevision => _localTransformRevision;

	public override void _EnterTree()
	{
		base._EnterTree();
		_owner = GetParent()?.GetParent() as TowerDefenseCharacter;
		_cachedLocalTransform = Transform;
		_cachedScale = _cachedLocalTransform.Scale;
		AdvanceScaleRevision();
		AdvanceLocalTransformRevision();
		SetNotifyLocalTransform(enable: true);
	}

	public override void _ExitTree()
	{
		SetNotifyLocalTransform(enable: false);
		_owner = null;
		base._ExitTree();
	}

	public override void _Notification(int what)
	{
		if ((long)what != 35)
		{
			return;
		}
		Transform2D transform = Transform;
		if (!transform.IsEqualApprox(_cachedLocalTransform))
		{
			Vector2 scale = transform.Scale;
			_cachedLocalTransform = transform;
			AdvanceLocalTransformRevision();
			if (GodotObject.IsInstanceValid(_owner?.sprite))
			{
				_owner.sprite.NotifyAncestorTransformChangedForRender();
			}
			if (!scale.IsEqualApprox(_cachedScale))
			{
				_cachedScale = scale;
				AdvanceScaleRevision();
			}
		}
	}

	private void AdvanceScaleRevision()
	{
		_scaleRevision++;
		if (_scaleRevision == 0L)
		{
			_scaleRevision = 1uL;
		}
	}

	private void AdvanceLocalTransformRevision()
	{
		_localTransformRevision++;
		if (_localTransformRevision == 0L)
		{
			_localTransformRevision = 1uL;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceScaleRevision, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceLocalTransformRevision, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceScaleRevision && args.Count == 0)
		{
			AdvanceScaleRevision();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceLocalTransformRevision && args.Count == 0)
		{
			AdvanceLocalTransformRevision();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.AdvanceScaleRevision)
		{
			return true;
		}
		if (method == MethodName.AdvanceLocalTransformRevision)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._cachedScale)
		{
			_cachedScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._scaleRevision)
		{
			_scaleRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedLocalTransform)
		{
			_cachedLocalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._localTransformRevision)
		{
			_localTransformRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._owner)
		{
			_owner = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CachedScale)
		{
			value = VariantUtils.CreateFrom<Vector2>(CachedScale);
			return true;
		}
		ulong from;
		if (name == PropertyName.ScaleRevision)
		{
			from = ScaleRevision;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CachedLocalTransform)
		{
			value = VariantUtils.CreateFrom<Transform2D>(CachedLocalTransform);
			return true;
		}
		if (name == PropertyName.LocalTransformRevision)
		{
			from = LocalTransformRevision;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._cachedScale)
		{
			value = VariantUtils.CreateFrom(in _cachedScale);
			return true;
		}
		if (name == PropertyName._scaleRevision)
		{
			value = VariantUtils.CreateFrom(in _scaleRevision);
			return true;
		}
		if (name == PropertyName._cachedLocalTransform)
		{
			value = VariantUtils.CreateFrom(in _cachedLocalTransform);
			return true;
		}
		if (name == PropertyName._localTransformRevision)
		{
			value = VariantUtils.CreateFrom(in _localTransformRevision);
			return true;
		}
		if (name == PropertyName._owner)
		{
			value = VariantUtils.CreateFrom(in _owner);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cachedScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._scaleRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._cachedLocalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._localTransformRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._owner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.CachedScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ScaleRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.CachedLocalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LocalTransformRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._cachedScale, Variant.From(in _cachedScale));
		info.AddProperty(PropertyName._scaleRevision, Variant.From(in _scaleRevision));
		info.AddProperty(PropertyName._cachedLocalTransform, Variant.From(in _cachedLocalTransform));
		info.AddProperty(PropertyName._localTransformRevision, Variant.From(in _localTransformRevision));
		info.AddProperty(PropertyName._owner, Variant.From(in _owner));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._cachedScale, out var value))
		{
			_cachedScale = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._scaleRevision, out var value2))
		{
			_scaleRevision = value2.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedLocalTransform, out var value3))
		{
			_cachedLocalTransform = value3.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._localTransformRevision, out var value4))
		{
			_localTransformRevision = value4.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._owner, out var value5))
		{
			_owner = value5.As<TowerDefenseCharacter>();
		}
	}
}
