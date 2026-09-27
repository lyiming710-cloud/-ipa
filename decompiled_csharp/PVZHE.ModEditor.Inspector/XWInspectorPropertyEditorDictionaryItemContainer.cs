using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Dictionary/ItemContainer/XWInspectorPropertyEditorDictionaryItemContainer.cs")]
public class XWInspectorPropertyEditorDictionaryItemContainer : VBoxContainer
{
	[Signal]
	public delegate void RemoveEventHandler(XWInspectorPropertyEditorDictionaryItemContainer item);

	public new class MethodName : VBoxContainer.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName GetKeyEditorContainer = "GetKeyEditorContainer";

		public static readonly StringName GetValueEditorContainer = "GetValueEditorContainer";

		public static readonly StringName OnRemoveButtonPressed = "OnRemoveButtonPressed";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName EntryProxy = "EntryProxy";

		public static readonly StringName _keyEditorContainer = "_keyEditorContainer";

		public static readonly StringName _valueEditorContainer = "_valueEditorContainer";

		public static readonly StringName _removeButton = "_removeButton";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
		public static readonly StringName Remove = "Remove";
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Dictionary/ItemContainer/XWInspectorPropertyEditorDictionaryItemContainer.tscn";

	public RefCounted EntryProxy;

	private PanelContainer _keyEditorContainer;

	private PanelContainer _valueEditorContainer;

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

	public static XWInspectorPropertyEditorDictionaryItemContainer Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Dictionary/ItemContainer/XWInspectorPropertyEditorDictionaryItemContainer.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorDictionaryItemContainer>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		_keyEditorContainer = GetNode<PanelContainer>("%KeyEditorContainer");
		_valueEditorContainer = GetNode<PanelContainer>("%ValueEditorContainer");
		_removeButton = GetNode<Button>("%RemoveButton");
		_removeButton.Pressed += OnRemoveButtonPressed;
	}

	public PanelContainer GetKeyEditorContainer()
	{
		return _keyEditorContainer;
	}

	public PanelContainer GetValueEditorContainer()
	{
		return _valueEditorContainer;
	}

	private void OnRemoveButtonPressed()
	{
		EmitSignal(SignalName.Remove, this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetKeyEditorContainer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValueEditorContainer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRemoveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorDictionaryItemContainer>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.GetKeyEditorContainer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(GetKeyEditorContainer());
			return true;
		}
		if (method == MethodName.GetValueEditorContainer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(GetValueEditorContainer());
			return true;
		}
		if (method == MethodName.OnRemoveButtonPressed && args.Count == 0)
		{
			OnRemoveButtonPressed();
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorDictionaryItemContainer>(Create());
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
		if (method == MethodName.GetKeyEditorContainer)
		{
			return true;
		}
		if (method == MethodName.GetValueEditorContainer)
		{
			return true;
		}
		if (method == MethodName.OnRemoveButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.EntryProxy)
		{
			EntryProxy = VariantUtils.ConvertTo<RefCounted>(in value);
			return true;
		}
		if (name == PropertyName._keyEditorContainer)
		{
			_keyEditorContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._valueEditorContainer)
		{
			_valueEditorContainer = VariantUtils.ConvertTo<PanelContainer>(in value);
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
		if (name == PropertyName.EntryProxy)
		{
			value = VariantUtils.CreateFrom(in EntryProxy);
			return true;
		}
		if (name == PropertyName._keyEditorContainer)
		{
			value = VariantUtils.CreateFrom(in _keyEditorContainer);
			return true;
		}
		if (name == PropertyName._valueEditorContainer)
		{
			value = VariantUtils.CreateFrom(in _valueEditorContainer);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.EntryProxy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._keyEditorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueEditorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.EntryProxy, Variant.From(in EntryProxy));
		info.AddProperty(PropertyName._keyEditorContainer, Variant.From(in _keyEditorContainer));
		info.AddProperty(PropertyName._valueEditorContainer, Variant.From(in _valueEditorContainer));
		info.AddProperty(PropertyName._removeButton, Variant.From(in _removeButton));
		info.AddSignalEventDelegate(SignalName.Remove, backing_Remove);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.EntryProxy, out var value))
		{
			EntryProxy = value.As<RefCounted>();
		}
		if (info.TryGetProperty(PropertyName._keyEditorContainer, out var value2))
		{
			_keyEditorContainer = value2.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._valueEditorContainer, out var value3))
		{
			_valueEditorContainer = value3.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._removeButton, out var value4))
		{
			_removeButton = value4.As<Button>();
		}
		if (info.TryGetSignalEventDelegate<RemoveEventHandler>(SignalName.Remove, out var value5))
		{
			backing_Remove = value5;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.Remove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null)
		};
	}

	protected void EmitSignalRemove(XWInspectorPropertyEditorDictionaryItemContainer item)
	{
		EmitSignal(SignalName.Remove, new ReadOnlySpan<Variant>((Variant)item));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.Remove && args.Count == 1)
		{
			backing_Remove?.Invoke(VariantUtils.ConvertTo<XWInspectorPropertyEditorDictionaryItemContainer>(in args[0]));
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
