using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Locale/XWInspectorPropertyEditorLocale.cs")]
public class XWInspectorPropertyEditorLocale : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnLocaleSelected = "OnLocaleSelected";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _optionButton = "_optionButton";

		public static readonly StringName _locales = "_locales";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Locale/XWInspectorPropertyEditorLocale.tscn";

	private OptionButton _optionButton;

	private string[] _locales = new string[34]
	{
		"zh", "zh_CN", "zh_TW", "zh_HK", "en", "en_US", "en_GB", "ja", "ko", "de",
		"fr", "es", "pt", "ru", "it", "ar", "hi", "th", "vi", "id",
		"nl", "pl", "sv", "da", "no", "fi", "cs", "hu", "ro", "bg",
		"uk", "tr", "el", "he"
	};

	public static XWInspectorPropertyEditorLocale Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Locale/XWInspectorPropertyEditorLocale.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorLocale>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_optionButton = GetNode<OptionButton>("%OptionButton");
		string[] locales = _locales;
		foreach (string label in locales)
		{
			_optionButton.AddItem(label);
		}
		_optionButton.ItemSelected += OnLocaleSelected;
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			string text = propertyValue.AsString();
			int num = Array.IndexOf(_locales, text);
			if (num >= 0)
			{
				_optionButton.Select(num);
				return;
			}
			_optionButton.AddItem(text);
			string[] array = new string[_locales.Length + 1];
			_locales.CopyTo(array, 0);
			array[_locales.Length] = text;
			_locales = array;
			_optionButton.Select(_optionButton.ItemCount - 1);
		}
	}

	public override Variant GetValue()
	{
		int selected = _optionButton.Selected;
		if (selected >= 0 && selected < _locales.Length)
		{
			return _locales[selected];
		}
		return "";
	}

	private void OnLocaleSelected(long index)
	{
		int num = (int)index;
		if (num >= 0 && num < _locales.Length)
		{
			ValueChange(_locales[num]);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnLocaleSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorLocale>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
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
		if (method == MethodName.OnLocaleSelected && args.Count == 1)
		{
			OnLocaleSelected(VariantUtils.ConvertTo<long>(in args[0]));
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorLocale>(Create());
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
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.OnLocaleSelected)
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
		if (name == PropertyName._locales)
		{
			_locales = VariantUtils.ConvertTo<string[]>(in value);
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
		if (name == PropertyName._locales)
		{
			value = VariantUtils.CreateFrom(in _locales);
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
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._locales, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
		info.AddProperty(PropertyName._locales, Variant.From(in _locales));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._optionButton, out var value))
		{
			_optionButton = value.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._locales, out var value2))
		{
			_locales = value2.As<string[]>();
		}
	}
}
