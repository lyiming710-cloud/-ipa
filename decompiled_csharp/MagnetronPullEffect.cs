using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter9/Magnetron/Effect/MagnetronPullEffect.cs")]
public class MagnetronPullEffect : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Configure = "Configure";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName UpdateVisual = "UpdateVisual";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _source = "_source";

		public static readonly StringName _target = "_target";

		public static readonly StringName _fallbackTarget = "_fallbackTarget";

		public static readonly StringName _remaining = "_remaining";

		public static readonly StringName _line = "_line";

		public static readonly StringName _pullLeft = "_pullLeft";

		public static readonly StringName _pullRight = "_pullRight";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private TowerDefenseZombieMagnetron _source;

	private TowerDefenseCharacter _target;

	private Vector2 _fallbackTarget;

	private double _remaining;

	private Sprite2D _line;

	private Node2D _pullLeft;

	private Node2D _pullRight;

	public override void _Ready()
	{
		_line = GetNode<Sprite2D>("Line");
		_pullLeft = GetNode<Node2D>("PullL");
		_pullRight = GetNode<Node2D>("PullR");
	}

	public void Configure(TowerDefenseZombieMagnetron source, TowerDefenseCharacter target, Vector2 fallbackTarget, double duration)
	{
		_source = source;
		_target = target;
		_fallbackTarget = fallbackTarget;
		_remaining = Math.Max(0.01, duration);
		UpdateVisual();
	}

	public override void _Process(double delta)
	{
		_remaining -= delta;
		if (_remaining <= 0.0 || !GodotObject.IsInstanceValid(_source))
		{
			QueueFree();
		}
		else
		{
			UpdateVisual();
		}
	}

	private void UpdateVisual()
	{
		if (IsNodeReady())
		{
			Vector2 pullOriginPosition = _source.GetPullOriginPosition();
			Vector2 vector = (GodotObject.IsInstanceValid(_target) ? _target.GetLogicalGlobalPosition() : _fallbackTarget);
			Vector2 vector2 = vector - pullOriginPosition;
			_pullRight.GlobalPosition = pullOriginPosition;
			_pullLeft.GlobalPosition = vector;
			_line.GlobalPosition = (pullOriginPosition + vector) * 0.5f;
			_line.GlobalRotation = vector2.Angle();
			_line.Scale = new Vector2(Math.Max(0.01f, vector2.Length() / 162f), 1f);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Configure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "fallbackTarget", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Configure && args.Count == 4)
		{
			Configure(VariantUtils.ConvertTo<TowerDefenseZombieMagnetron>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateVisual && args.Count == 0)
		{
			UpdateVisual();
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
		if (method == MethodName.Configure)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.UpdateVisual)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._source)
		{
			_source = VariantUtils.ConvertTo<TowerDefenseZombieMagnetron>(in value);
			return true;
		}
		if (name == PropertyName._target)
		{
			_target = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._fallbackTarget)
		{
			_fallbackTarget = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._remaining)
		{
			_remaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._line)
		{
			_line = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._pullLeft)
		{
			_pullLeft = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._pullRight)
		{
			_pullRight = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._source)
		{
			value = VariantUtils.CreateFrom(in _source);
			return true;
		}
		if (name == PropertyName._target)
		{
			value = VariantUtils.CreateFrom(in _target);
			return true;
		}
		if (name == PropertyName._fallbackTarget)
		{
			value = VariantUtils.CreateFrom(in _fallbackTarget);
			return true;
		}
		if (name == PropertyName._remaining)
		{
			value = VariantUtils.CreateFrom(in _remaining);
			return true;
		}
		if (name == PropertyName._line)
		{
			value = VariantUtils.CreateFrom(in _line);
			return true;
		}
		if (name == PropertyName._pullLeft)
		{
			value = VariantUtils.CreateFrom(in _pullLeft);
			return true;
		}
		if (name == PropertyName._pullRight)
		{
			value = VariantUtils.CreateFrom(in _pullRight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._source, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._fallbackTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._remaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._line, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pullLeft, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pullRight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._source, Variant.From(in _source));
		info.AddProperty(PropertyName._target, Variant.From(in _target));
		info.AddProperty(PropertyName._fallbackTarget, Variant.From(in _fallbackTarget));
		info.AddProperty(PropertyName._remaining, Variant.From(in _remaining));
		info.AddProperty(PropertyName._line, Variant.From(in _line));
		info.AddProperty(PropertyName._pullLeft, Variant.From(in _pullLeft));
		info.AddProperty(PropertyName._pullRight, Variant.From(in _pullRight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._source, out var value))
		{
			_source = value.As<TowerDefenseZombieMagnetron>();
		}
		if (info.TryGetProperty(PropertyName._target, out var value2))
		{
			_target = value2.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._fallbackTarget, out var value3))
		{
			_fallbackTarget = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._remaining, out var value4))
		{
			_remaining = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._line, out var value5))
		{
			_line = value5.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._pullLeft, out var value6))
		{
			_pullLeft = value6.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._pullRight, out var value7))
		{
			_pullRight = value7.As<Node2D>();
		}
	}
}
