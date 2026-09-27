using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ObjectPoolCustomLifecycleProbeNode : Node, IObjectPoolLifecycle
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName ModRefresh = "ModRefresh";

		public static readonly StringName ModRecycle = "ModRecycle";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName TypedRefreshCalls = "TypedRefreshCalls";

		public static readonly StringName TypedRecycleCalls = "TypedRecycleCalls";

		public static readonly StringName CustomRefreshCalls = "CustomRefreshCalls";

		public static readonly StringName CustomRecycleCalls = "CustomRecycleCalls";

		public static readonly StringName RefreshParent = "RefreshParent";

		public static readonly StringName RecycleParent = "RecycleParent";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public int TypedRefreshCalls { get; private set; }

	public int TypedRecycleCalls { get; private set; }

	public int CustomRefreshCalls { get; private set; }

	public int CustomRecycleCalls { get; private set; }

	public Node RefreshParent { get; private set; }

	public Node RecycleParent { get; private set; }

	bool IObjectPoolLifecycle.SupportsDirectPoolLifecycleDispatch => true;

	public void ModRefresh()
	{
		CustomRefreshCalls++;
		RefreshParent = GetParent();
	}

	public void ModRecycle()
	{
		CustomRecycleCalls++;
		RecycleParent = GetParent();
	}

	void IObjectPoolLifecycle.RefreshFromPool()
	{
		TypedRefreshCalls++;
	}

	void IObjectPoolLifecycle.RecycleToPool()
	{
		TypedRecycleCalls++;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.ModRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ModRecycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ModRefresh && args.Count == 0)
		{
			ModRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ModRecycle && args.Count == 0)
		{
			ModRecycle();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ModRefresh)
		{
			return true;
		}
		if (method == MethodName.ModRecycle)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.TypedRefreshCalls)
		{
			TypedRefreshCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TypedRecycleCalls)
		{
			TypedRecycleCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CustomRefreshCalls)
		{
			CustomRefreshCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CustomRecycleCalls)
		{
			CustomRecycleCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RefreshParent)
		{
			RefreshParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.RecycleParent)
		{
			RecycleParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.TypedRefreshCalls)
		{
			from = TypedRefreshCalls;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.TypedRecycleCalls)
		{
			from = TypedRecycleCalls;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CustomRefreshCalls)
		{
			from = CustomRefreshCalls;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CustomRecycleCalls)
		{
			from = CustomRecycleCalls;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Node from2;
		if (name == PropertyName.RefreshParent)
		{
			from2 = RefreshParent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RecycleParent)
		{
			from2 = RecycleParent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.TypedRefreshCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TypedRecycleCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CustomRefreshCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CustomRecycleCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.RefreshParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.RecycleParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.TypedRefreshCalls, Variant.From<int>(TypedRefreshCalls));
		info.AddProperty(PropertyName.TypedRecycleCalls, Variant.From<int>(TypedRecycleCalls));
		info.AddProperty(PropertyName.CustomRefreshCalls, Variant.From<int>(CustomRefreshCalls));
		info.AddProperty(PropertyName.CustomRecycleCalls, Variant.From<int>(CustomRecycleCalls));
		info.AddProperty(PropertyName.RefreshParent, Variant.From<Node>(RefreshParent));
		info.AddProperty(PropertyName.RecycleParent, Variant.From<Node>(RecycleParent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.TypedRefreshCalls, out var value))
		{
			TypedRefreshCalls = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TypedRecycleCalls, out var value2))
		{
			TypedRecycleCalls = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CustomRefreshCalls, out var value3))
		{
			CustomRefreshCalls = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CustomRecycleCalls, out var value4))
		{
			CustomRecycleCalls = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RefreshParent, out var value5))
		{
			RefreshParent = value5.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.RecycleParent, out var value6))
		{
			RecycleParent = value6.As<Node>();
		}
	}
}
