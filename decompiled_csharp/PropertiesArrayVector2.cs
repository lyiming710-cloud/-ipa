using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/ArrayVector2/PropertiesArrayVector2.cs")]
public class PropertiesArrayVector2 : PropertiesBase
{
	public delegate void ValueChangedEventHandler(Array<Vector2> value);

	public new class MethodName : PropertiesBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName UpdateUI = "UpdateUI";

		public static readonly StringName OnAddButtonPressed = "OnAddButtonPressed";

		public static readonly StringName OnRemoveButtonPressed = "OnRemoveButtonPressed";

		public static readonly StringName OnValueChangeX = "OnValueChangeX";

		public static readonly StringName OnValueChangeY = "OnValueChangeY";

		public static readonly StringName EmitValueChanged = "EmitValueChanged";

		public new static readonly StringName CanSetValue = "CanSetValue";

		public new static readonly StringName _GetType = "_GetType";
	}

	public new class PropertyName : PropertiesBase.PropertyName
	{
		public static readonly StringName _prefab = "_prefab";

		public static readonly StringName _root = "_root";

		public static readonly StringName _addButton = "_addButton";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private HBoxContainer _prefab;

	private VBoxContainer _root;

	private Button _addButton;

	public Array<Vector2> value = new Array<Vector2>();

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_prefab = GetNode<HBoxContainer>("%Prefab");
		_root = GetNode<VBoxContainer>("%Root");
		_addButton = GetNode<Button>("%AddButton");
		UpdateUI();
		_addButton.Pressed += OnAddButtonPressed;
	}

	public void SetValue(Array<Vector2> newValue)
	{
		value = newValue;
		if (IsNodeReady())
		{
			UpdateUI();
		}
	}

	private void UpdateUI()
	{
		foreach (Node child2 in _root.GetChildren())
		{
			child2.QueueFree();
		}
		for (int i = 0; i < value.Count; i++)
		{
			HBoxContainer hBoxContainer = _prefab.Duplicate() as HBoxContainer;
			hBoxContainer.GetNode<Label>("IndexLabel").Text = i.ToString();
			Node child = hBoxContainer.GetChild(1);
			SpinBox node = child.GetNode<SpinBox>("SpinBoxX");
			SpinBox node2 = child.GetNode<SpinBox>("SpinBoxY");
			node.Value = value[i].X;
			node2.Value = value[i].Y;
			int idx = i;
			node.ValueChanged += (double v) =>
			{
				OnValueChangeX((float)v, idx);
			};
			node2.ValueChanged += (double v) =>
			{
				OnValueChangeY((float)v, idx);
			};
			hBoxContainer.GetNode<Button>("RemoveButton").Pressed += () =>
			{
				OnRemoveButtonPressed(idx);
			};
			hBoxContainer.Visible = true;
			_root.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void OnAddButtonPressed()
	{
		value.Add(Vector2.Zero);
		UpdateUI();
		EmitValueChanged();
	}

	private void OnRemoveButtonPressed(int index)
	{
		GD.Print("remove_button_pressed", index);
		if (index < value.Count)
		{
			value.RemoveAt(index);
			UpdateUI();
			EmitValueChanged();
		}
	}

	private void OnValueChangeX(float v, int index)
	{
		if (index < value.Count)
		{
			value[index] = new Vector2(v, value[index].Y);
			EmitValueChanged();
		}
	}

	private void OnValueChangeY(float v, int index)
	{
		if (index < value.Count)
		{
			value[index] = new Vector2(value[index].X, v);
			EmitValueChanged();
		}
	}

	private void EmitValueChanged()
	{
		OnValueChanged?.Invoke(value);
	}

	public override bool CanSetValue()
	{
		foreach (Node child2 in _root.GetChildren())
		{
			Node child = child2.GetChild(1);
			SpinBox node = child.GetNode<SpinBox>("SpinBoxX");
			SpinBox node2 = child.GetNode<SpinBox>("SpinBoxY");
			Button node3 = child2.GetNode<Button>("RemoveButton");
			if (node.GetLineEdit().HasFocus() || node2.GetLineEdit().HasFocus() || node3.HasFocus())
			{
				return false;
			}
		}
		return true;
	}

	public override string _GetType()
	{
		return "ArrayVector2";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAddButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRemoveButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnValueChangeX, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "v", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnValueChangeY, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "v", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.SetValue && args.Count == 1)
		{
			SetValue(VariantUtils.ConvertToArray<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateUI && args.Count == 0)
		{
			UpdateUI();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAddButtonPressed && args.Count == 0)
		{
			OnAddButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRemoveButtonPressed && args.Count == 1)
		{
			OnRemoveButtonPressed(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnValueChangeX && args.Count == 2)
		{
			OnValueChangeX(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnValueChangeY && args.Count == 2)
		{
			OnValueChangeY(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitValueChanged && args.Count == 0)
		{
			EmitValueChanged();
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
		if (method == MethodName.SetValue)
		{
			return true;
		}
		if (method == MethodName.UpdateUI)
		{
			return true;
		}
		if (method == MethodName.OnAddButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnRemoveButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnValueChangeX)
		{
			return true;
		}
		if (method == MethodName.OnValueChangeY)
		{
			return true;
		}
		if (method == MethodName.EmitValueChanged)
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
		if (name == PropertyName._prefab)
		{
			_prefab = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._addButton)
		{
			_addButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertToArray<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._prefab)
		{
			value = VariantUtils.CreateFrom(in _prefab);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._addButton)
		{
			value = VariantUtils.CreateFrom(in _addButton);
			return true;
		}
		if (name == PropertyName.value)
		{
			value = VariantUtils.CreateFromArray(this.value);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._prefab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._prefab, Variant.From(in _prefab));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._addButton, Variant.From(in _addButton));
		info.AddProperty(PropertyName.value, Variant.CreateFrom(value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._prefab, out var variant))
		{
			_prefab = variant.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._root, out var variant2))
		{
			_root = variant2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._addButton, out var variant3))
		{
			_addButton = variant3.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant4))
		{
			value = variant4.AsGodotArray<Vector2>();
		}
	}
}
