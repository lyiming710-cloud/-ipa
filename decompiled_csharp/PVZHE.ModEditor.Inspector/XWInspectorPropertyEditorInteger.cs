using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Integer/XWInspectorPropertyEditorInteger.cs")]
public class XWInspectorPropertyEditorInteger : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public new static readonly StringName SetupRangeHint = "SetupRangeHint";

		public static readonly StringName ApplyRangeHint = "ApplyRangeHint";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _spinBox = "_spinBox";

		public static readonly StringName _pendingRangeHint = "_pendingRangeHint";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private SpinBox _spinBox;

	private string _pendingRangeHint = "";

	public override void _Ready()
	{
		base._Ready();
		_spinBox = GetNode<SpinBox>("%SpinBox");
		_spinBox.ValueChanged += (double v) =>
		{
			ValueChange((int)v);
		};
		_spinBox.GetLineEdit().CaretBlink = true;
		if (_pendingRangeHint != "")
		{
			ApplyRangeHint(_pendingRangeHint);
			_pendingRangeHint = "";
		}
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			_spinBox.Value = propertyValue.As<int>();
		}
	}

	public override Variant GetValue()
	{
		return (int)_spinBox.Value;
	}

	public override void SetupRangeHint(string hintString)
	{
		if (IsNodeReady())
		{
			ApplyRangeHint(hintString);
		}
		else
		{
			_pendingRangeHint = hintString;
		}
	}

	private void ApplyRangeHint(string hintString)
	{
		string[] array = hintString.Split(",");
		int num = 0;
		if (array.Length >= 2 && TryParseNumber(array[0], out var result) && TryParseNumber(array[1], out var result2))
		{
			_spinBox.MinValue = result;
			_spinBox.MaxValue = result2;
			_spinBox.AllowGreater = false;
			_spinBox.AllowLesser = false;
			num = 2;
			if (array.Length >= 3 && TryParseNumber(array[2], out var result3))
			{
				_spinBox.Step = result3;
				num = 3;
			}
		}
		for (int i = num; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text == "or_greater")
			{
				_spinBox.AllowGreater = true;
			}
			else if (text == "or_less")
			{
				_spinBox.AllowLesser = true;
			}
			else if (text.StartsWith("suffix:", StringComparison.Ordinal))
			{
				SpinBox spinBox = _spinBox;
				string text2 = text;
				int length = "suffix:".Length;
				spinBox.Suffix = text2.Substring(length, text2.Length - length);
			}
		}
	}

	private static bool TryParseNumber(string value, out double result)
	{
		return double.TryParse(value.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupRangeHint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hintString", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRangeHint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.SetupRangeHint && args.Count == 1)
		{
			SetupRangeHint(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRangeHint && args.Count == 1)
		{
			ApplyRangeHint(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.SetupRangeHint)
		{
			return true;
		}
		if (method == MethodName.ApplyRangeHint)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._spinBox)
		{
			_spinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._pendingRangeHint)
		{
			_pendingRangeHint = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spinBox)
		{
			value = VariantUtils.CreateFrom(in _spinBox);
			return true;
		}
		if (name == PropertyName._pendingRangeHint)
		{
			value = VariantUtils.CreateFrom(in _pendingRangeHint);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingRangeHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._spinBox, Variant.From(in _spinBox));
		info.AddProperty(PropertyName._pendingRangeHint, Variant.From(in _pendingRangeHint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._spinBox, out var value))
		{
			_spinBox = value.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._pendingRangeHint, out var value2))
		{
			_pendingRangeHint = value2.As<string>();
		}
	}
}
