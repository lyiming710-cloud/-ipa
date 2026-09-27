using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationReplacementCard.cs")]
public class XWAnimationReplacementCard : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName BindEmpty = "BindEmpty";

		public static readonly StringName ShowMode = "ShowMode";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _slotPanel = "_slotPanel";

		public static readonly StringName _slotThumbnail = "_slotThumbnail";

		public static readonly StringName _slotName = "_slotName";

		public static readonly StringName _slotMediaId = "_slotMediaId";

		public static readonly StringName _slotLayers = "_slotLayers";

		public static readonly StringName _slotRemoveButton = "_slotRemoveButton";

		public static readonly StringName _texturePanel = "_texturePanel";

		public static readonly StringName _texturePreview = "_texturePreview";

		public static readonly StringName _textureName = "_textureName";

		public static readonly StringName _textureReplaceButton = "_textureReplaceButton";

		public static readonly StringName _textureRemoveButton = "_textureRemoveButton";

		public static readonly StringName _emptyPanel = "_emptyPanel";

		public static readonly StringName _emptyGlyph = "_emptyGlyph";

		public static readonly StringName _emptyTitle = "_emptyTitle";

		public static readonly StringName _emptySubtitle = "_emptySubtitle";

		public static readonly StringName _mediaName = "_mediaName";

		public static readonly StringName _textureIndex = "_textureIndex";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private Control _slotPanel;

	private TextureRect _slotThumbnail;

	private Label _slotName;

	private Label _slotMediaId;

	private Label _slotLayers;

	private Button _slotRemoveButton;

	private Control _texturePanel;

	private TextureRect _texturePreview;

	private Label _textureName;

	private Button _textureReplaceButton;

	private Button _textureRemoveButton;

	private Control _emptyPanel;

	private Label _emptyGlyph;

	private Label _emptyTitle;

	private Label _emptySubtitle;

	private Action<string> _removeSlot;

	private Action<int> _replaceTexture;

	private Action<int> _removeTexture;

	private string _mediaName = "";

	private int _textureIndex = -1;

	public override void _Ready()
	{
		_slotPanel = GetNode<Control>("%SlotPanel");
		_slotThumbnail = GetNode<TextureRect>("%SlotThumbnail");
		_slotName = GetNode<Label>("%SlotName");
		_slotMediaId = GetNode<Label>("%SlotMediaId");
		_slotLayers = GetNode<Label>("%SlotLayers");
		_slotRemoveButton = GetNode<Button>("%SlotRemoveButton");
		_texturePanel = GetNode<Control>("%TexturePanel");
		_texturePreview = GetNode<TextureRect>("%TexturePreview");
		_textureName = GetNode<Label>("%TextureName");
		_textureReplaceButton = GetNode<Button>("%TextureReplaceButton");
		_textureRemoveButton = GetNode<Button>("%TextureRemoveButton");
		_emptyPanel = GetNode<Control>("%EmptyPanel");
		_emptyGlyph = GetNode<Label>("%EmptyGlyph");
		_emptyTitle = GetNode<Label>("%EmptyTitle");
		_emptySubtitle = GetNode<Label>("%EmptySubtitle");
		_slotRemoveButton.Pressed += () =>
		{
			_removeSlot?.Invoke(_mediaName);
		};
		_textureReplaceButton.Pressed += () =>
		{
			_replaceTexture?.Invoke(_textureIndex);
		};
		_textureRemoveButton.Pressed += () =>
		{
			_removeTexture?.Invoke(_textureIndex);
		};
	}

	public void BindSlot(string mediaName, int mediaId, Texture2D thumbnail, string layerSummary, bool mediaExists, Action<string> removeSlot)
	{
		ShowMode(_slotPanel, new Vector2(280f, 116f));
		_mediaName = mediaName ?? "";
		_removeSlot = removeSlot;
		_slotThumbnail.Texture = thumbnail;
		_slotThumbnail.TooltipText = (mediaExists ? $"原始媒体 #{mediaId} · {_mediaName}" : "原始媒体已不存在");
		_slotName.Text = _mediaName;
		_slotName.TooltipText = _mediaName;
		_slotName.Modulate = (mediaExists ? new Color(0.9f, 1f, 0.72f) : new Color(1f, 0.42f, 0.34f));
		_slotMediaId.Text = (mediaExists ? $"媒体 #{mediaId + 1}" : "媒体已不存在");
		_slotLayers.Text = (mediaExists ? layerSummary : "可删除此失效槽位");
		_slotLayers.TooltipText = (mediaExists ? layerSummary : "");
		_slotRemoveButton.TooltipText = "删除 " + _mediaName + " 替换槽位";
	}

	public void BindTexture(Texture2D texture, int index, string displayName, string resourcePath, Action<int> replaceTexture, Action<int> removeTexture)
	{
		ShowMode(_texturePanel, new Vector2(218f, 176f));
		_textureIndex = index;
		_replaceTexture = replaceTexture;
		_removeTexture = removeTexture;
		_texturePreview.Texture = texture;
		_texturePreview.TooltipText = (string.IsNullOrWhiteSpace(resourcePath) ? "内嵌纹理" : resourcePath);
		_textureName.Text = $"#{index + 1}  {displayName}";
		_textureName.TooltipText = _texturePreview.TooltipText;
	}

	public void BindEmpty(string glyph, string title, string subtitle)
	{
		ShowMode(_emptyPanel, new Vector2(270f, 108f));
		_emptyGlyph.Text = glyph ?? "";
		_emptyTitle.Text = title ?? "";
		_emptySubtitle.Text = subtitle ?? "";
	}

	private void ShowMode(Control requestedPanel, Vector2 minimumSize)
	{
		CustomMinimumSize = minimumSize;
		_slotPanel.Visible = requestedPanel == _slotPanel;
		_texturePanel.Visible = requestedPanel == _texturePanel;
		_emptyPanel.Visible = requestedPanel == _emptyPanel;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "glyph", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "requestedPanel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "minimumSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BindEmpty && args.Count == 3)
		{
			BindEmpty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowMode && args.Count == 2)
		{
			ShowMode(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
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
		if (method == MethodName.BindEmpty)
		{
			return true;
		}
		if (method == MethodName.ShowMode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._slotPanel)
		{
			_slotPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._slotThumbnail)
		{
			_slotThumbnail = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._slotName)
		{
			_slotName = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._slotMediaId)
		{
			_slotMediaId = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._slotLayers)
		{
			_slotLayers = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._slotRemoveButton)
		{
			_slotRemoveButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._texturePanel)
		{
			_texturePanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			_texturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._textureName)
		{
			_textureName = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._textureReplaceButton)
		{
			_textureReplaceButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._textureRemoveButton)
		{
			_textureRemoveButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._emptyPanel)
		{
			_emptyPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._emptyGlyph)
		{
			_emptyGlyph = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._emptyTitle)
		{
			_emptyTitle = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._emptySubtitle)
		{
			_emptySubtitle = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mediaName)
		{
			_mediaName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._textureIndex)
		{
			_textureIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._slotPanel)
		{
			value = VariantUtils.CreateFrom(in _slotPanel);
			return true;
		}
		if (name == PropertyName._slotThumbnail)
		{
			value = VariantUtils.CreateFrom(in _slotThumbnail);
			return true;
		}
		if (name == PropertyName._slotName)
		{
			value = VariantUtils.CreateFrom(in _slotName);
			return true;
		}
		if (name == PropertyName._slotMediaId)
		{
			value = VariantUtils.CreateFrom(in _slotMediaId);
			return true;
		}
		if (name == PropertyName._slotLayers)
		{
			value = VariantUtils.CreateFrom(in _slotLayers);
			return true;
		}
		if (name == PropertyName._slotRemoveButton)
		{
			value = VariantUtils.CreateFrom(in _slotRemoveButton);
			return true;
		}
		if (name == PropertyName._texturePanel)
		{
			value = VariantUtils.CreateFrom(in _texturePanel);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			value = VariantUtils.CreateFrom(in _texturePreview);
			return true;
		}
		if (name == PropertyName._textureName)
		{
			value = VariantUtils.CreateFrom(in _textureName);
			return true;
		}
		if (name == PropertyName._textureReplaceButton)
		{
			value = VariantUtils.CreateFrom(in _textureReplaceButton);
			return true;
		}
		if (name == PropertyName._textureRemoveButton)
		{
			value = VariantUtils.CreateFrom(in _textureRemoveButton);
			return true;
		}
		if (name == PropertyName._emptyPanel)
		{
			value = VariantUtils.CreateFrom(in _emptyPanel);
			return true;
		}
		if (name == PropertyName._emptyGlyph)
		{
			value = VariantUtils.CreateFrom(in _emptyGlyph);
			return true;
		}
		if (name == PropertyName._emptyTitle)
		{
			value = VariantUtils.CreateFrom(in _emptyTitle);
			return true;
		}
		if (name == PropertyName._emptySubtitle)
		{
			value = VariantUtils.CreateFrom(in _emptySubtitle);
			return true;
		}
		if (name == PropertyName._mediaName)
		{
			value = VariantUtils.CreateFrom(in _mediaName);
			return true;
		}
		if (name == PropertyName._textureIndex)
		{
			value = VariantUtils.CreateFrom(in _textureIndex);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._slotPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._slotThumbnail, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._slotName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._slotMediaId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._slotLayers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._slotRemoveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureReplaceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureRemoveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyGlyph, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyTitle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptySubtitle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._mediaName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._textureIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._slotPanel, Variant.From(in _slotPanel));
		info.AddProperty(PropertyName._slotThumbnail, Variant.From(in _slotThumbnail));
		info.AddProperty(PropertyName._slotName, Variant.From(in _slotName));
		info.AddProperty(PropertyName._slotMediaId, Variant.From(in _slotMediaId));
		info.AddProperty(PropertyName._slotLayers, Variant.From(in _slotLayers));
		info.AddProperty(PropertyName._slotRemoveButton, Variant.From(in _slotRemoveButton));
		info.AddProperty(PropertyName._texturePanel, Variant.From(in _texturePanel));
		info.AddProperty(PropertyName._texturePreview, Variant.From(in _texturePreview));
		info.AddProperty(PropertyName._textureName, Variant.From(in _textureName));
		info.AddProperty(PropertyName._textureReplaceButton, Variant.From(in _textureReplaceButton));
		info.AddProperty(PropertyName._textureRemoveButton, Variant.From(in _textureRemoveButton));
		info.AddProperty(PropertyName._emptyPanel, Variant.From(in _emptyPanel));
		info.AddProperty(PropertyName._emptyGlyph, Variant.From(in _emptyGlyph));
		info.AddProperty(PropertyName._emptyTitle, Variant.From(in _emptyTitle));
		info.AddProperty(PropertyName._emptySubtitle, Variant.From(in _emptySubtitle));
		info.AddProperty(PropertyName._mediaName, Variant.From(in _mediaName));
		info.AddProperty(PropertyName._textureIndex, Variant.From(in _textureIndex));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._slotPanel, out var value))
		{
			_slotPanel = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._slotThumbnail, out var value2))
		{
			_slotThumbnail = value2.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._slotName, out var value3))
		{
			_slotName = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._slotMediaId, out var value4))
		{
			_slotMediaId = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._slotLayers, out var value5))
		{
			_slotLayers = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._slotRemoveButton, out var value6))
		{
			_slotRemoveButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._texturePanel, out var value7))
		{
			_texturePanel = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._texturePreview, out var value8))
		{
			_texturePreview = value8.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._textureName, out var value9))
		{
			_textureName = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._textureReplaceButton, out var value10))
		{
			_textureReplaceButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._textureRemoveButton, out var value11))
		{
			_textureRemoveButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._emptyPanel, out var value12))
		{
			_emptyPanel = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._emptyGlyph, out var value13))
		{
			_emptyGlyph = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._emptyTitle, out var value14))
		{
			_emptyTitle = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._emptySubtitle, out var value15))
		{
			_emptySubtitle = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mediaName, out var value16))
		{
			_mediaName = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName._textureIndex, out var value17))
		{
			_textureIndex = value17.As<int>();
		}
	}
}
