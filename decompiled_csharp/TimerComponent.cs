using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Script/Component/TimerComponent/TimerComponent.cs")]
public class TimerComponent : ComponentBase
{
	public delegate void TimeoutEventHandler(string timerName);

	public new class MethodName : ComponentBase.MethodName
	{
		public new static readonly StringName _GetName = "_GetName";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RebuildActiveTimers = "RebuildActiveTimers";

		public static readonly StringName ResetRuntimeTimersFromConfig = "ResetRuntimeTimersFromConfig";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName SharedBatchPhysicsProcess = "SharedBatchPhysicsProcess";

		public static readonly StringName PhysicsProcessValidated = "PhysicsProcessValidated";

		public static readonly StringName RemoveActiveTimerAt = "RemoveActiveTimerAt";

		public static readonly StringName RemoveActiveTimer = "RemoveActiveTimer";

		public static readonly StringName EmitTimeout = "EmitTimeout";

		public static readonly StringName IsTimerRunning = "IsTimerRunning";

		public static readonly StringName AddTimer = "AddTimer";

		public static readonly StringName Run = "Run";

		public static readonly StringName Stop = "Stop";

		public static readonly StringName IsRunning = "IsRunning";

		public new static readonly StringName ExportComponentSave = "ExportComponentSave";

		public new static readonly StringName ImportComponentSave = "ImportComponentSave";

		public new static readonly StringName SyncSerialize = "SyncSerialize";

		public new static readonly StringName SyncDeserialize = "SyncDeserialize";
	}

	public new class PropertyName : ComponentBase.PropertyName
	{
		public new static readonly StringName UseSharedPhysicsBatch = "UseSharedPhysicsBatch";

		public static readonly StringName timerDictionary = "timerDictionary";

		public static readonly StringName timeScale = "timeScale";

		public static readonly StringName parent = "parent";
	}

	public new class SignalName : ComponentBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Dictionary timerDictionary = new Dictionary();

	[Export(PropertyHint.None, "")]
	public double timeScale = 1.0;

	public System.Collections.Generic.Dictionary<string, bool> timerRunning = new System.Collections.Generic.Dictionary<string, bool>();

	public System.Collections.Generic.Dictionary<string, double> timerWaitTime = new System.Collections.Generic.Dictionary<string, double>();

	public System.Collections.Generic.Dictionary<string, double> timerCurrent = new System.Collections.Generic.Dictionary<string, double>();

	private readonly List<string> _activeTimers = new List<string>();

	public Node parent;

	protected override bool UseSharedPhysicsBatch => true;

	public event TimeoutEventHandler OnTimeout;

	public override string _GetName()
	{
		return "TimerComponent";
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		parent = GetParent();
		RebuildActiveTimers();
	}

	public override void _Ready()
	{
		parent = GetParent();
		ResetRuntimeTimersFromConfig();
	}

	public override void _ExitTree()
	{
		_activeTimers.Clear();
		OnTimeout = null;
		parent = null;
	}

	private void RebuildActiveTimers()
	{
		_activeTimers.Clear();
		foreach (KeyValuePair<string, bool> item in timerRunning)
		{
			if (item.Value && timerCurrent.ContainsKey(item.Key) && timerWaitTime.ContainsKey(item.Key))
			{
				_activeTimers.Add(item.Key);
			}
		}
	}

	private void ResetRuntimeTimersFromConfig()
	{
		timerRunning.Clear();
		timerWaitTime.Clear();
		timerCurrent.Clear();
		_activeTimers.Clear();
		foreach (Variant key2 in timerDictionary.Keys)
		{
			string key = key2.AsString();
			timerRunning[key] = false;
			timerWaitTime[key] = timerDictionary[key2].AsDouble();
			timerCurrent[key] = 0.0;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (alive)
		{
			PhysicsProcessValidated(delta);
		}
	}

	internal override void SharedBatchPhysicsProcess(double delta)
	{
		PhysicsProcessValidated(delta);
	}

	private void PhysicsProcessValidated(double delta)
	{
		if (!TowerDefenseManager._IsGameRunning() || _activeTimers.Count == 0)
		{
			return;
		}
		int num = 0;
		while (num < _activeTimers.Count)
		{
			string text = _activeTimers[num];
			if (!timerRunning.TryGetValue(text, out var value) || !value || !timerCurrent.ContainsKey(text) || !timerWaitTime.ContainsKey(text))
			{
				RemoveActiveTimerAt(num);
				continue;
			}
			if (timerCurrent[text] < timerWaitTime[text])
			{
				timerCurrent[text] += delta * timeScale;
				num++;
				continue;
			}
			timerCurrent[text] = 0.0;
			timerRunning[text] = false;
			RemoveActiveTimerAt(num);
			CallDeferred(MethodName.EmitTimeout, text);
		}
	}

	private void RemoveActiveTimerAt(int index)
	{
		int index2 = _activeTimers.Count - 1;
		_activeTimers[index] = _activeTimers[index2];
		_activeTimers.RemoveAt(index2);
	}

	private void RemoveActiveTimer(string timerName)
	{
		int num = _activeTimers.IndexOf(timerName);
		if (num >= 0)
		{
			RemoveActiveTimerAt(num);
		}
	}

	private void EmitTimeout(string key)
	{
		OnTimeout?.Invoke(key);
	}

	private bool IsTimerRunning(string timerName)
	{
		bool value;
		return timerRunning.TryGetValue(timerName, out value) & value;
	}

	public void AddTimer(string timerName, Variant time = default(Variant))
	{
		if (!string.IsNullOrEmpty(timerName))
		{
			double num = ((time.VariantType == Variant.Type.Nil) ? 1.0 : time.AsDouble());
			RemoveActiveTimer(timerName);
			timerDictionary[timerName] = num;
			timerRunning[timerName] = false;
			timerWaitTime[timerName] = num;
			timerCurrent[timerName] = 0.0;
		}
	}

	public void Run(string timerName, double time = -1.0)
	{
		if (timerDictionary.ContainsKey(timerName))
		{
			if (!IsTimerRunning(timerName))
			{
				_activeTimers.Add(timerName);
			}
			timerWaitTime[timerName] = ((time == -1.0) ? timerDictionary[timerName].AsDouble() : time);
			timerCurrent[timerName] = 0.0;
			timerRunning[timerName] = true;
		}
	}

	public void Stop(string timerName)
	{
		if (timerDictionary.ContainsKey(timerName))
		{
			RemoveActiveTimer(timerName);
			timerCurrent[timerName] = 0.0;
			timerRunning[timerName] = false;
		}
	}

	public bool IsRunning(string timerName)
	{
		return IsTimerRunning(timerName);
	}

	public override Dictionary ExportComponentSave()
	{
		Dictionary dictionary = new Dictionary();
		Dictionary dictionary2 = new Dictionary();
		Dictionary dictionary3 = new Dictionary();
		foreach (string key5 in timerRunning.Keys)
		{
			dictionary[key5] = timerRunning[key5];
			dictionary2[key5] = (timerWaitTime.TryGetValue(key5, out var value) ? value : 0.0);
			dictionary3[key5] = (timerCurrent.TryGetValue(key5, out var value2) ? value2 : 0.0);
		}
		return new Dictionary
		{
			["timerRunning"] = dictionary,
			["timerWaitTime"] = dictionary2,
			["timerCurrent"] = dictionary3,
			["timeScale"] = timeScale
		};
	}

	public override void ImportComponentSave(Dictionary data, TowerDefenseLevelSaveConfigCSharp owner)
	{
		ResetRuntimeTimersFromConfig();
		Dictionary dictionary = data.GetValueOrDefault("timerRunning", new Dictionary()).AsGodotDictionary();
		Dictionary dictionary2 = data.GetValueOrDefault("timerWaitTime", new Dictionary()).AsGodotDictionary();
		Dictionary dictionary3 = data.GetValueOrDefault("timerCurrent", new Dictionary()).AsGodotDictionary();
		timeScale = data.GetValueOrDefault("timeScale", 1.0).AsDouble();
		foreach (Variant key in dictionary.Keys)
		{
			string text = key.AsString();
			double num = dictionary2.GetValueOrDefault(key, 0.0).AsDouble();
			if (!timerDictionary.ContainsKey(text))
			{
				timerDictionary[text] = num;
			}
			timerRunning[text] = dictionary[key].AsBool();
			timerWaitTime[text] = num;
			timerCurrent[text] = dictionary3.GetValueOrDefault(key, 0.0).AsDouble();
		}
		_activeTimers.Clear();
		foreach (KeyValuePair<string, bool> item in timerRunning)
		{
			if (item.Value)
			{
				_activeTimers.Add(item.Key);
			}
		}
	}

	public override Dictionary SyncSerialize()
	{
		return ExportComponentSave();
	}

	public override void SyncDeserialize(Dictionary data)
	{
		ImportComponentSave(data, null);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._GetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildActiveTimers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetRuntimeTimersFromConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SharedBatchPhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PhysicsProcessValidated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveActiveTimerAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveActiveTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsTimerRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "time", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Stop, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "timerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportComponentSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportComponentSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncSerialize, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncDeserialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._GetName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetName());
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildActiveTimers && args.Count == 0)
		{
			RebuildActiveTimers();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetRuntimeTimersFromConfig && args.Count == 0)
		{
			ResetRuntimeTimersFromConfig();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SharedBatchPhysicsProcess && args.Count == 1)
		{
			SharedBatchPhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PhysicsProcessValidated && args.Count == 1)
		{
			PhysicsProcessValidated(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveActiveTimerAt && args.Count == 1)
		{
			RemoveActiveTimerAt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveActiveTimer && args.Count == 1)
		{
			RemoveActiveTimer(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmitTimeout && args.Count == 1)
		{
			EmitTimeout(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsTimerRunning && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTimerRunning(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddTimer && args.Count == 2)
		{
			AddTimer(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 2)
		{
			Run(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Stop && args.Count == 1)
		{
			Stop(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRunning && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRunning(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ExportComponentSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportComponentSave());
			return true;
		}
		if (method == MethodName.ImportComponentSave && args.Count == 2)
		{
			ImportComponentSave(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSerialize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(SyncSerialize());
			return true;
		}
		if (method == MethodName.SyncDeserialize && args.Count == 1)
		{
			SyncDeserialize(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._GetName)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.RebuildActiveTimers)
		{
			return true;
		}
		if (method == MethodName.ResetRuntimeTimersFromConfig)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SharedBatchPhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.PhysicsProcessValidated)
		{
			return true;
		}
		if (method == MethodName.RemoveActiveTimerAt)
		{
			return true;
		}
		if (method == MethodName.RemoveActiveTimer)
		{
			return true;
		}
		if (method == MethodName.EmitTimeout)
		{
			return true;
		}
		if (method == MethodName.IsTimerRunning)
		{
			return true;
		}
		if (method == MethodName.AddTimer)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.Stop)
		{
			return true;
		}
		if (method == MethodName.IsRunning)
		{
			return true;
		}
		if (method == MethodName.ExportComponentSave)
		{
			return true;
		}
		if (method == MethodName.ImportComponentSave)
		{
			return true;
		}
		if (method == MethodName.SyncSerialize)
		{
			return true;
		}
		if (method == MethodName.SyncDeserialize)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.timerDictionary)
		{
			timerDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.parent)
		{
			parent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.UseSharedPhysicsBatch)
		{
			value = VariantUtils.CreateFrom<bool>(UseSharedPhysicsBatch);
			return true;
		}
		if (name == PropertyName.timerDictionary)
		{
			value = VariantUtils.CreateFrom(in timerDictionary);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			value = VariantUtils.CreateFrom(in timeScale);
			return true;
		}
		if (name == PropertyName.parent)
		{
			value = VariantUtils.CreateFrom(in parent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseSharedPhysicsBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.timerDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.parent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.timerDictionary, Variant.From(in timerDictionary));
		info.AddProperty(PropertyName.timeScale, Variant.From(in timeScale));
		info.AddProperty(PropertyName.parent, Variant.From(in parent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.timerDictionary, out var value))
		{
			timerDictionary = value.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.timeScale, out var value2))
		{
			timeScale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.parent, out var value3))
		{
			parent = value3.As<Node>();
		}
	}
}
