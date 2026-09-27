using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ProjectileEffect/MagicPeaBlow/TowerDefenseProjectileEffectMagicPeaBlow.cs")]
public class TowerDefenseProjectileEffectMagicPeaBlow : TowerDefenseProjectileEffectBase
{
	public new class MethodName : TowerDefenseProjectileEffectBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName PlaySpread = "PlaySpread";

		public static readonly StringName FinishSpread = "FinishSpread";
	}

	public new class PropertyName : TowerDefenseProjectileEffectBase.PropertyName
	{
		public static readonly StringName duration = "duration";

		public static readonly StringName startScale = "startScale";

		public static readonly StringName endScale = "endScale";

		public static readonly StringName spriteNodeName = "spriteNodeName";

		public static readonly StringName _blowSprite = "_blowSprite";

		public static readonly StringName _started = "_started";
	}

	public new class SignalName : TowerDefenseProjectileEffectBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public float duration = 0.5f;

	[Export(PropertyHint.None, "")]
	public float startScale = 0.25f;

	[Export(PropertyHint.None, "")]
	public float endScale = 1.25f;

	[Export(PropertyHint.None, "")]
	public string spriteNodeName = "Blow";

	private Sprite2D _blowSprite;

	private bool _started;

	public override void _Ready()
	{
		_blowSprite = GetNodeOrNull<Sprite2D>(spriteNodeName);
		if (!GodotObject.IsInstanceValid(_blowSprite))
		{
			_blowSprite = GetNodeOrNull<Sprite2D>("Blow");
		}
		if (!GodotObject.IsInstanceValid(_blowSprite))
		{
			QueueFree();
		}
		else
		{
			PlaySpread();
		}
	}

	private void PlaySpread()
	{
		if (!_started)
		{
			_started = true;
			Vector2 vector = Vector2.One * Mathf.Max(0.001f, startScale);
			Vector2 vector2 = Vector2.One * Mathf.Max(0.001f, endScale);
			float num = Mathf.Max(0.01f, duration);
			_blowSprite.Scale = vector;
			_blowSprite.Modulate = new Color(1f, 1f, 1f);
			Tween tween = CreateTween();
			tween.SetParallel();
			tween.TweenProperty(_blowSprite, "scale", vector2, num).From(vector);
			tween.TweenProperty(_blowSprite, "modulate:a", 0f, num).From(1f);
			tween.Chain().TweenCallback(Callable.From(FinishSpread));
		}
	}

	private void FinishSpread()
	{
		QueueFree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlaySpread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishSpread, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PlaySpread && args.Count == 0)
		{
			PlaySpread();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishSpread && args.Count == 0)
		{
			FinishSpread();
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
		if (method == MethodName.PlaySpread)
		{
			return true;
		}
		if (method == MethodName.FinishSpread)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.duration)
		{
			duration = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.startScale)
		{
			startScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.endScale)
		{
			endScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.spriteNodeName)
		{
			spriteNodeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._blowSprite)
		{
			_blowSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._started)
		{
			_started = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.duration)
		{
			value = VariantUtils.CreateFrom(in duration);
			return true;
		}
		if (name == PropertyName.startScale)
		{
			value = VariantUtils.CreateFrom(in startScale);
			return true;
		}
		if (name == PropertyName.endScale)
		{
			value = VariantUtils.CreateFrom(in endScale);
			return true;
		}
		if (name == PropertyName.spriteNodeName)
		{
			value = VariantUtils.CreateFrom(in spriteNodeName);
			return true;
		}
		if (name == PropertyName._blowSprite)
		{
			value = VariantUtils.CreateFrom(in _blowSprite);
			return true;
		}
		if (name == PropertyName._started)
		{
			value = VariantUtils.CreateFrom(in _started);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.duration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.startScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.endScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.spriteNodeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._blowSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._started, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.duration, Variant.From(in duration));
		info.AddProperty(PropertyName.startScale, Variant.From(in startScale));
		info.AddProperty(PropertyName.endScale, Variant.From(in endScale));
		info.AddProperty(PropertyName.spriteNodeName, Variant.From(in spriteNodeName));
		info.AddProperty(PropertyName._blowSprite, Variant.From(in _blowSprite));
		info.AddProperty(PropertyName._started, Variant.From(in _started));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.duration, out var value))
		{
			duration = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName.startScale, out var value2))
		{
			startScale = value2.As<float>();
		}
		if (info.TryGetProperty(PropertyName.endScale, out var value3))
		{
			endScale = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.spriteNodeName, out var value4))
		{
			spriteNodeName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._blowSprite, out var value5))
		{
			_blowSprite = value5.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._started, out var value6))
		{
			_started = value6.As<bool>();
		}
	}
}
