using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://addons/godot_state_charts/VisualEditor/StateMachineDiagnosticsPanel.cs")]
public class StateMachineDiagnosticsPanel : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ValidateDefinition = "ValidateDefinition";

		public static readonly StringName NavigateToStableId = "NavigateToStableId";

		public static readonly StringName Clear = "Clear";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _summary = "_summary";

		public static readonly StringName _items = "_items";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Label _summary;

	private VBoxContainer _items;

	public event Action<string> NavigateRequested;

	public override void _Ready()
	{
		CustomMinimumSize = new Vector2(0f, 128f);
		VBoxContainer vBoxContainer = new VBoxContainer();
		AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		_summary = new Label
		{
			Text = "诊断：尚未校验"
		};
		vBoxContainer.AddChild(_summary, forceReadableName: false, InternalMode.Disabled);
		ScrollContainer scrollContainer = new ScrollContainer
		{
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddChild(scrollContainer, forceReadableName: false, InternalMode.Disabled);
		_items = new VBoxContainer
		{
			Name = "DiagnosticItems",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		scrollContainer.AddChild(_items, forceReadableName: false, InternalMode.Disabled);
	}

	public void ValidateDefinition(StateMachineDefinition definition)
	{
		StateMachineValidationResult result = StateMachineValidator.Validate(definition);
		ShowDiagnostics(result);
	}

	public void ShowDiagnostics(StateMachineValidationResult result)
	{
		Clear();
		if (result == null)
		{
			_summary.Text = "诊断：无法取得校验结果";
			return;
		}
		int num = 0;
		int num2 = 0;
		foreach (StateMachineDiagnostic diagnostic in result.Diagnostics)
		{
			if (diagnostic.Severity == StateMachineDiagnosticSeverity.Error)
			{
				num++;
			}
			else
			{
				num2++;
			}
			AddDiagnostic(diagnostic);
		}
		_summary.Text = ((result.Diagnostics.Count == 0) ? "✓ 诊断：可以运行" : $"诊断：{num} 个错误，{num2} 个警告");
	}

	public void NavigateToStableId(string stableId)
	{
		if (!string.IsNullOrWhiteSpace(stableId))
		{
			NavigateRequested?.Invoke(stableId);
		}
	}

	private void AddDiagnostic(StateMachineDiagnostic diagnostic)
	{
		string value = ((diagnostic.Severity == StateMachineDiagnosticSeverity.Error) ? "⛔" : "⚠");
		Button button = new Button
		{
			Text = $"{value} [{diagnostic.Code}] {diagnostic.Message}",
			Alignment = HorizontalAlignment.Left,
			TooltipText = diagnostic.Name + "\nStableId: " + diagnostic.StableId,
			Disabled = string.IsNullOrWhiteSpace(diagnostic.StableId)
		};
		button.Pressed += () =>
		{
			NavigateToStableId(diagnostic.StableId);
		};
		_items.AddChild(button, forceReadableName: false, InternalMode.Disabled);
	}

	private void Clear()
	{
		if (_items == null)
		{
			return;
		}
		foreach (Node child in _items.GetChildren())
		{
			_items.RemoveChild(child);
			child.QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ValidateDefinition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NavigateToStableId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ValidateDefinition && args.Count == 1)
		{
			ValidateDefinition(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToStableId && args.Count == 1)
		{
			NavigateToStableId(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
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
		if (method == MethodName.ValidateDefinition)
		{
			return true;
		}
		if (method == MethodName.NavigateToStableId)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._summary)
		{
			_summary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._items)
		{
			_items = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._summary)
		{
			value = VariantUtils.CreateFrom(in _summary);
			return true;
		}
		if (name == PropertyName._items)
		{
			value = VariantUtils.CreateFrom(in _items);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._summary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._items, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._summary, Variant.From(in _summary));
		info.AddProperty(PropertyName._items, Variant.From(in _items));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._summary, out var value))
		{
			_summary = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._items, out var value2))
		{
			_items = value2.As<VBoxContainer>();
		}
	}
}
