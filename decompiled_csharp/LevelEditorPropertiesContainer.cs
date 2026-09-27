using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Container/LevelEditorPropertiesContainer.cs")]
public class LevelEditorPropertiesContainer : VBoxContainer
{
	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName restValue = "restValue";

		public static readonly StringName innerContainer = "innerContainer";

		public static readonly StringName outerContainer = "outerContainer";

		public static readonly StringName keyLabel = "keyLabel";

		public static readonly StringName refreshButton = "refreshButton";

		public static readonly StringName propertyEditor = "propertyEditor";

		public static readonly StringName _restValue = "_restValue";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	internal HBoxContainer innerContainer;

	internal VBoxContainer outerContainer;

	internal Label keyLabel;

	internal Button refreshButton;

	public PropertiesBase propertyEditor;

	private Variant _restValue;

	public Variant restValue
	{
		get
		{
			return _restValue;
		}
		set
		{
			_restValue = value;
			if (refreshButton != null)
			{
				refreshButton.Visible = _restValue.VariantType != Variant.Type.Nil;
			}
		}
	}

	public override void _Ready()
	{
		innerContainer = GetNode<HBoxContainer>("%InnerContainer");
		outerContainer = GetNode<VBoxContainer>("%OuterContainer");
		keyLabel = GetNode<Label>("%KeyLabel");
		refreshButton = GetNode<Button>("%RefreshButton");
	}

	public override void _Process(double delta)
	{
		if (restValue.VariantType != Variant.Type.Nil && propertyEditor != null)
		{
			refreshButton.Visible = !propertyEditor.GetValue().Equals(restValue);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.restValue)
		{
			restValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.innerContainer)
		{
			innerContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.outerContainer)
		{
			outerContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.keyLabel)
		{
			keyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.refreshButton)
		{
			refreshButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.propertyEditor)
		{
			propertyEditor = VariantUtils.ConvertTo<PropertiesBase>(in value);
			return true;
		}
		if (name == PropertyName._restValue)
		{
			_restValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.restValue)
		{
			value = VariantUtils.CreateFrom<Variant>(restValue);
			return true;
		}
		if (name == PropertyName.innerContainer)
		{
			value = VariantUtils.CreateFrom(in innerContainer);
			return true;
		}
		if (name == PropertyName.outerContainer)
		{
			value = VariantUtils.CreateFrom(in outerContainer);
			return true;
		}
		if (name == PropertyName.keyLabel)
		{
			value = VariantUtils.CreateFrom(in keyLabel);
			return true;
		}
		if (name == PropertyName.refreshButton)
		{
			value = VariantUtils.CreateFrom(in refreshButton);
			return true;
		}
		if (name == PropertyName.propertyEditor)
		{
			value = VariantUtils.CreateFrom(in propertyEditor);
			return true;
		}
		if (name == PropertyName._restValue)
		{
			value = VariantUtils.CreateFrom(in _restValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.innerContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.outerContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.keyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.refreshButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propertyEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._restValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.restValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.restValue, Variant.From<Variant>(restValue));
		info.AddProperty(PropertyName.innerContainer, Variant.From(in innerContainer));
		info.AddProperty(PropertyName.outerContainer, Variant.From(in outerContainer));
		info.AddProperty(PropertyName.keyLabel, Variant.From(in keyLabel));
		info.AddProperty(PropertyName.refreshButton, Variant.From(in refreshButton));
		info.AddProperty(PropertyName.propertyEditor, Variant.From(in propertyEditor));
		info.AddProperty(PropertyName._restValue, Variant.From(in _restValue));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.restValue, out var value))
		{
			restValue = value.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.innerContainer, out var value2))
		{
			innerContainer = value2.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.outerContainer, out var value3))
		{
			outerContainer = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.keyLabel, out var value4))
		{
			keyLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.refreshButton, out var value5))
		{
			refreshButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.propertyEditor, out var value6))
		{
			propertyEditor = value6.As<PropertiesBase>();
		}
		if (info.TryGetProperty(PropertyName._restValue, out var value7))
		{
			_restValue = value7.As<Variant>();
		}
	}
}
