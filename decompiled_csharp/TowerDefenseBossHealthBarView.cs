using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/BossHealthBar/TowerDefenseBossHealthBarView.cs")]
public class TowerDefenseBossHealthBarView : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName Configure = "Configure";

		public static readonly StringName SetRatios = "SetRatios";

		public static readonly StringName SetDead = "SetDead";

		public static readonly StringName SetVertical = "SetVertical";

		public static readonly StringName ContainsCanvasPoint = "ContainsCanvasPoint";

		public static readonly StringName SetPointerOverlap = "SetPointerOverlap";

		public static readonly StringName SetLifecycleAlpha = "SetLifecycleAlpha";

		public static readonly StringName ApplyCombinedAlpha = "ApplyCombinedAlpha";

		public static readonly StringName NormalizeRatio = "NormalizeRatio";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _rotatingBody = "_rotatingBody";

		public static readonly StringName _bodyProgress = "_bodyProgress";

		public static readonly StringName _shieldProgress = "_shieldProgress";

		public static readonly StringName _icon = "_icon";

		public static readonly StringName _deadCross = "_deadCross";

		public static readonly StringName _interactionAlpha = "_interactionAlpha";

		public static readonly StringName _targetInteractionAlpha = "_targetInteractionAlpha";

		public static readonly StringName _lifecycleAlpha = "_lifecycleAlpha";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public const float SourceWidth = 575f;

	public const float SourceHeight = 109f;

	private Control _rotatingBody;

	private TextureProgressBar _bodyProgress;

	private TextureProgressBar _shieldProgress;

	private TextureRect _icon;

	private TextureRect _deadCross;

	private float _interactionAlpha = 1f;

	private float _targetInteractionAlpha = 1f;

	private float _lifecycleAlpha = 1f;

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Ignore;
		_rotatingBody = GetNode<Control>("%RotatingBody");
		_bodyProgress = GetNode<TextureProgressBar>("%BodyProgress");
		_shieldProgress = GetNode<TextureProgressBar>("%ShieldProgress");
		_icon = GetNode<TextureRect>("%BossIcon");
		_deadCross = GetNode<TextureRect>("%DeadCross");
		SetVertical(vertical: false);
		ApplyCombinedAlpha();
	}

	public override void _Process(double delta)
	{
		float delta2 = (float)(delta / 0.12);
		float num = Mathf.MoveToward(_interactionAlpha, _targetInteractionAlpha, delta2);
		if (!Mathf.IsEqualApprox(num, _interactionAlpha))
		{
			_interactionAlpha = num;
			ApplyCombinedAlpha();
		}
	}

	public void Configure(Texture2D icon, Color bodyColor)
	{
		_icon.Texture = icon;
		_icon.Visible = GodotObject.IsInstanceValid(icon);
		_bodyProgress.SelfModulate = bodyColor;
	}

	public void SetRatios(double bodyRatio, double shieldRatio)
	{
		_bodyProgress.Value = NormalizeRatio(bodyRatio);
		double num = NormalizeRatio(shieldRatio);
		_shieldProgress.Value = num;
		_shieldProgress.Visible = num > 0.0;
	}

	public void SetDead(bool dead)
	{
		_deadCross.Visible = dead;
		if (dead)
		{
			SetRatios(0.0, 0.0);
		}
	}

	public void SetVertical(bool vertical)
	{
		if (IsNodeReady())
		{
			if (vertical)
			{
				CustomMinimumSize = new Vector2(109f, 575f);
				Size = CustomMinimumSize;
				_rotatingBody.Position = new Vector2(0f, 575f);
				_rotatingBody.Rotation = -(float)Math.PI / 2f;
				_icon.Position = new Vector2(22f, 499f);
				_icon.Size = new Vector2(64f, 60f);
				_deadCross.Position = new Vector2(19f, 497f);
				_deadCross.Size = new Vector2(71f, 62f);
			}
			else
			{
				CustomMinimumSize = new Vector2(575f, 109f);
				Size = CustomMinimumSize;
				_rotatingBody.Position = Vector2.Zero;
				_rotatingBody.Rotation = 0f;
				_icon.Position = new Vector2(8f, 18f);
				_icon.Size = new Vector2(78f, 73f);
				_deadCross.Position = new Vector2(4f, 17f);
				_deadCross.Size = new Vector2(87f, 75f);
			}
		}
	}

	public bool ContainsCanvasPoint(Vector2 canvasPoint)
	{
		Vector2 point = GetGlobalTransformWithCanvas().AffineInverse() * canvasPoint;
		return new Rect2(Vector2.Zero, Size).HasPoint(point);
	}

	public void SetPointerOverlap(bool overlaps)
	{
		_targetInteractionAlpha = (overlaps ? 0.4f : 1f);
	}

	public void SetLifecycleAlpha(float alpha)
	{
		_lifecycleAlpha = Mathf.Clamp(alpha, 0f, 1f);
		ApplyCombinedAlpha();
	}

	private void ApplyCombinedAlpha()
	{
		Color modulate = Modulate;
		modulate.A = _interactionAlpha * _lifecycleAlpha;
		Modulate = modulate;
	}

	private static double NormalizeRatio(double value)
	{
		if (!double.IsFinite(value))
		{
			return 0.0;
		}
		return Mathf.Clamp(value, 0.0, 1.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Configure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Color, "bodyColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRatios, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "bodyRatio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "shieldRatio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDead, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "dead", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVertical, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "vertical", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ContainsCanvasPoint, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "canvasPoint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPointerOverlap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "overlaps", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetLifecycleAlpha, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "alpha", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCombinedAlpha, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeRatio, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Configure && args.Count == 2)
		{
			Configure(VariantUtils.ConvertTo<Texture2D>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRatios && args.Count == 2)
		{
			SetRatios(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDead && args.Count == 1)
		{
			SetDead(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetVertical && args.Count == 1)
		{
			SetVertical(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ContainsCanvasPoint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsCanvasPoint(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPointerOverlap && args.Count == 1)
		{
			SetPointerOverlap(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLifecycleAlpha && args.Count == 1)
		{
			SetLifecycleAlpha(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCombinedAlpha && args.Count == 0)
		{
			ApplyCombinedAlpha();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeRatio && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(NormalizeRatio(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NormalizeRatio && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(NormalizeRatio(VariantUtils.ConvertTo<double>(in args[0])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.Configure)
		{
			return true;
		}
		if (method == MethodName.SetRatios)
		{
			return true;
		}
		if (method == MethodName.SetDead)
		{
			return true;
		}
		if (method == MethodName.SetVertical)
		{
			return true;
		}
		if (method == MethodName.ContainsCanvasPoint)
		{
			return true;
		}
		if (method == MethodName.SetPointerOverlap)
		{
			return true;
		}
		if (method == MethodName.SetLifecycleAlpha)
		{
			return true;
		}
		if (method == MethodName.ApplyCombinedAlpha)
		{
			return true;
		}
		if (method == MethodName.NormalizeRatio)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._rotatingBody)
		{
			_rotatingBody = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._bodyProgress)
		{
			_bodyProgress = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._shieldProgress)
		{
			_shieldProgress = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._icon)
		{
			_icon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._deadCross)
		{
			_deadCross = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._interactionAlpha)
		{
			_interactionAlpha = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._targetInteractionAlpha)
		{
			_targetInteractionAlpha = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._lifecycleAlpha)
		{
			_lifecycleAlpha = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._rotatingBody)
		{
			value = VariantUtils.CreateFrom(in _rotatingBody);
			return true;
		}
		if (name == PropertyName._bodyProgress)
		{
			value = VariantUtils.CreateFrom(in _bodyProgress);
			return true;
		}
		if (name == PropertyName._shieldProgress)
		{
			value = VariantUtils.CreateFrom(in _shieldProgress);
			return true;
		}
		if (name == PropertyName._icon)
		{
			value = VariantUtils.CreateFrom(in _icon);
			return true;
		}
		if (name == PropertyName._deadCross)
		{
			value = VariantUtils.CreateFrom(in _deadCross);
			return true;
		}
		if (name == PropertyName._interactionAlpha)
		{
			value = VariantUtils.CreateFrom(in _interactionAlpha);
			return true;
		}
		if (name == PropertyName._targetInteractionAlpha)
		{
			value = VariantUtils.CreateFrom(in _targetInteractionAlpha);
			return true;
		}
		if (name == PropertyName._lifecycleAlpha)
		{
			value = VariantUtils.CreateFrom(in _lifecycleAlpha);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._rotatingBody, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bodyProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shieldProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._icon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._deadCross, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._interactionAlpha, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._targetInteractionAlpha, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._lifecycleAlpha, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._rotatingBody, Variant.From(in _rotatingBody));
		info.AddProperty(PropertyName._bodyProgress, Variant.From(in _bodyProgress));
		info.AddProperty(PropertyName._shieldProgress, Variant.From(in _shieldProgress));
		info.AddProperty(PropertyName._icon, Variant.From(in _icon));
		info.AddProperty(PropertyName._deadCross, Variant.From(in _deadCross));
		info.AddProperty(PropertyName._interactionAlpha, Variant.From(in _interactionAlpha));
		info.AddProperty(PropertyName._targetInteractionAlpha, Variant.From(in _targetInteractionAlpha));
		info.AddProperty(PropertyName._lifecycleAlpha, Variant.From(in _lifecycleAlpha));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._rotatingBody, out var value))
		{
			_rotatingBody = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._bodyProgress, out var value2))
		{
			_bodyProgress = value2.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._shieldProgress, out var value3))
		{
			_shieldProgress = value3.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._icon, out var value4))
		{
			_icon = value4.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._deadCross, out var value5))
		{
			_deadCross = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._interactionAlpha, out var value6))
		{
			_interactionAlpha = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName._targetInteractionAlpha, out var value7))
		{
			_targetInteractionAlpha = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName._lifecycleAlpha, out var value8))
		{
			_lifecycleAlpha = value8.As<float>();
		}
	}
}
