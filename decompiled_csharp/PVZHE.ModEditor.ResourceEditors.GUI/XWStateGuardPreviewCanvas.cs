using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/StateMachine/XWStateGuardPreviewCanvas.cs")]
public class XWStateGuardPreviewCanvas : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Bind = "Bind";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName DrawGrid = "DrawGrid";

		public static readonly StringName DrawWire = "DrawWire";

		public static readonly StringName DrawCard = "DrawCard";

		public static readonly StringName DrawCentered = "DrawCentered";

		public static readonly StringName CreatePanelStyle = "CreatePanelStyle";

		public static readonly StringName GetGuardTypeLabel = "GetGuardTypeLabel";

		public static readonly StringName Compare = "Compare";

		public static readonly StringName OperatorGlyph = "OperatorGlyph";

		public static readonly StringName FormatValue = "FormatValue";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName _guard = "_guard";

		public static readonly StringName _actualValue = "_actualValue";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const int MaximumGuardTreeDepth = 32;

	private const int MaximumGuardTreeNodes = 256;

	private Resource _guard;

	private Variant _actualValue;

	private bool? _result;

	public bool? PreviewResult => _result;

	public override void _Ready()
	{
		MouseFilter = MouseFilterEnum.Ignore;
		SetProcess(enable: false);
		QueueRedraw();
	}

	public void Bind(Resource guard, Variant actualValue)
	{
		_guard = guard;
		_actualValue = actualValue;
		_result = EvaluatePreview(guard, actualValue);
		QueueRedraw();
	}

	public override void _Draw()
	{
		Rect2 rect = new Rect2(Vector2.Zero, Size);
		DrawStyleBox(CreatePanelStyle(new Color("101810"), new Color("445c2b"), 12), rect);
		DrawGrid(rect);
		if (!GodotObject.IsInstanceValid(_guard))
		{
			DrawCentered("未选择状态条件", new Rect2(20f, 20f, Math.Max(0f, Size.X - 40f), Math.Max(0f, Size.Y - 40f)), 18, new Color("c5d7a6"));
			return;
		}
		float num = Math.Max(112f, Math.Min(172f, (Size.X - 72f) / 3f));
		float num2 = 84f;
		float y = Math.Max(34f, (Size.Y - num2) * 0.48f);
		Rect2 rect2 = new Rect2(18f, y, num, num2);
		Rect2 rect3 = new Rect2((Size.X - num) * 0.5f, y, num, num2);
		Rect2 rect4 = new Rect2(Size.X - num - 18f, y, num, num2);
		DrawWire(rect2, rect3, new Color("7aa94e"));
		Rect2 left = rect3;
		Rect2 right = rect4;
		bool? result = _result;
		Color color;
		if (result.HasValue)
		{
			color = ((result != true) ? new Color("dc6b48") : new Color("85d94f"));
		}
		else
		{
			color = new Color("9eaa8c");
		}
		DrawWire(left, right, color);
		var (title, value, title2, value2) = Describe(_guard, _actualValue);
		DrawCard(rect2, title, value, new Color("24371d"), new Color("668a42"));
		DrawCard(rect3, title2, value2, new Color("283026"), new Color("8da35b"));
		result = _result;
		string text;
		if (result.HasValue)
		{
			text = ((result != true) ? "阻止切换" : "允许切换");
		}
		else
		{
			text = "等待运行时";
		}
		string text2 = text;
		string text3 = (_result.HasValue ? "实时模拟结果" : "需要场景上下文");
		XWStateGuardPreviewCanvas xWStateGuardPreviewCanvas = this;
		Rect2 rect5 = rect4;
		string text4 = text2;
		string text5 = text3;
		Color color2;
		if (_result == true)
		{
			color2 = new Color("24451e");
		}
		else
		{
			color2 = ((_result == false) ? new Color("4a241c") : new Color("30342d"));
		}
		XWStateGuardPreviewCanvas xWStateGuardPreviewCanvas2 = xWStateGuardPreviewCanvas;
		Rect2 rect6 = rect5;
		string title3 = text4;
		string value3 = text5;
		Color fill = color2;
		Color outline;
		if (_result == true)
		{
			outline = new Color("80c94a");
		}
		else
		{
			outline = ((_result == false) ? new Color("d26849") : new Color("8e9a82"));
		}
		xWStateGuardPreviewCanvas2.DrawCard(rect6, title3, value3, fill, outline);
		DrawCentered(GetGuardTypeLabel(_guard), new Rect2(12f, 8f, Math.Max(0f, Size.X - 24f), 24f), 16, new Color("e8f3c6"));
		DrawCentered("条件资源只在输入变化时重绘，不占用隐藏面板帧循环", new Rect2(12f, Math.Max(0f, Size.Y - 30f), Math.Max(0f, Size.X - 24f), 22f), 12, new Color("9eae8d"));
	}

	private void DrawGrid(Rect2 area)
	{
		Color color = new Color("25341f");
		for (float num = 24f; num < area.Size.X; num += 24f)
		{
			DrawLine(new Vector2(num, 0f), new Vector2(num, area.Size.Y), color, 1f);
		}
		for (float num2 = 24f; num2 < area.Size.Y; num2 += 24f)
		{
			DrawLine(new Vector2(0f, num2), new Vector2(area.Size.X, num2), color, 1f);
		}
	}

	private void DrawWire(Rect2 left, Rect2 right, Color color)
	{
		Vector2 vector = new Vector2(left.End.X, left.GetCenter().Y);
		Vector2 vector2 = new Vector2(right.Position.X, right.GetCenter().Y);
		float x = Math.Max(18f, (vector2.X - vector.X) * 0.45f);
		Vector2[] points = new Vector2[4]
		{
			vector,
			vector + new Vector2(x, 0f),
			vector2 - new Vector2(x, 0f),
			vector2
		};
		DrawPolyline(points, color, 4f, antialiased: true);
		DrawCircle(vector, 5f, color);
		DrawCircle(vector2, 5f, color);
	}

	private void DrawCard(Rect2 rect, string title, string value, Color fill, Color outline)
	{
		DrawStyleBox(CreatePanelStyle(fill, outline, 9), rect);
		DrawCentered(title, new Rect2(rect.Position + new Vector2(8f, 12f), rect.Size - new Vector2(16f, 44f)), 16, Colors.White);
		DrawCentered(value, new Rect2(rect.Position + new Vector2(8f, 48f), rect.Size - new Vector2(16f, 54f)), 12, new Color("d4dfbe"));
	}

	private void DrawCentered(string text, Rect2 rect, int fontSize, Color color)
	{
		DrawString(ThemeDB.FallbackFont, rect.Position + new Vector2(0f, fontSize), text ?? string.Empty, HorizontalAlignment.Center, rect.Size.X, fontSize, color, TextServer.JustificationFlag.Kashida | TextServer.JustificationFlag.WordBound, TextServer.Direction.Auto, TextServer.Orientation.Horizontal, 0f);
	}

	private static StyleBoxFlat CreatePanelStyle(Color fill, Color outline, int radius)
	{
		return new StyleBoxFlat
		{
			BgColor = fill,
			BorderColor = outline,
			BorderWidthLeft = 2,
			BorderWidthTop = 2,
			BorderWidthRight = 2,
			BorderWidthBottom = 2,
			CornerRadiusTopLeft = radius,
			CornerRadiusTopRight = radius,
			CornerRadiusBottomLeft = radius,
			CornerRadiusBottomRight = radius
		};
	}

	private static (string InputTitle, string InputValue, string GateTitle, string GateValue) Describe(Resource guard, Variant actual)
	{
		if (!(guard is StateMachineGuardDefinition stateMachineGuardDefinition))
		{
			if (!(guard is ExpressionGuard expressionGuard))
			{
				if (!(guard is StateIsActiveGuard stateIsActiveGuard))
				{
					if (!(guard is AllOfGuard allOfGuard))
					{
						if (!(guard is AnyOfGuard anyOfGuard))
						{
							if (guard is NotGuard notGuard)
							{
								return (InputTitle: "子条件", InputValue: GodotObject.IsInstanceValid(notGuard.guard) ? "已连接" : "未连接", GateTitle: "结果取反", GateValue: "NOT");
							}
							return (InputTitle: "状态输入", InputValue: "运行时", GateTitle: "自定义条件", GateValue: guard.GetType().Name);
						}
						return (InputTitle: "子条件", InputValue: $"{anyOfGuard.guards?.Count ?? 0} 项", GateTitle: "任一满足", GateValue: "OR");
					}
					return (InputTitle: "子条件", InputValue: $"{allOfGuard.guards?.Count ?? 0} 项", GateTitle: "全部满足", GateValue: "AND");
				}
				return (InputTitle: "状态树", InputValue: stateIsActiveGuard.state.ToString(), GateTitle: "激活？", GateValue: "监听状态变化");
			}
			return (InputTitle: "表达式输入", InputValue: "场景属性", GateTitle: "ƒ(x)", GateValue: string.IsNullOrWhiteSpace(expressionGuard.expression) ? "未填写表达式" : expressionGuard.expression);
		}
		if (stateMachineGuardDefinition.Kind == StateMachineGuardKind.Callback)
		{
			return (InputTitle: "C# 转移上下文", InputValue: "实际宿主", GateTitle: "C# bool", GateValue: string.IsNullOrWhiteSpace(stateMachineGuardDefinition.CallbackKey.ToString()) ? "尚未选择完整键" : (stateMachineGuardDefinition.CallbackKey.ToString() + (stateMachineGuardDefinition.Negate ? " · 取反" : "")));
		}
		StateMachineGuardDefinition stateMachineGuardDefinition2 = stateMachineGuardDefinition;
		return (InputTitle: string.IsNullOrWhiteSpace(stateMachineGuardDefinition2.ComparedProperty.ToString()) ? "状态属性" : stateMachineGuardDefinition2.ComparedProperty.ToString(), InputValue: FormatValue(actual), GateTitle: OperatorGlyph(stateMachineGuardDefinition2.Operator), GateValue: FormatValue(stateMachineGuardDefinition2.ExpectedValue) + (stateMachineGuardDefinition2.Negate ? " · 取反" : ""));
	}

	private static string GetGuardTypeLabel(Resource guard)
	{
		if (!(guard is StateMachineGuardDefinition stateMachineGuardDefinition))
		{
			if (!(guard is ExpressionGuard))
			{
				if (!(guard is StateIsActiveGuard))
				{
					if (!(guard is AllOfGuard))
					{
						if (!(guard is AnyOfGuard))
						{
							if (guard is NotGuard)
							{
								return "场景状态图 · 结果取反组合";
							}
							return "自定义状态条件";
						}
						return "场景状态图 · 任一满足组合";
					}
					return "场景状态图 · 全部满足组合";
				}
				return "场景状态图 · 状态激活条件";
			}
			return "场景状态图 · 表达式条件";
		}
		if (stateMachineGuardDefinition.Kind == StateMachineGuardKind.Callback)
		{
			return "资源状态机 · C# 回调条件";
		}
		return "资源状态机 · 属性比较条件";
	}

	private static bool? EvaluatePreview(Resource guard, Variant actual)
	{
		if (!(guard is StateMachineGuardDefinition guard2))
		{
			return null;
		}
		int nodes = 0;
		if (ContainsCallback(guard2, new HashSet<ulong>(), 0, ref nodes))
		{
			return null;
		}
		nodes = 0;
		return EvaluateRuntimeGuard(guard2, actual, new HashSet<ulong>(), 0, ref nodes);
	}

	private static bool ContainsCallback(StateMachineGuardDefinition guard, HashSet<ulong> visiting, int depth, ref int nodes)
	{
		if (!GodotObject.IsInstanceValid(guard) || depth >= 32 || ++nodes > 256)
		{
			return false;
		}
		if (guard.Kind == StateMachineGuardKind.Callback)
		{
			return true;
		}
		StateMachineGuardKind kind = guard.Kind;
		if (kind != StateMachineGuardKind.All && kind != StateMachineGuardKind.Any && kind != StateMachineGuardKind.Not)
		{
			return false;
		}
		ulong instanceId = guard.GetInstanceId();
		if (!visiting.Add(instanceId))
		{
			return false;
		}
		try
		{
			foreach (Resource child in guard.Children)
			{
				if (ContainsCallback(child as StateMachineGuardDefinition, visiting, depth + 1, ref nodes))
				{
					return true;
				}
			}
			return false;
		}
		finally
		{
			visiting.Remove(instanceId);
		}
	}

	private static bool EvaluateRuntimeGuard(StateMachineGuardDefinition guard, Variant actual, HashSet<ulong> visiting, int depth, ref int nodes)
	{
		if (!GodotObject.IsInstanceValid(guard) || depth >= 32 || ++nodes > 256)
		{
			return false;
		}
		ulong instanceId = guard.GetInstanceId();
		if (!visiting.Add(instanceId))
		{
			return false;
		}
		try
		{
			bool flag;
			switch (guard.Kind)
			{
			case StateMachineGuardKind.ExpressionProperty:
				flag = Compare(actual, guard.ExpectedValue, guard.Operator);
				break;
			case StateMachineGuardKind.All:
				flag = EvaluateAll(guard.Children, actual, visiting, depth, ref nodes);
				break;
			case StateMachineGuardKind.Any:
				flag = EvaluateAny(guard.Children, actual, visiting, depth, ref nodes);
				break;
			case StateMachineGuardKind.Not:
			{
				Array<Resource> children = guard.Children;
				flag = children != null && children.Count == 1 && !EvaluateRuntimeGuard(guard.Children[0] as StateMachineGuardDefinition, actual, visiting, depth + 1, ref nodes);
				break;
			}
			default:
				flag = false;
				break;
			}
			bool flag2 = flag;
			return guard.Negate ? (!flag2) : flag2;
		}
		finally
		{
			visiting.Remove(instanceId);
		}
	}

	private static bool EvaluateAll(Array<Resource> children, Variant actual, HashSet<ulong> visiting, int depth, ref int nodes)
	{
		if (children == null || children.Count == 0)
		{
			return false;
		}
		foreach (Resource child in children)
		{
			if (!EvaluateRuntimeGuard(child as StateMachineGuardDefinition, actual, visiting, depth + 1, ref nodes))
			{
				return false;
			}
		}
		return true;
	}

	private static bool EvaluateAny(Array<Resource> children, Variant actual, HashSet<ulong> visiting, int depth, ref int nodes)
	{
		if (children == null || children.Count == 0)
		{
			return false;
		}
		foreach (Resource child in children)
		{
			if (EvaluateRuntimeGuard(child as StateMachineGuardDefinition, actual, visiting, depth + 1, ref nodes))
			{
				return true;
			}
		}
		return false;
	}

	private static bool Compare(Variant left, Variant right, StateMachineComparisonOperator op)
	{
		if ((uint)op <= 1u)
		{
			bool flag = left.Equals(right) || string.Equals(FormatValue(left), FormatValue(right), StringComparison.Ordinal);
			if (op != StateMachineComparisonOperator.Equal)
			{
				return !flag;
			}
			return flag;
		}
		if (!TryNumber(left, out var number) || !TryNumber(right, out var number2))
		{
			return false;
		}
		return op switch
		{
			StateMachineComparisonOperator.Less => number < number2, 
			StateMachineComparisonOperator.LessOrEqual => number <= number2, 
			StateMachineComparisonOperator.Greater => number > number2, 
			StateMachineComparisonOperator.GreaterOrEqual => number >= number2, 
			_ => false, 
		};
	}

	private static bool TryNumber(Variant value, out double number)
	{
		Variant.Type variantType = value.VariantType;
		if (((ulong)(variantType - 2) <= 1uL) ? true : false)
		{
			number = value.AsDouble();
			return double.IsFinite(number);
		}
		return double.TryParse(value.AsString(), NumberStyles.Float, CultureInfo.InvariantCulture, out number);
	}

	private static string OperatorGlyph(StateMachineComparisonOperator op)
	{
		return op switch
		{
			StateMachineComparisonOperator.Equal => "等于 ＝", 
			StateMachineComparisonOperator.NotEqual => "不等于 ≠", 
			StateMachineComparisonOperator.Less => "小于 ＜", 
			StateMachineComparisonOperator.LessOrEqual => "小于等于 ≤", 
			StateMachineComparisonOperator.Greater => "大于 ＞", 
			StateMachineComparisonOperator.GreaterOrEqual => "大于等于 ≥", 
			_ => "比较", 
		};
	}

	private static string FormatValue(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		if ((ulong)variantType <= 3uL)
		{
			switch ((int)variantType)
			{
			case 0:
				return "未设置";
			case 1:
				return value.AsBool() ? "开启" : "关闭";
			case 3:
				return value.AsDouble().ToString("0.###");
			case 2:
				return value.AsInt64().ToString();
			}
		}
		return string.IsNullOrWhiteSpace(value.AsString()) ? "空文本" : value.AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Bind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "actualValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "area", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawWire, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "fill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCentered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fontSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "fill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "outline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "radius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGuardTypeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Compare, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "left", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "right", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Int, "op", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OperatorGlyph, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "op", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.Bind && args.Count == 2)
		{
			Bind(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawGrid && args.Count == 1)
		{
			DrawGrid(VariantUtils.ConvertTo<Rect2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawWire && args.Count == 3)
		{
			DrawWire(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCard && args.Count == 5)
		{
			DrawCard(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<Color>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCentered && args.Count == 4)
		{
			DrawCentered(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.GetGuardTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetGuardTypeLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.Compare && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(Compare(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<StateMachineComparisonOperator>(in args[2])));
			return true;
		}
		if (method == MethodName.OperatorGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(OperatorGlyph(VariantUtils.ConvertTo<StateMachineComparisonOperator>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.GetGuardTypeLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetGuardTypeLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.Compare && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(Compare(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<StateMachineComparisonOperator>(in args[2])));
			return true;
		}
		if (method == MethodName.OperatorGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(OperatorGlyph(VariantUtils.ConvertTo<StateMachineComparisonOperator>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatValue(VariantUtils.ConvertTo<Variant>(in args[0])));
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
		if (method == MethodName.Bind)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.DrawGrid)
		{
			return true;
		}
		if (method == MethodName.DrawWire)
		{
			return true;
		}
		if (method == MethodName.DrawCard)
		{
			return true;
		}
		if (method == MethodName.DrawCentered)
		{
			return true;
		}
		if (method == MethodName.CreatePanelStyle)
		{
			return true;
		}
		if (method == MethodName.GetGuardTypeLabel)
		{
			return true;
		}
		if (method == MethodName.Compare)
		{
			return true;
		}
		if (method == MethodName.OperatorGlyph)
		{
			return true;
		}
		if (method == MethodName.FormatValue)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._guard)
		{
			_guard = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._actualValue)
		{
			_actualValue = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._guard)
		{
			value = VariantUtils.CreateFrom(in _guard);
			return true;
		}
		if (name == PropertyName._actualValue)
		{
			value = VariantUtils.CreateFrom(in _actualValue);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._guard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName._actualValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._guard, Variant.From(in _guard));
		info.AddProperty(PropertyName._actualValue, Variant.From(in _actualValue));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._guard, out var value))
		{
			_guard = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._actualValue, out var value2))
		{
			_actualValue = value2.As<Variant>();
		}
	}
}
