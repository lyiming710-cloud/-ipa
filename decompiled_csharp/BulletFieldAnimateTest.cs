using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BulletFieldAnimateTest.cs")]
public class BulletFieldAnimateTest : Node2D
{
	private sealed class TestDefinition
	{
		public int Id;

		public Vector2 Offset;

		public float FrameRate;

		public int FrameCount;
	}

	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName LoadDefinitionsAndReferences = "LoadDefinitionsAndReferences";

		public static readonly StringName DrawReferenceBatchRow = "DrawReferenceBatchRow";

		public static readonly StringName DrawStressInstances = "DrawStressInstances";

		public static readonly StringName CreateBackgroundAndLabels = "CreateBackgroundAndLabels";

		public static readonly StringName AddLabel = "AddLabel";

		public static readonly StringName CaptureScreenshot = "CaptureScreenshot";

		public static readonly StringName FinishTest = "FinishTest";

		public static readonly StringName ApplyCommandLineArguments = "ApplyCommandLineArguments";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName SpawnCount = "SpawnCount";

		public static readonly StringName ZBucketCount = "ZBucketCount";

		public static readonly StringName StressDefinitionIndex = "StressDefinitionIndex";

		public static readonly StringName StressScale = "StressScale";

		public static readonly StringName WarmupSeconds = "WarmupSeconds";

		public static readonly StringName MeasureSeconds = "MeasureSeconds";

		public static readonly StringName ScreenshotPath = "ScreenshotPath";

		public static readonly StringName _renderer = "_renderer";

		public static readonly StringName _elapsed = "_elapsed";

		public static readonly StringName _measuredFrames = "_measuredFrames";

		public static readonly StringName _screenshotSaved = "_screenshotSaved";

		public static readonly StringName _finished = "_finished";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly string[] ProjectileScenePaths = new string[6] { "res://Asset/Config/Projectile/Pea/Sprite/FirePea/FirePea.tscn", "res://Asset/Config/Projectile/Pea/Sprite/IceFirePea/IceFirePea.tscn", "res://Asset/Config/Projectile/Pea/Sprite/MegaFirePea/MegaFirePea.tscn", "res://Asset/Config/Projectile/Star/FireStar/FireStar.tscn", "res://Asset/Config/Projectile/Pea/Sprite/WhiteFirePea/WhiteFirePea.tscn", "res://Asset/Config/Projectile/Puff/Sprite/HypnoPuff/HypnoPuff.tscn" };

	private readonly List<TestDefinition> _definitions = new List<TestDefinition>();

	private AnimateMultiMeshRenderer _renderer;

	private double _elapsed;

	private int _measuredFrames;

	private bool _screenshotSaved;

	private bool _finished;

	[Export(PropertyHint.None, "")]
	public int SpawnCount { get; set; } = 3000;

	[Export(PropertyHint.None, "")]
	public int ZBucketCount { get; set; } = 6;

	[Export(PropertyHint.None, "")]
	public int StressDefinitionIndex { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public float StressScale { get; set; } = 0.32f;

	[Export(PropertyHint.None, "")]
	public double WarmupSeconds { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public double MeasureSeconds { get; set; } = 4.0;

	[Export(PropertyHint.None, "")]
	public string ScreenshotPath { get; set; } = "res://TestResults/bulletfield-unified-animation.png";

	public override void _Ready()
	{
		ApplyCommandLineArguments();
		CreateBackgroundAndLabels();
		_renderer = new AnimateMultiMeshRenderer
		{
			Name = "BulletFieldUnifiedAnimateRenderer"
		};
		AddChild(_renderer, forceReadableName: false, InternalMode.Disabled);
		LoadDefinitionsAndReferences();
		if (_definitions.Count == 0)
		{
			GD.PrintErr("[BulletFieldAnimateTest] no compatible projectile animation definitions were loaded.");
			GetTree().Quit(1);
			return;
		}
		GD.Print($"[BulletFieldAnimateTest] ready definitions={_definitions.Count} spawn={SpawnCount} zBuckets={ZBucketCount} screenshot={ScreenshotPath}");
	}

	public override void _Process(double delta)
	{
		if (!_finished && _definitions.Count != 0)
		{
			_elapsed += delta;
			_renderer.SetAnimationTime(_elapsed);
			_renderer.BeginFrame();
			DrawReferenceBatchRow();
			DrawStressInstances();
			_renderer.EndFrame();
			if (_elapsed >= WarmupSeconds)
			{
				_measuredFrames++;
			}
			if (!_screenshotSaved && _elapsed >= WarmupSeconds + 0.5)
			{
				_screenshotSaved = CaptureScreenshot();
			}
			if (_elapsed >= WarmupSeconds + MeasureSeconds)
			{
				FinishTest();
			}
		}
	}

	private void LoadDefinitionsAndReferences()
	{
		for (int i = 0; i < ProjectileScenePaths.Length; i++)
		{
			PackedScene packedScene = GD.Load<PackedScene>(ProjectileScenePaths[i]);
			if (packedScene != null && packedScene.Instantiate(PackedScene.GenEditState.Disabled) is AdobeAnimateSprite { clip: var clip } adobeAnimateSprite)
			{
				int num = _renderer.RegisterDefinition(adobeAnimateSprite.flashAnimeData, clip, 9);
				AnimateMultiMeshRenderer.Definition definition = _renderer.GetDefinition(num);
				if (definition == null || definition.frameMax <= 0)
				{
					adobeAnimateSprite.Free();
					continue;
				}
				TestDefinition testDefinition = new TestDefinition
				{
					Id = num,
					Offset = adobeAnimateSprite.offset,
					FrameRate = (float)Math.Max(0.0, definition.frameRate),
					FrameCount = definition.frameMax
				};
				_definitions.Add(testDefinition);
				AddChild(adobeAnimateSprite, forceReadableName: false, InternalMode.Disabled);
				adobeAnimateSprite.SetClip(clip);
				adobeAnimateSprite.pause = true;
				float num2 = ResolveReferenceFrame(testDefinition);
				adobeAnimateSprite.frameIndex = definition.clipStart + (int)Math.Floor(num2);
				adobeAnimateSprite.elapsedTimer = (double)num2 - Math.Floor(num2);
				adobeAnimateSprite.Position = new Vector2(125f + (float)i * 165f, 105f);
				adobeAnimateSprite.ZAsRelative = false;
				adobeAnimateSprite.ZIndex = 200;
			}
		}
	}

	private void DrawReferenceBatchRow()
	{
		for (int i = 0; i < _definitions.Count; i++)
		{
			TestDefinition testDefinition = _definitions[i];
			Vector2 origin = new Vector2(125f + (float)i * 165f, 220f) + testDefinition.Offset;
			_renderer.DrawInstanceAtZIndex(testDefinition.Id, 201, new Transform2D(0f, origin), ResolveReferenceFrame(testDefinition), Colors.White);
		}
	}

	private void DrawStressInstances()
	{
		if (SpawnCount > 0)
		{
			Vector2 size = GetViewportRect().Size;
			float num = 290f;
			float num2 = Math.Max(1f, size.Y - num);
			int num3 = Math.Max(1, Mathf.CeilToInt(Mathf.Sqrt((float)SpawnCount * size.X / num2)));
			int num4 = Math.Max(1, (SpawnCount + num3 - 1) / num3);
			float num5 = size.X / (float)num3;
			float num6 = num2 / (float)num4;
			for (int i = 0; i < SpawnCount; i++)
			{
				int index = ((StressDefinitionIndex >= 0) ? (StressDefinitionIndex % _definitions.Count) : (i % _definitions.Count));
				TestDefinition testDefinition = _definitions[index];
				int num7 = i % num3;
				int num8 = i / num3;
				Vector2 vector = new Vector2(((float)num7 + 0.5f) * num5, num + ((float)num8 + 0.5f) * num6);
				Vector2 vector2 = Vector2.One * Math.Max(0.01f, StressScale);
				Vector2 originPos = vector + testDefinition.Offset * vector2;
				Transform2D transform = new Transform2D(new Vector2(vector2.X, 0f), new Vector2(0f, vector2.Y), originPos);
				float frameFloat = (float)(_elapsed * (double)testDefinition.FrameRate + (double)i * 0.61803398875);
				_renderer.DrawInstanceAtZIndex(testDefinition.Id, num8 % Math.Max(1, ZBucketCount), transform, frameFloat, Colors.White);
			}
		}
	}

	private static float ResolveReferenceFrame(TestDefinition definition)
	{
		return (float)Math.Min(2, Math.Max(0, definition.FrameCount - 1)) + 0.25f;
	}

	private void CreateBackgroundAndLabels()
	{
		Vector2 size = GetViewportRect().Size;
		ColorRect node = new ColorRect
		{
			Color = new Color(0.025f, 0.055f, 0.04f),
			Size = size,
			MouseFilter = Control.MouseFilterEnum.Ignore,
			ZAsRelative = false,
			ZIndex = -4096
		};
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
		AddLabel("完整 Sprite 参考", new Vector2(18f, 18f));
		AddLabel("BulletField Compact Crowd", new Vector2(18f, 145f));
		AddLabel($"{SpawnCount} 个动态实例压力区", new Vector2(18f, 264f));
	}

	private void AddLabel(string text, Vector2 position)
	{
		Label label = new Label
		{
			Text = text,
			Position = position,
			ZAsRelative = false,
			ZIndex = 4096
		};
		label.AddThemeFontSizeOverride("font_size", 22);
		AddChild(label, forceReadableName: false, InternalMode.Disabled);
	}

	private bool CaptureScreenshot()
	{
		if (string.IsNullOrWhiteSpace(ScreenshotPath))
		{
			return true;
		}
		Image image = GetViewport().GetTexture().GetImage();
		if (image == null || image.IsEmpty())
		{
			return false;
		}
		string text = ScreenshotPath.Replace('\\', '/');
		string text2 = (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? ProjectSettings.GlobalizePath(text) : text);
		Error error = DirAccess.MakeDirRecursiveAbsolute(text2.GetBaseDir());
		if (error != Error.Ok && error != Error.AlreadyExists)
		{
			return false;
		}
		Error error2 = image.SavePng(text2);
		if (error2 == Error.Ok)
		{
			GD.Print($"[BulletFieldAnimateTest] screenshot saved path={text} size={image.GetWidth()}x{image.GetHeight()}");
		}
		return error2 == Error.Ok;
	}

	private void FinishTest()
	{
		_finished = true;
		double num = Math.Max(0.0001, _elapsed - WarmupSeconds);
		double num2 = (double)_measuredFrames / num;
		int num3 = SpawnCount + _definitions.Count;
		int visibleInstanceCountForTest = _renderer.GetVisibleInstanceCountForTest();
		int unifiedBucketCountForTest = _renderer.GetUnifiedBucketCountForTest();
		bool flag = _screenshotSaved && visibleInstanceCountForTest == num3 && unifiedBucketCountForTest > 0 && num2 > 0.0;
		GD.Print($"[BulletFieldAnimateTestResult] spawn={SpawnCount} definitions={_definitions.Count} visible={visibleInstanceCountForTest}/{num3} buckets={unifiedBucketCountForTest} averageFps={num2:F2} screenshotSaved={_screenshotSaved} passed={flag}");
		GetTree().Quit((!flag) ? 1 : 0);
	}

	private void ApplyCommandLineArguments()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string text in cmdlineUserArgs)
		{
			int value2;
			int value3;
			double value4;
			double value5;
			if (TryReadInt(text, "--bullet-test-spawn=", out var value))
			{
				SpawnCount = Math.Max(0, value);
			}
			else if (TryReadInt(text, "--bullet-test-z-buckets=", out value2))
			{
				ZBucketCount = Math.Max(1, value2);
			}
			else if (TryReadInt(text, "--bullet-test-definition=", out value3))
			{
				StressDefinitionIndex = value3;
			}
			else if (TryReadDouble(text, "--bullet-test-warmup=", out value4))
			{
				WarmupSeconds = Math.Max(0.0, value4);
			}
			else if (TryReadDouble(text, "--bullet-test-measure=", out value5))
			{
				MeasureSeconds = Math.Max(0.1, value5);
			}
			else if (text.StartsWith("--bullet-test-screenshot=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text;
				int length = "--bullet-test-screenshot=".Length;
				ScreenshotPath = text2.Substring(length, text2.Length - length);
			}
		}
	}

	private static bool TryReadInt(string arg, string prefix, out int value)
	{
		value = 0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return int.TryParse(arg.Substring(length, arg.Length - length), out value);
		}
		return false;
	}

	private static bool TryReadDouble(string arg, string prefix, out double value)
	{
		value = 0.0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			int length = prefix.Length;
			return double.TryParse(arg.Substring(length, arg.Length - length), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadDefinitionsAndReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawReferenceBatchRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawStressInstances, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBackgroundAndLabels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureScreenshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishTest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCommandLineArguments, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadDefinitionsAndReferences && args.Count == 0)
		{
			LoadDefinitionsAndReferences();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawReferenceBatchRow && args.Count == 0)
		{
			DrawReferenceBatchRow();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawStressInstances && args.Count == 0)
		{
			DrawStressInstances();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBackgroundAndLabels && args.Count == 0)
		{
			CreateBackgroundAndLabels();
			ret = default;
			return true;
		}
		if (method == MethodName.AddLabel && args.Count == 2)
		{
			AddLabel(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureScreenshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CaptureScreenshot());
			return true;
		}
		if (method == MethodName.FinishTest && args.Count == 0)
		{
			FinishTest();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments && args.Count == 0)
		{
			ApplyCommandLineArguments();
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.LoadDefinitionsAndReferences)
		{
			return true;
		}
		if (method == MethodName.DrawReferenceBatchRow)
		{
			return true;
		}
		if (method == MethodName.DrawStressInstances)
		{
			return true;
		}
		if (method == MethodName.CreateBackgroundAndLabels)
		{
			return true;
		}
		if (method == MethodName.AddLabel)
		{
			return true;
		}
		if (method == MethodName.CaptureScreenshot)
		{
			return true;
		}
		if (method == MethodName.FinishTest)
		{
			return true;
		}
		if (method == MethodName.ApplyCommandLineArguments)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SpawnCount)
		{
			SpawnCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ZBucketCount)
		{
			ZBucketCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.StressDefinitionIndex)
		{
			StressDefinitionIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.StressScale)
		{
			StressScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.WarmupSeconds)
		{
			WarmupSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MeasureSeconds)
		{
			MeasureSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ScreenshotPath)
		{
			ScreenshotPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._renderer)
		{
			_renderer = VariantUtils.ConvertTo<AnimateMultiMeshRenderer>(in value);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			_elapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._measuredFrames)
		{
			_measuredFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._screenshotSaved)
		{
			_screenshotSaved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._finished)
		{
			_finished = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.SpawnCount)
		{
			from = SpawnCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZBucketCount)
		{
			from = ZBucketCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.StressDefinitionIndex)
		{
			from = StressDefinitionIndex;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.StressScale)
		{
			value = VariantUtils.CreateFrom<float>(StressScale);
			return true;
		}
		double from2;
		if (name == PropertyName.WarmupSeconds)
		{
			from2 = WarmupSeconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MeasureSeconds)
		{
			from2 = MeasureSeconds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ScreenshotPath)
		{
			value = VariantUtils.CreateFrom<string>(ScreenshotPath);
			return true;
		}
		if (name == PropertyName._renderer)
		{
			value = VariantUtils.CreateFrom(in _renderer);
			return true;
		}
		if (name == PropertyName._elapsed)
		{
			value = VariantUtils.CreateFrom(in _elapsed);
			return true;
		}
		if (name == PropertyName._measuredFrames)
		{
			value = VariantUtils.CreateFrom(in _measuredFrames);
			return true;
		}
		if (name == PropertyName._screenshotSaved)
		{
			value = VariantUtils.CreateFrom(in _screenshotSaved);
			return true;
		}
		if (name == PropertyName._finished)
		{
			value = VariantUtils.CreateFrom(in _finished);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.SpawnCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZBucketCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.StressDefinitionIndex, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.StressScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.WarmupSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MeasureSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ScreenshotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._renderer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._elapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measuredFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._screenshotSaved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._finished, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SpawnCount, Variant.From<int>(SpawnCount));
		info.AddProperty(PropertyName.ZBucketCount, Variant.From<int>(ZBucketCount));
		info.AddProperty(PropertyName.StressDefinitionIndex, Variant.From<int>(StressDefinitionIndex));
		info.AddProperty(PropertyName.StressScale, Variant.From<float>(StressScale));
		info.AddProperty(PropertyName.WarmupSeconds, Variant.From<double>(WarmupSeconds));
		info.AddProperty(PropertyName.MeasureSeconds, Variant.From<double>(MeasureSeconds));
		info.AddProperty(PropertyName.ScreenshotPath, Variant.From<string>(ScreenshotPath));
		info.AddProperty(PropertyName._renderer, Variant.From(in _renderer));
		info.AddProperty(PropertyName._elapsed, Variant.From(in _elapsed));
		info.AddProperty(PropertyName._measuredFrames, Variant.From(in _measuredFrames));
		info.AddProperty(PropertyName._screenshotSaved, Variant.From(in _screenshotSaved));
		info.AddProperty(PropertyName._finished, Variant.From(in _finished));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SpawnCount, out var value))
		{
			SpawnCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ZBucketCount, out var value2))
		{
			ZBucketCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.StressDefinitionIndex, out var value3))
		{
			StressDefinitionIndex = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.StressScale, out var value4))
		{
			StressScale = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.WarmupSeconds, out var value5))
		{
			WarmupSeconds = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MeasureSeconds, out var value6))
		{
			MeasureSeconds = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ScreenshotPath, out var value7))
		{
			ScreenshotPath = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._renderer, out var value8))
		{
			_renderer = value8.As<AnimateMultiMeshRenderer>();
		}
		if (info.TryGetProperty(PropertyName._elapsed, out var value9))
		{
			_elapsed = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._measuredFrames, out var value10))
		{
			_measuredFrames = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._screenshotSaved, out var value11))
		{
			_screenshotSaved = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._finished, out var value12))
		{
			_finished = value12.As<bool>();
		}
	}
}
