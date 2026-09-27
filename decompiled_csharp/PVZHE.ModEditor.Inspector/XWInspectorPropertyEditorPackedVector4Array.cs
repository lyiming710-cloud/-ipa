using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/PackedVector4Array/XWInspectorPropertyEditorPackedVector4Array.cs")]
public class XWInspectorPropertyEditorPackedVector4Array : XWInspectorPropertyEditorPackedArrayBase
{
	public new class MethodName : XWInspectorPropertyEditorPackedArrayBase.MethodName
	{
		public new static readonly StringName GetDefaultElement = "GetDefaultElement";

		public new static readonly StringName GetArraySize = "GetArraySize";

		public new static readonly StringName GetElement = "GetElement";

		public new static readonly StringName SetElement = "SetElement";

		public new static readonly StringName AppendElement = "AppendElement";

		public new static readonly StringName RemoveAt = "RemoveAt";

		public new static readonly StringName GetArrayAsVariant = "GetArrayAsVariant";

		public new static readonly StringName LoadArrayFromVariant = "LoadArrayFromVariant";
	}

	public new class PropertyName : XWInspectorPropertyEditorPackedArrayBase.PropertyName
	{
		public new static readonly StringName ArrayTypeName = "ArrayTypeName";

		public new static readonly StringName ElementType = "ElementType";

		public static readonly StringName _array = "_array";
	}

	public new class SignalName : XWInspectorPropertyEditorPackedArrayBase.SignalName
	{
	}

	private Vector4[] _array = Array.Empty<Vector4>();

	protected override string ArrayTypeName => "PackedVector4Array";

	protected override Variant.Type ElementType => Variant.Type.Vector4;

	protected override Variant GetDefaultElement()
	{
		return Vector4.Zero;
	}

	protected override int GetArraySize()
	{
		return _array.Length;
	}

	protected override Variant GetElement(int index)
	{
		return _array[index];
	}

	protected override void SetElement(int index, Variant value)
	{
		_array[index] = value.AsVector4();
	}

	protected override void AppendElement(Variant value)
	{
		Vector4[] array = new Vector4[_array.Length + 1];
		_array.CopyTo(array, 0);
		array[_array.Length] = value.AsVector4();
		_array = array;
	}

	protected override void RemoveAt(int index)
	{
		Vector4[] array = new Vector4[_array.Length - 1];
		Array.Copy(_array, 0, array, 0, index);
		Array.Copy(_array, index + 1, array, index, _array.Length - index - 1);
		_array = array;
	}

	protected override Variant GetArrayAsVariant()
	{
		return _array;
	}

	protected override void LoadArrayFromVariant(Variant value)
	{
		if (value.VariantType == Variant.Type.PackedVector4Array)
		{
			_array = value.AsVector4Array();
		}
		else
		{
			_array = Array.Empty<Vector4>();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.GetDefaultElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetArraySize, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AppendElement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArrayAsVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadArrayFromVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetDefaultElement && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetDefaultElement());
			return true;
		}
		if (method == MethodName.GetArraySize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetArraySize());
			return true;
		}
		if (method == MethodName.GetElement && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetElement(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetElement && args.Count == 2)
		{
			SetElement(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AppendElement && args.Count == 1)
		{
			AppendElement(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveAt && args.Count == 1)
		{
			RemoveAt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetArrayAsVariant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetArrayAsVariant());
			return true;
		}
		if (method == MethodName.LoadArrayFromVariant && args.Count == 1)
		{
			LoadArrayFromVariant(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetDefaultElement)
		{
			return true;
		}
		if (method == MethodName.GetArraySize)
		{
			return true;
		}
		if (method == MethodName.GetElement)
		{
			return true;
		}
		if (method == MethodName.SetElement)
		{
			return true;
		}
		if (method == MethodName.AppendElement)
		{
			return true;
		}
		if (method == MethodName.RemoveAt)
		{
			return true;
		}
		if (method == MethodName.GetArrayAsVariant)
		{
			return true;
		}
		if (method == MethodName.LoadArrayFromVariant)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._array)
		{
			_array = VariantUtils.ConvertTo<Vector4[]>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ArrayTypeName)
		{
			value = VariantUtils.CreateFrom<string>(ArrayTypeName);
			return true;
		}
		if (name == PropertyName.ElementType)
		{
			value = VariantUtils.CreateFrom<Variant.Type>(ElementType);
			return true;
		}
		if (name == PropertyName._array)
		{
			value = VariantUtils.CreateFrom(in _array);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.PackedVector4Array, PropertyName._array, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ArrayTypeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ElementType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._array, Variant.From(in _array));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._array, out var value))
		{
			_array = value.As<Vector4[]>();
		}
	}
}
