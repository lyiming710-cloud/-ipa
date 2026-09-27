using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Camera/TowerDefenseBattleFeatureCamera.cs")]
public class TowerDefenseBattleFeatureCamera : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName cameraControl = "cameraControl";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private static PackedScene _towerDefenseCameraControlScene;

	public TowerDefenseCameraControl cameraControl;

	private static PackedScene TowerDefenseCameraControlScene => _towerDefenseCameraControlScene ?? (_towerDefenseCameraControlScene = GD.Load<PackedScene>("uid://7i6bh10be5o0"));

	public override void Init(Dictionary data)
	{
		base.Init(data);
		if (!GodotObject.IsInstanceValid(TowerDefenseCameraControlScene) || !GodotObject.IsInstanceValid(control))
		{
			GD.PushError("[Camera] Cannot create Camera control without its scene and battle control.");
			return;
		}
		cameraControl = TowerDefenseCameraControlScene.Instantiate<TowerDefenseCameraControl>(PackedScene.GenEditState.Disabled);
		control.AddNode(cameraControl);
	}

	public override Task GameInit()
	{
		TowerDefenseBattleFeatureMap feature = GetFeature<TowerDefenseBattleFeatureMap>("Map");
		if (GodotObject.IsInstanceValid(cameraControl) && GodotObject.IsInstanceValid(feature) && GodotObject.IsInstanceValid(feature.mapConfig))
		{
			cameraControl.ApplyMapConfig(feature.mapConfig);
		}
		return Task.CompletedTask;
	}

	public override Task GameInitFromProgress()
	{
		return GameInit();
	}

	public override void Destroy()
	{
		if (GodotObject.IsInstanceValid(cameraControl))
		{
			cameraControl.QueueFree();
		}
		cameraControl = null;
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 0)
		{
			Destroy();
			ret = default;
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
		if (method == MethodName.Destroy)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.cameraControl)
		{
			cameraControl = VariantUtils.ConvertTo<TowerDefenseCameraControl>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.cameraControl)
		{
			value = VariantUtils.CreateFrom(in cameraControl);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.cameraControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.cameraControl, Variant.From(in cameraControl));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.cameraControl, out var value))
		{
			cameraControl = value.As<TowerDefenseCameraControl>();
		}
	}
}
