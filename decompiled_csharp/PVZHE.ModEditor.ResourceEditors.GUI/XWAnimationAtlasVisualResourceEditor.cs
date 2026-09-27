using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationAtlasVisualResourceEditor.cs")]
public class XWAnimationAtlasVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public static readonly StringName ResetManifestState = "ResetManifestState";

		public static readonly StringName CreateWorkbenchPanel = "CreateWorkbenchPanel";

		public static readonly StringName ReloadCurrentAtlasProfile = "ReloadCurrentAtlasProfile";

		public static readonly StringName AddProfileOverview = "AddProfileOverview";

		public static readonly StringName CreateBadge = "CreateBadge";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName UpdateStatus = "UpdateStatus";

		public static readonly StringName ResourcePathExists = "ResourcePathExists";

		public static readonly StringName CountAtlasPages = "CountAtlasPages";

		public static readonly StringName AddManifestSummary = "AddManifestSummary";

		public static readonly StringName CreateAtlasOverviewImage = "CreateAtlasOverviewImage";

		public static readonly StringName AddPageList = "AddPageList";

		public static readonly StringName AddSourceEntryList = "AddSourceEntryList";

		public static readonly StringName CreateSection = "CreateSection";

		public static readonly StringName AddMetric = "AddMetric";

		public static readonly StringName AddEmptyState = "AddEmptyState";

		public static readonly StringName EmptyValue = "EmptyValue";

		public static readonly StringName ShortPath = "ShortPath";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName ManifestLoaded = "ManifestLoaded";

		public static readonly StringName ManifestSourceCount = "ManifestSourceCount";

		public static readonly StringName ManifestPageCount = "ManifestPageCount";

		public static readonly StringName ManifestErrorCount = "ManifestErrorCount";

		public static readonly StringName ManifestStatusText = "ManifestStatusText";

		public static readonly StringName ActiveManifest = "ActiveManifest";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const int MaximumVisibleEntries = 256;

	private const int MaximumVisiblePages = 128;

	private const int MaximumVisibleErrors = 128;

	private const long MaximumManifestBytes = 4194304L;

	public bool ManifestLoaded { get; private set; }

	public int ManifestSourceCount { get; private set; }

	public int ManifestPageCount { get; private set; }

	public int ManifestErrorCount { get; private set; }

	public string ManifestStatusText { get; private set; } = "";

	public AdobeAnimateGlobalAtlasManifest ActiveManifest { get; private set; }

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		ResetManifestState();
		if (!GodotObject.IsInstanceValid(CanvasGrid))
		{
			return;
		}
		CanvasGrid.Columns = 1;
		PanelContainer panelContainer = CreateWorkbenchPanel();
		CanvasGrid.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer node = panelContainer.GetNode<VBoxContainer>("Margin/Content");
		if (!(CurrentResource is AdobeAnimateAtlasProfile adobeAnimateAtlasProfile))
		{
			SetStatus(node, "错误：当前资源不是动画图集配置。", error: true);
			ManifestErrorCount = 1;
			return;
		}
		AddProfileOverview(node, adobeAnimateAtlasProfile);
		if (string.IsNullOrWhiteSpace(adobeAnimateAtlasProfile.ManifestPath))
		{
			SetStatus(node, "等待清单：请在下方直接属性拼图中选择动画图集清单。", error: false);
			AddEmptyState(node, "尚未绑定清单", "绑定 .tres 清单后，这里会显示图集页、动画条目和数据完整性。");
			return;
		}
		List<string> list = new List<string>();
		AdobeAnimateGlobalAtlasManifest adobeAnimateGlobalAtlasManifest = LoadManifest(adobeAnimateAtlasProfile, list);
		if (!GodotObject.IsInstanceValid(adobeAnimateGlobalAtlasManifest))
		{
			ManifestErrorCount = Math.Max(1, list.Count);
			SetStatus(node, "错误：无法加载图集清单 · " + adobeAnimateAtlasProfile.ManifestPath, error: true);
			AddErrorPanel(node, list);
			return;
		}
		ActiveManifest = adobeAnimateGlobalAtlasManifest;
		ManifestLoaded = true;
		ManifestSourceCount = adobeAnimateGlobalAtlasManifest.SourceKeys?.Count ?? 0;
		ValidateManifest(adobeAnimateGlobalAtlasManifest, list);
		ManifestErrorCount = list.Count;
		ManifestPageCount = CountAtlasPages(adobeAnimateGlobalAtlasManifest);
		SetStatus(node, (list.Count == 0) ? $"清单已就绪 · {ManifestSourceCount} 个动画来源 · {ManifestPageCount} 个图集页" : $"清单已加载，但发现 {list.Count} 个问题", list.Count > 0);
		AddManifestSummary(node, adobeAnimateAtlasProfile, adobeAnimateGlobalAtlasManifest);
		AddAtlasPreview(node, adobeAnimateGlobalAtlasManifest, list);
		AddPageList(node, adobeAnimateGlobalAtlasManifest);
		AddSourceEntryList(node, adobeAnimateGlobalAtlasManifest);
		if (ManifestErrorCount != list.Count)
		{
			ManifestErrorCount = list.Count;
			UpdateStatus(node.GetNodeOrNull<Label>("AtlasManifestStatus"), (list.Count == 0) ? $"清单已就绪 · {ManifestSourceCount} 个动画来源 · {ManifestPageCount} 个图集页" : $"清单已加载，但发现 {list.Count} 个问题", list.Count > 0);
		}
		if (list.Count > 0)
		{
			AddErrorPanel(node, list);
		}
	}

	private void ResetManifestState()
	{
		ManifestLoaded = false;
		ManifestSourceCount = 0;
		ManifestPageCount = 0;
		ManifestErrorCount = 0;
		ManifestStatusText = "";
		ActiveManifest = null;
	}

	private PanelContainer CreateWorkbenchPanel()
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.Name = "AnimationAtlasWorkbench";
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", new StyleBoxFlat
		{
			BgColor = new Color(0.025f, 0.045f, 0.075f, 0.98f),
			BorderColor = new Color(0.24f, 0.58f, 0.86f, 0.92f),
			BorderWidthLeft = 2,
			BorderWidthTop = 2,
			BorderWidthRight = 2,
			BorderWidthBottom = 2,
			CornerRadiusTopLeft = 12,
			CornerRadiusTopRight = 12,
			CornerRadiusBottomLeft = 12,
			CornerRadiusBottomRight = 12
		});
		MarginContainer marginContainer = new MarginContainer
		{
			Name = "Margin"
		};
		marginContainer.AddThemeConstantOverride("margin_left", 16);
		marginContainer.AddThemeConstantOverride("margin_top", 14);
		marginContainer.AddThemeConstantOverride("margin_right", 16);
		marginContainer.AddThemeConstantOverride("margin_bottom", 16);
		panelContainer.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "Content",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 12);
		marginContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = "▦ 动画图集运行视图",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		label.AddThemeFontSizeOverride("font_size", 21);
		label.AddThemeColorOverride("font_color", new Color(0.66f, 0.87f, 1f));
		hBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Name = "ReloadManifestButton",
			Text = "重新读取清单",
			TooltipText = "重新读取并检查当前图集清单"
		};
		hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		button.Pressed += ReloadCurrentAtlasProfile;
		return panelContainer;
	}

	public void ReloadCurrentAtlasProfile()
	{
		if (GodotObject.IsInstanceValid(CurrentResource))
		{
			LoadResource(CurrentResource, CurrentResourcePath, CurrentDescriptor, CurrentEditContext);
		}
	}

	private void AddProfileOverview(VBoxContainer content, AdobeAnimateAtlasProfile profile)
	{
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			Name = "AtlasProfileOverview",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddThemeConstantOverride("h_separation", 10);
		hFlowContainer.AddThemeConstantOverride("v_separation", 8);
		content.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		hFlowContainer.AddChild(CreateBadge("配置标识", EmptyValue(profile.ProfileId), new Color(0.25f, 0.63f, 0.92f)), forceReadableName: false, InternalMode.Disabled);
		hFlowContainer.AddChild(CreateBadge("预载策略", profile.StartupOnly ? "仅启动阶段" : "按需加载", new Color(0.55f, 0.76f, 0.32f)), forceReadableName: false, InternalMode.Disabled);
		hFlowContainer.AddChild(CreateBadge("清单", EmptyValue(profile.ManifestPath), new Color(0.84f, 0.57f, 0.22f), 420f), forceReadableName: false, InternalMode.Disabled);
	}

	private static PanelContainer CreateBadge(string label, string value, Color accent, float width = 220f)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.CustomMinimumSize = new Vector2(width, 64f);
		panelContainer.AddThemeStyleboxOverride("panel", new StyleBoxFlat
		{
			BgColor = new Color(accent.R * 0.13f, accent.G * 0.13f, accent.B * 0.13f, 0.96f),
			BorderColor = new Color(accent.R, accent.G, accent.B, 0.72f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = 8,
			CornerRadiusTopRight = 8,
			CornerRadiusBottomLeft = 8,
			CornerRadiusBottomRight = 8
		});
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddThemeConstantOverride("separation", 2);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label2 = new Label
		{
			Text = label
		};
		label2.AddThemeColorOverride("font_color", new Color(accent.R, accent.G, accent.B));
		vBoxContainer.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		Label label3 = new Label
		{
			Text = value,
			TooltipText = value,
			TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis
		};
		label3.AddThemeFontSizeOverride("font_size", 16);
		vBoxContainer.AddChild(label3, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private void SetStatus(VBoxContainer content, string text, bool error)
	{
		ManifestStatusText = text ?? "";
		Label label = new Label
		{
			Name = "AtlasManifestStatus",
			Text = ManifestStatusText,
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		UpdateStatus(label, ManifestStatusText, error);
		content.AddChild(label, forceReadableName: false, InternalMode.Disabled);
	}

	private void UpdateStatus(Label status, string text, bool error)
	{
		ManifestStatusText = text ?? "";
		if (GodotObject.IsInstanceValid(status))
		{
			status.Text = ManifestStatusText;
			status.AddThemeColorOverride("font_color", error ? new Color(1f, 0.48f, 0.42f) : new Color(0.65f, 0.95f, 0.7f));
			status.AddThemeFontSizeOverride("font_size", 17);
		}
	}

	private static AdobeAnimateGlobalAtlasManifest LoadManifest(AdobeAnimateAtlasProfile profile, List<string> errors)
	{
		if (string.IsNullOrWhiteSpace(profile?.ManifestPath) || !ResourcePathExists(profile.ManifestPath))
		{
			errors.Add("清单资源不存在：" + profile?.ManifestPath);
			return null;
		}
		try
		{
			string text = ProjectSettings.GlobalizePath(profile.ManifestPath);
			if (File.Exists(text) && new FileInfo(text).Length > 4194304)
			{
				errors.Add($"清单超过 {4L} MB 安全上限。");
				return null;
			}
		}
		catch (Exception ex)
		{
			errors.Add("清单大小检查失败：" + ex.GetBaseException().Message);
			return null;
		}
		try
		{
			AdobeAnimateGlobalAtlasManifest adobeAnimateGlobalAtlasManifest = profile.LoadManifest();
			if (!GodotObject.IsInstanceValid(adobeAnimateGlobalAtlasManifest))
			{
				errors.Add("清单资源无法读取：" + profile.ManifestPath);
			}
			return adobeAnimateGlobalAtlasManifest;
		}
		catch (Exception ex2)
		{
			errors.Add("清单加载异常：" + ex2.GetBaseException().Message);
			return null;
		}
	}

	private static void ValidateManifest(AdobeAnimateGlobalAtlasManifest manifest, List<string> errors)
	{
		int num = manifest.SourceKeys?.Count ?? 0;
		RequireParallelCount(errors, "媒体起点", num, manifest.SourceStarts?.Count ?? 0);
		RequireParallelCount(errors, "媒体数量", num, manifest.SourceCounts?.Count ?? 0);
		RequireOptionalParallelCount(errors, "来源签名", num, manifest.SourceSignatures?.Count ?? 0);
		RequireOptionalParallelCount(errors, "姿态页", num, manifest.SourcePoseAtlasPages?.Count ?? 0);
		RequireOptionalParallelCount(errors, "姿态起点", num, manifest.SourcePoseTexelStarts?.Count ?? 0);
		RequireOptionalParallelCount(errors, "姿态数量", num, manifest.SourcePoseTexelCounts?.Count ?? 0);
		RequireOptionalParallelCount(errors, "姿态签名", num, manifest.SourcePoseSignatures?.Count ?? 0);
		int expected = manifest.MediaRects?.Count ?? 0;
		RequireParallelCount(errors, "媒体页索引", expected, manifest.MediaAtlasPages?.Count ?? 0);
		int expected2 = manifest.ExternalTexturePaths?.Count ?? 0;
		RequireOptionalParallelCount(errors, "外部贴图页索引", expected2, manifest.ExternalTextureAtlasPages?.Count ?? 0);
		RequireOptionalParallelCount(errors, "外部贴图矩形", expected2, manifest.ExternalTextureRects?.Count ?? 0);
		if (string.IsNullOrWhiteSpace(manifest.AtlasTextureArrayPath))
		{
			errors.Add("缺少视觉图集数组路径。");
		}
		else if (!ResourcePathExists(manifest.AtlasTextureArrayPath))
		{
			errors.Add("视觉图集数组不存在：" + manifest.AtlasTextureArrayPath);
		}
		if (manifest.AtlasTextureArrayLayerCount <= 0)
		{
			errors.Add("视觉图集页数必须大于 0。");
		}
		for (int i = 0; i < Math.Min(num, manifest.SourceCounts?.Count ?? 0); i++)
		{
			if (manifest.SourceCounts[i] < 0)
			{
				errors.Add($"来源 #{i + 1} 的媒体数量不能为负数。");
			}
			if (i < (manifest.SourceStarts?.Count ?? 0) && manifest.SourceStarts[i] < 0)
			{
				errors.Add($"来源 #{i + 1} 的媒体起点不能为负数。");
			}
		}
	}

	private static bool ResourcePathExists(string resourcePath)
	{
		if (string.IsNullOrWhiteSpace(resourcePath))
		{
			return false;
		}
		if (ResourceLoader.Exists(resourcePath))
		{
			return true;
		}
		try
		{
			string text = ProjectSettings.GlobalizePath(resourcePath);
			return !string.IsNullOrWhiteSpace(text) && File.Exists(text);
		}
		catch
		{
			return false;
		}
	}

	private static void RequireParallelCount(List<string> errors, string label, int expected, int actual)
	{
		if (actual != expected)
		{
			errors.Add($"{label}数组长度不匹配：需要 {expected}，实际 {actual}。");
		}
	}

	private static void RequireOptionalParallelCount(List<string> errors, string label, int expected, int actual)
	{
		if (actual != 0 && actual != expected)
		{
			errors.Add($"{label}数组长度不匹配：需要 0 或 {expected}，实际 {actual}。");
		}
	}

	private static int CountAtlasPages(AdobeAnimateGlobalAtlasManifest manifest)
	{
		int num = Math.Max(0, manifest.AtlasTextureArrayLayerCount);
		if ((manifest.MediaAtlasPages?.Count ?? 0) > 0)
		{
			num = Math.Max(num, manifest.MediaAtlasPages.Max() + 1);
		}
		int num2 = Math.Max(Math.Max(0, manifest.PoseTextureArrayLayerCount), manifest.PoseAtlasPagePaths?.Count ?? 0);
		return num + num2;
	}

	private static void AddManifestSummary(VBoxContainer content, AdobeAnimateAtlasProfile profile, AdobeAnimateGlobalAtlasManifest manifest)
	{
		PanelContainer panelContainer = CreateSection("清单总览", "运行时将按这些尺寸、页数和来源范围读取动画图集。");
		content.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		GridContainer node = panelContainer.GetNode<GridContainer>("Body/Grid");
		AddMetric(node, "动画来源", (manifest.SourceKeys?.Count ?? 0).ToString());
		AddMetric(node, "媒体切片", (manifest.MediaRects?.Count ?? 0).ToString());
		AddMetric(node, "视觉页", Math.Max(0, manifest.AtlasTextureArrayLayerCount).ToString());
		AddMetric(node, "姿态页", Math.Max(0, manifest.PoseTextureArrayLayerCount).ToString());
		AddMetric(node, "视觉页尺寸", manifest.AtlasTextureArrayLayerSize.ToString());
		AddMetric(node, "姿态页尺寸", manifest.PoseTextureArrayLayerSize.ToString());
		AddMetric(node, "启动预载", profile.StartupOnly ? "开启" : "关闭");
		AddMetric(node, "外部贴图", (manifest.ExternalTexturePaths?.Count ?? 0).ToString());
	}

	private static void AddAtlasPreview(VBoxContainer content, AdobeAnimateGlobalAtlasManifest manifest, List<string> errors)
	{
		PanelContainer panelContainer = CreateSection("视觉图集预览", "用轻量页阵列展示图集结构；不在主线程读取大型纹理或回读 GPU 图层。");
		content.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer node = panelContainer.GetNode<VBoxContainer>("Body");
		TextureRect textureRect = new TextureRect
		{
			Name = "AtlasPreviewTexture",
			CustomMinimumSize = new Vector2(640f, 240f),
			ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
			StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
			TextureFilter = TextureFilterEnum.Nearest,
			TooltipText = (manifest.AtlasTextureArrayPath ?? "")
		};
		node.AddChild(textureRect, forceReadableName: false, InternalMode.Disabled);
		try
		{
			using Image image = CreateAtlasOverviewImage(manifest);
			textureRect.Texture = ImageTexture.CreateFromImage(image);
		}
		catch (Exception ex)
		{
			errors.Add("图集结构预览生成失败：" + ex.GetBaseException().Message);
		}
	}

	private static Image CreateAtlasOverviewImage(AdobeAnimateGlobalAtlasManifest manifest)
	{
		Image image = Image.CreateEmpty(640, 240, useMipmaps: false, Image.Format.Rgba8);
		image.Fill(new Color(0.025f, 0.045f, 0.075f));
		int num = Math.Min(Math.Max(1, manifest?.AtlasTextureArrayLayerCount ?? 0), 16);
		int num2 = (num + 4 - 1) / 4;
		int num3 = 147;
		int num4 = (240 - 10 * (num2 + 1)) / Math.Max(1, num2);
		for (int i = 0; i < num; i++)
		{
			int num5 = i % 4;
			int num6 = i / 4;
			Rect2I rect = new Rect2I(10 + num5 * (num3 + 10), 10 + num6 * (num4 + 10), num3, num4);
			float hue = (0.54f + (float)i * 0.045f) % 1f;
			image.FillRect(rect, Color.FromHsv(hue, 0.55f, 0.7f));
			image.FillRect(new Rect2I(rect.Position + new Vector2I(6, 6), rect.Size - new Vector2I(12, 12)), Color.FromHsv(hue, 0.42f, 0.27f));
		}
		return image;
	}

	private static void AddPageList(VBoxContainer content, AdobeAnimateGlobalAtlasManifest manifest)
	{
		PanelContainer panelContainer = CreateSection("图集页", "视觉数组、姿态数组和姿态页文件的运行时入口。");
		content.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer node = panelContainer.GetNode<VBoxContainer>("Body");
		ItemList itemList = new ItemList
		{
			Name = "AtlasPageList",
			CustomMinimumSize = new Vector2(0f, 150f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			AllowReselect = true
		};
		node.AddChild(itemList, forceReadableName: false, InternalMode.Disabled);
		List<string> list = new List<string>();
		if (!string.IsNullOrWhiteSpace(manifest.AtlasTextureArrayPath))
		{
			list.Add($"视觉数组 · {manifest.AtlasTextureArrayLayerCount} 层 · {manifest.AtlasTextureArrayPath}");
		}
		if (!string.IsNullOrWhiteSpace(manifest.PoseTextureArrayPath))
		{
			list.Add($"姿态数组 · {manifest.PoseTextureArrayLayerCount} 层 · {manifest.PoseTextureArrayPath}");
		}
		for (int i = 0; i < (manifest.PoseAtlasPagePaths?.Count ?? 0); i++)
		{
			list.Add($"姿态页 {i + 1} · {manifest.PoseAtlasPagePaths[i]}");
		}
		for (int j = 0; j < Math.Min(list.Count, 128); j++)
		{
			itemList.AddItem(list[j]);
			itemList.SetItemTooltip(j, list[j]);
		}
		if (list.Count > 128)
		{
			itemList.AddItem($"……还有 {list.Count - 128} 个页面未展开");
		}
		if (list.Count == 0)
		{
			itemList.AddItem("清单未声明任何图集页路径");
		}
	}

	private static void AddSourceEntryList(VBoxContainer content, AdobeAnimateGlobalAtlasManifest manifest)
	{
		int num = manifest.SourceKeys?.Count ?? 0;
		PanelContainer panelContainer = CreateSection($"动画条目 · {num}", $"为避免大型清单阻塞主线程，画面最多展开前 {256} 条，完整数量始终保留在标题中。");
		content.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer node = panelContainer.GetNode<VBoxContainer>("Body");
		ItemList itemList = new ItemList
		{
			Name = "AtlasEntryList",
			CustomMinimumSize = new Vector2(0f, 260f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			AllowReselect = true
		};
		node.AddChild(itemList, forceReadableName: false, InternalMode.Disabled);
		for (int i = 0; i < Math.Min(num, 256); i++)
		{
			string text = manifest.SourceKeys[i];
			int value = ((i < (manifest.SourceStarts?.Count ?? 0)) ? manifest.SourceStarts[i] : (-1));
			int value2 = ((i < (manifest.SourceCounts?.Count ?? 0)) ? manifest.SourceCounts[i] : (-1));
			int num2 = ((i < (manifest.SourcePoseAtlasPages?.Count ?? 0)) ? manifest.SourcePoseAtlasPages[i] : (-1));
			string text2 = $"{i + 1:000}  {ShortPath(text)}  · 媒体 {value} + {value2}" + ((num2 >= 0) ? $" · 姿态页 {num2 + 1}" : "");
			itemList.AddItem(text2);
			itemList.SetItemTooltip(i, text);
		}
		if (num > 256)
		{
			itemList.AddItem($"……还有 {num - 256} 个动画来源未展开");
		}
		if (num == 0)
		{
			itemList.AddItem("清单中没有动画来源条目");
		}
	}

	private static PanelContainer CreateSection(string title, string description)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", new StyleBoxFlat
		{
			BgColor = new Color(0.045f, 0.075f, 0.115f, 0.96f),
			BorderColor = new Color(0.18f, 0.36f, 0.53f, 0.85f),
			BorderWidthLeft = 1,
			BorderWidthTop = 1,
			BorderWidthRight = 1,
			BorderWidthBottom = 1,
			CornerRadiusTopLeft = 9,
			CornerRadiusTopRight = 9,
			CornerRadiusBottomLeft = 9,
			CornerRadiusBottomRight = 9
		});
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "Body",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Label label = new Label
		{
			Text = title
		};
		label.AddThemeFontSizeOverride("font_size", 18);
		label.AddThemeColorOverride("font_color", new Color(0.71f, 0.88f, 1f));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		Label label2 = new Label
		{
			Text = description,
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		label2.AddThemeColorOverride("font_color", new Color(0.68f, 0.74f, 0.82f));
		vBoxContainer.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		GridContainer gridContainer = new GridContainer
		{
			Name = "Grid",
			Columns = 4,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		gridContainer.AddThemeConstantOverride("h_separation", 14);
		gridContainer.AddThemeConstantOverride("v_separation", 8);
		vBoxContainer.AddChild(gridContainer, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private static void AddMetric(GridContainer grid, string label, string value)
	{
		Label label2 = new Label
		{
			Text = label
		};
		label2.AddThemeColorOverride("font_color", new Color(0.55f, 0.68f, 0.78f));
		grid.AddChild(label2, forceReadableName: false, InternalMode.Disabled);
		Label label3 = new Label
		{
			Text = value,
			TooltipText = value
		};
		label3.AddThemeColorOverride("font_color", new Color(0.91f, 0.96f, 1f));
		label3.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
		grid.AddChild(label3, forceReadableName: false, InternalMode.Disabled);
	}

	private static void AddEmptyState(VBoxContainer content, string title, string detail)
	{
		PanelContainer node = CreateSection(title, detail);
		content.AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	private static void AddErrorPanel(VBoxContainer content, IReadOnlyList<string> errors)
	{
		if (errors != null && errors.Count != 0)
		{
			PanelContainer panelContainer = CreateSection($"检查结果 · {errors.Count} 个问题", "修复后点击“重新读取清单”再次检查。");
			content.AddChild(panelContainer, forceReadableName: false, InternalMode.Disabled);
			VBoxContainer node = panelContainer.GetNode<VBoxContainer>("Body");
			ItemList itemList = new ItemList
			{
				Name = "AtlasManifestErrors",
				CustomMinimumSize = new Vector2(0f, Math.Min(220, 38 + errors.Count * 28)),
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			node.AddChild(itemList, forceReadableName: false, InternalMode.Disabled);
			for (int i = 0; i < Math.Min(errors.Count, 128); i++)
			{
				itemList.AddItem("⚠ " + errors[i]);
				itemList.SetItemTooltip(i, errors[i]);
			}
			if (errors.Count > 128)
			{
				itemList.AddItem($"……还有 {errors.Count - 128} 个问题未展开");
			}
		}
	}

	private static string EmptyValue(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value.Trim();
		}
		return "未设置";
	}

	private static string ShortPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "未命名来源";
		}
		string text = path.Replace('\\', '/');
		string fileName = Path.GetFileName(text);
		if (!string.IsNullOrWhiteSpace(fileName))
		{
			return fileName;
		}
		return text;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName.ResetManifestState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWorkbenchPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReloadCurrentAtlasProfile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddProfileOverview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "profile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateBadge, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "accent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "status", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResourcePathExists, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountAtlasPages, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manifest", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddManifestSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "profile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "manifest", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAtlasOverviewImage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manifest", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddPageList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "manifest", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddSourceEntryList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "manifest", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSection, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "description", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddMetric, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "grid", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GridContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddEmptyState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "content", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "detail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShortPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResetManifestState && args.Count == 0)
		{
			ResetManifestState();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateWorkbenchPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreateWorkbenchPanel());
			return true;
		}
		if (method == MethodName.ReloadCurrentAtlasProfile && args.Count == 0)
		{
			ReloadCurrentAtlasProfile();
			ret = default;
			return true;
		}
		if (method == MethodName.AddProfileOverview && args.Count == 2)
		{
			AddProfileOverview(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateAtlasProfile>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBadge && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreateBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.SetStatus && args.Count == 3)
		{
			SetStatus(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateStatus && args.Count == 3)
		{
			UpdateStatus(VariantUtils.ConvertTo<Label>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResourcePathExists && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ResourcePathExists(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CountAtlasPages && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountAtlasPages(VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[0])));
			return true;
		}
		if (method == MethodName.AddManifestSummary && args.Count == 3)
		{
			AddManifestSummary(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateAtlasProfile>(in args[1]), VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAtlasOverviewImage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Image>(CreateAtlasOverviewImage(VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[0])));
			return true;
		}
		if (method == MethodName.AddPageList && args.Count == 2)
		{
			AddPageList(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSourceEntryList && args.Count == 2)
		{
			AddSourceEntryList(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreateSection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddMetric && args.Count == 3)
		{
			AddMetric(VariantUtils.ConvertTo<GridContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddEmptyState && args.Count == 3)
		{
			AddEmptyState(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmptyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShortPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ShortPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateBadge && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreateBadge(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.ResourcePathExists && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ResourcePathExists(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CountAtlasPages && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountAtlasPages(VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[0])));
			return true;
		}
		if (method == MethodName.AddManifestSummary && args.Count == 3)
		{
			AddManifestSummary(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateAtlasProfile>(in args[1]), VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAtlasOverviewImage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Image>(CreateAtlasOverviewImage(VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[0])));
			return true;
		}
		if (method == MethodName.AddPageList && args.Count == 2)
		{
			AddPageList(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSourceEntryList && args.Count == 2)
		{
			AddSourceEntryList(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PanelContainer>(CreateSection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddMetric && args.Count == 3)
		{
			AddMetric(VariantUtils.ConvertTo<GridContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddEmptyState && args.Count == 3)
		{
			AddEmptyState(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EmptyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShortPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ShortPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ResetManifestState)
		{
			return true;
		}
		if (method == MethodName.CreateWorkbenchPanel)
		{
			return true;
		}
		if (method == MethodName.ReloadCurrentAtlasProfile)
		{
			return true;
		}
		if (method == MethodName.AddProfileOverview)
		{
			return true;
		}
		if (method == MethodName.CreateBadge)
		{
			return true;
		}
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.UpdateStatus)
		{
			return true;
		}
		if (method == MethodName.ResourcePathExists)
		{
			return true;
		}
		if (method == MethodName.CountAtlasPages)
		{
			return true;
		}
		if (method == MethodName.AddManifestSummary)
		{
			return true;
		}
		if (method == MethodName.CreateAtlasOverviewImage)
		{
			return true;
		}
		if (method == MethodName.AddPageList)
		{
			return true;
		}
		if (method == MethodName.AddSourceEntryList)
		{
			return true;
		}
		if (method == MethodName.CreateSection)
		{
			return true;
		}
		if (method == MethodName.AddMetric)
		{
			return true;
		}
		if (method == MethodName.AddEmptyState)
		{
			return true;
		}
		if (method == MethodName.EmptyValue)
		{
			return true;
		}
		if (method == MethodName.ShortPath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ManifestLoaded)
		{
			ManifestLoaded = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ManifestSourceCount)
		{
			ManifestSourceCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ManifestPageCount)
		{
			ManifestPageCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ManifestErrorCount)
		{
			ManifestErrorCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ManifestStatusText)
		{
			ManifestStatusText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ActiveManifest)
		{
			ActiveManifest = VariantUtils.ConvertTo<AdobeAnimateGlobalAtlasManifest>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ManifestLoaded)
		{
			value = VariantUtils.CreateFrom<bool>(ManifestLoaded);
			return true;
		}
		int from;
		if (name == PropertyName.ManifestSourceCount)
		{
			from = ManifestSourceCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ManifestPageCount)
		{
			from = ManifestPageCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ManifestErrorCount)
		{
			from = ManifestErrorCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ManifestStatusText)
		{
			value = VariantUtils.CreateFrom<string>(ManifestStatusText);
			return true;
		}
		if (name == PropertyName.ActiveManifest)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateGlobalAtlasManifest>(ActiveManifest);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.ManifestLoaded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ManifestSourceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ManifestPageCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ManifestErrorCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ManifestStatusText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ActiveManifest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ManifestLoaded, Variant.From<bool>(ManifestLoaded));
		info.AddProperty(PropertyName.ManifestSourceCount, Variant.From<int>(ManifestSourceCount));
		info.AddProperty(PropertyName.ManifestPageCount, Variant.From<int>(ManifestPageCount));
		info.AddProperty(PropertyName.ManifestErrorCount, Variant.From<int>(ManifestErrorCount));
		info.AddProperty(PropertyName.ManifestStatusText, Variant.From<string>(ManifestStatusText));
		info.AddProperty(PropertyName.ActiveManifest, Variant.From<AdobeAnimateGlobalAtlasManifest>(ActiveManifest));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ManifestLoaded, out var value))
		{
			ManifestLoaded = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ManifestSourceCount, out var value2))
		{
			ManifestSourceCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ManifestPageCount, out var value3))
		{
			ManifestPageCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ManifestErrorCount, out var value4))
		{
			ManifestErrorCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ManifestStatusText, out var value5))
		{
			ManifestStatusText = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ActiveManifest, out var value6))
		{
			ActiveManifest = value6.As<AdobeAnimateGlobalAtlasManifest>();
		}
	}
}
