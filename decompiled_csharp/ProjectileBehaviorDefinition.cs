using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Behavior/Projectile/ProjectileBehaviorDefinition.cs")]
public class ProjectileBehaviorDefinition : BehaviorDefinitionBase
{
	public new class MethodName : BehaviorDefinitionBase.MethodName
	{
		public static readonly StringName ReadLegacyArray = "ReadLegacyArray";
	}

	public new class PropertyName : BehaviorDefinitionBase.PropertyName
	{
	}

	public new class SignalName : BehaviorDefinitionBase.SignalName
	{
	}

	public ProjectileBehaviorDefinition()
	{
		BehaviorTypeId = new StringName("projectile");
	}

	public virtual ProjectileBehaviorKernel CreateBulletFieldKernel(int capacity)
	{
		return null;
	}

	public static Array<ProjectileBehaviorDefinition> ReadLegacyArray(Variant value)
	{
		Array<ProjectileBehaviorDefinition> array = new Array<ProjectileBehaviorDefinition>();
		if (value.VariantType != Variant.Type.Array)
		{
			return array;
		}
		foreach (Variant item2 in value.AsGodotArray())
		{
			if (item2.VariantType == Variant.Type.Object && item2.AsGodotObject() is ProjectileBehaviorDefinition item)
			{
				array.Add(item);
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.ReadLegacyArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadLegacyArray && args.Count == 1)
		{
			Array<ProjectileBehaviorDefinition> array = ReadLegacyArray(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadLegacyArray && args.Count == 1)
		{
			Array<ProjectileBehaviorDefinition> array = ReadLegacyArray(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ReadLegacyArray)
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
