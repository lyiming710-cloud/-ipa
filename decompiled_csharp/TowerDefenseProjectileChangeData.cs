using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Projectile/Data/TowerDefenseProjectileChangeData.cs")]
public class TowerDefenseProjectileChangeData : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName AddChange = "AddChange";

		public static readonly StringName GetChange = "GetChange";

		public static readonly StringName HasChange = "HasChange";

		public static readonly StringName HasChangeTarget = "HasChangeTarget";

		public static readonly StringName IsEmpty = "IsEmpty";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public List<StringName> ChangeList = new List<StringName>();

	public Dictionary<StringName, StringName> ChangeToProjectile = new Dictionary<StringName, StringName>();

	public void AddChange(StringName projectileName, StringName toProjectileName)
	{
		ChangeToProjectile[projectileName] = toProjectileName;
		ChangeList.Add(projectileName);
	}

	public StringName GetChange(StringName projectileName)
	{
		if (!HasChange(projectileName))
		{
			return null;
		}
		return ChangeToProjectile[projectileName];
	}

	public bool HasChange(StringName projectileName)
	{
		return ChangeList.Contains(projectileName);
	}

	public bool HasChangeTarget(StringName projectileName)
	{
		return ChangeToProjectile.Values.Contains(projectileName);
	}

	public bool IsEmpty()
	{
		return ChangeList.Count == 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.AddChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toProjectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetChange, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasChange, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasChangeTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "projectileName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddChange && args.Count == 2)
		{
			AddChange(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetChange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetChange(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasChange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasChange(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.HasChangeTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasChangeTarget(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEmpty());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddChange)
		{
			return true;
		}
		if (method == MethodName.GetChange)
		{
			return true;
		}
		if (method == MethodName.HasChange)
		{
			return true;
		}
		if (method == MethodName.HasChangeTarget)
		{
			return true;
		}
		if (method == MethodName.IsEmpty)
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
