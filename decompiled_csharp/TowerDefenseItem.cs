using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseItem.cs")]
public class TowerDefenseItem : TowerDefenseCharacter
{
	public new class MethodName : TowerDefenseCharacter.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName OnTargetZombieDestroyed = "OnTargetZombieDestroyed";

		public static readonly StringName OnTargetPlantDestroyed = "OnTargetPlantDestroyed";
	}

	public new class PropertyName : TowerDefenseCharacter.PropertyName
	{
		public static readonly StringName canCheck = "canCheck";

		public static readonly StringName canCheckTarget = "canCheckTarget";

		public static readonly StringName targetZombie = "targetZombie";

		public static readonly StringName targetPlant = "targetPlant";
	}

	public new class SignalName : TowerDefenseCharacter.SignalName
	{
	}

	public TowerDefenseCharacter targetZombie;

	public TowerDefenseCharacter targetPlant;

	[Export(PropertyHint.None, "")]
	public bool canCheck { get; set; }

	[Export(PropertyHint.None, "")]
	public bool canCheckTarget { get; set; }

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base._Ready();
		if (!editorPreviewMode)
		{
			instance.hitpointsEmpty += () =>
			{
				Destroy();
			};
			AddToGroup("Item", persistent: true);
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!TowerDefenseCharacter.CachedEditorHint)
		{
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (GodotObject.IsInstanceValid(targetZombie))
			{
				SetGlobalPositionForPhysicsFrame(targetZombie.GetGlobalPositionForPhysicsFrame(currentPhysicsFrame), currentPhysicsFrame);
			}
			if (GodotObject.IsInstanceValid(targetPlant))
			{
				SetGlobalPositionForPhysicsFrame(targetPlant.GetGlobalPositionForPhysicsFrame(currentPhysicsFrame), currentPhysicsFrame);
			}
		}
	}

	public void OnTargetZombieDestroyed()
	{
		targetZombie = null;
		Destroy();
	}

	public void OnTargetPlantDestroyed()
	{
		targetPlant = null;
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTargetZombieDestroyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTargetPlantDestroyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTargetZombieDestroyed && args.Count == 0)
		{
			OnTargetZombieDestroyed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTargetPlantDestroyed && args.Count == 0)
		{
			OnTargetPlantDestroyed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.OnTargetZombieDestroyed)
		{
			return true;
		}
		if (method == MethodName.OnTargetPlantDestroyed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.canCheck)
		{
			canCheck = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canCheckTarget)
		{
			canCheckTarget = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.targetZombie)
		{
			targetZombie = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.targetPlant)
		{
			targetPlant = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.canCheck)
		{
			from = canCheck;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.canCheckTarget)
		{
			from = canCheckTarget;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.targetZombie)
		{
			value = VariantUtils.CreateFrom(in targetZombie);
			return true;
		}
		if (name == PropertyName.targetPlant)
		{
			value = VariantUtils.CreateFrom(in targetPlant);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.canCheck, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canCheckTarget, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.targetZombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.targetPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.canCheck, Variant.From<bool>(canCheck));
		info.AddProperty(PropertyName.canCheckTarget, Variant.From<bool>(canCheckTarget));
		info.AddProperty(PropertyName.targetZombie, Variant.From(in targetZombie));
		info.AddProperty(PropertyName.targetPlant, Variant.From(in targetPlant));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.canCheck, out var value))
		{
			canCheck = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canCheckTarget, out var value2))
		{
			canCheckTarget = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.targetZombie, out var value3))
		{
			targetZombie = value3.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.targetPlant, out var value4))
		{
			targetPlant = value4.As<TowerDefenseCharacter>();
		}
	}
}
