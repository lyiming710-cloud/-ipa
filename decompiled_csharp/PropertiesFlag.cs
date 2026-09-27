using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Flag/PropertiesFlag.cs")]
public class PropertiesFlag : PropertiesBase
{
	public delegate void ValueChangedEventHandler(int value);

	public new class MethodName : PropertiesBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetHintDictionary = "SetHintDictionary";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName ValueChange = "ValueChange";

		public new static readonly StringName CanSetValue = "CanSetValue";

		public new static readonly StringName _GetType = "_GetType";
	}

	public new class PropertyName : PropertiesBase.PropertyName
	{
		public static readonly StringName _checkBoxContainer = "_checkBoxContainer";

		public static readonly StringName checkBoxList = "checkBoxList";

		public static readonly StringName hintDictionary = "hintDictionary";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private VBoxContainer _checkBoxContainer;

	public Array<CheckBox> checkBoxList = new Array<CheckBox>();

	public Dictionary hintDictionary = new Dictionary();

	public int value;

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_checkBoxContainer = GetNode<VBoxContainer>("%CheckBoxContainer");
	}

	public void SetHintDictionary(Dictionary newHintDictionary)
	{
		if (!IsNodeReady())
		{
			return;
		}
		hintDictionary = newHintDictionary;
		foreach (Node child in _checkBoxContainer.GetChildren())
		{
			child.QueueFree();
		}
		checkBoxList.Clear();
		foreach (Variant key in hintDictionary.Keys)
		{
			string text = key.AsString();
			CheckBox checkBox = new CheckBox();
			checkBox.Text = text;
			checkBox.Toggled += ValueChange;
			_checkBoxContainer.AddChild(checkBox, forceReadableName: false, InternalMode.Disabled);
			checkBoxList.Add(checkBox);
		}
	}

	public void SetValue(int newValue)
	{
		if (!IsNodeReady())
		{
			return;
		}
		value = newValue;
		if (CanSetValue())
		{
			for (int i = 0; i < checkBoxList.Count; i++)
			{
				CheckBox checkBox = checkBoxList[i];
				checkBox.Toggled -= ValueChange;
				checkBox.ButtonPressed = (value & (1 << i)) != 0;
				checkBox.Toggled += ValueChange;
			}
		}
	}

	private void ValueChange(bool toggledOn)
	{
		int num = 0;
		for (int i = 0; i < checkBoxList.Count; i++)
		{
			if (checkBoxList[i].ButtonPressed)
			{
				num += 1 << i;
			}
		}
		value = num;
		OnValueChanged?.Invoke(value);
	}

	public override bool CanSetValue()
	{
		return true;
	}

	public override string _GetType()
	{
		return "Flag";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetHintDictionary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "newHintDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSetValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetHintDictionary && args.Count == 1)
		{
			SetHintDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetValue && args.Count == 1)
		{
			SetValue(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ValueChange && args.Count == 1)
		{
			ValueChange(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanSetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSetValue());
			return true;
		}
		if (method == MethodName._GetType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetType());
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
		if (method == MethodName.SetHintDictionary)
		{
			return true;
		}
		if (method == MethodName.SetValue)
		{
			return true;
		}
		if (method == MethodName.ValueChange)
		{
			return true;
		}
		if (method == MethodName.CanSetValue)
		{
			return true;
		}
		if (method == MethodName._GetType)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checkBoxContainer)
		{
			_checkBoxContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.checkBoxList)
		{
			checkBoxList = VariantUtils.ConvertToArray<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.hintDictionary)
		{
			hintDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checkBoxContainer)
		{
			value = VariantUtils.CreateFrom(in _checkBoxContainer);
			return true;
		}
		if (name == PropertyName.checkBoxList)
		{
			value = VariantUtils.CreateFromArray(checkBoxList);
			return true;
		}
		if (name == PropertyName.hintDictionary)
		{
			value = VariantUtils.CreateFrom(in hintDictionary);
			return true;
		}
		if (name == PropertyName.value)
		{
			value = VariantUtils.CreateFrom(in this.value);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._checkBoxContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.checkBoxList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.hintDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checkBoxContainer, Variant.From(in _checkBoxContainer));
		info.AddProperty(PropertyName.checkBoxList, Variant.CreateFrom(checkBoxList));
		info.AddProperty(PropertyName.hintDictionary, Variant.From(in hintDictionary));
		info.AddProperty(PropertyName.value, Variant.From(in value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checkBoxContainer, out var variant))
		{
			_checkBoxContainer = variant.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.checkBoxList, out var variant2))
		{
			checkBoxList = variant2.AsGodotArray<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.hintDictionary, out var variant3))
		{
			hintDictionary = variant3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant4))
		{
			value = variant4.As<int>();
		}
	}
}
