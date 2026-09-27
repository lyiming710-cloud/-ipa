using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/FogPlanternBoundaryRuntimeTest.cs")]
public class FogPlanternBoundaryRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateFogFeature = "CreateFogFeature";

		public static readonly StringName FindFog = "FindFog";

		public static readonly StringName ApplyPlanternLight = "ApplyPlanternLight";

		public static readonly StringName CountClearedPixels = "CountClearedPixels";

		public static readonly StringName ResolveProofDirectory = "ResolveProofDirectory";

		public static readonly StringName SetTowerDefenseManager = "SetTowerDefenseManager";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _previousManager = "_previousManager";

		public static readonly StringName _fogFeature = "_fogFeature";

		public static readonly StringName _testManager = "_testManager";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int MapColumns = 9;

	private const int MapRows = 5;

	private const int BeginColumn = 5;

	private const int ExtraColumns = 10;

	private const int ViewportWidth = 1100;

	private const int ViewportHeight = 700;

	private const int FadePhysicsFrames = 360;

	private int _checks;

	private int _failures;

	private TowerDefenseManager _previousManager;

	private TowerDefenseBattleFeatureFog _fogFeature;

	private TowerDefenseManager _testManager;

	public override async void _Ready()
	{
		string beforePath = string.Empty;
		string afterPath = string.Empty;
		int rightClearedPixels = 0;
		int bottomClearedPixels = 0;
		try
		{
			(beforePath, afterPath, rightClearedPixels, bottomClearedPixels) = await RunBoundaryScenario();
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[FogPlanternBoundaryRuntimeTest] 未处理异常：{value}");
		}
		finally
		{
			SetTowerDefenseManager(_previousManager);
			if (GodotObject.IsInstanceValid(_testManager))
			{
				_testManager.Free();
			}
		}
		bool flag = _failures == 0;
		GD.Print($"FOG_PLANTERN_BOUNDARY_RESULT passed={flag} checks={_checks} failures={_failures} rightClearedPixels={rightClearedPixels} bottomClearedPixels={bottomClearedPixels} before={beforePath} after={afterPath} renderer={RenderingServer.GetCurrentRenderingMethod()}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task<(string BeforePath, string AfterPath, int RightClearedPixels, int BottomClearedPixels)> RunBoundaryScenario()
	{
		_previousManager = TowerDefenseManager.Instance;
		_testManager = new TowerDefenseManager
		{
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(80f, 98f),
			gridNum = new Vector2I(9, 5)
		};
		SetTowerDefenseManager(_testManager);
		ColorRect node = new ColorRect
		{
			Color = new Color(0.015f, 0.03f, 0.07f),
			Size = new Vector2(1100f, 700f),
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
		AddChild(node, forceReadableName: false, InternalMode.Disabled);
		_fogFeature = CreateFogFeature();
		AddChild(_fogFeature.fogNode, forceReadableName: false, InternalMode.Disabled);
		_fogFeature.fogNode.AddChild(_fogFeature.fogBatch, forceReadableName: false, InternalMode.Disabled);
		await _fogFeature.GameInit();
		_fogFeature.fogNode.GlobalPosition = Vector2.Zero;
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		TowerDefenseFog rightBoundaryFog = FindFog(12, 5);
		TowerDefenseFog bottomBoundaryFog = FindFog(9, 6);
		TowerDefenseFog cornerBoundaryFog = FindFog(12, 6);
		TowerDefenseFog towerDefenseFog = FindFog(13, 5);
		Check(GodotObject.IsInstanceValid(rightBoundaryFog) && rightBoundaryFog.lightOverlapEnabled, "地图右侧第 3 个外扩雾列必须参与路灯花清雾检测。");
		Check(GodotObject.IsInstanceValid(bottomBoundaryFog) && bottomBoundaryFog.lightOverlapEnabled, "地图底部第 1 个外扩雾行必须参与路灯花清雾检测。");
		Check(GodotObject.IsInstanceValid(cornerBoundaryFog) && cornerBoundaryFog.lightOverlapEnabled, "右下角外扩雾块必须同时参与路灯花清雾检测。");
		Check(GodotObject.IsInstanceValid(towerDefenseFog) && !towerDefenseFog.lightOverlapEnabled, "右侧再外一列必须保持纯视觉雾，避免扩大物理更新范围。");
		Check(_fogFeature.fogBatch.ActiveCount == 48, $"9x5 地图应仅批量管理 48 个边界内外扩雾块，实际为 {_fogFeature.fogBatch.ActiveCount}。");
		CharacterAabbAreaComponentDefinition characterAabbAreaComponentDefinition = GD.Load<CharacterAabbAreaComponentDefinition>("res://Asset/Anime/Character/Plant/Chapter0/Plantern/Scene/TowerDefensePlanternFogAreaComponentDefinition.tres");
		CircleShape2D planternFogShape = ((characterAabbAreaComponentDefinition != null && characterAabbAreaComponentDefinition.shapeResources.Count > 0) ? (characterAabbAreaComponentDefinition.shapeResources[0].Geometry as CircleShape2D) : null);
		Check(GodotObject.IsInstanceValid(planternFogShape) && Mathf.IsEqualApprox(planternFogShape.Radius, 200f), "运行探针必须使用路灯花资源中半径 200 的真实清雾形状。");
		string path = ResolveProofDirectory();
		string beforePath = Path.Combine(path, "fog_plantern_boundary_before.png");
		string afterPath = Path.Combine(path, "fog_plantern_boundary_after.png");
		Image beforeImage = await CaptureFrame(beforePath);
		float radius = planternFogShape.Radius;
		Vector2 vector = new Vector2(700f, 470f);
		Rect2 planternLightRect = new Rect2(vector - Vector2.One * radius, Vector2.One * radius * 2f);
		ApplyPlanternLight(planternLightRect);
		Check(rightBoundaryFog.sprite.Modulate.A <= 0.001f, $"右侧外扩雾列应被路灯花清除，当前透明度为 {rightBoundaryFog.sprite.Modulate.A:F6}。");
		Check(bottomBoundaryFog.sprite.Modulate.A <= 0.001f, $"底部外扩雾行应被路灯花清除，当前透明度为 {bottomBoundaryFog.sprite.Modulate.A:F6}。");
		Check(cornerBoundaryFog.sprite.Modulate.A <= 0.001f, $"右下角外扩雾块应被路灯花清除，当前透明度为 {cornerBoundaryFog.sprite.Modulate.A:F6}。");
		Image afterImage = await CaptureFrame(afterPath);
		int num = CountClearedPixels(beforeImage, afterImage, new Rect2I(805, 300, 78, 280));
		int num2 = CountClearedPixels(beforeImage, afterImage, new Rect2I(520, 590, 280, 44));
		Check(num >= 100, $"右侧边缘必须出现真实清雾像素，实际减少亮度的像素数为 {num}。");
		Check(num2 >= 100, $"底部边缘必须出现真实清雾像素，实际减少亮度的像素数为 {num2}。");
		return (BeforePath: beforePath, AfterPath: afterPath, RightClearedPixels: num, BottomClearedPixels: num2);
	}

	private TowerDefenseBattleFeatureFog CreateFogFeature()
	{
		TowerDefenseBattleFeatureFog towerDefenseBattleFeatureFog = new TowerDefenseBattleFeatureFog
		{
			beginColumn = 5,
			mapGridNum = new Vector2I(9, 5),
			config = new TowerDefenseLevelFogManagerConfig
			{
				extraColumns = 10
			},
			fogNode = new Node2D
			{
				Name = "FogBoundaryRuntimeNode"
			},
			fogBatch = new TowerDefenseFogBatch
			{
				Name = "FogBoundaryRuntimeBatch",
				ProcessMode = ProcessModeEnum.Disabled
			}
		};
		int num = Math.Max(2, 7);
		for (int i = 0; i < num; i++)
		{
			towerDefenseBattleFeatureFog.fogLine.Add(new Array<TowerDefenseFog>());
		}
		return towerDefenseBattleFeatureFog;
	}

	private TowerDefenseFog FindFog(int column, int row)
	{
		if (row < 0 || row >= _fogFeature.fogLine.Count)
		{
			return null;
		}
		foreach (TowerDefenseFog item in _fogFeature.fogLine[row])
		{
			if (GodotObject.IsInstanceValid(item) && Mathf.IsEqualApprox(item.gridPos.X, column))
			{
				return item;
			}
		}
		return null;
	}

	private void ApplyPlanternLight(Rect2 planternLightRect)
	{
		List<AabbAreaLayerRegistry.WorldRectSnapshot> lightAreas = new List<AabbAreaLayerRegistry.WorldRectSnapshot>
		{
			new AabbAreaLayerRegistry.WorldRectSnapshot(planternLightRect)
		};
		for (int i = 0; i < 360; i++)
		{
			for (int j = 0; j < _fogFeature.fogLine.Count; j++)
			{
				foreach (TowerDefenseFog item in _fogFeature.fogLine[j])
				{
					if (GodotObject.IsInstanceValid(item) && item.lightOverlapEnabled)
					{
						item.BatchPhysicsUpdate(1.0 / 60.0, lightAreas);
					}
				}
			}
		}
	}

	private async Task<Image> CaptureFrame(string path)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Image image = GetViewport().GetTexture().GetImage();
		Error error = image.SavePng(path);
		Check(error == Error.Ok, $"边界清雾截图保存失败：{path}，错误码 {error}。");
		return image;
	}

	private int CountClearedPixels(Image beforeImage, Image afterImage, Rect2I region)
	{
		int num = 0;
		int num2 = Math.Min(region.End.X, Math.Min(beforeImage.GetWidth(), afterImage.GetWidth()));
		int num3 = Math.Min(region.End.Y, Math.Min(beforeImage.GetHeight(), afterImage.GetHeight()));
		for (int i = Math.Max(0, region.Position.Y); i < num3; i++)
		{
			for (int j = Math.Max(0, region.Position.X); j < num2; j++)
			{
				Color pixel = beforeImage.GetPixel(j, i);
				Color pixel2 = afterImage.GetPixel(j, i);
				float num4 = pixel.R * 0.2126f + pixel.G * 0.7152f + pixel.B * 0.0722f;
				float num5 = pixel2.R * 0.2126f + pixel2.G * 0.7152f + pixel2.B * 0.0722f;
				if (num4 - num5 >= 0.03f)
				{
					num++;
				}
			}
		}
		return num;
	}

	private string ResolveProofDirectory()
	{
		string environment = OS.GetEnvironment("FOG_BOUNDARY_PROOF_DIRECTORY");
		string text = (string.IsNullOrWhiteSpace(environment) ? ProjectSettings.GlobalizePath("user://fog-boundary-proof") : environment);
		Directory.CreateDirectory(text);
		return text;
	}

	private static void SetTowerDefenseManager(TowerDefenseManager manager)
	{
		typeof(TowerDefenseManager).GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(null, manager);
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[FogPlanternBoundaryRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(8)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateFogFeature, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.FindFog, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "row", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ApplyPlanternLight, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Rect2, "planternLightRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CountClearedPixels, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "beforeImage", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "afterImage", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Rect2I, "region", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ResolveProofDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetTowerDefenseManager, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateFogFeature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureFog>(CreateFogFeature());
			return true;
		}
		if (method == MethodName.FindFog && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseFog>(FindFog(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyPlanternLight && args.Count == 1)
		{
			ApplyPlanternLight(VariantUtils.ConvertTo<Rect2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountClearedPixels && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(CountClearedPixels(VariantUtils.ConvertTo<Image>(in args[0]), VariantUtils.ConvertTo<Image>(in args[1]), VariantUtils.ConvertTo<Rect2I>(in args[2])));
			return true;
		}
		if (method == MethodName.ResolveProofDirectory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveProofDirectory());
			return true;
		}
		if (method == MethodName.SetTowerDefenseManager && args.Count == 1)
		{
			SetTowerDefenseManager(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetTowerDefenseManager && args.Count == 1)
		{
			SetTowerDefenseManager(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]));
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
		if (method == MethodName.CreateFogFeature)
		{
			return true;
		}
		if (method == MethodName.FindFog)
		{
			return true;
		}
		if (method == MethodName.ApplyPlanternLight)
		{
			return true;
		}
		if (method == MethodName.CountClearedPixels)
		{
			return true;
		}
		if (method == MethodName.ResolveProofDirectory)
		{
			return true;
		}
		if (method == MethodName.SetTowerDefenseManager)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._previousManager)
		{
			_previousManager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		if (name == PropertyName._fogFeature)
		{
			_fogFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureFog>(in value);
			return true;
		}
		if (name == PropertyName._testManager)
		{
			_testManager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._previousManager)
		{
			value = VariantUtils.CreateFrom(in _previousManager);
			return true;
		}
		if (name == PropertyName._fogFeature)
		{
			value = VariantUtils.CreateFrom(in _fogFeature);
			return true;
		}
		if (name == PropertyName._testManager)
		{
			value = VariantUtils.CreateFrom(in _testManager);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._fogFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._testManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._previousManager, Variant.From(in _previousManager));
		info.AddProperty(PropertyName._fogFeature, Variant.From(in _fogFeature));
		info.AddProperty(PropertyName._testManager, Variant.From(in _testManager));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._previousManager, out var value3))
		{
			_previousManager = value3.As<TowerDefenseManager>();
		}
		if (info.TryGetProperty(PropertyName._fogFeature, out var value4))
		{
			_fogFeature = value4.As<TowerDefenseBattleFeatureFog>();
		}
		if (info.TryGetProperty(PropertyName._testManager, out var value5))
		{
			_testManager = value5.As<TowerDefenseManager>();
		}
	}
}
