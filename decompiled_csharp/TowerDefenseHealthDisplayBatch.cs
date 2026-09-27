using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Script/Component/TowerDefense/Character/ShowHealthComponent/TowerDefenseHealthDisplayBatch.cs")]
public sealed class TowerDefenseHealthDisplayBatch : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName EnsureInstance = "EnsureInstance";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GetOrCreateLayer = "GetOrCreateLayer";

		public static readonly StringName RemoveLayerIfEmpty = "RemoveLayerIfEmpty";

		public static readonly StringName ReportDrawRebuild = "ReportDrawRebuild";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName _drawRebuildCount = "_drawRebuildCount";

		public static readonly StringName _font = "_font";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string DefaultFontPath = "uid://coqskwlqtnypf";

	private static Font _defaultFont;

	private readonly Dictionary<int, TowerDefenseHealthDisplayBatchLayer> _layers = new Dictionary<int, TowerDefenseHealthDisplayBatchLayer>();

	private readonly Dictionary<ShowHealthComponent, TowerDefenseHealthDisplayBatchLayer> _displayLayers = new Dictionary<ShowHealthComponent, TowerDefenseHealthDisplayBatchLayer>(ReferenceEqualityComparer.Instance);

	private HealthTextLayoutCache _textLayoutCache;

	private long _drawRebuildCount;

	private Font _font;

	public static TowerDefenseHealthDisplayBatch Instance { get; private set; }

	public static int ActiveDisplayCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._displayLayers.Count;
		}
	}

	public static int ActiveLayerCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._layers.Count;
		}
	}

	public static int CachedTextLineCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0;
			}
			return Instance._textLayoutCache?.Count ?? 0;
		}
	}

	public static long TextLayoutBuildCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0L;
			}
			return Instance._textLayoutCache?.BuildCount ?? 0;
		}
	}

	public static long DrawRebuildCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0L;
			}
			return Instance._drawRebuildCount;
		}
	}

	public static bool Attach(ShowHealthComponent display)
	{
		if (display == null || !display.CanUseSharedDrawBatch())
		{
			return false;
		}
		if (!EnsureInstance(display.BatchOwner))
		{
			return false;
		}
		return Instance.AttachCore(display);
	}

	public static void Detach(ShowHealthComponent display)
	{
		if (display != null && GodotObject.IsInstanceValid(Instance))
		{
			Instance.DetachCore(display);
		}
	}

	public static void Update(ShowHealthComponent display)
	{
		if (display != null && GodotObject.IsInstanceValid(Instance) && Instance._displayLayers.TryGetValue(display, out var value))
		{
			value.RequestRedraw();
		}
	}

	public static void NotifyZIndexChanged(ShowHealthComponent display)
	{
		if (display != null && GodotObject.IsInstanceValid(Instance))
		{
			Instance.MoveToCurrentLayer(display);
		}
	}

	public static void NotifyPositionChanged(ShowHealthComponent display)
	{
		if (display != null && GodotObject.IsInstanceValid(Instance) && Instance._displayLayers.TryGetValue(display, out var value))
		{
			value.RequestRedraw();
		}
	}

	internal static bool TryGetDisplayZIndex(ShowHealthComponent display, out int zIndex)
	{
		zIndex = 0;
		if (display == null || !GodotObject.IsInstanceValid(Instance))
		{
			return false;
		}
		if (!Instance._displayLayers.TryGetValue(display, out var value))
		{
			return false;
		}
		zIndex = value.ZIndex;
		return true;
	}

	private static bool EnsureInstance(TowerDefenseCharacter owner)
	{
		if (GodotObject.IsInstanceValid(Instance))
		{
			return GodotObject.IsInstanceValid(Instance._font);
		}
		if (!GodotObject.IsInstanceValid(owner) || !owner.IsInsideTree())
		{
			return false;
		}
		if (_defaultFont == null)
		{
			_defaultFont = GD.Load<Font>("uid://coqskwlqtnypf");
		}
		if (!GodotObject.IsInstanceValid(_defaultFont))
		{
			return false;
		}
		Node node = TowerDefenseProcessModeDispatch.ResolveBatchParent(owner);
		if (!GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		TowerDefenseHealthDisplayBatch towerDefenseHealthDisplayBatch = (Instance = new TowerDefenseHealthDisplayBatch
		{
			Name = "TowerDefenseHealthDisplayBatch",
			_font = _defaultFont,
			_textLayoutCache = new HealthTextLayoutCache(_defaultFont),
			ProcessMode = ProcessModeEnum.Inherit,
			LightMask = 0,
			TopLevel = true,
			ZAsRelative = false
		});
		node.CallDeferred(Node.MethodName.AddChild, towerDefenseHealthDisplayBatch);
		return true;
	}

	public override void _Ready()
	{
		Instance = this;
		TopLevel = true;
		GlobalTransform = Transform2D.Identity;
		SetProcess(enable: false);
	}

	public override void _ExitTree()
	{
		_displayLayers.Clear();
		_layers.Clear();
		_textLayoutCache?.Dispose();
		_textLayoutCache = null;
		if (Instance == this)
		{
			Instance = null;
		}
	}

	private bool AttachCore(ShowHealthComponent display)
	{
		if (_displayLayers.ContainsKey(display))
		{
			MoveToCurrentLayer(display);
			return true;
		}
		int zIndex = display.ResolveSharedDrawZIndex();
		TowerDefenseHealthDisplayBatchLayer orCreateLayer = GetOrCreateLayer(zIndex);
		orCreateLayer.Add(display);
		_displayLayers.Add(display, orCreateLayer);
		orCreateLayer.RequestRedraw();
		return true;
	}

	private void DetachCore(ShowHealthComponent display)
	{
		if (_displayLayers.Remove(display, out var value))
		{
			value.Remove(display);
			RemoveLayerIfEmpty(value);
		}
	}

	private void MoveToCurrentLayer(ShowHealthComponent display)
	{
		if (_displayLayers.TryGetValue(display, out var value))
		{
			int num = display.ResolveSharedDrawZIndex();
			if (value.ZIndex == num)
			{
				value.RequestRedraw();
				return;
			}
			value.Remove(display);
			RemoveLayerIfEmpty(value);
			TowerDefenseHealthDisplayBatchLayer orCreateLayer = GetOrCreateLayer(num);
			orCreateLayer.Add(display);
			_displayLayers[display] = orCreateLayer;
			orCreateLayer.RequestRedraw();
		}
	}

	private TowerDefenseHealthDisplayBatchLayer GetOrCreateLayer(int zIndex)
	{
		if (_layers.TryGetValue(zIndex, out var value))
		{
			return value;
		}
		value = new TowerDefenseHealthDisplayBatchLayer
		{
			Name = $"HealthZ{zIndex}",
			ZIndex = zIndex,
			ZAsRelative = false,
			TopLevel = true,
			LightMask = 0,
			Font = _font,
			TextLayoutCache = _textLayoutCache,
			OwnerBatch = this
		};
		_layers.Add(zIndex, value);
		AddChild(value, forceReadableName: false, InternalMode.Disabled);
		return value;
	}

	private void RemoveLayerIfEmpty(TowerDefenseHealthDisplayBatchLayer layer)
	{
		if (layer != null && layer.DisplayCount == 0)
		{
			_layers.Remove(layer.ZIndex);
			if (layer.IsInsideTree())
			{
				layer.QueueFree();
			}
			else
			{
				layer.Free();
			}
		}
	}

	internal void ReportDrawRebuild()
	{
		_drawRebuildCount++;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.EnsureInstance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetOrCreateLayer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "zIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveLayerIfEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "layer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReportDrawRebuild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EnsureInstance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureInstance(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.GetOrCreateLayer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseHealthDisplayBatchLayer>(GetOrCreateLayer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveLayerIfEmpty && args.Count == 1)
		{
			RemoveLayerIfEmpty(VariantUtils.ConvertTo<TowerDefenseHealthDisplayBatchLayer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReportDrawRebuild && args.Count == 0)
		{
			ReportDrawRebuild();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EnsureInstance && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureInstance(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EnsureInstance)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.GetOrCreateLayer)
		{
			return true;
		}
		if (method == MethodName.RemoveLayerIfEmpty)
		{
			return true;
		}
		if (method == MethodName.ReportDrawRebuild)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._drawRebuildCount)
		{
			_drawRebuildCount = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._font)
		{
			_font = VariantUtils.ConvertTo<Font>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._drawRebuildCount)
		{
			value = VariantUtils.CreateFrom(in _drawRebuildCount);
			return true;
		}
		if (name == PropertyName._font)
		{
			value = VariantUtils.CreateFrom(in _font);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._drawRebuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._font, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._drawRebuildCount, Variant.From(in _drawRebuildCount));
		info.AddProperty(PropertyName._font, Variant.From(in _font));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._drawRebuildCount, out var value))
		{
			_drawRebuildCount = value.As<long>();
		}
		if (info.TryGetProperty(PropertyName._font, out var value2))
		{
			_font = value2.As<Font>();
		}
	}
}
