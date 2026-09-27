using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationVisualChoiceGrid.cs")]
public class XWAnimationVisualChoiceGrid : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SelectChoice = "SelectChoice";

		public static readonly StringName SetChoicesEnabled = "SetChoicesEnabled";

		public static readonly StringName ClearChoices = "ClearChoices";

		public static readonly StringName OnCardPressed = "OnCardPressed";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName ChoiceCount = "ChoiceCount";

		public static readonly StringName SelectedChoiceId = "SelectedChoiceId";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _emptyLabel = "_emptyLabel";

		public static readonly StringName _choiceHost = "_choiceHost";

		public static readonly StringName _selectedChoiceId = "_selectedChoiceId";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private const string CardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationVisualChoiceCard.tscn";

	private static PackedScene _cardScene;

	private readonly List<XWAnimationVisualChoiceCard> _cards = new List<XWAnimationVisualChoiceCard>();

	private Label _titleLabel;

	private Label _summaryLabel;

	private Label _emptyLabel;

	private HFlowContainer _choiceHost;

	private int _selectedChoiceId = -1;

	public int ChoiceCount => _cards.Count;

	public int SelectedChoiceId => _selectedChoiceId;

	public event Action<int> ChoiceSelected;

	public override void _Ready()
	{
		_titleLabel = GetNode<Label>("%ChoiceTitle");
		_summaryLabel = GetNode<Label>("%ChoiceSummary");
		_emptyLabel = GetNode<Label>("%EmptyLabel");
		_choiceHost = GetNode<HFlowContainer>("%ChoiceHost");
	}

	public void Configure(string title, IEnumerable<XWAnimationVisualChoice> choices, int selectedId, bool enabled)
	{
		ClearChoices();
		_titleLabel.Text = title ?? "";
		if (_cardScene == null)
		{
			_cardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAnimationVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (choices != null)
		{
			foreach (XWAnimationVisualChoice choice in choices)
			{
				XWAnimationVisualChoiceCard xWAnimationVisualChoiceCard = _cardScene?.Instantiate<XWAnimationVisualChoiceCard>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(xWAnimationVisualChoiceCard))
				{
					_choiceHost.AddChild(xWAnimationVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
					xWAnimationVisualChoiceCard.Configure(choice.Id, choice.Title, choice.Subtitle, choice.Icon, choice.Accent);
					xWAnimationVisualChoiceCard.Disabled = !enabled;
					xWAnimationVisualChoiceCard.ChoicePressed += OnCardPressed;
					_cards.Add(xWAnimationVisualChoiceCard);
				}
			}
		}
		_summaryLabel.Text = ((_cards.Count == 0) ? "无可用项" : $"{_cards.Count} 个视觉选项");
		_emptyLabel.Visible = _cards.Count == 0;
		SelectChoice(selectedId, emitSignal: false);
	}

	public bool SelectChoice(int choiceId, bool emitSignal)
	{
		bool flag = false;
		foreach (XWAnimationVisualChoiceCard card in _cards)
		{
			bool flag2 = card.ChoiceId == choiceId;
			card.SetSelected(flag2);
			flag |= flag2;
		}
		if (!flag)
		{
			return false;
		}
		_selectedChoiceId = choiceId;
		if (emitSignal)
		{
			ChoiceSelected?.Invoke(choiceId);
		}
		return true;
	}

	public void SetChoicesEnabled(bool enabled)
	{
		foreach (XWAnimationVisualChoiceCard card in _cards)
		{
			card.Disabled = !enabled;
		}
	}

	public void ClearChoices()
	{
		foreach (XWAnimationVisualChoiceCard card in _cards)
		{
			if (GodotObject.IsInstanceValid(card))
			{
				card.ChoicePressed -= OnCardPressed;
				if (card.GetParent() == _choiceHost)
				{
					_choiceHost.RemoveChild(card);
				}
				card.QueueFree();
			}
		}
		_cards.Clear();
		_selectedChoiceId = -1;
		if (GodotObject.IsInstanceValid(_emptyLabel))
		{
			_emptyLabel.Visible = true;
		}
	}

	private void OnCardPressed(int choiceId)
	{
		SelectChoice(choiceId, emitSignal: true);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectChoice, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "choiceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "emitSignal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetChoicesEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCardPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "choiceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SelectChoice && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectChoice(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.SetChoicesEnabled && args.Count == 1)
		{
			SetChoicesEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearChoices && args.Count == 0)
		{
			ClearChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCardPressed && args.Count == 1)
		{
			OnCardPressed(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.SelectChoice)
		{
			return true;
		}
		if (method == MethodName.SetChoicesEnabled)
		{
			return true;
		}
		if (method == MethodName.ClearChoices)
		{
			return true;
		}
		if (method == MethodName.OnCardPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			_emptyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._choiceHost)
		{
			_choiceHost = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._selectedChoiceId)
		{
			_selectedChoiceId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.ChoiceCount)
		{
			from = ChoiceCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SelectedChoiceId)
		{
			from = SelectedChoiceId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			value = VariantUtils.CreateFrom(in _emptyLabel);
			return true;
		}
		if (name == PropertyName._choiceHost)
		{
			value = VariantUtils.CreateFrom(in _choiceHost);
			return true;
		}
		if (name == PropertyName._selectedChoiceId)
		{
			value = VariantUtils.CreateFrom(in _selectedChoiceId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._choiceHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedChoiceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ChoiceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedChoiceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._emptyLabel, Variant.From(in _emptyLabel));
		info.AddProperty(PropertyName._choiceHost, Variant.From(in _choiceHost));
		info.AddProperty(PropertyName._selectedChoiceId, Variant.From(in _selectedChoiceId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._titleLabel, out var value))
		{
			_titleLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value2))
		{
			_summaryLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._emptyLabel, out var value3))
		{
			_emptyLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._choiceHost, out var value4))
		{
			_choiceHost = value4.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._selectedChoiceId, out var value5))
		{
			_selectedChoiceId = value5.As<int>();
		}
	}
}
