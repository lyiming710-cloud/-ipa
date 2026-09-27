using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogPopup.cs")]
public class DialogPopup : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RefreshHeadPosition = "RefreshHeadPosition";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName textLabel = "textLabel";

		public static readonly StringName ninePatchRect = "ninePatchRect";

		public static readonly StringName headTextureRect = "headTextureRect";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	protected RichTextLabel textLabel;

	private NinePatchRect ninePatchRect;

	private TextureRect headTextureRect;

	public override void _Ready()
	{
		base._Ready();
		textLabel = GetNode<RichTextLabel>("%TextLabel");
		ninePatchRect = GetNode<NinePatchRect>("%NinePatchRect");
		headTextureRect = GetNode<TextureRect>("%HeadTextureRect");
		ninePatchRect.Resized += RefreshHeadPosition;
		RefreshHeadPosition();
	}

	private void RefreshHeadPosition()
	{
		if (headTextureRect != null && ninePatchRect != null)
		{
			headTextureRect.Position = new Vector2(ninePatchRect.Size.X / 2f - 100f, -42f);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshHeadPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RefreshHeadPosition && args.Count == 0)
		{
			RefreshHeadPosition();
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
		if (method == MethodName.RefreshHeadPosition)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.textLabel)
		{
			textLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName.ninePatchRect)
		{
			ninePatchRect = VariantUtils.ConvertTo<NinePatchRect>(in value);
			return true;
		}
		if (name == PropertyName.headTextureRect)
		{
			headTextureRect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.textLabel)
		{
			value = VariantUtils.CreateFrom(in textLabel);
			return true;
		}
		if (name == PropertyName.ninePatchRect)
		{
			value = VariantUtils.CreateFrom(in ninePatchRect);
			return true;
		}
		if (name == PropertyName.headTextureRect)
		{
			value = VariantUtils.CreateFrom(in headTextureRect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.textLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ninePatchRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.headTextureRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.textLabel, Variant.From(in textLabel));
		info.AddProperty(PropertyName.ninePatchRect, Variant.From(in ninePatchRect));
		info.AddProperty(PropertyName.headTextureRect, Variant.From(in headTextureRect));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.textLabel, out var value))
		{
			textLabel = value.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName.ninePatchRect, out var value2))
		{
			ninePatchRect = value2.As<NinePatchRect>();
		}
		if (info.TryGetProperty(PropertyName.headTextureRect, out var value3))
		{
			headTextureRect = value3.As<TextureRect>();
		}
	}
}
