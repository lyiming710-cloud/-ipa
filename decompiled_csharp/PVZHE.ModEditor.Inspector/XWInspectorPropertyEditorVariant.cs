using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Variant/XWInspectorPropertyEditorVariant.cs")]
public class XWInspectorPropertyEditorVariant : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateSpinBoxRow = "CreateSpinBoxRow";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName ShowEditorForType = "ShowEditorForType";

		public static readonly StringName SetEditorValue = "SetEditorValue";

		public static readonly StringName OnTypeSelected = "OnTypeSelected";

		public static readonly StringName GetDefaultForType = "GetDefaultForType";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _typeOption = "_typeOption";

		public static readonly StringName _editorContainer = "_editorContainer";

		public static readonly StringName _boolCheck = "_boolCheck";

		public static readonly StringName _intSpin = "_intSpin";

		public static readonly StringName _floatSpin = "_floatSpin";

		public static readonly StringName _stringEdit = "_stringEdit";

		public static readonly StringName _colorPicker = "_colorPicker";

		public static readonly StringName _currentType = "_currentType";

		public static readonly StringName _vector2Edits = "_vector2Edits";

		public static readonly StringName _vector2iEdits = "_vector2iEdits";

		public static readonly StringName _vector3Edits = "_vector3Edits";

		public static readonly StringName _vector3iEdits = "_vector3iEdits";

		public static readonly StringName _vector4Edits = "_vector4Edits";

		public static readonly StringName _vector4iEdits = "_vector4iEdits";

		public static readonly StringName _rect2Edits = "_rect2Edits";

		public static readonly StringName _rect2iEdits = "_rect2iEdits";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Variant/XWInspectorPropertyEditorVariant.tscn";

	private OptionButton _typeOption;

	private VBoxContainer _editorContainer;

	private CheckBox _boolCheck;

	private SpinBox _intSpin;

	private SpinBox _floatSpin;

	private LineEdit _stringEdit;

	private ColorPickerButton _colorPicker;

	private Variant.Type _currentType;

	private readonly Dictionary<Variant.Type, int> _typeToIndex = new Dictionary<Variant.Type, int>();

	private readonly Dictionary<int, Variant.Type> _indexToType = new Dictionary<int, Variant.Type>();

	private SpinBox[] _vector2Edits;

	private SpinBox[] _vector2iEdits;

	private SpinBox[] _vector3Edits;

	private SpinBox[] _vector3iEdits;

	private SpinBox[] _vector4Edits;

	private SpinBox[] _vector4iEdits;

	private SpinBox[] _rect2Edits;

	private SpinBox[] _rect2iEdits;

	private static readonly Variant.Type[] SupportedTypes = new Variant.Type[13]
	{
		Variant.Type.Bool,
		Variant.Type.Int,
		Variant.Type.Float,
		Variant.Type.String,
		Variant.Type.Vector2,
		Variant.Type.Vector2I,
		Variant.Type.Vector3,
		Variant.Type.Vector3I,
		Variant.Type.Vector4,
		Variant.Type.Vector4I,
		Variant.Type.Color,
		Variant.Type.Rect2,
		Variant.Type.Rect2I
	};

	private static readonly string[] TypeNames = new string[13]
	{
		"bool", "int", "float", "String", "Vector2", "Vector2i", "Vector3", "Vector3i", "Vector4", "Vector4i",
		"Color", "Rect2", "Rect2i"
	};

	public static XWInspectorPropertyEditorVariant Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Variant/XWInspectorPropertyEditorVariant.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorVariant>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_typeOption = GetNode<OptionButton>("%TypeOption");
		_editorContainer = GetNode<VBoxContainer>("%EditorContainer");
		_boolCheck = GetNode<CheckBox>("%BoolCheck");
		_intSpin = GetNode<SpinBox>("%IntSpin");
		_floatSpin = GetNode<SpinBox>("%FloatSpin");
		_stringEdit = GetNode<LineEdit>("%StringEdit");
		_colorPicker = GetNode<ColorPickerButton>("%ColorPicker");
		XWInspectorPropertyEditorBase.NormalizeSpinBox(_intSpin);
		XWInspectorPropertyEditorBase.NormalizeSpinBox(_floatSpin);
		for (int i = 0; i < SupportedTypes.Length; i++)
		{
			_typeToIndex[SupportedTypes[i]] = i;
			_indexToType[i] = SupportedTypes[i];
			_typeOption.AddItem(TypeNames[i]);
		}
		_typeOption.ItemSelected += OnTypeSelected;
		_boolCheck.Toggled += (bool v) =>
		{
			ValueChange(v);
		};
		_intSpin.ValueChanged += (double v) =>
		{
			ValueChange((long)v);
		};
		_floatSpin.ValueChanged += (double v) =>
		{
			ValueChange(v);
		};
		_stringEdit.TextChanged += (string v) =>
		{
			ValueChange(v);
		};
		_colorPicker.ColorChanged += (Color v) =>
		{
			ValueChange(v);
		};
		_vector2Edits = CreateSpinBoxRow(2, -3.4028235E+38, 3.4028235E+38, 0.01);
		_vector2iEdits = CreateSpinBoxRow(2, -2147483648.0, 2147483647.0, 1.0);
		_vector3Edits = CreateSpinBoxRow(3, -3.4028235E+38, 3.4028235E+38, 0.01);
		_vector3iEdits = CreateSpinBoxRow(3, -2147483648.0, 2147483647.0, 1.0);
		_vector4Edits = CreateSpinBoxRow(4, -3.4028235E+38, 3.4028235E+38, 0.01);
		_vector4iEdits = CreateSpinBoxRow(4, -2147483648.0, 2147483647.0, 1.0);
		_rect2Edits = CreateSpinBoxRow(4, -3.4028235E+38, 3.4028235E+38, 0.01);
		_rect2iEdits = CreateSpinBoxRow(4, -2147483648.0, 2147483647.0, 1.0);
	}

	private SpinBox[] CreateSpinBoxRow(int count, double minVal, double maxVal, double stepVal)
	{
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			Visible = false,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddThemeConstantOverride("separation", 2);
		SpinBox[] array = new SpinBox[count];
		for (int i = 0; i < count; i++)
		{
			SpinBox spinBox = new SpinBox
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				MinValue = minVal,
				MaxValue = maxVal,
				Step = stepVal
			};
			spinBox.ValueChanged += (double _) =>
			{
				ValueChange(GetValue());
			};
			hBoxContainer.AddChild(spinBox, forceReadableName: false, InternalMode.Disabled);
			XWInspectorPropertyEditorBase.NormalizeSpinBox(spinBox);
			array[i] = spinBox;
		}
		_editorContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		return array;
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			Variant.Type variantType = propertyValue.VariantType;
			if (_typeToIndex.TryGetValue(variantType, out var value))
			{
				_typeOption.Select(value);
				_currentType = variantType;
				ShowEditorForType(variantType);
				SetEditorValue(propertyValue);
			}
		}
	}

	public override Variant GetValue()
	{
		Variant.Type currentType = _currentType;
		Variant.Type num = currentType - 1;
		if ((ulong)num <= 19uL)
		{
			switch ((int)num)
			{
			case 0:
				return _boolCheck.ButtonPressed;
			case 1:
				return (long)_intSpin.Value;
			case 2:
				return _floatSpin.Value;
			case 3:
				return _stringEdit.Text;
			case 4:
				return new Vector2((float)_vector2Edits[0].Value, (float)_vector2Edits[1].Value);
			case 5:
				return new Vector2I((int)_vector2iEdits[0].Value, (int)_vector2iEdits[1].Value);
			case 8:
				return new Vector3((float)_vector3Edits[0].Value, (float)_vector3Edits[1].Value, (float)_vector3Edits[2].Value);
			case 9:
				return new Vector3I((int)_vector3iEdits[0].Value, (int)_vector3iEdits[1].Value, (int)_vector3iEdits[2].Value);
			case 11:
				return new Vector4((float)_vector4Edits[0].Value, (float)_vector4Edits[1].Value, (float)_vector4Edits[2].Value, (float)_vector4Edits[3].Value);
			case 12:
				return new Vector4I((int)_vector4iEdits[0].Value, (int)_vector4iEdits[1].Value, (int)_vector4iEdits[2].Value, (int)_vector4iEdits[3].Value);
			case 19:
				return _colorPicker.Color;
			case 6:
				return new Rect2((float)_rect2Edits[0].Value, (float)_rect2Edits[1].Value, (float)_rect2Edits[2].Value, (float)_rect2Edits[3].Value);
			case 7:
				return new Rect2I((int)_rect2iEdits[0].Value, (int)_rect2iEdits[1].Value, (int)_rect2iEdits[2].Value, (int)_rect2iEdits[3].Value);
			}
		}
		return default;
	}

	private void ShowEditorForType(Variant.Type t)
	{
		_boolCheck.Visible = t == Variant.Type.Bool;
		_intSpin.Visible = t == Variant.Type.Int;
		_floatSpin.Visible = t == Variant.Type.Float;
		_stringEdit.Visible = t == Variant.Type.String;
		_colorPicker.Visible = t == Variant.Type.Color;
		_vector2Edits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Vector2);
		_vector2iEdits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Vector2I);
		_vector3Edits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Vector3);
		_vector3iEdits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Vector3I);
		_vector4Edits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Vector4);
		_vector4iEdits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Vector4I);
		_rect2Edits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Rect2);
		_rect2iEdits[0].GetParent().CallDeferred("set", "visible", t == Variant.Type.Rect2I);
	}

	private void SetEditorValue(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num <= 19uL)
		{
			switch ((int)num)
			{
			case 0:
				_boolCheck.ButtonPressed = value.AsBool();
				break;
			case 1:
				_intSpin.Value = value.AsInt64();
				break;
			case 2:
				_floatSpin.Value = value.AsDouble();
				break;
			case 3:
				XWInspectorPropertyEditorBase.SetLineEditTextPreservingCaret(_stringEdit, value.AsString());
				break;
			case 4:
			{
				Vector2 vector3 = value.AsVector2();
				_vector2Edits[0].Value = vector3.X;
				_vector2Edits[1].Value = vector3.Y;
				break;
			}
			case 5:
			{
				Vector2I vector2I = value.AsVector2I();
				_vector2iEdits[0].Value = vector2I.X;
				_vector2iEdits[1].Value = vector2I.Y;
				break;
			}
			case 8:
			{
				Vector3 vector2 = value.AsVector3();
				_vector3Edits[0].Value = vector2.X;
				_vector3Edits[1].Value = vector2.Y;
				_vector3Edits[2].Value = vector2.Z;
				break;
			}
			case 9:
			{
				Vector3I vector3I = value.AsVector3I();
				_vector3iEdits[0].Value = vector3I.X;
				_vector3iEdits[1].Value = vector3I.Y;
				_vector3iEdits[2].Value = vector3I.Z;
				break;
			}
			case 11:
			{
				Vector4 vector = value.AsVector4();
				_vector4Edits[0].Value = vector.X;
				_vector4Edits[1].Value = vector.Y;
				_vector4Edits[2].Value = vector.Z;
				_vector4Edits[3].Value = vector.W;
				break;
			}
			case 12:
			{
				Vector4I vector4I = value.AsVector4I();
				_vector4iEdits[0].Value = vector4I.X;
				_vector4iEdits[1].Value = vector4I.Y;
				_vector4iEdits[2].Value = vector4I.Z;
				_vector4iEdits[3].Value = vector4I.W;
				break;
			}
			case 19:
				_colorPicker.Color = value.AsColor();
				break;
			case 6:
			{
				Rect2 rect = value.AsRect2();
				_rect2Edits[0].Value = rect.Position.X;
				_rect2Edits[1].Value = rect.Position.Y;
				_rect2Edits[2].Value = rect.Size.X;
				_rect2Edits[3].Value = rect.Size.Y;
				break;
			}
			case 7:
			{
				Rect2I rect2I = value.AsRect2I();
				_rect2iEdits[0].Value = rect2I.Position.X;
				_rect2iEdits[1].Value = rect2I.Position.Y;
				_rect2iEdits[2].Value = rect2I.Size.X;
				_rect2iEdits[3].Value = rect2I.Size.Y;
				break;
			}
			case 10:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
				break;
			}
		}
	}

	private void OnTypeSelected(long index)
	{
		if (_indexToType.TryGetValue((int)index, out var value))
		{
			_currentType = value;
			ShowEditorForType(value);
			Variant defaultForType = GetDefaultForType(value);
			SetEditorValue(defaultForType);
			ValueChange(defaultForType);
		}
	}

	private static Variant GetDefaultForType(Variant.Type t)
	{
		Variant.Type num = t - 1;
		if ((ulong)num <= 19uL)
		{
			switch ((int)num)
			{
			case 0:
				return false;
			case 1:
				return 0L;
			case 2:
				return 0.0;
			case 3:
				return "";
			case 4:
				return Vector2.Zero;
			case 5:
				return Vector2I.Zero;
			case 8:
				return Vector3.Zero;
			case 9:
				return Vector3I.Zero;
			case 11:
				return Vector4.Zero;
			case 12:
				return Vector4I.Zero;
			case 19:
				return default(Color);
			case 6:
				return default(Rect2);
			case 7:
				return default(Rect2I);
			}
		}
		return default;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSpinBoxRow, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maxVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "stepVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowEditorForType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "t", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEditorValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDefaultForType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "t", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorVariant>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSpinBoxRow && args.Count == 4)
		{
			SpinBox[] array = CreateSpinBoxRow(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			GodotObject[] array2 = array;
			ret = VariantUtils.CreateFromSystemArrayOfGodotObject(array2);
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
		if (method == MethodName.ShowEditorForType && args.Count == 1)
		{
			ShowEditorForType(VariantUtils.ConvertTo<Variant.Type>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditorValue && args.Count == 1)
		{
			SetEditorValue(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTypeSelected && args.Count == 1)
		{
			OnTypeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDefaultForType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetDefaultForType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorVariant>(Create());
			return true;
		}
		if (method == MethodName.GetDefaultForType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetDefaultForType(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
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
		if (method == MethodName.CreateSpinBoxRow)
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
		if (method == MethodName.ShowEditorForType)
		{
			return true;
		}
		if (method == MethodName.SetEditorValue)
		{
			return true;
		}
		if (method == MethodName.OnTypeSelected)
		{
			return true;
		}
		if (method == MethodName.GetDefaultForType)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._typeOption)
		{
			_typeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			_editorContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._boolCheck)
		{
			_boolCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._intSpin)
		{
			_intSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._floatSpin)
		{
			_floatSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._stringEdit)
		{
			_stringEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._colorPicker)
		{
			_colorPicker = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._currentType)
		{
			_currentType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._vector2Edits)
		{
			_vector2Edits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._vector2iEdits)
		{
			_vector2iEdits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._vector3Edits)
		{
			_vector3Edits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._vector3iEdits)
		{
			_vector3iEdits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._vector4Edits)
		{
			_vector4Edits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._vector4iEdits)
		{
			_vector4iEdits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rect2Edits)
		{
			_rect2Edits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rect2iEdits)
		{
			_rect2iEdits = VariantUtils.ConvertToSystemArrayOfGodotObject<SpinBox>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._typeOption)
		{
			value = VariantUtils.CreateFrom(in _typeOption);
			return true;
		}
		if (name == PropertyName._editorContainer)
		{
			value = VariantUtils.CreateFrom(in _editorContainer);
			return true;
		}
		if (name == PropertyName._boolCheck)
		{
			value = VariantUtils.CreateFrom(in _boolCheck);
			return true;
		}
		if (name == PropertyName._intSpin)
		{
			value = VariantUtils.CreateFrom(in _intSpin);
			return true;
		}
		if (name == PropertyName._floatSpin)
		{
			value = VariantUtils.CreateFrom(in _floatSpin);
			return true;
		}
		if (name == PropertyName._stringEdit)
		{
			value = VariantUtils.CreateFrom(in _stringEdit);
			return true;
		}
		if (name == PropertyName._colorPicker)
		{
			value = VariantUtils.CreateFrom(in _colorPicker);
			return true;
		}
		if (name == PropertyName._currentType)
		{
			value = VariantUtils.CreateFrom(in _currentType);
			return true;
		}
		if (name == PropertyName._vector2Edits)
		{
			GodotObject[] vector2Edits = _vector2Edits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._vector2iEdits)
		{
			GodotObject[] vector2Edits = _vector2iEdits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._vector3Edits)
		{
			GodotObject[] vector2Edits = _vector3Edits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._vector3iEdits)
		{
			GodotObject[] vector2Edits = _vector3iEdits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._vector4Edits)
		{
			GodotObject[] vector2Edits = _vector4Edits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._vector4iEdits)
		{
			GodotObject[] vector2Edits = _vector4iEdits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._rect2Edits)
		{
			GodotObject[] vector2Edits = _rect2Edits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		if (name == PropertyName._rect2iEdits)
		{
			GodotObject[] vector2Edits = _rect2iEdits;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(vector2Edits);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._typeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._boolCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._intSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._floatSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stringEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._colorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._vector2Edits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._vector2iEdits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._vector3Edits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._vector3iEdits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._vector4Edits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._vector4iEdits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._rect2Edits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._rect2iEdits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._typeOption, Variant.From(in _typeOption));
		info.AddProperty(PropertyName._editorContainer, Variant.From(in _editorContainer));
		info.AddProperty(PropertyName._boolCheck, Variant.From(in _boolCheck));
		info.AddProperty(PropertyName._intSpin, Variant.From(in _intSpin));
		info.AddProperty(PropertyName._floatSpin, Variant.From(in _floatSpin));
		info.AddProperty(PropertyName._stringEdit, Variant.From(in _stringEdit));
		info.AddProperty(PropertyName._colorPicker, Variant.From(in _colorPicker));
		info.AddProperty(PropertyName._currentType, Variant.From(in _currentType));
		StringName vector2Edits = PropertyName._vector2Edits;
		GodotObject[] vector2Edits2 = _vector2Edits;
		info.AddProperty(vector2Edits, Variant.CreateFrom(vector2Edits2));
		StringName vector2iEdits = PropertyName._vector2iEdits;
		vector2Edits2 = _vector2iEdits;
		info.AddProperty(vector2iEdits, Variant.CreateFrom(vector2Edits2));
		StringName vector3Edits = PropertyName._vector3Edits;
		vector2Edits2 = _vector3Edits;
		info.AddProperty(vector3Edits, Variant.CreateFrom(vector2Edits2));
		StringName vector3iEdits = PropertyName._vector3iEdits;
		vector2Edits2 = _vector3iEdits;
		info.AddProperty(vector3iEdits, Variant.CreateFrom(vector2Edits2));
		StringName vector4Edits = PropertyName._vector4Edits;
		vector2Edits2 = _vector4Edits;
		info.AddProperty(vector4Edits, Variant.CreateFrom(vector2Edits2));
		StringName vector4iEdits = PropertyName._vector4iEdits;
		vector2Edits2 = _vector4iEdits;
		info.AddProperty(vector4iEdits, Variant.CreateFrom(vector2Edits2));
		StringName rect2Edits = PropertyName._rect2Edits;
		vector2Edits2 = _rect2Edits;
		info.AddProperty(rect2Edits, Variant.CreateFrom(vector2Edits2));
		StringName rect2iEdits = PropertyName._rect2iEdits;
		vector2Edits2 = _rect2iEdits;
		info.AddProperty(rect2iEdits, Variant.CreateFrom(vector2Edits2));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._typeOption, out var value))
		{
			_typeOption = value.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._editorContainer, out var value2))
		{
			_editorContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._boolCheck, out var value3))
		{
			_boolCheck = value3.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._intSpin, out var value4))
		{
			_intSpin = value4.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._floatSpin, out var value5))
		{
			_floatSpin = value5.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._stringEdit, out var value6))
		{
			_stringEdit = value6.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._colorPicker, out var value7))
		{
			_colorPicker = value7.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._currentType, out var value8))
		{
			_currentType = value8.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._vector2Edits, out var value9))
		{
			_vector2Edits = value9.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._vector2iEdits, out var value10))
		{
			_vector2iEdits = value10.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._vector3Edits, out var value11))
		{
			_vector3Edits = value11.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._vector3iEdits, out var value12))
		{
			_vector3iEdits = value12.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._vector4Edits, out var value13))
		{
			_vector4Edits = value13.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._vector4iEdits, out var value14))
		{
			_vector4iEdits = value14.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rect2Edits, out var value15))
		{
			_rect2Edits = value15.AsGodotObjectArray<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rect2iEdits, out var value16))
		{
			_rect2iEdits = value16.AsGodotObjectArray<SpinBox>();
		}
	}
}
