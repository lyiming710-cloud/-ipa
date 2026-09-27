using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/EventEditor/LevelEditorEventEditor.cs")]
public class LevelEditorEventEditor : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName Init = "Init";

		public static readonly StringName Save = "Save";

		public static readonly StringName InitEventChange = "InitEventChange";

		public static readonly StringName ReadyEventChange = "ReadyEventChange";

		public static readonly StringName StartEventChange = "StartEventChange";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName levelConfig = "levelConfig";

		public static readonly StringName initLevelEventListContainer = "initLevelEventListContainer";

		public static readonly StringName readyLevelEventListContainer = "readyLevelEventListContainer";

		public static readonly StringName startEventListContainer = "startEventListContainer";

		public static readonly StringName inspector = "inspector";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private LevelEventListContainer initLevelEventListContainer;

	private LevelEventListContainer readyLevelEventListContainer;

	private LevelEventListContainer startEventListContainer;

	private LevelEditorInspector inspector;

	public static LevelEditorEventEditor Instance;

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelConfig levelConfig { get; set; }

	public override void _Ready()
	{
		initLevelEventListContainer = GetNode<LevelEventListContainer>("%InitLevelEventListContainer");
		readyLevelEventListContainer = GetNode<LevelEventListContainer>("%ReadyLevelEventListContainer");
		startEventListContainer = GetNode<LevelEventListContainer>("%StartEventListContainer");
		inspector = GetNode<LevelEditorInspector>("%LevelEditorInspector");
		initLevelEventListContainer.OnChange += InitEventChange;
		readyLevelEventListContainer.OnChange += ReadyEventChange;
		startEventListContainer.OnChange += StartEventChange;
		Instance = this;
		VisibilityChanged += Save;
	}

	public void Clear()
	{
		initLevelEventListContainer.Clear();
		readyLevelEventListContainer.Clear();
		startEventListContainer.Clear();
		inspector.Clear();
	}

	public void Init(TowerDefenseLevelConfig _levelConfig)
	{
		Clear();
		levelConfig = _levelConfig;
		initLevelEventListContainer.Init(levelConfig.eventInit);
		readyLevelEventListContainer.Init(levelConfig.eventReady);
		startEventListContainer.Init(levelConfig.eventStart);
		inspector.Clear();
	}

	public void Save()
	{
		if (GodotObject.IsInstanceValid(levelConfig))
		{
			levelConfig.eventInit = initLevelEventListContainer.GetEventList();
			levelConfig.eventReady = readyLevelEventListContainer.GetEventList();
			levelConfig.eventStart = startEventListContainer.GetEventList();
		}
	}

	public void InitEventChange()
	{
		levelConfig.canExport = false;
		levelConfig.eventInit = initLevelEventListContainer.GetEventList();
		levelConfig.MarkEventDataEditedFromEditor();
		inspector.Clear();
	}

	public void ReadyEventChange()
	{
		levelConfig.canExport = false;
		levelConfig.eventReady = readyLevelEventListContainer.GetEventList();
		levelConfig.MarkEventDataEditedFromEditor();
		inspector.Clear();
	}

	public void StartEventChange()
	{
		levelConfig.canExport = false;
		levelConfig.eventStart = startEventListContainer.GetEventList();
		levelConfig.MarkEventDataEditedFromEditor();
		inspector.Clear();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Save, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitEventChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadyEventChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartEventChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Save && args.Count == 0)
		{
			Save();
			ret = default;
			return true;
		}
		if (method == MethodName.InitEventChange && args.Count == 0)
		{
			InitEventChange();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadyEventChange && args.Count == 0)
		{
			ReadyEventChange();
			ret = default;
			return true;
		}
		if (method == MethodName.StartEventChange && args.Count == 0)
		{
			StartEventChange();
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
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Save)
		{
			return true;
		}
		if (method == MethodName.InitEventChange)
		{
			return true;
		}
		if (method == MethodName.ReadyEventChange)
		{
			return true;
		}
		if (method == MethodName.StartEventChange)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.levelConfig)
		{
			levelConfig = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName.initLevelEventListContainer)
		{
			initLevelEventListContainer = VariantUtils.ConvertTo<LevelEventListContainer>(in value);
			return true;
		}
		if (name == PropertyName.readyLevelEventListContainer)
		{
			readyLevelEventListContainer = VariantUtils.ConvertTo<LevelEventListContainer>(in value);
			return true;
		}
		if (name == PropertyName.startEventListContainer)
		{
			startEventListContainer = VariantUtils.ConvertTo<LevelEventListContainer>(in value);
			return true;
		}
		if (name == PropertyName.inspector)
		{
			inspector = VariantUtils.ConvertTo<LevelEditorInspector>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.levelConfig)
		{
			value = VariantUtils.CreateFrom<TowerDefenseLevelConfig>(levelConfig);
			return true;
		}
		if (name == PropertyName.initLevelEventListContainer)
		{
			value = VariantUtils.CreateFrom(in initLevelEventListContainer);
			return true;
		}
		if (name == PropertyName.readyLevelEventListContainer)
		{
			value = VariantUtils.CreateFrom(in readyLevelEventListContainer);
			return true;
		}
		if (name == PropertyName.startEventListContainer)
		{
			value = VariantUtils.CreateFrom(in startEventListContainer);
			return true;
		}
		if (name == PropertyName.inspector)
		{
			value = VariantUtils.CreateFrom(in inspector);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.initLevelEventListContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.readyLevelEventListContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.startEventListContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelConfig, PropertyHint.ResourceType, "TowerDefenseLevelConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.levelConfig, Variant.From<TowerDefenseLevelConfig>(levelConfig));
		info.AddProperty(PropertyName.initLevelEventListContainer, Variant.From(in initLevelEventListContainer));
		info.AddProperty(PropertyName.readyLevelEventListContainer, Variant.From(in readyLevelEventListContainer));
		info.AddProperty(PropertyName.startEventListContainer, Variant.From(in startEventListContainer));
		info.AddProperty(PropertyName.inspector, Variant.From(in inspector));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.levelConfig, out var value))
		{
			levelConfig = value.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName.initLevelEventListContainer, out var value2))
		{
			initLevelEventListContainer = value2.As<LevelEventListContainer>();
		}
		if (info.TryGetProperty(PropertyName.readyLevelEventListContainer, out var value3))
		{
			readyLevelEventListContainer = value3.As<LevelEventListContainer>();
		}
		if (info.TryGetProperty(PropertyName.startEventListContainer, out var value4))
		{
			startEventListContainer = value4.As<LevelEventListContainer>();
		}
		if (info.TryGetProperty(PropertyName.inspector, out var value5))
		{
			inspector = value5.As<LevelEditorInspector>();
		}
	}
}
