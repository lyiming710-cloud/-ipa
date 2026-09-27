using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

public class BattleFeatureFullTimingProbeFeature : TowerDefenseBattleFeature
{
	public new class MethodName : TowerDefenseBattleFeature.MethodName
	{
		public new static readonly StringName Init = "Init";

		public new static readonly StringName OnReady = "OnReady";

		public new static readonly StringName CanLoadProgress = "CanLoadProgress";

		public new static readonly StringName Process = "Process";

		public new static readonly StringName GameFail = "GameFail";

		public new static readonly StringName ZombieEnterHouse = "ZombieEnterHouse";

		public new static readonly StringName Destroy = "Destroy";
	}

	public new class PropertyName : TowerDefenseBattleFeature.PropertyName
	{
		public static readonly StringName _id = "_id";

		public static readonly StringName _canLoadProgress = "_canLoadProgress";
	}

	public new class SignalName : TowerDefenseBattleFeature.SignalName
	{
	}

	private string _id = "Feature";

	private bool _canLoadProgress = true;

	public override void Init(Dictionary data)
	{
		base.Init(data);
		_id = data.GetValueOrDefault("Id", _id).AsString();
		_canLoadProgress = data.GetValueOrDefault("CanLoadProgress", true).AsBool();
		BattleFeatureFullTimingProbeLog.Add("Init:" + _id);
	}

	public override void OnReady()
	{
		BattleFeatureFullTimingProbeLog.Add("OnReady:" + _id);
	}

	public override bool CanLoadProgress()
	{
		return _canLoadProgress;
	}

	public override Task GameInit()
	{
		return RecordAsync("GameInit");
	}

	public override Task GameInitFromProgress()
	{
		return RecordAsync("GameInitFromProgress");
	}

	public override Task GameEntry()
	{
		return RecordAsync("GameEntry");
	}

	public override Task GameReady()
	{
		return RecordAsync("GameReady");
	}

	public override Task GameStart()
	{
		return RecordAsync("GameStart");
	}

	public override Task GameStartFromProgress()
	{
		return RecordAsync("GameStartFromProgress");
	}

	public override void Process(double delta)
	{
		BattleFeatureFullTimingProbeLog.Add("Process:" + _id);
	}

	public override void GameFail()
	{
		BattleFeatureFullTimingProbeLog.Add("GameFail:" + _id);
	}

	public override void ZombieEnterHouse(TowerDefenseCharacter character)
	{
		BattleFeatureFullTimingProbeLog.Add("ZombieEnterHouse:" + _id);
	}

	public override void Destroy()
	{
		BattleFeatureFullTimingProbeLog.Add("Destroy:" + _id);
		base.Destroy();
	}

	private async Task RecordAsync(string phase)
	{
		BattleFeatureFullTimingProbeLog.Add(phase + ".begin:" + _id);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		if (IsLifetimeActive)
		{
			BattleFeatureFullTimingProbeLog.Add(phase + ".end:" + _id);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanLoadProgress, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.OnReady && args.Count == 0)
		{
			OnReady();
			ret = default;
			return true;
		}
		if (method == MethodName.CanLoadProgress && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanLoadProgress());
			return true;
		}
		if (method == MethodName.Process && args.Count == 1)
		{
			Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 0)
		{
			GameFail();
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieEnterHouse && args.Count == 1)
		{
			ZombieEnterHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.CanLoadProgress)
		{
			return true;
		}
		if (method == MethodName.Process)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.ZombieEnterHouse)
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
		if (name == PropertyName._canLoadProgress)
		{
			_canLoadProgress = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._canLoadProgress)
		{
			value = VariantUtils.CreateFrom(in _canLoadProgress);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._id, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._canLoadProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._id, Variant.From(in _id));
		info.AddProperty(PropertyName._canLoadProgress, Variant.From(in _canLoadProgress));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._id, out var value))
		{
			_id = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._canLoadProgress, out var value2))
		{
			_canLoadProgress = value2.As<bool>();
		}
	}
}
