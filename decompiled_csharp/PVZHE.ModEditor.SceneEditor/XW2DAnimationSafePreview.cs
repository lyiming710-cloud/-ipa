using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/Animation/XW2DAnimationSafePreview.cs")]
public class XW2DAnimationSafePreview : Node
{
	private sealed class PropertyBaseline
	{
		public GodotObject Target;

		public NodePath PropertyPath;

		public Variant Value;
	}

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName Bind = "Bind";

		public static readonly StringName Play = "Play";

		public static readonly StringName Pause = "Pause";

		public static readonly StringName Seek = "Seek";

		public static readonly StringName Stop = "Stop";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName CanPreview = "CanPreview";

		public static readonly StringName IsPreviewPlayerValid = "IsPreviewPlayerValid";

		public static readonly StringName EnsureBaselines = "EnsureBaselines";

		public static readonly StringName CaptureBaselines = "CaptureBaselines";

		public static readonly StringName TryCaptureTrackBaseline = "TryCaptureTrackBaseline";

		public static readonly StringName RestoreBaselines = "RestoreBaselines";

		public static readonly StringName CloneBaselineValue = "CloneBaselineValue";

		public static readonly StringName PropertyPathForTransformTrack = "PropertyPathForTransformTrack";

		public static readonly StringName IsUnsafeTrack = "IsUnsafeTrack";

		public static readonly StringName NormalizePreviewName = "NormalizePreviewName";

		public static readonly StringName ReleasePlayer = "ReleasePlayer";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName UnsafeTrackCount = "UnsafeTrackCount";

		public static readonly StringName BaselineCount = "BaselineCount";

		public static readonly StringName IsPlaying = "IsPlaying";

		public static readonly StringName CurrentPosition = "CurrentPosition";

		public static readonly StringName SpeedScale = "SpeedScale";

		public static readonly StringName _previewPlayer = "_previewPlayer";

		public static readonly StringName _previewLibrary = "_previewLibrary";

		public static readonly StringName _safeAnimation = "_safeAnimation";

		public static readonly StringName _previewName = "_previewName";

		public static readonly StringName _baselineCaptured = "_baselineCaptured";

		public static readonly StringName _isPlaying = "_isPlaying";

		public static readonly StringName _speedScale = "_speedScale";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string InternalPlayerNamePrefix = "__XW2DAnimationSafePreview_";

	private const double MinimumSpeedScale = 0.1;

	private const double MaximumSpeedScale = 4.0;

	private readonly List<PropertyBaseline> _baselines = new List<PropertyBaseline>();

	private readonly HashSet<string> _baselineKeys = new HashSet<string>(StringComparer.Ordinal);

	private AnimationPlayer _previewPlayer;

	private AnimationLibrary _previewLibrary;

	private Animation _safeAnimation;

	private StringName _previewName = new StringName();

	private bool _baselineCaptured;

	private bool _isPlaying;

	private double _speedScale = 1.0;

	public int UnsafeTrackCount { get; private set; }

	public int BaselineCount => _baselines.Count;

	public bool IsPlaying => _isPlaying;

	public double CurrentPosition
	{
		get
		{
			if (!IsPreviewPlayerValid())
			{
				return 0.0;
			}
			return _previewPlayer.CurrentAnimationPosition;
		}
	}

	public double SpeedScale
	{
		get
		{
			return _speedScale;
		}
		set
		{
			_speedScale = Math.Clamp(value, 0.1, 4.0);
		}
	}

	public override void _Ready()
	{
		SetProcess(enable: false);
	}

	public override void _ExitTree()
	{
		Clear();
	}

	public override void _Process(double delta)
	{
		if (!_isPlaying || !IsPreviewPlayerValid())
		{
			_isPlaying = false;
			SetProcess(enable: false);
			return;
		}
		_previewPlayer.Advance(delta * _speedScale);
		if (!_previewPlayer.IsPlaying())
		{
			_isPlaying = false;
			SetProcess(enable: false);
		}
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 1)
		{
			Clear();
		}
	}

	public bool Bind(AnimationPlayer sourcePlayer, Animation sourceAnimation, StringName previewName)
	{
		Clear();
		if (!GodotObject.IsInstanceValid(sourcePlayer) || !GodotObject.IsInstanceValid(sourceAnimation))
		{
			return false;
		}
		Node parent = sourcePlayer.GetParent();
		Node nodeOrNull = sourcePlayer.GetNodeOrNull(sourcePlayer.RootNode);
		if (!GodotObject.IsInstanceValid(parent) || !GodotObject.IsInstanceValid(nodeOrNull))
		{
			return false;
		}
		Animation animation = sourceAnimation.Duplicate(deep: true) as Animation;
		if (!GodotObject.IsInstanceValid(animation))
		{
			return false;
		}
		int num = 0;
		for (int num2 = animation.GetTrackCount() - 1; num2 >= 0; num2--)
		{
			if (IsUnsafeTrack(animation.TrackGetType(num2)))
			{
				animation.RemoveTrack(num2);
				num++;
			}
		}
		StringName stringName = NormalizePreviewName(previewName);
		AnimationLibrary animationLibrary = new AnimationLibrary();
		if (animationLibrary.AddAnimation(stringName, animation) != Error.Ok)
		{
			return false;
		}
		AnimationPlayer animationPlayer = new AnimationPlayer
		{
			Name = new StringName($"{"__XW2DAnimationSafePreview_"}{GetInstanceId()}"),
			RootNode = sourcePlayer.RootNode,
			CallbackModeProcess = AnimationMixer.AnimationCallbackModeProcess.Manual,
			ProcessMode = ProcessModeEnum.Disabled,
			Active = true
		};
		parent.AddChild(animationPlayer, forceReadableName: false, InternalMode.Back);
		animationPlayer.Owner = null;
		if (animationPlayer.AddAnimationLibrary(new StringName(), animationLibrary) != Error.Ok)
		{
			ReleasePlayer(animationPlayer);
			return false;
		}
		_previewPlayer = animationPlayer;
		_previewLibrary = animationLibrary;
		_safeAnimation = animation;
		_previewName = stringName;
		UnsafeTrackCount = num;
		CaptureBaselines();
		return true;
	}

	public void Play()
	{
		if (CanPreview())
		{
			EnsureBaselines();
			_previewPlayer.Play(_previewName);
			_isPlaying = true;
			SetProcess(enable: true);
		}
	}

	public void Pause()
	{
		if (IsPreviewPlayerValid())
		{
			_previewPlayer.Pause();
		}
		_isPlaying = false;
		SetProcess(enable: false);
	}

	public void Seek(double position)
	{
		if (CanPreview())
		{
			EnsureBaselines();
			double seconds = Math.Clamp(position, 0.0, Math.Max(0.0, _safeAnimation.Length));
			bool isPlaying = _isPlaying;
			_previewPlayer.Play(_previewName);
			_previewPlayer.Seek(seconds, update: true);
			if (!isPlaying)
			{
				_previewPlayer.Pause();
			}
			_isPlaying = isPlaying;
			SetProcess(isPlaying);
		}
	}

	public void Stop()
	{
		_isPlaying = false;
		SetProcess(enable: false);
		if (IsPreviewPlayerValid())
		{
			_previewPlayer.Stop(keepState: true);
		}
		RestoreBaselines();
	}

	public void Clear()
	{
		Stop();
		AnimationPlayer previewPlayer = _previewPlayer;
		_previewPlayer = null;
		if (GodotObject.IsInstanceValid(previewPlayer))
		{
			ReleasePlayer(previewPlayer);
		}
		_previewLibrary = null;
		_safeAnimation = null;
		_previewName = new StringName();
		UnsafeTrackCount = 0;
		_baselines.Clear();
		_baselineKeys.Clear();
		_baselineCaptured = false;
	}

	private bool CanPreview()
	{
		if (IsPreviewPlayerValid() && GodotObject.IsInstanceValid(_safeAnimation))
		{
			return !string.IsNullOrEmpty(_previewName.ToString());
		}
		return false;
	}

	private bool IsPreviewPlayerValid()
	{
		return GodotObject.IsInstanceValid(_previewPlayer);
	}

	private void EnsureBaselines()
	{
		if (!_baselineCaptured)
		{
			CaptureBaselines();
		}
	}

	private void CaptureBaselines()
	{
		_baselines.Clear();
		_baselineKeys.Clear();
		_baselineCaptured = true;
		if (!IsPreviewPlayerValid() || !GodotObject.IsInstanceValid(_safeAnimation))
		{
			return;
		}
		Node nodeOrNull = _previewPlayer.GetNodeOrNull(_previewPlayer.RootNode);
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			return;
		}
		for (int i = 0; i < _safeAnimation.GetTrackCount(); i++)
		{
			if (_safeAnimation.TrackIsEnabled(i))
			{
				Animation.TrackType trackType = _safeAnimation.TrackGetType(i);
				if (!IsUnsafeTrack(trackType))
				{
					TryCaptureTrackBaseline(nodeOrNull, _safeAnimation.TrackGetPath(i), trackType);
				}
			}
		}
	}

	private void TryCaptureTrackBaseline(Node root, NodePath trackPath, Animation.TrackType trackType)
	{
		try
		{
			Godot.Collections.Array nodeAndResource = root.GetNodeAndResource(trackPath);
			if (nodeAndResource.Count < 3)
			{
				return;
			}
			Node node = nodeAndResource[0].As<Node>();
			Resource resource = nodeAndResource[1].As<Resource>();
			GodotObject godotObject = (GodotObject.IsInstanceValid(resource) ? ((GodotObject)resource) : ((GodotObject)node));
			if (!GodotObject.IsInstanceValid(godotObject))
			{
				return;
			}
			NodePath nodePath = nodeAndResource[2].AsNodePath();
			if (nodePath.IsEmpty)
			{
				nodePath = PropertyPathForTransformTrack(trackType);
			}
			if (!(nodePath == null) && !nodePath.IsEmpty)
			{
				string item = $"{godotObject.GetInstanceId()}|{nodePath}";
				if (_baselineKeys.Add(item))
				{
					_baselines.Add(new PropertyBaseline
					{
						Target = godotObject,
						PropertyPath = nodePath,
						Value = CloneBaselineValue(godotObject.GetIndexed(nodePath))
					});
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private void RestoreBaselines()
	{
		for (int num = _baselines.Count - 1; num >= 0; num--)
		{
			PropertyBaseline propertyBaseline = _baselines[num];
			if (GodotObject.IsInstanceValid(propertyBaseline.Target))
			{
				try
				{
					propertyBaseline.Target.SetIndexed(propertyBaseline.PropertyPath, propertyBaseline.Value);
				}
				catch (Exception)
				{
				}
			}
		}
		_baselines.Clear();
		_baselineKeys.Clear();
		_baselineCaptured = false;
	}

	private static Variant CloneBaselineValue(Variant value)
	{
		return value.VariantType switch
		{
			Variant.Type.Array => Variant.From<Godot.Collections.Array>(value.AsGodotArray().Duplicate(deep: true)), 
			Variant.Type.Dictionary => Variant.From<Dictionary>(value.AsGodotDictionary().Duplicate(deep: true)), 
			_ => value, 
		};
	}

	private static NodePath PropertyPathForTransformTrack(Animation.TrackType trackType)
	{
		Animation.TrackType num = trackType - 1;
		if ((ulong)num <= 2uL)
		{
			switch ((int)num)
			{
			case 0:
				return new NodePath(":position");
			case 1:
				return new NodePath(":quaternion");
			case 2:
				return new NodePath(":scale");
			}
		}
		return null;
	}

	private static bool IsUnsafeTrack(Animation.TrackType trackType)
	{
		if (trackType != Animation.TrackType.Method && trackType != Animation.TrackType.Audio)
		{
			return trackType == Animation.TrackType.Animation;
		}
		return true;
	}

	private static StringName NormalizePreviewName(StringName previewName)
	{
		string text = previewName.ToString().Trim();
		if (string.IsNullOrEmpty(text) || text.Contains('/'))
		{
			text = "安全预览";
		}
		return new StringName(text);
	}

	private static void ReleasePlayer(AnimationPlayer player)
	{
		if (GodotObject.IsInstanceValid(player))
		{
			Node parent = player.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(player);
			}
			player.Free();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sourcePlayer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationPlayer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "sourceAnimation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Animation"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "previewName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Play, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Pause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Seek, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Stop, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanPreview, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPreviewPlayerValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureBaselines, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureBaselines, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryCaptureTrackBaseline, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.NodePath, "trackPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "trackType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreBaselines, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloneBaselineValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.PropertyPathForTransformTrack, new PropertyInfo(Variant.Type.NodePath, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "trackType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsUnsafeTrack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "trackType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizePreviewName, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "previewName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleasePlayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "player", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AnimationPlayer"), exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Bind && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(Bind(VariantUtils.ConvertTo<AnimationPlayer>(in args[0]), VariantUtils.ConvertTo<Animation>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2])));
			return true;
		}
		if (method == MethodName.Play && args.Count == 0)
		{
			Play();
			ret = default;
			return true;
		}
		if (method == MethodName.Pause && args.Count == 0)
		{
			Pause();
			ret = default;
			return true;
		}
		if (method == MethodName.Seek && args.Count == 1)
		{
			Seek(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Stop && args.Count == 0)
		{
			Stop();
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.CanPreview && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanPreview());
			return true;
		}
		if (method == MethodName.IsPreviewPlayerValid && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPreviewPlayerValid());
			return true;
		}
		if (method == MethodName.EnsureBaselines && args.Count == 0)
		{
			EnsureBaselines();
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureBaselines && args.Count == 0)
		{
			CaptureBaselines();
			ret = default;
			return true;
		}
		if (method == MethodName.TryCaptureTrackBaseline && args.Count == 3)
		{
			TryCaptureTrackBaseline(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<NodePath>(in args[1]), VariantUtils.ConvertTo<Animation.TrackType>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreBaselines && args.Count == 0)
		{
			RestoreBaselines();
			ret = default;
			return true;
		}
		if (method == MethodName.CloneBaselineValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(CloneBaselineValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.PropertyPathForTransformTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<NodePath>(PropertyPathForTransformTrack(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.IsUnsafeTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUnsafeTrack(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePreviewName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(NormalizePreviewName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.ReleasePlayer && args.Count == 1)
		{
			ReleasePlayer(VariantUtils.ConvertTo<AnimationPlayer>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CloneBaselineValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(CloneBaselineValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.PropertyPathForTransformTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<NodePath>(PropertyPathForTransformTrack(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.IsUnsafeTrack && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsUnsafeTrack(VariantUtils.ConvertTo<Animation.TrackType>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizePreviewName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(NormalizePreviewName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.ReleasePlayer && args.Count == 1)
		{
			ReleasePlayer(VariantUtils.ConvertTo<AnimationPlayer>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName.Play)
		{
			return true;
		}
		if (method == MethodName.Pause)
		{
			return true;
		}
		if (method == MethodName.Seek)
		{
			return true;
		}
		if (method == MethodName.Stop)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.CanPreview)
		{
			return true;
		}
		if (method == MethodName.IsPreviewPlayerValid)
		{
			return true;
		}
		if (method == MethodName.EnsureBaselines)
		{
			return true;
		}
		if (method == MethodName.CaptureBaselines)
		{
			return true;
		}
		if (method == MethodName.TryCaptureTrackBaseline)
		{
			return true;
		}
		if (method == MethodName.RestoreBaselines)
		{
			return true;
		}
		if (method == MethodName.CloneBaselineValue)
		{
			return true;
		}
		if (method == MethodName.PropertyPathForTransformTrack)
		{
			return true;
		}
		if (method == MethodName.IsUnsafeTrack)
		{
			return true;
		}
		if (method == MethodName.NormalizePreviewName)
		{
			return true;
		}
		if (method == MethodName.ReleasePlayer)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.UnsafeTrackCount)
		{
			UnsafeTrackCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SpeedScale)
		{
			SpeedScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._previewPlayer)
		{
			_previewPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName._previewLibrary)
		{
			_previewLibrary = VariantUtils.ConvertTo<AnimationLibrary>(in value);
			return true;
		}
		if (name == PropertyName._safeAnimation)
		{
			_safeAnimation = VariantUtils.ConvertTo<Animation>(in value);
			return true;
		}
		if (name == PropertyName._previewName)
		{
			_previewName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._baselineCaptured)
		{
			_baselineCaptured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isPlaying)
		{
			_isPlaying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._speedScale)
		{
			_speedScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.UnsafeTrackCount)
		{
			from = UnsafeTrackCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BaselineCount)
		{
			from = BaselineCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsPlaying)
		{
			value = VariantUtils.CreateFrom<bool>(IsPlaying);
			return true;
		}
		double from2;
		if (name == PropertyName.CurrentPosition)
		{
			from2 = CurrentPosition;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SpeedScale)
		{
			from2 = SpeedScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._previewPlayer)
		{
			value = VariantUtils.CreateFrom(in _previewPlayer);
			return true;
		}
		if (name == PropertyName._previewLibrary)
		{
			value = VariantUtils.CreateFrom(in _previewLibrary);
			return true;
		}
		if (name == PropertyName._safeAnimation)
		{
			value = VariantUtils.CreateFrom(in _safeAnimation);
			return true;
		}
		if (name == PropertyName._previewName)
		{
			value = VariantUtils.CreateFrom(in _previewName);
			return true;
		}
		if (name == PropertyName._baselineCaptured)
		{
			value = VariantUtils.CreateFrom(in _baselineCaptured);
			return true;
		}
		if (name == PropertyName._isPlaying)
		{
			value = VariantUtils.CreateFrom(in _isPlaying);
			return true;
		}
		if (name == PropertyName._speedScale)
		{
			value = VariantUtils.CreateFrom(in _speedScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewLibrary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._safeAnimation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._previewName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._baselineCaptured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._speedScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.UnsafeTrackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.BaselineCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPlaying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.CurrentPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.SpeedScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.UnsafeTrackCount, Variant.From<int>(UnsafeTrackCount));
		info.AddProperty(PropertyName.SpeedScale, Variant.From<double>(SpeedScale));
		info.AddProperty(PropertyName._previewPlayer, Variant.From(in _previewPlayer));
		info.AddProperty(PropertyName._previewLibrary, Variant.From(in _previewLibrary));
		info.AddProperty(PropertyName._safeAnimation, Variant.From(in _safeAnimation));
		info.AddProperty(PropertyName._previewName, Variant.From(in _previewName));
		info.AddProperty(PropertyName._baselineCaptured, Variant.From(in _baselineCaptured));
		info.AddProperty(PropertyName._isPlaying, Variant.From(in _isPlaying));
		info.AddProperty(PropertyName._speedScale, Variant.From(in _speedScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.UnsafeTrackCount, out var value))
		{
			UnsafeTrackCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SpeedScale, out var value2))
		{
			SpeedScale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._previewPlayer, out var value3))
		{
			_previewPlayer = value3.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName._previewLibrary, out var value4))
		{
			_previewLibrary = value4.As<AnimationLibrary>();
		}
		if (info.TryGetProperty(PropertyName._safeAnimation, out var value5))
		{
			_safeAnimation = value5.As<Animation>();
		}
		if (info.TryGetProperty(PropertyName._previewName, out var value6))
		{
			_previewName = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._baselineCaptured, out var value7))
		{
			_baselineCaptured = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isPlaying, out var value8))
		{
			_isPlaying = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._speedScale, out var value9))
		{
			_speedScale = value9.As<double>();
		}
	}
}
