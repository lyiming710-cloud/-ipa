using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Buff/TowerDefenseCharacterBuffRuneMagic.cs")]
public class TowerDefenseCharacterBuffRuneMagic : TowerDefenseCharacterBuffConfig
{
	public new class MethodName : TowerDefenseCharacterBuffConfig.MethodName
	{
		public new static readonly StringName _Init = "_Init";

		public new static readonly StringName CreateRuntimeInstance = "CreateRuntimeInstance";

		public new static readonly StringName Step = "Step";

		public new static readonly StringName StepReadOnlyClient = "StepReadOnlyClient";

		public new static readonly StringName Refresh = "Refresh";
	}

	public new class PropertyName : TowerDefenseCharacterBuffConfig.PropertyName
	{
		public static readonly StringName timeScaleValue = "timeScaleValue";

		public static readonly StringName time = "time";

		public static readonly StringName currentTime = "currentTime";
	}

	public new class SignalName : TowerDefenseCharacterBuffConfig.SignalName
	{
	}

	public const string StormSlow = "RuneStormSlow";

	public const string FogHaste = "RuneFogHaste";

	public const string FogDizzyImmune = "RuneFogDizzyImmune";

	[Export(PropertyHint.None, "")]
	public double timeScaleValue = 1.0;

	[Export(PropertyHint.None, "")]
	public double time = 0.75;

	[Export(PropertyHint.None, "")]
	public double currentTime;

	public override void _Init()
	{
		if (key == null)
		{
			key = "RuneStormSlow";
		}
	}

	public override TowerDefenseCharacterBuffConfig CreateRuntimeInstance()
	{
		return new TowerDefenseCharacterBuffRuneMagic
		{
			key = key,
			refresh = refresh,
			canFliter = canFliter,
			timeScaleValue = timeScaleValue,
			time = time,
			currentTime = currentTime
		};
	}

	public override bool Step(double delta)
	{
		currentTime += delta;
		if (currentTime >= time)
		{
			return true;
		}
		character.timeScale *= timeScaleValue;
		return false;
	}

	public override void StepReadOnlyClient(double delta)
	{
		Step(delta);
	}

	public override void Refresh(TowerDefenseCharacterBuffConfig config)
	{
		TowerDefenseCharacterBuffRuneMagic towerDefenseCharacterBuffRuneMagic = (TowerDefenseCharacterBuffRuneMagic)config;
		time = Mathf.Max(time, currentTime + towerDefenseCharacterBuffRuneMagic.time);
		timeScaleValue = towerDefenseCharacterBuffRuneMagic.timeScaleValue;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateRuntimeInstance, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepReadOnlyClient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Init && args.Count == 0)
		{
			_Init();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateRuntimeInstance && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterBuffConfig>(CreateRuntimeInstance());
			return true;
		}
		if (method == MethodName.Step && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Step(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.StepReadOnlyClient && args.Count == 1)
		{
			StepReadOnlyClient(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 1)
		{
			Refresh(VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.CreateRuntimeInstance)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		if (method == MethodName.StepReadOnlyClient)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.timeScaleValue)
		{
			timeScaleValue = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.time)
		{
			time = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			currentTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.timeScaleValue)
		{
			value = VariantUtils.CreateFrom(in timeScaleValue);
			return true;
		}
		if (name == PropertyName.time)
		{
			value = VariantUtils.CreateFrom(in time);
			return true;
		}
		if (name == PropertyName.currentTime)
		{
			value = VariantUtils.CreateFrom(in currentTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScaleValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.time, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.currentTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.timeScaleValue, Variant.From(in timeScaleValue));
		info.AddProperty(PropertyName.time, Variant.From(in time));
		info.AddProperty(PropertyName.currentTime, Variant.From(in currentTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.timeScaleValue, out var value))
		{
			timeScaleValue = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.time, out var value2))
		{
			time = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.currentTime, out var value3))
		{
			currentTime = value3.As<double>();
		}
	}
}
