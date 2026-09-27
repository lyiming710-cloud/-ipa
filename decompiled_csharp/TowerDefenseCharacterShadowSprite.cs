using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseCharacterShadowSprite.cs")]
public sealed class TowerDefenseCharacterShadowSprite : Sprite2D
{
	internal readonly struct LocalPoseSnapshot
	{
		internal readonly Transform2D Transform;

		internal readonly Vector2 Position;

		internal readonly Vector2 Scale;

		internal readonly float Rotation;

		internal readonly float Skew;

		internal readonly bool TopLevel;

		internal LocalPoseSnapshot(Transform2D transform, bool topLevel)
		{
			Transform = transform;
			Position = transform.Origin;
			Scale = transform.Scale;
			Rotation = transform.Rotation;
			Skew = transform.Skew;
			TopLevel = topLevel;
		}
	}

	public new class MethodName : Sprite2D.MethodName
	{
		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName CaptureLocalTransform = "CaptureLocalTransform";

		public static readonly StringName AdvanceLocalTransformRevision = "AdvanceLocalTransformRevision";
	}

	public new class PropertyName : Sprite2D.PropertyName
	{
		public static readonly StringName CachedLocalTransform = "CachedLocalTransform";

		public static readonly StringName LocalTransformRevision = "LocalTransformRevision";

		public static readonly StringName LocalPoseReady = "LocalPoseReady";

		public static readonly StringName _localTransformRevision = "_localTransformRevision";

		public static readonly StringName _localPoseReady = "_localPoseReady";
	}

	public new class SignalName : Sprite2D.SignalName
	{
	}

	private LocalPoseSnapshot _cachedLocalPose = new LocalPoseSnapshot(Transform2D.Identity, topLevel: false);

	private ulong _localTransformRevision = 1uL;

	private bool _localPoseReady;

	internal ref readonly LocalPoseSnapshot CachedLocalPose => ref _cachedLocalPose;

	internal Transform2D CachedLocalTransform => _cachedLocalPose.Transform;

	internal ulong LocalTransformRevision => _localTransformRevision;

	internal bool LocalPoseReady => _localPoseReady;

	public override void _EnterTree()
	{
		base._EnterTree();
		CaptureLocalTransform(Transform, TopLevel);
		AdvanceLocalTransformRevision();
		SetNotifyLocalTransform(enable: true);
	}

	public override void _ExitTree()
	{
		SetNotifyLocalTransform(enable: false);
		base._ExitTree();
	}

	public override void _Notification(int what)
	{
		if ((long)what == 35)
		{
			Transform2D transform = Transform;
			bool topLevel = TopLevel;
			if (!transform.IsEqualApprox(_cachedLocalPose.Transform) || topLevel != _cachedLocalPose.TopLevel)
			{
				CaptureLocalTransform(transform, topLevel);
				AdvanceLocalTransformRevision();
			}
		}
	}

	private void CaptureLocalTransform(Transform2D localTransform, bool topLevel)
	{
		_cachedLocalPose = new LocalPoseSnapshot(localTransform, topLevel);
		_localPoseReady = true;
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
			new MethodInfo(MethodName.CaptureLocalTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "localTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "topLevel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.CaptureLocalTransform && args.Count == 2)
		{
			CaptureLocalTransform(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
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
		if (method == MethodName.CaptureLocalTransform)
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
		if (name == PropertyName._localTransformRevision)
		{
			_localTransformRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._localPoseReady)
		{
			_localPoseReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CachedLocalTransform)
		{
			value = VariantUtils.CreateFrom<Transform2D>(CachedLocalTransform);
			return true;
		}
		if (name == PropertyName.LocalTransformRevision)
		{
			value = VariantUtils.CreateFrom<ulong>(LocalTransformRevision);
			return true;
		}
		if (name == PropertyName.LocalPoseReady)
		{
			value = VariantUtils.CreateFrom<bool>(LocalPoseReady);
			return true;
		}
		if (name == PropertyName._localTransformRevision)
		{
			value = VariantUtils.CreateFrom(in _localTransformRevision);
			return true;
		}
		if (name == PropertyName._localPoseReady)
		{
			value = VariantUtils.CreateFrom(in _localPoseReady);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._localTransformRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._localPoseReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.CachedLocalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LocalTransformRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.LocalPoseReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._localTransformRevision, Variant.From(in _localTransformRevision));
		info.AddProperty(PropertyName._localPoseReady, Variant.From(in _localPoseReady));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._localTransformRevision, out var value))
		{
			_localTransformRevision = value.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._localPoseReady, out var value2))
		{
			_localPoseReady = value2.As<bool>();
		}
	}
}
