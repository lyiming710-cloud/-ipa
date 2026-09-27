using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/Resource/XWInspectorPropertyEditorResourceSummary.cs")]
public class XWInspectorPropertyEditorResourceSummary : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnResourceChanged = "OnResourceChanged";

		public static readonly StringName OnResourceSelected = "OnResourceSelected";

		public static readonly StringName OpenResourceInVisualEditor = "OpenResourceInVisualEditor";

		public static readonly StringName UpdatePreview = "UpdatePreview";

		public static readonly StringName BuildSummaryText = "BuildSummaryText";

		public static readonly StringName PopulateResourcePropertyTree = "PopulateResourcePropertyTree";

		public static readonly StringName AppendResourcePropertyRows = "AppendResourcePropertyRows";

		public static readonly StringName FormatPropertyName = "FormatPropertyName";

		public static readonly StringName FormatPropertyValue = "FormatPropertyValue";

		public static readonly StringName FormatVector = "FormatVector";

		public static readonly StringName FormatObject = "FormatObject";

		public static readonly StringName OpenSelectedPropertyResource = "OpenSelectedPropertyResource";

		public static readonly StringName GetPreviewTexture = "GetPreviewTexture";

		public static readonly StringName ReadEditedResource = "ReadEditedResource";

		public static readonly StringName ResolveBaseType = "ResolveBaseType";

		public static readonly StringName PlayAudioPreview = "PlayAudioPreview";

		public static readonly StringName StopAudioPreview = "StopAudioPreview";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _resourcePicker = "_resourcePicker";

		public static readonly StringName _previewPanel = "_previewPanel";

		public static readonly StringName _previewTexture = "_previewTexture";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _audioPreviewPlayer = "_audioPreviewPlayer";

		public static readonly StringName _playButton = "_playButton";

		public static readonly StringName _stopButton = "_stopButton";

		public static readonly StringName _openInspectorButton = "_openInspectorButton";

		public static readonly StringName _refreshButton = "_refreshButton";

		public static readonly StringName _resourcePropertyTree = "_resourcePropertyTree";

		public static readonly StringName _baseType = "_baseType";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/ResourceInspectorExtend/Resource/XWInspectorPropertyEditorResourceSummary.tscn";

	private XWResourcePicker _resourcePicker;

	private PanelContainer _previewPanel;

	private TextureRect _previewTexture;

	private Label _summaryLabel;

	private AudioStreamPlayer _audioPreviewPlayer;

	private Button _playButton;

	private Button _stopButton;

	private Button _openInspectorButton;

	private Button _refreshButton;

	private Tree _resourcePropertyTree;

	private string _baseType = "Resource";

	public static XWInspectorPropertyEditorResourceSummary Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/ResourceInspectorExtend/Resource/XWInspectorPropertyEditorResourceSummary.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorResourceSummary>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_resourcePicker = GetNode<XWResourcePicker>("%ResourcePicker");
		_previewPanel = GetNode<PanelContainer>("%PreviewPanel");
		_previewTexture = GetNode<TextureRect>("%PreviewTexture");
		_summaryLabel = GetNode<Label>("%SummaryLabel");
		_audioPreviewPlayer = GetNode<AudioStreamPlayer>("%AudioPreviewPlayer");
		_playButton = GetNode<Button>("%PlayButton");
		_stopButton = GetNode<Button>("%StopButton");
		_openInspectorButton = GetNode<Button>("%OpenInspectorButton");
		_refreshButton = GetNode<Button>("%RefreshButton");
		_resourcePropertyTree = GetNode<Tree>("%ResourcePropertyTree");
		_resourcePicker.ResourceChanged += OnResourceChanged;
		_resourcePicker.ResourceSelected += OnResourceSelected;
		_playButton.Pressed += PlayAudioPreview;
		_stopButton.Pressed += StopAudioPreview;
		_openInspectorButton.Pressed += () =>
		{
			OpenResourceInVisualEditor(ReadEditedResource());
		};
		_refreshButton.Pressed += UpdateValue;
		_resourcePropertyTree.ItemActivated += OpenSelectedPropertyResource;
		_resourcePropertyTree.SetColumnTitle(0, "属性");
		_resourcePropertyTree.SetColumnTitle(1, "值");
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		_baseType = ResolveBaseType(property);
		if (GodotObject.IsInstanceValid(_resourcePicker))
		{
			_resourcePicker.Setup(_baseType);
			UpdateValue();
		}
	}

	public override void UpdateValue()
	{
		Resource resource = ReadEditedResource();
		if (GodotObject.IsInstanceValid(_resourcePicker))
		{
			_resourcePicker.SetEditedResource(resource);
		}
		UpdatePreview(resource);
	}

	public override Variant GetValue()
	{
		return GetPropertyValue();
	}

	private void OnResourceChanged(Resource resource)
	{
		StopAudioPreview();
		ValueChange(resource);
		UpdatePreview(resource);
	}

	private void OnResourceSelected(Resource resource)
	{
		OpenResourceInVisualEditor(resource);
	}

	private void OpenResourceInVisualEditor(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface.Instance?.EditResource(resource);
		}
	}

	private void UpdatePreview(Resource resource)
	{
		if (IsNodeReady())
		{
			_summaryLabel.Text = BuildSummaryText(resource);
			Texture2D previewTexture = GetPreviewTexture(resource);
			_previewTexture.Texture = previewTexture;
			_previewTexture.Visible = GodotObject.IsInstanceValid(previewTexture);
			_previewPanel.Visible = GodotObject.IsInstanceValid(resource);
			bool flag = resource is AudioStream;
			_playButton.Visible = flag;
			_stopButton.Visible = flag;
			_openInspectorButton.Disabled = !GodotObject.IsInstanceValid(resource);
			_refreshButton.Disabled = !GodotObject.IsInstanceValid(resource);
			PopulateResourcePropertyTree(resource);
			if (!flag)
			{
				StopAudioPreview();
			}
		}
	}

	private string BuildSummaryText(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "空资源";
		}
		string value = resource.GetClass();
		string value2 = (string.IsNullOrWhiteSpace(resource.ResourceName) ? "未命名" : resource.ResourceName);
		string value3 = (string.IsNullOrWhiteSpace(resource.ResourcePath) ? "内置资源" : resource.ResourcePath);
		string text = $"类型: {value}\n名称: {value2}\n路径: {value3}";
		if (resource is AudioStream audioStream)
		{
			double length = audioStream.GetLength();
			if (length > 0.0)
			{
				text += $"\n时长: {length:0.00}s";
			}
		}
		else if (resource is Texture2D texture2D && XWTextureSafety.CanPreview(texture2D))
		{
			text += $"\n尺寸: {texture2D.GetWidth()} x {texture2D.GetHeight()}";
		}
		else if (resource is Font font)
		{
			text += $"\n字体高度: {font.GetHeight():0.##}";
		}
		else if (resource is PackedScene packedScene)
		{
			SceneState state = packedScene.GetState();
			int value4 = (GodotObject.IsInstanceValid(state) ? state.GetNodeCount() : 0);
			text += $"\n节点数: {value4}";
		}
		return text;
	}

	private void PopulateResourcePropertyTree(Resource resource)
	{
		if (GodotObject.IsInstanceValid(_resourcePropertyTree))
		{
			_resourcePropertyTree.Clear();
			TreeItem root = _resourcePropertyTree.CreateItem();
			if (GodotObject.IsInstanceValid(resource))
			{
				AppendResourcePropertyRows(resource, root);
			}
		}
	}

	private int AppendResourcePropertyRows(Resource resource, TreeItem root)
	{
		int num = 0;
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (!TryReadPropertyInfo(property, out var name, out var type, out var hint, out var hintString, out var usage) || (usage & (PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable)) == PropertyUsageFlags.None)
			{
				continue;
			}
			Variant value;
			try
			{
				value = resource.Get(new StringName(name));
			}
			catch
			{
				continue;
			}
			TreeItem treeItem = _resourcePropertyTree.CreateItem(root);
			treeItem.SetText(0, FormatPropertyName(name));
			treeItem.SetText(1, FormatPropertyValue(value));
			treeItem.SetTooltipText(0, $"{name}\n类型: {type}\nHint: {hint} {hintString}".StripEdges());
			treeItem.SetTooltipText(1, FormatPropertyValue(value));
			if (value.VariantType == Variant.Type.Object && value.As<GodotObject>() is Resource resource2)
			{
				treeItem.SetMetadata(0, resource2);
				Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(resource2.GetClass());
				if (GodotObject.IsInstanceValid(classIcon))
				{
					treeItem.SetIcon(0, classIcon);
				}
			}
			else
			{
				treeItem.SetMetadata(0, name);
			}
			num++;
		}
		if (num == 0)
		{
			TreeItem treeItem2 = _resourcePropertyTree.CreateItem(root);
			treeItem2.SetText(0, "没有可显示属性");
			treeItem2.SetText(1, "");
		}
		return num;
	}

	private static bool TryReadPropertyInfo(Dictionary property, out string name, out Variant.Type type, out PropertyHint hint, out string hintString, out PropertyUsageFlags usage)
	{
		name = "";
		type = Variant.Type.Nil;
		hint = PropertyHint.None;
		hintString = "";
		usage = PropertyUsageFlags.None;
		if (property == null || !property.TryGetValue("name", out var value))
		{
			return false;
		}
		name = value.AsString();
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		if (property.TryGetValue("type", out var value2))
		{
			type = (Variant.Type)value2.AsInt32();
		}
		if (property.TryGetValue("hint", out var value3))
		{
			hint = (PropertyHint)value3.AsInt32();
		}
		if (property.TryGetValue("hint_string", out var value4))
		{
			hintString = value4.AsString();
		}
		if (property.TryGetValue("usage", out var value5))
		{
			usage = (PropertyUsageFlags)value5.AsInt64();
		}
		return true;
	}

	private static string FormatPropertyName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return "";
		}
		return name.Replace('_', ' ');
	}

	private static string FormatPropertyValue(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		if ((ulong)variantType <= 28uL)
		{
			switch ((int)variantType)
			{
			case 0:
				return "空";
			case 1:
				return value.AsBool() ? "启用" : "关闭";
			case 2:
				return value.AsInt64().ToString();
			case 3:
				return value.AsDouble().ToString("0.###");
			case 4:
			case 21:
			case 22:
				return string.IsNullOrEmpty(value.AsString()) ? "空" : value.AsString();
			case 5:
				return FormatVector(value.AsVector2());
			case 6:
				return FormatVector(value.AsVector2I());
			case 9:
				return FormatVector(value.AsVector3());
			case 10:
				return FormatVector(value.AsVector3I());
			case 12:
				return FormatVector(value.AsVector4());
			case 13:
				return FormatVector(value.AsVector4I());
			case 20:
				return value.AsColor().ToHtml();
			case 28:
				return $"数组({value.AsGodotArray().Count})";
			case 27:
				return $"字典({value.AsGodotDictionary().Count})";
			case 24:
				return FormatObject(value.AsGodotObject());
			}
		}
		return value.ToString();
	}

	private static string FormatVector(Vector2 value)
	{
		return $"{value.X:0.###}, {value.Y:0.###}";
	}

	private static string FormatVector(Vector2I value)
	{
		return $"{value.X}, {value.Y}";
	}

	private static string FormatVector(Vector3 value)
	{
		return $"{value.X:0.###}, {value.Y:0.###}, {value.Z:0.###}";
	}

	private static string FormatVector(Vector3I value)
	{
		return $"{value.X}, {value.Y}, {value.Z}";
	}

	private static string FormatVector(Vector4 value)
	{
		return $"{value.X:0.###}, {value.Y:0.###}, {value.Z:0.###}, {value.W:0.###}";
	}

	private static string FormatVector(Vector4I value)
	{
		return $"{value.X}, {value.Y}, {value.Z}, {value.W}";
	}

	private static string FormatObject(GodotObject obj)
	{
		if (!GodotObject.IsInstanceValid(obj))
		{
			return "空";
		}
		if (obj is Resource resource)
		{
			string text = (string.IsNullOrWhiteSpace(resource.ResourcePath) ? "内置" : resource.ResourcePath.GetFile());
			return resource.GetClass() + " (" + text + ")";
		}
		return obj.GetClass();
	}

	private void OpenSelectedPropertyResource()
	{
		TreeItem selected = _resourcePropertyTree.GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<GodotObject>() is Resource resource)
			{
				OpenResourceInVisualEditor(resource);
			}
		}
	}

	private Texture2D GetPreviewTexture(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return null;
		}
		if (resource is Texture2D texture2D && XWTextureSafety.CanPreview(texture2D))
		{
			return texture2D;
		}
		if (resource is Gradient gradient)
		{
			return new GradientTexture1D
			{
				Gradient = gradient,
				Width = 128
			};
		}
		return null;
	}

	private Resource ReadEditedResource()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Object || !(propertyValue.As<GodotObject>() is Resource result))
		{
			return null;
		}
		return result;
	}

	private string ResolveBaseType(XWInspectorProperty property)
	{
		if (property == null)
		{
			return "Resource";
		}
		if (property.Hint == PropertyHint.ResourceType && !string.IsNullOrWhiteSpace(property.HintString))
		{
			return property.HintString;
		}
		if (!string.IsNullOrWhiteSpace(property.HintString))
		{
			return property.HintString;
		}
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType == Variant.Type.Object && propertyValue.As<GodotObject>() is Resource resource)
		{
			return resource.GetClass();
		}
		return "Resource";
	}

	private void PlayAudioPreview()
	{
		if (ReadEditedResource() is AudioStream stream)
		{
			_audioPreviewPlayer.Stop();
			_audioPreviewPlayer.Stream = stream;
			_audioPreviewPlayer.Play();
		}
	}

	private void StopAudioPreview()
	{
		if (GodotObject.IsInstanceValid(_audioPreviewPlayer))
		{
			_audioPreviewPlayer.Stop();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnResourceChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenResourceInVisualEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSummaryText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateResourcePropertyTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AppendResourcePropertyRows, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatPropertyName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatPropertyValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVector, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatObject, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenSelectedPropertyResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPreviewTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadEditedResource, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveBaseType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.PlayAudioPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopAudioPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorResourceSummary>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditProperty && args.Count == 2)
		{
			SetEditProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
			return true;
		}
		if (method == MethodName.OnResourceChanged && args.Count == 1)
		{
			OnResourceChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceSelected && args.Count == 1)
		{
			OnResourceSelected(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenResourceInVisualEditor && args.Count == 1)
		{
			OpenResourceInVisualEditor(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreview && args.Count == 1)
		{
			UpdatePreview(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSummaryText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummaryText(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.PopulateResourcePropertyTree && args.Count == 1)
		{
			PopulateResourcePropertyTree(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AppendResourcePropertyRows && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(AppendResourcePropertyRows(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		if (method == MethodName.FormatPropertyName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPropertyName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatPropertyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPropertyValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObject(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.OpenSelectedPropertyResource && args.Count == 0)
		{
			OpenSelectedPropertyResource();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPreviewTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetPreviewTexture(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadEditedResource && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Resource>(ReadEditedResource());
			return true;
		}
		if (method == MethodName.ResolveBaseType && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveBaseType(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0])));
			return true;
		}
		if (method == MethodName.PlayAudioPreview && args.Count == 0)
		{
			PlayAudioPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.StopAudioPreview && args.Count == 0)
		{
			StopAudioPreview();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorResourceSummary>(Create());
			return true;
		}
		if (method == MethodName.FormatPropertyName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPropertyName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatPropertyValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatPropertyValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatObject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatObject(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetEditProperty)
		{
			return true;
		}
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		if (method == MethodName.OnResourceChanged)
		{
			return true;
		}
		if (method == MethodName.OnResourceSelected)
		{
			return true;
		}
		if (method == MethodName.OpenResourceInVisualEditor)
		{
			return true;
		}
		if (method == MethodName.UpdatePreview)
		{
			return true;
		}
		if (method == MethodName.BuildSummaryText)
		{
			return true;
		}
		if (method == MethodName.PopulateResourcePropertyTree)
		{
			return true;
		}
		if (method == MethodName.AppendResourcePropertyRows)
		{
			return true;
		}
		if (method == MethodName.FormatPropertyName)
		{
			return true;
		}
		if (method == MethodName.FormatPropertyValue)
		{
			return true;
		}
		if (method == MethodName.FormatVector)
		{
			return true;
		}
		if (method == MethodName.FormatObject)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedPropertyResource)
		{
			return true;
		}
		if (method == MethodName.GetPreviewTexture)
		{
			return true;
		}
		if (method == MethodName.ReadEditedResource)
		{
			return true;
		}
		if (method == MethodName.ResolveBaseType)
		{
			return true;
		}
		if (method == MethodName.PlayAudioPreview)
		{
			return true;
		}
		if (method == MethodName.StopAudioPreview)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._resourcePicker)
		{
			_resourcePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._previewPanel)
		{
			_previewPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewTexture)
		{
			_previewTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._audioPreviewPlayer)
		{
			_audioPreviewPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._playButton)
		{
			_playButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stopButton)
		{
			_stopButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._openInspectorButton)
		{
			_openInspectorButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._refreshButton)
		{
			_refreshButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resourcePropertyTree)
		{
			_resourcePropertyTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			_baseType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._resourcePicker)
		{
			value = VariantUtils.CreateFrom(in _resourcePicker);
			return true;
		}
		if (name == PropertyName._previewPanel)
		{
			value = VariantUtils.CreateFrom(in _previewPanel);
			return true;
		}
		if (name == PropertyName._previewTexture)
		{
			value = VariantUtils.CreateFrom(in _previewTexture);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._audioPreviewPlayer)
		{
			value = VariantUtils.CreateFrom(in _audioPreviewPlayer);
			return true;
		}
		if (name == PropertyName._playButton)
		{
			value = VariantUtils.CreateFrom(in _playButton);
			return true;
		}
		if (name == PropertyName._stopButton)
		{
			value = VariantUtils.CreateFrom(in _stopButton);
			return true;
		}
		if (name == PropertyName._openInspectorButton)
		{
			value = VariantUtils.CreateFrom(in _openInspectorButton);
			return true;
		}
		if (name == PropertyName._refreshButton)
		{
			value = VariantUtils.CreateFrom(in _refreshButton);
			return true;
		}
		if (name == PropertyName._resourcePropertyTree)
		{
			value = VariantUtils.CreateFrom(in _resourcePropertyTree);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			value = VariantUtils.CreateFrom(in _baseType);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPreviewPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openInspectorButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._refreshButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePropertyTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._baseType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._resourcePicker, Variant.From(in _resourcePicker));
		info.AddProperty(PropertyName._previewPanel, Variant.From(in _previewPanel));
		info.AddProperty(PropertyName._previewTexture, Variant.From(in _previewTexture));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._audioPreviewPlayer, Variant.From(in _audioPreviewPlayer));
		info.AddProperty(PropertyName._playButton, Variant.From(in _playButton));
		info.AddProperty(PropertyName._stopButton, Variant.From(in _stopButton));
		info.AddProperty(PropertyName._openInspectorButton, Variant.From(in _openInspectorButton));
		info.AddProperty(PropertyName._refreshButton, Variant.From(in _refreshButton));
		info.AddProperty(PropertyName._resourcePropertyTree, Variant.From(in _resourcePropertyTree));
		info.AddProperty(PropertyName._baseType, Variant.From(in _baseType));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._resourcePicker, out var value))
		{
			_resourcePicker = value.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._previewPanel, out var value2))
		{
			_previewPanel = value2.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewTexture, out var value3))
		{
			_previewTexture = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value4))
		{
			_summaryLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._audioPreviewPlayer, out var value5))
		{
			_audioPreviewPlayer = value5.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._playButton, out var value6))
		{
			_playButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stopButton, out var value7))
		{
			_stopButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openInspectorButton, out var value8))
		{
			_openInspectorButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._refreshButton, out var value9))
		{
			_refreshButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resourcePropertyTree, out var value10))
		{
			_resourcePropertyTree = value10.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._baseType, out var value11))
		{
			_baseType = value11.As<string>();
		}
	}
}
