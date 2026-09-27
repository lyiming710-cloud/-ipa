using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWLevelGameCanvas.cs")]
public class XWLevelGameCanvas : VBoxContainer
{
	[Signal]
	public delegate void LevelEditedEventHandler();

	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EditLevel = "EditLevel";

		public static readonly StringName ReloadFromResource = "ReloadFromResource";

		public static readonly StringName CommitMapEdits = "CommitMapEdits";

		public static readonly StringName UpdateStatus = "UpdateStatus";

		public static readonly StringName HandleVisibilityChanged = "HandleVisibilityChanged";

		public static readonly StringName RefreshVisibilityGatedProcessing = "RefreshVisibilityGatedProcessing";

		public static readonly StringName ResolveMapConfig = "ResolveMapConfig";

		public static readonly StringName DisplayValue = "DisplayValue";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName _level = "_level";

		public static readonly StringName _runtimeMapEditor = "_runtimeMapEditor";

		public static readonly StringName _mapLabel = "_mapLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _initialized = "_initialized";

		public static readonly StringName _committing = "_committing";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
		public static readonly StringName LevelEdited = "LevelEdited";
	}

	private const string DefaultMapConfigPath = "res://Asset/Config/Map/Frontlawn/Config/FrontlawnMapFrontlawn.tres";

	private TowerDefenseLevelConfig _level;

	private LevelEditorMapEditor _runtimeMapEditor;

	private Label _mapLabel;

	private Label _statusLabel;

	private bool _initialized;

	private bool _committing;

	private LevelEditedEventHandler backing_LevelEdited;

	public event LevelEditedEventHandler LevelEdited
	{
		add
		{
			backing_LevelEdited = (LevelEditedEventHandler)Delegate.Combine(backing_LevelEdited, value);
		}
		remove
		{
			backing_LevelEdited = (LevelEditedEventHandler)Delegate.Remove(backing_LevelEdited, value);
		}
	}

	public override void _Ready()
	{
		_runtimeMapEditor = GetNode<LevelEditorMapEditor>("%RuntimeMapEditor");
		_mapLabel = GetNode<Label>("%MapLabel");
		_statusLabel = GetNode<Label>("%Status");
		GetNode<Button>("%ReloadButton").Connect(BaseButton.SignalName.Pressed, new Callable(this, "ReloadFromResource"));
		GetNode<Button>("%CommitButton").Connect(BaseButton.SignalName.Pressed, new Callable(this, "CommitMapEdits"));
		VisibilityChanged += HandleVisibilityChanged;
		RefreshVisibilityGatedProcessing();
		if (GodotObject.IsInstanceValid(_level))
		{
			ReloadFromResource();
		}
	}

	public override void _Process(double delta)
	{
		if (_initialized && !_committing && GodotObject.IsInstanceValid(_level) && !_level.canExport)
		{
			_level.canExport = true;
			_level.EmitChanged();
			UpdateStatus("画面已修改，等待写回布局或使用资源编辑器保存");
			EmitSignal(SignalName.LevelEdited);
		}
	}

	public override void _ExitTree()
	{
		VisibilityChanged -= HandleVisibilityChanged;
		SetProcess(enable: false);
		if (GodotObject.IsInstanceValid(_runtimeMapEditor))
		{
			_runtimeMapEditor.ProcessMode = ProcessModeEnum.Disabled;
		}
		if (_initialized && GodotObject.IsInstanceValid(_level) && GodotObject.IsInstanceValid(_runtimeMapEditor))
		{
			CommitMapEdits();
		}
		base._ExitTree();
	}

	public void EditLevel(TowerDefenseLevelConfig level)
	{
		_level = level;
		if (IsNodeReady())
		{
			ReloadFromResource();
		}
	}

	public void ReloadFromResource()
	{
		_initialized = false;
		RefreshVisibilityGatedProcessing();
		if (!GodotObject.IsInstanceValid(_level) || !GodotObject.IsInstanceValid(_runtimeMapEditor))
		{
			return;
		}
		_runtimeMapEditor.ClearCharacter();
		_runtimeMapEditor.Init(_level);
		TowerDefenseMapConfig towerDefenseMapConfig = ResolveMapConfig(_level.map);
		if (!GodotObject.IsInstanceValid(towerDefenseMapConfig))
		{
			_mapLabel.Text = "地图：" + DisplayValue(_level.map);
			_statusLabel.Text = "无法加载地图配置，请在关卡游戏画面的“地图”资源卡中选择有效地图。";
			return;
		}
		TowerDefenseBattleFeatureMap mapFeature = _runtimeMapEditor.mapFeature;
		mapFeature.editorPreviewMode = true;
		bool flag;
		if (GodotObject.IsInstanceValid(mapFeature.currentMap))
		{
			mapFeature.MapChange(towerDefenseMapConfig);
			flag = true;
		}
		else
		{
			flag = mapFeature.MapInit(towerDefenseMapConfig);
		}
		if (!flag)
		{
			_statusLabel.Text = "地图运行画面加载失败：" + DisplayValue(_level.map);
			return;
		}
		_runtimeMapEditor.ApplyMapPreviewConfig(towerDefenseMapConfig);
		_runtimeMapEditor.Save(isSave: false);
		_level.canExport = true;
		_initialized = true;
		RefreshVisibilityGatedProcessing();
		_mapLabel.Text = $"地图：{DisplayValue(_level.map)} · {towerDefenseMapConfig.gridNum.X}×{towerDefenseMapConfig.gridNum.Y}";
		UpdateStatus("已载入真实游戏地图；从卡牌库选择单位后直接放置到格子");
	}

	public void CommitMapEdits()
	{
		if (_initialized && !_committing && GodotObject.IsInstanceValid(_level) && GodotObject.IsInstanceValid(_runtimeMapEditor))
		{
			_committing = true;
			_runtimeMapEditor.Save(isSave: true);
			_level.canExport = true;
			_level.EmitChanged();
			UpdateStatus($"已从游戏画面写回 {_level.preSpawnList?.Count ?? 0} 个预生成单位");
			EmitSignal(SignalName.LevelEdited);
			_committing = false;
		}
	}

	private void UpdateStatus(string message)
	{
		if (GodotObject.IsInstanceValid(_statusLabel))
		{
			_statusLabel.Text = $"{message} · 当前 preSpawn {(_level?.preSpawnList?.Count).GetValueOrDefault()}";
		}
	}

	private void HandleVisibilityChanged()
	{
		RefreshVisibilityGatedProcessing();
	}

	private void RefreshVisibilityGatedProcessing()
	{
		bool flag = IsVisibleInTree();
		SetProcess(_initialized & flag);
		if (GodotObject.IsInstanceValid(_runtimeMapEditor))
		{
			_runtimeMapEditor.ProcessMode = (ProcessModeEnum)((_initialized & flag) ? 0 : 4);
		}
	}

	private static TowerDefenseMapConfig ResolveMapConfig(string mapValue)
	{
		if (!string.IsNullOrWhiteSpace(mapValue) && ResourceLoader.Exists(mapValue))
		{
			TowerDefenseMapConfig towerDefenseMapConfig = ResourceLoader.Load<TowerDefenseMapConfig>(mapValue, null, ResourceLoader.CacheMode.Reuse);
			if (GodotObject.IsInstanceValid(towerDefenseMapConfig))
			{
				return towerDefenseMapConfig;
			}
		}
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseMapConfig mapConfig = TowerDefenseManager.Instance.GetMapConfig(mapValue);
			if (GodotObject.IsInstanceValid(mapConfig))
			{
				return mapConfig;
			}
		}
		return ResourceLoader.Load<TowerDefenseMapConfig>("res://Asset/Config/Map/Frontlawn/Config/FrontlawnMapFrontlawn.tres", null, ResourceLoader.CacheMode.Reuse);
	}

	private static string DisplayValue(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "Frontlawn";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EditLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReloadFromResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitMapEdits, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandleVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshVisibilityGatedProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveMapConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "mapValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisplayValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.EditLevel && args.Count == 1)
		{
			EditLevel(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReloadFromResource && args.Count == 0)
		{
			ReloadFromResource();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitMapEdits && args.Count == 0)
		{
			CommitMapEdits();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateStatus && args.Count == 1)
		{
			UpdateStatus(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleVisibilityChanged && args.Count == 0)
		{
			HandleVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisibilityGatedProcessing && args.Count == 0)
		{
			RefreshVisibilityGatedProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveMapConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(ResolveMapConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DisplayValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveMapConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMapConfig>(ResolveMapConfig(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DisplayValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayValue(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.EditLevel)
		{
			return true;
		}
		if (method == MethodName.ReloadFromResource)
		{
			return true;
		}
		if (method == MethodName.CommitMapEdits)
		{
			return true;
		}
		if (method == MethodName.UpdateStatus)
		{
			return true;
		}
		if (method == MethodName.HandleVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshVisibilityGatedProcessing)
		{
			return true;
		}
		if (method == MethodName.ResolveMapConfig)
		{
			return true;
		}
		if (method == MethodName.DisplayValue)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._level)
		{
			_level = VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in value);
			return true;
		}
		if (name == PropertyName._runtimeMapEditor)
		{
			_runtimeMapEditor = VariantUtils.ConvertTo<LevelEditorMapEditor>(in value);
			return true;
		}
		if (name == PropertyName._mapLabel)
		{
			_mapLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			_initialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._committing)
		{
			_committing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._level)
		{
			value = VariantUtils.CreateFrom(in _level);
			return true;
		}
		if (name == PropertyName._runtimeMapEditor)
		{
			value = VariantUtils.CreateFrom(in _runtimeMapEditor);
			return true;
		}
		if (name == PropertyName._mapLabel)
		{
			value = VariantUtils.CreateFrom(in _mapLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._initialized)
		{
			value = VariantUtils.CreateFrom(in _initialized);
			return true;
		}
		if (name == PropertyName._committing)
		{
			value = VariantUtils.CreateFrom(in _committing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._level, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeMapEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._initialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._committing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._level, Variant.From(in _level));
		info.AddProperty(PropertyName._runtimeMapEditor, Variant.From(in _runtimeMapEditor));
		info.AddProperty(PropertyName._mapLabel, Variant.From(in _mapLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._initialized, Variant.From(in _initialized));
		info.AddProperty(PropertyName._committing, Variant.From(in _committing));
		info.AddSignalEventDelegate(SignalName.LevelEdited, backing_LevelEdited);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._level, out var value))
		{
			_level = value.As<TowerDefenseLevelConfig>();
		}
		if (info.TryGetProperty(PropertyName._runtimeMapEditor, out var value2))
		{
			_runtimeMapEditor = value2.As<LevelEditorMapEditor>();
		}
		if (info.TryGetProperty(PropertyName._mapLabel, out var value3))
		{
			_mapLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value4))
		{
			_statusLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._initialized, out var value5))
		{
			_initialized = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._committing, out var value6))
		{
			_committing = value6.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<LevelEditedEventHandler>(SignalName.LevelEdited, out var value7))
		{
			backing_LevelEdited = value7;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.LevelEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalLevelEdited()
	{
		EmitSignal(SignalName.LevelEdited, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.LevelEdited && args.Count == 0)
		{
			backing_LevelEdited?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.LevelEdited)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
