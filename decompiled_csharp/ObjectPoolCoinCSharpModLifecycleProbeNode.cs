using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public class ObjectPoolCoinCSharpModLifecycleProbeNode : TowerDefenseCoinBase
{
	public new class MethodName : TowerDefenseCoinBase.MethodName
	{
		public new static readonly StringName Refresh = "Refresh";

		public new static readonly StringName Recycle = "Recycle";

		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : TowerDefenseCoinBase.PropertyName
	{
		public static readonly StringName ModRefreshCalls = "ModRefreshCalls";

		public static readonly StringName ModRecycleCalls = "ModRecycleCalls";

		public static readonly StringName ModRefreshParent = "ModRefreshParent";

		public static readonly StringName ModRecycleParent = "ModRecycleParent";
	}

	public new class SignalName : TowerDefenseCoinBase.SignalName
	{
	}

	public int ModRefreshCalls { get; private set; }

	public int ModRecycleCalls { get; private set; }

	public Node ModRefreshParent { get; private set; }

	public Node ModRecycleParent { get; private set; }

	public new void Refresh()
	{
		ModRefreshCalls++;
		ModRefreshParent = GetParent();
	}

	public new void Recycle()
	{
		ModRecycleCalls++;
		ModRecycleParent = GetParent();
	}

	public override void _Ready()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Recycle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.Recycle)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ModRefreshCalls)
		{
			ModRefreshCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ModRecycleCalls)
		{
			ModRecycleCalls = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ModRefreshParent)
		{
			ModRefreshParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.ModRecycleParent)
		{
			ModRecycleParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.ModRefreshCalls)
		{
			from = ModRefreshCalls;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ModRecycleCalls)
		{
			from = ModRecycleCalls;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Node from2;
		if (name == PropertyName.ModRefreshParent)
		{
			from2 = ModRefreshParent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ModRecycleParent)
		{
			from2 = ModRecycleParent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ModRefreshCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ModRecycleCalls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ModRefreshParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ModRecycleParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ModRefreshCalls, Variant.From<int>(ModRefreshCalls));
		info.AddProperty(PropertyName.ModRecycleCalls, Variant.From<int>(ModRecycleCalls));
		info.AddProperty(PropertyName.ModRefreshParent, Variant.From<Node>(ModRefreshParent));
		info.AddProperty(PropertyName.ModRecycleParent, Variant.From<Node>(ModRecycleParent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ModRefreshCalls, out var value))
		{
			ModRefreshCalls = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ModRecycleCalls, out var value2))
		{
			ModRecycleCalls = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ModRefreshParent, out var value3))
		{
			ModRefreshParent = value3.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.ModRecycleParent, out var value4))
		{
			ModRecycleParent = value4.As<Node>();
		}
	}
}
