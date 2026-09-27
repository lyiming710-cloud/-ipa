using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Enum/XWInspectorPropertyEditorEnum.cs")]
public class XWInspectorPropertyEditorEnum : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName ParseEnumHint = "ParseEnumHint";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _optionButton = "_optionButton";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private OptionButton _optionButton;

	private readonly List<int> _enumValues = new List<int>();

	public override void _Ready()
	{
		base._Ready();
		_optionButton = GetNode<OptionButton>("%OptionButton");
		_optionButton.ItemSelected += (long index) =>
		{
			if (index >= 0 && index < _enumValues.Count)
			{
				ValueChange(_enumValues[(int)index]);
			}
		};
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		ParseEnumHint(property.HintString);
		base.SetEditProperty(property, field);
	}

	public override void UpdateValue()
	{
		if (_optionButton.ItemCount <= 0)
		{
			return;
		}
		int num = GetPropertyValue().As<int>();
		for (int i = 0; i < _optionButton.ItemCount && i < _enumValues.Count; i++)
		{
			if (_enumValues[i] == num)
			{
				_optionButton.Select(i);
				break;
			}
		}
	}

	public override Variant GetValue()
	{
		int selected = _optionButton.Selected;
		return (selected >= 0 && selected < _enumValues.Count) ? _enumValues[selected] : 0;
	}

	private void ParseEnumHint(string hintString)
	{
		_optionButton.Clear();
		_enumValues.Clear();
		string[] array = hintString.Split(",");
		int num = 0;
		string[] array2 = array;
		foreach (string text in array2)
		{
			string label = text.Trim();
			int num2 = num;
			int num3 = text.LastIndexOf(':');
			if (num3 > 0)
			{
				string text2 = text;
				int num4 = num3 + 1;
				if (int.TryParse(text2.Substring(num4, text2.Length - num4).Trim(), out var result))
				{
					label = text.Substring(0, num3).Trim();
					num2 = result;
				}
			}
			_optionButton.AddItem(label);
			_enumValues.Add(num2);
			num = num2 + 1;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ParseEnumHint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetEditProperty && args.Count == 2)
		{
			SetEditProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.ParseEnumHint && args.Count == 1)
		{
			ParseEnumHint(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.SetEditProperty)
		{
			return true;
		}
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.ParseEnumHint)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._optionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._optionButton, out var value))
		{
			_optionButton = value.As<OptionButton>();
		}
	}
}
