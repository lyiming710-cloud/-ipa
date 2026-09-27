using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Extends/Sprite2D/Sprite2DEffect.cs")]
public class Sprite2DEffect : Sprite2D
{
	public new class MethodName : Sprite2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";
	}

	public new class PropertyName : Sprite2D.PropertyName
	{
		public static readonly StringName shake = "shake";

		public static readonly StringName shakeInterval = "shakeInterval";

		public static readonly StringName shakeSpread = "shakeSpread";

		public static readonly StringName modulateRandom = "modulateRandom";

		public static readonly StringName modulateRamdomInterval = "modulateRamdomInterval";

		public static readonly StringName modulateRamdomGradient = "modulateRamdomGradient";

		public static readonly StringName modulateBlink = "modulateBlink";

		public static readonly StringName modulateBlinkSpeed = "modulateBlinkSpeed";

		public static readonly StringName _shakeTime = "_shakeTime";

		public static readonly StringName _modulateRandomTime = "_modulateRandomTime";

		public static readonly StringName _modulateBlinkTime = "_modulateBlinkTime";
	}

	public new class SignalName : Sprite2D.SignalName
	{
	}

	private double _shakeTime;

	private double _modulateRandomTime;

	private double _modulateBlinkTime;

	[Export(PropertyHint.None, "")]
	public bool shake { get; set; }

	[Export(PropertyHint.None, "")]
	public double shakeInterval { get; set; } = 0.05;

	[Export(PropertyHint.None, "")]
	public Vector2 shakeSpread { get; set; } = new Vector2(2f, 2f);

	[Export(PropertyHint.None, "")]
	public bool modulateRandom { get; set; }

	[Export(PropertyHint.None, "")]
	public double modulateRamdomInterval { get; set; } = 5.0;

	[Export(PropertyHint.None, "")]
	public Gradient modulateRamdomGradient { get; set; }

	[Export(PropertyHint.None, "")]
	public bool modulateBlink { get; set; }

	[Export(PropertyHint.None, "")]
	public double modulateBlinkSpeed { get; set; } = 1.0;

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			SetPhysicsProcess(shake || modulateRandom || modulateBlink);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (shake)
		{
			_shakeTime += delta;
			if (_shakeTime > shakeInterval)
			{
				Offset = new Vector2((float)GD.RandRange(0f - shakeSpread.X, shakeSpread.X), (float)GD.RandRange(0f - shakeSpread.Y, shakeSpread.Y));
				_shakeTime -= shakeInterval;
			}
		}
		if (modulateRandom)
		{
			_modulateRandomTime += delta;
			if (_modulateRandomTime > modulateRamdomInterval)
			{
				Modulate = modulateRamdomGradient.Sample((float)GD.RandRange(0.0, 1.0));
				_modulateRandomTime -= modulateRamdomInterval;
			}
		}
		if (modulateBlink)
		{
			_modulateBlinkTime += delta * modulateBlinkSpeed;
			Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, Mathf.Abs(Mathf.Sin((float)_modulateBlinkTime)));
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
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
		if (name == PropertyName.shake)
		{
			shake = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shakeInterval)
		{
			shakeInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.shakeSpread)
		{
			shakeSpread = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.modulateRandom)
		{
			modulateRandom = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.modulateRamdomInterval)
		{
			modulateRamdomInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.modulateRamdomGradient)
		{
			modulateRamdomGradient = VariantUtils.ConvertTo<Gradient>(in value);
			return true;
		}
		if (name == PropertyName.modulateBlink)
		{
			modulateBlink = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.modulateBlinkSpeed)
		{
			modulateBlinkSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._shakeTime)
		{
			_shakeTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._modulateRandomTime)
		{
			_modulateRandomTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._modulateBlinkTime)
		{
			_modulateBlinkTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.shake)
		{
			from = shake;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		double from2;
		if (name == PropertyName.shakeInterval)
		{
			from2 = shakeInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.shakeSpread)
		{
			value = VariantUtils.CreateFrom<Vector2>(shakeSpread);
			return true;
		}
		if (name == PropertyName.modulateRandom)
		{
			from = modulateRandom;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.modulateRamdomInterval)
		{
			from2 = modulateRamdomInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.modulateRamdomGradient)
		{
			value = VariantUtils.CreateFrom<Gradient>(modulateRamdomGradient);
			return true;
		}
		if (name == PropertyName.modulateBlink)
		{
			from = modulateBlink;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.modulateBlinkSpeed)
		{
			from2 = modulateBlinkSpeed;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._shakeTime)
		{
			value = VariantUtils.CreateFrom(in _shakeTime);
			return true;
		}
		if (name == PropertyName._modulateRandomTime)
		{
			value = VariantUtils.CreateFrom(in _modulateRandomTime);
			return true;
		}
		if (name == PropertyName._modulateBlinkTime)
		{
			value = VariantUtils.CreateFrom(in _modulateBlinkTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.shake, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.shakeInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.shakeSpread, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._shakeTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.modulateRandom, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.modulateRamdomInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.modulateRamdomGradient, PropertyHint.ResourceType, "Gradient", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._modulateRandomTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.modulateBlink, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.modulateBlinkSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._modulateBlinkTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shake, Variant.From<bool>(shake));
		info.AddProperty(PropertyName.shakeInterval, Variant.From<double>(shakeInterval));
		info.AddProperty(PropertyName.shakeSpread, Variant.From<Vector2>(shakeSpread));
		info.AddProperty(PropertyName.modulateRandom, Variant.From<bool>(modulateRandom));
		info.AddProperty(PropertyName.modulateRamdomInterval, Variant.From<double>(modulateRamdomInterval));
		info.AddProperty(PropertyName.modulateRamdomGradient, Variant.From<Gradient>(modulateRamdomGradient));
		info.AddProperty(PropertyName.modulateBlink, Variant.From<bool>(modulateBlink));
		info.AddProperty(PropertyName.modulateBlinkSpeed, Variant.From<double>(modulateBlinkSpeed));
		info.AddProperty(PropertyName._shakeTime, Variant.From(in _shakeTime));
		info.AddProperty(PropertyName._modulateRandomTime, Variant.From(in _modulateRandomTime));
		info.AddProperty(PropertyName._modulateBlinkTime, Variant.From(in _modulateBlinkTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shake, out var value))
		{
			shake = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shakeInterval, out var value2))
		{
			shakeInterval = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.shakeSpread, out var value3))
		{
			shakeSpread = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.modulateRandom, out var value4))
		{
			modulateRandom = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.modulateRamdomInterval, out var value5))
		{
			modulateRamdomInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.modulateRamdomGradient, out var value6))
		{
			modulateRamdomGradient = value6.As<Gradient>();
		}
		if (info.TryGetProperty(PropertyName.modulateBlink, out var value7))
		{
			modulateBlink = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.modulateBlinkSpeed, out var value8))
		{
			modulateBlinkSpeed = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._shakeTime, out var value9))
		{
			_shakeTime = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._modulateRandomTime, out var value10))
		{
			_modulateRandomTime = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._modulateBlinkTime, out var value11))
		{
			_modulateBlinkTime = value11.As<double>();
		}
	}
}
