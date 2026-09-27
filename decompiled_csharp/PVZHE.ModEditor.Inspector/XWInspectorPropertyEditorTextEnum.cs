using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/TextEnum/XWInspectorPropertyEditorTextEnum.cs")]
public class XWInspectorPropertyEditorTextEnum : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName ParseHintString = "ParseHintString";

		public static readonly StringName ShowCustomEdit = "ShowCustomEdit";

		public static readonly StringName OnOptionSelected = "OnOptionSelected";

		public static readonly StringName OnCustomTextChanged = "OnCustomTextChanged";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _optionButton = "_optionButton";

		public static readonly StringName _lineEdit = "_lineEdit";

		public static readonly StringName _options = "_options";

		public static readonly StringName _isCustom = "_isCustom";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/TextEnum/XWInspectorPropertyEditorTextEnum.tscn";

	private OptionButton _optionButton;

	private LineEdit _lineEdit;

	private string[] _options = Array.Empty<string>();

	private bool _isCustom;

	public static XWInspectorPropertyEditorTextEnum Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/TextEnum/XWInspectorPropertyEditorTextEnum.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorTextEnum>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_optionButton = GetNode<OptionButton>("%OptionButton");
		_lineEdit = GetNode<LineEdit>("%LineEdit");
		_optionButton.ItemSelected += OnOptionSelected;
		_lineEdit.TextChanged += OnCustomTextChanged;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		ParseHintString(property.HintString);
		base.SetEditProperty(property, field);
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			string text = propertyValue.ToString();
			int num = Array.IndexOf(_options, text);
			if (num >= 0)
			{
				_optionButton.Select(num);
				ShowCustomEdit(show: false);
			}
			else
			{
				_optionButton.Select(_options.Length);
				XWInspectorPropertyEditorBase.SetLineEditTextPreservingCaret(_lineEdit, text);
				ShowCustomEdit(show: true);
			}
		}
	}

	public override Variant GetValue()
	{
		if (_isCustom)
		{
			return _lineEdit.Text;
		}
		int selected = _optionButton.Selected;
		if (selected >= 0 && selected < _options.Length)
		{
			return _options[selected];
		}
		return "";
	}

	private void ParseHintString(string hintString)
	{
		_optionButton.Clear();
		_options = hintString.Split(",");
		string[] options = _options;
		foreach (string label in options)
		{
			_optionButton.AddItem(label);
		}
		_optionButton.AddItem("自定义...");
		ShowCustomEdit(show: false);
	}

	private void ShowCustomEdit(bool show)
	{
		_isCustom = show;
		_lineEdit.Visible = show;
		if (show)
		{
			_optionButton.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
			_lineEdit.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		}
		else
		{
			_optionButton.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			_lineEdit.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
		}
	}

	private void OnOptionSelected(long index)
	{
		if (index == _options.Length)
		{
			ShowCustomEdit(show: true);
			_lineEdit.GrabFocus();
		}
		else
		{
			ShowCustomEdit(show: false);
			ValueChange(_options[index]);
		}
	}

	private void OnCustomTextChanged(string newText)
	{
		ValueChange(newText);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ParseHintString, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowCustomEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "show", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnOptionSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCustomTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorTextEnum>(Create());
			return true;
		}
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
		if (method == MethodName.ParseHintString && args.Count == 1)
		{
			ParseHintString(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCustomEdit && args.Count == 1)
		{
			ShowCustomEdit(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnOptionSelected && args.Count == 1)
		{
			OnOptionSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCustomTextChanged && args.Count == 1)
		{
			OnCustomTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorTextEnum>(Create());
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
		if (method == MethodName.ParseHintString)
		{
			return true;
		}
		if (method == MethodName.ShowCustomEdit)
		{
			return true;
		}
		if (method == MethodName.OnOptionSelected)
		{
			return true;
		}
		if (method == MethodName.OnCustomTextChanged)
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
		if (name == PropertyName._lineEdit)
		{
			_lineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._options)
		{
			_options = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._isCustom)
		{
			_isCustom = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._lineEdit)
		{
			value = VariantUtils.CreateFrom(in _lineEdit);
			return true;
		}
		if (name == PropertyName._options)
		{
			value = VariantUtils.CreateFrom(in _options);
			return true;
		}
		if (name == PropertyName._isCustom)
		{
			value = VariantUtils.CreateFrom(in _isCustom);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._lineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._options, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isCustom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
		info.AddProperty(PropertyName._lineEdit, Variant.From(in _lineEdit));
		info.AddProperty(PropertyName._options, Variant.From(in _options));
		info.AddProperty(PropertyName._isCustom, Variant.From(in _isCustom));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._optionButton, out var value))
		{
			_optionButton = value.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._lineEdit, out var value2))
		{
			_lineEdit = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._options, out var value3))
		{
			_options = value3.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._isCustom, out var value4))
		{
			_isCustom = value4.As<bool>();
		}
	}
}
