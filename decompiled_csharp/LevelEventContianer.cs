using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/LevelEditor/EventContianer/LevelEventContianer.cs")]
public class LevelEventContianer : PanelContainer
{
	public delegate void ChangeEventHandler();

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName UpButtonPressed = "UpButtonPressed";

		public static readonly StringName DeleteButtonPressed = "DeleteButtonPressed";

		public static readonly StringName SettingButtonPressed = "SettingButtonPressed";

		public static readonly StringName EventOptionButtonItemSelected = "EventOptionButtonItemSelected";

		public static readonly StringName FindOptionButtonIndex = "FindOptionButtonIndex";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName eventOptionButton = "eventOptionButton";

		public static readonly StringName inspector = "inspector";

		public static readonly StringName eventConfig = "eventConfig";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private OptionButton eventOptionButton;

	public LevelEditorInspector inspector;

	public TowerDefenseLevelEventBase eventConfig;

	public event ChangeEventHandler OnChange;

	public override void _Ready()
	{
		eventOptionButton = GetNode<OptionButton>("%EventOptionButton");
		LevelEditorDropdown.Configure(eventOptionButton);
		foreach (TowerDefenseLevelEventDefinition definition in TowerDefenseLevelEventRegistry.Definitions)
		{
			int itemCount = eventOptionButton.ItemCount;
			eventOptionButton.AddItem(definition.DisplayKey);
			eventOptionButton.SetItemMetadata(itemCount, definition.Id);
		}
		GetNode<MainButton>("VBoxContainer/HBoxContainer/UpButton").Pressed += UpButtonPressed;
		GetNode<MainButton>("VBoxContainer/HBoxContainer/DeleteButton").Pressed += DeleteButtonPressed;
		GetNode<MainButton>("VBoxContainer/HBoxContainer/SettingButton").Pressed += SettingButtonPressed;
		eventOptionButton.ItemSelected += EventOptionButtonItemSelected;
	}

	public void Init(TowerDefenseLevelEventBase _eventConfig)
	{
		eventConfig = _eventConfig;
		eventOptionButton.Selected = (TowerDefenseLevelEventRegistry.TryGetDefinition(eventConfig, out var definition) ? FindOptionButtonIndex(definition.Id) : (-1));
	}

	public void UpButtonPressed()
	{
		if (GetIndex() > 0)
		{
			GetParent().MoveChild(this, GetIndex() - 1);
		}
		OnChange?.Invoke();
	}

	public void DeleteButtonPressed()
	{
		GetParent()?.RemoveChild(this);
		QueueFree();
		OnChange?.Invoke();
	}

	public void SettingButtonPressed()
	{
		OnChange?.Invoke();
		if (eventOptionButton.Selected != -1)
		{
			inspector.Init(eventConfig, eventConfig.GetProperty());
		}
	}

	public void EventOptionButtonItemSelected(long index)
	{
		if (index >= 0 && index < eventOptionButton.ItemCount)
		{
			TowerDefenseLevelEventBase instance = TowerDefenseLevelEventRegistry.Create(eventOptionButton.GetItemMetadata((int)index).AsString());
			if (GodotObject.IsInstanceValid(instance))
			{
				eventConfig = instance;
				SettingButtonPressed();
			}
		}
	}

	private int FindOptionButtonIndex(string eventId)
	{
		for (int i = 0; i < eventOptionButton.ItemCount; i++)
		{
			if (eventOptionButton.GetItemMetadata(i).AsString() == eventId)
			{
				return i;
			}
		}
		return -1;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_eventConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeleteButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SettingButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EventOptionButtonItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindOptionButtonIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "eventId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelEventBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpButtonPressed && args.Count == 0)
		{
			UpButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DeleteButtonPressed && args.Count == 0)
		{
			DeleteButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.SettingButtonPressed && args.Count == 0)
		{
			SettingButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.EventOptionButtonItemSelected && args.Count == 1)
		{
			EventOptionButtonItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindOptionButtonIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindOptionButtonIndex(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.UpButtonPressed)
		{
			return true;
		}
		if (method == MethodName.DeleteButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SettingButtonPressed)
		{
			return true;
		}
		if (method == MethodName.EventOptionButtonItemSelected)
		{
			return true;
		}
		if (method == MethodName.FindOptionButtonIndex)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.eventOptionButton)
		{
			eventOptionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName.inspector)
		{
			inspector = VariantUtils.ConvertTo<LevelEditorInspector>(in value);
			return true;
		}
		if (name == PropertyName.eventConfig)
		{
			eventConfig = VariantUtils.ConvertTo<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.eventOptionButton)
		{
			value = VariantUtils.CreateFrom(in eventOptionButton);
			return true;
		}
		if (name == PropertyName.inspector)
		{
			value = VariantUtils.CreateFrom(in inspector);
			return true;
		}
		if (name == PropertyName.eventConfig)
		{
			value = VariantUtils.CreateFrom(in eventConfig);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.eventOptionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.eventConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventOptionButton, Variant.From(in eventOptionButton));
		info.AddProperty(PropertyName.inspector, Variant.From(in inspector));
		info.AddProperty(PropertyName.eventConfig, Variant.From(in eventConfig));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventOptionButton, out var value))
		{
			eventOptionButton = value.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName.inspector, out var value2))
		{
			inspector = value2.As<LevelEditorInspector>();
		}
		if (info.TryGetProperty(PropertyName.eventConfig, out var value3))
		{
			eventConfig = value3.As<TowerDefenseLevelEventBase>();
		}
	}
}
