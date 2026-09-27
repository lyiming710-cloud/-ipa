using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/GUI/Button/MainButton.cs")]
public class MainButton : TextureButton
{
	public new class MethodName : TextureButton.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshLayout = "RefreshLayout";

		public static readonly StringName _ButtonPressed = "_ButtonPressed";

		public static readonly StringName OnTextLabel2Resized = "OnTextLabel2Resized";
	}

	public new class PropertyName : TextureButton.PropertyName
	{
		public static readonly StringName text = "text";

		public static readonly StringName label = "label";

		public static readonly StringName _text = "_text";
	}

	public new class SignalName : TextureButton.SignalName
	{
	}

	public Label label;

	private string _text = "Test";

	[Export(PropertyHint.None, "")]
	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			if (!(_text == value))
			{
				_text = value ?? string.Empty;
				RefreshLayout();
			}
		}
	}

	public override void _Ready()
	{
		label = GetNode<Label>("%TextLabel");
		label.Text = _text;
		Pressed += _ButtonPressed;
		Resized += RefreshLayout;
		Label nodeOrNull = GetNodeOrNull<Label>("TextLabel2");
		if (nodeOrNull != null)
		{
			nodeOrNull.Resized += OnTextLabel2Resized;
		}
		RefreshLayout();
	}

	private void RefreshLayout()
	{
		if (GodotObject.IsInstanceValid(label))
		{
			label.Text = _text;
			label.Size = Size - new Vector2(20f, 0f);
			label.Position = new Vector2(10f, 0f);
			OnTextLabel2Resized();
		}
	}

	public void _ButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
	}

	public void OnTextLabel2Resized()
	{
		Label nodeOrNull = GetNodeOrNull<Label>("TextLabel2");
		if (nodeOrNull != null)
		{
			nodeOrNull.Size = new Vector2(Size.X - 18f, nodeOrNull.Size.Y);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTextLabel2Resized, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RefreshLayout && args.Count == 0)
		{
			RefreshLayout();
			ret = default;
			return true;
		}
		if (method == MethodName._ButtonPressed && args.Count == 0)
		{
			_ButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextLabel2Resized && args.Count == 0)
		{
			OnTextLabel2Resized();
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
		if (method == MethodName.RefreshLayout)
		{
			return true;
		}
		if (method == MethodName._ButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnTextLabel2Resized)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.text)
		{
			text = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.label)
		{
			label = VariantUtils.ConvertTo<Label>(in value);
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
		if (name == PropertyName.text)
		{
			value = VariantUtils.CreateFrom<string>(text);
			return true;
		}
		if (name == PropertyName.label)
		{
			value = VariantUtils.CreateFrom(in label);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.label, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._text, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.text, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.text, Variant.From<string>(text));
		info.AddProperty(PropertyName.label, Variant.From(in label));
		info.AddProperty(PropertyName._text, Variant.From(in _text));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.text, out var value))
		{
			text = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.label, out var value2))
		{
			label = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._text, out var value3))
		{
			_text = value3.As<string>();
		}
	}
}
