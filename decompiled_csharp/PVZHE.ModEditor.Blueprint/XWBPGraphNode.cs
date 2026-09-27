using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/GraphNode/XWBPGraphNode.cs")]
public class XWBPGraphNode : GraphNode
{
	[Signal]
	public delegate void NodeChangedEventHandler(XWBPGraphNode node);

	[Signal]
	public delegate void PortValueChangedEventHandler(XWBPGraphNode node, string portName, Variant value);

	public new class MethodName : GraphNode.MethodName
	{
		public static readonly StringName Create = "Create";

		public static readonly StringName Init = "Init";

		public static readonly StringName GetStyleKey = "GetStyleKey";

		public static readonly StringName InitStyle = "InitStyle";

		public static readonly StringName BuildPorts = "BuildPorts";

		public static readonly StringName BuildBasePorts = "BuildBasePorts";

		public static readonly StringName OnInputPortValueChanged = "OnInputPortValueChanged";

		public static readonly StringName OnPositionOffsetChanged = "OnPositionOffsetChanged";

		public static readonly StringName OnDragged = "OnDragged";

		public static readonly StringName OnResizeRequest = "OnResizeRequest";

		public static readonly StringName OnResizeEnd = "OnResizeEnd";

		public static readonly StringName RemoveSelf = "RemoveSelf";

		public static readonly StringName RenameSelf = "RenameSelf";

		public static readonly StringName PortChange = "PortChange";

		public static readonly StringName UpdateTooltip = "UpdateTooltip";

		public static readonly StringName GetNodeTypeName = "GetNodeTypeName";
	}

	public new class PropertyName : GraphNode.PropertyName
	{
		public static readonly StringName Editor = "Editor";

		public static readonly StringName NodeData = "NodeData";

		public static readonly StringName NodeType = "NodeType";

		public static readonly StringName _resizeStartSize = "_resizeStartSize";

		public static readonly StringName _resizeInProgress = "_resizeInProgress";
	}

	public new class SignalName : GraphNode.SignalName
	{
		public static readonly StringName NodeChanged = "NodeChanged";

		public static readonly StringName PortValueChanged = "PortValueChanged";
	}

	private const string ScenePath = "res://addons/ModEditor/Blueprint/GUI/GraphNode/XWBPGraphNode.tscn";

	private static PackedScene _scene;

	private static readonly Dictionary<string, StyleBoxFlat> _styleCache = new Dictionary<string, StyleBoxFlat>();

	private Vector2 _resizeStartSize;

	private bool _resizeInProgress;

	private NodeChangedEventHandler backing_NodeChanged;

	private PortValueChangedEventHandler backing_PortValueChanged;

	public XWBPEditor Editor { get; set; }

	public XWBPNodeData NodeData { get; private set; }

	public XWBPNodeType NodeType { get; private set; }

	public Dictionary<string, Control> PortWidgets { get; } = new Dictionary<string, Control>();

	public event NodeChangedEventHandler NodeChanged
	{
		add
		{
			backing_NodeChanged = (NodeChangedEventHandler)Delegate.Combine(backing_NodeChanged, value);
		}
		remove
		{
			backing_NodeChanged = (NodeChangedEventHandler)Delegate.Remove(backing_NodeChanged, value);
		}
	}

	public event PortValueChangedEventHandler PortValueChanged
	{
		add
		{
			backing_PortValueChanged = (PortValueChangedEventHandler)Delegate.Combine(backing_PortValueChanged, value);
		}
		remove
		{
			backing_PortValueChanged = (PortValueChangedEventHandler)Delegate.Remove(backing_PortValueChanged, value);
		}
	}

	public static XWBPGraphNode Create()
	{
		if (_scene == null)
		{
			_scene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Blueprint/GUI/GraphNode/XWBPGraphNode.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		return _scene.Instantiate<XWBPGraphNode>(PackedScene.GenEditState.Disabled);
	}

	public void Init(XWBPNodeData nodeData, XWBPNodeType nodeType)
	{
		NodeData = nodeData;
		NodeType = nodeType;
		nodeData.NodeType = nodeType;
		Title = nodeType.DisplayName;
		PositionOffset = nodeData.Position;
		if (nodeData.Size != Vector2.Zero)
		{
			Size = nodeData.Size;
		}
		PositionOffsetChanged += OnPositionOffsetChanged;
		Dragged += OnDragged;
		ResizeRequest += OnResizeRequest;
		ResizeEnd += OnResizeEnd;
		InitStyle(nodeType.Color);
		nodeType.GraphNodeInit(this);
		BuildPorts();
		UpdateTooltip();
	}

	private static string GetStyleKey(Color nodeColor, string styleName)
	{
		return $"{styleName}_{nodeColor.R:F3}_{nodeColor.G:F3}_{nodeColor.B:F3}";
	}

	private static StyleBoxFlat GetOrCreateStyle(string key, Func<StyleBoxFlat> createFunc)
	{
		if (_styleCache.TryGetValue(key, out var value))
		{
			return value;
		}
		value = createFunc();
		_styleCache[key] = value;
		return value;
	}

	private void InitStyle(Color nodeColor)
	{
		StyleBoxFlat orCreateStyle = GetOrCreateStyle(GetStyleKey(nodeColor, "titlebar"), () => new StyleBoxFlat
		{
			BgColor = nodeColor,
			BorderColor = new Color(nodeColor.R * 0.5f, nodeColor.G * 0.5f, nodeColor.B * 0.5f, 0.6f),
			BorderWidthBottom = 1,
			ExpandMarginLeft = 4f,
			ExpandMarginRight = 4f,
			ExpandMarginTop = 4f,
			CornerRadiusTopLeft = 4,
			CornerRadiusTopRight = 4,
			ContentMarginLeft = 8f,
			ContentMarginTop = 4f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 4f
		});
		AddThemeStyleboxOverride("titlebar", orCreateStyle);
		StyleBoxFlat orCreateStyle2 = GetOrCreateStyle(GetStyleKey(nodeColor, "panel"), () => new StyleBoxFlat
		{
			BgColor = new Color(0.06f, 0.06f, 0.068f, 0.98f),
			BorderColor = new Color(0.04f, 0.54f, 1f, 0.08f),
			BorderWidthBottom = 1,
			BorderWidthLeft = 1,
			BorderWidthRight = 1,
			BorderWidthTop = 1,
			ExpandMarginLeft = 4f,
			ExpandMarginRight = 4f,
			ExpandMarginBottom = 4f,
			CornerRadiusBottomLeft = 4,
			CornerRadiusBottomRight = 4,
			ContentMarginLeft = 6f,
			ContentMarginTop = 4f,
			ContentMarginRight = 6f,
			ContentMarginBottom = 6f
		});
		AddThemeStyleboxOverride("panel", orCreateStyle2);
		StyleBoxFlat orCreateStyle3 = GetOrCreateStyle(GetStyleKey(nodeColor, "panel_selected"), () => new StyleBoxFlat
		{
			BgColor = new Color(0.06f, 0.06f, 0.068f, 0.98f),
			BorderColor = new Color(0.04f, 0.54f, 1f, 0.9f),
			BorderWidthBottom = 2,
			BorderWidthLeft = 2,
			BorderWidthRight = 2,
			BorderWidthTop = 2,
			ExpandMarginLeft = 4f,
			ExpandMarginRight = 4f,
			ExpandMarginBottom = 4f,
			CornerRadiusBottomLeft = 4,
			CornerRadiusBottomRight = 4,
			ShadowColor = new Color(0.04f, 0.54f, 1f, 0.25f),
			ShadowSize = 8,
			ContentMarginLeft = 6f,
			ContentMarginTop = 4f,
			ContentMarginRight = 6f,
			ContentMarginBottom = 6f
		});
		AddThemeStyleboxOverride("panel_selected", orCreateStyle3);
		StyleBoxFlat orCreateStyle4 = GetOrCreateStyle(GetStyleKey(nodeColor, "titlebar_selected"), () => new StyleBoxFlat
		{
			BgColor = nodeColor,
			BorderColor = new Color(0.04f, 0.54f, 1f, 0.9f),
			BorderWidthBottom = 2,
			BorderWidthLeft = 2,
			BorderWidthRight = 2,
			BorderWidthTop = 2,
			ExpandMarginLeft = 4f,
			ExpandMarginRight = 4f,
			ExpandMarginBottom = 0f,
			ExpandMarginTop = 4f,
			CornerRadiusTopLeft = 4,
			CornerRadiusTopRight = 4,
			CornerRadiusBottomLeft = 0,
			CornerRadiusBottomRight = 0,
			ShadowColor = new Color(0.04f, 0.54f, 1f, 0.25f),
			ShadowSize = 8,
			ContentMarginLeft = 8f,
			ContentMarginTop = 4f,
			ContentMarginRight = 8f,
			ContentMarginBottom = 4f
		});
		AddThemeStyleboxOverride("titlebar_selected", orCreateStyle4);
	}

	public async void BuildPorts()
	{
		ClearAllSlots();
		foreach (Node child in GetChildren())
		{
			if (child != null)
			{
				Node node = child;
				node.QueueFree();
			}
		}
		PortWidgets.Clear();
		NodeType.GraphNodeBuildRefresh(this);
		BuildBasePorts();
		await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
		if (GodotObject.IsInstanceValid(this) && GodotObject.IsInstanceValid(NodeData) && NodeData.Size != Vector2.Zero)
		{
			Size = new Vector2(Size.X, NodeData.Size.Y);
		}
	}

	private int BuildBasePorts()
	{
		int count = NodeData.InputPorts.Count;
		int count2 = NodeData.OutputPorts.Count;
		int num = Math.Max(count, count2);
		for (int i = 0; i < num; i++)
		{
			XWBPGraphNodePortContainer xWBPGraphNodePortContainer = XWBPGraphNodePortContainer.Create();
			AddChild(xWBPGraphNodePortContainer, forceReadableName: false, InternalMode.Disabled);
			bool flag = i < count;
			bool flag2 = i < count2;
			XWBPNodePortData xWBPNodePortData = (flag ? NodeData.InputPorts[i] : null);
			XWBPNodePortData xWBPNodePortData2 = (flag2 ? NodeData.OutputPorts[i] : null);
			if (flag)
			{
				xWBPGraphNodePortContainer.InputSet(xWBPNodePortData);
				if (xWBPGraphNodePortContainer.InputEditor != null)
				{
					PortWidgets[xWBPNodePortData.Name] = xWBPGraphNodePortContainer.InputEditor;
				}
				xWBPGraphNodePortContainer.InputValueChange += OnInputPortValueChanged;
			}
			if (flag2)
			{
				xWBPGraphNodePortContainer.OutputSet(xWBPNodePortData2);
			}
			bool flag3 = flag;
			int typeLeft = (int)(flag ? xWBPNodePortData.PortTypeValue : XWBPNodePortData.PortType.Unknown);
			Color colorLeft = (flag ? XWBPNodePortData.GetTypeColor(xWBPNodePortData.PortTypeValue, xWBPNodePortData.ClassName) : Colors.White);
			bool flag4 = flag2;
			int typeRight = (int)(flag2 ? xWBPNodePortData2.PortTypeValue : XWBPNodePortData.PortType.Unknown);
			Color colorRight = (flag2 ? XWBPNodePortData.GetTypeColor(xWBPNodePortData2.PortTypeValue, xWBPNodePortData2.ClassName) : Colors.White);
			SetSlot(i, flag3, typeLeft, colorLeft, flag4, typeRight, colorRight);
			if (flag3 && xWBPNodePortData.PortTypeValue == XWBPNodePortData.PortType.Flow)
			{
				SetSlotCustomIconLeft(i, XWBPNodePortData.GetTypeIcon(XWBPNodePortData.PortType.Flow));
			}
			if (flag4 && xWBPNodePortData2.PortTypeValue == XWBPNodePortData.PortType.Flow)
			{
				SetSlotCustomIconRight(i, XWBPNodePortData.GetTypeIcon(XWBPNodePortData.PortType.Flow));
			}
		}
		return num;
	}

	private void OnInputPortValueChanged(XWBPNodePortData portData, Variant oldValue, Variant newValue)
	{
		if (GodotObject.IsInstanceValid(portData))
		{
			Editor?.CommitGraphPortValue(portData, oldValue, newValue);
			EmitSignal(SignalName.PortValueChanged, this, portData.Name, newValue);
		}
	}

	private void OnPositionOffsetChanged()
	{
		if (GodotObject.IsInstanceValid(NodeData))
		{
			NodeData.Position = PositionOffset;
		}
		EmitSignal(SignalName.NodeChanged, this);
	}

	private void OnDragged(Vector2 from, Vector2 to)
	{
		if (GodotObject.IsInstanceValid(NodeData) && !from.IsEqualApprox(to))
		{
			Editor?.CommitGraphNodePosition(NodeData, from, to);
		}
	}

	private void OnResizeRequest(Vector2 newSize)
	{
		if (!_resizeInProgress)
		{
			_resizeStartSize = ((GodotObject.IsInstanceValid(NodeData) && NodeData.Size != Vector2.Zero) ? NodeData.Size : Size);
			_resizeInProgress = true;
		}
		Size = newSize;
	}

	private void OnResizeEnd(Vector2 newSize)
	{
		Vector2 oldSize = (_resizeInProgress ? _resizeStartSize : (NodeData?.Size ?? Size));
		_resizeInProgress = false;
		if (GodotObject.IsInstanceValid(NodeData) && !oldSize.IsEqualApprox(newSize))
		{
			Editor?.CommitGraphNodeSize(NodeData, oldSize, newSize);
		}
	}

	public void RemoveSelf()
	{
		if (GetParent() is XWBPGraphEdit xWBPGraphEdit)
		{
			xWBPGraphEdit.RemoveGraphNodeWithName(Name);
		}
	}

	public void RenameSelf(string newName)
	{
		if (NodeType.NodeTypeEnumValue == XWBPNodeType.NodeTypeEnum.Function)
		{
			Title = "调用 " + newName;
		}
		else if (NodeType.NodeTypeEnumValue == XWBPNodeType.NodeTypeEnum.FunctionEntry)
		{
			Title = newName;
		}
	}

	public void PortChange()
	{
		BuildPorts();
	}

	public void UpdateTooltip()
	{
		string text = "";
		text += $"节点 ID: {NodeData.Id}\n";
		text += $"类型 ID: {NodeType.TypeId}\n";
		text = text + "显示名称: " + NodeType.DisplayName + "\n";
		if (!string.IsNullOrEmpty(NodeType.Category))
		{
			text = text + "分类: " + NodeType.Category + "\n";
		}
		text = text + "节点类型: " + GetNodeTypeName(NodeType.NodeTypeEnumValue) + "\n";
		if (!string.IsNullOrEmpty(NodeType.Description))
		{
			text = text + "\n描述:\n" + NodeType.Description;
		}
		TooltipText = text;
	}

	private static string GetNodeTypeName(XWBPNodeType.NodeTypeEnum type)
	{
		return type switch
		{
			XWBPNodeType.NodeTypeEnum.General => "通用", 
			XWBPNodeType.NodeTypeEnum.Entry => "入口", 
			XWBPNodeType.NodeTypeEnum.Function => "函数", 
			XWBPNodeType.NodeTypeEnum.FunctionEntry => "函数入口", 
			XWBPNodeType.NodeTypeEnum.PropertyGet => "属性获取", 
			XWBPNodeType.NodeTypeEnum.PropertySet => "属性设置", 
			_ => "未知", 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "nodeType", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetStyleKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "nodeColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "styleName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitStyle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "nodeColor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPorts, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildBasePorts, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInputPortValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "portData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "oldValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPositionOffsetChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDragged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnResizeRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "newSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnResizeEnd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "newSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PortChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateTooltip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNodeTypeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(Create());
			return true;
		}
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<XWBPNodeType>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetStyleKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetStyleKey(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.InitStyle && args.Count == 1)
		{
			InitStyle(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPorts && args.Count == 0)
		{
			BuildPorts();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildBasePorts && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BuildBasePorts());
			return true;
		}
		if (method == MethodName.OnInputPortValueChanged && args.Count == 3)
		{
			OnInputPortValueChanged(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPositionOffsetChanged && args.Count == 0)
		{
			OnPositionOffsetChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDragged && args.Count == 2)
		{
			OnDragged(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResizeRequest && args.Count == 1)
		{
			OnResizeRequest(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResizeEnd && args.Count == 1)
		{
			OnResizeEnd(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelf && args.Count == 0)
		{
			RemoveSelf();
			ret = default;
			return true;
		}
		if (method == MethodName.RenameSelf && args.Count == 1)
		{
			RenameSelf(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PortChange && args.Count == 0)
		{
			PortChange();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateTooltip && args.Count == 0)
		{
			UpdateTooltip();
			ret = default;
			return true;
		}
		if (method == MethodName.GetNodeTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetNodeTypeName(VariantUtils.ConvertTo<XWBPNodeType.NodeTypeEnum>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(Create());
			return true;
		}
		if (method == MethodName.GetStyleKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetStyleKey(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetNodeTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetNodeTypeName(VariantUtils.ConvertTo<XWBPNodeType.NodeTypeEnum>(in args[0])));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.GetStyleKey)
		{
			return true;
		}
		if (method == MethodName.InitStyle)
		{
			return true;
		}
		if (method == MethodName.BuildPorts)
		{
			return true;
		}
		if (method == MethodName.BuildBasePorts)
		{
			return true;
		}
		if (method == MethodName.OnInputPortValueChanged)
		{
			return true;
		}
		if (method == MethodName.OnPositionOffsetChanged)
		{
			return true;
		}
		if (method == MethodName.OnDragged)
		{
			return true;
		}
		if (method == MethodName.OnResizeRequest)
		{
			return true;
		}
		if (method == MethodName.OnResizeEnd)
		{
			return true;
		}
		if (method == MethodName.RemoveSelf)
		{
			return true;
		}
		if (method == MethodName.RenameSelf)
		{
			return true;
		}
		if (method == MethodName.PortChange)
		{
			return true;
		}
		if (method == MethodName.UpdateTooltip)
		{
			return true;
		}
		if (method == MethodName.GetNodeTypeName)
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
		if (name == PropertyName.NodeData)
		{
			NodeData = VariantUtils.ConvertTo<XWBPNodeData>(in value);
			return true;
		}
		if (name == PropertyName.NodeType)
		{
			NodeType = VariantUtils.ConvertTo<XWBPNodeType>(in value);
			return true;
		}
		if (name == PropertyName._resizeStartSize)
		{
			_resizeStartSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._resizeInProgress)
		{
			_resizeInProgress = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.NodeData)
		{
			value = VariantUtils.CreateFrom<XWBPNodeData>(NodeData);
			return true;
		}
		if (name == PropertyName.NodeType)
		{
			value = VariantUtils.CreateFrom<XWBPNodeType>(NodeType);
			return true;
		}
		if (name == PropertyName._resizeStartSize)
		{
			value = VariantUtils.CreateFrom(in _resizeStartSize);
			return true;
		}
		if (name == PropertyName._resizeInProgress)
		{
			value = VariantUtils.CreateFrom(in _resizeInProgress);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.NodeData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.NodeType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._resizeStartSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resizeInProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Editor, Variant.From<XWBPEditor>(Editor));
		info.AddProperty(PropertyName.NodeData, Variant.From<XWBPNodeData>(NodeData));
		info.AddProperty(PropertyName.NodeType, Variant.From<XWBPNodeType>(NodeType));
		info.AddProperty(PropertyName._resizeStartSize, Variant.From(in _resizeStartSize));
		info.AddProperty(PropertyName._resizeInProgress, Variant.From(in _resizeInProgress));
		info.AddSignalEventDelegate(SignalName.NodeChanged, backing_NodeChanged);
		info.AddSignalEventDelegate(SignalName.PortValueChanged, backing_PortValueChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Editor, out var value))
		{
			Editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName.NodeData, out var value2))
		{
			NodeData = value2.As<XWBPNodeData>();
		}
		if (info.TryGetProperty(PropertyName.NodeType, out var value3))
		{
			NodeType = value3.As<XWBPNodeType>();
		}
		if (info.TryGetProperty(PropertyName._resizeStartSize, out var value4))
		{
			_resizeStartSize = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._resizeInProgress, out var value5))
		{
			_resizeInProgress = value5.As<bool>();
		}
		if (info.TryGetSignalEventDelegate<NodeChangedEventHandler>(SignalName.NodeChanged, out var value6))
		{
			backing_NodeChanged = value6;
		}
		if (info.TryGetSignalEventDelegate<PortValueChangedEventHandler>(SignalName.PortValueChanged, out var value7))
		{
			backing_PortValueChanged = value7;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(SignalName.NodeChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false)
			}, null),
			new MethodInfo(SignalName.PortValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false),
				new PropertyInfo(Variant.Type.String, "portName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null)
		};
	}

	protected void EmitSignalNodeChanged(XWBPGraphNode node)
	{
		EmitSignal(SignalName.NodeChanged, new ReadOnlySpan<Variant>((Variant)node));
	}

	protected void EmitSignalPortValueChanged(XWBPGraphNode node, string portName, Variant value)
	{
		StringName portValueChanged = SignalName.PortValueChanged;
		_003C_003Ey__InlineArray3<Variant> buffer = default;
		buffer[0] = node;
		buffer[1] = portName;
		buffer[2] = value;
		EmitSignal(portValueChanged, buffer);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.NodeChanged && args.Count == 1)
		{
			backing_NodeChanged?.Invoke(VariantUtils.ConvertTo<XWBPGraphNode>(in args[0]));
		}
		else if (signal == SignalName.PortValueChanged && args.Count == 3)
		{
			backing_PortValueChanged?.Invoke(VariantUtils.ConvertTo<XWBPGraphNode>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.NodeChanged)
		{
			return true;
		}
		if (signal == SignalName.PortValueChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
