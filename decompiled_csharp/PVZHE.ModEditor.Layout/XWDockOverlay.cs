using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Layout;

[ScriptPath("res://addons/ModEditor/Layout/XWDockOverlay.cs")]
public class XWDockOverlay : Control
{
	public enum Edge
	{
		Left,
		Right,
		Top,
		Bottom,
		None
	}

	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitEdgeAdjacency = "InitEdgeAdjacency";

		public static readonly StringName UpdateHighlight = "UpdateHighlight";

		public static readonly StringName ClearHighlight = "ClearHighlight";

		public static readonly StringName GetHighlightZone = "GetHighlightZone";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName ComputeEdgeRect = "ComputeEdgeRect";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _highlightZone = "_highlightZone";

		public static readonly StringName _highlightEdge = "_highlightEdge";

		public static readonly StringName _highlightRect = "_highlightRect";

		public static readonly StringName _leftRect = "_leftRect";

		public static readonly StringName _rightRect = "_rightRect";

		public static readonly StringName _centerRect = "_centerRect";

		public static readonly StringName _bottomRect = "_bottomRect";

		public static readonly StringName CenterZoneEnabled = "CenterZoneEnabled";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const float EdgeThreshold = 30f;

	private int _highlightZone = -1;

	private Edge _highlightEdge = Edge.None;

	private Rect2 _highlightRect;

	private Rect2 _leftRect;

	private Rect2 _rightRect;

	private Rect2 _centerRect;

	private Rect2 _bottomRect;

	private readonly Dictionary<int, Rect2> _subZones = new Dictionary<int, Rect2>();

	private readonly Dictionary<int, Dictionary<Edge, int>> _edgeAdjacency = new Dictionary<int, Dictionary<Edge, int>>();

	public bool CenterZoneEnabled = true;

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Ignore;
		AnchorRight = 1f;
		AnchorBottom = 1f;
		InitEdgeAdjacency();
	}

	private void InitEdgeAdjacency()
	{
		_edgeAdjacency[4] = new Dictionary<Edge, int>
		{
			{
				Edge.Right,
				6
			},
			{
				Edge.Bottom,
				5
			}
		};
		_edgeAdjacency[5] = new Dictionary<Edge, int>
		{
			{
				Edge.Top,
				4
			},
			{
				Edge.Right,
				7
			},
			{
				Edge.Bottom,
				3
			}
		};
		_edgeAdjacency[6] = new Dictionary<Edge, int>
		{
			{
				Edge.Left,
				4
			},
			{
				Edge.Right,
				2
			},
			{
				Edge.Bottom,
				7
			}
		};
		_edgeAdjacency[7] = new Dictionary<Edge, int>
		{
			{
				Edge.Top,
				6
			},
			{
				Edge.Left,
				5
			},
			{
				Edge.Right,
				2
			},
			{
				Edge.Bottom,
				3
			}
		};
		_edgeAdjacency[8] = new Dictionary<Edge, int>
		{
			{
				Edge.Left,
				2
			},
			{
				Edge.Right,
				10
			},
			{
				Edge.Bottom,
				9
			}
		};
		_edgeAdjacency[9] = new Dictionary<Edge, int>
		{
			{
				Edge.Top,
				8
			},
			{
				Edge.Left,
				2
			},
			{
				Edge.Right,
				11
			},
			{
				Edge.Bottom,
				3
			}
		};
		_edgeAdjacency[10] = new Dictionary<Edge, int>
		{
			{
				Edge.Left,
				8
			},
			{
				Edge.Bottom,
				11
			}
		};
		_edgeAdjacency[11] = new Dictionary<Edge, int>
		{
			{
				Edge.Top,
				10
			},
			{
				Edge.Left,
				9
			},
			{
				Edge.Bottom,
				3
			}
		};
		_edgeAdjacency[2] = new Dictionary<Edge, int>
		{
			{
				Edge.Left,
				6
			},
			{
				Edge.Right,
				8
			},
			{
				Edge.Bottom,
				3
			}
		};
		_edgeAdjacency[3] = new Dictionary<Edge, int>
		{
			{
				Edge.Top,
				2
			},
			{
				Edge.Left,
				4
			},
			{
				Edge.Right,
				8
			}
		};
	}

	public void UpdateZones(Rect2 left, Rect2 right, Rect2 center, Rect2 bottom, Dictionary<int, Rect2> subZones = null)
	{
		_leftRect = left;
		_rightRect = right;
		_centerRect = center;
		_bottomRect = bottom;
		_subZones.Clear();
		if (subZones == null)
		{
			return;
		}
		foreach (KeyValuePair<int, Rect2> subZone in subZones)
		{
			_subZones[subZone.Key] = subZone.Value;
		}
	}

	public void UpdateHighlight(Vector2 globalMousePos)
	{
		Vector2 pos = globalMousePos - GlobalPosition;
		var (num, edge, highlightRect) = GetZoneAndEdgeAtPosition(pos);
		if (num != _highlightZone || edge != _highlightEdge)
		{
			_highlightZone = num;
			_highlightEdge = edge;
			_highlightRect = highlightRect;
			QueueRedraw();
		}
	}

	public void ClearHighlight()
	{
		if (_highlightZone != -1)
		{
			_highlightZone = -1;
			_highlightEdge = Edge.None;
			_highlightRect = default;
			QueueRedraw();
		}
	}

	public int GetHighlightZone()
	{
		return _highlightZone;
	}

	public override void _Draw()
	{
		if (_highlightZone == -1)
		{
			return;
		}
		Rect2 highlightRect = _highlightRect;
		if (!(highlightRect.Size.X <= 0f) && !(highlightRect.Size.Y <= 0f))
		{
			Color color;
			Color color2;
			Color color3;
			float width;
			float width2;
			if (_highlightEdge != Edge.None)
			{
				color = new Color(0.04f, 0.54f, 1f, 0.18f);
				color2 = new Color(0.04f, 0.54f, 1f, 0.9f);
				color3 = new Color(0.04f, 0.54f, 1f, 0.15f);
				width = 2f;
				width2 = 1f;
			}
			else
			{
				color = new Color(0.04f, 0.54f, 1f, 0.1f);
				color2 = new Color(0.04f, 0.54f, 1f, 0.7f);
				color3 = new Color(0.04f, 0.54f, 1f, 0.1f);
				width = 2f;
				width2 = 1f;
			}
			DrawRect(highlightRect, color);
			Rect2 rect = highlightRect.Grow(-4f);
			if (rect.Size.X > 0f && rect.Size.Y > 0f)
			{
				Color color4 = ((_highlightEdge != Edge.None) ? new Color(0.04f, 0.54f, 1f, 0.08f) : new Color(0.04f, 0.54f, 1f, 0.05f));
				DrawRect(rect, color4);
			}
			DrawRect(highlightRect, color2, filled: false, width);
			Rect2 rect2 = highlightRect.Grow((_highlightEdge != Edge.None) ? 4 : 6);
			DrawRect(rect2, color3, filled: false, width2);
		}
	}

	private (int zone, Edge edge, Rect2 rect) GetZoneAndEdgeAtPosition(Vector2 pos)
	{
		foreach (KeyValuePair<int, Rect2> subZone in _subZones)
		{
			int key = subZone.Key;
			Rect2 value = subZone.Value;
			if (value.HasPoint(pos) && (key != 2 || CenterZoneEnabled))
			{
				var (num, item, item2) = CheckEdges(pos, value, key);
				if (num >= 0)
				{
					return (zone: num, edge: item, rect: item2);
				}
				return (zone: key, edge: Edge.None, rect: value);
			}
		}
		if (_bottomRect.HasPoint(pos))
		{
			var (num2, item3, item4) = CheckEdges(pos, _bottomRect, 3);
			if (num2 >= 0)
			{
				return (zone: num2, edge: item3, rect: item4);
			}
			return (zone: 3, edge: Edge.None, rect: _bottomRect);
		}
		if (CenterZoneEnabled && _centerRect.HasPoint(pos))
		{
			var (num3, item5, item6) = CheckEdges(pos, _centerRect, 2);
			if (num3 >= 0)
			{
				return (zone: num3, edge: item5, rect: item6);
			}
			return (zone: 2, edge: Edge.None, rect: _centerRect);
		}
		return (zone: -1, edge: Edge.None, rect: default);
	}

	private (int zone, Edge edge, Rect2 rect) CheckEdges(Vector2 pos, Rect2 zoneRect, int dockPos)
	{
		if (!_edgeAdjacency.TryGetValue(dockPos, out var value))
		{
			return (zone: -1, edge: Edge.None, rect: default);
		}
		float num = Mathf.Min(30f, zoneRect.Size.X * 0.3f);
		float num2 = Mathf.Min(30f, zoneRect.Size.Y * 0.3f);
		Edge edge = Edge.None;
		float num3 = 3.4028235E+38f;
		if (value.ContainsKey(Edge.Left))
		{
			float num4 = pos.X - zoneRect.Position.X;
			if (num4 < num && num4 < num3)
			{
				num3 = num4;
				edge = Edge.Left;
			}
		}
		if (value.ContainsKey(Edge.Right))
		{
			float num5 = zoneRect.End.X - pos.X;
			if (num5 < num && num5 < num3)
			{
				num3 = num5;
				edge = Edge.Right;
			}
		}
		if (value.ContainsKey(Edge.Top))
		{
			float num6 = pos.Y - zoneRect.Position.Y;
			if (num6 < num2 && num6 < num3)
			{
				num3 = num6;
				edge = Edge.Top;
			}
		}
		if (value.ContainsKey(Edge.Bottom))
		{
			float num7 = zoneRect.End.Y - pos.Y;
			if (num7 < num2 && num7 < num3)
			{
				num3 = num7;
				edge = Edge.Bottom;
			}
		}
		if (edge != Edge.None)
		{
			int num8 = value[edge];
			if (num8 == 2 && !CenterZoneEnabled)
			{
				return (zone: -1, edge: Edge.None, rect: default);
			}
			Rect2 item = ComputeEdgeRect(zoneRect, edge, num, num2);
			return (zone: num8, edge: edge, rect: item);
		}
		return (zone: -1, edge: Edge.None, rect: default);
	}

	private static Rect2 ComputeEdgeRect(Rect2 zoneRect, Edge edge, float hThreshold, float vThreshold)
	{
		return edge switch
		{
			Edge.Left => new Rect2(zoneRect.Position.X, zoneRect.Position.Y, hThreshold, zoneRect.Size.Y), 
			Edge.Right => new Rect2(zoneRect.End.X - hThreshold, zoneRect.Position.Y, hThreshold, zoneRect.Size.Y), 
			Edge.Top => new Rect2(zoneRect.Position.X, zoneRect.Position.Y, zoneRect.Size.X, vThreshold), 
			Edge.Bottom => new Rect2(zoneRect.Position.X, zoneRect.End.Y - vThreshold, zoneRect.Size.X, vThreshold), 
			_ => default, 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitEdgeAdjacency, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateHighlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalMousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearHighlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHighlightZone, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeEdgeRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "zoneRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "edge", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "hThreshold", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "vThreshold", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.InitEdgeAdjacency && args.Count == 0)
		{
			InitEdgeAdjacency();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateHighlight && args.Count == 1)
		{
			UpdateHighlight(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearHighlight && args.Count == 0)
		{
			ClearHighlight();
			ret = default;
			return true;
		}
		if (method == MethodName.GetHighlightZone && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetHighlightZone());
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.ComputeEdgeRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Rect2>(ComputeEdgeRect(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Edge>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ComputeEdgeRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Rect2>(ComputeEdgeRect(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Edge>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
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
		if (method == MethodName.InitEdgeAdjacency)
		{
			return true;
		}
		if (method == MethodName.UpdateHighlight)
		{
			return true;
		}
		if (method == MethodName.ClearHighlight)
		{
			return true;
		}
		if (method == MethodName.GetHighlightZone)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.ComputeEdgeRect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._highlightZone)
		{
			_highlightZone = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._highlightEdge)
		{
			_highlightEdge = VariantUtils.ConvertTo<Edge>(in value);
			return true;
		}
		if (name == PropertyName._highlightRect)
		{
			_highlightRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._leftRect)
		{
			_leftRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._rightRect)
		{
			_rightRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._centerRect)
		{
			_centerRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._bottomRect)
		{
			_bottomRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.CenterZoneEnabled)
		{
			CenterZoneEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._highlightZone)
		{
			value = VariantUtils.CreateFrom(in _highlightZone);
			return true;
		}
		if (name == PropertyName._highlightEdge)
		{
			value = VariantUtils.CreateFrom(in _highlightEdge);
			return true;
		}
		if (name == PropertyName._highlightRect)
		{
			value = VariantUtils.CreateFrom(in _highlightRect);
			return true;
		}
		if (name == PropertyName._leftRect)
		{
			value = VariantUtils.CreateFrom(in _leftRect);
			return true;
		}
		if (name == PropertyName._rightRect)
		{
			value = VariantUtils.CreateFrom(in _rightRect);
			return true;
		}
		if (name == PropertyName._centerRect)
		{
			value = VariantUtils.CreateFrom(in _centerRect);
			return true;
		}
		if (name == PropertyName._bottomRect)
		{
			value = VariantUtils.CreateFrom(in _bottomRect);
			return true;
		}
		if (name == PropertyName.CenterZoneEnabled)
		{
			value = VariantUtils.CreateFrom(in CenterZoneEnabled);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._highlightZone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._highlightEdge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._highlightRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._leftRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._rightRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._centerRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._bottomRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CenterZoneEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._highlightZone, Variant.From(in _highlightZone));
		info.AddProperty(PropertyName._highlightEdge, Variant.From(in _highlightEdge));
		info.AddProperty(PropertyName._highlightRect, Variant.From(in _highlightRect));
		info.AddProperty(PropertyName._leftRect, Variant.From(in _leftRect));
		info.AddProperty(PropertyName._rightRect, Variant.From(in _rightRect));
		info.AddProperty(PropertyName._centerRect, Variant.From(in _centerRect));
		info.AddProperty(PropertyName._bottomRect, Variant.From(in _bottomRect));
		info.AddProperty(PropertyName.CenterZoneEnabled, Variant.From(in CenterZoneEnabled));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._highlightZone, out var value))
		{
			_highlightZone = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._highlightEdge, out var value2))
		{
			_highlightEdge = value2.As<Edge>();
		}
		if (info.TryGetProperty(PropertyName._highlightRect, out var value3))
		{
			_highlightRect = value3.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._leftRect, out var value4))
		{
			_leftRect = value4.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._rightRect, out var value5))
		{
			_rightRect = value5.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._centerRect, out var value6))
		{
			_centerRect = value6.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._bottomRect, out var value7))
		{
			_bottomRect = value7.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.CenterZoneEnabled, out var value8))
		{
			CenterZoneEnabled = value8.As<bool>();
		}
	}
}
