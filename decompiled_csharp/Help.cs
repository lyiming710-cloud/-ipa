using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/Help/Help.cs")]
public class Help : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName helpTexture = "helpTexture";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private static Texture2D _helpMobile;

	private TextureRect helpTexture;

	private static Texture2D HelpMobile => _helpMobile ?? (_helpMobile = GD.Load<Texture2D>("res://Asset/Texture/GUI/Help/HelpMobile.png"));

	public override void _Ready()
	{
		base._Ready();
		helpTexture = GetNode<TextureRect>("%HelpTexture");
		GetNode<NinePatchButtonBase>("%BackButton").OnPressed += BackButtonPressed;
		if (Global.Instance.isMobile)
		{
			helpTexture.Texture = HelpMobile;
		}
	}

	public void BackButtonPressed()
	{
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
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
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.helpTexture)
		{
			helpTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.helpTexture)
		{
			value = VariantUtils.CreateFrom(in helpTexture);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.helpTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.helpTexture, Variant.From(in helpTexture));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.helpTexture, out var value))
		{
			helpTexture = value.As<TextureRect>();
		}
	}
}
