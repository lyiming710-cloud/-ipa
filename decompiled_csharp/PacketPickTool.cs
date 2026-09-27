using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketPick/Control/PacketPickTool.cs")]
public class PacketPickTool : Node
{
	public new class MethodName : Node.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName IsPicking = "IsPicking";

		public static readonly StringName PickTool = "PickTool";

		public static readonly StringName ProcessPick = "ProcessPick";

		public static readonly StringName ToolRelease = "ToolRelease";

		public static readonly StringName ToolReset = "ToolReset";

		public static readonly StringName GetMapSprite = "GetMapSprite";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName mapControl = "mapControl";

		public static readonly StringName toolPick = "toolPick";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public TowerDefenseMapControl mapControl;

	public bool toolPick;

	public virtual void Init(TowerDefenseMapControl _mapControl)
	{
		mapControl = _mapControl;
	}

	public virtual bool IsPicking()
	{
		return toolPick;
	}

	public virtual void PickTool(bool open)
	{
		toolPick = open;
	}

	public virtual void ProcessPick(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
	}

	public virtual void ToolRelease()
	{
		toolPick = false;
	}

	public virtual void ToolReset()
	{
		toolPick = false;
	}

	public virtual Node2D GetMapSprite()
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsPicking, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PickTool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessPick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToolRelease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToolReset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPicking && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPicking());
			return true;
		}
		if (method == MethodName.PickTool && args.Count == 1)
		{
			PickTool(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessPick && args.Count == 3)
		{
			ProcessPick(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToolRelease && args.Count == 0)
		{
			ToolRelease();
			ret = default;
			return true;
		}
		if (method == MethodName.ToolReset && args.Count == 0)
		{
			ToolReset();
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapSprite && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(GetMapSprite());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.IsPicking)
		{
			return true;
		}
		if (method == MethodName.PickTool)
		{
			return true;
		}
		if (method == MethodName.ProcessPick)
		{
			return true;
		}
		if (method == MethodName.ToolRelease)
		{
			return true;
		}
		if (method == MethodName.ToolReset)
		{
			return true;
		}
		if (method == MethodName.GetMapSprite)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mapControl)
		{
			mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName.toolPick)
		{
			toolPick = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mapControl)
		{
			value = VariantUtils.CreateFrom(in mapControl);
			return true;
		}
		if (name == PropertyName.toolPick)
		{
			value = VariantUtils.CreateFrom(in toolPick);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.toolPick, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mapControl, Variant.From(in mapControl));
		info.AddProperty(PropertyName.toolPick, Variant.From(in toolPick));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mapControl, out var value))
		{
			mapControl = value.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName.toolPick, out var value2))
		{
			toolPick = value2.As<bool>();
		}
	}
}
