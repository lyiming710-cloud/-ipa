using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/IceCap/TowerDefenseIceCap.cs")]
public class TowerDefenseIceCap : TowerDefenseGroundItemBase
{
	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName length = "length";

		public static readonly StringName iceSprite = "iceSprite";

		public static readonly StringName iceCapSprite = "iceCapSprite";

		public static readonly StringName _length = "_length";

		public static readonly StringName clearTime = "clearTime";

		public static readonly StringName clearTimer = "clearTimer";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private Sprite2D iceSprite;

	public Sprite2D iceCapSprite;

	private double _length;

	[Export(PropertyHint.None, "")]
	public double clearTime = 90.0;

	public double clearTimer;

	[Export(PropertyHint.None, "")]
	public double length
	{
		get
		{
			return _length;
		}
		set
		{
			_length = value;
			clearTimer = clearTime;
		}
	}

	public override void _Ready()
	{
		iceSprite = GetNode<Sprite2D>("%IceSprite");
		iceCapSprite = GetNode<Sprite2D>("%IceCapSprite");
		if (!Engine.IsEditorHint())
		{
			iceSprite.Material = (Material)iceSprite.Material.Duplicate();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			iceSprite.Scale = new Vector2(1f + (float)length / 150f, iceSprite.Scale.Y);
			iceCapSprite.Position = new Vector2(0f - (float)length, iceCapSprite.Position.Y);
			(iceSprite.Material as ShaderMaterial).SetShaderParameter("iceLength", 150.0 + length);
			if (clearTime > 0.0)
			{
				clearTime -= delta;
			}
			else
			{
				QueueFree();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.length)
		{
			length = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.iceSprite)
		{
			iceSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.iceCapSprite)
		{
			iceCapSprite = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._length)
		{
			_length = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.clearTime)
		{
			clearTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.clearTimer)
		{
			clearTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.length)
		{
			value = VariantUtils.CreateFrom<double>(length);
			return true;
		}
		if (name == PropertyName.iceSprite)
		{
			value = VariantUtils.CreateFrom(in iceSprite);
			return true;
		}
		if (name == PropertyName.iceCapSprite)
		{
			value = VariantUtils.CreateFrom(in iceCapSprite);
			return true;
		}
		if (name == PropertyName._length)
		{
			value = VariantUtils.CreateFrom(in _length);
			return true;
		}
		if (name == PropertyName.clearTime)
		{
			value = VariantUtils.CreateFrom(in clearTime);
			return true;
		}
		if (name == PropertyName.clearTimer)
		{
			value = VariantUtils.CreateFrom(in clearTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.iceSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.iceCapSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._length, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.length, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.clearTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.clearTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.length, Variant.From<double>(length));
		info.AddProperty(PropertyName.iceSprite, Variant.From(in iceSprite));
		info.AddProperty(PropertyName.iceCapSprite, Variant.From(in iceCapSprite));
		info.AddProperty(PropertyName._length, Variant.From(in _length));
		info.AddProperty(PropertyName.clearTime, Variant.From(in clearTime));
		info.AddProperty(PropertyName.clearTimer, Variant.From(in clearTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.length, out var value))
		{
			length = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.iceSprite, out var value2))
		{
			iceSprite = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.iceCapSprite, out var value3))
		{
			iceCapSprite = value3.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._length, out var value4))
		{
			_length = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.clearTime, out var value5))
		{
			clearTime = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.clearTimer, out var value6))
		{
			clearTimer = value6.As<double>();
		}
	}
}
