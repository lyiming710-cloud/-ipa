using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

public sealed class TowerDefenseHealthDisplayBatchLayer : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName RequestRedraw = "RequestRedraw";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName CollectDrawData = "CollectDrawData";

		public static readonly StringName DrawPass = "DrawPass";

		public static readonly StringName DrawHealthLine = "DrawHealthLine";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName Font = "Font";

		public static readonly StringName OwnerBatch = "OwnerBatch";

		public static readonly StringName DisplayCount = "DisplayCount";

		public static readonly StringName _redrawRequested = "_redrawRequested";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const int OutlineSize = 5;

	private readonly List<ShowHealthComponent> _displays = new List<ShowHealthComponent>(64);

	private readonly List<HealthDisplayDrawData> _drawData = new List<HealthDisplayDrawData>(64);

	private bool _redrawRequested;

	internal Font Font { get; set; }

	internal HealthTextLayoutCache TextLayoutCache { get; set; }

	internal TowerDefenseHealthDisplayBatch OwnerBatch { get; set; }

	internal int DisplayCount => _displays.Count;

	internal void Add(ShowHealthComponent display)
	{
		_displays.Add(display);
		RequestRedraw();
	}

	internal void Remove(ShowHealthComponent display)
	{
		int num = _displays.IndexOf(display);
		if (num >= 0)
		{
			_displays.RemoveAt(num);
			RequestRedraw();
		}
	}

	internal void RequestRedraw()
	{
		if (!_redrawRequested)
		{
			_redrawRequested = true;
			QueueRedraw();
		}
	}

	public override void _Ready()
	{
		TopLevel = true;
		GlobalTransform = Transform2D.Identity;
		SetProcess(enable: false);
	}

	public override void _Draw()
	{
		_redrawRequested = false;
		if (GodotObject.IsInstanceValid(Font))
		{
			CollectDrawData();
			float height = Font.GetHeight(14);
			Rid canvasItem = GetCanvasItem();
			DrawPass(outline: true, height, canvasItem);
			DrawPass(outline: false, height, canvasItem);
			OwnerBatch?.ReportDrawRebuild();
		}
	}

	private void CollectDrawData()
	{
		_drawData.Clear();
		for (int i = 0; i < _displays.Count; i++)
		{
			ShowHealthComponent showHealthComponent = _displays[i];
			if (showHealthComponent != null && showHealthComponent.TryGetSharedDrawData(out var data))
			{
				_drawData.Add(data);
			}
		}
	}

	private void DrawPass(bool outline, float lineHeight, Rid canvasItem)
	{
		for (int i = 0; i < _drawData.Count; i++)
		{
			DrawDisplay(_drawData[i], outline, lineHeight, canvasItem);
		}
	}

	private void DrawDisplay(in HealthDisplayDrawData data, bool outline, float lineHeight, Rid canvasItem)
	{
		int num = 0;
		if (!string.IsNullOrEmpty(data.ShieldText))
		{
			num++;
		}
		if (!string.IsNullOrEmpty(data.HelmetText))
		{
			num++;
		}
		if (!string.IsNullOrEmpty(data.BodyText))
		{
			num++;
		}
		if (num != 0)
		{
			float num2 = data.Center.Y - lineHeight * (float)num * 0.5f;
			float x = data.Center.X - 56f;
			if (!string.IsNullOrEmpty(data.ShieldText))
			{
				DrawHealthLine(canvasItem, new Vector2(x, num2), data.ShieldText, data.ShieldColor, outline);
				num2 += lineHeight;
			}
			if (!string.IsNullOrEmpty(data.HelmetText))
			{
				DrawHealthLine(canvasItem, new Vector2(x, num2), data.HelmetText, data.HelmetColor, outline);
				num2 += lineHeight;
			}
			if (!string.IsNullOrEmpty(data.BodyText))
			{
				DrawHealthLine(canvasItem, new Vector2(x, num2), data.BodyText, data.BodyColor, outline);
			}
		}
	}

	private void DrawHealthLine(Rid canvasItem, Vector2 topLeft, string text, Color color, bool outline)
	{
		CachedTextLine cachedTextLine = TextLayoutCache?.GetOrCreate(text);
		if (cachedTextLine != null)
		{
			if (outline)
			{
				cachedTextLine.Line.DrawOutline(canvasItem, topLeft, 5, Colors.Black);
			}
			else
			{
				cachedTextLine.Line.Draw(canvasItem, topLeft, color);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName.RequestRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CollectDrawData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawPass, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "lineHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rid, "canvasItem", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawHealthLine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rid, "canvasItem", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "topLeft", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RequestRedraw && args.Count == 0)
		{
			RequestRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.CollectDrawData && args.Count == 0)
		{
			CollectDrawData();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawPass && args.Count == 3)
		{
			DrawPass(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<Rid>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawHealthLine && args.Count == 5)
		{
			DrawHealthLine(VariantUtils.ConvertTo<Rid>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RequestRedraw)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.CollectDrawData)
		{
			return true;
		}
		if (method == MethodName.DrawPass)
		{
			return true;
		}
		if (method == MethodName.DrawHealthLine)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Font)
		{
			Font = VariantUtils.ConvertTo<Font>(in value);
			return true;
		}
		if (name == PropertyName.OwnerBatch)
		{
			OwnerBatch = VariantUtils.ConvertTo<TowerDefenseHealthDisplayBatch>(in value);
			return true;
		}
		if (name == PropertyName._redrawRequested)
		{
			_redrawRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Font)
		{
			value = VariantUtils.CreateFrom<Font>(Font);
			return true;
		}
		if (name == PropertyName.OwnerBatch)
		{
			value = VariantUtils.CreateFrom<TowerDefenseHealthDisplayBatch>(OwnerBatch);
			return true;
		}
		if (name == PropertyName.DisplayCount)
		{
			value = VariantUtils.CreateFrom<int>(DisplayCount);
			return true;
		}
		if (name == PropertyName._redrawRequested)
		{
			value = VariantUtils.CreateFrom(in _redrawRequested);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Font, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.OwnerBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DisplayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._redrawRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Font, Variant.From<Font>(Font));
		info.AddProperty(PropertyName.OwnerBatch, Variant.From<TowerDefenseHealthDisplayBatch>(OwnerBatch));
		info.AddProperty(PropertyName._redrawRequested, Variant.From(in _redrawRequested));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Font, out var value))
		{
			Font = value.As<Font>();
		}
		if (info.TryGetProperty(PropertyName.OwnerBatch, out var value2))
		{
			OwnerBatch = value2.As<TowerDefenseHealthDisplayBatch>();
		}
		if (info.TryGetProperty(PropertyName._redrawRequested, out var value3))
		{
			_redrawRequested = value3.As<bool>();
		}
	}
}
