using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Resource/Cell/Config/TowerDefenseCellConfig.cs")]
public class TowerDefenseCellConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName ElementFlags = "ElementFlags";

		public static readonly StringName pos = "pos";

		public static readonly StringName groundHeightCurve = "groundHeightCurve";

		public static readonly StringName gridType = "gridType";

		public static readonly StringName _elementFlags = "_elementFlags";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Vector4I pos = Vector4I.Zero;

	[Export(PropertyHint.None, "")]
	public CurveTexture groundHeightCurve;

	[ExportCategory("Setting")]
	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseEnum.PLANTGRIDTYPE> gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
	{
		TowerDefenseEnum.PLANTGRIDTYPE.GROUND,
		TowerDefenseEnum.PLANTGRIDTYPE.AIR
	};

	private int _elementFlags;

	[Export(PropertyHint.None, "")]
	public int ElementFlags
	{
		get
		{
			return _elementFlags;
		}
		set
		{
			_elementFlags = value;
		}
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		return new Array<Dictionary>
		{
			new Dictionary
			{
				["name"] = "Flag/Elemet",
				["type"] = 2,
				["hint"] = 6,
				["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.ELEMENT_SYSTEM>()),
				["usage"] = num
			}
		};
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property.ToString() == "Flag/Elemet")
		{
			_elementFlags = value.AsInt32();
			NotifyPropertyListChanged();
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if (property.ToString() == "Flag/Elemet")
		{
			return _elementFlags;
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if (property.ToString() == "Flag/Elemet")
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (property.ToString() == "Flag/Elemet")
		{
			return 0;
		}
		return default;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyCanRevert, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyGetRevert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ElementFlags)
		{
			ElementFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pos)
		{
			pos = VariantUtils.ConvertTo<Vector4I>(in value);
			return true;
		}
		if (name == PropertyName.groundHeightCurve)
		{
			groundHeightCurve = VariantUtils.ConvertTo<CurveTexture>(in value);
			return true;
		}
		if (name == PropertyName.gridType)
		{
			gridType = VariantUtils.ConvertToArray<TowerDefenseEnum.PLANTGRIDTYPE>(in value);
			return true;
		}
		if (name == PropertyName._elementFlags)
		{
			_elementFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ElementFlags)
		{
			value = VariantUtils.CreateFrom<int>(ElementFlags);
			return true;
		}
		if (name == PropertyName.pos)
		{
			value = VariantUtils.CreateFrom(in pos);
			return true;
		}
		if (name == PropertyName.groundHeightCurve)
		{
			value = VariantUtils.CreateFrom(in groundHeightCurve);
			return true;
		}
		if (name == PropertyName.gridType)
		{
			value = VariantUtils.CreateFromArray(gridType);
			return true;
		}
		if (name == PropertyName._elementFlags)
		{
			value = VariantUtils.CreateFrom(in _elementFlags);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector4I, PropertyName.pos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.groundHeightCurve, PropertyHint.ResourceType, "CurveTexture", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Setting", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.gridType, PropertyHint.TypeString, "2/2:ALL:-1,NOONE:0,SOIL:1,GROUND:2,WATER:3,AIR:4,LILYPAD:5,POT:6,SURROUND:7,GRAVESTONE:8,CRATER:9,BRICK:10,ICECAP:11,PLANT:12", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._elementFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ElementFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ElementFlags, Variant.From<int>(ElementFlags));
		info.AddProperty(PropertyName.pos, Variant.From(in pos));
		info.AddProperty(PropertyName.groundHeightCurve, Variant.From(in groundHeightCurve));
		info.AddProperty(PropertyName.gridType, Variant.CreateFrom(gridType));
		info.AddProperty(PropertyName._elementFlags, Variant.From(in _elementFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ElementFlags, out var value))
		{
			ElementFlags = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pos, out var value2))
		{
			pos = value2.As<Vector4I>();
		}
		if (info.TryGetProperty(PropertyName.groundHeightCurve, out var value3))
		{
			groundHeightCurve = value3.As<CurveTexture>();
		}
		if (info.TryGetProperty(PropertyName.gridType, out var value4))
		{
			gridType = value4.AsGodotArray<TowerDefenseEnum.PLANTGRIDTYPE>();
		}
		if (info.TryGetProperty(PropertyName._elementFlags, out var value5))
		{
			_elementFlags = value5.As<int>();
		}
	}
}
