using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationVisualChoiceCard.cs")]
public class XWAnimationVisualChoiceCard : Button
{
	public new class MethodName : Button.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Configure = "Configure";

		public static readonly StringName SetSelected = "SetSelected";

		public static readonly StringName OnPressed = "OnPressed";

		public static readonly StringName CreateStyle = "CreateStyle";
	}

	public new class PropertyName : Button.PropertyName
	{
		public static readonly StringName ChoiceId = "ChoiceId";
	}

	public new class SignalName : Button.SignalName
	{
	}

	public int ChoiceId { get; private set; } = -1;

	public event Action<int> ChoicePressed;

	public override void _Ready()
	{
		Pressed += OnPressed;
	}

	public override void _ExitTree()
	{
		Pressed -= OnPressed;
		base._ExitTree();
	}

	public void Configure(int choiceId, string title, string subtitle, Texture2D icon, Color accent)
	{
		ChoiceId = choiceId;
		Text = (string.IsNullOrWhiteSpace(subtitle) ? title : (title + "\n" + subtitle));
		TooltipText = Text.Replace('\n', ' ');
		Icon = icon;
		AddThemeConstantOverride("icon_max_width", 58);
		AddThemeColorOverride("font_color", new Color(0.9f, 0.95f, 0.88f));
		AddThemeColorOverride("font_hover_color", Colors.White);
		AddThemeColorOverride("font_pressed_color", Colors.White);
		AddThemeStyleboxOverride("normal", CreateStyle(accent, 0.13f, 0.42f));
		AddThemeStyleboxOverride("hover", CreateStyle(accent, 0.22f, 0.72f));
		AddThemeStyleboxOverride("pressed", CreateStyle(accent, 0.34f, 0.94f));
		AddThemeStyleboxOverride("focus", CreateStyle(accent, 0.28f, 1f));
	}

	public void SetSelected(bool selected)
	{
		ButtonPressed = selected;
		Modulate = (selected ? Colors.White : new Color(0.88f, 0.92f, 0.86f, 0.92f));
	}

	private void OnPressed()
	{
		ChoicePressed?.Invoke(ChoiceId);
	}

	private static StyleBoxFlat CreateStyle(Color accent, float fillAlpha, float borderAlpha)
	{
		return new StyleBoxFlat
		{
			BgColor = new Color(accent.R * 0.42f, accent.G * 0.42f, accent.B * 0.42f, fillAlpha),
			BorderColor = new Color(accent.R, accent.G, accent.B, borderAlpha),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = 7,
			CornerRadiusTopRight = 7,
			CornerRadiusBottomLeft = 7,
			CornerRadiusBottomRight = 7,
			ContentMarginLeft = 8f,
			ContentMarginTop = 6f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 6f
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Configure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "choiceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Color, "accent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "accent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fillAlpha", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "borderAlpha", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.Configure && args.Count == 5)
		{
			Configure(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Texture2D>(in args[3]), VariantUtils.ConvertTo<Color>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSelected && args.Count == 1)
		{
			SetSelected(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPressed && args.Count == 0)
		{
			OnPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreateStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreateStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.Configure)
		{
			return true;
		}
		if (method == MethodName.SetSelected)
		{
			return true;
		}
		if (method == MethodName.OnPressed)
		{
			return true;
		}
		if (method == MethodName.CreateStyle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ChoiceId)
		{
			ChoiceId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ChoiceId)
		{
			value = VariantUtils.CreateFrom<int>(ChoiceId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ChoiceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ChoiceId, Variant.From<int>(ChoiceId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ChoiceId, out var value))
		{
			ChoiceId = value.As<int>();
		}
	}
}
