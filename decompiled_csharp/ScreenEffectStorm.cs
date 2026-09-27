using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/ScreenEffect/Strom/ScreenEffectStorm.cs")]
public class ScreenEffectStorm : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName Storm = "Storm";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName white = "white";

		public static readonly StringName black = "black";

		public static readonly StringName blink = "blink";

		public static readonly StringName timer = "timer";

		public static readonly StringName nextTime = "nextTime";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private ColorRect white;

	private ColorRect black;

	private ColorRect blink;

	public double timer;

	public double nextTime = 5.0;

	public override void _Ready()
	{
		white = GetNodeOrNull<ColorRect>("%White");
		black = GetNodeOrNull<ColorRect>("%Black");
		blink = GetNodeOrNull<ColorRect>("%Blink");
		Color color = black.Color;
		color.A = 1f;
		black.Color = color;
		Color color2 = white.Color;
		color2.A = 0f;
		white.Color = color2;
		Color color3 = blink.Color;
		color3.A = 0f;
		blink.Color = color3;
		timer = 0.0;
		nextTime = GD.RandRange(4.0, 6.0);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (timer < nextTime)
		{
			timer += delta;
		}
		else
		{
			Storm();
		}
	}

	public async void Storm()
	{
		if (!Engine.IsEditorHint())
		{
			AudioManager.Instance.AudioPlay("Thunder");
		}
		timer = 0.0;
		nextTime = GD.RandRange(4.0, 6.0);
		Color color = black.Color;
		color.A = 0.2f;
		black.Color = color;
		Tween tween = CreateTween();
		tween.TweenProperty(white, "color:a", 0.0, 0.5).From(1.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		tween = CreateTween();
		tween.TweenProperty(black, "color:a", 1.0, 3.0).From(0.2);
		for (int i = 0; i < 5; i++)
		{
			Color color2 = blink.Color;
			color2.A = (float)GD.RandRange(0.2, 0.4);
			blink.Color = color2;
			SceneTreeTimer source = GetTree().CreateTimer(GD.RandRange(0.1, 0.15), processAlways: false);
			await ToSignal(source, SceneTreeTimer.SignalName.Timeout);
		}
		for (int i = 0; i < 5; i++)
		{
			Color color3 = blink.Color;
			color3.A = (float)GD.RandRange(0.5, 0.8);
			blink.Color = color3;
			SceneTreeTimer source2 = GetTree().CreateTimer(GD.RandRange(0.1, 0.15), processAlways: false);
			await ToSignal(source2, SceneTreeTimer.SignalName.Timeout);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Storm, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Storm && args.Count == 0)
		{
			Storm();
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
		if (method == MethodName.Storm)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.white)
		{
			white = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName.black)
		{
			black = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName.blink)
		{
			blink = VariantUtils.ConvertTo<ColorRect>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.nextTime)
		{
			nextTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.white)
		{
			value = VariantUtils.CreateFrom(in white);
			return true;
		}
		if (name == PropertyName.black)
		{
			value = VariantUtils.CreateFrom(in black);
			return true;
		}
		if (name == PropertyName.blink)
		{
			value = VariantUtils.CreateFrom(in blink);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.nextTime)
		{
			value = VariantUtils.CreateFrom(in nextTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.white, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.black, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.blink, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.nextTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.white, Variant.From(in white));
		info.AddProperty(PropertyName.black, Variant.From(in black));
		info.AddProperty(PropertyName.blink, Variant.From(in blink));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.nextTime, Variant.From(in nextTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.white, out var value))
		{
			white = value.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName.black, out var value2))
		{
			black = value2.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName.blink, out var value3))
		{
			blink = value3.As<ColorRect>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value4))
		{
			timer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.nextTime, out var value5))
		{
			nextTime = value5.As<double>();
		}
	}
}
