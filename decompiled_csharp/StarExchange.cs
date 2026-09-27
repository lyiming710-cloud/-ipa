using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/DialogBox/StarExchange/StarExchange.cs")]
public class StarExchange : DialogBoxBase
{
	public new class MethodName : DialogBoxBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddChapter = "AddChapter";

		public static readonly StringName BackButtonPressed = "BackButtonPressed";

		public static readonly StringName TryLevelButtonPressed = "TryLevelButtonPressed";
	}

	public new class PropertyName : DialogBoxBase.PropertyName
	{
		public static readonly StringName chapterContainer = "chapterContainer";
	}

	public new class SignalName : DialogBoxBase.SignalName
	{
	}

	private static Json _starExchangeResource;

	private static PackedScene _starExchangeContainner;

	private VBoxContainer chapterContainer;

	private static Json STAR_EXCHANGE_RESOURCE => _starExchangeResource ?? (_starExchangeResource = GD.Load<Json>("res://Asset/Config/StarExchange/StarExchangeResource.json"));

	private static PackedScene STAR_EXCHANGE_CONTAINNER => _starExchangeContainner ?? (_starExchangeContainner = GD.Load<PackedScene>("uid://clhvbc3i3p6tf"));

	public override void _Ready()
	{
		base._Ready();
		chapterContainer = GetNode<VBoxContainer>("%ChapterContainer");
		GetNode<BaseButton>("%BackButton").Pressed += BackButtonPressed;
		GetNode<BaseButton>("%TryLevelButton").Pressed += TryLevelButtonPressed;
		foreach (Variant item in ((Dictionary)STAR_EXCHANGE_RESOURCE.Data)["Exchange"].AsGodotArray())
		{
			AddChapter(item.AsGodotDictionary());
		}
	}

	public void AddChapter(Dictionary data)
	{
		StarExchangeContainner starExchangeContainner = (StarExchangeContainner)STAR_EXCHANGE_CONTAINNER.Instantiate(PackedScene.GenEditState.Disabled);
		chapterContainer.AddChild(starExchangeContainner, forceReadableName: false, InternalMode.Disabled);
		starExchangeContainner.Init(data);
	}

	public void BackButtonPressed()
	{
		CloseDialog();
	}

	public async void TryLevelButtonPressed()
	{
		AudioManager.Instance.AudioPlay("ButtonPress");
		Global.Instance.enterTryLevelGroup = "Star";
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("TryLevel", new Dictionary { { "openedFromStarExchange", true } });
		TowerDefenseManager.Instance.coinBank.CallDeferred("StartHide");
		Visible = false;
		await dialogBoxBase.WaitForClose();
		Visible = true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddChapter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BackButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryLevelButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.AddChapter && args.Count == 1)
		{
			AddChapter(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BackButtonPressed && args.Count == 0)
		{
			BackButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.TryLevelButtonPressed && args.Count == 0)
		{
			TryLevelButtonPressed();
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
		if (method == MethodName.AddChapter)
		{
			return true;
		}
		if (method == MethodName.BackButtonPressed)
		{
			return true;
		}
		if (method == MethodName.TryLevelButtonPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.chapterContainer)
		{
			chapterContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.chapterContainer)
		{
			value = VariantUtils.CreateFrom(in chapterContainer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.chapterContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.chapterContainer, Variant.From(in chapterContainer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.chapterContainer, out var value))
		{
			chapterContainer = value.As<VBoxContainer>();
		}
	}
}
