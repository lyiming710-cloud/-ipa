using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Layout;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/ScriptItemTree/XWBPScriptItemTreeBase.cs")]
public abstract class XWBPScriptItemTreeBase : Tree
{
	public new class MethodName : Tree.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMenu = "CreateMenu";

		public static readonly StringName PopMenu = "PopMenu";

		public static readonly StringName PopupMenuAtPosition = "PopupMenuAtPosition";

		public static readonly StringName SelectItemAtPosition = "SelectItemAtPosition";

		public static readonly StringName OnItemMouseSelected = "OnItemMouseSelected";

		public static readonly StringName OnItemActivated = "OnItemActivated";

		public static readonly StringName OnItemEdited = "OnItemEdited";

		public static readonly StringName OnMenuIdPressed = "OnMenuIdPressed";

		public static readonly StringName BuildDragDataForSelectedItem = "BuildDragDataForSelectedItem";

		public static readonly StringName UpdateMinimumHeight = "UpdateMinimumHeight";

		public static readonly StringName LoadScriptItemIcon = "LoadScriptItemIcon";

		public static readonly StringName AddScriptItemMenuItem = "AddScriptItemMenuItem";
	}

	public new class PropertyName : Tree.PropertyName
	{
		public static readonly StringName Editor = "Editor";

		public static readonly StringName _root = "_root";

		public static readonly StringName _menu = "_menu";
	}

	public new class SignalName : Tree.SignalName
	{
	}

	protected TreeItem _root;

	protected PopupMenu _menu;

	public XWBPEditor Editor { get; set; }

	public override void _Ready()
	{
		AllowRmbSelect = true;
		ItemActivated += OnItemActivated;
		ItemMouseSelected += OnItemMouseSelected;
		ItemEdited += OnItemEdited;
		CreateMenu();
	}

	protected virtual void CreateMenu()
	{
		_menu = new PopupMenu
		{
			Name = "ScriptItemTreeMenu"
		};
		AddChild(_menu, forceReadableName: false, InternalMode.Disabled);
		_menu.IdPressed += OnMenuIdPressed;
	}

	protected void PopMenu(Vector2 pos)
	{
		if (GodotObject.IsInstanceValid(SelectItemAtPosition(pos)))
		{
			PopupMenuAtPosition(pos);
		}
	}

	protected void PopupMenuAtPosition(Vector2 pos)
	{
		_menu.Position = new Vector2I((int)((float)GetWindow().Position.X + GlobalPosition.X + pos.X), (int)((float)GetWindow().Position.Y + GlobalPosition.Y + pos.Y));
		_menu.Popup();
	}

	protected TreeItem SelectItemAtPosition(Vector2 localPosition)
	{
		TreeItem itemAtPosition = GetItemAtPosition(localPosition);
		if (!GodotObject.IsInstanceValid(itemAtPosition))
		{
			return null;
		}
		DeselectAll();
		itemAtPosition.Select(0);
		return itemAtPosition;
	}

	protected virtual void OnItemMouseSelected(Vector2 mousePosition, long mouseButtonIndex)
	{
		if (mouseButtonIndex == 2)
		{
			PopMenu(mousePosition);
		}
	}

	protected virtual void OnItemActivated()
	{
	}

	protected virtual void OnItemEdited()
	{
	}

	protected abstract void OnMenuIdPressed(long id);

	protected Variant BuildDragDataForSelectedItem(Vector2 atPosition, XWDragData.Type dragType)
	{
		TreeItem treeItem = SelectItemAtPosition(atPosition);
		if (treeItem == null)
		{
			return default;
		}
		Variant metadata = treeItem.GetMetadata(0);
		if (metadata.VariantType != Variant.Type.Object)
		{
			return default;
		}
		SetDragPreview(new Label
		{
			Text = treeItem.GetText(0)
		});
		return Variant.From<XWDragData>(new XWDragData(metadata, dragType));
	}

	protected void UpdateMinimumHeight()
	{
		if (_root != null)
		{
			CustomMinimumSize = new Vector2(0f, _root.GetChildCount() * 24);
		}
	}

	protected static Texture2D LoadScriptItemIcon(string relativeIconPath)
	{
		if (string.IsNullOrEmpty(relativeIconPath))
		{
			return null;
		}
		string path = (relativeIconPath.StartsWith("res://") ? relativeIconPath : ("res://addons/ModEditor/Icons/" + relativeIconPath));
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	protected void AddScriptItemMenuItem(string text, int id, string iconPath = null)
	{
		Texture2D texture2D = LoadScriptItemIcon(iconPath);
		if (GodotObject.IsInstanceValid(texture2D))
		{
			_menu.AddIconItem(texture2D, text, id, Key.None);
		}
		else
		{
			_menu.AddItem(text, id, Key.None);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopupMenuAtPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectItemAtPosition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemMouseSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDragDataForSelectedItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "dragType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMinimumHeight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadScriptItemIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "relativeIconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddScriptItemMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateMenu && args.Count == 0)
		{
			CreateMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.PopMenu && args.Count == 1)
		{
			PopMenu(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopupMenuAtPosition && args.Count == 1)
		{
			PopupMenuAtPosition(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectItemAtPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(SelectItemAtPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.OnItemMouseSelected && args.Count == 2)
		{
			OnItemMouseSelected(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemActivated && args.Count == 0)
		{
			OnItemActivated();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemEdited && args.Count == 0)
		{
			OnItemEdited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMenuIdPressed && args.Count == 1)
		{
			OnMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildDragDataForSelectedItem && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(BuildDragDataForSelectedItem(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<XWDragData.Type>(in args[1])));
			return true;
		}
		if (method == MethodName.UpdateMinimumHeight && args.Count == 0)
		{
			UpdateMinimumHeight();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadScriptItemIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadScriptItemIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddScriptItemMenuItem && args.Count == 3)
		{
			AddScriptItemMenuItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadScriptItemIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadScriptItemIcon(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.CreateMenu)
		{
			return true;
		}
		if (method == MethodName.PopMenu)
		{
			return true;
		}
		if (method == MethodName.PopupMenuAtPosition)
		{
			return true;
		}
		if (method == MethodName.SelectItemAtPosition)
		{
			return true;
		}
		if (method == MethodName.OnItemMouseSelected)
		{
			return true;
		}
		if (method == MethodName.OnItemActivated)
		{
			return true;
		}
		if (method == MethodName.OnItemEdited)
		{
			return true;
		}
		if (method == MethodName.OnMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.BuildDragDataForSelectedItem)
		{
			return true;
		}
		if (method == MethodName.UpdateMinimumHeight)
		{
			return true;
		}
		if (method == MethodName.LoadScriptItemIcon)
		{
			return true;
		}
		if (method == MethodName.AddScriptItemMenuItem)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Editor)
		{
			Editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName._root)
		{
			_root = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName._menu)
		{
			_menu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Editor)
		{
			value = VariantUtils.CreateFrom<XWBPEditor>(Editor);
			return true;
		}
		if (name == PropertyName._root)
		{
			value = VariantUtils.CreateFrom(in _root);
			return true;
		}
		if (name == PropertyName._menu)
		{
			value = VariantUtils.CreateFrom(in _menu);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._root, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._menu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Editor, Variant.From<XWBPEditor>(Editor));
		info.AddProperty(PropertyName._root, Variant.From(in _root));
		info.AddProperty(PropertyName._menu, Variant.From(in _menu));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Editor, out var value))
		{
			Editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName._root, out var value2))
		{
			_root = value2.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName._menu, out var value3))
		{
			_menu = value3.As<PopupMenu>();
		}
	}
}
