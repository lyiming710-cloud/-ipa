using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWEditorToaster.cs")]
public class XWEditorToaster : VBoxContainer
{
	public enum Severity
	{
		Info,
		Warning,
		Error
	}

	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ShowToast = "ShowToast";

		public static readonly StringName BuildToast = "BuildToast";

		public static readonly StringName DismissLater = "DismissLater";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName _toastVBox = "_toastVBox";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	private VBoxContainer _toastVBox;

	public override void _Ready()
	{
		_toastVBox = GetNode<VBoxContainer>("%ToastVBox");
	}

	public void ShowToast(string message, Severity severity = Severity.Info, float duration = 3f)
	{
		if (IsInsideTree() && GodotObject.IsInstanceValid(_toastVBox) && _toastVBox.IsInsideTree())
		{
			PanelContainer panelContainer = BuildToast(message, severity);
			_toastVBox.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
			DismissLater(panelContainer, duration);
		}
	}

	private PanelContainer BuildToast(string message, Severity severity)
	{
		PanelContainer panelContainer = new PanelContainer();
		Label label = new Label
		{
			Text = message
		};
		label.AddThemeFontSizeOverride("font_size", 13);
		Color color = severity switch
		{
			Severity.Warning => new Color(1f, 0.84f, 0f), 
			Severity.Error => new Color(1f, 0.35f, 0.4f), 
			_ => new Color(0.85f, 0.9f, 1f), 
		};
		label.AddThemeColorOverride("font_color", color);
		panelContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private async void DismissLater(Node toast, float delay)
	{
		if (!GodotObject.IsInstanceValid(toast) || !IsInsideTree() || !toast.IsInsideTree())
		{
			return;
		}
		SceneTree tree = GetTree();
		if (GodotObject.IsInstanceValid(tree))
		{
			SceneTreeTimer source = tree.CreateTimer(delay);
			await ToSignal(source, SceneTreeTimer.SignalName.Timeout);
			if (GodotObject.IsInstanceValid(toast) && toast.IsInsideTree())
			{
				toast.QueueFree();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowToast, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "severity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildToast, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "severity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DismissLater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "toast", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ShowToast && args.Count == 3)
		{
			ShowToast(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Severity>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildToast && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(BuildToast(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Severity>(in args[1])));
			return true;
		}
		if (method == MethodName.DismissLater && args.Count == 2)
		{
			DismissLater(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
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
		if (method == MethodName.ShowToast)
		{
			return true;
		}
		if (method == MethodName.BuildToast)
		{
			return true;
		}
		if (method == MethodName.DismissLater)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._toastVBox)
		{
			_toastVBox = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._toastVBox)
		{
			value = VariantUtils.CreateFrom(in _toastVBox);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._toastVBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._toastVBox, Variant.From(in _toastVBox));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._toastVBox, out var value))
		{
			_toastVBox = value.As<VBoxContainer>();
		}
	}
}
