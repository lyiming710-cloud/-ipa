using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Enum/PropertiesEnum.cs")]
public class PropertiesEnum : PropertiesBase
{
	public delegate void ValueChangedEventHandler(Variant value);

	public new class MethodName : PropertiesBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetHintDictionary = "SetHintDictionary";

		public static readonly StringName RebuildItems = "RebuildItems";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName ValueChange = "ValueChange";

		public new static readonly StringName CanSetValue = "CanSetValue";

		public new static readonly StringName _GetType = "_GetType";

		public static readonly StringName FindOptionButtonId = "FindOptionButtonId";
	}

	public new class PropertyName : PropertiesBase.PropertyName
	{
		public static readonly StringName _optionButton = "_optionButton";

		public static readonly StringName itemDictionary = "itemDictionary";

		public static readonly StringName hintDictionary = "hintDictionary";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private OptionButton _optionButton;

	public Dictionary itemDictionary = new Dictionary();

	public Dictionary hintDictionary = new Dictionary();

	public Variant value = false;

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_optionButton = GetNode<OptionButton>("%OptionButton");
		LevelEditorDropdown.Configure(_optionButton);
		_optionButton.ItemSelected += ValueChange;
		if (hintDictionary.Count > 0)
		{
			RebuildItems();
		}
	}

	public void SetHintDictionary(Dictionary newHintDictionary)
	{
		hintDictionary = newHintDictionary;
		if (IsNodeReady())
		{
			RebuildItems();
		}
	}

	private void RebuildItems()
	{
		itemDictionary.Clear();
		_optionButton.Clear();
		foreach (Variant key in hintDictionary.Keys)
		{
			string text = key.AsString();
			itemDictionary[hintDictionary[key]] = text;
			_optionButton.AddItem(text);
		}
	}

	public void SetValue(Variant newValue)
	{
		if (IsNodeReady())
		{
			value = newValue;
			if (itemDictionary.ContainsKey(newValue))
			{
				_optionButton.ItemSelected -= ValueChange;
				_optionButton.Selected = FindOptionButtonId(_optionButton, itemDictionary[newValue].AsString());
				_optionButton.ItemSelected += ValueChange;
			}
		}
	}

	private void ValueChange(long index)
	{
		int idx = (int)index;
		value = hintDictionary[_optionButton.GetItemText(idx)];
		OnValueChanged?.Invoke(value);
	}

	public override bool CanSetValue()
	{
		return !_optionButton.HasFocus();
	}

	public override string _GetType()
	{
		return "Enum";
	}

	public int FindOptionButtonId(OptionButton optionButton, string key)
	{
		for (int i = 0; i < optionButton.ItemCount; i++)
		{
			if (optionButton.GetItemText(i) == key)
			{
				return optionButton.GetItemId(i);
			}
		}
		return -1;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetHintDictionary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "newHintDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSetValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindOptionButtonId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "optionButton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetHintDictionary && args.Count == 1)
		{
			SetHintDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildItems && args.Count == 0)
		{
			RebuildItems();
			ret = default;
			return true;
		}
		if (method == MethodName.SetValue && args.Count == 1)
		{
			SetValue(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ValueChange && args.Count == 1)
		{
			ValueChange(VariantUtils.ConvertTo<long>(in args[0]));
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
		if (method == MethodName.FindOptionButtonId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindOptionButtonId(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.RebuildItems)
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
		if (method == MethodName.FindOptionButtonId)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._optionButton)
		{
			_optionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.itemDictionary)
		{
			itemDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.hintDictionary)
		{
			hintDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._optionButton)
		{
			value = VariantUtils.CreateFrom(in _optionButton);
			return true;
		}
		if (name == PropertyName.itemDictionary)
		{
			value = VariantUtils.CreateFrom(in itemDictionary);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._optionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.itemDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.hintDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
		info.AddProperty(PropertyName.itemDictionary, Variant.From(in itemDictionary));
		info.AddProperty(PropertyName.hintDictionary, Variant.From(in hintDictionary));
		info.AddProperty(PropertyName.value, Variant.From(in value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._optionButton, out var variant))
		{
			_optionButton = variant.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.itemDictionary, out var variant2))
		{
			itemDictionary = variant2.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.hintDictionary, out var variant3))
		{
			hintDictionary = variant3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant4))
		{
			value = variant4.As<Variant>();
		}
	}
}
