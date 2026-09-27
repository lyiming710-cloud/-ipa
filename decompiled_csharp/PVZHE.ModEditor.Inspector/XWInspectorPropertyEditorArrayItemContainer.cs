using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Array/ItemContainer/XWInspectorPropertyEditorArrayItemContainer.cs")]
public class XWInspectorPropertyEditorArrayItemContainer : HBoxContainer
{
	[Signal]
	public delegate void RemoveEventHandler(XWInspectorPropertyEditorArrayItemContainer item);

	public new class MethodName : HBoxContainer.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddEditor = "AddEditor";

		public static readonly StringName RemoveButtonPressed = "RemoveButtonPressed";
	}

	public new class PropertyName : HBoxContainer.PropertyName
	{
		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName _removeButton = "_removeButton";
	}

	public new class SignalName : HBoxContainer.SignalName
	{
		public static readonly StringName Remove = "Remove";
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Array/ItemContainer/XWInspectorPropertyEditorArrayItemContainer.tscn";

	private PanelContainer _editorContainer;

	private Button _removeButton;

	private RemoveEventHandler backing_Remove;

	public event RemoveEventHandler Remove
	{
		add
		{
			backing_Remove = (RemoveEventHandler)Delegate.Combine(backing_Remove, value);
		}
		remove
		{
			backing_Remove = (RemoveEventHandler)Delegate.Remove(backing_Remove, value);
		}
	}

	public static XWInspectorPropertyEditorArrayItemContainer Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Array/ItemContainer/XWInspectorPropertyEditorArrayItemContainer.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorArrayItemContainer>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		_editorContainer = GetNode<PanelContainer>("%EditorContainer");
		_removeButton = GetNode<Button>("%RemoveButton");
		_removeButton.Pressed += RemoveButtonPressed;
	}

	public void AddEditor(XWInspectorPropertyEditorBase editor)
	{
		_editorContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
	}

	private void RemoveButtonPressed()
	{
		EmitSignal(SignalName.Remove, this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorArrayItemContainer>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.AddEditor && args.Count == 1)
		{
			AddEditor(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveButtonPressed && args.Count == 0)
		{
			RemoveButtonPressed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorArrayItemContainer>(Create());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.AddEditor)
		{
			return true;
		}
		if (method == MethodName.RemoveButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editorContainer)
		{
			_editorContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._removeButton)
		{
			_removeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editorContainer)
		{
			value = VariantUtils.CreateFrom(in _editorContainer);
			return true;
		}
		if (name == PropertyName._removeButton)
		{
			value = VariantUtils.CreateFrom(in _removeButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editorContainer, Variant.From(in _editorContainer));
		info.AddProperty(PropertyName._removeButton, Variant.From(in _removeButton));
		info.AddSignalEventDelegate(SignalName.Remove, backing_Remove);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editorContainer, out var value))
		{
			_editorContainer = value.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._removeButton, out var value2))
		{
			_removeButton = value2.As<Button>();
		}
		if (info.TryGetSignalEventDelegate<RemoveEventHandler>(SignalName.Remove, out var value3))
		{
			backing_Remove = value3;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false)
			}, null)
		};
	}

	protected void EmitSignalRemove(XWInspectorPropertyEditorArrayItemContainer item)
	{
		EmitSignal(SignalName.Remove, new ReadOnlySpan<Variant>((Variant)item));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.Remove && args.Count == 1)
		{
			backing_Remove?.Invoke(VariantUtils.ConvertTo<XWInspectorPropertyEditorArrayItemContainer>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.Remove)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
