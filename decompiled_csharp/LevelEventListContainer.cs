using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/LevelEditor/EventContianer/LevelEventListContainer.cs")]
public class LevelEventListContainer : ScrollContainer
{
	public delegate void ChangeEventHandler();

	public new class MethodName : ScrollContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName AddEvent = "AddEvent";

		public static readonly StringName HandleChange = "HandleChange";

		public static readonly StringName GetEventList = "GetEventList";
	}

	public new class PropertyName : ScrollContainer.PropertyName
	{
		public static readonly StringName inspector = "inspector";

		public static readonly StringName eventContainer = "eventContainer";

		public static readonly StringName isInit = "isInit";
	}

	public new class SignalName : ScrollContainer.SignalName
	{
	}

	private static PackedScene _levelEventContianerScene;

	private VBoxContainer eventContainer;

	public bool isInit;

	private static PackedScene LevelEventContianerScene => _levelEventContianerScene ?? (_levelEventContianerScene = GD.Load<PackedScene>("uid://b617wjctwpjyw"));

	[Export(PropertyHint.None, "")]
	public LevelEditorInspector inspector { get; set; }

	public event ChangeEventHandler OnChange;

	public override void _Ready()
	{
		eventContainer = GetNode<VBoxContainer>("%EventContainer");
		GetNode<MainButton>("VboxContainer/AddEventButton").Pressed += () =>
		{
			AddEvent();
		};
	}

	public void Init(Array<TowerDefenseLevelEventBase> eventList)
	{
		isInit = true;
		Clear();
		foreach (TowerDefenseLevelEventBase @event in eventList)
		{
			AddEvent(@event);
		}
		isInit = false;
	}

	public void Clear()
	{
		foreach (Node child in eventContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void AddEvent(TowerDefenseLevelEventBase eventConfig = null)
	{
		LevelEventContianer levelEventContianer = LevelEventContianerScene.Instantiate<LevelEventContianer>(PackedScene.GenEditState.Disabled);
		eventContainer.AddChild(levelEventContianer, forceReadableName: false, InternalMode.Disabled);
		levelEventContianer.inspector = inspector;
		levelEventContianer.OnChange += HandleChange;
		if (eventConfig != null)
		{
			levelEventContianer.Init(eventConfig);
		}
		else
		{
			levelEventContianer.Init(TowerDefenseLevelEventRegistry.Create("TipsPlay"));
		}
		HandleChange();
	}

	public void HandleChange()
	{
		if (!isInit)
		{
			OnChange?.Invoke();
		}
	}

	public Array<TowerDefenseLevelEventBase> GetEventList()
	{
		Array<TowerDefenseLevelEventBase> array = new Array<TowerDefenseLevelEventBase>();
		foreach (Node child in eventContainer.GetChildren())
		{
			if (child is LevelEventContianer levelEventContianer)
			{
				array.Add(levelEventContianer.eventConfig);
			}
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "eventList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "eventConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HandleChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEventList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
			Init(VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.AddEvent && args.Count == 1)
		{
			AddEvent(VariantUtils.ConvertTo<TowerDefenseLevelEventBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleChange && args.Count == 0)
		{
			HandleChange();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEventList && args.Count == 0)
		{
			Array<TowerDefenseLevelEventBase> eventList = GetEventList();
			ret = VariantUtils.CreateFromArray(eventList);
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
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.AddEvent)
		{
			return true;
		}
		if (method == MethodName.HandleChange)
		{
			return true;
		}
		if (method == MethodName.GetEventList)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.inspector)
		{
			inspector = VariantUtils.ConvertTo<LevelEditorInspector>(in value);
			return true;
		}
		if (name == PropertyName.eventContainer)
		{
			eventContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.inspector)
		{
			value = VariantUtils.CreateFrom<LevelEditorInspector>(inspector);
			return true;
		}
		if (name == PropertyName.eventContainer)
		{
			value = VariantUtils.CreateFrom(in eventContainer);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			value = VariantUtils.CreateFrom(in isInit);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.inspector, PropertyHint.NodeType, "LevelEditorInspector", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.eventContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.inspector, Variant.From<LevelEditorInspector>(inspector));
		info.AddProperty(PropertyName.eventContainer, Variant.From(in eventContainer));
		info.AddProperty(PropertyName.isInit, Variant.From(in isInit));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.inspector, out var value))
		{
			inspector = value.As<LevelEditorInspector>();
		}
		if (info.TryGetProperty(PropertyName.eventContainer, out var value2))
		{
			eventContainer = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.isInit, out var value3))
		{
			isInit = value3.As<bool>();
		}
	}
}
