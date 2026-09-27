using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/ProgressMeter/GeneralProgressMeter.cs")]
public class GeneralProgressMeter : Control
{
	public new class MethodName : Control.MethodName
	{
		public static readonly StringName Init = "Init";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName SetAutoProgressResponse = "SetAutoProgressResponse";

		public static readonly StringName SetWaveCurrent = "SetWaveCurrent";

		public static readonly StringName SetHideItem = "SetHideItem";

		public static readonly StringName SetManualProgressEnabled = "SetManualProgressEnabled";

		public static readonly StringName SetManualProgress = "SetManualProgress";

		public static readonly StringName SetProgressValue = "SetProgressValue";

		public static readonly StringName SetProgressMaxValue = "SetProgressMaxValue";

		public static readonly StringName SetProgressText = "SetProgressText";

		public static readonly StringName SetProgressTextVisible = "SetProgressTextVisible";

		public static readonly StringName FlagRefresh = "FlagRefresh";

		public static readonly StringName RefreshFlagReachState = "RefreshFlagReachState";

		public static readonly StringName _ClearFlags = "_ClearFlags";

		public static readonly StringName RefreshPhysicsProcessing = "RefreshPhysicsProcessing";

		public static readonly StringName RefreshProgressPresentation = "RefreshProgressPresentation";

		public static readonly StringName RefreshProgressText = "RefreshProgressText";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnProgressValueChanged = "OnProgressValueChanged";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName ManualProgressText = "ManualProgressText";

		public static readonly StringName ManualProgressTextTemplate = "ManualProgressTextTemplate";

		public static readonly StringName waveNum = "waveNum";

		public static readonly StringName waveInterval = "waveInterval";

		public static readonly StringName previewWave = "previewWave";

		public static readonly StringName _progressBar = "_progressBar";

		public static readonly StringName _headTexture = "_headTexture";

		public static readonly StringName _flagContainer = "_flagContainer";

		public static readonly StringName _progressTextLabel = "_progressTextLabel";

		public static readonly StringName _progressTextTemplate = "_progressTextTemplate";

		public static readonly StringName _hasInitConfig = "_hasInitConfig";

		public static readonly StringName _nextAutoTextRefreshMsec = "_nextAutoTextRefreshMsec";

		public static readonly StringName _lastTextValueUnits = "_lastTextValueUnits";

		public static readonly StringName _lastTextMaxUnits = "_lastTextMaxUnits";

		public static readonly StringName _lastTextPercentUnits = "_lastTextPercentUnits";

		public static readonly StringName _lastRenderedTemplate = "_lastRenderedTemplate";

		public static readonly StringName _autoProgressResponse = "_autoProgressResponse";

		public static readonly StringName _renderedFlagWaveNum = "_renderedFlagWaveNum";

		public static readonly StringName _renderedFlagWaveInterval = "_renderedFlagWaveInterval";

		public static readonly StringName hideItem = "hideItem";

		public static readonly StringName manualProgressValue = "manualProgressValue";

		public static readonly StringName showProgressText = "showProgressText";

		public static readonly StringName _waveNum = "_waveNum";

		public static readonly StringName _waveInterval = "_waveInterval";

		public static readonly StringName _previewWave = "_previewWave";

		public static readonly StringName flagList = "flagList";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const ulong AutoTextRefreshIntervalMsec = 100uL;

	private static PackedScene _generalProgressFlag;

	private TextureProgressBar _progressBar;

	private TextureRect _headTexture;

	private HBoxContainer _flagContainer;

	private Label _progressTextLabel;

	private string _progressTextTemplate = "";

	private bool _hasInitConfig;

	private ulong _nextAutoTextRefreshMsec;

	private long _lastTextValueUnits = -9223372036854775808L;

	private long _lastTextMaxUnits = -9223372036854775808L;

	private long _lastTextPercentUnits = -9223372036854775808L;

	private string _lastRenderedTemplate;

	private double _autoProgressResponse = 0.1;

	private int _renderedFlagWaveNum = -1;

	private int _renderedFlagWaveInterval = -1;

	[Export(PropertyHint.None, "")]
	public bool hideItem;

	[Export(PropertyHint.None, "")]
	public bool manualProgressValue;

	[Export(PropertyHint.None, "")]
	public bool showProgressText;

	private int _waveNum = 1;

	private int _waveInterval = 1;

	private int _previewWave;

	public Array<GeneralProgressFlag> flagList = new Array<GeneralProgressFlag>();

	private static PackedScene GENERAL_PROGRESS_FLAG => _generalProgressFlag ?? (_generalProgressFlag = GD.Load<PackedScene>("res://Prefab/GUI/ProgressMeter/ProgressFlag/GeneralProgressFlag.tscn"));

	public string ManualProgressText => _progressTextLabel?.Text ?? "";

	public string ManualProgressTextTemplate => _progressTextTemplate;

	[Export(PropertyHint.None, "")]
	public int waveNum
	{
		get
		{
			return _waveNum;
		}
		set
		{
			_waveNum = Mathf.Max(1, value);
			_previewWave = Mathf.Clamp(_previewWave, 0, _waveNum);
			if (Engine.IsEditorHint())
			{
				FlagRefresh();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int waveInterval
	{
		get
		{
			return _waveInterval;
		}
		set
		{
			_waveInterval = Mathf.Max(1, value);
			if (Engine.IsEditorHint())
			{
				FlagRefresh();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int previewWave
	{
		get
		{
			return _previewWave;
		}
		set
		{
			_previewWave = value;
			_previewWave = Mathf.Clamp(_previewWave, 0, _waveNum);
			if (!hideItem && flagList.Count != 0)
			{
				RefreshFlagReachState();
			}
		}
	}

	public void Init(int newWaveNum, int newWaveInterval)
	{
		_hasInitConfig = true;
		waveNum = newWaveNum;
		waveInterval = newWaveInterval;
		FlagRefresh();
		if (hideItem)
		{
			SetHideItem(hide: true);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Engine.IsEditorHint() && !manualProgressValue && IsVisibleInTree() && waveNum > 0 && !(_autoProgressResponse <= 0.0))
		{
			double num = _progressBar.MaxValue * (double)previewWave / (double)waveNum;
			float weight = 1f - Mathf.Exp((0f - (float)_autoProgressResponse) * (float)delta);
			_progressBar.Value = Mathf.Lerp((float)_progressBar.Value, (float)num, weight);
		}
	}

	public void SetAutoProgressResponse(double response)
	{
		_autoProgressResponse = Mathf.Max(0.0, response);
		RefreshPhysicsProcessing();
	}

	public void SetWaveCurrent(int waveCurrent)
	{
		previewWave = waveCurrent;
	}

	public void SetHideItem(bool hide)
	{
		if (hideItem == hide)
		{
			RefreshProgressPresentation();
			if (!hide)
			{
				FlagRefresh();
			}
			return;
		}
		hideItem = hide;
		RefreshProgressPresentation();
		if (hide)
		{
			_ClearFlags();
		}
		else
		{
			FlagRefresh();
		}
	}

	public void SetManualProgressEnabled(bool enabled)
	{
		bool flag = manualProgressValue;
		manualProgressValue = enabled;
		if (enabled)
		{
			showProgressText = true;
		}
		else if (flag)
		{
			showProgressText = false;
		}
		RefreshProgressPresentation(forceTextRefresh: true);
		RefreshPhysicsProcessing();
	}

	public void SetManualProgress(double value, double maxValue, string text)
	{
		_progressTextTemplate = text ?? "";
		SetManualProgressEnabled(enabled: true);
		double num = ((maxValue > 0.0) ? maxValue : 1.0);
		_progressBar.MaxValue = num;
		_progressBar.Value = Mathf.Clamp(value, 0.0, num);
		RefreshProgressPresentation(forceTextRefresh: true);
	}

	public void SetProgressValue(double value)
	{
		_progressBar.Value = Mathf.Clamp(value, _progressBar.MinValue, _progressBar.MaxValue);
	}

	public void SetProgressMaxValue(double maxValue)
	{
		_progressBar.MaxValue = ((maxValue > 0.0) ? maxValue : 1.0);
		RefreshProgressPresentation(forceTextRefresh: true);
	}

	public void SetProgressText(string text)
	{
		_progressTextTemplate = text ?? "";
		RefreshProgressPresentation(forceTextRefresh: true);
	}

	public void SetProgressTextVisible(bool visible)
	{
		showProgressText = visible;
		RefreshProgressPresentation(forceTextRefresh: true);
	}

	public void FlagRefresh()
	{
		if (hideItem || _flagContainer == null)
		{
			return;
		}
		int num = Mathf.FloorToInt(waveNum / waveInterval);
		if (num <= 0)
		{
			_ClearFlags();
			return;
		}
		if (_renderedFlagWaveNum == waveNum && _renderedFlagWaveInterval == waveInterval && flagList.Count == num)
		{
			RefreshFlagReachState();
			return;
		}
		_ClearFlags();
		_renderedFlagWaveNum = waveNum;
		_renderedFlagWaveInterval = waveInterval;
		_flagContainer.AddThemeConstantOverride("separation", Mathf.Max(0, Mathf.FloorToInt(150f / (float)num)));
		for (int i = 0; i < num; i++)
		{
			GeneralProgressFlag generalProgressFlag = GENERAL_PROGRESS_FLAG.Instantiate<GeneralProgressFlag>(PackedScene.GenEditState.Disabled);
			_flagContainer.AddChild(generalProgressFlag, forceReadableName: false, InternalMode.Disabled);
			flagList.Insert(0, generalProgressFlag);
		}
		RefreshFlagReachState();
	}

	private void RefreshFlagReachState()
	{
		int num = Mathf.Min(previewWave / waveInterval, flagList.Count);
		for (int i = 0; i < flagList.Count; i++)
		{
			flagList[i].SetReach(i < num);
		}
	}

	private void _ClearFlags()
	{
		if (_flagContainer == null)
		{
			return;
		}
		foreach (GeneralProgressFlag flag in flagList)
		{
			if (GodotObject.IsInstanceValid(flag) && !flag.IsQueuedForDeletion())
			{
				if (flag.GetParent() == _flagContainer)
				{
					_flagContainer.RemoveChild(flag);
				}
				flag.QueueFree();
			}
		}
		flagList.Clear();
		_renderedFlagWaveNum = -1;
		_renderedFlagWaveInterval = -1;
	}

	private void RefreshPhysicsProcessing()
	{
		SetPhysicsProcess(!Engine.IsEditorHint() && IsVisibleInTree() && !manualProgressValue && _autoProgressResponse > 0.0);
	}

	private void RefreshProgressPresentation(bool forceTextRefresh = false)
	{
		if (_headTexture != null)
		{
			_headTexture.Visible = !hideItem && !manualProgressValue;
			if (_progressBar != null && _progressBar.MaxValue > 0.0)
			{
				_headTexture.Position = new Vector2((float)(140.0 - 150.0 * _progressBar.Value / _progressBar.MaxValue), _headTexture.Position.Y);
			}
		}
		RefreshProgressText(forceTextRefresh);
	}

	private void RefreshProgressText(bool forceRefresh)
	{
		if (_progressTextLabel == null)
		{
			return;
		}
		_progressTextLabel.Visible = showProgressText;
		if (!showProgressText || _progressBar == null)
		{
			return;
		}
		ulong ticksMsec = Time.GetTicksMsec();
		if (forceRefresh || manualProgressValue || ticksMsec >= _nextAutoTextRefreshMsec)
		{
			if (!manualProgressValue)
			{
				_nextAutoTextRefreshMsec = ticksMsec + 100;
			}
			double num = ((_progressBar.MaxValue > 0.0) ? _progressBar.MaxValue : 1.0);
			bool flag = _progressTextTemplate.Contains("{value}", StringComparison.Ordinal);
			bool flag2 = _progressTextTemplate.Contains("{max}", StringComparison.Ordinal);
			bool flag3 = _progressTextTemplate.Contains("{percent}", StringComparison.Ordinal);
			long num2 = (flag ? ((long)Math.Round(_progressBar.Value * 100.0, MidpointRounding.AwayFromZero)) : 0);
			long num3 = (flag2 ? ((long)Math.Round(num * 100.0, MidpointRounding.AwayFromZero)) : 0);
			long num4 = (flag3 ? ((long)Math.Round(_progressBar.Value / num * 10000.0, MidpointRounding.AwayFromZero)) : 0);
			if (num2 != _lastTextValueUnits || num3 != _lastTextMaxUnits || num4 != _lastTextPercentUnits || !string.Equals(_progressTextTemplate, _lastRenderedTemplate, StringComparison.Ordinal))
			{
				_lastTextValueUnits = num2;
				_lastTextMaxUnits = num3;
				_lastTextPercentUnits = num4;
				_lastRenderedTemplate = _progressTextTemplate;
				string newValue = (flag ? ((double)num2 / 100.0).ToString("0.##", CultureInfo.InvariantCulture) : "");
				string newValue2 = (flag2 ? ((double)num3 / 100.0).ToString("0.##", CultureInfo.InvariantCulture) : "");
				string newValue3 = (flag3 ? ((double)num4 / 100.0).ToString("0.##", CultureInfo.InvariantCulture) : "");
				_progressTextLabel.Text = _progressTextTemplate.Replace("{value}", newValue, StringComparison.Ordinal).Replace("{max}", newValue2, StringComparison.Ordinal).Replace("{percent}", newValue3, StringComparison.Ordinal);
			}
		}
	}

	public override void _Ready()
	{
		_progressBar = GetNode<TextureProgressBar>("%ProgressBar");
		_headTexture = GetNode<TextureRect>("%HeadTexture");
		_flagContainer = GetNode<HBoxContainer>("%FlagContainer");
		_progressTextLabel = GetNodeOrNull<Label>("%ProgressTextLabel");
		_progressBar.ValueChanged += OnProgressValueChanged;
		VisibilityChanged += RefreshPhysicsProcessing;
		if (_hasInitConfig)
		{
			FlagRefresh();
		}
		RefreshProgressPresentation(forceTextRefresh: true);
		RefreshPhysicsProcessing();
	}

	private void OnProgressValueChanged(double value)
	{
		RefreshProgressPresentation();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "newWaveNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "newWaveInterval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAutoProgressResponse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "response", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetWaveCurrent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveCurrent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHideItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hide", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetManualProgressEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetManualProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maxValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressMaxValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "maxValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressTextVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlagRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFlagReachState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ClearFlags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPhysicsProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProgressPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "forceTextRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshProgressText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "forceRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnProgressValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAutoProgressResponse && args.Count == 1)
		{
			SetAutoProgressResponse(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetWaveCurrent && args.Count == 1)
		{
			SetWaveCurrent(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHideItem && args.Count == 1)
		{
			SetHideItem(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetManualProgressEnabled && args.Count == 1)
		{
			SetManualProgressEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetManualProgress && args.Count == 3)
		{
			SetManualProgress(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressValue && args.Count == 1)
		{
			SetProgressValue(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressMaxValue && args.Count == 1)
		{
			SetProgressMaxValue(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressText && args.Count == 1)
		{
			SetProgressText(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressTextVisible && args.Count == 1)
		{
			SetProgressTextVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlagRefresh && args.Count == 0)
		{
			FlagRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFlagReachState && args.Count == 0)
		{
			RefreshFlagReachState();
			ret = default;
			return true;
		}
		if (method == MethodName._ClearFlags && args.Count == 0)
		{
			_ClearFlags();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPhysicsProcessing && args.Count == 0)
		{
			RefreshPhysicsProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProgressPresentation && args.Count == 1)
		{
			RefreshProgressPresentation(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProgressText && args.Count == 1)
		{
			RefreshProgressText(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.OnProgressValueChanged && args.Count == 1)
		{
			OnProgressValueChanged(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.SetAutoProgressResponse)
		{
			return true;
		}
		if (method == MethodName.SetWaveCurrent)
		{
			return true;
		}
		if (method == MethodName.SetHideItem)
		{
			return true;
		}
		if (method == MethodName.SetManualProgressEnabled)
		{
			return true;
		}
		if (method == MethodName.SetManualProgress)
		{
			return true;
		}
		if (method == MethodName.SetProgressValue)
		{
			return true;
		}
		if (method == MethodName.SetProgressMaxValue)
		{
			return true;
		}
		if (method == MethodName.SetProgressText)
		{
			return true;
		}
		if (method == MethodName.SetProgressTextVisible)
		{
			return true;
		}
		if (method == MethodName.FlagRefresh)
		{
			return true;
		}
		if (method == MethodName.RefreshFlagReachState)
		{
			return true;
		}
		if (method == MethodName._ClearFlags)
		{
			return true;
		}
		if (method == MethodName.RefreshPhysicsProcessing)
		{
			return true;
		}
		if (method == MethodName.RefreshProgressPresentation)
		{
			return true;
		}
		if (method == MethodName.RefreshProgressText)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.OnProgressValueChanged)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.waveNum)
		{
			waveNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.waveInterval)
		{
			waveInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.previewWave)
		{
			previewWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._progressBar)
		{
			_progressBar = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._headTexture)
		{
			_headTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._flagContainer)
		{
			_flagContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._progressTextLabel)
		{
			_progressTextLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._progressTextTemplate)
		{
			_progressTextTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._hasInitConfig)
		{
			_hasInitConfig = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nextAutoTextRefreshMsec)
		{
			_nextAutoTextRefreshMsec = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._lastTextValueUnits)
		{
			_lastTextValueUnits = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastTextMaxUnits)
		{
			_lastTextMaxUnits = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastTextPercentUnits)
		{
			_lastTextPercentUnits = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lastRenderedTemplate)
		{
			_lastRenderedTemplate = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._autoProgressResponse)
		{
			_autoProgressResponse = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._renderedFlagWaveNum)
		{
			_renderedFlagWaveNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._renderedFlagWaveInterval)
		{
			_renderedFlagWaveInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hideItem)
		{
			hideItem = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.manualProgressValue)
		{
			manualProgressValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.showProgressText)
		{
			showProgressText = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._waveNum)
		{
			_waveNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._waveInterval)
		{
			_waveInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._previewWave)
		{
			_previewWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.flagList)
		{
			flagList = VariantUtils.ConvertToArray<GeneralProgressFlag>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.ManualProgressText)
		{
			from = ManualProgressText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ManualProgressTextTemplate)
		{
			from = ManualProgressTextTemplate;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.waveNum)
		{
			from2 = waveNum;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.waveInterval)
		{
			from2 = waveInterval;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.previewWave)
		{
			from2 = previewWave;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._progressBar)
		{
			value = VariantUtils.CreateFrom(in _progressBar);
			return true;
		}
		if (name == PropertyName._headTexture)
		{
			value = VariantUtils.CreateFrom(in _headTexture);
			return true;
		}
		if (name == PropertyName._flagContainer)
		{
			value = VariantUtils.CreateFrom(in _flagContainer);
			return true;
		}
		if (name == PropertyName._progressTextLabel)
		{
			value = VariantUtils.CreateFrom(in _progressTextLabel);
			return true;
		}
		if (name == PropertyName._progressTextTemplate)
		{
			value = VariantUtils.CreateFrom(in _progressTextTemplate);
			return true;
		}
		if (name == PropertyName._hasInitConfig)
		{
			value = VariantUtils.CreateFrom(in _hasInitConfig);
			return true;
		}
		if (name == PropertyName._nextAutoTextRefreshMsec)
		{
			value = VariantUtils.CreateFrom(in _nextAutoTextRefreshMsec);
			return true;
		}
		if (name == PropertyName._lastTextValueUnits)
		{
			value = VariantUtils.CreateFrom(in _lastTextValueUnits);
			return true;
		}
		if (name == PropertyName._lastTextMaxUnits)
		{
			value = VariantUtils.CreateFrom(in _lastTextMaxUnits);
			return true;
		}
		if (name == PropertyName._lastTextPercentUnits)
		{
			value = VariantUtils.CreateFrom(in _lastTextPercentUnits);
			return true;
		}
		if (name == PropertyName._lastRenderedTemplate)
		{
			value = VariantUtils.CreateFrom(in _lastRenderedTemplate);
			return true;
		}
		if (name == PropertyName._autoProgressResponse)
		{
			value = VariantUtils.CreateFrom(in _autoProgressResponse);
			return true;
		}
		if (name == PropertyName._renderedFlagWaveNum)
		{
			value = VariantUtils.CreateFrom(in _renderedFlagWaveNum);
			return true;
		}
		if (name == PropertyName._renderedFlagWaveInterval)
		{
			value = VariantUtils.CreateFrom(in _renderedFlagWaveInterval);
			return true;
		}
		if (name == PropertyName.hideItem)
		{
			value = VariantUtils.CreateFrom(in hideItem);
			return true;
		}
		if (name == PropertyName.manualProgressValue)
		{
			value = VariantUtils.CreateFrom(in manualProgressValue);
			return true;
		}
		if (name == PropertyName.showProgressText)
		{
			value = VariantUtils.CreateFrom(in showProgressText);
			return true;
		}
		if (name == PropertyName._waveNum)
		{
			value = VariantUtils.CreateFrom(in _waveNum);
			return true;
		}
		if (name == PropertyName._waveInterval)
		{
			value = VariantUtils.CreateFrom(in _waveInterval);
			return true;
		}
		if (name == PropertyName._previewWave)
		{
			value = VariantUtils.CreateFrom(in _previewWave);
			return true;
		}
		if (name == PropertyName.flagList)
		{
			value = VariantUtils.CreateFromArray(flagList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._progressBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._flagContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._progressTextLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._progressTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasInitConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextAutoTextRefreshMsec, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastTextValueUnits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastTextMaxUnits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastTextPercentUnits, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastRenderedTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._autoProgressResponse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderedFlagWaveNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderedFlagWaveInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ManualProgressText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ManualProgressTextTemplate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hideItem, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.manualProgressValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.showProgressText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._waveNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.waveNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._waveInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.waveInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._previewWave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.previewWave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.flagList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.waveNum, Variant.From<int>(waveNum));
		info.AddProperty(PropertyName.waveInterval, Variant.From<int>(waveInterval));
		info.AddProperty(PropertyName.previewWave, Variant.From<int>(previewWave));
		info.AddProperty(PropertyName._progressBar, Variant.From(in _progressBar));
		info.AddProperty(PropertyName._headTexture, Variant.From(in _headTexture));
		info.AddProperty(PropertyName._flagContainer, Variant.From(in _flagContainer));
		info.AddProperty(PropertyName._progressTextLabel, Variant.From(in _progressTextLabel));
		info.AddProperty(PropertyName._progressTextTemplate, Variant.From(in _progressTextTemplate));
		info.AddProperty(PropertyName._hasInitConfig, Variant.From(in _hasInitConfig));
		info.AddProperty(PropertyName._nextAutoTextRefreshMsec, Variant.From(in _nextAutoTextRefreshMsec));
		info.AddProperty(PropertyName._lastTextValueUnits, Variant.From(in _lastTextValueUnits));
		info.AddProperty(PropertyName._lastTextMaxUnits, Variant.From(in _lastTextMaxUnits));
		info.AddProperty(PropertyName._lastTextPercentUnits, Variant.From(in _lastTextPercentUnits));
		info.AddProperty(PropertyName._lastRenderedTemplate, Variant.From(in _lastRenderedTemplate));
		info.AddProperty(PropertyName._autoProgressResponse, Variant.From(in _autoProgressResponse));
		info.AddProperty(PropertyName._renderedFlagWaveNum, Variant.From(in _renderedFlagWaveNum));
		info.AddProperty(PropertyName._renderedFlagWaveInterval, Variant.From(in _renderedFlagWaveInterval));
		info.AddProperty(PropertyName.hideItem, Variant.From(in hideItem));
		info.AddProperty(PropertyName.manualProgressValue, Variant.From(in manualProgressValue));
		info.AddProperty(PropertyName.showProgressText, Variant.From(in showProgressText));
		info.AddProperty(PropertyName._waveNum, Variant.From(in _waveNum));
		info.AddProperty(PropertyName._waveInterval, Variant.From(in _waveInterval));
		info.AddProperty(PropertyName._previewWave, Variant.From(in _previewWave));
		info.AddProperty(PropertyName.flagList, Variant.CreateFrom(flagList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.waveNum, out var value))
		{
			waveNum = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.waveInterval, out var value2))
		{
			waveInterval = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.previewWave, out var value3))
		{
			previewWave = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName._progressBar, out var value4))
		{
			_progressBar = value4.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._headTexture, out var value5))
		{
			_headTexture = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._flagContainer, out var value6))
		{
			_flagContainer = value6.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._progressTextLabel, out var value7))
		{
			_progressTextLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._progressTextTemplate, out var value8))
		{
			_progressTextTemplate = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName._hasInitConfig, out var value9))
		{
			_hasInitConfig = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nextAutoTextRefreshMsec, out var value10))
		{
			_nextAutoTextRefreshMsec = value10.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._lastTextValueUnits, out var value11))
		{
			_lastTextValueUnits = value11.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastTextMaxUnits, out var value12))
		{
			_lastTextMaxUnits = value12.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastTextPercentUnits, out var value13))
		{
			_lastTextPercentUnits = value13.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lastRenderedTemplate, out var value14))
		{
			_lastRenderedTemplate = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName._autoProgressResponse, out var value15))
		{
			_autoProgressResponse = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName._renderedFlagWaveNum, out var value16))
		{
			_renderedFlagWaveNum = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._renderedFlagWaveInterval, out var value17))
		{
			_renderedFlagWaveInterval = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hideItem, out var value18))
		{
			hideItem = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.manualProgressValue, out var value19))
		{
			manualProgressValue = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.showProgressText, out var value20))
		{
			showProgressText = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._waveNum, out var value21))
		{
			_waveNum = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._waveInterval, out var value22))
		{
			_waveInterval = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._previewWave, out var value23))
		{
			_previewWave = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName.flagList, out var value24))
		{
			flagList = value24.AsGodotArray<GeneralProgressFlag>();
		}
	}
}
