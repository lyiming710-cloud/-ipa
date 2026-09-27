using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWCreateDialog.cs")]
public class XWCreateDialog : ConfirmationDialog
{
	[Signal]
	public delegate void CreatedEventHandler(string typeHint);

	public new class MethodName : ConfirmationDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ConfigureForResources = "ConfigureForResources";

		public static readonly StringName BuildTypeFilter = "BuildTypeFilter";

		public static readonly StringName BindTypeChips = "BindTypeChips";

		public static readonly StringName SelectTypeMode = "SelectTypeMode";

		public static readonly StringName RefreshTree = "RefreshTree";

		public static readonly StringName ResetTypePageAndRefresh = "ResetTypePageAndRefresh";

		public static readonly StringName ChangeTypePage = "ChangeTypePage";

		public static readonly StringName OnSearchGuiInput = "OnSearchGuiInput";

		public static readonly StringName UpdateSelectionPreview = "UpdateSelectionPreview";

		public static readonly StringName BuildInheritancePath = "BuildInheritancePath";

		public static readonly StringName ClearSelectionPreview = "ClearSelectionPreview";

		public static readonly StringName OnConfirmed = "OnConfirmed";
	}

	public new class PropertyName : ConfirmationDialog.PropertyName
	{
		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _typeFilter = "_typeFilter";

		public static readonly StringName _typeTree = "_typeTree";

		public static readonly StringName _previewContainer = "_previewContainer";

		public static readonly StringName _typeIcon = "_typeIcon";

		public static readonly StringName _selectedTypeLabel = "_selectedTypeLabel";

		public static readonly StringName _inheritanceLabel = "_inheritanceLabel";

		public static readonly StringName _scriptPathLabel = "_scriptPathLabel";

		public static readonly StringName _typeChips = "_typeChips";

		public static readonly StringName _previousTypePageButton = "_previousTypePageButton";

		public static readonly StringName _nextTypePageButton = "_nextTypePageButton";

		public static readonly StringName _typePageLabel = "_typePageLabel";

		public static readonly StringName _resourceOnly = "_resourceOnly";

		public static readonly StringName _selectedTypeMode = "_selectedTypeMode";

		public static readonly StringName _currentTypePage = "_currentTypePage";
	}

	public new class SignalName : ConfirmationDialog.SignalName
	{
		public static readonly StringName Created = "Created";
	}

	private const int TypePageSize = 200;

	private LineEdit _searchEdit;

	private OptionButton _typeFilter;

	private Tree _typeTree;

	private VBoxContainer _previewContainer;

	private TextureRect _typeIcon;

	private Label _selectedTypeLabel;

	private Label _inheritanceLabel;

	private Label _scriptPathLabel;

	private Button[] _typeChips;

	private Button _previousTypePageButton;

	private Button _nextTypePageButton;

	private Label _typePageLabel;

	private bool _resourceOnly;

	private int _selectedTypeMode;

	private int _currentTypePage;

	private CreatedEventHandler backing_Created;

	public event CreatedEventHandler Created
	{
		add
		{
			backing_Created = (CreatedEventHandler)Delegate.Combine(backing_Created, value);
		}
		remove
		{
			backing_Created = (CreatedEventHandler)Delegate.Remove(backing_Created, value);
		}
	}

	public override void _Ready()
	{
		_searchEdit = GetNode<LineEdit>("%SearchEdit");
		_typeFilter = GetNode<OptionButton>("%TypeFilter");
		_typeTree = GetNode<Tree>("%TypeTree");
		_previewContainer = GetNode<VBoxContainer>("%PreviewContainer");
		_typeIcon = GetNode<TextureRect>("%TypeIcon");
		_selectedTypeLabel = GetNode<Label>("%SelectedTypeLabel");
		_inheritanceLabel = GetNode<Label>("%InheritanceLabel");
		_scriptPathLabel = GetNode<Label>("%ScriptPathLabel");
		_previousTypePageButton = GetNode<Button>("%PreviousTypePageButton");
		_nextTypePageButton = GetNode<Button>("%NextTypePageButton");
		_typePageLabel = GetNode<Label>("%TypePageLabel");
		_typeChips = new Button[3]
		{
			GetNode<Button>("%AllTypeChip"),
			GetNode<Button>("%ResourceTypeChip"),
			GetNode<Button>("%NodeTypeChip")
		};
		_searchEdit.TextChanged += (string _) =>
		{
			ResetTypePageAndRefresh();
		};
		_searchEdit.GuiInput += OnSearchGuiInput;
		_typeFilter.ItemSelected += (long index) =>
		{
			SelectTypeMode((int)index);
		};
		_typeTree.ItemSelected += UpdateSelectionPreview;
		_typeTree.ItemActivated += () =>
		{
			OnConfirmed();
		};
		_previousTypePageButton.Pressed += () =>
		{
			ChangeTypePage(-1);
		};
		_nextTypePageButton.Pressed += () =>
		{
			ChangeTypePage(1);
		};
		Confirmed += OnConfirmed;
		BuildTypeFilter();
		BindTypeChips();
		RefreshTree();
	}

	public void ConfigureForResources()
	{
		_resourceOnly = true;
		Title = "新建其他资源";
		if (GodotObject.IsInstanceValid(_typeFilter))
		{
			SelectTypeMode(1);
			_typeFilter.Disabled = true;
			Button[] typeChips = _typeChips;
			for (int i = 0; i < typeChips.Length; i++)
			{
				typeChips[i].Disabled = true;
			}
		}
		if (IsNodeReady())
		{
			RefreshTree();
		}
	}

	private void BuildTypeFilter()
	{
		_typeFilter.Clear();
		_typeFilter.AddItem("全部", 0);
		_typeFilter.AddItem("资源", 1);
		_typeFilter.AddItem("节点", 2);
	}

	private void BindTypeChips()
	{
		for (int i = 0; i < _typeChips.Length; i++)
		{
			int mode = i;
			_typeChips[i].Pressed += () =>
			{
				SelectTypeMode(mode);
			};
		}
		SelectTypeMode(_resourceOnly ? 1 : 0);
	}

	private void SelectTypeMode(int mode)
	{
		int max = ((_typeChips == null) ? 2 : (_typeChips.Length - 1));
		_selectedTypeMode = (_resourceOnly ? 1 : Mathf.Clamp(mode, 0, max));
		_currentTypePage = 0;
		_typeFilter?.Select(_selectedTypeMode);
		if (_typeChips != null)
		{
			for (int i = 0; i < _typeChips.Length; i++)
			{
				_typeChips[i].SetPressedNoSignal(i == _selectedTypeMode);
			}
		}
		if (IsNodeReady())
		{
			RefreshTree();
		}
	}

	private void RefreshTree()
	{
		_typeTree.Clear();
		TreeItem treeItem = _typeTree.CreateItem();
		string value = _searchEdit.Text?.StripEdges() ?? "";
		int num = (_resourceOnly ? 1 : _selectedTypeMode);
		List<string> list = new List<string>();
		foreach (XWClassData allClassDatum in XWClassRegistry.Instance.GetAllClassData())
		{
			bool flag = XWClassRegistry.Instance.IsParentClass(allClassDatum.ClassName, "Resource");
			bool flag2 = XWClassRegistry.Instance.IsParentClass(allClassDatum.ClassName, "Node");
			if ((num != 1 || flag) && (num != 2 || flag2) && (num != 1 || allClassDatum.IsGlobalClass) && XWClassRegistry.Instance.CanInstantiate(allClassDatum.ClassName) && (string.IsNullOrEmpty(value) || allClassDatum.ClassName.Contains(value, StringComparison.OrdinalIgnoreCase)))
			{
				list.Add(allClassDatum.ClassName);
			}
		}
		list.Sort(StringComparer.OrdinalIgnoreCase);
		int num2 = Math.Max(1, (list.Count + 200 - 1) / 200);
		_currentTypePage = Mathf.Clamp(_currentTypePage, 0, num2 - 1);
		int num3 = _currentTypePage * 200;
		int num4 = Math.Min(num3 + 200, list.Count);
		for (int i = num3; i < num4; i++)
		{
			string text = list[i];
			TreeItem treeItem2 = _typeTree.CreateItem(treeItem);
			treeItem2.SetText(0, text);
			treeItem2.SetMetadata(0, text);
			treeItem2.SetIcon(0, XWClassRegistry.Instance.GetClassIcon(text));
		}
		_typePageLabel.Text = $"第 {_currentTypePage + 1} / {num2} 页 · {list.Count} 项";
		_previousTypePageButton.Disabled = _currentTypePage <= 0;
		_nextTypePageButton.Disabled = _currentTypePage >= num2 - 1;
		if (treeItem.GetFirstChild() != null)
		{
			treeItem.GetFirstChild().Select(0);
			UpdateSelectionPreview();
		}
		else
		{
			ClearSelectionPreview("没有匹配的可创建类型");
		}
	}

	private void ResetTypePageAndRefresh()
	{
		_currentTypePage = 0;
		RefreshTree();
	}

	private void ChangeTypePage(int direction)
	{
		_currentTypePage += direction;
		RefreshTree();
		_typeTree.GrabFocus();
	}

	private void OnSearchGuiInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey { Pressed: not false, Echo: false } inputEventKey && inputEventKey.Keycode == Key.Down && _typeTree.GetSelected() != null)
		{
			_typeTree.GrabFocus();
			_searchEdit.AcceptEvent();
		}
	}

	private void UpdateSelectionPreview()
	{
		TreeItem selected = _typeTree.GetSelected();
		if (selected == null)
		{
			ClearSelectionPreview("请选择资源类型");
			return;
		}
		string text = selected.GetMetadata(0).AsString();
		XWClassData classData = XWClassRegistry.Instance.GetClassData(text);
		_typeIcon.Texture = XWClassRegistry.Instance.GetClassIcon(text);
		_selectedTypeLabel.Text = text;
		_inheritanceLabel.Text = "继承：" + BuildInheritancePath(text);
		_scriptPathLabel.Text = classData?.ScriptFile?.ResourcePath ?? "Godot 内置类型";
	}

	private static string BuildInheritancePath(string className)
	{
		List<string> list = new List<string>();
		string text = className;
		for (int i = 0; i < 12; i++)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				break;
			}
			list.Add(text);
			if (text == "Resource" || text == "Node")
			{
				break;
			}
			text = XWClassRegistry.Instance.GetParentClass(text);
		}
		return string.Join("  →  ", list);
	}

	private void ClearSelectionPreview(string message)
	{
		_typeIcon.Texture = null;
		_selectedTypeLabel.Text = message;
		_inheritanceLabel.Text = "";
		_scriptPathLabel.Text = "";
	}

	private void OnConfirmed()
	{
		TreeItem selected = _typeTree.GetSelected();
		if (selected != null)
		{
			string text = selected.GetMetadata(0).AsString();
			EmitSignal(SignalName.Created, text);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureForResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTypeFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindTypeChips, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectTypeMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetTypePageAndRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChangeTypePage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSearchGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSelectionPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildInheritancePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearSelectionPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ConfigureForResources && args.Count == 0)
		{
			ConfigureForResources();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTypeFilter && args.Count == 0)
		{
			BuildTypeFilter();
			ret = default;
			return true;
		}
		if (method == MethodName.BindTypeChips && args.Count == 0)
		{
			BindTypeChips();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectTypeMode && args.Count == 1)
		{
			SelectTypeMode(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTree && args.Count == 0)
		{
			RefreshTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetTypePageAndRefresh && args.Count == 0)
		{
			ResetTypePageAndRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeTypePage && args.Count == 1)
		{
			ChangeTypePage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchGuiInput && args.Count == 1)
		{
			OnSearchGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSelectionPreview && args.Count == 0)
		{
			UpdateSelectionPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildInheritancePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildInheritancePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearSelectionPreview && args.Count == 1)
		{
			ClearSelectionPreview(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildInheritancePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildInheritancePath(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.ConfigureForResources)
		{
			return true;
		}
		if (method == MethodName.BuildTypeFilter)
		{
			return true;
		}
		if (method == MethodName.BindTypeChips)
		{
			return true;
		}
		if (method == MethodName.SelectTypeMode)
		{
			return true;
		}
		if (method == MethodName.RefreshTree)
		{
			return true;
		}
		if (method == MethodName.ResetTypePageAndRefresh)
		{
			return true;
		}
		if (method == MethodName.ChangeTypePage)
		{
			return true;
		}
		if (method == MethodName.OnSearchGuiInput)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectionPreview)
		{
			return true;
		}
		if (method == MethodName.BuildInheritancePath)
		{
			return true;
		}
		if (method == MethodName.ClearSelectionPreview)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._typeFilter)
		{
			_typeFilter = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._typeTree)
		{
			_typeTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._previewContainer)
		{
			_previewContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._typeIcon)
		{
			_typeIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._selectedTypeLabel)
		{
			_selectedTypeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._inheritanceLabel)
		{
			_inheritanceLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._scriptPathLabel)
		{
			_scriptPathLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._typeChips)
		{
			_typeChips = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
			return true;
		}
		if (name == PropertyName._previousTypePageButton)
		{
			_previousTypePageButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextTypePageButton)
		{
			_nextTypePageButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._typePageLabel)
		{
			_typePageLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceOnly)
		{
			_resourceOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._selectedTypeMode)
		{
			_selectedTypeMode = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._currentTypePage)
		{
			_currentTypePage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._typeFilter)
		{
			value = VariantUtils.CreateFrom(in _typeFilter);
			return true;
		}
		if (name == PropertyName._typeTree)
		{
			value = VariantUtils.CreateFrom(in _typeTree);
			return true;
		}
		if (name == PropertyName._previewContainer)
		{
			value = VariantUtils.CreateFrom(in _previewContainer);
			return true;
		}
		if (name == PropertyName._typeIcon)
		{
			value = VariantUtils.CreateFrom(in _typeIcon);
			return true;
		}
		if (name == PropertyName._selectedTypeLabel)
		{
			value = VariantUtils.CreateFrom(in _selectedTypeLabel);
			return true;
		}
		if (name == PropertyName._inheritanceLabel)
		{
			value = VariantUtils.CreateFrom(in _inheritanceLabel);
			return true;
		}
		if (name == PropertyName._scriptPathLabel)
		{
			value = VariantUtils.CreateFrom(in _scriptPathLabel);
			return true;
		}
		if (name == PropertyName._typeChips)
		{
			GodotObject[] typeChips = _typeChips;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(typeChips);
			return true;
		}
		if (name == PropertyName._previousTypePageButton)
		{
			value = VariantUtils.CreateFrom(in _previousTypePageButton);
			return true;
		}
		if (name == PropertyName._nextTypePageButton)
		{
			value = VariantUtils.CreateFrom(in _nextTypePageButton);
			return true;
		}
		if (name == PropertyName._typePageLabel)
		{
			value = VariantUtils.CreateFrom(in _typePageLabel);
			return true;
		}
		if (name == PropertyName._resourceOnly)
		{
			value = VariantUtils.CreateFrom(in _resourceOnly);
			return true;
		}
		if (name == PropertyName._selectedTypeMode)
		{
			value = VariantUtils.CreateFrom(in _selectedTypeMode);
			return true;
		}
		if (name == PropertyName._currentTypePage)
		{
			value = VariantUtils.CreateFrom(in _currentTypePage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typeIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedTypeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inheritanceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptPathLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._typeChips, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousTypePageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextTypePageButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._typePageLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resourceOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedTypeMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentTypePage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._typeFilter, Variant.From(in _typeFilter));
		info.AddProperty(PropertyName._typeTree, Variant.From(in _typeTree));
		info.AddProperty(PropertyName._previewContainer, Variant.From(in _previewContainer));
		info.AddProperty(PropertyName._typeIcon, Variant.From(in _typeIcon));
		info.AddProperty(PropertyName._selectedTypeLabel, Variant.From(in _selectedTypeLabel));
		info.AddProperty(PropertyName._inheritanceLabel, Variant.From(in _inheritanceLabel));
		info.AddProperty(PropertyName._scriptPathLabel, Variant.From(in _scriptPathLabel));
		StringName typeChips = PropertyName._typeChips;
		GodotObject[] typeChips2 = _typeChips;
		info.AddProperty(typeChips, Variant.CreateFrom(typeChips2));
		info.AddProperty(PropertyName._previousTypePageButton, Variant.From(in _previousTypePageButton));
		info.AddProperty(PropertyName._nextTypePageButton, Variant.From(in _nextTypePageButton));
		info.AddProperty(PropertyName._typePageLabel, Variant.From(in _typePageLabel));
		info.AddProperty(PropertyName._resourceOnly, Variant.From(in _resourceOnly));
		info.AddProperty(PropertyName._selectedTypeMode, Variant.From(in _selectedTypeMode));
		info.AddProperty(PropertyName._currentTypePage, Variant.From(in _currentTypePage));
		info.AddSignalEventDelegate(SignalName.Created, backing_Created);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._searchEdit, out var value))
		{
			_searchEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._typeFilter, out var value2))
		{
			_typeFilter = value2.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._typeTree, out var value3))
		{
			_typeTree = value3.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._previewContainer, out var value4))
		{
			_previewContainer = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._typeIcon, out var value5))
		{
			_typeIcon = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._selectedTypeLabel, out var value6))
		{
			_selectedTypeLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._inheritanceLabel, out var value7))
		{
			_inheritanceLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._scriptPathLabel, out var value8))
		{
			_scriptPathLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._typeChips, out var value9))
		{
			_typeChips = value9.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._previousTypePageButton, out var value10))
		{
			_previousTypePageButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextTypePageButton, out var value11))
		{
			_nextTypePageButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._typePageLabel, out var value12))
		{
			_typePageLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceOnly, out var value13))
		{
			_resourceOnly = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._selectedTypeMode, out var value14))
		{
			_selectedTypeMode = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._currentTypePage, out var value15))
		{
			_currentTypePage = value15.As<int>();
		}
		if (info.TryGetSignalEventDelegate<CreatedEventHandler>(SignalName.Created, out var value16))
		{
			backing_Created = value16;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.Created, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "typeHint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalCreated(string typeHint)
	{
		EmitSignal(SignalName.Created, new ReadOnlySpan<Variant>((Variant)typeHint));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.Created && args.Count == 1)
		{
			backing_Created?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.Created)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
