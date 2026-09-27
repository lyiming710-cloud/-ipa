using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Shovel/ShovelManager/ShovelPickTool.cs")]
public class ShovelPickTool : PacketPickTool
{
	public new class MethodName : PacketPickTool.MethodName
	{
		public new static readonly StringName Init = "Init";

		public static readonly StringName SetShovelManager = "SetShovelManager";

		public new static readonly StringName IsPicking = "IsPicking";

		public new static readonly StringName PickTool = "PickTool";

		public new static readonly StringName ProcessPick = "ProcessPick";

		public new static readonly StringName ToolRelease = "ToolRelease";

		public new static readonly StringName ToolReset = "ToolReset";

		public new static readonly StringName GetMapSprite = "GetMapSprite";
	}

	public new class PropertyName : PacketPickTool.PropertyName
	{
		public static readonly StringName shovelManager = "shovelManager";
	}

	public new class SignalName : PacketPickTool.SignalName
	{
	}

	public ShovelManager shovelManager;

	public override void Init(TowerDefenseMapControl _mapControl)
	{
		base.Init(_mapControl);
	}

	public void SetShovelManager(ShovelManager _shovelManager)
	{
		shovelManager = _shovelManager;
	}

	public override bool IsPicking()
	{
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			return shovelManager.shovelPick;
		}
		return false;
	}

	public override void PickTool(bool open)
	{
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			shovelManager.PickShovel(open);
		}
	}

	public override void ProcessPick(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			shovelManager.ProcessShovelPick(cell, gridPos, mousePos);
		}
	}

	public override void ToolRelease()
	{
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			shovelManager.ShovelRelease();
		}
	}

	public override void ToolReset()
	{
		if (GodotObject.IsInstanceValid(shovelManager))
		{
			shovelManager.ShovelReset();
		}
	}

	public override Node2D GetMapSprite()
	{
		if (GodotObject.IsInstanceValid(shovelManager) && GodotObject.IsInstanceValid(shovelManager.mapShovelSprite))
		{
			return shovelManager.mapShovelSprite;
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetShovelManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_shovelManager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
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
		if (method == MethodName.SetShovelManager && args.Count == 1)
		{
			SetShovelManager(VariantUtils.ConvertTo<ShovelManager>(in args[0]));
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
		if (method == MethodName.SetShovelManager)
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
		if (name == PropertyName.shovelManager)
		{
			shovelManager = VariantUtils.ConvertTo<ShovelManager>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.shovelManager)
		{
			value = VariantUtils.CreateFrom(in shovelManager);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.shovelManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shovelManager, Variant.From(in shovelManager));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shovelManager, out var value))
		{
			shovelManager = value.As<ShovelManager>();
		}
	}
}
