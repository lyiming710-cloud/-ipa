using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/Theme/XWInspectorPropertyEditorTheme.cs")]
public class XWInspectorPropertyEditorTheme : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName RefreshTheme = "RefreshTheme";

		public static readonly StringName ReadTheme = "ReadTheme";

		public static readonly StringName PopulateThemeTree = "PopulateThemeTree";

		public static readonly StringName OpenSelectedResource = "OpenSelectedResource";

		public static readonly StringName CommitThemeChange = "CommitThemeChange";

		public static readonly StringName FormatColor = "FormatColor";

		public static readonly StringName FormatResource = "FormatResource";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _themeItemTree = "_themeItemTree";

		public static readonly StringName _previewButton = "_previewButton";

		public static readonly StringName _previewPanel = "_previewPanel";

		public static readonly StringName _theme = "_theme";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private Tree _themeItemTree;

	private Button _previewButton;

	private PanelContainer _previewPanel;

	private Theme _theme;

	public override void _Ready()
	{
		base._Ready();
		_themeItemTree = GetNode<Tree>("%ThemeItemTree");
		_previewButton = GetNode<Button>("%PreviewButton");
		_previewPanel = GetNode<PanelContainer>("%PreviewPanel");
		_themeItemTree.SetColumnTitle(0, "项目");
		_themeItemTree.SetColumnTitle(1, "类型");
		_themeItemTree.SetColumnTitle(2, "值");
		_themeItemTree.ItemActivated += OpenSelectedResource;
		_previewButton.Pressed += CommitThemeChange;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		RefreshTheme();
	}

	public override void UpdateValue()
	{
		RefreshTheme();
	}

	public override Variant GetValue()
	{
		return _theme;
	}

	private void RefreshTheme()
	{
		_theme = ReadTheme();
		_previewPanel.Theme = _theme;
		_previewButton.Theme = _theme;
		PopulateThemeTree();
	}

	private Theme ReadTheme()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Object || !(propertyValue.As<GodotObject>() is Theme result))
		{
			return null;
		}
		return result;
	}

	private void PopulateThemeTree()
	{
		_themeItemTree.Clear();
		TreeItem parent = _themeItemTree.CreateItem();
		if (!GodotObject.IsInstanceValid(_theme))
		{
			return;
		}
		string[] typeList = _theme.GetTypeList();
		foreach (string typeName in typeList)
		{
			TreeItem treeItem = _themeItemTree.CreateItem(parent);
			treeItem.SetText(0, typeName);
			treeItem.SetText(1, "Type");
			treeItem.Collapsed = true;
			AppendThemeItemRows(treeItem, typeName, "颜色", _theme.GetColorList(typeName), (StringName name) => FormatColor(_theme.GetColor(name, typeName)), null);
			AppendThemeItemRows(treeItem, typeName, "常量", _theme.GetConstantList(typeName), (StringName name) => _theme.GetConstant(name, typeName).ToString(), null);
			AppendThemeItemRows(treeItem, typeName, "字体大小", _theme.GetFontSizeList(typeName), (StringName name) => _theme.GetFontSize(name, typeName).ToString(), null);
			AppendThemeItemRows(treeItem, typeName, "字体", _theme.GetFontList(typeName), (StringName name) => FormatResource(_theme.GetFont(name, typeName)), (StringName name) => _theme.GetFont(name, typeName));
			AppendThemeItemRows(treeItem, typeName, "图标", _theme.GetIconList(typeName), (StringName name) => FormatResource(_theme.GetIcon(name, typeName)), (StringName name) => _theme.GetIcon(name, typeName));
			AppendThemeItemRows(treeItem, typeName, "StyleBox", _theme.GetStyleboxList(typeName), (StringName name) => FormatResource(_theme.GetStylebox(name, typeName)), (StringName name) => _theme.GetStylebox(name, typeName));
		}
	}

	private void AppendThemeItemRows(TreeItem parent, string typeName, string groupName, string[] names, Func<StringName, string> valueFormatter, Func<StringName, Resource> resourceGetter)
	{
		if (names == null || names.Length == 0)
		{
			return;
		}
		TreeItem treeItem = _themeItemTree.CreateItem(parent);
		treeItem.SetText(0, groupName);
		treeItem.SetText(1, typeName);
		treeItem.Collapsed = true;
		foreach (string text in names)
		{
			StringName arg = new StringName(text);
			TreeItem treeItem2 = _themeItemTree.CreateItem(treeItem);
			treeItem2.SetText(0, text);
			treeItem2.SetText(1, groupName);
			treeItem2.SetText(2, valueFormatter(arg));
			Resource resource = resourceGetter?.Invoke(arg);
			if (GodotObject.IsInstanceValid(resource))
			{
				treeItem2.SetMetadata(0, resource);
			}
		}
	}

	private void OpenSelectedResource()
	{
		TreeItem selected = _themeItemTree.GetSelected();
		if (selected != null)
		{
			Variant metadata = selected.GetMetadata(0);
			if (metadata.VariantType == Variant.Type.Object && metadata.As<GodotObject>() is Resource res)
			{
				XWEditorInterface.Instance?.EditResource(res);
			}
		}
	}

	private void CommitThemeChange()
	{
		if (GodotObject.IsInstanceValid(_theme))
		{
			_theme.EmitChanged();
			if (GodotObject.IsInstanceValid(Property) && Property.PropName != (StringName)"")
			{
				ValueChange(_theme);
			}
			else
			{
				Property?.SetCall();
			}
		}
	}

	private static string FormatColor(Color color)
	{
		return "#" + color.ToHtml();
	}

	private static string FormatResource(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "空";
		}
		string text = (string.IsNullOrWhiteSpace(resource.ResourcePath) ? "内置" : resource.ResourcePath.GetFile());
		return resource.GetClass() + " (" + text + ")";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTheme, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadTheme, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Theme"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateThemeTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSelectedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitThemeChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatColor, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.RefreshTheme && args.Count == 0)
		{
			RefreshTheme();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadTheme && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Theme>(ReadTheme());
			return true;
		}
		if (method == MethodName.PopulateThemeTree && args.Count == 0)
		{
			PopulateThemeTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenSelectedResource && args.Count == 0)
		{
			OpenSelectedResource();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitThemeChange && args.Count == 0)
		{
			CommitThemeChange();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatColor(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FormatColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatColor(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.RefreshTheme)
		{
			return true;
		}
		if (method == MethodName.ReadTheme)
		{
			return true;
		}
		if (method == MethodName.PopulateThemeTree)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedResource)
		{
			return true;
		}
		if (method == MethodName.CommitThemeChange)
		{
			return true;
		}
		if (method == MethodName.FormatColor)
		{
			return true;
		}
		if (method == MethodName.FormatResource)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._themeItemTree)
		{
			_themeItemTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._previewButton)
		{
			_previewButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._previewPanel)
		{
			_previewPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._theme)
		{
			_theme = VariantUtils.ConvertTo<Theme>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._themeItemTree)
		{
			value = VariantUtils.CreateFrom(in _themeItemTree);
			return true;
		}
		if (name == PropertyName._previewButton)
		{
			value = VariantUtils.CreateFrom(in _previewButton);
			return true;
		}
		if (name == PropertyName._previewPanel)
		{
			value = VariantUtils.CreateFrom(in _previewPanel);
			return true;
		}
		if (name == PropertyName._theme)
		{
			value = VariantUtils.CreateFrom(in _theme);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._themeItemTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._theme, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._themeItemTree, Variant.From(in _themeItemTree));
		info.AddProperty(PropertyName._previewButton, Variant.From(in _previewButton));
		info.AddProperty(PropertyName._previewPanel, Variant.From(in _previewPanel));
		info.AddProperty(PropertyName._theme, Variant.From(in _theme));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._themeItemTree, out var value))
		{
			_themeItemTree = value.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._previewButton, out var value2))
		{
			_previewButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._previewPanel, out var value3))
		{
			_previewPanel = value3.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._theme, out var value4))
		{
			_theme = value4.As<Theme>();
		}
	}
}
