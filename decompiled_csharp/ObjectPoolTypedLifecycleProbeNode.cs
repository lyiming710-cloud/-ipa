using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ObjectPoolTypedLifecycleProbeNode : Node, IObjectPoolLifecycle
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName ResetRefreshCalls = "ResetRefreshCalls";

		public static readonly StringName ResetRecycleCalls = "ResetRecycleCalls";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName Recycle = "Recycle";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	public static long RefreshCalls { get; private set; }

	public static long RecycleCalls { get; private set; }

	bool IObjectPoolLifecycle.SupportsDirectPoolLifecycleDispatch => true;

	public static void ResetRefreshCalls()
	{
		RefreshCalls = 0L;
	}

	public static void ResetRecycleCalls()
	{
		RecycleCalls = 0L;
	}

	public void Refresh()
	{
		RefreshCalls++;
	}

	public void Recycle()
	{
		RecycleCalls++;
	}

	void IObjectPoolLifecycle.RefreshFromPool()
	{
		Refresh();
	}

	void IObjectPoolLifecycle.RecycleToPool()
	{
		Recycle();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.ResetRefreshCalls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ResetRecycleCalls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResetRefreshCalls && args.Count == 0)
		{
			ResetRefreshCalls();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetRecycleCalls && args.Count == 0)
		{
			ResetRecycleCalls();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.Recycle && args.Count == 0)
		{
			Recycle();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResetRefreshCalls && args.Count == 0)
		{
			ResetRefreshCalls();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetRecycleCalls && args.Count == 0)
		{
			ResetRecycleCalls();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ResetRefreshCalls)
		{
			return true;
		}
		if (method == MethodName.ResetRecycleCalls)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.Recycle)
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
