using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Sun/Brain/TowerDefenseBrainSun.cs")]
public class TowerDefenseBrainSun : TowerDefenseSunBase
{
	public new class MethodName : TowerDefenseSunBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName GetGroupName = "GetGroupName";

		public new static readonly StringName GetPoolKey = "GetPoolKey";

		public new static readonly StringName OnInitScaleTween = "OnInitScaleTween";
	}

	public new class PropertyName : TowerDefenseSunBase.PropertyName
	{
	}

	public new class SignalName : TowerDefenseSunBase.SignalName
	{
	}

	public override void _Ready()
	{
		base._Ready();
		dieDownTimer.Timeout += DieDown;
	}

	public override string GetGroupName()
	{
		return "BrainSun";
	}

	public override ObjectManagerConfig.OBJECT GetPoolKey()
	{
		return ObjectManagerConfig.OBJECT.SUN_BRAIN;
	}

	public override void OnInitScaleTween(Tween tween)
	{
		tween.Finished += Collection;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGroupName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPoolKey, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInitScaleTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tween", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false)
			}, null)
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
		if (method == MethodName.GetGroupName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetGroupName());
			return true;
		}
		if (method == MethodName.GetPoolKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(GetPoolKey());
			return true;
		}
		if (method == MethodName.OnInitScaleTween && args.Count == 1)
		{
			OnInitScaleTween(VariantUtils.ConvertTo<Tween>(in args[0]));
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
		if (method == MethodName.GetGroupName)
		{
			return true;
		}
		if (method == MethodName.GetPoolKey)
		{
			return true;
		}
		if (method == MethodName.OnInitScaleTween)
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
