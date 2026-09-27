using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Layers/XWInspectorPropertyEditorLayers.cs")]
public class XWInspectorPropertyEditorLayers : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnButtonPressed = "OnButtonPressed";

		public static readonly StringName OnLayerToggled = "OnLayerToggled";

		public static readonly StringName UpdateButtonsFromMask = "UpdateButtonsFromMask";

		public static readonly StringName UpdateButtonText = "UpdateButtonText";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _button = "_button";

		public static readonly StringName _popup = "_popup";

		public static readonly StringName _grid = "_grid";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Layers/XWInspectorPropertyEditorLayers.tscn";

	private Button _button;

	private PopupPanel _popup;

	private GridContainer _grid;

	private readonly List<Button> _layerButtons = new List<Button>();

	public static XWInspectorPropertyEditorLayers Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Layers/XWInspectorPropertyEditorLayers.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorLayers>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_button = GetNode<Button>("%LayersButton");
		_popup = GetNode<PopupPanel>("%LayersPopup");
		_grid = GetNode<GridContainer>("%GridContainer");
		_button.Pressed += OnButtonPressed;
		for (int i = 0; i < 32; i++)
		{
			Button button = new Button
			{
				Text = (i + 1).ToString(),
				CustomMinimumSize = new Vector2(18f, 18f),
				ToggleMode = true,
				TooltipText = $"层 {i + 1}"
			};
			int layerIndex = i;
			button.Toggled += (bool _) =>
			{
				OnLayerToggled(layerIndex);
			};
			_layerButtons.Add(button);
			_grid.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			long mask = propertyValue.AsInt64();
			UpdateButtonsFromMask(mask);
			UpdateButtonText(mask);
		}
	}

	public override Variant GetValue()
	{
		long num = 0L;
		for (int i = 0; i < 32; i++)
		{
			if (GodotObject.IsInstanceValid(_layerButtons[i]) && _layerButtons[i].ButtonPressed)
			{
				num |= 1L << i;
			}
		}
		return num;
	}

	private void OnButtonPressed()
	{
		_popup.Position = new Vector2I((int)GetGlobalMousePosition().X, (int)GetGlobalMousePosition().Y);
		_popup.Popup();
	}

	private void OnLayerToggled(int layerIndex)
	{
		long num = 0L;
		for (int i = 0; i < 32; i++)
		{
			if (GodotObject.IsInstanceValid(_layerButtons[i]) && _layerButtons[i].ButtonPressed)
			{
				num |= 1L << i;
			}
		}
		ValueChange(num);
		UpdateButtonText(num);
	}

	private void UpdateButtonsFromMask(long mask)
	{
		for (int i = 0; i < 32; i++)
		{
			if (GodotObject.IsInstanceValid(_layerButtons[i]))
			{
				_layerButtons[i].SetPressedNoSignal((mask & (1L << i)) != 0);
			}
		}
	}

	private void UpdateButtonText(long mask)
	{
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			if ((mask & (1L << i)) != 0L)
			{
				num++;
			}
		}
		_button.Text = $"{num} 层";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnLayerToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateButtonsFromMask, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateButtonText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorLayers>(Create());
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
		if (method == MethodName.OnButtonPressed && args.Count == 0)
		{
			OnButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnLayerToggled && args.Count == 1)
		{
			OnLayerToggled(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateButtonsFromMask && args.Count == 1)
		{
			UpdateButtonsFromMask(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateButtonText && args.Count == 1)
		{
			UpdateButtonText(VariantUtils.ConvertTo<long>(in args[0]));
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorLayers>(Create());
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
		if (method == MethodName.OnButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnLayerToggled)
		{
			return true;
		}
		if (method == MethodName.UpdateButtonsFromMask)
		{
			return true;
		}
		if (method == MethodName.UpdateButtonText)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._button)
		{
			_button = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._popup)
		{
			_popup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._grid)
		{
			_grid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._button)
		{
			value = VariantUtils.CreateFrom(in _button);
			return true;
		}
		if (name == PropertyName._popup)
		{
			value = VariantUtils.CreateFrom(in _popup);
			return true;
		}
		if (name == PropertyName._grid)
		{
			value = VariantUtils.CreateFrom(in _grid);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._button, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._popup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._grid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._button, Variant.From(in _button));
		info.AddProperty(PropertyName._popup, Variant.From(in _popup));
		info.AddProperty(PropertyName._grid, Variant.From(in _grid));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._button, out var value))
		{
			_button = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._popup, out var value2))
		{
			_popup = value2.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._grid, out var value3))
		{
			_grid = value3.As<GridContainer>();
		}
	}
}
