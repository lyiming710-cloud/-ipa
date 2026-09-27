using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

public class BattleFeatureFullTimingProbeTutorialFeature : TowerDefenseBattleFeatureTutorial
{
	public new class MethodName : TowerDefenseBattleFeatureTutorial.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeatureTutorial.PropertyName
	{
		public static readonly StringName _id = "_id";
	}

	public new class SignalName : TowerDefenseBattleFeatureTutorial.SignalName
	{
	}

	private string _id = "Tutorial";

	public override void Init(Dictionary data)
	{
		base.Init(data);
		_id = data.GetValueOrDefault("Id", _id).AsString();
		config = new TutorialConfig
		{
			step = new Array<TutorialStepConfig>
			{
				new TutorialStepConfig
				{
					conditionList = new Array<TutorialConditionConfig>
					{
						new BattleFeatureFullTimingPendingTutorialCondition()
					}
				}
			}
		};
		BattleFeatureFullTimingProbeLog.Add("Init:" + _id);
	}

	public override void OnReady()
	{
		BattleFeatureFullTimingProbeLog.Add("OnReady:" + _id);
	}

	public override async Task GameStart()
	{
		BattleFeatureFullTimingProbeLog.Add("GameStart.begin:" + _id);
		await base.GameStart();
		if (IsLifetimeActive)
		{
			BattleFeatureFullTimingProbeLog.Add("GameStart.end:" + _id);
		}
	}

	public override async Task GameStartFromProgress()
	{
		BattleFeatureFullTimingProbeLog.Add("GameStartFromProgress.begin:" + _id);
		await base.GameStartFromProgress();
		if (IsLifetimeActive)
		{
			BattleFeatureFullTimingProbeLog.Add("GameStartFromProgress.end:" + _id);
		}
	}

	public override void Destroy()
	{
		BattleFeatureFullTimingProbeLog.Add("Destroy:" + _id);
		base.Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
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
		if (method == MethodName.OnReady)
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
		if (name == PropertyName._id)
		{
			_id = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._id)
		{
			value = VariantUtils.CreateFrom(in _id);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._id, Variant.From(in _id));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._id, out var value))
		{
			_id = value.As<string>();
		}
	}
}
