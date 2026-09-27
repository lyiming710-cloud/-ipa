using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Registry;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/Window/Selector/Type/XWWindowTypeSelector.cs")]
public class XWWindowTypeSelector : XWWindowSelector
{
	[Signal]
	public delegate void TypeSelectEventHandler(Variant.Type type, StringName className);

	public new class MethodName : XWWindowSelector.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCloseRequested = "OnCloseRequested";

		public new static readonly StringName OnConfirmed = "OnConfirmed";

		public new static readonly StringName BuildTree = "BuildTree";

		public new static readonly StringName OnItemSelected = "OnItemSelected";

		public new static readonly StringName ShowDescription = "ShowDescription";

		public new static readonly StringName CanFilterTreeItem = "CanFilterTreeItem";
	}

	public new class PropertyName : XWWindowSelector.PropertyName
	{
		public static readonly StringName SelectData = "SelectData";
	}

	public new class SignalName : XWWindowSelector.SignalName
	{
		public static readonly StringName TypeSelect = "TypeSelect";
	}

	private const string ScenePath = "res://addons/ModEditor/Window/Selector/Type/XWWindowTypeSelector.tscn";

	public Variant SelectData;

	private TypeSelectEventHandler backing_TypeSelect;

	public event TypeSelectEventHandler TypeSelect
	{
		add
		{
			backing_TypeSelect = (TypeSelectEventHandler)Delegate.Combine(backing_TypeSelect, value);
		}
		remove
		{
			backing_TypeSelect = (TypeSelectEventHandler)Delegate.Remove(backing_TypeSelect, value);
		}
	}

	public static XWWindowTypeSelector Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Window/Selector/Type/XWWindowTypeSelector.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWWindowTypeSelector>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
	}

	protected override void OnCloseRequested()
	{
		QueueFree();
	}

	protected override void OnConfirmed()
	{
		Button okButton = GetOkButton();
		if (GodotObject.IsInstanceValid(okButton) && okButton.Disabled)
		{
			return;
		}
		if (SelectData.VariantType == Variant.Type.Object)
		{
			GodotObject godotObject = SelectData.As<GodotObject>();
			if (godotObject is XWClassData xWClassData)
			{
				EmitSignal(SignalName.TypeSelect, 24, xWClassData.ClassName);
			}
			else if (godotObject is XWTypeData xWTypeData)
			{
				EmitSignal(SignalName.TypeSelect, (int)xWTypeData.Type, new StringName(""));
			}
		}
		QueueFree();
	}

	protected override void BuildTree()
	{
		base.BuildTree();
		List<string> currentClassList = new List<string>(XWClassRegistry.Instance.GetAllClass());
		string text = "Object";
		TreeItem treeItem = _tree.CreateItem(Root);
		treeItem.Collapsed = true;
		XWClassData xWClassData = (XWClassRegistry.Instance.HasClass(text) ? XWClassRegistry.Instance.GetClassData(text) : null);
		treeItem.SetText(0, text);
		Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(text);
		if (GodotObject.IsInstanceValid(classIcon))
		{
			treeItem.SetIcon(0, classIcon);
		}
		if (GodotObject.IsInstanceValid(xWClassData))
		{
			treeItem.SetMetadata(0, xWClassData);
		}
		BuildClassTree(currentClassList, text, treeItem);
		foreach (XWTypeData item in XWTypeRegistry.Instance.GetAllType())
		{
			TreeItem treeItem2 = _tree.CreateItem(Root);
			treeItem2.SetText(0, item.Name);
			if (GodotObject.IsInstanceValid(item.Icon))
			{
				treeItem2.SetIcon(0, item.Icon);
			}
			treeItem2.SetMetadata(0, item);
		}
	}

	private void BuildClassTree(List<string> currentClassList, string currentClass, TreeItem classTreeItem)
	{
		List<string> list = new List<string>(XWClassRegistry.Instance.GetInheritersFromClass(currentClass));
		while (list.Count > 0)
		{
			string text = list[0];
			list.RemoveAt(0);
			if (currentClassList.Contains(text) && !(XWClassRegistry.Instance.GetParentClass(text) != currentClass))
			{
				XWClassData xWClassData = (XWClassRegistry.Instance.HasClass(text) ? XWClassRegistry.Instance.GetClassData(text) : null);
				currentClassList.Remove(text);
				TreeItem treeItem = _tree.CreateItem(classTreeItem);
				treeItem.Collapsed = true;
				treeItem.SetText(0, text);
				Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(text);
				if (GodotObject.IsInstanceValid(classIcon))
				{
					treeItem.SetIcon(0, classIcon);
				}
				if (GodotObject.IsInstanceValid(xWClassData))
				{
					treeItem.SetMetadata(0, xWClassData);
				}
				BuildClassTree(currentClassList, text, treeItem);
			}
		}
	}

	protected override void OnItemSelected()
	{
		TreeItem selected = _tree.GetSelected();
		if (!GodotObject.IsInstanceValid(selected))
		{
			return;
		}
		Variant metadata = selected.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object)
		{
			GodotObject godotObject = metadata.As<GodotObject>();
			if (godotObject is XWClassData || godotObject is XWTypeData)
			{
				SelectData = metadata;
				ShowDescription(selected);
				GetOkButton().Disabled = false;
				return;
			}
		}
		GetOkButton().Disabled = true;
	}

	protected void ShowDescription(TreeItem item)
	{
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object)
		{
			GodotObject godotObject = metadata.As<GodotObject>();
			if (godotObject is XWClassData xWClassData)
			{
				_nameLabel.Text = xWClassData.ClassName + "\n";
			}
			else if (godotObject is XWTypeData xWTypeData)
			{
				_nameLabel.Text = xWTypeData.Name + "\n";
			}
		}
		else
		{
			_nameLabel.Text = "";
			_describeLabel.Text = "";
		}
	}

	protected override bool CanFilterTreeItem(string lowerQuery, TreeItem treeItem)
	{
		if (treeItem.GetText(0).ToLower().Contains(lowerQuery))
		{
			return true;
		}
		Variant metadata = treeItem.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Object)
		{
			GodotObject godotObject = metadata.As<GodotObject>();
			if (godotObject is XWClassData xWClassData && xWClassData.ClassName.ToLower().Contains(lowerQuery))
			{
				return true;
			}
			if (godotObject is XWTypeData xWTypeData && xWTypeData.Name.ToLower().Contains(lowerQuery))
			{
				return true;
			}
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ConfirmationDialog"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCloseRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowDescription, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanFilterTreeItem, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "lowerQuery", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "treeItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWWindowTypeSelector>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCloseRequested && args.Count == 0)
		{
			OnCloseRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConfirmed && args.Count == 0)
		{
			OnConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTree && args.Count == 0)
		{
			BuildTree();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemSelected && args.Count == 0)
		{
			OnItemSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowDescription && args.Count == 1)
		{
			ShowDescription(VariantUtils.ConvertTo<TreeItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanFilterTreeItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanFilterTreeItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<TreeItem>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWWindowTypeSelector>(Create());
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
		if (method == MethodName.OnCloseRequested)
		{
			return true;
		}
		if (method == MethodName.OnConfirmed)
		{
			return true;
		}
		if (method == MethodName.BuildTree)
		{
			return true;
		}
		if (method == MethodName.OnItemSelected)
		{
			return true;
		}
		if (method == MethodName.ShowDescription)
		{
			return true;
		}
		if (method == MethodName.CanFilterTreeItem)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SelectData)
		{
			SelectData = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.SelectData)
		{
			value = VariantUtils.CreateFrom(in SelectData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, PropertyName.SelectData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SelectData, Variant.From(in SelectData));
		info.AddSignalEventDelegate(SignalName.TypeSelect, backing_TypeSelect);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SelectData, out var value))
		{
			SelectData = value.As<Variant>();
		}
		if (info.TryGetSignalEventDelegate<TypeSelectEventHandler>(SignalName.TypeSelect, out var value2))
		{
			backing_TypeSelect = value2;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.TypeSelect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalTypeSelect(Variant.Type type, StringName className)
	{
		StringName typeSelect = SignalName.TypeSelect;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = (long)type;
		buffer[1] = className;
		EmitSignal(typeSelect, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.TypeSelect && args.Count == 2)
		{
			backing_TypeSelect?.Invoke(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.TypeSelect)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
