using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Base/TowerDefenseGroundItemBase.cs")]
public class TowerDefenseGroundItemBase : Node2D
{
	public delegate void LandEventHandler();

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName OnGroundHeightChanged = "OnGroundHeightChanged";

		public static readonly StringName OnGridPositionChanged = "OnGridPositionChanged";

		public static readonly StringName ClearStaticBattleReferences = "ClearStaticBattleReferences";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName PhysiceUpdate = "PhysiceUpdate";

		public static readonly StringName CompleteLanding = "CompleteLanding";

		public static readonly StringName SetZ = "SetZ";

		public static readonly StringName FreshZIndex = "FreshZIndex";

		public static readonly StringName FreshZIndexForGlobalPosition = "FreshZIndexForGlobalPosition";

		public static readonly StringName SetTransientRenderZIndex = "SetTransientRenderZIndex";

		public static readonly StringName ApplyRenderZIndex = "ApplyRenderZIndex";

		public static readonly StringName ResolveRenderGridY = "ResolveRenderGridY";

		public static readonly StringName InvalidateDescendantAdobeAnimateRenderOrder = "InvalidateDescendantAdobeAnimateRenderOrder";

		public static readonly StringName MarkDescendantAdobeAnimateRenderOrderDebug = "MarkDescendantAdobeAnimateRenderOrderDebug";

		public static readonly StringName BuildGridRenderOrderReason = "BuildGridRenderOrderReason";

		public static readonly StringName DebugPrintGridRenderOrderChange = "DebugPrintGridRenderOrderChange";

		public static readonly StringName CountDescendantAdobeAnimateSprites = "CountDescendantAdobeAnimateSprites";

		public static readonly StringName GetPathOrName = "GetPathOrName";

		public static readonly StringName GetFallTime = "GetFallTime";

		public static readonly StringName Collection = "Collection";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName groundHeight = "groundHeight";

		public static readonly StringName z = "z";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName itemLayer = "itemLayer";

		public static readonly StringName topLayer = "topLayer";

		public static readonly StringName ZChangedInsidePhysicsUpdate = "ZChangedInsidePhysicsUpdate";

		public static readonly StringName _groundHeight = "_groundHeight";

		public static readonly StringName _z = "_z";

		public static readonly StringName _gridPos = "_gridPos";

		public static readonly StringName _itemLayer = "_itemLayer";

		public static readonly StringName cell = "cell";

		public static readonly StringName cellPercentage = "cellPercentage";

		public static readonly StringName gravityUse = "gravityUse";

		public static readonly StringName gravity = "gravity";

		public static readonly StringName gravityScale = "gravityScale";

		public static readonly StringName ySpeed = "ySpeed";

		public static readonly StringName _topLayer = "_topLayer";

		public static readonly StringName isGround = "isGround";

		public static readonly StringName _insidePhysicsUpdate = "_insidePhysicsUpdate";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private static readonly bool DebugGridRenderOrderTrace;

	private const int DebugGridRenderOrderTraceLimit = 800;

	private static int _debugGridRenderOrderTraceCount;

	private double _groundHeight;

	private double _z;

	private Vector2I _gridPos = new Vector2I(-1, -1);

	private TowerDefenseEnum.LAYER_GROUNDITEM _itemLayer;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCellInstance cell;

	[Export(PropertyHint.None, "")]
	public double cellPercentage = 0.5;

	[Export(PropertyHint.None, "")]
	public bool gravityUse = true;

	[Export(PropertyHint.None, "")]
	public double gravity = 245.0;

	[Export(PropertyHint.None, "")]
	public double gravityScale = 1.5;

	[Export(PropertyHint.None, "")]
	public double ySpeed;

	private bool _topLayer;

	public static Node2D characterNode;

	public bool isGround = true;

	private bool _insidePhysicsUpdate;

	[Export(PropertyHint.None, "")]
	public double groundHeight
	{
		get
		{
			return _groundHeight;
		}
		set
		{
			if (_groundHeight != value)
			{
				double oldGroundHeight = _groundHeight;
				_groundHeight = value;
				if (isGround && z != groundHeight)
				{
					z = groundHeight;
				}
				OnGroundHeightChanged(oldGroundHeight, value);
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double z
	{
		get
		{
			return _z;
		}
		set
		{
			if (_z != value)
			{
				_z = value;
				SetZ();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Vector2I gridPos
	{
		get
		{
			return _gridPos;
		}
		set
		{
			if (_gridPos != value)
			{
				Vector2I oldGridPos = _gridPos;
				int zIndex = ZIndex;
				_gridPos = value;
				cell = TowerDefenseManager.GetMapCell(gridPos);
				FreshZIndex("gridPos", oldGridPos, zIndex);
				OnGridPositionChanged(oldGridPos, value);
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.LAYER_GROUNDITEM itemLayer
	{
		get
		{
			return _itemLayer;
		}
		set
		{
			TowerDefenseEnum.LAYER_GROUNDITEM value2 = _itemLayer;
			int zIndex = ZIndex;
			_itemLayer = value;
			FreshZIndex($"itemLayer:{value2}->{value}", gridPos, zIndex);
		}
	}

	[Export(PropertyHint.None, "")]
	public bool topLayer
	{
		get
		{
			return _topLayer;
		}
		set
		{
			bool value2 = _topLayer;
			int zIndex = ZIndex;
			_topLayer = value;
			FreshZIndex($"topLayer:{value2}->{value}", gridPos, zIndex);
		}
	}

	protected bool ZChangedInsidePhysicsUpdate => _insidePhysicsUpdate;

	public event LandEventHandler OnLand;

	internal event Action<int> RenderZIndexChanged;

	protected virtual void OnGroundHeightChanged(double oldGroundHeight, double newGroundHeight)
	{
	}

	protected virtual void OnGridPositionChanged(Vector2I oldGridPos, Vector2I newGridPos)
	{
	}

	internal static void ClearStaticBattleReferences()
	{
		characterNode = null;
	}

	public override void _EnterTree()
	{
		if (!Engine.IsEditorHint())
		{
			characterNode = TowerDefenseManager.GetCharacterNode();
		}
	}

	public override void _Ready()
	{
		FreshZIndex();
	}

	public override void _PhysicsProcess(double delta)
	{
		PhysiceUpdate((float)delta);
	}

	public void PhysiceUpdate(float delta)
	{
		if (isGround && ySpeed >= 0.0 && z == groundHeight)
		{
			return;
		}
		_insidePhysicsUpdate = true;
		try
		{
			if (isGround && ySpeed >= 0.0)
			{
				if (z != groundHeight)
				{
					z = groundHeight;
				}
			}
			else if (z > groundHeight || ySpeed < 0.0)
			{
				isGround = false;
				if (gravityUse)
				{
					ySpeed += gravity * gravityScale * (double)delta;
				}
				z -= ySpeed * (double)delta;
				if (z <= groundHeight && ySpeed >= 0.0)
				{
					CompleteLanding();
				}
			}
			else
			{
				CompleteLanding();
			}
		}
		finally
		{
			_insidePhysicsUpdate = false;
		}
	}

	private void CompleteLanding()
	{
		OnLand?.Invoke();
		z = groundHeight;
		if (ySpeed >= 0.0)
		{
			ySpeed = 0.0;
			isGround = true;
		}
	}

	public virtual void SetZ()
	{
	}

	public void FreshZIndex()
	{
		FreshZIndex("", gridPos, ZIndex);
	}

	protected void FreshZIndexForGlobalPosition(Vector2 globalPosition, string reason)
	{
		int zIndex = ZIndex;
		int renderGridY = ResolveRenderGridY(globalPosition);
		ApplyRenderZIndex(renderGridY, reason, gridPos, zIndex);
	}

	protected void SetTransientRenderZIndex(int requestedZIndex, string reason)
	{
		int num = Math.Clamp(requestedZIndex, -4096, 4096);
		if (ZIndex != num)
		{
			ZIndex = num;
			RenderZIndexChanged?.Invoke(num);
			InvalidateDescendantAdobeAnimateRenderOrder(string.IsNullOrWhiteSpace(reason) ? "transient-z-index" : reason);
		}
	}

	private void FreshZIndex(string debugReason, Vector2I oldGridPos, int oldZIndex)
	{
		int renderGridY = ResolveRenderGridY();
		ApplyRenderZIndex(renderGridY, debugReason, oldGridPos, oldZIndex);
	}

	private void ApplyRenderZIndex(int renderGridY, string debugReason, Vector2I oldGridPos, int oldZIndex)
	{
		long value = (topLayer ? (1500L + (long)itemLayer) : ((long)renderGridY * 15L + (long)itemLayer));
		int num = (int)Math.Clamp(value, -4096L, 4096L);
		if (ZIndex != num)
		{
			ZIndex = num;
			RenderZIndexChanged?.Invoke(num);
			if (!string.IsNullOrEmpty(debugReason))
			{
				DebugPrintGridRenderOrderChange(debugReason, oldGridPos, oldZIndex, num, renderGridY, zChanged: true);
			}
			InvalidateDescendantAdobeAnimateRenderOrder(BuildGridRenderOrderReason(debugReason, oldGridPos, oldZIndex, num, renderGridY, zChanged: true));
		}
	}

	private int ResolveRenderGridY()
	{
		if (gridPos.Y >= 0)
		{
			return gridPos.Y;
		}
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			Vector2I mapGridPos = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition);
			if (mapGridPos.X >= 0 && mapGridPos.Y >= 0)
			{
				return mapGridPos.Y;
			}
		}
		return gridPos.Y;
	}

	private int ResolveRenderGridY(Vector2 globalPosition)
	{
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			Vector2I mapGridPos = TowerDefenseManager.Instance.GetMapGridPos(globalPosition);
			if (TowerDefenseManager.Instance.CheckMapGridPosIn(mapGridPos))
			{
				return mapGridPos.Y;
			}
		}
		return ResolveRenderGridY();
	}

	private void InvalidateDescendantAdobeAnimateRenderOrder(string reason)
	{
		foreach (Node child in GetChildren())
		{
			InvalidateDescendantAdobeAnimateRenderOrder(child, reason);
		}
	}

	private static void InvalidateDescendantAdobeAnimateRenderOrder(Node node, string reason)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.InvalidateInheritedRenderOrderForRender(reason);
		}
		foreach (Node child in node.GetChildren())
		{
			InvalidateDescendantAdobeAnimateRenderOrder(child, reason);
		}
	}

	private void MarkDescendantAdobeAnimateRenderOrderDebug(string reason)
	{
		foreach (Node child in GetChildren())
		{
			MarkDescendantAdobeAnimateRenderOrderDebug(child, reason);
		}
	}

	private static void MarkDescendantAdobeAnimateRenderOrderDebug(Node node, string reason)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.MarkRenderOrderDebugForRender(reason);
		}
		foreach (Node child in node.GetChildren())
		{
			MarkDescendantAdobeAnimateRenderOrderDebug(child, reason);
		}
	}

	private string BuildGridRenderOrderReason(string debugReason, Vector2I oldGridPos, int oldZIndex, int nextZIndex, int renderGridY, bool zChanged)
	{
		return $"{debugReason} ground={GetPathOrName(this)} grid={oldGridPos}->{gridPos} renderGridY={renderGridY} z={oldZIndex}->{nextZIndex} zChanged={zChanged}";
	}

	private void DebugPrintGridRenderOrderChange(string debugReason, Vector2I oldGridPos, int oldZIndex, int nextZIndex, int renderGridY, bool zChanged)
	{
		if (!DebugGridRenderOrderTrace || _debugGridRenderOrderTraceCount >= 800)
		{
			return;
		}
		int num = CountDescendantAdobeAnimateSprites(this);
		if (num > 0 && zChanged)
		{
			_debugGridRenderOrderTraceCount++;
			string value = "<no-manager>";
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				value = TowerDefenseManager.Instance.GetMapGridPos(GlobalPosition).ToString();
			}
			GD.Print($"[GridRenderOrderDebug] #{_debugGridRenderOrderTraceCount} reason={debugReason} path={GetPathOrName(this)} type={GetType().Name} grid={oldGridPos}->{gridPos} mapGridNow={value} renderGridY={renderGridY} itemLayer={itemLayer} topLayer={topLayer} z={oldZIndex}->{nextZIndex} currentZ={ZIndex} zChanged={zChanged} zRel={ZAsRelative} behind={ShowBehindParent} global=({GlobalPosition.X:F1},{GlobalPosition.Y:F1}) children={GetChildCount()} adobeChildren={num}");
		}
	}

	private static int CountDescendantAdobeAnimateSprites(Node node)
	{
		int num = 0;
		foreach (Node child in node.GetChildren())
		{
			if (child is AdobeAnimateSprite)
			{
				num++;
			}
			num += CountDescendantAdobeAnimateSprites(child);
		}
		return num;
	}

	private static string GetPathOrName(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return "<invalid>";
		}
		if (!node.IsInsideTree())
		{
			return node.Name.ToString();
		}
		return node.GetPath().ToString();
	}

	public double GetFallTime()
	{
		double num = Math.Abs(ySpeed) / (gravity * gravityScale);
		double num2 = gravity * gravityScale * num * num / 2.0 + (z - groundHeight);
		double num3 = Math.Sqrt(2.0 * num2 / (gravity * gravityScale));
		return num + num3;
	}

	public virtual void Collection()
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName.OnGroundHeightChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "oldGroundHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "newGroundHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGridPositionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "oldGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "newGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearStaticBattleReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PhysiceUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompleteLanding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetZ, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreshZIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FreshZIndexForGlobalPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetTransientRenderZIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requestedZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreshZIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "debugReason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "oldGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "oldZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRenderZIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "renderGridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "debugReason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "oldGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "oldZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveRenderGridY, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveRenderGridY, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateDescendantAdobeAnimateRenderOrder, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateDescendantAdobeAnimateRenderOrder, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkDescendantAdobeAnimateRenderOrderDebug, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkDescendantAdobeAnimateRenderOrderDebug, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildGridRenderOrderReason, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "debugReason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "oldGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "oldZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "nextZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "renderGridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "zChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DebugPrintGridRenderOrderChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "debugReason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "oldGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "oldZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "nextZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "renderGridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "zChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountDescendantAdobeAnimateSprites, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetPathOrName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetFallTime, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Collection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.OnGroundHeightChanged && args.Count == 2)
		{
			OnGroundHeightChanged(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGridPositionChanged && args.Count == 2)
		{
			OnGridPositionChanged(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearStaticBattleReferences && args.Count == 0)
		{
			ClearStaticBattleReferences();
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PhysiceUpdate && args.Count == 1)
		{
			PhysiceUpdate(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteLanding && args.Count == 0)
		{
			CompleteLanding();
			ret = default;
			return true;
		}
		if (method == MethodName.SetZ && args.Count == 0)
		{
			SetZ();
			ret = default;
			return true;
		}
		if (method == MethodName.FreshZIndex && args.Count == 0)
		{
			FreshZIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.FreshZIndexForGlobalPosition && args.Count == 2)
		{
			FreshZIndexForGlobalPosition(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetTransientRenderZIndex && args.Count == 2)
		{
			SetTransientRenderZIndex(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreshZIndex && args.Count == 3)
		{
			FreshZIndex(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRenderZIndex && args.Count == 4)
		{
			ApplyRenderZIndex(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveRenderGridY && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveRenderGridY());
			return true;
		}
		if (method == MethodName.ResolveRenderGridY && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveRenderGridY(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.InvalidateDescendantAdobeAnimateRenderOrder && args.Count == 1)
		{
			InvalidateDescendantAdobeAnimateRenderOrder(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateDescendantAdobeAnimateRenderOrder && args.Count == 2)
		{
			InvalidateDescendantAdobeAnimateRenderOrder(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkDescendantAdobeAnimateRenderOrderDebug && args.Count == 1)
		{
			MarkDescendantAdobeAnimateRenderOrderDebug(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkDescendantAdobeAnimateRenderOrderDebug && args.Count == 2)
		{
			MarkDescendantAdobeAnimateRenderOrderDebug(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGridRenderOrderReason && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<string>(BuildGridRenderOrderReason(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.DebugPrintGridRenderOrderChange && args.Count == 6)
		{
			DebugPrintGridRenderOrderChange(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountDescendantAdobeAnimateSprites && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountDescendantAdobeAnimateSprites(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPathOrName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPathOrName(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFallTime && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetFallTime());
			return true;
		}
		if (method == MethodName.Collection && args.Count == 0)
		{
			Collection();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ClearStaticBattleReferences && args.Count == 0)
		{
			ClearStaticBattleReferences();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateDescendantAdobeAnimateRenderOrder && args.Count == 2)
		{
			InvalidateDescendantAdobeAnimateRenderOrder(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkDescendantAdobeAnimateRenderOrderDebug && args.Count == 2)
		{
			MarkDescendantAdobeAnimateRenderOrderDebug(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountDescendantAdobeAnimateSprites && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountDescendantAdobeAnimateSprites(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPathOrName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPathOrName(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.OnGroundHeightChanged)
		{
			return true;
		}
		if (method == MethodName.OnGridPositionChanged)
		{
			return true;
		}
		if (method == MethodName.ClearStaticBattleReferences)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.PhysiceUpdate)
		{
			return true;
		}
		if (method == MethodName.CompleteLanding)
		{
			return true;
		}
		if (method == MethodName.SetZ)
		{
			return true;
		}
		if (method == MethodName.FreshZIndex)
		{
			return true;
		}
		if (method == MethodName.FreshZIndexForGlobalPosition)
		{
			return true;
		}
		if (method == MethodName.SetTransientRenderZIndex)
		{
			return true;
		}
		if (method == MethodName.ApplyRenderZIndex)
		{
			return true;
		}
		if (method == MethodName.ResolveRenderGridY)
		{
			return true;
		}
		if (method == MethodName.InvalidateDescendantAdobeAnimateRenderOrder)
		{
			return true;
		}
		if (method == MethodName.MarkDescendantAdobeAnimateRenderOrderDebug)
		{
			return true;
		}
		if (method == MethodName.BuildGridRenderOrderReason)
		{
			return true;
		}
		if (method == MethodName.DebugPrintGridRenderOrderChange)
		{
			return true;
		}
		if (method == MethodName.CountDescendantAdobeAnimateSprites)
		{
			return true;
		}
		if (method == MethodName.GetPathOrName)
		{
			return true;
		}
		if (method == MethodName.GetFallTime)
		{
			return true;
		}
		if (method == MethodName.Collection)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.groundHeight)
		{
			groundHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.z)
		{
			z = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.itemLayer)
		{
			itemLayer = VariantUtils.ConvertTo<TowerDefenseEnum.LAYER_GROUNDITEM>(in value);
			return true;
		}
		if (name == PropertyName.topLayer)
		{
			topLayer = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._groundHeight)
		{
			_groundHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._z)
		{
			_z = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._gridPos)
		{
			_gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._itemLayer)
		{
			_itemLayer = VariantUtils.ConvertTo<TowerDefenseEnum.LAYER_GROUNDITEM>(in value);
			return true;
		}
		if (name == PropertyName.cell)
		{
			cell = VariantUtils.ConvertTo<TowerDefenseCellInstance>(in value);
			return true;
		}
		if (name == PropertyName.cellPercentage)
		{
			cellPercentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gravityUse)
		{
			gravityUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			gravity = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.gravityScale)
		{
			gravityScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			ySpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._topLayer)
		{
			_topLayer = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isGround)
		{
			isGround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._insidePhysicsUpdate)
		{
			_insidePhysicsUpdate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		double from;
		if (name == PropertyName.groundHeight)
		{
			from = groundHeight;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.z)
		{
			from = z;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom<Vector2I>(gridPos);
			return true;
		}
		if (name == PropertyName.itemLayer)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.LAYER_GROUNDITEM>(itemLayer);
			return true;
		}
		bool from2;
		if (name == PropertyName.topLayer)
		{
			from2 = topLayer;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ZChangedInsidePhysicsUpdate)
		{
			from2 = ZChangedInsidePhysicsUpdate;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._groundHeight)
		{
			value = VariantUtils.CreateFrom(in _groundHeight);
			return true;
		}
		if (name == PropertyName._z)
		{
			value = VariantUtils.CreateFrom(in _z);
			return true;
		}
		if (name == PropertyName._gridPos)
		{
			value = VariantUtils.CreateFrom(in _gridPos);
			return true;
		}
		if (name == PropertyName._itemLayer)
		{
			value = VariantUtils.CreateFrom(in _itemLayer);
			return true;
		}
		if (name == PropertyName.cell)
		{
			value = VariantUtils.CreateFrom(in cell);
			return true;
		}
		if (name == PropertyName.cellPercentage)
		{
			value = VariantUtils.CreateFrom(in cellPercentage);
			return true;
		}
		if (name == PropertyName.gravityUse)
		{
			value = VariantUtils.CreateFrom(in gravityUse);
			return true;
		}
		if (name == PropertyName.gravity)
		{
			value = VariantUtils.CreateFrom(in gravity);
			return true;
		}
		if (name == PropertyName.gravityScale)
		{
			value = VariantUtils.CreateFrom(in gravityScale);
			return true;
		}
		if (name == PropertyName.ySpeed)
		{
			value = VariantUtils.CreateFrom(in ySpeed);
			return true;
		}
		if (name == PropertyName._topLayer)
		{
			value = VariantUtils.CreateFrom(in _topLayer);
			return true;
		}
		if (name == PropertyName.isGround)
		{
			value = VariantUtils.CreateFrom(in isGround);
			return true;
		}
		if (name == PropertyName._insidePhysicsUpdate)
		{
			value = VariantUtils.CreateFrom(in _insidePhysicsUpdate);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._groundHeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.groundHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._z, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.z, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._gridPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._itemLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.itemLayer, PropertyHint.Enum, "DEFAULT:0,GROUNDITEM:1,PLANT_UNDER:2,PLANT_BACK:3,PLANT:4,PLANT_FRONT:5,PLANT_AIR:6,ZOMBIE:7,DAMAGEPART:8,PROJECTILE:9,EFFECT:10,MAX:15", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.cell, PropertyHint.ResourceType, "TowerDefenseCellInstance", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cellPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.gravityUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.gravityScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ySpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._topLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.topLayer, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGround, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._insidePhysicsUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ZChangedInsidePhysicsUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.groundHeight, Variant.From<double>(groundHeight));
		info.AddProperty(PropertyName.z, Variant.From<double>(z));
		info.AddProperty(PropertyName.gridPos, Variant.From<Vector2I>(gridPos));
		info.AddProperty(PropertyName.itemLayer, Variant.From<TowerDefenseEnum.LAYER_GROUNDITEM>(itemLayer));
		info.AddProperty(PropertyName.topLayer, Variant.From<bool>(topLayer));
		info.AddProperty(PropertyName._groundHeight, Variant.From(in _groundHeight));
		info.AddProperty(PropertyName._z, Variant.From(in _z));
		info.AddProperty(PropertyName._gridPos, Variant.From(in _gridPos));
		info.AddProperty(PropertyName._itemLayer, Variant.From(in _itemLayer));
		info.AddProperty(PropertyName.cell, Variant.From(in cell));
		info.AddProperty(PropertyName.cellPercentage, Variant.From(in cellPercentage));
		info.AddProperty(PropertyName.gravityUse, Variant.From(in gravityUse));
		info.AddProperty(PropertyName.gravity, Variant.From(in gravity));
		info.AddProperty(PropertyName.gravityScale, Variant.From(in gravityScale));
		info.AddProperty(PropertyName.ySpeed, Variant.From(in ySpeed));
		info.AddProperty(PropertyName._topLayer, Variant.From(in _topLayer));
		info.AddProperty(PropertyName.isGround, Variant.From(in isGround));
		info.AddProperty(PropertyName._insidePhysicsUpdate, Variant.From(in _insidePhysicsUpdate));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.groundHeight, out var value))
		{
			groundHeight = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.z, out var value2))
		{
			z = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value3))
		{
			gridPos = value3.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.itemLayer, out var value4))
		{
			itemLayer = value4.As<TowerDefenseEnum.LAYER_GROUNDITEM>();
		}
		if (info.TryGetProperty(PropertyName.topLayer, out var value5))
		{
			topLayer = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._groundHeight, out var value6))
		{
			_groundHeight = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._z, out var value7))
		{
			_z = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._gridPos, out var value8))
		{
			_gridPos = value8.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._itemLayer, out var value9))
		{
			_itemLayer = value9.As<TowerDefenseEnum.LAYER_GROUNDITEM>();
		}
		if (info.TryGetProperty(PropertyName.cell, out var value10))
		{
			cell = value10.As<TowerDefenseCellInstance>();
		}
		if (info.TryGetProperty(PropertyName.cellPercentage, out var value11))
		{
			cellPercentage = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gravityUse, out var value12))
		{
			gravityUse = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.gravity, out var value13))
		{
			gravity = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.gravityScale, out var value14))
		{
			gravityScale = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ySpeed, out var value15))
		{
			ySpeed = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName._topLayer, out var value16))
		{
			_topLayer = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isGround, out var value17))
		{
			isGround = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._insidePhysicsUpdate, out var value18))
		{
			_insidePhysicsUpdate = value18.As<bool>();
		}
	}
}
