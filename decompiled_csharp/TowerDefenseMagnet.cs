using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Magnet/TowerDefenseMagnet.cs")]
public class TowerDefenseMagnet : TowerDefenseGroundItemBase
{
	public new class MethodName : TowerDefenseGroundItemBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName Init = "Init";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";
	}

	public new class PropertyName : TowerDefenseGroundItemBase.PropertyName
	{
		public static readonly StringName armorInstance = "armorInstance";

		public static readonly StringName adsorbedObject = "adsorbedObject";
	}

	public new class SignalName : TowerDefenseGroundItemBase.SignalName
	{
	}

	private static PackedScene _towerDefenseMagnetScene;

	public TowerDefenseArmorInstance armorInstance;

	public Node adsorbedObject;

	private static PackedScene TowerDefenseMagnetScene => _towerDefenseMagnetScene ?? (_towerDefenseMagnetScene = GD.Load<PackedScene>("uid://baxlrxaix6r2b"));

	public static TowerDefenseMagnet Create(TowerDefenseArmorInstance _armorInstance)
	{
		TowerDefenseMagnet towerDefenseMagnet = TowerDefenseMagnetScene.Instantiate<TowerDefenseMagnet>(PackedScene.GenEditState.Disabled);
		towerDefenseMagnet.armorInstance = _armorInstance;
		return towerDefenseMagnet;
	}

	public void Init(Node2D node)
	{
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!GodotObject.IsInstanceValid(adsorbedObject))
		{
			QueueFree();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_armorInstance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMagnet>(Create(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Node2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMagnet>(Create(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.armorInstance)
		{
			armorInstance = VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in value);
			return true;
		}
		if (name == PropertyName.adsorbedObject)
		{
			adsorbedObject = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.armorInstance)
		{
			value = VariantUtils.CreateFrom(in armorInstance);
			return true;
		}
		if (name == PropertyName.adsorbedObject)
		{
			value = VariantUtils.CreateFrom(in adsorbedObject);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.armorInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.adsorbedObject, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.armorInstance, Variant.From(in armorInstance));
		info.AddProperty(PropertyName.adsorbedObject, Variant.From(in adsorbedObject));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.armorInstance, out var value))
		{
			armorInstance = value.As<TowerDefenseArmorInstance>();
		}
		if (info.TryGetProperty(PropertyName.adsorbedObject, out var value2))
		{
			adsorbedObject = value2.As<Node>();
		}
	}
}
