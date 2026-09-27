using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/ScreenEffect/RuneStorm/RuneWeatherNotice.cs")]
public class RuneWeatherNotice : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName Show = "Show";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName PositionBanner = "PositionBanner";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _banner = "_banner";

		public static readonly StringName _remaining = "_remaining";

		public static readonly StringName _text = "_text";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private Control _banner;

	private double _remaining;

	private string _text;

	public static void Show(TowerDefenseControlNew control, string key, double seconds)
	{
		if (GodotObject.IsInstanceValid(control) && control.IsInsideTree() && GodotObject.IsInstanceValid(BroadCastManager.Instance?.broad))
		{
			control.FindChild("RuneWeatherNotice", recursive: true, owned: false)?.QueueFree();
			control.AddUI(new RuneWeatherNotice
			{
				Name = "RuneWeatherNotice",
				_text = TranslationServer.Translate(key),
				_remaining = Mathf.Max(0.01, seconds),
				MouseFilter = MouseFilterEnum.Ignore
			}, 49);
		}
	}

	public override void _Ready()
	{
		SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect, LayoutPresetMode.Minsize);
		_banner = (Control)BroadCastManager.Instance.broad.Duplicate(0);
		_banner.Visible = true;
		AddChild(_banner, forceReadableName: false, InternalMode.Disabled);
		_banner.GetNode<Label>("BroadCastLabel").Text = _text;
		PositionBanner();
	}

	public override void _PhysicsProcess(double delta)
	{
		PositionBanner();
		_remaining -= delta;
		if (_remaining <= 0.0)
		{
			QueueFree();
		}
	}

	private void PositionBanner()
	{
		Control control = BroadCastManager.Instance?.broad;
		if (GodotObject.IsInstanceValid(control) && GodotObject.IsInstanceValid(_banner))
		{
			float num = (control.Visible ? (control.Size.Y + 8f) : 0f);
			_banner.OffsetTop = control.OffsetTop - num;
			_banner.OffsetBottom = control.OffsetBottom - num;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Show, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PositionBanner, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Show && args.Count == 3)
		{
			Show(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.PositionBanner && args.Count == 0)
		{
			PositionBanner();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Show && args.Count == 3)
		{
			Show(VariantUtils.ConvertTo<TowerDefenseControlNew>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Show)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.PositionBanner)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._banner)
		{
			_banner = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._remaining)
		{
			_remaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._text)
		{
			_text = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._banner)
		{
			value = VariantUtils.CreateFrom(in _banner);
			return true;
		}
		if (name == PropertyName._remaining)
		{
			value = VariantUtils.CreateFrom(in _remaining);
			return true;
		}
		if (name == PropertyName._text)
		{
			value = VariantUtils.CreateFrom(in _text);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._banner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._remaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._text, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._banner, Variant.From(in _banner));
		info.AddProperty(PropertyName._remaining, Variant.From(in _remaining));
		info.AddProperty(PropertyName._text, Variant.From(in _text));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._banner, out var value))
		{
			_banner = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._remaining, out var value2))
		{
			_remaining = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._text, out var value3))
		{
			_text = value3.As<string>();
		}
	}
}
