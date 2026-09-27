using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/DropItem/Handler/DropItemHandler.cs")]
public class DropItemHandler : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName OnCollect = "OnCollect";

		public static readonly StringName OnSpawn = "OnSpawn";

		public static readonly StringName OnDestroy = "OnDestroy";

		public static readonly StringName GetCollectValue = "GetCollectValue";

		public static readonly StringName ShouldAutoCollect = "ShouldAutoCollect";

		public static readonly StringName Reset = "Reset";
	}

	public new class PropertyName : Resource.PropertyName
	{
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public virtual void OnCollect(Vector2 pos, long value)
	{
	}

	public virtual void OnCollect(DropItemCollectionContext context)
	{
		if (context.UsesLegacyLocalEconomy)
		{
			OnCollect(context.Position, context.Value);
		}
		else
		{
			GD.PushWarning("Drop handler " + GetType().Name + " rejected account-owned collection because it only implements the legacy API.");
		}
	}

	public virtual void OnSpawn(Vector2 pos)
	{
	}

	public virtual void OnDestroy()
	{
	}

	public virtual long GetCollectValue(long baseValue)
	{
		return baseValue;
	}

	public virtual long GetCollectValue(DropItemCollectionContext context)
	{
		return GetCollectValue(context.Value);
	}

	public virtual bool ShouldAutoCollect()
	{
		return false;
	}

	public virtual bool ShouldAutoCollect(DropItemCollectionContext context)
	{
		return ShouldAutoCollect();
	}

	public virtual void Reset()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.OnCollect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCollectValue, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "baseValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldAutoCollect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Reset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OnCollect && args.Count == 2)
		{
			OnCollect(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSpawn && args.Count == 1)
		{
			OnSpawn(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDestroy && args.Count == 0)
		{
			OnDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCollectValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(GetCollectValue(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldAutoCollect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAutoCollect());
			return true;
		}
		if (method == MethodName.Reset && args.Count == 0)
		{
			Reset();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.OnCollect)
		{
			return true;
		}
		if (method == MethodName.OnSpawn)
		{
			return true;
		}
		if (method == MethodName.OnDestroy)
		{
			return true;
		}
		if (method == MethodName.GetCollectValue)
		{
			return true;
		}
		if (method == MethodName.ShouldAutoCollect)
		{
			return true;
		}
		if (method == MethodName.Reset)
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
