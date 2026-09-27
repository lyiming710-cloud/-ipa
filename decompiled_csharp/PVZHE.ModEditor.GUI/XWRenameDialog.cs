using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWRenameDialog.cs")]
public class XWRenameDialog : ConfirmationDialog
{
	[Signal]
	public delegate void RenameConfirmedEventHandler(string[] oldNames, string[] newNames);

	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetTargets = "SetTargets";

		public static readonly StringName UpdatePreview = "UpdatePreview";

		public static readonly StringName OnConfirmed = "OnConfirmed";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _replaceEdit = "_replaceEdit";

		public static readonly StringName _regexCheck = "_regexCheck";

		public static readonly StringName _prefixEdit = "_prefixEdit";

		public static readonly StringName _suffixEdit = "_suffixEdit";

		public static readonly StringName _counterStart = "_counterStart";

		public static readonly StringName _counterStep = "_counterStep";

		public static readonly StringName _counterPadding = "_counterPadding";

		public static readonly StringName _previewList = "_previewList";

		public static readonly StringName _targets = "_targets";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
		public static readonly StringName RenameConfirmed = "RenameConfirmed";
	}

	private LineEdit _searchEdit;

	private LineEdit _replaceEdit;

	private CheckBox _regexCheck;

	private LineEdit _prefixEdit;

	private LineEdit _suffixEdit;

	private SpinBox _counterStart;

	private SpinBox _counterStep;

	private SpinBox _counterPadding;

	private ItemList _previewList;

	private string[] _targets = Array.Empty<string>();

	private RenameConfirmedEventHandler backing_RenameConfirmed;

	public event RenameConfirmedEventHandler RenameConfirmed
	{
		add
		{
			backing_RenameConfirmed = (RenameConfirmedEventHandler)Delegate.Combine(backing_RenameConfirmed, value);
		}
		remove
		{
			backing_RenameConfirmed = (RenameConfirmedEventHandler)Delegate.Remove(backing_RenameConfirmed, value);
		}
	}

	public override void _Ready()
	{
		_searchEdit = GetNode<LineEdit>("%SearchEdit");
		_replaceEdit = GetNode<LineEdit>("%ReplaceEdit");
		_regexCheck = GetNode<CheckBox>("%RegexCheck");
		_prefixEdit = GetNode<LineEdit>("%PrefixEdit");
		_suffixEdit = GetNode<LineEdit>("%SuffixEdit");
		_counterStart = GetNode<SpinBox>("%CounterStart");
		_counterStep = GetNode<SpinBox>("%CounterStep");
		_counterPadding = GetNode<SpinBox>("%CounterPadding");
		_previewList = GetNode<ItemList>("%PreviewList");
		_searchEdit.TextChanged += (string _) =>
		{
			UpdatePreview();
		};
		_replaceEdit.TextChanged += (string _) =>
		{
			UpdatePreview();
		};
		_regexCheck.Pressed += UpdatePreview;
		_prefixEdit.TextChanged += (string _) =>
		{
			UpdatePreview();
		};
		_suffixEdit.TextChanged += (string _) =>
		{
			UpdatePreview();
		};
		_counterStart.ValueChanged += (double _) =>
		{
			UpdatePreview();
		};
		_counterStep.ValueChanged += (double _) =>
		{
			UpdatePreview();
		};
		_counterPadding.ValueChanged += (double _) =>
		{
			UpdatePreview();
		};
		Confirmed += OnConfirmed;
	}

	public void SetTargets(string[] names)
	{
		_targets = names ?? Array.Empty<string>();
		UpdatePreview();
	}

	private void UpdatePreview()
	{
		_previewList.Clear();
		int num = (int)_counterStart.Value;
		int num2 = (int)_counterStep.Value;
		int num3 = (int)_counterPadding.Value;
		string text = _searchEdit.Text;
		string text2 = _replaceEdit.Text;
		string text3 = _prefixEdit.Text;
		string text4 = _suffixEdit.Text;
		string[] targets = _targets;
		foreach (string text5 in targets)
		{
			string text6 = text5;
			if (!string.IsNullOrEmpty(text))
			{
				if (_regexCheck.ButtonPressed)
				{
					try
					{
						text6 = Regex.Replace(text6, text, text2);
					}
					catch
					{
					}
				}
				else
				{
					text6 = text6.Replace(text, text2);
				}
			}
			text6 = text3 + text6 + text4;
			if (num3 > 0)
			{
				text6 += num.ToString("D" + num3);
			}
			_previewList.AddItem(text5 + "  →  " + text6);
			num += num2;
		}
	}

	private void OnConfirmed()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		int num = (int)_counterStart.Value;
		int num2 = (int)_counterStep.Value;
		int num3 = (int)_counterPadding.Value;
		string text = _searchEdit.Text;
		string text2 = _replaceEdit.Text;
		string text3 = _prefixEdit.Text;
		string text4 = _suffixEdit.Text;
		string[] targets = _targets;
		foreach (string text5 in targets)
		{
			string text6 = text5;
			if (!string.IsNullOrEmpty(text))
			{
				if (_regexCheck.ButtonPressed)
				{
					try
					{
						text6 = Regex.Replace(text6, text, text2);
					}
					catch
					{
					}
				}
				else
				{
					text6 = text6.Replace(text, text2);
				}
			}
			text6 = text3 + text6 + text4;
			if (num3 > 0)
			{
				text6 += num.ToString("D" + num3);
			}
			list.Add(text5);
			list2.Add(text6);
			num += num2;
		}
		EmitSignal(SignalName.RenameConfirmed, list.ToArray(), list2.ToArray());
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetTargets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetTargets && args.Count == 1)
		{
			SetTargets(VariantUtils.ConvertTo<string[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreview && args.Count == 0)
		{
			UpdatePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
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
		if (method == MethodName.SetTargets)
		{
			return true;
		}
		if (method == MethodName.UpdatePreview)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._replaceEdit)
		{
			_replaceEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._regexCheck)
		{
			_regexCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._prefixEdit)
		{
			_prefixEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._suffixEdit)
		{
			_suffixEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._counterStart)
		{
			_counterStart = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._counterStep)
		{
			_counterStep = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._counterPadding)
		{
			_counterPadding = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._previewList)
		{
			_previewList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._targets)
		{
			_targets = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._replaceEdit)
		{
			value = VariantUtils.CreateFrom(in _replaceEdit);
			return true;
		}
		if (name == PropertyName._regexCheck)
		{
			value = VariantUtils.CreateFrom(in _regexCheck);
			return true;
		}
		if (name == PropertyName._prefixEdit)
		{
			value = VariantUtils.CreateFrom(in _prefixEdit);
			return true;
		}
		if (name == PropertyName._suffixEdit)
		{
			value = VariantUtils.CreateFrom(in _suffixEdit);
			return true;
		}
		if (name == PropertyName._counterStart)
		{
			value = VariantUtils.CreateFrom(in _counterStart);
			return true;
		}
		if (name == PropertyName._counterStep)
		{
			value = VariantUtils.CreateFrom(in _counterStep);
			return true;
		}
		if (name == PropertyName._counterPadding)
		{
			value = VariantUtils.CreateFrom(in _counterPadding);
			return true;
		}
		if (name == PropertyName._previewList)
		{
			value = VariantUtils.CreateFrom(in _previewList);
			return true;
		}
		if (name == PropertyName._targets)
		{
			value = VariantUtils.CreateFrom(in _targets);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._replaceEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._regexCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._prefixEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._suffixEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._counterStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._counterStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._counterPadding, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._targets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._replaceEdit, Variant.From(in _replaceEdit));
		info.AddProperty(PropertyName._regexCheck, Variant.From(in _regexCheck));
		info.AddProperty(PropertyName._prefixEdit, Variant.From(in _prefixEdit));
		info.AddProperty(PropertyName._suffixEdit, Variant.From(in _suffixEdit));
		info.AddProperty(PropertyName._counterStart, Variant.From(in _counterStart));
		info.AddProperty(PropertyName._counterStep, Variant.From(in _counterStep));
		info.AddProperty(PropertyName._counterPadding, Variant.From(in _counterPadding));
		info.AddProperty(PropertyName._previewList, Variant.From(in _previewList));
		info.AddProperty(PropertyName._targets, Variant.From(in _targets));
		info.AddSignalEventDelegate(SignalName.RenameConfirmed, backing_RenameConfirmed);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._searchEdit, out var value))
		{
			_searchEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._replaceEdit, out var value2))
		{
			_replaceEdit = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._regexCheck, out var value3))
		{
			_regexCheck = value3.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._prefixEdit, out var value4))
		{
			_prefixEdit = value4.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._suffixEdit, out var value5))
		{
			_suffixEdit = value5.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._counterStart, out var value6))
		{
			_counterStart = value6.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._counterStep, out var value7))
		{
			_counterStep = value7.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._counterPadding, out var value8))
		{
			_counterPadding = value8.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._previewList, out var value9))
		{
			_previewList = value9.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._targets, out var value10))
		{
			_targets = value10.As<string[]>();
		}
		if (info.TryGetSignalEventDelegate<RenameConfirmedEventHandler>(SignalName.RenameConfirmed, out var value11))
		{
			backing_RenameConfirmed = value11;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.RenameConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "oldNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "newNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalRenameConfirmed(string[] oldNames, string[] newNames)
	{
		StringName renameConfirmed = SignalName.RenameConfirmed;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = oldNames;
		buffer[1] = newNames;
		EmitSignal(renameConfirmed, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.RenameConfirmed && args.Count == 2)
		{
			backing_RenameConfirmed?.Invoke(VariantUtils.ConvertTo<string[]>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.RenameConfirmed)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
