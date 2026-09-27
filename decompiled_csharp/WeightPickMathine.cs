using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/Core/WeightPick/WeightPickMathine.cs")]
public class WeightPickMathine : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Pick = "Pick";

		public static readonly StringName PickF = "PickF";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static WeightPickItemBase Pick(Array<WeightPickItemBase> items)
	{
		if (items.Count > 0)
		{
			int num = 0;
			foreach (WeightPickItemBase item in items)
			{
				num += (int)item.weight;
			}
			if (num <= 0)
			{
				return null;
			}
			int num2 = (int)(GD.Randi() % (uint)num);
			foreach (WeightPickItemBase item2 in items)
			{
				num2 -= (int)item2.weight;
				if (num2 <= 0)
				{
					return item2;
				}
			}
		}
		return null;
	}

	public static WeightPickItemBase PickF(Array<WeightPickItemBase> items)
	{
		if (items.Count > 0)
		{
			double num = 0.0;
			foreach (WeightPickItemBase item in items)
			{
				num += item.weight;
			}
			if (!(num > 0.0))
			{
				return null;
			}
			double num2 = GD.Randf();
			foreach (WeightPickItemBase item2 in items)
			{
				num2 -= item2.weight;
				if (num2 <= 0.0)
				{
					return item2;
				}
			}
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Pick, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "items", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PickF, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "items", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Pick && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<WeightPickItemBase>(Pick(VariantUtils.ConvertToArray<WeightPickItemBase>(in args[0])));
			return true;
		}
		if (method == MethodName.PickF && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<WeightPickItemBase>(PickF(VariantUtils.ConvertToArray<WeightPickItemBase>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Pick && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<WeightPickItemBase>(Pick(VariantUtils.ConvertToArray<WeightPickItemBase>(in args[0])));
			return true;
		}
		if (method == MethodName.PickF && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<WeightPickItemBase>(PickF(VariantUtils.ConvertToArray<WeightPickItemBase>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Pick)
		{
			return true;
		}
		if (method == MethodName.PickF)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
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
