using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Category/XWInspectorCategory.cs")]
public class XWInspectorCategory : PanelContainer
{
	public enum ClassMenuOption
	{
		MenuOpenDocs,
		MenuUnfavoriteAll
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Setup = "Setup";

		public static readonly StringName SetFavorite = "SetFavorite";

		public static readonly StringName IsFavorite = "IsFavorite";

		public static readonly StringName GetCategoryName = "GetCategoryName";

		public static readonly StringName OnGuiInput = "OnGuiInput";

		public static readonly StringName OnMenuItem = "OnMenuItem";

		public static readonly StringName OpenDocumentation = "OpenDocumentation";

		public static readonly StringName UnfavoriteAll = "UnfavoriteAll";

		public static readonly StringName GetInspector = "GetInspector";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _iconRect = "_iconRect";

		public static readonly StringName _label = "_label";

		public static readonly StringName _categoryName = "_categoryName";

		public static readonly StringName _categoryIcon = "_categoryIcon";

		public static readonly StringName _isFavorite = "_isFavorite";

		public static readonly StringName _menu = "_menu";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private static readonly string ScenePath = "res://addons/ModEditor/Inspector/GUI/Category/XWInspectorCategory.tscn";

	private TextureRect _iconRect;

	private Label _label;

	private string _categoryName = "";

	private Texture2D _categoryIcon;

	private bool _isFavorite;

	private PopupMenu _menu;

	public static XWInspectorCategory Create()
	{
		return ResourceLoader.Load<PackedScene>(ScenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorCategory>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		_iconRect = GetNode<TextureRect>("%IconRect");
		_label = GetNode<Label>("%Label");
		_menu = new PopupMenu();
		_menu.AddItem("打开文档", 0, Key.None);
		_menu.AddItem("取消全部收藏", 1, Key.None);
		_menu.IdPressed += OnMenuItem;
		AddChild(_menu, forceReadableName: false, InternalMode.Disabled);
		GuiInput += OnGuiInput;
	}

	public void Setup(string pName, Texture2D pIcon = null)
	{
		_categoryName = pName;
		_categoryIcon = pIcon;
		if (GodotObject.IsInstanceValid(_label))
		{
			_label.Text = pName;
		}
		if (GodotObject.IsInstanceValid(_iconRect))
		{
			if (GodotObject.IsInstanceValid(pIcon))
			{
				_iconRect.Texture = pIcon;
				_iconRect.Visible = true;
			}
			else
			{
				_iconRect.Visible = false;
			}
		}
	}

	public void SetFavorite(bool fav)
	{
		_isFavorite = fav;
	}

	public bool IsFavorite()
	{
		return _isFavorite;
	}

	public string GetCategoryName()
	{
		return _categoryName;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Right && inputEventMouseButton.Pressed)
		{
			_menu.ResetSize();
			Vector2 globalMousePosition = GetGlobalMousePosition();
			_menu.Position = new Vector2I((int)globalMousePosition.X, (int)globalMousePosition.Y);
			_menu.Popup();
		}
	}

	private void OnMenuItem(long id)
	{
		switch ((ClassMenuOption)id)
		{
		case ClassMenuOption.MenuOpenDocs:
			OpenDocumentation();
			break;
		case ClassMenuOption.MenuUnfavoriteAll:
			UnfavoriteAll();
			break;
		}
	}

	private void OpenDocumentation()
	{
	}

	private void UnfavoriteAll()
	{
		XWInspector inspector = GetInspector();
		if (GodotObject.IsInstanceValid(inspector))
		{
			string[] favoriteProperties = inspector.GetFavoriteProperties();
			foreach (string propertyName in favoriteProperties)
			{
				inspector.UnfavoriteProperty(propertyName);
			}
			inspector.UpDateProperties();
		}
	}

	private XWInspector GetInspector()
	{
		for (Node parent = GetParent(); parent != null; parent = parent.GetParent())
		{
			if (parent is XWInspector result)
			{
				return result;
			}
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Setup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "pName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "pIcon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetFavorite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "fav", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFavorite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCategoryName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenDocumentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnfavoriteAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetInspector, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorCategory>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.Setup && args.Count == 2)
		{
			Setup(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFavorite && args.Count == 1)
		{
			SetFavorite(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsFavorite && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFavorite());
			return true;
		}
		if (method == MethodName.GetCategoryName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCategoryName());
			return true;
		}
		if (method == MethodName.OnGuiInput && args.Count == 1)
		{
			OnGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMenuItem && args.Count == 1)
		{
			OnMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenDocumentation && args.Count == 0)
		{
			OpenDocumentation();
			ret = default;
			return true;
		}
		if (method == MethodName.UnfavoriteAll && args.Count == 0)
		{
			UnfavoriteAll();
			ret = default;
			return true;
		}
		if (method == MethodName.GetInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(GetInspector());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorCategory>(Create());
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
		if (method == MethodName.Setup)
		{
			return true;
		}
		if (method == MethodName.SetFavorite)
		{
			return true;
		}
		if (method == MethodName.IsFavorite)
		{
			return true;
		}
		if (method == MethodName.GetCategoryName)
		{
			return true;
		}
		if (method == MethodName.OnGuiInput)
		{
			return true;
		}
		if (method == MethodName.OnMenuItem)
		{
			return true;
		}
		if (method == MethodName.OpenDocumentation)
		{
			return true;
		}
		if (method == MethodName.UnfavoriteAll)
		{
			return true;
		}
		if (method == MethodName.GetInspector)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._iconRect)
		{
			_iconRect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._label)
		{
			_label = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._categoryName)
		{
			_categoryName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._categoryIcon)
		{
			_categoryIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._isFavorite)
		{
			_isFavorite = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._iconRect)
		{
			value = VariantUtils.CreateFrom(in _iconRect);
			return true;
		}
		if (name == PropertyName._label)
		{
			value = VariantUtils.CreateFrom(in _label);
			return true;
		}
		if (name == PropertyName._categoryName)
		{
			value = VariantUtils.CreateFrom(in _categoryName);
			return true;
		}
		if (name == PropertyName._categoryIcon)
		{
			value = VariantUtils.CreateFrom(in _categoryIcon);
			return true;
		}
		if (name == PropertyName._isFavorite)
		{
			value = VariantUtils.CreateFrom(in _isFavorite);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._iconRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._label, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._categoryName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categoryIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isFavorite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._menu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._iconRect, Variant.From(in _iconRect));
		info.AddProperty(PropertyName._label, Variant.From(in _label));
		info.AddProperty(PropertyName._categoryName, Variant.From(in _categoryName));
		info.AddProperty(PropertyName._categoryIcon, Variant.From(in _categoryIcon));
		info.AddProperty(PropertyName._isFavorite, Variant.From(in _isFavorite));
		info.AddProperty(PropertyName._menu, Variant.From(in _menu));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._iconRect, out var value))
		{
			_iconRect = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._label, out var value2))
		{
			_label = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._categoryName, out var value3))
		{
			_categoryName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._categoryIcon, out var value4))
		{
			_categoryIcon = value4.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._isFavorite, out var value5))
		{
			_isFavorite = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._menu, out var value6))
		{
			_menu = value6.As<PopupMenu>();
		}
	}
}
