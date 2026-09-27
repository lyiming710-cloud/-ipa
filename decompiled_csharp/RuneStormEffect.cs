using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ScreenEffect/RuneStorm/RuneStormEffect.cs")]
public class RuneStormEffect : ColorRect
{
	public new class MethodName : ColorRect.MethodName
	{
		public static readonly StringName Setup = "Setup";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SaveState = "SaveState";

		public static readonly StringName LoadState = "LoadState";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName ApplyTexture = "ApplyTexture";

		public static readonly StringName ApplyColor = "ApplyColor";

		public static readonly StringName ApplyFade = "ApplyFade";

		public static readonly StringName ComputeFade = "ComputeFade";

		public static readonly StringName ApplyViewportSize = "ApplyViewportSize";
	}

	public new class PropertyName : ColorRect.PropertyName
	{
		public static readonly StringName damagePercentPerSecond = "damagePercentPerSecond";

		public static readonly StringName healPerSecond = "healPerSecond";

		public static readonly StringName _timer = "_timer";

		public static readonly StringName _seed = "_seed";

		public static readonly StringName _elapsed = "_elapsed";

		public static readonly StringName _tickAccumulator = "_tickAccumulator";

		public static readonly StringName _refreshAccumulator = "_refreshAccumulator";

		public static readonly StringName _duration = "_duration";

		public static readonly StringName _color = "_color";
	}

	public new class SignalName : ColorRect.SignalName
	{
	}

	private const string ColorParameter = "runeColor";

	private const string SizeParameter = "effectSize";

	private const string TimerParameter = "timer";

	private const string FadeParameter = "fade";

	private const string TextureCountParameter = "runeTextureCount";

	private const string SeedParameter = "seed";

	private const string SeedStateKey = "textureSeed";

	private static readonly string[] TextureParameters = new string[3] { "runeTexture0", "runeTexture1", "runeTexture2" };

	private const double FadeInDuration = 0.5;

	private const double FadeOutDuration = 0.7;

	private const string RuneTexturePath = "res://Asset/Texture/ScreenEffect/RuneStorm/RuneStorm";

	[Export(PropertyHint.None, "")]
	public double damagePercentPerSecond = 0.05;

	[Export(PropertyHint.None, "")]
	public double healPerSecond = 50.0;

	private double _timer;

	private double _seed = -1.0;

	private double _elapsed;

	private double _tickAccumulator;

	private double _refreshAccumulator;

	private double _duration = 5.0;

	private TowerDefenseRuneMagicColor _color;

	public void Setup(TowerDefenseRuneMagicColor color, double duration)
	{
		_color = color;
		_duration = Mathf.Max(0.1, duration);
		_seed = GD.RandRange(0.0, 1000.0);
		ApplyTexture();
		ApplyColor();
	}

	public override void _Ready()
	{
		ApplyTexture();
		ApplyColor();
		ApplyFade();
		ApplyViewportSize();
	}

	public Dictionary SaveState()
	{
		return new Dictionary
		{
			["color"] = TowerDefenseRuneMagicUtil.ToKey(_color),
			["duration"] = _duration,
			["elapsed"] = _elapsed,
			["timer"] = _timer,
			["tickAccumulator"] = _tickAccumulator,
			["refreshAccumulator"] = _refreshAccumulator,
			["textureSeed"] = _seed,
			["damagePercentPerSecond"] = damagePercentPerSecond,
			["healPerSecond"] = healPerSecond
		};
	}

	public void LoadState(Dictionary state)
	{
		TowerDefenseRuneMagicUtil.TryParse(state["color"].AsString(), out _color);
		_duration = Mathf.Max(0.1, state["duration"].AsDouble());
		_elapsed = Mathf.Clamp(state["elapsed"].AsDouble(), 0.0, _duration);
		_timer = state.GetValueOrDefault("timer", _elapsed).AsDouble();
		_tickAccumulator = Mathf.Clamp(state.GetValueOrDefault("tickAccumulator", 0.0).AsDouble(), 0.0, 1.0);
		_refreshAccumulator = Mathf.Clamp(state.GetValueOrDefault("refreshAccumulator", 0.0).AsDouble(), 0.0, 0.1);
		_seed = state.GetValueOrDefault("textureSeed", -1.0).AsDouble();
		damagePercentPerSecond = state.GetValueOrDefault("damagePercentPerSecond", damagePercentPerSecond).AsDouble();
		healPerSecond = state.GetValueOrDefault("healPerSecond", healPerSecond).AsDouble();
		ApplyTexture();
		ApplyColor();
		ApplyFade();
	}

	public override void _PhysicsProcess(double delta)
	{
		double num = Mathf.Clamp(delta, 0.0, _duration - _elapsed);
		_timer += num;
		_elapsed += num;
		ApplyFade();
		if (Material is ShaderMaterial shaderMaterial)
		{
			shaderMaterial.SetShaderParameter("timer", _timer);
		}
		_refreshAccumulator += num;
		if (_refreshAccumulator >= 0.1 && _elapsed < _duration)
		{
			_refreshAccumulator = 0.0;
			TowerDefenseRuneMagicUtil.ApplyStormContinuous(_color, _duration - _elapsed);
		}
		_tickAccumulator += num;
		while (_tickAccumulator >= 0.999999999)
		{
			_tickAccumulator--;
			TowerDefenseRuneMagicUtil.ApplyStormPerSecond(_color, damagePercentPerSecond, healPerSecond);
		}
		if (_elapsed >= _duration - 1E-09)
		{
			QueueFree();
		}
	}

	private void ApplyTexture()
	{
		if (!(Material is ShaderMaterial shaderMaterial))
		{
			return;
		}
		for (int i = 0; i < TextureParameters.Length; i++)
		{
			Texture2D texture2D = GD.Load<Texture2D>("res://Asset/Texture/ScreenEffect/RuneStorm/RuneStorm" + (i + 1) + ".png");
			if (GodotObject.IsInstanceValid(texture2D))
			{
				shaderMaterial.SetShaderParameter(TextureParameters[i], texture2D);
			}
		}
		shaderMaterial.SetShaderParameter("runeTextureCount", TextureParameters.Length);
		if (_seed < 0.0)
		{
			_seed = GD.RandRange(0.0, 1000.0);
		}
		shaderMaterial.SetShaderParameter("seed", (float)_seed);
	}

	private void ApplyColor()
	{
		if (Material is ShaderMaterial shaderMaterial)
		{
			shaderMaterial.SetShaderParameter("runeColor", TowerDefenseRuneMagicUtil.ToStormTint(_color));
		}
	}

	private void ApplyFade()
	{
		if (Material is ShaderMaterial shaderMaterial)
		{
			shaderMaterial.SetShaderParameter("fade", (float)ComputeFade());
		}
	}

	private double ComputeFade()
	{
		double num = Mathf.Min(0.5, _duration * 0.5);
		double num2 = Mathf.Min(0.7, _duration * 0.5);
		if (num > 0.0 && _elapsed < num)
		{
			return Mathf.Clamp(_elapsed / num, 0.0, 1.0);
		}
		double num3 = _duration - _elapsed;
		if (num2 > 0.0 && num3 < num2)
		{
			return Mathf.Clamp(num3 / num2, 0.0, 1.0);
		}
		return 1.0;
	}

	private void ApplyViewportSize()
	{
		if (Material is ShaderMaterial shaderMaterial)
		{
			Viewport viewport = GetViewport();
			if (GodotObject.IsInstanceValid(viewport))
			{
				shaderMaterial.SetShaderParameter("effectSize", viewport.GetVisibleRect().Size);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName.Setup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFade, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeFade, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyViewportSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Setup && args.Count == 2)
		{
			Setup(VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SaveState());
			return true;
		}
		if (method == MethodName.LoadState && args.Count == 1)
		{
			LoadState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTexture && args.Count == 0)
		{
			ApplyTexture();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyColor && args.Count == 0)
		{
			ApplyColor();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFade && args.Count == 0)
		{
			ApplyFade();
			ret = default;
			return true;
		}
		if (method == MethodName.ComputeFade && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(ComputeFade());
			return true;
		}
		if (method == MethodName.ApplyViewportSize && args.Count == 0)
		{
			ApplyViewportSize();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Setup)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SaveState)
		{
			return true;
		}
		if (method == MethodName.LoadState)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.ApplyTexture)
		{
			return true;
		}
		if (method == MethodName.ApplyColor)
		{
			return true;
		}
		if (method == MethodName.ApplyFade)
		{
			return true;
		}
		if (method == MethodName.ComputeFade)
		{
			return true;
		}
		if (method == MethodName.ApplyViewportSize)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.damagePercentPerSecond)
		{
			damagePercentPerSecond = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.healPerSecond)
		{
			healPerSecond = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._timer)
		{
			_timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._seed)
		{
			_seed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			_elapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._tickAccumulator)
		{
			_tickAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._refreshAccumulator)
		{
			_refreshAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._duration)
		{
			_duration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._color)
		{
			_color = VariantUtils.ConvertTo<TowerDefenseRuneMagicColor>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.damagePercentPerSecond)
		{
			value = VariantUtils.CreateFrom(in damagePercentPerSecond);
			return true;
		}
		if (name == PropertyName.healPerSecond)
		{
			value = VariantUtils.CreateFrom(in healPerSecond);
			return true;
		}
		if (name == PropertyName._timer)
		{
			value = VariantUtils.CreateFrom(in _timer);
			return true;
		}
		if (name == PropertyName._seed)
		{
			value = VariantUtils.CreateFrom(in _seed);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			value = VariantUtils.CreateFrom(in _elapsed);
			return true;
		}
		if (name == PropertyName._tickAccumulator)
		{
			value = VariantUtils.CreateFrom(in _tickAccumulator);
			return true;
		}
		if (name == PropertyName._refreshAccumulator)
		{
			value = VariantUtils.CreateFrom(in _refreshAccumulator);
			return true;
		}
		if (name == PropertyName._duration)
		{
			value = VariantUtils.CreateFrom(in _duration);
			return true;
		}
		if (name == PropertyName._color)
		{
			value = VariantUtils.CreateFrom(in _color);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.damagePercentPerSecond, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.healPerSecond, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._seed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._elapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._tickAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._refreshAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._duration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._color, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.damagePercentPerSecond, Variant.From(in damagePercentPerSecond));
		info.AddProperty(PropertyName.healPerSecond, Variant.From(in healPerSecond));
		info.AddProperty(PropertyName._timer, Variant.From(in _timer));
		info.AddProperty(PropertyName._seed, Variant.From(in _seed));
		info.AddProperty(PropertyName._elapsed, Variant.From(in _elapsed));
		info.AddProperty(PropertyName._tickAccumulator, Variant.From(in _tickAccumulator));
		info.AddProperty(PropertyName._refreshAccumulator, Variant.From(in _refreshAccumulator));
		info.AddProperty(PropertyName._duration, Variant.From(in _duration));
		info.AddProperty(PropertyName._color, Variant.From(in _color));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.damagePercentPerSecond, out var value))
		{
			damagePercentPerSecond = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.healPerSecond, out var value2))
		{
			healPerSecond = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._timer, out var value3))
		{
			_timer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._seed, out var value4))
		{
			_seed = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._elapsed, out var value5))
		{
			_elapsed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._tickAccumulator, out var value6))
		{
			_tickAccumulator = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._refreshAccumulator, out var value7))
		{
			_refreshAccumulator = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._duration, out var value8))
		{
			_duration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._color, out var value9))
		{
			_color = value9.As<TowerDefenseRuneMagicColor>();
		}
	}
}
