using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/TutorialManager/TutorialManager.cs")]
public class TutorialManager : Node
{
	public delegate void TutorialFinishEventHandler();

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName ProcessBroadCastAsync = "ProcessBroadCastAsync";

		public static readonly StringName StartTutorial = "StartTutorial";

		public static readonly StringName TutorialExecute = "TutorialExecute";

		public static readonly StringName TutorialClear = "TutorialClear";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName currentTutoroal = "currentTutoroal";

		public static readonly StringName currentStep = "currentStep";

		public static readonly StringName stepExe = "stepExe";

		public static readonly StringName _isProcessing = "_isProcessing";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public TutorialConfig currentTutoroal;

	public int currentStep;

	public bool stepExe;

	private bool _isProcessing;

	public static TutorialManager Instance { get; private set; }

	public event TutorialFinishEventHandler OnTutorialFinish;

	public async Task WaitForFinish()
	{
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		OnTutorialFinish += Handler;
		try
		{
			await tcs.Task;
		}
		finally
		{
			OnTutorialFinish -= Handler;
		}
		void Handler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public override void _Ready()
	{
		Instance = this;
		SceneManager.Instance.OnSceneChange += (string _sceneName) =>
		{
			TutorialClear();
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!stepExe)
		{
			return;
		}
		TutorialStepConfig tutorialStep = currentTutoroal.GetTutorialStep(currentStep);
		if (!tutorialStep.Step())
		{
			return;
		}
		stepExe = false;
		tutorialStep.Exit();
		if (tutorialStep.broadCastUse)
		{
			if (tutorialStep.broadCastConfig.broadCastTime == -1.0)
			{
				BroadCastManager.Instance.Next();
				currentStep++;
				TutorialExecute();
			}
			else if (!_isProcessing)
			{
				_isProcessing = true;
				ProcessBroadCastAsync();
			}
		}
		else
		{
			currentStep++;
			TutorialExecute();
		}
	}

	private async void ProcessBroadCastAsync()
	{
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
		BroadCastManager.Instance.OnBroadCastOver += OnBroadCastOverHandler;
		try
		{
			await tcs.Task;
			if (GodotObject.IsInstanceValid(this))
			{
				currentStep++;
				TutorialExecute();
			}
		}
		finally
		{
			BroadCastManager.Instance.OnBroadCastOver -= OnBroadCastOverHandler;
			_isProcessing = false;
		}
		void OnBroadCastOverHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public void StartTutorial(TutorialConfig tutorial)
	{
		currentTutoroal = tutorial;
		currentStep = 0;
		TutorialExecute();
	}

	public void TutorialExecute()
	{
		if (!GodotObject.IsInstanceValid(currentTutoroal))
		{
			return;
		}
		if (currentStep >= currentTutoroal.GetStepNum())
		{
			currentTutoroal = null;
			BroadCastManager.Instance.BraodCastClear();
			OnTutorialFinish?.Invoke();
			return;
		}
		TutorialStepConfig tutorialStep = currentTutoroal.GetTutorialStep(currentStep);
		if (tutorialStep.broadCastUse)
		{
			BroadCastManager.Instance.BroadCastAdd(tutorialStep.broadCastConfig);
		}
		tutorialStep.Enter();
		stepExe = true;
	}

	public void TutorialClear()
	{
		currentTutoroal = null;
		currentStep = 0;
		stepExe = false;
		BroadCastManager.Instance.BraodCastClear();
	}

	public TutorialManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/TutorialManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessBroadCastAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartTutorial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tutorial", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TutorialExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TutorialClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessBroadCastAsync && args.Count == 0)
		{
			ProcessBroadCastAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.StartTutorial && args.Count == 1)
		{
			StartTutorial(VariantUtils.ConvertTo<TutorialConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TutorialExecute && args.Count == 0)
		{
			TutorialExecute();
			ret = default;
			return true;
		}
		if (method == MethodName.TutorialClear && args.Count == 0)
		{
			TutorialClear();
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.ProcessBroadCastAsync)
		{
			return true;
		}
		if (method == MethodName.StartTutorial)
		{
			return true;
		}
		if (method == MethodName.TutorialExecute)
		{
			return true;
		}
		if (method == MethodName.TutorialClear)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.currentTutoroal)
		{
			currentTutoroal = VariantUtils.ConvertTo<TutorialConfig>(in value);
			return true;
		}
		if (name == PropertyName.currentStep)
		{
			currentStep = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.stepExe)
		{
			stepExe = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isProcessing)
		{
			_isProcessing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.currentTutoroal)
		{
			value = VariantUtils.CreateFrom(in currentTutoroal);
			return true;
		}
		if (name == PropertyName.currentStep)
		{
			value = VariantUtils.CreateFrom(in currentStep);
			return true;
		}
		if (name == PropertyName.stepExe)
		{
			value = VariantUtils.CreateFrom(in stepExe);
			return true;
		}
		if (name == PropertyName._isProcessing)
		{
			value = VariantUtils.CreateFrom(in _isProcessing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.currentTutoroal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentStep, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.stepExe, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isProcessing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.currentTutoroal, Variant.From(in currentTutoroal));
		info.AddProperty(PropertyName.currentStep, Variant.From(in currentStep));
		info.AddProperty(PropertyName.stepExe, Variant.From(in stepExe));
		info.AddProperty(PropertyName._isProcessing, Variant.From(in _isProcessing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.currentTutoroal, out var value))
		{
			currentTutoroal = value.As<TutorialConfig>();
		}
		if (info.TryGetProperty(PropertyName.currentStep, out var value2))
		{
			currentStep = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.stepExe, out var value3))
		{
			stepExe = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isProcessing, out var value4))
		{
			_isProcessing = value4.As<bool>();
		}
	}
}
