using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Projection/XWInspectorPropertyEditorProjection.cs")]
public class XWInspectorPropertyEditorProjection : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnSpinBoxChanged = "OnSpinBoxChanged";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _spinBoxes = "_spinBoxes";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Projection/XWInspectorPropertyEditorProjection.tscn";

	private readonly SpinBox[] _spinBoxes = new SpinBox[16];

	public static XWInspectorPropertyEditorProjection Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Projection/XWInspectorPropertyEditorProjection.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorProjection>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		GridContainer node = GetNode<GridContainer>("%EditorContainer");
		node.Columns = 4;
		for (int i = 0; i < 16; i++)
		{
			SpinBox spinBox = new SpinBox
			{
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				MinValue = -9999999999.0,
				MaxValue = 9999999999.0,
				Step = 0.001,
				AllowGreater = true,
				AllowLesser = true
			};
			node.AddChild(spinBox, forceReadableName: false, InternalMode.Disabled);
			XWInspectorPropertyEditorBase.NormalizeSpinBox(spinBox);
			spinBox.ValueChanged += (double _) =>
			{
				OnSpinBoxChanged();
			};
			_spinBoxes[i] = spinBox;
		}
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Projection)
		{
			Projection projection = propertyValue.AsProjection();
			for (int i = 0; i < 4; i++)
			{
				Vector4 vector = projection[i];
				_spinBoxes[i * 4].SetValueNoSignal(vector.X);
				_spinBoxes[i * 4 + 1].SetValueNoSignal(vector.Y);
				_spinBoxes[i * 4 + 2].SetValueNoSignal(vector.Z);
				_spinBoxes[i * 4 + 3].SetValueNoSignal(vector.W);
			}
		}
	}

	public override Variant GetValue()
	{
		Projection projection = default;
		for (int i = 0; i < 4; i++)
		{
			projection[i] = new Vector4((float)_spinBoxes[i * 4].Value, (float)_spinBoxes[i * 4 + 1].Value, (float)_spinBoxes[i * 4 + 2].Value, (float)_spinBoxes[i * 4 + 3].Value);
		}
		return projection;
	}

	private void OnSpinBoxChanged()
	{
		ValueChange(GetValue());
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
			new MethodInfo(MethodName.OnSpinBoxChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorProjection>(Create());
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
		if (method == MethodName.OnSpinBoxChanged && args.Count == 0)
		{
			OnSpinBoxChanged();
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
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorProjection>(Create());
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
		if (method == MethodName.OnSpinBoxChanged)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spinBoxes)
		{
			GodotObject[] spinBoxes = _spinBoxes;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(spinBoxes);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._spinBoxes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
